using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyViewer.ViewModel;
using KeyViewer.ViewModel.Keys;

namespace KeyViewer.View.Keys
{
    /// <summary>
    /// Interaction logic for ButtonsList.xaml
    /// </summary>
    public partial class KeyListView : UserControl
    {
        public KeyListView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.DataContext is KeyListViewModel viewModel)
            {
                viewModel.AddKeyAction = (k) => KeyContainer.Items.Add(k);
                viewModel.RemoveKeyAction = (k) => KeyContainer.Items.Remove(k);
            }
        }
    }
}
