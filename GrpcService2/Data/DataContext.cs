using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrpcService2.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext(options)
{
}
