<%@ Page Title="Discount Approval" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmDiscountApproval.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmDiscountApproval" %>


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
    <script>
        function Finalamoutcalulate() {
            //debugger;
            var TotalDisc = $('#ContentPlaceHolder1_txtAppDisc').val();
            var Dealershare = $('#ContentPlaceHolder1_txtAppDealershare').val();
            var MTIShare = $('#ContentPlaceHolder1_txtAppMTIshare').val();
            $('#ContentPlaceHolder1_txtAppDealershare').val(0);
            $('#ContentPlaceHolder1_txtAppMTIshare').val(0);
        }

        function FinalamoutcalulateDealerShare() {
            //debugger;
            var TotalDisc = $('#ContentPlaceHolder1_txtAppDisc').val();
            var Dealershare = $('#ContentPlaceHolder1_txtAppDealershare').val();
            var MTIShare = $('#ContentPlaceHolder1_txtAppMTIshare').val();
            //debugger;

            if (TotalDisc > 0) {

                ////if (Dealershare > TotalDisc) {
                ////    $('#ContentPlaceHolder1_txtAppDealershare').val(0);
                ////    $('#ContentPlaceHolder1_txtAppMTIshare').val(0);
                ////}
                //else {
                 
                    var FinalMTIShare = TotalDisc - Dealershare;
                    $('#ContentPlaceHolder1_txtAppMTIshare').val(FinalMTIShare);
                //}

            }
        }

        function FinalamoutcalulateMTIShare() {
            //debugger;
            var TotalDisc = $('#ContentPlaceHolder1_txtAppDisc').val();
            var Dealershare = $('#ContentPlaceHolder1_txtAppDealershare').val();
            var MTIShare = $('#ContentPlaceHolder1_txtAppMTIshare').val();

            //debugger;
            if (TotalDisc > 0) {

                //if (MTIShare > TotalDisc) {
                //    $('#ContentPlaceHolder1_txtAppDealershare').val(0);
                //    $('#ContentPlaceHolder1_txtAppMTIshare').val(0);
                //}
                //else {
                    var FinalDealerShare = TotalDisc - MTIShare;
                    $('#ContentPlaceHolder1_txtAppDealershare').val(FinalDealerShare);
                //}

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
                    <asp:Label ID="lblTitle" runat="server" Text="Discount Approval"> </asp:Label>
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


                                    <td style="width: 15%" class="tdLabel">Type:
                                    </td>
                                    <td style="width: 15%">
                                        <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                          onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                             AutoPostBack="true"  >
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="drpM0CustType" runat="server" AutoPostBack="True"
                                            CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                    </td>

                                </tr>

                                <tr>


                                    <td style="width: 15%" class="tdLabel">Title:
                                    </td>
                                    <td style="width: 15%">

                                        <asp:DropDownList ID="drpTitle" runat="server" AutoPostBack="True"
                                            CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label25" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                    </td>

                                    <td style="width: 15%" class="tdLabel">Name:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label26" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
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
                            SuppressPostBack="true" CollapsedText="M1 (Enquiry Generated) Details" ExpandedText="M1 (Enquiry Generated) Details"
                            TextLabelID="lblTtlM1">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlM1" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">

                                        <asp:Label ID="lblTtlM1" runat="server" Text="M1 Details" Width="96%"
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
                                        <uc3:CurrentDate ID="txtDocDate" runat="server" bCheckforCurrentDate="false" />

                                    </td>
                                    <td class="tdLabel">PO Type:
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
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">Model
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
                            ExpandControlID="TtlQuotation" CollapseControlID="TtlQuotation" Collapsed="false"
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
                                    <td class="tdLabel">M2 No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtM2No" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>

                                    <td>

                                        <asp:Label ID="Label4" runat="server" Text="M2 Date:" CssClass="tdLabel"></asp:Label></td>
                                    <td>
                                        <uc3:CurrentDate ID="txtM2Date" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="tdLabel">Quotation No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQutNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>

                                    <td>

                                        <asp:Label ID="Label24" runat="server" Text="Quotation Date:" CssClass="tdLabel"></asp:Label></td>
                                    <td>
                                        <uc3:CurrentDate ID="txtqutdate" runat="server" Mandatory="False" bCheckforCurrentDate="false" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="tdLabel">Competitor:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCompetitor" runat="server" CssClass="ComboBoxFixedSize">
                                        </asp:DropDownList>

                                    </td>
                                    <td class="tdLabel">Competitor Model:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompModel" runat="server" CssClass="TextBoxForString"></asp:TextBox>

                                    </td>
                                    <td class="tdLabel">Proposed Discount:
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
                                            <asp:TextBox ID="txtQuotValue" runat="server" CssClass="TextForAmount" Text='<%# Eval("Value","{0:#0}") %>' EnableViewState="false"
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
                                        <asp:Label ID="Label1" runat="server" Text="Total Approved Discount:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAppDisc" runat="server" onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextForAmount" Text='<%# Eval("Value","{0:#0}") %>'
                                            onblur=" return Finalamoutcalulate();"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Approved Dealer Share:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAppDealershare" runat="server"
                                            onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextForAmount" Text='<%# Eval("Value","{0:#0}") %>'
                                            onblur=" return FinalamoutcalulateDealerShare();"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Approved MTI Share:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAppMTIshare" runat="server"
                                            CssClass="TextForAmount" onkeypress=" return CheckForTextBoxValue(event,this,'6');" Text='<%# Eval("Value","{0:#0}") %>'
                                            onblur=" return FinalamoutcalulateMTIShare();"></asp:TextBox>

                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Remarks:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAppremarks" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>

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
                                Text=""></asp:TextBox><asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                                    Text=""></asp:TextBox><asp:TextBox ID="txtM1ID" runat="server" CssClass="DispalyNon" Width="1px"
                                        Text=""></asp:TextBox><asp:TextBox ID="txtM2ID" runat="server" CssClass="DispalyNon" Width="1px"
                                            Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                                                Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                     <asp:TextBox ID="txtRFPID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
