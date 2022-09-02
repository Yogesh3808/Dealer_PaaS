<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportLocation.ascx.cs" Inherits="MANART.WebParts.ExportLocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<div class="table-responsive">
    <asp:Panel ID="PExpLocation" runat="server">
        <cc1:CollapsiblePanelExtender ID="CPEExpLocation" runat="server" TargetControlID="CntExpLocation"
            ExpandControlID="TtlExpLocation" CollapseControlID="TtlExpLocation" Collapsed="false"
            ImageControlID="ImgTtlLocation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
            SuppressPostBack="true" CollapsedText="Supplier Details" ExpandedText="Supplier Details"
            TextLabelID="lblTtlLocation">
        </cc1:CollapsiblePanelExtender>
        <asp:Panel ID="TtlExpLocation" runat="server">
            <table width="100%">
                <tr class="panel-heading">
                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                        <asp:Label ID="lblTtlLocation" runat="server" Text="Supplier Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                    </td>
                    <td width="1%">
                        <asp:Image ID="ImgTtlLocation" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                            Width="100%" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="CntExpLocation" runat="server">
            <table id="LocationDetails" runat="server" class="ContainTable table table-bordered">
                <tr>
                    <td class="tdLabel" style=" display:none; ">
                        <asp:Label ID="lblType" runat="server" Text="Type:" Width="96%"></asp:Label>
                    </td>
                    <td style=" display:none">
                        <asp:DropDownList ID="ddlType" runat="server" Width="150px" CssClass="ComboBoxFixedSize" EnableViewState="true"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem  Value="ED" Enabled="false">Distributor</asp:ListItem>
                            <asp:ListItem Selected="True" Value="LS">Supplier</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                        <%--Supplier Name:--%>
                        <asp:Label ID="LblDealerName" runat="server" Text="Supplier Name:" Width="96%"></asp:Label>
                    </td>
                    <td style="width: 35%">
                        <asp:DropDownList ID="drpDealerName" runat="server" Width="90%" CssClass="ComboBoxFixedSize " EnableViewState="true"
                            AutoPostBack="True" OnSelectedIndexChanged="drpDealerName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblMDealername" runat="server" CssClass="Mandatory" Text=" *" ></asp:Label>
                    </td>
                    <td class="tdLabel" style="width: 10%; padding-left: 10px;">
                        <%--Supplier Code:--%>
                        <asp:Label ID="lblDealerCode" runat="server" Text="Supplier Code:" Width="96%"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txtDealerCode" runat="server" CssClass="NonEditableFields"  width="96%"
                            ReadOnly="true" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtAllDealerID" runat="server" Width="1%" CssClass="HideControl" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                        <asp:Label ID="lblRegion" runat="server" Text=" Region:"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:TextBox ID="txtRegion" runat="server" Width="60%" Text="" ReadOnly="true" CssClass="NonEditableFields"></asp:TextBox>
                    </td>
                    <td class="tdLabel" style="width: 10%;padding-left: 10px;">
                        <asp:Label ID="lblCountry" runat="server" Text=" Country:"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:TextBox ID="txtCountry" runat="server"  width="96%" Text="" ReadOnly="true" CssClass="NonEditableFields"></asp:TextBox>
                    </td>
                    <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                        <asp:Label ID="LblCurrency" runat="server" Text="Currency:" Width="96%"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:TextBox ID="txtCurrency" Text="" runat="server" CssClass="NonEditableFields"
                             width="96%" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</div>
