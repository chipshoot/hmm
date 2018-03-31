using AutoMapper;
using DomainEntity.User;
using Hmm.Contract;
using Hmm.Core.Manager;
using Hmm.Dal.Data;
using Hmm.Dal.Querys;
using Hmm.Dal.Validation;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hmm.Api
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
            var connstr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HmmDataContext>(opt => opt.UseSqlServer(connstr));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddSingleton<IDateTimeProvider, DateTimeAdapter>();
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IEntityLookup, EfEntityLookup>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddMvc();
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = EnvironmentName.Development;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}