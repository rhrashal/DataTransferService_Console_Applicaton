using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferService
{
    class Program
    {
        static void Main(string[] args)
        {
            /// https://stackoverflow.com/questions/13019433/calling-a-method-every-x-minutes
            var time = ConfigurationManager.AppSettings["excuteDuration"];
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);
            try
            {
                periodTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(time));
            }
            catch (Exception exp)
            {
                
                Console.Write(exp.Message + " \n");
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
                    Console.WriteLine("New Sale Data Not Found at "+DateTime.Now.ToShortTimeString()+".  \n");
                }                
                if (isSave)
                {
                    sale.UpdateData();
                }

                /// save summary data data
                /// 
                bool isSaveSummary = false;
                if (summryDt.Rows.Count>0)
                {
                    isSaveSummary = summary.SetData(summryDt);
                }
                else
                {
                    Console.WriteLine("New Summary Data Not Found at " + DateTime.Now.ToShortTimeString() + ".  \n");
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
