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
    /// Summary description for clsLC
    /// </summary>
    public class clsLC
    {
        public clsLC()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool bSaveLCDetails(DataTable dtDet, DataTable dtLCItemDtls)
        {
            int iHdrID;
            string ParameterList = "";
            iHdrID = Func.Convert.iConvertToInt(dtDet.Rows[0]["ID"]);

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                if (dtDet.Rows.Count != 0)
                {

                    if (iHdrID == 0)
                    {
                        iHdrID = objDB.ExecuteStoredProcedure("SP_LCDetails_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Proforma_ID"], dtDet.Rows[0]["LC_No"], dtDet.Rows[0]["LC_date"], dtDet.Rows[0]["Dealer_Id"], dtDet.Rows[0]["LC_Amt"], dtDet.Rows[0]["LC_Expiry_Date"], dtDet.Rows[0]["Expiry_At"], dtDet.Rows[0]["Negotiation_days"], dtDet.Rows[0]["Opening_Bank"], dtDet.Rows[0]["Negotiating_Bank"], dtDet.Rows[0]["Drawee_Bank"], dtDet.Rows[0]["Reimbursing_Bank"], dtDet.Rows[0]["Confirmation_status"], dtDet.Rows[0]["Invoice_copies"], dtDet.Rows[0]["PackingList_copies"], dtDet.Rows[0]["WeightList_copies"], dtDet.Rows[0]["COO_copies"], dtDet.Rows[0]["Legalization"], dtDet.Rows[0]["BillOfLading_conditions"], dtDet.Rows[0]["Appended_Declaration"], dtDet.Rows[0]["MarksNo"], dtDet.Rows[0]["LC_Confirm"], dtDet.Rows[0]["ModelPart"], dtDet.Rows[0]["PartDescription"], dtDet.Rows[0]["LCType"], dtDet.Rows[0]["Amendment"], dtDet.Rows[0]["LC_ADVPayment"], dtDet.Rows[0]["AmendmentDate"], dtDet.Rows[0]["Notify"], dtDet.Rows[0]["DescOfGoods"], dtDet.Rows[0]["Preshipment_ID"]);

                    }
                    else
                    {
                        objDB.ExecuteStoredProcedure("SP_LCDetails_Save", dtDet.Rows[0]["ID"], dtDet.Rows[0]["Proforma_ID"], dtDet.Rows[0]["LC_No"], dtDet.Rows[0]["LC_date"], dtDet.Rows[0]["Dealer_Id"], dtDet.Rows[0]["LC_Amt"], dtDet.Rows[0]["LC_Expiry_Date"], dtDet.Rows[0]["Expiry_At"], dtDet.Rows[0]["Negotiation_days"], dtDet.Rows[0]["Opening_Bank"], dtDet.Rows[0]["Negotiating_Bank"], dtDet.Rows[0]["Drawee_Bank"], dtDet.Rows[0]["Reimbursing_Bank"], dtDet.Rows[0]["Confirmation_status"], dtDet.Rows[0]["Invoice_copies"], dtDet.Rows[0]["PackingList_copies"], dtDet.Rows[0]["WeightList_copies"], dtDet.Rows[0]["COO_copies"], dtDet.Rows[0]["Legalization"], dtDet.Rows[0]["BillOfLading_conditions"], dtDet.Rows[0]["Appended_Declaration"], dtDet.Rows[0]["MarksNo"], dtDet.Rows[0]["LC_Confirm"], dtDet.Rows[0]["ModelPart"], dtDet.Rows[0]["PartDescription"], dtDet.Rows[0]["LCType"], dtDet.Rows[0]["Amendment"], dtDet.Rows[0]["LC_ADVPayment"], dtDet.Rows[0]["AmendmentDate"], dtDet.Rows[0]["Notify"], dtDet.Rows[0]["DescOfGoods"], dtDet.Rows[0]["Preshipment_ID"]);
                    }
                }

                if (bSaveLCItemDetails(objDB, dtLCItemDtls, iHdrID) == false)
                {

                    objDB.RollbackTransaction();
                    bSaveRecord = false;

                }
                else
                {
                    objDB.CommitTransaction();

                }
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
        private bool bSaveLCItemDetails(clsDB objDB, DataTable dtDet, int iHdrID)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            try
            {
                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_LCItemDet_Save", dtDet.Rows[iRowCnt]["ID"], iHdrID, dtDet.Rows[iRowCnt]["ProformaDtlID"], dtDet.Rows[iRowCnt]["HS_Code"], dtDet.Rows[iRowCnt]["Model_Description"]);

                }
                bSaveRecord = true;
            }
            catch
            {

            }
            return bSaveRecord;
        }

        //Get LC Details For Grid
        public DataSet GetLCDetails(int ForData, int iDealerID, string ID, string sModelPart)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsLCDetails;
                dsLCDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetLCDetails", ForData, iDealerID, ID, sModelPart);
                return dsLCDetails;
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
