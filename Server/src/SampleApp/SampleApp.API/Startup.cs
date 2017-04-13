using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleApp.BusinessLogic.Common;
using SampleApp.API.Common;
using SampleApp.DataAccess.Common;
using SampleApp.DataAccess;
using Swashbuckle.Swagger.Model;

namespace sampleAppNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddSingleton<EncryptionProvider>();
            services.AddSingleton<CodeGenerator>();
            services.AddCommonService(Configuration, ServiceType.Security);
            services.AddSingleton<BaseValidationErrorCodes, ValidationErrorCodes>();

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.OperationFilter<ConfigureCommonServices.AuthorizationHeaderParameterOperationFilter>();
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Sample API",
                    Description = "REST API Access to System",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });

            services.AddSampleServiceRepositories();

            services.AddCustomMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Console.WriteLine("Starting...");

            //var cacheScheduler = app.ApplicationServices.GetService<UserCacheScheduler>();
            //cacheScheduler.ScheduleCache(message =>
            //{
            //    Console.WriteLine(message);
            //});

            app.UseCors(builder =>
               builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
               );
            
            app.UseSwagger();
            app.UseSwaggerUi();

            //app.UseJwtAuthentication();

            //app.UseSimpleTokenProvider(new TokenProviderOptions
            //{
            //    Path = "/token",
            //    Audience = Configuration["ValidAudience"],
            //    Issuer = Configuration["ValidIssuer"],
            //    SigningCredentials = new SigningCredentials(JWTService.SigningKey, SecurityAlgorithms.HmacSha256),
            //    IdentityResolver = app.ApplicationServices.GetService<IdentityResolver>().CheckUserLogin,
            //    Expiration = DateTime.Now.AddDays(7).TimeOfDay
            //});

            // app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "index");
                //routes.MapRoute(
                //    name: "resetPassword",
                //    template: "resetpassword/token",
                //    defaults: new { controller = "ResetPassword", action = "GetUserDetail" });
            });

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Add("index.html");

            app.UseDefaultFiles(options);

            app.UseStaticFiles();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        //    loggerFactory.AddDebug();

        //    app.UseApplicationInsightsRequestTelemetry();

        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //        app.UseBrowserLink();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //    }

        //    app.UseApplicationInsightsExceptionTelemetry();

        //    app.UseStaticFiles();

        //    app.UseMvc(routes =>
        //    {
        //        routes.MapRoute(
        //            name: "default",
        //            template: "{controller=Home}/{action=Index}/{id?}");
        //    });
        //}
    }
}
