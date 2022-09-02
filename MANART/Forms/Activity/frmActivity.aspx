<%@ Page Title="Activity" Language="C#" MasterPageFile="~/Header.Master"
    EnableViewState="true" AutoEventWireup="true" CodeBehind="frmActivity.aspx.cs" Inherits="MANART.Forms.Activity.frmActivity" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="../../WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="../../CSS/Style.css" rel="Stylesheet" type="text/css" />
    <link href="../../CSS/GridStyle.css" rel="Stylesheet" type="text/css" />
    
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    

    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsGCRfunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsWarrantyFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsActivityRequestClaimFunction.js" type="text/javascript"></script>
    <script src="../../JavaScripts/jsIndentConsolidation.js" type="text/javascript"></script>
    
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
<script type ="text/javascript" >

    function pageLoad() {
        $(document).ready(function () {
            var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
            var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
            $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value, minDate: (txtFromDate.value == '') ? '0d' : txtFromDate.value
            });

            $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtFromDate.value == '') ? '0d' : txtFromDate.value
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

    <script type="text/javascript">
        window.onload
        {
            AtPageLoad();

        }
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            HideControl();
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }
        function refresh() {
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }
        function SetCurrAndFutureDate(obj, Msg) {

            var objDateValue = "";
            var ObjDate = obj;
            var x = new Date();
            var y = x.getYear();
            var m = x.getMonth() + 1; // added +1 because javascript counts month from 0
            var d = x.getDate();
            var dtCur = d + '/' + m + '/' + y;
            var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)

            objDateValue = ObjDate.value;
            var sTmpValue = objDateValue;
            var day = dGetValue(sTmpValue.split("/")[0]);
            var month = dGetValue(sTmpValue.split("/")[1]) - 1;
            var year = dGetValue(sTmpValue.split("/")[2]);
            var sTmpDate = new Date(year, month, day);
            var TmpDay = 0;

            if (objDateValue == '') {
                return false;
            }
            if (dtCurDate > sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }
        function CheckAcDateGreter(obj1, obj2, obj3) {

            var splDate = obj1.value.split("/")
            var splDate1 = obj2.value.split("/")
            var dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
            var dt1 = new Date(splDate1[2], splDate1[1] - 1, splDate1[0]);

            if (dt < dt1) {
                alert(obj3)
                d = dt1.getDate() + 1
                dt1.setDate(d);
                obj1.value = dt1.format("dd/MM/yyyy");
                obj1.focus();
                return false
            }
        }

    </script>

    <script language="JavaScript">
        var message = "Right-mouse click is not allowed";
        function click(e) {
            if (document.all) {
                if (event.button == 2 || event.button == 3) {
                    alert(message);
                    return false;
                }
            }
            else {
                if (e.button == 2 || e.button == 3) {
                    e.preventDefault();
                    e.stopPropagation();
                    alert(message);
                    return false;
                }
            }
        }
        if (document.all) // for IE
        {
            document.onmousedown = click;
        }
        else // for other browsers
        {
            document.onclick = click;
        }
    </script>

    <script type="text/javascript">

        if (typeof window.event != 'undefined')
            document.onkeydown = function () {
                if (event.srcElement.tagName.toUpperCase() != 'INPUT')
                    return (event.keyCode != 8);
            }
        else
            document.onkeypress = function (e) {
                if (e.target.nodeName.toUpperCase() != 'INPUT')
                    return (e.keyCode != 8);
            }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Activity"></asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
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
                <asp:UpdatePanel ID ="UpdatePanel1" runat ="server" >
<ContentTemplate>          
                <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="txtDocNoDetails" runat="server" class="ContainTable">
                        <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="8">
                                Activity Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <asp:Label ID="Label2" runat="server" Text="Departmental Activity:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDepartmentalActivity" runat="server" AutoPostBack="True"
                                    CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpDepartmentalActivity_SelectedIndexChanged" TabIndex ="1">
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel">
                           <asp:Label ID="lblCategory" runat="server" Text="Category:" ></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ComboBoxFixedSize" 
                                    onselectedindexchanged="ddlCategory_SelectedIndexChanged" AutoPostBack ="true"   >
                                </asp:DropDownList>
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="lblRefClaimNo" runat="server" Text="Type Of Activity:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpTypeOfActivity" runat="server" 
                                    CssClass="ComboBoxFixedSize" 
                                    onselectedindexchanged="drpTypeOfActivity_SelectedIndexChanged" AutoPostBack ="true" TabIndex ="2">
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="lblClaimDate" runat="server" Text="Dealer Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDealerType" runat="server" CssClass="ComboBoxFixedSize"
                                    Style="display: none">
                                    <asp:ListItem Selected="True" Value="0">-----Select-----</asp:ListItem>
                                    <asp:ListItem Value="E">Export</asp:ListItem>
                                    <asp:ListItem Value="D">Domestic</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtDealerType" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                            </td>
                            
                             
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <asp:Label ID="Label5" runat="server" Text="Creditor Posting Key:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpCreditorPositionKey" runat="server" CssClass="ComboBoxFixedSize"
                                    AutoPostBack="True" OnSelectedIndexChanged="drpCreditorPositionKey_SelectedIndexChanged" TabIndex ="4">
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="Label7" runat="server" Text="MTI Posting Key:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpEtbPostingKey" runat="server" 
                                    CssClass="ComboBoxFixedSize" TabIndex="5">
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="Label9" runat="server" Text="GL Account:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGLAccount" runat ="server" Enabled ="false" CssClass="TextBoxForString"></asp:TextBox>
                               <asp:DropDownList ID="drpAccount" runat="server" CssClass="ComboBoxFixedSize" style="display :none">
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>                               
                            </td>
                            <%--<td class="tdLabel">
                                <asp:Label ID="Label8" runat="server" Text="Cost Center" ></asp:Label>
                            </td>
                            <td>
                               <asp:TextBox ID="txtCostCenter" runat ="server" Enabled ="false" CssClass="TextBoxForString" ></asp:TextBox>
                               <asp:DropDownList ID="drpCosrCenter" runat="server" CssClass="ComboBoxFixedSize" style="display :none">
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>                               
                            </td>--%>
                            <td class="tdLabel" style ="display :none ">
                                <asp:Label ID="Label4" runat="server" Text="Direct Claim"></asp:Label>
                            </td>
                            <td style ="display :none ">
                                <asp:DropDownList ID="drpDirectClaim" runat="server" CssClass="ComboBoxFixedSize" TabIndex ="3">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                </asp:DropDownList>
                                <b class="Mandatory">*</b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <asp:Label ID="lblRequestDate" runat="server" Text="Name of Activity:"></asp:Label>
                            </td>
                            <td>
                              <asp:TextBox ID="txtNameOfActivity" runat="server" CssClass="TextBoxForString" 
                                    Font-Bold="true" TabIndex="6"></asp:TextBox>
                               <%-- <asp:DropDownList ID="drpActivityName" runat="server" CssClass="ComboBoxFixedSize"
                                    onblur="return newActivity()">
                                </asp:DropDownList>--%>
                                
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="Label1" runat="server" Text="Start Date:"></asp:Label>
                            </td>
                            <td>
                                <uc3:CurrentDate ID="txtFromDate" runat="server" bCheckforCurrentDate="false" Mandatory="true" TabIndex ="7" />
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="Label14" runat="server" Text="End Date:"></asp:Label>
                            </td>
                            <td>
                                <uc3:CurrentDate ID="txtToDate" runat="server" bCheckforCurrentDate="false" Mandatory="true" TabIndex ="8" />
                            </td>
                            <td class="tdLabel">
                                <asp:Label ID="Label3" runat="server" Text="Remark:"></asp:Label>
                            </td>
                            <td rowspan ="2">
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="MultilineTextbox" 
                                    Font-Bold="true" Width ="80%" TabIndex="9" ></asp:TextBox>
                            </td>
                            <%--    <td class="tdLabel">
                                <asp:Label ID="Label5" CssClass="DispalyNon" runat="server" Text="Objective:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObjective" CssClass="DispalyNon" runat="server"  Font-Bold="true"></asp:TextBox>
                           
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <asp:Label ID="lblSpGL" runat="server" Text="Special GL:" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpSpecialGL" runat="server" CssClass="ComboBoxFixedSize" Visible="False" Enabled="false" >
                                </asp:DropDownList>
                            </td>                           
                            <td class="tdLabel">
                            </td>
                            <td>
                            </td>
                            <td class="tdLabel">
                            </td>
                            <td>
                            </td>
                            <td class="tdLabel">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="tblLocationDetails" runat="server" class="ContainTable" border="1">
                        <%--<tr>
                            <td>
                                <asp:Panel ID="PanelLocation" runat="server">
                                    <uc6:Location ID="Location" runat="server" />
                                </asp:Panel>
                            </td>
                        </tr>--%>
                        <tr align="left">
                                    <td class="tdLabel" style="width: 8%">
                                        Region:
                                    </td>
                                    <td class="tdLabel" style="width: 25%" >
                                        <div id="dvTopReg" class="dt" style="width: 80%;">
                                            <asp:CheckBox ID="ChkAllReg" runat="server"  
                                                OnCheckedChanged="ChkAllReg_CheckedChanged"  AutoPostBack ="true" /> <%--onClick="return SelectAllReg();"--%>
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
                                            <asp:CheckBox ID="ChkAllDpo" runat="server"  AutoPostBack ="true" OnCheckedChanged="ChkAllDpo_CheckedChanged" /> <%--onClick="return SelectAllDpo();"--%>
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
                                        <asp:Button ID="btnShow" runat="server" CssClass="CommandButton" 
                                            Text="Show" Visible="false"/>
                                    </td>
                                   <td style="width: 25%">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                    </td>
                                   
                                    <%--<td style="width: 18%">
                                    </td>--%>
                                </tr>
                    </table>
                </asp:Panel>
                </ContentTemplate>    
    </asp:UpdatePanel>
    
    
                <asp:Panel ID="PnlSearchGrid" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table1" runat="server" class="ContainTable">
                        <tr>
                            <td align="center">
                                <asp:Panel ID="PSelectionGrid" runat="server">
                                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                                        bIsCallForServer="true" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PActivityDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEActivityDetails" runat="server" TargetControlID="CntActivityDetails"
                        ExpandControlID="TtlActivityDetails" CollapseControlID="TtlActivityDetails" Collapsed="true"
                        ImageControlID="ImgTtlActivityDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Activity Dealer Details" ExpandedText="Hide Activity Dealer Details"
                        TextLabelID="lblTtlActivityDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlActivityDetails" runat="server">
                        <table width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlActivityDetails" runat="server" Text="Activity Dealer Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlActivityDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>                                    
                        <asp:Panel ID="CntActivityDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <asp:GridView ID="GridSelectionDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                    AllowPaging="True" AlternatingRowStyle-Wrap="true" 
                                EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black" 
                                onpageindexchanging="GridSelectionDetails_PageIndexChanging">
                                    <FooterStyle CssClass="GridViewFooterStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>                                    
                                     <asp:TemplateField HeaderText="Serial No" HeaderStyle-Width="10%" ItemStyle-Width ="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Name" HeaderStyle-Width="70%" ItemStyle-Width ="70%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Dealer Code" HeaderStyle-Width="20%" ItemStyle-Width ="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer_Code") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                  
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
            </td>
        </tr>
        
        <%--<tr id="Location">
            <td style="width: 14%">
                
            </td>
        </tr>--%>
        <tr id="TmpControl" style="display :none">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtDealerCode" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtActivityExists" runat="server" Width="1px" Text=""></asp:TextBox>
                
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
</asp:Content>
