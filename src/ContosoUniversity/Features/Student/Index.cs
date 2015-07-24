﻿namespace ContosoUniversity.Features.Student
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using DAL;
	using ContosoUniversity.Infrastructure;
	using Infrastructure.Mapping;
	using MediatR;
	using Models;
	using PagedList;

	public class Index
	{
		public class Query : IAsyncRequest<Result>
		{
			public string SortOrder { get; set; }
			public string CurrentFilter { get; set; }
			public string SearchString { get; set; }
			public int? Page { get; set; }
		}

		public class Result : IAsyncRequest
		{
			public string CurrentSort { get; set; }
			public string NameSortParm { get; set; }
			public string DateSortParm { get; set; }
			public string CurrentFilter { get; set; }
			public string SearchString { get; set; }

			public IPagedList<Student> Results { get; set; }

			public class Student
			{
				public int ID { get; set; }
				[Display(Name = "First Name")]
				public string FirstMidName { get; set; }
				public string LastName { get; set; }
				public DateTime EnrollmentDate { get; set; }
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
				var model = new Result
				{
					CurrentSort = message.SortOrder,
					NameSortParm = String.IsNullOrEmpty(message.SortOrder) ? "name_desc" : "",
					DateSortParm = message.SortOrder == "Date" ? "date_desc" : "Date",
				};

				if (message.SearchString != null)
				{
					message.Page = 1;
				}
				else
				{
					message.SearchString = message.CurrentFilter;
				}

				model.CurrentFilter = message.SearchString;
				model.SearchString = message.SearchString;

				var students = from s in _db.Students
											 select s;
				if (!String.IsNullOrEmpty(message.SearchString))
				{
					students = students.Where(s => s.LastName.Contains(message.SearchString)
																				 || s.FirstMidName.Contains(message.SearchString));
				}
				switch (message.SortOrder)
				{
					case "name_desc":
						students = students.OrderByDescending(s => s.LastName);
						break;
					case "Date":
						students = students.OrderBy(s => s.EnrollmentDate);
						break;
					case "date_desc":
						students = students.OrderByDescending(s => s.EnrollmentDate);
						break;
					default: // Name ascending 
						students = students.OrderBy(s => s.LastName);
						break;
				}

				int pageSize = 3;
				int pageNumber = (message.Page ?? 1);
				model.Results = students.ProjectToPagedList<Result.Student>(pageNumber, pageSize);

				return model;
			}
		}

		public class UiController : MediatedController<Query, Result>
		{
			public UiController(IMediator mediator) : base(mediator) { }

		}
	}
}