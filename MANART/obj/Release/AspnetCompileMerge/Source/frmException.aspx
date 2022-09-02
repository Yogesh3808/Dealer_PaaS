<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmException.aspx.cs" Inherits="MANART.frmException" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>dCAN........ </title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/style.css" rel="stylesheet" />
    <%--<link href="Content/loginForm.css" rel="stylesheet" />--%>
    <script src="Scripts/jquery-latest.pack.js"></script>
    <script src="Scripts/jcarousellite_1.0.1c4.js"></script>
    <script src="Scripts/jquery.corner.js"></script>
    <script src="Scripts/jquery.li-scroller.1.0.js"></script>
    <script src="Scripts/jquery-1.11.1.js"></script>

    <style type="text/css">
        .newStyle1 {
            background-image: url(../Images/LeftMenu_menuOnBGTop.gif) no-repeat;
            height: 36px;
        }

        .newStyle2 {
            width: 400px;
        }
    </style>
    <style>
        .Padding {
            padding-left: 45px;

        }
    </style>

    <%--<script type="text/javascript" language="javascript">   
 if (document.getElementById) {
        // IE 5 and up, FF  
        var upLevel = true;
    } else if (document.layers) {
        // Netscape 4   
        var ns4 = true;
    } else if (document.all) {
        //IE 4   
        var ie4 = true;
    }
    function showObject(obj) {
        if (ns4) { obj.visibility = "show"; }
        else if (ie4 || upLevel) {
            drawMessageBox();
            //obj.style.visibility = "visible";
        }
    }
    showObject('splashScreen');
    function hideObject(obj) {
        if (ns4) {
            obj.visibility = "hide";
        }
        if (ie4 || upLevel) 
     { obj.style.visibility = "hidden"; } }
    function drawMessageBox() {
        var box = '<div id="splashScreen" style="position: absolute; z-index: 5; top: 30%; left: 35%; background-color: #FFFFFF; font-family: Verdana">'
           + '<table cellpadding="0" cellspacing="0" style="width:300px">'
              + '<tr><td style="width:100%; height:100%;font-family:Tahoma;" align="center" valign="middle">'
                     + '<br/><br />' + '<asp:Image ID="LoadImg" ImageUrl="~/images/Wait.gif" runat="server" /><br />'
                                         + '<span style="font-family: Verdana; font-size: 15px; font-weight: bold;">Please wait...</span>'
                                         + '</td><td></td></tr>'
                                     + '</table>'
                                       + '</div>';
        document.write(box);
    }</script>--%>

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
    </script>
    <script type="text/javascript">
        function preventBack() { window.history.forward(-1); }
        setTimeout("preventBack()", 10);
        window.onunload = function () { null };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <header class="Topheader">
            <div class="container">
                <div class="logo">
                    <a href="#">
                        <%--<img src="Images/logo.png" alt="Logo" title="Logo" /> height="55px;" width="135px;" --%>
                        <%--<img src="Images/dcan_logo.jpg" alt="Logo" title="dCAN"  />--%>
                        <img src="./Images/dcan_logo1.png" />
                    </a>
                </div>
                <span class="siteTitle">MAN</span>
                <span class="siteTitle">Trucks <span class="stspan"></span>India</span>
                <div class="siteName">
                  dCAN
                </div >
                
                <div class="site-title hidden-xs " style=" margin-top: -25px;">
                   <%--MAN Toll free 24x7 Customer support -180030702424--%> 
                    <img src="Images/logo.png" alt="Logo" title="Logo" />
                    <%--dCAN Helpdesk Email: dCAN.Helpdesk@in.man.eu--%>
                </div>
                <span class="siteTitle_right">
                    dCAN Helpdesk Email: dCAN.Helpdesk@in.man.eu
                    <br />
                     dCAN Phone numbers : +91 20 6645 2924 / 2948 (08:30 to 18:00 Hours on working days)
                   <%--dCAN Toll free 24x7 Customer support -1800 266 6599--%> 
                    <%--dCAN Toll free number : 1800 266 6599 (08:30 to 18:00 Hours on working days)--%>
                </span>
                <%--<div class="site-title"  >
                     dCAN Helpdesk Email: dCAN.Helpdesk@in.man.eu
                </div>--%>
            </div>
        </header>
         <!-- Start Section-->
        <section>
            <div class="container" style="height: 52px">
                <div class="navbar navbar-default">
                    <div class="container-fluid">
                        <div class="navbar-header">
                           <%-- <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1"
                                aria-expanded="false">
                                <span class="sr-only">Toggle navigation</span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                            </button>--%>
                          
                        </div>

                        <!-- Collect the nav links, forms, and other content for toggling -->
                        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                            <div class="nav navbar-nav navbar-left">
                                <%--<asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" RenderingMode="List" CssClass="Padding"
                                    IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav navbar-nav" DynamicMenuStyle-CssClass="dropdown-menu">
                                </asp:Menu>--%>
                            </div>
                            <div class="nav navbar-nav navbar-right">
                                <div class="nav navbar-nav navbar-left">
                                    <%--<span class="glyphicon glyphicon-user"></span>--%> <%= @Session["lblUserName"] %>
                                </div>
                                <div class="nav navbar-nav navbar-right">
                                    <asp:ImageButton ID="ImageButton1" OnClick="lnkLogOut_Click" runat="server" ImageUrl="images/LogOutIcon.png" CssClass="img-responsive"
                                        ToolTip="LogOut" />
                                </div>
                            </div>
                            <%--<li><a href="#"><span class="glyphicon glyphicon-cog"></span>SignUp</a></li>--%>
                            <asp:Label ID="lblUserName" runat="server" Visible="false" Text="" Font-Size="Small"></asp:Label>
                            <%--</ul>--%>
                            <%--<div class="nav navbar-nav navbar-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-link dropdown-toggle" data-toggle="dropdown">
                                        <span class="glyphicon glyphicon-user"></span><%= @Session["lblUserName"] %>  <span class="caret"></span>
                                        <span class="glyphicon glyphicon-user"></span>
                                        <asp:Label ID="lblUserName" runat="server" Text="" Font-Size="Small"></asp:Label><span class="caret"></span>
                                    </button>
                                    <!-- dropdown menu items-->
                                    <ul class="dropdown-menu" role="menu">
                                        <li><a href="#"><span class="glyphicon glyphicon-pencil"></span>Edit profile</a></li>
                                        <li><span class="glyphicon glyphicon-cog"></span>
                                            <asp:ImageButton ID="ImageButton1" OnClick="lnkLogOut_Click" runat="server" ImageUrl="images/LogOutIcon.png"
                                                ToolTip="LogOut" />Log out</li>
                                    </ul>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <!--End COntainer-fluid-->
                </div>
            </div>
            <!-- Start Bulletin-->
            <%--<div class="container">
                <div class="Bulletin">
                    <span class="bg-black">Bulletin</span>
                    <span>
                        <marquee onmouseover="this.setAttribute('scrollamount', 0, 0);" onmouseout="this.setAttribute('scrollamount', 3, 0);" scrollamount="3">
                            <asp:PlaceHolder ID="plhTag" runat ="server" ></asp:PlaceHolder>                                                       
                            <a href="~/Forms/Admin/frmChangePassword.aspx" id="achPassword" runat ="server" ><span style="color:Red;font-weight:bold ">Your Password will be expired within <asp:Label ID="lblPasswordExp" runat ="server" ></asp:Label> days ...</span></a>	
  <span id="sp1" runat="server" style="color:Red;font-weight:bold "><asp:Label ID="lblSiteMt" runat ="server"  Text="" ></asp:Label></span></marquee>

                    </span>
                </div>
            </div>--%>
            <!-- End Bulletin-->
            

        </section>
        <!-- End Section-->
        <table class="container" style="height:300px;">
            <tr>
                <td style ="font-size:medium;font-weight:bold;font-family:Verdana;text-align :center ">
                      <asp:Label ID="lblTitlenew" runat="server" Text="Error in operation on This Form, Please Contact to Administrator !">
                                    </asp:Label>
                </td>
            </tr>
        </table>
        
        
        <!--Start Footer  -->
        <footer>
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <p>A member of the MAN Group  </p>
                    </div>
                    <div class="col-md-6  ">
                        <p class="text-right ">&copy; Copyright 2013 MAN Trucks India. All Rights Reserved </p>
                    </div>
                </div>
            </div>
        </footer>
        <!--End Footer  -->
            
           <%-- <div class="background">
                <table class="tblbackground table-responsive">
                    <tr>
                        <td style="width: 70%">
                            <div class="DivMenu">
                            </div>
                        </td>
                        <td style="width: 25%; text-align: right; padding-top: 5px;">
                            <div style="margin-left: 350px;">
                                <asp:ImageButton ID="ImageButton2" OnClick="lnkLogOut_Click" runat="server" ImageUrl="~/Images/LogOutIcon.png"
                                    ToolTip="LogOut" />
                            </div>
                        </td>
                        <td style="width: 7%; text-align: left; padding-top: 10px;">
                            <asp:Label ID="Label1" Text="Log Off" runat="server" Font-Size="Small" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>


        </div>
        <table style="width: 100%; background-color: #efefef" border="1" class="table-responsive">
            <tr>
                <td style="width: 80%">
                    <table style="width: 100%; height: 1%">
                        <tr>
                            <td style="width: 8%" class="heading">Bulletin
                            </td>
                            <td>&nbsp;</td>
                            <td style="width: 35%; vertical-align: middle; padding-left: 10px; background-color: #006699;">
                                <asp:Label ID="lblUserName" runat="server" Text="" Style="font-weight: 700; color: #FFFFFF"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div>

                        <table id="PageTbl" class="PageTable" border="1">
                            <tr id="TitleOfPage">
                                <td class="PageTitle" align="center" style="width: 15%">
                                    <asp:Label ID="lblTitlenew" runat="server" Text="Error in operation on This Form, Please Contact to Administrator !">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>--%>
        <%--<table id="footer" class="table-responsive">
            <tr>
                <td>
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/footer_left.gif" AlternateText="MTI Logo"
                        Width="5" Height="49" />
                </td>
                <td>
                    <div style="padding-left: 5px">
                        Home &nbsp;&nbsp;| &nbsp;&nbsp;<a href="#" class="normalLink">Product</a> &nbsp;&nbsp;|&nbsp;&nbsp;
                    <a href="#" class="normalLink">Contact Us</a>
                    </div>
                </td>
                <td>
                    <div style="text-align: center">
                     
                    </div>
                </td>
                <td>
                    <div style="text-align: right; padding-right: 5px;">
                        Copyright © 2013 MAN Trucks India. All Rights Reserved
                    </div>
                </td>
                <td class="lasttd">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/footer_right.gif" AlternateText="MTI Logo"
                        Width="5" Height="49" />
                </td>
            </tr>
        </table>--%>

    </form>
</body>
    <script src="Scripts/jquery-1.12.2.min.js"></script>
<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>--%>
<script src="Scripts/bootstrap.min.js"></script>
</html>
