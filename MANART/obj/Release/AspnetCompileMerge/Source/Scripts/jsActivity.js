var bUsed = null;



//Check Sublet value is select
function CheckActivityHeadSelected(eve, objcontrol) {
    if (CheckForComboValue(eve, objcontrol) == true) {
        if (CheckComboValueAlreadyUsedInActivityHeadGrid(objcontrol) == false)
            return false;
        
    }
    //else {
    //   // ClearRowValueForActivityHead(event, objcontrol)
    //}
}
//check Combo Value already select in previous / next row in grid
function CheckComboValueAlreadyUsedInActivityHeadGrid(ObjCurRecord) {
    
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

