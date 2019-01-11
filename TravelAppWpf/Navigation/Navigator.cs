using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace TravelAppWpf.Navigation
{
    class Navigator : INavigator
    {
        string exceptionFirstPart = "Page";
        string exceptionLastPart = "not found. Error occured in NavigateTo function.";
        private Dictionary<Type, ViewModelBase> viewmodels = new Dictionary<Type, ViewModelBase>();

        public void Register(ViewModelBase viewModel)
        {
            viewmodels[viewModel.GetType()] = viewModel;
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            WrapSwitchingViewModelInExceptionChecker(() => Messenger.Default.Send(viewmodels[typeof(T)]), $"{exceptionFirstPart} {typeof(T).ToString()} {exceptionLastPart}");
        }

        public void NavigateTo(Type t)
        {
            WrapSwitchingViewModelInExceptionChecker(() => Messenger.Default.Send(viewmodels[t]), $"{exceptionFirstPart} {t} {exceptionLastPart}");
        }

        private void WrapSwitchingViewModelInExceptionChecker(Action action, string exceptionMessage)
        {
            try
            {
                action.Invoke();
            }
            catch (KeyNotFoundException ex)
            {
                throw new PageNotFoundException(exceptionMessage, ex);
            }
        }

    }


}
