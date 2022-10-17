using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class TaskBoardDTO : DTO
    {
        public const string TaskBoardBoardIdColumnName = "IdBoard";
        public const string TaskBoardColumnIdColumnName = "IdColumn";

        internal int idTask;
        public int IdTask { get => idTask; }
        private string idBoard;
        public string IdBoard { get => idBoard; }
        private int idColumn;
        public int IdColumn { get => idColumn; set { idColumn = value; _controller.Update(idTask, TaskBoardColumnIdColumnName, value); } }
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="IdTask">task's id</param>
        /// <param name="IdBoard">board's id</param>
        /// <param name="IdColumn">column's id</param>
        public TaskBoardDTO(int IdTask, string IdBoard, int IdColumn) : base(new TaskBoardDalController())
        {
            this.idTask = IdTask;
            this.idBoard = IdBoard;
            this.idColumn = IdColumn;

        }
    }
}
