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
    /// Interaction logic for WindowBoard.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        public BoardView(UserModel u, BoardModel b)
        {
            
            this.viewModel = new BoardViewModel(u, b);
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

        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskView taskView = new TaskView(viewModel.Board.Columns[0]);
            taskView.Show();
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            AddColumnView addColumnView = new AddColumnView(viewModel.Board);
            addColumnView.Show();
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AllBoardsView boardView = new AllBoardsView(viewModel.User);
            boardView.Show();
            this.Close();
        }

        private void MoveColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.MoveColumn();
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AdvanceTask();
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