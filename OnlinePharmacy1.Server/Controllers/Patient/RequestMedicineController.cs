using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.DAL;
using OnlinePharmacy.DTOs;
using OnlinePharmacy.Helpers;
using OnlinePharmacy.Models;
using System.Security.Claims;
namespace OnlinePharmacy.Controllers.Patient
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestMedicineController : ControllerBase
    {
        private readonly DBConn _context;
        private readonly UserManager<AppUser> _userManager;
        public RequestMedicineController(UserManager<AppUser> userManager ,DBConn context)
        {
            _context = context;
            _userManager = userManager;

        }


        [HttpPost("RequestMedicine")]
        public async Task<IActionResult> RequestMedicine([FromForm] RequestMedicineDTO medicineRequest)
        {
            if (ModelState.IsValid)
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
               
                var appUser = await _userManager.FindByIdAsync(UserId);

                if (appUser != null)

                {
                    var requestId = Guid.NewGuid().ToString();

                    var patient = new MedicineRequest
                    {
                        RequestId = requestId,
                        PatientId = appUser.Id,
                        MedicineName = medicineRequest.MedicineName,
                        PrescriptionDetails = medicineRequest.PrescriptionDetails,
                        RequestStatus = "Pending"
                    };

                    _context.Requests.Add(patient);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "Medicine request submitted successfully!" });
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                    return NotFound(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid medicine request details.");
                return BadRequest(ModelState);
            }
        }

        

    }

}