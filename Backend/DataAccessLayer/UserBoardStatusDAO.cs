using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{

    internal class UserBoardStarusDAO
    {

        internal int BoardId { get; set; }
        internal string email { get; set; }
        internal bool IsPersisted = false;

        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "BoardUserStatus";

        private const string BoardIdColumnName = "BoardId";
        private const string EmailColumnName = "Email";

        //constructor
        internal UserBoardStarusDAO(int boardid , string email){
            string path =  Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data source={path}; verstion=3;";
            this._tableName = TableName;
            this.BoardId = boardid;
            this.email = email;
            IsPersisted = false;
        }

        //empty constructor
        internal UserBoardStarusDAO()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data source={path}; verstion=3;";
            this._tableName = TableName;
            IsPersisted = false;
        }

        ///<summary>This method loads all persisted boardmembers data.
        /// </summary>
        /// <returns>a List of all the email of the users that memmbers of this board, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<string> loadMembers(int boardId){
            List<string> res=null;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string select = $"SELECT Email FROM {TableName} WHERE BoardId = {boardId}";
                    connection.Open();
                    command.CommandText = select;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        res=new List<string>();
                        while (reader.Read())
                        {
                            // Create a new BoardDAO object 
                            string email = Convert.ToString(reader["Email"]);
                            res.Add(email);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new System.Exception("the data retrive faild");
                }
                finally
                {
                    connection.Close();
                }
            return res;
        }

        /// <summary>
        /// This method add one members to the data base.
        /// </summary>
        /// <param UserBoardStarusDAO="boardDao">UserBoardStarusDAO of the board. incluse all data needed</param>
        /// <returns>A bool, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool Joinboard(int boardId, string email)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName} ({BoardIdColumnName},{EmailColumnName}) " +
                    $"VALUES (@BoardId, @Email)";
                    SQLiteParameter nameParam = new SQLiteParameter(@"BoardId", boardId);
                    SQLiteParameter idParam = new SQLiteParameter(@"Email", email);
                    connection.Open();
                    command.CommandText = insert;
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(idParam);
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    throw new System.Exception("the connection to the database faild");
                }
                finally
                {
                    connection.Close();
                }
            return res > 0;
        }

        /// <summary>
        /// This method deletes one members to the data base.
        /// </summary>
        /// <param UserBoardStarusDAO="boardDao">UserBoardStarusDAO of the board. incluse all data needed</param>
        /// <returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool LeaveBoard(int boardId, string email)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string delete = $"DELETE FROM {TableName} WHERE {BoardIdColumnName} = @BoardId AND {EmailColumnName} = @email";
                    SQLiteParameter idParam = new SQLiteParameter(@"BoardId",boardId);
                    SQLiteParameter emailParam = new SQLiteParameter(@"email",email);
                    connection.Open();
                    command.CommandText = delete;
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
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

        /// <summary>
        /// This method deletes all board members from the data base.
        /// </summary>
        /// <param UserBoardStarusDAO="boardDao">UserBoardStarusDAO of the board. incluse all data needed</param>
        /// <returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool DeleteBoardUser(int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string delete = $"DELETE FROM {TableName} WHERE {BoardIdColumnName} = @BoardId";
                    SQLiteParameter idParam = new SQLiteParameter(@"BoardId", boardId);
                    connection.Open();
                    command.CommandText = delete;
                    command.Parameters.Add(idParam);
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

        ///<summary>This method deletes all persisted board members data
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void DeleteAllMembers()
        { 
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
                    throw new System.Exception("the connection to the database faild");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}