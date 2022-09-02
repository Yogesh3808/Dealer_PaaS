
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
    if (document.getElementById("txtLastName").value != '') {
        for (var i = 0; i < document.getElementById("txtLastName").value.length; i++) {
            if (document.getElementById("txtLastName").value.charAt(0) == '') {
                alert("Last Name: Please do not enter space(s).");
                document.getElementById("txtLastName").focus();
                return false;
            }
        }
    }
    else {
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

function client_OnTreeNodeChecked() {
    var e = window.event || e;
    var obj = e.target || e.srcElement;
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


