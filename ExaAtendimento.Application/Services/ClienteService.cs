using FluentValidation;
using MapsterMapper;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Interfaces;
using ExaAtendimento.Domain.Queries;
using ExaAtendimento.Application.Interfaces;
using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Helpers;

namespace ExaAtendimento.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IValidator<ClienteDto> _validator;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository,
                              IValidator<ClienteDto> validator,
                              IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task AdicionarAsync(ClienteDto clienteDto)
        {
            ValidatorHelper.Validar(_validator, clienteDto);

            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _clienteRepository.AdicionarAsync(cliente);
            clienteDto.Id = cliente.Id;
        }

        public async Task AtualizarAsync(ClienteDto clienteDto)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(clienteDto.Id);

            ValidatorHelper.Validar(_validator, clienteDto);

            _mapper.Map(clienteDto, cliente);
            await _clienteRepository.AtualizarAsync(cliente);
        }

        public async Task RemoverAsync(int id)
        {
            await _clienteRepository.RemoverAsync(id);
        }

        public async Task<ResponseDto<ClienteDto>> ObterPorIdAsync(int id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            if (cliente == null)
            {
                return new ResponseDto<ClienteDto>(null, "Registro não encontrado.", false);
            }

            var dto = _mapper.Map<ClienteDto>(cliente);
            return new ResponseDto<ClienteDto>(dto, "Registro encontrado com sucesso.", true);
        }

        public async Task<List<ClienteDto>> ObterTodosAsync()
        {
            var clientes = await _clienteRepository.ObterTodosAsync();
            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<List<ClienteDto>> BuscarPorCaAsync(int caId)
        {
            var clientes = await _clienteRepository.BuscarPorCaAsync(caId);
            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<PagedListDto<ClienteDto>> PesquisarListaAsync(ClienteRequest req)
        {
            var resultado = await _clienteRepository.PesquisarListaAsync(req);

            var lista = new PagedListDto<ClienteDto>
            {
                PageIndex = req.PageNumber,
                PageSize = req.PageSize
            };

            if (resultado == null || resultado.Itens == null || !resultado.Itens.Any())
            {
                lista.Mensagem = "Nenhum registro encontrado.";
                lista.Sucesso = false;
                lista.Itens = new List<ClienteDto>();
                lista.TotalCount = 0;
                lista.TotalPages = 0;
                return lista;
            }

            lista.Itens = _mapper.Map<List<ClienteDto>>(resultado.Itens);

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
            return await _clienteRepository.GetTotalCountAsync();
        }
    }
}
