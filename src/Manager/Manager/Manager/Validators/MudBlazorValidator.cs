using FluentValidation;

namespace KalanalyzeCode.ConfigurationManager.Ui.Validators;

internal abstract class MudBlazorValidator<T> : AbstractValidator<T>
{
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => 
        async (model, propertyName) =>
        {
            var result =
                await ValidateAsync(ValidationContext<T>
                    .CreateWithOptions((T)model,
                        x => x.IncludeProperties(propertyName)));
            return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
        };
}