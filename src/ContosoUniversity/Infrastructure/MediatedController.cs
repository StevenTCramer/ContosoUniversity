using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediatR;

namespace ContosoUniversity.Infrastructure
{
	public class MediatedController<TQuery, TResponse> : Controller
		where TQuery : IAsyncRequest<TResponse>
	{
		protected readonly IMediator _mediator;

		public MediatedController(IMediator aMediator)
		{
			_mediator = aMediator;
		}
		

		public virtual async Task<ActionResult> Action(TQuery query)
		{			
			TResponse model = await _mediator.SendAsync(query);

			return View(model);
		}

	}
}