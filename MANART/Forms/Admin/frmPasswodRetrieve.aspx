<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmPasswodRetrieve.aspx.cs" Inherits="MANART.Forms.Admin.frmPasswodRetrieve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Validation() {
            var txtLoginName = document.getElementById("ContentPlaceHolder1_txtLoginName")
            if (txtLoginName.value == "") {
                alert("Please Enter Login Name");
                txtLoginName.focus();
                return false;
            }
        }

        function ErrorMessage() {
            var txtLoginName = document.getElementById("ContentPlaceHolder1_txtLoginName")
            alert("Opps ! Login Name not found in our database");
            txtLoginName.focus();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text="Password Retrieve">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 15%">
                    <asp:Panel ID="PanelLoginDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="tblProformaDetails" runat="server" class="ContainTable table-condensed table-hover" width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader well well-sm" style="height: 15px" colspan="2">Password Information
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 15px" colspan="2">
                                    <asp:TextBox ID="lblUserRegitrationFor" runat="server"
                                        Style="font-weight: 700; text-align: center; color: #339966; background-color: #F0F0F0; border: 0px;"
                                        Width="761px" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                </td>
                            </tr>

                            <tr style="width: 20%" class="tdLabel">
                                <td class="tdLabel" style="text-align:right">Login Name:
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="" TabIndex="1"></asp:TextBox>
                                    <b class="Mandatory">*</b>

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="right" class="tdLabel">Password :
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="" TabIndex="2" MaxLength="20" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="padding-left: 10px">
                                    <asp:Button ID="btnGetPwd" runat="server" OnClientClick="return Validation();"
                                        Text="Retrieve Password" TabIndex="3" CssClass="CommandButton btn btn-primary btn-sm"
                                        OnClick="btnGetPwd_Click" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server"
                                        Text="New" TabIndex="4" CssClass="CommandButton btn btn-primary btn-sm"
                                        OnClick="btnCancel_Click" />

                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <asp:TextBox ID="txtAvailCheck" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
