using System;
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
using MANART.WebParts;
using AjaxControlToolkit;
using System.Drawing;

namespace MANART.Forms.Master
{
    public partial class frmDynamicMaster : System.Web.UI.Page
    {
        #region Private Variables

        private clsSaveMaster.MasterType l_ValMasterType;
        private HtmlTableRow row = new HtmlTableRow();
        private HtmlTableCell cell = new HtmlTableCell();
        private int l_iRowLineNo = 0;
        private int l_iFormControlCount = 0;
        private string[] l_sTableUsed;
        private clsGlobal.enmFormState l_ValenmFormState = new clsGlobal.enmFormState();
        private int l_iNoOfControlOnRow = 0;
        private string l_sQueryBindToGrid = "";
        private string l_sGridPanelTitle = "";
        private string l_sFormDetailsGridID = "";
        private int iIDForFillDetailsGrid = 0;
        private string sWhereClauseForFillDetailsGrid = "";
        private string sMenuId;
        private string new_ID;
        private string sql = "";
        string sMessage = "";
        bool bDependentableFlag = false;
        private string sDealerCode = "";
        private string _sEffDate = "";
        private string _sEffFromDate = "";
        private string _sEffToDate = "";
        public string sEffDate
        {
            get
            {
                return _sEffDate;
            }
            set
            {
                _sEffDate = value;
            }
        }
        public string sEffFromDate
        {
            get
            {
                return _sEffFromDate;
            }
            set
            {
                _sEffFromDate = value;
            }
        }
        public string sEffToDate
        {
            get
            {
                return _sEffToDate;
            }
            set
            {
                _sEffToDate = value;
            }
        }

        #endregion

        #region Page Methods
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ToolbarC.iFormIdToOpenForm = 9999;
                //Megha 23092011
                ToolbarC.iValidationIdForSave = 9999;
                //Megha 23092011
                sMenuId = Func.Convert.sConvertToString(Request.QueryString["MenuID"]);
                sDealerCode = Func.Convert.sConvertToString(Session["sDealerCode"]);

                if (sMenuId == "")
                {
                    Server.Transfer(@"~/Default.aspx");
                }

                else
                {
                    CreatAndAddControlToForm(sMenuId);

                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SearchGrid.bIsCollapsable = false;

                Page.Title = "MAN-" + lblTitle.Text;
                //        if (!Page.ClientScript.IsStartupScriptRegistered(this.GetType(), onloadScriptName))
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "","window.onload = function() {ShowMe();};", true);  
                //if (sMenuId == "401")
                //{
                //    if (drpTarget.Items.Count > 0)
                //    {
                //        //drpTarget.SelectedValue=drpTarget.Items[1].Value;
                //        //dropdownList_SelectedIndexChanged(null, null);
                //    }
                //}   
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
                string strDisAbleBackButton;
                strDisAbleBackButton = "<SCRIPT language=javascript>\n";
                strDisAbleBackButton += "window.history.forward(1);\n";
                strDisAbleBackButton += "\n</SCRIPT>";
                ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
                btnReadonly();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        // Function Use for Readonly User
        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();
                if (objCommon.sUserRole == "15")
                {
                    // Change By Vikram K
                    //ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmNew, false);
                }
                //Megha 25012012  --Confirm, cancel and Print button non editable
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmConfirm, false);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmCancel, false);
                ToolbarC.EnableDisableImage(Toolbar.enmToolbarType.enmPrint, false);
                //Megha 25012012
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objCommon != null) objCommon = null;
            }
        }

        #endregion

        #region Function Related To Form Controls
        /// <summary>
        /// Create And Add Control to form 
        /// </summary>
        private void CreatAndAddControlToForm(string sMenuID)
        {
            try
            {
                clsCommon ObjCommon = new clsCommon();
                DataSet dsFrmControlData = null;
                SControl.SText ObjTextbox = null;
                SControl.SComboBox ObjCombobox = null;
                // Change By VIkram K
               MANART.WebParts.CurrentDate ObjDate = null;
                //ASP.webparts_currentdate_ascx ObjDate = null;

                string sParentControlId = "";
                string sTblNameForFill = "";
                SControl.SGrid ObjGrid = null;
                Label ObjLabel = null;
                System.Web.UI.WebControls.Image imgforDate = new System.Web.UI.WebControls.Image();
                string sControlTypeID = "0";
                int iControlTypeID = 0;
                int iPositionLineNo = 0;
                string sCtrlId = "1";
                string sControlName = "";
                string sTmpTableUsed = "";
                bool bManadatory = false;
                dsFrmControlData = ObjCommon.GetFormControlData(sMenuID);

                if (dsFrmControlData.Tables[0].Rows.Count == 0)
                {
                    //Server.Transfer(@"~/Default.aspx");
                    //Response.Write("Record Not Exist For Selected Menu.");
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Form Is Not Attached To Menu Or Data Not Exist For The Selected Menu.');</script>");
                    //Server.Transfer(@"~/Default.aspx");
                    return;
                }
                // set Form Title

                lblTitle.Text = Func.Convert.sConvertToString(dsFrmControlData.Tables[0].Rows[0]["Title"]);
                sTmpTableUsed = Func.Convert.sConvertToString(dsFrmControlData.Tables[0].Rows[0]["TblName"]);
                l_ValMasterType = (clsSaveMaster.MasterType)Func.Convert.iConvertToInt(dsFrmControlData.Tables[0].Rows[0]["FormTypeID"]);
                //Megha
                txtFormTypeID.Text = Func.Convert.sConvertToString(dsFrmControlData.Tables[0].Rows[0]["FormTypeID"]);
                //Megha
                if (sTmpTableUsed.Contains(',') == true)
                {
                    l_sTableUsed = sTmpTableUsed.Split(',');
                }
                else
                {
                    l_sTableUsed = new string[1];
                    l_sTableUsed[0] = sTmpTableUsed;
                }
                if (Func.Convert.sConvertToString(dsFrmControlData.Tables[0].Rows[0]["Toolbar_DisplayYN"]) == "N")
                {
                    ToolbarContainer.Visible = false;
                }
                //getrecord for Form Controls
                l_iFormControlCount = dsFrmControlData.Tables[1].Rows.Count;
                txtControlCount.Text = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows.Count);
                for (int iCnt = 0; iCnt < l_iFormControlCount; iCnt++)
                {
                    #region Set Default Property Of Each Control
                    //Default Control Property
                    sControlTypeID = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ControlTypeId"]);
                    iControlTypeID = Func.Convert.iConvertToInt(sControlTypeID);
                    iPositionLineNo = Func.Convert.iConvertToInt(dsFrmControlData.Tables[1].Rows[iCnt]["PositionLineNo"]);
                    sCtrlId = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ControlID"]);
                    sControlName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["Name"]);
                    if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["AllowSearch"]) == true)
                    {
                        SearchGrid.AddToSearchCombo(sControlName);
                    }
                    // to Create Caption for Control
                    ObjLabel = new Label();
                    ObjLabel.ID = "lbl" + sCtrlId;
                    // CHanged y VIkram K
                    //ObjLabel.Attributes["class"] = "tdLabel";
                    ObjLabel.Attributes["cssClass"] = "control-label";
                    ObjLabel.Text = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["Caption"]);
                    bManadatory = Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["IsMandatory"]);

                    # endregion

                    if (iControlTypeID >= 1 && iControlTypeID <= 30)//Control is of type of Texbox   
                    {
                        #region  Set TextboxPropertry

                        // Set TextboxPropertry                
                        ObjTextbox = new SControl.SText((SControl.enmTextBoxType)iControlTypeID);
                        //Set Default Value                
                        ObjTextbox.ID = sCtrlId;
                        ObjTextbox.Name = sControlName;
                        //Megha 25052012 increase width of textbox 
                        // CHnaged by VIkram 
                        ObjTextbox.Attributes["cssClass"] = "form-control";
                        //ObjTextbox.Width = 150;
                        //Megha 25052012 
                        ObjTextbox.MaxLength = Func.Convert.iConvertToInt(dsFrmControlData.Tables[1].Rows[iCnt]["MaxLength"]);
                        ObjTextbox.MapToTblFieldName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["MapToTblFieldName"]);
                        ObjTextbox.ToolTipText = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ToolTipText"]);
                        if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["AllowTochange"]) == true)
                        {
                            ObjTextbox.AllowTochange = true;
                        }
                        if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["AllowDuplicate"]) == false)
                        {
                            ObjTextbox.AllowDuplicate = false;
                        }
                        ObjTextbox.EnableViewState = false;
                        // ObjTextbox.EnableViewState = true; 

                        if (l_ValenmFormState == clsGlobal.enmFormState.enmEmpty_State)
                        {
                            if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["Read_Only"]) == true)
                            {
                                ObjTextbox.ReadOnly = true;
                            }
                        }

                        AddControlToCell(ObjLabel, ObjTextbox, iPositionLineNo, bManadatory);
                        #endregion
                    }
                    else if (iControlTypeID >= 31 && iControlTypeID <= 40)//for combobox
                    {
                        #region Set Combobox Propety

                        if (sControlTypeID.ToString() == "30")
                        {
                        }
                        else if (sControlTypeID.ToString() == "31")
                        {
                            ObjCombobox = new SControl.SComboBox(SControl.enmComboBoxType.enmYesNoCombo);
                            ObjCombobox.DataBind(clsGlobal.enmConnectionType.enmconnectionOfMaster);
                        }
                        else if (sControlTypeID.ToString() == "32")
                        {
                            ObjCombobox = new SControl.SComboBox(SControl.enmComboBoxType.enmDataCombo);
                            ObjCombobox.QueryBindToControl = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["QueryBindToControl"]);
                            ObjCombobox.DataBind(clsGlobal.enmConnectionType.enmconnectionOfMaster);

                        }
                        else if (sControlTypeID.ToString() == "33")
                        {
                            ObjCombobox = new SControl.SComboBox(SControl.enmComboBoxType.enmFixedCombo);
                            ObjCombobox.QueryBindToControl = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["QueryBindToControl"]);
                            ObjCombobox.DataBind(clsGlobal.enmConnectionType.enmconnectionOfMaster);

                        }
                        //SetDefault value
                        ObjCombobox.ID = sCtrlId;
                        sControlName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["Name"]);
                        ObjCombobox.Name = sControlName;
                        //Megha 25052012 increase width of combobox 
                        ObjCombobox.Width = 200;
                        //Megha 25052012
                        ObjCombobox.MapToTblFieldName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["MapToTblFieldName"]);
                        ObjCombobox.ToolTipText = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ToolTipText"]);
                        ObjCombobox.MaxLength = Func.Convert.iConvertToInt(dsFrmControlData.Tables[1].Rows[iCnt]["MaxLength"]);
                        ObjCombobox.EnableViewState = false;
                        //ObjCombobox.EnableViewState = true;

                        if (l_ValenmFormState == clsGlobal.enmFormState.enmEmpty_State)
                        {
                            if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["Read_Only"]) == true)
                            {
                                ObjCombobox.ReadOnly = true;
                            }
                        }
                        if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["AllowTochange"]) == true)
                        {
                            ObjCombobox.AllowTochange = true;
                        }
                        //Set working of grid fill is on depend on which control    
                        sParentControlId = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ParentControlID"]);
                        sTblNameForFill = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["TblNameForFill"]);

                        if (sParentControlId != "" && sParentControlId != "0")
                        {
                            SetParentComboBox(ObjCombobox.UniqueID, sParentControlId, sTblNameForFill);
                        }
                        AddControlToCell(ObjLabel, ObjCombobox, iPositionLineNo, bManadatory);
                        # endregion
                    }
                    else if (iControlTypeID >= 51 && iControlTypeID < 60)//for date 
                    {
                        #region Set Date Control Property
                        // ObjDate = new ASP.webparts_currentdate_ascx((SControl.enmDateType)iControlTypeID);

                        // Change By VIkram k
                        // ObjDate = new ASP.webparts_currentdate_ascx();  // old 
                        ObjDate = new MANART.WebParts.CurrentDate();

                        ///ObjDate.bCheckforCurrentDate = false;
                        //ObjDate.bCheckforCurrentDate = true;
                        ObjDate.Mandatory = false;
                        //SetDefault Value              
                        ObjDate.ID = sCtrlId;
                        //Megha 25052012 increase width of Date
                        ObjDate.Width = 59;
                        //Megha 25052012
                        //for single date
                        sEffDate = sCtrlId;

                        //for from date and To Date
                        if (sEffFromDate == "")
                            sEffFromDate = sCtrlId;
                        else
                        {
                            sEffToDate = sCtrlId;
                            sEffDate = "";
                        }

                        //ObjDate.Name = sControlName;
                        ObjDate.MapToTblFieldName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["MapToTblFieldName"]);
                        ObjDate.ToolTipText = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ToolTipText"]);
                        ObjDate.EnableViewState = false;
                        //ObjDate.Attributes["AutoPostBack"] = "true";
                        ObjDate.Attributes.Add("runat", "server");
                        if (bManadatory == true)
                        {
                            ObjDate.Mandatory = true;
                        }
                        if (l_ValenmFormState == clsGlobal.enmFormState.enmEmpty_State)
                        {
                            if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["Read_Only"]) == true)
                            {
                                //ObjDate.ReadOnly = true;
                                ObjDate.Enabled = false;
                            }
                        }
                        if (Func.Convert.bConvertToBoolean(dsFrmControlData.Tables[1].Rows[iCnt]["AllowTochange"]) == true)
                        {
                            ObjDate.AllowTochange = true;
                        }
                        AddControlToCell(ObjLabel, ObjDate, iPositionLineNo, bManadatory);
                        # endregion

                    }
                    else if (iControlTypeID >= 61 && iControlTypeID < 80)//for Grid
                    {
                        #region Set Grid Control Property
                        ObjGrid = new SControl.SGrid((SControl.GridType)iControlTypeID);
                        ObjGrid.ID = sCtrlId;
                        ObjGrid.QueryBindToControl = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["QueryBindToControl"]);

                        //Set working of grid fill is on depend on which control    
                        sParentControlId = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ParentControlID"]);
                        ObjGrid.TblNameForFill = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["TblNameForFill"]);
                        if (ObjGrid.TblNameForFill.Contains(".") == true)
                        {
                            sWhereClauseForFillDetailsGrid = ObjGrid.TblNameForFill.Substring(ObjGrid.TblNameForFill.IndexOf(".") + 1);
                        }
                        ObjGrid.Attributes["AutoPostBack"] = "true";
                        ObjGrid.Attributes.Add("runat", "server");
                        ObjGrid.EnableViewState = true;
                        if (iControlTypeID == 61)
                        {
                            l_sFormDetailsGridID = sCtrlId;
                        }
                        else
                        {
                            ObjGrid.BindDataToGrid();
                        }
                        ObjGrid.MapToTblFieldName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["MapToTblFieldName"]);
                        AddControlToCell(ObjLabel, ObjGrid, iPositionLineNo, bManadatory);
                        if (iControlTypeID != 61)
                        {
                            if (sParentControlId != "0" && sParentControlId != "")
                            {
                                l_sFormDetailsGridID = sCtrlId;
                                WebControl ObjTmpControl = (WebControl)ControlContainer.FindControl(sParentControlId);
                                if (ObjTmpControl.GetType().Name == "SComboBox")
                                {
                                    SControl.SComboBox ObjCombo = (SControl.SComboBox)ControlContainer.FindControl(sParentControlId);
                                    ObjCombo.SelectedIndexChanged += new EventHandler(dropdownList_SelectedIndexChanged);
                                    ObjCombo.AutoPostBack = true;
                                    ObjCombo.EnableViewState = true;
                                }

                            }
                        }

                        if (l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable)
                        {
                            SearchGrid.bIsCallForServer = true;
                            // SearchGrid.bIsCallForServer = false; 

                        }
                        if (l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster)
                        {
                            SearchGrid.bIsCallForServer = true;
                            // SearchGrid.bIsCallForServer = false; 

                        }
                        # endregion
                    }
                    else if (iControlTypeID >= 81 && iControlTypeID < 90)//for button
                    {
                        #region Set Button Control Property
                        SControl.SButton ObjButton = new SControl.SButton();
                        ObjButton.ID = sCtrlId;
                        sControlName = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["Name"]);
                        ObjButton.Text = sControlName;
                        //Set working of grid fill is on depend on which control    
                        sParentControlId = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["ParentControlID"]);
                        ObjButton.Attributes.Add("runat", "server");
                        ObjButton.Click += new EventHandler(btnShowDetails_Click);
                        if (sParentControlId != "0")
                        {
                            ObjButton.ParentControlID = sParentControlId;
                            ObjButton.QueryToExecute = Func.Convert.sConvertToString(dsFrmControlData.Tables[1].Rows[iCnt]["QueryBindToControl"]);
                        }
                        AddControlToCell(ObjLabel, ObjButton, iPositionLineNo, bManadatory);
                        # endregion
                    }
                    ObjCombobox = null;
                    ObjTextbox = null;
                    ObjDate = null;
                    ObjGrid = null;
                }
                //Set Fill Selection Grid 
                l_sQueryBindToGrid = Func.Convert.sConvertToString(dsFrmControlData.Tables[0].Rows[0]["QueryBindToSrchGrid"]);
                l_sGridPanelTitle = Func.Convert.sConvertToString(dsFrmControlData.Tables[0].Rows[0]["Title"]);
                FillSelectionGrid();
                //if (sMenuId == "270")
                //{
                //    FillDetailsGrid();
                //}
                //Set Toolbarbuttons working
                SetEnableDisableToolbarButtons(dsFrmControlData.Tables[2]);

                ObjCommon = null;
                ObjCombobox = null;
                ObjDate = null;
                ObjTextbox = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        //To Fill Grid of Selection
        private void FillSelectionGrid()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid = null;
                string sControlName = "";
                WebControl currControl = null; ;

                //Change by Vikram k
                //ASP.webparts_currentdate_ascx dateControl;
                MANART.WebParts.CurrentDate dateControl;


                int iCntOfCol = 0;
                if (l_sQueryBindToGrid.Trim() != "")
                {
                    try
                    {
                        if (l_sQueryBindToGrid != "")
                        {
                            if (sMenuId == "632" || sMenuId == "851")
                            {
                                l_sQueryBindToGrid = l_sQueryBindToGrid + " where M_BayDetails.DealerID=" + Session["iDealerID"] + " and M_BayDetails.HOBrID =" + Session["HOBR_ID"];
                            }
                            if (sMenuId == "628")
                            {
                                if (sDealerCode.Trim().StartsWith("R"))
                                    l_sQueryBindToGrid = l_sQueryBindToGrid + " where  M_Lubricant.Lubricant_Type <>'New' and isnull(M_ModelMaster.DcodeOrRcode,'D')='R' order by M_ModelMaster.Model_name,M_Lubricant.ID ";
                                else
                                    l_sQueryBindToGrid = l_sQueryBindToGrid + " where  M_Lubricant.Lubricant_Type <>'New' order by M_ModelMaster.Model_name,M_Lubricant.ID";
                            }
                            
                            dsSrchgrid = objDB.ExecuteQueryAndGetDataset(l_sQueryBindToGrid);
                            SearchGrid.sGridPanelTitle = l_sGridPanelTitle + " List";
                            if (dsSrchgrid.Tables[0].Rows.Count != 0)
                            {
                                SearchGrid.bGridFillUsingSql = true;
                                SearchGrid.FillSelectionGrid(dsSrchgrid);
                            }
                            else
                            {
                                return;
                            }
                            //DataView dv = new DataView(dsSrchgrid);
                            //DataTable dt = new DataTable();
                            //dv.RowFilter = strSearchOnField + " like" + SearchExpression;
                            //dt = dv.ToTable();

                            txtID.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                            iCntOfCol++;
                            //fill form Controls    
                            #region SetDataToFormControls
                            //if (l_ValMasterType == clsSaveMaster.MasterType.enmEditableMaster)
                            //{
                            txtFormType.Text = l_ValMasterType.ToString();
                            for (int i = 1; i <= l_iFormControlCount; i++)
                            {
                                if (iCntOfCol > dsSrchgrid.Tables[0].Columns.Count)
                                {
                                    goto FillDetails;
                                }
                                else
                                {
                                    iCntOfCol++;
                                }
                                // If control is Webcontrol else it will be a Date Control
                                try
                                {
                                    currControl = (WebControl)ControlContainer.FindControl((i).ToString());
                                }
                                catch
                                {
                                    try
                                    {
                                        //dateControl = (ASP.webparts_currentdate_ascx)ControlContainer.FindControl((i).ToString());
                                        dateControl = (CurrentDate)ControlContainer.FindControl((i).ToString());
                                        dateControl.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                        currControl = null;
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (currControl != null)
                                {
                                    sControlName = currControl.GetType().Name;
                                    if (sControlName == "SText")
                                    {
                                        (currControl as SControl.SText).Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                    }
                                    else if (sControlName == "SComboBox")
                                    {
                                        SControl.SComboBox ObjCombo = (SControl.SComboBox)currControl;
                                        if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmFixedCombo)
                                        {
                                            //ObjCombo.SelectedItem.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                            string sDataValue = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]).Trim();
                                            int iItemCnt = 0;
                                            while (iItemCnt < ObjCombo.Items.Count)
                                            {
                                                if (ObjCombo.Items[iItemCnt].Text == sDataValue)
                                                {
                                                    ObjCombo.SelectedIndex = iItemCnt;
                                                    break;
                                                }
                                                iItemCnt++;
                                            }
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmYesNoCombo)
                                        {
                                            if (Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]) == "Y")
                                            {
                                                ObjCombo.SelectedValue = "1";
                                            }
                                            else if (Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]) == "N")
                                            {
                                                ObjCombo.SelectedValue = "2";
                                            }
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmDataCombo)
                                        {
                                            // ObjCombo.SelectedIndex = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                            string sDataValue = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]).Trim();
                                            int iItemCnt = 0;
                                            while (iItemCnt < ObjCombo.Items.Count)
                                            {
                                                if (ObjCombo.Items[iItemCnt].Text.Trim() == sDataValue.Trim())
                                                {
                                                    ObjCombo.SelectedIndex = iItemCnt;
                                                    break;
                                                }
                                                iItemCnt++;
                                            }

                                        }
                                    }
                                }
                            }
                        FillDetails:
                            {
                                iIDForFillDetailsGrid = Func.Convert.iConvertToInt(txtID.Text);
                                FillDetailsGridAfterImageClick();
                            }
                            //}
                            #endregion
                            //else
                            //{
                            //    txtFormType.Text = "";
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        string sextype = ex.GetType().ToString();
                    }
                }
                else
                {
                    SearchGrid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }
        private void FillSelectionGridOnlySpecific(string ID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsSrchgrid = null;
                string sControlName = "";
                WebControl currControl = null; ;

                //ASP.webparts_currentdate_ascx dateControl;
               MANART.WebParts.CurrentDate dateControl;
                int iCntOfCol = 0;
                if (l_sQueryBindToGrid.Trim() != "")
                {
                    try
                    {
                        if (l_sQueryBindToGrid != "")
                        {
                            dsSrchgrid = objDB.ExecuteQueryAndGetDataset(l_sQueryBindToGrid);
                            if (dsSrchgrid.Tables[0].Rows.Count != 0)
                            {
                                SearchGrid.bGridFillUsingSql = true;
                                SearchGrid.FillSelectionGrid(dsSrchgrid);
                            }
                            else
                            {
                                return;
                            }
                            DataView dv = new DataView(dsSrchgrid.Tables[0]);
                            DataTable dt = new DataTable();
                            dv.RowFilter = " Convert(ID, 'System.String')   like '*" + Convert.ToString(ID) + "'";
                            dt = dv.ToTable();

                            dsSrchgrid.Tables.Remove(dsSrchgrid.Tables[0]);
                            dsSrchgrid.Tables.Add(dt);
                            dsSrchgrid.AcceptChanges();
                            txtID.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                            iCntOfCol++;
                            //fill form Controls    
                            #region SetDataToFormControls
                            //if (l_ValMasterType == clsSaveMaster.MasterType.enmEditableMaster)
                            //{
                            txtFormType.Text = l_ValMasterType.ToString();
                            for (int i = 1; i <= l_iFormControlCount; i++)
                            {
                                if (iCntOfCol > dsSrchgrid.Tables[0].Columns.Count)
                                {
                                    goto FillDetails;
                                }
                                else
                                {
                                    iCntOfCol++;
                                }
                                // If control is Webcontrol else it will be a Date Control
                                try
                                {
                                    currControl = (WebControl)ControlContainer.FindControl((i).ToString());
                                }
                                catch
                                {
                                    try
                                    {
                                        //dateControl = (ASP.webparts_currentdate_ascx)ControlContainer.FindControl((i).ToString());
                                        dateControl = (CurrentDate)ControlContainer.FindControl((i).ToString());
                                        dateControl.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                        currControl = null;
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (currControl != null)
                                {
                                    sControlName = currControl.GetType().Name;
                                    if (sControlName == "SText")
                                    {
                                        (currControl as SControl.SText).Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                    }
                                    else if (sControlName == "SComboBox")
                                    {
                                        SControl.SComboBox ObjCombo = (SControl.SComboBox)currControl;
                                        if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmFixedCombo)
                                        {
                                            //ObjCombo.SelectedItem.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                            string sDataValue = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]).Trim();
                                            int iItemCnt = 0;
                                            while (iItemCnt < ObjCombo.Items.Count)
                                            {
                                                if (ObjCombo.Items[iItemCnt].Text == sDataValue)
                                                {
                                                    ObjCombo.SelectedIndex = iItemCnt;
                                                    break;
                                                }
                                                iItemCnt++;
                                            }
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmYesNoCombo)
                                        {
                                            if (Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]) == "Y")
                                            {
                                                ObjCombo.SelectedValue = "1";
                                            }
                                            else if (Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]) == "N")
                                            {
                                                ObjCombo.SelectedValue = "2";
                                            }
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmDataCombo)
                                        {
                                            // ObjCombo.SelectedIndex = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                            string sDataValue = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]).Trim();
                                            int iItemCnt = 0;
                                            while (iItemCnt < ObjCombo.Items.Count)
                                            {
                                                if (ObjCombo.Items[iItemCnt].Text == sDataValue)
                                                {
                                                    ObjCombo.SelectedIndex = iItemCnt;
                                                    break;
                                                }
                                                iItemCnt++;
                                            }

                                        }
                                    }
                                }
                            }
                        FillDetails:
                            {
                                iIDForFillDetailsGrid = Func.Convert.iConvertToInt(txtID.Text);
                                FillDetailsGridAfterImageClick();
                            }
                            //}
                            #endregion
                            //else
                            //{
                            //    txtFormType.Text = "";
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        string sextype = ex.GetType().ToString();
                    }

                }
                else
                {
                    SearchGrid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }




        }
        private void FillSelectionGridOnlyRegular(string ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet dsSrchgrid = null;
                string sControlName = "";
                WebControl currControl = null; ;
                //ASP.webparts_currentdate_ascx dateControl;
                MANART.WebParts.CurrentDate dateControl;

                int iCntOfCol = 0;
                if (l_sQueryBindToGrid.Trim() != "")
                {
                    try
                    {
                        if (l_sQueryBindToGrid != "")
                        {
                            dsSrchgrid = objDB.ExecuteQueryAndGetDataset(l_sQueryBindToGrid);
                            if (dsSrchgrid.Tables[0].Rows.Count != 0)
                            {
                                SearchGrid.bGridFillUsingSql = true;
                                SearchGrid.FillSelectionGrid(dsSrchgrid);
                            }
                            else
                            {
                                return;
                            }
                            DataView dv = new DataView(dsSrchgrid.Tables[0]);
                            DataTable dt = new DataTable();
                            dv.RowFilter = " Convert(ID, 'System.String')   like '*" + Convert.ToString(ID) + "'";
                            dt = dv.ToTable();

                            dsSrchgrid.Tables.Remove(dsSrchgrid.Tables[0]);
                            dsSrchgrid.Tables.Add(dt);
                            dsSrchgrid.AcceptChanges();
                            txtID.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                            iCntOfCol++;
                            //fill form Controls    
                            #region SetDataToFormControls
                            //if (l_ValMasterType == clsSaveMaster.MasterType.enmEditableMaster)
                            //{
                            txtFormType.Text = l_ValMasterType.ToString();
                            for (int i = 1; i <= l_iFormControlCount; i++)
                            {
                                if (iCntOfCol > dsSrchgrid.Tables[0].Columns.Count)
                                {
                                    goto FillDetails;
                                }
                                else
                                {
                                    iCntOfCol++;
                                }
                                // If control is Webcontrol else it will be a Date Control
                                try
                                {
                                    currControl = (WebControl)ControlContainer.FindControl((i).ToString());
                                }
                                catch
                                {
                                    try
                                    {
                                        //dateControl = (ASP.webparts_currentdate_ascx)ControlContainer.FindControl((i).ToString());
                                        dateControl = (CurrentDate)ControlContainer.FindControl((i).ToString());
                                        dateControl.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                        currControl = null;
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (currControl != null)
                                {
                                    sControlName = currControl.GetType().Name;
                                    if (sControlName == "SText")
                                    {
                                        (currControl as SControl.SText).Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                    }
                                    else if (sControlName == "SComboBox")
                                    {
                                        SControl.SComboBox ObjCombo = (SControl.SComboBox)currControl;
                                        if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmFixedCombo)
                                        {
                                            //ObjCombo.SelectedItem.Text = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                            string sDataValue = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]).Trim();
                                            int iItemCnt = 0;
                                            while (iItemCnt < ObjCombo.Items.Count)
                                            {
                                                if (ObjCombo.Items[iItemCnt].Text == sDataValue)
                                                {
                                                    ObjCombo.SelectedIndex = iItemCnt;
                                                    break;
                                                }
                                                iItemCnt++;
                                            }
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmYesNoCombo)
                                        {
                                            if (Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]) == "Y")
                                            {
                                                ObjCombo.SelectedValue = "1";
                                            }
                                            else if (Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]) == "N")
                                            {
                                                ObjCombo.SelectedValue = "2";
                                            }
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmDataCombo)
                                        {
                                            // ObjCombo.SelectedIndex = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]);
                                            string sDataValue = Func.Convert.sConvertToString(dsSrchgrid.Tables[0].Rows[0][iCntOfCol]).Trim();
                                            int iItemCnt = 0;
                                            while (iItemCnt < ObjCombo.Items.Count)
                                            {
                                                if (ObjCombo.Items[iItemCnt].Text == sDataValue)
                                                {
                                                    ObjCombo.SelectedIndex = iItemCnt;
                                                    break;
                                                }
                                                iItemCnt++;
                                            }

                                        }
                                    }
                                }
                            }
                        FillDetails:
                            {
                                iIDForFillDetailsGrid = Func.Convert.iConvertToInt(txtID.Text);
                                FillDetailsGridAfterImageClick();
                            }
                            //}
                            #endregion
                            //else
                            //{
                            //    txtFormType.Text = "";
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        string sextype = ex.GetType().ToString();
                    }
                }
                else
                {
                    SearchGrid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }




        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                iIDForFillDetailsGrid = Func.Convert.iConvertToInt(txtID.Text);

                FillDetailsGridAfterImageClick();
                if (sMenuId == "169" || sMenuId == "135")
                {
                    FillSelectionGridOnlySpecific(txtID.Text);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }



        }
        /// <summary>
        /// Create a cell and add control to container
        /// </summary>
        private void AddControlToCell(Control ObjCaptionControl, Control ObjControl, int iPositionLineNo, bool bManadatory)
        {
            try
            {
                if (l_iRowLineNo != iPositionLineNo)
                {
                    //if (iRowLineNo == 0)
                    //{
                    if (l_iNoOfControlOnRow == 1)
                    {
                        // when in a row there exist only one control then to create blank cell
                        // following code is created.
                        cell = new HtmlTableCell();
                        cell.Attributes["class"] = "EmptyRow";
                        cell.Attributes["colspan"] = "2";
                        row.Cells.Add(cell);
                        ControlContainer.Rows.Add(row);
                        l_iNoOfControlOnRow = 0;
                        row = new HtmlTableRow();
                        l_iRowLineNo = iPositionLineNo;
                    }
                    //}
                    else
                    {
                        row = new HtmlTableRow();
                        l_iRowLineNo = iPositionLineNo;
                        l_iNoOfControlOnRow = 0;
                    }

                }
                if (ObjCaptionControl != null)
                {
                    cell = new HtmlTableCell();
                    cell.BorderColor = "Black";
                    cell.Controls.Add(ObjCaptionControl);
                    cell.Width = Unit.Percentage(15).ToString();
                    row.Cells.Add(cell);
                }

                l_iNoOfControlOnRow = l_iNoOfControlOnRow + 1;

                if (ObjControl.GetType().Name == "SGrid")
                {
                    cell.Attributes["colspan"] = "4";
                    row.Cells.Add(cell);
                    ControlContainer.Rows.Add(row);
                    cell = new HtmlTableCell();
                    row = new HtmlTableRow();
                    cell.Attributes["colspan"] = "4";
                }
                else
                {
                    cell = new HtmlTableCell();
                }

                cell.Width = Unit.Percentage(18).ToString();
                cell.BorderColor = "Black";
                cell.Controls.Add(ObjControl);

                //Add Mandatory field        
                if (bManadatory == true)
                {
                    //If Control is type of the Date Control Or Grid Control then Mandatory (*) is not required
                    //if (ObjControl.GetType().Name == "webparts_currentdate_ascx" || ObjControl.GetType().Name == "SGrid")
                    if (ObjControl.GetType().Name == "  Man_Art.WebPart.CurrentDate" || ObjControl.GetType().Name == "SGrid")
                    {
                    }
                    else
                    {
                        //Add Mandatory field        
                        var span = new HtmlGenericControl("span");
                        span.InnerHtml = "<b class='Mandatory'>*</b>";
                        span.Attributes["Width"] = Unit.Pixel(5).ToString();
                        cell.Controls.Add(span);
                    }
                }
                row.Cells.Add(cell);
                ControlContainer.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        //Set Parent combo box
        private void SetParentComboBox(string sTargetControlID, string sParentControlID, string sTblNameForCombo)
        {
            try
            {
                //if (CascadingDropDown1.ParentControlID == "")
                //{
                //    CascadingDropDown1.Category = sTblNameForCombo;
                //    CascadingDropDown1.ParentControlID = sParentControlID;
                //    CascadingDropDown1.TargetControlID = sTargetControlID;

                //}
                //else if (CascadingDropDown2.ParentControlID == "")
                //{
                //    CascadingDropDown2.Category = sTblNameForCombo;
                //    CascadingDropDown2.TargetControlID = sTargetControlID;
                //    CascadingDropDown2.ParentControlID = sParentControlID;
                //}
                //else if (CascadingDropDown3.ParentControlID == "")
                //{
                //    CascadingDropDown3.Category = sTblNameForCombo;
                //    CascadingDropDown3.ParentControlID = sParentControlID;
                //    CascadingDropDown3.TargetControlID = sTargetControlID;
                //}
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void dropdownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iIDForFillDetailsGrid = Func.Convert.iConvertToInt(((System.Web.UI.WebControls.DropDownList)(sender)).SelectedValue);
                string sMapToTblFieldName = ((SControl.SComboBox)(sender)).MapToTblFieldName;
                sWhereClauseForFillDetailsGrid = sMapToTblFieldName.Substring(sMapToTblFieldName.IndexOf('.') + 1);
                FillDetailsGrid();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        private void FillDetailsGrid()
        {

            clsCommon ObjCommon = new clsCommon();
            try
            {
                if (l_sFormDetailsGridID != null)
                {
                    if (sWhereClauseForFillDetailsGrid == null)
                    {
                        if (Func.Convert.sConvertToString(Session["DetailsGrid"]) != "")
                        {
                            string sTempValue = Func.Convert.sConvertToString(Session["DetailsGrid"]);
                            iIDForFillDetailsGrid = Func.Convert.iConvertToInt(sTempValue.Substring(0, sTempValue.IndexOf("#")));
                            sWhereClauseForFillDetailsGrid = Func.Convert.sConvertToString(sTempValue.Substring(sTempValue.IndexOf("#") + 1));
                        }
                    }
                    SControl.SGrid ObjclsGrid = (SControl.SGrid)ControlContainer.FindControl(l_sFormDetailsGridID);
                    if (ObjclsGrid.ValenmGridType == SControl.GridType.enmWithCheckBox)
                    {
                        if (ObjclsGrid.TblNameForFill.Contains('.') == true)
                        {
                            string sTableName = ObjclsGrid.TblNameForFill.Substring(0, ObjclsGrid.TblNameForFill.IndexOf('.'));
                            string sFieldNameForSelect = ObjclsGrid.TblNameForFill.Substring(ObjclsGrid.TblNameForFill.IndexOf('.') + 1);
                            DataTable dt = ObjCommon.GetGridData(sTableName, sFieldNameForSelect, sWhereClauseForFillDetailsGrid, iIDForFillDetailsGrid);
                            ObjclsGrid.MarkForSelection(dt);
                            Session["DetailsGrid"] = iIDForFillDetailsGrid + "#" + sWhereClauseForFillDetailsGrid;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void FillDetailsGridAfterButtonClick(string sSQL)
        {
            try
            {
                if (sSQL != null)
                {
                    SControl.SGrid ObjclsGrid = (SControl.SGrid)ControlContainer.FindControl(l_sFormDetailsGridID);
                    if (ObjclsGrid.ValenmGridType == SControl.GridType.enmGrid)
                    {

                        ObjclsGrid.QueryBindToControl = sSQL;
                        ObjclsGrid.BindDataToGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }
        private void FillDetailsGridAfterImageClick()
        {
            clsCommon ObjCommon = new clsCommon();
            try
            {
                //Sujata 16022011
                //if (l_sFormDetailsGridID != null)
                if (l_sFormDetailsGridID != null && l_sFormDetailsGridID != "")
                //Sujata 16022011
                {
                    SControl.SGrid ObjclsGrid = (SControl.SGrid)ControlContainer.FindControl(l_sFormDetailsGridID);
                    if (ObjclsGrid.ValenmGridType == SControl.GridType.enmWithCheckBox)
                    {
                        if (ObjclsGrid.TblNameForFill.Contains('.') == true)
                        {
                            string sTableName = ObjclsGrid.MapToTblFieldName.Substring(0, ObjclsGrid.MapToTblFieldName.IndexOf('.'));
                            string sFieldNameForSelect = ObjclsGrid.MapToTblFieldName.Substring(ObjclsGrid.MapToTblFieldName.IndexOf('.') + 1);
                            DataTable dt = ObjCommon.GetGridData(sTableName, sFieldNameForSelect, sWhereClauseForFillDetailsGrid, iIDForFillDetailsGrid);
                            ObjclsGrid.MarkForSelection(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

            ObjCommon = null;
        }
        #endregion

        #region Function Related To Toolbar
        /// <summary>
        /// Function is used to set enable disable Toolbar buttons
        /// </summary>
        /// <param name="dtToolbarDetails"></param>
        private void SetEnableDisableToolbarButtons(DataTable dtToolbarDetails)
        {
            ToolbarC.EnableDisableImage(dtToolbarDetails);
        }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ImageButton ObjImageButton = (ImageButton)sender;
                if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {

                    bSaveRecord();
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }


        //Check Duplicate Record is exist
        private bool bCheckDuplicateRecord(string sTableName, string sFieldName, string sFieldValue)
        {
            try
            {
                string ssql = "select count(" + sFieldName + ")from " + sTableName + " where " + sFieldName + " = '" + sFieldValue + "'";
                //Megha02052011  Check Duplicate record only new entry
                if (Func.Common.bCheckRecordExist(ssql) == true && txtID.Text == "")
                //Megha02052011
                //  if (Func.Common.bCheckRecordExist(ssql) == true
                {
                    Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record already exist with " + sFieldValue + " can not save the record.');</script>");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }
        }

        //private bool bSaveRecordFertDealerLink(int Model_ID,DataTable dt)
        //{

        //}
        // to Save The Record
        private bool bSaveRecord()
        {
            try
            {
                string[] ssql;
                string sFieldName = "";
                string sTableName = "";
                string sBaseDeleteWhereClause = "";
                string[] sFieldList;
                string[] sValueList;
                string[] sWhereClause;
                int iNoOfTableUsed = 1;
                int iCurrTblIndex = 1;
                int ideleteCnt = 0;
                int ideleteCurrTblIndex = 0;
                string[] sID;
                string sControlName = "";
                SControl.SText ObjText = null;

                //ASP.webparts_currentdate_ascx ObjDate = null;
                MANART.WebParts.CurrentDate ObjDate = null;
                SControl.SComboBox ObjCombo = null;
                bool bSaveRecord = false;

                clsSaveMaster ObjSaveMaster;
                string sTmp = "";
                string sTableNameForDependable = "";
                string sIDNameForDependable = "";
                WebControl currControl = null;

                try
                {
                    #region First Step Initialise Variables
                    iNoOfTableUsed = Func.Convert.iConvertToInt(l_sTableUsed.GetLength(0));
                    // default array initialize
                    ssql = new string[iNoOfTableUsed];
                    sFieldList = new string[iNoOfTableUsed];
                    sValueList = new string[iNoOfTableUsed];
                    sWhereClause = new string[iNoOfTableUsed];
                    sID = new string[iNoOfTableUsed];
                    if (l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster)
                    {
                        txtID.Text = "";
                    }
                    if (txtID.Text == "")
                    {
                        for (int i = 0; i < iNoOfTableUsed; i++)
                        {

                            ssql[i] = " Insert Into " + l_sTableUsed[i];
                            sFieldList[i] = "(";
                        }
                        l_ValenmFormState = clsGlobal.enmFormState.enmNew_State;
                    }
                    else
                    {
                        l_ValenmFormState = clsGlobal.enmFormState.enmUpdate_State;

                        sTmp = txtID.Text;
                        if (sTmp.Contains(',') == true)
                        {
                            sID = txtID.Text.Split(',');
                        }
                        else
                        {
                            sID[0] = txtID.Text;
                        }
                        for (int i = 0; i < iNoOfTableUsed; i++)
                        {
                            ssql[i] = " Update " + l_sTableUsed[i];
                            sFieldList[i] = "";
                            if (sTmp.Length > i)
                            {
                                sWhereClause[i] = " Where Id=" + sTmp;
                            }
                            else
                            {
                                sWhereClause[i] = " Where Id=" + sTmp;
                                //sWhereClause[i] = "";
                            }
                        }

                    }
                    #endregion
                    // Get Data From Control and Build Sql
                    //Megha28052012   changes for G & B region mapping master
                    if (sMenuId == "227")
                    {
                        l_iFormControlCount = 5;
                    }
                    //Megha28052012 

                    for (int i = 1; i <= l_iFormControlCount; i++)
                    {
                        try
                        {
                            currControl = (WebControl)ControlContainer.FindControl((i).ToString());
                            if (sMenuId == "227" && i == 5)
                            {
                                currControl = (WebControl)ControlContainer.FindControl((3).ToString());
                            }

                        }
                        catch
                        {
                            try
                            {
                                //ObjDate = (ASP.webparts_currentdate_ascx)ControlContainer.FindControl((i).ToString());
                                ObjDate = (CurrentDate)ControlContainer.FindControl((i).ToString());
                                #region Date Control
                                if (ObjDate != null) //Check For DateField
                                {
                                    currControl = null;
                                    GetTableNameAndFieldName(ObjDate.MapToTblFieldName, l_sTableUsed, ref iCurrTblIndex, ref sFieldName, ref sTableName);
                                    if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                                    {
                                        sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + ",";
                                        sValueList[iCurrTblIndex] = sValueList[iCurrTblIndex] + "CONVERT(DATETIME,'" + ObjDate.Text + "',103),";
                                    }
                                    else if (l_ValenmFormState == clsGlobal.enmFormState.enmUpdate_State)
                                    {
                                        if (ObjDate.AllowTochange == true)
                                        {
                                            sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + "=CONVERT(DATETIME,'" + ObjDate.Text + "',103),";
                                        }
                                    }
                                }
                                #endregion
                            }
                            catch
                            { }
                        }
                        if (currControl != null)
                        {
                            sControlName = currControl.GetType().Name;
                            #region Textbox Control
                            if (sControlName == "SText")
                            {
                                ObjText = (SControl.SText)currControl;
                                GetTableNameAndFieldName(ObjText.MapToTblFieldName, l_sTableUsed, ref iCurrTblIndex, ref sFieldName, ref sTableName);
                                if (ObjText.AllowDuplicate == false)
                                {
                                    if (l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable)
                                    {
                                        if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                                        {
                                            if (bCheckDuplicateRecord(sTableName, sFieldName, ObjText.Text) == true)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    else if (bCheckDuplicateRecord(sTableName, sFieldName, ObjText.Text) == true)
                                    {
                                        return false;
                                    }
                                }
                                if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                                {
                                    if (ObjText.ValenmTextBoxType == SControl.enmTextBoxType.enmNumeric || ObjText.ValenmTextBoxType == SControl.enmTextBoxType.enmNumericWithDecimal)
                                    {
                                        sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + ",";
                                        sValueList[iCurrTblIndex] = sValueList[iCurrTblIndex] + ObjText.Text + ",";
                                    }
                                    else
                                    {
                                        //Megha28052012  changes for G & B region mapping master
                                        if (sMenuId == "227" && i == 5)
                                        {
                                            sFieldName = "Domestic_Export";
                                            ObjText.Text = "E";

                                            sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + ",";
                                            sValueList[iCurrTblIndex] = sValueList[iCurrTblIndex] + "'" + ObjText.Text + "',";
                                        }
                                        else
                                        {
                                            sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + ",";
                                            sValueList[iCurrTblIndex] = sValueList[iCurrTblIndex] + "'" + ObjText.Text + "',";
                                        }
                                        //Megha28052012 
                                    }


                                }
                                else if (l_ValenmFormState == clsGlobal.enmFormState.enmUpdate_State)
                                {
                                    if (ObjText.AllowTochange == true)
                                    {
                                        if (ObjText.ValenmTextBoxType == SControl.enmTextBoxType.enmNumeric || ObjText.ValenmTextBoxType == SControl.enmTextBoxType.enmNumericWithDecimal)
                                        {
                                            sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + "=" + ObjText.Text + ",";
                                        }
                                        else
                                        {
                                            //Megha28052012 changes for G & B region mapping master 
                                            if (sMenuId == "227" && i == 5)
                                            {
                                                sFieldName = "Domestic_Export";
                                                ObjText.Text = "E";

                                                sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + "='" + ObjText.Text + "',";
                                            }
                                            else
                                            {
                                                sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + "='" + ObjText.Text + "',";
                                            }
                                            //Megha28052012

                                        }
                                    }
                                }

                            }
                            #endregion
                            #region combobox  Control
                            else if (sControlName == "SComboBox")//combobox
                            {
                                ObjCombo = (SControl.SComboBox)currControl;
                                GetTableNameAndFieldName(ObjCombo.MapToTblFieldName, l_sTableUsed, ref iCurrTblIndex, ref sFieldName, ref sTableName);
                                //if (ObjCombo.AllowDuplicate == false)
                                //{
                                //    if (l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable)
                                //    {
                                //        if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                                //        {
                                //            if (bCheckDuplicateRecord(sTableName, sFieldName, ObjCombo.SelectedValue) == true)
                                //            {
                                //                return false;
                                //            }
                                //        }
                                //    }
                                //    else if (bCheckDuplicateRecord(sTableName, sFieldName, ObjCombo.SelectedValue) == true)
                                //    {
                                //        return false;
                                //    }
                                //}
                                if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                                {
                                    sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + ",";
                                    if ((ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmYesNoCombo) || (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmFixedCombo))
                                    {
                                        sValueList[iCurrTblIndex] = sValueList[iCurrTblIndex] + "'" + ObjCombo.SelectedItem.Text + "',";
                                        sBaseDeleteWhereClause = sBaseDeleteWhereClause + sFieldName + " = " + ObjCombo.SelectedItem.Text + " And ";
                                    }
                                    else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmDataCombo)
                                    {
                                        sValueList[iCurrTblIndex] = sValueList[iCurrTblIndex] + ObjCombo.SelectedValue + ",";
                                        sBaseDeleteWhereClause = sBaseDeleteWhereClause + sFieldName + " = " + ObjCombo.SelectedValue + " And ";
                                    }
                                    if (l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster)
                                    {
                                        //ObjCombo.SelectedValue = "0";
                                        // ObjCombo.SelectedIndex = "0";  
                                        iIDForFillDetailsGrid = Convert.ToInt32(ObjCombo.SelectedValue);
                                        //iIDForFillDetailsGrid = (((System.Web.UI.WebControls.DropDownList)(ObjCombo)).SelectedIndex);
                                    }
                                }
                                else if (l_ValenmFormState == clsGlobal.enmFormState.enmUpdate_State)
                                {
                                    if (ObjCombo.AllowTochange == true)
                                    {
                                        if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmYesNoCombo || ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmFixedCombo)
                                        {
                                            sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + "='" + ObjCombo.SelectedItem.Text + "',";
                                        }
                                        else if (ObjCombo.ValenmComboBoxType == SControl.enmComboBoxType.enmDataCombo)
                                        {
                                            sFieldList[iCurrTblIndex] = sFieldList[iCurrTblIndex] + sFieldName + "=" + ObjCombo.SelectedValue + ",";
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Grid Control
                            else if (sControlName == "SGrid")
                            {
                                try
                                {
                                    SControl.SGrid ObjGrid = (SControl.SGrid)currControl;
                                    CheckBox Chk;
                                    TextBox txt;
                                    string sBaseSql = "";
                                    string sBaseDeleteSql = "";
                                    string sBaseField = "";
                                    string sBaseValue = "";
                                    string sCompleteFieldName = ObjGrid.MapToTblFieldName;
                                    string sRecordStatus = "";
                                    iNoOfTableUsed = 0;
                                    if (ObjGrid.ValenmGridType == SControl.GridType.enmWithCheckBox)
                                    {
                                        sTableNameForDependable = ObjGrid.TblNameForFill.Substring(0, ObjGrid.TblNameForFill.IndexOf('.'));
                                        sIDNameForDependable = ObjGrid.TblNameForFill.Substring(ObjGrid.TblNameForFill.IndexOf('.') + 1);
                                    }
                                    if (l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster)
                                    {
                                        sBaseSql = ssql[0];
                                        sBaseField = sFieldList[0];
                                        sBaseValue = sValueList[0];
                                        sBaseDeleteSql = "DELETE  from  " + sTableName + " where ";


                                    }
                                    else if (l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable)
                                    {


                                        iNoOfTableUsed = 1;
                                        sTableName = sCompleteFieldName.Substring(0, sCompleteFieldName.IndexOf('.'));
                                        sBaseSql = "Insert Into " + sTableName;
                                        sBaseDeleteSql = "DELETE  from  " + sTableName + " where ";
                                        sBaseField = "(" + "";
                                        sBaseValue = "";

                                    }

                                    GetTableNameAndFieldName(ObjGrid.MapToTblFieldName, l_sTableUsed, ref iCurrTblIndex, ref sFieldName, ref sTableName);
                                    bDependentableFlag = false;
                                    for (int k = 0; k < ObjGrid.Rows.Count; k++)
                                    {
                                        ideleteCnt = ObjGrid.Rows.Count;
                                        // Following Working for One to many record 
                                        if ((ObjGrid.ValenmGridType == SControl.GridType.enmWithCheckBox) && (l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster))
                                        {
                                            if (ObjGrid.Rows[k].FindControl("gridChk" + k.ToString()) != null)
                                            {

                                                Chk = (CheckBox)ObjGrid.Rows[k].FindControl("gridChk" + k.ToString());
                                                txt = (TextBox)ObjGrid.Rows[k].FindControl("gridtxt" + k.ToString());
                                                if (Chk.Checked == true)
                                                {
                                                    bDependentableFlag = true;
                                                }

                                                if (Chk.GetType().Name.ToString() == "CheckBox")
                                                {
                                                    sRecordStatus = txt.Text;
                                                    if (sRecordStatus == "E")//If already Exist Record 
                                                    {
                                                        iNoOfTableUsed = iNoOfTableUsed + 1;
                                                        Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                        Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                        Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                        //ssql[iCurrTblIndex] = " DELETE from  " + sTableName + " where " + sFieldName + " = " + ObjGrid.Rows[k].Cells[2].Text;
                                                        ssql[iCurrTblIndex] = sBaseDeleteSql + sBaseDeleteWhereClause + sFieldName + " = " + ObjGrid.Rows[k].Cells[2].Text;
                                                        iCurrTblIndex = iCurrTblIndex + 1;
                                                        if (Chk.Checked == true)
                                                        {
                                                            iNoOfTableUsed = iNoOfTableUsed + 1;
                                                            Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                            Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                            Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                            ssql[iCurrTblIndex] = sBaseSql;

                                                            sFieldList[iCurrTblIndex] = sBaseField + sFieldName + ",";
                                                            sValueList[iCurrTblIndex] = sBaseValue + ObjGrid.Rows[k].Cells[2].Text;
                                                            iCurrTblIndex = iCurrTblIndex + 1;
                                                        }

                                                    }
                                                    else if (sRecordStatus == "D")//If delete have to the Record 
                                                    {
                                                        //Delete the record                                                
                                                        iNoOfTableUsed = iNoOfTableUsed + 1;
                                                        Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                        Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                        Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                        ssql[iCurrTblIndex] = sBaseDeleteSql + sBaseDeleteWhereClause + sFieldName + " = " + ObjGrid.Rows[k].Cells[2].Text;
                                                        iCurrTblIndex = iCurrTblIndex + 1;
                                                        ideleteCurrTblIndex = ideleteCurrTblIndex + 1;
                                                    }
                                                    else if (sRecordStatus == "N")//If New Record
                                                    {
                                                        //Insert New Record
                                                        if (Chk.Checked == true)
                                                        {
                                                            iNoOfTableUsed = iNoOfTableUsed + 1;
                                                            Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                            Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                            Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                            ssql[iCurrTblIndex] = sBaseSql;

                                                            sFieldList[iCurrTblIndex] = sBaseField + sFieldName + ",";
                                                            sValueList[iCurrTblIndex] = sBaseValue + ObjGrid.Rows[k].Cells[2].Text;
                                                            iCurrTblIndex = iCurrTblIndex + 1;
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                        //If Form Is Dependable and 
                                        if ((ObjGrid.ValenmGridType == SControl.GridType.enmWithCheckBox) && (l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable))
                                        {

                                            if (ObjGrid.Rows[k].FindControl("gridChk" + k.ToString()) != null)
                                            {

                                                Chk = (CheckBox)ObjGrid.Rows[k].FindControl("gridChk" + k.ToString());
                                                txt = (TextBox)ObjGrid.Rows[k].FindControl("gridtxt" + k.ToString());
                                                if (Chk.Checked == true)
                                                {
                                                    bDependentableFlag = true;
                                                }

                                                if (Chk.GetType().Name.ToString() == "CheckBox")
                                                {
                                                    sRecordStatus = txt.Text;
                                                    if (sRecordStatus == "E")//If already Exist Record 
                                                    {
                                                        iNoOfTableUsed = iNoOfTableUsed + 1;
                                                        Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                        Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                        Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                        ssql[iCurrTblIndex] = " DELETE from  " + sTableName + " where " + sFieldName + " = " + ObjGrid.Rows[k].Cells[2].Text;
                                                        // sFieldList[iCurrTblIndex] = "d";
                                                        // sValueList[iCurrTblIndex] = "d";
                                                        iCurrTblIndex = iCurrTblIndex + 1;
                                                        if (Chk.Checked == true)
                                                        {
                                                            iNoOfTableUsed = iNoOfTableUsed + 1;
                                                            Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                            Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                            Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                            ssql[iCurrTblIndex] = sBaseSql;

                                                            sFieldList[iCurrTblIndex] = sBaseField + sFieldName + ",";
                                                            sValueList[iCurrTblIndex] = sBaseValue + ObjGrid.Rows[k].Cells[2].Text;
                                                            iCurrTblIndex = iCurrTblIndex + 1;
                                                        }


                                                        //NoChange
                                                    }
                                                    else if (sRecordStatus == "D")//If delete have to the Record 
                                                    {
                                                        //Delete the record                                              
                                                        iNoOfTableUsed = iNoOfTableUsed + 1;
                                                        Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                        Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                        Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                        ssql[iCurrTblIndex] = " DELETE from  " + sTableName + " where " + sFieldName + " = " + ObjGrid.Rows[k].Cells[2].Text;
                                                        sFieldList[iCurrTblIndex] = "d";
                                                        sValueList[iCurrTblIndex] = "d";
                                                        iCurrTblIndex = iCurrTblIndex + 1;
                                                        // ideleteCurrTblIndex = ideleteCurrTblIndex + 1;

                                                    }
                                                    else if (sRecordStatus == "N")//If New Record
                                                    {
                                                        //Insert New Record
                                                        if (Chk.Checked == true)
                                                        {
                                                            iNoOfTableUsed = iNoOfTableUsed + 1;
                                                            Array.Resize(ref ssql, iCurrTblIndex + 1);
                                                            Array.Resize(ref sFieldList, iCurrTblIndex + 1);
                                                            Array.Resize(ref sValueList, iCurrTblIndex + 1);
                                                            ssql[iCurrTblIndex] = sBaseSql;

                                                            sFieldList[iCurrTblIndex] = sBaseField + sFieldName + ",";
                                                            sValueList[iCurrTblIndex] = sBaseValue + ObjGrid.Rows[k].Cells[2].Text;
                                                            iCurrTblIndex = iCurrTblIndex + 1;
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
                            #endregion
                            }
                        }
                    }

                    if ((sMenuId == "169" || sMenuId == "270" || sMenuId == "401" || sMenuId == "135") && ideleteCurrTblIndex != ideleteCnt)
                    {
                        if (bDependentableFlag == false)
                        {
                            sMessage = "Please Select record";
                            Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
                            return false;

                        }
                    }
                    // Final sql is build
                    ObjSaveMaster = new clsSaveMaster();
                    for (int i = 0; i < iNoOfTableUsed; i++)
                    {
                        if (sFieldList[i] != "")
                        {
                            if (ssql[i].Contains("DELETE") == true)
                            {
                            }
                            else
                            {
                                if (sFieldList[i] != null && sFieldList[i] != "")
                                {
                                    sFieldList[i] = sFieldList[i].Substring(0, sFieldList[i].Length - 1);
                                }
                                if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                                {
                                    if (l_ValMasterType != clsSaveMaster.MasterType.enmOneToManyMaster && l_ValMasterType != clsSaveMaster.MasterType.enmDependableTable)
                                    {
                                        if (sValueList[i] != null && sValueList[i] != "")
                                        {
                                            sValueList[i] = sValueList[i].Substring(0, sValueList[i].Length - 1);
                                        }
                                    }
                                    else
                                    {

                                        if (sValueList[i] != null && sValueList[i] != "")
                                        {
                                            if (i == 0 && l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable)//if first master table
                                            {
                                                sValueList[i] = sValueList[i].Substring(0, sValueList[i].Length - 1);
                                            }
                                            else
                                            {
                                                sValueList[i] = sValueList[i].Substring(0, sValueList[i].Length);
                                            }
                                        }

                                    }
                                    ssql[i] = ssql[i] + sFieldList[i] + " ) Values (" + sValueList[i] + ")";

                                }

                                else if (l_ValenmFormState == clsGlobal.enmFormState.enmUpdate_State)
                                {
                                    if (ssql[i].Contains("Update") == true)
                                    {
                                        sFieldList[i] = " Set " + sFieldList[i];
                                        ssql[i] = ssql[i] + sFieldList[i] + sWhereClause[i];
                                    }
                                    else if (ssql[i].Contains("DELETE") == true)
                                    {
                                    }
                                    else if (ssql[i].Contains("Insert") == true)
                                    {
                                        ssql[i] = ssql[i] + sFieldList[i] + " ) Values (" + sValueList[i] + ")";
                                    }
                                }
                            }
                        }
                    }

                    if (l_ValMasterType == clsSaveMaster.MasterType.enmEditableMaster || l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster)
                    {
                        new_ID = txtID.Text;
                        bSaveRecord = ObjSaveMaster.bSaveRecord(ssql);

                        if (sMenuId != "270")
                        {
                            // 'Replace Func.DB to objDB by Shyamal on 05042012
                            clsDB objDB = new clsDB();
                            if (l_ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                            {
                                sql = "Select Max(ID)as ID from " + sTableName;
                                try
                                {
                                    new_ID = Func.Convert.sConvertToString(objDB.ExecuteQuery(sql));
                                    //Megha02082012
                                    if (sMenuId == "141") //Finacier Master 
                                    {
                                        sql = "Update " + sTableName + " Set SAP_code=" + "'F" + new_ID + "'  where ID=" + new_ID;
                                        objDB.ExecuteQuery(sql);
                                        sql = "Update " + sTableName + " Set  type = 'Update' ";
                                        objDB.ExecuteQuery(sql);
                                    }
                                    else if (sMenuId == "632")
                                    {
                                        sql = "Update " + sTableName + " Set cr_date='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "',DealerID='" + Session["iDealerID"] + "',HOBrID ='" + Session["HOBR_ID"] + "' ,additional_bay='N' where ID=" + new_ID;
                                        objDB.ExecuteQuery(sql);
                                    }
                                    //Megha02082012
                                }
                                catch (Exception ex)
                                {
                                    objDB.RollbackTransaction();
                                    return bSaveRecord;
                                }
                                finally
                                {

                                    if (objDB != null) objDB = null;
                                }
                            }
                            else if (l_ValenmFormState == clsGlobal.enmFormState.enmUpdate_State)
                            {
                                //Megha02082012
                                if (sMenuId == "141")
                                {
                                    sql = "Update " + sTableName + " Set  type  =  'Update' ";
                                    objDB.ExecuteQuery(sql);
                                }

                                //Megha02082012

                            }
                        }
                        else
                        {

                        }

                    }
                    else if (l_ValMasterType == clsSaveMaster.MasterType.enmDependableTable)
                    {

                        bSaveRecord = ObjSaveMaster.bSaveRecordForDependable(ssql, l_sTableUsed[0], sIDNameForDependable, txtID.Text, l_ValenmFormState);
                        new_ID = txtID.Text;
                    }

                    ObjCombo = null;
                    ObjText = null;
                }
                catch (Exception ex)
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ex", "alert('" + ex.Message + "');", true);             
                    //Response.Write(ex.ToString());
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + ex.Message + ".');</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ex.Message + ".');", true);
                    bSaveRecord = false;

                }
                if (bSaveRecord == true)
                {


                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Record Saved !.');", true);
                    //Page.RegisterStartupScript("Close", "<script language='javascript'>alert('Record Saved !.');</script>");
                    if (l_ValMasterType == clsSaveMaster.MasterType.enmOneToManyMaster)
                    {
                        ////if (sMenuId != "270" || sMenuId != "401")
                        ////{
                        ////    FillDetailsGrid();
                        ////}
                        //if (sMenuId != "270" && sMenuId != "401") 
                        //{
                        //    FillDetailsGrid();
                        //}
                        if (sMenuId == "270")
                        {
                            string sMapToTblFieldName = "M_DealerwiseIncoTerms.Dealer_ID";
                            sWhereClauseForFillDetailsGrid = sMapToTblFieldName.Substring(sMapToTblFieldName.IndexOf('.') + 1);
                            FillDetailsGrid();
                        }

                        else if (sMenuId == "401")
                        {
                            //iIDForFillDetailsGrid = Func.Convert.iConvertToInt(((System.Web.UI.WebControls.DropDownList)(sender)).SelectedValue);
                            string sMapToTblFieldName = "M_Model_Dealer_Link.Model_ID";
                            sWhereClauseForFillDetailsGrid = sMapToTblFieldName.Substring(sMapToTblFieldName.IndexOf('.') + 1);
                            FillDetailsGrid();

                        }
                        else
                        {
                            FillDetailsGrid();
                        }


                    }
                    else
                    {
                        //FillSelectionGrid();
                        if (sMenuId == "169" || sMenuId == "135")
                        {

                            FillSelectionGridOnlySpecific(new_ID);
                        }
                        else
                        {
                            FillSelectionGridOnlyRegular(new_ID);
                        }
                        //FillSelectionGrid();
                    }
                }
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
                return false;
            }
        }

        // To Get TableName And FieldName from 'MaptoFieldName'
        private void GetTableNameAndFieldName(string sCompleteFieldName, string[] sAllTableNames, ref int iTableIndex, ref string sFieldName, ref  string sTblName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            sFieldName = "";
            try
            {
                sFieldName = sCompleteFieldName.Substring(sCompleteFieldName.IndexOf('.') + 1);
                sTblName = sCompleteFieldName.Substring(0, sCompleteFieldName.IndexOf('.'));
                iTableIndex = iGetTableIndex(sAllTableNames, sTblName);
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        //Get Table Index when multiple tables are used.
        private int iGetTableIndex(string[] sAllTableNames, string sTblName)
        {
            for (int i = 0; i < sAllTableNames.GetLength(0); i++)
            {
                if (sAllTableNames[i].Trim() == sTblName.Trim())
                    return i;
            }
            return 0;
        }
        #endregion

        #region Other Functions
        // evnt is used for temp dropdown control
        protected void drpTarget_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        //To Show Details 
        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            try
            {
                SControl.SButton objbutton = (SControl.SButton)sender;
                if (objbutton.ParentControlID != "" && objbutton.ParentControlID != null)
                {
                    iIDForFillDetailsGrid = Func.Convert.iConvertToInt(((System.Web.UI.WebControls.DropDownList)(ControlContainer.FindControl(objbutton.ParentControlID))).SelectedValue);
                    string sSql = objbutton.QueryToExecute + "  " + iIDForFillDetailsGrid;
                    FillDetailsGridAfterButtonClick(sSql);
                }
                else
                {
                    //
                    iIDForFillDetailsGrid = Func.Convert.iConvertToInt(txtID.Text);
                    FillDetailsGridAfterImageClick();
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        #endregion
    }
}