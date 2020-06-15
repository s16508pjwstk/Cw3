using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DAL;
using WebApplication2.DTO.Request;
using WebApplication2.GeneratedModels;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{   
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        //private readonly IDbService _dbService;

        //public StudentsController(IDbService dbService)
        //{
        //    _dbService = dbService;
        //}

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var db = new apbd1Context();
            return Ok(db.Student.ToList());
            //return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            //return Ok(_dbService.GetStudent(id));
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("update")]
        public IActionResult UpdateStudent(UpdateStudentForm updateStudentForm)
        {
            var db = new apbd1Context();
            var d1 = db.Student.Where(e => e.IndexNumber.Equals(updateStudentForm.IndexNumber)).First();
            d1.FirstName = updateStudentForm.FirstName;
            d1.LastName = updateStudentForm.LastName;
            d1.BirthDate = updateStudentForm.BirthDate;
            d1.IdEnrollment = updateStudentForm.IdEnrollment;
            db.SaveChanges();
            return Ok(d1);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(String id)
        {
            var db = new apbd1Context();
            var d1 = db.Student.Where(e => e.IndexNumber.Equals(id)).First();
            db.Student.Remove(d1);

       
            return Ok("Student o indexie: " + id + " został poprawnie usuniety");
        }

    }
}