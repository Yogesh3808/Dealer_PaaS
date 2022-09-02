<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmCouponClaimSelection.aspx.cs" Inherits="MANART.Forms.Coupon.frmCouponClaimSelection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="../../WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="CustomPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/CustomPager.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsCouponClaimProcessing.js"></script>

    <script type="text/javascript">
        function OnStateChange() {
            var trDate = document.getElementById("<%=trDate.ClientID%>");
            var drpClaimStatus = document.getElementById("<%=drpClaimStatus.ClientID %>");
            var ClaimStatusVal = drpClaimStatus.options[drpClaimStatus.selectedIndex].text
            if (ClaimStatusVal == "Pending")
                trDate.style.display = "none"
            else
                trDate.style.display = ""
        }
        function ClearSearch() {
            document.getElementById("<%=txtSearchText.ClientID %>").value = '';
        }
        function CheckText(SourceID) {
            var hidSourceID =
            document.getElementById("<%=hidSourceID.ClientID%>");
        hidSourceID.value = SourceID;

        var txtSearchText = document.getElementById("<%=txtSearchText.ClientID %>")
        if (txtSearchText.value == '')
            return false;
    }
    </script>
    <script type="text/javascript">

        function pageLoad() {
            $(document).ready(function () {
                var txtFromDate = document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate");
                var txtToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtToDate_txtDocDate");
                $('#ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '01/09/2015', maxDate: txtToDate.value
                });

                $('#ctl00_ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtFromDate.value
                });

                function customRange(dates) {
                    if (this.id == 'ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate') {
                        $('#ctl00_ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                    }
                    else {
                        $('#ctl00_ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:Location ID="Location" runat="server" />
                </asp:Panel>
                <asp:Panel ID="Selection" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" runat="server" class="ContainTable" border="0">
                        <tr>
                            <td style="width: 15%" class="tdLabel">Claim Status:
                            </td>
                            <td style="width: 18%">
                                <asp:DropDownList ID="drpClaimStatus" runat="server"
                                    CssClass="ComboBoxFixedSize"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="drpClaimStatus_SelectedIndexChanged">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="P">Pending</asp:ListItem>
                                    <asp:ListItem Value="Y">Approved</asp:ListItem>
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel" style="width: 15%"></td>
                            <td style="width: 18%"></td>
                            <td class="tdLabel" style="width: 15%"></td>
                            <td style="width: 18%"></td>
                        </tr>

                        <tr id="trDate" runat="server">
                            <td class="tdLabel" style="width: 15%">From:
                            </td>
                            <td style="width: 18%">
                                <uc3:CurrentDate ID="txtFromDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                            </td>
                            <td style="width: 15%" class="tdLabel">To:
                            </td>
                            <td style="width: 18%">
                                <uc3:CurrentDate ID="txtToDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                            </td>

                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%">&nbsp;
                            </td>
                            <td style="width: 12%" class="tdLabel"></td>
                            <td class="tdLabel" style="width: 15%">
                                <asp:Button ID="btnShow" runat="server" CssClass="CommandButton" OnClick="btnShow_Click"
                                    Text="Show" />
                            </td>
                            <td style="width: 18%" colspan="2">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>

                            </td>
                            <td class="tdLabel" style="width: 15%">&nbsp;
                            </td>
                            <td style="width: 18%">&nbsp;
                            </td>
                        </tr>

                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    ScrollBars="Vertical">
                    <table id="Table1" runat="server" class="ContainTable" border="1">
                        <tr>
                            <td id="Td1" align="center" style="height: 15px">Search By Claim No or Claim Type
                                <asp:TextBox ID="txtSearchText" runat="server"></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" CssClass="CommandButton"
                                    Text="Search" OnClick="btnSearch_Click" OnClientClick="return CheckText(this.id)" />
                                <asp:Button ID="btnClearSearch" runat="server" CssClass="CommandButton" Text="Clear Search" OnClientClick="return ClearSearch()" OnClick="btnClearSearch_Click" />
                                &nbsp;&nbsp;<asp:Label ID="lblSearch" runat="server" Text="Ignore above filter when you search Claim No or Claim Type." Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td id="ClaimGrid" align="center" class="ContaintTableHeader" style="height: 15px"></td>
                        </tr>
                        <tr>
                            <td style="width: 18%;">
                                <asp:Panel ID="GridDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                    <asp:GridView ID="CouponClaimGrid" GridLines="Horizontal" Width="100%"
                                        AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                        AutoGenerateColumns="False" AllowPaging="true"
                                        OnPageIndexChanging="CouponClaimGrid_PageIndexChanging" runat="server"
                                        OnRowDataBound="CouponClaimGrid_RowDataBound">
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png"
                                                        OnClientClick="return ShowCouponClaim(this);" OnClick="ImgSelect_Click" Style="height: 16px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" /><%--Text='<%# Container.DataItemIndex + 1  %>'--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dlr_Code") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Claim Type" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClaimType" runat="server" Text='<%# Eval("ClaimType") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="Claim No" ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClaimNo" runat="server" Text='<%# Eval("Coupon_Claim_no") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Claim Date" ItemStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClaimDate" runat="server" Text='<%# Eval("Claim_Date") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Claim Status" ItemStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClaimStatus" runat="server" Text='<%# Eval("Claim_Status") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>


                                        </Columns>
                                    </asp:GridView>

                                    <%--<asp:ObjectDataSource ID="ObjDSource" runat ="server" 
                                                MaximumRowsParameterName ="MaxRowCount" EnablePaging="true" 
                                            StartRowIndexParameterName ="StartIndexRow" SelectCountMethod="DataCount" 
                                                SelectMethod="GetWarrantyClaimUserWise" TypeName ="clsWarranty" onselecting="ObjDSource_Selecting"
                                               EnableCaching="false"   
                                             >
                                             
                                            <SelectParameters>
                                            
                                            <asp:Parameter Name="sRegionID" Type="Int32" />
                                            <asp:Parameter Name="sContryID" Type ="Int32" />
                                            <asp:Parameter Name="sDealerId" Type="String" />
                                            <asp:Parameter Name="sFromDate" Type ="String" />
                                            <asp:Parameter Name="sToDate" Type="String" />
                                            <asp:Parameter Name="sRequestOrClaim" Type ="String" />
                                            <asp:Parameter Name="iClaimStatus" Type="Int32" />
                                            <asp:Parameter Name="iUserRoleId" Type ="Int32" />
                                            <asp:Parameter Name="sDomestic_Export" Type="String" />
                                            <asp:Parameter Name="sSearchText" Type ="String" />
                                            <asp:Parameter Name="StartIndexRow" Type="Int32" />
                                            <asp:Parameter Name="MaxRowCount" Type ="Int32" />
                                            </SelectParameters>
                                            </asp:ObjectDataSource>--%>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="PagerTableTD"><strong>
                                <CustomPager:PagerV2_8 ID="objCustomPager" runat="server" NextClause="..."
                                    PreviousClause="..." NormalModePageCount="10" CompactModePageCount="10" ShowResultClause="false" GeneratePagerInfoSection="false"
                                    MaxSmartShortCutCount="0"
                                    OnCommand="objCustomPager_Command" />
                            </strong>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtRequestOrClaim" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hidSourceID" runat="server" />
                <asp:HiddenField ID="hdnDealers" runat="server" Value="" />
                <asp:HiddenField ID="hdnFirstSlabamt" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPagecount" runat="server" Value="0" />
            </td>
        </tr>
    </table>
</asp:Content>
