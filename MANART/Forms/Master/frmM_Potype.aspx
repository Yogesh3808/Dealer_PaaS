<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmM_Potype.aspx.cs" Inherits="MANART.Forms.Master.frmM_Potype" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
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
     <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="Po Type">
                </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

  <tr id="TblControl1">
            <td>
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" width="100%" runat="server">
                        <tr>
                            <td>
                                <uc2:Location ID="Location" runat="server" OnDDLSelectedIndexChanged="Location_DDLSelectedIndexChanged" />
                            </td>
                        </tr>

                    </table>
                    <tr id="TblControl">
                        <td style="width: 15%; height: 92px;">
                            <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                                    ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                                    Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                                    CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="PO Type Details"
                                    ExpandedText="PO Type Details" TextLabelID="lblTtlDealerHeaderDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                                    <table width="100%">
                                        <%-- Green Color Code- #91b900,  --%>
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Employee Header Details"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%" class="panel-title">
                                                <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                                    <table id="Table1" runat="server" class="table table-bordered">
                                        <tr>
                                            <td style="width: 15%; padding-left: 10px">Post Code :</td>
                                            <td style="width: 18%">
                                                <asp:TextBox ID="txtpocode" runat="server" CssClass="TextBoxForString" Enabled="false"
                                                    Text=""></asp:TextBox>
                                              
                                            </td>

                                            <td style="width: 15%; padding-left: 10px;" class="tdLabel">Prefix:
                                            </td>
                                            <td style="width: 15%">
                                                   <asp:TextBox ID="txtprefix" runat="server" CssClass="TextBoxForString" Enabled="false"
                                                    Text=""></asp:TextBox>
                                            
                                            </td>
                                        </tr>
                                        <tr>

                                            <td style="width: 15%" class="tdLabel">Active:
                                            </td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtactive" runat="server" CssClass="TextBoxForString" Enabled="false"
                                                    Text=""></asp:TextBox>
                                            </td>
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
                    <tr>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr id="TmpControl">
                        <td style="width: 15%">
                            <asp:TextBox ID="TextBox1" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="TextBox2" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="TextBox3" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtDealerCode" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="TextBox4" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="TextBox5" runat="server" CssClass="HideControl" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="HideControl">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtRecordUsed" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
                            <input id="hsiv" type="hidden" name="hsiv" runat="server" />
                            <input id="__ET1" type="hidden" name="__ET1" runat="server" />
                            <input id="__EA1" type="hidden" name="__EA1" runat="server" />
                            <input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />
                        </td>
                    </tr>
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
