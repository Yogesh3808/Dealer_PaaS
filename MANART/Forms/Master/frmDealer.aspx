<%@ Page Title="MTI-Dealer Master" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableViewState="true" CodeBehind="frmDealer.aspx.cs" Inherits="MANART.Forms.Master.frmDealer" %>

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

        function GoLivevalidate() {
            var txtDealerOrigin = document.getElementById("ContentPlaceHolder1_txtDealerOrigin");
            
            var DrpDistrict = document.getElementById("ContentPlaceHolder1_DrpDistrict");
            if (DrpDistrict.value == "0" && txtDealerOrigin.value.trim() == "Domestic") {
                alert("Please Select Dealer District!");
                DrpDistrict.focus();
                return false;
            }

            var DrpWarehouse = document.getElementById("ContentPlaceHolder1_DrpWarehouse");
            if (DrpWarehouse.value == "0") {
                alert("Please Select Dealer Warehouse!");
                DrpWarehouse.focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1" style="width:100%;">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle" align="center">
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title">
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
            <td style="width: 15%;">
                <%--height: 92px;"--%>
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
                                <td align="center" width="96%">
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
                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="None">
                        <table id="Table1" runat="server" class="table table-bordered">
                            <tr>
                                <td style="width: 10%" class="">Dealer Name:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerName" runat="server" CssClass="TextBoxForString" ReadOnly="false"></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Dealer Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtVehicleCode" runat="server" ReadOnly="false"></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">City:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtCity" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="HideControl">
                                <td style="width: 10%" class="">Spares Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtSparesCode" runat="server" ReadOnly="false"></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">HD Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtHDCode" runat="server" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">SAP Hierarchy Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtHierarchyCode" runat="server" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="" style="width: 10%">District:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtDistrict" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtDistrict" runat="server" ReadOnly="false"  Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">State:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtState" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtState" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td class="" style="width: 10%" rowspan="2">Address :
                                </td>
                                <td style="width: 20%" rowspan="2">
                                    <%--Sujata 24082011<asp:TextBox ID="txtAddress" runat="server" CssClass="MultilineTextbox" ReadOnly="false"
                                        Text="" TextMode="MultiLine" Rows="3" Width="209px"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtAddress" runat="server" ReadOnly="false"
                                        Enabled="false" Text="" TextMode="MultiLine" Rows="2" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="" style="width: 10%">Country:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtCountry" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Dealer Depot:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtDealerDepot" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtDealerDepot" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="">Dealer Region:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtDealerRegion" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtDealerRegion" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Dealer Mobile:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtDealerMobile" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtDealerMobile" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">
                                    <%--Credit Limit--%>
                                    HO Branch:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtHOBranch" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <%--<asp:TextBox ID="txtCreditLimit" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtHOBranch" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="">Landline Phone:
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:TextBox ID="txtLandLinePhone" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtLandLinePhone" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Email:
                                </td>
                                <td style="width: 20%">
                                    <%--Sujata 24082011
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtEmail" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>
                                </td>
                                <td style="width: 10%" class="">MD Email:
                                </td>
                                <td style="width: 20%">
                                    <%--Sujata 24082011
                                        <asp:TextBox ID="txtMDEmail" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtMDEmail" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="" style="width: 10%">Dealer Type:
                                </td>
                                <td style="width: 20%">
                                    <%--Sujata 24082011
                                        <asp:TextBox ID="txtDealerType" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:DropDownList ID="DrpDealerType" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="True" OnSelectedIndexChanged="DrpDealerType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%-- sujata 28082011--%>
                                </td>
                                
                                <td style="width: 10%" class="">Dealer Origin:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerOrigin" runat="server" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Active:
                                </td>
                                <td style="width: 20%">
                                    <%-- sujata 28082011--%>
                                    <%-- <asp:TextBox ID="txtActive" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtActive" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>
                                </td>
                            </tr>
                            <tr class="HideControl">
                                <td style="width: 10%" class="">Dealer Category:
                                </td>
                                <td style="width: 20%">
                                    <%--Sujata 24082011
                                        <asp:TextBox ID="txtDealerCategory" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtDealerCategory" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                    <%-- sujata 28082011--%>
                                </td>
                                <td style="width: 10%" class="">Sales Office:
                                </td>
                                <td style="width: 20%">
                                    <%--sujata 24082011
                                    <asp:TextBox ID="txtSalesOffice" runat="server" CssClass="TextBoxForString" ReadOnly="false" Enabled="false" 
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtSalesOffice" runat="server" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">
                                    <%-- sujata 28082011 Extended Warranty:--%>
                                    Under Distributor:
                                </td>
                                <td style="width: 20%">
                                    <%-- sujata 28082011
                                    <asp:TextBox ID="txtExtendedWarr" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:DropDownList ID="drpDistributor" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtExtendedWarr" runat="server" CssClass="DispalyNon" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="width: 10%" class="">PAN No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtPANNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">TIN No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtTINNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">VAT No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtVATNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="HideControl">
                                <td style="width: 10%" class="">LVA No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtLVANo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">IRC No:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtIRCNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Service Tax Type:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtServiceTaxType" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="HideControl">
                                 <td style="width: 10%" class="">HO Code:
                                </td>
                                <td style="width: 20%">
                                    <%--Sujata 24082011
                                        <asp:TextBox ID="txtHOCode" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>--%>
                                    <asp:TextBox ID="txtHOCode" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Enabled="false" Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Reman Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtRemanCode" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">BUS Code:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtBusCode" runat="server" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="HideControl">
                                <td style="width: 10%" class="">Dealer Report Region :
                                </td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="drpDealerRepRegion" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%" class="" visible="false">Dealer Short Name:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerShortName" runat="server" CssClass="HideControl" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="" visible="false">Dealer Territory:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDealerTerritory" runat="server" CssClass="HideControl" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="">Dealer Live:
                                </td>
                                <td style="width: 20%">
                                  <asp:TextBox ID="txtDealerLive" runat="server" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">Dealer Live Date:
                                </td>
                                <td style="width: 20%">
                                 <asp:TextBox ID="txtDealerLiveDt" runat="server" ReadOnly="false"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 10%" class="">
                                    <asp:Label ID="lblDistrict" runat="server" Text="District"></asp:Label>
                                </td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="DrpDistrict" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="width: 10%" class="">
                                     <asp:Label ID="lblWarehouse" runat="server" Text="Warehouse"></asp:Label>
                                </td>
                                <td style="width: 20%">
                                  <asp:DropDownList ID="DrpWarehouse" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%" class="">
                                </td>
                                <td style="width: 20%">
                                     <asp:Button ID="btnGoLive" runat="server" OnClientClick="return GoLivevalidate();" Text="Go Live"
                                     TabIndex="12" CssClass="CommandButton btn btn-primary" OnClick="btnGoLive_Click" />
                                </td>
                                <td style="width: 10%" class="">
                                    
                                </td>
                                <td style="width: 20%">
                                    
                                </td>
                                
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="None">
                    <div class="scrolling-table-container">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 15%"></td>
        </tr>
        <tr id="TmpControl" style="display: none">
            <td style="width: 15%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtDlrStateID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtDistrictID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
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
