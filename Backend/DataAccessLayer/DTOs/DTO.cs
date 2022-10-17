using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DTO
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string IDColumnName = "ID";
        protected DalController _controller;
        //public long Id { get; set; } = -1;
        public DTO(DalController controller)
        {
            _controller = controller;
        }

        public void Insert()
        {
            _controller.Insert(this);
        }

    }
}
