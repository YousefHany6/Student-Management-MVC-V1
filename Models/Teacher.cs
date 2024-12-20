﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FStudentManagement.Data;

namespace FStudentManagement.Models
{
 public class Teacher
 {
  [Key]
  public int Teacher_ID { get; set; }

  [Required(ErrorMessage ="ادخل اسم المعلم")]
  public string Teacher_Name { get; set; }
		public string? userid { get; set; }
		public virtual SUser SUser { get; set; }
		public virtual ICollection<StudentTeacher>? StudentTeachers { get; set; }
  public virtual ICollection<TeacherCourse>? TeacherCourses { get; set; }
		public virtual ICollection<TeacherStageCourse>? TeacherStageCourses { get; set; }
		public ICollection<Attendance>? tAttendances { get; set; }



	}
}
