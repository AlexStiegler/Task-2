using Microsoft.EntityFrameworkCore;
using Second_project.Models.Data;
using System.Collections.Generic;

namespace Second_project.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }
        public DbSet<WeatherData> WeatherData { get; set; }

    }

}
