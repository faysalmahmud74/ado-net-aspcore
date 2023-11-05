using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAdoNet
{
    public class TeacherManager
    {
        private String connectionString = "Data Source=DESKTOP-RG5G9U4;Initial Catalog=SMS;Integrated Security=True";

        public bool AddTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException("Student can not be null");
            }
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            connection.Open();
            var insertQuery = "INSERT INTO Teacher (Name, Phone, Address, BloodGroup, Nationality, JoinDate) VALUES (@Name, @Phone, @Address, @BloodGroup, @Nationality, @JoinDate)";
            command.CommandText = insertQuery;

            //command.Parameters.AddWithValue("TeacherId", teacher.TeacherId);
            command.Parameters.AddWithValue("Name", teacher.Name);
            command.Parameters.AddWithValue("Phone", teacher.Phone);
            command.Parameters.AddWithValue("Address", teacher.Address);
            command.Parameters.AddWithValue("BloodGroup", teacher.BloodGroup);
            command.Parameters.AddWithValue("Nationality", teacher.Nationality);
            command.Parameters.AddWithValue("JoinDate", teacher.JoinDate);

            var rowAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowAffected > 0;
        }

        public List<Teacher> GetAllTeacher()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            connection.Open();
            var listQuery = "SELECT * FROM Teacher";
            command.CommandText = listQuery;

            List<Teacher> teachers = new List<Teacher>();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var teacher = new Teacher();
                teacher.TeacherId = Convert.ToInt32(reader["TeacherId"].ToString());
                teacher.Name = reader["Name"].ToString();
                teacher.Phone = reader["Phone"].ToString();
                teacher.Address = reader["Address"].ToString();
                teacher.BloodGroup = reader["BloodGroup"].ToString();
                teacher.Nationality = reader["Nationality"].ToString();
                teacher.JoinDate = (DateTime)reader["JoinDate"];
                teachers.Add(teacher);
            }
            connection.Close();
            return teachers;
        }

        public bool UpdateTeacher(Teacher teacher)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            var updateQuery = "Update Teaches SET (Name = @Name, Phone = @Phone, Address = @Address, BloodGroup = @Bloodgroup, Nationality = @Nationality, JoinDate = @JoinDate) WHERE TeacherId = @Id";
            command.CommandText = updateQuery;

            command.Parameters.AddWithValue("TeacherId", teacher.TeacherId);
            command.Parameters.AddWithValue("Name", teacher.Name);
            command.Parameters.AddWithValue("Phone", teacher.Phone);
            command.Parameters.AddWithValue("Address", teacher.Address);
            command.Parameters.AddWithValue("BloodGroup", teacher.BloodGroup);
            command.Parameters.AddWithValue("Nationality", teacher.Nationality);
            command.Parameters.AddWithValue("JoinDate", teacher.JoinDate);

            var rowAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowAffected > 0;
        }

        public List<Teacher> FilterByTeacher(string filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("Filter canot be null");
            }
            if (filter.Length < 3)
            {
                throw new ArgumentException("Filter must be three character long");
            }

            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            var filterQuery = "SELECT * FROM Teacher Where Name Like '%' + @filter + '%' ";
            command.CommandText = filterQuery;

            command.Parameters.AddWithValue("filter", filter);

            List<Teacher> teachers = new List<Teacher>();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var teacher = new Teacher();
                teacher.TeacherId = Convert.ToInt32(reader["TeacherId"].ToString());
                teacher.Name = reader["Name"].ToString();
                teacher.Phone = reader["Phone"].ToString();
                teacher.Address = reader["Address"].ToString();
                teacher.BloodGroup = reader["BloodGroup"].ToString();
                teacher.Nationality = reader["Nationality"].ToString();
                teacher.JoinDate = (DateTime)reader["JoinDate"];
                teachers.Add(teacher);
            }
            connection.Close();
            return teachers;

        }

        /*public List<Teacher> GetLimitTeacher()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            connection.Open();
            var listQuery = "SELECT * FROM Teacher ORDER BY TeacherId OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY";
            command.CommandText = listQuery;

            command.Parameters.AddWithValue("@offset", 10);
            command.Parameters.AddWithValue("@fetch", 10);

            bool showNext = true;
            List<Teacher> teachers = new List<Teacher>();

            SqlDataReader reader = command.ExecuteReader();

            while (showNext)
            {
                while (reader.Read())
                {
                    var teacher = new Teacher();
                    teacher.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                    teacher.Name = reader["Name"].ToString();
                    teacher.Phone = reader["Phone"].ToString();
                    teacher.Address = reader["Address"].ToString();
                    teacher.BloodGroup = reader["BloodGroup"].ToString();
                    teacher.Nationality = reader["Nationality"].ToString();
                    teacher.JoinDate = (DateTime)reader["JoinDate"];
                    teachers.Add(teacher);
                }
                reader.Close();

                var key = Console.ReadKey(intercept: true).Key;
                if (key == ConsoleKey.N)
                {
                    command.Parameters["@offset"].Value = teachers.Count;
                    reader = command.ExecuteReader();
                }
                else if (key == ConsoleKey.Q)
                {
                    showNext = false;
                }
                else
                {
                    throw new Exception("Invalid Key");
                }
            }

            connection.Close();
            return teachers;
        }*/

        public List<Teacher> GetLimitTeacher(int page, int pageSize)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            connection.Open();
            var listQuery = "SELECT * FROM Teacher ORDER BY TeacherId OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY";
            command.CommandText = listQuery;
            int offset = (page - 1) * pageSize;

            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@fetch", pageSize);

            List<Teacher> teachers = new List<Teacher>();
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var teacher = new Teacher();
                teacher.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                teacher.Name = reader["Name"].ToString();
                teacher.Phone = reader["Phone"].ToString();
                teacher.Address = reader["Address"].ToString();
                teacher.BloodGroup = reader["BloodGroup"].ToString();
                teacher.Nationality = reader["Nationality"].ToString();
                teacher.JoinDate = (DateTime)reader["JoinDate"];
                teachers.Add(teacher);
            }

            return teachers;
        }


    }
}
