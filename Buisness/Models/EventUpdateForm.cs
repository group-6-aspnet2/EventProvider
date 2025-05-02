using System.ComponentModel.DataAnnotations;

namespace Buisness.Models;

public class EventUpdateForm
{
    [Required]
    string EventName { get; set; } = null!;

    [Required]
    string EventCategory { get; set; } = null!;

    [Required]
    string EventLocation { get; set; } = null!;

    [Required]
    DateTime EventDate { get; set; }

    [Required]
    TimeOnly EventTime { get; set; }

    [Required]
    int EventAmountOfGuests { get; set; }

    [Required]
    string EventStatus { get; set; } = null!;
}
