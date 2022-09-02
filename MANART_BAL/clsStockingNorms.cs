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
    /// Summary description for clsStockingNorms
    /// </summary>
    public class clsStockingNorms
    {
        public clsStockingNorms()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //Get Stock Details

        /// <summary>    
        /// To Get StockNorms of all types    
        /// </summary>    
        /// <param name="sRFPType"> sStockType 'All','Max', 'New' </param>
        public DataSet GetStockingNorms(int iStockID, string sStockType, int CategoryID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetStockingNorms", iStockID, sStockType, CategoryID);
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

        public int CheckValidStockingNorms(int CategoryID)
        {

            clsDB objDB = new clsDB();
            int ret = 1;
            try
            {
                ret = objDB.ExecuteStoredProcedure("SP_ValidStockingNorms", CategoryID);
                return ret;
            }
            catch (Exception ex)
            {
                return ret;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        public bool bSaveStockNorms(ref int iHdrID, DataTable dtHdr, DataTable dtDet)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveStockHeader(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                if (bSaveStockDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

                // Create XML
                if (bCreateStockCategoryXML(objDB, dtHdr, iHdrID) == false) goto ExitFunction;

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

        public bool bSaveStockNormsCatWise(ref int iHdrID, DataTable dtHdr)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveStockNormsCatWiseHeader(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Create XML
                //if (bCreateStockCategoryXML(objDB, dtHdr, iHdrID) == false) goto ExitFunction;

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
        private bool bSaveStockNormsCatWiseHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                //string sFinYear = Func.sGetFinancialYear();
                for (int iRowCnt = 0; iRowCnt < dtHdr.Rows.Count; iRowCnt++)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_StockingNormsCatWise_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Dealer_Id"],
                        dtHdr.Rows[0]["User_ID"], dtHdr.Rows[0]["Cat_AF"], dtHdr.Rows[0]["Cat_BF"], dtHdr.Rows[0]["Cat_CF"],
                        dtHdr.Rows[0]["Cat_AM"], dtHdr.Rows[0]["Cat_BM"], dtHdr.Rows[0]["Cat_CM"], dtHdr.Rows[0]["Cat_AS"],
                        dtHdr.Rows[0]["Cat_BS"], dtHdr.Rows[0]["Cat_CS"], dtHdr.Rows[0]["HoBrID"]);
                    //Func.Common.UpdateMaxNo(objDB, sFinYear, "STNRMS", Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_Id"].ToString()));
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveStockHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                string sFinYear = Func.sGetFinancialYear(0);
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_StockNormsHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["EffectiveDate From"], dtHdr.Rows[0]["EffectiveDate To"], dtHdr.Rows[0]["Category_ID"], dtHdr.Rows[0]["Document No"], dtHdr.Rows[0]["Document Date"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["Is_Confirm"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "STNRMS", -1);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_StockNormsHDR_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["EffectiveDate From"], dtHdr.Rows[0]["EffectiveDate To"], dtHdr.Rows[0]["Category_ID"], dtHdr.Rows[0]["Document No"], dtHdr.Rows[0]["Document Date"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["Is_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveStockDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    //{
                    if (dtDet.Rows[iRowCnt]["Part No"].ToString() != "" && dtDet.Rows[iRowCnt]["Large"].ToString() != "" && dtDet.Rows[iRowCnt]["Medium"].ToString() != "" && dtDet.Rows[iRowCnt]["Small"].ToString() != "" && dtDet.Rows[iRowCnt]["Basic"].ToString() != "")
                    {
                        objDB.ExecuteStoredProcedure("SP_StockNormsDet_Save", iHdrID, dtDet.Rows[iRowCnt]["Part No"], dtDet.Rows[iRowCnt]["Large"], dtDet.Rows[iRowCnt]["Medium"], dtDet.Rows[iRowCnt]["Small"], dtDet.Rows[iRowCnt]["Basic"]);

                    }
                    //}
                    //else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    //{
                    //    //To Delete
                    //    objDB.ExecuteStoredProcedure("SP_RFPDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    //}
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        private bool bCreateStockCategoryXML(clsDB objDB, DataTable dtHdr, int iHdrID)
        {

            try
            {
                if (Func.Convert.sConvertToString(dtHdr.Rows[0]["Is_Confirm"]) == "Y")
                    objDB.ExecuteStoredProcedure("SP_Generate_StockNorms_Cur", dtHdr.Rows[0]["Category_ID"], dtHdr.Rows[0]["ID"]);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public DataSet GetAssignDealerCategory(int iID, string sSelectType, int Region_ID, int Category_ID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetAssignDealerCategory", iID, sSelectType, Region_ID, Category_ID);
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

        public int CheckValidAssignDealerCategory(int RegionID, int CategoryID)
        {

            clsDB objDB = new clsDB();
            int ret = 1;
            try
            {
                ret = objDB.ExecuteStoredProcedure("SP_ValidAssignDealerCategory", RegionID, CategoryID);
                return ret;
            }
            catch (Exception ex)
            {
                return ret;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        public bool bSaveAssignDealerCategory(ref int iHdrID, DataTable dtHdr, DataTable dtDet)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveAssignDealerCategoryHeader(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                // Save Details
                if (bSaveAssignDealerCategoryDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

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


        private bool bSaveAssignDealerCategoryHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {

            try
            {
                string sFinYear = Func.sGetFinancialYear(0);
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_Save_AssignDealerCategoryHdr", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Document No"], dtHdr.Rows[0]["Document Date"], dtHdr.Rows[0]["Region_ID"], dtHdr.Rows[0]["Category_ID"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["Is_Confirm"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "ADC", -1);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_Save_AssignDealerCategoryHdr", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Document No"], dtHdr.Rows[0]["Document Date"], dtHdr.Rows[0]["Region_ID"], dtHdr.Rows[0]["Category_ID"], dtHdr.Rows[0]["UserID"], dtHdr.Rows[0]["Is_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveAssignDealerCategoryDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    //{
                    //if (dtDet.Rows[iRowCnt]["Part_ID"].ToString() != "0")
                    //{
                    objDB.ExecuteStoredProcedure("SP_Save_AssignDealerCategoryDtls", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Dealer_ID"], dtDet.Rows[iRowCnt]["SubCat_ID"]);

                    //}
                    //}
                    //else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    //{
                    //    //To Delete
                    //    objDB.ExecuteStoredProcedure("SP_RFPDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    //}
                }
                bSaveRecord = true;
            }
            catch (Exception ex)
            {
            }
            return bSaveRecord;
        }

        public DataSet GetDealerStockingNorms(string sDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_DealerLinkStockingNorms", sDealerID);
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

        public DataSet GetDealerStockingNormsCatWise(int iID, string sDealerIds, string DocType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetStockingNormsCatWise", iID, sDealerIds, DocType);
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

        public bool bSaveDealerStockingNorms(DataTable dtDet)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if ((dtDet.Rows[iRowCnt]["DealerID"].ToString() != "0") && (dtDet.Rows[iRowCnt]["ID"].ToString() == "0"))
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveDealerLinkStockingNorms", dtDet.Rows[iRowCnt]["DealerID"], dtDet.Rows[iRowCnt]["Md_Gr_Code"], dtDet.Rows[iRowCnt]["size"]);
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

        #region Parts Stocking Norms Upload from Excel
        public DataSet UploadPartsStockingNormsDetails(string sFileName, DataTable dtDet, int iUserId)
        {
            DataSet ds = null;
            clsDB objDB = new clsDB();
            try
            {
                if (bSaveUploadedPartStkNormsDetails(sFileName, dtDet) == true)
                {
                    ds = objDB.ExecuteStoredProcedureAndGetDataset("[SP_ImportPartsStockingNormsDetailsFromExcelSheet]", sFileName, iUserId);
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
        private bool bSaveUploadedPartStkNormsDetails(string sFileName, DataTable dtDet)
        {
            bool bSaveRecord = false;
            string bNewTable = "Y";
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_InsertPartsStockingNormsDetailsFromExcelSheet", sFileName, dtDet.Rows[iRowCnt]["Dealer_Code"],
                        dtDet.Rows[iRowCnt]["Cat_AF"], dtDet.Rows[iRowCnt]["Cat_BF"],dtDet.Rows[iRowCnt]["Cat_CF"],
                        dtDet.Rows[iRowCnt]["Cat_AM"], dtDet.Rows[iRowCnt]["Cat_BM"],dtDet.Rows[iRowCnt]["Cat_CM"], 
                        dtDet.Rows[iRowCnt]["Cat_AS"], dtDet.Rows[iRowCnt]["Cat_BS"], dtDet.Rows[iRowCnt]["Cat_CS"],
                        bNewTable);
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

        #region Save Stocking Norms
        public bool bStockingNorms(DataTable dtDet,int iUserID)
        {
            bool bSaveRecord = false;
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_Save_StockingNorms", dtDet.Rows[iRowCnt]["Dealer_Code"], iUserID,
                        dtDet.Rows[iRowCnt]["Cat_AF"], dtDet.Rows[iRowCnt]["Cat_BF"],dtDet.Rows[iRowCnt]["Cat_CF"],
                        dtDet.Rows[iRowCnt]["Cat_AM"], dtDet.Rows[iRowCnt]["Cat_BM"],dtDet.Rows[iRowCnt]["Cat_CM"], 
                        dtDet.Rows[iRowCnt]["Cat_AS"], dtDet.Rows[iRowCnt]["Cat_BS"], dtDet.Rows[iRowCnt]["Cat_CS"]);
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

    }
}
