using System;
using System.Collections.Generic;


namespace Kanban_2024_2024_24.Backend.DataAccessLayer
{

    internal class BoardDAO
    { 
        internal string boardName { get; private set; }
        internal int boardId { get; private set; }
        private string owner;
        internal string Owner { 
            get{return owner;} 
            set{ 
                if(IsPersisted)
                    bc.UpdateBoard(boardId,ownerColumnName,value); 
                owner=value;} }
        internal bool IsPersisted=false; 

        internal string boardNameColumnName = "BoardName";
        internal string ownerColumnName = "Owner";
        internal string boardIdColumnName = "BoardId";

        private boardController bc;

        //constructor
        internal BoardDAO(string name, int id,string owner)
        {
            this.boardName = name;
            this.boardId = id;
            this.Owner=owner;
            this.bc=new boardController();
        }

        //check that this input dont insert into the database already and pass it forward
        internal void persist(){
            if (this.IsPersisted)
            {
                throw new System.Exception("the user already in the saved");
            }
            bc.AddBoard(this);
            IsPersisted =true;
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param TaskDAO="taskDao">the DAO of the task</param>
        /// <returns>A void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void addTask(TaskDAO taskDao){
            taskDao.persist(this.boardId);
        }

        internal void JoinBoard(int boardId, string email){
            bc.Joinboard(boardId,email);
        }

        internal void LeaveBoard(int boardId, string email){
            bc.LeaveBoard(boardId,email);
        }
    }
}