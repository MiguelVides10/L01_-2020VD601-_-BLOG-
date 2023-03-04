using Microsoft.EntityFrameworkCore;

namespace L01_2020VD601.Models
{
    public class blogContext : DbContext
    {
        public blogContext(DbContextOptions<blogContext> options) : base(options)
        { }

        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
        public DbSet<calificaciones> calificaciones { get;set; }
    }
}
