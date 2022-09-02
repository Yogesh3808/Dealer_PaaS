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
    public class clsSaveMaster
    {
        public enum MasterType
        {
            enmReadOnlyMaster = 1,
            enmEditableMaster = 2,
            enmDependableTable = 3,
            enmMultipleMaster = 4,
            enmOneToManyMaster = 5
        }
        public clsSaveMaster()
        {

        }

        public bool bSaveRecord(string[] sSql)
        {


            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                //Using Stored Procedure                

                int _Id = 0;

                objDB.BeginTranasaction();
                for (int i = 0; i < sSql.GetLength(0); i++)
                {
                    _Id = objDB.ExecuteStoredProcedure("SP_Save_Master", sSql[i]);

                    if (_Id == 0)
                    {
                        bSaveRecord = true;
                    }
                    else
                    {
                        bSaveRecord = false;

                    }
                }
                if (bSaveRecord == true)
                {
                    objDB.CommitTransaction();
                }
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                Func.Common.ProcessUnhandledException(ex);
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        public bool bSaveRecordForDependable(string[] sSql, string sMainTableName, string sMainTableIDName, string sMainTableIDValue, clsGlobal.enmFormState ValenmFormState)
        {
            bool bSaveRecord = true;
            int _Id = 0;
            string sql = "";
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                _Id = objDB.ExecuteStoredProcedure("SP_Save_Master", sSql[0]);
                if (ValenmFormState == clsGlobal.enmFormState.enmNew_State)
                {
                    sql = "Select Max(ID)as ID from " + sMainTableName;
                    _Id = objDB.ExecuteQuery(sql);

                }
                else if (ValenmFormState == clsGlobal.enmFormState.enmUpdate_State)
                {
                    _Id = Func.Convert.iConvertToInt(sMainTableIDValue);
                }
                for (int i = 1; i < sSql.GetLength(0); i++)
                {
                    if (sSql[i].Contains("DELETE") == true)
                    {
                        sSql[i] = sSql[i] + " And " + sMainTableIDName + " = " + _Id.ToString();
                        objDB.ExecuteStoredProcedure("SP_Save_Master", sSql[i]);
                    }
                    else
                    {
                        sSql[i] = sAddMainTableIdToDependableMasterAndReturnSql(sSql[i], sMainTableIDName, _Id.ToString());
                        objDB.ExecuteStoredProcedure("SP_Save_Master", sSql[i]);
                    }
                }
                objDB.CommitTransaction();
                return bSaveRecord;
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
                Func.Common.ProcessUnhandledException(ex);
                return bSaveRecord;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
        private string sAddMainTableIdToDependableMasterAndReturnSql(string sOrgSql, string sIDName, string sIDValue)
        {
            string sTmpSql = sOrgSql;
            string sSqlfirstPart = "";
            string sSqlLastPart = "";
            string sReturnSql = "";
            sSqlfirstPart = sTmpSql.Substring(0, sTmpSql.IndexOf(')'));
            sSqlLastPart = sTmpSql.Substring(sTmpSql.IndexOf(')'));
            sSqlfirstPart = sSqlfirstPart + "," + sIDName;
            sSqlLastPart = sSqlLastPart.Substring(0, sSqlLastPart.LastIndexOf(')'));
            sSqlLastPart = sSqlLastPart + "," + sIDValue + ")";
            sReturnSql = sSqlfirstPart + sSqlLastPart;
            return sReturnSql;

        }
    }
}
