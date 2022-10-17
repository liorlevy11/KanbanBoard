using System;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer;



namespace IntroSE.Kanban.Backend.BusinessLayer

{
    public class UserController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, User> users;
        public Dictionary<string, User> Users { get => users; }
        private bool userOnline;
        public bool UserOnline { get => userOnline; set { userOnline = value; } }
        private UserDalController UserDalController;
        /// <summary>
        /// user constroller constractor
        /// </summary>
        public UserController()
        {
            users = new Dictionary<string, User>();
            userOnline = false;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            UserDalController = new UserDalController();
        }

        /// <summary>
        /// return object User, if does not exist or email is null throw Exception
        /// </summary>
        /// <param name="email">The email address of the user we want to get</param>
        /// <returns>return user, if the user exist</returns>
        public User GetUser(string email)
        {
            if (email == null)
            {
                log.Error("user entered email as null");
                throw new Exception("null is not a user");
            }
            if (existEmail(email))
                return users[email];
            log.Error("User does not exist");
            throw new Exception("User does not exist");
        }
        /// <summary>
        /// register new user to kanban, if user already exist or try to register with null- throw Exception
        /// </summary>
        /// <param name="email">>The email address the user try to register with</param>
        /// <param name="pass">The password the user try to register with</param>
        /// 
        public void Register(string email, string pass)
        {
            if (email == null || pass == null)
            {
                log.Error("user entered email or pass as null when register");
                throw new Exception("email and pass cannot be null");
            }
            ///verify email does not exist
            if (existEmail(email))
            { ///throw exeption if email is already registered
                log.Info("a register user tries to register again");
                throw new Exception("user alredy exists");
            }
            //create user
            User U = new User(email, pass);
            //add new user to users
            users[email] = U;
        }
        /// <summary>
        ///login user, its also check if only one user log in to system
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="pass">The password of the user to login</param>
        /// <returns>the user if he sucssess</returns>
        public User Login(string email, string pass)
        {
            if (email == null | pass == null)
            {
                log.Error("user entered email or pass as null when log in");
                throw new Exception("email or password can not be null");

            }

            if (userOnline)
            {
                log.Error("user try to log in when another user is logged");
                throw new Exception("can not log to system, there is a user already online");

            }

            if (existEmail(email))
            {
                if (users[email].LoggedIn)
                {
                    log.Error("user try to log in when he was already online");
                    throw new Exception("user already log");

                }

                users[email].validatePasswordMatch(pass);

                users[email].LoggedIn = true;
                userOnline = true;
                return users[email];
            }

            throw new Exception("user does not exist");
        }

        /// <summary>
        /// check if the user already regiser
        /// </summary>
        /// <param name="email">The email address we want to check if it exist</param>
        /// <returns></returns>
        private bool existEmail(string email)
        {
            return users.ContainsKey(email);
        }
        /// <summary>
        /// represent the login/logout of the user at system, if user log out the system, changes to false
        /// </summary>
        /// <param name="email">The email address of the user how want to change his situation</param>
        public void SetUserOnline(string email)
        {
            GetUser(email).logOut();
            userOnline = false;

        }
        /// <summary>
        /// load all the data to the Dictionary
        /// </summary>
        public void LoadData()
        {
            List<UserDTO> list = UserDalController.SelectAllUsers();
            foreach (UserDTO user in list)
            {
                User newUser = new User(user);
                users[user.Email] = newUser;
            }
            log.Info("Load Data of usercontroller");
        }

        /// <summary>
        /// delete user from the controller
        /// </summary>
        public void DeleteUserController()
        {
            UserDalController.Delete();
            log.Debug(" User Controller delete");
        }
    }
}
