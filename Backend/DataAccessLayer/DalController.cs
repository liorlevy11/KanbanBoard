using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalController
    {
        public readonly string _connectionString;
        public readonly string _tableName;
        /// <summary>
        /// constactor of tables
        /// </summary>
        /// <param name="tableName">table name</param>
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        /// <summary>
        /// generic delete function
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName} "
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
        /// generic select
        /// </summary>
        /// <returns>list of DTO</returns>
        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
        /// <summary>
        /// Generic update of two string, and string value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue">new value</param>
        /// <returns></returns>
        public virtual bool Update(string id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"Update {_tableName} set {attributeName}=@attributeValue where ID=@idval";
                command.Parameters.AddWithValue(@"idval", id);
                command.Parameters.AddWithValue(@"attributeValue", attributeValue);
                connection.Open();
                res = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            return res > 0;
        }
        /// <summary>
        /// Generic update by 2 string and int value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public virtual bool Update(string id, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"Update {_tableName} set {attributeName}=@attributeValue where ID=@idval";
                command.Parameters.AddWithValue(@"idval", id);
                command.Parameters.AddWithValue(@"attributeValue", attributeValue);
                connection.Open();
                res = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            return res > 0;
        }
        /// <summary>
        /// Generic update by int string, and int value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool Update(int id, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"Update {_tableName} set {attributeName}=@attributeValue where ID=@idval";
                command.Parameters.AddWithValue(@"idval", id);
                command.Parameters.AddWithValue(@"attributeValue", attributeValue);
                connection.Open();
                res = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            return res > 0;
        }
        /// <summary>
        ///  Generic update by int string, and string value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool Update(int id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"Update {_tableName} set {attributeName}=@attributeValue where ID=@idval";
                command.Parameters.AddWithValue(@"idval", id);
                command.Parameters.AddWithValue(@"attributeValue", attributeValue);
                connection.Open();
                res = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            return res > 0;
        }
        /// <summary>
        /// abstract method
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);
        /// <summary>
        /// abstract method
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        public abstract bool Insert(DTO DTO);

        public abstract bool Delete(DTO DTO);
    }
}

