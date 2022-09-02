
<%@ Page Title="Direct Billing Order Form Approval" Language="C#"  MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
     CodeBehind="frmVehOrderFormApprovalMTI.aspx.cs" Inherits="MANART.Forms.VehiclePurchase.frmVehOrderFormApprovalMTI" %>
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
        <script src="../../Scripts/jsVehicle.js"></script>
        <script src="../../Scripts/jsVehicleINAndInsttaltionCer.js"></script>
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
                    <asp:Label ID="lblTitle" runat="server" Text="Direct Billing Order Form Approval"> </asp:Label>
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
                                   <%-- <td class="auto-style1">
                                    PO Type:
                                    </td>
                                    <td class="auto-style1">
                                   
                                 <asp:DropDownList ID="drpVehPOType" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpVehPOType_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">For Stock</asp:ListItem>
                                    <asp:ListItem Value="2">From Enquiry</asp:ListItem>
                                </asp:DropDownList>
                                    
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
                                    <uc3:CurrentDate ID="txtPODate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

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

                                
                                </tr>
                               <tr>

                                     <td class="auto-style1">
                                    Plant:
                                    </td>

                                    <td>
                                    <asp:DropDownList ID="drpPlant" runat="server" AutoPostBack="true" CssClass="ComboBoxFixedSize" >
                                    </asp:DropDownList>
                                    </td>
                                   
                                      <td class="auto-style1">
                                    Depot:
                                    </td>

                                    <td>
                                    <asp:TextBox ID="txtDepot" runat="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                    </td>
                               </tr>

                                <tr>

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
                                    <uc3:CurrentDate ID="txtRoadPermitDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

                                    </td>
                                </tr>
                           
                            <tr>
                                 <td class="auto-style1">
                                     Billing Address (Place of Supply):
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtSoldToParty" TextMode="MultiLine" 
                                        runat="server" MaxLength="250" CssClass="TextBoxForString" height="50px"  Text="" ></asp:TextBox>
                                      
                                    </td>
                                <td class="auto-style1">
                                    Source Dealer:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpSourceDealer" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                  
                                </td>
                                 
                            </tr>

                            
                            <tr>
                                
                                
                                 <td class="auto-style1">
                                    Servicing Dealer:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpServiceDealer" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                  
                                </td>
                                <td class="auto-style1">
                                    PDI Dealer:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpPDIDealer" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                  
                                </td>
                            </tr>
                           <tr>
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


                      <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="CntCustomer"
                            ExpandControlID="TtlCustomer" CollapseControlID="TtlCustomer" Collapsed="false"
                            ImageControlID="ImgTtlCustomer" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Customer Details" ExpandedText="Customer Details"
                            TextLabelID="lblTtlCustomer">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlCustomer" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlCustomer" runat="server" Text="Model" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlCustomer" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntCustomer" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                          <table id="Table3" runat="server" class="ContainTable" border="1">
                            <%-- <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    Model
                                </td>
                            </tr>--%>
                            <tr>
                                
                                <td class="auto-style1">
                                    Customer:
                                </td>

                                <td>
                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                </td>

                                 <td class="auto-style1">
                                    Mobile No:
                                </td>

                               <td>
                                    <asp:TextBox ID="txtMobileNo" runat="server"  Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                </td>


                                    

                            </tr>
                             <tr>
                                 <td class="auto-style1">
                                        <asp:Label ID="lblcstno" runat="server" Text="GSTIN No:"></asp:Label>
                                  
                                </td>

                                <td>
                                    <asp:TextBox ID="txtCSTNo" MaxLength="50" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                </td>
                                  

                               
                                <td class="auto-style1">
                                    PAN No:
                                </td>

                                <td>
                                    <asp:TextBox ID="txtPAN" MaxLength="50" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                </td>
                               

                                 </tr>
                          
                            
                             <tr>
                                   <td class="auto-style1">
                                   
                                     <asp:Label ID="Label2" runat="server" Text="Place of Delivery:"></asp:Label>
                                    </td>
                                  <td>
                                   <asp:TextBox ID="txtDeliveryAdd" TextMode="MultiLine" height="50px" MaxLength="250" runat="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
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
                                  <td class="auto-style1">
                                    
                                      <asp:Label ID="Label1" Text="Approved Quantity:" runat="server" Visible="false" ></asp:Label>
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtChangeQty" runat="server" Visible="false" CssClass="TextBoxForString"></asp:TextBox>
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



                    <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="CntOtherDet"
                            ExpandControlID="TtlOtherDet" CollapseControlID="TtlOtherDet" Collapsed="false"
                            ImageControlID="ImgTtlOtherDet" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Other Details" ExpandedText="Other Details"
                            TextLabelID="lblTtlOtherDet">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlOtherDet" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlOtherDet" runat="server" Text=" Other Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlOtherDet" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>


                         <asp:Panel ID="CntOtherDet" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                        <table id="Table5" runat="server" class="ContainTable" border="1">
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
                                    Form No:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtFormNo" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                               
                                </td>
                                  <td class="auto-style1">
                                    Online Form:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtOlForm"  MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr>
                                 <td class="auto-style1">
                                    Entry Tax:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtEntryTax"  MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>
                                 <td class="auto-style1">
                                    Checkpost:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtCheckpost" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>
                            </tr>
                      
                              <tr>
                                   <td class="auto-style1" id="tdbilllocation" runat="server">
                                    Billing Location:
                                    </td>
                                    <td id="tdbilllocation1" runat="server">
                                  <asp:DropDownList ID="drpBillingLoc" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpBillingLoc_SelectedIndexChanged" >
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Factory</asp:ListItem>
                                    <asp:ListItem Value="2">Depot</asp:ListItem>
                                </asp:DropDownList>
                                      </td>

                                    <td class="auto-style1">
                                    Billing Type:
                                    </td>
                                  <td>
                                  <asp:DropDownList ID="drpBillingType" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Billing with Normal ED & tax</asp:ListItem>
                                    <asp:ListItem Value="2">EPCG/CT-3/SEZ</asp:ListItem>
                                </asp:DropDownList>
                                      </td>

                              </tr>
                            <tr>
                                
                                     <td class="auto-style1">
                                    Tax Type:
                                    </td>
                                  <td>
                                  <asp:DropDownList ID="drpTaxtypeother" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true">
                               
                                </asp:DropDownList>
                                      </td>
                                   <td class="auto-style1">
                                    Insurance Company:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtInsurance" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                            </tr>
                             <tr>
                                   <td class="auto-style1" id="cstcertificateno" runat="server">
                                    CST Certificate No:
                                    </td>
                                <td id="txtcstcertificateno" runat="server">
                                    <asp:TextBox ID="txtCSTCertificate" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>
                                   <td class="auto-style1">
                                    EPCG No:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtEPCGNo" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                            </tr>
                            <tr>
                                <td class="auto-style1">
                                    Covernote Serial No:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtCovernoteno" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>
                                <td class="auto-style1">
                                    Expiry Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtCovernoteExpiry" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

                                    </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">
                                    Bank Name:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtBankName" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                                <td class="auto-style1">
                                    Chq/DD/UTR No.:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtchqno" MaxLength="50" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>


                            </tr>
                            <tr>
                                   <td class="auto-style1">
                                    Chq/DD/UTR Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtChqDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

                                    </td>

                                 <td class="tdLabel">
                                    Payment Amount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPayment" runat="server"  CssClass="TextForAmount"  onkeypress=" return CheckForTextBoxValue(event,this,'6');" ></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                
                                <td class="auto-style1">
                                    Any Specific Instructions:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtremarks"  MaxLength="100" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
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
                                    <uc3:CurrentDate ID="txtAppDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

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
                        <table id="Table4" runat="server" class="ContainTable">
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
                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("File_Names") %>' Width="90%"
                                                        onClick="return ShowAttachDocument(this);"> 
                                                    </asp:Label>
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
                             <tr id="TrFileDesc1" runat="server">
                                <td class="tdLabel" style="width: 50%" align="center">File Description
                                </td>
                                <td class="tdLabel" style="width: 50%" align="center">File Name
                                </td>
                            </tr>
                              <tr id="TrBtnUpload1" runat="server">
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
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                                     <asp:HiddenField ID="HiddenGSt" runat="server" Value="" />
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                        <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                     <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                
                </td>
            </tr>
        </table>
            <table visible="false">
                  <tr visible="false">
                      
                                   
                                
                                  <td visible="false">
                                    <asp:TextBox ID="txtShipToParty" visible="false" TextMode="MultiLine" runat="server" 
                                        CssClass="TextBoxForString" MaxLength="100" Text="" ></asp:TextBox>
                                     
                                    </td>
                      
                          

                                <td>
                                    <asp:TextBox visible="false" ID="txtLBT" MaxLength="50" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                </td>
                  </tr>
                   <tr>

                                
                                  <td>
                                    <asp:TextBox ID="txtPayerAdd" visible="false" TextMode="MultiLine" MaxLength="100" runat="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                    </td>

                                    <td>
                                    <uc3:CurrentDate ID="txtCSTExpiry" visible="false" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

                                    </td>
                               

                            </tr>
                   <tr id="trvatno" visible="false" runat="server">
                                   

                                <td>
                                    <asp:TextBox ID="txtVATNo" visible="false"  MaxLength="50" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                </td>
                               
                                    <td>
                                    <uc3:CurrentDate visible="false"  ID="txtVATExpiry" runat="server" Mandatory="false" bCheckforCurrentDate="false" />

                                    </td>
                             </tr>


            </table>
        </div>
    </asp:Content>
