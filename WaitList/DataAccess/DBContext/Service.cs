using System;
using System.Collections.Generic;

namespace WaitList.DataAccess;

public partial class Service
{
    public int ServiceId { get; set; }

    public int ServiceType { get; set; }

    public string ServiceDescription { get; set; } = null!;

    public virtual ICollection<Queue> Queues { get; set; } = new List<Queue>();
}
