using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    /// <summary>
    /// presetation's board
    /// </summary>
    public class BoardModel : NotifiableModelObject
    {
        public String Name { get; set; }
        public String AdminEmail { get; set; }
        public int ID;
        public BoardModel(BackendController controller, IntroSE.Kanban.Backend.ServiceLayer.BoardSL board) : base(controller)
        {
            this.Name = board.BoardName;
            this.AdminEmail = board.owner;
            this.ID = board.boardId;
        }
    }
}
