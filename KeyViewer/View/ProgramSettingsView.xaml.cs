using System;
using System.Windows;
using KeyViewer.ViewModel;

namespace KeyViewer.View
{
    /// <summary>
    /// Interaction logic for ProgramSettings.xaml
    /// </summary>
    public partial class ProgramSettingsView : Window
    {
        private ProgramViewModel ViewModel => (ProgramViewModel)this.DataContext;

        public ProgramSettingsView()
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
