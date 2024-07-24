#nullable enable
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using Kanban_2024_2024_24.Backend.DataAccessLayer;

namespace Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard
{
    internal class CollumnBL
    {
        internal List<TaskBL> tasks;
        internal int limit;
        readonly int UN_LIMITED=-1;
        internal ColumnDAO dao;

        //constructor
        internal CollumnBL(int limit)
        {
            this.limit = limit;
            this.tasks = new List<TaskBL>();
            this.dao=new ColumnDAO(limit);
            this.dao.persist();
        }

        //constructor
        internal CollumnBL(ColumnDAO col, List<TaskDAO> taskDao)
        {
            this.limit = col.Limit;
            this.tasks = new List<TaskBL>();
            foreach (TaskDAO task in taskDao)
            {
                this.tasks.Add(new TaskBL(task));
            }

        }

        //constructor
        internal CollumnBL()
        {
            this.limit = this.UN_LIMITED;
            this.tasks = new List<TaskBL>();
            this.dao=new ColumnDAO(limit);
            this.dao.persist();
        }

        //return column limit
        internal int GetLimit()
        {
            return limit;
        }

        //set new limit
        internal void SetLimit(int limit)
        {
            if (tasks.Count <= limit || limit == this.UN_LIMITED){
                this.dao.Limit=limit;
                this.limit = limit;
            }
            else
            {
                throw new ArgumentException("the new limit input isnt valid, the collumn is larger than limit wanted");
            }
        }

        //return tasks list
        internal List<TaskBL> GetTasks()
        {
            return tasks;
        }
    }
}
