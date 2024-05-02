using FStudentManagement.Data;
using FStudentManagement.Models;
using FStudentManagement.Rpo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Collections.ObjectModel;

namespace FStudentManagement.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AddStagesController : Controller  
	{ 
	
	
		private readonly IRepository<ParentStage> repository;
		private readonly IRepository<Student> std1;
		private readonly IRepository<TeacherStageCourse> tsh;
		private readonly IRepository<Teacher> th;
		private readonly IRepository<Course> cou;
		private readonly IRepository<ChildStage> child;
		private readonly IRepository<TermStage> term;

  public AddStagesController(IRepository<ParentStage> repository, IRepository<Student> std1, IRepository<TeacherStageCourse> tsh, IRepository<Teacher> th, IRepository<Course> cou, IRepository<ChildStage> repository1, IRepository<TermStage> Term)
		{
			this.repository = repository;
			this.std1 = std1;
			this.tsh = tsh;
			this.th = th;
			this.cou = cou;
			this.child = repository1;
			term = Term;
  }

		public async Task<ActionResult> GetAllStags()
		{
			ViewBag.parents = new SelectList(await repository.GetAll(), "ParentStageId","Name");
			ViewBag.Childs = new SelectList(await child.GetAll(),"ChildStageId","Name");

   var PS_Ch_TS = await repository.GetAllWithIncludes(
             p => p.ChildStages,
             p => p.TermStages
         );
			if ( PS_Ch_TS != null )
			{

   return View(PS_Ch_TS);
			}
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create_Parent_S(ParentStage parentStage)
		{
			if (ModelState.IsValid)
			{
				var par = await repository.GetByCondition(s=>s.Name==parentStage.Name.TrimStart().TrimEnd());
				if (par.Any())
				{
					ModelState.AddModelError(string.Empty, "هذه المرحلة موجوده");
     ViewBag.parents = new SelectList(await repository.GetAll(), "ParentStageId", "Name");
     ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
     var PS_Ch_TS = await repository.GetAllWithIncludes(
          p => p.ChildStages,
          p => p.TermStages
      );
     return View("GetAllStags", PS_Ch_TS);
				}
				parentStage.Name=parentStage.Name.TrimStart().TrimEnd();
    await repository.Create(parentStage);

				return RedirectToAction("GetAllStags");

			}

			return View("GetAllStags");

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create_Child_S(ChildStage childStage)
		{
			if (ModelState.IsValid)

			{
    var par = await child.GetByCondition(s => s.Name == childStage.Name.TrimStart().TrimEnd());
    if (par.Any())
    {
     ModelState.AddModelError(string.Empty, "هذه السنة موجوده");
     ViewBag.parents = new SelectList(await repository.GetAll(), "ParentStageId", "Name");
     ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
     var PS_Ch_TS = await repository.GetAllWithIncludes(
          p => p.ChildStages,
          p => p.TermStages
      );
     return View("GetAllStags", PS_Ch_TS);
    }
				childStage.Name=childStage.Name.TrimStart().TrimEnd();
    await child.Create(childStage);

				return RedirectToAction("GetAllStags");


			}

			return View("GetAllStags");

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create_Term_S(TermStage termStage)
		{
			if (ModelState.IsValid)

			{
    var par = await term.GetByCondition(s => s.Name == termStage.Name.TrimStart().TrimEnd());
    if (par.Any())
    {
     ModelState.AddModelError(string.Empty, "هذه القسم موجود");
     ViewBag.parents = new SelectList(await repository.GetAll(), "ParentStageId", "Name");
     ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name");
     var PS_Ch_TS = await repository.GetAllWithIncludes(
          p => p.ChildStages,
          p => p.TermStages
      );
     return View("GetAllStags", PS_Ch_TS);
    }
				termStage.Name=termStage.Name.TrimStart().TrimEnd();
    await term.Create(termStage);

				return RedirectToAction("GetAllStags");


			}

			return View("GetAllStags");

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete_Parent_Stage(int id)
		{
			ParentStage getid=await repository.GetById(id);
			if (getid!=null)
			{
				await tsh.DeleteWhere(s => s.ParentStageId == id);
				await std1.DeleteWhere(s => s.ParentStageId == id);
				await cou.DeleteWhere(s => s.ParentStageId == id);

				await repository.Delete(getid);
			}
			return RedirectToAction(nameof(GetAllStags)); 
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete_child_Stage(int id)
		{
			ChildStage getid = await child.GetById(id);
			if (getid != null)
			{
				await cou.DeleteWhere(s => s.ChildStageId == id);
				await tsh.DeleteWhere(s => s.ChildStageId == id);
				await std1.DeleteWhere(s => s.ChildStageId == id);
				await child.Delete(getid);
			}
			return RedirectToAction(nameof(GetAllStags));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete_Term_Stage(int id)
		{
			TermStage getid = await term.GetById(id);
			if (getid != null)
			{
				await cou.DeleteWhere(s => s.TermStageId == id);
				await tsh.DeleteWhere(s => s.TermStageId == id);
				await std1.DeleteWhere(s => s.TermStageId == id); 
				await term.Delete(getid);
			}
			return RedirectToAction(nameof(GetAllStags));
		}

		public async Task<IActionResult> Edit_Parent_Stage(int id)
		{
			ParentStage getid = await repository.GetById(id);
			return View(getid);
		}
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit_Parent_Stage(ParentStage parentStage)
  {
			if (ModelState.IsValid)
			{
				await repository.Update(parentStage);
				return RedirectToAction(nameof(GetAllStags));

			}

   return View();
  }
		public async Task<IActionResult> Edit_Child_Stage(int id)
		{
			ChildStage getid = await child.GetById(id);
			ViewBag.parents = new SelectList(await repository.GetAll(), "ParentStageId", "Name", getid.ParentStage);

			return View(getid);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit_Child_Stage(ChildStage childStage)
		{
			if (ModelState.IsValid)
			{
				await child.Update(childStage);
				return RedirectToAction(nameof(GetAllStags));

			}

			return View();
		}
		public async Task<IActionResult> Edit_Term_Stage(int id)
		{
   TermStage getid = await term.GetById(id);
   ViewBag.parents = new SelectList(await repository.GetAll(), "ParentStageId", "Name",getid.ParentStage);
   ViewBag.Childs = new SelectList(await child.GetAll(), "ChildStageId", "Name",getid.ChildStage);
			return View(getid);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit_Term_Stage(TermStage termStage)
		{
			if (ModelState.IsValid)
			{
				if (termStage.ChildStageId==0)
				{
					var ch = await term.GetById(termStage.TermStageId);
					term.Detach(ch);
					termStage.ChildStageId = ch.ChildStageId;
				}
				await term.Update(termStage);
				return RedirectToAction(nameof(GetAllStags));

			}

			return View();
		}



	}
}
