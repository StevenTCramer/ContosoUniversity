namespace ContosoUniversity.Features.Home
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using ContosoUniversity.DAL;

	public class About
	{
		public class UiController : Controller
		{
			private readonly SchoolContext schoolContext;

			public UiController(SchoolContext aSchoolContext)
			{
				schoolContext = aSchoolContext;
			}

			public ActionResult Request()
			{
				// Commenting out LINQ to show how to do the same thing in SQL.
				//IQueryable<EnrollmentDateGroup> = from student in db.Students
				//           group student by student.EnrollmentDate into dateGroup
				//           select new EnrollmentDateGroup()
				//           {
				//               EnrollmentDate = dateGroup.Key,
				//               StudentCount = dateGroup.Count()
				//           };

				// SQL version of the above LINQ code.
				string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
						+ "FROM Person "
						+ "WHERE Discriminator = 'Student' "
						+ "GROUP BY EnrollmentDate";
				IEnumerable<EnrollmentDateGroup> data = schoolContext.Database.SqlQuery<EnrollmentDateGroup>(query);

				return View(data.ToList());
			}

			public class EnrollmentDateGroup
			{
				[DataType(DataType.Date)]
				public DateTime? EnrollmentDate { get; set; }

				public int StudentCount { get; set; }
			}
		}
	}
}