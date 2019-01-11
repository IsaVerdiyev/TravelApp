using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppWpf.Navigation;

namespace TravelAppWpf.ViewModels
{
    class AppViewModel : ViewModelBase
    {
        #region Fields and Properties

        ViewModelBase currentPage;
        public ViewModelBase CurrentPage { get => currentPage; set => Set(ref currentPage, value); }

        #endregion


        #region Dependencies

        INavigator navigator;

        #endregion


        #region Constructors

        public AppViewModel(INavigator navigator)
        {
            this.navigator = navigator;
            Messenger.Default.Register<ViewModelBase>(this, vm => CurrentPage = vm);
        }

        #endregion
    }
}
