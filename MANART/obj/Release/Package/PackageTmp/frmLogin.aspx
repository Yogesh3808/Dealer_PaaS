<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="MANART.frmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>MTI- Home Page </title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <!-- Le styles -->
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/style.css" rel="stylesheet" />

    <%-- <script src="Scripts/jquery-1.10.2.js"></script>--%>

    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js">
</script>--%>

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
<![endif]-->

    <%--<script src="https://cdn.jsdelivr.net/jquery.validation/1.15.0/jquery.validate.js"></script>--%>


    <script>
        //$(document).ready(function () {
        //    $("#btnLogin").click(function () {

        //    });
        //});
        function Validate() {
            var txtLoginName = $("#txtLoginName").val();
            var txtPassword = $("#txtPassword").val();
            // Checking for blank fields.
            if (txtLoginName == '' || txtPassword == '') {
                $('input[type="text"],input[type="txtPassword"]').css("border", "2px solid red");
                $('input[type="text"],input[type="txtPassword"]').css("box-shadow", "0 0 3px red");
                alert("Please fill all fields...!!!!!!");
                return false;
            }
        }

        function UserNotExist() {
            alert("User Not Exixst!");
            //$('input[type="text"], input[type="Password"]').val('');
            //$("#txtLoginName").focus();
            $('input[type=text]').each(function () {
                $(this).val('');
            })
        }
        function LockedUser() {
            $('input[type="text"], input[type="Password"]').val() = '';
            alert("This User ID is Locked! Please contact to MTI Administrator!");
        }
        function ValidateUser() {
            if ($("#txtLoginName").val() == '') {
                $("#txtLoginName").focus();
                $('input[type="text"]').css("border", "2px solid red");
                $('input[type="text"]').css("box-shadow", "0 0 3px red");
                alert("Please fill UserName fields");
                return false;
            }
            if ($("#txtPassword").val() == '') {
                $("#txtPassword").focus();
                $('input[type="Password"]').css("border", "2px solid red");
                $('input[type="Password"]').css("box-shadow", "0 0 3px red");
                alert("Please fill Correct Password fields or Don't Blank");
                return false;
            }
            if ($("#txtPassword").val() != '') {
                $("#txtPassword").focus();
                $('input[type="Password"]').css("border", "2px solid red");
                $('input[type="Password"]').css("box-shadow", "0 0 3px red");
                alert("Please fill Correct Password fields or Don't Blank");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function Show(para) {
            var LoginName = document.getElementById("txtLoginName");
            if (LoginName.value == "") {
                alert("Please Enter User Name");
                LoginName.focus();
                return false;
            }
            else {
                window.showModalDialog("./Forms/Admin/frmForgetPassword.aspx?LoginName=" + LoginName.value, "List", "dialogHeight: 150px; dialogWidth: 500px;  edge: Raised; center: Yes; help: No; scroll: No; status: No;");
            }
            return false;
        }

        //         function Show1(para) {
        //             var LoginName = document.getElementById("txtLoginName");
        //             if (LoginName.value == "") {
        //                 alert("Please Enter User Name");
        //                 LoginName.focus();
        //                 return false;
        //             }
        //             else {
        //                 window.showModalDialog("Forms/Admin/frmChangePassword.aspx?LoginName=" + LoginName.value, "List", "dialogHeight: 250px; dialogWidth: 600px;  edge: Raised; center: Yes; help: No; scroll: No; status: No;");
        //             }
        //             return false;
        //         }

    </script>

    <script type="text/javascript">
        function CheckLoginCredential() {
            var jtxtPassword = document.getElementById('txtPassword');
            var jtxtLoginName = document.getElementById('txtLoginName');
            if (jtxtLoginName.value == "") {
                alert("Please Enter Login Name.");
                jtxtLoginName.focus();
                return false;
            }
            if (jtxtPassword.value == "") {
                alert("Please Enter Password.");
                jtxtPassword.focus();
                return false;
            }
        }
    </script>
    <script type="text/javascript">
<!--      Begin
    function checkCapsLock(e) {
        var myKeyCode = 0;
        var myShiftKey = false;
        var myMsg = 'Caps Lock is On.\n\nTo prevent entering your password incorrectly,\nyou should press Caps Lock to turn it off.';

        // Internet Explorer 4+
        if (document.all) {
            myKeyCode = e.keyCode;
            myShiftKey = e.shiftKey;

            // Netscape 4
        } else if (document.layers) {
            myKeyCode = e.which;
            myShiftKey = (myKeyCode == 16) ? true : false;

            // Netscape 6
        } else if (document.getElementById) {
            myKeyCode = e.which;
            myShiftKey = (myKeyCode == 16) ? true : false;

        }

        // Upper case letters are seen without depressing the Shift key, therefore Caps Lock is on
        if ((myKeyCode >= 65 && myKeyCode <= 90) && !myShiftKey) {
            alert(myMsg);

            // Lower case letters are seen while depressing the Shift key, therefore Caps Lock is on
        } else if ((myKeyCode >= 97 && myKeyCode <= 122) && myShiftKey) {
            alert(myMsg);

        }
    }
    //  End -->
    </script>

    <script>
        function capLock(e) {
            kc = e.keyCode ? e.keyCode : e.which;
            sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
                document.getElementById('divMayus').style.visibility = 'visible';
            else
                document.getElementById('divMayus').style.visibility = 'hidden';
        }
    </script>
</head>
<body runat="server">
    <form id="form1" runat="server">
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
                <%--<span class="siteTitle_right" style="margin-top:-15px;">--%>
                    <span class="siteTitle_right">
                    dCAN Helpdesk Email: dCAN.Helpdesk@in.man.eu
                    <br />
                         dCAN Phone numbers : +91 20 6645 2924 / 2948 (08:30 to 18:00 Hours on working days)
                    <%--dCAN Toll free number : 1800 266 6599 (08:30 to 18:00 Hours on working days)--%>
                    <%--dCAN Toll free 24x7 Customer support -1800 266 6599 (08:30 to 18:00 Hours on working days) --%>
                    <%--dCAN Toll free number : 1800 266 6599--%> 
                    <br />
                   <%-- <span class="siteTitle_right_new">
                    Working Hour:Monday-Friday 8:30a.m - 6p.m
                </span>--%>
                </span>
                
                <%--<div class="site-title"  >
                     dCAN Helpdesk Email: dCAN.Helpdesk@in.man.eu
                </div>--%>
            </div>
        </header>
        <!-- Login -->
        <section>
            <div class=" container">
                <div class="loging">
                    <div class="login-id">
                        <div class="col-md-12 ">
                            <span>
                                <asp:Label ID="lblResult" runat="server"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class="Login-info ">
                        <div class="row">

                            <div class="col-md-5 col-sm-5 col-xs-12 col-lg-5">
                                <div class="input-group">
                                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="form-control login-input" placeholder="LOGIN ID"></asp:TextBox>
                                    <%--<input type="text" class="form-control login-input" placeholder="LOGIN ID" id="txtLoginName" runat="server">--%>
                                    <span class="input-group-btn">
                                        <div class="btn-icon"><i class="icon user"></i></div>
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-5 col-sm-5 col-xs-12 col-lg-5">
                                <div class="input-group">
                                    <asp:TextBox ID="txtPassword" runat="server" class="form-control login-input" placeholder="PASSWORD" TextMode="Password"></asp:TextBox>
                                    <%--<input type="password" class="form-control login-input" placeholder="PASSWORD" runat="server" id="txtPassword" required="required" >--%>
                                    <span class="input-group-btn">
                                        <div class=" btn-icon"><i class="icon pass"></i></div>
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-2 col-xs-12 col-lg-2">
                                <div class="input-group">
                                    <asp:Button ID="btnLogin" runat="server" Text="LOGIN" CssClass="btn btn-black" OnClick="btnLogin_Click" OnClientClick="return Validate();"></asp:Button>
                                    <%--<button class="btn btn-black" type="button" id="Login" runat="server"  onserverclick="btn_Login">LOGIN</button>--%>
                                </div>
                            </div>
                            <div class="forgot-pass text-right">
                                <div id="divMayus" style="visibility: hidden; color: Red; font-family: Arial; font-size: 9px; font-weight: bold">Caps Lock is on.</div>
                                <asp:LinkButton ID="lblForgotPassWord" runat="server" CausesValidation="false" Text="Forgot your password?"
                                    OnClientClick='return Show(this);'></asp:LinkButton>
                                <%--<a href="#">Forgot Password</a></div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container">
                <div id="carousel-a" class="carousel slide carousel-sync" data-ride="carousel" data-pause="false">
                    <div class="carousel-inner" role="listbox">
                        <div class="item active">
                            <%--<img src="Images/banner2.png" alt="man" style="width: 100%">--%>
                            <img src="Images/MANTruck1.png" alt="man" style="width: 100%; height:500px;">
                            <div class="carousel-caption">
                                <div>
                                    <span class="Black-strip">df</span>
                                </div>
                                <div class="title-info">
                                    <h3>Made in India, Made for India</h3>
                                    <p>Solutions for construction industry and public utility vehicles</p>
                                </div>
                            </div>
                        </div>

                        <%--<div class="item">
                            <img src="Images/MANTruck2.png" alt="man" style="width: 100%; height:500px;">

                            <div class="carousel-caption">
                                <span class="Black-strip">df</span>

                                <div class="title-info">
                                    <h3>MAN SPECIAL APPLICATIONS</h3>
                                    <p>Solutions for construction industry and public utility vehicles</p>
                                </div>
                            </div>

                        </div>--%>

                        <div class="item">
                            <img src="Images/MANTruck3.png" alt="man" style="width: 100%; height:500px;">

                            <div class="carousel-caption">
                                <span class="Black-strip">df</span>

                                <div class="title-info">
                                    <h3>MAN SPECIAL APPLICATIONS</h3>
                                    <p>Solutions for construction industry and public utility vehicles</p>
                                </div>
                            </div>

                        </div>

                        <%--<div class="item">
                            <img src="Images/MANTruck4.png" alt="man" style="width: 100%; height:500px;">

                            <div class="carousel-caption">
                                <span class="Black-strip">df</span>

                                <div class="title-info">
                                    <h3>MAN SPECIAL APPLICATIONS</h3>
                                    <p>Solutions for construction industry and public utility vehicles</p>
                                </div>
                            </div>

                        </div>--%>

                    </div>

                </div>

            </div>
        </section>

        <section>
            <div class="container">
                <div class="row">
                    <div class="col-md-8 col-sm-8">
                        <header>
                            <h1>MAN Trucks India</h1>
                            <div class="devider"></div>
                        </header>
                        <p>
                            MAN Trucks India in the past six years since its inception has positioned itself as a key player on the technology platform by offering high quality and cost effective products. The company's initial success came through off-road applications like trucks used in mining, construction and road building activities, which was mainly due to the inherent strength and robustness of MAN Tipper range. Later with introduction of haulage applications MAN Trucks India entered into mass market
MAN Trucks India in the past six years since its inception has positioned itself as a key player on the technology platform by offering high quality and cost effective products. The company's initial success came through off-road applications like trucks used in mining, construction and road building activities, which was mainly due to the inherent strength and robustness of MAN Tipper range. Later with introduction of haulage applications MAN Trucks India entered into mass market
                        </p>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="news-event">
                            <h2>News and Event</h2>
                            <div id="carousel-b" class="carousel slide carousel" data-ride="carousel" data-pause="false">
                                <div class="carousel-inner" role="listbox">
                                    <div class="item active">

                                        <span>10/09/2015 | Truck</span>
                                        <strong>Man Trucks India signs MOU with Canara Bank</strong>

                                        <p>
                                            When massive projects must be executed within tight deadlines,
        MAN Tippers rise to the occasion. They are deployed across
        the length and breadth of the country for the toughest assignments.
                                        </p>
                                        <a href="#"><b class="red-color">> </b>More..</a>



                                    </div>
                                    <div class="item ">

                                        <span>10/09/2015 | Truck</span>
                                        <strong>Man Trucks India signs MOU with Canara Bank</strong>

                                        <p>
                                            When massive projects must be executed within tight deadlines,
        MAN Tippers rise to the occasion. They are deployed across
        the length and breadth of the country for the toughest assignments.
                                        </p>
                                        <a href="#"><b class="red-color">> </b>More..</a>



                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
</div>
        </section>

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
    </form>
</body>
<script src="Scripts/jquery-1.12.2.min.js"></script>
<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>--%>
<script src="Scripts/bootstrap.min.js"></script>
<script>
    $('.carousel-sync').on('slide.bs.carousel', function (ev) {
        var dir = ev.direction == 'right' ? 'prev' : 'next';
        $('.carousel-sync').not('.sliding').addClass('sliding').carousel(dir);
    });
    $('.carousel-sync').on('slid.bs.carousel', function (ev) {
        $('.carousel-sync').removeClass('sliding');
    });
</script>
</html>
