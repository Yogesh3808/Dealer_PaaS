using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;

namespace MANART.WebParts
{
    public partial class MANPendingDoc : System.Web.UI.UserControl
    {
        private string _sFormID = "";
        private string _sDealerID = "";
        private int _iDealerID;
        private int _iPDocID;
        private string _sDoc = "";

        public string sFormID
        {
            get
            {

                return _sFormID;
            }
            set
            {
                _sFormID = value;
            }
        }

        public int PDocID
        {
            get
            {

                return (_iPDocID != 0) ? _iPDocID : Func.Convert.iConvertToInt((Session["PDocID"] != null) ? Session["PDocID"] : 0); ;
            }
            set
            {
                _iPDocID = value;
            }
        }

        public int iDealerID
        {
            get
            {

                return _iDealerID;
            }
            set
            {
                _iDealerID = value;
            }
        }
        public string sDealerID
        {
            get
            {

                return _sDealerID;
            }
            set
            {
                _sDealerID = value;
            }
        }
        public string sDoc
        {
            get
            {

                return (_sDoc != "") ? _sDoc : Func.Convert.sConvertToString((Session["PsDoc"] != null) ? Session["PsDoc"] : ""); ;
            }
            set
            {
                _sDoc = value;
            }
        }
        public string sCollapsedText
        {
            get
            {

                return CPEPendDocList.CollapsedText;
            }
            set
            {
                CPEPendDocList.CollapsedText = value;
            }
        }

        public string sExpandedText
        {
            get
            {

                return CPEPendDocList.ExpandedText;
            }
            set
            {
                CPEPendDocList.ExpandedText = value;
            }
        }

        public string sPendDocText
        {
            get
            {

                return lblTtlPendDocList.Text;
            }
            set
            {
                lblTtlPendDocList.Text = value;
            }
        }
        public event EventHandler DocumentGridRowCommand;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Session["PDocID"] = null;
                Session["PsDoc"] = null;

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                if (Session["PDocID"] != null) Session["PDocID"] = null;

                if (Session["PsDoc"] != null) Session["PsDoc"] = null;

                BindDataToGrid();
            }        
        }
        public void BindDataToGrid()
        {
            DataTable dtDealerDetails = null;
            clsUser objectUser = null;
            DataTable dtNewRecord = null;
            int iUserID;
            int iUserRole;
            int iUserSQHResourceId;
            int iMenuId;
            int iUserPCRHeadApprID;
            try
            {
                dtNewRecord = new DataTable();
                objectUser = new clsUser();
                string sDocName = "";
                string sGetRecordFor = "";
              
                sGetRecordFor = sGetRecordText(ref sDocName);
                if (Session["UserID"] == null) return;
                else
                    iUserID = Func.Convert.iConvertToInt(Session["UserID"]);
                if (sDealerID == "")
                {
                    dtDealerDetails = new DataTable();
                    dtDealerDetails = objectUser.FillDealersByUserIDMTI(iUserID, "");
                    if (dtDealerDetails != null)
                        if (dtDealerDetails.Rows.Count > 0)
                            for (int iCnt = 0; iCnt < dtDealerDetails.Rows.Count; iCnt++)
                            {
                                if (iCnt == 0)
                                    sDealerID = Func.Convert.sConvertToString(dtDealerDetails.Rows[iCnt]["DealerID"]);
                                else
                                    sDealerID = sDealerID + "," + Func.Convert.sConvertToString(dtDealerDetails.Rows[iCnt]["DealerID"]);
                            }
                }

                if (Session["UserRole"] == null) return;
                else
                    iUserRole = Func.Convert.iConvertToInt(Session["UserRole"]);

                if (sGetRecordFor == "POApproval" || sGetRecordFor == "OrderAppDD" || sGetRecordFor == "OrderAppDM" || sGetRecordFor == "RFPApp") 
                {
                    iUserSQHResourceId = Func.Convert.iConvertToInt(Func.Convert.dConvertToDouble(Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetUserVehPOApprv, 0, "")));
                    iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);

                    if (iUserRole != 2 && sGetRecordFor == "POApproval")
                    {
                        iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Common.sGetMultiUserAccess(Func.Convert.iConvertToInt(Session["UserID"]), iMenuId));                        
                        iUserRole = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 9 : iUserRole;

                    }

                    else if (iUserRole != 3 && (sGetRecordFor == "OrderAppDD" || sGetRecordFor == "OrderAppDM"))
                    {
                        iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Common.sGetMultiUserAccess(Func.Convert.iConvertToInt(Session["UserID"]), iMenuId));
                        iUserRole = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 9 : iUserRole;

                    }
                    else if (sGetRecordFor == "RFPApp")
                    {
                        iUserPCRHeadApprID = Func.Convert.iConvertToInt(Func.Common.sGetMultiUserAccess(Func.Convert.iConvertToInt(Session["UserID"]), iMenuId));
                        iUserRole = (Func.Convert.iConvertToInt(Session["UserID"]) == iUserPCRHeadApprID) ? 9 : iUserRole;
                    }
                   


                }
                else if (sGetRecordFor == "ActivityRequest")
                {
                    iUserRole = Func.Convert.iConvertToInt(Session["DepartmentID"]);


                }


                dtNewRecord = Func.Common.GetRecordOnOpenClick(sGetRecordFor, sDealerID, iUserRole);

                dtNewRecord = Func.Common.GetRecordOnOpenClick(sGetRecordFor, sDealerID, iUserRole);

                if (Func.Common.iRowCntOfTable(dtNewRecord) == 0)
                {
                    lblTitle.Style.Add("display", "");
                    lblTitle.Text = sDocName + " Records Does Not Exist For Selection Criteria!";
                    dtNewRecord = null;
                    DocumentGrid.DataSource = dtNewRecord;
                    DocumentGrid.DataBind();
                    //HideColumn();
                    return;
                }
                else
                {
                    lblTitle.Style.Add("display", "none");
                    lblTitle.Text = "";
                }

                DocumentGrid.DataSource = dtNewRecord;
                DocumentGrid.DataBind();
                HideColumn();
                dtNewRecord = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objectUser != null) objectUser = null;
                if (dtDealerDetails != null) dtDealerDetails = null;
                if (dtNewRecord != null) dtNewRecord = null;
            }

        }

        private string sGetRecordText(ref string sDocName)
        {

            string sGetRecordFor = "";

            switch (sFormID)
            {
                case "1":
                    {
                        // To Get Vehicle RFP To Create Proforma
                        sGetRecordFor = "VRFP"; sDocName = "Vehicle RFP";
                        sExpandedText = "Hide Pending Vehicle RFP details";
                        sCollapsedText = "Show Pending Vehicle RFP details";
                        sPendDocText = "Pending Vehicle RFP details";
                        DocumentGrid.Columns[5].HeaderText = "RFP No.";
                        DocumentGrid.Columns[6].HeaderText = "RFP Date";
                        //lblTitle.Text = "Vehicle RFP Selection To Create Proforma";
                        break;
                    }
                case "2":
                    {
                        // To Get Spares RFP To Create Proforma
                        sGetRecordFor = "SRFP"; sDocName = "Spares RFP";
                        sExpandedText = "Hide Pending Spares RFP details";
                        sCollapsedText = "Show Pending Spares RFP details";
                        sPendDocText = "Pending Spares RFP details";
                        DocumentGrid.Columns[5].HeaderText = "RFP No.";
                        DocumentGrid.Columns[6].HeaderText = "RFP Date";
                        //lblTitle.Text = "Spares RFP Selection To Create Proforma";
                        break;
                    }
                case "3":
                    {
                        // To Get Vehicle Proforma To Create ORF
                        sGetRecordFor = "VProforma"; sDocName = "Vehicle Proforma";
                        sExpandedText = "Hide Pending Vehicle Proforma details";
                        sCollapsedText = "Show Pending Vehicle Proforma details";
                        sPendDocText = "Pending Vehicle Proforma details";
                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Vehicle Proforma Selection To Create ORF.";
                        ////lblNote.Text = "Note: Please Check 'LC Details' are entered,those Proforma will be available for selection!.";
                        //lblNote.Text = "Note: Proforma will be available for selection only if 'Advance Payment' is done";
                        //lblNote.Visible = true;
                        break;
                    }

                case "4":
                    {
                        // To Get Spares Proforma To Create ORF
                        sGetRecordFor = "SProforma"; sDocName = "Spares Proforma";
                        sExpandedText = "Hide Pending Spares Proforma details";
                        sCollapsedText = "Show Pending Spares Proforma details";
                        sPendDocText = "Pending Vehicle Spares details";
                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Spares Proforma Selection To Create ORF.";
                        ////lblNote.Text = "Note: Please Check 'LC Details' are entered,those Proforma will be available for selection!";
                        //lblNote.Text = "Note: Proforma will be available for selection only if 'Advance Payment' is done";
                        //lblNote.Visible = true;
                        break;
                    }
                case "5":
                    {
                        // To Get Vehicle Indent for Preshipmet
                        sGetRecordFor = "VIndent"; sDocName = "Vehicle Indent";
                        sExpandedText = "Hide Pending Vehicle Indent details";
                        sCollapsedText = "Show Pending Vehicle Indent details";
                        sPendDocText = "Pending Vehicle Indent details";
                        DocumentGrid.Columns[5].HeaderText = "Indent No.";
                        DocumentGrid.Columns[6].HeaderText = "Indent Date";
                        //lblTitle.Text = "Vehicle Indent Selection To Create PreShipment Invoice.";
                        //lblNote.Text = "Check for: Indent Created,Payment details(LC/Advance) entered,SAP details Received for the Indent!";
                        //lblNote.Font.Size = 11;
                        //lblNote.ForeColor = Color.White;
                        //lblNote.Visible = true;
                        //HdnStatus.Value = sGetRecordFor;
                        break;
                    }
                case "6":
                    {
                        //// To Get Spares Indent for Preshipmet
                        //sGetRecordFor = "SIndent"; sDocName = "Spares Indent";
                        //DocumentGrid.Columns[5].HeaderText = "Indent No.";
                        //DocumentGrid.Columns[6].HeaderText = "Indent Date";
                        //lblTitle.Text = "Spares Indent Selection To Create PreShipment Invoice.";
                        //lblNote.Text = "Check for: Indent Created,Payment details(LC/Advance) entered,SAP details Received for the Indent!";
                        //lblNote.Font.Size = 11;
                        //lblNote.ForeColor = Color.White;
                        //lblNote.Visible = true;
                        //HdnStatus.Value = sGetRecordFor;
                        //break;
                        sGetRecordFor = "ExPreshipS"; sDocName = "Existing Preshipment";

                        sExpandedText = "Hide Pending Existing Preshipment details";
                        sCollapsedText = "Show Pending Existing Preshipment details";
                        sPendDocText = "Pending Existing Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = "Existing Preshipment Selection To Create Preshipment Invoice.";
                        //lblNote.Text = "Note: Preshipment Invoice create from existing Preshipment.";
                        //lblNote.Visible = true;
                        break;
                    }
                case "7":
                    {
                        // To Get Preshipment For Packing Slip
                        sGetRecordFor = "VPRS_PCK_Slip"; sDocName = "Vehicle Preshipment";

                        sExpandedText = "Hide Pending Vehicle Preshipment details";
                        sCollapsedText = "Show Pending Vehicle Preshipment details";
                        sPendDocText = "Pending Vehicle Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = "Vehicle Preshipment Invoice Selection To Create Packing List.";
                        break;
                    }
                case "8":
                    {
                        // To Get Preshipment For Packing Slip
                        sGetRecordFor = "SPRS_PCK_Slip"; sDocName = "Spares Preshipment";

                        sExpandedText = "Hide Pending Spares Preshipment details";
                        sCollapsedText = "Show Pending Spares Preshipment details";
                        sPendDocText = "Pending Spares Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = "Spares Preshipment Invoice Selection To Create Packing List.";
                        break;
                    }
                case "9":
                    {
                        // To Get Preshipment For Bill of Lading
                        //Sujata 25022011          
                        //sGetRecordFor = "VBillofLadding"; 
                        sGetRecordFor = Func.Convert.sConvertToString(Session["MenuType"]);
                        //Sujata 25022011                    
                        sDocName = "Vehicle Preshipment";

                        sExpandedText = "Hide Pending Vehicle Preshipment details";
                        sCollapsedText = "Show Pending Vehicle Preshipment details";
                        sPendDocText = "Pending Vehicle Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //////Sujata 25022011     
                        //////lblTitle.Text = " Preshipment Invoice Selection To Create Bill Of Lading/Truck Receipt";
                        ////if (sGetRecordFor == "VIBillofLadding")
                        ////{
                        ////    lblTitle.Text = " Preshipment Invoice Selection To Create Truck Receipt";
                        ////}
                        ////else
                        ////{
                        ////    lblTitle.Text = " Preshipment Invoice Selection To Create Bill Of Lading";
                        ////}
                        //////Sujata 25022011     
                        break;
                    }
                case "10":
                    {
                        // To Get Preshipment For Bill of Lading
                        //Sujata 25022011     
                        //sGetRecordFor = "SBillofLadding";
                        sGetRecordFor = Func.Convert.sConvertToString(Session["MenuType"]);
                        //Sujata 25022011     
                        sDocName = "Spares Preshipment";

                        sExpandedText = "Hide Pending Spares Preshipment details";
                        sCollapsedText = "Show Pending Spares Preshipment details";
                        sPendDocText = "Pending Spares Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //////Sujata 25022011  
                        //////lblTitle.Text = "Spares Preshipment Invoice Selection To Create Bill Of Lading/Truck Receipt";
                        ////if (sGetRecordFor == "SIBillofLadding")
                        ////{
                        ////    lblTitle.Text = "Spares Preshipment Invoice Selection To Create Truck Receipt";
                        ////}
                        ////else if (sGetRecordFor == "SSBillofLadding")
                        ////{
                        ////    lblTitle.Text = "Spares Preshipment Invoice Selection To Create Bill Of Lading";
                        ////}
                        ////else
                        ////{
                        ////    lblTitle.Text = "Spares Preshipment Invoice Selection To Create Airway Bill";
                        ////}
                        //////Sujata 25022011  
                        break;
                    }
                case "11":
                    {
                        // To Get Preshipment For Delivery Challan 
                        sGetRecordFor = "VPRS_Challan"; sDocName = "Vehicle Preshipment";

                        sExpandedText = "Hide Pending Vehicle Preshipment details";
                        sCollapsedText = "Show Pending Vehicle Preshipment details";
                        sPendDocText = "Pending Vehicle Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = "Vehicle Preshipment Invoice Invoice Selection To Create Delivery Challan.";
                        //lblNote.Text = "Note: Delivery Challan will be made for Which Mode of Dispatch Is 'By Road'!.";
                        //lblNote.Visible = true;
                        break;
                    }
                case "12":
                    {
                        // To Get Preshipment For Delivery Challan 
                        sGetRecordFor = "SPRS_Challan"; sDocName = "Spares Preshipment";

                        sExpandedText = "Hide Pending Spares Preshipment details";
                        sCollapsedText = "Show Pending Spares Preshipment details";
                        sPendDocText = "Pending Spares Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = "Spares Preshipment Invoice Selection To Create Preshipment Delivery Challan.";
                        //lblNote.Text = "Note: Delivery Challan will be made for Which Mode of Dispatch Is 'By Road'!.";
                        //lblNote.Visible = true;
                        break;
                    }
                case "13":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "VORF"; sDocName = "Vehicle ORF";

                        sExpandedText = "Hide Pending Vehicle ORF details";
                        sCollapsedText = "Show Pending Vehicle ORF details";
                        sPendDocText = "Pending Vehicle ORF details";

                        DocumentGrid.Columns[5].HeaderText = "ORF No.";
                        DocumentGrid.Columns[6].HeaderText = "ORF Date";
                        //lblTitle.Text = "Vehicle ORF Selection To Create Indent.";
                        break;
                    }
                case "14":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "SORF"; sDocName = "Spares ORF";

                        sExpandedText = "Hide Pending Spares ORF details";
                        sCollapsedText = "Show Pending Spares ORF details";
                        sPendDocText = "Pending Spares ORF details";

                        DocumentGrid.Columns[5].HeaderText = "ORF No.";
                        DocumentGrid.Columns[6].HeaderText = "ORF Date";
                        //lblTitle.Text = "Spares ORF Selection To Create Indent.";
                        break;
                    }
                case "15":
                    {
                        // To Get Preshipment To Postshipment
                        sGetRecordFor = "VPostshipment"; sDocName = "Vehicle Preshipment";

                        sExpandedText = "Hide Pending Vehicle Preshipment details";
                        sCollapsedText = "Show Pending Vehicle Preshipment details";
                        sPendDocText = "Pending Vehicle Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = " Preshipment Invoice Selection To Create Postshipment Invoice";
                        //lblNote.Text = "Check for : Original LC details entered";
                        //lblNote.Font.Size = 11;
                        //lblNote.ForeColor = Color.White;
                        //lblNote.Visible = true;
                        //HdnStatus.Value = sGetRecordFor;
                        break;
                    }
                case "16":
                    {
                        // To Get Preshipment To Postshipment
                        sGetRecordFor = "SPostshipment"; sDocName = "Spares Preshipment";

                        sExpandedText = "Hide Pending Spares Preshipment details";
                        sCollapsedText = "Show Pending Spares Preshipment details";
                        sPendDocText = "Pending Spares Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        //lblTitle.Text = " Preshipment Invoice Selection To Create Postshipment Invoice";
                        //lblNote.Text = "Check for : Original LC details entered";
                        //lblNote.Font.Size = 11;
                        //lblNote.ForeColor = Color.White;
                        //lblNote.Visible = true;
                        //HdnStatus.Value = sGetRecordFor;
                        break;
                    }
                case "17":
                    {
                        // To Get postshipment To Create Packing Slip            
                        sGetRecordFor = "VPOS_PCK_Slip"; sDocName = "Vehicle Postshipment";

                        sExpandedText = "Hide Pending Vehicle Postshipment details";
                        sCollapsedText = "Show Pending Vehicle Postshipment details";
                        sPendDocText = "Pending Vehicle Postshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Postshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Postshipment Date";
                        //lblTitle.Text = "Vehicle Postshipment Invoice Selection To Create Packing List.";
                        //lblNote.Visible = true;
                        break;
                    }
                case "18":
                    {
                        // To Get postshipment To Create Packing Slip            
                        sGetRecordFor = "SPOS_PCK_Slip"; sDocName = "Spares Postshipment";

                        sExpandedText = "Hide Pending Spares Postshipment details";
                        sCollapsedText = "Show Pending Spares Postshipment details";
                        sPendDocText = "Pending Spares Postshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Postshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Postshipment Date";
                        //lblTitle.Text = "Spares Postshipment Invoice Selection To Create Packing List.";
                        break;
                    }

                case "19":
                    {
                        // To Get PostShipment For Delivery Challan 
                        sGetRecordFor = "VPOS_Challan"; sDocName = "Vehicle PostShipment";

                        sExpandedText = "Hide Pending Vehicle Postshipment details";
                        sCollapsedText = "Show Pending Vehicle Postshipment details";
                        sPendDocText = "Pending Vehicle Postshipment details";

                        DocumentGrid.Columns[5].HeaderText = "PostShipment No.";
                        DocumentGrid.Columns[6].HeaderText = "PostShipment Date";
                        //lblTitle.Text = "Vehicle PostShipment Invoice Selection To Create PostShipment Delivery Challan.";
                        break;
                    }
                case "20":
                    {
                        // To Get PostShipment For Delivery Challan 
                        sGetRecordFor = "SPOS_Challan"; sDocName = "Spares PostShipment";

                        sExpandedText = "Hide Pending Spares Postshipment details";
                        sCollapsedText = "Show Pending Spares Postshipment details";
                        sPendDocText = "Pending Spares Postshipment details";

                        DocumentGrid.Columns[5].HeaderText = "PostShipment No.";
                        DocumentGrid.Columns[6].HeaderText = "PostShipment Date";
                        //lblTitle.Text = "Spares PostShipment Invoice Selection To Create PostShipment Delivery Challan.";
                        break;
                    }
                case "21":
                    {
                        //To Get Proforma For Dummy LC 
                        sGetRecordFor = "VPInvDummyLC"; sDocName = "Vehicle Proforma Invoice";

                        sExpandedText = "Hide Pending Vehicle Proforma Invoice details";
                        sCollapsedText = "Show Pending Vehicle Proforma Invoice details";
                        sPendDocText = "Pending Vehicle Proforma Invoice details";

                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Vehicle Proforma Invoice Selection To Create Dummy LC.";
                        break;
                    }
                case "22":
                    {
                        //To Get Proforma For Dummy LC 
                        sGetRecordFor = "SPInvDummyLC"; sDocName = "Spares Proforma Invoice";

                        sExpandedText = "Hide Pending Spares Proforma Invoice details";
                        sCollapsedText = "Show Pending Spares Proforma Invoice details";
                        sPendDocText = "Pending Spares Proforma Invoice details";


                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Spares Proforma Invoice Selection To Create Dummy LC.";
                        break;
                    }
                case "23":
                    {
                        //To Get Proforma For Original LC 
                        sGetRecordFor = "VPInvLC"; sDocName = "Vehicle Proforma Invoice";

                        sExpandedText = "Hide Pending Vehicle Proforma Invoice details";
                        sCollapsedText = "Show Pending Vehicle Proforma Invoice details";
                        sPendDocText = "Pending Vehicle Proforma Invoice details";

                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Vehicle Proforma Invoice Selection To Create LC.";
                        break;
                    }
                case "24":
                    {
                        //To Get Proforma For Original LC 
                        sGetRecordFor = "SPInvLC"; sDocName = "Spares Proforma Invoice";

                        sExpandedText = "Hide Pending Spares Proforma Invoice details";
                        sCollapsedText = "Show Pending Spares Proforma Invoice details";
                        sPendDocText = "Pending Spares Proforma Invoice details";

                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Spares Proforma Invoice Selection To Create LC.";
                        break;
                    }
                case "25":
                    {
                        //To Get Proforma For Advance Payment
                        sGetRecordFor = "VPInvADV"; sDocName = "Vehicle Proforma Invoice";

                        sExpandedText = "Hide Pending Vehicle Proforma Invoice details";
                        sCollapsedText = "Show Pending Vehicle Proforma Invoice details";
                        sPendDocText = "Pending Vehicle Proforma Invoice details";

                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Vehicle Proforma Invoice Selection To Create Advance Payment.";
                        break;
                    }
                case "26":
                    {
                        //To Get Proforma For Advance Payment
                        sGetRecordFor = "SPInvADV"; sDocName = "Spares Proforma Invoice";

                        sExpandedText = "Hide Pending Spares Proforma Invoice details";
                        sCollapsedText = "Show Pending Spares Proforma Invoice details";
                        sPendDocText = "Pending Spares Proforma Invoice details";

                        DocumentGrid.Columns[5].HeaderText = "Proforma Invoice No.";
                        DocumentGrid.Columns[6].HeaderText = "Proforma Invoice Date";
                        //lblTitle.Text = "Spares Proforma Invoice Selection To Create Advance Payment.";
                        break;
                    }
                case "27":
                    {
                        sGetRecordFor = "VExPreship"; sDocName = "Existing Preshipment";

                        sExpandedText = "Hide Pending Existing Preshipment details";
                        sCollapsedText = "Show Pending Existing Preshipment details";
                        sPendDocText = "Pending Existing Preshipment details";

                        DocumentGrid.Columns[5].HeaderText = "Preshipment No.";
                        DocumentGrid.Columns[6].HeaderText = "Preshipment Date";
                        ////lblTitle.Text = "Existing Preshipment Selection To Create Preshipment Invoice.";
                        //lblNote.Text = "Note: Preshipment Invoice create from existing Preshipment.";
                        //lblNote.Visible = true;
                        break;
                    }
                case "41":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "VPLNT"; sDocName = "Vehicle Indent For Plant Details";

                        sExpandedText = "Hide Pending Vehicle Indent details";
                        sCollapsedText = "Show Pending Vehicle Indent details";
                        sPendDocText = "Pending Vehicle Indent details";

                        DocumentGrid.Columns[5].HeaderText = "Indent No.";
                        DocumentGrid.Columns[6].HeaderText = "Indent Date";
                        //lblTitle.Text = "Vehicle Indent Selection To Create Plant Details.";
                        break;
                    }
                case "42":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "SPLNT"; sDocName = "Spares Indent For Plant Details";

                        sExpandedText = "Hide Pending Spares Indent details";
                        sCollapsedText = "Show Pending Spares Indent details";
                        sPendDocText = "Pending Spares Indent details";

                        DocumentGrid.Columns[5].HeaderText = "Indent No.";
                        DocumentGrid.Columns[6].HeaderText = "Indent Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Plant Details.";
                        break;
                    }
                case "43":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "VORDEX"; sDocName = "Vehicle Indent For Export Details";

                        sExpandedText = "Hide Pending Vehicle Indent details";
                        sCollapsedText = "Show Pending Vehicle Indent details";
                        sPendDocText = "Pending Vehicle Indent details";


                        DocumentGrid.Columns[5].HeaderText = "Indent No.";
                        DocumentGrid.Columns[6].HeaderText = "Indent Date";
                        //lblTitle.Text = "Vehicle Indent Selection To Create Export Details.";
                        break;
                    }
                case "44":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "SORDEX"; sDocName = "Spares Indent For Export Details";

                        sExpandedText = "Hide Pending Spares Indent details";
                        sCollapsedText = "Show Pending Spares Indent details";
                        sPendDocText = "Pending Spares Indent details";

                        DocumentGrid.Columns[5].HeaderText = "Indent No.";
                        DocumentGrid.Columns[6].HeaderText = "Indent Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "45":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M1"; sDocName = "M0 Details";

                        sExpandedText = "Pending M0 details";
                        sCollapsedText = "Pending M0 details";
                        sPendDocText = "Pending M0 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                      

                        DocumentGrid.Columns[5].HeaderText = "M0 No.";
                        DocumentGrid.Columns[6].HeaderText = "M0 Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
               

                case "46":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M2"; sDocName = "M1 Details";

                        sExpandedText = "Pending M1 details";
                        sCollapsedText = "Pending M1 details";
                        sPendDocText = "Pending M1 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";

                        DocumentGrid.Columns[5].HeaderText = "Enquiry Details";
                        DocumentGrid.Columns[6].HeaderText = "RFP Details";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "47":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "DiscountApproval"; sDocName = "M2 Details";

                        sExpandedText = "Pending For Discount Approval Details";
                        sCollapsedText = "Pending For Discount Approval Details";
                        sPendDocText = "Pending for Discount Approval Details List";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M2 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }

                case "48":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M3"; sDocName = "M2 Details";

                        sExpandedText = "Pending M2 details";
                        sCollapsedText = "Pending M2 details";
                        sPendDocText = "Pending M2 details";


                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M2 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }


                case "49":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M4"; sDocName = "M3 Details";

                        sExpandedText = "Pending M3 details";
                        sCollapsedText = "Pending M3 details";
                        sPendDocText = "Pending M3 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M3 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "50":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M5"; sDocName = "M4 Details";

                        sExpandedText = "Pending M4 details";
                        sCollapsedText = "Pending M4 details";
                        sPendDocText = "Pending M4 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M4 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }

                case "51":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M6"; sDocName = "M5 Details";

                        sExpandedText = "Pending M5 details";
                        sCollapsedText = "Pending M5 details";
                        sPendDocText = "Pending M5 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M5 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "52":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M7"; sDocName = "Doc Details";

                        sExpandedText = "Pending Doc details";
                        sCollapsedText = "Pending Doc details";
                        sPendDocText = "Pending Doc details";


                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M3 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }

                case "53":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "POApproval"; sDocName = "Doc Details";

                        sExpandedText = "Pending Dealer PO details";
                        sCollapsedText = "Pending Dealer PO details";
                        sPendDocText = "Pending Dealer PO details";

                        DocumentGrid.Columns[5].HeaderText = "Doc No.";
                        DocumentGrid.Columns[6].HeaderText = "Doc Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "54":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M8"; sDocName = "Doc Details";

                        sExpandedText = "Pending Doc details";
                        sCollapsedText = "Pending Doc details";
                        sPendDocText = "Pending Doc details";
                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Doc Details";
                        DocumentGrid.Columns[5].HeaderText = "Customer";
                        DocumentGrid.Columns[6].HeaderText = "Chassis No";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "55":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "OrderAppDD"; sDocName = "Doc Details";

                        sExpandedText = "Pending Direct Billing Order Form details";
                        sCollapsedText = "Pending Direct Billing Order Form details";
                        sPendDocText = "Pending Direct Billing Order Form details";

                        DocumentGrid.Columns[5].HeaderText = "Doc No.";
                        DocumentGrid.Columns[6].HeaderText = "Doc Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }

                case "56":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "OrderAppDM"; sDocName = "Doc Details";

                        sExpandedText = "Pending Direct Billing Order Form details";
                        sCollapsedText = "Pending Direct Billing Order Form details";
                        sPendDocText = "Pending Direct Billing Order Form details";

                        DocumentGrid.Columns[5].HeaderText = "Doc No.";
                        DocumentGrid.Columns[6].HeaderText = "Doc Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }


                case "57":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "LossApproval"; sDocName = "Doc Details";

                        sExpandedText = "Pending for Loss Approval";
                        sCollapsedText = "Pending  for Loss Approval";
                        sPendDocText = "Pending for  Loss Approval";
                        //DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "Doc Details";
                        DocumentGrid.Columns[6].HeaderText = "Doc Type";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        //DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        //DocumentGrid.Columns[4].HeaderText = "Doc Details";
                        //DocumentGrid.Columns[5].HeaderText = "Customer";
                        //DocumentGrid.Columns[6].HeaderText = "Chassis No";


                        break;
                    }
                case "58":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M0"; sDocName = "Call Ticket Details";

                        sExpandedText = "Pending Call Ticket Details";
                        sCollapsedText = "Pending Call Ticket Details";
                        sPendDocText = "Pending Call Ticket Details";
                        DocumentGrid.Columns[4].HeaderText = "Call Sub Type";
                        DocumentGrid.Columns[5].HeaderText = "Customer";
                        DocumentGrid.Columns[6].HeaderText = "Ticket Details";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "59":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M2MTI"; sDocName = "M1 Details";

                        sExpandedText = "Pending M1 details";
                        sCollapsedText = "Pending M1 details";
                        sPendDocText = "Pending M1 details";
                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "Enquiry No.";
                        DocumentGrid.Columns[6].HeaderText = "M1 Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "60":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M3MTI"; sDocName = "M2 Details";

                        sExpandedText = "Pending M2 details";
                        sCollapsedText = "Pending M2 details";
                        sPendDocText = "Pending M2 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M2 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "61":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M4MTI"; sDocName = "M3 Details";

                        sExpandedText = "Pending M3 details";
                        sCollapsedText = "Pending M3 details";
                        sPendDocText = "Pending M3 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M3 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "62":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M5MTI"; sDocName = "M4 Details";

                        sExpandedText = "Pending M4 details";
                        sCollapsedText = "Pending M4 details";
                        sPendDocText = "Pending M4 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M4 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "63":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M6MTI"; sDocName = "M5 Details";

                        sExpandedText = "Pending M5 details";
                        sCollapsedText = "Pending M5 details";
                        sPendDocText = "Pending M5 details";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M5 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "64":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M7MTI"; sDocName = "Doc Details";

                        sExpandedText = "Pending Doc details";
                        sCollapsedText = "Pending Doc details";
                        sPendDocText = "Pending Doc details";



                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M3 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "65":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "M1MTI"; sDocName = "M0 Details";

                        sExpandedText = "Pending M0 details";
                        sCollapsedText = "Pending M0 details";
                        sPendDocText = "Pending M0 details";
                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";

                        DocumentGrid.Columns[5].HeaderText = "M0 No.";
                        DocumentGrid.Columns[6].HeaderText = "M0 Date";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
                case "66":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "ProformaInv"; sDocName = "M2 Details";

                        sExpandedText = "Pending For Proforma Invoice";
                        sCollapsedText = "Pending For Proforma Invoice";
                        sPendDocText = "Pending for Proforma Invoice List";

                        DocumentGrid.Columns[2].HeaderText = "Dealer Details";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        DocumentGrid.Columns[5].HeaderText = "M2 Details.";
                        DocumentGrid.Columns[6].HeaderText = "Enquiry No.";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }

                case "67":
                    {
                        sGetRecordFor = "RFPApp"; sDocName = "RFP Details";
                        sExpandedText = "Pending For RFP Approval";
                        sCollapsedText = "Pending For RFP Approval";
                        sPendDocText = "Pending For RFP Approval";
                        DocumentGrid.Columns[4].HeaderText = "Customer";
                        
                        break;
                    }
                case "68":
                    {
                        sGetRecordFor = "TicketMngApp"; sDocName = "Ticket Management Details";
                        sExpandedText = "Pending For Ticket Management  Approval";
                        sCollapsedText = "Pending For Ticket Management  Approval";
                        sPendDocText = "Pending For Ticket Management  Approval";
                        DocumentGrid.Columns[6].HeaderText = "Ticket Type";

                        break;
                    }
                case "69":
                    {
                        // To Get ORF for Indent Creation
                        sGetRecordFor = "ActivityRequest"; sDocName = "Activity Request Details";

                        sExpandedText = "Pending Activity Request Details";
                        sCollapsedText = "Pending Activity Request Details";
                        sPendDocText = "Pending Activity Request Details";
                        //DocumentGrid.Columns[4].HeaderText = "Call Sub Type";
                        //DocumentGrid.Columns[5].HeaderText = "Customer";
                        //DocumentGrid.Columns[6].HeaderText = "Ticket Details";
                        //lblTitle.Text = "Spares Indent Selection To Create Export Details.";
                        break;
                    }
               
            }
            return sGetRecordFor;
        }
        protected void DocumentGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DocumentGrid.PageIndex = e.NewPageIndex;
            BindDataToGrid();
        }
        protected void DocumentGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string[] splstr = e.CommandArgument.ToString().Split(',');
                if (splstr.Length > 1)
                {
                    PDocID = Func.Convert.iConvertToInt(splstr[0]);
                    iDealerID = Func.Convert.iConvertToInt(splstr[1]);
                    sDoc=Func.Convert.sConvertToString(splstr[2]);
                    hdnPDocID.Value = Func.Convert.sConvertToString(PDocID);
                    hdnPDocNo.Value = Func.Convert.sConvertToString(sDoc);
                    Session["PDocID"] = PDocID;
                    Session["PsDoc"] = sDoc;
                    
                    //raise event               
                    DocumentGridRowCommand(this, e);
                }

            }

        }
        private void HideColumn()
        {
            //  To Hide the ID and RecordUsed Colummn       

            if (sFormID == "1" || sFormID == "3" || sFormID == "5" || sFormID == "7" || sFormID == "9" || sFormID == "11" || sFormID == "13" || sFormID == "15" || sFormID == "17" || sFormID == "19" || sFormID == "21" || sFormID == "23" || sFormID == "25" || sFormID == "27" || sFormID == "41" || sFormID == "43")
            {
                DocumentGrid.HeaderRow.Cells[7].Style.Add("display", "none");
            }
            else
            {
                DocumentGrid.HeaderRow.Cells[7].Style.Add("display", "");
            }

            for (int i = 0; i < DocumentGrid.Rows.Count; i++)
            {
                if (sFormID == "1" || sFormID == "3" || sFormID == "5" || sFormID == "7" || sFormID == "9" || sFormID == "11" || sFormID == "13" || sFormID == "15" || sFormID == "17" || sFormID == "19" || sFormID == "21" || sFormID == "23" || sFormID == "25" || sFormID == "27" || sFormID == "41" || sFormID == "43")
                {
                    DocumentGrid.Rows[i].Cells[7].Style.Add("display", "none");
                }
                else
                {
                    DocumentGrid.Rows[i].Cells[7].Style.Add("display", "");
                }
            }
        }
    }
}