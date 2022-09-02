<%@ Page Title="MTI-Service History Details" Language="C#" AutoEventWireup="true" CodeBehind="frmServiceHistoryDetails.aspx.cs" Inherits="MANART.Forms.Admin.frmServiceHistoryDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<head runat="server">
    <title></title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
             

    <table class="PageTable" border="1"><tr id="trMultiGrid" runat="server">
            <td colspan="2">
           <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <asp:GridView ID="DetailsGrid" runat="server" Width="100%" AutoGenerateColumns="False"
                        AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black" onrowdatabound="DetailsGrid_RowDataBound" Height="165px">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                       
                        <Columns>                            
                            <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dealer_Name" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' ></asp:Label>
                                </ItemTemplate>
                                 
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service_Type" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceType" runat="server" Text='<%# Eval("Service_Type") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jobcard_No" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobcardNo" runat="server" Text='<%# Eval("Jobcard_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jobcard_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate >
                                    <asp:Label ID="lblJobcardDate" runat="server" Text='<%# Eval("Jobcard_Date","{0:dd/MM/yyyy}") %>'  Width="96%"/>
                                </ItemTemplate>
                                  <ItemStyle Width="500%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Coupon_No" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblCouponNo" runat="server" Text='<%# Eval("coupon_no") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Kms" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblKms" runat="server" Text='<%# Eval("Kms") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Engine_Hr" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblEngineHr" runat="server" Text='<%# Eval("Engine_Hr") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Invoice_No" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblInvNo" runat="server" Text='<%# Eval("inv_no") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Invoice_Date" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("InvDate","{0:dd/MM/yyyy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Part/Labor/Oil_type" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartLabourOilType" runat="server" Text='<%# Eval("Part_type_tag") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Part_LaborCode" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartLabourCode" runat="server" Text='<%# Eval("PartLabourCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Part_Description" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartDesc" runat="server" Text='<%# Eval("Part_Description") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Mech_Name" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMechName" runat="server" Text='<%# Eval("mech_name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jobcode" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobcode" runat="server" Text='<%# Eval("jobcode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FOC_Qty" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblFOCQty" runat="server" Text='<%# Eval("FOCQty","{0:#0.00}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FSC_Qty" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblFSCQty" runat="server" Text='<%# Eval("FSCQty","{0:#0.00}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>                           
                             
                        </Columns>
                        <HeaderStyle Wrap="True" />
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>

                 
                    <asp:TextBox ID="txtPostID" runat="server" CssClass="DispalyNon" Visible="false"></asp:TextBox>                  
                </asp:Panel>
            </td>
        </tr></table>
    </div>
    </form>
</body>
</html>
