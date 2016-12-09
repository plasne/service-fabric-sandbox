using System.Web.Http;
using Owin;
using System.Web.Http.Dispatcher;
using System.Reflection;

namespace Api
{
    public static class Startup
    {

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            // remove the Controller suffix requirement for Web API
            var suffix = typeof(DefaultHttpControllerSelector).GetField("ControllerSuffix", BindingFlags.Static | BindingFlags.Public);
            if (suffix != null) suffix.SetValue(null, string.Empty);

            // routes
            config.MapHttpAttributeRoutes();

            // host
            appBuilder.UseWebApi(config);

        }
    }
}
