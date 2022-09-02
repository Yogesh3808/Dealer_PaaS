<%@ Page Title="dCAN-PRN" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmPurRetunNote.aspx.cs"
    Inherits="MANART.Forms.Spares.frmPurRetunNote" EnableEventValidation="false"
    Theme="SkinFile" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/ExportLocation.ascx" TagName="ExportLocation" TagPrefix="uc2" %>
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
    <script src="../../Scripts/jsParts_PRN.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtPurRetNoteDate = document.getElementById("ContentPlaceHolder1_txtPurRetNoteDate_txtDocDate");
            var txtInvDate = document.getElementById("ContentPlaceHolder1_txtInvDate_txtDocDate");
            $('#ContentPlaceHolder1_txtInvDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', maxDate: (txtInvDate.value == '') ? '0d' : txtPurRetNoteDate.value //this is working 
                //dateFormat: 'dd/mm/yyyy', maxDate: (txtInvDate.value == '') ? txtPurRetNoteDate.value : txtPurRetNoteDate.value
            });
            $('#ContentPlaceHolder1_txtPurRetNoteDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtPurRetNoteDate.value == '') ? '0d' : txtPurRetNoteDate.value, maxDate: (txtPurRetNoteDate.value == '') ? '0d' : txtPurRetNoteDate.value
            });

        });
    </script>
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
        function CommanValidationFunction(sDocName) {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName").value == "0") {
                errMessage += "*Please Select Supplier.\n";
            }
            if (errMessage != "") {
                alert(errMessage);
                return false;
            }

            // Purchase Returns Page
            if (sDocName == "PRN") {
                if (document.getElementById("ContentPlaceHolder1_ddlPurReturnType").value == "Y"
                    && document.getElementById("ContentPlaceHolder1_ddlInvoice").options[document.getElementById("ContentPlaceHolder1_ddlInvoice").selectedIndex].text == "--Select--") {
                    errMessage += "*Please Select Invoice No.\n";
                }
                if (document.getElementById("ContentPlaceHolder1_ddlPurReturnType").value == "N"
                    && document.getElementById("ContentPlaceHolder1_txtInvNo").value == "") {
                    errMessage += "*Please Enter Invoice No.\n";
                }
                if (document.getElementById("ContentPlaceHolder1_ddlPurReturnType").value == "N"
                   && document.getElementById("ContentPlaceHolder1_txtInvDate_txtDocDate").value == "") {
                    errMessage += "*Please Select Invoice Date.\n";
                }

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
        function ShowPartSearchForPRN(objNewPartLabel, sDealerId, sSupplierId, Is_AutoPRN, sInvoiceNo, iHOBrID, sIsOpenPRN) {
            debugger;
            if (CommanValidationFunction("PRN") == false) {
                return false;
            }
            var PartDetailsValue;
            var sSelectedPartID = "";
            var sSelectedPartID = GetPrevSelPartIDInvNo(objNewPartLabel);
            //var sSelPartIDInvNoPONo = GetPrevSelPartIDInvNoPONo(objNewPartLabel);
            if (sIsOpenPRN == "Y") {
                var txtPrevOpenPrnNo = document.getElementById("ContentPlaceHolder1_txtPrevOpenPrnNo").value;
                alert(" PRN No " + " " + txtPrevOpenPrnNo + " is open for this Supplier. Pl. confirm it first.");
                return false;
            }
            var hdnSupTaxTag = document.getElementById('ContentPlaceHolder1_hdnSupTaxTag');
            var sCustTaxTag = hdnSupTaxTag.value;
            var hdnIsDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
            var sDocGST = hdnIsDocGST.value;
            var ddlAppClaimRet = document.getElementById("ContentPlaceHolder1_ddlAppClaimRet");
            //var hdnAppClaimRet = document.getElementById('ContentPlaceHolder1_hdnAppClaimRet');
            var sIs_AppClaimRet = ddlAppClaimRet.value;
            PartDetailsValue = window.showModalDialog("frmSelMultiPartForPRN.aspx?DealerID=" + sDealerId + "&SupplierID=" + sSupplierId + "&SelectedPartID=" + sSelectedPartID + "&InvoiceNo=" + sInvoiceNo + "&Is_Auto=" + Is_AutoPRN + "&HOBrID=" + iHOBrID + "&sDocGST=" + sDocGST + "&CustTaxTag=" + sCustTaxTag + "&Is_AppClaimRet=" + sIs_AppClaimRet, "List", "dialogHeight: 560px; dialogWidth: 900px;");
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

        // To Get Part Id which are previously selected by user.
        function GetPrevSelPartIDInvNo(objNewPartLabel) {
            debugger;
            var objRow;
            var PartIds = "";
            var PartId = "";
            var InvNo = "";
            var lblPartId;
            var lblInvNo;
            var lblMR_Dts_ID;
            var MR_Dts_ID = "";

            // get grid object
            var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
            if (objGrid == null) return PartIds;

            for (var i = 2; i <= objGrid.rows.length - 1; i++) {
                //Get Row
                objRow = objGrid.rows[i];

                //Get Lable of Part ID
                lblPartId = objGrid.rows[i].children[1].childNodes[1];

                //Get Lable of InvNo 
                lblInvNo = objGrid.rows[i].children[4].childNodes[1];
                //Get Lable of MR_Dts_ID 
                lblMR_Dts_ID = objGrid.rows[i].children[7].childNodes[1];

                //Get PartId;
                if (lblPartId.value != "0" && lblPartId.value != "" && lblPartId.value != null) {
                    PartId = lblPartId.value;
                }

                if (lblInvNo.value != "0" && lblInvNo.value != "" && lblInvNo.value != null) {
                    InvNo = lblInvNo.value;
                }
                else {
                    InvNo = "0";
                }
                if (lblMR_Dts_ID.value != "0" && lblMR_Dts_ID.value != "" && lblMR_Dts_ID.value != null) {
                    MR_Dts_ID = lblMR_Dts_ID.value;
                }
                else {
                    MR_Dts_ID = "0";
                }

                //PartId = InvNo + "-" + (lblPartId.value); // New Change Invoice No replaced to Material Dts ID
                PartId = MR_Dts_ID + "-" + (PartId);
                if (PartId != "0" && PartId != "" && PartId != null) {
                    PartIds = PartIds + PartId + ",";
                }
            }
            PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

            return PartIds;
        }

        function SelectDeleteRowValueForPRNPart(event, objCancelControl, objDeleteCheck) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;

            if (objDeleteCheck.checked == true) {
                objRow[16].childNodes[0].value = 'D';
            }
            else {
                objRow[16].childNodes[0].value = 'E';
            }
            return false
        }


        function SelectDeletCheckboxAndCalcPRN(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    SelectDeleteRowValueForPRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForPRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
            else {
                ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                SelectDeleteRowValueForPRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
            }
        }

    </script>
    <%-- New Webmethod--%>
    <script type="text/javascript">
        function onTextChange(data) {
            PageMethods.GetInvoice(document.getElementById("<%=txtInvNo.ClientID%>").value, OnSuccess);
        }
        function OnSuccess(response, userContext, methodName) {
            if (response != 0) {
                alert('Invoice Number Already Exist!');
                document.getElementById("<%=txtInvNo.ClientID%>").value = "";
                document.getElementById("<%=txtInvNo.ClientID%>").style.border = "2px solid red";
            }
            else { document.getElementById("<%=txtInvNo.ClientID%>").style.border = "2px solid green"; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Purchase Return Note"> </asp:Label>
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
                    <uc2:ExportLocation ID="ExportLocation" runat="server" />
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
                        SuppressPostBack="true" CollapsedText="Purchase Return Note Details" ExpandedText="Purchase Return Note Details"
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
                                    <asp:Label ID="lblPurReturnType" runat="server" Text="Purchase Return Type:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlPurReturnType" runat="server"
                                        CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPurReturnType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="Y">Auto</asp:ListItem>
                                        <asp:ListItem Value="N">Manual</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMPurReturnType" runat="server" Text=" *" CssClass="Mandatory" Visible="false"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblPurRetNoteNo" runat="server" Text="Purchase Note No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPurRetNoteNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblPurRetNoteDate" runat="server" Text="Purchase Note Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtPurRetNoteDate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblInvNo" runat="server" Text="Invoice No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlInvoice" runat="server" AutoPostBack="true"
                                        CssClass="ComboBoxFixedSize "
                                        OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged"
                                        Style="display: none">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtInvNo" Text="" runat="server" MaxLength="20" CssClass="TextBoxForString" onchange="onTextChange(this)"></asp:TextBox>
                                    <asp:Label ID="lblMInvoiceNo" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtInvDate" runat="server" bCheckforCurrentDate="false" ReadOnly="true" Mandatory="false" />
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblReference" runat="server" Text="Reference"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtReference" Text="" runat="server" CssClass="TextBoxForString" MaxLength="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trGRNDetails" runat="server">
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblGrnNo" runat="server" Text="GRN No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtGrnNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblGrnDate" runat="server" Text="GRN Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtGrnDate" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:Label ID="lblDeliveryNo" runat="server" Text="Delivery No:" CssClass=""></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDeliveryNo" Text="" runat="server" MaxLength="20" CssClass="TextBoxForString " ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtCreatedBy" Text="" runat="server" CssClass="TextBoxForString HideControl"
                                        Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <%-- HideControl style="width: 15%" --%>
                                <td class="tdLabel " colspan="4">
                                    <%--<asp:Label ID="lblPerOrAmt" runat="server" Text="Please Select"></asp:Label>--%>
                                    <%--</td>
                                <td style="width: 18%">--%>
                                    <div>
                                        <asp:RadioButtonList ID="rbtLstDiscount" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="rbtLstDiscount_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" RepeatLayout="Table">
                                            <asp:ListItem Text=" Discount In Percent" Value="Per"></asp:ListItem>
                                            <asp:ListItem Text="Discount In Amount" Value="Amt"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr id="trAppClaimRet" runat="server" ><%--class="HideControl"--%>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="Label1" runat="server" Text="Against Discrepancy Claim "></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlAppClaimRet" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="ddlAppClaimRet_SelectedIndexChanged">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr class="HideControl">
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblMode" runat="server" Text="Mode"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlMode" runat="server" CssClass="ComboBoxFixedSize " Enabled="false">
                                        <%-- <asp:ListItem Selected ="True" Value ="0">--Select--</asp:ListItem>--%>
                                        <asp:ListItem Value="Y">Auto</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="True">Manual</asp:ListItem>
                                    </asp:DropDownList>
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
                        <div class="scrolling-table-container" style="height: 200px; background-color: #D4D4D4;">
                            <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="110%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" OnPageIndexChanging="PartGrid_PageIndexChanging"
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
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part No." ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:LinkButton ID="lnkSelectPart" runat="server" CssClass="btn btn-link" OnClick="lnkSelectPart_Click">Select Part</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <%--<HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />--%>
                                        <%--<ItemStyle Width="1%" />--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Qty" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Invoice_Qty","{0:#0.00}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Return Qty" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReturnQty" runat="server" CssClass="GridTextBoxForAmount" Width="90%" MaxLength="7"
                                                Text='<%# Eval("Ret_Qty","{0:#0.00}") %>' onblur="return CalculatePRNPartTotal(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="TxtPrevRetQty" runat="server" CssClass="GridTextBoxForAmount HideControl" Width="90%" MaxLength="7"
                                                Text='<%# Eval("Ret_Qty","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MR_Dts_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMR_Dts_ID" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("MR_Dts_ID","{0:#0}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnit" runat="server" Text='<%# Eval("Unit") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MRP" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("Price","{0:#0.00}") %>' Width="96%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disc(Per)" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiscPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Disc_Per","{0:#0.00}") %>'
                                                Width="90%"></asp:TextBox>
                                            <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Rate" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Accept_Rate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                            <asp:TextBox ID="txtExclDiscountRate" runat="server" CssClass="HideControl" Width="96%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="TxtExclTotal" runat="server" CssClass="HideControl" Width="80%"
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PO No" ItemStyle-Width="2%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPONo" runat="server" Text='<%# Eval("PO_No") %>' Width="90%"
                                                CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Qty" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOrgStkQty" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Stock_Qty","{0:#0.00}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
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
                                        </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeletCheckboxAndCalcPRN(event,this);" />
                                            <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatus" runat="server" Width="5%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part TaxPerGroupCode" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTaxPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                Text='<%# Eval("Tax_Per") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:TextBox ID="txtGroupCode" runat="server" Width="80%" Text='<%# Eval("group_code") %>'
                                                onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="HideControl" />
                                        <ItemStyle CssClass="HideControl" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>

                    <table id="tblTotal" runat="server" class="ContainTable table table-bordered" style="background-color: #D4D4D4;" width="100%">
                        <tr>
                            <td style="width: 13%; text-align: right">
                                <b>Total:</b>
                            </td>
                            <td style="width: 12%; text-align: left">
                                <asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    ReadOnly="true" Width="20%"></asp:TextBox>
                            </td>
                            <td style="width: 8%; text-align: left">
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="TextForAmount" Font-Bold="true"
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
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="false" PageSize="5">
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
                                <asp:TemplateField HeaderText="Net Receipt Amt" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtGrnetrevamt" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount " Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateReceivePartGranTotal();"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
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
                                        <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
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
                                        <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                        </asp:DropDownList>
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
                        SuppressPostBack="true" CollapsedText="Receipt Tax Details" ExpandedText="Receipt Tax Details"
                        TextLabelID="lblTtlTaxDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlTaxDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Receipt Tax Details" Width="96%"
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
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5">
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
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="7%">
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
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
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
                                <%--<asp:TemplateField HeaderText="Excise Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrExciseAmt" runat="server" Text='<%# Eval("Excise_Amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Insu Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrInsuAmt" runat="server" Text='<%# Eval("Insu_Amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>--%>
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
                                <asp:TemplateField HeaderText="PF Charges%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                            Width="90%"></asp:TextBox>
                                        <%--onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalulateReceivePartGranTotal();"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PF Charges Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalulateReceivePartGranTotal();"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_money","{0:#0.00}")%>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
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
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnDealerID" runat="server" Value="0" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnIsAutoPRN" runat="server" Value="Y" />
                <asp:HiddenField ID="hdnSupTaxTag" runat="server" />
                <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="" />
                <asp:HiddenField ID="hdnSupplierType" runat="server" Value="" />
                <asp:HiddenField ID="hdnIsOpenPRN" runat="server" Value="N" />
                <asp:TextBox ID="txtPrevOpenPrnNo" CssClass="HideControl" runat="server" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                <asp:HiddenField ID="hdnAppClaimRet" runat="server" Value="N" />
                <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
