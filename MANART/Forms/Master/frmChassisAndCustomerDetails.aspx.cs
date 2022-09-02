using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MANART_DAL;
using MANART_BAL;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace MANART.Forms.Master
{
    public partial class frmChassisAndCustomerDetails : System.Web.UI.Page
    {
        private int iDealerID = 0;     
        private int iID;
        private int CustID=0;
        private DataSet dsState;
        private DataSet dsmodelcode;
        private DataSet srvnm;
        int iUserId = 0;
        int iHOBr_id = 0;
        clsSupplier ObjSup = new clsSupplier();
        private string sSelText, sSelType;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                iUserId = Func.Convert.iConvertToInt(Session["UserID"]);
                iHOBr_id = Func.Convert.iConvertToInt(Session["HOBR_ID"]);
                iDealerID = Func.Convert.iConvertToInt(Session["iDealerID"]);
                iID = Func.Convert.iConvertToInt(txtID.Text);
                //ds = ObjSup.GetMaxChassisAndCust(iHOBr_id, iDealerID);

                if (!IsPostBack)
                {
                    FillStateCountry();
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, true);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, true);
                    iID = Func.Convert.iConvertToInt(txtID.Text);
                  
                    chk.Checked = false;
                    lblmsg.Visible = false;
                    FillServnm();
                    FillModelCode();
                    SelectService();
                }  

            }
            catch (Exception ex) { }
         
        }

        private void SelectService()
        {
            try
            {
                foreach (GridViewRow row in srvno.Rows)
                {
                    TextBox textBox = row.FindControl("txtcouponrate") as TextBox;
                    textBox.ToolTip = "Please Select Service";
                    string srvnm = row.Cells[1].Text;
                    if (srvnm == "PDI" || srvnm == "SC1")
                    {
                        CheckBox chksrv = (CheckBox)row.FindControl("chkselctsrv");
                        chksrv.Enabled = true;
                     
                    }
                    else
                    {
                        CheckBox chksrv = (CheckBox)row.FindControl("chkselctsrv");
                        chksrv.Enabled = false;
                                            
                    }
                        
                }
            }
            catch (Exception ex) { }
        }

        private void FillServnm()
        {
            try
            {                           
               clsDB objDB = new clsDB();
               srvnm = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "SrvDetails");
               if (srvnm != null)
                {
                  srvno.DataSource = srvnm.Tables[0];
                  srvno.DataBind();
               }                
            }
            catch (Exception)
            {                
            }
        }

        private void FillModelCode()
        {
            try
            {
                clsDB objDB = new clsDB();
                dsmodelcode = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "ModelCode");
                if (dsmodelcode != null)
                {
                    drpmodelcode.DataSource = dsmodelcode.Tables[0];
                    drpmodelcode.DataTextField = "Model_code";
                    drpmodelcode.DataValueField = "id";
                    drpmodelcode.DataBind();
                    drpmodelcode.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex) { }
        }

      //  private void GetDataAndDisplay()
      //  {
      //      try
      //      {
      //          clsSupplier ObjDealer = new clsSupplier();
      //          DataSet ds = new DataSet();
      //          if (iID != 0)
      //          {
      //              ds = ObjDealer.GetChassisAndCustDetails(iID);
      //             DisplayData(ds);
      //              ObjDealer = null;
      //          }
      //          else
      //          {
      //              ds = null;
      //             DisplayData(ds);
      //              ObjDealer = null;
      //          }

      //      }
      //      catch (Exception ex)
      //      {
      //          Func.Common.ProcessUnhandledException(ex);
      //      }
      //  }

      //private void DisplayData(DataSet ds)
      //  {
      //      try
      //      {
      //              txtID.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["ID"]);
      //              txtcustname.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Customer_name"]);
      //              txtaddress1.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["add1"]);
      //              txtaddress2.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["add2"]);
      //              txtphone.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["phone1"]);
      //              ddlstate.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["state_id"]);
      //              FillRegion();
      //              ddlRegion.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Region_id"]);
                  
      //              txtpincode.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["pincode"]);
                 
      //              FillModelCode();
                   
      //              txtmobileno.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Mobile"]);
      //              txtChassisNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Chassis_no"]);
      //              drpmodelcode.SelectedValue = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Model_ID"]);
      //              txtEngineNo.Text = Func.Convert.sConvertToString(ds.Tables[0].Rows[0]["Engine_no"]);
              
      //      }
      //      catch (Exception)
      //      {                
      //          throw;
      //      }
      //  }

        protected void ToolbarImg_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton ObjImageButton = (ImageButton)sender;
                clsSupplier objSupplier = new clsSupplier();
                DataSet ds = new DataSet();
                if (ObjImageButton.ID == "ToolbarButton1")//for New
                {
                    ClearDealerHeader();
                    txtID.Text = "0";
                    iID = 0;
                    FillModelCode();
                    SelectService();
                   //ToolbarC.sSetMessage(MANART.WebParts.Toolbar.enmToolbarType.enmNothing);
                }
                else if (ObjImageButton.ID == "ToolbarButton2")//for Save
                {
                    if (bValidateRecord() == false) return;
                  //  iID = Func.Convert.iConvertToInt(txtID.Text);declare @CustName as varchar(250)
                  

                    iID = objSupplier.bSaveChassisAndCustomerDetials(iID, txtcustname.Text, txtphone.Text, txtaddress1.Text, txtaddress2.Text, ddlstate.SelectedValue, ddlRegion.SelectedValue, txtpincode.Text, iDealerID, txtDealercode.Text, CustID, iHOBr_id, txtChassisNo.Text, txtEngineNo.Text, drpmodelcode.SelectedItem.Text, txtmobileno.Text,txtinsdate.Text,txtvechicleno.Text);
                    string your_String = txtcouponno.Text;
                    Regex regex = new Regex("[^a-zA-Z0-9]");
                    string my_String = regex.Replace(your_String, "");
                    if (txtcouponno.Text != "")
                    {
                        SaveCopaonDetails(my_String);
                    }


                    if (iID > 0)
                    {

                        // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(4)", true);

                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    else
                    {         
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "ShowMessage(5)", true);
                       // Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(5);</script>");
                        Page.RegisterStartupScript("Close", "<script language='javascript'>ShowMessage(4);</script>");
                    }
                    //txtID.Text = Func.Convert.sConvertToString(iID);
                    FillSelectionGrid();

                }
                objSupplier = null;
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }


        }

        private void ClearDealerHeader()
        {
            try
            {             
                txtDealercode.Text = "";
                txtcustname.Text = "";
                txtphone.Text = "";
                txtaddress1.Text = "";
                txtaddress2.Text = "";
                txtmobileno.Text = "";
                txtpincode .Text = "";
                ddlRegion.Items.Clear();
                ddlstate.SelectedIndex = -1;
                txtChassisNo.Text = "";
                txtEngineNo.Text = "";
                drpmodelcode.Items.Insert(0, new ListItem("--Select--", "0"));
                drpmodelcode.SelectedIndex = -1;
                txtcouponno.Text = "";
                txtcouponrate.Text = "";
                FillServnm();
                txtinsdate.Text = "";
                chk.Checked = false;
                txtvechicleno.Text = "";
            }
            catch (Exception ex) { }
        }

        private void FillRegion()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {
                DataSet dsRegion;

                // dsRegion = objDB.ExecuteQueryAndGetDataset("select ID as ID,Region_Name as Name from M_region where Domestic_Export= 'D'");
                dsRegion = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", ddlstate.SelectedValue, "RegionCRM");
                if (dsRegion != null)
                {
                    ddlRegion.DataSource = dsRegion.Tables[0];
                    ddlRegion.DataTextField = "Name";
                    ddlRegion.DataValueField = "ID";
                    ddlRegion.DataBind();
                    if (Func.Convert.iConvertToInt(ddlstate.SelectedValue) == 0)
                    {
                        ddlRegion.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }


        }
        private void FillStateCountry()
        {
            // 'Replace Func.DB to objDB by Shyamal on 05042012
            clsDB objDB = new clsDB();
            try
            {

                ddlstate.Items.Clear();
                dsState = objDB.ExecuteStoredProcedureAndGetDataset("SP_EGPDealer_FillDropdown", 0, "State");
                if (dsState != null)
                {
                    ddlstate.DataSource = dsState.Tables[0];
                    ddlstate.DataTextField = "Name";
                    ddlstate.DataValueField = "ID";
                    ddlstate.DataBind();
                    ddlstate.Items.Insert(0, new ListItem("--Select--", "0"));
                }



            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objDB != null) objDB = null;
            }



        }

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillRegion();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool bValidateRecord()
        {

            string sMessage = " ";

            bool bValidateRecord = true;
              if (txtChassisNo.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Chassis No.";
                bValidateRecord = false;
            }

            else if (txtEngineNo.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter Engine No.";
                bValidateRecord = false;
            }
            else if (drpmodelcode.SelectedValue == "0")
            {
                sMessage = sMessage + "\\n Please select the ModelCode.";
                bValidateRecord = false;
            }
              else if (txtDealercode.Text == "")
              {
                  sMessage = sMessage + "\\n Please Enter Dealer Code.";
                  bValidateRecord = false;
              }

              else if (txtcustname.Text == "")
              {
                  sMessage = sMessage + "\\n Please Enter Customer Code.";
                  bValidateRecord = false;
              }

              else if (ddlstate.SelectedValue == "0")
              {
                  sMessage = sMessage + "\\n Please select the State.";
                  bValidateRecord = false;
              }
              else if (ddlRegion.SelectedValue == "0")
              {
                  sMessage = sMessage + "\\n Please select the Region.";
                  bValidateRecord = false;
              }


              else if (txtphone.Text == "")
              {
                  sMessage = sMessage + "\\n Please Enter Phone No";
                  bValidateRecord = false;
              }
              else if (txtaddress1.Text == "")
              {
                  sMessage = sMessage + "\\n Please Enter Address1.";
                  bValidateRecord = false;
              }
              else if (txtpincode.Text == "")
              {
                  sMessage = sMessage + "\\n Please Enter Pincode.";
                  bValidateRecord = false;
              }

            else if (chk.Checked == true)
            {
                if (txtinsdate.Text == "")
                {
                    sMessage = sMessage + "\\n Please Enter InsDate no.";
                    bValidateRecord = false;
                }
            }
           
         
          
           
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;

        }

        protected void SearchImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SearchGrid.iID = Func.Convert.iConvertToInt(txtID.Text);
                iID = Func.Convert.iConvertToInt(txtID.Text);
            //    GetDataAndDisplay();
            }
            catch (Exception ex) { }
        }

        private void FillSelectionGrid()
        {
            try
            {
                SearchGrid.AddToSearchCombo("Customer_name");
                SearchGrid.iDealerID = iDealerID;
                SearchGrid.sSqlFor = "ChassisAndCustDetls";
                SearchGrid.iBrHODealerID = iHOBr_id;
                SearchGrid.sGridPanelTitle = "Chassis And Customers  List";
            }
            catch (Exception ex) { }
        }

  

        private void btnReadonly()
        {
            clsCommon objCommon = null;
            try
            {
                objCommon = new clsCommon();
                if (objCommon.sUserRole == "15")
                {
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmConfirm, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmCancel, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmPrint, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmSave, false);
                    ToolbarC.EnableDisableImage(MANART.WebParts.Toolbar.enmToolbarType.enmNew, false);
                }
            }
            catch (Exception ex)
            {
                Func.Common.ProcessUnhandledException(ex);
            }
            finally
            {
                if (objCommon != null) objCommon = null;
            }
        }

  

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chk.Checked == true)
                {
                    lblmsg.Text = "PDI done for this chassis";
                    lblmsg.Visible = true;
                    txtinsdate.Enabled = true;
                   if (txtinsdate.Text!="")
                   {
                       txtinsdate.Enabled = false;
                   }
          
                }
                else
                {
                    lblmsg.Text = "";
                    lblmsg.Visible = false;
                    chk.Checked = false;
                    txtinsdate.Enabled = false;
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (bValidateRecord1() == false) return;
                iID = ObjSup.bUpdatechasisInsdate(iID,txtDealercode.Text,txtChassisNo.Text ,txtinsdate.Text,txtvechicleno.Text);
                if (iID != 0) { Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + " This Chassis Successfully Update!" + ".');</script>"); } else { Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + "This Chassis Not Available!" + ".');</script>"); }
           
                //var conn = new SqlConnection
                //{
                //    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                //};
                //string query = null;

                //clsDB objDB = new clsDB();
                //iID = objDB.ExecuteStoredProcedure("SP_ChassisCustomerInsert");               
                //if (bValidateRecord1() == false) return;
                //if (chk.Checked == true)
                //{
                //    DateTime insdate = Convert.ToDateTime(txtinsdate.Text);
                //    string now = insdate.ToString("yyyy-MM-dd");

                //    query = "UPDATE  M_ChassisMaster SET   Chassis_no ='" + txtChassisNo.Text + "', Installation_date ='" + now + "'  where Chassis_no='" + txtChassisNo.Text + "'";
                //}
                //else
                //{
                //    query = "UPDATE  M_ChassisMaster SET   Chassis_no ='" + txtChassisNo.Text + "',  where Chassis_no='" + txtChassisNo.Text + "'";
                //}
                //conn.Open();

                //SqlCommand cmd = new SqlCommand(query, conn);
                //int retVal = cmd.ExecuteNonQuery();
               
                //conn.Close();
            }
            catch (Exception ex) { }
        }

        private bool bValidateRecord1()
        {
            string sMessage = " ";
            bool bValidateRecord = true;
            if (txtChassisNo.Text == "")
            {
                sMessage = sMessage + "\\n Please Enter chassis no.";
                bValidateRecord = false;
            }
           
            else if (chk.Checked == true)
            {
                if (txtinsdate.Text == "")
                {
                    sMessage = sMessage + "\\n Please Enter InsDate no.";
                    bValidateRecord = false;
                }
            }
            
            if (bValidateRecord == false)
            {
                Page.RegisterStartupScript("Close", "<script language='javascript'>alert('" + sMessage + ".');</script>");
            }
            return bValidateRecord;
                                        
        }

        private void SaveCopaonDetails(string Mystring)
        {
            try
            {
               
                   int counter = 0;
                  foreach (GridViewRow row in srvno.Rows)            
                  {
                    counter++;
                    var SrvID = row.FindControl("lblID") as Label;
                    var Rate = row.FindControl("txtcouponrate") as TextBox;    
                    string Coupon;
                    CheckBox ch = (CheckBox)row.FindControl("checkboxcoupon");                 
                    if (ch.Checked == true)
                    {
                        Coupon = "Y";
                    }
                    else { Coupon = "N"; }
                    string MSPD = "0";
                    CheckBox Selectsrvnm = (CheckBox)row.FindControl("chkselctsrv");
                    if (Selectsrvnm.Checked== true)
                    {
                        iID = ObjSup.bSaveCouponDetails(iID, SrvID.Text, txtChassisNo.Text, Rate.Text, Mystring + '_' + counter, MSPD, Coupon);
                    }

                    //var conn = new SqlConnection
                    //{
                    //    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                    //};
                    //string query = null;
                    //query = "select M_chassismaster.ID   from M_chassismaster where Chassis_no='" + txtChassisNo.Text + "'";
                    //conn.Open();
                    //SqlCommand cmd = new SqlCommand(query, conn);
                    //DataTable dt = new DataTable();
                    //dt.Load(cmd.ExecuteReader());
                    //query = "select * from M_ChassisCoupon where Chassis_ID='" + dt.Rows[0]["ID"] + "' AND  ServID='" + textbox.Text + "'";
                    //SqlCommand cmd2 = new SqlCommand(query, conn);
                    //DataTable dt1 = new DataTable();
                    //dt1.Load(cmd2.ExecuteReader());
                    //if (dt.Rows.Count != 0 && dt1.Rows.Count == 0)
                    //{
                    //    query = "INSERT INTO M_ChassisCoupon(Chassis_ID, ServID, CouponNo, LSPSD, MSPSD,  Status)VALUES('" + dt.Rows[0]["ID"] + "','" + textbox.Text + "','" + txtcouponno.Text + '_' + counter + "','" + txtcouponrate.Text + "','0','" + Coupon + "')";
                    //}
                    //else if (dt.Rows.Count != 0 && dt1.Rows.Count != 0)
                    //{
                    //    //query = "UPDATE  M_ChassisCoupon SET   Chassis_ID ='" + dt.Rows[0]["ID"] + "', ServID ='" + textbox.Text + "', CouponNo ='" + txtcouponno.Text + '_' + counter + "', LSPSD ='" + txtcouponrate.Text + "', MSPSD ='0', Coupon_Block ='N', Status='" + Coupon + "' WHERE  (Chassis_ID = '" + dt.Rows[0]["ID"] + "') AND (ServID = '" + textbox.Text + "')";    
                    //    query = "UPDATE  M_ChassisCoupon SET     LSPSD ='" + txtcouponrate.Text + "', Status='" + Coupon + "' WHERE  (Chassis_ID = '" + dt.Rows[0]["ID"] + "') AND (ServID = '" + textbox.Text + "')";
                    //}
                    //SqlCommand cmd1 = new SqlCommand(query, conn);
                    //int retVal = cmd1.ExecuteNonQuery(); conn.Close();
                 
                }
            }
            catch (Exception ex) { }
                                                                                
        }
         [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetInvoice(string chassisNo)
        {
            try
            {
                //Convert.ToInt32(HttpContext.Current.)***YOGESH***;
               
                var conn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                };

                string query = "select * from M_ChassisMaster where Chassis_no='" + chassisNo + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                return dt.Rows.Count.ToString();
            }
            catch (Exception ex) { return null; }
        }

         protected void txtChassisNo_TextChanged(object sender, EventArgs e)
         {
             try
             {
            
             
                 FillClear();
                 clsDB objDB = new clsDB();
                 DataSet chassisdetails = new DataSet();
                 chassisdetails = objDB.ExecuteStoredProcedureAndGetDataset("Get_ChassisDetails", txtChassisNo.Text);
                 if (chassisdetails.Tables[0].Rows.Count != 0)
                 {

                     txtDealercode.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Dealer_Spares_Code"]);
                     txtcustname.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Customer_name"]);
                     txtphone.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["phone1"]);
                     txtaddress1.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["add1"]);
                     txtaddress2.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["add2"]);
                     txtmobileno.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Mobile"]);
                     txtpincode.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["pincode"]);
                     txtEngineNo.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Engine_no"]);



                     if (chassisdetails.Tables[0].Rows[0]["Model_ID"] != null)
                     {
                         drpmodelcode.SelectedValue = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Model_ID"]);
                     }
                     if (chassisdetails.Tables[0].Rows[0]["state_id"] != null)
                     {
                         ddlstate.SelectedValue = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["state_id"]);
                     }
                     if (chassisdetails.Tables[0].Rows[0]["Region_id"] != null)
                     {

                         ddlRegion.SelectedValue = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Region_id"]);
                     }
                     if (chassisdetails.Tables[0].Rows[0]["Installation_date"] != DBNull.Value)
                     {
                         txtinsdate.Text = Func.Convert.tConvertToDate(chassisdetails.Tables[0].Rows[0]["Installation_date"], false);
                         chk.Checked = true;
                         txtinsdate.Enabled = false;
                     }
                     else
                     {
                         chk.Checked = false;
                         txtinsdate.Enabled = false;
                     }
                     if (chassisdetails.Tables[0].Rows[0]["M8_no"] != DBNull.Value)
                     {
                         M8Customer();
                     }
                     else
                     {
                         M8CustomerEnable();
                     }
                     txtvechicleno.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["Reg_no"]);
                     txtcouponno.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["CouponNo"]);
                     txtcouponno.Text = Func.Convert.sConvertToString(chassisdetails.Tables[0].Rows[0]["CouponNo"]).Split('_')[0];
                     int j = 0; int i = 0;
                     foreach (GridViewRow row in srvno.Rows)
                     {
                         TextBox textBox = row.FindControl("txtcouponrate") as TextBox;
                         CheckBox chkselctsrv = row.FindControl("chkselctsrv") as CheckBox;
                         CheckBox chkstatusdetails = row.FindControl("checkboxcoupon") as CheckBox;
                         if (txtcouponno.Text != "")
                         {
                             CheckBox chksrv = (CheckBox)row.FindControl("chkselctsrv");

                             if (row.RowIndex < chassisdetails.Tables[0].Rows.Count)
                             {

                                 if (chassisdetails.Tables[0].Rows[i]["Status"].ToString() == "Y")
                                 {
                                     chkstatusdetails.Checked = true;
                                 }
                                 else
                                 {
                                     chkstatusdetails.Checked = false;
                                 }
                                 textBox.Text = chassisdetails.Tables[0].Rows[i]["LSPSD"].ToString();
                                 chkselctsrv.Checked = true;
                                 chksrv.Enabled = true;
                                 textBox.Enabled = true;
                                 i++;
                             }
                             else
                             {
                                 // textBox.Text = "0";                               
                                 if (j == 0)
                                 {
                                     chksrv.Enabled = true;
                                     j++;
                                 }
                             }
                         }

                     }
                 }
                 else
                 {
                     M8CustomerEnable();
                 }
             }
               

             catch (Exception ex) { }

         }

         private void M8CustomerEnable()
         {
             try
             {
                 txtcustname.Enabled = true;
                 //txtDealercode.Enabled = true;
                 //txtaddress1.Enabled = true;
                 //txtaddress2.Enabled = true;
                 //txtmobileno.Enabled = true;
                 //txtpincode.Enabled = true;
                 //txtphone.Enabled = true;
                 //ddlRegion.Enabled = true;
                 ddlstate.Enabled = true;
             }
             catch (Exception)
             {
                 
                 throw;
             }
         }

         private void M8Customer()
         {
             try
             {

                 txtcustname.Enabled = false;
                 //txtDealercode.Enabled = false;
                 //txtaddress1.Enabled = false;
                 //txtaddress2.Enabled = false;
                 //txtmobileno.Enabled = false;
                 //txtpincode.Enabled = false;
                 //txtphone.Enabled = false;
                 //ddlRegion.Enabled = false;
                 ddlstate.Enabled = false;
                 

             }
             catch (Exception)
             {
                 
                 throw;
             }
         }

         private void FillClear()
         {
             try
             {
                 FillModelCode();
                 txtinsdate.Text = "";
                 txtEngineNo.Text = "";
                 txtinsdate.Text = "";
                 txtvechicleno.Text = "";
               
                 txtcustname.Text = "";
                 txtDealercode.Text = "";
                 txtaddress1.Text = "";
                 txtaddress2.Text = "";
                 txtmobileno.Text = "";
                 txtpincode.Text = "";
                 txtphone.Text = "";
                 FillStateCountry();
                 FillRegion();
                 txtcouponno.Text = "";
                 foreach (GridViewRow row in srvno.Rows)
                 {
                     TextBox textBox = row.FindControl("txtcouponrate") as TextBox;
                     textBox.Text = "0";
                 }
                 
             }
             catch (Exception)
             {                
               
             }
         }

         protected void chkselctsrv_CheckedChanged(object sender, EventArgs e)
         {
             try
             {
                 var rowIndex = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
                 int index = srvno.Rows.Count;
                  GridViewRow row = srvno.Rows[rowIndex];
                  GridViewRow row1 = srvno.Rows[rowIndex];
                    if (index != rowIndex+1)
                    {
                        row1 = srvno.Rows[rowIndex + 1];
                    }
                      TextBox textBox = row.FindControl("txtcouponrate") as TextBox;
                      CheckBox chservno = (CheckBox)row.FindControl("chkselctsrv");
                      if (chservno.Checked == true)
                      {
                          CheckBox chservno1 = (CheckBox)row1.FindControl("chkselctsrv");
                          chservno1.Enabled = true;
                          textBox.Enabled = true;
                          textBox.ToolTip = "";
                      }
                      else
                      {
                          Fillsrvstatus(row, row1);

                      }
                  
             }
             catch (Exception)
             {
             }  
         }

         private void Fillsrvstatus(GridViewRow row,GridViewRow row1)
         {
             try
             {
                 foreach (GridViewRow Newrow in srvno.Rows)
                 {
                       string srvnm1 = row.Cells[1].Text;
                         CheckBox chservno = (CheckBox)row.FindControl("chkselctsrv");
                    
                     if (Newrow.RowIndex >= row1.RowIndex)
                     {
                         CheckBox chservno1 = (CheckBox)Newrow.FindControl("chkselctsrv");
                         TextBox textBox = Newrow.FindControl("txtcouponrate") as TextBox;
                         chservno1.Enabled = false;
                         chservno1.Checked = false;
                         textBox.Enabled = false;
                         textBox.ToolTip = "Please Select Service";
                     }   

                 }
             }
             catch (Exception)
             {
                
             }
         }

         protected void lblSelectCustomer_Click(object sender, EventArgs e)
         {
             try
             {
                 DisplayData();
                 ModalPopUpExtender1.Show();
             }
             catch (Exception)
             {

               }
         }

         private void DisplayData()
         {
             // 'Replace Func.DB to objDB by Shyamal on 05042012
             clsDB objDB = new clsDB();
             try
             {
                 DataTable dtSrchgrid;

                 sSelType = DdlSelctionCriteria.SelectedValue.ToString();
                 sSelText = txtSearch.Text.ToString();


                 dtSrchgrid = objDB.ExecuteStoredProcedureAndGetDataTable("SP_EGPDealer_FillDropdown", 0, "CustDetails");
                 // ViewState["Chassis"] = dtSrchgrid;
                 //Session["ChassisDetails"] = dtSrchgrid;

                 if (dtSrchgrid == null)
                 {
                     return;
                 }

                 DataView dvDetails = new DataView();
                 dvDetails = dtSrchgrid.DefaultView;
                 if (DdlSelctionCriteria.SelectedValue == "Customer_name" && txtSearch.Text != "")
                     dvDetails.RowFilter = (DdlSelctionCriteria.SelectedValue + " LIKE '*" + txtSearch.Text + "*'");
                 CustomerGrid.DataSource = dvDetails;
                 //  ChassisGrid.DataSource = dtSrchgrid;
                 CustomerGrid.DataBind();

                 if (CustomerGrid.Rows.Count == 0) return;



             }
             catch (Exception ex)
             {
                 Func.Common.ProcessUnhandledException(ex);
             }
             finally
             {
                 if (objDB != null) objDB = null;
             }
         }

         protected void CustomerGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
         {
             CustomerGrid.PageIndex = e.NewPageIndex;
             DisplayData();
             ModalPopUpExtender1.Show();
         }

         protected void btnSearch_Click(object sender, EventArgs e)
         {
             if (txtSearch.Text != "" && btnSearch.Text == "Search")
                 btnSearch.Text = "ClearSearch";
             else if (txtSearch.Text != "" && btnSearch.Text == "ClearSearch")
             {
                 txtSearch.Text = "";
                 btnSearch.Text = "Search";
             }
             DisplayData();
             ModalPopUpExtender1.Show();
         }

         protected void btnSelectCustomer_Click1(object sender, ImageClickEventArgs e)
         {
             try
             {
                 clsDB objDB = new clsDB();
                 DataTable custhistory;
                 GridViewRow clickedrow = ((ImageButton)sender).NamingContainer as GridViewRow;
                 int Cust_ID = Convert.ToInt32(CustomerGrid.DataKeys[clickedrow.RowIndex].Value);
                 custhistory = objDB.ExecuteStoredProcedureAndGetDataTable("SP_EGPDealer_FillDropdown", Cust_ID, "SelectCust");
                 CusthistoryDetails(custhistory);
             }
             catch (Exception)
             {

                 throw;
             }
         }

         private void CusthistoryDetails(DataTable custhistory)
         {
             try
             {
                 if (custhistory.Rows.Count > 0)
                 {
                     txtDealercode.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["Dealer_Vehicle_Code"]);
                     txtcustname.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["Customer_name"]);
                     txtphone.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["phone1"]);
                     txtaddress1.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["add1"]);
                     txtaddress2.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["add2"]);
                     txtmobileno.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["Mobile"]);
                     txtpincode.Text = Func.Convert.sConvertToString(custhistory.Rows[0]["pincode"]);
                     if (custhistory.Rows[0]["state_id"] != null)
                     {
                         ddlstate.SelectedValue = Func.Convert.sConvertToString(custhistory.Rows[0]["state_id"]);
                     }
                     FillRegion();
                     if (custhistory.Rows[0]["Region_id"] != null)
                     {

                         ddlRegion.SelectedValue = Func.Convert.sConvertToString(custhistory.Rows[0]["Region_id"]);
                     }
                 }
             }
             catch (Exception)
             {
                
               }
         }

         [System.Web.Services.WebMethod(EnableSession = true)]
         public static string GetDealer(string DealerNo)
         {
             try
             {
                 //Convert.ToInt32(HttpContext.Current.)***YOGESH***;

                 var conn = new SqlConnection
                 {
                     ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString
                 };

                 string query = "select * from M_dealer where Dealer_Vehicle_code='" + DealerNo + "'";
                 conn.Open();
                 SqlCommand cmd = new SqlCommand(query, conn);
                 DataTable dt = new DataTable();
                 dt.Load(cmd.ExecuteReader());
                 conn.Close();
                 return dt.Rows.Count.ToString();
             }
             catch (Exception ex) { return null; }
         }
    }
}