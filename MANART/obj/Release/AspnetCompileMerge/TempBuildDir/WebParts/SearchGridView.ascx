<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchGridView.ascx.cs" Inherits="MANART.WebParts.SearchGridView" %>

<%--<%@ Register Src="~/WebParts/CurrentDateTime.ascx" TagName="CurrentDate" TagPrefix="uc3" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagPrefix="uc3" TagName="CurrentDate" %>

<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<link href="../Content/GridStyle.css" rel="stylesheet" />
<script src="../Scripts/jquery-jtemplates.js"></script>
<script src="../Scripts/jquery.tablesorter.min.js"></script>

<%--For Jquery DatePicker--%>
<link href="../../Content/cssDatePicker.css" rel="stylesheet" />
<%--<script src="../../Scripts/jquery-1.4.2.min.js"></script>
<script src="../../Scripts/jquery.datepick.js"></script>--%>

<script type="text/javascript">
    $(document).ready(function () {
        var txtDocDateFrom = document.getElementById("ContentPlaceHolder1_<%=_SGridName %>_txtDocDateFrom_txtDocDate");
        var txtDocDateTo = document.getElementById("ContentPlaceHolder1_<%=_SGridName %>_txtDocDateTo_txtDocDate");
        if (txtDocDateFrom != null && txtDocDateTo != null) {
            $('#ContentPlaceHolder1_<%=_SGridName%>_txtDocDateFrom_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtDocDateTo.value
            });

            $('#ContentPlaceHolder1_<%=_SGridName%>_txtDocDateTo_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtDocDateFrom.value
            });

            function customRange(dates) {
                if (this.id == 'ContentPlaceHolder1_<%=_SGridName%>_txtDocDateFrom_txtDocDate') {
                    $('#ContentPlaceHolder1_<%=_SGridName%>_txtDocDateTo_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                }
                else {
                    $('#ContentPlaceHolder1_<%=_SGridName%>_txtDocDateFrom_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                }
            }
        }
    });

</script>
<div class="table-responsive">
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
                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
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
            <table width="100%" class=" ">
                <tr style="padding-top: 10px;">
                    <td>
                        <table class="ContainTable">
                            <tr >
                                <td style="text-align: right;">
                                    <asp:Label ID="GridTitle" runat="server" Text="Search: "></asp:Label>
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="" 
                                        OnSelectedIndexChanged="DdlSelctionCriteria_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <div style="float: left">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    </div>
                                    <div id="divDateSearch" runat="server" style="float: left;">                                    
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
                                    <div id="div1" runat="server" style="text-align: left; padding-left: 5px;">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" Width="60px"  CssClass="btn btn-search"
                                            OnClick="btnSearch_Click" />
                                        <asp:TextBox ID="txtUseDate" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search" CssClass=" btn btn-search" Width="70px"
                                            Visible="false" OnClick="btnClearSearch_Click" />
                                        <asp:Label ID="lblConfirm" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblSort" Text="Sort" runat="server"></asp:Label>
                                        <asp:DropDownList ID="drpSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSort_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="Asc">Ascending</asp:ListItem>
                                            <asp:ListItem Value="Desc">Descending</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSorted" Width="" Text="By Document No" runat="server"></asp:Label>
                                        &nbsp;<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Search_Progress.gif" AlternateText="Searhing...." />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress><asp:HiddenField ID="hdnSort" runat="server" Value="N" />
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
                    <asp:GridView ID="SelectionGrid" runat="server" CssClass="table table-striped table-bordered" CellPadding="0"
                        CellSpacing="0" GridLines="Horizontal" Style="border-collapse: separate;" 
                        AlternatingRowStyle-CssClass="odd" RowStyle-CssClass="even" AllowPaging="True" PageSize="10"
                        OnPageIndexChanging="SelectionGrid_PageIndexChanging" HeaderStyle-ForeColor="White" >
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
      <%--                  <HeaderStyle CssClass="GridViewHeaderStyle"></HeaderStyle>--%>
                        <HeaderStyle BackColor="#70757A" 
                                                    ForeColor="White" />
                        <AlternatingRowStyle CssClass="odd"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</div>

<%--<asp:Panel ID="PSelection" runat="server" BorderColor="DarkGray" BorderStyle="Double"
    class="">
    <cc1:CollapsiblePanelExtender ID="CPESelection" runat="server" TargetControlID="CntSelection"
        ExpandControlID="TtlSelection" CollapseControlID="TtlSelection" Collapsed="true"
        ImageControlID="ImgTtlSelection" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
        SuppressPostBack="true" TextLabelID="lblTtlSelection">
    </cc1:CollapsiblePanelExtender>
    <asp:Panel ID="TtlSelection" runat="server">
        <table>
            <tr class="panel-heading">
                <td align="center" class="panel-title">
                    <asp:Label ID="lblTtlSelection" runat="server" onmouseover="SetCancelStyleonMouseOver(this);"
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
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-3 col-sm-3">
                    <div class="form-group">
                        <asp:Label ID="GridTitle" runat="server" Text="Search: "></asp:Label>
                        <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass=""
                            OnSelectedIndexChanged="DdlSelctionCriteria_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2">
                    <div class="form-group">
                        <div>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                            <div id="divDateSearch" runat="server">
                                <div>
                                    From:
                                </div>
                                <div>
                                    <uc3:CurrentDate ID="txtDocDateFrom" runat="server" Mandatory="false" />
                                </div>
                                <div>
                                    To:
                                </div>
                                <div>
                                    <uc3:CurrentDate ID="txtDocDateTo" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-7 col-sm-7">
                    <div class="form-group">
                        <div>
                            <div id="div1" runat="server">
                                <asp:Button ID="btnSearch" runat="server" Text="Search"
                                    OnClick="btnSearch_Click" />
                                <asp:TextBox ID="txtUseDate" runat="server"></asp:TextBox>
                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search"
                                    Visible="false" OnClick="btnClearSearch_Click" />
                                <asp:Label ID="lblConfirm" runat="server"></asp:Label>
                                <asp:Label ID="lblSort" Text="Sort" runat="server" CssClass="control-label"></asp:Label>
                                <asp:DropDownList ID="drpSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSort_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="Asc">Ascending</asp:ListItem>
                                    <asp:ListItem Value="Desc">Descending</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblSorted" Text="By Document No" runat="server"></asp:Label>
                                &nbsp;<asp:HiddenField ID="hdnSort" runat="server" Value="N" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="table-responsive">
                    <div class="">
                        <asp:GridView ID="SelectionGrid" runat="server" CssClass="table table-striped table-bordered"
                            AllowPaging="True" PageSize="10"
                            OnPageIndexChanging="SelectionGrid_PageIndexChanging" OnSelectedIndexChanged="SelectionGrid_SelectedIndexChanged">
                            <FooterStyle CssClass="" />
                            <RowStyle CssClass="" />
                            <SelectedRowStyle CssClass="" />
                            <AlternatingRowStyle CssClass="" />
                            <HeaderStyle />
                            <PagerStyle />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Panel>--%>


