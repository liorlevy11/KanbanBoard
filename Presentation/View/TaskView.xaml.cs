using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for WindowTask.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        
        private TaskViewModel viewModel;

        public TaskView(ColumnModel c)
        {
            this.viewModel = new TaskViewModel(c);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddTask();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
