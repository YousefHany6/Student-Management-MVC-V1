using System.ComponentModel.DataAnnotations;

namespace FStudentManagement.Models
{
 public class ParentStage
 {
  public int ParentStageId { get; set; }
  [Required(ErrorMessage = "ادخل اسم المرحلة الدراسية")]
  public string Name { get; set; }
  public virtual ICollection<ChildStage>? ChildStages { get; set; }
  public virtual ICollection<TermStage>? TermStages { get; set; }
  public virtual ICollection<Student>? Students { get; set; }
  public virtual ICollection<Course>? Courses { get; set; }
		public virtual ICollection<TeacherStageCourse>? TeacherStageCourses { get; set; }

	}
}
