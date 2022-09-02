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
    public class clsLading
    {
        public clsLading()
        {

        }
        public bool bSaveDetails(DataTable dtDet)
        {
            //saveDetails       
            int PreShipmentInv_ID = 0;
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    PreShipmentInv_ID = Func.Convert.iConvertToInt(dtDet.Rows[iRowCnt]["ID"]);
                    if (PreShipmentInv_ID == 0)
                    {
                        PreShipmentInv_ID = objDB.ExecuteStoredProcedure("SP_BillofLading_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["PreShipmentInv_ID"], dtDet.Rows[0]["BL_No"], dtDet.Rows[0]["BL_date"], dtDet.Rows[0]["NameOf_Shipment"], dtDet.Rows[0]["Dealer_Id"], dtDet.Rows[0]["Model_Part"]);
                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_BillofLading_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["PreShipmentInv_ID"], dtDet.Rows[0]["BL_No"], dtDet.Rows[0]["BL_date"], dtDet.Rows[0]["NameOf_Shipment"], dtDet.Rows[0]["Dealer_Id"], dtDet.Rows[0]["Model_Part"]);
                    }
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
        public void ConfirmBL(int iBLId)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.ExecuteStoredProcedureAndGetDataset("SP_BillofLading_Confirm", iBLId);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }
        //Get BL Details For Grid
        public DataSet GetBLDetails(string sBLType, int iPreshipmentId, string sModel_Part, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsBLDetails;
                dsBLDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetBLDetails", sBLType, iPreshipmentId, sModel_Part, iDealerID);
                return dsBLDetails;
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
        //public DataSet GetBLDetails(int iPreshipmentId)
        //{
        //    DataSet dsBLDetails;
        //    dsBLDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetBLDetails", iPreshipmentId);
        //    return dsBLDetails;

        //}


    }
}
