using CustomerAPI.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CustomerAPI.Repositories;

namespace CustomerAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var connectionString = Configuration["connectionStrings:EntityConnectionString"];
            services.AddDbContext<EntityContext>(option => {
                option.UseSqlServer(connectionString);
            });

            // register the repository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            AutoMapper.Mapper.Initialize(conf =>
            {
                conf.CreateMap<Entities.Customer, Model.CustomerModel>()
                    .ForMember(dest => dest.Name, sour => sour.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                    .ForMember(dest => dest.AgeOfCustomerRelation, sour => sour.MapFrom(s => s.CreatedDate.GetRelationAge()));
                conf.ValidateInlineMaps = false;
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
