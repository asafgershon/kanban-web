#nullable enable
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserSL
    {
        public string? Email { get; private set; }

        //constructor
        public UserSL(string email, string password)
        {
            Email = email;
        }
    }
}
