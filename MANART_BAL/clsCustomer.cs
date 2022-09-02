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
    /// Summary description for clsDealer
    /// </summary>
    public class clsCustomer
    {
        public clsCustomer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //public DataSet GetCustomer(int DealerID)
        //{
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataSet ds;
        //        ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_Customer", DealerID);
        //        return ds;
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
        public DataSet GetCustomer(int DealerID, int iHOBr_id)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_Customer", DealerID, iHOBr_id);
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





        public DataSet GetLeadMaster(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_LeadMaster", DealerID);
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


        public DataSet GetLeadDealerID(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_LeadDealerID", DealerID);
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



        public DataSet GetMaxCustomer(int HOBrID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxCustomer", HOBrID);
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
        public DataSet GetPartGroupMasterDts(int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPartGroupMaster", iID);
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
        public DataSet GetCustomerdetails(int ID,int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCustomer",ID, DealerID);
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

        public DataSet GetCustomeRecordCount(string customer, string address, string city, string mobileNo)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCustomeRecordCount", customer, address, city, mobileNo);
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

        public int bSaveM0Master(int iID, string sID, int dealerID, int HOBrID, int CustType, int prefix, 
            //string FirstName, String LAstName,
            string Name, 
            string Address1,
       string Address2, string City,
       string pincode, int Region, int State, int District, int Country, string Mobile, string Email,
            int priApp, int secApp, string MTICust, int M0Financier, string bodybuilder, string Doneby
            ,string LastPurchdate ,int FleetQty,string KAM
      )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_M0Master_save",iID,sID, dealerID, HOBrID, CustType, prefix, Name,"","",
                    Address1, Address2, City,
                pincode, Region, State, District, Country, Mobile, Email,priApp,secApp,MTICust,M0Financier,bodybuilder,Doneby,LastPurchdate,FleetQty,KAM);
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

        public DataSet GetM0(int iID, string type, int dealer, int HOBrID ,int CRMCust)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetM0", iID, type, dealer, HOBrID, CRMCust);
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

        public int bSaveCustomerDetails(int iID, int CustType, string Name, string Address1, string Address2, string Address3, string City,
            string pincode, int Region, int State, int Country, string Mobile, string Phone, string Email, string ContactPerson, string ContactPersonPhone,
           string PANNo, string TINNo, string CSTNO, string STVATNO, string LBT, string DealerActive, int CustType_Dealer_ID,string Deliveryadd1,string Deliveryadd2,string Deliveryadd3,string  Deliverycity,int deliverystate,string GstNo
            , string Delv_Contact_Person, string Delv_Contact_phone_no, string Delv_pincode, string Delv_E_mail, string Delv_PANNo, string Delv_GST_No
            , string DlvName, string DlvMobile
            )
            
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_Customer_save", iID, CustType, Name, Address1, Address2, Address3, City,
                pincode, Region, State, Country, Mobile, Phone, Email, ContactPerson, ContactPersonPhone, PANNo, TINNo, CSTNO, STVATNO, LBT, 
                DealerActive, CustType_Dealer_ID, Deliveryadd1, Deliveryadd2, Deliveryadd3, Deliverycity, deliverystate, GstNo
                , Delv_Contact_Person, Delv_Contact_phone_no, Delv_pincode, Delv_E_mail, Delv_PANNo, Delv_GST_No, DlvName, DlvMobile);
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

        public int bSaveLeadDetails(int iID, int dealerID, int HOBrID, int CustType, string Name, string Address1,
            string Address2, string City,
            string pincode, int Region, int State, int District, int Country, string Mobile, string Email
           )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_LeadMaster_save", iID, dealerID, HOBrID, CustType, Name, Address1, Address2, City,
                pincode, Region, State, District, Country, Mobile, Email);
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
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //objDB.ExecuteStoredProcedure("SP_CustomerpartCategory_save", sID, dtDet.Rows[iRowCnt]["PartCat"], dtDet.Rows[iRowCnt]["Percentage"]);
                    objDB.ExecuteStoredProcedure("SP_CustomerPartGroupDiscountPer_save", sID, dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["Percentage"]);
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

        public bool bDealerWiseCustomerSave(int DealerID, int HOBrid, int CustID, string sLedgerName)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();


                objDB.ExecuteStoredProcedure("SP_DealerWiseCustomer_save", DealerID, HOBrid, CustID, sLedgerName);



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
