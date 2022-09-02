<%@ Page Title="" Language="C#" Theme="SkinFile" AutoEventWireup="true" CodeBehind="frmSelectMultipart1.aspx.cs" Inherits="MANART.Forms.Common.frmSelectMultipart1" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Details</title>
    <script src="../../Scripts/jquery-1.11.1.js"></script>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>

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
        <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="PageTable table-responsive" border="1">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="PageTitle panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text="">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">

                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef;" width="70%">
                                    <tr align="center">
                                        <td class="tdLabel" style="width: 15%;">
                                            <asp:DropDownList ID="DrpSelFrom" runat="server" CssClass="ComboBoxFixedSize">
                                                <asp:ListItem Selected="True" Value="P">Part Master</asp:ListItem>
                                                <asp:ListItem Value="E">From Estimate</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
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
                                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search"
                                                OnClick="btnSave_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search"
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
                                    AutoGenerateColumns="false" AllowPaging="True" PageSize="20">
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClickOnJobcard(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Billing Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Warranty Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWarrRate" runat="server" Text='<%# Eval("WarrRate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Type Tag" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartTypeTag" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Part_Type_Tag")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartstock" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Stock","{0:#0.00}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EstID" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstDtlID" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("estDtlID")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Group code" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartGroup" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("group_code")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="1%">
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
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RMC Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lblAMCRate" runat="server" Text='<%# Eval("AMCRate","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="DealerOrigin" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDealerOrigin" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("DealerOrigin")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Qty" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Qty")%>'></asp:Label>
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>--%>

                                        <%--<asp:TemplateField HeaderText="Warrantable" ItemStyle-Width="5%" ItemStyle-CssClass="LabelCenterAlign">
                                <ItemTemplate>
                                    <asp:Label ID="lblWarrantable" runat="server" Text='<%# Eval("Warrantable") %>'></asp:Label>
                                </ItemTemplate>
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
                            <asp:TextBox ID="txtPartIds" runat="server" Width="1px"
                                Text=""></asp:TextBox><%--CssClass="HideControl"--%>
               
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>


<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<form id="form1" runat="server"> --%>

<%--</form>--%>
<%--</asp:Content>--%>
