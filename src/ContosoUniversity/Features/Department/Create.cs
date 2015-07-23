﻿namespace ContosoUniversity.Features.Department
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using FluentValidation;
	using MediatR;
	using Models;
	using Infrastructure;

	public class Create
	{
		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(m => m.Name).NotNull().Length(3, 50);
				RuleFor(m => m.Budget).NotNull();
				RuleFor(m => m.StartDate).NotNull();
				RuleFor(m => m.Administrator).NotNull();
			}
		}

		public class Command : IRequest
		{
			[StringLength(50, MinimumLength = 3)]
			public string Name { get; set; }

			[DataType(DataType.Currency)]
			[Column(TypeName = "money")]
			public decimal? Budget { get; set; }

			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
			public DateTime? StartDate { get; set; }

			public Instructor Administrator { get; set; }
		}

		public class CommandHandler : RequestHandler<Command>
		{
			private readonly SchoolContext _context;

			public CommandHandler(SchoolContext context)
			{
				_context = context;
			}

			protected override void HandleCore(Command message)
			{
				var department = Mapper.Map<Command, Department>(message);

				_context.Departments.Add(department);
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
			public ActionResult Create(Create.Command model)
			{
				_mediator.Send(model);

				return this.RedirectToActionJson("Index");
			}
		}
	}
}