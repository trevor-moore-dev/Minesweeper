using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Trevor Moore
// CST-247
// 2/7/2018
// Milestone 4: Final Game Board Module

namespace PlayMinesweeper.Models
{
	[Serializable]
	public class ButtonModel
	{
		//member variables with getters and setters
		public int Row { get; set; }
		public int Column { get; set; }
		public bool Visited { get; set; }
		public bool Live { get; set; }
		public int Neighbors { get; set; }
		public string Text { get; set; }
		public string Id { get; set; }
		public bool Win { get; set; }

		//constructor that initializes values
		public ButtonModel()
		{
			this.Text = "";
			this.Neighbors = 0;
			this.Live = false;
			this.Visited = false;
			this.Column = -1;
			this.Row = -1;
			this.Id = "null";
			this.Win = false;
		}

	}
}