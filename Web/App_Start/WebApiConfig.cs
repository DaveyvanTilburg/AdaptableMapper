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