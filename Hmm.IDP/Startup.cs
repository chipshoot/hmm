// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Hmm.IDP.DbContexts;
using Hmm.IDP.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Hmm.IDP.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hmm.IDP
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();
            services.AddScoped<ILocalUserService, LocalUserService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            }).AddProfileService<LocalUserProfileService>();

            var connectString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            builder.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = bd =>
                    bd.UseMySQL(connectString, opts => opts.MigrationsAssembly(migrationsAssembly));
            });
            builder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = bd =>
                    bd.UseMySQL(connectString, opts => opts.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
            });

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseMySQL(connectString);
            });

            builder.AddSigningCredential(LoadCertificateFromStore());
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app);

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        // todo: will remove after we get the seed data insert SQL script
        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                dbContext.Database.Migrate();
                if (!dbContext.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        dbContext.Clients.Add(client.ToEntity());
                    }
                    dbContext.SaveChanges();
                }

                if (!dbContext.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        dbContext.IdentityResources.Add(resource.ToEntity());
                    }

                    dbContext.SaveChanges();
                }

                if (!dbContext.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                    {
                        dbContext.ApiScopes.Add(scope.ToEntity());
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public X509Certificate2 LoadCertificateFromStore()
        {
            var thumbPrint = "4608df51bbe558ec243c8b1c8947f73eb6cfc323";

            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, true);
            if (certCollection.Count == 0)
            {
                throw new Exception("The specified certificate wasn't found");
            }

            return certCollection[0];
        }
    }
}