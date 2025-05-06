using Data.Contexts;
using Data.Entities;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;

public interface IEventRepository : IBaseRepository<EventEntity>
{
    Task<ResponseResult<IEnumerable<EventEntity>>> GetAllAsync();
}

public class EventRepository(DataContext context) : BaseRepository<EventEntity>(context), IEventRepository
{
    public async Task<ResponseResult<IEnumerable<EventEntity>>> GetAllAsync()
    {
        try
        {
            var entities = await _dbSet.ToListAsync();
            return new ResponseResult<IEnumerable<EventEntity>> { Success = true, StatusCode = 200, Data = entities };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ResponseResult<IEnumerable<EventEntity>> { Success = false, StatusCode = 500, Error = "An error occurred while retrieving the entities" };
        }
    }
}
