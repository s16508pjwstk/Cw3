using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;
        // baza wystawiona u siebie lokalnie
        static string connectionString = "Data Source=localhost\\localsql;Initial Catalog=apbd1;User ID=sa;Password=Szuchow97!";

        static MockDbService()
        {
            
        }

        public Student GetStudent(string id)
        {
            using (SqlConnection connection = new SqlConnection(
               connectionString))
            {
                SqlCommand command = new SqlCommand("select * from Student where IndexNumber=@id", connection);
                command.Parameters.AddWithValue("id", id);
                command.Connection.Open();
                var dr = command.ExecuteReader();
                _students = new List<Student>();
                List<Student> students = new List<Student>();
                var st = new Student();
                while (dr.Read())
                {
                    st.IdStudent = (dr["IndexNumber"].ToString());
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    students.Add(st);
                }
                return st;
                
            }
        }

        public IEnumerable<Student> GetStudents()
        {
            using (SqlConnection connection = new SqlConnection(
               connectionString))
            {
                SqlCommand command = new SqlCommand("select * from Student;", connection);
                command.Connection.Open();
                var dr = command.ExecuteReader();
                _students = new List<Student>();
                List<Student> students = new List<Student>();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IdStudent = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    students.Add(st);
                }
                _students = students;
            }

            return _students;
        }
    }
}
