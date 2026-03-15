using System;
using System.Collections.Generic;

namespace ExaAtendimento.Domain.Entities;

public class Assunto
{
    public int Id { get; set; }
    public string TipoAssunto { get; set; } = string.Empty;
    public int ModuloId { get; set; }
    public Modulo? Modulo { get; set; }

}
