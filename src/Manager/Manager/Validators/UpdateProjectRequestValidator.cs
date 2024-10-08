using FluentValidation;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

namespace KalanalyzeCode.ConfigurationManager.Ui.Validators;

internal sealed class UpdateProjectRequestValidator : MudBlazorValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .Length(1, 100);
    }
}