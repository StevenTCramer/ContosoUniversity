﻿namespace ContosoUniversity.Features.Instructor
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using FluentValidation;
	using MediatR;

	public class Details
	{
		public class Query : IAsyncRequest<Model>
		{
			public int? Id { get; set; }
		}

		public class Validator : AbstractValidator<Query>
		{
			public Validator()
			{
				RuleFor(m => m.Id).NotNull();
			}
		}

		public class Model
		{
			public int? ID { get; set; }

			public string LastName { get; set; }
			[Display(Name = "First Name")]
			public string FirstMidName { get; set; }

			[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
			public DateTime? HireDate { get; set; }

			[Display(Name = "Location")]
			public string OfficeAssignmentLocation { get; set; }
		}

		public class Handler : IAsyncRequestHandler<Query, Model>
		{
			private readonly SchoolContext _db;

			public Handler(SchoolContext db)
			{
				_db = db;
			}

			public async Task<Model> Handle(Query message)
			{
				return await _db.Instructors.Where(i => i.ID == message.Id).ProjectToSingleOrDefaultAsync<Model>();
			}
		}

		public class UiController : Controller
		{
			private readonly IMediator _mediator;

			public UiController(IMediator mediator)
			{
				_mediator = mediator;
			}

			public async Task<ActionResult> Details(Details.Query query)
			{
				var model = await _mediator.SendAsync(query);

				return View(model);
			}
		}
	}
}