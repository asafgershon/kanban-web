using System;
using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;

namespace UI
{
    public class UserTest
    {
        public ServiceFactory sf;
        //public UserService us;
        string currentResponse;
        string wantedResponse;

        public UserTest()
        {
            this.sf = new ServiceFactory();
            currentResponse = "";
            wantedResponse = "";
        }

        public void SetUp()
        {
            this.sf = new ServiceFactory();
            currentResponse = "";
            wantedResponse = "";
        }

        public void RunTest()
        {
            Console.WriteLine("check register:");
            Console.WriteLine();
            if (TestRegister())
            {
                Console.WriteLine("pass all register test");
            }
            //sf.deleteData();
            //Console.WriteLine();
            /*
            Console.WriteLine("check Logout:");
            Console.WriteLine();
            if (TestLogout())
            {
                Console.WriteLine("pass all logout test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check Login:");
            Console.WriteLine();
            if (TestLogin())
            {
                Console.WriteLine("pass all Login test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check IsLoggedIn:");
            Console.WriteLine();
            if (TestIsloggedIn())
            {
                Console.WriteLine("pass all IsloggedIn test");
            }
            sf.deleteData();
            Console.WriteLine();

            Console.WriteLine("check Logout:");
            Console.WriteLine();
            if (TestLogout())
            {
                Console.WriteLine("pass all Logout test");
            }
            sf.deleteData();
            Console.WriteLine();
            */
        }

        public bool TestRegister()
        {
            SetUp();
            bool test = true;
            // Register a new valid user
            currentResponse = sf.us.Register("amit@gmail.com", "Amit1234");
            wantedResponse = JsonSerializer.Serialize(new Response());
            if (!currentResponse.Equals(wantedResponse))
            {
                Console.WriteLine("currentResponse: " + currentResponse);
                test = false;
            }

            // Register a new valid user
            currentResponse = sf.us.Register("Asaf", "Asaf1234");
            if (!currentResponse.Equals(wantedResponse))
            {
                Console.WriteLine("currentResponse: " + currentResponse);
                test = false;
            }

            // Register a new invalid user (user already exists)
            currentResponse = sf.us.Register("amit@gmail.com", "Amit1234");
            wantedResponse = JsonSerializer.Serialize(new Response("User name already taken"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }

            // Register a new invalid user (no uppercase letter in password)
            currentResponse = sf.us.Register("amit@gmail.com1", "amit123");
            wantedResponse = JsonSerializer.Serialize(new Response("Password must contain at least one uppercase letter, one lowercase letter, one digit and be between 6 and 20 characters long"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }

            //Register a new invalid user(no digit in password)
            currentResponse = sf.us.Register("amit@gmail.com1", "amitttt2");
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }

            // Register a new invalid user (no lowercase letter in password)
            currentResponse = sf.us.Register("amit@gmail.com1", "am1");
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }

            // Register a new invalid user (password too short)
            currentResponse = sf.us.Register("amit@gmail.com1", "amq");
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }

            // Register a new invalid user (password too long)
            currentResponse = sf.us.Register("amit@gmail.com1", "amitmfkewfwfeffdsvdcom12345678901234567890");
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }
            currentResponse = sf.us.Register("amit@gmail.com", "w");
            if(currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("currentResponse: " + currentResponse);
            }
            
            return test;
        }

        public bool TestLogout()
        {
            SetUp();
            bool test = true;
            sf.us.Register("amit@gmail.com", "Amit1234");
            // Logout a logged in user
            currentResponse = sf.us.Logout("amit@gmail.com");
            wantedResponse = JsonSerializer.Serialize(new Response());
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for valid logout, email: amit@gmail.com");
            }

            sf.us.Register("Asaf", "Asaf1234");
            // Logout a logged in user
            currentResponse = sf.us.Logout("Asaf");
            wantedResponse = JsonSerializer.Serialize(new Response());
            if (!currentResponse.Equals(wantedResponse))
            {
                Console.WriteLine("2");
                test = false;
                Console.WriteLine("test failed for valid logout, email: Asaf");
            }

            // Logout a user that is not logged in
            currentResponse = sf.us.Logout("amit@gmail.com");
            wantedResponse = JsonSerializer.Serialize(new Response("User not logged in"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for not logged in user, email: amit@gmail.com");
            }

            // Logout a user that does not exist
            currentResponse = sf.us.Logout("amit@gmail.com1");
            wantedResponse = JsonSerializer.Serialize(new Response("User does not exist"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for not existing user, email: amit@gmail.com1");
            }

            return test;
        }

        public bool TestLogin()
        {
            SetUp();
            bool test = true;
            //Login with a valid user
            currentResponse = sf.us.Register("amit@gmail.com", "Amit1234");
            sf.us.Logout("amit@gmail.com");
            wantedResponse = JsonSerializer.Serialize(new Response());
            currentResponse = sf.us.Login("amit@gmail.com", "Amit1234");
            wantedResponse = JsonSerializer.Serialize(new Response(null, "amit@gmail.com"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for valid login, email: amit@gmail.com and password: amit@gmail.com1234");
            }

            //Login with an already logged in user
            currentResponse = sf.us.Login("amit@gmail.com", "Amit1234");
            wantedResponse = JsonSerializer.Serialize(new Response("User already logged in"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for already logged in user, email: amit@gmail.com and password: amit@gmail.com1234");
            }

            //Login with an invalid user (wrong password)
            currentResponse = sf.us.Login("Asaf", "asaf1234");
            wantedResponse = JsonSerializer.Serialize(new Response("Incorrect user name or password"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for invalid login, email: Asaf and password: asaf1234");
            }

            //Login with an invalid user (user does not exist)
            currentResponse = sf.us.Login("amit@gmail.com1", "Amit1234");
            wantedResponse = JsonSerializer.Serialize(new Response("Incorrect user name or password"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for invalid login, email: amit@gmail.com1 and password: amit@gmail.com1234");
            }

            return test;
        }

        public bool TestIsloggedIn()
        {
            SetUp();
            bool test = true;
            currentResponse = sf.us.Register("amit@gmail.com", "Amit1234");
            // Check if a user is logged in, for a logged in user
            currentResponse = sf.us.IsloggedIn("amit@gmail.com");
            wantedResponse = JsonSerializer.Serialize(new Response());
            if (!currentResponse.Equals(wantedResponse))
            {
                Console.WriteLine("currentResponse: " + currentResponse);
                test = false;
                Console.WriteLine("test failed for valid IsloggedIn, email: amit@gmail.com");
            }

            // Check if a user is logged in, for a user that is not logged in
            sf.us.Register("Asaf", "Amit1234");
            sf.us.Logout("Asaf");
            currentResponse = sf.us.IsloggedIn("Asaf");
            wantedResponse = JsonSerializer.Serialize(new Response("User not logged in"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for not logged in user, email: Asaf");
            }

            // Check if a user is logged in, for a user that is not exist
            currentResponse = sf.us.IsloggedIn("amit@gmail.com1");
            wantedResponse = JsonSerializer.Serialize(new Response("User does not exist"));
            if (!currentResponse.Equals(wantedResponse))
            {
                test = false;
                Console.WriteLine("test failed for not logged in user, email: amit@gmail.com1");
            }

            return test;
        }

    }
}
