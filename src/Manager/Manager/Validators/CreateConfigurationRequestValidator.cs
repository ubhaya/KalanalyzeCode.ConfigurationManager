using FluentValidation;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;

namespace KalanalyzeCode.ConfigurationManager.Ui.Validators;

internal class CreateConfigurationRequestValidator : MudBlazorValidator<CreateConfigurationRequest>
{
    public CreateConfigurationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 100);
        
        RuleFor(x => x.Value)
            .NotEmpty()
            .Length(1, 100);
    }
}