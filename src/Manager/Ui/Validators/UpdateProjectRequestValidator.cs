using FluentValidation;

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