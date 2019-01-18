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
            TripDb tripDb = new TripDb();

            //tripDb.Database.Log = s => Debug.WriteLine(s);
            //User user = new User
            //{
            //    Email = "V.isa1ail.",
            //    NickName = "vagif",
            //    Password = "12345",
            //    Trips = new List<Trip>
            //    {
            //       new Trip{
            //           Name="Trip1",
            //           ArriavalDate = DateTime.Now.AddDays(3),
            //           DepartureDate = DateTime.Now,
            //            CheckList = new List<ToDoItem>
            //            {
            //                new ToDoItem{Name = "First item to do", Done = false},
            //                new ToDoItem{Name = "Second item to do", Done = true}
            //            },
            //            Tickets = new List<Ticket>{
            //                new Ticket{ImagePath = "a;lkdsjf;ajds;f"},
            //                new Ticket{ImagePath = ";alskdjf;lajsd;fja;sdjf;kasjd" }
            //            },
            //            Cities = new List<City>
            //            {
            //                new City{
            //                    Name = "Baku",
            //                    FullName = "Azerbaijan, Baku",
            //                    Currency = "AZN",
            //                    Language = "Azeri",
            //                    CityCoordinate  = new CityCoordinate
            //                    {
            //                        Latitude = 234,
            //                        Longitude = 123
            //                    }
            //                },
            //                new City
            //                {
            //                    Name = "Qwebeck",
            //                    Currency  = "USD",
            //                    Language = "English",
            //                    FullName ="Canada, Qwebek",
            //                    CityCoordinate = new CityCoordinate
            //                    {
            //                        Latitude = 123,
            //                        Longitude = 532
            //                    }
            //                },

            //            }

            //       }
            //    }
            //};



            ITicketService ticketService = new TicketService(new EfRepository<Ticket>(tripDb));
            ICityService cityService = new CityService(new EfRepository<City>(tripDb));
            ICheckListService checkListService = new CheckListService(new EfRepository<ToDoItem>(tripDb));

            Trip trip = new Trip { Id = 6 };
            var list = ticketService.GetTicketsOfTripAsync(trip).Result;

            foreach(var item in list)
            {
                Console.WriteLine(item.ImagePath);
            }


            Console.ReadKey();
        }
    }
}
