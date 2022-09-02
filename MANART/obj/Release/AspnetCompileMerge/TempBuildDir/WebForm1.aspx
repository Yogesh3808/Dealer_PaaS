<%@ page title="Document Status" language="C#" masterpagefile="~/Header.Master" autoeventwireup="true" codebehind="WebForm1.aspx.cs" inherits="MANART.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
  <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Document Status"> </asp:Label>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Open Document" ExpandedText="Open Document"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Document Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDocDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Document Name:
                                </td>
                                <td style="width: 25%">
                                    <asp:DropDownList ID="ddlDocName" runat="server" CssClass="ComboBoxFixedSize"  >
                                        <asp:ListItem Text=" Parts OA" Value="OA" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Parts Counter Sale Invoice" Value="INV"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:TextBox ID="txtDocNo" runat="server" ></asp:TextBox>
                                </td>
                                <td style="width: 15%">
                                    <asp:button ID="btnOpenDocument" runat="server" Text="Open Document" CssClass="btn btn-search btn-sm" OnClick="OpenDocument"  />
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                </td>
                                <td style="width: 15%">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
               
                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="true"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Only Header Save Details" ExpandedText="Only Header Save Details"
                        TextLabelID="lblTtlPartDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Only Header Save Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <div style="border: 1px solid #084B8A; color: #ffffff; font-weight: bold;">
                            <table style="text-align: left; height: 38px; line-height: 17px; padding: 0px 4px; background-color: #70757A; border-right: solid 1px #9e9e9e; color: white;"
                                class="table table-condensed table-bordered">
                                <tr>
                                    <th style="width: 2%;">Sr No </th>
                                    <th style="width: 5%;">Doc ID </th>
                                    <th style="width: 30%;">Doc No</th>
                                    <th style="width: 10%;">Doc Date</th>
                                    <th style="width: 3%;">Dealer ID</th>
                                    <th style="width: 20%;">Document Status</th>
                                </tr>
                            </table>
                        </div>
                        <div style="height: 300px; overflow: auto; background-color: #D4D4D4;">
                            <asp:GridView ID="gvOnlyHeaderSave" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                CssClass="table table-condensed table-bordered" ShowHeader="false"
                                AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"  EmptyDataText="No records Found"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SrNo." ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Eval("SRNo")   %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc ID" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                             <asp:Label ID="lblDocID" runat="server" Text='<%# Eval("DocID")   %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document No." ItemStyle-Width="30%">
                                        <ItemTemplate>
                                               <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("DocNo")   %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocDate" runat="server" Text='<%# Eval("DocDate")   %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer ID" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                                <asp:Label ID="lblDealerID" runat="server" Text='<%# Eval("DealerID")   %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Status" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                                <asp:Label ID="lblDocStatus" runat="server" Text='<%# Eval("DocStatus")   %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                
            </td>
        </tr>
    </table>
</asp:content>
