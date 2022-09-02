<%@ Page Title="MTI-User SignUp" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmSignup.aspx.cs" Inherits="MANART.Forms.Admin.frmSignup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
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
            var Control = document.getElementById('ContentPlaceHolder1_txtID');
            if (Control != null) {
                Control.style.visibility = "hidden";
            }
            Control = document.getElementById('ContentPlaceHolder1_txtControlCount');
            if (Control != null) {
                Control.style.visibility = "hidden";
            }
        }
    </script>
    <script type="text/javascript">
        function ShowUserSelect(para) {
            var radioButtons = document.getElementsByName("ContentPlaceHolder1_OptionUserType_0");
            var Flage;
            if (radioButtons[0].checked) {
                Flage = 1;
            }
            var Return;
            Return = window.showModalDialog("frmSelectDealerOrVECVAsUser.aspx?Flage=" + Flage, "List", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            if (Return != null) {
                document.getElementById("ContentPlaceHolder1_txtID").value = Return[0];
                document.getElementById("ContentPlaceHolder1_lblUserRegitrationFor").value = "User Registration For " + Return[1];
                document.getElementById("ContentPlaceHolder1_txtFirstName").value = Return[1];
                document.getElementById("ContentPlaceHolder1_txtLoginName").value = Return[2];
            }
        }

        function DisplayEmpCode() {
            var drpDept = document.getElementById("ContentPlaceHolder1_drpDept")
            var drpUserType = document.getElementById("ContentPlaceHolder1_drpUserType");
            var DeptText = drpDept.options[drpDept.selectedIndex].text;

            var txtEmpCode = document.getElementById("ContentPlaceHolder1_txtEmpCode")

            var lblModelCategoryMD = document.getElementById("ContentPlaceHolder1_lblModelCategoryMD")
            var chkModelCategory = document.getElementById("ContentPlaceHolder1_chkModelCategory")
            var lblModelCategory = document.getElementById("ContentPlaceHolder1_lblModelCategory")

            if (DeptText == "Service")
                txtEmpCode.style.display = '';
            else {
                txtEmpCode.style.display = 'none';
            }
            if ((DeptText.trim().toUpperCase() == "SALES" || DeptText.trim().toUpperCase() == "SERVICE") && (drpUserType.value == "2" || drpUserType.value == "1" || drpUserType.value == "8")) {
                lblModelCategoryMD.style.display = '';
                chkModelCategory.style.display = '';
                lblModelCategory.style.display = '';
            }
            else {
                lblModelCategoryMD.style.display = 'none';
                (chkModelCategory != null) ? chkModelCategory.style.display = 'none' : chkModelCategory = null;
                lblModelCategory.style.display = 'none';
            }

        }

        function setLoginName(objSelect) {
            //debugger;
            var drpUserType = document.getElementById("ContentPlaceHolder1_drpUserType")
            var UserTypevalue = drpUserType.options[drpUserType.selectedIndex].value;




            var txtEmail = document.getElementById("ContentPlaceHolder1_txtEmail")
            var txtLoginName = document.getElementById("ContentPlaceHolder1_txtLoginName")


            var hdnSpareCode = document.getElementById("ContentPlaceHolder1_hdnSpareCode")
            var hdnSalesCode = document.getElementById("ContentPlaceHolder1_hdnSalesCode")


            //var splEmail = txtEmail.value.split('@');
            if (objSelect == "VECVUser") {
                var txtEmpCode = document.getElementById("ContentPlaceHolder1_txtEmpCode")
                
                if (txtEmpCode.value != "") {
                    //for (var i = 0; i < Deptvalue.length; i++) {
                    //    if (Deptvalue[i].text.toUpperCase().lastIndexOf(txtEmpCode.value.toUpperCase()) != -1) {
                    //        alert("Employee Code '" + Deptvalue[i].text.toUpperCase() + "' already Exist!");
                    //        txtEmpCode.focus();
                    //        return false;
                    //    }
                    //}

                    var patt = new RegExp("[0-9a-zA-Z]");
                    var space = /\s+/
                    if (!patt.test(txtEmpCode.value)) {
                        alert("Please Enter Valid Employee Code!");
                        txtEmpCode.focus();
                        return false;
                    }
                    else if (space.test(txtEmpCode.value)) {
                        alert("Space not allowed!");
                        txtEmpCode.focus();
                        return false;
                    }

                }
                var splEmail = "dCAN" + txtEmpCode.value;

                var drpLoginName = document.getElementById("ContentPlaceHolder1_drpLoginName")
                var LoginName = drpLoginName.options;

                var drpEmpCode = document.getElementById("ContentPlaceHolder1_drpEmpCode")
                var Deptvalue = drpEmpCode.options;
                
                //if (txtEmail.value != "" && splEmail.length > 1)
                //for (var i = 0; i < LoginName.length; i++) {
                //    if (LoginName[i].text.toUpperCase() == splEmail[0].toUpperCase()) { //.lastIndexOf(splEmail[0].toUpperCase()) != -1
                //        alert("Login Name '" + LoginName[i].text.toUpperCase() + "' already Exist!");
                //        txtEmail.focus();                        
                //    }
                //}

                //if (UserTypevalue == "1" || UserTypevalue == "2" || UserTypevalue == "5" || UserTypevalue == "8") {
                //    if (splEmail.length > 1)
                //        txtLoginName.value = splEmail[0].toUpperCase();
                //    else
                //        txtLoginName.value = '';
                //}

                if (txtEmpCode.value != "" && splEmail.length > 1)
                    for (var i = 0; i < LoginName.length; i++) {
                        if (LoginName[i].text.toUpperCase() == splEmail.toUpperCase()) { //.lastIndexOf(splEmail[0].toUpperCase()) != -1
                            alert("Login Name '" + LoginName[i].text.toUpperCase() + "' already Exist!");                            
                            txtEmpCode.focus();
                        }
                    }
                
                if (UserTypevalue == "1" || UserTypevalue == "2" || UserTypevalue == "5" || UserTypevalue == "8") {
                    if (splEmail.length > 1)
                        txtLoginName.value = splEmail.toUpperCase();
                    else
                        txtLoginName.value = '';
                }
            }
            else if (objSelect == "DealerUser") {
                var chkDept = document.getElementById("ContentPlaceHolder1_chkDept");
                var chkBoxCount = chkDept.getElementsByTagName("input");
                txtLoginName.value = '';
                for (var i = 0; i < chkBoxCount.length; i++) {
                    if (txtLoginName.value == '' && chkBoxCount[i].checked == true && chkBoxCount[i].disabled == false) {
                        if (chkDept.cells[i].innerText.toUpperCase().lastIndexOf("SALES") == -1) {
                            if (hdnSpareCode.value != "")
                                txtLoginName.value = hdnSpareCode.value + chkDept.cells[i].innerText.toUpperCase();
                            else {
                                alert('Dealer user for Spare/Service cannot create.Spare/Service code not found');
                            }
                        }
                        else if (chkDept.cells[i].innerText.toUpperCase().lastIndexOf("SALES") != -1) {
                            if (hdnSalesCode.value != "")
                                txtLoginName.value = hdnSalesCode.value + chkDept.cells[i].innerText.toUpperCase();
                            else {
                                alert('Dealer user for Sale cannot create.Sales code not found!');
                            }
                        }
                    }
                    else if (txtLoginName.value != '' && chkBoxCount[i].checked == true && chkBoxCount[i].disabled == false) {
                        if (chkDept.cells[i].innerText.toUpperCase().lastIndexOf("SALES") == -1 && hdnSpareCode.value != "")
                            txtLoginName.value = txtLoginName.value + '/' + hdnSpareCode.value + chkDept.cells[i].innerText.toUpperCase();
                        else if (hdnSalesCode.value != "")
                            txtLoginName.value = txtLoginName.value + '/' + hdnSalesCode.value + chkDept.cells[i].innerText.toUpperCase();
                    }

                }

            }
        }


    </script>

    <script src="../../Scripts/jsAdminFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="border:double">
        <div class="row">
            <div class="col-md-12 panel-heading">
                <div class=" panel-title" align="center"><strong>New User SIgn Up</strong></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="well well-sm" align="center">
                    <span>( Note: A confirmation code will be sent via email.) </span>
                    <br />
                    <span style="color: Red">* An Asterisk indicates required information </span>
                </div>
            </div>
        </div>
        <div class="form-horizontal">
            <div class="form-group">
                <label class="col-xs-2 ">Select User Type</label>
                <div class="col-xs-4">
                    <asp:DropDownList ID="drpUserType" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="True"
                        OnSelectedIndexChanged="drpUserType_SelectedIndexChanged" TabIndex="1">
                    </asp:DropDownList>
                </div>
                <label class="col-xs-2"> <asp:Label ID="lblSelDlr" runat="server" Text="Select Dealer"></asp:Label></label>
                <div class="col-xs-4">
                    <asp:DropDownList ID="drpDealer" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="True"
                        OnSelectedIndexChanged="drpDealer_SelectedIndexChanged" TabIndex="2"
                        Width="90%">
                    </asp:DropDownList>
                </div>
            </div>

           <%-- <div  class="form-group HideControl">--%>
                  <div id="dvHOBranch"  class="form-group" runat ="server" Style="display: none" >
                <label class="col-xs-2 ">Dealer User Type:</label>
                <div class="col-xs-4">
                    <asp:DropDownList ID="drpDlrLocation" runat="server" CssClass="ComboBoxFixedSize"
                        OnSelectedIndexChanged="drpDlrLocation_SelectedIndexChanged" TabIndex="3" Width="38%" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <label class="col-xs-2">Dealer HO/Branch List:</label>
                <div class="col-xs-4">
                    <asp:DropDownList ID="drpHOBRList" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpHOBRList_SelectedIndexChanged"
                        TabIndex="3" Width="90%" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 ">Role:</label>
                <div class="col-xs-4">
                    <asp:DropDownList ID="drpLevels" runat="server" CssClass="ComboBoxFixedSize" Width="38%" OnSelectedIndexChanged="drpLevels_SelectedIndexChanged"
                        TabIndex="3" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <label class="col-xs-2">Email:</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString" Text="" TabIndex="4" Width="60%"  placeholder="Email"
                        MaxLength="50" onblur="return setLoginName('VECVUser');"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 ">First Name:</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="TextBoxForString" TabIndex="5" placeholder="First Name"
                        MaxLength="20"></asp:TextBox>
                </div>
                <label class="col-xs-2"> <asp:Label ID="lblEmpCode1" runat="server" Text="Employee Code :"></asp:Label></label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtEmpCode" runat="server" CssClass="TextBoxForString" Text="" TabIndex="8"
                        MaxLength="8" onpaste="return false" onblur="return setLoginName('VECVUser');"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 ">Last Name:</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="TextBoxForString" Text="" placeholder="Last Name"
                        TabIndex="7" MaxLength="20"></asp:TextBox>
                </div>
                <label class="col-xs-2">Login Name:</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="TextBoxForString" TabIndex="6"></asp:TextBox>
                    <asp:ImageButton ID="btnAvailability" runat="server" Height="14px" ImageUrl="~/Images/CheckUserAvail.jpg"
                        ToolTip="Check User Availability" Width="19px" TabIndex="6" OnClick="btnAvailability_Click"
                        Style="display: none" />
                    &nbsp;<asp:Label ID="lblavail" runat="server" Style="font-weight: 700; display: none"></asp:Label>
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 ">Department :</label>
                <div class="col-xs-4">
                    <asp:CheckBoxList ID="chkDept" CssClass="ComboBoxFixedSize" runat="server" RepeatDirection="Vertical"
                        TabIndex="9" Width="80%" onclick="return setLoginName('DealerUser');">
                    </asp:CheckBoxList>
                    <asp:DropDownList ID="drpDept" runat="server" TabIndex="9" CssClass="ComboBoxFixedSize" Width="38%"
                        AutoPostBack="true" OnSelectedIndexChanged="drpDept_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <asp:Label ID="lblModelCategory" CssClass="col-xs-2" runat="server" Text="Model Category :" Style="display: none"></asp:Label>
                <%--<label class="col-xs-2">Model Category :</label>--%>
                <div class="col-xs-4">
                    <asp:CheckBoxList ID="chkModelCategory" CssClass="ComboBoxFixedSize" runat="server"
                        RepeatDirection="Horizontal" TabIndex="9" Width="80%" Style="display: none">
                    </asp:CheckBoxList>
                </div>
            </div>

            <div class="form-group">
                <div class="col-xs-4"></div>
                <div class="col-xs-4" >
                    Please enter this number:
                    <br />
                    <asp:Image ID="ImageCaptcha0" runat="server" AlternateText="If you can't read this number refresh your screen"
                        BorderWidth="1" ImageUrl="../../WebService/captcha.ashx" CssClass="center-block" ImageAlign="Left" />
                    <br />
                    <br />
                    <asp:TextBox ID="TextBox_number" runat="server" CssClass="TextBoxForString" EnableViewState="False"
                        TabIndex="10"></asp:TextBox>
                    <asp:ImageButton ID="btnCaptchaChange0" runat="server" ImageUrl="~/Images/captcha-change.gif"
                        OnClick="btnCaptchaChange_Click" TabIndex="11" />
                    <br />
                    <asp:Label ID="lblCaptcha" runat="server" Style="color: #FF0000"></asp:Label>
                </div>
                <div class="col-xs-4"></div>
            </div>

            <div class="form-group">
                <div class="col-xs-4"></div>
                <div class="col-xs-4">
                    <asp:Button ID="btnSignup" runat="server" OnClientClick="return validate();" Text="Sign up"
                        OnClick="btnSignup_Click" TabIndex="12" CssClass="CommandButton btn btn-primary" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                        TabIndex="13" CssClass="CommandButton btn btn-primary" />
                </div>
                <div class="col-xs-4"></div>
            </div>

            <div class="form-group">
                <div class="col-xs-4 ">
                    <div class="HideControl">
                    <asp:TextBox ID="txtAvailCheck" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnSpareCode" runat="server" Value="" />
                    <asp:HiddenField ID="hdnSalesCode" runat="server" Value="" />
                    <asp:HiddenField ID="hdnHOBranchCode" runat="server" Value="" />
                    <asp:HiddenField ID="hdnHOBranch" runat="server" Value="" />
                    <asp:HiddenField ID="hdnHOBranchID" runat="server" Value="" />
                    <asp:DropDownList ID="drpLoginName" runat="server" Style="display: none">
                    </asp:DropDownList>
                    <asp:DropDownList ID="drpEmpCode" runat="server" Style="display: none">
                    </asp:DropDownList>
                        </div>
                </div>
                <div class="col-xs-4">
                </div>
                <div class="col-xs-4"></div>
            </div>

        </div>
    </div>


    <table id="PageTbl" class="PageTable" border="1" style="display:none">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="New User Sign up">
                </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="tblLogin" runat="server" class="ContainTable">
                    <tr>
                        <td align="center" style="padding-top: 5px; padding-bottom: 5px;">
                            <span>( Note: A confirmation code will be sent via email.) </span>
                            <br />
                            <span style="color: Red">* An Asterisk indicates required information </span>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 15%">
                <asp:Panel ID="PanelUserSelection" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" runat="server" class="ContainTable">
                        <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="4">User Type Selection
                            </td>
                        </tr>
                        <tr style="padding-top: 10px;">
                            <td class="tdLabel">
                                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                                    Text=""></asp:TextBox>
                                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%;">Select User Type:&nbsp;&nbsp;
                            </td>
                            <td style="width: 35%" class="tdLabel">
                            </td>
                            <td class="tdLabel" valign="middle" style="width: 15%; margin-right: 5px;">
                                <asp:Label ID="lblDealer" runat="server" Text="Select Dealer :" Style="display: none"></asp:Label>&nbsp;&nbsp;
                            </td>
                            <td style="width: 35%">
                                
                            </td>
                        </tr>
                        
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelLoginDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="tblProformaDetails" runat="server" class="ContainTable">
                        <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="4">Login Information
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="height: 15px" colspan="4">
                                <asp:TextBox ID="lblUserRegitrationFor" runat="server" Style="font-weight: 700; text-align: center; color: #339966; background-color: #F0F0F0; border: 0px;"
                                    Width="761px" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trDealer" runat="server" style="display: none">
                            <td style="width: 15%" class="tdLabel">Dealer User Type:
                            </td>
                            <td class="tdLabel" style="width: 35%">
                                
                                <b class="Mandatory">*</b>
                            </td>
                            <td style="width: 15%" class="tdLabel">Dealer HO/Branch List:
                            </td>
                            <td class="tdLabel" style="width: 35%;">
                               
                                <b class="Mandatory">*</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="tdLabel">Role:
                            </td>
                            <td class="tdLabel" style="width: 35%">
                               
                                <b class="Mandatory">*</b>
                            </td>
                            <td style="width: 15%" class="tdLabel">Email:
                            </td>
                            <td class="tdLabel" style="width: 35%;">
                               
                                <b class="Mandatory">*</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="tdLabel">First Name:
                            </td>
                            <td class="tdLabel" style="width: 35%">
                               
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel" style="width: 15%">Login Name:
                            </td>
                            <td class="tdLabel" style="width: 35%; vertical-align: bottom">
                              
                            </td>
                        </tr>
                        <tr style="padding-top: 10px;">
                            <td class="tdLabel" style="width: 15%">Last Name:
                            </td>
                            <td class="tdLabel" style="width: 35%">
                            
                                <b class="Mandatory">*</b>
                            </td>
                            <td class="tdLabel" style="width: 15%">
                                <asp:Label ID="lblEmpCode" runat="server" Text="Employee Code :"></asp:Label>&nbsp;&nbsp;
                            </td>
                            <td class="tdLabel" style="width: 35%;">
                              
                            </td>
                        </tr>
                        <tr id="trDept" runat="server" style="display: none">
                            <td class="tdLabel" style="width: 15%">Department :
                            </td>
                            <td class="tdLabel" style="width: 35%;">
                                <table width="30%">
                                    <tr>
                                        <td>
                                        
                                        </td>
                                        <td align="left" class="tdLabel">
                                            <b class="Mandatory">*</b>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdLabel" style="width: 15%">
                            </td>
                            <td class="tdLabel" style="width: 35%">
                                <table width="40%">
                                    <tr>
                                        <td>
                                          
                                        </td>
                                        <td align="left" class="tdLabel">
                                            <b class="Mandatory">
                                                <asp:Label ID="lblModelCategoryMD" runat="server" Text="*" Style="display: none"></asp:Label></b>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="height: 15px" colspan="4">Please enter this number:<br />
                              
                                <br />
                                <br />
                                
                                &nbsp;&nbsp;&nbsp;
                             
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="height: 15px" colspan="4">
                                <br />
                             
                                &nbsp;
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 15%">
               
            </td>
        </tr>
    </table>
</asp:Content>
