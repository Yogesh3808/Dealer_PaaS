using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsHDTrialAllocation
    /// </summary>
    public class clsHDTrialAllocation
    {
        public clsHDTrialAllocation()
        {
            //
            // TODO: Add constructor logic here
            //

        }
        public DataSet GetHDTrialReq(string sSelType, int HDTReqID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHDTrialRequest", sSelType, 0, 0, 0, 0, "", "", HDTReqID);
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
        public DataSet GetHDTrialAllocation(string sSelType, int TAllocationID, string AllocationStartDate, string AllocationEndDate, int HDTReqID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHDTrialAllocation", sSelType, TAllocationID, AllocationStartDate, AllocationEndDate, HDTReqID);
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
        public bool bSaveHDTrialAllocation(ref int iHdrID, DataTable dtHdr)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHDTrialAllocation(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

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
        private bool bSaveHDTrialAllocation(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                string sFinYear = Func.sGetFinancialYear(9999);
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_HDTrialAllocation_Save", Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Allocation_No"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Allocation_Date"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["TrialRequest_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Chassis_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["ModelGrp_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_Start_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_End_Date"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["User_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]));
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "HDTAL", -1);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_HDTrialAllocation_Save", Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Allocation_No"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Allocation_Date"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["TrialRequest_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Chassis_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["ModelGrp_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_Start_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_End_Date"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["User_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]));
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
