using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace ClientPortal
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.

            //not boiler plate - added this for api requests (ajax)
           //  config.EnableCors();
           // config.EnableCors(new EnableCorsAttribute(Properties.Settings.Default.Cors, "", ""));
           // app.UseCors(CorsOptions.AllowAll);
            //////////////////////////////////////////////////////

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
         
            // Web API routes
            config.MapHttpAttributeRoutes();

           // config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //i added this - this is not part of the boilerplate web api template - pupose is to return json rather than xml 
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            
        }


    }
}
