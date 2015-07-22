namespace ContosoUniversity.Features.Student
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
	using System.Web.Mvc;
	using DAL;
	using Infrastructure.Mapping;
    using MediatR;
    using Models;

	public class Details
	{
		public class Query : IAsyncRequest<Model>
		{
			public int Id { get; set; }
		}

		public class Model
		{
			public int ID { get; set; }
			[Display(Name = "First Name")]
			public string FirstMidName { get; set; }
			public string LastName { get; set; }
			public DateTime EnrollmentDate { get; set; }
			public List<Enrollment> Enrollments { get; set; }

			public class Enrollment
			{
				public string CourseTitle { get; set; }
				public Grade? Grade { get; set; }
			}
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
				return await _db.Students.Where(s => s.ID == message.Id).ProjectToSingleOrDefaultAsync<Model>();
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