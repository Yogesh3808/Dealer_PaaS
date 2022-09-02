<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MANPendingDoc.ascx.cs" Inherits="MANART.WebParts.MANPendingDoc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<link href="../Content/GridStyle.css" rel="stylesheet" />
<%--<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />--%>
<div class="table-responsive">
    <asp:Panel ID="PPendDocList" runat="server">
        <cc1:CollapsiblePanelExtender ID="CPEPendDocList" runat="server" TargetControlID="CntPendDocList"
            ExpandControlID="TtlPendDocList" CollapseControlID="TtlPendDocList" Collapsed="false"
            ImageControlID="ImgTtlLocation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
            SuppressPostBack="true" CollapsedText="Show Location Details" ExpandedText="Hide Location Details"
            TextLabelID="lblTtlPendDocList">
        </cc1:CollapsiblePanelExtender>
        <asp:Panel ID="TtlPendDocList" runat="server">
            <table width="100%">
                <tr>
                    <td align="center" class="ContaintTableHeader" width="96%">
                        <asp:Label ID="lblTtlPendDocList" runat="server" Text="Pending Document Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                    </td>
                    <td width="1%">
                        <asp:Image ID="ImgTtlLocation" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                            Width="100%" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="CntPendDocList" runat="server" ScrollBars="Auto" Width="100%">
            <asp:Label ID="lblTitle" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Small" Font-Names="Verdana" Style="display: none">
            </asp:Label>
            <div id="gridView" class="grid">
                <asp:GridView ID="DocumentGrid" runat="server" CssClass="datatable table table-condensed table-responsive" CellPadding="0"
                    CellSpacing="0" GridLines="Horizontal" Style="border-collapse: separate;" HeaderStyle-CssClass="GridViewHeaderStyle"
                    AlternatingRowStyle-CssClass="odd" RowStyle-CssClass="even" AllowPaging="True" AutoGenerateColumns="false" OnPageIndexChanging="DocumentGrid_PageIndexChanging" OnRowCommand="DocumentGrid_RowCommand">
                    <FooterStyle CssClass="GridViewFooterStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                    <%--<SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <EditRowStyle BorderColor="Black" Wrap="True" />--%>
                    <PagerStyle CssClass="GridViewPagerStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png"
                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);" CommandArgument='<%# Eval("ID")+ "," + Eval("Dealer_ID") + "," + Eval("PO_Type") %>' CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dealer Code">
                            <ItemTemplate>
                                <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dealer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Doc No">
                            <ItemTemplate>
                                <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("Doc_No") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Doc Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDocDate" runat="server" Text='<%# Eval("Doc_Date") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                      
                       
                    </Columns>

                </asp:GridView>
                <asp:HiddenField ID="hdnPDocID" Value="0" runat="server" />
                <asp:HiddenField ID="hdnPDocNo" Value="0" runat="server" />
            </div>
        </asp:Panel>
    </asp:Panel>
</div>
