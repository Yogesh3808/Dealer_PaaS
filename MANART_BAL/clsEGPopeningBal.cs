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
    /// Summary description for clsEGPopeningBal
    /// </summary>
    public class clsEGPopeningBal
    {
        public clsEGPopeningBal()
        {
            //
            // TODO: Add constructor logic here
            //

        }
        public bool bSaveRecord(DataTable dtDet, int DealerId)
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
                if (bSaveDetails(objDB, dtDet, DealerId) == false) goto ExitFunction;
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

        public bool bstockchk(int DealerId)
        {
            //saveDetails
            int part_ID = 0;
            bool bChkRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                part_ID = objDB.ExecuteStoredProcedure("SP_GetEGPStock_Present", DealerId);
                if (part_ID > 0)
                {
                    bChkRecord = true;

                }
                else
                {
                    bChkRecord = false;
                }

            }
            catch (Exception ex)
            {

            }
            return bChkRecord;
        }
        private bool bSaveDetails(clsDB objDB, DataTable dtDet, int DealerId)
        {
            //saveDetails
            int part_ID = 0;
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["PartNo"].ToString() != "")
                    {
                        part_ID = objDB.ExecuteStoredProcedure("SP_GetPart_Present", dtDet.Rows[iRowCnt]["PartNo"].ToString().Trim());
                        if (part_ID > 0)
                        {
                            objDB.ExecuteStoredProcedure("sp_saveEGPopeningstock", dtDet.Rows[iRowCnt]["PartNo"].ToString().Trim(), dtDet.Rows[iRowCnt]["Quantity"], DealerId);
                        }

                    }

                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {

            }
            return bSaveRecord;
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

                    ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGP_ImportPartopeningstockFromExcelSheet", sFileName, iDealerId);

                }

                return ds;
            }
            catch (Exception ex)
            {
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
                    objDB.ExecuteStoredProcedure("SP_EGP_InsertPartopeningstockFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["PartNo"], dtDet.Rows[iRowCnt]["Quantity"], bNewTable);
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
