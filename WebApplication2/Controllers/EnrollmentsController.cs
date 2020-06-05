using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DAL;
using WebApplication2.DAL.Dto;
using WebApplication2.DTO.Request;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : Controller
    {
        private readonly IStudentDbService _dbService;

        public EnrollmentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpPost]
        public IActionResult AddEnrollment(EnrollmentDTO enrollmentDTO)
        {
            if (!ModelState.IsValid)
            {
                var state = ModelState;
                return BadRequest();
            }
            Enrollment enrollment = _dbService.EnrollStudent(enrollmentDTO);

            if (enrollment != null)
            {
                return Created("api/students/" + enrollmentDTO.IndexNumber, enrollment);
            }
            else
            {
                return BadRequest();
            }

        }



        [HttpPost("promotions")]
        public IActionResult Promote(PromotionDTO promotionDTO)
        {

            if (!ModelState.IsValid)
            {
                var state = ModelState;
                return BadRequest();
            }
            Enrollment enrollment = _dbService.Promote(promotionDTO);
            if (enrollment != null)
            {
                return Created("", enrollment);
            }
            else
            {
                return BadRequest();
            }
        }

    }

    public interface IStudentsDbService
    {
    }
}