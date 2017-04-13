using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Base;
using SampleApp.Base.Repositories;
using SampleApp.Base.Entities;

namespace SampleApp.API.Common
{
    
    public abstract class UnSecureRepositoryController<TRepo> : BaseRepositoryController<TRepo>
        where TRepo : class, IDepRepository
    {
        public UnSecureRepositoryController(TRepo repository) : base(repository) { }

        
    }
    
}
