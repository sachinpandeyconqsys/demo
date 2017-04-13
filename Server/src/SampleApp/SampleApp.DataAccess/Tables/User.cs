using SampleApp.Base;
using SampleApp.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;

namespace SampleApp.DataAccess
{
    [TableWithSequence("user", SequenceName = "user_id_seq")]
    [Alias("user")]
    public partial class User : IUser
    {
        public User()
        {
            GroupData = new List<UserGroup>();

        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.EmailAddress]
        [Alias("email")]
        public string Email { get; set; }

        [Alias("password")]
        public string Password { get; set; }

        [Alias("is_active")]
        public bool IsActive { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("role_id")]
        public long? RoleId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Alias("phone")]
        public string Phone { get; set; }

        [Alias("group_id")]
        public long? GroupId { get; set; }

        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Ignore]
        [Alias("role_name")]
        public string RoleName { get; set; }
        
        [Ignore]
        [Alias("group_name")]
        public string GroupName { get; set; }

        [Ignore]       
        public IEnumerable<IUserGroup> GroupData { get; set; }
         
    }
}
