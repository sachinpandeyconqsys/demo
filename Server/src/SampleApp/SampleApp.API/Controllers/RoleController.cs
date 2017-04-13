using Microsoft.AspNetCore.Mvc;
using SampleApp.API.Common;
using SampleApp.Base.Entities;
using SampleApp.Base.Objects;
using SampleApp.Base.Repositories;
using SampleApp.DataAccess;
using SampleApp.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.API.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : UnSecureRepositoryController<IRoleRepository> 
    {
        public RoleController(IRoleRepository repository) : base(repository)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddFacility([FromBody]Role entity)
        {
            try
            {
                var savedEntity = await this.Repository.AddNew(entity);
                return Ok(savedEntity);
            }
            catch (SampleException ex)
            {
                return StatusCode(400, ex.ValidationCodeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFacility([FromBody]Role entity)
        {
            try
            {
                var updatedEntity = await this.Repository.Update(entity);

                return Ok(updatedEntity);
            }
            catch (SampleException ex)
            {
                return StatusCode(400, ex.ValidationCodeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<IEnumerable<IRole>> FindAll()
        {
            return await this.Repository.FindAll();
        }

        [HttpGet("ddo")]
        public async Task<IEnumerable<CustomDDO>> FindAllDDO()
        {
            return await this.Repository.FindAllDDO();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await this.Repository.Delete(id);
                return Ok();
            }
            catch (SampleException ex)
            {
                return StatusCode(400, ex.ValidationCodeResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
