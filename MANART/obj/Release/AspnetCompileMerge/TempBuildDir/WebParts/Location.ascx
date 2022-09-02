<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Location.ascx.cs"
    EnableViewState="true" Inherits="MANART.WebParts.Location" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<link href="../Content/bootstrap.min.css" rel="stylesheet" />--%>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
<script src="../Scripts/megamenu.js"></script>
<%--<script src="../Scripts/bootstrap.min.js"></script>--%>
<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<div class="table-responsive">
    <asp:Panel ID="PLocation" runat="server">
        <cc1:CollapsiblePanelExtender ID="CPELocation" runat="server" TargetControlID="CntLocation"
            ExpandControlID="TtlLocation" CollapseControlID="TtlLocation" Collapsed="false"
            ImageControlID="ImgTtlLocation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
            SuppressPostBack="true" CollapsedText="Location Details" ExpandedText="Location Details"
            TextLabelID="lblTtlLocation">
        </cc1:CollapsiblePanelExtender>
        <asp:Panel ID="TtlLocation" runat="server">
            <table width="100%">
                <tr class="panel-heading">
                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                        <asp:Label ID="lblTtlLocation" runat="server" Text="Location Details" Width="96%"
                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                    </td>
                    <td width="1%">
                        <asp:Image ID="ImgTtlLocation" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                            Width="100%" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="CntLocation" runat="server">
            <table id="LocationDetails" runat="server" class="ContainTable table table-bordered">
                <tr>
                    <td class="tdLabel" style="width: 12%;padding-left:10px">Region:
                    </td>
                    <td style="width: 20%">
                        <asp:DropDownList ID="drpRegion" Width="175px" runat="server" CssClass="ComboBoxFixedSize"
                            OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td class="tdLabel" style="width: 12%; padding-left:10px">
                        <asp:Label ID="lblCountry" runat="server" Text="Country.:" Width="96%" Style="text-align: left"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:DropDownList ID="drpCountry" runat="server" Width="175px" CssClass="ComboBoxFixedSize"
                            OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="tdLabel" style="width: 12%;padding-left:10px">
                        <%--Currency:--%>
                        <asp:Label ID="LblCurrency" runat="server" Text="Currency:" Width="96%"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:TextBox ID="txtCurrency" Text="" runat="server" CssClass="TextBoxForString"
                            Width="30%" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr id="DealerRow" >
                    <td class="tdLabel" style="width: 12%;padding-left:10px">
                        <%--Distributor Name:--%>
                        <asp:Label ID="LblDealerName" runat="server" Text="Supplier Name:" Width="96%"></asp:Label>
                    </td>
                    <td colspan="2" style="width:32%;">
                        <asp:DropDownList ID="drpDealerName" runat="server" Width="335px" CssClass="ComboBoxFixedSize"
                            EnableViewState="true" AutoPostBack="True" OnSelectedIndexChanged="drpDealerName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdLabel" style="width: 12%; padding-left:10px">
                        <%--Distributor Code:--%>
                        <asp:Label ID="lblDealerCode" runat="server" Text="Supplier Code:" Width="96%"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:TextBox ID="txtDealerCode" runat="server" CssClass="TextBoxForString" Width="60%"
                            ReadOnly="true" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtAllDealerID" runat="server" Width="1%" CssClass="HideControl"
                            Text=""></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</div>
<%--<section>
    <div class="panel-body">
        <div class="white-border">
            <div style="margin-bottom: 15px;" class="row">
                <div id="LocationDetails" runat="server">
                    <div class="col-md-4 col-sm-4">
                        <div class="form-group">
                            <label for="usr">Region</label>
                            <asp:Label ID="lblRegion" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="drpRegion" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="form-group">
                            <label for="usr" class="HideControl">Country.:</label>
                            <asp:Label ID="lblCountry" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="drpCountry" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="form-group">
                            <label for="usr" class="HideControl">Currency:</label>
                            <asp:Label ID="LblCurrency" runat="server" Visible="false">Currency:</asp:Label>
                            <asp:TextBox ID="txtCurrency" Text="" runat="server" CssClass="form-control"
                                ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-5 col-sm-5">
                    <div class="form-group">
                        <label for="usr">Distributor Name:</label>
                        <asp:Label ID="LblDealerName" runat="server" Visible="false">Distributor Name:</asp:Label>
                        <asp:DropDownList ID="drpDealerName" runat="server" CssClass="form-control" EnableViewState="true" Width="90%"
                            AutoPostBack="True" OnSelectedIndexChanged="drpDealerName_SelectedIndexChanged">
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="col-md-5 col-sm-5">
                    <div class="form-group">
                        <label for="usr">Distributor Code:</label>
                        <asp:Label ID="lblDealerCode" runat="server" Visible="false">Distributor Code:</asp:Label>
                        <asp:TextBox ID="txtDealerCode" runat="server" CssClass="form-control " Width="30%"
                            ReadOnly="true" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtAllDealerID" runat="server" CssClass="form-control hidden"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2"></div>
            </div>

        </div>
    </div>
</section>--%>



