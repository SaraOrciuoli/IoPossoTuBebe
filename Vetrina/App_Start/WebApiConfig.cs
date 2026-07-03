using Vetrina.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

//using System.Web.Http;

namespace Vetrina
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new BasicAuthenticationAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

