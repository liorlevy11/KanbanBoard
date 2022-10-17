using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
    /// Interaction logic for WindowAllBoards.xaml
    /// </summary>
    public partial class AllBoardsView : Window
    {
        private AllBoardsViewModel viewModel;

        public AllBoardsView(UserModel u)
        {
            this.viewModel = new AllBoardsViewModel(u);
            this.DataContext = viewModel;
            InitializeComponent();
        }


        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RemoveBoard_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveBoard();
        }

        private void TaskInProgress_Click(object sender, RoutedEventArgs e)
        {
            List<TaskModel> tasksInProgress =viewModel.TaskInProgress();
            InProgressView inProgressView = new InProgressView(viewModel.User, tasksInProgress);
            inProgressView.Show();
        }

        private void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            
            JoinBoardView joinBoardView = new JoinBoardView(viewModel.User, viewModel.AllBoards);
            joinBoardView.Show();

        }

        private void AddBoard_Click(object sender, RoutedEventArgs e)
        {
            AddBoardView addBoardView = new AddBoardView(viewModel.User,viewModel.AllBoards);
            addBoardView.Show();

        }

        private void Entery_Board(object sender, RoutedEventArgs e)
        {
            BoardModel board = viewModel.EntryBoard();
            BoardView boardV = new BoardView(viewModel.User, board);
            boardV.Show();
            this.Close();
        }

      
    }
}
