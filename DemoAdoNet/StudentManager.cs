using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DemoAdoNet
{
    public class StudentManager
    {
        private static String connectionString = "Data Source=DESKTOP-RG5G9U4;Initial Catalog=SMS;Integrated Security=True";
        public bool AddStudent(Student newStudent) {
            if (newStudent == null)
            {
                throw new ArgumentNullException("Student can not be null");
            }
            using var connection = new SqlConnection(connectionString);

            connection.Open();

            string insertQuery = "INSERT INTO Student (StudentId, Name, Email, Phone, FatherName, MotherName, Dob, Nationality, JoinDate, Status) VALUES (@StudentId, @Name, @Email, @Phone, @FatherName, @MotherName, @Dob, @Nationality, @JoinDate, @Status)";


            using var command = connection.CreateCommand();
            command.CommandText = insertQuery;


            command.Parameters.AddWithValue("@StudentId", newStudent.StudentId);
            command.Parameters.AddWithValue("@Name", newStudent.Name);
            command.Parameters.AddWithValue("@Email", newStudent.Email);
            command.Parameters.AddWithValue("@Phone", newStudent.Phone);
            command.Parameters.AddWithValue("@FatherName", newStudent.FatherName);
            command.Parameters.AddWithValue("@MotherName", newStudent.MotherName);
            command.Parameters.AddWithValue("@Dob", newStudent.Dob);
            command.Parameters.AddWithValue("@Nationality", newStudent.Nationality);
            command.Parameters.AddWithValue("@JoinDate", newStudent.JoinDate);
            command.Parameters.AddWithValue("@Status", newStudent.Status);

            connection.Close();
            var rowAffected = command.ExecuteNonQuery();
            return rowAffected > 0;

        }

        public List<Student> GetAllStudents()
        {
            using var connection = new SqlConnection(connectionString);

            string selectQuery = "SELECT * FROM Student";
            using var command = connection.CreateCommand();
            command.CommandText = selectQuery;
            List<Student> students = new List<Student>(); 
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            //reader.Read();
            while (reader.Read())
            {
                Student student = new Student();
                student.StudentId = Convert.ToInt32(reader["StudentId"]);
                student.Name = reader["Name"].ToString();
                student.Email = reader["Email"].ToString();
                student.Phone = reader["Phone"].ToString();
                student.FatherName = reader["FatherName"].ToString();
                student.MotherName = reader["MotherName"].ToString();
                student.Dob = (DateTime)reader["Dob"];
                student.Nationality = reader["Nationality"].ToString();
                student.JoinDate = (DateTime)reader["JoinDate"];
                student.Status = (bool)reader["Status"];
                students.Add(student);
            }

            connection.Close();
            return students;
            
        }

        public bool UpdateStudent(Student student)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            string updateQuery = "UPDATE Student SET Name = @Name, Email = @Email, Phone = @Phone, FatherName = @Fathername, MotherName = @Mothername, Dob = @Dob, Nationality = @Nationality, JoinDate = @Joindate, Status = @Status WHERE StudentId = @StudenId";
            
            using var command = connection.CreateCommand();
            command.CommandText = updateQuery;

            command.Parameters.AddWithValue("Name", student.Name);
            command.Parameters.AddWithValue("Email", student.Email);
            command.Parameters.AddWithValue("Phone", student.Phone);
            command.Parameters.AddWithValue("FatherName", student.FatherName);
            command.Parameters.AddWithValue("MotherName", student.MotherName);
            command.Parameters.AddWithValue("Dob", student.Dob);
            command.Parameters.AddWithValue("Nationality", student.Nationality);
            command.Parameters.AddWithValue("JoinDate", student.JoinDate);
            command.Parameters.AddWithValue("Status", student.Status); 

            var rowAffected = command.ExecuteNonQuery();
            return rowAffected > 0;
        }

        public bool DeleteStudent(int StudentId)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            string deleteQuery = "DELETE FROM Student And StudentClass WHERE StudentID = @StudentId";

            using var command = connection.CreateCommand();
            command.CommandText = deleteQuery;

            command.Parameters.AddWithValue("StudentId", StudentId);

            var rowAffected = command.ExecuteNonQuery();
            return rowAffected > 0;

        }

        public List<Student> GetStudentsByFilter(string filter)
        {
            if(filter == null)
            {
                throw new ArgumentNullException("Filter canot be null");
            }
            if(filter.Length < 3)
            {
                throw new ArgumentException("Filter must be three character long");
            }

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var filterQuery = $"Select * From Student Where Name like '%' + @filter + '%'";
            using var command = connection.CreateCommand();
            command.CommandText = filterQuery;
            command.Parameters.AddWithValue("@filter", filter);

            List<Student>  student = new List<Student>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Student std = new Student();
                std.StudentId = Convert.ToInt32(reader["StudentId"]);
                std.Name = reader["Name"].ToString();
                std.Email = reader["Email"].ToString();
                std.Phone = reader["Phone"].ToString();
                std.FatherName = reader["FatherName"].ToString();
                std.MotherName = reader["MotherName"].ToString();
                std.Dob = (DateTime)reader["Dob"];
                std.Nationality = reader["Nationality"].ToString();
                std.JoinDate = (DateTime)reader["JoinDate"];
                std.Status = (bool)reader["Status"];
                student.Add(std);
            }
            return student;
        }
        public List<Class> GetClassDetails()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            List<Class> classes = new List<Class>();
            using var command = connection.CreateCommand();
            //var courseQuery = "SELECT Class.ClassId,COUNT(Student.StudentId) AS NumberOfStudents,Teacher.TeacherId,Teacher.Name AS TeacherName,TeacherClass.Schedule AS TeacherClassTime Course.CourseName FROM StudentClass JOIN TeacherClass ON StudentClass.ClassId = TeacherClass.ClassId JOIN Student ON StudentClass.StudentId = Student.StudentId JOIN Teacher ON TeacherClass.TeacherId = Teacher.TeacherId JOIN Class ON StudentClass.ClassId = Class.ClassId JOIN Course ON StudentClass.CourseId = Course.CourseId WHERE StudentClass.Schedule = TeacherClass.Schedule GROUP BY Class.ClassId, Teacher.TeacherId, Teacher.Name, TeacherClass.Schedule, StudentClass.Schedule, Course.CourseName;";
            var courseQuery = @"
            SELECT 
                Class.ClassId,
                COUNT(Student.StudentId) AS NumberOfStudents,
                Teacher.TeacherId,
                Teacher.Name AS TeacherName,
                TeacherClass.Schedule AS TeacherClassTime,
                StudentClass.Schedule AS StudentClassTime,
                Course.CourseName
            FROM 
                StudentClass 
            JOIN 
                TeacherClass ON StudentClass.ClassId = TeacherClass.ClassId
            JOIN 
                Student ON StudentClass.StudentId = Student.StudentId
            JOIN 
                Teacher ON TeacherClass.TeacherId = Teacher.TeacherId
            JOIN 
                Class ON StudentClass.ClassId = Class.ClassId
            JOIN 
                Course ON StudentClass.CourseId = Course.CourseId
            WHERE 
                StudentClass.Schedule = TeacherClass.Schedule
            GROUP BY 
                Class.ClassId, Teacher.TeacherId, Teacher.Name, TeacherClass.Schedule, StudentClass.Schedule, Course.CourseName;
            ";
            command.CommandText = courseQuery;

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Class st = new Class();
                st.CourseName = reader["CourseName"].ToString();
                st.ClassId = Convert.ToInt32(reader["ClassId"]);
                st.NumberOfStudents = Convert.ToInt32(reader["NumberOfStudents"]);
                st.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                if(reader["TeacherName"] != DBNull.Value)
                {
                    st.TeacherName = reader["TeacherName"].ToString();
                }
                st.Time = (TimeSpan)reader["TeacherClassTime"];

                classes.Add(st);
            }
            return classes;

        }
    }
}