using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnBoardDalController : DalController
    {
        public const string ColumnBoardTableName = "ColumnBoard";

        public ColumnBoardDalController() : base(ColumnBoardTableName)
        {
           
        }

        public override bool Insert(DTO DTO)
        {
            ColumnBoardDTO ColumnBoardDTO = (ColumnBoardDTO)DTO;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} ({ColumnBoardDTO.ColumnBoardIdBoardColumnName} ,{ColumnBoardDTO.ColumnBoardColumnOrdinalColumnName},{ColumnBoardDTO.ColumnBoardColumnNameColumnName},{ColumnBoardDTO.ColumnBoardColumnLimitColumnName}) " +
                        $"VALUES (@idBoardVal,@ColumnOrdinalVal,@ColumnNameVal,@ColumnLimitVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idBoardVal", ColumnBoardDTO.IdBoard);
                    SQLiteParameter ColumnOrdinalParam = new SQLiteParameter(@"ColumnOrdinalVal", ColumnBoardDTO.ColumnOrdinal);
                    SQLiteParameter ColumnNameParam = new SQLiteParameter(@"ColumnNameVal", ColumnBoardDTO.ColumnName);
                    SQLiteParameter ColumnLimitParam = new SQLiteParameter(@"ColumnLimitVal", ColumnBoardDTO.ColumnLimit);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(ColumnOrdinalParam);
                    command.Parameters.Add(ColumnNameParam);
                    command.Parameters.Add(ColumnLimitParam);
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

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnBoardDTO result = new ColumnBoardDTO(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3));
            return result;
        }

        public List<ColumnBoardDTO> SelectAllColumnBoard()
        {
            List<ColumnBoardDTO> result = Select().Cast<ColumnBoardDTO>().ToList();
            return result;
        }

        public override bool Delete(DTO DTO)
        {
            ColumnBoardDTO deleteColumnBoard = (ColumnBoardDTO)DTO;
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {

                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where IdBoard={"'" + deleteColumnBoard.IdBoard + "'"} and ColumnOrdinal={deleteColumnBoard.ColumnOrdinal}"
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


        public bool DeleteBoard(DTO DTO)
        {
            BoardDTO deleteBoard = (BoardDTO)DTO;
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {

                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where IdBoard={"'" + deleteBoard.IdBoard + "'"}"
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
        /// update columnName at ColumnBoardTable
        /// </summary>
        /// <param name="id1"></param>idBoard
        /// <param name="id2"></param>columnOrdinal
        /// <param name="attributeName"></param>the columnName we want to change
        /// <param name="attributeValue"></param>the changed value
        /// <returns></returns>true if change happend
        public override bool Update(string id1,string attributeName, string attributeValue)
        {
            string[] ids = id1.Split(" ");
            id1 = ids[0] + " " + ids[1];
            int id2 = Int32.Parse(ids[2]);
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"Update {_tableName} set {attributeName}=@attributeValue where IdBoard=@idval1 and ColumnOrdinal=@idval2";
                command.Parameters.AddWithValue(@"idval1", id1);
                command.Parameters.AddWithValue(@"idval2", id2);
                command.Parameters.AddWithValue(@"attributeValue", attributeValue);
                connection.Open();
                res = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            return res > 0;
        }

        /// <summary>
        /// update columnlimit at ColumnBoardTable
        /// </summary>
        /// <param name="id1"></param>idBoard
        /// <param name="id2"></param>columnOrdinal
        /// <param name="attributeName"></param>the columnName we want to change
        /// <param name="attributeValue"></param>the changed value
        /// <returns></returns>true if change happend
        public override bool Update(string id1, string attributeName, int attributeValue)
        {
            string[] ids=id1.Split(" ");
            id1 = ids[0]+" "+ids[1];
            int id2 = Int32.Parse(ids[2]);
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"Update {_tableName} set {attributeName}=@attributeValue where IdBoard=@idval1 And ColumnOrdinal=@idval2";
                command.Parameters.AddWithValue(@"idval1", id1);
                command.Parameters.AddWithValue(@"idval2", id2);
                command.Parameters.AddWithValue(@"attributeValue", attributeValue);
                connection.Open();
                res = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            return res > 0;
        }


    }
}
