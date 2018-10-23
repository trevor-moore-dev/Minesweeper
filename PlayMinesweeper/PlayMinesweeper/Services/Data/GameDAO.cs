using PlayMinesweeper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

// Trevor Moore
// CST-247
// 4/17/2018

namespace PlayMinesweeper.Services.Data
{
	public class GameDAO
	{
		//setup connection string
		string connectionString = "Server = tcp:playminesweeperserver.database.windows.net,1433;Initial Catalog = PlayMinesweeper_DB; Persist Security Info=False;User ID = trevormoore; Password=Meghan98!; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

		//"Data Source=(localdb)\\MSSQLLocalDB;initial catalog=MinesweeperDatabase ; Integrated Security=True;";

		//method for getting saved game
		public ButtonModel[] getGame(string userName)
		{
			bool result = false;
			ButtonModel[] savedGame = new ButtonModel[225];

			JavaScriptSerializer js = new JavaScriptSerializer();

			try
			{
				// Setup SELECT query with parameters
				string query = "SELECT * FROM dbo.GameBoards WHERE USERNAME=@Username";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = userName;
					//Trace.WriteLine(userName);
					//open the connection
					cn.Open();
					//using a datareader see if query returns any rows
					SqlDataReader reader = cmd.ExecuteReader();
					reader.Read();
					//Trace.WriteLine(reader.GetString(reader.GetOrdinal("GAMEBOARD")));
					if ((string)reader["GAMEBOARD"] != "0")
					{
						savedGame = js.Deserialize<ButtonModel[]>((string)reader["GAMEBOARD"]);
						result = true;
					}
					else
						result = false;

					//close connection
					cn.Close();

				}
			}
			catch (SqlException e)
			{
				//log exception and then throw
				throw e;
			}

			//return result of finder
			if (result)
				return savedGame;
			else
				return null;

		}

		//method for saving current users game
		public string saveGame(ButtonModel[,] game, string userName, int time)
		{
			bool result = false;
			JavaScriptSerializer js = new JavaScriptSerializer();
			string JSONBoard = js.Serialize(game);

			try
			{
				// Setup UPDATE query with parameters
				string query = "UPDATE dbo.GameBoards SET GAMEBOARD=@Gameboard, TIME=@Time WHERE USERNAME=@Username";

				// Create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					// Set query parameters and their values
					cmd.Parameters.Add("@Gameboard", SqlDbType.VarChar).Value = JSONBoard;
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = userName;
					cmd.Parameters.Add("@Time", SqlDbType.Int).Value = time;

					// Open the connection, execute UPDATE, and close the connection
					cn.Open();
					int rows = cmd.ExecuteNonQuery();
					if (rows == 1)
						result = true;
					else
						result = false;

					//close connection
					cn.Close();
				}

			}
			catch (SqlException e)
			{
				// TODO: should log exception and then throw a custom exception
				throw e;
			}

			// Return result of update
			if (result == false)
				return "fail";
			else
				return "success";
		}

		//method for getting time of saved game
		public int getTime(string userName)
		{
			int time;

			try
			{
				// Setup SELECT query with parameters
				string query = "SELECT * FROM dbo.GameBoards WHERE USERNAME=@Username";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = userName;
					Trace.WriteLine(userName);
					//open the connection
					cn.Open();
					//using a datareader see if query returns any rows
					SqlDataReader reader = cmd.ExecuteReader();
					reader.Read();

					//grab the time
					time = (int)reader["TIME"];

					//close connection
					cn.Close();

				}
			}
			catch (SqlException e)
			{
				//log exception and then throw
				throw e;
			}

			//return result of finder
			return time;

		}

		//method for saving current users time to the highscore table
		public string saveTime(string userName, int time)
		{
			bool result;
			int userId = 0;

			try
			{
				// checking to see if username or password is already in database
				string query = "SELECT * FROM dbo.Users WHERE USERNAME=@Username";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = userName;

					//open the connection
					cn.Open();

					//using a datareader see if query returns any rows (to make sure that username or password is not already in the database)
					SqlDataReader dataReader = cmd.ExecuteReader();

					dataReader.Read();

					//grab the time
					userId = (int)dataReader["ID"];

					//close connection
					cn.Close();
				}

				
				// Setup INSERT query with parameters
				query = "INSERT INTO dbo.Highscores (USER_ID, USERNAME, TIME) VALUES (@Userid, @Username, @Time)";

				// Create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					// Set query parameters and their values
					cmd.Parameters.Add("@Userid", SqlDbType.Int).Value = userId;
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = userName;
					cmd.Parameters.Add("@Time", SqlDbType.Int).Value = time;

					// Open the connection, execute INSERT, and close the connection
					cn.Open();
					int rows = cmd.ExecuteNonQuery();
					if (rows == 1)
						result = true;
					else
						result = false;

					//close connection
					cn.Close();
				}

			}
			catch (SqlException e)
			{
				// TODO: should log exception and then throw a custom exception
				throw e;
			}

			// Return result of insert
			if (result == false)
				return "fail";
			else
				return "success";
		}




		///  Methods below are only used in the HighscoreRESTService   ///

		/// THE FOLLOWING METHODS ARE TO BE USED IN HIGHSCORE REST API ///

		//method for getting time of saved game
		public List<HighscoreModel> getHighscores()
		{
			// list of type highscore model
			List<HighscoreModel> highscores = new List<HighscoreModel>();

			try
			{
				// Setup SELECT query with parameters
				string query = "SELECT * FROM dbo.Highscores ORDER BY TIME ASC";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//open the connection
					cn.Open();
					//using a datareader see if query returns any rows
					SqlDataReader reader = cmd.ExecuteReader();

					while(reader.Read())
					{
						int id = (int)reader["ID"];
						string userName = (string)reader["USERNAME"];
						int time = (int)reader["TIME"];

						HighscoreModel hs = new HighscoreModel();
						hs.Id = id;
						hs.Username = userName;
						hs.Time = time;

						highscores.Add(hs);
					}
					//close connection
					cn.Close();
				}
			}
			catch (SqlException e)
			{
				//log exception and then throw
				throw e;
			}
			//return list of highscore models
			return highscores;
		}

		//method for getting a particular users highscores
		public List<HighscoreModel> getUserHighscore(string username)
		{
			// list of type highscore model
			List<HighscoreModel> highscore = new List<HighscoreModel>();

			try
			{
				// Setup SELECT query with parameters
				string query = "SELECT TOP 1 * FROM dbo.Highscores WHERE USERNAME=@Username ORDER BY TIME ASC";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
					//open the connection
					cn.Open();
					//using a datareader see if query returns any rows
					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						int id = (int)reader["ID"];
						string userName = (string)reader["USERNAME"];
						int time = (int)reader["TIME"];

						HighscoreModel hs = new HighscoreModel();
						hs.Id = id;
						hs.Username = userName;
						hs.Time = time;

						highscore.Add(hs);
					}
					//close connection
					cn.Close();
				}
			}
			catch (SqlException e)
			{
				//log exception and then throw
				throw e;
			}
			//return list of highscore models
			return highscore;
		}

		//method for getting the top three highscores in the highscores table
		public List<HighscoreModel> getTopThreeHighscores()
		{
			// list of type highscore model
			List<HighscoreModel> highscores = new List<HighscoreModel>();

			try
			{
				// Setup SELECT query with parameters
				string query = "SELECT TOP 3 * FROM dbo.Highscores ORDER BY TIME ASC";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//open the connection
					cn.Open();
					//using a datareader see if query returns any rows
					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						int id = (int)reader["ID"];
						string userName = (string)reader["USERNAME"];
						int time = (int)reader["TIME"];

						HighscoreModel hs = new HighscoreModel();
						hs.Id = id;
						hs.Username = userName;
						hs.Time = time;

						highscores.Add(hs);
					}
					//close connection
					cn.Close();
				}
			}
			catch (SqlException e)
			{
				//log exception and then throw
				throw e;
			}
			//return list of highscore models
			return highscores;
		}

	}
}