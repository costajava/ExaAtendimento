using System;
using System.Collections.Generic;

namespace ExaAtendimento.Domain.Entities;

public class Sugestao
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;
}
