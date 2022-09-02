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

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for clsGlobal
    /// </summary>
    public class clsGlobal
    {
        public enum enmConnectionType
        {
            enmconnectionOfMaster,
            enmconnectionOfTransaction
        }
        public enum enmFormState
        {
            enmEmpty_State,
            enmNew_State,
            enmUpdate_State
        }

        public clsGlobal()
        {

        }
    }
}
