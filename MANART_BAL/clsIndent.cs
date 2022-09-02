using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    public class clsIndent
    {
        public clsIndent()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool bSaveIndent(string sDealerCode, DataTable dtHdr, DataTable dtIndDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveIndentHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details 
                if (bSaveIndentDetails(objDB, dtIndDet, iHdrID) == false) goto ExitFunction;
                bSaveRecord = true;
            ExitFunction:
                if (bSaveRecord == false)
                {
                    objDB.RollbackTransaction();
                    bSaveRecord = false;
                }
                else
                {
                    objDB.CommitTransaction();
                }
                return bSaveRecord;
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

        //To Save Indent Header
        private bool bSaveIndentHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {                    
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["Indent_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, "IND", iDelearId);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_DomIndentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Indent_No"], dtHdr.Rows[0]["Indent_Date"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Indent_MonthNo"], dtHdr.Rows[0]["Indent_Confirm"], dtHdr.Rows[0]["Curr_Month_Dealer_Qty"], dtHdr.Rows[0]["Delear_Remarks"], dtHdr.Rows[0]["AVG_PurchaseRetail"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "IND", iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_DomIndentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Indent_No"], dtHdr.Rows[0]["Indent_Date"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Indent_MonthNo"], dtHdr.Rows[0]["Indent_Confirm"], dtHdr.Rows[0]["Curr_Month_Dealer_Qty"], dtHdr.Rows[0]["Delear_Remarks"], dtHdr.Rows[0]["AVG_PurchaseRetail"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //To save Indent Details
        private bool bSaveIndentDetails(clsDB objDB, DataTable dtIndDet, int iHdrId)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iIndDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtIndDet.Rows.Count; iRowCnt++)
                {
                    iIndDetID = Func.Convert.iConvertToInt(dtIndDet.Rows[iRowCnt]["ID"]);
                    //Sujata 23022011
                    int iModlID = Func.Convert.iConvertToInt(dtIndDet.Rows[iRowCnt]["Model_ID"]);
                    //Sujata 23022011
                    if (iIndDetID == 0)
                    {
                        //Sujata 23022011
                        //iIndDetID = objDB.ExecuteStoredProcedure("SP_DomIndentDet_Save", dtIndDet.Rows[iRowCnt]["ID"], iHdrId, dtIndDet.Rows[iRowCnt]["Model_ID"], dtIndDet.Rows[iRowCnt]["AVG_Purchase"], dtIndDet.Rows[iRowCnt]["Stock"], dtIndDet.Rows[iRowCnt]["W1_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W1_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W1_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W1_Total_Qty"], dtIndDet.Rows[iRowCnt]["W1_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W1_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W2_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W2_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W2_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W2_Total_Qty"], dtIndDet.Rows[iRowCnt]["W2_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W2_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W3_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W3_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W3_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W3_Total_Qty"], dtIndDet.Rows[iRowCnt]["W3_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W3_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W4_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W4_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W4_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W4_Total_Qty"], dtIndDet.Rows[iRowCnt]["W4_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W4_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W5_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W5_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W5_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W5_Total_Qty"], dtIndDet.Rows[iRowCnt]["W5_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W5_VECV_Qty"], dtIndDet.Rows[iRowCnt]["M1_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["M1_VECV_Qty"], dtIndDet.Rows[iRowCnt]["M2_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["M2_VECV_Qty"]);
                        if (iModlID != 0)
                        {
                            iIndDetID = objDB.ExecuteStoredProcedure("SP_DomIndentDet_Save", dtIndDet.Rows[iRowCnt]["ID"], iHdrId, dtIndDet.Rows[iRowCnt]["Model_ID"], dtIndDet.Rows[iRowCnt]["AVG_Purchase"], dtIndDet.Rows[iRowCnt]["AVG_Retail"], dtIndDet.Rows[iRowCnt]["Stock"], dtIndDet.Rows[iRowCnt]["W1_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W1_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W1_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W1_Total_Qty"], dtIndDet.Rows[iRowCnt]["W1_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W1_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W2_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W2_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W2_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W2_Total_Qty"], dtIndDet.Rows[iRowCnt]["W2_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W2_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W3_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W3_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W3_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W3_Total_Qty"], dtIndDet.Rows[iRowCnt]["W3_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W3_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W4_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W4_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W4_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W4_Total_Qty"], dtIndDet.Rows[iRowCnt]["W4_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W4_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W5_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W5_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W5_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W5_Total_Qty"], dtIndDet.Rows[iRowCnt]["W5_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W5_VECV_Qty"], dtIndDet.Rows[iRowCnt]["M1_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["M1_VECV_Qty"], dtIndDet.Rows[iRowCnt]["M2_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["M2_VECV_Qty"]);
                        }
                        //Sujata 23022011
                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_DomIndentDet_Save", dtIndDet.Rows[iRowCnt]["ID"], iHdrId, dtIndDet.Rows[iRowCnt]["Model_ID"], dtIndDet.Rows[iRowCnt]["AVG_Purchase"], dtIndDet.Rows[iRowCnt]["AVG_Retail"], dtIndDet.Rows[iRowCnt]["Stock"], dtIndDet.Rows[iRowCnt]["W1_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W1_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W1_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W1_Total_Qty"], dtIndDet.Rows[iRowCnt]["W1_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W1_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W2_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W2_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W2_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W2_Total_Qty"], dtIndDet.Rows[iRowCnt]["W2_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W2_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W3_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W3_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W3_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W3_Total_Qty"], dtIndDet.Rows[iRowCnt]["W3_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W3_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W4_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W4_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W4_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W4_Total_Qty"], dtIndDet.Rows[iRowCnt]["W4_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W4_VECV_Qty"], dtIndDet.Rows[iRowCnt]["W5_Cold_Qty"], dtIndDet.Rows[iRowCnt]["W5_Warm_Qty"], dtIndDet.Rows[iRowCnt]["W5_Hot_Qty"], dtIndDet.Rows[iRowCnt]["W5_Total_Qty"], dtIndDet.Rows[iRowCnt]["W5_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["W5_VECV_Qty"], dtIndDet.Rows[iRowCnt]["M1_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["M1_VECV_Qty"], dtIndDet.Rows[iRowCnt]["M2_Dealer_Qty"], dtIndDet.Rows[iRowCnt]["M2_VECV_Qty"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }




        //To Save IndeentByVECV
        public bool bUpdateIndent(int iHdrID, bool bSubmit, DataTable dtIndDet)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save Details 
                if (bSaveIndentDetails(objDB, dtIndDet, iHdrID) == false) goto ExitFunction;
                //Submit Header record
                if (bSubmit == true)
                {
                    objDB.ExecuteStoredProcedure("SP_DomIndentHdr_Submit", iHdrID, "Y");
                }
                bSaveRecord = true;
            ExitFunction:
                if (bSaveRecord == false)
                {
                    objDB.RollbackTransaction();
                    bSaveRecord = false;
                }
                else
                {
                    objDB.CommitTransaction();
                }
                return bSaveRecord;
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
        // To Get Indent Data For the Dealer
        public DataSet GetDealerIndent(int iID, string sIndentType, int iDealerID, string sDate, int AvgPurRetailValue)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDomIndent", iID, sIndentType, iDealerID, sDate, AvgPurRetailValue);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet GetDepoWiseQTY(int iDepoHdrID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GET_DEPOWISEQTY", iDepoHdrID);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        //To Save IndeentByVECV
        public bool bUpdateDepoIndent(DataTable dtIndDet, string sIsProcess, int iDepoIndentID, int iUserID)
        {

            bool bSaveRecord = false;
            int iIndDetID = 0;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_Save_DepoIndentHdr", iDepoIndentID, sIsProcess, iUserID);
                for (int iRowCnt = 0; iRowCnt < dtIndDet.Rows.Count; iRowCnt++)
                {
                    iIndDetID = Func.Convert.iConvertToInt(dtIndDet.Rows[iRowCnt]["ID"]);
                    if (Func.Convert.sConvertToString(dtIndDet.Rows[iRowCnt]["Status"]) != "D")
                    {
                        if (iIndDetID == 0)
                        {
                            iIndDetID = objDB.ExecuteStoredProcedure("SP_Save_DepoIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], iDepoIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], dtIndDet.Rows[iRowCnt]["D1_SPLQty"], dtIndDet.Rows[iRowCnt]["D2_SPLQty"], dtIndDet.Rows[iRowCnt]["D3_SPLQty"], dtIndDet.Rows[iRowCnt]["D4_SPLQty"], dtIndDet.Rows[iRowCnt]["D5_SPLQty"], dtIndDet.Rows[iRowCnt]["D6_SPLQty"], dtIndDet.Rows[iRowCnt]["D7_SPLQty"], dtIndDet.Rows[iRowCnt]["D8_SPLQty"], dtIndDet.Rows[iRowCnt]["D9_SPLQty"], dtIndDet.Rows[iRowCnt]["D10_SPLQty"], dtIndDet.Rows[iRowCnt]["IsModelAdded"]);

                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_Save_DepoIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], iDepoIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], dtIndDet.Rows[iRowCnt]["D1_SPLQty"], dtIndDet.Rows[iRowCnt]["D2_SPLQty"], dtIndDet.Rows[iRowCnt]["D3_SPLQty"], dtIndDet.Rows[iRowCnt]["D4_SPLQty"], dtIndDet.Rows[iRowCnt]["D5_SPLQty"], dtIndDet.Rows[iRowCnt]["D6_SPLQty"], dtIndDet.Rows[iRowCnt]["D7_SPLQty"], dtIndDet.Rows[iRowCnt]["D8_SPLQty"], dtIndDet.Rows[iRowCnt]["D9_SPLQty"], dtIndDet.Rows[iRowCnt]["D10_SPLQty"], dtIndDet.Rows[iRowCnt]["IsModelAdded"]);
                        }
                    }
                    else
                        objDB.ExecuteStoredProcedure("SP_Delete_DepoIndentDtls", dtIndDet.Rows[iRowCnt]["ID"]);
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
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

        //To Save Final Initial Indent By VECV
        public bool bUpdateFianlInitialIndent(DataTable dtIndDet, string sIsProcess, int icnslIndentID, int iUserID, string sSelectType)
        {

            bool bSaveRecord = false;
            int iIndDetID = 0;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentHdr", icnslIndentID, sIsProcess, iUserID, sSelectType);
                for (int iRowCnt = 0; iRowCnt < dtIndDet.Rows.Count; iRowCnt++)
                {
                    iIndDetID = Func.Convert.iConvertToInt(dtIndDet.Rows[iRowCnt]["ID"]);
                    //if (Func.Convert.sConvertToString(dtIndDet.Rows[iRowCnt]["Status"]) != "D")
                    //{
                    if (iIndDetID == 0)
                    {
                        iIndDetID = objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], dtIndDet.Rows[iRowCnt]["W1_FInlQty"], dtIndDet.Rows[iRowCnt]["W2_FInlQty"], dtIndDet.Rows[iRowCnt]["W3_FInlQty"], dtIndDet.Rows[iRowCnt]["W4_FInlQty"], dtIndDet.Rows[iRowCnt]["W5_FInlQty"], dtIndDet.Rows[iRowCnt]["M1_FinalQty"], dtIndDet.Rows[iRowCnt]["M2_FinalQty"], icnslIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], sSelectType);

                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], dtIndDet.Rows[iRowCnt]["W1_FInlQty"], dtIndDet.Rows[iRowCnt]["W2_FInlQty"], dtIndDet.Rows[iRowCnt]["W3_FInlQty"], dtIndDet.Rows[iRowCnt]["W4_FInlQty"], dtIndDet.Rows[iRowCnt]["W5_FInlQty"], dtIndDet.Rows[iRowCnt]["M1_FinalQty"], dtIndDet.Rows[iRowCnt]["M2_FinalQty"], icnslIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], sSelectType);
                    }
                    //}
                    //else
                    //    objDB.ExecuteStoredProcedure("SP_Delete_DepoIndentDtls", dtIndDet.Rows[iRowCnt]["ID"]);
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
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

        //To Save Revise Indent By VECV
        public bool bUpdateReviseIndent(DataTable dtIndDet, string sIsProcess, int icnslIndentID, int iUserID, string sSelectType)
        {

            bool bSaveRecord = false;
            int iIndDetID = 0;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentHdr", icnslIndentID, sIsProcess, iUserID, sSelectType);
                for (int iRowCnt = 0; iRowCnt < dtIndDet.Rows.Count; iRowCnt++)
                {
                    iIndDetID = Func.Convert.iConvertToInt(dtIndDet.Rows[iRowCnt]["ID"]);
                    //if (Func.Convert.sConvertToString(dtIndDet.Rows[iRowCnt]["Status"]) != "D")
                    //{
                    if (iIndDetID == 0)
                    {
                        iIndDetID = objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], dtIndDet.Rows[iRowCnt]["W1_RevQty"], dtIndDet.Rows[iRowCnt]["W2_RevQty"], dtIndDet.Rows[iRowCnt]["W3_RevQty"], dtIndDet.Rows[iRowCnt]["W4_RevQty"], dtIndDet.Rows[iRowCnt]["W5_RevQty"], dtIndDet.Rows[iRowCnt]["M1_RevQty"], dtIndDet.Rows[iRowCnt]["M2_RevQty"], icnslIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], sSelectType);

                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], dtIndDet.Rows[iRowCnt]["W1_RevQty"], dtIndDet.Rows[iRowCnt]["W2_RevQty"], dtIndDet.Rows[iRowCnt]["W3_RevQty"], dtIndDet.Rows[iRowCnt]["W4_RevQty"], dtIndDet.Rows[iRowCnt]["W5_RevQty"], dtIndDet.Rows[iRowCnt]["M1_RevQty"], dtIndDet.Rows[iRowCnt]["M2_RevQty"], icnslIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], sSelectType);
                    }
                    //}
                    //else
                    //    objDB.ExecuteStoredProcedure("SP_Delete_DepoIndentDtls", dtIndDet.Rows[iRowCnt]["ID"]);
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
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

        //To Save Supply Plan Indent By VECV
        public bool bUpdateSupplyPlanIndent(DataTable dtIndDet, string sIsProcess, int iSPIndentID, int iUserID, string sSelectType)
        {

            bool bSaveRecord = false;
            int iIndDetID = 0;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentHdr", iSPIndentID, sIsProcess, iUserID, sSelectType);
                for (int iRowCnt = 0; iRowCnt < dtIndDet.Rows.Count; iRowCnt++)
                {
                    iIndDetID = Func.Convert.iConvertToInt(dtIndDet.Rows[iRowCnt]["ID"]);
                    //if (Func.Convert.sConvertToString(dtIndDet.Rows[iRowCnt]["Status"]) != "D")
                    //{
                    if (iIndDetID == 0)
                    {
                        iIndDetID = objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], dtIndDet.Rows[iRowCnt]["W1_SPQty"], dtIndDet.Rows[iRowCnt]["W2_SPQty"], dtIndDet.Rows[iRowCnt]["W3_SPQty"], dtIndDet.Rows[iRowCnt]["W4_SPQty"], dtIndDet.Rows[iRowCnt]["W5_SPQty"], 0, 0, iSPIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], sSelectType);

                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveDOMSplPlanReviseIndentDtls", dtIndDet.Rows[iRowCnt]["ID"], dtIndDet.Rows[iRowCnt]["W1_SPQty"], dtIndDet.Rows[iRowCnt]["W2_SPQty"], dtIndDet.Rows[iRowCnt]["W3_SPQty"], dtIndDet.Rows[iRowCnt]["W4_SPQty"], dtIndDet.Rows[iRowCnt]["W5_SPQty"], 0, 0, iSPIndentID, dtIndDet.Rows[iRowCnt]["Model_ID"], sSelectType);
                    }
                    //}
                    //else
                    //    objDB.ExecuteStoredProcedure("SP_Delete_DepoIndentDtls", dtIndDet.Rows[iRowCnt]["ID"]);
                }
                objDB.CommitTransaction();
                bSaveRecord = true;
                return bSaveRecord;
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

        public DataTable GetIndentBlockDate(string SelectType)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = new DataTable();
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetIndentBlockDate", SelectType);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
