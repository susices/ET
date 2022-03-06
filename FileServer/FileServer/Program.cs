using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace FileServer;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length<2)
        {
            Console.WriteLine("Args Error! args0: url args1: path");
            return;
        }

        var url = args[0];
        var path = args[1];
        
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(path)
            .UseWebRoot(path)
            .UseUrls(url)
            .UseStartup<Startup>()
            .Build();

        host.Run();
    }
}

internal class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole(LogLevel.Information);
    
        var fileServerOptions = new FileServerOptions();
        fileServerOptions.EnableDefaultFiles = true;
        fileServerOptions.EnableDirectoryBrowsing = true;
        fileServerOptions.FileProvider = env.WebRootFileProvider;
        fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;
        app.UseFileServer(fileServerOptions);
    }
}