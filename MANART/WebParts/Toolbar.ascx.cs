using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MANART_BAL;
using MANART_DAL;

namespace MANART.WebParts
{
    public partial class Toolbar : System.Web.UI.UserControl
    {
        private int _iFormIdToOpenForm = 0;
        private int _iValidationIdForConfirm = 0;
        private int _iValidationIdForSave = 0;
        public bool bUseImgOrButton = false;
        public enum enmToolbarType
        {
            enmNew = 1,
            enmSave = 2,
            enmConfirm = 3,
            enmCancel = 4,
            enmPrint = 5,
            enmHelp = 6,
            enmNothing = 99
        }
        public delegate void ImageClick(object sender, ImageClickEventArgs e);
        public event ImageClick Image_Click;

        public int iValidationIdForSave
        {
            get { return _iValidationIdForSave; }
            set { _iValidationIdForSave = value; }
        }
        public int iValidationIdForConfirm
        {
            get { return _iValidationIdForConfirm; }
            set { _iValidationIdForConfirm = value; }
        }

        public int iFormIdToOpenForm
        {
            get { return _iFormIdToOpenForm; }
            set { _iFormIdToOpenForm = value; }
        }

        public void sSetMessage(enmToolbarType ValenmToolbarType)
        {
            if (ValenmToolbarType == enmToolbarType.enmConfirm)
            {
                txtShowToolBarName.Text = " Record is confirmed.";
            }
            else if (ValenmToolbarType == enmToolbarType.enmCancel)
            {
                txtShowToolBarName.Text = " Record is Cancelled.";
            }
            else if (ValenmToolbarType == enmToolbarType.enmSave)
            {
                txtShowToolBarName.Text = " Record is saved.";
            }
            else if (ValenmToolbarType == enmToolbarType.enmNothing)
            {
                txtShowToolBarName.Text = " Record is editable.";
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string sMenuId = Func.Convert.sConvertToString(Request.QueryString["MenuID"]);
            // Add Function on New Button
            ToolbarButton1.Attributes.Add("onClick", "return OpenFormOnNewClick('" + _iFormIdToOpenForm + "');");

            // Add Function on Save Button
            ToolbarButton2.Attributes.Add("onClick", "return CheckForSave('" + _iValidationIdForSave + "');");

            // Add Function on Confirm Button
            //Megha24/06/2011 
            if (sMenuId != "464") //This code added only DACreation  
            {
                ToolbarButton3.Attributes.Add("onClick", "return CheckForConfirm('" + _iValidationIdForConfirm + "');");
            }
            //Megha24/06/2011 
            //VItthal Report Start
            string strReportpath;
            strReportpath = Func.Convert.sConvertToString(ConfigurationManager.AppSettings["ReportPath"]);
            ToolbarButton5.Attributes.Add("onClick", "return ShowReport('" + _iFormIdToOpenForm + "','" + strReportpath + "');");
            //VItthal Report End
            //ToolbarButton5.Attributes.Add("onClick", "return ShowReport('" + _iFormIdToOpenForm + "');");            

            if (sMenuId != "")
            {
                //SetToolbarAsPerUserRights((DataTable)Session["UserPermissions"], sMenuId);
            }
        }
        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            Image_Click(sender, e);

        }
        public void EnableDisableImage(DataTable dtToolbarDetails)
        {
            bool bIsReadOnly = false;
            ImageButton TmpImageButton;
            if (dtToolbarDetails != null)
            {
                for (int i = 1; i < dtToolbarDetails.Rows.Count - 1; i++)
                {
                    bIsReadOnly = Func.Convert.bConvertToBoolean(dtToolbarDetails.Rows[i]["ReadOnly"]);
                    TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton" + (i));
                    if (TmpImageButton != null)
                    {
                        if (bIsReadOnly == true)
                        {
                            TmpImageButton.Enabled = false;
                        }
                    }

                }
            }
        }
        public void EnableDisableImage(enmToolbarType ValenmToolbarType, bool bEnable)
        {
            ImageButton TmpImageButton = null;
            TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton" + ((int)ValenmToolbarType));
            if (TmpImageButton != null)
            {
                TmpImageButton.Enabled = bEnable;
            }
        }
        private void SetDefaultDisable()
        {
            ImageButton TmpImageButton;
            for (int i = 1; i < 6; i++)
            {
                TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton" + (i));
                if (TmpImageButton != null)
                {
                    //    TmpImageButton.Attributes.Add("disabled", "disabled");
                    TmpImageButton.Enabled = false;
                }

            }
        }
        // To Set ToolbarImage working As Per User Rightts
        private void SetToolbarAsPerUserRights(DataTable dtRights, string MenuId)
        {
            SetDefaultDisable();
            string sMenuId = "";
            string sRight = "";
            ImageButton TmpImageButton = null;
            if (dtRights == null) return;
            foreach (DataRow dr in dtRights.Rows)
            {
                sMenuId = dr["MenuID"].ToString();
                sRight = dr["Rights"].ToString();
                if (sMenuId == MenuId)
                {
                    if (sRight == "N")// New 
                    {
                        TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton1");
                        TmpImageButton.Enabled = true;
                    }
                    else if (sRight == "S")//Save
                    {
                        TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton2");
                        TmpImageButton.Enabled = true;
                    }
                    else if (sRight == "M")//Confirm
                    {
                        TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton3");
                        TmpImageButton.Enabled = true;
                    }
                    else if (sRight == "C") // Cancel
                    {
                        TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton4");
                        TmpImageButton.Enabled = true;
                    }
                    else if (sRight == "P")//Print
                    {
                        TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton5");
                        TmpImageButton.Enabled = true;
                    }
                    //else if (sRight == "P") // Send To SAP
                    //{
                    //    TmpImageButton = (ImageButton)ToolbarContainer.FindControl("ToolbarButton5");
                    //    TmpImageButton.Attributes.Add("disabled", "disabled");
                    //}

                }


            }


        }
    }
}