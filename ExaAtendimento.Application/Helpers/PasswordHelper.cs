using Microsoft.AspNetCore.Identity;

namespace ExaAtendimento.Application.Helpers
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<object> _hasher = new();

        public static string HashPassword(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                throw new ArgumentException("Senha não pode ser nula ou vazia.");

            return _hasher.HashPassword(null, senha);
        }

        public static bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            if (string.IsNullOrEmpty(senhaHash)) return false;

            var resultado = _hasher.VerifyHashedPassword(null, senhaHash, senhaDigitada);
            return resultado == PasswordVerificationResult.Success;
        }
    }
}
