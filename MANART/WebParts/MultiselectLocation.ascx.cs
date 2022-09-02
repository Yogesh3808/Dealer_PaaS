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
    public partial class MultiselectLocation : System.Web.UI.UserControl
    {
        #region Fields
        private int _iRegionId;
        private int _iCountryId;
        private string _sDealerId;
        private int _iStateId;
        private int _iUsreTypeId;
        private int _sUserID;
        //To Use and show Dealer Spares Code 
        private bool _bUseSpareDealerCode = false;

        #region Properties
        public bool bUseSpareDealerCode
        {
            get { return _bUseSpareDealerCode; }
            set { _bUseSpareDealerCode = value; }
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
        public string sDealerId
        {
            get
            {
                string sId = "";
                for (int k = 0; k < ChkDealer.Items.Count; k++)
                {
                    if (ChkDealer.Items[k].Selected)
                    {
                        sId = sId + ChkDealer.Items[k].Value + ",";
                    }
                }
                if (sId.Length != 0)
                {
                    sId = sId.Substring(0, sId.Length - 1);
                    _sDealerId = sId;
                }
                else
                {
                    _sDealerId = "";
                }
                return _sDealerId;
            }
            set
            {
                _sDealerId = value;
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
                drpRegion.Enabled = value;
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
                drpCountry.Enabled = value;
            }
        }
        #endregion

        #endregion

        /*   User Type      1   VE	VECV Export 
     * 2	VD	VECV Domestic
                        3	DD	Dealer Domestic
                        4	DE	Dealer Export
                        5	DDA	Dealer Domestic Administator
                        6	DEA	Dealer Domestic Administrator
                        7	Administrator	Web Site Administator */

        private string sControlClientID = "";
        DataTable dtDealerDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sControlClientID = "ContentPlaceHolder1_" + this.ID;
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                _sUserID = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                if (!IsPostBack)
                {
                    if (Request.QueryString["UserType"] != null)
                    {
                        _iUsreTypeId = Func.Convert.iConvertToInt(Request.QueryString["UserType"].ToString());
                        _sUserID = 0;
                    }
                    else
                    {


                    }


                    SetControlValue();
                }

                txtDealerName.Attributes.Add("onclick", "SHMulSel('" + sControlClientID + "', event)");
                lnkMain.Attributes.Add("onclick", "javascript:return SHMulSel01('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");
                //txtDealerName.Attributes.Add("onmouseover", "javascript:return SHMulSel01('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");
                //divMain.Attributes.Add("onmouseout", "javascript:return SHMulSel01('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");
                //onmouseover ="return CheckValidDataBeforeSave(this);" onmouseout="RemoveToolBarName(this);"
                //imgDropDown.Attributes.Add("onclick", "javascript:return SHMulSel01('" + sControlClientID + "'," + 55 + "," + 50 + ", event)");
                // Set Dealer Name in Textbox
                txtDealerName.Text = "";
                for (int k = 0; k < ChkDealer.Items.Count; k++)
                {
                    if (ChkDealer.Items[k].Selected == true)
                    {


                        txtDealerName.Text = txtDealerName.Text + ChkDealer.Items[k].Text + ",";
                    }
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }
        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDealerCountryOrStateWiseCombo();
            txtDealerName.Text = "--Select--";
        }
        private void FillDealerCountryOrStateWiseCombo()
        {
            try
            {
                //sujata 23022011
                _iRegionId = Func.Convert.iConvertToInt(drpRegion.SelectedValue);
                //sujata 23022011
                _iCountryId = Func.Convert.iConvertToInt(drpCountry.SelectedValue);
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                if (_iUsreTypeId == 2)//For Domestic VECV Users
                {
                    //sujata 23022011
                    //_iUsreTypeId =1 for Domestic and 2 Export
                    //Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, _iCountryId, "or " + _iCountryId + "=-1) And (Dealer_Region_ID=" + _iRegionId + " or " + _iRegionId + "=-1))");

                    //if (_iRegionId == -1 && _iCountryId == -1)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Origin='D' and Dealer_Region_ID !=0 and Dealer_State_ID !=0");
                    //}
                    //else if (_iRegionId == -1 && _iCountryId != -1 && _iCountryId != 0)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Region_ID !=0 and Dealer_State_ID !=0 And (Dealer_State_ID=" + _iCountryId + ") and Dealer_Origin='D'");
                    //}
                    //else if (_iRegionId != -1 && _iCountryId != -1 && _iRegionId != 0)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Region_ID !=0 and Dealer_State_ID !=0  And (Dealer_State_ID=" + _iCountryId + ") And (Dealer_Region_ID=" + _iRegionId + ") and Dealer_Origin='D'");
                    //}
                    //else if (_iRegionId != -1 && _iCountryId == -1 && _iRegionId != 0)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Region_ID !=0 and Dealer_State_ID !=0  And (Dealer_Region_ID=" + _iRegionId + ") and Dealer_Origin='D'");
                    //}
                    //sujata 23022011
                    clsUser objUser = new clsUser();
                    DataSet dsState = new DataSet();

                    dsState = objUser.FillDealerForState(_sUserID, _iRegionId, _iCountryId);
                    ChkDealer.Items.Clear();
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
                                //dvDealerDetail.RowFilter = "SNAME LIKE '%(1S%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                ChkDealer.DataSource = dtDealerDetails;
                                ChkDealer.DataTextField = "SName";
                                ChkDealer.DataValueField = "ID";
                                ChkDealer.DataBind();
                            }
                            else
                            {

                                dtDealerDetails = dsState.Tables[dsState.Tables.Count - 1];
                                DataView dvDealerDetail = new DataView();
                                dvDealerDetail = dsState.Tables[dsState.Tables.Count - 1].DefaultView;
                                // dvDealerDetail.RowFilter = "VNAME !=  'NULL'";
                                dvDealerDetail.RowFilter = "VNAME LIKE '%(1V%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                ChkDealer.DataSource = dtDealerDetails;
                                ChkDealer.DataTextField = "VName";
                                ChkDealer.DataValueField = "ID";
                                ChkDealer.DataBind();
                            }
                            //Megha08072011

                            //ChkDealer.DataSource = dsState.Tables[dsState.Tables.Count - 1];
                            //ChkDealer.DataTextField = "Name";
                            //ChkDealer.DataValueField = "ID";
                            //ChkDealer.DataBind();
                        }
                    txtCurrency.Text = "INR";
                }
                else if (_iUsreTypeId == 1)//For Export VECV Users
                {
                    //sujata 23022011
                    //Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfCountry, _iCountryId, "or " + _iCountryId + "=-1)");
                    //if (_iRegionId == -1 && _iCountryId == -1)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Origin='E' and Dealer_Region_ID !=0 and Dealer_Country_ID !=0");
                    //}
                    //else if (_iRegionId == -1 && _iCountryId != -1 && _iCountryId != 0)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Region_ID !=0 and Dealer_Country_ID  !=0 And (Dealer_Country_ID =" + _iCountryId + ") and Dealer_Origin='E'");
                    //}
                    //else if (_iRegionId != -1 && _iCountryId != -1 && _iRegionId != 0)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Region_ID !=0 and Dealer_Country_ID  !=0  And (Dealer_Country_ID =" + _iCountryId + ") And (Dealer_Region_ID=" + _iRegionId + ") and Dealer_Origin='E'");
                    //}
                    //else if (_iRegionId != -1 && _iCountryId == -1 && _iRegionId != 0)
                    //{
                    //    Func.Common.FillCheckBoxList(ChkDealer, clsCommon.ComboQueryType.AllDealerOfState, 0, " and Dealer_Region_ID !=0 and Dealer_Country_ID  !=0  And (Dealer_Region_ID=" + _iRegionId + ") and Dealer_Origin='E'");
                    //}
                    //sujata 23022011

                    clsUser objUser = new clsUser();
                    DataSet dsState = new DataSet();

                    dsState = objUser.FillDealerForCountry(_sUserID, _iRegionId, _iCountryId);
                    ChkDealer.Items.Clear();
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
                                //Sujata 11012018 For display MTI Export dealer name into approval list
                                //dvDealerDetail.RowFilter = "SNAME LIKE '%(1XS%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                ChkDealer.DataSource = dtDealerDetails;
                                ChkDealer.DataTextField = "SName";
                                ChkDealer.DataValueField = "ID";
                                ChkDealer.DataBind();
                            }
                            else
                            {

                                dtDealerDetails = dsState.Tables[dsState.Tables.Count - 1];
                                DataView dvDealerDetail = new DataView();
                                dvDealerDetail = dsState.Tables[dsState.Tables.Count - 1].DefaultView;
                                // dvDealerDetail.RowFilter = "VNAME !=  'NULL'";
                                //Sujata 11012018 For display MTI Export dealer name into approval list
                                //dvDealerDetail.RowFilter = "VNAME LIKE '%(1XV%'";
                                dtDealerDetails = dvDealerDetail.ToTable();
                                ChkDealer.DataSource = dtDealerDetails;
                                ChkDealer.DataTextField = "VName";
                                ChkDealer.DataValueField = "ID";
                                ChkDealer.DataBind();
                            }
                            //Megha08072011

                            //ChkDealer.DataSource = dsState.Tables[dsState.Tables.Count - 1];
                            //ChkDealer.DataTextField = "Name";
                            //ChkDealer.DataValueField = "ID";
                            //ChkDealer.DataBind();
                        }

                    txtCurrency.Text = Func.Common.sGetDataFromDataQuery(clsCommon.GetDataQueryType.ToGetCurrency, _iCountryId, "");

                }
                sControlClientID = "ContentPlaceHolder1_" + this.ID;
                txtControl_ID.Value = sControlClientID;
                //ListItem lstAll= new ListItem("--Select All--","0");
                //lstAll.Attributes.Add("onclick", "SelectAll()");
                //ChkDealer.Items.Add(lstAll);

                // ChkAll.Attributes.Add("onclick", "SelectAll(this,'" + sControlClientID + "')");

                for (int k = 0; k < ChkDealer.Items.Count; k++)
                {
                    ChkDealer.Items[k].Attributes.Add("onclick", "SCIT(this,'" + sControlClientID + "')");
                    ChkDealer.Items[k].Attributes.Add("alt", ChkDealer.Items[k].Value);
                    ChkDealer.Items[k].Attributes.Add("runat", "server");
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }

        protected void drpRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCountryOrStateCombo();
            txtDealerName.Text = "--Select--";
        }

        private void FillCountryOrStateCombo()
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
                    Func.Common.BindDataToCombo(drpCountry, clsCommon.ComboQueryType.StateUserWise, 0, " AND M_State.ID in(Select distinct StateID from M_Sys_UserPermissions where IsActive=1 And (RegionID=" + iRegionId + " or " + iRegionId + "= -1) And UserID=" + _sUserID + ")");
                }
                else if (_iUsreTypeId == 3)//For Domestic  Dealer
                {
                    Func.Common.BindDataToCombo(drpCountry, clsCommon.ComboQueryType.StateUserWise, 0, " And M_State.ID in(Select distinct StateID from M_Sys_UserPermissions where IsActive=1 And (RegionID=" + iRegionId + " or " + iRegionId + "= -1) And UserID=" + _sUserID + ")");
                }
                else
                {
                    Func.Common.BindDataToCombo(drpCountry, clsCommon.ComboQueryType.CountryUserWise, 0, " And M_Country.ID  in (Select distinct CountryId from M_Sys_UserPermissions where IsActive=1 And (RegionID=" + iRegionId + " or " + iRegionId + "= -1) And UserID=" + _sUserID + ")");
                }
                if (drpCountry.Items.Count == 2)
                {
                    drpCountry.Enabled = false;
                    drpCountry.Items[1].Selected = true;
                }
                //else
                //    drpCountry.Items.Insert(1, new ListItem("All", "-1"));

                FillDealerCountryOrStateWiseCombo();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }
        public void SetControlValue()
        {
            try
            {
                clsUser objectUser = new clsUser();
                _iUsreTypeId = Func.Convert.iConvertToInt(Session["UserType"]);
                if (_sUserID == 0)
                {
                    _sUserID = Func.Convert.iConvertToInt(Session["UserID"].ToString());
                }
                drpRegion.Items.Clear();
                drpCountry.Items.Clear();
                Func.Common.BindDataToCombo(drpRegion, clsCommon.ComboQueryType.RegionUserWise, 0, " And M_Region.ID  in(select distinct(RegionId) from M_Sys_UserPermissions where userid =" + _sUserID + " And IsActive=1)");
                if (_iUsreTypeId == 2) //For Domestic VECV Users
                {
                    lblCountry.Text = "State";
                    lblDistributorName.Text = "Dealer Name:";
                    lblCurrency.Visible = false;
                    txtCurrency.Visible = false;
                }
                else //For Export VECV Users
                {

                }
                drpRegion.Enabled = true;
                if (drpRegion.Items.Count == 2)
                {
                    drpRegion.Enabled = false;
                    drpRegion.Items[1].Selected = true;
                }
                //else
                //    drpRegion.Items.Insert(1, new ListItem("All", "-1"));
                FillCountryOrStateCombo();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);

            }
        }

        //protected void ChkAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ChkAll.Checked == true)
        //    {
        //        for (int j = 0; j < ChkDealer.Items.Count; j++)
        //        {
        //            ChkDealer.Items[j].Selected = true;
        //        }
        //        txtDealerName.Text = "All Selected";
        //    }
        //    else
        //    {
        //        for (int j = 0; j < ChkDealer.Items.Count; j++)
        //        {
        //            ChkDealer.Items[j].Selected = false;
        //        }
        //        txtDealerName.Text = "--Select--";
        //    }
        //}
    }
}