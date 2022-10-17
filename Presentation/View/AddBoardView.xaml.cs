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
    /// Interaction logic for AddBoard.xaml
    /// </summary>
    public partial class AddBoardView : Window
    {
        private AddBoardViewModel viewModel;
        private ObservableCollection<BoardModel> allBoards;



        public AddBoardView(UserModel u, ObservableCollection<BoardModel> allBoards )
        {
            this.viewModel = new AddBoardViewModel(u);
            this.DataContext = viewModel;
            this.allBoards = allBoards;
            InitializeComponent();
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddBoard(allBoards);
        }


        private void BackToAllBoards_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
