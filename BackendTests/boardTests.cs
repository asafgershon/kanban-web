using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using log4net;
using System.Text.Json;
using log4net.Config;
using Microsoft.VisualBasic;
namespace UI
{
    public class BoardTests
    {
        public UserService us;
        public boardService bs;
        public TaskService ts;
        public Response res;
        public ServiceFactory sf;

        public BoardTests()
        {
            sf = new ServiceFactory();
            us = sf.GetUser();
            bs = sf.GetBoard();
            ts = sf.GetTask();
            this.res = new Response();
        }

        public void SetUp()
        {
            ServiceFactory sf = new ServiceFactory();
            us = sf.GetUser();
            bs = sf.GetBoard();
            ts = sf.GetTask();
            this.res = new Response();
        }

        public void RunTest()
        {
            sf.deleteData();
            Console.WriteLine("check create board:");
            Console.WriteLine();
            if (TestCreateboard())
            {
                Console.WriteLine("pass all create board test");
            }
            sf.deleteData();

            Console.WriteLine();
            Console.WriteLine("check delete board:");

            Console.WriteLine();
            if (TestDeleteboard())
            {
                Console.WriteLine("pass all delete board test");
            }

            sf.deleteData();
            Console.WriteLine();
            Console.WriteLine("check limit column:");

            Console.WriteLine();
            if (TestLimitCol())
            {
                Console.WriteLine("pass all limit column test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check in progress tasks:");

            Console.WriteLine();
            if (TestInProg())
            {
                Console.WriteLine("pass all in progress tasks test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check get coulmn name:");

            Console.WriteLine();
            if (TestGetColName())
            {
                Console.WriteLine("pass all get coulmn name");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check get coulmn");
            Console.WriteLine();
            if (TestGetCol())
            {
                Console.WriteLine("pass all get coulmn");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check get name");
            Console.WriteLine();
            if (TestGetName())
            {
                Console.WriteLine("pass all get name");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check transfer owner ship");
            Console.WriteLine();
            if (TestTransferOwnerShip())
            {
                Console.WriteLine("pass all transfer owner test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check user board");
            Console.WriteLine();
            if (TestUserBoard())
            {
                Console.WriteLine("pass all user board test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check join board");
            Console.WriteLine();
            if (TestJoinBoard())
            {
                Console.WriteLine("pass all join board");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check Leave board");
            Console.WriteLine();
            if (TestLeaveBoard())
            {
                Console.WriteLine("pass all Leave board");
            }
            sf.deleteData();
            Console.WriteLine();

        }

        public bool TestCreateboard()
        {
            // SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            bool test = true;

            us.Register("amit", "Adi1234");
            us.Register("amit1", "Adi1234");
            us.Logout("amit1");
            // bs.CreateBoard("adi2", "testboard");

            // Check create board for a user logged in with no boards
            Response response = JsonSerializer.Deserialize<Response>(bs.CreateBoard("amit", "testboard"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check create board for a user logged in with existing boards
            response = JsonSerializer.Deserialize<Response>(bs.CreateBoard("amit", "testboard1"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check create board for a user logged out
            response = JsonSerializer.Deserialize<Response>(bs.CreateBoard("amit1", "testboard2"));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check logout -> login and create new board
            us.Login("amit1", "Adi1234");
            response = JsonSerializer.Deserialize<Response>(bs.CreateBoard("amit1", "testboard21"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check create new board with existing board name
            response = JsonSerializer.Deserialize<Response>(bs.CreateBoard("amit1", "testboard21"));
            if (!response.Equals(boardExist))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestDeleteboard()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response emailOrBoardNotExist = new Response("board/email does not exist");
            bool test = true;

            us.Register("adi", "Adi1234");
            us.Register("adi1", "Adi1234");
            us.Register("adi3", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            bs.CreateBoard("adi1", "testboard0");
            bs.CreateBoard("adi1", "testboard1");
            bs.CreateBoard("adi1", "testboard2");
            bs.CreateBoard("adi1", "testboard21");

            // Check delete board for a user logged in with only needed board
            Response response = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("adi", "testboard"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check delete board for a user logged in with existing board and other board
            response = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("adi1", "testboard0"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check delete board for a user logged in without neede board
            response = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("adi1", "testboard3"));
            if (!response.Equals(emailOrBoardNotExist))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check delete board for a user logged out
            us.Logout("adi1");
            response = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("adi1", "testboard2"));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check logout -> login and delete existing board
            us.Login("adi1", "Adi1234");
            response = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("adi1", "testboard21"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check delete board for user with no boards
            response = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("adi3", "testboard21"));
            if (!response.Equals(emailOrBoardNotExist))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestLimitCol()
        {
            SetUp();
            //TaskService ts = new TaskService(us.getUserFacade(), bs.getBoardFacade());
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response emailOrBoardNotExist = new Response("board/email does not exist");
            Response limitNotValid = new Response("the new limit input isnt valid, the collumn is larger than limit wanted");
            Response colOrdinalNotValid = new Response("the column ordinal input isnt valid, supposed to be 0/1/2");
            bool test = true;

            us.Register("adi", "Adi1234");
            us.Register("adi1", "Adi1234");
            us.Register("adi3", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            bs.CreateBoard("adi1", "testboard");
            bs.CreateBoard("adi1", "testboard1");
            bs.CreateBoard("adi1", "testboard2");
            DateTime dt = new DateTime(2024, 6, 18);

            // Check limit colum backlog for a user logged in with only one board and no prev limit
            Response response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 0, 2));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum backlog for a user logged in with more task than limit requested
            us.Register("Adi2", "Asaf1234");
            bs.CreateBoard("Adi2", "testboard1");
            ts.AddTask("adi2", "testboard1", "test", "a task", dt);
            ts.AddTask("adi2", "testboard1", "test", "a task", dt);
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi2", "testboard1", 0, 1));
            if (!response.Equals(limitNotValid))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum in progress for a user logged in with only one board and no prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 1, 2));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum done for a user logged in with only one board and no prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 2, 2));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with only one board with prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 0, 3));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum in progress for a user logged in with only one board with prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 1, 1));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum done for a user logged in with only one board with prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 2, 4));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with more than one board and no prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard", 0, 2));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum in progress for a user logged in with more than one board and no prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard", 1, 2));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum done for a user logged in with more than one board and no prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard", 2, 2));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with only one board with prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard", 0, 3));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum in progress for a user logged in with only one board with prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard", 1, 1));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check limit colum done for a user logged in with only one board with prev limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard", 2, 4));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check limit colum for a user logged in without needed board
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard3", 0, 3));
            if (!response.Equals(emailOrBoardNotExist))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //Check limit column for a user logged out
            us.Logout("adi1");
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard2", 1, 2));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check logout -> login and limit existing board
            us.Login("adi1", "Adi1234");
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi1", "testboard2", 0, 4));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit column for user with no boards
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi3", "testboard21", 2, 8));
            if (!response.Equals(emailOrBoardNotExist))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with not valid coulmn ordinal
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 3, 2));
            if (!response.Equals(colOrdinalNotValid))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with not valid coulmn ordinal
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", -4, 2));
            if (!response.Equals(colOrdinalNotValid))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with only one board back to no limit
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 0, -1));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check limit colum backlog for a user logged in with only one board to the same limit currently
            response = JsonSerializer.Deserialize<Response>(bs.LimitColumn("adi", "testboard", 0, -1));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestInProg()
        {
            SetUp();
            //TaskService ts = new TaskService(us.getUserFacade(), bs.getBoardFacade());
            Response notLogedIn = new Response("User not logged in");
            Response res = new Response();
            bool test = true;

            us.Register("adi", "Adi1234");
            us.Register("adi1", "Adi1234");
            us.Register("adi2", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            bs.CreateBoard("adi2", "testboard");
            bs.CreateBoard("adi1", "testboard");
            bs.CreateBoard("adi1", "testboard1");
            bs.CreateBoard("adi1", "testboard2");
            DateTime dt = new DateTime(2024, 6, 18);
            ts.AddTask("adi", "testboard", "test", "a task", dt);
            ts.AddTask("adi", "testboard", "test1", "a task", dt);
            ts.AddTask("adi", "testboard", "test3", "a task", dt);

            ts.AddTask("adi1", "testboard", "test", "a task", dt);
            ts.AddTask("adi1", "testboard", "test1", "a task", dt);
            ts.AddTask("adi1", "testboard", "test3", "a task", dt);

            ts.AddTask("adi1", "testboard1", "test", "a task", dt);
            ts.AddTask("adi1", "testboard1", "test1", "a task", dt);
            ts.AddTask("adi1", "testboard1", "test3", "a task", dt);

            //  Check get tasks in progress for a user logged in with only one board and one in progress task,while having tasks in the backlog and done
            Response response = JsonSerializer.Deserialize<Response>(bs.InProgressTasks("adi"));
            if (response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check get tasks in progress for a user logged in with more than one boardand one in progress task , while having tasks in the backlog and done
            response = JsonSerializer.Deserialize<Response>(bs.InProgressTasks("adi"));
            if (response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //  Check get tasks in progress for a user logged in with only one board and few in progress tasks ,while having tasks in the backlog and done
            response = JsonSerializer.Deserialize<Response>(bs.InProgressTasks("adi"));
            if (response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check get tasks in progress for a user logged in with more than one boardand few in progress tasks , while having tasks in the backlog and done
            response = JsonSerializer.Deserialize<Response>(bs.InProgressTasks("adi"));
            if (response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            us.Logout("adi");
            //  Check get tasks in progress for a user logged out with only one board and one in progress task,while having tasks in the backlog and done
            response = JsonSerializer.Deserialize<Response>(bs.InProgressTasks("adi"));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check get tasks in progress for a user logged in with empty board
            response = JsonSerializer.Deserialize<Response>(bs.InProgressTasks("adi2"));

            if (response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }

        public bool TestGetColName()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardNotExist = new Response("board doesnt exist");
            Response defExeption = new Response("columnOrdinal doesn't match");
            String backlog = JsonSerializer.Serialize(new Response(null, "backlog"));
            String inProg = JsonSerializer.Serialize(new Response(null, "in progress"));
            String done = JsonSerializer.Serialize(new Response(null, "done"));

            bool test = true;

            us.Register("adi", "Adi1234");
            us.Register("adi1", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            bs.CreateBoard("adi1", "testboard");
            us.Logout("adi1");

            Response response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi", "testboard", 0));
            String answer = JsonSerializer.Serialize(response);
            if (!answer.Equals(backlog))
            {
                test = false;
                Console.WriteLine(response.ReturnValue);
            }
            // Check get backlog column name for npt valid column input
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi", "testboard", -1));
            if (!response.Equals(defExeption))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            // Check get backlog column name for not exist board 
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi", "testboard1", 0));
            if (!response.Equals(boardNotExist))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check get in progress column name for a user logged in
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi", "testboard", 1));
            answer = JsonSerializer.Serialize(response);
            if (!answer.Equals(inProg))
            {

                test = false;
                Console.WriteLine(response.ReturnValue);
            }

            // Check get done column name for a user logged in
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi", "testboard", 2));
            answer = JsonSerializer.Serialize(response);
            if (!answer.Equals(done))
            {
                test = false;
                Console.WriteLine(response.ReturnValue);
            }

            // Check get backlog column name for a user logged out 
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi1", "testboard", 0));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check get in progress column name for a user logged out
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi1", "testboard", 1));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check get done column name for a user logged out 
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi1", "testboard", 2));
            if (!response.Equals(notLogedIn))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check logout -> login and get column name
            us.Login("adi1", "Adi1234");
            response = JsonSerializer.Deserialize<Response>(bs.GetColumnName("adi1", "testboard", 0));
            answer = JsonSerializer.Serialize(response);
            if (!answer.Equals(backlog))
            {
                Console.WriteLine("9");
                test = false;
                Console.WriteLine(response.ReturnValue);
            }

            return test;
        }

        public bool TestGetCol()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            bool test = true;

            us.Register("adi", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            DateTime dt = new DateTime(2024, 6, 18);
            ts.AddTask("adi", "testboard", "test", "a task", dt);
            ts.AddTask("adi", "testboard", "test1", "a task", dt);
            ts.AddTask("adi", "testboard", "test3", "a task", dt);

            // Check create board for a user logged in with no boards
            Response response = JsonSerializer.Deserialize<Response>(bs.GetColumn("adi", "testboard", 0));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestGetName()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            Response boardId = new Response("Board ID not found");
            bool test = true;

            us.Register("adi", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            //test for valid board
            Response response = JsonSerializer.Deserialize<Response>(bs.GetBoardName(1));
            String answer = JsonSerializer.Serialize(response);
            String returnvalue = JsonSerializer.Serialize(new Response(null, "testboard"));
            if (!answer.Equals(returnvalue))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            bs.CreateBoard("adi", "testboard1");
            us.Register("adi1", "Adi1234");
            bs.CreateBoard("adi", "testboard2");
            response = JsonSerializer.Deserialize<Response>(bs.GetBoardName(3));
            answer = JsonSerializer.Serialize(response);
            returnvalue = JsonSerializer.Serialize(new Response(null, "testboard2"));
            //test for valid board
            if (!answer.Equals(returnvalue))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            response = JsonSerializer.Deserialize<Response>(bs.GetBoardName(4));
            Response error = new Response("Board ID not found");
            //test for unvalid boardid
            if (!response.Equals(error))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestTransferOwnerShip()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            Response boardId = new Response("Board ID not found");
            bool test = true;

            us.Register("adi", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            us.Register("asaf", "Asaf2345");
            bs.JoinBoard("asaf", 1);

            Response response = JsonSerializer.Deserialize<Response>(bs.TransferOwnership("adi", "asaf", "testboard"));
            String answer = JsonSerializer.Serialize(response);
            //valid transfer owner ship
            if (!answer.Equals(JsonSerializer.Serialize(emptyResponse)))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage + "1");
            }

            response = JsonSerializer.Deserialize<Response>(bs.TransferOwnership("asaf", "asaf1", "testboard"));
            answer = JsonSerializer.Serialize(response);
            String returnvalue = JsonSerializer.Serialize(new Response("new owner email didnt member in the board"));
            //invalid transfer owner ship - user dont exist
            if (!answer.Equals(returnvalue))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            response = JsonSerializer.Deserialize<Response>(bs.TransferOwnership("adi", "asaf", "testboard"));
            answer = JsonSerializer.Serialize(response);
            returnvalue = JsonSerializer.Serialize(new Response("you are not the board owner / board dont exist"));
            //invalid transfer owner ship - user dosent the owner
            if (!answer.Equals(returnvalue))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestUserBoard()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            Response boardId = new Response("Board ID not found");
            bool test = true;

            us.Register("adi", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            us.Register("asaf", "Asaf2345");
            bs.JoinBoard("asaf", 1);
            bs.CreateBoard("asaf", "testboard1");

            Response response = JsonSerializer.Deserialize<Response>(bs.GetUserBoards("asaf"));
            String answer = JsonSerializer.Serialize(response);
            //valid get user board
            if (answer.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }

        public bool TestJoinBoard()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            Response boardId = new Response("Board ID not found");
            bool test = true;

            us.Register("adi", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            us.Register("asaf", "Asaf2345");

            Response response = JsonSerializer.Deserialize<Response>(bs.JoinBoard("asaf", 1));
            //valid join board
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //join board for owner
            response = JsonSerializer.Deserialize<Response>(bs.JoinBoard("adi", 1));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //join board for board member
            response = JsonSerializer.Deserialize<Response>(bs.JoinBoard("asaf", 1));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //join board for user dont exist
            response = JsonSerializer.Deserialize<Response>(bs.JoinBoard("amit@gmail.com", 1));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //join board for board dont exist
            response = JsonSerializer.Deserialize<Response>(bs.JoinBoard("amit@gmail.com", 2));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }

        public bool TestLeaveBoard()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response notLogedIn = new Response("User not logged in");
            Response boardExist = new Response("Board name already exists");
            Response boardId = new Response("Board ID not found");
            bool test = true;

            us.Register("adi", "Adi1234");
            bs.CreateBoard("adi", "testboard");
            us.Register("asaf", "Asaf2345");
            bs.JoinBoard("asaf", 1);
            Response response = JsonSerializer.Deserialize<Response>(bs.LeaveBoard("asaf", 1));
            //valid join board
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //leave board for owner
            response = JsonSerializer.Deserialize<Response>(bs.LeaveBoard("adi", 1));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //leave board for user dont exist
            response = JsonSerializer.Deserialize<Response>(bs.LeaveBoard("amit@gmail.com", 1));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //leave board for user dont member
            response = JsonSerializer.Deserialize<Response>(bs.LeaveBoard("asaf", 1));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            //leave board for board dont exist
            response = JsonSerializer.Deserialize<Response>(bs.LeaveBoard("asaf", 2));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            return test;
        }
    }
}