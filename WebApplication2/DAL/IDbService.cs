using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public interface IDbService
    {
         IEnumerable<Student> GetStudents();
         Student GetStudent(int id);
    }
}
