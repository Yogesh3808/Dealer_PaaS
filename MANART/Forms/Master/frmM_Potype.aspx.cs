using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_BAL;
using MANART_DAL;
using AjaxControlToolkit;
using System.Drawing;

namespace MANART.Forms.Master
{
    public partial class frmM_Potype : System.Web.UI.Page
    {
       
        private int iPartPriceID = 0;
        private int Dealer_ED = 0;
        private int iID;
        int iUserId = 0;
        private int iDealerID = 0;
        int iHOBr_id = 0;
        private string GroupCode;
        int iMenuId = 0;
        DataSet DSPrice = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                clsSupplier Objpotype = new clsSupplier();
                DataSet ds = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iMenuId = Func.Convert.iConvertToInt(Request.QueryString["MenuID"]);
                ds = Objpotype.GetMaxPotype();
                if (iMenuId == 607 || iMenuId == 467)
                {

                   
                    GroupCode = "01";
                }
                else if (iMenuId == 609)
                {

                   
                    GroupCode = "99";
                }


                Location.bUseSpareDealerCode = true;
                iDealerID = Location.iDealerId;
                FillSelectionGrid();
                iID = Func.Convert.iConvertToInt(txtID.Text);
                if (iID == 0)
                    iID = Func.Convert.iConvertToInt(ds.Tables[0].Rows[0]["ID"]);
                if (iID != 0)
                {
                    GetDataAndDisplay();
                }
                //ExpirePageCache();
               // ExpirePageCache();

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void FillSelectionGrid()
        {
            try
            {
                //SearchGrid.bGridFillUsingSql = true;
                SearchGrid.AddToSearchCombo("POcode");
                SearchGrid.AddToSearchCombo("prefix");
                SearchGrid.iDealerID = iDealerID;
                 SearchGrid.iBrHODealerID = iHOBr_id;
                SearchGrid.sGridPanelTitle = "PO Type List";
                SearchGrid.sSqlFor = "M_PoType";
                if (iMenuId == 607 || iMenuId == 467)
                {

                    SearchGrid.sModelPart = "01";
                    GroupCode = "01";
                }
                else if (iMenuId == 609)
                {

                    SearchGrid.sModelPart = "99";
                    GroupCode = "99";
                }
                SearchGrid.iBrHODealerID = iHOBr_id;

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }

        private void DisplayData(DataSet ds)
        {
            try
            {

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                  
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
                    txtprefix.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["prefix"]);
                    txtpocode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["POcode"]);
                    txtactive.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Active"]);
                   

                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }

        protected void Location_DDLSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iDealerID = Location.iDealerId;
                FillSelectionGrid();
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }

        }
        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
                iID = Func.Convert.iConvertToInt(txtID.Text);
                GetDataAndDisplay();
            }
            catch (Exception ex) { }
        }

        private void GetDataAndDisplay()
        {
            try
            {
                clsSupplier ObjDealer = new clsSupplier();
                DataSet ds = new DataSet();
                if (iID != 0)
                {
                    ds = ObjDealer.GetPotypedetails(iID);
                    DisplayData(ds);
                    ObjDealer = null;
                }
                else
                {
                    ds = null;
                    DisplayData(ds);
                    ObjDealer = null;
                }

            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
    }
}