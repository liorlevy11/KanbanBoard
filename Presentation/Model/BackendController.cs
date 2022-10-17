using System;
using System.Collections.Generic;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;


namespace Presentation.Model
{
    public class BackendController
    {
        private Service Service { get; set; }
        public BackendController(Service service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new Service();
            Service.LoadData();
        }

        //calling service functions
        internal void Register(string username, string password)
        {
            Response res = Service.Register(username, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public UserModel Login(string username, string password)
        {
            Response<User> user = Service.Login(username, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        public void LogOut(string username)
        {
            Response res = Service.Logout(username);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public List<Board> GetBoards(string username)
        {
            Response<List<Board>> res = Service.GetBoards(username);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }

        public void AddBoard(string username,string name)
        {
            Response res = Service.AddBoard(username,name);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
           
        }

        public void RemoveBoard(string username,string creatorEmail, string name)
        {
            Response res = Service.RemoveBoard(username, creatorEmail,name);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }

        public void JoinBoard(string username, string creatorEmail, string name)
        {
            Response res = Service.JoinBoard(username, creatorEmail, name);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        public void RemoveColumn(string username, string creatorEmail, string name ,int columnOrdinal)
        {
            Response res = Service.RemoveColumn(username, creatorEmail, name,columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }

        public IList<Task> InProgressTasks(string username)
        {
            Response<IList<Task>> res = Service.InProgressTasks(username);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }

        public Task AddTask(string username, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            Response<Task> res = Service.AddTask(username,creatorEmail,boardName,title,description,dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;

        }

        public IList<Task> GetColum(string username, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response<IList<Task>> res = Service.GetColumn(username, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }

        public IList<Column> GetBoardColumns(string username, string creatorEmail, string boardName)
        {
            Response<List<Column>> res = Service.GetBoardColumns(username, creatorEmail, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;

        }

        public void AddColumn(string username, string emailCreator, string boardName, int columnOrdinal, string columnName)
        {
            Response res = Service.AddColumn(username, emailCreator, boardName, columnOrdinal, columnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }
        public void MoveColumn(string username, string emailCreator, string boardName, int columnOrdinal, int shiftSize)
        {
            Response res = Service.MoveColumn(username, emailCreator, boardName, columnOrdinal, shiftSize);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }

        public void SetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            Response res = Service.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }

        public Board GetBoard (string userEmail, string creatorEmail, string boardName)
        {
            Response<Board> res = Service.GetBoard(userEmail, creatorEmail, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }

        public void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId,string title)
        {
            Response res = Service.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId,title);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response res = Service.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = Service.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string taskAssign)
        {
            Response res = Service.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, taskAssign);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response res = Service.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

    }
}
