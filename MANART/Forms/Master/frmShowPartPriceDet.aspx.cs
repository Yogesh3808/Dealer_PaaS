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

namespace MANART.Forms.Master
{
    public partial class frmShowPartPriceDet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            clsDB objDB = new clsDB();
            try
            {
                if (!IsPostBack)
                {
                    lblTitle.Text = "Show Part Rate History";

                }
                ViewState["partPrice"] = null;

                int iDealerId = 0;
                int iPartID = 0;
                int iDealer_ID = 0;
                //if (Request.QueryString["DealerId"] != null)
                //{
                //    iDealerId = Func.Convert.iConvertToInt(Request.QueryString["DealerId"]);
                //}
                DataTable dtDetails = null;
                if (Request.QueryString["SqlFor"] != null)
                    if (Func.Convert.sConvertToString(Request.QueryString["SqlFor"]) == "ModelPrice")
                    {
                        if (Request.QueryString["PartID"] != null)
                        {
                            iPartID = Func.Convert.iConvertToInt(Request.QueryString["PartID"]);
                        }
                        DataSet dsDetails = objDB.ExecuteQueryAndGetDataset("Select Model_ID,Dealer_ID from M_ModelRate where ID=" + iPartID);
                        int Model_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Model_ID"].ToString());
                        int Dealer_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Dealer_ID"].ToString());
                        dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetModelPrice", "E", Dealer_ID, Model_ID, "All");
                        if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                        {

                            ModelPriceGrid.DataSource = null;
                            ModelPriceGrid.DataBind();
                        }
                        else
                        {

                            ModelPriceGrid.DataSource = dtDetails;
                            ViewState["partPrice"] = dtDetails;
                            ModelPriceGrid.DataBind();
                        }

                    }
                    else
                        if (Func.Convert.sConvertToString(Request.QueryString["SqlFor"]) == "PartPrice")
                        {
                            if (Request.QueryString["PartID"] != null)
                            {
                                iPartID = Func.Convert.iConvertToInt(Request.QueryString["PartID"]);
                            }
                            DataSet dsDetails = new DataSet(); ;

                            if (Func.Convert.sConvertToString(Request.QueryString["ModelPart"]) == "01" || Func.Convert.sConvertToString(Request.QueryString["ModelPart"]) == "02")
                            {
                                if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                                {
                                    iDealer_ID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                                    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Distinct Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.Part_ID=" + iPartID + " And M_PartRateMaster.Dealer_ID=" + iDealer_ID);
                                }
                                else
                                    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_PartRateMaster inner join M_PartMaster on M_PartRateMaster.Part_ID=M_PartMaster.ID  where M_PartRateMaster.ID=" + iPartID);
                            }
                            if (Func.Convert.sConvertToString(Request.QueryString["ModelPart"]) == "99")
                            {
                                if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                                {
                                    iDealer_ID = Func.Convert.iConvertToInt(Request.QueryString["DealerID"].ToString());
                                    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_LocalPartRate inner join M_PartMaster on M_LocalPartRate.Part_ID=M_PartMaster.ID  where M_LocalPartRate.Part_ID=" + iPartID + " And M_LocalPartRate.Dealer_ID=" + iDealer_ID);
                                }
                                else
                                    dsDetails = objDB.ExecuteQueryAndGetDataset("Select Part_ID,Dealer_ID,group_code from M_LocalPartRate inner join M_PartMaster on M_LocalPartRate.Part_ID=M_PartMaster.ID  where M_LocalPartRate.ID=" + iPartID);

                            }

                            int Part_Id = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Part_ID"].ToString());
                            int Dealer_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Dealer_ID"].ToString());
                            string sGroup_code = Func.Convert.sConvertToString(dsDetails.Tables[0].Rows[0]["group_code"].ToString());

                            if (Func.Convert.sConvertToString(Request.QueryString["FromPage"]) == "OA_Invoice")
                            {
                                dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetPartPrice", sGroup_code, Dealer_ID, Part_Id, "OA_Invoice");
                            }
                            else
                            {
                                dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetPartPrice", sGroup_code, Dealer_ID, Part_Id, "All");
                            }
                            

                            if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                            {

                                PartPriceGrid.DataSource = null;
                                PartPriceGrid.DataBind();
                            }
                            else
                            {
                                if (Func.Convert.sConvertToString(Request.QueryString["Distributor"]) == "Y")
                                {
                                    PartPriceGrid.Columns[0].Visible = false;
                                }
                                else
                                {
                                    PartPriceGrid.Columns[0].Visible = true;
                                }
                                PartPriceGrid.DataSource = dtDetails;
                                ViewState["partPrice"] = dtDetails;
                                PartPriceGrid.DataBind();
                            }
                        }
                        else if (Func.Convert.sConvertToString(Request.QueryString["SqlFor"]) == "LubricantPrice")
                        {
                            if (Request.QueryString["PartID"] != null)
                            {
                                iPartID = Func.Convert.iConvertToInt(Request.QueryString["PartID"]);
                            }
                            DataSet dsDetails = objDB.ExecuteQueryAndGetDataset("Select Lub_Ty_No,State_ID,LubType from M_LubricantRate where ID=" + iPartID);
                            string Lub_Ty_No = dsDetails.Tables[0].Rows[0]["Lub_Ty_No"].ToString();
                            int State_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["State_ID"].ToString());
                            string LubType = dsDetails.Tables[0].Rows[0]["LubType"].ToString();

                            dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetLubricantPrice", State_ID, iPartID, Lub_Ty_No, "All", LubType);
                            if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                            {

                                LubricantPriceGrid.DataSource = null;
                                LubricantPriceGrid.DataBind();
                            }
                            else
                            {

                                LubricantPriceGrid.DataSource = dtDetails;
                                ViewState["partPrice"] = dtDetails;
                                LubricantPriceGrid.DataBind();
                            }
                        }
                        else if (Func.Convert.sConvertToString(Request.QueryString["SqlFor"]) == "ServicePolicy")
                        {
                            if (Request.QueryString["PartID"] != null)
                            {
                                iPartID = Func.Convert.iConvertToInt(Request.QueryString["PartID"]);
                            }
                            DataSet dsDetails = objDB.ExecuteQueryAndGetDataset("Select Serv_Id,Model_ID from M_Mgsrm where ID=" + iPartID);
                            int ServicePolicy_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Serv_Id"].ToString());
                            int Model_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Model_ID"].ToString());
                            dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetServicePolicy", ServicePolicy_ID, Model_ID, "All");
                            if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                            {

                                ServicePolicyGrid.DataSource = null;
                                ServicePolicyGrid.DataBind();
                            }
                            else
                            {

                                ServicePolicyGrid.DataSource = dtDetails;
                                ViewState["partPrice"] = dtDetails;
                                ServicePolicyGrid.DataBind();
                            }
                        }
                        else if (Func.Convert.sConvertToString(Request.QueryString["SqlFor"]) == "LaborRate")
                        {
                            if (Request.QueryString["PartID"] != null)
                            {
                                iPartID = Func.Convert.iConvertToInt(Request.QueryString["PartID"]);
                            }
                            DataSet dsDetails = objDB.ExecuteQueryAndGetDataset("Select Dealer_Id,LabourType from M_LaborRate where ID=" + iPartID);
                            int Dealer_ID = Func.Convert.iConvertToInt(dsDetails.Tables[0].Rows[0]["Dealer_Id"].ToString());
                            string LubType = dsDetails.Tables[0].Rows[0]["LabourType"].ToString();
                            dtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetLaborRate", Dealer_ID, "All", LubType);
                            if (Func.Common.iRowCntOfTable(dtDetails) == 0)
                            {

                                LaborRateGrid.DataSource = null;
                                LaborRateGrid.DataBind();
                            }
                            else
                            {

                                LaborRateGrid.DataSource = dtDetails;
                                ViewState["partPrice"] = dtDetails;
                                LaborRateGrid.DataBind();
                            }
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
        protected void ModelPriceGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ModelPriceGrid.DataSource = (DataTable)ViewState["partPrice"];
            ModelPriceGrid.PageIndex = e.NewPageIndex;
            ModelPriceGrid.DataBind();
        }
        protected void PartPriceGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PartPriceGrid.DataSource = (DataTable)ViewState["partPrice"];
            PartPriceGrid.PageIndex = e.NewPageIndex;
            PartPriceGrid.DataBind();
        }
        protected void LubricantPriceGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LubricantPriceGrid.DataSource = (DataTable)ViewState["partPrice"];
            LubricantPriceGrid.PageIndex = e.NewPageIndex;
            LubricantPriceGrid.DataBind();
        }

        protected void ServicePolicyGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ServicePolicyGrid.DataSource = (DataTable)ViewState["partPrice"];
            ServicePolicyGrid.PageIndex = e.NewPageIndex;
            ServicePolicyGrid.DataBind();
        }
        protected void LaborRateGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LaborRateGrid.DataSource = (DataTable)ViewState["partPrice"];
            LaborRateGrid.PageIndex = e.NewPageIndex;
            LaborRateGrid.DataBind();
        }
    }
}