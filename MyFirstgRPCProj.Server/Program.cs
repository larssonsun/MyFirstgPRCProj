using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MyFirstgRPCProj.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // NotSupportedException: HTTP/2 over TLS is not supported on Windows 7 due to missing ALPN support
                    //配置不包含TLS的HTTP/2终结点
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(5000, a => a.Protocols =
                                Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
