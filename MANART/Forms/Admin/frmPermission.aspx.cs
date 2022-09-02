using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
using System.Collections.Generic;

namespace MANART.Forms.Admin
{
    public partial class frmPermission1 : System.Web.UI.Page
    {
        private int UsreType;
        private int UserRole;
        private int UserSlab;
        private string sSelDept = "";
        private int SaveUserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UsreType = Func.Convert.iConvertToInt(Request.QueryString["UserType"]);
                UserRole = Func.Convert.iConvertToInt(Request.QueryString["UserRole"]);
                txtLoginName.Text = Request.QueryString["LoginName"];
                lblID.Text = Request.QueryString["ID"];
                SaveUserID = Func.Convert.iConvertToInt(Session["UserID"]);

                if (Session["UserID"] == null)
                {
                    btnSave.Enabled = false;
                }
                if (!IsPostBack)
                {
                    //if (Func.Convert.iConvertToInt(Session["UserRole"]) == 8 && Func.Convert.iConvertToInt(Session["UserID"]) != 1)
                    //{
                    //    drpUserType.Enabled = false;
                    //    drpLevels.Enabled = false;
                    //    drpDept.Enabled = false;
                    //}
                    //else
                    //{
                    //    drpUserType.Enabled = true;
                    //    drpLevels.Enabled = (UsreType == 3 || UsreType == 4 || UsreType == 7 || UsreType == 6) ? false : true;
                    //    drpDept.Enabled = (UsreType == 3 || UsreType == 4 || UsreType == 7 || UsreType == 6) ? false : true;
                    //}

                    drpUserType.Enabled = false;
                    drpLevels.Enabled = (UsreType == 3 || UsreType == 4 || UsreType == 7 || UsreType == 6) ? false : true;
                    drpDept.Enabled = (UsreType == 3 || UsreType == 4 || UsreType == 7 || UsreType == 6) ? false : true;

                    clsUser objectUser = new clsUser();
                    UserSlab = objectUser.getUserClaimSlab(lblID.Text);
                    FillCombo(UsreType);

                    //Func.Common.BindDataToCombo(drpUserType, clsCommon.ComboQueryType.UserType, 0);
                    //Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0);

                    Func.Common.BindDataToCombo(ddlClaimSlab, clsCommon.ComboQueryType.ClaimSlabAll, 0);

                    ddlClaimSlab.SelectedValue = (ddlClaimSlab.SelectedValue == "0") ? Func.Convert.sConvertToString(UserSlab) : ddlClaimSlab.SelectedValue;
                    drpUserType.SelectedValue = Func.Convert.sConvertToString(UsreType);
                    drpLevels.SelectedValue = Func.Convert.sConvertToString(UserRole);                    
                    hdnUserRole.Value = Func.Convert.sConvertToString(UserRole);
                    hdnUserRoleDatabase.Value = Func.Convert.sConvertToString(UserRole);
                    DataTable dtUser = new DataTable();
                    dtUser = objectUser.GetUserByID(Func.Convert.iConvertToInt(lblID.Text));

                    objectUser = null;
                    //BranchReportchnages Added by megha
                    // if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4" || drpUserType.SelectedValue == "6")
                    if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
                        Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " And ExportDomestic='E'");
                    else
                        Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " And ExportDomestic='D'");
                    //BranchReportchnages Added by megha  
                    if (dtUser.Rows.Count > 0)
                    {
                        txtFirstName.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["FirstName"]);//Session["FirstName"].ToString();          
                        txtLastName.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["LastName"]);
                        txtEmail.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["Email"]);
                        txtEmpCode.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["Emp_No"]);
                        txtEmpCode.ReadOnly = true;                        
                        txtEmail.ReadOnly = (Func.Convert.iConvertToInt(Session["UserID"]) != 1) ? true : false;
                        drpDept.SelectedValue = Func.Convert.sConvertToString(dtUser.Rows[0]["UserDept_ID"]);
                        hdnDeptdatabase.Value = Func.Convert.sConvertToString(dtUser.Rows[0]["UserDept_ID"]);
                        hdnLinked.Value = Func.Convert.sConvertToString(dtUser.Rows[0]["Linked"]);

                        if (UsreType == 2 || UsreType == 1 || UsreType == 8)
                        {
                            chkModelCategory.Style.Add("display", "");

                            hdnHD.Value = "N";
                            hdnLMD.Value = "N";
                            hdnBUS.Value = "N";
                            hdnDefault.Value = "N";

                            hdnHDDatabase.Value = "N";
                            hdnLMDDatabase.Value = "N";
                            hdnBUSDatabase.Value = "N";
                            hdnDefaultDatabase.Value = "N";

                            //lblModelCategory.Style.Add("display", "");
                            lblModelCategory.Style.Add("display", "none");
                            if (Func.Convert.iConvertToInt(drpDept.SelectedValue) == 6 || Func.Convert.iConvertToInt(drpDept.SelectedValue) == 2)
                            {
                                chkModelCategory.Items.Add(new ListItem("NA", "0"));
                            }
                            else
                            {
                                //Func.Common.FillCheckBoxList(chkModelCategory, clsCommon.ComboQueryType.ModelCategoryBasic, 0);                                                                
                                chkModelCategory.Items.Add(new ListItem("Truck", "1"));
                                chkModelCategory.Items.Add(new ListItem("Other", "3"));
                                chkModelCategory.Items.Add(new ListItem("BUS", "2"));
                            }

                            if (chkModelCategory.Items.Count > 0)
                                chkModelCategory.Items.Remove(new ListItem("--Select--", "0"));

                            if (Func.Convert.sConvertToString(dtUser.Rows[0]["UHD"]).Trim() == "Y" || Func.Convert.sConvertToString(dtUser.Rows[0]["UDefault"]).Trim() == "Y")
                            {

                                //Megha change                            
                                if (Func.Convert.sConvertToString(dtUser.Rows[0]["UHD"]).Trim() == "Y" && (Func.Convert.iConvertToInt(drpDept.SelectedValue) != 6 || Func.Convert.iConvertToInt(drpDept.SelectedValue) != 2))
                                {
                                    //chkModelCategory.Items[1].Selected = true;
                                    chkModelCategory.Items[0].Selected = true;
                                    hdnHD.Value = "Y";
                                    hdnHDDatabase.Value = "Y";
                                    ChkRoleSelection.Enabled = true;
                                    lstRoleSelection.Enabled = true;
                                }
                                else if (Func.Convert.sConvertToString(dtUser.Rows[0]["UDefault"]).Trim() == "Y" && (Func.Convert.iConvertToInt(drpDept.SelectedValue) == 6 || Func.Convert.iConvertToInt(drpDept.SelectedValue) == 2))
                                {
                                    chkModelCategory.Items[0].Selected = true;
                                    hdnDefault.Value = "Y";
                                    hdnDefaultDatabase.Value = "Y";
                                    ChkRoleSelection.Enabled = true;
                                    lstRoleSelection.Enabled = true;
                                }
                                //Megha change\
                            }
                            if (Func.Convert.sConvertToString(dtUser.Rows[0]["ULMD"]).Trim() == "Y")
                            {
                                //chkModelCategory.Items[2].Selected = true;
                                chkModelCategory.Items[1].Selected = true;
                                //Megha change
                                ChkRoleSelection1.Enabled = true;
                                lstRoleSelection1.Enabled = true;
                                hdnLMD.Value = "Y";
                                hdnLMDDatabase.Value = "Y";
                                //Megha change
                            }
                            if (Func.Convert.sConvertToString(dtUser.Rows[0]["UBUS"]).Trim() == "Y")
                            {
                                chkModelCategory.Items[2].Selected = true;
                                
                                //Megha change
                                ChkRoleSelection2.Enabled = true;
                                lstRoleSelection2.Enabled = true;
                                hdnBUS.Value = "Y";
                                hdnBUSDatabase.Value = "Y";
                            }
                            //Megha change
                            if (((UsreType == 2 || UsreType == 8) && (drpDept.SelectedValue == "5" || drpDept.SelectedValue == "7" || drpDept.SelectedValue == "6")) || (UsreType == 1 && (drpDept.SelectedValue == "1" || drpDept.SelectedValue == "2")))
                                chkModelCategory.Enabled = true;
                            else
                                chkModelCategory.Enabled = false;

                        }
                        chkModelCategory.Style.Add("display", "none");

                    }
                    Display();
                }

                FillPermissionGrid();
                ExpirePageCache();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void Display()
        {
            clsUser objectUser = new clsUser();
            FillListOfRegion();
            FillPermissionDetails(Func.Convert.iConvertToInt(lblID.Text));
            FillMainMenuTree(UsreType);
            CheckUserMenu(objectUser.GetPermissionDetails(Func.Convert.iConvertToInt(lblID.Text), "MainMenu"));
            Visiblility(UsreType);

        }

        private void FillCombo(int iUserType)
        {
            try
            {
                if (iUserType == 3 || iUserType == 4 || iUserType == 7)
                {
                    Func.Common.BindDataToCombo(drpUserType, clsCommon.ComboQueryType.UserType, 0, " And (UserType='DE' Or UserType='DD'  Or UserType='EG')");
                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='D'");
                }
                else if (iUserType == 1 || iUserType == 2 || iUserType == 8)
                {
                    Func.Common.BindDataToCombo(drpUserType, clsCommon.ComboQueryType.UserType, 0," And (UserType='VE' Or UserType='VD' Or UserType='VTI')" );
                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='E' " + ((Func.Convert.iConvertToInt(Request.QueryString["UserRole"]) == 4) ? " " : " and ID not in(4)"));
                }
                //BranchReportchnages Added by megha
                else if (iUserType == 6)
                {
                    Func.Common.BindDataToCombo(drpUserType, clsCommon.ComboQueryType.UserType, 0, " And UserType='MD'");
                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='D'");
                }
                //BranchReportchnages Added by megha
                else
                {
                    Func.Common.BindDataToCombo(drpUserType, clsCommon.ComboQueryType.UserType, 0, " And UserType='AD'");
                    Func.Common.BindDataToCombo(drpLevels, clsCommon.ComboQueryType.UserLevel, 0, " And DealerOrETB='A'");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        private void Visiblility(int iUserType)
        {
            // if (iUserType == 3 || iUserType == 4 || iUserType == 5)
            if (iUserType == 3 || iUserType == 4 || iUserType == 5 || iUserType == 6 || iUserType == 7)
                Panel1.Style.Add("display", "none");                
            else
                Panel1.Style.Add("display", "");

            if (drpUserType.SelectedValue == "5")
                drpLevels.SelectedValue = "8";
            //BranchReportchnages Added by megha
            if (drpUserType.SelectedValue == "6")
                drpLevels.SelectedValue = "19";
            //BranchReportchnages Added by megha
        }

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
        }

        //private void CheckUserMenu(DataTable DTMainMunuSelected)
        //{
        //    try
        //    {
        //        string sValuePath = ""; ;
        //        TreeNode tmpNode;
        //        string sParentId = "";
        //        for (int iRowCnt = 0; iRowCnt < DTMainMunuSelected.Rows.Count; iRowCnt++)
        //        {
        //            sParentId = Func.Convert.sConvertToString(DTMainMunuSelected.Rows[iRowCnt]["Parent"]);
        //            if (sParentId == "0")
        //            {
        //                sValuePath = Func.Convert.sConvertToString(DTMainMunuSelected.Rows[iRowCnt]["MenuSequenceID"]);
        //            }
        //            else
        //            {
        //                sValuePath = sParentId + "/" + Func.Convert.sConvertToString(DTMainMunuSelected.Rows[iRowCnt]["MenuSequenceID"]);
        //            }

        //            tmpNode = TreePermission.FindNode(sValuePath);
        //            if (tmpNode != null)
        //            {
        //                tmpNode.Checked = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}

        private void CheckUserMenu(DataSet DTMainMunuSelected)
        {
            try
            {
                string sValuePath = ""; ;
                TreeNode tmpNode;
                string sParentId = "";
                for (int iRowCnt = 0; iRowCnt < DTMainMunuSelected.Tables[0].Rows.Count; iRowCnt++)
                {
                    sParentId = Func.Convert.sConvertToString(DTMainMunuSelected.Tables[0].Rows[iRowCnt]["Parent"]);
                    if (sParentId == "0")
                    {
                        sValuePath = Func.Convert.sConvertToString(DTMainMunuSelected.Tables[0].Rows[iRowCnt]["MenuSequenceID"]);
                    }
                    else
                    {
                        sValuePath = sParentId + "/" + Func.Convert.sConvertToString(DTMainMunuSelected.Tables[0].Rows[iRowCnt]["MenuSequenceID"]);
                    }

                    tmpNode = TreePermission.FindNode(sValuePath);
                    if (tmpNode != null)
                    {
                        tmpNode.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillMainMenuTree(int UserType)
        {
            clsUser objectUser = new clsUser();
            string sUserType = "";
            sUserType = clsUser.IdentifyUserType(UserType);
            TreePermission.Nodes.Clear();

            objectUser.PopulateMainTreeMenu(sUserType, TreePermission);


        }


        protected DataTable SavePermissionsDetails()
        {

            int cntListChkRegion;
            int cntListChkCountryState;

            int iUserID = Func.Convert.iConvertToInt(lblID.Text);
            cntListChkRegion = lstRegion.Items.Count;
            cntListChkCountryState = lstCountryOrState.Items.Count;
            DataRow dr;
            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("RegionID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("CountryID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("StateID", typeof(int)));
            for (int i = 0; i < cntListChkRegion; i++)
            {

                if (lstRegion.Items[i].Selected == true)
                {
                    lblRegionID.Text = Func.Convert.iConvertToInt(lstRegion.Items[i].Value).ToString();
                    for (int j = 0; j < cntListChkCountryState; j++)
                    {
                        if (lstCountryOrState.Items[j].Selected == true)
                        {
                            dr = dtDetails.NewRow();
                            dr["UserID"] = iUserID;
                            dr["RegionID"] = Func.Convert.iConvertToInt(lblRegionID.Text);
                            if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4" || drpUserType.SelectedValue == "6")
                            {
                                dr["CountryID"] = Func.Convert.iConvertToInt(lstCountryOrState.Items[j].Value);
                                dr["StateID"] = 0;
                            }
                            else
                            {
                                dr["StateID"] = Func.Convert.iConvertToInt(lstCountryOrState.Items[j].Value);
                                dr["CountryID"] = 0;
                            }
                            dtDetails.Rows.Add(dr);
                            dtDetails.AcceptChanges();
                        }
                    }
                }
            }
            return dtDetails;
        }

        protected DataTable SaveUserDepartment()
        {
            DataRow dr;
            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("DepartmentID", typeof(int)));

            for (int j = 0; j < lstDepartments.Items.Count; j++)
            {
                if (lstDepartments.Items[j].Selected == true)
                {
                    dr = dtDetails.NewRow();
                    dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
                    dr["DepartmentID"] = Func.Convert.iConvertToInt(lstDepartments.Items[j].Value);
                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                }
            }
            return dtDetails;
        }
        protected void drpUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iUserType = Func.Convert.iConvertToInt(drpUserType.SelectedValue);
            if (iUserType == 5 || iUserType == 7)
            {
                Panel1.Style.Add("display", "none");
                drpLevels.SelectedValue = "8";
            }
            else
            {
                Panel1.Style.Add("display", "");
                FillListOfRegion();
                lstCountryOrState.Items.Clear();
            }
            TreePermission.Nodes.Clear();
            FillMainMenuTree(iUserType);
        }
        private void FillListOfRegion()
        {
            try
            {
                //BranchReportchnages Added by megha
                // if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4" || drpUserType.SelectedValue == "6")
                //  {
                if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
                {
                    Func.Common.FillCheckBoxList(lstDepartments, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='E'");
                    Func.Common.FillCheckBoxList(lstRegion, clsCommon.ComboQueryType.RegionUserTypeWise, 0, " and Domestic_Export='E'");
                    //Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " And ExportDomestic='E'");
                    lblCountryOrState.Text = "COUNTRY:";
                }
                else
                {
                    Func.Common.FillCheckBoxList(lstDepartments, clsCommon.ComboQueryType.Department, 0, " and ExportDomestic='D'");
                    Func.Common.FillCheckBoxList(lstRegion, clsCommon.ComboQueryType.RegionUserTypeWise, 0, "and Domestic_Export='D'");
                    //Func.Common.BindDataToCombo(drpDept, clsCommon.ComboQueryType.Department, 0, " And ExportDomestic='D'");
                    lblCountryOrState.Text = "STATE:";
                }
                //BranchReportchnages Added by megha 
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }




        //private void FillPermissionDetails(int UserID)
        //{
        //    try
        //    {
        //        clsUser objectUser = new clsUser();
        //        FillListOfRegion();
        //        DataTable DTPermissionDetails = objectUser.GetPermissionDetails(UserID, "Region");

        //        if (DTPermissionDetails != null)
        //        {
        //            if (DTPermissionDetails.Rows.Count > 0)
        //            {
        //                CheckSelectedListItems(lstRegion, DTPermissionDetails, "RegionID");
        //                CheckselectedCountryOrSate(UserID);
        //                CheckSelectedDepartments(UserID);
        //            }
        //        }
        //        setEnableDisableRegion();
        //        setEnableDisableState();
        //        setEnableDisableDept();
        //        FillUserRoleUserOrDealer();
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}
        private void FillPermissionDetails(int UserID)
        {
            try
            {
                clsUser objectUser = new clsUser();
                FillListOfRegion();
                DataSet DSPermissionDetails = objectUser.GetPermissionDetails(UserID, "Region");

                if (DSPermissionDetails != null)
                {
                    if (DSPermissionDetails.Tables[0].Rows.Count > 0)
                    {
                        CheckSelectedListItems(lstRegion, DSPermissionDetails.Tables[0], "RegionID");
                        CheckselectedCountryOrSate(UserID);
                        CheckSelectedDepartments(UserID);
                    }
                }
                setEnableDisableRegion();
                setEnableDisableState();
                setEnableDisableDept();
                FillUserRoleUserOrDealer();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // change by Megha
        //private void CheckselectedCountryOrSate(int UserID)
        //{
        //    try
        //    {
        //        FillCountryStateRegionWise();
        //        clsUser objectUser = new clsUser();
        //        string CountryOrState = "";
        //        DataTable DTPermissionDetails;
        //        if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
        //        {
        //            DTPermissionDetails = objectUser.GetPermissionDetails(UserID, "Country");
        //            lblCountryOrState.Text = "COUNTRY:";
        //            CountryOrState = "CountryID";
        //        }
        //        else
        //        {
        //            DTPermissionDetails = objectUser.GetPermissionDetails(UserID, "State");
        //            lblCountryOrState.Text = "STATE:";
        //            CountryOrState = "StateID";
        //        }

        //        if (DTPermissionDetails != null)
        //        {
        //            if (DTPermissionDetails.Rows.Count > 0)
        //            {
        //                CheckSelectedListItems(lstCountryOrState, DTPermissionDetails, CountryOrState);
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }


        //}
        private void CheckselectedCountryOrSate(int UserID)
        {
            try
            {
                FillCountryStateRegionWise();
                clsUser objectUser = new clsUser();
                string CountryOrState = "";
                DataSet DSPermissionDetails;
                if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
                {
                    DSPermissionDetails = objectUser.GetPermissionDetails(UserID, "Country");
                    lblCountryOrState.Text = "COUNTRY:";
                    CountryOrState = "CountryID";
                }
                else
                {
                    DSPermissionDetails = objectUser.GetPermissionDetails(UserID, "State");
                    lblCountryOrState.Text = "STATE:";
                    CountryOrState = "StateID";
                }

                if (DSPermissionDetails != null)
                {
                    if (DSPermissionDetails.Tables[0].Rows.Count > 0)
                    {
                        CheckSelectedListItems(lstCountryOrState, DSPermissionDetails.Tables[0], CountryOrState);
                    }

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        // change by Megha
        private void CheckSelectedListItems(CheckBoxList CheckList, DataTable DTPermissionDetails, string ListType)
        {
            try
            {
                if (DTPermissionDetails.Rows.Count > 0)
                {
                    int idtRowCnt = 0;
                    int ListID = 0;
                    int iChkCnt = 0;
                    sSelDept = "";
                    for (int iLCRowCnt = 0; iLCRowCnt < CheckList.Items.Count; iLCRowCnt++)
                    {
                        ListID = Func.Convert.iConvertToInt(CheckList.Items[iLCRowCnt].Value);
                        //lstRegion.Items[iLCRowCnt].Selected = false;
                        if (idtRowCnt < DTPermissionDetails.Rows.Count)
                        {
                            for (int i = 0; i < DTPermissionDetails.Rows.Count; i++)
                            {

                                if (Func.Convert.iConvertToInt(DTPermissionDetails.Rows[i][ListType]) == ListID)
                                {
                                    CheckList.Items[iLCRowCnt].Selected = true;
                                    idtRowCnt++;
                                }
                            }
                        }
                        iChkCnt = iChkCnt + 1;
                    }
                    if (iChkCnt == idtRowCnt && ListType == "RegionID")
                        ChkRegion.Checked = true;
                    else if (iChkCnt != idtRowCnt && ListType == "RegionID")
                        ChkRegion.Checked = false;
                    else if (iChkCnt == idtRowCnt && (ListType == "CountryID" || ListType == "StateID"))
                        ChkCountryOrState.Checked = true;
                    else if (iChkCnt != idtRowCnt && (ListType == "CountryID" || ListType == "StateID"))
                        ChkCountryOrState.Checked = false;
                    else if (iChkCnt == idtRowCnt && ListType == "RoleUserID")
                    {
                        if (CheckList.ID.ToString() == "lstRoleSelection")
                        {
                            ChkRoleSelection.Checked = true;
                        }
                        else if (CheckList.ID.ToString() == "lstRoleSelection1")
                        {
                            ChkRoleSelection1.Checked = true;
                        }
                        else if (CheckList.ID.ToString() == "lstRoleSelection2")
                        {
                            ChkRoleSelection2.Checked = true;
                        }
                    }
                    else if (iChkCnt != idtRowCnt && ListType == "RoleUserID")
                    {
                        if (CheckList.ID.ToString() == "lstRoleSelection")
                        {
                            ChkRoleSelection.Checked = false;
                        }
                        else if (CheckList.ID.ToString() == "lstRoleSelection1")
                        {
                            ChkRoleSelection1.Checked = false;
                        }
                        else if (CheckList.ID.ToString() == "lstRoleSelection2")
                        {
                            ChkRoleSelection2.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        //private void CheckSelectedDepartments(int UserID)
        //{
        //    try
        //    {
        //        clsUser objectUser = new clsUser();
        //        DataTable DTDepartmentDetails = objectUser.GetPermissionDetails(UserID, "UserWiseDepartment");
        //        if (DTDepartmentDetails.Rows.Count > 0)
        //        {
        //            CheckSelectedListItems(lstDepartments, DTDepartmentDetails, "DepartmentID");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }



        //}
        private void CheckSelectedDepartments(int UserID)
        {
            try
            {
                clsUser objectUser = new clsUser();
                DataSet DTDepartmentDetails = objectUser.GetPermissionDetails(UserID, "UserWiseDepartment");
                if (DTDepartmentDetails.Tables[0].Rows.Count > 0)
                {
                    CheckSelectedListItems(lstDepartments, DTDepartmentDetails.Tables[0], "DepartmentID");
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }



        }
        protected void lstRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillCountryStateRegionWise();
                CheckselectedCountryOrSate(Func.Convert.iConvertToInt(lblID.Text));
                FillUserRoleUserOrDealer();
                setEnableDisableRegion();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected void ChkRegion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FillCountryStateRegionWise();
                CheckselectedCountryOrSate(Func.Convert.iConvertToInt(lblID.Text));
                FillUserRoleUserOrDealer();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        private void FillCountryStateRegionWise()
        {
            try
            {
                string sRegionsID = "";
                hdnSelRegion.Value = "";
                int iSelctedItemCount = 0;
                for (int i = 0; i < lstRegion.Items.Count; i++)
                {
                    if (lstRegion.Items[i].Selected == true)
                    {
                        sRegionsID += lstRegion.Items[i].Value + ",";
                        hdnSelRegion.Value = hdnSelRegion.Value + lstRegion.Items[i].Value + ",";
                        iSelctedItemCount++;
                    }

                }

                if (iSelctedItemCount > 0)
                {
                    sRegionsID = sRegionsID.Substring(0, sRegionsID.Length - 1);

                    if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
                    {

                        Func.Common.FillCheckBoxList(lstCountryOrState, clsCommon.ComboQueryType.CountryForRegion, 0, "and Region_Id in (" + sRegionsID + ")");
                    }
                    else
                    {
                        Func.Common.FillCheckBoxList(lstCountryOrState, clsCommon.ComboQueryType.StateForRegion, 0, "and Region_Id in (" + sRegionsID + ")");
                    }
                }
                else
                {
                    sRegionsID = "0";
                    Func.Common.FillCheckBoxList(lstCountryOrState, clsCommon.ComboQueryType.CountryForRegion, 0, "and Region_Id in (" + sRegionsID + ")");

                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        //protected void TreePermission_SelectedNodeChanged(object sender, EventArgs e)
        //{
        //    clsUser objectUser = new clsUser();
        //    string str = "";
        //    if (TreePermission.SelectedNode.Checked == true)
        //    {
        //        TreePermission.SelectedNode.Checked = false;

        //    }
        //    else
        //    {
        //        TreePermission.SelectedNode.Checked = true;
        //    }
        //    if (TreePermission.SelectedNode.Checked == true)
        //    {
        //        if (TreePermission.SelectedNode.ChildNodes.Count == 0)
        //        {
        //            TreePermission.SelectedNode.Parent.Checked = true;
        //        }
        //        TreePermission.SelectedNode.Checked = true;

        //        str = TreePermission.SelectedNode.Value;
        //        int ID = Func.Convert.iConvertToInt(str);

        //    }

        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsUser objectUser = new clsUser();
                int UserID;
                int UserDeptID;
                UserDeptID = Func.Convert.iConvertToInt(drpDept.SelectedValue);
                UsreType = Func.Convert.iConvertToInt(drpUserType.SelectedValue);
                UserRole = Func.Convert.iConvertToInt(drpLevels.SelectedValue);
                UserID = Func.Convert.iConvertToInt(lblID.Text);
                SaveUserID = Func.Convert.iConvertToInt(Session["UserID"]);

                objectUser.UpdateUserRoleType(UserID, UsreType, UserRole, UserDeptID);

                if (UserRole == 3 && UsreType == 1)
                    UserRole = 4;

                if (objectUser.SavePermissions(SavePermissionsDetails(), SaveUserWiseMainMenu(TreePermission), SaveUserRights(), SaveUserRoleUserOrDealer(), UserID, UserRole, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtEmail.Text.Trim(), txtEmpCode.Text.Trim(), chkModelCategory, SaveUserID))//(2nd Parameter )SaveUserDepartment(),(last Parameter )Func .Convert.iConvertToInt(drpDept .SelectedValue)
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                }
                else
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                }

                //Sujata 29072015
                DataTable dtUser = new DataTable();
                dtUser = objectUser.GetUserByID(Func.Convert.iConvertToInt(lblID.Text));
                objectUser = null;
                //BranchReportchnages Added by megha  
                if (dtUser.Rows.Count > 0)
                {
                    txtFirstName.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["FirstName"]);//Session["FirstName"].ToString();          
                    txtLastName.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["LastName"]);
                    txtEmail.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["Email"]);
                    txtEmpCode.Text = Func.Convert.sConvertToString(dtUser.Rows[0]["Emp_No"]);
                    drpDept.SelectedValue = Func.Convert.sConvertToString(dtUser.Rows[0]["UserDept_ID"]);
                    hdnLinked.Value = Func.Convert.sConvertToString(dtUser.Rows[0]["Linked"]);
                    if (UserRole == 4 && UsreType == 1) UserRole = 3;
                    hdnUserRole.Value = Func.Convert.sConvertToString(UserRole);
                    hdnUserRoleDatabase.Value = Func.Convert.sConvertToString(UserRole);

                    if (UsreType == 2 || UsreType == 1 || UsreType == 8)
                    {
                        chkModelCategory.Style.Add("display", "");

                        hdnHD.Value = "N";
                        hdnLMD.Value = "N";
                        hdnBUS.Value = "N";
                        hdnDefault.Value = "N";

                        hdnHDDatabase.Value = "N";
                        hdnLMDDatabase.Value = "N";
                        hdnBUSDatabase.Value = "N";
                        hdnDefaultDatabase.Value = "N";

                        if (chkModelCategory.Items.Count > 0)
                            chkModelCategory.Items.Remove(new ListItem("--Select--", "0"));
                        if (Func.Convert.sConvertToString(dtUser.Rows[0]["UHD"]).Trim() == "Y" || Func.Convert.sConvertToString(dtUser.Rows[0]["UDefault"]).Trim() == "Y")
                        {
                            //Megha change                        
                            if (Func.Convert.sConvertToString(dtUser.Rows[0]["UHD"]).Trim() == "Y" && (Func.Convert.iConvertToInt(drpDept.SelectedValue) != 6 || Func.Convert.iConvertToInt(drpDept.SelectedValue) != 2))
                            {
                                hdnHD.Value = "Y";
                                hdnHDDatabase.Value = "Y";
                                //chkModelCategory.Items[1].Selected = true;
                                chkModelCategory.Items[0].Selected = true;
                                ChkRoleSelection.Enabled = true;
                                lstRoleSelection.Enabled = true;
                            }
                            else if (Func.Convert.sConvertToString(dtUser.Rows[0]["UDefault"]).Trim() == "Y" && (Func.Convert.iConvertToInt(drpDept.SelectedValue) == 6 || Func.Convert.iConvertToInt(drpDept.SelectedValue) == 2))
                            {
                                hdnDefault.Value = "Y";
                                hdnDefaultDatabase.Value = "Y";
                                chkModelCategory.Items[0].Selected = true;
                                ChkRoleSelection.Enabled = true;
                                lstRoleSelection.Enabled = true;
                            }
                            //Megha change\
                        }
                        if (Func.Convert.sConvertToString(dtUser.Rows[0]["ULMD"]).Trim() == "Y")
                        {
                            //chkModelCategory.Items[2].Selected = true;
                            chkModelCategory.Items[1].Selected = true;
                            //Megha change
                            ChkRoleSelection1.Enabled = true;
                            lstRoleSelection1.Enabled = true;
                            hdnLMD.Value = "Y";
                            hdnLMDDatabase.Value = "Y";
                            //Megha change
                        }
                        if (Func.Convert.sConvertToString(dtUser.Rows[0]["UBUS"]).Trim() == "Y")
                        {
                            //chkModelCategory.Items[0].Selected = true;
                            chkModelCategory.Items[2].Selected = true;
                            //Megha change
                            ChkRoleSelection2.Enabled = true;
                            lstRoleSelection2.Enabled = true;
                            hdnBUS.Value = "Y";
                            hdnBUSDatabase.Value = "Y";
                        }
                        //Megha change
                        if (((UsreType == 2 || UsreType == 8) && (drpDept.SelectedValue == "5" || drpDept.SelectedValue == "7" || drpDept.SelectedValue == "6")) || (UsreType == 1 && (drpDept.SelectedValue == "1" || drpDept.SelectedValue == "2")))
                            chkModelCategory.Enabled = true;
                        else
                            chkModelCategory.Enabled = false;

                        chkModelCategory.Style.Add("display", "none");
                    }

                }
                //Sujata 29072015
                Display();
                FillPermissionGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        protected DataTable SaveUserWiseMainMenu(TreeView Treep)
        {
            DataTable dtDetails = new DataTable();
            try
            {
                //DataTable dtDetails = new DataTable();
                dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("MenuID", typeof(int)));
                for (int i = 0; i < Treep.Nodes.Count; i++)
                {
                    ChecknodeSelected(Treep.Nodes[i], dtDetails);
                }
                //return dtDetails;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtDetails;
        }

        protected void ChecknodeSelected(TreeNode tmpNode, DataTable dtDetails)
        {
            try
            {
                DataRow dr;
                string sTmp = tmpNode.Text;
                if (tmpNode.ChildNodes.Count > 0)
                {
                    if (tmpNode.Checked == true)
                    {
                        dr = dtDetails.NewRow();
                        dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
                        dr["MenuID"] = Func.Convert.iConvertToInt(tmpNode.Target);
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                    }
                    for (int j = 0; j < tmpNode.ChildNodes.Count; j++)
                    {
                        ChecknodeSelected(tmpNode.ChildNodes[j], dtDetails);
                    }
                }
                else if (tmpNode.Checked == true)
                {
                    dr = dtDetails.NewRow();
                    dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
                    dr["MenuID"] = Func.Convert.iConvertToInt(tmpNode.Target);
                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        //protected DataTable SaveUserWiseMainMenu(TreeView Treep)
        //{
        //    DataRow dr;
        //    DataTable dtDetails = new DataTable();
        //    dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
        //    dtDetails.Columns.Add(new DataColumn("MenuID", typeof(int)));
        //    for (int i = 0; i < Treep.Nodes.Count; i++)
        //    {
        //        if (Treep.Nodes[i].Checked == true)
        //        {
        //            dr = dtDetails.NewRow();
        //            dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
        //            dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].Value);
        //            dtDetails.Rows.Add(dr);
        //            dtDetails.AcceptChanges();
        //        }

        //        for (int j = 0; j < Treep.Nodes[i].ChildNodes.Count; j++)
        //        {

        //            if (Treep.Nodes[i].ChildNodes[j].Checked == true)
        //            {
        //                dr = dtDetails.NewRow();
        //                dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
        //                dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].ChildNodes[j].Value);
        //                dtDetails.Rows.Add(dr);
        //                dtDetails.AcceptChanges();
        //            }

        //            for (int k = 0; k < Treep.Nodes[i].ChildNodes[j].ChildNodes.Count; k++)
        //            {

        //                if (Treep.Nodes[i].ChildNodes[j].ChildNodes[k].Checked == true)
        //                {
        //                    dr = dtDetails.NewRow();
        //                    dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
        //                    dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].ChildNodes[j].ChildNodes[k].Value);
        //                    dtDetails.Rows.Add(dr);
        //                    dtDetails.AcceptChanges();
        //                }

        //                for (int l = 0; l < Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count; l++)
        //                {

        //                    if (Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Checked == true)
        //                    {
        //                        dr = dtDetails.NewRow();
        //                        dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
        //                        dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Value);
        //                        dtDetails.Rows.Add(dr);
        //                        dtDetails.AcceptChanges();
        //                    }
        //                }
        //            }
        //        }


        //    }
        //    return dtDetails;
        //}


        protected DataTable GetSelecteTreeNodeFOrPermission(TreeView Treep)
        {
            DataRow dr;
            DataTable dtDetails = new DataTable();
            try
            {
                //DataRow dr;
                //DataTable dtDetails = new DataTable();

                dtDetails.Columns.Add(new DataColumn("MenuID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("MenuName", typeof(string)));
                for (int i = 0; i < Treep.Nodes.Count; i++)
                {
                    if (Treep.Nodes[i].Checked == true)
                    {
                        if (Treep.Nodes[i].ChildNodes.Count == 0)
                        {
                            dr = dtDetails.NewRow();

                            dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].Value);
                            dr["MenuName"] = Treep.Nodes[i].Text;
                            dtDetails.Rows.Add(dr);
                            dtDetails.AcceptChanges();
                        }
                    }

                    for (int j = 0; j < Treep.Nodes[i].ChildNodes.Count; j++)
                    {
                        if (Treep.Nodes[i].ChildNodes[j].Checked == true)
                        {
                            if (Treep.Nodes[i].ChildNodes[j].ChildNodes.Count == 0)
                            {
                                dr = dtDetails.NewRow();
                                dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].ChildNodes[j].Value);
                                dr["MenuName"] = Treep.Nodes[i].ChildNodes[j].Text;
                                dtDetails.Rows.Add(dr);
                                dtDetails.AcceptChanges();
                            }
                        }

                        for (int k = 0; k < Treep.Nodes[i].ChildNodes[j].ChildNodes.Count; k++)
                        {
                            if (Treep.Nodes[i].ChildNodes[j].ChildNodes[k].Checked == true)
                            {
                                if (Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count == 0)
                                {
                                    dr = dtDetails.NewRow();
                                    dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].ChildNodes[j].ChildNodes[k].Value);
                                    dr["MenuName"] = Treep.Nodes[i].ChildNodes[j].ChildNodes[k].Text;
                                    dtDetails.Rows.Add(dr);
                                    dtDetails.AcceptChanges();
                                }
                            }

                            for (int l = 0; l < Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count; l++)
                            {
                                if (Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Checked == true)
                                {
                                    if (Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count == 0)
                                    {
                                        dr = dtDetails.NewRow();
                                        dr["MenuID"] = Func.Convert.iConvertToInt(Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Value);
                                        dr["MenuName"] = Treep.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Text;
                                        dtDetails.Rows.Add(dr);
                                        dtDetails.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }


                }
                //return dtDetails;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtDetails;
        }

        //private void FillPermissionGrid()
        //{
        //    try
        //    {
        //        string ColName = "";
        //        clsUser objectUser = new clsUser();
        //        ArrayList RightListID = new ArrayList();
        //        DataTable dtCurrentTable = new DataTable();
        //        DataTable DtRights = new DataTable();
        //        bool flage = true;
        //        DtRights = objectUser.GetPermissionDetails(Func.Convert.iConvertToInt(lblID.Text), "Rights");
        //        dtCurrentTable = GetSelecteTreeNodeFOrPermission(TreePermission);
        //        for (int i = 0; i < DtRights.Rows.Count; i++)
        //        {
        //            dtCurrentTable.Columns.Add(DtRights.Rows[i]["Description"].ToString());
        //            RightListID.Add(DtRights.Rows[i]["ID"].ToString());
        //        }

        //        if (dtCurrentTable.Rows.Count > 0)
        //        {
        //            ViewState["RightdID"] = RightListID;
        //            gridPermission.DataSource = dtCurrentTable;
        //            gridPermission.DataBind();


        //            for (int k = 3; k < gridPermission.HeaderRow.Cells.Count; k++)
        //            {
        //                ColName = gridPermission.HeaderRow.Cells[k].Text;
        //                CheckBox chk = new CheckBox();
        //                chk.Text = ColName;
        //                chk.Attributes.Add("onclick", "HeaderClick(this);");
        //                gridPermission.HeaderRow.Cells[k].Controls.Add(chk);
        //            }

        //            foreach (GridViewRow objRow in gridPermission.Rows)
        //            {
        //                int RightsIsCounter = 0;
        //                for (int i = 3; i < objRow.Cells.Count; i++)
        //                {
        //                    CheckBox chkCheckBox = new CheckBox();
        //                    chkCheckBox.ID = RightListID[RightsIsCounter].ToString();
        //                    objRow.Cells[i].Controls.Add(chkCheckBox);
        //                    RightsIsCounter = RightsIsCounter + 1;

        //                }

        //                if (flage == true)
        //                {
        //                    if (objectUser.WarrantyApplicableYN(objRow.Cells[1].Text))
        //                    {
        //                        lblclmslab.Visible = true;
        //                        ddlClaimSlab.Visible = true;
        //                        flage = false;
        //                    }
        //                    else
        //                    {
        //                        lblclmslab.Visible = false;
        //                        ddlClaimSlab.Visible = false;
        //                    }
        //                }

        //            }

        //            CheckUserMenuRights();
        //        }
        //        else
        //        {
        //            dtCurrentTable = null;
        //            gridPermission.DataSource = dtCurrentTable;
        //            gridPermission.DataBind();
        //        }
        //        objectUser = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}
        private void FillPermissionGrid()
        {
            try
            {
                string ColName = "";
                clsUser objectUser = new clsUser();
                ArrayList RightListID = new ArrayList();
                DataTable dtCurrentTable = new DataTable();

                DataSet DSRights = new DataSet();
                bool flage = true;
                DSRights = objectUser.GetPermissionDetails(Func.Convert.iConvertToInt(lblID.Text), "Rights");
                dtCurrentTable = GetSelecteTreeNodeFOrPermission(TreePermission);
                for (int i = 0; i < DSRights.Tables[0].Rows.Count; i++)
                {
                    dtCurrentTable.Columns.Add(DSRights.Tables[0].Rows[i]["Description"].ToString());
                    RightListID.Add(DSRights.Tables[0].Rows[i]["ID"].ToString());
                }

                if (dtCurrentTable.Rows.Count > 0)
                {
                    ViewState["RightdID"] = RightListID;
                    gridPermission.DataSource = dtCurrentTable;
                    gridPermission.DataBind();


                    for (int k = 3; k < gridPermission.HeaderRow.Cells.Count; k++)
                    {
                        ColName = gridPermission.HeaderRow.Cells[k].Text;
                        CheckBox chk = new CheckBox();
                        //chk.Text = ColName;
                        //Label lbl = new Label();
                        //lbl.Text = ColName;

                        chk.Attributes.Add("onclick", "HeaderClick(this);");
                        gridPermission.HeaderRow.Cells[k].Controls.Add(chk);
                        //gridPermission.HeaderRow.Cells[k].Controls.Add(lbl);
                    }

                    foreach (GridViewRow objRow in gridPermission.Rows)
                    {
                        int RightsIsCounter = 0;
                        for (int i = 3; i < objRow.Cells.Count; i++)
                        {
                            CheckBox chkCheckBox = new CheckBox();
                            chkCheckBox.ID = RightListID[RightsIsCounter].ToString();
                            objRow.Cells[i].Controls.Add(chkCheckBox);
                            RightsIsCounter = RightsIsCounter + 1;

                        }

                        if (flage == true)
                        {
                            if (objectUser.WarrantyApplicableYN(objRow.Cells[1].Text))
                            {
                                lblclmslab.Visible = true;
                                ddlClaimSlab.Visible = true;
                                flage = false;
                            }
                            else
                            {
                                lblclmslab.Visible = false;
                                ddlClaimSlab.Visible = false;
                            }
                        }

                    }

                    CheckUserMenuRights();
                }
                else
                {
                    dtCurrentTable = null;
                    gridPermission.DataSource = dtCurrentTable;
                    gridPermission.DataBind();
                }
                objectUser = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            FillPermissionGrid();
        }

        private DataTable SaveUserRights()
        {

            DataRow dr;
            CheckBox chkRights = new CheckBox();
            DropDownList ddlSlabs = new DropDownList();
            int iRCount = 0;
            int icount = 3;
            clsUser objectUser = new clsUser();
            bool flage = true;
            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("MenuID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("RightsID", typeof(int)));
            dtDetails.Columns.Add(new DataColumn("ClmslabID", typeof(int)));
            ArrayList RightsList = new ArrayList();
            RightsList = (ArrayList)ViewState["RightdID"];
            iRCount = (RightsList == null) ? 0 : RightsList.Count;

            foreach (GridViewRow objRow in gridPermission.Rows)
            {
                for (int i = 0; i < iRCount; i++)
                {
                    chkRights = (CheckBox)objRow.Cells[icount].FindControl(RightsList[i].ToString());
                    if (chkRights != null)
                        if (chkRights.Checked)
                        {
                            dr = dtDetails.NewRow();
                            dr["UserID"] = Func.Convert.iConvertToInt(lblID.Text);
                            dr["MenuID"] = Func.Convert.iConvertToInt(objRow.Cells[1].Text);
                            dr["RightsID"] = Func.Convert.iConvertToInt(RightsList[i].ToString());
                            if (flage == true)
                            {
                                if (objectUser.WarrantyApplicableYN(objRow.Cells[1].Text))
                                {
                                    dr["ClmslabID"] = Func.Convert.iConvertToInt(ddlClaimSlab.SelectedValue);
                                    flage = false;
                                }
                                else
                                {
                                    dr["ClmslabID"] = 0;
                                }
                            }
                            else
                            {
                                dr["ClmslabID"] = 0;
                            }

                            dtDetails.Rows.Add(dr);
                            dtDetails.AcceptChanges();
                        }

                    icount++;
                }

                icount = 3;

            }

            FillPermissionGrid();
            objectUser = null;
            return dtDetails;
        }

        //private void CheckUserMenuRights()
        //{
        //    try
        //    {
        //        clsUser objectUser = new clsUser();
        //        int iRCount = 0;
        //        DataTable DtRights = new DataTable();
        //        DtRights = objectUser.GetPermissionDetails(Func.Convert.iConvertToInt(lblID.Text), "UserMenuRights");
        //        DropDownList ddlClaimSlabs = new DropDownList();
        //        CheckBox chkRights = new CheckBox();
        //        ArrayList RightsList = new ArrayList();
        //        RightsList = (ArrayList)ViewState["RightdID"];
        //        iRCount = RightsList.Count;
        //        int icount = 3;
        //        int menuID = 0;
        //        int RightID = 0;
        //        int imenuID = 0;
        //        int iRightID = 0;
        //        foreach (GridViewRow objRow in gridPermission.Rows)
        //        {

        //            for (int i = 0; i < iRCount; i++)
        //            {
        //                chkRights = (CheckBox)objRow.Cells[icount].FindControl(RightsList[i].ToString());
        //                menuID = Func.Convert.iConvertToInt(objRow.Cells[1].Text);
        //                RightID = Func.Convert.iConvertToInt(chkRights.ID);
        //                for (int j = 0; j < DtRights.Rows.Count; j++)
        //                {
        //                    imenuID = Func.Convert.iConvertToInt(DtRights.Rows[j]["MenuID"].ToString());
        //                    iRightID = Func.Convert.iConvertToInt(DtRights.Rows[j]["RightsID"].ToString());
        //                    if (menuID == imenuID && RightID == iRightID)
        //                    {
        //                        chkRights.Checked = true;
        //                    }
        //                }

        //                icount++;
        //            }

        //            icount = 3;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }


        //}
        private void CheckUserMenuRights()
        {
            try
            {
                clsUser objectUser = new clsUser();
                int iRCount = 0;
                //DataTable DtRights = new DataTable();
                DataSet DSRights = new DataSet();
                DSRights = objectUser.GetPermissionDetails(Func.Convert.iConvertToInt(lblID.Text), "UserMenuRights");
                DropDownList ddlClaimSlabs = new DropDownList();
                CheckBox chkRights = new CheckBox();
                ArrayList RightsList = new ArrayList();
                RightsList = (ArrayList)ViewState["RightdID"];
                iRCount = RightsList.Count;
                int icount = 3;
                int menuID = 0;
                int RightID = 0;
                int imenuID = 0;
                int iRightID = 0;
                foreach (GridViewRow objRow in gridPermission.Rows)
                {

                    for (int i = 0; i < iRCount; i++)
                    {
                        chkRights = (CheckBox)objRow.Cells[icount].FindControl(RightsList[i].ToString());
                        menuID = Func.Convert.iConvertToInt(objRow.Cells[1].Text);
                        RightID = Func.Convert.iConvertToInt(chkRights.ID);
                        for (int j = 0; j < DSRights.Tables[0].Rows.Count; j++)
                        {
                            imenuID = Func.Convert.iConvertToInt(DSRights.Tables[0].Rows[j]["MenuID"].ToString());
                            iRightID = Func.Convert.iConvertToInt(DSRights.Tables[0].Rows[j]["RightsID"].ToString());
                            if (menuID == imenuID && RightID == iRightID)
                            {
                                chkRights.Checked = true;
                            }
                        }

                        icount++;
                    }

                    icount = 3;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        //change by Megha
        //private void FillUserRoleUserOrDealer()
        //{
        //    try
        //    {
        //        hdnSelStateorCountry.Value = "";
        //        for (int i = 0; i < lstCountryOrState.Items.Count; i++)
        //        {
        //            if (lstCountryOrState.Items[i].Selected == true)
        //            {
        //                hdnSelStateorCountry.Value = hdnSelStateorCountry.Value + lstCountryOrState.Items[i].Value + ",";
        //            }
        //        }

        //        clsUser objclsUser = new clsUser();
        //        DataTable dtUserRoleUserOrDealer = new DataTable();
        //        DataSet dsUserRoleUserOrDealer = new DataSet();
        //        dsUserRoleUserOrDealer = objclsUser.GetRoleUserRoleOrDealer(hdnSelRegion.Value, hdnSelStateorCountry.Value, Func.Convert.iConvertToInt(lblID.Text), drpDept.SelectedValue);  //sSelDept          
        //        if (dsUserRoleUserOrDealer != null)
        //        {
        //            lblRoleSelections.Text = objclsUser.UseRoleUserDealerListName(UserRole, UsreType, drpDept.SelectedValue, 1);//sSelDept
        //            if (dsUserRoleUserOrDealer.Tables[0].Rows.Count > 0)
        //            {
        //                lstRoleSelection.DataSource = dsUserRoleUserOrDealer.Tables[0];
        //                lstRoleSelection.DataTextField = "Name";
        //                lstRoleSelection.DataValueField = "ID";
        //                lstRoleSelection.DataBind();
        //            }
        //            else
        //            {
        //                dtUserRoleUserOrDealer = null;
        //                lstRoleSelection.Items.Clear();
        //                lstRoleSelection.DataSource = dsUserRoleUserOrDealer.Tables[0];
        //                lstRoleSelection.DataBind();
        //            }

        //        }
        //        CheckSelectedUserRoleUserOrDealer(Func.Convert.iConvertToInt(lblID.Text));
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }

        //}

        private void FillUserRoleUserOrDealer()
        {
            try
            {
                hdnSelStateorCountry.Value = "";
                for (int i = 0; i < lstCountryOrState.Items.Count; i++)
                {
                    if (lstCountryOrState.Items[i].Selected == true)
                    {
                        hdnSelStateorCountry.Value = hdnSelStateorCountry.Value + lstCountryOrState.Items[i].Value + ",";
                    }
                }

                clsUser objclsUser = new clsUser();
                DataTable dtUserRoleUserOrDealer = new DataTable();
                DataSet dsUserRoleUserOrDealer = new DataSet();
                dsUserRoleUserOrDealer = objclsUser.GetRoleUserRoleOrDealer(hdnSelRegion.Value, hdnSelStateorCountry.Value, Func.Convert.iConvertToInt(lblID.Text), drpDept.SelectedValue);  //sSelDept          
                if (dsUserRoleUserOrDealer != null)
                {
                    lblRoleSelections.Text = objclsUser.UseRoleUserDealerListName(UserRole, UsreType, drpDept.SelectedValue, 1);//sSelDept
                    if (dsUserRoleUserOrDealer.Tables[0].Rows.Count > 0)
                    {
                        lstRoleSelection.DataSource = dsUserRoleUserOrDealer.Tables[0];
                        lstRoleSelection.DataTextField = "Name";
                        lstRoleSelection.DataValueField = "ID";
                        lstRoleSelection.DataBind();
                    }
                    else
                    {
                        dtUserRoleUserOrDealer = null;
                        lstRoleSelection.Items.Clear();
                        lstRoleSelection.DataSource = dsUserRoleUserOrDealer.Tables[0];
                        lstRoleSelection.DataBind();
                    }
                    //Megha 29062015 BUS
                    //lblRoleSelections1.Text = objclsUser.UseRoleUserDealerListName(UserRole, UsreType, drpDept.SelectedValue, 2);//sSelDept
                    lblRoleSelections1.Text = objclsUser.UseRoleUserDealerListName(UserRole, UsreType, drpDept.SelectedValue, 3);//sSelDept//2
                    if (dsUserRoleUserOrDealer.Tables[1].Rows.Count > 0)
                    {
                        lstRoleSelection1.DataSource = dsUserRoleUserOrDealer.Tables[1];
                        lstRoleSelection1.DataTextField = "Name";
                        lstRoleSelection1.DataValueField = "ID";
                        lstRoleSelection1.DataBind();
                    }
                    else
                    {
                        dtUserRoleUserOrDealer = null;
                        lstRoleSelection1.Items.Clear();
                        lstRoleSelection1.DataSource = dsUserRoleUserOrDealer.Tables[1];
                        lstRoleSelection1.DataBind();
                    }
                    lblRoleSelections1.Style.Add("display", "none");
                    lstRoleSelection1.Style.Add("display", "none");
                    ChkRoleSelection1.Style.Add("display", "none");
                    lblRoleSelection1.Style.Add("display", "none");           
                    //Other
                    lblRoleSelections2.Text = objclsUser.UseRoleUserDealerListName(UserRole, UsreType, drpDept.SelectedValue, 2);//sSelDept//3
                    if (dsUserRoleUserOrDealer.Tables[2].Rows.Count > 0)
                    {
                        lstRoleSelection2.DataSource = dsUserRoleUserOrDealer.Tables[2];
                        lstRoleSelection2.DataTextField = "Name";
                        lstRoleSelection2.DataValueField = "ID";
                        lstRoleSelection2.DataBind();
                    }
                    else
                    {
                        dtUserRoleUserOrDealer = null;
                        lstRoleSelection2.Items.Clear();
                        lstRoleSelection2.DataSource = dsUserRoleUserOrDealer.Tables[2];
                        lstRoleSelection2.DataBind();
                    }
                    lblRoleSelections2.Style.Add("display", "none");
                    lstRoleSelection2.Style.Add("display", "none");
                    ChkRoleSelection2.Style.Add("display", "none");
                    lblRoleSelection2.Style.Add("display", "none");           
                    //Megha 29062015
                }
                CheckSelectedUserRoleUserOrDealer(Func.Convert.iConvertToInt(lblID.Text));
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //changes by megha
        //protected DataTable SaveUserRoleUserOrDealer()
        //{
        //    DataRow dr;
        //    DataTable dtDetails = new DataTable();
        //    try
        //    {
        //        //DataRow dr;
        //        //DataTable dtDetails = new DataTable();
        //        dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
        //        dtDetails.Columns.Add(new DataColumn("ParentUserID", typeof(int)));

        //        for (int j = 0; j < lstRoleSelection.Items.Count; j++)
        //        {
        //            if (lstRoleSelection.Items[j].Selected == true)
        //            {
        //                dr = dtDetails.NewRow();
        //                dr["ParentUserID"] = Func.Convert.iConvertToInt(lblID.Text);
        //                dr["UserID"] = Func.Convert.iConvertToInt(lstRoleSelection.Items[j].Value);
        //                dtDetails.Rows.Add(dr);
        //                dtDetails.AcceptChanges();
        //            }
        //        }
        //        //return dtDetails;   
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }
        //    return dtDetails;   
        //}

        protected DataTable SaveUserRoleUserOrDealer()
        {
            DataRow dr;
            DataTable dtDetails = new DataTable();
            try
            {
                //DataRow dr;
                //DataTable dtDetails = new DataTable();
                dtDetails.Columns.Add(new DataColumn("UserID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("ParentUserID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("HDFlag", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("LMDFlag", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("BUSFlag", typeof(string)));
                dtDetails.Columns.Add(new DataColumn("DFlag", typeof(string)));


                for (int j = 0; j < lstRoleSelection.Items.Count; j++)
                {
                    if (lstRoleSelection.Items[j].Selected == true)
                    {
                        dr = dtDetails.NewRow();
                        dr["ParentUserID"] = Func.Convert.iConvertToInt(lblID.Text);
                        dr["UserID"] = Func.Convert.iConvertToInt(lstRoleSelection.Items[j].Value);
                        dr["HDFlag"] = (drpDept.SelectedItem.Text == "Spares" && UserRole != 15) ? "N" : (hdnHD.Value == "Y") ? "Y" : "N";
                        dr["LMDFlag"] = (UserRole != 15) ? "N" : "Y";
                        dr["BUSFlag"] = (UserRole != 15) ? "N" : "Y";
                        dr["DFlag"] = (UserRole == 15) ? "Y" : (hdnDefault.Value == "Y") ? "Y" : "N";
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                    }
                }
                for (int j = 0; j < lstRoleSelection1.Items.Count; j++)
                {
                    if (lstRoleSelection1.Items[j].Selected == true)
                    {
                        for (int l = 0; l < dtDetails.Rows.Count; l++)
                        {
                            if (Func.Convert.iConvertToInt(dtDetails.Rows[l]["UserID"]) == Func.Convert.iConvertToInt(lstRoleSelection1.Items[j].Value))
                            {
                                dtDetails.Rows[l]["LMDFlag"] = (hdnLMD.Value == "Y") ? "Y" : "N";
                                goto Last;
                            }

                        }
                        dr = dtDetails.NewRow();
                        dr["ParentUserID"] = Func.Convert.iConvertToInt(lblID.Text);
                        dr["UserID"] = Func.Convert.iConvertToInt(lstRoleSelection1.Items[j].Value);
                        dr["HDFlag"] = "N";
                        dr["LMDFlag"] = "Y";
                        dr["BUSFlag"] = "N";
                        dr["DFlag"] = "N";
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                    }

                Last: ;

                }
                for (int j = 0; j < lstRoleSelection2.Items.Count; j++)
                {
                    if (lstRoleSelection2.Items[j].Selected == true)
                    {
                        for (int l = 0; l < dtDetails.Rows.Count; l++)
                        {
                            if (Func.Convert.iConvertToInt(dtDetails.Rows[l]["UserID"]) == Func.Convert.iConvertToInt(lstRoleSelection2.Items[j].Value))
                            {
                                dtDetails.Rows[l]["BUSFlag"] = (hdnBUS.Value == "Y") ? "Y" : "N";
                                goto Last;

                            }

                        }

                        dr = dtDetails.NewRow();
                        dr["ParentUserID"] = Func.Convert.iConvertToInt(lblID.Text);
                        dr["UserID"] = Func.Convert.iConvertToInt(lstRoleSelection2.Items[j].Value);
                        dr["HDFlag"] = "N";
                        dr["LMDFlag"] = "N";
                        dr["BUSFlag"] = "Y";
                        dr["DFlag"] = "N";
                        dtDetails.Rows.Add(dr);
                        dtDetails.AcceptChanges();
                    }

                Last: ;
                }

                //return dtDetails;   
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            return dtDetails;
        }
        //changes by megha
        //private void CheckSelectedUserRoleUserOrDealer(int UserID)
        //{
        //    try
        //    {
        //        clsUser objectUser = new clsUser();
        //        DataTable dtRoleUserRole = objectUser.GetPermissionDetails(UserID, "UserRoleUser");


        //        if (dtRoleUserRole != null)
        //        {
        //            if (dtRoleUserRole.Rows.Count > 0)
        //            {
        //                CheckSelectedListItems(lstRoleSelection, dtRoleUserRole, "RoleUserID");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }



        //}

        private void CheckSelectedUserRoleUserOrDealer(int UserID)
        {
            try
            {
                clsUser objectUser = new clsUser();
                //DataTable dtRoleUserRole = objectUser.GetPermissionDetails(UserID, "UserRoleUser");
                DataSet dsRoleUserRole = objectUser.GetPermissionDetails(UserID, "UserRoleUser");
                //hdnHD.Value = "N";
                //hdnLMD.Value = "N";
                //hdnBUS.Value = "N";

                //hdnHDDatabase.Value = "N";
                //hdnLMDDatabase.Value = "N";
                //hdnBUSDatabase.Value = "N";

                if (dsRoleUserRole != null)
                {
                    if (dsRoleUserRole.Tables[0].Rows.Count > 0)
                    {
                        CheckSelectedListItems(lstRoleSelection, dsRoleUserRole.Tables[0], "RoleUserID");
                        //hdnHD.Value = "Y";
                        //hdnHDDatabase.Value = "Y"; 
                    }
                    if (dsRoleUserRole.Tables[1].Rows.Count > 0)
                    {
                        CheckSelectedListItems(lstRoleSelection1, dsRoleUserRole.Tables[1], "RoleUserID");
                        //hdnLMD.Value = "Y";
                        //hdnLMDDatabase.Value = "Y"; 
                    }
                    if (dsRoleUserRole.Tables[2].Rows.Count > 0)
                    {
                        CheckSelectedListItems(lstRoleSelection2, dsRoleUserRole.Tables[2], "RoleUserID");
                        //hdnBUS.Value = "Y";
                        //hdnBUSDatabase.Value = "Y"; 
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }



        }

        protected void lstCountryOrState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillUserRoleUserOrDealer();
            setEnableDisableState();
        }
        protected void ChkCountryOrState_CheckedChanged(object sender, EventArgs e)
        {
            FillUserRoleUserOrDealer();
            setEnableDisableState();
        }

        //private void CheckselectedDealers(int UserID)
        //{
        //    try
        //    {
        //        FillCountryStateRegionWise();
        //        clsUser objectUser = new clsUser();
        //        string CountryOrState = "";
        //        DataTable DTPermissionDetails;
        //        if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
        //        {
        //            DTPermissionDetails = objectUser.GetPermissionDetails(UserID, "Country");
        //            lblCountryOrState.Text = "COUNTRY:";
        //            CountryOrState = "CountryID";
        //        }
        //        else
        //        {
        //            DTPermissionDetails = objectUser.GetPermissionDetails(UserID, "State");
        //            lblCountryOrState.Text = "STATE:";
        //            CountryOrState = "StateID";
        //        }

        //        if (DTPermissionDetails != null)
        //        {
        //            if (DTPermissionDetails.Rows.Count > 0)
        //            {
        //                CheckSelectedListItems(lstCountryOrState, DTPermissionDetails, CountryOrState);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Func.Common.ProcessUnhandledException(ex);
        //    }


        //}
        private void CheckselectedDealers(int UserID)
        {
            try
            {
                FillCountryStateRegionWise();
                clsUser objectUser = new clsUser();
                string CountryOrState = "";
                // DataTable DTPermissionDetails;
                DataSet DSPermissionDetails;
                if (drpUserType.SelectedValue == "1" || drpUserType.SelectedValue == "4")
                {
                    DSPermissionDetails = objectUser.GetPermissionDetails(UserID, "Country");
                    lblCountryOrState.Text = "COUNTRY:";
                    CountryOrState = "CountryID";
                }
                else
                {
                    DSPermissionDetails = objectUser.GetPermissionDetails(UserID, "State");
                    lblCountryOrState.Text = "STATE:";
                    CountryOrState = "StateID";
                }

                if (DSPermissionDetails != null)
                {
                    if (DSPermissionDetails.Tables[0].Rows.Count > 0)
                    {
                        CheckSelectedListItems(lstCountryOrState, DSPermissionDetails.Tables[0], CountryOrState);
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        // Get all users from an Active Directory distribution group
        //public SortedList GetUsersInGroup(string domain, string group)
        //{
        //    SortedList groupMemebers = new SortedList();

        //    string sam = "";
        //    string fname = "";
        //    string lname = "";
        //    string active = "";

        //    DirectoryEntry de = new DirectoryEntry("LDAP://DC=" + domain + ",DC=com");

        //    DirectorySearcher ds = new DirectorySearcher(de, "(objectClass=person)");
        //    ds.Filter = "(memberOf=CN=" + group + ",OU=Distribution Groups,DC=" + domain + ",DC=com)";

        //    ds.PropertiesToLoad.Add("givenname");
        //    ds.PropertiesToLoad.Add("samaccountname");
        //    ds.PropertiesToLoad.Add("sn");
        //    ds.PropertiesToLoad.Add("useraccountcontrol");

        //    foreach (SearchResult sr in ds.FindAll())
        //    {
        //        try
        //        {
        //            sam = sr.Properties["samaccountname"][0].ToString();
        //            fname = sr.Properties["givenname"][0].ToString();
        //            lname = sr.Properties["sn"][0].ToString();
        //            active = sr.Properties["useraccountcontrol"][0].ToString();
        //        }
        //        catch (Exception e)
        //        {
        //        }

        //        // don't grab disabled users
        //        if (active.ToString() != "514")
        //        {
        //            groupMemebers.Add(sam.ToString(), (fname.ToString() + " " + lname.ToString()));
        //        }
        //    }

        //    return groupMemebers;
        //}


        protected void lstDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEnableDisableDept();
            FillUserRoleUserOrDealer();
        }

        private void setEnableDisableDept()
        {
            int iCount = 0;
            sSelDept = "";
            if (UsreType == 2)
            {
                for (int j = 0; j < lstDepartments.Items.Count; j++)
                {
                    if (lstDepartments.Items[j].Selected == true)
                    {
                        iCount = iCount + 1;
                        if (sSelDept == "")
                            sSelDept = Func.Convert.sConvertToString(lstDepartments.Items[j].Value);
                        else
                            sSelDept = sSelDept + "," + Func.Convert.sConvertToString(lstDepartments.Items[j].Value);
                    }
                    if (lstDepartments.SelectedValue == "")
                    {
                        lstDepartments.Items[j].Enabled = true;
                    }
                    else if (lstDepartments.SelectedValue == "6" && (UsreType == 2 || UsreType == 4))
                    {
                        if (lstDepartments.Items[j].Value != "6")
                            lstDepartments.Items[j].Enabled = false;
                    }
                    else if (lstDepartments.SelectedValue != "6" && (UsreType == 2 || UsreType == 4))
                    {
                        if (lstDepartments.Items[j].Value == "6")
                            lstDepartments.Items[j].Enabled = false;
                    }

                }

                //if (iCount == 1)
                //{
                //    if (sSelDept != "6")
                //        sSelDept = "0";
                //}
                //else
                //    sSelDept = "0";

            }
            else if (UsreType == 1)
            {
                for (int j = 0; j < lstDepartments.Items.Count; j++)
                {
                    if (lstDepartments.Items[j].Selected == true)
                    {
                        iCount = iCount + 1;
                        if (sSelDept == "")
                            sSelDept = Func.Convert.sConvertToString(lstDepartments.Items[j].Value);
                        else
                            sSelDept = sSelDept + "," + Func.Convert.sConvertToString(lstDepartments.Items[j].Value);
                    }
                }
            }
        }

        private void setEnableDisableRegion()
        {
            if (UsreType == 2 && (UserRole == 2 || UserRole == 3 || UserRole == 4))
            {
                ChkRegion.Enabled = false;
                int iCount = 0;

                for (int j = 0; j < lstRegion.Items.Count; j++)
                {

                    if (lstRegion.Items[j].Selected == true)
                    {
                        iCount = iCount + 1;
                    }
                    lstRegion.Items[j].Enabled = true;

                }
                if (iCount == 2)
                    for (int j = 0; j < lstRegion.Items.Count; j++)
                    {

                        if (lstRegion.Items[j].Selected == true)
                        {
                            iCount = iCount + 1;
                        }
                        if (lstRegion.SelectedValue == "")
                        {
                            lstRegion.Items[j].Enabled = true;
                        }
                        else if (lstRegion.Items[j].Selected == true)
                        {
                            lstRegion.Items[j].Enabled = true;
                        }
                        else
                        {
                            lstRegion.Items[j].Enabled = false;
                        }

                    }

            }
            else
            {
                ChkRegion.Enabled = true;
            }
        }

        private void setEnableDisableState()
        {
            if (UsreType == 2 && UserRole == 4)
            {
                ChkCountryOrState.Enabled = false;
                int iCount = 0;
                for (int j = 0; j < lstCountryOrState.Items.Count; j++)
                {

                    if (lstCountryOrState.Items[j].Selected == true)
                    {
                        iCount = iCount + 1;
                    }
                    lstCountryOrState.Items[j].Enabled = true;

                }
                if (iCount == 2)
                    for (int j = 0; j < lstCountryOrState.Items.Count; j++)
                    {

                        if (lstCountryOrState.SelectedValue == "")
                        {
                            lstCountryOrState.Items[j].Enabled = true;
                        }
                        else if (lstCountryOrState.Items[j].Selected == true)
                        {
                            lstCountryOrState.Items[j].Enabled = true;
                        }
                        else
                        {
                            lstCountryOrState.Items[j].Enabled = false;
                        }

                    }


            }
            else if (UsreType == 2 && UserRole == 3)
            {

                int iCount = 0;
                for (int i = 0; i < lstCountryOrState.Items.Count; i++)
                {
                    if (lstCountryOrState.Items[i].Selected == true)
                    {
                        iCount = iCount + 1;
                        //if (sSelDept == "")
                        //    sSelDept = Func.Convert.sConvertToString(lstDepartments.Items[j].Value);
                        //else
                        //    sSelDept = sSelDept + "," + Func.Convert.sConvertToString(lstDepartments.Items[j].Value);
                    }
                    lstCountryOrState.Items[i].Enabled = true;
                }
                if (iCount == 3)
                    for (int j = 0; j < lstCountryOrState.Items.Count; j++)
                    {


                        if (lstCountryOrState.SelectedValue == "")
                        {
                            lstCountryOrState.Items[j].Enabled = true;
                            ChkCountryOrState.Enabled = true;
                        }
                        else if (lstCountryOrState.Items[j].Selected == true)
                        {
                            lstCountryOrState.Items[j].Enabled = true;
                            ChkCountryOrState.Enabled = true;
                        }
                        if (lstCountryOrState.Items[j].Selected == false)
                        {
                            lstCountryOrState.Items[j].Enabled = false;
                            ChkCountryOrState.Enabled = false;
                        }

                    }


            }
            else
            {
                ChkCountryOrState.Enabled = true;
            }
        }



        protected void drpLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserRole = Func.Convert.iConvertToInt(drpLevels.SelectedValue);
            hdnUserRole.Value = Func.Convert.sConvertToString(drpLevels.SelectedValue);
            if (UserRole == 8)
                UsreType = 5;
            Display();


        }

        protected void chkModelCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Func.Convert.iConvertToInt(drpDept.SelectedValue) == 6)// || Func.Convert.iConvertToInt(drpDept.SelectedValue) == 7
            {
                if (chkModelCategory.Items[0].Selected == true)
                {
                    ChkRoleSelection.Enabled = true;
                    lstRoleSelection.Enabled = true;
                    hdnDefault.Value = "Y";
                }
                else
                {
                    ChkRoleSelection.Enabled = false;
                    lstRoleSelection.Enabled = false;
                    hdnDefault.Value = "N";
                }
            }

            if (Func.Convert.iConvertToInt(drpDept.SelectedValue) != 6)//&& Func.Convert.iConvertToInt(drpDept.SelectedValue) != 7
            {
                if (chkModelCategory.Items[2].Selected == true)//0
                {
                    ChkRoleSelection2.Enabled = true;
                    lstRoleSelection2.Enabled = true;
                    hdnBUS.Value = "Y";
                }
                else
                {
                    ChkRoleSelection2.Enabled = false;
                    lstRoleSelection2.Enabled = false;
                    hdnBUS.Value = "N";
                }
                if (chkModelCategory.Items[0].Selected == true)//1
                {
                    ChkRoleSelection.Enabled = true;
                    lstRoleSelection.Enabled = true;
                    hdnHD.Value = "Y";
                }
                else
                {
                    ChkRoleSelection.Enabled = false;
                    lstRoleSelection.Enabled = false;
                    hdnHD.Value = "N";
                }

                if (chkModelCategory.Items[1].Selected == true)//2
                {

                    ChkRoleSelection1.Enabled = true;
                    lstRoleSelection1.Enabled = true;
                    hdnLMD.Value = "Y";
                }
                else
                {
                    ChkRoleSelection1.Enabled = false;
                    lstRoleSelection1.Enabled = false;
                    hdnLMD.Value = "N";
                }
            }
            //clsUser objclsUser = new clsUser();
            //DataTable dtAllowedtoDeselect = new DataTable();
            //DataSet dsAllowedtoDeselect = new DataSet();
            //dsAllowedtoDeselect = objclsUser.ValidatePermissionDetails(Func.Convert.iConvertToInt(lblID.Text), hdnSelRegion.Value, hdnSelStateorCountry.Value, drpDept.SelectedValue.ToString());  //sSelDept          

            //chkModelCategory.Items[1].Selected = (dsAllowedtoDeselect.Tables[0].Rows[0]["AllowedDeselect"].ToString()  == "Y") ? false  : true ;
            //chkModelCategory.Items[0].Selected = (hdnBUS.Value == "Y") ? true : false;
            //chkModelCategory.Items[2].Selected = (hdnLMD.Value == "Y") ? true : false;
        }

    }
}