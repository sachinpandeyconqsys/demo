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
    public class UserRepository<TUser, TGroup, TRole, TUserGroup>
    : ModuleBaseRepository<TUser>, IUserRepository
    where TUser : class, IUser, new()
    where TGroup : class, IGroup, new()
    where TRole : class, IRole, new()
    where TUserGroup : class, IUserGroup, new()
    {
        IGroupRepository _groupRepository;
        IUserGroupRepository _userGroupRepository;
        public UserRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext,
            IGroupRepository groupRepository,
            IUserGroupRepository userGroupRepository)
        : base(errorCodes, dbContext)
        {
            this._groupRepository = groupRepository;
            this._userGroupRepository = userGroupRepository;
        }

        public async Task<IUser> AddNew(IUser entity)
        {
            try
            {
                TUser tEntity = entity as TUser;

                var errors = await this.ValidateEntityToAdd(tEntity);
                if (errors.Count() > 0)
                {
                    await this.ThrowEntityException(errors);
                }

                var savedEntity = await base.AddNew(tEntity);

               // await this._userGroupRepository.AddNew(entity, savedEntity.Id);
                return savedEntity;
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

        public async Task<IUser> Update(IUser entity)
        {
            try
            {
                TUser tEntity = entity as TUser;

                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                {
                    await this.ThrowEntityException(errors);
                }

                var savedEntity = await base.Update(tEntity, x => new
                {
                    x.Name,
                    x.ModifiedBy,
                    x.ModifiedDate
                });
                return entity;
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

        public async Task Delete(long id)
        {
            try
            {
                await this.Connection.DeleteAllAsync<TUser>(i => i.Id == id);
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

        public async Task<IEnumerable<IUser>> GetActiveUsers()
        {
            List<IUser> users = new List<IUser>();

            return (await this.Connection.SelectAsync<TUser>()).Where(i => i.IsActive == true).OrderByDescending(i => i.Id);

        }

        public async Task<IEnumerable<IUser>> GetActUsers()
        {
            List<IUser> users = new List<IUser>();

            var sqlQuery = new JoinSqlBuilder<TUser, TGroup>(this.Connection.DialectProvider)
                                    .Join<TGroup, TUser>(i => i.Id, i => i.GroupId)
                                    .Join<TRole, TUser>(i => i.Id, i => i.RoleId)
                                    .SelectAll<TUser>()
                                    .Select<TRole>(p => new { RoleName = p.Name })
                                     .Select<TGroup>(g => new { GroupName = g.Name })
                                          .ToSql();
            var result = await this.Connection.QueryAsync<TUser>(sqlQuery);
            return result;

        }




        public async Task<IEnumerable<IUser>> GetUserById(long id)
        {

            var sqlQuery = new JoinSqlBuilder<TUser, TGroup>(this.Connection.DialectProvider)
                                    .Join<TGroup, TUser>(i => i.Id, i => i.GroupId)
                                    .Join<TRole, TUser>(i => i.Id, i => i.RoleId)
                                    .SelectAll<TUser>()
                                 .Select<TRole>(p => new { RoleName = p.Name })
                                 .Select<TGroup>(g => new { GroupName = g.Name })
                                          .ToSql();

            var result = await this.Connection.QueryAsync<TUser>(sqlQuery, new { p_0 = id });
            return result;
        }


        public async Task<IEnumerable<IUser>> GetUserByFn(long id)
        {
            var result = await base.FindByFunctionAsync<TUser>("get_user_by_id", id);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<CustomDDO>> FindAllDDO()
        {
            var expression = this.Connection.DialectProvider.ExpressionVisitor<TUser>().Select(i => new { i.Id, i.Name });

            return await this.Connection.QueryAsync<CustomDDO>(expression.SelectExpression);
        }

        private async Task<bool> CheckDuplicate(IUser entity)
        {
            var user = await this.Connection.FirstOrDefaultAsync<TUser>(i => i.Name == entity.Name && i.Id != entity.Id);
            return user != null;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TUser entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TUser entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntity(TUser entity)
        {
            ICollection<IValidationResult> errors = (await base.ValidateEntity(entity)).ToList();

            if (await this.CheckDuplicate(entity))
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.UserAlreadyExist, entity.Name]));
            }
            return errors;
        }


    }
}
