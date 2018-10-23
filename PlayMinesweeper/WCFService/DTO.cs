using PlayMinesweeper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HighscoreRESTService
{
	[DataContract]
	public class DTO
	{
		public DTO(int ErrorCode, string ErrorMsg, List<HighscoreModel> Data)
		{
			this.ErrorCode = ErrorCode;
			this.ErrorMsg = ErrorMsg;
			this.Data = Data;
		}

		[DataMember]
		public int ErrorCode { get; set; }

		[DataMember]
		public string ErrorMsg { get; set; }

		[DataMember]
		public List<HighscoreModel> Data { get; set; }

	}
}