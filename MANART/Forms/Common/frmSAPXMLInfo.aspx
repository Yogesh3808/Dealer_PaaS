<%@ Page Title="MTI-SAP Order Info" Language="C#"  MasterPageFile="~/Header.Master" AutoEventWireup="true" 
    CodeBehind="frmSAPXMLInfo.aspx.cs" Inherits="MANART.Forms.Common.frmSAPXMLInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Retrive XML"> </asp:Label>
            </td>
        </tr>

        <tr id="TblControl">
            <td class="tdLabel" style="width: 15%">Document Type:
            </td>
            <td align="left" style="height: 15px">
                <asp:DropDownList ID="drpXML" runat="server" CssClass="ComboBoxFixedSize"
                    AutoPostBack="false">
                    <asp:ListItem Value="SPInv">Spare Invoice</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="tdLabel" style="width: 10%">&nbsp;</td>
            <td class="tdLabel" style="width: 25%">&nbsp;</td>
            <td style="width: 15%">
                <asp:Button ID="btnClick" runat="server"  Text="PingXML" OnClick="btnClick_Click" />
                <%--<asp:Button ID="btnRetrive" runat="server" CssClass="btn btn-search"
                        Text="PingXML" OnClick="btnRetrive_Click" />--%>
                   

            </td>
            <td style="width: 15%">
                <asp:Label ID="Label16" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
      
    </table>

</asp:Content>
