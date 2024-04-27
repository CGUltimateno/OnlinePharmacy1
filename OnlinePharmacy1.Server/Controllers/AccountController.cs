using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlinePharmacy.DAL;
using OnlinePharmacy.DTOs;
using OnlinePharmacy.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration, DBConn context, RoleManager<IdentityRole> roleManager, ITokenService tokenService, IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            _context = context;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser([FromForm] NewUserDTO user)
        {
            if (ModelState.IsValid)
            {

                var encryptedEmail = TripleDesEncryptionHelper.Encrypt(user.Email);

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
                var decryptedEmail = TripleDesEncryptionHelper.Decrypt(login.Email);
                var decryptedPassword = TripleDesEncryptionHelper.Decrypt(login.Password);

                AppUser? user = await _userManager.FindByEmailAsync(decryptedEmail);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, decryptedPassword))
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
                            role = userRoles.Count > 0 ? userRoles[0] : "No roles"
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
                var encryptedEmail = TripleDesEncryptionHelper.Encrypt(user.Email);

                AppUser appUser = new()
                {
                    UserName = user.Username,
                    Email = user.Email,

                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(appUser, UserRoles.Admin);
                }
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
    }
}