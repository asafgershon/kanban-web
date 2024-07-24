using System.Reflection.Metadata;
using System.Collections.Generic;
using System;
using log4net;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.DataAccessLayer;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Kanban_2024_2024_24.Backend.BusinessLayer.User
{
    internal class UserFacade
    {
        private Dictionary<string, UserBL> users;
        private UserController uc;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //constructor
        internal UserFacade()
        {
            users = new Dictionary<string, UserBL>();
            uc = new UserController();
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>None, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void Register(string email, string password)
        {
            log.Info("A user try to register");
            email = email.Trim().ToLower();
            if ((email == null) || email.Equals(""))
            {
                log.Error("User tried to register with a null email");
                throw new System.Exception("email is null");
            }
            if (users.ContainsKey(email))
            {
                log.Error("User tried to register with an existing email");
                throw new System.Exception("User name already taken");
            }
            if (!checkPassword(password))
            {
                log.Error("user tried to register with invalid password");
                throw new System.Exception("Password must contain at least one uppercase letter, one lowercase letter, one digit and be between 6 and 20 characters long");
            }
            if (email.Contains(" "))
            {
                log.Error("user tried to register with invalid mail");
                throw new System.Exception("mail cant contain space");
            }

            UserBL u = new UserBL(email, password);
            users.Add(email, u);
            Login(email, password); //after Register user is loged in
            log.Info("User registered successfully");
            return;
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>None, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void Login(string email, string password)
        {
            log.Info("A user try to login");
            email = email.Trim().ToLower();
            if (email != null && users.ContainsKey(email))
            {
                UserBL u = users[email];
                if (u.loggedIn)
                {
                    log.Error("User tried to login while already logged in");
                    throw new System.Exception("User already logged in");
                }
                if (u.signin(password))
                {
                    u.loggedIn = true;
                    log.Info("User logged in successfully");
                    return;
                }
            }
            else
            {
                log.Error("User tried to login with incorrect email or password");
                throw new System.Exception("Incorrect user name or password");
            }
        }

        /// <summary>
        /// This method logs out an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to logout</param>
        /// <returns>None, unless an error occurs</returns>
        internal void Logout(string email)
        {
            log.Info("A user try to logout");
            email = email.Trim().ToLower();
            if (email != null && users.ContainsKey(email))
            {
                UserBL u = users[email];
                if (u.loggedIn)
                {
                    u.loggedIn = false;
                    log.Info("User logged out successfully");
                    return;
                }
                else
                {
                    log.Error("User tried to logout while not logged in");
                    throw new System.Exception("User not logged in");
                }
            }
            else
            {
                log.Error("User tried to logout with incorrect email");
                throw new System.Exception("User does not exist");
            }
        }

        /// <summary>
        /// This method logs out an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to logout</param>
        /// <returns>true if logged in, error if not</returns>
        internal bool IsLoggedIn(string email)
        {
            log.Info("Checking if a user is logged in");
            email = email.Trim().ToLower();
            if (email != null && users.ContainsKey(email))
            {
                if (!users[email].loggedIn)
                {
                    log.Error("User is not logged in");
                    throw new System.Exception("User not logged in");
                }
                log.Info("User is logged in");
                return true;
            }
            log.Error("User does not exist");
            throw new System.Exception("User does not exist");
        }

        //check password enter valid for all condition
        private Boolean checkPassword(string password)
        {
            if (password.Length < 6 || password.Length > 20)
            {
                return false;
            }
            Boolean hasLower = false;
            Boolean hasUpper = false;
            Boolean hasDigit = false;
            foreach (char c in password)
            {
                if (Char.IsLower(c))
                {
                    hasLower = true;
                }
                if (Char.IsUpper(c))
                {
                    hasUpper = true;
                }
                if (Char.IsDigit(c))
                {
                    hasDigit = true;
                }
            }
            return hasLower & hasUpper & hasDigit;
        }

        ///<summary>This method loads all persisted user data.
        /// <returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void loadUsers()
        {
            log.Info("system is loading all users");
            users = new Dictionary<string, UserBL>();
            List<UserDAO> usersDAO = uc.loadAllUsers();
            foreach (UserDAO u in usersDAO)
            {
                users.Add(u.Email, new UserBL(u.Email, u.Password));
            }
        }

        ///<summary>This method deletes all persisted user data.
        /// </summary>
        ///<returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void deleteUsers()
        {
            log.Info("system is deleting all users");
            uc.DeleteAllUsers();
            users = new Dictionary<string, UserBL>();
        }

    }

}
