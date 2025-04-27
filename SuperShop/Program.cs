using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SuperShop
{
    public class Program
    {
        public static void Main(string[] args)  //Arranca na Main
        {
            CreateHostBuilder(args).Build().Run();  //Cria um Host (permite correr a aplicação em qualquer sistema operativo)
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();  //Injeta o "Startup" (ficheiros da configuração)
                });
    }
}
