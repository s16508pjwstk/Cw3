using System;
using System.Data.SqlClient;
using System.Linq;
using WebApplication2.DAL;
using WebApplication2.GeneratedModels;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class SqlServerDbService : IStudentDbService
    {
        static string connectionString = "Data Source=localhost\\localsql;Initial Catalog=apbd1;User ID=sa;Password=Szuchow97!";
        public Enrollment EnrollStudent(StudentEnrollmentForm studentEnrollmentForm)
        {
            Enrollment enrollment = new Enrollment();
            var db = new apbd1Context();
            var study = db.Studies.Where(e => e.Name.Equals(studentEnrollmentForm.Studies)).FirstOrDefault();
            if (study == null)
            {
                Console.WriteLine("Requested study doesn't exist!");
                throw new NullReferenceException();
            }
            int idStudy = study.IdStudy;

            var enrollment_tran = db.Enrollment.Where(e => e.IdStudy == idStudy && e.Semester == 1).FirstOrDefault();

            if (enrollment_tran == null)
            {
                DateTime dateNow = DateTime.Now;
                var lastEnrollmentId = db.Enrollment.OrderBy(e => e.IdEnrollment).Last().IdEnrollment;
                enrollment_tran = new Enrollment
                {
                    IdEnrollment = lastEnrollmentId + 1,
                    Semester = 1,
                    IdStudy = idStudy,
                    StartDate = dateNow
                };
                db.Enrollment.Add(enrollment_tran);
                db.SaveChanges();
            }

            var enrollmentId = enrollment_tran.IdEnrollment;
            var student_enrollment = new Student
            {
                IndexNumber = studentEnrollmentForm.IndexNumber,
                FirstName = studentEnrollmentForm.FirstName,
                LastName = studentEnrollmentForm.LastName,
                BirthDate = DateTime.Parse(studentEnrollmentForm.BirthDate),
                IdEnrollment = enrollmentId
            };
            db.Student.Add(student_enrollment);

            //db.SaveChanges();

            return enrollment_tran;




            //Enrollment enrollment = new Enrollment();
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    SqlCommand command = connection.CreateCommand();
            //    SqlTransaction transaction;

            //    transaction = connection.BeginTransaction("SampleTransaction");

            //    command.Connection = connection;
            //    command.Transaction = transaction;
            //    try
            //    {
            //        command.CommandText = "select IdStudy from Studies where name = @name";
            //        command.Parameters.AddWithValue("name", studentEnrollmentForm.Studies);

            //        var dr = command.ExecuteReader();
            //        if (!dr.Read())
            //        {
            //            dr.Close();
            //            transaction.Rollback();
            //            Console.WriteLine("Requested study doesn't exist!");
            //            throw new NullReferenceException();
            //        }
            //        int idstudy = (int)dr["IdStudy"];
            //        Console.WriteLine("idstudy: " + idstudy);
            //        dr.Close();

            //        command.CommandText = "select IdEnrollment from Enrollment where IdStudy = @id and Semester = 1";
            //        command.Parameters.AddWithValue("id", idstudy);
            //        var dr2 = command.ExecuteReader();
            //        DateTime dateNow = DateTime.Now;
            //        if (!dr2.Read())
            //        {
            //            command.CommandText = "insert into Enrollment (IdEnrollment, Semester, IdStudy, StartDate) values ((SELECT ISNULL(MAX(IdEnrollment) + 1, 1) FROM Enrollment), 1, @id, @date)";
            //            command.Parameters.AddWithValue("id", idstudy);
            //            command.Parameters.AddWithValue("date", dateNow);
            //            command.ExecuteNonQuery();
            //        }
            //        int idEnrollment = (int)dr2["IdEnrollment"];
            //        Console.WriteLine("IdEnrollment: " + idstudy);
            //        dr2.Close();

            //        command.CommandText = "insert into Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values (@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)";
            //        command.Parameters.AddWithValue("indexNumber", studentEnrollmentForm.IndexNumber);
            //        command.Parameters.AddWithValue("firstName", studentEnrollmentForm.FirstName);
            //        command.Parameters.AddWithValue("lastName", studentEnrollmentForm.LastName);
            //        command.Parameters.AddWithValue("birthDate", Convert.ToDateTime(studentEnrollmentForm.BirthDate));
            //        command.Parameters.AddWithValue("idEnrollment", idEnrollment);
            //        command.ExecuteNonQuery();

            //        enrollment.IdEnrollment = idEnrollment;
            //        enrollment.StartDate = DateTime.Now.ToString();
            //        enrollment.IdStudy = idstudy;
            //        enrollment.Semester = 1;

            //        transaction.Commit();
            //        connection.Close();

            //    }
            //    catch (InvalidOperationException)
            //    {
            //        transaction.Rollback();
            //    }
            //    catch (SqlException e)
            //    {
            //        transaction.Rollback();
            //        throw e;
            //    }
            //}
            //return enrollment;
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            //Enrollment enrollment = new Enrollment();
            var db = new apbd1Context();

            var study = db.Studies.Where(e => e.Name.Equals(studies)).FirstOrDefault();
            if (study == null)
            {
                Console.WriteLine("Requested study doesn't exist!");
                throw new NullReferenceException();
            }
            int idStudy = study.IdStudy;

            var enrollment_tran = db.Enrollment.Where(e => e.IdStudy == idStudy && e.Semester == semester).FirstOrDefault();

            if (enrollment_tran == null)
            {
                DateTime dateNow = DateTime.Now;
                var lastEnrollmentId = db.Enrollment.OrderBy(e => e.IdEnrollment).Last().IdEnrollment;
                enrollment_tran = new Enrollment
                {
                    IdEnrollment = lastEnrollmentId + 1,
                    Semester = semester,
                    IdStudy = idStudy,
                    StartDate = dateNow
                };
                db.Enrollment.Add(enrollment_tran);
                db.SaveChanges();
            }

            var startEnrollmentId = enrollment_tran.IdEnrollment;

            var endEnrollmentTran = db.Enrollment.Where(e => e.IdStudy == idStudy && e.Semester == semester + 1).FirstOrDefault();

            if (endEnrollmentTran == null)
            {
                DateTime dateNow = DateTime.Now;
                var lastEnrollmentId = db.Enrollment.OrderBy(e => e.IdEnrollment).Last().IdEnrollment;
                endEnrollmentTran = new Enrollment
                {
                    IdEnrollment = lastEnrollmentId + 1,
                    Semester = semester,
                    IdStudy = idStudy,
                    StartDate = dateNow
                };
                db.Enrollment.Add(endEnrollmentTran);
                db.SaveChanges();
            }

            var endEnrollmentId = endEnrollmentTran.IdEnrollment;

            var studentsList = db.Student
                .Where(e => e.IdEnrollment == startEnrollmentId)
                .ToList();
            foreach(var i in studentsList)
            {
                Console.Write("Stud");
                i.IdEnrollment = endEnrollmentId;
            }
            db.SaveChanges();

            return endEnrollmentTran;
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
                        //enrollment.StartDate = dr2["StartDate"].ToString();
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
