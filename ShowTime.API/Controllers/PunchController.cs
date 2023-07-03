using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowTime.Services.IServices;

namespace ShowTime.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _service;

        public PunchController(IPunchService service)
        {
            _service = service;
        }


    }
}
