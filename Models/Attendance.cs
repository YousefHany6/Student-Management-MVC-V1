namespace FStudentManagement.Models
{
	public class Attendance
	{
		public int AttendanceId { get; set; }
		public DateTime Date { get; set; }
		// Other attendance properties
		public int Course_ID { get; set; }
		public virtual Course? Course { get; set; }

		public int Teacher_ID { get; set; }
		public virtual Teacher? Teacher { get; set; }
		public ICollection<StudentAttendance>? StudentAttendances { get; set; }

	}
}
