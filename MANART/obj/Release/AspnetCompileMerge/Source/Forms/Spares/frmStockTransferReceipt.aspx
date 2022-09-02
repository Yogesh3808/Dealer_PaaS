<%@ Page Title="Stock Transfer Receipt" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
     CodeBehind="frmStockTransferReceipt.aspx.cs" Inherits="MANART.Forms.Spares.frmStockTransferReceipt" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
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
    <script src="../../Scripts/jsRFPFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtInvDate = document.getElementById("ContentPlaceHolder1_txtStkTrnChDate_txtDocDate");
            $('#ContentPlaceHolder1_txtStkTrnChDate_txtDocDate').datepick({
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

        function SelectDeleteRowValueForStkReceiptPart(event, objCancelControl, objDeleteCheck) {
            //  //debugger;
            var objRow = objCancelControl.parentNode.parentNode.childNodes;

            if (objDeleteCheck.checked == true) {
                objRow[10].childNodes[1].value = 'D';
            }
            else {
                objRow[10].childNodes[1].value = 'U';
            }
            CalulateStkTranReceiptGranTotal();
            return false
        }
        function SelectDeleteCheckboxStkReceipt(event, ObjChkDelete) {
            // //debugger;
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    SelectDeleteRowValueForStkReceiptPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForStkReceiptPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
            else {
                if (confirm("Are you sure you want to revert changes?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    SelectDeleteRowValueForStkReceiptPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForStkReceiptPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
        }
        function CalculateStkTranReceipt(event, ObjQtyControl) {
            if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
                return;
            }
            else {
                ////debugger;
                var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

                var ChallanQty = dGetValue(objRow[5].childNodes[1].value);
                var RecvQty = dGetValue(objRow[6].childNodes[1].value);
                var MRPRate = dGetValue(objRow[7].childNodes[1].value);
                var Total = 0;

                if (dGetValue(ChallanQty) >= dGetValue(RecvQty)) {
                    Total = RecvQty * MRPRate;
                    //objRow[8].childNodes[1].value = Total.toFixed(2);
                }
                else {
                    alert("Cannot Enter Receive Qty Greater Than Challan Qty");
                    objRow[6].childNodes[1].value = 0;
                    objRow[8].childNodes[1].value = 0.00;
                    objRow[6].childNodes[1].focus();
                    CalulateStkTranReceiptGranTotal();
                    return false;
                }
                objRow[8].childNodes[1].value = Total.toFixed(2);
                CalulateStkTranReceiptGranTotal();
            }
        }
        function CalulateStkTranReceiptGranTotal() {
            ////debugger;
            var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
            var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");
            var objID = $("#ContentPlaceHolder1_PartGrid");
            var objGrid = objID[0];
            var qty, Rate;
            var TotalRate = 0;
            var totalQtypart = 0;
            var sPArtName = "";
            var sStatus = "";
            var CountRow = objGrid.rows.length;

            for (var i = 1; i < CountRow; i++) {
                qty = objGrid.rows[i].cells[5].children[0].value;
                Rate = objGrid.rows[i].cells[6].children[0].value;
                sPArtName = objGrid.rows[i].cells[3].children[0].value;
                sStatus = objGrid.rows[i].cells[9].children[0].value;
                if (sPArtName != "" && sStatus != "C" && sStatus != "D") {
                    TotalRate = dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
                    totalQtypart = dGetValue(totalQtypart) + dGetValue(qty);
                }
            }
            txtTotalQty.value = totalQtypart;
            txtTotal.value = parseFloat(TotalRate).toFixed(2);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage " class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
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
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black">
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
                        SuppressPostBack="true" CollapsedText="Stock Receipt Details" ExpandedText="Stock Receipt Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="" Width="96%"
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
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblReceiptNo" runat="server" Text="Receipt No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtReceiptNo" Text="" runat="server" CssClass="TextBoxForString NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblChallanNo" runat="server" Text="Challan No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpChallanNo" runat="server" OnSelectedIndexChanged="drpChallanNo_SelectedIndexChanged"
                                        CssClass="ComboBoxFixedSize" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMReceiptType" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblReceiptDate" runat="server" Text="Receipt Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtReceiptDate" runat="server" bCheckforCurrentDate="false" Style="background: #CCC; color: #333;" />
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
                        <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered"
                            Width="100%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnPageIndexChanging="PartGrid_PageIndexChanging"
                            OnRowCommand="PartGrid_RowCommand">
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
                                <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartID" runat="server" Width="5%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("part_no") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--<asp:LinkButton ID="lnkSelectPart" CssClass="btn btn-link" runat="server" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Part Stock" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("bal_qty","{0:#0}") %>'
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Challan Qty" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Width="90%" MaxLength="4"
                                            Text='<%# Eval("Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Receive Qty" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRecvQty" runat="server" CssClass="GridTextBoxForAmount" Width="90%" MaxLength="4"
                                            Text='<%# Eval("Recv_Qty","{0:#0.00}") %>' onblur="return CalculateStkTranReceipt(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("MRPRate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxStkReceipt(event,this);" />
                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStatus" runat="server" Text='<%# Eval("Status") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BFR GST" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBFRGST" runat="server" Text='<%# Eval("BFRGST") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
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
                <asp:HiddenField ID="hdnChallanNo" runat="server" />
                <asp:HiddenField ID="hdnReference" runat="server" />
                <asp:HiddenField ID="hdnMenuID" runat="server" />
                <asp:HiddenField ID="hdnISDocGST" runat="server" />   
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
