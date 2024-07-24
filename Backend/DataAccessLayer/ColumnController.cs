using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{
    internal class ColumnController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "Column";
        private const string BoardIdColumnName = "BoardId";
        //constructor
        internal ColumnController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data source={path}; verstion=3;";
            this._tableName = TableName;
        }
        ///<summary>This method loads all persisted columns data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<ColumnDAO> loadCoulmns(int boardId)
        {
            //when laod all boards, need to get the max board id and put it in the NextBoardId in the BoardFacede
            List<ColumnDAO> res = null;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string select = $"SELECT TaskLimit, Ordinal FROM {TableName} WHERE BoardId = {boardId}";
                    connection.Open();
                    command.CommandText = select;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new BoardDAO object 
                            int limit = Convert.ToInt32(reader["TaskLimit"]);
                            int ordinal = Convert.ToInt32(reader["Ordinal"]);
                            ColumnDAO col = new ColumnDAO(boardId, limit, ordinal);
                            res.Add(col);
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
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param ColumnDAO="columnDao">the DAO of the column</param>
        /// <returns>A true / false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool SetLimit(string TaskLimitColumnName, int boardId, int ordinal, int limit)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string update = $"UPDATE {TableName} SET {TaskLimitColumnName} = @TaskLimit " +
                    $"WHERE BoardId = {boardId} AND Ordinal = {ordinal}";

                    SQLiteParameter taskLimitParam = new SQLiteParameter(@"TaskLimit", limit);
                    connection.Open();
                    command.CommandText = update;
                    command.Parameters.Add(taskLimitParam);
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
        /// This method delete all column in the data base.
        /// </summary>
        /// <param BoardDAO="boardDao">DAO of the board</param>
        /// <returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void DeleteColumn(int boardId)
        {
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string delete = $"DELETE FROM {TableName} WHERE {BoardIdColumnName} = @BoardId";
                    SQLiteParameter idParam = new SQLiteParameter(@"BoardId", boardId);
                    connection.Open();
                    command.CommandText = delete;
                    command.Parameters.Add(idParam);
                    command.ExecuteNonQuery();
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

        ///<summary>This method deletes all persisted board members data
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void DeleteAllcolumn()
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