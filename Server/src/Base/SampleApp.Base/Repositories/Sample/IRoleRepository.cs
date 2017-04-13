using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Repositories
{
    public interface IRoleRepository : IDepRepository
    {
        Task<IRole> AddNew(IRole entity);

        Task<IRole> Update(IRole entity);

        Task Delete(long id);

        Task<IEnumerable<IRole>> FindAll();

        Task<IEnumerable<CustomDDO>> FindAllDDO();


    }
}
