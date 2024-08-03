#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskSL
    {
        public int TaskId { get; set; }
        public DateTime Time { get; set; }
        public DateTime DueDate { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int ColumnOrdinal { get; set; }

        public string? assign { get; set; }

        //constructor
        public TaskSL(int TaskId, DateTime Time, DateTime DueDate, string Title, string Description, int ColumnOrdinal)
        {
            this.TaskId = TaskId;
            this.Time = Time;
            this.DueDate = DueDate;
            this.Title = Title;
            this.Description = Description;
            this.ColumnOrdinal = ColumnOrdinal;
            this.assign = null;
        }
        public TaskSL() { }
    }
}