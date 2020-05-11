using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public interface IStudentDbService
    {
        Enrollment EnrollStudent(StudentEnrollmentForm studentEnrollmentForm);
        Enrollment PromoteStudents(int semester, string studies);
    }
}
