using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{

    internal class taskController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "Task";

        private const string ColOrdinalColumnName="ColumnOrdinal";
        private const string BoardIdColumnName="BoardID";

        //constructor
        internal taskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data source={path}; verstion=3;";
            this._tableName = TableName;
        }

        ///<summary>This method loads all persisted task data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<TaskDAO> loadTasks(int boardId, int ordinal)
        {
            List<TaskDAO> res = new List<TaskDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string select = $"SELECT * FROM {TableName} WHERE BoardId = {boardId} AND ColumnOrdinal = {ordinal}";
                    connection.Open();
                    command.CommandText = select;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new TaskDAO object 
                            int taskId = Convert.ToInt32(reader["TaskId"]);
                            DateTime dueDate = Convert.ToDateTime(reader["DueDate"]);
                            DateTime time = Convert.ToDateTime(reader["Time"]);
                            string description = Convert.ToString(reader["Description"]);
                            string asignee = Convert.ToString(reader["Asignee"]);
                            string title = Convert.ToString(reader["Title"]);
                            TaskDAO task = new TaskDAO(taskId, title, dueDate, time, description, ordinal, asignee, boardId);

                            // Add the BoardDAO to the result list
                            res.Add(task);
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
        /// This method deletes all board members from the data base.
        /// </summary>
        /// <param UserBoardStarusDAO="boardDao">UserBoardStarusDAO of the board. incluse all data needed</param>
        /// <returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool DeleteTask(int boardId)
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

        /// <summary>
        /// This method creates a task in the data base.
        /// </summary>
        /// <param TaskDAO="="">DAO of the task</param>
        /// <returns>A true / false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool addTask(TaskDAO taskDao)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName}({taskDao.TaskIdColumnName},{taskDao.TimeColumnName},{taskDao.DueDateColumnName},{taskDao.TitleColumnName}," +
                        $"{taskDao.DescriptionColumnName},{taskDao.ColumnsOrdinaleColumnName},{taskDao.AssigneColumnName},{taskDao.BoardIdColumnName}) " +
                    $"VALUES (@TaskId, @Time ,@DueDate, @Title, @Description, @ColumnOrdinal, @Asignee, @BoardID)";
                    SQLiteParameter IdParam = new SQLiteParameter(@"TaskId", taskDao.taskId);
                    SQLiteParameter TimeParam = new SQLiteParameter(@"Time", taskDao.time);
                    SQLiteParameter dateParam = new SQLiteParameter(@"DueDate", taskDao.DueDate);
                    SQLiteParameter titleParam = new SQLiteParameter(@"Title", taskDao.Title);
                    SQLiteParameter descripitionParam = new SQLiteParameter(@"Description", taskDao.Description);
                    SQLiteParameter OrdinalParam = new SQLiteParameter(@"ColumnOrdinal", taskDao.ColumnOrdinal);
                    SQLiteParameter assignParam = new SQLiteParameter(@"Asignee", taskDao.Asignee);
                    SQLiteParameter boardidParam = new SQLiteParameter(@"BoardID", taskDao.boardId);
                    connection.Open();
                    command.CommandText = insert;
                    command.Parameters.Add(IdParam);
                    command.Parameters.Add(TimeParam);
                    command.Parameters.Add(dateParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descripitionParam);
                    command.Parameters.Add(OrdinalParam);
                    command.Parameters.Add(assignParam);
                    command.Parameters.Add(boardidParam);
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
        /// This method update a board in the data base - depened on the "FieldToUpdate"
        /// </summary>
        /// <param boardid="boardId">the board id</param>
        /// <param FieldToUpdate="FieldToUpdate">the feild we want to update</param>
        /// <param valueToUpdate="valueToUpdate">the new value</param>
        /// <returns>A true / false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void UpdateTask(int taskId,int boardId, string FieldToUpdate,object valueToUpdate){
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string update = $"UPDATE {TableName} SET {FieldToUpdate} = @valueToUpdate " +
                    $"WHERE TaskId = {taskId} AND BoardId = {boardId}";

                    SQLiteParameter valueParam = new SQLiteParameter(@"valueToUpdate",valueToUpdate);
                    connection.Open();
                    command.CommandText = update;
                    command.Parameters.Add(valueParam);
                    command.ExecuteNonQuery();
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
        }

        ///<summary>This method delete all persisted task data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void DeleteAllTask()
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