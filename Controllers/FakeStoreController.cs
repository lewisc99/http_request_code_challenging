using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenging.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FakeStoreController: ControllerBase
    {
        public IFakeStoreService _service;

        public FakeStoreController(IFakeStoreService service) => _service = service;

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Product>>> get() => Ok(await _service.FindProductsAsync());
    }
}
