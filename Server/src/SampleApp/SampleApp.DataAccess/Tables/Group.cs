using SampleApp.Base;
using SampleApp.Base.Entities;
using SimpleStack.Orm.Attributes;
using System;

namespace SampleApp.DataAccess
{
    [TableWithSequence("group", SequenceName = "group_id_seq")]
    [Alias("group")]
    public partial class Group : IGroup
    {
        public Group()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

       
       
    }
}
