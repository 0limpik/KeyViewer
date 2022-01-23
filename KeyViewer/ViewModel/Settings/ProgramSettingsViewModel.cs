using System.Collections.Generic;
using System;
using System.Windows.Input;
using KeyViewer.Model.Programs;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;
using System.Linq;

namespace KeyViewer.ViewModel.Settings
{
    public class ProgramSettingsViewModel : AViewModel
    {

        [AlsoNotifyFor(nameof(IsNotNull), nameof(SelectedSettings))]
        public ProgramSettings SelectedSettings { get; set; }

        public bool IsNotNull => SelectedSettings != null;

        [AlsoNotifyFor(nameof(DisplayNameSource), nameof(SelectedSettings))]
        public DisplayNameSource? DisplayNameSource { get => SelectedSettings?.DisplayNameSource; set => SelectedSettings.DisplayNameSource = (DisplayNameSource)value; }

        public ICommand SelectAlightCommand { get; }

        private void SelectAlight(object parameter)
        {
            if (parameter is Alighument alight)
            {
                if (SelectedSettings != null) SelectedSettings.Alight = alight;
            }
        }

        private bool CanSelectAlightCommandExecute(object parameter)
        {
            if (parameter is Alighument alight)
            {
                if (SelectedSettings != null) return SelectedSettings.Alight != alight;
            }
            return false;
        }

        public int MaxAlightHorizontal => (int)(SettingsClass.ScreenSizeX - SettingsClass.SizeX);
        public int MaxAlightVertical => (int)(SettingsClass.ScreenSizeY - SettingsClass.SizeY);

        public ProgramSettingsViewModel()
        {
            SelectAlightCommand = new LambdaCommand(SelectAlight, CanSelectAlightCommandExecute);
        }

        public IEnumerable<DisplayNameSource> ReportTemplateValues
        {
            get { return Enum.GetValues(typeof(DisplayNameSource)).Cast<DisplayNameSource>(); }
        }
    }
}
