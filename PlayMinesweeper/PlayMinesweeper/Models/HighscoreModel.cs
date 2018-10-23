using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PlayMinesweeper.Models
{
	public class HighscoreModel
	{
		public string Username { get; set; }
		public int Time { get; set; }
		public int Id { get; set; }
	}
}