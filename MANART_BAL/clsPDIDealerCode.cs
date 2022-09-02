using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsPDIDealerCode
    /// </summary>
    public class clsPDIDealerCode
    {
        public clsPDIDealerCode()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetPDIDealer(string sSearchText, string sSelectType, int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPDIDealer", sSearchText, sSelectType, iDealerID);
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
        public bool bSavePDIDealerCode(ref int iPDIDealer, DataTable dtPDIDealerCode)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //int iActivityHdr_ID = 0;
                if (dtPDIDealerCode.Rows.Count != 0)
                {

                    objDB.BeginTranasaction();
                    iPDIDealer = Func.Convert.iConvertToInt(dtPDIDealerCode.Rows[0]["ID"]);
                    if (iPDIDealer == 0)
                        iPDIDealer = objDB.ExecuteStoredProcedure("SP_Save_PDIDealer", dtPDIDealerCode.Rows[0]["ID"], dtPDIDealerCode.Rows[0]["PDIDealer_Code"], dtPDIDealerCode.Rows[0]["UserID"], dtPDIDealerCode.Rows[0]["IsProcessed"]);
                    else
                        objDB.ExecuteStoredProcedure("SP_Save_PDIDealer", dtPDIDealerCode.Rows[0]["ID"], dtPDIDealerCode.Rows[0]["PDIDealer_Code"], dtPDIDealerCode.Rows[0]["UserID"], dtPDIDealerCode.Rows[0]["IsProcessed"]);
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

        public DataSet GetDealerByDealerID(int iDealerID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerByDealerID", iDealerID);
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
    }
}
