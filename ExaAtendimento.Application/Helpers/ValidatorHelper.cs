using FluentValidation;
using FluentValidation.Results;

using ExaAtendimento.Application.Exceptions;

namespace ExaAtendimento.Application.Helpers
{
    public class ValidatorHelper
    {
        public static void Validar<T>(IValidator<T> validator, T dto)
        {
            // Verifica explicitamente se o DTO é null.
            if (dto == null)
            {
                var erroDtoNull = new Dictionary<string, string[]>
                {
                    { "requestBody", new[] { "Corpo da requisição não pode ser vazio ou inválido." } }
                };
                throw new ApiValidationException(erroDtoNull);
            }

            ValidationResult result = validator.Validate(dto);
            if (result.IsValid) return;

            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            throw new ApiValidationException(errors);
        }

    }
}
