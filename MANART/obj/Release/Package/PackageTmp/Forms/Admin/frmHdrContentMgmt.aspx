<%@ Page Title="MTI-Content Management" Theme="SkinFile" EnableViewState="true" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmHdrContentMgmt.aspx.cs" Inherits="MANART.Forms.Admin.frmHdrContentMgmt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                // event.returnValue = false;
                //alert(message);
                // return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }
        function AtPageLoad() {
            //FirstTimeGridDisplay('ContentPlaceHolder1_');

            //setTimeout("disableBackButton()", 0);
            //disableBackButton();
            return true;
        }

        function refresh() {
            if (event.keyCode == 116 || event.keyCode == 8) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }


        function ShowContent(para) {
            var ID = para.parentNode.parentNode.childNodes[1].all[0].innerText;
            var PartDetailsValue;
            PartDetailsValue = window.showModalDialog("frmDtlsContentMgmt.aspx?ID=" + ID, "List", "dialogHeight: 500px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
        }

    </script>
    <script type="text/javascript">
        function disableBackButton() {
            window.history.forward(1);
        }

    </script>

    <script type="text/javascript">

        function pageLoad() {
            $(document).ready(function () {

                var txtEffFromDate = document.getElementById("ContentPlaceHolder1_txtEffFromDate_txtDocDate");
                var txtEffToFromDate = document.getElementById("ContentPlaceHolder1_txtEffToFromDate_txtDocDate");
                $('#ContentPlaceHolder1_txtEffFromDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtEffToFromDate.value, minDate: (txtEffFromDate.value == '') ? '0d' : txtEffFromDate.value
                });

                $('#ContentPlaceHolder1_txtEffToFromDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtEffFromDate.value == '') ? '0d' : txtEffFromDate.value
                });

                function customRange(dates) {
                    var objDate = new Date(dates[0]);
                    if (this.id == 'ContentPlaceHolder1_txtEffFromDate_txtDocDate') {
                        objDate.setDate(objDate.getDate() + 60);
                        $('#ContentPlaceHolder1_txtEffToFromDate_txtDocDate').datepick('option', 'minDate', objDate || null);
                        $('#ContentPlaceHolder1_txtEffToFromDate_txtDocDate').datepick('setDate', dates[0], objDate)
                    }
                    else {
                        objDate.setDate(objDate.getDate() - 60);
                        $('#ContentPlaceHolder1_txtEffFromDate_txtDocDate').datepick('option', 'maxDate', objDate || null);
                        $('#ContentPlaceHolder1_txtEffFromDate_txtDocDate').datepick('setDate', dates[0], objDate)

                    }
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
            <%--<tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
    </tr>--%>
            <tr id="TblControl">
                <td style="width: 15%">
                    <asp:Panel ID="PChassisHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisHeaderDetails" runat="server" TargetControlID="CntChassisHeaderDetails"
                            ExpandControlID="TtlChassisHeaderDetails" CollapseControlID="TtlChassisHeaderDetails"
                            Collapsed="false" ImageControlID="ImgTtlChassisHeaderDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Content Header Details"
                            ExpandedText="Hide Content Header Details" TextLabelID="lblTtlChassisHeaderDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisHeaderDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlChassisHeaderDetails" runat="server" Text="Content Header Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntChassisHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="Table1" runat="server" class="ContainTable">
                                <tr>
                                    <td style="width: 15%; height: 15px;" class="tdLabel">File  Name:
                                    </td>
                                    <td style="width: 18%; height: 15px;" colspan="2">
                                        <asp:TextBox ID="txtFileName" runat="server" CssClass="TextBoxForString"
                                            Text="" Style="display: none" TabIndex="1"></asp:TextBox>
                                        <asp:FileUpload ID="fupFileName" runat="server" CssClass="TextBoxForString" TabIndex="1" Width="86%" /><%--<b class="Mandatory">*</b> --%>                           
                                    </td>

                                    <td style="width: 18%; height: 15px;" colspan="3">
                                        <b style="color: red">(Note : 'File upload' and 'File name' size limit upto '15MB' and '100' Character.)</b>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 15px;" class="tdLabel">Document Name:
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtDocumentName" runat="server" CssClass="TextBoxForString"
                                            Text="" TabIndex="2"></asp:TextBox><b class="Mandatory">*</b>
                                    </td>
                                    <td style="width: 15%; height: 15px;" class="tdLabel">Document Heading:
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtDocumentHeading" runat="server" CssClass="TextBoxForString"
                                            Text="" TabIndex="3"></asp:TextBox><b class="Mandatory">*</b>
                                    </td>
                                    <td class="tdLabel" style="width: 15%; height: 17px;">Department Name:
                                    </td>
                                    <td style="width: 18%; height: 17px;">
                                        <asp:DropDownList ID="drpDept" runat="server" CssClass="ComboBoxFixedSize" TabIndex="4"></asp:DropDownList><b class="Mandatory">*</b>
                                    </td>

                                </tr>
                                <tr>

                                    <td style="width: 15%; height: 17px;" class="tdLabel">Effective Date From:
                                    </td>
                                    <td style="width: 18%; height: 17px;">
                                        <uc3:CurrentDate ID="txtEffFromDate" runat="server" Visible="true" Mandatory="true" bCheckforCurrentDate="true" TabIndex="5" />
                                    </td>
                                    <td style="width: 15%; height: 17px;" class="tdLabel">Effective Date To:
                                    </td>
                                    <td style="width: 18%; height: 17px;">
                                        <uc3:CurrentDate ID="txtEffToFromDate" runat="server" Visible="true" Mandatory="true" bCheckforCurrentDate="false" TabIndex="6" />
                                    </td>
                                    <td style="width: 18%; height: 15px;">&nbsp;
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="tdLabel" style="width: 15%; height: 15px;">Active Status:
                                    </td>
                                    <td style="width: 18%; height: 15px;">
                                        <asp:TextBox ID="txtActiveStatus" runat="server" CssClass="TextBoxForString"
                                            Text="N"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 15%; height: 15px;">&nbsp;
                                    </td>
                                    <td style="width: 18%; height: 15px;">&nbsp;
                                    </td>
                                    <td style="width: 15%; height: 15px;" class="tdLabel">&nbsp;
                                    </td>
                                    <td style="width: 18%; height: 15px;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel"></td>
                                    <td style="width: 18%" class="tdLabel">
                                        <asp:Button ID="bSave" runat="server" Text="Save" CssClass="CommandButton"
                                            OnClick="bSave_Click" TabIndex="7" />
                                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="CommandButton"
                                            TabIndex="8" OnClick="btnConfirm_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="New" CssClass="CommandButton"
                                            OnClick="btnCancel_Click" TabIndex="9" />
                                    </td>
                                    <td class="tdLabel" style="width: 12%;"></td>
                                    <td style="width: 18%">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="PChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEChassisFSDetails" runat="server" TargetControlID="CntChassisFSDetails"
                            ExpandControlID="TtlChassisFSDetails" CollapseControlID="TtlChassisFSDetails" Collapsed="false"
                            ImageControlID="ImgTtlChassisFSDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Show Content Details" ExpandedText="Hide Content Details"
                            TextLabelID="lblTtlChassisFSDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlChassisFSDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlChassisFSDetails" runat="server" Text="Content Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlChassisFSDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="Horizontal">
                            <asp:GridView ID="gvFSDetails" runat="server" GridLines="Horizontal" 
                                Width="100%" AllowPaging="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true"
                                OnRowCommand="gvFSDetails_RowCommand"
                                OnRowDeleting="gvFSDetails_RowDeleting" OnPageIndexChanging="gvFSDetails_PageIndexChanging">
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" CommandName="ImgSelect" CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="10%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" CssClass="DispalyNon" runat="server" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File  Name" ItemStyle-Width="13%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Name" ItemStyle-Width="21%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("Document_Name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Heading" ItemStyle-Width="21%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocument_Heading" runat="server" Text='<%# Eval("Document_Heading") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department Name" ControlStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDept" runat="server" Text='<%# Eval("DepartmentName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date From" ItemStyle-Width="11%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFromDt" runat="server" Text='<%# Eval("Effective_Date_From","{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date To" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblToDt" runat="server" Text='<%# Eval("Effective_Date_To","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField HeaderText="Active Status" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Eval("Active_Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField ItemStyle-Width="3%" HeaderText="Delete" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lnkDelete" runat="server" ToolTip="Click to Delete Document" CommandArgument='<%# Eval("ID") + "/" + Eval("File_Name") %>' CommandName="Delete" ImageUrl="~/Images/delete.png" OnClientClick="javascript:return confirm('Are you sure you want to delete this Document!')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="11%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDealer" runat="server" Text="Dealer Details" CommandArgument='<%# Eval("ID") %>' OnClientClick='ShowContent(this);'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
