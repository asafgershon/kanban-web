#nullable enable
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Security.Policy;
using System.Reflection.Metadata;
using System.Text.Json;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using Kanban_2024_2024_24.Backend.DataAccessLayer;

namespace Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard
{
    internal class BoardBL
    {

        internal string BoardName;
        internal CollumnBL[] col;
        internal int boardId;
        private int nextTaskId;
        private string owner;
        internal string Owner
        {
            get { return owner; }
            set
            {
                dao.Owner = value;
                owner = value;
            }
        }
        private BoardDAO dao;
        internal List<string> memebrs;
        private const int BACKLOG_INDEX = 0;
        private const int INPROG_INDEX = 1;
        private const int DONE_INDEX = 2;
        private const int SUM_COL = 3;

        /// <summary>
        /// This method creates a board in the system
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <param boardId="boardId">the id of the new board</param>
        internal BoardBL(string email, string BoardName, int boardId)
        {
            this.BoardName = BoardName;
            this.col = new CollumnBL[SUM_COL];
            this.col[BACKLOG_INDEX] = new CollumnBL(); // Backlog
            this.col[BACKLOG_INDEX].dao.persist(this.boardId, BACKLOG_INDEX);
            this.col[INPROG_INDEX] = new CollumnBL(); // InProgress
            this.col[INPROG_INDEX].dao.persist(this.boardId, INPROG_INDEX);
            this.col[DONE_INDEX] = new CollumnBL(); // Done
            this.col[DONE_INDEX].dao.persist(this.boardId, DONE_INDEX);
            this.boardId = boardId;
            nextTaskId = 0;
            this.owner = email;
            this.memebrs = new List<string>();
            dao = new BoardDAO(BoardName, boardId, email);
            dao.persist();
        }
        //constructor with DAO for change in the db
        internal BoardBL(BoardDAO dao, List<string> members)
        {
            this.dao = dao;
            this.BoardName = dao.boardName;
            this.col = new CollumnBL[SUM_COL];
            this.boardId = dao.boardId;
            //change acording to the dao data
            this.col[BACKLOG_INDEX] = null;
            this.col[INPROG_INDEX] = null;
            this.col[DONE_INDEX] = null;
            nextTaskId = 0;
            this.owner = dao.Owner;
            this.memebrs = members;
        }

        /// <summary>
        /// This method verify that the coulmn ordinal input is valid.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="title">New title for the task</param>
        private void colOrdinalValidation(int columnOrdinal)
        {
            if (GetColumnName(columnOrdinal) == null)
                throw new ArgumentException("the column ordinal input isnt valid, supposed to be 0/1/2");
        }

        internal string GetColumnName(int columnOrdinal)
        {
            switch (columnOrdinal)
            {
                case 0:
                    return "backlog";
                case 1:
                    return "in progress";
                case 2:
                    return "done";
                default:
                    return null;
            }

        }

        /// <summary>
        /// This method sets the limit of the board
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The requested new limit</param>
        internal void setLimit(int columnOrdinal, int limit)
        {
            colOrdinalValidation(columnOrdinal);
            col[columnOrdinal].SetLimit(limit);
        }

        /// <summary>
        /// This method return the limit of this board.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>int column current limit.</returns>
        internal int GetColumnLimit(int columnOrdinal)
        {
            return col[columnOrdinal].GetLimit();
        }

        /// <summary>
        /// This method return the name of this board.
        /// </summary>
        /// <returns>string board current name.</returns>
        internal string? GetBoardName()
        {
            return BoardName;
        }

        /// <summary>
        /// This method sets the name of the board
        /// </summary>
        /// <param name="boardName">The name of the board</param>
        internal void SetBoardName(string BoardName)
        {
            this.BoardName = BoardName;
        }

        /// <summary>
        /// This method create a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A TaskBL that create or error message.</returns>
        internal TaskBL createTask(string email, string title, string description, DateTime dueDate)
        {
            if (col[BACKLOG_INDEX].GetLimit() == -1 || col[BACKLOG_INDEX].GetLimit() > col[BACKLOG_INDEX].GetTasks().Count)
            {
                if (DateTime.Compare(dueDate, DateTime.Now) > 0)
                {
                    // Implement logic to add a task to the board
                    TaskBL newTask = new TaskBL(title, nextTaskId, description, dueDate);
                    nextTaskId++;
                    return newTask;
                }
                else
                {
                    throw new ArgumentException("invalid due date");
                }
            }
            else
            {
                throw new ArgumentException("you reach the limit amount of task, please update task limit");
            }
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param TaskBL="newTask">the task we want to add.</param>
        /// <returns>A void</returns>
        internal void AddTask(TaskBL newTask)
        {
            this.dao.addTask(newTask.dao);
            col[BACKLOG_INDEX].GetTasks().Add(newTask);
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A TaskBL that added or error message.</returns>
        internal void AdvanceTask(int columnOrdinal, int taskId, string email)
        {
            if (col[columnOrdinal].GetLimit() == -1 || col[columnOrdinal + 1].GetLimit() > col[columnOrdinal + 1].GetTasks().Count)
            {
                // Implement logic to advance a task from one column to the next
                TaskBL task = GetTaskById(columnOrdinal, taskId);
                if (task != null && (task.Assign == email || task.Assign == null))
                {
                    task.dao.ColumnOrdinal = columnOrdinal + 1;
                    task.AdvanceTask();
                    col[columnOrdinal].GetTasks().Remove(task);
                    col[columnOrdinal + 1].GetTasks().Add(task);
                }
                else
                {
                    throw new ArgumentException("task does not exist");
                }
            }
            else
            {
                throw new ArgumentException("you reach the limit amount of task for this collumns, please update task limit");
            }

        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A TaskBL that Update or error message.</returns>
        internal void UpdateTaskDescription(int columnOrdinal, int taskId, string description, string email)
        {
            colOrdinalValidation(columnOrdinal);
            // Implement logic to update task description
            TaskBL task = GetTaskById(columnOrdinal, taskId);
            if (task != null && (task.Assign == email || task.Assign == null))
            {
                task.dao.Description = description;
                task.UpdateTaskDescription(description);
            }
            else
            {
                throw new ArgumentException("task does not exist");
            }
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A TaskBL that Update or error message.</returns>
        internal void UpdateTaskTitle(int columnOrdinal, int taskId, string title, string email)
        {
            colOrdinalValidation(columnOrdinal);
            // Implement logic to update task title
            TaskBL task = GetTaskById(columnOrdinal, taskId);
            if (task != null && (task.Assign == email || task.Assign == null))
            {
                task.dao.Title = title;
                task.UpdateTaskTitle(title);
            }
            else
                throw new ArgumentException("task does not exist");
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A TaskBL that Update or error message.</returns>
        internal void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate, string email)
        {
            colOrdinalValidation(columnOrdinal);
            // Implement logic to update task due date
            TaskBL task = GetTaskById(columnOrdinal, taskId);
            if (task != null && (task.Assign == email || task.Assign == null))
            {
                if (DateTime.Compare(dueDate, task.Time) > 0)
                {
                    task.dao.DueDate = dueDate;
                    task.UpdateTaskDueDate(dueDate);
                }
            }
            else
                throw new ArgumentException("task does not exist");
        }

        /// <summary>
        /// This method return the column requested
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A TaskBL list with all task related to requested colum in the board</returns>
        internal List<TaskBL> GetColumn(int columnOrdinal)
        {
            // Implement logic to retrieve tasks from a specific column
            List<TaskBL> columns = col[columnOrdinal].GetTasks();
            return columns;
        }

        /// <summary>
        /// This method return the Task
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A TaskBL match for this data</returns>
        internal TaskBL GetTaskById(int columnOrdinal, int taskId)
        {
            foreach (var task in col[columnOrdinal].GetTasks()) // Assuming 'GetAllTasks' returns all tasks in a board
            {
                if (task.TaskId == taskId)
                {
                    return task;
                }
            }
            return null;
        }

        //add new email to the member list
        internal void JoinBoard(string email)
        {
            this.dao.JoinBoard(this.boardId, email);
            memebrs.Add(email);
        }

        //remove an user from the member list
        internal void LeaveBoard(string email)
        {
            foreach (string user in memebrs)
            {
                if (user == email)
                {
                    this.dao.LeaveBoard(boardId,email);
                    memebrs.Remove(email);
                    for (int i = 0; i < SUM_COL; i++)
                    {
                        foreach (TaskBL task in col[i].GetTasks())
                        {
                            if (task.Assign == email)
                            {
                                task.Assign = null;
                            }
                        }
                    }
                    return;
                }
            }
            throw new ArgumentException("user is not a member in this board");
        }

    }
}