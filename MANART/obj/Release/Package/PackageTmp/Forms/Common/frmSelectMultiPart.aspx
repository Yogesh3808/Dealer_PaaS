<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master"
    Theme="SkinFile" AutoEventWireup="true" CodeBehind="frmSelectMultiPart.aspx.cs" Inherits="MANART.Forms.Common.frmSelectMultiPart" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Details</title>
    <link href="../../CSS/PaggerStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/GridStyle.css" rel="stylesheet" type="text/css" />
    

    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsShowForm.js" type="text/javascript"></script>

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
    </script>
    
  <base target="_self" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <table class="PageTable" border="1" width="100%">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <div align="center" class="ContainTable">
                    <table style="background-color: #efefef;" width="50%">
                        <tr align="center">
                            <td class="tdLabel" style="width: 7%;">
                                Search:
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
                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="CommandButton" 
                                  Height="15px" Width="50px" onclick="btnSave_Click"/>                            
                            </td>
                            <td class="tdLabel" style="width: 10%;">
                                <asp:Label ID="lblBack" runat="server" Text="Back" onClick="return ReturnMultiWPartDetails();"
                                    CssClass="CommandButton"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <%--Sujata 24022011--%>
                         <tr align="center">
                            <td class="tdLabel" style="width: 7%;">                                
                            </td>
                            <td class="tdLabel" style="width: 15%;" align=left colspan =2>                                
                               <asp:Label ID="lblNMsg" runat="server" Font-Size ="8"  CssClass="Mandatory"  Text='Search Not Found...!'></asp:Label>
                            </td>                            
                            <td class="tdLabel" style="width: 15%;">                                                              
                            </td>                            
                            <td class="tdLabel" style="width: 10%;">
                            </td>
                        </tr>
                        <%--Sujata 24022011--%>
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
                        <Columns>
                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkPart" OnClick="return ChkPartClick(this);" runat="server" />
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
                            <asp:TemplateField HeaderText="Part No." ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="80%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="80%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Warrantable" ItemStyle-Width="5%" ItemStyle-CssClass="LabelCenterAlign">
                                <ItemTemplate>
                                    <asp:Label ID="lblWarrantable" runat="server" Text='<%# Eval("Warrantable") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="LabelCenterAlign" Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                            <HeaderStyle Wrap="True" />
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                    <cc2:PagerV2_8 ID="PagerV2_1" runat="server"  OnCommand="pager_Command" Width="1000px" 
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
    </form>
</asp:Content>
