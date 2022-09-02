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
    /// Summary description for clsMRN
    /// </summary>
    public class clsMRN
    {
        public clsMRN()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //Sujata 25012011 
        //public DataSet GetMRN(int iID, string sSelectionType, string sStatus)
        public DataSet GetMRN(string iID, string sSelectionType, string sStatus, int Role_ID, string FromDate, string ToDate)
        //Sujata 25012011 
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMRN", iID, sSelectionType, sStatus, Role_ID, FromDate, ToDate);
                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
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

        public int GetChallanIdForMRN(int iID)
        {
            int ichallanID = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteQueryAndGetDataTable("select TM_MRNChallan.ID as ID from TM_MRNRequest   inner join  TM_MRNChallan on TM_MRNRequest.mrn_no=TM_MRNChallan.Mrn_no where TM_MRNRequest.ID=" + iID);
                if (dt != null && dt.Rows.Count > 0)
                    ichallanID = Func.Convert.iConvertToInt(dt.Rows[0]["ID"]);
                return ichallanID;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool bSaveMRN(DataTable dtDet, int Role_ID, string sConfirmStatus)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        //Sujata 25012011
                        //objDB.ExecuteStoredProcedure("SP_MRN_Save", dtDet.Rows[iRowCnt]["MRN_Hdr_ID"], dtDet.Rows[iRowCnt]["Approved_No"], dtDet.Rows[iRowCnt]["Approved_Date"], dtDet.Rows[iRowCnt]["Approved_Name"], dtDet.Rows[iRowCnt]["Remark"], dtDet.Rows[iRowCnt]["Confirmed"], dtDet.Rows[iRowCnt]["MRN_Dtl_ID"], dtDet.Rows[iRowCnt]["Approved_qty"], dtDet.Rows[iRowCnt]["Rejection_Res"], dtDet.Rows[iRowCnt]["Approved_Status"], dtDet.Rows[iRowCnt]["MRNRequest_Status"]);
                        objDB.ExecuteStoredProcedure("SP_MRN_Save", dtDet.Rows[iRowCnt]["MRN_Hdr_ID"], dtDet.Rows[iRowCnt]["Approved_No"], dtDet.Rows[iRowCnt]["Approved_Date"], dtDet.Rows[iRowCnt]["Approved_Name"], dtDet.Rows[iRowCnt]["Remark"], sConfirmStatus, dtDet.Rows[iRowCnt]["MRN_Dtl_ID"], dtDet.Rows[iRowCnt]["Approved_qty"], dtDet.Rows[iRowCnt]["Rejection_Res"], dtDet.Rows[iRowCnt]["Approved_Status"], dtDet.Rows[iRowCnt]["MRNRequest_Status"], dtDet.Rows[iRowCnt]["Head_Remark"], dtDet.Rows[iRowCnt]["RSM_Remark"], dtDet.Rows[iRowCnt]["CSM_Remark"], dtDet.Rows[iRowCnt]["ASM_Remark"], Role_ID, dtDet.Rows[iRowCnt]["UserID"]);
                        //Sujata 25012011
                    }
                    objDB.CommitTransaction();
                    bSaveRecord = true;
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
        public static string sGetMaxDocNo(string sDocName)
        {
            string sFinYear = "";
            string sINSNo = "";
            int iMaxDocNo = 0;
            string sMaxINSNo = "";
            if (sFinYear == "")
            {
                sFinYear = Func.sGetFinancialYear(9999);
            }
            iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, 0);
            sMaxINSNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
            sMaxINSNo = sMaxINSNo.PadLeft(4, '0');
            sINSNo = sDocName + sFinYear + sMaxINSNo;
            return sINSNo;
        }
    }
}
