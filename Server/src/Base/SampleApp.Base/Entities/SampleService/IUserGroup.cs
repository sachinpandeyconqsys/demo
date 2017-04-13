using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Entities
{
    public interface IUserGroup : IEntity
    {
         string Name { get; set; }
         long UserId { get; set; }
         long GroupId { get; set; }
         string GroupName { get; set; }
       

    }
}
