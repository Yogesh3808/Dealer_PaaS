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
 //*******************Investigations***********************
 
//When User Change Investigation
function OnInvestigationValueChange(eve, ObjCombo, txtboxId) {
    //debugger;
    if (OnComboValueChange(ObjCombo, txtboxId) == false) {

    }
    if (ObjCombo.options[ObjCombo.selectedIndex].text != "NEW") {
        CheckInvestigationSelected(eve, ObjCombo);
    }
    else
        return true;
    //if (CheckTextValueAlreadyUsedInGrid(event, ObjCombo) == true) {
    //    SetInvestigationRecordCount();
    //}
}

//Check Complaint value is select
function CheckInvestigationSelected(eve, objcontrol) {
    //debugger;
    if (CheckForComboValue(eve, objcontrol, false) == true) {
        if (CheckComboValueAlreadySelectInGrid(objcontrol) == false)
            return false;
        if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
            return false;
        SetInvestigationRecordCount();
    }

}

//Check Investigations Is already Used in Grid
function CheckInvestigationAlreadyUsedInGrid(event, Objcontrol) {
    //debugger;
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
                    alert("Please Enter Investigation Description")
                    Objcontrol.focus();
                    return false;
                }
            }
        }
        SetInvestigationRecordCount();
    }
}
// Claer the Row Value of the selected Row
function ClearRowValueForInvestigation(event, ObjControl) {
    ClearRowValue(event, ObjControl);
    SetInvestigationRecordCount();
}
// Set Total Investigations Record Count
function SetInvestigationRecordCount() {
    var ObjGrid;
    var iRecordCnt = 0;

    ObjGrid = document.getElementById("ContentPlaceHolder1_InvestigationsGrid");
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
    // To calculate Investigation Count
    var ObjInvestigationCount = document.getElementById("ContentPlaceHolder1_lblInvestigationsRecCnt");
    if (ObjInvestigationCount != null) {
        ObjInvestigationCount.innerText = iRecordCnt;
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
function AddAmountToTotal(typeofAmt, dAmount) {
    var PcontainerName = '';
    var txtTotalAmt;
    var txtClaimAmt;
    var TotalAmtId;
    var ClaimAmtId = 'txtClaimAmt';

    PcontainerName = GetContainerName();
    //Megha 09102012
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //Megha 09102012
    //if (typeofAmt == "Part") {
    //    // Get Part Amount
    //    TotalAmtId = 'txtPartAmount';
    //}
    //else if (typeofAmt == "Labour") {
    //    // Get Labour Amount
    //    TotalAmtId = 'txtLabourAmount';
    //}
    //else if (typeofAmt == "Lubricant") {
    //    // Get Labour Amount
    //    TotalAmtId = 'txtLubricantAmount';
    //}
    //else if (typeofAmt == "Sublet") {
    //    // Get Labour Amount
    //    TotalAmtId = 'txtSubletAmount';
    //}

    //txtTotalAmt = document.getElementById(PcontainerName + TotalAmtId);
    txtClaimAmt = document.getElementById(PcontainerName + ClaimAmtId);
    txtPartAmount = document.getElementById("ContentPlaceHolder1_txtPartAmount");
    txtLabourAmount = document.getElementById("ContentPlaceHolder1_txtLabourAmount");
    txtLubricantAmount = document.getElementById("ContentPlaceHolder1_txtLubricantAmount");
    txtSubletAmount = document.getElementById("ContentPlaceHolder1_txtSubletAmount");
    debugger;
    var PartAmt = 0;
    var LabourAmt = 0;
    var LubAmt = 0;
    var SubletAmt = 0;

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");
    if (ObjGrid == null) return;
    var ObjControl = null;

    for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
        var objPartChk = ObjGrid.rows[i].cells[16].childNodes[1].childNodes[0].checked;        
        if (objPartChk == false) {            
            var PartAmt = PartAmt + dGetValue(ObjGrid.rows[i].cells[11].childNodes[1].value);
        }
    }
    txtPartAmount.value = RoundupValue(dGetValue(PartAmt));

    ObjGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
    if (ObjGrid == null) return;


    for (var i = 2; i < ObjGrid.rows.length; i++) {
        var objLabourChk = ObjGrid.rows[i].children[11].childNodes[1].childNodes[0].checked;
        if (objLabourChk == false) {
            var lblLabTotal = ObjGrid.rows[i].children[6].childNodes[1].value;            
            LabourAmt = LabourAmt + dGetValue(lblLabTotal);
        }
    }
    txtLabourAmount.value = RoundupValue(dGetValue(LabourAmt));
    
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_LubricantDetailsGrid");
    if (ObjGrid == null) return;


    for (var i = 2; i < ObjGrid.rows.length; i++) {
        var objLubChk = ObjGrid.rows[i].children[10].childNodes[1].childNodes[0].checked;
        if (objLubChk == false) {
            LubAmt = LubAmt + dGetValue(ObjGrid.rows[i].children[5].childNodes[1].value);
        }
    }
    
    txtLubricantAmount.value = RoundupValue(dGetValue(LubAmt));

    ObjGrid = window.document.getElementById("ContentPlaceHolder1_SubletDetailsGrid");
    if (ObjGrid == null) return;


    for (var i = 1; i < ObjGrid.rows.length; i++) {
        var objsubChk = ObjGrid.rows[i].children[10].childNodes[1].childNodes[0].checked;
        if (objsubChk == false) {            
            var lblsubsAmt = ObjGrid.rows[i].children[5].childNodes[1];            
            SubletAmt = SubletAmt + dGetValue(lblsubsAmt.value);
        }
    }
    txtSubletAmount.value = RoundupValue(dGetValue(SubletAmt));

    txtClaimAmt.value = RoundupValue(dGetValue(PartAmt) + dGetValue(LabourAmt) + dGetValue(LubAmt) + dGetValue(SubletAmt));

    //if (txtTotalAmt.value == "0") {
    //    txtTotalAmt.value = RoundupValue(dGetValue(dAmount));
    //}
    //else {
    //    txtTotalAmt.value = RoundupValue(dGetValue(txtTotalAmt.value) + dGetValue(dAmount));
    //}


    //// Get Grand Amount    
    //txtClaimAmt = document.getElementById(PcontainerName + ClaimAmtId);
    //if (txtClaimAmt.value == "0") {
    //    txtClaimAmt.value = RoundupValue(dGetValue(dAmount));
    //}
    //else {
    //    txtClaimAmt.value = RoundupValue(dGetValue(txtClaimAmt.value) + dGetValue(dAmount));
    //}
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
        debugger;
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
//To Show Chassis Master
function ShowChassisMaster(objNewModelLabel, sDealerId, sHOBrID, sDocFormat) {
    //var ChassisNo;
    debugger;
    var JobID;
    var sClmInvType;
    var DrpInvType = window.document.getElementById('ContentPlaceHolder1_DrpInvType');
    if (DrpInvType == null) return;
    if (DrpInvType.selectedIndex == 0) {
        alert('Please Select Claim Invoice Type !');
        return false;
    }
    else {
        sClmInvType = DrpInvType.options[DrpInvType.selectedIndex].value;
    }

    var DropClaimTypes = window.document.getElementById('ContentPlaceHolder1_DropClaimTypes');
    if (DropClaimTypes == null) return;
    if (DropClaimTypes.selectedIndex == 0) {
        alert('Please Select Claim Type !');
        return false;
    }
    else {
        sDocType = DropClaimTypes.options[DropClaimTypes.selectedIndex].value;        
        //ChassisNo = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectChassis.aspx?DealerID=" + sDealerId + "&sDocType=" + sDocType, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
        JobID = window.showModalDialog("../Common/frmJobcardSelection.aspx?DealerID=" + sDealerId + "&HOBrID=" + sHOBrID + "&sDocType=" + sDocType + "&sDocFormat=" + sDocFormat + "&ClmInvType=" + sClmInvType, 'PopupPage', 'dialogHeight:300px;dialogWidth:1000px;resizable:0;location=no;');
        //debugger;
        //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
        if (JobID == null) {
            return false;
        }
        else {
            //        hdnChassis = document.getElementById("ContentPlaceHolder1_hdnChassis");
            //        if(hdnChassis!=null)
            //            hdnChassis.value = ChassisNo;

            hdnJobcardID = document.getElementById("ContentPlaceHolder1_hdnJobcardID");
            if (hdnJobcardID != null) {
                // hdnJobcardID.value = JobID[0];  // OLD Code 
                hdnJobcardID.value = JobID[1];
            }
        }
    }
    return true;
}

///*************************
//To Show Model Master
//sujata 02022011
//function ShowModelMaster(objNewModelLabel, sDealerId) {
function ShowModelMaster(objNewModelLabel, sDealerId, sDocType) {
//sujata 02022011
    var ModelDetailsValue;
    
    //sujata 02022011
    //ModelDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
    ModelDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId + "&sDocType=" + sDocType, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
    //sujata 02022011
    
    //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
    if (ModelDetailsValue == null) 
    {
    return false;
    }
    else
    {
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
    if (ObjControl != null) 
    {
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
    if (ObjControl != null) 
    {
        if (ModelDetailsValue[15] == "9999") 
        {
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
///************************* Common***************
// to Check OdometerReading with last meter reading
function CheckOdometerReading(event,ObjControl) 
{
    var ObjLastMeterReading;
    var sLastMeterReading = "";
    if (CheckTextboxValueForNumeric(event, ObjControl,false ) == false) {
        return;
    }
    if (ObjControl.value == "") return;
    ObjLastMeterReading = window.document.getElementById("ContentPlaceHolder1_txtLastMeterReading");
    if (ObjLastMeterReading != null) {
        sLastMeterReading = ObjLastMeterReading.value;
    }
    if (sLastMeterReading == "") return;
    else 
    {
        if (dGetValue(ObjControl.value) < dGetValue(sLastMeterReading)) 
        {
            alert("Odometer Reading should not be less than the Last Odometer Reading  '" + sLastMeterReading + "'!.");
            ObjControl.value = "";
            ObjControl.focus();
        }
    }
}
// to Check Hour meterReading with last meter reading
function CheckHrsReading(event,ObjControl) {
    var ObjLastMeterReading;
    var sLastMeterReading = "";
    if (CheckTextboxValueForNumeric(event, ObjControl,false) == false) {
        return;
    }
    if (ObjControl.value == "") return;
    ObjLastMeterReading = window.document.getElementById("ContentPlaceHolder1_txtLastMeterReading");
    if (ObjLastMeterReading != null) {
        sLastMeterReading = ObjLastMeterReading.value;
    }
    if (sLastMeterReading == "") return;    
    else {
        if (dGetValue(ObjControl.value) < dGetValue(sLastMeterReading))
         {
            alert("Hour Meter Reading should not be less than the Last Odometer Reading  '" + sLastMeterReading + "'!.");
            ObjControl.value = "";
            ObjControl.focus();
        }
    }
    //
}
// check Repair Complete date should be less than or equal to Warranty Claim date
function CheckRepairCompleteDate() {
    var PcontainerName = '';
    var ObjControl;
    var ObjRepairOrderDate;
    var ObjRepairCompleteDate;
    var sRepairOrderDate = '';
    var sClaimDate = '';
    var sRepairCompleteDate = '';
    PcontainerName = GetContainerName();

    // Get Repair Complete Date
    ObjRepairCompleteDate = window.document.getElementById(PcontainerName + "txtRepairCompleteDate_txtDocDate");
    if (ObjRepairCompleteDate != null) {
        sRepairCompleteDate = ObjRepairCompleteDate.value;
    }

    if (sRepairCompleteDate == "") return;

    // Get claim Date
    ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (ObjControl != null) {
        sClaimDate = ObjControl.value;
    }

    //Check With claim Date
    if (bCheckFirstDateIsGreaterThanSecondDate(sRepairCompleteDate, sClaimDate) == true) {
        alert('Repair Complete Date should not be greater than Claim Date !');
        ObjRepairCompleteDate.value = '';
        ObjRepairCompleteDate.focus();
        return false;
    }

    // Get Repair Order Date
    ObjRepairOrderDate = window.document.getElementById(PcontainerName + "txtRepairOrderDate_txtDocDate");
    if (ObjRepairOrderDate != null) {
        sRepairOrderDate = ObjRepairOrderDate.value;
    }
    if (sRepairOrderDate == "") {

        return
    }
    //Check With repair Order Date
    if (bCheckFirstDateIsGreaterThanSecondDate(sRepairOrderDate, sRepairCompleteDate) == true) {
        alert('Repair Complete Date should be greater than the Repair Order Date !');
        ObjRepairCompleteDate.value = '';
        ObjRepairCompleteDate.focus();
        return false;
    }
}

// check Repair order date should be less than or equal to Warranty Claim date
function CheckRepairOrderDateWithClaimDate() {
    var PcontainerName = '';
    var ObjControl;
    var ObjRepairOrderDate;
    var sRepairOrderDate = '';
    var sClaimDate = '';
    var sFailureDate = '';
    var ObjRepairCompleteDate;    
    var sRepairCompleteDate = '';
    PcontainerName = GetContainerName();

    // Get claim Date
    ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (ObjControl != null) {
        sClaimDate = ObjControl.value;
    }
    
    // Get Repair Order Date
    ObjRepairOrderDate = window.document.getElementById(PcontainerName + "txtRepairOrderDate_txtDocDate");

    if (ObjRepairOrderDate != null) {
        sRepairOrderDate = ObjRepairOrderDate.value;
    }
    if (sRepairOrderDate == "") return;

    //Check With Claim Date    
    if (bCheckFirstDateIsGreaterThanSecondDate(sRepairOrderDate, sClaimDate) == true) {
        alert('Repair Order date should not be greater than Claim Date !');
        ObjRepairOrderDate.value = '';
        ObjRepairOrderDate.focus();
        return;
    }    

    // Get Failure Date
    ObjControl = window.document.getElementById(PcontainerName + "txtFailureDate_txtDocDate");
    if (ObjControl != null) {
        sFailureDate = ObjControl.value;
    }
    if (sFailureDate == "") return;

    if (bCheckFirstDateIsGreaterThanSecondDate(sFailureDate, sRepairOrderDate) == true)
     {    
        alert('Repair Order Date should be greater than Failure Date !');
        ObjRepairOrderDate.value = '';
        ObjRepairOrderDate.focus();
        return false;
    }
    // check with Repair Complete Date
    // Get Repair Complete Date
    ObjRepairCompleteDate = window.document.getElementById(PcontainerName + "txtRepairCompleteDate_txtDocDate");
    if (ObjRepairCompleteDate != null) {
        sRepairCompleteDate = ObjRepairCompleteDate.value;
    }
    if (sRepairCompleteDate == "") return;
     if (bCheckFirstDateIsGreaterThanSecondDate(sRepairOrderDate, sRepairCompleteDate) == true)
     {   
        alert('Repair Order Date should be less than Repair Complete Date !');
        ObjRepairOrderDate.value = '';        
        ObjRepairOrderDate.focus();
        return false;
    }
}
// check Repair order date should be less than or equal to Warranty Claim date
function CheckFailureDateWithRepairOrderDate() {
    var PcontainerName = '';
    var ObjControl;
    var sRepairOrderDate = '';
    var sLastRepairDate = '';
    var ObjFailureDate;
    var sFailureDate = '';
    PcontainerName = GetContainerName();

    // Get Failure Date
    ObjFailureDate = window.document.getElementById(PcontainerName + "txtFailureDate_txtDocDate");
    if (ObjFailureDate != null) {
        sFailureDate = ObjFailureDate.value;
    }
    if (sFailureDate == "") return;

    //Check With Claim Date
    ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (bCheckFirstDateIsGreaterThanSecondDate(sFailureDate, ObjControl.value) == true ) 
    {
        //VHP Warranty Start
        alert("Please Select The Failure Date Less Than Claim Date!");        
        //alert("Please Select The Failure Date Greater Than Claim Date!");
        //VHP Warranty End
        ObjFailureDate.value = "";
        return;
    }
    
    //Get Last Repair Complete Date
    ObjControl = window.document.getElementById(PcontainerName + "txtLastRepairDate");
    if (ObjControl != null) {
        sLastRepairDate = ObjControl.value;
    }
    if (sLastRepairDate != "") 
    {
        //Sujata 18012011
        //The following check is not required. Only check should be that Failure can’t be in Future Date or After Job card date
//        if (sLastRepairDate == sFailureDate) 
//        {
//            alert("Failure Date should not be Less than or equal to Last Repair Complete Date '" + sLastRepairDate + "'  !.");
//            ObjFailureDate.value = '';            
//            return false;
//        }
//     if (bCheckFirstDateIsGreaterThanSecondDate(sLastRepairDate, sFailureDate) == true)
//        {          
//            alert("Failure Date should not be Less than Last Repair Complete Date '" + sLastRepairDate +  "'  !.");
//            ObjFailureDate.value = '';            
//            return false;
//        }
        //Sujata 18012011
    }
    // Get Repair Order Date
    ObjControl = window.document.getElementById(PcontainerName + "txtRepairOrderDate_txtDocDate");
    if (ObjControl != null) {
        sRepairOrderDate = ObjControl.value;
    }
    if (sRepairOrderDate == "") return;    
    
    if (bCheckFirstDateIsGreaterThanSecondDate(sFailureDate, sRepairOrderDate) == true)
        {    
        alert('Failure Date should be less than Repair Order Date!.');
        ObjFailureDate.value = '';
        ObjFailureDate.focus();
        return false;
    }
}

// To check Total of all Percent should be 100 for Item Wise
function CheckTotalOfPercentage(event ,objControl) {

    //    if (CheckPercentageValue(event, objControl) == false) {
    //        return false;
    //    }
    var objRow = objControl.parentNode.parentNode.childNodes;
    var iCellId = objControl.parentNode.cellIndex;

    var dCustShare = dGetValue(objRow[iCellId].children[0].value);
    objRow[iCellId].children[0].value = RoundupValue(dCustShare);

    iCellId = iCellId - 1;
    var dDealerShare = dGetValue(objRow[iCellId].children[0].value);
    objRow[iCellId].children[0].value = RoundupValue(dDealerShare);

    iCellId = iCellId - 1;
    var dVECVShare = dGetValue(objRow[iCellId].children[0].value);
    objRow[iCellId].children[0].value = RoundupValue(dVECVShare);

    var dTotal = dVECVShare + dDealerShare + dCustShare;
    if (dTotal != 100) {

        alert("Sum of all the share percentage should be equal to 100.");
        objControl.value = 0;        
        return false;

    }

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
    var sSelecedValue = ObjCurRecord.value;
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var ObjRecord;
    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1; i < objGrid.children.length; i++) {
        ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];

        if (i != iRowOfSelectControl) {
            if (ObjRecord.type == "text") {
                if (sSelecedValue.toUpperCase() == ObjRecord.value.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    ObjCurRecord.selectedIndex = 0;
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
    return true;
}
//To Check Repair Order no With Last Repair Order no
function CheckRepairOrderNoWithLastOrderNo(ObjCurrentRepirOrderNo) {

    var ObjControl = null;
    if (ObjCurrentRepirOrderNo.value == "") return;
    
    // Get Last Repair Order no
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtLastRepairOrderNo");
    if (ObjControl != null) 
    {
        if (ObjControl.value == ObjCurrentRepirOrderNo.value) 
        {

            alert("Repair Order No. is already used in previous warranty claim, Please enter another Repair Order No. !");
            ObjCurrentRepirOrderNo.value = '';
            return false;    
        }
    }
    
    
}