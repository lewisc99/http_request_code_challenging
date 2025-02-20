using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypicodeController
    {

        public IJsonplaceholderClientService _service;
        public TypicodeController(IJsonplaceholderClientService service) => _service = service;

        [HttpGet()]
        public async Task<ActionResult<JsonPlaceHolder[]>> get([FromQuery] int Id, [FromQuery] int UserId)
        {
            JsonPlaceHolder[] result =  await _service.Get(Id, UserId);

            return result;
        }
    }
}
