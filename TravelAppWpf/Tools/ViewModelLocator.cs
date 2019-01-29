using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppCore.Interfaces;
using TravelAppInfrastructure.Data;
using TravelAppWpf.Navigation;
using TravelAppWpf.ViewModels;

namespace TravelAppWpf.Tools
{
    class ViewModelLocator
    {
        INavigator navigator;

        public AppViewModel AppViewModel { get; }

        public AddTicketViewModel AddTicketViewModel { get; private set; }

        public ViewModelLocator()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");
            var module = new ConfigurationModule(config.Build());
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(module);
            containerBuilder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            var container = containerBuilder.Build();

            AppViewModel = container.Resolve<AppViewModel>();
            navigator = container.Resolve<INavigator>();
            navigator.Register(AppViewModel);
            navigator.Register(container.Resolve<SignInViewModel>());
            navigator.Register(container.Resolve<TripsViewModel>());
            navigator.Register(container.Resolve<CitiesViewModel>());
            navigator.Register(container.Resolve<TicketsViewModel>());
            navigator.Register(container.Resolve<CheckListViewModel>());
            navigator.Register(container.Resolve<RegisterViewModel>());
            navigator.Register(container.Resolve<AddTripViewModel>());
            navigator.Register(container.Resolve<AddCityViewModel>());
            navigator.Register(container.Resolve<CityOnMapViewModel>());
            navigator.Register(container.Resolve<AddTicketViewModel>());
            navigator.Register(container.Resolve<AddItemInCheckListViewModel>());


            navigator.NavigateTo<SignInViewModel>();

        }
    }
}
