namespace ExaAtendimento.Application.DTOs
{
    public class ResponseDto<T>
    {
        public T? Dados { get; set; }
        public string? Mensagem { get; set; }
        public bool Sucesso { get; set; }

        public ResponseDto(T? dados, string? mensagem = null, bool sucesso = true)
        {
            Dados = dados;
            Mensagem = mensagem;
            Sucesso = sucesso;
        }
    }

}
