

<%@ Page Title="Dealer PO Approval" Language="C#"  MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmDealerPOApproval.aspx.cs" Inherits="MANART.Forms.VehiclePurchase.frmDealerPOApproval" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
    <%@ Register Src="~/WebParts/MTILocation.ascx" TagPrefix="uc2" TagName="Location" %>
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
        <script src="../../Scripts/jsVehicleINAndInsttaltionCer.js"></script>        <%--<script>
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



                    //var txtPODate = document.getElementById("ContentPlaceHolder1_txtPODate_txtDocDate");
                    //$('#ContentPlaceHolder1_txtPODate_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                    //});


                    //var txtRoadPermitDate = document.getElementById("ContentPlaceHolder1_txtRoadPermitDate_txtDocDate");
                    //$('#ContentPlaceHolder1_txtRoadPermitDate_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                    //});

                    //var txtPOApppDate = document.getElementById("ContentPlaceHolder1_txtPOApppDate_txtDocDate");
                    //$('#ContentPlaceHolder1_txtPOApppDate_txtDocDate').datepick({
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
                    <asp:Label ID="lblTitle" runat="server" Text="Dealer PO"> </asp:Label>
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
                  <%--  <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>--%>
                      <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:Location ID="Location" runat="server" />
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
                            SuppressPostBack="true" CollapsedText="Dealer PO" ExpandedText="Dealer PO"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlDealerPO" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="Dealer PO" Width="96%"
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
                                    <%--<td class="auto-style1">
                                    PO Type:
                                    </td>--%>
                                    

                                     <td class="auto-style1">
                                    PO No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                    </td>
                                     <td class="auto-style1">
                                    PO Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtPODate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>
                                    <td class="auto-style1">
                                   
                                 <asp:DropDownList ID="drpVehPOType" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpVehPOType_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">For Stock</asp:ListItem>
                                    <asp:ListItem Value="2">From Enquiry</asp:ListItem>
                                </asp:DropDownList>
                                    
                                    </td>

                                    </tr>
                            <tr>
                                     
                                     <td class="auto-style1">
                                    Enquiry No:
                                    </td>
                                    <td class="auto-style1">

                                         <asp:DropDownList ID="drpM7Det" runat="server" AutoPostBack="true" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpM7Det_SelectedIndexChanged">
                                    </asp:DropDownList>
                                        
                                      
                                        </td>

                                    <td class="auto-style1">
                                    Customer:
                                    </td>

                                      <td>
                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                   
                                    </td>

                                   <td class="auto-style1">
                                    Plant:
                                    </td>

                                    <td>
                                    <asp:DropDownList ID="drpPlant" runat="server" AutoPostBack="true" CssClass="ComboBoxFixedSize" >
                                    </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                               <tr>

                                  
                                   
                                      <td class="auto-style1">
                                    Depot:
                                    </td>

                                    <td>
                                    <asp:TextBox ID="txtDepot" runat="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                    </td>
                                   <td class="auto-style1">
                                    Road Permit No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtRoadPermitNo" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                   
                                    </td>
                                     <td class="auto-style1">
                                    Validity Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtRoadPermitDate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>
                               </tr>
                             <tr>

                                  <td class="tdLabel">
                                Financier:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpM4Financier" runat="server" CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>
                            </td>
                                 <td class="auto-style1">
                                    Location:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtLocFinc" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                               
                                </td>
                             </tr>
                                <tr>
                                    <td class="auto-style1">
                                    Payment Details:
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtPaymentDetailsPO" runat="server" TextMode="MultiLine"  MaxLength="250"  CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                    </td>
                                      <td class="auto-style1">
                                    Approval No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtPOAppNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                    </td>
                                     <td class="auto-style1">
                                    Approval Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtPOApppDate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

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
                                  <td class="auto-style1">
                                    Model Code:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModelCode" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    
                                </td>
                            
                                  <td class="auto-style1">
                                    Model:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                   
                                </td>
                            </tr>
                            <tr>
                            
                            
                                <td class="auto-style1">
                                    Model Price:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtModelRate" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td class="auto-style1">
                                    Quantity:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtQty" runat="server" CssClass="TextBoxForString" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                                    &nbsp;
                                </td>
                                 <td>

                               
                                </td>
                                 <td>
                                    <asp:Button ID="bModify"  OnClick="bModifyPO" Text="Modify PO" visible="false" runat="server" ></asp:Button>
                                     
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChangeQty" Visible="false" runat="server"  CssClass="TextBoxForString"></asp:TextBox>
                                 <%--   <asp:Label ID="Label1" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                                </td>
                            </tr>
                              <tr>
                                   <td>
                                       <asp:Label ID="lblModify" Text="Modify Remarks" Visible="false"  runat ="server" ></asp:Label>
                                    
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtHoldRemarks" MaxLength="100" Visible="false" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
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

                   

                 

                    <asp:Panel ID="AppDet" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntApp"
                            ExpandControlID="TtlApp" CollapseControlID="TtlApp" Collapsed="false"
                            ImageControlID="ImgTtlAppDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Discount Approval Details" ExpandedText="Discount Approval Details"
                            TextLabelID="lblTtlApp">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlApp" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlApp" runat="server" Text="Discount Approval Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlAppDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntApp" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                        <table id="Table1" runat="server" class="ContainTable" border="1">
                             
                            
                      
                                <tr>
                                    <td>
                                    <asp:Label ID="Label5" runat="server" Text="Approval No:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Approval Date:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <uc3:CurrentDate ID="txtAppDate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                </td>
                                     <td>
                                    <asp:Label ID="Label9" runat="server" Text="Total Approved Discount:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppDisc" runat="server" onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                   
                                </td>

                                   </tr>

                                <tr>
                                 
                                  <td>
                                    <asp:Label ID="Label10" runat="server" Text="Approved Dealer Share:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppDealershare" runat="server"  onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                   
                                </td>
                                  <td>
                                    <asp:Label ID="Label11" runat="server" Text="Approved MTI Share:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppMTIshare" runat="server"  CssClass="TextBoxForString" onkeypress=" return CheckForTextBoxValue(event,this,'6');" Text="" ></asp:TextBox>
                                    
                                </td>
                                    <td>
                                    <asp:Label ID="Label13" runat="server" Text="Final Amount:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppFinalAmt" runat="server" CssClass="TextBoxForString" onkeypress=" return CheckForTextBoxValue(event,this,'6');" Text="" ></asp:TextBox>
                                    
                                </td>

                            </tr>
                            <tr>
                                 

                                  <td>
                                    <asp:Label ID="Label12" runat="server" Text="Remarks:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppremarks" runat="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                   
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
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="HdnModify" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
