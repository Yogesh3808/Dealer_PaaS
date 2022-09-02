<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"  ValidateRequest="false" EnableEventValidation="false" 
    CodeBehind="frmActivityRequestSelection.aspx.cs" Inherits="MANART.Forms.Activity.frmActivityRequestSelection" %>
   
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%--<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%@ Register Src="~/WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc2" %>

<%@ Register Src="../../WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../JavaScripts/jsProformaFunction.js" type="text/javascript"></script>

    <link href="../../CSS/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/cssDatePicker.css" rel="stylesheet" type="text/css/">
    <link href="../../CSS/GridStyle.css" rel="stylesheet" type="text/css" />
    
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script type="text/javascript" src="../../JavaScripts/jsWarrantyProcessing.js"></script>
    
    
    <script type="text/javascript">

        function Show(para) {
            //  debugger;
            var DeptId = 0;
            var Flage = 0;
          //  var DeptId = document.getElementById('ContentPlaceHolder1_txtDeptId').value
            var DeptId = para.parentNode.parentNode.childNodes[7].children[1].innerText;
             var Flage = document.getElementById('ContentPlaceHolder1_txtFlage').value
             var UserDeptId = document.getElementById('ContentPlaceHolder1_txtUserDeptId').value
            var ActivityID = para.parentNode.parentNode.childNodes[5].children[0].innerText;
            var DealerID = para.parentNode.parentNode.childNodes[3].children[0].innerText;
            var iRegionID = para.parentNode.parentNode.childNodes[5].children[2].innerText;
            var status = para.parentNode.parentNode.childNodes[8].innerText;
             var MenuID = document.getElementById('ContentPlaceHolder1_txtID').value
            var UserRoleID = document.getElementById('ContentPlaceHolder1_txtUserRoleID').value
            var ApprProcc = document.getElementById('ContentPlaceHolder1_txtApprProcc').value


            if (UserRoleID == "1" && (status.search('Pending at HEAD') != -1)) {
                //&& status == "Pending at HEAD") {
                Flage = "P";
            }
            if (UserRoleID == "2" && (status.search('Pending at RSM') != -1)) {
                //&& status == "Pending at RSM") {
                Flage = "P";
            }
            if (UserRoleID == "3" && (status.search('Pending at ASM') != -1)) {
                //&& status == "Pending at ASM") {
                Flage = "P";
            }
            if (UserRoleID == "4" && (status.search('Pending at CSM') != -1)) {
                // && status == "Pending at CSM") {
                Flage = "P";
            }
            if (UserRoleID == "9" && (status.search('Pending at Finance') != -1)) {
                //status == "Pending at Finance") {
                Flage = "P";
            }
            if (UserRoleID == "9" && (status.search('GST invoice awaited') != -1)) {
                //status == "Pending at Finance") {
                Flage = "A";
            }
            if (UserRoleID == "9" && (status.search('Documents Awaited') != -1)) {
                //status == "Pending at Finance") {
                Flage = "P";
            }
            // debugger;
            window.showModalDialog("frmActivityRequestClaimProcessing.aspx?MenuID=" + MenuID + "&ActivityID=" + ActivityID + "&UserDeptId=" + UserDeptId + "&DealerID=" + DealerID + "&Flage=" + Flage + "&DeptId=" + DeptId + "&ApprProcc=" + ApprProcc + "&RegionID=" + iRegionID, "List", "dialogHeight: 700px; dialogWidth: 1200px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            //window.location.reload();
            return true;
        }
        function Validation() {
            //            var drpActivityCalimStatus = document.getElementById("<%=drpActivityCalimStatus.ClientID %>")
            //            var ActivityCalimStatusVal = drpActivityCalimStatus.options[drpActivityCalimStatus.selectedIndex].value
            //            if (ActivityCalimStatusVal == "0") {
            //                alert("Please Select Activity Status ")
            //                drpActivityCalimStatus.focus();
            //                return false;
            //            }
        }
        function ClearSearch() {
            document.getElementById("<%=txtSearchText.ClientID %>").value = '';
        }
        function CheckText() {
            var txtSearchText = document.getElementById("<%=txtSearchText.ClientID %>")
            if (txtSearchText.value == '')
                return false;
        }
        function OnStateChange() {
            var trDate = document.getElementById("<%=trDate.ClientID%>");
            var drpClaimStatus = document.getElementById("<%=drpActivityCalimStatus.ClientID %>");
            var ClaimStatusVal = drpClaimStatus.options[drpClaimStatus.selectedIndex].text
            if (ClaimStatusVal == "Pending")
                trDate.style.display = "none"
            else
                trDate.style.display = ""
        }
        function CheckDateLess(obj1, obj2, obj3) {
            var splDate = obj1.value.split("/")
            var splDate1 = obj2.value.split("/")
            var dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
            var dt1 = new Date(splDate1[2], splDate1[1] - 1, splDate1[0]);

            if (dt > dt1) {
                alert(obj3)
                d = dt1.getDate() - 1
                dt1.setDate(d);
                obj1.value = dt1.format("dd/MM/yyyy");
                obj1.focus();
                return false
            }
        }
        function CheckDateGreater(obj1, obj2, obj3) {
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
    <script type ="text/javascript" >


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

     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table id="PageTbl" class="PageTable" border="1">
                <tr id="TitleOfPage">
                    <td class="PageTitle" align="center" style="width: 14%">
                        <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                    </td>
                </tr>
                <tr id="ToolbarPanel">
                    <td style="width: 14%">
                        <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TblControl">
                    <td style="width: 14%">
                        <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="tblLocationDetails" runat="server" class="ContainTable" border="1">
                                <tr>
                                    <td>
                                        <asp:Panel ID="PanelLocation" runat="server">
                                            <uc6:Location ID="Location" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div id="divSelectProforma" runat="server">
                            <asp:Panel ID="RFQSelection" runat="server" BorderColor="Black" BorderStyle="Double">
                                <div style="text-align: left; padding-top: 5px; padding-bottom: 5px">
                                    <table id="Table2" runat="server" class="ContainTable" border="1">
                                        <tr>
                                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="6">
                                                <asp:Panel ID="Panel1" runat="server">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" class="tdLabel">
                                                <asp:Label ID="lblActStatus" runat="server" Text="Activity Status:"></asp:Label>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:DropDownList ID="drpActivityCalimStatus" runat="server" CssClass="ComboBoxFixedSize"   onchange="OnStateChange()"> <%--AutoPostBack ="true" --%>
                                                    <asp:ListItem Value="0">All</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="P">Pending</asp:ListItem>
                                                    <asp:ListItem Value="Y">Approved</asp:ListItem>
                                                    <asp:ListItem Value="R">Return</asp:ListItem>
                                                             </asp:DropDownList>
                                                <b class="Mandatory">*</b><asp:Button ID="btnShow" Text="Show" runat="server" CssClass="CommandButton "
                                                    OnClick="btnShow_Click" OnClientClick="return Validation();" />
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td class="tdLabel" style="width: 18%">
                                                &nbsp;
                                            </td>
                                            <td style="width: 15%"  colspan ="2">
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
                                    <td colspan="2"><span style ="font-weight:bold"> Note : Days in line level is showing as per status </span> </td>
                                    <td id="Td1" align="center"  style="height: 15px" colspan="4">
                                   Search By <asp:DropDownList ID ="drpActivity" runat ="server" CssClass ="ComboBoxFixedSize">
                                   <asp:ListItem Value="1" Selected="True" >Activity No</asp:ListItem>
                                   <asp:ListItem Value="2">Activity Name</asp:ListItem>
                                   </asp:DropDownList> <asp:TextBox ID="txtSearchText" runat ="server" CssClass="TextBoxForString" ></asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" CssClass="CommandButton"  
                                            Text="Search" onclick="btnSearch_Click" OnClientClick ="return CheckText()"  />
                                        <asp:Button ID="btnClearSearch" runat="server" CssClass="CommandButton" 
                                            Text="Clear Search" OnClientClick ="return ClearSearch()" 
                                            onclick="btnClearSearch_Click" />
                                    </td>                                                                      
                                </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                ScrollBars="Vertical">
                                <table id="Table1" runat="server" class="ContainTable" border="1" width ="100%">
                                    <tr>
                                        <td id="ClaimGrid" align="center" class="ContaintTableHeader" style="height: 15px;width:99%" >
                                            <asp:Label ID="lblTtlActivityDetails" runat="server" Text="Activity Details"></asp:Label>
                                        </td>
                                        <td width="1%">
                                            <asp:Image ID="ImgTtlActivityDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                                Height="15px" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%;" align="center">
                                            <asp:Panel ID="GridDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                                ScrollBars="Vertical">
                                                <asp:GridView ID="ActivityClaim" runat="server" GridLines="Horizontal" Width="100%"
                                                    AllowPaging="True" AlternatingRowStyle-Wrap="true" 
                                                    EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                                    AutoGenerateColumns="False" 
                                                    onpageindexchanging="ActivityClaim_PageIndexChanging">
                                                    <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="3%">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png"
                                                                    OnClientClick='Show(this);' Style="height: 16px" OnClick="ImgSelect_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                            <asp:Label ID="lblDealerID" CssClass="DispalyNon" runat="server" Text='<%# Eval("Dealer_ID") %>' />
                                                                <asp:Label ID="lblDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="9%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Dealer_Code") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Activity No" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ActivityId" CssClass="DispalyNon" runat="server" Text='<%# Eval("ID") %>' />
                                                                <asp:Label ID="lblActivityNo" runat="server" Text='<%# Eval("Activity_Req_No") %>' />
                                                                <asp:Label ID="lblRegionID" CssClass="DispalyNon" runat="server" Text='<%# Eval("Region_ID") %>' />
                                                                 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Activity Name" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblActivityName" runat="server" CausesValidation="false" CommandName="Select"
                                                                    Text='<%# Eval("Activity_Name") %>' CommandArgument="<%# Container.DataItemIndex %>"
                                                                    OnClientClick='Show(this);'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Activity Date" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActivityDate" runat="server" Text='<%# Eval("Activity_Req_Date") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Activity Type" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActivityType" runat="server" Text='<%# Eval("ActivityType") %>' />
                                                                <asp:Label ID="lblActivityTypeId" CssClass="DispalyNon" runat="server" Text='<%# Eval("ActivityTypeID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Days" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDays" runat="server" Text='<%# Eval("PendingDays") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle Wrap="True" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="GridDetails_CollapsiblePanelExtender" runat="server"
                                Enabled="True" TargetControlID="GridDetails" CollapseControlID="ClaimGrid" ExpandControlID="ClaimGrid"
                                Collapsed="false" ImageControlID="ImgTtlActivityDetails" ExpandedImage="~/Images/Minus.png"
                                CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Activity Details"
                                ExpandedText="Hide Activity Details" TextLabelID="lblTtlActivityDetails">
                            </cc1:CollapsiblePanelExtender>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 14%">
                    </td>
                </tr>
                <tr id="TmpControl">
                    <td style="width: 14%">
                        <asp:TextBox ID="txtControlCount" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                        <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                        <asp:TextBox ID="txtID" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                        <asp:TextBox ID="txtFlage" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                        <asp:TextBox ID="txtApprProcc" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                       <asp:TextBox ID="txtUserRoleID" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox> 
                        <asp:TextBox ID="txtDeptId" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                        <asp:TextBox ID="txtUserDeptId" runat="server" Width="1px" Text="" CssClass ="HideControl"></asp:TextBox>
                    </td>
                </tr>
            </table>
</asp:Content>
