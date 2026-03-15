namespace ExaAtendimento.Application.DTOs
{
    public class PagedListDto<T>
    {
        public List<T>? Itens { get; set; }
        public string? Mensagem { get; set; }
        public bool Sucesso { get; set; }

        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
