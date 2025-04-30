using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;

namespace SuperShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;  //Não há instanciamento. A "configuration" é injetada aqui
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configuro o meu Data Context
            services.AddDbContext<DataContext>(cfg =>           //Crio um serviço de DataContext e injeto lá o meu DataContext
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));      //Estipulo que tipo de base de dados o meu serviço vai utilizar ("UseSqlServer")  e indico a connection string ("GetConnectionString("DefaultConnection")")
            });

            services.AddTransient<SeedDb>();    //"Quando alguém perguntar pelo SeedDb, tu vais criá-lo".
                                                //Este service é usado uma única vez (usa, deita fora e o objeto desaparece e não pode ser mais usado).
                                                //Neste caso só é usado quando a aplicação arranca.

            //services.AddScoped<IRepository, Repository>();  //Quando for preciso, vai compilar o interface do repositório e quando for necessário vai ser instanciado (injeto a classe "Repository")
            //                                                //Assim que detetar que é preciso o repositório, vai instanciá-lo
            //                                                //Posso criar o objeto as vezes que quiser, mas ao criar um um novo objeto vai apagar o anterior
            //                                                //Ao contrário do "AddSingleton", através do qual é criado um objeto que está sempre ativo
            //                                                //(quando é necessário ter o mesmo objeto durante o ciclo de vida da aplicação)

            //services.AddScoped<IRepository, MockRepository>();  //Para testar a aplicação

            services.AddScoped<IProductRepository, ProductRepository>();


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"); //Digo como se vai manipular
            });
        }
    }
}
