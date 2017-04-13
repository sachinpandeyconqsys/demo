using SampleApp.Base;
using SampleApp.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;
using System.Collections.Generic;

namespace SampleApp.DataAccess
{
    [TableWithSequence("user_group", SequenceName = "user_group_id_seq")]
    [Alias("user")]
    public partial class UserGroup : IUserGroup
    {
        public UserGroup()
        {
            

        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]        
        [Alias("user_id")]
        public long UserId { get; set; }

        [Alias("group_id")]
        public long GroupId { get; set; }

        [Alias("group_name")]
        public string GroupName { get; set; }
        
       

       
         
    }
}
