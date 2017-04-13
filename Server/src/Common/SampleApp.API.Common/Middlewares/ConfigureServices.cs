using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using SampleApp.API.Common;
using SampleApp.Base.Entities;
using SampleApp.DataAccess.Common;
using ServiceStack.Caching;
using ServiceStack.Redis;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleApp.BusinessLogic.Common;
using SampleApp.Base.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureCommonServices
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen();

            services.ConfigureSwaggerGen(options =>
            {
                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();

                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Version = "v1",
                    Title = "Sample API",
                    Description = "REST API Access to System",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });
        }

        public static void AddCommonService(this IServiceCollection services,
            IConfigurationRoot configuration,
            string serviceType = ServiceType.BaseHost)
        {
            Configuration = configuration;
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            services.AddScoped<DatabaseContext>();
            
            services.AddCors();

        }

        public static void AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                /* right now injected IdentityUser globally and using in BaseRepository and handling there
                *config.Filters.Add(new UserStampFilterAttribute());
                */

            })

            .AddJsonOptions(opt =>
               {
                   opt.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                   opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               });
        }
        public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
                var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
                var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

                if (isAuthorized && !allowAnonymous)
                {
                    if (operation.Parameters == null)
                        operation.Parameters = new List<IParameter>();

                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "Authorization",
                        In = "header",
                        Description = "access token",
                        Required = true,
                        Type = "string"
                    });
                }
            }
        }

    }
}
