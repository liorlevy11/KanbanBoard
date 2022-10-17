using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Data;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDalController : DalController
    {
        private const string TaskTableName = "Task";
        public TaskDalController() : base(TaskTableName)
        {

        }
        /// <summary>
        /// return all the task
        /// </summary>
        /// <returns>list of task DTO</returns>
        public List<TaskDTO> SelectAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            return result;
        }
        /// <summary>
        /// delete task
        /// </summary>
        /// <param name="DTOObj"></param>
        /// <returns>false if it doesnt work</returns>
        public override bool Delete(DTO DTOObj)
        {
            TaskBoardDTO deleteTaskBoard = (TaskBoardDTO)DTOObj;
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {

                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TaskTableName} where ID={deleteTaskBoard.IdTask}"
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

            }
            return res > 0;
        }
        /// <summary>
        /// insert new task
        /// </summary>
        /// <param name="task">task DTO</param>
        /// <returns>false if it doesnt work</returns>
        public override bool Insert(DTO task)
        {

            TaskDTO taskDTO = (TaskDTO)task;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({TaskDTO.IDColumnName},{TaskDTO.TaskCreationTimeColumnName},{TaskDTO.TaskTitleColumnName},{TaskDTO.TaskDescriptionColumnName},{TaskDTO.TaskDueDateColumnName},{TaskDTO.TaskAssignColumnName}) " +
                      $"VALUES (@idTaskVal,@creationTimeVal,@titleVal,@descriptionVal,@dueDateVal,@assignVal);";

                    SQLiteParameter idTaskParam = new SQLiteParameter(@"idTaskVal", taskDTO.IdTask);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", (taskDTO.CreationTime.ToString()));
                    SQLiteParameter titleValParam = new SQLiteParameter(@"titleVal", (taskDTO.Title));
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", (taskDTO.Description));
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", (taskDTO.DueDate.ToString()));
                    SQLiteParameter assignParam = new SQLiteParameter(@"assignVal", (taskDTO.Assign));

                    command.Parameters.Add(idTaskParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(titleValParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(assignParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log error
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
        /// convert SQLite to DTO
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>taskDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        { 
            TaskDTO result = new TaskDTO(reader.GetInt32(0), Convert.ToDateTime(reader.GetString(1)), reader.GetString(2), reader.GetString(3), Convert.ToDateTime(reader.GetString(4)), reader.GetString(5));
            return result;

           
        }
    }
}
