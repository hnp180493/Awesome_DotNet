using Microsoft.EntityFrameworkCore;
using UploadFile.Models;

namespace UploadFile
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}
