using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.Types;
using System.Linq.Expressions;

namespace FStudentManagement.Controllers
{
	[Authorize(Roles = "Admin")]

	public class CoursesController : Controller
	{
		private readonly IRepository<TeacherStageCourse> teacherStage;
		private readonly IRepository<StudentCourse> stdcou;
		private readonly IRepository<Attendance> att_Course;
		private readonly IRepository<TeacherCourse> c1;
		private readonly IRepository<Course> Course;
		private readonly IRepository<ParentStage> parent;
		private readonly IRepository<ChildStage> child;
		private readonly IRepository<TermStage> term;

		public CoursesController(IRepository<TeacherStageCourse> TeacherStage, IRepository<StudentCourse> stdcou, IRepository<Attendance> att_course, IRepository<TeacherCourse> c1, IRepository<Course> Course, IRepository<ParentStage> parent, IRepository<ChildStage> child, IRepository<TermStage> Term)
		{
			teacherStage = TeacherStage;
			this.stdcou = stdcou;
			att_Course = att_course;
			this.c1 = c1;
			this.Course = Course;
			this.parent = parent;
			this.child = child;
			term = Term;
		}
		public async Task<IActionResult> Courses()
		{
			ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
			ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
			ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
			return View(await Course.GetAll());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create_Course(Course course)
		{
			if (ModelState.IsValid)
			{
				var check=await Course.GetByCondition(s=>s.Course_Name==course.Course_Name.TrimStart().TrimEnd() && s.ParentStageId==course.ParentStageId&&s.ChildStageId==course.ChildStageId&&s.TermStageId==course.TermStageId);
				if (check.Any())
				{
					ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name");
					ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
					ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name");
					ModelState.AddModelError(string.Empty,"هذه المادة موجودة");
					return View(nameof(Courses), await Course.GetAll());
				}
				course.Course_Name = course.Course_Name.TrimStart().TrimEnd();
				await Course.Create(course);
				return RedirectToAction("Courses");
			}
   return BadRequest();

  }

		public async Task<IActionResult> Delete_Course(int id)
		{
			var getid=await Course.GetById(id);
			if (getid != null)
			{
				Expression<Func<TeacherStageCourse, bool>> condition = tsc =>
															tsc.CourseId == id;
				await c1.DeleteWhere(e => e.Course_ID == id);
				await att_Course.DeleteWhere(e => e.Course_ID == id);
				await stdcou.DeleteWhere(e => e.CourseID == id);

				await teacherStage.DeleteWhere(condition);
				await Course.Delete(getid);
				return RedirectToAction("Courses");

			}

			return RedirectToAction("Courses");

		}
		public async Task<IActionResult> Edit_Course(int id)
		{
			var getid = await Course.GetById(id);
			if (getid != null)
			{
				ViewBag.parents = new SelectList(await parent.GetAll(), "ParentStageId", "Name",getid.ParentStage);
				ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name", getid.ChildStage);
				ViewBag.terms = new SelectList(await term.GetAll(), "TermStageId", "Name", getid.TermStage);
				return View("Edit_Course", getid);

			}

			return RedirectToAction("Courses");

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit_Course(Course course)
		{ 
			if (course != null)
			{
				if (course.ChildStageId==0&&course.TermStageId==0)
				{
					var co = await Course.GetById(course.Course_ID);
					Course.Detach(co);
					course.ChildStageId =  co.ChildStageId;
					course.TermStageId = co.TermStageId;
				}
				await Course.Update(course);
				return RedirectToAction("Courses");
			}

			return RedirectToAction("Courses");

		}


	}
}
