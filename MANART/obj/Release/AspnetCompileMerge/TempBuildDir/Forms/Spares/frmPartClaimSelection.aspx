<%@ Page Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmPartClaimSelection.aspx.cs"
    Title="MTI-Part Claim Processing" EnableEventValidation="false" ValidateRequest="false" Inherits="MANART.Forms.Spares.frmPartClaimSelection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script type="text/javascript">
        // Vikram 15122017
       <%-- function CheckedClaim(Obj, Obj1, Obj2) {
            var GridParClaim = document.getElementById("<%=GridParClaim.ClientID%>");
            var hdnRememberValue = document.getElementById("<%=hdnRememberValue.ClientID%>");
            var GridID = Obj.id.replace(Obj1, Obj2)
            var Elements = GridParClaim.getElementsByTagName("input");
            var Span = GridParClaim.getElementsByTagName("span");
            var dynamic_array = new Array();
            var splElements;
            var splSpan;
            for (var i = 0; i < Elements.length; i++) {
                if (Elements[i].type == 'checkbox' && Elements[i].id == Obj.id && Obj.checked) {
                    for (var j = 0; j < Span.length; j++) {
                        if (Span[j].id == GridID) {
                            if (hdnRememberValue.value == "")
                                hdnRememberValue.value = Span[j].innerHTML;
                            else
                                hdnRememberValue.value = hdnRememberValue.value + "," + Span[j].innerHTML;
                            break
                        }

                    }

                }
                else if (Elements[i].type == 'checkbox' && Elements[i].id == Obj.id && !Obj.checked) {
                    var splValue = "";
                    if (hdnRememberValue.value != "") {
                        splValue = hdnRememberValue.value.split(",");
                    }
                    hdnRememberValue.value = "";
                    for (var j = 0; j < Span.length; j++) {
                        if (Span[j].id == GridID) {
                            for (k = 0; k < splValue.length; k++) {
                                if (splValue[k] == Span[j].innerHTML)
                                    splValue.splice(k, 1);
                            }
                            break
                        }

                    }
                    for (x = 0; x < splValue.length; x++)
                        if (hdnRememberValue.value == "")
                            hdnRememberValue.value = splValue[x];
                        else
                            hdnRememberValue.value = hdnRememberValue.value + "," + splValue[x];

                }

            }
        }--%>
        function ShowPartClaim(para) {
            //debugger;
            //var ID = para.parentNode.parentNode.childNodes[3].all[0].innerText;
            //var Confirm = para.parentNode.parentNode.childNodes[3].all[1].innerText;
            //var IsPartClaimMgr = para.parentNode.parentNode.childNodes[3].all[2].innerText;
            //var IS3PL = para.parentNode.parentNode.childNodes[3].all[3].innerText;
            //var PClmType = para.parentNode.parentNode.childNodes[4].all[0].innerHTML;

            var ID = para.parentNode.parentNode.childNodes[4].children[0].innerText;
            var Confirm = para.parentNode.parentNode.childNodes[4].children[1].innerText;
            var IsPartClaimMgr = para.parentNode.parentNode.childNodes[4].children[2].innerText;
            var IS3PL = para.parentNode.parentNode.childNodes[4].children[3].innerText;
            var PClmType = para.parentNode.parentNode.childNodes[5].children[0].innerHTML;
            var PClmTypeID = para.parentNode.parentNode.childNodes[5].children[1].innerHTML;
            var DealerCode = para.parentNode.parentNode.childNodes[2].children[1].innerHTML;
            var DealerID = para.parentNode.parentNode.childNodes[2].children[2].innerHTML;
            var Fin_No = para.parentNode.parentNode.childNodes[2].children[3].innerHTML;

            //Vikram Dated 15122017
            <%-- if (document.getElementById("<%=txtRoleID.ClientID%>").value == "12" && IsPartClaimMgr == "P") {
                alert("Please Check whether Part Claim Manager Processed this PartClaim! ")
                return false;
            }
            else if (document.getElementById("<%=txtRoleID.ClientID%>").value == "11" && IS3PL == "P") {
                alert("Please Check whether 3PL Processed this PartClaim! ")
                return false;
            }
            else--%>
            var Parameters = "ID=" + ID + "&Confirm=" + Confirm + "&PClmType=" + PClmType + "&sDealerCode=" + DealerCode + "&sDealerID=" + DealerID + "&sFinNo=" + Fin_No;
            var feature = "dialogHeight: 700px; dialogWidth: 1300px; dialogTop: 50px; dialogLeft: 50px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;";
            window.showModalDialog("frmPartClaimProcessing.aspx?" + Parameters, null, feature);

            //window.showModalDialog("frmPartClaimProcessing.aspx?ID=" + ID + "&Confirm=" + Confirm + "&PClmType=" + PClmType + "&sDealerCode=" + DealerCode + "sDealerID=" + DealerID + "sFinNo=" + Fin_No, "List", "dialogHeight: 700px; dialogWidth: 1300px; dialogTop: 50px; dialogLeft: 50px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            window.location = window.location;
        }
        //Vikram 15122017
        function PartClaimSelVal() {
            var drpClaimType = document.getElementById("<%=drpClaimType.ClientID%>");
            var ClaimType = drpClaimType.options[drpClaimType.selectedIndex]
            if (ClaimType.value == "0") {
                alert("Please Select Atleast One Part Claim!");
                drpClaimType.focus();
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        function OnStateChange() {
            var trDate = document.getElementById("<%=trDate.ClientID%>");
            var drpClaimStatus = document.getElementById("<%=drpPartClaimStatus.ClientID %>");
            var ClaimStatusVal = drpClaimStatus.options[drpClaimStatus.selectedIndex].text

            if (ClaimStatusVal == "Pending")
                trDate.style.display = "none"
            else {
                trDate.style.display = ""
            }
        }
        function ClearSearch() {
            document.getElementById("<%=txtSearchText.ClientID %>").value = '';
        }
        function CheckText() {
            var txtSearchText = document.getElementById("<%=txtSearchText.ClientID %>")
            if (txtSearchText.value == '')
                return false;
        }
        function LocationValidation() {
            var drpRegion = document.getElementById("<%=Location.ClientID %>_drpRegion");
            var RegionVal = drpRegion.options[drpRegion.selectedIndex].text
            if (RegionVal == "--Select--") {
                alert("Please Select Region")
                return false;
            }
            var drpCountry = document.getElementById("<%=Location.ClientID %>_drpCountry");
            var CountryVal = drpCountry.options[drpCountry.selectedIndex].text
            if (CountryVal == "--Select--") {
                alert("Please Select Country")
                return false;
            }
            var txtDealerName = document.getElementById("<%=Location.ClientID %>_txtDealerName");
            if (txtDealerName.value == "--Select--") {
                alert("Please Select Dealer")
                return false;
            }

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
        function Validation() {
            //debugger;
            var hdnRememberValue = document.getElementById("<%=hdnRememberValue.ClientID%>");
           if (hdnRememberValue.value == "") {
               alert("Please Select Atleast One Part Claim");
               return false;
           }
       }

    </script>

    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
                //debugger;
                var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
                var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
                $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value, autoSize: true
                    //dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value
                });

                $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtFromDate.value
                    //dateFormat: 'dd/mm/yyyy', minDate: txtFromDate.value
                });

                function customRange(dates) {
                    //debugger;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <table id="PageTbl" class="PageTable" border="1">
                <tr id="TitleOfPage" class="panel-heading">
                    <td class="PageTitle panel-title" align="center" style="width: 14%">
                        <asp:Label ID="lblTitle" runat="server" Text="Part Claim Processing"> </asp:Label>
                    </td>
                </tr>
                <tr id="ToolbarPanel">
                    <td style="width: 14%">
                        <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                            <tr>
                                <td></td>
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

                                <table id="Table2" runat="server" class="ContainTable table table-bordered" border="1">
                                    <tr>
                                        <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="6">
                                            <asp:Panel ID="Panel1" runat="server">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%;" class="tdLabel">Claim Status:
                                        </td>
                                        <td style="width: 10%;" class="tdLabel">
                                            <asp:DropDownList ID="drpPartClaimStatus" runat="server"
                                                CssClass="ComboBoxFixedSize" onchange="OnStateChange()">
                                                <%-- AutoPostBack ="true" --%>
                                                <%-- <asp:ListItem Selected="True" Value="N">Pending</asp:ListItem>--%>
                                                <asp:ListItem Value="A">All</asp:ListItem>
                                                <asp:ListItem Value="N" Selected="True">Pending</asp:ListItem>
                                                <asp:ListItem Value="Y">Approved/Submitted</asp:ListItem>
                                                <asp:ListItem Value="R">Rejected</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 5%;" class="tdLabel">Claim Type:
                                        </td>
                                        <td style="width: 10%;" class="tdLabel">
                                            <asp:DropDownList ID="drpClaimType" runat="server" CssClass="ComboBoxFixedSize">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 5%;" class="tdLabel">
                                            <asp:Button ID="btnClaimRequest" runat="server" Text="Show"
                                                CssClass="btn btn-search" OnClick="btnClaimRequest_Click" OnClientClick="return PartClaimSelVal();" />
                                        </td>
                                        <td style="width: 18%;" class="tdLabel">

                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trDate" runat="server">
                                        <td class="tdLabel" style="width: 5%">From:
                                        </td>
                                        <td style="width: 10%" class="tdLabel">
                                            <uc3:CurrentDate ID="txtFromDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                        </td>
                                        <td style="width: 5%" class="tdLabel">To:
                                        </td>
                                        <td style="width: 10%" class="tdLabel">
                                            <uc3:CurrentDate ID="txtToDate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                        </td>
                                        <td class="tdLabel" style="width: 10%">&nbsp;
                                        </td>
                                        <td style="width: 10%">&nbsp;
                                        </td>
                                    </tr>
                                </table>

                            </asp:Panel>
                            <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                ScrollBars="Vertical">
                                <table id="Table1" runat="server" class="ContainTable" border="1">
                                    <tr>
                                        <td id="Td1" align="center" style="height: 15px">Search By Request Claim No or Invoice No
                                            <asp:TextBox ID="txtSearchText" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-search"
                                                Text="Search" OnClientClick="return CheckText()"
                                                OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-search"
                                                Text="Clear Search" OnClientClick="return ClearSearch()"
                                                OnClick="btnClearSearch_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btnSendPartClaimTo3PL" runat="server" CssClass="btn btn-search HideControl"
                                            Text="Send PartClaim To 3PL" OnClientClick="return Validation()" OnClick="btnSendPartClaimTo3PL_Click" />
                                        </td>
                                    </tr>
                                    <tr class="panel-heading">
                                        <td id="ClaimGrid" align="center" class="ContaintTableHeader panel-title" style="height: 15px" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);">Part Claim
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%;" align="center">
                                            <asp:Panel ID="GridDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                                ScrollBars="Vertical">
                                                <asp:GridView ID="GridParClaim" runat="server" GridLines="Horizontal" Width="100%"
                                                    AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                                    AutoGenerateColumns="False" AllowPaging="true"
                                                    OnPageIndexChanging="GridParClaim_PageIndexChanging"
                                                    OnRowDataBound="GridParClaim_RowDataBound">
                                                    <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png"
                                                                    OnClientClick='ShowPartClaim(this);' Style="height: 16px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="SrNo" ItemStyle-Width="5%">--%>
                                                        <asp:TemplateField HeaderText="Dealer code" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />--%>
                                                                <asp:Label ID="lblNo" CssClass="DispalyNon" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                                <asp:Label ID="lblDealerGrCode" runat="server" Text='<%# Eval("DealerCode")  %>' />
                                                                <asp:Label ID="lblDealerID" runat="server" CssClass="DispalyNon" Text='<%# Eval("dealer_Id")  %>'></asp:Label>
                                                                <asp:Label ID="lblFinNo" runat="server" CssClass="DispalyNon" Text='<%# Eval("Fin_No")  %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerGrName" runat="server" Text='<%# Eval("Dealer_Name") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Request Claim No" ItemStyle-Width="7%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" CssClass="DispalyNon" runat="server" Text='<%# Eval("ID") %>' />
                                                                <asp:Label ID="lblStatus" CssClass="DispalyNon" runat="server" Text='<%# Eval("Approval_Status") %>' />
                                                                <asp:Label ID="lblIsPartClaimMgr" CssClass="DispalyNon" runat="server" Text='<%# Eval("Is_PartClaimMgr") %>' />
                                                                <asp:Label ID="lblIs3PL" CssClass="DispalyNon" runat="server" Text='<%# Eval("Is_3PL") %>' />
                                                                <asp:Label ID="lblPartClaimNo" runat="server" Text='<%# Eval("part_claim_no") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Claim Type" ItemStyle-Width="7%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPartClaimType" runat="server" Text='<%# Eval("Claim Type") %>' />
                                                                <asp:Label ID="lblPartClaimID" runat="server"  CssClass="DispalyNon" Text='<%# Eval("ClaimType_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Claim Date" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPartClaimDate" runat="server" Text='<%# Eval("claim_date") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Claim Amount" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPartClaimAmout" runat="server" Text='<%# Eval("claim_amt","{0:#0.00}") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInv_no" runat="server" Text='<%# Eval("Inv_no") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInv_Date" runat="server" Text='<%# Eval("Inv_date") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Part Claim Status" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatusDesc" runat="server" Text='<%# Eval("Approval_Status_Desc") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Select Claim" ItemStyle-Width="2%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckedClaim(this,'chkSelect','lblId')" />
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
                                Collapsed="false" AutoExpand="true">
                            </cc1:CollapsiblePanelExtender>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 14%"></td>
                </tr>
                <tr id="TmpControl">
                    <td style="width: 14%">
                        <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtID" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtFlage" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtRoleID" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:HiddenField ID="hdnRememberValue" runat="server" Value="" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
