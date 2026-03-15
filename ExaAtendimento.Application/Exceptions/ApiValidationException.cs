namespace ExaAtendimento.Application.Exceptions
{
    public class ApiValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ApiValidationException(IDictionary<string, string[]> errors)
            : base("Erros de validação")
        {
            Errors = errors;
        }
    }
}
