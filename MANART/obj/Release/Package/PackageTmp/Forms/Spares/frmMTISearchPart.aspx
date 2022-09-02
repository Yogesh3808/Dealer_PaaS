<%@ Page Title="MTI-Search Part" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="frmMTISearchPart.aspx.cs" EnableEventValidation="false" Theme="SkinFile" Inherits="MANART.Forms.Spares.frmMTISearchPart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script type="text/javascript">
        function ShowReportPartSearch(Menuid) {
            var iDocId = 0;
            var sExportYesNo = "";
            var RptTitle = "";
            var msgAnswer = "";
            var iPartNo = "";
            var Region = 0;
            var imenuid = 0;
            var UserRoleID = 0;
            var sReportName = "";

            var Region = document.getElementById('ContentPlaceHolder1_drpRegion');
            //Megha24012012
            var PartNo = document.getElementById('ContentPlaceHolder1_txtPartNo');

            //Megha24012012



            if (PartNo == null) return;
            if (PartNo.value == "") {
                alert("Please Select The Record For Print!");
                return false;
            }
            else {
                sReportName = "/RptDCSSearch&";
                iPartNo = PartNo.value;
                iRegion = Region.value;
                sFPDAYesNo = "N";
                sExportYesNo = "Y"
            }
            //var Url = "/DCS/Forms/Common/frmDocumentView.aspx?RptName=/DCSReports";
            alert("Work in Process...");

            if (sReportName == "") {
                return false;
            }
            //Url = Url + sReportName + "ID=" + iDocId + "&PartNo=" + iPartNo + "&UserRoleId=" + iUserRoleId + "&Menuid=" + Menuid + "&ExportYesNo=" + sExportYesNo + "";
            Url = Url + sReportName + "ID=" + iDocId + "&FPDAYesNo=" + sFPDAYesNo + "&RegionID=" + iRegion + "&UserRoleID=" + UserRoleID + "&ProdCat=" + iPartNo + "&Menuid=" + Menuid + "&ExportYesNo=" + sExportYesNo + "";

            //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No;    scroll: Yes; status: Yes;");
            var windowFeatures;
            window.opener = self;
            //window.close()  
            windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
            newWindow = window.open(Url, "", windowFeatures)
            window.moveTo(0, 0);
            window.resizeTo(screen.width, screen.height - 100);
            newWindow.focus();
            return false;
        }



    </script>
    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                event.returnValue = false;
                alert(message);
                return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }




        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');

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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="MTI-Search Part"> </asp:Label>
            </td>
        </tr>

        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="LocationDetails" runat="server">
                    <%--<uc2:Location ID="Location" runat="server" Visible ="false" />--%>
                </asp:Panel>
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Search Part" ExpandedText="Search Part"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Search Part" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
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
                                <td class="tdLabel" style="width: 15%; padding-left: 15px">Region:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpRegion" Width="175px" runat="server" CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel" style="width: 12%">Search Stock by Part No:
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox ID="txtPartNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 70%" class="tdLabel">
                                    <asp:Button ID="btnSearchPart" runat="server"
                                        Text="Search" CssClass="btn btn-search" OnClick="btnSearchPart_Click" />
                                    <asp:Label ID="lblConfirm" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                    <asp:Button ID="btnPrint" runat="server"
                                        Text="Print" CssClass="btn btn-search" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="false"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="false" CollapsedText="Part Details" ExpandedText="Part Details"
                        TextLabelID="lblTtlPartDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="PartGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="99%"  AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" >
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartID" runat="server" Width="5%" Text='<%# Eval("Part_ID") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtstateCode" runat="server" Text='<%# Eval("State") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEGPDealerCode" runat="server" Text='<%# Eval("Dealer_Spares_Code") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEGPDealerName" runat="server" Text='<%# Eval("Dealer_Name") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contact No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEGPContactNo" runat="server" Text='<%# Eval("Dealer_LandLinePhone") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Part No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartNo" runat="server" Text='<%# Eval("Part_No") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="32%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPartName" runat="server" Text='<%# Eval("Part_Name") %>' Width="90%"
                                            CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="32%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stock" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Qty","{0:#0.00}") %>'
                                            Width="90%" onkeypress=" return CheckForTextBoxValue(event,this,'5');" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Rate" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMRPRate" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                            Text='<%# Eval("MRPRate","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox></ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                            Text='<%# Eval("Total","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>

                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
