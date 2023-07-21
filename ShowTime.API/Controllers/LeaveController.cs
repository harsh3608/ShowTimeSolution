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
                        response.Message = "Leave request added successfully";

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
        [Route("DeleteLeaveRequest/{leaveId:Guid}")]
        public async Task<ResponseDTO<LeaveDTO>> DeleteLeaveRequest([FromRoute] Guid leaveId)
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


        [HttpGet]
        [Route("GetAllLeaveRequests")]
        public async Task<ResponseDTO<IEnumerable<LeaveDTO>>> GetAllLeaveRequests()
        {
            ResponseDTO<IEnumerable<LeaveDTO>> response = new ResponseDTO<IEnumerable<LeaveDTO>>();

            var result = await _leaveService.GetAllLeaveRequests();

            if (result != null)
            {
                response.StatusCode = 200;
                response.IsSuccess = true;
                response.Response = result;
                response.Message = "All leave requests fetched successfully.";

                return response;
            }
            else
            {
                response.StatusCode = 401;
                response.IsSuccess = false;
                response.Response = null;
                response.Message = "Leave Requests Not Found.";

                return response;
            }
        }


        [HttpGet]
        [Route("GetLeaveRequestById/{leaveId:Guid}")]
        public async Task<ResponseDTO<LeaveDTO>> GetLeaveRequestById([FromRoute] Guid leaveId)
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
                var leave = await _leaveService.GetLeaveRequest(leaveId);

                if (leave != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = leave;
                    response.Message = "Leave request fetched successfully";

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


        [HttpGet]
        [Route("GetUserAllLeaves/{userId:Guid}")]
        public async Task<ResponseDTO<IEnumerable<LeaveDTO>>> GetUserAllLeaves([FromRoute] Guid userId)
        {
            ResponseDTO<IEnumerable<LeaveDTO>> response = new ResponseDTO<IEnumerable<LeaveDTO>>();

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
                var requests = await _leaveService.GetUserAllLeaves(userId);

                if (requests != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = requests;
                    response.Message = "Leave requests fetched successfully";

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


        [HttpPatch]
        [Route("ToggleLeaveStatus")]
        public async Task<ResponseDTO<LeaveDTO>> ToggleLeaveStatus(ToggleRequest data)
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
                var leave = await _leaveService.ToggleLeaveStatus(data.LeaveId, data.value);

                if (leave != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = leave;
                    response.Message = "Leave request updated successfully";

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
