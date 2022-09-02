using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using MANART_BAL;
using MANART_DAL;
using System.Drawing;
using Microsoft.AspNet.Web.Optimization.WebForms;
using System.Web.Optimization;

namespace MANART
{
    public partial class Header : System.Web.UI.MasterPage
    {
        private int UserID;
        private int UserTypeID;

        #region Page Pre-Init: force uplevel browser setting
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //if (BrowserCompatibility.IsUplevel)
            //{
            Page.ClientTarget = "uplevel";
            //}
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            Response.Expires = -1500;
            Response.CacheControl = "no-cache";

            DataSet DSSiteMaintainance = new DataSet();
            clsPageUnderMaintainance objPageUnderMaintainance = null;
            if (Session["UserID"] != null)
            {
                objPageUnderMaintainance = new clsPageUnderMaintainance();
                DSSiteMaintainance = objPageUnderMaintainance.GetSiteMaintainance();
                if (DSSiteMaintainance != null)
                    if (DSSiteMaintainance.Tables[0].Rows.Count > 0)
                    {
                        //Display Header   
                        string[] sDMsgTime, sStartTime, sDMsgTime1, sStartTime1;
                        sDMsgTime = Func.Convert.sConvertToString(DSSiteMaintainance.Tables[0].Rows[0]["StartTime"]).Split(':');
                        sDMsgTime1 = (sDMsgTime[0]).Split('/');
                        sStartTime = Func.Convert.sConvertToString(DSSiteMaintainance.Tables[0].Rows[0]["EndTime"]).Split(':');
                        sStartTime1 = (sStartTime[0]).Split('/');

                        DateTime dtDMsgTime = new DateTime(Func.Convert.iConvertToInt(Func.Convert.sConvertToString(sDMsgTime1[2]).Substring(0, 4)), Func.Convert.iConvertToInt(sDMsgTime1[1]), Func.Convert.iConvertToInt(sDMsgTime1[0]));
                        dtDMsgTime = dtDMsgTime.AddHours(Func.Convert.iConvertToInt(sDMsgTime[0].Substring(sDMsgTime[0].Length - 2, 2)));
                        dtDMsgTime = dtDMsgTime.AddMinutes(Func.Convert.iConvertToInt(sDMsgTime[1]));

                        DateTime dtStartTime = new DateTime(Func.Convert.iConvertToInt(Func.Convert.sConvertToString(sStartTime1[2]).Substring(0, 4)), Func.Convert.iConvertToInt(sStartTime1[1]), Func.Convert.iConvertToInt(sStartTime1[0]));
                        dtStartTime = dtStartTime.AddHours(Func.Convert.iConvertToInt(sStartTime[0].Substring(sStartTime[0].Length - 2, 2)));
                        dtStartTime = dtStartTime.AddMinutes(Func.Convert.iConvertToInt(sStartTime[1]));

                        lblSiteMt.Text = "This site will be under maintanance from " + dtDMsgTime.ToString("dd/MM/yyyy hh:mm:ss tt") + " (IST) to " + dtStartTime.ToString("dd/MM/yyyy hh:mm:ss tt") + " (IST), Sorry for inconvenience!";

                    }
                    else if (DSSiteMaintainance.Tables[1].Rows.Count > 0)
                    {
                        clsUser objUser = new clsUser();
                        //int iID = Func.Convert.iConvertToInt(Session["UserLoginTrackID"]);
                        //objUser.SaveUserLoginHistory(ref iID, 0, "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "");
                        DataTable dt = new DataTable();
                        if (Application["UserTrack"] != null)
                            dt = (DataTable)Application["UserTrack"];
                        if (dt != null)
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (Func.Convert.sConvertToString(dt.Rows[i]["LoginName"]) == Func.Convert.sConvertToString(Session["LoginName"]) && Func.Convert.sConvertToString(dt.Rows[i]["SessionID"]) == Session.SessionID.ToString())
                                    {
                                        dt.Rows.RemoveAt(i);
                                        dt.AcceptChanges();
                                        Application.Lock();
                                        Application["Active"] = Convert.ToInt32(Application["Active"]) - 1;
                                        //this adds 1 to the number of active users when a new user hits 
                                        Application.UnLock();
                                    }
                                }
                                Application["UserTrack"] = dt;
                            }
                        ClearSessionVlaue();
                        Response.Redirect("~/frmUnderMaintainance.aspx", false);
                    }
            }
            if (Session["UserID"] == null)
            {
                //lblSessiontimeout.Text = Func.Convert.sConvertToString(Session.Timeout);
                //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")); 
                //if (Session.Timeout > 20)
                //{
                clsUser objUser = new clsUser();
                int iID = Func.Convert.iConvertToInt(Session["UserLoginTrackID"]);
                objUser.SaveUserLoginHistory(ref iID, 0, "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "");
                DataTable dt = new DataTable();
                if (Application["UserTrack"] != null)
                    dt = (DataTable)Application["UserTrack"];
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Func.Convert.sConvertToString(dt.Rows[i]["SessionID"]) == Session.SessionID.ToString())
                            {
                                dt.Rows.RemoveAt(i);
                                dt.AcceptChanges();
                                Application.Lock();
                                Application["Active"] = Convert.ToInt32(Application["Active"]) - 1;
                                //this adds 1 to the number of active users when a new user hits 
                                Application.UnLock();

                            }
                        }
                        Application["UserTrack"] = dt;
                    }
                //Response.Redirect("~/frmLogin.aspx");               
                Response.Redirect("~/frmException.aspx?sType=" + "Session");
                //}
                //Response.Redirect("~/frmException.aspx?sType=" + "Session");
                //Response.Cookies.Clear();           
            }
            else
            {
                UserID = Func.Convert.iConvertToInt(Session["UserID"]);//sMenuType = "DE";
                UserTypeID = Func.Convert.iConvertToInt(Session["UserType"]);

                if (UserID != 0)
                {

                    //if (UserTypeID == 1 || UserTypeID == 4)
                    //    tdLMS.Style.Add("display","none");
                    //else
                    //    tdLMS.Style.Add("display", "");

                    // Changed by  VIkram
                    lblPasswordExp.Text = Func.Convert.sConvertToString(Session["Expiredays"]);
                    if (Func.Convert.iConvertToInt(lblPasswordExp.Text) > 0 && Func.Convert.iConvertToInt(lblPasswordExp.Text) < 8)
                        achPassword.Visible = true;
                    else
                        achPassword.Visible = false;

                    //12042017 Change Dealer City to User LoginName
                    //lblUserName.Text = "Welcome " + Session["FirstName"].ToString() + " " + Session["LastName"].ToString() + "!" + " (" + Session["DealerCity"].ToString() + ")";
                    lblUserName.Text = "Welcome " + Func.Convert.sConvertToString(Session["FirstName"].ToString()) + " " + Func.Convert.sConvertToString(Session["LastName"].ToString()) + "!" + " (" + Func.Convert.sConvertToString(Session["LoginName"].ToString()) + ")";
                    Session["lblUserName"] = lblUserName.Text;
                    clsContent objContent = new clsContent();
                    DataSet ds = new DataSet();

                    if (UserTypeID == 1)
                    {
                        ds = objContent.GetDocumentBulletin(0, UserTypeID, "E", "Latest");
                    }
                    else if (UserTypeID == 2)
                    {
                        ds = objContent.GetDocumentBulletin(0, UserTypeID, "D", "Latest");
                    }
                    else if (UserTypeID == 3)
                    {
                        // VIkram on 14.11.2016
                        ds = objContent.GetDocumentBulletin(Func.Convert.iConvertToInt(Session["iDealerID"]), UserTypeID, "D", "Latest");
                        //ds = objContent.GetDocumentBulletin_New(Func.Convert.iConvertToInt(Session["iDealerID"]), UserTypeID, 1, "Latest");
                    }
                    else if (UserTypeID == 4)
                    {
                        ds = objContent.GetDocumentBulletin(Func.Convert.iConvertToInt(Session["iDealerID"]), UserTypeID, "E", "Latest");
                    }
                    if (lblSiteMt.Text == "")
                    {
                        sp1.Visible = false;
                        lblSiteMt.Visible = false;
                    }
                    if (lblSiteMt.Text == "")
                    {
                        if (ds.Tables.Count != 0)
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Literal Lstr = new Literal();
                                String str = "";
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    HtmlAnchor achLink = new HtmlAnchor();
                                    if (ds.Tables[0].Rows[i]["File_Name"].ToString() != "")
                                    {
                                        achLink.HRef = "~/DownLoadFiles/Bulletin/" + ds.Tables[0].Rows[i]["File_Name"].ToString();
                                        achLink.Target = "_blank";
                                    }
                                    else
                                    {
                                        achLink.HRef = "#";
                                        achLink.Target = "_self";
                                    }
                                    achLink.InnerHtml = "  " + (i + 1).ToString() + ". " + ds.Tables[0].Rows[i]["Doc_Name"].ToString();
                                    achLink.Style.Add("color", "Red");
                                    achLink.Style.Add("text-decoration", "none");
                                    achLink.Style.Add("font-weight", "bold");
                                    achLink.Style.Add("padding-right", "15px");
                                    plhTag.Controls.Add(achLink);
                                }

                            }
                    }
                    else
                    {

                        //Label lblMsg = new Label();
                        //lblMsg.Text = lblSiteMt.Text;
                        //lblMsg.Style.Add("color", "Red");
                        //lblMsg.Style.Add("font-weight", "bold");
                        //plhTag.Controls.Add(lblMsg);
                        //sp1.Visible = true;
                        lblSiteMt.Visible = true;
                        lblSiteMt.Style.Add("width", "150%");
                    }

                }
                if (!IsPostBack)
                {
                    object a = null;
                    PopulateMenu(UserID);
                    if (Session["sMenuValue"] != null && Session["sMenuText"] != null)
                    {
                        //clsMenu objectclsGetData = new clsMenu();
                        //objectclsGetData.treePopup(Session["sMenuValue"].ToString(), Session["sMenuText"].ToString(), TreeView1);


                    }
                    //if (Func.Convert.iConvertToInt(Session["UserType"]) == 3 || Func.Convert.iConvertToInt(Session["UserType"]) == 4 || Func.Convert.iConvertToInt(Session["UserType"]) == 6 || Func.Convert.sConvertToString(Session["Is_AdminCreatedPwd"]) == "Y")
                    //{
                    //    if (Menu1.Items[0].Text == "Pending Summary")
                    //        Menu1.Items.RemoveAt(0);
                    //}
                    //if ((Func.Convert.sConvertToString(Session["UserType"]).Trim() == "5" && Func.Convert.sConvertToString(Session["UserRole"]).Trim() == "8") && Func.Convert.sConvertToString(Session["Is_AdminCreatedPwd"]) == "N")
                    //{
                    //    Menu1.Items[0].Text = "Active User";
                    //    Menu1.Items[0].NavigateUrl = "~/Forms/Common/frmUserLoginList.aspx";

                    //}
                }
            }
        }
        private void PopulateMenu(int UserID)
        {
            clsMenu objectclsGetData = new clsMenu();
            string sMenuName = "";
            string sMenuValue = "";
            string sMenuURl = "";
            string sMenuToolTip = "";
            string sParentID = "0";
            string sMenuID = "";

            DataSet ds = objectclsGetData.GetMenuData(UserID);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sParentID = Func.Convert.sConvertToString(ds.Tables[0].Rows[i]["Parent"]);
                sMenuName = Func.Convert.sConvertToString(ds.Tables[0].Rows[i]["Title"]);
                sMenuValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[i]["MenuSequenceID"]);
                sMenuURl = Func.Convert.sConvertToString(ds.Tables[0].Rows[i]["Url"]);
                sMenuToolTip = Func.Convert.sConvertToString(ds.Tables[0].Rows[i]["Description"]);

                sMenuID = Func.Convert.sConvertToString(ds.Tables[0].Rows[i]["ID"]);
                //sMenuValue = sMenuID;
                if (!sMenuURl.Contains("http://"))
                    sMenuURl = sMenuURl + "?MenuID=" + sMenuID;

                if (sParentID == "0")
                {
                    sMenuURl = "";
                }

                MenuItem Childmenu = new MenuItem(sMenuName, sMenuValue, null, sMenuURl, "");

                if (sMenuURl.Contains("http://"))
                    Childmenu.Target = "_blank";

                Childmenu.ToolTip = sMenuToolTip;
                if (Func.Convert.sConvertToString(Session["Is_AdminCreatedPwd"]) == "N")
                {
                    if (sParentID == "0")
                    {
                        Menu1.Items.Add(Childmenu);
                    }
                    else if (Menu1.FindItem(sParentID) != null)
                    {
                        Menu1.FindItem(sParentID).ChildItems.Add(Childmenu);
                    }
                }
                else if (Func.Convert.sConvertToString(Session["Is_AdminCreatedPwd"]) == "Y") //&& (sMenuName == "Admin" || sMenuName == "Change Password")
                {

                    //if (sParentID == "0")
                    //{
                    //    Menu1.Items.Add(Childmenu);
                    //}
                    //else if (Menu1.FindItem(sParentID) != null)
                    //{
                    //    Menu1.FindItem(sParentID).ChildItems.Add(Childmenu);
                    //}
                }

            }
        }

        protected void lnkLogOut_Click(object sender, ImageClickEventArgs e)
        {
            clsUser objUser = new clsUser();
            int iID = Func.Convert.iConvertToInt(Session["UserLoginTrackID"]);
            objUser.SaveUserLoginHistory(ref iID, 0, "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "");
            DataTable dt = new DataTable();
            if (Application["UserTrack"] != null)
                dt = (DataTable)Application["UserTrack"];
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Func.Convert.sConvertToString(dt.Rows[i]["LoginName"]) == Func.Convert.sConvertToString(Session["LoginName"]) && Func.Convert.sConvertToString(dt.Rows[i]["SessionID"]) == Session.SessionID.ToString())
                        {
                            dt.Rows.RemoveAt(i);
                            dt.AcceptChanges();
                            Application.Lock();
                            Application["Active"] = Convert.ToInt32(Application["Active"]) - 1;
                            //this adds 1 to the number of active users when a new user hits 
                            Application.UnLock();
                        }
                    }
                    Application["UserTrack"] = dt;
                }
            ClearSessionVlaue();
            Response.Redirect("~/frmLogin.aspx", true);
        }

        // To Clear all User Session 
        private void ClearSessionVlaue()
        {
            //Page.Response.Cache.SetCacheability(HttpCacheability.NoCache); 
            Session["sMenuValue"] = null;
            Session["sMenuText"] = null;
            Session["UserType"] = null;
            Session["UserID"] = null;
            Session.Abandon();
            FormsAuthentication.SignOut();

        }
    }
}