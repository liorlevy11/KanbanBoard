using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UserController userController;
        public UserController UserController { get => userController; }
        public UserService()
        {
            userController = new BusinessLayer.UserController();
        }

        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>
        public Response Register(string userEmail, string password)
        {
            try
            {
                userController.Register(userEmail, password);
                log.Info("user registreted Sucssefuly");
                return new Response();

            }
            catch (Exception e)
            {
                return new Response("Registration failed: " + e.Message);
            }
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string userEmail, string password)
        {
            try
            {
                BusinessLayer.User u = userController.Login(userEmail, password);
                User su = new User(u.Email);
                log.Info("user login Sucssefuly");
                return Response<User>.FromValue(su);

            }
            catch (Exception e)
            {
                return Response<User>.FromError("Login failed: " + e.Message);
            }
        }


        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string userEmail)
        {
            try
            {
                userController.SetUserOnline(userEmail);
                log.Info("user logout Sucssefuly");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response("LogOut failed: " + e.Message);
            }
        }
    }
}
