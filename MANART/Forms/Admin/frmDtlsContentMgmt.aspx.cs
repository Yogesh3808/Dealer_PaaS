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
using System.IO;

namespace MANART.Forms.Admin
{
    public partial class frmDtlsContentMgmt : System.Web.UI.Page
    {
        int ContentHeaderID;
        protected void Page_Load(object sender, EventArgs e)
        {
            ContentHeaderID = Func.Convert.iConvertToInt(Request.QueryString["ID"]);

            if (Page.IsPostBack == false)
            {
                TrMsg.Visible = false;
                DataSet ds = null;
                // 'Replace Func.DB to objDB by Shyamal on 05042012
                clsDB objDB = new clsDB();
                try
                {

                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMenuDataForContent", "", ContentHeaderID);
                    if (ds.Tables[1].Rows.Count > 0)
                    {

                        if (ds.Tables[1].Rows[0]["Is_DomExp"].ToString() == "D")
                        {
                            rdoType.SelectedValue = "D";
                            BindMenuFirstTime(ds, "State");
                            trMenu.Style.Add("display", "");
                        }
                        else if (ds.Tables[1].Rows[0]["Is_DomExp"].ToString() == "E")
                        {
                            BindMenuFirstTime(ds, "Country");
                            trMenu.Style.Add("display", "");
                            rdoType.SelectedValue = "E";
                        }
                        else
                        {
                            trMenu.Style.Add("display", "none");
                            rdoType.SelectedValue = "C";
                        }
                    }
                    else
                    {
                        trMenu.Style.Add("display", "none");
                        rdoType.SelectedValue = "C";
                        lblMsg.Text = "Dealer details does not exists!";
                        TrMsg.Visible = true;
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
        
        }
        private void BindMenu(DataSet ds, string obj)
        {
            TreeNode Rootmenu;
            TreeNode Regionmenu;
            TreeNode Statemenu;
            TreeNode Dealermenu;
            TreeDocument.Nodes.Clear();
            DataTable dtRegion = SelectDistinct(ds.Tables[0], "Region_ID", "Region_Name");
            Rootmenu = new TreeNode("ALL", "0");
            TreeDocument.Nodes.Add(Rootmenu);
            for (int iRegion = 0; iRegion < dtRegion.Rows.Count; iRegion++)
            {
                Regionmenu = new TreeNode(dtRegion.Rows[iRegion]["Region_Name"].ToString(), dtRegion.Rows[iRegion]["Region_ID"].ToString());
                if (TreeDocument.FindNode("0") != null)
                {
                    TreeDocument.FindNode("0").ChildNodes.Add(Regionmenu);
                    TreeDocument.FindNode("0").Expanded = true;
                }
                DataView dvState = ds.Tables[0].DefaultView;
                dvState.RowFilter = "Region_ID=" + dtRegion.Rows[iRegion]["Region_ID"].ToString();
                DataTable dtState = SelectDistinct(dvState.ToTable(), obj + "_ID", obj);
                for (int iState = 0; iState < dtState.Rows.Count; iState++)
                {
                    Statemenu = new TreeNode(dtState.Rows[iState][obj].ToString(), dtState.Rows[iState][obj + "_ID"].ToString());
                    if (TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString()) != null)
                    {
                        TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString()).ChildNodes.Add(Statemenu);
                        TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString()).Expanded = false;
                    }
                    DataView dvDealer = ds.Tables[0].DefaultView;
                    dvDealer.RowFilter = obj + "_ID=" + dtState.Rows[iState][obj + "_ID"].ToString();
                    for (int iDealer = 0; iDealer < dvDealer.ToTable().Rows.Count; iDealer++)
                    {
                        Dealermenu = new TreeNode(dvDealer.ToTable().Rows[iDealer]["Dealer_Name"].ToString(), dvDealer.ToTable().Rows[iDealer]["Dealer_ID"].ToString());
                        if (TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString() + "/" + dtState.Rows[iState][obj + "_ID"].ToString()) != null)
                        {
                            TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString() + "/" + dtState.Rows[iState][obj + "_ID"].ToString()).ChildNodes.Add(Dealermenu);

                        }
                    }
                }

            }

        }
        private void BindMenuFirstTime(DataSet ds, string obj)
        {
            TreeNode Rootmenu;
            TreeNode Regionmenu;
            TreeNode Statemenu;
            TreeNode Dealermenu;
            Boolean Is_ALL = false;
            Boolean Is_Region = false;
            Boolean Is_State = false;
            TreeDocument.Nodes.Clear();
            DataTable dtRegion = SelectDistinct(ds.Tables[0], "Region_ID", "Region_Name");
            Rootmenu = new TreeNode("ALL", "0");
            TreeDocument.Nodes.Add(Rootmenu);
            if (Func.Convert.iConvertToInt(ds.Tables[1].Rows[0]["ContentHdr_ID"]) == ContentHeaderID && Func.Convert.sConvertToString(ds.Tables[1].Rows[0]["Is_All"]) == "Y" && Func.Convert.iConvertToInt(ds.Tables[1].Rows[0]["Region_ID"]) == 0 && Func.Convert.iConvertToInt(ds.Tables[1].Rows[0]["State_ID"]) == 0)
            {
                TreeDocument.Nodes[0].Checked = true;
                Is_ALL = true;
            }
            for (int iRegion = 0; iRegion < dtRegion.Rows.Count; iRegion++)
            {
                Is_Region = false;
                Regionmenu = new TreeNode(dtRegion.Rows[iRegion]["Region_Name"].ToString(), dtRegion.Rows[iRegion]["Region_ID"].ToString());
                if (TreeDocument.FindNode("0") != null)
                {
                    TreeDocument.FindNode("0").ChildNodes.Add(Regionmenu);
                    TreeDocument.FindNode("0").Expanded = true;

                    for (int idtlsRegion = 0; idtlsRegion < ds.Tables[1].Rows.Count; idtlsRegion++)
                        if ((Func.Convert.iConvertToInt(ds.Tables[1].Rows[idtlsRegion]["ContentHdr_ID"]) == ContentHeaderID && Func.Convert.sConvertToString(ds.Tables[1].Rows[idtlsRegion]["Is_All"]) == "Y" && Func.Convert.iConvertToInt(ds.Tables[1].Rows[idtlsRegion]["Region_ID"]) == Func.Convert.iConvertToInt(dtRegion.Rows[iRegion]["Region_ID"])) || Is_ALL == true)
                        {
                            TreeDocument.Nodes[0].ChildNodes[iRegion].Checked = true;
                            Is_Region = true;
                        }

                }

                DataView dvState = ds.Tables[0].DefaultView;
                dvState.RowFilter = "Region_ID=" + dtRegion.Rows[iRegion]["Region_ID"].ToString();
                DataTable dtState = SelectDistinct(dvState.ToTable(), obj + "_ID", obj);
                for (int iState = 0; iState < dtState.Rows.Count; iState++)
                {
                    Is_State = false;
                    Statemenu = new TreeNode(dtState.Rows[iState][obj].ToString(), dtState.Rows[iState][obj + "_ID"].ToString());
                    if (TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString()) != null)
                    {
                        TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString()).ChildNodes.Add(Statemenu);
                        TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString()).Expanded = false;

                        for (int idtlsState = 0; idtlsState < ds.Tables[1].Rows.Count; idtlsState++)
                            if ((Func.Convert.iConvertToInt(ds.Tables[1].Rows[idtlsState]["ContentHdr_ID"]) == ContentHeaderID && Func.Convert.sConvertToString(ds.Tables[1].Rows[idtlsState]["Is_All"]) == "Y" && Func.Convert.iConvertToInt(ds.Tables[1].Rows[idtlsState][obj + "_ID"]) == Func.Convert.iConvertToInt(dtState.Rows[iState][obj + "_ID"])) || Is_ALL == true || Is_Region == true)
                            {
                                TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].Checked = true;
                                Is_State = true;
                            }

                    }

                    DataView dvDealer = ds.Tables[0].DefaultView;
                    dvDealer.RowFilter = obj + "_ID=" + dtState.Rows[iState][obj + "_ID"].ToString();
                    for (int iDealer = 0; iDealer < dvDealer.ToTable().Rows.Count; iDealer++)
                    {
                        Dealermenu = new TreeNode(dvDealer.ToTable().Rows[iDealer]["Dealer_Name"].ToString(), dvDealer.ToTable().Rows[iDealer]["Dealer_ID"].ToString());
                        if (TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString() + "/" + dtState.Rows[iState][obj + "_ID"].ToString()) != null)
                        {
                            TreeDocument.FindNode("0/" + dtRegion.Rows[iRegion]["Region_ID"].ToString() + "/" + dtState.Rows[iState][obj + "_ID"].ToString()).ChildNodes.Add(Dealermenu);
                            for (int idtlsDealer = 0; idtlsDealer < ds.Tables[1].Rows.Count; idtlsDealer++)
                                if ((Func.Convert.iConvertToInt(ds.Tables[1].Rows[idtlsDealer]["ContentHdr_ID"]) == ContentHeaderID && Func.Convert.sConvertToString(ds.Tables[1].Rows[idtlsDealer]["Is_All"]) == "Y" && Func.Convert.iConvertToInt(ds.Tables[1].Rows[idtlsDealer]["Dealer_ID"]) == Func.Convert.iConvertToInt(dvDealer.ToTable().Rows[iDealer]["Dealer_ID"])) || Is_ALL == true || Is_Region == true || Is_State == true)
                                {
                                    TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].ChildNodes[iDealer].Checked = true;
                                    Is_State = true;
                                }

                        }
                    }
                }

            }

        }

        private DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                    setLastValues(lastValues, row, FieldNames);
                }
            }

            return newTable;
        }
        private bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        private DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }

        protected void rdoType_SelectedIndexChanged(object sender, EventArgs e)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (rdoType.Items[0].Selected == true)
                    trMenu.Style.Add("display", "none");

                else if (rdoType.Items[1].Selected == true)
                {
                    DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMenuDataForContent", 'D', 0);
                    BindMenu(ds, "State");
                    trMenu.Style.Add("display", "");
                }
                else if (rdoType.Items[2].Selected == true)
                {
                    DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMenuDataForContent", 'E', 0);
                    BindMenu(ds, "Country");
                    trMenu.Style.Add("display", "");
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
        //protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        //{
        //    Activity objActivity = new Activity();
        //    try
        //    {
        //        ImageButton ObjImageButton = (ImageButton)sender;
        //        if (ObjImageButton.ID == "ToolbarButton1")//for New
        //        {


        //        }
        //        else if (ObjImageButton.ID == "ToolbarButton2")//for Save
        //        {

        //            if (bSaveRecord(false) == false) return;         

        //        }
        //        else if (ObjImageButton.ID == "ToolbarButton3")//for Confirm
        //        {
        //            if (bSaveRecord(true) == false) return; 
        //        }            

        //    }
        //    catch (Exception ex)
        //    {
        //        objActivity = null;
        //    }
        //    objActivity = null;
        //}
        //ToSave Record
        private bool bSaveRecord(bool bSaveWithConfirm)
        {
            if (bValidateRecord() == false)
            {
                return false;
            }
            DataTable dtHdr = new DataTable();
            clsContent ObjContent = new clsContent();

            UpdateHdrValueFromControl(dtHdr, bSaveWithConfirm);

            if (ObjContent.bSaveContentDetails(dtHdr, ContentHeaderID) == true)
            {
                //if (bSaveWithConfirm == true)
                //{
                //    Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(7);</script>");
                //}
                //else
                //{

                string vMsg = "Document Content Details Saved.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);

                //}
            }
            else
            {
                string vMsg = "Error in Document Content Details Saved.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowInformationMessage('" + vMsg + "');", true);

                return false;
            }
            ObjContent = null;
            return true;
        }
        private bool bValidateRecord()
        {
            return true;
        }
        private void UpdateHdrValueFromControl(DataTable dtHdr, Boolean ConfirmStatus)
        {
            DataRow dr;
            //Get Header InFormation                
            dtHdr.Columns.Add(new DataColumn("ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("ContentHdr_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Region_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Country_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("State_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
            dtHdr.Columns.Add(new DataColumn("Is_Confirm", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Is_DomExp", typeof(string)));
            dtHdr.Columns.Add(new DataColumn("Is_All", typeof(string)));


            if (rdoType.SelectedItem.Text != "Common")
            {
                for (int iRoot = 0; iRoot < TreeDocument.Nodes.Count; iRoot++)
                {
                    if (TreeDocument.Nodes[0].Checked == true)
                    {
                        dr = dtHdr.NewRow();
                        dr["ID"] = 0;
                        dr["ContentHdr_ID"] = ContentHeaderID;
                        dr["Region_ID"] = 0;
                        dr["Country_ID"] = 0;
                        dr["State_ID"] = 0;
                        dr["Dealer_ID"] = 0;
                        dr["Is_Confirm"] = (ConfirmStatus == true) ? "Y" : "N";
                        dr["Is_DomExp"] = rdoType.SelectedItem.Text;
                        dr["Is_All"] = "Y";
                        dtHdr.Rows.Add(dr);
                        dtHdr.AcceptChanges();
                    }
                    else
                    {
                        for (int iRegion = 0; iRegion < TreeDocument.Nodes[0].ChildNodes.Count; iRegion++)
                        {
                            if (TreeDocument.Nodes[0].ChildNodes[iRegion].Checked == true)
                            {
                                dr = dtHdr.NewRow();
                                dr["ID"] = 0;
                                dr["ContentHdr_ID"] = ContentHeaderID;
                                dr["Region_ID"] = TreeDocument.Nodes[0].ChildNodes[iRegion].Value;
                                dr["Country_ID"] = 0;
                                dr["State_ID"] = 0;
                                dr["Dealer_ID"] = 0;
                                dr["Is_Confirm"] = (ConfirmStatus == true) ? "Y" : "N";
                                dr["Is_DomExp"] = rdoType.SelectedItem.Text;
                                dr["Is_All"] = "Y";
                                dtHdr.Rows.Add(dr);
                                dtHdr.AcceptChanges();
                            }
                            else
                            {
                                for (int iState = 0; iState < TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes.Count; iState++)
                                {
                                    if (TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].Checked == true)
                                    {
                                        dr = dtHdr.NewRow();
                                        dr["ID"] = 0;
                                        dr["ContentHdr_ID"] = ContentHeaderID;
                                        dr["Region_ID"] = 0;
                                        dr["Country_ID"] = 0;
                                        dr["State_ID"] = TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].Value;
                                        dr["Dealer_ID"] = 0;
                                        dr["Is_Confirm"] = (ConfirmStatus == true) ? "Y" : "N";
                                        dr["Is_DomExp"] = rdoType.SelectedItem.Text;
                                        dr["Is_All"] = "Y";
                                        dtHdr.Rows.Add(dr);
                                        dtHdr.AcceptChanges();
                                    }
                                    else
                                    {
                                        for (int iDealer = 0; iDealer < TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].ChildNodes.Count; iDealer++)
                                        {
                                            if (TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].ChildNodes[iDealer].Checked == true)
                                            {
                                                dr = dtHdr.NewRow();
                                                dr["ID"] = 0;
                                                dr["ContentHdr_ID"] = ContentHeaderID;
                                                dr["Region_ID"] = 0;
                                                dr["Country_ID"] = 0;
                                                dr["State_ID"] = 0;
                                                dr["Dealer_ID"] = TreeDocument.Nodes[0].ChildNodes[iRegion].ChildNodes[iState].ChildNodes[iDealer].Value;
                                                dr["Is_Confirm"] = (ConfirmStatus == true) ? "Y" : "N";
                                                dr["Is_DomExp"] = rdoType.SelectedItem.Text;
                                                dr["Is_All"] = "N";
                                                dtHdr.Rows.Add(dr);
                                                dtHdr.AcceptChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                dr = dtHdr.NewRow();
                dr["ID"] = 0;
                dr["ContentHdr_ID"] = ContentHeaderID;
                dr["Region_ID"] = 0;
                dr["Country_ID"] = 0;
                dr["State_ID"] = 0;
                dr["Dealer_ID"] = 0;
                dr["Is_Confirm"] = (ConfirmStatus == true) ? "Y" : "N";
                dr["Is_DomExp"] = "C";
                dr["Is_All"] = "Y";
                dtHdr.Rows.Add(dr);
                dtHdr.AcceptChanges();
            }
        }

        protected void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (bSaveRecord(false) == false) return;
            }
            catch (Exception ex)
            {

            }
        }
    }
}