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

namespace MANART.WebParts
{
    public partial class LocationRes : System.Web.UI.UserControl
    {
        public LocationRes()
        {

        }

        #region Fields
        private int _iRegionId;
        private int _iCountryId;
        private int _iDealerId;
        private int _iStateId;
        private int _iUsreTypeId;
        private string _sDealerCode;
        private DataSet DSDealer;
        private int _sUserID;
        //To Use and show Dealer Spares Code 
        private bool _bUseSpareDealerCode = false;

        // To Hide Dealer Details
        private bool _bHideDealerDetails = false;
        private bool _bHideCountryDetails = false;
        DataTable dtDealerDetails = new DataTable();
        #endregion

        #region Properties


        public bool bHideDealerDetails
        {
            get { return _bHideDealerDetails; }
            set { _bHideDealerDetails = value; }
        }
        public bool bHideCountryDetails
        {
            get { return _bHideCountryDetails; }
            set { _bHideCountryDetails = value; }
        }

        public bool bUseSpareDealerCode
        {
            get { return _bUseSpareDealerCode; }
            set { _bUseSpareDealerCode = value; }
        }

        public string sDealerCode
        {
            get
            {
                _sDealerCode = txtDealerCode.Text;
                return _sDealerCode;
            }
            set
            {
                txtDealerCode.Text = value;
                _sDealerCode = value;
            }
        }
        public int iRegionId
        {
            get
            {
                _iRegionId = Func.Convert.iConvertToInt(drpRegion.SelectedValue.ToString());
                return _iRegionId;
            }

        }
        public int iCountryId
        {
            get
            {
                _iCountryId = Func.Convert.iConvertToInt(drpCountry.SelectedValue.ToString());
                return _iCountryId;
            }
            set
            {
                _iCountryId = value;
                drpCountry.SelectedValue = Func.Convert.sConvertToString(value);
            }
        }


        public int iDealerId
        {
            get
            {
                //_iDealerId = BLL.Func.Convert.iConvertToInt(drpDealerName.SelectedValue.ToString());
                _iDealerId = 0;
                _iDealerId = Func.Convert.iConvertToInt(Session["iDealerID"]);

                return _iDealerId;
            }
            set
            {
                _iDealerId = value;
                Session["iDealerID"] = value;
                try
                {
                    drpDealerName.SelectedValue = Func.Convert.sConvertToString(value);
                }
                catch
                { }
            }
        }
        public int iStateId
        {
            get
            {
                _iStateId = Func.Convert.iConvertToInt(drpCountry.SelectedValue.ToString());

                return _iStateId;
            }

        }

        public bool EnableRegion
        {
            get
            {
                return drpRegion.Enabled;
            }
            set
            {
                if (value == true)
                {
                    if (drpRegion.Items.Count == 2)
                    {
                        drpRegion.Enabled = false;
                        drpRegion.Items[1].Selected = true;
                    }
                    else
                    {
                        drpRegion.Enabled = value;
                    }
                }
                else
                {
                    drpRegion.Enabled = false;
                }
            }
        }
        public bool EnableCountry
        {
            get
            {
                return drpCountry.Enabled;
            }
            set
            {
                if (value == true)
                {
                    if (drpCountry.Items.Count == 2)
                    {
                        drpCountry.Enabled = false;
                        drpCountry.Items[1].Selected = true;
                    }
                    else
                    {
                        drpCountry.Enabled = value;
                    }
                }
                else
                {
                    drpCountry.Enabled = false;
                }
            }
        }
        public bool EnableDealer
        {
            get
            {
                //return drpDealerName.Enabled;
                return true;
            }
            set
            {
                ///drpDealerName.Enabled = value;

            }
        }
        #endregion

        /*   User Type      1   VE	VECV Export 
     * 2	VD	VECV Domestic
                        3	DD	Dealer Domestic
                        4	DE	Dealer Export
                        5	DDA	Dealer Domestic Administator
                        6	DEA	Dealer Domestic Administrator
                        7	Administrator	Web Site Administator */

        public event EventHandler DDLSelectedIndexChanged;
        public event EventHandler drpRegionIndexChanged;
        public event EventHandler drpCountryIndexChanged;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                _sUserID = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                if (!IsPostBack)
                {

                    if (Request.QueryString["UserType"] != null)
                    {
                        _iUsreTypeId = Func.Convert.iConvertToInt(Request.QueryString["UserType"].ToString());
                        _sUserID = 0;
                        _iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                    }
                    else
                    {
                        _iDealerId = 0;
                        iDealerId = 0;
                    }
                    txtCurrency.Enabled = true;
                    txtDealerCode.Enabled = true;
                    SetControlValue();
                }
                Session["DealerID"] = drpDealerName.SelectedValue;
                if (_bHideDealerDetails == true)
                {
                    //LocationDetails.Rows[1].Visible = false;
                    LocationDetails.Visible = false;
                }
                if (_bHideCountryDetails == true)
                {
                    drpCountry.Visible = false;
                    lblCountry.Visible = false;
                }
                else
                {
                    drpCountry.Visible = true;
                    lblCountry.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDealerComboCountryOrStateWise();
            if (drpCountryIndexChanged != null)
            {
                drpCountryIndexChanged(this, e);
            }
        }

        private void FillDealerComboCountryOrStateWise()
        {
            try
            {
                _iCountryId = Func.Convert.iConvertToInt(drpCountry.SelectedValue);
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                drpDealerName.ToolTip = "--Select-- To Show Records Of All Dealer In New Form.";
                if (_iCountryId == 0)
                {
                    drpDealerName.Items.Clear();
                    drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpDealerName.SelectedValue = "0";
                    txtDealerCode.Text = "";
                    iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                    return;
                }

                if (_iUsreTypeId == 2)//For Domestic VECV Users
                {
                    //Func.Common.BindDataToCombo(drpDealerName, clsCommon.ComboQueryType.DealerForState, _iCountryId);  

                    clsUser objUser = new clsUser();
                    DataSet dsState = new DataSet();
                    drpDealerName.Items.Clear();
                    drpDealerName.SelectedValue = null;
                    dsState = objUser.FillDealerForState(_sUserID, iRegionId, iCountryId);
                    if (dsState != null)
                        if (dsState.Tables[dsState.Tables.Count - 1].Rows.Count > 0)
                        {
                            //Megha08072011
                            if (bUseSpareDealerCode == true)
                            {

                                dtDealerDetails = dsState.Tables[dsState.Tables.Count - 1];
                                DataView dvDealerDetail = new DataView();
                                dvDealerDetail = dsState.Tables[dsState.Tables.Count - 1].DefaultView;
                                //dvDealerDetail.RowFilter = "SNAME= 'NULL'";
                                dvDealerDetail.RowFilter = "SNAME LIKE '%(1S%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                drpDealerName.DataSource = dtDealerDetails;
                                drpDealerName.DataTextField = "SName";
                                drpDealerName.DataValueField = "ID";
                                drpDealerName.DataBind();
                                //  drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                            }
                            else
                            {

                                dtDealerDetails = dsState.Tables[dsState.Tables.Count - 1];
                                DataView dvDealerDetail = new DataView();
                                dvDealerDetail = dsState.Tables[dsState.Tables.Count - 1].DefaultView;
                                //dvDealerDetail.RowFilter = "VNAME =  '";
                                dvDealerDetail.RowFilter = "VNAME LIKE '%(1V%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                drpDealerName.DataSource = dtDealerDetails;
                                drpDealerName.DataTextField = "VName";
                                drpDealerName.DataValueField = "ID";
                                drpDealerName.DataBind();
                            }
                            //Megha08072011

                            //drpDealerName.DataSource = dsState.Tables[dsState.Tables.Count - 1];
                            //drpDealerName.DataTextField = "Name";
                            //drpDealerName.DataValueField = "ID";
                            //drpDealerName.DataBind();

                        }
                    drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
                    //sCurrencyName = Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetCurrency, 1, ""); //For domestic Country ID =1                                    
                }
                else if (_iUsreTypeId == 1)//For Export VECV Users
                {
                    //Func.Common.BindDataToCombo(drpDealerName, clsCommon.ComboQueryType.DealerForCountry, _iCountryId);
                    clsUser objUser = new clsUser();
                    DataSet dsState = new DataSet();
                    drpDealerName.Items.Clear();
                    dsState = objUser.FillDealerForCountry(_sUserID, iRegionId, iCountryId);
                    if (dsState != null)
                        if (dsState.Tables[dsState.Tables.Count - 1].Rows.Count > 0)
                        {
                            //Megha08072011
                            if (bUseSpareDealerCode == true)
                            {
                                dtDealerDetails = dsState.Tables[dsState.Tables.Count - 1];
                                DataView dvDealerDetail = new DataView();
                                dvDealerDetail = dsState.Tables[dsState.Tables.Count - 1].DefaultView;
                                //dvDealerDetail.RowFilter = "SNAME!= 'NULL'";
                                dvDealerDetail.RowFilter = "SNAME LIKE '%(1XS%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                drpDealerName.DataSource = dtDealerDetails;
                                drpDealerName.DataTextField = "SName";
                                drpDealerName.DataValueField = "ID";
                                drpDealerName.DataBind();
                            }
                            else
                            {

                                dtDealerDetails = dsState.Tables[dsState.Tables.Count - 1];
                                DataView dvDealerDetail = new DataView();
                                dvDealerDetail = dsState.Tables[dsState.Tables.Count - 1].DefaultView;
                                // dvDealerDetail.RowFilter = "VNAME !=  'NULL'";
                                dvDealerDetail.RowFilter = "VNAME LIKE '%(1XV%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                drpDealerName.DataSource = dtDealerDetails;
                                drpDealerName.DataTextField = "VName";
                                drpDealerName.DataValueField = "ID";
                                drpDealerName.DataBind();
                            }
                            //Megha08072011

                            //    drpDealerName.DataSource = dsState.Tables[dsState.Tables.Count - 1];
                            //    drpDealerName.DataTextField = "Name";
                            //    drpDealerName.DataValueField = "ID";
                            //    drpDealerName.DataBind();
                        }
                    drpDealerName.Items.Insert(0, new ListItem("--Select--", "0"));

                    txtCurrency.Text = Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetCurrency, _iCountryId, "");
                }
                drpDealerName.Enabled = true;
                if (drpDealerName.Items.Count == 2)
                {
                    drpDealerName.Enabled = false;
                    drpDealerName.Items[1].Selected = true;
                    txtCurrency.Enabled = false;
                    txtDealerCode.Enabled = false;
                    iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                }
                else
                {
                    txtCurrency.Enabled = true;
                    txtDealerCode.Enabled = true;
                    iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                }
                if (drpDealerName.SelectedValue != "0")
                    FillDealerCode();
                else
                    txtDealerCode.Text = "";
                GetAllDealerIDs();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCountryOrStateComboRegionWise();
            if (drpRegionIndexChanged != null)
            {
                drpRegionIndexChanged(this, e);
            }
        }

        private void FillCountryOrStateComboRegionWise()
        {
            try
            {
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                _sUserID = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                int iRegionId = Func.Convert.iConvertToInt(drpRegion.SelectedValue);
                if (iRegionId == 0) return;
                drpCountry.Enabled = true;
                if (_iUsreTypeId == 2)//For Domestic VECV Users
                {
                    Func.Common.BindDataToCombo(drpCountry, clsCommon.ComboQueryType.StateUserWise, 0, " AND M_State.ID in(Select distinct StateID from M_Sys_UserPermissions where IsActive=1 And RegionID=" + iRegionId + " And UserID=" + _sUserID + ")");
                }
                else if (_iUsreTypeId == 3)//For Domestic  Dealer
                {
                    Func.Common.BindDataToCombo(drpCountry, clsCommon.ComboQueryType.StateUserWise, 0, " And M_State.ID in(Select distinct StateID from M_Sys_UserPermissions where IsActive=1 And RegionID=" + iRegionId + " And UserID=" + _sUserID + ")");
                }
                else
                {
                    Func.Common.BindDataToCombo(drpCountry, clsCommon.ComboQueryType.CountryUserWise, 0, " And M_Country.ID  in (Select distinct CountryId from M_Sys_UserPermissions where IsActive=1 And RegionID=" + iRegionId + " And UserID=" + _sUserID + ")");
                }
                if (drpCountry.Items.Count == 2)
                {
                    drpCountry.Enabled = false;
                    drpCountry.Items[1].Selected = true;
                    txtCurrency.Enabled = false;
                    txtDealerCode.Enabled = false;
                }
                else
                {
                    txtCurrency.Enabled = true;
                    txtDealerCode.Enabled = true;
                }
                FillDealerComboCountryOrStateWise();

            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void drpDealerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["DealerID"] = 0;
                iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
                FillDealerCode();
                GetAllDealerIDs();
                if (DDLSelectedIndexChanged != null)
                {
                    //raise event               
                    DDLSelectedIndexChanged(this, e);
                }

            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillDealerCode()
        {
            string sDealerCode = "";
            int iDealerId = Func.Convert.iConvertToInt(drpDealerName.SelectedValue);
            Session["DealerID"] = iDealerId;
            if (iDealerId == 0) return;
            if (bUseSpareDealerCode == true)
            {
                sDealerCode = Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetSpareDealerCode, iDealerId, "");
            }
            else
            {
                sDealerCode = Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetVehicleDealerCode, iDealerId, "");
            }
            txtDealerCode.Text = sDealerCode;
        }

        //To get Dealer Id of the all record
        protected void GetAllDealerIDs()
        {
            string sDealerID = "";
            if (drpDealerName.Items != null)
            {
                if (drpDealerName.SelectedValue == "0")
                {
                    for (int i = 1; i < drpDealerName.Items.Count; i++)
                    {
                        sDealerID = sDealerID + drpDealerName.Items[i].Value + ",";
                    }
                    if (sDealerID != "")
                    {
                        sDealerID = sDealerID.Substring(0, sDealerID.Length - 1);
                    }
                }
                else
                {
                    sDealerID = drpDealerName.SelectedValue;
                }
            }

            txtAllDealerID.Text = sDealerID;
        }

        public void SetControlValue()
        {
            try
            {
                ListItem lstitm;
                clsUser objectUser = new clsUser();
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                if (Session["UserID"] == null) return;
                if (_sUserID == 0)
                {
                    _sUserID = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                }
                drpRegion.Items.Clear();
                drpCountry.Items.Clear();
                drpDealerName.Items.Clear();

                //For Export dealers Users
                if (_iUsreTypeId == 4)
                {
                    drpRegion.Enabled = false;
                    drpCountry.Enabled = false;
                    //drpDealerName.Enabled = false;
                    txtCurrency.Enabled = false;
                    txtDealerCode.Enabled = false;

                    DSDealer = objectUser.DealerDetails(_sUserID.ToString(), _iDealerId, _iUsreTypeId);

                    // Dealer Name and Id
                    Session["DealerID"] = DSDealer.Tables[0].Rows[0]["DealerID"];
                    iDealerId = Func.Convert.iConvertToInt(DSDealer.Tables[0].Rows[0]["DealerID"]);
                    lstitm = new ListItem(DSDealer.Tables[0].Rows[0]["Dealer_Name"].ToString(), DSDealer.Tables[0].Rows[0]["DealerID"].ToString());
                    drpDealerName.Items.Add(lstitm);
                    drpDealerName.Enabled = false;

                    //Region Name And ID
                    lstitm = new ListItem(DSDealer.Tables[0].Rows[0]["Region_Name"].ToString(), DSDealer.Tables[0].Rows[0]["RegionID"].ToString());
                    drpRegion.Items.Add(lstitm);
                    drpRegion.Items[0].Selected = true;

                    //Country Name And Id
                    lstitm = new ListItem(DSDealer.Tables[0].Rows[0]["Country_Name"].ToString(), DSDealer.Tables[0].Rows[0]["CountryID"].ToString());
                    drpCountry.Items.Add(lstitm);
                    drpCountry.Items[0].Selected = true;

                    if (_bUseSpareDealerCode == true)
                    {
                        txtDealerCode.Text = DSDealer.Tables[0].Rows[0]["Dealer_Spares_Code"].ToString();
                    }
                    else
                    {
                        txtDealerCode.Text = DSDealer.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
                    }
                    sDealerCode = txtDealerCode.Text;
                    txtCurrency.Text = DSDealer.Tables[0].Rows[0]["Currency"].ToString();
                    //Sujata 16022011
                    lblDealerCode.Text = "Distributor Code:";
                    LblDealerName.Text = "Distributor Name:";
                    //Sujata 16022011

                }
                //else if (_iUsreTypeId == 3)//For Domestic Dealers
                else if (_iUsreTypeId == 3 || _iUsreTypeId == 7)//For Domestic Dealers
                {
                    //drpDealerName.Enabled = false;
                    DSDealer = objectUser.DealerDetails(_sUserID.ToString(), _iDealerId, _iUsreTypeId);

                    // Dealer Name and Id
                    Session["DealerID"] = DSDealer.Tables[0].Rows[0]["DealerID"];
                    iDealerId = Func.Convert.iConvertToInt(DSDealer.Tables[0].Rows[0]["DealerID"]);
                    lstitm = new ListItem(DSDealer.Tables[0].Rows[0]["Dealer_Name"].ToString(), DSDealer.Tables[0].Rows[0]["DealerID"].ToString());
                    drpDealerName.Items.Add(lstitm);
                    drpDealerName.Enabled = false;

                    //Region Name And ID
                    lstitm = new ListItem(DSDealer.Tables[0].Rows[0]["Region_Name"].ToString(), DSDealer.Tables[0].Rows[0]["RegionID"].ToString());
                    drpRegion.Items.Add(lstitm);
                    drpRegion.Enabled = false;

                    //Country Name And Id
                    lstitm = new ListItem(DSDealer.Tables[0].Rows[0]["Country_Name"].ToString(), DSDealer.Tables[0].Rows[0]["CountryID"].ToString());
                    drpCountry.Items.Add(lstitm);
                    drpCountry.Enabled = false;
                    txtCurrency.Enabled = false;
                    txtDealerCode.Enabled = false;
                    if (_bUseSpareDealerCode == true)
                    {
                        txtDealerCode.Text = DSDealer.Tables[0].Rows[0]["Dealer_Spares_Code"].ToString();
                    }
                    else
                    {
                        txtDealerCode.Text = DSDealer.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();//DSDealer.Tables[0].Rows[0]["Dealer_Vehicle_Code"].ToString();
                    }
                    sDealerCode = txtDealerCode.Text;
                    txtCurrency.Text = "INR";
                    lblCountry.Text = "State";
                    //Sujata 16022011
                    txtCurrency.Visible = false;
                    LblCurrency.Visible = false;
                    lblDealerCode.Text = "Dealer Code:";
                    LblDealerName.Text = "Dealer Name:";
                    //Sujata 16022011
                }
                else if (_iUsreTypeId == 2 || _iUsreTypeId == 1) //For VECV Users
                {
                    if (_iUsreTypeId == 2) //For Domestic VECV Users
                    {
                        Func.Common.BindDataToCombo(drpRegion, clsCommon.ComboQueryType.RegionUserWise, 0, " And M_Region.ID  in(select distinct(RegionId) from M_Sys_UserPermissions where userid =" + _sUserID + " And IsActive=1)");
                        lblCountry.Text = "State";
                        txtCurrency.Text = "INR";
                        //Sujata 16022011
                        lblDealerCode.Text = "Dealer Code:";
                        LblDealerName.Text = "Dealer Name:";
                        txtCurrency.Visible = false;
                        LblCurrency.Visible = false;
                        //Sujata 16022011
                    }
                    else if (_iUsreTypeId == 1)  //For Export VECV Users
                    {
                        Func.Common.BindDataToCombo(drpRegion, clsCommon.ComboQueryType.RegionUserWise, 0, " And M_Region.ID  in(select distinct(RegionId) from M_Sys_UserPermissions where userid =" + _sUserID + " And IsActive=1)");
                        //Sujata 16022011
                        lblDealerCode.Text = "Distributor Code:";
                        LblDealerName.Text = "Distributor Name:";
                        //Sujata 16022011

                    }
                    if (drpRegion.Items.Count == 2)
                    {
                        drpRegion.Enabled = false;
                        drpRegion.Items[1].Selected = true;
                        txtCurrency.Enabled = false;
                        txtDealerCode.Enabled = false;
                    }
                    else
                    {
                        txtCurrency.Enabled = true;
                        txtDealerCode.Enabled = true;
                    }
                    FillCountryOrStateComboRegionWise();
                }

            }
            catch (Exception ex)
            {

                Func.Common.ProcessUnhandledException(ex);
            }
        }
    }
}