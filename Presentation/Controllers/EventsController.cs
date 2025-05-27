using Buisness.Models;
using Buisness.Services;
using EventGrpcContract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    //[Produces("application/json")]
    //[Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(IEventService eventService) : ControllerBase
    {
        private readonly IEventService _eventService = eventService;

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventRegistrationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (form != null)
            {
                var request = new EventRegistrationForm
                {
                    EventName = form.EventName,
                    EventCategoryName = form.EventCategoryName,
                    EventLocation = form.EventLocation,
                    EventDate = form.EventDate,
                    EventTime = form.EventTime,
                    EventAmountOfGuests = form.EventAmountOfGuests,
                    EventStatus = form.EventStatus
                };

                var response = await _eventService.CreateEventAsync(request);

                return response.Success ? Ok(response) : BadRequest(response.Result);
            }

            return BadRequest();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _eventService.GetEventAsync(id);
            if (!result.Success)
                return NotFound();

            return result.Success ? Ok(result) : BadRequest(result.Result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _eventService.GetAllEventsAsync();
            if (!result.Success)
                return NotFound();

            return result.Success ? Ok(result) : BadRequest(result.Result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, EventUpdateForm form)
        {
            //var authorization = Request.Headers.Authorization[0];

            ///* bearer TOKENNYCKELN - splittningen görs i React */
            //var token = authorization!.Split(" ")[1];

            //using var http = new HttpClient();
            //var response = await http.PostAsJsonAsync("http://tokenservice.azurewebsite.net/api/validatetoken", new { token = token });
            //if (!response.IsSuccessStatusCode)
            //    return Unauthorized();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var eventEntity = await _eventService.GetEventAsync(id);
            if (!eventEntity.Success)
                return NotFound();

            var request = new EventUpdateForm
            {
                EventId = form.EventId,
                EventName = form.EventName,
                EventCategoryName = form.EventCategoryName,
                EventLocation = form.EventLocation,
                EventDate = form.EventDate,
                EventTime = form.EventTime,
                EventAmountOfGuests = form.EventAmountOfGuests,
                EventStatus = form.EventStatus
            };

            var result = await _eventService.UpdateEventAsync(form);
            return result.Success ? Ok(result) : BadRequest(result.Error);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            //var authorization = Request.Headers.Authorization[0];

            ///* bearer TOKENNYCKELN - splittningen görs i React */
            //var token = authorization!.Split(" ")[1];

            //using var http = new HttpClient();
            //var response = await http.PostAsJsonAsync("http://tokenservice.azurewebsite.net/api/validatetoken", new { token = token }); //görs via Grpc istället
            //if (!response.IsSuccessStatusCode)
            //    return Unauthorized();


            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _eventService.DeleteEventAsync(id);
            return result.Success ? Ok(result) : BadRequest(result.Error);
        }
    }
}
