using Microsoft.EntityFrameworkCore;

namespace WPHW5.Models
{
    public class WeatherContext:DbContext
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public WeatherContext(DbContextOptions options):base(options) { }
        public WeatherContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("WeatherContext");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
