using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;
using System.Threading;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsHDTrialUploadChassis
    /// </summary>
    public class clsHDTrialUploadChassis
    {
        public clsHDTrialUploadChassis()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //To Save Model Record
        public bool bSaveRecord(DataTable dtDet, string QUERY)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();

                // Save Details
                if (bSaveDetails(objDB, dtDet, QUERY) == false) goto ExitFunction;
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
        private bool bSaveDetails(clsDB objDB, DataTable dtDet, string QUERY)
        {
            //saveDetails
            int Chassis_ID = 0;
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //Megha23062011
                    if (dtDet.Rows[iRowCnt]["Chassis_No"].ToString() != "")
                    {
                        //Chassis_ID = objDB.ExecuteQuery("SELECT ID FROM M_ChassisMaster where Chassis_no='" + dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim() + "' ");
                        Chassis_ID = objDB.ExecuteStoredProcedure("SP_GetChassis_ActiveModel", dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim());
                        if (Chassis_ID > 0)
                        {
                            if (QUERY == "HDChassis")
                            {
                                objDB.ExecuteStoredProcedure("SP_SAVEHDChassis", Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Engine_No"].ToString().Trim()), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["HDTrialFrom_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["HDTrialTo_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Region"].ToString().Trim()), dtDet.Rows[iRowCnt]["Vehicle_Upload_Date"]);
                            }
                        }

                    }
                    //Megha23062011
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
        }
        public DataSet GetChassisHDTrial()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHDChassis", 0);
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
        public DataSet NotUploadedChassisDetails(string sFileName, DataTable dtDet, string QUERY)
        {

            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                if (QUERY == "HDChassis")
                {

                    if (bSaveUploadedChassisDetails(sFileName, dtDet, "HDChassis") == true)
                    {

                        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportHDChassisDetailsFromExcelSheet", sFileName);

                    }

                }
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

        private bool bSaveUploadedChassisDetails(string sFileName, DataTable dtDet, string QUERY)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            string bNewTable = "Y";
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                if (QUERY == "HDChassis")
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_InsertHDChassisDetailsFromExcelSheet", sFileName, Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Engine_no"].ToString().Trim()), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["HDTrialFrom_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["HDTrialTo_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Region"].ToString().Trim()), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["Vehicle_Upload_Date"], true), bNewTable);
                        bNewTable = "N";
                    }

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
    }
}
