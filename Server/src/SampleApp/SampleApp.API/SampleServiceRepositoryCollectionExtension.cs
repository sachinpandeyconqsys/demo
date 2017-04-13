using SampleApp.Base.Repositories;
using SampleApp.BusinessLogic.Repositories;
using SampleApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SampleServiceRepositoryCollectionExtension
    {
        public static IServiceCollection AddSampleServiceRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository<Role>>();
            services.AddScoped<IUserRepository, UserRepository<User, Group, Role, UserGroup>>();
            services.AddScoped<IGroupRepository, GroupRepository<Group>>();

            return services;
        }
    }
}
