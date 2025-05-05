using Data.Contexts;
using Domain.Responses;
using EventGrpcContract;

namespace Buisness.Services;

public class EventService(DataContext context) : EventContract.EventContractBase
{
    private readonly DataContext _context = context;

    public async Task<ResponseResult> Create(CreateEventRequest createRequest)
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
