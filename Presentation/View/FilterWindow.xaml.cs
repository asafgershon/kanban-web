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
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        ViewModel.FilterViewModel viewModel;
        public FilterWindow(Model.UserModel user, View.BoardMenu boardMenu, Model.BackendController Controller)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.FilterViewModel(user, boardMenu, Controller);
            this.viewModel = (ViewModel.FilterViewModel)DataContext;
        }


        /// <summary>
        /// join the board by click it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ListViewItem)sender).Content as Model.BoardModel;
            if (item != null)
            {
                Action ConfirmJoinBoard = () =>
                {
                    viewModel.LeaveBoard(viewModel.userModel.Email, item.ID);
                };
                ConfirmWindow confirmWindow = new ConfirmWindow(viewModel.Controller, ConfirmJoinBoard);
                confirmWindow.Show();
            }
        }
    }
}
