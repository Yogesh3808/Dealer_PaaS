<%@ page title="Parts RFP" language="C#" masterpagefile="~/Header.Master" autoeventwireup="true" codebehind="frmPartsRFP.aspx.cs"
    inherits="MANART.Forms.Spares.Export.frmPartsRFP" maintainscrollpositiononpostback="true"
    enableeventvalidation="false" %>

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
    <link href="../../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../../Content/PaggerStyle.css" rel="stylesheet" />

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
            var txtRFPDate = document.getElementById("ContentPlaceHolder1_txtRFPDate_txtDocDate");
            $('#ContentPlaceHolder1_txtRFPDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtRFPDate.value == '') ? '0d' : txtRFPDate.value, maxDate: (txtRFPDate.value == '') ? '0d' : txtRFPDate.value
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

        // Validation Function
        function Validation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_DrpSupplier").value == "0") {
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

        //To Show Part Master
        function ShowMultiPartSearchForRFP(objNewPartLabel, sDealerId, sSupplierID) {
            if (Validation() == false) {
                return false;
            }
            var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            return true;
        }

        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }


        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            if (Validation() == false) {
                return false;
            }
            debugger;
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
            if (filename.search('_PartsRFP_PartDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }

        }

        function CalculateRFPPartTotal(event, ObjQtyControl) {
            debugger;
            if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
                //ObjControl.focus(); 
                return;
            }
            else {
                var objID = $("#" + ObjQtyControl.id);
                var objRow = objID[0].parentNode.parentNode;
                var MOQ = dGetValue(objRow.cells[4].children[0].value);
                var Qty = dGetValue(ObjQtyControl.value);

                if (MOQ != 0 && (Qty % MOQ) != 0) {
                    if (Qty / MOQ != 0) {
                        ObjQtyControl.value = (parseInt(Qty / MOQ) + 1) * MOQ
                    }
                }

                var FOBRate = dGetValue(objRow.cells[6].children[0].value);
                var Total = dGetValue(ObjQtyControl.value) * FOBRate;

                //objRow[7].childNodes[0].value = RoundupValue(Total);
                //objRow.cells[7].children[0].value = RoundupValue(Total);
                objRow.cells[7].children[0].value = parseFloat(Total).toFixed(2);
                CalulateRFPGranTotal()

            }
        }

        function CalulateRFPGranTotal() {
            var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
            var txtAllTotal = document.getElementById("ContentPlaceHolder1_txtAllTotal");
            var txtTotalLineItem = document.getElementById("ContentPlaceHolder1_txtTotalLineItem");
            var objID = $("#ContentPlaceHolder1_PartGrid");
            var objGrid = objID[0];
            var qty, Rate;
            var TotalRate = 0.00;
            var totalQtypart = 0.00;
            var sPArtName = "";
            var sStatus = "";
            var bPartSel = false;
            var iLineItemCount = 0;
            var CountRow = objGrid.rows.length;

            for (var i = 1; i < CountRow; i++) {
                qty = objGrid.rows[i].cells[5].children[0].value;
                Rate = objGrid.rows[i].cells[6].children[0].value;
                sPArtName = objGrid.rows[i].cells[3].children[0].value;
                sStatus = objGrid.rows[i].cells[9].children[0].value;
                bPartSel = objGrid.rows[i].cells[8].children[0].children[0].checked;
                if (sPArtName != "" && bPartSel == false) {
                    TotalRate = dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
                    totalQtypart = dGetValue(totalQtypart) + dGetValue(qty);
                    var newTotal = parseFloat(qty * Rate).toFixed(2);
                    objGrid.rows[i].cells[7].children[0].value = newTotal;
                    iLineItemCount = iLineItemCount + 1;

                }
            }
            txtTotalLineItem.value = iLineItemCount;
            txtTotalQty.value = totalQtypart;
            txtAllTotal.value = parseFloat(TotalRate).toFixed(2);

        }

        function SelectDeletCheckboxPO(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    CalulateRFPGranTotal();
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    CalulateRFPGranTotal();
                    return false;
                }
            }
            else {
                if (confirm("Are you sure you want to revert changes?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    CalulateRFPGranTotal();
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    ObjChkDelete.checked = false;
                    CalulateRFPGranTotal();
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

        //opener.ChkSpNDPPartClick(objImgControl);
        function ChkSpNDPPartClick(objImgControl) {
            debugger;
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
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
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
                    <%--<uc2:ExportLocation runat="server" ID="ExportLocation" />--%>
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
                        SuppressPostBack="true" 
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <%--CollapsedText="RFP Details" ExpandedText="RFP Details"--%>
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
                                    <asp:DropDownList ID="DrpSupplier" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="DrpSupplier_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblSupplierName" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblRFPNo" runat="server" ></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtRFPNo" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblRFPDate" runat="server" ></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <uc3:CurrentDate ID="txtRFPDate" runat="server" bCheckforCurrentDate="false" Enabled="false" />
                                    <asp:TextBox ID="txtCreatedBy" Text="" runat="server" CssClass="TextBoxForString"
                                        Enabled="false" Style="display: none"></asp:TextBox>
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
                                    <a>Please select the file in the excel format. File name should be in format as 'DealerCode_RFPDetails_Datestamp'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;D002500_PartsRFP_PartDetails_11072016&#39;. </a>
                                    <br />
                                    2.
                                   <a>Superceded Part Show in Aqua Color.</a>
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
                        <div style="border: 1px solid #084B8A; color: #ffffff; font-weight: bold;">
                            <table style="text-align: left; height: 38px; line-height: 17px; padding: 0px 4px; background-color: #70757A; border-right: solid 1px #9e9e9e; color: white;"
                                class="table table-condensed table-bordered">
                                <tr>
                                    <th style="width: 1%;">No </th>
                                    <th style="width: 10%;">Part No </th>
                                    <th style="width: 50%;">Part Name</th>
                                    <th style="width: 3%;">MOQ</th>
                                    <th style="width: 5%;">PO Qty</th>
                                    <th style="width: 7%;">Rate</th>
                                    <th style="width: 10%;">Total</th>
                                    <th style="width: 30%;">Delete</th>
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
                                    <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                       
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="96%" 
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:LinkButton ID="lnkSelectPart" runat="server" CssClass="btn btn-link" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="50%">
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
                                                Width="90%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateRFPPartTotal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Rate","{0:#0.00}") %>' onkeypress=" return CheckForTextBoxValue(event,this,'5');"></asp:TextBox>
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
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("DocStatus") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PartSupercded" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartSupercded" runat="server" Width="5%" Text='<%# Eval("Is_Superceded") %>'></asp:TextBox>
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
                            <td style="width: 55%; text-align: right">
                                <b>Total:</b>
                            </td>
                            <td style="width: 9%; text-align: left">
                                <asp:TextBox ID="txtTotalLineItem" runat="server" CssClass="TextForAmount" Font-Bold="true" 
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    ReadOnly="true" Width="40%"></asp:TextBox>
                            </td>
                            <td style="width: 22%; text-align: left">
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
                <asp:HiddenField ID="hdnPartsIDs" runat="server" value=""/>
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="" />
                 <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>

    <cc1:ModalPopupExtender ID="ModalPopUpExtender" runat="server" TargetControlID="lblTragetID" PopupControlID="pnlPopupWindow"
        OkControlID="btnOK" BackgroundCssClass="modalBackground_RFP">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup_RFP" Style="display: none;">
        <table class="PageTable table-responsive">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="PageTitle panel-title" align="center">
                    <asp:Label ID="Label1" runat="server" Text="Part Master">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl1">
                <td>
                    <div align="center" class="ContainTable">
                        <table class="table-bordered" style="background-color:#D4D4D4;">
                            <tr align="center">
                                <td class="tdLabel">Search:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtPopSearch" runat="server"></asp:TextBox>
                                </td>
                                <td class="tdLabel">
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server">
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
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClick(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FOBRate" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFOBRate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Rate","{0:#0.00}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                    HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMOQ" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("MOQ") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
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
                                <asp:TemplateField HeaderText="Group Code" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl" ItemStyle-Width="1%" HeaderStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGroupCode" runat="server" Text='<%# Eval("Group_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
</asp:content>
