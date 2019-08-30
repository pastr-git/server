using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pastr.MongoDB;

namespace Pastr
{
    public class Program
    {
        public static DriverContainer Database { get; } = new DriverContainer();

        public static void Main(string[] args)
        {
            Database.LoadDatabase();
            Database.LoadCollections();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseUrls("http://0.0.0.0:5000");
    }
}
