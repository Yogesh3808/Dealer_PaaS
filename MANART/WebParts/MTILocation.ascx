
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MTILocation.ascx.cs" Inherits="MANART.WebParts.MTILocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<%--<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<style type="text/css">    
</style>--%>
<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<div class="table-responsive">
<asp:Panel ID="PExpLocation" runat="server" >
    <cc1:CollapsiblePanelExtender ID="CPEExpLocation" runat="server" TargetControlID="CntExpLocation"
        ExpandControlID="TtlExpLocation" CollapseControlID="TtlExpLocation" Collapsed="false"
        ImageControlID="ImgTtlLocation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
        SuppressPostBack="true" CollapsedText="Show Location Details" ExpandedText="Hide Location Details"
        TextLabelID="lblTtlLocation">
    </cc1:CollapsiblePanelExtender>
    <asp:Panel ID="TtlExpLocation" runat="server">
        <table width="100%">
            <tr class="panel-heading">
                <td align="center" class="ContaintTableHeader" width="96%">
                    <asp:Label ID="lblTtlLocation" runat="server" Text="Location Details" width="96%"
                         onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                </td>
                <td width="1%">
                    <asp:Image ID="ImgTtlLocation" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                        Width="100%" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="CntExpLocation" runat="server" >
        <table id="LocationDetails" runat="server" class="ContainTable table table-bordered">
        
        <tr >
                
                <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                    <%--Distributor Name:--%>
                    <asp:Label ID="LblDealerName" runat="server" Text="Dealer Name:" width="96%"></asp:Label>
                </td>
                <td style="width: 18%">
                    <asp:DropDownList ID="drpDealerName" runat="server" Width="335px" CssClass="ComboBoxFixedSize" EnableViewState="true"
                        AutoPostBack="True" OnSelectedIndexChanged="drpDealerName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                 <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                    <%--Distributor Code:--%>
                    <asp:Label ID="lblDealerCode" runat="server" Text="Dealer Code:" width="96%"></asp:Label>
                </td>
                <td style="width: 15%">
                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="TextBoxForString" Width="40%"
                        ReadOnly="true" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtAllDealerID" runat="server"  Width="1%" CssClass="HideControl" Text=""></asp:TextBox>
                </td>
            </tr>
            <tr>
                  <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                   <asp:Label ID="lblRegion" runat="server" Text=" Region:"></asp:Label>
                </td>
                <td style="width: 18%">                    
                    <asp:TextBox ID="txtRegion" runat="server"  Width="60%" Text="" ReadOnly ="true" CssClass="TextBoxForString"></asp:TextBox>
                </td>
                <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                    <asp:Label ID="lblCountry" runat="server" Text=" Country:"></asp:Label>
                </td>
                <td style="width: 18%">
                    <asp:TextBox ID="txtCountry" runat="server"  Width="40%" Text="" ReadOnly ="true" CssClass="TextBoxForString"></asp:TextBox>
                </td>
              
            </tr>
            <tr>
                <td class="tdLabel" style="width: 15%">
                    <%--Distributor Name:--%>
                    <asp:Label ID="lblType" runat="server" Visible="false" Text="Type:" width="96%"></asp:Label>
                </td>
                <td style="width: 18%; display:none">
                    <asp:DropDownList ID="ddlType" Visible="false" runat="server" Width="150px" CssClass="ComboBoxFixedSize" EnableViewState="true"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" value="ED">Distributor</asp:ListItem>
                         <asp:ListItem value="LS">Local Supplier</asp:ListItem>
                    </asp:DropDownList>
                </td>
                  <td class="tdLabel" style="width: 15%">
                    <%--Currency:--%>
                    <asp:Label ID="LblCurrency" Visible="false" runat="server" Text="Currency:" width="96%"></asp:Label>
                </td>
                <td style="width: 18%">
                    <asp:TextBox ID="txtCurrency" Visible="false" Text="" runat="server" CssClass="TextBoxForString"
                        Width="40%" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            
        </table>     
        
    </asp:Panel>
</asp:Panel>
    </div>