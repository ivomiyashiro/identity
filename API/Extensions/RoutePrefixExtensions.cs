using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace API.Extensions;

public static class RoutePrefixExtensions
{
    public static void UseGeneralRoutePrefix(this MvcOptions opts, string prefix)
    {
        opts.Conventions.Add(new RoutePrefixConvention(new RouteAttribute(prefix)));
    }
}

public class RoutePrefixConvention(IRouteTemplateProvider route) : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix = new AttributeRouteModel(route);

    public void Apply(ApplicationModel application)
    {
        foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
        {
            if (selector.AttributeRouteModel != null)
            {
                selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
            }
            else
            {
                selector.AttributeRouteModel = _routePrefix;
            }
        }
    }
}