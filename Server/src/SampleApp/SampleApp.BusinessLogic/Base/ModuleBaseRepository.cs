using SampleApp.Base.Entities;
using SampleApp.BusinessLogic.Common;
using SampleApp.DataAccess;
using SampleApp.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.BusinessLogic.Base
{
    public class ModuleBaseRepository<T> : BaseRepository<T> where T : class, IEntity
    {
        public ModuleBaseRepository(BaseValidationErrorCodes errorCodes, DatabaseContext dbContext) 
            : base(errorCodes, dbContext)
        {

        }

        public new ValidationErrorCodes ErrorCodes
        {
            get
            {
                return base.ErrorCodes as ValidationErrorCodes;
            }
        }

        public override async Task<bool> Exists(string description)
        {
            return false;
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToAdd(T entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntityToUpdate(T entity)
        {
            return await this.ValidateEntity(entity);
        }

        public override async Task<IEnumerable<IValidationResult>> ValidateEntity(T entity)
        {
            return await base.ValidateEntity(entity);
        }
    }
}
