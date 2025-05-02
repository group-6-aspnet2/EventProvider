using Data.DataContext;
using EventGrpcService;

namespace Buisness.Services;

public class EventService(DataContext context) : EventContract.EventContractBase
{
    private readonly DataContext _context = context;

    public async Task<bool> Create(CreateEventRequest createRequest)
    {
        try
        {
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
}
