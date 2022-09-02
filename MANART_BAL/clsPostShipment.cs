using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsPostShipment
    /// </summary>
    public class clsPostShipment
    {
        public clsPostShipment()
        {
        }
        // To Save PostShipment Header record
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "";
            string sModel_Part = Func.Convert.sConvertToString(dtHdr.Rows[0]["Model_Part"]);
            if (sModel_Part == "M")
            {
                sDocName = "VPOSI";
            }
            else if (sModel_Part == "P")
            {
                sDocName = "SPOSI";
            }
            //Save Header
            try
            {
                iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                if (iHdrID == 0)
                {                    
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    //dtHdr.Rows[0]["PostShipment_Inv_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDelearId);
                    //iHdrID = objDB.ExecuteStoredProcedure("SP_PostShipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PostShipment_Inv_No"], dtHdr.Rows[0]["PostShipment_Inv_Date"], dtHdr.Rows[0]["PreShipment_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["PostShipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["PreSInvNo"]);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_PostShipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PostShipment_Inv_No"], dtHdr.Rows[0]["PostShipment_Inv_Date"], dtHdr.Rows[0]["PreShipment_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["PostShipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["PreSInvNo"], dtHdr.Rows[0]["Comformity"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["PostShipment_InvRef_No"], dtHdr.Rows[0]["Delivery_Challan_Date1"]);
                    //if (iHdrID == 0)
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDelearId);

                }
                else
                {
                    // objDB.ExecuteStoredProcedure("SP_PostShipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PostShipment_Inv_No"], dtHdr.Rows[0]["PostShipment_Inv_Date"], dtHdr.Rows[0]["PreShipment_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["PostShipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["PreSInvNo"]);
                    objDB.ExecuteStoredProcedure("SP_PostShipmentHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PostShipment_Inv_No"], dtHdr.Rows[0]["PostShipment_Inv_Date"], dtHdr.Rows[0]["PreShipment_ID"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Model_Part"], dtHdr.Rows[0]["PostShipment_Confirm"], dtHdr.Rows[0]["Narration"], dtHdr.Rows[0]["TotalAmount"], dtHdr.Rows[0]["GrandAmount"], dtHdr.Rows[0]["PreSInvNo"], dtHdr.Rows[0]["Comformity"], dtHdr.Rows[0]["TotalIncoAmount"], dtHdr.Rows[0]["PostShipment_InvRef_No"], dtHdr.Rows[0]["Delivery_Challan_Date1"]);
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //To Save Model Record
        public bool bSaveRecord(string sDealerCode, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save Header
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details 
                if (bSaveDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
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

        private bool bSaveDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_PreShipmentDet_Update", dtDet.Rows[iRowCnt]["PreShipment_Det_ID"], iHdrID);
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        /// <summary>    
        /// PostShipmentId else '0' 
        /// sPostShipmentType 'All' all PostShipment HDR +Det,'Confirm' get Confirm PostShipment ,'Max' Max Record, 'LC' 
        /// PostShipment_Model_Part "M' for Model, "P" for Part, else ""
        /// </summary>    
        public DataSet GetPostShipment(int iPostShipmentID, string sPostShipmentType, string sPostShipment_Model_Part, int iDealerID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = null;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPostShipment", iPostShipmentID, sPostShipmentType, sPostShipment_Model_Part, iDealerID);
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

    }
}
