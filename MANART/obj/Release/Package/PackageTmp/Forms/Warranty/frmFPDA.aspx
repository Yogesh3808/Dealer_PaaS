<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmFPDA.aspx.cs" Inherits="MANART.Forms.Warranty.frmFPDA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <%--<link href="../../Content/DateStyle.css" rel="stylesheet" />--%>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/JSFPDAWarrantyClaim.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
            var txtLRDate = document.getElementById("ContentPlaceHolder1_txtLRDate_txtDocDate");

            var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
            var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");


            var hdnMinFPDADate = document.getElementById("ContentPlaceHolder1_hdnMinFPDADate");
            var hdnMaxFPDADate = document.getElementById("ContentPlaceHolder1_hdnMaxFPDADate");

            var hdnMinCreditDate = document.getElementById("ContentPlaceHolder1_hdnMinCreditDate");
            var hdnMaxCreditDate = document.getElementById("ContentPlaceHolder1_hdnMaxCreditDate");

            var objDate = new Date();

            $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (hdnMinFPDADate.value == '') ? new Date(objDate.getFullYear(), 1 - 1, 1) : hdnMinFPDADate.value, maxDate: (hdnMaxFPDADate.value == '') ? '0d' : hdnMaxFPDADate.value
            });

            //$('#ContentPlaceHolder1_txtLRDate_txtDocDate').datepick({
            //    dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
            //});
            $('#ContentPlaceHolder1_txtLRDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: new Date(objDate.getFullYear(),- 12, 1), maxDate: '0d'
            });

            var objDate = new Date();
            var objYear = objDate.getFullYear();
            $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', defaultDate: new Date(objYear, 1 - 1, 1), selectDefaultDate: (txtFromDate.value == '') ? true : false, minDate: hdnMinCreditDate.value, maxDate: hdnMaxCreditDate.value
            });

            $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', defaultDate: '0d', selectDefaultDate: (txtToDate.value == '') ? true : false, minDate: hdnMinCreditDate.value, maxDate: hdnMaxCreditDate.value
            });

            function customRange(dates) {
                if (this.id == 'ContentPlaceHolder1_txtDocDate_txtDocDate') {
                    $('#ContentPlaceHolder1_txtLRDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                }

                if (this.id == 'ContentPlaceHolder1_txtFromDate_txtDocDate') {
                    $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                }

                if (this.id == 'ContentPlaceHolder1_txtToDate_txtDocDate') {
                    $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                }
            }
        });

        //To Show FPDAWarranty Claim
        function ShowFPDAWarrantyClaim(sDealerId) {//, FromDate, ToDate

            //alert("Hi");
            //debugger;
            var FPDADetails = null;
            var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
            var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
            //FPDADetails = window.showModalDialog("/DCS/Forms/Warranty/frmFPDASelection.aspx", "List", "dialogWidth:700px;dialogHeight:500px;status:no;help:no");
            FPDADetails = window.showModalDialog("../Warranty/frmFPDASelection.aspx?DealerID=" + sDealerId + "&FromDate=" + txtFromDate.value + "&ToDate=" + txtToDate.value, "List", "dialogWidth:700px;dialogHeight:500px;status:no;help:no");
        }

        function SelectAllFPDA(id) {
            var frm = document.forms[0];
            if (document.getElementById(id).checked == false) {
                if (confirm("Are you sure you want to deselect all the record?") == true) {
                }
                else {
                    return false;
                }
            }
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
        }
        function SetCurrentAndPastDate(obj, Msg) {

            var objDateValue = "";
            var ObjDate = obj;
            var x = new Date();
            var y = x.getYear();
            var m = x.getMonth() + 1; // added +1 because javascript counts month from 0
            var d = x.getDate();
            var dtCur = d + '/' + m + '/' + y;
            var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)

            objDateValue = ObjDate.value;
            var sTmpValue = objDateValue;
            var day = dGetValue(sTmpValue.split("/")[0]);
            var month = dGetValue(sTmpValue.split("/")[1]) - 1;
            var year = dGetValue(sTmpValue.split("/")[2]);
            var sTmpDate = new Date(year, month, day);
            var TmpDay = 0;

            if (objDateValue == '') {
                return false;
            }
            if (dtCurDate < sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }

        function BoxDtlsValidation(Obj) {
            //            var ObjGrid = null;
            var TotHedearBox = null;
            //            ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");            
            TotHedearBox = window.document.getElementById("ContentPlaceHolder1_txtNoOfCases");
            var TotDetailsBox = 0;            
            var objtxtControl;
            var iCountDel = 0;
            var ObjControl;
            var bCheckValidation = true;
            var sMessage = "";
            var ObjRow = Obj.parentNode.parentNode.childNodes;
            debugger;
            TotDetailsBox = ObjRow[8].childNodes[1].value;

            if (TotHedearBox.value == "" || TotHedearBox.value == "0") {
                alert("Please Enter The No Of Boxes In The Header.");
                Obj.value = 0;
                //TotHedearBox.focus();
                return false;
            }

            if (dGetValue(TotHedearBox.value) < dGetValue(TotDetailsBox)) {
                alert("Box No Can Not Be Greator Than No Of Boxes In The Header.");
                Obj.value = 0;
                Obj.focus();
                return false;
            }
            return true;
        }

        function BoxHdrValidation(Obj) {
            var ObjGrid = null;
            var TotHedearBox = null;
            ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
            TotHedearBox = window.document.getElementById("ContentPlaceHolder1_txtNoOfCases");
            var TotDetailsBox = 0;
            if (ObjGrid == null) {
                return false;
            }
            var objtxtControl;
            var iCountDel = 0;
            var ObjControl;
            var bCheckValidation = true;
            var sMessage = "";
            for (var i = 1; i <= ObjGrid.rows.length - 1; i++) {
                //      Check Expected Exp. Head
                objtxtControl = ObjGrid.rows[i].cells[7].childNodes[1];
                //TotDetailsBox = dGetValue(TotDetailsBox) + dGetValue(objtxtControl.value)
                TotDetailsBox = dGetValue(objtxtControl.value)
                if (dGetValue(TotHedearBox.value) < TotDetailsBox) {
                    alert("In Header No Of Boxes Can Not Be Less Than Box No Present In Detail Section.");
                    TotHedearBox.focus();
                    return false;
                }
            }
            return true;
        }

        function AcceptValidation(Obj) {
            var ObjGrid = null;
            ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
            var TotUserCheck = 0;
            var ChassisCnt = 0;
            var tmpChassisNo = "";
            var tmpResetValue = 0;
            var tmpCountValue = 0;
            if (ObjGrid == null) return;
            var objtxtControl;
            //debugger;
            if (Obj != null)
                for (var MLoopCnt = 1; MLoopCnt <= ObjGrid.rows.length - 1; MLoopCnt++) {
                    ChassisCnt = 0;
                    TotUserCheck = 0;
                    tmpChassisNo = ObjGrid.rows[MLoopCnt].cells[2].childNodes[1].innerHTML;
                    for (var SLoopCnt = 1; SLoopCnt <= ObjGrid.rows.length - 1; SLoopCnt++) {
                        if (tmpChassisNo == ObjGrid.rows[SLoopCnt].cells[2].childNodes[1].innerHTML) {
                            ChassisCnt = ChassisCnt + 1;
                            if (ObjGrid.rows[SLoopCnt].cells[8].childNodes[1].checked == false) {
                                TotUserCheck = TotUserCheck + 1;
                                if (ObjGrid.rows[SLoopCnt].cells[8].childNodes[1].id == Obj.id) {
                                    tmpResetValue = ObjGrid.rows[SLoopCnt].cells[7].childNodes[1].value;
                                    tmpCountValue = SLoopCnt;
                                }
                                ObjGrid.rows[SLoopCnt].cells[7].childNodes[1].value = 0;
                                ObjGrid.rows[SLoopCnt].cells[7].childNodes[1].disabled = true;
                                ObjGrid.rows[SLoopCnt].cells[9].childNodes[1].value = "";
                                ObjGrid.rows[SLoopCnt].cells[9].childNodes[1].disabled = false;

                            }
                            else {
                                ObjGrid.rows[SLoopCnt].cells[7].childNodes[1].disabled = false;
                                ObjGrid.rows[SLoopCnt].cells[9].childNodes[1].value = "";
                                ObjGrid.rows[SLoopCnt].cells[9].childNodes[1].disabled = true;
                            }
                        }
                    }
                    //if (ChassisCnt == TotUserCheck) {
                    //    ObjGrid.rows[tmpCountValue].cells[7].childNodes[1].value = tmpResetValue;
                    //    ObjGrid.rows[tmpCountValue].cells[7].childNodes[1].disabled = false;
                    //    ObjGrid.rows[tmpCountValue].cells[9].childNodes[1].value = "";
                    //    ObjGrid.rows[tmpCountValue].cells[9].childNodes[1].disabled = true;
                    //    alert("Atleast One Part should be select for individual warranty claim");
                    //    Obj.checked = true
                    //    Obj.focus();
                    //    break;
                    //}
                }
            return true;
        }

        function SetClaimStatus(Obj) {
            var ObjGrid = null;
           
            debugger;
            var objRow = Obj.parentNode.parentNode.childNodes;
            var objChecked = Obj.checked;

            var ClaimNo = "";
            var PartNo = "";
            var select = "";

            ClaimNo = objRow[3].childNodes[1].innerText;
            PartNo = objRow[5].childNodes[1].innerText;

            if (objChecked == true) 
                select =" deselect ";
            else
                select =" select ";

            if (confirm("Are you sure you want to " + select + " Claim no: " + ClaimNo + " in the FPDA?") == true) {
            }
            else {
                return false;
            }

            ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
          
            var tmpClaimNo = "";
            var tmpPartNo = "";
        
            if (ObjGrid == null) return;
            var objtxtControl;
            //debugger;
            if (Obj != null)
                for (var MLoopCnt = 1; MLoopCnt <= ObjGrid.rows.length - 1; MLoopCnt++) {                    

                    tmpClaimNo = ObjGrid.rows[MLoopCnt].cells[2].childNodes[1].innerText;
                    tmpPartNo = ObjGrid.rows[MLoopCnt].cells[4].childNodes[1].innerText;
                    if (ClaimNo.trim() == tmpClaimNo.trim()) {
                        ObjGrid.rows[MLoopCnt].cells[10].childNodes[1].checked = objChecked;
                    }
                }

            ObjGrid = window.document.getElementById("ContentPlaceHolder1_gvScrapPart");
            if (ObjGrid == null) return;
            var objtxtControl;
            //debugger;
            if (Obj != null)
                for (var MLoopCnt = 1; MLoopCnt <= ObjGrid.rows.length - 1; MLoopCnt++) {

                    tmpClaimNo = ObjGrid.rows[MLoopCnt].cells[2].childNodes[1].innerText;                    
                    if (ClaimNo.trim() == tmpClaimNo.trim()) {
                        ObjGrid.rows[MLoopCnt].cells[6].childNodes[1].checked = objChecked;
                    }
                }            
            return true;
        }


        function ShowReportScarpRegister() {
            var iDocId = 0;
            var sExportYesNo = "";
            var RptTitle = "";
            var msgAnswer = "";
            var iUserRoleId = 0;

            var Control = document.getElementById('ContentPlaceHolder1_txtID');
            //Megha24012012
            var UserRole = document.getElementById('ContentPlaceHolder1_txtUserRoleID');
            iUserRoleId = UserRole.value;
            //Megha24012012



            if (Control == null) return;
            if (Control.value == "") {
                alert("Please Select The Record For Print!");
                return false;
            }
            else {
                iDocId = Control.value;
            }
            var Url = "/DCS/Forms/Common/frmDocumentView.aspx?RptName=/DCSReports";

            var sReportName = "";

            if (confirm("Are you sure, you want to Print the Scrap Report?") == true) {
                sReportName = "/RptFPDADetails&";  //+ strReportName + "&";
                //sExportYesNo = "";
                sFPDAYesNo = "N";
                sExportYesNo = "F"
            }
            else {
                return false;
            }
            if (sReportName == "") {
                return false;
            }
            Url = Url + sReportName + "ID=" + iDocId + "&FPDAYesNo=" + sFPDAYesNo + "&UserRoleId=" + iUserRoleId + "&ExportYesNo=" + sExportYesNo + "";


            //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No;    scroll: Yes; status: Yes;");
            var windowFeatures;
            window.opener = self;
            //window.close()  
            windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
            newWindow = window.open(Url, "", windowFeatures)
            window.moveTo(0, 0);
            window.resizeTo(screen.width, screen.height - 100);
            newWindow.focus();
            return false;
        }


        function ShowReportFPDAClaimReport() {

            var iDocId = 0;
            var sExportYesNo = "";
            var RptTitle = "";
            var msgAnswer = "";
            var iUserRoleId = 0;

            var Control = document.getElementById('ContentPlaceHolder1_txtID');

            if (Control == null) return;
            if (Control.value == "") {
                alert("Please Select The Record For Print!");
                return false;
            }
            else {
                iDocId = Control.value;
            }
            var Url = "/DCS/Forms/Common/frmDocumentView.aspx?RptName=/DCSReports";

            var sReportName = "";

            if (confirm("Are you sure, you want to Print the Report?") == true) {
                sReportName = "/RptFPDAClaimDetails&";  //+ strReportName + "&";
                //sExportYesNo = "";
                sFPDAYesNo = "";
                sExportYesNo = ""
            }
            else {
                return false;
            }
            if (sReportName == "") {
                return false;
            }
            //Url = Url + sReportName + "ID=" + iDocId + "&FPDAYesNo=" + sFPDAYesNo + "&UserRoleId=" + iUserRoleId + "&ExportYesNo=" + sExportYesNo + "";
            Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=" + sExportYesNo + "";

            //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No;    scroll: Yes; status: Yes;");
            var windowFeatures;
            window.opener = self;
            //window.close()  
            windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
            newWindow = window.open(Url, "", windowFeatures)
            window.moveTo(0, 0);
            window.resizeTo(screen.width, screen.height - 100);
            newWindow.focus();
            return false;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintScarpReg" Visible="false" runat="server" Text="Print Scrap Register" CssClass="btn btn-search"
                                OnClick="btnPrintScarpReg_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrintFPDAClaim" runat="server" Text="Print FPDA Claim Details" CssClass="btn btn-search" Visible="false"
                                OnClick="btnPrintFPDAClaim_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:Location ID="Location" runat="server" OnDDLSelectedIndexChanged="Location_DDLSelectedIndexChanged"
                        OndrpCountryIndexChanged="Location_drpCountryIndexChanged" OndrpRegionIndexChanged="Location_drpRegionChanged" />
                </asp:Panel>
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Document Details" ExpandedText="Document Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </asp:Panel>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Document Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        Height="16px"></asp:Label>
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
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered" style="width:100%">
                            <tr>
                                <td style="width:10% ; height: 15px;" class="tdLabel">
                                    <asp:Label ID="lblCreditDateFrom" runat="server" Text="Claim Date From"></asp:Label>
                                </td>
                                <td style="width: 15%; height: 15px;">
                                    <uc3:CurrentDate ID="txtFromDate" runat="server" />
                                </td>
                                <td style="width: 10%; height: 15px;" class="tdLabel">
                                    <asp:Label ID="lblCreditDateTo" runat="server" Text="Claim Date To :"></asp:Label>
                                </td>
                                <td style="width: 15%; height: 15px;">
                                    <uc3:CurrentDate ID="txtToDate" runat="server" />
                                </td>
                                <td class="tdLabel" style="width: 10%; height: 15px;">
                                    <asp:Label ID="lblDocNo" runat="server" Text="Doc No"></asp:Label>
                                </td>
                                <td style="width: 15%; height: 15px;">
                                     <asp:TextBox ID="txtDocNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        MaxLength="12"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 10%; height: 15px;">
                                     <asp:Label ID="lblDocDate" runat="server" Text="Doc Date"></asp:Label>
                                </td>
                                <td style="width: 15%; height: 15px;">
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" />
                                </td>
                            </tr>                            
                            <tr>
                                <td class="tdLabel" style="width: 10%;">Transporter:
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtTransporter" runat="server" CssClass="TextBoxForString" MaxLength="50"
                                        ReadOnly="false" Text=""></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td class="tdLabel" style="width: 10%">No Of Boxes
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtNoOfCases" runat="server" CssClass="TextForAmount" ReadOnly="false"
                                        Text="" MaxLength="5" onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);" onblur="return BoxHdrValidation(this);"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                               
                                <td class="tdLabel" style="width: 10%; height: 15px;">LR No:
                                </td>
                                <td style="width: 15%; height: 15px;">
                                    <asp:TextBox ID="txtLRNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text="" MaxLength="15"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td class="tdLabel" style="width: 10%; height: 15px;">LR Date
                                </td>
                                <td style="width: 15%; height: 15px;">
                                    <uc3:CurrentDate ID="txtLRDate" runat="server" />
                                </td>
                            </tr>                            
                            <tr>
                                <%--<td class="tdLabel" style="width: 10%;"></td>--%>
                                
                                <td class="tdLabel" style="width: 10%">Remarks:
                                </td>
                                <td style="width: 35%" colspan="3">
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                       Text="" style="Height:34px;width :462px"></asp:TextBox>
                                </td>
                                <td style="width: 10%;" colspan="4">
                                    <asp:LinkButton ID="lblSelectWarrantyClaim" runat="server" ForeColor="#49A3D3" Height="25px"
                                        Text="Select Warranty Claims" ToolTip="Select Warranty Claims" Width="77%" OnClick="lblSelectWarrantyClaim_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" colspan="8" style="color: Red; font-size: small;"><%--Records which are not selected, will not be available for selection in future.--%>
                                </td>                                
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEModelDetails" runat="server" TargetControlID="CntModelDetails"
                        ExpandControlID="TtlModelDetails" CollapseControlID="TtlModelDetails" Collapsed="false"
                        ImageControlID="ImgTtlModel Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Failed Part Details" ExpandedText="Failed Part Details"
                        TextLabelID="lblTtlModelDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlModelDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlModelDetails" runat="server" Text="Failed Part Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<div align="center" class="containtable" style="height: 200px; overflow: auto">--%>
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="96%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                            AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand"
                            Height="16px">
                            <%--OnRowDataBound="DetailsGrid_RowDataBound"--%>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <%--<asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                    <asp:ImageButton ID="ImgSelect" runat="server" 
                                        ImageUrl="~/Images/arrowRight.png"  
                                        Style="height: 16px" />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="17%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomer" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claim  No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCliam_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cliam_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claim Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCliam_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cliam_Date","{0:dd-MM-yyyy}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Code" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPart_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claimed Qty" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPart_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Qty","{0:#0.00}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Sent" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAccPartQty" runat="server" CssClass="TextForAmount" Text='<%# Eval("AccPart_Qty","{0:#0.00}") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Box No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBoxNo" runat="server" CssClass="TextForAmount" onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);"
                                            Text='<%# Eval("Box_no") %>' onblur="return BoxDtlsValidation(this);"> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sent (Y/N)" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForAccept" runat="server" Checked='<%# Eval("ChkForAccept") %>'
                                            onclick="return AcceptValidation(this);" />
                                        <%-- onclick="return AcceptValidation(this);"--%> <%--cal refernce FPDA unselect issue|| IM1310291 || SM23124|Priority P2--%>
                                    </ItemTemplate>
                                    <%-- <HeaderTemplate>
                                            <asp:CheckBox ID="ChkAcceptAll" runat="server" Text="Accept" ToolTip="Accept All" />
                                        </HeaderTemplate>--%>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRejRemark" runat="server" Enabled="false" CssClass="TextBoxForString" Text='<%# Eval("RejRemark") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="TextBoxForString" Text='<%# Eval("Location") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkStatus" runat="server" Checked='<%# Eval("Status") %>'
                                            onclick="return SetClaimStatus(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                    <%-- </div>--%>
                </asp:Panel>
                <asp:Panel ID="PScrapDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEScrapDetails" runat="server" TargetControlID="CntScrapDetails"
                        ExpandControlID="TtlScrapDetails" CollapseControlID="TtlScrapDetails" Collapsed="false"
                        ImageControlID="ImgTtlScrap Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Scrap Part Details" ExpandedText="Scrap Part Details"
                        TextLabelID="lblTtlScrapDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlScrapDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label3" runat="server" Text="Scrap Part Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                        onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div align="center" class="containtable" style="height: 200px; overflow: auto">
                        <asp:Panel ID="CntScrapDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                            <asp:GridView ID="gvScrapPart" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="96%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False"
                                Height="16px">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_Name") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claim  No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCliam_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cliam_No") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claim Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCliam_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cliam_Date","{0:dd-MM-yyyy}") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Code" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPart_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Qty" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPart_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Qty","{0:#0.00}") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkPStatus" runat="server" Checked='<%# Eval("Status") %>'
                                            onclick="return SetClaimStatus(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Box No" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBoxNo" runat="server" CssClass="TextForAmount"   onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);"  
                                            Text='<%# Eval("Box_no") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                      
                                <asp:TemplateField HeaderText="Accept (Y/N)" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center"  >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForAccept" runat="server" 
                                            Checked='<%# Eval("ChkForAccept") %>'   />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ChkAcceptAll" runat="server" Text="Accept"  
                                            ToolTip="Accept All"  />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="8%" />
                                </asp:TemplateField>       --%>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
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
                <asp:TextBox ID="txtUserRoleID" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                <asp:HiddenField ID="hdnMinCreditDate" runat="server" Value="" />
                <asp:HiddenField ID="hdnMaxCreditDate" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
            </td>
        </tr>
    </table>
</asp:Content>
