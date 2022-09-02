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
using System.Threading;

namespace MANART_BAL
{

    /// <summary>
    /// Summary description for clsChassis
    /// </summary>
    public class clsChassis
    {
        public clsChassis()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //public DataSet GetChassis(string ChassisNo, string EngineNo, string FertCode, string ModelGroup,string ModelCategory,string CustomerName)
        //{
        //    DataSet ds;
        //    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Proforma", ChassisNo, EngineNo, FertCode, ModelGroup, ModelCategory, CustomerName);
        //    return ds;
        //}
        public DataSet GetChassis(int ChassisID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("ListAll_Chassis", ChassisID);
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

        public DataSet GETServiceHistoryForService(int DealerID, string JobcardNo)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GETServiceHistoryForService", DealerID, JobcardNo);
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
        public DataSet GetMaxChassis(int ChassisID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("GetMax_Chassis", 0);
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
                            if (QUERY == "AMC")
                            {
                                objDB.ExecuteStoredProcedure("SP_SAVEChassisAMC", Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_Type"]), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["AMC_Start_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["AMC_End_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_Agreement_No"]), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["AMC_Agreement_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_Start_KM"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_End_KM"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Inclusion"]));
                                //objDB.ExecuteStoredProcedure("SP_SAVEChassisAMC", dtDet.Rows[iRowCnt]["Chassis_No"], dtDet.Rows[iRowCnt]["AMC_Type"], dtDet.Rows[iRowCnt]["AMC_Start_Date"], dtDet.Rows[iRowCnt]["AMC_End_Date"], dtDet.Rows[iRowCnt]["AMC_Agreement_No"], dtDet.Rows[iRowCnt]["AMC_Agreement_Date"], dtDet.Rows[iRowCnt]["AMC_Start_KM"], dtDet.Rows[iRowCnt]["AMC_End_KM"]);
                            }
                            else if (QUERY == "KAM")
                            {
                                objDB.ExecuteStoredProcedure("SP_SAVEChassisKAM", Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), dtDet.Rows[iRowCnt]["Engine_No"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["KAM_Start_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["KAM_End_Date"], true), dtDet.Rows[iRowCnt]["Customer_Name"], dtDet.Rows[iRowCnt]["Address"], dtDet.Rows[iRowCnt]["Phone_No"], dtDet.Rows[iRowCnt]["Vehicle_desc"]);
                            }
                            else if (QUERY == "UnderObservation")
                            {
                                objDB.ExecuteStoredProcedure("SP_SAVEChassisUnderObservation", Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), dtDet.Rows[iRowCnt]["Description"], dtDet.Rows[iRowCnt]["UnderObservation_Start_Date"], dtDet.Rows[iRowCnt]["UnderObservation_End_Date"]);
                            }
                            else if (QUERY == "HDChassis")
                            {
                                objDB.ExecuteStoredProcedure("SP_SAVEHDChassis", Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), dtDet.Rows[iRowCnt]["Vehicle_Upload_Date"]);
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

        public DataSet GetChassisAMC()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChassisAMC", 0);
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
        public DataSet GetChassisKAM()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChassisKAM", 0);
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
        public DataSet GetChassisUnderObservation()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChassisUnderObservation", 0);
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
        public DataSet GetHDChassis()
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
        //public DataSet GetChassisAMCSearch(string ActualData, int Selectedvalue)
        //{
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet ds;
        //        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChassisAMCSearch", ActualData, Selectedvalue);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}

        //public DataSet GetChassisUnderObservationSearch(string ActualData, int Selectedvalue)
        //{
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet ds;
        //        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChassisUnderObservationSearch", ActualData, Selectedvalue);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}

        public DataSet NotUploadedChassisDetails(string sFileName, DataTable dtDet, string QUERY)
        {

            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                // Commented by Shyamal on 05042012 BeginTranasaction not needed here
                //objDB.BeginTranasaction();
                if (QUERY == "AMC")
                {
                    if (bSaveUploadedChassisDetails(sFileName, dtDet, "AMC") == true)
                    {

                        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportChassisAMCDetailsFromExcelSheet", sFileName);

                    }
                }
                else if (QUERY == "UnderObservation")
                {

                    if (bSaveUploadedChassisDetails(sFileName, dtDet, "UnderObservation") == true)
                    {

                        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportChassisUnderObservationDetailsFromExcelSheet", sFileName);

                    }

                }
                else if (QUERY == "KAM")
                {

                    if (bSaveUploadedChassisDetails(sFileName, dtDet, "KAM") == true)
                    {

                        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportChassisKAMDetailsFromExcelSheet", sFileName);

                    }

                }
                else if (QUERY == "HDChassis")
                {

                    if (bSaveUploadedChassisDetails(sFileName, dtDet, "HDChassis") == true)
                    {

                        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportHDChassisDetailsFromExcelSheet", sFileName);

                    }

                }

                // Commented by Shyamal on 05042012 RollbackTransaction not needed here
                //else
                //{
                //    objDB.RollbackTransaction();
                //}
                return ds;
            }
            // Commented by Shyamal on 05042012 RollbackTransaction not needed here
            //catch (Exception ex)
            //{
            //    // Commented by Shyamal on 05042012 RollbackTransaction not needed here
            //    //objDB.RollbackTransaction();
            //    return ds;
            //}       
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
                if (QUERY == "AMC")
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_InsertChassisAMCDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["Chassis_No"], Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_Type"]), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["AMC_Start_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["AMC_End_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_Agreement_No"]), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["AMC_Agreement_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_Start_KM"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["AMC_End_KM"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Inclusion"]), bNewTable);
                        bNewTable = "N";
                    }
                }
                else if (QUERY == "UnderObservation")
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_InsertChassisUnderObservationDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["Chassis_No"], Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Description"]), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["UnderObservation_Start_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["UnderObservation_End_Date"], true), bNewTable);
                        bNewTable = "N";
                    }
                }
                else if (QUERY == "KAM")
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_InsertChassisKAMDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["Chassis_No"], Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Engine_No"]), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["KAM_Start_Date"], true), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["KAM_End_Date"], true), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Customer_Name"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Address"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Phone_No"]), Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Vehicle_desc"]), bNewTable);
                        bNewTable = "N";
                    }

                }
                else if (QUERY == "HDChassis")
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_InsertHDChassisDetailsFromExcelSheet", sFileName, Func.Convert.sConvertToString(dtDet.Rows[iRowCnt]["Chassis_No"].ToString().Trim()), Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["Vehicle_Upload_Date"], true), bNewTable);
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
        //Megha13062012
        public DataSet GetFertCodeAndServicePolicy(string FertCode)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetFertCodeDetails", FertCode);
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
        public bool bSaveFertCodeDetails(int chassisID, int ModelID, string SAPSTNNo, string SAPSTNDate, string RegNo)
        {
            //saveDetails
            // int Chassis_ID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_SAVEFertCodeDetails", chassisID, ModelID, SAPSTNNo, SAPSTNDate, RegNo);

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
        public bool bGenearteChassisXML(int chassisID)
        {
            //saveDetails
            // int Chassis_ID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_GENXML_M_ChassisMaster_Singlechassis", chassisID);
                Thread.Sleep(1000);
                objDB.ExecuteStoredProcedure("SP_GENXML_M_ChassisCoupon_Singlechassis", chassisID);

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

        public DataSet GetchassisID(string chassisNo)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetGetchassisID", chassisNo);
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
        public DataSet GetDealerforChassisMaster()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("GetDealerforChassisMaster");
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
        //Megha13062012

        public bool bUpdateCouponDetails(int CouponID, string CouponNo, string LSPSD, string MSPSD)
        {
            //saveDetails
            // int Chassis_ID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_UpdateCouponDetails", CouponID, CouponNo, LSPSD, MSPSD);

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
