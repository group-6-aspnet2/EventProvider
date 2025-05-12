using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class EventEntity
{
    [Key]
    public string EventId { get; set; } = null!;

    public string EventName { get; set; } = null!;

    public string EventCategoryName { get; set; } = null!;

    public string EventLocation { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public TimeOnly EventTime { get; set; }

    public string EventStatus { get; set; } = null!;

    public Int32 EventAmountOfGuests { get; set; }
}
