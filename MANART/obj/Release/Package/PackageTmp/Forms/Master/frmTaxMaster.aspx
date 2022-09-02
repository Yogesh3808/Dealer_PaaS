<%@ Page Title="MTI-Tax Master" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmTaxMaster.aspx.cs" Inherits="MANART.Forms.Master.frmTaxMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
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

        function refresh() {
            if (event.keyCode == 116) {
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
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="Tax"> </asp:Label>
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
            <td style="width: 15% Height: 45%">

                 <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" width="100%" runat="server">
                        <tr>
                            <td>
                                <uc2:Location ID="Location" runat="server" OndrpCountryIndexChanged="Location_drpCountryIndexChanged" OndrpRegionIndexChanged="Location_drpRegionChanged" />
                            </td>
                        </tr>
                      
                    </table>
                   
                </asp:Panel>
                <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                        ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Tax Master Details"
                        ExpandedText="Tax Master Details" TextLabelID="lblTtlPartHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartHeaderDetails" runat="server" Text="Tax Master Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>

                    <asp:Panel ID="CntPartHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Tax Description:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtTaxDesc" runat="server" CssClass="TextBoxForString" Width="60%"
                                        Text="" ReadOnly="true"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td style="width: 15%" class="tdLabel">Tax Percentage:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtTaxPercentage" runat="server" CssClass="TextBoxForString" Width="60%"
                                        Text="" ReadOnly="true"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td style="width: 15%; height: 17px;" class="tdLabel">Tax Type:
                                </td>
                                <td style="width: 20%; height: 17px;">
                                    <asp:DropDownList ID="drpTaxType" runat="server" Width="60%" AutoPostBack ="true" 
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpTaxType_SelectedIndexChanged1">
                                    </asp:DropDownList>
                                    <b class="Mandatory">*</b>
                                </td>

                                
                            </tr>
                            <tr>
                                <td style="width: 15%; height: 17px;" class="tdLabel">Category:
                                </td>

                                <td style="width: 20%; height: 17px;">
                                    <asp:DropDownList ID="drpTaxCategory" runat="server" Width="60%"
                                        CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="drpTaxCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td style="width: 15%; height: 17px;" class="tdLabel">Tax Applicable on:
                                </td>
                                <td style="width: 20%; height: 17px;">
                                    <asp:DropDownList ID="drpTaxApplicable" runat="server" Width="60%"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <b class="Mandatory">*</b>
                                </td>
                                  <td class="tdLabel" style="width: 15%">Is Service Tax :
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="drpServiceTax" runat="server" Width="60%"
                                        CssClass="ComboBoxFixedSize">
                                       <%-- <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="Y" >Service Tax</asp:ListItem>
                                        <asp:ListItem Value="N">VAT/CST</asp:ListItem>
                                        <asp:ListItem Value="T">TCS Tax</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                               

                            </tr>
                              <tr>
                                 <td class="tdLabel" style="width: 15%">Active:
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="drpActive" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" EnableViewState="true" Width="100px">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1" Selected>Y</asp:ListItem>
                                        <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label17" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                  <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 20%"></td>
                                  <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 20%"></td>
                            </tr>
                            </tr>
                           
                             <tr runat="server" id="trAddTaxId" visible="false">
                                <td style="width: 15%" class="tdLabel">
                                     <asp:Label ID="lblAdd1" runat="server" Text="Additional Sales Tax 1:"></asp:Label>
                                   <%-- Additional Sales Tax 1:--%>
                                </td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="drpAdd1" runat="server" Width="60%"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>

                                <td style="width: 15%" class="tdLabel">
                                     <asp:Label ID="lblAdd2" runat="server" Text="Additional Sales Tax 2:"></asp:Label>
                                   <%-- Additional Sales Tax 2:--%>
                                </td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="drpAdd2" runat="server" Width="60%"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>

                                <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 20%"></td>
                            </tr>

                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />
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
