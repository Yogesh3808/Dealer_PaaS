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
    public class clsEGPDealer
    {
        public clsEGPDealer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetEGPDealer(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_EGPDealer", DealerID);
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
        public DataSet GetMaxEGPDealer(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxEGPDealer", 0);
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
        public DataSet GetAllDealer()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteQueryAndGetDataset("Select ID,Dealer_Name as Name from M_Dealer order by ID");
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
        public DataSet GetAllDomesticDealer()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteQueryAndGetDataset("Select ID,Dealer_Name as Name from M_Dealer where Dealer_Origin='D' order by ID");
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
        public bool bSaveEGPDistributor(int EGPDealerID, int EGPDistributorID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            string bNewTable = "Y";
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();


                objDB.ExecuteStoredProcedure("sp_EGPDistributorLinking", EGPDealerID, EGPDistributorID);

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
        public bool bSaveEGPHOBranch(int EGPDealerID, int HOBranch, int HO, string fin_no)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            string bNewTable = "Y";
            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();


                objDB.ExecuteStoredProcedure("sp_EGPEGPHOBranch", EGPDealerID, HOBranch, HO, fin_no);

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
        public int bSaveEGPDealerDetails(int iID, string DealerName, string DealerShortName, string DealerCity, string DealerAddress1, string DealerAddress2,
            string DealerMobile, string DealerLandLinePhone, string DealerEmail, string DealerMDEmail, string DealerOrigin, string DealerActive,
            string TINNo, string CSTNO, string STNO, string ServiceTax, string PANNo, string Fax, int DealerCountry, int DealerRegion, int DealerState, string IsEGP, string pincode, string contactperson)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_EGPDealer_save", iID, DealerName, DealerShortName, DealerCity, DealerAddress1, DealerAddress2, DealerMobile, DealerLandLinePhone, DealerEmail, DealerMDEmail, DealerOrigin, DealerActive,
                      TINNo, CSTNO, STNO, ServiceTax, PANNo, Fax, DealerCountry, DealerRegion, DealerState, IsEGP, pincode, contactperson);

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

        public DataTable GetDealerByID(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDealerByID", DealerID);
                return dt;
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
        public DataSet GetEGPLoginname(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_EGPLoginname", ID);
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
