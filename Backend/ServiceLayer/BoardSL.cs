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
    public class BoardSL
    {
        public string? BoardName { get; private set; }
        public CollumnSL[]? col { get; private set; }
        public int boardId { get; }
        public string? owner { get; private set; }
        public List<string> memebrs { get; private set; }

        //constructor
        public BoardSL(string BoardName, string owner, int boardid, List<string> members)
        {
            this.BoardName = BoardName;
            this.col = new CollumnSL[3];
            this.boardId = boardid;
            this.col[0] = new CollumnSL(); // Backlog
            this.col[1] = new CollumnSL(); // InProgress
            this.col[2] = new CollumnSL(); // Done
            this.owner = owner;
            this.memebrs = members;
        }

        public BoardSL(string BoardName, CollumnSL backlog, CollumnSL inProgress, CollumnSL done, string owner, int boardid)
        {
            this.BoardName = BoardName;
            this.col = new CollumnSL[3];
            this.boardId = boardid;
            this.col[0] = backlog; // Backlog
            this.col[1] = inProgress; // InProgress
            this.col[2] = done; // Done
            this.owner = owner;
            this.memebrs = new List<string>();
        }
    }
}
