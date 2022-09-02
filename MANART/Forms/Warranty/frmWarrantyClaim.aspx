<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableEventValidation="false" CodeBehind="frmWarrantyClaim.aspx.cs" Inherits="MANART.Forms.Warranty.frmWarrantyClaim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .WordWrap {
            width: 100%;
            word-break: break-all;
        }

        .WordBreak {
            width: 100px;
            OVERFLOW: hidden;
            TEXT-OVERFLOW: ellipsis;
        }
    </style>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>

    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script src="../../Scripts/jsWarrantyFunction.js"></script>
    <script src="../../Scripts/jsWCPartFunc.js"></script>
    <script src="../../Scripts/jsWCLabourFunc.js"></script>
    <script src="../../Scripts/jsWCLubricantFunc.js"></script>
    <script src="../../Scripts/jsWCJobCode.js"></script>
   <%-- <script src="../../Scripts/jsWCFileAttach.js"></script>--%>
    <script src="../../Scripts/jsFileAttach.js"></script>
    <script src="../../Scripts/jsWCServiceHistory.js"></script>
    <script>
        function ShowAttachDocument(objFileControl) {
            //debugger;
            var objRow = objFileControl.parentNode.parentNode.childNodes;
            var sFileName = '';
            var sUserType = '';
            popht = 3; // popup height
            popwth = 3; // popup width
            var sDealerCode = document.getElementById('ContentPlaceHolder1_txtDealerCode').value;
            var txtUserType = document.getElementById('ContentPlaceHolder1_txtUserType');
            if (txtUserType != null) {
                sUserType = txtUserType.value;
            }
            sFileName = (objRow[4].children[0].innerText);
            var scrleft = (screen.width / 2) - (popwth / 2) - 80; //centres horizontal
            var scrtop = ((screen.height / 2) - (popht / 2)) - 40; //centres vertical
            window.open("../Spares/frmOpenAttachDocument.aspx?FileName=" + sFileName + "&DealerCode=" + sDealerCode + "&UserType=" + sUserType, "List", "top=" + scrtop + ",left=" + scrleft + ",width=1px,height=3px");
        }

        function CheckInvDescValueAlreadyUsedInGrid(Objcontrol) {
            debugger;
            var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[1].innerText);
            var objGrid = Objcontrol.parentNode.parentNode.parentNode;
            var sSelecedValue = Objcontrol.options[Objcontrol.selectedIndex].text;
            if (sSelecedValue != "--Select--" && sSelecedValue != "0") {
                for (i = 1; i < objGrid.children.length; i++) {
                    ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

                    if (i != iRowOfSelectControl) {
                        if (sSelecedValue == ObjRecord.options[ObjRecord.selectedIndex].text) {
                            alert("Record is already selected at line No." + i);
                            Objcontrol.selectedIndex = 0;
                            Objcontrol.focus();
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
                var txtClaimDate = document.getElementById("ContentPlaceHolder1_txtClaimDate_txtDocDate");
                var txtRepairOrderDate = document.getElementById("ContentPlaceHolder1_txtRepairOrderDate_txtDocDate");
                var txtRepairCompleteDate = document.getElementById("ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate");
                var txtFailureDate = document.getElementById("ContentPlaceHolder1_txtFailureDate_txtDocDate");
                var txtInstallationDate = document.getElementById("ContentPlaceHolder1_txtInstallationDate_txtDocDate");
                var txtInvoiceDate = document.getElementById("ContentPlaceHolder1_txtInvoiceDate_txtDocDate");
                var hdnPostShipmentDate = document.getElementById("ContentPlaceHolder1_hdnPostShipmentDate");
                var hdnMinClaimDate = document.getElementById("ContentPlaceHolder1_hdnMinClaimDate");
                var splInsDate = txtInstallationDate.value.split("/")
                var InsDate = new Date(splInsDate[2], splInsDate[1], splInsDate[0] - 1);

                $('#ContentPlaceHolder1_txtInstallationDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (hdnPostShipmentDate.value != '') ? hdnPostShipmentDate.value : '-5y', maxDate: '0d'

                });
                $('#ContentPlaceHolder1_txtInvoiceDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (hdnPostShipmentDate.value != '') ? hdnPostShipmentDate.value : '-5y', maxDate: (txtInstallationDate.value != '') ? InsDate : '0d'

                });
                $('#ContentPlaceHolder1_txtClaimDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtClaimDate.value != '') ? txtClaimDate.value : (hdnMinClaimDate.value != '') ? hdnMinClaimDate.value : (txtRepairCompleteDate.value != '') ? txtRepairCompleteDate.value : (txtRepairOrderDate.value != '') ? txtRepairOrderDate.value : (txtFailureDate.value != '') ? txtFailureDate.value : txtInstallationDate.value, maxDate: '0d'

                });
                $('#ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtClaimDate.value, minDate: (txtRepairOrderDate.value != '') ? txtRepairOrderDate.value : (txtFailureDate.value != '') ? txtFailureDate.value : (txtInstallationDate.value != "") ? txtInstallationDate.value : hdnPostShipmentDate.value

                });

                $('#ContentPlaceHolder1_txtRepairOrderDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: (txtRepairCompleteDate.value == '') ? txtClaimDate.value : txtRepairCompleteDate.value, minDate: (txtFailureDate.value != '') ? txtFailureDate.value : (txtInstallationDate.value != "") ? txtInstallationDate.value : hdnPostShipmentDate.value

                });

                $('#ContentPlaceHolder1_txtFailureDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtInstallationDate != null) ? (txtInstallationDate.value != "") ? txtInstallationDate.value : hdnPostShipmentDate.value : '', maxDate: (txtRepairOrderDate.value != '') ? txtRepairOrderDate.value : (txtRepairCompleteDate.value != '') ? txtRepairCompleteDate.value : txtClaimDate.value

                });
                function customRange(dates) {
                    if (this.id == 'ContentPlaceHolder1_txtClaimDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtRepairOrderDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtFailureDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }
                    else if (this.id == 'ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtClaimDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtRepairOrderDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }
                    else if (this.id == 'ContentPlaceHolder1_txtRepairOrderDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtClaimDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtFailureDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }
                    else if (this.id == 'ContentPlaceHolder1_txtFailureDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtClaimDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                        $('#ContentPlaceHolder1_txtRepairOrderDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                    }
                    else if (this.id == 'ContentPlaceHolder1_txtInstallationDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtInvoiceDate_txtDocDate').datepick('option', 'maxDate', new Date(dates[0].setDate(dates[0].getDate() - 1)) || null);
                        var Orgdates = dates[0];
                        $('#ContentPlaceHolder1_txtFailureDate_txtDocDate').datepick('option', 'minDate', new Date(Orgdates.setDate(Orgdates.getDate() + 1)) || null);

                    }
                    else if (this.id == 'ContentPlaceHolder1_txtInvoiceDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtInstallationDate_txtDocDate').datepick('option', 'minDate', new Date(dates[0].setDate(dates[0].getDate() + 1)) || null);
                    }

                }
            });
        }

    </script>

    <script type="text/javascript">

        document.onmousedown = disableclick;
        function disableclick(e) {
            if (event.button == 2) {
                return false;
            }
        }


        window.onload
        {
            AtPageLoad();
        }
        window.on
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');


            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }
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
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                    <asp:Label ID="lblHighValueMsg" runat="server" Text="(  High Value Request Amount:  "
                        CssClass="HideControl"> </asp:Label>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 14%">
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
                <td>
                    <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>
                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </asp:Panel>
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="txtDocNoDetails" runat="server" class="ContainTable">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" style="height: 15px" colspan="6">Document Details
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:Label ID="lblClaimType" runat="server" Text="Claim Type:"></asp:Label>
                                     <asp:DropDownList ID="DrpInvType" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="True" OnSelectedIndexChanged="DrpInvType_SelectedIndexChanged">
                                        <%-- <asp:ListItem Text="--Select--" Value="N" Selected="True"></asp:ListItem>
                                         <asp:ListItem Text="Part" Value="P"></asp:ListItem>
                                         <asp:ListItem Text="Labour" Value="L"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DropClaimTypes" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="True" OnSelectedIndexChanged="DropClaimTypes_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtClaimType" Text="" Font-Bold="true" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblClaimNo" runat="server" Text="Claim No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtClaimNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblClaimDate" runat="server" Text="Claim Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtClaimDate" runat="server" Mandatory="true" bCheckforCurrentDate="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%" >
                                    <asp:Label ID="lblSelectRefClaim" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Select Claim" Width="40%"> </asp:Label>
                                </td>
                                 <td style="width: 18%">
                                    <asp:Button ID="BtnSaveAccGrpDtl" runat="server" CssClass="CommandButton" Text="Save Accepted Amount Tax Details" OnClick="BtnSaveAccGrpDtl_Click" Visible="False"/>                                   
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblRequestNo" runat="server" Text="Request No.:"></asp:Label>
                                    <asp:Label ID="lblRefClaimNo" runat="server" Text="Ref. Claim No."></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRequestNo" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                    <asp:TextBox ID="txtRefClaimNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblRequestDate" runat="server" Text="Request Date:"></asp:Label>
                                    <asp:Label ID="lblRefClaimDate" runat="server" Text="Ref. Claim Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRequestDate" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                    <asp:TextBox ID="txtRefClaimDate" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" colspan="2" style="width: 15%">
                                    <asp:Button ID="btnCreateWarrantyClaim" runat="server" CssClass="CommandButton" Text="Create Warranty Claim"
                                        OnClientClick="return bCheckBeforeCreateWarrantyClaim();" OnClick="btnCreateWarrantyClaim_Click" />
                                    <asp:Button ID="btnReSubmitRequest" runat="server" CssClass="CommandButton" Text="ReSubmit Request"
                                        OnClick="btnReSubmitRequest_Click" />
                                    <asp:Button ID="btnReSubmitClaim" runat="server" CssClass="CommandButton" Text="ReSubmit Claim"
                                        OnClick="btnReSubmitClaim_Click" />
                                    <asp:Button ID="btnReturnedClaim" runat="server" CssClass="CommandButton" Text="Create Returned Claim"
                                        OnClick="btnReturnedClaim_Click" />
                                </td>
                                <td class="tdLabel" style="width: 15%">Claim Invoice No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprovalNo" runat="server" CssClass="TextBoxForString" Text=""
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Claim Invoice Date:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprovalDate" runat="server" CssClass="TextBoxForString" Text=""
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PVehicleDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEVehicleDetails" runat="server" TargetControlID="CntVehicleDetails"
                            ExpandControlID="TtlVehicleDetails" CollapseControlID="TtlVehicleDetails" Collapsed="false"
                            ImageControlID="imgTitleVD" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Vehicle Details" ExpandedText="Vehicle Details"
                            TextLabelID="lblTitleVehicleDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlVehicleDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="99%">
                                        <asp:Label ID="lblTitleVD" runat="server" Text="Vehicle Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Image ID="imgTitleVD" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntVehicleDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="tblVehicleDetails" runat="server" class="ContainTable">
                                <tr>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:LinkButton ID="lblSelectModel" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            onmouseover="SetCancelStyleonMouseOver(this);" Text="Select Jobcard" Width="80%"
                                            ToolTip="Select Jobcard No " OnClick="lblSelectModel_Click"> </asp:LinkButton>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:LinkButton ID="lblServiceHistroy" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            onmouseover="SetCancelStyleonMouseOver(this);" Text="Service History" Width="80%"
                                            ToolTip="Service History Details" Style="display: none"> </asp:LinkButton>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:Label ID="lblVehicleHistory" runat="server" ForeColor="#49A3D3" Text="Claim History"
                                            onmouseover="SetCancelStyleonMouseOver(this);" Visible="false" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">&nbsp;
                                    </td>
                                    <td style="width: 18%" colspan="2">&nbsp;
                                    </td>
                                    <td class="tdLabel" style="width: 18%;">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:Label ID="lblModelCode" runat="server" Text="Model Code:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtModelCode" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                        <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:Label ID="lblModelName" runat="server" Text="Model Name:"></asp:Label>
                                    </td>
                                    <td colspan="2" style="width: 18%">
                                        <asp:TextBox ID="txtModelDescription" runat="server" CssClass="TextBoxForString"
                                            Text="" Width="80%"></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 18%;"></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:Label ID="lblChassisNo" runat="server" Text="Chassis No.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtChassisNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        <asp:TextBox ID="txtAggreagateNo" Text="" runat="server" CssClass="TextBoxForString" Visible="false"></asp:TextBox>
                                        <asp:Label ID="Label3" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:Label ID="lblEngineNo" runat="server" Text="Engine No.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtEngineNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">INS Date:
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="txtInstallationDate" runat="server" Mandatory="true" bCheckforCurrentDate="false"
                                            EnableTheming="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%;">Customer Name:
                                    </td>
                                    <td style="width: 18%;">
                                        <asp:TextBox ID="txtCustomerName" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox><%--ReadOnly="true"--%>
                                        <asp:Label ID="Label8" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">Customer Address:
                                    </td>
                                    <td style="width: 18%;" colspan="2">
                                        <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="TextBoxForString" Width="80%"
                                            Height="50%" Text="" TextMode="MultiLine"></asp:TextBox><%--ReadOnly="true"--%>
                                        <asp:Label ID="Label9" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%;">Customer Mobile No:
                                    </td>
                                    <td style="width: 18%;">
                                        <asp:TextBox ID="txtCustMobNo" Text="" runat="server" CssClass="TextBoxForString"
                                            MaxLength="12" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                        <%--<asp:Label ID="lblCustMobNo" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>--%>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">Customer Email Address:
                                    </td>
                                    <td style="width: 18%;" colspan="2">
                                        <asp:TextBox ID="txtCustEmail" runat="server" CssClass="TextBoxForString" Width="80%"
                                            Height="50%" Text=""></asp:TextBox><%--<asp:Label ID="Label17" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%;">Odometer Reading :
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtOdometer" Text="" runat="server" CssClass="TextForAmount" onkeypress="return CheckForTextBoxValue(event,this,'5');"
                                            onBlur="return CheckOdometerReading(event,this);"></asp:TextBox><%--sujata 22012011--%>
                                        <asp:Label ID="Label14" runat="server" CssClass="Mandatory" Text="*" Font-Bold="true"></asp:Label><%--sujata 22012011--%>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">Hour Meter Reading:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtHrsReading" Text="" runat="server" CssClass="TextForAmount" onBlur="return CheckHrsReading(event,this);"></asp:TextBox><%--sujata 22012011--%>
                                        <asp:Label ID="Label15" runat="server" CssClass="Mandatory" Text="*" Font-Bold="true"></asp:Label><%--sujata 22012011--%>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        <%--Route Type:--%>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="drpRouteType" runat="server" CssClass="ComboBoxFixedSize" Visible="false">
                                        </asp:DropDownList>
                                        <%--<asp:Label ID="Label10" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%;">Vehicle Reg. No.:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtVehicleNo" Text="" runat="server" CssClass="TextBoxForString"
                                            MaxLength="12"></asp:TextBox><%--<asp:Label ID="lblVehicleNo" runat="server" CssClass="Mandatory"
                                            Text=" *" Font-Bold="true" Style="display: none"></asp:Label>--%>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">
                                        <asp:Label ID="lblRepairOrderNo" runat="server" Text="Repair Order No."></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRepairOrderNo" Text="" runat="server" CssClass="TextBoxForString"
                                            MaxLength="50" onBlur="return CheckRepairOrderNoWithLastOrderNo(this);"></asp:TextBox><asp:Label
                                                ID="Label12" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 18%; display: none">
                                        <asp:Label ID="lblLastServiceHistory" runat="server" ForeColor="#49A3D3" Text="Last 3 Service History"
                                          Visible="false"  onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">
                                        <asp:Label ID="lblFailureDate" runat="server" Text="Failure Date"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <%--sujata 22012011--%>
                                        <%--<uc3:CurrentDate ID="txtFailureDate" runat="server" Mandatory="false" bCheckforCurrentDate="false"
                                        EnableTheming="True" />--%>
                                        <uc3:CurrentDate ID="txtFailureDate" runat="server" Mandatory="true" bCheckforCurrentDate="false"
                                            EnableTheming="True" />
                                        <%--sujata 22012011--%>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">
                                        <asp:Label ID="lblRepairOrderDate" runat="server" Text="Repair Order Date"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <%--sujata 22012011--%>
                                        <%--<uc3:CurrentDate ID="txtRepairOrderDate" runat="server" Mandatory="false" bCheckforCurrentDate="false"
                                        Visible="True" />--%>
                                        <uc3:CurrentDate ID="txtRepairOrderDate" runat="server" Mandatory="true" bCheckforCurrentDate="false"
                                            Visible="True" />
                                        <%--sujata 22012011--%>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">
                                        <asp:Label ID="lblRepairCompleteDate" runat="server" Text="Repair Complete Date"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <%--sujata 22012011--%>
                                        <%--<uc3:CurrentDate ID="txtRepairCompleteDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />--%>
                                        <uc3:CurrentDate ID="txtRepairCompleteDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                        <%--sujata 22012011--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel HideControl" style="width: 15%">Sub Dealer Name:
                                    </td>
                                    <td style="width: 18%" class="HideControl">
                                        <asp:TextBox ID="txtSubDealerName" Text="" runat="server" CssClass="TextBoxForString"
                                            Width="90%" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:Label ID="lblDealerRemark" runat="server" Text="Dealer Remark:"></asp:Label>
                                    </td>
                                    <td style="width: 18%" colspan="2">
                                        <asp:TextBox ID="txtDealerRemark" runat="server" CssClass="TextBoxForString" Width="80%"
                                            Height="50%" Text="" TextMode="MultiLine" MaxLength="200"></asp:TextBox><%--sujata 22012011--%>
                                        <asp:Label ID="Label2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label><%--sujata 22012011--%>
                                    </td>
                                    <td style="width: 18%">&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="RequestDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                                <table id="tblRequestDetails" runat="server" class="ContainTable">
                                    <tr>
                                        <td class="tdLabel" style="width: 15%">Set Share
                                        </td>
                                        <td style="width: 18%" colspan="5">
                                            <asp:RadioButtonList ID="OptShareType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1" Selected="True" onClick=" OnOverallClaimChange(this);">To Overall Claim</asp:ListItem>
                                                <asp:ListItem onClick=" OnItemWiseSelected(this);" Value="2">To Item </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="width: 15%">
                                            <asp:Label ID="lblVECVShare" runat="server" Text="VECV Share%:"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="TextForAmount" Text="" Font-Bold="true"></asp:TextBox>
                                        </td>
                                        <td class="tdLabel" style="width: 15%">
                                            <asp:Label ID="lblDealerShare" runat="server" Text="Distributor Share%:"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="TextForAmount" Text=""
                                                Font-Bold="true"></asp:TextBox>
                                        </td>
                                        <td class="tdLabel" style="width: 15%">
                                            <asp:Label ID="lblCustomerShare" runat="server" Text="Customer Share%:"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtCustomerShare" runat="server" CssClass="TextForAmount" Text=""
                                                Font-Bold="true"> </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PInvDesc" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEInvDesc" runat="server" TargetControlID="CntInvDesc"
                                    ExpandControlID="TtlInvDesc" CollapseControlID="TtlInvDesc" Collapsed="True"
                                    ImageControlID="ImgTtlInvDesc" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Warranty Job Description"
                                    ExpandedText="Warranty Job Description" TextLabelID="lblTtlInvDesc">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlInvDesc" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlInvDesc" runat="server" Text="Invoice Job Description" ForeColor="White"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>                                           
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlInvDesc" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntInvDesc" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="InvJobDescGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Description">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DrpInvJobDesc" runat="server" CssClass="GridComboBoxFixedSize" Width="90%" > 
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPCompl" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEComplaints" runat="server" TargetControlID="CntComplaints"
                                    ExpandControlID="TtlComplaints" CollapseControlID="TtlComplaints" Collapsed="True"
                                    ImageControlID="ImgTtlComplaints" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Customer Complaints/ Incidence of Failure"
                                    ExpandedText="Customer Complaints/ Incidence of Failure" TextLabelID="lblTtlComplaints">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlComplaints" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title" width="82%">
                                                <asp:Label ID="lblTtlComplaints" runat="server" Text="Customer Complaints/ Incidence of Failure"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblComplaintsRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlComplaints" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="ComplaintsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Complaints">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpComplaint" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="90%" onblur="return CheckComplaintSelected(event,this);">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewComplaintDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                        Width="90%" onblur="return CheckComplaintAlreadyUsedInGrid(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Delete/Cancel" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForComplaint(event,this); "
                                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label><asp:LinkButton
                                                            ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRCompl" runat="server" AssociatedUpdatePanelID="UPCompl">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPInv" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PInvestigations" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEInvestigations" runat="server" TargetControlID="CntInvestigations"
                                    ExpandControlID="TtlInvestigations" CollapseControlID="TtlInvestigations" Collapsed="true"
                                    ImageControlID="ImgTtlInvestigations" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Dealer’s Investigations /Probable Cause"
                                    ExpandedText="Dealer’s Investigations /Probable Cause" TextLabelID="lblTtlInvestigations">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlInvestigations" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title" width="82%">
                                                <asp:Label ID="lblTtlInvestigations" runat="server" Text="Dealer’s Investigations /Probable Cause"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%-- <td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblInvestigationsRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlInvestigations" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntInvestigations" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="InvestigationsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Investigations">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpInvestigation" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="90%" onblur="return CheckInvestigationSelected(event,this);">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewInvestigationDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                        Width="90%" onblur=" return CheckInvestigationAlreadyUsedInGrid(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Delete/Cancel" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForInvestigation(event,this); "
                                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label><asp:LinkButton
                                                            ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRInv" runat="server" AssociatedUpdatePanelID="UPInv">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPPart" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                                    ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="True"
                                    ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Part Details" ExpandedText="Part Details"
                                    TextLabelID="lblTtlPartDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlPartDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title" width="82%">
                                                <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblPartRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                    <%--Style="display:'' "--%>
                                    <asp:GridView ID="PartDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                        AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                        GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                        SkinID="NormalGrid" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Failed Part ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPartID" runat="server" Text="0" Width="1%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Failed Part No." ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="SelectPart" ToolTip="Click Here To Select Part"
                                                        Width="70%"></asp:Label><asp:TextBox ID="txtPartNo" runat="server" CssClass="GridTextBoxForString"
                                                            Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Failed Part Name" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPartName" runat="server" CssClass="GridTextBoxForString" Text=""
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="98%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Failed Part Make" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <%--<asp:TextBox ID="txtPartMake" runat="server" CssClass="GridTextBoxForString" Text=""
                                                    Width="98%"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="drpPartMake" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Replaced Part ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRPartID" runat="server" Text="0" Width="1%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Replaced Part No." ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRPartNo" runat="server" CssClass="GridTextBoxForString" Text=""
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%"></asp:TextBox><asp:Label
                                                            ID="lblChngPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                            onmouseover="SetCancelStyleonMouseOver(this);" Text="Change" ToolTip="Click Here To Change the Part"
                                                            Width="70%"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Replaced Part Name" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRPartName" runat="server" CssClass="GridTextBoxForString" Text=""
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="98%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Replaced Part Make" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <%--<asp:TextBox ID="txtRPartMake" runat="server" CssClass="GridTextBoxForString" Text=""
                                                    Width="98%"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="drpRPartMake" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Code" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpJobCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                        onChange="return CheckJobCodeValidationForPart(this);" Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VECV(%)" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Distributor(%)" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckTotalOfPercentage(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Cancel" ItemStyle-Width="3%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" Text="Delete" onClick="return SelectDeleteCheckboxForPart(this);" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" onclick="ClearRowValueForPartWarranty(event,this); "
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"
                                                        Text="Cancel"></asp:Label>
                                                    <asp:LinkButton ID="lnkNew" runat="server" OnClientClick="return CheckRowValue(event,this);">New</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JobCard Det ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtJobcardDetID" runat="server" Text="0" Width="1%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BFRGST" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBFRGST" runat="server" Text="0" Width="1%"></asp:TextBox>
                                                </ItemTemplate>
                                                 <HeaderStyle CssClass="HideControl" />
                                                 <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Acc Qty" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAccQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deduction Per %" ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deduction Amt " ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeductionAmt" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Final Acc Amt " ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>    
                                            <asp:TemplateField HeaderText="D/M Flag" ItemStyle-Width="2%" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDealerOrMTI_Flag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRPart" runat="server" AssociatedUpdatePanelID="UPPart">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <%--<asp:UpdatePanel UpdateMode="Conditional" ID="UPLabor" runat="server" ChildrenAsTriggers="true">                  
                    <ContentTemplate>--%>
                    <asp:Panel ID="PLabourDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPELabourDetails" runat="server" TargetControlID="CntLabourDetails"
                            ExpandControlID="TtlLabourDetails" CollapseControlID="TtlLabourDetails" Collapsed="true"
                            ImageControlID="ImgTtlLabourDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Labour Details" ExpandedText="Labour Details"
                            TextLabelID="lblTtlLabourDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlLabourDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="82%">
                                        <asp:Label ID="lblTtlLabourDetails" runat="server" Text="Labour Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <%-- <td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblLabourRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlLabourDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntLabourDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                            <%--Style="display: none;"--%>
                            <asp:GridView ID="LabourDetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Labour ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLabourID" runat="server" Text="" Width="1"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Labour Code" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewLabour" runat="server" ForeColor="#49A3D3" Text="SelectLabour"
                                                ToolTip="Click Here To Select Labour Code" onmouseover="SetCancelStyleonMouseOver(this);"
                                                onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label><asp:TextBox ID="txtLabourCode"
                                                    runat="server" Text="" Width="96%" CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Labour Description" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLabourDesc" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Man Hrs" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtManHrs" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckForTextBoxValue(event,this,'6');"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotal" runat="server" onkeypress="return CheckForTextBoxValue(event,this,'6');"
                                                CssClass="GridTextBoxForAmount" Width="96%"></asp:TextBox><asp:TextBox ID="txtOldAmount"
                                                    runat="server" CssClass="HideControl" Width="4%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job Code" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpJobCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                Width="96%">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VECV(%)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Distributor(%)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckTotalOfPercentage(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="New/Cancel" HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxForLabour(this);"
                                                Text="Delete" />
                                            <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForLabourWarranty(event,this); "
                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label><asp:LinkButton
                                                    ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JobCard Det ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJobcardDetID" runat="server" Text="0" Width="1%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            <asp:TextBox ID="txtLTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            <asp:TextBox ID="txtLTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);" >                                                
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction Amt " ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeductionAmt" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Amount" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <%--  </ContentTemplate>
                </asp:UpdatePanel>--%>
                    <%--<asp:UpdateProgress ID="UPRLabor" runat="server" AssociatedUpdatePanelID="UPLabor">
                    <ProgressTemplate>
                        Inserting Record ......
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPLub" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:Panel ID="PLubricantDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPELubricantDetails" runat="server" TargetControlID="CntLubricantDetails"
                                    ExpandControlID="TtlLubricantDetails" CollapseControlID="TtlLubricantDetails"
                                    Collapsed="True" ImageControlID="ImgTtlLubricantDetails" ExpandedImage="~/Images/Minus.png"
                                    CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Lubricant Details"
                                    ExpandedText="Lubricant Details" TextLabelID="lblTtlLubricantDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlLubricantDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title" width="82%">
                                                <asp:Label ID="lblTtlLubricantDetails" runat="server" Text="Lubricant Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%-- <td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblLubricantRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlLubricantDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntLubricantDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;">
                                    <asp:GridView ID="LubricantDetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="65%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lubricant Type" ItemStyle-Width="18%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpLubricantType" runat="server" CssClass="GridComboBoxFixedSize"
                                                        onBlur="return CheckLubricantSelected(event,this);" Width="98%">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewLubricantDesc" runat="server" TextMode="MultiLine" CssClass="GridTextBoxForString"
                                                        Width="90%" onblur=" return CheckLubricantDescUsed(event,this);"></asp:TextBox><asp:DropDownList
                                                            ID="drpLubData" runat="server" CssClass="HideControl">
                                                        </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQty" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                        onfocus="SetLubricantFocus(this);" onBlur="return CheckLubQtyWithMaxQty(event,this);"
                                                        onkeypress="return CheckForTextBoxValue(event,this,'6');"></asp:TextBox><asp:TextBox
                                                            ID="txtMaxQty" runat="server" CssClass="HideControl" Width="80%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UOM" ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtUOM" runat="server" CssClass="GridTextBoxForAmount" Width="80%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onBlur="return CalculateLineTotalForLubricant(event,this);" onkeypress="return CheckForTextBoxValue(event,this,'6');"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox><asp:TextBox
                                                            ID="txtOldAmount" runat="server" CssClass="GridTextBoxForAmount" Width="4%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Code" ItemStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpJobCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VECV(%)" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Distributor(%)" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckTotalOfPercentage(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Delete/ Cancel" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" Text="Delete" onClick="return SelectDeleteCheckboxForLubricant(this);" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForLubricant(event,this); "
                                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label><asp:LinkButton
                                                            ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JobCard Det ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtJobcardDetID" runat="server" Text="0" Width="1%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtOTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtOTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BFRGST" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOBFRGST" runat="server" Text="0" Width="1%"></asp:TextBox>
                                                </ItemTemplate>
                                                 <HeaderStyle CssClass="HideControl" />
                                                 <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acc Qty" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAccQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deduction Per %" ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deduction Amt " ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeductionAmt" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Final Acc Amt " ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                             <asp:TemplateField HeaderText="D/M Flag" ItemStyle-Width="2%" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLubDealerOrMTI_Flag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>      
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRLub" runat="server" AssociatedUpdatePanelID="UPLub">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <%--<asp:UpdatePanel UpdateMode="Conditional" ID="UPSublet" runat="server" ChildrenAsTriggers="true">                    
                    <ContentTemplate>--%>
                    <asp:Panel ID="PSublettDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPESublettDetails" runat="server" TargetControlID="CntSublettDetails"
                            ExpandControlID="TtlSublettDetails" CollapseControlID="TtlSublettDetails" Collapsed="True"
                            ImageControlID="ImgTtlSublettDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Sublet Details" ExpandedText="Sublet Details"
                            TextLabelID="lblTtlSublettDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlSublettDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="82%">
                                        <asp:Label ID="lblTtlSublettDetails" runat="server" Text="Sublet Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <%--<td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblSubletRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlSublettDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntSublettDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                            <asp:GridView ID="SubletDetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="65%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Labour ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLabourID" runat="server" Text="1" Width="1"></asp:TextBox></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sublet Code" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLabourCode" Text="9999" runat="server" Width="96%" CssClass="GridTextBoxForString"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox></ItemTemplate>
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Sublet Code" ItemStyle-Width="16%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpSubletCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                Width="98%" onblur="return CheckSubletSelected(event,this);">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sublet Description" ItemStyle-Width="16%">
                                        <ItemTemplate>
                                            <%--<asp:DropDownList ID="drpSublet" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="98%"></asp:DropDownList>--%> <%--onblur="return CheckSubletSelected(event,this);"--%>
                                            <asp:TextBox ID="txtNewSubletDesc" runat="server" CssClass="GridTextBoxForString"
                                                onblur="return CheckSubletDescUsed(event,this);" Width="90%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Man Hrs" ItemStyle-Width="3%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtManHrs" runat="server" CssClass="GridTextBoxForAmount" Text="1"
                                                Width="96%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text="1" onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return CheckTextboxValueForNumeric(event,this,false);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return AddSubletTotalToClaimAmount(event,this);"></asp:TextBox><asp:TextBox
                                                    ID="txtOldAmount" runat="server" CssClass="HideControl" Width="4%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job Code" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpJobCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                Width="96%">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VECV(%)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Distributor(%)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                onkeypress="return CheckPercentageAmount(event,this)" onblur="return CheckTotalOfPercentage(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="New/Delete/ Cancel" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxForSubLet(this);"
                                                Text="Delete" />
                                            <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForSublet(event,this); "
                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label><asp:LinkButton
                                                    ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="JobCard Det ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtJobcardDetID" runat="server" Text="0" Width="1%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            <asp:TextBox ID="txtSTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            <asp:TextBox ID="txtSTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);" >                                                
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction Amt " ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeductionAmt" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Amount" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <%--   </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UPRSublet" runat="server" AssociatedUpdatePanelID="UPSublet">
                    <ProgressTemplate>
                        Inserting Record ......
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
                    <asp:Panel ID="PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntPartGroupDetails"
                            ExpandControlID="TtlPartGroupDetails" CollapseControlID="TtlPartGroupDetails" Collapsed="true"
                            ImageControlID="ImgTtlPartGroupDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Group Tax Details" ExpandedText="Group Tax Details"
                            TextLabelID="lblTtlPartGroupDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlPartGroupDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlPartGroupDetails" runat="server" Text="Group Tax Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlPartGroupDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <div class="scrolling-table-container WordWrap">
                                <asp:GridView ID="GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                    Width="99%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                    EditRowStyle-BorderColor="Black" AutoGenerateColumns="false" PageSize="5" CssClass="table table-condensed table-bordered">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Code" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGRPID" runat="server" Width="96%" Text='<%# Eval("group_code") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMGrName" runat="server" Text='<%# Eval("Gr_Name") %>' Width="90%"
                                                    CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Taxable Inv Amt" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                    MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateJbPartGranTotal();"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpTaxPer" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="drpTaxTag" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtTaxTag" runat="server" Text='<%# Eval("Tax_Tag") %>' CssClass="GridTextBoxForString" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax %" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTaxPer" runat="server" Text='<%# Eval("TAX_Percentage","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                    Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax Amt" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrTaxAmt" runat="server" Text='<%# Eval("tax_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                    Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpTax1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax1 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpTaxPer1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <%--Sujata 22092014_Begin--%>
                                                <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <%--Sujata 22092014_End--%>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax1 %" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTax1Per" runat="server" Text='<%# Eval("Tax1_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                    Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax1 Amt" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrTax1Amt" runat="server" Text='<%# Eval("tax1_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                    Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpTax2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <%--Sujata 22092014_Begin--%>
                                                <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <%--Sujata 22092014_End--%>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax2 %" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTax2Per" runat="server" Text='<%# Eval("Tax2_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                    Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax2 Amt" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrTax2Amt" runat="server" Text='<%# Eval("tax2_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                    Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTaxTot" runat="server" Width="90%" Text='<%# Eval("Total","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Acc_PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Acc_CntPartGroupDetails"
                                    ExpandControlID="Acc_TtlPartGroupDetails" CollapseControlID="Acc_TtlPartGroupDetails" Collapsed="true"
                                    ImageControlID="Acc_ImgTtlPartGroupDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText=" Accepted Group Tax Details" ExpandedText=" Accepted Group Tax Details"
                                    TextLabelID="Acc_lblTtlPartGroupDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="Acc_TtlPartGroupDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="Acc_lblTtlPartGroupDetails" runat="server" Text="Group Tax Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="Acc_ImgTtlPartGroupDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Acc_CntPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    ScrollBars="None">
                                    <asp:GridView ID="Acc_GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                                        CssClass="table table-condensed table-bordered">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Code" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGRPID" runat="server" Width="96%" Text='<%# Eval("group_code") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMGrName" runat="server" Text='<%# Eval("Gr_Name") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable Inv Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("Acc_net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                        MaxLength="6"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="drpTaxTag" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTaxTag" runat="server" Text='<%# Eval("Tax_Tag") %>' CssClass="GridTextBoxForString" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxPer" runat="server" Text='<%# Eval("TAX_Percentage","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTaxAmt" runat="server" Text='<%# Eval("Acc_tax_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <%--Sujata 22092014_Begin--%>
                                                    <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--Sujata 22092014_End--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax1Per" runat="server" Text='<%# Eval("Tax1_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTax1Amt" runat="server" Text='<%# Eval("Acc_tax1_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <%--Sujata 22092014_Begin--%>
                                                    <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--Sujata 22092014_End--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax2Per" runat="server" Text='<%# Eval("Tax2_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTax2Amt" runat="server" Text='<%# Eval("Acc_tax2_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxTot" runat="server" Width="90%" Text='<%# Eval("Acc_Total","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPJob" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PJobDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEJobDetails" runat="server" TargetControlID="CntJobDetails"
                                    ExpandControlID="TtlJobDetails" CollapseControlID="TtlJobDetails" Collapsed="True"
                                    ImageControlID="ImgTtlJobDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Job Details" ExpandedText="Job Details"
                                    TextLabelID="lblTtlJobDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlJobDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title" width="82%">
                                                <asp:Label ID="lblTtlJobDetails" runat="server" Text="Job Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                                    onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblJobRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlJobDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntJobDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                    <asp:GridView ID="JobDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                        AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                        GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                        SkinID="NormalGrid" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Code" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobCode" runat="server" CssClass="LabelCenterAlign" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Part ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPartID" runat="server" Text=""></asp:TextBox><asp:TextBox ID="txtWarrantablePart"
                                                        runat="server" Text=""></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Causal Part No." ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="GridTextBoxForString" Text=""
                                                        Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--<asp:Label ID="lblNewPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="SelectPart" ToolTip="Click Here To Select Part"
                                                        Width="70%"></asp:Label>--%>
                                                    <asp:LinkButton ID="lnkJbSelectPart" runat="server" OnClick="lnkJbSelectPart_Click">Select Part</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Causal Part Name" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPartName" runat="server" CssClass="GridTextBoxForString" Text=""
                                                        Width="98%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Culprit Code" ItemStyle-Width="13%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpCulpritCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="100%" onChange="return CheckCulpritCodeValidation(this);">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtCulprit_Tbl_ID" runat="server" Text="" Width="1" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Defect Code" ItemStyle-Width="13%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpDefectCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                        onChange="return CheckDefectCodeValidation(this);" Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Technical Code" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTechnicalCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                        onChange="return CheckTechnicalCodeValidation(this);" Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Delete/ Cancel" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxForJob(this);"
                                                        Text="Delete" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" onclick="return ClearRowValueForJob(this); "
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"
                                                        Text="Cancel"></asp:Label><asp:LinkButton ID="lnkNew" runat="server" OnClientClick="return CheckRowValue(event,this);">New</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRJob" runat="server" AssociatedUpdatePanelID="UPJob">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEFileAttchDetails" runat="server" TargetControlID="CntFileAttchDetails"
                            ExpandControlID="TtlFileAttchDetails" CollapseControlID="TtlFileAttchDetails"
                            Collapsed="false" ImageControlID="ImgTtlFileAttchDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Attached Documents"
                            ExpandedText="Attached Documents" TextLabelID="lblTtlFileAttchDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlFileAttchDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="82%">
                                        <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <%--<td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblFileAttachRecCnt" runat="server" Text="0"></asp:Label>
                                </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlFileAttchDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            Style="display: none;">
                            <table id="Table2" runat="server" class="ContainTable">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                            AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                            GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                            SkinID="NormalGrid" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="1%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                    ItemStyle-CssClass="HideControl">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFileAttchID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User File Description" ItemStyle-Width="50%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description") %>'
                                                            Width="96%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name" ItemStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                            onmouseover="SetCancelStyleonMouseOver(this);" onClick="return ShowAttachDocument(this);"
                                                            Text='<%# Eval("File_Names") %>' ToolTip="Click Here To Open The File" Width="90%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CreatedUserRole" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                    ItemStyle-CssClass="HideControl">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCrUserRole" runat="server" Text='<%# Eval("CreatedUserRole") %>' Width="1"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);"
                                                            Text="Delete" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle Wrap="True" />
                                            <EditRowStyle BorderColor="Black" Wrap="True" />
                                            <AlternatingRowStyle Wrap="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr id="trNewAttachment" runat="server">
                                    <td class="tdLabel" style="width: 50%" align="center">User File Description
                                    </td>
                                    <td class="tdLabel" style="width: 50%" align="center">File Name
                                    </td>
                                </tr>
                                <tr id="trNewAttachment1" runat="server">
                                    <td colspan="2" class="tdLabel">
                                        <div id="upload1">
                                            <input id="Text1" type="text" name="Text1" class="TextBoxForString" style="width: 50%" placeholder="User File Description" />
                                            <input id="AttachFile" type="file" runat="server" style="width: 45%" class="Cntrl1"
                                                onblur="return addFileUploadBox(this);" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PTotal" runat="server">
                        <table id="TblTotal" runat="server" border="1" class="ContainTable table table-bordered">
                            <tr>
                                <td style="text-align: right">
                                    <b>Parts Amount: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtPartAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                 <td style="text-align: right">
                                    <b><asp:Label ID="lblAccPartAmount" runat="server" CssClass="LabelRightAlign" Text="Accepted Part Amount: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccPartAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b>Labour Amount: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtLabourAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                 <td style="text-align: right">
                                    <b><asp:Label ID="lblAccLabourAmount" runat="server" CssClass="LabelRightAlign" Text="Accepted Labour Amount: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccLabourAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b>Lubricant Amount: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtLubricantAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                  <td style="text-align: right">
                                   <b><asp:Label ID="lblAccLubricantAmount" runat="server" CssClass="LabelRightAlign" Text="Accepted Lubricant Amount: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccLubricantAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b>Sublet Amount: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtSubletAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <b><asp:Label ID="lblAccSubletAmount" runat="server" CssClass="LabelRightAlign" Text="Accepted Sublet Amount: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccSubletAmount" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b><asp:Label ID="lblDTaxlbl" runat="server" CssClass="LabelRightAlign" Text="Labour + Sublet Tax Amt: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtDTaxAmt" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <b><asp:Label ID="lblAccDTaxlbl" runat="server" CssClass="LabelRightAlign" Text="Accepted Labour + Sublet Tax Amt: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccDTaxAmt" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr  id="trTax1" runat="server">
                                <td style="text-align: right">
                                    <b><asp:Label ID="lblDTax1lbl" runat="server" CssClass="LabelRightAlign" Text="Labour + Sublet Additional Tax1 Amt: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtDTax1Amt" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <b><asp:Label ID="lblAccDTax1lbl" runat="server" CssClass="LabelRightAlign" Text="Accepted Labour + Sublet Additional Tax1 Amt: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccDTax1Amt" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trTax2" runat="server">
                                <td style="text-align: right">
                                    <b>Labour + Sublet Additional Tax2 Amt: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtDTax2Amt" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                     <b><asp:Label ID="lblAccDTax2Amt" runat="server" CssClass="LabelRightAlign" Text="Labour + Sublet Additional Tax2 Amt: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccDTax2Amt" runat="server" CssClass="TextForAmount" Text="0"
                                        Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b>Claim Amount:</b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtClaimAmt" runat="server" CssClass="TextForAmount" Text="0" Font-Bold="true"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <b><asp:Label ID="lblAccClaimAmt" runat="server" CssClass="LabelRightAlign" Text="Accepted Claim Amount: "></asp:Label></b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAccClaimAmt" runat="server" CssClass="TextForAmount" Text="0" Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server"
                            Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" CssClass="HideControl"
                                runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerCode"
                                    runat="server" Width="1%" Text="" CssClass="HideControl"></asp:TextBox><asp:TextBox
                                        ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtModelGroupID" Text="1" runat="server" CssClass="HideControl" Width="5%"></asp:TextBox><asp:TextBox
                        ID="txtModelID" Text="" runat="server" Width="5%" CssClass="HideControl"></asp:TextBox><asp:TextBox
                            ID="txtRequestID" runat="server" Text="" Width="1%" CssClass="HideControl"></asp:TextBox><asp:TextBox
                                ID="txtRefClaimID" runat="server" Text="" Width="1%" CssClass="HideControl"></asp:TextBox><asp:TextBox
                                    ID="txtClaimRevNo" runat="server" Text="" Width="1%" CssClass="HideControl"></asp:TextBox><asp:TextBox
                                        ID="txtLastRepairDate" runat="server" CssClass="HideControl" Text=""></asp:TextBox><asp:TextBox
                                            ID="txtLastMeterReading" runat="server" CssClass="HideControl" Text=""></asp:TextBox><asp:TextBox
                                                ID="txtHrsApplicable" runat="server" CssClass="HideControl" Text=""></asp:TextBox><asp:TextBox
                                                    ID="txtNewRecountCount" runat="server" Text="1" Width="1%" CssClass="HideControl"></asp:TextBox><asp:DropDownList
                                                        ID="drpClaimType" runat="server" CssClass="HideControl">
                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Goodwill" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Normal" Value="2" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                    <asp:LinkButton ID="lblSelectRequest" runat="server" CssClass="HideControl" ForeColor="#49A3D3"
                        Text="Select Request" OnClick="lblSelectRequest_Click"></asp:LinkButton><asp:TextBox
                            ID="txtLastRepairOrderNo" runat="server" Text="" Width="1%" CssClass="HideControl"></asp:TextBox><asp:TextBox
                                ID="txtRootTypeID" runat="server" CssClass="HideControl" Text=""></asp:TextBox><asp:TextBox
                                    ID="txtchassisID" runat="server" CssClass="HideControl" Text=""></asp:TextBox><asp:TextBox
                                        ID="txtUserType" runat="server" CssClass="HideControl" Text=""></asp:TextBox><%--sujata 19012011--%>
                    <asp:TextBox ID="txtChkfun" runat="server" CssClass="HideControl" Text="false"></asp:TextBox><%--sujata 19012011--%>
                    <asp:HiddenField ID="hdnChassis" runat="server" Value="" />
                    <asp:HiddenField ID="hdnReSubmitRequest" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnReSubmitClaim" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnReturnedClaim" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnRejectCnt" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnReturnedCnt" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnPostShipmentDate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMinClaimDate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnIsSHQResource" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnIsSHQ" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnJobcardID" runat="server" Value="" />
                    <asp:Label ID="lblInvestigationsRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblPartRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblLabourRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblLubricantRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblSubletRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblJobRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblComplaintsRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:TextBox ID="txtInvoiceNo" Text="" runat="server" CssClass="HideControl"></asp:TextBox>
                    <uc3:CurrentDate ID="txtInvoiceDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" Visible="false"
                        EnableTheming="True" />
                    <asp:TextBox ID="txtGVW" Text="" runat="server" CssClass="HideControl" Visible="false"></asp:TextBox>
                    <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="" />
                     <asp:HiddenField ID="hdnISDocGST" runat="server" /> 
                    <asp:HiddenField ID="hdnCustID" runat="server" /> 
                    <asp:HiddenField ID="hdnAccDetUpdate" runat="server" /> 
                    <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
