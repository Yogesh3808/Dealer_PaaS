using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MANART_BAL;
using MANART_DAL;

namespace MANART.WebParts
{
    public partial class CurrentDateTime : System.Web.UI.UserControl
    {
        private bool l_bMandatory = true;
        private bool l_TimeVisible = false;

        public string Date
        {
            get { return txtDocDate.Text; }
            set
            {
                if (txtDocDate != null)
                {
                    txtDocDate.Text = value;
                }
            }
        }
        public string Hr
        {
            get { return DrpHrs.SelectedValue; }
            set
            {
                if (DrpHrs != null)
                {
                    DrpHrs.SelectedValue = value;
                }
            }
        }
        public string Min
        {
            get { return DrpMin.SelectedValue; }
            set
            {
                if (DrpMin != null)
                {
                    DrpMin.SelectedValue = value;
                }
            }
        }

        public string Text
        {
            get { return (l_TimeVisible == true) ? Date + " " + Hr + ":" + Min : Date; }
            set
            {
                if (value == "")
                {
                    txtDocDate.Text = "";
                    if (l_TimeVisible == true)
                    {
                        DrpHrs.SelectedValue = "00";
                        DrpMin.SelectedValue = "00";
                    }
                }
                else
                {
                    string svlaue = value;
                    DateTime dt = new DateTime();
                    dt = Convert.ToDateTime(Func.Convert.tConvertToDate(svlaue, (l_TimeVisible == true) ? true : false));

                    if (dt != null)
                    {
                        txtDocDate.Text = Func.Convert.tConvertToDate(dt, false);
                        if (l_TimeVisible == true)
                        {
                            DrpHrs.SelectedValue = dt.Hour.ToString().PadLeft(2, '0');
                            DrpMin.SelectedValue = dt.Minute.ToString().PadLeft(2, '0');
                        }
                    }
                }
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

        public bool bTimeVisible
        {
            get
            {
                return l_TimeVisible;
            }
            set
            {
                l_TimeVisible = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (l_bMandatory == true)
            {
                lblMandatory.Visible = true;
            }
            else
            {
                lblMandatory.Visible = false;
            }
            if (l_TimeVisible == true)
            {
                DrpHrs.Visible = true;
                DrpMin.Visible = true;
            }
            else
            {
                DrpHrs.Visible = false;
                DrpMin.Visible = false;
            }
        }
    }
}