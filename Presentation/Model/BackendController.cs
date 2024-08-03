using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// Connects the Presentation layer with the Bussines layer
    /// </summary>
    public class BackendController
    {
        private GradingService Service { get; set; }
        private static BackendController b;
        public static BackendController getInstante(GradingService service)
        {
            if (b == null)
                b = new BackendController(service, true);
            return b;
        }
        private BackendController(GradingService service, Boolean b)
        {
            this.Service = service;
        }
        public static BackendController getInstante()
        {
            if (b == null)
                b = new BackendController(new GradingService(), true);
            return b;
            //Service.LoadData();
        }
        /// <summary>
        /// logs the user in
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="password">user's password</param>
        /// <returns></returns>
        public UserModel Login(string email, string password)
        {
            Response user = JsonSerializer.Deserialize<Response>(Service.Login(email, password));
            if (user.isError)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email);
        }
        /// <summary>
        /// logs the user out
        /// </summary>
        /// <param name="email">user's email</param>
        public void Logout(string email)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.Logout(email));
            if (res.isError)
                throw new Exception("logout failed");
        }
        /// <summary>
        /// register new user
        /// </summary>
        /// <param name="email">new user's email</param>
        /// <param name="password">new user's password</param>
        internal UserModel Register(string email, string password)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.Register(email, password));
            if (res.isError)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this, email);
        }

        /// <summary>
        /// add new board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardname">new board's name</param>
        public void AddBoard(string email, string boardname)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.CreateBoard(email, boardname));
            if (res.isError)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// remove board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="boardname">name of the board to remove</param>
        public void RemoveBoard(string email, string boardname)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.DeleteBoard(email, boardname));
            if (res.isError)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// add new task
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">name of the board of the new task</param>
        /// <param name="title">new task's title</param>
        /// <param name="description">new task's description</param>
        /// <param name="dueDate">new task's due date</param>
        /// <returns></returns>
        public Model.TaskModel AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            TaskSL res = (Service.AddTask(userEmail, boardName, title, description, dueDate));
            if (res == null)
            {
                throw new Exception("you enter invalid input");
            }
            else
            {
                return new Model.TaskModel(res.Title, res.Description, res.DueDate, res.Time, res.assign, res.TaskId, res.ColumnOrdinal, this, userEmail);
            }
        }

        /// <summary>
        /// get all the user's boards' names
        /// </summary>
        /// <param name="email">user's email</param>
        /// <returns></returns>
        public string GetBoardNames(string email)
        {
            return Service.GetUserBoards(email);
        }

        /// <summary>
        /// get the user's boards
        /// </summary>
        /// <param name="user">user's email</param>
        /// <returns></returns>
        public IList<BoardModel> GetUserBoards(UserModel user)
        {
            IList<BoardSL> boards = Service.ViewUserBoards(user.Email);
            IList<BoardModel> boardModels = new List<BoardModel>();
            foreach (IntroSE.Kanban.Backend.ServiceLayer.BoardSL board in boards)
            {
                boardModels.Add(new BoardModel(this, board));
            }
            return boardModels;
        }

        /// <summary>
        /// change column's position
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">column's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="shiftSize">new column's position</param>

        public void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.TransferOwnership(creatorEmail, userEmail, boardName));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// get board by name
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <returns></returns>
        public IList<ColumnModel> GetBoard(string userEmail, string boardName)
        {
            List<ColumnModel> columnModels = new List<ColumnModel>();
            for (int i = 0; i < 3; i++)
            {
                string jsonData = Service.GetColumn(userEmail, boardName, i);
                Response response = JsonSerializer.Deserialize<Response>(jsonData);

                List<TaskSL> columns = new List<TaskSL>();
                if (response?.ReturnValue is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                {
                    columns = JsonSerializer.Deserialize<List<TaskSL>>(jsonElement.GetRawText()) ?? new List<TaskSL>();
                }
                string colname = Service.GetColumnName(userEmail, boardName, i);
                colname = colname.Trim('{', '}');
                string[] parts = colname.Split(':');
                string result = parts.Length > 1 ? parts[1].Trim('\"') : string.Empty;
                ColumnModel columnModel = new ColumnModel(result, i, new List<TaskModel>(), this);
                columnModels.Add(columnModel);
                foreach (IntroSE.Kanban.Backend.ServiceLayer.TaskSL t in columns)
                {
                    TaskModel taskModel = new TaskModel(t.Title, t.Description, t.DueDate, t.Time, t.assign, t.TaskId, i, this, userEmail);
                    columnModel.TaskList.Add(taskModel);
                }
            }
            return columnModels;
        }

        public IList<BoardModel> GetAllBoards(string email)
        {
            IList<IntroSE.Kanban.Backend.ServiceLayer.BoardSL> res = Service.GetAllBoards(email);
            if (res.Count == 0)
            {
                throw new Exception("no board to join");
            }
            else
            {
                List<BoardModel> boardModels = new List<BoardModel>();
                foreach (IntroSE.Kanban.Backend.ServiceLayer.BoardSL board in res)
                {
                    boardModels.Add(new BoardModel(this, board));
                }
                return boardModels;
            }
        }

        public IList<BoardModel> GetAllBoards()
        {
            IList<IntroSE.Kanban.Backend.ServiceLayer.BoardSL> res = Service.GetAllBoards();
            if (res.Count == 0)
            {
                throw new Exception("no board to join");
            }
            else
            {
                List<BoardModel> boardModels = new List<BoardModel>();
                foreach (IntroSE.Kanban.Backend.ServiceLayer.BoardSL board in res)
                {
                    boardModels.Add(new BoardModel(this, board));
                }
                return boardModels;
            }
        }

        /// <summary>
        /// get the board's members' list
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="creatorEmail">board's creator's email</param>
        /// <returns></returns>
        public IList<string> GetBoardMembers(string email, string boardName)
        {
            return Service.GetBoardMembers(email, boardName);
        }

        /// <summary>
        /// change the task's title
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="title">new title</param>
        public void UpdateTaskTitle(string userEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.UpdateTaskTitle(userEmail, boardName, columnOrdinal, taskId, title));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// change the task's description
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="description">new description</param>
        public void UpdateTaskDescription(string userEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.UpdateTaskDescription(userEmail, boardName, columnOrdinal, taskId, description));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// change the task's due date
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="dueDate">new due date</param>
        public void UpdateTaskDueDate(string userEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.UpdateTaskDueDate(userEmail, boardName, columnOrdinal, taskId, dueDate));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// change the task's assignee
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">task's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="taskId">task's ID</param>
        /// <param name="assigneeEmail">new assignee's email</param>
        public void UpdateTaskAssignee(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string assigneeEmail)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.AssignTask(userEmail, boardName, columnOrdinal, taskId, assigneeEmail));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// join board from diffrent user
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        public void JoinBoard(string userEmail, int boardid)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.JoinBoard(userEmail, boardid));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// join board from diffrent user
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        public void LeaveBoard(string userEmail, int boardid)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.LeaveBoard(userEmail, boardid));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        public void loadData()
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.LoadData());
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// move the task to the next column
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">task position</param>
        /// <param name="taskId">task's id</param>
        public void AdvanceTask(string userEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.AdvanceTask(userEmail, boardName, columnOrdinal, taskId));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// give the column task's limit
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">board's creator email</param>
        /// <param name="boardName">board's name</param>
        /// <param name="columnOrdinal">column's position</param>
        /// <param name="limit">new limit</param>
        public void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            Response res = JsonSerializer.Deserialize<Response>(Service.LimitColumn(userEmail, boardName, columnOrdinal, limit));
            if (res.isError)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// get all the tasks in the boards that are not in backlog or done columns
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <returns></returns>
        public IList<Model.TaskModel> InProgressTasks(string userEmail)
        {
            List<TaskSL> res = (Service.InProgressTasks(userEmail));
            List<TaskModel> inProgressTasks = new List<TaskModel>();
            foreach (var t in res)
            {
                inProgressTasks.Add(new TaskModel(t.Title, t.Description, t.DueDate, t.Time, t.assign, t.TaskId, t.ColumnOrdinal, this, userEmail));
            }
            return inProgressTasks;
        }
    }
}
