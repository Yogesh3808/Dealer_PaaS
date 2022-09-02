<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmCouponClaim.aspx.cs" Inherits="MANART.Forms.Warranty.frmCouponClaim" %>

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
    <script src="../../Scripts/jsFileAttach.js"></script>
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

            $('#ContentPlaceHolder1_txtLRDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
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
            debugger;
            var FPDADetails = null;
            //var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
            //var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");           
            var drpClaimType = window.document.getElementById('ContentPlaceHolder1_drpClaimType');
            if (drpClaimType == null) return;
            sClaimtype = drpClaimType.options[drpClaimType.selectedIndex].value;
            FPDADetails = window.showModalDialog("../Warranty/frmFPDASelection.aspx?DealerID=" + sDealerId + "&PageName=CouponClaim&ClaimType=" + sClaimtype, "List", "dialogWidth:700px;dialogHeight:500px;status:no;help:no");
            if (FPDADetails != null) {
                debugger;
                var hdnJobcardID = window.document.getElementById('ContentPlaceHolder1_hdnJobcardID');                
                hdnJobcardID.value = FPDADetails[3];
            }
            return true;
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
            //Sujata 03122012_Begin
            //if (TotHedearBox.value == "" ) {
            if (TotHedearBox.value == "" || TotHedearBox.value == "0") {
                //Sujata 03122012_End
                alert("Please Enter The No Of Boxes In The Header.");
                TotHedearBox.focus();
                return false;
            }
            //            if (ObjGrid == null) return;
            var objtxtControl;
            var iCountDel = 0;
            var ObjControl;
            var bCheckValidation = true;
            var sMessage = "";
            var ObjRow = Obj.parentNode.parentNode.childNodes;

            TotDetailsBox = ObjRow[7].childNodes[1].value;

            if (dGetValue(TotHedearBox.value) < TotDetailsBox) {
                alert("Box No Can Not Be Greator Than No Of Boxes In The Header.");
                Obj.focus();
                return false;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title" Text=""> </asp:Label>
                <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
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
                        <table id="tblRFPDetails" runat="server" class="ContainTable ">
                            <tr>
                                <td class="tdLabel"  style="text-align: left">
                                    <asp:LinkButton ID="lblSelectWarrantyClaim" runat="server" CssClass="btn btn-link"
                                        Text="Select Jobcard" ToolTip="Select Warranty Claims" Width="77%" OnClick="lblSelectWarrantyClaim_Click"></asp:LinkButton>
                                </td>
                                <td class="tdLabel" colspan="5" style="text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%; height: 15px;">Claim Type:
                                </td>
                                <td style="width: 18%; height: 15px;">
                                    <asp:DropDownList ID="drpClaimType" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpClaimType_SelectedIndexChanged">
                                        <asp:ListItem Text="PDI" Value="P" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%; height: 15px;" class="tdLabel">
                                    <asp:Label ID="lblDocNo" runat="server" Text="Doc No"></asp:Label>
                                </td>
                                <td style="width: 18%; height: 15px;">
                                    <asp:TextBox ID="txtDocNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        MaxLength="12"></asp:TextBox>
                                </td>
                                <td style="width: 15%; height: 15px;" class="tdLabel">
                                    <asp:Label ID="lblDocDate" runat="server" Text="Doc Date"></asp:Label>
                                </td>
                                <td style="width: 18%; height: 15px;">
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%;">Customer:
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString" MaxLength="50"
                                        ReadOnly="false" Text=""></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                                <td class="tdLabel" style="width: 15%">Remarks:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxForString" TextMode="SingleLine"
                                        Height="80%" Text="" Width="90%"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Claim Amount:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtClmAmt" runat="server" CssClass="TextForAmount"
                                        Height="80%" Text="" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                 <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblApprovalNo" runat="server" Text="Approval Inv No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblApprovalDt" runat="server" Text="Approval Inv Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprDt" Font-Bold="true" runat="server" CssClass="TextBoxForString"
                                     ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                </td>
                                <td style="width: 18%">
                                    
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
                        SuppressPostBack="true" CollapsedText="Jobcard Coupon Details" ExpandedText="Jobcard Coupon Details"
                        TextLabelID="lblTtlModelDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlModelDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlModelDetails" runat="server" Text="Jobcard Coupon Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
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
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="JobCardID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtJobcardHDRID" runat="server" Text='<%# Eval("Jobcard_HDR_ID") %>' Width="1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="JobCard No" ItemStyle-Width="17%">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="txtJobcardNo" runat="server" Text='<%# Eval("JobcardNo") %>' Width="80%"></asp:TextBox>--%>
                                        <asp:Label ID="lblJobcardNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("JobcardNo") %>'> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jobdate" ItemStyle-Width="17%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServDt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Service_Dt") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModelCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChassis" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("chassis_no") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEngNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("engine_no") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kms" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblKms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Kms","{0:#0}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Hrs" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHrs" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Hrs","{0:#0}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Coupon No" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCouponNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("coupon_no") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCouponAmt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total_Amt","{0:#0.00}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>                             
                                <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>                                            
                                            <asp:TextBox ID="txtLTax" runat="server"  Width="98%" CssClass="GridTextBoxForString" Text='<%# Eval("Tax","{0:#0.00}") %>'></asp:TextBox>                                           
                                            <asp:TextBox ID="txtLTax1" runat="server" Width="98%" CssClass="GridTextBoxForString" Text='<%# Eval("Tax1","{0:#0.00}") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtLTax2" runat="server" Width="98%" CssClass="GridTextBoxForString" Text='<%# Eval("Tax2","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLTotTaxAmt" runat="server" Width="98%" CssClass="GridTextBoxForString" Text='<%# Eval("TaxAmt","{0:#0.00}") %>'></asp:TextBox>                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount With Tax" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCouponAmtWtTax" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("TotAmtWtTax","{0:#0.00}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" Text="Delete" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Box No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBoxNo" runat="server" CssClass="TextForAmount" onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);"
                                                Text='<%# Eval("Box_no") %>' onblur="return BoxDtlsValidation(this);"> </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accept (Y/N)" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForAccept" runat="server" Checked='<%# Eval("ChkForAccept") %>'
                                              onclick="return AcceptValidation(this);"
                                                 />                                                  
                                        </ItemTemplate>                                       
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="Rejection Remark">
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
                                    </asp:TemplateField>--%>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
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
                <asp:HiddenField ID="hdnCustID" runat="server" Value="" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                <asp:HiddenField ID="hdnJobcardID" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="" />
                <asp:HiddenField ID="hdnISDocGST" runat="server" />   
                <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
            </td>
        </tr>
    </table>
</asp:Content>
