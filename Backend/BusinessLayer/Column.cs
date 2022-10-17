namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using log4net;
    using System.Reflection;
    using log4net.Config;
    using System.IO;
    using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

    public class Column:IColumn
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal readonly int id;
        public int Id { get => id; }
        internal string name;
        public string Name { get => name; }
        private int maxTaskLimit;
        public int MaxTaskLimit { get { return maxTaskLimit; }  set { if(value<0) throw new Exception("limit need to be bigger than zero"); maxTaskLimit = value; } }
        private readonly Dictionary<int, Task> dictTask;
        public Dictionary<int, Task> DictTask { get => dictTask; }
       // private ColumnBoardDTO columnBoardDTO;
       // public ColumnBoardDTO ColumnBoardDTO { get => columnBoardDTO; set { columnBoardDTO = value; } }

        /// <summary>
        /// column constructor. by defult the column is not limit
        /// </summary>
        /// <param name="id">id of the column 0/1/2</param>
        /// <param name="name">name of the column</param>
        public Column(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.maxTaskLimit = -1;
            this.dictTask = new Dictionary<int, Task>();
        }

        public Column(ColumnBoardDTO columnBoard)
        {
            this.id = columnBoard.ColumnOrdinal;
            this.name = columnBoard.ColumnName;
            this.maxTaskLimit = columnBoard.ColumnLimit;
            this.dictTask= new Dictionary<int, Task>();
        }

        //getters and setters
        public int GetMaxTaskLimit()
        {
            if (maxTaskLimit == -1) 
                throw new Exception("the column has no limit"); 
            return maxTaskLimit;
        }
        /// <summary>
        /// search task by task's id in the dictionary
        /// </summary>
        /// <param name="idTask">the id of the task we want</param>
        /// <returns> task object</returns>
        public Task GetTask(int idTask)
        {
            if (!checkIfTaskExist(idTask))
                throw new Exception("task does not exist");
            return dictTask[idTask];
        }


        private void CheckIfColumnDone()
        {
            if (id == 2)
                throw new Exception("task at Column DONE can not be updated"); 
        }
    
        /// <summary>
        /// set the title of task-column get the informition from the board and sent to task
        /// </summary>
        /// <param name="newTitle">the new title</param>
        /// <param name="idtask">the id of the task we want</param>
        /// <param name="email">the email assign</param>
        public void setTitleTask(string newTitle, int idtask, string email)
        {
            CheckIfColumnDone();
            GetTask(idtask).setTitle(newTitle, email);
            log.Info("set TitleTask"); 
        }
        /// <summary>
        /// set the description of task-column get the informition from the board and sent to task
        /// </summary>
        /// <param name="newDescription">the new describtion</param>
        /// <param name="idtask">the id of the task we want</param>
        /// <param name="email">the email assign</param
        public void setDescriptionTask(string newDescription, int idtask, string email)
        {
            CheckIfColumnDone();
           GetTask(idtask).setDescription(newDescription, email);
            log.Info("set DescriptionTask");
        }
        /// <summary>
        /// set the due date of task-column get the informition from the board and sent to task
        /// </summary>
        /// <param name="the new due date"></param>
        /// <param name="idtask">the id of the task we want</param>
        /// <param name="email">the email assign</param
        public void setnewDueDateTask(DateTime newDueDate, int idtask, string email)
        {
            CheckIfColumnDone();
            GetTask(idtask).setDueDate(newDueDate, email);
            log.Info("setnew DueDate Task");
        }
        /// <summary>
        /// set the email assign of task-column get the informition from the board and sent to task
        /// </summary>
        /// <param name="idtask">the id of the task we want</param>
        /// <param name="email">the new email of assign</param>
        public void setEmailAssignee(int idtask, string email)
        {
            CheckIfColumnDone();
            GetTask(idtask).EmailAssigne=email;
            log.Info("set Email Assignee");
        }

        /// <summary>
        /// remove Task from the dictionary
        /// </summary>
        /// <param name="id">the task's id we want to remove</param>
        public void removeTask(int id)
        {
            checkIfTaskExist(id);
            dictTask.Remove(id);
            log.Info("task has removed");
        }

        /// <summary>
        ///add Task to the dictionary,task added only to column backlog,if column passed task limit- throw Exception
        /// </summary>
        /// <param name="task">the task we want to add</param>
        public void AddTaskToD(Task task)
        {
            if ((this.maxTaskLimit != -1) & (dictTask.Keys.Count >= this.maxTaskLimit))
                throw new Exception("cannot add more tasks, the limit is " + maxTaskLimit);
            if (!(dictTask.ContainsKey(task.Id))) { 
                dictTask[task.Id] = task;
                log.Info("Add Task");
            }
            else
                throw new Exception("the task is already in the column" + name);
        }
        public Boolean isEmpty()
        {
            return DictTask.Count != 0;
        }

        /// <summary>
        /// check if the task already exist(by check if the task in the dictinary)
        /// </summary>
        /// <param name="idtask">the id of the task</param>
        /// <returns>false if the task not exist, otherwise true</returns>
        internal bool checkIfTaskExist(int idtask)
        {
            if (idtask == null)
                throw new Exception("id task can not be null");
            if (dictTask.ContainsKey(idtask))
                return true;
            log.Debug("user try to make action on a task that does not exist");
            return false;
        }
    }
}
