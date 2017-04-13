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
    public class GroupRepository<TGroup>
        : ModuleBaseRepository<TGroup>, IGroupRepository
        where TGroup : class, IGroup, new()
    {
        public GroupRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext)
            : base(errorCodes, dbContext)
        {

        }


        public async Task<bool> AddNew(IEnumerable<IGroup> entities, long userid)
        {
            foreach (IGroup entity in entities)
            {
                TGroup tEntity = entity as TGroup;
                
                try
                {
                    this.StartTransaction();
                    var savedEntity = await base.AddNew(entity as TGroup);
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
        //public async Task<IGroup> AddNew(IGroup entity)
        //{
        //    try
        //    {
        //        TGroup tEntity = entity as TGroup;

        //        var errors = await this.ValidateEntityToAdd(tEntity);
        //        if (errors.Count() > 0)
        //        {
        //            await this.ThrowEntityException(errors);
        //        }

        //        var savedEntity = await base.AddNew(tEntity);
        //        return savedEntity;
        //    }
        //    catch (PostgresException ex)
        //    {
        //        throw new EntityUpdateException(ex);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public async Task<IGroup> Update(IGroup entity)
        {
            try
            {
                TGroup tEntity = entity as TGroup;

                var errors = await this.ValidateEntityToUpdate(tEntity);
                if (errors.Count() > 0)
                {
                    await this.ThrowEntityException(errors);
                }

                var savedEntity = await base.Update(tEntity, x => new
                {
                    x.Name

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
                await this.Connection.DeleteAllAsync<TGroup>(i => i.Id == id);
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

        public async Task<IEnumerable<IGroup>> FindAll()
        {
            var result = await this.Connection.SelectAsync<TGroup>();
            return result;
        }

        private async Task<bool> CheckDuplicate(IGroup entity)
        {
            var Group = await this.Connection.FirstOrDefaultAsync<TGroup>(i => i.Name == entity.Name && i.Id != entity.Id);
            return Group != null;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(TGroup entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(TGroup entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntity(TGroup entity)
        {
            ICollection<IValidationResult> errors = (await base.ValidateEntity(entity)).ToList();

            if (await this.CheckDuplicate(entity))
            {
                errors.Add(new ValidationCodeResult(ErrorCodes[EnumErrorCode.GroupAlreadyExist, entity.Name]));
            }
            return errors;
        }



    }
}
