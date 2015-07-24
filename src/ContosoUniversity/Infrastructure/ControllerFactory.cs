namespace ContosoUniversity.Infrastructure
{
	using System;
	using System.Web.Mvc;
	using System.Web.Mvc.Async;
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
		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			var controller = base.CreateController(requestContext, controllerName);
			return ReplaceActionInvoker(controller);
		}

		private IController ReplaceActionInvoker(IController controller)
		{
			var mvcController = controller as Controller;
			if (mvcController != null)
			{
				mvcController.ActionInvoker = new FeatureActionInvoker();
			}

			return controller;
		}

	}

	public class FeatureActionInvoker : AsyncControllerActionInvoker
	{

		protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
		{
			return base.FindAction(controllerContext, controllerDescriptor, "Action");
			//return base.FindAction(controllerContext, controllerDescriptor, actionName);
		}
	}
}