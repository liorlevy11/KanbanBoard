
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using log4net;
    using System.Reflection;
    using log4net.Config;
    using System.IO;
    using IntroSE.Kanban.Backend.DataAccessLayer;
    using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
    
    public class BoardController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, Board> boards;
        public Dictionary<string, Board> Boards { get => boards; set { boards = value; } }
        private BoardDalController boardDalController;
        /// <summary>
        /// board controller constractor
        /// </summary>
        public BoardController()
        {
            boards = new Dictionary<string, Board>();
            boardDalController = new BoardDalController();
        }
      
        /// <summary>
        /// return board object
        /// </summary>
        /// <param name="userEmail">who try to get</param>
        /// <param name="creatorEmail">the creator of the board</param>
        /// <param name="name">the board's name</param>
        /// <param name="userController">the user controller</param>
        /// <returns>board object</returns>
        public Board GetBoard(string userEmail, string creatorEmail, string name, UserController userController)
        {
            CheckIfUserLogIn(userEmail, userController);
            if (name == null)
                throw new Exception("the name canot be null");
            string id = creatorEmail + " " + name;
            if (!CheckoutBoard(id))
            {
                log.Debug("user try to make action on un exsiting board");
                throw new Exception("board doesnt exist");
            }
            if ((boards[id]).CheckIfMember(userEmail))
                return boards[id];
            throw new Exception("you need to sign for this board first");
        }
        /// <summary>
        /// divarge the id board to email and name
        /// </summary>
        /// <param name="Id">id board</param>
        /// <returns>email of the creator</returns>
        private string GetBoardEmail(string Id)
        {
            string email = "";
            int i = 0;
            while (Id[i] != ' ')
            {
                email = email + Id[i];
                i = i + 1;
            }
            return email;
        }
        /// <summary>
        /// divarge the id the name and email
        /// </summary>
        /// <param name="Id">id board</param>
        /// <returns>board's name</returns>
        private string GetBoardName(string Id)
        {
            string name = "";
            string email = GetBoardEmail(Id);
            name = Id.Substring(email.Length + 1);
            return name;
        }
        /// <summary>
        /// check if the board already exist
        /// </summary>
        /// <param name="idBoard">id board</param>
        /// <returns>true if the board exist, otherwise false</returns>
        private bool CheckoutBoard(string idBoard)
        {
            if (idBoard == null)
                throw new Exception("idboard canot be null");
            if (boards.ContainsKey(idBoard))
                return true;
            return false;
        }
        /// <summary>
        /// add new board
        /// </summary>
        /// <param name="Email">email creator</param>
        /// <param name="name">board's name</param>
        /// <param name="userController">user controller</param>
        public void AddBoard(String Email, String name, UserController userController)
        {
            CheckIfUserLogIn(Email, userController);
            if (name == null)
                throw new Exception("name cant be null");
            string id = Email + " " + name;
            if (CheckoutBoard(id))
                throw new Exception("the board already exist");
            Board board = new Board(Email, name);
            boards[id] = board;
            log.Info("new board created");
        }
        /// <summary>
        /// get the task that inProgress column of one user
        /// </summary>
        /// <param name="userEmail">the user's email</param>
        /// <param name="userController">user controller</param>
        /// <returns>list of task</returns>
        public List<Task> GetListInProgress(string userEmail, UserController userController)
        {
            CheckIfUserLogIn(userEmail, userController);
            List<Task> InProgressTask = new List<Task>();
            foreach (string item in boards.Keys)
            {
                if (boards[item].CheckIfMember(userEmail))
                {
                    for (int i = 1; i < boards[item].Columns.Count - 1; i++)
                    {
                        foreach (Task t in boards[item].Columns[i].DictTask.Values)
                        {
                            if (t.EmailAssigne.Equals(userEmail)) //only task that user assign to will add
                                InProgressTask.Add(t);
                        }
                    }
                }
            }
            return InProgressTask;
        }
        /// <summary>
        /// get all the board of spesific user
        /// </summary>
        /// <param name="userEmail">the user email</param>
        /// <param name="userController">user controller</param>
        /// <returns>list of board</returns>
        public List<string> GetBoardNames(string userEmail, UserController userController)
        {
            CheckIfUserLogIn(userEmail, userController);
            List<string> BoardNames = new List<string>();
            foreach (string item in boards.Keys)
            {
                if (boards[item].CheckIfMember(userEmail))
                {
                    BoardNames.Add(GetBoardName(item));
                }
            }
            return BoardNames;
        }
        public List<Board> GetBoards(string userEmail, UserController userController)
        {
            CheckIfUserLogIn(userEmail, userController);
            List<Board> myBoards = new List<Board>();
            foreach (string item in boards.Keys)
            {
                if (boards[item].CheckIfMember(userEmail))
                    myBoards.Add(boards[item]);
            }
            return myBoards;
        }

        /// <summary>
        /// remove board from boardController, if user do not login or board doesnt exist throw Exception
        /// </summary>
        /// <param name="userEmail">the user want to delete</param>
        /// <param name="creatorEmail">the creator email</param>
        /// <param name="name">board's name</param>
        /// <param name="userController">user controller</param>
        public void RemoveBoard(string userEmail, string creatorEmail, string name, UserController userController)
        {
             CheckIfUserLogIn(userEmail, userController);
            if (!userEmail.Equals(creatorEmail))
                throw new Exception("only the board creator can remove board");
            string id = creatorEmail + " " + name;
            if (!CheckoutBoard(id))
                throw new Exception("board doesnt exist");
            BoardDTO todelete = boards[id].BoardDTO;
            //BoardDTO = new BoardDTO(id, boards[id].GetMembers(), boards[id].GetColumn(0).GetLimitNumber(), boards[id].GetColumn(1).GetLimitNumber(), boards[id].GetColumn(2).GetLimitNumber());
            boardDalController.Delete(todelete);
            boards.Remove(id);
            log.Info("board  removed");
        }

        /// <summary>
        /// return true is user is log in- else throw Exception
        /// </summary>
        /// <param name="Email">user's email</param>
        /// <param name="userController">user controller</param>
        /// <returns>true if log in, othewise throw exception</returns>
        private bool CheckIfUserLogIn(string Email, UserController userController)
        {
            if (userController.GetUser(Email).LoggedIn)
                return true;
            log.Debug("user try to make action while he is not logged to system");
            throw new Exception("To make this action user must log in");
        }

        /// <summary>
        /// join user to exist board
        /// </summary>
        /// <param name="userEmail">the user want to join</param>
        /// <param name="creatorEmail">the creator email</param>
        /// <param name="name">board's name</param>
        /// <param name="userController">user controller</param>
        public void JoinBoard(string userEmail, string creatorEmail, string name, UserController userController)
        {
            CheckIfUserLogIn(userEmail, userController);
            string id = creatorEmail + " " + name;
            if (!CheckoutBoard(id))
                throw new Exception("board doesnt exist");
            if (boards[id].BoardMember.Contains(userEmail))
                throw new Exception("user already join to this board");
            //update at dal
            boards[id].SetBoardMember(userEmail);
            log.Info("JoinBoard");
        }
        /// <summary>
        /// delete the board dal controler
        /// </summary>
        public void DeleteBoardController()
        {
            foreach (string id in boards.Keys)
            {
                boardDalController.Delete(boards[id].BoardDTO);
            }
            log.Debug("Delete Board Controller");
        }
        /// <summary>
        /// load all the data to the Dictionary
        /// </summary>
        public void LoadData()
        {
            List<BoardDTO> lBoard = boardDalController.SelectAllBoards(); 
            ColumnBoardDalController ColumnBoardDalController = boardDalController.ColumnBoardDalController;
            TaskBoardDalController TaskBoardDalController = boardDalController.TaskBoardDalController;
            TaskDalController TaskDalController = TaskBoardDalController.TaskDalController;

            List<ColumnBoardDTO> lColumnBoard = ColumnBoardDalController.SelectAllColumnBoard();
            List<TaskBoardDTO> lBoardAndTask = TaskBoardDalController.SelectAllTaskBoards();
            List<TaskDTO> ltasks = TaskDalController.SelectAllTasks();
            foreach (BoardDTO board in lBoard)
            {
                Board newBoard = new Board(board);
                boards[newBoard.GetIdBoard()] = newBoard;
                Dictionary<int, Column> newBoardColumns = new Dictionary<int, Column>();
                Dictionary<int, ColumnBoardDTO> ColumnBoard = new Dictionary<int, ColumnBoardDTO>();
                foreach (ColumnBoardDTO columnBoard in lColumnBoard)
                {
                    if (columnBoard.IdBoard.Equals(board.IdBoard))
                    {
                        Column column = new Column(columnBoard);
                        newBoardColumns[column.Id] = column;
                        ColumnBoard[column.id] = columnBoard;
                    }
                }
                newBoard.LoadData(newBoardColumns, ColumnBoard, ltasks, lBoardAndTask);
            }

                //for (int i = 0; i < newBoardColumns.Count; i++)
                //{
                //    newBoard.Columns.Add(newBoardColumns[i]);
                //    newBoard.BoardDTO.Columns.Add(ColumnBoard[i]);
                //}
                //List<TaskDTO> newBoardTasks = new List<TaskDTO>();
                //foreach (TaskBoardDTO taskboard in lBoardAndTask)
                //{
                //    if (taskboard.IdBoard.Equals(newBoard.GetIdBoard()))
                //    {
                //        foreach (TaskDTO task in ltasks)
                //        {
                //            if (taskboard.IdTask == task.IdTask)
                //            {
                //                newBoardTasks.Add(task);
                //                Task newTask = new Task(task);
                //                newTask.TaskBoardDTO = taskboard;
                //                newBoard.GetColumn(taskboard.IdColumn).AddTaskToD(newTask);
                //            }
                //        }
                //    }
                //}
            


            //to see what been loaded to program
            foreach (string item in boards.Keys)
            {
                Console.WriteLine(boards[item].Email + " "+ boards[item].Name);
                foreach (Column c in boards[item].Columns)
                {
                    Console.WriteLine(c.Id + " " + c.Name + " " + c.MaxTaskLimit);

                    foreach (int id in c.DictTask.Keys)
                    {
                        Console.WriteLine(id);
                    }
                }

            }


            log.Info("load data");
     
        }
    }
}
