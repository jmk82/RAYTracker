﻿using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.ViewModels;
using RAYTracker.Views;

namespace RAYTracker.Dialogs
{
    public class FilterWindowService : IFilterWindowService
    {
        private FilterWindow _filterWindow;

        public void ShowWindow(FilterViewModel viewModel)
        {
            _filterWindow = new FilterWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = viewModel
            };

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "CloseFilterWindow") CloseWindow();
            });

            _filterWindow.ShowDialog();
        }

        public void CloseWindow()
        {
            _filterWindow.Close();
        }
    }
}
