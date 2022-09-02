<%@ Page Title="MTI-SAP Order Info" Language="C#" MasterPageFile="~/Header.Master"
    EnableViewState="true" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="frmSAPOrderInfo.aspx.cs" Inherits="MANART.Forms.Common.frmSAPOrderInfo" %>


<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%--<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>--%>
<%--<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <%--<script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsIndentConsolidation.js"></script>--%>
         
    <script type ="text/javascript" >

        function pageLoad() {
            $(document).ready(function () {
                var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
                var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
                $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: '0d'
                });

                $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: '0d'
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
     
     <script type ="text/javascript" >
         function ShowSAPDocumentNo(obj) {
             var a = obj;
         }

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
              <asp:Panel ID="OldData" Visible="false" runat="server">
            <table id="PageTbl" class="PageTable" border="1">
                <tr id="TitleOfPage" class="panel-heading">
                    <td class="PageTitle panel-title" align="center" style="width: 14%">
                        <asp:Label ID="lblTitle" runat="server" Text="SAP Order Info"> </asp:Label>
                    </td>
                </tr>
                <tr id="TblControl">
                    <td style="width: 14%">
                        <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" Visible="false">
                    <table id="tblLocationDetails" runat="server" class="ContainTable" >
    
                        <tr align="left">
                                    <td class="tdLabel" style="width: 8%">
                                        Region:
                                    </td>
                                    <td class="tdLabel" style="width: 25%" >
                                        <div id="dvTopReg" class="dt" style="width: 80%;">
                                            <asp:CheckBox ID="ChkAllReg" runat="server"  
                                                  AutoPostBack ="true" /> 
                                            <asp:TextBox ID="txtRegion" ReadOnly="True" runat="server" class="mstbm" onmouseover="DisplayTitleReg(this);"
                                                Width="87%" Height="16px">--Select--</asp:TextBox>
                                        </div>
                                        <div id="divMainReg" runat="server" class="dvmain" style="width: 80%;">
                                            <img class="nicheimage" src="../Images/niche.gif" style="display: none" />
                                            <asp:CheckBoxList ID="ChkRegion" runat="server" Width="87%" OnSelectedIndexChanged="ChkRegion_CheckedChanged" AutoPostBack ="true"  >
                                            </asp:CheckBoxList>
                                            <asp:LinkButton ID="lnkMainReg" runat="server" Text="Close Me"></asp:LinkButton>
                                        </div>
                                    </td>
                                    <td class="tdLabel" style="width: 7%">
                                        <asp:Label ID="lblStateCountry" runat ="server" Text ="State"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 29%">
                                        <div id="dvTopDpo" class="dt" style="width: 80%;">
                                            <asp:CheckBox ID="ChkAllDpo" runat="server"  AutoPostBack ="true" OnCheckedChanged="ChkAllDpo_CheckedChanged" /> 
                                            <asp:TextBox ID="txtDepoName" ReadOnly="True" runat="server" class="mstbm" onmouseover="DisplayTitleDpo(this);"
                                                Width="87%" Height="16px">--Select--</asp:TextBox>
                                        </div>
                                        <div id="divMainDpo" runat="server" class="dvmain" style="width: 80%;">
                                            <img class="nicheimage" src="../Images/niche.gif" style="display: none" />
                                            <asp:CheckBoxList ID="ChkDepo" runat="server" Width="87%" AutoPostBack ="true" OnSelectedIndexChanged="ChkDepo_CheckedChanged"> 
                                            </asp:CheckBoxList>
                                            <asp:LinkButton ID="lnkMainDpo" runat="server" Text="Close Me"></asp:LinkButton>
                                        </div>
                                    </td>
                                   <td style="width: 15%">
                                    </td>
                                    <td style="width: 15%">
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td class="tdLabel" style="width: 8%">                                   
                                        <asp:Label ID="lblDealer" runat ="server" Text ="Dealer Name"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 25%" >
                                        <div id="dvTopDlr" class="dt" style="width: 80%;">
                                            <asp:CheckBox ID="ChkAllDlr" runat="server" onClick="return SelectAllDlr();" />
                                            <asp:TextBox ID="txtDealerName" ReadOnly="True" runat="server" class="mstbm" onmouseover="DisplayTitleReg(this);"
                                                Width="87%" Height="16px">--Select--</asp:TextBox>
                                        </div>
                                        <div id="divMainDlr" runat="server" class="dvmain" style="width: 80%;">
                                            <img class="nicheimage" src="../Images/niche.gif" style="display: none" />
                                            <asp:CheckBoxList ID="ChkDealer" runat="server" Width="87%" AutoPostBack ="true" OnSelectedIndexChanged="ChkDealer_CheckedChanged">
                                            </asp:CheckBoxList>
                                            <asp:LinkButton ID="lnkMainDlr" runat="server" Text="Close Me"></asp:LinkButton>
                                        </div>
                                    </td>
                                     <td style="width: 7%">
                                    </td>
                                    <td align="left" class="tdLabel" style="width: 29%;display :none">
                                        &nbsp;
                                    </td>
                                   <td style="width: 25%">
                                        &nbsp;
                                    </td>
                                   
                        
                                </tr>
                    </table>
                </asp:Panel>
                        <asp:Panel ID="Selection" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="Table2" runat="server" class="ContainTable" border="0">
                                <tr>                                    
                                    
                                    <td class="tdLabel" style="width: 15%">
                                        Document Type:
                                    </td>
                                    <td style="width: 18%">
                                       <asp:DropDownList ID="drpClaimType" runat="server" CssClass="ComboBoxFixedSize" 
                                            AutoPostBack ="true" onselectedindexchanged="drpClaimType_SelectedIndexChanged">
                                       <%--<asp:ListItem Value="All" Selected="True" >ALL</asp:ListItem>--%>
                                       <%--<asp:ListItem Value="WC" >Warranty Claim</asp:ListItem>--%>
                                       <%--<asp:ListItem Value="WR" >Claim Request</asp:ListItem>--%>
                                       <%--<asp:ListItem Value="COU" >Coupon Claim</asp:ListItem>--%>
                                       <asp:ListItem Value="SPO" Selected="True" >Spare PO</asp:ListItem>
                                       <%--<asp:ListItem Value="VDA" >Vehicle DA</asp:ListItem>--%>
                                       <%--<asp:ListItem Value="INS" >INS</asp:ListItem>--%>
                                       <%--<asp:ListItem Value="FPDA" >FPDA</asp:ListItem>--%>
                                       </asp:DropDownList>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        Dealer Type:
                                    </td>
                                    <td style="width: 18%">
                                         <asp:DropDownList ID="drpDomExp" runat="server" CssClass="ComboBoxFixedSize" 
                                             AutoPostBack ="true" onselectedindexchanged="drpDomExp_SelectedIndexChanged">
                                       <asp:ListItem Value="D" Selected="True" >Domestic</asp:ListItem>
                                       <asp:ListItem Value="E" >Export</asp:ListItem>
                                        
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id ="trDate" runat ="server" >
                                    <td class="tdLabel" style="width: 15%">
                                        From:
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="txtFromDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                    </td>
                                    <td style="width: 15%" class="tdLabel">
                                        To:
                                    </td>
                                    <td style="width: 18%">
                                        <uc3:CurrentDate ID="txtToDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        &nbsp;<asp:Button ID="btnShow" runat="server" CssClass="CommandButton" OnClick="btnShow_Click"
                                            Text="Show" />
                                    </td>
                                   <td class="tdLabel" style="width: 15%">                                  
                                        <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>                                    
                                    </td>
                                    
                                    <td style="width: 18%" colspan ="2">                                        
                                        &nbsp;
                                    </td>
                                    <td class="tdLabel" style="width: 15%" >
                                      &nbsp;
                                    </td>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Vertical">
                            <table id="Table1" runat="server" class="ContainTable" border="1">
                                <tr>                                
                                    <td id="Td1" align="center"  style="height: 15px">
                                   Search By <asp:DropDownList ID ="drpSearch" runat ="server" CssClass ="ComboBoxFixedSize">
                                   <asp:ListItem Selected ="True" Value ="Document_No">Document No</asp:ListItem>    
                                    <asp:ListItem Value ="Posted_SAP">Posted to SAP</asp:ListItem> 
                                     <asp:ListItem Value ="Exception">Exception</asp:ListItem> 
                                     <asp:ListItem Value ="SAP_Order_No">SAP Order No</asp:ListItem>
                                     <asp:ListItem Value ="Credit_Note_No">Credit Note No/SAP Invoice No</asp:ListItem>
                                     <asp:ListItem Value ="Vehicle_Invoice_No">Vehicle Invoice No</asp:ListItem>
                                        </asp:DropDownList><asp:TextBox ID="txtSearchText" runat ="server" CssClass ="TextBoxForString " ></asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" CssClass="CommandButton"  
                                            Text="Search" onclick="btnSearch_Click" OnClientClick ="return CheckText()"  />
                                        <asp:Button ID="btnClearSearch" runat="server" CssClass="CommandButton" Text="Clear Search" OnClientClick ="return ClearSearch()" onclick="btnClearSearch_Click"/>
                                          <asp:Label ID="lblMessage1" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                    </td>                                                                      
                                </tr>
                                <tr>
                                    <td id="ClaimGrid" align="center" class="ContaintTableHeader" style="height: 15px">
                                   
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td style="width: 18%;" align="center">
                                        <asp:Panel ID="GridDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                            ScrollBars="Vertical">
                                            <asp:GridView ID="gvGrid" GridLines="Horizontal" Width="100%"
                                                     AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                                    AutoGenerateColumns="False" AllowPaging="true"
                                                onpageindexchanging="gvGrid_PageIndexChanging" runat="server" 
                                                onrowcommand="gvGrid_RowCommand"  >
                                                <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                <Columns>
                                                                                  
                                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>                                                    
                                                     <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="21%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer Name") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document No" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimNo" runat="server" Text='<%# Eval("Document_No") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Date" ItemStyle-Width="9%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocumentDate" runat="server" Text='<%# Eval("Document Date") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="9%" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Document Type" ItemStyle-Width="9%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimDate" runat="server" Text='<%# Eval("Document_Type") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="9%" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Posted to SAP" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPostedtoSAP" runat="server" Text='<%# Eval("Posted_SAP") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="8%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exception" ItemStyle-Width="7%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblException" runat="server" Text='<%# Eval("Exception") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SAP Order No" ItemStyle-Width="6%" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSAPOrderNo" runat="server" Text='<%# Eval("SAP_Order_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="6%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SAP Order Date" ItemStyle-Width="6%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSAPOrderDate" runat="server" Text='<%# Eval("SAP Order Date") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="6%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit Note No/SAP Invoice No" ItemStyle-Width="7%" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreditNoteNo" runat="server" Text='<%# Eval("Credit_Note_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="7%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit Note Date/SAP Invoice Date" ItemStyle-Width="9%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreditNoteDate" runat="server" Text='<%# Eval("Credit Note Date") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="9%" />
                                                    </asp:TemplateField>   
                                                     <asp:TemplateField HeaderText="Vehicle Invoice No" ItemStyle-Width="9%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVehicleInvoiceNo" runat="server" Text='<%# Eval("Vehicle_Invoice_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="9%" />
                                                    </asp:TemplateField>                                                       
                                                    
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                 <tr>
                                    <td id="Td2" align="center" class="ContaintTableHeader" style="height: 15px">
                                   
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td style="width: 18%;" align="center">
                                        <asp:Panel ID="pnlGridDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                            ScrollBars="Vertical">
                                            <asp:GridView ID="gvGridDtls" GridLines="Horizontal" Width="100%"
                                                     AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                                    AutoGenerateColumns="true" AllowPaging="true" runat ="server"                                                
                                                    PageSize="5" 
                                                  >                                                  
                                                <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                              <Columns >
                                              <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                              </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 14%">
                    </td>
                </tr>
                <tr id="TmpControl" >
                    <td style="width: 14%;display :none">
                        <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                            Text=""></asp:TextBox>
                        <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtRequestOrClaim" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtDealerIDs" runat="server" Width="1px" Text=""></asp:TextBox>
                        
                        <asp:HiddenField ID="hdnDocumentNo" runat="server"  Value="" />
                        <asp:HiddenField ID="hdnDocumentType" runat="server" Value="" />
                        
                        
                         <input id="hapbDlr" type="hidden" name="tempHiddenField" runat="server" />
                        <input id="hsivDlr" type="hidden" name="hsiv" runat="server" />
                        <input id="txtControl_IDDlr" type="hidden" name="__EA1" runat="server" />
                        <input id="hapbReg" type="hidden" name="tempHiddenField" runat="server" />
                        <input id="hsivReg" type="hidden" name="hsiv" runat="server" />
                        <input id="txtControl_IDReg" type="hidden" name="__EA1" runat="server" />
                        <input id="hapbDpo" type="hidden" name="tempHiddenField" runat="server" />
                        <input id="hsivDpo" type="hidden" name="hsiv" runat="server" />
                        <input id="txtControl_IDDpo" type="hidden" name="__EA1" runat="server" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="tblUpdateChassisNO" runat="server" BorderColor="Black" BorderStyle="Double" Visible="false">
            <table id="tblDoc" class="ContainTable" border="0">
                <tr id="Tr1" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 100%" colspan=6 >
                <asp:Label ID="Label1" runat="server" Text="DCS Doccument Status"> </asp:Label>
                </td>
                </tr>
                <tr>
                <td align="center" class="ContaintTableHeader" width="100%"   colspan=6>
                <asp:Label ID="Label3" runat="server" Text="Update Chassis No" Width="96%"  Height="16px"></asp:Label>
                </td>
                </tr>            
                <tr>
                <td class="tdLabel" style="width: 15%">
                Incorrect Chassis No:
                </td>  
                <td  style="width: 25%" >
                <asp:TextBox ID="txtIncorrectCHassiNo" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="150px"  ></asp:TextBox>
                <asp:Label ID="lblBoxes" runat="server" Text="*" ForeColor="Red"></asp:Label>                              
                </td>
                <td class="tdLabel" style="width: 15%">
                Correct Chassis No:
                </td>
                <td  style="width: 25%"  >
                <asp:TextBox ID="txtcorrectCHassiNo" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="150px" ></asp:TextBox>
                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>                              
                </td>  
                <td style="width: 15%" >
                <asp:Button ID="btnChassiNo" runat="server" CssClass="CommandButton" 
                        Text="Update Chassis" onclick="btnChassiNo_Click" />                
                </td> 
                <td style="width: 15%"   >
                <asp:Button ID="btnChsNoWthINSPostFlag" runat="server" CssClass="CommandButton" 
                        Text="Update Chassis&INS post Flag" onclick="btnChsNoWthINSPostFlag_Click" />
                <asp:Label ID="lblUpdateChassisDetails" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                </td>                    
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="tblDocStatus" runat="server" BorderColor="Black" BorderStyle="Double" Visible="false">
             <table id="Table4" runat="server" class="ContainTable" border="0" visible="false">
             <tr class="panel-heading">
              <td align="center" class="ContaintTableHeader panel-title" width="96%" colspan=8>
                <asp:Label ID="Label4" runat="server" Text="DCS-SAP Document posted Status" Width="96%"  Height="16px"></asp:Label>                             
              </td>  
             </tr>
             <tr>                
                <td class="tdLabel" style="width: 15%">
                Document Type:
                </td>                
                <td id="Td3" align="left"  style="height: 15px">
                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="ComboBoxFixedSize" 
                AutoPostBack ="true" >                
                <asp:ListItem Value="WC" >Warranty Claim/AMC Claim</asp:ListItem>                
                <asp:ListItem Value="COU" >Coupon Claim</asp:ListItem>
                <asp:ListItem Value="SPO" >Spare PO</asp:ListItem>
                <asp:ListItem Value="VDA" >Vehicle DA</asp:ListItem>
                <asp:ListItem Value="INS" >INS</asp:ListItem>
                 <asp:ListItem Value="FPDA" >FPDA</asp:ListItem>
                </asp:DropDownList>
                </td>
                <td class="tdLabel" style="width: 15%">
                Dlr Code:
                </td>   
                <td class="tdLabel" style="width: 15%">
                <asp:TextBox ID="txtDlrCode" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="15" Width="100px" ></asp:TextBox>              
                </td>
                <td class="tdLabel" style="width: 10%">
                Document No:
                </td>   
                <td class="tdLabel" style="width: 25%">
                <asp:TextBox ID="txtDocNo" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px" ></asp:TextBox>&nbsp                
                </td> 
                <td style="width: 15%" >         
                <asp:Button ID="btnDLDealerLiveUpdate" runat="server" CssClass="CommandButton" 
                    Text="Get Records" onclick="btnDLDealerLiveUpdate_Click" />
                </td>                
                <td style="width: 15%" >                                         
                <asp:Button ID="btnClearRecords" runat="server" CssClass="CommandButton" 
                        Text="Clear Records" onclick="btnClearRecords_Click"  />                        
                <asp:Label ID="lblNorecords" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                </td>            
               </tr>
            <tr>
                <td style="width: 18%;" align="center" colspan="8">                                
                        <asp:GridView ID="grdDocDeatils"  runat="server" Width="100%" AutoGenerateColumns="False"
                        AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" /> 
                            <Columns>
                                <asp:TemplateField HeaderText="Dlr Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDlr_Code" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Dlr_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dlr Live Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDlrLiveDate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DlrLiveDate") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Doc Type" ItemStyle-Width="0%" Visible=false>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_TypeValue" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Doc_TypeValue") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="0%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Doc Type" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Type" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Doc_Type") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Doc_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Doc_Date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="DCS Cr Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCS_Cr_date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DCS_Cr_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Processed status" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcessedstatus" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Processedstatus") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="SAP Post Status" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSAPPostStatus" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("SAPPostStatus") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />                                    
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="SAP Order No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSAP_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("SAP_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />                                    
                                </asp:TemplateField>         
                                <asp:TemplateField HeaderText="Re-Post to SAP" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnUptXMLGenFlag"  runat="server" CssClass="CommandButton" Text="Update XML Gen Flag"
                                        OnClick="btnUpdateXMLGenFlag" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SAP Input XML" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRetXML"  runat="server" CssClass="CommandButton" Text="Retrive I/P XML Data"
                                        OnClick="btnretiveInputXML" />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>                                
                            </Columns>
                        </asp:GridView>                            
                </td>
                </tr>
             </table>
             </asp:Panel>
            <asp:Panel ID="tblDCSDocStatus" runat="server" BorderColor="Black" BorderStyle="Double">
             <table id="Table3" runat="server" class="ContainTable" border="0">
             <tr class="panel-heading">
              <td align="center" class="ContaintTableHeader panel-title" width="96%" colspan="6" >
                <asp:Label ID="Label2" runat="server" Text="DCS-DMS Document posted Status" Width="96%"  Height="16px"></asp:Label>                             
              </td>  
             </tr>
             <tr>                
                <td class="tdLabel" style="width: 15%">
                Document Type:
                </td>                
                <td id="Td4" align="left"  style="height: 15px">
                <asp:DropDownList ID="drpDMSDocStats" runat="server" CssClass="ComboBoxFixedSize" 
                AutoPostBack ="true"    onselectedindexchanged="drpDMSDocStats_SelectedIndexChanged"  >                
                <asp:ListItem Value="SPInv" Selected="True" >Spare Invoice</asp:ListItem>                
                <%--<asp:ListItem Value="VHInvP" >Vehicle Invoice PDI</asp:ListItem>--%>
                <%--<asp:ListItem Value="VHInvD" >Vehicle Invoice Dealer</asp:ListItem>--%>
                <%--<asp:ListItem Value="WCCr" >Warranty Credit</asp:ListItem>--%>
                <%--<asp:ListItem Value="RQApp" >Request Approval</asp:ListItem>--%>                
                </asp:DropDownList>
                </td>
                <td class="tdLabel HideControl" style="width: 10%" >                
                <asp:label id="Label7" runat="server" Text="Document No:"></asp:label>                                               
                </td>   
                <td class="tdLabel HideControl " style="width: 25%">
                <asp:TextBox ID="txtDMSDocNo" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px" ></asp:TextBox>
                </td>
                <td style="width: 15%" >         
                <asp:Button ID="btnDMSStatus" runat="server" CssClass="CommandButton"  
                    Text="DMS XML" onclick="btnDMSStatus_Click" />
                   
                </td>                
                <td style="width: 15%" >                                         
                <asp:Button ID="btnDMSClearRecords" runat="server" CssClass="CommandButton"  Visible="false"
                        Text="Clear Records" onclick="btnDMSClearRecords_Click"  />                        
                <asp:Label ID="lblDMSNorecords" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                </td>            
               </tr>
               <tr>            
               <td></td>
               <td></td>
               <td class="tdLabel" style="width: 10%">
                <asp:label id="lblChassisOr" runat="server" Text="Chassis No:"></asp:label>                               
               </td>
               <td class="tdLabel" style="width: 25%">
                <asp:TextBox ID="txtChassisNo" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px" ></asp:TextBox>                               
               </td>
               <td></td>
               <td></td>
               </tr>
               <tr>
               <td style="width: 18%;" align="center" colspan="6">               
                        <asp:GridView ID="grdDMSDocDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                        AllowPaging="True" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                        EditRowStyle-BorderColor="Black" PageSize="5" 
                            onpageindexchanged="grdDMSDocDetails_PageIndexChanged" 
                            onpageindexchanging="grdDMSDocDetails_PageIndexChanging">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />                            
                            <Columns>
                                <asp:TemplateField HeaderText="Dlr Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDlr_Code" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Dlr_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Type" ItemStyle-Width="0%" Visible=false>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_TypeValue" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Doc_TypeValue") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="0%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Doc Type" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Type" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Doc_Type") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Doc_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Doc_Date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="DCS Cr Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCS_Cr_date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DCS_Cr_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DMS Post Status" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSAPPostStatus" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DMSPostStatus") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />                                    
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Re-Post to DMS" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnUptDMSXMLGenFlag"  runat="server" CssClass="CommandButton" Text="Update XML Gen Flag"
                                        OnClick="btnUptDMSXMLGenFlag" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                            </Columns>                        
                        </asp:GridView>                              
                        
               </td>
               </tr>               
             </table>            
            </asp:Panel> 
                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double" Visible="false">
             <table id="Table5" runat="server" class="ContainTable" border="0">
             <tr class="panel-heading">
              <td align="center" class="ContaintTableHeader panel-title" width="96%" colspan="6" >
                <asp:Label ID="Label6" runat="server" Text="DCS-DMS Jobcard Hrs/KM Update Status " Width="96%"  Height="16px"></asp:Label>                             
              </td>  
             </tr>
             <tr>                
                
                <td class="tdLabel" style="width: 5%; height: 23px;">                
                <asp:label id="lbljobNo" runat="server" Text="Jobcard No:"></asp:label>                                               
                </td>   
                <td class="tdLabel" style="width: 10%; height: 23px;">
                <asp:TextBox ID="txtjobcardNo" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px" ></asp:TextBox>
                </td>
                <td class="tdLabel" style="width: 10%; height: 23px;">                
                <asp:label id="lbldlrcode" runat="server" Text="Dealer Code :"></asp:label>                                               
                </td>   
                <td class="tdLabel" style="width: 10%; height: 23px;">
                <asp:TextBox ID="txtDealerCode"  Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px" ></asp:TextBox>
                </td>
                <td style="width: 15%; height: 23px;" >         
                <asp:Button ID="btnGetJobcard" runat="server" CssClass="CommandButton" 
                    Text="Get Records" onclick="btnGetJobcard_Click" />
                    
                </td>                
                <td style="width: 15%; height: 23px;" >                                         
                <asp:Button ID="btnDMSClearJobcardRecords" runat="server" CssClass="CommandButton" 
                        Text="Clear Records" onclick="btnDMSClearJobcardRecords_Click"  />                        
               <asp:Label ID="lblclearjobcard" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                </td>            
               </tr>
               <tr>            
                 <td class="tdLabel" style="width: 10%">
                <asp:label id="lblkms" runat="server" Text="Kms:" Visible="false"></asp:label>                               
               </td>
               <td class="tdLabel" style="width: 10%">
                <asp:TextBox ID="txtKms" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px"  Visible="false"></asp:TextBox>                               
               </td>
                <td class="tdLabel" style="width: 10%">
                <asp:label id="lblHrs" runat="server" Text="Hrs:" Visible="false"></asp:label>                               
               </td>
               <td class="tdLabel" style="width: 10%">
                <asp:TextBox ID="txtHrs" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="30" Width="172px" Height="16px" Visible="false" ></asp:TextBox>                               
               </td>
               <td style="width: 15%"  >         
                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpdateJobcard" runat="server" CssClass="CommandButton"  Visible="false"
                    Text="Update Hrs/KM" onclick="btnUpdateJobcard_Click" />
                </td>           
               </tr>
               <tr>
               <td style="width: 18%;" align="center" colspan="6">   
               <asp:GridView ID="grdjobcardDetails" runat="server" AllowPaging="True" 
                            AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False" 
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" 
                            onpageindexchanged="grdjobcardDetails_PageIndexChanged" 
                            onpageindexchanging="grdjobcardDetails_PageIndexChanging" 
                             PageSize="5" 
                            Width="100%">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Dlr Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDlr_Code" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Dlr_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Type" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Type" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Doc_Type") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jobcard No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Doc_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jobcard Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Date" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Doc_Date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kms" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Kms") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Hrs" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Hrs") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Chassis_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Engine_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Vehicle_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DCS Cr Date" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCS_Cr_date" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("DCS_Cr_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>            
                        &nbsp;</td>
               </tr>               
             </table>            
            </asp:Panel> 
            
            <asp:Panel ID="PChassisInsert" runat="server" BorderColor="Black" BorderStyle="Double" Visible="false">
             <table id="Table6" runat="server" class="ContainTable" border="0">
             <tr class="panel-heading">
              <td align="center" class="ContaintTableHeader panel-title" width="96%" colspan="6" >
                <asp:Label ID="Label8" runat="server" Text="Chassis Insertion" Width="96%"  Height="16px"></asp:Label>                             
              </td>  
             </tr>
             <tr>                
                
                <td class="tdLabel" style="width: 5%; height: 23px;">                
                <asp:label id="Label9" runat="server" Text="Chassis No:"></asp:label>                                               
                </td>   
                <td class="tdLabel" style="width: 10%; height: 23px;">
                <asp:TextBox ID="txtchassis" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="20" Width="160px" Height="16px" ></asp:TextBox>
                <b class="Mandatory">*</b> 
                </td>
                <td class="tdLabel" style="width: 5%; height: 23px;">                
                <asp:label id="Label10" runat="server" Text="Engine No :"></asp:label>                                               
                </td>   
                <td class="tdLabel" style="width: 10%; height: 23px;">
                <asp:TextBox ID="txtEngineNo"  Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="20" Width="160px" Height="16px" ></asp:TextBox> <b class="Mandatory">*</b> </td> 
                <td style="width: 15%; height: 23px;" >         
                &nbsp;&nbsp; <asp:Button ID="btnInsertChassis" runat="server" CssClass="CommandButton btn btn-primary btn-xs" 
                    Text="Insert Chassis" onclick="btnInsertChassis_Click" />
                </td>                
                <td style="width: 15%; height: 23px;" >                                         
                <asp:Button ID="Button2" runat="server" CssClass="CommandButton"  Visible="false" 
                        Text="Clear Records" onclick="btnDMSClearJobcardRecords_Click"  />                        
               <asp:Label ID="Label11" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                </td>            
               </tr>
               <tr>            
                             <td class="tdLabel" style="width: 5%">
                <asp:label id="Label12" runat="server" Text="Model Code:"></asp:label>                               
               </td>
               <td class="tdLabel" style="width: 10%">
                <asp:TextBox ID="txtModelcode" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="18" Width="160px" Height="16px" ></asp:TextBox>     
                 <b class="Mandatory">*</b>         
               </td>
              
                <td class="tdLabel" style="width: 5%">
                <asp:label id="Label13" runat="server" Text="Vehicle No:" ></asp:label>                               
               </td>
               <td class="tdLabel" style="width: 10%">
                <asp:TextBox ID="txtvehicleno" Text="" runat="server" CssClass="TextBoxForString"
                MaxLength="12" Width="160px" Height="16px" ></asp:TextBox>
                  <%--<b class="Mandatory">*</b>          --%>                     
               </td>
               <td style="width: 15%"  >         
               
                </td>  
                 <td style="width: 15%"  >         
               
                </td>           
               </tr>
               <tr>
               <td style="width: 18%;" align="center" colspan="6" visible ="false">   
               <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                            AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False" 
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" 
                            onpageindexchanged="grdjobcardDetails_PageIndexChanged" 
                            onpageindexchanging="grdjobcardDetails_PageIndexChanging" 
                             PageSize="5" 
                            Width="100%">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Dlr Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDlr_Code" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Dlr_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Type" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Type" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Doc_Type") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jobcard No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Doc_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jobcard Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Date" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Doc_Date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kms" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Kms") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Hrs" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Hrs") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Chassis_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Engine_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_No" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("Vehicle_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DCS Cr Date" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCS_Cr_date" runat="server" CssClass="LabelCenterAlign" 
                                            Text='<%# Eval("DCS_Cr_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>            
                        &nbsp;</td>
               </tr>               
             </table>            
            </asp:Panel> 
    </div>
    <script type ="text/javascript" >

        function ClearSearch() {
            document.getElementById("<%=txtSearchText.ClientID %>").value = '';
    }
    function CheckText() {
        var txtSearchText = document.getElementById("<%=txtSearchText.ClientID %>")
        if (txtSearchText.value == '')
            return false;
    }
    </script>
</asp:Content>
