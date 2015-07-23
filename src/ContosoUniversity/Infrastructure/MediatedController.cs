using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediatR;

namespace ContosoUniversity.Infrastructure
{
	public class MediatedController : Controller
	{
		protected readonly IMediator _mediator;

		public MediatedController(IMediator mediator)
		{
			_mediator = mediator;
		}
		

		//TODO Can I make this more generic
		public async Task<ActionResult> Query(IAsyncRequest<IAsyncRequest> query)
		{
			var model = await _mediator.SendAsync(query);

			return View(model);
		}

	}
}