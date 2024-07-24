#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class CollumnSL
    {
        public List<TaskSL>? Tasks { get; private set; }
        public int? Limit { get; set; }

        //constructor
        public CollumnSL(int limit)
        {
            this.Limit = limit;
            this.Tasks = new List<TaskSL>();
        }

        //empty constructor - set limit to (-1)
        public CollumnSL()
        {
            this.Limit = -1;
            this.Tasks = new List<TaskSL>();
        }
    }
}