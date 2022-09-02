<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    Theme="SkinFile" EnableEventValidation="false" CodeBehind="frmEstimate.aspx.cs" Inherits="MANART.Forms.Service.frmEstimate" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%--<%@ Register Src="~/WebParts/ExportLocation.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDateOnly" TagPrefix="uc6" %>
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
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
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
    <script src="../../Scripts/jsEstimateFunction.js"></script>
    <script src="../../Scripts/jsEstimatePartFunc.js"></script>
    <script src="../../Scripts/jsEstimateLabourFunc.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsShowForm.js"></script>

    <style type="text/css">
        .scrolling-table-container {
            overflow-y: hidden;
            overflow-x: scroll;
        }
    </style>
    <script type="text/javascript">
        opener.ChkLabourClick(objImgControl);


        //Click on Selection Part
        function ChkSpNDPPartClickOnJobcard(objImgControl) {
            //var objID = $('#' + objImgControl.id);
            //var objCol = objID[0].parentNode.parentNode;
            //var txtparst = document.getElementById("txtPartIds");
            //Changed by Vikram
            objImgControl.parentNode.parentNode.childNodes;
            var objCol = objImgControl.parentNode.parentNode
            //var txtparst = document.getElementById('txtPartIds');
            var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds');

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

        //Function For return Chassis Details
        function ReturnChassisValue(objImgControl) {
            //debugger;
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            var sValue;
            var ArrOfChassis = new Array();
            for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
                sValue = objRow[cnt].innerText.trim();
                ArrOfChassis.push(sValue);
            }
            if (ArrOfChassis != null) {
                var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
                txtChassisID.value = ArrOfChassis[1];
            }

            return true;
            //$find("mpe").hide();
            //return false;
            //alert(ArrOfChassis);
            //window.returnValue = ArrOfChassis;
            //window.close();

        }
        //To Show Chassis Master
        function ShowChassisMaster(objNewModelLabel, sDealerId, sHOBR_ID, sUserDepart) {
            //var ChassisDetailsValues;

            //ChassisDetailsValues = window.showModalDialog("../Common/frmChassisSelection.aspx?DealerID=" + sDealerId + "&HOBR_ID=" + sHOBR_ID + "&JobTypeID=999", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            //if (ChassisDetailsValues != null) {
            //    //SetChassisDetails(ChassisDetailsValues, sUserDepart);
            //    //debugger;
            //    var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
            //    txtChassisID.value = ChassisDetailsValues[1];
            //}

            return true;
        }

        //Function For Set Chassis Details
        function SetChassisDetails(ChassisDetailsValue, sUserDepart) {
            //debugger;
            var txtChassisNo = window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
            var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
            var txtVehicleNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleNo');
            var txtEngineNo = window.document.getElementById('ContentPlaceHolder1_txtEngineNo');
            var txtCustomer = window.document.getElementById('ContentPlaceHolder1_txtCustomer');
            var txtCustID = window.document.getElementById('ContentPlaceHolder1_txtCustID');
            var TxtModelCode = window.document.getElementById('ContentPlaceHolder1_TxtModelCode');
            var txtModelName = window.document.getElementById('ContentPlaceHolder1_txtModelName');
            var txtModelGroupID = window.document.getElementById('ContentPlaceHolder1_txtModelGroupID');
            var txtModCatIDBasic = window.document.getElementById('ContentPlaceHolder1_txtModCatIDBasic');
            var hdnCustTaxTag = window.document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');

            txtChassisID.value = ChassisDetailsValue[1];
            txtChassisNo.value = ChassisDetailsValue[2];
            txtVehicleNo.value = ChassisDetailsValue[3];
            txtCustomer.value = ChassisDetailsValue[4];
            txtEngineNo.value = ChassisDetailsValue[7];
            txtCustID.value = ChassisDetailsValue[9];
            TxtModelCode.value = ChassisDetailsValue[10];
            txtModelName.value = ChassisDetailsValue[11];
            txtModelGroupID.value = ChassisDetailsValue[16];
            //debugger;
            txtModCatIDBasic.value = ChassisDetailsValue[34];
            hdnCustTaxTag.value = ChassisDetailsValue[35];
            //SetGridColumnsBasedOnChStatusJobType(sUserDepart);

            //        if (txtCustEdit.value == "Y") {
            //            txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
            //        }
            //        else {
            //            txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
            //        }
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
                ////debugger;
                //var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
                var dtpAllocateTime = document.getElementById("ContentPlaceHolder1_dtpAllocateTime_txtDocDate");
                var dtpJobCommited = document.getElementById("ContentPlaceHolder1_dtpInspectDt_txtDocDate");
                var dtpJobOpeningTm = document.getElementById("ContentPlaceHolder1_dtpVisitTime_txtDocDate");

                var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
                $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick({
                    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
                });
                $('#ContentPlaceHolder1_txtInsValidDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value
                });

                $('#ContentPlaceHolder1_dtpInspectDt_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value
                });
                $('#ContentPlaceHolder1_dtpVisitTime_txtDocDate').datepick({
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
                                    <asp:LinkButton ID="lblSelectModel" runat="server" CssClass="btn btn-link" Text="Select Chassis"
                                        ToolTip="Select Chassis Details" OnClick="lblSelectModel_Click"> </asp:LinkButton><%--OnClick="lblSelectModel_Click"  onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" --%>
                                </td>
                                <td style="width: 18%">
                                    <asp:Button ID="btnDownload" Visible="false" runat="server" CssClass="btn btn-search btn-sm" Text="Download" OnClick="btnDownload_Click" /></td>
                                <td style="width: 15%" class="">&nbsp;</td>
                                <td style="width: 18%">&nbsp;</td>
                                <td style="width: 15%" class="">&nbsp;</td>
                                <td style="width: 18%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblDocNo" runat="server" Text="Estimate No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDocNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblDocDate" runat="server" Text="Estimate Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="true" />
                                </td>
                                <td style="width: 15%" class="">&nbsp;</td>
                                <td style="width: 18%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblVehicleNo" runat="server" Text="Veh Reg No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    <%-- <b class="Mandatory">*</b>--%>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblChassisNo" runat="server" Text="Chassis No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    <b class="Mandatory">*</b>
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
                                    <%--<asp:DropDownList ID="DrpCustomer" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>   --%>
                                    <%--<b class="Mandatory">*</b>--%>
                                    <asp:TextBox ID="txtCustomer" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblModel" runat="server" Text="Model code.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="TxtModelCode" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label4" runat="server" Text="Model Name.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtModelName" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="" style="width: 15%">Suppervisor Name:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpSupervisorName" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                <td class="" style="width: 15%">Insurance Validity Dt</td>
                                <td style="width: 18%">
                                    <uc6:CurrentDateOnly ID="txtInsValidDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                </td>
                                <td class="" style="width: 15%;"></td>
                                <td style="width: 18%;"></td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label3" runat="server" Text="Phone:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="TextBoxForString" MaxLength="11" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label2" runat="server" Text="Inspection Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="dtpInspectDt" runat="server" Mandatory="false" bTimeVisible="true" bCheckforCurrentDate="true" />
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label1" runat="server" Text="Visit Date:"></asp:Label></td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="dtpVisitTime" runat="server" Mandatory="false" bTimeVisible="true" bCheckforCurrentDate="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label5" runat="server" Text="Insurnce Company Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInsurnceComp" runat="server" CssClass="TextBoxForString" MaxLength="50" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label6" runat="server" Text="PolicyNo:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="TextBoxForString" MaxLength="25" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label7" runat="server" Text="Driver Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDriverName" runat="server" CssClass="TextBoxForString" MaxLength="50" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label8" runat="server" Text="Driver Liscence No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDriverLicNo" runat="server" CssClass="TextBoxForString" MaxLength="25" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label9" runat="server" Text="Surveyor:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtSurveyor" runat="server" CssClass="TextBoxForString" MaxLength="50" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label10" runat="server" Text="Insurance Division:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInsuranceDiv" runat="server" CssClass="TextBoxForString" MaxLength="50" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
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
                    <asp:Panel ID="PInvDesc" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEInvDesc" runat="server" TargetControlID="CntInvDesc"
                                    ExpandControlID="TtlInvDesc" CollapseControlID="TtlInvDesc" Collapsed="True"
                                    ImageControlID="ImgTtlInvDesc" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Estimate Job Description"
                                    ExpandedText="Estimate Job Description" TextLabelID="lblTtlInvDesc">
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
                        <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            Style="display: none;">
                            <div class="scrolling-table-container WordWrap">
                                <asp:GridView ID="PartDetailsGrid" runat="server" AllowPaging="false" Width="100%"
                                    AutoGenerateColumns="False" EditRowStyle-BorderColor="Black"
                                    GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand"
                                    SkinID="NormalGrid" OnRowDataBound="PartDetailsGrid_RowDataBound"
                                    AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    EditRowStyle-Wrap="true"
                                    HeaderStyle-Wrap="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartID" runat="server" Text='<%# Eval("PartLabourID") %>' Width="1%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAddPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" Text="SelectPart" ToolTip="Click Here To Select Part"
                                                    Width="70%"></asp:Label>--%>
                                                <asp:LinkButton ID="lnkSelectPart" runat="server" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="GridTextBoxForString"
                                                    Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartName" runat="server" CssClass="GridTextBoxForString" Text=""
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Est Qty" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReqQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'5');"
                                                    onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="9"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Type Tag" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartTypeTag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DtlID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDtlPartID" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                <asp:TextBox ID="txtPartGroupCode" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtPTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancel" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkForDelete" runat="server" Text="Delete" onClick="return SelectDeleteCheckboxForPart(this);" />
                                                <%--<asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" onclick="ClearRowValueForPartWarranty(event,this); "
                                                            onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"
                                                            Text="Cancel"></asp:Label>--%>
                                                <%--<asp:LinkButton ID="lnkNew" runat="server" OnClientClick="return CheckRowValue(event,this);">New</asp:LinkButton>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <%--<asp:UpdateProgress ID="UPRPart" runat="server" AssociatedUpdatePanelID="UPPart">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
                    <%--<asp:UpdatePanel UpdateMode="Conditional" ID="UPLabor" runat="server" ChildrenAsTriggers="true">                     
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
                            Style="display: none;">
                            <div class="scrolling-table-container WordWrap">
                                <asp:GridView ID="LabourDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                    GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                    OnRowDataBound="LabourDetailsGrid_RowDataBound"
                                    SkinID="NormalGrid" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
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
                                        <asp:TemplateField HeaderText="Labour Code" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <%-- <asp:Label ID="lblNewLabour" runat="server" ForeColor="#49A3D3" Text="SelectLabour"
                                                    ToolTip="Click Here To Select Labour Code" onmouseover="SetCancelStyleonMouseOver(this);"
                                                    onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>--%>
                                                <asp:LinkButton ID="lnkSelectLabour" runat="server" OnClick="lnkSelectLabour_Click">Select Labour</asp:LinkButton>
                                                <asp:TextBox ID="txtLabourCode"
                                                    runat="server" Text="" Width="96%" CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Description" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourDesc" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other Description" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpLbrDescription" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="90%" onblur="return CheckLbrDescSelected(event,this);">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtLbrDescription" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                    Width="90%" MaxLength="50" onblur="return CheckLbrDescAlreadyUsedInGrid(event,this);"></asp:TextBox>
                                                <%--<asp:TextBox ID="txtLbrDescription" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Man Hrs" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtManHrs" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server"  
                                                    CssClass="GridTextBoxForAmount" Width="96%" MaxLength="9"></asp:TextBox> <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                <asp:TextBox ID="txtOldAmount" runat="server" CssClass="HideControl" Width="4%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C/D" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabCD" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LbrInfo" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabMnGr" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLGroupCode" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLTax" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLTax1" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                                <asp:TextBox ID="txtLTax2" runat="server" Text="" Width="98%" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sublet Amt " ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSubletAmt" runat="server" CssClass="GridTextBoxForAmount" Width="96%" MaxLength="6"
                                                    onkeypress="return CheckForTextBoxValue(event,this,'6');" onblur="return calculateLabourTotal(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sublet Description" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSubletDescription" runat="server" Text="" Width="98%" MaxLength="30" CssClass="GridTextBoxForString"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancel" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxForLabour(this);"
                                                    Text="Delete" />
                                                <%-- <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" onclick="return ClearRowValueForLabourWarranty(event,this); "
                                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <%--   </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRLabor" runat="server" AssociatedUpdatePanelID="UPLabor">
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
                                        <asp:TemplateField HeaderText="Discount %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                    MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateEstPartGranTotal();"></asp:TextBox>
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
                    <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                            ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                            ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Estimate Tax Details" ExpandedText="Estimate Tax Details"
                            TextLabelID="lblTtlTaxDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlTaxDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Estimate Tax Details" Width="96%"
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
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered">
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
                                    <asp:TemplateField HeaderText="PF Charges%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                                Width="90%" MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateEstPartGranTotal();"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PF Charges Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this)" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Other Charges %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                                Text='<%# Eval("other_per","{0:#0.00}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateEstPartGranTotal();"></asp:TextBox>
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
                                                Text='<%# Eval("Estimate_tot","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
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
                                    <asp:TextBox ID="txtLabourAmount" runat="server" CssClass="TextForAmount" EnableViewState="true"
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
                                    <b>Estimate Amount:</b>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtEstTotAmt" runat="server" CssClass="TextForAmount" Text="0" Font-Bold="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

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
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:TextBox ID="txtCustID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="txtModelGroupID" runat="server" Value="N" />
                    <asp:TextBox ID="txtChassisID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtModCatIDBasic" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <%--   <asp:HiddenField ID="hdnAMCType" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnKAM" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnUpgCamp" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnUndObserv" runat="server" Value="N" />--%>
                    <asp:Label ID="lblComplaintsRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblPartRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblLabourRecCnt" Visible="false" runat="server" Text="0"></asp:Label>
                    <asp:TextBox ID="txtChkfun" runat="server" CssClass="HideControl" Text="false"></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnSelectedLabourID" runat="server" Value="N" />
                    <asp:TextBox ID="txtNewRecountCount" runat="server" Text="1" Width="1%" CssClass="HideControl"></asp:TextBox>
                    <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="" />
                    <asp:HiddenField ID="hdnISDocGST" runat="server" />   
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                    <asp:HiddenField ID="hdnRounOff" runat="server" Value="" />
                </td>
            </tr>
        </table>
    </div>
    <%-- Select Chassis --%>
    <div>
        <cc1:ModalPopupExtender ID="mpeSelectChassis" runat="server" TargetControlID="lblTragetID1" PopupControlID="pnlPopupWindow"
            BehaviorID="mpe1"
            BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
        <asp:Label ID="lblTragetID1" runat="server"></asp:Label>
        <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup_SelectChassis" Style="display: none">
            <table class="" border="1">
                <tr id="TitleOfPage1" class="panel-heading">
                    <td class="panel-title" align="center">
                        <asp:Label ID="Label11" runat="server" Text="Chassis Selection"> </asp:Label>
                    </td>
                </tr>
                <tr id="TblControl1">
                    <td style="width: 14%">
                        <div align="center" class="">
                            <table style="background-color: #efefef;" width="50%">
                                <tr align="center">
                                    <td class="tdLabel" style="width: 7%;">Search:
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    </td>

                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:DropDownList ID="DdlSelctionCriteria" runat="server"
                                            CssClass="ComboBoxFixedSize">
                                            <asp:ListItem Selected="True" Value="Chassis_no">Chassis No</asp:ListItem>
                                            <asp:ListItem Value="Reg_no">Vehicle Reg No</asp:ListItem>
                                            <asp:ListItem Value="Customer_name">Customer Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnOkChassis" runat="server" Text="OK" OnClick="btnOkChassis_Click" CssClass="btn btn-search" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            class="ContaintTableHeader" ScrollBars="Vertical">
                            <asp:GridView ID="ChassisGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-bordered"
                                EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                                AutoGenerateColumns="false"
                                OnPageIndexChanging="ChassisGrid_PageIndexChanging">
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Image ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" onClick="return ReturnChassisValue(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chassis No." ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChassisNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Vehicle_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle In No" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehInNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("VehInNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle In DateTime" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehInDate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("VehInTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEngineNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Engine_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Editing" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustEdit" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CustEdit") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CustID" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CustID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model Name" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aggregate" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAggregate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Aggregate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="WarrantyTag" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrantyTag" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("WarrantyTag") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AMC_Chk" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAMC_Chk" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_Chk") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AMC Type" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAMC_Type" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_Type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model Group ID" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblModelGrID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_Gr_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Veh In ID" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehInID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("VehInID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chassis Last Kms" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastKm" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstKm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="chassis Last Hrs" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastHrs" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstHrs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AMC Start Kms" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAMCStKm" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_Start_KM") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AMC End Kms" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAMCEndKm" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_End_KM") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Kms Before Spd Mtr Change" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBfr_Last_SpdMtrChange_Kms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Bfr_Last_SpdMtrChange_Kms") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hrs Before Hrs Mtr Change" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBfr_Last_OdoMtrChange_Kms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Bfr_Last_HrsMtrChange_Hrs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="KAM" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblKAM" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("In_KAM") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upgrade Campaign" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUpgrdCamp" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UpgrdCamp") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AMC End Date" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAMCEndDt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_End_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Under Observe" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUndObserv" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UndObserv") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Under Observe Eff From" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUndObservEffFrom" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UndObservEffFrom") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Under Observe Eff To" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUndObservEffTo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UndObservEffTo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Float Part" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFloatPart" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Float_flag") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Theft Vehicle" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehTheft" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Theft_flag") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Jb Kms" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLstJbKms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstJbKm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Jb Hrs" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLstJbHrs" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstJbHrs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Jb Hrs" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMod_Bas_Cat" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Mod_Cat_ID_Basic") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CRM HDR ID" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCRMHDRID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CRM_HDR_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CRM Ticket No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTicketNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Ticket_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CRM Ticket Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTicketDate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Ticket_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle Wrap="True" />
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <%-- Select Part --%>
    <div>
        <cc1:ModalPopupExtender ID="mpeSelectPart" runat="server" TargetControlID="lblTargetID_selectPart"
            PopupControlID="pnlPopUpWindow_SelectPart"
            BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
        <asp:Label ID="lblTargetID_selectPart" runat="server"></asp:Label>
        <asp:Panel ID="pnlPopUpWindow_SelectPart" runat="server" CssClass="modalPopup_SelectPart" Style="display: none;">
            <table class="PageTable table-responsive" border="1">
                <tr id="TitleOfPage2" class="panel-heading">
                    <td class="PageTitle panel-title" align="center" style="width: 14%">
                        <asp:Label ID="Label12" runat="server" Text="">
                        </asp:Label>
                    </td>
                </tr>
                <tr id="TblControl2">
                    <td style="width: 14%">

                        <div align="center" class="ContainTable">
                            <table style="background-color: #efefef;" width="70%">
                                <tr align="center">
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:DropDownList ID="DrpSelFrom" runat="server" CssClass="ComboBoxFixedSize">
                                            <asp:ListItem Selected="True" Value="P">Part Master</asp:ListItem>
                                            <asp:ListItem Value="E">From Estimate</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdLabel" style="width: 7%;">Search:
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:TextBox ID="txtSearch_Part" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    </td>

                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:DropDownList ID="DdlSelctionCriteria_part" runat="server" CssClass="ComboBoxFixedSize">
                                            <asp:ListItem Selected="True" Value="P">Part No</asp:ListItem>
                                            <asp:ListItem Value="N">Part Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdLabel" style="width: 10%;">
                                        <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                        <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search"
                                            OnClick="btnSave_Click" />
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td class="tdLabel" style="width: 10%;">
                                        <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search"
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
                        <asp:Panel ID="Panel3" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            class="ContaintTableHeader">
                            <asp:GridView ID="PartDetailsGrid_Part" runat="server" AlternatingRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%" CssClass="table table-condensed table-bordered"
                                AutoGenerateColumns="false" AllowPaging="True" PageSize="20">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClickOnJobcard(this);" />
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


                                    <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Billing Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Warranty Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrRate" runat="server" Text='<%# Eval("WarrRate","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Type Tag" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartTypeTag" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Part_Type_Tag")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartstock" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Stock","{0:#0.00}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EstID" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstDtlID" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("estDtlID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Group code" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartGroup" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("group_code")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax1" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax1")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax2" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax2")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Qty" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Qty")%>'></asp:Label>
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>--%>

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
                            <cc2:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" Width="1000px"
                                GenerateGoToSection="true" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr id="TmpControl2">
                    <td style="width: 15%">
                        <asp:TextBox ID="txtPartIds" runat="server" Width="1px"></asp:TextBox><%--CssClass="HideControl"--%>
               
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <%-- Select Labour --%>
    <cc1:ModalPopupExtender ID="mpeSelectLabour" runat="server" TargetControlID="lblTargetID_selectLabour"
        PopupControlID="pnlPopUpWindow_SelectLabour"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTargetID_selectLabour" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopUpWindow_SelectLabour" runat="server" CssClass="modalPopup_SelectLabour" Style="display: none;">
        <table class="PageTable table-responsive" border="1" width="100%">
                    <tr id="TitleOfPage3" class="panel-heading">
                        <td class="panel-title" align="center" style="width: 14%">
                            <asp:Label ID="Label13" runat="server" Text="Labour Master">
                            </asp:Label>
                            <asp:Label ID="lblMessage" runat="server" Text="" Visible="false">
                            </asp:Label>
                        </td>
                    </tr>
                    <%--style="background-color: #efefef;"--%>
                    <tr id="TblControl3">
                        <td style="width: 14%">
                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef;" width="100%">
                                    <tr align="center">
                                        <td>
                                            <asp:DropDownList ID="DrpLabGrp" runat="server" >
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel">Search:
                                        </td>
                                        <td class="tdLabel" >
                                            <asp:TextBox ID="txtSearch_Labour" runat="server" ></asp:TextBox>
                                        </td>
                                        <td class="tdLabel" >
                                            <asp:DropDownList ID="DdlSelctionCriteria_Labour" runat="server" CssClass="">
                                                <%--Sujata 27012011 Set Value For Labour Code(L) and Name(N)--%>
                                                <asp:ListItem Selected="True" Value="L">Labour Code</asp:ListItem>
                                                <asp:ListItem Value="N">Labour Name</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 25%;">
                                            <asp:DropDownList ID="DrpLabourSelect" runat="server" CssClass="">
                                                <%--Sujata 27012011 Set Value For Labour Code(L) and Name(N)--%>
                                               <%-- <asp:ListItem Selected="True" Value="D">Paid Labour</asp:ListItem>
                                                <asp:ListItem Value="C">Warranty Labour</asp:ListItem>
                                                <asp:ListItem Value="E">Estimated Labour</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 15%;">
                                            <asp:DropDownList ID="DrpSelFrom_Labour" runat="server" CssClass="ComboBoxFixedSize">
                                                <asp:ListItem Selected="True" Value="M">From Master</asp:ListItem>
                                                <asp:ListItem Value="E">From Estimate</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <asp:Button ID="btnSave_Labour" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                                 OnClick="btnSave_Labour_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="tdLabel" style="width: 10%;">
                                            <%-- Sujata 25012011 Add ReturnMultiWLabourDetails instead of ReturnMultiLabourDetails--%>
                                            <%--<asp:Label ID="lblBack" runat="server" Text="Back" onClick="return ReturnMultiWLabourDetails();"
                                    CssClass="CommandButton"></asp:Label>--%>
                                            <asp:Button ID="btnBack_Labour" runat="server" Text="OK" CssClass="btn btn-search btn-sm"
                                                 OnClick="btnBack_Labour_Click"></asp:Button>
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 110px;">
                            <asp:Panel ID="Panel4" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader" ScrollBars="Vertical" Height="500px">
                                <%-- Sujata 25012011 Add AllowPaging="True" PageSize="20" for Grid --%>
                                <asp:GridView ID="LabourDetailsGrid_Labour" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%" AutoGenerateColumns="false"
                                    AllowPaging="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <%-- Sujata 25012011 Add OnClick( code) --%>
                                                <asp:CheckBox ID="ChkLabour" OnClick="return ChkLabourClick(this);" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Code." ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabourCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LabourCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Name" ItemStyle-Width="80%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabourName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Labour_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Man Hrs." ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManHrs" runat="server" Text='<%# Eval("Man Hrs","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <%-- Sujata 02022011
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Labour_Rate") %>'></asp:Label>--%>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Labour_Rate","{0:#0.00}") %>'></asp:Label>
                                                <%--Sujata 02022011--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total" ,"{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LabourTag" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabTag" runat="server" Text='<%# Eval("Lab_Tag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group code" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartGroup" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("group_code")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax1" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax1")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax2" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax2")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="EstID" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstDtlID" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("EstDtlID")%>'></asp:Label>
                                            <asp:Label ID="lblMiscDesc" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("AddLbrDescriptionID")%>'></asp:Label>
                                            <asp:Label ID="lblOutSubDesc" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("out_lab_desc")%>'></asp:Label>                                            
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Wrap="True" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                                <cc2:PagerV2_8 ID="PagerV2_2" runat="server" OnCommand="PagerV2_2_Command" Width="1000px"
                                    GenerateGoToSection="true" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="TmpControl3" class="HideControl">
                        <td style="width: 15%">
                            <asp:TextBox ID="txtPartIds_Labour" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>

                        </td>
                    </tr>
                </table>
    </asp:Panel>
</asp:Content>
