using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Repositories
{
    public interface IUserRepository : IDepRepository
    {
        Task<IUser> AddNew(IUser entity);

        Task<IUser> Update(IUser entity);

        Task Delete(long id);
            
        Task<IEnumerable<CustomDDO>> FindAllDDO();

        Task<IEnumerable<IUser>> GetActiveUsers();
        Task<IEnumerable<IUser>> GetUserById(long id);

        Task<IEnumerable<IUser>> GetUserByFn(long id);
        Task<IEnumerable<IUser>> GetActUsers();


    }
}
