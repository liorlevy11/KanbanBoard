using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDalController : DalController
    { 
        public const string BoardTableName = "Board";
        private TaskBoardDalController taskBoardDalController;
        public TaskBoardDalController TaskBoardDalController { get => taskBoardDalController; }
        private ColumnBoardDalController columnBoardDalController;
        public ColumnBoardDalController ColumnBoardDalController { get => columnBoardDalController; }

        public BoardDalController() : base(BoardTableName)
        {
            taskBoardDalController = new TaskBoardDalController();
            columnBoardDalController = new ColumnBoardDalController();
        }

        /// <summary>
        /// return all the boards
        /// </summary>
        /// <returns>list of boardDTO</returns>
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }
        /// <summary>
        /// insert new board to the dto
        /// </summary>
        /// <param name="Board"></param>
        /// <returns>if its not work return false</returns>
        public override bool Insert(DTO Board)
        {
            BoardDTO BoardDTO = (BoardDTO)Board;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({BoardDTO.IDColumnName} ,{BoardDTO.BoardMemberColumnName}) " +
                        $"VALUES (@idVal,@memberVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", BoardDTO.IdBoard);
                    SQLiteParameter memberParam = new SQLiteParameter(@"memberVal", BoardDTO.BoardMember);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(memberParam);
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
        /// delete DTO object
        /// </summary>
        /// <param name="DTOObj"></param>
        /// <returns>if dosent work return false</returns>
        public override bool Delete(DTO DTOObj)
        {
            BoardDTO deleteBoard = (BoardDTO)DTOObj;
            int res = -1;
            taskBoardDalController.DeleteBoard(DTOObj);
            columnBoardDalController.DeleteBoard(DTOObj);
            using (var connection = new SQLiteConnection(_connectionString))
            {
                
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {BoardTableName} where ID={"'"+deleteBoard.IdBoard+ "'"}"
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
        /// convert SQLite to DTO
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>boardDTO object</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetString(0), reader.GetString(1));
            return result;
        }

      
    }



}
