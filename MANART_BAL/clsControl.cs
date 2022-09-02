using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using AjaxControlToolkit;
using System.Web.UI.WebControls;
using MANART_DAL;

namespace MANART_BAL
{
    /// <summary>
    /// Summary description for SControl
    /// </summary>

    public class SControl
    {
        #region Enum Variables

        /////series is start from  1 and end with 18     
        ///<summary> enum is used to define textbox type</summary>         
        public enum enmTextBoxType
        {
            /// <summary>To select textbox of type 'String' </summary>
            enmString = 1,
            /// <summary>To select textbox of type 'StringWithLowerCase'</summary>            
            enmStringWithLowerCase = 2,
            /// <summary>To select textbox of type 'StringWithUpperCase'</summary>
            enmStringWithUpperCase = 3,
            /// <summary>To select textbox of type 'StringWithProperCase'</summary>
            enmStringWithProperCase = 4,
            /// <summary>To select textbox of type 'Numeric'</summary>
            enmNumeric = 5,
            /// <summary>To select textbox of type 'NumericWithDecimal' </summary>
            enmNumericWithDecimal = 6,
            /// <summary>To select textbox of type 'Email'</summary>
            enmEmail = 7,
            /// <summary>To select textbox of type 'PhoneNo'</summary>
            enmPhoneNo = 8,
            /// <summary>To select textbox of type 'MobileNo'</summary>
            enmMobileNo = 9,
            /// <summary>To select textbox of type 'FaxNo'</summary>
            enmFaxNo = 10,
            /// <summary>To select textbox of type 'VehicleNo'</summary>
            enmVehicleNo = 11,
            /// <summary>To select textbox of type 'PinCode'</summary>
            enmPincCode = 12,
            /// <summary>To select textbox of type 'AlphaNumeric'</summary>
            enmAlphaNumeric = 13,
            /// <summary>To select textbox of type 'Memo'</summary>
            enmMemo = 14,
            /// <summary>To select textbox of type 'Alphabets(Text only)'</summary>
            enmAlphabets = 15,


        }

        // series is start from  31 and end with 33
        /// <summary>enum is used to define Type Of Combobox</summary>
        public enum enmComboBoxType
        {
            /// <summary>For Yes/No selection</summary>
            enmYesNoCombo = 31,
            /// <summary>For data is fetch from table and bind to control</summary>
            enmDataCombo = 32,
            /// <summary>For specificValue selection e.g.Approve,Cancel,etc.</summary>
            enmFixedCombo = 33,
        }

        // series is start from  51 and end with 60     
        /// <summary>enum is used to define Type Of Date</summary>
        public enum enmDateType
        {
            /// <summary>To select date in format 'dd/mm/yyyy'</summary>
            enmDate = 51,
            /// <summary>To select date in format dd/mm/yyyy hh:mm </summary>
            enmDateWithTime = 52,
        }

        // series is start from  61 and end with 70     
        /// <summary>enum is used to define Type Of Grid</summary>
        public enum GridType
        {
            enmGrid = 61,
            enmWithCheckBox = 62
        }

        #endregion

        public class SText : TextBox
        {
            #region Private Field
            private enmTextBoxType l_ValenmTextBoxType;
            private string _Name;
            private string _MapToTblFieldName;
            private bool _IsMandatory;
            private bool _AllowDuplicate;
            private bool _AllowSearch;
            private string _AllowedChars;
            private bool _AllowTochange;


            #endregion

            #region Properties
            /// <summary>Gets or sets the type of the control. </summary>

            public enmTextBoxType ValenmTextBoxType
            {
                get { return l_ValenmTextBoxType; }
                set { l_ValenmTextBoxType = value; }
            }
            /// <summary>Gets or sets the programmatic identifier assigned to the control. </summary>
            public string ID
            {
                get { return base.ID; }
                set { base.ID = value; }
            }
            /// <summary>Gets or sets the name of the control. </summary>
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            ///<summary> Gets or sets the text displayed when the mouse pointer hovers over the control</summary>            
            public string ToolTipText
            {
                get { return base.ToolTip; }
                set { ToolTip = value; }
            }

            ///<summary> Gets or sets the tablefield map to the control</summary>            
            public string MapToTblFieldName
            {
                get { return _MapToTblFieldName; }
                set { _MapToTblFieldName = value; }
            }

            /// <summary> Gets or sets the tab index of the control.</summary>
            public int TabIndex
            {
                get { return base.TabIndex; }
                set { base.TabIndex = Convert.ToInt16(value); }
            }
            ///<summary>Gets or sets the maximum number of characters allowed in the text box.</summary>
            public int MaxLength
            {
                get { return base.MaxLength; }
                set
                {
                    //   base.Width = value; 
                    if (l_ValenmTextBoxType != enmTextBoxType.enmPincCode && l_ValenmTextBoxType != enmTextBoxType.enmMobileNo && l_ValenmTextBoxType != enmTextBoxType.enmPhoneNo && l_ValenmTextBoxType != enmTextBoxType.enmVehicleNo && l_ValenmTextBoxType != enmTextBoxType.enmFaxNo)
                    {
                        base.MaxLength = value;
                    }
                }
            }

            public bool AllowTochange
            {
                get { return _AllowTochange; }
                set
                {
                    _AllowTochange = value;
                    // If data of this control allow to change then set it  true else set false
                    if (value == false)
                    {
                        base.Attributes.Add("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
                    }
                    else
                    {
                        base.Attributes.Remove("onkeydown");
                    }
                }
            }
            /// <summary>Gets or sets a value indicating whether the contents of the control can be changed.           
            ///<para>
            /// Returns:
            ///     true if the contents of the control cannot be changed;            
            ///     otherwise, false. The default value is false.
            ///</para>
            ///</summary>            
            public bool ReadOnly
            {
                get { return !base.Enabled; }
                set { base.Enabled = !value; }
            }
            public bool IsMandatory
            {
                get { return _IsMandatory; }
                set { _IsMandatory = value; }
            }
            public bool AllowDuplicate
            {
                get { return _AllowDuplicate; }
                set { _AllowDuplicate = value; }
            }
            public bool AllowSearch
            {
                get { return _AllowSearch; }
                set { _AllowSearch = value; }
            }
            public string AllowedChars
            {
                get { return _AllowedChars; }
                set { _AllowedChars = value; }
            }
            ///<summary>Gets or sets the maximum number of characters allowed in the text box.</summary>
            #endregion



            public SText(enmTextBoxType ValenmTextBoxType)
            {
                // set default Value of Control
                _IsMandatory = false;
                _AllowDuplicate = true;
                _AllowSearch = false;
                _AllowedChars = "";
                MaxLength = 15;
                _AllowTochange = false;
                l_ValenmTextBoxType = ValenmTextBoxType;
                Attributes["AutoPostBack"] = "true";
                Attributes.Add("runat", "server");
                Attributes.Add("onkeypress", "CheckForTextBoxValue(event,this," + (int)ValenmTextBoxType + ")");
                //for Lostfocus function
                Attributes.Add("onblur", "CheckForTextBoxLostFocus(event,this," + (int)ValenmTextBoxType + ")");
                Attributes["class"] = "TextBoxForString";
                if (ValenmTextBoxType == enmTextBoxType.enmString)
                {

                }
                else if (ValenmTextBoxType == enmTextBoxType.enmStringWithLowerCase)
                {

                }
                else if (ValenmTextBoxType == enmTextBoxType.enmStringWithProperCase)
                {
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmStringWithProperCase)
                {

                }
                else if (ValenmTextBoxType == enmTextBoxType.enmNumeric)
                {
                    base.Style["text-align"] = "right";
                    Attributes["class"] = "TextForAmount";
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmNumericWithDecimal)
                {
                    base.Style["text-align"] = "right";
                    Attributes["class"] = "TextBoxForString";
                    Attributes["class"] = "TextForAmount";

                }
                else if (ValenmTextBoxType == enmTextBoxType.enmEmail)
                {
                    base.MaxLength = 50;
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmPhoneNo)
                {
                    base.MaxLength = 10;
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmMobileNo)
                {
                    base.MaxLength = 13;
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmFaxNo)
                {
                    MaxLength = 13;
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmVehicleNo)
                {
                    base.MaxLength = 10;
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmPincCode)
                {
                    base.MaxLength = 6;
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmAlphaNumeric)
                {
                }
                else if (ValenmTextBoxType == enmTextBoxType.enmMemo)
                {
                }

            }
        }

        public class SComboBox : DropDownList
        {
            #region Private Field
            private enmComboBoxType l_ValenmComboBoxType;
            private string _Name;
            private string _MapToTblFieldName;
            private bool _IsMandatory;
            private bool _AllowDuplicate;
            private bool _AllowSearch;
            private string _QueryBindToControl;
            private string l_sComboValueField = "";
            private string l_sComboTextField = "";
            private bool _AllowTochange;
            private string _TblNameForFill;
            #endregion

            #region Properties
            /// <summary>Gets or sets the type of the control. </summary>
            public enmComboBoxType ValenmComboBoxType
            {
                get { return l_ValenmComboBoxType; }
                set { l_ValenmComboBoxType = value; }
            }

            /// <summary>Gets or sets the programmatic identifier assigned to the control. </summary>

            public string ID
            {
                get { return base.ID; }
                set { base.ID = value; }
            }

            /// <summary>Gets or sets the name of the control. </summary>
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            ///<summary> Gets or sets the text displayed when the mouse pointer hovers over the control</summary>            
            public string ToolTipText
            {
                get { return base.ToolTip; }
                set { base.ToolTip = value; }
            }
            ///<summary> Gets or sets the tablefield map to the control</summary>            
            public string MapToTblFieldName
            {
                get { return _MapToTblFieldName; }
                set { _MapToTblFieldName = value; }
            }
            /// <summary> Gets or sets the tab index of the control.</summary>
            public short TabIndex
            {
                get { return base.TabIndex; }
                set { base.TabIndex = value; }
            }

            /// <summary>Gets or sets a value indicating whether the contents of the control can be changed.           
            ///<para>
            /// Returns:
            ///     true if the contents of the control cannot be changed;            
            ///     otherwise, false. The default value is false.
            ///</para>
            ///</summary>                
            public bool ReadOnly
            {
                get { return !base.Enabled; }
                set
                {
                    if (value == true)
                        base.Enabled = false;
                    else
                    {
                        base.Enabled = true;
                    }
                }
            }
            public bool IsMandatory
            {
                get { return _IsMandatory; }
                set { _IsMandatory = value; }
            }
            public bool AllowDuplicate
            {
                get { return _AllowDuplicate; }
                set { _AllowDuplicate = value; }
            }
            public bool AllowSearch
            {
                get { return _AllowSearch; }
                set { _AllowSearch = value; }
            }
            /// <summary>
            /// Table name from which dat will be bind
            /// </summary>
            public string TblNameForFill
            {
                get { return _TblNameForFill; }
                set { _TblNameForFill = value; }
            }
            ///<summary>Gets or sets the maximum number of characters allowed in the text box.
            ///Also set width of the control
            /// </summary>
            public int MaxLength
            {
                set
                {
                    //   base.Width = value; 
                    if (value >= 150)
                    {
                        base.Width = value + 200;
                    }
                }
            }
            /// <summary>
            /// For YesNoCombo sets ""
            /// For dataCombo sets query which will execute bind data ro Control.
            /// For FixedCombo sets text with set separated by '#'.e.g.#Confirm#Cancel#Send#’.
            /// </summary>
            public string QueryBindToControl
            {
                get { return _QueryBindToControl; }
                set { _QueryBindToControl = value.ToUpper(); }
            }
            public bool AllowTochange
            {
                get { return _AllowTochange; }
                set { _AllowTochange = value; }
            }
            #endregion

            public SComboBox(enmComboBoxType ValenmComboBoxType)
            {

                // set default Value of Control
                _IsMandatory = false;
                _AllowDuplicate = false;
                _AllowSearch = false;
                CssClass = "ComboBoxFixedSize";
                _AllowTochange = false;
                l_ValenmComboBoxType = ValenmComboBoxType;
                Attributes.Add("runat", "server");
                Attributes["AutoPostBack"] = "true";
                Attributes.Add("onkeypress", "CheckForComboValue(event,this," + (int)ValenmComboBoxType + ")");
                EnableViewState = false;
                if (ValenmComboBoxType == enmComboBoxType.enmYesNoCombo)
                {
                }
                else if (ValenmComboBoxType == enmComboBoxType.enmDataCombo)
                {
                }
                else if (ValenmComboBoxType == enmComboBoxType.enmFixedCombo)
                {
                }
            }

            public void DataBind(clsGlobal.enmConnectionType valenmConnectionType)
            {
                // 'Replace Func.DB to objDB by Shyamal on 05042012
                clsDB objDB = new clsDB();
                try
                {
                    if (l_ValenmComboBoxType == enmComboBoxType.enmDataCombo)
                    {
                        if (_QueryBindToControl != "")
                        {
                            DataTable tmpDataTbl = null;
                            DataRow tmpDataRow;
                            DataSet ds = objDB.ExecuteQueryAndGetDataset(_QueryBindToControl);
                            if (ds != null)
                            {
                                if (ds.Tables[0] != null)
                                {
                                    tmpDataTbl = ds.Tables[0];
                                }
                            }
                            if (tmpDataTbl != null)
                            {
                                SetComboTextandValueField();
                                tmpDataRow = tmpDataTbl.NewRow();

                                tmpDataRow[l_sComboTextField] = "--Select--";
                                tmpDataRow[l_sComboValueField] = 0;
                                tmpDataTbl.Rows.InsertAt(tmpDataRow, 0);
                                base.DataSource = tmpDataTbl;
                                base.DataValueField = l_sComboValueField;
                                base.DataTextField = l_sComboTextField;
                                base.DataBind();


                            }
                            ds = null;
                            tmpDataTbl = null;
                        }
                    }
                    else if (l_ValenmComboBoxType == enmComboBoxType.enmFixedCombo)
                    {
                        ListItem lstitm;
                        string sOrgValue, sTemp1;
                        sOrgValue = _QueryBindToControl.Trim();
                        string sValue = "";
                        int iIndex = 1;
                        lstitm = new ListItem("--Select--", "0");
                        base.Items.Add(lstitm);
                        while (sOrgValue != "")
                        {
                            sOrgValue = sOrgValue.Substring(1);
                            if (sOrgValue.Contains('#') == true)
                            {
                                sValue = sOrgValue.Substring(0, sOrgValue.IndexOf('#'));
                                lstitm = new ListItem(sValue, iIndex.ToString());
                                Items.Add(lstitm);
                            }
                            sOrgValue = sOrgValue.Substring(sOrgValue.IndexOf('#'));
                            iIndex = iIndex + 1;
                            if (sOrgValue == "#")
                            {
                                return;
                            }
                        }
                    }
                    else if (l_ValenmComboBoxType == enmComboBoxType.enmYesNoCombo)
                    {
                        ListItem lstitm = new ListItem("--Select--", "0");
                        base.Items.Add(lstitm);
                        lstitm = new ListItem("Y", "1");
                        base.Items.Add(lstitm);
                        lstitm = new ListItem("N", "2");
                        base.Items.Add(lstitm);
                        base.SelectedIndex = 0;
                        lstitm = null;
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
            private void SetComboTextandValueField()
            {
                //string sTemp = "";
                //string[] sArrTemp = new string[2];
                //sArrTemp[0] = "";
                //sArrTemp[1] = "";
                try
                {
                    //sTemp = _QueryBindToControl.Substring(0, _QueryBindToControl.IndexOf("FROM")).Trim();
                    //sTemp = sTemp.Substring(sTemp.IndexOf(' '));
                    //sArrTemp = sTemp.Split(',');
                    l_sComboValueField = "ID";
                    l_sComboTextField = "Name";
                }
                catch
                {
                }
            }

        }

        public class SDate : TextBox
        {
            #region Private Field
            private enmDateType l_ValenmDateType;
            private string _Name;
            private string _MapToTblFieldName;
            private bool _IsMandatory;
            private bool _AllowDuplicate;
            private bool _AllowSearch;
            private string _AllowedChars;
            private bool _AllowTochange;

            #endregion

            #region Properties


            /// <summary>Gets or sets the programmatic identifier assigned to the control. </summary>
            public string ID
            {
                get { return base.ID; }
                set
                {
                    base.ID = value;
                }
            }
            /// <summary>Gets or sets the name of the control. </summary>
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
            public enmDateType ValenmDateType
            {
                get { return l_ValenmDateType; }
                set { l_ValenmDateType = value; }
            }

            ///<summary> Gets or sets the text displayed when the mouse pointer hovers over the control</summary>            
            public string ToolTipText
            {
                get { return base.ToolTip; }
                set { ToolTip = value; }
            }

            ///<summary> Gets or sets the tablefield map to the control</summary>            
            public string MapToTblFieldName
            {
                get { return _MapToTblFieldName; }
                set { _MapToTblFieldName = value; }
            }

            /// <summary> Gets or sets the tab index of the control.</summary>
            public int TabIndex
            {
                get { return base.TabIndex; }
                set { base.TabIndex = Convert.ToInt16(value); }
            }
            ///<summary>Gets or sets the maximum number of characters allowed in the text box.</summary>
            public int MaxLength
            {
                get { return base.MaxLength; }
                set
                {

                    base.MaxLength = value;

                }
            }
            public bool AllowTochange
            {
                get { return _AllowTochange; }
                set { _AllowTochange = value; }
            }
            /// <summary>Gets or sets a value indicating whether the contents of the control can be changed.           
            ///<para>
            /// Returns:
            ///     true if the contents of the control cannot be changed;            
            ///     otherwise, false. The default value is false.
            ///</para>
            ///</summary>            
            public bool ReadOnly
            {
                get { return !base.Enabled; }
                set { base.Enabled = !value; }
            }
            public bool IsMandatory
            {
                get { return _IsMandatory; }
                set { _IsMandatory = value; }
            }
            public bool AllowDuplicate
            {
                get { return _AllowDuplicate; }
                set { _AllowDuplicate = value; }
            }
            public bool AllowSearch
            {
                get { return _AllowSearch; }
                set { _AllowSearch = value; }
            }
            public string AllowedChars
            {
                get { return _AllowedChars; }
                set { _AllowedChars = value; }
            }
            public string Text
            {
                get
                {

                    if (ValenmDateType == enmDateType.enmDate)
                    {
                        return Func.Convert.tConvertToDate(base.Text, true);
                    }
                    else if (ValenmDateType == enmDateType.enmDateWithTime)
                    {
                        return Func.Convert.tConvertToDate(base.Text, false);
                    }
                    return DateTime.Now.ToString();
                }
                set
                {
                    base.Text = value;
                }
            }
            #endregion
            public SDate(enmDateType ValenmDateType)
            {
                // set default Value of Control
                l_ValenmDateType = ValenmDateType;
                _IsMandatory = false;
                _AllowDuplicate = false;
                _AllowSearch = false;
                _AllowedChars = "";
                MaxLength = 15;
                _AllowTochange = false;
                Attributes["class"] = "TextBoxForString";
                Attributes["AutoPostBack"] = "true";
                Attributes.Add("runat", "server");

                Attributes.Add("onkeypress", "CheckForDateValue(event,this," + (int)ValenmDateType + ")");
                Attributes.Add("onblur", "CheckForDateValue(event,this," + (int)ValenmDateType + ")");




            }

            protected override void Render(HtmlTextWriter output)
            {
                System.Web.UI.WebControls.Image imgforDate = new System.Web.UI.WebControls.Image();
                imgforDate.ImageUrl = @"~\Images\DateImages\calendar.gif";
                string sTxtboxID = base.UniqueID;
                sTxtboxID = sTxtboxID.Replace('$', '_');
                if (ValenmDateType == SControl.enmDateType.enmDate)
                {
                    imgforDate.Attributes.Add("onclick", "DateFormat('" + sTxtboxID + "','ddmmyyyy')");
                }
                else
                {
                    imgforDate.Attributes.Add("onclick", "DateFormat('" + sTxtboxID + "','ddmmyyyy','dropdown',true)");
                }
                imgforDate.Attributes["class"] = "DateImage";
                base.Controls.AddAt(0, imgforDate);
                output.WriteBeginTag("table");
                output.Write(HtmlTextWriter.TagRightChar);

                output.WriteBeginTag("tr");
                output.Write(HtmlTextWriter.TagRightChar);

                output.WriteBeginTag("td");
                output.Write(HtmlTextWriter.TagRightChar);
                base.Render(output);
                output.WriteEndTag("td");

                output.WriteBeginTag("td");
                output.Write(HtmlTextWriter.TagRightChar);
                base.RenderChildren(output);
                output.WriteEndTag("td");
                output.WriteEndTag("tr");
                output.WriteEndTag("table");
            }
        }

        public class SGrid : GridView
        {
            private GridType l_enmGridType;
            private string _MapToTblFieldName;
            private string _TblNameForFill;
            private string _QueryBindToControl;

            public GridType ValenmGridType
            {
                get { return l_enmGridType; }
                set { l_enmGridType = value; }
            }
            ///<summary> Gets or sets the tablefield map to the control</summary>            
            public string MapToTblFieldName
            {
                get { return _MapToTblFieldName; }
                set { _MapToTblFieldName = value; }
            }
            /// <summary>
            /// Table name from which dat will be bind
            /// </summary>
            public string TblNameForFill
            {
                get { return _TblNameForFill; }
                set { _TblNameForFill = value; }
            }
            /// <summary>
            /// query which will execute bind data ro grid.    
            /// </summary>
            public string QueryBindToControl
            {
                get { return _QueryBindToControl; }
                set { _QueryBindToControl = value.ToUpper(); }
            }
            public SGrid(GridType ValenmGridType)
            {
                l_enmGridType = ValenmGridType;
                base.BackColor = Color.White;
                base.BorderColor = Color.FromArgb(33, 66, 66);
                base.BorderStyle = BorderStyle.Double;
                base.Width = Unit.Percentage(100);
                base.BorderWidth = 5;
                base.AllowPaging = false;
                base.FooterStyle.CssClass = "GridViewFooterStyle";
                base.RowStyle.CssClass = "GridViewRowStyle";
                base.SelectedRowStyle.CssClass = "GridViewSelectedRowStyle";
                base.PagerStyle.CssClass = "GridViewPagerStyle";
                base.AlternatingRowStyle.CssClass = "GridViewAlternatingRowStyle";
                base.HeaderStyle.CssClass = "GridViewHeaderStyle";
                base.AutoGenerateColumns = true;
                //Attributes["OnLoad"] = "ForTest();";                
                base.EnableViewState = true;
            }
            public void BindDataToGrid()
            {
                if (_QueryBindToControl != null)
                {
                    // 'Replace Func.DB to objDB by Shyamal on 05042012
                    DataSet dsGridDetails = null;
                    clsDB objDB = new clsDB();
                    try
                    {
                        dsGridDetails = objDB.ExecuteQueryAndGetDataset(_QueryBindToControl);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (objDB != null) objDB = null;
                    }
                    base.DataSource = null;
                    base.DataSource = dsGridDetails.Tables[0];
                    try
                    {
                        base.DataBind();
                    }
                    catch
                    { }
                }
                else
                {
                    return;
                }

                if (ValenmGridType == GridType.enmWithCheckBox)
                {

                    CheckBox Chk = new CheckBox();
                    TextBox txt = new TextBox();
                    TableCell Cell = new TableCell();
                    //Add Header Cell to select All Checkbox
                    Cell = new TableCell();
                    Cell.Controls.Add(Chk);
                    Chk.ID = "ChkAll";
                    //Chk.Text = "Select All";
                    Chk.Attributes["AutoPostBack"] = "true";
                    Chk.Attributes.Add("runat", "server");
                    Chk.Attributes["onClick"] = "SelectAllCheckboxes(this)";

                    HeaderRow.Cells[0].Visible = false;
                    Cell.CssClass = "GridViewHeaderMyStyle";
                    HeaderRow.Cells.AddAt(0, Cell);
                    //Add Header Cell to set status of record N For new, D for Delete, E for Allready Exit 
                    Cell = new TableCell();
                    Cell.Text = "";
                    Cell.EnableViewState = true;
                    HeaderRow.Cells.AddAt(1, Cell);
                    HeaderRow.Cells[1].Width = 1;
                    HeaderRow.Cells[1].Style.Add("display", "none");
                    for (int j = 0; j <= Rows.Count - 1; j++)//Add row wise Cell
                    {
                        Rows[j].Cells[0].Visible = false; //to hide Id field
                        //Rows[j].Cells[1].Visible = false; //to hide Id Record_Used
                        Chk = new CheckBox();
                        Chk.ID = "gridChk" + j.ToString();
                        Chk.Attributes["AutoPostBack"] = "true";
                        Chk.Attributes.Add("runat", "server");
                        Chk.Attributes["onClick"] = "SelectCheckbox(this)";
                        Chk.EnableViewState = true;
                        Cell = new TableCell();
                        Cell.Controls.Add(Chk);
                        Cell.Width = Unit.Percentage(3);
                        Rows[j].Cells.AddAt(0, Cell);
                        //                        
                        txt = new TextBox();
                        txt.Width = 0;
                        txt.ID = "gridtxt" + j.ToString();
                        txt.Attributes["AutoPostBack"] = "true";
                        txt.Attributes.Add("runat", "server");
                        txt.EnableViewState = true;
                        txt.Text = "";
                        Cell = new TableCell();
                        Cell.Width = 0;
                        Cell.Controls.Add(txt);
                        Rows[j].Cells.AddAt(1, Cell);
                        Rows[j].Cells[1].Style.Add("display", "none");
                    }

                }
            }

            public void MarkForSelection(DataTable dt)
            {
                CheckBox Chk = new CheckBox();
                TextBox txt = new TextBox();
                int iDtRowCnt = 0;
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        for (int iRowCnt = 0; iRowCnt < base.Rows.Count; iRowCnt++)
                        {
                            Chk = (CheckBox)this.Rows[iRowCnt].FindControl("gridChk" + iRowCnt.ToString());
                            txt = (TextBox)this.Rows[iRowCnt].FindControl("gridtxt" + iRowCnt.ToString());
                            Chk.Checked = false;
                            this.Rows[iRowCnt].BackColor = Color.White;
                            txt.Text = "";

                            DataRow[] foundRows;
                            foundRows = dt.Select(" Convert(" + dt.Columns[1].Caption + ", 'System.String')  like '" + this.Rows[iRowCnt].Cells[2].Text + "'");
                            if (foundRows.Length > 0)
                            {
                                //if (this.Rows[iRowCnt].Cells[2].Text == Func.Convert.sConvertToString(dt.Rows[iDtRowCnt][1]))
                                //{
                                Chk.Checked = true;
                                if (Func.Convert.sConvertToString(dt.Rows[iDtRowCnt]["Record_used"]).ToUpper() == "Y")
                                {
                                    this.Rows[iRowCnt].Enabled = false;
                                    txt.Text = "E";
                                }
                                else
                                {
                                    this.Rows[iRowCnt].BackColor = Color.Wheat;
                                    this.Rows[iRowCnt].Enabled = true;
                                    txt.Text = "E";
                                }
                                iDtRowCnt++;
                                if (iDtRowCnt == dt.Rows.Count)
                                {
                                    for (iRowCnt = iRowCnt + 1; iRowCnt < base.Rows.Count; iRowCnt++)
                                    {
                                        Chk = (CheckBox)this.Rows[iRowCnt].FindControl("gridChk" + iRowCnt.ToString());
                                        Chk.Checked = false;
                                        txt = (TextBox)this.Rows[iRowCnt].FindControl("gridtxt" + iRowCnt.ToString());
                                        txt.Text = "";
                                        this.Rows[iRowCnt].BackColor = Color.White;
                                        this.Rows[iRowCnt].Enabled = true;
                                    }
                                    return;
                                }
                                //}
                                //else
                                //{
                                //    Chk.Checked = false;
                                //    this.Rows[iRowCnt].BackColor = Color.White;
                                //    txt.Text = "";
                                //}

                            }
                            else
                            {
                                Chk.Checked = false;
                                this.Rows[iRowCnt].BackColor = Color.White;
                                txt.Text = "";
                            }


                        }
                    }
                    else
                    {

                        for (int iRowCnt = 0; iRowCnt < base.Rows.Count; iRowCnt++)
                        {
                            Chk = (CheckBox)this.Rows[iRowCnt].FindControl("gridChk" + iRowCnt.ToString());
                            Chk.Checked = false;
                            txt = (TextBox)this.Rows[iRowCnt].FindControl("gridtxt" + iRowCnt.ToString());
                            txt.Text = "";
                            this.Rows[iRowCnt].BackColor = Color.White;
                            this.Rows[iRowCnt].Enabled = true;
                        }
                    }
                }
            }

        }
        public class SButton : Button
        {
            private string _ParentControlID;
            private string _QueryToExecute;

            public string QueryToExecute
            {
                get { return _QueryToExecute; }
                set { _QueryToExecute = value; }
            }
            public string ParentControlID
            {
                get { return _ParentControlID; }
                set { _ParentControlID = value; }
            }
            public SButton()
            {
                base.CssClass = "CommandButton";
            }

        }
    }
}
