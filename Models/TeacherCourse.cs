namespace FStudentManagement.Models
{
 public class TeacherCourse
 {
  public int Course_ID { get; set; }
  public virtual Course Course { get; set; }

  public int Teacher_ID { get; set; }
  public virtual Teacher Teacher { get; set; }


 }
}
