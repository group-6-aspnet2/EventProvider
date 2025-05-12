using Data.Contexts;
using Data.Entities;
using EventGrpcContract;
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
            EventId = Guid.NewGuid().ToString(),
            EventName = request.EventName,
            EventCategoryName = request.EventCategoryName,
            EventLocation = request.EventLocation,
            EventDate = DateTime.Parse(request.EventDate),
            EventTime = TimeOnly.Parse(request.EventTime),
            EventAmountOfGuests = request.EventAmountOfGuests,
            EventStatus = request.EventStatus
        };

        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();

        return new CreateEventReply
        {
            Succeeded = true,
            Message = "Event was created.",
            EventId = newEvent.EventId
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
                Message = "Event not found.",
                Event = null
            };
        }

        var eventReply = new Event
        {
            EventId = getEvent.EventId,
            EventName = getEvent.EventName,
            EventCategoryName = getEvent.EventCategoryName,
            EventLocation = getEvent.EventLocation,
            EventDate = getEvent.EventDate.ToString("yyyy-MM-dd"),
            EventTime = getEvent.EventTime.ToString("HH:mm"),
            EventStatus = getEvent.EventStatus,
            EventAmountOfGuests = getEvent.EventAmountOfGuests
        };

        return new GetEventByIdReply
        {
            Succeeded = true,
            Message = "Event was found.",
            Event = eventReply
        };
    }

    public override async Task<GetAllEventsReply> GetAllEvents(GetAllEventsRequest request, ServerCallContext callContext)
    {
        var events = await _context.Events.ToListAsync();

        var reply = new GetAllEventsReply();
        if (events.Count == 0)
        {
            reply.Succeeded = false;
            reply.Message = "No events found.";
            return reply;
        }

        foreach (var eventEntity in events)
        {
            reply.Events.Add(new Event
            {
                EventId = eventEntity.EventId,
                EventName = eventEntity.EventName,
                EventCategoryName = eventEntity.EventCategoryName,
                EventLocation = eventEntity.EventLocation,
                EventDate = eventEntity.EventDate.ToString("yyyy-MM-dd"),
                EventTime = eventEntity.EventTime.ToString("HH:mm"),
                EventStatus = eventEntity.EventStatus,
                EventAmountOfGuests = eventEntity.EventAmountOfGuests
            });
        }

        reply.Succeeded = true;
        reply.Message = "Events retrieved successfully.";

        return reply;
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
            
            updateEvent.EventId = request.EventId;
            updateEvent.EventName = request.EventName;
            updateEvent.EventCategoryName = request.EventCategoryName;
            updateEvent.EventLocation = request.EventLocation;
            updateEvent.EventDate = DateTime.Parse(request.EventDate);
            updateEvent.EventTime = TimeOnly.Parse(request.EventTime);
            updateEvent.EventStatus = request.EventStatus;
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