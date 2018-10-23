
using PlayMinesweeper.GameEngine;
using PlayMinesweeper.Models;
using PlayMinesweeper.Services.Business;
using PlayMinesweeper.Services.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

// Trevor Moore
// CST-247
// 4/17/2018

namespace PlayMinesweeper.Controllers
{
	[CustomAction]
    public class UserController : Controller
    {
		// injecting logging service into controller
		private readonly ILogger logger;

		public UserController(ILogger service)
		{
			this.logger = service;
		}

		//get method for rendering Login view
		[HttpGet]
		public ActionResult Index()
		{
			return View("Login");
		}

		//get method for rendering Register view
		[HttpGet]
		public ActionResult Register()
		{
			return View("Register");
		}

		//get method for rendering Login view
		[HttpGet]
		public ActionResult Login()
		{
			return View("Login");
		}

		//get method for rendering Login view
		[HttpGet]
		public ActionResult Logout()
		{
			// setting the Username in the session to null so that they cannot access pages after logout
			HttpContext.Session["Username"] = null;

			return View("Login");
		}

		//post for loging in user
		[HttpPost]
		public ActionResult LoginUser(UserModel user)
		{

			logger.Info("Entering UserController.LoginUser()");
			logger.Info("Parameters are: {0}", new JavaScriptSerializer().Serialize(user));

			try
			{
				//validate input
				if (!ModelState.IsValid)
					return View("Login");

				//new security service for authenticating user
				SecurityService ss = new SecurityService();

				bool result = ss.Authenticate(user);

				//returning correct view
				if (result == true)
				{
					logger.Info("Exiting UserController.LoginUser() with login passed");

					// storing username of current user in session
					HttpContext.Session["Username"] = user.Username;
					return View("Home", user);
				}
				else
				{
					logger.Info("Exiting UserController.LoginUser() with login failed");
					return View("LoginFailed");
				}
			}
			catch(Exception e)
			{
				logger.Error("Exception UserController.LoginUser()", e.Message);
				return View("LoginError");
			}

		}

		//post for registering user
		[HttpPost]
		public ActionResult RegisterUser(RegisterModel user)
		{

			logger.Info("Entering UserController.RegisterUser()");
			logger.Info("Parameters are: {0}", new JavaScriptSerializer().Serialize(user));

			try
			{
				//validate input
				if (!ModelState.IsValid)
					return View("Register");

				//new security service for registering user
				SecurityService ss = new SecurityService();

				string result = ss.Register(user);

				//returning the correct view
				if (result.Equals("success"))
				{
					logger.Info("Exiting UserController.RegisterUser() with register success");
					return View("Login");
				}
				else if (result.Equals("fail"))
				{
					logger.Info("Exiting UserController.RegisterUser() with register fail");
					return View("RegisterFailed");
				}
				else
				{
					logger.Info("Exiting UserController.RegisterUser() with attempt to register duplicate user");
					return View("DuplicateUser");
				}
			}
			catch(Exception e)
			{
				logger.Error("Exception UserController.RegisterUser()", e.Message);
				Trace.WriteLine(e.Message);
				return View("RegisterError");
			}
		}



		[HttpGet]
		[CustomAuthorization]
		public string Protected()
		{
			return "I am a proteted method";
		}




	}
}