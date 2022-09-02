<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmFPDAFORReceipt.aspx.cs"
    Theme="SkinFile" Inherits="MANART.Forms.Warranty.frmFPDAFORReceipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="~/WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="CustomPager"  %>
<%@ Register Src="~/WebParts/MultiselectLocation.ascx" TagPrefix="uc2" TagName="MultiselectLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
      <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />     
    <link href="../../Content/CustomPager.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsWarrantyProcessing.js"></script>
    <script src="../../Scripts/jsWCPartFunc.js"></script>      
    <script src="../../Scripts/jsToolbarFunction.js"></script>   
    <script type ="text/javascript" >

        function pageLoad() {
            $(document).ready(function () {
                var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
                var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
                $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value
                });

                $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtFromDate.value
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

        function GetFPDADtls(objLink, sDealerId,  iFPDAHDRID, sDealerCode) {
            //var iJob_HDR_ID = 0;
            var objRow = objLink.parentNode.parentNode.childNodes;
            debugger;
            var Parameters = "FPDAHDRID=" + iFPDAHDRID + "&DealerID=" + sDealerId + "&sDealerCode=" + sDealerCode;
            var feature = "dialogWidth:1300px;dialogHeight:900px;status:no;help:no;scrollbars:no;resizable:no;";
            var FPDAId;
            FPDAId = window.showModalDialog("../Warranty/frmFPDAReceipt.aspx?" + Parameters, null, feature);
            if (FPDAId != null) {

            }
            return;
        }

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
                <tr id="TitleOfPage" class="panel-heading">
                    <td class="PageTitle panel-title" align="center" style="width: 14%">
                        <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                    </td>
                </tr>
                <tr id="TblControl">
                    <td style="width: 14%">
                        <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <uc2:Location ID="Location" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="Selection" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="Table2" runat="server" class="ContainTable" border="0">
                                <tr>
                                    <td style="width: 15%" class="tdLabel">
                                        FPDA Status:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="drpClaimStatus" runat="server" CssClass="ComboBoxFixedSize" onchange="OnStateChange()">
                                            <asp:ListItem Text="All" Value="99"></asp:ListItem>
                                            <asp:ListItem Text="Pending For Receipt" Value="1" ></asp:ListItem>
                                            <asp:ListItem Text="Receipt Confirmed" Value="2" ></asp:ListItem>
                                            <asp:ListItem Text="Receipt Saved" Value="3" ></asp:ListItem>
                                        </asp:DropDownList>
                                        <b class="Mandatory">*</b>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">                                        
                                    </td>
                                    <td style="width: 18%">                                       
                                    </td>
                                   <td class="tdLabel" style="width: 15%">                                       
                                    </td>
                                    <td style="width: 18%">                                       
                                    </td>
                                </tr>
                                <tr>                                 
                                     <td class="tdLabel" style="width: 15%">                                        
                                    </td>
                                    <td style="width: 18%">                                       
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
                                    
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 12%" class="tdLabel">
                                        <asp:TextBox ID="txtClr2" runat="server" CssClass="TextBoxForString" Width="5%" BackColor="#FFFF66" Visible="false"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblClr2" runat="server" Visible="false" ></asp:Label><%--Text="Claim Below 5000 Amt"--%>
                                    </td>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:Button ID="btnShow" runat="server" CssClass="btn btn-search btn-sm" OnClick="btnShow_Click"
                                            Text="Show" />
                                    </td>
                                    <td style="width: 18%" colspan ="2">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                        &nbsp;&nbsp;</td>
                                    <td class="tdLabel" style="width: 15%" >
                                      &nbsp;
                                    </td>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id ="trRejRetStatus" runat ="server" >
                                    <td class="tdLabel" style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 12%" class="tdLabel">
                                        <asp:TextBox ID="txtRetStatus" runat="server" CssClass="TextBoxForString" Width="5%" BackColor="#ffe6e6" Visible="false"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblRetStatus" runat="server" Text="Return" Visible="false"></asp:Label>
                                    </td>                                    
                                    <td style="width: 12%" class="tdLabel">
                                        <asp:TextBox ID="txtRejStatus" runat="server" CssClass="TextBoxForString" Width="6%" BackColor="#ff9797" Visible="false"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblRejStatus" runat="server" Text="Reject"></asp:Label>
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
                                        Search By FPDA/ Receipt No  <asp:TextBox ID="txtSearchText" runat ="server" ></asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" CssClass="CommandButton"  
                                            Text="Search" onclick="btnSearch_Click" OnClientClick ="return CheckText(this.id)"  />
                                        <asp:Button ID="btnClearSearch" runat="server" CssClass="CommandButton" Text="Clear Search" OnClientClick ="return ClearSearch()" onclick="btnClearSearch_Click"/>
                                        &nbsp;&nbsp;<asp:Label ID="lblSearch" runat ="server" Text ="Ignore above filter when you search Claim No." Font-Bold="true"  ></asp:Label> 
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
                                            <asp:GridView ID="WarrantyClaimGrid" GridLines="Horizontal" Width="100%" 
                                                     AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                                    AutoGenerateColumns="False" AllowPaging="false" 
                                                onpageindexchanging="WarrantyClaimGrid_PageIndexChanging" runat="server" 
                                                onrowdatabound="WarrantyClaimGrid_RowDataBound" >
                                                <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle"  />                                                    
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png"
                                                                 OnClick="ImgSelect_Click" Style="height: 16px" /> 
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNo" runat="server"  />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                        HeaderStyle-CssClass="HideControl">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                                            <asp:Label ID="lblStateOrCountryID" runat="server" Text='<%# Eval("StateOrCountryID") %>' />                                                                                                                    
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer_Spares_Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="18%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="FPDA No" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimNo" runat="server" Text='<%# Eval("Advice_no") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="FPDA Date" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimDate" runat="server" Text='<%# Eval("Advice_Date") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="8%" />
                                                    </asp:TemplateField>                                                     
                                                    <asp:TemplateField HeaderText="LR No" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblJobcardNo" runat="server" Text='<%# Eval("LR") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Receipt No" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReceiptNo" runat="server" Text='<%# Eval("ReceiptNo") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField HeaderText="Receipt Date" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReceiptDate" runat="server" Text='<%# Eval("ReceiptDate") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="8%" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="DealerID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                        HeaderStyle-CssClass="HideControl">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDealerID" runat="server" Text='<%# Eval("Dealer_ID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                    
                                                    <asp:TemplateField HeaderText="FPDA Status" ItemStyle-Width="19%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimStatus" runat="server" Text='<%# Eval("Status") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="19%" />
                                                    </asp:TemplateField>                                                                                                        
                                                </Columns>
                                            </asp:GridView>
                                           </asp:Panel>
                                    </td>
                                </tr>
                                <tr  >
                                <td align="center"  class="PagerTableTD" > <strong > <CustomPager:PagerV2_8 ID="objCustomPager" runat ="server"  NextClause="..." 
                                PreviousClause ="..." NormalModePageCount ="10" CompactModePageCount="10"   ShowResultClause="false" GeneratePagerInfoSection="false"  
                                MaxSmartShortCutCount="0"
                                oncommand="objCustomPager_Command" /></strong>
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
                <tr id="TmpControl">
                    <td style="width: 14%">
                        <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                            Text=""></asp:TextBox>
                        <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtRequestOrClaim" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:HiddenField ID="hidSourceID" runat="server" />
                        <asp:HiddenField ID="hdnDealers" runat="server" Value ="" />
                        <asp:HiddenField ID="hdnFirstSlabamt" runat="server" Value ="0" />
                        <asp:HiddenField ID="hdnPagecount" runat="server" Value ="0" />
                        <asp:TextBox ID="txtDealerCode" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox>

                        
                    </td>
                </tr>
            </table>
    <script type ="text/javascript" >
        function OnStateChange() {
            var trDate = document.getElementById("<%=trDate.ClientID%>");
            var drpClaimStatus = document.getElementById("<%=drpClaimStatus.ClientID %>");
            var ClaimStatusVal = drpClaimStatus.options[drpClaimStatus.selectedIndex].text
            if (ClaimStatusVal == "Pending")
                trDate.style.display = "none"
            else
                trDate.style.display = ""
        }
        function ClearSearch() {
            document.getElementById("<%=txtSearchText.ClientID %>").value = '';
        }
        function CheckText(SourceID) {
            var hidSourceID =
            document.getElementById("<%=hidSourceID.ClientID%>");
            hidSourceID.value = SourceID;

            var txtSearchText = document.getElementById("<%=txtSearchText.ClientID %>")
        if (txtSearchText.value == '')
            return false;
    }
    </script>
</asp:Content>