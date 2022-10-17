using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDTO : DTO
    {
        public const string UserPasswordColumnName = "Password";


        public string _email;
        private string _Password;
        public string Email { get => _email; }



        public string Password { get => _Password; }

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="Email">Email</param>
        /// <param name="Password">Password</param>
        public UserDTO(String Email, string Password) : base(new UserDalController())
        {
            _email = Email;
            _Password = Password;
        }

      
        


    }
}
