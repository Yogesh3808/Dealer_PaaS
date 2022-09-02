<%@ Page Title="MTI- Part Master" Language="C#" AutoEventWireup="true" CodeBehind="frmSelectPart.aspx.cs" Inherits="MANART.Forms.Common.frmSelectPart1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
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

        }
    </script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="table-responsive">
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
                        <table style="background-color: #efefef;" width="50%">
                            <tr align="center">
                                <td class="tdLabel" >Search:
                                </td>
                                <td class="tdLabel" >
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass=""></asp:TextBox>
                                </td>

                                <td class="tdLabel" >
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="ComboBoxFixedSize">
                                        <asp:ListItem Selected="True" Value="Part_No">Part No</asp:ListItem>
                                        <asp:ListItem Value="Part_Name">Part Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel" >
                                    <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass="CommandButton"></asp:Label>--%>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                                        CssClass="btn btn-search btn-sm" OnClick="btnSearch_Click"></asp:Button>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="tdLabel" style="width: 10%;">
                                    <%--<asp:Label ID="lblBack" runat="server" Text="Back" onClick="return ReturnFromForm();" CssClass="CommandButton"></asp:Label> --%>
                                    <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search btn-sm" OnClientClick="return ReturnFromForm();"></asp:Button>

                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                        <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                            AutoGenerateColumns="false"
                            OnPageIndexChanging="PartDetailsGrid_PageIndexChanging">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Image ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" onClick="return ReturnPartDetails(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="80%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warrantable" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWarrantable" runat="server" Text='<%# Eval("Warrantable") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMOQ" runat="server" Text='<%# Eval("MOQ") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle Wrap="True" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
