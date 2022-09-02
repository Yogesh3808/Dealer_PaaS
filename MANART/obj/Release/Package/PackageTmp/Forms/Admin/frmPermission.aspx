<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPermission.aspx.cs" Inherits="MANART.Forms.Admin.frmPermission1"
    Title="Permission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Permission</title>

    <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js">
</script>--%>

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
<![endif]-->

    <%--<script src="https://cdn.jsdelivr.net/jquery.validation/1.15.0/jquery.validate.js"></script>--%>


    <%--<link href="../../Content/bootstrap.css" rel="stylesheet" />--%>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsAdminFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>

    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jspermission.js"></script>
    <script src="../../Scripts/jquery-1.11.1.js"></script>
    <style type="text/css">
        .checkbox {
            padding-left: 20px;
        }

            .checkbox label {
                display: inline-block;
                vertical-align: middle;
                text-align: left;
                position: relative;
                padding-left: 5px;
                width: auto;
            }

                .checkbox label::before {
                    content: "";
                    display: inline-block;
                    position: absolute;
                    width: 17px;
                    height: 17px;
                    left: 0;
                    margin-left: -20px;
                    border: 1px solid #cccccc;
                    border-radius: 3px;
                    background-color: #fff;
                    -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                }

                .checkbox label::after {
                    display: inline-block;
                    position: absolute;
                    width: 16px;
                    height: 16px;
                    left: 0;
                    top: 0;
                    margin-left: -20px;
                    padding-left: 3px;
                    padding-top: 1px;
                    font-size: 11px;
                    color: #555555;
                }

            .checkbox input[type="checkbox"] {
                opacity: 0;
                z-index: 1;
            }

                .checkbox input[type="checkbox"]:checked + label::after {
                    font-family: "FontAwesome";
                    content: "\f00c";
                }

        .checkbox-primary input[type="checkbox"]:checked + label::before {
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .checkbox-primary input[type="checkbox"]:checked + label::after {
            color: #fff;
        }
    </style>
    <script type="text/javascript">
        var TotalChkBx;
        var Counter;

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
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gridPermission.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }
        function ClosePopupWindow() {
            window.close();
            return false;
        }
        function Confirmation() {
            //First name
            if (document.getElementById("txtFirstName").value != '') {
                for (var i = 0; i < document.getElementById("txtFirstName").value.length; i++) {
                    if (document.getElementById("txtFirstName").value.charAt(0) == '') {
                        alert("First Name: Please do not enter space(s).");
                        document.getElementById("txtFirstName").focus();
                        return false;
                    }
                }
            }
            else {
                alert("First Name: Please enter First Name.");
                document.getElementById('txtFirstName').focus();
                return false;
            }
            //Last Name
            if (document.getElementById("hdnUserRole").value != '6' && document.getElementById("txtLastName").value != '') {
                for (var i = 0; i < document.getElementById("txtLastName").value.length; i++) {
                    if (document.getElementById("txtLastName").value.charAt(0) == '') {
                        alert("Last Name: Please do not enter space(s).");
                        document.getElementById("txtLastName").focus();
                        return false;
                    }
                }
            }
            else if (document.getElementById("hdnUserRole").value != '6') {
                alert("Last Name: Please enter Last Name.");
                document.getElementById('txtLastName').focus();
                return false;
            }

            //txtEmail
            if (document.getElementById("txtEmail").value == '') {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi")
                document.getElementById("txtEmail").focus();
                return false;
            }
            else {

                var result = validEmail(document.getElementById("txtEmail").value)
                if (result != "") {
                    alert("Email: Please enter a valid e-mail address, such as abc@def.ghi\n\n")
                    document.getElementById("txtEmail").focus();
                    return false;
                }

            }
            debugger;
            var sLevels = "";
            var drpLevels = window.document.getElementById("drpLevels");
            if (drpLevels != null) {               
                sLevels = drpLevels.options[drpLevels.selectedIndex].value;
            }
            if (document.getElementById("hdnUserRoleDatabase").value.trim() != sLevels && document.getElementById("hdnLinked").value.trim() == "Y")
            {
                alert("Please delink all present(upper and lower) heirarchy linking and then change Role.")                
                return false;
            }

            var sDeptID = "";
            var drpDept = window.document.getElementById("drpDept");
            if (drpDept != null) {
                sDeptID = drpDept.options[drpDept.selectedIndex].value;
            }
            if (document.getElementById("hdnDeptdatabase").value.trim() != sDeptID && document.getElementById("hdnLinked").value.trim() == "Y") {
                alert("Please delink all present(upper and lower) heirarchy linking and then change Department.")
                return false;
            }

            //    if (document.getElementById("drpDept").value == '0' && document.getElementById("drpUserType").value != '5' && document.getElementById("drpLevels").value != '8') {
            //        alert("Deparment: Please Select Department")
            //        document.getElementById("drpDept").focus();
            //        return false;
            //    }

            if (confirm('Do you want to save') == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function validEmail(email) {
            // returns "" if valid else the error string	
            // you can add your own custom checks.

            // check for invalid character
            if (email.match(/^[\w_\-\@\.]+$/) == null)
                return ("\tEmail contains character other than alphanumeric and _ - @ .}");
            // check if the .dot is in the begining of the email string
            if (email.match(/^[\.]/) != null)
                return ("\tEmail cannot start with a dot");
            // check if the .dot is in the begining of the email string
            if (email.match(/[\.]$/) != null)
                return ("\tEmail cannot end with a dot");
            // check for initial pattern
            //if (email.match(/^[\w_\-\.]+@[\w_\.\-]+\.[a-z]{2,3}$/i) == null)		// 'i' for case insensitive
            if (email.match(/^[\w_\-\.]+@[\w_\.\-]+\.[a-z]{2,4}$/i) == null)		// 'i' for case insensitive
                return ("\tEmail is not of the correct form");
            // check if the dots are adjacent	
            if (email.match(/[\.]{2,}/) != null)
                return ("\tEmail cannot have adjacent dots");
            // check if the dot is adjacent to the @ character
            if (email.match(/[\.]+@|@[\.]+/) != null)
                return ("\tEmail cannot have dot adjacent to the @ character");

            // return blank string for valid email	
            return ("");
        }


        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var CheckBoxCelIndex = CheckBox.parentNode.cellIndex;
            var TargetBaseControl =
       document.getElementById('<%= this.gridPermission.ClientID %>');
            var Inputs;
            var bchk;
            bchk = CheckBox.checked;

            for (var n = 1; n < TargetBaseControl.rows.length; ++n) {
                Inputs = TargetBaseControl.rows[n].cells[CheckBoxCelIndex].childNodes[0];
                Inputs.checked = bchk;
            }
        }

        function client_OnTreeNodeChecked(event) {
            //Vikram on 17.11.2016
            var obj = event.target || event.srcElement;
            //var e = window.event || e;
            //var obj = e.target || e.srcElement;
            //var obj = window.event.srcElement;
            var treeNodeFound = false;
            var checkedState;
            if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                var treeNode = obj;
                checkedState = treeNode.checked;
                do {
                    obj = obj.parentElement;
                } while (obj.tagName != "TABLE")
                var parentTreeLevel = obj.rows[0].cells.length;
                var parentTreeNode = obj.rows[0].cells[0];
                var tables = obj.parentElement.getElementsByTagName("TABLE");
                var numTables = tables.length
                if (numTables >= 1) {
                    for (i = 0; i < numTables; i++) {
                        if (tables[i] == obj) {
                            treeNodeFound = true;
                            i++;
                            if (i == numTables) {
                                return;
                            }
                        }
                        if (treeNodeFound == true) {
                            var childTreeLevel = tables[i].rows[0].cells.length;
                            if (childTreeLevel > parentTreeLevel) {
                                var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                var inputs = cell.getElementsByTagName("INPUT");
                                inputs[0].checked = checkedState;
                            }
                            else {
                                return;
                            }
                        }
                    }
                }
            }
        }

    </script>

    <script type="text/javascript">
        function checkAll(obj1, obj2) {
            var checkboxCollection = '';
            if (obj2 == 'Region')
                checkboxCollection = document.getElementById('<%=lstRegion.ClientID %>').getElementsByTagName('input');
            else if (obj2 == 'CountryOrState')
                checkboxCollection = document.getElementById('<%=lstCountryOrState.ClientID %>').getElementsByTagName('input');
            else if (obj2 == 'RoleSelection')
                checkboxCollection = document.getElementById('<%=lstRoleSelection.ClientID %>').getElementsByTagName('input');

    for (var i = 0; i < checkboxCollection.length; i++) {
        if (checkboxCollection[i].type.toString().toLowerCase() == "checkbox") {
            checkboxCollection[i].checked = obj1.checked;
        }
    }
}


function checkListAll(obj1, obj2) {
    var checkboxCollection = '';
    var ChkALL = '';
    if (obj2 == 'Region') {
        checkboxCollection = document.getElementById('<%=lstRegion.ClientID %>').getElementsByTagName('input');
        ChkALL = document.getElementById('<%=ChkRegion.ClientID %>')
    }
    else if (obj2 == 'CountryOrState') {
        checkboxCollection = document.getElementById('<%=lstCountryOrState.ClientID %>').getElementsByTagName('input');
        ChkALL = document.getElementById('<%=ChkCountryOrState.ClientID %>');
    }
    else if (obj2 == 'RoleSelection') {
        checkboxCollection = document.getElementById('<%=lstRoleSelection.ClientID %>').getElementsByTagName('input');
        ChkALL = document.getElementById('<%=ChkRoleSelection.ClientID %>');
    }
    var Icnt = 0;
    for (var i = 0; i < checkboxCollection.length; i++) {
        if (checkboxCollection[i].type.toString().toLowerCase() == "checkbox") {
            if (checkboxCollection[i].checked == false) {
                Icnt = Icnt + 1;
            }

        }
    }
    if (Icnt > 0)
        ChkALL.checked = false;
    else
        ChkALL.checked = true;
}

    </script>

    <%-- <style type="text/css">
        .style1 {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10px;
            color: Black;
            font-weight: bold;
            padding-left: 10px;
            text-decoration: underline;
        }

        .style2 {
            text-decoration: underline;
        }
    </style>--%>
    <base target="_self" />
</head>
<body>
    <%--<form id="form1" runat="server">
            <div class="table-responsive">
                <table id="PageTbl"  border="1">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="panel-title" align="center">
                            <asp:Label ID="lblTitle" runat="server" Text="Help Desk Admin"></asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 15%">
                            <asp:Panel ID="PanelLoginDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                                <table id="tblProformaDetails" runat="server" class="" >
                                    <tr class="panel-heading">
                                        <td align="center" class="panel-title" style="height: 15px" colspan="6">User Type-Role
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 15px" colspan="6"></td>
                                    </tr>
                                    <tr style="padding-top: 10px;">
                                        <td class="">Login&nbsp; Name:
                                        </td>
                                        <td class="">
                                            <asp:TextBox ID="txtLoginName" runat="server" CssClass="TextBoxForString" Text=""
                                                TabIndex="1" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                            <asp:Label ID="lblID" runat="server" CssClass="DispalyNon" Text=""></asp:Label>
                                        </td>
                                        <td class="">User Type:
                                        </td>
                                        <td class="">
                                            <asp:DropDownList ID="drpUserType" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="True"
                                                OnSelectedIndexChanged="drpUserType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td style="width: 15%" class="">Role:
                                        </td>
                                        <td class="" style="width: 18%">
                                            <asp:DropDownList ID="drpLevels" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpLevels_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="padding-top: 10px;">
                                        <td class="">First&nbsp; Name:
                                        </td>
                                        <td class="">
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="TextBoxForString" Text=""
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="">Last&nbsp; Name:
                                        </td>
                                        <td class="">
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="TextBoxForString" Text=""
                                                MaxLength="20"></asp:TextBox>
                                            &nbsp;
                                        </td>
                                        <td style="width: 15%" class="">Email:
                                        </td>
                                        <td class="" style="width: 18%">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString" Text="" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding-top: 10px;">
                                        <td class="">Employee Code:
                                        </td>
                                        <td class="">
                                            <asp:TextBox ID="txtEmpCode" runat="server" CssClass="TextBoxForString" Text="" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td class="">Department :
                                        </td>
                                        <td class="">
                                            <asp:DropDownList ID="drpDept" runat="server" CssClass="ComboBoxFixedSize" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="" style="width: 15%">
                                            <asp:Label ID="lblModelCategory" runat="server" Text="Model Category :" Style="display: none"></asp:Label>
                                        </td>
                                        <td class="" style="width: 35%">
                                            <table width="40%">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="chkModelCategory" CssClass="ComboBoxFixedSize" runat="server"
                                                            RepeatDirection="Horizontal" TabIndex="9" Width="80%" Style="display: none">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="" align="center" colspan="6">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return Confirmation();"
                                                OnClick="btnSave_Click" CssClass="CommandButton btn btn-primary btn-sm" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="return ClosePopupWindow();"
                                    CssClass="CommandButton btn btn-primary btn-sm" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
                                <table id="Table1" runat="server" class="">
                                    <tr class="panel-heading">
                                        <td align="center" class="panel-title" style="height: 15px" colspan="4">Region-Country/State-Department-UserRoleLink
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="" style="width: 25%">REGION<span class="style2">:</span>
                                        </td>
                                        <td class="" style="width: 30%">
                                            <asp:Label ID="lblCountryOrState" runat="server" Style="text-decoration: underline"></asp:Label>
                                        </td>
                                        <td class="style1" style="display: none">DEPARTMENT:
                                        </td>
                                        <td class="" align="left" style="width: 50%">
                                            <asp:Label ID="lblRoleSelections" runat="server" Text="" Style="text-decoration: underline"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="" style="width: 25%">
                                            <asp:CheckBox ID="ChkRegion" runat="server" onClick="checkAll(this,'Region');" Text="Select All"
                                                OnCheckedChanged="ChkRegion_CheckedChanged" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="lstRegion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstRegion_SelectedIndexChanged"
                                                onClick="checkListAll(this,'Region');">
                                            </asp:CheckBoxList>
                                            <asp:Label ID="lblRegionID" runat="server" Visible="False"></asp:Label>
                                        </td>
                                        <td class="" style="width: 30%">
                                            <div style="width: 200px; height: 150px; overflow: scroll;">
                                                <asp:CheckBox ID="ChkCountryOrState" runat="server" onClick="checkAll(this,'CountryOrState');"
                                                    Text="Select All" OnCheckedChanged="ChkCountryOrState_CheckedChanged" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="lstCountryOrState" runat="server" OnSelectedIndexChanged="lstCountryOrState_SelectedIndexChanged"
                                                    onClick="checkListAll(this,'CountryOrState');" AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td class="" style="display: none">
                                            <asp:CheckBoxList ID="lstDepartments" runat="server" OnSelectedIndexChanged="lstDepartments_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </td>
                                        <td class="" style="width: 50%">
                                            <div style="width: 80%; height: 150px; overflow: scroll; border-width: 1px">
                                                <asp:CheckBox ID="ChkRoleSelection" runat="server" onClick="checkAll(this,'RoleSelection');"
                                                    Text="Select All" />
                                                <asp:CheckBoxList ID="lstRoleSelection" runat="server" onClick="checkListAll(this,'RoleSelection');">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Double">
                                <table id="Table2" runat="server" class="">
                                    <tr class="panel-heading">
                                        <td align="center" class="panel-title" style="height: 15px" colspan="4">Permissions
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">&nbsp;
                                <asp:Label ID="lblPermissions0" runat="server">Main Menu</asp:Label>
                                        </td>
                                        <td class="">Permissions
                                        </td>
                                        <td class="">&nbsp;
                                        </td>
                                        <td class="">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="">&nbsp;
                                <div class="DivTree">
                                    <asp:TreeView ID="TreePermission" runat="server" AutoGenerateDataBindings="False"
                                        Font-Names="Verdana" Font-Size="X-Small" ForeColor="#156FC4" Height="97px" LineImagesFolder="~/Images/TreeLineImages"
                                        ShowCheckBoxes="All" ShowLines="True" Style="direction: ltr; border-top-style: none; border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none; font-family: verdana, sans-serif;"
                                        Width="244px" onclick="client_OnTreeNodeChecked();">
                                        <ParentNodeStyle CssClass="normal" />
                                        <LeafNodeStyle CssClass="normalLeaf" />
                                        <HoverNodeStyle CssClass="hover" />
                                        <RootNodeStyle CssClass="normal" />
                                        <SelectedNodeStyle CssClass="hover" />
                                    </asp:TreeView>
                                </div>
                                        </td>
                                        <td class="" colspan="2">
                                            <div id="DivSlabSelect" runat="server">
                                                &nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Show"
                                                    CssClass="CommandButton btn btn-primary btn-sm" />
                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblclmslab" runat="server" Text="Select Claim Slab: "></asp:Label>
                                                &nbsp;<asp:DropDownList ID="ddlClaimSlab" CssClass="ComboBoxFixedSize" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <br />
                                            <div class="DivTree">
                                                <div id="gridView" class="grid">
                                                    <asp:GridView ID="gridPermission" runat="server" AllowPaging="False" AlternatingRowStyle-CssClass="odd"
                                                        AutoGenerateColumns="true" CellPadding="0" CellSpacing="0" CssClass="datatable"
                                                        GridLines="None" RowStyle-CssClass="even" Style="border-collapse: separate;">
                                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                                        <RowStyle CssClass="GridViewRowStyle" />
                                                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sr&#160;No." SortExpression="LoginName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            &nbsp;
                                <br />
                                        </td>
                                        <td class="">&nbsp;
                                <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnSelRegion" runat="server" Value="" />
                <asp:HiddenField ID="hdnSelStateorCountry" runat="server" Value="" />
            </div>
        </form>--%>
    <form id="form1" runat="server">
        <table id="PageTbl" class="PageTable table-responsive" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text="Help Desk Admin"></asp:Label>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 15%">
                    <asp:Panel ID="PanelLoginDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="tblProformaDetails" runat="server" class="ContainTable">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" style="height: 15px" colspan="6">User Type-Role
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 15px" colspan="6"></td>
                            </tr>
                            <tr style="padding-top: 10px;">
                                <td class="tdLabel" style="padding-left: 10px;">Login Name:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="TextBoxForString" Text=""
                                        TabIndex="1" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    <asp:Label ID="lblID" runat="server" CssClass="DispalyNon" Text=""></asp:Label>
                                </td>
                                <td class="tdLabel">User Type:
                                </td>
                                <td class="tdLabel">
                                    <asp:DropDownList ID="drpUserType" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="True"
                                        OnSelectedIndexChanged="drpUserType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;
                                </td>
                                <td style="width: 15%" class="tdLabel">Role:
                                </td>
                                <td class="tdLabel" style="width: 18%">
                                    <asp:DropDownList ID="drpLevels" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpLevels_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="padding-top: 10px;">
                                <td class="tdLabel" style="padding-left: 10px;">First Name:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="TextBoxForString" Text=""
                                        MaxLength="20"></asp:TextBox>
                                </td>
                                <td class="tdLabel">Last&nbsp; Name:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="TextBoxForString" Text=""
                                        MaxLength="20"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td style="width: 15%" class="tdLabel">Email:
                                </td>
                                <td class="tdLabel" style="width: 18%">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString" Text="" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="padding-top: 10px;">
                                <td class="tdLabel" style="padding-left: 10px;">Employee Code:
                                </td>
                                <td class="tdLabel">
                                    <asp:TextBox ID="txtEmpCode" runat="server" CssClass="TextBoxForString" Text="" MaxLength="20"></asp:TextBox>
                                </td>
                                <td class="tdLabel">Department :
                                </td>
                                <td class="tdLabel">
                                    <asp:DropDownList ID="drpDept" runat="server" CssClass="ComboBoxFixedSize" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:Label ID="lblModelCategory" runat="server" Text="Model Category :" Style="display: none"></asp:Label>
                                </td>
                                <td class="tdLabel" style="width: 35%">
                                    <table width="40%">
                                        <tr>
                                            <td>
                                                <div class="checkbox checkbox-primary" >
                                                    <asp:CheckBoxList ID="chkModelCategory" CssClass="ComboBoxFixedSize" runat="server"
                                                        RepeatDirection="Horizontal" TabIndex="9" Width="80%" AutoPostBack="true"
                                                        Style="display: none">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" align="center" colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return Confirmation();"
                                        OnClick="btnSave_Click" CssClass="btn btn-search" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="return ClosePopupWindow();"
                                    CssClass="btn btn-search" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" class="ContainTable">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" style="height: 15px" colspan="6">Region-Country/State-Department-UserRoleLink
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 13%; padding-left: 10px;">
                                    <asp:Label ID="lblRegion1" runat="server" Text="REGION" Style="text-decoration: underline"></asp:Label>
                                </td>
                                <td class="tdLabel" style="width: 12%; padding-left: 10px;">
                                    <asp:Label ID="lblCountryOrState" runat="server" Style="text-decoration: underline"></asp:Label>
                                </td>
                                <td class="style1" style="display: none">DEPARTMENT:
                                </td>
                                <td class="tdLabel" align="left" style="width: 25%; padding-left: 10px;">
                                    <asp:Label ID="lblRoleSelections" runat="server" Text="" Style="text-decoration: underline"></asp:Label>
                                </td>
                                <td class="tdLabel" align="left" style="width: 25%; padding-left: 10px;">
                                    <asp:Label ID="lblRoleSelections1" runat="server" Text="" Style="text-decoration: underline"></asp:Label>
                                </td>
                                <td class="tdLabel" align="left" style="width: 25%; padding-left: 10px;">
                                    <asp:Label ID="lblRoleSelections2" runat="server" Text="" Style="text-decoration: underline"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 13%; padding-left: 10px; vertical-align: top">
                                    <asp:CheckBox ID="ChkRegion" runat="server" onClick="checkAll(this,'Region');" Text=""
                                        OnCheckedChanged="ChkRegion_CheckedChanged" AutoPostBack="True" />
                                    <asp:Label ID="lblRegion" runat="server" Text="Select All"></asp:Label>
                                    <div class="checkbox checkbox-primary">
                                        <asp:CheckBoxList ID="lstRegion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstRegion_SelectedIndexChanged"
                                            onClick="checkListAll(this,'Region');">
                                        </asp:CheckBoxList>
                                    </div>
                                    <asp:Label ID="lblRegionID" runat="server" Visible="False"></asp:Label>
                                </td>
                                <td class="tdLabel" style="width: 12%; vertical-align: top; padding-left: 10px;">
                                    <asp:CheckBox ID="ChkCountryOrState" runat="server" onClick="checkAll(this,'CountryOrState');"
                                        Text="" OnCheckedChanged="ChkCountryOrState_CheckedChanged" AutoPostBack="True" />
                                    <asp:Label ID="lblCountryOrSate" runat="server" Text="Select All"></asp:Label>
                                    <div style="width: 170px; height: 200px; overflow: scroll;">
                                        <div class="checkbox checkbox-primary">
                                            <asp:CheckBoxList ID="lstCountryOrState" runat="server" OnSelectedIndexChanged="lstCountryOrState_SelectedIndexChanged"
                                                onClick="checkListAll(this,'CountryOrState');" AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </td>
                                <td class="tdLabel" style="display: none; vertical-align: top; padding-left: 10px;">
                                    <div class="checkbox checkbox-primary">
                                        <asp:CheckBoxList ID="lstDepartments" runat="server" OnSelectedIndexChanged="lstDepartments_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                                <td class="tdLabel" style="width: 45%; vertical-align: top; padding-left: 10px;">
                                    <asp:CheckBox ID="ChkRoleSelection" runat="server" onClick="checkAll(this,'RoleSelection');"
                                        Text="" Enabled="false" />
                                    <asp:Label ID="lblRoleSelection" runat="server" Text="Select All"></asp:Label>
                                    <div style="width: 95%; height: 200px; overflow: scroll; border-width: 1px;" class="checkbox checkbox-primary">
                                        <asp:CheckBoxList ID="lstRoleSelection" runat="server" onClick="checkListAll(this,'RoleSelection');" Enabled="false">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                                <td class="tdLabel" style="width: 15%; vertical-align: top; padding-left: 10px;">
                                    <asp:CheckBox ID="ChkRoleSelection1" runat="server" onClick="checkAll(this,'RoleSelection1');"
                                        Text="" Enabled="false" />
                                    <asp:Label ID="lblRoleSelection1" runat="server" Text="Select All"></asp:Label>
                                    <div style="width: 95%; height: 200px; overflow: scroll; border-width: 1px" class="checkbox checkbox-primary HideControl">
                                        <asp:CheckBoxList ID="lstRoleSelection1" runat="server" onClick="checkListAll(this,'RoleSelection1');" Enabled="false">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                                <td class="tdLabel" style="width: 15%; vertical-align: top; padding-left: 10px;">
                                    <asp:CheckBox ID="ChkRoleSelection2" runat="server" onClick="checkAll(this,'RoleSelection2');"
                                        Text="" Enabled="false" />
                                    <asp:Label ID="lblRoleSelection2" runat="server" Text="Select All"></asp:Label>
                                    <div style="width: 95%; height: 200px; overflow: scroll; border-width: 1px" class="checkbox checkbox-primary HideControl">
                                        <asp:CheckBoxList ID="lstRoleSelection2" runat="server" onClick="checkListAll(this,'RoleSelection2');" Enabled="false">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table2" runat="server" class="ContainTable">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" style="height: 15px" colspan="4">Permissions
                                </td>
                            </tr>
                            <tr class="well well-sm">
                                <td class="tdLabel">&nbsp;
                                <asp:Label ID="lblPermissions0" runat="server">Main Menu</asp:Label>
                                </td>
                                <td class="tdLabel"><%--Permissions--%>
                                </td>
                                <td class="tdLabel">&nbsp;
                                </td>
                                <td class="tdLabel">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">&nbsp;
                                <div class="DivTree" style="height: 280px">
                                    <asp:TreeView ID="TreePermission" runat="server" AutoGenerateDataBindings="False"
                                        Font-Names="Verdana" Font-Size="X-Small" ForeColor="#156FC4" Height="97px" LineImagesFolder="~/Images/TreeLineImages"
                                        ShowCheckBoxes="All" ShowLines="True" Style="direction: ltr; border-top-style: none; border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none; font-family: verdana, sans-serif;"
                                        Width="244px" onclick=" return client_OnTreeNodeChecked(event);">
                                        <ParentNodeStyle CssClass="normal" />
                                        <LeafNodeStyle CssClass="normalLeaf" />
                                        <HoverNodeStyle CssClass="hover" />
                                        <RootNodeStyle CssClass="normal" />
                                        <SelectedNodeStyle CssClass="hover" />
                                    </asp:TreeView>
                                </div>
                                </td>
                                <td class="tdLabel HideControl" colspan="2" >
                                    <div id="DivSlabSelect" runat="server">
                                        &nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Show"
                                            CssClass="CommandButton btn btn-primary btn-sm" />
                                        &nbsp;&nbsp;&nbsp;<asp:Label ID="lblclmslab" runat="server" Text="Select Claim Slab: "></asp:Label>
                                        &nbsp;<asp:DropDownList ID="ddlClaimSlab" CssClass="ComboBoxFixedSize" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <br />
                                    <div class="DivTree">
                                        <div id="gridView" class="grid">
                                            <asp:GridView ID="gridPermission" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="odd"
                                                AutoGenerateColumns="true" CellPadding="0" CellSpacing="0" CssClass="datatable table table-bordered"
                                                GridLines="None" RowStyle-CssClass="even" Style="border-collapse: separate;">
                                                <FooterStyle CssClass="GridViewFooterStyle" />
                                                <RowStyle CssClass="GridViewRowStyle" />
                                                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                                <PagerStyle CssClass="GridViewPagerStyle" />
                                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr&#160;No." SortExpression="LoginName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    &nbsp;
                                <br />
                                </td>
                                <td class="tdLabel">&nbsp;
                                <br />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnSelRegion" runat="server" Value="" />
        <asp:HiddenField ID="hdnSelStateorCountry" runat="server" Value="" />
        <asp:HiddenField ID="hdnHD" runat="server" Value="" />
        <asp:HiddenField ID="hdnLMD" runat="server" Value="" />
        <asp:HiddenField ID="hdnBUS" runat="server" Value="" />
        <asp:HiddenField ID="hdnUserRole" runat="server" Value="" />
        <asp:HiddenField ID="hdnUserRoleDatabase" runat="server" Value="" />
        <asp:HiddenField ID="hdnDefault" runat="server" Value="" />
        <asp:HiddenField ID="hdnHDDatabase" runat="server" Value="" />
        <asp:HiddenField ID="hdnLMDDatabase" runat="server" Value="" />
        <asp:HiddenField ID="hdnBUSDatabase" runat="server" Value="" />
        <asp:HiddenField ID="hdnDefaultDatabase" runat="server" Value="" />
        <asp:HiddenField ID="hdnLinked" runat="server" Value="" />                
        <asp:HiddenField ID="hdnDeptdatabase" runat="server" Value="" />
    </form>
</body>
</html>
