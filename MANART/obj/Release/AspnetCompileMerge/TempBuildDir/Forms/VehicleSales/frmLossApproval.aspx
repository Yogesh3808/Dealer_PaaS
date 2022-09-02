

    <%@ Page Title="Loss Approval" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmLossApproval.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmLossApproval" %>


    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

 
<%--<uc2:MTILocation runat="server" id="MTILocation" />--%>
    <%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
    <%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
    <%@ Register Src="~/WebParts/MANPendingDoc.ascx" TagName="PendingDocument" TagPrefix="UCPDoc" %>
    <%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="~/WebParts/MultiselectLocation.ascx" TagPrefix="uc2" TagName="MultiselectLocation" %>
<%@ Register Src="~/WebParts/MTILocation.ascx" TagPrefix="uc2" TagName="Location" %>





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
        <script>
            function Finalamoutcalulate() {
                var TotalDisc = $('#ContentPlaceHolder1_txtAppDisc').val();
                var Dealershare = $('#ContentPlaceHolder1_txtAppDealershare').val();
                var MTIShare = $('#ContentPlaceHolder1_txtAppMTIshare').val();

                if (TotalDisc > 0) {
                    $('#ContentPlaceHolder1_txtAppDealershare').blur(function () {
                        var FinalMTIShare = TotalDisc - Dealershare;
                        $('#ContentPlaceHolder1_txtAppMTIshare').val(FinalMTIShare);
                    });

                    $('#ContentPlaceHolder1_txtAppMTIshare').blur(function () {
                        var FinalDealerShare = TotalDisc - MTIShare;
                        $('#ContentPlaceHolder1_txtAppDealershare').val(FinalDealerShare);
                    });

                }


            }
        </script>



   

        <script type="text/javascript">
            function pageLoad() {
                $(document).ready(function () {







                    //var txtM3Date = document.getElementById("ContentPlaceHolder1_txtM3Date_txtDocDate");
                    //$('#ContentPlaceHolder1_txtM3Date_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtM3Date.value == '') ? '0d' : txtM3Date.value, maxDate: '0d'
                    //});



                    //var txtAppDate = document.getElementById("ContentPlaceHolder1_txtAppDate_txtDocDate");
                    //$('#ContentPlaceHolder1_txtAppDate_txtDocDate').datepick({
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
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="table-responsive">
            <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="Loss Approval"> </asp:Label>
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
                <%--    <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OndrpCountryIndexChanged="Location_DealerSelectedIndexChanged" OnDDLSelectedIndexChanged="Location_DealerSelectedIndexChanged" />


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


                    <asp:Panel ID="M0Details" runat="server" BorderColor="Black" BorderStyle="Double">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntM0"
                            ExpandControlID="TtlM0" CollapseControlID="TtlM0" Collapsed="true"
                            ImageControlID="ImgTtlM0Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M0 (General Discussion) Details" ExpandedText="M0 (General Discussion) Details"
                            TextLabelID="lblTtlM0">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM0" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM0" runat="server" Text="M0 (General Discussion) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM0Details" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntM0" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                            <table id="Table4" runat="server" class="ContainTable" border="1">
                            <%--<tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    M0 Details
                                </td>
                            </tr>--%>

                                <tr>
                                 
                                 
                                  

                                         <td>
                                            <asp:Label ID="Label32" runat="server" Text="M0 No:" CssClass="tdLabel"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:TextBox ID="txtM0" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                   
                                        </td>
                                        <td>
                                        <asp:Label ID="Label33" runat="server" Text="M0 Date:" CssClass="tdLabel"></asp:Label>
                                        </td>
                                        <td>
                                        <uc3:CurrentDate ID="txtM0Date" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                        </td>
                                  
                                
                               <td style="width: 15%" class="tdLabel">
                                       Type:
                                    </td>
                                    <td style="width: 15%">
                                     <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                          onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                             AutoPostBack="true"  >
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="drpM0CustType" runat="server"  AutoPostBack="True" 
                                            CssClass="ComboBoxFixedSize" ></asp:DropDownList>
                                         <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                    </td>
                                
                                    </tr>

                                <tr>
                                    
                                
                                     <td style="width: 15%" class="tdLabel">
                                       Title:
                                    </td>
                                    <td style="width: 15%">
                                 
                                        <asp:DropDownList ID="drpTitle" runat="server"  AutoPostBack="True" 
                                            CssClass="ComboBoxFixedSize"></asp:DropDownList>
                                         <asp:Label ID="Label25" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                    </td>
                                
                                    <td style="width: 15%" class="tdLabel">
                                       Name:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString" 
                                            Text=""></asp:TextBox>
                                             <asp:Label ID="Label26" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                    </td>
                                
                                                        
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="Existing MTI Customer:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpIsMTICust" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                        EnableViewState="true" AutoPostBack="True">
                                        <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Y</asp:ListItem>
                                        <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </td>
                                </tr>
                              
                              
                                
                                
                                    
                                 
                            </table>
                        </asp:Panel>

                         </asp:Panel>
                
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntM1"
                            ExpandControlID="TtlM1" CollapseControlID="TtlM1" Collapsed="true"
                            ImageControlID="ImgTtlM1Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Document Details" ExpandedText="Document Details"
                            TextLabelID="lblTtlM1">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM1" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM1" runat="server" Text="Document Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM1Details" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntM1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                        <table id="txtDocNoDetails" runat="server" class="ContainTable" border="1">
                             
                            
                      
                                <tr>
                                    <td>
                                    <asp:Label ID="lblLeadNo" runat="server" Text="Doc No:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLeadNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblLeadDate" runat="server" Text="Doc Date:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" bCheckforCurrentDate="false" />

                                </td>
                                    <td class="tdLabel">
                                    PO Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpPOType" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                  
                                </td>

                                  
                                </tr>
                               
                           
                            
                           
                       
                        
                        
                        </table>
                    </asp:Panel>
                        </asp:Panel>
                
                    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table2" runat="server" class="ContainTable" border="1">
                             <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    Model
                                </td>
                            </tr>
                           <tr>
                            <td class="tdLabel">
                                Model Category:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label35" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                             
                             <td class="tdLabel">
                                Model Code:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCode" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label37" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>

                            
                              <td >
                                 <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false" ></asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label36" Text="*" Visible="false" runat="server" CssClass="Mandatory"></asp:Label>
                               
                            </td>
                        </tr>
                        <tr>
                            
                           
                            
                            <td class="tdLabel">
                                Model:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label38" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>
                            <td class="tdLabel">
                                Quantity:
                            </td>
                            <td>
                                <asp:TextBox ID="txtQty" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                <asp:Label ID="Label39" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                        </tr>
                        </table>
                    </asp:Panel>

                    
                
               

                     
                    <asp:Panel ID="AppDet" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntApp"
                            ExpandControlID="TtlApp" CollapseControlID="TtlApp" Collapsed="false"
                            ImageControlID="ImgTtlAppDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Loss Approval Details" ExpandedText="Loss Approval Details"
                            TextLabelID="lblTtlApp">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlApp" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlApp" runat="server" Text="Loss Approval Details" Width="96%"
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
                                    <asp:Label ID="Label2" runat="server" Text="Approval No:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Approval Date:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <uc3:CurrentDate ID="txtAppDate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                </td>

                                   

                                </tr>
                       
                            <tr>
                                  <td>
                                    <asp:Label ID="Label7" runat="server" Text="Remarks:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppremarks"  MaxLength="100" runat ="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                   
                                </td>
                            </tr>
                        
                        </table>
                    </asp:Panel>
                        </asp:Panel>

                     <asp:Panel ID="PClosure" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="CntClosureDetails"
                        ExpandControlID="TtlClosureDetails" CollapseControlID="TtlClosureDetails" Collapsed="false"
                        ImageControlID="ImgTtlClosureDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Loss Details" ExpandedText="Loss Details"
                        TextLabelID="lblTtlClosureDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlClosureDetails" runat="server">
                        <table width="100%">
                             <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlClosureDetails" runat="server" Text="Loss Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td><td width="1%">
                                    <asp:Image ID="ImgTtlClosureDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntClosureDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                       
                        
                        <asp:GridView ID="ClosureGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="ClosureGrid_RowCommand" >
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtClosureID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox></ItemTemplate></asp:TemplateField>
                                <asp:TemplateField HeaderText="Loss Reason" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpCloseRsn"  AutoPostBack="true"  runat="server" CssClass="GridComboBoxFixedSize">
                                                <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%></asp:DropDownList>
                                            <%--OnSelectedIndexChanged="drpCloseRsn_SelectedIndexChanged"--%>

                                        </ItemTemplate></asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Competitor" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpCloseCompetitor" runat="server" CssClass="GridComboBoxFixedSize">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Competitor Make" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCompetitor" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox></ItemTemplate><ItemStyle Width="10%" />
                                    </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCompQty" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>   


                                    <asp:TemplateField HeaderText="New/Cancel" ItemStyle-Width="10%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" />
                                            <asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValueLead(event,this);"
                                                runat="server">New</asp:LinkButton><asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" 
                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></ItemTemplate><ItemStyle Width="10%" />
                                    </asp:TemplateField>

                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>


              
                </td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM1ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM2ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:TextBox ID="txtDealerLoc" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                    <asp:TextBox ID="txtDoctype" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:TextBox ID="txtDocID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:TextBox ID="txtRFPID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
