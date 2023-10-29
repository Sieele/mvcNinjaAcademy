using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvcAppNinjaDemo.Models;

namespace mvcAppNinjaDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Ninja>? Ninjas { get; set; }
        public DbSet<Team>? Teams { get; set; }
        public DbSet<Mission>? Missions { get; set; }
        public DbSet<Image>? Images { get; set; }
        public DbSet<Clan>? Clans { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}