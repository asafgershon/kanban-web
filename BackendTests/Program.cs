using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using Kanban_2024_2024_24.Backend.DataAccessLayer;

namespace UI
{
    public class Program
    {

        static void Main(string[] args)
        {
            UserTest tests = new UserTest();
            tests.RunTest();

            //BoardTests boardT=new BoardTests();
            //boardT.RunTest();

            //TaskTest taskTests = new TaskTest();
            //taskTests.RunTest();
        }
    }
}
