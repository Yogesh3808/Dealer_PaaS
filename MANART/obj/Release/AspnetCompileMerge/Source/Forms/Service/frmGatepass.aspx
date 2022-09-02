<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmGatepass.aspx.cs" Inherits="MANART.Forms.Service.frmGatepass" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery-1.11.1.js"></script>
    <script src="../../Scripts/jsWCFileAttach.js"></script>
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
            FirstTimeGridDisplay('');
        }
        function CloseMe() {
            window.close();
        }

        function ReturnGPIDValue() {
            //debugger;
            var ObjControl = window.document.getElementById("txtID");
            var sValue;
            sValue = ObjControl.value;
            window.returnValue = sValue;
            window.close();

        }

    </script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server" style="background-color:#D4D4D4;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
         <div class="ContainTable" style="background-color: #D4D4D4;">
        <table class="ContainTable table-responsive" border="1" width="100%" >
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title"  align="center" style="width: 14%" colspan="4">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6">

                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 9%">
                    
                </td>
                <td style="width: 9%; font-weight: 700;">
                    <asp:Button ID="btnSave" runat="server" Text="Save GatePass" CssClass="btn btn-search btn-sm"
                        Width="120px" OnClick="btnSave_Click" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnPrint_Click"  Visible="false"/>
                </td>
                <td class="tdLabel" style="width: 14%;" colspan="2">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-search btn-sm"
                        OnClientClick="ReturnGPIDValue();"
                        OnClick="btnBack_Click"></asp:Button>
                </td>
            </tr>
            <tr id="TblControl1">
                <td style="width: 9%; font-weight: 700; padding-left:10px">Gatepass No :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDocNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%; padding-left:10px">GatePass Date :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <uc3:CurrentDate ID="txtDocDate" runat="server" Mandatory="true" bCheckforCurrentDate="true" />
                </td>
            </tr>
            <tr>
                <td style="width: 9%; padding-left:10px">Customer name :</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtCustName" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%; padding-left:10px;"> Invoice No</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtJbInvNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                    
                </td>
            </tr>
            <tr id="trJobcardDetails" runat="server">
                <td style="width: 9%; padding-left:10px">
                    <asp:Label ID="lblJobcardNo" runat="server" Text="Jobcard No"></asp:Label>
                   </td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtJobNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%; padding-left:10px">Jobcard Date:</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtFailDate" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr id="trchassisDetails" runat="server">
                <td style="width: 9%; padding-left:10px">Chassis No:</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtVIN" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%; padding-left:10px">Engine No.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtEngNr" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr id="trVahicleDetails" runat="server">
                <td style="width: 9%; padding-left:10px;">Sales Invoice No.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtSlInvNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%; padding-left:10px;">Veh. Regn. Nr. :</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtVehRegNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%; padding-left:10px;">Narration.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtNarr" CssClass="TextBoxForString" runat="server" MaxLength="30" Width="96%" Font-Bold="False"></asp:TextBox>
                </td>
                <td colspan="2"></td>
            </tr>

            <tr>
                <td colspan="4">
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerBrID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDlrCode" CssClass="HideControl" runat="server" Width="96%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtGPType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                </td>
            </tr>
        </table>
             </div>
    </form>
</body>
</html>
