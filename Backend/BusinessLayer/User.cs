using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly string email;
        public string Email { get => email; }
        private string password;
        public string Password { get => password; }
        private List<string> oldPassword;
        private bool loggedIn;
        public bool LoggedIn { get => loggedIn; set { loggedIn = value; } }
        private UserDTO userDTO;
        public UserDTO UserDTO { get => userDTO; set { userDTO = value; } }
        
        /// <summary>
        /// user constructor
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="Password">the user password</param>
        public User(string email, string Password)
        {
            if (validateEmail(email))
                this.email = email;
            if (validatePasswordRules(Password))
                this.password = Password;
            this.oldPassword = new List<string>();
            this.loggedIn = false;
            userDTO = new UserDTO(email, Password);
            userDTO.Insert();
            log.Info("new user has created");
        }
        /// <summary>
        /// constructor by the dal-for load the data from the dal
        /// </summary>
        /// <param name="UserDTO">the table of user in dal</param>
      
        public User(UserDTO UserDTO)
        {
            this.email = UserDTO.Email;
            this.password = UserDTO.Password;
            this.userDTO = UserDTO;
        }


        /// <summary>
        /// help func that compare strings
        /// </summary>
        /// <param name="pass1">the first password we want to compare</param>
        /// <param name="pass2">the second password we want to compare</param>
        /// <returns></returns>
        private bool compareString(string pass1, string pass2)
        {
            return pass1.Equals(pass2);
        }

        /// <summary>
        /// update password-if newpassword is not at oldPass list, and valid- password will be change
        /// </summary>
        /// <param name="oldPass">user Contemporary password</param>
        /// <param name="newPass">the new password</param>
        public void changePassword(string oldPass, string newPass)
        {
            bool canChange = validatePasswordRules(newPass);
            if (canChange)
            {
                foreach (string p in oldPassword)
                {
                    if (compareString(newPass, p))
                    {
                        canChange = false;
                        break;
                    }
                }
            }
            else
                throw new Exception("Password is not legal");
            if (canChange)
            {
                this.password = newPass;
                oldPassword.Add(oldPass);
                log.Info("change Password");
            }
            else
            {
                throw new Exception("Password was already being used");
            }
        }

        /// <summary>
        /// check the password matches, if not throw Exception
        /// </summary>
        /// <param name="Pass">The password we to check if its equal to the user's real password</param>
        /// <returns>match or not</returns>
        public bool validatePasswordMatch(string Pass)
        {

            if (compareString(Pass, this.password))
                return true;
            else
            {
                log.Debug("the password does not match");
                throw new Exception("Password does not match");
            }
        }

        /// <summary>
        ///  check if password is legal
        /// </summary>
        /// <param name="Pass">The password we to check if it legal</param>
        /// <returns></returns>legal or not
        private bool validatePasswordRules(string Pass)
        {
            int minlen = 4;
            int maxlen = 20;
            bool oneUperCase = false;
            bool oneLowerCase = false;
            bool oneNumber = false;
            bool passlegal = false;

            if (Pass.Length >= minlen && Pass.Length <= maxlen)
            {
                for (int i = 0; i < Pass.Length & (!passlegal); i++)
                {
                    if (Pass[i] >= 48 && Pass[i] <= 57)
                        oneNumber = true;
                    else if (Pass[i] >= 65 && Pass[i] <= 90)
                        oneUperCase = true;
                    else if (Pass[i] >= 97 & Pass[i] <= 122)
                        oneLowerCase = true;

                    passlegal = oneNumber & oneUperCase & oneLowerCase;
                }
            }

            if (!passlegal)
            {
                log.Debug("password do not stands is rules");
                throw new Exception("pass should have 4-20 characters, include at least one uppercase letter, one small character and a number");
            }
            if(!validnot20top(Pass))
            {
                log.Debug("password do not stands is rules The password must not be any of the top 20 passwords - ");
                throw new Exception("pass must not be any of the top 20 passwords ");
            }

            return passlegal;

        }


        /// <summary>
        /// check if email is legal
        /// </summary>
        /// <param name="email">The email address we want to check if it legal</param>
        /// <returns></returns>legal or not
        private bool validateEmail(string email)
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            Regex r = new Regex(pattern);
            if (!r.IsMatch(email))
            {
                log.Debug("email is not valid");
                throw new Exception("email is not valid");
            }
            return true;
        }

        /// <summary>
        /// log out the user, if user is not logges- throw Exception
        /// </summary>
        public void logOut()
        {
            if (this.loggedIn)
                loggedIn=false;
            else
                throw new Exception("User is not logged");
            log.Info("User loged out");
        }



        public bool validnot20top(string pass)
        {
            string[] top20 = { "123456", "123456789", "qwerty", "password", "1111111", "12345678", "abc123", "1234567", "password1", "12345", "1234567890", "123123", "000000", "Iloveyou", "1234", "1q2w3e4r5t", "Qwertyuiop", "123", "Monkey", "Dragon"};
            foreach(string PassFromTop20 in top20)
            {
                if (email.Equals(PassFromTop20))
                    return false;
            }
            return true;
        }

    }
}
