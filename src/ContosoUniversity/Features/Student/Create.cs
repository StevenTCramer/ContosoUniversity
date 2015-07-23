namespace ContosoUniversity.Features.Student
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using FluentValidation;
	using MediatR;
	using Models;
	using Infrastructure;

	public class Create
	{
		public class Command : IRequest
		{
			public string LastName { get; set; }

			[Display(Name = "First Name")]
			public string FirstMidName { get; set; }

			public DateTime? EnrollmentDate { get; set; }
		}

		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(m => m.LastName).NotNull().Length(1, 50);
				RuleFor(m => m.FirstMidName).NotNull().Length(1, 50);
				RuleFor(m => m.EnrollmentDate).NotNull();
			}
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
				var student = Mapper.Map<Command, Student>(message);

				_db.Students.Add(student);
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
				return View(new Create.Command());
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