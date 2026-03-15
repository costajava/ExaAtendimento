
namespace ExaAtendimento.Domain.Helpers
{
    public class PagedList<T>
    {
        public List<T>? Itens { get; set; }
        public int TotalCount { get; set; }
    }
}
