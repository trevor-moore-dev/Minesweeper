using PlayMinesweeper.Models;
using PlayMinesweeper.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Trevor Moore
// CST-247
// 4/17/2018

namespace PlayMinesweeper.Services.Business
{
	public class SecurityService
	{
		//method for calling findbyuser method from SecurityDAO class
		public bool Authenticate(UserModel user)
		{
			SecurityDAO service = new SecurityDAO();

			// return result of findByUser method called on security dao
			return service.findByUser(user);
		}

		//method for calling Create method from SecurityDAO class
		public string Register(RegisterModel user)
		{
			SecurityDAO service = new SecurityDAO();

			// return result of Create method called on security dao
			return service.Create(user);
		}

	}
}