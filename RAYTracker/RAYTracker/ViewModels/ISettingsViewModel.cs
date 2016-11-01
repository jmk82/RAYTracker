using GalaSoft.MvvmLight.Command;

namespace RAYTracker.ViewModels
{
    public interface ISettingsViewModel
    {
        RelayCommand ChooseSessionFileCommand { get; set; }
        RelayCommand SaveSettingsCommand { get; set; }
        string SessionXMLFilename { get; set; }
    }
}