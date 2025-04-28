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
            //CreateHostBuilder(args).Build().Run();  //Cria um Host (permite correr a aplicação em qualquer sistema operativo)
            var host = CreateHostBuilder(args).Build();     //Controi o host mas ainda não o arranca (é guardado na ver host)
            RunSeeding(host);   //Uso o "host" para correr o método Seeding
            host.Run();     //Só depois é que corre o host
        }

        private static void RunSeeding(IHost host)   //Este método usa o Design Pattern Factoring (antes de existir, cria-se a ele próprio)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();         //crio um serviço no "host" que é passado
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();    //Crio o mecanismo para se instanciar a ele próprio quando for preciso
                seeder.SeedAsync().Wait();  //Tem de esperar até ser criado
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();  //Injeta o "Startup" (ficheiros da configuração)
                });
    }
}
