using Buisness.Models;
using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;

namespace Buisness.Services;

public interface IEventService 
{ 
    public Task<ResponseResult<Event>> CreateEventAsync(EventRegistrationForm form);
    public Task<ResponseResult<Event>> GetEventAsync(string id);
    public Task<ResponseResult<IEnumerable<Event>>> GetAllEventsAsync();
    public Task<ResponseResult> UpdateEventAsync(EventUpdateForm form);
    public Task<ResponseResult> DeleteEventAsync(string id);
}

public class EventService(IEventRepository eventRepository, DataContext context) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly DataContext _context = context;

    public async Task<ResponseResult<Event>> CreateEventAsync(EventRegistrationForm form)
    {
        try
        {
            if (form == null)
                return null!;

            var eventEntity = form.MapTo<EventEntity>();
            
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            var eventModel = eventEntity.MapTo<Event>();
            return new ResponseResult<Event> { Success = true, StatusCode = 201, Result = eventModel
            };
        }
        catch (Exception ex)
        {
            return new ResponseResult<Event> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ResponseResult<Event>> GetEventAsync(string id)
    {
        try
        {
            var result = await _eventRepository.GetAsync(x => x.EventId == id);

            if (result == null)
                return new ResponseResult<Event> { Success = false, StatusCode = 404, Error = "Event not found" };

            if(result.Result != null)
            {
                var eventModel = result.Result.MapTo<Event>();
            return new ResponseResult<Event> { Success = true, StatusCode = 200, Result = eventModel };

            }
            return new ResponseResult<Event> { Success = true, StatusCode = 404 };


        }
        catch (Exception ex)
        {
            return new ResponseResult<Event> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }


    public async Task<ResponseResult<IEnumerable<Event>>> GetAllEventsAsync()
    {
        try
        {
            var result = await _eventRepository.GetAllAsync();

            if (!result.Success)
                return new ResponseResult<IEnumerable<Event>> { Success = false, StatusCode = result.StatusCode, Error = result.Error };

            var events = result.Result?.Select(x => x.MapTo<Event>()) ?? Enumerable.Empty<Event>();
            return new ResponseResult<IEnumerable<Event>> { Success = true, StatusCode = 200, Result = events };
        }
        catch (Exception ex)
        {
            return new ResponseResult<IEnumerable<Event>> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ResponseResult> UpdateEventAsync(EventUpdateForm form)
    {
        try
        {
            var eventEntity = await _eventRepository.GetAsync(x => x.EventId == form.EventId);
            if (eventEntity == null)
                return null!;

            var entity = form.MapTo<EventEntity>();
            var result = await _eventRepository.UpdateAsync(entity);

            if (result.Success)
                return new ResponseResult { Success = true, StatusCode = 200 };

            return new ResponseResult { Success = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            return new ResponseResult { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ResponseResult> DeleteEventAsync(string id)
    {
        try
        {
            var result = await _eventRepository.DeleteAsync(x => x.EventId == id);

            if (result.Success)
                return new ResponseResult { Success = true, StatusCode = 200 };

            return new ResponseResult { Success = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            return new ResponseResult { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }


}
