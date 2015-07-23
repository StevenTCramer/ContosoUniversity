namespace ContosoUniversity.Features.Home
{
	using System;	
	using System.Linq;
	using System.Web.Mvc;

	public class Index
	{
		public class UiController : Controller
		{
			public ActionResult Request()
			{
				return View();
			}
		}
	}
}