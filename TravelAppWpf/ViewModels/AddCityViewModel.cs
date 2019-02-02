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
using TravelAppWpf.Extensions;
using TravelAppWpf.Messages;
using TravelAppWpf.Navigation;
using TravelAppWpf.Services.ProcessesInfo;

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

        (string cityFullName, string cityUrl) foundCity;
        public (string cityFullName, string cityUrl) FoundCity { get => foundCity; set => Set(ref foundCity, value); }


        private string currentProcessesInfo;
        public string CurrentProcessesInfo
        {
            get { return currentProcessesInfo; }
            set
            {
                Set(ref currentProcessesInfo, value);

            }
        }
        #endregion

        #region Messages

        UpdateCitiesMessage updateCitiesMessage = new UpdateCitiesMessage();
        UpdateProcessInfoMessage updateProcessInfoMessage = new UpdateProcessInfoMessage();

        #endregion

        #region Dependencies

        INavigator navigator;
        private readonly IProcessesInfoService processesInfoService;
        ICityFromApiGetter<string> cityFromApiGetter;
        ICityMatchesSearcherFromApi<string, IList<(string cityFullName, string cityUrl)>, string> cityMatchesSearcherFromApi;
        private readonly ICityService cityService;
        private readonly IDestinationsInTripService destinationsInTripService;

        #endregion

        #region Constructors

        public AddCityViewModel(INavigator navigator, IProcessesInfoService processesInfoService, ICityFromApiGetter<string> cityFromApiGetter, ICityMatchesSearcherFromApi<string, IList<(string cityFullName, string cityUrl)>, string> cityMatchesSearcherFromApi, ICityService cityService, IDestinationsInTripService destinationsInTripService)
        {
            this.navigator = navigator;
            this.processesInfoService = processesInfoService;
            this.cityFromApiGetter = cityFromApiGetter;
            this.cityMatchesSearcherFromApi = cityMatchesSearcherFromApi;
            this.cityService = cityService;
            this.destinationsInTripService = destinationsInTripService;
            Messenger.Default.Register<AddCityViewModelMessage>(this, m =>
            {
                user = m.User;
                trip = m.Trip;
            });

            Messenger.Default.Register<UpdateProcessInfoMessage>(this, m => UpdateCurrentProcessesInfo());
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
                    FoundCity = (await cityMatchesSearcherFromApi.GetMatchesFromApiByInputAsync(SearchInput)).First();
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
                    City addedCity = await cityService.GetCityFromReposByFullnameAsync(FoundCity.cityFullName);
                    if(addedCity == null)
                    {
                        addedCity = await cityMatchesSearcherFromApi.GetCityFromApiBySelectedMatchAsync(FoundCity.cityUrl);
                        addedCity = await cityService.AddCityAsync(addedCity);
                    }
                    await destinationsInTripService.AddDestinationInTripAsync(trip, new DestinationCityInTrip {
                        CityId = addedCity.Id, 
                        TripId = trip.Id
                    });
                    Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage);
                });
            }));
        }

        RelayCommand searchByNameAndAddCityCommand;
        public RelayCommand SearchByNameAndAddCityCommand
        {
            get => searchByNameAndAddCityCommand ?? (searchByNameAndAddCityCommand = new RelayCommand(async () =>
            {
                int processId = processesInfoService.GenerateUniqueId();
                processesInfoService.ActivateProcess(ProcessEnum.AddingCity, processesInfoService.ProcessNames[ProcessEnum.AddingCity], processId);
                try
                {
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                    await Task.Run(async () =>
                    {
                        FoundCity = (await cityMatchesSearcherFromApi.GetMatchesFromApiByInputAsync(SearchInput)).First();
                        navigator.NavigateTo<CitiesViewModel>();
                        City addedCity = await cityService.GetCityFromReposByFullnameAsync(FoundCity.cityFullName);
                        if (addedCity == null)
                        {
                            addedCity = await cityMatchesSearcherFromApi.GetCityFromApiBySelectedMatchAsync(FoundCity.cityUrl);
                            addedCity = await cityService.AddCityAsync(addedCity);
                        }
                        await destinationsInTripService.AddDestinationInTripAsync(trip, new DestinationCityInTrip
                        {
                            CityId = addedCity.Id,
                            TripId = trip.Id
                        });
                        Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage);
                    });
                }
                finally
                {
                    processesInfoService.DeactivateProcess(ProcessEnum.AddingCity, processId);
                    Messenger.Default.Send<UpdateProcessInfoMessage>(updateProcessInfoMessage);
                }
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
                    City addedCity = await cityService.GetCityFromReposByFullnameAsync(tuple.cityFullName);
                    if (addedCity == null)
                    {
                        addedCity = await cityMatchesSearcherFromApi.GetCityFromApiBySelectedMatchAsync(tuple.cityUrl);
                        addedCity = await cityService.AddCityAsync(addedCity);
                    }
                    await destinationsInTripService.AddDestinationInTripAsync(trip, new DestinationCityInTrip
                    {
                        CityId = addedCity.Id,
                        TripId = trip.Id
                    });
                    Messenger.Default.Send<UpdateCitiesMessage>(updateCitiesMessage);
                });
            }));
        }

        RelayCommand returnBackCommand;
        public RelayCommand ReturnBackCommand { get => returnBackCommand ?? (returnBackCommand = new RelayCommand(() => navigator.NavigateTo<CitiesViewModel>())); }

        #endregion

        #region Private Functions

        void UpdateCurrentProcessesInfo()
        {
            try
            {
                CurrentProcessesInfo = processesInfoService.GetOneInfoStringFromAllProcesses();
            }
            catch (InvalidOperationException ex)
            {
                CurrentProcessesInfo = "";
            }
        }

        #endregion
    }
}
