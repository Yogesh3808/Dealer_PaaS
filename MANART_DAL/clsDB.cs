using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Diagnostics;

namespace MANART_DAL
{
    public class clsDB
    {
        private enum CommandCallFor
        {
            enmToGetId,
            enmToGetDataSet,
            enmToGetObject
        }
        private SqlConnection l_connection = null;
        private SqlTransaction l_Transaction = null;
        private bool l_bBeginTranasaction = false;
        public clsDB()
        {
            l_connection = null;
            l_Transaction = null;
            l_bBeginTranasaction = false;
        }

        // Change Made by VRK
        private string l_sConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

        /// <summary>
        /// function is used to read connection string from web.config and Create and open Sql connection.
        /// </summary>
        private void CreateAndOpenSqlconnection()
        {
            string sDatabaseName = "";
            try
            {
                if (l_connection == null)
                    l_connection = new SqlConnection(l_sConnectionString);
                sDatabaseName = l_connection.Database;
                //l_connection.Database = l_connection.Database + "201011";                                
                if (l_connection.State == ConnectionState.Closed)
                {
                    l_connection.Open();
                    if (l_bBeginTranasaction == true)
                    {
                        l_Transaction = l_connection.BeginTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error In opening Database Connection, Contact To Your Administrator." + ex.Message);
            }
        }

        /// <summary>
        /// function is used to close Sql connection.
        /// </summary>
        private void CloseSqlconnection()
        {
            try
            {
                if (l_connection != null)
                    l_connection.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// function is used to execute sql query and return data in integer value.
        /// </summary>
        /// <param name="sSqlQuery"></param>
        /// <returns></returns>
        public int ExecuteQuery(string sSqlQuery)
        {
            int iID = 0;
            try
            {
                iID = Convert.ToInt32(ExecuteCommand(CommandCallFor.enmToGetId, CommandType.Text, sSqlQuery, null));
                return iID;
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sSqlQuery + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }

        /// <summary>
        /// function is used to execute sql query and return data in datatable
        /// </summary>
        /// <param name="sSqlQuery"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryAndGetDataTable(string sSqlQuery)
        {
            try
            {
                DataSet ds = (DataSet)ExecuteCommand(CommandCallFor.enmToGetDataSet, CommandType.Text, sSqlQuery, null);
                if (ds == null)
                    return null;
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sSqlQuery + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }
        /// <summary>
        /// function is used to execute sql query and return data in dataset. 
        /// </summary>
        /// <param name="sSqlQuery"></param>
        /// <returns></returns>
        public DataSet ExecuteQueryAndGetDataset(string sSqlQuery)
        {
            try
            {
                return (DataSet)ExecuteCommand(CommandCallFor.enmToGetDataSet, CommandType.Text, sSqlQuery, null);
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sSqlQuery + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }

        ///<summary>     
        ///ExecuteQueryAndGetDataset is used to execute sql query and return data in object type.
        ///<para>If data is required to use in specific data type format then use this function and convert the value as per the required format.    
        ///</para>
        ///</summary>  
        public object ExecuteQueryAndGetObject(string sSqlQuery)
        {
            try
            {
                return ExecuteCommand(CommandCallFor.enmToGetObject, CommandType.Text, sSqlQuery, null);
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sSqlQuery + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }

        /// <summary>
        /// function is used to execute sql stored procedure and return double
        /// </summary>
        /// <returns></returns>
        /// 
        public double ExecuteStoredProcedure_double(string sStoredProcedureName, params object[] ParametersValues)
        {
            double iID = 0;
            try
            {
                iID = Convert.ToDouble(ExecuteCommand(CommandCallFor.enmToGetId, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues));
                //ExecuteCommand(CommandCallFor.enmToGetId, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues);
                return iID;
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sStoredProcedureName + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }

        }

        /// <summary>
        /// function is used to execute sql stored procedure
        /// </summary>
        /// <returns></returns>
        /// 
        public int ExecuteStoredProcedure(string sStoredProcedureName, params object[] ParametersValues)
        {
            int iID = 0;
            try
            {
                iID = Convert.ToInt32(ExecuteCommand(CommandCallFor.enmToGetId, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues));
                //ExecuteCommand(CommandCallFor.enmToGetId, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues);
                return iID;
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sStoredProcedureName + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }

        }

        /// <summary>
        /// function is used to execute stored procedure and get dataset
        /// </summary>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="ParametersValues"></param>
        /// <returns></returns>
        public DataSet ExecuteStoredProcedureAndGetDataset(string sStoredProcedureName, params object[] ParametersValues)
        {
            try
            {
                DataSet ds = (DataSet)ExecuteCommand(CommandCallFor.enmToGetDataSet, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues);
                if (ds == null)
                    return null;
                else
                {
                    if (Func.Common.iTableCntOfDatatSet(ds) == 0)
                        return null;
                    return ds;
                }
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sStoredProcedureName + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }
        /// <summary>
        /// function is used to execute stored procedure and get datatable
        /// </summary>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="ParametersValues"></param>
        /// <returns></returns>
        public DataTable ExecuteStoredProcedureAndGetDataTable(string sStoredProcedureName, params object[] ParametersValues)
        {
            try
            {
                DataSet ds = (DataSet)ExecuteCommand(CommandCallFor.enmToGetDataSet, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues);
                if (ds == null)
                    return null;
                else
                {
                    if (Func.Common.iTableCntOfDatatSet(ds) == 0)
                        return null;
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sStoredProcedureName + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }
        /// <summary>
        /// function is used to execute stored procedure and get object
        /// </summary>
        /// <param name="sStoredProcedureName"></param>
        /// <param name="ParametersValues"></param>
        /// <returns></returns>

        public object ExecuteStoredProcedureAndGetObject(string sStoredProcedureName, params object[] ParametersValues)
        {
            try
            {
                return ExecuteCommand(CommandCallFor.enmToGetObject, CommandType.StoredProcedure, sStoredProcedureName, ParametersValues);
            }
            catch (Exception ex)
            {

                if (!ex.Source.Contains("Request Procedure"))
                    ex.Source = ex.Source + " Request Procedure : " + sStoredProcedureName + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                throw new ArgumentException("Error : " + ex.Message);
            }
        }
        /// <summary>
        /// function is used to start the Sqltransacton.
        /// </summary>
        public void BeginTranasaction()
        {
            if (l_bBeginTranasaction == false)
                l_bBeginTranasaction = true;
        }

        /// <summary>
        /// function is used to commit the Sqltransacton.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (l_Transaction != null)
                {
                    l_Transaction.Commit();
                }
                CloseSqlconnection();
                l_bBeginTranasaction = false;
            }
            catch
            {
                l_bBeginTranasaction = false;
                throw new ArgumentException("Error in Committ the transaction.");
            }
        }

        /// <summary>
        /// function is used to rollback the Sqltransacton.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (l_Transaction != null)
                {
                    l_Transaction.Rollback();
                }
                l_bBeginTranasaction = false;
            }
            catch
            {
                l_bBeginTranasaction = false;
                throw new ArgumentException("Error in Rollback the transaction");
            }
        }

        /// <summary>
        /// execute sqlCommand
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sCommandName"></param>
        /// <param name="ParametersValues"></param>
        /// <returns></returns>
        private object ExecuteCommand(CommandCallFor ValCommandCallFor, CommandType commandType, string sCommandName, object[] ParametersValues)
        {
            object ObjReturn = null;
            SqlCommand command = new SqlCommand();
            try
            {
                if (ValCommandCallFor != CommandCallFor.enmToGetDataSet)
                {
                    CreateAndOpenSqlconnection();
                    command.Transaction = l_Transaction;
                }
                else
                {
                    if (l_connection == null)
                        l_connection = new SqlConnection(l_sConnectionString);
                }
                command.Connection = l_connection;
                command.CommandType = commandType;
                command.CommandText = sCommandName;
                //command.CommandTimeout = 100;
                command.CommandTimeout = 0;
                if (commandType == CommandType.StoredProcedure)
                {
                    if (ParametersValues != null)
                    {
                        if (bSetCommandParameterValues(command, ParametersValues) == false)
                        {
                            throw new ArgumentException("Parameter count does not match Parameter Value count.");
                        }
                    }
                }
                if (ValCommandCallFor == CommandCallFor.enmToGetId || ValCommandCallFor == CommandCallFor.enmToGetObject)
                {
                    ObjReturn = command.ExecuteScalar();
                    if (command.Parameters.Contains("@return") == true)
                    {
                        ObjReturn = command.Parameters["@return"].Value;
                    }
                }
                else if (ValCommandCallFor == CommandCallFor.enmToGetDataSet)
                {
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    ObjReturn = new DataSet();
                    try
                    {
                        da.Fill((DataSet)ObjReturn);
                    }
                    catch (Exception ex)
                    {
                        da.Fill((DataSet)ObjReturn);
                    }
                    da.Dispose();
                    da = null;
                    CloseSqlconnection();
                }

                command.Parameters.Clear();
                if (l_bBeginTranasaction == false)
                {
                    CloseSqlconnection();
                }
                command.Dispose();
            }
            catch (Exception ex)
            {

                ex.Source = ex.Source + " Request Procedure : " + sCommandName + " Request User : " + ((HttpContext.Current.Session["LoginName"] != null) ? HttpContext.Current.Session["LoginName"].ToString() : "");
                l_bBeginTranasaction = false;
                CloseSqlconnection();
                command.Dispose();
                Func.Common.ProcessUnhandledException(ex);
                // throw new ArgumentException(ex.Message);

            }
            return ObjReturn;
        }

        /// <summary>
        /// function is used to get parameters of stored procedure And set Values.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ParametersValues"></param>
        /// <returns></returns>
        private bool bSetCommandParameterValues(SqlCommand command, object[] ParametersValues)
        {
            //step 1 Get parameter Set of StoredProcedure
            SqlParameter[] commandParameters = GetStoredProcedureParameters(command.CommandText);
            //step 2 Assign Parameters Values
            if (bAssignParameterValuesToCommand(commandParameters, ParametersValues) == false)
            {
                return false;
            }
            //step 3  AttachParametersToCommand
            AttachParametersToCommand(command, commandParameters);
            return true;
        }

        /// <summary>
        /// function is used to get parameters of stored procedure
        /// </summary>
        /// <param name="sStoredProcedureName"></param>
        /// <returns></returns>
        private SqlParameter[] GetStoredProcedureParameters(string sStoredProcedureName)
        {
            return SqlHelperParameterCache.GetSpParameterSet(l_sConnectionString, sStoredProcedureName);
        }

        /// <summary>
        /// function is used to set values to parameter of SqlCommand.
        /// </summary>
        /// <param name="commandParameters"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        private bool bAssignParameterValuesToCommand(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return false;
            }

            if (commandParameters.Length != parameterValues.Length)
            {
                return false;
            }
            try
            {
                for (int i = 0, j = commandParameters.Length; i < j; i++)
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// function is used to attach parameters to SqlCommand.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        private void AttachParametersToCommand(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {

                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        /// <summary>
        /// this is inbuild MSDN class used to get parameters of stored procedure
        /// </summary>
        private sealed class SqlHelperParameterCache
        {
            private SqlHelperParameterCache() { }
            private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

            private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(spName, cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlCommandBuilder.DeriveParameters(cmd);

                    if (!includeReturnValueParameter)
                    {
                        cmd.Parameters.RemoveAt(0);
                    }

                    SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
                    cmd.Parameters.CopyTo(discoveredParameters, 0);

                    return discoveredParameters;
                }
            }

            private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
            {

                SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
                for (int i = 0, j = originalParameters.Length; i < j; i++)
                {
                    clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
                }

                return clonedParameters;
            }

            private static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
            {
                string hashKey = connectionString + ":" + commandText;
                paramCache[hashKey] = commandParameters;
            }

            private static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
            {
                string hashKey = connectionString + ":" + commandText;
                SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];
                if (cachedParameters == null)
                {
                    return null;
                }
                else
                {
                    return CloneParameters(cachedParameters);
                }
            }

            public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
            {
                return GetSpParameterSet(connectionString, spName, false);
            }
            private static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
            {
                string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
                SqlParameter[] cachedParameters;
                cachedParameters = (SqlParameter[])paramCache[hashKey];
                if (cachedParameters == null)
                {
                    cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
                }
                return CloneParameters(cachedParameters);
            }
        }

    }
}
