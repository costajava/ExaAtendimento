using FluentValidation;
using MapsterMapper;

using ExaAtendimento.Application.Interfaces;
using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;
using ExaAtendimento.Application.Exceptions;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;

namespace ExaAtendimento.Application.Services
{
    public class TipoAtendimentoService
    {
        private readonly ITipoAtendimentoRepository _tipoAtendimentoRepository;
        private readonly IValidator<TipoAtendimentoDto> _validator;
        private readonly IMapper _mapper;

        public TipoAtendimentoService(ITipoAtendimentoRepository tipoAtendimentoRepository,
                             IValidator<TipoAtendimentoDto> validator,
                             IMapper mapper)
        {
            _tipoAtendimentoRepository = tipoAtendimentoRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task AdicionarAsync(TipoAtendimentoDto tipoAtendimentoDto)
        {
            ValidatorHelper.Validar(_validator, tipoAtendimentoDto);
            var jaExisteDescricao = await DescricaoJaExisteAsync(tipoAtendimentoDto.Descricao);
            if (jaExisteDescricao)
            {
                throw new BusinessException("Tipo de Atendimento já cadastrado");
            }

            var tipoAtendimento = _mapper.Map<TipoAtendimento>(tipoAtendimentoDto);
            await _tipoAtendimentoRepository.AdicionarAsync(tipoAtendimento);
            tipoAtendimentoDto.Id = tipoAtendimento.Id;
        }

        public async Task AtualizarAsync(TipoAtendimentoDto tipoAtendimentoDto)
        {
            var tipoAtendimento = await _tipoAtendimentoRepository.ObterPorIdAsync(tipoAtendimentoDto.Id);

            ValidatorHelper.Validar(_validator, tipoAtendimentoDto);

            _mapper.Map(tipoAtendimentoDto, tipoAtendimento);
            await _tipoAtendimentoRepository.AtualizarAsync(tipoAtendimento);
        }

        public async Task RemoverAsync(int id)
        {
            await _tipoAtendimentoRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<TipoAtendimentoDto>> ObterPorIdAsync(int id)
        {
            var tipoAtendimento = await _tipoAtendimentoRepository.ObterPorIdAsync(id);
            if (tipoAtendimento == null)
            {
                return new ResponseDto<TipoAtendimentoDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<TipoAtendimentoDto>(tipoAtendimento);
            return new ResponseDto<TipoAtendimentoDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<TipoAtendimentoDto>> ObterTodosAsync()
        {
            var modulos = await _tipoAtendimentoRepository.ObterTodosAsync();
            return _mapper.Map<List<TipoAtendimentoDto>>(modulos);
        }

        public async Task<PagedListDto<TipoAtendimentoDto>> PesquisarListaAsync(TipoAtendimentoRequest req)
        {
            var resultado = await _tipoAtendimentoRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<TipoAtendimentoDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<TipoAtendimentoDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<TipoAtendimentoDto>>(resultado.Itens);

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
            return await _tipoAtendimentoRepository.GetTotalCountAsync();
        }

        private async Task<bool> DescricaoJaExisteAsync(string descricao, int id = 0)
        {
            var tipoAtendimento = await _tipoAtendimentoRepository.ObterPorDescricaoAsync(descricao);
            if (tipoAtendimento != null)
            {
                return true;
            }
            return false;
        }

    }
}
