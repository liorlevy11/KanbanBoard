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
    class AddBoardViewModel: NotifiableObject
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

        public AddBoardViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
        }

        /// <summary>
        /// add new board to obseverable collection 
        /// </summary>
        /// <param name="allBoards"></param>collection of all boards of a user
        /// <returns></returns>the new added board
        public BoardModel AddBoard(ObservableCollection<BoardModel> allBoards)
        {
            try
            {
                controller.AddBoard(user.Email, BoardName);
                BoardModel b = new BoardModel(controller,  controller.GetBoard(user.Email, user.Email, BoardName),user);
                //BoardModel b = new BoardModel(controller, user, user.Email, BoardName);
                allBoards.Add(b);
                MessageBox.Show("Board added Successfully");
                return b;
            }
            catch (Exception error)
            {
                MessageBox.Show("Cannot add board. " + error.Message);
                return null;
            }
        }

    }
}
