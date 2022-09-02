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
    /// Summary description for CompetitorTonnageWise
    /// </summary>
    public class clsCompetitorTonnageWise
    {
        public clsCompetitorTonnageWise()
        {
            //
            // TODO: Add constructor logic here
            //
        }



        public DataSet CompetitorTonnageWise(int iCSMId, int iYear)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDetails = new DataSet();
                dsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetCompetitorTonnage", iCSMId, iYear);

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

        public bool bSaveCompetitorTonnageWise(DataTable dtDet)
        {
            //saveVehicleInDetails
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                for (int iRowCnt = 0; iRowCnt < dtDet.Rows.Count; iRowCnt++)
                {
                    objDB.BeginTranasaction();
                    objDB.ExecuteStoredProcedure("SP_CompetitorTonnage_Save", dtDet.Rows[iRowCnt]["ID"], dtDet.Rows[iRowCnt]["CSM_Id"], dtDet.Rows[iRowCnt]["Month_Id"], dtDet.Rows[iRowCnt]["Year_Id"], dtDet.Rows[iRowCnt]["Tonnage_Id"], dtDet.Rows[iRowCnt]["Competitor_Id"], dtDet.Rows[iRowCnt]["Qty"]);

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
    }
}
