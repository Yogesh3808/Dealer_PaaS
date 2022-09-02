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
    /// Summary description for clsParameter
    /// </summary>
    public class clsParameter
    {
        public clsParameter()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetSystemParameters(int DealerID, int HoBr_Id, int iParaID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 29032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSystemParameters_values", DealerID, HoBr_Id, iParaID);
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
        public bool bSaveSystemParametersValue(DataTable dtDet, DataTable dtDet1, int iDealerID, int iHOBr_id)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.ExecuteStoredProcedure("SP_SaveSystemParametersValue", dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["ParaValue"], dtDet.Rows[iRowCnt]["Active"], dtDet.Rows[iRowCnt]["Editable"], iDealerID, iHOBr_id);

                }
                for (int iRowCnt = 0; iRowCnt < dtDet1.Rows.Count; iRowCnt++)
                {

                    objDB.ExecuteStoredProcedure("SP_SaveParametersValue", dtDet1.Rows[iRowCnt]["ID"], dtDet1.Rows[iRowCnt]["FlagValue"], dtDet1.Rows[iRowCnt]["Active"], dtDet1.Rows[iRowCnt]["Editable"], iDealerID, iHOBr_id);

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
        public bool bSaveParameter(int iMenuId, int iSysParaID, string sysParaDesc, string Active, int iUserId)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                if (iMenuId == 585 || iMenuId == 590)
                {
                    objDB.ExecuteStoredProcedure("SP_SystemParameter_Save", iSysParaID, sysParaDesc, Active, iUserId);
                }
                else if (iMenuId == 586 || iMenuId == 591)
                {
                    objDB.ExecuteStoredProcedure("SP_Parameter_Save", iSysParaID, sysParaDesc, Active, iUserId);
                }

                bSaveRecord = true;

                objDB.CommitTransaction();
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
