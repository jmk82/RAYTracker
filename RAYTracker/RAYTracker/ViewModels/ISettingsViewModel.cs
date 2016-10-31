using GalaSoft.MvvmLight.Command;

namespace RAYTracker.ViewModels
{
    public interface ISettingsViewModel
    {
        RelayCommand ChooseFileCommand { get; set; }
        RelayCommand SaveSettingsCommand { get; set; }
        string SessionXMLFilename { get; set; }
    }
}