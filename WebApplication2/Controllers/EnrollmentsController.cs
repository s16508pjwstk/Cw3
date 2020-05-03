using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DAL;
using WebApplication2.DTO.Request;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private static IEnumerable<Study> _studies;

        private static List<Study> studiesList;
        static string connectionString = "Data Source=localhost\\localsql;Initial Catalog=apbd1;User ID=sa;Password=Szuchow97!";

        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateEnrollment(StudentEnrollmentForm studentEnrollmentForm)
        {
            if (studentEnrollmentForm.FirstName == null)
            {
                return BadRequest();
            }
            else if (studentEnrollmentForm.IndexNumber == null)
            {
                return BadRequest();
            }
            else if (studentEnrollmentForm.LastName == null)
            {
                return BadRequest();
            }
            else if (studentEnrollmentForm.BirthDate == null)
            {
                return BadRequest();
            }
            else if (studentEnrollmentForm.Studies == null)
            {
                return BadRequest();
            }

            Enrollment enrollment = new Enrollment();
            try
            {
                enrollment = _service.EnrollStudent(studentEnrollmentForm);
            } catch (NullReferenceException)
            {
                return BadRequest();
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
            return Created("", enrollment);
            
           

        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsForm promoteStudentsForm)
        {
            Enrollment enrollment = new Enrollment();
            try
            {
                enrollment = _service.PromoteStudents(Int32.Parse(promoteStudentsForm.Semester), promoteStudentsForm.Studies);
            } catch (NullReferenceException)
            {
                return BadRequest();
            }
            return Created("", enrollment);
        }
    }
}