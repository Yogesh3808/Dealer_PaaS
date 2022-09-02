<%@ Page Title="" Language="C#" Theme="SkinFile" AutoEventWireup="true" CodeBehind="frmJobcardSelection.aspx.cs" Inherits="MANART.Forms.Common.frmJobcardSelection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc2" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chassis Details</title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jquery-1.3.2.js"></script>
    <script src="../../Scripts/jquery.min.js"></script>

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
        window.onload = function () {
        }

        //Function For return Chassis Details
        function ReturnChassisValue(objImgControl) {
            debugger;
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            if (objRow[6].childNodes[1].innerText== "N")
            {
                alert("Customer of jobcard is Inactive. Please make active from Customer Master.");
                return false;
            }
            if (objRow[7].childNodes[1].innerText == "Y") {
                alert("Jobcard contain part which does not have tax details attached to HSN code. So jobcard cannot be selected. Pl. discuss with MTI team.");
                return false;
            }

            var sValue;
            var ArrOfChassis = new Array();
            for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
                sValue = objRow[cnt].innerText.trim();
                ArrOfChassis.push(sValue);
            }
            //alert(ArrOfChassis);
            window.returnValue = ArrOfChassis;
            window.close();

        }
    </script>

    <base target="_self" />
</head>
<%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
<body runat="server">
    <form id="form1" runat="server">
        <div class="table-responsive">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table class="" border="1">
                        <tr id="TitleOfPage" class="panel-heading">
                            <td class="panel-title" align="center">
                                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                            </td>
                        </tr>
                        <tr id="TblControl">
                            <td style="width: 14%">
                                <div align="center" class="">
                                    <table style="background-color: #efefef;" width="50%">
                                        <tr align="center">
                                            <td class="tdLabel">Search:
                                            </td>
                                            <td class="tdLabel">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                            </td>

                                            <td class="tdLabel">
                                                <asp:DropDownList ID="DdlSelctionCriteria" runat="server"
                                                    CssClass="ComboBoxFixedSize">
                                                    <asp:ListItem Selected="True">Job No</asp:ListItem>
                                                    <asp:ListItem >Chassis No</asp:ListItem>
                                                    <%--<asp:ListItem Value="Customer_name">Customer Name</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSearch" runat="server" Text="Search"
                                                    CssClass="btn btn-search btn-sm" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    class="ContaintTableHeader" ScrollBars="Vertical">
                                    <asp:GridView ID="ChassisGrid" runat="server" AlternatingRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                        HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                                        AutoGenerateColumns="false"
                                        OnPageIndexChanging="ChassisGrid_PageIndexChanging">
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" onClick="return ReturnChassisValue(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("JobCard_HDR_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No." ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobcardNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("job_no") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job Date" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobcardDate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("JobDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChassisNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_no") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active" ItemStyle-Width="10%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActive" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CustActive") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MsgShow" ItemStyle-Width="10%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShMsg" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ShMsg") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle Wrap="True" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>

                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
<%--</asp:Content>--%>




