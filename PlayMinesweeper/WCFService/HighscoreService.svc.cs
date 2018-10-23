using PlayMinesweeper.Models;
using PlayMinesweeper.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HighscoreRESTService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class HighscoreService : IHighscoreService
	{

		public DTO GetHighscores()
		{
			try
			{
				// new game service for passing DTO all highscores
				GameService gs = new GameService();
				// new DTO to be returned to the user with correct error code, error message, and data
				DTO dto = new DTO(200, "All Highscores", gs.getAllHighscores());

				return dto;
			}
			catch (Exception e)
			{
				// new DTO to be returned to the user with correct error code, error message, and data, if there is an exception
				DTO dto = new DTO(500, "Internal Server Error", null);
				return dto;
			}
		}
		
		public DTO GetUserHighscore(string userName)
		{
			try
			{
				// new DTO to be returned to the user with correct error code, error message, and data
				DTO dto;
				// new game service for passing DTO a particular user's highscore
				GameService gs = new GameService();

				// if the username doesnt exist, return correct DTO, if they do, return their top highscore
				if(!gs.getUserHighscore(userName).Any())
					 dto = new DTO(404, "User '" + userName + "' doesn't exist.", gs.getUserHighscore(userName));
				else
					dto = new DTO(200, userName + "'s Highscore", gs.getUserHighscore(userName));

				return dto;
			}
			catch (Exception e)
			{
				// new DTO to be returned to the user with correct error code, error message, and data, if there is an exception
				DTO dto = new DTO(500, "Internal Server Error", null);
				return dto;
			}
}
		
		public DTO GetTopThree()
		{
			try
			{
				// new game service for passing DTO a particular user's highscore
				GameService gs = new GameService();
				// new game service for passing DTO top three highscores
				DTO dto = new DTO(200, "Top Three Highscores", gs.getTopThreeHighscores());

				return dto;
			}
			catch (Exception e)
			{
				// new DTO to be returned to the user with correct error code, error message, and data, if there is an exception
				DTO dto = new DTO(500, "Internal Server Error", null);
				return dto;
			}
}
		
	}
}
