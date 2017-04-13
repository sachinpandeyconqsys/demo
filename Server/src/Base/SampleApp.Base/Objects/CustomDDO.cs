using SampleApp.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Objects
{
    public class CustomDDO : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
