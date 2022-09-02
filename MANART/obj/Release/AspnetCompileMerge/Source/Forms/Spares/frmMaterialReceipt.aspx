<%@ Page Title="dCAN-Parts Material Receipt" Language="C#" MasterPageFile="~/Header.Master" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Theme="SkinFile" AutoEventWireup="true" CodeBehind="frmMaterialReceipt.aspx.cs" Inherits="MANART.Forms.Spares.frmMaterialReceipt" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/ExportLocation.ascx" TagPrefix="uc2" TagName="ExportLocation" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFPFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsEGPSpareReceipt.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <style type="text/css">
        input[type="radio"] {
            margin: 4px 0 0;
            margin-top: 1px \9;
            line-height: normal;
            margin-left: -310px;
        }

        label {
            color: #303c49;
            float: left;
            font-size: 12px;
            font-weight: normal;
            width: 203px;
            padding-left: 30px;
        }
    </style>
    <style>
        .NonEditableFields {
            background: #CCC;
            color: #333;
            border: 1px solid #666;
            width: 90%;
            height: 25px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            //debugger;
            var txtMReceiptDate = document.getElementById("ContentPlaceHolder1_txtMReceiptDate_txtDocDate");
            var txtDMSInvDate = document.getElementById("ContentPlaceHolder1_txtDMSInvDate_txtDocDate");
            var txtLRDate = document.getElementById("ContentPlaceHolder1_txtLRDate_txtDocDate");
            var _myDate = $('#ContentPlaceHolder1_txtMReceiptDate_txtDocDate').val();
            var ReceiptType = $('#ContentPlaceHolder1_hdnIsAutoReceipt').val();

            $('#ContentPlaceHolder1_txtMReceiptDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtMReceiptDate.value == '') ? '0d' : txtMReceiptDate.value, maxDate: (txtMReceiptDate.value == '') ? '0d' : txtMReceiptDate.value
            });

            $('#ContentPlaceHolder1_txtDMSInvDate_txtDocDate').datepick({
                //dateFormat: 'dd/mm/yyyy', maxDate: (txtDMSInvDate.value == '') ? '0d' : txtMReceiptDate.value --this is working 
                dateFormat: 'dd/mm/yyyy', maxDate: (txtDMSInvDate.value == '') ? txtMReceiptDate.value : txtMReceiptDate.value
            });

            $('#ContentPlaceHolder1_txtLRDate_txtDocDate').datepick({
                //dateFormat: 'dd/mm/yyyy', maxDate: (txtLRDate.value == '') ? '0d' : txtLRDate.value this is working
                dateFormat: 'dd/mm/yyyy', maxDate: (txtLRDate.value == '') ? txtMReceiptDate.value : txtLRDate.value //this wong in after Publish
            });


        });
    </script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                event.returnValue = false;
                alert(message);
                return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }

        function FormValidation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName").value == "0") {
                errMessage += "*Please Select Supplier Name.\n";
            }
            if (errMessage != "") {
                alert(errMessage);
                return false;
            }
            else {
                return true;
            }
        }
        function GetPreviousSelectedPOPartID(objNewPartLabel) {
            //debugger;
            var objRow;
            var PartIds = "";
            var PartId = "";
            var txtPartId;
            var txtPODetId;
            var PODetId = "";
            // get grid object
            var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
            if (objGrid == null) return PartIds;
            for (var i = 2; i <= objGrid.rows.length - 1 ; i++) {
                //Get Row
                objRow = objGrid.rows[i];

                //Get Part ID
                txtPartId = objGrid.rows[i].children[1].childNodes[1];

                txtPODetId = objGrid.rows[i].children[1].childNodes[3];
                //Get PartId;
                PODetId = (txtPODetId.value);

                if (PODetId == "0" && PODetId == "" && PODetId == null) {
                    PODetId = "0";
                }

                PartId = PODetId + "-" + (txtPartId.value);
                if (PartId != "0" && PartId != "" && PartId != null) {
                    PartIds = PartIds + PartId + ",";
                }
            }
            PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

            return PartIds;
        }

        //To Show Part Master        
        function ShowMultiPartSearch(objNewPartLabel, sDealerId, sSupplierId, Is_AutoReceipt, iHOBrId) {
            if (FormValidation() == false) {
                return false;
            }
            //var PartDetailsValue;
            //var sSelectedPartID = GetPreviousSelectedPOPartID(objNewPartLabel);
            //if (Is_AutoReceipt == "N") {
            //    PartDetailsValue = window.showModalDialog("../Common/frmSelMultiPartPenPartsPO.aspx?DealerID=" + sDealerId + "&SupplierID=" + sSupplierId + "&Is_AutoReceipt=" + Is_AutoReceipt + "&SelectedPartID=" + sSelectedPartID + "&HOBR_ID=" + iHOBrId + "&SourchFrom=MRPartDetails&TransFrom=Common", "List", "dialogHeight: 500px; dialogWidth: 700px;");
            //}
        }
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
        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var filename = objFileUpload.value;
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search('_SparesPO_PartDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }
        }

        function ClearRowValueForMR(event, objCancelControl) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;
            var i = 1;
            //Status
            objRow[26].childNodes[0].value = 'C';

            CalulateReceivePartGranTotal();
        }

        function SelectDeleteRowValueForMR(event, objCancelControl, objDeleteCheck) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;

            if (objDeleteCheck.checked == true) {
                objRow[26].childNodes[0].value = 'D';
            }
            else {
                objRow[26].childNodes[0].value = 'E';
            }
            CalulateReceivePartGranTotal();
            return false
        }
        function SelectDeletCheckboxMR(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
            else {
                if (confirm("Are you sure you want to revert changes?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }

            }
        }



    </script>

    <script type="text/javascript">
        //  opener.ChkSpNDPPartClick(objImgControl);
        function ChkSpNDPPartClick(objImgControl) {
            //debugger;
            var objID = $('#' + objImgControl.id);
            var objCol = objID[0].parentNode.parentNode;
            var txtparst = document.getElementById("ContentPlaceHolder1_txtPartIds");

            var ArrOfPartDtls = '';
            var removePartID;

            //Changes done for jobcard part selection solution for part type tag not get selected here
            //for (i = 1; i < objCol.cells.length - 1; i++) {
            for (i = 1; i < objCol.cells.length; i++) {
                if (i == objCol.cells.length - 1)
                    ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML;
                else
                    ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML + '<--';
            }

            //ArrOfPartDtls = sPartID + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;
            //ArrOfPartDtls = sPartID + '<--' + sParFOBRt + '<--' + sParMOQ + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;

            if (objImgControl.checked == true) {
                if (txtparst.value == "") {
                    txtparst.value = ArrOfPartDtls;
                }
                else {
                    txtparst.value = txtparst.value + '#' + ArrOfPartDtls;
                }

            } else {
                removePartID = txtparst.value;

                var afterRemove = "";
                var arr = removePartID.split("#");
                txtparst.value = "";
                var arrlen = arr.length;
                for (var i = 0; i < arrlen; i++) {
                    if (arr[i] == ArrOfPartDtls) {
                        // arr.splice(i, 1);

                    }
                    else {

                        if (txtparst.value == "") {
                            txtparst.value = arr[i];
                        }
                        else {
                            txtparst.value = txtparst.value + '#' + arr[i];
                        }
                    }
                }
                // txtparst.value = arr;
            }
            return txtparst.value;

        }
    </script>

    <%-- New Webmethod--%>
    <script type="text/javascript">
        function onTextChange(data) {
            PageMethods.GetInvoice(document.getElementById("<%=txtDMSInvNo.ClientID%>").value, OnSuccess);
        }
        function OnSuccess(response, userContext, methodName) {
            if (response != 0) {
                alert('Invoice Number Already Exist!');
                document.getElementById("<%=txtDMSInvNo.ClientID%>").value = "";
                document.getElementById("<%=txtDMSInvNo.ClientID%>").style.border = "2px solid red";
            }
            else { document.getElementById("<%=txtDMSInvNo.ClientID%>").style.border = "2px solid green"; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Material Receipt"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc1:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:ExportLocation ID="ExportLocation" runat="server" />
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />
                </asp:Panel>
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Material Receipt Details" ExpandedText="Material Receipt Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Document Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDocDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="tblMReceiptDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblInvoiceType" runat="server" Text="Receipt Type"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlInvoiceType" runat="server"
                                        CssClass="ComboBoxFixedSize NonEditableFields ">
                                        <%-- AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceType_SelectedIndexChanged" <asp:ListItem Selected="True" Value="Y">Auto</asp:ListItem>
                                        <asp:ListItem Value="N">Manual</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMInvoiceType" runat="server" Text=" *" CssClass="Mandatory" Visible="false"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblMReceiptNo" runat="server" Text="Material Receipt No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMReceiptNo" Text="" runat="server" CssClass="TextBoxForString NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblMReceiptDate" runat="server" Text="Material Receipt Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtMReceiptDate" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblDMSInvNo" runat="server" Text="Invoice No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlInvoice" runat="server" AutoPostBack="true"
                                        CssClass="ComboBoxFixedSize "
                                        OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged" Style="display: none">
                                    </asp:DropDownList>
                                    <%--AutoPostBack="true"--%>
                                    <asp:TextBox ID="txtDMSInvNo" Text="" runat="server" MaxLength="20" CssClass="TextBoxForString" onchange="onTextChange(this)"></asp:TextBox>
                                    <%--  <asp:TextBox ID="txtDMSInvNo" Text="" runat="server" MaxLength="20" CssClass="TextBoxForString" AutoPostBack="true" OnTextChanged="txtDMSInvNo_TextChanged"></asp:TextBox>--%>
                                    <asp:Label ID="lblMInvoiceNo" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtDMSInvDate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" Mandatory="false" />
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">LR No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLR_No" runat="server" MaxLength="20" CssClass="TextBoxForString "></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblLrDate" runat="server" Text="LR Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtLRDate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" Mandatory="false" />
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:Label ID="lblDeliveryNo" runat="server" Text="Delivery No:" CssClass=""></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDeliveryNo" Text="" runat="server" MaxLength="20" CssClass="TextBoxForString " AutoPostBack="true" OnTextChanged="txtDeliveryNo_TextChanged"></asp:TextBox>
                                    <asp:Label ID="lblMDeliveryNo" runat="server" CssClass="Mandatory" Text="*"></asp:Label>
                                    <asp:TextBox ID="txtCreatedBy" Text="" runat="server" CssClass="TextBoxForString HideControl"
                                        Enabled="false"></asp:TextBox>
                                </td>
                                <%-- HideControl style="width: 15%" --%>
                                <td class="tdLabel " colspan="2">
                                    <%--<asp:Label ID="lblPerOrAmt" runat="server" Text="Please Select"></asp:Label>--%>
                                    <%--</td>
                                <td style="width: 18%">--%>
                                    <div>
                                        <asp:RadioButtonList ID="rbtLstDiscount" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="rbtLstDiscount_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Discount In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Discount In Amount" Value="Amt"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="true"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Part Details" ExpandedText="Part Details"
                        TextLabelID="lblTtlPartDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <div class="scrolling-table-container" style="height: 200px; background-color: #D4D4D4;">
                         <%--<div style ="height:200px; overflow:auto;">--%>
                            <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                 CssClass="table table-condensed table-bordered"
                                AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnPageIndexChanging="PartGrid_PageIndexChanging"
                                OnRowCommand="PartGrid_RowCommand" OnRowDataBound="PartGrid_RowDataBound">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex   %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartID" runat="server" Width="1%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtPODetID" runat="server" Width="5%" Text='<%# Eval("PO_Det_ID") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtInvDetID" runat="server" Width="5%" Text='<%# Eval("Inv_Det_ID") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part No." ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);" ></asp:TextBox>
                                            <asp:LinkButton ID="lnkSelectPart" CssClass="btn btn-link" runat="server" OnClientClick="return FormValidation();" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name - Location" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="96%" TextMode="MultiLine"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bal PO Qty" ItemStyle-Width="3%">
                                        <%-- 4 --%>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtbalPOQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Bal_PO_Qty","{0:#0.00}") %>'
                                                Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill Qty" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBillQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Bill_Qty","{0:#0.00}") %>' MaxLength="6"
                                                Width="96%"></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this); onblur="return CalculateReceiptPartTotal(event,this);""--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recv Qty" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRecvQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Recv_Qty","{0:#0.00}")%>' MaxLength="6"
                                                Width="96%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateReceiptPartTotal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnit" runat="server" Text='<%# Eval("Unit") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("Price","{0:#0.00}") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Enabled="true" Width="96%" Text='<%# Eval("MRPRate","{0:#0.00}") %>'> </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disc (Per)" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Disc_Per","{0:#0.00}") %>'
                                                Width="96%"></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this)" onblur="return CalculateReceiptPartTotal(event,this);"--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Rate" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Accept_Rate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                            <asp:TextBox ID="txtExclDiscountRate" runat="server" CssClass="HideControl" Width="96%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="3%">
                                        <%-- 10 --%>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="TxtExclTotal" runat="server" CssClass="HideControl" Width="80%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MRP Rate" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMRP_Rate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("MRP_Rate","{0:#0.00}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'6');"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ass Value" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAssValue" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Ass_Value","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PO No" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPONo" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("PO_NO") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Tax" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpPartTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%"
                                                OnSelectedIndexChanged="drpPartTax_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DrpPartTax1" runat="server" CssClass="HideControl" Width="99%"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DrpPartTax2" runat="server" CssClass="HideControl" Width="99%"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="drpPartTaxPer" runat="server" CssClass="HideControl" Width="99%"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtPartTaxPer" runat="server" CssClass="HideControl" Width="80%"></asp:TextBox>
                                            <%--Sujata 05092014_Begin added tax1 and tax2 revert calculation--%>
                                            <asp:DropDownList ID="DrpPartTax1Per" runat="server" CssClass="HideControl" Width="99%"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtPartTax1Per" runat="server" CssClass="HideControl" Width="80%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                            <asp:DropDownList ID="DrpPartTax2Per" runat="server" CssClass="HideControl" Width="99%"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtPartTax2Per" runat="server" CssClass="HideControl" Width="80%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part TaxPerGroupCode" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTaxPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Tax_Per") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="txtGroupCode" runat="server" Width="80%" Text='<%# Eval("group_code") %>'
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discrip ancy" ItemStyle-Width="2%">
                                        <%-- 15 --%>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpDescripancy" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" Enabled="false"
                                                onchange="return CalculateReceiptPartTotal(event,this);">
                                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shortage Qty" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtShortageQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Shortage_Qty","{0:#0.00}")%>'
                                                Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Excess Qty" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtExcessQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Excess_Qty","{0:#0.00}")%>'
                                                Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Damage Qty" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDamageQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Damage_Qty","{0:#0.00}")%>'
                                                Width="96%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateReceiptPartTotal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Manf Defect Qty" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtManDefectQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Man_Defect_Qty","{0:#0.00}")%>'
                                                Width="96%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateReceiptPartTotal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wrong Supply Qty" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWrongSupplyQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Wrong_Supply_Qty","{0:#0.00}")%>'
                                                Width="96%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateReceiptPartTotal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Retain (Y/N)" ItemStyle-Width="2%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpRetain" runat="server" CssClass="GridComboBoxFixedSize" Width="96%">
                                                <%--onchange="CalculateReceiptPartTotal(event,this);"--%>
                                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wrg Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWrgPartID" runat="server" Width="1%" Text='<%# Eval("Wrg_Part_ID") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wrong Part No" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWrgPartNo" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Wrg_Part_No")%>'
                                                Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:LinkButton ID="lnkSelectPart1" runat="server" CssClass="btn btn-link" OnClick="lnkSelectPart1_Click">Select Part</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="9%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWrgPartName" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Wrg_Part_Name")%>'
                                                Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxMR(event,this);" />
                                            <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SAP Order No" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSAPOrderNo" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("SAP_Order_No")%>'
                                                Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tax %" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTaxPer1" runat="server" Text='<%# Eval("TAX_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax Amt" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTaxAmt" runat="server" Text='<%# Eval("Tax_Amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpTax1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax1 %" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTax1Per" runat="server" Text='<%# Eval("Tax1_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax1 Amt" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTax1Amt" runat="server" Text='<%# Eval("Tax1_Amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BFRGST" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBFRGST" runat="server" Width="96%" Text='<%# Eval("BFRGST") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <table id="tblTotal" runat="server" class="ContainTable table table-bordered" style="background-color: #D4D4D4;" width="100%">
                        <tr>
                            <td style="width: 15%; text-align: right">
                                <b>Total:</b>
                            </td>
                            <td style="width: 8%; text-align: left">
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                            <td style="width: 22%; text-align: left">
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
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
                        <asp:GridView ID="GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="false" PageSize="5">
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
                                <asp:TemplateField HeaderText="Net Receipt Amt" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtGrnetrevamt" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount " Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateReceivePartGranTotal();"--%>
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
                                        <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
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
                                        <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
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
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                        ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                        ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Receipt Tax Details" ExpandedText="Receipt Tax Details"
                        TextLabelID="lblTtlTaxDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlTaxDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Receipt Tax Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlTaxDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="GrdDocTaxDet" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5">
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocID" runat="server" Width="5%" Text='<%# Eval("ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocTotal" runat="server" Text='<%# Eval("net_tr_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtDocRevTotal" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForAmount HideControl" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocDisc" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                     <%--<HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />--%>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Before Tax Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBeforeTax" runat="server" Text='<%# Eval("before_tax_amt") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Excise Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrExciseAmt" runat="server" Text='<%# Eval("Excise_Amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Insu Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrInsuAmt" runat="server" Text='<%# Eval("Insu_Amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LST Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("mst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CST Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("cst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax 1" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax1" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("surcharge_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax 2" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax2" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("tot_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PF Charges%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                            Width="90%"></asp:TextBox>
                                        <%--onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalulateReceivePartGranTotal();"--%>
                                    </ItemTemplate>
                                     <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PF Charges Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                    </ItemTemplate>
                                     <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalulateReceivePartGranTotal();"--%>
                                    </ItemTemplate>
                                     <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_money","{0:#0.00}")%>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                     <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
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
                <asp:TextBox ID="Wrg_Part_ID" CssClass="HideControl" runat="server" Width="1px"></asp:TextBox>
                <asp:HiddenField ID="hdnDealerID" runat="server" Value="0" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsDistributor" runat="server" Value="Y" />
                <asp:HiddenField ID="hdnIsWithPO" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsAutoReceipt" runat="server" Value="Y" />
                <asp:HiddenField ID="hdnSupTaxTag" runat="server" />
                <asp:HiddenField ID="hdnInvoiceID" runat="server" />
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                 <asp:HiddenField ID="hdnSupplierType" runat="server" Value="" />
                <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>

    <!---model popup for select local part design------>
    <cc1:ModalPopupExtender ID="mpeSelectPart" runat="server" TargetControlID="lblTragetID" PopupControlID="pnlPopupWindow"
        OkControlID="btnOK" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup_Receipt" Style="display: none;">
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                    <asp:Label ID="Label1" runat="server" Text="Pending Po And Part Master">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl1">
                <td style="width: 14%">

                    <div align="center" class="ContainTable">
                        <table class="table-bordered">
                            <tr align="center">
                                <td class="tdLabel" style="width: 7%;">Search:
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>

                                <td class="tdLabel" style="width: 15%;">
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="ComboBoxFixedSize" Width="100px">
                                        <asp:ListItem Selected="True" Value="P">Part No</asp:ListItem>
                                        <asp:ListItem Value="N">Part Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel" style="width: 10%;">
                                    <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                    <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                        OnClick="btnSave_Click" />
                                    &nbsp;
                                </td>
                                <td class="tdLabel" style="width: 10%;">
                                    <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search btn-sm"
                                        OnClick="btnBack_Click"></asp:Button>
                                </td>
                            </tr>
                            <tr align="center">
                                <td class="tdLabel" style="width: 7%;"></td>
                                <td class="tdLabel" style="width: 15%;" align="left" colspan="2">
                                    <asp:Label ID="lblNMsg" runat="server" Font-Size="8" CssClass="Mandatory" Text='Search Not Found...!'></asp:Label>
                                </td>
                                <td class="tdLabel" style="width: 15%;"></td>
                                <td class="tdLabel" style="width: 10%;"></td>
                            </tr>
                        </table>
                    </div>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%"
                            AutoGenerateColumns="false">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClick(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FOBRate" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFOBRate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Rate","{0:#0.00}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PODetID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOADetID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_Det_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PO" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOANo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMOQ" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("MOQ") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name - Location" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group Code" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrCode" runat="server" Text='<%# Eval("group_code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group Name" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Gr_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bal" ItemStyle-Width="1%" ItemStyle-CssClass="LabelRightAlign">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClbal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cl_bal","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%--<ItemStyle CssClass="LabelRightAlign" Width="5%" />--%>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PartTax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartTax" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PartTaxID","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <%--Sujata 04092014_Begin OA Discount and tax taken in Invoice--%>
                                <asp:TemplateField HeaderText="Bal PO Qty" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvQty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Qty","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Per" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscountPer" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("discount_per","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscountAmt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("discount_amt","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Rate" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscountRate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("disc_rate","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shortage_Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblShortage_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Shortage_Qty","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Excess_Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExcess_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Excess_Qty","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Damage_Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDamage_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Damage_Qty","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Man_Defect_Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMan_Defect_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Man_Defect_Qty","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Wrong_Supply_Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWrong_Supply_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrong_Supply_Qty","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Wrg_Part_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWrg_Part_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrg_Part_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Wrg_Part_No" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWrg_Part_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrg_Part_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Wrg_Part_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWrg_Part_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Wrg_Part_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="TaxTag" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxTag" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("TaxTag") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnit" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Unit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrice" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Price","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTax_Per1" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Tax_Per","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle Wrap="True" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                        <cc2:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" Width="1000px" PageSize="15"
                            GenerateGoToSection="true" />
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TmpControl1">
                <td style="width: 15%">
                    <asp:TextBox ID="txtPartIds" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!---ENd Here popup for select local part design------>

</asp:Content>
