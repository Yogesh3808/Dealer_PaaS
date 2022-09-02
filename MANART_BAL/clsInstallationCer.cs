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
    /// Summary description for clsInstallationCer
    /// </summary>
    public class clsInstallationCer
    {
        private string sFincYear = "";

        //public bool bUseSpareDealerCode
        //{
        //    get { return _bUseSpareDealerCode; }
        //    set { _bUseSpareDealerCode = value; }
        //}

        public clsInstallationCer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetInstallationCer(int iID, int iDealerID, string sSelectionType)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_InstallationCer", iID, iDealerID, sSelectionType);
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

        public DataSet GetINSProcess(int iID, string sDealerID, string sSelectionType, string sStatus, string ExportDomestic, string FromDate, string ToDate)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetINSForProcessing", iID, sDealerID, sStatus, sSelectionType, ExportDomestic, FromDate, ToDate);
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

        public bool bsaveInstallationCer(DataTable dtDet)
        {
            string sFinYear = Func.sGetFinancialYear(Func.Convert.iConvertToInt(dtDet.Rows[0]["Dealer_Id"]));
            int iID = Func.Convert.iConvertToInt(dtDet.Rows[0]["ID"]);
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_InstallationCer_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["VehicleIn_Dtl_ID"], dtDet.Rows[0]["Dealer_Id"], dtDet.Rows[0]["Customer_Name"], dtDet.Rows[0]["INS_No"], dtDet.Rows[0]["INS_Date"], dtDet.Rows[0]["Customer_Addr"], dtDet.Rows[0]["Country_ID"], dtDet.Rows[0]["City"], dtDet.Rows[0]["Ph_No"], dtDet.Rows[0]["Email"], dtDet.Rows[0]["Cust_Type"], dtDet.Rows[0]["Drive_Type"], dtDet.Rows[0]["Primary_Application"], dtDet.Rows[0]["Secondary_Application"], dtDet.Rows[0]["Road_Type"], dtDet.Rows[0]["Financir_Type"], dtDet.Rows[0]["Load_Type"], dtDet.Rows[0]["Route_Type"], dtDet.Rows[0]["Vehicle_No"], dtDet.Rows[0]["Industry_Type"]);
                    if (iID == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "INS", 0);
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
            sINSNo = "1100" + sDocName + sMaxINSNo;
            return sINSNo;
        }

        public bool bsaveINSProcess(DataTable dtDet, string sIsProcessed)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (dtDet.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_INSProcess_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Cust_Type"], dtDet.Rows[0]["Drive_Type"], dtDet.Rows[0]["Primary_Application"], dtDet.Rows[0]["Secondary_Application"], dtDet.Rows[0]["Road_Type"], dtDet.Rows[0]["Financir_Type"], dtDet.Rows[0]["Load_Type"], dtDet.Rows[0]["Route_Type"], dtDet.Rows[0]["Industry_Type"], dtDet.Rows[0]["ProcessedBy"], sIsProcessed, dtDet.Rows[0]["ExportDom"]);
                    //Sujata 04022011
                    if (sIsProcessed == "Y")
                    {
                        objDB.ExecuteStoredProcedure("SP_ChassisCust_Update", dtDet.Rows[0]["ID"]);
                    }
                    //Sujata 04022011
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
    }
}
