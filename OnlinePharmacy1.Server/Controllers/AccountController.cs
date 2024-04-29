using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlinePharmacy.DAL;
using OnlinePharmacy.DTOs;
using OnlinePharmacy.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlinePharmacy.Helpers;
using OnlinePharmacy.Extensions;
using System.Threading.Tasks;
using System;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace OnlinePharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;
        public readonly DBConn _context;
        public readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration, DBConn context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser([FromForm] NewUserDTO user)
        {
            if (ModelState.IsValid)
            {

                

                AppUser appUser = new()
                {
                    UserName = user.Username,
                    Email = user.Email,

                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    var encryptedFirstName = TripleDesEncryptionHelper.Encrypt(user.FirstName);
                    var encryptedLastName = TripleDesEncryptionHelper.Encrypt(user.LastName);

                    var encryptedAddress = TripleDesEncryptionHelper.Encrypt(user.Address);
                    var encryptedPhoneNumber = TripleDesEncryptionHelper.Encrypt(user.PhoneNumber);


                    var patient = new PatientInfo
                    {
                        UserId = appUser.Id,
                        FirstName = encryptedFirstName,
                        LastName = encryptedLastName,
                        PhoneNumber = encryptedPhoneNumber,
                        Address = encryptedAddress,

                        // Set other patient-related properties
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "Registered successfully!" });



                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }

            }
            return BadRequest(ModelState);
        }




        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromForm] LoginDTO login)
        {
            if (ModelState.IsValid)
            {


                AppUser? user = await _userManager.FindByEmailAsync(login.email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, login.password))
                    {
                        // Generate and return JWT token
                        var claims = new List<Claim>(); 

                        var userRoles = await _userManager.GetRolesAsync(user);
                        claims.Add(new Claim(ClaimTypes.Name, user.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        foreach (var userRole in userRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: sc
                        );

                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            role = userRoles.Count > 0 ? userRoles[0] : "No roles",
                             userId = user.Id
                        };

                        return Ok(_token);

                    }
                    else
                    {
                        return Unauthorized();
                    }
                }

                else { ModelState.AddModelError("", " email  is invalid "); }


            }
            return BadRequest(ModelState);
            {

            }
        }
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] NewUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(user.Username);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "A user with this username already exists." });
                }
                

                AppUser appUser = new()
                {
                    UserName = user.Username,
                    Email = user.Email,

                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                if (result.Succeeded)
                {
                    if (!await _userManager.IsInRoleAsync(appUser, UserRoles.Admin))
                    {
                        await _userManager.AddToRoleAsync(appUser, UserRoles.Admin);
                    }
                    var encryptedFirstName = TripleDesEncryptionHelper.Encrypt(user.FirstName);
                    var encryptedLastName = TripleDesEncryptionHelper.Encrypt(user.LastName);

                    var encryptedAddress = TripleDesEncryptionHelper.Encrypt(user.Address);
                    var encryptedPhoneNumber = TripleDesEncryptionHelper.Encrypt(user.PhoneNumber);


                    var patient = new PatientInfo
                    {
                        UserId = appUser.Id,
                        FirstName = encryptedFirstName,
                        LastName = encryptedLastName,
                        PhoneNumber = encryptedPhoneNumber,
                        Address = encryptedAddress,

                        // Set other patient-related properties
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "Registered successfully!" });



                }


                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }


            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetails()
        {

            // Retrieve user ID from the current JWT claim
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Missing or invalid user ID in JWT token.");
            }

            var user = await _context.Patients.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                user.FirstName = TripleDesEncryptionHelper.Decrypt(user.FirstName);
                user.LastName = TripleDesEncryptionHelper.Decrypt(user.LastName);
                user.PhoneNumber = TripleDesEncryptionHelper.Decrypt(user.PhoneNumber);
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

    }

}