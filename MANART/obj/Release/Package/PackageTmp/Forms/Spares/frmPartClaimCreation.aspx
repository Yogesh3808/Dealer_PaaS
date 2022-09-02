<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" CodeBehind="frmPartClaimCreation.aspx.cs" Inherits="MANART.Forms.Spares.frmPartClaimCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/ExportLocation.ascx" TagPrefix="uc2" TagName="ExportLocation" %>
<%@ Register Src="~/WebParts/Location.ascx" TagPrefix="uc1" TagName="Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />

    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsEstimateFunction.js"></script>
    <%--For file Attachment--%>
    <script src="../../Scripts/jsFileAttach.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var txtMReceiptDate = document.getElementById("ContentPlaceHolder1_txtValidityDate_txtDocDate");
            //var txtDMSInvDate = document.getElementById("ContentPlaceHolder1_txtDMSInvDate_txtDocDate");

            //$('#ContentPlaceHolder1_txtValidityDate_txtDocDate').datepick({
            //    dateFormat: 'dd/mm/yyyy', minDate: (txtValidityDate.value == '') ? '0d' : txtValidityDate.value, maxDate: (txtValidityDate.value == '') ? '0d' : txtValidityDate.value
            //});
            //$('#ContentPlaceHolder1_txtDMSInvDate_txtDocDate').datepick({
            //    dateFormat: 'dd/mm/yyyy', maxDate: (txtDMSInvDate.value == '') ? '0d' : txtDMSInvDate.value
            //});
        });
    </script>
    <%-- <style type="text/css">
        .checkboxlist input {
        font: inherit;
        font-size: 0.875em; /* 14px / 16px */
        color: #494949;
        float:left;
        margin-top:2px;
        margin-bottom:18px;
	}
	.checkboxlist label {
        font: inherit;
        font-size: 0.875em; /* 14px / 16px */
        color: #494949;
        position:relative;
        margin-top:2px;
        display:block;
	}
	</style>--%>
    <style type="text/css">
        .checkbox {
            padding-left: 20px;
        }

            .checkbox label {
                display: inline-block;
                vertical-align: middle;
                text-align: left;
                position: relative;
                padding-left: 5px;
                width: auto;
            }

                .checkbox label::before {
                    content: "";
                    display: inline-block;
                    position: absolute;
                    width: 17px;
                    height: 17px;
                    left: 0;
                    margin-left: -20px;
                    border: 1px solid #cccccc;
                    border-radius: 3px;
                    background-color: #fff;
                    -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                }

                .checkbox label::after {
                    display: inline-block;
                    position: absolute;
                    width: 16px;
                    height: 16px;
                    left: 0;
                    top: 0;
                    margin-left: -20px;
                    padding-left: 3px;
                    padding-top: 1px;
                    font-size: 11px;
                    color: #555555;
                }

            .checkbox input[type="checkbox"] {
                opacity: 0;
                z-index: 1;
            }

                .checkbox input[type="checkbox"]:checked + label::after {
                    font-family: "FontAwesome";
                    content: "\f00c";
                }

        .checkbox-primary input[type="checkbox"]:checked + label::before {
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .checkbox-primary input[type="checkbox"]:checked + label::after {
            color: #fff;
        }
    </style>
    <style>
        .table table > tbody > tr > td > span {
            z-index: 3;
            color: #fff;
            cursor: default;
            /*background-color: #337ab7;*/
            border-color: #337ab7;
        }

        .table table tbody tr td a, .table table tbody tr td span {
            position: relative;
            float: left;
            /*padding: 3px 12px;*/
            margin-left: -1px;
            line-height: 1.42857143;
            color: #337ab7;
            text-decoration: none;
            background-color: #fff;
            border: 1px solid #ddd;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var txtValidDate = document.getElementById("ContentPlaceHolder1_txtValidityDate_txtDocDate");
            $('#ContentPlaceHolder1_txtValidityDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtValidDate.value == '') ? '0d' : txtValidDate.value
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
    </script>
    <%--<script>
        function ShowAttachDocument(objFileControl) {
            debugger;
            var objRow = objFileControl.parentNode.parentNode.childNodes;
            var sFileName = '';
            var sUserType = '';
            popht = 3; // popup height
            popwth = 3; // popup width
            var sDealerCode = document.getElementById('ContentPlaceHolder1_txtDealerCode').value;
            var txtUserType = document.getElementById('ContentPlaceHolder1_txtUserType');
            if (txtUserType != null) {
                sUserType = txtUserType.value;
            }
            sFileName = (objRow[4].children[0].innerText);
            var scrleft = (screen.width / 2) - (popwth / 2) - 80; //centres horizontal
            var scrtop = ((screen.height / 2) - (popht / 2)) - 40; //centres vertical
            window.open("../Spares/frmOpenAttachDocument.aspx?FileName=" + sFileName + "&DealerCode=" + sDealerCode + "&UserType=" + sUserType, "List", "top=" + scrtop + ",left=" + scrleft + ",width=1px,height=3px");
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Part Claim Creation"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="">
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
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:exportlocation runat="server" id="ExportLocation" />
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:searchgridview id="SearchGrid" runat="server" onimage_click="SearchImage_Click"
                        biscallforserver="true" />
                </asp:Panel>
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:collapsiblepanelextender id="CPEDocDetails" runat="server" targetcontrolid="CntDocDetails"
                        expandcontrolid="TtlDocDetails" collapsecontrolid="TtlDocDetails" collapsed="false"
                        imagecontrolid="ImgTtlDocDetails" expandedimage="~/Images/Minus.png" collapsedimage="~/Images/Plus.png"
                        suppresspostback="true" collapsedtext="Part Claim Details" expandedtext="Part Claim Details"
                        textlabelid="lblTtlDocDetails">
                    </cc1:collapsiblepanelextender>
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
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Claim Type:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpClaimType" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpClaimType_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMClaimType" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblClaimNo" runat="server" Text="Claim Request No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtClaimNo" runat="server" CssClass="TextBoxForString NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblClaimDate" runat="server" Text="Claim Request Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:currentdate id="txtClaimDate" runat="server" bcheckforcurrentdate="false" enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblGRNNo" runat="server" Text="GRN No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpGRNNo" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="true" OnSelectedIndexChanged="drpGRNNo_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:TextBox ID="txtGrnNo" runat="server" CssClass="NonEditableFields" Enabled="false" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblMGrnNo" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">GRN Date:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtGRN_Date" runat="server" Enabled="false" CssClass="NonEditableFields"></asp:TextBox>
                                    <asp:TextBox ID="txtCreatedBy" Text="" runat="server" CssClass="TextBoxForString"
                                        Enabled="false" Style="display: none"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Invoice No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInvoice_Date" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">LR No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLR_No" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblLrDate" runat="server" Text="LR Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLR_Date" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trInsuranceDts" runat="server" visible="false">
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblInsuCmpyName" runat="server" Text="Insurance Company Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInsuCmpyName" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="lblMInsuCmpyName" runat="server" Text="*" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Insurance Cover Note No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInsuCoverNoteNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="lblMInsuCoverNoteNo" runat="server" Text="*" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblValidityDate" runat="server" Text="Validity Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:currentdate id="txtValidityDate" runat="server" bcheckforcurrentdate="false" />
                                </td>
                            </tr>
                            <tr id="trApproveDetails" runat="server" visible="false">
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblApproveClaimNo" runat="server" Text="Claim Invoice No:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApproveClaimNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Claim Invoice Date:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprovalDate" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <%--<asp:Label ID="Label4" runat="server" Text="Validity Date:"></asp:Label>--%>
                                </td>
                                <td style="width: 18%">
                                    <%--<uc3:CurrentDate ID="CurrentDate1" runat="server" bCheckforCurrentDate="false" />--%>
                                </td>
                            </tr>
                            <tr id="trInsuranceDoc" runat="server" visible="false">
                                <td>Insurance Claim Document:
                                </td>
                                <td colspan="5">
                                    <%--RepeatDirection="Horizontal" RepeatLayout="Flow" --%>
                                    <div class="checkbox checkbox-primary">
                                        <asp:CheckBoxList ID="chkInsuranceDoc" runat="server" Width="500"></asp:CheckBoxList>
                                    </div>
                                </td>

                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:UpdatePanel UpdateMode="Conditional" ID="UPPart" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:Panel ID="PartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            class="">
                            <cc1:collapsiblepanelextender id="CPEPartDetails" runat="server" targetcontrolid="CntPartDetails"
                                expandcontrolid="TtlPartDetails" collapsecontrolid="TtlPartDetails" collapsed="True"
                                imagecontrolid="ImgTtlPartDetails" expandedimage="~/Images/Minus.png" collapsedimage="~/Images/Plus.png"
                                suppresspostback="true" collapsedtext="Part Details" expandedtext="Part Details"
                                textlabelid="lblTtlPartDetails">
                            </cc1:collapsiblepanelextender>
                            <asp:Panel ID="TtlPartDetails" runat="server">
                                <table width="100%">
                                    <tr class="panel-heading">
                                        <td align="center" class="panel-title">
                                            <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                        </td>
                                        <td width="1%">
                                            <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                <asp:GridView ID="PartGrid" runat="server" AllowPaging="false" CssClass="table table-bordered"
                                    AutoGenerateColumns="False" EditRowStyle-BorderColor="Black"
                                    GridLines="Horizontal"
                                    SkinID="NormalGrid"
                                    AlternatingRowStyle-Wrap="true"
                                    EditRowStyle-Wrap="true"
                                    HeaderStyle-Wrap="true">
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartID" runat="server" Text='<%# Eval("Part_ID") %>' Width="1%"></asp:TextBox>
                                                <asp:TextBox ID="txtMRDtsID" runat="server" Text='<%# Eval("MR_Dts_ID") %>' Width="1%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Part_No") %>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPartName" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Part_Name") %>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Qty" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBill_Qty" runat="server" CssClass="GridTextBoxForString" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                                    Text='<%# Eval("Bill_Qty","{0:#0.00}") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recv Qty" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRecv_Qty" runat="server" CssClass="GridTextBoxForString" Width="96%"
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Text='<%# Eval("Recv_Qty","{0:#0.00}")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripancy Qty" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescripancy_Qty" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Descripancy_Qty","{0:#0.00}")%>'
                                                    onkeypress=" return CheckForTextBoxValue(event,this,'5');" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Rate","{0:#0.00}")%>'
                                                    onkeypress="return CheckForTextBoxValue(event,this);" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Total","{0:#0.00}")%>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Tax" ItemStyle-Width="5%">
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
                                            <ItemStyle Width="5%" />
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtStatus" runat="server" Width="1%" Text='<%# Eval("Status") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wrg Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWrgPartID" runat="server" Width="1%" Text='<%# Eval("Wrg_Part_ID") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recv Part No" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWrgPartNo" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Wrg_Part_No") %>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recv Part Name" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWrgPartName" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Wrg_Part_Name") %>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Retain" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpRetain" runat="server" CssClass="GridComboBoxFixedSize"
                                                    Width="96%">
                                                    <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <%-- <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Group" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrNo" runat="server" Width="80%" Text='<%# Eval("group_code") %>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve Qty" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtApprovedQty" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Approved_qty","{0:0.00}") %>'
                                                    Width="96%" onkeypress="return CheckForTextBoxValue(event,this);" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acc Total" ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAccTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("AccTotal","{0:#0.00}")%>'
                                                    onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UPRPart" runat="server" AssociatedUpdatePanelID="UPPart">
                    <ProgressTemplate>
                        Inserting Record ......
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:Panel ID="PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                    class="ContaintTableHeader">
                    <cc1:collapsiblepanelextender id="CollapsiblePanelExtender1" runat="server" targetcontrolid="CntPartGroupDetails"
                        expandcontrolid="TtlPartGroupDetails" collapsecontrolid="TtlPartGroupDetails" collapsed="true"
                        imagecontrolid="ImgTtlPartGroupDetails" expandedimage="~/Images/Minus.png" collapsedimage="~/Images/Plus.png"
                        suppresspostback="true" collapsedtext="Group Tax Details" expandedtext="Group Tax Details"
                        textlabelid="lblTtlPartGroupDetails">
                    </cc1:collapsiblepanelextender>
                    <asp:Panel ID="TtlPartGroupDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                            CssClass="table table-condensed table-bordered">
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
                                            MaxLength="6"></asp:TextBox>
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

                <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                    class="ContaintTableHeader">
                    <cc1:collapsiblepanelextender id="CollapsiblePanelExtender2" runat="server" targetcontrolid="CntTaxDetails"
                        expandcontrolid="TtlTaxDetails" collapsecontrolid="TtlTaxDetails" collapsed="true"
                        imagecontrolid="ImgTtlTaxDetails" expandedimage="~/Images/Minus.png" collapsedimage="~/Images/Plus.png"
                        suppresspostback="true" collapsedtext="Receipt Tax Details" expandedtext="Receipt Tax Details"
                        textlabelid="lblTtlTaxDetails">
                    </cc1:collapsiblepanelextender>
                    <asp:Panel ID="TtlTaxDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered">
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
                                            Width="90%" MaxLength="5"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
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
                                        <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Claim_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>

                <%-- Accepted Quantity Calculations Start Here --%>
                <asp:Panel ID="Acc_PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                    class="ContaintTableHeader">
                    <cc1:collapsiblepanelextender id="CollapsiblePanelExtender3" runat="server" targetcontrolid="Acc_CntPartGroupDetails"
                        expandcontrolid="Acc_TtlPartGroupDetails" collapsecontrolid="Acc_TtlPartGroupDetails" collapsed="true"
                        imagecontrolid="Acc_ImgTtlPartGroupDetails" expandedimage="~/Images/Minus.png" collapsedimage="~/Images/Plus.png"
                        suppresspostback="true" collapsedtext=" Acc Group Tax Details" expandedtext=" Acc Group Tax Details"
                        textlabelid="Acc_lblTtlPartGroupDetails">
                                </cc1:collapsiblepanelextender>
                    <asp:Panel ID="Acc_TtlPartGroupDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Acc_lblTtlPartGroupDetails" runat="server" Text="Group Tax Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Acc_ImgTtlPartGroupDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Acc_CntPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="Acc_GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                            CssClass="table table-condensed table-bordered">
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
                                        <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("Acc_net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                            MaxLength="6"></asp:TextBox>
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
                                        <asp:TextBox ID="txtGrTaxAmt" runat="server" Text='<%# Eval("Acc_tax_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
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
                                        <asp:TextBox ID="txtGrTax1Amt" runat="server" Text='<%# Eval("Acc_tax1_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
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
                                        <asp:TextBox ID="txtGrTax2Amt" runat="server" Text='<%# Eval("Acc_tax2_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                            Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTaxTot" runat="server" Width="90%" Text='<%# Eval("Acc_Total","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="Acc_PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                    class="ContaintTableHeader">
                    <cc1:collapsiblepanelextender id="CollapsiblePanelExtender4" runat="server" targetcontrolid="Acc_CntTaxDetails"
                        expandcontrolid="Acc_TtlTaxDetails" collapsecontrolid="Acc_TtlTaxDetails" collapsed="true"
                        imagecontrolid="Acc_ImgTtlTaxDetails" expandedimage="~/Images/Minus.png" collapsedimage="~/Images/Plus.png"
                        suppresspostback="true" collapsedtext="Acc Receipt Tax Details" expandedtext="Acc Receipt Tax Details"
                        textlabelid="Acc_lblTtlTaxDetails">
                                </cc1:collapsiblepanelextender>
                    <asp:Panel ID="Acc_TtlTaxDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Acc_lblTtlTaxDetails" runat="server" Text="Acc Receipt Tax Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Acc_ImgTtlTaxDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Acc_CntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="Acc_GrdDocTaxDet" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered">
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
                                <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocID" runat="server" Width="5%" Text='<%# Eval("ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable Total" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDocTotal" runat="server" Text='<%# Eval("Acc_net_tr_amt","{0:#0.00}") %>' Width="90%"
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
                                <asp:TemplateField HeaderText="LST Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("Acc_mst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CST Amt" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("Acc_cst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax 1" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax1" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("Acc_surcharge_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
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
                                            Width="90%" MaxLength="5"></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
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
                                        <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                            Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Acc_Claim_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:collapsiblepanelextender id="CPEFileAttchDetails" runat="server" targetcontrolid="CntFileAttchDetails"
                        expandcontrolid="TtlFileAttchDetails" collapsecontrolid="TtlFileAttchDetails"
                        collapsed="false" imagecontrolid="ImgTtlFileAttchDetails" expandedimage="~/Images/Minus.png"
                        collapsedimage="~/Images/Plus.png" suppresspostback="true" collapsedtext="Attached Documents"
                        expandedtext="Attached Documents" textlabelid="lblTtlFileAttchDetails">
                    </cc1:collapsiblepanelextender>
                    <asp:Panel ID="TtlFileAttchDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="82%">
                                    <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlFileAttchDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;">
                        <table id="Table2" runat="server" class="ContainTable">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                        AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" 
                                        GridLines="Horizontal" HeaderStyle-Wrap="true" DataKeyNames="File_Names" CssClass="table table-condensed table-bordered"
                                        SkinID="NormalGrid" Width="100%">
                                        <%--OnRowCommand="DetailsGrid_RowCommand" OnRowDataBound="FileAttchGrid_RowDataBound"--%>
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFileAttchID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Description" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description") %>'
                                                        Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name" ItemStyle-Width="40%" >
                                                <ItemTemplate>
                                                   <asp:Label ID="lblFile" runat="server" Text='<%# Eval("File_Names") %>' Width="90%" CssClass="HideControl" 
                                                        onClick="return ShowAttachDocument(this);" ToolTip="Click Here To Open The File" ForeColor="#49A3D3"
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"></asp:Label> 
                                                   
                                                    <%--<a id="achFileName" runat="server" title="Click here to download file"><%# Eval("File_Names") %></a>--%>
                                                    <%--<asp:LinkButton ID="lnkDownload" runat="server" Text='<%# Eval("File_Names") %>' ToolTip="Click Here To Open/Download File" OnClick="lnkDownload_Click"></asp:LinkButton>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);" />
                                                    <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Download" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" ToolTip="Click Here To Open/Download File" OnClick="lnkDownload_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Path" ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPath" runat="server" Text='<%# Eval("Path") %>' Width="90%"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle Wrap="True" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 50%" align="center">User File Description
                                </td>
                                <td class="tdLabel" style="width: 50%" align="center">File Name
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdLabel">
                                    <div id="upload1" style="display: inline-block; padding-left: 15px; width: 100%">
                                        <input id="Text1" type="text" name="Text1" class="TextBoxForString" placeholder="File Description" style="width: 50%" />
                                        <input id="AttachFile" type="file" runat="server" style="width: 45%" class="TextBoxForString Cntrl1"
                                            onblur="return addFileUploadBox(this);" />
                                        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
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
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnGrnNo" runat="server" />
                <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="N" />
                <asp:HiddenField ID="hdnTrNo" runat="server" Value="" />
            </td>
        </tr>
    </table>
</asp:Content>
