using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowTime.Core.DTO;
using ShowTime.Core.IdentityEntities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShowTime.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GeneralController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GeneralController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllUsersDob")]
        public async Task<ResponseDTO<IEnumerable<DobDTO>>> GetAllUsersDob()
        {
            ResponseDTO<IEnumerable<DobDTO>> response = new ResponseDTO<IEnumerable<DobDTO>>();

            List<DobDTO> allUsersDob = new List<DobDTO>();

            List<ApplicationUser> users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                allUsersDob.Add(new DobDTO
                {
                    DateOfBirth = user.DateOfBirth,
                    UserId = user.Id,
                    PersonName = user.PersonName,
                    email = user.Email
                });
            }
            if (users != null)
            {
                response.StatusCode = 200;
                response.IsSuccess = true;
                response.Response = allUsersDob;
                response.Message = "All Users DOB Fetched Succesfully";

                return response;
            }
            else
            {
                response.StatusCode = 500;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = "Internal Server Error";

                return response;
            }

        }
    }
}
