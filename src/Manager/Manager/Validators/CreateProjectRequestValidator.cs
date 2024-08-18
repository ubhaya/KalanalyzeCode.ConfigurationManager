using FluentValidation;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

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