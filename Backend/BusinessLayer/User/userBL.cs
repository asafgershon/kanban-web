using System;
using System.Collections.Generic;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.DataAccessLayer;

namespace Kanban_2024_2024_24.Backend.BusinessLayer.User
{

    internal class UserBL
    {
        internal string Email { get; set; }
        private string Password { get; set; }
        internal bool loggedIn { get; set; }
        private UserDAO dao;

        /// <summary>
        /// This method create a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        internal UserBL(string email, string password)
        {
            Email = email;
            Password = password;
            loggedIn = false;
            dao = new UserDAO(email, password);
            dao.persist();
        }

        //constructor for DAO
        internal UserBL(UserDAO user)
        {
            Email = user.Email;
            Password = user.Password;
            loggedIn = false;
        }

        //check valid password for this user
        internal bool signin(string password)
        {
            return password == Password;
        }
    }
}