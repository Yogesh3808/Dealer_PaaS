<%@ Page Title="MTI-Part Price Master" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmPartPriceMaster.aspx.cs" Inherits="MANART.Forms.Master.frmPartPriceMaster" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridViewForPriceMaster.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
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
            FirstTimeGridDisplay('ContentPlaceHolder1_');

            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }

        function refresh() {
            if (event.keyCode == 116) {
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
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="Part Price">
                </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="TblControl">
            <td style="width: 15% Height: 45%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" width="100%" runat="server">
                        <tr>
                            <td>
                                <uc2:Location ID="Location" runat="server" OnDDLSelectedIndexChanged="Location_DDLSelectedIndexChanged" OndrpRegionIndexChanged="Location_drpRegionChanged" />
                            </td>
                        </tr>
                        <%--<tr>
                                <td align ="center" >
                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="CommandButton" 
                                    onclick="btnShow_Click" />
                                        <asp:Label ID="lblConfirm" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>--%>
                    </table>
                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table3" runat="server" class="table table-responsive table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Select File:
                                </td>
                                <%--  OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');" --%>
                                <td style="width: 18%">
                                    <asp:FileUpload ID="txtFilePath" runat="server" />
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-search"
                                        OnClick="btnUpload_Click" /><br />
                                    <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

                <cc1:CollapsiblePanelExtender ID="CPESelection" runat="server" TargetControlID="PSelectionGrid"
                    ExpandControlID="TtlSelection" CollapseControlID="TtlSelection" Collapsed="true"
                    ImageControlID="ImgTtlSelection" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                    SuppressPostBack="true" TextLabelID="lblTtlSelection">
                </cc1:CollapsiblePanelExtender>
                <asp:Panel ID="TtlSelection" runat="server">
                    <table width="100%">
                        <tr class="panel-heading">
                            <td align="center" class="panel-title" width="96%">
                                <asp:Label ID="lblTtlSelection" runat="server" Text="" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                    onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                            </td>
                            <td width="1%">
                                <asp:Image ID="ImgTtlSelection" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                    Width="100%" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <table width="100%" class="table-responsive">
                        <tr>
                            <td>
                                <table class="table table-bordered">
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="GridTitle" runat="server" Text="Search: "></asp:Label>
                                            <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="ComboBoxFixedSize">
                                                <asp:ListItem Text="Part No"></asp:ListItem>
                                                <asp:ListItem Text="Part Name"></asp:ListItem>
                                                <asp:ListItem Text="Dealer Code"></asp:ListItem>
                                                <asp:ListItem Text="Dealer Name"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <div style="float: left">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                            </div>
                                            <div id="divDateSearch" runat="server" style="float: left;" visible="false">

                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFrom" runat="server" Text="From: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <uc3:CurrentDate ID="txtDocDateFrom" runat="server" Mandatory="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTo" runat="server" Text="To: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <uc3:CurrentDate ID="txtDocDateTo" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="div1" runat="server" style="float: left; padding-left: 5px;">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="60px" CssClass="btn btn-search"
                                                    OnClick="btnSearch_Click" />
                                                <asp:TextBox ID="txtUseDate" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search" CssClass="btn btn-search"
                                                    Visible="false" OnClick="btnClearSearch_Click" />
                                                <asp:Label ID="lblConfirm" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblSort" Text="Sort" runat="server"></asp:Label>
                                                <asp:DropDownList ID="drpSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSort_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Value="Asc">Ascending</asp:ListItem>
                                                    <asp:ListItem Value="Desc">Descending</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblSorted" Text="By Document No" runat="server"></asp:Label>
                                                &nbsp;<%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Search_Progress.gif" AlternateText="Searhing...." />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>--%><asp:HiddenField ID="hdnSort" runat="server" Value="N" />
                                            </div>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div id="gridView" class="grid">
                        <asp:GridView ID="SearchGrid1" runat="server" CssClass="datatable table table-bordered" CellPadding="0" DataKeyNames="ID"
                            CellSpacing="0" GridLines="Horizontal" Style="border-collapse: separate;" HeaderStyle-CssClass="GridViewHeaderStyle"
                            AlternatingRowStyle-CssClass="odd" RowStyle-CssClass="even" AllowPaging="True" AutoGenerateColumns="false" PageSize="10" OnPageIndexChanging="SearchGrid1_PageIndexChanging">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnedit" runat="server" ImageUrl="~/Images/arrowRight.png" OnClick="btnedit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Part No" DataField="Part No" ItemStyle-Width="8%" />
                                <asp:BoundField HeaderText="Part Name" DataField="Part Name" ItemStyle-Width="15%" />
                                <asp:BoundField HeaderText="Dealer Code" DataField="Dealer Code" />
                                <asp:BoundField HeaderText="Dealer Name" DataField="Dealer Name"  ItemStyle-Width="15%"/>
                                <asp:BoundField HeaderText="From Date" DataField="From Date" />
                                <asp:BoundField HeaderText="To Date" DataField="To Date" />
                                <asp:BoundField HeaderText="Category" DataField="Part_cat" />
                                <asp:BoundField HeaderText="HSN Code" DataField="HSN_Code" ItemStyle-Width="6%" />
                                  <asp:TemplateField HeaderText="GST(%)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMRP1" runat="server" Text='<%# Eval("GSTTaxPer","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField HeaderText="NDP/DLP" DataField="NDP (INR)" />
                                <asp:BoundField HeaderText="Dealer Margin(%)." DataField="" />
                                <asp:BoundField HeaderText="Dealer Margin Value(On NDP)" DataField="" />
                                <asp:BoundField HeaderText="LIST" DataField="LIST (INR)" />
                                <asp:BoundField HeaderText="MRP" DataField="MRP (INR)" />
                                <asp:BoundField HeaderText="Active" DataField="Active" />
                                <asp:TemplateField HeaderText="Parts Price History">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Btnshowhistory" runat="server" ImageUrl="~/Images/History.png" OnClick="Btnshowhistory_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle"></HeaderStyle>
                            <AlternatingRowStyle CssClass="odd"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                        ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                        Collapsed="true" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Part Price Header Details"
                        ExpandedText="Part Price Header Details" TextLabelID="lblTtlPartHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlPartHeaderDetails" runat="server" Text="Part Header Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" width="100%">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Part No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Part Name:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPartName" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Dealer Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%">Dealer:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDealer" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Effective From:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Effective To:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEffectiveTo" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true" Text="" Width="180px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">LIST:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLISTPRICE" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">MRP:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMRP" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">NDP/DLP:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtNDP" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                 <td style="width: 15%" class="tdLabel">Category:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCategory" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">HSN Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtHSNCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">GST(%):
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPer" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>


                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">Dealer Margin(%):
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDealerMargin" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Dealer Margin Value(On NDP):
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtdealerMarginvalue" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Active:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtActive" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>


                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">dCAN Update Date:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDCSUpdateDate" runat="server" CssClass="TextBoxForString" ReadOnly="true" Width="180px"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">XML Creation Date:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtXMLCreationDate" runat="server" CssClass="TextBoxForString" ReadOnly="true" Width="180px"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>

        <tr id="TmpControl">
            <td style="width: 15%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                    Text=""></asp:TextBox>
                <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                </asp:DropDownList>
            </td>
        </tr>

    </table>
    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lblTragetID2" PopupControlID="pnlPopupWindow2"
        OkControlID="btnOK2" BackgroundCssClass="modalBackground" BehaviorID="mpe2">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID2" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow2" runat="server" CssClass="modalPopup" Style="display: none; width: 900px; height: 400px;">
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="PageTitle panel-title" align="center">
                    <asp:Label ID="Label1" runat="server" Text="Show Part History">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl2">
                <td style="width: 14%">
                    <div align="center" class="ContainTable">
                        <table style="background-color: #efefef;" width="100%">

                            <tr align="center">



                                <td style="width: 15%; float: right;">
                                    <asp:Button ID="btnok" runat="server" Text="OK" CssClass="btn btn-search btn-sm" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                        <div class="rounded_corners">
                            <asp:GridView ID="PartPriceGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                CssClass=""
                                Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                                OnPageIndexChanging="PartPriceGrid_PageIndexChanging"
                                HeaderStyle-BackColor="#00FFFF" RowStyle-BackColor="" AlternatingRowStyle-BackColor="White"
                                RowStyle-ForeColor="#3A3A3A">

                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>

                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PartID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartID" runat="server" Text='<%# Eval("Part_ID") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPart_No" runat="server" Text='<%# Eval("Part_No") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartl_Name" runat="server" Text='<%# Eval("Part_name") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("EffectiveFromDate") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("EffectiveToDate") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LIST" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("LIST","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MRP" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("MRP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NDP/DLP" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNDP" runat="server" Text='<%# Eval("NDP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
