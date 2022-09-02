<%@ page title="Proforma Invoice" language="C#" masterpagefile="~/Header.Master" autoeventwireup="true" codebehind="frmPartsProforma.aspx.cs" inherits="MANART.Forms.Spares.Export.frmPartsProforma" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/WebParts/Toolbar.ascx" tagname="Toolbar" tagprefix="uc1" %>
<%@ register src="~/WebParts/CurrentDate.ascx" tagname="CurrentDate" tagprefix="uc3" %>
<%@ register src="~/WebParts/SearchGridView.ascx" tagname="SearchGridView" tagprefix="uc4" %>
<%@ register src="~/WebParts/ExportLocation.ascx" tagprefix="uc2" tagname="ExportLocation" %>
<%@ register assembly="ASPnetPagerV2_8" namespace="ASPnetControls" tagprefix="cc2" %>
<%@ register src="~/WebParts/Location.ascx" tagprefix="uc5" tagname="Location" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../Content/style.css" rel="stylesheet" />
    <link href="../../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../../Scripts/jquery.datepick.js"></script>
    <link href="../../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../../Scripts/jsGridFunction.js"></script>
    <script src="../../../Scripts/jsShowForm.js"></script>
    <script src="../../../Scripts/jsValidationFunction.js"></script>
    <script src="../../../Scripts/jsMessageFunction.js"></script>
    <script src="../../../Scripts/jsToolbarFunction.js"></script>

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
    <script type="text/javascript">
        $(document).ready(function () {
            var txtProfDate = document.getElementById("ContentPlaceHolder1_txtProfDate_txtDocDate");
            $('#ContentPlaceHolder1_txtProfDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtProfDate.value == '') ? '0d' : txtProfDate.value, maxDate: (txtProfDate.value == '') ? '0d' : txtProfDate.value
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

        function CalculatePartTotal(event, ObjQtyControl) {
            //debugger;
            if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
                //ObjControl.focus(); 
                return;
            }
            else {
                var objID = $("#" + ObjQtyControl.id);
                var objRow = objID[0].parentNode.parentNode;
                var dAccQty = dGetValue(ObjQtyControl.value);
                var dProfQty = dGetValue(objRow.cells[4].children[0].value);
                if (dAccQty > dProfQty) {
                    alert("Accept Quantity Should not Greater than Proforma Quantity");
                    ObjQtyControl.value = 0;
                    ObjQtyControl.focus();
                    return;
                }
                var FOBRate = dGetValue(objRow.cells[6].children[0].value);
                var Total = dGetValue(ObjQtyControl.value) * FOBRate;
                if (isNaN(Total) == true) Total = 0;
                objRow.cells[7].children[0].value = parseFloat(Total).toFixed(2);

                CalculateGrandTotal()

            }
        }

        function CalculateGrandTotal() {
            //debugger;
            var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
            var txtAllTotal = document.getElementById("ContentPlaceHolder1_txtAllTotal");
            var txtTotalLineItem = document.getElementById("ContentPlaceHolder1_txtTotalLineItem");
            var objID = $("#ContentPlaceHolder1_PartGrid");
            var objGrid = objID[0];
            var qty, Rate;
            var TotalRate = 0.00;
            var totalQtypart = 0.00;
            var sPArtName = "";
            var iLineItemCount = 0;
            var bPartSel = false;
            var CountRow = objGrid.rows.length;

            for (var i = 0; i < CountRow; i++) {
                qty = objGrid.rows[i].cells[5].children[0].value;
                Rate = objGrid.rows[i].cells[6].children[0].value;
                sPArtName = objGrid.rows[i].cells[3].children[0].value;
                bPartSel = objGrid.rows[i].cells[9].children[0].children[0].checked;
                if (sPArtName != "" && bPartSel == false) {
                    TotalRate = dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
                    totalQtypart = dGetValue(totalQtypart) + dGetValue(qty);
                    var newTotal = parseFloat(qty * Rate).toFixed(2);
                    if (isNaN(newTotal) == true) newTotal = 0;
                    objGrid.rows[i].cells[7].children[0].value = newTotal;
                    iLineItemCount = iLineItemCount + 1;
                }
            }
            txtTotalLineItem.value = iLineItemCount;
            txtTotalQty.value = totalQtypart;
            txtAllTotal.value = parseFloat(TotalRate).toFixed(2);

        }

        function SelectDeleteCheckbox(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    CalculateGrandTotal();
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    CalculateGrandTotal();
                    return false;
                }
            }
            else {
                if (confirm("Are you sure you want to revert changes?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    CalculateGrandTotal();
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    ObjChkDelete.checked = false;
                    CalculateGrandTotal();
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

</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Proforma Invoice Acceptance"> </asp:Label>
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
                    <uc5:location runat="server" id="Location" />
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
                        SuppressPostBack="true" CollapsedText="Proforma Details" ExpandedText="Proforma Details"
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
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Supplier Name:
                                </td>
                                <td style="width: 25%">
                                    <asp:DropDownList ID="DrpSupplier" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblProfNo" runat="server" Text="Proforma No:"></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtProfNo" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblProfDate" runat="server" Text="Proforma Date:"></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <uc3:CurrentDate ID="txtProfDate" runat="server" bCheckforCurrentDate="false" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Proforma Invoice No:
                                </td>
                                <td style="width: 25%">
                                    <asp:DropDownList ID="DrpProfomaInvoice" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="DrpProfomaInvoice_SelectedIndexChanged" 
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMProfInvoice" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lbl" runat="server" Text="Proforma Invoice Date:"></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <uc3:CurrentDate ID="txtProfInvDate" runat="server" bCheckforCurrentDate="false" Enabled="false" />
                                </td>
                                 <td style="width: 15%; padding-left: 10px;" class="tdLabel HideControl">
                                    <asp:Label ID="Label2" runat="server" Text="RfP No:" ></asp:Label>
                                </td>
                                <td style="width: 15%;" class="HideControl">
                                    <asp:TextBox ID="txtRFPNo" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td class="tdLabel"  style="width: 15%; padding-left: 10px;">Remark:
                                </td>
                                <td style="width: 30%" colspan="2">
                                    <asp:TextBox ID="txtRemaks" Text="" runat="server" CssClass="TextBoxForString" MaxLength="100" TextMode="MultiLine"
                                        onblur="checkTextAreaMaxLength(this,event,'250');" Rows="2"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%" colspan="3">
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
                        <div style="border: 1px solid #084B8A; color: #ffffff; font-weight: bold;">
                            <table style="text-align: left; height: 38px; line-height: 17px; padding: 0px 4px; background-color: #70757A; border-right: solid 1px #9e9e9e; color: white;"
                                class="table table-condensed table-bordered">
                                <tr>
                                    <th style="width: 1%;">No</th>
                                    <th style="width: 10%;">Part No</th>
                                    <th style="width: 39%;">Part Name</th>
                                    <th style="width: 5%;">Prof Qty</th>
                                    <th style="width: 5%;">Acc Qty</th>
                                    <th style="width: 10%;">Rate</th>
                                    <th style="width: 10%;">Total</th>
                                    <th style="width: 10%;">RFP No</th>
                                    <th style="width: 25%;" class="HideControl">Delete</th>
                                </tr>
                            </table>
                        </div>
                        <div style="height: 300px; overflow: auto; background-color: #D4D4D4;">
                            <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                CssClass="table table-condensed table-bordered" ShowHeader="false"
                                AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" 
                                OnRowDataBound="PartGrid_RowDataBound">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" />
                                            <%--Text='<%# Container.DataItemIndex   %>'--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartID" runat="server" Width="1%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtRFPDetID" runat="server" Width="1%" Text='<%# Eval("RFP_det_ID") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtProfInvDetID" runat="server" Width="1%" Text='<%# Eval("ProfInv_Det_ID") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="txtGroupCode" runat="server" Text='<%# Eval("Group_Code") %>' Width="96%"
                                                CssClass="GridTextBoxForString HideControl" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="40%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prof Qty" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtProfQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Prof_Qty","{0:#0}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Qty" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Acc_Qty","{0:#0}") %>' MaxLength="6"
                                                Width="90%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <%--onblur="return CalculatePartTotal(event,this);"--%>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Rate","{0:#0.00}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'5');"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RFP No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRFP_No" runat="server" Text='<%# Eval("RFP_No") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="txtSAPOrderNo" runat="server" Text='<%# Eval("SAP_Order_No") %>' Width="96%"
                                                CssClass="GridTextBoxForString HideControl" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="25%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckbox(event,this);" />
                                            <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("DocStatus") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <table id="tblTotal" runat="server" class="ContainTable" style="background-color:#D4D4D4;" width="100%">
                        <tr>
                            <td style="width: 40%; text-align: right">
                                <b>Total:</b>
                            </td>
                            <td style="width: 11%; text-align: left">
                                 <asp:TextBox ID="txtTotalLineItem" runat="server" CssClass="TextForAmount" Font-Bold="true" tooltip="Total Line Item"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextForAmount" Font-Bold="true" tooltip="Total Accepted Quantity"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left">
                                <asp:TextBox ID="txtAllTotal" runat="server" CssClass="TextForAmount" Font-Bold="true"
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
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:content>
