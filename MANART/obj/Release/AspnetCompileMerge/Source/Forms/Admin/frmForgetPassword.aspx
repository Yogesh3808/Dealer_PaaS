<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmForgetPassword.aspx.cs"
    EnableEventValidation="false" Title="MTI-Forget Password" Inherits="MANART.Forms.Admin.frmForgetPassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Your Password......</title>
      <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />

    <script type="text/javascript">
        function ClosePopupWindow() {
            window.close();
            return false;
        }

    </script>

    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <table id="PageTbl" class="PageTable table-responsive" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="Password recovery for your Account"> </asp:Label>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 14%">
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="txtDocNoDetails" runat="server" class="ContainTable">
                            <tr>
                                <td align="center" class="ContaintTableHeader" style="height: 15px">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblForgotPasswordMessage" runat="server" Text=""
                                        Style="color: #006699; font-weight: 700"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text=" OK "
                                        CssClass="btn btn-search" OnClick="btnSave_Click" />&nbsp;<asp:Button
                                            ID="btnCancel" runat="server" Text=" Cancel " OnClientClick="return ClosePopupWindow()"
                                            CssClass="btn btn-search" />
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
