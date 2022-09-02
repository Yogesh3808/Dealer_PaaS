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
  public  class clsCompanyMaster
    {

      public clsCompanyMaster()
        {
            //
            // TODO: Add constructor logic here
            //
        }
      public DataSet GetCompanyInfo(int DealerID, int iHOBr_id)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds;
                ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_CompanyInfo", DealerID, iHOBr_id);
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
      public bool bSaveCompanyMaster(int DealerID, string TINNo, string VATNo,string PANNo)
      {



          // 'Replace Func.DB to objDB by Shyamal on 05042012
          clsDB objDB = new clsDB();
          try
          {
              
              objDB.BeginTranasaction();
             objDB.ExecuteStoredProcedure("SP_SAVE_CompanyInfo",  DealerID,  TINNo,  VATNo, PANNo);

              objDB.CommitTransaction();
              return true;
          }
          catch (Exception ex)
          {
              objDB.RollbackTransaction();
            
              return false;
          }
          finally
          {
              if (objDB != null) objDB = null;
          }
      }
  }
}
