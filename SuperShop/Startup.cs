using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;

namespace SuperShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;  //N�o h� instanciamento. A "configuration" � injetada aqui
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true; //Para cada user o email deve ser �nico (n�o pode haver emails repetidos)
                cfg.Password.RequireDigit = false;  //Coloquei estes campos de password false para facilitar a cria��o de users e a testagem. Quando estiver em produ��o tenho de colocar true (para n�o ficar inseguro)
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<DataContext>();   //Depois do user fazer o login, passa a usar o DataContext simples

            //Configuro o meu Data Context
            services.AddDbContext<DataContext>(cfg =>           //Crio um servi�o de DataContext e injeto l� o meu DataContext
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));      //Estipulo que tipo de base de dados o meu servi�o vai utilizar ("UseSqlServer")  e indico a connection string ("GetConnectionString("DefaultConnection")")
            });

            services.AddTransient<SeedDb>();    //"Quando algu�m perguntar pelo SeedDb, tu vais cri�-lo".
                                                //Este service � usado uma �nica vez (usa, deita fora e o objeto desaparece e n�o pode ser mais usado).
                                                //Neste caso s� � usado quando a aplica��o arranca.

            services.AddScoped<IUserHelper, UserHelper>();

            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();

            //services.AddScoped<IRepository, Repository>();  //Quando for preciso, vai compilar o interface do reposit�rio e quando for necess�rio vai ser instanciado (injeto a classe "Repository")
            //                                                //Assim que detetar que � preciso o reposit�rio, vai instanci�-lo
            //                                                //Posso criar o objeto as vezes que quiser, mas ao criar um novo objeto vai apagar o anterior
            //                                                //Ao contr�rio do "AddSingleton", atrav�s do qual � criado um objeto que est� sempre ativo
            //                                                //(quando � necess�rio ter o mesmo objeto durante o ciclo de vida da aplica��o)

            //services.AddScoped<IRepository, MockRepository>();  //Para testar a aplica��o

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

            app.UseAuthentication();
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
