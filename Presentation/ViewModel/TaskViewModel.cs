using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Windows;

namespace Presentation.ViewModel
{
    class TaskViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        public BoardModel Board;
        public UserModel User;
        private ColumnModel column;
        public ColumnModel Column { get => column; set { column = value; } }
        private string title;
        private string description;
        private DateTime dueDate;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }


        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        public TaskViewModel(ColumnModel column)
        {
            this.column = column;
            this.Board = column.Board;
            this.User = Board.User;
            this.controller = User.Controller;
        }

        /// <summary>
        /// add the new task- calls to service layer
        /// </summary>
        public void AddTask()
        {
            try
            { 
                IntroSE.Kanban.Backend.ServiceLayer.Task t = controller.AddTask(User.Email, Board.Email, Board.Name, Title, Description, DueDate);
                TaskModel task = new TaskModel(controller,t,column);
                column.AddTask(task);
                MessageBox.Show("Task added Successfully.");
            }
            catch(Exception e)
            {
                MessageBox.Show("Cannot add Task. " + e.Message);
            }
        }
    }
}
