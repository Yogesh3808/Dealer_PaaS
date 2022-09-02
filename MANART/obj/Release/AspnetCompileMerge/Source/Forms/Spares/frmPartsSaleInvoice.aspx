<%@ Page Title="MTI- Part Invoice" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmPartsSaleInvoice.aspx.cs"
    Inherits="MANART.Forms.Spares.frmPartsSaleInvoice" MaintainScrollPositionOnPostback="true" Theme="SkinFile" EnableEventValidation="false" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
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

    <script src="../../Scripts/jsPartsOAInv.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsPartSpareScheme.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtInvDate = document.getElementById("ContentPlaceHolder1_txtInvDate_txtDocDate");
            $('#ContentPlaceHolder1_txtInvDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtInvDate.value == '') ? '0d' : txtInvDate.value, maxDate: (txtInvDate.value == '') ? '0d' : txtInvDate.value
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

        // Function Validation
        function Validation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_DrpInvType").value == "0") {
                errMessage += "*Please Select Invoice Type.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtInvNo").value == "") {
                errMessage += "*Please Do not Blank Invoice No.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtInvDate_txtDocDate").value == "") {
                errMessage += "*Please Do not Blank Invoice Date.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_DrpCustomer").value == "0") {
                errMessage += "*Please Select Customer.\n";
            }
            if (errMessage != "") {
                alert(errMessage);
                return false;
            }
            else {
                return true;
            }
        }

        function ShowChassisMaster(objNewModelLabel, sDealerId, sHOBrID) {
            //var ChassisNo;
            //debugger;
            var JobID;
            var ObjDrpInvType = document.getElementById("ContentPlaceHolder1_DrpInvType");
            if (ObjDrpInvType.value == "0") {
                alert("Please Select Invoice Type");
                return false;
            }
            var JbInvType = (ObjDrpInvType.value == "3" || ObjDrpInvType.value == "4") ? "JbInv" : (ObjDrpInvType.value == "5" || ObjDrpInvType.value == "6") ? "JbInvP" : "JbInvL";
            //JobID = window.showModalDialog("../Common/frmJobcardSelection.aspx?DealerID=" + sDealerId + "&HOBrID=" + sHOBrID + "&sDocType=0&sDocFormat=JbInv", 'PopupPage', 'dialogHeight:300px;dialogWidth:1000px;resizable:0;location=no;');
            JobID = window.showModalDialog("../Common/frmJobcardSelection.aspx?DealerID=" + sDealerId + "&HOBrID=" + sHOBrID + "&sDocType=0&sDocFormat=" + JbInvType, 'PopupPage', 'dialogHeight:300px;dialogWidth:1000px;resizable:0;location=no;');
            //debugger;
            if (JobID == null) {
                return false;
            }
            else {
                hdnJobHDRID = document.getElementById("ContentPlaceHolder1_hdnJobHDRID");
                if (hdnJobHDRID != null) {
                    hdnJobHDRID.value = JobID[1];
                }
            }
            return true;
        }

        function GetGatePassDtls(objLink, sDealerId, sJbInvID, sSlInvID) {
            var iJob_HDR_ID = 0;
            //var objRow = objLink.parentNode.parentNode.childNodes;
            var ObjControl = window.document.getElementById("ContentPlaceHolder1_hdnJobHDRID");
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
            var feature = "dialogWidth:700px;dialogHeight:200px;status:no;help:no;scrollbars:no;resizable:no;";

            GPID = window.showModalDialog("../Service/frmGatepass.aspx?" + Parameters, null, feature);

            if (GPID != null) {
                ObjhdnGPID.value = GPID;
            }
            return;
        }

        // To Edit Part Rate
        function ShowPartRate(ObjQtyControl, sGroupCode, sDealerId, iPartID) {
            var PartRateValue;
            var SqlFor = "";
            if (sGroupCode == "01" || sGroupCode == "02" || sGroupCode == "99")
                SqlFor = "PartPrice";

            PartRateValue = window.showModalDialog("../Master/frmShowPartPriceDet.aspx?ModelPart=" + sGroupCode + "&DealerID=" + sDealerId + "&PartID=" + iPartID + "&SqlFor=" + SqlFor + "&FromPage=" + "OA_Invoice", "List", "dialogHeight: 350px; dialogWidth: 900px;");// "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
            if (PartRateValue != null) {
                //debugger;
                var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
                objRow[12].childNodes[1].value = PartRateValue[8];
                objRow[13].childNodes[1].value = PartRateValue[7];
                var Qty = dGetValue(objRow[9].childNodes[1].value);
                var DiscPer = dGetValue(objRow[14].childNodes[1].value);
                var FOBRate = dGetValue(objRow[13].childNodes[1].value);
                var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
                if (isNaN(DiscAmt) == true) DiscAmt = 0;
                objRow[15].childNodes[1].value = parseFloat(DiscAmt).toFixed(2);

                var PartRate = dGetValue(FOBRate) - dGetValue(DiscAmt);
                if (isNaN(PartRate) == true) PartRate = 0;

                var PartTaxRate = objRow[18].children[4].value;
                var PartTax1Rate = objRow[18].children[6].value;
                var PartTax2Rate = objRow[18].children[8].value;

                var FOCTag = objRow[23].childNodes[1].value;
                var WarrTag = objRow[24].childNodes[1].value;

                if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
                if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

                var PartRateTaxAmt = 1 + (dGetValue(PartTaxRate / 100) + dGetValue(PartTax1Rate / 100) + dGetValue(PartTax2Rate / 100));
                if (isNaN(PartRateTaxAmt) == true) PartRateTaxAmt = 0;

                var PartRateExclTax = dGetValue(PartRate / PartRateTaxAmt);

                if (isNaN(PartRateExclTax) == true) PartRateExclTax = 0;

                objRow[16].childNodes[1].value = parseFloat(PartRate).toFixed(2);
                var Total = (FOCTag.trim() == "N" && WarrTag.trim() == "N") ? dGetValue(Qty) * PartRate : 0;
                if (isNaN(Total) == true) Total = 0;
                objRow[17].childNodes[1].value = RoundupValue(Total);

                objRow[16].childNodes[3].value = parseFloat(PartRateExclTax).toFixed(2);
                var PartTotalExclTax = dGetValue(Qty) * PartRateExclTax;
                if (isNaN(PartTotalExclTax) == true) PartTotalExclTax = 0;
                //objRow[17].childNodes[3].value = RoundupValue(PartTotalExclTax);
                objRow[17].childNodes[3].value = RoundupValue(Total);

                CalulateInvPartGranTotal()

            }
        }

        //To Part Master
        function ShowSpWPFPart_New(objNewPartLabel, sDealerId, objCustTypeID, iHOBrId) {
            if (Validation() == false) {
                return false;
            }
            var PartDetailsValue;
            //var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            var sSelectedPartID = GetPreviousSelectedOAPartID(objNewPartLabel);
            var objCustType = document.getElementById("ContentPlaceHolder1_" + objCustTypeID);
            //var RefType = document.getElementById("ContentPlaceHolder1_drpReferType");
            //var sRefType = RefType.options[RefType.selectedIndex].value;
            //var sCustType = objCustType[objCustType.selectedIndex].innerText.trim();
            var sCustType = objCustType.options[objCustType.selectedIndex].text;
            var sCustID = objCustType[objCustType.selectedIndex].value;
            var sInvType = document.getElementById("ContentPlaceHolder1_DrpInvType").value
            if (sInvType == "1" || sInvType == "2")
                sSelCase = "D"
            else
                sSelCase = "R";
            //var sRefType = "N";
            //var sSelCase = "";
            //if (sRefType == "Y") {
            //    sSelCase = "R";
            //}
            //else {
            //    sSelCase = "D"
            //}
            var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
            var sCustTaxTag = hdnCustTaxTag.value;
            var hdnIsDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
            var sDocGST = hdnIsDocGST.value;
            PartDetailsValue = window.showModalDialog("../Common/frmSelMultiPartPenSparesOA.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&SelCase=" + sSelCase + "&CustID=" + sCustID + "&HOBR_ID=" + iHOBrId + "&sDocGST=" + sDocGST + "&CustTaxTag=" + sCustTaxTag, "List", "dialogHeight: 590px; dialogWidth: 950px;");// "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
            // PartDetailsValue = window.showModalDialog("../Common/frmSelectMultiPartSparesOA.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&SelCase=" + sSelCase + "&CustID=" + sCustID, "List", "dialogHeight: 550px; dialogWidth: 700px;");// "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
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

        function GetPreviousSelectedOAPartID(objNewPartLabel) {
            //debugger;
            var objRow;
            var PartIds = "";
            var PartId = "";
            var txtPartId;
            var txtOADetId;
            var OADetId = "";
            // get grid object
            var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
            if (objGrid == null) return PartIds;
            for (var i = 2; i <= objGrid.rows.length - 1 ; i++) {
                //Get Row
                objRow = objGrid.rows[i];

                //Get Part ID
                txtPartId = objGrid.rows[i].children[1].childNodes[1];

                txtOADetId = objGrid.rows[i].children[2].childNodes[1];
                //Get PartId;
                OADetId = (txtOADetId.value);

                if (OADetId == "0" && OADetId == "" && OADetId == null) {
                    OADetId = "0";
                }

                PartId = OADetId + "-" + (txtPartId.value);
                if (PartId != "0" && PartId != "" && PartId != null) {
                    PartIds = PartIds + PartId + ",";
                }
            }
            PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

            return PartIds;
        }
        //function SelectDeletCheckboxAndCalcInv(ObjChkDelete) {
        //    debugger;
        //    if (ObjChkDelete.checked) {
        //        if (confirm("Are you sure you want to delete this record?") == true) {
        //            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
        //            CalulateInvPartGranTotal();
        //        }
        //        else {
        //            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        //            ObjChkDelete.checked = false;
        //            CalulateInvPartGranTotal();
        //            return false;
        //        }
        //    }
        //    else {
        //        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        //        CalulateInvPartGranTotal();
        //    }
        //}

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
    <style>
        .cls {
            display: inline-block;
            vertical-align: middle;
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Sale Invoice"> </asp:Label>
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
                    <uc2:Location ID="Location" runat="server"
                        OnDDLSelectedIndexChanged="Location_DDLSelectedIndexChanged" />
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
                        SuppressPostBack="true" CollapsedText="Invoice Details" ExpandedText="Invoice Details"
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
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 10%" class="tdLabel">
                                    <div id="AllErrors"></div>
                                    <asp:Label ID="Label2" runat="server" Text="Invoice Type"></asp:Label>
                                </td>
                                <td style="width: 22%">
                                    <asp:DropDownList ID="DrpInvType" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="DrpInvType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMInvType" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 10%" class="tdLabel">
                                    <asp:Label ID="lblPONo" runat="server" Text="Invoice No"></asp:Label>
                                </td>
                                <td style="width: 22%">
                                    <asp:TextBox ID="txtInvNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td style="width: 10%" class="tdLabel">
                                    <asp:Label ID="lblPODate" runat="server" Text="Invoice Date"></asp:Label>
                                </td>
                                <td style="width: 22%">
                                    <uc3:CurrentDate ID="txtInvDate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 10%">Customer:
                                </td>
                                <td style="width: 22%">
                                    <asp:DropDownList ID="DrpCustomer" runat="server" CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <%--OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true"--%>
                                    <asp:Label ID="lblMCustomer" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                    <%--onBlur="SetCustType(this,'DrpCustomerType')"--%>
                                </td>
                                <%--<td class="tdLabel" style="width: 15%">
                                    Customer Type:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpCustomerType" runat="server" CssClass="ComboBoxFixedSize" Enabled="false">
                                    </asp:DropDownList>
                                </td>--%>
                                <td class="tdLabel" style="width: 10%">Refernce :
                                </td>
                                <td style="width: 22%">
                                    <asp:TextBox ID="txtRefernce" Text="" runat="server" CssClass="TextBoxForString" TextMode="MultiLine" MaxLength="50"
                                        onblur="checkTextAreaMaxLength(this,event,'50');" Rows="2"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 10%">
                                    <asp:LinkButton ID="lblSelectModel" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Select Jobcard" Width="80%"
                                        ToolTip="Select Jobcard No " OnClick="lblSelectModel_Click"> </asp:LinkButton>
                                    <asp:Button ID="BtnOpen" runat="server" Text="Open Invoice" CssClass="btn btn-search btn-sm" Width="125px" OnClick="BtnOpen_Click" />
                                </td>
                                <td style="width: 22%">
                                    <asp:LinkButton ID="lnkGatePass" runat="server" Style="height: 12px" Text="Gatepass" ToolTip="Gatepass" Width="80%"> </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" colspan="2">
                                    <div>
                                        <asp:RadioButtonList ID="rbtLstDiscount" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="rbtLstDiscount_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Freight In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Freight In Amount" Value="Amt"></asp:ListItem>
                                            <%--<asp:ListItem Text=" Discount In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Discount In Amount" Value="Amt"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    <%--Scheme:--%>
                                </td>
                                <td style="width: 18%">
                                    <%--<asp:CheckBox ID="ChkScheme" runat="server" Text="Scheme" />--%>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    <%--Scheme:--%>
                                </td>
                                <td style="width: 18%">
                                    <%--<asp:TextBox ID="txtNarration" Text="" runat="server" CssClass="TextBoxForString" MaxLength="100" TextMode="MultiLine"></asp:TextBox>--%>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PInvDesc" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="">
                    <cc1:CollapsiblePanelExtender ID="CPEInvDesc" runat="server" TargetControlID="CntInvDesc"
                        ExpandControlID="TtlInvDesc" CollapseControlID="TtlInvDesc" Collapsed="True"
                        ImageControlID="ImgTtlInvDesc" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Invoice Job Description"
                        ExpandedText="Invoice Job Description" TextLabelID="lblTtlInvDesc">
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
                                        <asp:DropDownList ID="DrpInvJobDesc" runat="server" CssClass="GridComboBoxFixedSize" Width="90%">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="true"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true"
                        TextLabelID="lblTtlPartDetails">
                        <%--CollapsedText="Part Details" ExpandedText="Part Details"--%>
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
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered"
                            OnPageIndexChanging="PartGrid_PageIndexChanging" OnRowCommand="PartGrid_RowCommand" OnRowDataBound="PartGrid_RowDataBound">
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
                                        <asp:TextBox ID="txtPartID" runat="server" Width="5%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OA Detl ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOADetID" runat="server" Width="5%" Text='<%# Eval("OA_Det_ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OA No" ItemStyle-Width="10%" >
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOANo" runat="server" Text='<%# Eval("OANo") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="8%" >
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblSelectPart" runat="server" ForeColor="#49A3D3" Text="Select Part"
                                            onmouseover="SetCancelStyleonMouseOver(this);" ToolTip="Select Part" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>--%>
                                        <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:LinkButton ID="lnkSelectPart" runat="server" CssClass="btn btn-link" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="18%" >
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Group" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrNo" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("group_code") %>'
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OA Bal Qty" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOABal_Qty" runat="server" CssClass="GridTextBoxForAmount"
                                            Width="90%" Text='<%# Eval("OABal_Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inv Qty" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Qty","{0:#0.00}") %>' MaxLength="6"
                                            Width="96%"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalculateInvPartTotal(event,this);"--%>
                                        <asp:TextBox ID="txtPreviousInvQty" runat="server" CssClass="GridTextBoxForAmount HideControl"
                                            Width="90%" Text='<%# Eval("Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Stock" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalQty" runat="server" CssClass="GridTextBoxForAmount"
                                            Width="90%" Text='<%# Eval("bal_qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>

                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUnit" runat="server" Text='<%# Eval("Unit") %>' Width="96%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MRP" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("Price","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <%--Vikram on 29082017--%>
                                <%--<asp:TemplateField HeaderText="Rate" ItemStyle-Width="6%"> --%>
                                <asp:TemplateField HeaderText="Selling List Price" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%" onfocusin="return CalculateInvPartTotal(event,this);" onchange="return CalculateInvPartTotal(event,this);"
                                            Text='<%# Eval("MRPRate","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Per" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscountPer" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("discount_per","{0:#0.00}") %>' MaxLength="6"></asp:TextBox>
                                        <%--onblur="return CalculateInvPartTotal(event,this);" onkeypress=" return CheckForTextBoxValue(event,this,'6');"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscountAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("discount_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <%--Vikram on 29082017--%>
                                <%--<asp:TemplateField HeaderText="Discounted Rate" ItemStyle-Width="6%">--%>
                                <asp:TemplateField HeaderText="Taxable Price" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscountRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("disc_rate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        <asp:TextBox ID="txtExclDiscountRate" runat="server" CssClass="HideControl" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:TextBox ID="TxtExclTotal" runat="server" CssClass="HideControl" Width="80%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:TextBox ID="txtLabourTotal" runat="server" CssClass="GridTextBoxForAmount HideControl" Width="96%"
                                            Text='<%# Eval("Labour_Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Tax" ItemStyle-Width="7%">
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
                                        <%--Sujata 05092014_End--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Rate" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditRate" CssClass="btn btn-link" runat="server">Edit Rate</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="6%" ControlStyle-CssClass="cls">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxAndCalcInv(event,this);" />
                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LabTag" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtlabtag" runat="server" Width="5%" Text='<%# Eval("lab_tag") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="foctag" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtfoctag" runat="server" Width="5%" Text='<%# Eval("foc_tag") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="wartag" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtwartag" runat="server" Width="5%" Text='<%# Eval("war_tag") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="part_type_tag" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParttypetag" runat="server" Width="5%" Text='<%# Eval("part_type_tag") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warranty" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="DrpWarrType" runat="server" CssClass="GridComboBoxFixedSize" Width="99%"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FOC" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFOC" runat="server" Width="96%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BFRGST" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBFRGST" runat="server" Width="96%" Text='<%# Eval("BFRGST") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pre GST Stock" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBFRGST_Stock" runat="server" Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                            Text='<%# Eval("BFRGST_Stock","{0:#0.00}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>

                    <table id="tblTotal" runat="server" class="table table-bordered" width="100%">
                        <tr>
                            <td style="width: 55%; text-align: right"></td>
                            <td style="width: 9%; text-align: left">
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="HideControl" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                            <td style="width: 22%; text-align: left">
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="HideControl" Font-Bold="true"
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
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered"
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
                                        <%--<asp:TextBox ID="txtGrnetrevamt" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            MaxLength="6"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);" MaxLength="6"--%>
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
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                        ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                        ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Invoice Tax Details" ExpandedText="Invoice Tax Details"
                        TextLabelID="lblTtlTaxDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlTaxDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Invoice Tax Details" Width="96%"
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
                                <asp:TemplateField HeaderText="Freight %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                            Width="90%" MaxLength="5"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Freight Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight Tax%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFTaxPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("PF_Tax_Per","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight IGST Amt" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFIGSTorSGSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("PF_IGSTorSGST_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight CGST Tax%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFTaxPer1" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("PF_Tax_Per1","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Freight CGST Amt" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPfCGSTrAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("PF_CGST_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Inv_tot","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
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
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="N" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                <asp:HiddenField ID="hdnCancel" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsWithOA" runat="server" Value="N" />
                <asp:HiddenField ID="hdnJobHDRID" runat="server" Value="N" />
                <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="N" />
                <asp:HiddenField ID="hdnGPID" runat="server" Value="N" />
                <asp:HiddenField ID="hdnMenuID" runat="server" Value="" />
                <asp:HiddenField ID="hdnInvTypeID" runat="server" />
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
