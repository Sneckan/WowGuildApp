using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Models;

namespace WowGuildApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Kontroll om databasen finns. OBS Vid större ändringar deleta alltid databasen i Solution explorer!
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //test data
            //builder.Entity<User>().HasData(TestData.userData);


        }

        public DbSet<WowGuildApp.Models.Event> Events { get; set; }
        public DbSet<WowGuildApp.Models.Signup> Signups { get; set; }
        public DbSet<WowGuildApp.Models.Lineup> Lineups { get; set; }
    }
}
