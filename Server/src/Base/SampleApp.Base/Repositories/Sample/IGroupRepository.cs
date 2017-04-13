using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Repositories
{
    public interface IGroupRepository : IDepRepository 
    {
        //Task<IGroup> AddNew(IGroup entity);

        Task<IGroup> Update(IGroup entity);

        Task Delete(long id);

        Task<IEnumerable<IGroup>> FindAll();

        Task<bool> AddNew(IEnumerable<IGroup> entities, long userid);





    }
}
