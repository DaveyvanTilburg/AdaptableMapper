using System.Linq;
using System.Web.Http;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "XPathConfiguration",
                routeTemplate: "XPath",
                defaults: new { Controller = "XPathConfiguration" }
            );

            config.Routes.MapHttpRoute(
                name: "Template",
                routeTemplate: "Template",
                defaults: new { Controller = "Template" }
            );
        }
    }
}