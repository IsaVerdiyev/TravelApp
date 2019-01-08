using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;

namespace TravelAppInfrastructure.Data
{
    class TripDb: DbContext
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
    }
}
