using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("BoardTests")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;
    using BusinessLayer;
    using System.Collections.Generic;
    using log4net;
    using System.Reflection;
    using log4net.Config;
    using System.IO;
    using IntroSE.Kanban.Backend.DataAccessLayer;
    using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;


    public class Board
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string name;
        public string Name { get => name; }
        private readonly string email;
        public string Email { get => email; }
        private List<Column> columns;
        public List<Column> Columns { get => columns; } 
        private List<string> boardMember;
        public List<string> BoardMember { get => boardMember;}
        private BoardDTO boardDTO;
        public BoardDTO BoardDTO { get => boardDTO; set { boardDTO = value; } }

        /// <summary>
        /// board constractor. the id board is complex from name and email. 
        /// the email directly is added to the members list
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="name">the board name</param>
        public Board(string email, string name)
        {
            if (email == null)
                throw new Exception("email is not legal");
            if (name == null)
                throw new Exception("name is not legal");
            this.name = name;
            this.email = email;
            boardMember = new List<string>();
            boardMember.Add(email);
            columns = new List<Column>();
            columns.Add(new Column(0, "backlog"));
            columns.Add(new Column(1, "in progress"));
            columns.Add(new Column(2, "done"));
            boardDTO = new BoardDTO(email + " " + name, email);
            boardDTO.Insert();
            //insert to boardDTO.columns and to db
            boardDTO.AddColumn(0, "backlog");
            boardDTO.AddColumn(1, "in progress");
            boardDTO.AddColumn(2, "done");
        }
            /// <summary>
            /// constructor by the dal-for load the data from the dal
            /// </summary>
            /// <param name="boardDTO">the table of board in dal</param>
            public Board(BoardDTO boardDTO)
            {
            this.name = GetBoardName(boardDTO.IdBoard);
            this.email = GetBoardEmail(boardDTO.IdBoard);
            boardMember = new List<string>();
            string members = boardDTO.BoardMember; ;
            string[] arrMembers = members.Split(' ');
            foreach (string word in arrMembers)
            {
                boardMember.Add(word);
            }
            columns = new List<Column>();
            this.boardDTO = boardDTO;
        }

        /// <summary>
        /// diverge id board to email and name
        /// </summary>
        /// <param name="Id">the board's id</param>
        /// <returns>the email of the board</returns>
        private string GetBoardEmail(string Id)
        {
            string Myemail = "";
            int i = 0;
            while (Id[i] != ' ')
            {
                Myemail = Myemail + Id[i];
                i = i + 1;
            }
            
            return Myemail;
        }

        /// <summary>
        /// diverge id board to email and name
        /// </summary>
        /// <param name="Id">the board's id</param>
        /// <returns>the name of the board</returns>
        private string GetBoardName(string Id)
        {
            string Myname = "";
            string Myemail = GetBoardEmail(Id);
            Myname = Id.Substring(Myemail.Length + 1);
            return Myname;
        }

        /// <summary>
        /// return the column name by id
        /// </summary>
        /// <param name="id">the column's id</param>
        /// <returns></returns>
        public Column GetColumn(int id)
        {
            if (id >= columns.Count)
                throw new Exception("the id is not exist");
            return columns[id];
        }

        /// <summary>
        /// add task to a board
        /// </summary>
        /// <param name="dueDate">task's due date</param>
        /// <param name="title">task's title</param>
        /// <param name="description">task's description</param>
        /// <param name="assignEmail">task's assign email</param>
        /// <returns>the task</returns>
        /// 
        public Task AddTask(DateTime dueDate, string title, string description, string assignEmail)
        {
            if(!CheckIfMember(assignEmail))
                throw new Exception("only Board member can add task");
            Task task = new Task(title, description, dueDate, assignEmail);
            columns[0].AddTaskToD(task);
            log.Info("new task created");
            TaskBoardDTO TaskBoardDTO = new TaskBoardDTO(task.Id, email + " " + name, 0);
            TaskBoardDTO.Insert();
            task.TaskBoardDTO = TaskBoardDTO;
            return task;
        }
      

        /// <summary>
        ///move task from the one column to other
        ///if user try to move task from column 2 or task dosent exist- throw error
        /// </summary>
        /// <param name="task">the task we want to move</param>
        /// <param name="assignEmail">the email assign</param>
        /// <param name="columnOrdinal">the column we want to move from</param>
        public void MoveTask(Task task, String assignEmail, int columnOrdinal)
        {
            if (!task.EmailAssigne.Equals(assignEmail))
                throw new Exception("only the assign can move task");
            ///the movement did not allow 
            if (columnOrdinal == columns.Count-1)
                throw new Exception("the movement from the last column is not allow");
            if(!columns[columnOrdinal].DictTask.ContainsValue(task))
                throw new Exception("first you need to create the task");
            columns[columnOrdinal + 1].AddTaskToD(task);
            columns[columnOrdinal].removeTask(task.Id);
            task.TaskBoardDTO.IdColumn=columnOrdinal + 1;
            log.Info("task was moved");
        }
        /// <summary>
        /// add column to the board
        /// </summary>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="columnName">column Name</param>
        public void AddColumn(int columnOrdinal,string columnName)
        {
            if (columnOrdinal > columns.Count)
                throw new Exception("the next available column is:"+columns.Count);
            Column newcolumn = new Column(columnOrdinal, columnName);
            columns.Add(newcolumn);
            for (int i=columns.Count-1;i> columnOrdinal; i--)
                columns[i] = columns[i - 1];
            columns[columnOrdinal] = newcolumn;
            boardDTO.AddColumn(columnOrdinal, columnName);
        }
        /// <summary>
        /// not Use DB for mock Unittest
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="columnName"></param>
        public void AddColumnMock(int columnOrdinal, string columnName)
        {
            if (columnOrdinal > columns.Count)
                throw new Exception("the next available column is:" + columns.Count);
            if(columnName==null)
                throw new Exception("the column cant be null");
            Column newcolumn = new Column(columnOrdinal, columnName);
            columns.Add(newcolumn);
            for (int i = columns.Count - 1; i > columnOrdinal; i--)
                columns[i] = columns[i - 1];
            columns[columnOrdinal] = newcolumn;
        }
        /// <summary>
        /// Removes a specific column 
        /// </summary>
        /// <param name="columnOrdinal">column Ordinal</param>
        public void RemoveColumn(int columnOrdinal)
        {
            if(columnOrdinal>=columns.Count| columnOrdinal<0)
                throw new Exception("there is no column in this columnOrdinal");
            if(columns.Count==2)
                throw new Exception("the minimum is two columns");
            if (columnOrdinal != 0)
                MoveTasksColumn(columnOrdinal, columnOrdinal - 1);
            else
                MoveTasksColumn(columnOrdinal, columnOrdinal + 1);
            columns.RemoveAt(columnOrdinal);
            boardDTO.RemoveColumn(columnOrdinal);
        }
        /// <summary>
        /// remove column without DB for the unittest
        /// </summary>
        /// <param name="columnOrdinal"></param>
        public void RemoveColumnMock(int columnOrdinal)
        {
            if (columnOrdinal >= columns.Count | columnOrdinal < 0)
                throw new Exception("there is no column in this columnOrdinal");
            if (columns.Count == 2)
                throw new Exception("the minimum is two columns");
            if (columnOrdinal != 0)
                MoveTasksColumn(columnOrdinal, columnOrdinal - 1);
            else
                MoveTasksColumn(columnOrdinal, columnOrdinal + 1);
            columns.RemoveAt(columnOrdinal);
        }
        /// <summary>
        /// function that move all the task of one's column to the other
        /// </summary>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="newColumnOrdinal">the column we move all the task to it</param>
        private void MoveTasksColumn(int columnOrdinal,int newColumnOrdinal)
        {
            int movedColumntasks=columns[newColumnOrdinal].DictTask.Count;
            int columntasks = columns[columnOrdinal].DictTask.Count;
            int newColumnOrdinalLimit = columns[newColumnOrdinal].MaxTaskLimit;
            if (newColumnOrdinalLimit != -1 & newColumnOrdinalLimit < movedColumntasks + columntasks)
                throw new Exception("the column can't be remove-there is no place to it's task");
            foreach (Task task in columns[columnOrdinal].DictTask.Values)
            {
                columns[newColumnOrdinal].DictTask.Add(task.Id,task);
                columns[columnOrdinal].removeTask(task.Id);
                if(columnOrdinal!=0)
                    task.TaskBoardDTO.IdColumn = newColumnOrdinal;
            }
        }
        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="newColumnName">The column new name</param>
        public void RenameColumn(int columnOrdinal,string newColumnName)
        {
            if (columnOrdinal >= columns.Count)
                throw new Exception("there is no column in this columnOrdinal");
            columns[columnOrdinal].name = newColumnName;
            boardDTO.GetColumnBoardDTO(columnOrdinal).ColumnName = newColumnName;
        }
        /// <summary>
        ///  Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="columnOrdinal">columnOrdinal</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location.</param>
        public void MoveColumn(int columnOrdinal, int shiftSize)
        {
            if(GetColumn(columnOrdinal)==null)
                throw new Exception("column dosent exist");
            if (!columns[columnOrdinal].isEmpty())
            {
                Column shiftColumn = columns[columnOrdinal];
                if (columnOrdinal + shiftSize > columns.Count)
                    throw new Exception("the shift size make the column out of range");
                for (int i = columnOrdinal; i < columnOrdinal + shiftSize; i++)
                {
                    columns[i] = columns[i + 1];
                }
                columns[columnOrdinal + shiftSize] = shiftColumn;
                boardDTO.MoveColumn(columnOrdinal, shiftSize);
            }
            else
                throw new Exception("only empty column can be move");
        }
        /// <summary>
        /// Move column with no DB for unittest
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="shiftSize"></param>
        public void MoveColumnMock(int columnOrdinal, int shiftSize)
        {
            if (columnOrdinal >= columns.Count | columnOrdinal < 0)
                throw new Exception("column dosent exist");
            if (!columns[columnOrdinal].isEmpty())
            {
                Column shiftColumn = columns[columnOrdinal];
                if (columnOrdinal + shiftSize > columns.Count)
                    throw new Exception("the shift size make the column out of range");
                for (int i = columnOrdinal; i < columnOrdinal + shiftSize; i++)
                {
                    columns[i] = columns[i + 1];
                }
                columns[columnOrdinal + shiftSize] = shiftColumn;
            }
            else
                throw new Exception("only empty column can be move");
        }

        /// <summary>
        /// add new board member to the boardMember list
        /// </summary>
        /// <param name="emailToAdd">the new member email</param>
        public void SetBoardMember(string emailToAdd)
        {
            boardMember.Add(emailToAdd);
            boardDTO.BoardMember=emailToAdd;
            log.Info("Set Board Member"); 
        }

        /// <summary>
        /// set the column limit
        /// </summary>
        /// <param name="ColumnOrdinal">the column id</param>
        /// <param name="limit">the new limit</param>
        public void SetColumnLimit(int ColumnOrdinal, int limit)
        {
            this.GetColumn(ColumnOrdinal).MaxTaskLimit=limit;
            boardDTO.GetColumnBoardDTO(ColumnOrdinal).ColumnLimit = limit;
            log.Info("Set Column Limit");
        }
   
        /// <summary>
        /// check if the board is a one of th board's member
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <returns></returns>
        internal bool CheckIfMember(string email)
        {
            return boardMember.Contains(email);
        }

        ///// <summary>
        ///// set the task's assign by sent it to column
        ///// </summary>
        ///// <param name="task"><the task/param>
        ///// <param name="emailAssignee">the new email assign</param>
        public void SetTaskAssign(int taskId, string emailAssignee,int columnOrdinal)
        {
            if (!boardMember.Contains(emailAssignee))
                throw new Exception("only the board member can be assign of task");
            GetColumn(columnOrdinal).setEmailAssignee(taskId, emailAssignee);
            log.Info("Set Task Assign");
        }
     
        /// <summary>
        /// return the id board by adding the email and the name
        /// </summary>
        /// <returns>id board</returns>
        public string GetIdBoard()
        {
            return email + " " + name;
        }
        /// <summary>
        /// return string of all the members
        /// </summary>
        /// <returns>string of members</returns>
        public String GetMembers()
        {
            string s = "";
            foreach (string member in boardMember)
            {
                s = s + " " + member;
            }
            return s;
        }

        public List<Column> GetColumns()
        {
            return Columns;
        }

        /// <summary>
        /// load columns for board
        /// </summary>
        /// <param name="newBoardColumns"></param>columns of board
        /// <param name="ColumnBoard"></param>list of columnBoard DTO
        /// <param name="ltasks"></param>All tasks
        /// <param name="lBoardAndTask"></param>list of TaskBoard DTO
        public void LoadData(Dictionary<int, Column> newBoardColumns, Dictionary<int, ColumnBoardDTO> ColumnBoard, List<TaskDTO> ltasks, List<TaskBoardDTO> lBoardAndTask)
        {
            for (int i = 0; i < newBoardColumns.Count; i++)
            {
                Columns.Add(newBoardColumns[i]);
                BoardDTO.Columns.Add(ColumnBoard[i]);
            }
           
            foreach (TaskBoardDTO taskboard in lBoardAndTask)
            {
                if (taskboard.IdBoard.Equals(GetIdBoard()))
                {
                    foreach (TaskDTO task in ltasks)
                    {
                        if (taskboard.IdTask == task.IdTask)
                        {
                            Task newTask = new Task(task);
                            newTask.TaskBoardDTO = taskboard;
                            columns[taskboard.IdColumn].AddTaskToD(newTask);
                        }
                    }
                }
            }
        }
    }
}
