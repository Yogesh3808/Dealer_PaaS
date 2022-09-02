<%@ Page Title="MTI- Stock Transfer Challan" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false"
    CodeBehind="frmStockTransferChallan.aspx.cs" Inherits="MANART.Forms.Spares.frmStockTransferChallan" %>

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

        // Function Validation
        function Validation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_drpStkTrnChallanType").value == "0") {
                errMessage += "*Please Select Challan Type.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtStkTrnChNo").value == "") {
                errMessage += "*Please Do not Blank Challan No.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtStkTrnChDate_txtDocDate").value == "") {
                errMessage += "*Please Do not Blank Challan Date.\n";
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

        //To Part Master
        function ShowMultiPartSearch(objNewPartLabel, sDealerId, objCustTypeID) {

            if (Validation() == false) {
                return false;
            }
            var PartDetailsValue;
            var sDocType = "";

            var objhdnMenuID = document.getElementById("ContentPlaceHolder1_hdnMenuID");
            if (objhdnMenuID == null) sRefID = "";
            sDocType = objhdnMenuID.value;
            if (sDocType == "667")
                sDocType = "SockTranChallan";
            else
                sDocType = "WOChallan";

            var sSelectedPartID = GetPreviousSelectedChallanPartID(objNewPartLabel);
            var objCustType = document.getElementById("ContentPlaceHolder1_" + objCustTypeID);
            var sCustType = objCustType.options[objCustType.selectedIndex].text;
            var sCustID = objCustType[objCustType.selectedIndex].value;

            var sRefID = "";
            if (sDocType == "WOChallan") {
                var objJobID = document.getElementById("ContentPlaceHolder1_DrpReference");
                if (objJobID == null) return;
                if (objJobID.selectedIndex == 0) {
                    alert('Please Select Jobcard Reference !');
                    return false;
                }
                else {
                    sRefID = objJobID.options[objJobID.selectedIndex].value;
                }
            }
            var hdnIsDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
            var sDocGST = hdnIsDocGST.value;

            PartDetailsValue = window.showModalDialog("frmSelectMultiPartForChallan.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&TransFrom=" + sDocType + "&RefID=" + sRefID + "&sDocGST=" + sDocGST, "List", "dialogHeight: 550px; dialogWidth: 900px;");
            //PartDetailsValue = window.showModalDialog("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + bDistSuppl + "&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=StkTrnChaPart&TransFrom=SockTranChallan", "List", "dialogHeight: 550px; dialogWidth: 500px;");
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
            if (Validation() == false) {
                return false;
            }
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
            if (filename.search('_StkTransfer_PartDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }

        }

        function GetPreviousSelectedChallanPartID() {
            ////debugger;
            var objRow;
            var PartIds = "";
            var PartId = "";
            var txtPartId;
            var txtOADetId;
            var OADetId = "";
            // get grid object
            var objGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
            //var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
            if (objGrid == null) return PartIds;
            for (var i = 1; i <= objGrid.rows.length - 1 ; i++) {
                //Get Row
                objRow = objGrid.rows[i];

                //Get Part ID
                txtPartId = objGrid.rows[i].children[1].childNodes[1];

                // txtOADetId = objGrid.rows[i].children[3].childNodes[1];
                //Get PartId;
                // OADetId = (txtOADetId.value);

                //if (OADetId == "0" && OADetId == "" && OADetId == null) {
                //    OADetId = "0";
                //}

                // PartId = OADetId + "-" + (txtPartId.value);
                PartId = dGetValue(txtPartId.value);
                if (PartId != "0" && PartId != "" && PartId != null) {
                    PartIds = PartIds + PartId + ",";
                }
            }
            PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

            return PartIds;
        }
        function SelectDeletCheckboxAndCalcInv(ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    CalulateInvPartGranTotal();
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    CalulateInvPartGranTotal();
                    return false;
                }
            }
            else {
                ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                CalulateInvPartGranTotal();
            }
        }

        function CalculateStockTransfer(event, ObjQtyControl) {
            //debugger;
            if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
                return;
            }
            else {
                var sRefID = "";
                var objhdnMenuID = document.getElementById("ContentPlaceHolder1_hdnMenuID");
                if (objhdnMenuID == null) sRefID = "";
                var sDocType = objhdnMenuID.value;
                if (sDocType == "667")
                    sRefID = "ST";
                else
                    sRefID = "WO";

                var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

                var StockQty = dGetValue(objRow[5].childNodes[1].value);
                var ChallanQty = dGetValue(objRow[6].childNodes[1].value);
                var PrevChQty = dGetValue(objRow[6].childNodes[3].value);
                var MRPRate = dGetValue(objRow[7].childNodes[1].value);
                var Total = 0;
                var BFRGST = objRow[11].childNodes[1].value;
                var dBFRGST_Stock = dGetValue(objRow[12].childNodes[1].value);

                
                if ((dGetValue(ChallanQty) > (dGetValue(StockQty) + dGetValue(PrevChQty)) && sRefID == "ST" && BFRGST.trim() == "N"))//|| sRefID == "WO"
                {
                    alert("Cannot Enter Challan Qty Greater Than Part Stock Qty");
                    objRow[6].childNodes[1].value = 0;
                    objRow[6].childNodes[1].focus();
                    return false;
                }
                if ((dGetValue(ChallanQty) > (dGetValue(dBFRGST_Stock) + dGetValue(PrevChQty)) && sRefID == "ST" && BFRGST.trim() == "Y")) {//|| sRefID == "WO"
                    alert("Cannot Enter Challan Qty Greater Than Pre GST Part Stock Qty");
                    objRow[6].childNodes[1].value = 0;
                    objRow[6].childNodes[1].focus();
                    return false;
                }
                Total = ChallanQty * MRPRate;

                //if ((dGetValue(StockQty) + dGetValue(PrevChQty) >= dGetValue(ChallanQty) && sRefID == "ST") || sRefID == "WO") {
                //    Total = ChallanQty * MRPRate;
                //    //objRow[8].childNodes[1].value = Total.toFixed(2);
                //}
                //else {
                //    alert("Cannot Enter Challan Qty Greater Than Part Stock Qty");
                //    objRow[6].childNodes[1].value = 0;
                //    objRow[6].childNodes[1].focus();
                //    return false;
                //}
                //if (BFRGST.trim() == "Y") {
                //    if ((dGetValue(dBFRGST_Stock) + dGetValue(PrevChQty) >= dGetValue(ChallanQty) && sRefID == "ST" && BFRGST.trim() == "Y") || sRefID == "WO") {
                //        Total = ChallanQty * MRPRate;
                //        //objRow[8].childNodes[1].value = Total.toFixed(2);
                //    }
                //    else {
                //        alert("Cannot Enter Challan Qty Greater Than Pre GST Part Stock Qty");
                //        objRow[6].childNodes[1].value = 0;
                //        objRow[6].childNodes[1].focus();
                //        return false;
                //    }
                //}
                objRow[8].childNodes[1].value = Total.toFixed(2);
                CalulateStkTransferGranTotal();
            }
        }

        function CalulateStkTransferGranTotal() {
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
                    <%--<asp:Label runat ="server" ID="lblEGPDealer" Text ="EGP Dealer Name" class="tdLabel"></asp:Label>
                    <asp:DropDownList ID="ddlEGPDealer" runat="server" CssClass ="ComboBoxFixedSize" Width ="30%" ></asp:DropDownList>--%>
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
                        SuppressPostBack="true"
                        TextLabelID="lblTtlDocDetails">
                        <%--CollapsedText="Stock Transfer Details" ExpandedText="Stock Transfer Details"--%>
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
                                <td style="width: 10%" class="tdLabel">
                                    <asp:Label ID="lblStkTrnChType" runat="server" Text="Challan Type"></asp:Label>
                                </td>
                                <td style="width: 23%">
                                    <asp:DropDownList ID="drpStkTrnChallanType" runat="server"
                                        CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="drpStkTrnChallanType_SelectedIndexChanged">
                                        <%--<asp:ListItem Selected="True" Value="0">---Select---</asp:ListItem>
                                        <asp:ListItem Value="ST">Stock Transfer</asp:ListItem>
                                        <asp:ListItem Value="WO">Work Order</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMStkTrnChallanType" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblStkTrnChNo" runat="server" Text="Challan No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtStkTrnChNo" Text="" runat="server" CssClass="TextBoxForString NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblstkTrnChDate" runat="server" Text="Challan Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtStkTrnChDate" runat="server" bCheckforCurrentDate="false" Style="background: #CCC; color: #333;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 10%">
                                    <asp:Label ID="lblCustTitle" runat="server" Text=" Customer:"></asp:Label>
                                </td>
                                <td style="width: 23%">
                                    <asp:DropDownList ID="DrpCustomer" runat="server" AutoPostBack="true" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMCustomer" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblReference" runat="server" CssClass="" Text="Ref Jobcard(Sublet)"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="DrpReference" runat="server" AutoPostBack="true" CssClass="ComboBoxFixedSize"></asp:DropDownList>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblnarration" runat="server" Text="Narration"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtnarration" Text="" runat="server" CssClass="TextBoxForString" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trLast" runat="server">
                                <td class="tdLabel" style="width: 15%">
                                    <asp:Button ID="btnReceive" runat="server" CssClass="btn btn-search btn-sm" Text="Receipt" Width="125px" Height="25px" OnClick="btnReceive_Click" />
                                </td>
                                <td style="width: 18%"></td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblstkTrnRcpNo" runat="server" Text="Receipt No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtReceiptNo" Text="" runat="server" CssClass="TextBoxForString NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblstkTrnRcpDt" runat="server" Text="Receipt Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtReceiptDate" runat="server" bCheckforCurrentDate="false" Style="background: #CCC; color: #333;" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="UploafFile" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    Style="display: none">
                    <table id="Table1" runat="server" class=" ContainTable table table-bordered">
                        <tr>
                            <td style="width: 15%; padding-left: 10px;">Select File For Upload
                                
                            </td>
                            <td style="width: 65%; padding-left: 10px;">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="75%" CssClass="Cntrl1" />
                            </td>
                            <td style="width: 10%;">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-search " Text="Upload"
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
                                <a>Please select the file in the excel format. File name should be in format as 'DealerCode_StkTransfer_PartDetails_Datestamp'
                                    <br />
                                    e.g. 'D016514_StkTransfer_PartDetails_30082016'. </a>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 10px;">
                                <br />
                                <asp:Label ID="lblListPartNo" runat="server" Text="" Width="100%" ForeColor="#49A3D3"
                                    Visible="false"> </asp:Label>
                                <asp:TextBox ID="txtListPartNo" TextMode="MultiLine" runat="server" Text="" Width="100%"
                                    ForeColor="#49A3D3" Visible="false"></asp:TextBox>
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
                        <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered"
                            Width="100%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnPageIndexChanging="PartGrid_PageIndexChanging"
                            OnRowCommand="PartGrid_RowCommand" OnRowDataBound="PartGrid_RowDataBound">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" />
                                        <%--Text='<%# Container.DataItemIndex + 1  %>'--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartID" runat="server" Width="5%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtRefDtlID" runat="server" Width="5%" Text='<%# Eval("RefDtlID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("part_no") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <asp:LinkButton ID="lnkSelectPart" CssClass="btn btn-link" runat="server" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
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
                                <asp:TemplateField HeaderText="Part Stock" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("bal_qty","{0:#0.00}") %>'
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Challan Qty" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Width="90%" MaxLength="4"
                                            Text='<%# Eval("Qty","{0:#0.00}") %>' onblur="return CalculateStockTransfer(event,this);"></asp:TextBox>
                                        <asp:TextBox ID="txtPreviousInvQty" runat="server" CssClass="GridTextBoxForAmount HideControl"
                                            Width="90%" Text='<%# Eval("Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
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
                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxStkAdj(event,this);" />
                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                        <asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValue(event,this);" runat="server">New</asp:LinkButton>
                                        <asp:LinkButton ID="lblCancel" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            OnClientClick="return ClearRowValueForEGPPart(event,this);" runat="server" OnClick="lblCancel_Click">Cancel</asp:LinkButton>
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
                <asp:HiddenField ID="hdnChallanType" runat="server" />
                <asp:HiddenField ID="hdnReference" runat="server" />
                <asp:HiddenField ID="hdnMenuID" runat="server" />
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                <asp:HiddenField ID="hdnCancel" runat="server" Value="N" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
