using System;
using System.Windows;
using KeyViewer.ViewModel;

namespace KeyViewer.View
{
    /// <summary>
    /// Interaction logic for ProgramSettings.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private SettingsViewModel ViewModel => (SettingsViewModel)this.DataContext;

        public SettingsView()
        {
            InitializeComponent();
        }

        public void Init(KeyViewerViewModel viewModel)
        {
            ViewModel.keyViewModel = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("TEst");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
