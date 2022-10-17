using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class TaskDTO : DTO
    {
        public const string TaskCreationTimeColumnName = "CreationTime";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskTitleColumnName = "Title";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskAssignColumnName = "Assign";

        internal int idTask;
        public int IdTask { get => idTask; }
        private DateTime creationTime;
        public DateTime CreationTime { get => creationTime; set { } }
        private string title;
        public string Title { get => title; set { title = value; _controller.Update(idTask, TaskTitleColumnName, value); } }
        private string description;
        public string Description { get => description; set { description = value; _controller.Update(idTask, TaskDescriptionColumnName, value); } }
        private DateTime dueDate;
        public DateTime DueDate { get => dueDate; set { dueDate = value; _controller.Update(idTask, TaskDueDateColumnName, value.ToString()); } }
        private string assign;
        public string Assign { get => assign; set { assign = value; _controller.Update(idTask, TaskAssignColumnName, value); } }
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="IdTask">IdTask</param>
        /// <param name="CreationTime">CreationTime</param>
        /// <param name="Title">Title</param>
        /// <param name="Description">Description</param>
        /// <param name="DueDate">DueDate</param>
        /// <param name="Assign">Assign</param>
        public TaskDTO(int IdTask, DateTime CreationTime, string Title, string Description, DateTime DueDate, string Assign) : base(new TaskDalController())
        {
            this.idTask = IdTask;
            this.creationTime = CreationTime;
            this.dueDate = DueDate;
            this.title = Title;
            this.description = Description;
            this.assign = Assign;
        }
    }
}
