using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.AuthenticationDtos
{
	public class dtoRegister
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email format")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "First name is required")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Username is required")]
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Birth date is required")]
		public DateOnly BirthDate { get; set; }

		[Required(ErrorMessage = "Telephone is required")]
		[Phone(ErrorMessage = "Invalid phone number format")]
		public string Telephone { get; set; } = string.Empty;

		[Required(ErrorMessage = "Gender is required")]
		public string Gender { get; set; } = string.Empty;

		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? ZipCode { get; set; }
		public string? Country { get; set; }
	}
}
