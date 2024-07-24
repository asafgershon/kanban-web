using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for NewColumn.xaml
    /// </summary>
    public partial class NewColumn : Window
    {
        private ViewModel.NewColumnViewModel viewModel;
        public NewColumn(Model.BackendController controller, Model.BoardModel board, Model.UserModel user, View.BoardView boardView)
        {
            this.DataContext = new ViewModel.NewColumnViewModel(controller, board, user, boardView);
            this.viewModel = (ViewModel.NewColumnViewModel)DataContext;
            InitializeComponent();
        }
    }
}
