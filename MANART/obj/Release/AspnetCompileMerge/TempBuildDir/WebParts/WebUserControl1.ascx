<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="MANART.WebParts.WebUserControl1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Content/bootstrap.css" rel="stylesheet" />
<link href="../Content/bootstrap.min.css" rel="stylesheet" />
<link href="../Content/style.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
<script src="../Scripts/megamenu.js"></script>
<script src="../Scripts/bootstrap.min.js"></script>
<section>
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
                            <label for="usr">Country.:</label>
                            <asp:Label ID="lblCountry" runat="server" Visible="false">Country.:</asp:Label>
                            <asp:DropDownList ID="drpCountry" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="form-group">
                            <label for="usr">Currency:</label>
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
</section>

