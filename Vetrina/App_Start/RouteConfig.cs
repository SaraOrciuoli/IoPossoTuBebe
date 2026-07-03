using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vetrina
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}/{titolo}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, titolo = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "CategoriaRoute",
               url: "{idCategoria}/{nomeCategoria}/{titolo}",
               defaults: new { controller = "Ecommerce", action = "Categoria" }
           );

           
           
            
        }

    }
}
