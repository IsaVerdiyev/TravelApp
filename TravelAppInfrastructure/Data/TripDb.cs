using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppInfrastructure.Data
{
    public class TripDb: DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityCoordinate> CityCoordinates { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }

        public TripDb(): base("name=TripDbConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasOptional(city => city.CityCoordinate).WithRequired(coordinate => coordinate.City).WillCascadeOnDelete(true);

            modelBuilder.Entity<User>().Property(u => u.NickName).IsRequired().HasMaxLength(30).IsUnicode(true);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(40).IsUnicode(false);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(40).IsUnicode(true);

            modelBuilder.Entity<Trip>().Property(t => t.ArriavalDate).IsRequired();
            modelBuilder.Entity<Trip>().Property(t => t.DepartureDate).IsRequired();
            modelBuilder.Entity<Trip>().Property(t => t.Name).IsRequired().HasMaxLength(30).IsUnicode(true);

            modelBuilder.Entity<City>().Property(c => c.Currency).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<City>().Property(c => c.FullName).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<City>().Property(c => c.Language).HasMaxLength(50);
            modelBuilder.Entity<City>().Property(c => c.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<City>().Property(c => c.PictureUrl).HasMaxLength(500);
            

            modelBuilder.Entity<CityCoordinate>().Property(cor => cor.Latitude).IsRequired();
            modelBuilder.Entity<CityCoordinate>().Property(cor => cor.Longitude).IsRequired();

            modelBuilder.Entity<Ticket>().Property(tick => tick.ImagePath).HasMaxLength(500);

            modelBuilder.Entity<ToDoItem>().Property(td => td.Done).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(td => td.Name).IsRequired().HasMaxLength(70);

            base.OnModelCreating(modelBuilder);
        }
    }
}
