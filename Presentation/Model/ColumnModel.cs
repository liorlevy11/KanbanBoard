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
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {

        public UserModel User { get; set; }
        public BoardModel Board { get; set; }
        private int id;
        public int Id
        {
            get => id; set
            {
                id = value;
                RaisePropertyChanged("Id");
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
        private int maxTaskLimit;
        public int MaxTaskLimit
        {
            get => maxTaskLimit;
            set
            {
                try
                {
                    Controller.SetColumnLimit(User.Email, Board.Email, Board.Name, id, value);
                    maxTaskLimit = value;
                    RaisePropertyChanged("MaxTaskLimit");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }
        private TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                selectedTask = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
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
        private ObservableCollection<TaskModel> tasks;
        public ObservableCollection<TaskModel> Tasks { get => tasks; set { tasks = value; } }


        public ColumnModel(BackendController controller, Column c, BoardModel b) : base(controller)
        {
            this.id = c.id;
            this.name = c.name;
            this.maxTaskLimit = c.maxTaskLimit;
            this.Board = b;
            this.User = Board.User;
            this.Tasks = new ObservableCollection<TaskModel>(c.dictTask.Values.Select((i) => new TaskModel(controller, i, this)));
        }
      

        public ColumnModel(BackendController controller, int id, string name, BoardModel b) : base(controller)
        {
            this.id = id;
            this.name = name;
            this.maxTaskLimit = -1;
            this.Tasks = new ObservableCollection<TaskModel>();
            this.Board = b;
            this.User = Board.User;

        }

        /// <summary>
        /// add t to tasks
        /// </summary>
        /// <param name="t"></param>
        public void AddTask(TaskModel t)
        {
            Tasks.Add(t);
        }

        /// <summary>
        /// remove t from tasks
        /// </summary>
        /// <param name="t"></param>
        public void RemoveTask(TaskModel t)
        {
            Tasks.Remove(t);
        }

        /// <summary>
        /// advance task-calling to serviceLayer
        /// </summary>
        /// <returns></returns>
        public TaskModel AdvanceTask()
        {
            try
            {
                Controller.AdvanceTask(User.Email, Board.Email, Board.Name, Id, SelectedTask.Id);
                TaskModel task = selectedTask;
                Tasks.Remove(selectedTask);
                MessageBox.Show("Move Task Successfully");
                return task;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot Move Task. " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// sort tasks in column by due date
        /// </summary>
        public void SortTasks()
        {
            if (Tasks.Count != 0)
            {
                Comparison<TaskModel> compare = new Comparison<TaskModel>(CompareTo);
                Sort(Tasks, compare);
            }
        }

        /// <summary>
        /// sort the tasks- use compare of task model that define by duedate
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparison"></param>
        public static void Sort(ObservableCollection<TaskModel> collection, Comparison<TaskModel> comparison)
        {
            var sortableList = new List<TaskModel>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }

        /// <summary>
        /// return int that represent the comparison between tasks due date
        /// </summary>
        /// <param name="t1"></param>task model 1
        /// <param name="t2"></param>task model 2
        /// <returns></returns>
        public int CompareTo(TaskModel t1, TaskModel t2)
        {
            return t1.DueDate.CompareTo(t2.DueDate);
        }

        public void FilterTasks(string Filter)
        {

            if (string.IsNullOrWhiteSpace(Filter))
            {
                Refresh();
            }
            else
            {
                List<TaskModel> toRemove = new List<TaskModel>();
                ObservableCollection<TaskModel> FilterTasks = new ObservableCollection<TaskModel>(Tasks.Where((task) => task.Title.ToLower().Contains(Filter) | task.Description.ToLower().Contains(Filter)));
                foreach (TaskModel task in Tasks)
                {
                    if (!FilterTasks.Contains(task))
                        toRemove.Add(task);
                }

                foreach (TaskModel task in toRemove)
                {
                    Tasks.Remove(task);
                }
            }
        }

        public void Refresh()
        {
            List<TaskModel> refreshTasks = new List<TaskModel>(Controller.GetColum(User.Email, Board.Email, Board.Name, Id).Select((i) => new TaskModel(Controller, i, this)));
            tasks.Clear();
            foreach (TaskModel t in refreshTasks)
            {
                tasks.Add(t);
            }

        }

        public List<TaskModel> TaskInProgress()
        {
            List<TaskModel> TaskInProgress = new List<TaskModel>(Tasks.Where((task) => task.AssignEmail.Equals(User.Email)));
            return TaskInProgress;

        }
    }
}
