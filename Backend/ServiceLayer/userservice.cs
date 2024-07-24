using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private UserFacade uf;

        //constructor
        internal UserService(UserFacade uf)
        {
            this.uf = uf;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            try
            {
                uf.Register(email, password);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            try
            {
                uf.Login(email, password);
                Response response = new Response(null, email);
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }

        }

        /// <summary>
        /// This method logs out an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to logout</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string Logout(string email)
        {
            try
            {
                uf.Logout(email);
                Response response = new Response();
                return JsonSerializer.Serialize(response);

            }
            catch (Exception ex)
            {
                // Exception handling if any error occurs
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        /// <summary>
        /// This method logs out an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to logout</param>
        /// <returns>An emptey response if the users logged in, and with error if not</returns>
        public string IsloggedIn(string email)
        {
            try
            {
                uf.IsLoggedIn(email);
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        ///<summary>This method loads all persisted users data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal string loadUsers()
        {
            try
            {
                uf.loadUsers();
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }

        ///<summary>This method deletes all persisted board data.
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal string deleteUsers()
        {
            try
            {
                uf.deleteUsers();
                Response response = new Response();
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response);
            }
        }
    }
}
