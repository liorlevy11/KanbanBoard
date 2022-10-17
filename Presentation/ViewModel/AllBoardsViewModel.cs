using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class AllBoardsViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
        public UserModel User { get => user; }    
        private ObservableCollection<BoardModel> allBoards;
        public ObservableCollection<BoardModel> AllBoards { get=>allBoards; set { allBoards = value; } }
        public string Title { get; private set; }
       
        private BoardModel selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return selectedBoard;
            }
            set
            {
                selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        public AllBoardsViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "All Boards of " + user.Email;
            AllBoards = new ObservableCollection<BoardModel>(controller.GetBoards(user.Email).Select((i) => new BoardModel(controller, i,user)));
        }

        /// <summary>
        /// log out a user
        /// </summary>
        internal void Logout()
        {
            controller.LogOut(user.Email);
        }

        /// <summary>
        /// add new board
        /// </summary>
        /// <param name="b"></param>
        public void AddBoard(BoardModel b)
        {
            allBoards.Add(b);
        }

        /// <summary>
        /// remove the selected board
        /// </summary>
        public void RemoveBoard()
        {
            try
            {
                controller.RemoveBoard(user.Email, SelectedBoard.Email, SelectedBoard.Name);
                AllBoards.Remove(SelectedBoard);
            }
            catch (Exception error)
            {
                MessageBox.Show("Cannot remove board. " + error.Message);
            }
            
        }

        /// <summary>
        /// return the user selected board
        /// </summary>
        /// <returns></returns>
        public BoardModel EntryBoard()
        {
            return SelectedBoard;
        }

        /// <summary>
        /// return list og task in progress of user
        /// </summary>
        /// <returns></returns>
        public List<TaskModel> TaskInProgress()
        {
            List<TaskModel> TaskInProgress = new List<TaskModel>();
            foreach(BoardModel b in allBoards)
            {
                foreach(TaskModel t in b.TaskInProgress())
                {
                    TaskInProgress.Add(t);
                }
            }
            return TaskInProgress;
        }

    }
}
