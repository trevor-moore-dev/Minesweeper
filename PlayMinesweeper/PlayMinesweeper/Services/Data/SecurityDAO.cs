using PlayMinesweeper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

// Trevor Moore
// CST-247
// 4/17/2018

namespace PlayMinesweeper.Services.Data
{
	public class SecurityDAO
	{
		//setup connection string
		string connectionString = string.Empty;

		//method for logging in
		public bool findByUser(UserModel user)
		{
			bool result = false;

			try
			{
				string query = "SELECT * FROM dbo.Users WHERE USERNAME=@Username COLLATE SQL_Latin1_General_CP1_CS_AS AND PASSWORD=@Password COLLATE SQL_Latin1_General_CP1_CS_AS";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = user.Username;
					cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = user.Password;

					//open the connection
					cn.Open();

					//using a datareader see if query returns any rows
					SqlDataReader reader = cmd.ExecuteReader();
					if (reader.HasRows)
						result = true;
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
			return result;
		}

		//method for registering a user
		public string Create(RegisterModel user)
		{
			bool result = false;
			int userId = 1;

			try
			{
				// checking to see if username or password is already in database
				string query = "SELECT * FROM dbo.Users WHERE USERNAME=@Username OR PASSWORD=@Password";

				//create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = user.Username;
					cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = user.Password;

					//open the connection
					cn.Open();

					//using a datareader see if query returns any rows (to make sure that username or password is not already in the database)
					SqlDataReader reader = cmd.ExecuteReader();
					if (reader.HasRows)
						result = false;
					else
						result = true;

					//close connection
					cn.Close();
				}

				if (result == false)
					return "duplicate";

				// Setup INSERT query with parameters
				query = "INSERT INTO dbo.Users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, SEX, AGE, STATE, EMAIL) VALUES (@Username, @Password, @Firstname, @Lastname, @Sex, @Age, @State, @Email); SELECT SCOPE_IDENTITY () As UserID";

				// Create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					// Set query parameters and their values
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = user.Username;
					cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = user.Password;
					cmd.Parameters.Add("@Firstname", SqlDbType.VarChar, 50).Value = user.Firstname;
					cmd.Parameters.Add("@Lastname", SqlDbType.VarChar, 50).Value = user.Lastname;
					cmd.Parameters.Add("@Sex", SqlDbType.VarChar, 50).Value = user.Sex;
					cmd.Parameters.Add("@Age", SqlDbType.Int).Value = user.Age;
					cmd.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = user.State;
					cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = user.Email;

					// Open the connection, execute INSERT, and close the connection
					cn.Open();

					// execute query using sqldatareader
					SqlDataReader dataReader = cmd.ExecuteReader();

					// if the query returned rows save the user's ID
					if (dataReader.HasRows)
					{
						dataReader.Read();
						userId = Convert.ToInt32(dataReader["UserID"]);
					}

					//close connection
					cn.Close();
				}

				//inserting place holder for users gameboard into GameBoards table in database
				query = "INSERT INTO dbo.GameBoards (USER_ID, USERNAME, GAMEBOARD, TIME) VALUES (@Userid, @Username, @Gameboard, @Time)";

				// Create connection and command
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					// Set query parameters and their values
					cmd.Parameters.Add("@Userid", SqlDbType.Int).Value = userId;
					cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = user.Username;
					cmd.Parameters.Add("@Gameboard", SqlDbType.VarChar).Value = "0";
					cmd.Parameters.Add("@Time", SqlDbType.Int).Value = -1;

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
	}
}
