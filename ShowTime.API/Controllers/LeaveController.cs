using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using ShowTime.Services.IServices;
using ShowTime.Services.Services;

namespace ShowTime.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost]
        [Route("AddLeaveRequest")]
        public async Task<ResponseDTO<LeaveDTO>> AddLeaveRequest(LeaveAddRequest leaveAddRequest)
        {
            ResponseDTO<LeaveDTO> response = new ResponseDTO<LeaveDTO>();

            if (!ModelState.IsValid)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = "Bad Request, One or more validation errors occured.";

                return response;
            }
            else
            {
                if (leaveAddRequest != null)
                {
                    var leave = await _leaveService.AddLeaveRequest(leaveAddRequest);

                    if (leave != null)
                    {
                        response.StatusCode = 200;
                        response.IsSuccess = true;
                        response.Response = leave;
                        response.Message = "Added leave request successfully";

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
                else
                {
                    response.StatusCode = 400;
                    response.IsSuccess = false;
                    response.Response = null;
                    response.Message = "Failed while Adding leave request";

                    return response;
                }
            }
        }


        [HttpDelete]
        [Route("DeleteLeaveRequest")]
        public async Task<ResponseDTO<LeaveDTO>> DeleteLeaveRequest(Guid leaveId)
        {
            ResponseDTO<LeaveDTO> response = new ResponseDTO<LeaveDTO>();

            if (!ModelState.IsValid)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = "Bad Request, One or more validation errors occured.";

                return response;
            }
            else
            {
                var deletedRequest = await _leaveService.DeleteLeaveRequest(leaveId);

                if (deletedRequest != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = deletedRequest;
                    response.Message = "Leave request deleted successfully";

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
}
