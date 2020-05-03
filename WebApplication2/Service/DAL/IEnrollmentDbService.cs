using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Service.DAL
{
    public interface IEnrollmentDbService
    {
        Enrollment getEnrollment(StudentEnrollmentForm studentEnrollmentForm, List<Study> studies);
        
    }
}
