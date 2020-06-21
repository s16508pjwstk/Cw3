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

            return enrollment_tran;
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
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
