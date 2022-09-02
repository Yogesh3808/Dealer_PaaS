<%@ Page Title="Parts PO" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="frmPartsPO.aspx.cs" Inherits="MANART.Forms.Spares.frmPartsPO" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/ExportLocation.ascx" TagPrefix="uc2" TagName="ExportLocation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <style>
        .checkbox .btn,
        .checkbox-inline .btn {
            padding-left: 2em;
            min-width: 8em;
        }

        .checkbox label,
        .checkbox-inline label {
            text-align: left;
            padding-left: 0.5em;
        }
    </style>

    <%--<style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 1px;
            padding-left: 1px;
            width: 550px;
        }
    </style>--%>

    <script src="showModalDialog.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var PrevPoDate = document.getElementById('ContentPlaceHolder1_txtPreviousPoDate').value;
            var txtPODate = document.getElementById("ContentPlaceHolder1_txtPODate_txtDocDate");
            var POID = document.getElementById('ContentPlaceHolder1_txtID');

            if (POID.value != 0) { // POID is Not Zero or EDIT CASE
                if (PrevPoDate == 'N') {// When Edit Last Record from Grid then set max Date to Current Date(System Date)
                    PrevPoDate = "0";
                }
                $('#ContentPlaceHolder1_txtPODate_txtDocDate').datepick({
                    dateFormat: 'dd/mm/yyyy', minDate: txtPODate.value, maxDate: PrevPoDate
                });
            }
            else if (POID.value == 0 && PrevPoDate != 'N') { //ADD Mode and has More than one PO in Table of any Type
                $('#ContentPlaceHolder1_txtPODate_txtDocDate').datepick({
                    dateFormat: 'dd/mm/yyyy', minDate: PrevPoDate, maxDate: "0"
                });
            }
            else if (PrevPoDate == 'N') {//has Previous Order Date Not Present or Last Record When Editing
                $('#ContentPlaceHolder1_txtPODate_txtDocDate').datepick({
                    dateFormat: 'dd/mm/yyyy', minDate: (txtPODate.value == '') ? '0d' : txtPODate.value, maxDate: (txtPODate.value == '') ? '0d' : txtPODate.value
                });
            }
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

        // Validation Function
        function Validation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName").value == "0") {
                errMessage += "*Please Select Supplier.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_drpPoType").value == "0" && document.getElementById('ContentPlaceHolder1_hdnPoTypeID').value == "") {
                errMessage += "*Please Select PO Type.\n";
            }
            if (errMessage != "") {
                alert(errMessage);
                return false;
            }
            else {
                return true;
            }
        }

        //To Show Part Master
        function ShowMultiPartSearch(objNewPartLabel, sDealerId, bDistSuppl, sSupplierID) {
            debugger;
            if (Validation() == false) {
                return false;
            }
            //debugger;
            //alert(document.getElementById('' + objNewPartLabel.id + ''));
            //document.getElementById(''+objNewPartLabel.id+'').addEventListener("onclick", function () {
            //    var ret = window.showModalDialog("frmSelectMultiPartSearch.aspx", "some argument", "dialogWidth:500px;dialogHeight:200px");
            //    alert("Returned from modal: " + ret);
            //});
            var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            //PartDetailsValue = window.open("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + bDistSuppl + "&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=SPOPartDetails&TransFrom=Common", "_blank", "height:300px,width:550px,left=175px,top=150px,resizable=yes,location=no,scrollbars=yes,toolbar=no,status=no");
            //PartDetailsValue = window.showModalDialog("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + bDistSuppl + "&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=SPOPartDetails&TransFrom=Common", "List", "dialogHeight: 550px; dialogWidth: 500px;");
            //alert("Returned from modal: " + PartDetailsValue);
            //if (PartDetailsValue !== undefined) {
            //   alert("Please After Select Part click on OK Button");
            //    return false;
            // }

            //PartDetailsValue = window.openthebox("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + bDistSuppl + "&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=SPOPartDetails&TransFrom=Common", "List", "dialogHeight: 550px; dialogWidth: 500px;");
            // PartDetailsValue = window.open("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + bDistSuppl + "&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=SPOPartDetails&TransFrom=Common", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=200, left=500, width=800, height=600");
            //PartDetailsValue = window.open("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + bDistSuppl + "&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=SPOPartDetails&TransFrom=Common", '', 'toolbar=0,titlebar=0,fullscreen=1,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=800,height=600,left = 82,top = 54', '');
            return true;
        }

        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }
        //function refresh(event) {
        //    //debugger;
        //    if (116 == event.keyCode || 8 == event.keyCode) {
        //        event.keyCode = 0;
        //        event.returnValue = false
        //        return false;
        //    }
        //}

        //document.onkeydown = function () {
        //    refresh(event);
        //}

        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            if (Validation() == false) {
                return false;
            }
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var filename = objFileUpload.value;
            var sFileExtension = filename.split('.')[filename.split('.').length - 1].toLowerCase();
            var iFileSize = objFileUpload.size;
            var iConvert = (objFileUpload.size / 10485760).toFixed(2);
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

        function CalculatePOPartTotal(event, ObjQtyControl) {
            if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
                //ObjControl.focus(); 
                return;
            }
            else {
                var drpPoType = document.getElementById('ContentPlaceHolder1_drpPoType').value;
                var objID = $("#" + ObjQtyControl.id);
                var objRow = objID[0].parentNode.parentNode;  //ObjQtyControl.parentNode.parentNode.childNodes;               

                var MOQ = dGetValue(objRow.cells[4].children[0].value);//dGetValue(objRow[4].childNodes[0].value);
                var Qty = dGetValue(ObjQtyControl.value);

                if (drpPoType != 11) {// 11 is For Local Order
                    if (MOQ != 0 && (Qty % MOQ) != 0) {
                        if (Qty / MOQ != 0) {
                            ObjQtyControl.value = (parseInt(Qty / MOQ) + 1) * MOQ
                        }
                    }
                }
                //else {
                //    alert("Local Order");
                //}

                //GetFoBRate                   
                var FOBRate = dGetValue(objRow.cells[6].children[0].value);//dGetValue(objRow[6].childNodes[0].value);
                var Total = dGetValue(ObjQtyControl.value) * FOBRate;
                //SetNewLabel Display
                //objRow[7].childNodes[0].value = RoundupValue(Total);
                objRow.cells[7].children[0].value = RoundupValue(Total);
                CalulatePOGranTotal()

            }
        }

        function CalulatePOGranTotal() {
            var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
            var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");
            var objID = $("#ContentPlaceHolder1_PartGrid");
            var objGrid = objID[0];  //document.getElementById("ContentPlaceHolder1_PartGrid");           
            //var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
            var qty, Rate;
            var TotalRate = 0;
            var totalQtypart = 0;
            var sPArtName = "";
            var sStatus = "";
            var CountRow = objGrid.rows.length;
            //Sujata 19022011
            //for (var i = 1; i < CountRow - 1; i++)

            //Shyamal 02062012
            for (var i = 1; i < CountRow; i++)
                //Sujata 19022011
            {
                //Shyamal 02062012
                //if (objGrid.rows[i].className.indexOf('RowStyle') > 0) {
                //qty = objGrid.rows[i].childNodes[5].children[0].value;
                qty = objGrid.rows[i].cells[5].children[0].value;//objGrid.rows[i].childNodes[5].children[0].value;
                Rate = objGrid.rows[i].cells[6].children[0].value;//objGrid.rows[i].childNodes[6].children[0].value;
                sPArtName = objGrid.rows[i].cells[3].children[0].value;//objGrid.rows[i].childNodes[3].children[0].value;
                sStatus = objGrid.rows[i].cells[9].children[0].value;//objGrid.rows[i].childNodes[9].children[0].value;
                if (sPArtName != "" && sStatus != "C" && sStatus != "D") {
                    TotalRate = dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
                    totalQtypart = dGetValue(totalQtypart) + dGetValue(qty);
                    // }
                }
            }
            txtTotalQty.value = totalQtypart;
            //Sujata 19022011
            //txtTotal.value = TotalRate;
            //Shyamal 02062012,Added toFixed(2)
            txtTotal.value = parseFloat(TotalRate).toFixed(2);
            //Shyamal 02062012
            //Sujata 19022011
        }

        // Function for Cancel button
        function ClearRowValueForPO(event, objCancelControl) {
            var objID = $("#" + objCancelControl.id);
            var objRow = objID[0].parentNode.parentNode;  //objCancelControl.parentNode.parentNode.childNodes;

            var i = 1;

            //objCancelControl.style.display = "none";
            objCancelControl.innerHTML = "Cancelled"
            objCancelControl.disabled = "true"

            //            //Set PartId;
            //            objRow[1].childNodes[0].value = '';

            //            //SetPartNo

            //            objRow[2].children[0].value = "";
            //            //objRow[2].children[0].style.display = "none";


            //            //SetPartName
            //            objRow[3].childNodes[0].value = '';

            //            //Set MOQ
            //            objRow[4].childNodes[0].value = '0';

            //            //SetQuantity
            //            objRow[5].childNodes[0].value = '0';

            //            //SetFoBRate        
            //            objRow[6].childNodes[0].value = '0';

            //            //Total
            //            objRow[7].childNodes[0].value = '0';
            //Status
            //objRow[9].childNodes[0].value = 'C';
            objRow.cells[9].children[0].value = 'C';

            // objRow[2].children[0].style.display = "none";
            // objRow[2].children[1].style.display = "";
            //SetNewLabel Display
            //objRow[].children[1].style.display = "none";
            CalulatePOGranTotal();
        }
        function SelectDeleteRowValueForPO(event, objCancelControl, objDeleteCheck) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;

            if (objDeleteCheck.checked == true) {
                objRow[9].childNodes[0].value = 'D';
            }
            else {
                objRow[9].childNodes[0].value = 'E';
            }
            CalulatePOGranTotal();
            return false
        }
        function SelectDeletCheckboxPO(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    SelectDeleteRowValueForPO(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForPO(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
            else {
                if (confirm("Are you sure you want to revert changes?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    SelectDeleteRowValueForPO(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForPO(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }

            }
        }

    </script>
    
    <script type="text/javascript">
        window.onkeydown = function (event) {
            ////debugger;
            if (event.keyCode == 8 || event.keyCode == 116) {
                if (event.preventDefault)
                    event.preventDefault();
                event.keyCode = 0;
                event.returnValue = false
                return false;
            };
            return true;
        }
    </script>
    <script>
        function ChkSpNDPPartClick1234(objImgControl) {
            debugger;
            var objID = $('#' + objImgControl.id);
            var objCol = objID[0].parentNode.parentNode;
            var txtparst = document.getElementById("ContentPlaceHolder1_txtPartIds");
            //Changed by Vikram Date 17.06.2016
            //objImgControl.parentNode.parentNode.childNodes;
            //var objCol = objImgControl.parentNode.parentNode
            ////var txtparst = document.getElementById('txtPartIds');
            //var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds');

            var ArrOfPartDtls = '';
            var removePartID;
            //    var sPartID = objRow[1].innerText;
            //    var sParFOBRt = objRow[2].innerText;
            //    var sParMOQ = objRow[3].innerText;
            //    var sParNo = objRow[4].innerText;
            //    var sParName = objRow[5].innerText;
            //    var sNDPRate = objRow[6].innerText;

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
    
<style type="text/css">
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=60);
        opacity: 0.6;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        width: 500px;
        border: 12px solid #0DA9D0;
        border-radius: 12px;
        padding:0
      
    }
    .modalPopup .header
    {
        background-color: #2FBDF1;
        height: 30px;
        color: White;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
    }
    .modalPopup .body
    {
        min-height: 50px;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
    }
    .modalPopup .footer
    {
        padding: 6px;
    }
    .modalPopup .yes, .modalPopup .no
    {
        height: 23px;
        color: White;
        line-height: 23px;
        text-align: center;
        font-weight: bold;
        cursor: pointer;
        border-radius: 4px;
    }
    .modalPopup .yes
    {
        background-color: #2FBDF1;
        border: 1px solid #0DA9D0;
    }
    .modalPopup .no
    {
        background-color: #9F9F9F;
        border: 1px solid #5C5C5C;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Purchase Order"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="">
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
                    <%--<asp:Label runat ="server" ID="lblEGPDealer" Text ="Dealer Name" class="tdLabel"></asp:Label>
                    <asp:DropDownList ID="ddlEGPDealer" runat="server" CssClass ="ComboBoxFixedSize" Width ="30%" ></asp:DropDownList>--%>
                    <%--<uc2:ExportLocation ID="ExportLocation" runat ="server" />--%>
                    <uc2:ExportLocation runat="server" ID="ExportLocation" />
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
                        SuppressPostBack="true" CollapsedText="PO Details" ExpandedText="PO Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">PO Type:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpPoType" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpPoType_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtPoType" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                    <asp:Label ID="lblMPoType" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblPONo" runat="server" Text="PO No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblPODate" runat="server" Text="PO Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtPODate" runat="server" bCheckforCurrentDate="false" Enabled="false" />
                                    <asp:TextBox ID="txtPreviousPoDate" runat="server" EnableViewState="false" CssClass="HideControl"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trPCRDetails" runat="server">
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                                    <asp:Label ID="lblChasisNo" runat="server" Text="Chassis No:"></asp:Label></td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:TextBox ID="txtCreatedBy" Text="" runat="server" CssClass="TextBoxForString"
                                        Enabled="false" Style="display: none"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">
                                    <asp:Label ID="lblEngNo" runat="server" Text="Engine No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEngineNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblJobCardNo" runat="server" Text="JobCard Ref"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtJobCardNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:DropDownList ID="DrpEstimate" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="DrpEstimate_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 18%" colspan="6">
                                    <asp:Button ID="btnShortClose" runat="server"
                                        Text="Shortclose" CssClass="btn btn-search" OnClick="btnShortClose_Click" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="UploafFile" runat="server" BorderColor="DarkGray" BorderStyle="Double" Style="display: none">
                    <table id="Table1" runat="server" class="table table-bordered">
                        <tr>
                            <td style="width: 15%; padding-left: 10px;">Select File For Upload
                            </td>
                            <td style="width: 65%; padding-left: 10px;">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="75%" CssClass="Cntrl1" />
                            </td>
                            <td style="width: 10%;">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-search btn-sm" Text="Upload"
                                    OnClientClick="return CheckBeforeUploadClick(this,'FileUpload1');" OnClick="btnUpload_Click" />
                                <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <img src="../../Images/Search_Progress.gif" alt="Searchig" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px; color: Red" colspan="4">
                                <p>
                                    1.
                                    <a>Please select the file in the excel format. File name should be in format as 'DealerCode_SparesPO_PartDetails_Datestamp'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;D002500_SparesPO_PartDetails_11072016&#39;. </a>
                                    <br />
                                    2.
                                   <a>Superceded Part Show in Red Color.</a>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 10px;">
                                <asp:Label ID="lblListPartNo" runat="server" Text="" Width="100%" ForeColor="Red"
                                    Visible="false"> </asp:Label>
                                <asp:TextBox ID="txtListPartNo" TextMode="MultiLine" runat="server" Text="" Width="100%"
                                    ForeColor="Red" Visible="false"></asp:TextBox>
                                <%--#49A3D3--%>
                            </td>
                        </tr>
                    </table>
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
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            CssClass="table table-condensed table-bordered"
                            AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnPageIndexChanging="PartGrid_PageIndexChanging"
                            OnRowCommand="PartGrid_RowCommand" OnRowDataBound="PartGrid_RowDataBound">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" />
                                        <%--Text='<%# Container.DataItemIndex   %>'--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartID" runat="server" Width="1%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:LinkButton ID="lnkSelectPart" runat="server" CssClass="btn btn-link" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                    </ItemTemplate>
                                    <%--<ItemStyle Width="7%" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMOQ" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("MOQ") %>'
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PO Qty" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Qty","{0:#0}") %>' MaxLength="4"
                                            Width="90%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculatePOPartTotal(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("MRPRate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxPO(event,this);" />
                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                        <%--<asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>--%>
                                        <%--<asp:LinkButton ID="lblCancel" CssClass="btn btn-link btn-sm"
                                            OnClientClick="return ClearRowValueForPO(event,this);" runat="server" OnClick="lblCancel_Click">Delete</asp:LinkButton>--%>
                                        <%-- onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);" --%>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtJobDtlID" runat="server" Width="5%" Text='<%# Eval("JobDtlID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                    <table id="tblTotal" runat="server" class="ContainTable" width="100%">
                        <tr>
                            <td style="width: 55%; text-align: right">
                                <b>Total:</b>
                            </td>
                            <td style="width: 9%; text-align: left">
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
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnPoTypeID" runat="server" />
                <asp:HiddenField ID="hdnPartsIDs" runat="server" />
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="N" />
                <asp:TextBox ID="txtJobcardHDRID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
    <cc1:ModalPopupExtender ID="ModalPopUpExtender" runat="server" TargetControlID="lblTragetID" PopupControlID="pnlPopupWindow"
        OkControlID="btnOK"  BackgroundCssClass="modalBackground" >
       <%-- <Animations>
            <OnShown><Fadein Duration="0.50" /></OnShown>
            <OnHiding><Fadeout Duration="0.75" /></OnHiding>
        </Animations>--%>
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup"  Style="display: none">
        <table class="PageTable">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="PageTitle panel-title" align="center">
                    <asp:Label ID="Label1" runat="server" Text="Part Master">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl1">
                <td >
                    <div align="center" class="ContainTable">
                        <table class="table-bordered" >
                            <tr align="center">
                                <td class="tdLabel">Search:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtSearch" runat="server" ></asp:TextBox>
                                </td>
                                <td class="tdLabel">
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" >
                                        <asp:ListItem Selected="True" Value="P">Part No</asp:ListItem>
                                        <asp:ListItem Value="N">Part Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel">
                                    <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                    <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                        OnClick="btnSave_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="tdLabel">
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
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClick1234(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
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

                                <%-- <asp:TemplateField HeaderText="Part_No_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartNoID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> --%>
                                <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMOQ" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("MOQ") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Qty","{0:#0}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>


                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Warrantable" ItemStyle-Width="5%" ItemStyle-CssClass="LabelCenterAlign">
                                <ItemTemplate>
                                    <asp:Label ID="lblWarrantable" runat="server" Text='<%# Eval("Warrantable") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            </Columns>
                            <HeaderStyle Wrap="True" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                        <cc2:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" Width="3000px" PageSize="15"
                            GenerateGoToSection="true" />
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TmpControl1">
                <td style="width: 15%">
                    <asp:TextBox ID="txtPartIds" CssClass="HideControl txtPartIds" runat="server" Width="1px"></asp:TextBox>

                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
