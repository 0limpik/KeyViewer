using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using KeyViewer.Model.Programs;
using KeyViewer.Services;
using KeyViewer.ViewModel.Settings;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.ViewModel
{
    public class SettingsViewModel : AViewModel
    {
        public KeyViewerViewModel keyViewModel;

        public ICollectionView Settings => _Settings.View;
        private CollectionViewSource _Settings = new CollectionViewSource();

        [AlsoNotifyFor(nameof(SelectedSettings))]
        public ProgramSettings SelectedSettings { get => SelectedSettingsModel.SelectedSettings; set => SelectedSettingsModel.SelectedSettings = value; }

        public ProgramSettingsViewModel SelectedSettingsModel { get; set; }

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

        public ICommand SelectDefautCommand { get; }

        private void SelectDefaut(object parameter)
        {
            //For deselect
            SelectedSettings = null;
            SelectedSettings = programsRepository.Default;
        }

        private bool CanSelectDefaultCommandExecute(object parameter)
        {
            return SelectedSettings != programsRepository.Default;
        }

        public ICommand DeleteSettingsCommand { get; }

        private void DeleteSettings(object parameter)
        {
            if (parameter is ProgramSettings settings)
            {
                programsRepository.Repository.Remove(settings);
                SelectedSettings = null;
            }
        }

        private bool CanDeleteSettingsCommandExecute(object parameter) => true;

        public ICommand DeletePermanentlySettingsCommand { get; }

        private void DeletePermanentlySettings(object parameter)
        {
            if (parameter is ProgramSettings settings)
            {
                programsRepository.Repository.Remove(settings);
                programsRepository.Save();
                deletedProgramsRepository.Repository.Add(settings.Window.FilePath);
                deletedProgramsRepository.Save();
                SelectedSettings = null;
            }
        }

        private bool CanDeletePermanentlySettingsCommandExecute(object parameter) => true;

        private ProgramsRepository programsRepository = ProgramsRepository.Instanse;
        private DeletedProgramsRepository deletedProgramsRepository = DeletedProgramsRepository.Instanse;

        public SettingsViewModel()
        {
            SelectDefautCommand = new LambdaCommand(SelectDefaut, CanSelectDefaultCommandExecute);
            DeleteSettingsCommand = new LambdaCommand(DeleteSettings, CanDeleteSettingsCommandExecute);
            DeletePermanentlySettingsCommand = new LambdaCommand(DeletePermanentlySettings, CanDeletePermanentlySettingsCommandExecute);

            SelectedSettingsModel = new ProgramSettingsViewModel
            {
                SelectedSettings = programsRepository.Repository.FirstOrDefault()
            };

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
    }
}
