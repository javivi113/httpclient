using Microsoft.EntityFrameworkCore;

namespace Tiempo.Models
{
    public class TiempoContext : DbContext
    {
        public TiempoContext(DbContextOptions<TiempoContext> options)
            : base(options)
        {
        }

        public DbSet<Tiempo2> Tiempo { get; set; }
        public string connString { get; private set; }
        public TiempoContext()
        {
            connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database=DB07Javier;User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
            //connString = $"Server=localhost;Database=EFPrueba;User Id=sa;Password=Pa88word;";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)=> options.UseSqlServer(connString);
    }
}