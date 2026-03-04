using System;
using System.Collections.Generic;

namespace WaitList.DataAccess;

public partial class Account
{
    public int AccountId { get; set; }

    public string OrgName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public int ProvinceId { get; set; }

    public string ZipCode { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
