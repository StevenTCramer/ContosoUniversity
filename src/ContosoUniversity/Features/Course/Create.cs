namespace ContosoUniversity.Features.Course
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using MediatR;
	using Models;
	using Infrastructure;

	public class Create
	{
		public class Command : IRequest
		{
			[Display(Name = "Number")]
			public int CourseID { get; set; }
			public string Title { get; set; }
			public int Credits { get; set; }
			public Department Department { get; set; }
		}

		public class Handler : RequestHandler<Command>
		{
			private readonly SchoolContext _db;

			public Handler(SchoolContext db)
			{
				_db = db;
			}

			protected override void HandleCore(Command message)
			{
				var course = Mapper.Map<Command, Course>(message);

				_db.Courses.Add(course);
			}
		}

		public class UiController : Controller
		{
			private readonly IMediator _mediator;

			public UiController(IMediator mediator)
			{
				_mediator = mediator;
			}

			public ActionResult Create()
			{
				return View();
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public ActionResult Create(Create.Command command)
			{
				_mediator.Send(command);

				return this.RedirectToActionJson("Index");
			}
		}
	}
}