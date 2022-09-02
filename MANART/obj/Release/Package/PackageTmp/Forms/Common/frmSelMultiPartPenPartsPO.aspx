<%@ Page Language="C#" Title="Parts PO Details" AutoEventWireup="true" CodeBehind="frmSelMultiPartPenPartsPO.aspx.cs" Inherits="MANART.Forms.Common.frmSelMultiPartPenPartsPO" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
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
    </script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="PageTable" border="1" width="100%">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text="">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">

                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef;" width="70%">
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
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                                OnClick="btnSave_Click" />
                                            &nbsp;
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search btn-sm"
                                                 OnClick="btnBack_Click"></asp:Button>
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
                                </table>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%"
                                    AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="FOBRate" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFOBRate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Rate","{0:#0.00}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>

                                        <%-- <asp:TemplateField HeaderText="Part_No_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartNoID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> --%>
                                        <asp:TemplateField HeaderText="PODetID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOADetID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_Det_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOANo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMOQ" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("MOQ") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Qty","{0:#0}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>


                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="70%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Code" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrCode" runat="server" Text='<%# Eval("group_code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Name" ItemStyle-Width="80%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Gr_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bal" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClbal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cl_bal","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<ItemStyle CssClass="LabelRightAlign" Width="5%" />--%>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PartTax" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartTax" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PartTaxID","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <%--Sujata 04092014_Begin OA Discount and tax taken in Invoice--%>
                                        <asp:TemplateField HeaderText="Bal PO Qty" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvQty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Per" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountPer" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("discount_per","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="7%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountAmt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("discount_amt","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Rate" ItemStyle-Width="8%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountRate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("disc_rate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shortage_Qty" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShortage_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Shortage_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Excess_Qty" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExcess_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Excess_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Damage_Qty" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDamage_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Damage_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Man_Defect_Qty" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMan_Defect_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Man_Defect_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wrong_Supply_Qty" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWrong_Supply_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrong_Supply_Qty","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wrg_Part_ID" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWrg_Part_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrg_Part_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wrg_Part_No" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWrg_Part_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrg_Part_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wrg_Part_Name" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWrg_Part_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrg_Part_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                       
                                         <asp:TemplateField HeaderText="TaxTag" ItemStyle-Width="9%" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaxTag" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("TaxTag") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="5%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Tax1" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax1" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax1")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax2" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax2")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>--%>
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
