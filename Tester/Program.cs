using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppInfrastructure.Data;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            TripDb tripDb = new TripDb();
            User user = new User
            {
                Email = "verdiyev.isa1992@gmail.com",
                NickName = "Isa",
                Password = "12345",
                Trips = new List<Trip>
                {
                   new Trip{
                       ArriavalDate = DateTime.Now.AddDays(3),
                       DepartureDate = DateTime.Now,
                        CheckList = new List<ToDoItem>
                        {
                            new ToDoItem{Name = "First item to do", Done = false}
                        },
                        Cities = new List<City>
                        {
                            new City{
                                Name = "Baku",
                                Currency = "AZN",
                                Language = "Azeri",
                                CityCoordinate  = new CityCoordinate
                                {
                                    Latitude = 234,
                                    Longitude = 123
                                }
                            }
                        }

                   }
                }
            };
            tripDb.Users.Add(user);

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
            tripDb.SaveChanges();
        }
    }
}
