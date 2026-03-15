using Mapster;

using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Mappings
{
    public class MappingConfig
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {

            //Assunto
            config.NewConfig<Assunto, AssuntoDto>()
                  .Map(dest => dest.NomeModulo, src => src.Modulo != null ? src.Modulo.Nome : "");

            config.NewConfig<AssuntoDto, Assunto>();
                //.Ignore(dest => dest.Modulo!); 

            //Atendimento
            config.NewConfig<Atendimento, AtendimentoDto>()
                  .Map(dest => dest.NomeCa, src => src.Ca != null ? src.Ca.Nome : "")
                  .Map(dest => dest.NomeCliente, src => src.Cliente != null ? src.Cliente.Nome : "");
            //.Map(dest => dest.TipoAssunto, src => src.Assunto != null ? src.Assunto.TipoAssunto : "")
            //.Map(dest => dest.NomeModulo, src => src.Modulo != null ? src.Modulo.Nome : "")
            //.Map(dest => dest.DescricaoSugestao, src => src.Sugestao != null ? src.Sugestao.Descricao : "")
            //.Map(dest => dest.DescricaoTipoAtendimento, src => src.TipoAtendimento != null ? src.TipoAtendimento.Descricao : "")
            //.Map(dest => dest.NomeUsuario, src => src.Usuario != null ? src.Usuario.Nome : "");

            config.NewConfig<AtendimentoDto, Atendimento>();

            //Ca
            config.NewConfig<Ca, CaDto>();

            config.NewConfig<CaDto, Ca>();

            //Cliente
            config.NewConfig<Cliente, ClienteDto>()
                  .Map(dest => dest.NomeCa, src => src.Ca != null ? src.Ca.Nome : "")
                  .Map(dest => dest.NomeCaCompartilhada, src => src.CaCompartilhada != null ? src.CaCompartilhada.Nome : "");

            config.NewConfig<ClienteDto, Cliente>();
            
            //Modulo
            config.NewConfig<Modulo, ModuloDto>();

            config.NewConfig<ModuloDto, Modulo>();

            //Sugestao
            config.NewConfig<Sugestao, SugestaoDto>();

            config.NewConfig<SugestaoDto, Sugestao>();

            //TipoAtendimento
            config.NewConfig<TipoAtendimento, TipoAtendimentoDto>();

            config.NewConfig<TipoAtendimentoDto, TipoAtendimento>();

            //Usuario
            config.NewConfig<Usuario, UsuarioDto>()
                  .Map(dest => dest.NomeModulo, src => src.Modulo != null ? src.Modulo.Nome : null);

            config.NewConfig<Usuario, UsuarioListaDto>();

            config.NewConfig<UsuarioDto, Usuario>();
            config.NewConfig<UsuarioCriacaoDto, Usuario>();

        }
    }
}
