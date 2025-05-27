using Buisness.Models;
using Buisness.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Documentation;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class EventsController(IEventService eventService) : ControllerBase
    {
        private readonly IEventService _eventService = eventService;

        [HttpPost]
        [SwaggerOperation(Summary = "Create new events.")]
        [SwaggerResponse(200, "Event added to event list successfully.")]
        [SwaggerResponse(400, "Event request contained invalid properties or missing properties.")]
        [SwaggerRequestExample(typeof(EventRegistrationForm), typeof(EventRegistrationForm_Example))]

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
        [SwaggerOperation(Summary = "Returns a specific event.")]
        [SwaggerResponse(200, "Event recived successfully.")]
        [SwaggerResponse(400, "Event Id is missing or invalid.")]

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
        [SwaggerOperation(Summary = "Returns a list of events.")]
        [SwaggerResponse(200, "Events recived successfully.")]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _eventService.GetAllEventsAsync();
            if (!result.Success)
                return NotFound();

            return result.Success ? Ok(result) : BadRequest(result.Result);
        }


        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates a specific event.")]
        [SwaggerResponse(200, "Event was updated successfully.")]
        [SwaggerResponse(400, "Event request contained invalid properties or missing properties.")]
        [SwaggerRequestExample(typeof(EventRegistrationForm), typeof(EventRegistrationForm_Example))]

        public async Task<IActionResult> Update(string id, EventUpdateForm form)
        {         
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
        [SwaggerOperation(Summary = "Deletes a specific event.")]
        [SwaggerResponse(200, "Event was deleted successfully.")]
        [SwaggerResponse(400, "Event Id is missing or invalid.")]

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _eventService.DeleteEventAsync(id);
            return result.Success ? Ok(result) : BadRequest(result.Error);
        }
    }
}
