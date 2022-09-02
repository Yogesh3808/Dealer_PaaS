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
    /// Summary description for clsPO
    /// </summary>
    public class clsSparesPO
    {
        String sPONo = "";
        public clsSparesPO()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string GeneratePO1(string sDealerCode, int iDealerID)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);

                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();


                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, "DO", iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(3, '0');
                // sDocNo = sDealerCode.Substring(2, 4) + "PO" + sFinYearChar + sMaxDocNo;
                // sDocNo = sDealerCode + "PO" + sFinYearChar + sMaxDocNo;
                // sDocNo = sDealerCode.Substring(2, 4) + "RO" + sMaxDocNo;
                if (sDealerCode != "")
                {
                    sDocNo = sDealerCode.Substring(2, 4) + "DO" + sFinYearChar + sMaxDocNo;
                }
                else
                {
                    sDocNo = "DO" + sFinYearChar + sMaxDocNo;
                }
                // Func.Common.UpdateMaxNo(sFinYear, "PO", iDealerID);

                // 'Commented by Shyamal as on 26032012       
                //objDB.CommitTransaction();

                return sDocNo;
            }
            catch
            {
                // 'Commented by Shyamal as on 26032012
                //objDB.RollbackTransaction();
                return "0";
            }
        }
        public string GeneratePO(string sDealerCode, int iDealerID, int POType)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
                sFinYearChar = sFinYear.Substring(3);

                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";

                if (POType == 2)
                {
                    sDocName = "DO";
                }
                else if (POType == 11)
                {
                    sDocName = "DE";
                }
                else if (POType == 14) //As per discussion with vikram sir change prefix of Reman PO  'RN' instead of 'RE'  date: 07072014
                {
                    // sDocName = "RE";
                    sDocName = "RN";
                }
                else if (POType == 15)
                {
                    sDocName = "RS";
                }
                else if (POType == 16)
                {
                    sDocName = "CO";
                }


                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(3, '0');
                // sDocNo = sDealerCode.Substring(2, 4) + "PO" + sFinYearChar + sMaxDocNo;
                // sDocNo = sDealerCode + "PO" + sFinYearChar + sMaxDocNo;
                // sDocNo = sDealerCode.Substring(2, 4) + "RO" + sMaxDocNo;
                if (sDealerCode != "")
                {
                    sDocNo = sDealerCode.Substring(2, 4) + sDocName + sFinYearChar + sMaxDocNo;
                }
                else
                {
                    sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                }


                // Func.Common.UpdateMaxNo(sFinYear, "PO", iDealerID);

                // 'Commented by Shyamal as on 26032012       
                //objDB.CommitTransaction();

                return sDocNo;
            }
            catch
            {
                // 'Commented by Shyamal as on 26032012
                //objDB.RollbackTransaction();
                return "0";
            }
        }
        public DataSet GetPOType()
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                //ds = objDB.ExecuteQueryAndGetDataset("Select ID,PO_SAP_Desc as Name from M_POType ");
                ds = objDB.ExecuteQueryAndGetDataset("Select ID,PO_SAP_Desc as Name from M_POType where Id in (2,11,15,16)");
                return ds;
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
        public DataSet GetPOType_REMAN()
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                //ds = objDB.ExecuteQueryAndGetDataset("Select ID,PO_SAP_Desc as Name from M_POType ");
                ds = objDB.ExecuteQueryAndGetDataset("Select ID,PO_SAP_Desc as Name from M_POType where Id in (14) ");
                return ds;
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
        public DataSet GetPO(int POId, string POType, int DealerID, int POTypeID) //change for REMAN PO
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_PO", POId, POType, DealerID, POTypeID);
                return ds;
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

        // 'Replace Func.DB to objDB and added parameter objDB of type clsDB by Shyamal on 05042012
        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            string sDocName = "";

            //  sDocName = "DO";
            int POTypeID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["PO_Type_ID"]);

            if (POTypeID == 2)
            {
                sDocName = "DO";
            }
            else if (POTypeID == 11)
            {
                sDocName = "DE";
            }
            else if (POTypeID == 14) //As per discussion with vikram sir change prefix of Reman PO  'RN' instead of 'RE'  date: 07072014
            {
                // sDocName = "RE"; 
                sDocName = "RN";
            }
            else if (POTypeID == 15)
            {
                sDocName = "RS";
            }
            else if (POTypeID == 16)
            {
                sDocName = "CO";
            }
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                   
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    sPONo = Func.Convert.sConvertToString(dtHdr.Rows[0]["PO_No"]);
                    //dtHdr.Rows[0]["PO_No"] = Func.Common.sGetMaxDocNo(sDealerCode, sFinYear, sDocName, iDealerId);
                    iHdrID = objDB.ExecuteStoredProcedure("SP_SparesPOHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PO_No"], dtHdr.Rows[0]["PO_Date"], dtHdr.Rows[0]["PO_CreatedBy"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["PO_Confirm"], dtHdr.Rows[0]["PO_Cancel"], dtHdr.Rows[0]["PO_Total"], dtHdr.Rows[0]["PO_TotalQty"], dtHdr.Rows[0]["PO_TotalItems"], dtHdr.Rows[0]["Chassis_No"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SparesPOHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PO_No"], dtHdr.Rows[0]["PO_Date"], dtHdr.Rows[0]["PO_CreatedBy"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Po_Type_ID"], dtHdr.Rows[0]["PO_Confirm"], dtHdr.Rows[0]["PO_Cancel"], dtHdr.Rows[0]["PO_Total"], dtHdr.Rows[0]["PO_TotalQty"], dtHdr.Rows[0]["PO_TotalItems"], dtHdr.Rows[0]["Chassis_No"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        // 'Replace Func.DB to objDB and added parameter objDB of type clsDB by Shyamal on 05042012
        private bool bSavePartDetails(clsDB objDB, string sDealerCode, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["Part_No_ID"].ToString() != "0")
                        {
                            objDB.ExecuteStoredProcedure("SP_PODet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["Qty"], dtDet.Rows[iRowCnt]["FOBRate"], dtDet.Rows[iRowCnt]["Total"], dtDet.Rows[iRowCnt]["Process_Accept"], dtDet.Rows[iRowCnt]["Process_Confirm"], dtDet.Rows[iRowCnt]["Status"]);
                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_PODet_Del", dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["Part_No_ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {
            }
            return bSaveRecord;
        }
        public bool bSaveRecordWithPart(string sDealerCode, DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSavePartDetails(objDB, sDealerCode, dtDet, iHdrID) == false) goto ExitFunction;
                if (dtHdr.Rows[0]["PO_Confirm"] == "Y")
                {
                    objDB.ExecuteStoredProcedureAndGetObject("SP_PO_SAP_DMS_Send", iHdrID, sDealerCode, dtHdr.Rows[0]["PO_No"]);


                }
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
        }
        public DataSet UploadPartDetailsAndGetPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        {


            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header

                // Commented by Shyamal on 27032012
                //objDB.BeginTranasaction();

                if (bSaveUploadedPartDetails(sFileName, iDealerId, dtDet) == true)
                {

                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportSparesPOPartDetailsFromExcelSheet", sFileName, iDealerId);

                }

                // Commented by Shyamal on 27032012
                //else
                //{
                //    objDB.RollbackTransaction();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                // Commented by Shyamal on 27032012
                //objDB.RollbackTransaction();

                return ds;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }



        }
        public DataSet UploadPartDetailsAndGetPartDetails_REMAN(string sFileName, int iDealerId, DataTable dtDet)
        {


            DataSet ds = null;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header

                // Commented by Shyamal on 27032012
                //objDB.BeginTranasaction();

                if (bSaveUploadedPartDetails_REMAN(sFileName, iDealerId, dtDet) == true)
                {

                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ImportSparesPOPartDetailsFromExcelSheet_REMAN", sFileName, iDealerId);

                }

                // Commented by Shyamal on 27032012
                //else
                //{
                //    objDB.RollbackTransaction();
                //}
                return ds;
            }
            catch (Exception ex)
            {
                // Commented by Shyamal on 27032012
                //objDB.RollbackTransaction();

                return ds;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }



        }
        private bool bSaveUploadedPartDetails(string sFileName, int iDealerId, DataTable dtDet)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            string bNewTable = "Y";
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertSparesPOPartDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
                    bNewTable = "N";
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

        private bool bSaveUploadedPartDetails_REMAN(string sFileName, int iDealerId, DataTable dtDet)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            string bNewTable = "Y";
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertSparesPOPartDetailsFromExcelSheet_REMAN", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
                    bNewTable = "N";
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
