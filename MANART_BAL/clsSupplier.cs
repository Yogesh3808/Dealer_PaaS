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
    public class clsSupplier
    {
        public clsSupplier()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetMaxSupplier(int HOBr_ID, int dealerId)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxSupplier", HOBr_ID, dealerId);
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
        public string sGetMaxDocNo(string sDocName, int iDealerID , string DealerCode)
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
            sMaxNo = sMaxNo.PadLeft(5, '0');
            sNo = sDocName + DealerCode + sMaxNo;
            return sNo;
        }
        public DataSet GetSupplier(int DealerID, string Flag)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_Supplier", DealerID, Flag);
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
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_DealerID", DealerID);
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

        public DataSet GetMaxSupplier(int DealerID)
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

        public DataSet GetDealerCode(int dealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
           
          string DealerCode;
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();

                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerCode", dealerID);

               
                objDB.CommitTransaction();
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



        public int bSaveSupplierDetails(int iID, int dealerID, int HOBr_ID, string Name, string Address1, string Address2, string City,
         int Region, int State, int Country, string Mobile, string Phone, string Email, int Sup_Type,
       string PANNo, string TINNo, string CSTNO, string STVATNO, string DealerActive, string SupplierCode, int Type_Dealer_ID,string GSTIN, string sLedgerName)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            string sFinYear = Func.sGetFinancialYear(dealerID);
            

            int suppID = iID;
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_Save_Supplier", iID, dealerID, HOBr_ID, Name, Address1, Address2, City,
                Region, State, Country, Mobile, Phone, Email, Sup_Type, PANNo, TINNo, CSTNO, STVATNO, DealerActive, SupplierCode, Type_Dealer_ID, GSTIN, sLedgerName);

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


        public DataSet GetSupplier(int ID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetSupplier", ID);
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


        public DataSet GetEmployee(int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetEmployee", iID);
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

        public int bSaveEmplMaster(int iID, int dealerID, int iHOBr_id, string p1, string p2, string active, string Contact_No)
        {

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_Employee", iID, dealerID, iHOBr_id, p1, p2, active, Contact_No);
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

        public DataSet GetMaxEmp(int iHOBr_id, int iDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxEmployee", iHOBr_id, iDealerID);
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


        public DataSet GetMaxPotype()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxPotype");
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

        public DataSet GetPotypedetails(int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPoTypeDetails", iID);
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

  
        public DataSet GetMaxClaimType()
        {

            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxCliamtype");
                //  ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCliamtype", iID);
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

   


        public int bSaveTermsAndConditions(int iID, int iDealerID, string termsandcondition, string transID, string active)
        {

            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_SaveTermsAndCondition ", iID, iDealerID, transID, termsandcondition, active);
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

        public DataSet GetMaxTandC( int iDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_MaxTermsandCondition", iDealerID);
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

        public DataSet GetTAndCDetails(int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_TermsandConditionDetails", iID);
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

        public int bSaveChassisAndCustomerDetials(int iID,string customerName, string phoneno, string address1, string txtaddress2, string stateID, string RegionID, string Pincode, int iDealerID, string DealerCode, int CustID, int iHOBr_id,string ChassisNo,string Engineno,string modelno ,string MobNo, string insdate,string vehno)
        {
            clsDB objDB = new clsDB();
            try
            {
            
                objDB.BeginTranasaction();
             // ID = objDB.ExecuteStoredProcedure("SP_ChassisAndCustomerDetails", iID, customerName, phoneno, address1, txtaddress2, stateID, RegionID, Pincode, iDealerID, DealerCode, CustID, iHOBr_id, ChassisNo, Engineno, modelno, ChassisID,ModelID,ModelGrpID,ModelCatID,SaleDate);
                //iID = objDB.ExecuteStoredProcedure("SP_ChassisCustomerInsert", customerName, phoneno, address1, txtaddress2, stateID, RegionID, Pincode, DealerCode, ChassisNo, Engineno, modelno, SaleDate, iDealerID, CustID, iHOBr_id, ChassisID, ModelID, ModelGrpID, ModelCatID, MobNo);

                iID = objDB.ExecuteStoredProcedure("SP_ChassisCustomerInsert", customerName, phoneno, address1, txtaddress2, stateID, RegionID, Pincode, DealerCode, ChassisNo, Engineno, modelno, MobNo,(insdate), vehno);
                           
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

        public DataSet GetMaxChassisAndCust(int iHOBr_id, int iDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_MaxChassisAndCust", iDealerID);
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

        public DataSet GetChassisAndCustDetails(int iID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetChassisAndCustDetials", iID);
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





        public DataSet Sp_LubricantPrice(string p1, int StateId, int iLubPriceID, string p2, string p3, string LubType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset(p1, StateId, iLubPriceID,p2,p3,LubType);
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

        public DataSet Sp_GetMaxLubricantPrice(string p, string DealerOrigin, int StateId, string LubType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset(p, DealerOrigin, StateId, LubType);
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

     
        
        public int bSaveCouponDetails(int iID, string ServID, string ChassisNO, string Couponrate, string CouponNo, string MSPD, string CouponStatus)
        {
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_CouponInsert", ServID, ChassisNO, Couponrate, CouponNo, MSPD, CouponStatus);
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

        public int bUpdatechasisInsdate(int iID,string dealercode,string chassisNo ,string insdate,string VehNo)
        {
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("Get_UpdateChassisInsdateDetails", dealercode,chassisNo, insdate, VehNo);
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
    }
}
