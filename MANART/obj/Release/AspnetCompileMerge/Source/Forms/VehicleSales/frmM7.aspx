

    <%@ Page Title="M7 (Margin Money Received) Details" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmM7.aspx.cs" 
        Inherits="MANART.Forms.VehicleSales.frmM7" %>


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
                                 //dateFormat: 'dd/mm/yyyy', minDate: '0d'
                                 dateFormat: 'dd/mm/yyyy'
                             });


                         }
                     }
                 }

             });
    </script>
        <script >
            function CheckComplaintSelected(eve, objcontrol) {
                //debugger;
                alert ("Hi");
                //if (CheckForComboValue(eve, objcontrol, false) == true) {
                   
                //    if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
                //        return false;
                
                //}
               
            }

            
            function CheckComboValueAlreadyUsedInGrid(ObjCurRecord) {
                //debugger;
                ////if (bUsed == null) bUsed = false;

                
                //var i;
                //var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
                ////var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
                ////var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
                //var iColumnIndexOfControl = parseInt(ObjCurRecord.parentNode.cellIndex);
                //var ObjRecord;
                //var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
                //for (i = 1; i < objGrid.rows.length; i++) {
                //    ObjRecord = objGrid.rows[i].cells[iColumnIndexOfControl].children[0];
                //    //if (i != iRowOfSelectControl) {
                //        if (ObjRecord.id != ObjCurRecord.id) {
                //            if (sSelecedValue != "NEW") {
                //                if (sSelecedValue == ObjRecord.options[ObjRecord.selectedIndex].text) {
                //                    alert("Record is already selected at line No." + i);
                //                    //ObjCurRecord.selectedIndex = 0;
                //                    //if (bUsed == false) {
                //                    //    ObjCurRecord.focus();
                //                    //    bUsed = true;
                //                    //}
                //                    //else {
                //                    //    bUsed = null;
                //                    //}
                //                    return false;
                //                }
                //            }
                //        }
                //    //}
                //}
                alert("Hi");
               // return false;
            }
    </script>


    <script type="text/javascript">
        $(document).ready(function () {
            var txtDocDate = document.getElementById("ContentPlaceHolder1_txtM7Date_txtDocDate");
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
                            //  dateFormat: 'dd/mm/yyyy', minDate: '0d'
                            dateFormat: 'dd/mm/yyyy', maxDate: txtDocDate.value
                        });


                    }
                }
            }

        });
    </script>


        <script type="text/javascript">
            function pageLoad() {
                $(document).ready(function () {



                    //var txtM7Date = document.getElementById("ContentPlaceHolder1_txtM7Date_txtDocDate");
                    //$('#ContentPlaceHolder1_txtM7Date_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                    //});

                
                    var txtChRtgsDate = document.getElementById("ContentPlaceHolder1_txtChRtgsDate_txtDocDate");
                    $('#ContentPlaceHolder1_txtChRtgsDate_txtDocDate').datepick({
                        // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                        onSelect: customRange, dateFormat: 'dd/mm/yyyy'
                    });

                    //var txtM7DateCash = document.getElementById("ContentPlaceHolder1_txtM7DateCash_txtDocDate");
                    //$('#ContentPlaceHolder1_txtM7DateCash_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy'
                    //});


                    var txtChRTGSDateCash = document.getElementById("ContentPlaceHolder1_txtChRTGSDateCash_txtDocDate");
                    $('#ContentPlaceHolder1_txtChRTGSDateCash_txtDocDate').datepick({
                        // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                        onSelect: customRange, dateFormat: 'dd/mm/yyyy'
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
                    <asp:Label ID="lblTitle" runat="server" Text="M7 (Margin Money Received) Details"> </asp:Label>
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
                                  <%--  <b class="Mandatory">*</b>--%>
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
                                 <asp:Label ID="Label40" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                             
                             <td class="tdLabel">
                                Model Code:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCode" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label41" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>

                            
                              <td >
                                 <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false" ></asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label42" Text="*" Visible="false" runat="server" CssClass="Mandatory"></asp:Label>
                               
                            </td>
                        </tr>
                        <tr>
                            
                           
                            
                            <td class="tdLabel">
                                Model:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label43" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>
                            <td class="tdLabel">
                                Quantity:
                            </td>
                            <td>
                                <asp:TextBox ID="txtQty" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                <asp:Label ID="Label44" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
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
                            ExpandControlID="TtlM5Det" CollapseControlID="TtlM5Det" Collapsed="true"
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
                                    <uc3:CurrentDate ID="txtM5Date" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                </td>

                            </tr>

                            <tr>

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
                        </table>
                    </asp:Panel>
                          </asp:Panel>

                    <asp:Panel ID="M6DocDet" runat="server" BorderColor="Black" BorderStyle="Double">


                           <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender9" runat="server" TargetControlID="M6Det"
                            ExpandControlID="TtlM6Det" CollapseControlID="TtlM6Det" Collapsed="false"
                            ImageControlID="ImgTtlM6Det" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M6 (Delivery Order / Payment to MTI) Details" 
                               ExpandedText="M6 (Delivery Order / Payment to MTI) Details"
                            TextLabelID="lblTtlM6Det">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM6Det" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM6Det" runat="server" Text="M6 (Delivery Order / Payment to MTI) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM6Det" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="M6Det" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">



                       <table id="Table7" runat="server" class="ContainTable" border="1">
                        
                            
                            
                                 <tr>
                                <td class="tdLabel">
                                    M6 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM6No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label27" runat="server" Text="M6 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM6Date" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                </td>

                            </tr>

                            <tr>

                                <td class="tdLabel">
                                    Delivery Order No.:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDoNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                 <td>
                                
                                    <asp:Label ID="Label21" runat="server" Text="Delivery Order Date:" CssClass="tdLabel"></asp:Label>

                                 </td>
                                <td>
                                    <uc3:CurrentDate ID="txtDODate" runat="server" bCheckforCurrentDate="false" />
                                </td>
                                <td class="tdLabel">
                                    Amount Of Delivery Order:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDoAMt" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>

                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                    Payment Amount Received:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentAmt" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label28" runat="server" Text="Payment Date:" CssClass="tdLabel"></asp:Label>

                                 </td>
                                <td>
                                    <uc3:CurrentDate ID="txtPaymentDate" runat="server" bCheckforCurrentDate="false" />
                                </td>
                                </tr>

                        </table>
                    </asp:Panel>
                          </asp:Panel>


                    <asp:Panel ID="M7DocDet" runat="server" BorderColor="Black" BorderStyle="Double" Visible="false">


                           <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender10" runat="server" TargetControlID="M7Det"
                            ExpandControlID="TtlM7Det" CollapseControlID="TtlM7Det" Collapsed="false"
                            ImageControlID="ImgTtlM7Det" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M7 (Margin Money Received) Details" 
                               ExpandedText="M7 (Margin Money Received) Details"
                            TextLabelID="lblTtlM7Det">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM7Det" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM7Det" runat="server" Text="M7 (Margin Money Received) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM7Det" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="M7Det" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">



                       <table id="Table8" runat="server" class="ContainTable" border="1">
                        
                            
                            
                                 <tr>
                                <td class="tdLabel">
                                    M7 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM7No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label30" runat="server" Text="M7 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM7Date" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                </td>

                            </tr>

                            <tr>

                                <td class="tdLabel">
                                    Amount Of Margin Money:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMarginAmt" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                      <asp:Label ID="Label29" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                
                                <td class="tdLabel">
                                    Cheque/RTGS Details:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChRTGSDet" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                      <asp:Label ID="Label36" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                 <td>
                                
                                    <asp:Label ID="Label31" runat="server" Text="Cheque/Transfer Date:" CssClass="tdLabel"></asp:Label>

                                 </td>
                                <td>
                                    <uc3:CurrentDate ID="txtChRtgsDate" Mandatory="true" runat="server" bCheckforCurrentDate="false" />
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



                    <asp:Panel ID="M7DocCash" runat="server" BorderColor="Black" BorderStyle="Double" Visible="false">


                           <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender11" runat="server" TargetControlID="M7CashDet"
                            ExpandControlID="TtlM7CashDet" CollapseControlID="TtlM7CashDet" Collapsed="false"
                            ImageControlID="ImgTtlM7CashDet" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M7 (Payment Received) Details" 
                               ExpandedText="M7 (Payment Received) Details"
                            TextLabelID="lblTtlM7CashDet">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlM7CashDet" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM7CashDet" runat="server" Text="M7 (Payment Received) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM7CashDet" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="M7CashDet" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">



                       <table id="Table9" runat="server" class="ContainTable" border="1">
                        
                            
                            
                                 <tr>
                                <td class="tdLabel">
                                    M7 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM7NoCash" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label34" runat="server" Text="M7 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM7DateCash" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                </td>

                            </tr>

                            <tr>

                                <td class="tdLabel">
                                    Payment Amount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentCash" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                      <asp:Label ID="Label37" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                
                                <td class="tdLabel">
                                    Cheque/RTGS Details:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChRTGSDetCash" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                      <asp:Label ID="Label38" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                 <td>
                                
                                    <asp:Label ID="Label35" runat="server" Text="Cheque/Transfer Date:" CssClass="tdLabel"></asp:Label>

                                 </td>
                                <td>
                                    <uc3:CurrentDate ID="txtChRTGSDateCash" Mandatory="true" runat="server" bCheckforCurrentDate="false" />
                                </td>
                                </tr>
                            <tr>

                                 <td>
                                <asp:Label ID="Label39" runat="server" Text="Enquiry No:" Font-Bold="true" CssClass="tdLabel"></asp:Label>
                                     
                            </td>
                            <td>
                                <asp:TextBox ID="txtEnquiryNoCash" Text="" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                
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

                    <asp:Panel ID="PAllocation" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender12" runat="server" TargetControlID="CntAllocation"
                            ExpandControlID="TtlAllocation" CollapseControlID="TtlAllocation" Collapsed="false"
                            ImageControlID="ImgAllocation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Allocation Details" ExpandedText="Allocation Details"
                            TextLabelID="lblAllocation">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlAllocation" runat="server">
                            <table width="100%">
                                 <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                                        <asp:Label ID="lblAllocation" runat="server" Text="Allocation Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td><td width="1%">
                                        <asp:Image ID="ImgAllocation" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntAllocation" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                            <table>
                                 <tr>
                               <%-- <td class="" style="width: 15%"></td>--%>
                                <td>
                                    <asp:Button ID="bConfirm" OnClick="SaveAllocation" Text="Save Allocation" visible="true" runat="server" CssClass="ComboBoxFixedSize"></asp:Button>
                                </td>
                            </tr>
                        
                            </table>
                           
                        <asp:GridView ID="GrdAllocation" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"   >
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtallotID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>

                                        </ItemTemplate>

                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpChassisNo" runat="server" 
                                                OnSelectedIndexChanged="drpChassisNo_SelectedIndexChanged" 
                                              
                                                 AutoPostBack="true" 
                                                 CssClass="GridComboBoxFixedSize">
                                                
                                            </asp:DropDownList>
                                             <%--  onblur="return CheckComplaintSelected(event,this);"--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEngineNo" runat="server" EnableViewState="true" ReadOnly="true" CssClass="TextForAmount"
                                             > </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <asp:TemplateField HeaderText="MTI Invoice No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMTIInvNo" runat="server"  EnableViewState="true" ReadOnly="true" CssClass="TextForAmount"
                                             > </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="MTI Invoice Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMTIInvDate" runat="server"  EnableViewState="true" ReadOnly="true"  CssClass="TextForAmount"
                                             > </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllocationQty"   EnableViewState="true" ReadOnly="true" runat="server" CssClass="TextForAmount"
                                             > </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                   <asp:TemplateField HeaderText="Cancel" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);" />
                                                    <asp:Label ID="lblDelete" runat="server" Text="Cancel"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
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
                                            <asp:TextBox ID="txtClosureID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>

                                        </ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Loss Reason" ItemStyle-Width="10%">
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
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                     <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                    

                <asp:TextBox ID="txtRFPID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>

                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
