<%@ Page Title="Dealer Sale Invoice" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmDealerSale.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmDealerSale" MaintainScrollPositionOnPostback="true" %>

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
                    <asp:Label ID="lblTitle" runat="server" Text="Dealer Sale Invoice"> </asp:Label>
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
                    <%--  <asp:Panel ID="PnlPendingDocument" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                       <UCPDoc:PendingDocument ID="PDoc" runat="server" />
                       <%--<uc4:SearchGridView ID="PDoc" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />--%>
                    <%--</asp:Panel>--%>

                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />
                    </asp:Panel>

                    <asp:Panel ID="POHeader" runat="server" BorderColor="Black" BorderStyle="Double">

                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntDealerPO"
                            ExpandControlID="TtlDealerPO" CollapseControlID="TtlDealerPO" Collapsed="false"
                            ImageControlID="ImgTtlDealerPODetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Dealer Sale Invoice" ExpandedText="Dealer Sale Invoice"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlDealerPO" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">

                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="Dealer Sale Invoice" Width="96%"
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


                                    <td class="auto-style1">Invoice No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtM8No" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td class="auto-style1">Invoice Date:
                                    </td>
                                    <td>
                                        <uc3:CurrentDate ID="txtM8Date" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>
                                    <td class="auto-style1">Customer:
                                    </td>

                                    <td>

                                        <asp:DropDownList ID="drpCustName" runat="server" CssClass="GridComboBoxFixedSize"
                                            EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpCustName_SelectedIndexChanged1">
                                        </asp:DropDownList>



                                    </td>


                                </tr>

                                <tr>
                                    <td class="auto-style1">PO No:
                                    </td>

                                    <td>

                                        <asp:DropDownList ID="drpPOList" runat="server" CssClass="GridComboBoxFixedSize"
                                            EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpPOList_SelectedIndexChanged">
                                        </asp:DropDownList>



                                    </td>
                                    <td class="tdLabel">Chassis No:
                                    </td>
                                    <td>

                                        <asp:DropDownList ID="drpChassisNo" runat="server" CssClass="GridComboBoxFixedSize"
                                            EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpChassisNo_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </td>
                                </tr>



                                <tr>



                                    <td class="tdLabel">Engine No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString" AutoPostBack="true"></asp:TextBox>

                                    </td>

                                    <td>

                                        <asp:Label ID="Label4" runat="server" Text="Tax:" CssClass="tdLabel"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpTaxPer" AutoPostBack="True" runat="server" OnSelectedIndexChanged="drpTaxPer_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
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
                                    <td class="tdLabel">
                                Model Category:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label35" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
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
                                        <asp:TextBox ID="txtModelRate" runat="server" CssClass="TextBoxForString" AutoPostBack="true" OnTextChanged="txtModelRate_TextChanged" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
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
                                        <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="TextBoxForString" AutoPostBack="true" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                        &nbsp;
                                    </td>


                                </tr>
                                <tr>
                                      <td class="auto-style1">
                                 <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false" ></asp:Label>
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false"  CssClass="ComboBoxFixedSize">
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
                            <table id="Table3" runat="server" border="1" >
                                <tr>
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


                                    <td class="tdLabel"  id="lbldiscount" runat="server">Discount
                                    </td>
                                    <td class="tdLabel">
                                        <asp:Label ID="lblCST" runat="server" Text="CST"></asp:Label>
                                    </td>


                                    <td class="tdLabel">
                                        <asp:Label ID="lblVat" runat="server" Text="VAT"></asp:Label>
                                    </td>
                                    

                                    <td class="tdLabel">
                                        <asp:Label ID="lblTax1" runat="server" Text="Tax1"></asp:Label>
                                    </td>
                                    <td id="tdlbl" runat="server">

                                    <td class="tdLabel">
                                        <asp:Label ID="lblTax2" runat="server" Text="Tax2"></asp:Label>
                                    </td>

                                    <td class="tdLabel">PF Charges
                                    </td>
                                    <td class="tdLabel">Entry Tax
                                    </td>
                    
                                    </td>
                                                    <td class="tdLabel">
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
                                        <asp:TextBox ID="DiscBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
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
                                  

                                    <td>
                                        <asp:TextBox ID="txttax1Per" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                        <%--<asp:DropDownList ID="drpTax1" AutoPostBack="True" runat="server" CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>--%>
                                    
                                    </td>
                                    <td id="tdper" runat="server" >
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
                                        <asp:TextBox ID="txtDisc" runat="server" onkeypress="return validateFloatKeyPress(this,event);"
                                            CssClass="TextForAmount" Width="90%" AutoPostBack="true" OnTextChanged="txtDisc_TextChanged"></asp:TextBox>

                                    </td>
                                     <td>
                                        <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVatAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                   

                                    <td>
                                        <asp:TextBox ID="txttax1Amt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td id="tdamount" runat="server">
                                    <td>
                                        <asp:TextBox ID="txttax2Amt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtPFCharges" runat="server" AutoPostBack="true" onkeypress="return validateFloatKeyPress(this,event);"
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
                </td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                            Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                                Text=""></asp:TextBox><asp:TextBox ID="txtCashLoan" runat="server" CssClass="DispalyNon" Width="1px"
                                    Text=""></asp:TextBox><asp:TextBox ID="txtDocCashLoanType" runat="server" CssClass="DispalyNon" Width="1px"
                                        Text=""></asp:TextBox><asp:TextBox ID="txtStockID" runat="server" CssClass="DispalyNon" Width="1px"
                                            Text=""></asp:TextBox><asp:TextBox ID="txtSAlePOID" runat="server" CssClass="DispalyNon" Width="1px"
                                                Text=""></asp:TextBox><asp:TextBox ID="txtSaleDealer" runat="server" CssClass="DispalyNon" Width="1px"
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
                    <asp:TextBox ID="txtAlotID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidegst" runat="server" Value="" />
                    <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
