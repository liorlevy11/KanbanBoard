using System;
using System.Threading;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int id;
        public int Id { get => id; }
        private readonly DateTime creationTime;
        public DateTime CreationTime { get => creationTime; }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate; set
            {
                legalDueDate(value);
                this.dueDate = value;
                taskDTO.DueDate = value;
            }
        }   
        private string title;
        public string Title { get => title; set
            {
                legalTitle(value);
                this.title = value;
                taskDTO.Title = value;
            }
        }
        private string description;
        public string Description { get => description; set
            {
                legaldescription(value);
                this.description = value;
                taskDTO.Description = value;
            }
        }
        private string emailAssignee;
        public string EmailAssigne
        {
            get => emailAssignee; set
            {
                if (emailAssignee.Equals(value)) throw new Exception(value + " is already assign to task");
                emailAssignee = value;
                taskDTO.Assign = value;
            }
        }  
        private static int counter = 0;
        private TaskDTO taskDTO;
        public TaskDTO TaskDTO { get => taskDTO; set { taskDTO = value; } }
        internal int maxtitlelen = 50;
        internal int maxlendescription = 300;
        public TaskBoardDTO TaskBoardDTO { get; set; }
        
        /// <summary>
        /// constructor Task- the Creation time is now, id is update by a counter
        /// </summary>
        /// <param name="title">we get the title from the user</param>
        /// <param name="description">we get the description from the user</param>
        /// <param name="DueDate">we get the DueDate from the user</param>
        /// <param name="emailAssignee">we get the email Assignee from the user</param>
        internal Task(string title, string description, DateTime DueDate, string emailAssignee)
        {
            this.id = counter;
            Interlocked.Increment(ref counter);
            this.creationTime = DateTime.Now;
            legalTitle(title);
            this.title = title;
            legaldescription(description);
            this.description = description;
            legalDueDate(DueDate);
            this.dueDate = DueDate;
            log.Info("new task created");
            this.emailAssignee = emailAssignee;
            taskDTO = new TaskDTO(id, creationTime, title, description, DueDate, emailAssignee);
            taskDTO.Insert();
            
        }
        /// <summary>
        /// constructor Task from the dal.we use it when we load the data from the dal
        /// </summary>
        /// <param name="TaskDTO">table task in the dal</param>
        
        internal Task(TaskDTO TaskDTO)
        {
            this.id = TaskDTO.IdTask;
            //counter = id;
            Interlocked.Increment(ref counter);
            this.creationTime = TaskDTO.CreationTime ;
            this.title = TaskDTO.Title;
            this.description = TaskDTO.Description;
            this.dueDate = TaskDTO.DueDate;
            this.emailAssignee = TaskDTO.Assign;
            this.taskDTO = TaskDTO;
        }
        //metodes
        //geters and seters:

        /// <summary>
        /// check the assign and call to title setter 
        /// </summary>
        /// <param name = "newTitle" > the new title</param>
        public void setTitle(string newTitle, string email)
        {
            CheckAssign(email);
            Title = newTitle;
        }

        /// <summary>
        /// check the assign and call due date setter 
        /// </summary>
        /// <param name="newDueDate">new due date</param>
        public void setDueDate(DateTime newDueDate, string email)
        {
            CheckAssign(email);
            DueDate = newDueDate;

        }

        /// <summary>
        /// check the assign and call Description setter
        /// </summary>
        /// <param name="newDescription">the new describtion</param>
        public void setDescription(string newDescription, string email)
        {
            CheckAssign(email);
            Description = newDescription;
        }
        //other metodes

        /// <summary>
        /// check if the title is legal, if not throw exception
        /// </summary>
        /// <param name="title">the title we want to check(the task's title)</param>
        public void legalTitle(string title)
        {
            if (title == null)
                throw new Exception("title can not be null");
            if (title.Length > maxtitlelen)
                throw new Exception("the max characters for task's title is 50");
            if (title.Length == 0)
                throw new Exception("the task's title can not be empty");
        }

        /// <summary>
        /// check if description is legal,if not throw Exception
        /// </summary>
        /// <param name="description">the description we want to check(the task's description)</param>
        public void legaldescription(string description)
        {
            if (description == null)
                throw new Exception("description can not be null");
            if (description.Length > maxlendescription)
                throw new Exception("the max characters for description it's 300");
        }

        /// <summary>
        /// check if due date is legal(not null and after now),if not throw Exception
        /// </summary>
        /// <param name="newDueDate">the dueDate we want to check(the task's dueDate)</param>
        public void legalDueDate(DateTime dueDate)
        {
            if (dueDate == null)
                throw new Exception("due date can not be null");
            if (DateTime.Now.CompareTo(dueDate) > 0)
            {
                log.Error("user try to set unvalid dueDate");
                throw new Exception("the DueDate have to be valid (after the currect time and not null ");
            }
        }

        /// <summary>
        /// check if field email assign is equal to given string
        /// </summary> if not equal throw exception
        /// <param name="email"></param> the given string
        private void CheckAssign(string email)
        {
            if (!emailAssignee.Equals(email))
                throw new Exception("only email assign can edit task details");
        }








    }
}
