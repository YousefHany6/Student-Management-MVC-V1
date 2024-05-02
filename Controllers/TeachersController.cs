using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace FStudentManagement.Controllers
{
 [Authorize(Roles = "Admin")]
 public class TeachersController : Controller
 {
  private readonly UserManager<SUser> userManager;
  private readonly RoleManager<IdentityRole> roleManager;
  private readonly IRepository<TeacherCourse> course_Teacher;
		private readonly IRepository<Attendance> att_Teacher;
		private readonly IRepository<StudentTeacher> std_Teacher;
		private readonly IRepository<TeacherStageCourse> teacherStage;
  private readonly IRepository<Teacher> Teacher;
  private readonly IRepository<ParentStage> parent;
  private readonly IRepository<ChildStage> child;
  private readonly IRepository<TermStage> term;
  private readonly IRepository<Course> Course;


  public TeachersController(UserManager<SUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<TeacherCourse> Course_Teacher, IRepository<Attendance> att_Teacher, IRepository<StudentTeacher> std_Teacher, IRepository<TeacherStageCourse> TeacherStage, IRepository<Teacher> Teacher, IRepository<Course> Course, IRepository<ParentStage> parent, IRepository<ChildStage> child, IRepository<TermStage> Term)
  {
   this.userManager = userManager;
   this.roleManager = roleManager;
   course_Teacher = Course_Teacher;
			this.att_Teacher = att_Teacher;
			this.std_Teacher = std_Teacher;
			teacherStage = TeacherStage;
   this.Teacher = Teacher;
   this.parent = parent;
   this.child = child;
   term = Term;
   this.Course = Course;



  }
  public async Task<IActionResult> display_th()
  {
   return View(await Teacher.GetAll());
  }
  public async Task<IActionResult> Add_Teacher()
  {
   ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
   ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
   ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
   ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
   return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Add_Teacher(Teacher teacher, List<int> CoursesList, List<int> parentid, List<int> childid, List<int> termid)
  {
   if (ModelState.IsValid && teacher != null)
   {
    var email = await userManager.FindByEmailAsync(teacher.SUser.Email);
    if (email != null)
    {
     ModelState.AddModelError(string.Empty, "هذا الايميل موجود يرجى تغيير الايميل الحالى");
     ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
     ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
     ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
     ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
     return View();
    }
    if (teacher.SUser.PasswordHash == null)
    {
     teacher.SUser.PasswordHash = teacher.SUser.Email;
    }
    var user = new SUser
    {
     FullName = teacher.Teacher_Name.TrimStart().TrimEnd(),
     Email = teacher.SUser.Email.TrimStart().TrimEnd(),
     UserName = teacher.SUser.Email.TrimStart().TrimEnd()
    };
    var result = await userManager.CreateAsync(user, teacher.SUser.PasswordHash);

    if (result.Succeeded)
    {
     await userManager.AddToRoleAsync(user, "Teacher");

     teacher.SUser = user;
     await Teacher.Create(teacher);

     foreach (var courseId in CoursesList)
     {

      var teachercourse = new TeacherCourse
      {
       Teacher_ID = teacher.Teacher_ID, // Ensure this property is set correctly
       Course_ID = courseId
      };

      await course_Teacher.Create(teachercourse);
      foreach (var parent in parentid)
      {
       foreach (var child in childid)
       {
        foreach (var term in termid)
        {
         var TeacherStageCourse = new TeacherStageCourse
         {
          TeacherId = teacher.Teacher_ID,
          ParentStageId = parent,
          ChildStageId = child,
          TermStageId = term,
          CourseId = courseId
         };
         await teacherStage.Create(TeacherStageCourse);


        }

       }

      }


     }
    }

    return RedirectToAction(nameof(Add_Teacher));


   }

   return View(teacher);
  }

  public async Task<IActionResult> display_Teacher()
  {
   ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");

   ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
   ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
   ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
   ViewBag.teachers = new SelectList(await Teacher.GetAll(), "Teacher_ID", "Teacher_Name");
   return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> display_Teacher(int parentid, int childid, int termid, int teacherid, int courseid)
  {
   ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name", courseid);

   ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name", parentid);
   ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name", childid);
   ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name", termid);
   ViewBag.teachers = new SelectList(await Teacher.GetAll(), "Teacher_ID", "Teacher_Name", teacherid);

   var par = await parent.GetById(parentid);
   var coo = await Course.GetById(courseid);
			ViewBag.parent = par.Name;
			ViewBag.cours = coo.Course_Name;
			ViewBag.par = parentid;
   ViewBag.ch = childid;
   ViewBag.te = termid;
   ViewBag.co = courseid;

			return View(await Teacher.GetById(teacherid));
  }

  [HttpGet]
  public async Task<IActionResult> GetChildStages(int parentId)
  {
   Expression<Func<ChildStage, bool>> filter = c => c.ParentStageId == parentId;

   var childStages = await child.GetFilteredAsync(filter);
   ViewBag.Childs = new SelectList(childStages, "ChildStageId", "Name");
   return Json(ViewBag.Childs);
  }

  [HttpGet]
  public async Task<IActionResult> GetTermStages(int childId)
  {
   Expression<Func<TermStage, bool>> filter = t => t.ChildStageId == childId;

   var termStages = await term.GetFilteredAsync(filter);
   ViewBag.terms = new SelectList(termStages, "TermStageId", "Name");
   return Json(ViewBag.terms);
  }
  [HttpGet]
  public async Task<IActionResult> GetCourses(int parentId, int childId, int termId)
  {
   // Retrieve courses based on parent, child, and term IDs
   var courses = await Course.GetFilteredAsync(c => c.ParentStageId == parentId &&
                                                          c.ChildStageId == childId &&
                                                          c.TermStageId == termId);

   var courseIds = courses.Select(c => c.Course_ID).ToList();

   // Retrieve course names using the obtained course IDs
   var courseNames = await Course.GetFilteredAsync(c => courseIds.Contains(c.Course_ID));

   // Create a list of SelectListItem to send back to the view
   var courseList = courseNames.Select(c => new SelectListItem
   {
    Value = c.Course_ID.ToString(),
    Text = c.Course_Name
   }).ToList();

   return Json(courseList);
  }

  [HttpGet]
  public async Task<IActionResult> GetTeachers(int parentID, int childID, int termID, int courseID)
  {
   var teacherStageCourses = await teacherStage.GetFilteredAsync(tc =>
       tc.ParentStageId == parentID &&
       tc.ChildStageId == childID &&
       tc.TermStageId == termID &&
       tc.CourseId == courseID);

   var teacherIds = teacherStageCourses.Select(tc => tc.TeacherId).Distinct();

   var filteredTeachers = await Teacher.GetFilteredAsync(t => teacherIds.Contains(t.Teacher_ID)); // Include TeacherStageCourses if necessary

   var teacherList = filteredTeachers.Select(teacher => new SelectListItem
   {
    Value = teacher.Teacher_ID.ToString(),
    Text = teacher.Teacher_Name
   }).ToList();

   return Json(teacherList);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete_Teacher_System(int id,int parentid,int childid,int termid,int courseid)
  {
   var teacher = await Teacher.GetById(id);
   if (teacher != null && teacher.userid != null)
   {
    Expression<Func<TeacherStageCourse, bool>> condition = tsc =>
                   tsc.TeacherId == id;
    await course_Teacher.DeleteWhere(e=>e.Teacher_ID==id);								
    await teacherStage.DeleteWhere(condition);
    await att_Teacher.DeleteWhere(s => s.Teacher_ID == id);
    await std_Teacher.DeleteWhere(s => s.Teacher_ID == id);
				await Teacher.Delete(teacher);
    var user = await userManager.FindByIdAsync(teacher.userid);

    if (user != null)
    {
     var result = await userManager.DeleteAsync(user);

     if (result.Succeeded)
     {
      // Redirect to Teachers action with the same stage parameters
      return RedirectToAction("display_Teacher");
     }
    }

   }
   ModelState.AddModelError(Empty.ToString(), "خطأ لم يتم الحذف بشكل صحيح");
     return View("display_Teacher");
  }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete_Teacher_Stage(int id, int parentid, int childid, int termid, int courseid)
		{
			var teacher = await Teacher.GetById(id);
			if (teacher != null && teacher.userid != null)
			{
				Expression<Func<TeacherStageCourse, bool>> condition = tsc =>
																tsc.TeacherId == id &&
																tsc.ParentStageId == parentid &&
																tsc.ChildStageId == childid &&
																tsc.TermStageId == termid &&
																tsc.CourseId == courseid;

				await teacherStage.DeleteWhere(condition);
				Expression<Func<TeacherCourse, bool>> condition2 = tsc =>
												tsc.Teacher_ID == id &&
												tsc.Course_ID == courseid;
				await course_Teacher.DeleteWhere(condition2);


				// Redirect to Teachers action with the same stage parameters
				return RedirectToAction("display_Teacher");
					}
				

			
			ModelState.AddModelError(Empty.ToString(), "خطأ لم يتم الحذف بشكل صحيح");
			return View("display_Teacher");
		}

  public async Task<IActionResult> addnewstage()
  {
			ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
			ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
			ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
			ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
			return View();
  }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> addnewstage(string emailth, int parid,int chiid,int terid, List<int> CoursesList)
		{
   if (!ModelState.IsValid)
   {
    return BadRequest(ModelState);
   }
   var user=await userManager.FindByEmailAsync(emailth);
   if (user == null)
   {
    ModelState.AddModelError(string.Empty, "هذا الايميل غير موجود");
				ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
				ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
				ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
				ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
				return View();
			}

   var teacher = await Teacher.filterone(s=>s.userid==user.Id);
			foreach (var courseId in CoursesList)
			{
    var checkcoures=await course_Teacher.GetByCondition(ss=>ss.Course_ID==courseId&&ss.Teacher_ID==teacher.Teacher_ID);
    if (checkcoures.Any())
    {
     continue;
    }
				var teachercourse = new TeacherCourse
				{
					Teacher_ID = teacher.Teacher_ID, 
					Course_ID = courseId
				};

				await course_Teacher.Create(teachercourse);

							var TeacherStageCourse = new TeacherStageCourse
							{
								TeacherId = teacher.Teacher_ID,
								ParentStageId = parid,
								ChildStageId = chiid,
								TermStageId = terid,
								CourseId = courseId
							};
							await teacherStage.Create(TeacherStageCourse);
			}
			return RedirectToAction("addnewstage");
		}


	}
}


