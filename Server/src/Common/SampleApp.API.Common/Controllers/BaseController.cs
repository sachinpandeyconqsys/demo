using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Base.Repositories;
using SampleApp.Base.Entities;

namespace SampleApp.API.Common
{
    public abstract class BaseRepositoryController<TRepo> : Controller where TRepo : class, IDepRepository
    {
        public TRepo Repository { get; set; }
        public BaseRepositoryController(TRepo repository)
        {
            this.Repository = repository;
        }

        [HttpGet("{id}")]
        public virtual async Task<object> Get(long id)
        {
            return await this.Repository.FindById(id);
        }



    }
}
