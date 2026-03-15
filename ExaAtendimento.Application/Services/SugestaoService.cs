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
    public class SugestaoService
    {
        private readonly ISugestaoRepository _sugestaoRepository;
        private readonly IValidator<SugestaoDto> _validator;
        private readonly IMapper _mapper;

        public SugestaoService(ISugestaoRepository sugestaoRepository,
                               IValidator<SugestaoDto> validator,
                               IMapper mapper) 
        {
            _sugestaoRepository = sugestaoRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task AdicionarAsync(SugestaoDto sugestaoDto)
        {
            ValidatorHelper.Validar(_validator, sugestaoDto);
            var jaExisteDescricao = await DescricaoJaExisteAsync(sugestaoDto.Descricao);
            if (jaExisteDescricao)
            {
                throw new BusinessException("Sugestão já cadastrada");
            }

            var sugestao = _mapper.Map<Sugestao>(sugestaoDto);
            await _sugestaoRepository.AdicionarAsync(sugestao);
            sugestaoDto.Id = sugestao.Id;
        }

        public async Task AtualizarAsync(SugestaoDto sugestaoDto)
        {
            var sugestao = await _sugestaoRepository.ObterPorIdAsync(sugestaoDto.Id);

            ValidatorHelper.Validar(_validator, sugestaoDto);

            _mapper.Map(sugestaoDto, sugestao);
            await _sugestaoRepository.AtualizarAsync(sugestao);
        }

        public async Task RemoverAsync(int id)
        {
            await _sugestaoRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<SugestaoDto>> ObterPorIdAsync(int id)
        {
            var sugestao = await _sugestaoRepository.ObterPorIdAsync(id);
            if (sugestao == null)
            {
                return new ResponseDto<SugestaoDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<SugestaoDto>(sugestao);
            return new ResponseDto<SugestaoDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<SugestaoDto>> ObterTodosAsync()
        {
            var sugestoes = await _sugestaoRepository.ObterTodosAsync();
            return _mapper.Map<List<SugestaoDto>>(sugestoes);
        }

        public async Task<PagedListDto<SugestaoDto>> PesquisarListaAsync(SugestaoRequest req)
        {
            var resultado = await _sugestaoRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<SugestaoDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<SugestaoDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<SugestaoDto>>(resultado.Itens);

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
            return await _sugestaoRepository.GetTotalCountAsync();
        }

        private async Task<bool> DescricaoJaExisteAsync(string descricao, int id = 0)
        {
            var sugestao = await _sugestaoRepository.ObterPorDescricaoAsync(descricao);
            if (sugestao != null)
            {
                return true;
            }
            return false;
        }
    }
}
