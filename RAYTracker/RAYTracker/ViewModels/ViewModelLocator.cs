using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RAYTracker.Dialogs;
using RAYTracker.Domain.Repository;
using RAYTracker.Domain.Utils;

namespace RAYTracker.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CashGameViewModel>();
            SimpleIoc.Default.Register<ICashGameService, CashGameService>();
            SimpleIoc.Default.Register<IOpenFileDialogService, OpenFileDialogService>();
            SimpleIoc.Default.Register<IWaitDialogService, WaitDialogService>();
            SimpleIoc.Default.Register<ISessionRepository, SessionRepository>();
            
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public static ViewModelLocator Instance
        {
            get
            {
                return Application.Current.Resources["Locator"] as ViewModelLocator;
            }
        }
    }
}
