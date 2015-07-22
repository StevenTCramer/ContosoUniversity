namespace ContosoUniversity.Infrastructure
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;

	public class ControllerFactory : DefaultControllerFactory
	{
		protected override Type GetControllerType(RequestContext requestContext, string controllerName)
		{	
			bool isFeature = Convert.ToBoolean(requestContext.RouteData.GetRequiredString("isFeature"));
			string typeName = "";

			if (isFeature)
			{
				string action = requestContext.RouteData.GetRequiredString("action");
				typeName = "ContosoUniversity.Features." + controllerName + "." + action + "+UiController";
			}
			else
			{
				typeName = "ContosoUniversity.Features." + controllerName + ".UiController";
			}
			var assembly = typeof(ControllerFactory).Assembly;
			Type type = assembly.GetType(typeName);
			return type;
		}
	}
}