using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;

namespace SuperShop
{
    public class Program
    {
        public static void Main(string[] args)  //Arranca na Main
        {
            //CreateHostBuilder(args).Build().Run();  //Cria um Host (permite correr a aplica��o em qualquer sistema operativo)
            var host = CreateHostBuilder(args).Build();     //Controi o host mas ainda n�o o arranca (� guardado na ver host)
            RunSeeding(host);   //Uso o "host" para correr o m�todo Seeding
            host.Run();     //S� depois � que corre o host
        }

        private static void RunSeeding(IHost host)   //Este m�todo usa o Design Pattern Factoring (antes de existir, cria-se a ele pr�prio)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();         //crio um servi�o no "host" que � passado
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();    //Crio o mecanismo para se instanciar a ele pr�prio quando for preciso
                seeder.SeedAsync().Wait();  //Tem de esperar at� ser criado
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();  //Injeta o "Startup" (ficheiros da configura��o)
                });
    }
}
