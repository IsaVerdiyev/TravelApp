using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Services;
using TravelAppInfrastructure.Data;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            TripDb tripDb = new TripDb();

            //tripDb.Database.Log = s => Debug.WriteLine(s);
            //User user = new User
            //{
            //    Email = "v.isa1ail.com",
            //    NickName = "Isa",
            //    Password = "12345",
            //    Trips = new List<Trip>
            //    {
            //       new Trip{
            //           ArriavalDate = DateTime.Now.AddDays(3),
            //           DepartureDate = DateTime.Now,
            //            CheckList = new List<ToDoItem>
            //            {
            //                new ToDoItem{Name = "First item to do", Done = false}
            //            },
            //            Cities = new List<City>
            //            {
            //                new City{
            //                    Name = "Baku",
            //                    Currency = "AZN",
            //                    Language = "Azeri",
            //                    CityCoordinate  = new CityCoordinate
            //                    {
            //                        Latitude = 234,
            //                        Longitude = 123
            //                    }
            //                }
            //            }

            //       }
            //    }
            //};
            //tripDb.Users.Add(user);
            //tripDb.SaveChanges();

            //Trip trip = new Trip
            //{
            //    UserId = 1,
            //    ArriavalDate = DateTime.Now.AddDays(3),
            //    DepartureDate = DateTime.Now,
            //    CheckList = new List<ToDoItem>
            //            {
            //                new ToDoItem{Name = "First item to do", Done = false}
            //            },
            //    Cities = new List<City>
            //            {
            //                new City{
            //                    Name = "Baku",
            //                    Currency = "AZN",
            //                    Language = "Azeri",
            //                    CityCoordinate  = new CityCoordinate
            //                    {
            //                        Latitude = 234,
            //                        Longitude = 123
            //                    }
            //                }
            //            }

            //};
            //tripDb.Trips.Add(trip);

            ITripService tripService = new TripService(new EfRepository<City>(tripDb), new EfRepository<Trip>(tripDb));
            var trips = tripService.GetTripsOfUser(tripDb.Users.FirstOrDefault());
            foreach(var trip in trips)
            {
                Console.WriteLine(trip.ArriavalDate);
            }
            //IUserService userService = new UserService(new EfRepository<Trip>(tripDb));
            //userService.AddTrip();
            
        }
    }
}
