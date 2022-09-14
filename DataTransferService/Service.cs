using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataTransferService
{
    class Service
    {
        string from_connection = ConfigurationManager.ConnectionStrings["FROM_DB_CONN"].ConnectionString;
        string to_connection = ConfigurationManager.ConnectionStrings["TO_DB_CONN"].ConnectionString;
        [STAThread]
        public DataTable GetData()
        {   
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(from_connection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    cmd.CommandText = "SELECT * FROM dbo.Sale" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + "_Processed WHERE isSync IS NULL OR isSync = 0";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 0;
                    da.Fill(dataTable);

                    //Console.WriteLine("Succesfully Selected \n");
                   
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message + " \n");
                
            }
            finally
            {
                conn.Close();
            }
            return dataTable;
            //Console.WriteLine("\n\n Press any key to quite");
            //Console.Read();
        }




        [STAThread]
        public bool SetData(DataTable dt)
        {
            bool res = false;
            SqlConnection conn = new SqlConnection(to_connection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@" INSERT INTO [dbo].[Sale" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + "] ([CmpIDX] ,[Invoice] ,[SaleDT] ,[sBarCode] ,[BarCode] ,[SKU] ,[SupID] ,[SupName] ,[GroupName] ,[PrdName] ,[BTName] ,[SSName] ,[Qty] ,[CPU] ,[RPU] ,[wsp] ,[wsq] ,[SQty] ,[rQty] ,[rAmt] ,[cInvoice] ,[ShopID] ,[PayType] ,[Discount] ,[DiscAmt] ,[DiscAmtPrd] ,[VAT] ,[PrdSlNo] ,[UserID] ,[CardName] ,[CardNo] ,[CounterID] ,[PrvCusID] ,[CusName] ,[VATPrcnt] ,[DiscPrcnt] ,[Returned] ,[Flag] ,[MrCode] ,[Point] ,[TSEC] ,[Posted] ,[Ref] ,[IsTransfer] ) VALUES ");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("( '" + row["CmpIDX"].ToString() + "',");
                        sb.Append(" '" + row["Invoice"].ToString() + "',");
                        sb.Append(" '" + row["SaleDT"].ToString() + "',");
                        sb.Append(" '" + row["sBarCode"].ToString() + "',");
                        sb.Append(" '" + row["BarCode"].ToString() + "',");
                        sb.Append(" '" + row["SKU"].ToString() + "',");
                        sb.Append(" '" + row["SupID"].ToString() + "',");
                        sb.Append(" '" + row["SupName"].ToString() + "',");
                        sb.Append(" '" + row["GroupName"].ToString() + "',");
                        sb.Append(" '" + row["PrdName"].ToString() + "',");
                        sb.Append(" '" + row["BTName"].ToString() + "',");
                        sb.Append(" '" + row["SSName"].ToString() + "',");
                        sb.Append(" '" + row["Qty"].ToString() + "',");
                        sb.Append(" '" + row["CPU"].ToString() + "',");
                        sb.Append(" '" + row["RPU"].ToString() + "',");
                        sb.Append(" '" + row["wsp"].ToString() + "',");
                        sb.Append(" '" + row["wsq"].ToString() + "',");
                        sb.Append(" '" + row["SQty"].ToString() + "',");
                        sb.Append(" '" + row["rQty"].ToString() + "',");
                        sb.Append(" '" + row["rAmt"].ToString() + "',");
                        sb.Append(" '" + row["cInvoice"].ToString() + "',");
                        sb.Append(" '" + row["ShopID"].ToString() + "',");
                        sb.Append(" '" + row["PayType"].ToString() + "',");
                        sb.Append(" '" + row["Discount"].ToString() + "',");
                        sb.Append(" '" + row["DiscAmt"].ToString() + "',");
                        sb.Append(" '" + row["DiscAmtPrd"].ToString() + "',");
                        sb.Append(" '" + row["VAT"].ToString() + "',");
                        sb.Append(" '" + row["PrdSlNo"].ToString() + "',");
                        sb.Append(" '" + row["UserID"].ToString() + "',");
                        sb.Append(" '" + row["CardName"].ToString() + "',");
                        sb.Append(" '" + row["CardNo"].ToString() + "',");
                        sb.Append(" '" + row["CounterID"].ToString() + "',");
                        sb.Append(" '" + row["PrvCusID"].ToString() + "',");
                        sb.Append(" '" + row["CusName"].ToString() + "',");
                        sb.Append(" '" + row["VATPrcnt"].ToString() + "',");
                        sb.Append(" '" + row["DiscPrcnt"].ToString() + "',");
                        sb.Append(" '" + row["Returned"].ToString() + "',");
                        sb.Append(" '" + row["Flag"].ToString() + "',");
                        sb.Append(" '" + row["MrCode"].ToString() + "',");
                        sb.Append(" '" + row["Point"].ToString() + "',");
                        sb.Append(" '" + row["TSEC"].ToString() + "',");
                        sb.Append(" '" + row["Posted"].ToString() + "',");
                        sb.Append(" '" + row["Ref"].ToString() + "',");
                        sb.Append(" '" + row["IsTransfer"].ToString() + "'),");                        
                    }
                    sb.Remove(sb.Length - 1, 1);  
                    cmd.CommandText = sb.ToString();

                    cmd.ExecuteScalar();
                    res = true;
                    Console.WriteLine(dt.Rows.Count + " data succesfully inserted at " + DateTime.Now.ToShortTimeString() + " \n");

                }
            }
            catch (Exception exp)
            {
                res = false;
                Console.Write(exp.Message + " \n");

            }
            finally
            {
                conn.Close();
            }
            return res;
            //Console.WriteLine("\n\n Press any key to quite");
            //Console.Read();
        }



        [STAThread]
        public void UpdateData()
        {

            
            SqlConnection conn = new SqlConnection(from_connection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@" UPDATE [dbo].[Sale" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + "_Processed] SET isSync = 1 WHERE isSync IS NULL OR isSync = 0 ");                    
                    cmd.CommandText = sb.ToString();
                    cmd.ExecuteScalar();
                    //Console.WriteLine("Succesfully Selected \n");

                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message + " \n");

            }
            finally
            {
                conn.Close();
            }
            //Console.WriteLine("\n\n Press any key to quite");
            //Console.Read();
        }  

    }


    class SummaryService
    {
        string from_connection = ConfigurationManager.ConnectionStrings["FROM_DB_CONN"].ConnectionString;
        string to_connection = ConfigurationManager.ConnectionStrings["TO_DB_CONN"].ConnectionString;
        [STAThread]
        public DataTable GetData()
        {            
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(from_connection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    cmd.CommandText = "SELECT * FROM dbo.SSummary_Processed WHERE isSync IS NULL OR isSync = 0";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 0;
                    da.Fill(dataTable);

                    //Console.WriteLine("Succesfully Selected \n");

                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message + " \n");

            }
            finally
            {
                conn.Close();
            }
            return dataTable;
            //Console.WriteLine("\n\n Press any key to quite");
            //Console.Read();
        }




        [STAThread]
        public bool SetData(DataTable dt)
        {
            bool res = false;            
            SqlConnection conn = new SqlConnection(to_connection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@" INSERT INTO [dbo].[SSummary] ([Invoice],[SaleDT],[TotalCost],[TotalAmt],[Discount],[DiscAmt],[VAT],[NetAmt],[CshAmt],[CrdAmt],[PayType],[UserID],[ShopID],[CardName],[CardNo],[CounterID],[PrvCusID],[CusName],[ReturnedAmt],[rTotalCost],[rVATAmt],[rDiscAmt],[rTotalAmt],[cInvoice],[ReturnedType],[Flag],[PaidAmt],[ChangeAmt],[Point],[MrCode],[TSEC],[advamt],[advslip],[Ref],[RefNO],[DiscType],[AutoID] ) VALUES ");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("( '" + row["Invoice"].ToString() + "',");
                        sb.Append(" '" + row["SaleDT"].ToString() + "',");
                        sb.Append(" '" + row["TotalCost"].ToString() + "',");
                        sb.Append(" '" + row["TotalAmt"].ToString() + "',");
                        sb.Append(" '" + row["Discount"].ToString() + "',");
                        sb.Append(" '" + row["DiscAmt"].ToString() + "',");
                        sb.Append(" '" + row["VAT"].ToString() + "',");
                        sb.Append(" '" + row["NetAmt"].ToString() + "',");
                        sb.Append(" '" + row["CshAmt"].ToString() + "',");
                        sb.Append(" '" + row["CrdAmt"].ToString() + "',");
                        sb.Append(" '" + row["PayType"].ToString() + "',");
                        sb.Append(" '" + row["UserID"].ToString() + "',");
                        sb.Append(" '" + row["ShopID"].ToString() + "',");
                        sb.Append(" '" + row["CardName"].ToString() + "',");
                        sb.Append(" '" + row["CardNo"].ToString() + "',");
                        sb.Append(" '" + row["CounterID"].ToString() + "',");
                        sb.Append(" '" + row["PrvCusID"].ToString() + "',");
                        sb.Append(" '" + row["CusName"].ToString() + "',");
                        sb.Append(" '" + row["ReturnedAmt"].ToString() + "',");
                        sb.Append(" '" + row["rTotalCost"].ToString() + "',");
                        sb.Append(" '" + row["rVATAmt"].ToString() + "',");
                        sb.Append(" '" + row["rDiscAmt"].ToString() + "',");
                        sb.Append(" '" + row["rTotalAmt"].ToString() + "',");
                        sb.Append(" '" + row["cInvoice"].ToString() + "',");
                        sb.Append(" '" + row["ReturnedType"].ToString() + "',");
                        sb.Append(" '" + row["Flag"].ToString() + "',");
                        sb.Append(" '" + row["PaidAmt"].ToString() + "',");
                        sb.Append(" '" + row["ChangeAmt"].ToString() + "',");
                        sb.Append(" '" + row["Point"].ToString() + "',");
                        sb.Append(" '" + row["MrCode"].ToString() + "',");
                        sb.Append(" '" + row["TSEC"].ToString() + "',");
                        sb.Append(" '" + row["advamt"].ToString() + "',");
                        sb.Append(" '" + row["advslip"].ToString() + "',");
                        sb.Append(" '" + row["Ref"].ToString() + "',");
                        //sb.Append(" '" + row["IsTransfer"].ToString() + "',");
                        sb.Append(" '" + row["RefNO"].ToString() + "',");
                        sb.Append(" '" + row["DiscType"].ToString() + "',");
                        sb.Append(" '" + row["AutoID"].ToString() + "'),");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    cmd.CommandText = sb.ToString();

                    cmd.ExecuteScalar();
                    res = true;
                    Console.WriteLine(dt.Rows.Count + " data succesfully inserted at "+ DateTime.Now.ToShortTimeString() + " \n");

                }
            }
            catch (Exception exp)
            {
                res = false;
                Console.Write(exp.Message + " \n");

            }
            finally
            {
                conn.Close();
            }
            return res;
            //Console.WriteLine("\n\n Press any key to quite");
            //Console.Read();
        }



        [STAThread]
        public void UpdateData()
        {
            SqlConnection conn = new SqlConnection(from_connection);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@" UPDATE [dbo].[SSummary_Processed] SET isSync = 1 WHERE isSync IS NULL OR isSync = 0 ");
                    cmd.CommandText = sb.ToString();
                    cmd.ExecuteScalar();
                    //Console.WriteLine("Succesfully Selected \n");

                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message + " \n");

            }
            finally
            {
                conn.Close();
            }
            //Console.WriteLine("\n\n Press any key to quite");
            //Console.Read();
        }

    }

}
