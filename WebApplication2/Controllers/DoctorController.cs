using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DTO.Request;
using WebApplication2.GeneratedModels;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {

        private readonly DoctorDbContext _context;

        public DoctorController(DoctorDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDoctors()
        {
            return Ok(_context.Doctors.ToList());
        }

        [HttpPost]
        public IActionResult createDoctor(CreateDoctorForm createDoctorForm)
        {
            if (createDoctorForm.IndexNumber == null)
            {
                return BadRequest();
            }

            if (createDoctorForm.FirstName == null)
            {
                return BadRequest();
            }

            if (createDoctorForm.LastName == null)
            {
                return BadRequest();
            }

            if (createDoctorForm.Email == null)
            {
                return BadRequest();
            }

            Doctor doctor = new Doctor();
            doctor.FirstName = createDoctorForm.FirstName;
            doctor.LastName = createDoctorForm.LastName;
            doctor.Email = createDoctorForm.Email;

            _context.Add(doctor);
            _context.SaveChanges();
            return Ok(doctor);
        }

        [HttpPut]
        public IActionResult updateDoctor(CreateDoctorForm createDoctorForm)
        {
            if (createDoctorForm.IndexNumber == null)
            {
                return BadRequest();
            }

            if (createDoctorForm.FirstName == null)
            {
                return BadRequest();
            }

            if (createDoctorForm.LastName == null)
            {
                return BadRequest();
            }

            if (createDoctorForm.Email == null)
            {
                return BadRequest();
            }

            using (var db = _context)
            {
                var result = db.Doctors.SingleOrDefault(b => b.IdDoctor == int.Parse(createDoctorForm.IndexNumber));
                if (result != null)
                {
                    result.FirstName = createDoctorForm.FirstName;
                    result.LastName = createDoctorForm.LastName;
                    result.Email = createDoctorForm.Email;

                    db.SaveChanges();
                }
            }
            return Ok(createDoctorForm);
        }


        [HttpDelete("{index}")]
        public IActionResult deleteDoctor(int index)
        {
            using (var db = _context)
            {
                var result = db.Doctors.SingleOrDefault(b => b.IdDoctor == index);
                if (result != null)
                {
                    _context.Entry(result).State = EntityState.Deleted;

                    db.SaveChanges();
                }
            }

            return Ok("deleted");
        }


    }
}