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
using System.Windows.Shapes;
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for InProgressView.xaml
    /// </summary>
    public partial class InProgressView : Window
    {
        private InProgressViewModel viewModel;

        public InProgressView(UserModel u, List<TaskModel> tasksInProgress)
        {
            
            this.viewModel = new InProgressViewModel(u, tasksInProgress);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SortTasks();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            viewModel.FilterTasks();
        }
    }
}
