namespace FStudentManagement.Models
{
	public class TeacherStageCourse
	{
		public int TeacherId { get; set; }
		public Teacher Teacher { get; set; }

		public int ParentStageId { get; set; }
		public ParentStage ParentStage { get; set; }

		public int ChildStageId { get; set; }
		public ChildStage ChildStage { get; set; }

		public int TermStageId { get; set; }
		public TermStage TermStage { get; set; }

		public int CourseId { get; set; }
		public Course Course { get; set; }
	}
}
