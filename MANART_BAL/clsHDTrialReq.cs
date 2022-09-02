using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsHDTrialReq
    /// </summary>
    public class clsHDTrialReq
    {
        public clsHDTrialReq()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetHDTrialReq(string sSelType, int Region_ID, int OtherRegion_ID, int User_ID, int ModelGrp_ID, string RequestStartDate, string RequestEndDate, int HDTReqID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHDTrialRequest", sSelType, Region_ID, OtherRegion_ID, User_ID, ModelGrp_ID, RequestStartDate, RequestEndDate, HDTReqID);
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
        public DataSet GetNewTrialReqDate(string sSelType, int Region_ID, int User_ID, int ModelGrp_ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetNewTrialReqDate", sSelType, Region_ID, User_ID, ModelGrp_ID);
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
        public bool bSaveHDTrialRequest(ref int iHdrID, DataTable dtHdr, DataTable dtDet, DataTable dtObjDet, DataTable dtObjTRC)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHDTrialRequest(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Fleet Details
                if (bSaveHDFleetDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                // Save Objective Details
                if (bSaveHDObjectiveDetails(objDB, dtObjDet, iHdrID) == false) goto ExitFunction;

                // Save Trial Request Chassis Details
                if (bSaveHDTrialRequestChassisDetails(objDB, dtObjTRC, iHdrID) == false) goto ExitFunction;

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
        private bool bSaveHDTrialRequest(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                string sFinYear = Func.sGetFinancialYear(9999);
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_HDTrialRequest_Save", Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Request_No"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Request_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Request_By"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Competition_Model"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Approved_By"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Ref_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_ReSubmitReq"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Object_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Cust_Name"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Location"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Application_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_Start_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_End_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Purch_Plan"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Purch_Horizon_Timeline"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Region_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["OtherRegion_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["ModelGrp_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["User_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Trial_Duration"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_Supervisor"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["File_Name"]));
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "HDTR", -1);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_HDTrialRequest_Save", Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Request_No"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Request_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Request_By"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Competition_Model"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Approved_By"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Ref_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_ReSubmitReq"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Object_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Cust_Name"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Location"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Application_ID"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_Start_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_End_Date"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Purch_Plan"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Purch_Horizon_Timeline"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Region_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["OtherRegion_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["ModelGrp_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["User_ID"]), Func.Convert.iConvertToInt(dtHdr.Rows[0]["Trial_Duration"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Trial_Supervisor"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]), Func.Convert.sConvertToString(dtHdr.Rows[0]["File_Name"]));
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveHDFleetDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_HDTRFleetDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Fleet_ID"], dtDet.Rows[iRowCnt]["Count"]);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        private bool bSaveHDObjectiveDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_HDTRObjectiveDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Obj_ID"], dtDet.Rows[iRowCnt]["Accept"]);
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        private bool bSaveHDTrialRequestChassisDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //Sujata 29112014
                    //objDB.ExecuteStoredProcedure("SP_HDTRChassisDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Chassis_ID"]);
                    if (dtDet.Rows[iRowCnt]["ReqSel"] == "Y")
                        objDB.ExecuteStoredProcedure("SP_HDTRChassisDetails_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Chassis_ID"]);
                    else
                        objDB.ExecuteStoredProcedure("SP_HDTRChassisDetails_Del", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Chassis_ID"]);
                    //Sujata 29112014
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
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

        public DataTable FillHDTrialModel(int ChassisID, string sSourceFrom)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            DataTable dt;
            try
            {

                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_HDTrialModel", ChassisID, sSourceFrom);
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
        public bool bApproveHDTrialRequest(int iID, string Status, string ReasonID, string sApprovedBy)
        {

            clsDB objDB = null;
            try
            {
                objDB = new clsDB();
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_HDTrialRequest_Approve", iID, Status, Func.Convert.iConvertToInt(ReasonID), sApprovedBy);
                objDB.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }


    }
}
