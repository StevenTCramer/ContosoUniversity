namespace ContosoUniversity.Features.Home
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	public class Contact
	{
		public class UiController : Controller
		{
			public ActionResult Contact()
			{
				ViewBag.Message = "Your contact page.";

				return View();
			}
		}
	}
}