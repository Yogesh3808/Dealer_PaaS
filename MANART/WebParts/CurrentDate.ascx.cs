using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MANART.WebParts
{
    public partial class CurrentDate : System.Web.UI.UserControl
    {
        private string sDate = "";
        private string l_sOnBlurScript = "";
        private string l_sOnKeyPressScript = "";

        private bool l_bCheckforCurrentDate = true;
        private bool l_bCheckDateGreaterThanOrEqualToCurrentDate = false;
        private bool l_bMandatory = true;
        private int width = 0;
        private string _MapToTblFieldName;
        private bool _AllowTochange;

        public CurrentDate()
        {
            txtDocDate = new TextBox();
            txtDocDate.Attributes.Add("runat", "server");
        }
        public bool AllowTochange
        {
            get { return _AllowTochange; }
            set
            {
                _AllowTochange = value;
                // If data of this control allow to change then set it true else set false
                if (value == false)
                {
                    txtDocDate.Enabled = false;
                }
                else
                {
                    txtDocDate.Enabled = true;
                }
            }
        }

        ///<summary> Gets or sets the text displayed when the mouse pointer hovers over the control</summary>            
        public string ToolTipText
        {
            get { return txtDocDate.ToolTip; }
            set
            {
                if (txtDocDate != null)
                {
                    txtDocDate.ToolTip = value;
                }
            }
        }
        /// <summary>
        /// Get CSS Style For Date Control
        /// </summary>
        public CssStyleCollection Style
        {
            get { return txtDocDate.Style; }

        }
        ///<summary> Gets or sets the tablefield map to the control</summary>            
        public string MapToTblFieldName
        {
            get { return _MapToTblFieldName; }
            set { _MapToTblFieldName = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public string sOnKeyPressScript
        {
            get { return l_sOnKeyPressScript; }
            set { l_sOnKeyPressScript = value; }
        }

        public string sOnBlurScript
        {
            get { return l_sOnBlurScript; }
            set { l_sOnBlurScript = value; }
        }
        public string Text
        {
            get
            {
                return txtDocDate.Text;
            }
            set
            {
                txtDocDate.Text = value;
                sDate = value;
                //  txtDocDate.Text = DateTime.Now.ToShortDateString();   

            }
        }
        public bool Enabled
        {
            get
            {
                return txtDocDate.Enabled;
            }
            set
            {
                if (value == false)
                {
                    txtDocDate.Enabled = false;
                }
                else
                {
                    txtDocDate.Enabled = true;
                }
            }
        }
        public bool ReadOnly
        {
            get
            {
                return txtDocDate.ReadOnly;
            }
            set
            {
                if (value == true)
                {
                    txtDocDate.Enabled = false;
                }
                else
                {
                    txtDocDate.Enabled = true;
                }
            }
        }
        //public bool bCheckforCurrentDate
        //{
        //    get
        //    {
        //        return l_bCheckforCurrentDate;
        //    }
        //    set
        //    {
        //        l_bCheckforCurrentDate = value;
        //    }
        //}

        public bool bCheckDateGreaterThanOrEqualToCurrentDate
        {
            get
            {
                return l_bCheckDateGreaterThanOrEqualToCurrentDate;
            }
            set
            {
                l_bCheckDateGreaterThanOrEqualToCurrentDate = value;
            }
        }
        public bool FontBold
        {
            get
            {
                return txtDocDate.Font.Bold;
            }
            set
            {

                txtDocDate.Font.Bold = value;

            }
        }
        public bool Mandatory
        {
            get
            {
                return l_bMandatory;
            }
            set
            {
                l_bMandatory = value;
            }
        }

        public short TabIndex
        {
            get
            {
                return txtDocDate.TabIndex;
            }
            set
            {
                txtDocDate.TabIndex = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDocDate.Attributes.Add("readonly", "readonly");
            //SetScriptToDate();
            if (l_bMandatory == true)
            {
                lblMandatory.Visible = true;
            }
            else
            {
                lblMandatory.Visible = false;
            }
            //ControlContainer.Width = Unit.Percentage(20).ToString();
        }
        protected override void OnPreRender(EventArgs e)
        {
            //txtDocDate.Text = BLL.Func.Convert.tConvertToDate(sDate, false); ;        
            if (sDate != "")
            {
                txtDocDate.Text = sDate;
            }
            // txtDocDate.Style.Add("readOnly", "true");
            if (width == 0)
            {
                txtDocDate.Width = Unit.Pixel(145);
            }
            else
            {
                txtDocDate.Width = Unit.Percentage(width);
                //Calc1.CssClass="CalendarCSS";         
            }

        }
        public void SetScriptToDate()
        {
            if (l_sOnKeyPressScript == "")
            {
                if (l_bCheckforCurrentDate == true)
                {
                    txtDocDate.Attributes.Add("onkeypress", "CheckForDateValue(event,this,null,true)");
                }
                else
                {
                    txtDocDate.Attributes.Add("onkeypress", "CheckForDateValue(event,this,null,false)");
                }

                if (l_bCheckDateGreaterThanOrEqualToCurrentDate == true)
                {
                    txtDocDate.Attributes.Add("onkeypress", "CheckDateGreaterThanOrEqualToCurrentDate(this)");
                }
            }
            else
            {
                txtDocDate.Attributes.Add("onkeypress", l_sOnKeyPressScript);
            }
            if (l_sOnBlurScript == "")
            {
                if (l_bCheckforCurrentDate == true)
                {
                    txtDocDate.Attributes.Add("onblur", "CheckForDateValue(event,this,null,true)");
                }
                else
                {
                    txtDocDate.Attributes.Add("onblur", "CheckForDateValue(event,this,null,false)");
                }
                if (l_bCheckDateGreaterThanOrEqualToCurrentDate == true)
                {
                    txtDocDate.Attributes.Add("onkeypress", "CheckDateGreaterThanOrEqualToCurrentDate(this)");
                }



            }
            else
            {
                txtDocDate.Attributes.Add("onblur", l_sOnBlurScript);
            }
        }
        protected void txtDocDate_TextChanged(object sender, EventArgs e)
        {

        }
    }
}