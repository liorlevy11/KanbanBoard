using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {

        public UserModel User { get; set; }
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        private ObservableCollection<ColumnModel> columns;
        public ObservableCollection<ColumnModel> Columns { get => columns; set { columns = value; } }
        private List<string> boardMember;
        public List<string> BoardMember { get => boardMember; set { boardMember = value; } }
        private ColumnModel selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return selectedColumn;
            }
            set
            {
                selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }


        public BoardModel(BackendController controller, Board board, UserModel user) : base(controller)
        {
            this.User = user;
            this.email = board.email;
            this.name = board.name;
            this.boardMember = board.BoardMember;
            this.columns = new ObservableCollection<ColumnModel>(board.columns.Select((i) => new ColumnModel(Controller, i, this)));
        }

        /// <summary>
        /// add new column
        /// </summary>
        /// <param name="c"></param>Column to add
        /// <param name="columnOrdinal"></param>new column ID
        public void AddColumn(ColumnModel c, int columnOrdinal)
        {
            //fixing pointers
            Columns.Add(c);
            for (int i = Columns.Count - 1; i > columnOrdinal; i--)
            {
                Columns[i] = Columns[i - 1];
                Columns[i].Id = i;

            }
            Columns[columnOrdinal] = c;
        }

        /// <summary>
        /// remove column from observerecollections and fixing pointers
        /// </summary>
        public void RemoveColumn()
        {
            try
            {
                int ordinal = SelectedColumn.Id;
                Controller.RemoveColumn(User.Email, Email, Name, ordinal);
                //Refresh();
                Columns.RemoveAt(ordinal);
                for (int i = ordinal; i < Columns.Count; i++)
                {
                    Columns[i].Id = i;
                }
                foreach (ColumnModel c in columns)
                {
                    c.Refresh();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Cannot remove column. " + error.Message);
            }
        }

        /// <summary>
        /// move selected column Step steps
        /// </summary>
        /// <param name="Step"></param> movments left of column
        public void MoveColumn(int Step)
        {
            try
            {
                Controller.MoveColumn(User.Email, Email, Name, selectedColumn.Id, Step);
                //columns = new ObservableCollection<ColumnModel>(Controller.GetBoardColumns(User.Email,Email,Name).Select((i) => new ColumnModel(Controller, i, this)));
                ColumnModel c = selectedColumn;
                int columnOrdinal = c.Id;
                c.Id = Columns.Count;

                for (int i = columnOrdinal; i < columnOrdinal + Step; i++)
                {
                    Columns[i] = Columns[i + 1];
                    Columns[i].Id = i;

                }
                Columns[columnOrdinal + Step] = c;
                Columns[columnOrdinal + Step].Id = columnOrdinal + Step;
                //Refresh();
                MessageBox.Show("Move Column Successfully");
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot Move Column. " + e.Message);
            }


        }

        /// <summary>
        /// advance selected task
        /// </summary>
        public void AdvanceTask()
        {
            if (selectedColumn != null)
            {
                TaskModel tomove = SelectedColumn.AdvanceTask();
                if(tomove!=null)
                    columns[selectedColumn.Id + 1].AddTask(tomove);
            }
            else
                throw new Exception("you shold select column and task");
        }

        /// <summary>
        /// sort tasks by due date
        /// </summary>
        public void SortTasks()
        {
            foreach(ColumnModel c in columns)
            {
                c.SortTasks();
            }
        }

        /// <summary>
        /// filter tasks at each column
        /// </summary>
        /// <param name="Filter"></param>
        public void FilterTasks(string Filter)
        {
            foreach (ColumnModel c in columns)
            {
                c.FilterTasks(Filter);
            }
        }

        /// <summary>
        /// return list of task in progress of user
        /// </summary>
        /// <returns></returns>
        public List<TaskModel> TaskInProgress()
        {
            List<TaskModel> l = new List<TaskModel>();
            for(int i=1; i<columns.Count-1; i++)
            {
                List<TaskModel> temp = columns[i].TaskInProgress();
                foreach (TaskModel t in temp)
                {
                    l.Add(t);
                }
            }
            return l;
        }

        /// <summary>
        /// refresh observerCollections from businessLayer
        /// </summary>
        public void Refresh()
        {
            columns.Clear();
            List<ColumnModel> refreshColumns = new List<ColumnModel>(Controller.GetBoardColumns(User.Email,Email,Name).Select((i) => new ColumnModel(Controller, i, this)));
            foreach(ColumnModel c in refreshColumns)
            {
                columns.Add(c);
            }
        }

    }
}
