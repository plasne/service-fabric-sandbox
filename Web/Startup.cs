using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Web
{
    public static class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {

            HttpConfiguration config = new HttpConfiguration();

            PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem(@".\wwwroot");

            FileServerOptions fileOptions = new FileServerOptions();
            fileOptions.EnableDefaultFiles = true;
            fileOptions.RequestPath = PathString.Empty;
            fileOptions.FileSystem = physicalFileSystem;
            fileOptions.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            fileOptions.StaticFileOptions.FileSystem = fileOptions.FileSystem = physicalFileSystem;
            fileOptions.StaticFileOptions.ServeUnknownFileTypes = true;
            fileOptions.EnableDirectoryBrowsing = true;

            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);
            appBuilder.UseFileServer(fileOptions);

        }
    }
}
