using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using ShowTime.Services.IServices;

namespace ShowTime.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;

        public PunchController(IPunchService service)
        {
            _punchService = service;
        }

        [HttpPost]
        [Route("AddPunch")]
        public async Task<ResponseDTO<PunchDTO>> AddPunch(PunchAddRequest punchAddRequest)
        {
            ResponseDTO<PunchDTO> response = new ResponseDTO<PunchDTO>();

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

                if (punchAddRequest != null)
                {
                    var punch = await _punchService.AddPunch(punchAddRequest);

                    if (punch != null)
                    {
                        response.StatusCode = 200;
                        response.IsSuccess = true;
                        response.Response = punch;
                        response.Message = "User Punched successfully";

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
                    response.Message = "Failed while Punching";

                    return response;
                }
            }

        }


        [HttpGet]
        [Route("GetUserLatestPunchStatus/{userId:Guid}")]
        public async Task<ResponseDTO<bool?>> GetUserLatestPunchStatus([FromRoute] Guid userId)
        {
            ResponseDTO<bool?> response = new ResponseDTO<bool?>();

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
                var status = await _punchService.GetPunchStatus(userId);

                if (status != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = status;
                    response.Message = "User Punch fetched successfully";

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
        [Route("GetAllPunchedInUsers")]
        public async Task<ResponseDTO<List<PunchDTO>>> GetAllPunchedInUsers()
        {
            ResponseDTO<List<PunchDTO>> response = new ResponseDTO<List<PunchDTO>>();

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
                var punchedInUsers = await _punchService.GetAllPunchedInUsers();

                if (punchedInUsers != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = punchedInUsers;
                    response.Message = "PunchedIn users fetched successfully";

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
        [Route("GetAllUserPunchesForToday/{userId:Guid}")]
        public async Task<ResponseDTO<List<PunchDTO>>> GetAllUserPunchesForToday(Guid userId)
        {
            ResponseDTO<List<PunchDTO>> response = new ResponseDTO<List<PunchDTO>>();

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
                var punches = await _punchService.GetAllUserPunchesForToday(userId);

                if (punches != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = punches;
                    response.Message = "User's all punches for today fetched successfully";

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
        [Route("CalculateTotalPunchedInTime/{userId:Guid}")]
        public async Task<ResponseDTO<TimeSpan>> CalculateTotalPunchedInTime(Guid userId)
        {
            ResponseDTO<TimeSpan> response = new ResponseDTO<TimeSpan>();

            if (!ModelState.IsValid)
            {
                response.StatusCode = 400;
                response.IsSuccess = false;
                response.Response = TimeSpan.Zero;
                response.Message = "Bad Request, One or more validation errors occured.";

                return response;
            }
            else
            {
                var punchedInTime = await _punchService.CalculateTotalPunchedInTime(userId);

                if (punchedInTime != TimeSpan.Zero)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = punchedInTime;
                    response.Message = "User's all punches for today fetched successfully";

                    return response;
                }
                else
                {
                    response.StatusCode = 500;
                    response.IsSuccess = false;
                    response.Response = TimeSpan.Zero;
                    response.Message = "Internal Server Error";

                    return response;
                }
            }

        }


        [HttpGet]
        [Route("GetFiveDaysWorkingTime/{userId:Guid}")]
        public async Task<ResponseDTO<List<WorkingTimeDTO>>> GetFiveDaysWorkingTime(Guid userId)
        {
            ResponseDTO<List<WorkingTimeDTO>> response = new ResponseDTO<List<WorkingTimeDTO>>();

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
                var workingTimes = await _punchService.GetFiveDaysWorkingTime(userId);

                if (workingTimes != null )
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Response = workingTimes;
                    response.Message = "User's all punches for today fetched successfully";

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
