<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchGridViewForPriceMaster.ascx.cs" Inherits="MANART.WebParts.SearchGridViewForPriceMaster" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<link href="../Content/GridStyle.css" rel="stylesheet" />
<%--<script src="../Scripts/jquery-1.4.2.min.js"></script>
<script src="../Scripts/jquery.datepick.js"></script>--%>
<link href="../Content/jquery.datepick.css" rel="stylesheet" />
<script src="../Scripts/jquery.min.js"></script>
<script src="../Scripts/jquery-jtemplates.js"></script>
<script src="../Scripts/jquery.tablesorter.min.js"></script>


<script type="text/javascript">


    $(document).ready(function () {
        var txtDocDateFrom = document.getElementById("ContentPlaceHolder1_SearchGrid_txtDocDateFrom_txtDocDate");
        var txtDocDateTo = document.getElementById("ContentPlaceHolder1_SearchGrid_txtDocDateTo_txtDocDate");
        $('#ContentPlaceHolder1_SearchGrid_txtDocDateFrom_txtDocDate').datepick({
            onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtDocDateTo.value
        });

        $('#ContentPlaceHolder1_SearchGrid_txtDocDateTo_txtDocDate').datepick({
            onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDateFrom.value
        });

        function customRange(dates) {
            if (this.id == 'ContentPlaceHolder1_SearchGrid_txtDocDateFrom_txtDocDate') {
                $('#ContentPlaceHolder1_SearchGrid_txtDocDateTo_txtDocDate').datepick('option', 'minDate', dates[0] || null);
            }
            else {
                $('#ContentPlaceHolder1_SearchGrid_txtDocDateFrom_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
            }
        }
    });

</script>

<script type="text/javascript">
    //    function ShowPriceWiseDealarHistory(PartID, iDealerId) {
    //        var feature = "dialogWidth:1000px;dialogHeight:400px;status:no;help:no;scrollbars:no;resizable:no;";
    ////        var ObjDealer = document.getElementById("ContentPlaceHolder1_Location_drpDealerName");
    ////        var iDealerId = ObjDealer.value;
    //        window.showModalDialog("frmShowPartPriceDet.aspx?DealerId=" + iDealerId + "&PartID=" + PartID, "List", feature);
    //        return true;
    //    }
    function ShowPriceWiseDealarHistory(SValue) {

        var feature = "dialogWidth:1000px;dialogHeight:400px;status:no;help:no;scrollbars:no;resizable:no;";
        var sSqlFor = '<%=sSqlFor%>';
        var sModelPart = '<%=sModelPart%>';
        var ObjDealer = document.getElementById("ContentPlaceHolder1_SearchGrid_SelectionGrid");
        //debugger;
        //var iPartID = SValue.parentNode.parentNode.childNodes[1].innerText; //ObjDealer.value;
        //var IsDistributor = SValue.parentNode.parentNode.childNodes[2].innerText;
        var iPartID = SValue.parentNode.parentNode.childNodes[2].innerText; //ObjDealer.value;
        var IsDistributor = SValue.parentNode.parentNode.childNodes[3].innerText;
        //window.showModalDialog("frmShowPartPriceDet.aspx?PartID=" + iPartID + "&Distributor=" + IsDistributor + "&SqlFor=" + sSqlFor, "List", feature);
        window.showModalDialog("frmShowPartPriceDet.aspx?PartID=" + iPartID + "&Distributor=" + IsDistributor + "&SqlFor=" + sSqlFor + "&ModelPart=" + sModelPart, "List", feature);
        return false;
    }
</script>

<%--<style type="text/css">
    .style1
    {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 10px;
        color: Black;
        font-weight: bold;
        padding-left: 10px;
        width: 228px;
    }
</style>--%>
    <asp:Panel ID="PSelection" runat="server" BorderColor="DarkGray" BorderStyle="Double"
        class="ContaintTableHeader">
        <cc1:CollapsiblePanelExtender ID="CPESelection" runat="server" TargetControlID="CntSelection"
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
        <asp:Panel ID="CntSelection" runat="server" ScrollBars="Auto">
            <table width="100%" class="table-responsive">
                <tr>
                    <td>
                        <table class="table table-bordered">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="GridTitle" runat="server" Text="Search: "></asp:Label>
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="DdlSelctionCriteria_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <div style="float: left">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    </div>
                                    <div id="divDateSearch" runat="server" style="float: left;">
                                       <%-- <div style="float: left;">
                                            <div style="float: left;">
                                                From:
                                            </div>
                                            <div style="float: left;">
                                                <uc3:CurrentDate ID="txtDocDateFrom" runat="server" Mandatory="false" />
                                            </div>
                                        </div>
                                        <div style="float: left;">
                                            <div style="float: left;">
                                                To:
                                            </div>
                                            <div style="float: left;">
                                                <uc3:CurrentDate ID="txtDocDateTo" runat="server" />
                                            </div>
                                        </div>--%>
                                        <table >
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
                                    </asp:UpdateProgress>--%>
                                        <asp:HiddenField ID="hdnSort" runat="server" Value="N" />
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
            <asp:Panel ID="Panel" runat="server" ScrollBars="Auto" Width="100%">
                <div id="gridView" class="grid">
                    <asp:GridView ID="SelectionGrid" runat="server" CssClass="datatable table table-bordered" CellPadding="0"
                        CellSpacing="0" GridLines="Horizontal" Style="border-collapse: separate;" HeaderStyle-CssClass="GridViewHeaderStyle"
                        AlternatingRowStyle-CssClass="odd" RowStyle-CssClass="even" AllowPaging="True"
                        OnPageIndexChanging="SelectionGrid_PageIndexChanging">
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeaderStyle"></HeaderStyle>
                        <AlternatingRowStyle CssClass="odd"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>

