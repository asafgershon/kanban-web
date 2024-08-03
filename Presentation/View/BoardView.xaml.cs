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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        public ViewModel.BoardViewModel viewModel;
        

        public BoardView(Model.UserModel u, Model.BoardModel boardModel, IList<Model.ColumnModel> columnModels, Model.BackendController controller)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.BoardViewModel(controller, u, boardModel, columnModels);
            this.viewModel = (ViewModel.BoardViewModel)DataContext;
        }

        /// <summary>
        /// add new task button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_NewTask(object sender, RoutedEventArgs e)
        {
            foreach(Model.ColumnModel c in viewModel.columnModels)
            {
                if (c.Position == 0)
                {
                    NewTask newTask = new NewTask(viewModel.Controller, viewModel.boardModel, viewModel.userModel, this, c);
                    newTask.Show();
                }
            }
        }
        
        /// <summary>
        /// preperations when the window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Model.ColumnModel[] columnArray = new Model.ColumnModel[viewModel.columnModels.Count];
            foreach (Model.ColumnModel c in viewModel.columnModels)
            {
                columnArray[c.Position] = c;
            }
            for (int i = 0; i < columnArray.Length; i++)
            {
                ColumnCB.Items.Add(columnArray[i].Name);
            }
            
        }

        /// <summary>
        /// display the board's task button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_Click(object sender, RoutedEventArgs e)
        {
            if (ColumnCB.Text != null)
            {
                foreach (Model.ColumnModel c in viewModel.columnModels)
                {
                    if (c.Name == ColumnCB.Text)
                    {
                        viewModel.SelectedColumn = c.TaskList;
                        break;
                    }
                }
            }
        }

        private void RemoveColumn_Click()
        {
            //
        }

        /// <summary>
        /// return to the board's menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            BoardMenu boardMenu = new BoardMenu(viewModel.userModel, viewModel.Controller);
            boardMenu.Show();
            this.Close();
        }

        /// <summary>
        /// add new column button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_assigntask(object sender, RoutedEventArgs e)
        {
            //NewColumn newColumn = new NewColumn(viewModel.Controller, viewModel.boardModel, viewModel.userModel, this);
            //newColumn.Show();
        }

        /// <summary>
        /// edit task by click it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((ListViewItem)sender).Content as Model.TaskModel;
            if (item != null)
            {
                Task task = new Task(viewModel.Controller, item, viewModel.boardModel, viewModel.userModel, viewModel.BoardMembers, viewModel.columnModels);
                task.Show();
            }
        }



        /// <summary>
        /// limit cloumn button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Limit_Click(object sender, RoutedEventArgs e)
        {
            LimitColumnWindow limitWindow = new LimitColumnWindow(viewModel.GetColumnByName(ColumnCB.Text), viewModel.userModel, viewModel.boardModel, viewModel.Controller);
            limitWindow.Show();
        }

        //private void Limit1_Click(object sender, RoutedEventArgs e)
        //{
          //  Task2 task = new Task2(viewModel.Controller, item, viewModel.boardModel, viewModel.userModel, viewModel.BoardMembers, viewModel.columnModels);
            //task.Show();
        //}

        public void RefreshBoardView()
        {
            // Clear and reinitialize components, or reload data
            InitializeComponent();
            this.viewModel = (ViewModel.BoardViewModel)DataContext;
        }
    }
}
