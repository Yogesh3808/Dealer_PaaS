<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDocumentView.aspx.cs" Inherits="MANART.Forms.Common.frmDocumentView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head  id="Head1" runat="server">
    <title>Report</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double" >                
                <div>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
                   <%-- <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" 
                        ShowBackButton="True"  ProcessingMode="Remote" SizeToReportContent=true>
                    </rsweb:ReportViewer>
         --%>
                </div>
             </asp:Panel>
    </form>
</body>
</html>
