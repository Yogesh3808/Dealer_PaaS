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
    /// Summary description for clsEGPDealer
    /// </summary>
    public class clsEGPCustomer
    {
        public clsEGPCustomer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetEGPCustomer(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_EGPCustomer", DealerID);
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
        public DataSet GetMaxEGPCustomer(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxEGPCustomer", DealerID);
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

        //Megha06/06/2011



        public int bSaveEGPCustomerDetails(int iID, int dealerID, int CustType, string Name, string Address1, string Address2, string Address3, string City,
            string pincode, int Region, int State, int Country, string Mobile, string Phone, string Email, string ContactPerson, string ContactPersonPhone,
           string PANNo, string TINNo, string CSTNO, string STVATNO, string DealerActive)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_EGPCustomer_save", iID, dealerID, CustType, Name, Address1, Address2, Address3, City,
                pincode, Region, State, Country, Mobile, Phone, Email, ContactPerson, ContactPersonPhone, PANNo, TINNo, CSTNO, STVATNO, DealerActive);
                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public bool bPartCaregory(int sID, DataTable dtDet)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.ExecuteStoredProcedure("SP_EGPpartCategory_save", sID, dtDet.Rows[iRowCnt]["PartCat"], dtDet.Rows[iRowCnt]["Percentage"]);

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
