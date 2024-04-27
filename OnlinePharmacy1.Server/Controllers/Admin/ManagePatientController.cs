using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using OnlinePharmacy.DAL;

using OnlinePharmacy.DTOs;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;
using Microsoft.AspNetCore.Authorization;



namespace OnlinePharmacy.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagePatientController : ControllerBase
    {

        private readonly DBConn _context;


        public ManagePatientController(DBConn context)
        {
            _context = context;

        }

        [Authorize (Roles=UserRoles.User)]
        [Authorize]

        [HttpGet]
        public IActionResult ListPatients()
        {
            var patient= _context.Patients.ToList();
            if (patient.Count == 0)
            {
                return NotFound("Patients Not available. ");
            }
            return Ok(patient);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCPatient([FromForm] AddPatientDTO model)
        {
            if (model == null)
            {
                return BadRequest("Patient model is null.");
            }

            var patient = new PatientInfo
            {
                
               FirstName = model.FirstName,
               LastName = model.LastName,
               DateOfBirth = model.DateOfBirth,
               Address = model.Address,
               PhoneNumber =model.PhoneNumber,
               
               

            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return Ok(patient);
        }
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, [FromForm] UpdatePatientDto model)
        {
            if (model == null || model.PatientId == 0)
            {
                if (model == null)
                {
                    return BadRequest($"Patient model is null.");
                }
                return BadRequest($"Patient Id {model.PatientId} is invalid.");
            }

            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return BadRequest($"Patient was not found with id {id}");
            }

            patient.FirstName = model.FirstName;
            patient.LastName = model.LastName;
            patient.DateOfBirth = model.DateOfBirth;
            patient.PhoneNumber = model.PhoneNumber;
            patient.Address = model.Address;
          

            _context.SaveChanges();
            return Ok("Patient details updated");
        }

        [HttpDelete]
                public IActionResult DeletePatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient== null)
            {
                return NotFound($"Product not found with {id} ");
            }
            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return Ok("Patient  deleted successfully.");
        }

    }
}
