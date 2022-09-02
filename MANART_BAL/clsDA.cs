using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;

namespace MANART_BAL
{/// <summary>
    /// Summary description for clsDA
    /// </summary>
    public class clsDA
    {
        public clsDA()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetDADetails(int iDealerID, string sGetDataFrom)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                return objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_DADetails", iDealerID, sGetDataFrom);
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


        public bool bCreateDA(string sDealerCode, int iDealerID, DataTable dtDADetails)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDADetails.Rows.Count; iRowCnt++)
                {
                    if (dtDADetails.Rows[iRowCnt]["Status"] == "I")
                    {
                        iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, "DA", iDealerID);
                        sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                        sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                        //Sujata 01032011
                        //sDocNo = sDealerCode.Substring(0, 4) + "I" + sFinYearChar + sMaxDocNo;
                        sDocNo = sDealerCode.Substring(2, 4) + "I" + sFinYearChar + sMaxDocNo;
                        //Sujata 01032011
                        objDB.ExecuteStoredProcedure("SP_Save_DA", dtDADetails.Rows[iRowCnt]["ID"], sDealerCode, "00", iDealerID, sDocNo, dtDADetails.Rows[iRowCnt]["DA_Date"], dtDADetails.Rows[iRowCnt]["Inq_no"], dtDADetails.Rows[iRowCnt]["Inq_date"], dtDADetails.Rows[iRowCnt]["Cust_name"], dtDADetails.Rows[iRowCnt]["Model_ID"], dtDADetails.Rows[iRowCnt]["Cust_name"], dtDADetails.Rows[iRowCnt]["Model_code"]);
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "DA", iDealerID);
                    }
                }
                objDB.CommitTransaction();
                return true;
            }
            catch
            {
                objDB.RollbackTransaction();
                return false;
            }
        }

        //Megha24062011
        // Function 'bSaveDAOld' Commented by Shyamal on 05042012 bcoz not used any where
        //public bool bSaveDAOld(DataTable dtDAtDetSave)
        //{
        //    try
        //    {
        //        string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear();
        //        int iMaxDocNo,iDealerID;
        //        sFinYearChar = sFinYear.Substring(3);
        //        Func.DB.BeginTranasaction();
        //        //for (int iRowCnt = 0; iRowCnt < dtDADetails.Rows.Count; iRowCnt++)
        //        //{
        //        //        Func.DB.ExecuteStoredProcedure("SP_Save_DA", dtDADetails.Rows[iRowCnt]["ID"], sDealerCode, "00", iDealerID, sDocNo, dtDADetails.Rows[iRowCnt]["DA_Date"], dtDADetails.Rows[iRowCnt]["Inq_no"], dtDADetails.Rows[iRowCnt]["Inq_date"], dtDADetails.Rows[iRowCnt]["Cust_name"], dtDADetails.Rows[iRowCnt]["Model_ID"], dtDADetails.Rows[iRowCnt]["Cust_name"], dtDADetails.Rows[iRowCnt]["Model_code"]);
        //        //        Func.Common.UpdateMaxNo(sFinYear, "DA", iDealerID);

        //        //}
        //        for (int cnt = 0; cnt < dtDAtDetSave.Rows.Count; cnt++)
        //        {
        //            iDealerID = Func.Convert.iConvertToInt(dtDAtDetSave.Rows[cnt]["DealerID"]);
        //            Func.DB.ExecuteStoredProcedureAndGetObject("sp_Save_DACreation", Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["DealerCode"]), Func.Convert.iConvertToInt(dtDAtDetSave.Rows[cnt]["DealerID"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["DANo"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["DADate"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["ModelCode"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["VECVFlag"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["IsProcessed"]),Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["BillingType"]),Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["PlantID"]),Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["DepotID"]));
        //            Func.Common.UpdateMaxNo(sFinYear, "DA", iDealerID);
        //        }
        //        Func.DB.CommitTransaction();
        //        return true;
        //    }
        //    catch
        //    {
        //        Func.DB.RollbackTransaction();
        //        return false;
        //    }
        //}

        public bool bSaveDA(DataTable dtDAtDetSave, string VECVFlag, string ISProcessed, string BillingType, string PlantID, int DepotID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(0);
                int iMaxDocNo, iDealerID;
                sFinYearChar = sFinYear.Substring(3);
                objDB.BeginTranasaction();
                //for (int iRowCnt = 0; iRowCnt < dtDADetails.Rows.Count; iRowCnt++)
                //{
                //        objDB.ExecuteStoredProcedure("SP_Save_DA", dtDADetails.Rows[iRowCnt]["ID"], sDealerCode, "00", iDealerID, sDocNo, dtDADetails.Rows[iRowCnt]["DA_Date"], dtDADetails.Rows[iRowCnt]["Inq_no"], dtDADetails.Rows[iRowCnt]["Inq_date"], dtDADetails.Rows[iRowCnt]["Cust_name"], dtDADetails.Rows[iRowCnt]["Model_ID"], dtDADetails.Rows[iRowCnt]["Cust_name"], dtDADetails.Rows[iRowCnt]["Model_code"]);
                //        Func.Common.UpdateMaxNo(sFinYear, "DA", iDealerID);

                //}
                for (int cnt = 0; cnt < dtDAtDetSave.Rows.Count; cnt++)
                {
                    iDealerID = Func.Convert.iConvertToInt(dtDAtDetSave.Rows[cnt]["Dealer ID"]);
                    objDB.ExecuteStoredProcedureAndGetObject("sp_Save_DACreation", Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["Dealer Code"]), Func.Convert.iConvertToInt(dtDAtDetSave.Rows[cnt]["Dealer ID"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["DA No"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["DA Date"]), Func.Convert.sConvertToString(dtDAtDetSave.Rows[cnt]["Model No"]), VECVFlag, ISProcessed, BillingType, PlantID, DepotID);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "DA", iDealerID);
                }
                objDB.CommitTransaction();
                return true;
            }
            catch
            {
                objDB.RollbackTransaction();
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        public void UpdateFlagInDACreationHeader(string ISConfirmedFlag, string BillingType, string PlantID, int DepotID, int iDAID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_UpdateFlagInDACreationHeader", ISConfirmedFlag, BillingType, PlantID, DepotID, iDAID);
                objDB.CommitTransaction();
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool bSaveDAGrid(DataTable dtDAtHederSave)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                int iDealerID;
                objDB.BeginTranasaction();
                for (int cnt = 0; cnt < dtDAtHederSave.Rows.Count; cnt++)
                {
                    iDealerID = Func.Convert.iConvertToInt(dtDAtHederSave.Rows[cnt]["Dealer ID"]);
                    objDB.ExecuteStoredProcedure("sp_Save_DACreationHeader", Func.Convert.iConvertToInt(dtDAtHederSave.Rows[cnt]["ID"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["Dealer Code"]), Func.Convert.iConvertToInt(dtDAtHederSave.Rows[cnt]["Dealer ID"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["Dealer Name"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["Model No"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["Model Name"]), Func.Convert.iConvertToInt(dtDAtHederSave.Rows[cnt]["Qty"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["DA No"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["DA Date"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["BillingType"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["PlantID"]), Func.Convert.sConvertToString(dtDAtHederSave.Rows[cnt]["DepotID"]));
                    //Func.Common.UpdateMaxNo(sFinYear, "DA", iDealerID);
                }
                objDB.CommitTransaction();
                return true;

            }
            catch
            {
                objDB.RollbackTransaction();
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        public string bCreateDANo(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);
                // 'BeginTranasaction' not needed here, Commented by Shyamal
                //Func.DB.BeginTranasaction();

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, "DA", iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(4, '0');
                sDocNo = sDealerCode.Substring(2, 4) + "I" + sFinYearChar + sMaxDocNo;
                // Func.Common.UpdateMaxNo(sFinYear, "DA", iDealerID);
                // 'CommitTransaction' not needed here, Commented by Shyamal  
                //Func.DB.CommitTransaction();
                return sDocNo;
            }
            catch
            {
                // 'RollbackTransaction' not needed here, Commented by Shyamal 
                //Func.DB.RollbackTransaction();
                return "0";
            }
        }



        public DataSet GetDA(string sDealerID, string Flag)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_DACreationHeader", sDealerID, Flag);
                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;

            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        //Megha24062011


        public DataSet GetModelIDByCode(string sModelCode)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetModelIDByCode", sModelCode);
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

    }
}
