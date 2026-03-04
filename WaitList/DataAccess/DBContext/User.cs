using System;
using System.Collections.Generic;

namespace WaitList.DataAccess;

public partial class User
{
    public int UserId { get; set; }

    public int AccountId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public virtual Account Account { get; set; } = null!;
}
