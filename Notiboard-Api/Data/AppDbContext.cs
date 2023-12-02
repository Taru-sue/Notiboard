using Microsoft.EntityFrameworkCore;
using Notiboard_Api.Model;

namespace Notiboard_Api.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Group> Groups { get; set; }

    }
}
