using System;
using System.Collections.Generic;

namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{

    internal class ColumnDAO
    {
        internal int boardId { get; set; }
        private int limit;
        internal int Limit
        {
            get { return limit; }
            set
            {
                if (IsPersisted)
                {
                    cc.SetLimit(this.TaskLimitColumnName, this.boardId, this.ordinal, value);
                }
                this.limit = value;
            }
        }
        internal int ordinal { get; set; }
        internal bool IsPersisted = false;

        internal string TaskLimitColumnName = "TaskLimit";
        internal string BoardIdColumnName = "BoardId";
        internal string ordinalColumnName = "Ordinal";
        private ColumnController cc;
        internal const int NOT_ASSIGNED_VALUE=-1;

        //constructor
        public ColumnDAO(int boardId, int limit, int ordinal)
        {

            this.boardId = boardId;
            this.Limit = limit;
            this.ordinal = ordinal;
            this.cc = new ColumnController();

        }
        public ColumnDAO(int limit)
        {
            this.boardId = NOT_ASSIGNED_VALUE;
            this.Limit = limit;
            this.ordinal = NOT_ASSIGNED_VALUE;
            this.cc = new ColumnController();

        }

        //check that this input dont insert into the database already and pass it forward
        public void persist()
        {
            return;
        }

        internal void persist(int boardId, int columnOrdinal)
        {
            if (this.IsPersisted)
                throw new System.Exception("this column is already inserted");
            this.boardId = boardId;

        }
    }
}