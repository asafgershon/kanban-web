using System;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;
using log4net;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net.Config;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserService us;
        public boardService bs;
        public TaskService ts;

        //constructor
        public ServiceFactory()
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));
            log.Info("System Init");
            UserFacade UF = new UserFacade();
            BoardFacede BF = new BoardFacede(UF);
            us = new UserService(UF);
            bs = new boardService(BF);
            ts = new TaskService(BF);
            //loadData();
        }

        //constructor without the loaddata
        public ServiceFactory(bool containdata)
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new System.IO.FileInfo("log4net.config"));
            log.Info("System init");
            UserFacade UF = new UserFacade();
            BoardFacede BF = new BoardFacede(UF);
            us = new UserService(UF);
            bs = new boardService(BF);
            ts = new TaskService(BF);
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string loadData()
        {
            log.Info("Loading data");
            string res = us.loadUsers();
            string res1 = bs.loadBoards();
            if(res.Equals(res1) && res.Equals(JsonSerializer.Serialize(new Response()))){
                return res1;
            }
            else
            {
                if(res.Equals(JsonSerializer.Serialize(new Response()))){
                    return res1;
                }
                else
                {
                    return res;
                }
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string deleteData()
        {
            log.Info("Deleting data");
            string res1 = bs.deleteAllBoards();
            string res = us.deleteUsers();
            if (res.Equals(res1) && res.Equals(JsonSerializer.Serialize(new Response()))){
                return res1;
            }
            else
            {
                if (res.Equals(JsonSerializer.Serialize(new Response()))){
                    return res1;
                }
                else
                {
                    return res;
                }
            }
        }

        //return the UserService for use in the board test
        public UserService GetUser()
        {
            return us;
        }

        //return the boardservice for use in the board test
        public boardService GetBoard()
        {
            return bs;
        }

        ////return the TaskService for use in the board test
        public TaskService GetTask()
        {
            return ts;
        }
    }
}
