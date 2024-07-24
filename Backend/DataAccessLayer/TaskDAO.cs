using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
// using System

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{

    internal class TaskDAO
    {
        internal int taskId { get; private set; }
        private string title;
        internal string Title
        {
            get { return title; }
            set
            {
                if (IsPersisted)
                    tc.UpdateTask(taskId, boardId, TitleColumnName, value);
                title = value;
            }
        }
        private DateTime dueDate;
        internal DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                if (IsPersisted)
                    tc.UpdateTask(taskId, boardId, DueDateColumnName, value);
                dueDate = value;
            }
        }
        internal DateTime time { get; private set; }
        private string description;
        internal string Description
        {
            get { return description; }
            set
            {
                if (IsPersisted)
                    tc.UpdateTask(taskId, boardId, DescriptionColumnName, value);
                description = value;
            }
        }
        private int columnOrdinal;
        internal int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                if (IsPersisted)
                    tc.UpdateTask(taskId, boardId, ColumnsOrdinaleColumnName, value);
                columnOrdinal = value;
            }
        }
        private string? asignee;
        internal string? Asignee
        {
            get { return asignee; }
            set
            {
                if (IsPersisted)
                    tc.UpdateTask(taskId, boardId, AssigneColumnName, value);
                asignee = value;
            }
        }
        internal int boardId { get; private set; }
        internal bool IsPersisted = false;
        internal string TaskIdColumnName = "TaskId";
        internal string TimeColumnName = "Time";
        internal string DueDateColumnName = "DueDate";
        internal string TitleColumnName = "Title";
        internal string DescriptionColumnName = "Description";
        internal string ColumnsOrdinaleColumnName = "ColumnOrdinal";
        internal string AssigneColumnName = "Asignee";
        internal string BoardIdColumnName = "BoardID";
        private taskController tc;

        //constructor
        internal TaskDAO(int taskId, string title, DateTime dueDate, DateTime time, string description, int columnOrdinal, string asignee)
        {
            tc = new taskController();
            this.taskId = taskId;
            this.title = title;
            this.dueDate = dueDate;
            this.time = time;
            this.description = description;
            this.columnOrdinal = columnOrdinal;
            this.asignee = asignee;
            this.boardId = -1;
        }

        //constructor
        internal TaskDAO(int taskId, string title, DateTime dueDate, DateTime time, string description, int columnOrdinal, string asignee, int boardId)
        : this(taskId, title, dueDate, time, description, columnOrdinal, asignee)
        {
            this.boardId = boardId;
        }

        internal void persist()
        {
            if (this.IsPersisted)
                throw new System.Exception("the task already inserted");
        }

        //check that this input dont insert into the database already and pass it forward
        internal void persist(int boardId)
        {
            if (this.IsPersisted)
                throw new System.Exception("the task already inserted");
            this.boardId = boardId;
            tc.addTask(this);
            IsPersisted = true;
        }

    }
}