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
   public class clsCRM
    {
        public clsCRM()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string GenerateTicketNo(string sDealerCode, int iDealerID, int CallType)
        {
            try
            {
                string sDocNo = "", sMaxDocNo = "", sFinYearChar = "", sFinYear = Func.sGetFinancialYear(iDealerID);
                int iMaxDocNo;
               // sFinYearChar = sFinYear.Substring(3);

                if (sFinYear == "2016")
                {
                    sFinYearChar = sFinYear.Substring(3);
                }
                else
                {
                    sFinYearChar = sFinYear;
                }

               
                string sDocName = "";

                if (CallType == 1)  //Product Enquiry
                {
                    sDocName = "CRMPE";
                }
                else if (CallType == 2) //Regular Service/repaire Visit
                {
                    sDocName = "CRMRS";
                }
                else if (CallType == 3) //Emergency Breakdown
                {
                   
                    sDocName = "CRMEB";
                }
                else if (CallType == 4 ) //General Feedback/Enquiry/Complaints      //sales calls
                {

                    sDocName = "CRMGV";
                }
                else if (CallType == 5 ) //General Feedback/Enquiry/Complaints      //Service calls
                {

                    sDocName = "CRMGS";
                }
               
               

                iMaxDocNo = Func.Common.iGetMaxDocNo(sFinYear, sDocName, iDealerID);
                sMaxDocNo = Func.Convert.sConvertToString(iMaxDocNo + 1);
                sMaxDocNo = sMaxDocNo.PadLeft(4, '0');


                        if (sDealerCode != "" && sFinYear == "2016")
                        {
                            sDocNo = sDealerCode.Substring(1, 6) + sDocName + sFinYearChar + sMaxDocNo;
                        }
                        else if (sDealerCode != "" && sFinYear != "2016")
                        {
                            sDocNo = sDealerCode + sDocName + sFinYearChar + sMaxDocNo;
                        }
                        else
                        {
                            sDocNo = sDocName + sFinYearChar + sMaxDocNo;
                        }

               


                
                return sDocNo;
            }
            catch
            {
                
                return "0";
            }
        }
        public bool bSaveMTIFeedBack(DataTable dtDet, int iHdrID)
        {

            //saveDetails
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {


                    objDB.BeginTranasaction();

                    objDB.ExecuteStoredProcedure("SP_CRM_SaveMTIFeedBack", iHdrID, dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["FeedBackStatus1"], dtDet.Rows[iRowCnt]["FeedBackStatus2"], dtDet.Rows[iRowCnt]["FeedBackStatus3"], dtDet.Rows[iRowCnt]["Scale"], dtDet.Rows[iRowCnt]["MTIRemark"]);

                    objDB.CommitTransaction();


                }





                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }
        public bool  bSaveDealerFeedBack(DataTable dtDet, int iHdrID)
        {
          
            //saveDetails
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                   
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_CRM_SaveDealerFeedBack", iHdrID, dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["Remark"]);



                            objDB.CommitTransaction();

                        
                    }

                    


           
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }
        public bool bSaveRecordCRM(DataTable dtHdr, ref int iHdrID, string DealerCode)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, dtHdr, ref iHdrID, DealerCode) == false) goto ExitFunction;
              //  if (bSaveDealerFeedBack(objDB, dtDealerFeedback, int iHdrID) == false) goto ExitFunction;
               
             
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
        }
        public bool bSaveAcknowledge(int iHdrID,string DetailsRemark)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedureAndGetObject("SP_CRM_SAVEDealerAcknowledge", iHdrID, DetailsRemark);
                bSaveRecord = true;
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
        }
        public bool bSaveDealerCallClosure(int iHdrID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedureAndGetObject("SP_CRM_SaveDealerCallClosure", iHdrID);
                bSaveRecord = true;
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
        }
        public bool bSAVE_IsCalClose(int iHdrID, string CallCenterRemark)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedureAndGetObject("SP_CRM_SAVE_IsCalClose", iHdrID, CallCenterRemark);
                bSaveRecord = true;
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
        }
        public int bSaveCRMCustomerMaster(int CRMCustID, int GlobalCustID,int CustType, int Title,string IsMTICust,
            string Name,
            string Address1,
       string Address2, string pincode,int State,int District,int Region,string City,
        int Country, string Mobile, string Email,
            int priApp, int secApp
      )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                CRMCustID = objDB.ExecuteStoredProcedure("sp_CRM_CustomerMaster_save", CRMCustID, GlobalCustID, CustType, Title, IsMTICust,
             Name, Address1, Address2, pincode, State, District, Region, City, Country, Mobile, Email, priApp, secApp);
              objDB.CommitTransaction();
              return CRMCustID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return CRMCustID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet GetCRM_Sales(int CRMId, string Type, string ISDealer)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCRMDetails_Sales", CRMId, Type, ISDealer);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet GetCRM(int CRMId, string Type, string ISDealer)
        {
            // 'Replace objDB to objDB by Shyamal on 26032012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_CRM_GetCRMDetails", CRMId, Type, ISDealer);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }



        public int bSaveTicketManagement(int iID, int dealerID, int HOBrID, string TcktNo, string TcketDate,
         int TcktType,string Complaint,string Confirm,string Cancel,string appflag,string rply,string appconfirm
            , string AppFlagRWH, string AppConfirmRWH, string AppFlag2NdLvl, string AppConfirm2NdLvl, string TcktRplyRWH, string TcktRplyLevel2
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_TicketManagement", iID, dealerID, HOBrID, TcktNo,TcketDate,TcktType,Complaint,Confirm,Cancel
                    ,appflag,rply,appconfirm,
                    AppFlagRWH,AppConfirmRWH,AppFlag2NdLvl,AppConfirm2NdLvl,TcktRplyRWH,TcktRplyLevel2
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "TM";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }

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

        public bool bSaveTicketManagementFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            int iWarrantyDetID = 0;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtFileAttach.Rows.Count; iRowCnt++)
                {
                    if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "S")
                    {
                        iWarrantyDetID = Func.Convert.iConvertToInt(dtFileAttach.Rows[iRowCnt]["ID"]);
                        if (iWarrantyDetID == 0)
                        {
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_TicketManagement_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_TicketManagement_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                       
                        objDB.ExecuteStoredProcedure("SP_TicketManagement_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }




        private bool bSaveHeader(clsDB objDB, DataTable dtHdr, ref int iHdrID, string DealerCode)
        {
            string sDocName = "";

            //  sDocName = "DO";
            int CallType = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Call_Type_ID"]);
           

            if (CallType == 1)  //Product Enquiry
            {
                sDocName = "CRMPE";
            }
            else if (CallType == 2) //Regular Service/repaire Visit
            {
                sDocName = "CRMRS";
            }
            else if (CallType == 3) //Emergency Breakdown
            {

                sDocName = "CRMEB";
            }
            else if (CallType == 4) //General Feedback/Enquiry/Complaints      //sales calls
            {

                sDocName = "CRMGV";
            }
            else if (CallType == 5) //General Feedback/Enquiry/Complaints      //Service calls
            {

                sDocName = "CRMGS";
            }
               

            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {
                   
                    int iDealerId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string ISDealer = Func.Convert.sConvertToString(dtHdr.Rows[0]["IS_Dealer"]);
                    if (Func.Convert.sConvertToString(dtHdr.Rows[0]["IS_Dealer"])=="N")
                    {
                        iDealerId = 9999;
                    }
                    string sFinYear = Func.sGetFinancialYear(iDealerId);
                    dtHdr.Rows[0]["Ticket_No"] = Func.Convert.sConvertToString(GenerateTicketNo(DealerCode, iDealerId, CallType));

                    iHdrID = objDB.ExecuteStoredProcedure("SP_CRM_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Ticket_No"], dtHdr.Rows[0]["Ticket_Date"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Call_Type_ID"], dtHdr.Rows[0]["Call_Sub_Type_ID"], dtHdr.Rows[0]["Call_Confirm"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["CRM_Cust_ID"], dtHdr.Rows[0]["Global_Cust_ID"], dtHdr.Rows[0]["IS_Acknowledge"], dtHdr.Rows[0]["IS_Dealer"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["DealerRemark"], dtHdr.Rows[0]["AlternateContact_No"], dtHdr.Rows[0]["DDM_District_ID"], dtHdr.Rows[0]["Vehicle_No"], dtHdr.Rows[0]["Call_Cancel"]);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, iDealerId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_CRM_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Ticket_No"], dtHdr.Rows[0]["Ticket_Date"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["Call_Type_ID"], dtHdr.Rows[0]["Call_Sub_Type_ID"], dtHdr.Rows[0]["Call_Confirm"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["CRM_Cust_ID"], dtHdr.Rows[0]["Global_Cust_ID"], dtHdr.Rows[0]["IS_Acknowledge"], dtHdr.Rows[0]["IS_Dealer"], dtHdr.Rows[0]["UserId"], dtHdr.Rows[0]["DealerRemark"], dtHdr.Rows[0]["AlternateContact_No"], dtHdr.Rows[0]["DDM_District_ID"], dtHdr.Rows[0]["Vehicle_No"], dtHdr.Rows[0]["Call_Cancel"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool bSaveOpen(int iHdrID)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save HeaderFunc.DB.BeginTranasaction();
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedureAndGetObject("SP_CRM_Save_ISCalOpen", iHdrID);
                bSaveRecord = true;
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
        }
    }
}
