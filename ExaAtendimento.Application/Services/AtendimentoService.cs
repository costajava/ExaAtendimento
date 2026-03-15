using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Domain.Enums;
using FluentValidation;
using MapsterMapper;

namespace ExaAtendimento.Application.Services
{
    public class AtendimentoService
    {
        private readonly IAtendimentoRepository _atendimentoRepository;
        private readonly IValidator<AtendimentoDto> _validator;
        private readonly IMapper _mapper;

        public AtendimentoService(IAtendimentoRepository atendimentoRepository,
                                  IValidator<AtendimentoDto> validator,
                                  IMapper mapper) 
        { 
            _atendimentoRepository = atendimentoRepository;
            _validator = validator; 
            _mapper = mapper;
        }

        public async Task AdicionarAsync(AtendimentoDto atendimentoDto)
        {
            // move a data do dia para DataRegistro e o AnoRegistro
            atendimentoDto.DataRegistro = DateTime.Now.Date;
            atendimentoDto.AnoRegistro = DateTime.Now.Year;

            if (atendimentoDto.AtendimentoConcluido == StatusAtendimento.Concluido
                && atendimentoDto.HoraFinal == null) 
            {
                atendimentoDto.HoraFinal = HoraEncerramento();
            }

            ValidatorHelper.Validar(_validator, atendimentoDto);
            
            var atendimento = _mapper.Map<Atendimento>(atendimentoDto);
            await _atendimentoRepository.AdicionarAsync(atendimento);
            atendimentoDto.Id = atendimento.Id;
        }

        public async Task AtualizarAsync(AtendimentoDto atendimentoDto)
        {
            var atendimento = await _atendimentoRepository.ObterPorIdAsync(atendimentoDto.Id);

            // move a data do dia para DataRegistro e o AnoRegistro
            atendimentoDto.DataRegistro = DateTime.Now.Date;
            atendimentoDto.AnoRegistro = DateTime.Now.Year;

            if (atendimentoDto.AtendimentoConcluido == StatusAtendimento.Concluido
                && atendimentoDto.HoraFinal == null)
            {
                atendimentoDto.HoraFinal = HoraEncerramento();
            }

            ValidatorHelper.Validar(_validator, atendimentoDto);

            _mapper.Map(atendimentoDto, atendimento);
            await _atendimentoRepository.AtualizarAsync(atendimento);
        }

        public async Task RemoverAsync(int id)
        {
            await _atendimentoRepository.RemoverAsync(id);
        }

        public async Task EncerrarAync(int id) 
        {
            var atendimento = await _atendimentoRepository.ObterPorIdAsync(id);
            if (atendimento != null)
            {
                atendimento.HoraFinal = HoraEncerramento();
                atendimento.AtendimentoConcluido = StatusAtendimento.Concluido;
                await _atendimentoRepository.AtualizarAsync(atendimento);
            }
        }

        public async Task<ResponseDto<AtendimentoDto>> ObterPorIdAsync(int id)
        {
            var atendimento = await _atendimentoRepository.ObterPorIdAsync(id);
            if (atendimento == null)
            {
                return new ResponseDto<AtendimentoDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<AtendimentoDto>(atendimento);
            return new ResponseDto<AtendimentoDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<AtendimentoDto>> ObterTodosAsync()
        {
            var atendimentos = await _atendimentoRepository.ObterTodosAsync();
            return _mapper.Map<List<AtendimentoDto>>(atendimentos);
        }

        public async Task<PagedListDto<AtendimentoDto>> PesquisarListaAsync(AtendimentoRequest req)
        {
            var resultado = await _atendimentoRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<AtendimentoDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<AtendimentoDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<AtendimentoDto>>(resultado.Itens);

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
            return await _atendimentoRepository.GetTotalCountAsync();
        }

        private TimeSpan HoraEncerramento()
        {
            DateTime data = DateTime.Now;
            TimeSpan horaEncerramento = new TimeSpan(data.Hour, data.Minute, 0);
            return horaEncerramento;
        }
    }
}
