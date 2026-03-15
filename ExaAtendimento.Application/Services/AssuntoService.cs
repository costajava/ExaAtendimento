using FluentValidation;
using MapsterMapper;

using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;
using ExaAtendimento.Application.Exceptions;
using ExaAtendimento.Application.Interfaces;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;

namespace ExaAtendimento.Application.Services
{
    public class AssuntoService
    {
        private readonly IAssuntoRepository _assuntoRepository;
        private readonly IValidator<AssuntoDto> _validator;
        private readonly IMapper _mapper;

        public AssuntoService(IAssuntoRepository assuntoRepository,
                              IValidator<AssuntoDto> validator,
                              IMapper mapper) 
        { 
            _assuntoRepository = assuntoRepository;
            _validator = validator; 
            _mapper = mapper;
        }

        public async Task AdicionarAsync(AssuntoDto assuntoDto)
        {
            ValidatorHelper.Validar(_validator, assuntoDto);

            var jaExisteTipoAssunto = await AssuntoJaExisteAsync(assuntoDto.TipoAssunto);
            if (jaExisteTipoAssunto)
            {
                throw new BusinessException("Tipo de assunto já cadastrado");
            }

            var assunto = _mapper.Map<Assunto>(assuntoDto);
            await _assuntoRepository.AdicionarAsync(assunto);
            assuntoDto.Id = assunto.Id;
        }

        public async Task AtualizarAsync(AssuntoDto assuntoDto)
        {
            var assunto = await _assuntoRepository.ObterPorIdAsync(assuntoDto.Id);

            ValidatorHelper.Validar(_validator, assuntoDto);
            _mapper.Map(assuntoDto, assunto);
            await _assuntoRepository.AtualizarAsync(assunto);
        }

        public async Task RemoverAsync(int id)
        {
            await _assuntoRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<AssuntoDto>> ObterPorIdAsync(int id)
        {
            var assunto = await _assuntoRepository.ObterPorIdAsync(id);

            if (assunto == null)
            {
                return new ResponseDto<AssuntoDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<AssuntoDto>(assunto);
            return new ResponseDto<AssuntoDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<AssuntoDto>> ObterTodosAsync()
        {
            var assuntos = await _assuntoRepository.ObterTodosAsync();
            return _mapper.Map<List<AssuntoDto>>(assuntos);
        }

        public async Task<PagedListDto<AssuntoDto>> PesquisarListaAsync(AssuntoRequest req)
        {
            var resultado = await _assuntoRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<AssuntoDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<AssuntoDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<AssuntoDto>>(resultado.Itens);

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
            return await _assuntoRepository.GetTotalCountAsync();
        }

        private async Task<bool> AssuntoJaExisteAsync(string tipoAssunto, int id = 0)
        {
            var assunto = await _assuntoRepository.ObterPorTipoAssuntoAsync(tipoAssunto);
            if (assunto != null)
            {
                return true;
            }
            return false;
        }

    }

}
