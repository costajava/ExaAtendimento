namespace ExaAtendimento.API.Helpers;

public static class ApiErrorHelper
{
    public static string GetMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Requisição inválida.",
            401 => "Não autorizado.",
            403 => "Proibido.",
            404 => "O recurso solicitado não foi encontrado.",
            405 => "Método HTTP não permitido para este endpoint.",
            500 => "Erro interno no servidor.",
            _ => "Ocorreu um erro inesperado."
        };
    }
}
