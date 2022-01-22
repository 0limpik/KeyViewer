using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using KeyViewer.Model.Programs;
using KeyViewer.Services;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.ViewModel
{
    public class ProgramViewModel : AViewModel
    {
        public KeyViewerViewModel keyViewModel;

        public ICollectionView Settings => _Settings.View;
        private CollectionViewSource _Settings = new CollectionViewSource();

        [AlsoNotifyFor(nameof(IsNotNull), nameof(SelectedSettings))]
        public ProgramSettings SelectedSettings { get; set; }

        public bool IsNotNull => SelectedSettings != null;

        [AlsoNotifyFor(nameof(ProgramFilter))]
        public string ProgramFilter
        {
            get => _ProgramFilter;
            set
            {
                if (_ProgramFilter != value.ToLower())
                {
                    _ProgramFilter = value.ToLower();
                    Settings.Refresh();
                }
            }
        }
        private string _ProgramFilter;

        public bool ProgramFilterPredicate(object sender)
        {
            if (ProgramFilter is null)
                return true;

            if (sender is ProgramSettings settings)
                if (settings.DisplayName != null)
                    if (settings.DisplayName.ToLower().Contains(ProgramFilter))
                        return true;

            return false;
        }

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

        private ProgramsRepository programsRepository = ProgramsRepository.Instanse;

        public ProgramViewModel()
        {
            SelectAlightCommand = new LambdaCommand(SelectAlight, CanSelectAlightCommandExecute);

            SelectedSettings = programsRepository.Repository.FirstOrDefault();
            _Settings.Source = programsRepository.Repository;
            Settings.Filter = ProgramFilterPredicate;
            Settings.SortDescriptions.Add(new SortDescription(nameof(ProgramSettings.InGameAllTime), ListSortDirection.Descending));
            PropertyChanged += ProgramViewModel_PropertyChanged;
            programsRepository.Update += () => Settings.Refresh();
            Settings.Refresh();
        }

        private void ProgramViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            programsRepository?.Save();
        }

        public IEnumerable<DisplayNameSource> ReportTemplateValues
        {
            get { return Enum.GetValues(typeof(DisplayNameSource)).Cast<DisplayNameSource>(); }
        }
    }
}
