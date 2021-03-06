﻿using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RAYTracker.Dialogs;
using RAYTracker.Domain.Repository;
using RAYTracker.Domain.Utils;
using System.Windows;

namespace RAYTracker.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CashGameViewModel>();
            SimpleIoc.Default.Register<TournamentViewModel>();
            SimpleIoc.Default.Register<FilterViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
            SimpleIoc.Default.Register<StatsViewModel>();
            SimpleIoc.Default.Register<InfoDialogViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();

            SimpleIoc.Default.Register<ICashGameService, CashGameService>();
            SimpleIoc.Default.Register<ITournamentService, TournamentService>();

            SimpleIoc.Default.Register<IOpenFileDialogService, OpenFileDialogService>();
            SimpleIoc.Default.Register<ISaveFileDialogService, SaveFileDialogService>();
            SimpleIoc.Default.Register<IWaitDialogService, WaitDialogService>();
            SimpleIoc.Default.Register<IFilterWindowService, FilterWindowService>();
            SimpleIoc.Default.Register<IInfoDialogService, InfoDialogService>();
            SimpleIoc.Default.Register<ISettingsWindowService, SettingsWindowService>();

            SimpleIoc.Default.Register<ISessionRepository, XmlSessionRepository>();
            SimpleIoc.Default.Register<ITournamentRepository, XmlTournamentRepository>();

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
