using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for JoinBoardView.xaml
    /// </summary>
    public partial class JoinBoardView : Window
    {
        private JoinBoardViewModel viewModel;
        private ObservableCollection<BoardModel> allBoards;


        public JoinBoardView(UserModel u, ObservableCollection<BoardModel> allBoards)
        {
            this.viewModel = new JoinBoardViewModel(u);
            this.DataContext = viewModel;
            this.allBoards = allBoards;
            InitializeComponent();
        }
        public JoinBoardView()
        {
            InitializeComponent();
        }

        private void BackToBoards_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            viewModel.JoinBoard(allBoards);
        }
    }
}
