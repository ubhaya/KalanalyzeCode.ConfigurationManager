using FluentValidation;

namespace KalanalyzeCode.ConfigurationManager.Ui.Validators;

internal sealed class CreateProjectRequestValidator : MudBlazorValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .Length(1, 100);
    }
}