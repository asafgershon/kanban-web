using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{

    internal class boardController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName="Board";
        private UserBoardStarusDAO buStatus;
        private ColumnController columnsC;
        private taskController taskC;

        // private const string TableNameStatus = "BoardUserStatus";
        public boardController()
        {
            this.buStatus = new UserBoardStarusDAO();
            this.columnsC = new ColumnController();
            this.taskC = new taskController();
            string path =  Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data source={path}; verstion=3;";
            this._tableName = TableName;
        }

        ///<summary>This method loads all persisted board data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<BoardDAO> loadAllBoards()
        {
            //when laod all boards, need to get the max board id and put it in the NextBoardId in the BoardFacede
             List<BoardDAO> res=new List<BoardDAO>();
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
                            // Create a new BoardDAO object 
                            string BoardName = Convert.ToString(reader["BoardName"]);
                            string Owner = Convert.ToString(reader["Owner"]);
                            int BoardId = Convert.ToInt32(reader["BoardId"]);
                            BoardDAO board = new BoardDAO(BoardName,BoardId,Owner);

                            // Add the BoardDAO to the result list
                            res.Add(board);
                        }
                    }
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e);
                    throw new System.Exception("the data retrive faild");
                }
                finally
                {
                    connection.Close();
                }
            return res;
        }

        /// <summary>
        /// This method creates a board in the data base.
        /// </summary>
        /// <param BoardDAO="boardDao">DAO of the board</param>
        /// <returns>A true / false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool AddBoard(BoardDAO boardDao)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName}({boardDao.boardNameColumnName},{boardDao.ownerColumnName},{boardDao.boardIdColumnName}) " +
                    $"VALUES (@BoardName, @Owner , @BoardId)";
                    SQLiteParameter nameParam = new SQLiteParameter(@"BoardName", boardDao.boardName);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"Owner", boardDao.Owner);
                    SQLiteParameter idParam = new SQLiteParameter(@"BoardId", boardDao.boardId);
                    connection.Open();
                    command.CommandText = insert;
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(ownerParam);
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
        /// This method delete a board in the data base.
        /// </summary>
        /// <param BoardDAO="boardDao">DAO of the board</param>
        /// <returns>A true / false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal bool deleteBoard(BoardDAO boardDao)
        {
            buStatus.DeleteBoardUser(boardDao.boardId);
            columnsC.DeleteColumn(boardDao.boardId);
            taskC.DeleteTask(boardDao.boardId);
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string delete = $"DELETE FROM {TableName} WHERE {boardDao.boardIdColumnName} = @BoardId";
                    SQLiteParameter idParam = new SQLiteParameter(@"BoardId", boardDao.boardId);
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
        /// This method update a board in the data base - depened on the "FieldToUpdate"
        /// </summary>
        /// <param boardid="boardId">the board id</param>
        /// <param FieldToUpdate="FieldToUpdate">the feild we want to update</param>
        /// <param valueToUpdate="valueToUpdate">the new value</param>
        /// <returns>A true / false, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void UpdateBoard(int boardId, string FieldToUpdate,object valueToUpdate){
             using (var connection = new SQLiteConnection(this._connectionString))
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string update = $"UPDATE {TableName} SET {FieldToUpdate} = @FieldToUpdate " +
                    $"WHERE BoardId = {boardId}";

                    SQLiteParameter FieldToUpdateParam = new SQLiteParameter(@"FieldToUpdate", valueToUpdate);
                    connection.Open();
                    command.CommandText = update;
                    command.Parameters.Add(FieldToUpdateParam);
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

        ///<summary>This method loads all persisted members data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<string> loadMembers(int boardId){
            return buStatus.loadMembers(boardId);
        }
        ///<summary>This method loads all persisted members data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<TaskDAO> loadTasks(int boardId, int colOrdinal){
            return taskC.loadTasks(boardId,colOrdinal);
        }
        
        ///<summary>This method loads all persisted members data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<ColumnDAO> loadCoulmns(int boardId){
            return columnsC.loadCoulmns(boardId);
        }

        ///<summary>This method delete all persisted board data.
        /// </summary>
        /// <returns>A list of all the boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void DeleteAllBoards()
        {
            columnsC.DeleteAllcolumn();
            taskC.DeleteAllTask();
            buStatus.DeleteAllMembers();
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

        internal void Joinboard(int boardId, string email){
            buStatus.Joinboard(boardId,email);
        }

        internal void LeaveBoard(int boardId, string email){
            buStatus.LeaveBoard(boardId,email);
        }
    }
}