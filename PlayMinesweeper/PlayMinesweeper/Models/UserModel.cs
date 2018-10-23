using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Trevor Moore
// CST-247
// 2/7/2018
// Milestone 3: Initial Game Board Module

namespace PlayMinesweeper.Models
{
	public class UserModel
	{
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