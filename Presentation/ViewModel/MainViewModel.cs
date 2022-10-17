using Presentation.Model;
using System;

namespace Presentation.ViewModel
{
    class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// log in the user to the system- calls service layer
        /// </summary>
        /// <returns></returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// calls service layer to register a user by entered email and password
        /// </summary>
        public void Register()
        {
            Message = "";
            try
            {
                Controller.Register(Username, Password);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public MainViewModel()
        {
            this.Controller = new BackendController();
           
        }
    }
}
