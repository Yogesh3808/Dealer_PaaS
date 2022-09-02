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
    /// Summary description for clsPartGroup
    /// </summary>
    public class clsPartGroup
    {
        public clsPartGroup()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetMaxPartGroupTaxMaster(int HOBr_ID, int dealerId)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxTaxGroup", HOBr_ID, dealerId);
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

        public DataSet GetPartGroupTaxMaster(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartGroupTaxMaster", ID);
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
        // public bool bSavePartGroupTaxMaster(int TaxID,string TaxDescription,double TaxPecentage,int TaxType,string TaxTag,int TaxApplicable,int Tax1,int Tax2,string Active,int dealerID,int HoBrID)
        // {
        public bool bSavePartGroupTaxMaster(int TaxID, string PartGroup, int TaxType, string Active, int iDealerID, int iHOBr_id)
        {
            bool bSubmit = false;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_PartGroupTaxMaster_Save", TaxID, PartGroup, TaxType, Active, iDealerID, iHOBr_id);

                objDB.CommitTransaction();

                bSubmit = true;
                return bSubmit;
            }
            catch (Exception ex)
            {

                objDB.RollbackTransaction();

                return bSubmit;
            }

            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
