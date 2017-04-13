using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Repositories
{
    public interface IUserGroupRepository : IDepRepository
    {
        Task<bool> AddNew(IEnumerable<IUserGroup> entities, long userid);



        Task<IEnumerable<IUserGroup>> Get();
       


    }
}
