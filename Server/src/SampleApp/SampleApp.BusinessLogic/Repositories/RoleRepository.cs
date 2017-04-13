using Dapper;
using Npgsql;
using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using SampleApp.Base.Repositories;
using SampleApp.BusinessLogic.Base;
using SampleApp.DataAccess;
using SampleApp.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.BusinessLogic.Repositories
{
    public class RoleRepository<TRole> 
        : ModuleBaseRepository<TRole>, IRoleRepository
        where TRole : class, IRole, new()
    {
        public RoleRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext)
            : base(errorCodes, dbContext)
        {

        }

        public async Task<IRole> AddNew(IRole entity)
        {
            try
            {
                TRole tEntity = entity as TRole;

                var errors = await this.ValidateEntityToAdd(tEntity);
                if (errors.Count() > 0)
                {
                    await this.ThrowEntityException(errors);
                }

                var savedEntity = await base.AddNew(tEntity);
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

        public async Task<IRole> Update(IRole entity)
        {
            try
            {
                TRole tEntity = entity as TRole;

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
                await this.Connection.DeleteAllAsync<TRole>(i => i.Id == id);
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

        public async Task<IEnumerable<IRole>> FindAll()
        {
            return await this.Connection.SelectAsync<TRole>();
        }

        public async Task<IEnumerable<CustomDDO>> FindAllDDO()
        {
            var expression = this.Connection.DialectProvider.ExpressionVisitor<TRole>().Select(i => new { i.Id, i.Name });

            return await this.Connection.QueryAsync<CustomDDO>(expression.SelectExpression);
        }

        private async Task<bool> CheckDuplicate(IRole entity)
        {
            var role = await this.Connection.FirstOrDefaultAsync<TRole>(i => i.Name == entity.Name && i.Id != entity.Id);
            return role != null;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TRole entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TRole entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntity(TRole entity)
        {
            ICollection<IValidationResult> errors = (await base.ValidateEntity(entity)).ToList();

            if (await this.CheckDuplicate(entity))
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.RoleAlreadyExist, entity.Name]));
            }
            return errors;
        }
    }
}
