using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class DataService
    {
        public DataService() { }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Response LoadData(UserService s, BoardService b)
        {
            try
            {
                s.UserController.LoadData();
                b.BoardController.LoadData();
                log.Info("Load Data");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("LoadData failed: " + e.Message);
            }
        }

        ///<summary>Removes all persistent data.</summary>
        public Response DeleteData(UserService s, BoardService b)
        {
            try
            {
                s.UserController.DeleteUserController();
                b.BoardController.DeleteBoardController();
                log.Info("Delete Data");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("DeleteData failed: " + e.Message);
            }
        }
    }
}
