<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUnderMaintainance.aspx.cs" Inherits="MANART.frmUnderMaintainance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID ="ScriptManager" runat ="server"  ></asp:ScriptManager>
    <div>
    <table  width ="100%" >
    <tr>
    
    <td style="padding-left :20%;padding-top:10%">
    <img src="Images/maintenance.gif" />
    </td></tr>
    <tr>
    
    <td style ="font-size:medium;font-weight:bold;font-family:Verdana;text-align :center ">
    <asp:UpdatePanel ID ="UpdatePanel1" runat ="server" UpdateMode ="Always"  >
    <ContentTemplate>
    Site will resume in <asp:Label  ID ="lblTime" runat ="server" ></asp:Label>
     <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick">
    </asp:Timer>
    </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    </tr>
    </table>
    </div>
   
    </form>
</body>
</html>
