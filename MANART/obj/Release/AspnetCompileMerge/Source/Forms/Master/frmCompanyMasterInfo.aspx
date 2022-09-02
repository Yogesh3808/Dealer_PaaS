<%@ Page Title="MTI-Company Master" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableViewState="true"  CodeBehind="frmCompanyMasterInfo.aspx.cs" Inherits="MANART.Forms.Master.frmCompanyMasterInfo" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                // event.returnValue = false;
                //alert(message);
                // return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }

        //        function refresh() {
        //            if (event.keyCode == 116 || event.keyCode == 8) {
        //                event.keyCode = 0;
        //                event.returnValue = false
        //                return false;
        //            }
        //        }

        function refresh() {
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }
        document.onkeydown = function () {
            refresh();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle" align="center" >
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title" ForeColor="White">
                </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 15%; height: 92px;">
                <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                        ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Dealer Header Details"
                        ExpandedText="Dealer Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center"  width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Dealer Header Details" CssClass="panel-title" 
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" width="100%">
                            <tr>
                                <td style="width: 15%" >
                                    Dealer Name:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerName" runat="server" CssClass="TextBoxForString"   ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" >
                                    Dealer Short Name:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerShortName" runat="server"  CssClass="TextBoxForString"  ReadOnly="true"
                                        Text=""></asp:TextBox>
                                </td>
                                 <td style="width: 15%" >
                                    Dealer Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="TextBoxForString"   ReadOnly="true"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                
                                <td class="" style="width: 15%" rowspan="2">
                                    Address :
                                </td>
                                <td style="width: 20%" rowspan="2">
                                   
                                    <asp:TextBox ID="txtAddress" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                        Text="" TextMode="MultiLine" Rows="2" Height="50px"  ></asp:TextBox>
                                
                                </td>
                           
                            <td style="width: 15%" >
                                    City:
                                </td>
                                <td style="width: 20%">
                                  
                                    <asp:TextBox ID="txtCity" runat="server" ReadOnly="true" CssClass="TextBoxForString" 
                                       Text=""></asp:TextBox>
                                   
                                </td>
                                <td class="" style="width: 15%">
                                    District:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtDistrict" runat="server" ReadOnly="true" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" >
                                    State:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtState" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                         Text=""></asp:TextBox>
                                   
                                </td>
                                <td style="width: 15%">
                                    Dealer Depot:
                                </td>
                                <td style="width: 20%">
                                  
                                    <asp:TextBox ID="txtDealerDepot" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                         Text=""></asp:TextBox>
                                   
                                </td>
                               
                            </tr>
                            <tr>
                                 <td class="" style="width: 15%">
                                    Country:
                                </td>
                                <td style="width: 20%">
                                 
                                    <asp:TextBox ID="txtCountry" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                         Text=""></asp:TextBox>
                                    
                                </td>
                                <td style="width: 15%" >
                                    Dealer Region:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtDealerRegion" runat="server" ReadOnly="true" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                 
                                </td>
                                <td style="width: 15%" >
                                    Dealer Mobile:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtDealerMobile" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                         Text=""></asp:TextBox>
                                    
                                </td>
                                
                               
                            </tr>
                            <tr>
                                <td style="width: 15%" >
                                    Landline Phone:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtLandLinePhone" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                         Text=""></asp:TextBox>
                                </td>
                                  <td style="width: 15%" class="">
                                    Email:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtEmail" runat="server" ReadOnly="true" CssClass="TextBoxForString" 
                                         Text=""></asp:TextBox>
                                  
                                </td>
                               
                                <td style="width: 15%" class="">
                                    MD Email:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtMDEmail" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                    
                                </td>
                                
                            </tr>
                            <tr>
                               
                                <td style="width: 15%" >
                                    Dealer Category:
                                </td>
                                <td style="width: 20%">
                                    
                                    <asp:TextBox ID="txtDealerCategory" runat="server"  ReadOnly="true"
                                       CssClass="TextBoxForString"  Text=""></asp:TextBox>
                                   
                                </td>
                                <td style="width: 15%" >
                                    Dealer Origin:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerOrigin" runat="server"  ReadOnly="true" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                </td>
                          
                          <td style="width: 15%" >
                                    TIN No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtTINNo" runat="server" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                  <td style="width: 15%">
                                    GSTIN No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtGSTNo" runat="server" ReadOnly="true" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%">
                                    PAN No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtPANNo" runat="server" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" >
                                    Active:
                                </td>
                                <td style="width: 20%">
                                   
                                    <asp:TextBox ID="txtActive" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text=""></asp:TextBox>
                                   
                                </td>
                            </tr>
                            <tr>
                                 
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtVATNo" Visible="false" ReadOnly="true" runat="server" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                          <%--  <tr>
                                <td style="width: 15%" class="">
                                    LVA No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLVANo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    IRC No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtIRCNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    Service Tax Type:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtServiceTaxType" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>--%>
                   <%--         <tr>
                              
                                <td style="width: 15%" class="">
                                    Reman Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRemanCode" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>--%>
                   <%--         <tr>
                                <td style="width: 15%" class="">
                                    Dealer Report Region :
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpDealerRepRegion" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                
                                <td style="width: 15%" class="" visible="false">
                                    Dealer Territory:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDealerTerritory" runat="server" CssClass="HideControl" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                              
                            </tr>--%>
                      <%--      <tr>
                                  <td style="width: 15%" class="">
                                    Vehicle Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtVehicleCode" runat="server"  ReadOnly="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    Spares Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtSparesCode" runat="server"  ReadOnly="false"></asp:TextBox>
                                </td>
                            </tr>--%>
                           <%-- <tr>
                                <td style="width: 15%" class="">
                                    HD Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtHDCode" runat="server"  ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    SAP Hierarchy Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtHierarchyCode" runat="server"  ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>--%>
                    <%--  <tr>
                                <td style="width: 15%" class="">
                                    BUS Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtBusCode" runat="server"  ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                 <td class="" style="width: 15%">
                                    Dealer Type:
                                </td>
                                <td style="width: 18%">
                                    <%--Sujata 24082011
                                        <asp:TextBox ID="txtDealerType" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                   <%-- <asp:DropDownList ID="DrpDealerType" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="True" OnSelectedIndexChanged="DrpDealerType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%-- sujata 28082011--%>
                            <%--    </td>
                            </tr>--%>
                             <%-- <tr>
                                <td style="width: 15%" class="">
                                    Sales Office:
                                </td>
                                <td style="width: 18%">
                                    <%--sujata 24082011
                                    <asp:TextBox ID="txtSalesOffice" runat="server" CssClass="TextBoxForString" ReadOnly="false" Enabled="false" 
                                        Text=""></asp:TextBox>--%>
                                <%--    <asp:TextBox ID="txtSalesOffice" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>
                            <%--    </td>
                                <td style="width: 15%" class="">
                                    <%-- sujata 28082011 Extended Warranty:--%>
                                   <%-- Under Distributor:
                                </td>--%>
                               <%-- <td style="width: 18%">
                                    <%--<%-- sujata 28082011
                                    <asp:TextBox ID="txtExtendedWarr" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <%--<asp:DropDownList ID="drpDistributor" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtExtendedWarr" runat="server" CssClass="DispalyNon" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>
                               <%-- </td>
                                <td style="width: 15%" class="">
                                    HO Branch:
                                </td>--%>
                              <%--  <td style="width: 18%">
                                    <%--Sujata 24082011<asp:TextBox ID="txtHOBranch" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                   <%-- <asp:TextBox ID="txtHOBranch" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>--%>
                              <%--  </td>
                           <%-- </tr>--%>--%>--%>--%>--%>--%>--%>--%>--%>--%>--%>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            
                    <asp:Panel ID="PModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEModelDetails" runat="server" TargetControlID="CntModelDetails"
                        ExpandControlID="TtlModelDetails" CollapseControlID="TtlModelDetails" Collapsed="false"
                        ImageControlID="ImgTtlModel Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="HO Branch Details" ExpandedText="HO Branch Details"
                        TextLabelID="lblTtlModelDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlModelDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlModelDetails" runat="server" Text="HO Branch Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<div align="center" class="containtable" style="height: 200px; overflow: auto">--%>
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                            AutoGenerateColumns="False" 
                           >
                            <%--OnRowDataBound="DetailsGrid_RowDataBound"--%>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                               
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HO Dealer Code" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHODealerCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("HODealerCode") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HO Dealer Name" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHODealerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("HODealerName") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Dealer Code" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchDealerCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("BranchDealerCode") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Dealer Name" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchDealerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("BranchDealerName") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HOBranchCode" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHOBranchCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("HOBranchCode") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                              
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                    <%-- </div>--%>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 15%">
            </td>
        </tr>
        <tr id="TmpControl" style="display:none">
            <td style="width: 15%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                    Text=""></asp:TextBox>
                <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
