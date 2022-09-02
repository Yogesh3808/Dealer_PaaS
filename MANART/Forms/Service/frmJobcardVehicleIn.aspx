<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableEventValidation="false" EnableViewState="true" CodeBehind="frmJobcardVehicleIn.aspx.cs" Inherits="MANART.Forms.Service.frmJobcardVehicleIn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%--<%@ Register Src="~/WebParts/ExportLocation.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%--<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>--%>
<%@ Register Src="~/WebParts/CurrentDateTime.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>

    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />

    <script type="text/javascript">
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
                SetChassisDetails(ArrOfChassis);
            }
            // For Hiding PopUp
            $find("mpe1").hide();
            return true;

        }
        //To Show Chassis Master
        function ShowChassisMaster(objNewModelLabel, sDealerId, sHOBR_ID) {
            // This Fucntion is not required
            var ChassisDetailsValues;

            ChassisDetailsValues = window.showModalDialog("../Common/frmChassisSelection.aspx?DealerID=" + sDealerId + "&HOBR_ID=" + sHOBR_ID + "&JobTypeID='0'", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            if (ChassisDetailsValues != null) {
                SetChassisDetails(ChassisDetailsValues);
            }

            return false;
        }

        //Function For Set Chassis Details
        function SetChassisDetails(ChassisDetailsValue) {
            var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
            var txtChassisNo = window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
            var txtVehicleNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleNo');
            var txtCustName = window.document.getElementById('ContentPlaceHolder1_txtCustName');
            var txtCustEdit = window.document.getElementById('ContentPlaceHolder1_txtCustEdit');

            var txtPreviousDocId = window.document.getElementById('ContentPlaceHolder1_txtPreviousDocId');
            var txtCRMTicketNo = window.document.getElementById('ContentPlaceHolder1_txtCRMTicketNo');
            var txtCRMTicketDate = window.document.getElementById('ContentPlaceHolder1_txtCRMTicketDate');



            //        var txtChassisNo = $('#' + 'ContentPlaceHolder1_txtChassisNo');
            //        var txtChassisID = $('#' + 'ContentPlaceHolder1_txtChassisID');
            //        var txtVehicleNo = $('#' + 'ContentPlaceHolder1_txtVehicleNo');
            //        var txtCustName = $('#' + 'ContentPlaceHolder1_txtCustName');
            //        var txtChassisNo = $('#' + 'ContentPlaceHolder1_txtCustEdit');

            //debugger;
            txtChassisID.value = ChassisDetailsValue[1];
            txtChassisNo.value = ChassisDetailsValue[2];
            txtVehicleNo.value = ChassisDetailsValue[3];
            txtCustName.value = ChassisDetailsValue[4];
            txtCustEdit.value = ChassisDetailsValue[8];
            txtPreviousDocId.value = ChassisDetailsValue[35];
            txtCRMTicketNo.value = ChassisDetailsValue[36];
            //alert(txtCRMTicketNo.value);
            txtCRMTicketDate.value = ChassisDetailsValue[37];

            if (txtCustEdit.value == "Y") {
                txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
            }
            else {
                txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
            }
        }
    </script>

    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
                var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
                $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick({
                    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
                });

                function customRange(dates) {
                    if (this.id == 'ContentPlaceHolder1_txtDocDate_txtDocDate') {
                        //$('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                    }
                    else {
                        //$('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }

                }
                // $('#ContentPlaceHolder1_txtDocDate_txtDocDate').val(Date.now());

            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                    <asp:Label ID="lblHighValueMsg" runat="server" Text="(  High Value Request Amount:  "
                        CssClass="HideControl"> </asp:Label>
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
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="txtDocNoDetails" runat="server" class="ContainTable table table-bordered" >
                            <%--<tr class="panel-heading">
                                <td align="center" class="panel-title" colspan="6">Document Details
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:LinkButton ID="lblSelectModel" runat="server" CssClass="LinkButton" Text="Select Chassis" Width="80%"
                                        ToolTip="Select Chassis Details" OnClick="lblSelectModel_Click"> </asp:LinkButton><%--OnClick="lblSelectModel_Click"--%>
                                </td>
                                <td style="width: 18%">&nbsp;</td>
                                <td style="width: 15%" class="">&nbsp;</td>
                                <td style="width: 18%"></td>
                                <td style="width: 15%" class=""></td>
                                <td style="width: 18%"></td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblManDocNo" runat="server" Text="Manual Inward No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtManDocNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
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
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" Mandatory="true" bTimeVisible="true" bCheckforCurrentDate="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblVehicleNo" runat="server" Text="Veh Reg No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="TextBoxForString" Text="" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="lblChassisNo" runat="server" Text="Chassis No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="" style="width: 15%">
                                    <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label1" runat="server" Text="Call Center Ticket No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCRMTicketNo" runat="server" CssClass="TextBoxForString" ></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">
                                    <asp:Label ID="Label2" runat="server" Text="Call Center Ticket Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCRMTicketDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="" style="width: 15%"></td>
                                <td style="width: 18%"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TblControl">
                <td>
                    <asp:Panel ID="PAccessoriesDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEAccessoriesDetails" runat="server" TargetControlID="CntAccessoriesDetails"
                            ExpandControlID="TtlAccessoriesDetails" CollapseControlID="TtlAccessoriesDetails" Collapsed="false"
                            ImageControlID="TtlAccessoriesDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Accessories Details" ExpandedText="Accessories Details"
                            TextLabelID="lblTtlAccessoriesDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlAccessoriesDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblTtlAccessoriesDetails" runat="server" Text="Accessories Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlAccessoriesDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntAccessoriesDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                            <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                CssClass="table table-condensed table-bordered"
                                Width="96%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False"
                                Height="16px">
                                <%--OnRowCommand="DetailsGrid_RowCommand"--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccessoryDtlID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accessory No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccessoryNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AccID") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accessory Name" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccessoryName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("accessory_name") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccessoryQty" runat="server" CssClass="TextForAmount" Text='<%# Eval("Qty","{0:#0.00}") %>'
                                                onkeypress=" return CheckForTextBoxValue(event,this,'6');"> </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TmpControl" style="display: none">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerCode" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtChassisID" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:TextBox ID="txtCustEdit" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
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
                                        <%--<asp:Button ID="btnOkChassis" runat="server" Text="OK" OnClick="btnOkChassis_Click" CssClass="btn btn-search" />--%>
                                    </td>
                                    <td  style="width: 15%; float:right;">                                
                                <asp:Button ID="btnok" runat="server" Text="OK"  
                                     CssClass="btn btn-search btn-sm"  />
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
                                    <asp:TemplateField HeaderText="Call Ticket No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTicketNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Ticket_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Call Ticket Date" ItemStyle-Width="10%">
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
</asp:Content>
