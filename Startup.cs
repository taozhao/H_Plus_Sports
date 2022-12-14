using H_Plus_Sports.Contracts;
using H_Plus_Sports.Models;
using H_Plus_Sports.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace H_Plus_Sports
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISalespersonRepository, SalespersonRepository>();

            services.AddDistributedRedisCache( options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisConnection");
                options.InstanceName = "master";
            });

            services.AddMvc();

            //var connection = "Server=tcp:hsportswoodruff.database.windows.net,1433;Initial Catalog=H_Plus_Sports;Persist Security Info=False;User ID=YOUR_USER_ID_HERE;Password=YOUR_PASSWORD_HERE;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var connection = "Server=tcp:localhost,1433;Initial Catalog=H_Plus_Sports;Persist Security Info=True;User ID=sa;Password==1024@Tony;";
            services.AddDbContext<H_Plus_SportsContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();

            app.UseMvc();
        }
    }
}
