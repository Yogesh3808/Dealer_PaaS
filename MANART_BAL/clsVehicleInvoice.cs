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
    /// Summary description for clsVehicleInvoice
    /// </summary>
    public class clsVehicleInvoice
    {
        public clsVehicleInvoice()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public DataSet GetVehicleInvoice(string iDealerID, string VInvStatus, string FromDate, string ToDate)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = new DataSet();
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetVehicleInvoiceDespatch", iDealerID, VInvStatus, FromDate, ToDate);
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
        public bool bSaveAndConfirmVehicleInvoice(DataTable dtVInvSaveConf)
        {
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int cnt = 0; cnt < dtVInvSaveConf.Rows.Count; cnt++)
                {
                    //objDB.ExecuteStoredProcedure("SP_Save_VehicleInvoiceDespatch", Func.Convert.iConvertToInt(dtVInvSaveConf.Rows[cnt]["ID"]), Func.Convert.iConvertToInt(dtVInvSaveConf.Rows[cnt]["Dealer_ID"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Invoice_No"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Invoice_Date"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["SAP_order_no"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["SAP_Order_Date"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Despatch_Date"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["DriverName"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["LicenceNo"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["ISConfirm"]));
                    objDB.ExecuteStoredProcedureAndGetObject("SP_Save_VehicleInvoiceDespatch", Func.Convert.iConvertToInt(dtVInvSaveConf.Rows[cnt]["ID"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Dlr_Code"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Invoice_No"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Invoice_Date"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["SAP_order_no"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["SAP_Order_Date"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["Despatch_Date"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["DriverName"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["LicenceNo"]), Func.Convert.sConvertToString(dtVInvSaveConf.Rows[cnt]["ISConfirmedFlag"]));
                }
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
    }
}
