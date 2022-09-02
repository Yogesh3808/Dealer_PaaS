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
    /// Summary description for clsVehicleReportingTime
    /// </summary>
    public class clsVehicleReportingTime
    {
        public clsVehicleReportingTime()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetVehicleReportingTime(int iDocID, string sDocType, int iDealerID, int iHOBranchDealerId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_VehicleReportingTime", iDocID, sDocType, iDealerID, iHOBranchDealerId);
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
        public bool bSaveVehicleReporting(ref int iHdrID, string sDealerCode, DataTable dtHdr, DataTable dtDet)
        {
            iHdrID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //Save Header
                objDB.BeginTranasaction();
                if (bSaveHeader(objDB, sDealerCode, dtHdr, ref iHdrID) == false) goto ExitFunction;

                //save Accessory Details
                if (bSaveAccessoryDetails(objDB, dtDet, iHdrID) == false) goto ExitFunction;

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

        private bool bSaveHeader(clsDB objDB, string sDealerCode, DataTable dtHdr, ref int iHdrID)
        {
            try
            {
                if (Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]) == 0)
                {                   
                    int iDelearId = Func.Convert.iConvertToInt(dtHdr.Rows[0]["Dealer_ID"]);
                    string sFinYear = Func.sGetFinancialYear(iDelearId);
                    dtHdr.Rows[0]["Inward_tr_no"] = Func.Common.sGetMaxDocNo(sDealerCode, "", "I", iDelearId);
                    //txtDocNo.Text = Func.Common.sGetMaxDocNo(Location.sDealerCode, "", "I", iDealerId);

                    iHdrID = objDB.ExecuteStoredProcedure("SP_VehicleReportingTime_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Inward_no"], dtHdr.Rows[0]["Inward_tr_no"], dtHdr.Rows[0]["time_in"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["DlrBranchID"], dtHdr.Rows[0]["veh_in_confirm"], dtHdr.Rows[0]["veh_in_cancel"], dtHdr.Rows[0]["Job_HDR_ID"], dtHdr.Rows[0]["App_no"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Customer_name"], 0, dtHdr.Rows[0]["CRM_HDR_ID"], dtHdr.Rows[0]["TrNo"]);
                    dtHdr.Rows[0]["ID"] = iHdrID;

                    Func.Common.UpdateMaxNo(objDB, sFinYear, "I", iDelearId);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_VehicleReportingTime_Save", dtHdr.Rows[0]["ID"], dtHdr.Rows[0]["Inward_no"], dtHdr.Rows[0]["Inward_tr_no"], dtHdr.Rows[0]["time_in"], dtHdr.Rows[0]["Dealer_ID"], dtHdr.Rows[0]["DlrBranchID"], dtHdr.Rows[0]["veh_in_confirm"], dtHdr.Rows[0]["veh_in_cancel"], dtHdr.Rows[0]["Job_HDR_ID"], dtHdr.Rows[0]["App_no"], dtHdr.Rows[0]["Chassis_ID"], dtHdr.Rows[0]["Customer_name"], 0, dtHdr.Rows[0]["CRM_HDR_ID"], dtHdr.Rows[0]["TrNo"]);
                    iHdrID = Func.Convert.iConvertToInt(dtHdr.Rows[0]["ID"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool bSaveAccessoryDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    if (dtDet.Rows[iRowCnt]["AccID"].ToString() != "0")
                    {
                        objDB.ExecuteStoredProcedure("SP_VehicleReportingTimeDtl_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["AccID"], dtDet.Rows[iRowCnt]["Qty"], 0);
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
