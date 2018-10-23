using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HighscoreRESTService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IHighscoreService
	{
		// get all highscores API method
		[OperationContract]
		[WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetHighscores/")]
		DTO GetHighscores();

		// get a particular users' highscore API method
		[OperationContract]
		[WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserHighscore/{userName}")]
		DTO GetUserHighscore(string userName);

		// get top three highscores API method
		[OperationContract]
		[WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTopThreeHighscores/")]
		DTO GetTopThree();
	}
}
