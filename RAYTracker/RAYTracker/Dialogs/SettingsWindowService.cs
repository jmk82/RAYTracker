﻿using GalaSoft.MvvmLight.Messaging;
using RAYTracker.ViewModels;
using RAYTracker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RAYTracker.Dialogs
{
    public class SettingsWindowService
    {
        private SettingsWindow _settingsWindow;

        public void showWindow(ISettingsViewModel viewModel)
        {
            _settingsWindow = new SettingsWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = viewModel
            };

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "CloseSettingsWindow") CloseWindow();
            });

            _settingsWindow.ShowDialog();
        }

        private void CloseWindow()
        {
            _settingsWindow.Close();
        }
    }
}