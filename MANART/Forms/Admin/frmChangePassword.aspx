<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmChangePassword.aspx.cs" Inherits="MANART.Forms.Admin.frmChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Change your password.......</title>
    <script src="../../Scripts/jsAdminFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>

    <base target="_self" />
    <script type="text/javascript">
        function CloseWindow() {
            //debugger;
            //window.history.go(-1);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" table-responsive ">
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text="Change your password ">
                    </asp:Label>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 15%">
                    <table id="tblLogin" runat="server" class="ContainTable">
                    </table>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 15%">
                    <asp:Panel ID="PanelLoginDetails" runat="server" BorderColor="Black" BorderStyle="Double">

                        <table id="tblPassword" runat="server" class="ContainTable">
                            <%-- <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="2">
                                Password!!</td>
                        </tr>--%>
                            <tr class="well well-sm">
                                <td align="center" style="height: 15px" colspan="2">
                                    <asp:Label ID="Label1" runat="server"
                                        Text="(A strong password helps prevent unauthorized access to your account)"></asp:Label>

                                    <br />
                                    <br />

                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" align="right">User Name:
                                </td>
                                <td class="tdLabel">
                                    <asp:Label ID="lblUserName" runat="server" CssClass="" Text="" Width="100%"></asp:Label>

                                </td>

                            </tr>
                            <tr>
                                <td class="tdLabel" align="right">Type Old password:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass=""
                                        TabIndex="2" Text="" TextMode="Password"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" align="right">Type new password:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass=""
                                        TabIndex="2" Text="" TextMode="Password"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>

                            </tr>
                            <tr>
                                <td class="tdLabel" align="right">Retype New Password:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtRPassword" runat="server" CssClass="" Text=""
                                        TextMode="Password" TabIndex="3"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>

                            </tr>

                            <tr>
                                <td align="center" style="height: 15px" colspan="2">
                                    <br />
                                    <asp:Button ID="btnChangePassword" runat="server" CssClass="CommandButton btn btn-primary btn-xs"
                                        OnClientClick="return ChangePasswordvalidate();" TabIndex="8"
                                        Text="Save" OnClick="btnChangePassword_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="CommandButton btn btn-primary btn-xs"
                                        TabIndex="9"
                                        Text="Cancel" OnClick="btnCancel_Click" />
                                    &nbsp;
                                <br />
                                    <br />
                                </td>
                            </tr>
                        </table>


                        <table id="tblConfirmMessage" runat="server" class="ContainTable">
                            <tr>
                                <td>
                                    <asp:Label ID="LblConfirmMessage" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <asp:TextBox ID="txtUserID" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnPassword" runat="server" Value="" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
