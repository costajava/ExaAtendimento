using FluentValidation;
using MapsterMapper;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Application.Interfaces;
using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;
using ExaAtendimento.Application.Exceptions;

namespace ExaAtendimento.Application.Services
{
    public class ModuloService
    {
        private readonly IModuloRepository _moduloRepository;
        private readonly IValidator<ModuloDto> _validator;
        private readonly IMapper _mapper;

        public ModuloService(IModuloRepository moduloRepository,
                             IValidator<ModuloDto> validator,
                             IMapper mapper)
        {
            _moduloRepository = moduloRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task AdicionarAsync(ModuloDto moduloDto)
        {
            ValidatorHelper.Validar(_validator, moduloDto);
            var jaExisteNome = await NomeJaExisteAsync(moduloDto.Nome);
            if (jaExisteNome)
            {
                throw new BusinessException("Módulo já cadastrado");
            }

            var modulo = _mapper.Map<Modulo>(moduloDto);
            await _moduloRepository.AdicionarAsync(modulo);
            moduloDto.Id = modulo.Id;
        }

        public async Task AtualizarAsync(ModuloDto moduloDto)
        {
            var modulo = await _moduloRepository.ObterPorIdAsync(moduloDto.Id);

            ValidatorHelper.Validar(_validator, moduloDto);

            _mapper.Map(moduloDto, modulo);
            await _moduloRepository.AtualizarAsync(modulo);
        }

        public async Task RemoverAsync(int id)
        {
            await _moduloRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<ModuloDto>> ObterPorIdAsync(int id)
        {
            var modulo = await _moduloRepository.ObterPorIdAsync(id);
            if (modulo == null)
            {
                return new ResponseDto<ModuloDto>(null, "Registro não encontrado (Módulo)", false);
            }

            var dto = _mapper.Map<ModuloDto>(modulo);
            return new ResponseDto<ModuloDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _moduloRepository.ExisteAsync(id);
        }

        public async Task<List<ModuloDto>> ObterTodosAsync()
        {
            var modulos = await _moduloRepository.ObterTodosAsync();
            return _mapper.Map<List<ModuloDto>>(modulos);
        }

        public async Task<PagedListDto<ModuloDto>> PesquisarListaAsync(ModuloRequest req)
        {
            var resultado = await _moduloRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<ModuloDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<ModuloDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<ModuloDto>>(resultado.Itens);

            lista.TotalCount = resultado.TotalCount;
            lista.TotalPages = lista.PageSize > 0
                ? (int)Math.Ceiling((double)lista.TotalCount / lista.PageSize)
                : 0;

            lista.Sucesso = true;
            lista.Mensagem = "Registros encontrados com sucesso.";

            return lista;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _moduloRepository.GetTotalCountAsync();
        }
        private async Task<bool> NomeJaExisteAsync(string nome, int id = 0)
        {
            var modulo = await _moduloRepository.ObterPorNomeAsync(nome);
            if (modulo != null)
            {
                return true;
            }
            return false;
        }

    }
}

