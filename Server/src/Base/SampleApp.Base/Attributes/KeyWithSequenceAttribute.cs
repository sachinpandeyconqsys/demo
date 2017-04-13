using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base
{
    public class TableWithSequenceAttribute : TableAttribute
    {
        public string SequenceName { get; set; }
        public TableWithSequenceAttribute(string name) : base(name)
        {

        }
    }
}
