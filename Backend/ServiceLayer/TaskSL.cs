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
        public int TaskId { get; private set; }
        public DateTime Time { get; private set; }
        public DateTime DueDate { get; private set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ColumnOrdinal { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? assign { get; set; }

        //constructor
        public TaskSL(int TaskId, DateTime time, DateTime dueDate, string title, string description, int columnOrdinal)
        {
            this.TaskId = TaskId;
            this.Time = time;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.ColumnOrdinal = columnOrdinal;
            this.assign = null;
        }
    }
}