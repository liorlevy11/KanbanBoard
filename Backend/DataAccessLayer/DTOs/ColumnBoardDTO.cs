using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class ColumnBoardDTO : DTO
    {
        public const string ColumnBoardIdBoardColumnName = "IdBoard";
        public const string ColumnBoardColumnOrdinalColumnName = "ColumnOrdinal";
        public const string ColumnBoardColumnNameColumnName = "ColumnName";
        public const string ColumnBoardColumnLimitColumnName = "ColumnLimit";

        private string idBoard;
        public string IdBoard { get => idBoard; }
        private int columnOrdinal;
        public int ColumnOrdinal { get => columnOrdinal; set { _controller.Update(idBoard + " " + columnOrdinal, ColumnBoardColumnOrdinalColumnName, value); columnOrdinal = value;  } }
        private string columnName;
        public string ColumnName { get => columnName; set { columnName = value; _controller.Update(idBoard + " " + columnOrdinal, ColumnBoardColumnNameColumnName, value);  } }
        private int columnLimit;
        public int ColumnLimit { get => columnLimit; set { columnLimit = value; _controller.Update(idBoard + " " + columnOrdinal, ColumnBoardColumnLimitColumnName, value); } }

        public ColumnBoardDTO(string idBoard,int columnOrdinal, string columnName,int columnLimit) : base(new ColumnBoardDalController())
        {
            this.idBoard = idBoard;
            this.columnOrdinal = columnOrdinal;
            this.columnName = columnName;
            this.columnLimit = columnLimit;
        }

        public ColumnBoardDTO(string idBoard, int columnOrdinal, string columnName) : base(new ColumnBoardDalController())
        {
            this.idBoard = idBoard;
            this.columnOrdinal = columnOrdinal;
            this.columnName = columnName;
            this.columnLimit = -1;
        }

        public void Delete()
        {
            _controller.Delete(this);
            
        }
    }
}

