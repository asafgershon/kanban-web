using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using log4net;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using Kanban_2024_2024_24.Backend.DataAccessLayer;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Net.Sockets;


namespace Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard
{

    internal class BoardFacede
    {
        private boardController bc;
        private Dictionary<int, BoardBL> boards;
        private UserFacade uf;
        private int nextBoardID;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardFacede(UserFacade userfacade)
        {
            this.bc = new boardController();
            this.boards = new Dictionary<int, BoardBL>();
            this.uf = userfacade;
            if (boards.Count > 0)
            {
                this.nextBoardID = boards.Keys.Max() + 1;
            }
            else
            {
                this.nextBoardID = 1;
            }

        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A TaskBL that added or error message.</returns>
        internal void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            log.Info("A user try to add a task");
            uf.IsLoggedIn(email); // Check if user is logged in
            email = email.Trim().ToLower();
            boardName = boardName.Trim();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(boardName) || string.IsNullOrEmpty(title))
            {
                // Check if description exceeds maximum length
                log.Error("one parameters is empty");
                throw new ArgumentException("one of the option is empty");
            }
            if (description.Length > 300)
            {
                // Check if description exceeds maximum length
                log.Error("description exceeds 300 characters");
                throw new ArgumentException("can't add task because description exceeds 300 characters.");
            }
            if (title.Length > 50)
            {
                // Check if description exceeds maximum length
                log.Error("can't add task because title exceeds 50 characters");
                throw new ArgumentException("title exceeds 50 characters.");
            }
            BoardBL board = GetBoardByName(email, boardName);
            if (board != null)
            {
                TaskBL newTask = board.createTask(email, title, description, dueDate);
                newTask.dao.persist();
                board.AddTask(newTask);
                log.Info("Task added successfully");
            }
            else
            {
                log.Error("can't add task because board does not exist");
                throw new ArgumentException("board does not exist");
            }
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A TaskBL that added or error message.</returns>
        internal void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            log.Info("A user try to advance a task");
            uf.IsLoggedIn(email); // Check if user is logged in
            if (columnOrdinal == 2)
            {
                // Check if task is in 'done' column
                log.Error("can't advance the task because the task is in done collumns");
                throw new ArgumentException("the task is in done collumns");
            }
            BoardBL board = GetBoardByName(email, boardName);
            if (board != null)
            {
                board.AdvanceTask(columnOrdinal, taskId, email);
                log.Info("Task advanced successfully");
            }
            else
            {
                log.Error("board does not exist");
                throw new ArgumentException("your board doesnt exist");
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A TaskBL that Update or error message.</returns>
        internal void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            log.Info("A user try to update a task description");
            uf.IsLoggedIn(email); // Check if user is logged in
            if (description == null || description.Length > 300 || email == null)
            {
                // Check if description exceeds maximum length
                log.Error("can't update the description because description exceeds 300 characters");
                throw new ArgumentException("Description exceeds 300 characters.");
            }
            if (columnOrdinal == 2)
            {
                log.Error("can't update the description because the task in done column");
                throw new ArgumentException("your task in done column");
            }
            else
            {
                BoardBL board = GetBoardByName(email, boardName);
                if (board != null)
                {
                    board.UpdateTaskDescription(columnOrdinal, taskId, description, email);
                    log.Info("Task description updated successfully");
                }
                else
                {
                    log.Error("can't update the description because board does not exist");
                    throw new ArgumentException("board does not exist");
                }


            }

        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A TaskBL that Update or error message.</returns>
        internal void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            log.Info("A user try to update a task title");
            uf.IsLoggedIn(email); // Check if user is logged in
            if (string.IsNullOrEmpty(title) || title.Length > 50 || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(boardName))
            {
                // Check if description exceeds maximum length
                log.Error("can't update task title because your title is empty or pass 50 charcters");
                throw new ArgumentException("your title is empty or pass 50 charcters");
            }
            if (columnOrdinal == 2)
            {
                log.Error("can't update task title because your task in done column");
                throw new ArgumentException("your task in done column");
            }
            else
            {
                BoardBL board = GetBoardByName(email, boardName);
                if (board != null)
                {
                    log.Info("Task title updated successfully");
                    board.UpdateTaskTitle(columnOrdinal, taskId, title, email);
                }
                else
                {
                    log.Error("can't update task title because board does not exist");
                    throw new ArgumentException("board does not exist");
                }
            }

        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A TaskBL that Update or error message.</returns>
        internal void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            log.Info("A user try to update a task due date");
            uf.IsLoggedIn(email); // Check if user is logged in
            if (columnOrdinal == 2)
            {
                log.Error("can't update task due date because your task in done column");
                throw new ArgumentException("your task in done column");
            }
            BoardBL board = GetBoardByName(email, boardName);
            if (board != null)
            {
                log.Info("A user try to update a task due date");
                board.UpdateTaskDueDate(columnOrdinal, taskId, dueDate, email);
                log.Info("Task due date updated successfully");
            }
            else
            {
                log.Error("can't update task due date because board does not exist");
                throw new ArgumentException("board does not exist");
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        internal void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            log.Info("A user try to limit a column");
            uf.IsLoggedIn(email); // Check if user is logged in
            BoardBL currBoard = GetBoardByName(email, boardName);
            //check the requested board exist
            if (currBoard != null)
            {
                log.Info("Column limited successfully");
                currBoard.setLimit(columnOrdinal, limit);
            }
            else
            {
                log.Error("can't limit the column because board/email does not exist");
                throw new ArgumentException("board/email does not exist");
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> the column's limit, unless the board doesnt exist in the user's boards </returns>
        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            log.Info("A user try to get a column limit");
            uf.IsLoggedIn(email); // Check if user is logged in
            BoardBL currBoard = GetBoardByName(email, boardName);
            //check the requested board exist
            if (currBoard != null)
            {
                int currColLimit = currBoard.GetColumnLimit(columnOrdinal);
                log.Info("Column limit retrieved successfully");
                // in case there is no limit of task in the requested column
                if (currColLimit == -1)
                {
                    return -1;
                }
                return currColLimit;
            }
            else
            {
                log.Error("board/email does not exist");
                throw new ArgumentException("board/email does not exist");
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs </returns>
        internal List<TaskBL> GetColumn(string email, string boardName, int columnOrdinal)
        {
            log.Info("A user try to get a column");
            uf.IsLoggedIn(email); // Check if user is logged in
            email = email.Trim().ToLower();
            BoardBL currBoard = GetBoardByName(email, boardName);
            List<TaskBL> currList = new List<TaskBL>();
            //check the requested board exist
            if (currBoard != null)
            {
                if (columnOrdinal > 2 || columnOrdinal < 0)
                {
                    log.Error("columnOrdinal not valid input");
                    throw new ArgumentException("columnOrdinal not valid input");
                }
                // Assuming GetTasks returns a list of tasks from the column
                log.Info("Column retrieved successfully");
                currList = currBoard.GetColumn(columnOrdinal);
            }
            else
            {
                log.Error("board/email does not exist");
                throw new ArgumentException("board/email does not exist");
            }
            return currList;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> column's name, unless an error occurs </returns>
        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            log.Info("A user try to get a column name");
            uf.IsLoggedIn(email); // Check if user is logged in
            BoardBL currBoard = GetBoardByName(email, boardName);
            //check the requested board exist
            if (currBoard != null)
            {
                string colName = currBoard.GetColumnName(columnOrdinal);
                if (colName == null)
                {
                    throw new ArgumentException("columnOrdinal doesn't match");
                }
                else
                {
                    return colName;
                }
            }
            else
            {
                log.Error("board/email does not exist");
                throw new ArgumentException("board doesnt exist");
            }

        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        internal void CreateBoard(string email, string name)
        {
            log.Info("A user try to create a board");
            email = email.Trim().ToLower();
            name = name.Trim();
            uf.IsLoggedIn(email); // Check if user is logged in
            // Check if board name already exists
            if (GetBoardByName(email, name) != null)
            {
                log.Error("Board name already exists");
                throw new ArgumentException("Board name already exists");
            }
            // Create a new board and add it to the boards dictionary
            BoardBL newBoard = new BoardBL(email, name, nextBoardID);
            boards.Add(nextBoardID, newBoard);
            nextBoardID++;
            log.Info("Board created successfully");
        }

        /// <summary>
        /// This method delete board with the givin name of the user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        internal void DeleteBoard(string email, string name)
        {
            uf.IsLoggedIn(email); // Check if user is logged in
            log.Info("A user try to delete a board");
            email = email.Trim().ToLower();
            name = name.Trim();
            BoardBL currBoard = GetBoardByName(email, name); // Assuming GetBoardByName method is defined correctly
            //check the requested board to delete is exist
            if (currBoard != null && currBoard.Owner == email)
            {
                BoardDAO boarddao = new BoardDAO(name, nextBoardID, email);
                bc.deleteBoard(boarddao);
                // UserBoardStarusDAO userboard = new UserBoardStarusDAO(currBoard.boardId, email);
                // userboard.deleteBoard(userboard);
                boards.Remove(currBoard.boardId, out var removedBoard);
                log.Info("Board deleted successfully");
            }
            else
            {
                log.Error("Email/Board does not exist");
                throw new ArgumentException("board/email does not exist");
            }
        }

        /// <summary>
        /// This method gets all progress task from all boards of the user
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <returns> List of TaskBL contains all progress tasks, unless an error occurs </returns>
        internal List<TaskBL> GetProgressTask(string email)
        {
            log.Info("A user try to get in progress tasks");
            uf.IsLoggedIn(email); // Check if user is logged in
            email = email.Trim().ToLower();
            List<TaskBL> inProgressTasks = new List<TaskBL>();
            foreach (BoardBL currboard in boards.Values)
            {
                if (currboard.Owner == email || currboard.memebrs.Contains(email))
                {
                    foreach (TaskBL task in currboard.col[1].GetTasks())
                    {
                        if (task.Assign == email)
                        {
                            inProgressTasks.Add(task);
                        }
                    }
                }
            }
            log.Info("In progress tasks retrieved successfully");
            return inProgressTasks;
        }

        /// <summary>
        /// This method gets board with the given name of the user
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns> board, unless an error occurs </returns>
        private BoardBL GetBoardByName(string email, string boardName)
        {
            log.Info("A user try to get a board by name");
            email = email.Trim().ToLower();
            boardName = boardName.Trim();
            //serch the reequested board in all user boards
            foreach (BoardBL currBoard in boards.Values)
            {
                if (currBoard.GetBoardName() == boardName && (currBoard.Owner == email || currBoard.memebrs.Contains(email)))
                {
                    log.Info("Board retrieved successfully");
                    return currBoard;
                }
            }
            return null;
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal String GetBoardName(int boardId)
        {
            log.Info("A user try to get a board name by ID");
            if (boards.TryGetValue(boardId, out BoardBL currBoard))
            {
                log.Info($"Board name for ID {boardId} retrieved successfully");
                return currBoard.GetBoardName();
            }
            else
            {
                log.Error("Board does not found");
                throw new ArgumentException("Board ID not found");
            }
        }

        ///<summary>This method loads all persisted board data - include columns and tasks
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void loadBoards()
        {
            log.Info("Loading all persisted board data");
            this.boards = new Dictionary<int, BoardBL>();
            List<BoardDAO> boardsDao = bc.loadAllBoards();
            foreach (BoardDAO board in boardsDao)
            {
                List<ColumnDAO> colDao = bc.loadCoulmns(board.boardId);
                List<string> members = bc.loadMembers(board.boardId);

                BoardBL boardBL = new BoardBL(board, members);

                if (colDao != null)
                {
                    foreach (ColumnDAO col in colDao)
                    {
                        List<TaskDAO> taskDao = bc.loadTasks(board.boardId, col.ordinal);
                        CollumnBL colBl = new CollumnBL(col, taskDao);
                        boardBL.col[col.ordinal] = colBl;
                    }
                }
                boards.Add(boardBL.boardId, boardBL);
            }
            Setnextid();
        }

        private void Setnextid()
        {
            if (boards.Count > 0)
            {
                this.nextBoardID = boards.Keys.Max() + 1;
            }
            else
            {
                this.nextBoardID = 1;
            }
        }

        ///<summary>This method deletes all persisted board data - include task and columns.
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void deleteAllBoard()
        {
            log.Info("Deleting all persisted board data");
            bc.DeleteAllBoards();
            boards = new Dictionary<int, BoardBL>();
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            log.Info("A user try to assign a task to a user");
            email = email.Trim().ToLower();
            emailAssignee = emailAssignee.Trim().ToLower();
            boardName = boardName.Trim();
            if (email == null || emailAssignee == null || boardName == null)
            {
                throw new ArgumentException("you enter null email");
            }
            // Check if the user is logged in (assuming uf.IsLoggedIn is a valid method)
            uf.IsLoggedIn(email);
            // Find the board by boardName
            BoardBL board = GetBoardByName(email, boardName);
            if (board == null)
            {
                log.Error("Board not found");
                throw new ArgumentException("Board not found");
            }
            // Check if both email and emailAssignee are either the owner or in the members list
            if ((board.Owner == emailAssignee) || board.memebrs.Contains(emailAssignee))
            {
                if (columnOrdinal < 0 || columnOrdinal > 1)
                {
                    throw new ArgumentException("columnOrdinal not valid");
                }
                // Proceed with task assignment
                TaskBL task = board.GetTaskById(columnOrdinal, taskID);
                if (task == null)
                {
                    log.Error("Task not found");
                    throw new ArgumentException("Task not found");
                }

                if (task.Assign == null || task.Assign == email)
                {
                    task.Assign = emailAssignee; // Assuming task has a property Assignee
                    log.Info("Task assigned successfully");
                }
            }
            else
            {
                log.Error("Task already assigned or the user is not in the board");
                throw new UnauthorizedAccessException("you try to change the assign but the user not in the board");
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void transferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            log.Info("A user try to transfer a board ownership");
            currentOwnerEmail = currentOwnerEmail.Trim().ToLower();
            newOwnerEmail = newOwnerEmail.Trim().ToLower();
            uf.IsLoggedIn(currentOwnerEmail);
            BoardBL board = GetBoardByName(currentOwnerEmail, boardName);
            if (board == null)
            {
                log.Error("you are not the board owner / board dont exist");
                throw new ArgumentException("you are not the board owner / board dont exist");
            }
            else
            {
                if (board.memebrs.Contains(newOwnerEmail))
                {
                    board.Owner = newOwnerEmail;
                    log.Info("Board ownership transferred successfully");
                }
                else
                {
                    log.Error("new owner email didnt member in the board");
                    throw new ArgumentException("new owner email didnt member in the board");
                }
            }
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<int> GetUserBoards(String email)
        {
            log.Info("A user try to get all his boards");
            email = email.Trim().ToLower();
            uf.IsLoggedIn(email);
            List<int> returnvalue = new List<int>();

            foreach (var board in boards.Values)
            {
                if (board.Owner == email || board.memebrs.Contains(email))
                {
                    returnvalue.Add(board.boardId);
                }
            }
            log.Info("User boards retrieved successfully");
            return returnvalue;

        }

        internal List<BoardBL> ViewUserBoards(String email)
        {
            log.Info("A user try to get all his boards");
            email = email.Trim().ToLower();
            uf.IsLoggedIn(email);
            List<BoardBL> returnvalue = new List<BoardBL>();

            foreach (var board in boards.Values)
            {
                if (board.Owner == email || board.memebrs.Contains(email))
                {
                    returnvalue.Add(board);
                }
            }
            log.Info("User boards retrieved successfully");
            return returnvalue;

        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void JoinBoard(String email, int boardID)
        {
            log.Info("A user try to join a board");
            email = email.Trim().ToLower();
            uf.IsLoggedIn(email);
            List<int> userboard = GetUserBoards(email);
            if (email == null || userboard.Contains(boardID))
            {
                log.Error("user is member in board already");
                throw new ArgumentException("user is member in board with this name already");
            }
            else
            {
                BoardBL curr = GetBoardByID(boardID);
                if (curr == null)
                {
                    log.Error("the board is null");
                    throw new ArgumentException("there is not a board like this");
                }
                else
                {
                    curr.JoinBoard(email);
                    log.Info("User joined board successfully");
                }
            }
        }

        //get the id and return the board
        private BoardBL GetBoardByID(int boardID)
        {
            if (boards.TryGetValue(boardID, out BoardBL currBoard))
            {
                log.Info("Board retrieved successfully");
                return currBoard;
            }
            else
            {
                log.Error("Board with that id dont exist");
                return null;
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void LeaveBoard(string email, int boardID)
        {
            log.Info("A user try to leave a board");
            email = email.Trim().ToLower();
            uf.IsLoggedIn(email);
            string currboard = GetBoardName(boardID);
            List<int> userboard = GetUserBoards(email);
            if (!userboard.Contains(boardID))
            {
                log.Error("user is not member in board");
                throw new ArgumentException("user is not member in board or he is the owner");
            }
            else
            {
                BoardBL curr = GetBoardByID(boardID);
                if (curr == null || curr.Owner == email)
                {
                    log.Error("the user is the owner of the board");
                    throw new ArgumentException("the user is the owner of the board");
                }
                else
                {
                    curr.LeaveBoard(email);
                    log.Info("User left board successfully");
                }

            }
        }

        internal IList<BoardBL> allboard(string email)
        {
            IList<BoardBL> boards1 = new List<BoardBL>();
            foreach (BoardBL board in boards.Values)
            {
                if(board.Owner.Contains(email) || board.memebrs.Contains(email))
                {

                }
                else
                {
                    boards1.Add(board);
                }
            }
            return boards1;
        }
    }
}
