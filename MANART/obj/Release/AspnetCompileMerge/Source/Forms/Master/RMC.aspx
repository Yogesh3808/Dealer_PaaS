<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="RMC.aspx.cs" Inherits="MANART.Forms.Master.RMC" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../../CSS/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/cssDatePicker.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/GridStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsProformaFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>

        <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <table id="PageTbl" class="PageTable" border="1">

        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 15%; display: none">
                <asp:Label ID="Label1" runat="server" Text="Chassis RMC"> </asp:Label>
            </td>
        </tr>

        <%--<tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
        <tr id="TblControl">
            <td style="width: 15%">
               
                <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Visible="true" >
                    <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                        ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Upload File"
                        ExpandedText="Hide Upload File" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server" >
                        <table width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Upload File" Width="100%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double" >
                        <table id="Table3" runat="server" class="table table-responsive table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Select File:
                                </td>
                                <%--  OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');" --%>
                                <td style="width: 18%">
                                    <asp:FileUpload ID="txtFilePath" runat="server" />
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-search"
                                         /><br />
                                    <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double" >
                    <table id="Table2" runat="server" class="table table-bordered" border="1">
                        <tr>



                            <td>

                              <asp:DropDownList ID="drpSearch" runat="server" CssClass="ComboBoxFixedSize"> <%--onchange="OnStateChange()--%> 
                                                <asp:ListItem Selected="True" Value ="1">Chassis No</asp:ListItem> 
                                                <asp:ListItem Value ="2" Enabled="false">SA Type</asp:ListItem>                                                
                                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Search: "></asp:Label>
                            </td>
                            <td style="width: 1%; height: 23px;">


                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString" Width="150px"
                                    Text=""></asp:TextBox>
                            </td>

                            <td class="tdLabel" style="width: 10%; height: 23px;">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="CommandButton" />
                            </td>
                            <td>
                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search" CssClass="CommandButton"
                                    Visible="false"  />
                                <asp:Label ID="lblMessage1" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>

                            </td>
                            <td></td>

                        </tr>
                    </table>
                </asp:Panel>
                   

                <asp:Panel ID="PChassisAMCDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEChassisAMCDetails" runat="server" TargetControlID="CntChassisAMCDetails"
                        ExpandControlID="TtlChassisAMCDetails" CollapseControlID="TtlChassisAMCDetails" Collapsed="false"
                        ImageControlID="ImgTtlChassisAMCDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Lubricant Part Rate Details" ExpandedText="Hide Lubricant Part Rate Details">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlChassisAMCDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlChassisAMCDetails" runat="server" Text="LUBRICANT PART RATE DETAILS" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);" ForeColor="White" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlChassisAMCDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntChassisAMCDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="Horizontal">
                           <asp:GridView ID="gvLubPartRateDetails" runat="server" AllowPaging="True" Width="100%" CssClass="datatable table table-bordered" DataKeyNames="ID"
                            AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="False" CellPadding="0"
                            CellSpacing="0" GridLines="Horizontal"
                            HeaderStyle-CssClass="GridViewHeaderStyle"
                             RowStyle-CssClass="even" Style="border-collapse: separate;">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                                           <Columns>
                                            <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChassisNo" runat="server" Text='<%# Eval("Chassis_no") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SA Type" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCType" runat="server" Text='<%# Eval("AMC_type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SA Start Date" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCStartDate" runat="server" 
                                                        Text='<%# Eval("AMC_Start_Date", "{0:d}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SA End Date" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCEndDate" runat="server" 
                                                        Text='<%# Eval("AMC_End_Date", "{0:d}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%" HeaderText="SA Agreement No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCaggrementNo" runat="server" 
                                                        Text='<%# Eval("AMC_aggrement_No") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SA Agreement Date" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCAggrementDate" runat="server" 
                                                        Text='<%# Eval("AMC_aggrement_date", "{0:d}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SA Start KM" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCStartKM" runat="server" 
                                                        Text='<%# Eval("AMC_Start_KM", "{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SA End KM" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCEndKM" runat="server" 
                                                        Text='<%# Eval("AMC_End_KM", "{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ControlStyle-Width="22%" HeaderText="Inclusion">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCInclusion" runat="server" 
                                                        Text='<%# Eval("Inclusion") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Upload Date" ItemStyle-Width="8%"  >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrDate" runat="server" 
                                                        Text='<%# Eval("Cr_Date", "{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Active" ItemStyle-Width="1%"  >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActive" runat="server" 
                                                        Text='<%# Eval("Active") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                        </asp:GridView>
                    </asp:Panel>                    
                </asp:Panel>

            </td>
        </tr>

        <tr id="TmpControl">
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
</asp:Content>
