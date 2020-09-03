using AutoMapper;
using Hmm.Api.Authorization;
using Hmm.Contract;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.Core.Manager;
using Hmm.Core.Manager.Validation;
using Hmm.Dal.Data;
using Hmm.Dal.DataRepository;
using Hmm.Dal.Queries;
using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.DtoEntity.Api;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
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
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
                .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                //.AddNewtonsoftJson(setupAction =>
                //{
                //    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //})
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "https://homemademessage.com/modelvalidationproblem",
                            Title = "One or more model validation errors occurred.",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "See the errors property for details.",
                            Instance = context.HttpContext.Request.Path
                        };
                        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            services.AddHttpClient(HmmApiConstants.HttpClient.Idp, client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, MustOwnGasLogHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    HmmApiConstants.Policy.MustOwnGasLog,
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.AddRequirements(new MustOwnGasLogRequirement());
                    });
            });
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = HmmConstants.HmmApiId;
                    options.LegacyAudienceValidation = true;
                });
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "https://localhost:5001";
            //        options.Audience = HmmConstants.HmmApiId;
            //        options.TokenValidationParameters.ValidateAudience = false;
            //        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            //    });
            services.AddDbContext<HmmDataContext>(opt => opt.UseSqlServer(connectString));
            services.AddSingleton<IDateTimeProvider, DateTimeAdapter>();
            services.AddScoped<IVersionRepository<HmmNote>, NoteEfRepository>();
            services.AddScoped<IHmmDataContext, HmmDataContext>();
            services.AddScoped<IEntityLookup, EfEntityLookup>();
            services.AddScoped<IGuidRepository<Author>, AuthorEfRepository>();
            services.AddScoped<IRepository<NoteRender>, NoteRenderEfRepository>();
            services.AddScoped<IRepository<NoteCatalog>, NoteCatalogEfRepository>();
            services.AddScoped<IAuthorManager, AuthorManager>();
            services.AddScoped<IHmmNoteManager, HmmNoteManager>();
            services.AddScoped<INoteRenderManager, NoteRenderManager>();
            services.AddScoped<INoteCatalogManager, NoteCatalogManager>();
            services.AddScoped<IAutoEntityManager<GasLog>, GasLogManager>();
            services.AddScoped<IAutoEntityManager<Automobile>, AutomobileManager>();
            services.AddScoped<IAutoEntityManager<GasDiscount>, DiscountManager>();
            services.AddScoped<AuthorValidator, AuthorValidator>();
            services.AddScoped<NoteValidator, NoteValidator>();
            services.AddScoped<NoteRenderValidator, NoteRenderValidator>();
            services.AddScoped<NoteCatalogValidator, NoteCatalogValidator>();
            //services.AddMvc();
            services.AddAutoMapper(typeof(ApiEntity));
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}