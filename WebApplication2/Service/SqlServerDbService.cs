using System;
using System.Data.SqlClient;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class SqlServerDbService : IStudentDbService
    {
        static string connectionString = "Data Source=localhost\\localsql;Initial Catalog=apbd1;User ID=sa;Password=Szuchow97!";
        public Enrollment EnrollStudent(StudentEnrollmentForm studentEnrollmentForm)
        {
            Enrollment enrollment = new Enrollment();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("SampleTransaction");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "select IdStudy from Studies where name = @name";
                    command.Parameters.AddWithValue("name", studentEnrollmentForm.Studies);

                    var dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        Console.WriteLine("Requested study doesn't exist!");
                        throw new NullReferenceException();
                    }
                    int idstudy = (int)dr["IdStudy"];
                    Console.WriteLine("idstudy: " + idstudy);
                    dr.Close();

                    command.CommandText = "select IdEnrollment from Enrollment where IdStudy = @id and Semester = 1";
                    command.Parameters.AddWithValue("id", idstudy);
                    var dr2 = command.ExecuteReader();
                    DateTime dateNow = DateTime.Now;
                    if (!dr2.Read())
                    {
                        command.CommandText = "insert into Enrollment (IdEnrollment, Semester, IdStudy, StartDate) values ((SELECT ISNULL(MAX(IdEnrollment) + 1, 1) FROM Enrollment), 1, @id, @date)";
                        command.Parameters.AddWithValue("id", idstudy);
                        command.Parameters.AddWithValue("date", dateNow);
                        command.ExecuteNonQuery();
                    }
                    int idEnrollment = (int)dr2["IdEnrollment"];
                    Console.WriteLine("IdEnrollment: " + idstudy);
                    dr2.Close();

                    command.CommandText = "insert into Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values (@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)";
                    command.Parameters.AddWithValue("indexNumber", studentEnrollmentForm.IndexNumber);
                    command.Parameters.AddWithValue("firstName", studentEnrollmentForm.FirstName);
                    command.Parameters.AddWithValue("lastName", studentEnrollmentForm.LastName);
                    command.Parameters.AddWithValue("birthDate", Convert.ToDateTime(studentEnrollmentForm.BirthDate));
                    command.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    command.ExecuteNonQuery();

                    enrollment.IdEnrollment = idEnrollment;
                    enrollment.StartDate = DateTime.Now.ToString();
                    enrollment.IdStudy = idstudy;
                    enrollment.Semester = 1;

                    transaction.Commit();
                    connection.Close();

                }
                catch (InvalidOperationException)
                {
                    transaction.Rollback();
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return enrollment;
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            int idstudy;
            Enrollment enrollment = new Enrollment();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    command.CommandText = "select IdStudy from Studies where name = @name";
                    command.Parameters.AddWithValue("name", studies);
                    var dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        Console.WriteLine("Requested study doesn't exist!");
                        throw new NullReferenceException();
                    }
                    idstudy = (int)dr["IdStudy"];
                    Console.WriteLine("idstudy: " + idstudy);
                    dr.Close();

                    command.CommandText = "select IdEnrollment from Enrollment where IdStudy = @id and Semester = 1";
                    command.Parameters.AddWithValue("id", idstudy);
                    var dr2 = command.ExecuteReader();
                    if (!dr2.Read())
                    {
                        dr2.Close();
                        throw new NullReferenceException();
                    }
                    dr2.Close();
                    command = new SqlCommand("PromoteStudents6", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@semester", semester));
                    command.Parameters.Add(new SqlParameter("@studies", studies));
                    command.ExecuteNonQuery();

                    enrollment = this.GetEnrollment(idstudy, semester);

                    return enrollment;

                }
                catch (InvalidOperationException)
                {

                }
                catch (SqlException)
                {

                }
                return enrollment;
            }
        }

        public Enrollment GetEnrollment(int idstudy, int semester)
        {
            Enrollment enrollment = new Enrollment();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                try
                {
                    command.CommandText = "select * from Enrollment where IdStudy = @id and Semester = @semester + 1";
                    command.Parameters.AddWithValue("id", idstudy);
                    command.Parameters.AddWithValue("semester", semester);
                    var dr2 = command.ExecuteReader();
                    if (dr2.Read())
                    {
                        enrollment.IdEnrollment = (int)dr2["IdEnrollment"];
                        enrollment.Semester = (int)dr2["Semester"];
                        enrollment.IdStudy = (int)dr2["IdStudy"];
                        enrollment.StartDate = dr2["StartDate"].ToString();
                    }
                    dr2.Close();
                }
                catch (InvalidOperationException)
                {

                }
                catch (SqlException)
                {

                }
                return enrollment;
            }
        }
    }
}
