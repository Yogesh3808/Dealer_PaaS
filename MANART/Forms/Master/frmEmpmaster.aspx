<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmEmpmaster.aspx.cs" Inherits="MANART.Forms.Master.WebForm1" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td align="center" class="panel-title">
                <asp:Label ID="lblTitle" runat="server" Text="Employee Master"></asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td>
                <table id="ToolbarContainer" runat="server" border="1" width="100%">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
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
                                    CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Employee Header Details"
                                    ExpandedText="Employee Header Details" TextLabelID="lblTtlDealerHeaderDetails">
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
                                            <td style="width: 15%; padding-left: 10px">Emp Name :</td>
                                            <td style="width: 18%">
                                                <asp:TextBox ID="txtEmpnm" runat="server" CssClass="TextBoxForString"
                                                    Text=""></asp:TextBox>
                                                <asp:Label ID="Label2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                            </td>

                                            <td style="width: 15%; padding-left: 10px;" class="tdLabel">Emp Type:
                                            </td>
                                            <td style="width: 15%">
                                                <asp:DropDownList ID="ddlemptype" runat="server"
                                                    CssClass="ComboBoxFixedSize">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label3" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td style="width: 15%" class="tdLabel">Contact No:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                                Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                              
                                        </td>

                                            <td style="width: 15%" class="tdLabel">Active:
                                            </td>
                                            <td style="width: 15%">
                                                <asp:DropDownList ID="drpActive" runat="server" CssClass="ComboBoxFixedSize"
                                                    EnableViewState="true" Width="100px">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                                    <asp:ListItem Value="2">N</asp:ListItem>
                                                </asp:DropDownList>
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
                            <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtDealerCode" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
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
    </table>
</asp:Content>
