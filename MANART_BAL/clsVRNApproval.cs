using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsVRNApproval
    /// </summary>
    public class clsVRNApproval
    {
        public clsVRNApproval()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetVRNApproval(string sSelectType, int iID, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetVRNApproval", sSelectType, iID, iDealerID);
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
        public bool bSaveVRNApproval(ref int iPDIDealer, DataTable dtVRNApproval)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                //int iActivityHdr_ID = 0;
                if (dtVRNApproval.Rows.Count != 0)
                {
                    iPDIDealer = Func.Convert.iConvertToInt(dtVRNApproval.Rows[0]["ID"]);
                    if (iPDIDealer == 0)
                        iPDIDealer = objDB.ExecuteStoredProcedure("SP_SaveVRNApproval", dtVRNApproval.Rows[0]["ID"], dtVRNApproval.Rows[0]["UserID"], dtVRNApproval.Rows[0]["IsApprove"]);
                    else
                        objDB.ExecuteStoredProcedure("SP_SaveVRNApproval", dtVRNApproval.Rows[0]["ID"], dtVRNApproval.Rows[0]["UserID"], dtVRNApproval.Rows[0]["IsApprove"]);
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

        //public DataSet GetDealerByDealerID(int iDealerID)
        //{
        //    DataSet dsDetails = new DataSet();
        //    dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerByDealerID", iDealerID);
        //    if (dsDetails != null)
        //    {
        //        return dsDetails;
        //    }
        //    else
        //        return null;

        //}
    }
}
