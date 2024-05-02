using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NuGet.DependencyResolver;
using System.Linq.Expressions;

namespace FStudentManagement.Controllers
{
 [Authorize(Roles = "Admin")]

 public class StudentsController : Controller
 {
		private readonly IRepository<StudentAttendance> studentatt;
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

  public StudentsController(IRepository<StudentAttendance> Studentatt,IRepository<Student> Student, UserManager<SUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<StudentTeacher> Student_Teacher, IRepository<TeacherCourse> T_course, IRepository<StudentCourse> Student_courses, IRepository<Course> Course, IRepository<ParentStage> parent, IRepository<ChildStage> child, IRepository<TermStage> Term)
  {
			studentatt = Studentatt;
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
  public async Task<IActionResult> Add_Student()
  {
   ViewBag.courses = new SelectList(await course.GetAll(), "Course_ID", "Course_Name");
   ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
   ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
   ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
   return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Add_Student(Student student, List<int> CoursesList)
  {
   if (ModelState.IsValid && student != null)
   {
    var email = await userManager.FindByEmailAsync(student.SUser.Email);
    if (email != null)
    {
     ModelState.AddModelError(string.Empty, "هذا الايميل موجود يرجى تغيير الايميل الحالى");
     ViewBag.courses = new SelectList(await course.GetAll(), "Course_ID", "Course_Name");
     ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
     ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
     ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
     return View();
    }
    if (student.SUser.PasswordHash == null)
    {
     student.SUser.PasswordHash = student.SUser.Email;
    }
    var user = new SUser
    {
     FullName = student.Student_Name.TrimStart().TrimEnd(),
     Email = student.SUser.Email.TrimStart().TrimEnd(),
     UserName = student.SUser.Email.TrimStart().TrimEnd()
    };
    var result = await userManager.CreateAsync(user, student.SUser.PasswordHash);

    if (result.Succeeded)
    {
     await userManager.AddToRoleAsync(user, "Student");

     student.SUser = user;
     await std.Create(student);

     foreach (var courseId in CoursesList)
     {
      var StudentCourse = new StudentCourse
      {
       StudentID = student.Student_ID, // Ensure this property is set correctly
       CourseID = courseId
      };

      await student_Courses.Create(StudentCourse);

      var teachersForCourse = await course1.teacherforcourse(courseId);
 
      if (teachersForCourse == null)
      {
       await std.Delete(student);
       await userManager.DeleteAsync(user);
       return View("Errorstd");

      }
      foreach (var STDT in teachersForCourse)
      {

       var studentTeacher = new StudentTeacher
       {
        Student_ID = student.Student_ID,
        Teacher_ID = STDT.Teacher_ID,
        CourseID=courseId

       };
       await student_Teacher.Create(studentTeacher);

      }
     }
    }
    return RedirectToAction(nameof(Add_Student));


   }

   return View(student);
  }


  public async Task<IActionResult> display_Student()
  {
   ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
   ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
   ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
   return View();
  }

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> display_Student( int parentid, int childid, int termid)
		{
			ViewBag.courses = new SelectList(await course.GetAll(), "Course_ID", "Course_Name");
			ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
			ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
			ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");

			Expression<Func<Student, bool>> filter = t =>
														t.ParentStageId == parentid && t.ChildStageId == childid && t.TermStageId == termid;

			// Eager loading TeacherCourses relationship
			var studentWithCourses = await std.GetFilteredAsync(
							filter,
							e => e.StudentCourses
			);
			return View(studentWithCourses);
		}
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> delete_Student(int id)
  {
   var stud = await std.GetById(id);

   if (stud != null && stud.userid != null)
   {
    await student_Courses.DeleteWhere(s => s.StudentID == id);
    await student_Teacher.DeleteWhere(s => s.Student_ID == id);
    await studentatt.DeleteWhere(s => s.StudentId == id);
				await std.Delete(stud);
    var user = await userManager.FindByIdAsync(stud.userid);

    if (user != null)
    {
     var result = await userManager.DeleteAsync(user);

     if (result.Succeeded)
     {
      // Redirect to Teachers action with the same stage parameters
      return RedirectToAction("display_Student");
     }
     // Handle failure to delete user
    }
   }

   // Redirect to Teachers action with the same stage parameters
   return RedirectToAction("display_Student");
  }

		public async Task<IActionResult> Edit_Student(int id)
  {
			var getid = await std.GetById(id);
			if (getid != null)
			{
				var user = await userManager.FindByIdAsync(getid.userid);
				ViewBag.User = user;


				var stud = await std.GetByIdWithRelatedEntitiesAsync(
				t => t.Student_ID == id,
				t => t.StudentCourses
);

				if (stud != null)
				{
					var allCourses = await course.GetAll(); // Assuming this retrieves all courses

					ViewBag.courses = allCourses.Select(course => new SelectListItem
					{
						Value = course.Course_ID.ToString(),
						Text = course.Course_Name,
						Selected = stud.StudentCourses.Any(tc => tc.Course.Course_Name == course.Course_Name)
					}).ToList();

					ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name", getid.ParentStage);
					ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name", getid.ChildStage);
					ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name", getid.TermStage);

					return View("Edit_Student", getid);
				}
			}

			return RedirectToAction("Teachers");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit_Student(Student student, List<int> CoursesList)
  {
   if (ModelState.IsValid && student != null && student.Student_ID != 0)
   {
    var user = await userManager.FindByIdAsync(student.userid);

    if (user != null)
    {
     // Update Teacher entity
     user.FullName = student.Student_Name.TrimStart().TrimEnd();
     user.UserName = student.SUser.Email.TrimStart().TrimEnd();
     user.Email = student.SUser.Email.TrimStart().TrimEnd();
     user.PasswordHash = student.SUser.PasswordHash;
     // Note: It's better to handle password changes through a separate mechanism for security reasons

     var updateResult = await userManager.UpdateAsync(user);

     if (updateResult.Succeeded)
     {
      var stu = await std.GetById(student.Student_ID);
      if (student.ChildStageId==0&&student.TermStageId==0)
      {
       student.ChildStageId = stu.ChildStageId;
       student.TermStageId = stu.TermStageId;
						}

						// Update Teacher details
						await std.Update(student);

      // Update associated courses
      if (CoursesList != null && CoursesList.Any())
      {
       // Remove existing courses associated with the teacher
       await student_Courses.DeleteWhere(tc => tc.StudentID == student.Student_ID);

       // Add new courses to the teacher
       foreach (var courseId in CoursesList)
       {
        var newTeacherCourse = new StudentCourse
        {
         StudentID = student.Student_ID,
         CourseID = courseId
        };

        await student_Courses.Create(newTeacherCourse);
       }
      }

      return RedirectToAction("display_Student");
     }
    }
   }

			return RedirectToAction("display_Student");
		}

	}
}
