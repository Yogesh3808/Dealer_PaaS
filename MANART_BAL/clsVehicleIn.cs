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
    /// Summary description for clsVehicleIn
    /// </summary>
    public class clsVehicleIn
    {
        public clsVehicleIn()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public bool bSaveVehicleInHdr(ref int iHdrID, DataTable dtHdr, DataTable dtDet)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                //Save Header

                if (dtHdr.Rows.Count > 0)
                {
                    string sFinYear = Func.sGetFinancialYear(0);
                    objDB.BeginTranasaction();
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                    if (iHdrID == 0)
                        iHdrID = objDB.ExecuteStoredProcedure("SP_VehicleInHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PostShipmentInv_ID"], dtHdr.Rows[0]["Proforma_ID"], dtHdr.Rows[0]["Receipt_No"], dtHdr.Rows[0]["Receipt_Date"], dtHdr.Rows[0]["Receipt_Confirm"]);
                    else
                        objDB.ExecuteStoredProcedure("SP_VehicleInHdr_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["PostShipmentInv_ID"], dtHdr.Rows[0]["Proforma_ID"], dtHdr.Rows[0]["Receipt_No"], dtHdr.Rows[0]["Receipt_Date"], dtHdr.Rows[0]["Receipt_Confirm"]);
                }

                // Save Details
                if (bSaveVehicleInDetails(objDB, dtDet, iHdrID) == false)
                {
                    bSaveRecord = false;
                    objDB.RollbackTransaction();
                }
                else
                {
                    objDB.CommitTransaction();
                    bSaveRecord = true;
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
        private bool bSaveVehicleInDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_VehicleInDetail_Save", dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["VDID"], iHdrID, dtDet.Rows[iRowCnt]["ModelID"], dtDet.Rows[iRowCnt]["ChassisNo"], dtDet.Rows[iRowCnt]["EngineNo"]);

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }


        public bool bSaveVehicleInRemarkChkList(DataTable dtRemark, DataTable dtChkList)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtRemark.Rows.Count; iRowCnt++)
                {
                    //Sujata 18102013_Begin
                    //objDB.ExecuteStoredProcedure("SP_VehicleInRemark_Save", dtRemark.Rows[iRowCnt]["ID"], dtRemark.Rows[iRowCnt]["VehicleInID"], dtRemark.Rows[iRowCnt]["ModelID"], dtRemark.Rows[iRowCnt]["ParameterID"], dtRemark.Rows[iRowCnt]["Remark"]);
                    objDB.ExecuteStoredProcedure("SP_VehicleInRemark_Save", dtRemark.Rows[iRowCnt]["ID"], dtRemark.Rows[iRowCnt]["VehicleInID"], dtRemark.Rows[iRowCnt]["ModelID"], dtRemark.Rows[iRowCnt]["ParameterID"], dtRemark.Rows[iRowCnt]["Remark"], dtRemark.Rows[iRowCnt]["ChkStatus"]);
                    //Sujata 18102013_End

                }

                if (bSaveVehicleInCheckList(objDB, dtChkList) == false)
                {
                    bSaveRecord = false;
                    objDB.RollbackTransaction();
                    return bSaveRecord;
                }
                else
                {
                    objDB.CommitTransaction();
                    bSaveRecord = true;
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





        //public bool bSaveVehicleInRemark(DataTable dtRemark, int iHdrID) 
        //{
        //    //saveVehicleInRemarkDetails
        //    bool bSaveRecord = false;
        //    try
        //    {
        //        for (int iRowCnt = 0; iRowCnt < dtRemark.Rows.Count; iRowCnt++)
        //        {
        //            objDB.ExecuteStoredProcedure("SP_VehicleInRemark_Save",iHdrID,dtRemark.Rows[iRowCnt]["ID"], dtRemark.Rows[iRowCnt]["VehicleID"], dtRemark.Rows[iRowCnt]["ParameterID"], dtRemark.Rows[iRowCnt]["Remark"]);

        //        }
        //        bSaveRecord = true;
        //    }
        //    catch
        //    {

        //    }
        //    return bSaveRecord;
        //}
        public bool bSaveVehicleInCheckList(clsDB objDB, DataTable dtChkList)
        {
            //saveVehicleInRemarkDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtChkList.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_VehicleInCheckList_Save", dtChkList.Rows[iRowCnt]["ID"], dtChkList.Rows[iRowCnt]["VehicleInID"], dtChkList.Rows[iRowCnt]["ModelID"], dtChkList.Rows[iRowCnt]["TransitDamages"], dtChkList.Rows[iRowCnt]["Rusting"], dtChkList.Rows[iRowCnt]["FinishingIssues"], dtChkList.Rows[iRowCnt]["PaintProblem"], dtChkList.Rows[iRowCnt]["Others"]);

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        public DataSet GetVehicleIn(int iVehicleInID, string sVehicleInype, int iDealerID)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetVehicleInDetails", iDealerID, sVehicleInype, iVehicleInID);
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

        public DataTable GetVehicleINRemarkCheckList(int ivehicleInID, int iModelID, string TypeOfSelection)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt = new DataTable();
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetVehicleInRemarkAndCheckList", ivehicleInID, iModelID, TypeOfSelection);
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

    }
}
