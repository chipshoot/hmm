﻿using AutoMapper;
using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.Core.Manager;
using Hmm.Core.Manager.Validation;
using Hmm.Dal.Data;
using Hmm.Dal.Queries;
using Hmm.Dal.Storage;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VehicleInfoManager.GasLogMan;

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
            var connectString = Configuration.GetConnectionString("DefaultConnection");
            services.AddControllers();
            services.AddDbContext<HmmDataContext>(opt => opt.UseSqlServer(connectString));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddSingleton<IDateTimeProvider, DateTimeAdapter>();
            services.AddScoped<IDataStore<HmmNote>, NoteStorage>();
            services.AddScoped<IHmmDataContext, HmmDataContext>();
            services.AddScoped<IEntityLookup, EfEntityLookup>();
            services.AddScoped<IDataStore<User>, UserStorage>();
            services.AddScoped<IDataStore<NoteRender>, NoteRenderStorage>();
            services.AddScoped<IDataStore<NoteCatalog>, NoteCatalogStorage>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IHmmNoteManager<HmmNote>, HmmNoteManager>();
            services.AddScoped<INoteRenderManager, NoteRenderManager>();
            services.AddScoped<INoteCatalogManager, NoteCatalogManager>();
            services.AddScoped<IAutoEntityManager<GasLog>, GasLogManager>();
            services.AddScoped<IAutoEntityManager<Automobile>, AutomobileManager>();
            services.AddScoped<IAutoEntityManager<GasDiscount>, DiscountManager>();
            services.AddScoped<UserValidator, UserValidator>();
            services.AddScoped<NoteValidator, NoteValidator>();
            services.AddScoped<NoteRenderValidator, NoteRenderValidator>();
            services.AddScoped<NoteCatalogValidator, NoteCatalogValidator>();
            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}