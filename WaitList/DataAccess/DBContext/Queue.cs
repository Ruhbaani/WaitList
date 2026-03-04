using System;
using System.Collections.Generic;

namespace WaitList.DataAccess;

public partial class Queue
{
    public int QueueId { get; set; }

    public int CustomerId { get; set; }

    public int AccountId { get; set; }

    public byte[] DateTime { get; set; } = null!;

    public int ServiceId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
