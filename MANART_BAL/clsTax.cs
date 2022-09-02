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
    /// Summary description for clsTax
    /// </summary>
    public class clsTax
    {
        public clsTax()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public DataSet GetMaxTax(int HOBr_ID, int dealerId,string Type)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxTax", HOBr_ID, dealerId, Type);
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
        public DataSet GetTax(int ID,string Type)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetTaxMaster", ID, Type);
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

        public bool bSaveTaxMaster(int TaxID, string TaxDescription, double TaxPecentage, int TaxType, string TaxTag, int TaxApplicable, int Tax1, int Tax2, string Active, int StateId, string serviceTax,string ISGST)
        {
            bool bSubmit = false;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_TaxMaster_Save_Statewise", TaxID, TaxDescription, TaxPecentage, TaxType, TaxTag, TaxApplicable, Tax1, Tax2, Active, StateId, serviceTax, ISGST);

                //Func.Common.UpdateMaxNo(sFinYear, sDocName, 0);

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
