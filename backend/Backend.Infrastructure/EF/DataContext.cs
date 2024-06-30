using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.EF;

public class DataContext : DbContext
{
    public DbSet<CryptoToken> Tokens { get; set; } = default!;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
}