using Data.DataContext;
using Data.Entities;
using EventGrpcService;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Services;

public class EventGrpcService(DataContext context) : EventContract.EventContractBase
{
    private readonly DataContext _context = context;

    public override async Task<CreateEventReply> CreateEvent(CreateEventRequest request, ServerCallContext callContext)
    {
        var createEvent = await _context.Events.FirstOrDefaultAsync(x => x.EventName == request.EventName);
        if (createEvent != null)
        {
            return new CreateEventReply
            {
                Succeeded = false,
                Message = "Event already exists."
            };
        }

        var newEvent = new EventEntity
        {
            EventName = request.EventName,
            EventCategoryName = request.EventCategory,
            EventLocation = request.EventLocation,
            EventDate = DateTime.Parse(request.EventDate),
            EventTime = TimeOnly.Parse(request.EventTime),
            EventAmountOfGuests = request.EventAmountOfGuests
        };

        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();

        return new CreateEventReply
        {
            Succeeded = true,
            Message = "Event was created."
        };
    }   
    
    public override async Task<GetEventByIdReply> GetEventById(GetEventByIdRequest request, ServerCallContext callContext)
    {
        var getEvent = await _context.Events.FindAsync(request.EventId);
        if (getEvent == null)
        {
            return new GetEventByIdReply
            {
                Succeeded = false,
                Message = "Event not found."
            };
        }
        return new GetEventByIdReply
        {
            Succeeded = true,
            Message = "Event was found.",
            Event 
        };
    }

    public override async Task<UpdateEventReply> UpdateEvent(UpdateEventRequest request, ServerCallContext callContext)
    {
        var updateEvent = await _context.Events.FindAsync(request.EventId);
        if (updateEvent == null)
        {
            return new UpdateEventReply
            {
                Succeeded = false,
                Message = "Event not found."
            };
        }
            updateEvent.EventName = request.EventName;
            updateEvent.EventCategoryName = request.EventCategory;
            updateEvent.EventLocation = request.EventLocation;
            updateEvent.EventDate = DateTime.Parse(request.EventDate);
            updateEvent.EventTime = TimeOnly.Parse(request.EventTime);
            updateEvent.EventAmountOfGuests = request.EventAmountOfGuests;
            await _context.SaveChangesAsync();
        
        return new UpdateEventReply
        {
            Succeeded = true,
            Message = "Event was updated."
        };
    }

    public override async Task<DeleteEventReply> DeleteEvent(DeleteEventRequest request, ServerCallContext callContext)
    {
        var deleteEvent = await _context.Events.FindAsync(request.EventId);
        if (deleteEvent == null)
        {
            return new DeleteEventReply
            {
                Succeeded = false,
                Message = "Event not found."
            };
        }

        _context.Events.Remove(deleteEvent);
        await _context.SaveChangesAsync();

        return new DeleteEventReply
        {
            Succeeded = true,
            Message = "Event was deleted."
        };
    }
}