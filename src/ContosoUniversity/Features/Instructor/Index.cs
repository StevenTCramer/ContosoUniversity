namespace ContosoUniversity.Features.Instructor
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using AutoMapper;
	using DAL;
	using ContosoUniversity.Infrastructure;
	using MediatR;
	using Models;

	public class Index
	{
		public class Query : IAsyncRequest<Result>
		{
			public int? Id { get; set; }
			public int? CourseID { get; set; }
		}

		public class Result
		{
			public int? InstructorID { get; set; }
			public int? CourseID { get; set; }

			public List<Instructor> Instructors { get; set; }
			public List<Course> Courses { get; set; }
			public List<Enrollment> Enrollments { get; set; }

			public class Instructor
			{
				public int ID { get; set; }

				[Display(Name = "Last Name")]
				public string LastName { get; set; }

				[Display(Name = "First Name")]
				public string FirstMidName { get; set; }

				[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
				[Display(Name = "Hire Date")]
				public DateTime HireDate { get; set; }

				public string OfficeAssignmentLocation { get; set; }

				public IEnumerable<CourseInstructor> CourseInstructors { get; set; }
			}

			public class CourseInstructor
			{
				public int CourseID { get; set; }
				public string CourseTitle { get; set; }
			}

			public class Course
			{
				public int CourseID { get; set; }
				public string Title { get; set; }
				public string DepartmentName { get; set; }
			}

			public class Enrollment
			{
				[DisplayFormat(NullDisplayText = "No grade")]
				public Grade? Grade { get; set; }
				public string StudentFullName { get; set; }
			}
		}

		public class QueryHandler : IAsyncRequestHandler<Query, Result>
		{
			private readonly SchoolContext _db;

			public QueryHandler(SchoolContext db)
			{
				_db = db;
			}

			public async Task<Result> Handle(Query message)
			{
				var instructors = await _db.Instructors
						.OrderBy(i => i.LastName)
						.ProjectToListAsync<Result.Instructor>();

				var courses = new List<Result.Course>();
				var enrollments = new List<Result.Enrollment>();

				if (message.Id != null)
				{
					courses = await _db.CourseInstructors
							.Where(ci => ci.InstructorID == message.Id)
							.Select(ci => ci.Course)
							.ProjectToListAsync<Result.Course>();
				}

				if (message.CourseID != null)
				{
					enrollments = await _db.Enrollments
							.Where(x => x.CourseID == message.CourseID)
							.ProjectToListAsync<Result.Enrollment>();
				}

				var viewModel = new Result
				{
					Instructors = instructors,
					Courses = courses,
					Enrollments = enrollments,
					InstructorID = message.Id,
					CourseID = message.CourseID
				};

				return viewModel;
			}
		}

		public class UiController : MediatedController<Query, Result>
		{
			public UiController(IMediator aMediator) : base(aMediator){}

		}

	}
}