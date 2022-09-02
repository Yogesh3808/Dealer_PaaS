<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSelMultiPartForPRN.aspx.cs"
    Theme="SkinFile" Inherits="MANART.Forms.Spares.frmSelMultiPartForPRN" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.3.2.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>

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
        window.onload = function () {
            FirstTimeGridDisplay('');
        }
        function CloseMe() {
            window.close();
        }

        // New Fuction for select Part
        function ChkRNPartClick(objImgControl) {
            //debugger;
            var objID = $('#' + objImgControl.id);
            var objCol = objID[0].parentNode.parentNode;
            var txtparst = document.getElementById('txtPartIds');

            var ArrOfPartDtls = '';
            var removePartID;

            for (i = 1; i < objCol.cells.length; i++) {
                if (i == objCol.cells.length - 1)
                    ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML;
                else
                    ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML + '<--';
            }

            if (objImgControl.checked == true) {
                if (txtparst.value == "") {
                    txtparst.value = ArrOfPartDtls;
                }
                else {
                    txtparst.value = txtparst.value + '#' + ArrOfPartDtls;
                }

            } else {
                removePartID = txtparst.value;

                var afterRemove = "";
                var arr = removePartID.split("#");
                txtparst.value = "";
                var arrlen = arr.length;
                for (var i = 0; i < arrlen; i++) {
                    if (arr[i] == ArrOfPartDtls) {
                    }
                    else {

                        if (txtparst.value == "") {
                            txtparst.value = arr[i];
                        }
                        else {
                            txtparst.value = txtparst.value + '#' + arr[i];
                        }
                    }
                }
            }
            return txtparst.value;
        }
    </script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="PageTable table-responsive" border="1" width="100%">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="PageTitle panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text="">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">

                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef;" width="50%">
                                    <tr align="center">
                                        <td class="tdLabel" style="width: 7%;">Search:
                                        </td>
                                        <td class="tdLabel" style="width: 15%;">
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        </td>

                                        <td class="tdLabel" style="width: 15%;">
                                            <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="ComboBoxFixedSize">
                                                <asp:ListItem Selected="True" Value="P">Part No</asp:ListItem>
                                                <asp:ListItem Value="N">Part Name</asp:ListItem>
                                                <%--<asp:ListItem Value="I">Invoice No</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                                OnClick="btnSave_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search btn-sm" OnClick="btnBack_Click"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="tdLabel" style="width: 7%;"></td>
                                        <td class="tdLabel" style="width: 15%;" align="left" colspan="2">
                                            <asp:Label ID="lblNMsg" runat="server" Font-Size="8" CssClass="Mandatory" Text='Search Not Found...!'></asp:Label>
                                        </td>
                                        <td class="tdLabel" style="width: 15%;"></td>
                                        <td class="tdLabel" style="width: 10%;"></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; color: Red" colspan="4">
                                            <a>
                                                <asp:Label ID="lblMessage" runat="server" Visible="false" Text="1.Zero Stock Parts Not Show in the List"></asp:Label>
                                                </a>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%"
                                    AutoGenerateColumns="false" AllowPaging="True" PageSize="20">
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkRNPartClick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Qty" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceQty" runat="server" Text='<%# Eval("Invoice_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Code" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrCode" runat="server" Text='<%# Eval("group_code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Name" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Gr_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Qty" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockQty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Stock_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PartTax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartTax" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PartTaxID","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Per" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountPer" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("disc_per","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Accept Rate" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccept_Rate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Accept_Rate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO_No" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPO_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TaxTag" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaxTag" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("TaxTag") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnit" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Unit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrice" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Price","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax_Per" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax_Per" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Tax_Per","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MR_Dts_ID" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMR_Dts_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("MR_Dts_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Wrap="True" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>

                                <cc2:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" Width="1000px"
                                    GenerateGoToSection="true" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="TmpControl">
                        <td style="width: 15%">
                            <asp:TextBox ID="txtPartIds" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
