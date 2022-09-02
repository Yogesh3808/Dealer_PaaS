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
    /// Summary description for clsVehicle
    /// </summary>
    public class clsVehicle
    {
        public clsVehicle()
        {
        }


        public int bSaveLeadTransactionHdr(int iID, int dealerID, int HOBrID, string LeadNo, string LeadDate, int LeadID,
            int Inqsource, string SourceName, string SourceAdd, string SourceMob, int Area, int AttendedBy, int AllocatedTo,
            int Route_ID, int Cust_Type_ID, int Drive_ID, int Load_type_ID, int Finac_Type_ID, int Industry_type_ID, int Road_ID,
            int pri_app_code, int sec_app_code, int Brand, string LikelyDate,
            string LoadPermit, string TieUpdBodyBuilder, string LocalMech, string MechName, string MechAdd, string MechPhone,
            int ModelCat, int MPG, int Model, string Cancel, string Confirm, string Hold, string Cash, string FinancierYN, int Finance,
            string QutYN, string QutDate
            )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_LeadTransaction_save", iID, dealerID, HOBrID, LeadNo, LeadDate,
                    LeadID, Inqsource, SourceName, SourceAdd, SourceMob, Area, AttendedBy, AllocatedTo, Route_ID, Cust_Type_ID, Drive_ID, Load_type_ID, Finac_Type_ID
                    , Industry_type_ID, Road_ID, pri_app_code, sec_app_code, Brand, LikelyDate,
                    LoadPermit, TieUpdBodyBuilder, LocalMech, MechName, MechAdd, MechPhone, ModelCat, MPG, Model, Cancel, Confirm,
                    Hold, Cash, FinancierYN, Finance, QutYN, QutDate);

                objDB.CommitTransaction();


                string sFinYear = Func.sGetFinancialYear(dealerID);
                //int iMaxDocNo;
                //sFinYearChar = sFinYear.Substring(3);

                // 'Commented by Shyamal as on 26032012
                //objDB.BeginTranasaction();
                string sDocName = "";
                sDocName = "L";



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

        public bool bSaveM1Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M1Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }

                    //else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    //{
                    //    //To Delete
                    //    objDB.ExecuteStoredProcedure("SP_DelLeadObjective", dtDet.Rows[iRowCnt]["ID"], "L");
                    //}



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveM2Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M2Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }

                  

                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM3Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M3Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM4Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M4Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM5Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M5Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM6Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M6Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM7Objectives(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M7Objectives", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }


        public int bSaveM1Header(int iID, int dealerID, int HOBrID, string LeadNo, string LeadDate,
        int Inqsource, int Area, int AttendedBy, int AllocatedTo,
        int Route_ID, int Cust_Type_ID, int Drive_ID, int Load_type_ID, int Finac_Type_ID, int Industry_type_ID, int Road_ID,
        int pri_app_code, int sec_app_code, string LikelyDate,
         int MPG, int Model, string Cancel, string Confirm,
        int modcat, double Qty, int M0ID, int LikelyBuyID,
        string Source_name, string Source_Address, string Source_Ph_no,
            string Load_Permit_Available, string Tie_Body_Builder, string Mech_Name, string Mech_Address, string Mech_PH_No
        , string Tie_Local_MEch, string OrderStatus,int PO_Type
            ,string AmcDet,string SpWarrDet,string Exwarrdet,string SpPackages, string OtherDet,int menu, string Enquiry
            , string LostAppNo, string DealerGST  
        )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();

                if (menu == 696)
                {
                    iID = objDB.ExecuteStoredProcedure("SP_Save_M1Header", iID, dealerID, HOBrID, LeadNo, LeadDate,
                         Inqsource, Area, AttendedBy, AllocatedTo, Route_ID, Cust_Type_ID, Drive_ID, Load_type_ID, Finac_Type_ID
                        , Industry_type_ID, Road_ID, pri_app_code, sec_app_code, LikelyDate,
                         MPG, Model, Cancel,
                          Confirm, modcat, Qty, M0ID, LikelyBuyID,
                          Source_name, Source_Address, Source_Ph_no, Load_Permit_Available, Tie_Body_Builder, Mech_Name, Mech_Address, Mech_PH_No, Tie_Local_MEch,
                          OrderStatus, PO_Type, AmcDet, SpWarrDet, Exwarrdet, SpPackages, OtherDet,"M",Enquiry,LostAppNo,DealerGST
                         );
                }
                else
                {

                    iID = objDB.ExecuteStoredProcedure("SP_Save_M1Header", iID, dealerID, HOBrID, LeadNo, LeadDate,
                         Inqsource, Area, AttendedBy, AllocatedTo, Route_ID, Cust_Type_ID, Drive_ID, Load_type_ID, Finac_Type_ID
                        , Industry_type_ID, Road_ID, pri_app_code, sec_app_code, LikelyDate,
                         MPG, Model, Cancel,
                          Confirm, modcat, Qty, M0ID, LikelyBuyID,
                          Source_name, Source_Address, Source_Ph_no, Load_Permit_Available, Tie_Body_Builder, Mech_Name, Mech_Address, Mech_PH_No, Tie_Local_MEch,
                          OrderStatus, PO_Type, AmcDet, SpWarrDet, Exwarrdet, SpPackages, OtherDet, "D", Enquiry, "", DealerGST
                         );
                }
                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                //string sDocName = "";
                //sDocName = "M1";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "M1", dealerID);
                    Func.Common.UpdateMaxNo(objDB, sFinYear, "EN", dealerID);
                }
                if (PO_Type == 3)
                {
                    if (OrderStatus == "Y")
                    {
                        if (menu == 696)
                        {
                            if (iHdrid != 0)
                            {
                                Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                            }
                        }
                    }
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

        public int bSaveM2Header(int iID, int dealerID, int HOBrID, string QutNo, string QutDate,
            string M2No, string M2Date,int Competitor, string CompModel,double CompDiscamt,string RFPNo, string RFPDate,string RFPGen,
            string Cancel, string Confirm,
            string OrderStatus, int M1ID, int POType, string LikelyBuyDate, int menu, string LossAppNo, int Modelcat, int model, int Qty, int DelWeeks
            ,string TCSApp,string StandardDisc
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M2Header", iID, dealerID, HOBrID, QutNo, QutDate
                    , RFPNo, RFPDate, RFPGen, Cancel, Confirm, OrderStatus, M1ID, M2No, M2Date, Competitor, CompModel, CompDiscamt, LikelyBuyDate,LossAppNo
                    , Modelcat, model, Qty, DelWeeks, TCSApp, StandardDisc
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M2";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }


                if(POType==1)
                { 
                    sDocName = "";
                    sDocName = "Q";
                    if (iHdrid == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }
                }
                else
                {
                    sDocName = "";
                }


                if (Confirm == "Y")
                {
                    if (menu == 681)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "D", dealerID);
                    }
                    else if (POType == 1 && StandardDisc=="Y")
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "D", dealerID);
                    }
                }

                if (POType == 3)
                {
                    if (OrderStatus == "Y")
                    {
                        if (menu == 681)
                        {
                            Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                        }
                    }
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

        public int bSaveM3Header(int iID, int dealerID, int HOBrID, string M3No, string M3Date,
            string PoNo, string PODate, string MTIProformaNo, string MTIProformaDate, 
            string remarks,string fund,
            string Cancel, string Confirm,
            string OrderStatus, int M2ID,string LikelyDate
            , string BookingAmt, string BookingDate, int POtype,int menu, string LossAppNo,int Financier
            ,string TCS_App
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M3Header", iID, dealerID, HOBrID, M3No, M3Date, PoNo, PODate, MTIProformaNo,
                    MTIProformaDate, remarks, fund, Cancel, Confirm, OrderStatus, M2ID, LikelyDate, BookingAmt, BookingDate, LossAppNo, Financier
                    , TCS_App
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M3";

                

                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }

                if (POtype == 1)
                {
                    //sDocName = "";
                    //sDocName = "PI";

                    if (iHdrid == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "PI", dealerID);
                    }
                }


                if (OrderStatus == "Y")
                {
                    if (menu == 682)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                    }
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

        public int bSaveM4Header(int iID, int dealerID, int HOBrID, string M4No, string M4Date,string Confirm,string Orderstatus,
           int M3ID,int M4Financier ,string Branch, string DocPending, string PendingDoc,double LoanAmt,double MarginMoney ,
          int Tenure,double Interest_Rate,string likelydate, string FullFund,int menu,string LossApp
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M4Header", iID, dealerID, HOBrID, M4No, M4Date,  M4Financier ,Branch, DocPending, PendingDoc,
                    LoanAmt,MarginMoney,Tenure,Interest_Rate,Confirm,Orderstatus,  M3ID,likelydate,FullFund,LossApp
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M4";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }

                if (Orderstatus == "Y")
                {
                    if (menu == 683)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                    }
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

        public int bSaveM5Header(int iID, int dealerID, int HOBrID, string M5No, string M5Date, string Confirm, string Orderstatus,
           int M4ID, string AggPDC, string PendingRemark,string likelydate,int menu,string LossApp
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M5Header", iID, dealerID, HOBrID, M5No, M5Date, AggPDC,PendingRemark, Confirm, Orderstatus, M4ID,likelydate,LossApp
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M5";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }

                if (Orderstatus == "Y")
                {
                    if (menu == 684)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                    }
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


        public int bSaveM6Header(int iID, int dealerID, int HOBrID, string M6No, string M6Date, string Confirm, string Orderstatus,
           int M5ID, string DONo ,string DODate,double DOAmt,double PaymentAmt,string PaymentDate,string Likelydate
            ,string AggPDC,string PendingRemarks,int menu,string LossApp
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M6Header", iID, dealerID, HOBrID, M6No, M6Date, DONo,DODate,DOAmt,PaymentAmt,PaymentDate, Confirm, Orderstatus, M5ID,Likelydate
                    ,AggPDC,PendingRemarks,LossApp
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M6";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }


                if (Orderstatus == "Y")
                {
                    if (menu == 685)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                    }
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

        public int bSaveM7Header(int iID, int dealerID, int HOBrID, string M7No, string M7Date, string Confirm, string Orderstatus,
           int PrvID,  double MarginAmt,  string CHRTGSDet,string CHRTGSDate,string CashLoan,int menu,string LossApp
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M7Header", iID, dealerID, HOBrID, M7No, M7Date,CashLoan,MarginAmt,CHRTGSDet,
                    CHRTGSDate , Confirm, Orderstatus, PrvID,LossApp
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M7";



                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }


                if (Orderstatus == "Y")
                {
                    if (menu == 686)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, "LA", dealerID);
                    }
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

        public int bSaveM8Header(int iID, int dealerID, int HOBrID, string M8_No,string M8_Date,int M7_Hdr,string IS_Confirm,string Is_Cancel,int ChangeFinc,
            double Loan_amt,double MarginMoney,int tenure,double Interest_rate,string Modelchange,string TrailerChassis,double TrailerAmt,
            int CST,int VAT,double PFCharges,double Other,double Total,double Rate,double Grandtotal,int tax1,int Tax2, double CSTAmt,
            double VATamt,double Tax1Amt,double Tax2Amt, int TCSPer,double TCSAmt,int AllotId,double Disc,int Qty,int TCS_App,string taxtype,
            string DONo, string DODate, double DOAmt, string Branch, string chassisno,string Cname,string CAdd,string CCST,string CVAT,string CTIN
            ,string GSTNo
      )
        {
            
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_M8Header", iID, dealerID, HOBrID,
                    M8_No, M8_Date, M7_Hdr, IS_Confirm, Is_Cancel, ChangeFinc, Loan_amt, MarginMoney, tenure, Interest_rate,
                    Modelchange, TrailerChassis, TrailerAmt, CST, VAT, PFCharges, Other, Total, Rate, Grandtotal, tax1, Tax2, CSTAmt, VATamt, Tax1Amt,
                    Tax2Amt, TCSPer, TCSAmt, AllotId, Disc, Qty, TCS_App, taxtype, DONo, DODate, DOAmt, Branch, chassisno, Cname, CAdd, CCST, CVAT, CTIN
                    ,GSTNo
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "M8";



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

        public int bSaveDealerSale(int iID, int dealerID, int HOBrID, string M8_No, string M8_Date, string IS_Confirm, string Is_Cancel,
            int CST, int VAT, double PFCharges, double Other, double Total, double Rate, double Grandtotal, int tax1, int Tax2, double CSTAmt,
            double VATamt, double Tax1Amt, double Tax2Amt, int TCSPer, double TCSAmt, double Disc, int Qty, int TCS_App, string taxtype
            ,int StockID,int PO_Id ,int SaleDealer ,int ModelID,int ModelGP ,int Chassis, int Cust,int SalePOId,int ModelCat,string ISGST


      )
        {

            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_DealerSale", iID, dealerID, HOBrID,
                    M8_No, M8_Date,IS_Confirm, Is_Cancel,CST, VAT, PFCharges, Other, Total, Rate, Grandtotal, tax1, Tax2, CSTAmt, VATamt, Tax1Amt,
                    Tax2Amt, TCSPer, TCSAmt, Disc, Qty, TCS_App, taxtype, StockID, PO_Id, SaleDealer, ModelID, ModelGP, Chassis, Cust, SalePOId, ModelCat, ISGST
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "DS";



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
        public int bSaveVehSRN(int iID, int dealerID, int HOBrID, string M8_No, string M8_Date, string IS_Confirm, string Is_Cancel,
            int CST, int VAT, double PFCharges, double Other, double Total, double Rate, double Grandtotal, int tax1, int Tax2, double CSTAmt,
            double VATamt, double Tax1Amt, double Tax2Amt, int TCSPer, double TCSAmt, double Disc, int Qty, int TCS_App, string taxtype
            , int ModelID, int ModelGP, int Chassis, int Cust,int retailhdr,string Saletype ,string engineno,int ModelCat,string Vehgst


      )
        {

            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_VehSRN", iID, dealerID, HOBrID,
                    M8_No, M8_Date, IS_Confirm, Is_Cancel, CST, VAT, PFCharges, Other, Total, Rate, Grandtotal, tax1, Tax2, CSTAmt, VATamt, Tax1Amt,
                    Tax2Amt, TCSPer, TCSAmt, Disc, Qty, TCS_App, taxtype, ModelID, ModelGP, Chassis, Cust, retailhdr, Saletype, engineno, ModelCat, Vehgst
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "SR";



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

        public int bSaveVehPRN(int iID, int dealerID, int HOBrID, string M8_No, string M8_Date, string IS_Confirm, string Is_Cancel,
           int CST, int VAT, double PFCharges, double Other, double Total, double Rate, double Grandtotal, int tax1, int Tax2, double CSTAmt,
           double VATamt, double Tax1Amt, double Tax2Amt, int TCSPer, double TCSAmt, double Disc, int Qty, int TCS_App, string taxtype
           , int ModelID, int ModelGP, int Chassis, int supp, int receipthdr, string typeid, string engineno,int modelcatID


     )
        {

            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_VehPRN", iID, dealerID, HOBrID,
                    M8_No, M8_Date, IS_Confirm, Is_Cancel, CST, VAT, PFCharges, Other, Total, Rate, Grandtotal, tax1, Tax2, CSTAmt, VATamt, Tax1Amt,
                    Tax2Amt, TCSPer, TCSAmt, Disc, Qty, TCS_App, taxtype, ModelID, ModelGP, Chassis, supp, receipthdr, typeid, engineno,modelcatID
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "PR";



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

        public int bSaveStockTransfer(int iID, int dealerID, int HOBrID, string M8_No, string M8_Date, string IS_Confirm, string Is_Cancel,
           double Total, double Rate, int Qty, int StockID, int PO_Id, int SaleDealer, int ModelID, int ModelGP, int Chassis, int Cust
            , string remarks, int SalePOId,int ModelCat
      )
        {

            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_StockTransfer", iID, dealerID, HOBrID,
                    M8_No, M8_Date, IS_Confirm, Is_Cancel, Total, Rate,
                     Qty,  StockID, PO_Id, SaleDealer, ModelID, ModelGP, Chassis, Cust
                     , remarks, SalePOId,ModelCat
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "STC";



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

        public int bSaveDealerPurchase(int iID, int dealerID, int HOBrID, string InvNo, string InvDate, string IS_Confirm, string Is_Cancel,
            int CST, int VAT, double PFCharges, double Other, double Total, double Rate, double Grandtotal, int tax1, int Tax2, double CSTAmt,
            double VATamt, double Tax1Amt, double Tax2Amt, int TCSPer, double TCSAmt, double Disc, int Qty, int TCS_App, string taxtype
            , int StockID, int purchDealer, int ModelID, int ModelGP,int Chassis, int supp,string tr_no,string ref_date,string Parking,int SalePO_ID
            ,int Modelcat,string GST


      )
        {

            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_DealerPurchase", iID, dealerID, HOBrID,
                    InvNo, InvDate, IS_Confirm, Is_Cancel, CST, VAT, PFCharges, Other, Total, Rate, Grandtotal, tax1, Tax2, CSTAmt, VATamt, Tax1Amt,
                    Tax2Amt, TCSPer, TCSAmt, Disc, Qty, TCS_App, taxtype, StockID, purchDealer, ModelID, ModelGP,Chassis,supp,tr_no,ref_date,Parking,SalePO_ID
                    ,Modelcat,GST
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "VI";



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


        public int bSaveStockTransferReceipt(int iID, int dealerID, int HOBrID, string InvNo, string InvDate, string IS_Confirm, string Is_Cancel,
             double Total, double Rate,  int Qty
            , int StockID, int purchDealer, int ModelID, int ModelGP, int Chassis, int supp, string tr_no, string ref_date, string Parking
            ,int VehCondition,int SalePO_Id,int ModelCat 


      )
        {

            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_StockTransferReceipt", iID, dealerID, HOBrID,
                    InvNo, InvDate, IS_Confirm, Is_Cancel, Total, Rate,
                     Qty, StockID, purchDealer, ModelID, ModelGP, Chassis, supp, tr_no, ref_date, Parking, VehCondition, SalePO_Id,ModelCat
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "STR";



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



        public int bSaveVehOrderForm(int iID, int dealerID, int HOBrID, string PONo, string PODate, string Confirm, string Cancel,
            int M7ID, int Model, int Plant, string depo, string qty, string modelrate, string RoadPermitNo, string RoadPermitDate,
           int modelgrp, string changeqty, string AppNo, string AppDate, int menu, string Appflag, string AppConfirm
           , string sAppNew,int TypeID,string ShipToparty,string SoldToParty,string PayerAdd,string DeliveryAdd,int ServiceDealer,int SoureDealer,
            string CSTNO,string VATNO,string PANNO,string LBTNO,string CSTExpDate,string VATExpDate,string FormNo,string EntryTax,string OnlineForm,
            string CheckPost, int BillingLocation, int BillingType, int TaxType, string InsuranceComp, string CoverNoteNo, string CoverNoteExp, string Remarks
            ,string Bank_Name,string Chq_No,string Chq_Date,int Finc,string Loc,string CST_CertNo,string EPCG_No,int UserRole,int ModelCAt,
            int PDIDealer,string ISGST,  string E_mail ,string Contact_Person ,string Contact_Person_Phone ,string pincode , string Delv_Contact_Person ,
            string Delv_Contact_phone_no ,string Delv_pincode , string Delv_E_mail , string Delv_PANNo  ,string Delv_GST_No 
            ,string Payment ,string DelvName,string DelvMob,int DelvState


      )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();

                if (menu == 657)
                {
                    iID = objDB.ExecuteStoredProcedure("SP_Save_VehOrderForm", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
                M7ID, Model, Plant, depo, qty, modelrate, RoadPermitNo, RoadPermitDate, modelgrp, changeqty, AppNo, AppDate, Appflag, AppConfirm,2
                ,ShipToparty,SoldToParty,PayerAdd,DeliveryAdd,ServiceDealer,SoureDealer,
                CSTNO, VATNO, PANNO, LBTNO, CSTExpDate, VATExpDate, FormNo, EntryTax, OnlineForm, CheckPost, BillingLocation, BillingType, TaxType,
                InsuranceComp, CoverNoteNo, CoverNoteExp, Remarks, Bank_Name, Chq_No, Chq_Date, Finc, Loc, CST_CertNo, EPCG_No, UserRole, ModelCAt, PDIDealer, ISGST,
                E_mail ,Contact_Person ,Contact_Person_Phone ,pincode ,Delv_Contact_Person ,Delv_Contact_phone_no ,Delv_pincode ,Delv_E_mail ,Delv_PANNo
                    , Delv_GST_No, Payment, DelvName, DelvMob, DelvState

                         );
                }
                else if (menu == 679)
                {
                    iID = objDB.ExecuteStoredProcedure("SP_Save_VehOrderForm", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
            M7ID, Model, Plant, depo, qty, modelrate, RoadPermitNo, RoadPermitDate, modelgrp, changeqty, AppNo, AppDate, Appflag, AppConfirm,3
           , ShipToparty, SoldToParty, PayerAdd, DeliveryAdd, ServiceDealer, SoureDealer,
           CSTNO, VATNO, PANNO, LBTNO, CSTExpDate, VATExpDate, FormNo, EntryTax, OnlineForm, CheckPost, BillingLocation, BillingType, TaxType,
           InsuranceComp, CoverNoteNo, CoverNoteExp, Remarks, Bank_Name, Chq_No, Chq_Date, Finc, Loc, CST_CertNo, EPCG_No, UserRole, ModelCAt, PDIDealer, ISGST
           , E_mail, Contact_Person, Contact_Person_Phone, pincode, Delv_Contact_Person, Delv_Contact_phone_no, Delv_pincode, Delv_E_mail, Delv_PANNo
                    , Delv_GST_No, Payment, DelvName, DelvMob, DelvState
                    );
                }
                if (menu == 660)
                {
                    iID = objDB.ExecuteStoredProcedure("SP_Save_VehOrderForm", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
                M7ID, Model, Plant, depo, qty, modelrate, RoadPermitNo, RoadPermitDate, modelgrp, changeqty, AppNo, AppDate, Appflag, AppConfirm, 2
                , ShipToparty, SoldToParty, PayerAdd, DeliveryAdd, ServiceDealer, SoureDealer,
                CSTNO, VATNO, PANNO, LBTNO, CSTExpDate, VATExpDate, FormNo, EntryTax, OnlineForm, CheckPost, BillingLocation,
                BillingType, TaxType, InsuranceComp, CoverNoteNo, CoverNoteExp, Remarks, Bank_Name, Chq_No, Chq_Date, Finc, Loc
                , CST_CertNo, EPCG_No, UserRole, ModelCAt, PDIDealer, ISGST, E_mail, Contact_Person, Contact_Person_Phone, pincode, Delv_Contact_Person,
                Delv_Contact_phone_no, Delv_pincode, Delv_E_mail, Delv_PANNo
                    , Delv_GST_No, Payment, DelvName, DelvMob, DelvState
                         );
                }
                else if (menu == 661)
                {
                    iID = objDB.ExecuteStoredProcedure("SP_Save_VehOrderForm", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
            M7ID, Model, Plant, depo, qty, modelrate, RoadPermitNo, RoadPermitDate, modelgrp, changeqty, AppNo, AppDate, Appflag, AppConfirm, 3
           , ShipToparty, SoldToParty, PayerAdd, DeliveryAdd, ServiceDealer, SoureDealer, CSTNO, VATNO, PANNO, LBTNO, CSTExpDate,
           VATExpDate, FormNo, EntryTax, OnlineForm, CheckPost, BillingLocation, BillingType, TaxType, InsuranceComp, CoverNoteNo, CoverNoteExp, Remarks
           , Bank_Name, Chq_No, Chq_Date, Finc, Loc
           , CST_CertNo, EPCG_No, UserRole, ModelCAt, PDIDealer, ISGST, E_mail, Contact_Person, Contact_Person_Phone, pincode, Delv_Contact_Person, Delv_Contact_phone_no, 
           Delv_pincode, Delv_E_mail, Delv_PANNo
                    , Delv_GST_No, Payment, DelvName, DelvMob, DelvState
                    );
                }
                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";


                if (menu == 657)
                {
                    sDocName = "DD";
                    if (iHdrid == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }
                }
                else if (menu == 679)
                {
                    sDocName = "DM";


                    //if (sAppNew == "Y")
                    if (iHdrid == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }
                    //}
                    sDocName = "DMA";
                    if (Confirm == "Y")
                    {
                        if (sAppNew == "Y")
                        {
                            Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                        }
                    }
                }

                else if (menu == 660)
                {
                    sDocName = "DDA";
                   
                        if (sAppNew == "Y")
                        {
                            Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                        }
                    

                }

                else if (menu == 661)
                {
                    sDocName = "DMA";
                   
                        if (sAppNew == "Y")
                        {
                            Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                        }
                   

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


        public int bSaveDealerSAlePO(int iID, int dealerID, int HOBrID, 
            int SuppID  ,string PO_No,string PO_Date ,int Model_GP ,int Model_ID,int qty ,double ModelPrice,string Is_Confirm ,
            string Is_Cancel ,int ModelCat
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_DealerSalePO", iID, dealerID, HOBrID,SuppID,
                    PO_No,PO_Date,Model_GP,Model_ID,1,ModelPrice,Is_Confirm,Is_Cancel,ModelCat
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "SP";
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
        public int bSaveStockTransferPO(int iID, int dealerID, int HOBrID,
            int SuppID, string PO_No, string PO_Date, int Model_GP, int Model_ID, int qty, double ModelPrice, string Is_Confirm,
            string Is_Cancel,int ModelCat
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_StockTransferPO", iID, dealerID, HOBrID, SuppID,
                    PO_No, PO_Date, Model_GP, Model_ID, 1, ModelPrice, Is_Confirm, Is_Cancel,ModelCat
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "STP";
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



        public int bSaveDealerVehPO(int iID, int dealerID, int HOBrID, string PONo, string PODate, string Confirm,string Cancel,
            int POType,int M7ID,int Model,int Plant,string depo,string qty,string modelrate,string RoadPermitNo, string RoadPermitDate,
            int modelgrp, string changeqty, string PaymentDetails,string AppNo,string AppDate,int menu,string Appflag,string AppConfirm
            , string sAppNew,int Finc,string Loc,int UserRole,int ModelCat
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                string DoneBy = "";

                if (menu==711)
                {
                    DoneBy = "Y";
                }
                else
                {
                    DoneBy="N";
                }

                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_DealerVehPO", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
            POType,M7ID,Model, Plant,depo,qty,modelrate,RoadPermitNo, RoadPermitDate,modelgrp ,changeqty,PaymentDetails,AppNo,AppDate,Appflag,AppConfirm,1
            , Finc, Loc, DoneBy,UserRole,ModelCat
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";


                if (menu==647)
                { 
                sDocName = "PO";
                    if (iHdrid == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }
                }

                if (menu == 711)
                {
                    sDocName = "PO";
                    if (iHdrid == 0)
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }
                sDocName = "POA";
                      if (sAppNew == "Y")
                    
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }
                }
                    
                else if (menu==648)
                {
                    sDocName = "POA";


                    if (sAppNew == "Y")
                    {
                        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                    }


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

        public int bSaveRFP(int iID, int dealerID, int HOBrID, string PONo, string PODate, string Confirm, string Cancel,
            int M7ID, int Model, string AppNo, string AppDate, int menu, string Appflag, string AppConfirm
            , int UserRole, int ModelCat, string remarks, string sAppNew,string Appremarks,double List,double MRP,int TaxID,string GST
       )
        {



            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_RFP", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
             M7ID, Model, AppNo, AppDate, Appflag, AppConfirm, UserRole, ModelCat, remarks,Appremarks,List,MRP,TaxID,GST
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";



                sDocName = "RF";
                if (iHdrid == 0)
                {
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }

                if(sAppNew=="Y")
                {
                    sDocName = "RFA";
                    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                }




                //else if (menu == 648)
                //{
                //    sDocName = "POA";


                //    if (sAppNew == "Y")
                //    {
                //        Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                //    }


                //}



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



        public int bSaveVehExRFP(int iID, int dealerID, int HOBrID, string PONo, string PODate, string Confirm, string Cancel,
          int Model,  int menu, int UserRole, int ModelCat, string remarks, double MRP, int Qty,double total
      )
        {



            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_VehExRFP", iID, dealerID, HOBrID, PONo, PODate, Confirm, Cancel,
              Model,  UserRole, ModelCat, remarks, MRP, Qty,total
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";



                sDocName = "VRF";
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



        public int bSaveVehExProformaAcceptance(int iID, int dealerID, 
            string AccNo ,string Accdate,string SAPInv,string saPInvdate,
            string PONo, string PODate, string Confirm, string Cancel,
          int Model, int menu, int UserRole, int ModelCat, double MRP, int Qty, double total
      )
        {



            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_VehExProformaAcceptance", iID, dealerID,AccNo,Accdate,SAPInv,saPInvdate, PONo, PODate, Confirm, Cancel,
              Model, UserRole, ModelCat, MRP, Qty, total
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";



                sDocName = "VPA";
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







        public int bSaveDealerVehReceipt(int iID, int dealerID, int HOBrID,string Tr_no, string ref_date,string inv_no,string inv_date,string delivery_No,
            string PO_No,string PO_date,string inv_amt,string chassis_no,string engine_no,int model_ID,int Model_GP,string rate,
            string Qty,string TotalAmt,string Discount,int CST_TaxID,string CST_Amt,int VAT_TaxID,string VAT_Amt, int Add1_TaxID,string VATSURCHAMT,
            int Add2_TaxID,string ADDVATAMT, string TCS_Per,string TCS_Amt,string ParkingLocation,int VehConditionID,string Is_Confirm,string Is_Cancel
            ,string PF, string other,int Modelcat,string vehGST
       )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_dealerVehReceipt", iID, dealerID, HOBrID, Tr_no, ref_date, inv_no, inv_date, delivery_No, PO_No, PO_date, inv_amt, chassis_no, engine_no, model_ID, Model_GP,
rate, Qty, TotalAmt, Discount, CST_TaxID, CST_Amt, VAT_TaxID, VAT_Amt, Add1_TaxID, VATSURCHAMT, Add2_TaxID, ADDVATAMT, TCS_Per, TCS_Amt,
 ParkingLocation, VehConditionID, Is_Confirm, Is_Cancel, PF, other, Modelcat,vehGST
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";


               
                    sDocName = "VR";
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



        public int bSaveDirectBillingReceipt(int iID, int dealerID, int HOBrID, string Tr_no, string ref_date, string inv_no, string inv_date, string delivery_No,
            string PO_No, string PO_date, string inv_amt, string chassis_no, string engine_no, int model_ID, int Model_GP, string rate,
            string Qty, string TotalAmt, string Discount, int CST_TaxID, string CST_Amt, int VAT_TaxID, string VAT_Amt, int Add1_TaxID, string VATSURCHAMT,
            int Add2_TaxID, string ADDVATAMT, string TCS_Per, string TCS_Amt, string ParkingLocation, string Is_Confirm, string Is_Cancel
            , string PF, string other,string Customer,int type,int menu,int ModelCat,string DBRGST
       )
        {
         
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Save_DirectBillingReceipt", iID, dealerID, HOBrID, Tr_no, ref_date, inv_no, inv_date, delivery_No, PO_No, PO_date, inv_amt, chassis_no, engine_no, model_ID, Model_GP,
rate, Qty, TotalAmt, Discount, CST_TaxID, CST_Amt, VAT_TaxID, VAT_Amt, Add1_TaxID, VATSURCHAMT, Add2_TaxID, ADDVATAMT, TCS_Per, TCS_Amt,
 ParkingLocation, Is_Confirm, Is_Cancel, PF, other, Customer, type, ModelCat, DBRGST
                     );

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";


                if (menu == 662)
                {
                    sDocName = "VD";
                }
                else if (menu == 663)
                {
                    sDocName = "VM";
                }

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

        public int bSaveDiscAppDet(int iID, int dealerID, int HOBrID, string AppNo, string AppDate,
          double AppDiscAmt,double AppDealerShare,double AppMTIShare,string remarks,
           string Cancel, string Confirm,
           string OrderStatus, int M2ID
      )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_Save_DiscApproval", iID, dealerID, HOBrID, AppNo, AppDate, AppDiscAmt, AppDealerShare
                    ,AppMTIShare,remarks,Cancel, Confirm,OrderStatus,M2ID);

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "D";



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
        public int bSaveProformaInvDirectBilling(int iID, int dealerID, int HOBrID, string AppNo, string AppDate,
          double AppDiscAmt, double AppDealerShare, double AppMTIShare, string remarks,
           string Cancel, string Confirm,
           string OrderStatus, int M2ID
      )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_Save_ProformaInvDirectBilling", iID, dealerID, HOBrID, AppNo, AppDate, AppDiscAmt, AppDealerShare
                    , AppMTIShare, remarks, Cancel, Confirm, OrderStatus, M2ID);

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "MPI";



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

        public int bSaveLossDetApp(int iID, int dealerID, int HOBrID, string AppNo, string AppDate,
      string remarks,string Doctype,int DocID,string Is_Approve
           
      )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_Save_LossApproval", iID, dealerID, HOBrID, AppNo, AppDate, remarks, Doctype, DocID, Is_Approve);

                objDB.CommitTransaction();

                string sFinYear = Func.sGetFinancialYear(dealerID);

                string sDocName = "";
                sDocName = "LA";



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

        public bool bSaveM2QuotationDtls(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.BeginTranasaction();

                    //,,,,,,

                    objDB.ExecuteStoredProcedure("SP_SaveM2QuotationDetails", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["QuotId"],
                        dtDet.Rows[iRowCnt]["Value"]
                        );


                    objDB.CommitTransaction();

                }

                bSaveRecord = true;
            }


            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM1ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M1LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM2ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M2LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM3ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M3LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM4ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M4LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM5ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M5LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM6ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M6LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM7ClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("SP_Save_M7LostReason", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"], dtDet.Rows[iRowCnt]["Qty"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveM1FleetDtls(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.BeginTranasaction();

                   

                    objDB.ExecuteStoredProcedure("SP_Save_M1FleetDetails", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["CompID"],
                       dtDet.Rows[iRowCnt]["model1"], dtDet.Rows[iRowCnt]["qty1"], dtDet.Rows[iRowCnt]["model2"],
                        dtDet.Rows[iRowCnt]["qty2"], dtDet.Rows[iRowCnt]["model3"], dtDet.Rows[iRowCnt]["qty3"]
                        );

                    objDB.CommitTransaction();

                }

                bSaveRecord = true;
            }


            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveLeadObjDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("sp_LeadObj_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["plt_Id"], dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }

                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadObjective", dtDet.Rows[iRowCnt]["ID"], "L");
                    }



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveLeadClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("sp_LeadClosure_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveInqClosureDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {

                        if (dtDet.Rows[iRowCnt]["rsn_id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("sp_InqClosure_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["rsn_id"]
                            , dtDet.Rows[iRowCnt]["CompID"], dtDet.Rows[iRowCnt]["CompetitorMake"]
                                );

                            objDB.CommitTransaction();

                        }
                    }
                    else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    {
                        //To Delete
                        objDB.ExecuteStoredProcedure("SP_DelLeadCloseRsn", dtDet.Rows[iRowCnt]["ID"], "L");
                    }

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveLeadFleetDtls(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.BeginTranasaction();

                    //,,,,,,

                    objDB.ExecuteStoredProcedure("sp_LeadFleet_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["CompID"],
                        dtDet.Rows[iRowCnt]["HDTQty"], dtDet.Rows[iRowCnt]["HDBusQty"], dtDet.Rows[iRowCnt]["LDTQty"],
                        dtDet.Rows[iRowCnt]["LDBusQty"], dtDet.Rows[iRowCnt]["MDTQty"], dtDet.Rows[iRowCnt]["MDBusQty"], dtDet.Rows[iRowCnt]["EngQty"]
                        );

                    objDB.CommitTransaction();

                }

                bSaveRecord = true;
            }


            catch
            {

            }
            return bSaveRecord;
        }




        public bool bSaveLeadQuotationDtls(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.BeginTranasaction();

                    //,,,,,,

                    objDB.ExecuteStoredProcedure("sp_LeadQuotation_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["QuotId"],
                        dtDet.Rows[iRowCnt]["Value"]
                        );


                    objDB.CommitTransaction();

                }

                bSaveRecord = true;
            }


            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveProspect(clsDB objDB, int dealerid, int hobrId, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {

                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("sp_SaveProspect", dealerid, hobrId, iHdrID
                );

                objDB.CommitTransaction();


                bSaveRecord = true;
            }
            catch
            {

            }

            return bSaveRecord;
        }

        public bool bSavecustomer(clsDB objDB, int dealerid, int hobrId, int iHdrID, string Cust)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {

                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("sp_SaveCustomer", dealerid, hobrId, iHdrID, Cust
                );

                objDB.CommitTransaction();


                bSaveRecord = true;
            }
            catch
            {

            }

            return bSaveRecord;
        }

        public int iConversionLeadInq(clsDB objDB, int iHdrID, string Type, string stage, string BuyDate)
        {

            int Value = 0;


            try
            {

                objDB.BeginTranasaction();

                Value = objDB.ExecuteStoredProcedure("sp_getLeadInqconversion", Type, stage, iHdrID, BuyDate);

                objDB.CommitTransaction();



            }
            catch
            {

            }

            return Value;
        }




        public int bSaveInqHdr(clsDB objDB, int dealerid, int hobrId, int iHdrID, string InqNo, int InqID)
        {
            //saveDetails

            //int InqID = 0;


            int iHdrid = 0;
            iHdrid = InqID;

            try
            {

                objDB.BeginTranasaction();

                InqID = objDB.ExecuteStoredProcedure("sp_SaveinquiryHdr", dealerid, hobrId, iHdrID, InqNo);




                objDB.CommitTransaction();





            }
            catch
            {

            }

            string sFinYear = Func.sGetFinancialYear(dealerid);

            string sDocName = "";
            sDocName = "IQ";



            if (iHdrid == 0)
            {
                Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerid);
            }


            return InqID;
        }


        public bool bSaveInqDtls(clsDB objDB, int dealerid, int hobrId, int iHdrID, int InqID)
        {
            //saveDetails

            bool bSaveRecord = false;



            try
            {

                objDB.BeginTranasaction();

                objDB.ExecuteStoredProcedure("SP_SaveInqdtls", dealerid, hobrId, iHdrID, InqID);


                objDB.CommitTransaction();


                bSaveRecord = true;


            }
            catch
            {

            }

            return bSaveRecord;
        }







        public int bSaveInqTransactionHdr(int iID, int dealerID, int HOBrID, string LeadNo, string LeadDate, int LeadID,
            int Inqsource, int Area, int AttendedBy, int AllocatedTo,
            int Route_ID, int Cust_Type_ID, int Drive_ID, int Load_type_ID, int Finac_Type_ID, int Industry_type_ID, int Road_ID,
            int pri_app_code, int sec_app_code, string LikelyDate,
             int MPG, int Model, string Cancel, string Cash, string FinancierYN, int Finance, int InqStage, string Confirm,
            string Orderstatus, string DONo, string DoDate, string AdvPaid, double AdvAmt, string AdvDate, string ExpDate,
            int modcat, string CancConfDate
            )
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                int iHdrid = 0;
                iHdrid = iID;

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("sp_InqTransaction_save", iID, dealerID, HOBrID, LeadNo, LeadDate,
                    LeadID, Inqsource, Area, AttendedBy, AllocatedTo, Route_ID, Cust_Type_ID, Drive_ID, Load_type_ID, Finac_Type_ID
                    , Industry_type_ID, Road_ID, pri_app_code, sec_app_code, LikelyDate,
                     MPG, Model, Cancel,
                     Cash, FinancierYN, Finance, InqStage, Confirm, Orderstatus, DONo, DoDate, AdvPaid,
                     AdvAmt, AdvDate, ExpDate, modcat, CancConfDate
                     );

                objDB.CommitTransaction();


                //string sFinYear = Func.sGetFinancialYear(dealerID);

                //string sDocName = "";
                //sDocName = "L";



                //if (iHdrid == 0)
                //{
                //    Func.Common.UpdateMaxNo(objDB, sFinYear, sDocName, dealerID);
                //}
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


        public bool bSaveInqObjDetails(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    if (dtDet.Rows[iRowCnt]["Status"] != "D")
                    {
                        if (dtDet.Rows[iRowCnt]["obj_Id"].ToString() != "0")
                        {
                            objDB.BeginTranasaction();

                            objDB.ExecuteStoredProcedure("sp_InqObj_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["obj_Id"], Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["obj_date"], false),
                            dtDet.Rows[iRowCnt]["discussion"], dtDet.Rows[iRowCnt]["time_spent"], dtDet.Rows[iRowCnt]["next_obj_Id"],
                             Func.Convert.tConvertToDate(dtDet.Rows[iRowCnt]["next_date"], false), dtDet.Rows[iRowCnt]["plt_Id"], dtDet.Rows[iRowCnt]["commit_det"]
                                );

                            objDB.CommitTransaction();

                        }
                    }

                    //else if (dtDet.Rows[iRowCnt]["Status"] == "D")
                    //{
                    //    //To Delete
                    //    objDB.ExecuteStoredProcedure("SP_DelLeadObjective", dtDet.Rows[iRowCnt]["ID"], "L");
                    //}



                }
                bSaveRecord = true;

            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveWarrantyClaimFileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_OrderForm_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_OrderForm_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_DealerPO_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
        public bool bSaveM3FileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_M3_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_M3_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_M3_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveM6FileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_M6_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_M6_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_M6_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public bool bSaveM7FileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_M7_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_M7_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_M7_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }
         
        
        public bool bSaveM8FileAttachDetails(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_M8_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_M8_AttchFiles_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_M8_AttchFiles_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveGRNChecklistAttachment(clsDB objDB, DataTable dtFileAttach, int iHdrID)
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
                            iWarrantyDetID = objDB.ExecuteStoredProcedure("SP_GRNChecklist_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                        else
                        {
                            objDB.ExecuteStoredProcedure("SP_GRNChecklist_Save", dtFileAttach.Rows[iRowCnt]["ID"], iHdrID, dtFileAttach.Rows[iRowCnt]["File_Names"], dtFileAttach.Rows[iRowCnt]["Description"], dtFileAttach.Rows[iRowCnt]["UserId"]);
                        }
                    }
                    else if (dtFileAttach.Rows[iRowCnt]["Status"].ToString() == "D")
                    {
                        objDB.ExecuteStoredProcedure("SP_GRNChecklist_Del", dtFileAttach.Rows[iRowCnt]["ID"]);
                    }
                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveInqFleetDtls(clsDB objDB, int dealerid, int hobrId, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {

                    objDB.BeginTranasaction();

                    //,,,,,,

                    objDB.ExecuteStoredProcedure("sp_InqFleet_save", dtDet.Rows[iRowCnt]["ID"], dealerid, hobrId, iHdrID, dtDet.Rows[iRowCnt]["CompID"],
                        dtDet.Rows[iRowCnt]["HDTQty"], dtDet.Rows[iRowCnt]["HDBusQty"], dtDet.Rows[iRowCnt]["LDTQty"],
                        dtDet.Rows[iRowCnt]["LDBusQty"], dtDet.Rows[iRowCnt]["MDTQty"], dtDet.Rows[iRowCnt]["MDBusQty"], dtDet.Rows[iRowCnt]["EngQty"]
                        );

                    objDB.CommitTransaction();

                }

                bSaveRecord = true;
            }


            catch
            {

            }
            return bSaveRecord;
        }


    }
}
