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
    /// Interaction logic for Task2.xaml
    /// </summary>
    public partial class Task2 : Window
    {
        private ViewModel.TaskViewModel viewModel;
        public Task2(Model.BackendController controller, Model.TaskModel task, Model.BoardModel board, Model.UserModel user, IList<string> members, IList<Model.ColumnModel> columns)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.TaskViewModel(controller, task, board, user, members, columns);
            this.viewModel = (ViewModel.TaskViewModel)DataContext;
        }

        /// <summary>
        /// return back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// save the new assignee button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsignee_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskAssignee();
        }
    }
}
