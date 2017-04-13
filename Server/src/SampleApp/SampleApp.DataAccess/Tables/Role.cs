using SampleApp.Base;
using SampleApp.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace SampleApp.DataAccess
{
    [TableWithSequence("role", SequenceName = "role_id_seq")]
    [Alias("role")]
    public partial class Role : IRole
    {
        public Role()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
        
        [Alias("created_by")]
        public long? CreatedBy { get; set; }

        [Alias("created_date")]
        public DateTime? CreatedDate { get; set; }
        
        [Alias("modified_by")]
        public long? ModifiedBy { get; set; }

        [Alias("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
