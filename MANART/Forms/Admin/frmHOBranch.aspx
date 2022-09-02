<%@ Page Title="MTI-HO-Branch Link" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmHOBranch.aspx.cs" Inherits="MANART.Forms.Admin.frmHOBranch" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text="Ho-Branch Creation">
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
            <tr>
                <td style="width: 14%">

                    <asp:Panel ID="TtlLiveHOBRANCHDeatils" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="Table3" runat="server" class="ContainTable">
                            <tr>
                                <td style="width: 18%">
                                    <asp:Label ID="Label1" runat="server" Text="Select HO/Branch:"></asp:Label>
                                </td>

                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpHOBranch" runat="server" CssClass="ComboBoxFixedSize"
                                        Width="150px" OnSelectedIndexChanged="drpHOBranch_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1" Selected>HO</asp:ListItem>
                                        <asp:ListItem Value="2">Branch</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 18%">
                                    <asp:Label ID="lblDLHOBranchCode" runat="server" Text="HO/Branch Code"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtHOBranchCode" Text="" runat="server" CssClass="TextForAmount"
                                        ReadOnly="false" MaxLength="2" Width="100px"></asp:TextBox>
                                </td>



                            </tr>
                            <tr>
                                <td style="width: 18%">
                                    <asp:Label ID="lblHOBranchDealerID" runat="server" Text="Select Dealer"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpHOBranchDealerName" runat="server" Width="300px" CssClass="ComboBoxFixedSize"
                                        EnableViewState="true" AutoPostBack="True"
                                        OnSelectedIndexChanged="drpHOBranchDealerName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 18%">
                                    <asp:Label ID="lblHODealerID" runat="server" Text="Select HO Dealer"></asp:Label>
                                </td>
                                <td>

                                    <asp:DropDownList ID="drpHODealerName" runat="server" Width="300px" CssClass="ComboBoxFixedSize"
                                        EnableViewState="true" AutoPostBack="True"
                                        OnSelectedIndexChanged="drpHODealerName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 18%">
                                    <asp:Label ID="Label2" runat="server" Text="Select Dealer Location Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpDealerLocationType" runat="server" Width="200px" CssClass="ComboBoxFixedSize"
                                        EnableViewState="true" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 18%"></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>

                                    <asp:Button ID="btnHOBranchUpdate" runat="server" CssClass="CommandButton" Text="Add HO/Branch Entry"
                                        OnClick="btnHOBranchUpdate_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>
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
        </table>
    </div>
</asp:Content>
