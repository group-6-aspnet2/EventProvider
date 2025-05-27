using Buisness.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Documentation;

public class EventRegistrationForm_Example : IExamplesProvider<EventRegistrationForm>
{
    public EventRegistrationForm GetExamples() => new()
    {
        EventName = "Sample Event",
        EventCategoryName = "Sample Category",
        EventLocation = "Sample Location",
        EventDate = DateTime.Now.AddDays(7),
        EventTime = new TimeOnly(14, 30),
        EventStatus = "Scheduled",
        EventAmountOfGuests = 100
    };
}
