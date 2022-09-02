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
    /// Summary description for clsVehicleTarget
    /// </summary>
    public class clsVehicleTarget
    {
        public clsVehicleTarget()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //vrushali14032011_Begin
        //public DataSet GetVehicleTargetAnnual(string FType, int iDealerID,int iModelCatID, int iYearID, string  cStatus)
        public DataSet GetVehicleTargetAnnualMaster(string FType, string sDealerID, int iYearID)
        //vrushali14032011_End
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                //vrushali14032011_Begin
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_TargetMaster_Dtls", FType, iDealerID, iModelCatID, iYearID, cStatus);
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_TargetMaster_Dtls", FType, sDealerID, iModelCatID, iYearID, cStatus);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_VehicleTargetHeader", FType, sDealerID, iYearID);
                //vrushali14032011_End
                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
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
        //vrushali14032011_End
        // public DataSet GetVehicleTargetAnnualDetails(string FType, int sDealerID, int iModelCatID, int iTonnageID, int iYearID)
        public DataSet GetVehicleTargetAnnualDetails(int sDealerID, int iModelCatID, int iYearID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();

                //vrushali14032011_Begin
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_TargetMaster_Dtls", FType, iDealerID, iModelCatID, iYearID, cStatus);
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_TargetMaster_Dtls", FType, sDealerID, iModelCatID, iYearID, cStatus);
                //dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_VehicleTargetDetail", FType, sDealerID, iModelCatID, iTonnageID, iYearID);
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_VehicleTargetDetail", sDealerID, iModelCatID, iYearID);
                //vrushali14032011_End
                if (dsDetails != null)
                {
                    return dsDetails;
                }
                else
                    return null;
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

        // public bool bMonthlyTarget(string Ftype, int sDealerID, int iMdId, int iTonnageID, int iYearID, DataTable dtDet)
        // {
        public bool bMonthlyTarget(int sDealerID, int iMdId, int iYearID, DataTable dtDet)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {


                objDB.BeginTranasaction();
                //if (Ftype == "V")
                //{
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    //objDB.ExecuteStoredProcedure("SP_Save_VehicleTargetDetail", Ftype, sDealerID, iMdId, iTonnageID, iYearID,
                    //    dtDet.Rows[iRowCnt]["MonthId"], dtDet.Rows[iRowCnt]["Montly_Value"], dtDet.Rows[iRowCnt]["T1_Qty"], dtDet.Rows[iRowCnt]["T2_Qty"],
                    //     dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]
                    //    , dtDet.Rows[iRowCnt]["Retail_Qty"]);
                    objDB.ExecuteStoredProcedure("SP_Save_VehicleTargetDetail", sDealerID, iMdId, iYearID,
                       dtDet.Rows[iRowCnt]["MonthId"], dtDet.Rows[iRowCnt]["Montly_Value"], dtDet.Rows[iRowCnt]["T1_Qty"], dtDet.Rows[iRowCnt]["T2_Qty"],
                        dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]
                       , dtDet.Rows[iRowCnt]["Retail_Qty"]);
                }
                //Megha28052012 
                bSaveRecord = true;
                //Megha28052012
                //  }
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

        public bool bAnnualTarget(string Ftype, DataTable dtDet)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                if (Ftype == "V")
                {
                    for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                    {
                        //objDB.ExecuteStoredProcedure("SP_Save_VehicleTargetHeader", Ftype, dtDet.Rows[iRowCnt]["Dealer_Id"], dtDet.Rows[iRowCnt]["Year_Id"],
                        //    dtDet.Rows[iRowCnt]["Model_Cat_Id"],
                        //    //dtDet.Rows[iRowCnt]["Tonnage_ID"],
                        //    dtDet.Rows[iRowCnt]["Yearly_Value"]);
                        objDB.ExecuteStoredProcedure("SP_Save_VehicleTargetHeader", Ftype, dtDet.Rows[iRowCnt]["Dealer_Id"], dtDet.Rows[iRowCnt]["Year_Id"], dtDet.Rows[iRowCnt]["Model_Cat_Id"], dtDet.Rows[iRowCnt]["Yearly_Value"]);
                    }
                    //Megha28052012 
                    bSaveRecord = true;
                    //Megha28052012
                }
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
        //To Save Model Record 
        //vrushali14032011_Begin
        //public bool bSaveRecordWithModel(string Ftype, int iDealerID, int iYearID, DataTable dtDet)
        public bool bSaveRecordWithModel(string Ftype, string sDealerID, int iYearID, DataTable dtDet)
        //vrushali14032011_End
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();

                // Save Details 
                //vrushali14032011_Begin 
                //if (bSaveRecordWithModelAnnualTarget(Ftype , iDealerID, iYearID, dtDet) == false) goto ExitFunction;
                if (bSaveRecordWithModelAnnualTarget(objDB, Ftype, sDealerID, iYearID, dtDet) == false) goto ExitFunction;
                //vrushali14032011_end
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
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        // To Save Record 
        //vrushali14032011_Begin
        //private bool bSaveRecordWithModelAnnualTarget(string  FType, int iDealerID, int iYearID, DataTable dtDet)
        private bool bSaveRecordWithModelAnnualTarget(clsDB objDB, string FType, string sDealerID, int iYearID, DataTable dtDet)
        //vrushali14032011_End
        {
            //saveDetails
            bool bSaveRecord = false;

            int DealerID = 0;

            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]) == 0)
                    {
                        //vrushali14032011_Begin 
                        if (Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Month_Id"]) == 13)
                        {
                            DealerID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["DealerID"]);
                        }
                        else
                        {
                            DealerID = Func.Convert.iConvertToInt(sDealerID);
                        }

                        //objDB.ExecuteStoredProcedure("SP_TargetMaster_Save", FType, dtDet.Rows[iRowCnt]["ID"], iDealerID, dtDet.Rows[iRowCnt]["Model_cat_Id"], iYearID, dtDet.Rows[iRowCnt]["Month_Id"], dtDet.Rows[iRowCnt]["T1_Qty"], dtDet.Rows[iRowCnt]["T2_Qty"], dtDet.Rows[iRowCnt]["Retail_Qty"], dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]);
                        objDB.ExecuteStoredProcedure("SP_TargetMaster_Save", FType, dtDet.Rows[iRowCnt]["ID"], DealerID, dtDet.Rows[iRowCnt]["Model_cat_Id"], iYearID, dtDet.Rows[iRowCnt]["Month_Id"], dtDet.Rows[iRowCnt]["T1_Qty"], dtDet.Rows[iRowCnt]["T2_Qty"], dtDet.Rows[iRowCnt]["Retail_Qty"], dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]);
                        //vrushali14032011_End
                    }
                    else
                    {
                        //vrushali14032011_Begin
                        if (Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["Month_Id"]) == 13)
                        {
                            DealerID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["DealerID"]);
                        }
                        else
                        {
                            DealerID = Func.Convert.iConvertToInt(sDealerID);
                        }

                        //objDB.ExecuteStoredProcedure("SP_TargetMaster_Save", FType, dtDet.Rows[iRowCnt]["ID"], iDealerID, dtDet.Rows[iRowCnt]["Model_cat_Id"], iYearID, dtDet.Rows[iRowCnt]["Month_Id"], dtDet.Rows[iRowCnt]["T1_Qty"], dtDet.Rows[iRowCnt]["T2_Qty"], dtDet.Rows[iRowCnt]["Retail_Qty"], dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]);
                        objDB.ExecuteStoredProcedure("SP_TargetMaster_Save", FType, dtDet.Rows[iRowCnt]["ID"], DealerID, dtDet.Rows[iRowCnt]["Model_cat_Id"], iYearID, dtDet.Rows[iRowCnt]["Month_Id"], dtDet.Rows[iRowCnt]["T1_Qty"], dtDet.Rows[iRowCnt]["T2_Qty"], dtDet.Rows[iRowCnt]["Retail_Qty"], dtDet.Rows[iRowCnt]["OrdIntentPlnNo"], dtDet.Rows[iRowCnt]["BillPossNo"]);
                        //vrushali14032011_End          
                        //                    iTargetID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);                    
                    }
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
