<%@ Page Title="dCAN-Parts SRN" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false"
    Theme="SkinFile" CodeBehind="frmSalesRetunNote.aspx.cs" Inherits="MANART.Forms.Spares.frmSalesRetunNote" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFPFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsParts_SRN.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtSalesRetNoteDate = document.getElementById("ContentPlaceHolder1_txtSalesRetNoteDate_txtDocDate");
            $('#ContentPlaceHolder1_txtSalesRetNoteDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtSalesRetNoteDate.value == '') ? '0d' : txtSalesRetNoteDate.value, maxDate: (txtSalesRetNoteDate.value == '') ? '0d' : txtSalesRetNoteDate.value
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
        //function FormValidation() {
        //    var errMessage = "";
        //    if (document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName").value == "0") {
        //        errMessage += "*Please Select Customer Name.\n";
        //    }
        //    if (errMessage != "") {
        //        alert(errMessage);
        //        return false;
        //    }
        //    else {
        //        return true;
        //    }
        //}


        //To Show Part Master
        function ShowINVMultiPartSearch(objNewPartLabel, sDealerId, objCustTypeID, iHOBrId, sIsOpenSRN) {
            //debugger;
            var PartDetailsValue;
            //var sSelectedPartID = "";

            var objCustType = document.getElementById("ContentPlaceHolder1_" + objCustTypeID);
            var sCustType = objCustType.options[objCustType.selectedIndex].text;
            var sCustID = objCustType[objCustType.selectedIndex].value;

            if (sCustID == "0") {
                alert("Please Select Customer.!");
                return false;
            }
            if (sIsOpenSRN == "Y") {
                var txtPrevOpenSrnNo = document.getElementById("ContentPlaceHolder1_txtPrevOpenSrnNo").value;
                alert(" SRN No " + " " + txtPrevOpenSrnNo + " is open for this customer. Pl. confirm it first.");
                return false;
            }
            var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
            var sCustTaxTag = hdnCustTaxTag.value;
            var hdnIsDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
            var sDocGST = hdnIsDocGST.value;
            //sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            var sSelPartIDInvNoPONo = GetPrevSelPartIDInvNoPONo(objNewPartLabel);

            PartDetailsValue = window.showModalDialog("frmSelMultiPartWithInv.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelPartIDInvNoPONo + "&CustID=" + sCustID + "&HOBR_ID=" + iHOBrId + "&sDocGST=" + sDocGST + "&CustTaxTag=" + sCustTaxTag, "List", "dialogHeight: 560px; dialogWidth: 950px;");
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



    </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Sales Return Note"> </asp:Label>
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
                    <%--<asp:Label runat ="server" ID="lblEGPDealer" Text ="EGP Dealer Name" class="tdLabel"></asp:Label>
                    <asp:DropDownList ID="ddlEGPDealer" runat="server" CssClass ="ComboBoxFixedSize" Width ="30%" ></asp:DropDownList>--%>
                    <uc2:Location ID="Location" runat="server" />
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
                        SuppressPostBack="true" CollapsedText="Sales Return Note Details" ExpandedText="Sales Return Note Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
                                <td class="tdLabel" style="width: 15%">Customer:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpCustomer" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMCustomer" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblSalesRetNoteNo" runat="server" Text="Sales Return Note No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtSalesRetNoteNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblSalesRetNoteDate" runat="server" Text="Sales Return Note Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtSalesRetNoteDate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" colspan="2">
                                    <div>
                                        <asp:RadioButtonList ID="rbtLstDiscount" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="rbtLstDiscount_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Discount In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Discount In Amount" Value="Amt"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <td class="tdLabel" style="width: 15%"></td>
                                <td style="width: 18%"></td>
                                <td class="tdLabel" style="width: 15%"></td>
                                <td style="width: 18%"></td>
                            </tr>
                        </table>
                    </asp:Panel>
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
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <%--<div style="overflow: hidden;" id="DivHeaderRow">
                            </div>
                            <div style="overflow: scroll; background-color: #D4D4D4;" onscroll="OnScrollDiv(this,DivHeaderRow)" id="DivMainContent">--%>
                        
                            <div class="scrolling-table-container" style="height: 250px; background-color: #D4D4D4;">
                            <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                                Width="120%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnPageIndexChanging="PartGrid_PageIndexChanging"
                                OnRowDataBound="PartGrid_RowDataBound">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" />
                                            <%--Text='<%# Container.DataItemIndex + 1  %>' OnClientClick="return FormValidation();"--%>
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
                                    <asp:TemplateField HeaderText="Part No." ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:LinkButton ID="lnkSelectPart" runat="server" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="9%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Invoice_Qty","{0:#0}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revised Invoice Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOrgInvQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Rev_Inv_Qty","{0:#0}") %>'
                                                Width="90%" onkeypress=" return CheckForTextBoxValue(event,this,'5');"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Return Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReturnQty" runat="server" CssClass="GridTextBoxForAmount" Width="90%" MaxLength="6"
                                                Text='<%# Eval("Ret_Qty","{0:#0}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateSRNPartTotal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Stock" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStockQty" runat="server" CssClass="GridTextBoxForAmount"
                                                Width="90%" Text='<%# Eval("Stock_Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnit" runat="server" Text='<%# Eval("Unit") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" />
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
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disc(Per)" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Disc_Per","{0:#0.00}") %>'
                                                onblur="return CalculateSRNPartTotal(event,this);" onkeypress=" return CheckForTextBoxValue(event,this,'6');"
                                                Width="90%" MaxLength="5"></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("disc_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <%--Vikram on 29082017--%>
                                    <%--<asp:TemplateField HeaderText="Discounted Rate" ItemStyle-Width="6%">--%>
                                    <asp:TemplateField HeaderText="Taxable Price" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("disc_rate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                            <asp:TextBox ID="txtExclDiscountRate" runat="server" CssClass="HideControl" Width="90%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="TxtExclTotal" runat="server" CssClass="HideControl" Width="80%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Tax" ItemStyle-Width="7%">
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
                                            <asp:TextBox ID="txtPartTaxPer" runat="server" CssClass="HideControl" Width="80%"></asp:TextBox>
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
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxAndCalcSRN(event,this);" />
                                            <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatus" runat="server" Width="1%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LabTag" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtlabtag" runat="server" Width="1%" Text='<%# Eval("lab_tag") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Group" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGrNo" runat="server" Width="80%" Text='<%# Eval("group_code") %>'
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>

                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                        <%--<div id="DivFooterRow" style="overflow: hidden">
                        </div>--%>
                    </asp:Panel>

                    <asp:Panel ID="PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
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
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
                                CssClass="table table-condensed table-bordered">
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
                                    <asp:TemplateField HeaderText="Taxable Inv Amt" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <%--<asp:TextBox ID="txtGrnetrevamt" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount HideControl" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                MaxLength="6"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
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

                    <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                            ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                            ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Receipt Tax Details" ExpandedText="Receipt Tax Details"
                            TextLabelID="lblTtlTaxDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlTaxDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Receipt Tax Details" Width="96%"
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
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered">
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
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
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
                                    <asp:TemplateField HeaderText="PF Charges%" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                                Width="90%" MaxLength="5"></asp:TextBox>
                                            <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PF Charges Amt" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                                Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                            <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>

                    <table id="tblTotal" runat="server" class="ContainTable" width="100%" style="display: none">
                        <tr>
                            <td style="width: 55%; text-align: right">
                                <b>Total:</b>
                            </td>
                            <td style="width: 9%; text-align: left">
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                            <td style="width: 22%; text-align: left">
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
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
                <asp:HiddenField ID="hdnDealerID" runat="server" Value="0" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsOpenSRN" runat="server" Value="N" />
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="" />
                <asp:TextBox ID="txtPrevOpenSrnNo" CssClass="HideControl" runat="server" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
