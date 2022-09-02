var PcontainerName = null;
var bCheckAll = null;
//
function GetContainerName() {
    return PcontainerName;
}
function SetContainerName(ContainerName) {
    PcontainerName = ContainerName;
}

//Check When key is Pressed in textbox
function CheckForTextBoxValue(eve, objcontrol, TextboxType) {
    //debugger;
    eve = (eve) ? eve : window.event;
    var charCode = (eve.which) ? eve.which : eve.keyCode;
    //check for string textbox
    var txtOfControl;
    if (charCode == 0) return;
    if (objcontrol.readOnly == true) {
        return;
    }
    if (TextboxType == 1) {
        // novalidation                
    }
        //check for Lowercase string textbox
    else if (TextboxType == 2) {
        if (charCode >= 65 && charCode <= 90) {
            ShowMessage(1);
            eve.keyCode = 0;
            //document.all(id).focus();
            return false
        }
    }
        //check for Uppercase textbox
    else if (TextboxType == 3) {
        if (charCode >= 97 && charCode <= 122) {
            ShowMessage(2);
            eve.keyCode = 0;
            //document.all(id).focus();
            return false
        }
    }
        //properCase
    else if (TextboxType == 4) {
        txtOfControl = objcontrol.value;
        if (txtOfControl.length != 0) {
            var Previouscharcode = txtOfControl.charCodeAt(0);
            for (i = 0; i < txtOfControl.length; i++) {
                charCode = txtOfControl.charCodeAt(i);
                Previouscharcode = charCode;
            }
            if (Previouscharcode == 32) {
                eve.keyCode = eve.keyCode - 32;
            }
        }
        else {
            var capsLockON;
            KC = eve.keyCode ? eve.keyCode : eve.which;
            SK = eve.shiftKey ? eve.shiftKey : ((KC == 16) ? true : false);
            if (((KC >= 65 && KC <= 90) && !SK) || ((KC >= 97 && KC <= 122) && SK)) {
                capsLockON = true;
            }
            else if (SK == true) {
                capsLockON = true;
            }
            else {
                capsLockON = false;
            }
            if (capsLockON != true) {
                eve.keyCode = eve.keyCode - 32;
            }

        }
    }
        //numeric
    else if (TextboxType == 5) {
        var sValue = objcontrol.value
        var iMaxLength = sValue.length;

        if (iMaxLength > 6) {
            eve.keyCode = 0;
            return false;
        }
        return CheckForNumerric(eve, charCode, true);
    }
        //numeric with decimal
    else if (TextboxType == 6) {
        // check max length of the textbox for float/amount  type of texbox            
        var sValue = objcontrol.value
        if (sValue.indexOf(".") == -1) {
            if (charCode == 46)//if . is entered 
            {
                if (sValue.length > 9) {
                    eve.keyCode = 0;
                    return false;
                }
            }
            else {
                if (sValue.length >= 9) {
                    eve.keyCode = 0;
                    return false;
                }
            }
        }
        else {
            var sValueBefDot = "", sValueAftDot = "";
            sValueBefDot = sValue.substring(0, (sValue.indexOf(".")));
            sValueAftDot = sValue.substring(sValue.indexOf("."));

            if (sValueBefDot.length > 9) {

                if (sValueAftDot.length > 2) {
                    eve.keyCode = 0;
                    return false;
                }
                if (sValueAftDot.length > 2) {
                    eve.keyCode = 0;
                    return false;
                }
                eve.keyCode = 0;
                return false;
            }
            else {

            }
        }
        if (charCode == 46) {
            txtOfControl = objcontrol.value;
            if (txtOfControl.length == 0) {
                alert("Enter Numeric Value Before '.'");
                objcontrol.value = '';
                event.returnValue = false;
                return false;
            }
            else {
                for (i = 0; i < txtOfControl.length; i++) {
                    charCode = txtOfControl.charCodeAt(i);
                    if (charCode == 46) {
                        alert("'.' is already entered.");
                        event.returnValue = false
                        return false;
                    }
                }
            }
        }
        else {
            return CheckForNumerric(eve, charCode, false);
        }
    }
        //email
    else if (TextboxType == 7) {
        if (charCode == 64) {
            txtOfControl = objcontrol.value;
            if (txtOfControl.length == 0) {
                alert("Enter Text Before '@'");
                objcontrol.value = '';
                event.returnValue = false
                return false;
            }
            else {
                for (i = 0; i < txtOfControl.length; i++) {
                    charCode = txtOfControl.charCodeAt(i);
                    if (charCode == 64) {
                        alert("'@' is already entered.");
                        event.returnValue = false
                        return false;
                    }
                }
            }
        }

    }
        //Phoneno
    else if (TextboxType == 8) {
        return CheckForNumerric(eve, charCode, true);
    }
        //MobileNo
    else if (TextboxType == 9) {
        return CheckForNumerric(eve, charCode, true);
    }
        //FaxNo
    else if (TextboxType == 10) {
        return CheckForNumerric(eve, charCode, true);
    }
        //VehicleNo
    else if (TextboxType == 11) {
        if (charCode == 32) {
            alert("Space Not Allowed.");
            eve.keyCode = 0;
            return false;
        }
        else {
            txtOfControl = objcontrol.value;
            if (txtOfControl.length > 3)
                for (i = 5; i < txtOfControl.length; i++) {
                    charCode = txtOfControl.charCodeAt(i);
                    if (CheckForNumerric(eve, charCode, true) == false) {
                        return;
                    }
                }
        }

    }
        //PincCode
    else if (TextboxType == 12) {
        return CheckForNumerric(eve, charCode, true);
    }
        //Alphanumeric
    else if (TextboxType == 13) {
        //if (charCode > 32 && charCode < 47)
        //Megha06042011
        if ((charCode > 47 && charCode < 58) || (charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
            return true;
        }
        else {
            alert("Please Enter AlphaNumeric value.");
            eve.keyCode = 0;

            return false;
        }
        return true;

    }
        //charCode = 32 //space
    else if (TextboxType == 15) {
        //if (charCode > 32 && charCode < 47)
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
            return true;
        }
        else {
            alert("Please Enter Alphabets value.");
            eve.keyCode = 0;

            return false;
        }
        return true;

    }
        //Megha06042011
        //memo
    else if (TextboxType == 14) {
        // no validation
    }
}

function CheckForNumerric(eve, charCode, CheckDecimal) {
    ////debugger;
    if (charCode == 46) {
        if (CheckDecimal == true) {
            alert("Decimal not Allowed.");
            eve.keyCode = 0;
            return false;
        }
    }
    else if (charCode > 47 && charCode < 58 || charCode == 9) {
        return true;
    }
    else {
        alert("Please Enter numeric value.");
        eve.returnValue = false;
        //alert(eve.keyCode);
        eve.keyCode = 0;
        return false;
    }
    return true;
}
// check For Numeric Value
function CheckForNumericForKeyPress(eve, id) {
    var charCode = (eve.which) ? eve.which : event.keyCode;
    //////alert(charCode);    
    if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46) {
        alert("Please Enter numeric value.");
        event.keyCode = 0;
        //document.all(id).focus();
        return false
    }

    return true
}
// Check only Number 0 to 9  VIkramK 10.11.2016
function CheckisNumber(event) {
    ////debugger;
    event = (event) ? event : window.event;
    var charCode = (event.which) ? event.which : event.keyCode;
    //if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39) {
    //    return true;
    //}
    //else if (key < 48 || key > 57) {
    //    return false;
    //}
    //else return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
// allow only 2 decimal point in textbox VikramK on 10.11.2016
function checkDecimal(event) {
  
    var ex = /^[0-9]+\.?[0-9]*$/;
  
    if (ex.test(event.value) == false) {
        event.value = event.value.substring(0, event.value.length - 1);
    }
}
  function validateFloatKeyPress(el, evt) {

           var charCode = (evt.which) ? evt.which : event.keyCode;
           var number = el.value.split('.');
           if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
               return false;
           }
           //just one dot (thanks ddlab)
           if (number.length > 1 && charCode == 46) {
               return false;
           }
           //get the carat position
           var caratPos = getSelectionStart(el);
           function getSelectionStart(o) {
               if (o.createTextRange) {
                   var r = document.selection.createRange().duplicate()
                   r.moveEnd('character', o.value.length)
                   if (r.text == '') return o.value.length
                   return o.value.lastIndexOf(r.text)
               } else return o.selectionStart
           }
           var dotPos = el.value.indexOf(".");
           if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
               return false;
           }
           return true;
       }

// Check On Lost focus Of Textbox
function CheckForTextBoxLostFocus(eve, objcontrol, TextboxType) {
    //debugger;
    //        var charCode = (eve.which) ? eve.which : event.keyCode;               
    //        //check for string textbox
    //        if (charCode == 0)return;
    if (TextboxType == 1) {
        // novalidation                
    }
        //check for Lowercase string textbox
    else if (TextboxType == 2) {
        // novalidation                
    }
        //check for Uppercase textbox 
    else if (TextboxType == 3) {
        // novalidation                
    }
        //properCase
    else if (TextboxType == 4) {

    }
        //numeric
    else if (TextboxType == 5) {
        var i, charCode;
        sValue = objcontrol.value;
        for (i = 0; i < sValue.length; i++) {
            charCode = sValue.charCodeAt(i);

            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46) {
                objcontrol.value = '';
                event.returnValue = false
                return false;
            }
        }
        return true;
    }
        //numeric with decimal
    else if (TextboxType == 6) {
        // roundup

    }
        //email
    else if (TextboxType == 7) {
        // novalidation                
    }
        //Phoneno
    else if (TextboxType == 8) {
        // novalidation                
    }
        //MobileNo
    else if (TextboxType == 9) {
        // novalidation                
    }
        //FaxNo
    else if (TextboxType == 10) {
        // novalidation                
    }
        //VehicleNo
    else if (TextboxType == 11) {
        // novalidation                
    }
        //PincCode
    else if (TextboxType == 12) {
        // novalidation                
    }
        //Alphanumeric
    else if (TextboxType == 13) {
        // novalidation                
    }
        //memo
    else if (TextboxType == 14) {
        // no validation
    }
}


function CheckForNumericForPaste(eve, id) {
    var charCode = (eve.which) ? eve.which : event.keyCode;
    var sValue = window.clipboardData.getData('Text');

    var i;
    for (i = 0; i < sValue.length; i++) {
        charCode = sValue.charCodeAt(i);

        if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46) {
            alert("In Paste Data Alphabatic Character Exist.");
            document.getElementById(id).value = '';
            event.returnValue = false
            return false;
        }
    }
    return true;
}

function CheckForComboValue(eve, objcontrol, bShowMessage) {
    if (objcontrol.selectedIndex == 0) {
        if (bShowMessage == null) bShowMessage = true;
        if (bShowMessage == true) {
            alert("Please Select Record");
        }
        //objcontrol.focus();
        return false;
    }
    else {
        return true;
    }
}


function CheckForDateValue(eve, objcontrol, DateType, bcheckforCurrentDate) {
    if (eve == null) return;
    //var obj = document.getElementById("<%=txtDate.ClientID%>");
    if (eve.type == "keypress") {
        objcontrol.valueOf = "";
        event.keyCode = 0;
        return false;
    }
    if (objcontrol.value == "") {
        return;
    }
    var day = objcontrol.value.split("/")[0];

    var month = objcontrol.value.split("/")[1];
    var year = objcontrol.value.split("/")[2];

    if ((day < 1 || day > 31) || (month < 1 && month > 12) && (year.length != 4)) {
        alert("Invalid Format");
        objcontrol.value = "";
        event.keyCode = 0;
        return false;
    }
    else {
        if (bcheckforCurrentDate == true) {
            CheckDateForCurrentDate(objcontrol);
        }

    }
}
function CheckDateForCurrentDate(objcontrol) {
    var currdate = new Date();
    currdate = currdate.format("dd/MM/yyyy")
    if (objcontrol.value != currdate) {
        event.keyCode = 0;
        alert("Plese Select the Current Date ");
        objcontrol.focus();
        objcontrol.value = "";
        return false;
    }
}

// to Check Date is Grater Than or equal to Current Date
function CheckDateGreaterThanOrEqualToCurrentDate(ObjControl) {
    var ObjControl;
    //ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (ObjControl.value == "") return;
    var currdate = new Date();
    currdate = currdate.format("dd/MM/yyyy")
    if (ObjControl.value <= currdate) {
        event.keyCode = 0;
        alert("Please Select Greater Than Or Equal To the Current Date ");
        ObjControl.value = "";
        ObjControl.focus();
        return false;
    }
}
//ChekcFrom Date is Less than Todate
function CheckFromDateValidation(ObjFromDate, ToDateID, bCheckForCurrentDate) {
    if (ObjFromDate.value == "") {
        return;
    }
    var ObjToDate = document.getElementById("ContentPlaceHolder1_" + ToDateID + "_txtDocDate");


    CheckForDateValue(null, ObjFromDate, null, bCheckForCurrentDate);

    if (ObjToDate == null) return;
    if (ObjToDate.value == "") {
        return;
    }
    if (bCheckFirstDateIsGreaterThanSecondDate(ObjFromDate.value, ObjToDate.value) == true) {
        ObjFromDate.value = "";
        alert('From date should be less than to date');
        return false;
    }
}
//Check ToDate is greater than FromDate
function CheckToDateValidation(ObjToDate, FromDateID) {
    if (ObjToDate.value == "") {
        return;
    }
    CheckForDateValue(null, ObjToDate, null, false);

    var ObjFromDate = document.getElementById("ContentPlaceHolder1_" + FromDateID + "_txtDocDate");
    if (ObjFromDate == null) return;
    if (ObjFromDate.value == "") {
        return;
    }
    if (bCheckFirstDateIsGreaterThanSecondDate(ObjFromDate.value, ObjToDate.value) == true) {
        ObjToDate.value = "";
        alert('To date should be greater than from date');
    }

}

//compare First Date with Second Date return false if first date is greater than SecondDate
function bCheckFirstDateIsGreaterThanSecondDate(sFirstDate, sSecondDate) {

    if (sFirstDate == '' || sSecondDate == '') {

        return false;
    }

    var sTmpValue = sFirstDate;
    var day = sTmpValue.split("/")[0];
    var month = dGetValue(sTmpValue.split("/")[1]);
    var year = sTmpValue.split("/")[2];

    //Set sFromDate
    var sFromDate = new Date();
    sFromDate.setFullYear(year, month, day);

    //Set To Date
    var sToDate = new Date();
    sTmpValue = sSecondDate;
    day = sTmpValue.split("/")[0];
    month = dGetValue(sTmpValue.split("/")[1]);
    year = sTmpValue.split("/")[2];
    sToDate.setFullYear(year, month, day);

    if (sFromDate > sToDate) {
        return true;
    }
    return false;
}
//Megha22/04/2011
// to Check Date is Grater Than or equal to Current Date
function bCheckInputDateIsGreaterThanorEqualToCurrentDate(ObjControl) {

    if (ObjControl.value == "") return;

    var sTmpValue = ObjControl.value;
    var day = sTmpValue.split("/")[0];
    var month = dGetValue(sTmpValue.split("/")[1]);
    var year = sTmpValue.split("/")[2];


    //Set sFromDate
    var sFromDate = new Date();
    sFromDate.setFullYear(year, month, day);

    //Set To Date
    var sCurrdate = new Date();
    sCurrdate = sCurrdate.format("dd/MM/yyyy")

    var sToDate = new Date();
    sTmpValue = sCurrdate;
    day = sTmpValue.split("/")[0];
    month = dGetValue(sTmpValue.split("/")[1]);
    year = sTmpValue.split("/")[2];
    sToDate.setFullYear(year, month, day);

    if (sFromDate < sToDate) {
        event.keyCode = 0;
        alert("Please Select Greater Than Or Equal To the Current Date ");
        ObjControl.value = "";
        ObjControl.focus();
        return false;
    }
    return true;
}
function bCheckFromDateIsGreaterThanToCurrentDate(ObjControl) {

    if (ObjControl.value == "") return;

    var sTmpValue = ObjControl.value;
    var day = sTmpValue.split("/")[0];
    var month = dGetValue(sTmpValue.split("/")[1]);
    var year = sTmpValue.split("/")[2];


    //Set sFromDate
    var sFromDate = new Date();
    sFromDate.setFullYear(year, month, day);

    //Set To Date
    var sCurrdate = new Date();
    sCurrdate = sCurrdate.format("dd/MM/yyyy")

    var sToDate = new Date();
    sTmpValue = sCurrdate;
    day = sTmpValue.split("/")[0];
    month = dGetValue(sTmpValue.split("/")[1]);
    year = sTmpValue.split("/")[2];
    sToDate.setFullYear(year, month, day);

    if (sFromDate <= sToDate) {
        event.keyCode = 0;
        alert("Please Select From Date Greater Than the Current Date ");
        ObjControl.value = "";
        ObjControl.focus();
        return false;
    }
    return true;
}

//Megha22/04/2011

// Check Textbox value is empty for text field
function CheckTextboxValueForString(event, ObjControl, bSetFocus) {
    if (ObjControl.readOnly == true) return true;
    if (ObjControl.value == "") {
        alert("Please enter the record");
        event.returnValue = false
        if (bSetFocus == true) {
            ObjControl.focus();
        }
        return false;
    }
}

//check textbox value is empty or 0 for Qty,Rate ,Amount i.e numerical field
function CheckTextboxValueForNumeric(event, ObjControl, bSetFocus, bShowMessage) {
    if (ObjControl.readOnly == true) return true;
    if (ObjControl.value == "") {
        if (bShowMessage == undefined || bShowMessage == null) bShowMessage = true;
        if (bShowMessage == true) {
            alert("Please enter the value");
        }
        event.returnValue = false;
        if (bSetFocus == true) {
            ObjControl.focus();
        }
        return false;
    }
    else if (dGetValue(ObjControl.value) < 0) {
        ObjControl.value = '';
        if (bShowMessage == undefined || bShowMessage == null) bShowMessage = true;
        if (bShowMessage == true) {
            alert("Please enter the value greater than '0'.");
            event.returnValue = false;
            if (bSetFocus == true) {
                ObjControl.focus();
            }
        }
        return false;
    }
}
//Function used to hide control which are not ot show but required for working  
function HideControl() {
    var ObjControl = document.getElementById(PcontainerName + 'txtID');
    PcontainerName = GetContainerName();
    if (ObjControl != null) {
        ObjControl.style.display = "none";
    }
    ObjControl = document.getElementById(PcontainerName + 'txtControlCount');
    if (ObjControl != null) {
        ObjControl.style.display = "none";
    }
    ObjControl = document.getElementById(PcontainerName + 'drpParent');
    if (ObjControl != null) {
        ObjControl.style.visibility = "hidden";
    }
    ObjControl = document.getElementById(PcontainerName + 'drpTarget');
    if (ObjControl != null) {
        ObjControl.style.visibility = "hidden";
    }
    ObjControl = document.getElementById(PcontainerName + 'txtFormType');
    if (ObjControl != null) {
        ObjControl.style.visibility = "hidden";
    }
    ObjControl = document.getElementById(PcontainerName + 'txtPreviousDocId');
    if (ObjControl != null) {
        ObjControl.style.visibility = "hidden";
    }

}
function disableBackButton() {
    window.history.forward();
}

// when user mouse over ion cancel label
function SetCancelStyleonMouseOver(ObjLabel) {
    ObjLabel.style.cursor = 'pointer'
    //ObjLabel.style.color = '#fff';
    //ObjLabel.style.color = '#fff';       
}

function SetCancelStyleOnMouseOut(ObjLabel) {
    ObjLabel.style.cursor = 'pointer'
    //ObjLabel.style.color = '#fff';
    //ObjLabel.style.color = '#fff';       
}



// Get Value into float
function dGetValue(svalue) {
    if (svalue == "") {
        return 0;
    }
    else {
        return parseFloat(svalue);
    }
}


//MAKING AMOUNT DECIMAL PLACES
function RoundupValue(num) {
    num = roundNumber(num);
    var strnum = num.toString();
    var isfloat = (strnum.indexOf('.'))

    if (isfloat != -1) {
        var splitArray = strnum.split('.');
        var afterDeci
        var newStr
        var newnum

        afterDeci = splitArray[1];

        var strAfterDeci
        strAfterDeci = afterDeci.toString();

        var str2Deci = strAfterDeci.substring(0, 2);
        if (str2Deci.length == 1) {
            str2Deci = str2Deci + "0";
        }
        newStr = splitArray[0] + "." + str2Deci;
        return newStr;
    }
    else {
        newStr = strnum + "." + "00";
        return newStr;
    }

}
function roundNumber(dAmount) {
    var result = Math.round(dAmount * Math.pow(10, 2)) / Math.pow(10, 2);
    return result;
}
// On keypress  of the Control 
function CheckPercentageAmount(eve, ObjControl) {
    if (CheckForTextBoxValue(eve, ObjControl, '6') == true) {
        var ValuebeforeDecimal = 0;
        var sValue = ObjControl.value;
        if (sValue.indexOf(".") != -1) {
            sValue = sValue.substring(0, sValue.indexOf("."));
            return true;
        }
        if (sValue.length == 1) {
            if (parseFloat(sValue) <= 100) return true;
        }
        else if (sValue.length == 2) {
            if (eve.keyCode == 48) return true;
            if (eve.keyCode == 46) return true;
            alert("Percent value should not be greater than 100 !");
            eve.keyCode = 0;
            return false;
        }
        else if (sValue.length > 2) {
            alert("Percent value should not be greater than 100 !");
            eve.keyCode = 0;
            return false;
        }

    }
}
// On blur (Lost focus) of the Control 
function CheckPercentageValue(event, ObjControl) {
    var dValue = dGetValue(ObjControl.value)
    if (dValue > 100) {
        alert("Percent value should not be greater than 100 !");
        ObjControl.focus();
        event.returnValue = false
        return false;
    }
    else {
        ObjControl.value = RoundupValue(dValue);
    }
}

///############Following Functions are for multiselect DropDown
function SHMulSel(ControlClientID, e) {
    debugger;
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        if (window.document.getElementById(ControlClientID + '_' + 'hapb').value == 'True') {
            document.getElementById(ControlClientID + '_' + '__EVENTTARGET1').value = 'MultiSelectDropDown';
            document.getElementById(ControlClientID + '_' + '__EVENTARGUMENT1').value = textBoxMain.value;
            __doPostBack('MultiSelectDropDown', window.document.getElementById(ControlClientID + '_' + 'txtDealerName').value);
        }
    }
    else {
        divisionMain.style.display = 'block';
        divisionMain.className = 'dvmain'
    }
    var evt = (window.event == null) ? e : window.event;
    evt.cancelBubble = true;
}

function SHMulSel01(ControlClientID, divMainWidth, divMainHeight, e) {
    debugger;
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        return false;
    }
    else {
        var ChkDealer = document.getElementById(ControlClientID + "_" + "ChkDealer");
        //        divisionMain.style.width = divMainWidth + 'px';
        //        divisionMain.style.height = divMainHeight + 'px';
        if (ChkDealer == null) {
            divisionMain.style.display = 'none';
            return false;
        }
        else {
            divisionMain.style.display = 'block';
            return true;
        }
    }
}

function openthebox(ControlClientID, divMainWidth, divMainHeight) {
    //    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');
    //    divisionMain.style.display = 'block';
    //    divisionMain.style.width = divMainWidth + 'px';
    //    divisionMain.style.height = divMainHeight + 'px';
}

function DisplayTitle(control) {
    control.title = control.value;
}

function SCIT(chkbox, ControlClientID) {
    debugger;
    var labelCollection = window.document.getElementsByTagName('label');
    var hSelectedItemsValueList = document.getElementById(ControlClientID + '_' + 'hsiv');
    var textBoxCurrentValue = new String();
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    var selectedText;
    var selectedValue;

    textBoxCurrentValue = textBoxMain.value;
    if (chkbox.nextSibling != null) {
        selectedText = chkbox.nextSibling.innerText;
    }
    var pElement = chkbox.parentElement == null ? chkbox.parentNode : chkbox.parentElement;
    //    selectedValue = pElement.attributes["alt"].value

    if (chkbox.checked) {
        textBoxCurrentValue = selectedText + ', ';

        if (textBoxMain.value == '--Select--')
            textBoxMain.value = textBoxCurrentValue;
        else
            textBoxMain.value += textBoxCurrentValue;

        hSelectedItemsValueList.value = hSelectedItemsValueList.value + selectedValue + ', ';
    }
    else {
        textBoxCurrentValue = textBoxCurrentValue.replace(selectedText + ', ', "");

        if (textBoxCurrentValue == '')
            textBoxMain.value = '--Select--';
        else
            textBoxMain.value = textBoxCurrentValue;

        hSelectedItemsValueList.value = hSelectedItemsValueList.value.replace(selectedValue + ', ', "");
    }

}
function SelectAllMail() {
    //<%=this.ClientID%>
    var ObjControl_ID = document.getElementById('ContentPlaceHolder1_Page');
    ControlClientID = ObjControl_ID.value;
    var tblCbl = document.getElementById(ControlClientID + '_ChkDealer');
    if (tblCbl == null) return;
    var tblBody = tblCbl.childNodes[0];
    var counter = tblBody.childNodes.length;
    if (bCheckAll == null) bCheckAll = true;
    else if (bCheckAll == true) bCheckAll = false;
    else if (bCheckAll == false) bCheckAll = true;
    for (index = 0; index < counter; index++) {
        var tr = tblBody.childNodes[index];
        if (tr.childNodes[0].childNodes[0].childNodes[0] == null) {
            var checkbox = tr.childNodes[0].childNodes[0];
        }
        else {
            var checkbox = tr.childNodes[0].childNodes[0].childNodes[0];
        }
        if (checkbox == null) return;
        if (bCheckAll == true) {
            checkbox.checked = true;
        }
        else {
            checkbox.checked = false;
        }
        //        if (checkbox.checked)
        //            checkbox.checked = false;
        //        else
        //            checkbox.checked = true;

        SCIT(checkbox, ControlClientID);
    }
}
function SelectAllNew() {
    var bSelectAllChk = false;
    var ObjControl_ID = document.getElementById('ContentPlaceHolder1_Location_txtControl_ID');
    ControlClientID = ObjControl_ID.value;
    var tblCbl = document.getElementById(ControlClientID + '_ChkDealer');
    if (tblCbl == null) return;
    var tblBody = tblCbl.childNodes[1];
    var counter = tblBody.childNodes.length - 1;
    for (index = 0; index < counter; index++) {
        var tr = tblBody.childNodes[index];
        if (tr.childNodes[1].childNodes[0].childNodes[0].checked == true) {
            bSelectAllChk = true;
        }
        else {
            bSelectAllChk = false;
        }
    }
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    if (bSelectAllChk == true) {
        if (textBoxMain != null) {
            textBoxMain.value = 'All Selected';
        }
    }
    
}
function SelectAll() {
    //<%=this.ClientID%>
    var ObjControl_ID = document.getElementById('ContentPlaceHolder1_Location_txtControl_ID');
    ControlClientID = ObjControl_ID.value;
    var tblCbl = document.getElementById(ControlClientID + '_ChkDealer');
    if (tblCbl == null) return;
    debugger;
    var tblBody = tblCbl.childNodes[1];
    var counter = tblBody.childNodes.length - 1;
    if (bCheckAll == null) bCheckAll = true;
    else if (bCheckAll == true) bCheckAll = false;
    else if (bCheckAll == false) bCheckAll = true;
    for (index = 0; index < counter; index++) {
        var tr = tblBody.childNodes[index];
        if (tr.childNodes[1].childNodes[0].childNodes[0] == null) {
            var checkbox = tr.childNodes[1].childNodes[0];
        }
        else {
            var checkbox = tr.childNodes[1].childNodes[0].childNodes[0];
        }
        if (checkbox == null) return;
        if (bCheckAll == true) {
            checkbox.checked = true;
        }
        else {
            checkbox.checked = false;
        }
        //        if (checkbox.checked)
        //            checkbox.checked = false;
        //        else
        //            checkbox.checked = true;

        SCIT(checkbox, ControlClientID);
    }
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    if (bCheckAll == true) {
        if (textBoxMain != null) {
            textBoxMain.value = 'All Selected';
        }
    }
    else if (bCheckAll == false) {

        textBoxMain.value = '--Select--';
    }

}
//###############

function SetControlReadOnly(objConotrol) {
    if (objConotrol == null) return;
    objConotrol.readOnly = true;
}


//To disallowed to user enter value in textbox
function ToSetKeyPressValueFalse(event, ObjControl) {
    if (event.keyCode != 9 && event.keyCode != 16 && event.keyCode != 17) {
        event.keyCode = 0;
        return false;
    }
}
//To Allowed to user enter value in textbox
function ToSetKeyPressValueTrue(event, ObjControl) {
    return true;
}

//To disallowed to user Paste the value in textbox
function ToSetPasteValueFalse(event, ObjControl) {
    ObjControl.value = '';
    event.keyCode = 0;
    return false;
}

function bCheckForBlank(ObjControl) {
    if (ObjControl == null) return true;
    if (ObjControl.value == "") {
        alert("Plese enter the record.");
        if (ObjControl.currentStyle.display == "") {
            ObjControl.focus();
        }
        return false;
    }
    return true;
}
// To Check Qty Should Not Be Greater than 100000
function CheckQtyValidation(event, ObjControl, bSetFocus) {
    if (CheckTextboxValueForNumeric(event, ObjControl, true) == false) {
        return false;
    }
    if (dGetValue(ObjControl.value) > 100000) {
        alert('Please enter the quantity less than 100000.');
        ObjControl.value = '';
        if (bSetFocus == true)
        { ObjControl.focus(); }
        return false;
    }
}
//To Check Records are entered or selected in a row
function CheckRowValue(event, objAddNewControl) {
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    var ObjCell;
    for (var i = 0; i < objRow.length; i++) {
        ObjCell = objRow[i].childNodes[0];
        if (objRow[i].style.display == "")
            if (ObjCell.type == "text") {
                if (ObjCell.isDisabled == false && ObjCell.style.display == "") {
                    if (ObjCell.value == "") {
                        alert("Please enter the record");
                        ObjCell.focus();
                        return false;
                    }
                }
            }
            else if (ObjCell.type == "textarea") {
                if (ObjCell.isDisabled == false) {
                    if (ObjCell.value == "") {
                        alert("Please enter the record");
                        ObjCell.focus();
                        return false;
                    }
                }
            }
            else if (ObjCell.type == "select-one") {
                if (ObjCell.value == "0" && ObjCell.isDisabled == false) {
                    alert("Please select the record");
                    ObjCell.focus();
                    return false;
                }
            }

    }
} // when user select check box To Delete The Record 
function SelectDeletCheckbox(ObjChkDelete) {
    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
        }
        else {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
    }
}
// To Check Dealer Is Selected or not
function CheckDealerIsSelected(ObjControl) {
    var objDealerCombo = document.getElementById('ContentPlaceHolder1_Location_drpDealerName');
    if (objDealerCombo == null) return;

    if (objDealerCombo.selectedIndex == 0) {
        alert('Please first select the dealer');
        ObjControl.selectedIndex = 0;
        return false;
    }
}


// To Get Part Id which are previously selected by user.
function GetPreviousSelectedPartID(objNewPartLabel) {
    ////debugger;
    var objRow;
    var PartIds = "";
    var PartId = "";
    var lblPartId;
    // get grid object
    var objID = $('#' + objNewPartLabel.id)

    var objGrid = objID[0].parentNode.parentNode.parentNode; //objNewPartLabel.parentNode.parentNode.parentNode.parentNode;

    if (objGrid == null) return PartIds;

    for (var i = 1; i <= objGrid.rows.length - 1; i++) {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Lable of Part ID
        lblPartId = objGrid.children[i].cells[1].children[0]; //objGrid.rows[i].children[1].childNodes[0];       
        //Get PartId;
        PartId = lblPartId.value
        if (PartId != "0" && PartId != "" && PartId != null) {
            PartIds = PartIds + PartId + ",";
        }
    }
    PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

    return PartIds;
}

/// Array Function
// to Check Value Exist in Array
function bCheclValueExistInArray(arr, sValue) {
    var bExist = false;
    for (iIndex = 0; iIndex < arr.length; iIndex++) {
        if (arr[iIndex] == sValue) {
            bExist = true;
        }
    }
    return bExist;
}
//Get Index value from Array
function iGetIndexOfValueFromArray(arr, sValue) {
    var iIndex = 0;
    for (m = 0; m < arr.length; m++) {

        if (arr[m] == sValue) {
            iIndex = m;
        }
    }
    return iIndex;
}

function iGetIndexOfValueFromArrayJob(arr, sValue) {
    var iIndex = false;
    for (m = 0; m < arr.length; m++) {

        if (arr[m] == sValue) {
            iIndex = true;
        }
    }
    return iIndex;
}

//Sujata 14012011
//For Vehicle RFP, User shoul not allowed allowed CBU/ SKD /CKD
function CheckForComboValueForType(eve, objcontrol, bShowMessage) {

    if (CheckForComboValue(eve, objcontrol, bShowMessage) == false) {
        return false;
    }

    var objGrid = objcontrol.parentNode.parentNode.parentNode.parentNode;
    if (objGrid == null) return PartIds;
    var iCurrRowIndex = objcontrol.parentNode.parentNode.rowIndex;
    var sRFPType = objGrid.rows[1].cells[1].children[0].selectedIndex;

    if (sRFPType == 0 && iCurrRowIndex != 1) {
        alert("Please select Record at line No. 1");
        objcontrol.selectedIndex = 0;
        return false;
    }

    var sCurrentType = objGrid.rows[iCurrRowIndex].cells[1].children[0].selectedIndex;
    if (sRFPType != sCurrentType) {
        alert("Please select Same Type as line No. 1");
        objcontrol.selectedIndex = 0;
        return false;
    }
}
//Sujata 14012011


//function SHMulSelmail() {
//    var textBoxMain = window.document.getElementById( 'ContentPlaceHolder1_' + 'txtDealerName');
//    var divisionMain = window.document.getElementById( 'ContentPlaceHolder1_' + 'divMain');

//    var displayStatus = divisionMain.style.display;
//    if (displayStatus == 'block') {
//        divisionMain.style.display = 'none';
//        if (window.document.getElementById( 'ContentPlaceHolder1_' + 'hapb').value == 'True') {
//            document.getElementById( 'ContentPlaceHolder1_' + '__EVENTTARGET1').value = 'MultiSelectDropDown';
//            document.getElementById( 'ContentPlaceHolder1_' + '__EVENTARGUMENT1').value = textBoxMain.value;
//            __doPostBack('MultiSelectDropDown', window.document.getElementById(ControlClientID + '_' + 'txtDealerName').value);
//        }
//    }
//    else {
//        divisionMain.style.display = 'block';
//        divisionMain.className = 'dvmain'
//    }
//    var evt = (window.event == null) ? e : window.event;
//    evt.cancelBubble = true;
//}

function Calculate(eve, objcontrol) {

    var objRow = objcontrol.parentNode.parentNode.childNodes;

    var objtxtBalControl = objRow[5].childNodes[0];
    var objtxtBillControl = objRow[6].childNodes[0];

    var BalQty = dGetValue(objtxtBalControl.innerText);
    var BillQty = dGetValue(objtxtBillControl.value);

    var MOQ = dGetValue(objRow[3].childNodes[0].innerText);

    if (BalQty < BillQty) {
        alert("Please Enter the Bill Qty less than Balance Qty");
        objcontrol.focus();
        return false;
    }
    else {

        if (MOQ != 0 && (BillQty % MOQ) != 0) {
            if (BillQty / MOQ != 0) {
                objtxtBillControl.value = (parseInt(BillQty / MOQ) + 1) * MOQ > 0 ? (parseInt(BillQty / MOQ) + 1) * MOQ : 0;
                BillQty = dGetValue(objtxtBillControl.value);
            }
        }

        var objtxtRateControl = objRow[7].childNodes[0];
        var Rate = dGetValue(objtxtRateControl.innerText);

        var objtxtAmtControl = objRow[8].childNodes[0];
        objtxtAmtControl.innerText = RoundupValue(BillQty * Rate);
        CalulateGrandTotal();
    }
    return true;
};
function CalulateGrandTotal() {
    var txtTotalAmt = document.getElementById("ContentPlaceHolder1_txtTotalAmt");
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var qty, Rate;
    var TotalRate = 0;
    var totalQtypart = 0;
    var sPArtName = "";
    var dTotal = 0;
    var dDiscount = 0;
    var objtxtGrandTotalAmt;
    var CountRow = objGrid.rows.length;
    for (var i = 0; i < CountRow; i++) {
        if (objGrid.rows[i].className.indexOf('RowStyle') > 0) {
            qty = objGrid.rows[i].childNodes[6].childNodes[0].value
            Rate = objGrid.rows[i].childNodes[7].childNodes[0].innerText
            sPArtName = objGrid.rows[i].childNodes[2].childNodes[0].innerText
            if (sPArtName != "") {
                TotalRate = dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
                //totalQtypart = dGetValue(totalQtypart) + dGetValue(qty);
            }
        }
    }
    //txtTotalQty.value = totalQtypart;
    txtTotalAmt.value = parseFloat(TotalRate).toFixed(2);

    //    var txtIncoAmount = document.getElementById("ContentPlaceHolder1_txtIncoAmount");
    //    dTotal = parseFloat(TotalRate).toFixed(2);
    //    // To Get Discount
    //    dDiscount = dGetValue(txtIncoAmount.value);
    ////    if (dDiscount >= dTotal) {
    ////        alert("Discount is greater than or equal to the Total,Please reentered the discount !");
    ////        txtIncoAmount.value = 0.00;
    ////        dDiscount = 0;
    ////    }
    //    dTotal = dTotal - dDiscount;

    //    // To get Object of Grand Total
    //    objtxtGrandTotalAmt = document.getElementById("ContentPlaceHolder1_txtGrandAmt");
    //    objtxtGrandTotalAmt.value = RoundupValue(dTotal);


}
//vrushali30032015 start
function CheckRowValueLead(event, objAddNewControl) {
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    var ObjCell;
    for (var i = 1; i < objRow.length - 1; i++) {
        ObjCell = objRow[i].childNodes[0];
        if (objRow[i].style.display == "")
            if (ObjCell.type == "text") {
                if (ObjCell.isDisabled == false && ObjCell.style.display == "") {
                    if (ObjCell.value == "") {
                        alert("Please enter the record");
                        ObjCell.focus();
                        return false;
                    }
                }
            }
            else if (ObjCell.type == "textarea") {
                if (ObjCell.isDisabled == false) {
                    if (ObjCell.value == "") {
                        alert("Please enter the record");
                        ObjCell.focus();
                        return false;
                    }
                }
            }
            else if (ObjCell.type == "select-one") {
                if (ObjCell.value == "0" && ObjCell.isDisabled == false) {
                    alert("Please select the record");
                    ObjCell.focus();
                    return false;
                }
            }

    }
}
//vrushali30032015 end


function Check(eve, objcontrol) {

    var objRow = objcontrol.parentNode.parentNode.childNodes;

    var objtxtBalControl = objRow[5].childNodes[0];
    var objtxtBillControl = objRow[6].childNodes[0];

    var BalQty = dGetValue(objtxtBalControl.innerText);
    var BillQty = dGetValue(objtxtBillControl.value);

    var MOQ = dGetValue(objRow[3].childNodes[0].innerText);

    if (BalQty < BillQty) {
        alert("Please Enter the Bill Qty less than Balance Qty");
        objcontrol.focus();
        return false;
    }
    else {
        if (MOQ != 0 && (BillQty % MOQ) != 0) {
            if (BillQty / MOQ != 0) {
                objtxtBillControl.value = (parseInt(BillQty / MOQ) + 1) * MOQ > 0 ? (parseInt(BillQty / MOQ) + 1) * MOQ : 0;
                BillQty = dGetValue(objtxtBillControl.value);
            }
        }

        var objtxtRateControl = objRow[7].childNodes[0];
        var Rate = dGetValue(objtxtRateControl.innerText);

        var objtxtAmtControl = objRow[8].childNodes[0];
        objtxtAmtControl.innerText = RoundupValue(BillQty * Rate);
        CalulateGrandTotal();
    }
    return true;
};

//VikramK Begin
function checkTextAreaMaxLength(textBox, e, length) {
    //debugger;
    text = document.getElementById(textBox.id);
    var mLen = textBox["MaxLength"];
    if (null == mLen)
        mLen = length;

    var maxLength = parseInt(mLen);
    if (textBox.value.length > maxLength - 1) {
        alert("only max " + maxLength + " characters are allowed");
        //this limits the textbox with only 250 characters as lenght is given as 250.
        textBox.value = text.value.substring(0, length - 1);
    }
}
//END

