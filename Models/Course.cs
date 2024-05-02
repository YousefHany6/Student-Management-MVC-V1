using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FStudentManagement.Models
{
 public class Course
 {
  [Key]
  public int Course_ID { get; set; }

  [Required(ErrorMessage ="ادخل اسم المادة")]
  public string Course_Name { get; set; }

  public virtual ICollection<TeacherCourse>? TeacherCourses { get; set; }
  public virtual ICollection<StudentCourse>? StudentCourses { get; set; }

		// Navigation properties for stages
		public ICollection<Attendance>? cAttendances { get; set; }
		[Required(ErrorMessage = "ادخل المراحل الدراسية")]

		public int ParentStageId { get; set; }
  public virtual ParentStage? ParentStage { get; set; }
		[Required(ErrorMessage = "ادخل الصفوف الدراسية")]

		public int ChildStageId { get; set; }
  public virtual ChildStage? ChildStage { get; set; }

		[Required(ErrorMessage = "ادخل الأقسام الدراسية")]

		public int TermStageId { get; set; }
  public virtual TermStage? TermStage { get; set; }
		public virtual ICollection<StudentTeacher>? StudentTeachers { get; set; }
		public virtual ICollection<TeacherStageCourse>? TeacherStageCourses { get; set; }


	}
}
