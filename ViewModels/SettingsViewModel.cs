using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RasSlider.Helpers;
using RasSlider.Services;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace RasSlider.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : ViewModelBase
    {

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        private MotorService motorService;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private string _countButtonText = "Start Counting...";

        public string CountButtonText
        {
            get { return _countButtonText; }
            set { Set(ref _countButtonText, value); }
        }


        private int _totalNumberOfSteps;
        public int TotalNumberOfSteps
        {
            get { return _totalNumberOfSteps; }
            set { Set(ref _totalNumberOfSteps, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        private ICommand _countTotalStepsCommand;

        public ICommand CountTotalStepsCommand
        {
            get
            {
                if (_countTotalStepsCommand == null)
                {
                    _countTotalStepsCommand = new RelayCommand<bool>(
                        async (param) =>
                        {
                            TotalNumberOfSteps = await motorService.CountSteps(param);
                            await motorService.SaveTotalSteps(TotalNumberOfSteps);
                        });
                }

                return _countTotalStepsCommand;
            }
        }

        private ICommand _stopCountingCommand;

        public ICommand StopCountingCommand
        {
            get
            {
                if (_stopCountingCommand == null)
                {
                    _stopCountingCommand = new RelayCommand<bool>(
                        (param) =>
                        {
                            motorService.StopCountingSteps = true;
                        });
                }

                return _countTotalStepsCommand;
            }
        }

        public SettingsViewModel()
        {
        }

        public void Initialize()
        {
            VersionDescription = GetVersionDescription();
            motorService = new MotorService();
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
