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
    public class clsEGPSupplier
    {
        public clsEGPSupplier()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string sGetMaxDocNo(string sDocName, int iDealerID)
        {
            string sFinYear = "";
            string sNo = "";
            int iMaxDocNo = 0;
            string sMaxNo = "";
            if (sFinYear == "")
            {
                sFinYear = Func.sGetFinancialYear(iDealerID);
            }
            iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
            sMaxNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
            sMaxNo = sMaxNo.PadLeft(4, '0');
            sNo = sDocName + iDealerID + sMaxNo;
            return sNo;
        }
        public DataSet GetEGPSupplier(int DealerID, string Flag)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_EGPSupplier", DealerID, Flag);
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
        public DataSet GetDealerID(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealerID", DealerID);
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

        public DataSet GetMaxEGPSupplier(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxEGPSupplier", DealerID);
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



        //public int bSaveEGPSupplierDetails(int iID, int dealerID, int SupType, string Name, string Address1, string Address2, string Address3, string City,
        //    string pincode, int Region, int State, int Country, string Mobile, string Phone, string Email, string ContactPerson, string ContactPersonPhone,
        //   string PANNo, string TINNo, string CSTNO, string STVATNO, string DealerActive)

        public int bSaveEGPSupplierDetails(int iID, int dealerID, string Name, string Address1, string Address2, string City,
         int Region, int State, int Country, string Mobile, string Phone, string Email,
       string PANNo, string TINNo, string CSTNO, string STVATNO, string DealerActive, string SupplierCode)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            string sFinYear = Func.sGetFinancialYear(dealerID);
            int suppID = iID;
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                //iID = objDB.ExecuteStoredProcedure("sp_EGPSupplier_save", iID, dealerID, SupType, Name, Address1, Address2, Address3, City,
                //pincode, Region, State, Country, Mobile, Phone, Email, ContactPerson, ContactPersonPhone, PANNo, TINNo, CSTNO, STVATNO, DealerActive);
                iID = objDB.ExecuteStoredProcedure("sp_EGPSupplier_save", iID, dealerID, Name, Address1, Address2, City,
                Region, State, Country, Mobile, Phone, Email, PANNo, TINNo, CSTNO, STVATNO, DealerActive, SupplierCode);

                if (suppID == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "SPL", dealerID);
                }

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

        public bool bEGPSupplierlinking(int EGpdealerID, int SupplierID)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();



                objDB.ExecuteStoredProcedure("sp_EGPSupplierlinking_Save", EGpdealerID, SupplierID);



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
        public DataSet GetEGPSupplier(int EGPDealerId)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("sp_getEGPSupplier", EGPDealerId);
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
