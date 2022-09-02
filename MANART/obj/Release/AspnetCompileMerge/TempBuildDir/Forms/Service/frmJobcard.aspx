<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    Theme="SkinFile" EnableEventValidation="false" CodeBehind="frmJobcard.aspx.cs" Inherits="MANART.Forms.Service.frmJobcard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%--<%@ Register Src="~/WebParts/ExportLocation.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%--<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>--%>
<%@ Register Src="~/WebParts/CurrentDateTime.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
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
    <script src="../../Scripts/jsJobcardFunction.js"></script>
    <script src="../../Scripts/jsJobcardPartFunc.js"></script>
    <script src="../../Scripts/jsJobcardLabourFunc.js"></script>
    <script src="../../Scripts/jsJobcardJobCode.js"></script>
    <script src="../../Scripts/jsWCServiceHistory.js"></script>
    <%--<script src="../../Scripts/showModalDialog.js"></script>--%>
    <script src="../../Scripts/jsFileAttach.js"></script>
    <style type="text/css">
        .scrolling-table-container {
            overflow-y: hidden;
            overflow-x: scroll;
        }
    </style>
    <%--<script>

        function CreateGridHeader(DataDiv, PartDetailsGrid, HeaderDiv) {
            //alert("CreateGridHeader function calling");
            //debugger;
            var DataDivObj = document.getElementById('DataDiv');
            var DataGridObj = document.getElementById('ContentPlaceHolder1_PartDetailsGrid');
            var HeaderDivObj = document.getElementById('HeaderDiv');
            //var DataDivObj123 = document.getElementById('ContentPlaceHolder1_PartDetailsGrid');
            //alert(DataDivObj123);
            //DataGridObj = DataDivObj123;
            //********* Creating new table which contains the header row ***********
            var HeadertableObj = HeaderDivObj.appendChild(document.createElement('table'));
            DataDivObj.style.paddingTop = '0px';
            var DataDivWidth = DataDivObj.clientWidth;
            DataDivObj.style.width = '160%';
            //DataDivObj.style.width = '5000px';
            //DataDivObj.style.width = 'auto';
            //********** Setting the style of Header Div as per the Data Div ************
            HeaderDivObj.className = DataDivObj.className;
            HeaderDivObj.style.cssText = DataDivObj.style.cssText;
            //**** Making the Header Div scrollable. *****
            HeaderDivObj.style.overflow = 'auto';
            //*** Hiding the horizontal scroll bar of Header Div ****
            //*** this is because we have to scroll the Div along with the DataDiv.
            HeaderDivObj.style.overflowX = 'hidden';
            //**** Hiding the vertical scroll bar of Header Div ****
            HeaderDivObj.style.overflowY = 'hidden';
            HeaderDivObj.style.height = DataGridObj.rows[0].clientHeight + 'px';
            //**** Removing any border between Header Div and Data Div ****
            HeaderDivObj.style.borderBottomWidth = '0px';
            //********** Setting the style of Header Table as per the GridView ************
            HeadertableObj.className = DataGridObj.className;
            //**** Setting the Headertable css text as per the GridView css text
            HeadertableObj.style.cssText = DataGridObj.style.cssText;
            HeadertableObj.border = '1px';
            HeadertableObj.rules = 'all';
            HeadertableObj.cellPadding = DataGridObj.cellPadding;
            HeadertableObj.cellSpacing = DataGridObj.cellSpacing;
            //********** Creating the new header row **********
            var Row = HeadertableObj.insertRow(0);
            Row.className = DataGridObj.rows[0].className;
            Row.style.cssText = DataGridObj.rows[0].style.cssText;
            Row.style.fontWeight = 'bold';
            //******** This loop will create each header cell *********
            for (var iCntr = 0; iCntr < DataGridObj.rows[0].cells.length; iCntr++) {
                var spanTag = Row.appendChild(document.createElement('td'));
                spanTag.innerHTML = DataGridObj.rows[0].cells[iCntr].innerHTML;
                var width = 0;
                //****** Setting the width of Header Cell **********
                if (spanTag.clientWidth > DataGridObj.rows[1].cells[iCntr].clientWidth) {
                    width = spanTag.clientWidth;
                }
                else {
                    width = DataGridObj.rows[1].cells[iCntr].clientWidth;
                }
                if (iCntr <= DataGridObj.rows[0].cells.length - 2) {
                    spanTag.style.width = width + 'px';
                }
                else {
                    spanTag.style.width = width + 20 + 'px';
                }
                DataGridObj.rows[1].cells[iCntr].style.width = width + 'px';
            }
            var tableWidth = DataGridObj.clientWidth;
            //********* Hidding the original header of GridView *******
            DataGridObj.rows[0].style.display = 'none';
            //********* Setting the same width of all the components **********
            HeaderDivObj.style.width = DataDivWidth + 'px';
            DataDivObj.style.width = DataDivWidth + 'px';
            DataGridObj.style.width = tableWidth + 'px';
            HeadertableObj.style.width = tableWidth + 20 + 'px';
            return false;
        }
        function Onscrollfnction() {
            //alert("Onscrollfnction Function calling");
            debugger;
            var div = document.getElementById('DataDiv');
            var div2 = document.getElementById('HeaderDiv');
            alert(div);
            //****** Scrolling HeaderDiv along with DataDiv ******
            div2.scrollLeft = div.scrollLeft;
            return false;
        }
    </script>--%>
    <script type="text/javascript">
        //To Show Chassis Master
        function ShowChassisMaster(objNewModelLabel, sDealerId, sHOBR_ID, sUserDepart) {
            //debugger;
            var ChassisDetailsValues;
            var sJobtype = "";
            var drpJobType = window.document.getElementById('ContentPlaceHolder1_drpJobType');
            if (drpJobType == null) return;
            if (drpJobType.selectedIndex == 0) {
                alert('Please Select Jobcard Type !');
            }
            else {
                sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
                ChassisDetailsValues = window.showModalDialog("../Common/frmChassisSelection.aspx?DealerID=" + sDealerId + "&HOBR_ID=" + sHOBR_ID + "&JobTypeID=" + sJobtype +"&Screen=J", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
                if (ChassisDetailsValues != null) {
                    //SetChassisDetails(ChassisDetailsValues, sUserDepart);
                    var txtPreviousDocId = window.document.getElementById('ContentPlaceHolder1_txtPreviousDocId');
                    var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
                    var hdnChassisID = window.document.getElementById('ContentPlaceHolder1_hdnChassisID');
                    var hdnVehInID = window.document.getElementById('ContentPlaceHolder1_hdnVehInID');
                    var txtCRMID = window.document.getElementById('ContentPlaceHolder1_txtCRMID');

                    txtChassisID.value = ChassisDetailsValues[1];
                    txtPreviousDocId.value = ChassisDetailsValues[17];
                    hdnChassisID.value = ChassisDetailsValues[1];
                    hdnVehInID.value = ChassisDetailsValues[17];
                    txtCRMID.value = ChassisDetailsValues[35];
                }
            }
            return true;
        }



        //To Show Preshipment Qty Editing Form
        function GetJobcodeDtls(objLink, sDealerId, sJobCodeID, sPCRID, sJobSavedORNot) {
            var iJob_HDR_ID = 0;
            var objRow = objLink.parentNode.parentNode.childNodes;
            var ObjControl = window.document.getElementById("ContentPlaceHolder1_txtID");
            //Get ORF ID
            if (ObjControl != null) {
                iJob_HDR_ID = dGetValue(ObjControl.value);
            }
            //debugger;
            if (sJobSavedORNot == "N" || sJobSavedORNot == "") {
                alert("Please Save Jobcode details before enter PCR");
                return;
            }

            var Parameters = "JobID=" + iJob_HDR_ID + "&DealerID=" + sDealerId + "&JobCodeID=" + sJobCodeID + "&PCRID=" + sPCRID + "&Display=N&sFOR=JbC";
            var feature = "dialogWidth:1000px;dialogHeight:900px;status:no;help:no;scrollbars:no;resizable:no;";
            var PCRId;
            PCRId = window.showModalDialog("../Service/frmJobcodeDtls.aspx?" + Parameters, null, feature);
            if (PCRId != null) {
                objRow[11].childNodes[1].value = PCRId;
            }
            return;
        }

        function GetGatePassDtls(objLink, sDealerId, sJbInvID, sSlInvID) {
            var iJob_HDR_ID = 0;
            var objRow = objLink.parentNode.parentNode.childNodes;
            var ObjControl = window.document.getElementById("ContentPlaceHolder1_txtID");
            var ObjhdnGPID = window.document.getElementById("ContentPlaceHolder1_hdnGPID");
            var GPID;
            //Get ORF ID
            if (ObjControl != null) {
                iJob_HDR_ID = dGetValue(ObjControl.value);
            }
            if (ObjhdnGPID != null) {
                GPID = dGetValue(ObjhdnGPID.value);
            }
            //debugger;  
            var Parameters = "JobID=" + iJob_HDR_ID + "&DealerID=" + sDealerId + "&GPID=" + GPID;
            var feature = "dialogWidth:1000px;dialogHeight:300px;status:no;help:no;scrollbars:no;resizable:no;";

            GPID = window.showModalDialog("../Service/frmGatepass.aspx?" + Parameters, null, feature);
            if (GPID != null) {
                ObjhdnGPID.value = GPID;
            }
            return;
        }

        function GetSrvVANDtls(objLink, sDealerId) {
            var iJob_HDR_ID = 0;
            var objRow = objLink.parentNode.parentNode.childNodes;
            var ObjControl = window.document.getElementById("ContentPlaceHolder1_txtID");
            var ObjhdnSrvVANID = window.document.getElementById("ContentPlaceHolder1_hdnSrvVANID");
            var ObjtxtWarrantyTag = window.document.getElementById("ContentPlaceHolder1_txtWarrantyTag");
            var ObjtxtAggregate = window.document.getElementById("ContentPlaceHolder1_txtAggregate");
            var SrvVANID;
            var WarrTag, Aggregate;
            //Get ORF ID
            if (ObjControl != null) {
                iJob_HDR_ID = dGetValue(ObjControl.value);
            }
            if (ObjhdnSrvVANID != null) {
                SrvVANID = dGetValue(ObjhdnSrvVANID.value);
            }
            if (ObjtxtWarrantyTag != null) {
                WarrTag = ObjtxtWarrantyTag.value;
            }
            if (ObjtxtAggregate != null) {
                Aggregate = ObjtxtAggregate.value;
            }
            //debugger;
            var Parameters = "JobID=" + iJob_HDR_ID + "&DealerID=" + sDealerId + "&SrvVANHDRID=" + SrvVANID + "&sWarrTag=" + WarrTag + "&sAggregate=" + Aggregate;
            var feature = "dialogWidth:1000px;dialogHeight:300px;status:no;help:no;scrollbars:no;resizable:no;";

            SrvVANID = window.showModalDialog("../Service/frmServiceVANCharges.aspx?" + Parameters, null, feature);
            if (SrvVANID != null) {
                ObjhdnSrvVANID.value = SrvVANID;
            }
            return;
        }

        //To Show Claim Request Form
        function GetClaimRequestDetails(objLink, sDealerId, sReqID, ReqType, sDealerCode, sStateID) {
            var iJob_HDR_ID = 0;
            var sJobtype = 0;
            //debugger;
            var ObjControl = window.document.getElementById("ContentPlaceHolder1_txtID");

            if (ReqType == "H") var ObjtxtHVReqID = window.document.getElementById("ContentPlaceHolder1_txtHVReqID");
            if (ReqType == "G") var ObjtxtGDReqID = window.document.getElementById("ContentPlaceHolder1_txtGDReqID");

            //Get ORF ID
            if (ObjControl != null) {
                iJob_HDR_ID = dGetValue(ObjControl.value);
            }
            if (ReqType == "H") {
                if (ObjtxtHVReqID != null) sReqID = dGetValue(ObjtxtHVReqID.value);
            }
            if (ReqType == "G") {
                if (ObjtxtGDReqID != null) sReqID = dGetValue(ObjtxtGDReqID.value);
            }
            var drpJobType = window.document.getElementById('ContentPlaceHolder1_drpJobType');
            if (drpJobType != null) sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
            //debugger;

            var Parameters = "MenuID=415&JobID=" + iJob_HDR_ID + "&DealerID=" + sDealerId + "&ReqID=" + sReqID + "&ReqType=" + ReqType + "&JobType=" + sJobtype + "&DealerCode=" + sDealerCode + "&StateID=" + sStateID;
            var feature = "dialogWidth:1000px;dialogHeight:900px;status:no;help:no;scrollbars:no;resizable:no;";

            sReqID = window.showModalDialog("../Warranty/frmRequestCreation.aspx?" + Parameters, "list", feature);
            //alert(sReqID);
            if (sReqID != null) {
                if (ReqType == "H" && sReqID != "0" && sReqID != "") ObjtxtHVReqID.value = sReqID;
                if (ReqType == "G" && sReqID != "0" && sReqID != "") ObjtxtGDReqID.value = sReqID;
            }
            return;
        }

        //Function For Set Chassis Details
        function SetChassisDetails(ChassisDetailsValue, sUserDepart) {
            //debugger;
            var txtChassisNo = window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
            var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
            var txtVehicleNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleNo');
            var txtEngineNo = window.document.getElementById('ContentPlaceHolder1_txtEngineNo');
            var txtCustomer = window.document.getElementById('ContentPlaceHolder1_txtCustomer');
            var txtCustEdit = window.document.getElementById('ContentPlaceHolder1_txtCustEdit');
            var txtCustID = window.document.getElementById('ContentPlaceHolder1_txtCustID');
            var TxtModelCode = window.document.getElementById('ContentPlaceHolder1_TxtModelCode');
            var txtModelName = window.document.getElementById('ContentPlaceHolder1_txtModelName');

            var txtAggregate = window.document.getElementById('ContentPlaceHolder1_txtAggregate');
            var txtPrevAggregate = window.document.getElementById('ContentPlaceHolder1_txtPrevAggregate');

            var txtCAggregate = window.document.getElementById('ContentPlaceHolder1_txtCAggregate');

            var txtWarrantyTag = window.document.getElementById('ContentPlaceHolder1_txtWarrantyTag');

            var txtNormalWarrantyTag = window.document.getElementById('ContentPlaceHolder1_txtNormalWarrantyTag');
            var txtExtndWarrTag = window.document.getElementById('ContentPlaceHolder1_txtExtndWarrTag');
            var txtAddnWarrTag = window.document.getElementById('ContentPlaceHolder1_txtAddnWarrTag');

            var txtAMCChk = window.document.getElementById('ContentPlaceHolder1_txtAMCChk');
            var txtCAMCChk = window.document.getElementById('ContentPlaceHolder1_txtCAMCChk');

            var txtAMCType = window.document.getElementById('ContentPlaceHolder1_txtAMCType');
            var hdnAMCType = window.document.getElementById('ContentPlaceHolder1_hdnAMCType');
            var txtModelGroupID = window.document.getElementById('ContentPlaceHolder1_txtModelGroupID');

            var txtPreviousDocId = window.document.getElementById('ContentPlaceHolder1_txtPreviousDocId');
            var txtVehicleInNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleInNo');
            //var dtpVehInTime = window.document.getElementById('ContentPlaceHolder1_dtpVehInTime_txtDocDate');
            var dtpVehInTime = window.document.getElementById('ContentPlaceHolder1_dtpVehInTime');

            var txtLastKms = window.document.getElementById('ContentPlaceHolder1_txtLastKms');
            var txtLastHrs = window.document.getElementById('ContentPlaceHolder1_txtLastHrs');

            var txtLstJbKms = window.document.getElementById('ContentPlaceHolder1_txtLstJbKms');
            var txtLstJbHrs = window.document.getElementById('ContentPlaceHolder1_txtLstJbHrs');

            var hdnAMCStKms = window.document.getElementById('ContentPlaceHolder1_hdnAMCStKms');
            var hdnAMCEndKms = window.document.getElementById('ContentPlaceHolder1_hdnAMCEndKms');

            var txtBfr_Last_SpdMtrChange_Kms = window.document.getElementById('ContentPlaceHolder1_txtBfr_Last_SpdMtrChange_Kms');
            var txtBfr_Last_HrsMtrChange_Hrs = window.document.getElementById('ContentPlaceHolder1_txtBfr_Last_HrsMtrChange_Hrs');
            var txtKAM = window.document.getElementById('ContentPlaceHolder1_txtKAM');
            var txtUpgCamp = window.document.getElementById('ContentPlaceHolder1_txtUpgCamp');
            var txtAMCDate = window.document.getElementById('ContentPlaceHolder1_txtAMCDate');
            var txtUndObserv = window.document.getElementById('ContentPlaceHolder1_txtUndObserv');
            var txtObservEffFrom = window.document.getElementById('ContentPlaceHolder1_txtObservEffFrom');
            var txtObservEffTo = window.document.getElementById('ContentPlaceHolder1_txtObservEffTo');
            var txtFloatPart = window.document.getElementById('ContentPlaceHolder1_txtFloatPart');
            var txtIsTheft = window.document.getElementById('ContentPlaceHolder1_txtIsTheft');
            var hdnFloatPart = window.document.getElementById('ContentPlaceHolder1_hdnFloatPart');
            var IsTheft = window.document.getElementById('ContentPlaceHolder1_IsTheft');

            var txtSpdMtrChg = window.document.getElementById('ContentPlaceHolder1_txtSpdMtrChg');
            var txtTotKm = window.document.getElementById('ContentPlaceHolder1_txtTotKm');
            var txtKms = window.document.getElementById('ContentPlaceHolder1_txtKms');

            var txtHrsMtrChg = window.document.getElementById('ContentPlaceHolder1_txtHrsMtrChg');
            var txtTotHrs = window.document.getElementById('ContentPlaceHolder1_txtTotHrs');
            var txtHrs = window.document.getElementById('ContentPlaceHolder1_txtHrs');
            var txtModCatIDBasic = window.document.getElementById('ContentPlaceHolder1_txtModCatIDBasic');

            txtChassisID.value = ChassisDetailsValue[1];
            txtChassisNo.value = ChassisDetailsValue[2];
            txtVehicleNo.value = ChassisDetailsValue[3];
            txtCustomer.value = ChassisDetailsValue[4];

            txtVehicleInNo.value = ChassisDetailsValue[5];
            dtpVehInTime.value = ChassisDetailsValue[6];

            txtEngineNo.value = ChassisDetailsValue[7];
            txtCustEdit.value = ChassisDetailsValue[8];
            txtCustID.value = ChassisDetailsValue[9];
            TxtModelCode.value = ChassisDetailsValue[10];
            txtModelName.value = ChassisDetailsValue[11];

            txtAggregate.value = ChassisDetailsValue[12];
            txtPrevAggregate.value = ChassisDetailsValue[12];
            txtCAggregate.value = (ChassisDetailsValue[12] == "G") ? "Yes" : "No";
            txtWarrantyTag.value = ChassisDetailsValue[13];

            txtNormalWarrantyTag.value = (ChassisDetailsValue[13] == "W") ? "Yes" : "No";
            txtExtndWarrTag.value = (ChassisDetailsValue[13] == "E") ? "Yes" : "No";
            txtAddnWarrTag.value = (ChassisDetailsValue[13] == "A") ? "Yes" : "No";

            txtAMCChk.value = ChassisDetailsValue[14];
            txtCAMCChk.value = (ChassisDetailsValue[14] == "Y") ? "Yes" : "No";
            hdnAMCType.value = ChassisDetailsValue[15];

            //txtAMCType.value = (ChassisDetailsValue[14] == "N") ? "" : (ChassisDetailsValue[15] == "L") ? "Labor" : "Labor & Part";
            txtAMCType.value = (ChassisDetailsValue[14] == "N") ? "" : (ChassisDetailsValue[15] == "C") ? "Comfort" : (ChassisDetailsValue[15] == "S") ? "Comfort Super" : "Comfort Premium";

            txtModelGroupID.value = ChassisDetailsValue[16];

            txtPreviousDocId.value = ChassisDetailsValue[17];

            txtLastKms.value = ChassisDetailsValue[18];
            txtLastHrs.value = ChassisDetailsValue[19];

            hdnAMCStKms.value = ChassisDetailsValue[20];
            hdnAMCEndKms.value = ChassisDetailsValue[21];

            txtBfr_Last_SpdMtrChange_Kms.value = ChassisDetailsValue[22];
            txtBfr_Last_HrsMtrChange_Hrs.value = ChassisDetailsValue[23];

            txtKAM.value = (ChassisDetailsValue[24] == "Y") ? "Yes" : "No";
            txtUpgCamp.value = (ChassisDetailsValue[25] == "Y") ? "Yes" : "No";
            txtAMCDate.value = ChassisDetailsValue[26];
            txtUndObserv.value = (ChassisDetailsValue[27] == "Y") ? "Yes" : "No";
            txtObservEffFrom.value = ChassisDetailsValue[28];
            txtObservEffTo.value = ChassisDetailsValue[29];

            txtFloatPart.value = (ChassisDetailsValue[30] == "Y") ? "Yes" : "No";
            txtIsTheft.value = (ChassisDetailsValue[31] == "Y") ? "Yes" : "No";
            hdnFloatPart.value = ChassisDetailsValue[30];
            IsTheft.value = ChassisDetailsValue[31];

            txtLstJbKms.value = ChassisDetailsValue[32];
            txtLstJbHrs.value = ChassisDetailsValue[33];

            txtSpdMtrChg.value = "No";
            txtHrsMtrChg.value = "No";

            txtTotKm.value = txtLastKms.value;
            txtTotHrs.value = txtLastHrs.value;

            txtKms.value = "0";
            txtHrs.value = "0";
            //debugger;
            txtModCatIDBasic.value = ChassisDetailsValue[34];

            SetGridColumnsBasedOnChStatusJobType(sUserDepart);

            //        if (txtCustEdit.value == "Y") {
            //            txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
            //        }
            //        else {
            //            txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
            //        }
        }

    </script>
    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
                ////debugger;
                //var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
                var dtpAllocateTime = document.getElementById("ContentPlaceHolder1_dtpAllocateTime_txtDocDate");
                var dtpVehSaleDt = document.getElementById("ContentPlaceHolder1_dtpVehSaleDt_txtDocDate");
                var dtpJobCommited = document.getElementById("ContentPlaceHolder1_dtpJobCommited_txtDocDate");
                var dtpJobOpeningTm = document.getElementById("ContentPlaceHolder1_dtpJobOpeningTm_txtDocDate");
                var txtFailureDt = document.getElementById("ContentPlaceHolder1_txtFailureDt_txtDocDate");
                var DtpVehicleOut = document.getElementById("ContentPlaceHolder1_DtpVehicleOut_txtDocDate");

                var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
                $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick({
                    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
                });

                $('#ContentPlaceHolder1_dtpAllocateTime_txtDocDate').datepick({
                    //onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtShipmentDate.value == '') ? '0d' : txtShipmentDate.value
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value, maxDate: txtDocDate.value
                });

                $('#ContentPlaceHolder1_dtpVehSaleDt_txtDocDate').datepick({
                    //onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value, maxDate: txtDocDate.value
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '-5y', maxDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //For 22
                    //onSelect: customRange, dateFormat: 'dd-mm-yyyy', minDate: '-5y', maxDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                });
                //onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '-5y', maxDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                $('#ContentPlaceHolder1_dtpJobCommited_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value
                });
                $('#ContentPlaceHolder1_dtpJobOpeningTm_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value, maxDate: txtDocDate.value
                });
                $('#ContentPlaceHolder1_txtFailureDt_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value, maxDate: txtDocDate.value
                });
                $('#ContentPlaceHolder1_DtpVehicleOut_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value
                });

                function customRange(dates) {
                    if (this.id == 'ContentPlaceHolder1_txtFromDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                    }
                    else {
                        $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" style="width: 100%" class="" border="1">
            <tr id="TitleOfPage">
                <td class="panel-heading" align="center">
                    <asp:Label ID="lblTitle" CssClass="panel-title" runat="server" Text=""> </asp:Label>
                    <div style="display: none">
                        <asp:Label ID="lblHighValueMsg" runat="server" Text="(  High Value Request Amount:  "> </asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 14%">
                    <table id="ToolbarContainer" runat="server" width="100%" border="1" class="">
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
                        <uc4:SearchGridView ID="SearchGrid" runat="server"
                            bIsCallForServer="true" OnImage_Click="SearchImage_Click" />
                    </asp:Panel>
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="txtDocNoDetails" runat="server" class="ContainTable">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" colspan="6">Document Details
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:LinkButton ID="lblSelectModel" runat="server" Text="Select Chassis" Width="80%"
                                        ToolTip="Select Chassis Details" Style="height: 12px" OnClick="lblSelectModel_Click"> </asp:LinkButton>
                                    <asp:Button ID="BtnOpen" runat="server" Text="Open Jobcard" CssClass="btn btn-search btn-sm" Width="125px" OnClick="BtnOpen_Click" />
                                    <%--OnClick="lblSelectModel_Click"  onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" --%>
                                </td>
                                <td style="width: 18%">
                                    <asp:Button ID="btnJobSave" runat="server" Text="Save Jobcode" CssClass="btn btn-search btn-sm" Width="125px"
                                        OnClick="btnJobSave_Click" />
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Button ID="btnJobConfirm" runat="server" CssClass="btn btn-search btn-sm" Text="2nd Stage Confirm" Width="145px" OnClick="btnJobConfirm_Click" />
                                </td>
                                <td style="width: 18%">
                                    <asp:Button ID="btnPO" runat="server" CssClass="btn btn-search btn-sm" Text="" Width="125px" OnClick="btnPO_Click" />
                                    <asp:LinkButton ID="lnkSrvVAN" runat="server" Style="height: 12px" Text="Service VAN Details" ToolTip="Service VAN Details" Width="80%" OnClick="lnkSrvVAN_Click"> </asp:LinkButton>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:LinkButton ID="lnkRequestHV" runat="server" Text="High Value Request" Width="80%" ToolTip="High Value Request" Style="height: 12px"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%">
                                    <asp:LinkButton ID="lnkRequestGD" runat="server" Text="Goodwill Request" Width="80%" ToolTip="Goodwill Request" Style="height: 12px"> </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblDocNo" runat="server" Text="Tr No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDocNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblDocDate" runat="server" Text="Time In:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="false" Enabled="false" />
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblJobType" runat="server" Text="Job Type.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpJobType" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <b class="Mandatory">*</b>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblVehicleNo" runat="server" Text="Veh Reg No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtAggreagateNo" runat="server" CssClass="TextBoxForString" MaxLength="20"></asp:TextBox>
                                    <asp:Label ID="lblAggrMndt" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    <%--<b class="Mandatory">*</b>--%>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblChassisNo" runat="server" Text="Chassis No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lblchassisMandt" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    <%--<b class="Mandatory">*</b>--%>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblEngineNo" runat="server" Text="Engine No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="" style="width: 15%">
                                    <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpCustomer" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <%--<b class="Mandatory">*</b>--%>
                                    <asp:TextBox ID="txtCustomer" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblModel" runat="server" Text="Model code.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="TxtModelCode" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:DropDownList ID="DrpModelCode" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label4" runat="server" Text="Model Name.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtModelName" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:DropDownList ID="DrpModelName" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblKms" runat="server" Text="Kms.:"></asp:Label>
                                    <%--<asp:Label ID="lblTotKm" runat="server" EnableViewState="true"></asp:Label>--%>
                                    <asp:TextBox ID="txtTotKm" runat="server" CssClass="TextForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);" BackColor="#EFEFEF" BorderStyle="None"></asp:TextBox>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtKms" Text="" runat="server" CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'5');" MaxLength="7"
                                        onBlur="return SetJobCardTotalKms(this);"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblHrs" runat="server" Text="Hrs.:"></asp:Label>
                                    <%--<asp:Label ID="lblTotHrs" runat="server" Text="0"></asp:Label>--%>
                                    <asp:TextBox ID="txtTotHrs" runat="server" CssClass="TextForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);" BackColor="#EFEFEF" BorderStyle="None"></asp:TextBox>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtHrs" Text="" runat="server" CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'5');" MaxLength="5"
                                        onBlur="return SetJobCardTotalHrs(this);"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label8" runat="server" Text="Vehicle Sale Dealer:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpVehSaleDealer" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label1" runat="server" Text="Bay Name.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpBay" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label2" runat="server" Text="Allocation Time:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="dtpAllocateTime" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="true" />
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label7" runat="server" Text="Vehicle Sale Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="dtpVehSaleDt" runat="server" bTimeVisible="false" Mandatory="false" bCheckforCurrentDate="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label6" runat="server" Text="Call Center Ticket No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCRMTicketNo" runat="server" CssClass="TextBoxForString" Text="" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label22" runat="server" Text="Call Center Ticket Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCRMTicketDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="" style="width: 15%">
                                    <asp:LinkButton ID="lblServiceHistroy" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Service History" Width="80%"
                                        ToolTip="Service History Details"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%"></td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblReqlabel" runat="server" Text="Requisition No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpRequisitionLst" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:LinkButton ID="lblReqPrint" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Requisition Print" Width="80%"
                                        ToolTip="Show Requisition Details"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%"></td>
                                <td class="" style="width: 15%"></td>
                                <td style="width: 18%"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PChassisStatus" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisStatus" runat="server" TargetControlID="CntChassisStatus"
                            ExpandControlID="TtlChassisStatus" CollapseControlID="TtlChassisStatus" Collapsed="true"
                            ImageControlID="ImgTtlChassisStatus" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Chassis’s Status"
                            ExpandedText="Chassis’s Status" TextLabelID="lblTtlChassisStatus">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisStatus" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlChassisStatus" runat="server" Text="Chassis’s Status Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisStatus" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntChassisStatus" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="TblChassisStatus" width="100%" runat="server" class="">
                                <tr>
                                    <td style="width: 15%" class="">Warranty Flag:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtNormalWarrantyTag" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>

                                    <td style="width: 15%" class="">Extended Warranty:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtExtndWarrTag" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Additional Warranty:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAddnWarrTag" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">Aggregate:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtCAggregate" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">KAM Flag:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtKAM" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>

                                    <td style="width: 15%" class="">Upgradation Campaign:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtUpgCamp" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">In RMC:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtCAMCChk" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">RMC End Date
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAMCDate" CssClass="TextBoxForString" runat="server" Width="96%" Text=""
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">RMC Type:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtAMCType" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">Chassis Under Observation:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtUndObserv" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Observation from:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtObservEffFrom" CssClass="TextBoxForString" runat="server" Width="96%" Text=""
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Observation To:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtObservEffTo" CssClass="TextBoxForString" runat="server" Width="96%" Text=""
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">Attached Float Part:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtFloatPart" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Theft Chassis:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtIsTheft" CssClass="TextBoxForString" runat="server" Width="96%" Text="N"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>

                                    <td style="width: 15%" class=""></td>
                                    <td style="width: 18%"></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">Speedometer Changed:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtSpdMtrChg" CssClass="TextBoxForString" runat="server" Width="96%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Kms Bfr Changed Last Speedometer:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtBfr_Last_SpdMtrChange_Kms" CssClass="TextBoxForString" runat="server" Width="96%" Text="0"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Last Jobcard Kms
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtLstJbKms" CssClass="TextBoxForString" runat="server" Width="96%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">Hrs Meter Changed:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtHrsMtrChg" CssClass="TextBoxForString" runat="server" Width="96%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Hrs Bfr Changed Last Hrs meter:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtBfr_Last_HrsMtrChange_Hrs" CssClass="TextBoxForString" runat="server" Width="96%" Text="0"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">Last Jobcard Hrs
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtLstJbHrs" CssClass="TextBoxForString" runat="server" Width="96%" Text="0"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%">
                                        <asp:Label ID="Label20" runat="server" Text="Chassis Last Kms.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <%--<asp:Label ID="lblLastKms" runat="server" ></asp:Label>--%>
                                        <asp:TextBox ID="txtLastKms" runat="server" CssClass="TextBoxForString" MaxLength="9"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%" BackColor="White" BorderStyle="None"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%">
                                        <asp:Label ID="Label21" runat="server" Text="Chassis Last Hrs.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <%--<asp:Label ID="lblLastHrs" runat="server" Text="0"></asp:Label>--%>
                                        <asp:TextBox ID="txtLastHrs" runat="server" CssClass="TextBoxForString" Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);" BackColor="White" BorderStyle="None"></asp:TextBox>
                                    </td>
                                    <td class="" style="width: 15%"></td>
                                    <td style="width: 18%"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PRefDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPERefDetails" runat="server" TargetControlID="CntRefDetails"
                            ExpandControlID="TtlRefDetails" CollapseControlID="TtlRefDetails" Collapsed="true"
                            ImageControlID="ImgTtlRefDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Reference and Approximate Amount Details" ExpandedText="Reference and Approximate Amount Details"
                            TextLabelID="lblTtlRefDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlRefDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlRefDetails" runat="server" Text="Reference and Approximate Amount Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlRefDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="123%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntRefDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="tblRefDetails" runat="server" class="" width="100%">
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="lblManDocNo" runat="server" Text="Vehicle In No.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtVehicleInNo" Text="" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        <b class="Mandatory">*</b>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label9" runat="server" Text="Approximate Part Amount.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtApPartAmt" Text="" runat="server" CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'6');" MaxLength="7"></asp:TextBox>
                                        <b class="Mandatory">*</b>
                                    </td>

                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label13" runat="server" Text="Job Opening Time:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="dtpJobOpeningTm" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label3" runat="server" Text="Vehicle In Time:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <%--<uc3:CurrentDate ID="dtpVehInTime" runat="server" Mandatory="true" bTimeVisible="true" bCheckforCurrentDate="true" />                                    --%>
                                        <asp:TextBox ID="dtpVehInTime" Text="" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        <b class="Mandatory">*</b>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label10" runat="server" Text="Approximate Labor Amount.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtApLabAmt" Text="" runat="server" CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'6');" MaxLength="7"></asp:TextBox>
                                        <b class="Mandatory">*</b>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label14" runat="server" Text="Job Committed Time:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="dtpJobCommited" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label5" runat="server" Text="Estimate No.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="DrpEstimate" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="DrpEstimate_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label11" runat="server" Text="Approximate Lubs Amount..:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtApLubAmt" Text="" runat="server" CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'6');" MaxLength="7"></asp:TextBox>
                                        <b class="Mandatory">*</b>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label17" runat="server" Text="Delay Reason.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="DrpDelayReason" runat="server" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label16" runat="server" Text="Estimated Time:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:Label ID="lblEstmtdTm" runat="server" Text="1"></asp:Label>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label12" runat="server" Text="Approximate Misc. Amount.:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtApMiscAmt" Text="" runat="server" CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'6');" MaxLength="7"></asp:TextBox>
                                        <b class="Mandatory">*</b>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label15" runat="server" Text="Vehicle Out Time:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="DtpVehicleOut" runat="server" Mandatory="true" bTimeVisible="true" bCheckforCurrentDate="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label18" runat="server" Text="Recommendations:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtRecomm" Text="" TextMode="MultiLine" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <asp:Label ID="Label19" runat="server" Text="Narration:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtNarration" Text="" TextMode="MultiLine" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="">
                                        <%--<asp:Label ID="Label6" runat="server" Text="Estimate Date:"></asp:Label>--%>
                                         
                                    </td>
                                    <td style="width: 18%">
                                        <%--<uc3:CurrentDate ID="dtpEstTm" runat="server" Mandatory="true" bTimeVisible="false" bCheckforCurrentDate="true" />--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TblControl">
                <td>
                    <asp:Panel ID="PVehInfoDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEVehInfoDetails" runat="server" TargetControlID="CntVehInfoDetails"
                            ExpandControlID="TtlVehInfoDetails" CollapseControlID="TtlVehInfoDetails" Collapsed="true"
                            ImageControlID="ImgTtlVehInfoDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Vehicle Information Details" ExpandedText="Vehicle Information Details"
                            TextLabelID="lblTtlVehInfoDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlVehInfoDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlVehInfoDetails" runat="server" Text="Vehicle Information Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlVehInfoDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="123%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntVehInfoDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                            <table id="tblVehInfoDetails" width="100%" runat="server" class="">
                                <tr>
                                    <td class="" style="width: 15%">Suppervisor Name:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="DrpSupervisorName" runat="server" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="" style="width: 15%">Failure Date:
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="txtFailureDt" runat="server" Mandatory="true" bTimeVisible="false" bCheckforCurrentDate="true" />
                                    </td>
                                    <td class="" style="width: 15%;">Jobcard Amount.:
                                    </td>
                                    <td style="width: 18%;">
                                        <asp:Label ID="lblTotJobAmt" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" style="width: 15%">Invoice No.:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:Label ID="lblJobInvoice" runat="server"></asp:Label>
                                    </td>
                                    <td class="" style="width: 15%">Claim No.:              
                                    </td>
                                    <td style="width: 18%">
                                        <asp:Label ID="lblJobClaim" runat="server"></asp:Label>
                                    </td>
                                    <td class="" style="width: 15%">
                                        <asp:LinkButton ID="lnkGatePass" runat="server" Style="height: 12px" Text="Gatepass" ToolTip="Gatepass" Width="80%"> </asp:LinkButton>
                                    </td>
                                    <td style="width: 18%"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TblControl">
                <td>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPCompl" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEComplaints" runat="server" TargetControlID="CntComplaints"
                                    ExpandControlID="TtlComplaints" CollapseControlID="TtlComplaints" Collapsed="True"
                                    ImageControlID="ImgTtlComplaints" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Customer Complaints/ Incidence of Failure"
                                    ExpandedText="Customer Complaints/ Incidence of Failure" TextLabelID="lblTtlComplaints">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlComplaints" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlComplaints" runat="server" Text="Customer Complaints/ Incidence of Failure" ForeColor="White"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="" width="8%">Count:
                                            </td>
                                            <td class="" width="8%">
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
                                                        Width="90%">
                                                        <%--onblur="return CheckComplaintSelected(event,this);"--%>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewComplaintDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString" MaxLength="500"
                                                        Width="90%" onblur="return CheckComplaintAlreadyUsedInGrid(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Delete/Cancel" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" Text="Delete" runat="server" />
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

                    <%--<asp:UpdatePanel UpdateMode="Conditional" ID="UPPart" runat="server" ChildrenAsTriggers="true">                      
                      <ContentTemplate>--%>
                    <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                            ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="True"
                            ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Part Details" ExpandedText="Part Details"
                            TextLabelID="lblTtlPartDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlPartDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <%-- <td class="" width="8%">Count:
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
                        <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double" >
                            <%--Style="display: none;"--%>
                            <div class="scrolling-table-container WordWrap">
                            <%--Div contains the new header of the GridView--%>
                            <%--  <div id="HeaderDiv">
                            </div>--%>
                            <%--Wrapper Div which will scroll the GridView--%>
                         <%--   <div id="DataDiv" style="overflow: auto; border: 1px solid olive; width: 160%; height: 300px;"
                                onscroll="Onscrollfnction();">--%>
                            <%--<div style="overflow: hidden;" id="DivHeaderRow_part">
                            </div>
                            <div style="overflow: scroll; background-color: #D4D4D4;" onscroll="OnScrollDiv(this,DivHeaderRow_part)" id="DivMainContent_part">--%>
                                <asp:GridView ID="PartDetailsGrid" runat="server" AllowPaging="false" Width="160%"
                                    AutoGenerateColumns="False" EditRowStyle-BorderColor="Black"
                                    GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" OnRowDataBound="PartDetailsGrid_RowDataBound"
                                    SkinID="NormalGrid" ShowFooter="false"
                                    AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    EditRowStyle-Wrap="true"
                                    HeaderStyle-Wrap="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign"></asp:Label>
                                                <%--Text="<%# Container.DataItemIndex + 1  %>"--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartID" runat="server" Text='<%# Eval("PartLabourID") %>' Width="1%"></asp:TextBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAddPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" Text="SelectPart" ToolTip="Click Here To Select Part"
                                                    Width="70%"></asp:Label>--%>
                                                <asp:LinkButton ID="lnkSelectPart" runat="server" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="GridTextBoxForString"
                                                    Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartName" runat="server" CssClass="GridTextBoxForString" Text=""
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref No" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIPO_no" runat="server" CssClass="GridTextBoxForString" Width="96%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mechanic" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpPMechanic" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Req Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReqQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"
                                                    onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIssueQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"
                                                    onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ret Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRetQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                                <%--onkeypress="return CheckForTextBoxValue(event,this,'5');"--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Use Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUseQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Paid Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBillQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="War Tag" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtwar_tag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Foc Tag" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpfoctag" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                    <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FOC Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFOCQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Foc Reason ID" ItemStyle-Width="2.5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpReasonID" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FSC Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFSCQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PDI Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPDIQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RMC Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAMCQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Campaign Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCampaignQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="transit Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txttransitQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EnRouteTech Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEnRouteTechQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EnrouteNonTech Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEnrouteNonTechQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SpWar Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpWarQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GoodWl Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGoodWlQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Warranty Qty" ItemStyle-Width="1.3%" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWarrQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pre-PDI Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrePDIQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"
                                                    onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aggregate Qty" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAggregateQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"
                                                    onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Extra Qty1" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtExtraQty1" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"></asp:TextBox>
                                                <%--onblur="return CalculateLineTotalForPart(event,this);"--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Extra Qty2" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtExtraQty2" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"></asp:TextBox>
                                                <%--onblur="return CalculateLineTotalForPart(event,this);"--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="9"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Make" ItemStyle-Width="2.5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpMake" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lubricant Location" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpLubLoc" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lubricant Capacity" ItemStyle-Width="1.5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpLubCap" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Jobcode" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpJobCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NDP Rate" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNDPRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Type Tag" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartTypeTag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock" ItemStyle-Width="1.3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartStock" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <asp:TextBox ID="txtPreviousIssueQty" runat="server" CssClass="HideControl" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <asp:TextBox ID="txtPreviousRetQty" runat="server" CssClass="HideControl" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DtlID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDtlPartID" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <asp:TextBox ID="txtPEstDtlID" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPartGroupCode" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancel" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkForDelete" runat="server" Text="Delete" onClick="return SelectDeleteCheckboxForPart(this);" />
                                                <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" onclick="ClearRowValueForPartWarranty(event,this); "
                                                    onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"
                                                    Text="Cancel"></asp:Label>
                                                <%--<asp:LinkButton ID="lnkNew" runat="server" OnClientClick="return CheckRowValue(event,this);">New</asp:LinkButton>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RMC Rate" ItemStyle-Width="5%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAMCRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--<div id="DivFooterRow" style="overflow:hidden">--%>
                        </asp:Panel>
                    </asp:Panel>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <%--<asp:UpdateProgress ID="UPRPart" runat="server" AssociatedUpdatePanelID="UPPart">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
                    <%--  <asp:UpdatePanel UpdateMode="Conditional" ID="UPLabor" runat="server" ChildrenAsTriggers="true">                  
                        <ContentTemplate>--%>
                    <asp:Panel ID="PLabourDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPELabourDetails" runat="server" TargetControlID="CntLabourDetails"
                            ExpandControlID="TtlLabourDetails" CollapseControlID="TtlLabourDetails" Collapsed="true"
                            ImageControlID="ImgTtlLabourDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Labour Details" ExpandedText="Labour Details"
                            TextLabelID="lblTtlLabourDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlLabourDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlLabourDetails" runat="server" Text="Labour Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <%-- <td class="ContaintTableHeader" width="8%">Count:
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
                        <asp:Panel ID="CntLabourDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double" 
                            >
                             <div class="scrolling-table-container WordWrap">
                          <%--  <div style="overflow: hidden;" id="DivHeaderRow_labour">
                            </div>
                            <div style="overflow: scroll; background-color: #D4D4D4;" onscroll="OnScrollDiv(this,DivHeaderRow_labour)" id="DivMainContent_labour">--%>
                                <asp:GridView ID="LabourDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                    GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true" OnRowDataBound="LabourDetailsGrid_RowDataBound"
                                    SkinID="NormalGrid" Width="130%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourID" runat="server" Text='<%# Eval("PartLabourID") %>' Width="1"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Code" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <%-- <asp:Label ID="lblNewLabour" runat="server" ForeColor="#49A3D3" Text="SelectLabour"
                                                    ToolTip="Click Here To Select Labour Code" onmouseover="SetCancelStyleonMouseOver(this);"
                                                    onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>--%>
                                                <asp:LinkButton ID="lnkSelectLabour" runat="server" OnClick="lnkSelectLabour_Click">Select Labour</asp:LinkButton>
                                                <asp:TextBox ID="txtLabourCode"
                                                    runat="server" Text="" Width="96%" CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Description" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourDesc" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other Description" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpLbrDescription" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="90%" onblur="return CheckLbrDescSelected(event,this);">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtLbrDescription" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                    Width="90%" MaxLength="50" onblur="return CheckLbrDescAlreadyUsedInGrid(event,this);"></asp:TextBox>
                                                <%--<asp:TextBox ID="txtLbrDescription" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Man Hrs" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtManHrs" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(event,this);"></asp:TextBox>                                                
                                                <asp:TextBox ID="txtPaidRate" runat="server"
                                                     Width="96%" MaxLength="9"  CssClass="HideControl"></asp:TextBox>                                                
                                                <asp:TextBox ID="txtWRate" runat="server"
                                                     Width="96%" MaxLength="9"  CssClass="HideControl"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server"
                                                    CssClass="GridTextBoxForAmount" Width="96%" MaxLength="9"></asp:TextBox>
                                                <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                <asp:TextBox ID="txtOldAmount" runat="server" CssClass="HideControl" Width="4%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company /Dealer" ItemStyle-Width="2.2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabCD" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Warrantable   /Non-Warrantable" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabWarr" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"
                                                    Visible="false" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <asp:DropDownList ID="drpLabWarr" runat="server" EnableViewState="true" CssClass="GridComboBoxFixedSize"
                                                  onblur="return calculateLabourTotal(event,this);"   Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FOC" ItemStyle-Width="1.5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpFOC" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                    <asp:ListItem Text="N" Value="N" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FOC Reason" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpFOCReason" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LbrInfo" ItemStyle-Width="3%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabMnGr" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtEstDtlID" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtRepjob" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLGroupCode" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mech Name" ItemStyle-Width="2.5%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpMechName" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sublet Amt " ItemStyle-Width="2.5%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSubletAmt" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sublet Name" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpSubletSupp" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Out Mech  Name" ItemStyle-Width="3%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpOutMechName" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sublet Description" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSubletDescription" runat="server" Text="" Width="98%" MaxLength="30" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Print" ItemStyle-Width="2%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLPrint" runat="server">Print</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WO No" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWONo" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <asp:TextBox ID="txtMaterialReceipt" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Jobcode" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpJobCode" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancel" HeaderStyle-Width="1%" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxForLabour(this);"
                                                    Text="Delete" />
                                                <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForLabourWarranty(event,this); "
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <%--                    <asp:UpdateProgress ID="UPRLabor" runat="server" AssociatedUpdatePanelID="UPLabor">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>

                    <%-- <asp:UpdatePanel UpdateMode="Conditional" ID="UPGrpTax" runat="server" ChildrenAsTriggers="true">  
                        <ContentTemplate>--%>
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
                                    EditRowStyle-BorderColor="Black" AutoGenerateColumns="false" PageSize="5" CssClass="table table-condensed table-bordered"
                                    OnRowCommand="GrdPartGroup_RowCommand">
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
                                        <asp:TemplateField HeaderText="Discount %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                    MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateJbPartGranTotal();"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
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
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <%--  <asp:UpdateProgress ID="UPRGrpTax" runat="server" AssociatedUpdatePanelID="UPGrpTax">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
                    <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                            ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                            ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Jobcard Tax Details" ExpandedText="Jobcard Tax Details"
                            TextLabelID="lblTtlTaxDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlTaxDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Jobcard Tax Details" Width="96%"
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
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered"
                                OnRowCommand="GrdPartGroup_RowCommand">
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
                                    <asp:TemplateField HeaderText="Taxable Total" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDocTotal" runat="server" Text='<%# Eval("net_tr_amt","{0:#0.00}") %>' Width="90%"
                                                CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDocDisc" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
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
                                    <asp:TemplateField HeaderText="Freight %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                                Width="90%" MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateJbPartGranTotal();"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Freight Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this)" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Other Charges %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                                Text='<%# Eval("other_per","{0:#0.00}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateJbPartGranTotal();"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Other Charges" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("other_money","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Jobcard_tot","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
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
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEJobDetails" runat="server" TargetControlID="CntJobDetails"
                                    ExpandControlID="TtlJobDetails" CollapseControlID="TtlJobDetails" Collapsed="True"
                                    ImageControlID="ImgTtlJobDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Jobcode Details" ExpandedText="JobCode Details"
                                    TextLabelID="lblTtlJobDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlJobDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlJobDetails" runat="server" Text="Job Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                                    onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">Count:
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
                                            <asp:TemplateField HeaderText="Job ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtJobID" runat="server" Text=""></asp:TextBox>
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
                                                        onChange="return CheckCulpritCodeValidation(this);" Width="100%">
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
                                            <asp:TemplateField HeaderText="Delete/ Cancel" ItemStyle-Width="3%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxForJob(this);"
                                                        Text="Delete" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" onclick="return ClearRowValueForJob(this); "
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"
                                                        Text="Cancel"></asp:Label>
                                                    <%--<asp:LinkButton ID="lnkNew" runat="server" OnClientClick="return CheckRowValue(event,this);">New</asp:LinkButton>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PCRHDRID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPCRHDRID" runat="server" Text=""></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PCR" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectJobDtl" runat="server" OnClick="lnkSelectJobDtl_Click">Job PCR</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="JobCodeDtlSavedOrNot" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtJobCodeDtlSaved" runat="server" Text=""></asp:TextBox>
                                                    <asp:TextBox ID="txtWarrJobCode" runat="server" Text=""></asp:TextBox>
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

                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPFreeService" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PFreeServices" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEFreeServices" runat="server" TargetControlID="CntFreeServices"
                                    ExpandControlID="TtlFreeServices" CollapseControlID="TtlFreeServices" Collapsed="true"
                                    ImageControlID="ImgTtlFreeServices" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Dealer’s FreeServices"
                                    ExpandedText="Dealer’s FreeServices" TextLabelID="lblTtlFreeServices">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlFreeServices" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlFreeServices" runat="server" Text="Free Service Coupon Details"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">Count:
                                            </td>
                                            <td class="ContaintTableHeader" width="8%">
                                                <asp:Label ID="lblFreeServicesRecCnt" runat="server" Text="0"></asp:Label>
                                            </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlFreeServices" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntFreeServices" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    ScrollBars="None">
                                    <asp:GridView ID="FreeServicesGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrvo" runat="server" Text='<%# Eval("ID") %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkService" runat="server" OnClick="return ChkFreeServiceClick(this);" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrvName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Serv_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Coupon No">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSrvCoupon" runat="server" Text='<%# Eval("CouponNo") %>' Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Jobcard No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrvJobDtl" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("JobDtl") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Jobcard ID" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblSrvJobID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("JobID") %>'></asp:Label>                                                 --%>
                                                    <asp:TextBox ID="txtSrvJobID" runat="server" Width="98%" CssClass="GridTextBoxForString" Text='<%# Eval("JobID") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Labor Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSrvLbrAmt" runat="server" Width="98%" CssClass="GridTextBoxForAmount" Text='<%# Eval("LSPSD","{0:#0.00}") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Part Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSrvPartAmt" runat="server" Width="98%" CssClass="GridTextBoxForAmount" Text='<%# Eval("MSPSD","{0:#0.00}") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last User Service ID" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLstSelSrvID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstSelectSrv") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Effective From">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtefffromdate" runat="server" Width="98%" CssClass="GridTextBoxForAmount" Text='<%# Eval("eff_from_date") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Effective To">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txteffTodate" runat="server" Width="98%" CssClass="GridTextBoxForAmount" Text='<%# Eval("eff_To_date") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ValidDate" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblValidDt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ValidDt") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPInv" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PInvestigations" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEInvestigations" runat="server" TargetControlID="CntInvestigations"
                                    ExpandControlID="TtlInvestigations" CollapseControlID="TtlInvestigations" Collapsed="true"
                                    ImageControlID="ImgTtlInvestigations" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Dealer’s Investigations /Probable Cause"
                                    ExpandedText="Dealer’s Investigations /Probable Cause" TextLabelID="lblTtlInvestigations">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlInvestigations" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlInvestigations" runat="server" Text="Dealer’s Investigations /Probable Cause"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">Count:
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
                                                        Width="90%">
                                                        <%--onblur="return CheckInvestigationSelected(event,this);"--%>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewInvestigationDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                        Width="90%" MaxLength="500" onblur=" return CheckInvestigationAlreadyUsedInGrid(event,this);"></asp:TextBox>
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

                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPAction" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PActions" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEActions" runat="server" TargetControlID="CntActions"
                                    ExpandControlID="TtlActions" CollapseControlID="TtlActions" Collapsed="true"
                                    ImageControlID="ImgTtlActions" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Dealer’s Actions"
                                    ExpandedText="Dealer’s Actions" TextLabelID="lblTtlActions">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlActions" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlActions" runat="server" Text="Dealer’s Actions /Probable Cause"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--  <td class="ContaintTableHeader" width="8%">Count:
                                            </td>
                                            <td class="ContaintTableHeader" width="8%">
                                                <asp:Label ID="lblActionsRecCnt" runat="server" Text="0"></asp:Label>
                                            </td>--%>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlActions" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntActions" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="ActionsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpAction" runat="server" CssClass="GridComboBoxFixedSize"
                                                        Width="90%">
                                                        <%-- onblur="return CheckActionSelected(event,this);"--%>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewActionDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                        Width="90%" onblur=" return CheckActionAlreadyUsedInGrid(event,this);" MaxLength="150"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New/Delete/Cancel" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" />
                                                    <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForAction(event,this); "
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
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPAction">
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
                                    <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                        <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
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
                                            GridLines="Horizontal" OnRowDataBound="FileAttchGrid_RowDataBound" HeaderStyle-Wrap="true"
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
                                <%--sujata 02062011 Remove commented code to upload a file from RSM User for Domestic Goodwill Claim Request--%>
                                <%--sujata 12012011 --%>
                                <tr id="trNewAttachment" runat="server">
                                    <td class="tdLabel" style="width: 50%" align="center">User File Description
                                    </td>
                                    <td class="tdLabel" style="width: 50%" align="center">File Name
                                    </td>
                                </tr>
                                <tr id="trNewAttachment1" runat="server">
                                    <td colspan="2" class="tdLabel">
                                        <div id="upload1">
                                            <input id="Text1" type="text" name="Text1" class="TextBoxForString" placeholder="User File Description" style="width: 50%" />
                                            <input id="AttachFile" type="file" runat="server" style="width: 45%" class="Cntrl1"
                                                onblur="return addFileUploadBox(this);" />
                                        </div>
                                    </td>
                                </tr>
                                <%--sujata 12012011--%>
                                <%--sujata 02062011 Remove commented code to upload a file from RSM User for Domestic Goodwill Claim Request--%>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PTotal" runat="server">
                        <table id="TblTotal" runat="server" border="1" class="ContainTable">
                            <tr>
                                <td style="text-align: right">
                                    <b>Parts Amount: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtPartAmount" runat="server" CssClass="TextForAmount" Text="0"
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
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b>Lubricant Amount: </b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtLubricantAmount" runat="server" CssClass="TextForAmount" Text="0"
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
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <b>Jobcard Amount:</b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtJbTotAmt" runat="server" CssClass="TextForAmount" Text="0" Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<asp:Panel ID="PLabourTimeDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double" class="">
                                <cc1:CollapsiblePanelExtender ID="CPELabourTimeDetails" runat="server" TargetControlID="CntLabourTimeDetails"
                                    ExpandControlID="TtlLabourTimeDetails" CollapseControlID="TtlLabourTimeDetails" Collapsed="true"
                                    ImageControlID="ImgTtlLabourTimeDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Labour Timing Details" ExpandedText="Labour Timing Details"
                                    TextLabelID="lblTtlLabourTimeDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlLabourTimeDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlLabourTimeDetails" runat="server" Text="Labour Timing Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                           <%-- <td class="ContaintTableHeader" width="8%">Count:
                                            </td>
                                            <td class="ContaintTableHeader" width="8%">
                                                <asp:Label ID="lblLabourTimeRecCnt" runat="server" Text="0"></asp:Label>
                                            </td>--%>
                    <%--<td width="1%">
                                                <asp:Image ID="ImgTtlLabourTimeDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntLabourTimeDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                    <div class="scrolling-table-container WordWrap">
                                    <asp:GridView ID="LabourTimeDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                        AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                        GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                        SkinID="NormalGrid" Width="130%">
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
                                            <asp:TemplateField HeaderText="Labour Code" ItemStyle-Width="3%">
                                                <ItemTemplate>                                                                                                       
                                                    <asp:TextBox ID="txtLabourCode"
                                                        runat="server" Text="" Width="96%" CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Labour Description" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLabourDesc" runat="server" Text="" Width="95%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company /Dealer" ItemStyle-Width="2.2%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLabCD" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start/ Resume Time " ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStartTime" runat="server" Text="" Width="95%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>                
                                            <asp:TemplateField HeaderText="Pause" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnAction" runat="server" CssClass="btn btn-search btn-sm" Text=""  Width="125px"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>                
                                            <asp:TemplateField HeaderText="Pause Time" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPauseTime" runat="server" Text="" Width="95%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>                
                                            <asp:TemplateField HeaderText="Pause Reason" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="DrpPauseReason" runat="server" Text="" Width="95%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>                
                                            <asp:TemplateField HeaderText="End Time" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEndTime" runat="server" Text="" Width="95%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>        
                                            <asp:TemplateField HeaderText="Cancel" HeaderStyle-Width="1%" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" Text="Delete" /> 
                                                    <asp:TextBox ID="txtSrNo" runat="server" Text="" Width="1"></asp:TextBox>                                                   
                                                </ItemTemplate>
                                            </asp:TemplateField>        
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                        </div>
                                </asp:Panel>
                            </asp:Panel>--%>
                </td>
            </tr>
            <%--<tr id="TblControl">
                <td>
                    <asp:Panel ID="CntChassisStatus" runat="server">
                        
                    </asp:Panel>
                </td>
            </tr>--%>
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerCode" runat="server" Width="1%" Text="" CssClass="HideControl"></asp:TextBox>
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnJbCdConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnWorkShopStoreLogin" runat="server" Value="N" />
                    <asp:TextBox ID="txtCustEdit" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtCustID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtNewRecountCount" runat="server" Text="1" Width="1%" CssClass="HideControl"></asp:TextBox>
                    <asp:TextBox ID="txtChkfun" runat="server" CssClass="HideControl" Text="false"></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnSelectedLabourID" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnSelectedPartIDN" runat="server" Value="N" />
                    <asp:HiddenField ID="txtModelGroupID" runat="server" Value="N" />
                    <asp:HiddenField ID="txtModelID" runat="server" Value="N" />
                    <asp:TextBox ID="txtWarrantyTag" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                    <asp:TextBox ID="txtAggregate" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                    <asp:TextBox ID="txtPrevAggregate" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                    <asp:TextBox ID="txtAMCChk" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                    <asp:HiddenField ID="hdnRepeatJob" runat="server" Value="N" />
                    <asp:TextBox ID="txtChassisID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtModCatIDBasic" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txthdnBfr_Last_SpdMtrChange_Kms" CssClass="HideControl" runat="server" Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                    <asp:TextBox ID="txthdnBfr_Last_HrsMtrChange_Hrs" CssClass="HideControl" runat="server" Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                    <%--<asp:HiddenField ID="hdnAddnWarrTag" runat="server" Value="N" />--%>
                    <%--<asp:HiddenField ID="hdnAMCChk" runat="server" Value="N" />--%>
                    <%--<asp:HiddenField ID="hdnAMCDate" runat="server" Value="" />--%>

                    <asp:HiddenField ID="hdnAMCType" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnKAM" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnUpgCamp" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnUndObserv" runat="server" Value="N" />
                    <%--<asp:HiddenField ID="hdnObservEffFrom" runat="server" Value="" />--%>
                    <%--<asp:HiddenField ID="hdnObservEffTo" runat="server" Value="" />--%>
                    <asp:HiddenField ID="IsTheft" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnFloatPart" runat="server" Value="N" />
                    <%--<asp:HiddenField ID="hdnBfr_Last_SpdMtrChange_Kms" runat="server" Value="0" />--%>
                    <%--<asp:HiddenField ID="hdnBfr_Last_HrsMtrChange_Hrs" runat="server" Value="0" />--%>
                    <%--<asp:HiddenField ID="hdnCumulativeKms" runat="server" Value="0" />--%>
                    <%--<asp:HiddenField ID="hdnSpdMtrChg" runat="server" Value="N" />--%>
                    <%--<asp:HiddenField ID="hdnHrsMtrChg" runat="server" Value="N" />--%>
                    <asp:Label ID="lblComplaintsRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblPartRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblLabourRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblJobRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblActionsRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblInvestigationsRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblFreeServicesRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:TextBox ID="txtHVReqID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtGDReqID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnNeedHVReq" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnNeedGWReq" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnChassisID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnVehInID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnJobInvID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnSaleInvID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnGPID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnSrvVANID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="" />
                    <asp:TextBox ID="txtCRMID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnHVReqApprNo" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnGWReqApprNo" runat="server" Value="N" />
                    <asp:Label ID="lblFileAttachRecCnt" CssClass="HideControl" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hdnPendingVORPORec" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnISDocGST" runat="server" />
                    <asp:TextBox ID="txtReportChassisID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnShwReqLst" runat="server" Value="" />

                    <asp:HiddenField ID="hdnWarrEndKms" runat="server" Value="" />
                    <asp:HiddenField ID="hdnWarrEndHrs" runat="server" Value="" />

                    <asp:HiddenField ID="hdnExtWarrStartKms" runat="server" Value="" />
                    <asp:HiddenField ID="hdnExtWarrEndKms" runat="server" Value="" />

                    <asp:HiddenField ID="hdnExtWarrStartHrs" runat="server" Value="" />
                    <asp:HiddenField ID="hdnExtWarrEndHrs" runat="server" Value="" />

                    <asp:HiddenField ID="hdnAMCStKms" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnAMCEndKms" runat="server" Value="0" />

                    <asp:HiddenField ID="hdnAMCStHrs" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnAMCEndHrs" runat="server" Value="0" />

                    <asp:HiddenField ID="hdnWarrChkType" runat="server" />

                    <asp:HiddenField ID="hdnWarrEndDt" runat="server" />
                    <asp:HiddenField ID="hdnExtWarrStDt" runat="server" />
                    <asp:HiddenField ID="hdnExtWarrEndDt" runat="server" />
                    <asp:HiddenField ID="hdnAMCStDt" runat="server" />

                    <asp:HiddenField ID="hdnAddWarrStDt" runat="server" />
                    <asp:HiddenField ID="hdnAddWarrEndDt" runat="server" />
                    <asp:HiddenField ID="hdnPDIDone" runat="server" />
                    <asp:HiddenField ID="hdnRounOff" runat="server" Value="" />

                    <asp:HiddenField ID="hdnLWarrRate" runat="server" />
                    <asp:HiddenField ID="hdnLPaidRate" runat="server"  />
                    <asp:HiddenField ID="hdnHVReqStatus" runat="server"  />
                    <asp:HiddenField ID="hdnGDReqStatus" runat="server"  />
                    

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
