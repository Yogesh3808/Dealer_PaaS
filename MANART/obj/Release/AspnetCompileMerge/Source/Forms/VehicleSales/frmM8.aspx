<%@ Page Title="M8 (Vehicle Invoiced & Delivered to Customer)" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmM8.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmM8" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/MANPendingDoc.ascx" TagName="PendingDocument" TagPrefix="UCPDoc" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsLCDetailsFunctions.js"></script>
    <script src="../../Scripts/jsVehicleINAndInsttaltionCer.js"></script>
    <script src="../../Scripts/jsCouponFileAttach.js"></script>

    <script src="../../Scripts/jsFileAttach.js"></script>
    <%--<script>
            function Finalamoutcalulate() {
                //alert("hi");
                var SalesPrice = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_0').val();
                var DiscAppAmt = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_2').val();
                var FinalAmt = SalesPrice - DiscAppAmt;
                //alert(FinalAmt);
                $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val(FinalAmt);
                
                // alert($('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val());
            }
        </script>--%>



   


    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {



                //var txtM8Date = document.getElementById("ContentPlaceHolder1_txtM8Date_txtDocDate");
                //$('#ContentPlaceHolder1_txtM8Date_txtDocDate').datepick({
                //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                //});

                var txtDODate = document.getElementById("ContentPlaceHolder1_txtDODate_txtDocDate");
                $('#ContentPlaceHolder1_txtDODate_txtDocDate').datepick({
                    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy'//, minDate: '0d'
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







    <script type='text/javascript'>
        function sldeUpDown(id, header) {
            if ($("#" + id).css('display') == 'block') {
                $("#" + id).slideUp('slow');

            }
            else {
                $("#" + id).slideDown('slow');
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="M8 (Vehicle Invoiced & Delivered to Customer)"> </asp:Label>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 15%">
                    <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                        <tr>
                            <td>
                                <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 14%">
                    <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>

                    <asp:Panel ID="PnlPendingDocument" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <UCPDoc:PendingDocument ID="PDoc" runat="server" />
                        <%--<uc4:SearchGridView ID="PDoc" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />--%>
                    </asp:Panel>


                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />
                    </asp:Panel>




                    <asp:Panel ID="POHeader" runat="server" BorderColor="Black" BorderStyle="Double">

                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntDealerPO"
                            ExpandControlID="TtlDealerPO" CollapseControlID="TtlDealerPO" Collapsed="false"
                            ImageControlID="ImgTtlDealerPODetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M8 (Vehicle Invoiced & Delivered to Customer)" ExpandedText="M8 (Vehicle Invoiced & Delivered to Customer)"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlDealerPO" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">

                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="M8 (Vehicle Invoiced & Delivered to Customer)" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDealerPODetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntDealerPO" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                            <table id="txtDocNoDetails" runat="server" class="ContainTable" border="1">
                                <tr>


                                    <td class="auto-style1">M8 No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtM8No" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td class="auto-style1">M8 Date:
                                    </td>
                                    <td>
                                        <uc3:CurrentDate ID="txtM8Date" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>
                                    <td class="auto-style1">Customer:
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                    </td>


                                </tr>
                                <tr>

                                    <td class="tdLabel">Financier:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpM4Financier" runat="server" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                        <%--<b class="Mandatory">*</b>--%>
                                    </td>
                                    <td class="tdLabel">Branch:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBranch" runat="server" MaxLength="100" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="Loan Amount:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoanAmt" runat="server" CssClass="TextBoxForString" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="Margin Money:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMarginMoney" runat="server" CssClass="TextBoxForString" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>

                                    </td>

                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="Tenure (In Months):" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTenure" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" Text="Interest Rate:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInterestRate" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>

                                </tr>
                                <tr>
                                    <td>

                                        <asp:Label ID="Label1" runat="server" Text="Model Change:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtModelChange" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>

                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="DO No:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDONo" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="DO Date:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <uc3:CurrentDate ID="txtDODate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

                                    </td>

                                </tr>
                                <tr>
                                    <td>

                                        <asp:Label ID="Label8" runat="server" Text="DO Amount:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOAmt" runat="server" CssClass="TextBoxForString" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>

                                    </td>

                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Trailer Chassis:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTrailerChanssis" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                    <td>

                                        <asp:Label ID="Label3" runat="server" Text="Trailer Amount:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTrailerAmt" runat="server" CssClass="TextBoxForString" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>

                                    </td>


                                </tr>
                                <tr>
                                    <td>

                                        <asp:Label ID="Label4" runat="server" Text="Tax:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpTaxPer" AutoPostBack="True" runat="server" OnSelectedIndexChanged="drpTaxPer_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                        <asp:Label ID="lbltaxper" runat="server" Text="*" CssClass="Mandatory"></asp:Label>
                                    </td>

                                      <td>
                                     <asp:Label ID="Label15" runat="server" Text="Customer GSTIN:" CssClass="tdLabel"></asp:Label>
                                        </td>
                                    <td>
                                         <asp:TextBox ID="txtGSTNo" runat="server" MaxLength="50"  CssClass="TextBoxForString"></asp:TextBox>
                                    </td>


                                    <td>

                                        <asp:Label ID="Label9" runat="server" Text="Consignee Name:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCName" runat="server" MaxLength="100"  CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                     
                                </tr>
                                <tr>
                                     <td>

                                        <asp:Label ID="Label10" runat="server" Text="Consignee Address:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCAdd" runat="server" TextMode="MultiLine"  MaxLength="250" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>

                                    <td>

                                        <asp:Label ID="Label11" runat="server" Text="Consignee GSTIN:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCCST" runat="server" MaxLength="50"  CssClass="TextBoxForString"></asp:TextBox>
                                        
                                    </td>
                                    
                                      <td>

                                        <asp:Label ID="Label13" runat="server" Text="Consignee TIN:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCTIN" runat="server" MaxLength="50"  CssClass="TextBoxForString"></asp:TextBox>
                                        
                                    </td>

                                </tr>
                                <tr>
                                  

                                      <td>

                                        <asp:Label ID="Label12" runat="server" Visible="false" Text="Consignee VAT:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCVAT" runat="server" Visible="false" MaxLength="50"  CssClass="TextBoxForString"></asp:TextBox>
                                        
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderStyle="Double">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntModel"
                            ExpandControlID="TtlModel" CollapseControlID="TtlModel" Collapsed="false"
                            ImageControlID="ImgTtlModelDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Model" ExpandedText="Model"
                            TextLabelID="lblTtlModel">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlModel" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">

                                        <asp:Label ID="lblTtlModel" runat="server" Text="Model" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntModel" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                            <table id="Table2" runat="server" class="ContainTable" border="1">
                                <%-- <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    Model
                                </td>
                            </tr>--%>
                                <tr>
                                    <td class="tdLabel">Model Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label40" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                    </td>



                                    <td class="tdLabel">Model Code:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpModelCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>

                                    </td>

                                    <td class="tdLabel">Model:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpModel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpModel_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>


                                    <td class="tdLabel">Model Price:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtModelRate" runat="server" CssClass="TextBoxForString" AutoPostBack="true" onkeypress="return validateFloatKeyPress(this,event);" OnTextChanged="txtModelRate_TextChanged"></asp:TextBox>
                                        &nbsp;
                                    </td>

                                    <td class="tdLabel">Quantity:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="TextBoxForString" AutoPostBack="true"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td class="tdLabel">Total:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="TextBoxForString" AutoPostBack="true"></asp:TextBox>
                                        &nbsp;
                                    </td>


                                </tr>
                                <tr>
                                    <td class="tdLabel">Chassis No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        &nbsp;
                                    </td>

                                    <td class="tdLabel">Engine No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString" AutoPostBack="true"></asp:TextBox>
                                        &nbsp;
                                    </td>


                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                        <%--<b class="Mandatory">*</b>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PTaxDet" runat="server" BorderColor="Black" BorderStyle="Double">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntTaxDet"
                            ExpandControlID="TtlTaxDet" CollapseControlID="TtlModel" Collapsed="false"
                            ImageControlID="ImgTtlTaxDetDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Tax Details" ExpandedText="Tax Details"
                            TextLabelID="lblTtlTaxDet">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlTaxDet" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">

                                        <asp:Label ID="lblTtlTaxDet" runat="server" Text="Tax Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlTaxDetDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntTaxDet" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table id="Table3" runat="server" border="1">
                                <tr id="trtcs" runat="server">
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="TCS Applicable"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpTCSApp" runat="server" Width="100px"
                                            OnSelectedIndexChanged="drpTCSApp_SelectedIndexChanged"
                                            CssClass="ComboBoxFixedSize" EnableViewState="true" AutoPostBack="True">
                                            <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Y</asp:ListItem>
                                            <asp:ListItem Value="2">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>


                                </tr>

                            </table>
                            <table id="Table1" runat="server" class="ContainTable" border="1">




                                <tr>
                                    <td class="tdLabel"></td>
                                    <td class="tdLabel"></td>

                                    <td class="tdLabel">Total Amount
                                    </td>


                                    <td class="tdLabel" id="lbldiscount" runat="server">Discount
                                    </td>

                                    <td id="tdvatlbl" runat="server">
                                        <td class="tdLabel">
                                        <asp:Label ID="lblCST" runat="server" Text="CST"></asp:Label>
                                    </td>
                                    <td class="tdLabel">
                                        <asp:Label ID="lblVat" runat="server" Text="VAT"></asp:Label>
                                    </td>
                                    </td>
                                    <%--<td id="tdSgstlbl" runat="server">
                                      <td class="tdLabel">
                                        <asp:Label ID="lblSGST" runat="server" Text="SGST"></asp:Label>
                                    </td>
                                     <td class="tdLabel">
                                        <asp:Label ID="lblIGST" runat="server" Text="IGST"></asp:Label>
                                    </td></td>--%>
                                    <td class="tdLabel">
                                        <asp:Label ID="lblTax1" runat="server" Text="Tax1"></asp:Label>
                                    </td>
                                          <td id="tdlbl" runat="server">

                                    <td class="tdLabel">
                                        <asp:Label ID="lblTax2" runat="server" Text="Tax2"></asp:Label>
                                    </td>
                             
                                    <td class="tdLabel" id="PFchager" runat="server">PF Charges
                                    </td>
                                    <td class="tdLabel" id="tdEntertax" runat="server">Entry Tax
                                    </td>
                                   
                                        </td>
                                     <td class="tdLabel" id="tdtcs" runat="server">
                                        <asp:Label ID="lblTCS" runat="server" Text="TCS"></asp:Label>
                                    </td>
                                    <td class="tdLabel">Grand Total
                                    </td>


                                </tr>

                                <tr>
                                    <td class="tdLabel">%
                                    </td>
                                    <td class="tdLabel"></td>
                                    <td>
                                        <asp:TextBox ID="Totalblank" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                        &nbsp;
                                    </td>

                                    <td id="tddiscbank" runat="server">
                                        <asp:TextBox ID="DiscBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>

                                    </td>
                                    <td id="tdvattax" runat="server">
                                         <td>
                                        <asp:TextBox ID="txtCSTPer" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                        <%--<asp:DropDownList ID="drpCSTPer" AutoPostBack="True" runat="server" OnSelectedIndexChanged="drpCSTPer_SelectedIndexChanged"   CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVatPer" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox><%--Onmouseover="txtVatPer_TextChanged"--%>
                                        <%--<asp:DropDownList ID="drpVATPer" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpVATPer_SelectedIndexChanged"  CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
                                    </td>
                                   </td>
<%--                                    <td id="tdsgsttax" runat="server">
                                     <td>
                                        <asp:TextBox ID="txtSGST" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>--%>
                                        <%--<asp:DropDownList ID="drpCSTPer" AutoPostBack="True" runat="server" OnSelectedIndexChanged="drpCSTPer_SelectedIndexChanged"   CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
<%--                                    </td>
                                     <td>
                                        <asp:TextBox ID="txtIGST" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>--%>
                                        <%--<asp:DropDownList ID="drpCSTPer" AutoPostBack="True" runat="server" OnSelectedIndexChanged="drpCSTPer_SelectedIndexChanged"   CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
                                  <%--  </td></td>--%>
                                   
                                    <td>
                                        <asp:TextBox ID="txttax1Per" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                        <%--<asp:DropDownList ID="drpTax1" AutoPostBack="True" runat="server" CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
                                    </td>
                                        <td id="tdper" runat="server">
                                    <td>
                                        <asp:TextBox ID="txtTax2Per" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                        <%--<asp:DropDownList ID="drpTax2" AutoPostBack="True" runat="server" CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
                                    </td>
                                
                                    <td>
                                        <asp:TextBox ID="PfBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="Otherblank" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                   </td>
                                     <td>
                                        <asp:TextBox ID="txtTCSPer" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="GrandBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>



                                </tr>
                                <tr>
                                    <td class="tdLabel">Amt
                                    </td>
                                    <td class="tdLabel"></td>
                                    <td>
                                        <asp:TextBox ID="txtTaxTotalAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                        &nbsp;
                                    </td>

                                    <td id="tdtxtdisc" runat="server">
                                        <asp:TextBox ID="txtDisc" runat="server" onkeypress="return validateFloatKeyPress(this,event);" Enabled="false"
                                            CssClass="TextForAmount" Width="90%" AutoPostBack="true" OnTextChanged="txtDisc_TextChanged"></asp:TextBox>

                                    </td>
                                    <td id="tdvatamt" runat="server">
                                         <td>
                                        <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVatAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                   </td>
                           <%--         <td id="tdsgstamt" runat="server">
                                       <td>
                                        <asp:TextBox ID="txtSGCTamt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIGSTamt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td></td>--%>
                                    <td>

                                        <asp:TextBox ID="txttax1Amt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    </td>
                                                         <td id="tdmount" runat="server">
                                    <td>
                                        <asp:TextBox ID="txttax2Amt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>

                                    <td>

                                        <asp:TextBox ID="txtPFCharges" runat="server" AutoPostBack="true" onkeypress=" return validateFloatKeyPress(this,event);"
                                            CssClass="TextForAmount" Width="90%" OnTextChanged="txtPFCharges_TextChanged"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOthercharges" runat="server" AutoPostBack="true" onkeypress="return validateFloatKeyPress(this,event);"
                                            CssClass="TextForAmount" Width="90%" OnTextChanged="txtOthercharges_TextChanged"></asp:TextBox>


                                    </td>
                                   </td>
                                     <td>
                                        <asp:TextBox ID="txtTCS" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>



                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEFileAttchDetails" runat="server" TargetControlID="CntFileAttchDetails"
                            ExpandControlID="TtlFileAttchDetails" CollapseControlID="TtlFileAttchDetails"
                            Collapsed="false" ImageControlID="ImgTtlFileAttchDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Attached Documents"
                            ExpandedText="Attached Documents" TextLabelID="lblTtlFileAttchDetails">
                        </cc1:CollapsiblePanelExtender>
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
                            <table id="Table10" runat="server" class="ContainTable">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                            AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                            GridLines="Horizontal" HeaderStyle-Wrap="true" DataKeyNames="File_Names"
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
                                                <asp:TemplateField HeaderText="File Name" ItemStyle-Width="40%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" Text='<%# Eval("File_Names") %>' Width="90%" onClick="return ShowAttachDocument(this);"></asp:Label>
                                                        <%-- onClick="return ShowAttachDoc(this);" ToolTip="Click Here To Open The File" ForeColor="#49A3D3"
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"--%>
                                                        <%--<a id="achFileName" runat="server" title="Click here to download file"><%# Eval("File_Names") %></a>--%>
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
                                            </Columns>
                                            <HeaderStyle Wrap="True" />
                                            <EditRowStyle BorderColor="Black" Wrap="True" />
                                            <AlternatingRowStyle Wrap="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 50%" align="center">File Description
                                    </td>
                                    <td class="tdLabel" style="width: 50%" align="center">File Name
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdLabel">
                                        <div id="upload1" style="display: inline-block; padding-left: 15px">
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
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                            Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                                Text=""></asp:TextBox><asp:TextBox ID="txtCashLoan" runat="server" CssClass="DispalyNon" Width="1px"
                                    Text=""></asp:TextBox><asp:TextBox ID="txtDocCashLoanType" runat="server" CssClass="DispalyNon" Width="1px"
                                        Text=""></asp:TextBox><asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                                            Text=""></asp:TextBox><asp:TextBox ID="txtM1ID" runat="server" CssClass="DispalyNon" Width="1px"
                                                Text=""></asp:TextBox><asp:TextBox ID="txtM2ID" runat="server" CssClass="DispalyNon" Width="1px"
                                                    Text=""></asp:TextBox><asp:TextBox ID="txtM3ID" runat="server" CssClass="DispalyNon" Width="1px"
                                                        Text=""></asp:TextBox><asp:TextBox ID="txtM4ID" runat="server" CssClass="DispalyNon" Width="1px"
                                                            Text=""></asp:TextBox><asp:TextBox ID="txtM5ID" runat="server" CssClass="DispalyNon" Width="1px"
                                                                Text=""></asp:TextBox><asp:TextBox ID="txtM6ID" runat="server" CssClass="DispalyNon" Width="1px"
                                                                    Text=""></asp:TextBox><asp:TextBox ID="txtAppID" runat="server" CssClass="DispalyNon" Width="1px"
                                                                        Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                                                                            Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTaxType" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTax1ID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTax2ID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTaxApp" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTaxApp1" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTaxApp2" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtTCSTaxID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtM7ID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtCSTID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtVATID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
<%--                                        <asp:TextBox ID="txtSGSTID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtIgstiD" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>--%>
                    <asp:TextBox ID="txtAlotID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                    <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:TextBox ID="txtRoundoff" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                    <asp:TextBox ID="txtRFPID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                    <asp:HiddenField ID="txtMRPPrice" runat="server" />
                    <asp:HiddenField ID="hiddentaxID" runat="server" /> 
                                        <asp:HiddenField ID="hdntaxtype" runat="server" />
                </td>
            </tr>
        </table>
    </div>
  <style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>


</asp:Content>
