using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventPresentation_Tests.Repositories;

public class EventRepository_Tests
{
    private readonly DataContext _context;
    private readonly IEventRepository _eventRepository;

    public EventRepository_Tests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new DataContext(options);
        _eventRepository = new EventRepository(_context);
    }

    #region CreateEvent

    [Fact]
    public async Task CreateEvent_ShouldReturnSuccessAndSavedInDatabase_WhenEventIsCreated()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            EventId = Guid.NewGuid().ToString(),
            EventName = "Test Event",
            EventCategoryName = "Test Category",
            EventLocation = "Test Location",
            EventDate = DateTime.Now,
            EventTime = new TimeOnly(19,0),
            EventAmountOfGuests = 100,
            EventStatus = "Scheduled"
        };

        // Act
        var result = await _eventRepository.CreateAsync(eventEntity);
        var savedEvent = await _context.Events.FirstOrDefaultAsync(x => x.EventId == eventEntity.EventId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(201, result.StatusCode);
        Assert.NotNull(savedEvent);
    }

    #endregion

    #region GetAllEvents

    [Fact]
    public async Task GetAllEvent_ShouldReturnAllEvents_WhenEventsExist()
    {
        // Arrange
        var eventEntity1 = new EventEntity
        {
            EventId = Guid.NewGuid().ToString(),
            EventName = "Test Event 1",
            EventCategoryName = "Test Category 1",
            EventLocation = "Test Location 1",
            EventDate = DateTime.Now,
            EventTime = new TimeOnly(19, 0),
            EventAmountOfGuests = 100,
            EventStatus = "Coming"
        };
        var eventEntity2 = new EventEntity
        {
            EventId = Guid.NewGuid().ToString(),
            EventName = "Test Event 2",
            EventCategoryName = "Test Category 2",
            EventLocation = "Test Location 2",
            EventDate = DateTime.Now,
            EventTime = new TimeOnly(20, 0),
            EventAmountOfGuests = 200,
            EventStatus = "Draft"
        };
        await _eventRepository.CreateAsync(eventEntity1);
        await _eventRepository.CreateAsync(eventEntity2);

        // Act
        var result = await _eventRepository.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.NotEmpty(result.Result!);
        Assert.Equal(2, result.Result!.Count());
    }



    #endregion

    #region GetEventById

    [Fact]
    public async Task GetEventById_ShouldReturnEvent_WhenEventExists()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            EventId = Guid.NewGuid().ToString(),
            EventName = "Test Event",
            EventCategoryName = "Test Category",
            EventLocation = "Test Location",
            EventDate = DateTime.Now,
            EventTime = new TimeOnly(19, 0),
            EventAmountOfGuests = 100,
            EventStatus = "Scheduled"
        };
        await _eventRepository.CreateAsync(eventEntity);

        // Act
        var result = await _eventRepository.GetAsync(x => x.EventId == eventEntity.EventId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Result);
        Assert.Equal(eventEntity.EventId, result.Result!.EventId);
    }

    #endregion

    #region UpdateEvent

    [Fact]
    public async Task UpdateEvent_ShouldReturnSuccess_WhenEventIsUpdated()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            EventId = Guid.NewGuid().ToString(),
            EventName = "Test Event",
            EventCategoryName = "Test Category",
            EventLocation = "Test Location",
            EventDate = DateTime.Now,
            EventTime = new TimeOnly(19, 0),
            EventAmountOfGuests = 100,
            EventStatus = "Scheduled"
        };
        await _eventRepository.CreateAsync(eventEntity);

        // Act
        eventEntity.EventName = "Updated Test Event";
        var result = await _eventRepository.UpdateAsync(eventEntity);
        var updatedEvent = await _context.Events.FirstOrDefaultAsync(x => x.EventId == eventEntity.EventId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(updatedEvent);
        Assert.Equal("Updated Test Event", updatedEvent!.EventName);
    }

    #endregion

    #region DeleteEvent

    [Fact]
    public async Task DeleteEvent_ShouldReturnSuccessAndGetRemovedFromDatabase_WhenEventIsDeleted()
    {
        // Arrange
        var eventEntity = new EventEntity
        {
            EventId = Guid.NewGuid().ToString(),
            EventName = "Test Event",
            EventCategoryName = "Test Category",
            EventLocation = "Test Location",
            EventDate = DateTime.Now,
            EventTime = new TimeOnly(19, 0),
            EventAmountOfGuests = 100,
            EventStatus = "Scheduled"
        };
        await _eventRepository.CreateAsync(eventEntity);

        // Act
        var result = await _eventRepository.DeleteAsync(x => x.EventId == eventEntity.EventId);
        var deletedEvent = await _context.Events.FirstOrDefaultAsync(x => x.EventId == eventEntity.EventId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.Null(deletedEvent);
    }

    #endregion
}
