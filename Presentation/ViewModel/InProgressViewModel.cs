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
    class InProgressViewModel : NotifiableObject
    {
        private Model.BackendController controller;
      
        public UserModel User { get; set; }
        public string Title { get; private set; }
        private List<TaskModel> originalTasks;
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

        public ObservableCollection<TaskModel> Tasks {get; set;}

        public InProgressViewModel(UserModel user, List<TaskModel> tasksInProgress)
        {
            this.controller = user.Controller;
            this.User = user;
            Title = "In Progress tasks of " + user.Email;
            Tasks = new ObservableCollection<TaskModel>(tasksInProgress.Select((i) => i));
            originalTasks = new List<TaskModel>(tasksInProgress.Select((i) => i));
        }

        /// <summary>
        /// sort the in progress tasks by due date
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

        /// <summary>
        /// filter the inprogress tasks by title or description
        /// </summary>
        public void FilterTasks()
        {
            FilterT(Filter);
        }

        public void FilterT(string Filter)
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
            Tasks.Clear();
            foreach (TaskModel t in originalTasks)
            {
                Tasks.Add(t);
            }

        }

    }
}
