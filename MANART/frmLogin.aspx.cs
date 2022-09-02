using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace MANART
{
    public partial class frmLogin : System.Web.UI.Page
    {/// 
        /// Summary description for converter
        /// 
        string sClientIP = "";
        // string Is_AdminCreatedPwd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //System.Web.HttpContext context = System.Web.HttpContext.Current;
                string sClientIP = "";
                sClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (sClientIP == "" || sClientIP == null)
                    sClientIP = Request.ServerVariables["REMOTE_ADDR"];
                if (!IsPostBack)
                {
                    //Response.Redirect("frmUnderMaintainance.aspx", false);
                    DataSet DSSiteMaintainance = new DataSet();
                    clsPageUnderMaintainance objPageUnderMaintainance = new clsPageUnderMaintainance();
                    DSSiteMaintainance = objPageUnderMaintainance.GetSiteMaintainance();
                    if (DSSiteMaintainance != null && DSSiteMaintainance.Tables.Count > 0)
                        if (DSSiteMaintainance.Tables[1].Rows.Count > 0)
                        {
                            Response.Redirect("frmUnderMaintainance.aspx", false);
                        }

                    if (Request.QueryString["MSG"] != null)
                        if (Request.QueryString["MSG"] == "Success")
                            clsAlert.Show("Password Changed Successfully.Please Login with new Password");

                    clsContent objContent = new clsContent();
                    DataSet ds = new DataSet();
                    ds = objContent.GetDocumentBulletin(0, 1, "C", "Latest");

                    //HtmlGenericControl li = new HtmlGenericControl("li");
                    //HtmlGenericControl div = new HtmlGenericControl("div");
                    //HtmlGenericControl span = new HtmlGenericControl("span");
                    //HtmlGenericControl span1 = new HtmlGenericControl("span");
                    //HtmlAnchor a = new HtmlAnchor();
                    //a.HRef = "PDF/AB Lubes Maharashtra.pdf";
                    //a.Target = "_blank";
                    //span.InnerHtml = "Navdurga Motors - New Dealership at Sambalpur";
                    //span1.Style.Add("class", "more");
                    //span.Style.Add("class", "cat");
                    //span1.Controls.Add(a);
                    //div.Controls.Add(span);
                    //div.Controls.Add(span1);
                    //div.Style.Add("class", "info");
                    //li.Controls.Add(div);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string sPath = "";
                            if (ds.Tables[0].Rows[i]["File_Name"].ToString().Trim() != "")
                                sPath = "href='PDF/" + ds.Tables[0].Rows[i]["File_Name"].ToString().Trim() + "' target='_blank'";
                            else
                                sPath = "href='#'";

                            Literal str = new Literal();
                            str.Text = "<li><div class='info'><span style ='font-family :Arial ;font-size :9px;color:#f5f5f5'>" + ds.Tables[0].Rows[i]["Doc_Heading"] + "</span><span class='more'><a " + sPath + " ><div style ='text-align :right;font-family :Arial ;font-size :9px;' >Read more...</div></a></span></div><div class='clear'></div></li>";
                            //PlaceHolder1.Controls.Add(str);
                        }
                        //lblFirst.Style.Add("display", "");
                        //achFirst.Style.Add("display", "");
                        //liShow.Style.Add("display", "");
                        //lblFirst.Text = ds.Tables[0].Rows[0]["Document_Heading"].ToString();
                        //achFirst.HRef = "PDF/" + ds.Tables[0].Rows[0]["File_Name"].ToString().Trim();
                    }
                    else
                    {
                        //lblFirst.Style.Add("display", "none");
                        //achFirst.Style.Add("display", "none");
                        //liShow.Style.Add("display", "none");
                    }
                    //string sSql = "Select * from M_Sys_Ticker";
                    //ds = Func.DB.ExecuteQueryAndGetDataset(sSql);
                    //disp(ds);
                }

                //lblError.Text = "";
                ExpirePageCache();
                ClearCacheItems();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //New Code for remove data from Cache Dated 31/07/2017 Vikram K
        public void ClearCacheItems()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();

            while (enumerator.MoveNext())
                keys.Add(enumerator.Key.ToString());

            for (int i = 0; i < keys.Count; i++)
                Cache.Remove(keys[i]);
        } 
        //END
        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetNoStore();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser objectUser = new clsUser();
            DataSet dsUserDetails = new DataSet();
            int ipCount = 0;

            int flage = objectUser.Authenticate(txtLoginName.Text, txtPassword.Text, Func.Convert.iConvertToInt(Session["Count"]));
            if (flage == 0)
            {
                //lblError.Text = "User Not Exixst!";

                // CHanged by Vikram
                //txtLoginName.Text = "";
                //txtLoginName.Focus();
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "UserNotExist();", true);
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please check the User ID, Password you have entered. Both User ID and Password are case sensitive.');</script>");
            }
            else if (flage == 2)
            {
                if (Session["LoginName"] != null && Session["LoginName"].ToString() != txtLoginName.Text)
                {
                    Session["Count"] = "0";
                    Session["LoginName"] = txtLoginName.Text;
                }
                if (Session["LoginName"] != null && Session["LoginName"].ToString() == txtLoginName.Text && Session["Count"] != null)
                    ipCount = Func.Convert.iConvertToInt(Session["Count"]);
                ipCount = ipCount + 1;
                Session["Count"] = ipCount.ToString();
                //lblError.Text = "Please Enter The Correct Password!";
                Session["LoginName"] = txtLoginName.Text;
                txtPassword.Focus();

                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ValidateUser();", true);
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Please Enter The Correct Password!');</script>");
            }
            else if (flage == 1)
            {
                dsUserDetails = objectUser.GetUserDetails("Authentication", txtLoginName.Text);
                if (dsUserDetails.Tables[0] != null && dsUserDetails.Tables[0].Rows.Count != 0)
                {
                    Session["UserID"] = Func.Convert.iConvertToInt(dsUserDetails.Tables[0].Rows[0]["UserId"]);
                    Session["UserType"] = Func.Convert.iConvertToInt(dsUserDetails.Tables[0].Rows[0]["UserType"]);
                    Session["UserClaimSlabID"] = Func.Convert.iConvertToInt(dsUserDetails.Tables[0].Rows[0]["SlabID"]);
                    Session["UserRole"] = Func.Convert.iConvertToInt(dsUserDetails.Tables[0].Rows[0]["UserRole"]);
                    Session["FirstName"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["FirstName"]);
                    Session["LastName"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["LastName"]);
                    //Session["Email"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Email"]);
                    //Session["Password"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Password"]);

                    //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);
                    //Session["Password"]= objCrypto.Decrypting(Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Password"]), "ETS");

                    Session["Password"] = clsCrypto.Decrypt(Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Password"]));

                    Session["Expiredays"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Expiredays"]);
                    Session["DealerName"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["DealerName"]);
                    Session["Is_AdminCreatedPwd"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Is_AdminCreatedPwd"]);
                    Session["HOBR_ID"] = Func.Convert.iConvertToInt(dsUserDetails.Tables[0].Rows[0]["HOBR_ID"]);
                    Session["DepartmentID"] = Func.Convert.iConvertToInt(dsUserDetails.Tables[0].Rows[0]["DepartmentID"]);
                    Session["DealerCity"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["DealerCity"]);
                    Session["DlrEOYDone"] = "Y";
                    Session["ISGST"] = Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["ISGST"]);
                    //Sujata Remove this becuase it is applicable for head login bydefault
                    //Session["Is_SpPermission"] = BLL.Func.Convert.sConvertToString(dsUserDetails.Tables[0].Rows[0]["Is_SpPermission"]);
                }


                //drpDealerName.Enabled = false;
                if (Func.Convert.iConvertToInt(Session["UserType"]) == 3 || Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                {
                    DataSet DSDealer = objectUser.DealerDetails(Session["UserID"].ToString(), 0, Func.Convert.iConvertToInt(Session["UserType"]));
                    if (DSDealer.Tables[0].Rows.Count > 0)
                    {
                        Session["iDealerID"] = DSDealer.Tables[0].Rows[0]["DealerID"];
                        Session["sDealerCode"] = DSDealer.Tables[0].Rows[0]["Dealer_Spares_Code"];
                        Session["DlrEOYDone"] = DSDealer.Tables[0].Rows[0]["EOYDone"];
                        Session["DlrCurrMaxYear"] = DSDealer.Tables[0].Rows[0]["MaxYear"];
                        Session["ISGST"] = DSDealer.Tables[0].Rows[0]["ISGST"];
                    }

                }
                Session["UserPermissions"] = objectUser.GetUserRights(Session["UserID"].ToString());
                //Session["UserRole"] = 
                //Response.Redirect("~/Forms/Warranty/frmWarrantyClaim.aspx?MenuID=224");                

                //Response.Redirect("~/Forms/Master/frmDynamicMaster.aspx?MenuID=159");
                //Response.Redirect("~/Forms/Admin/frmHelpDesk.aspx");
                //Response.Redirect("~/Forms/Warranty/frmWarrantySelection.aspx?MenuID=112");             

                //Response.Redirect("~/Forms/Indent/frmDealerIndent.aspx");                                                 
                //Response.Redirect("~/Forms/Indent/frmIndentProcessing.aspx?IndID=14");            
                //Response.Redirect("~/Forms/Indent/frmDACreation.aspx");      
                //Response.Redirect("~/Forms/Indent/frmDepoWiseIndSelection.aspx");                    
                //Response.Redirect("~/Forms/Warranty/frmWarrantyClaimProcessing.aspx?ClaimID=23&RequestOrClaim=C");
                //Response.Redirect("~/Forms/Warranty/frmJobDetails.aspx?RequestOrClaim=R&ClaimID=7&JobID=1");
                //Response.Redirect("~/Forms/Export/frmPreshipmentDocPrinting.aspx");
                //Response.Redirect("~/Forms/Export/frmSparesDeliveryChallan.aspx");
                //Response.Redirect("~/Forms/Export/frmDAProcessing.aspx");

                //Response.Redirect("~/Forms/Master/frmCompetitorDataTonnageWise.aspx");
                Session["Count"] = "0";

                //Response.Redirect("~/Forms/Admin/frmDtlsContentMgmt.aspx");
                //Response.Redirect("~/Forms/Admin/frmHdrContentMgmt.aspx");
                string sUserDealer = "";
                string sUserName = "";
                string sUserType = "";
                if (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 2)
                {
                    sUserDealer = "MTI";
                    sUserName = Func.Convert.sConvertToString(Session["FirstName"]) + " " + Func.Convert.sConvertToString(Session["LastName"]);
                }
                else if (Func.Convert.iConvertToInt(Session["UserType"]) == 3 || Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                {
                    sUserDealer = "Dealer";
                    sUserName = Func.Convert.sConvertToString(Session["DealerName"]);
                }

                if (Func.Convert.iConvertToInt(Session["UserType"]) == 1 || Func.Convert.iConvertToInt(Session["UserType"]) == 4)
                    sUserType = "Export";
                else if (Func.Convert.iConvertToInt(Session["UserType"]) == 2 || Func.Convert.iConvertToInt(Session["UserType"]) == 3)
                    sUserType = "Domestic";

                // Uncomment Dated 04092017 Vikram _Begin
                int iID = 0;
                objectUser.SaveUserLoginHistory(ref iID, Func.Convert.iConvertToInt(Session["UserID"]), DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "", sClientIP);
                Session["UserLoginTrackID"] = iID;
                //_End

                Session["LoginName"] = txtLoginName.Text;
                DataTable dtUserLogin = new DataTable();
                if (Convert.ToInt32(Application["Active"]) == 0)
                {

                    dtUserLogin.Columns.Add("LoginName", typeof(System.String));
                    dtUserLogin.Columns.Add("LoginDateTime", typeof(System.String));
                    dtUserLogin.Columns.Add("SessionID", typeof(System.String));
                    dtUserLogin.Columns.Add("UserName", typeof(System.String));
                    dtUserLogin.Columns.Add("UserType", typeof(System.String));
                    dtUserLogin.Columns.Add("UserDealer", typeof(System.String));
                    DataRow dr = dtUserLogin.NewRow();
                    dr["LoginName"] = Session["LoginName"].ToString();
                    dr["LoginDateTime"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    dr["SessionID"] = Session.SessionID;
                    dr["UserName"] = sUserName;
                    dr["UserType"] = sUserType;
                    dr["UserDealer"] = sUserDealer;
                    dtUserLogin.Rows.Add(dr);
                    Application["UserTrack"] = dtUserLogin;
                }
                else
                {
                    if (Application["UserTrack"] != null)
                        dtUserLogin = (DataTable)Application["UserTrack"];
                    if (dtUserLogin != null)
                        if (dtUserLogin.Rows.Count > 0)
                        {
                            DataRow dr = dtUserLogin.NewRow();
                            dr["LoginName"] = Session["LoginName"].ToString();
                            dr["LoginDateTime"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                            dr["SessionID"] = Session.SessionID;
                            dr["UserName"] = sUserName;
                            dr["UserType"] = sUserType;
                            dr["UserDealer"] = sUserDealer;
                            dtUserLogin.Rows.Add(dr);
                            Application["UserTrack"] = dtUserLogin;
                        }
                }
                if (Func.Convert.sConvertToString(Session["Is_AdminCreatedPwd"]) == "Y")
                {
                    Response.Redirect("~/Forms/Admin/frmChangePassword.aspx");
                    //Server.Transfer("~/Forms/Admin/frmChangePassword.aspx");
                }
                else
                {
                    Application.Lock();
                    if (Session["UserID"] != null)
                        Application["Active"] = Convert.ToInt32(Application["Active"]) + 1;
                    //this adds 1 to the number of active users when a new user hits 
                    Application.UnLock();
                    if (Func.Convert.sConvertToString(Session["DlrEOYDone"]) == "N")
                        Response.Redirect("~/Forms/Admin/frmEOY.aspx?MenuID=724");
                    else
                        Response.Redirect("Default.aspx");
                }
                //Response.Redirect("~/Forms/Common/frmPendingSummary.aspx");
                //Response.Redirect("ForTest.aspx");
            }
            else if (flage == 3)
            {
                //lblError.Text = "This User ID is Either Locked or Blocked!<br>Please contact to System Administrator";
                //WriteBlinkingText(lblError.Text, 0);
                // Changed By VIkram
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "LockedUser();", true);
                //txtLoginName.Text = "";
                //txtLoginName.Focus();
                string sMsg = "This User ID is Locked! Please contact to dCAN Administrator!";
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMsg + "');</script>");
            }


        }
    }
}