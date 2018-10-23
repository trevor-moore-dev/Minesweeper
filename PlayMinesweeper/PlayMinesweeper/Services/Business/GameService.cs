using PlayMinesweeper.Models;
using PlayMinesweeper.Services.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

// Trevor Moore
// CST-247
// 4/17/2018

namespace PlayMinesweeper.Services.Business
{
	public class GameService
	{
		//method for saving the current users game
		public string saveGame(ButtonModel[,] game, string userName, int time)
		{
			GameDAO service = new GameDAO();

			// return result of saveGame method called on game dao
			return service.saveGame(game, userName, time);
		}

		//method for getting the current users game
		public ButtonModel[] getGame(string userName)
		{
			GameDAO service = new GameDAO();

			// return result of getGame method called on game dao
			return service.getGame(userName);
		}

		//method for getting current users time
		public int getTime(string userName)
		{
			GameDAO service = new GameDAO();

			// return result of getTime method called on game dao
			return service.getTime(userName);
		}

		//method for saving current users time to the highscores table
		public void saveTime(string userName, int time)
		{
			GameDAO service = new GameDAO();

			// return result of saveTime method called on game dao
			service.saveTime(userName, time);
		}



		
		/// The following methods are only used in the HighscoreRESTService ///

		//method for getting all the scores in the highscores table
		public List<HighscoreModel> getAllHighscores()
		{
			// new game dao
			GameDAO service = new GameDAO();
			// new list of highscore models
			List<HighscoreModel> highscores = new List<HighscoreModel>();
			// set list to result of getHighscores called on the service
			highscores = service.getHighscores();

			// return the list
			return highscores;
		}

		//method for getting particular user score in the highscores table
		public List<HighscoreModel> getUserHighscore(string userName)
		{
			// new game dao
			GameDAO service = new GameDAO();
			// new list of highscore models
			List<HighscoreModel> highscore = new List<HighscoreModel>();
			// set list to result of getUserHighscore called on the service
			highscore = service.getUserHighscore(userName);

			// return the list
			return highscore;
		}

		//method for getting the top three scores in the highscores table
		public List<HighscoreModel> getTopThreeHighscores()
		{
			// new game dao
			GameDAO service = new GameDAO();
			// new list of highscore models
			List<HighscoreModel> highscores = new List<HighscoreModel>();
			// set list to result of getTopThreeHighscores called on the service
			highscores = service.getTopThreeHighscores();

			// return the list
			return highscores;
		}


	}
}