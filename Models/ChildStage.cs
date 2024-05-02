using System.ComponentModel.DataAnnotations;

namespace FStudentManagement.Models
{
 public class ChildStage
 {
  [Key]
  public int ChildStageId { get; set; }
  [Required(ErrorMessage ="ادخل اسم السنة الدراسية")]
  public string Name { get; set; }
  [Required(ErrorMessage = "ادخل المراحل الدراسية")]
  public int ParentStageId { get; set; }
  public virtual ParentStage? ParentStage { get; set; }
  public virtual ICollection<TermStage>? TermStages { get; set; }
  public virtual ICollection<Student>? Students { get; set; }
  public virtual ICollection<Course>? Courses { get; set; }
		public virtual ICollection<TeacherStageCourse>? TeacherStageCourses { get; set; }

	}
}
