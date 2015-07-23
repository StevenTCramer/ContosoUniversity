namespace ContosoUniversity.Features.Department
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using MediatR;

	public class Index
	{
		public class Query : IAsyncRequest<List<Model>>
		{
		}

		public class Model
		{
			public string Name { get; set; }

			public decimal Budget { get; set; }

			public DateTime StartDate { get; set; }

			public int DepartmentID { get; set; }

			[Display(Name = "Administrator")]
			public string AdministratorFullName { get; set; }
		}

		public class QueryHandler : IAsyncRequestHandler<Query, List<Model>>
		{
			private readonly SchoolContext _context;

			public QueryHandler(SchoolContext context)
			{
				_context = context;
			}

			public async Task<List<Model>> Handle(Query message)
			{
				return await _context.Departments
					.ProjectToListAsync<Model>();
			}
		}

		public class UiController : Controller
		{
			private readonly IMediator _mediator;

			public UiController(IMediator mediator)
			{
				_mediator = mediator;
			}

			public async Task<ActionResult> Index()
			{
				var model = await _mediator.SendAsync(new Index.Query());

				return View(model);
			}
		}
	}
}