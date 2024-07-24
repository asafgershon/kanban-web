using System;
using System.Collections.Generic;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{
    internal class UserDAO
    {
        internal string Email { get; set; }
        internal string Password { get; set; }
        internal bool IsPersisted { get; set; }
        internal string emailColumnName = "Email";
        internal string passwordColumnName = "Password";
        private UserController uc;

        //constructor
        internal UserDAO(string email, string password)
        {
            this.Email = email;
            this.Password = password;
            this.IsPersisted = false;
            this.uc = new UserController();
        }

        //constructor
        internal UserDAO(string email, string password, bool IsPersisted) : this(email, password)
        {
            this.IsPersisted = IsPersisted;
        }

        //check that this input dont insert into the database already and pass it forward
        internal void persist()
        {
            if (this.IsPersisted)
            {
                throw new System.Exception("the user already in the saved");
            }
            this.uc.addUser(this);
            this.IsPersisted = true;
        }
    }
}