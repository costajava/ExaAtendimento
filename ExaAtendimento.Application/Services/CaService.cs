using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Queries;
using FluentValidation;
using MapsterMapper;

namespace ExaAtendimento.Application.Services
{
    public class CaService
    {
        private readonly ICaRepository _caRepository;
        private readonly IValidator<CaDto> _validator;
        private readonly IMapper _mapper;

        public CaService(ICaRepository caRepository,
                         IValidator<CaDto> validator,
                         IMapper mapper)
        {
            _caRepository = caRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task AdicionarAsync(CaDto caDto)
        {
            ValidatorHelper.Validar(_validator, caDto);

            var ca = _mapper.Map<Ca>(caDto);
            await _caRepository.AdicionarAsync(ca);
            caDto.Id = ca.Id;
        }

        public async Task AtualizarAsync(CaDto caDto)
        {
            var ca = await _caRepository.ObterPorIdAsync(caDto.Id);

            ValidatorHelper.Validar(_validator, caDto);

            _mapper.Map(caDto, ca);
            await _caRepository.AtualizarAsync(ca);
        }

        public async Task RemoverAsync(int id)
        {
            await _caRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<CaDto>> ObterPorIdAsync(int id)
        {
            var ca = await _caRepository.ObterPorIdAsync(id);
            if (ca == null)
            {
                return new ResponseDto<CaDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<CaDto>(ca);
            return new ResponseDto<CaDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<CaDto>> ObterTodosAsync()
        {
            var cas = await _caRepository.ObterTodosAsync();
            return _mapper.Map<List<CaDto>>(cas);
        }

        public async Task<PagedListDto<CaDto>> PesquisarListaAsync(CaRequest req)
        {
            var resultado = await _caRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<CaDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<CaDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<CaDto>>(resultado.Itens);

            lista.TotalCount = resultado.TotalCount;
            lista.TotalPages = lista.PageSize > 0
                ? (int)Math.Ceiling((double)lista.TotalCount / lista.PageSize)
                : 0;

            lista.Sucesso = true;
            lista.Mensagem = "Registros encontrados com sucesso.";

            return lista;
        }

        public async Task<List<Ca>> BuscarPorNomeAsync(string nome) 
        {
            var cas = await _caRepository.BuscarPorNomeAsync(nome);
            return cas;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _caRepository.GetTotalCountAsync();
        }
    }
}
