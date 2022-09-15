using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DataTransferService
{
     ///log4net 4.5 : https://stackify.com/log4net-guide-dotnet-logging/
     ///log4net 5.0 : https://jakubwajs.wordpress.com/2019/11/28/logging-with-log4net-in-net-core-3-0-console-app/comment-page-1/?unapproved=3749&moderation-hash=de63c05c7449972c2afd0e03a2bea206#comment-3749
     ///log4net .net core web : https://blog.hackajob.co/how-to-log-in-asp-net-core-web-apps-using-log4net/

    class Program
    {
        ///log4net: https://stackify.com/log4net-guide-dotnet-logging/
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        ///show/hide cmd  https://stackoverflow.com/questions/3571627/show-hide-the-console-window-of-a-c-sharp-console-application      
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        static void Main(string[] args)
        {
           

            //log.Info("Hello logging world!   Info");
            //log.Debug("Hello logging world!   Debug");
            //log.Warn("Hello logging world!   Warn");
            //log.Error("Hello logging world!  Error");
            //log.Fatal("Hello logging world!  Fatal");
            

            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);
            // Show
            //ShowWindow(handle, SW_SHOW);



            //Console.WriteLine("*************** Wellcome  Data Transfer Service. Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "***************");
            log.Info("*************** Welcome  Data Transfer Service. Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "***************");
            var time = ConfigurationManager.AppSettings["excuteDuration"];
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);
            try
            {
                periodTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(time));
            }
            catch (Exception exp)
            {                
                //Console.Write(exp.Message + " \n");
                log.Error(exp.Message);
            }

            var timer = new System.Threading.Timer((e) =>
            {

                /// create service instance
                Service sale = new Service();
                SummaryService summary = new SummaryService();

                /// get data from source db
                DataTable saleDt = sale.GetData();
                DataTable summryDt = summary.GetData();

                /// save sale data data
                /// 
                bool isSave = false;
                if (saleDt.Rows.Count>0)
                {
                    isSave = sale.SetData(saleDt);
                }
                else
                {
                    //Console.WriteLine("New Sale Data Not Found at "+DateTime.Now.ToShortTimeString());
                    log.Info("New Sale Data Not Found at " + DateTime.Now.ToShortTimeString());
                }                
                if (isSave)
                {
                    sale.UpdateData();
                }

                /// save summary data data
                /// 
                bool isSaveSummary = false;
                if (summryDt.Rows.Count > 0)
                {
                    isSaveSummary = summary.SetData(summryDt);
                }
                else
                {
                    //Console.WriteLine("New Summary Data Not Found at " + DateTime.Now.ToShortTimeString());
                    log.Info("New Summary Data Not Found at " + DateTime.Now.ToShortTimeString());
                }
                if (isSaveSummary)
                {
                    summary.UpdateData();
                }

            }, null, startTimeSpan, periodTimeSpan);



            Console.Read();

            /// run-> shell:startup then paste exe shortcut for auto run when pc start
        }
    }
}
