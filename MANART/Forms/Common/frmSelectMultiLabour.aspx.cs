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
using ASPnetControls;

namespace MANART.Forms.Common
{
    public partial class frmSelectMultiLabour : System.Web.UI.Page
    {
        DataSet dsSrchgrid;
        private string sDealerId;
        private string sSelectedLabourID = "";
        private string sModelGroupID = "41";
        private int iModBasCatID = 0;
        private int iEstmtId = 0;
        private string sWarr = "N";
        private string sJobtype = "";
        string RepairOrderDate = "";
        string AMCLab = "";
        string sDocType = "";
        //sujata 25012011
        int iTotalCnt = 0;
        //Sujata 25012011
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ExpirePageCache();
                iModBasCatID = Func.Convert.iConvertToInt(Request.QueryString["ModBasCatID"].ToString());
                iEstmtId = Func.Convert.iConvertToInt(Request.QueryString["SelectedEstmtID"].ToString());
                sWarr = Func.Convert.sConvertToString(Request.QueryString["WarrantyLab"].ToString());
                sJobtype= Func.Convert.sConvertToString(Request.QueryString["Jobtype"].ToString());
                RepairOrderDate = Func.Convert.sConvertToString(Request.QueryString["RepairOrderDate"]);
                AMCLab = Func.Convert.sConvertToString(Request.QueryString["AMCLab"]);
                sDocType = Func.Convert.sConvertToString(Request.QueryString["DocType"]);
                if (!IsPostBack)
                {
                    lblTitle.Text = "Labour Master";
                    //bindGrid("A", "x");
                    DrpSelFrom.Visible = (Func.Convert.iConvertToInt(Request.QueryString["SelectedEstmtID"]) == 0) ? false : true;
                    Func.Common.BindDataToCombo(DrpLabGrp, clsCommon.ComboQueryType.LaborGroup, iModBasCatID);
                    if (sDocType == "EXL")
                    {
                        DrpLabourSelect.Items.Add(new ListItem("Warranty Labour", "C", true));
                    }
                    else
                    {
                        DrpLabourSelect.Items.Add(new ListItem("Paid Labour", "D", true));
                        if (sJobtype == "5" && AMCLab == "Y") DrpLabourSelect.Items.Add(new ListItem("RMC Labour", "C", true));
                        if (sWarr == "Y" && sJobtype != "5") DrpLabourSelect.Items.Add(new ListItem("Warranty Labour", "C", true));
                        //if (iEstmtId > 0) DrpLabourSelect.Items.Add(new ListItem("Estimated Labour", "E", true));                
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
        }
           private void ExpirePageCache()
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
        Response.Cache.SetLastModified(DateTime.Now);
        Response.Cache.SetAllowResponseInBrowserHistory(false);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            int idataCount = 0;
            DataTable dtDetails = new DataTable();
            DataTable dtTemp = new DataTable();
            
            dtDetails = (DataTable)Session["LabourDetails"];
            idataCount = dtDetails.Rows.Count;

            dtTemp = dtDetails.Clone();

            sDocType = Func.Convert.sConvertToString(Request.QueryString["DocType"]);

            DataRow dr;
            if (dtDetails.Columns.Count == 0)
            {
                dtDetails.Columns.Clear();

                if (sDocType =="EXL")
                {
                    dtDetails.Columns.Add(new DataColumn("Labour_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Labour_Code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Labour_Desc", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Jobcard_Det_ID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("VECV_Share", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Dealer_Share", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Cust_Share", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("TaxID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Deduction_Percentage", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Deducted_Amount", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("Accepted_Amount", typeof(double)));
                    dtDetails.Columns.Add(new DataColumn("ChangeDetails_YN", typeof(string)));                    
                }
                else
                {
                    dtDetails.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("labour_code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("Tax", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("Lab_Tag", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("group_code", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("EstDtlID", typeof(int)));
                    dtDetails.Columns.Add(new DataColumn("out_lab_desc", typeof(string)));
                    dtDetails.Columns.Add(new DataColumn("AddLbrDescriptionID", typeof(int)));
                }

                dtDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("ManHrs", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Rate", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Total", typeof(double)));
                dtDetails.Columns.Add(new DataColumn("Job_Code_ID", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("Status", typeof(string)));                
                dtDetails.Columns.Add(new DataColumn("Tax1", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("Tax2", typeof(int)));
                dtDetails.Columns.Add(new DataColumn("AccdFlag", typeof(string)));                
            }

            string sPartID = sSelectedLabourID;
            //txtPartIds.Text = txtPartIds_Labour.Text;
            if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
            {
                string myString = Func.Convert.sConvertToString(txtPartIds.Text);
                string[] asd = myString.ToString().Split('#');
                int iRowCnt = 0;

                for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                {
                    //if (dtDetails.Rows.Count == 301) break;
                    myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                    string[] delim = { "<--" };
                    string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                    dr = dtDetails.NewRow();
                    //dr["SRNo"] = iRowCnt;
                    dr["ID"] = 0;
                    sPartID = sPartID + (sPartID.Length == 0 ? "" : ",") + Func.Convert.sConvertToString(myArray[0]).Trim();

                    if (sDocType == "EXL")
                    {   
                        dr["Labour_ID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["Labour_Code"] = Func.Convert.sConvertToString(myArray[1]);
                        dr["Labour_Desc"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["Jobcard_Det_ID"] = Func.Convert.iConvertToInt("0");

                        dr["VECV_Share"] = Func.Convert.iConvertToInt("0");
                        dr["Dealer_Share"] = Func.Convert.iConvertToInt("0");
                        dr["Cust_Share"] = Func.Convert.iConvertToInt("0");
                        dr["TaxID"] = Func.Convert.iConvertToInt(myArray[8]);
                        dr["Deduction_Percentage"] = Func.Convert.iConvertToInt("0");
                        dr["Deducted_Amount"] = Func.Convert.iConvertToInt("0");
                        dr["Accepted_Amount"] = Func.Convert.iConvertToInt("0");
                        dr["ChangeDetails_YN"] = Func.Convert.sConvertToString("N");
                    }
                    else
                    {
                        dr["PartLabourID"] = Func.Convert.sConvertToString(myArray[0]);
                        dr["labour_code"] = Func.Convert.sConvertToString(myArray[1]);
                        dr["PartLabourName"] = Func.Convert.sConvertToString(myArray[2]);
                        dr["part_type_tag"] = "L";
                        dr["Tax"] = Func.Convert.iConvertToInt(myArray[8]);
                        dr["Lab_Tag"] = Func.Convert.sConvertToString(myArray[6]);
                        dr["group_code"] = "L";
                        dr["EstDtlID"] = Func.Convert.iConvertToInt(myArray[11]);
                        dr["out_lab_desc"] = Func.Convert.sConvertToString(myArray[13]);
                        dr["AddLbrDescriptionID"] = Func.Convert.iConvertToInt(myArray[12]);               
                    }
                    
                    
                    dr["ManHrs"] = Func.Convert.dConvertToDouble(myArray[3]);
                    dr["Rate"] = Func.Convert.dConvertToDouble(myArray[4]);
                    dr["Total"] = Func.Convert.dConvertToDouble(myArray[5]);
                    dr["Job_Code_ID"] = Func.Convert.iConvertToInt(0);
                    dr["Status"] = "S";                   
                    dr["Tax1"] = Func.Convert.iConvertToInt(myArray[9]);
                    dr["Tax2"] = Func.Convert.iConvertToInt(myArray[10]);
                    dr["AccdFlag"] = "N";
                    
                    dtDetails.Rows.Add(dr);
                    dtDetails.AcceptChanges();
                    iRowCnt = iRowCnt + 1;
                }
            }            
            Session["LabourDetails"] = dtDetails;

            //Add Labour Timing
            int itimedataCount = 0;
            DataTable dttimeDetails = new DataTable();
            DataTable dttimeTemp = new DataTable();

            dttimeDetails = (DataTable)Session["LabourTimeDetails"];
            if (dttimeDetails != null)
            {
                itimedataCount = dttimeDetails.Rows.Count;

                dttimeTemp = dttimeDetails.Clone();
            }
            DataRow drTime;

            if (itimedataCount == 0)
            {
                if (dttimeDetails != null) dttimeDetails.Columns.Clear();
                if (dttimeDetails == null) dttimeDetails = new DataTable();

                dttimeDetails.Columns.Add(new DataColumn("ID", typeof(int)));
                dttimeDetails.Columns.Add(new DataColumn("PartLabourID", typeof(int)));
                dttimeDetails.Columns.Add(new DataColumn("labour_code", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("PartLabourName", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("Lab_Tag", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("StartTime", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("PauseReason", typeof(int)));
                dttimeDetails.Columns.Add(new DataColumn("PauseTime", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("EndTime", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("SRNo", typeof(string)));
                dttimeDetails.Columns.Add(new DataColumn("Status", typeof(string)));                
            }

            sPartID = sSelectedLabourID;
            if (Func.Convert.sConvertToString(txtPartIds.Text) != "")
            {
                string myString = Func.Convert.sConvertToString(txtPartIds.Text);
                string[] asd = myString.ToString().Split('#');
                int iRowCnt = 0;

                for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
                {
                    if (dttimeDetails.Rows.Count == 301) break;
                    myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                    string[] delim = { "<--" };
                    string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                    drTime = dttimeDetails.NewRow();
                    //dr["SRNo"] = iRowCnt;
                    drTime["ID"] = 0;
                    drTime["PartLabourID"] = Func.Convert.sConvertToString(myArray[0]);
                    sPartID = sPartID + (sPartID.Length == 0 ? "" : ",") + Func.Convert.sConvertToString(myArray[0]).Trim();
                    drTime["labour_code"] = Func.Convert.sConvertToString(myArray[1]);
                    drTime["PartLabourName"] = Func.Convert.sConvertToString(myArray[2]);
                    drTime["Lab_Tag"] = Func.Convert.sConvertToString(myArray[6]);

                    drTime["PauseReason"] = Func.Convert.iConvertToInt(0);
                    drTime["StartTime"] = "";
                    drTime["PauseTime"] = "";
                    drTime["EndTime"] = "";
                    drTime["SRNo"] = "1";
                    drTime["Status"] = "S";

                    dttimeDetails.Rows.Add(drTime);
                    dttimeDetails.AcceptChanges();
                    iRowCnt = iRowCnt + 1;
                }
            }
            Session["LabourTimeDetails"] = dttimeDetails;
            //

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseMe()", true);
        }
        catch (Exception ex)
        {
            Func.Common.ProcessUnhandledException(ex);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            PagerV2_1.ItemCount = 10;
            PagerV2_1.CurrentIndex = 0;
            bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
            LabourDetailsGrid.DataBind();
        }
        catch (Exception ex)
        {
            Func.Common.ProcessUnhandledException(ex);
        }
    }
    private void bindGrid(string sSelect, string sSearchPart)
    {

       // 'Replace Func.DB to objDB by Shyamal on 05042012
        clsDB objDB = new clsDB();
        try
        {

        DataSet dsSrchgrid;
        int iDealerId;
        string sSelectedLabourID, sSelectedEstLabourID, sSelectedWarrLabourID, sSelectedPaidLabourID, sSelectedLabourIDSent;
        string sModelGroupID, sLab_Tag;
        int iLabGrpID;
        string sCustTaxTag = "";
        string sDocGST = "";
        string RepairOrderDate = "";
                    
        sDealerId = Request.QueryString["DealerID"].ToString();
        sModelGroupID = Request.QueryString["ModelGroupID"].ToString();
        sLab_Tag = DrpLabourSelect.SelectedValue;                     
        sSelectedLabourID = Request.QueryString["SelectedLabourID"].ToString();
        sCustTaxTag = Request.QueryString["CustTaxTag"].ToString();
        sJobtype = Request.QueryString["Jobtype"].ToString();
        RepairOrderDate = Func.Convert.sConvertToString(Request.QueryString["RepairOrderDate"]);
        sDocType = Func.Convert.sConvertToString(Request.QueryString["DocType"]);
        sDocGST = Func.Convert.sConvertToString(Request.QueryString["sDocGST"].ToString());

        sSelectedEstLabourID = ""; sSelectedWarrLabourID = ""; sSelectedPaidLabourID = ""; sSelectedLabourIDSent="";

        iLabGrpID=0;
        iLabGrpID = Func.Convert.iConvertToInt(DrpLabGrp.SelectedValue);
        if (Func.Convert.sConvertToString(sSelectedLabourID) != "")
        {
            string myString = Func.Convert.sConvertToString(sSelectedLabourID);
            string[] asd = myString.ToString().Split(',');

            for (int iArrCnt = 0; iArrCnt < asd.Length; iArrCnt++)
            {

                myString = Func.Convert.sConvertToString(asd[iArrCnt]);
                string[] delim = { "<--" };
                string[] myArray = myString.ToString().Split(delim, StringSplitOptions.None);

                //if (Func.Convert.sConvertToString(myArray[2]) != "" && Func.Convert.sConvertToString(myArray[2]) != "0" && sLab_Tag =="E")
                if (Func.Convert.sConvertToString(myArray[2]) != "" && Func.Convert.sConvertToString(myArray[2]) != "0" && DrpSelFrom.SelectedValue == "E")
                {
                    sSelectedEstLabourID = sSelectedEstLabourID + ((sSelectedEstLabourID.Length > 0) ? "," : "") + Func.Convert.sConvertToString(myArray[2]);
                }
                else if (Func.Convert.sConvertToString(myArray[1]) == "C" && sLab_Tag == "C")
                {
                    sSelectedWarrLabourID = sSelectedWarrLabourID + ((sSelectedWarrLabourID.Length > 0) ? "," : "") + Func.Convert.sConvertToString(myArray[0]);
                }
                else if (Func.Convert.sConvertToString(myArray[1]) == "D" && sLab_Tag == "D")
                {
                    sSelectedPaidLabourID = sSelectedPaidLabourID + ((sSelectedPaidLabourID.Length > 0) ? "," : "") + Func.Convert.sConvertToString(myArray[0]);
                }
            }
        }

        if (PagerV2_1.CurrentIndex == 0)
        {
            PagerV2_1.CurrentIndex = 1;
        }

        //sSelectedLabourIDSent = (sLab_Tag == "E") ? sSelectedEstLabourID : (sLab_Tag == "C") ? sSelectedWarrLabourID : sSelectedPaidLabourID;
        sSelectedLabourIDSent = (DrpSelFrom.SelectedValue == "E") ? sSelectedEstLabourID : (sLab_Tag == "C") ? sSelectedWarrLabourID : sSelectedPaidLabourID;
        //We never got sLab_Tag ='E' because we remove "Estimate" list item from DrpLabourSelect. and Add new DrpSelFrom for "from where selection(Master or Estimate)" criteria
        if (sDocType == "EXL") sSelect = sDocType + sSelect;
        else if (DrpSelFrom.SelectedValue == "E") sSelect = "E" + sSelect;
        

        //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_LabourDetails_Paging", sDealerId, sModelGroupID, sSelectedLabourID, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, sSelect, sLab_Tag, iEstmtId, "");        
        //dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_LabourDetails_Paging", sDealerId, sModelGroupID, sSelectedLabourIDSent, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, sSelect, sLab_Tag, iEstmtId, "", iLabGrpID, sCustTaxTag, sJobtype);
        dsSrchgrid = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_LabourDetails_Paging", sDealerId, sModelGroupID, sSelectedLabourIDSent, sSearchPart, PagerV2_1.PageSize, PagerV2_1.CurrentIndex, sSelect, sLab_Tag, iEstmtId, "", iLabGrpID, sCustTaxTag, sJobtype, iModBasCatID, sDocGST, RepairOrderDate);

        if (dsSrchgrid == null)
        {
            return;
        }
        if (dsSrchgrid.Tables[0].Rows.Count != 0)
        {
            iTotalCnt = Func.Convert.iConvertToInt(dsSrchgrid.Tables[0].Rows[0][6]);
            PagerV2_1.ItemCount = iTotalCnt;
            LabourDetailsGrid.DataSource = dsSrchgrid;
            LabourDetailsGrid.DataBind();
            ChkSelectedParts();
        }
        else
        {
            return;
        }
        }
        catch (Exception ex)
        {
           
        }
        finally
        {
            if (objDB != null) objDB = null;
        }
    }

    private void ChkSelectedParts()
    {
        CheckBox chk = new CheckBox();
        string sLabID = "";
        string str = null;
        string strAllLabor = null;
        string[] strArr = null;
        string[] strAllLaborArr = null;

        strAllLabor = txtPartIds.Text;
        if (strAllLabor == "") return;
        strAllLaborArr = strAllLabor.Split('#');

        for (int i = 0; i < strAllLaborArr.Length; i++)
        {
            //str = txtPartIds.Text;
            str = Func.Convert.sConvertToString(strAllLaborArr[i]);
            if (str == "") return;

            string[] stringSeparators = new string[] { "<--" };

            strArr = str.Split(stringSeparators, StringSplitOptions.None);

            for (int iRowCnt = 0; iRowCnt < LabourDetailsGrid.Rows.Count; iRowCnt++)
            {
                sLabID = (LabourDetailsGrid.Rows[iRowCnt].FindControl("lblID") as Label).Text;
                if (sLabID == Func.Convert.sConvertToString(strArr[0]))
                {
                    chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkLabour");
                    chk.Checked = true;
                    break;
                }
                //for (int j = 0; j < strArr.Length; j++)
                //{
                //    if (sLabID == GetIdFromString(Func.Convert.sConvertToString(strArr[j])))
                //    {
                //        chk = (CheckBox)LabourDetailsGrid.Rows[iRowCnt].FindControl("ChkLabour");
                //        chk.Checked = true;
                //        break;
                //    }
                //}
            }
        }
    }

    private string GetIdFromString(string strArr)
    {
        
        string sID = "";
        char[] strToParse = strArr.ToCharArray();
        // convert string to array of chars    
        char ch; int charpos = 0;
        ch = strToParse[charpos];
        while (ch != ' ')
        {
            sID = sID + ch;
            charpos++;
            ch = strToParse[charpos];

        }
        return sID;
    }

    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        PagerV2_1.CurrentIndex = currnetPageIndx;
        bindGrid(Func.Convert.sConvertToString(DdlSelctionCriteria.SelectedValue), Func.Convert.sConvertToString(txtSearch.Text));
    }
}
    }
