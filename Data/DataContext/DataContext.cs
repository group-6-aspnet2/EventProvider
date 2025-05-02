using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<EventEntity> Events { get; set; } 
}
