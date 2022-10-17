using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.ViewModel
{
    class AddColumnViewModel:NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
        private BoardModel board;
        public string Title { get; private set; }
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
        public AddColumnViewModel(BoardModel board)
        {
            this.board = board;
            this.user = board.User;
            this.controller = user.Controller;
            Title = "Add new Column to " + board.Email+" "+board.Name;
        }

        /// <summary>
        /// add new column to service and obseverable collection
        /// </summary>
        /// <returns></returns>the added column
        public ColumnModel AddColumn()
        {
            try
            {
                controller.AddColumn(user.Email, board.Email, board.Name, ColumnOrdinal, ColumnName);
                ColumnModel c = new ColumnModel(controller, ColumnOrdinal, ColumnName,board);
                board.AddColumn(c, ColumnOrdinal);
                MessageBox.Show("Column added Successfully.");
                return c;

            }
            catch (Exception error)
            {
                MessageBox.Show("Cannot add column. " + error.Message);
                return null;
            }
        }

    }
}
