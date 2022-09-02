//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using MANART_BAL;
//using MANART_DAL;
using System;
using System.Collections.Generic;
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


namespace MANART.WebParts
{
    public partial class MTILocation : System.Web.UI.UserControl
    {
        #region Fields
        private int _iRegionId;
        private int _iCountryId;
        private int _iStateId;
        private int _iDealerId;
        private int _iUserTypeId;
        private int _iUserID;
        private string _sDealerCode = "";
        private string _bDistributor = "Y";

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
            get { return (ddlType.SelectedValue == "ED") ? "Y" : "N"; }
            set { _bDistributor = value; }
            //get { return "N"; }
            //set { _bDistributor = value; }
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
                return _iDealerId;
            }
            set
            {
                _iDealerId = value;
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
            //get
            //{
            //    return (_sDealerCode != "") ? _sDealerCode : Func.Convert.sConvertToString((txtDealerCode.Text != "") ? txtDealerCode.Text : "");
            //}
            //set
            //{
            //    _sDealerCode = value;
            //}
            get
            {
                return (_sDealerCode != "") ? _sDealerCode : Func.Convert.sConvertToString((txtDealerCode.Text != "") ? txtDealerCode.Text : "");
            }
            set
            {
                _sDealerCode = value;
            }
        }

        #endregion Properties
        public event EventHandler DealerSelectedIndexChanged;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Session["CountryId"] = null;
                Session["StateId"] = null;
                Session["RegionId"] = null;
                Session["DealerId"] = null;
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
                dtDealerDetails = objectUser.FillDealersByUserIDMTI(iUserID, bDistributor);
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
                else if (iUserTypeId == 1) //For MTI Users
                {
                    lblCountry.Text = "Country";
                    ddlType.Enabled = false;
                    FillLocation();

                }
                else if (iUserTypeId == 2) //For ExportVECV Users
                {
                    lblCountry.Text = "State";
                    ddlType.Enabled = false;
                    FillLocation();

                }
                if (iUserTypeId == 7) //For EGP dealers Users
                {
                    lblCountry.Text = "State";
                    ddlType.Enabled = true;
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

                if (iDealerId == 99999)
                {
                    Session["DealerID"] = "99999";
                    iDealerId = 99999;
                    drpDealerName.SelectedValue = Func.Convert.sConvertToString(iDealerId);


                    txtAllDealerID.Text = Func.Convert.sConvertToString(iDealerId);

                    //Region Name And ID
                    txtRegion.Text = "All";
                    iRegionId = 0;
                    Session["RegionID"] = 0;


                    //State Name And Id                   
                    txtCountry.Text = "All";
                    iStateId = 0;
                    Session["StateID"] = 0;



                    txtCurrency.Text = "";

                    txtDealerCode.Text = "D099999";

                    sDealerCode = "D099999";
                }

                else
                {
                    dtLocation = objectUser.GetLocDetailsByDealerID(iDealerId, bDistributor);
                    if (dtLocation == null)
                        return;
                    else if (dtLocation.Rows.Count != 0)
                    {

                        Session["DealerID"] = dtLocation.Rows[0]["DealerID"];
                        iDealerId = Func.Convert.iConvertToInt(dtLocation.Rows[0]["DealerID"]);
                        drpDealerName.SelectedValue = Func.Convert.sConvertToString(iDealerId);


                        txtAllDealerID.Text = Func.Convert.sConvertToString(iDealerId);

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
                        else if (iUserTypeId == 2)
                        {
                            //State Name And Id                   
                            txtCountry.Text = dtLocation.Rows[0]["State"].ToString();
                            iStateId = Func.Convert.iConvertToInt(dtLocation.Rows[0]["StateID"]);
                            Session["StateID"] = iStateId;
                        }
                        else if (iUserTypeId == 7)
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
                }
                lblDealerCode.Text = "Dealer Code:";
                LblDealerName.Text = "Dealer Name:";

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

                iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                //iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"].ToString());
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

                SetControlValue();
            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }

        }

    }
}