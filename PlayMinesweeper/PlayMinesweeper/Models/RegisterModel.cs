using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Trevor Moore
// CST-247
// 2/7/2018
// Milestone 4: Final Game Board Module

namespace PlayMinesweeper.Models
{
	public class RegisterModel
	{
		[Required]
		[DisplayName("First Name")]
		[StringLength(20, MinimumLength = 4)]
		[DefaultValue("")]
		public string Firstname { get; set; }

		[Required]
		[DisplayName("Last Name")]
		[StringLength(20, MinimumLength = 4)]
		[DefaultValue("")]
		public string Lastname { get; set; }

		[Required]
		[DisplayName("Sex")]
		[StringLength(6, MinimumLength = 4)]
		[DefaultValue("")]
		public string Sex { get; set; }

		[Required]
		[DisplayName("Age")]
		[Range(1, 125)]
		[DefaultValue("")]
		public int Age { get; set; }

		[Required]
		[DisplayName("State")]
		[StringLength(20, MinimumLength = 4)]
		[DefaultValue("")]
		public string State { get; set; }

		[Required]
		[DisplayName("Email")]
		[StringLength(30, MinimumLength = 4)]
		[DefaultValue("")]
		public string Email { get; set; }

		[Required]
		[DisplayName("User Name")]
		[StringLength(20, MinimumLength = 4)]
		[DefaultValue("")]
		public string Username { get; set; }

		[Required]
		[DisplayName("Password")]
		[StringLength(20, MinimumLength = 4)]
		[DefaultValue("")]
		public string Password { get; set; }
	}
}