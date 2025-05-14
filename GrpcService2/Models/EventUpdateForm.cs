using System.ComponentModel.DataAnnotations;

namespace Buisness.Models;

public class EventUpdateForm
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
