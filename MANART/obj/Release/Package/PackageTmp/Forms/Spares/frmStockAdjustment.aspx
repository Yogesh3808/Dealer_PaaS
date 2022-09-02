<%@ Page Title="Parts Stock Adjustment" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false"
    Theme="SkinFile" CodeBehind="frmStockAdjustment.aspx.cs" Inherits="MANART.Forms.Spares.frmStockAdjustment" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="~/WebParts/ExportLocation.ascx" TagName="Location" TagPrefix="uc2" %>--%>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Location.ascx" TagPrefix="uc1" TagName="Location" %>

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
            var txtStockAdjDate = document.getElementById("ContentPlaceHolder1_txtStockAdjDate_txtDocDate");
            $('#ContentPlaceHolder1_txtStockAdjDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtStockAdjDate.value == '') ? '0d' : txtStockAdjDate.value, maxDate: (txtStockAdjDate.value == '') ? '0d' : txtStockAdjDate.value
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


        //To Show Part Master
        function ShowMultiPartSearchfromStockAdj(objNewPartLabel, sDealerId, ObjddlGstType) {
            //debugger;
            if (document.getElementById("ContentPlaceHolder1_ddlGST_Type").value == "0") {
                alert("*Please Select GST Type.\n");
                return false;
            }
            var PartDetailsValue;
            var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            var sSupplierID = "";
            var bDistSuppl;
            var objGSTType;
            objGSTType = document.getElementById("ContentPlaceHolder1_" + ObjddlGstType);
            var sGSTTypeID = objGSTType[objGSTType.selectedIndex].value;
            var sGSTType;
            if (sGSTTypeID == "" || sGSTTypeID == "0") {
                alert('Please select the GST Type.');
                return false;
            }
            if (sGSTTypeID == 3) {
                sGSTType = "O";
            }
            else {
                sGSTType = "N";
            }

            PartDetailsValue = window.showModalDialog("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=N&SupplierID=" + sSupplierID + "&SelectedPartID=" + sSelectedPartID + "&GSTType=" + sGSTType + "&SourchFrom=StkAdjPart&TransFrom=SockAdjustment", "List", "dialogHeight: 550px; dialogWidth: 700px;");
            return true;
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
            if (document.getElementById("ContentPlaceHolder1_ddlGST_Type").value == "0") {
                alert("*Please Select GST Type.\n");
                return false;
            }
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
            if (filename.search('StockAdj_PartDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }
        }
        function CalculateStockAdjustment(event, ObjQtyControl) {
            debugger;
            if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
                //ObjControl.focus(); 
                return;
            }
            else {

                var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

                var SysQty = dGetValue(objRow[5].childNodes[1].value);
                var PhysicalQty = dGetValue(objRow[6].childNodes[1].value);
                var InwQty = dGetValue(objRow[7].childNodes[1].value);
                var OutwQty = dGetValue(objRow[8].childNodes[1].value);
                var sBFRGST = objRow[12].childNodes[1].value;
                var dBFRGST_Stock = dGetValue(objRow[13].childNodes[1].value);
                var hdnIsAutoReceipt = document.getElementById("ContentPlaceHolder1_hdnGSTType");
                var ddlGST_Type = document.getElementById("ContentPlaceHolder1_ddlGST_Type");
                var errMessage = "";

                if (sBFRGST == "Y" && (ddlGST_Type.value == "4")) {//hdnIsAutoReceipt.value == "N" ||
                    if (dGetValue(PhysicalQty) < dGetValue(dBFRGST_Stock) && errMessage == "") {
                        errMessage += " Pre GST Stock is " + dBFRGST_Stock + ". " + "Physical Qty should be >= " + dBFRGST_Stock + "\n";
                    }
                }

                if (sBFRGST == "Y" && (ddlGST_Type.value == "3")) {//hdnIsAutoReceipt.value == "O" ||
                    if (dGetValue(PhysicalQty) > dGetValue(dBFRGST_Stock) && errMessage == "") {
                        errMessage += " Pre GST Stock is " + dBFRGST_Stock + ". " + "Physical Qty should be <= " + dBFRGST_Stock + "\n";
                    }
                    if (dGetValue(PhysicalQty) > dGetValue(SysQty) && errMessage == "") {
                        errMessage += "Post GST Stock is " + SysQty + ". " + "Physical Qty should be <= " + SysQty + "\n";
                    }
                }

                if (errMessage != "") {
                    alert(errMessage);
                    objRow[6].childNodes[1].value = 0.00;
                    objRow[7].childNodes[1].value = 0.00;
                    objRow[8].childNodes[1].value = 0.00;
                    objRow[6].childNodes[1].focus();
                    return false;
                }
                

                // If PreGST Stock Adjustment then calculation are as below
                if (ddlGST_Type.value == "3") {
                    objRow[7].childNodes[1].value = 0.00;// InwQty
                    if (dGetValue(SysQty) > dGetValue(dBFRGST_Stock)) {
                        objRow[8].childNodes[1].value = parseFloat(dGetValue(dBFRGST_Stock) - dGetValue(PhysicalQty));
                    }
                    if (dGetValue(dBFRGST_Stock) > dGetValue(SysQty)) {
                        objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                    }
                    if (dGetValue(dBFRGST_Stock) == dGetValue(SysQty)) {
                        objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                    }
                }
                if (ddlGST_Type.value == "4") {//4
                    if (dGetValue(SysQty) > dGetValue(dBFRGST_Stock)) {//A
                        if (dGetValue(PhysicalQty) >= dGetValue(dBFRGST_Stock) && dGetValue(SysQty) > dGetValue(PhysicalQty)) {//1
                            objRow[7].childNodes[1].value = 0.00;
                            objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                        }//1
                        else if (dGetValue(PhysicalQty) >= dGetValue(dBFRGST_Stock) && dGetValue(PhysicalQty) > dGetValue(SysQty)) {//2
                            objRow[7].childNodes[1].value = parseFloat(dGetValue(PhysicalQty) - dGetValue(SysQty))
                            objRow[8].childNodes[1].value = 0.00;
                        }//2
                    }//A
                    else if (dGetValue(dBFRGST_Stock) > dGetValue(SysQty)) {//B
                        if (dGetValue(PhysicalQty) >= dGetValue(dBFRGST_Stock) && dGetValue(SysQty) > dGetValue(PhysicalQty)) {//1
                            objRow[7].childNodes[1].value = 0.00;
                            objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                        }//1
                        else if (dGetValue(PhysicalQty) >= dGetValue(dBFRGST_Stock) && dGetValue(PhysicalQty) > dGetValue(SysQty)) {//2
                            objRow[7].childNodes[1].value = parseFloat(dGetValue(PhysicalQty) - dGetValue(SysQty))
                            objRow[8].childNodes[1].value = 0.00;
                        }//2

                    }//B
                    else if (dGetValue(dBFRGST_Stock) == dGetValue(SysQty)) {
                        if (dGetValue(PhysicalQty) >= dGetValue(dBFRGST_Stock) && dGetValue(SysQty) > dGetValue(PhysicalQty)) {//1
                            objRow[7].childNodes[1].value = 0.00;
                            objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                        }//1
                        else if (dGetValue(PhysicalQty) >= dGetValue(dBFRGST_Stock) && dGetValue(PhysicalQty) > dGetValue(SysQty)) {//2
                            objRow[7].childNodes[1].value = parseFloat(dGetValue(PhysicalQty) - dGetValue(SysQty))
                            objRow[8].childNodes[1].value = 0.00;
                        }//2
                    }
                    
                }//4

                //if (dGetValue(SysQty) > dGetValue(PhysicalQty) && ddlGST_Type.value == "4") {
                //    objRow[7].childNodes[1].value = 0;
                //    objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                //}
                //else if (dGetValue(PhysicalQty) > dGetValue(SysQty) && ddlGST_Type.value == "4") {
                //    objRow[7].childNodes[1].value = parseFloat(dGetValue(PhysicalQty) - dGetValue(SysQty))
                //    objRow[8].childNodes[1].value = 0;
                //}
                //else if (dGetValue(SysQty) == dGetValue(PhysicalQty) && ddlGST_Type.value == "4") {
                //    objRow[7].childNodes[1].value = parseFloat(dGetValue(PhysicalQty) - dGetValue(SysQty));
                //    objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                //}
                //else if (dGetValue(dBFRGST_Stock) > dGetValue(PhysicalQty) && ddlGST_Type.value == "3") {
                //    objRow[7].childNodes[1].value = 0;
                //    objRow[8].childNodes[1].value = parseFloat(dGetValue(SysQty) - dGetValue(PhysicalQty));
                //}
                //else if (dGetValue(dBFRGST_Stock) == dGetValue(PhysicalQty) && ddlGST_Type.value == "3") {
                //    objRow[7].childNodes[1].value = parseFloat(dGetValue(PhysicalQty) - dGetValue(dBFRGST_Stock));
                //    objRow[8].childNodes[1].value = parseFloat(dGetValue(dBFRGST_Stock) - dGetValue(PhysicalQty));
                //}


            }
        }

        function ClearRowValueForEGPPart(event, objCancelControl) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;
            var i = 1;

            //objCancelControl.style.display = "none";
            objCancelControl.innerHTML = "Cancelled"
            objCancelControl.disabled = "true"
            //Set PartId;
            //objRow[1].childNodes[0].value = '';

            //SetPartNo
            //objRow[2].children[0].value = "";
            //            objRow[2].children[0].style.display = "none";
            //            objRow[2].children[1].style.display = "";

            //SetPartName
            //objRow[3].childNodes[0].value = '';

            ////            //Stock MOQ
            ////            objRow[4].childNodes[0].value = '0';

            ////            //Physical Quantity
            ////            objRow[5].childNodes[0].value = '0';

            ////            //In ward Qty        
            ////            objRow[6].childNodes[0].value = '0';

            ////            //Out ward Qty
            ////            objRow[7].childNodes[0].value = '0';

            //Status
            objRow[10].childNodes[0].value = 'C';

            return false
        }
        function SelectDeleteRowValueForStkAdjPart(event, objCancelControl, objDeleteCheck) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;

            if (objDeleteCheck.checked == true) {
                objRow[10].childNodes[0].value = 'D';
            }
            else {
                objRow[10].childNodes[0].value = 'E';
            }
            return false
        }
        function SelectDeletCheckboxStkAdj(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    SelectDeleteRowValueForStkAdjPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForStkAdjPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
            else {
                ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                SelectDeleteRowValueForStkAdjPart(event, ObjChkDelete.parentNode, ObjChkDelete);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage " class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:label id="lblTitle" runat="server" text="Parts Stock Adjustment"> </asp:label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc1:toolbar id="ToolbarC" runat="server" onimage_click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:panel id="LocationDetails" runat="server" bordercolor="Black">
                    <%--<asp:Label runat ="server" ID="lblEGPDealer" Text ="EGP Dealer Name" class="tdLabel"></asp:Label>
                    <asp:DropDownList ID="ddlEGPDealer" runat="server" CssClass ="ComboBoxFixedSize" Width ="30%" ></asp:DropDownList>--%>
                    <%--<uc2:Location ID="Location" runat ="server"  />--%>
                    <uc1:Location runat="server" ID="Location" />
                </asp:panel>
                <asp:panel id="PSelectionGrid" runat="server" bordercolor="DarkGray" borderstyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />
                </asp:panel>
                <asp:panel id="PDocDetails" runat="server" bordercolor="DarkGray" borderstyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Stock Adjustment Details" ExpandedText="Stock Adjustment Details"
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
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblStockAdjNo" runat="server" Text="Stock Adjustment No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtStockAdjNo" Text="" runat="server" CssClass="TextBoxForString NonEditableFields" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblStockAdjDate" runat="server" Text="Stock Adjustment Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtStockAdjDate" runat="server" bCheckforCurrentDate="false" Style="background: #CCC; color: #333;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblReference" runat="server" Text="Reference"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtReference" Text="" runat="server" CssClass="TextBoxForString" MaxLength="100"
                                        TextMode="MultiLine"
                                        onblur="checkTextAreaMaxLength(this,event,'100');" Rows="2"></asp:TextBox>
                                </td>

                                 <td class="tdLabel" style="width: 15%">Parts Stock Type: 
                                </td>
                                <td style="width: 18%">
                                    
                                    <asp:DropDownList ID="ddlGST_Type" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true"  OnSelectedIndexChanged="ddlGST_Type_SelectedIndexChanged">
                                        <%--<asp:ListItem Selected="True" Value="N">Post GST</asp:ListItem>
                                        <asp:ListItem Value="O">Pre GST</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <%--<asp:CheckBox ID="ChkScheme" runat="server" Text="Scheme" />--%>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:panel>
                <asp:panel id="UploafFile" runat="server" bordercolor="DarkGray" borderstyle="Double"
                    style="display: none">
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
                                <a>Please select the file in the excel format. File name should be in format as 'DealerCode_StockAdj_PartDetails_Datestamp'
                                    <br />
                                    e.g. 'D002500_StockAdj_PartDetails_30082016'. </a>
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
                </asp:panel>
                <asp:panel id="PPartDetails" runat="server" bordercolor="DarkGray" borderstyle="Double"
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
                       <%-- <div style="border: 1px solid #084B8A; color: #ffffff; font-weight: bold;">
                            <table style="text-align: left; height: 38px; line-height: 17px; padding: 0px 4px; background-color: #70757A; border-right: solid 1px #9e9e9e; color: #333;"
                                class="table table-condensed table-bordered">
                                <tr>
                                    <th style="width: 1%;">No </th>
                                    <th style="width: 10%;">Part No </th>
                                    <th style="width: 30%;">Part Name</th>
                                    <th style="width: 4%;">Stock Qty</th>
                                    <th style="width: 4%;">Physical Qty</th>
                                    <th style="width: 4%;">Inward Qty</th>
                                    <th style="width: 4%;">Outward Qty</th>
                                    <th style="width: 20%;">Reason</th>
                                    <th style="width: 10%;">Delete</th>
                                    <th style="width: 5%;">Pre GST Stock</th>
                                </tr>
                            </table>
                        </div>--%>
                        <div style="height: 500px; overflow: auto; background-color: #D4D4D4;">
                            <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                                Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" ShowHeader="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" OnPageIndexChanging="PartGrid_PageIndexChanging"
                                OnRowCommand="PartGrid_RowCommand" OnRowDataBound="PartGrid_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" />
                                            <%--Text='<%# Container.DataItemIndex + 1  %>'--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part ID">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartID" runat="server" Width="5%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                            <asp:TextBox ID="txtGrNo" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("group_code") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="90%"
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
                                    <%--<asp:TemplateField HeaderText="MOQ" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMOQ" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("MOQ") %>'
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Stock Qty" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Qty","{0:#0.00}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Physical Qty" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <%--                                     <asp:TextBox ID="txtPhysicalQty" runat="server" CssClass="GridTextBoxForAmount" Width="90%" MaxLength ="4"
                                            Text='<%# Eval("Physical_Qty","{0:#0}") %>' onblur="return CalculateStockAdjustment(event,this);"></asp:TextBox></ItemTemplate>--%>
                                            <asp:TextBox ID="txtPhysicalQty" runat="server" CssClass="GridTextBoxForAmount" Width="90%" 
                                                Text='<%# Eval("Physical_Qty","{0:#0.00}") %>' onblur="return CalculateStockAdjustment(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inward Qty" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtInwQty" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Inward_Qty","{0:#0}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtInwQty" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Inward_Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outward Qty" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <%--                                        <asp:TextBox ID="txtOutwQty" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Outward_Qty","{0:#0}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>

                                            <asp:TextBox ID="txtOutwQty" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Outward_Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtreason" runat="server" CssClass="GridTextBoxForString" Width="90%" MaxLength="50"
                                                Text='<%# Eval("Reason") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxStkAdj(event,this);" />
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
                                    <asp:TemplateField HeaderText="BFRGST" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBFRGST" runat="server" Width="96%" Text='<%# Eval("BFRGST") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl"  />
                                        <ItemStyle CssClass="HideControl" width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pre GST Stock" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBFRGST_Stock" runat="server" Width="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                                Text='<%# Eval("BFRGST_Stock","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <table id="tblTotal" runat="server" class="ContainTable" width="100%" style="display: none">
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
                </asp:panel>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:textbox id="txtControlCount" cssclass="HideControl" runat="server" width="1px"
                    text=""></asp:textbox>
                <asp:textbox id="txtFormType" cssclass="HideControl" runat="server" width="1px" text=""></asp:textbox>
                <asp:textbox id="txtID" cssclass="HideControl" runat="server" width="1px" text=""></asp:textbox>
                <asp:hiddenfield id="hdnConfirm" runat="server" value="N" />
                <asp:textbox id="txtUserType" cssclass="HideControl" runat="server" width="1px" text=""></asp:textbox>
                <asp:hiddenfield id="hdnTrNo" runat="server" value="" />
                <asp:hiddenfield id="hdnGSTType" runat="server" value="N" />
            </td>
        </tr>
    </table>
</asp:Content>
