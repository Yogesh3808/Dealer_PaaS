///############Following Functions are for multiselect DropDown For Dealer
function SHMulSelDlr(ControlClientID, e) {
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMainDlr');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        if (window.document.getElementById(ControlClientID + '_' + 'hapbDlr').value == 'True') {
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

function SHMulSel01Dlr(ControlClientID, divMainWidth, divMainHeight, e) {
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDealerName');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMainDlr');

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

function opentheboxDlr(ControlClientID, divMainWidth, divMainHeight) {
    //    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');
    //    divisionMain.style.display = 'block';
    //    divisionMain.style.width = divMainWidth + 'px';
    //    divisionMain.style.height = divMainHeight + 'px';
}

function DisplayTitleDlr(control) {
    control.title = control.value;
}

function SCITDlr(chkbox, ControlClientID) {
    var labelCollection = window.document.getElementsByTagName('label');
    var hSelectedItemsValueList = document.getElementById(ControlClientID + '_' + 'hsivDlr');
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

function SelectAllDlr() {
    //<%=this.ClientID%>
    var ObjControl_ID = document.getElementById('ContentPlaceHolder1_txtControl_IDDlr');
    ControlClientID = ObjControl_ID.value;
    var tblCbl = document.getElementById('ContentPlaceHolder1_ChkDealer');
    var ChkAllReg = document.getElementById('ContentPlaceHolder1_ChkAllDlr');
    if (tblCbl == null) return;
    var tblBody = tblCbl.childNodes[0];
    var counter = tblBody.childNodes.length;
    bCheckAll = ChkAllReg.checked;
//    if (bCheckAll == null) bCheckAll = true;
//    else if (bCheckAll == true) bCheckAll = false;
    //else if (bCheckAll == false) bCheckAll = true;
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
        ControlClientID = 'ContentPlaceHolder1';
        SCITDlr(checkbox, ControlClientID);
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



///############Following Functions are for multiselect DropDown For Depo
function SHMulSelDpo(ControlClientID, e) {
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDepoName');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMainDpo');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        if (window.document.getElementById(ControlClientID + '_' + 'hapbDpo').value == 'True') {
            document.getElementById(ControlClientID + '_' + '__EVENTTARGET1').value = 'MultiSelectDropDown';
            document.getElementById(ControlClientID + '_' + '__EVENTARGUMENT1').value = textBoxMain.value;
            __doPostBack('MultiSelectDropDown', window.document.getElementById(ControlClientID + '_' + 'txtDepoName').value);
        }
    }
    else {
        divisionMain.style.display = 'block';
        divisionMain.className = 'dvmain'
    }
    var evt = (window.event == null) ? e : window.event;
    evt.cancelBubble = true;
}

function SHMulSel01Dpo(ControlClientID, divMainWidth, divMainHeight, e) {
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDepoName');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMainDpo');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        return false;
    }
    else {
        var ChkDealer = document.getElementById(ControlClientID + "_" + "ChkDepo");
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

function opentheboxDpo(ControlClientID, divMainWidth, divMainHeight) {
    //    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');
    //    divisionMain.style.display = 'block';
    //    divisionMain.style.width = divMainWidth + 'px';
    //    divisionMain.style.height = divMainHeight + 'px';
}

function DisplayTitleDpo(control) {
    control.title = control.value;
}

function SCITDpo(chkbox, ControlClientID) {
    var labelCollection = window.document.getElementsByTagName('label');
    var hSelectedItemsValueList = document.getElementById(ControlClientID + '_' + 'hsivDpo');
    var textBoxCurrentValue = new String();
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDepoName');
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

function SelectAllDpo() {
    //<%=this.ClientID%>
    var tblCbl = document.getElementById('ContentPlaceHolder1_ChkDepo');
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
        ControlClientID = 'ContentPlaceHolder1';
        SCITDpo(checkbox, ControlClientID);
    }
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtDepoName');
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


///############Following Functions are for multiselect DropDown For Region
function SHMulSelReg(ControlClientID, e) {
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtRegion');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMainReg');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        if (window.document.getElementById(ControlClientID + '_' + 'hapbReg').value == 'True') {
            document.getElementById(ControlClientID + '_' + '__EVENTTARGET1').value = 'MultiSelectDropDown';
            document.getElementById(ControlClientID + '_' + '__EVENTARGUMENT1').value = textBoxMain.value;
            __doPostBack('MultiSelectDropDown', window.document.getElementById(ControlClientID + '_' + 'txtRegion').value);
        }
    }
    else {
        divisionMain.style.display = 'block';
        divisionMain.className = 'dvmain'
    }
    var evt = (window.event == null) ? e : window.event;
    evt.cancelBubble = true;
}

function SHMulSel01Reg(ControlClientID, divMainWidth, divMainHeight, e) {
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtRegion');
    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMainReg');

    var displayStatus = divisionMain.style.display;
    if (displayStatus == 'block') {
        divisionMain.style.display = 'none';
        return false;
    }
    else {
        var ChkDealer = document.getElementById(ControlClientID + "_" + "ChkRegion");
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

function opentheboxReg(ControlClientID, divMainWidth, divMainHeight) {
    //    var divisionMain = window.document.getElementById(ControlClientID + '_' + 'divMain');
    //    divisionMain.style.display = 'block';
    //    divisionMain.style.width = divMainWidth + 'px';
    //    divisionMain.style.height = divMainHeight + 'px';
}

function DisplayTitleReg(control) {
    control.title = control.value;
}

function SCITReg(chkbox, ControlClientID) {
    var labelCollection = window.document.getElementsByTagName('label');
    var hSelectedItemsValueList = document.getElementById(ControlClientID + '_' + 'hsivReg');
    var textBoxCurrentValue = new String();
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtRegion');
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

function SelectAllReg() {
    //<%=this.ClientID%>
    var tblCbl = document.getElementById('ContentPlaceHolder1_ChkRegion');   
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
        ControlClientID = 'ContentPlaceHolder1';
        SCITReg(checkbox, ControlClientID);
    }
    var textBoxMain = window.document.getElementById(ControlClientID + '_' + 'txtRegion');
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