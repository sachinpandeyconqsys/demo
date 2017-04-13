using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Entities
{
    public interface IUser : IEntity, IStamp
    {
        string Name { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool IsActive { get; set; }
        long? RoleId { get; set; }
        string Phone { get; set; }
        long? GroupId { get; set; }
        string RoleName { get; set; }            
        string GroupName { get; set; }      

        IEnumerable<IUserGroup> GroupData { get; set; }

    }
}
