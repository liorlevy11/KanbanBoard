using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.ViewModel
{
    class JoinBoardViewModel:NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
        private string boardName;
        public string BoardName
        {
            get => boardName;
            set
            {
                boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
        private string creatorEmail;
        public string CreatorEmail
        {
            get => creatorEmail;
            set
            {
                creatorEmail = value;
                RaisePropertyChanged("CreatorEmail");
            }
        }

        public JoinBoardViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
        }

        /// <summary>
        /// user join to a new board
        /// </summary>
        /// <param name="allBoards"></param>add the joined board to all boards
        public void JoinBoard(ObservableCollection<BoardModel> allBoards)
        {
            try
            {
                controller.JoinBoard(user.Email, CreatorEmail, BoardName);
                BoardModel b = new BoardModel(controller,controller.GetBoard(user.Email, CreatorEmail, boardName),user);
                allBoards.Add(b);
                MessageBox.Show("Join Board Successfully");
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot join board. " + e.Message);
            }
        }
    }
}
