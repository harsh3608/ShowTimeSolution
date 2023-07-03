using Azure;
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
        [Route("GetPunchStatus")]
        public async Task<ResponseDTO<string>> GetPunchStatus(Guid userId)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();

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
    }
}
