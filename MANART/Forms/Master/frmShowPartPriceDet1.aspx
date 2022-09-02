<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmShowPartPriceDet.aspx.cs" 
    Theme="SkinFile" Title="MTI-Show Price History"Inherits="MANART.Forms.Master.frmShowPartPriceDet" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>History Of Part Price</title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <table class="table-responsive" border="1" width="100%">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="PartPriceGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered "
                        Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                        OnPageIndexChanging="PartPriceGrid_PageIndexChanging">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PartID" ItemStyle-Width="2%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartID" runat="server" Text='<%# Eval("Part_ID") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Part No" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPart_No" runat="server" Text='<%# Eval("Part_No") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartl_Name" runat="server" Text='<%# Eval("Part_name") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("EffectiveFromDate") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("EffectiveToDate") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LIST" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("LIST","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MRP" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("MRP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NDP" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNDP" runat="server" Text='<%# Eval("NDP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                    <asp:GridView ID="ModelPriceGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                        Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                        OnPageIndexChanging="ModelPriceGrid_PageIndexChanging">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPart_No" runat="server" Text='<%# Eval("Fert Code") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model Name" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartl_Name" runat="server" Text='<%# Eval("Model Name") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("From Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="20%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("To Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MRP" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("MRP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NDP" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNDP" runat="server" Text='<%# Eval("NDP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                    <asp:GridView ID="LubricantPriceGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                        Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                        OnPageIndexChanging="LubricantPriceGrid_PageIndexChanging">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lubricant Type" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblLubricantType" runat="server" Text='<%# Eval("Lubricant Type") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lubricant Rate" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblLubricantRate" runat="server" Text='<%# Eval("Lubricant Rate","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("From Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("To Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                    <asp:GridView ID="ServicePolicyGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                        Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                        OnPageIndexChanging="ServicePolicyGrid_PageIndexChanging">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceName" runat="server" Text='<%# Eval("Service Name") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelCode" runat="server" Text='<%# Eval("Model Code") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UP TO Km" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelCode" runat="server" Text='<%# Eval("Up TO Km") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("From Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("To Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("Active") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                    <asp:GridView ID="LaborRateGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                        Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                        OnPageIndexChanging="LaborRateGrid_PageIndexChanging">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer Code") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer Name") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("From Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("To Date") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate Per Hours" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblRatePerHrs" runat="server" Text='<%# Eval("Rate Per Hours") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency") %>' CssClass="LabelLeftAlign" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
