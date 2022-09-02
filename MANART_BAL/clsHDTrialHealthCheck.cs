using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MANART_DAL;
namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsHDTrialHealthCheck
    /// </summary>
    public class clsHDTrialHealthCheck
    {
        public clsHDTrialHealthCheck()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetHDTrialHealthCheck(string Type, int HDTReqID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetHDTrialHealthCheck", Type, HDTReqID);
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
        public bool bSubmitHealthcheckForSave(int ID, string TrialHealthCheckDate,
                    string DealerName, string Location, string LastService, string drpServiceRequired, string drpPerformanceChecksReq,
                   string Leakeages, string Transmission, string Suspension, string Cabin,
                  string Axle, string LoadBody, string TippingKit, string txtTyres, string LubricationSystem, string VehiclePerformance,
                   string Comments, int iHDTrialAllocationID, int iHDTrialReportID)
        {
            bool bSubmit = false;

            // 'Replace Func.DB to objDB by Shyamal on 27032012
            clsDB objDB = new clsDB();
            try
            {


                objDB.ExecuteStoredProcedure("SP_HDTrialHealthcheck_Save", ID, TrialHealthCheckDate,
                     DealerName, Location, LastService, drpServiceRequired, drpPerformanceChecksReq,
                    Leakeages, Transmission, Suspension, Cabin,
                  Axle, LoadBody, TippingKit, txtTyres, LubricationSystem, VehiclePerformance,
                  Comments, iHDTrialAllocationID, iHDTrialReportID);



                objDB.CommitTransaction();

                bSubmit = true;
                return bSubmit;
            }
            catch (Exception ex)
            {
                //if (iClaimStatus == 2)
                //{
                objDB.RollbackTransaction();
                //}
                return bSubmit;
            }

            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public DataSet GetHDTrialHealthCheck_DealerName(string RegionName)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_getDealerName_THDHealthCheck", RegionName);
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
    }
}
