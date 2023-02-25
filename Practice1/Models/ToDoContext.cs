using Microsoft.EntityFrameworkCore;

namespace Practice1.Models
{
    public class ToDoContext:DbContext
    {
        public ToDoContext(DbContextOptions options) : base(options) { }
        public ToDoContext() { }
        public DbSet<ToDoItem> toDos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\77071\\source\\repos\\WPHW\\Practice1\\Data\\ToDoDb.mdf;Integrated Security=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
