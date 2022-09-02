using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MANART_DAL;

namespace MANART_BAL
{
    public class ClsMail
    {
        public ClsMail()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public bool bSendMail(string sMailType, string sModule_Name, string sAdditionalMessage)
        {
            bool bSendMail = false;
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                objDB.ExecuteStoredProcedure("SP_Send_Mail", sMailType, sModule_Name, "", "", sAdditionalMessage);
                bSendMail = true;
                return bSendMail;
            }
            catch (Exception ex)
            {
                return bSendMail;
            }
            finally
            {
                if (objDB != null) objDB = null;
            }
        }
    }
}
