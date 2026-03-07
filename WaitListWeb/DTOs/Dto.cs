using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models;

public class LoginDto
{
	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string Password { get; set; } = string.Empty;
}

public class CreateUserDto
{
	[Required]
	public string FirstName { get; set; } = string.Empty;

	[Required]
	public string LastName { get; set; } = string.Empty;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string UserName { get; set; } = string.Empty;

	public string? PhoneNumber { get; set; }

	[Required]
	public string Password { get; set; } = string.Empty;

	[Required]
	public string Role { get; set; } = string.Empty;

	public int AccountId { get; set; }
}

public class CreateAccountDto
{
	[Required]
	public string OrgName { get; set; } = string.Empty;

	[Required]
	public string FirstName { get; set; } = string.Empty;

	[Required]
	public string LastName { get; set; } = string.Empty;

	public string Phone { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string ProvinceId { get; set; } = string.Empty;
	public string ZipCode { get; set; } = string.Empty;

	[Required]
	[EmailAddress]
	public string OwnerEmail { get; set; } = string.Empty;

	[Required]
	public string OwnerUserName { get; set; } = string.Empty;

	[Required]
	public string OwnerPassword { get; set; } = string.Empty;
}

public class CreateQueueDto
{
	[Required]
	public string Name { get; set; } = string.Empty;

	public bool IsActive { get; set; } = true;

	public int AccountId { get; set; }
}

public class CreateQueueEntryDto
{
	[Required]
	public int CustomerId { get; set; }

	public int? ServiceId { get; set; }
}

public class ReorderQueueEntriesDto
{
	[Required]
	public List<int> EntryIdsInOrder { get; set; } = new();
}