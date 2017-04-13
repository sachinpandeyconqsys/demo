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
    public class UserController : UnSecureRepositoryController<IUserRepository>
    {
        public UserController(IUserRepository repository) : base(repository)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]User entity)
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
        public async Task<IActionResult> UpdateUser([FromBody]User entity)
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

        [HttpGet("listOfActiveUsers")]
        public async Task<IEnumerable<IUser>> GetUser()
        {
            return await this.Repository.GetActiveUsers();
        }

        [HttpGet("listOfActiveUsersDetail")]
        public async Task<IEnumerable<IUser>> GetUsers()
        {
            return await this.Repository.GetActUsers();
        }

        [HttpGet("getUser/{id}")]
        public async Task<IEnumerable<IUser>> GetUserDetail(long id)
        {
            return await this.Repository.GetUserById(id);
        }

        [HttpGet("getUserByFn/{id}")]

        public async Task<IEnumerable<IUser>> GetUserByFn(long id)
        {
            return await this.Repository.GetUserByFn(id);
        }
    }
}
