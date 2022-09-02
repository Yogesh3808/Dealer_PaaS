using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsServiceIndex
    /// </summary>
    public class clsServiceIndex
    {
        public clsServiceIndex()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool bSaveSI(ref int iHdrID, DataTable dtHdr, DataTable dtDet, int SIType)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveSIndexHdr(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                if (SIType == 1)
                    if (bSaveSIndexDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
                if (SIType == 2)
                    if (bSaveSFDealerDetails(objDB, dtDet, iHdrID, SIType) == false) goto ExitFunction;
                if (SIType == 3)
                    if (bSaveSFDistributorDetails(objDB, dtDet, iHdrID, SIType) == false) goto ExitFunction;

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


        private bool bSaveSIndexHdr(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                string sFinYear = Func.sGetFinancialYear(9999);
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {

                    iHdrID = objDB.ExecuteStoredProcedure("SP_ServiceIndexHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Document No"], dtHdr.Rows[0]["Document Date"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["Index_Type"], dtHdr.Rows[0]["File_Name"]);

                    Func.Common.UpdateMaxNo(objDB, sFinYear, "SINDX", -1);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_ServiceIndexHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Document No"], dtHdr.Rows[0]["Document Date"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["Index_Type"], dtHdr.Rows[0]["File_Name"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveSFDealerDetails(clsDB objDB, DataTable dtDet, int iHdrID, int SIType)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_SISFDlrDistributorDtls_Save", 0, iHdrID, dtDet.Rows[iRowCnt]["Region"], dtDet.Rows[iRowCnt]["Dealer Code"], dtDet.Rows[iRowCnt]["Part Code"], dtDet.Rows[iRowCnt]["Stock Qty"], dtDet.Rows[iRowCnt]["Dealer 15 days Avg Dmd"], dtDet.Rows[iRowCnt]["No of Picks / Year"], dtDet.Rows[iRowCnt]["Service Index - Distributor"], dtDet.Rows[iRowCnt]["Shortfall Qnty"], SIType);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        private bool bSaveSFDistributorDetails(clsDB objDB, DataTable dtDet, int iHdrID, int SIType)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_SISFDlrDistributorDtls_Save", 0, iHdrID, dtDet.Rows[iRowCnt]["Region"], dtDet.Rows[iRowCnt]["Distributor Code"], dtDet.Rows[iRowCnt]["Part Code"], dtDet.Rows[iRowCnt]["Stock Qty"], dtDet.Rows[iRowCnt]["Dist 7 days Avg Dmd"], dtDet.Rows[iRowCnt]["No of Picks / Year"], dtDet.Rows[iRowCnt]["Service Index - Distributor"], dtDet.Rows[iRowCnt]["Shortfall Qnty"], SIType);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        private bool bSaveSIndexDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_ServiceIndexDtls_Save", 0, iHdrID, dtDet.Rows[iRowCnt]["Region"], dtDet.Rows[iRowCnt]["Dealer/DistributorCode"], dtDet.Rows[iRowCnt]["Month"], dtDet.Rows[iRowCnt]["Year"], dtDet.Rows[iRowCnt]["Service Index - Distributor"]);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }
    }
}
