using Data.Contexts;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<ResponseResult> CreateAsync(TEntity entity);
    Task<ResponseResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
    Task<ResponseResult<IEnumerable<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
    Task<ResponseResult> UpdateAsync(TEntity entity);
    Task<ResponseResult> DeleteAsync(Expression<Func<TEntity, bool>> expression);
}


public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<ResponseResult> CreateAsync(TEntity entity)
    {
        try
        {
            if (entity == null)
                return new ResponseResult { Success = false, StatusCode = 400, Error = "Entity cannot be null" };

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return new ResponseResult { Success = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ResponseResult { Success = false, StatusCode = 500, Error = "An error occurred while creating the entity" };
        }
    }


    public virtual async Task<ResponseResult<IEnumerable<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            if (expression == null)
                return new ResponseResult<IEnumerable<TEntity>> { Success = false, StatusCode = 400, Error = "Expression cannot be null" };

            var entities = await _dbSet.Where(expression).ToListAsync();
            return new ResponseResult<IEnumerable<TEntity>> { Success = true, StatusCode = 200, Result = entities };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ResponseResult<IEnumerable<TEntity>> { Success = false, StatusCode = 500, Error = "An error occurred while retrieving the entities" };
        }
    }

    public virtual async Task<ResponseResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            if (expression == null)
                return new ResponseResult<TEntity> { Success = false, StatusCode = 400, Error = "Expression cannot be null" };

            var entity = await _dbSet.FirstOrDefaultAsync(expression);
            if (entity == null)
                return new ResponseResult<TEntity> { Success = false, StatusCode = 404, Error = "Entity not found" };

            return new ResponseResult<TEntity> { Success = true, StatusCode = 200, Result = entity };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ResponseResult<TEntity> { Success = false, StatusCode = 500, Error = "An error occurred while retrieving the entity" };
        }
    }

    public virtual async Task<ResponseResult> UpdateAsync(TEntity entity)
    {
        try
        {
            if (entity == null)
                return new ResponseResult { Success = false, StatusCode = 400, Error = "Expression cannot be null" };

            if (!await _dbSet.ContainsAsync(entity))
                return new ResponseResult { Success = false, StatusCode = 404, Error = "Entity not found" };

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return new ResponseResult { Success = true, StatusCode = 200 };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ResponseResult { Success = false, StatusCode = 500, Error = "An error occurred while updating the entity" };
        }
    }

    public virtual async Task<ResponseResult> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            if (expression == null)
                return new ResponseResult { Success = false, StatusCode = 400, Error = "Expression cannot be null" };

            var entity = await _dbSet.FirstOrDefaultAsync(expression);
            if (entity == null)
                return new ResponseResult { Success = false, StatusCode = 404, Error = "Entity not found" };

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return new ResponseResult { Success = true, StatusCode = 200 };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ResponseResult { Success = false, StatusCode = 500, Error = "An error occurred while deleting the entity" };
        }
    }
}