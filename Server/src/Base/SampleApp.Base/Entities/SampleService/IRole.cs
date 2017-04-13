﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Base.Entities
{
    public interface IRole : IEntity, IStamp
    {
        string Name { get; set; }
    }
}
