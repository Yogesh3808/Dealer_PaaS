<%@ Page Title="MTI-Chassis Master" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableViewState="true" EnableEventValidation="false" CodeBehind="frmChassis.aspx.cs" Inherits="MANART.Forms.Master.frmChassis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>
    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>
    <script src="../../JavaScripts/jsProformaFunction.js" type="text/javascript"></script>
    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>
    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>
    <script src="../../JavaScripts/jsWCServiceHistory.js" type="text/javascript"></script>--%>

    <link href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.datepick.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsWCServiceHistory.js"></script>

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

    <script type="text/javascript">
        function disableBackButton() {
            window.history.forward(1);
        }
        function ShowServiceHistory1(objNewPartLabel, sDealerId, sJobCardNo) {
            var ChassisDetailsValues;

            //window.showModalDialog("../Admin/frmServiceHistoryDetails.aspx?DealerID=" + sDealerId + "&JobCardNo=" + sJobCardNo, "List", "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");

            ChassisDetailsValues = window.showModalDialog("../Admin/frmServiceHistoryDetails.aspx?DealerID=" + sDealerId + "&JobCardNo=" + sJobCardNo, "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 15%; height: 21px;">
                    <table id="ToolbarContainer" runat="server" width="100%" border="1">
                        <tr>
                            <td>
                                <uc5:Toolbar ID="ToolbarC" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 15%">
                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <div class="panel-body">
                            <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                                bIsCallForServer="true" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PChassisHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisHeaderDetails" runat="server" TargetControlID="CntChassisHeaderDetails"
                            ExpandControlID="TtlChassisHeaderDetails" CollapseControlID="TtlChassisHeaderDetails"
                            Collapsed="false" ImageControlID="ImgTtlChassisHeaderDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Chassis Header Details"
                            ExpandedText="Hide Chassis Header Details" TextLabelID="lblTtlChassisHeaderDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisHeaderDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlChassisHeaderDetails" runat="server" Text="Chassis Header Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntChassisHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="Table1" runat="server" class="" width="100%">
                                <tr>
                                    <td style="width: 15%" class="">Chassis No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Engine No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">In Warranty:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtInWarranty" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%">Original Model Code:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFertCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Original Model Name:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtModelName" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">Change Model Code:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtChangeFertCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">Change Model Name:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtChangeModelName" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">In Aggregate Warranty:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtInAggreWarranty" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%; height: 15px;">Model Category:
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtModelCategory" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%; height: 15px;">IN RMC:
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtInAMC" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%; height: 15px;" class="">Coupon No:
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtCoupanNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Coupons Blocked:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtCouponsBlocked" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                    <%-- <td style="width: 15%" class="">Model Group:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtModelGroup" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>--%>
                                </tr>
                                <tr>

                                    <td style="width: 15%" class="">IN Additional Warranty:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtInAdditionalWarranty" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">Reg No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Float Flag:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFloatFlag" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>


                                    <td style="width: 15%" class="">Vehicle Under Observation:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtVUO" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">In Comfort Plus :
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtInExtendedWarranty" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Direct Customer :
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtDirectCustomer" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>


                                    <td style="width: 15%" class="">In KAM:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtInKAM" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPInvoiceNo" runat="server" Text="SAP Invoice No:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPInvoiceNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPInvoiceDate" runat="server" Text="SAP Invoice Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPInvoiceDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>



                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSellingDealerCode" runat="server" Text="Selling Dealer Code:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSellDealCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblPDIDealerCode" runat="server" Text="PDI Dealer Code:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtPDIDealerCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%">
                                        <asp:LinkButton ID="lblServiceHistroy" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            onmouseover="SetCancelStyleonMouseOver(this);" Text="Service History" Width="80%"
                                            ToolTip="Service History Details"> </asp:LinkButton>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>

                                <tr id="tr1" visible="false">

                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSellingDealerName" runat="server" Text="Selling Dealer Name:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSellDealName" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%; height: 15px;" class="">
                                        <asp:Label ID="lblSAPInvoiceAmt" runat="server" Text="SAP Invoice Amt:"></asp:Label>
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtSAPInvoiceAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblTheftFlag" runat="server" Text="Theft Flag:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtTheftFlag" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>


                                </tr>
                                <tr id="tr2" visible="false">
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPSTNNo" runat="server" Text="SAP STN No:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPSTNNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPSTNDate" runat="server" Text="SAP STN Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPSTNDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="CommandButton" Visible="false" OnClick="btnSave_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <asp:Button ID="btnGenerate" runat="server" Text="GenerateXML" CssClass="CommandButton" Visible="false" OnClick="btnGenerate_Click" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr id="tr3" visible="false">
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblDANo" runat="server" Text="DA No:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtDANo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%; height: 15px;" class="">
                                        <asp:Label ID="lblDealerSpareCode" runat="server" Text="Dealer Spare Code:"></asp:Label>
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtSpareCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%; height: 15px;" class="">

                                        <asp:Label ID="lblDealerHDCode" runat="server" Text="Dealer HD Code:"></asp:Label>
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtHDCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PPortalDateDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" Visible="false">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntPortalDateDetails"
                            ExpandControlID="TtlPortalDateDetails" CollapseControlID="TtlPortalDateDetails"
                            Collapsed="true" ImageControlID="ImgTtlPortalDateDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Portal Received Date Details"
                            ExpandedText="Hide Portal Received Date Details" TextLabelID="lblTtlPortalDateDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlPortalDateDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlPortalDateDetails" runat="server" Text="Portal Received Date Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlPortalDateDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntPortalDateDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="Table4" runat="server" class="" width="100%">
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPSTNReceivedDate" runat="server" Text="SAP STN Received Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPSTNReceivedDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPVEHPOSTReceivedDate" runat="server" Text="SAP VEHPOST Received Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPVEHPOSTReceivedDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblDMSINSReceivedDate" runat="server" Text="DMS INS Received Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtDMSINSReceivedDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPINSPostedDate" runat="server" Text="SAP INS Posted Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPINSPostedDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblSAPINSReceivedDate" runat="server" Text="SAP INS Received Date:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAPINSReceivedDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>


                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblLastXMLCreateDate" runat="server" Text="Last XML Create Date :"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtLastXMLCreateDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label1" runat="server" Text="DCS INS Status:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtDCSINSStatus" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text=""></asp:TextBox>
                                    </td>

                                    <td style="width: 15%" class=""></td>
                                    <td style="width: 18%"></td>


                                    <td style="width: 15%" class=""></td>
                                    <td style="width: 18%"></td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PWarAMCDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEWarAMCDetails" runat="server" TargetControlID="CntWarAMCDetails"
                            ExpandControlID="TtlWarAMCDetails" CollapseControlID="TtlWarAMCDetails" Collapsed="true"
                            ImageControlID="ImgTtlWarAMCDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Show Warranty/RMC Details" ExpandedText="Hide Warranty/RMC Details"
                            TextLabelID="lblTtlWarAMCDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlWarAMCDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlWarAMCDetails" runat="server" Text="Warranty/RMC Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlWarAMCDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntWarAMCDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="tblRFPDetails" runat="server" class="" width="100%">
                                <tr>
                                    <td class="" style="width: 15%;">Warranty End Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtWarEndDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">Aggregate Warranty End Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAggreWarEndDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">Comfort Plus Start Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtExtWarStarDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%">Comfort Plus End Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtExtWarEndDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%;">Additional Warranty Start Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAddWarStartDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">Additional Warranty End Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAddWarEndDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">RMC Agreement No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAMCAggreNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">RMC Agreement Type:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAMCAggreType" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">RMC Start Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAMCStartDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">RMC End Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAMCEndDate" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%;">RMC Start KM:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAStartKM" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%;">RMC End KM:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSAEndKM" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">RMC Start Hrs:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtStartHrs" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%;">RMC End Hrs:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtEndHrs" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PCouponDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPECouponDetails" runat="server" TargetControlID="CntCouponDetails"
                            ExpandControlID="TtlCouponDetails" CollapseControlID="TtlCouponDetails"
                            Collapsed="true" ImageControlID="ImgTtlCouponDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Service Coupon Details"
                            ExpandedText="Hide Service Coupon Details" TextLabelID="lblCouponDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlCouponDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblCouponDetails" runat="server" Text="Coupon Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlCouponDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntCouponDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <asp:GridView ID="gvCouponDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="CouponID" ItemStyle-Width="20%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCouponID" runat="server" Text='<%# Eval("CouponID") %>' CssClass="GridTextBoxForString">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Coupon No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCouponNo" runat="server" Text='<%# Eval("CouponNo") %>' CssClass="GridTextBoxForString" MaxLength="18">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceName" runat="server" Text='<%# Eval("Serv_name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Coupon Amt" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLSPSD" runat="server" Text='<%# Eval("LSPSD") %>' CssClass="GridTextBoxForString" MaxLength="10">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MSPSD" ItemStyle-Width="10%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMSPSD" runat="server" Text='<%# Eval("MSPSD") %>' CssClass="GridTextBoxForString" MaxLength="10">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      
                                      <asp:TemplateField HeaderText="Effective From date" ItemStyle-Width="10%"  >
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEffectiveFromdate" runat="server" Text='<%# Eval("eff_from_date") %>' CssClass="GridTextBoxForString" MaxLength="10">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective To date" ItemStyle-Width="10%"  >
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEffectiveTodate" runat="server" Text='<%# Eval("Eff_to_date") %>' CssClass="GridTextBoxForString" MaxLength="10">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update CouponDetails" CssClass="CommandButton" OnClick="btnUpdate_Click" Visible="false" />
                        </asp:Panel>

                    </asp:Panel>
                    <asp:Panel ID="PAggregateDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEAggregateDetails" runat="server" TargetControlID="CntAggregateDetails"
                            ExpandControlID="TtlAggregateDetails" CollapseControlID="TtlAggregateDetails"
                            Collapsed="true" ImageControlID="ImgTtlAggregateDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Aggregate Details"
                            ExpandedText="Hide Aggregate Details" TextLabelID="lblTtlAggregateDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlAggregateDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlAggregateDetails" runat="server" Text="Document Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlAggregateDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntAggregateDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="Table2" runat="server" class="" width="100%">
                                <tr>
                                    <td class="" style="width: 15%;">Battery No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtBatteryNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Battery Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtBatteryMake" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">FIP No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFIPNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%">FIP Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFIPMake" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%;">Front Axle No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFrontAxleNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Front Axle Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFrontAxleMake" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Rear Axle1 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearAxle1No" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear Axle1 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearAxle1Make" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear Axle2 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearAxle2No" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Rear Axle2 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearAxle2Make" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear Axle2 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearAxle3No" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear Axle3 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearAxle3Make" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Transmission No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtTransNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Transmission Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtTransMake" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Gear Box No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtGearBoxNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Gear Box Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtGearBoxMake" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PChassisTyreDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisTyreDetails" runat="server" TargetControlID="CntChassisTyreDetails"
                            ExpandControlID="TtlChassisTyreDetails" CollapseControlID="TtlChassisTyreDetails"
                            Collapsed="true" ImageControlID="ImgTtlChassisTyreDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Chassis Tyre Details"
                            ExpandedText="Hide Chassis Tyre Details" TextLabelID="lblTtlChassisTyreDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisTyreDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlChassisTyreDetails" runat="server" Text="Chassis Tyre Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisTyreDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntChassisTyreDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="Table3" runat="server" class="" width="100%">
                                <tr>
                                    <td class="" style="width: 15%;">Tyre size front :
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFrontLeftTyreNo" runat="server" CssClass="TextBoxForString" Height="80%"
                                            Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Tyre Size Rear :
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFrontRightTyreNo" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%;">Tyre Size Spare:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftOuterTyreAxle1No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Front Tyre No LHS:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFRONT_TYRE_NO_LHS" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Front Tyre No RHS:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFRONT_TYRE_NO_RHS" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightInner Tyre Axle1 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightInnerTyreAxle1No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Rear LeftOuter Tyre Axle2 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftOuterTyreAxle2No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftInner Tyre Axle2 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftInnerTyreAxle2No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightOuter Tyre Axle2 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightOuterTyreAxle2No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Rear RightInner Tyre Axle2 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightInnerTyreAxle2No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftOuter Tyre Axle3 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftOuterTyreAxle3No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftInner Tyre Axle3 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftInnerTyreAxle3No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%;">Rear RightOuter Tyre Axle3 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightOuterTyreAxle3No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightInner Tyre Axle3 No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightInnerTyreAxle3No" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%" visible="false">Front Left Tyre Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFrontLeftTyreMake" runat="server" CssClass="HideControl"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr visible="false">
                                    <td class="" style="width: 15%">Front Right Tyre Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFrontRightTyreMake" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftOuter Tyre Axle1 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftOuterTyreAxle1Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftInner Tyre Axle1 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftInnerTyreAxle1Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr visible="false">
                                    <td class="" style="width: 15%;">Rear RightOuter Tyre Axle1 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightOuterTyreAxle1Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightInner Tyre Axle1 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightInnerTyreAxle1Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftOuter Tyre Axle2 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftOuterTyreAxle2Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr visible="false">
                                    <td class="" style="width: 15%;">Rear LeftInner Tyre Axle2 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftInnerTyreAxle2Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightOuter Tyre Axle2 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightOuterTyreAxle2Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightInner Tyre Axle2 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightInnerTyreAxle2Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr visible="false">
                                    <td class="" style="width: 15%;">Rear LeftOuter Tyre Axle3 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftOuterTyreAxle3Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear LeftInner Tyre Axle3 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearLeftInnerTyreAxle3Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td class="" style="width: 15%">Rear RightOuter Tyre Axle3 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightOuterTyreAxle3Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr visible="false">
                                    <td class="" style="width: 15%;">Rear RightInner Tyre Axle3 Make:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRearRightInnerTyreAxle3Make" runat="server" CssClass="TextBoxForString"
                                            Height="80%" Text="" ReadOnly="true"></asp:TextBox></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisFSDetails" runat="server" TargetControlID="CntChassisFSDetails"
                            ExpandControlID="TtlChassisFSDetails" CollapseControlID="TtlChassisFSDetails"
                            Collapsed="true" ImageControlID="ImgTtlChassisFSDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Service Details"
                            ExpandedText="Hide Service Details" TextLabelID="lblTtlChassisFSDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisFSDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlChassisFSDetails" runat="server" Text="Service Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisFSDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>

                        </asp:Panel>
                        <asp:Panel ID="CntChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <table width="100%">
                                <tr align="right">
                                    <td class="">Part :
                                    <asp:TextBox ID="txtTotPart" runat="server" CssClass="TextBoxForString"></asp:TextBox></td>
                                    <td class="">Labor :
                                    <asp:TextBox ID="txtTotLabour" runat="server" CssClass="TextBoxForString"></asp:TextBox></td>
                                    <td class="">Lubricant :
                                    <asp:TextBox ID="txtTotLubricant" runat="server" CssClass="TextBoxForString"></asp:TextBox></td>
                                    <td class="">Sublet :
                                    <asp:TextBox ID="txtTotSublet" runat="server" CssClass="TextBoxForString"></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvFSDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true"
                                OnRowCommand="gvFSDetails_RowCommand" OnRowDataBound="gvFSDetails_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20%" HeaderStyle-CssClass="HideControl " ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerID" runat="server" Text='<%# Eval("Dealer_ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkServName" runat="server" Text='<%# Eval("Serv_Name") %>' CommandArgument='<%# Eval("Jobcard_no") %>' CommandName="ServiceName" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Coupon No" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCouponNo" runat="server" Text='<%# Eval("coupon_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jobcard No" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobcardNo" runat="server" Text='<%# Eval("Jobcard_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jobcard Date" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobcardDate" runat="server" Text='<%# Eval("Jobcard_date","{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Kms" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKms" runat="server" Text='<%# Eval("Kms") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hours" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHrs" runat="server" Text='<%# Eval("Hrs") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer_Spares_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PChassisFloatDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisFloatDetails" runat="server" TargetControlID="CntChassisFloatDetails"
                            ExpandControlID="TtlChassisFloatDetails" CollapseControlID="TtlChassisFloatDetails"
                            Collapsed="true" ImageControlID="ImgTtlChassisFloatDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Float Details"
                            ExpandedText="Hide Float Details" TextLabelID="lblTtlChassisFloatDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisFloatDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlChassisFloatDetails" runat="server" Text="Float Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisFloatDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntChassisFloatDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <asp:GridView ID="gvFloatDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Jobcard No" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobcardNo" runat="server" Text='<%# Eval("Jobcard_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jobcard Date" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobcardDate" runat="server" Text='<%# Eval("Jobcard_date","{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GRN No" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGRNNo" runat="server" Text='<%# Eval("GRN_No") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GRN Date" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGRNDate" runat="server" Text='<%# Eval("GRN_date","{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActive" runat="server" Text='<%# Eval("Active") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Code" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartCode" runat="server" Text='<%# Eval("parts_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer_Spares_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="POwnershipDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEOwnershipDetails" runat="server" TargetControlID="CntOwnershipDetails"
                            ExpandControlID="TtlOwnershipDetails" CollapseControlID="TtlOwnershipDetails"
                            Collapsed="true" ImageControlID="ImgTtlOwnershipDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Ownership Details"
                            ExpandedText="Hide Ownership Details" TextLabelID="lblTtlOwnershipDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlOwnershipDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlOwnershipDetails" runat="server" Text="Ownership Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlOwnershipDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntOwnershipDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <asp:GridView ID="gvOwnerDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("Customer_name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Phone No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhoneNo" runat="server" Text='<%# Eval("Phone1") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("Sale_Invoice_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblnvoiceDate" runat="server" Text='<%# Eval("Sale_Invoice_date", "{0:dd-MM-yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInv_Amt" runat="server" Text='<%# Eval("INV_Amt", "{0:00.00}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI Invoice No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVECVInvoiceNo" runat="server" Text='<%# Eval("VECV_Sale_Invoice_no") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI Invoice Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVECVInvoiceDate" runat="server" Text='<%# Eval("VECV_Sale_Invoice_date", "{0:dd-MM-yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="SRN No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNNo" runat="server" Text='<%# Eval("SRN_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SRN Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNDate" runat="server" Text='<%# Eval("SRN_date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActive" runat="server" Text='<%# Eval("active") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PInstallationDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEInstallationDetails" runat="server" TargetControlID="CntInstallationDetails"
                            ExpandControlID="TtlInstallationDetails" CollapseControlID="TtlInstallationDetails"
                            Collapsed="true" ImageControlID="ImgTtlInstallationDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Installation Details"
                            ExpandedText="Hide Installation Details" TextLabelID="lblTtlInstallationDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlInstallationDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlInstallationDetails" runat="server" Text="Installation Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlInstallationDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntInstallationDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <asp:GridView ID="gvinstall" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Installation No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstallNo" runat="server" Text='<%# Eval("INS_No") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SAP Installation No" ItemStyle-Width="12%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSAPInstallNo" runat="server" Text='<%# Eval("SAP_INS_No") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Installation Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstallDate" runat="server" Text='<%# Eval("INS_date","{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--                                <asp:TemplateField HeaderText="SAP Installation Date" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSAPInstallDate" runat="server" Text='<%# Eval("SAP_INS_Date","{0:dd/MM/yyyy}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustName" runat="server" Text='<%# Eval("Customer_name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Type" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustType" runat="server" Text='<%# Eval("CustType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Primary Application" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPriCode" runat="server" Text='<%# Eval("pri_app_desc") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Secondary Application" ControlStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSecCode" runat="server" Text='<%# Eval("sec_app_desc") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Road Type" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoadType" runat="server" Text='<%# Eval("Road_type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Route Type" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRouteType" runat="server" Text='<%# Eval("Route_Type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Financier Type" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinType" runat="server" Text='<%# Eval("FinType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="kms" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblkms" runat="server" Text='<%# Eval("Kms") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hours" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblhrs" runat="server" Text='<%# Eval("hrs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 15%"></td>
            </tr>
            <tr id="TmpControl" style="display: none">
                <td style="width: 15%">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                            Text=""></asp:TextBox><asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                                Text=""></asp:TextBox><asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                                </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
