
using SampleApp.DataAccess.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

using System.Data;

using SimpleStack.Orm;
using SampleApp.Base.Entities;
using System.Linq.Expressions;
using Dapper;
using System.Collections;
using SampleApp.Base.Repositories;

namespace SampleApp.BusinessLogic.Common
{
    public abstract class BaseRepository<T> : IRepository<T>, IDepRepository
        where T : class, IEntity
    {
        private string _connectionString;

        private DatabaseContext _dbContext;
        public DatabaseContext DbContext
        {
            get { return _dbContext; }
            private set { _dbContext = value; }
        }
        
        protected OrmConnection Connection
        {
            get
            {
                return this._dbContext.Connection;
            }
        }
        
        //public IUser LoggedUser { get; private set; }

        public BaseRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext)
        {
            this.ErrorCodes = errorCodes; // (BaseValidationErrorCodes)ServiceProvider.GetService(typeof(BaseValidationErrorCodes));
            this.DbContext = dbContext; // (DatabaseContext)ServiceProvider.GetService(typeof(DatabaseContext));
            //this.LoggedUser = loggedUser; // (IUser)ServiceProvider.GetService(typeof(IUser));
        }

        public virtual BaseValidationErrorCodes ErrorCodes
        {
            get; protected set;
        }

        public bool TransInitialized { get; private set; }

        public IDbTransaction CurrentTransaction { get; private set; }
        
        public string StartTransaction(bool matchTransactionId = false)
        {
            return this.DbContext.BeginTransaction(matchTransactionId);
        }

        public void CommitTransaction(string transactionId = null)
        {
            this.DbContext.CommitTransaction(transactionId);
        }

        public async Task<IEnumerable<T1>> FindByFunctionAsync<T1>(string functionName, params object[] parameters) 
        {
            var t = await this.DbContext.FunctionAsync<T1>(functionName, parameters);
            return t;
        }
        
        public virtual async Task<object> FindById(long id)
        {
            var t = await this.Connection.FirstOrDefaultAsync<T>(i => i.Id == id);
            return t;
        }

        public virtual async Task<T> AddNew(T entity)
        {
            this.SetUserStamp(entity, false);
            
            long newId = await this.DbContext.InsertAsync<long, T>(entity);
            entity = await this.Connection.FirstOrDefaultAsync<T>(i => i.Id == newId);
            return entity;
        }

        public abstract Task<bool> Exists(string name);

        private void SetUpdateFields(T entity)
        {
            this.SetUserStamp(entity, true);
        }

        public virtual async Task<bool> Update(T entity)
        {
            this.SetUpdateFields(entity);
            await this.Connection.UpdateAsync<T>(entity);
            return true;
        }

        public virtual async Task<bool> Update<TKey>(T entity, Expression<Func<T, TKey>> onlyFields)
        {
            this.SetUpdateFields(entity);
            await this.Connection.UpdateAsync(entity, onlyFields);
            return true;
        }

        public abstract Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(T entity);

        public virtual Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(T entity)
        {
            return Task.FromResult<IEnumerable<IValidationResult>>(new List<IValidationResult>());
        }
        
        public virtual async Task<IEnumerable<IValidationResult>> ValidateEntity(T entity)
        {
            IList<ValidationResult> validations = new List<ValidationResult>();
            Validator.TryValidateObject(entity, new ValidationContext(entity), validations, true);

            var errors = (from x in validations
                          select new ValidationCodeResult(x.ErrorMessage, x.MemberNames)).ToList<IValidationResult>();

            return errors;
        }

        protected Task ThrowEntityException(IEnumerable<IValidationResult> validationErrors, string message = "Record rejected due to following errors -")
        {
            throw new EntityValidationException(message, validationErrors);
        }

        protected Task ThrowEntityException(ValidationCodeResult validationError, string message = "Record rejected due to following errors -")
        {
            throw new EntityValidationException(message, new List<IValidationResult>() { validationError });
        }

        private void SetUserStamp(T entity, bool isUpdate)
        {
            if (entity is IStamp)
            {
                IStamp stamp = entity as IStamp;
                if (!isUpdate)
                {
                    stamp.CreatedBy = 0; // LoggedUser.Id;
                    stamp.CreatedDate = DateTime.Now;
                }
                stamp.ModifiedBy = 0; // LoggedUser.Id;
                stamp.ModifiedDate = DateTime.Now;
            }
            else if (entity is ICreatedStamp && !isUpdate)
            {
                ICreatedStamp stamp = entity as ICreatedStamp;

                stamp.CreatedBy = 0; // LoggedUser.Id;
                stamp.CreatedDate = DateTime.Now;
            }
        }
        
    }
}

