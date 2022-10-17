using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskBoardDalController : DalController
    {
        public const string TaskBoardTableName = "TaskBoard";
        public TaskDalController taskDalController;
        public TaskDalController TaskDalController { get => taskDalController; }

        public TaskBoardDalController() : base(TaskBoardTableName)
        {
            taskDalController = new TaskDalController();
        }
   
        /// <summary>
        /// return list of all the tasks in boards
        /// </summary>
        /// <returns>list of Task Board DTO </returns>
        public List<TaskBoardDTO> SelectAllTaskBoards()
        {
            List<TaskBoardDTO> result = Select().Cast<TaskBoardDTO>().ToList();
            return result;
        }
        /// <summary>
        /// convert SQLite to DTO
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>TaskBoardDTO</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskBoardDTO result = new TaskBoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
            return result;
        }
        /// <summary>
        /// insert new taskBoard to the table
        /// </summary>
        /// <param name="TaskBoard"></param>
        /// <returns>false if it doesnt work</returns>
        public override bool Insert(DTO TaskBoard)
        {
            TaskBoardDTO taskBoardDTO = (TaskBoardDTO)TaskBoard;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskBoardTableName} ({TaskBoardDTO.IDColumnName} ,{TaskBoardDTO.TaskBoardBoardIdColumnName},{TaskBoardDTO.TaskBoardColumnIdColumnName}) " +
                        $"VALUES (@idTaskVal,@idBoardVal,@idColumnVal);";

                    SQLiteParameter idTaskParam = new SQLiteParameter(@"idTaskVal", taskBoardDTO.idTask);
                    SQLiteParameter idBoardParam = new SQLiteParameter(@"idBoardVal", taskBoardDTO.IdBoard);
                    SQLiteParameter idColumnParam = new SQLiteParameter(@"idColumnVal", taskBoardDTO.IdColumn);

                    command.Parameters.Add(idTaskParam);
                    command.Parameters.Add(idBoardParam);
                    command.Parameters.Add(idColumnParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log.Error("Board not added to db");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// <summary>
        /// delete TaskBoard and for each task -sent to taskDalController for delete
        /// </summary>
        /// <param name="DTOObj"></param>
        /// <returns></returns>
        public bool DeleteBoard(DTO DTOObj)
        {
            BoardDTO deleteBoard = (BoardDTO)DTOObj;
            //TaskDalController taskDalController = new TaskDalController();
            int res = -1;
            List<TaskBoardDTO> ltaskBoard = new List<TaskBoardDTO>();
            ltaskBoard = SelectAllTaskBoards();
            foreach (TaskBoardDTO task in ltaskBoard)
            {
                if (task.IdBoard == deleteBoard.IdBoard)
                {
                    taskDalController.Delete(task);
                }
            }
            using (var connection = new SQLiteConnection(_connectionString))
            {

                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TaskBoardTableName} where IdBoard={"'"+deleteBoard.IdBoard+"'"}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {   
                    command.Dispose();
                    connection.Close();       
                }
              
                return res > 0;
            }
        }

        public override bool Delete(DTO DTO)
        {
            throw new NotImplementedException();
        }
    }
}
