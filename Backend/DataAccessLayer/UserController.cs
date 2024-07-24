using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{
    internal class UserController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "User";

        //constructor
        internal UserController()
        {

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._tableName = TableName;
            this._connectionString = $"Data source={path};version=3;";
            Console.WriteLine(_connectionString);
        }

        ///<summary>This method loads all persisted user data.
        /// </summary>
        /// <returns>a List of all the userDAO data, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<UserDAO> loadAllUsers()
        {
            List<UserDAO> res = new List<UserDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string select = $"SELECT * FROM {TableName}";
                    connection.Open();
                    command.CommandText = select;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new UserDAO object 
                            string email = Convert.ToString(reader["Email"]);
                            string password = Convert.ToString(reader["Password"]);
                            UserDAO user = new UserDAO(email, password, true);
                            res.Add(user);

                        }
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    throw new System.Exception("the user data retrive faild");
                }
                finally
                {
                    connection.Close();
                }
            return res;
        }

        ///<summary>This method deletes all persisted user data.
        /// </summary>
        ///<returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void DeleteAllUsers()
        {
            List<UserDAO> res = new List<UserDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteQuery = $"DELETE FROM {TableName}";

                    using (var command = new SQLiteCommand(deleteQuery, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new System.Exception("the user data retrive faild");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// This method add a new user to the database.
        /// </summary>
        /// <param userdao="UserDao">The user DAO, include all the data of the user</param>
        /// <returns> True or false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool addUser(UserDAO userDAO)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName}({userDAO.emailColumnName},{userDAO.passwordColumnName}) " +
                    $"VALUES (@Email, @Password)";
                    SQLiteParameter emailParam = new SQLiteParameter(@"Email", userDAO.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"Password", userDAO.Password);
                    connection.Open();
                    command.CommandText = insert;
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new System.Exception("the connection to the database faild");
                }
                finally
                {
                    connection.Close();
                }
            return res > 0;
        }
    }
}
