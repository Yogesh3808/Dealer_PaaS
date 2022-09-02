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
    /// Summary description for clsWarrantyLeaveMgmt
    /// </summary>
    public class clsWarrantyLeaveMgmt
    {
        public clsWarrantyLeaveMgmt()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetDataForCurrentUser(int UserID, int UserType)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_GetDataForCurrentUserWarranty", UserID, UserType);
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
        public DataSet GetDataForAllUser(int UserType)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDataForAllUserWarranty", UserType);
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
        public DataSet GetDataForGridLeaveDetails(int iID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetFillGridLeaveDetails", iID);
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
        public int GetDataForValidateLeaveDetails(int UserID, string Fromdate, string ToDate, int CurrentUserID, int AllUserID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            int ret = 0;
            try
            {
                ret = objDB.ExecuteStoredProcedure("sp_ValidateLeaveDetails", UserID, Fromdate, ToDate, CurrentUserID, AllUserID);
                return ret;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }
        public int bSaveLeaveDetails(int iID, int CurrentUserID, int AllUserID, string Fromdate, string ToDate, string remark, int UserID, string IsConfirm)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_SaveLeaveDetails ", iID, CurrentUserID, AllUserID, Fromdate, ToDate, remark, UserID, IsConfirm);

                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
