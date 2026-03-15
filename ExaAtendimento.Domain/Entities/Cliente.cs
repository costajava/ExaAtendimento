using System;
using System.Collections.Generic;

namespace ExaAtendimento.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public int CaId { get; set; }

    public Ca? Ca { get; set; } 

    public int? CaCompartilhadaId { get; set; }

    public Ca? CaCompartilhada { get; set; }
}
