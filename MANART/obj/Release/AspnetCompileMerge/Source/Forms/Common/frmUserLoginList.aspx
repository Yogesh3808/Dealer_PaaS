<%@ Page Title="MTI-User LoginList" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmUserLoginList.aspx.cs" Inherits="MANART.Forms.Common.frmUserLoginList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <div style="height: 375px; width: 721px">
            <%--<asp:Panel ID ="pnlPendingSummary" runat ="server"   >--%>
            <%-- <table border="1" width="300px">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 10%">
                    <asp:Label ID="lblTitle" runat="server" Text="Activ User List">
                    </asp:Label>
                </td>
            </tr>
        </table>--%>
            <table>
                <tr>
                    <td class="tdLabel" colspan="2">
                        <asp:Label ID="lblNote" runat="server" Text="" ForeColor="Green" Font-Size="Medium">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="CommandButton btn btn-search btn-xs"
                            OnClick="btnRefresh_Click" />
                    </td>
                </tr>
                <tr id="TblControl" runat="server">
                    <td colspan="2">
                        <div style="overflow: auto; height: 300px; width: 709px">
                            <%-- <asp:Panel ID="GridDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    ScrollBars="Vertical" Height ="1%" Width ="100%" >--%>
                            <asp:GridView ID="DocumentGrid" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black">
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />

                                <EditRowStyle Wrap="True" BorderColor="Black"></EditRowStyle>

                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>

                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Login Name" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLoginName" runat="server" Text='<%# Eval("LoginName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time in" ItemStyle-Width="24%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLoginDateTime" runat="server" Text='<%# Eval("LoginDateTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Type" ItemStyle-Width="13%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserType" runat="server" Text='<%# Eval("UserType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI/Dealer" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserDealer" runat="server" Text='<%# Eval("UserDealer") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                        <%-- </asp:Panel>--%>
                    </td>
                </tr>

            </table>
        </div>
    </div>
</asp:Content>
