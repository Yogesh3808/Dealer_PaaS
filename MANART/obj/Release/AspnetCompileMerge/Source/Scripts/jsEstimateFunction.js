var bUsed = null;
//************Complaint**************
//When User Change Complaint
function OnComplaintValueChange(eve, ObjCombo, txtboxId) {
    if (OnComboValueChange(ObjCombo, txtboxId) == false) {

    }
    if (ObjCombo.options[ObjCombo.selectedIndex].text != "NEW") {
        CheckComplaintSelected(eve, ObjCombo);
    }
    else
        return true;
}
//Check Complaint value is select 
function CheckComplaintSelected(eve, objcontrol) {
    ////debugger;
    if (CheckForComboValue(eve, objcontrol, false) == true) {
        if (CheckComboValueAlreadySelectInGrid(objcontrol) == false)
            return false;
        if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
            return false;
        SetComplaintRecordCount();
    }
    else {
        ClearRowValueForComplaint(null, objcontrol);
    }
}
//Check Complaint Is already Used in Grid
function CheckComplaintAlreadyUsedInGrid(event, Objcontrol) {
    ////debugger;
    if (CheckTextValueAlreadyUsedInGrid(event, Objcontrol) == true) {
        //var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[0].innerText);
        var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[1].innerText);
        var ObjRecord;
        var objGrid = Objcontrol.parentNode.parentNode.parentNode;
        for (i = 1; i < objGrid.children.length; i++) {
            //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
            ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

            if (i == iRowOfSelectControl) {
                if (Objcontrol.value == "") {
                    alert("Please Enter Complaint Description")
                    Objcontrol.focus();
                    return false;
                }
            }
        }
        SetComplaintRecordCount();
    }
}
// Claer the Row Value of the selected Row
function ClearRowValueForComplaint(event, ObjControl) {
    ClearRowValue(event, ObjControl);
    SetComplaintRecordCount();
}
// Set Total Complaint Record Count
function SetComplaintRecordCount() {
    var ObjGrid;
    var iRecordCnt = 0;

    ObjGrid = document.getElementById("ContentPlaceHolder1_ComplaintsGrid");
    if (ObjGrid == null) return;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        if (ObjGrid.rows[i].cells[1].children[0].selectedIndex != 0) {
            if (ObjGrid.rows[i].cells[1].children[0].value == "9999") {
                if (ObjGrid.rows[i].cells[1].children[1].innerText != null && ObjGrid.rows[i].cells[1].children[1].innerText != "") {
                    iRecordCnt = iRecordCnt + 1;
                }
            }
            else {
                iRecordCnt = iRecordCnt + 1;
            }
        }
    }
    // To calculate Complaint Count
    var ObjComplaintCount = document.getElementById("ContentPlaceHolder1_lblComplaintsRecCnt");
    if (ObjComplaintCount != null) {
        ObjComplaintCount.innerText = iRecordCnt;
    }
}

//***************Common Function To Warranty****************
//To Check Records are entered or selected in a row
function CheckRowValue(event, objAddNewControl) {
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    ////debugger;
    var ObjCell;
    //for (var i = 0; i < objRow.length; i++) {
    for (var i = 1; i < objRow.length; i++) {
        //ObjCell = objRow[i].childNodes[0];
        ObjCell = objRow[i].childNodes[1];
        if (ObjCell.style.display == "none") {
            ObjCell = objRow[i].childNodes[3];
        }
        if (objRow[i].style.display != "none") {
            //if (ObjCell.type == "text") {
            if (ObjCell.type == "textarea") {
                //if (ObjCell.isDisabled == false) {
                if (ObjCell.value == "") {
                    alert("Please enter the record");
                    ObjCell.focus();
                    return false;
                }
                //}
            }
            else if (ObjCell.type == "select-one") {
                if (ObjCell.style.display != "none") {
                    //if (ObjCell.isDisabled == false) {
                    if (ObjCell.value == "0") {
                        alert("Please select the record");
                        ObjCell.focus();
                        return false;
                    }
                    //}
                }
            }
        }
    }
}

// Add Amount to Total Amount And Grand Amount
// typeofAmt i.e.Part/Labour/Lubricant/SubLet
function AddAmountToTotal() {
    var PcontainerName = '';
    var txtTotalAmt;
    var txtJbTotAmt;
    var TotalAmtId;

    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";

    var txtPartAmount = document.getElementById(PcontainerName + 'txtPartAmount');
    var txtLubricantAmount = document.getElementById(PcontainerName + 'txtLubricantAmount');

    //Part And Lubricant Calculation START
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    var dPTotal = 0;
    var dOTotal = 0;
    var dLTotal = 0;
    var dSTotal = 0;
    var sPartType = "";
    var sCancle = "";
    //debugger;
    for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
        var dBillQty = 0;
        var dPListRate = 0;

        var objPartType = ObjGrid.rows[i].cells[7].childNodes[1];

        //var objPCancle = ObjGrid.rows[i].cells[10].childNodes[1].childNodes[0];
        var objtxtPaidQtyControl = ObjGrid.rows[i].cells[4].childNodes[1];
        var objPListRate = ObjGrid.rows[i].cells[5].childNodes[1];
        var objPTotal = ObjGrid.rows[i].cells[6].childNodes[1];
        var ObjChkForDelete = ObjGrid.rows[i].cells[9].childNodes[1].childNodes[0];
        if (ObjChkForDelete.checked == false) {

            sPartType = objPartType.value.trim();
            //sCancle = (objPCancle.checked == true) ? "Y" : "N";
            sCancle = "N";
            dBillQty = dGetValue(objtxtPaidQtyControl.value);
            dPListRate = dGetValue(objPListRate.value);
            if (sCancle == "N") {
                if (sPartType == "P") dPTotal = dGetValue(dPTotal) + (dBillQty * dPListRate);
                if (sPartType == "O") dOTotal = dGetValue(dOTotal) + (dBillQty * dPListRate);
            }        
        }
    }
    txtPartAmount.value = parseFloat(dPTotal).toFixed(2);
    txtLubricantAmount.value = parseFloat(dOTotal).toFixed(2);

    //Part And Lubricant Calculation END
    //debugger;
    //Labour and Sublet Calculation START
    var txtLabourAmount = document.getElementById(PcontainerName + 'txtLabourAmount');
    var txtSubletAmount = document.getElementById(PcontainerName + 'txtSubletAmount');
    var ObjLGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");

    if (ObjLGrid == null) return;

    for (var i = 2; i <= ObjLGrid.rows.length - 1; i++) {
        var ObjLChkForDelete = ObjLGrid.rows[i].cells[12].childNodes[1].childNodes[0];
        if (ObjLChkForDelete.checked == false) {
            var objtxtLabTag = ObjLGrid.rows[i].cells[8].childNodes[1];
            var objtxtLSubletAmt = ObjLGrid.rows[i].cells[10].childNodes[1];
            var txtTotal = ObjLGrid.rows[i].cells[7].childNodes[1];
            //var ObjChkForDelete = ObjLGrid.rows[i].cells[10].childNodes[1].childNodes[0];
            //if (ObjChkForDelete.checked == false) {

            var dSubletAmt = dGetValue(objtxtLSubletAmt.value);
            var dLaborAmt = dGetValue(txtTotal.value);
            var sLabTag = objtxtLabTag.value.trim();

            if (sLabTag == "D" && dSubletAmt > 0) dSTotal = dGetValue(dSTotal) + dSubletAmt;
            if (sLabTag == "D" && dSubletAmt == 0) dLTotal = dGetValue(dLTotal) + dLaborAmt;
        }
    }
    txtSubletAmount.value = parseFloat(dSTotal).toFixed(2);
    txtLabourAmount.value = parseFloat(dLTotal).toFixed(2);
    //Labour and Sublet Calculation END 

    // Get Grand Amount       
    txtEstTotAmt = document.getElementById(PcontainerName + 'txtEstTotAmt');
    txtEstTotAmt.value = parseFloat(dGetValue(dPTotal) + dGetValue(dOTotal) + dGetValue(dLTotal) + dGetValue(dSTotal)).toFixed(2);

    CalulateEstPartGranTotal();
}


// When user Click on Cancel then clear the value of row
function ClearRowValue(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjCell;
    var icntOfChildren;
    var bUseChildren = true;
    for (var i = 0; i < objRow.length; i++) {
        icntOfChildren = objRow[i].children.length;
        bUseChildren = true;
        if (icntOfChildren == 0) {
            icntOfChildren = objRow[i].childNodes.length;
            bUseChildren = false;
        }
        for (var j = 0; j < icntOfChildren; j++) {
            if (bUseChildren == true) {
                ObjCell = objRow[i].children[j];
            }
            else {
                ObjCell = objRow[i].childNodes[j];
            }

            if (ObjCell.type == "text") {
                ObjCell.value = '';
            }
            else if (ObjCell.type == "textarea") {
                ObjCell.value = '';
                ObjCell.style.display = 'none';
            }
            else if (ObjCell.type == "select-one") {
                ObjCell.selectedIndex = 0;
                ObjCell.style.display = '';
            }
        }

    }
}
//check Combo Value already select in previous / next row in grid
function CheckComboValueAlreadyUsedInGrid(ObjCurRecord) {
    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
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

//check Combo Value already select in previous / next row in grid
function CheckComboValueAlreadySelectInGrid(ObjCurRecord) {
    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
    ////debugger;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
    var iColumnIndexOfControl = parseInt(ObjCurRecord.parentNode.cellIndex);
    var ObjRecord;

    //if (ObjCurRecord.id.indexOf("ContentPlaceHolder1_LabourDetailsGrid_drpLbrDescription") >= 0) {
    //    i = 2;
    //}
    //else {
    //    i = 1;
    //}

    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1 ; i < objGrid.rows.length; i++) {
        ObjRecord = objGrid.rows[i].cells[iColumnIndexOfControl].children[0];

        if (i != iRowOfSelectControl) {

            if (i < iRowOfSelectControl) {
                if (sSelecedValue != "NEW") {
                    if (ObjRecord.options[ObjRecord.selectedIndex].text == "--Select--") {
                        alert("Please select Record at line No." + i);
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

function OnComboValueChange(ObjCombo, txtboxId) {
    var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].text;
    var ParentCtrlID;
    var Sup, Post;
    var objtxtControl;
    if (sSelecedValue == "NEW") {
        ObjCombo.style.display = 'none';

        //ParentCtrlID = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        Post = ObjCombo.id.substring(ObjCombo.id.lastIndexOf("_"), (ObjCombo.id).length);
        Sup = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        ParentCtrlID = Sup.substring(0, Sup.lastIndexOf("_"));

        objtxtControl = document.getElementById(ParentCtrlID + "_" + txtboxId + Post);
        objtxtControl.style.display = '';
        objtxtControl.focus();
    }
    else {
        ObjCombo.style.display = '';

        //ParentCtrlID = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        Post = ObjCombo.id.substring(ObjCombo.id.lastIndexOf("_"), (ObjCombo.id).length);
        Sup = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        ParentCtrlID = Sup.substring(0, Sup.lastIndexOf("_"));
        objtxtControl = document.getElementById(ParentCtrlID + "_" + txtboxId + Post);

        //objtxtControl = document.getElementById(ParentCtrlID + "_" + txtboxId);
        objtxtControl.style.display = 'none';
        //objtxtControl.value='';
        //        if (CheckComboValueAlreadyUsedInGrid(ObjCombo) == false)
        //            return false;

    }
    return true;
}
///*************************


function OnJobTypeChange(sUserDepart) {
    //Check validation for jobcard type selection.
    SetGridColumnsBasedOnChStatusJobType(sUserDepart);
}


//function ShowModelMaster(objNewModelLabel, sDealerId) {
function ShowModelMaster(objNewModelLabel, sDealerId, sDocType) {
    //sujata 02022011
    var ModelDetailsValue;

    //sujata 02022011
    //ModelDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
    ModelDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId + "&sDocType=" + sDocType, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
    //sujata 02022011

    //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
    if (ModelDetailsValue == null) {
        return false;
    }
    else {
        SetModelDetails(ModelDetailsValue);
    }
    return true;
}
//SetModelDetails
function SetModelDetails(ModelDetailsValue) {
    var PcontainerName = '';
    var bValue;
    var ObjControl;
    PcontainerName = 'ContentPlaceHolder1_';//GetContainerName();
    //Sujata 20012011
    //bValue = ModelDetailsValue[18];
    bValue = ModelDetailsValue[19];
    //Sujata 20012011
    ObjControl = window.document.getElementById(PcontainerName + "lblModel");
    if (ObjControl != null) {
        ObjControl.style.display = "";
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtModelID");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[0];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtModelDescription");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[1];
        ObjControl.style.display = "";
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtGVW");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[2];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtChassisNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[3];
        ObjControl.readOnly = bValue;
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtEngineNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[4];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtInvoiceNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[5];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtInvoiceDate");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[6];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtInstallationDate");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[7];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtCustomerName");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[8];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtCustomerAddress");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[9];
        ObjControl.readOnly = bValue;
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtModelGroupID");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[10];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtModelCode");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[11];
        ObjControl.readOnly = bValue;
    }
    //Last Repair Order Date
    ObjControl = window.document.getElementById(PcontainerName + "txtLastRepairDate");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[12];
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtHrsApplicable");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[13];
        if (ModelDetailsValue[13] == "Y") {
            ObjControl = window.document.getElementById(PcontainerName + "txtHrsReading");
            if (ObjControl != null) {
                ObjControl.readOnly = false;
            }

            ObjControl = window.document.getElementById(PcontainerName + "txtOdometer");
            if (ObjControl != null) {
                ObjControl.readOnly = true;
            }
        }
        else {
            ObjControl = window.document.getElementById(PcontainerName + "txtHrsReading");
            if (ObjControl != null) {
                ObjControl.readOnly = true;
            }

            ObjControl = window.document.getElementById(PcontainerName + "txtOdometer");
            if (ObjControl != null) {
                ObjControl.readOnly = false;
            }
        }
    }

    //Odometer
    ObjControl = window.document.getElementById(PcontainerName + "txtOdometer");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[14];
    }

    //Vehicle Registration No
    ObjControl = window.document.getElementById(PcontainerName + "txtVehicleNo");
    if (ObjControl != null) {
        if (ModelDetailsValue[15] == "9999") {
            ObjControl.value = '';
            ObjControl.readOnly = false;
        }
        else {
            ObjControl.value = ModelDetailsValue[15];
            ObjControl.readOnly = true;
        }
    }

    //LasrRepairOrderNo
    ObjControl = window.document.getElementById(PcontainerName + "txtLastRepairOrderNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[16];
    }

    //Root Type ID
    ObjControl = window.document.getElementById(PcontainerName + "txtRootTypeID");
    if (ObjControl != null) {
        if (ModelDetailsValue[15] == "9999") {
            ObjControl.value = '';
            ObjControl.readOnly = false;
        }
        else {
            ObjControl.value = ModelDetailsValue[17];
            ObjControl.readOnly = true;
        }
    }
    //Sujata 20012011
    //Chassis ID
    ObjControl = window.document.getElementById(PcontainerName + "txtchassisID");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[19];
    }
    //Sujata 20012011
}

//###Common
// When user Click to delete the record then reduce the amount 
function SelectDeleteCheckboxCommon(ObjChkDelete) {

    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            return true;
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.parentNode.style.backgroundColor = 'white';
    }
}

function CheckTextValueAlreadyUsedInGrid(event, ObjCurRecord) {

    if (bUsed == null) bUsed = false;
    var i;
    ////debugger;
    var sSelecedValue = ObjCurRecord.value;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
    var ObjRecord;
    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1; i < objGrid.children.length; i++) {
        //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
        ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

        if (i != iRowOfSelectControl) {
            if (ObjRecord.type == "text" || ObjRecord.type == "textarea") {
                if (sSelecedValue.toUpperCase() == ObjRecord.value.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    //ObjCurRecord.selectedIndex = 0;
                    if (ObjCurRecord.type == "select-one")
                        ObjCurRecord.selectedIndex = 0;
                    else
                        ObjCurRecord.value = "";
                    event.returnValue = false;
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
            else if (ObjRecord.type == "select-one") {
                if (sSelecedValue.toUpperCase() == ObjRecord.options[ObjRecord.selectedIndex].text.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    //ObjCurRecord.selectedIndex = 0;
                    if (ObjCurRecord.type == "select-one")
                        ObjCurRecord.selectedIndex = 0;
                    else
                        ObjCurRecord.value = "";
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
    return true;
}


function CalulateEstPartGranTotal() {

    var objGrid = document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

    var objLGrid = document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");

    var objGridGroupTax = document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridDocTax = document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");

    var total = 0;
    var TotalSparesRate = 0;
    var TotalOilRate = 0;
    var TotalInv = 0;

    var totalQtypart = 0;
    var sPArtName = "";
    var sGroupCode = "";

    var bPartSel = "";
    var CountRow = objGrid.rows.length;
    var CountRowGrTax = objGridGroupTax.rows.length;

    for (var k = 1; k < CountRowGrTax; k++) {
        objGridGroupTax.rows[k].childNodes[4].children[0].value = 0;
    }
    
    for (var i = 1; i < CountRow; i++) {
        //total = objGrid.rows[i].childNodes[15].children[1].value;
        //debugger;
        total = objGrid.rows[i].childNodes[7].children[0].value;
        sPArtName = objGrid.rows[i].childNodes[4].children[0].value;
        sGroupCode = objGrid.rows[i].childNodes[9].children[1].value;
        bPartSel = objGrid.rows[i].childNodes[10].children[0].children[0].checked;

        sGridPartTax = objGrid.rows[i].childNodes[9].children[2].value.trim();

        //sGridPartTax = objGrid.rows[i].childNodes[16].children[0].selectedIndex;

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value)).toFixed(2);
            }
        }

        if (sPArtName != "" && bPartSel == false) {
            TotalInv = dGetValue(total) + dGetValue(TotalInv)
        }
    }
    
    CountRow = objLGrid.rows.length;
    for (var i = 1; i < CountRow; i++) {
        //total = objGrid.rows[i].childNodes[15].children[1].value;
        //debugger;
        var objtxtLabTag = objLGrid.rows[i].cells[8].childNodes[1];
        var objtxtLSubletAmt = objLGrid.rows[i].cells[10].childNodes[1];
        var txtTotal = objLGrid.rows[i].cells[7].childNodes[1];

        var dSubletAmt = dGetValue(objtxtLSubletAmt.value);
        var dLaborAmt = dGetValue(txtTotal.value);
        var sLabTag = objtxtLabTag.value.trim();
        sPArtName = objLGrid.rows[i].cells[3].childNodes[1].value;

        if (sLabTag == "D" && dSubletAmt > 0) total = dSubletAmt;
        if (sLabTag == "D" && dSubletAmt == 0) total = dLaborAmt;

        sGroupCode = objLGrid.rows[i].childNodes[10].children[1].value;
        bPartSel = objLGrid.rows[i].childNodes[13].children[0].children[0].checked;

        sGridPartTax = objLGrid.rows[i].childNodes[10].children[2].value.trim();

        //sGridPartTax = objGrid.rows[i].childNodes[16].children[0].selectedIndex;

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value)).toFixed(2);
            }
        }
        if (sPArtName != "" && bPartSel == false) {
            TotalInv = dGetValue(total) + dGetValue(TotalInv)
        }
    }

    //    objGridGroupTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalSparesRate).toFixed(2);
    //    objGridGroupTax.rows[2].childNodes[3].children[0].value = parseFloat(TotalOilRate).toFixed(2);

    var CountGrpRow = objGridGroupTax.rows.length;

    var dGrpTotal = 0;
    var dGrpDiscPer = 0;
    var dGrpDiscAmt = 0;
    var dGrpTaxAppAmt = 0;

    var dGrpMTaxPer = 0;
    var dGrpMTaxAmt = 0;

    var dGrpTax1Per = 0;
    var dGrpTax1Amt = 0;
    var sGrpTax1ApplOn = "";

    var dGrpTax2Per = 0;
    var dGrpTax2Amt = 0;

    var dGrpTotal = 0;

    var dDocDiscAmt = 0;
    var dDocLSTAmt = 0;
    var dDocCSTAmt = 0;
    var dDocTax1Amt = 0;
    var dDocTax2Amt = 0;
    var dDocTotalAmtFrPFOther = 0;
    var sGrpMTaxTag = "";

    for (var i = 1; i < CountGrpRow; i++) {
        // if (objGridGroupTax.rows[i].className.indexOf('RowStyle') > 0) {
        //group total
        dGrpTotal = dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
        //group Percentage
        dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].childNodes[5].children[0].value);
        //group Discount Amount
        dGrpDiscAmt = parseFloat(dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100))).toFixed(2);
        //Doc Discount Amount
        dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
        //group Discount Amount display                                   
        objGridGroupTax.rows[i].childNodes[6].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
        //Amount whiich is applicable for tax
        dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);
        //debugger;
        //Main tax calculation
        dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].childNodes[9].children[0].value);
        if (isNaN(dGrpMTaxPer) == true) dGrpMTaxPer = 0;
        ////debugger;
        dGrpMTaxAmt = parseFloat(dGetValue(dGetValue(dGrpTaxAppAmt) * dGetValue(dGrpMTaxPer / 100))).toFixed(2);
        sGrpMTaxTag = objGridGroupTax.rows[i].childNodes[8].children[2].value;
        //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
        if (sGrpMTaxTag == "I") {
            dDocLSTAmt = dGetValue(dDocLSTAmt) + dGetValue(dGrpMTaxAmt);
        }
        else if (sGrpMTaxTag == "O") {
            dDocCSTAmt = dGetValue(dDocCSTAmt) + dGetValue(dGrpMTaxAmt);
        }
        objGridGroupTax.rows[i].childNodes[10].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);

        dGrpTax1Per = dGetValue(objGridGroupTax.rows[i].childNodes[13].children[0].value);
        sGrpTax1ApplOn = objGridGroupTax.rows[i].childNodes[12].children[2].value;

        if (isNaN(dGrpTax1Per) == true) dGrpTax1Per = 0;
        //Sujata 24092014Begin
        //dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        if (sGrpTax1ApplOn == "1") {
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax1Per / 100));
        } else if (sGrpTax1ApplOn == "3") {
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax1Per / 100));
        } else {
            dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        }
        //Sujata 24092014End
        dDocTax1Amt = dGetValue(dDocTax1Amt) + dGetValue(dGrpTax1Amt);
        objGridGroupTax.rows[i].childNodes[14].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);

        dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].childNodes[17].children[0].value);
        //New
        sGrpTax2ApplOn = objGridGroupTax.rows[i].childNodes[16].children[2].value;
        //sGrpTax2ApplOn = objGridGroupTax.rows[i].childNodes[17].children[2].value;

        if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;
        //Sujata 24092014Begin
        //dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
        if (sGrpTax2ApplOn == "1") {
            dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax2Per / 100));
        } else if (sGrpTax2ApplOn == "3") {
            dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax2Per / 100));
        } else {
            dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
        }
        //Sujata 24092014End            
        dDocTax2Amt = dGetValue(dDocTax2Amt) + dGetValue(dGrpTax2Amt);
        objGridGroupTax.rows[i].childNodes[18].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);

        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt));
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(2);
        //dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal).toFixed(0); //This takes for apply PF and Other tax
        //objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(0);

        //  }
    }

    objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalInv).toFixed(2);
    objGridDocTax.rows[1].childNodes[4].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);

    objGridDocTax.rows[1].childNodes[6].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);
    objGridDocTax.rows[1].childNodes[7].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);

    objGridDocTax.rows[1].childNodes[8].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);
    objGridDocTax.rows[1].childNodes[9].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);

    var dDocPFPer = 0;
    var dDocPFAmt = 0;
    var dDocOtherPer = 0;
    var dDocOtherAmt = 0;
    var dDocGrandAmt = 0;

    dDocPFPer = objGridDocTax.rows[1].childNodes[10].children[0].value;
    if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
    dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    objGridDocTax.rows[1].childNodes[11].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    dDocOtherPer = objGridDocTax.rows[1].childNodes[12].children[0].value;
    if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
    dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
    objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));
    var hdnRounOff = document.getElementById('ContentPlaceHolder1_hdnRounOff');

    if (hdnRounOff.value == "N") {
        objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);
    }
    else {
        objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(0);
    }
    //objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(0);

}
