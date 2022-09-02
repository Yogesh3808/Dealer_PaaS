<%@ Page Title="MTI-SAP Order Info" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="frmSAPOrderInfo.aspx.cs" Inherits="MANART.Forms.Common.frmSAPOrderInfo" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsIndentConsolidation.js"></script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Double">
             <table id="Table7" runat="server" class="ContainTable" border="0">
             <tr class="panel-heading">
              <td align="center" class="ContaintTableHeader panel-title" width="96%" colspan="6" >
                <asp:Label ID="Label14" runat="server" Text="Retrive XML" Width="96%"  Height="16px"></asp:Label>                             
              </td>  
             </tr>
             <tr>                
                <td class="tdLabel" style="width: 15%">
                Document Type:
                </td>                
                <td  align="left"  style="height: 15px">
                <asp:DropDownList ID="drpXML" runat="server" CssClass="ComboBoxFixedSize" 
                AutoPostBack ="false" >                
                <asp:ListItem Value="SPInv" >Spare Invoice</asp:ListItem>                
                 </asp:DropDownList>
                </td>
                <td class="tdLabel" style="width: 10%">                
                    &nbsp;</td>   
                <td class="tdLabel" style="width: 25%">
                    &nbsp;</td>
                <td style="width: 15%" >         
                <asp:Button ID="btnRetrive" runat="server" CssClass="btn btn-search"
                    Text="PingXML" OnClick="btnRetrive_Click"  />
                </td>                
                <td style="width: 15%" >                                         
                 <asp:Label ID="Label16" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                </td>            
               </tr>
                
             </table>            
            </asp:Panel> 

</asp:Content>
