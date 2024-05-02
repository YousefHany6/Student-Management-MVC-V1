using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FStudentManagement.Controllers
{
	[Authorize(Roles = "Student")]

	public class stdDashboardController : Controller
	{
		private readonly IRepository<Attendance> att;
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

		public stdDashboardController(IRepository<Attendance> att, IRepository<StudentAttendance> stdatt, IRepository<Student> Student, UserManager<SUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<StudentTeacher> Student_Teacher, IRepository<TeacherCourse> T_course, IRepository<StudentCourse> Student_courses, IRepository<Course> Course, IRepository<ParentStage> parent, IRepository<ChildStage> child, IRepository<TermStage> Term)
		{
			this.att = att;
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

		public async Task<IActionResult> std_home()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Std_Courses(string user)
		{
			if (user == null)
			{
				return BadRequest("غير موجود فى النظام");
			}

			var student = await std.GetFirstOrDefaultAsync(s => s.userid == user);

			// Assuming you have a property in your Student class representing student ID
			var studentId = student.Student_ID;

			// Example: Get courses for the specific student
			var courses = await student_Courses.GetFilteredAsync(sc => sc.StudentID == studentId, c => c.Course);

   return View(courses);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> std_Absence(int courseid, int stdid)
		{
			var Teacher = await student_Teacher.filterone(
											st => st.CourseID == courseid && st.Student_ID == stdid,
											include: q => q.Include(t => t.Teacher)
							);
			var studentAttendance = await stdatt.GetFilteredAsync(
				a=>a.Attendance.Teacher_ID==Teacher.Teacher_ID&&
				a.Attendance.Course_ID==courseid&&
				a.StudentId==stdid,
				aa => aa.Attendance);
	
			return View(studentAttendance);
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
     return RedirectToAction(nameof(std_home));
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


 }
}
