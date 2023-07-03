using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowTime.Core.DTO;
using ShowTime.Core.Enums;
using ShowTime.Core.IdentityEntities;
using ShowTime.Core.Models;
using ShowTime.Services.IServices;

namespace ShowTime.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ResponseDTO<AuthenticationResponse>> PostRegister(UserRegistrationRequest registerDTO)
        {
            ResponseDTO<AuthenticationResponse> response = new ResponseDTO<AuthenticationResponse>();

            //Validation
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = errorMessage;

                return response;
            }


            //Create user
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName,
                Gender = registerDTO.Gender,
                UserType = nameof(UserTypeOptions.Employee),
                JobRole = registerDTO.JobRole,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //Check status of User Type
                if (registerDTO.UserType == UserTypeOptions.Admin)
                {
                    //Create 'Admin' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());

                }
                else if (registerDTO.UserType == UserTypeOptions.Employee)
                {
                    //Create 'Employee' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Employee.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Employee.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //Add the new user into 'Employee' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Employee.ToString());

                }
                
                //sign-in
                await _signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(user);

                response.StatusCode = 200;
                response.IsSuccess = true;
                response.Response = authenticationResponse;
                response.Message = "User registered and logged in successfully";

                return response;
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description)); //error1 | error2

                response.StatusCode = 500;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = errorMessage;

                return response;
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ResponseDTO<AuthenticationResponse>> PostLogin(UserLoginRequest loginDTO)
        {
            ResponseDTO<AuthenticationResponse> response = new ResponseDTO<AuthenticationResponse>();

            //Validation
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = errorMessage;

                return response;
            }


            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user == null)
                {
                    response.StatusCode = 404;
                    response.IsSuccess = false;
                    response.Response = null;
                    response.Message = "User Not Found";

                    return response;
                }

                //sign-in
                await _signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(user);

                await _userManager.UpdateAsync(user);


                response.StatusCode = 200;
                response.IsSuccess = true;
                response.Response = authenticationResponse;
                response.Message = "User logged in successfully";

                return response;
            }

            else
            {
                response.StatusCode = 404;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = "Wrong Credentials!";

                return response;
            }
        }
    }
}
