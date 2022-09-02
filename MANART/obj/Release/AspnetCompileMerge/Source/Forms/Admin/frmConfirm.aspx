<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmConfirm.aspx.cs" Inherits="MANART.Forms.Admin.frmConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 15%">
                &nbsp;</td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="tblLogin" runat="server" class="ContainTable">
                    <tr>
                        <td align="center" style="padding-top: 5px; padding-bottom: 5px;">
                            <asp:Label ID="lblConfirmation" runat="server"></asp:Label>
                            <br />
                            <asp:Label ID="lblEmailconfirmation" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 15%">
                &nbsp;</td> </tr> </table>
    </div>
</asp:Content>
