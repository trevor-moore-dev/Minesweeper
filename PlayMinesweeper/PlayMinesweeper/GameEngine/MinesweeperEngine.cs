using PlayMinesweeper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

// Trevor Moore
// CST-247
// 2/7/2018
// Milestone 3: Initial Game Board Module

namespace PlayMinesweeper.GameEngine
{
	public class MinesweeperEngine
	{
		//member variables of MinesweeperEngine to be used and accessed
		public ButtonModel[,] grid;
		private Random randm = new Random();
		static int numLive = 0;

		//empty constructor
		public MinesweeperEngine()
		{

		}


		//method for instantiating a new board
		public ButtonModel[,] createBoard()
		{
			//instantiating 2d array of ButtonModels
			grid = new ButtonModel[15, 15];
			int id = 1;

			//Adding buttons to the array with their corresponding row and column
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					//initializes each cell in the grid array
					grid[i, j] = new ButtonModel();
					grid[i, j].Column = j;
					grid[i, j].Row = i;
					grid[i, j].Id = "" + id;
					id++;
				}
			}

			////////////////////////////////////////// ACTIVATE CELLS //////////////////////////////////////

			numLive = 0;
			//Activating live buttons
			foreach (ButtonModel c in grid)
			{
				//activate is random number from 0 - 99
				int activate = randm.Next(100);

				//if random int is less than 20
				if (activate < 15)
				{
					//cell is set to live
					c.Live = true;
					//neighbors set to 9
					c.Neighbors = 9;
					//if the cell has a bomb +1 is added to the numLive bomb counter
					numLive++;
				}
			}



			////////////////////////////////////////// COUNT LIVE NEIGHBORS /////////////////////////////////
			//counting each buttons live neighbors
			foreach (ButtonModel c in grid)
			{
				//counter for number of live neighbors a cell has
				int liveNeighbors = 0;

				//double for loops that loop through all 8 possible neighbors that a cell can have
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						//check to make sure neighbor cell is in bounds of the array
						if ((c.Row + i) >= 0 && (c.Row + i) <= (15 - 1) && (c.Column + j) >= 0 && (c.Column + j) <= (15 - 1))
						{
							//if a neighbor cell is live, 1 is added to the counter
							if (grid[(c.Row + i), (c.Column + j)].Live == true)
								liveNeighbors++;
							//current cell's live neighbors gets set to the counter
							grid[c.Row, c.Column].Neighbors = liveNeighbors;
						}
					}
				}
			}

			return grid;
		}

		//method that will be called on each button click
		public ButtonModel[,] onClick(ButtonModel c)
		{
			revealNeighbors(c);

			checkWin(c);

			//if button that was clicked was live, all buttons marked as visited
			if (c.Live == true)
			{
				foreach (ButtonModel b in grid)
				{
					b.Visited = true;
				}
			}

			return grid;
		}

		//method that recursively reveals neighbors
		public void revealNeighbors(ButtonModel c)
		{
			//bailout statement that stops method from recursing infinitely, if a cell has already been through the method once
			//(i.e. been visited) the method is returned and the recursion stops
			if (grid[c.Row, c.Column].Visited == true)
				return;

			//if the selected cell has zero live neighbors it goes through the following loops
			if (grid[c.Row, c.Column].Neighbors == 0)
			{
				//selected cell is marked as visited and cell displays "" before going through loops
				grid[c.Row, c.Column].Visited = true;
				grid[c.Row, c.Column].Text = "";

				//double for loops that loop through all 8 possibilities of neighbors
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						//if statment to check if all 8 possibilities of neighbors are valid (in bounds of the array)
						if ((c.Row + i) >= 0 && (c.Row + i) <= (15 - 1) && (c.Column + j) >= 0 && (c.Column + j) <= (15 - 1))
						{
							//if the neighbor has zero live neighbors, the method is recursed on that neighbor
							if (grid[(c.Row + i), (c.Column + j)].Neighbors == 0)
							{
								revealNeighbors(grid[(c.Row + i), (c.Column + j)]);
							}
							//if the neighbor has live neighbors, that neighbor is marked as visited and neighbors are displayed, thus stopping the recursion
							else if (grid[(c.Row + i), (c.Column + j)].Neighbors > 0)
							{
								grid[(c.Row + i), (c.Column + j)].Visited = true;
								grid[(c.Row + i), (c.Column + j)].Text = grid[(c.Row + i), (c.Column + j)].Neighbors + "";
							}
						}
					}
				}
			}
			//if the selected cell has more than zero live neighbors, it is marked as visited and displays neighbors, thus stopping the recursion
			else
			{
				grid[c.Row, c.Column].Visited = true;
				grid[c.Row, c.Column].Text = c.Neighbors + "";
			}
		}


		//method to check and see if the user has won
		public void checkWin(ButtonModel current)
		{
			//integer to count how many cells havent been visited by the user
			int unvisited = 0;

			//loop that counts how many cells havent been visited by the user
			foreach (ButtonModel c in grid)
			{
				if (c.Visited == false)
				{
					unvisited++;
				}
			}
			//Trace.WriteLine(unvisited);
			//Trace.WriteLine(numLive);

			//if the number of bombs equals the amount of cells not visited by the user, the user won
			if ((unvisited == numLive) && (!current.Live))
			{
				//foreach that disables all cell buttons in grid
				foreach (ButtonModel c in grid)
				{
					c.Visited = true;
					c.Win = true;
					//Trace.WriteLine("Button set to visited and win");
				}
			}
		}

		// Getter for grid
		public ButtonModel[,] getGrid()
		{
			return grid;
		}

		// Setter for grid
		public void setGrid(ButtonModel[,] grid)
		{
			this.grid = grid;
		}

		public void createSavedGame(ButtonModel[] game)
		{
			//instantiating 2d array of ButtonModels
			ButtonModel[,] savedGame = new ButtonModel[15, 15];
			int x = 0;

			//Adding buttons to the array with their corresponding row and column
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					//initializes each cell in the grid array
					savedGame[i, j] = new ButtonModel();
					savedGame[i, j] = game[x];
					x++;
				}
			}

			this.grid = savedGame;
		}





	}
}