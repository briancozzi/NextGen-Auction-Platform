﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using NextGen.BiddingPlatform.Web.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NextGen.BiddingPlatform.HostingServices;

namespace NextGen.BiddingPlatform.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<QueuedHostedService>();
                })
                .UseKestrel(opt => opt.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>();
        }
    }
}
