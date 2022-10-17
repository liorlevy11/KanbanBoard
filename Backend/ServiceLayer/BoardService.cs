using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private BoardController boardController;
        public BoardController BoardController { get => boardController; }

        public BoardService()
        {
            boardController = new BusinessLayer.BoardController();
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit, UserController UserController)
        {
            try
            {
                boardController.GetBoard(userEmail, creatorEmail, boardName, UserController).SetColumnLimit(columnOrdinal, limit);
                log.Info("column limited");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("LimitColumn failed: " + e.Message);
            }
        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                int limit = b.GetColumn(columnOrdinal).GetMaxTaskLimit();
                return Response<int>.FromValue(limit);
            }
            catch (Exception e)
            {
                return Response<int>.FromError("GetColumnLimit failed: " + e.Message);
            }
        }

        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                string name = b.GetColumn(columnOrdinal).Name;
                return Response<string>.FromValue(name);
            }
            catch (Exception e)
            {
                return Response<string>.FromError("GetColumnName failed: " + e.Message);
            }
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                BusinessLayer.Task task = b.AddTask(dueDate, title, description, userEmail);
                Task t = new Task(task.Id, DateTime.Now, title, description, dueDate, userEmail);
                return Response<Task>.FromValue(t);
            }
            catch (Exception e)
            {
                return Response<Task>.FromError("AddTask failed: " + e.Message);
            }
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.GetColumn(columnOrdinal).setnewDueDateTask(dueDate, taskId, userEmail);
                log.Info("task duedate updated");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("UpdateTaskDueDate failed: " + e.Message);
            }
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.GetColumn(columnOrdinal).setTitleTask(title, taskId, userEmail);
                log.Info("task title updated");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("UpdateTaskTitle failed: " + e.Message);
            }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.GetColumn(columnOrdinal).setDescriptionTask(description, taskId, userEmail);
                log.Info("task descripton updated");

                return new Response();
            }
            catch (Exception e)
            {
                return new Response("UpdateTaskDescription failed: " + e.Message);
            }
        }
        /// <summary>
        /// `nce a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.MoveTask(b.GetColumn(columnOrdinal).GetTask(taskId), userEmail, columnOrdinal);

                log.Info("tasked moved");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("AdvanceTask failed: " + e.Message);
            }
        }
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                IList<Task> l = new List<Task>();
                Dictionary<int, BusinessLayer.Task> d = b.GetColumn(columnOrdinal).DictTask;
                foreach (KeyValuePair<int, BusinessLayer.Task> entry in d)
                {
                    Task task = new Task(entry.Value.Id, entry.Value.CreationTime, entry.Value.Title, entry.Value.Description, entry.Value.DueDate, userEmail);
                    l.Add(task);
                }
                return Response<IList<Task>>.FromValue(l);
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError("GetColumn failed: " + e.Message);
            }
        }
        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string userEmail, string name, UserController UserController)
        {
            try
            {
                boardController.AddBoard(userEmail, name, UserController);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("AddBoard failed: " + e.Message);
            }
        }



        public Response JoinBoard(string userEmail, string creatorEmail, string boardName, UserController UserController)
        {
            try
            {
                boardController.JoinBoard(userEmail, creatorEmail, boardName, UserController);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("JoinBoard failed: " + e.Message);
            }
        }
        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string userEmail, string creatorEmail, string name, UserController UserController)
        {
            try
            {
                boardController.RemoveBoard(userEmail, creatorEmail, name, UserController);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("RemoveBoard failed: " + e.Message);
            }
        }
        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string userEmail, UserController UserController)
        {
            try
            {
                IList<Task> l = new List<Task>();
                List<BusinessLayer.Task> list = boardController.GetListInProgress(userEmail, UserController);
                foreach (BusinessLayer.Task t in list)
                {
                    Task task = new Task(t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, userEmail);
                    l.Add(task);
                }
                return Response<IList<Task>>.FromValue(l);
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError("InProgressTasks failed: " + e.Message);
            }
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.SetTaskAssign(taskId, emailAssignee, columnOrdinal);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("AssignTask failed: " + e.Message);
            }
        }

        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(string userEmail, UserController UserController)
        {
            try
            {
                IList<String> l = new List<String>();
                List<string> list = boardController.GetBoardNames(userEmail, UserController);
                foreach (string name in list)
                {
                    l.Add(name);
                }
                return Response<IList<String>>.FromValue(l);
            }
            catch (Exception e)
            {
                return Response<IList<String>>.FromError("GetBoardNames failed: " + e.Message);
            }
        }
        /// <summary>
        /// Return a dictionary of all the board of user. creator email is the key and list of board's name is the value
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<List<ServiceLayer.Objects.Board>> GetBoards(string userEmail, UserController UserController)
        {
            try
            {
                List<Board> boards=boardController.GetBoards(userEmail, UserController);
                List<ServiceLayer.Objects.Board> serviceBoards = new List<Objects.Board>();
                foreach (Board item in boards)
                {
                    serviceBoards.Add(new ServiceLayer.Objects.Board(item));

                }
                return Response<List<ServiceLayer.Objects.Board>>.FromValue(serviceBoards);
            }
            catch (Exception e)
            {
                return Response<List<ServiceLayer.Objects.Board>>.FromError("GetBoards failed: " + e.Message);
            }
        }
        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The location of the new column. Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="columnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.AddColumn(columnOrdinal, columnName);
                log.Info("column added");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("Add column fail: " + e.Message);
            }
        }
        /// <summary>
        /// Removes a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.RemoveColumn(columnOrdinal);
                log.Info("column removed");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("Remove column fail: " + e.Message);
            }
        }


        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="newColumnName">The new column name</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.RenameColumn(columnOrdinal, newColumnName);
                log.Info("column renamed");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("Rename column fail: " + e.Message);
            }

        }

        /// <summary>
        /// Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>  
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. Negative values are allowed</param>  
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                b.MoveColumn(columnOrdinal, shiftSize);
                log.Info("column moved");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("Move column fail: " + e.Message);
            }

        }

        public Response<List<ServiceLayer.Objects.Column>> GetBoardColumns(string userEmail, string creatorEmail, string boardName, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                List<ServiceLayer.Objects.Column> l = new List<ServiceLayer.Objects.Column>(b.GetColumns().Select((i) => new ServiceLayer.Objects.Column(i)));
                return Response<List<ServiceLayer.Objects.Column>>.FromValue(l);
            }
            catch (Exception e)
            {
                return Response<List<ServiceLayer.Objects.Column>>.FromError("get columns fail: " + e.Message);
            }
        }

        public Response<ServiceLayer.Objects.Board> GetBoard(string userEmail, string creatorEmail, string boardName, UserController UserController)
        {
            try
            {
                BusinessLayer.Board b = boardController.GetBoard(userEmail, creatorEmail, boardName, UserController);
                ServiceLayer.Objects.Board boardService = new ServiceLayer.Objects.Board(b);
                return Response<ServiceLayer.Objects.Board>.FromValue(boardService);
            }
            catch (Exception e)
            {
                return Response<ServiceLayer.Objects.Board>.FromError("GetBoard Failed: " + e.Message);
            }
        }
    }
}
