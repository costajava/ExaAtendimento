using Microsoft.Extensions.Options;
using FluentValidation;
using MapsterMapper;

using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;
using ExaAtendimento.Application.Interfaces.Utils;
using ExaAtendimento.Application.Exceptions;
using ExaAtendimento.Application.Settings;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Enums;

namespace ExaAtendimento.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly IOptions<FrontendUrlsSettings> _frontendUrls;        
        private readonly ModuloService _moduloService;
        private readonly IValidator<UsuarioDto> _validator;
        private readonly IValidator<UsuarioCriacaoDto> _validatorCriacao;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                              IEmailService emailService,
                              IOptions<FrontendUrlsSettings> frontendUrls,
                              ModuloService moduloService,
                              IValidator<UsuarioDto> validator,
                              IValidator<UsuarioCriacaoDto> validatorCriacao,
                              IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _frontendUrls = frontendUrls;
            _moduloService = moduloService;
            _validator = validator;
            _validatorCriacao = validatorCriacao;
            _mapper = mapper;
        }

        public async Task<ResponseDto<UsuarioDto>> LoginAsync(UsuarioLoginDto loginDto)
        {
            var usuario = await _usuarioRepository.ObterPorNomeAsync(loginDto.Nome);
            if (usuario == null)
            {
                return new ResponseDto<UsuarioDto>(null, "Usuário/Senha inválido.", false);
            }
            bool senhaOk = PasswordHelper.VerificarSenha(loginDto.Senha, usuario.Senha);
            if (!senhaOk)
            {
                return new ResponseDto<UsuarioDto>(null, "Usuário/Senha inválido.", false);
            }

            var dto = _mapper.Map<UsuarioDto>(usuario);
            return new ResponseDto<UsuarioDto>(dto, "Usuário logado com sucesso.", true);
        }

        public async Task CriarAdminAsync(UsuarioCriacaoDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.Senha = PasswordHelper.HashPassword(usuarioDto.Senha);
            await _usuarioRepository.AdicionarAsync(usuario);
            usuarioDto.Id = usuario.Id;
        }

        public async Task AdicionarAsync(UsuarioCriacaoDto usuarioDto)
        {
            //var isSystemContext = _userContext.UsuarioId == null;
            //if (isSystemContext)
            //{
            //    throw new BusinessException("Acesso negado");
            //}
            //if (_userContext.IsAtendente)
            //{
            //    throw new BusinessException("Sem permissão para criar usuário");
            //}
            //if (_userContext.IsManager && usuarioDto.Perfil == PerfilUsuario.Administrador)
            //{
            //    throw new BusinessException("Sem permissão para criar usuário administrador");
            //}

            ValidatorHelper.Validar(_validatorCriacao, usuarioDto);
            var jaExisteNome = await NomeJaExisteAsync(usuarioDto.Nome); 
            if (jaExisteNome) 
            {
                throw new BusinessException("Já existe usuário com este nome");
            }

            var jaExisteEmail = await EmailJaExisteAsync(usuarioDto.Email);
            if (jaExisteEmail)
            {
                throw new BusinessException("Já existe usuário com este email");
            }
            
            if (usuarioDto.Perfil == PerfilUsuario.Administrador)
            {
                var jaExisteAdmin = await _usuarioRepository.ExisteAdminAsync();
                if (jaExisteAdmin)
                {
                    throw new BusinessException("Sem permissão para criar usuário administrador.");
                }
            }

            if (usuarioDto.ModuloId != null && usuarioDto.ModuloId > 0)
            {
                var existeModulo = await _moduloService.ExisteAsync((int)usuarioDto.ModuloId);
                if (!existeModulo) throw new ValidationException("Módulo não encontrato.");
            }

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.Senha = PasswordHelper.HashPassword(usuarioDto.Senha);
            await _usuarioRepository.AdicionarAsync(usuario);
            usuarioDto.Id = usuario.Id;
        }

        public async Task AtualizarAsync(UsuarioDto usuarioDto)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioDto.Id);
            if (usuario == null) 
            {
                throw new BusinessException("Usuário não encontrato.");
            }

            ValidatorHelper.Validar(_validator, usuarioDto);

            _mapper.Map(usuarioDto, usuario);
            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task RemoverAsync(int id)
        {
            await _usuarioRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<UsuarioDto>> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
            {
                return new ResponseDto<UsuarioDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<UsuarioDto>(usuario);
            return new ResponseDto<UsuarioDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<ResponseDto<UsuarioDto>> ObterPorNomeAsync(string nome)
        {
            var usuario = await _usuarioRepository.ObterPorNomeAsync(nome);
            if (usuario == null)
            {
                return new ResponseDto<UsuarioDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<UsuarioDto>(usuario);
            return new ResponseDto<UsuarioDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<ResponseDto<UsuarioDto>> ObterPorEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            if (usuario == null)
            {
                return new ResponseDto<UsuarioDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<UsuarioDto>(usuario);
            return new ResponseDto<UsuarioDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<UsuarioListaDto>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return _mapper.Map<List<UsuarioListaDto>>(usuarios);
        }

        public async Task<PagedListDto<UsuarioListaDto>> PesquisarListaAsync(UsuarioRequest req)
        {
            var resultado = await _usuarioRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<UsuarioListaDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<UsuarioListaDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<UsuarioListaDto>>(resultado.Itens);

            lista.TotalCount = resultado.TotalCount;
            lista.TotalPages = lista.PageSize > 0
                ? (int)Math.Ceiling((double)lista.TotalCount / lista.PageSize)
                : 0;

            lista.Sucesso = true;
            lista.Mensagem = "Registros encontrados com sucesso.";

            return lista;
        }

        private async Task<bool> NomeJaExisteAsync(string nome, int id = 0) 
        {
            var usuario = await _usuarioRepository.ObterPorNomeAsync(nome);
            if (usuario != null)
            {
                return true;
            }
            return false;
        }
        private async Task<bool> EmailJaExisteAsync(string email, int id = 0)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            if (usuario != null)
            {
                return true;
            }
            return false;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _usuarioRepository.GetTotalCountAsync();
        }

        public async Task TrocarSenhaAsync(TrocarSenhaDto dto)
        {
            if (dto.NovaSenha != dto.ConfirmaSenha)
                throw new BusinessException("Confirmação de senha não confere.");

            var usuario = await _usuarioRepository.ObterPorIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new BusinessException("Credenciais inválidas.");

            var senhaOk = PasswordHelper.VerificarSenha(dto.SenhaAtual, usuario.Senha);
            if (!senhaOk)
                throw new BusinessException("Credenciais inválidas.");

            usuario.Senha = PasswordHelper.HashPassword(dto.NovaSenha);
            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task SolicitarRedefinirSenhaAsync(string email) 
        {
            //Esqueci a senha
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            if (usuario == null)
            {
                return;
            }
            var random = new Random();
            var codigoSeguranca = random.Next(100000, 999999).ToString(); 
            usuario.ResetSenhaToken = codigoSeguranca;
            usuario.ResetSenhaExpiracao = DateTime.UtcNow.AddMinutes(30);

            await _usuarioRepository.AtualizarAsync(usuario);

            var resetUrlBase = _frontendUrls.Value.ResetSenha;

            var bodyHtml = $@"
                <html>
                  <body style='font-family: Arial, sans-serif;'>
                    <p><strong>Controle de Atendimento</strong></p>
                    <p><strong>Solicitação de redefinição de senha</strong></p>
                    <p>Código de segurança para redefinição da senha: <strong style='font-size: 18px;'>{codigoSeguranca}</strong></p>
                    <p style='font-size: 12px; color: red;'>
                       Este código expira em 30 minutos.
                    </p>
                  </body>
                </html>";

            var destinoEmail = new EmailDto
            {
                To = usuario.Email,
                Subject = "Solicitação de redefinição de senha",
                Body = bodyHtml
            };

            await _emailService.EnviarEmailAsync(destinoEmail);
        }

        public async Task<bool> ValidarCodigoSegurancaAsync(string email, string codigoSeguranca)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            if (usuario == null ||
                usuario.ResetSenhaToken != codigoSeguranca ||
                usuario.ResetSenhaExpiracao < DateTime.UtcNow)
            {
                throw new BusinessException("Código de segurança inválido ou expirado.");
            }
            return true;
        }

        public async Task RedefinirSenhaAsync(RedefinirSenhaDto dto)
        {
            if (dto.NovaSenha != dto.ConfirmaSenha)
                throw new BusinessException("Confirmação de senha não confere.");

            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (usuario == null ||
                usuario.ResetSenhaToken != dto.CodigoSeguranca ||
                usuario.ResetSenhaExpiracao < DateTime.UtcNow)
            {
                throw new BusinessException("Código de segurança inválido ou expirado.");
            }
            usuario.Senha = PasswordHelper.HashPassword(dto.NovaSenha);
            usuario.ResetSenhaToken = null;
            usuario.ResetSenhaExpiracao = null;
            await _usuarioRepository.AtualizarAsync(usuario);
        }

        public async Task<bool> ExisteAdminAsync()
        {
            return await _usuarioRepository.ExisteAdminAsync();
        }

    }
}
