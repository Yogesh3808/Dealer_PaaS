

    <%@ Page Title="M5 (Agreement & PDC Collected by Financier)" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmM5MTI.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmM5MTI" %>


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
     $(document).ready(function () {
         var DetailsGrid = document.getElementById("<%=DetailsGrid.ClientID%>");
         if (DetailsGrid != null) {
             var Elements = DetailsGrid.getElementsByTagName("input");
         }
         objDate = new Date();

         if (Elements != null) {
             var splDate = '';
             var dt = '';
             var Today = new Date(objDate.getFullYear(), objDate.getMonth + 1, objDate.getDay)
             for (var i = 0; i < Elements.length; i++) {


                 if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtNextObjDate") != -1) {
                     splDate = Elements[i].value.split("/")
                     dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
                     $('#' + Elements[i].id).datepick({
                         //                    dateFormat: 'dd/mm/yyyy', minDate: (Today > dt) ? '0d' : Elements[i].value
                         //  dateFormat: 'dd/mm/yyyy', minDate: '0d'
                         dateFormat: 'dd/mm/yyyy'
                     });


                 }
             }
         }

     });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtDocDate = document.getElementById("ContentPlaceHolder1_txtM5Date_txtDocDate");
            var DetailsGrid = document.getElementById("<%=DetailsGrid.ClientID%>");
            if (DetailsGrid != null) {
                var Elements = DetailsGrid.getElementsByTagName("input");
            }
            objDate = new Date();

            if (Elements != null) {
                var splDate = '';
                var dt = '';
                var Today = new Date(objDate.getFullYear(), objDate.getMonth + 1, objDate.getDay)
                for (var i = 0; i < Elements.length; i++) {
                    if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtObjDate") != -1) {
                        splDate = Elements[i].value.split("/")
                        dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
                        $('#' + Elements[i].id).datepick({
                            //                    dateFormat: 'dd/mm/yyyy', minDate: (Today > dt) ? '0d' : Elements[i].value
                            //    dateFormat: 'dd/mm/yyyy', minDate: '0d'
                            dateFormat: 'dd/mm/yyyy'
                        });


                    }
                }
            }

        });
    </script>
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



                    //var txtM5Date = document.getElementById("ContentPlaceHolder1_txtM5Date_txtDocDate");
                    //$('#ContentPlaceHolder1_txtM5Date_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                    //});

                    var txtLikelydate = document.getElementById("ContentPlaceHolder1_txtLikelydate_txtDocDate");
                    $('#ContentPlaceHolder1_txtLikelydate_txtDocDate').datepick({
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
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="table-responsive">
            <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="M5 (Agreement & PDC Collected by Financier)"> </asp:Label>
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
                                         <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                    </td>
                                
                                
                                    
                                
                                                        
                               
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">
                                       Title:
                                    </td>
                                    <td style="width: 15%">
                                 
                                        <asp:DropDownList ID="drpTitle" runat="server"  AutoPostBack="True" 
                                            CssClass="ComboBoxFixedSize"></asp:DropDownList>
                                         <asp:Label ID="Label25" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
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
                              
                                
                                
                                    <tr>
                                 
                                 
                                  

                                         

                                         <td class="tdLabel">
                                Financier of the Customer:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpM0Financier" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>
                            </td>
                                  
                                
                               
                                    </tr>
                                 
                            </table>
                        </asp:Panel>

                         </asp:Panel>
                
                   <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntM1"
                            ExpandControlID="TtlM1" CollapseControlID="TtlM1" Collapsed="true"
                            ImageControlID="ImgTtlM1Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M1 (Enquiry Generated) Details" ExpandedText="M1 (Enquiry Generated) Details"
                            TextLabelID="lblTtlM1">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM1" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM1" runat="server" Text="M1 (Enquiry Generated) Details" Width="96%"
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
                                    <asp:Label ID="lblLeadNo" runat="server" Text="M1 No:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLeadNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblLeadDate" runat="server" Text="M1 Date:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

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

                   

                    <asp:Panel ID="PQuotation" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Cntquotation"
                            ExpandControlID="TtlQuotation" CollapseControlID="TtlQuotation" Collapsed="true"
                            ImageControlID="ImgTtlQuotation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Details for Discount Approval" ExpandedText="Details for Discount Approval"
                            TextLabelID="lblTtlQuotation">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlQuotation" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlQuotation" runat="server" Text="Details for Discount Approval" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlQuotation" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Cntquotation" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                        
                       <table width="100%">
                                <tr>
                                    <td class="tdLabel">
                                        M2 No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtM2No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                    </td>

                                    <td>
                                
                                        <asp:Label ID="Label4" runat="server" Text="M2 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                        <uc3:CurrentDate ID="txtM2Date" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                    </td>
                                </tr>
                            
                                <tr>
                                    <td class="tdLabel">
                                        Quotation No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQutNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        
                                    </td>

                                    <td>
                                
                                        <asp:Label ID="Label24" runat="server" Text="Quotation Date:" CssClass="tdLabel"></asp:Label></td><td>
                                        <uc3:CurrentDate ID="txtqutdate" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        Competitor:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCompetitor" runat="server" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                   
                                    </td>
                                    <td class="tdLabel">
                                        Competitor Model:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompModel" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                               
                                    </td>
                                    <td class="tdLabel">
                                        Proposed Discount:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompDiscAmt" runat="server" onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                    </td>

                                </tr>
                        </table>
                        
                            <asp:GridView ID="QuotationDtls" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" CssClass="table table-bordered table-hover"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuotationID" runat="server" Text='<%# Eval("ID") %>' Width="3"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                
                                   <asp:TemplateField HeaderText="No." ItemStyle-Width="2%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuotID" runat="server" CssClass="LabelCenterAlign" 
                                                Text='<%# Eval("QuotId") %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                        
                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" CssClass="LabelCenterAlign" 
                                                Text='<%# Eval("Description") %>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>      
                                    
                                         <asp:TemplateField HeaderText="Value" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuotValue" runat="server"  CssClass="TextForAmount" Text='<%# Eval("Value","{0:#0}") %>' EnableViewState="true"
                                                 onkeypress=" return CheckForTextBoxValue(event,this,'6');"> </asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>        
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="AppDet" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntApp"
                            ExpandControlID="TtlApp" CollapseControlID="TtlApp" Collapsed="true"
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

                                   </tr>

                                <tr>
                                  <td>
                                    <asp:Label ID="Label9" runat="server" Text="Total Approved Discount:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppDisc" runat="server" onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                   
                                </td>
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

                            </tr>
                            <tr>
                                 <td>
                                    <asp:Label ID="Label13" runat="server" Text="Final Amount:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppFinalAmt" runat="server" CssClass="TextBoxForString" onkeypress=" return CheckForTextBoxValue(event,this,'6');" Text="" ></asp:TextBox>
                                    
                                </td>

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


                      <asp:Panel ID="M3DocDet" runat="server" BorderColor="Black" BorderStyle="Double">


                           <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="M3Det"
                            ExpandControlID="TtlM3Det" CollapseControlID="TtlM3Det" Collapsed="true"
                            ImageControlID="ImgTtlM3Det" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M3 (Purchase Order Received) Details" ExpandedText="M3 (Purchase Order Received) Details"
                            TextLabelID="lblTtlM3Det">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM3Det" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM3Det" runat="server" Text="M3 (Purchase Order Received) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM3Det" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="M3Det" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">



                       <table id="Table3" runat="server" class="ContainTable" border="1">
                        
                            
                            
                                 <tr>
                                <td class="tdLabel">
                                    M3 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM3No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label1" runat="server" Text="M3 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM3Date" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                </td>

                            </tr>
                            <tr>
                                    <td class="tdLabel">
                                    PO No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustPONo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label2" runat="server" Text="PO Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtCustPODate" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                </td>
                                </tr>

                            <tr>
                                    <td class="tdLabel">
                                    MTI Proforma Inv No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMTIProforma" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label3" runat="server" Text="MTI Proforma Inv Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtMTIProformaDate" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                </td>
                                </tr>

                           

                                
                            <tr>

                                  <td>
                                <asp:Label ID="Label7" runat="server" Text="Remarks:" CssClass="tdLabel"></asp:Label>
                            </td>
                                <td>
                                      <asp:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;

                                </td>

                                   <td>
                                <asp:Label ID="Label6" runat="server" Text="Vehicle Fund:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpFund" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Cash</asp:ListItem>
                                    <asp:ListItem Value="2">Loan</asp:ListItem>
                                </asp:DropDownList>
                               
                            </td>
                                </tr>
                        </table>
                    </asp:Panel>
                          </asp:Panel>


                    <asp:Panel ID="M4DocDet" runat="server" BorderColor="Black" BorderStyle="Double">


                           <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="M4Det"
                            ExpandControlID="TtlM4Det" CollapseControlID="TtlM4Det" Collapsed="true"
                            ImageControlID="ImgTtlM4Det" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M4 (Financing Process) Details" ExpandedText="M4 (Financing Process) Details"
                            TextLabelID="lblTtlM4Det">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM4Det" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM4Det" runat="server" Text=" M4 (Financing Process) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM4Det" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="M4Det" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">



                       <table id="Table5" runat="server" class="ContainTable" border="1">
                        
                            
                            
                                 <tr>
                                <td class="tdLabel">
                                    M4 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM4No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label15" runat="server" Text="M4 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM4Date" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
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

                            <td class="tdLabel">
                                    Branch:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBranch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                </td>
                           </tr>
                           

                                
                            <tr>

                                

                                <td>
                                    <asp:Label ID="Label19" runat="server" Text="All Document Collected:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpDocCollected" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                
                                </td>
                                 <td class="tdLabel">
                                    Pending Document:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPendingDoc" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                </td>



                                </tr>
                           <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="Loan Amount:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoanAmt" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                </td>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Margin Money:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMarginMoney" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                </td>

                           </tr>

                           <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="Tenure (In Months):" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTenure" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Text="Interest Rate (In %):" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInterestRate" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                   
                                </td>

                           </tr>
                        </table>
                    </asp:Panel>
                          </asp:Panel>

                    <asp:Panel ID="M5DocDet" runat="server" BorderColor="Black" BorderStyle="Double">


                           <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender8" runat="server" TargetControlID="M5Det"
                            ExpandControlID="TtlM5Det" CollapseControlID="TtlM5Det" Collapsed="false"
                            ImageControlID="ImgTtlM5Det" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M5 (Agreement & PDC Collected by Financier) Details" 
                               ExpandedText="M5 (Agreement & PDC Collected by Financier) Details"
                            TextLabelID="lblTtlM5Det">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM5Det" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM5Det" runat="server" Text=" M5 (Agreement & PDC Collected by Financier) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM5Det" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="M5Det" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">



                       <table id="Table6" runat="server" class="ContainTable" border="1">
                        
                            
                            
                                 <tr>
                                <td class="tdLabel">
                                    M5 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM5No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label22" runat="server" Text="M5 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM5Date" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                </td>

                            </tr>

                            <tr>
                                <td>
                                <asp:Label ID="Label21" runat="server" Text="Likely Buy Date:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <uc3:CurrentDate ID="txtLikelydate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />
                                
                            </td>
                                <td>
                                    <asp:Label ID="Label23" runat="server" Text="Agreement & PDC Collected by Financier:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpAggPDC" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                
                                </td>
                                 <td class="tdLabel">
                                    If No,then Pending Remarks:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPendingRemarks" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                </tr>
                            <tr>

                                 <td>
                                <asp:Label ID="Label50" runat="server" Text="Enquiry No:" Font-Bold="true" CssClass="tdLabel"></asp:Label>
                                     
                            </td>
                            <td>
                                <asp:TextBox ID="txtEnquiryNo" Text="" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                
                            </td>
                           </tr>

                        </table>
                    </asp:Panel>
                          </asp:Panel>

                       <asp:Panel ID="PObjectiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEObjectiveDetails" runat="server" TargetControlID="CntObjectiveDetails"
                        ExpandControlID="TtlObjectiveDetails" CollapseControlID="TtlObjectiveDetails"
                        Collapsed="false" ImageControlID="ImgTtlObjectiveDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Visit Details"
                        ExpandedText="Visit Details" TextLabelID="lblTtlObjectiveDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlObjectiveDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title">
                                    <asp:Label ID="lblTtlObjectiveDetails" runat="server" Text="Visit Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlObjectiveDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntObjectiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                            <Columns>
                                <%--<asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtObjID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Visit Details" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpVisitObj" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <uc3:CurrentDate ID="txtObjDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discussion/Result" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscussion" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Time Spent" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTimeSpent" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Visit Details" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpNextObj" runat="server" CssClass="GridComboBoxFixedSize">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <uc3:CurrentDate ID="txtNextObjDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Commitment To Customer" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCommitment" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New/Cancel" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" />
                                        <asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValueLead(event,this);"
                                            runat="server">New</asp:LinkButton>
                                        <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" 
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>

                    <asp:Panel ID="PClosure" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server" TargetControlID="CntClosureDetails"
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
                                            <asp:TextBox ID="txtClosureID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox></ItemTemplate>

                                </asp:TemplateField><asp:TemplateField HeaderText="Loss Reason" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpCloseRsn"  AutoPostBack="true" OnSelectedIndexChanged="drpCloseRsn_SelectedIndexChanged" runat="server" CssClass="GridComboBoxFixedSize">
                                                <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%></asp:DropDownList>

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
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="true"> </asp:TextBox>
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
                         Text=""></asp:TextBox><asp:TextBox ID="txtM3ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM4ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtAppID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    
 <asp:TextBox ID="txtLossApp" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
