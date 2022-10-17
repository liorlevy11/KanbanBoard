using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
        private BoardModel board;
        public UserModel User { get => user; set { user = value; } }
        public BoardModel Board { get => board; set { board = value; } }
        private int step;
        public int Step
        {
            get => step;
            set
            {
                step = value;
                RaisePropertyChanged("step");
            }
        }
        private int columnOrdinal;
        public int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        private string columnName;
        public string ColumnName
        {
            get => columnName;
            set
            {
                columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }
        public string Title { get; private set; }

        private string filter;
        public string Filter
        {
            get => filter;
            set
            {
                filter = value;
                
                
                RaisePropertyChanged("Filter");
            }
        }

        public BoardViewModel(UserModel user, BoardModel board)
        {
            this.Board = board;
            this.User = user;
            this.controller = user.Controller;
            Title = "The Board: " + board.Name;
            
        }

        /// <summary>
        /// calls log out from service
        /// </summary>
        internal void Logout()
        {
            controller.LogOut(user.Email);
        }

       /// <summary>
       ///calls board to remove column
       /// </summary>
        public void RemoveColumn()
        {
            board.RemoveColumn();  
        }
        /// <summary>
        /// calls board to move column
        /// </summary>
        public void MoveColumn()
        {
            board.MoveColumn(step);
        }

        /// <summary>
        /// calls board to advance task 
        /// </summary>
        public void AdvanceTask()
        {
            try
            {
                board.AdvanceTask();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// calls board to sort tasks
        /// </summary>
        public void SortTasks()
        {
            board.SortTasks();
        }

        /// <summary>
        /// calls board to filter tasks
        /// </summary>
        public void FilterTasks()
        {
            board.FilterTasks(Filter);
        }

    }
}
