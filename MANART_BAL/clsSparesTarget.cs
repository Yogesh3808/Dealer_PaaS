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
    /// Summary description for clsSparesTarget
    /// </summary>
    public class clsSparesTarget
    {
        public clsSparesTarget()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetSparesTargetAnnualMaster(string sDealerID, int iYearID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_SparesTargetHeader", sDealerID, iYearID);
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
        #region Save Annual Target 
        public bool bAnnualTarget(DataTable dtDet)
        {
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_Save_SparesTargetHeader", dtDet.Rows[iRowCnt]["Dealer_Code"],dtDet.Rows[iRowCnt]["YTD_Target"], 
                       dtDet.Rows[iRowCnt]["T1_Q1"],dtDet.Rows[iRowCnt]["Q1L1"], dtDet.Rows[iRowCnt]["Q1L2"], dtDet.Rows[iRowCnt]["Q1L3"],
                       dtDet.Rows[iRowCnt]["T1_Q2"],dtDet.Rows[iRowCnt]["Q2L1"], dtDet.Rows[iRowCnt]["Q2L2"], dtDet.Rows[iRowCnt]["Q2L3"],
                       dtDet.Rows[iRowCnt]["T1_Q3"],dtDet.Rows[iRowCnt]["Q3L1"], dtDet.Rows[iRowCnt]["Q3L2"], dtDet.Rows[iRowCnt]["Q3L3"],
                       dtDet.Rows[iRowCnt]["T1_Q4"],dtDet.Rows[iRowCnt]["Q4L1"],dtDet.Rows[iRowCnt]["Q4L2"],dtDet.Rows[iRowCnt]["Q4L3"]);
                }
                bSaveRecord = true;
                objDB.CommitTransaction();

                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                bSaveRecord = false;
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        #endregion

        //public DataSet GetSparesTargetAnnualDetails(int sDealerID, int iYearID)
        //{
        //    // 'Replace Func.DB to objDB by Shyamal on 05042012
        //    clsDB objDB = new clsDB();
        //    try
        //    {

        //        DataSet dsDetails = new DataSet();
        //        //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_TargetMaster_Dtls", FType, iDealerID, iModelCatID, iYearID, cStatus);
        //        //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_TargetMaster_Dtls", FType, sDealerID, iModelCatID, iYearID, cStatus);
        //        dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_SparesTargetDetail", sDealerID, iYearID);

        //        if (dsDetails != null)
        //        {
        //            return dsDetails;
        //        }
        //        else
        //            return null;
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
        //public bool bMonthlyTarget(int sDealerID, int iYearID, DataTable dtDet)
        //{

        //    bool bSaveRecord = false;
        //    // 'Replace Func.DB to objDB by Shyamal on 05042012
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        objDB.BeginTranasaction();
        //        for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
        //        {
        //            objDB.ExecuteStoredProcedure("SP_Save_SparesTargetDetail", sDealerID, iYearID,
        //                dtDet.Rows[iRowCnt]["Month_Id"], dtDet.Rows[iRowCnt]["Montly_Amt"], dtDet.Rows[iRowCnt]["L1_Amt"], dtDet.Rows[iRowCnt]["L2_Amt"], dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]);

        //        }
        //        //Megha29052012 
        //        bSaveRecord = true;
        //        //Megha29052012
        //        objDB.CommitTransaction();
        //        return bSaveRecord;
        //    }
        //    catch (Exception ex)
        //    {
        //        objDB.RollbackTransaction();
        //        bSaveRecord = false;
        //        return bSaveRecord;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}
        #region Parts Annual Target Upload from Excel
        public DataSet UploadPartsTargetDetails(string sFileName, DataTable dtDet, int UserId)
        {
            DataSet ds = null;
            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartTargetDetails(sFileName, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("[SP_ImportPartsTargetDetailsFromExcelSheet]", sFileName, UserId);
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
        private bool bSaveUploadedPartTargetDetails(string sFileName, DataTable dtDet)
        {
            bool bSaveRecord = false;
            string bNewTable = "Y";
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertPartsTargetDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["Dealer_Code"],dtDet.Rows[iRowCnt]["YTD_Target"],
                        dtDet.Rows[iRowCnt]["T1_Q1"], dtDet.Rows[iRowCnt]["Q1L1"], dtDet.Rows[iRowCnt]["Q1L2"],dtDet.Rows[iRowCnt]["Q1L3"],
                        dtDet.Rows[iRowCnt]["T1_Q2"], dtDet.Rows[iRowCnt]["Q2L1"], dtDet.Rows[iRowCnt]["Q2L2"],dtDet.Rows[iRowCnt]["Q2L3"], 
                        dtDet.Rows[iRowCnt]["T1_Q3"], dtDet.Rows[iRowCnt]["Q3L1"], dtDet.Rows[iRowCnt]["Q3L2"],dtDet.Rows[iRowCnt]["Q3L3"],
                        dtDet.Rows[iRowCnt]["T1_Q4"], dtDet.Rows[iRowCnt]["Q4L1"], dtDet.Rows[iRowCnt]["Q4L2"],dtDet.Rows[iRowCnt]["Q4L3"], bNewTable);
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
        #endregion

        #region For Getting all Model Categoue
        public DataSet GetModelCatelogue(string sDcodeOrRcode)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_ModelCatelogue", sDcodeOrRcode);
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
#endregion
    }
}
