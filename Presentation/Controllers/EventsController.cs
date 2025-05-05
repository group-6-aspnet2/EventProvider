using EventGrpcContract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateEventRequest createRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            
            return Ok();
        }


        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateEventRequest updateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }


        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteEventRequest deleteRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
    }
}
