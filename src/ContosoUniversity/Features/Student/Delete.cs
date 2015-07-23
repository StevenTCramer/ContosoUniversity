namespace ContosoUniversity.Features.Student
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using Infrastructure;	
	using MediatR;

	public class Delete
	{
		public class Query : IAsyncRequest<Command>
		{
			public int Id { get; set; }
		}

		public class Command : IAsyncRequest
		{
			public int ID { get; set; }
			[Display(Name = "First Name")]
			public string FirstMidName { get; set; }
			public string LastName { get; set; }
			public DateTime EnrollmentDate { get; set; }
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
				return await _db.Students.Where(s => s.ID == message.Id).ProjectToSingleOrDefaultAsync<Command>();
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
				var student = await _db.Students.FindAsync(message.ID);

				_db.Students.Remove(student);
			}
		}

		public class UiController : MediatedController
		{
			public UiController(IMediator mediator) : base(mediator) {}
			
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

				return this.RedirectToActionJson("Index", "Student");
			}
		}

	}
}