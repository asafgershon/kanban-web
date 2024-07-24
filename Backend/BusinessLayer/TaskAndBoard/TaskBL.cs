using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using Kanban_2024_2024_24.Backend.DataAccessLayer;

namespace Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard
{
    internal class TaskBL
    {
        internal int TaskId { get; private set; }
        internal string Title { get; private set; }
        internal string Description { get; private set; }
        internal DateTime DueDate { get; private set; }
        internal DateTime Time { get; private set; }
        internal int ColumnOrdinal { get; private set; }
        private string assign;
        internal string Assign
        {
            get { return assign; }
            set
            {
                this.dao.Asignee = value;
                this.assign = value;
            }
        }
        internal TaskDAO dao;

        /// <summary>
        /// This method create a new task.
        /// </summary>
        /// <param name="title">Title of the new task</param>
        ///  /// <param name="Taskid">Taskid of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        internal TaskBL(string title, int Taskid, string description, DateTime dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Time = DateTime.Now;
            TaskId = Taskid;
            ColumnOrdinal = 0;
            assign = null;
            dao = new TaskDAO(Taskid, title, dueDate, this.Time, description, 0, null);

        }

        //build new DAO for update in the db
        internal TaskBL(TaskDAO dao)
        {
            Title = dao.Title;
            Description = dao.Description;
            DueDate = dao.DueDate;
            Time = dao.time;
            TaskId = dao.taskId;
            ColumnOrdinal = dao.ColumnOrdinal;
            assign = dao.Asignee;

        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <returns>update the ColumnOrdinal</returns>
        internal void AdvanceTask()
        {
            if (ColumnOrdinal >= 2) // assuming 2 is the 'done' column
            {
                throw new InvalidOperationException("Task is already in the done column");
            }
            ColumnOrdinal++;
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="description">New description for the task</param>
        internal void UpdateTaskDescription(string description)
        {
            Description = description;
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="title">New title for the task</param>
        internal void UpdateTaskTitle(string title)
        {
            if (title.Length > 50)
            {
                throw new ArgumentException("Title exceeds maximum length of 50 characters");
            }
            Title = title;
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="dueDate">The new due date of the column</param>
        internal void UpdateTaskDueDate(DateTime dueDate)
        {
            DueDate = dueDate;
        }

        //return title of the task
        internal String Getname()
        {
            return Title;
        }


    }
}