using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDalController : DalController
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string UserTableName = "User";

        public UserDalController() : base(UserTableName)
        {

        }
        /// <summary>
        /// return all the users
        /// </summary>
        /// <returns>list of DTO user</returns>
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();

            return result;
        }
        /// <summary>
        /// insert new user to the users table
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public override bool Insert(DTO User)
        {
            UserDTO userDTO = (UserDTO)User;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({UserDTO.IDColumnName} ,{UserDTO.UserPasswordColumnName}) " +
                        $"VALUES (@idVal,@nameVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", userDTO.Email);
                    SQLiteParameter titleParam = new SQLiteParameter(@"nameVal", userDTO.Password);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("User not added to db");
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
        /// <returns>UserDTO</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1));
            return result;

        }

        public override bool Delete(DTO DTO)
        {
            throw new NotImplementedException();
        }
    }
}
