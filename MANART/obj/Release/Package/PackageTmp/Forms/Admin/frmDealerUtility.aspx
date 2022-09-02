<%@ Page Title="MTI-Dealer Utility" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableViewState="true" EnableEventValidation="false" CodeBehind="frmDealerUtility.aspx.cs" Inherits="MANART.Forms.Admin.frmDealerUtility" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/DateStyle.css" rel="stylesheet" />

    <script src="../../Scripts/JSFPDAWarrantyClaim.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtCrDate = document.getElementById("ContentPlaceHolder1_txtCrDate_txtDocDate");
            if (txtCrDate != null)
                $('#ContentPlaceHolder1_txtCrDate_txtDocDate').datepick({
                    dateFormat: 'dd/mm/yyyy'
                });
        });
    </script>

    <script type="text/javascript">
        function PatchEntryStatusAll(id) {
            var frm = document.forms[0];
            var bEntryStatusAll;
            bEntryStatusAll = false;
            if (document.getElementById(id).checked == false) {
                alert("Are you sure you want to deselect all the record? Already download status will get not deselect ")
                bEntryStatusAll = false;
            }
            else {
                alert("Are you sure you want to select all the record? Already download status will not get Updated")
                bEntryStatusAll = true;
            }
            var gvcheck = document.getElementById('ContentPlaceHolder1_DetailsGrid');
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes

            for (i = 1; i < gvcheck.rows.length; i++) {
                if (gvcheck.rows[i].cells[10].innerText == "No ") {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = bEntryStatusAll;
                }
            }


        }
        function PatchDownloadedStatusAll(id) {
            var frm = document.forms[0];
            var bEntryStatusAll;
            bEntryStatusAll = false;
            if (document.getElementById(id).checked == false) {
                alert("Are you sure you want to deselect all the record? Already download status will get not deselect and It will Update only to patch Added status is Yes ")
                bEntryStatusAll = false;
            }
            else {
                alert("Are you sure you want to select all the record? Already download status will not get Updated and It will Update only to patch Added status is Yes")
                bEntryStatusAll = true;
            }
            var gvcheck = document.getElementById('ContentPlaceHolder1_DetailsGrid');
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes

            for (i = 1; i < gvcheck.rows.length; i++) {
                if (gvcheck.rows[i].cells[10].innerText == "No ") {
                    if (gvcheck.rows[i].cells[9].innerText == "Yes ") {
                        var inputs = gvcheck.rows[i].getElementsByTagName('input');
                        inputs[1].checked = bEntryStatusAll;
                    }
                }
            }


        }
        function SetCurrentAndPastDate(obj, Msg) {

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
            if (dtCurDate < sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="menu">
        <asp:Menu ID="Menu1" CssClass="NavigationMenu" Orientation="Horizontal" runat="server"
            Height="36px" StaticEnableDefaultPopOutImage="False" StaticTopSeparatorImageUrl="~/Images/menu_sprtr.gif"
            OnMenuItemClick="Menu1_MenuItemClick1" BackColor="#919191">
            <StaticMenuItemStyle CssClass="staticMenuItemStyle" ForeColor="Blue" />
            <StaticHoverStyle CssClass="staticHoverStyle" ForeColor="White" />
            <StaticSelectedStyle CssClass="staticMenuItemSelectedStyle" ForeColor="White" />
            <DynamicMenuItemStyle CssClass="dynamicMenuItemStyle" ForeColor="White" />
            <DynamicHoverStyle CssClass="menuItemMouseOver" ForeColor="White" />
            <DynamicMenuStyle CssClass="menuItem" ForeColor="White" />
            <DynamicSelectedStyle CssClass="menuItemSelected" ForeColor="White" />
            <Items>
                <asp:MenuItem ImageUrl="" Text="Patch Management" Value="0" Selected="True"></asp:MenuItem>
                <asp:MenuItem ImageUrl="" Text="Dealer Live Entry" Value="1"></asp:MenuItem>
                <asp:MenuItem ImageUrl="" Text="XML Gen For SAP,DMS</br> SQL Jobs Status" Value="2">
                </asp:MenuItem>
                <asp:MenuItem></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="Tab1" runat="server">
                <table class="PageTable" border="1">
                    <tr valign="top">
                        <td class="TabArea" style="width: 1176px">
                            <table id="PageTbl" class="PageTable" border="1">
                                <tr>
                                    <td class="PageTitle">
                                        <asp:Label ID="lblTitle" runat="server" Text="Patch Management"> </asp:Label>
                                    </td>
                                </tr>
                                <tr id="TblControl">
                                    <td style="width: 14%">
                                        <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                            class="ContaintTableHeader">
                                            
                                            <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                                                ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                                                ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                                SuppressPostBack="true" CollapsedText="Add Patch Entry Details" ExpandedText="Hide Patch Entery Details"
                                                TextLabelID="lblTtlDocDetails">
                                            </cc1:CollapsiblePanelExtender>
                                            <asp:Panel ID="PSelectionGrid11" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                                <uc4:SearchGridView ID="SearchGridView1" runat="server" bIsCallForServer="false"
                                                    sGridName="SearchGridView1" />
                                            </asp:Panel>
                                            <asp:Panel ID="TtlDocDetails" runat="server">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center" class="ContaintTableHeader" width="96%">
                                                            <asp:Label ID="lblTtlDocDetails" runat="server" Text="Patche Details" Width="96%"
                                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                                Height="16px"></asp:Label>
                                                        </td>
                                                        <td width="1%">
                                                            <asp:Image ID="ImgTtlDocDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                                Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                                ScrollBars="None">
                                                <table id="tblRFPDetails" runat="server" class="ContainTable">
                                                    <tr>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblPatchName" runat="server" Text="Patch Name"></asp:Label>
                                                        </td>
                                                        <td style="width: 18%; height: 15px;">
                                                            <asp:DropDownList ID="drpPatchName" runat="server" Width="283px" CssClass="ComboBoxFixedSize"
                                                                EnableViewState="true" AutoPostBack="True" OnSelectedIndexChanged="drpPatchName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 50%; height: 15px;">
                                                            <asp:TextBox ID="txtPatchName" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="50" Width="416px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" style="width: 15%; height: 18px;">
                                                            <asp:Label ID="lblNofiles" runat="server" Text="No OF Files"></asp:Label>
                                                        </td>
                                                        <td style="width: 10%; height: 18px;">
                                                            <asp:TextBox ID="txtNoSplitFiles" Text="" runat="server" CssClass="TextForAmount"
                                                                ReadOnly="false" MaxLength="2" onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);"
                                                                Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%; height: 18px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" style="width: 10%; height: 15px;">
                                                            <asp:Label ID="lblPatchremarks" runat="server" Text="Patch Details"></asp:Label>
                                                        </td>
                                                        <td colspan="2" style="width: 75%; height: 40px;">
                                                            <asp:TextBox ID="txtPatchRemarks" runat="server" CssClass="TextBoxForString" TextMode="MultiLine"
                                                                Height="78%" Text="" Width="72%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" style="width: 10%; height: 15px;">
                                                        </td>
                                                        <td style="width: 18%; height: 15px;">
                                                            <asp:Button ID="btnSelectDealer" runat="server" CssClass="CommandButton" Text="Select Live Dealer"
                                                                OnClick="btnSelectDealer_Click" />
                                                        </td>
                                                        <td style="height: 15px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 15px;" colspan="3">
                                                            <asp:Label ID="lblNoDlLiveLocation" runat="server" Text="Live Dealer Location Count :-"></asp:Label>&nbsp;
                                                            <asp:Label ID="lblNoDlLiveLocationCount" runat="server"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblNoDlLivePatches" runat="server" Text="Patch Added Dealer Location Count :-"></asp:Label>&nbsp;
                                                            <asp:Label ID="lblNoDlLivePatchesCount" runat="server"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblNoDlLivePatchesDownload" runat="server" Text="Patch Download Dealer Location Count :-"></asp:Label>&nbsp;
                                                            <asp:Label ID="lblNoDlLivePatchesDownloadCount" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnUpdate" runat="server" CssClass="CommandButton" Text="Update Patch Entry for Live Dealer"
                                                                OnClick="btnUpdate_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="PatcGrid"
                                                                Width="100%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                                                 AutoGenerateColumns="False" AllowSorting="True" Height="16px" OnRowDataBound="DetailsGrid_RowDataBound"
                                                                OnRowCommand="DetailsGrid_RowCommand1" onsorting="DetailsGrid_Sorting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Patch No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="PatchNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PatchNo")  %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="30%" 
                                                                        SortExpression="Dealer_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDealerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Dealer_Name") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Dealer ID" ItemStyle-Width="10%" SortExpression="DealerID">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDealerID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DealerID") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Region" ItemStyle-Width="10%" SortExpression="Region_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRegion" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Region_Name") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                    </asp:TemplateField> 
                                                                    <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="10%" SortExpression="Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Code") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="15%" />
                                                                    </asp:TemplateField>                                                                                                                                        
                                                                    <asp:TemplateField HeaderText="Dealer Type" ItemStyle-Width="20%" SortExpression="DealerTypeDesc">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDealerType" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DealerTypeDesc") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Selected Patch Name" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpatchname" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("patchname") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rollut Patch" ItemStyle-Width="10%"  SortExpression="RolloutPatchName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRolloutpatchName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("RolloutPatchName") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="7%" />
                                                                    </asp:TemplateField>                                                                    
                                                                    <asp:TemplateField HeaderText="Last Patch(Executed Status)" ItemStyle-Width="10%"  SortExpression="LastExcoutPatchName" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLastExcoutPatchName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LastExcoutPatchName") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>                                                                    
                                                                    <asp:TemplateField HeaderText="Patch Added Status" ItemStyle-Width="10%" SortExpression="patchstatus">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpatchstatus" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("patchstatus") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Patch Download Status" ItemStyle-Width="10%" SortExpression="patchDownstatus">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpatchDownloadstatus" runat="server" CssClass="LabelCenterAlign"
                                                                                Text='<%# Eval("patchDownstatus") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Update Patch Added" ItemStyle-Width="10%"  >
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkPatchEntry" runat="server" Checked='<%# Eval("EntryAdded") %>'
                                                                                OnCheckedChanged="PatchEntryStatusSingleOne" AutoPostBack="True" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="ChkPatchEntryStatusAll" runat="server" Text="All Update Patch Added"
                                                                                ToolTip="it will get update only if patch Download status is false" />                                                                            
                                                                        </HeaderTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Update Download Status" ItemStyle-Width="10%"  >
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkDownloadStatus" runat="server" Checked='<%# Eval("DownloadedStatus") %>'
                                                                                OnCheckedChanged="PatchDownloadStatusSingleOne" AutoPostBack="True" />
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="ChkDownloadedStatusAll" runat="server" Text="All Update Download Status"
                                                                                ToolTip="it will get update only if patch Download status is false" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#74868B" BorderColor="Black" />
                                                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                                                <AlternatingRowStyle Wrap="True" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14%; height: 14px;">
                                    </td>
                                </tr>
                                <tr id="TmpControl1">
                                    <td style="width: 14%">
                                        <%--<asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                                    Text=""></asp:TextBox>
                                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="Tab2" runat="server">
                <table class="PageTable" border="1">
                    <tr valign="top">
                        <td class="TabArea" style="width: 1176px">
                            <table id="Table2" class="PageTable" border="1">
                                <tr>
                                    <td class="PageTitle">
                                        <asp:Label ID="Label1" runat="server" Text="Dealer Live Deatils"> </asp:Label>
                                    </td>
                                </tr>
                                <tr id="Tr1">
                                    <td style="width: 14%">
                                        <asp:Panel ID="PDocDLDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                            class="ContaintTableHeader">
                                            <%--               <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntDealerLiveDetails"
                                        ExpandControlID="TtlDealerLiveDetails" CollapseControlID="TtlDealerLiveDetails" Collapsed="false"
                                        ImageControlID="ImgDealerLiveDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                        SuppressPostBack="true" CollapsedText="Show Dealer Live Details" ExpandedText="Hide Dealer Live Details"
                                        TextLabelID="lblTtlDocDetails1">--%>
                                            </cc1:CollapsiblePanelExtender>
                                            <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                                <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                                                    bIsCallForServer="true" />
                                            </asp:Panel>
                                            <asp:Panel ID="CntDealerLiveDetails" runat="server">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center" class="ContaintTableHeader" width="96%">
                                                            <asp:Label ID="lblTtlDocDetails1" runat="server" Text="Dealer Live(Partialy) Details" Width="96%"
                                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                                Height="16px"></asp:Label>
                                                        </td>
                                                        <td width="1%">
                                                            <asp:Image ID="ImgDealerLiveDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                                                Height="15px" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="TtlDealerLiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                                ScrollBars="None">
                                                <table id="Table1" runat="server" class="ContainTable">
                                                    <tr>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLDealerID" runat="server" Text="Dealer ID"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLDealerID" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="10" Width="100px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDL" runat="server" Text="Dealer Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLDealerName" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="100" Width="416px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLDealercity" runat="server" Text="Dealer City"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLDealercity" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="30" Width="150px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLDEalerCOde" runat="server" Text="Dealer Codes"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLDEalerCOde" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="100" Width="416px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLLiveStatus" runat="server" Text="Dealer Live Status"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLStatus" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="10" Width="100px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLType" runat="server" Text="Exists Dealer Location Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLDealerType" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="50" Width="200px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLHirerachyCode" runat="server" Text="Dealer Hierarchy Code"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDLHierarchyCode" Text="" runat="server" CssClass="TextBoxForString"
                                                           MaxLength="15" Width="100px" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%; height: 15px;" class="tdLabel">
                                                    <asp:Label ID="lblDLBranchCode" runat="server" Text="Dealer Branch Code"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDLBranchCode" Text="" runat="server" CssClass="TextForAmount"
                                                           MaxLength="2" Width="100px" ></asp:TextBox> 
                                                           <asp:Label ID="lblRedDLBranchCode" runat="server" Text="*" ForeColor="Red"></asp:Label>                                                   
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLPartilayLiveStatus" runat="server" Text="Dealer Partilay Live Status"></asp:Label>                                                        
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLPartilayStatus" Text="" runat="server" CssClass="TextBoxForString"
                                                                MaxLength="10" Width="100px" ReadOnly="true"></asp:TextBox>                                                        
                                                        </td>                                                    
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLDealerType" runat="server" Text="Dealer Location Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpDLDealerType" runat="server" Width="283px" CssClass="ComboBoxFixedSize"
                                                                EnableViewState="true" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtDealerLOcatioID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td style="width: 10%; height: 15px;" class="tdLabel">
                                                    <asp:Label ID="lbllblDLRolloutPatchName" runat="server" Text="Rollout Patch Name"></asp:Label>                                                        
                                                    </td>
                                                    <td colspan=2>
                                                    <asp:DropDownList ID="drpDLRolloutPatchName" runat="server" Width="290px" CssClass="ComboBoxFixedSize"
                                                                EnableViewState="true" AutoPostBack="True" >
                                                            </asp:DropDownList>
                                                    <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>                                                            
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td  style="width: 15%; height: 15px;" class="tdLabel">                                                    
                                                    <asp:Button ID="btnDLDealerPartialyLive" runat="server" CssClass="CommandButton" 
                                                            Text="11" onclick="btnDLDealerPartialyLive_Click" 
                                                            Visible="False"/>                                                                                                                                                                                                    
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td colspan=3>
                                                          <asp:Button ID="btnDLDealerLiveTypeUpdate" runat="server" CssClass="CommandButton"
                                                           Text=" Make Dealer Partialy Live and Add Location Type " OnClick="btnDLDealerLiveTypeUpdate_Click" />                
                                                    &nbsp;&nbsp;                                                                                                  
                                                    <asp:Button ID="btnDLDealerLiveUpdate" runat="server" CssClass="CommandButton" Text=" Make Dealer Live "
                                                             OnClick="btnDLDealerLiveUpdate_Click1" />    
                                                    &nbsp;&nbsp;                                                                                                  
                                                    <asp:Button ID="btnDLDealerRolloutUpdate" runat="server" CssClass="CommandButton" 
                                                              Text=" Rollout Data Uploaded On FTP " onclick="btnDLDealerRolloutUpdate_Click" />
                                                    &nbsp;&nbsp;                                                                                                  
                                                    <asp:Button ID="btnDLDealerBranchCodeUpdate" runat="server" CssClass="CommandButton" 
                                                              Text=" Dealer Branch Code update " 
                                                              onclick="btnDLDealerBranchCodeUpdate_Click"  />                                                                                                                                                                                              
                                                    </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr id="Tr3">
                                    <td style="width: 14%">
                                        <asp:Panel ID="PLiveHOBRANCHDeatils" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                            class="ContaintTableHeader">
                                            <%--<cc1:CollapsiblePanelExtender ID="CPELiveHOBRANCHDeatils" runat="server" TargetControlID="CntLiveHOBRANCHDeatils"
                                        ExpandControlID="TtlLiveHOBRANCHDeatils" CollapseControlID="TtlLiveHOBRANCHDeatils" Collapsed="false"
                                        ImageControlID="imgTtlLiveHOBRANCHDeatils" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                        SuppressPostBack="true" CollapsedText="Live Dealer HO Branch Details" ExpandedText="Hide Live Dealer HO Branch Details"
                                        TextLabelID="lblTtlLiveHOBRANCHDeatils">
                                    </cc1:CollapsiblePanelExtender>--%>
                                            <asp:Panel ID="Panel2" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                                <uc4:SearchGridView ID="SearchGridView2" runat="server" bIsCallForServer="false"
                                                    sGridName="SearchGridView2" />
                                            </asp:Panel>
                                            <asp:Panel ID="CntLiveHOBRANCHDeatils" runat="server">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center" class="ContaintTableHeader" width="96%">
                                                            <asp:Label ID="lblTtlLiveHOBRANCHDeatils" runat="server" Text="Live Dealer HO Branch Details"
                                                                Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                                Height="16px"></asp:Label>
                                                        </td>
                                                        <td width="1%">
                                                            <asp:Image ID="imgTtlLiveHOBRANCHDeatils" runat="server" ImageUrl="~/Images/Plus.png"
                                                                Height="15px" Width="100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="TtlLiveHOBRANCHDeatils" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                                ScrollBars="None">
                                                <table id="Table3" runat="server" class="ContainTable">
                                                    <tr>
                                                        <td style="width: 10%; height: 15px;" class="tdLabel">
                                                            <asp:Label ID="lblDLHODealerID" runat="server" Text="HO Dealer ID"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLHODealerID" Text="" runat="server" CssClass="TextForAmount"
                                                                ReadOnly="false" MaxLength="3" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDLBRDealerID" runat="server" Text="Dealer ID"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDLBRDealerID" Text="" runat="server" CssClass="TextForAmount"
                                                                ReadOnly="false" MaxLength="3" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDLHOBranchCode" runat="server" Text="HO/Branch Code"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHOBranchCode" Text="" runat="server" CssClass="TextForAmount"
                                                                ReadOnly="false" MaxLength="2" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                            <asp:Button ID="btnHOBranchUpdate" runat="server" CssClass="CommandButton" Text="Add HO/Branch Entry"
                                                                OnClick="btnHOBranchUpdate_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr id="Tr2">
                                    <td style="width: 14%">
                                        <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                                            Text=""></asp:TextBox>
                                        <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                                        <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="Tab3" runat="server">
                <table class="PageTable" border="1" runat="server" >
                    <tr valign="top">
                        <td style="width: 1176px">
                            <table class="PageTable" border="1">
                                <tr>
                                    <td class="PageTitle">
                                        <asp:Label ID="Label2" runat="server" Text="XML Generate For SAP,DMS And SQL Jobs Enable-Disable"> </asp:Label>
                                    </td>
                                </tr>
                                </table>
                                <table class="PageTable" border="1" >
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td align="center" class="ContaintTableHeader" width="96%">
                                                        <asp:Label ID="Label5" runat="server" Text="Live Dealer Specific Master XML Generate"
                                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                            Height="16px"></asp:Label>
                                                    </td>
                                                    <td width="1%">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                            Width="100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="Table5" runat="server" class="ContainTable" width="100%">
                                            <tr>
                                                <td class="tdLabel" style="width: 30%; height: 15px;">
                                                    <asp:Label ID="Label4" runat="server" Text="Part Prizes,Model Prizes,Labour Rate(WAR/AMC),Part Tax "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDealerXMLGen" runat="server" CssClass="CommandButton" Text="Dealer Specific XML Generate"
                                                        OnClick="btnDealerXMLGen_Click" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDealerliveXMLProcessstete" runat="server" Text="After Button click it take few mints to generate XML"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="width: 30%; height: 15px;">
                                                    <asp:Label ID="lblPingSingleSTN" runat="server" Text="STN Chassis No"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtChassisNo" Text="" runat="server" MaxLength="30" Width="35%" CssClass="TextBoxForString"></asp:TextBox>
                                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="LblSTNData" runat="server" Text="STN Chassis Date"></asp:Label>
                                                </td>
                                                <td style="width: 35%; height: 15px;">                                                                                                                                           
                                                    <uc3:CurrentDate ID="txtCrDate" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPingSingleSTN" runat="server" CssClass="CommandButton" 
                                                        Text="Ping Single STN" onclick="btnPingSingleSTN_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                </table>
                                <table class="PageTable" border="1" id="tblWinService" runat="server"  >
                                 <tr>
                                <td>
                                        <asp:Panel ID="Panel4" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td align="center" class="ContaintTableHeader" width="96%">
                                                        <asp:Label ID="lblWindiwsservice" runat="server" Text="For Date :-  Windows Service Status-No Of File In Process From SAP,DMS "
                                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                            Height="16px"></asp:Label>
                                                    </td>
                                                    <td width="1%">
                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                            Width="100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>                                
                                </td>                                
                                </tr>
                                <tr>
                                <td>
                                <%--<asp:ScriptManager EnablePartialRendering="true"  ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate> 
                                    <table id="Table4" runat="server" class="ContainTable" width="100%">
                                        <tr>
                                        <td class="tdLabel" style="width: 30%; height: 15px;" colspan=2>
                                            <asp:Label ID="lblNoOFSAPFileInProcessing" runat="server" Text="No SAP Files in Pipeline for processes  :- "></asp:Label>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblNoOFDMSFileInProcessing" runat="server" Text="No DMS Files in Pipeline for processes  :- "></asp:Label>
                                        </td>                      
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="width: 30%; height: 15px;">
                                            <asp:Label ID="lblNoOfSAPFileInProcessed" runat="server" Text="No OF SAP Files Processed :- "></asp:Label>
                                             &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblNoOfSAPFileInException" runat="server" Text="No OF SAP Files Exception :- "></asp:Label>
                                            </td>
                                            <td  class="tdLabel" style="width: 30%; height: 15px;">
                                            <asp:Label ID="lblNoOfDMSFileInProcessed" runat="server" Text="No OF DMS Files Processed :- "></asp:Label>
                                             &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblNoOfDMSFileInException" runat="server" Text="No OF DMS Files Exception :- "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnWindowsServiceStatus" runat="server" CssClass="CommandButton" 
                                                   Text="Refresh-Windows Service,No OF File in Porcess and Space Status" 
                                                    onclick="btnWindowsServiceStatus_Click"  />                                            
                                            </td>
                                        </tr>        
                                        <tr>
                                        <td class="tdLabel" style="width: 70%; height: 15px;" colspan=2>
                                            <asp:Label ID="lblSpacesOn114" runat="server" Text="Available drive spaces on 114 Server(In gb) : "></asp:Label>                                            
                                        </td>                      
                                        </tr>                                                                         
                                     </table>
                                     </ContentTemplate>
                                     </asp:UpdatePanel>                                   
                                </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <tr>
                    <td>
                     <table class="PageTable" border="1" id="tblJobstatus" >
                         <tr>
                             <td>
                                 <asp:Panel ID="Panel3" runat="server">
                                     <table width="100%">
                                         <tr>
                                             <td align="center" class="ContaintTableHeader" width="96%">
                                                 <asp:Label ID="Label3" runat="server" Height="16px" 
                                                     onmouseout="SetCancelStyleOnMouseOut(this);" 
                                                     onmouseover="SetCancelStyleonMouseOver(this);" 
                                                     Text="SQL Job Status-Set Enable/Disable" Width="96%"></asp:Label>
                                             </td>
                                             <td width="1%">
                                                 <asp:Image ID="Image2" runat="server" Height="15px" 
                                                     ImageUrl="~/Images/Plus.png" Width="100%" />
                                             </td>
                                         </tr>
                                     </table>
                                 </asp:Panel>
                             </td>
                         </tr>
                         <tr>
                        <asp:GridView ID="GridView2" runat="server" AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            Height="16px" SkinID="NormalGrid" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Job Name" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblname" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("job_name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Enabled Status" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEnabled" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("job_status") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Run Status" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastrunstatus" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("last_run_status") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Run Date(MM/dd/YYYY:hh:mm:ss)" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastrundate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("last_run_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Sch Run Date(MM/dd/YYYY:hh:mm:ss)" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnextscheduledrundate" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("next_scheduled_run_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Step Description" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstep_description" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("step_description") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Enable" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSQLJobenable" runat="server" CssClass="CommandButton" Text="Enable"
                                            OnClick="btnbtnSQLJOBEnable" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Disable" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSQLJobdisable" runat="server" CssClass="CommandButton" Text="Disable"
                                            OnClick="btnbtnSQLJOBDisable" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </tr>
                     </table>&nbsp;
                     </td>
                    </tr>
                    
                </table>               
            </asp:View>
        </asp:MultiView>
    </div>
    <%--    <input id="Hidden1" type="hidden" runat="server" />    
    <table id="PageTbl" class="PageTable" border="1">
        <tr>
            <td class="PageTitle">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">                           
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Document Details" ExpandedText="Hide Document Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />
                    </asp:Panel>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Document Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" 
                                        onmouseout="SetCancelStyleOnMouseOut(this);" Height="16px"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDocDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable">
                            <tr>
                                <td style="width: 15%; height: 15px;" class="tdLabel">
                                    <asp:Label ID="lblDocNo" runat="server" Text="Doc No"></asp:Label>
                                </td>
                                <td style="width: 18%; height: 15px;">
                                    <asp:TextBox ID="txtDocNo" Text="" runat="server" CssClass="TextBoxForString" 
                                        ReadOnly="true" MaxLength="12"></asp:TextBox>
                                </td>
                                <td style="width: 15%; height: 15px;" class="tdLabel">
                                    <asp:Label ID="lblDocDate" runat="server" Text="Doc Date"></asp:Label>
                                </td>
                                <td style="width: 18%; height: 15px;">
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" />
                                </td>
                                <td class="tdLabel" style="width: 15%; height: 15px;">
                                </td>
                                <td style="width: 18%; height: 15px;">
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%;">
                                    Transporter:
                                </td>
                                <td style="width: 25%;">
                                    <asp:TextBox ID="txtTransporter" runat="server" CssClass="TextBoxForString" 
                                        MaxLength="50" ReadOnly="false" Text="" Width="193px"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    No Of Cases</td>
                                <td style="width: 18%">                                    
                                    <asp:TextBox ID="txtNoOfCases" runat="server" CssClass="TextForAmount" 
                                        ReadOnly="false" Text="" MaxLength="15"  onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);" ></asp:TextBox>                                    
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                   
                                </td>
                                <td style="width: 18%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%; height: 15px;">
                                    LR No:
                                </td>
                                <td style="width: 18%; height: 15px;">
                                     <asp:TextBox ID="txtLRNo" runat="server" CssClass="TextBoxForString" 
                                        ReadOnly="false" Text="" MaxLength="15"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%; height: 15px;">
                                    LR Date</td>
                                <td style="width: 18%; height: 15px;">                                    
                                    <uc3:CurrentDate ID="txtLRDate" runat="server" bCheckforCurrentDate="false"  />
                                </td>
                                <td class="tdLabel" style="width: 15%; height: 15px;">
                                    
                                </td>
                                <td style="width: 18%; height: 15px;">
                                    </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%;">
                                    
                                </td>
                                <td style="width: 18%;">                                    
                                    <asp:LinkButton ID="lblSelectWarrantyClaim" runat="server" ForeColor="#49A3D3" 
                                        Height="25px"  
                                       Text="Select Warranty Claims" 
                                        ToolTip="Select Warranty Claims" Width="77%"                                         
                                        onclick="lblSelectWarrantyClaim_Click"></asp:LinkButton>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    Remarks:
                                </td>
                                <td style="width: 18%" colspan="3">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxForString" TextMode="MultiLine"
                                        Height="80%" Text="" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
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
            </td>
        </tr>
    </table>    --%>
</asp:Content>
