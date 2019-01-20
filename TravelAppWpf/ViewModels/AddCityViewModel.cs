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
    class AddCityViewModel: ViewModelBase
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
        public ObservableCollection<(string cityFullName, string cityUrl)> FoundCities {
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

            Messenger.Default.Register<AddCityViewModelMessage>(this, m => {
                user = m.User;
                trip = m.Trip;
            });
        }

        #endregion


        #region Commands

        RelayCommand searchMatchesByNameCommand;
        public RelayCommand SearchMatchesByNameCommand
        {
            get => searchMatchesByNameCommand ?? (searchMatchesByNameCommand = new RelayCommand(() => {
                var searchMathesTask = Task.Run<IList<(string cityFullName, string cityUrl)>>(async () => await cityMatchesSearcherFromApi.GetMatchesFromApiByInputAsync(SearchInput));
                searchMathesTask.ContinueWith(t => FoundCities = new ObservableCollection<(string cityFullName, string cityUrl)>(t.Result), TaskScheduler.Current);
            }));
        }

        RelayCommand searchSingleCityByNameCommand;
        public RelayCommand SearchSingleCityByNameCommand
        {
            get => searchSingleCityByNameCommand ?? (searchSingleCityByNameCommand = new RelayCommand(() => {
                var cityFromApiGetterTask = Task.Run<City>(async() =>await cityFromApiGetter.GetCityFromApiByNameAsync(SearchInput));
                cityFromApiGetterTask.ContinueWith(t => FoundCity = t.Result, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);
                cityFromApiGetterTask.ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
            }));
        }

        RelayCommand addFoundCityCommand;
        public RelayCommand AddFoundCityCommand
        {
            get => addFoundCityCommand ?? (addFoundCityCommand = new RelayCommand(() => {
                var AddingCityInTripTask = Task.Run(() => cityService.AddCityAsync(trip, FoundCity));
                AddingCityInTripTask.ContinueWith(t => Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                AddingCityInTripTask.ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
                navigator.NavigateTo<CitiesViewModel>();
            }));
        }

        RelayCommand searchByNameAndAddCityCommand;
        public RelayCommand SearchByNameAndAddCityCommand
        {
            get => searchByNameAndAddCityCommand ?? (searchByNameAndAddCityCommand = new RelayCommand(() => {
                var searchCityByNameTask = Task.Run<City>(() => cityFromApiGetter.GetCityFromApiByName(SearchInput));
                searchCityByNameTask.ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
                var addFoundCityInTripTask = searchCityByNameTask.ContinueWith( t => cityService.AddCity(trip, t.Result),CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                addFoundCityInTripTask.ContinueWith(t => Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage),CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                addFoundCityInTripTask.ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);

                //Task.WhenAny(searchCityByNameTask, addFoundCityInTripTask).ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
                navigator.NavigateTo<CitiesViewModel>();
            }));
        }

        RelayCommand<(string cityFullName, string cityUrl)> addCityFromSelectedMatchCommand;
        public RelayCommand<(string cityFullName, string cityUrl)> AddCityFromSelectedMatchCommand
        {
            get => addCityFromSelectedMatchCommand ?? (addCityFromSelectedMatchCommand = new RelayCommand<(string cityFullName, string cityUrl)>(tuple => {
                var GettingCityBySelectedMatchTask = Task.Run<City>(async() =>await cityMatchesSearcherFromApi.GetCityFromApiBySelectedMatchAsync(tuple.cityUrl));
                var AddingCityInTripTask = GettingCityBySelectedMatchTask.ContinueWith(async t => await cityService.AddCityAsync(trip, t.Result),CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                
                AddingCityInTripTask.ContinueWith(t => Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage),CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                Task[] tasks = { GettingCityBySelectedMatchTask, AddingCityInTripTask };
                
                AddingCityInTripTask.ContinueWith(t => MessageBox.Show(t.Exception.InnerExceptions.First().Message), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
                navigator.NavigateTo<CitiesViewModel>();
            }));
        }

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand { get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<CitiesViewModel>())); }

        #endregion
    }
}
