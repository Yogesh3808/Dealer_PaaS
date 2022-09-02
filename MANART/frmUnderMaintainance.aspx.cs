using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MANART_BAL;
using MANART_DAL;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading;

namespace MANART
{
    public partial class frmUnderMaintainance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet DSSiteMaintainance = new DataSet();
            clsPageUnderMaintainance objPageUnderMaintainance = new clsPageUnderMaintainance();
            try
            {
                Timer1.Enabled = true;
                Timer1.Interval = 1000;



                //if (DSSiteMaintainance != null)
                //{
                //    if (DSSiteMaintainance.Tables[1].Rows.Count == 0)
                //    {
                //        Response.Redirect("frmLogin.aspx", false);
                //    }
                //}
                //else
                //    Response.Redirect("frmLogin.aspx", false);

                if (Page.IsPostBack == false)
                {
                    DateTime dtEndTime;
                    if (Application["EndTime"] == null)
                    {
                        DSSiteMaintainance = objPageUnderMaintainance.GetSiteMaintainance();
                        if (DSSiteMaintainance != null)
                            if (DSSiteMaintainance.Tables[1].Rows.Count > 0)
                            {
                                //Display Header   
                                string[] sStartTime, sStartTime1;

                                sStartTime = Func.Convert.sConvertToString(DSSiteMaintainance.Tables[1].Rows[0]["EndTime"]).Split(':');
                                sStartTime1 = (sStartTime[0]).Split('/');

                                dtEndTime = new DateTime(Func.Convert.iConvertToInt(Func.Convert.sConvertToString(sStartTime1[2]).Substring(0, 4)), Func.Convert.iConvertToInt(sStartTime1[1]), Func.Convert.iConvertToInt(sStartTime1[0]));
                                dtEndTime = dtEndTime.AddHours(Func.Convert.iConvertToInt(sStartTime[0].Substring(sStartTime[0].Length - 2, 2)));
                                dtEndTime = dtEndTime.AddMinutes(Func.Convert.iConvertToInt(sStartTime[1]));
                                //dtEndTime = dtEndTime.AddSeconds(59);                                         
                                //lblTime.Text = (dtEndTime.Hour - DateTime.Now.Hour).ToString().PadLeft(2, '0') + ":" + (dtEndTime.Minute - DateTime.Now.Minute).ToString().PadLeft(2, '0') + ":" + (dtEndTime.Second - (DateTime.Now.Second + (60 - DateTime.Now.Second))).ToString().PadLeft(2, '0');
                                TimeSpan objTSEndTime = new TimeSpan();
                                objTSEndTime = dtEndTime.Subtract(DateTime.Now);
                                lblTime.Text = objTSEndTime.Hours.ToString().PadLeft(2, '0') + ":" + objTSEndTime.Minutes.ToString().PadLeft(2, '0') + ":" + objTSEndTime.Seconds.ToString().PadLeft(2, '0');
                                //ViewState["EndTime"] = dtEndTime;
                                Application["EndTime"] = dtEndTime;
                            }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DSSiteMaintainance = null;
            }
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //DateTime dtEndTime = (DateTime)ViewState["EndTime"];
            DateTime dtEndTime = (DateTime)Application["EndTime"];
            //lblTime.Text = (dtEndTime.Hour - DateTime.Now.Hour).ToString().PadLeft(2, '0') + ":" + (dtEndTime.Minute - DateTime.Now.Minute).ToString().PadLeft(2, '0') + ":" + (dtEndTime.Second - DateTime.Now.Second).ToString().PadLeft(2, '0');
            TimeSpan objTSEndTime = new TimeSpan();
            objTSEndTime = dtEndTime.Subtract(DateTime.Now);
            lblTime.Text = objTSEndTime.Hours.ToString().PadLeft(2, '0') + ":" + objTSEndTime.Minutes.ToString().PadLeft(2, '0') + ":" + objTSEndTime.Seconds.ToString().PadLeft(2, '0');

            if (lblTime.Text == "00:00:00")
            {
                Thread.Sleep(1000);
                Application["EndTime"] = null;
                Response.Redirect("frmLogin.aspx", false);

            }
        }
    }
}