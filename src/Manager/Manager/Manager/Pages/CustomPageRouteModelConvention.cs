using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace KalanalyzeCode.ConfigurationManager.Ui.Pages;

public sealed class CustomPageRouteModelConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        foreach (var selector in model.Selectors)
        {
            if (selector.AttributeRouteModel is null)
                continue;
            selector.AttributeRouteModel.Template = AttributeRouteModel.CombineTemplates(
                "IdentityServer",
                selector.AttributeRouteModel.Template);
        }
    }
}