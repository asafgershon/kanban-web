using System;
using System.Text.Json;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private BoardFacede bf;

        //constructor
        internal TaskService(BoardFacede bf)
        {
            // Initialize BoardFacade instance
            this.bf = bf;
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A JSON string representing the response containing task details or error message.</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                // Add task using BoardFacade
                bf.AddTask(email, boardName, title, description, dueDate);
                Response response = new Response();
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
            catch (Exception ex)
            {
                // Handle exception and create error response
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A JSON string representing the response containing task details or error message.</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                // Advance task using BoardFacade
                bf.AdvanceTask(email, boardName, columnOrdinal, taskId);
                Response response = new Response();
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
            catch (Exception ex)
            {
                // Handle exception and create error response
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response); // Serialize response to JSON
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
        /// <returns>A JSON string representing the response containing task details or error message.</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                // Update task description using BoardFacade
                bf.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                Response response = new Response();
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
            catch (Exception ex)
            {
                // Handle exception and create error response
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response); // Serialize response to JSON
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
        /// <returns>A JSON string representing the response containing task details or error message.</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                // Update task title using BoardFacade
                bf.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                Response response = new Response();
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
            catch (Exception ex)
            {
                // Handle exception and create error response
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response); // Serialize response to JSON
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
        /// <returns>A JSON string representing the response containing task details or error message.</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                // Update task due date using BoardFacade
                bf.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                Response response = new Response();
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
            catch (Exception ex)
            {
                // Handle exception and create error response
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                bf.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                Response response = new Response();
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
            catch (Exception ex)
            {
                // Handle exception and create error response
                Response response = new Response(ex.Message);
                return JsonSerializer.Serialize(response); // Serialize response to JSON
            }
        }
    }
}

