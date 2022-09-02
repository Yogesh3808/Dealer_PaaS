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
    /// Summary description for clsDealer
    /// </summary>
    public class clsDealer
    {
        public clsDealer()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetDealer(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_ListAll_Dealer", DealerID);
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
        public DataSet GetMaxDealer(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMaxDealer", 0);
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
        public DataSet GetAllDealer()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteQueryAndGetDataset("Select ID,Dealer_Name as Name from M_Dealer where Dealer_Origin='E' order by ID");
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
        //Megha06/06/2011
        public DataSet GetAllDomesticDealer()
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteQueryAndGetDataset("Select ID,Dealer_Name as Name from M_Dealer where Dealer_Origin='D' order by ID");
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
        //Megha06/06/2011
        //Sujata 24082011 add distributor
        //megha 16042013 add Bus code Field 
        //Sujata 03102013 add Report Region ID

        public int bSaveDealerDetails(int iID, string DealerName, string DealerShortName, string DealerCity, string DealerAddress1, string DealerMobile, string DealerLandLinePhone,
                                      string DealerEmail, string DealerMDEmail, string DealerOrigin, string DealersalesOffice, string DealerTerritory, string DealerSparesCode,
                                       string DealerVehicleCode, string DealerHDCode, string DealerExtendedwarr, string DealerHOBranch, string DealerHOCode, string DealerActive,
                                       string TINNo, string VATNo, string LVANo, string IRCNo, string DealerCountry, string DealerState, string DealerDistrict, string DealerType,
                                       string Dealercategory, string DealerRegion, string DealerDepot, string Hierarchy_code, int DistributorID, string BusCode, string Servicetaxtype, int Report_Region_ID, string PANNo, string RemanCode)
        {



            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                iID = objDB.ExecuteStoredProcedure("SP_Dealer_Save", iID, DealerName, DealerShortName, DealerCity, DealerAddress1, DealerMobile, DealerLandLinePhone,
                                                     DealerEmail, DealerMDEmail, DealerOrigin, DealersalesOffice, DealerTerritory, DealerSparesCode, DealerVehicleCode,
                                                     DealerHDCode, DealerExtendedwarr, DealerHOBranch, DealerHOCode, DealerActive, TINNo, VATNo, LVANo, IRCNo, DealerCountry, DealerState,
                                                     DealerDistrict, DealerType, Dealercategory, DealerRegion, DealerDepot, Hierarchy_code, DistributorID, BusCode, Servicetaxtype, Report_Region_ID, PANNo, RemanCode);

                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public DataTable GetDealerByID(int DealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dt;
                dt = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDealerByID", DealerID);
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

        public int bDealerLive(int iID, int iDistrictID, int iWarehouseID)
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
              {
                objDB.BeginTranasaction();

                iID = objDB.ExecuteStoredProcedure("SP_DealerGoLive", iID, iDistrictID, iWarehouseID);

                objDB.CommitTransaction();
                return iID;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                return iID;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

    }
}
