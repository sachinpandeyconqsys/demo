using Dapper;
using Npgsql;
using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using SampleApp.Base.Repositories;
using SampleApp.BusinessLogic.Base;
using SampleApp.DataAccess;
using SampleApp.DataAccess.Common;
using SimpleStack.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.BusinessLogic.Repositories
{    
        public class UserGroupRepository<TUserGroup, TGroup>
        : ModuleBaseRepository<TUserGroup>, IUserGroupRepository
        where TUserGroup : class, IUserGroup, new()
        where TGroup : class, IGroup, new()
        
    {
            IGroupRepository _groupRepository;
            public UserGroupRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext,
                IGroupRepository groupRepository)
            : base(errorCodes, dbContext)
        {
                this._groupRepository = groupRepository;
        }

        public async Task<bool> AddNew(IEnumerable<IUserGroup> entities, long userid)
        {
            foreach (IUserGroup entity in entities)
            {
                TUserGroup tEntity = entity as TUserGroup;
                entity.UserId = userid;
                try
                {
                    this.StartTransaction();
                    var savedEntity = await base.AddNew(entity as TUserGroup);
                    this.CommitTransaction();
                }

                catch (PostgresException ex)
                {
                    throw new EntityUpdateException(ex);
                }
                catch
                {
                    throw;
                }
            }
            return true;
        }




        public async Task<IEnumerable<IUserGroup>> Get()
        {
            List<IUserGroup> users = new List<IUserGroup>();

            var result = await this.Connection.SelectAsync<TUserGroup>();
                
            //var result = await this.Connection.QueryAsync<TUserGroup>(sqlQuery);
            return result;

        }


        
       


    }
}
