using System.ComponentModel.DataAnnotations;

namespace Buisness.Models;

public class EventUpdateForm
{
    [Required]
    public string EventId { get; set; } = null!;

    [Required]
    public string EventName { get; set; } = null!;

    [Required]
    public string EventCategoryName { get; set; } = null!;

    [Required]
    public string EventLocation { get; set; } = null!;

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    public TimeOnly EventTime { get; set; }

    [Required]
    public Int32 EventAmountOfGuests { get; set; }

    [Required]
    public string EventStatus { get; set; } = null!;
}
