using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppWpf.Navigation;
using TravelAppWpf.ViewModels;

namespace TravelAppWpf.Tools
{
    class ViewModelLocator
    {
        INavigator navigator = new Navigator();

        public AppViewModel AppViewModel { get; }

        public ViewModelLocator()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");
            var module = new ConfigurationModule(config.Build());
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(module);
            containerBuilder.RegisterInstance(navigator).As<INavigator>().SingleInstance();

            var container = containerBuilder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                AppViewModel = scope.Resolve<AppViewModel>();
                navigator.Register(AppViewModel);
                navigator.Register(scope.Resolve<SignInViewModel>());
                navigator.Register(scope.Resolve<TripsViewModel>());
                navigator.Register(scope.Resolve<CitiesViewModel>());
                navigator.Register(scope.Resolve<TicketsViewModel>());
                navigator.Register(scope.Resolve<CheckListViewModel>());
            }

            navigator.NavigateTo<SignInViewModel>();

        }
    }
}
