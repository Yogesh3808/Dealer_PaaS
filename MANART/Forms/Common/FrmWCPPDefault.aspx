﻿<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        body{font: 13px 'Segoe UI', Tahoma, Arial, Helvetica, sans-serif;}        
    </style>

    <%-- WCPP-detection meta tag for IE10 --%>       
    <%= Neodynamic.SDK.Web.WebClientPrint.GetWcppDetectionMetaTag() %>
</head>
<body>
    <form id="form1" runat="server">
    <div id="msgInProgress">
        <div id="mySpinner" style="width:32px;height:32px"></div>
        <br />
        Detecting WCPP utility at client side... 
        <br />
        Please wait a few seconds...
        <br />
    </div>
    <div id="msgInstallWCPP" style="display:none;">    
        <h3>#1 Install WebClientPrint Processor (WCPP)!</h3>
        <p>
            <strong>WCPP is a native app (without any dependencies!)</strong> that handles all print jobs 
            generated by the <strong>WebClientPrint ASP.NET component</strong> at the server side. The WCPP 
            is in charge of the whole printing process and can be 
            installed on <strong>Windows, Linux & Mac!</strong>
        </p>
        <p>
            <a href="http://www.neodynamic.com/downloads/wcpp/" target="_blank">Download and Install WCPP from Neodynamic website</a><br />                
        </p>       
        <h3>#2 After installing WCPP...</h3>
        <p>
            <a href="ReportViewerCrossBrowserClientPrint.aspx">You can go and test WebClientPrint for ASP.NET</a>
        </p>
            
    </div>
    </form>

    <%-- Add Reference to jQuery at Google CDN --%>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>

    <%-- Add Reference to spin.js (an animated spinner) --%>
    <script src="http://fgnass.github.io/spin.js/spin.min.js"></script>

    <script type="text/javascript">

        var wcppPingDelay_ms = 10000;

        function wcppDetectOnSuccess() {
            <%-- WCPP utility is installed at the client side
                 redirect to WebClientPrint sample page --%>
            
            <%-- get WCPP version --%>
            var wcppVer = arguments[0];
            if (wcppVer.substring(0, 1) == "2")
                window.location.href = "ReportViewerCrossBrowserClientPrint.aspx";
            else //force to install WCPP v2.0
                wcppDetectOnFailure();
        }

        function wcppDetectOnFailure() {
            <%-- It seems WCPP is not installed at the client side
                 ask the user to install it --%>
            $('#msgInProgress').hide();
            $('#msgInstallWCPP').show();
        }

        $(document).ready(function () {
            <%-- Create the Spinner with options (http://fgnass.github.io/spin.js/) --%>
            var spinner = new Spinner({
                lines: 12,
                length: 7,
                width: 3,
                radius: 10,
                color: '#336699',
                speed: 1,
                trail: 60
            }).spin($('#mySpinner')[0]);
        });

    </script>

    
    <%-- WCPP detection script --%>       
    <%= Neodynamic.SDK.Web.WebClientPrint.CreateWcppDetectionScript() %>

</body>
</html>
