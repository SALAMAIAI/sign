using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
//using WebApplication2.WebReference;
using System.Configuration;
using System.Web.Services;
using System.Net;
using System.Threading;
using System.Web.SessionState;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WebApplication2.App_Code;
namespace WebApplication2.application
{
    public partial class viewAppIdikhar : System.Web.UI.Page
    {
        int mid;

        public string sqlconn = ConfigurationManager.ConnectionStrings["LIFE"].ToString();
        string conn = ConfigurationManager.ConnectionStrings["LIFE"].ToString();
        string cust_id = "";
        int app_id = 0;
        string illust_id = "";
        int v_trms = 0;

        string constr = ConfigurationManager.ConnectionStrings["MembershipConnectionString"].ToString();
     
       string cconn= ConfigurationManager.ConnectionStrings["LIFE"].ToString();


        private string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {
                return null;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Broker_Id"] != null || Session["disb_uid"] != null)
                {

                }
                else
                {
                    Session.Abandon();
                    Response.Redirect(@"..\index.aspx");
                }


            }
            catch
            {
                Session.Abandon();
                Response.Redirect(@"..\index.aspx");
            }

            int illustration_id = 0;
            string cust_id = "";
            int app_id = 0;
            string illust_id = "";
            int joint_life = 0;
            int plan_holder = 0;
            
            DataSet dst_App_Other_Insurance_Details_PlanHolder = new DataSet();
            DataTable dt_App_Other_Insurance_Details_PlanHolder = new DataTable();                        
            DataSet dst_App_Details_PlanHolder = new DataSet();
            DataTable dt_App_Details_PlanHolder = new DataTable();
            DataSet dst_App_Contact_PlanHolder = new DataSet();
            DataTable dt_App_Contact_PlanHolder = new DataTable();
            DataSet dst_app_permanent_PlanHolder = new DataSet();
            DataTable dt_App_Permanent_PlanHolder = new DataTable();
            DataSet dst_App_Fund_PlanHolder = new DataSet();
            DataTable dt_App_Fund_PlanHolder = new DataTable();
            DataSet dst_App_Assets_PlanHolder = new DataSet();
            DataTable dt_App_Assets_PlanHolder = new DataTable();
            DataSet dst_App_Liability_PlanHolder = new DataSet();
            DataTable dt_App_Liability_PlanHolder = new DataTable();
            DataTable dt_App_Agent_Details = new DataTable();

            mid = Convert.ToInt32(Decrypt(Request.QueryString["Pid"]));
          
            if (mid > 0)
            {
            }
            else
            {
                Session.Abandon();
                Response.Redirect(@"..\login.aspx");
            }


            string plan_Holder_name = "";
            OleDbConnection myconnection_4 = new OleDbConnection(sqlconn);
            OleDbCommand cmd;
            OleDbDataReader prod_dr;
            string SqlQuery = "";
            string rpt_path = "";
            string Path_App = "";
            string Path_Illu = "";
            string App_no="";
            string App_no1 = "";
            string Exclusion_path = "";

            try
            {
                myconnection_4.Open();
                SqlQuery = "Select application_path,illustration_path,Application_no,Exclusion_path from  Application_Master where Application_Id=" + mid + "  and Maker_Status=1  ";
                cmd = new OleDbCommand(SqlQuery, myconnection_4);
                prod_dr = cmd.ExecuteReader();
                while (prod_dr.Read())
                {
                    {
                        Path_App = prod_dr[0].ToString().Trim();
                        Path_Illu = prod_dr[1].ToString().Trim();
                        App_no = prod_dr[2].ToString().Trim();
                        App_no1 = prod_dr[2].ToString().Trim();
                        Exclusion_path = prod_dr[3].ToString().Trim();

                    }
                   
                }
                prod_dr.Close();
                myconnection_4.Close();
                cmd.Cancel();
            }
            catch 
            {
                myconnection_4.Close();
            }

            if(Path_App !="" && Path_Illu!="")
            {
                try
                {
                    App_no = "ApplicationForm_" + App_no;
                    Response.ClearContent();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment;Filename=" + App_no + ".pdf");
                    Response.Clear();
                    Response.TransmitFile(Path_App);
                    Response.End();

                    return;
                }
                catch 
                {

                }

            }
            
            String sqlquery;
            DataSet dst = new DataSet();
            OleDbConnection mycon = new OleDbConnection(conn);

            mycon.Open();
            //sqlquery = "Select Application_Master.Application_no,Customer_Master.Email,Customer_Master.Title, " +
            //    "  Customer_Master.F_Name,Customer_Master.M_Name,Customer_Master.L_Name,Application_Master.Customer_Id,Application_Master.illustration_id from " +
            //    " Application_Master inner join Customer_Master on Application_Master.Customer_Id =Customer_Master.Customer_Id where Application_Id=" + mid + " and  Lock_Status=1";
            sqlquery = "select Customer_Id from Application_Master where Application_Id = '" + mid + "'";
            cmd = new OleDbCommand(sqlquery, mycon);
            OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
            dst = new DataSet();
            oda.Fill(dst);
            oda.Dispose();
            cmd.Cancel();
            mycon.Close();
            cust_id = dst.Tables[0].Rows[0]["Customer_Id"].ToString();            
            app_id = Convert.ToInt32(mid);
           
            try
            {
                sqlquery = "";
                DataSet dst_Life = new DataSet();
                mycon = new OleDbConnection(conn);
                mycon.Open();
                sqlquery = "Select Plan_Holder,Joint_life,Illustration_Id from Application_Master where Application_Id=" + mid + "";
                cmd = new OleDbCommand(sqlquery, mycon);
                oda = new OleDbDataAdapter(cmd);
                dst_Life = new DataSet();
                oda.Fill(dst_Life);
                oda.Dispose();
                cmd.Cancel();
                mycon.Close();
                
                plan_holder = Convert.ToInt16(dst_Life.Tables[0].Rows[0]["Plan_Holder"]);
                illustration_id = Convert.ToInt32(dst_Life.Tables[0].Rows[0]["Illustration_Id"]);

            }
            catch
            { }


            try
            {
                ViewReportNew(illustration_id, App_no1, mid, Exclusion_path, plan_holder);
            }
            catch
            {

            }

            try
            {
                sqlquery = "";
                DataSet dst_Life = new DataSet();
                mycon = new OleDbConnection(conn);
                mycon.Open();
                sqlquery = "Select Plan_Holder,Joint_life,Illustration_Id from Application_Master where Application_Id=" + mid + "";
                cmd = new OleDbCommand(sqlquery, mycon);
                oda = new OleDbDataAdapter(cmd);
                dst_Life = new DataSet();
                oda.Fill(dst_Life);
                oda.Dispose();
                cmd.Cancel();
                mycon.Close();
                
                plan_holder = Convert.ToInt16(dst_Life.Tables[0].Rows[0]["Plan_Holder"]);
                illustration_id = Convert.ToInt32(dst_Life.Tables[0].Rows[0]["Illustration_Id"]);

                mycon.Open();
                sqlquery = "select Customer_Id from Application_Master where Application_Id = '" + mid + "'";
                cmd = new OleDbCommand(sqlquery, mycon);
                oda = new OleDbDataAdapter(cmd);
                dst = new DataSet();
                oda.Fill(dst);
                oda.Dispose();
                cmd.Cancel();
                mycon.Close();

                cust_id = dst.Tables[0].Rows[0]["Customer_Id"].ToString();
                app_id = Convert.ToInt32(mid);

            }
            catch
            { }


            DataSet dst_App_Details = new DataSet();
            DataTable dt_App_Details = new DataTable();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select A.* , B.* from Customer_Master A inner join Application_Master C ON C.Customer_Id = A.Customer_Id inner join Application_Occupation B ON C.Application_Id = B.Application_Id where A.Customer_type = '1' and B.Application_Id = '" + app_id + "' and B.Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Details);
                oledbAdapter.Dispose();
                MyConnection.Close();

                int row_count = dst_App_Details.Tables[0].Rows.Count;

                dt_App_Details.Clear();
                dt_App_Details.Columns.Add("name");
                dt_App_Details.Columns.Add("gender");
                dt_App_Details.Columns.Add("dob");
                dt_App_Details.Columns.Add("age");
                dt_App_Details.Columns.Add("habit");
                dt_App_Details.Columns.Add("residence");
                dt_App_Details.Columns.Add("nationality");
                dt_App_Details.Columns.Add("birthcountry");
                dt_App_Details.Columns.Add("maratialstatus");
                dt_App_Details.Columns.Add("idtype");
                dt_App_Details.Columns.Add("idno");
                dt_App_Details.Columns.Add("VisaNo");
                dt_App_Details.Columns.Add("natureofbusiness");
                dt_App_Details.Columns.Add("residency");
                dt_App_Details.Columns.Add("empname");
                //dt_App_Details.Columns.Add("empaddress");
                dt_App_Details.Columns.Add("pobox");
                dt_App_Details.Columns.Add("dailyduties");
                dt_App_Details.Columns.Add("occupation");
                dt_App_Details.Columns.Add("email");

                if (dst_App_Details.Tables[0].Rows.Count > 0)
                {
                    DataRow dr_App_Details = dt_App_Details.NewRow();
                    dr_App_Details["name"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["F_Name"]) + ' ' + Convert.ToString(dst_App_Details.Tables[0].Rows[0]["M_Name"]) + ' ' + Convert.ToString(dst_App_Details.Tables[0].Rows[0]["L_Name"]);

                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Gender"]) == "0")
                    {
                        dr_App_Details["gender"] = "Male";
                    }
                    else
                    {
                        dr_App_Details["gender"] = "Female";
                    }
                    string dob = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Dob"]);
                    dr_App_Details["dob"] = DateTime.Parse(dob).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                    dr_App_Details["age"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["age"]);
                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Habit"]) == "0")
                    {
                        dr_App_Details["habit"] = "Non - Smoker";

                    }
                    else
                    {
                        dr_App_Details["habit"] = "Smoker";
                    }


                    dr_App_Details["residence"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]);
                    dr_App_Details["nationality"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Nationality"]);
                    dr_App_Details["birthcountry"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["BirthCountry"]);
                    dr_App_Details["maratialstatus"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Marital_Status"]);

                    if (!String.IsNullOrEmpty(dst_App_Details.Tables[0].Rows[0]["ID_Type"].ToString()))
                    {
                        if (dst_App_Details.Tables[0].Rows[0]["ID_Type"].ToString() == "Passport")
                        {
                            dr_App_Details["idtype"] = "Passport";
                            dr_App_Details["idno"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["ID_No"]);
                            dr_App_Details["VisaNo"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Visa_No"]);
                        }
                        else
                        {
                            dr_App_Details["idtype"] = "Emirates Id";
                            dr_App_Details["idno"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["ID_No"]);
                            dr_App_Details["VisaNo"] = "";
                        }
                    }


                    dr_App_Details["natureofbusiness"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Nature"]);
                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]) == "UAE")
                    {
                        dr_App_Details["residency"] = "UAE";
                    }
                    else if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]) == "Saudi Arabia" || Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]) == "Qatar" || Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]) == "Bahrain" || Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]) == "Oman")
                    {
                        dr_App_Details["residency"] = "GCC";
                    }
                    else
                    {
                        dr_App_Details["residency"] = "Non - Resident";
                    }
                    dr_App_Details["empname"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["EmpAddress1"]) + ' ' + Convert.ToString(dst_App_Details.Tables[0].Rows[0]["EmpAddress2"]) + ' ' + Convert.ToString(dst_App_Details.Tables[0].Rows[0]["EmpAddress3"]);
                    dr_App_Details["pobox"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Pobox"]);
                    dr_App_Details["dailyduties"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Duties"]);
                    dr_App_Details["occupation"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Occupation"]);
                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Email"]) != "")
                    {
                        dr_App_Details["email"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Email"]);
                    }
                    else
                    {
                        dr_App_Details["email"] = "";
                    }
                    dt_App_Details.Rows.Add(dr_App_Details);
                }
            }
            catch { }
            
            // PLan Holder Other Insurance Details
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Insurance_History A inner join Application_Master B ON A.Application_Id = B.Application_Id where B.Plan_Holder = '1'  and A.Life = '3' and B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Other_Insurance_Details_PlanHolder);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }

            dt_App_Other_Insurance_Details_PlanHolder.Clear();
            dt_App_Other_Insurance_Details_PlanHolder.Columns.Add("companyname");
            dt_App_Other_Insurance_Details_PlanHolder.Columns.Add("plannumber");
            dt_App_Other_Insurance_Details_PlanHolder.Columns.Add("yearofissuance");
            dt_App_Other_Insurance_Details_PlanHolder.Columns.Add("sumcovered");
            dt_App_Other_Insurance_Details_PlanHolder.Columns.Add("contribution");
            dt_App_Other_Insurance_Details_PlanHolder.Columns.Add("standard");

            if (dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Other_Insurance_Details = dt_App_Other_Insurance_Details_PlanHolder.NewRow();
                    dr_App_Other_Insurance_Details["companyname"] = Convert.ToString(dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows[i]["Company"]);
                    dr_App_Other_Insurance_Details["plannumber"] = Convert.ToString(dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows[i]["Policy_No"]);
                    dr_App_Other_Insurance_Details["yearofissuance"] = Convert.ToString(dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows[i]["Year"]);
                    dr_App_Other_Insurance_Details["sumcovered"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows[i]["SumCover"]).Replace(",", string.Empty))).ToString("#,##0");       //Convert.ToString(dst_App_Other_Insurance_Details_2ndLife.Tables[0].Rows[i]["SumCover"]);
                    dr_App_Other_Insurance_Details["contribution"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows[i]["Contribution"]).Replace(",", string.Empty))).ToString("#,##0");    // Convert.ToString(dst_App_Other_Insurance_Details_2ndLife.Tables[0].Rows[i]["Contribution"]);
                    dr_App_Other_Insurance_Details["standard"] = Convert.ToString(dst_App_Other_Insurance_Details_PlanHolder.Tables[0].Rows[i]["InsType"]);
                    dt_App_Other_Insurance_Details_PlanHolder.Rows.Add(dr_App_Other_Insurance_Details);
                }
            }
            else
            {
                DataRow dr_App_Other_Insurance_Details = dt_App_Other_Insurance_Details_PlanHolder.NewRow();
                dr_App_Other_Insurance_Details["companyname"] = " ";
                dr_App_Other_Insurance_Details["plannumber"] = " ";
                dr_App_Other_Insurance_Details["yearofissuance"] = "";
                dr_App_Other_Insurance_Details["sumcovered"] = " ";
                dr_App_Other_Insurance_Details["contribution"] = " ";
                dr_App_Other_Insurance_Details["standard"] = " ";
                dt_App_Other_Insurance_Details_PlanHolder.Rows.Add(dr_App_Other_Insurance_Details);
            }
                       
            //Plan Holder details

            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select A.*, B.*,C.*  from Customer_Master A inner join Application_Master B ON B.Plan_Holder_Id = A.Customer_Id inner join Application_Occupation C ON C.Application_Id = B.Application_Id  where A.Customer_type = '3' and B.Application_Id = '" + app_id + "' and B.Plan_Holder = '1'";
                //Myquery = "Select A.* , B.* from Customer_Master A inner join Application_Master C ON C.Planholder_Id = A.Customer_Id inner join Application_Occupation B ON C.Application_Id = B.Application_Id where B.Application_Id = '" + app_id + "' and B.Life = '3' and A.Customer_type = '3'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Details_PlanHolder);
                oledbAdapter.Dispose();
                MyConnection.Close();

                int row_count = dst_App_Details_PlanHolder.Tables[0].Rows.Count;



                dt_App_Details_PlanHolder.Clear();
                dt_App_Details_PlanHolder.Columns.Add("name");
                dt_App_Details_PlanHolder.Columns.Add("gender");
                dt_App_Details_PlanHolder.Columns.Add("dob");
                dt_App_Details_PlanHolder.Columns.Add("age");
                dt_App_Details_PlanHolder.Columns.Add("habit");
                dt_App_Details_PlanHolder.Columns.Add("residence");
                dt_App_Details_PlanHolder.Columns.Add("nationality");
                dt_App_Details_PlanHolder.Columns.Add("birthcountry");
                dt_App_Details_PlanHolder.Columns.Add("maratialstatus");
                dt_App_Details_PlanHolder.Columns.Add("idtype");
                dt_App_Details_PlanHolder.Columns.Add("VisaNo");
                dt_App_Details_PlanHolder.Columns.Add("idno");
                dt_App_Details_PlanHolder.Columns.Add("natureofbusiness");
                dt_App_Details_PlanHolder.Columns.Add("residency");
                dt_App_Details_PlanHolder.Columns.Add("empname");
                dt_App_Details_PlanHolder.Columns.Add("email");
                dt_App_Details_PlanHolder.Columns.Add("pobox");
                dt_App_Details_PlanHolder.Columns.Add("dailyduties");
                dt_App_Details_PlanHolder.Columns.Add("occupation");

                if (dst_App_Details_PlanHolder.Tables[0].Rows.Count > 0)
                {
                    DataRow dr_App_Details_PlanHolder = dt_App_Details_PlanHolder.NewRow();
                    dr_App_Details_PlanHolder["name"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["F_Name"]) + ' ' + Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["M_Name"]) + ' ' + Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["L_Name"]);

                    if (Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Gender"]) == "0")
                    {
                        dr_App_Details_PlanHolder["gender"] = "Male";
                    }
                    else
                    {
                        dr_App_Details_PlanHolder["gender"] = "Female";
                    }

                    string dobPlanHolder = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Dob"]);
                    dr_App_Details_PlanHolder["dob"] = DateTime.Parse(dobPlanHolder).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    dr_App_Details_PlanHolder["age"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["age"]);
                    if (Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Habit"]) == "0")
                    {
                        dr_App_Details_PlanHolder["habit"] = "Non - Smoker";

                    }
                    else
                    {
                        dr_App_Details_PlanHolder["habit"] = " Smoker";
                    }


                    dr_App_Details_PlanHolder["residence"] = "";
                    dr_App_Details_PlanHolder["nationality"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Nationality"]);
                    dr_App_Details_PlanHolder["email"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Email"]);
                    dr_App_Details_PlanHolder["birthcountry"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["BirthCountry"]);
                    dr_App_Details_PlanHolder["maratialstatus"] = "";
                    if (!String.IsNullOrEmpty(dst_App_Details_PlanHolder.Tables[0].Rows[0]["ID_Type"].ToString()))
                    {
                        if (dst_App_Details_PlanHolder.Tables[0].Rows[0]["ID_Type"].ToString() == "Passport")
                        {
                            dr_App_Details_PlanHolder["idtype"] = "Passport";
                            dr_App_Details_PlanHolder["idno"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["ID_No"]);
                            dr_App_Details_PlanHolder["VisaNo"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Visa_No"]);
                        }
                        else
                        {
                            dr_App_Details_PlanHolder["idtype"] = "Emirates Id";
                            dr_App_Details_PlanHolder["idno"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["ID_No"]);
                        }
                    }

                    dr_App_Details_PlanHolder["natureofbusiness"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Nature"]);
                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Resident"]) == "UAE")
                    {
                        dr_App_Details_PlanHolder["residency"] = "UAE";
                    }
                    else if (Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Resident"]) == "Saudi Arabia" || Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Resident"]) == "Qatar" || Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Resident"]) == "Bahrain" || Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Resident"]) == "Oman")
                    {
                        dr_App_Details_PlanHolder["residency"] = "GCC";
                    }
                    else
                    {
                        dr_App_Details_PlanHolder["residency"] = "Non - Resident";
                    }
                    dr_App_Details_PlanHolder["empname"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["EmpAddress1"]) + ' ' + Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["EmpAddress2"]) + ' ' + Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["EmpAddress3"]); ;
                    dr_App_Details_PlanHolder["pobox"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Pobox"]);
                    ;
                    dr_App_Details_PlanHolder["dailyduties"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Duties"]);
                    ;
                    dr_App_Details_PlanHolder["occupation"] = Convert.ToString(dst_App_Details_PlanHolder.Tables[0].Rows[0]["Occupation"]);
                    dt_App_Details_PlanHolder.Rows.Add(dr_App_Details_PlanHolder);
                }
                else
                {
                    DataRow dr_App_Details_PlanHolder = dt_App_Details_PlanHolder.NewRow();
                    dr_App_Details_PlanHolder["name"] = "";
                    dr_App_Details_PlanHolder["gender"] = "";
                    dr_App_Details_PlanHolder["dob"] = "";
                    dr_App_Details_PlanHolder["age"] = "";
                    dr_App_Details_PlanHolder["habit"] = "";
                    dr_App_Details_PlanHolder["residence"] = "";
                    dr_App_Details_PlanHolder["nationality"] = "";
                    dr_App_Details_PlanHolder["birthcountry"] = "";
                    dr_App_Details_PlanHolder["maratialstatus"] = "";
                    dr_App_Details_PlanHolder["idno"] = "";
                    dr_App_Details_PlanHolder["natureofbusiness"] = "";
                    dr_App_Details_PlanHolder["residency"] = "";
                    dr_App_Details_PlanHolder["residency"] = "";
                    dr_App_Details_PlanHolder["residency"] = "";
                    dr_App_Details_PlanHolder["empname"] = "";
                    dr_App_Details_PlanHolder["pobox"] = "";
                    dr_App_Details_PlanHolder["dailyduties"] = "";
                    dr_App_Details_PlanHolder["occupation"] = "";
                    dt_App_Details_PlanHolder.Rows.Add(dr_App_Details_PlanHolder);
                }


            }
            catch { }


            try
            {
                // Plan Holder contact details

                try
                {
                    OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                    OleDbDataAdapter oledbAdapter1;
                    if (MyConnection.State == ConnectionState.Open)
                    {
                        MyConnection.Close();
                    }

                    MyConnection.Open();
                    string Myquery = "";
                    Myquery = "Select * from  Application_Contact where Life = '3' and Application_Id=" + app_id + "";
                    oledbAdapter1 = new OleDbDataAdapter(Myquery, MyConnection);
                    oledbAdapter1.Fill(dst_App_Contact_PlanHolder);
                    oledbAdapter1.Dispose();
                    MyConnection.Close();
                    //int dst_App_Contact_2ndLife_count = dst_App_Contact_PlanHolder.Tables[0].Rows.Count;
                }
                catch { }


                dt_App_Contact_PlanHolder.Clear();
                dt_App_Contact_PlanHolder.Columns.Add("apartment");
                dt_App_Contact_PlanHolder.Columns.Add("building");
                dt_App_Contact_PlanHolder.Columns.Add("street");
                dt_App_Contact_PlanHolder.Columns.Add("city");
                dt_App_Contact_PlanHolder.Columns.Add("country");
                dt_App_Contact_PlanHolder.Columns.Add("phone");
                dt_App_Contact_PlanHolder.Columns.Add("fax");
                dt_App_Contact_PlanHolder.Columns.Add("mobile");
                dt_App_Contact_PlanHolder.Columns.Add("email");
                dt_App_Contact_PlanHolder.Columns.Add("pobox");

                try
                {
                    if (dst_App_Contact_PlanHolder.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr_App_Contact_PlanHolder = dt_App_Contact_PlanHolder.NewRow();

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Apartment"]) != "")
                        {
                            dr_App_Contact_PlanHolder["apartment"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Apartment"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["apartment"] = "";
                        }
                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Building"]) != "")
                        {
                            dr_App_Contact_PlanHolder["building"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Building"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["building"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Street"]) != "")
                        {
                            dr_App_Contact_PlanHolder["street"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Street"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["street"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["City"]) != "")
                        {
                            dr_App_Contact_PlanHolder["city"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["City"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["city"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Country"]) != "")
                        {
                            dr_App_Contact_PlanHolder["country"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Country"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["country"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Phone"]) != "")
                        {
                            dr_App_Contact_PlanHolder["phone"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Phone"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["phone"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Fax"]) != "")
                        {
                            dr_App_Contact_PlanHolder["fax"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Fax"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["fax"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Mobile"]) != "")
                        {
                            dr_App_Contact_PlanHolder["mobile"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Mobile"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["mobile"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Email"]) != "")
                        {
                            dr_App_Contact_PlanHolder["email"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Email"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["email"] = "";
                        }

                        if (Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Pobox"]) != "")
                        {
                            dr_App_Contact_PlanHolder["pobox"] = Convert.ToString(dst_App_Contact_PlanHolder.Tables[0].Rows[0]["Pobox"]);
                        }
                        else
                        {
                            dr_App_Contact_PlanHolder["pobox"] = "";
                        }


                        dt_App_Contact_PlanHolder.Rows.Add(dr_App_Contact_PlanHolder);
                    }
                    else
                    {
                        DataRow dr_App_Contact_PlanHolder = dt_App_Contact_PlanHolder.NewRow();

                        dr_App_Contact_PlanHolder["apartment"] = "";

                        dr_App_Contact_PlanHolder["building"] = "";

                        dr_App_Contact_PlanHolder["street"] = "";

                        dr_App_Contact_PlanHolder["city"] = "";

                        dr_App_Contact_PlanHolder["country"] = "";

                        dr_App_Contact_PlanHolder["phone"] = "";

                        dr_App_Contact_PlanHolder["fax"] = "";

                        dr_App_Contact_PlanHolder["mobile"] = "";

                        dr_App_Contact_PlanHolder["email"] = "";

                        dt_App_Contact_PlanHolder.Rows.Add(dr_App_Contact_PlanHolder);
                    }
                }
                catch { }
            }
            catch
            {

            }

            //Plan Holder permanent address

            //int App_Id = 26; Convert.ToInt32(lblillustrationId.Text);
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select * from  Application_Contact where Type = '2' and Life = '3' and Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_app_permanent_PlanHolder);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }

            //int row_count = dst_App_Details.Tables[0].Rows.Count;                
            dt_App_Permanent_PlanHolder.Clear();
            dt_App_Permanent_PlanHolder.Columns.Add("apartment");
            dt_App_Permanent_PlanHolder.Columns.Add("building");
            dt_App_Permanent_PlanHolder.Columns.Add("street");
            dt_App_Permanent_PlanHolder.Columns.Add("city");
            dt_App_Permanent_PlanHolder.Columns.Add("country");
            dt_App_Permanent_PlanHolder.Columns.Add("pobox");

            try
            {
                if (dst_app_permanent_PlanHolder.Tables[0].Rows.Count > 0)
                {
                    DataRow dr_App_Permanent_PlanHolder = dt_App_Permanent_PlanHolder.NewRow();
                    if (Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Apartment"]) != "")
                    {
                        dr_App_Permanent_PlanHolder["apartment"] = Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Apartment"]);
                    }
                    else
                    {
                        dr_App_Permanent_PlanHolder["apartment"] = "";
                    }
                    if (Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Building"]) != "")
                    {
                        dr_App_Permanent_PlanHolder["building"] = Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Building"]);
                    }
                    else
                    {
                        dr_App_Permanent_PlanHolder["building"] = "";
                    }
                    if (Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Street"]) != "")
                    {
                        dr_App_Permanent_PlanHolder["street"] = Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Street"]);
                    }
                    else
                    {
                        dr_App_Permanent_PlanHolder["street"] = "";
                    }
                    if (Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["City"]) != "")
                    {
                        dr_App_Permanent_PlanHolder["city"] = Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["City"]);
                    }
                    else
                    {
                        dr_App_Permanent_PlanHolder["city"] = "";
                    }
                    if (Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Country"]) != "")
                    {
                        dr_App_Permanent_PlanHolder["country"] = Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Country"]);
                    }
                    else
                    {
                        dr_App_Permanent_PlanHolder["country"] = "";
                    }
                    if (Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Pobox"]) != "")
                    {
                        dr_App_Permanent_PlanHolder["pobox"] = Convert.ToString(dst_app_permanent_PlanHolder.Tables[0].Rows[0]["Pobox"]);
                    }
                    else
                    {
                        dr_App_Permanent_PlanHolder["pobox"] = "";
                    }
                    dt_App_Permanent_PlanHolder.Rows.Add(dr_App_Permanent_PlanHolder);
                }
                else
                {
                    DataRow dr_App_Permanent_PlanHolder = dt_App_Permanent_PlanHolder.NewRow();

                    dr_App_Permanent_PlanHolder["apartment"] = "";

                    dr_App_Permanent_PlanHolder["building"] = "";

                    dr_App_Permanent_PlanHolder["street"] = "";

                    dr_App_Permanent_PlanHolder["city"] = "";

                    dr_App_Permanent_PlanHolder["country"] = "";

                    dr_App_Permanent_PlanHolder["pobox"] = "";

                    dt_App_Permanent_PlanHolder.Rows.Add(dr_App_Permanent_PlanHolder);
                }
            }
            catch
            {

            }

            //Plan holder and source of funds

            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select * from Application_Bank_Source A inner join Application_Master B ON A.Application_Id = B.Application_Id where B.Plan_Holder = '1'  and A.Life = '3' and B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Fund_PlanHolder);
                oledbAdapter.Dispose();
                MyConnection.Close();


                dt_App_Fund_PlanHolder.Clear();
                dt_App_Fund_PlanHolder.Columns.Add("bankname");
                dt_App_Fund_PlanHolder.Columns.Add("iban");
                dt_App_Fund_PlanHolder.Columns.Add("source");

                if (dst_App_Fund_PlanHolder.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= dst_App_Fund_PlanHolder.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow dr_App_Fund_PlanHolder = dt_App_Fund_PlanHolder.NewRow();
                        dr_App_Fund_PlanHolder["bankname"] = Convert.ToString(dst_App_Fund_PlanHolder.Tables[0].Rows[i]["Bank_Name"]);
                        dr_App_Fund_PlanHolder["iban"] = Convert.ToString(dst_App_Fund_PlanHolder.Tables[0].Rows[i]["IBAN"]);
                        dr_App_Fund_PlanHolder["source"] = Convert.ToString(dst_App_Fund_PlanHolder.Tables[0].Rows[i]["Source"]);
                        dt_App_Fund_PlanHolder.Rows.Add(dr_App_Fund_PlanHolder);
                    }

                }
                else
                {
                    DataRow dr_App_Fund_PlanHolder = dt_App_Fund_PlanHolder.NewRow();
                    dr_App_Fund_PlanHolder["bankname"] = "";
                    dr_App_Fund_PlanHolder["iban"] = "";
                    dr_App_Fund_PlanHolder["source"] = "";
                    dt_App_Fund_PlanHolder.Rows.Add(dr_App_Fund_PlanHolder);
                }
            }
            catch
            { }


            // Plan Holder Assets
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Assest A inner join Application_Master B ON A.Application_Id = B.Application_Id where B.Plan_Holder = '1'  and A.Life = '3' and B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Assets_PlanHolder);
                oledbAdapter.Dispose();
                MyConnection.Close();


                dt_App_Assets_PlanHolder.Clear();
                dt_App_Assets_PlanHolder.Columns.Add("name");
                dt_App_Assets_PlanHolder.Columns.Add("currency");
                dt_App_Assets_PlanHolder.Columns.Add("amount");
                dt_App_Assets_PlanHolder.Columns.Add("remarks");

                if (dst_App_Assets_PlanHolder.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= dst_App_Assets_PlanHolder.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow dr_App_Assets_PlanHolder = dt_App_Assets_PlanHolder.NewRow();
                        dr_App_Assets_PlanHolder["name"] = Convert.ToString(dst_App_Assets_PlanHolder.Tables[0].Rows[i]["Name"]);
                        dr_App_Assets_PlanHolder["currency"] = Convert.ToString(dst_App_Assets_PlanHolder.Tables[0].Rows[i]["Currency"]);
                        dr_App_Assets_PlanHolder["amount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Assets_PlanHolder.Tables[0].Rows[i]["Amount"]).Replace(",", string.Empty))).ToString("#,##0");
                        if (Convert.ToString(dst_App_Assets_PlanHolder.Tables[0].Rows[i]["Remarks"]) != "")
                        {
                            dr_App_Assets_PlanHolder["remarks"] = Convert.ToString(dst_App_Assets_PlanHolder.Tables[0].Rows[i]["Remarks"]);
                        }
                        else
                        {
                            dr_App_Assets_PlanHolder["remarks"] = "";
                        }

                        dt_App_Assets_PlanHolder.Rows.Add(dr_App_Assets_PlanHolder);
                    }
                }
                else
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        DataRow dr_App_Assets_PlanHolder = dt_App_Assets_PlanHolder.NewRow();
                        dr_App_Assets_PlanHolder["name"] = "";
                        dr_App_Assets_PlanHolder["currency"] = "";
                        dr_App_Assets_PlanHolder["amount"] = "";
                        dr_App_Assets_PlanHolder["remarks"] = "";
                        dt_App_Assets_PlanHolder.Rows.Add(dr_App_Assets_PlanHolder);
                    } 
                }
            }
            catch { }


            //Plan Holder Liability

            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Liability A inner join Application_Master B ON A.Application_Id = B.Application_Id where B.Plan_Holder = '1'  and A.Life = '3' and B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Liability_PlanHolder);
                oledbAdapter.Dispose();
                MyConnection.Close();


                dt_App_Liability_PlanHolder.Clear();
                dt_App_Liability_PlanHolder.Columns.Add("name");
                dt_App_Liability_PlanHolder.Columns.Add("currency");
                dt_App_Liability_PlanHolder.Columns.Add("amount");
                dt_App_Liability_PlanHolder.Columns.Add("remarks");

                if (dst_App_Liability_PlanHolder.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= dst_App_Liability_PlanHolder.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow dr_App_Liability_PlanHolder = dt_App_Liability_PlanHolder.NewRow();
                        dr_App_Liability_PlanHolder["name"] = Convert.ToString(dst_App_Liability_PlanHolder.Tables[0].Rows[i]["Name"]);
                        dr_App_Liability_PlanHolder["currency"] = Convert.ToString(dst_App_Liability_PlanHolder.Tables[0].Rows[i]["Currency"]);
                        dr_App_Liability_PlanHolder["amount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Liability_PlanHolder.Tables[0].Rows[i]["Amount"]).Replace(",", string.Empty))).ToString("#,##0");//Convert.ToString();
                        if (Convert.ToString(dst_App_Liability_PlanHolder.Tables[0].Rows[i]["Remarks"]) != "")
                        {
                            dr_App_Liability_PlanHolder["remarks"] = Convert.ToString(dst_App_Liability_PlanHolder.Tables[0].Rows[i]["Remarks"]);
                        }
                        else
                        {
                            dr_App_Liability_PlanHolder["remarks"] = "";
                        }
                        dt_App_Liability_PlanHolder.Rows.Add(dr_App_Liability_PlanHolder);
                    }
                }
                else
                {
                    DataRow dr_App_Liability_PlanHolder = dt_App_Liability_PlanHolder.NewRow();
                    dr_App_Liability_PlanHolder["name"] = "";
                    dr_App_Liability_PlanHolder["currency"] = "";
                    dr_App_Liability_PlanHolder["amount"] = "";
                    dr_App_Liability_PlanHolder["remarks"] = "";
                    dt_App_Liability_PlanHolder.Rows.Add(dr_App_Liability_PlanHolder);
                }
            }
            catch { }



            //Correspondence address
            DataSet dst_App_Contact = new DataSet();
            DataTable dt_App_Contact = new DataTable();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter1;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select * from  Application_Contact where Type = 1 and Application_Id=" + app_id + " and Life = '1'";
                oledbAdapter1 = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter1.Fill(dst_App_Contact);
                oledbAdapter1.Dispose();
                MyConnection.Close();
                int dst_App_Contact_Details_count = dst_App_Contact.Tables[0].Rows.Count;
            }
            catch { }


            dt_App_Contact.Clear();
            dt_App_Contact.Columns.Add("apartment");
            dt_App_Contact.Columns.Add("building");
            dt_App_Contact.Columns.Add("street");
            dt_App_Contact.Columns.Add("city");
            dt_App_Contact.Columns.Add("country");
            dt_App_Contact.Columns.Add("phone");
            dt_App_Contact.Columns.Add("fax");
            dt_App_Contact.Columns.Add("mobile");
            dt_App_Contact.Columns.Add("email");
            dt_App_Contact.Columns.Add("pobox");

            try
            {
                if (dst_App_Contact.Tables[0].Rows.Count > 0)
                {
                    DataRow dr_App_Contact = dt_App_Contact.NewRow();

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Apartment"]) != "")
                    {
                        dr_App_Contact["apartment"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Apartment"]);
                    }
                    else
                    {
                        dr_App_Contact["apartment"] = "";
                    }
                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Building"]) != "")
                    {
                        dr_App_Contact["building"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Building"]);
                    }
                    else
                    {
                        dr_App_Contact["building"] = "";
                    }

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Street"]) != "")
                    {
                        dr_App_Contact["street"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Street"]);
                    }
                    else
                    {
                        dr_App_Contact["street"] = "";
                    }

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["City"]) != "")
                    {
                        dr_App_Contact["city"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["City"]);
                    }
                    else
                    {
                        dr_App_Contact["city"] = "";
                    }

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Country"]) != "")
                    {
                        dr_App_Contact["country"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Country"]);
                    }
                    else
                    {
                        dr_App_Contact["country"] = "";
                    }

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Phone"]) != "")
                    {
                        dr_App_Contact["phone"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Phone"]);
                    }
                    else
                    {
                        dr_App_Contact["phone"] = "";
                    }

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Fax"]) != "")
                    {
                        dr_App_Contact["fax"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Fax"]);
                    }
                    else
                    {
                        dr_App_Contact["fax"] = "";
                    }

                    if (Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Mobile"]) != "")
                    {
                        dr_App_Contact["mobile"] = Convert.ToString(dst_App_Contact.Tables[0].Rows[0]["Mobile"]);
                    }
                    else
                    {
                        dr_App_Contact["mobile"] = "";
                    }

                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Email"]) != "")
                    {
                        dr_App_Contact["email"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Email"]);
                    }
                    else
                    {
                        dr_App_Contact["email"] = "";
                    }

                    if (Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Pobox"]) != "")
                    {
                        dr_App_Contact["pobox"] = Convert.ToString(dst_App_Details.Tables[0].Rows[0]["Pobox"]);
                    }
                    else
                    {
                        dr_App_Contact["pobox"] = "";
                    }

                    dt_App_Contact.Rows.Add(dr_App_Contact);
                }
            }
            catch { }


            //permanent address
            DataSet dst_App_Permanent = new DataSet();
            //int App_Id = 26; Convert.ToInt32(lblillustrationId.Text);
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select * from  Application_Contact where Type = 2 and Application_Id=" + app_id + " and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Permanent);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }

            //int row_count = dst_App_Details.Tables[0].Rows.Count;

            DataTable dt_App_Permanent = new DataTable();
            dt_App_Permanent.Clear();
            dt_App_Permanent.Columns.Add("apartment");
            dt_App_Permanent.Columns.Add("building");
            dt_App_Permanent.Columns.Add("street");
            dt_App_Permanent.Columns.Add("city");
            dt_App_Permanent.Columns.Add("country");
            dt_App_Permanent.Columns.Add("pobox");

            try
            {
                if (dst_App_Permanent.Tables[0].Rows.Count > 0)
                {
                    DataRow dr_App_Permanent = dt_App_Permanent.NewRow();
                    if (Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Apartment"]) != "")
                    {
                        dr_App_Permanent["apartment"] = Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Apartment"]);
                    }
                    else
                    {
                        dr_App_Permanent["apartment"] = "";
                    }
                    if (Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Building"]) != "")
                    {
                        dr_App_Permanent["building"] = Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Building"]);
                    }
                    else
                    {
                        dr_App_Permanent["building"] = "";
                    }
                    if (Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Street"]) != "")
                    {
                        dr_App_Permanent["street"] = Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Street"]);
                    }
                    else
                    {
                        dr_App_Permanent["street"] = "";
                    }
                    if (Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["City"]) != "")
                    {
                        dr_App_Permanent["city"] = Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["City"]);
                    }
                    else
                    {
                        dr_App_Permanent["city"] = "";
                    }
                    if (Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Country"]) != "")
                    {
                        dr_App_Permanent["country"] = Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Country"]);
                    }
                    else
                    {
                        dr_App_Permanent["country"] = "";
                    }
                    if (Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Pobox"]) != "")
                    {
                        dr_App_Permanent["pobox"] = Convert.ToString(dst_App_Permanent.Tables[0].Rows[0]["Pobox"]);
                    }
                    else
                    {
                        dr_App_Permanent["pobox"] = "";
                    }
                    dt_App_Permanent.Rows.Add(dr_App_Permanent);
                }
            }
            catch
            {

            }

            //FATCA
            DataSet dst_FATCA = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select CRS1,TINUSA from  Application_FATCA where Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_FATCA);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_FATCA = new DataTable();
            dt_FATCA.Clear();
            dt_FATCA.Columns.Add("Q1");
            dt_FATCA.Columns.Add("TIN");
            DataRow dr_FATCA = dt_FATCA.NewRow();

            if (dst_FATCA.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(dst_FATCA.Tables[0].Rows[0]["CRS1"]) == "1")
                {
                    dr_FATCA["Q1"] = "YES";
                }
                else
                {
                    dr_FATCA["Q1"] = "NO";
                }


                if (String.IsNullOrEmpty(dst_FATCA.Tables[0].Rows[0]["TINUSA"].ToString()))
                {
                    dr_FATCA["TIN"] = "NO";
                }
                else
                {
                    dr_FATCA["TIN"] = Convert.ToString(dst_FATCA.Tables[0].Rows[0]["TINUSA"]);

                }
                dt_FATCA.Rows.Add(dr_FATCA);
            }

            else
            {
                dr_FATCA["Q1"] = "NO";
                dr_FATCA["TIN"] = "NO";
                dt_FATCA.Rows.Add(dr_FATCA);
            }

            //CRS
            DataSet dst_CRS = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select * from  Application_CRS where Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_CRS);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_CRS = new DataTable();
            dt_CRS.Clear();
            dt_CRS.Columns.Add("country");
            dt_CRS.Columns.Add("TIN");


            if (dst_CRS.Tables[0].Rows.Count > 0)
            {
                DataRow dr_CRS = dt_CRS.NewRow();
                dr_CRS["Country"] = Convert.ToString(dst_CRS.Tables[0].Rows[0]["Country"]); ;
                dr_CRS["TIN"] = Convert.ToString(dst_CRS.Tables[0].Rows[0]["TIN"]); ;
                dt_CRS.Rows.Add(dr_CRS);
            }
            else
            {
                DataRow dr_CRS = dt_CRS.NewRow();
                dr_CRS["Country"] = " ";
                dr_CRS["TIN"] = " ";
                dt_CRS.Rows.Add(dr_CRS);
            }


            // 1st life Benefits
            DataSet dst_App_Benefit_Details = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select A.Payment_Term, A.Currency,A.Plan_Code from Illustration_Master A inner join Application_Master B ON A.Illustration_Id = B.Illustration_Id where B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Benefit_Details);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }


            DataTable dt_App_Benefit_Details = new DataTable();
            dt_App_Benefit_Details.Clear();
            dt_App_Benefit_Details.Columns.Add("PlanTerm");
            dt_App_Benefit_Details.Columns.Add("PlanCurrency");
            dt_App_Benefit_Details.Columns.Add("PlanSelected");

            if (dst_App_Benefit_Details.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Benefit_Details.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Benefit_Details = dt_App_Benefit_Details.NewRow();
                    dr_App_Benefit_Details["PlanTerm"] = Convert.ToString(dst_App_Benefit_Details.Tables[0].Rows[i]["Payment_Term"]);
                    dr_App_Benefit_Details["PlanCurrency"] = Convert.ToString(dst_App_Benefit_Details.Tables[0].Rows[i]["Currency"]);

                    if (Convert.ToString(dst_App_Benefit_Details.Tables[0].Rows[i]["Plan_Code"]).Trim() == "SP+")

                    {
                        dr_App_Benefit_Details["PlanSelected"] = "IDIKHAR PLUS";

                    }
                    
                    dt_App_Benefit_Details.Rows.Add(dr_App_Benefit_Details);
                }


            }

            DataSet dst_App_Benefits = new DataSet();
            DataSet dst_App_Terminalillness_Life1 = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select  * from Illustration_Rider A inner join Application_Master B ON A.illustration_Id = B.Illustration_Id where A.status=1 and A.life=1 and B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Benefits);
                oledbAdapter.Dispose();
                MyConnection.Close();

                MyConnection.Open();
                string Myquery2 = "";
                Myquery2 = " select Sum_Cover from Illustration_Master A inner join Application_Master B ON A.Illustration_Id = B.Illustration_Id where B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery2, MyConnection);
                oledbAdapter.Fill(dst_App_Terminalillness_Life1);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Benefits = new DataTable();
            dt_App_Benefits.Clear();
            dt_App_Benefits.Columns.Add("name");
            dt_App_Benefits.Columns.Add("amount");            

            if (dst_App_Benefits.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Benefits.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Benefits = dt_App_Benefits.NewRow();
                    dr_App_Benefits["name"] = Convert.ToString(dst_App_Benefits.Tables[0].Rows[i]["Name"]);
                    if (String.IsNullOrEmpty(dst_App_Benefits.Tables[0].Rows[i]["Amount"].ToString()))
                    {
                        dr_App_Benefits["amount"] = "";
                    }
                    else
                    {
                        dr_App_Benefits["amount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Benefits.Tables[0].Rows[i]["Amount"]).Replace(",", string.Empty))).ToString("#,##0");                           //Convert.ToString();

                    }

                    dt_App_Benefits.Rows.Add(dr_App_Benefits);
                }



            }
            
            //Guardian details
            DataSet dst_App_Guardian = new DataSet();
            DataTable dt_App_Guardian = new DataTable();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select  * from Application_Guardian where life= '1' and application_id= '" + app_id + "'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Guardian);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }

            dt_App_Guardian.Clear();
            dt_App_Guardian.Columns.Add("name");
            dt_App_Guardian.Columns.Add("age");
            dt_App_Guardian.Columns.Add("passport");
            dt_App_Guardian.Columns.Add("relation");

            if (dst_App_Guardian.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Guardian.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Guardian = dt_App_Guardian.NewRow();
                    dr_App_Guardian["name"] = Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["Name"]);
                    dr_App_Guardian["age"] = Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["Age"]);
                    dr_App_Guardian["passport"] = Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["PassportNumber"]);
                    dr_App_Guardian["relation"] = Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["Relationship"]);
                    dt_App_Guardian.Rows.Add(dr_App_Guardian);
                }
            }
            else
            {
                DataRow dr_App_Guardian = dt_App_Guardian.NewRow();
                dr_App_Guardian["name"] = " NA ";//Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["Name"]);
                dr_App_Guardian["age"] = " NA "; //Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["Age"]);
                dr_App_Guardian["passport"] = " NA "; //Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["PassportNumber"]);
                dr_App_Guardian["relation"] = " NA ";//Convert.ToString(dst_App_Guardian.Tables[0].Rows[i]["Relationship"]);
                dt_App_Guardian.Rows.Add(dr_App_Guardian);
            }
            
            //Contribution
            DataSet dst_app_contribution = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select  A.* , B.Contribution_Mode from Illustration_Master A inner join Application_Master B ON A.Illustration_Id = B.Illustration_Id where B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_app_contribution);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_app_contribution = new DataTable();
            dt_app_contribution.Clear();
            dt_app_contribution.Columns.Add("amount");
            dt_app_contribution.Columns.Add("mode");
            dt_app_contribution.Columns.Add("method");
            dt_app_contribution.Columns.Add("term");

            DataRow dr_app_contribution = dt_app_contribution.NewRow();
            dr_app_contribution["amount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Contribution"]).Replace(",", string.Empty))).ToString("#,##0");//Convert.ToString();                           //Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Contribution"]);
            if (Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Frequency"]) == "12")
            {
                dr_app_contribution["mode"] = "Monthly";
            }
            if (Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Frequency"]) == "6")
            {
                dr_app_contribution["mode"] = "Half - Yearly";
            }
            if (Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Frequency"]) == "4")
            {
                dr_app_contribution["mode"] = "Quarterly";
            }
            if (Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Frequency"]) == "1")
            {
                dr_app_contribution["mode"] = "Yearly";
            }

            dr_app_contribution["method"] = Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Contribution_Mode"]);
            dr_app_contribution["term"] = Convert.ToString(dst_app_contribution.Tables[0].Rows[0]["Payment_Term"]);
            dt_app_contribution.Rows.Add(dr_app_contribution);

            //Investment
            DataSet dst_App_Investment = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select Fund_Code , Share from Application_Fund where Application_Id = " + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Investment);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Investment = new DataTable();
            dt_App_Investment.Clear();
            dt_App_Investment.Columns.Add("fundname");
            dt_App_Investment.Columns.Add("percentage");

            if (dst_App_Investment.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Investment.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Investment = dt_App_Investment.NewRow();
                    dr_App_Investment["fundname"] = Convert.ToString(dst_App_Investment.Tables[0].Rows[i]["Fund_Code"]);
                    dr_App_Investment["percentage"] = Convert.ToString(dst_App_Investment.Tables[0].Rows[i]["Share"]);
                    dt_App_Investment.Rows.Add(dr_App_Investment);
                }

            }
            else
            {
                DataRow dr_App_Investment = dt_App_Investment.NewRow();
                dr_App_Investment["fundname"] = " ";
                dr_App_Investment["percentage"] = " ";
                dt_App_Investment.Rows.Add(dr_App_Investment);
            }



            //Bank and source of funds
            DataSet dst_App_Fund = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Bank_Source where Application_Id = '" + app_id + "' and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Fund);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }

            DataTable dt_App_Fund = new DataTable();
            dt_App_Fund.Clear();
            dt_App_Fund.Columns.Add("bankname");
            dt_App_Fund.Columns.Add("iban");
            dt_App_Fund.Columns.Add("source");

            if (dst_App_Fund.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Fund.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Fund = dt_App_Fund.NewRow();
                    dr_App_Fund["bankname"] = Convert.ToString(dst_App_Fund.Tables[0].Rows[i]["Bank_Name"]);
                    dr_App_Fund["iban"] = Convert.ToString(dst_App_Fund.Tables[0].Rows[i]["IBAN"]);
                    dr_App_Fund["source"] = Convert.ToString(dst_App_Fund.Tables[0].Rows[i]["Source"]);
                    dt_App_Fund.Rows.Add(dr_App_Fund);
                }

            }

            //Last 3 incomes
            DataSet dst_App_Income = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Occupation where Application_Id = " + app_id + " and Life= '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Income);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }

            

            DataSet dst_App_Income_PlanHolder = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter3;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Occupation where Application_Id = " + app_id + " and Life = '3'";
                oledbAdapter3 = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter3.Fill(dst_App_Income_PlanHolder);
                oledbAdapter3.Dispose();
                MyConnection.Close();
            }
            catch { }

            DataTable dt_App_Income = new DataTable();
            dt_App_Income.Clear();
            dt_App_Income.Columns.Add("Income1");
            dt_App_Income.Columns.Add("Income2");
            dt_App_Income.Columns.Add("Income3");
            dt_App_Income.Columns.Add("Year1");
            dt_App_Income.Columns.Add("Year2");
            dt_App_Income.Columns.Add("Year3");            

            DataTable dt_App_PlanHolderIncome = new DataTable();
            dt_App_PlanHolderIncome.Columns.Add("PlanHolderIncome1");
            dt_App_PlanHolderIncome.Columns.Add("PlanHolderIncome2");
            dt_App_PlanHolderIncome.Columns.Add("PlanHolderIncome3");

            DataRow dr_App_income = dt_App_Income.NewRow();
            if (String.IsNullOrEmpty(dst_App_Income.Tables[0].Rows[0]["Income1"].ToString()))
            {
                dr_App_income["Income1"] = "";
            }
            else
            {
                dr_App_income["Income1"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]).Replace(",", string.Empty))).ToString("#,##0");     // Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]);
            }

            if (String.IsNullOrEmpty(dst_App_Income.Tables[0].Rows[0]["Income2"].ToString()))
            {
                dr_App_income["Income2"] = "";
            }
            else
            {
                dr_App_income["Income2"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income2"]).Replace(",", string.Empty))).ToString("#,##0");     // Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]);
            }

            if (String.IsNullOrEmpty(dst_App_Income.Tables[0].Rows[0]["Income3"].ToString()))
            {
                dr_App_income["Income3"] = "";
            }
            else
            {
                dr_App_income["Income3"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income3"]).Replace(",", string.Empty))).ToString("#,##0");     // Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]);
            }
            dr_App_income["Year1"] = dst_App_Income.Tables[0].Rows[0]["IncomeYear1"].ToString();
            dr_App_income["Year2"] = dst_App_Income.Tables[0].Rows[0]["IncomeYear2"].ToString();
            dr_App_income["Year3"] = dst_App_Income.Tables[0].Rows[0]["IncomeYear3"].ToString();
            dt_App_Income.Rows.Add(dr_App_income);

            
            //PLanholder Income
            if (dst_App_Income_PlanHolder.Tables[0].Rows.Count > 0)
            {
                DataRow dr_App_PlanHolderIncome = dt_App_PlanHolderIncome.NewRow();
                if (String.IsNullOrEmpty(dst_App_Income_PlanHolder.Tables[0].Rows[0]["Income1"].ToString()))
                {
                    dr_App_PlanHolderIncome["PlanHolderIncome1"] = "";
                }
                else
                {
                    dr_App_PlanHolderIncome["PlanHolderIncome1"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Income_PlanHolder.Tables[0].Rows[0]["Income1"]).Replace(",", string.Empty))).ToString("#,##0");     // Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]);
                }

                if (String.IsNullOrEmpty(dst_App_Income_PlanHolder.Tables[0].Rows[0]["Income2"].ToString()))
                {
                    dr_App_PlanHolderIncome["PlanHolderIncome2"] = "";
                }
                else
                {
                    dr_App_PlanHolderIncome["PlanHolderIncome2"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Income_PlanHolder.Tables[0].Rows[0]["Income2"]).Replace(",", string.Empty))).ToString("#,##0");     // Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]);
                }

                if (String.IsNullOrEmpty(dst_App_Income_PlanHolder.Tables[0].Rows[0]["Income3"].ToString()))
                {
                    dr_App_PlanHolderIncome["PlanHolderIncome3"] = "";
                }
                else
                {
                    dr_App_PlanHolderIncome["PlanHolderIncome3"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Income_PlanHolder.Tables[0].Rows[0]["Income3"]).Replace(",", string.Empty))).ToString("#,##0");     // Convert.ToString(dst_App_Income.Tables[0].Rows[0]["Income1"]);
                }
                dt_App_PlanHolderIncome.Rows.Add(dr_App_PlanHolderIncome);
            }
            else
            {
                DataRow dr_App_PlanHolderIncome = dt_App_PlanHolderIncome.NewRow();
                dr_App_PlanHolderIncome["PlanHolderIncome1"] = "";
                dr_App_PlanHolderIncome["PlanHolderIncome2"] = "";
                dr_App_PlanHolderIncome["PlanHolderIncome3"] = "";
                dt_App_PlanHolderIncome.Rows.Add(dr_App_PlanHolderIncome);
            }

            //Other Insurance Details
            DataSet dst_App_Other_Insurance_Details = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Insurance_History where Application_Id = '" + app_id + "' and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Other_Insurance_Details);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Other_Insurance_Details = new DataTable();
            dt_App_Other_Insurance_Details.Clear();
            dt_App_Other_Insurance_Details.Columns.Add("companyname");
            dt_App_Other_Insurance_Details.Columns.Add("plannumber");
            dt_App_Other_Insurance_Details.Columns.Add("yearofissuance");
            dt_App_Other_Insurance_Details.Columns.Add("sumcovered");
            dt_App_Other_Insurance_Details.Columns.Add("contribution");
            dt_App_Other_Insurance_Details.Columns.Add("standard");

            if (dst_App_Other_Insurance_Details.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Other_Insurance_Details.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Other_Insurance_Details = dt_App_Other_Insurance_Details.NewRow();
                    dr_App_Other_Insurance_Details["companyname"] = Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["Company"]);
                    dr_App_Other_Insurance_Details["plannumber"] = Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["Policy_No"]);
                    dr_App_Other_Insurance_Details["yearofissuance"] = Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["Year"]);
                    dr_App_Other_Insurance_Details["sumcovered"] = Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["SumCover"]);
                    if (String.IsNullOrEmpty(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["contribution"].ToString()))
                    {
                        dr_App_Other_Insurance_Details["contribution"] = "";
                    }
                    else
                    {
                        dr_App_Other_Insurance_Details["contribution"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["Contribution"]).Replace(",", string.Empty))).ToString("#,##0");//Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["Contribution"]);
                    }

                    dr_App_Other_Insurance_Details["standard"] = Convert.ToString(dst_App_Other_Insurance_Details.Tables[0].Rows[i]["InsType"]);
                    dt_App_Other_Insurance_Details.Rows.Add(dr_App_Other_Insurance_Details);
                }
            }
            else
            {
                DataRow dr_App_Other_Insurance_Details = dt_App_Other_Insurance_Details.NewRow();
                dr_App_Other_Insurance_Details["companyname"] = "NA";
                dr_App_Other_Insurance_Details["plannumber"] = "NA";
                dr_App_Other_Insurance_Details["yearofissuance"] = "NA";
                dr_App_Other_Insurance_Details["sumcovered"] = "NA";
                dr_App_Other_Insurance_Details["contribution"] = "NA";
                dr_App_Other_Insurance_Details["standard"] = "NA";
                dt_App_Other_Insurance_Details.Rows.Add(dr_App_Other_Insurance_Details);
            }

            //Other Beneficiaries
            DataSet dst_App_Beneficiaries = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Beneficiary where Application_Id = " + app_id + " and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Beneficiaries);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Beneficiaries = new DataTable();
            dt_App_Beneficiaries.Clear();
            dt_App_Beneficiaries.Columns.Add("name");
            dt_App_Beneficiaries.Columns.Add("relationship");
            dt_App_Beneficiaries.Columns.Add("age");
            dt_App_Beneficiaries.Columns.Add("share");

            if (dst_App_Beneficiaries.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Beneficiaries.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Beneficiaries = dt_App_Beneficiaries.NewRow();
                    dr_App_Beneficiaries["name"] = Convert.ToString(dst_App_Beneficiaries.Tables[0].Rows[i]["Name"]);
                    dr_App_Beneficiaries["relationship"] = Convert.ToString(dst_App_Beneficiaries.Tables[0].Rows[i]["Relation"]);
                    dr_App_Beneficiaries["age"] = Convert.ToString(dst_App_Beneficiaries.Tables[0].Rows[i]["Age"]);
                    dr_App_Beneficiaries["share"] = Convert.ToString(dst_App_Beneficiaries.Tables[0].Rows[i]["Share"]); ;
                    dt_App_Beneficiaries.Rows.Add(dr_App_Beneficiaries);
                }
            }
            else
            {
                DataRow dr_App_Beneficiaries = dt_App_Beneficiaries.NewRow();
                dr_App_Beneficiaries["name"] = "";
                dr_App_Beneficiaries["relationship"] = "";
                dr_App_Beneficiaries["age"] = "";
                dr_App_Beneficiaries["share"] = "";
                dt_App_Beneficiaries.Rows.Add(dr_App_Beneficiaries);
            }

            //Family History
            DataSet dst_App_FamilyHistory = new DataSet();
            int family_1st_count = 2;
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }
                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Family_History where Application_Id = " + app_id + " and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_FamilyHistory);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_FamilyHistory = new DataTable();
            dt_App_FamilyHistory.Clear();
            dt_App_FamilyHistory.Columns.Add("relation");
            dt_App_FamilyHistory.Columns.Add("state1");
            dt_App_FamilyHistory.Columns.Add("state2");
            dt_App_FamilyHistory.Columns.Add("state3");

            family_1st_count = dst_App_FamilyHistory.Tables[0].Rows.Count;

            if (dst_App_FamilyHistory.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_FamilyHistory.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_FamilyHistory = dt_App_FamilyHistory.NewRow();
                    dr_App_FamilyHistory["relation"] = Convert.ToString(dst_App_FamilyHistory.Tables[0].Rows[i]["Relation"]);
                    dr_App_FamilyHistory["state1"] = Convert.ToString(dst_App_FamilyHistory.Tables[0].Rows[i]["State1"]);
                    dr_App_FamilyHistory["state2"] = Convert.ToString(dst_App_FamilyHistory.Tables[0].Rows[i]["State2"]);
                    dr_App_FamilyHistory["state3"] = Convert.ToString(dst_App_FamilyHistory.Tables[0].Rows[i]["State3"]); ;
                    dt_App_FamilyHistory.Rows.Add(dr_App_FamilyHistory);
                }

            }
            else
            {
                DataRow dr_App_FamilyHistory = dt_App_FamilyHistory.NewRow();
                dr_App_FamilyHistory["relation"] = "";
                dr_App_FamilyHistory["state1"] = "";
                dr_App_FamilyHistory["state2"] = "";
                dr_App_FamilyHistory["state3"] = "";
                dt_App_FamilyHistory.Rows.Add(dr_App_FamilyHistory);
            }
            
            //Assets
            DataSet dst_App_Assets = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Assest where Application_Id = " + app_id + " and Life = '1';";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Assets);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Assets = new DataTable();
            dt_App_Assets.Clear();
            dt_App_Assets.Columns.Add("name");
            dt_App_Assets.Columns.Add("currency");
            dt_App_Assets.Columns.Add("amount");
            dt_App_Assets.Columns.Add("remarks");

            if (dst_App_Assets.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Assets.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Assets = dt_App_Assets.NewRow();
                    dr_App_Assets["name"] = Convert.ToString(dst_App_Assets.Tables[0].Rows[i]["Name"]);
                    dr_App_Assets["currency"] = Convert.ToString(dst_App_Assets.Tables[0].Rows[i]["Currency"]);
                    dr_App_Assets["amount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Assets.Tables[0].Rows[i]["Amount"]).Replace(",", string.Empty))).ToString("#,##0");//Convert.ToString();//Convert.ToString(dst_App_Assets.Tables[0].Rows[i]["Amount"]);
                    if (Convert.ToString(dst_App_Assets.Tables[0].Rows[i]["Remarks"]) != "")
                    {
                        dr_App_Assets["remarks"] = Convert.ToString(dst_App_Assets.Tables[0].Rows[i]["Remarks"]);
                    }
                    else
                    {
                        dr_App_Assets["remarks"] = "";
                    }
                    dt_App_Assets.Rows.Add(dr_App_Assets);
                }
            }
            else
            {
                DataRow dr_App_Assets = dt_App_Assets.NewRow();
                dr_App_Assets["name"] = "";
                dr_App_Assets["currency"] = "";
                dr_App_Assets["amount"] = "";
                dr_App_Assets["remarks"] = "";
                dt_App_Assets.Rows.Add(dr_App_Assets);
            }

            //Liability
            DataSet dst_App_Liability = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Liability where Application_Id = " + app_id + " and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Liability);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Liability = new DataTable();
            dt_App_Liability.Clear();
            dt_App_Liability.Columns.Add("name");
            dt_App_Liability.Columns.Add("currency");
            dt_App_Liability.Columns.Add("amount");
            dt_App_Liability.Columns.Add("remarks");

            if (dst_App_Liability.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dst_App_Liability.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr_App_Liability = dt_App_Liability.NewRow();
                    dr_App_Liability["name"] = Convert.ToString(dst_App_Liability.Tables[0].Rows[i]["Name"]);
                    dr_App_Liability["currency"] = Convert.ToString(dst_App_Liability.Tables[0].Rows[i]["Currency"]);
                    if (String.IsNullOrEmpty(dst_App_Liability.Tables[0].Rows[i]["Amount"].ToString()))
                    {
                        dr_App_Liability["amount"] = "";
                    }
                    else
                    {
                        dr_App_Liability["amount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(dst_App_Liability.Tables[0].Rows[i]["Amount"]).Replace(",", string.Empty))).ToString("#,##0");//Convert.ToString();  //Convert.ToString(dst_App_Liability.Tables[0].Rows[i]["Amount"]);
                    }

                    if (Convert.ToString(dst_App_Liability.Tables[0].Rows[i]["Remarks"]) != "")
                    {
                        dr_App_Liability["remarks"] = Convert.ToString(dst_App_Liability.Tables[0].Rows[i]["Remarks"]);
                    }
                    else
                    {
                        dr_App_Liability["remarks"] = "";
                    }
                    dt_App_Liability.Rows.Add(dr_App_Liability);
                }
            }
            else
            {
                DataRow dr_App_Liability = dt_App_Liability.NewRow();
                dr_App_Liability["name"] = "";
                dr_App_Liability["currency"] = "";
                dr_App_Liability["amount"] = "";
                dr_App_Liability["remarks"] = "";
                dt_App_Liability.Rows.Add(dr_App_Liability);
            }


            //Medical
            DataSet dst_App_Medical = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select * from Application_Medical where Application_Id = " + app_id + " and Life = '1'";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_Medical);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_App_Medical = new DataTable();
            dt_App_Medical.Clear();
            dt_App_Medical.Columns.Add("weight");
            dt_App_Medical.Columns.Add("height");            
            dt_App_Medical.Columns.Add("m1");
            dt_App_Medical.Columns.Add("m2");
            dt_App_Medical.Columns.Add("m3");
            dt_App_Medical.Columns.Add("m4");
            dt_App_Medical.Columns.Add("m5");
            dt_App_Medical.Columns.Add("m6");
            dt_App_Medical.Columns.Add("m7");
            dt_App_Medical.Columns.Add("m8");
            dt_App_Medical.Columns.Add("m9");
            dt_App_Medical.Columns.Add("m10");
            dt_App_Medical.Columns.Add("m11");
            dt_App_Medical.Columns.Add("m12");
            dt_App_Medical.Columns.Add("m13");
            dt_App_Medical.Columns.Add("m14");
            dt_App_Medical.Columns.Add("m15");
            dt_App_Medical.Columns.Add("m16");
            dt_App_Medical.Columns.Add("m17");
            dt_App_Medical.Columns.Add("m18");
            dt_App_Medical.Columns.Add("m19");
            dt_App_Medical.Columns.Add("m20");
            dt_App_Medical.Columns.Add("m21");

            dt_App_Medical.Columns.Add("t1");
            dt_App_Medical.Columns.Add("t2");
            dt_App_Medical.Columns.Add("t3");
            dt_App_Medical.Columns.Add("t4");
            dt_App_Medical.Columns.Add("t5");
            dt_App_Medical.Columns.Add("t6");
            dt_App_Medical.Columns.Add("t7");
            dt_App_Medical.Columns.Add("t8");
            dt_App_Medical.Columns.Add("t9");
            dt_App_Medical.Columns.Add("t10");
            dt_App_Medical.Columns.Add("t11");
            dt_App_Medical.Columns.Add("t12");
            dt_App_Medical.Columns.Add("t13");
            dt_App_Medical.Columns.Add("t14");
            dt_App_Medical.Columns.Add("t15");
            dt_App_Medical.Columns.Add("t16");
            dt_App_Medical.Columns.Add("t17");
            dt_App_Medical.Columns.Add("t18");
            dt_App_Medical.Columns.Add("t19");
            dt_App_Medical.Columns.Add("t20");
            dt_App_Medical.Columns.Add("t21");

            try
            {

                DataRow dr_App_Medical = dt_App_Medical.NewRow();
                dr_App_Medical["weight"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["Weight"]);
                dr_App_Medical["height"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["Height"]);
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M1"]) == "0")
                {
                    dr_App_Medical["m1"] = "No";
                }
                else
                {
                    dr_App_Medical["m1"] = "Yes";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M2"]) == "0")
                {
                    dr_App_Medical["m2"] = "No";
                }
                else
                {
                    dr_App_Medical["m2"] = "Yes";
                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M3"]) == "0")
                {
                    dr_App_Medical["m3"] = "No";
                }
                else
                {
                    dr_App_Medical["m3"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M4"]) == "0")
                {
                    dr_App_Medical["m4"] = "No";
                }
                else
                {
                    dr_App_Medical["m4"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M5"]) == "0")
                {
                    dr_App_Medical["m5"] = "No";
                }
                else
                {
                    dr_App_Medical["m5"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M6"]) == "0")
                {
                    dr_App_Medical["m6"] = "No";
                }
                else
                {
                    dr_App_Medical["m6"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M7"]) == "0")
                {
                    dr_App_Medical["m7"] = "No";
                }
                else
                {
                    dr_App_Medical["m7"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M8"]) == "0")
                {
                    dr_App_Medical["m8"] = "No";
                }
                else
                {
                    dr_App_Medical["m8"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M9"]) == "0")
                {
                    dr_App_Medical["m9"] = "No";
                }
                else
                {
                    dr_App_Medical["m9"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M10"]) == "0")
                {
                    dr_App_Medical["m10"] = "No";
                }
                else
                {
                    dr_App_Medical["m10"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M11"]) == "0")
                {
                    dr_App_Medical["m11"] = "No";
                }
                else
                {
                    dr_App_Medical["m11"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M12"]) == "0")
                {
                    dr_App_Medical["m12"] = "No";
                }
                else
                {
                    dr_App_Medical["m12"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M13"]) == "0")
                {
                    dr_App_Medical["m13"] = "No";
                }
                else
                {
                    dr_App_Medical["m13"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M14"]) == "0")
                {
                    dr_App_Medical["m14"] = "No";
                }
                else
                {
                    dr_App_Medical["m14"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M15"]) == "0")
                {
                    dr_App_Medical["m15"] = "No";
                }
                else
                {
                    dr_App_Medical["m15"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M16"]) == "0")
                {
                    dr_App_Medical["m16"] = "No";
                }
                else
                {
                    dr_App_Medical["m16"] = "Yes";

                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M17"]) == "0")
                {
                    dr_App_Medical["m17"] = "No";
                }
                else
                {
                    dr_App_Medical["m17"] = "Yes";

                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M18"]) == "0")
                {
                    dr_App_Medical["m18"] = "No";
                }
                else
                {
                    dr_App_Medical["m18"] = "Yes";

                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M19"]) == "0")
                {
                    dr_App_Medical["m19"] = "No";
                }
                else
                {
                    dr_App_Medical["m19"] = "Yes";

                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M20"]) == "0")
                {
                    dr_App_Medical["m20"] = "No";
                }
                else
                {
                    dr_App_Medical["m20"] = "Yes";

                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["M21"]) == "0")
                {
                    dr_App_Medical["m21"] = "No";
                }
                else
                {
                    dr_App_Medical["m21"] = "Yes";

                }


                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T1"]) != "")
                {
                    dr_App_Medical["t1"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T1"]);
                }
                else
                {
                    dr_App_Medical["t1"] = "";
                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T2"]) != "")
                {
                    dr_App_Medical["t2"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T2"]);
                }
                else
                {
                    dr_App_Medical["t2"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T3"]) != "")
                {
                    dr_App_Medical["t3"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T3"]);
                }
                else
                {
                    dr_App_Medical["t3"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T4"]) != "")
                {
                    dr_App_Medical["t4"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T4"]);
                }
                else
                {
                    dr_App_Medical["t4"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T5"]) != "")
                {
                    dr_App_Medical["t5"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T5"]);
                }
                else
                {
                    dr_App_Medical["t5"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T6"]) != "")
                {
                    dr_App_Medical["t6"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T6"]);
                }
                else
                {
                    dr_App_Medical["t6"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T7"]) != "")
                {
                    dr_App_Medical["t7"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T7"]);
                }
                else
                {
                    dr_App_Medical["t7"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T8"]) != "")
                {
                    dr_App_Medical["t8"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T8"]);
                }
                else
                {
                    dr_App_Medical["t8"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T9"]) != "")
                {
                    dr_App_Medical["t9"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T9"]);
                }
                else
                {
                    dr_App_Medical["t9"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T10"]) != "")
                {
                    dr_App_Medical["t10"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T10"]);
                }
                else
                {
                    dr_App_Medical["t10"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T11"]) != "")
                {
                    dr_App_Medical["t11"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T11"]);
                }
                else
                {
                    dr_App_Medical["t11"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T12"]) != "")
                {
                    dr_App_Medical["t12"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T12"]);
                }
                else
                {
                    dr_App_Medical["t12"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T13"]) != "")
                {
                    dr_App_Medical["t13"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T13"]);
                }
                else
                {
                    dr_App_Medical["t13"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T14"]) != "")
                {
                    dr_App_Medical["t14"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T14"]);
                }
                else
                {
                    dr_App_Medical["t14"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T15"]) != "")
                {
                    dr_App_Medical["t15"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T15"]);
                }
                else
                {
                    dr_App_Medical["t15"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T16"]) != "")
                {
                    dr_App_Medical["t16"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T16"]);
                }
                else
                {
                    dr_App_Medical["t16"] = "";
                }
                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T17"]) != "")
                {
                    dr_App_Medical["t17"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T17"]);
                }
                else
                {
                    dr_App_Medical["t17"] = "";
                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T18"]) != "")
                {
                    dr_App_Medical["t18"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T18"]);
                }
                else
                {
                    dr_App_Medical["t18"] = "";
                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T19"]) != "")
                {
                    dr_App_Medical["t19"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T19"]);
                }
                else
                {
                    dr_App_Medical["t19"] = "";
                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T20"]) != "")
                {
                    dr_App_Medical["t20"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T20"]);
                }
                else
                {
                    dr_App_Medical["t20"] = "";
                }

                if (Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T21"]) != "")
                {
                    dr_App_Medical["t21"] = Convert.ToString(dst_App_Medical.Tables[0].Rows[0]["T21"]);
                }
                else
                {
                    dr_App_Medical["t21"] = "";
                }

                dt_App_Medical.Rows.Add(dr_App_Medical);

            }
            catch
            { }
            
            //travel Life 1
            DataSet dst_travel = new DataSet();
            DataTable dt_travel = new DataTable();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select * from Application_travel where Application_Id=" + app_id + " and Life = '1' order  by Type";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_travel);
                oledbAdapter.Dispose();
                MyConnection.Close();

                dt_travel.Clear();
                dt_travel.Columns.Add("CountryCity");
                dt_travel.Columns.Add("LengthOfStay");
                dt_travel.Columns.Add("NoOfVisits");
                dt_travel.Columns.Add("PurposeOfVisits");
                dt_travel.Columns.Add("Duration");

                if (dst_travel.Tables[0].Rows.Count > 0)
                {
                                        
                    for (int i = 0; i <= dst_travel.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow drtravel1 = dt_travel.NewRow();
                        drtravel1["CountryCity"] = Convert.ToString(dst_travel.Tables[0].Rows[i]["countryandcity"]);
                        drtravel1["LengthOfStay"] = Convert.ToString(dst_travel.Tables[0].Rows[i]["lengthofstaypervisit"]);
                        drtravel1["NoOfVisits"] = Convert.ToString(dst_travel.Tables[0].Rows[i]["numbofvisit"]);
                        drtravel1["PurposeOfVisits"] = Convert.ToString(dst_travel.Tables[0].Rows[i]["purposeoftravel"]);

                        if (Convert.ToString(dst_travel.Tables[0].Rows[i]["Type"]) == "1")
                        {
                            drtravel1["Duration"] = "Next 12 Months";
                        }
                        else
                        {
                            drtravel1["Duration"] = "Past 12 Months";
                        }

                        dt_travel.Rows.Add(drtravel1);
                    }
                }
                else
                {                    
                        DataRow drtravel1 = dt_travel.NewRow();
                        drtravel1["CountryCity"] = "Did not Travel";
                        drtravel1["LengthOfStay"] = " NA ";
                        drtravel1["NoOfVisits"] = " NA ";
                        drtravel1["PurposeOfVisits"] = " NA ";
                        drtravel1["Duration"] = " Past and Next 12 Months ";
                        dt_travel.Rows.Add(drtravel1);                    
                }
            }
            catch 
            {

            }
            
            //Agent Details
            DataSet dst_Agent_Details = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select a.V_Name,a.V_Code from Life_Login_Master a inner join Application_Master b on a.log_id = b.broker_id and b.Application_Id=" + app_id + " and b.broker_id = " + Session["Broker_Id"].ToString().Trim() + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_Agent_Details);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            DataTable dt_Agent_Details = new DataTable();
            try
            {

                dt_Agent_Details.Clear();
                dt_Agent_Details.Columns.Add("AgentName");
                dt_Agent_Details.Columns.Add("Code");
                
                dt_Agent_Details.Columns.Add("Date");
                           
                if (dst_Agent_Details.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= dst_Agent_Details.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow drAgentDetails = dt_Agent_Details.NewRow();
                        drAgentDetails["AgentName"] = Convert.ToString(dst_Agent_Details.Tables[0].Rows[i]["V_Name"]);
                        drAgentDetails["Code"] = Convert.ToString(dst_Agent_Details.Tables[0].Rows[i]["V_Code"]);
                        drAgentDetails["Date"] = DateTime.Now.ToString("dd - MMM - yyyy", CultureInfo.InvariantCulture);
                        dt_Agent_Details.Rows.Add(drAgentDetails);
                    }
                }
                else
                {
                    DataRow drAgentDetails = dt_Agent_Details.NewRow();
                    drAgentDetails["Name"] = " ";
                    drAgentDetails["Date"] = " ";
                    drAgentDetails["Code"] = " ";
                    dt_Agent_Details.Rows.Add(drAgentDetails);
                }
            }
            catch
            {

            }

            string strPath = "";
            strPath = Server.MapPath("~/Report/ApplicationFormIdikhar.rdlc");
            reportViewer1.LocalReport.ReportPath = strPath;
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource("dst_App_Details", dt_App_Details);
            ReportDataSource datasource1 = new ReportDataSource("dst_App_Contact", dt_App_Contact);
            ReportDataSource datasource2 = new ReportDataSource("dst_App_Permanent", dt_App_Permanent);
            ReportDataSource datasource3 = new ReportDataSource("dst_FATCA", dt_FATCA);
            ReportDataSource datasource4 = new ReportDataSource("dst_CRS", dt_CRS);
            ReportDataSource datasource5 = new ReportDataSource("dst_App_Benefits", dt_App_Benefits);
            ReportDataSource datasource6 = new ReportDataSource("dst_app_contribution", dt_app_contribution);
            ReportDataSource datasource7 = new ReportDataSource("dst_App_Investment", dt_App_Investment);
            ReportDataSource datasource8 = new ReportDataSource("dst_App_Fund", dt_App_Fund);
            ReportDataSource datasource9 = new ReportDataSource("dst_App_Income", dt_App_Income);
            ReportDataSource datasource10 = new ReportDataSource("dst_App_Other_Insurance_Details", dt_App_Other_Insurance_Details);
            ReportDataSource datasource11 = new ReportDataSource("dst_App_Beneficiaries", dt_App_Beneficiaries);
            ReportDataSource datasource12 = new ReportDataSource("dst_App_Medical", dt_App_Medical);
            ReportDataSource datasource13 = new ReportDataSource("dst_App_FamilyHistory", dt_App_FamilyHistory);
            ReportDataSource datasource14 = new ReportDataSource("dst_App_Assets", dt_App_Assets);
            ReportDataSource datasource15 = new ReportDataSource("dst_App_Liability", dt_App_Liability);           
            ReportDataSource datasource16 = new ReportDataSource("dst_App_Details_PlanHolder", dt_App_Details_PlanHolder);
            ReportDataSource datasource17 = new ReportDataSource("dst_App_Contact_PlanHolder", dt_App_Contact_PlanHolder);
            ReportDataSource datasource18 = new ReportDataSource("dst_app_permanent_PlanHolder", dt_App_Permanent_PlanHolder);
            ReportDataSource datasource19 = new ReportDataSource("dst_App_Fund_PlanHolder", dt_App_Fund_PlanHolder);
            ReportDataSource datasource20 = new ReportDataSource("dst_App_Assets_PlanHolder", dt_App_Assets_PlanHolder);
            ReportDataSource datasource21 = new ReportDataSource("dst_App_Liability_PlanHolder", dt_App_Liability_PlanHolder);            
            ReportDataSource datasource22 = new ReportDataSource("dst_App_Guardian", dt_App_Guardian);            
            ReportDataSource datasource23 = new ReportDataSource("dst_travel", dt_travel);
            ReportDataSource datasource24 = new ReportDataSource("dst_App_Benefit_Details", dt_App_Benefit_Details);            
            ReportDataSource datasource25 = new ReportDataSource("dst_App_Income_PlanHolder", dt_App_PlanHolderIncome);           
            ReportDataSource datasource26 = new ReportDataSource("dst_App_Other_Insurance_Details_PlanHolder", dt_App_Other_Insurance_Details_PlanHolder);
            ReportDataSource datasource27 = new ReportDataSource("dst_Agent_Details", dt_Agent_Details);
            reportViewer1.LocalReport.DataSources.Add(datasource);
            reportViewer1.LocalReport.DataSources.Add(datasource1);
            reportViewer1.LocalReport.DataSources.Add(datasource2);
            reportViewer1.LocalReport.DataSources.Add(datasource3);
            reportViewer1.LocalReport.DataSources.Add(datasource4);
            reportViewer1.LocalReport.DataSources.Add(datasource5);
            reportViewer1.LocalReport.DataSources.Add(datasource6);
            reportViewer1.LocalReport.DataSources.Add(datasource7);
            reportViewer1.LocalReport.DataSources.Add(datasource8);
            reportViewer1.LocalReport.DataSources.Add(datasource9);
            reportViewer1.LocalReport.DataSources.Add(datasource10);
            reportViewer1.LocalReport.DataSources.Add(datasource11);
            reportViewer1.LocalReport.DataSources.Add(datasource12);
            reportViewer1.LocalReport.DataSources.Add(datasource13);
            reportViewer1.LocalReport.DataSources.Add(datasource14);
            reportViewer1.LocalReport.DataSources.Add(datasource15);

            reportViewer1.LocalReport.DataSources.Add(datasource16);
            reportViewer1.LocalReport.DataSources.Add(datasource17);
            reportViewer1.LocalReport.DataSources.Add(datasource18);
            reportViewer1.LocalReport.DataSources.Add(datasource19);
            reportViewer1.LocalReport.DataSources.Add(datasource20);
            reportViewer1.LocalReport.DataSources.Add(datasource21);


            reportViewer1.LocalReport.DataSources.Add(datasource22);

            reportViewer1.LocalReport.DataSources.Add(datasource23);
            reportViewer1.LocalReport.DataSources.Add(datasource24);
            reportViewer1.LocalReport.DataSources.Add(datasource25);
            reportViewer1.LocalReport.DataSources.Add(datasource26);

            reportViewer1.LocalReport.DataSources.Add(datasource27);
            
            
            reportViewer1.LocalReport.EnableExternalImages = true;
            string imagePath = new Uri(Server.MapPath("~/Signature/" + mid + ".jpg")).AbsoluteUri;
            
            DataSet sign = new DataSet();
            string SignatureDateStamp = "";
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "Select Sign_Date from Application_Master where Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(sign);
                oledbAdapter.Dispose();
                MyConnection.Close();

                SignatureDateStamp = Convert.ToString(sign.Tables[0].Rows[0]["Sign_Date"]);
                           
                ReportParameter parameter = new ReportParameter("sign1", imagePath);
                reportViewer1.LocalReport.SetParameters(parameter);

                ReportParameter parameter12 = new ReportParameter("Sign_Date", DateTime.Parse(SignatureDateStamp).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                reportViewer1.LocalReport.SetParameters(parameter12);
            }
            catch
            {

            }

            string imagePath2 = new Uri(Server.MapPath("~/Signature/Agent_" + mid + ".jpg")).AbsoluteUri;
            try
            {
                ReportParameter parameter7 = new ReportParameter("sign4", imagePath2);
                reportViewer1.LocalReport.SetParameters(parameter7);

                ReportParameter parameter13 = new ReportParameter("Sign_Date", DateTime.Parse(SignatureDateStamp).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                reportViewer1.LocalReport.SetParameters(parameter13);
            }
            catch
            {

            }
            
           
            if (plan_holder == 1)
            {
                imagePath = new Uri(Server.MapPath("~/Signature/" + mid + "_2.jpg")).AbsoluteUri;
                ReportParameter parameter15 = new ReportParameter("Sign_Date3", DateTime.Parse(SignatureDateStamp).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                reportViewer1.LocalReport.SetParameters(parameter15);
            }
            else
            {
                imagePath = new Uri(Server.MapPath("~/Signature/blank.jpg")).AbsoluteUri;

                ReportParameter parameter15 = new ReportParameter("Sign_Date3", "");
                reportViewer1.LocalReport.SetParameters(parameter15);
            }
            ReportParameter parameter3 = new ReportParameter("sign3", imagePath);
            reportViewer1.LocalReport.SetParameters(parameter3);
           
            //Report Name
            DataSet dst_App_RptHeading = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapter;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select A.Plan_Code from Illustration_Master A inner join Application_Master B ON A.Illustration_Id = B.Illustration_Id where B.Application_Id=" + app_id + "";
                oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapter.Fill(dst_App_RptHeading);
                oledbAdapter.Dispose();
                MyConnection.Close();
            }
            catch { }
            
            if (dst_App_RptHeading.Tables[0].Rows.Count > 0)
            {


                if (Convert.ToString(dst_App_Benefit_Details.Tables[0].Rows[0]["Plan_Code"]).Trim() == "SP+")

                {
                    
                    ReportParameter parameter4 = new ReportParameter("PlanName", "IDIKHAR PLUS");
                    reportViewer1.LocalReport.SetParameters(parameter4);


                    ReportParameter parameter5 = new ReportParameter("PlanNameArabic", "توضيح منافع التكافل حياة الأشمل");
                    reportViewer1.LocalReport.SetParameters(parameter5);

                }
                
            }
            DataSet dst_ApplicationId = new DataSet();
            try
            {
                OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                OleDbDataAdapter oledbAdapterapp;
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.Open();
                string Myquery = "";
                Myquery = "select Application_no,illustration_path from Application_Master where Application_Id = " + app_id + "";
                oledbAdapterapp = new OleDbDataAdapter(Myquery, MyConnection);
                oledbAdapterapp.Fill(dst_ApplicationId);
                oledbAdapterapp.Dispose();
                MyConnection.Close();
            }
            catch { }
            ReportParameter parameter6 = new ReportParameter("ApplicationId", Convert.ToString(dst_ApplicationId.Tables[0].Rows[0]["Application_no"]));
            reportViewer1.LocalReport.SetParameters(parameter6);

            reportViewer1.LocalReport.Refresh();
            
            rpt_path = "";
            try
            {
                System.IO.DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/doc"));

                if (dir.Exists)
                {

                    rpt_path = Server.MapPath("~/Hyat/doc");
                }

                else
                {

                    rpt_path = Server.MapPath("~/Hyat/doc");
                    Directory.CreateDirectory(rpt_path);
                }
            }
            catch
            {
            }


            try
            {
                string[] sFilenames;
                string sDirectory = rpt_path;

                string sSuperCoolExtension = ".pdf";
                sFilenames = Directory.GetFiles(sDirectory);

                foreach (string tempstring in sFilenames)
                {

                    if (tempstring.Contains(sSuperCoolExtension))
                    {
                      //  System.IO.File.Delete(tempstring);
                    }
                }
            }
            catch
            {

            }
            App_no = "ApplicationForm_" + App_no;
            rpt_path += @"\" + App_no + ".pdf";
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = reportViewer1.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            using (FileStream fs = new FileStream(rpt_path, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            Thread.Sleep(1000);

            try
            {
                string path1 = Server.MapPath("~/Hyat/doc/" + @"\" + mid + ".pdf");
                System.IO.File.Copy( rpt_path, path1);
                string[] inputFiles = new String[2];
                string path3 = Server.MapPath("~/Hyat/Merge/" + @"\" + App_no + ".pdf");
                inputFiles[0] = path1;
                inputFiles[1] =dst_ApplicationId.Tables[0].Rows[0]["illustration_path"].ToString();
                PdfMerge.MergeFiles(path3, inputFiles);
                string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + App_no + ".pdf");
                System.IO.File.Copy(path3, filePath);
                rpt_path = filePath;
            }
            catch
            {

            }
          
           
            try
            {

                mycon = new OleDbConnection(conn);
                mycon.Open();
                SqlQuery = "";
                SqlQuery = "Update Application_Master set application_path='"+ rpt_path + "',application_stat=1 where Application_Id=" + Convert.ToInt32(mid) + "  ";
                OleDbCommand cmd_new = new OleDbCommand(SqlQuery, mycon);
                cmd_new.ExecuteNonQuery();
                mycon.Close();
                cmd_new.Cancel();
            }
            catch
            {

            }
            
            try
            {

                Response.ClearContent();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment;Filename=" + App_no + ".pdf");
                Response.Clear();
                Response.TransmitFile(rpt_path);
                Response.End();
            }
            catch 
            {

            }
        }


        public void viewReport(int mid,string ApplicationNo,int mid1,string Exclusion_path)
        {

        
            string SqlQuery = "";
            OleDbConnection mycon = new OleDbConnection(sqlconn);
            OleDbCommand cmd;
          

            try
            {
                commonFun Fn_Common = new commonFun();
                //WebReference.AuthHeader user = new AuthHeader();
                //user.Username = "SalamaL##";
                //user.Password = "&Portal%!&";
                //WebReference.WebService1 service = new WebService1();
                AppService service1 = new AppService();


                int Nation_discount = 0, Nation_discount2 = 0;
                int joint_life = 0;
                string plan_code = "";
                double v_sa = 0;
                int v_age = 0;
                int v_gender = 0;
                int v_smoker = 0;
                int v_trms = 0;
                string v_curr = "";
                int V_Freq = 0;
                int growthRate = 0;
                int Woc = 0;
                int ci = 0;
                int ptd = 0;
                int hcb = 0;
                int adb = 0;
                double contri = 0;
                int atpd = 0;
                int fib = 0;
                int Woc2 = 0;
                int ci2 = 0;
                int ptd2 = 0;
                int hcb2 = 0;
                int adb2 = 0;
                int pw = 0;
                int pw1 = 0;
                int pw2 = 0;
                int pw3 = 0;

                int atpd2 = 0;
                int fib2 = 0;
                int Woc_type = 0;
                double ci_amt = 0;
                double ptd_amt = 0;
                double hcb_amt = 0;
                double adb_amt = 0;
                double atpd_amt = 0;
                double fib_amt = 0;
                int fib_term = 0;
                double v_sa2 = 0;
                int v_age2 = 0;
                int v_gender2 = 0;
                int v_smoker2 = 0;
                double ci_amt2 = 0;
                double ptd_amt2 = 0;
                double hcb_amt2 = 0;
                double adb_amt2 = 0;
                double atpd_amt2 = 0;
                double fib_amt2 = 0;
                int fib_term2 = 0;              
                DataSet dstMed = new DataSet();
                int Resident_exclu = 0;
                int Resident_exclu_life2 = 0;
                string resident_exclu = "";
                string resident_exclu2 = "";
                DataSet dst_dt = new DataSet();
                DataSet dstCustomer_dt = new DataSet();
                DataSet dst_withdraw = new DataSet();
                DataSet dst_regular = new DataSet();
                int regular_withdrwal_no = 0;
                string regular_frequency = "";
                int regular_startyear = 0;
                double withdrawal1 = 0;
                double withdrawal2 = 0;
                double withdrawal3 = 0;
                int pyear1 = 0;
                int pyear2 = 0;
                int pyear3 = 0;
                double Rwithdrwal = 0;
                double Rwithdrwal_Sar = 0;
                int regwNo = 0;
                int regwAmt = 0;
                int regular_bln = 0;
                int rw = 0;
                int freq = 0;
                int regYear = 0;
                string nationality = "";
                string nationality2 = "";
                string country = "";
                string country2 = "";

                string plan_Holder_name = "";
                OleDbConnection myconnection_4 = new OleDbConnection(sqlconn);
                OleDbCommand prod_mCmmd_4;
                string prod_sqry_4;
                DataSet dst_valid = new DataSet();

                if (myconnection_4.State == ConnectionState.Open)
                {
                    myconnection_4.Close();
                }

                if(Exclusion_path!="")
                {
                    Resident_exclu = 1;
                }

            

                try
                {
                    
                    if (myconnection_4.State == ConnectionState.Open)
                    {
                        myconnection_4.Close();
                    }

                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "Select * from  Illustration_Master where Illustration_Id=" + mid + "  ";
                    OleDbDataAdapter oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dst_dt);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();
                    
                    if (myconnection_4.State == ConnectionState.Open)
                    {
                        myconnection_4.Close();
                    }

                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "select * from  Illustration_PW where Illustration_Id=" + mid + "";
                    oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dst_withdraw);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();




                    if (myconnection_4.State == ConnectionState.Open)
                    {
                        myconnection_4.Close();
                    }

                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "select * from  Illustration_RW where Illustration_Id=" + mid + "";
                    oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dst_regular);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();

                }
                catch
                {

                }
                
                try
                {
                    if (dst_dt.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "Message", "alert(' Please Process the new values before generating the Illustration');", true);

                        return;
                    }
                }
                catch
                {

                }

                int withdraw_bln = 0;

                try
                {
                    if (dst_withdraw.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            withdraw_bln = 1;
                        }
                        catch { }
                        try
                        {
                            pw = 1;
                        }
                        catch { }
                        try
                        {
                            withdrawal1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Amount1"]);
                        }
                        catch { }
                        try
                        {
                            withdrawal2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Amount2"]);
                        }
                        catch { }
                        try
                        {
                            withdrawal3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Amount3"]);
                        }
                        catch { }

                        try
                        {
                            pyear1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Pw1"]);
                        }
                        catch { }
                        try
                        {
                            pyear2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Pw2"]);
                        }
                        catch { }
                        try
                        {
                            pyear3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Pw3"]);
                        }
                        catch { }
                        try
                        {
                            pw1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Pw1"]);
                        }
                        catch { }
                        try
                        {
                            pw2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Pw2"]);
                        }
                        catch { }
                        try
                        {
                            pw3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["Pw3"]);
                        }
                        catch { }



                    }

                }
                catch { }

                try
                {
                    if (dst_regular.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            regular_bln = 1;
                        }
                        catch { }
                        try
                        {
                            rw = 1;
                        }
                        catch { }

                        try
                        {
                            regwNo = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["no_withdraw"]);
                        }
                        catch { }
                        try
                        {
                            regwAmt = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["amount"]);
                        }
                        catch { }
                        try
                        {
                            regular_frequency = Convert.ToString(dst_regular.Tables[0].Rows[0]["frequency"]);
                        }
                        catch { }
                        try
                        {
                            freq = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["frequency"]);
                        }
                        catch { }
                        try
                        {
                            regular_startyear = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["startyear"]);
                        }
                        catch { }


                        try
                        {
                            regYear = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["startyear"]);
                        }
                        catch { }
                    }
                }
                catch { }

                int custmer_id = 0;

                try
                {
                    if (dst_dt.Tables[0].Rows.Count > 0)
                    {

                        try
                        {
                            custmer_id = Convert.ToInt32(dst_dt.Tables[0].Rows[0]["Cust_Id"]);
                        }
                        catch
                        {

                        }


                        try
                        {
                            myconnection_4.Open();
                            prod_sqry_4 = "";
                            prod_sqry_4 = "Select * from  Customer_Master where Customer_Id=" + Convert.ToInt32(custmer_id) + " ";
                            OleDbDataAdapter oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                            oledbAdapter.Fill(dstCustomer_dt);
                            oledbAdapter.Dispose();
                            myconnection_4.Close();
                        }
                        catch
                        {

                        }
                        try
                        {
                            joint_life = Convert.ToInt32(dst_dt.Tables[0].Rows[0]["JLife"]);
                        }
                        catch
                        {

                        }
                        
                        contri = Convert.ToDouble(dst_dt.Tables[0].Rows[0]["Contribution"]);
                        
                        if (joint_life == 1)
                        {
                            nationality2 = Convert.ToString(dst_dt.Tables[0].Rows[0]["Nationality2"]);
                            v_gender2 = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Gender2"]);

                            v_age2 = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["age2"]);

                            v_smoker2 = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Habit2"]);

                            v_sa2 = Convert.ToDouble(dst_dt.Tables[0].Rows[0]["Sum_Cover2"]);



                            try
                            {

                                // string cconn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + physicalPath3 + " ";
                                OleDbConnection myconnection = new OleDbConnection(cconn);
                                string prod_query = "";

                                OleDbCommand prod_mCmmd;
                                OleDbDataReader prod_dr;
                                myconnection.Open();
                                prod_query = "select rate_disc from Nationality where nationality_nm ='" + Convert.ToString(nationality2) + "' ";
                                prod_mCmmd = new System.Data.OleDb.OleDbCommand(prod_query, myconnection);

                                prod_dr = prod_mCmmd.ExecuteReader();
                                prod_dr.Read();
                                Nation_discount2 = 0;
                                Nation_discount2 = Convert.ToInt16(prod_dr[0]);
                                myconnection.Close();
                                prod_mCmmd.Cancel();
                            }
                            catch
                            {

                            }


                            try
                            {
                                using (SqlConnection con = new SqlConnection(constr))
                                {
                                    using (SqlCommand cmd1 = new SqlCommand("select  * from Illustration_Rider where status=1 and life=2 and illustration_id=" + mid + " "))
                                    {

                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                        {

                                            cmd1.Connection = con;
                                            sda.SelectCommand = cmd1;
                                            using (DataTable rider2_dt = new DataTable())
                                            {
                                                sda.Fill(rider2_dt);

                                                if (rider2_dt.Rows.Count > 0)
                                                {

                                                    for (int r2 = 0; r2 < rider2_dt.Rows.Count; r2++)
                                                    {

                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Permanent Total Disability")
                                                        {
                                                            ptd = 1;
                                                            ptd_amt2 = Convert.ToDouble(rider2_dt.Rows[r2]["Amount"]);
                                                        }


                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Critical Illness")
                                                        {
                                                            ci = 1;
                                                            ci_amt2 = Convert.ToDouble(rider2_dt.Rows[r2]["Amount"]);
                                                        }


                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Hospital Cash Benefit")
                                                        {
                                                            hcb = 1;
                                                            hcb_amt2 = Convert.ToDouble(rider2_dt.Rows[r2]["Amount"]);
                                                        }


                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Family Income Benefit")
                                                        {
                                                            fib = 1;
                                                            fib_amt2 = Convert.ToDouble(rider2_dt.Rows[r2]["Amount"]);
                                                            fib_term2 = Convert.ToInt32(rider2_dt.Rows[r2]["Term"]);
                                                        }

                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Accidental Death Benefit")
                                                        {
                                                            adb = 1;
                                                            adb_amt2 = Convert.ToDouble(rider2_dt.Rows[r2]["Amount"]);

                                                        }


                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Accidental Total/Partial Permanent Disability")
                                                        {
                                                            atpd = 1;
                                                            atpd_amt2 = Convert.ToDouble(rider2_dt.Rows[r2]["Amount"]);

                                                        }


                                                        if (Convert.ToString(rider2_dt.Rows[r2]["Name"]) == "Waiver of Contribution")
                                                        {
                                                            Woc2 = 1;


                                                        }

                                                    }

                                                }


                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                            
                        }

                        try
                        {
                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd1 = new SqlCommand("select  * from Illustration_Rider where status=1 and life=1 and illustration_id=" + mid + ""))
                                {

                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {

                                        cmd1.Connection = con;
                                        sda.SelectCommand = cmd1;
                                        using (DataTable Rider1_dt = new DataTable())
                                        {
                                            sda.Fill(Rider1_dt);
                                            if (Rider1_dt.Rows.Count > 0)
                                            {

                                                for (int r1 = 0; r1 < Rider1_dt.Rows.Count; r1++)
                                                {

                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Permanent Total Disability")
                                                    {
                                                        ptd = 1;
                                                        ptd_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                                    }


                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Critical Illness")
                                                    {
                                                        ci = 1;
                                                        ci_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                                    }


                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Hospital Cash Benefit")
                                                    {
                                                        hcb = 1;
                                                        hcb_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                                    }


                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Family Income Benefit")
                                                    {
                                                        fib = 1;
                                                        fib_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                                        fib_term = Convert.ToInt32(Rider1_dt.Rows[r1]["Term"]);
                                                    }

                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Accidental Death Benefit")
                                                    {
                                                        adb = 1;
                                                        adb_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);

                                                    }


                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Accidental Total/Partial Permanent Disability")
                                                    {
                                                        atpd = 1;
                                                        atpd_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);

                                                    }


                                                    if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Waiver of Contribution")
                                                    {
                                                        Woc = 1;


                                                    }

                                                }

                                            }


                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }

                        nationality = Convert.ToString(dst_dt.Tables[0].Rows[0]["nationality"]).Trim();
                  


                        v_age = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Age"]);

                        growthRate = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Growth"]);

                        v_trms = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Payment_Term"]);

                        v_sa = Convert.ToDouble(dst_dt.Tables[0].Rows[0]["Sum_Cover"]);
                        v_gender = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Gender"]);
                        v_smoker = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Habit"]);
                       
                        plan_code = Convert.ToString(dst_dt.Tables[0].Rows[0]["Plan_Code"]).Trim();
                        v_curr = Convert.ToString(dst_dt.Tables[0].Rows[0]["Currency"]).Trim();
                        V_Freq = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Frequency"]);

                        try
                        {


                            OleDbConnection myconnection = new OleDbConnection(cconn);
                            string prod_query = "";

                            OleDbCommand prod_mCmmd;
                            OleDbDataReader prod_dr;
                            myconnection.Open();
                            prod_query = "select rate_disc from Nationality where nationality_nm ='" + Convert.ToString(nationality) + "' ";
                            prod_mCmmd = new System.Data.OleDb.OleDbCommand(prod_query, myconnection);

                            prod_dr = prod_mCmmd.ExecuteReader();
                            prod_dr.Read();
                            Nation_discount = 0;
                            Nation_discount = Convert.ToInt16(prod_dr[0]);
                            myconnection.Close();
                            prod_mCmmd.Cancel();
                        }
                        catch
                        {

                        }



                        StringBuilder sb = new StringBuilder();
                        sb.Append(Fn_Common.GetRandomNumber(30, 90));
                        sb.Append(Fn_Common.GetRandomNumber(12, 56));
                        sb.Append(Fn_Common.GetRandomNumber(9, 19));
                        sb.Append(Fn_Common.GetRandomNumber(7, 45));


                        string illustID;
                        string dob = "";

                        //  illustID = Session["disb_uid"].ToString().ToLower() + "- " + "Web " + "- " + Fn_Common.Illust_Version + "." + sb.ToString();
                        illustID = "OnBoard " + "- " + Fn_Common.Illust_Version + "." + sb.ToString();
                        try
                        {
                            dob = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Dob"]);

                        }
                        catch
                        {

                        }
                        DataTable dt = new DataTable();
                        dt.Clear();
                        dt.Columns.Add("Name");
                        dt.Columns.Add("Gender");
                        dt.Columns.Add("DateofBirth");
                        dt.Columns.Add("Age");
                        dt.Columns.Add("Smoker");
                        dt.Columns.Add("CountryofResidence");
                        dt.Columns.Add("Nationality");

                        DataRow dr = dt.NewRow();
                        DataRow dr_fund = dt.NewRow();
                        dr["Name"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["F_Name"]) + " " + Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["M_Name"]) + " " + Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["L_Name"]);
                        plan_Holder_name = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["F_Name"]);
                        if (v_gender == 0)
                        {
                            dr["Gender"] = "Male";
                        }
                        else
                        {
                            dr["Gender"] = "Female";
                        }
                        dr["DateofBirth"] = DateTime.Parse(dob).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture); 
                        dr["Age"] = v_age;
                        /* 26 June 2019 Smoker */
                        if (Convert.ToInt16(dstCustomer_dt.Tables[0].Rows[0]["Habit"]) == 0)
                        {
                            dr["Smoker"] = "Non-Smoker";
                        }
                        else
                        {
                            dr["Smoker"] = "Smoker";
                        }
                        /* 26 June 2019 Smoker */


                        //dr["CountryofResidence"] = Session["Resident"];
                        dr["CountryofResidence"] = Convert.ToString(dst_dt.Tables[0].Rows[0]["Resident"]);
                        dr["Nationality"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]);
                        country = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["BirthCountry"]);
                        if (Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["BirthCountry"]) == Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]))
                        {
                            dr["Nationality"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]);
                        }
                        else
                        {
                            dr["Nationality"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]) + " / " + Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["BirthCountry"]);
                        }
                        dt.Rows.Add(dr);

                        DataTable dt2 = new DataTable();
                        dt2.Clear();
                        dt2.Columns.Add("Currency");
                        dt2.Columns.Add("Contribution");
                        dt2.Columns.Add("FrequencyofContribution");
                        dt2.Columns.Add("ContributionYears");
                        dt2.Columns.Add("PlanTerm");

                        DataRow dr2 = dt2.NewRow();
                        dr2["Currency"] = v_curr;
                        dr2["Contribution"] = Convert.ToDouble(contri).ToString("#,##0");

                        if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 12)
                        {
                            dr2["FrequencyofContribution"] = "Monthly";
                        }
                        else if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 2)
                        {
                            dr2["FrequencyofContribution"] = "Half Yearly";
                        }
                        else if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 4)
                        {
                            dr2["FrequencyofContribution"] = "Quarterly";
                        }
                        else if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 1)
                        {
                            dr2["FrequencyofContribution"] = "Yearly";
                        }
                        dr2["ContributionYears"] = v_trms;
                        dr2["PlanTerm"] = 100 - v_age;
                        dt2.Rows.Add(dr2);



                        DataTable dt7 = new DataTable();
                        if (joint_life == 1)   // Joint member details 2nd covered member
                        {

                            dt7.Clear();
                            dt7.Columns.Add("Name");
                            dt7.Columns.Add("Gender");
                            dt7.Columns.Add("DateofBirth");
                            dt7.Columns.Add("Age");
                            dt7.Columns.Add("Smoker");
                            dt7.Columns.Add("CountryofResidence");
                            dt7.Columns.Add("Nationality");

                            DataRow dr8 = dt7.NewRow();
                            dr8["Name"] = Convert.ToString(dst_dt.Tables[0].Rows[0]["Name2"]);

                            if (v_gender2 == 0)
                            {
                                dr8["Gender"] = "Male";
                            }
                            else
                            {
                                dr8["Gender"] = "Female";
                            }

                            string dob2 = "";
                            try
                            {
                                dob2 = Convert.ToString(dst_dt.Tables[0].Rows[0]["dob2"]);

                            }
                            catch
                            {

                            }

                            dr8["DateofBirth"] =  DateTime.Parse(dob2).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture); ;
                            dr8["Age"] = v_age2;

                            if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Habit2"]) == 0)
                            {
                                dr8["Smoker"] = "Non-Smoker";
                            }
                            else
                            {
                                dr8["Smoker"] = "Smoker";
                            }

                            dr8["CountryofResidence"] = "UAE";
                            dr8["Nationality"] = Convert.ToString(dst_dt.Tables[0].Rows[0]["Nationality2"]);

                            country2 = Convert.ToString(dst_dt.Tables[0].Rows[0]["BirthCountry2"]);

                            if (Convert.ToString(dst_dt.Tables[0].Rows[0]["BirthCountry2"]) == Convert.ToString(dst_dt.Tables[0].Rows[0]["Nationality2"]))
                            {
                                dr8["Nationality"] = Convert.ToString(dst_dt.Tables[0].Rows[0]["Nationality2"]);
                            }
                            else
                            {
                                dr8["Nationality"] = Convert.ToString(dst_dt.Tables[0].Rows[0]["Nationality2"]) + " / " + Convert.ToString(dst_dt.Tables[0].Rows[0]["BirthCountry2"]);
                            }
                            dt7.Rows.Add(dr8);
                        }






                        DataTable dt4 = new DataTable();
                        dt4.Clear();
                        dt4.Columns.Add("BenefitsRiders");
                        dt4.Columns.Add("BenefitsRidersAR");
                        dt4.Columns.Add("BasisofPayment");                      // Benefit Rider details contribution
                        dt4.Columns.Add("BasisofPaymentAR");
                        dt4.Columns.Add("CoveredAmount");
                        dt4.Columns.Add("Term");

                        DataTable dt9 = new DataTable();
                        dt9.Clear();
                        dt9.Columns.Add("BenefitsRiders");
                        dt9.Columns.Add("BenefitsRidersAR");
                        dt9.Columns.Add("BasisofPayment");
                        dt9.Columns.Add("BasisofPaymentAR");
                        dt9.Columns.Add("CoveredAmount");
                        dt9.Columns.Add("Term");

                        DataTable dt10 = new DataTable();
                        dt10.Clear();
                        dt10.Columns.Add("BenefitsRiders");
                        dt10.Columns.Add("BenefitsRidersAR");
                        dt10.Columns.Add("BasisofPayment");
                        dt10.Columns.Add("BasisofPaymentAR");
                        dt10.Columns.Add("CoveredAmount");
                        dt10.Columns.Add("Term");


                        //  Funds growth return table form the data grid on the form (gv_fund20)
                        DataTable dt30 = new DataTable();
                        dt30.Clear();
                        dt30.Columns.Add("P_Year");
                        dt30.Columns.Add("Prem_Paid");
                        dt30.Columns.Add("fund");
                        dt30.Columns.Add("csv");
                        dt30.Columns.Add("p_year1");
                        dt30.Columns.Add("prem_paid1");
                        dt30.Columns.Add("fund1");
                        dt30.Columns.Add("csv1");


                        //effects of fluctuation of growth rate table details.

                        DataTable dt15 = new DataTable();
                        dt15.Clear();
                        dt15.Columns.Add("A5");
                        dt15.Columns.Add("A10");
                        dt15.Columns.Add("A15");
                        dt15.Columns.Add("A20");
                        dt15.Columns.Add("A80");
                        dt15.Columns.Add("A90");
                        dt15.Columns.Add("A95");
                        DataTable dt16 = new DataTable();
                        dt16.Clear();
                        dt16.Columns.Add("B5");
                        dt16.Columns.Add("B10");
                        dt16.Columns.Add("B15");
                        dt16.Columns.Add("B20");
                        dt16.Columns.Add("B80");
                        dt16.Columns.Add("B90");
                        dt16.Columns.Add("B95");

                        DataTable dt17 = new DataTable();
                        dt17.Clear();
                        dt17.Columns.Add("C5");
                        dt17.Columns.Add("C10");
                        dt17.Columns.Add("C15");
                        dt17.Columns.Add("C20");
                        dt17.Columns.Add("C80");
                        dt17.Columns.Add("C90");
                        dt17.Columns.Add("C95");
                        DataSet dst_grw = new DataSet();
                        dst_grw.Clear();

                        if ((Woc2 == 1) && (Woc == 1))
                        {
                            Woc_type = 2;
                        }
                        else if ((Woc2 == 1))
                        {
                            Woc = 1;
                            Woc_type = 1;
                        }
                        else if ((Woc == 1))
                        {
                            Woc_type = 0;
                        }
                        else
                        {
                            Woc = 0;
                        }
                        Boolean non_resident = false;
                     


                        double p_load = 0;
                        double pptd_load = 0;

                      

                        try
                        {

                            //dst_grw = service.Hyat_cal_Grw(user, plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, 3, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);
                            dst_grw = service1.Hyat_cal_Grw( plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, 3, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);


                            if (dst_grw.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr11 = dt15.NewRow();
                                try
                                {
                                    dr11["A5"] = Math.Round((double)dst_grw.Tables[0].Rows[0][1]);
                                }
                                catch
                                {
                                    dr11["A5"] = 0;
                                }
                                try
                                {
                                    dr11["A10"] = Math.Round((double)dst_grw.Tables[0].Rows[1][1]);
                                }
                                catch
                                {
                                    dr11["A10"] = 0;
                                }
                                try
                                {
                                    dr11["A15"] = Math.Round((double)dst_grw.Tables[0].Rows[2][1]);
                                }
                                catch
                                {
                                    dr11["A15"] = 0;
                                }
                                try
                                {
                                    dr11["A20"] = Math.Round((double)dst_grw.Tables[0].Rows[3][1]);
                                }
                                catch
                                {
                                    dr11["A20"] = 0;
                                }

                                try
                                {
                                    dr11["A80"] = Math.Round((double)dst_grw.Tables[0].Rows[4][1]);
                                }
                                catch
                                {
                                    dr11["A80"] = 0;
                                }
                                try
                                {
                                    dr11["A90"] = Math.Round((double)dst_grw.Tables[0].Rows[5][1]);
                                }
                                catch
                                {
                                    dr11["A90"] = 0;
                                }
                                try
                                {
                                    dr11["A95"] = Math.Round((double)dst_grw.Tables[0].Rows[6][1]);
                                }
                                catch
                                {
                                    dr11["A95"] = 0;
                                }

                                dt15.Rows.Add(dr11);
                            }
                        }
                        catch
                        {
                        }


                        dst_grw = new DataSet();
                        dst_grw.Clear();
                        try
                        {
                           // dst_grw = service.Hyat_cal_Grw(user, plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, 5, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);
                            dst_grw = service1.Hyat_cal_Grw( plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, 5, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);

                            if (dst_grw.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr11 = dt16.NewRow();
                                try
                                {
                                    dr11["B5"] = Math.Round((double)dst_grw.Tables[0].Rows[0][1]);
                                }
                                catch
                                {
                                    dr11["B5"] = 0;
                                }
                                try
                                {
                                    dr11["B10"] = Math.Round((double)dst_grw.Tables[0].Rows[1][1]);
                                }
                                catch
                                {
                                    dr11["B10"] = 0;
                                }
                                try
                                {
                                    dr11["B15"] = Math.Round((double)dst_grw.Tables[0].Rows[2][1]);
                                }
                                catch
                                {
                                    dr11["B15"] = 0;
                                }
                                try
                                {
                                    dr11["B20"] = Math.Round((double)dst_grw.Tables[0].Rows[3][1]);
                                }
                                catch
                                {
                                    dr11["B20"] = 0;
                                }

                                try
                                {
                                    dr11["B80"] = Math.Round((double)dst_grw.Tables[0].Rows[4][1]);
                                }
                                catch
                                {
                                    dr11["B80"] = 0;
                                }
                                try
                                {
                                    dr11["B90"] = Math.Round((double)dst_grw.Tables[0].Rows[5][1]);
                                }
                                catch
                                {
                                    dr11["B90"] = 0;
                                }
                                try
                                {
                                    dr11["B95"] = Math.Round((double)dst_grw.Tables[0].Rows[6][1]);
                                }
                                catch
                                {
                                    dr11["B95"] = 0;
                                }

                                dt16.Rows.Add(dr11);
                            }
                        }
                        catch
                        {
                        }




                        dst_grw = new DataSet();
                        dst_grw.Clear();
                        try
                        {
                           // dst_grw = service.Hyat_cal_Grw(user, plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, 7, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);

                            dst_grw = service1.Hyat_cal_Grw( plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, 7, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);
                            if (dst_grw.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr11 = dt17.NewRow();
                                try
                                {
                                    dr11["C5"] = Math.Round((double)dst_grw.Tables[0].Rows[0][1]);
                                }
                                catch
                                {
                                    dr11["C5"] = 0;
                                }
                                try
                                {
                                    dr11["C10"] = Math.Round((double)dst_grw.Tables[0].Rows[1][1]);
                                }
                                catch
                                {
                                    dr11["C10"] = 0;
                                }
                                try
                                {
                                    dr11["C15"] = Math.Round((double)dst_grw.Tables[0].Rows[2][1]);
                                }
                                catch
                                {
                                    dr11["C15"] = 0;
                                }
                                try
                                {
                                    dr11["C20"] = Math.Round((double)dst_grw.Tables[0].Rows[3][1]);
                                }
                                catch
                                {
                                    dr11["C20"] = 0;
                                }

                                try
                                {
                                    dr11["C80"] = Math.Round((double)dst_grw.Tables[0].Rows[4][1]);
                                }
                                catch
                                {
                                    dr11["C80"] = 0;
                                }
                                try
                                {
                                    dr11["C90"] = Math.Round((double)dst_grw.Tables[0].Rows[5][1]);
                                }
                                catch
                                {
                                    dr11["C90"] = 0;
                                }
                                try
                                {
                                    dr11["C95"] = Math.Round((double)dst_grw.Tables[0].Rows[6][1]);
                                }
                                catch
                                {
                                    dr11["C95"] = 0;
                                }

                                dt17.Rows.Add(dr11);
                            }
                        }
                        catch
                        {
                        }
                        DataSet dst_fund20 = new DataSet();
                        int i = 0;
                        DataTable dtFund20 = new DataTable();
                        DataRow drFund20;
                        DataTable dtFund100 = new DataTable();
                        DataRow drFund100;

                        dtFund20.Columns.Add(new DataColumn("PolicyYear", Type.GetType("System.String")));
                        dtFund20.Columns.Add(new DataColumn("ContributionPaid", Type.GetType("System.String")));
                        dtFund20.Columns.Add(new DataColumn("FundValue", Type.GetType("System.String")));
                        dtFund20.Columns.Add(new DataColumn("CashValue", Type.GetType("System.String")));


                        dtFund100.Columns.Add(new DataColumn("PolicyYear", Type.GetType("System.String")));
                        dtFund100.Columns.Add(new DataColumn("ContributionPaid", Type.GetType("System.String")));
                        dtFund100.Columns.Add(new DataColumn("FundValue", Type.GetType("System.String")));
                        dtFund100.Columns.Add(new DataColumn("CashValue", Type.GetType("System.String")));
                        try
                        {
                          //  dst_fund20 = service.Hyat_cal_contribution(user, plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, growthRate, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);

                            dst_fund20 = service1.Hyat_cal_contribution( plan_code, joint_life, contri, v_curr, V_Freq, v_trms, v_age, v_age2, v_gender, v_gender2, v_smoker, v_smoker2, growthRate, v_sa, v_sa2, Nation_discount, Nation_discount2, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, Woc_type, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, ci_amt2, ptd_amt2, hcb_amt2, adb_amt2, atpd_amt2, fib_amt2, fib_term2, p_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);

                        }
                        catch
                        {
                        }

                        try
                        {

                            if (dst_fund20.Tables[0].Rows.Count > 0)
                            {
                                for (i = 0; i <= dst_fund20.Tables[0].Rows.Count - 1; i++)
                                {
                                    drFund20 = dtFund20.NewRow();

                                    drFund20["PolicyYear"] = Convert.ToInt32(dst_fund20.Tables[0].Rows[i][0]);
                                    drFund20["ContributionPaid"] = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][1]).ToString("#,##0");
                                    drFund20["FundValue"] = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][2]).ToString("#,##0");
                                    drFund20["CashValue"] = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][3]).ToString("#,##0");
                                    dtFund20.Rows.Add(drFund20);


                                }

                            }


                        }

                        catch
                        {
                        }

                        try
                        {

                            if (dst_fund20.Tables[1].Rows.Count > 0)
                            {
                                for (i = 0; i <= dst_fund20.Tables[1].Rows.Count - 1; i++)
                                {
                                    drFund100 = dtFund100.NewRow();

                                    drFund100["PolicyYear"] = Convert.ToInt32(dst_fund20.Tables[1].Rows[i][0]);
                                    drFund100["ContributionPaid"] = Convert.ToDouble((double)dst_fund20.Tables[1].Rows[i][1]).ToString("#,##0");
                                    drFund100["FundValue"] = Convert.ToDouble((double)dst_fund20.Tables[1].Rows[i][2]).ToString("#,##0");
                                    drFund100["CashValue"] = Convert.ToDouble((double)dst_fund20.Tables[1].Rows[i][3]).ToString("#,##0");

                                    dtFund100.Rows.Add(drFund100);


                                }

                            }


                        }

                        catch
                        {
                        }

                        try
                        {


                            for (i = 0; i < 20; i++)
                            {

                                if (i >= Convert.ToInt32(dtFund100.Rows.Count))
                                {
                                    dr_fund = dt30.NewRow();

                                    dr_fund["P_Year"] = dtFund20.Rows[i][0];
                                    dr_fund["Prem_Paid"] = dtFund20.Rows[i][1];
                                    dr_fund["fund"] = dtFund20.Rows[i][2];
                                    dr_fund["csv"] = dtFund20.Rows[i][3];
                                    dr_fund["p_year1"] = "";
                                    dr_fund["prem_paid1"] = "1.111";
                                    dr_fund["fund1"] = "1.111";
                                    dr_fund["csv1"] = "1.111";
                                    dt30.Rows.Add(dr_fund);
                                }

                                else
                                {
                                    dr_fund = dt30.NewRow();
                                    dr_fund["P_Year"] = dtFund20.Rows[i][0];
                                    dr_fund["Prem_Paid"] = dtFund20.Rows[i][1];
                                    dr_fund["fund"] = dtFund20.Rows[i][2];
                                    dr_fund["csv"] = dtFund20.Rows[i][3];
                                    dr_fund["p_year1"] = dtFund100.Rows[i][0];
                                    dr_fund["prem_paid1"] = dtFund100.Rows[i][1];
                                    dr_fund["fund1"] = dtFund100.Rows[i][2];
                                    dr_fund["csv1"] = dtFund100.Rows[i][3];
                                    dt30.Rows.Add(dr_fund);
                                }

                            }

                        }

                        catch
                        {

                        }



                        if (joint_life == 1)
                        {
                            DataRow dr4 = dt9.NewRow();
                            dr4["BenefitsRiders"] = "Family Takaful Benefit including Terminal Illness";
                            dr4["BenefitsRidersAR"] = "منفعة التكافل العائلي متضمنة المرض المميت";
                            dr4["BasisofPayment"] = "Inclusive";
                            dr4["BasisofPaymentAR"] = "ضمنى";
                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(v_sa).Replace(",", string.Empty))).ToString("#,##0");
                            dr4["Term"] = Convert.ToString(100 - Convert.ToDouble(v_age));
                            dt9.Rows.Add(dr4);

                            DataRow dr3 = dt10.NewRow();
                            dr3["BenefitsRiders"] = "Family Takaful Benefit including Terminal Illness";
                            dr3["BenefitsRidersAR"] = "منفعة التكافل العائلي متضمنة المرض المميت";
                            dr3["BasisofPayment"] = "Inclusive";
                            dr3["BasisofPaymentAR"] = "ضمنى";
                            dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(v_sa2).Replace(",", string.Empty))).ToString("#,##0");
                            dr3["Term"] = Convert.ToString(100 - Convert.ToDouble(v_age));
                            dt10.Rows.Add(dr3);

                        }

                        else
                        {

                            DataRow dr4 = dt4.NewRow();
                            dr4["BenefitsRiders"] = "Family Takaful Benefit including Terminal Illness";
                            dr4["BenefitsRidersAR"] = "منفعة التكافل العائلي متضمنة المرض المميت";
                            dr4["BasisofPayment"] = "Inclusive";
                            dr4["BasisofPaymentAR"] = "ضمنى";
                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(v_sa).Replace(",", string.Empty))).ToString("#,##0");
                            dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                            dt4.Rows.Add(dr4);
                        }

                        if (ci == 1)
                        {

                            if (joint_life == 1)

                            {
                                if (Convert.ToInt32(ci_amt) > 0)
                                {

                                    DataRow dr4 = dt9.NewRow();
                                    dr4["BenefitsRiders"] = "Critical Illness";
                                    dr4["BenefitsRidersAR"] = "المرض العضال";
                                    dr4["BasisofPayment"] = "Prepayment";
                                    dr4["BasisofPaymentAR"] = "دفع مسبق";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ci_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                    dt9.Rows.Add(dr4);
                                }

                                if (Convert.ToInt32(ci_amt2) > 0)
                                {

                                    DataRow dr4 = dt10.NewRow();
                                    dr4["BenefitsRiders"] = "Critical Illness";
                                    dr4["BenefitsRidersAR"] = "المرض العضال";
                                    dr4["BasisofPayment"] = "Prepayment";
                                    dr4["BasisofPaymentAR"] = "دفع مسبق";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ci_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                    dt10.Rows.Add(dr4);
                                }



                            }
                            else
                            {

                                DataRow dr4 = dt4.NewRow();
                                dr4["BenefitsRiders"] = "Critical Illness";
                                dr4["BenefitsRidersAR"] = "المرض العضال";
                                dr4["BasisofPayment"] = "Prepayment";
                                dr4["BasisofPaymentAR"] = "دفع مسبق";
                                dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ci_amt).Replace(",", string.Empty))).ToString("#,##0");
                                dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                dt4.Rows.Add(dr4);
                            }

                        }

                        if (Woc == 1)
                        {

                            if (joint_life == 1)
                            {
                                if (Woc_type == 0)
                                {

                                    if (Convert.ToInt32(75 - Convert.ToInt32(v_age)) < Convert.ToInt32(v_trms))
                                    {

                                        DataRow dr4 = dt9.NewRow();
                                        dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                        dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = "Applicable";
                                        dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                                        dt9.Rows.Add(dr4);

                                    }
                                    else
                                    {

                                        DataRow dr4 = dt9.NewRow();
                                        dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                        dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = "Applicable";
                                        dr4["Term"] = Convert.ToInt32(v_trms);
                                        dt9.Rows.Add(dr4);
                                    }
                                }
                                else if (Woc_type == 1)
                                {

                                    if (Convert.ToInt32(75 - Convert.ToInt32(v_age2)) < Convert.ToInt32(v_trms))
                                    {

                                        DataRow dr4 = dt10.NewRow();
                                        dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                        dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = "Applicable";
                                        dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age2));
                                        dt10.Rows.Add(dr4);

                                    }
                                    else
                                    {

                                        DataRow dr4 = dt10.NewRow();
                                        dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                        dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = "Applicable";
                                        dr4["Term"] = Convert.ToInt32(v_trms);
                                        dt10.Rows.Add(dr4);
                                    }
                                }

                                else if (Woc_type == 2)
                                {

                                    if (Convert.ToInt32(v_age2) > Convert.ToInt32(v_age))
                                    {
                                        // Nothing

                                    }

                                    else
                                    {


                                        if (75 - (Convert.ToInt32(v_age)) < Convert.ToInt32(v_trms))
                                        {
                                            DataRow dr4 = dt9.NewRow();
                                            dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                            dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = "Applicable";
                                            dr4["Term"] = Convert.ToInt32(75 - (Convert.ToInt32(v_age)));
                                            dt9.Rows.Add(dr4);

                                        }

                                        else
                                        {
                                            DataRow dr4 = dt9.NewRow();
                                            dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                            dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = "Applicable";
                                            dr4["Term"] = Convert.ToInt32(v_trms);
                                            dt9.Rows.Add(dr4);



                                        }


                                        if (75 - (Convert.ToInt32(v_age2)) < Convert.ToInt32(v_trms))
                                        {

                                            DataRow dr5 = dt10.NewRow();
                                            dr5["BenefitsRiders"] = "Waiver Of Contribution";
                                            dr5["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                            dr5["BasisofPayment"] = "Additional";
                                            dr5["BasisofPaymentAR"] = "إضافية";
                                            dr5["CoveredAmount"] = "Applicable";
                                            dr5["Term"] = Convert.ToInt32(75 - (Convert.ToInt32(v_age2)));
                                            dt10.Rows.Add(dr5);
                                        }
                                        else
                                        {
                                            DataRow dr5 = dt10.NewRow();
                                            dr5["BenefitsRiders"] = "Waiver Of Contribution";
                                            dr5["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                            dr5["BasisofPayment"] = "Additional";
                                            dr5["BasisofPaymentAR"] = "إضافية";
                                            dr5["CoveredAmount"] = "Applicable";
                                            dr5["Term"] = Convert.ToInt32(v_trms);
                                            dt10.Rows.Add(dr5);

                                        }



                                    }

                                }

                            }

                            else
                            {
                                if (Convert.ToInt32(75 - Convert.ToInt32(v_age)) < Convert.ToInt32(v_trms))
                                {
                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                    dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = "Applicable";
                                    dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                                    dt4.Rows.Add(dr4);
                                }
                                else
                                {
                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Waiver Of Contribution";
                                    dr4["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = "Applicable";
                                    dr4["Term"] = Convert.ToInt32(v_trms);
                                    dt4.Rows.Add(dr4);
                                }

                            }
                        }

                        if (ptd == 1)
                        {

                            if (joint_life == 1)
                            {
                                if (Convert.ToInt32(v_age2) > Convert.ToInt32(v_age))
                                {


                                }
                                else
                                {
                                    if (Convert.ToInt32(ptd_amt) > 0)
                                    {

                                        DataRow dr4 = dt9.NewRow();
                                        dr4["BenefitsRiders"] = "Permanent Total Disability";
                                        dr4["BenefitsRidersAR"] = " العجز الكلى الدائم";
                                        dr4["BasisofPayment"] = "Prepayment";
                                        dr4["BasisofPaymentAR"] = "دفع مسبق";
                                        dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ptd_amt).Replace(",", string.Empty))).ToString("#,##0");
                                        dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                                        dt9.Rows.Add(dr4);
                                    }
                                    if (Convert.ToInt32(75 - Convert.ToInt32(v_age2)) < Convert.ToInt32(100 - Convert.ToInt32(v_age)))
                                    {
                                        if (Convert.ToInt32(ptd_amt2) > 0)
                                        {

                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Permanent Total Disability";
                                            dr4["BenefitsRidersAR"] = " العجز الكلى الدائم";
                                            dr4["BasisofPayment"] = "Prepayment";
                                            dr4["BasisofPaymentAR"] = "دفع مسبق";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ptd_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age2));
                                            dt10.Rows.Add(dr4);
                                        }
                                    }
                                    else
                                        if (Convert.ToInt32(ptd_amt2) > 0)
                                    {

                                        DataRow dr4 = dt10.NewRow();
                                        dr4["BenefitsRiders"] = "Permanent Total Disability";
                                        dr4["BenefitsRidersAR"] = " العجز الكلى الدائم";
                                        dr4["BasisofPayment"] = "Prepayment";
                                        dr4["BasisofPaymentAR"] = "دفع مسبق";
                                        dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ptd_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                        dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                        dt10.Rows.Add(dr4);

                                    }

                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(ptd_amt) > 0)
                                {

                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Permanent Total Disability";
                                    dr4["BenefitsRidersAR"] = " العجز الكلى الدائم";
                                    dr4["BasisofPayment"] = "Prepayment";
                                    dr4["BasisofPaymentAR"] = "دفع مسبق";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ptd_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                                    dt4.Rows.Add(dr4);
                                }
                            }
                        }

                        if (adb == 1)
                        {

                            if (joint_life == 1)
                            {
                                if (Convert.ToInt32(v_age) >= Convert.ToInt32(v_age2))
                                {
                                    if (adb_amt > 0)
                                    {


                                        DataRow dr4 = dt9.NewRow();
                                        dr4["BenefitsRiders"] = "Accidental Death Benefit";
                                        dr4["BenefitsRidersAR"] = "منفعة الوفاة العرضية";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = adb_amt;
                                        dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                                        dt9.Rows.Add(dr4);
                                    }


                                    if (Convert.ToInt32(75 - Convert.ToInt32(v_age2)) < Convert.ToInt32(100 - Convert.ToInt32(v_age)))
                                    {
                                        if (adb_amt2 > 0)
                                        {

                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Accidental Death Benefit";
                                            dr4["BenefitsRidersAR"] = "منفعة الوفاة العرضية";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(adb_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age2));
                                            dt10.Rows.Add(dr4);
                                        }

                                    }
                                    else
                                    {

                                        if (adb_amt2 > 0)
                                        {

                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Accidental Death Benefit";
                                            dr4["BenefitsRidersAR"] = "منفعة الوفاة العرضية";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(adb_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                            dt10.Rows.Add(dr4);
                                        }


                                    }

                                }
                            }




                            else
                            {
                                if (Convert.ToInt32(adb_amt) > 0)
                                {

                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Accidental Death Benefit";
                                    dr4["BenefitsRidersAR"] = "منفعة الوفاة العرضية";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(adb_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                                    dt4.Rows.Add(dr4);
                                }

                            }

                        }


                        if (fib == 1)
                        {

                            if (joint_life == 1)
                            {

                                if ((fib_amt) > 0)
                                {

                                    DataRow dr4 = dt9.NewRow();
                                    dr4["BenefitsRiders"] = "Family Income Benefit(Monthly)";
                                    dr4["BenefitsRidersAR"] = "منفعة دخل العائلة (شهرى)";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(fib_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(Convert.ToInt32(fib_term));
                                    dt9.Rows.Add(dr4);
                                }

                                if ((fib_amt2) > 0)
                                {

                                    DataRow dr4 = dt10.NewRow();
                                    dr4["BenefitsRiders"] = "Family Income Benefit(Monthly)";
                                    dr4["BenefitsRidersAR"] = "منفعة دخل العائلة (شهرى)";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(fib_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(Convert.ToInt32(fib_term2));
                                    dt10.Rows.Add(dr4);
                                }

                            }

                            else
                            {
                                if (Convert.ToInt32(fib_amt) > 0)
                                {

                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Family Income Benefit(Monthly)";
                                    dr4["BenefitsRidersAR"] = "منفعة دخل العائلة (شهرى)";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(fib_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(Convert.ToInt32(fib_term));
                                    dt4.Rows.Add(dr4);
                                }
                            }
                        }

                        if (hcb == 1)
                        {

                            if (joint_life == 1)
                            {
                                if (Convert.ToInt32(v_age2) > Convert.ToInt32(v_age))
                                {

                                }
                                else
                                {
                                    if (hcb_amt > 0)
                                    {

                                        DataRow dr4 = dt9.NewRow();
                                        dr4["BenefitsRiders"] = "Hospital Cash Benefit(Daily)";
                                        dr4["BenefitsRidersAR"] = "منفعة الإستشفاء النقدى (يومى)";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = hcb_amt;
                                        dr4["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age));
                                        dt9.Rows.Add(dr4);
                                    }

                                    if (Convert.ToInt32(70 - Convert.ToInt32(v_age2)) < Convert.ToInt32(100 - Convert.ToInt32(v_age)))
                                    {
                                        if (hcb_amt2 > 0)
                                        {

                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Hospital Cash Benefit(Daily)";
                                            dr4["BenefitsRidersAR"] = "منفعة الإستشفاء النقدى (يومى)";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(hcb_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age2));
                                            dt10.Rows.Add(dr4);
                                        }
                                    }
                                    else
                                    {
                                        if (hcb_amt2 > 0)
                                        {

                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Hospital Cash Benefit(Daily)";
                                            dr4["BenefitsRidersAR"] = "منفعة الإستشفاء النقدى (يومى)";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(hcb_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                            dt10.Rows.Add(dr4);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(hcb_amt) > 0)
                                {

                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Hospital Cash Benefit(Daily)";
                                    dr4["BenefitsRidersAR"] = "منفعة الإستشفاء النقدى (يومى)";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(hcb_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age));
                                    dt4.Rows.Add(dr4);
                                }
                            }

                        }


                        if (atpd == 1)
                        {

                            if (joint_life == 1)
                            {
                                if (Convert.ToInt32(v_age) >= Convert.ToInt32(v_age2))
                                {


                                    if (atpd_amt > 0)
                                    {

                                        DataRow dr4 = dt9.NewRow();
                                        dr4["BenefitsRiders"] = "Accidental Total or Partial Permanent Disability(Accidental Dismemberment Benefit)";
                                        dr4["BenefitsRidersAR"] = "منفعة العجز الدائم الكلي أو الجزئي العرضي (منفعة فقدان الأعضاء العرضي)";
                                        dr4["BasisofPayment"] = "Additional";
                                        dr4["BasisofPaymentAR"] = "إضافية";
                                        dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(atpd_amt).Replace(",", string.Empty))).ToString("#,##0");
                                        dr4["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age));
                                        dt9.Rows.Add(dr4);
                                    }
                                    if (Convert.ToInt32(70 - Convert.ToInt32(v_age2)) < Convert.ToInt32(100 - Convert.ToInt32(v_age)))
                                    {
                                        if (atpd_amt2 > 0)
                                        {


                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Accidental Total or Partial Permanent Disability(Accidental Dismemberment Benefit)";
                                            dr4["BenefitsRidersAR"] = "منفعة العجز الدائم الكلي أو الجزئي العرضي (منفعة فقدان الأعضاء العرضي)";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(atpd_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age2));
                                            dt10.Rows.Add(dr4);
                                        }

                                    }
                                    else
                                    {
                                        if (atpd_amt2 > 0)
                                        {

                                            DataRow dr4 = dt10.NewRow();
                                            dr4["BenefitsRiders"] = "Accidental Total or Partial Permanent Disability(Accidental Dismemberment Benefit)";
                                            dr4["BenefitsRidersAR"] = "منفعة العجز الدائم الكلي أو الجزئي العرضي (منفعة فقدان الأعضاء العرضي)";
                                            dr4["BasisofPayment"] = "Additional";
                                            dr4["BasisofPaymentAR"] = "إضافية";
                                            dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(atpd_amt2).Replace(",", string.Empty))).ToString("#,##0");
                                            dr4["Term"] = Convert.ToString(100 - Convert.ToInt32(v_age));
                                            dt10.Rows.Add(dr4);

                                        }
                                    }



                                }

                            }
                            else
                            {
                                if (Convert.ToInt32(atpd_amt) > 0)
                                {

                                    DataRow dr4 = dt4.NewRow();
                                    dr4["BenefitsRiders"] = "Accidental Total or Partial Permanent Disability(Accidental Dismemberment Benefit)";
                                    dr4["BenefitsRidersAR"] = "منفعة العجز الدائم الكلي أو الجزئي العرضي (منفعة فقدان الأعضاء العرضي)";
                                    dr4["BasisofPayment"] = "Additional";
                                    dr4["BasisofPaymentAR"] = "إضافية";
                                    dr4["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(atpd_amt).Replace(",", string.Empty))).ToString("#,##0");
                                    dr4["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age));
                                    dt4.Rows.Add(dr4);
                                }
                            }
                        }


                        DataTable dt3 = new DataTable();
                        dt3.Clear();
                        dt3.Columns.Add("MedicalReqs");

                        DataTable dt5 = new DataTable();
                        dt5.Clear();
                        dt5.Columns.Add("MedicalReqs");


                        string smoker1 = "";
                        string smoker2 = "";
                        if (Convert.ToInt16(dstCustomer_dt.Tables[0].Rows[0]["Habit"]) == 1)
                        {
                            smoker1 = "Y";
                        }
                        else
                        {
                            smoker1 = "N";
                        }

                        if (joint_life == 1)
                        {
                            if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Habit2"]) == 1)
                            {
                                smoker2 = "Y";
                            }
                            else
                            {
                                smoker2 = "N";
                            }
                        }


                        double msum_value = 0;
                        double msum_value2 = 0;

                        if (v_curr == "USD")
                        {
                            msum_value = (v_sa + (fib_amt * 12 * fib_term * 0.5)) * 3.671;

                        }
                        else
                        {
                            msum_value = (v_sa + (fib_amt * 12 * fib_term * 0.5));
                        }
                        if (joint_life == 1)
                        {

                            if (v_curr == "USD")
                            {
                                msum_value2 = (v_sa2 + (fib_amt2 * 12 * fib_term2 * 0.5)) * 3.671;

                            }
                            else
                            {
                                msum_value2 = (v_sa2 + (fib_amt2 * 12 * fib_term2 * 0.5));
                            }
                        }



                        //if ((Resident.Text.ToString().ToUpper()) == "INDIA" || (Resident.Text.ToString().ToUpper()) == "PAKISTAN")
                        //{
                        //    DataRow dr3 = dt3.NewRow();
                        //    dr3["MedicalReqs"] = "* Medical underwriting requirements would be provided by SALAMA";
                        //    dt3.Rows.Add(dr3);
                        //}
                        //else
                        //{

                        dstMed = Fn_Common.Medical(v_age, msum_value, "HYPR", smoker1);

                        if (dstMed.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j <= dstMed.Tables[0].Rows.Count - 1; j++)
                            {
                                DataRow dr3 = dt3.NewRow();
                                dr3["MedicalReqs"] = j + 1 + ")  " + dstMed.Tables[0].Rows[j][0];
                                dt3.Rows.Add(dr3);
                            }
                            DataRow dr5 = dt3.NewRow();
                            dr5["MedicalReqs"] = "* SALAMA reserves the right to call for any additional medical if deemed necessary";
                            dt3.Rows.Add(dr5);
                        }
                        else
                        {
                            DataRow dr3 = dt3.NewRow();
                            dr3["MedicalReqs"] = "1) No Medical Examination Required";
                            dt3.Rows.Add(dr3);

                            DataRow dr5 = dt3.NewRow();
                            dr5["MedicalReqs"] = "* SALAMA reserves the right to call for any additional medical if deemed necessary";
                            dt3.Rows.Add(dr5);
                        }


                        if (joint_life == 1)
                        {



                            dstMed = Fn_Common.Medical(v_age2, msum_value2, "HYPR", smoker2);

                            if (dstMed.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j <= dstMed.Tables[0].Rows.Count - 1; j++)
                                {
                                    DataRow dr11 = dt5.NewRow();
                                    dr11["MedicalReqs"] = j + 1 + ")  " + dstMed.Tables[0].Rows[j][0];
                                    dt5.Rows.Add(dr11);
                                }

                                DataRow dr6 = dt5.NewRow();
                                dr6["MedicalReqs"] = "* SALAMA reserves the right to call for any additional medical if deemed necessary";
                                dt5.Rows.Add(dr6);
                            }
                            else
                            {
                                DataRow dr11 = dt5.NewRow();
                                dr11["MedicalReqs"] = "1) No Medical Examination required";
                                dt5.Rows.Add(dr11);

                                DataRow dr6 = dt5.NewRow();
                                dr6["MedicalReqs"] = "* SALAMA reserves the right to call for any additional medical if deemed necessary";
                                dt5.Rows.Add(dr6);
                            }
                        }

                        //}
                        DateTime valid_date, curr_date, curr_date1;
                        DateTime str_validdate = DateTime.Today;
                        valid_date = DateTime.Now.AddMonths(1);

                        curr_date = (Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age)).AddMonths(6);
                        curr_date1 = (Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age)).AddMonths(6);



                        if (joint_life == 1)
                        {
                            try
                            {
                                if (curr_date > curr_date1)
                                {
                                    if (valid_date >= curr_date1)
                                    {
                                        str_validdate = ((Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age2)).AddMonths(6)).AddDays(1);

                                    }
                                    else
                                    {
                                        str_validdate = valid_date;

                                    }
                                }
                                else
                                {
                                    if (valid_date >= curr_date)
                                    {
                                        str_validdate = ((Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age)).AddMonths(6)).AddDays(1);
                                    }
                                    else
                                    {
                                        str_validdate = valid_date;
                                    }
                                }
                            }
                            catch
                            {

                            }


                        }
                        else
                        {

                            try
                            {


                                if (valid_date >= curr_date)
                                {
                                    str_validdate = ((Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age)).AddMonths(6)).AddDays(1);
                                }

                                else
                                {
                                    str_validdate = valid_date;
                                }

                            }
                            catch
                            {

                            }
                        }

                        string strPath = "";


                        if (joint_life == 1)
                        {

                            reportViewer2.Reset();

                            if (Session["disb_uid"].ToString().ToUpper() == "SCB")
                            {
                                strPath = Server.MapPath("~/Hyat/Reports/RptHyatSuperiorJointSCB.rdlc");
                            }

                            else if (Session["disb_uid"].ToString().ToUpper() == "ENBD")
                            {


                                if ((plan_code == "ULP+") || (plan_code == "LULP+"))
                                {
                                    strPath = Server.MapPath("~/Hyat/Reports/RptLifeLongPlusJointENBD.rdlc");
                                }
                                else
                                {
                                    strPath = Server.MapPath("~/Hyat/Reports/RptHyatSuperiorJointENBD.rdlc");
                                }
                            }

                            else
                            {
                                strPath = Server.MapPath("~/Hyat/Reports/RptHyatSuperiorJoint.rdlc");
                            }

                            reportViewer2.LocalReport.ReportPath = strPath;
                            reportViewer2.ProcessingMode = ProcessingMode.Local;



                            ReportDataSource datasource = new ReportDataSource("dstHyatSupJnFirstMemDetails", dt);
                            ReportDataSource datasource1 = new ReportDataSource("dstHyatSupJnCovAndContr", dt2);
                            ReportDataSource datasource2 = new ReportDataSource("dstHyatSupJnFirstBenefits", dt9);
                            ReportDataSource datasource3 = new ReportDataSource("dstHyatSupJnFundGrowth", dt30);
                            ReportDataSource datasource4 = new ReportDataSource("dstHyatSupJnFirstMedical", dt3);
                            ReportDataSource datasource6 = new ReportDataSource("dstHyatSupJnSecondMemDetails", dt7);
                            ReportDataSource datasource7 = new ReportDataSource("dstHyatSupJnSecondBenefits", dt10);
                            ReportDataSource datasource8 = new ReportDataSource("dstHyatSupJnSecondMedical", dt5);
                            ReportDataSource datasource5 = new ReportDataSource("ThreeGrwJoint", dt15);
                            ReportDataSource datasource9 = new ReportDataSource("FiveGrwJoint", dt16);
                            ReportDataSource datasource10 = new ReportDataSource("SevenGrwJoint", dt17);


                            reportViewer2.LocalReport.DataSources.Clear();
                            reportViewer2.LocalReport.DataSources.Add(datasource);
                            reportViewer2.LocalReport.DataSources.Add(datasource1);
                            reportViewer2.LocalReport.DataSources.Add(datasource2);
                            reportViewer2.LocalReport.DataSources.Add(datasource3);
                            reportViewer2.LocalReport.DataSources.Add(datasource4);
                            reportViewer2.LocalReport.DataSources.Add(datasource6);
                            reportViewer2.LocalReport.DataSources.Add(datasource7);
                            reportViewer2.LocalReport.DataSources.Add(datasource8);
                            reportViewer2.LocalReport.DataSources.Add(datasource5);
                            reportViewer2.LocalReport.DataSources.Add(datasource9);
                            reportViewer2.LocalReport.DataSources.Add(datasource10);


                        }
                        else
                        {
                            reportViewer2.Reset();
                            if (Session["disb_uid"].ToString().ToUpper() == "SCB")
                            {
                                strPath = Server.MapPath("~/Hyat/Reports/HyatSuperiorSCB.rdlc");
                            }
                            else if (Session["disb_uid"].ToString().ToUpper() == "ENBD")

                            {

                                if ((plan_code == "ULP+") || (plan_code == "LULP+"))
                                {
                                    strPath = Server.MapPath("~/Hyat/Reports/LifeLongPlusENBD.rdlc");
                                }
                                else
                                {
                                    strPath = Server.MapPath("~/Hyat/Reports/LifeLongSuperiorENBD.RDLC");
                                }



                            }
                            else
                            {
                                strPath = Server.MapPath("~/Hyat/Reports/HyatSuperior.rdlc");
                            }

                            reportViewer2.LocalReport.ReportPath = strPath;
                            reportViewer2.ProcessingMode = ProcessingMode.Local;

                            ReportDataSource datasource = new ReportDataSource("dstHyatSupCovMemDetails", dt);
                            ReportDataSource datasource1 = new ReportDataSource("CovAndContrDetails", dt2);
                            ReportDataSource datasource2 = new ReportDataSource("dstHyatSupBenefits", dt4);
                            ReportDataSource datasource3 = new ReportDataSource("dstHyatSupFundGrowth", dt30);
                            ReportDataSource datasource4 = new ReportDataSource("dstHyatSupMedicalReq", dt3);
                            ReportDataSource datasource5 = new ReportDataSource("Threegrw", dt15);
                            ReportDataSource datasource6 = new ReportDataSource("Fivegrw", dt16);
                            ReportDataSource datasource7 = new ReportDataSource("Sevengrw", dt17);


                            reportViewer2.LocalReport.DataSources.Clear();
                            reportViewer2.LocalReport.DataSources.Add(datasource);
                            reportViewer2.LocalReport.DataSources.Add(datasource1);
                            reportViewer2.LocalReport.DataSources.Add(datasource2);
                            reportViewer2.LocalReport.DataSources.Add(datasource3);
                            reportViewer2.LocalReport.DataSources.Add(datasource4);
                            reportViewer2.LocalReport.DataSources.Add(datasource5);
                            reportViewer2.LocalReport.DataSources.Add(datasource6);
                            reportViewer2.LocalReport.DataSources.Add(datasource7);


                        }

                        string strLoad = "";

                        if (Convert.ToInt16(Session["Resident_Id"]) != 0)
                        {
                            strLoad = " NR Illustration.";

                        }


                        reportViewer2.LocalReport.EnableExternalImages = true;
                        string imagePath = new Uri(Server.MapPath("../Signature/" + mid1 + ".jpg")).AbsoluteUri;
                        try
                        {
                            ReportParameter parameter = new ReportParameter("sign1", imagePath);
                            reportViewer2.LocalReport.SetParameters(parameter);
                        }
                        catch (Exception ex)
                        {

                        }

                        if (joint_life == 1)
                        {
                            imagePath = new Uri(Server.MapPath("../Signature/" + mid1 + "_1.jpg")).AbsoluteUri;

                        }

                        else
                        {
                            imagePath = new Uri(Server.MapPath("../Signature/blank.jpg")).AbsoluteUri;
                        }
                        ReportParameter parameter2 = new ReportParameter("sign2", imagePath);
                        reportViewer2.LocalReport.SetParameters(parameter2);

                        //if (plan_holder == 1)
                        //{
                        //    imagePath = new Uri(Server.MapPath("~/Signature/" + mid + "_2.jpg")).AbsoluteUri;

                        //}
                        //else
                        //{
                        imagePath = new Uri(Server.MapPath("../Signature/blank.jpg")).AbsoluteUri;
                        //}
                        ReportParameter parameter3 = new ReportParameter("sign3", imagePath);
                        reportViewer2.LocalReport.SetParameters(parameter3);


                        ReportParameter[] parameters = new ReportParameter[19];



                        



                        if (joint_life == 1)
                        {
                            Session["Joint"] = 1;

                        }
                        else
                        {
                            Session["Joint"] = 0;
                        }

                        if (plan_code == "ULPS")
                        {

                            if ((Session["disb_uid"].ToString().ToUpper() == "DIB"))
                            {

                                parameters[0] = new ReportParameter("PlanCode", "MUS");
                                parameters[3] = new ReportParameter("RptName", "MUSTAQBAL - The Universal Takaful Plan");
                                parameters[4] = new ReportParameter("RptNameAR", " توضيح منافع التكافل مستقبل - خطة التكافل العالمي");
                                parameters[11] = new ReportParameter("declaration", "• In case of Death of a Covered Member, higher of the Fund Value or Family Takaful Benefit Amount is payable. ");
                                parameters[12] = new ReportParameter("declarationAR", "في حال وفاة العضو المغطى، يتم دفع قيمة الصندوق أو قيمة منفعة التكافل العائلي، أيهما أعلى.");
                                parameters[17] = new ReportParameter("HyatSup_Declare", "* Total Bonus Allocation of 70% of Initial or Reduced Annualized Contribution (whichever is lower) is distributable in Plan years 4, 5, 6  and 7.  Amounts shown in the illustration include the impact of Bonus Allocation for respective Plan Years.");
                                parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "إجمالى مكافأة التخصيص كنسبة 70% من المساهمة السنوية الأساسية أو المخفضة ( أيهما أقل) يتم توزيعها فى السنوات 4، 5، 6، و 7 من الخطة. القيم الموضحة فى التوضيح متضمنة مكافأة التخصيص  فى سنوات الخطة المرتبطة");


                            }
                            else if (Session["disb_uid"].ToString().ToLower() == "salama")
                            {
                                //if (Session["Sal"].ToString() == "1")
                                //{
                                //    parameters[0] = new ReportParameter("PlanCode", "MUS");
                                //    parameters[3] = new ReportParameter("RptName", "MUSTAQBAL - The Universal Takaful Plan");
                                //    parameters[4] = new ReportParameter("RptNameAR", " توضيح منافع التكافل مستقبل - خطة التكافل العالمي");
                                //    parameters[11] = new ReportParameter("declaration", "• In case of Death of a Covered Member, higher of the Fund Value or Family Takaful Benefit Amount is payable. ");
                                //    parameters[12] = new ReportParameter("declarationAR", "في حال وفاة العضو المغطى، يتم دفع قيمة الصندوق أو قيمة منفعة التكافل العائلي، أيهما أعلى.");
                                //    parameters[17] = new ReportParameter("HyatSup_Declare", "* Total Bonus Allocation of 70% of Initial or Reduced Annualized Contribution (whichever is lower) is distributable in Plan years 4, 5, 6  and 7.  Amounts shown in the illustration include the impact of Bonus Allocation for respective Plan Years.");
                                //    parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "إجمالى مكافأة التخصيص كنسبة 70% من المساهمة السنوية الأساسية أو المخفضة ( أيهما أقل) يتم توزيعها فى السنوات 4، 5، 6، و 7 من الخطة. القيم الموضحة فى التوضيح متضمنة مكافأة التخصيص  فى سنوات الخطة المرتبطة");

                                //}

                                //else
                                //{
                                parameters[0] = new ReportParameter("PlanCode", "HYS");
                                parameters[3] = new ReportParameter("RptName", "HYAT SUPERIOR");
                                parameters[4] = new ReportParameter("RptNameAR", "توضيح منافع التكافل حياة الأشمل ");
                                parameters[11] = new ReportParameter("declaration", "• In case of Death of a Covered Member, higher of the Fund Value or Family Takaful Benefit Amount is payable. ");
                                parameters[12] = new ReportParameter("declarationAR", "في حال وفاة العضو المغطى، يتم دفع قيمة الصندوق أو قيمة منفعة التكافل العائلي، أيهما أعلى.");
                                parameters[17] = new ReportParameter("HyatSup_Declare", "* Total Bonus Allocation of 70% of Initial or Reduced Annualized Contribution (whichever is lower) is distributable in Plan years 4, 5, 6  and 7.  Amounts shown in the illustration include the impact of Bonus Allocation for respective Plan Years.");
                                parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "إجمالى مكافأة التخصيص كنسبة 70% من المساهمة السنوية الأساسية أو المخفضة ( أيهما أقل) يتم توزيعها فى السنوات 4، 5، 6، و 7 من الخطة. القيم الموضحة فى التوضيح متضمنة مكافأة التخصيص  فى سنوات الخطة المرتبطة");


                                //}

                            }

                            else
                            {
                                parameters[0] = new ReportParameter("PlanCode", "HYS");
                                parameters[3] = new ReportParameter("RptName", "HYAT SUPERIOR");
                                parameters[4] = new ReportParameter("RptNameAR", "توضيح منافع التكافل حياة الأشمل ");
                                parameters[11] = new ReportParameter("declaration", "• In case of Death of a Covered Member, higher of the Fund Value or Family Takaful Benefit Amount is payable. ");
                                parameters[12] = new ReportParameter("declarationAR", "في حال وفاة العضو المغطى، يتم دفع قيمة الصندوق أو قيمة منفعة التكافل العائلي، أيهما أعلى.");


                                parameters[17] = new ReportParameter("HyatSup_Declare", "* Total Bonus Allocation of 70% of Initial or Reduced Annualized Contribution (whichever is lower) is distributable in Plan years 4, 5, 6  and 7.  Amounts shown in the illustration include the impact of Bonus Allocation for respective Plan Years.");
                                parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "إجمالى مكافأة التخصيص كنسبة 70% من المساهمة السنوية الأساسية أو المخفضة ( أيهما أقل) يتم توزيعها فى السنوات 4، 5، 6، و 7 من الخطة. القيم الموضحة فى التوضيح متضمنة مكافأة التخصيص  فى سنوات الخطة المرتبطة");




                            }
                        }
                        else if (plan_code == "ULP+")
                        {

                            parameters[0] = new ReportParameter("PlanCode", "HYP");
                            parameters[3] = new ReportParameter("RptName", "HYAT PLUS");
                            parameters[4] = new ReportParameter("RptNameAR", "‘توضيح منافع التكافل  ‘حياة الإضافية");
                            parameters[11] = new ReportParameter("declaration", "");
                            parameters[12] = new ReportParameter("declarationAR", "");
                            parameters[17] = new ReportParameter("HyatSup_Declare", "");
                            parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "");

                        }
                        else if (plan_code == "LULPS")
                        {
                            parameters[0] = new ReportParameter("PlanCode", "LS");
                            parameters[3] = new ReportParameter("RptName", "LIFELONG SUPERIOR");
                            parameters[4] = new ReportParameter("RptNameAR", "''توضيح منافع التكافل   '‘مدى الحياة الأشمل");
                            parameters[11] = new ReportParameter("declaration", "• In case of Death of a Covered Member, higher of the Fund Value or Family Takaful Benefit Amount is payable. ");
                            parameters[12] = new ReportParameter("declarationAR", "في حال وفاة العضو المغطى، يتم دفع قيمة الصندوق أو قيمة منفعة التكافل العائلي، أيهما أعلى.");


                            parameters[17] = new ReportParameter("HyatSup_Declare", "* Total Bonus Allocation of 70% of Initial or Reduced Annualized Contribution (whichever is lower) is distributable in Plan years 4, 5, 6  and 7.  Amounts shown in the illustration include the impact of Bonus Allocation for respective Plan Years.");
                            parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "إجمالى مكافأة التخصيص كنسبة 70% من المساهمة السنوية الأساسية أو المخفضة ( أيهما أقل) يتم توزيعها فى السنوات 4، 5، 6، و 7 من الخطة. القيم الموضحة فى التوضيح متضمنة مكافأة التخصيص  فى سنوات الخطة المرتبطة");



                        }
                        else if (plan_code == "LULP+")
                        {
                            parameters[0] = new ReportParameter("PlanCode", "LP");
                            parameters[3] = new ReportParameter("RptName", "LIFELONG PLUS");
                            parameters[4] = new ReportParameter("RptNameAR", "  توضيح منافع التكافل   مدى الحياة الإضافية");
                            parameters[11] = new ReportParameter("declaration", "");
                            parameters[12] = new ReportParameter("declarationAR", "");
                            parameters[17] = new ReportParameter("HyatSup_Declare", "");
                            parameters[18] = new ReportParameter("HyatSup_DeclareArabic", "");

                        }


                        if (joint_life == 1)
                        {
                            if (withdraw_bln == 1 || regular_bln == 1)
                            {
                                parameters[13] = new ReportParameter("signature2", "false");
                            }
                            else
                            {
                                parameters[13] = new ReportParameter("signature2", "true");
                            }
                        }
                        else
                        {
                            if (withdraw_bln == 1 || regular_bln == 1)
                            {
                                parameters[13] = new ReportParameter("signature", "false");
                            }
                            else
                            {
                                parameters[13] = new ReportParameter("signature", "true");
                            }
                        }



                        if (regular_bln == 1)
                        {
                            parameters[5] = new ReportParameter("hideReg", "false");
                            parameters[6] = new ReportParameter("Reghidden", "false");


                           // reportViewer2.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReg);

                        }
                        else
                        {
                            parameters[5] = new ReportParameter("hideReg", "true");
                            parameters[6] = new ReportParameter("Reghidden", "true");
                        }

                        if (withdraw_bln == 1)
                        {
                            parameters[2] = new ReportParameter("hidden", "false");
                         //   reportViewer2.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Subreport);
                        }
                        else
                        {
                            parameters[2] = new ReportParameter("hidden", "true");
                        }

                        parameters[1] = new ReportParameter("hidetravel", "true");
                        parameters[7] = new ReportParameter("Growth", growthRate + "%");
                        parameters[8] = new ReportParameter("hide", "true");
                        parameters[9] = new ReportParameter("hide2", "true");
                        parameters[10] = new ReportParameter("hide3", "true");
                        parameters[14] = new ReportParameter("illustID", illustID);
                        parameters[15] = new ReportParameter("Validity", str_validdate.ToString("dd-MMM-yyyy"));
                        parameters[16] = new ReportParameter("Load", strLoad);
                        reportViewer2.LocalReport.SetParameters(parameters);




                    }
                }
                catch( Exception ex)
                {

                }
             

                if (myconnection_4.State == ConnectionState.Open)
                {
                    myconnection_4.Close();
                }
                try
                {
                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "Select * from  Customer_Master where Customer_Id=" + Convert.ToInt32(custmer_id) + " ";
                    OleDbDataAdapter oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dstCustomer_dt);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();
                }
                catch
                {

                }

                string intDiceRoll;

                try
                {

                    intDiceRoll = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                }
                catch
                {
                }



                //try
                //{
                //    string str = "select Resident_Exclu from Nationality ";
                //    string Condition = "nationality_nm ='" + nationality + "'";
                //    Resident_exclu = Convert.ToInt32(ReadRow(str, Condition).Rows[0][0]);



                //    if (Resident_exclu == 0)
                //    {
                //        str = "select Resident_Exclu from Nationality ";
                //        Condition = "nationality_nm ='" + country + "'";
                //        Resident_exclu = Convert.ToInt32(ReadRow(str, Condition).Rows[0][0]);



                //    }




                //    if (joint_life == 1)
                //    {
                //        str = "select Resident_Exclu from Nationality ";
                //        Condition = "nationality_nm ='" + nationality2 + "'";
                //        Resident_exclu_life2 = Convert.ToInt32(ReadRow(str, Condition).Rows[0][0]);

                //        if (Resident_exclu_life2 == 0)
                //        {
                //            str = "select Resident_Exclu from Nationality ";
                //            Condition = "nationality_nm ='" + country2 + "'";
                //            Resident_exclu_life2 = Convert.ToInt32(ReadRow(str, Condition).Rows[0][0]);

                //        }

                //    }

                //}

                //catch
                //{

                //}
                string randomFname = "Illustration_" + ApplicationNo;

                string rpt_path = "";
                string rpt_FileName = "";
                try
                {
                    System.IO.DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Hyat/Output"));

                    if (dir.Exists)
                    {

                        rpt_path = Server.MapPath("~/Hyat/Output");
                    }

                    else
                    {

                        rpt_path = Server.MapPath("~/Hyat/Output");
                        Directory.CreateDirectory(rpt_path);
                    }
                }
                catch
                {
                }

                try
                {
                    string[] sFilenames;
                    string sDirectory = Server.MapPath("~/Hyat/doc/");

                    string sSuperCoolExtension = ".pdf";
                    sFilenames = Directory.GetFiles(sDirectory);

                    foreach (string tempstring in sFilenames)
                    {

                        if (tempstring.Contains(sSuperCoolExtension))
                        {
                            FileInfo fi = new FileInfo(tempstring);
                            if (fi.LastAccessTime < DateTime.Now.AddDays(-2))
                                fi.Delete();
                            // System.IO.File.Delete(tempstring);
                        }
                    }
                }
                catch
                {

                }

                StringBuilder sb2 = new StringBuilder();
                sb2.Append(Fn_Common.GetRandomNumber(30, 90));
                sb2.Append(Fn_Common.GetRandomNumber(12, 56));
                sb2.Append(Fn_Common.GetRandomNumber(9, 19));
                sb2.Append(Fn_Common.GetRandomNumber(7, 45));
                string rpt_path2 = Server.MapPath("~/Hyat/doc/");

                rpt_path += @"\" + sb2.ToString() + ".pdf";

                rpt_path2 += @"\" + sb2.ToString() + ".pdf";
                try
                {



                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    byte[] bytes = reportViewer2.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(rpt_path2, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                double sum_value = 0;
                double sum_value2 = 0;
                double Finacial_sum = 0;

                if (fib == 1)
                {
                    sum_value = (v_sa + ((double)fib_amt * 12 * fib_term * 0.5));
                }
                else
                {
                    sum_value = v_sa;
                }


                if (joint_life == 1)
                {
                    if (fib == 1)
                    {
                        sum_value2 = (v_sa2 + ((double)fib_amt2 * 12 * fib_term2 * 0.5));
                    }
                    else
                    {
                        sum_value2 = v_sa2;
                    }

                }

                if (sum_value >= sum_value2)
                {
                    Finacial_sum = sum_value;
                }
                else
                {
                    Finacial_sum = sum_value2;
                }

                string country_Res = "UAE";


                string random = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string path3 = Server.MapPath("~/Hyat/Merge/" + @"\" + random + ".pdf");
               
                try
                {

                    if ((Convert.ToInt16(v_age) < 21) || ((Convert.ToInt16(v_age2) < 21) && (joint_life == 1)))
                    {

                        if (country_Res != "UAE")
                        {

                            //if (Resident_exclu == 1 || Resident_exclu_life2 == 1)
                            //{
                            //    string[] inputFiles = new String[7];



                            //    inputFiles[0] = rpt_path2;
                            //    //inputFiles[1] = Convert.ToString(Session["travel_Res_path"]);
                            //    inputFiles[1] = Convert.ToString(Exclusion_path);
                            //    inputFiles[2] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                            //    inputFiles[3] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                            //    inputFiles[4] = Server.MapPath("~/Hyat/Reports/KYC.pdf");
                            //    inputFiles[5] = Server.MapPath("~/Hyat/Reports/General.pdf");
                            //    inputFiles[6] = Server.MapPath("~/Hyat/Reports/Legal.pdf");
                            //    PdfMerge.MergeFiles(path3, inputFiles);

                            //}
                            //else
                            //{
                            //    string[] inputFiles = new String[6];

                            //    inputFiles[0] = rpt_path2;
                            //    inputFiles[1] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                            //    inputFiles[2] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                            //    inputFiles[3] = Server.MapPath("~/Hyat/Reports/KYC.pdf");
                            //    inputFiles[4] = Server.MapPath("~/Hyat/Reports/General.pdf");
                            //    inputFiles[5] = Server.MapPath("~/Hyat/Reports/Legal.pdf");
                            //    PdfMerge.MergeFiles(path3, inputFiles);
                            //}

                           
                            //string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");

                            //System.IO.File.Copy(path3, filePath);


                            //rpt_path = filePath;
                        }





                        else if (((v_curr == "USD") && (sum_value > 2000000)) || ((v_curr == "AED") && (Finacial_sum > 7342000)))
                        {


                            if (Resident_exclu == 1 || Resident_exclu_life2 == 1)
                            {
                                string[] inputFiles = new String[4];



                                inputFiles[0] = rpt_path2;
                                inputFiles[1] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                               //inputFiles[2] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                                //inputFiles[3] = Convert.ToString(Session["travel_Res_path"]);
                                inputFiles[2] = Convert.ToString(Exclusion_path);
                                inputFiles[3] = Server.MapPath("~/Hyat/Reports/Legal.pdf");
                                PdfMerge.MergeFiles(path3, inputFiles);

                            }
                            else
                            {
                                string[] inputFiles = new String[3];



                                inputFiles[0] = rpt_path2;
                                inputFiles[1] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                               // inputFiles[2] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                                inputFiles[2] = Server.MapPath("~/Hyat/Reports/Legal.pdf");
                                PdfMerge.MergeFiles(path3, inputFiles);
                            }


                        
                            string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");

                            System.IO.File.Copy(path3, filePath);


                            rpt_path = filePath;
                        }

                        else
                        {

                            if (Resident_exclu == 1 || Resident_exclu_life2 == 1)
                            {
                                string[] inputFiles = new String[3];

                                inputFiles[0] = rpt_path2;
                              //  inputFiles[1] = Convert.ToString(Session["travel_Res_path"]);
                                inputFiles[1] = Convert.ToString(Exclusion_path);
                                inputFiles[2] = Server.MapPath("~/Hyat/Reports/Legal.pdf");
                                PdfMerge.MergeFiles(path3, inputFiles);

                                string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                                System.IO.File.Copy(path3, filePath);


                                rpt_path = filePath;
                            }
                            else
                            {
                                try
                                {

                                

                                string[] inputFiles = new String[2];

                                inputFiles[0] = rpt_path2;
                                inputFiles[1] = Server.MapPath("~/Hyat/Reports/Legal.pdf");
                                PdfMerge.MergeFiles(path3, inputFiles);
                                  string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                                System.IO.File.Copy(path3, filePath);


                                rpt_path = filePath;
                                }
                                catch
                                {

                                }
                            }

                        }



                    }
                    else
                    {
                        if (country_Res != "UAE")
                        {

                            //if (Resident_exclu == 1 || Resident_exclu_life2 == 1)
                            //{
                            //    string[] inputFiles = new String[6];



                            //    inputFiles[0] = rpt_path2;
                            //   // inputFiles[1] = Convert.ToString(Session["travel_Res_path"]);
                            //    inputFiles[1] = Convert.ToString(Exclusion_path);
                            //    inputFiles[2] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                            //    inputFiles[3] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                            //    inputFiles[4] = Server.MapPath("~/Hyat/Reports/KYC.pdf");
                            //    inputFiles[5] = Server.MapPath("~/Hyat/Reports/General.pdf");

                            //    PdfMerge.MergeFiles(path3, inputFiles);

                            //}
                            //else
                            //{
                            //    string[] inputFiles = new String[5];

                            //    inputFiles[0] = rpt_path2;
                            //    inputFiles[1] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                            //    inputFiles[2] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                            //    inputFiles[3] = Server.MapPath("~/Hyat/Reports/KYC.pdf");
                            //    inputFiles[4] = Server.MapPath("~/Hyat/Reports/General.pdf");

                            //    PdfMerge.MergeFiles(path3, inputFiles);
                            //}

                           
                            //string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");

                            //System.IO.File.Copy(path3, filePath);


                            //rpt_path = filePath;
                        }





                        else if (((v_curr == "USD") && (sum_value > 2000000)) || ((v_curr == "AED") && (Finacial_sum > 7342000)))
                        {


                            if (Resident_exclu == 1 || Resident_exclu_life2 == 1)
                            {
                                string[] inputFiles = new String[3];



                                inputFiles[0] = rpt_path2;
                                inputFiles[1] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                                //inputFiles[2] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");
                              //  inputFiles[3] = Convert.ToString(Session["travel_Res_path"]);
                                inputFiles[2] = Convert.ToString(Exclusion_path);
                                PdfMerge.MergeFiles(path3, inputFiles);

                            }
                            else
                            {
                                string[] inputFiles = new String[2];



                                inputFiles[0] = rpt_path2;
                                inputFiles[1] = Server.MapPath("~/Hyat/Reports/FINANCE.pdf");
                               // inputFiles[2] = Server.MapPath("~/Hyat/Reports/TRAVEL.pdf");

                                PdfMerge.MergeFiles(path3, inputFiles);
                            }


                            

                            string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");

                            System.IO.File.Copy(path3, filePath);


                            rpt_path = filePath;
                        }

                        else
                        {

                            if (Resident_exclu == 1 || Resident_exclu_life2 == 1)
                            {
                                string[] inputFiles = new String[2];

                                inputFiles[0] = rpt_path2;
                              //  inputFiles[1] = Convert.ToString(Session["travel_Res_path"]);
                                inputFiles[1] = Convert.ToString(Exclusion_path);

                                PdfMerge.MergeFiles(path3, inputFiles);
                                string filePath = Server.MapPath("~/Hyat/Output/" + @"\" + randomFname + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                                System.IO.File.Copy(path3, filePath);


                                rpt_path = filePath;
                            }
                            else
                            {
                                rpt_path = rpt_path2;
                            }

                        }


                    }


                    try
                    {

                        mycon.Open();
                        SqlQuery = "";
                        SqlQuery = "Update Illustration_Master set   Fpath='" + rpt_path + "' where Illustration_Id=" + mid + "";

                        cmd = new OleDbCommand(SqlQuery, mycon);
                        cmd.ExecuteNonQuery();
                        mycon.Close();
                        cmd.Cancel();
                    }
                    catch
                    {
                        mycon.Close();

                    }

                    Thread.Sleep(1000);


                    try
                    {

                        mycon.Open();
                        SqlQuery = "";
                        SqlQuery = "Update Application_Master set  illustration_path='" + rpt_path + "' where Application_Id=" + mid1 + "";

                        cmd = new OleDbCommand(SqlQuery, mycon);
                        cmd.ExecuteNonQuery();
                        mycon.Close();
                        cmd.Cancel();
                    }
                    catch
                    {
                        mycon.Close();

                    }

                 



                }
                catch
                {
                    Thread.Sleep(1000);
                }

            }

            catch
            {
                Thread.Sleep(1000);
            }




        }


        public void ViewReportNew(int mid, string ApplicationNo, int mid1, string Exclusion_path,int plan_holder)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["MembershipConnectionString"].ToString();
                string sqlconn = ConfigurationManager.ConnectionStrings["LIFE"].ToString();
                string conn = ConfigurationManager.ConnectionStrings["LIFE"].ToString();

        string physicalPath3 = HttpContext.Current.Request.MapPath(@"~\App_Data\dbhyat2.mdb");
        int Nation_discount = 0;
        int joint_life = 0;
        string plan_code = "";
        double v_sa = 0;
        int v_age = 0;
        int v_gender = 0;
        int v_smoker = 0;
        int v_trms = 0;
        string v_curr = "";
        string smoker1 = "";
        int V_Freq = 0;
        int growthRate = 0;
        int Woc = 0;
        int ci = 0;
        int ptd = 0;
        int hcb = 0;
        int adb = 0;
        double contri = 0;
        int atpd = 0;
        int fib = 0;
        int pw = 0;
        int pw1 = 0;
        int pw2 = 0;
        int pw3 = 0;
        int Woc_type = 0;
        double ci_amt = 0;
        double ptd_amt = 0;
        double hcb_amt = 0;
        double adb_amt = 0;
        double atpd_amt = 0;
        double fib_amt = 0;
        int fib_term = 0;
        string plan_Holder_name = "";
        DataSet dstMed = new DataSet();

        //Hyatcalc Hyat_cal = new Hyatcalc();
        int Resident_exclu = 0;
        string resident_exclu = "";
        DataSet dst_dt = new DataSet();
        DataSet dst_withdraw = new DataSet();
        DataSet dst_regular = new DataSet();
        DataSet dst_education = new DataSet();
        int regular_withdrwal_no = 0;
        string regular_frequency = "";
        int regular_startyear = 0;
        double withdrawal1 = 0;
        double withdrawal2 = 0;
        double withdrawal3 = 0;
        int pyear1 = 0;
        int pyear2 = 0;
        int pyear3 = 0;
        double Rwithdrwal = 0;
        double Rwithdrwal_Sar = 0;
        int regwNo = 0;
        int regwAmt = 0;
        int regular_bln = 0;
        int rw = 0;
        int freq = 0;
        int regYear = 0;
        string nationality = "";
        string country = "";

                string randomFname = "Illustration_" + ApplicationNo;
                string MobileNo = "";
                string dob = "";
                DataSet dstCustomer_dt = new DataSet();
                string cconn = ConfigurationManager.ConnectionStrings["LIFE"].ToString();

                string SqlQuery = "";
                OleDbConnection mycon = new OleDbConnection(sqlconn);
                OleDbCommand cmd;
                try
                {

                    mycon.Open();
                    SqlQuery = "";
                    SqlQuery = "Update Illustration_Master set lock=1 where Illustration_Id=" + mid + "";

                    cmd = new OleDbCommand(SqlQuery, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    cmd.Cancel();
                }
                catch
                {
                    mycon.Close();

                }

                AppService service1 = new AppService();
                commonFun Fn_Common = new commonFun();


                OleDbConnection myconnection_4 = new OleDbConnection(sqlconn);
                OleDbCommand prod_mCmmd_4;
                string prod_sqry_4;
                DataSet dst_valid = new DataSet();
                if (myconnection_4.State == ConnectionState.Open)
                {
                    myconnection_4.Close();
                }







                //DataSet dst_dt = new DataSet();
                DBfuction db_func = new DBfuction();

                try
                {






                    if (myconnection_4.State == ConnectionState.Open)
                    {
                        myconnection_4.Close();
                    }

                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "Select * from  Illustration_Master where Illustration_Id=" + mid + "  ";
                    OleDbDataAdapter oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dst_dt);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();





                    if (myconnection_4.State == ConnectionState.Open)
                    {
                        myconnection_4.Close();
                    }

                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "select * from  Illustration_PW where Illustration_Id=" + mid + "";
                    oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dst_withdraw);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();




                    if (myconnection_4.State == ConnectionState.Open)
                    {
                        myconnection_4.Close();
                    }

                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "select * from  Illustration_RW where Illustration_Id=" + mid + "";
                    oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dst_regular);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();

                }
                catch
                {

                }


                try
                {
                    if (dst_dt.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "myalert", "alert('  Please Process the new values before generating the Illustration  ');", true);
                        return;
                    }
                }
                catch
                {

                }





                int withdraw_bln = 0;

                try
                {
                    if (dst_withdraw.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            withdraw_bln = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["pw"]);
                            pw = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["pw"]);
                        }
                        catch { }
                        try
                        {
                            withdrawal1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount1"]);

                        }
                        catch { }
                        try
                        {
                            withdrawal2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount2"]);

                        }
                        catch { }
                        try
                        {
                            withdrawal3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount3"]);
                            pw3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount3"]);
                        }
                        catch { }

                        try
                        {
                            pyear1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year1"]);
                            pw1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year1"]);
                        }
                        catch { }
                        try
                        {
                            pyear2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year2"]);
                            pw2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year2"]);
                        }
                        catch { }
                        try
                        {
                            pyear3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year3"]);
                            pw3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year3"]);
                        }
                        catch { }

                    }

                }
                catch { }

                try
                {
                    if (dst_regular.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            regular_bln = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["rw"]);
                            rw = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["rw"]);
                        }
                        catch { }

                        try
                        {
                            regwNo = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["no_withdraw"]);
                        }
                        catch { }
                        try
                        {
                            regwAmt = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["amount"]);
                        }
                        catch { }
                        try
                        {
                            regular_frequency = Convert.ToString(dst_regular.Tables[0].Rows[0]["frequency"]);
                        }
                        catch { }

                        try
                        {
                            freq = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["frequency"]);
                        }
                        catch { }
                        try
                        {
                            regular_startyear = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["startyear"]);
                        }
                        catch { }
                        try
                        {
                            regYear = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["startyear"]);
                        }
                        catch { }
                    }
                }
                catch { }


                int custmer_id = 0;
                try
                {
                    if (dst_dt.Tables[0].Rows.Count > 0)
                    {

                        try
                        {
                            custmer_id = Convert.ToInt32(dst_dt.Tables[0].Rows[0]["Cust_Id"]);
                        }
                        catch
                        {

                        }

                        contri = Convert.ToDouble(dst_dt.Tables[0].Rows[0]["Contribution"]);




                        v_age = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Age"]);

                        growthRate = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Growth"]);

                        v_trms = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Payment_Term"]);

                        v_sa = Convert.ToDouble(dst_dt.Tables[0].Rows[0]["Sum_Cover"]);
                        v_gender = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Gender"]);
                        v_smoker = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Habit"]);
                        nationality = Convert.ToString(dst_dt.Tables[0].Rows[0]["nationality"]).Trim();
                        plan_code = Convert.ToString(dst_dt.Tables[0].Rows[0]["Plan_Code"]).Trim();
                        v_curr = Convert.ToString(dst_dt.Tables[0].Rows[0]["Currency"]).Trim();
                        V_Freq = Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Frequency"]);

                      


                    }
                }

                catch
                {
                }


                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd1 = new SqlCommand("select  * from Illustration_Rider where status=1 and life=1 and illustration_id=" + mid + ""))
                        {

                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {

                                cmd1.Connection = con;
                                sda.SelectCommand = cmd1;
                                using (DataTable Rider1_dt = new DataTable())
                                {
                                    sda.Fill(Rider1_dt);
                                    if (Rider1_dt.Rows.Count > 0)
                                    {

                                        for (int r1 = 0; r1 < Rider1_dt.Rows.Count; r1++)
                                        {

                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Permanent Total Disability")
                                            {
                                                ptd = 1;
                                                ptd_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                            }


                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Critical Illness")
                                            {
                                                ci = 1;
                                                ci_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                            }


                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Hospital Cash Benefit")
                                            {
                                                hcb = 1;
                                                hcb_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                            }


                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Family Income Benefit")
                                            {
                                                fib = 1;
                                                fib_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);
                                                fib_term = Convert.ToInt32(Rider1_dt.Rows[r1]["Term"]);
                                            }

                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Accidental Death Benefit")
                                            {
                                                adb = 1;
                                                adb_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);

                                            }


                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Accidental Total/Partial Permanent Disability")
                                            {
                                                atpd = 1;
                                                atpd_amt = Convert.ToDouble(Rider1_dt.Rows[r1]["Amount"]);

                                            }


                                            if (Convert.ToString(Rider1_dt.Rows[r1]["Name"]) == "Waiver of Contribution")
                                            {
                                                Woc = 1;


                                            }

                                        }

                                    }


                                }
                            }
                        }
                    }
                }
                catch
                {

                }

                if (contri < 50)
                {
                    Session.Abandon();
                    Response.Redirect(@"..\login.aspx");

                }


                try
                {
                    myconnection_4.Open();
                    prod_sqry_4 = "";
                    prod_sqry_4 = "Select * from  Customer_Master where Customer_Id=" + Convert.ToInt32(custmer_id) + " ";
                    OleDbDataAdapter oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                    oledbAdapter.Fill(dstCustomer_dt);
                    oledbAdapter.Dispose();
                    myconnection_4.Close();
                }
                catch
                {

                }



                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Gender");
                dt.Columns.Add("DateofBirth");
                dt.Columns.Add("Age");
                dt.Columns.Add("Smoker");
                dt.Columns.Add("CountryofResidence");
                dt.Columns.Add("Nationality");

                DataRow dr = dt.NewRow();
                DataRow dr_fund = dt.NewRow();
                dr["Name"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["F_Name"]) + " " + Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["M_Name"]) + " " + Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["L_Name"]);
                plan_Holder_name = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["F_Name"]);

                try
                {
                    dob = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Dob"]);

                }
                catch
                {

                }
                if (v_gender == 0)
                {
                    dr["Gender"] = "Male";
                }
                else
                {
                    dr["Gender"] = "Female";
                }
                dr["DateofBirth"] = DateTime.Parse(dob).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                dr["Age"] = v_age;

                if (Convert.ToInt16(dstCustomer_dt.Tables[0].Rows[0]["Habit"]) == 0)
                {
                    dr["Smoker"] = "Non-Smoker";
                }
                else
                {
                    dr["Smoker"] = "Smoker";
                }

                try
                {

                    MobileNo = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Mobile"]);
                }

                catch
                {
                }
              
                dr["CountryofResidence"] = Convert.ToString(dst_dt.Tables[0].Rows[0]["Resident"]);
                dr["Nationality"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]);
                country = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["BirthCountry"]);
                if (Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["BirthCountry"]) == Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]))
                {
                    dr["Nationality"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]);
                }
                else
                {
                    dr["Nationality"] = Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["Nationality"]) + " / " + Convert.ToString(dstCustomer_dt.Tables[0].Rows[0]["BirthCountry"]);
                }
                dt.Rows.Add(dr);

                DataTable dt2 = new DataTable();
                dt2.Clear();
                dt2.Columns.Add("Currency");
                dt2.Columns.Add("Contribution");
                dt2.Columns.Add("FrequencyofContribution");
                dt2.Columns.Add("ContributionYears");

                DataRow dr2 = dt2.NewRow();
                dr2["Currency"] = v_curr;
                dr2["Contribution"] = Convert.ToDouble(contri).ToString("#,##0");

                if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 12)
                {
                    dr2["FrequencyofContribution"] = "Monthly";
                }
                else if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 2)
                {
                    dr2["FrequencyofContribution"] = "Half Yearly";
                }
                else if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 4)
                {
                    dr2["FrequencyofContribution"] = "Quarterly";
                }
                else if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["frequency"]) == 1)
                {
                    dr2["FrequencyofContribution"] = "Yearly";
                }
                dr2["ContributionYears"] = v_trms;
                dt2.Rows.Add(dr2);

                DataTable dt3 = new DataTable();
                dt3.Clear();
                dt3.Columns.Add("BenefitsRiders");
                dt3.Columns.Add("BenefitsRidersAR");
                dt3.Columns.Add("BasisofPayment");                      // Benefit Rider details contribution
                dt3.Columns.Add("BasisofPaymentAR");
                dt3.Columns.Add("CoveredAmount");
                dt3.Columns.Add("Term");

                DataRow dr3 = dt3.NewRow();
                dr3["BenefitsRiders"] = "Family Takaful Benefit including Terminal Illness";
                dr3["BenefitsRidersAR"] = "منفعة التكافل العائلي متضمنة المرض المميت";
                dr3["BasisofPayment"] = "Inclusive";
                dr3["BasisofPaymentAR"] = "ضمنى";
                dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(v_sa).Replace(",", string.Empty))).ToString("#,##0");
                dr3["Term"] = Convert.ToString(100 - v_age);
                dt3.Rows.Add(dr3);

                if (ci == 1)
                {
                    dr3 = dt3.NewRow();
                    dr3["BenefitsRiders"] = "Critical Illness";
                    dr3["BenefitsRidersAR"] = "المرض العضال";
                    dr3["BasisofPayment"] = "Prepayment";
                    dr3["BasisofPaymentAR"] = "دفع مسبق";
                    dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ci_amt).Replace(",", string.Empty))).ToString("#,##0");
                    dr3["Term"] = Convert.ToString(100 - v_age);
                    dt3.Rows.Add(dr3);
                }
                if (Woc == 1)
                {
                    if (Convert.ToInt32(75 - Convert.ToInt32(v_age)) < Convert.ToInt32(v_trms))
                    {

                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Waiver Of Contribution";
                        dr3["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                        dr3["BasisofPayment"] = "Additional";
                        dr3["BasisofPaymentAR"] = "إضافية";
                        dr3["CoveredAmount"] = "Applicable";
                        dr3["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                        dt3.Rows.Add(dr3);

                    }
                    else
                    {

                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Waiver Of Contribution";
                        dr3["BenefitsRidersAR"] = " الإعفاء من المساهمة";
                        dr3["BasisofPayment"] = "Additional";
                        dr3["BasisofPaymentAR"] = "إضافية";
                        dr3["CoveredAmount"] = "Applicable";
                        dr3["Term"] = Convert.ToInt32(v_trms);
                        dt3.Rows.Add(dr3);
                    }
                }

                if (ptd == 1)
                {
                    if (Convert.ToInt32(ptd_amt) > 0)
                    {
                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Permanent Total Disability";
                        dr3["BenefitsRidersAR"] = " العجز الكلى الدائم";
                        dr3["BasisofPayment"] = "Prepayment";
                        dr3["BasisofPaymentAR"] = "دفع مسبق";
                        dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(ptd_amt).Replace(",", string.Empty))).ToString("#,##0");
                        dr3["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                        dt3.Rows.Add(dr3);
                    }
                }
                if (adb == 1)
                {
                    if (Convert.ToInt32(adb_amt) > 0)
                    {
                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Accidental Death Benefit";
                        dr3["BenefitsRidersAR"] = "منفعة الوفاة العرضية";
                        dr3["BasisofPayment"] = "Additional";
                        dr3["BasisofPaymentAR"] = "إضافية";
                        dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(adb_amt).Replace(",", string.Empty))).ToString("#,##0");
                        dr3["Term"] = Convert.ToString(75 - Convert.ToInt32(v_age));
                        dt3.Rows.Add(dr3);
                    }

                }
                if (fib == 1)
                {
                    if (Convert.ToInt32(fib_amt) > 0)
                    {
                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Family Income Benefit(Monthly)";
                        dr3["BenefitsRidersAR"] = "منفعة دخل العاائلة (شهرى)";
                        dr3["BasisofPayment"] = "Additional";
                        dr3["BasisofPaymentAR"] = "إضافية";
                        dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(fib_amt).Replace(",", string.Empty))).ToString("#,##0");
                        dr3["Term"] = Convert.ToString(Convert.ToInt32(fib_term));
                        dt3.Rows.Add(dr3);
                    }
                }
                if (hcb == 1)
                {
                    if (Convert.ToInt32(hcb_amt) > 0)
                    {
                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Hospital Cash Benefit(Daily)";
                        dr3["BenefitsRidersAR"] = "منفعة الإستشفاء النقدى (يومى)";
                        dr3["BasisofPayment"] = "Additional";
                        dr3["BasisofPaymentAR"] = "إضافية";
                        dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(hcb_amt).Replace(",", string.Empty))).ToString("#,##0");
                        dr3["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age));
                        dt3.Rows.Add(dr3);
                    }

                }
                if (atpd == 1)
                {
                    if (Convert.ToInt32(atpd_amt) > 0)
                    {
                        dr3 = dt3.NewRow();
                        dr3["BenefitsRiders"] = "Accidental Total or Partial Permanent Disability(Accidental Dismemberment Benefit)";
                        dr3["BenefitsRidersAR"] = "منفعة العجز الدائم الكلي أو الجزئي العرضي (منفعة فقدان الأعضاء العرضي)";
                        dr3["BasisofPayment"] = "Additional";
                        dr3["BasisofPaymentAR"] = "إضافية";
                        dr3["CoveredAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(atpd_amt).Replace(",", string.Empty))).ToString("#,##0");
                        dr3["Term"] = Convert.ToString(70 - Convert.ToInt32(v_age));
                        dt3.Rows.Add(dr3);
                    }
                }

                DataTable dt4 = new DataTable();
                dt4.Clear();
                dt4.Columns.Add("p_year");
                dt4.Columns.Add("prem_paid");
                dt4.Columns.Add("fund");
                dt4.Columns.Add("csv");
                dt4.Columns.Add("fundOne");
                dt4.Columns.Add("csvOne");

                double total_amt = 0;
                double fund_end = 0;
                double fund_end5 = 0;

                int i = 0;
                DataSet dst_fund20_5Percentage = new DataSet();
                DataSet dst_fund20 = new DataSet();
              
                try
                {


                    OleDbConnection myconnection = new OleDbConnection(cconn);
                    string prod_query = "";

                    OleDbCommand prod_mCmmd;
                    OleDbDataReader prod_dr;
                    myconnection.Open();
                    prod_query = "select rate_disc from Nationality where nationality_nm ='" + Convert.ToString(nationality) + "' ";
                    prod_mCmmd = new System.Data.OleDb.OleDbCommand(prod_query, myconnection);

                    prod_dr = prod_mCmmd.ExecuteReader();
                    prod_dr.Read();
                    Nation_discount = 0;
                    Nation_discount = Convert.ToInt16(prod_dr[0]);
                    myconnection.Close();
                    prod_mCmmd.Cancel();
                }
                catch
                {

                }



                StringBuilder sb = new StringBuilder();
                sb.Append(Fn_Common.GetRandomNumber(30, 90));
                sb.Append(Fn_Common.GetRandomNumber(12, 56));
                sb.Append(Fn_Common.GetRandomNumber(9, 19));
                sb.Append(Fn_Common.GetRandomNumber(7, 45));


                string illustID;

                illustID = "OnBoard " + "- " + Fn_Common.Illust_Version + "." + sb.ToString();


                Boolean non_resident = false;

             

                double p_load = 0;
                double pptd_load = 0;

             






                try
                {
                    try
                    {
                        dst_fund20 = service1.Idikhar_cal_contribution("SP+", 0, contri, v_curr, V_Freq, v_trms, v_age, v_gender, v_smoker, growthRate, v_sa, Nation_discount, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, 0, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, p_load, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);

                    }
                    catch
                    {
                    }
                  
                    dst_fund20_5Percentage = service1.Idikhar_cal_contribution("SP+", 0, contri, v_curr, V_Freq, v_trms, v_age, v_gender, v_smoker, 5, v_sa, Nation_discount, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, 0, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, p_load, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);
                    for (i = 0; i <= Convert.ToInt32(dst_fund20_5Percentage.Tables[0].Rows.Count); i++)
                    {
                        if (i < v_trms)
                        {
                            DataRow dr4 = dt4.NewRow();
                            dr4["p_year"] = Convert.ToInt32(dst_fund20.Tables[0].Rows[i][0]);
                            dr4["prem_paid"] = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][1]).ToString("#,##0");
                            dr4["fund"] = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][2]).ToString("#,##0");
                            fund_end = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][2]);
                            dr4["csv"] = Convert.ToDouble((double)dst_fund20.Tables[0].Rows[i][3]).ToString("#,##0");
                            dr4["fundOne"] = Convert.ToDouble((double)dst_fund20_5Percentage.Tables[0].Rows[i][2]).ToString("#,##0");
                            fund_end5 = Convert.ToDouble((double)dst_fund20_5Percentage.Tables[0].Rows[i][2]);
                            dr4["csvOne"] = Convert.ToDouble((double)dst_fund20_5Percentage.Tables[0].Rows[i][3]).ToString("#,##0");
                            dt4.Rows.Add(dr4);
                        }


                    }
                }
                catch
                {

                }

                DataTable dt5 = new DataTable();
                dt5.Clear();
                dt5.Columns.Add("MedicalReqs");

                if (Convert.ToInt16(dst_dt.Tables[0].Rows[0]["Habit"]) == 1)
                {
                    smoker1 = "Y";
                }
                else
                {
                    smoker1 = "N";
                }

                DataSet dst_grw = new DataSet();
                dst_grw.Clear();


                DataTable dt200 = new DataTable();
                DataSet dst_FundGrw = new DataSet();
                DataSet dst_FundGrw100 = new DataSet();
                dt200.Clear();
                dt200.Columns.Add("Age");
                dt200.Columns.Add("Total");
                dt200.Columns.Add("G3");
                dt200.Columns.Add("G5");
                dt200.Columns.Add("G7");

                DataTable dt210 = new DataTable();
                dt210.Clear();
                dt210.Columns.Add("Age");
                dt210.Columns.Add("Total");
                dt210.Columns.Add("G3");
                dt210.Columns.Add("G5");
                dt210.Columns.Add("G7");
                DataTable dt23 = new DataTable();
                dt23.Clear();
                dt23.Columns.Add("Age");
                dt23.Columns.Add("Total");
                dt23.Columns.Add("Fund");

                DataTable dt25 = new DataTable();
                dt25.Clear();
                dt25.Columns.Add("Age");
                dt25.Columns.Add("Total");
                dt25.Columns.Add("Fund");

                DataTable dt27 = new DataTable();
                dt27.Clear();
                dt27.Columns.Add("Age");
                dt27.Columns.Add("Total");
                dt27.Columns.Add("Fund");

                try
                {
                    dst_grw = new DataSet();

                    dst_grw = service1.Idikhar_cal_contributionGrw("SP+", 0, contri, v_curr, V_Freq, v_trms, v_age, v_gender, v_smoker, 3, v_sa, Nation_discount, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, 0, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, p_load, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);
                    if (dst_grw.Tables[0].Rows.Count > 0)
                    {
                        dt23 = dst_grw.Tables[0];
                    }
                }
                catch
                {
                }
                try
                {
                    dst_grw = new DataSet();
                    dst_grw = service1.Idikhar_cal_contributionGrw("SP+", 0, contri, v_curr, V_Freq, v_trms, v_age, v_gender, v_smoker, 5, v_sa, Nation_discount, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, 0, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, p_load, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);

                    if (dst_grw.Tables[0].Rows.Count > 0)
                    {
                        dt25 = dst_grw.Tables[0];
                    }
                }
                catch
                {
                }
                dst_grw = new DataSet();
                try
                {
                    dst_grw = new DataSet();
                    dst_grw = service1.Idikhar_cal_contributionGrw("SP+", 0, contri, v_curr, V_Freq, v_trms, v_age, v_gender, v_smoker, 7, v_sa, Nation_discount, pw, pw1, pw2, pw3, withdrawal1, withdrawal2, withdrawal3, rw, regYear, regwNo, regwAmt, freq, Woc, ci, ptd, hcb, adb, atpd, fib, 0, ci_amt, ptd_amt, hcb_amt, adb_amt, atpd_amt, fib_amt, fib_term, p_load, 0, 0, 0, 0, 0, pptd_load, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, non_resident);
                    if (dst_grw.Tables[0].Rows.Count > 0)
                    {
                        dt27 = dst_grw.Tables[0];
                    }
                }
                catch
                {

                }


                try
                {

                    int j = 0;
                    int p = 0;
                    int z = 5;
                    if (dst_grw.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < dst_grw.Tables[0].Rows.Count; i++)
                        {


                            if ((Int32)dt23.Rows[i][0] == z)
                            {
                                DataRow dr11 = dt200.NewRow();
                                dr11["Age"] = dt23.Rows[i][0];
                                dr11["Total"] = dt23.Rows[i][1];
                                dr11["G3"] = Math.Round((double)dt23.Rows[i][2]);
                                for (j = 0; j < dst_grw.Tables[0].Rows.Count; j++)
                                {
                                    if ((Int32)dt25.Rows[j][0] == z)
                                    {
                                        dr11["G5"] = Math.Round((double)dt25.Rows[j][2]);
                                    }
                                }
                                for (p = 0; p < dst_grw.Tables[0].Rows.Count; p++)
                                {
                                    if ((Int32)dt27.Rows[p][0] == z)
                                    {
                                        dr11["G7"] = Math.Round((double)dt27.Rows[p][2]);
                                    }
                                }
                                dt200.Rows.Add(dr11);
                                z = z + 5;
                            }



                        }

                        dst_FundGrw.Tables.Add(dt200);
                        Session["dst_FundGrw"] = (DataSet)dst_FundGrw;
                    }
                }
                catch
                {

                }


                try
                {

                    int j = 0;
                    int p = 0;
                    int z = 50;
                    if (dst_grw.Tables[0].Rows.Count > 0)
                    {

                        for (z = 50; z < 101; z = z + 10)
                        {
                            i = 0;
                            p = 0;
                            for (i = 0; i < dst_grw.Tables[0].Rows.Count; i++)
                            {

                                if ((Int32)dt23.Rows[i][0] == z)
                                {
                                    DataRow dr11 = dt210.NewRow();
                                    dr11["Age"] = dt23.Rows[i][0];
                                    dr11["Total"] = dt23.Rows[i][1];
                                    dr11["G3"] = Math.Round((double)dt23.Rows[i][2]);
                                    for (j = 0; j < dst_grw.Tables[0].Rows.Count; j++)
                                    {
                                        if ((Int32)dt25.Rows[j][0] == z)
                                        {
                                            dr11["G5"] = Math.Round((double)dt25.Rows[j][2]);
                                        }
                                    }
                                    for (p = 0; p < dst_grw.Tables[0].Rows.Count; p++)
                                    {
                                        if ((Int32)dt27.Rows[p][0] == z)
                                        {
                                            dr11["G7"] = Math.Round((double)dt27.Rows[p][2]);
                                        }
                                    }
                                    dt210.Rows.Add(dr11);

                                }


                            }

                        }

                        
                    }

                    dst_FundGrw100.Tables.Add(dt210);

                    Session["dst_FundGrw100"] = (DataSet)dst_FundGrw100;

                }
                catch
                {
                }










                double msum_value = 0;


                if (v_curr == "USD")
                {
                    msum_value = (v_sa + (fib_amt * 12 * fib_term * 0.5)) * 3.671;

                }
                else
                {
                    msum_value = (v_sa + (fib_amt * 12 * fib_term * 0.5));
                }


             



                dstMed = Fn_Common.Medical(v_age, msum_value, "IPR", smoker1);

                if (dstMed.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j <= dstMed.Tables[0].Rows.Count - 1; j++)
                    {
                        DataRow dr5 = dt5.NewRow();
                        dr5["MedicalReqs"] = j + 1 + ")  " + dstMed.Tables[0].Rows[j][0];
                        dt5.Rows.Add(dr5);
                    }
                    DataRow dr6 = dt5.NewRow();
                    dr6["MedicalReqs"] = "* SALAMA reserves the right to call for any additional medical if deemed necessary";
                    dt5.Rows.Add(dr6);
                }
                else
                {
                    DataRow dr5 = dt5.NewRow();
                    dr5["MedicalReqs"] = "1) No Medical Examination Required";
                    dt5.Rows.Add(dr5);

                    DataRow dr6 = dt5.NewRow();
                    dr6["MedicalReqs"] = "* SALAMA reserves the right to call for any additional medical if deemed necessary";
                    dt5.Rows.Add(dr6);
                }
               
                DateTime valid_date, curr_date, curr_date1;
                DateTime str_validdate = DateTime.Today;
                try
                {
                    valid_date = DateTime.Now.AddMonths(1);
                    curr_date = (Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age)).AddMonths(6);
                    if (valid_date >= curr_date)
                    {
                        str_validdate = ((Convert.ToDateTime(dstCustomer_dt.Tables[0].Rows[0]["Dob"]).AddYears(v_age)).AddMonths(6)).AddDays(1);
                    }
                    else
                    {
                        str_validdate = valid_date;
                    }
                }
                catch
                { }

                reportViewer1.Reset();
                string strPath;
                if (Session["disb_uid"].ToString().ToUpper() == "ENBD")
                {
                    strPath = Server.MapPath("~/Idikhar/Reports/IdikharPlusENBD.rdlc");
                }
                else
                {
                    strPath = Server.MapPath("~/Idikhar/Reports/IdikharPlus.rdlc");
                }

                reportViewer1.LocalReport.ReportPath = strPath;
                reportViewer1.ProcessingMode = ProcessingMode.Local;

                ReportDataSource datasource = new ReportDataSource("CoveredMemberDetails", dt);
                ReportDataSource datasource1 = new ReportDataSource("CovAndContrDetails", dt2);
                ReportDataSource datasource2 = new ReportDataSource("Benefits", dt3);
                ReportDataSource datasource3 = new ReportDataSource("IdikharFundGrowth", dt4);
                ReportDataSource datasource4 = new ReportDataSource("MedicalRequirements", dt5);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(datasource);
                reportViewer1.LocalReport.DataSources.Add(datasource1);
                reportViewer1.LocalReport.DataSources.Add(datasource2);
                reportViewer1.LocalReport.DataSources.Add(datasource3);
                reportViewer1.LocalReport.DataSources.Add(datasource4);

                string strLoad = "";


                reportViewer1.LocalReport.EnableExternalImages = true;
                string imagePath = new Uri(Server.MapPath("../Signature/" + mid1 + ".jpg")).AbsoluteUri;
                DataSet sign = new DataSet();
                string SignatureDateStamp = "";
                try
                {
                    OleDbConnection MyConnection = new OleDbConnection(sqlconn);
                    OleDbDataAdapter oledbAdapter;
                    if (MyConnection.State == ConnectionState.Open)
                    {
                        MyConnection.Close();
                    }

                    MyConnection.Open();
                    string Myquery = "";
                    Myquery = "Select Sign_Date from Application_Master where Application_Id=" + mid1 + "";
                    oledbAdapter = new OleDbDataAdapter(Myquery, MyConnection);
                    oledbAdapter.Fill(sign);
                    oledbAdapter.Dispose();
                    MyConnection.Close();

                    SignatureDateStamp = Convert.ToString(sign.Tables[0].Rows[0]["Sign_Date"]);

                    ReportParameter parameter = new ReportParameter("sign1", imagePath);
                    reportViewer1.LocalReport.SetParameters(parameter);

                    ReportParameter parameter12 = new ReportParameter("Sign_Date", DateTime.Parse(SignatureDateStamp).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                    reportViewer1.LocalReport.SetParameters(parameter12);
                }
                catch (Exception ex)
                {

                }

              



                if (joint_life == 1)
                {
                    imagePath = new Uri(Server.MapPath("../Signature/" + mid1 + "_1.jpg")).AbsoluteUri;

                }

                else
                {
                    imagePath = new Uri(Server.MapPath("../Signature/blank.jpg")).AbsoluteUri;
                }
                ReportParameter parameter2 = new ReportParameter("sign2", imagePath);
                reportViewer1.LocalReport.SetParameters(parameter2);

                if (plan_holder == 1)
                {
                    imagePath = new Uri(Server.MapPath("~/Signature/" + mid + "_2.jpg")).AbsoluteUri;

                }
                else
                {
                    imagePath = new Uri(Server.MapPath("../Signature/blank.jpg")).AbsoluteUri;
                }
                ReportParameter parameter3 = new ReportParameter("sign3", imagePath);
                reportViewer1.LocalReport.SetParameters(parameter3);




                ReportParameter[] parameters = new ReportParameter[25];

                if (withdraw_bln == 1)
                {
                    parameters[0] = new ReportParameter("hidden", "false");
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Subreport);
                }
                else
                {
                    parameters[0] = new ReportParameter("hidden", "true");
                }

                parameters[1] = new ReportParameter("growthValue", growthRate + "%");

                if (regular_bln == 1)
                {
                    parameters[2] = new ReportParameter("Reghidden", "false");
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReg);
                }
                else
                {
                    parameters[2] = new ReportParameter("Reghidden", "true");
                }
                
                sb.Append(db_func.GetRandomNumber(30, 90));
                sb.Append(db_func.GetRandomNumber(12, 56));
                sb.Append(db_func.GetRandomNumber(9, 19));
                sb.Append(db_func.GetRandomNumber(7, 45));
             
        


                string Query_str;
                double bonus = 0;
                double bonus_amt = 0;
                DBaccess_Idikhar obj_DBaccess_Idikar = new DBaccess_Idikhar();
                try
                {
                    Query_str = "";
                    Query_str = "select bonus from Bonus where  Currency='" + v_curr + "' and From_Amt <= " + contri + " and To_Amt >=" + contri + " and Frequency='" + V_Freq + "' ";
                    bonus = (double)obj_DBaccess_Idikar.getValuedouble(Query_str) / 100;
                }
                catch
                {
                }
                bonus_amt = contri * bonus * V_Freq * v_trms;

                parameters[3] = new ReportParameter("fundend", Convert.ToString(fund_end.ToString("#,##0")));
                parameters[4] = new ReportParameter("fundend5", Convert.ToString(fund_end5.ToString("#,##0")));
                parameters[5] = new ReportParameter("bonus_val", Convert.ToString(bonus_amt));

              
                parameters[6] = new ReportParameter("hideEdu", "true");
               
                if (Session["disb_uid"].ToString().ToUpper() == "SCB")
                {
                    parameters[7] = new ReportParameter("hidePlanCharges", "false");
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubIdikharPlanCharges);
                }
                else if (Session["disb_uid"].ToString().ToUpper() == "ENBD")
                {
                    parameters[7] = new ReportParameter("hidePlanCharges", "false");
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubIdikharPlanCharges);
                }
                else
                {
                    parameters[7] = new ReportParameter("hidePlanCharges", "true");
                }

                parameters[8] = new ReportParameter("EduChild1", "");
                parameters[9] = new ReportParameter("EduChild2", "");
                parameters[10] = new ReportParameter("EduChild3", "");
                parameters[11] = new ReportParameter("EduChild4", "");
                parameters[12] = new ReportParameter("Fundgrw", "true");
                parameters[13] = new ReportParameter("Fundgrw100", "true");

                double EduTlt1 = 0;
                double EduTlt2 = 0;
                double EduTlt3 = 0;
                double EduTlt4 = 0;
                double GrandTotalEdu = EduTlt1 + EduTlt2 + EduTlt3 + EduTlt4;

                parameters[14] = new ReportParameter("GrdTotal", GrandTotalEdu.ToString());//Session["GrandTotalEdu"].ToString());
                parameters[15] = new ReportParameter("illustID", illustID);
                parameters[16] = new ReportParameter("Validity", str_validdate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                parameters[17] = new ReportParameter("hide1", "true");
                parameters[18] = new ReportParameter("hide2", "true");
                parameters[19] = new ReportParameter("hide3", "true");
                parameters[20] = new ReportParameter("hide4", "true");
                parameters[21] = new ReportParameter("Load", strLoad);
                parameters[22] = new ReportParameter("PlanYear20", "x");
                parameters[23] = new ReportParameter("Percent", "x");
                parameters[24] = new ReportParameter("PlanYear20AR", "x");
                reportViewer1.LocalReport.SetParameters(parameters);


                try
                {
                    ReportParameter Fundgrw = new ReportParameter("Fundgrw", "false", true);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { Fundgrw });
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubFundGrw);
                    ReportParameter Fundgrw100 = new ReportParameter("Fundgrw100", "false", true);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { Fundgrw100 });
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubFundGrw100);




                }
                catch
                {

                }

                string rpt_path = "";
                string rpt_FileName = "";
                try
                {
                    System.IO.DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Idikhar/Output"));

                    if (dir.Exists)
                    {

                        rpt_path = Server.MapPath("~/Idikhar/Output");
                    }

                    else
                    {
                        rpt_path = Server.MapPath("~/Idikhar/Output");
                        Directory.CreateDirectory(rpt_path);
                    }
                }
                catch
                {
                }


                string rpt_path2 = Server.MapPath("~/Idikhar/doc/");
                rpt_path += @"\" + sb.ToString() + ".pdf";
                rpt_path2 += @"\" + sb.ToString() + ".pdf";
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = reportViewer1.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);

                using (FileStream fs = new FileStream(rpt_path2, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

              

                try
                {
                    FileInfo fileinformation;
                    string sDirectory = Server.MapPath("~/Idikhar/Merge");
                    string[] sFilenames = Directory.GetFiles(sDirectory);


                    foreach (string File1 in sFilenames)
                    {

                        fileinformation = new FileInfo(File1);

                        if (fileinformation.CreationTime <= DateTime.Now.AddDays(-1))
                        {
                            fileinformation.Delete();
                        }

                    }

                }

                catch
                {

                }

                try
                {
                    FileInfo fileinformation;
                    string sDirectory = Server.MapPath("~/Idikhar/doc");
                    string[] sFilenames = Directory.GetFiles(sDirectory);


                    foreach (string File1 in sFilenames)
                    {

                        fileinformation = new FileInfo(File1);

                        if (fileinformation.CreationTime <= DateTime.Now.AddDays(-1))
                        {
                            fileinformation.Delete();
                        }

                    }

                }

                catch
                {

                }

                try
                {
                    FileInfo fileinformation;
                    string sDirectory = Server.MapPath("~/Idikhar/Output");
                    string[] sFilenames = Directory.GetFiles(sDirectory);


                    foreach (string File1 in sFilenames)
                    {

                        fileinformation = new FileInfo(File1);

                        if (fileinformation.CreationTime <= DateTime.Now.AddDays(-1))
                        {
                            fileinformation.Delete();
                        }

                    }

                }

                catch
                {

                }
                if (Exclusion_path != "")
                {
                    Resident_exclu = 1;
                }


                string random = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string path3 = Server.MapPath("~/Idikhar/Merge/" + @"\" + random + ".pdf");
                string country_Res = "UAE";

                double sum_value = 0;
              
                double Finacial_sum = 0;

                if (fib == 1)
                {
                    sum_value = (v_sa + ((double)fib_amt * 12 * fib_term * 0.5));
                }
                else
                {
                    sum_value = v_sa;
                }
                Finacial_sum = sum_value;


                if ((v_age) < 21)
               {


                if (((v_curr == "USD") && (sum_value > 2000000)) || ((v_curr == "AED") && (Finacial_sum > 7342000)))
                {


                        if (Resident_exclu == 1)
                        {
                            string[] inputFiles = new String[4];



                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Server.MapPath("~/Idikhar/Reports/FINANCE.pdf");
                            //inputFiles[2] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");
                            inputFiles[2] = Convert.ToString(Exclusion_path);
                            inputFiles[3] = Server.MapPath("~/Idikhar/Reports/Legal.pdf");
                            PdfMerge.MergeFiles(path3, inputFiles);

                        }
                        else
                        {
                            string[] inputFiles = new String[3];



                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Server.MapPath("~/Idikhar/Reports/FINANCE.pdf");
                          //  inputFiles[2] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");
                            inputFiles[2] = Server.MapPath("~/Idikhar/Reports/Legal.pdf");
                            PdfMerge.MergeFiles(path3, inputFiles);
                        }


                        randomFname = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                        string filePath = Server.MapPath("~/Idikhar/Output/" + @"\" + randomFname + ".pdf");

                        System.IO.File.Copy(path3, filePath);


                        rpt_path = filePath;
                    }

                    else
                    {

                        if (Resident_exclu == 1)
                        {
                            string[] inputFiles = new String[3];

                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Convert.ToString(Exclusion_path);
                         inputFiles[2] = Server.MapPath("~/Idikhar/Reports/Legal.pdf");
                           // inputFiles[3] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");

                            PdfMerge.MergeFiles(path3, inputFiles);
                            string randomName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                            string filePath = Server.MapPath("~/Idikhar/Output/" + @"\" + randomName + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                            System.IO.File.Copy(path3, filePath);


                            rpt_path = filePath;
                        }
                        else
                        {
                            string[] inputFiles = new String[2];

                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Server.MapPath("~/Idikhar/Reports/Legal.pdf");
                            //inputFiles[2] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");

                            PdfMerge.MergeFiles(path3, inputFiles);
                            string randomName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                            string filePath = Server.MapPath("~/Idikhar/Output/" + @"\" + randomName + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                            System.IO.File.Copy(path3, filePath);


                            rpt_path = filePath;
                        }

                    }
                }
                else
                {



                    if (((v_curr == "USD") && (sum_value > 2000000)) || ((v_curr == "AED") && (Finacial_sum > 7342000)))
                    {


                        if (Resident_exclu == 1)
                        {
                            string[] inputFiles = new String[3];



                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Server.MapPath("~/Idikhar/Reports/FINANCE.pdf");
                            //inputFiles[2] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");
                            inputFiles[2] = Convert.ToString(Exclusion_path);
                            PdfMerge.MergeFiles(path3, inputFiles);

                        }
                        else
                        {
                            string[] inputFiles = new String[2];



                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Server.MapPath("~/Idikhar/Reports/FINANCE.pdf");
                           // inputFiles[2] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");

                            PdfMerge.MergeFiles(path3, inputFiles);
                        }

randomFname = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                        string filePath = Server.MapPath("~/Idikhar/Output/" + @"\" + randomFname + ".pdf");

                        System.IO.File.Copy(path3, filePath);


                        rpt_path = filePath;
                    }

                    else
                    {

                        if (Resident_exclu == 1)
                        {
                            string[] inputFiles = new String[2];

                            inputFiles[0] = rpt_path2;
                            inputFiles[1] = Convert.ToString(Exclusion_path); 
                           // inputFiles[2] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");

                            PdfMerge.MergeFiles(path3, inputFiles);
                            string randomName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                            string filePath = Server.MapPath("~/Idikhar/Output/" + @"\" + randomName + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                            System.IO.File.Copy(path3, filePath);


                            rpt_path = filePath;
                        }
                        else
                        {
                            if (((v_curr == "USD") && (v_sa > 1362)) || ((v_curr == "AED") && (v_sa > 5000)))
                            {

                                //      string[] inputFiles = new String[2];

                                //      inputFiles[0] = rpt_path2;
                                //inputFiles[1] = Server.MapPath("~/Idikhar/Reports/TRAVEL.pdf");
                                //      PdfMerge.MergeFiles(path3, inputFiles);
                                //      string randomName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                                //      string filePath = Server.MapPath("~/Idikhar/Output/" + @"\" + randomName + ".pdf");       // path3 + @"\" + randomName + ".pdf";

                                //      System.IO.File.Copy(path3, filePath);


                                //      rpt_path = filePath;

                                rpt_path = rpt_path2;

                            }

                            else
                            {
                                rpt_path = rpt_path2;
                            }


                        }

                    }
                }

            

                try
                {

                    mycon.Open();
                    SqlQuery = "";
                    SqlQuery = "Update Illustration_Master set   Fpath='" + rpt_path + "' where Illustration_Id=" + mid + "";

                    cmd = new OleDbCommand(SqlQuery, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    cmd.Cancel();
                }
                catch
                {
                    mycon.Close();

                }

                Thread.Sleep(1000);


                try
                {

                    mycon.Open();
                    SqlQuery = "";
                    SqlQuery = "Update Application_Master set  illustration_path='" + rpt_path + "' where Application_Id=" + mid1 + "";

                    cmd = new OleDbCommand(SqlQuery, mycon);
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                    cmd.Cancel();
                }
                catch
                {
                    mycon.Close();

                }



            }
            catch (Exception ex)
            {

            }

        }

        public DataTable ReadRow(string str, string condition)
        {

            OleDbConnection myconnection1 = new OleDbConnection();
            myconnection1 = new OleDbConnection(conn);
            OleDbCommand prod_mCmmd = new OleDbCommand();
            DataTable dt;
            myconnection1.Open();

            OleDbCommand cmd = new OleDbCommand(str + " where " + condition, myconnection1);
            try
            {
                dt = new DataTable();
                OleDbDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                cmd.Dispose();
                myconnection1.Close();
                return dt;

            }
            catch
            {
                cmd.Dispose();
                myconnection1.Close();
                throw;
            }
        }

        private void SubFundGrw(object sender, SubreportProcessingEventArgs e)
        {
            try
            {

                DataSet dst_FundGrw = new DataSet();
                dst_FundGrw = (System.Data.DataSet)Session["dst_FundGrw"];

                e.DataSources.Add(new ReportDataSource("FundGrw20", dst_FundGrw.Tables[0]));
            }
            catch
            {

            }
        }

        private void SubFundGrw100(object sender, SubreportProcessingEventArgs e)
        {
            try
            {

                DataSet dst_FundGrw100 = new DataSet();
                dst_FundGrw100 = (System.Data.DataSet)Session["dst_FundGrw100"];

                e.DataSources.Add(new ReportDataSource("FundGrw100", dst_FundGrw100.Tables[0]));
            }
            catch
            {

            }
        }

        private void SubReg(object sender, SubreportProcessingEventArgs e)
        {
            int regular_withdrwal_no = 0;
            string regular_frequency = "";
            DateTime regular_startyear = DateTime.Today;
            double Rwithdrwal = 0;
            double Rwithdrwal_Sar = 0;

            int regwNo = 0;
            int regwAmt = 0;
            int regular_bln = 0;
            int rw = 0;
            int freq = 0;
            int regYear = 0;

            DataTable dt20 = new DataTable();
            dt20.Clear();
            dt20.Columns.Add("To");
            dt20.Columns.Add("Amount");
            dt20.Columns.Add("From");
            dt20.Columns.Add("Freq");
            dt20.Columns.Add("NoWid");

            OleDbConnection myconnection_4 = new OleDbConnection(sqlconn);
            OleDbCommand prod_mCmmd_4;
            OleDbDataAdapter oledbAdapter;
            string prod_sqry_4;
            if (myconnection_4.State == ConnectionState.Open)
            {
                myconnection_4.Close();
            }

            myconnection_4.Open();
            prod_sqry_4 = "";
            prod_sqry_4 = "select * from tbl_regular  where plan_id=" + mid + "";
            oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
            DataSet dst_regular = new DataSet();
            oledbAdapter.Fill(dst_regular);
            oledbAdapter.Dispose();
            //myconnection_4.Dispose();
            myconnection_4.Close();


            try
            {
                if (dst_regular.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        regular_bln = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["rw"]);
                    }
                    catch { }

                    try
                    {
                        regwNo = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["no_withdraw"]);
                    }
                    catch { }
                    try
                    {
                        regwAmt = Convert.ToInt32(dst_regular.Tables[0].Rows[0]["amount"]);
                    }
                    catch { }
                    try
                    {
                        regular_frequency = Convert.ToString(dst_regular.Tables[0].Rows[0]["frequency"]);
                    }
                    catch { }
                    try
                    {
                        regular_startyear = Convert.ToDateTime(dst_regular.Tables[0].Rows[0]["startdate"]);
                    }
                    catch { }
                }
            }
            catch { }
            if (!string.IsNullOrWhiteSpace(regwAmt.ToString()))
            {
                DataRow dr20 = dt20.NewRow();
                dr20["Amount"] = Convert.ToDouble(Convert.ToDouble(regwAmt.ToString().Replace(",", string.Empty))).ToString("#,##0");
                dr20["From"] = regular_startyear.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                dr20["NoWid"] = regwNo;

                if (regular_frequency == "1")
                {
                    dr20["Freq"] = "Yearly";

                    dr20["To"] = (Convert.ToDateTime(regular_startyear).AddYears(Convert.ToInt32(regwNo))).ToString("dd/MMM/yyyy");
                }
                else if (regular_frequency == "12")
                {
                    dr20["Freq"] = "Monthly";
                    dr20["To"] = (Convert.ToDateTime(regular_startyear).AddMonths(Convert.ToInt32(regwNo))).ToString("dd/MMM/yyyy");
                }
                else if (regular_frequency == "4")
                {
                    dr20["Freq"] = "Quarterly";
                    dr20["To"] = (Convert.ToDateTime(regular_startyear).AddMonths(Convert.ToInt32(regwNo) * 3)).ToString("dd/MMM/yyyy");
                }
                else if (regular_frequency == "2")
                {
                    dr20["Freq"] = "Half Yearly";
                    dr20["To"] = (Convert.ToDateTime(regular_startyear).AddMonths(Convert.ToInt32(regwNo) * 6)).ToString("dd/MMM/yyyy");
                }
                dt20.Rows.Add(dr20);

            }
            e.DataSources.Add(new ReportDataSource("IdikharRegular", dt20));
        }

        void Subreport(object sender, SubreportProcessingEventArgs e)
        {

            double withdrawal1 = 0;
            double withdrawal2 = 0;
            double withdrawal3 = 0;
            int pyear1 = 0;
            int pyear2 = 0;
            int pyear3 = 0;

            string date1 = "";
            string date2 = "";
            string date3 = "";

            DataTable dt6 = new DataTable();
            dt6.Clear();
            dt6.Columns.Add("PlanYear");
            dt6.Columns.Add("Amount");
            dt6.Columns.Add("Date");

            OleDbConnection myconnection_4 = new OleDbConnection(sqlconn);
            OleDbCommand prod_mCmmd_4;
            OleDbDataAdapter oledbAdapter;
            string prod_sqry_4;
            if (myconnection_4.State == ConnectionState.Open)
            {
                myconnection_4.Close();
            }

            myconnection_4.Open();
            prod_sqry_4 = "";
            prod_sqry_4 = "select * from  tbl_withdrawal where plan_id=" + mid + "";
            oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
            DataSet dst_withdraw = new DataSet();
            oledbAdapter.Fill(dst_withdraw);
            oledbAdapter.Dispose();
            myconnection_4.Close();

            try
            {
                if (dst_withdraw.Tables[0].Rows.Count > 0)
                {

                    try
                    {
                        withdrawal1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount1"]);
                    }
                    catch { }
                    try
                    {
                        withdrawal2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount2"]);
                    }
                    catch { }
                    try
                    {
                        withdrawal3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["amount3"]);
                    }
                    catch { }

                    try
                    {
                        pyear1 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year1"]);
                    }
                    catch { }
                    try
                    {
                        pyear2 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year2"]);
                    }
                    catch { }
                    try
                    {
                        pyear3 = Convert.ToInt32(dst_withdraw.Tables[0].Rows[0]["year3"]);
                    }
                    catch { }
                    try
                    {
                        date1 = Convert.ToString(dst_withdraw.Tables[0].Rows[0]["date1"]);
                    }
                    catch { }
                    try
                    {
                        date2 = Convert.ToString(dst_withdraw.Tables[0].Rows[0]["date2"]);
                    }
                    catch { }
                    try
                    {
                        date3 = Convert.ToString(dst_withdraw.Tables[0].Rows[0]["date3"]);
                    }
                    catch { }
                }
            }
            catch { }



            if (!string.IsNullOrWhiteSpace(pyear1.ToString()))
            {
                DataRow dr6 = dt6.NewRow();
                dr6["PlanYear"] = pyear1;
                dr6["Amount"] = Convert.ToDouble(Convert.ToDouble(withdrawal1.ToString().Replace(",", string.Empty))).ToString("#,##0");
                dr6["Date"] = date1;
                dt6.Rows.Add(dr6);
            }

            if (!string.IsNullOrWhiteSpace(pyear2.ToString()))
            {
                if (pyear2 != 0)
                {
                    DataRow dr6 = dt6.NewRow();
                    dr6["PlanYear"] = pyear2;
                    dr6["Amount"] = Convert.ToDouble(Convert.ToDouble(withdrawal2.ToString().Replace(",", string.Empty))).ToString("#,##0");
                    dr6["Date"] = date2;
                    dt6.Rows.Add(dr6);
                }

            }

            if (!string.IsNullOrWhiteSpace(pyear3.ToString()))
            {
                if (pyear3 != 0)
                {
                    DataRow dr6 = dt6.NewRow();
                    dr6["PlanYear"] = pyear3;
                    dr6["Amount"] = Convert.ToDouble(Convert.ToDouble(withdrawal3.ToString().Replace(",", string.Empty))).ToString("#,##0");
                    dr6["Date"] = date3;
                    dt6.Rows.Add(dr6);
                }

            }
            e.DataSources.Add(new ReportDataSource("IdikharPartial", dt6));
        }

        public void SubIdikharPlanCharges(object sender, SubreportProcessingEventArgs e)
        {
            try
            {

                double[] a = new double[50];
                a = (double[])Session["surr"];
                DataTable dt20 = new DataTable();
                dt20.Clear();
                dt20.Columns.Add("trmOne");
                dt20.Columns.Add("trmTwo");
                dt20.Columns.Add("trmThree");
                dt20.Columns.Add("trmFour");

                DataTable dt21 = new DataTable();
                dt21.Clear();
                DataRow dr21 = dt21.NewRow();

                DataTable dt22 = new DataTable();
                dt22.Clear();
                DataRow dr22 = dt22.NewRow();

                DataTable dt23 = new DataTable();
                dt23.Clear();
                DataRow dr23 = dt23.NewRow();

                DataTable dt24 = new DataTable();
                dt24.Clear();
                DataRow dr24 = dt24.NewRow();

                if (v_trms > 20)
                {
                    for (int i = 0; i <= 20; i++)
                    {

                        dt21.Columns.Add("Year" + i);
                        dr21["Year" + i] = i;

                        dt22.Columns.Add("PlanPer" + i);
                        dr22["PlanPer" + i] = Math.Round(a[i], 3) * 100;

                    }


                    for (int j = 21; j <= v_trms; j++)
                    {

                        dt23.Columns.Add("Year" + j);
                        dr23["Year" + j] = j;

                        dt24.Columns.Add("PlanPer" + j);
                        dr24["PlanPer" + j] = Math.Round(a[j], 3) * 100;

                    }

                    dt21.Rows.Add(dr21);
                    dt22.Rows.Add(dr22);

                    dt23.Rows.Add(dr23);
                    dt24.Rows.Add(dr24);


                }
                else
                {
                    for (int i = 0; i <= v_trms; i++)
                    {

                        dt21.Columns.Add("Year" + i);
                        dr21["Year" + i] = i;

                        dt22.Columns.Add("PlanPer" + i);
                        dr22["PlanPer" + i] = Math.Round(a[i], 3) * 100;

                    }
                    dt21.Rows.Add(dr21);
                    dt22.Rows.Add(dr22);
                }


                DataRow dr20 = dt20.NewRow();
                if (v_trms > 9)
                {
                    dr20["trmOne"] = "5.0%";
                    dr20["trmTwo"] = "5.5%";
                    dr20["trmThree"] = "6.0%";
                    dr20["trmFour"] = "6.5%";
                    dt20.Rows.Add(dr20);
                    e.DataSources.Add(new ReportDataSource("IdikharPlanCharges", dt20));
                }
                else
                {
                    dr20["trmOne"] = "5.5%";
                    dr20["trmTwo"] = "6.5%";
                    dr20["trmThree"] = "6.5%";
                    dr20["trmFour"] = "6.5%";
                    dt20.Rows.Add(dr20);
                    e.DataSources.Add(new ReportDataSource("IdikharPlanCharges", dt20));
                }

                e.DataSources.Add(new ReportDataSource("dstIdikharSCBPlanYr", dt21));
                e.DataSources.Add(new ReportDataSource("dstIdikharSCBPlanPer", dt22));
                //if (IdikharVariables.v_trms > 20)
                //{
                e.DataSources.Add(new ReportDataSource("dstIdikharSCBPlanYr2", dt23));
                e.DataSources.Add(new ReportDataSource("dstIdikharSCBPlanPer2", dt24));
                //}

            }
            catch
            {

            }
        }



        private void SubEdu(object sender, SubreportProcessingEventArgs e)
        {
            try
            {


                DataTable dt20 = new DataTable();
                dt20.Clear();
                dt20.Columns.Add("Name");
                dt20.Columns.Add("EducationYear");
                dt20.Columns.Add("EAmount");
                dt20.Columns.Add("Index");
                dt20.Columns.Add("TotalAmount");

                OleDbConnection myconnection_4 = new OleDbConnection(sqlconn);
                OleDbCommand prod_mCmmd_4;
                OleDbDataAdapter oledbAdapter;
                string prod_sqry_4;
                if (myconnection_4.State == ConnectionState.Open)
                {
                    myconnection_4.Close();
                }

                myconnection_4.Open();
                prod_sqry_4 = "";
                prod_sqry_4 = "select * from tbl_education  where plan_id=" + mid + "";
                oledbAdapter = new OleDbDataAdapter(prod_sqry_4, myconnection_4);
                DataSet dst_education = new DataSet();
                oledbAdapter.Fill(dst_education);
                oledbAdapter.Dispose();
                //myconnection_4.Dispose();
                myconnection_4.Close();

                int i = 0;
                int rcount = dst_education.Tables[0].Rows.Count;
                int cyear = System.DateTime.Now.Year;

                double EduWAmt1, EduWAmt2, EduWAmt3, EduWAmt4;
                int syear1, syear2, syear3, syear4;
                int Eyear1, Eyear2, Eyear3, Eyear4;
                double index1, index2, index3, index4;
                int Syearchild1, Syearchild2, Syearchild3, Syearchild4;
                int Eyearchild1, Eyearchild2, Eyearchild3, Eyearchild4;

                double GTotal = 0;
                EduWAmt1 = 0;
                EduWAmt2 = 0;
                EduWAmt3 = 0;
                EduWAmt4 = 0;
                syear1 = 0;
                syear2 = 0;
                syear3 = 0;
                syear4 = 0;
                Eyear1 = 0;
                Eyear2 = 0;
                Eyear3 = 0;
                Eyear4 = 0;
                Syearchild1 = 0;
                Syearchild2 = 0;
                Syearchild3 = 0;
                Syearchild4 = 0;
                index1 = 0;
                index2 = 0;
                index3 = 0;
                index4 = 0;
                Eyearchild1 = 0;
                Eyearchild2 = 0;
                Eyearchild3 = 0;
                Eyearchild4 = 0;
                Syearchild1 = 0;
                Syearchild2 = 0;
                Syearchild3 = 0;
                Syearchild4 = 0;
                double EduTlt1 = (double)Session["eduTotal1"];
                double EduTlt2 = (double)Session["eduTotal2"];
                double EduTlt3 = (double)Session["eduTotal3"];
                double EduTlt4 = (double)Session["eduTotal4"];
                DataSet dstedu = new DataSet();
                dstedu = (System.Data.DataSet)Session["edudst"];


                if (rcount >= 1)
                {

                    for (i = 0; i < rcount; i++)
                    {
                        if (i == 0)
                        {
                            //DataRow dr21 = dt21.NewRow();

                            EduWAmt1 = Convert.ToDouble(dst_education.Tables[0].Rows[0]["amount"]) * (Math.Pow((1 + Convert.ToDouble(dst_education.Tables[0].Rows[0]["indexation"]) / 100), (Convert.ToDouble(dst_education.Tables[0].Rows[0]["sdate"]) - cyear)));
                            syear1 = Convert.ToInt32(dst_education.Tables[0].Rows[0]["sdate"]) - cyear + 1;
                            Eyear1 = Convert.ToInt32(dst_education.Tables[0].Rows[0]["edate"]) - cyear + 1;
                            index1 = (1 + Convert.ToDouble(dst_education.Tables[0].Rows[0]["indexation"]) / 100);
                            Syearchild1 = Convert.ToInt32(dst_education.Tables[0].Rows[0]["sdate"]);
                            Eyearchild1 = Convert.ToInt32(dst_education.Tables[0].Rows[0]["edate"]);

                            DataRow dr20 = dt20.NewRow();
                            dr20["Name"] = dst_education.Tables[0].Rows[0]["name"];
                            dr20["EducationYear"] = Syearchild1 + " - " + Eyearchild1;
                            dr20["EAmount"] = Math.Max((Eyearchild1 - Syearchild1 + 1) * Convert.ToDouble(dst_education.Tables[0].Rows[0]["amount"]), 0).ToString("#,##0");
                            dr20["Index"] = Convert.ToDouble(dst_education.Tables[0].Rows[0]["indexation"]) + " %";
                            dr20["TotalAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(EduTlt1).Replace(",", string.Empty))).ToString("#,##0");
                            dt20.Rows.Add(dr20);
                            try
                            {

                                GTotal = Convert.ToDouble(Convert.ToString(EduTlt1).Replace(",", string.Empty));

                            }
                            catch
                            {

                            }
                        }
                        else if (i == 1)
                        {

                            EduWAmt2 = Convert.ToDouble(dst_education.Tables[0].Rows[1]["amount"]) * (Math.Pow((1 + Convert.ToDouble(dst_education.Tables[0].Rows[1]["indexation"]) / 100), (Convert.ToDouble(dst_education.Tables[0].Rows[1]["sdate"]) - cyear)));
                            syear2 = Convert.ToInt32(dst_education.Tables[0].Rows[1]["sdate"]) - cyear + 1;
                            Eyear2 = Convert.ToInt32(dst_education.Tables[0].Rows[1]["edate"]) - cyear + 1;
                            index2 = (1 + Convert.ToDouble(dst_education.Tables[0].Rows[1]["indexation"]) / 100);
                            Syearchild2 = Convert.ToInt32(dst_education.Tables[0].Rows[1]["sdate"]);
                            Eyearchild2 = Convert.ToInt32(dst_education.Tables[0].Rows[1]["edate"]);
                            DataRow dr20 = dt20.NewRow();
                            dr20["Name"] = dst_education.Tables[0].Rows[1]["name"];
                            dr20["EducationYear"] = Syearchild2 + " - " + Eyearchild2;
                            dr20["EAmount"] = Math.Max((Eyearchild2 - Syearchild2 + 1) * Convert.ToDouble(dst_education.Tables[0].Rows[1]["amount"]), 0).ToString("#,##0");
                            dr20["Index"] = Convert.ToDouble(dst_education.Tables[0].Rows[1]["indexation"]) + " %";
                            dr20["TotalAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(EduTlt2).Replace(",", string.Empty))).ToString("#,##0");
                            dt20.Rows.Add(dr20);
                            try
                            {
                                GTotal = GTotal + Convert.ToDouble(Convert.ToString(EduTlt2).Replace(",", string.Empty));
                                //ReportParameter child2 = new ReportParameter("child2", Convert.ToString(Grid[0, 1].Value), true);
                                //rep.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { child2 });
                            }
                            catch
                            {

                            }
                        }

                        else if (i == 2)
                        {

                            EduWAmt3 = Convert.ToDouble(dst_education.Tables[0].Rows[2]["amount"]) * (Math.Pow((1 + Convert.ToDouble(dst_education.Tables[0].Rows[2]["indexation"]) / 100), (Convert.ToDouble(dst_education.Tables[0].Rows[2]["sdate"]) - cyear)));
                            syear3 = Convert.ToInt32(dst_education.Tables[0].Rows[2]["sdate"]) - cyear + 1;
                            Eyear3 = Convert.ToInt32(dst_education.Tables[0].Rows[2]["edate"]) - cyear + 1;
                            index3 = (1 + Convert.ToDouble(dst_education.Tables[0].Rows[2]["indexation"]) / 100);
                            Syearchild3 = Convert.ToInt32(dst_education.Tables[0].Rows[2]["sdate"]);
                            Eyearchild3 = Convert.ToInt32(dst_education.Tables[0].Rows[2]["edate"]);

                            DataRow dr20 = dt20.NewRow();
                            dr20["Name"] = dst_education.Tables[0].Rows[2]["name"];
                            dr20["EducationYear"] = Syearchild3 + " - " + Eyearchild3;
                            dr20["EAmount"] = Math.Max((Eyearchild3 - Syearchild3 + 1) * Convert.ToDouble(dst_education.Tables[0].Rows[2]["amount"]), 0).ToString("#,##0");
                            dr20["Index"] = Convert.ToDouble(dst_education.Tables[0].Rows[2]["indexation"]) + " %";
                            dr20["TotalAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(EduTlt3).Replace(",", string.Empty))).ToString("#,##0");
                            dt20.Rows.Add(dr20);
                            try
                            {
                                GTotal = GTotal + Convert.ToDouble(Convert.ToString(EduTlt3).Replace(",", string.Empty));


                            }
                            catch
                            {

                            }
                        }

                        else if (i == 3)
                        {

                            EduWAmt4 = Convert.ToDouble(dst_education.Tables[0].Rows[3]["amount"]) * (Math.Pow((1 + Convert.ToDouble(dst_education.Tables[0].Rows[3]["indexation"]) / 100), (Convert.ToDouble(dst_education.Tables[0].Rows[3]["sdate"]) - cyear)));
                            syear4 = Convert.ToInt32(dst_education.Tables[0].Rows[3]["sdate"]) - cyear + 1;
                            Eyear4 = Convert.ToInt32(dst_education.Tables[0].Rows[3]["edate"]) - cyear + 1;
                            index4 = (1 + Convert.ToDouble(dst_education.Tables[0].Rows[3]["indexation"]) / 100);
                            Syearchild4 = Convert.ToInt32(dst_education.Tables[0].Rows[3]["sdate"]);
                            Eyearchild4 = Convert.ToInt32(dst_education.Tables[0].Rows[3]["edate"]);

                            DataRow dr20 = dt20.NewRow();
                            dr20["Name"] = dst_education.Tables[0].Rows[3]["name"];
                            dr20["EducationYear"] = Syearchild4 + " - " + Eyearchild4;
                            dr20["EAmount"] = Math.Max((Eyearchild4 - Syearchild4 + 1) * Convert.ToDouble(dst_education.Tables[0].Rows[3]["amount"]), 0).ToString("#,##0");
                            dr20["Index"] = Convert.ToDouble(dst_education.Tables[0].Rows[3]["indexation"]) + " %";
                            dr20["TotalAmount"] = Convert.ToDouble(Convert.ToDouble(Convert.ToString(EduTlt4).Replace(",", string.Empty))).ToString("#,##0");
                            dt20.Rows.Add(dr20);
                            try
                            {
                                GTotal = GTotal + Convert.ToDouble(Convert.ToString(EduTlt4).Replace(",", string.Empty));


                            }
                            catch
                            {

                            }
                        }
                        Session["GrandTotalEdu"] = "";
                        Session["GrandTotalEdu"] = GTotal;



                    }

                }

                e.DataSources.Add(new ReportDataSource("IdikharEdu1", dt20));
                e.DataSources.Add(new ReportDataSource("IdikharEduDetails", dstedu.Tables[0]));

            }
            catch
            {

            }

        }




    }
}