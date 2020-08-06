using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Dahab
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{Lan}",
                defaults: new { Lan = RouteParameter.Optional }
            );

            // For Return Content-Type ["application/json"]
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
