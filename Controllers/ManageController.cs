using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using System.Linq.Expressions;

namespace FStudentManagement.Controllers
{
	[Authorize(Roles = "Admin")]

	public class ManageController : Controller
 {
  private readonly ApplicationDbContext dbContext;
  private readonly IRepository<Attendance> att;
  private readonly IRepository<StudentAttendance> stdatt;
  private readonly IRepository<Student> std;
		private readonly UserManager<SUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IRepository<TeacherCourse> course_Teacher;
		private readonly IRepository<TeacherStageCourse> teacherStage;
		private readonly IRepository<Teacher> Teacher;
		private readonly IRepository<ParentStage> parent;
		private readonly IRepository<ChildStage> child;
		private readonly IRepository<TermStage> term;
		private readonly IRepository<Course> Course;


		public ManageController(ApplicationDbContext dbContext,IRepository<Attendance>  att, IRepository<StudentAttendance> stdatt,IRepository<Student> Student,UserManager<SUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<TeacherCourse> Course_Teacher, IRepository<TeacherStageCourse> TeacherStage, IRepository<Teacher> Teacher, IRepository<Course> Course, IRepository<ParentStage> parent, IRepository<ChildStage> child, IRepository<TermStage> Term)
		{
   this.dbContext = dbContext;
   this.att = att;
   this.stdatt = stdatt;
   std = Student;
			this.userManager = userManager;
			this.roleManager = roleManager;
			course_Teacher = Course_Teacher;
			teacherStage = TeacherStage;
			this.Teacher = Teacher;
			this.parent = parent;
			this.child = child;
			term = Term;
			this.Course = Course;



		}


		public IActionResult Index()
  {
   return View();
  }
  public async Task<IActionResult> Create_Table()
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

  public async Task<IActionResult> create_table(DateTime datenow, int teacherid, int courseid, int termid, int childid, int parentid)
		{
   if (teacherid==0&& courseid == 0&& termid == 0&& childid == 0&& parentid == 0)
   {
				ModelState.AddModelError(string.Empty, "يرجى انشاء جدول صحيح");
				ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
				ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
				ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
				ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
				ViewBag.teachers = new SelectList(await Teacher.GetAll(), "Teacher_ID", "Teacher_Name");
				return View("Create_Table");

			}
			Expression<Func<Student, bool>> filter = s =>
							s.StudentTeachers.Any(st => st.Teacher_ID == teacherid) &&
							s.StudentCourses.Any(sc => sc.CourseID == courseid) &&
							s.TermStageId == termid &&
							s.ChildStageId == childid &&
							s.ParentStageId == parentid;

			var students = await std.GetFilteredAsync(filter, s => s.StudentCourses, s => s.StudentTeachers, s => s.TermStage, s => s.ChildStage, s => s.ParentStage);

   ViewBag.th = teacherid;
   ViewBag.date = datenow;
   ViewBag.co = courseid;

   return View("attendance", students);
		}
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> attendance(List<int> attendanceStates, List<int> stds, int courseid1, int teacherid1, DateTime datenow1)
  {
   var ta = new Attendance()
   {
    Date = datenow1,
    Course_ID = courseid1,
    Teacher_ID = teacherid1
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

   return RedirectToAction("Create_Table");
  }

  public async Task<IActionResult> Edit_attendance()
  {
   ViewBag.att = new SelectList(await att.GetAll(),"AttendanceId", "Date");
   ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
   ViewBag.teachers = new SelectList(await Teacher.GetAll(), "Teacher_ID", "Teacher_Name");


   return View();
  }
  public async Task<IActionResult> GetDatesByTeacherAndCourse(int teacherID, int courseID)
  {
   try
   {
    // Fetch the dates based on the provided teacher and course IDs
    var dates = await att.GetFilteredAsync(
               filter: a => a.Teacher_ID == teacherID && a.Course_ID == courseID
      
           );
    // You might want to select only necessary fields from dates to avoid circular references
    ViewBag.att = new SelectList(dates, "AttendanceId", "Date");

    // Return the dates as JSON
    return Json(ViewBag.att);
   }
   catch (Exception ex)
   {
    // Handle exceptions appropriately
    return BadRequest(ex.Message);
   }
  }


  public async Task<IActionResult> GetTeachersByCourse(int courseID)
		{
			try
			{
				// Fetch the teachers based on the provided course ID
				var teachers = await Teacher.GetFilteredAsync(
								t => t.TeacherCourses.Any(tc => tc.Course_ID == courseID),
								t => t.TeacherCourses // Assuming this represents the collection navigation property in Teacher entity
				);
    ViewBag.teachers = new SelectList( teachers, "Teacher_ID", "Teacher_Name");

    // Return the teachers as JSON
    return Json(ViewBag.teachers);
			}
			catch (Exception ex)
			{
				// Handle exceptions appropriately
				return BadRequest(ex.Message);
			}
		}
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit_attendance(int Course_ID, int Teacher_ID, int tableid)
  {
   try
   {
    // Fetch the attendance based on the tableid

    if (tableid != 0)
    {
     ViewBag.table = tableid;
     // Assuming StudentAttendances is a navigation property in your Attendance model
     var studentAttendances = await stdatt.GetFilteredAsync(sa => sa.AttendanceId == tableid, sa => sa.Student);

     // Assuming you want to display the student attendances in a view called "display_att"
     return View("display_att", studentAttendances);
    }
    else
    {
					// Handle scenario where no attendance is found
					ModelState.AddModelError(string.Empty, "يرجى اختيار جدول صحيح");
					ViewBag.att = new SelectList(await att.GetAll(), "AttendanceId", "Date");
					ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
					ViewBag.teachers = new SelectList(await Teacher.GetAll(), "Teacher_ID", "Teacher_Name");

					return View("Edit_attendance");
				}
   }
   catch (Exception ex)
   {
    // Handle exceptions appropriately
    return BadRequest(ex.Message);
   }
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> edit_done(List<int> students,List<int> attstate,int tabid)
  {

   foreach (var st in students)
   {
    bool isPresent = attstate.Contains(st);

    var st_std = new StudentAttendance
    {
     AttendanceId = tabid,
     StudentId = st,
     IsPresent = isPresent
    };
    await stdatt.Update(st_std);
   }

   return RedirectToAction("Edit_attendance");
  
 }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> delete_Table(int id)
  {
   if (id != 0)
   {
   var ta=await att.GetById(id);
    await stdatt.DeleteWhere(s=>s.AttendanceId==id);
    await att.Delete(ta);
    return RedirectToAction("Edit_attendance");

   }
   else
   {
    ModelState.AddModelError(string.Empty, "يرجى اختيار جدول صحيح");
    ViewBag.att = new SelectList(await att.GetAll(), "AttendanceId", "Date");
    ViewBag.courses = new SelectList(await Course.GetAll(), "Course_ID", "Course_Name");
    ViewBag.teachers = new SelectList(await Teacher.GetAll(), "Teacher_ID", "Teacher_Name");

    return View("Edit_attendance");

   }

  }



 }
}
