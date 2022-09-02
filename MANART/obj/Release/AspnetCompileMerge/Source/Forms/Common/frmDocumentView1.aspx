<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="frmDocumentView1.aspx.cs" Inherits="MANART.Forms.Common.frmDocumentView1" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report</title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <style type="text/css" >
        .cssHide {display:none}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
            <div id="divPrint" class="accordion-group" runat="server" >
                   <%-- <div class="accordion-heading" >
                        <h4><a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapse2">
                        Test it!
                        </a></h4>
                    </div>  class="accordion-body collapse"--%>
                    <div id="collapse2" >
                        <div class="accordion-inner">
                           <%-- <p>
                            The following are pre-selected files to test WebClientPrint File Printing feature.
                            </p>--%>
                            <div class="row">
                               <%-- <div class="span4">--%>
                                    <%--<hr />--%>

                                    <label class="checkbox" style="padding-left:75px;">
                                        <a class="btn btn-primary" onclick="javascript:jsWebClientPrint.print('useDefaultPrinter=' + $('#useDefaultPrinter').attr('checked') + '&printerName=' + $('#installedPrinterName').val() + '&filetype=' + $('#ddlFileType').val());">Print</a>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" checked="checked" id="useDefaultPrinter" />Print to Default printer 
                                    </label>                                   
                                    <div id="loadPrinters">
                                    <%--Click to load and select one of the installed printers!
                                    <br />--%>
                                    <label style="padding-left:75px;">
                                    <a onclick="javascript:jsWebClientPrint.getPrinters();" class="btn btn-success">Show available Printers</a>
                                    </label>
                              <%--      <br /><br />--%>
                                        
                                    </div>
                                    <div id="installedPrinters" style="visibility:hidden">
                                    <label for="installedPrinterName" style="padding-left:75px;">Select Printer:</label>
                                    <select name="installedPrinterName" id="installedPrinterName"></select>
                                    </div>            
                                    <script type="text/javascript">
                                        var wcppGetPrintersDelay_ms = 5000; //5 sec

                                        function wcpGetPrintersOnSuccess(){
                                            <%-- Display client installed printers --%>
                                            if(arguments[0].length > 0){
                                                var p=arguments[0].split("|");
                                                var options = '';
                                                for (var i = 0; i < p.length; i++) {
                                                    options += '<option>' + p[i] + '</option>';
                                                }
                                                $('#installedPrinters').css('visibility','visible');
                                                $('#installedPrinterName').html(options);
                                                $('#installedPrinterName').focus();
                                                $('#loadPrinters').hide();                                                        
                                            }else{
                                                alert("No printers are installed in your system.");
                                            }
                                        }

                                        function wcpGetPrintersOnFailure() {
                                            <%-- Do something if printers cannot be got from the client --%>
                                            alert("No printers are installed in your system.");
                                        }
                                    </script>


                                <%--</div>--%>
                                <div class="span4">
                                    <%--<hr />--%>
                                    <div id="fileToPrint" style="padding-left:75px;">
                                        <p>
                                            For Print Utility, Please Install
                                        <a href="../../DownloadFiles/Neodynamic/wcpp-2.0.16.1000-win.exe">WebClientPrint Processor (WCPP)</a>
                                            <br />
                                            
                                       If Acrobat Reader is not Installed on Your machine  then Install <a href="../../DownloadFiles/Neodynamic/FoxitReader82_enu_Setup_Prom.exe">Foxit Reader</a> on Local machine
                                            </p>
                                        <%--<label for="ddlFileType">Select a sample File to print:</label>--%>
                                        <select id="ddlFileType" style="display:none;">
                                            <option>PDF</option>
                                            <option>TXT</option>
                                            <option>DOC</option>
                                            <option>XLS</option>
                                            <option>JPG</option>
                                            <option>PNG</option>
                                            <option>TIF</option>
                                        </select>
                                        
                                        <%--<br />--%>
                                        
                        
                                    </div>
                                </div>
                            </div>
                            <%--<h5>File Preview</h5>--%>
                            <%--<iframe id="ifPreview" style="width:100%; height:500px;" frameborder="0"></iframe>--%>
                        </div>
                        
                    </div>
                </div>
            <div > 
                <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="600px"  ZoomMode="PageWidth"
                    ShowBackButton="True" ProcessingMode="Remote"  InternalBorderColor="Black" AsyncRendering="False" ></rsweb:ReportViewer>--%>

                 <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" 
                        ShowBackButton="True"  ProcessingMode="Remote" SizeToReportContent=true>
                    </rsweb:ReportViewer>

                <asp:TextBox ID="txtPDFFile" runat="server" CssClass="cssHide"></asp:TextBox> 
                <%--style="margin-bottom:40px"--%>
            </div>
        </asp:Panel>
    </form>
    
    <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(MANART.Forms.Common.MyUtils.GetWebsiteRoot() + "DemoPrintFile.ashx?PDFName=" + txtPDFFile.Text ) %> <!-- PDFFile=" + StrPDFFile-->

<script type="text/javascript">

    $("#ddlFileType").change(function () {
        var s = $("#ddlFileType option:selected").text();
        if (s == 'DOC') $("#ifPreview").attr("src", "http://docs.google.com/gview?url=http://webclientprint.azurewebsites.net/files/LoremIpsum.doc&embedded=true");
        if (s == 'PDF') $("#ifPreview").attr("src", "http://docs.google.com/gview?url=http://webclientprint.azurewebsites.net/files/LoremIpsum.pdf&embedded=true");
        if (s == 'TXT') $("#ifPreview").attr("src", "http://docs.google.com/gview?url=http://webclientprint.azurewebsites.net/files/LoremIpsum.txt&embedded=true");
        if (s == 'TIF') $("#ifPreview").attr("src", "http://docs.google.com/gview?url=http://webclientprint.azurewebsites.net/files/patent2pages.tif&embedded=true");
        if (s == 'XLS') $("#ifPreview").attr("src", "http://docs.google.com/gview?url=http://webclientprint.azurewebsites.net/files/SampleSheet.xls&embedded=true");
        if (s == 'JPG') $("#ifPreview").attr("src", "http://webclientprint.azurewebsites.net/files/penguins300dpi.jpg");
        if (s == 'PNG') $("#ifPreview").attr("src", "http://webclientprint.azurewebsites.net/files/SamplePngImage.png");
    }).change();


</script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js" type="text/javascript"></script>
    <script src="http://netdna.bootstrapcdn.com/twitter-bootstrap/2.1.1/js/bootstrap.min.js" type="text/javascript"></script>
</body>
</html>
