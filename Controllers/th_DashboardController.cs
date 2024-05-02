using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FStudentManagement.Controllers
{
	[Authorize(Roles = "Teacher")]
	public class th_DashboardController : Controller
	{
		private readonly IRepository<Attendance> att;
		private readonly IRepository<Teacher> tH;
		private readonly IRepository<StudentAttendance> stdatt;
		private readonly IRepository<Student> std;
		private readonly UserManager<SUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IRepository<StudentTeacher> student_Teacher;
		private readonly IRepository<TeacherCourse> course1;
		private readonly IRepository<StudentCourse> student_Courses;
		private readonly IRepository<Course> course;
		private readonly IRepository<ParentStage> parent;
		private readonly IRepository<ChildStage> child;
		private readonly IRepository<TermStage> term;

		public th_DashboardController(IRepository<Attendance> att, IRepository<Teacher> TH, IRepository<StudentAttendance> stdatt, IRepository<Student> Student, UserManager<SUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<StudentTeacher> Student_Teacher, IRepository<TeacherCourse> T_course, IRepository<StudentCourse> Student_courses, IRepository<Course> Course, IRepository<ParentStage> parent, IRepository<ChildStage> child, IRepository<TermStage> Term)
		{
			this.att = att;
			tH = TH;
			this.stdatt = stdatt;
			std = Student;
			this.userManager = userManager;
			this.roleManager = roleManager;
			student_Teacher = Student_Teacher;
			course1 = T_course;
			student_Courses = Student_courses;
			course = Course;
			this.parent = parent;
			this.child = child;
			term = Term;
		}

		public async Task<IActionResult> th_home()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> th_Courses(string user)
		{
			if (user == null)
			{
				return BadRequest("غير موجود فى النظام");
			}

			var Teacher = await tH.GetFirstOrDefaultAsync(s => s.userid == user);

			// Assuming you have a property in your Student class representing student ID
			var Teacherid = Teacher.Teacher_ID;

			// Example: Get courses for the specific student
			var courses = await course1.GetFilteredAsync(sc => sc.Teacher_ID == Teacherid, c => c.Course.ParentStage, cc => cc.Course.ChildStage, ccs => ccs.Course.TermStage);

			return View(courses);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> th_student(int teacherid, int courseid)
		{


			var students = await student_Teacher.GetFilteredAsync(s => s.Teacher_ID == teacherid && s.CourseID == courseid, ss => ss.Student);


			return View(students);
		}
		public async Task<IActionResult> change_password()
		{
			return View();
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> change_password(string userid, string Npassword, string Cpassword)
		{
			var user = await userManager.FindByIdAsync(userid);

			if (user == null)
			{
				return BadRequest();
			}

			// Validate the current password against the stored hash
			if (await userManager.CheckPasswordAsync(user, Cpassword))
			{
				// Change the password
				var newPasswordHash = userManager.PasswordHasher.HashPassword(user, Npassword);
				user.PasswordHash = newPasswordHash;

				// Update the user
				var result = await userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(th_home));
				}
				else
				{
					// Handle update failure, e.g., log the errors
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, "الباسورد الحالي غير صحيح");
			}

			return View();
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> th_table(string user)
		{
			var Teacher = await tH.GetFirstOrDefaultAsync(s => s.userid == user);

			// Assuming you have a property in your Student class representing student ID
			var Teacherid = Teacher.Teacher_ID;
			ViewBag.thid = Teacherid;
			// Example: Get courses for the specific student
			var courses = await course1.GetFilteredAsync(sc => sc.Teacher_ID == Teacherid, c => c.Course.ParentStage, cc => cc.Course.ChildStage, ccs => ccs.Course.TermStage);


			return View(courses);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> create_table(DateTime datenow, int thid, int courseid)
		{
			if (thid == 0 || courseid == 0)
			{
				return NotFound("يرجى اختيار مادة");

			}

			var students = await student_Teacher.GetFilteredAsync(ss => ss.Teacher_ID == thid && ss.CourseID == courseid, s => s.Student);

			ViewBag.th = thid;
			ViewBag.date = datenow;
			ViewBag.co = courseid;
			var namecourse = await course.GetById(courseid);
			ViewBag.name = namecourse.Course_Name;

			return View("attendance", students);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> att_done(int thidd, DateTime ddate, int cid, List<int> attendanceStates, List<int> stds)
		{
			var ta = new Attendance()
			{
				Date = ddate,
				Course_ID = cid,
				Teacher_ID = thidd
			};

			await att.Create(ta); // Assuming 'att' is your Attendance repository or service

			foreach (var st in stds)
			{
				bool isPresent = attendanceStates.Contains(st);

				var st_std = new StudentAttendance
				{
					AttendanceId = ta.AttendanceId,
					StudentId = st,
					IsPresent = isPresent
				};

				// Save 'st_std' to your database or repository

				// Example of saving to a repository
				await stdatt.Create(st_std);
			}

			return RedirectToAction("th_home");
		}



	}
}
