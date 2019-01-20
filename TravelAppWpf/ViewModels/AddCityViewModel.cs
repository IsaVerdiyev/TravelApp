using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelAppCore.Entities;
using TravelAppCore.Interfaces;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class AddCityViewModel : ViewModelBase
    {
        #region Fields And Properties

        Trip trip;
        User user;


        string searchInput;
        public string SearchInput
        {
            get => searchInput;
            set => Set(ref searchInput, value);
        }


        ObservableCollection<(string cityFullName, string cityUrl)> foundCities = new ObservableCollection<(string cityFullName, string cityUrl)>();
        public ObservableCollection<(string cityFullName, string cityUrl)> FoundCities
        {
            get => foundCities;
            set => Set(ref foundCities, value);
        }

        City foundCity;
        public City FoundCity { get => foundCity; set => Set(ref foundCity, value); }

        #endregion

        #region Messages

        UpdateCitiesMessage updateCitiesMessage = new UpdateCitiesMessage();

        #endregion

        #region Dependencies

        INavigator navigator;
        ICityFromApiGetter<string> cityFromApiGetter;
        ICityMatchesSearcherFromApi<string, IList<(string cityFullName, string cityUrl)>, string> cityMatchesSearcherFromApi;
        private readonly ICityService cityService;

        #endregion

        #region Consturctors

        public AddCityViewModel(INavigator navigator, ICityFromApiGetter<string> cityFromApiGetter, ICityMatchesSearcherFromApi<string, IList<(string cityFullName, string cityUrl)>, string> cityMatchesSearcherFromApi, ICityService cityService)
        {
            this.navigator = navigator;
            this.cityFromApiGetter = cityFromApiGetter;
            this.cityMatchesSearcherFromApi = cityMatchesSearcherFromApi;
            this.cityService = cityService;

            Messenger.Default.Register<AddCityViewModelMessage>(this, m =>
            {
                user = m.User;
                trip = m.Trip;
            });
        }

        #endregion


        #region Commands

        RelayCommand searchMatchesByNameCommand;
        public RelayCommand SearchMatchesByNameCommand
        {
            get => searchMatchesByNameCommand ?? (searchMatchesByNameCommand = new RelayCommand(async () =>
            {
                await Task.Run(async () =>
                {
                    var searchMatches = await cityMatchesSearcherFromApi.GetMatchesFromApiByInputAsync(SearchInput);
                    FoundCities = new ObservableCollection<(string cityFullName, string cityUrl)>(searchMatches);
                });
            }));
        }

        RelayCommand searchSingleCityByNameCommand;
        public RelayCommand SearchSingleCityByNameCommand
        {
            get => searchSingleCityByNameCommand ?? (searchSingleCityByNameCommand = new RelayCommand(async () =>
            {
                await Task.Run(async () =>
                {
                    FoundCity = await cityFromApiGetter.GetCityFromApiByNameAsync(SearchInput);
                });
            }));
        }

        RelayCommand addFoundCityCommand;
        public RelayCommand AddFoundCityCommand
        {
            get => addFoundCityCommand ?? (addFoundCityCommand = new RelayCommand(async () =>
            {
                await Task.Run(async () =>
                {
                    navigator.NavigateTo<CitiesViewModel>();
                    await cityService.AddCityAsync(trip, FoundCity);
                    Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage);
                });
            }));
        }

        RelayCommand searchByNameAndAddCityCommand;
        public RelayCommand SearchByNameAndAddCityCommand
        {
            get => searchByNameAndAddCityCommand ?? (searchByNameAndAddCityCommand = new RelayCommand(async () =>
            {
                await Task.Run(async () =>
                {
                    navigator.NavigateTo<CitiesViewModel>();
                    City city = await cityFromApiGetter.GetCityFromApiByNameAsync(SearchInput);
                    await cityService.AddCityAsync(trip, city);
                    Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage);
                });
            }));
        }

        RelayCommand<(string cityFullName, string cityUrl)> addCityFromSelectedMatchCommand;
        public RelayCommand<(string cityFullName, string cityUrl)> AddCityFromSelectedMatchCommand
        {
            get => addCityFromSelectedMatchCommand ?? (addCityFromSelectedMatchCommand = new RelayCommand<(string cityFullName, string cityUrl)>(async tuple =>
            {
                await Task.Run(async () =>
                {
                    navigator.NavigateTo<CitiesViewModel>();
                    City city = await cityMatchesSearcherFromApi.GetCityFromApiBySelectedMatchAsync(tuple.cityUrl);
                    await cityService.AddCityAsync(trip, city);
                    Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage);
                });
            }));
        }

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand { get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<CitiesViewModel>())); }

        #endregion
    }
}
