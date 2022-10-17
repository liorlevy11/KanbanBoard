using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Presentation.Model
{
    public class TaskModel: NotifiableModelObject  
    {
        public UserModel User { get; set; }
        private BoardModel board;
        private ColumnModel column;
        private int id;
        public int Id { get => id; }
        private DateTime creationTime;
        public DateTime CreationTime { get => creationTime; }
        private string PlcreationTime;
        public string PlCreationTime { get => PlcreationTime; }
        private string title;
        private string description;
        private DateTime dueDate;
        private string assignEmail;
        public string Title
        {
            get => title;
            set
            {
                try
                {
                    Controller.UpdateTaskTitle(User.Email, board.Email, board.Name, column.Id, id, value);
                    title = value;
                    RaisePropertyChanged("Title");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
               
            }
        }
       
        public string Description
        {
            get => description;
            set
            {
                try
                {
                    Controller.UpdateTaskDescription(User.Email, board.Email, board.Name, column.Id, id, value);
                    description = value;
                    RaisePropertyChanged("Description");
                }
                catch (Exception e)
                {
                    //throw new Exception(e.Message);
                    MessageBox.Show(e.Message);
                }
                
            }
        }

        private string newDueDate;
        public string NewDueDate
        {
            get => newDueDate;
            set
            {
                try
                {
                    DateTime d = DateTime.Parse(value);
                    DueDate = d;
                    newDueDate = value;
                    RaisePropertyChanged("NewDueDate");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }


        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                   Controller.UpdateTaskDueDate(User.Email, board.Email, board.Name, column.Id, id, value);
                   dueDate = value;
                   RaisePropertyChanged("DueDate");
                   RaisePropertyChanged("BackgroundColor");

            }
        }
        public string AssignEmail
        {
            get => assignEmail;
            set
            {
                try
                {
                    Controller.AssignTask(User.Email, board.Email, board.Name,column.Id, id, value);
                    assignEmail = value;
                    RaisePropertyChanged("AssignEmail");
                    RaisePropertyChanged("BorderBrush");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (dueDate.CompareTo(DateTime.Now) < 0)
                    return new SolidColorBrush(Colors.Red);
                if ((dueDate - creationTime).TotalDays * 0.75 <= (DateTime.Now - creationTime).TotalDays)
                    return new SolidColorBrush(Colors.Orange);
                return null;


            }
        }
        public System.Windows.Media.Brush BorderBrush
        {
            get
            { 
                if (User.Email.Equals(AssignEmail))
                {
                    Brush b = Brushes.Blue;
                    return b;
                }
                return null;
            }
           
        }


        public TaskModel(BackendController controller, int id,DateTime creationTime,string title, string description, DateTime dueDate, string assignEmail,ColumnModel c) : base(controller)
        {
            this.id = id;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.assignEmail = assignEmail;
            this.dueDate = dueDate;
            this.column = c;
            this.board = c.Board;
            this.User = board.User;
            this.newDueDate = String.Format("{0:G}", dueDate);
            PlcreationTime = String.Format("{0:G}", creationTime);

        }

        public TaskModel(BackendController controller, IntroSE.Kanban.Backend.ServiceLayer.Task t, ColumnModel c) : base(controller)
        {
            this.id = t.Id;
            this.creationTime = t.CreationTime;
            this.title = t.Title;
            this.description = t.Description;
            this.assignEmail = t.emailAssignee;
            this.dueDate = t.DueDate;
            this.column = c;
            this.board = c.Board;
            this.User = board.User;
            this.newDueDate = String.Format("{0:G}", dueDate);
            PlcreationTime = String.Format("{0:G}", creationTime);
        }

        public TaskModel(BackendController controller, IntroSE.Kanban.Backend.ServiceLayer.Task t) : base(controller)
        {
            this.id = t.Id;
            this.creationTime = t.CreationTime;
            this.title = t.Title;
            this.description = t.Description;
            this.assignEmail = t.emailAssignee;
            this.dueDate = t.DueDate;
            this.newDueDate = String.Format("{0:G}", dueDate);
            PlcreationTime = String.Format("{0:G}", creationTime);


        }

     
    }
}