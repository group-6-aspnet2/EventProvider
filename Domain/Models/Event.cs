using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Event
{
    public string EventId { get; set; } = null!;

    public string EventName { get; set; } = null!;

    public string EventCategory { get; set; } = null!;

    public string EventLocation { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public TimeOnly EventTime { get; set; }

    public Int32 EventAmountOfGuests { get; set; }

    public string EventStatus { get; set; } = null!;
}
