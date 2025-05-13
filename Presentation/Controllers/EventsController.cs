using EventGrpcContract;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(EventContract.EventContractClient eventClient) : ControllerBase
    {
        private readonly EventContract.EventContractClient _eventClient = eventClient;

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest createRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createRequest != null)
            {
                var request = new CreateEventRequest
                {
                    EventName = createRequest.EventName,
                    EventCategoryName = createRequest.EventCategoryName,
                    EventLocation = createRequest.EventLocation,
                    EventDate = createRequest.EventDate,
                    EventTime = createRequest.EventTime,
                    EventAmountOfGuests = createRequest.EventAmountOfGuests,
                    EventStatus = createRequest.EventStatus
                };

                var response = await _eventClient.CreateEventAsync(request);

                return response.Succeeded ? Ok(response) : BadRequest(response.Message);
            }

            return BadRequest();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _eventClient.GetEventByIdAsync(new GetEventByIdRequest { EventId = id });
            if (!result.Succeeded)
                return NotFound();

            return result.Succeeded ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents(GetAllEventsRequest getAllRequest)
        {
            var result = await _eventClient.GetAllEventsAsync(new GetAllEventsRequest());
            if (!result.Succeeded)
                return NotFound();

            return result.Succeeded ? Ok(result) : BadRequest(result.Message);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateEventRequest updateRequest)
        {
            var authorization = Request.Headers.Authorization[0];

            /* bearer TOKENNYCKELN - splittningen görs i React */
            var token = authorization!.Split(" ")[1];

            using var http = new HttpClient();
            var response = await http.PostAsJsonAsync("http://tokenservice.azurewebsite.net/api/validatetoken", new { token = token });
            if (!response.IsSuccessStatusCode)
                return Unauthorized();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var eventEntity = await _eventClient.GetEventByIdAsync(new GetEventByIdRequest { EventId = id });
            if (!eventEntity.Succeeded)
                return NotFound();

            var request = new UpdateEventRequest
            {
                EventId = updateRequest.EventId,
                EventName = updateRequest.EventName,
                EventCategoryName = updateRequest.EventCategoryName,
                EventLocation = updateRequest.EventLocation,
                EventDate = updateRequest.EventDate,
                EventTime = updateRequest.EventTime,
                EventAmountOfGuests = updateRequest.EventAmountOfGuests,
                EventStatus = updateRequest.EventStatus
            };

            var result = await _eventClient.UpdateEventAsync(updateRequest);
            return result.Succeeded ? Ok(result) : BadRequest(result.Message);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var authorization = Request.Headers.Authorization[0];

            /* bearer TOKENNYCKELN - splittningen görs i React */
            var token = authorization!.Split(" ")[1];

            using var http = new HttpClient();
            var response = await http.PostAsJsonAsync("http://tokenservice.azurewebsite.net/api/validatetoken", new { token = token }); //görs via Grpc istället
            if (!response.IsSuccessStatusCode)
                return Unauthorized();


            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _eventClient.DeleteEventAsync(new DeleteEventRequest { EventId = id });
            return result.Succeeded ? Ok(result) : BadRequest(result.Message);
        }
    }
}
