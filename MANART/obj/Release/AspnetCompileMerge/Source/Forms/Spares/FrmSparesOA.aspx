<%@ Page Title="dCAN-Parts OA" Language="C#" MasterPageFile="~/Header.Master" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Theme="SkinFile" AutoEventWireup="true" CodeBehind="FrmSparesOA.aspx.cs" Inherits="MANART.Forms.Spares.FrmSparesOA" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFPFunction.js"></script>
    <script src="../../Scripts/jsOA.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <%--<script src="../../Scripts/jsPartSpareScheme.js"></script>--%>
    <style type="text/css">
        input[type="radio"] {
            margin: 4px 0 0;
            margin-top: 1px \9;
            line-height: normal;
            margin-left: -310px;
        }

        label {
            color: #303c49;
            float: left;
            font-size: 12px;
            font-weight: normal;
            width: 203px;
            padding-left: 30px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var txtOADate = document.getElementById("ContentPlaceHolder1_txtOADate_txtDocDate");
            $('#ContentPlaceHolder1_txtOADate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtOADate.value == '') ? '0d' : txtOADate.value, maxDate: (txtOADate.value == '') ? '0d' : txtOADate.value
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var txtValidDate = document.getElementById("ContentPlaceHolder1_txtValidDate_txtDocDate");
            $('#ContentPlaceHolder1_txtValidDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtValidDate.value == '') ? '0d' : txtValidDate.value
            });
        });
    </script>
    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                event.returnValue = false;
                alert(message);
                return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }
        // Function Validation
        function Validation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_txtOANo").value == "") {
                errMessage += "*Please Do not Blank Invoice No.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtOADate_txtDocDate").value == "") {
                errMessage += "*Please Do not Blank Invoice Date.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_DrpCustomer").value == "0") {
                errMessage += "*Please Select Customer.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_ddlGST_OA_Type").value == "0") {
                errMessage += "*Please Select Rate Type.\n";
            }
            if (errMessage != "") {
                alert(errMessage);
                return false;
            }
            else {
                return true;
            }
        }

        //To Show Part Master
        function ShowSpWPFPart_New(objNewPartLabel, sDealerId, objCust, iHOBr_id, ObjOAType) {
            //debugger;
            if (Validation() == false) {
                return false;
            }
            var PartDetailsValue;
            var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            var ParentCtrlID;
            var objCustomer;
            objCustomer = document.getElementById("ContentPlaceHolder1_" + objCust);
            var sCustID = objCustomer[objCustomer.selectedIndex].value;
            if (sCustID == "" || sCustID == "0") {
                alert('Please select the Customer.');
                return false;
            }
            var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
            var sCustTaxTag = hdnCustTaxTag.value;
            var hdnIsDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
            var sDocGST = hdnIsDocGST.value;
            var objGSTOAType;
            objGSTOAType = document.getElementById("ContentPlaceHolder1_" + ObjOAType);
            var sGSTOATypeID = objGSTOAType[objGSTOAType.selectedIndex].value;
            var sGSTOAType;
            if (sGSTOATypeID == "" || sGSTOATypeID == "0") {
                alert('Please select the Rate Type.');
                return false;
            }
            if (sGSTOATypeID == 1) {
                sGSTOAType = "O";
            }
            else {
                sGSTOAType = "N";
            }
                

            PartDetailsValue = window.showModalDialog("frmSelectMultiPartSparesOA.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&sCustID=" + sCustID + "&HOBR_ID=" + iHOBr_id + "&sDocGST=" + sDocGST + "&CustTaxTag=" + sCustTaxTag + "&GSTOAType=" + sGSTOAType, "List", "dialogHeight: 550px; dialogWidth: 900px;");// "scrollbars=no,resizable=no,dialogWidth=500px,dialogHeight=700px");
            return true;
        }

        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');

            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }
        function refresh() {
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }

        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            if (Validation() == false) {
                return false;
            }
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var filename = objFileUpload.value;
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search('SparesOA_PartDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }
        }
        function SelectDeletCheckboxAndCalc(ObjChkDelete) {
            //debugger;
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    CalulateOAPartGranTotal();
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    CalulateOAPartGranTotal();
                    return false;
                }
            }
            else {
                ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                CalulateOAPartGranTotal();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts OA"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc1:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:Location ID="Location" runat="server"
                        OnDDLSelectedIndexChanged="Location_DDLSelectedIndexChanged" />
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />

                </asp:Panel>
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Parts OA Details" ExpandedText="Parts OA Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Document Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDocDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblPONo" runat="server" Text="OA No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtOANo" runat="server" CssClass="TextBoxForString NonEditableFields" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblPODate" runat="server" Text="OA Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtOADate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>

                                <td class="tdLabel" style="width: 15%">Customer:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpCustomer" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel" style="width: 15%">OA Validity Date:
                                </td>
                                <td style="width: 18%">
                                    <%--<uc3:CurrentDate ID="txtValidDate" runat="server" bCheckforCurrentDate="true" />--%>
                                    <uc3:CurrentDate ID="txtValidDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" Visible="True" />
                                </td>
                            </tr>
                            <tr>

                                <td class="tdLabel" style="width: 15%">Narration:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtNarration" Text="" runat="server" CssClass="TextBoxForString" MaxLength="100" TextMode="MultiLine"
                                        onblur="checkTextAreaMaxLength(this,event,'100');" Rows="2"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Refernce :
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRefernce" Text="" runat="server" CssClass="TextBoxForString" MaxLength="50" TextMode="MultiLine"
                                        onblur="checkTextAreaMaxLength(this,event,'50');" Rows="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" colspan="2">
                                    <div>
                                        <asp:RadioButtonList ID="rbtLstDiscount" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="rbtLstDiscount_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Freight In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Freight In Amount" Value="Amt"></asp:ListItem>
                                            <%-- <asp:ListItem Text=" Discount In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Discount In Amount" Value="Amt"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <td class="tdLabel" style="width: 15%">Rate Type: 
                                    <%--Scheme:--%>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlGST_OA_Type" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="ddlGST_OA_Type_SelectedIndexChanged">
                                        <%--<asp:ListItem Selected="True" Value="N">Post GST</asp:ListItem>
                                        <asp:ListItem Value="O">Pre GST</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <%--<asp:CheckBox ID="ChkScheme" runat="server" Text="Scheme" />--%>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="UploafFile" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    Style="display: none">
                    <table id="Table1" runat="server" class=" ContainTable table table-bordered">
                        <tr>
                            <td style="width: 15%; padding-left: 10px;">Select File For Upload
                                
                            </td>
                            <td style="width: 65%; padding-left: 10px;">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="75%" CssClass="Cntrl1" />
                            </td>
                            <td style="width: 10%;">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-search " Text="Upload"
                                    OnClientClick="return CheckBeforeUploadClick(this,'FileUpload1');" OnClick="btnUpload_Click" />
                                <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <img src="../../Images/Search_Progress.gif" alt="Searchig" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px; color: Red" colspan="4">
                                <a>Please select the file in the excel format. File name should be in format as 'DealerCode_SparesOA_PartDetails_Datestamp'
                                    <br />
                                    e.g. 'D016514_SparesOA_PartDetails_29112016'. </a>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 10px;">
                                <br />
                                <asp:Label ID="lblListPartNo" runat="server" Text="" Width="100%" ForeColor="#49A3D3"
                                    Visible="false"> </asp:Label>
                                <asp:TextBox ID="txtListPartNo" TextMode="MultiLine" runat="server" Text="" Width="100%"
                                    ForeColor="#49A3D3" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="true"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Part Details" ExpandedText="Part Details"
                        TextLabelID="lblTtlPartDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="Vertical">

                        <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-bordered table-condensed"
                            OnPageIndexChanging="PartGrid_PageIndexChanging"
                            OnRowDataBound="PartGrid_RowDataBound">
                            <%--OnRowCommand="PartGrid_RowCommand"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server"  />
                                        <%--Text='<%# Container.DataItemIndex + 1  %>'--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartID" runat="server" Width="5%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblSelectPart" runat="server" ForeColor="#49A3D3" Text="Select Part"
                                            onmouseover="SetCancelStyleonMouseOver(this);" ToolTip="Select Part" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>--%>
                                        <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:LinkButton ID="lnkSelectPart" runat="server" CssClass="btn btn-link" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Group" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrNo" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("group_code") %>'
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OA Qty" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Qty","{0:#0.00}") %>'
                                            MaxLength="6" Width="90%" onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalculateOAPartTotal(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Stock" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalQty" runat="server" CssClass="GridTextBoxForAmount"
                                            Width="90%" Text='<%# Eval("bal_qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUnit" runat="server" Text='<%# Eval("Unit") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MRP" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("Price","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <%--Vikram on 29082017--%>
                                <%--<asp:TemplateField HeaderText="Rate" ItemStyle-Width="6%">--%>
                                <asp:TemplateField HeaderText="Selling List Price" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("MRPRate","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Per" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscountPer" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            MaxLength="5" Text='<%# Eval("discount_per","{0:#0.00}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalculateOAPartTotal(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscountAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("discount_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <%--Vikram on 29082017--%>
                                <%--<asp:TemplateField HeaderText="Discounted Rate" ItemStyle-Width="5%">--%>
                                <asp:TemplateField HeaderText="Taxable Price" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscountRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("disc_rate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        <asp:TextBox ID="txtExclDiscountRate" runat="server" CssClass="HideControl" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:TextBox ID="TxtExclTotal" runat="server" CssClass="HideControl" Width="80%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Tax" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpPartTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%"
                                            OnSelectedIndexChanged="drpPartTax_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DrpPartTax1" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DrpPartTax2" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>

                                        <asp:DropDownList ID="drpPartTaxPer" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtPartTaxPer" runat="server" CssClass="HideControl" Width="80%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--Sujata 05092014_Begin added tax1 and tax2 revert calculation--%>
                                        <asp:DropDownList ID="DrpPartTax1Per" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtPartTax1Per" runat="server" CssClass="HideControl" Width="80%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                        <asp:DropDownList ID="DrpPartTax2Per" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtPartTax2Per" runat="server" CssClass="HideControl" Width="80%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--Sujata 05092014_End--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxAndCalc(this);" />
                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BFRGST" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBFRGST" runat="server" Width="96%" Text='<%# Eval("BFRGST") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pre GST Stock" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBFRGST_Stock" runat="server" Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                            Text='<%# Eval("BFRGST_Stock","{0:#0.00}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>

                    <table id="tblTotal" runat="server" class="ContainTable" width="100%">
                        <tr>
                            <td style="width: 55%; text-align: right">
                                <%--<b>Total:</b>--%>
                            </td>
                            <td style="width: 9%; text-align: left">
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="HideControl" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                            <td style="width: 22%; text-align: left">
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="HideControl" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntPartGroupDetails"
                        ExpandControlID="TtlPartGroupDetails" CollapseControlID="TtlPartGroupDetails" Collapsed="true"
                        ImageControlID="ImgTtlPartGroupDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Group Tax Details" ExpandedText="Group Tax Details"
                        TextLabelID="lblTtlPartGroupDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartGroupDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartGroupDetails" runat="server" Text="Group Tax Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartGroupDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                            OnRowCommand="GrdPartGroup_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group Code" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGRPID" runat="server" Width="96%" Text='<%# Eval("group_code") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMGrName" runat="server" Text='<%# Eval("Gr_Name") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Net Taxable OA Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%-- <asp:TextBox ID="txtGrnetrevamt" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount " Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            MaxLength="5"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateOAPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTaxPer" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="drpTaxTag" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTaxTag" runat="server" Text='<%# Eval("Tax_Tag") %>' CssClass="GridTextBoxForString" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTaxPer" runat="server" Text='<%# Eval("TAX_Percentage","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrTaxAmt" runat="server" Text='<%# Eval("tax_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTax1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax1 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTaxPer1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                        <%--Sujata 22092014_Begin--%>
                                        <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--Sujata 22092014_End--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax1 %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax1Per" runat="server" Text='<%# Eval("Tax1_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax1 Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrTax1Amt" runat="server" Text='<%# Eval("tax1_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTax2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                        <%--Sujata 22092014_Begin--%>
                                        <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--Sujata 22092014_End--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax2 %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax2Per" runat="server" Text='<%# Eval("Tax2_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax2 Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrTax2Amt" runat="server" Text='<%# Eval("tax2_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTaxTot" runat="server" Width="90%" Text='<%# Eval("Total","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                        ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                        ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="OA Tax Details" ExpandedText="OA Tax Details"
                        TextLabelID="lblTtlTaxDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlTaxDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlTaxDetails" runat="server" Text="OA Tax Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlTaxDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="GrdDocTaxDet" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                            OnRowCommand="GrdPartGroup_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocID" runat="server" Width="5%" Text='<%# Eval("ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable Total" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocTotal" runat="server" Text='<%# Eval("net_tr_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtDocRevTotal" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForAmount HideControl" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocDisc" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Before Tax Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBeforeTax" runat="server" Text='<%# Eval("before_tax_amt") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LST Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("mst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CST Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("cst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax 1" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax1" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("surcharge_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax 2" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax2" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("tot_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Freight %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFPer" runat="server" MaxLength="5" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                            Width="90%"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateOAPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Freight Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateOAPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight Tax%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFTaxPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("PF_Tax_Per","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight IGST Amt" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFIGSTorSGSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("PF_IGSTorSGST_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight CGST Tax%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFTaxPer1" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("PF_Tax_Per1","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight CGST Amt" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPfCGSTrAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("PF_CGST_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("oa_tot","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="N" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="" />
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnGSTOAType" runat="server" Value="N" />
            </td>
        </tr>
    </table>
</asp:Content>
