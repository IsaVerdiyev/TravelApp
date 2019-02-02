using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppCore.Services;
using TravelAppCore.Specifications;
using TravelAppInfrastructure.Data;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            
            User user = new User
            {
                Email = "isa@gmail.com",
                NickName = "isa",
                Password = "12345",
                Trips = new List<Trip>
                {
                   new Trip{
                       Name="Trip1",
                       ArriavalDate = DateTime.Now.AddDays(3),
                       Destinations = new List<DestinationCityInTrip>(),
                       DepartureDate = DateTime.Now,
                        CheckList = new List<ToDoItem>
                        {
                            new ToDoItem{Name = "First item to do", Done = false},
                            new ToDoItem{Name = "Second item to do", Done = true}
                        },
                        Tickets = new List<Ticket>{
                            new Ticket{Name="dsfafd", ImagePath = "a;lkdsjf;ajds;f"},
                            new Ticket{Name = "sdfds", ImagePath = ";alskdjf;lajsd;fja;sdjf;kasjd" }
                        },


                   }
                }
            };

            List<City> Cities = new List<City>
                        {
                            new City{
                                Name = "Baku",
                                FullName = "Azerbaijan, Baku",
                                Currency = "AZN",
                                Language = "Azeri",
                                CityCoordinate  = new CityCoordinate
                                {
                                    Latitude = 234,
                                    Longitude = 123
                                }
                            },
                            new City
                            {
                                Name = "Qwebeck",
                                Currency  = "USD",
                                Language = "English",
                                FullName ="Canada, Qwebek",
                                CityCoordinate = new CityCoordinate
                                {
                                    Latitude = 123,
                                    Longitude = 532
                                }
                            },

                        };

            
            TripDb tripdb = new TripDb();
            tripdb.Set<User>().Add(user);
            IDestinationsInTripService destinationsInTripService = new DestinationsInTripService(new EfRepository<DestinationCityInTrip>(tripdb));
            ICityService cityService = new CityService(new EfRepository<City>(tripdb));
            cityService.AddCity(Cities.First());
            destinationsInTripService.AddDestinationInTrip(tripdb.Set<Trip>().First(), new DestinationCityInTrip { CityId = cityService.GetCityFromReposByFullname("Azerbaijan, Baku").Id });
            

            
        }
    }
}
