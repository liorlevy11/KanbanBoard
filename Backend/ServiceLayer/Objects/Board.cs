using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
   public struct Board
    {
        public readonly string name;
        public readonly string email;
        public List<Column> columns;
        public List<string> BoardMember;

        internal Board(string email, string name, List<Column> columns, List<string> BoardMember)
        {
            this.name = name;
            this.email = email;
            this.columns = columns;
            this.BoardMember = BoardMember;
        }

        internal Board(BusinessLayer.Board b)
        {
            this.name = b.Name;
            this.email = b.Email;
            this.BoardMember = b.BoardMember;
            columns = new List<Column>();
            foreach (BusinessLayer.Column column in b.Columns)
            {
                columns.Add(new Column(column));
            }
        }


    }
}

