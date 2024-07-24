using System;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;

namespace UI
{
    public class TaskTest
    {
        public ServiceFactory sf;

        public TaskTest()
        {
            this.sf = new ServiceFactory();
        }

        public void SetUp()
        {
            this.sf = new ServiceFactory();
        }

        public void RunTest()
        {
            Console.WriteLine("check Add Task");
            Console.WriteLine();
            sf.deleteData();
            if (TestAddTask())
            {
                Console.WriteLine("pass all Add Task test");
            }
            sf.deleteData();
            Console.WriteLine();


            Console.WriteLine("check Advance Task");

            Console.WriteLine();
            if (TestAdvanceTask())
            {
                Console.WriteLine("pass all Advance Task test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check update description Task");

            Console.WriteLine();
            if (UpdateTaskDescription())
            {
                Console.WriteLine("pass all update description Task test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check update title Task:");

            Console.WriteLine();
            if (TestUpdateTaskTitle())
            {
                Console.WriteLine("pass all update title Task test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check update Duedate Task:");

            Console.WriteLine();
            if (TestUpdateTaskDueDate())
            {
                Console.WriteLine("pass all update DueDate Task test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check assign Task:");

            Console.WriteLine();
            if (TestAssignTask())
            {
                Console.WriteLine("pass all assign Task test");
            }
            sf.deleteData();

            Console.WriteLine();

        }

        public bool TestAddTask()
        {
            SetUp();
            Response emptyResponse = new Response();
            bool test = true;

            sf.us.Register("asafgershon", "Asaf3150");
            sf.us.Register("asafgershon2", "Asaf3151");
            sf.bs.CreateBoard("asafgershon", "testboard");

            DateTime dt = new DateTime(2024, 6, 18); // Year, Month, Day

            // Check add valid task
            Response response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check add more valid tasks
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "test2", "test the add task", dt));
            Response response1 = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "test3", "test the add task", dt));
            if (!response.Equals(emptyResponse) && !response1.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check add more task to new board
            sf.bs.CreateBoard("asafgershon", "testboard2");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard2", "test3", "test the add task", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check logout -> login and add more task to old board
            sf.us.Logout("asafgershon");
            sf.us.Login("asafgershon", "Asaf3150");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard2", "test4", "test the add task", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check logout -> switch user and add more task to old board
            sf.us.Logout("asafgershon");
            sf.us.Login("asafgershon2", "Asaf3151");
            sf.bs.CreateBoard("asafgershon2", "testboard3");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon2", "testboard3", "test4", "test the add task", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }
            sf.us.Logout("asafgershon2");
            sf.us.Login("asafgershon", "Asaf3150");

            // Check add valid task to invalid email
            Response res = new Response("User does not exist");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon1", "testboard", "test", "test the add task", dt));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine("you enter task to user that not exist");
            }

            // Check add invalid task (no title)
            res = new Response("one of the option is empty");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "", "test the add task", dt));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine("you add empty title task");
            }

            // Check add invalid task (exceeds limit)
            res = new Response("you reach the limit amount of task, please update task limit");
            sf.bs.LimitColumn("asafgershon", "testboard", 0, 3);
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "test2", "test the limit task", dt));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine("you able to add task despite the limit");
            }
            sf.bs.LimitColumn("asafgershon", "testboard", 0, -1);

            // Check add task with leading/trailing whitespaces in email or board name
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask(" asafgershon ", " testboard ", "test6", "test whitespace", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with leading/trailing whitespaces in email or board name" + response.ErrorMessage);
            }

            // Check add task with case-insensitive email
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("ASAFGERSHON", "testboard", "test6", "test case insensitive", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with case-insensitive email " + response.ErrorMessage);
            }

            // Check create board with varying case in email
            sf.bs.CreateBoard("ASAFGERSHON", "TestBoardCase");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("ASAFGERSHON", "TestBoardCase", "testCase1", "test case insensitive", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task to board with case-insensitive email " + response.ErrorMessage);
            }

            // Check create board with leading/trailing whitespaces in email and board name
            sf.bs.CreateBoard(" asafgershon ", " TestBoardWhitespace ");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask(" asafgershon ", "TestBoardWhitespace ", "testWhitespace1", "test whitespace", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task to board with leading/trailing whitespaces in email or board name");
            }

            // Add Task with Duplicate Title
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "test", "test duplicate title", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with duplicate title");
            }

            // Add Task with Special Characters in Title and Description
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "test@#!$", "test the add task @#!$", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with special characters");
            }

            // Add Task with Future Due Date
            DateTime futureDate = new DateTime(2025, 6, 18);
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "testFuture", "test the future due date", futureDate));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with future due date");
            }

            // Add Task with Past Due Date
            DateTime pastDate = new DateTime(2020, 6, 18);
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "testPast", "test the past due date", pastDate));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with past due date");
            }

            // Add Task with Unicode Characters in Title and Description
            response = JsonSerializer.Deserialize<Response>(sf.ts.AddTask("asafgershon", "testboard", "テスト", "テストタスク", dt));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("Failed to add task with Unicode characters");
            }

            return test;
        }

        public bool TestAdvanceTask()
        {
            SetUp();
            // Build the object
            Response emptyResponse = new Response();
            bool test = true;

            sf.us.Register("asafgershon", "Asaf3150");
            sf.us.Register("amit@gmail.com", "Asaf3150");
            sf.bs.CreateBoard("asafgershon", "testboard");
            sf.bs.JoinBoard("amit@gmail.com", 1);
            DateTime dt = new DateTime(2024, 6, 18); // Year, Month, Day
            sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt);

            // Check advance valid task
            Response response = JsonSerializer.Deserialize<Response>(sf.ts.AdvanceTask("asafgershon", "testboard", 0, 0));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check advance from done column valid task
            Response res = new Response("the task is in done collumns");
            sf.ts.AdvanceTask("asafgershon", "testboard", 1, 0);
            response = JsonSerializer.Deserialize<Response>(sf.ts.AdvanceTask("asafgershon", "testboard", 2, 0));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("You advanced a task that is in the done column");
            }

            // Check add valid task to invalid email
            res = new Response("User does not exist");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AdvanceTask("asafgershon1", "testboard", 1, 0));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check add invalid task (no title)
            res = new Response("one of the option is empty");
            sf.ts.AssignTask("amit@gmail.com", "testboard", 0, 0, "asafgershon");
            response = JsonSerializer.Deserialize<Response>(sf.ts.AdvanceTask("asafgershon", "testboard", 1, 0));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }

        public bool UpdateTaskDescription()
        {
            SetUp();

            // Build the object and set up the initial state
            Response emptyResponse = new Response();
            bool test = true;

            // Register and log in the user
            sf.us.Register("asafgershon", "Asaf3150");

            // Create a board and add a task
            sf.bs.CreateBoard("asafgershon", "testboard");
            DateTime dt = new DateTime(2024, 6, 18); // Year, Month, Day
            sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt);

            // Change the task description successfully
            Response response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDescription("asafgershon", "testboard", 0, 0, "Updated description"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Test case: Attempt to update a task description while not logged in
            sf.us.Logout("asafgershon");
            Response res = new Response("User not logged in");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDescription("asafgershon", "testboard", 0, 0, "Should not succeed"));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Log back in for further tests
            sf.us.Login("asafgershon", "Asaf3150");

            // Test case: Attempt to update a task on a non-existent board
            res = new Response("board does not exist");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDescription("asafgershon", "nonexistentboard", 0, 0, "Should not succeed"));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Test case: Attempt to update a non-existent task
            res = new Response("the column ordinal input isnt valid, supposed to be 0/1/2");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDescription("asafgershon", "testboard", 999, 0, "Should not succeed"));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Additional test cases can be added here, such as:
            // - Attempting to update the task description with invalid inputs
            // - Checking if the description length exceeds 300 characters

            // Example: Attempt to update the task description with an excessively long description
            string longDescription = new string('a', 301); // 301 characters
            res = new Response("Description exceeds 300 characters.");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDescription("asafgershon", "testboard", 0, 0, longDescription));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            sf.ts.AdvanceTask("asafgershon", "testboard", 0, 0);
            sf.ts.AdvanceTask("asafgershon", "testboard", 1, 0);
            res = new Response("your task in done column");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDescription("asafgershon", "testboard", 2, 0, "Updated description"));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }

        public bool TestUpdateTaskTitle()
        {
            SetUp();
            // Build the object
            Response emptyResponse = new Response();
            bool test = true;

            sf.us.Register("asafgershon", "Asaf3150");
            sf.bs.CreateBoard("asafgershon", "testboard");
            DateTime dt = new DateTime(2024, 6, 18); // Year, Month, Day
            sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt);

            // Check update valid task title
            Response response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskTitle("asafgershon", "testboard", 0, 0, "New Title"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check update task title when task is in the done column
            Response res = new Response("your task in done column");
            sf.ts.AdvanceTask("asafgershon", "testboard", 0, 0);
            sf.ts.AdvanceTask("asafgershon", "testboard", 1, 0);
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskTitle("asafgershon", "testboard", 2, 0, "Another Title"));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine("You updated task title in the done column");
            }

            // Check update task title with invalid email
            res = new Response("User does not exist");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskTitle("asafgershon1", "testboard", 0, 0, "Title"));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check update task title with empty title
            res = new Response("your title is empty or pass 50 charcters");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskTitle("asafgershon", "testboard", 0, 0, ""));
            if (!response.Equals(res))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }

        public bool TestUpdateTaskDueDate()
        {
            SetUp();
            // Build the object
            Response emptyResponse = new Response();
            bool test = true;

            sf.us.Register("asafgershon", "Asaf3150");
            sf.bs.CreateBoard("asafgershon", "testboard");
            DateTime dt = new DateTime(2024, 6, 18); // Year, Month, Day
            sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt);

            // Check update valid task due date
            DateTime newDate = new DateTime(2024, 7, 18); // New due date
            Response response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDueDate("asafgershon", "testboard", 0, 0, newDate));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage + "1");
            }

            // Check update task due date when task is in the done column
            Response res = new Response("your task in done column");
            sf.ts.AdvanceTask("asafgershon", "testboard", 0, 0);
            sf.ts.AdvanceTask("asafgershon", "testboard", 1, 0);
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDueDate("asafgershon", "testboard", 2, 0, newDate));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine("You updated task due date in the done column");
            }

            sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt);

            // Check update task due date with invalid email
            res = new Response("User does not exist");
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDueDate("asafgershon1", "testboard", 0, 1, newDate));
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Check update task due date with past due date
            DateTime pastDate = new DateTime(2023, 6, 18); // Past due date
            response = JsonSerializer.Deserialize<Response>(sf.ts.UpdateTaskDueDate("asafgershon", "testboard", 0, 1, pastDate));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }

        public bool TestAssignTask()
        {
            SetUp();
            Response emptyResponse = new Response();
            Response error = new Response();
            bool test = true;

            // Register users
            sf.us.Register("asafgershon", "Asaf3150");
            sf.us.Register("amit12", "Amit1234");
            sf.us.Register("adicohen", "Adi5678");

            // Create and join boards
            sf.bs.CreateBoard("asafgershon", "testboard");
            sf.bs.JoinBoard("amit12", 1);
            sf.bs.JoinBoard("adicohen", 1);

            // Add a task
            DateTime dt = new DateTime(2024, 6, 18); // Year, Month, Day
            sf.ts.AddTask("asafgershon", "testboard", "test", "test the add task", dt);

            // Assign task successfully
            Response response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("asafgershon", "testboard", 0, 0, "asafgershon"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task successfully
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("asafgershon", "testboard", 0, 0, "amit12"));
            if (!response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task that is not in the column
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("asafgershon", "testboard", 1, 0, "amit12"));
            error = new Response("Task not found");
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task with incorrect task id
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("asafgershon", "testboard", 0, 1, "amit12"));
            error = new Response("Task not found");
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task to a user not in the board
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("asafgershon", "testboard", 0, 0, "nonexistentuser"));
            error = new Response("User not found");
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task on a nonexistent board
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("asafgershon", "nonexistentboard", 0, 0, "adicohen"));
            error = new Response("Board not found");
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task by a user who is not the owner and not the assignee
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("amit@gmail.comgershon", "testboard", 0, 0, "adicohen"));
            error = new Response("Permission denied");
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            // Assign task by a user who is not in the board
            response = JsonSerializer.Deserialize<Response>(sf.ts.AssignTask("adicohen1", "testboard", 0, 0, "asafgershon"));
            error = new Response("Permission denied");
            if (response.Equals(emptyResponse))
            {
                test = false;
                Console.WriteLine(response.ErrorMessage);
            }

            return test;
        }
    }
}