using FStudentManagement.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FStudentManagement.Models
{
 public class Student
 {
  [Key]
  public int Student_ID { get; set; }

		[Required(ErrorMessage = "ادخل اسم الطالب")]
		public string Student_Name { get; set; }

  public string? userid {  get; set; }
  public virtual SUser SUser { get; set; }

  public virtual ICollection<StudentCourse>? StudentCourses { get; set; }
  public virtual ICollection<StudentTeacher>? StudentTeachers { get; set; }
		public ICollection<StudentAttendance>? StudentAttendances { get; set; }


		// Foreign keys for stages
		public int ParentStageId { get; set; }
  public virtual ParentStage? ParentStage { get; set; }

  public int ChildStageId { get; set; }
  public virtual ChildStage? ChildStage { get; set; }

  public int TermStageId { get; set; }
  public virtual TermStage? TermStage { get; set; }


 }
}
