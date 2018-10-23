using PlayMinesweeper.GameEngine;
using PlayMinesweeper.Models;
using PlayMinesweeper.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

// Trevor Moore
// CST-247
// 4/17/2018

namespace PlayMinesweeper.Controllers
{
    public class GameController : Controller
    {
		//MinesweeperEngine object to be used for instantiating game board
		private MinesweeperEngine me = new MinesweeperEngine();

		//Get method for rendering new game board
		[CustomAuthorization]
		public ActionResult PlayAgain()
		{
			// getting minesweeper engine from session
			MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];

			// redirecting if user is not logged in
			if (HttpContext.Session["Username"] == null)
				return View("~/Views/User/Login.cshtml");
			else
			{
				// getting the users current time on the game
				HttpContext.Session["Time"] = -1;

				// create new game board
				return View("PlayMinesweeper", newMe.createBoard());

			}
		}

		//Get method for rendering new or saved game board
		[CustomAuthorization]
		public ActionResult PlayMinesweeper()
		{
			if (HttpContext.Session["Username"] == null)
				return View("~/Views/User/Login.cshtml");
			else
			{
				// getting the existing board (if there is one)

				// storing minesweeper engine in session
				HttpContext.Session["ME"] = me;
				MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];

				// grabbing the users username from the session
				string userName = (string)HttpContext.Session["Username"];

				GameService gs = new GameService();

				// getting the users current time on the game
				HttpContext.Session["Time"] = gs.getTime(userName);

				// check and see if current user has a game saved in database, if not, create new one
				if (gs.getGame(userName) != null)
				{
					newMe.createSavedGame((gs.getGame(userName)));
					return View("PlayMinesweeper", newMe.getGrid());
				}
				else	
					return View("PlayMinesweeper", newMe.createBoard());
			
			}
		}

		//Post method for rendering PlayMinesweeper view after button click ( FULL PAGE REFRESH)
		[CustomAuthorization]
		public ActionResult OnButtonClick(string mine)
		{
			// getting minesweeper engine from session
			MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];

			foreach (ButtonModel c in newMe.getGrid())
			{
				if (mine.Equals(c.Id))
				{
					newMe.onClick(c);
				}
			}

			return View("PlayMinesweeper", newMe.getGrid());
		}

		/**
		 * NEW PARTIAL PAGE UPDATE METHOD
		 **/
		//Post method for rendering MinesweeperBoard view after button click ( PARTIAL PAGE UPDATE)
		[HttpPost]
		[CustomAuthorization]
		public PartialViewResult OnClick(string mine)
		{
			// getting minesweeper engine from session
			MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];

			foreach (ButtonModel c in newMe.getGrid())
			{
				if (mine.Equals(c.Id))
				{
					newMe.onClick(c);

					// if users has won, send time to highscore table
					if (c.Win)
					{
						GameService gs = new GameService();
						gs.saveTime((string)HttpContext.Session["Username"], (int)HttpContext.Session["Time"]);
					}
				}
			}

			return PartialView("MinesweeperBoard", newMe.getGrid());
		}

		//method to logout user and save game state to GameBoard table
		[HttpGet]
		public ActionResult Logout()
		{
			// getting minesweeper engine from session
			MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];

			//saving game and time to the GameBoards table in the database
			GameService gs = new GameService();
			gs.saveGame(newMe.getGrid(), (string)HttpContext.Session["Username"], (int)HttpContext.Session["Time"]);

			//setting the Username in the session to null so that they cannot access pages after logout
			HttpContext.Session["Username"] = null;
			return View("~/Views/User/Login.cshtml");
		}

		//method that will save the game for the user
		[CustomAuthorization]
		public ActionResult SaveGame()
		{
			// getting minesweeper engine and username from session
			MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];
			string userName = (string)HttpContext.Session["Username"];

			//saving game and time to the GameBoards table in the database
			GameService gs = new GameService();
			gs.saveGame(newMe.getGrid(), userName, (int)HttpContext.Session["Time"]);

			// getting the users current time on the game
			HttpContext.Session["Time"] = gs.getTime(userName);

			// creating the users saved game
			newMe.createSavedGame((gs.getGame(userName)));

			return View("PlayMinesweeper", newMe.getGrid());

		}

		//method for rendering the Time partial view every second
		[OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 1)]
		[CustomAuthorization]
		public PartialViewResult Time()
		{
			// getting minesweeper engine from session
			MinesweeperEngine newMe = (MinesweeperEngine)HttpContext.Session["ME"];

			// if user has won, stop updating time, else increment time
			if (newMe.getGrid()[0, 0].Win)
				return PartialView("Time", (int)HttpContext.Session["Time"]);
			else
			{
				HttpContext.Session["Time"] = (int)HttpContext.Session["Time"] + 1;

				return PartialView("Time", (int)HttpContext.Session["Time"]);
			}

		}
	}
}