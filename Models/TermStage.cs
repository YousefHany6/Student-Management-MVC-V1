using System.ComponentModel.DataAnnotations;

namespace FStudentManagement.Models
{
 public class TermStage
 {
  [Key]
  public int TermStageId { get; set; }
  [Required(ErrorMessage = "ادخل اسم القسم")]
  public string Name { get; set; }
  [Required(ErrorMessage = "ادخل السنين الدراسية")]

  public int ChildStageId { get; set; }
  public virtual ChildStage? ChildStage { get; set; }
  [Required(ErrorMessage = "ادخل المراحل الدراسية")]
  public int ParentStageId { get; set; }
		public virtual ParentStage? ParentStage { get; set; }
		public virtual ICollection<Student>? Students { get; set; }
  public virtual ICollection<Course>? Courses { get; set; }
		public virtual ICollection<TeacherStageCourse>? TeacherStageCourses { get; set; }

	}
}
