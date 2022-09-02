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
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsUser
    /// </summary>
    public class clsUser
    {

        #region Variables

        DataSet DSUserDetails = null;
        DataSet DSDealerDeatils = null;

        #endregion




        public clsUser()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// Form Method For Authentication
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public int Authenticate(string sLoginName, string Password, int ret)
        {
            string sEncriptPassword = "";


            //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);
            //sEncriptPassword = objCrypto.Encrypting(Password, "ETS");

            //Modified By Shyamal 21/01/2011
            clsDB objDB = new clsDB();
            try
            {
                sEncriptPassword = clsCrypto.Encrypt(Password);
                ret = objDB.ExecuteStoredProcedure("SP_AuthenticateUser", sLoginName, sEncriptPassword, ret);
            }
            catch (Exception ex)
            {
                return ret;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            //if (ret == 1)
            //{
            //    DSUserDetails = GetUserDetails("Authentication", sLoginName);//objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_UserDetails", "Authentication", UserID);
            //    if (DSUserDetails.Tables[0] != null && DSUserDetails.Tables[0].Rows.Count!=0)
            //    {

            //        iUserID = BLL.Func.Convert.iConvertToInt(DSUserDetails.Tables[0].Rows[0]["UserId"]);
            //        iUserType = BLL.Func.Convert.iConvertToInt(DSUserDetails.Tables[0].Rows[0]["UserType"]);
            //        _iUserClaimSlab = BLL.Func.Convert.iConvertToInt(DSUserDetails.Tables[0].Rows[0]["SlabID"]);               
            //    }

            //}


            //objCrypto = null;


            return ret;
        }

        public int PasswordValidation(string Password, int UserID, int ret)
        {
            string sEncriptPassword = "";

            //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);
            //sEncriptPassword = objCrypto.Encrypting(Password, "ETS");

            sEncriptPassword = clsCrypto.Encrypt(Password);
            clsDB objDB = new clsDB();
            try
            {
                ret = objDB.ExecuteStoredProcedure("SP_PasswordValidation", sEncriptPassword, UserID, ret);
            }
            catch (Exception ex)
            {
                return ret; ;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            //if (ret == 1)
            //{
            //    DSUserDetails = GetUserDetails("Authentication", sLoginName);//objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_UserDetails", "Authentication", UserID);
            //    if (DSUserDetails.Tables[0] != null && DSUserDetails.Tables[0].Rows.Count!=0)
            //    {

            //        iUserID = BLL.Func.Convert.iConvertToInt(DSUserDetails.Tables[0].Rows[0]["UserId"]);
            //        iUserType = BLL.Func.Convert.iConvertToInt(DSUserDetails.Tables[0].Rows[0]["UserType"]);
            //        _iUserClaimSlab = BLL.Func.Convert.iConvertToInt(DSUserDetails.Tables[0].Rows[0]["SlabID"]);               
            //    }

            //}


            //objCrypto = null;
            return ret;
        }

        /// <summary>
        ///   Method For Get dealer Details. 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="iDealerID"></param>
        /// <param name="iUserType"></param>
        /// <returns></returns>
        public DataSet DealerDetails(string UserID, int iDealerID, int iUserType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DSDealerDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_DealerDetails", UserID, iDealerID, iUserType);
                return DSDealerDeatils;
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

        public DataSet FillDealerForState(int UserID, int RegionID, int StateID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DSDealerDeatils = objDB.ExecuteStoredProcedureAndGetDataset("sp_FillDealerForState", UserID, "", RegionID, StateID);
                return DSDealerDeatils;
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
        public DataSet FillDealerForCountry(int UserID, int RegionID, int CountryID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DSDealerDeatils = objDB.ExecuteStoredProcedureAndGetDataset("sp_FillDealerForCountry", UserID, "", RegionID, CountryID);
                return DSDealerDeatils;
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

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="SelectionType"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetUserDetails(string SelectionType, string UserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DSDealerDeatils = new DataSet();
                DSDealerDeatils = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_UserDetails", SelectionType, UserID);
                return DSDealerDeatils;
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
        /// <summary>
        /// Register New  User 
        /// </summary>
        /// <param name="iID"></param>
        /// <param name="iUserID"></param>
        /// <param name="sLoginName"></param>
        /// <param name="sPassword"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastName"></param>
        /// <param name="sEmail"></param>
        /// <returns></returns>
        public bool AddUser(DataTable dtHdr)
        {
            int UserId;
            string sEncriptPassword = "";
            //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                //sEncriptPassword = objCrypto.Encrypting(sPassword, "ETS");
                objDB.BeginTranasaction();
                for (int iCnt = 0; iCnt < dtHdr.Rows.Count; iCnt++)
                {
                    string sUserType = Func.Convert.sConvertToString(dtHdr.Rows[iCnt]["UserType"]);
                    string sDocName = Func.Convert.sConvertToString(dtHdr.Rows[iCnt]["DocNo"]);
                    string sDocKey = Func.Convert.sConvertToString(dtHdr.Rows[iCnt]["HOBR_HdrID"]) + "/" + Func.Convert.sConvertToString(dtHdr.Rows[iCnt]["UserDepartment"]);
                    sEncriptPassword = clsCrypto.Encrypt(Func.Convert.sConvertToString(dtHdr.Rows[iCnt]["Password"]));
                    objDB.ExecuteStoredProcedure("SP_User_Save", 0, dtHdr.Rows[iCnt]["UserID"], dtHdr.Rows[iCnt]["LoginName"], dtHdr.Rows[iCnt]["FirstName"], dtHdr.Rows[iCnt]["LastName"], sEncriptPassword, dtHdr.Rows[iCnt]["Email"], dtHdr.Rows[iCnt]["EmpCode"], dtHdr.Rows[iCnt]["UserType"], dtHdr.Rows[iCnt]["UserRole"], dtHdr.Rows[iCnt]["UserDepartment"], dtHdr.Rows[iCnt]["UserBasicModelCatIDs"], dtHdr.Rows[iCnt]["HOBR_HdrID"]);
                    if (sUserType == "3" || sUserType == "4" || sUserType == "6")
                        UpdateMaxNo(objDB, sDocName.ToUpper(), sDocKey);
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
        public bool ChangePassword(int iUserId, string sNewPassword)
        {

            string sEncriptPassword = "";

            //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                //sEncriptPassword = objCrypto.Encrypting(sNewPassword, "ETS");

                sEncriptPassword = clsCrypto.Encrypt(sNewPassword);
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_ChangePassword", iUserId, sEncriptPassword);
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

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="sLoginName"></param>
        /// <param name="ValidationType"></param>
        /// <returns></returns>
        public int UserValiadation(int UserID, string sLoginName, string ValidationType)
        {

            int ret;
            clsDB objDB = new clsDB();
            try
            {
                ret = objDB.ExecuteStoredProcedure("SP_UserValidation", UserID, sLoginName, ValidationType, 0);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

            return ret;

        }
        public bool CheckUserExist(string sLoginName, ref string sEmailID)
        {
            //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);

            DataTable dtUser = new DataTable();
            clsCommon objclsComman = new clsCommon();
            bool bIsSend = false;
            clsDB objDB = new clsDB();
            try
            {
                dtUser = objDB.ExecuteStoredProcedureAndGetDataTable("SP_CheckUserExist", sLoginName);

                if (dtUser != null)
                {
                    if (dtUser.Rows.Count > 0)
                    {
                        sEmailID = Func.Convert.sConvertToString(dtUser.Rows[0]["Email"]);
                        bIsSend = true;
                    }

                }
            }
            catch (Exception ex)
            {
                bIsSend = false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

            return bIsSend;
        }

        public bool EmailForgotPasswordSend(string sLoginName)
        {
            //clsCrypto objCrypto = new clsCrypto(clsCrypto.SymmProvEnum.Rijndael);

            DataTable dtUser = new DataTable();
            clsCommon objclsComman = new clsCommon();
            bool bIsSend = false;
            int iUserId;
            string sPassword = "";
            string sUserName = "";
            string AdditionalMessage = "";
            int ret = 0;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                dtUser = objDB.ExecuteStoredProcedureAndGetDataTable("SP_ForgotPasswordMail", sLoginName);

                if (dtUser != null)
                {
                    if (dtUser.Rows.Count > 0)
                    {
                        iUserId = Func.Convert.iConvertToInt(dtUser.Rows[0]["userID"]);
                        sUserName = Func.Convert.sConvertToString(dtUser.Rows[0]["UserName"]);
                        //sPassword = objCrypto.Decrypting(Func.Convert.sConvertToString(dtUser.Rows[0]["Password"]),"ETS");
                        sPassword = clsCrypto.Decrypt(Func.Convert.sConvertToString(dtUser.Rows[0]["Password"]));
                        AdditionalMessage = "User Name: " + sUserName + " Password: " + sPassword;
                        objclsComman.bConfirmAndSendMail(iUserId, "Y", "Password", AdditionalMessage);
                        bIsSend = true;
                    }
                }
                return bIsSend;
            }
            catch (Exception ex)
            {
                return bIsSend;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        /// <summary>
        /// Get User Status: Active, Lock and Block
        /// </summary>
        /// <param name="StatusType"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool GetStatusForUser(string StatusType, int UserId)
        {
            clsDB objDB = new clsDB();
            int iStatus;
            Boolean IsStatusType = false;
            try
            {

                iStatus = objDB.ExecuteStoredProcedure("SP_GetStatusForUser", UserId, StatusType);

                if (iStatus == 0)
                {
                    IsStatusType = false;
                }
                else if (iStatus == 1)
                {
                    IsStatusType = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return IsStatusType;

        }

        /// <summary>
        /// Update Status: Active, Lock and Block
        /// </summary>
        /// <param name="StatusType"></param>
        /// <param name="UserId"></param>
        /// <param name="StatusValue"></param>
        public void UpdateStatusForUser(string StatusType, int UserId, bool StatusValue)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_UpdateStatusForUser", UserId, StatusType, StatusValue);
                objDB.CommitTransaction();
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        /// <summary>
        /// Update User Role
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserType"></param>
        /// <param name="UserRole"></param>
        public void UpdateUserRoleType(int UserID, int UserType, int UserRole, int UserDept)
        {

            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                objDB.ExecuteStoredProcedure("SP_UserUpdate", UserID, UserType, UserRole, UserDept);
                objDB.CommitTransaction();
            }
            catch (Exception ex)
            {
                objDB.RollbackTransaction();

            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }


        /// <summary>
        /// GEt User wise Permission Details
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SelectionType"></param>
        /// <returns></returns>
        //public DataTable GetPermissionDetails(int UserID, string SelectionType)
        //{
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataTable DtDetails = new DataTable();
        //        DtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetPermissionDetails", UserID, SelectionType);
        //        if (DtDetails != null)
        //        {
        //            return DtDetails;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }

        //}
        public DataSet GetPermissionDetails(int UserID, string SelectionType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DsDetails = new DataSet();
                DsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPermissionDetails", UserID, SelectionType);
                if (DsDetails != null)
                {
                    return DsDetails;
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
        public DataTable GetUserByID(int UserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable DtDetails = new DataTable();
                DtDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_UserByID", UserID);
                if (DtDetails != null)
                {
                    return DtDetails;
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

        /// <summary>
        /// Save User Permission Details
        /// </summary>
        /// <param name="DTPermissions"></param>
        /// <param name="DTDeparment"></param>
        /// <param name="DTUserMainMenu"></param>
        /// <param name="DTUserSubMenu"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool SavePermissions(DataTable DTPermissions, DataTable DTUserMainMenu, DataTable dtUserMenuRights, DataTable dtUserRoleUserOrDealer, int UserID, int UserRole, string sFirstName, string sLastName, string sEmail, string sEmpCode, CheckBoxList lstModelCategory, int SaveUserID)//DataTable DTDeparment,
        {

            string SQlUpdate = "";
            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                objDB.BeginTranasaction();
                // Save User Wise Permission Region,Country or State 
                SQlUpdate = "Update M_Sys_UserPermissions Set IsActive=0 where UserID =" + UserID;
                objDB.ExecuteQuery(SQlUpdate);
                for (int iRowCnt = 0; iRowCnt < DTPermissions.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_SavePermission", UserID, DTPermissions.Rows[iRowCnt]["RegionID"], DTPermissions.Rows[iRowCnt]["CountryID"], DTPermissions.Rows[iRowCnt]["StateID"]);
                }

                // Commented by Shyamal as on 23012013 already covered in create user page,Multi department savings removed.
                ////// Save User Wise Department
                ////SQlUpdate = "Delete  from M_Sys_UserDepartment where UserID =" + UserID;
                ////objDB.ExecuteQuery(SQlUpdate);
                ////for (int iRowCnt = 0; iRowCnt < DTDeparment.Rows.Count; iRowCnt++)
                ////{
                ////    objDB.ExecuteStoredProcedure("SP_SaveUserDepartment", UserID, DTDeparment.Rows[iRowCnt]["DepartmentID"]);
                ////}

                // Save User wise Main Menu 
                SQlUpdate = "Update M_Sys_UserMainMenu Set IsActive=0 where UserID =" + UserID;
                objDB.ExecuteQuery(SQlUpdate);
                for (int iRowCnt = 0; iRowCnt < DTUserMainMenu.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_SaveUserMenu", UserID, DTUserMainMenu.Rows[iRowCnt]["MenuID"]);
                }

                SQlUpdate = "Update M_Sys_UserMenuRights Set Active=0 where UserID =" + UserID;
                objDB.ExecuteQuery(SQlUpdate);
                SQlUpdate = "Update M_Sys_UserUserClaimProcessSlab Set Active=0 where UserID =" + UserID;
                objDB.ExecuteQuery(SQlUpdate);
                if (dtUserMenuRights != null)
                    for (int iRowCnt = 0; iRowCnt < dtUserMenuRights.Rows.Count; iRowCnt++)
                    {
                        objDB.ExecuteStoredProcedure("SP_SaveUserMEnuRights", UserID, dtUserMenuRights.Rows[iRowCnt]["MenuID"], dtUserMenuRights.Rows[iRowCnt]["RightsID"], dtUserMenuRights.Rows[iRowCnt]["ClmslabID"]);
                    }


                if (UserRole == 1 || UserRole == 2 || UserRole == 3 || UserRole == 9 || UserRole == 10 || UserRole == 11 || UserRole == 12 || UserRole == 13 || UserRole == 14 || UserRole == 15 || UserRole == 16 || UserRole == 17 || UserRole == 18 || UserRole == 21)
                {

                    objDB.ExecuteStoredProcedure("SP_UserRoleUserOrDealer_Delete", UserID, "UserDelete");

                    //SQlUpdate = "Delete from M_Sys_UserRoleUser where ParentUserID=" + UserID;
                    //objDB.ExecuteQuery(SQlUpdate);

                }
                if (UserRole == 4 || UserRole == 3)
                {
                    objDB.ExecuteStoredProcedure("SP_UserRoleUserOrDealer_Delete", UserID, "DealerDelete");
                    //SQlUpdate = "Delete from M_Sys_UserRoleWiseDealer where UserID=" + UserID;
                    //objDB.ExecuteQuery(SQlUpdate);
                }
                //changes by megha
                //for (int iRowCnt = 0; iRowCnt < dtUserRoleUserOrDealer.Rows.Count; iRowCnt++)
                //{
                //    objDB.ExecuteStoredProcedure("SP_UserRoleUserOrDealer_Save", dtUserRoleUserOrDealer.Rows[iRowCnt]["UserID"], dtUserRoleUserOrDealer.Rows[iRowCnt]["ParentUserID"]);
                //}

                for (int iRowCnt = 0; iRowCnt < dtUserRoleUserOrDealer.Rows.Count; iRowCnt++)
                {
                    objDB.ExecuteStoredProcedure("SP_UserRoleUserOrDealer_Save", dtUserRoleUserOrDealer.Rows[iRowCnt]["UserID"], dtUserRoleUserOrDealer.Rows[iRowCnt]["ParentUserID"], dtUserRoleUserOrDealer.Rows[iRowCnt]["HDFlag"], dtUserRoleUserOrDealer.Rows[iRowCnt]["LMDFlag"], dtUserRoleUserOrDealer.Rows[iRowCnt]["BUSFlag"], dtUserRoleUserOrDealer.Rows[iRowCnt]["DFlag"], SaveUserID);
                }
                //changes by megha
                // Save User wise Sub Menu 

                SQlUpdate = "Update M_Sys_User set FirstName='" + sFirstName + "',LastName='" + sLastName + "',Email='" + sEmail + "',Emp_No='" + sEmpCode + "' where ID=" + UserID;
                objDB.ExecuteQuery(SQlUpdate);


                for (int iRowCnt = 0; iRowCnt < lstModelCategory.Items.Count; iRowCnt++)
                {

                    objDB.ExecuteStoredProcedure("SP_Save_UserModelCategory", UserID, Func.Convert.iConvertToInt(lstModelCategory.Items[iRowCnt].Value), (lstModelCategory.Items[iRowCnt].Selected == true) ? "Y" : "N");
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

        public static string IdentifyUserType(int iUserType)
        {
            string sUserType = "";
            switch (iUserType)
            {
                case 1: sUserType = "VE"; break;
                case 2: sUserType = "VD"; break;
                case 3: sUserType = "DD"; break;
                case 4: sUserType = "DE"; break;
                case 5: sUserType = "AD"; break;
                // case 6: sUserType = "DE"; break;
                case 6: sUserType = "DD"; break;
                case 7: sUserType = "EG"; break;
                case 8: sUserType = "VD"; break;
            }
            return sUserType;

        }

        public void PopulateMainTreeMenu(string UserType, TreeView Tree)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet ds = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetMenuDataForPermission", UserType);

                string sMenuName = "";
                string sMenuValue = "";
                string sMenuURl = "";
                string sMenuToolTip = "";
                int iParentID = 0;
                TreeNode Currrmenu;
                TreeNode Childmenu;
                string sParentID = "0";
                string sMenuId = "0";
                //Create Master Menu
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iParentID = Func.Convert.iConvertToInt(dtRow["Parent"]);
                    sMenuName = Func.Convert.sConvertToString(dtRow["Title"]);
                    sMenuId = Func.Convert.sConvertToString(dtRow["ID"]);
                    sMenuValue = Func.Convert.sConvertToString(dtRow["MenuSequenceID"]);
                    sMenuURl = Func.Convert.sConvertToString(dtRow["Url"]);
                    sMenuToolTip = Func.Convert.sConvertToString(dtRow["Description"]);
                    Currrmenu = new TreeNode(sMenuName, sMenuValue);
                    Currrmenu.ToolTip = sMenuToolTip;
                    Currrmenu.Target = sMenuId;
                    Tree.Nodes.Add(Currrmenu);
                }
                //Create Child Menu

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    sParentID = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["Parent"]);
                    sMenuName = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["Title"]);
                    sMenuValue = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["MenuSequenceID"]);
                    sMenuId = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["ID"]);
                    sMenuURl = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["Url"]);
                    sMenuToolTip = Func.Convert.sConvertToString(ds.Tables[1].Rows[i]["Description"]);
                    Childmenu = new TreeNode(sMenuName, sMenuValue);
                    Childmenu.Target = sMenuId;
                    Childmenu.ToolTip = sMenuToolTip;
                    if (Tree.FindNode(sParentID) != null)
                    {
                        Tree.FindNode(sParentID).ChildNodes.Add(Childmenu);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }

        public bool WarrantyApplicableYN(string sMenuID)
        {
            bool Flage = true;
            int iMenuId = Func.Convert.iConvertToInt(sMenuID);
            clsDB objDB = new clsDB();
            try
            {
                int iWYN = objDB.ExecuteStoredProcedure("SP_WarrantyApplicableNYToMenu", iMenuId, 0);

                if (iWYN == 0)
                {
                    Flage = false;
                }
            }
            catch (Exception ex)
            {
                return Flage;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return Flage;
        }

        public int getUserClaimSlab(string sUserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                int iUserId = Func.Convert.iConvertToInt(sUserID);
                return objDB.ExecuteStoredProcedure("SP_GetUserClaimSlab", iUserId, 0);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }

        }

        //public DataTable GetUserRights(string UserID)
        //{

        //    DataTable dtUserRights = new DataTable();

        //    dtUserRights = GetPermissionDetails(Func.Convert.iConvertToInt(UserID), "UserRights");

        //    return dtUserRights;

        //}
        public DataSet GetUserRights(string UserID)
        {

            DataSet dsUserRights = new DataSet();

            dsUserRights = GetPermissionDetails(Func.Convert.iConvertToInt(UserID), "UserRights");

            return dsUserRights;

        }
        //public DataTable GetRoleUserRoleOrDealer(string sRegionID, string sStateorCountryID, int iUserID, string sSelDept)
        //{
        //    clsDB objDB = new clsDB();
        //    try
        //    {
        //        DataTable dtRoleUserRoleOrDealer = new DataTable();

        //        dtRoleUserRoleOrDealer = objDB.ExecuteStoredProcedureAndGetDataTable("SP_Get_UserRoleUserOrDealer", sRegionID, sStateorCountryID, iUserID, sSelDept);

        //        return dtRoleUserRoleOrDealer;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (objDB != null) objDB = null;
        //    }
        //}

        public DataSet GetRoleUserRoleOrDealer(string sRegionID, string sStateorCountryID, int iUserID, string sSelDept)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtRoleUserRoleOrDealer = new DataSet();

                dtRoleUserRoleOrDealer = objDB.ExecuteStoredProcedureAndGetDataset("SP_Get_UserRoleUserOrDealer", sRegionID, sStateorCountryID, iUserID, sSelDept);

                return dtRoleUserRoleOrDealer;
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
        public DataSet GetPendingSummary(int iUserID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dtGetPendingSummary = new DataSet();
                dtGetPendingSummary = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetPendingSummary", iUserID);
                if (dtGetPendingSummary != null)
                    return dtGetPendingSummary;
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

        //public string UseRoleUserDealerListName(int UserRole, int UserType, string sSelDept)
        //{
        //    string lblListName = "";
        //    if (UserRole == 3 && UserType == 1 || (UserRole == 3 && UserType == 2 && sSelDept == "6"))
        //        UserRole = 4;
        //    //else if (UserRole == 1 && UserType == 1)
        //    //    UserRole = 2;
        //    //Sujata 03062011
        //    //else if (UserRole == 9 || UserRole == 10 || UserRole == 11 || UserRole == 12)
        //    else if ((UserRole == 9 && UserType == 1) || (UserRole == 9 && UserType == 2 && sSelDept != "5") || UserRole == 10 || UserRole == 11 || UserRole == 12 || UserRole == 13 || UserRole == 14 || UserRole == 15 || UserRole == 16 || UserRole == 17 || UserRole == 18)
        //        //Sujata 03062011
        //        UserRole = 5;
        //    else if (((UserRole == 9 && sSelDept == "5") || (UserRole == 21 && (sSelDept == "5" || sSelDept == "7" || sSelDept == "1" || sSelDept == "3"))) && UserType == 2)
        //        UserRole = 1;
        //    else if (UserRole == 1 && (sSelDept == "5" || sSelDept == "7" || sSelDept == "1" || sSelDept == "3")) // For Sales and Service
        //        UserRole = 6;

        //    else if (UserRole == 19)
        //        UserRole = 5;
        //    switch (UserRole)
        //    {
        //        case 1: lblListName = "RSM"; break;
        //        case 2: lblListName = "ASM"; break;
        //        case 3: lblListName = "CSM"; break;
        //        case 4: lblListName = "DEALER'S"; break;
        //        case 5: lblListName = "Head"; break;
        //        case 6: lblListName = "GroupHead"; break;

        //    }
        //    return lblListName;
        //}
        public string UseRoleUserDealerListName(int UserRole, int UserType, string sSelDept, int ModelcategoryID)
        {
            int Option = 0;
            string lblListName = "";
            if (UserRole == 3 && UserType == 1 || (UserRole == 3 && UserType == 2 && (sSelDept == "6" || sSelDept == "7" || sSelDept == "5")))
            {
                if (ModelcategoryID == 1) Option = 41;
                else if (ModelcategoryID == 2) Option = 42;
                else if (ModelcategoryID == 3) Option = 43;            

            }
            //else if (UserRole == 1 && UserType == 1)
            //    UserRole = 2;
            //Sujata 03062011
            //else if (UserRole == 9 || UserRole == 10 || UserRole == 11 || UserRole == 12)
            else if ((UserRole == 9 && UserType == 1) || (UserRole == 9 && UserType == 2 && sSelDept != "5") || UserRole == 10 || UserRole == 11 || UserRole == 12 || UserRole == 13 || UserRole == 14 || UserRole == 15 || UserRole == 16 || UserRole == 17 || UserRole == 18)
            //Sujata 03062011
            //UserRole = 5;
            {
                if (ModelcategoryID == 1) Option = 51;
                else if (ModelcategoryID == 2) Option = 52;
                else if (ModelcategoryID == 3) Option = 53;
            }
            else if (((UserRole == 9 && sSelDept == "5") || (UserRole == 21 && (sSelDept == "5" || sSelDept == "7" || sSelDept == "1" || sSelDept == "3"))) && UserType == 2)
            //  UserRole = 1;
            {
                if (ModelcategoryID == 1) Option = 11;
                else if (ModelcategoryID == 2) Option = 12;
                else if (ModelcategoryID == 3) Option = 13;
            }

            else if (UserRole == 1 && (sSelDept == "5" || sSelDept == "7" || sSelDept == "1" || sSelDept == "3")) // For Sales and Service
            //  UserRole = 6;
            {
                if (ModelcategoryID == 1) Option = 61;
                else if (ModelcategoryID == 2) Option = 62;
                else if (ModelcategoryID == 3) Option = 63;
            }


            else if (UserRole == 19)
            //  UserRole = 5;
            {
                if (ModelcategoryID == 1) Option = 51;
                else if (ModelcategoryID == 2) Option = 52;
                else if (ModelcategoryID == 3) Option = 53;
            }
            else if (UserRole == 4)
            //  UserRole = 5;
            {
                if (ModelcategoryID == 1) Option = 41;
                else if (ModelcategoryID == 2) Option = 42;
                else if (ModelcategoryID == 3) Option = 43;
            }
            else if (UserRole == 3)
            //  UserRole = 5;
            {
                if (ModelcategoryID == 1) Option = 31;
                else if (ModelcategoryID == 2) Option = 32;
                else if (ModelcategoryID == 3) Option = 33;
            }
            else if (UserRole == 2)
            //  UserRole = 5;
            {
                if (ModelcategoryID == 1) Option = 21;
                else if (ModelcategoryID == 2) Option = 22;
                else if (ModelcategoryID == 3) Option = 23;
            }
            else
            {
                if (ModelcategoryID == 1) Option = 1;
                else if (ModelcategoryID == 2) Option = 2;
                else if (ModelcategoryID == 3) Option = 3;
            }

            switch (Option)
            {
                case 1: lblListName = "TRUCK"; break;
                case 2: lblListName = "BUS"; break;
                case 3: lblListName = "OTHER"; break;

                //case 11: lblListName = "RSM TRUCK"; break;
                case 11: lblListName = (sSelDept == "5") ? "RM" : "RSM"; break; 
                case 12: lblListName = "RSM BUS"; break;
                case 13: lblListName = "RSM OTHER"; break;

                //case 21: lblListName = "ASM TRUCK"; break;
                case 21: lblListName = (sSelDept == "5") ? "BM" : "ASM"; break;
                case 22: lblListName = "ASM BUS"; break;
                case 23: lblListName = "ASM OTHER"; break;

                //case 31: lblListName = "MSE TRUCK"; break;
                case 31: lblListName = (sSelDept == "6") ? "CSM" : (sSelDept == "5") ? "PM" : "MSE"; break;
                case 32: lblListName = "MSE BUS"; break;
                case 33: lblListName = "MSE OTHER"; break;

                //case 41: lblListName = (sSelDept == "6") ? "SPARES DEALER'S" : "TRUCK DEALER'S"; break;
                case 41: lblListName = (sSelDept == "6") ? "DEALER'S" : "DEALER'S"; break;
                case 42: lblListName = (sSelDept == "6") ? "" : "BUS DEALER'S"; break;
                case 43: lblListName = (sSelDept == "6") ? "" : "OTHER DEALER'S"; break;

                //case 51: lblListName = "Head TRUCK"; break;
                case 51: lblListName = "Head"; break;
                case 52: lblListName = "Head BUS"; break;
                case 53: lblListName = "Head OTHER"; break;

                //case 5: lblListName = "Head"; break;
                //case 6: lblListName = "GroupHead"; break;

            }
            return lblListName;
        }

        public bool SaveUserLoginHistory(ref int iID, int iUserID, string LoginDateTime, string LogoutDateTime, string Action)
        {

            bool bSaveRecord = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.BeginTranasaction();
                if (iID == 0)
                {
                    iID = objDB.ExecuteStoredProcedure("SP_SaveUserLoginHistory", iID, iUserID, LoginDateTime, LogoutDateTime, Action);
                }
                else
                {
                    objDB.ExecuteStoredProcedure("SP_SaveUserLoginHistory", iID, iUserID, LoginDateTime, LogoutDateTime, Action);
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

        public DataTable GetDealerByDepo(int iUserID, string sDepo)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtDealerByDepo = new DataTable();
                dtDealerByDepo = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDealerByDepo", iUserID, sDepo);
                return dtDealerByDepo;
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

        public DataTable GetDealerByStateORCountry(int iUserID, string sStateOrCountry, int BasicCatID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtStateOrCountry = new DataTable();
                dtStateOrCountry = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetDealerByStateORCountry", iUserID, sStateOrCountry, BasicCatID);
                return dtStateOrCountry;
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

        public DataTable GetSAPOrderInfo(string sFromDate, string sToDate, string sSelectType, string sDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSAPOrderInfo = new DataTable();
                dtSAPOrderInfo = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetSAPOrderInfo", sFromDate, sToDate, sSelectType, sDealerID);
                return dtSAPOrderInfo;
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

        public DataTable GetSAPOrderInfoDetails(int ID, string sDocumentNo, string sDocumentType, string sDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtSAPOrderInfoDetails = new DataTable();
                dtSAPOrderInfoDetails = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetSAPOrderInfoDtls", ID, sDocumentNo, sDocumentType, sDealerID);
                return dtSAPOrderInfoDetails;
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

        public DataSet GetDealerUserInfo(int iDealerID)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsDealerUserInfo = new DataSet();
                dsDealerUserInfo = objDB.ExecuteStoredProcedureAndGetDataset("SP_GetDealerUserInfo", iDealerID);
                return dsDealerUserInfo;
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

        public DataTable GetETVUserLoginAndEmpCode(int iUserType)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtUserLoginNameAndEmpCode = new DataTable();
                dtUserLoginNameAndEmpCode = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetUserLoginNameAndEmpCode", iUserType);
                return dtUserLoginNameAndEmpCode;
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

        public DataTable GetLocDetailsByDealerID(int iDealerID, string IsDistributor)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtUserLoginNameAndEmpCode = new DataTable();
                dtUserLoginNameAndEmpCode = objDB.ExecuteStoredProcedureAndGetDataTable("SP_GetLocDetailsByDealerID", iDealerID, IsDistributor);
                return dtUserLoginNameAndEmpCode;
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
        public DataTable FillDealersByUserID(int iUserID, string IsDistributor)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtUserLoginNameAndEmpCode = new DataTable();
                dtUserLoginNameAndEmpCode = objDB.ExecuteStoredProcedureAndGetDataTable("SP_FillDealersByUserID", iUserID, IsDistributor);
                return dtUserLoginNameAndEmpCode;
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

        public DataSet ValidatePermissionDetails(int UserID, string RegionID, string StateOrCountryID, string ModelCategory)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataSet DsDetails = new DataSet();
                DsDetails = objDB.ExecuteStoredProcedureAndGetDataset("SP_Validate_PermissionDetails", RegionID, StateOrCountryID, UserID, ModelCategory);
                if (DsDetails != null)
                {
                    return DsDetails;
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

        // To get max documnet number
        public int iGetMaxDocNo(string sDocName, string sDocKey)
        {
            int iMaxDocNo = 0;
            clsDB objDB = new clsDB();
            try
            {
                if (sDocName != "" && sDocName != null)
                {
                    iMaxDocNo = objDB.ExecuteStoredProcedure("SP_GetMaxDocNo_DealerUser", sDocName, sDocKey);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
            return iMaxDocNo;
        }

        //vrushali04052016_Begin for new web part MTI pending doc
        public DataTable FillDealersByUserIDMTI(int iUserID, string IsDistributor)
        {
            clsDB objDB = new clsDB();
            try
            {
                DataTable dtUserLoginNameAndEmpCode = new DataTable();
                dtUserLoginNameAndEmpCode = objDB.ExecuteStoredProcedureAndGetDataTable("SP_FillDealersByUserIDMTI", iUserID);
                return dtUserLoginNameAndEmpCode;
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
        //vrushali04052016_End

        // To update Max No In Document  No Series
        public void UpdateMaxNo(clsDB objDB, string sDocName, string sDocKey)
        {
            if (sDocName != "" && sDocName != null)
            {

                objDB.ExecuteStoredProcedure("SP_UpdateMaxDocNo_DealerUser", sDocName, sDocKey);

            }
        }
    }
}
