using System.Web.Http;
using System.Web.Http.Cors;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET,POST");
            config.EnableCors(cors);

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

            config.Routes.MapHttpRoute(
                name: "DeserializeExample",
                routeTemplate: "DeserializeExample",
                defaults: new { Controller = "DeserializeExample" }
            );

            config.Routes.MapHttpRoute(
                name: "SerializeExample",
                routeTemplate: "SerializeExample",
                defaults: new { Controller = "SerializeExample" }
            );
        }
    }
}