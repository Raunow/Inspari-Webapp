using Inspari_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/*
 * Create a C# webapp. Functionality should include:

 * List table from SQL database (4-6 column data)

 * Implement paging and editing using javascript

 * Use dapper extract and save data.
 */


namespace Inspari_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHost(args).Run();
        }

        public static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();
    }
}
