using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class NewColumnViewModel : NotifiableObject
    {
        public Model.BackendController Controller;
        public Model.UserModel user;
        public Model.BoardModel board;
        public View.BoardView boardView;
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _position;
        public int Position
        {
            get => _position;
            set
            {
                this._position = value;
                RaisePropertyChanged("Position");
            }
        }

        public NewColumnViewModel(Model.BackendController controller, Model.BoardModel board, Model.UserModel user, View.BoardView boardView)
        {
            Controller = controller;
            this.user = user;
            this.board = board;
            this.boardView = boardView;
        }
    }
}
