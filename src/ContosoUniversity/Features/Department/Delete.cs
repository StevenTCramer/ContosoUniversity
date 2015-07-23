namespace ContosoUniversity.Features.Department
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using MediatR;
	using Infrastructure;

	public class Delete
	{
		public class Query : IAsyncRequest<Command>
		{
			public int Id { get; set; }
		}

		public class Command : IAsyncRequest
		{
			public string Name { get; set; }

			public decimal Budget { get; set; }

			public DateTime StartDate { get; set; }

			public int DepartmentID { get; set; }

			[Display(Name = "Administrator")]
			public string AdministratorFullName { get; set; }

			public byte[] RowVersion { get; set; }
		}

		public class QueryHandler : IAsyncRequestHandler<Query, Command>
		{
			private readonly SchoolContext _db;

			public QueryHandler(SchoolContext db)
			{
				_db = db;
			}

			public async Task<Command> Handle(Query message)
			{
				var department = await _db.Departments
						.Where(d => d.DepartmentID == message.Id)
						.ProjectToSingleOrDefaultAsync<Command>();

				return department;
			}
		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			private readonly SchoolContext _db;

			public CommandHandler(SchoolContext db)
			{
				_db = db;
			}

			protected override async Task HandleCore(Command message)
			{
				var department = await _db.Departments.FindAsync(message.DepartmentID);

				_db.Departments.Remove(department);
			}
		}

		public class UiController : Controller
		{
			private readonly IMediator _mediator;

			public UiController(IMediator mediator)
			{
				_mediator = mediator;
			}

			public async Task<ActionResult> Delete(Delete.Query query)
			{
				var model = await _mediator.SendAsync(query);

				return View(model);
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<ActionResult> Delete(Delete.Command command)
			{
				await _mediator.SendAsync(command);

				return this.RedirectToActionJson("Index");
			}
		}
	}
}