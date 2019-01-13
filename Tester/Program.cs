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
            //                    CityCoordinate = new CityCoordinate
            //                    {
            //                        Latitude = 123,
            //                        Longitude = 532
            //                    }
            //                }

            //            }

            //       }
            //    }
            //};

            IRepository<User> userRepository = new EfRepository<User>(tripDb);

            IAccountService accountService = new AccountService(userRepository);

            //userRepository.Add(user);

            accountService.DeleteAccount(new DeleteByIdSpecification<User>(8));

            //User user = userRepository.GetSingleBySpec(new CustomSpecification<User>(null));

            //accountService.DeleteAccount(user);





            //EfRepository<User> repository = new EfRepository<User>(tripDb);

            //repository.DeleteSingleBySpec(new CustomSpecification<User>(user => ));

            //var items = repository.List(new CustomSpecification<Ticket>(null));

            //foreach(var item in items)
            //{
            //    repository.Delete(item);
            //}



            //accountService.TrySignUp(user);

            ////Trip trip = new Trip
            ////{
            ////    UserId = 1,
            ////    ArriavalDate = DateTime.Now.AddDays(3),
            ////    DepartureDate = DateTime.Now,
            ////    CheckList = new List<ToDoItem>
            ////            {
            ////                new ToDoItem{Name = "First item to do", Done = false}
            ////            },
            ////    Cities = new List<City>
            ////            {
            ////                new City{
            ////                    Name = "Baku",
            ////                    Currency = "AZN",
            ////                    Language = "Azeri",
            ////                    CityCoordinate  = new CityCoordinate
            ////                    {
            ////                        Latitude = 234,
            ////                        Longitude = 123
            ////                    }
            ////                }
            ////            }

            ////};
            ////tripDb.Trips.Add(trip);

            //ITripService tripService = new TripService(new EfRepository<City>(tripDb), new EfRepository<Trip>(tripDb));
            //var trips = tripService.GetTripsOfUser(tripDb.Users.FirstOrDefault());
            //foreach(var trip in trips)
            //{
            //    Console.WriteLine(trip.ArriavalDate);
            //}
            ////IUserService userService = new UserService(new EfRepository<Trip>(tripDb));
            ////userService.AddTrip();




        }
    }
}
