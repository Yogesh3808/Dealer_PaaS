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
    /// Summary description for clsScheme
    /// </summary>
    public class clsScheme
    {
        public clsScheme()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private bool bSaveHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012      
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["scheme_no"]) == 0)
                {
                    iHdrID = objDB.ExecuteStoredProcedure("SP_SchemeHDR_SAVE", dtHdr.Rows[0]["scheme_no"], dtHdr.Rows[0]["VECV_scheme_no"], dtHdr.Rows[0]["scheme_desc"], dtHdr.Rows[0]["scheme_type"], dtHdr.Rows[0]["eff_from_date"], dtHdr.Rows[0]["eff_to_date"], dtHdr.Rows[0]["Scheme_Confirm"]);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SchemeHDR_SAVE", dtHdr.Rows[0]["scheme_no"], dtHdr.Rows[0]["VECV_scheme_no"], dtHdr.Rows[0]["scheme_desc"], dtHdr.Rows[0]["scheme_type"], dtHdr.Rows[0]["eff_from_date"], dtHdr.Rows[0]["eff_to_date"], dtHdr.Rows[0]["Scheme_Confirm"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["scheme_no"]);
                }
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                return bSaveRecord;
            }
        }
        public bool bSaveRecordWithScheme(DataTable dtHdr, DataTable dtDet)
        {
            int iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //saveDetails
                if (bSaveSchemeDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;
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
        private bool bSaveSchemeDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012      
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    //{
                    if (dtDet.Rows[iRowCnt]["Part_No_ID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_SchemeDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["Part_No_ID"], dtDet.Rows[iRowCnt]["min_qty"], dtDet.Rows[iRowCnt]["max_qty"], dtDet.Rows[iRowCnt]["disc_per"]);
                    }
                    //}
                    //else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    //{
                    //    //To Delete
                    //    objDB.ExecuteStoredProcedure("SP_RFPDet_Del", dtDet.Rows[iRowCnt]["ID"]);
                    //}
                }
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                return bSaveRecord;
            }
        }
        public DataSet GetPartScheme(int iSchemeNo, string sSchemeType)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_Scheme", iSchemeNo, sSchemeType);
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
    }
}
