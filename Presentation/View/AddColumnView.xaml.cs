using Presentation.ViewModel;
using Presentation.Model;
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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AddColumnView.xaml
    /// </summary>
    public partial class AddColumnView : Window
    {
        private AddColumnViewModel viewModel;
       
        public AddColumnView(BoardModel b)
        {
            this.viewModel = new AddColumnViewModel(b);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddColumn();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
