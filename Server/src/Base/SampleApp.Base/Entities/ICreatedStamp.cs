
using System;

namespace SampleApp.Base.Entities
{
    public interface ICreatedStamp
    {
        long? CreatedBy { get; set; }

        DateTime? CreatedDate { get; set; }
    }
}
