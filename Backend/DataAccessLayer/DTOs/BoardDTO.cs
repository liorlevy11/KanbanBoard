using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class BoardDTO : DTO
    {

        public const string BoardMemberColumnName = "Member";
        //public const string BoardLimitBacklogColumnName = "LimitBacklog";
        //public const string BoardLimitInProgressColumnName = "LimitInProgress";
        //public const string BoardLimitDoneColumnName = "LimitDone";

        private string idBoard;
        public string IdBoard { get => idBoard; }
        private string boardMember;
        public string BoardMember { get => boardMember; set
            {
                string newMembers = "" + this.boardMember + " " + value;
                boardMember = newMembers;
                _controller.Update(idBoard, BoardMemberColumnName, newMembers);
            }
        }
        private List<ColumnBoardDTO> columns;
        public List<ColumnBoardDTO> Columns { get => columns; }

        public BoardDTO(string IdBoard, string BoardMember) : base(new BoardDalController())
        {
            this.idBoard = IdBoard;
            this.boardMember = BoardMember;
            this.columns = new List<ColumnBoardDTO>();
        }

        /// <summary>
        /// add a new column to the list and to data base at the given place
        /// </summary>
        /// <param name="columnOrdinal"></param>column ordinal of the new column
        /// <param name="columnName"></param>column name of the new column
        public void AddColumn(int columnOrdinal, string columnName)
        {
            ColumnBoardDTO ColumnBoardDTO = new ColumnBoardDTO(IdBoard, columnOrdinal, columnName);
            if (columnOrdinal != columns.Count)
            { 
                columns.Add(ColumnBoardDTO); //first add to list and than fix pointers
                for (int i = columns.Count-1 ; i > columnOrdinal; i--)
                {
                    columns[i] = columns[i - 1];  
                    columns[i].ColumnOrdinal = i ;  //update at db
                }
                columns[columnOrdinal] = ColumnBoardDTO;
                ColumnBoardDTO.Insert();//insert new column to db
                columns[columnOrdinal].ColumnOrdinal = columnOrdinal;//update column ordinal of new column at db
            }
            else
            {//case that new column adds at the end
                columns.Add(ColumnBoardDTO);
                ColumnBoardDTO.Insert();
            }
        }

        /// <summary>
        /// remove and delete column from list and db
        /// </summary>
        /// <param name="columnOrdinal"></param>column ordinal of the removed column
        public void RemoveColumn(int columnOrdinal)
        {
            columns[columnOrdinal].Delete();//delete at db
            columns.RemoveAt(columnOrdinal);//remove from list
            for(int i= columnOrdinal; i<columns.Count; i++)
            {
                columns[i].ColumnOrdinal = i;//update at db
            }
        }

        /// <summary>
        /// move given column (column ordinal) "shiftsize" times- change all the needed column ordinal at list and db
        /// </summary>
        /// <param name="columnOrdinal"></param>column ordinal of the moved column
        /// <param name="shiftSize"></param>number of times to shift the column- new column ordinal of column will be column ordinal+shiftSize
        public void MoveColumn(int columnOrdinal,int shiftSize)
        {
            ColumnBoardDTO toMove = columns[columnOrdinal];
            toMove.ColumnOrdinal = columns.Count;
            for (int i = columnOrdinal; i < columnOrdinal + shiftSize; i++)
            {
                columns[i] = columns[i + 1];
                columns[i].ColumnOrdinal = i;

            }
            columns[columnOrdinal + shiftSize] = toMove;
            toMove.ColumnOrdinal = columnOrdinal + shiftSize;
        }

        /// <summary>
        /// returns the column with the given column ordinal
        /// </summary>
        /// <param name="columnOrdinal"></param>column ordinal of the returned column
        /// <returns></returns>column
        public ColumnBoardDTO GetColumnBoardDTO(int columnOrdinal)
        {
            return columns[columnOrdinal];
        }
    }
}

