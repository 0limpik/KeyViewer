using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using KeyViewer.Model.Programs;
using KeyViewer.Services;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;
using Point = KeyViewer.Model.Programs.Point;

namespace KeyViewer.ViewModel
{
    public class ProgramViewModel : AViewModel
    {
        public KeyViewModel keyViewModel;

        public ICollectionView Settings => _Settings.View;
        private CollectionViewSource _Settings = new CollectionViewSource();

        [AlsoNotifyFor(nameof(SelectedSettings))]
        public ProgramSettings SelectedSettings { get; set; }

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

        public string DisplayName => SelectedSettings?.DisplayName;
        [AlsoNotifyFor(nameof(DisplayNameSource), nameof(DisplayName))] public DisplayNameSource DisplayNameSource { get => SelectedSettings.DisplayNameSource; set => SelectedSettings.DisplayNameSource = value; }
        public IEnumerable<DisplayNameSource> ReportTemplateValues
        {
            get { return Enum.GetValues(typeof(DisplayNameSource)).Cast<DisplayNameSource>(); }
        }

        [AlsoNotifyFor(nameof(Alighument))] public Alighument Alighument { get => SelectedSettings.Alight; set => SelectedSettings.Alight = value; }
        public ICommand SelectAlightCommand { get; }

        private void SelectAlight(object parameter)
        {
            if (parameter is Alighument alight)
            {
                Alighument = alight;
            }
        }

        private bool CanSelectAlightCommandExecute(object parameter)
        {
            if (parameter is Alighument alight)
            {
                return Alighument != alight;
            }
            return false;
        }

        [AlsoNotifyFor(nameof(AlightHorizontal))] public int AlightHorizontal { get => (int)SelectedSettings.AlightPosition.X; set => SelectedSettings.AlightPosition.X = value; }
        [AlsoNotifyFor(nameof(AlightVertical))] public int AlightVertical { get => (int)SelectedSettings.AlightPosition.Y; set => SelectedSettings.AlightPosition.Y = value; }

        [AlsoNotifyFor(nameof(MaxAlightHorizontal))] public int MaxAlightHorizontal => (int)(SettingsClass.ScreenSizeX - SettingsClass.SizeX);
        [AlsoNotifyFor(nameof(MaxAlightVertical))] public int MaxAlightVertical => (int)(SettingsClass.ScreenSizeY - SettingsClass.SizeY);

        [AlsoNotifyFor(nameof(IsVisible))] public bool IsVisible { get => SelectedSettings.IsVisible; set => SelectedSettings.IsVisible = value; }

        private ProgramsRepository programsRepository = ProgramsRepository.Instanse;

        public ProgramViewModel()
        {
            SelectAlightCommand = new LambdaCommand(SelectAlight, CanSelectAlightCommandExecute);

            SelectedSettings = programsRepository.Repository.FirstOrDefault();
            _Settings.Source = programsRepository.Repository;
            Settings.Filter = ProgramFilterPredicate;
            Settings.SortDescriptions.Add(new SortDescription(nameof(ProgramSettings.InGameAllTime), ListSortDirection.Descending));
            PropertyChanged += ProgramViewModel_PropertyChanged;
            Settings.Refresh();
        }

        private void ProgramViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            programsRepository?.Save();
        }
    }
}
