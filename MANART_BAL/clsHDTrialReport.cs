using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{

    /// <summary>
    /// Summary description for clsHDTrialReport
    /// </summary>
    public class clsHDTrialReport
    {
        public clsHDTrialReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetHDTrialReport(string sSelType, int HDTReqID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHDTrialReport", sSelType, HDTReqID);
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
        public bool bSaveHDTrialReport(ref int iHdrID, DataTable dtHdr, DataTable dtFileAttach)//, DataTable dtDet
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHDTrialReport(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                //if (bSaveHDFleetDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                // Save Attachment details
                if (bSaveHDTReportFileAttachDetails(objDB, dtFileAttach, iHdrID) == false) goto ExitFunction;

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
        public DataTable FillRegion(int iUserID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            DataTable dt;
            try
            {

                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_RegionByUserID", iUserID);
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

        private bool bSaveHDTrialReport(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                string sFinYear = Func.sGetFinancialYear(9999);
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    //,,,,,,
                    //,,,,, 	

                    iHdrID = objDB.ExecuteStoredProcedure("SP_HDTrialReport_Save", Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Report_No"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Report_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["AllocationHDRID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["VehicleMovedfrom"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["LocationSite"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["VehicleCondition"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["EstRepairVal"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["TerrainGradeability"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["KmsIn"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["KmsOut"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["Kmsduringtrial"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["NoOfHours"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["TrialInchargeETB"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["TrialInchargeCust"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Chassis_Location"]));
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "HDTRPT", -1);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_HDTrialReport_Save", Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Report_No"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Report_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["AllocationHDRID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["VehicleMovedfrom"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["LocationSite"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["VehicleCondition"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["EstRepairVal"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["TerrainGradeability"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["KmsIn"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["KmsOut"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["Kmsduringtrial"]), Func.Convert.dConvertToDouble(dtHdr.Rows[0]["NoOfHours"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["TrialInchargeETB"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["TrialInchargeCust"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Chassis_Location"]));
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //private bool bSaveHDFleetDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        //{
        //    bool bSaveRecord = false;
        //    try
        //    {
        //        for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
        //        {
        //            objDB.ExecuteStoredProcedure("SP_HDTRFleetDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Fleet_ID"], dtDet.Rows[iRowCnt]["Count"]);
        //        }
        //        bSaveRecord = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return bSaveRecord;
        //}

        private bool bSaveHDTReportFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
        {
            bool bSaveRecord = false;
            int iAttachDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iAttachDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iAttachDetID == 0)
                        {
                            objDB.ExecuteStoredProcedure("SP_HDTrialReport_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Name"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_HDTrialReport_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Name"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_HDTrialReport_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
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
