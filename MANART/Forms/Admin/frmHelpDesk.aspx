<%@ Page Title="MTI-Help Desk " Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmHelpDesk.aspx.cs" Inherits="MANART.Forms.Admin.frmHelpDesk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/jquery-latest.pack.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //  fetch the data for the jTemplate grid
            //  fix up the gridview so its header row is in a thead 
            //  and the rows are in a tbody ...
            $('#gridView .datatable').prepend(
                //  remove the header and wrap it in a THEAD
                $('<thead></thead>').append($('#gridView .datatable TR.header').remove())
            );
            //  wrap the header text in a div so we can hang the sort icon off of it ...
            $('.datatable TH').wrapInner('<div class="sort"></div>');
            //  apply the tablesorter
            $('#gridView .datatable').tablesorter({ cssAsc: 'asc', cssDesc: 'desc', widgets: ['zebra'] });
        });
    </script>

    <script type="text/javascript">
        function ShowPermission(para) {
            var paravalue = $('#' + para);
            var PartDetailsValue;
            var Id = paravalue[0].parentNode.parentNode.cells[1].children[0].innerHTML; //para.parentNode.parentNode.childNodes[1].all[0].innerText;
            var LoginName = paravalue[0].parentNode.parentNode.cells[1].children[1].innerHTML; //para.parentNode.parentNode.childNodes[1].all[1].innerText;
            var UserType = paravalue[0].parentNode.parentNode.cells[1].children[2].innerHTML; //para.parentNode.parentNode.childNodes[1].all[2].innerText;
            var UserRole = paravalue[0].parentNode.parentNode.cells[1].children[3].innerHTML; //para.parentNode.parentNode.childNodes[1].all[3].innerText; 1000px
            window.showModalDialog("frmPermission.aspx?LoginName=" + LoginName + "&ID=" + Id + "&UserType=" + UserType + "&UserRole=" + UserRole, "List", "dialogHeight: 700px; dialogWidth: 1277px; dialogTop: 150px; dialogLeft: 50px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            //window.location.reload();
            //window.location = window.location;
            window.href = window.href;
            return false;
        }
        function ValidateSearchTextbox() {
            var txtSearch = document.getElementById("<%=txtSearch.ClientID %>");
            if (txtSearch.value.trim() == "") {
                alert("Please enter value in search text");
                txtSearch.focus();
                return false;
            }
        }
    </script>
    <style>
        .table {
            width: 100%;
            border: double;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
            <table border="1" style="width: 100%">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="GridTitle" runat="server" Text="Search" CssClass="GridTitle"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString "></asp:TextBox>
                                </td>
                                <td>&nbsp;<asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="dropdown dropdown-toggle">
                                    <asp:ListItem>UserName</asp:ListItem>
                                    <asp:ListItem>LoginName</asp:ListItem>
                                    <asp:ListItem>Role</asp:ListItem>
                                    <asp:ListItem>Type</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                                <td>&nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="btn btn-search" Text="Search" Width="" OnClick="btnSearch_Click" OnClientClick="return ValidateSearchTextbox()" />
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <img src="../../Images/Search_Progress.gif" alt="Searchig" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblConfirm" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search" CssClass="btn btn-search" Width="100%"
                                        Visible="false" OnClick="btnClearSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <div id="gridView">
                            <asp:GridView ID="SelectionGrid" runat="server" CssClass="datatable table table-bordered " CellPadding="0" Width="100%"
                                AllowPaging="true" AllowSorting="true" CellSpacing="0" GridLines="None" Style="border-collapse: separate;"
                                AlternatingRowStyle-CssClass="odd" RowStyle-CssClass="even" AutoGenerateColumns="false"
                                OnRowDataBound="SelectionGrid_RowDataBound" OnPageIndexChanging="SelectionGrid_PageIndexChanging"
                                OnSorting="SelectionGrid_OnSorting" PageSize="20">
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr&#160;No." SortExpression="LoginName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LoginName" SortExpression="LoginName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" CssClass="DispalyNon" runat="server" Text='<%# Eval("ID") %>' />
                                            <asp:Label ID="lblloginName" runat="server" Text='<%#Eval("LoginName")%>'></asp:Label>
                                            <asp:Label ID="lblUserType" CssClass="DispalyNon" runat="server" Text='<%# Eval("UserType") %>' />
                                            <asp:Label ID="lblUserRole" CssClass="DispalyNon" runat="server" Text='<%# Eval("UserRole") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName"></asp:BoundField>
                                    <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role"></asp:BoundField>
                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type"></asp:BoundField>
                                    <asp:BoundField DataField="RegDate" HeaderText="CreationDate" SortExpression="RegDate"></asp:BoundField>
                                    <asp:TemplateField SortExpression="id" HeaderText="User&#160;Status" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("IsActive")%>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User&#160;Status">
                                        <ItemTemplate>
                                            <a id="lnkUpdate" href='<%#DataBinder.Eval(Container,"DataItem.ID")%>' onserverclick="UpdateStatus"
                                                runat="server">
                                                <asp:Label ID="lblActs" runat="server" Text=""></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="id" HeaderText="Lock&#160;Status" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLockStatus" runat="server" Text='<%#Eval("Lock")%>' Visible="False"></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lock&#160;Status">
                                        <ItemTemplate>
                                            <a id="lnkLockUpdate" href='<%#DataBinder.Eval(Container,"DataItem.ID")%>' onserverclick="UpdateLockStatus"
                                                runat="server">
                                                <asp:Label ID="lblActs2" runat="server" Text=""></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField SortExpression="id" HeaderText="BLock&#160;Status" Visible="False">--%>
                                    <asp:TemplateField HeaderText="BLock&#160;Status" ItemStyle-CssClass="HideControl"
                                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBLockStatus" runat="server" Text='<%#Eval("Block")%>' Visible="False"></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BLock&#160;Status" ItemStyle-CssClass="HideControl"
                                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <a id="lnkBLockUpdate" href='<%#DataBinder.Eval(Container,"DataItem.ID")%>' onserverclick="UpdateBLockedStatus"
                                                runat="server" style="display: none"></a>
                                            <asp:Label ID="lblActs3" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add&#160;User&#160;Permission">
                                        <ItemTemplate>
                                            <a href="#" onclick="return ShowPermission(this.id);" id="ahlPermissions" runat="server">
                                                <asp:Label ID="lblPermissions" runat="server" Text="Add Permissions" Visible="true"></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
