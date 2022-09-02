//**************SubLet function***********************
function AddSubletTotalToClaimAmount(event, ObjAmtControl) {
    var dOrgLineSubletAmt;
    var dLineSubletAmt;
    var dDiffOfSubletAmt;

    debugger;
    if (ObjAmtControl.value == "") {
        alert("Please enter the record");
        //event.returnValue=false     
        //ObjControl.focus();
        return false;
    }
    // Calculate Line Level Sublet amount    
    //*******************************
    var objRow = ObjAmtControl.parentNode.parentNode.childNodes;

    // Get Line Level Old Sublet amount  
    dOrgLineSubletAmt = dGetValue(objRow[6].children[1].value);

    var LRate = dGetValue(objRow[5].children[0].value);
    var LHRs = dGetValue(objRow[4].children[0].value);

    // Calculate Line Level Sublet Amt
    dLineSubletAmt = dGetValue(objRow[6].children[0].value);
    dLineSubletAmt = RoundupValue(dLineSubletAmt);

    //if (dLineSubletAmt == 0) {        
    //    return;
    //}
    //objRow[6].children[1].value = RoundupValue(dLineSubletAmt);

    LHRs = dLineSubletAmt / LRate;
    objRow[4].children[0].value = RoundupValue(LHRs);

    if (dOrgLineSubletAmt == 0) {
        dDiffOfSubletAmt = dLineSubletAmt;
    }
    else if (dOrgLineSubletAmt == dLineSubletAmt) {
        dDiffOfSubletAmt = 0;
    }
    else {

        dDiffOfSubletAmt = (dLineSubletAmt - dOrgLineSubletAmt);
    }
    AddAmountToTotal("Sublet", dDiffOfSubletAmt);
    //Sujata 19012011
    SetSubletRecordCount();
    //Sujata 19012011
}


// Set Total Sublet Record Count
function SetSubletRecordCount() {
    var ObjGrid;
    var iRecordCnt = 0;

    ObjGrid = document.getElementById("ContentPlaceHolder1_SubletDetailsGrid");
    if (ObjGrid == null) return;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        //if (ObjGrid.rows[i].cells[1].children[0].value != 0) 
        if (ObjGrid.rows[i].cells[6].children[0].value != 0) {
            iRecordCnt = iRecordCnt + 1;
        }
    }
    // To calculate Part Count
    var ObjSubletCount = document.getElementById("ContentPlaceHolder1_lblSubletRecCnt");
    if (ObjSubletCount != null) {
        ObjSubletCount.innerText = iRecordCnt;
    }
}
//Check Sublet Description Is Entered
function CheckSubletDescUsed(eve, objcontrol) {
    if (CheckTextboxValueForString(event, objcontrol, false) == false) {
        return false;
    }
    if (CheckTextValueAlreadyUsedInGrid(eve, objcontrol) == false) {
        ClearRowValueForSublet(eve, objcontrol)
        return false;
    }
}
//Check Sublet value is select
function CheckSubletSelected(eve, objcontrol) {
    if (CheckForComboValue(eve, objcontrol) == true) {
        if (CheckComboValueAlreadyUsedInSubletGrid(objcontrol) == false)
            return false;
        //SetSubletRecordCount();
    }
    else {
        ClearRowValueForSublet(event, objcontrol)
    }
}
//check Combo Value already select in previous / next row in grid
function CheckComboValueAlreadyUsedInSubletGrid(ObjCurRecord) {
    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iColumnIndexOfControl = parseInt(ObjCurRecord.parentNode.cellIndex);
    var ObjRecord;
    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1; i < objGrid.rows.length; i++) {
        ObjRecord = objGrid.rows[i].cells[iColumnIndexOfControl].children[0];

        if (i != iRowOfSelectControl) {
            if (ObjRecord.selectedIndex != 0) {
                if (sSelecedValue != "NEW") {
                    if (sSelecedValue == ObjRecord.options[ObjRecord.selectedIndex].text) {
                        alert("Record is already selected at line No." + i);
                        ObjCurRecord.selectedIndex = 0;
                        if (bUsed == false) {
                            ObjCurRecord.focus();
                            bUsed = true;
                        }
                        else {
                            bUsed = null;
                        }
                        return false;
                    }
                }
            }
        }
    }
    return true;
}
// When user Click on Cancel of SubLet Grid then clear the value of row
function ClearRowValueForSublet(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var i = 1;
    var ObjControl;
    var TotalAmount;

    //Set LabourId;
    objRow[1].childNodes[0].value = '0';

    //Set Labour/SubLet Code;
    //objRow[2].childNodes[0].value = '9999';


    //Set SubLet description;
    objRow[2].children[0].style.display = "";
    objRow[2].children[0].selectedIndex = 0;
    objRow[2].children[1].value = '';
    objRow[2].children[1].style.display = "none";


    //Set ManHrs
    objRow[3].childNodes[0].value = '1';

    //Set Labour Rate
    objRow[4].childNodes[0].value = '1';

    //Total
    TotalAmount = dGetValue(objRow[6].children[0].value);
    objRow[5].children[0].value = '';
    objRow[5].children[1].value = '';

    //Set Job Code
    objRow[6].children[0].selectedIndex = 0;

    //Set Percentage
    objRow[7].childNodes[0].value = '';
    objRow[8].childNodes[0].value = '';
    objRow[9].childNodes[0].value = '';


    TotalAmount = (0 - TotalAmount);
    AddAmountToTotal("Sublet", TotalAmount);


    //SetNewLabel Display
    ObjControl = objRow[10].children[1];
    if (ObjControl != null) ObjControl.style.display = "none";



    //SetNewLabel Display    
    ObjControl = objRow[i].children[1];
    if (ObjControl != null) ObjControl.style.display = "none";
    SetSubletRecordCount();
}
// When user Click to delete the record then reduce the amount
function SelectDeleteCheckboxForSubLet(ObjChkDelete) {

    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    var dDiffOfSubLetAmt = 0;

    // Get Line Level SubLet amount
    var dOrgLineAmt = dGetValue(ObjChkDelete.parentNode.parentNode.parentNode.children[6].childNodes[1].value);

    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            dDiffOfSubLetAmt = 0 - dOrgLineAmt
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        dDiffOfSubLetAmt = dOrgLineAmt
    }
    AddAmountToTotal("Sublet", dDiffOfSubLetAmt);
}

//---------------------------------------------

