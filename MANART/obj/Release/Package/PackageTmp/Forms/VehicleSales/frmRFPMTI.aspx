<%@ Page Title="Request For Price" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmRFPMTI.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmRFPMTI" MaintainScrollPositionOnPostback="true"%>

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
        <script src="../../Scripts/jsVehicleINAndInsttaltionCer.js"></script>        

        <script type="text/javascript">
            function pageLoad() {
                $(document).ready(function () {

                    var txtRoadPermitDate = document.getElementById("ContentPlaceHolder1_txtRoadPermitDate_txtDocDate");
                    $('#ContentPlaceHolder1_txtRoadPermitDate_txtDocDate').datepick({
                        // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                        onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
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
                    <asp:Label ID="lblTitle" runat="server" Text="Requset For Price (RFP)"> </asp:Label>
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
                        <uc2:Location ID="Location" runat="server" OndrpCountryIndexChanged="Location_CountrySelectedIndexChanged" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
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
                            SuppressPostBack="true" CollapsedText="Requset For Price (RFP)" ExpandedText="Requset For Price (RFP)"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlDealerPO" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="Requset For Price (RFP)" Width="96%"
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
                                  

                                     <td class="auto-style1">
                                    RFP No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                    </td>
                                     <td class="auto-style1">
                                    RFP Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtPODate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>


                                    </tr>
                            <tr>
                                     
                                     <td class="auto-style1">
                                    M1 Details:
                                    </td>
                                    <td class="auto-style1">

                                         <asp:DropDownList ID="drpM7Det" runat="server" AutoPostBack="true" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpM7Det_SelectedIndexChanged">
                                    </asp:DropDownList>
                                        
                                      
                                        </td>

                                  <td class="auto-style1">
                                    Dealer Remarks:
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtPaymentDetailsPO" runat="server" TextMode="MultiLine"  MaxLength="250" CssClass="TextBoxForString" Text="" ></asp:TextBox>
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
                            <tr>
                                   <td class="auto-style1">
                                    Before Tax Amount:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtList" runat="server"  AutoPostBack="true"    onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextForAmount" OnTextChanged="txtList_TextChanged"></asp:TextBox>
                                </td>

                                <td class="auto-style1">
                                    MRP:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtMRP" runat="server" ReadOnly="true" onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextForAmount"></asp:TextBox>
                                </td>
                                  
                            </tr>
                            <tr>
                                       <td class="auto-style1">
                                    Tax Details:
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txttaxdetails" runat="server"  ReadOnly="true" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                    </td>
                                   <td class="auto-style1">
                                    HO Remarks:
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtAppRemarks" runat="server" TextMode="MultiLine"  MaxLength="250" CssClass="TextBoxForString" Text="" ></asp:TextBox>
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
                            
                                
                            </tr>
                            
                              <tr>
                                    <td class="auto-style1">
                                    Model:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                   
                                </td>
                                   <td class="tdLabel">
                                Quantity:
                            </td>
                            <td>
                                <asp:TextBox ID="txtQty" runat="server"  ReadOnly="true"   onkeypress=" return CheckForTextBoxValue(event,this,'5');" CssClass="TextForAmount"></asp:TextBox>
                               
                            </td>

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
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hideentaxID" runat="server" />
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
