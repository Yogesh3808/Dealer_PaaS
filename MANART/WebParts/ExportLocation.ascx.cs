using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;

namespace MANART.WebParts
{
    public partial class ExportLocation : System.Web.UI.UserControl
    {
        #region Fields
        private int _iSupplierId;
        private int _iRegionId;
        private int _iCountryId;
        private int _iStateId;
        private int _iDealerId;
        private int _iUserTypeId;
        private int _iUserID;
        private int _iSupType;
        private string _sDealerCode = "";
        //private string _bDistributor = "N";
        private string _bDistributor = "Y";
        private string _sSupplierType = "";

        // To Hide Dealer Details   
        private bool _bHideRegionDetails = false;
        private bool _bHideCountryDetails = false;
        private bool _bHideCurrancyDetails = false;

        private bool _bUseSpareDealerCode = false;

        #endregion

        #region Properties

        public bool bHideRegionDetails
        {
            get { return _bHideRegionDetails; }
            set { _bHideRegionDetails = value; }
        }
        public String bDistributor
        {
            //get { return (ddlType.SelectedValue == "ED") ? "Y" : "N"; }
            get { return "N"; }
            set { _bDistributor = value; }
        }
        public bool bHideCountryDetails
        {
            get { return _bHideCountryDetails; }
            set { _bHideCountryDetails = value; }
        }

        public bool bHideCurrancyDetails
        {
            get { return _bHideCurrancyDetails; }
            set { _bHideCurrancyDetails = value; }
        }
        public bool bUseSpareDealerCode
        {
            get { return _bUseSpareDealerCode; }
            set { _bUseSpareDealerCode = value; }
        }

        public int iSupplierId
        {
            get
            {

                return (_iSupplierId != 0) ? _iSupplierId : (drpDealerName.SelectedValue != "") ? Func.Convert.iConvertToInt(drpDealerName.SelectedValue) : Func.Convert.iConvertToInt((Session["SupplierId"] != null) ? Session["SupplierId"] : 0);
            }
            set
            {
                _iSupplierId = value;
                drpDealerName.SelectedValue = Func.Convert.sConvertToString(value);
            }
        }
        public int iDealerId
        {
            //get
            //{

            //    return (_iDealerId != 0) ? _iDealerId : (drpDealerName.SelectedValue != "") ? Func.Convert.iConvertToInt(drpDealerName.SelectedValue) : Func.Convert.iConvertToInt((Session["DealerId"] != null) ? Session["DealerId"] : 0);
            //}
            //set
            //{
            //    _iDealerId = value;
            //    drpDealerName.SelectedValue = Func.Convert.sConvertToString(value);
            //}
            get
            {
                //_iDealerId = 0;
                //_iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);
                return (_iDealerId != 0) ? _iDealerId : Func.Convert.iConvertToInt((Session["DealerId"] != null) ? Session["DealerId"] : 0);
                //return _iDealerId;
            }
            set
            {
                _iDealerId = value;
                //Session["iDealerID"] = value;
            }
        }
        public int iUserTypeId
        {
            get
            {

                return _iUserTypeId;
            }
            set
            {
                _iUserTypeId = value;
            }
        }
        public int iUserID
        {
            get
            {

                return _iUserID;
            }
            set
            {
                _iUserID = value;
            }
        }


        public int iRegionId
        {
            get
            {

                return (_iRegionId != 0) ? _iRegionId : Func.Convert.iConvertToInt((Session["RegionId"] != null) ? Session["RegionId"] : 0);
            }
            set
            {
                _iRegionId = value;
            }
        }
        public int iCountryId
        {
            get
            {
                return (_iCountryId != 0) ? _iCountryId : Func.Convert.iConvertToInt((Session["CountryId"] != null) ? Session["CountryId"] : 0);
            }
            set
            {
                _iCountryId = value;
            }
        }
        public int iStateId
        {
            get
            {
                return (_iStateId != 0) ? _iStateId : Func.Convert.iConvertToInt((Session["StateId"] != null) ? Session["StateId"] : 0);
            }
            set
            {
                _iStateId = value;
            }
        }

        public string sDealerCode
        {
            get
            {
                return (_sDealerCode != "") ? _sDealerCode : Func.Convert.sConvertToString((txtDealerCode.Text != "") ? txtDealerCode.Text : "");
            }
            set
            {
                _sDealerCode = value;
            }
        }
        public int iSupType
        {
            get
            {

               // return _iSupType;
                return (_iSupType != 0) ? _iSupType : Func.Convert.iConvertToInt((Session["SupTypeID"] != null) ? Session["SupTypeID"] : 0);
            }
            set
            {
                _iSupType = value;
            }
        }

        public string sSupplierType
        {
            get
            {

                //return _sSupplierType;
                return (_sSupplierType != "") ? _sSupplierType : Func.Convert.sConvertToString((Session["SupplierType"] != null) ? Session["SupplierType"] : "");
            }
            set
            {
                _sSupplierType = value;
            }
        }

        #endregion Properties
        public event EventHandler DealerSelectedIndexChanged;
        public event EventHandler TypeSelectedIndexChanged;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Session["CountryId"] = null;
                Session["StateId"] = null;
                Session["RegionId"] = null;
                Session["DealerId"] = null;
                Session["SupTypeID"] = null;
                Session["SupplierType"] = null;
            }
            iUserTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                SetControlValue();
            }
        }
        public void SetControlValue()
        {
            clsUser objectUser = null;
            DataTable dtDealerDetails = null;
            try
            {
                ListItem lstitm;
                objectUser = new clsUser();
                if (Session["UserID"] == null) return;
                else
                    iUserID = Func.Convert.iConvertToInt(Session["UserID"]);
                dtDealerDetails = new DataTable();

                drpDealerName.Items.Clear();
                dtDealerDetails = objectUser.FillDealersByUserID(iUserID, bDistributor);
                if (dtDealerDetails == null)
                    return;
                else if (dtDealerDetails.Rows.Count != 0)
                {
                    // Dealer Name and Id
                    drpDealerName.DataSource = dtDealerDetails;
                    if (bUseSpareDealerCode == false)
                    {
                        drpDealerName.DataTextField = "vcode";
                    }
                    else
                    {
                        drpDealerName.DataTextField = "scode";
                    }
                    //drpDealerName.DataTextField = "Dealer_Name";
                    drpDealerName.DataValueField = "DealerID";
                    drpDealerName.DataBind();
                    lstitm = new ListItem("--Select--", "0");
                    drpDealerName.Items.Insert(0, lstitm);

                }
                //For Export dealers Users
                if (iUserTypeId == 4)
                {
                    lblCountry.Text = "Country";
                    drpDealerName.SelectedValue = drpDealerName.Items[1].Value;
                    iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                    drpDealerName.Enabled = false;
                    ddlType.Enabled = false;

                    FillLocation();
                }
                else if (iUserTypeId == 1) //For ExportVECV Users
                {
                    lblCountry.Text = "Country";
                    ddlType.Enabled = false;
                    FillLocation();

                }
                if (iUserTypeId == 3 || iUserTypeId == 6) //For Domestic dealers Users and MD Users 
                {
                    lblCountry.Text = "State";
                    ddlType.Enabled = true;
                    //if (drpDealerName.Items.Count == 2 && bDistributor == "Y")
                    //{
                    //    drpDealerName.SelectedValue = drpDealerName.Items[1].Value;
                    //    iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                    //    drpDealerName.Enabled = false;
                    //}
                    //else
                    //{
                    //    drpDealerName.Enabled = true;
                    //}
                    if (Func.Convert.iConvertToInt(Request["MenuID"].ToString()) == 646)
                    { // Part Cliam Creation
                         for (int i = 0; i < dtDealerDetails.Rows.Count; i++)
                         {
                             iSupType = Func.Convert.iConvertToInt(dtDealerDetails.Rows[i]["Sup_Type"]);
                             if (iSupType == 18)
                             {
                                 iDealerId = Func.Convert.iConvertToInt(dtDealerDetails.Rows[i]["DealerID"]);
                                 iSupplierId = iDealerId;
                                 drpDealerName.Enabled = false;
                                 drpDealerName.CssClass = "NonEditableFields";
                             }
                         }
                    }
                    else if (Func.Convert.iConvertToInt(Request["MenuID"].ToString()) == 694)
                    {
                        drpDealerName.DataSource = null;
                        DataView dv = dtDealerDetails.DefaultView;
                        //dv.RowFilter = "Sup_Type=16";
                        if (iUserTypeId != 6)
                        {
                            dv.RowFilter = "Sup_Type=16";
                        }

                        DataTable dtNew = dv.ToTable();
                        drpDealerName.DataValueField = "DealerID";
                        drpDealerName.DataSource = dtNew;
                        drpDealerName.DataBind();
                        lstitm = new ListItem("--Select--", "0");
                        drpDealerName.Items.Insert(0, lstitm);
                    }
                    FillLocation();
                }

            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objectUser != null) objectUser = null;
                if (dtDealerDetails != null) dtDealerDetails = null;
            }
        }
        public void FillLocation()
        {
            DataTable dtLocation = null;
            clsUser objectUser = null;
            try
            {
                dtLocation = new DataTable();
                objectUser = new clsUser();
                //Changed By VIkram
               // dtLocation = objectUser.GetLocDetailsByDealerID(iDealerId, bDistributor);
               dtLocation = objectUser.GetLocDetailsByDealerID(iSupplierId, bDistributor);
                if (dtLocation == null)
                    return;
                else if (dtLocation.Rows.Count != 0)
                {
                    //Changed on 06.07.16 Morning Vikram
                    //Session["DealerID"] = dtLocation.Rows[0]["DealerID"];
                    Session["DealerID"] = Session["iDealerID"].ToString();
                    iDealerId = Func.Convert.iConvertToInt(dtLocation.Rows[0]["DealerID"]);
                    txtAllDealerID.Text = Func.Convert.sConvertToString(iDealerId);

                    iSupType = Func.Convert.iConvertToInt(dtLocation.Rows[0]["Sup_Type"]);
                    sSupplierType = dtLocation.Rows[0]["SupplierType"].ToString();

                    Session["SupTypeID"] = iSupType;
                    Session["SupplierType"] = sSupplierType;

                    //Region Name And ID
                    txtRegion.Text = dtLocation.Rows[0]["Region_Name"].ToString();
                    iRegionId = Func.Convert.iConvertToInt(dtLocation.Rows[0]["RegionID"]);
                    Session["RegionID"] = iRegionId;

                    if (iUserTypeId == 1 || iUserTypeId == 4)
                    {
                        //Country Name And Id                   
                        txtCountry.Text = dtLocation.Rows[0]["Country_Name"].ToString();
                        iCountryId = Func.Convert.iConvertToInt(dtLocation.Rows[0]["CountryID"]);
                        Session["CountryID"] = iCountryId;
                    }
                    else if (iUserTypeId == 3 || iUserTypeId == 6) //Dealer MD user 
                    {
                        //State Name And Id                   
                        txtCountry.Text = dtLocation.Rows[0]["State"].ToString();
                        iStateId = Func.Convert.iConvertToInt(dtLocation.Rows[0]["StateID"]);
                        Session["StateID"] = iCountryId;
                    }

                    txtCurrency.Text = Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetCurrency, iCountryId, "");
                    if (bUseSpareDealerCode == false)
                        txtDealerCode.Text = dtLocation.Rows[0]["Vehicle_Code"].ToString();
                    else
                        txtDealerCode.Text = dtLocation.Rows[0]["Spare_Code"].ToString();
                    sDealerCode = txtDealerCode.Text;
                }
                else
                {
                    txtRegion.Text = "";
                    txtCountry.Text = "";
                    txtDealerCode.Text = "";
                    txtCurrency.Text = "";
                }

                lblDealerCode.Text = "Supplier Code:";
                LblDealerName.Text = "Supplier Name:";
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objectUser != null) objectUser = null;
                if (dtLocation != null) dtLocation = null;
            }
        }
        protected void drpDealerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iOldDealerID = 0;
            try
            {
                //iDealerId =Func.Convert.iConvertToInt( Session["iDealerID"].ToString());
               // iSupplierId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                if (iUserTypeId == 6) //For MD User
                {
                    iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                    iSupplierId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                    Session["iDealerID"] = iDealerId;
                }
                else
                {
                    iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
                    iSupplierId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                }


                iOldDealerID = iDealerId;
                FillLocation();
                if (DealerSelectedIndexChanged != null)
                {
                    //raise event               
                    DealerSelectedIndexChanged(this, e);
                }
                if (iOldDealerID != iDealerId)
                    FillLocation();
            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iOldDealerID = 0;
            try
            {
                if (TypeSelectedIndexChanged != null)
                {
                    //raise event               
                    TypeSelectedIndexChanged(this, e);
                }
                SetControlValue();

            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }

        }

    }
}