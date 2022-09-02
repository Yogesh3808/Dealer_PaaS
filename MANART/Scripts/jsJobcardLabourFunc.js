//****************Labour Function*************************
// To Get Labour Id which are previously selected by user.
function GetPreviousSelectedLabourIDOnJobcard() {
    var objRow;
    var LabourIds = "";
    var LabourId = "";
    var EstDtlId = "";
    var lblLabourId
    // get grid object
    //var objGrid = objNewLabourLabel.parentNode.parentNode.parentNode.parentNode;
    var objGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
    if (objGrid == null) return LabourIds;

    for (var i = 1; i < objGrid.rows.length; i++) {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Lable of Labour ID 
        lblLabourId = objGrid.rows[i].children[1].childNodes[1];
        var lblLabTag = objGrid.rows[i].children[8].childNodes[1];
        var lblLabCode = objGrid.rows[i].children[2].childNodes[3];
        var lblEstID = objGrid.rows[i].children[12].childNodes[3];
        //Get LabourId;
        LabourId = lblLabourId.value;
        EstDtlId = lblEstID.value.trim();
        //if (LabourId != "0" && LabourId != "" && lblLabCode.value != "999999") {
        if (EstDtlId == "0" || EstDtlId == "") {
            if (LabourId != "0" && LabourId != "" && lblLabCode.value.toString().substr(0, 5) != "MTIMI") {
                //LabourIds = LabourIds + LabourId + ",";
                LabourIds = LabourIds + LabourId.trim() + "<--" + lblLabTag.value.trim() + "<--" + lblEstID.value.trim() + ",";
            }
        }
        else {
            if (LabourId != "0" && LabourId != "") {
                //LabourIds = LabourIds + LabourId + ",";
                LabourIds = LabourIds + LabourId.trim() + "<--" + lblLabTag.value.trim() + "<--" + lblEstID.value.trim() + ",";
            }
        }
    }
    LabourIds = LabourIds.substring(0, (LabourIds.lastIndexOf(",")));
    return LabourIds;
}

function ShowMultiLabourMaster(objNewLabourLabel, sDealerId) {
    var LabourDetailsValue;
    var sModelGroupID, sEstmtID,sModBasCatID;
    var PcontainerName = '';
    var sJobtype = "";
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //sModelGroupID = document.getElementById(PcontainerName + "txtModelGroupID").value;
    sModelGroupID = "1";
    var txtDocDate = document.getElementById('ContentPlaceHolder1_txtDocDate_txtDocDate');
    if (txtDocDate != null)
        if (txtDocDate.value == "") {
            alert("Please enter Jobcard date.");
            txtDocDate.focus();
            return false;
        }
    var drpJobType = document.getElementById("ContentPlaceHolder1_drpJobType");
    if (drpJobType != null) {        
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }
    var drpEstimate = document.getElementById('ContentPlaceHolder1_DrpEstimate');
    sEstmtID = 0;
    if (drpEstimate != null && dGetValue(drpEstimate.selectedIndex) > 0) {
        sEstmtID = dGetValue(drpEstimate.options[drpEstimate.selectedIndex].value);
    }
    if (sJobtype == 0) {
        alert('Please Select Jobcard Type !');
        return false;
    }
    if (sJobtype == 2 && sEstmtID == 0) {
        alert('Please Select Estimate !');
        return false;
    }
    debugger;
    var txtModCatIDBasic = document.getElementById('ContentPlaceHolder1_txtModCatIDBasic');
    sModBasCatID = txtModCatIDBasic.value;
    var sWarrantyLab = "N";
    var sAMCLab = "N";
    var txtWarrantyTag = document.getElementById('ContentPlaceHolder1_txtWarrantyTag');
    var txtAggregate = document.getElementById('ContentPlaceHolder1_txtAggregate');
    var txtAMCChk = document.getElementById('ContentPlaceHolder1_txtAMCChk');
    if (txtWarrantyTag.value.trim() != "N" || txtAggregate.value.trim() == "G" || sJobtype == "12" || sJobtype == "7" || sJobtype == "16" || sJobtype == "13") sWarrantyLab = "Y";
    if (txtAMCChk.value.trim() == "Y" && sJobtype == "5") sAMCLab = "Y";

    var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
    var sCustTaxTag = hdnCustTaxTag.value; 

    var hdnISDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
    var sDocGST = hdnISDocGST.value;

    var sSelectedLabourID = GetPreviousSelectedLabourIDOnJobcard();
    //var hdnSelectedLabourID = document.getElementById("ContentPlaceHolder1_hdnSelectedLabourID");
    //var sSelectedLabourID = hdnSelectedLabourID.value;
    LabourDetailsValue = window.showModalDialog("../Common/frmSelectMultiLabour.aspx?DealerID=" + sDealerId + "&ModelGroupID=" + sModelGroupID + "&SelectedLabourID=" + sSelectedLabourID + "&SelectedEstmtID=" + sEstmtID + "&ModBasCatID=" + sModBasCatID + "&WarrantyLab=" + sWarrantyLab + "&CustTaxTag=" + sCustTaxTag + "&Jobtype=" + sJobtype + "&sDocGST=" + sDocGST + "&RepairOrderDate=" + txtDocDate.value + "&AMCLab=" + sAMCLab, "List", "scrollbars=no;resizable=no;dialogHeight:600px;dialogWidth:1000px;");
    if (LabourDetailsValue != null) {
        SetMultiLabourDetails(objNewLabourLabel, LabourDetailsValue);

    }
}

function WEnableRow(objGridRow) {
    eobjRow = objGridRow.parentNode.parentNode.rowIndex;

    var LbrRowVal = objGridRow.parentNode.childNodes[1].value;
    //if (LbrRowVal == "999999") {
    if (LbrRowVal.toString().substr(0, 5) == "MTIMI") {
        objGridRow.parentNode.parentNode.childNodes[3].childNodes[0].readOnly = false;
        objGridRow.parentNode.parentNode.childNodes[4].childNodes[0].readOnly = false;
        objGridRow.parentNode.parentNode.childNodes[5].childNodes[0].readOnly = false;
        objGridRow.parentNode.parentNode.childNodes[6].childNodes[0].readOnly = false;
    }
    else if (LbrRowVal.toString().substr(0, 5) == "MTICC") {        
        objGridRow.parentNode.parentNode.childNodes[5].childNodes[0].readOnly = false;
        objGridRow.parentNode.parentNode.childNodes[6].childNodes[0].readOnly = false;
    }


}
//SetLabourDetails
//function SetMultiLabourDetails(objAddNewControl, LabourValue) {
//    var gridView = null;
//    gridView = document.getElementById('ContentPlaceHolder1_LabourDetailsGrid');
//    if (gridView == null) return;
//    var iColCnt = 1;
//    var rows = gridView.rows;
//    var objRow;
//    var iCnt = 0;
//    var iStartRowCnt = 0;
//    var LabourCodeWith99;
//    var LabourCodeSVC;
//    iStartRowCnt = objAddNewControl.parentNode.parentNode.rowIndex;

//    for (iRowCnt = iStartRowCnt; iRowCnt < rows.length; iRowCnt++)
//     {
//        objRow = gridView.children[0].rows[iRowCnt].childNodes;
//        iColCnt = 1;
//        if (iCnt == LabourValue.length)
//         {
//            iColCnt = iColCnt + 1;
//            objRow[iColCnt].children[0].style.display = "";       // Show New Labour button next button
//            var objNewLabour = objRow[iColCnt].children[1];
//            if (objNewLabour != null) {
//                if (objNewLabour.value != "") {
//                    objRow[iColCnt].children[0].style.display = "none";
//                }
//            }  
//            SetLabourRecordCount();
//            return;
//        }

//        //Set LabourId;
//        objRow[iColCnt].childNodes[0].value = LabourValue[iCnt][0];
//        iColCnt = iColCnt + 1;

//        //Set Labour Code
//        LabourCodeWith99 = LabourValue[iCnt][1];
//        objRow[iColCnt].children[1].value = LabourCodeWith99;
//        objRow[iColCnt].children[1].style.display = "";
//        objRow[iColCnt].children[1].readOnly = true;
//        objRow[iColCnt].children[0].style.display = "none";       // Hide New Labour button
//        iColCnt = iColCnt + 1;

////        if (LabourCodeWith99 == "999999") {           
////            objRow[iColCnt].childNodes[0].readOnly = false;                    
////        }
//        // check Labour Code Contain last 2 Char as 99 then allo w user to enter man hrs.
//        LabourCodeSVC = LabourCodeWith99.trim();
//        LabourCodeWith99 = LabourCodeWith99.substring(4);

//        //SetLabourName
//        objRow[iColCnt].childNodes[0].style.display = "";
//        if (LabourCodeWith99.trim() != "99" && LabourCodeSVC != "999991") {
//            objRow[iColCnt].childNodes[0].value = LabourValue[iCnt][2];
//            objRow[iColCnt].childNodes[0].readOnly = true;
//        }
//        if (LabourCodeWith99.trim() != "99" && LabourCodeSVC!="999991") {
//            //SetManHrs
//            iColCnt = iColCnt + 1;
//            objRow[iColCnt].childNodes[0].value = LabourValue[iCnt][3];
//            objRow[iColCnt].childNodes[0].readOnly = true;
//            //SetRate
//            iColCnt = iColCnt + 1;
//            objRow[iColCnt].childNodes[0].value = LabourValue[iCnt][4];
//            objRow[iColCnt].childNodes[0].readOnly = true;

//            //Labour Amount
//            iColCnt = iColCnt + 1;
//            objRow[iColCnt].children[0].value = LabourValue[iCnt][5];
//            objRow[iColCnt].children[0].focus();
//            //objRow[iColCnt].children[1].value = LabourValue[iCnt][5];
//            objRow[iColCnt].childNodes[0].readOnly = true;

//            // Add line level Labour amount To Total Labour Amount
//            AddLabourTotalToClaimAmount(null, objRow[iColCnt].children[0]);

//        }
//        else {
//            objRow[iColCnt].childNodes[0].value = LabourValue[iCnt][2];
//            objRow[iColCnt].childNodes[0].readOnly = false;
//            iColCnt = iColCnt + 1;

//            objRow[iColCnt].childNodes[0].readOnly = false;
//            //SetRate
//            iColCnt = iColCnt + 1;
//            objRow[iColCnt].childNodes[0].value = LabourValue[iCnt][4];
//            objRow[iColCnt].childNodes[0].readOnly = true;


//        }

//        iCnt = iCnt + 1;
//    }

//}

// Set Total Parts Record Count
function SetLabourRecordCount() {
    var ObjGrid;
    var iRecordCnt = 0;
    ObjGrid = document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
    if (ObjGrid == null) return;
    var iLabourID = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        //        ObjGrid.rows[i].cells[0].outerText = "";
        //        ObjGrid.rows[i].cells[0].outerText = i;

        //Set iLabourID
        iLabourID = dGetValue(ObjGrid.rows[i].cells[1].children[0].value);
        if (iLabourID != 0) {
            iRecordCnt = iRecordCnt + 1;
        }
    }
    // To calculate Total Labour Count
    var ObjLabourCount = document.getElementById("ContentPlaceHolder1_lblLabourRecCnt");
    if (ObjLabourCount != null) {
        ObjLabourCount.innerText = iRecordCnt;
    }
}

function calculateLabourTotal(event, objGridRow) {
    //alert("Here" );    

    //var LTotal = 0;
    debugger;
    var objLCode = objGridRow.parentNode.parentNode.childNodes[3].childNodes[3];    
    var sFirstFiveDigit = objLCode.value.toString().substr(0, 5);
    var LCurrRate = dGetValue(objGridRow.parentNode.parentNode.childNodes[7].childNodes[1].value);
    var LPaidRate = dGetValue(objGridRow.parentNode.parentNode.childNodes[7].childNodes[3].value);
    var LWarrRate = dGetValue(objGridRow.parentNode.parentNode.childNodes[7].childNodes[5].value);    

    var objtxtLabTag = objGridRow.parentNode.parentNode.childNodes[9].childNodes[1];
    
    var objdrpLabWarr = objGridRow.parentNode.parentNode.childNodes[10].childNodes[1];
    
    var sLabTag = objtxtLabTag.value.trim();
    var sWarrTag = objdrpLabWarr.value.trim();

    if (sLabTag == "D" && sWarrTag == "G" && sFirstFiveDigit != "MTIDC" && LCurrRate != LWarrRate)
    {
        objGridRow.parentNode.parentNode.childNodes[7].childNodes[1].value = LWarrRate;
    }
    if (sLabTag == "D" && sWarrTag == "N" && sFirstFiveDigit != "MTIDC" && LCurrRate != LPaidRate) {
        objGridRow.parentNode.parentNode.childNodes[7].childNodes[1].value = LPaidRate;
    }

    
    //if (sFirstFiveDigit != "MTIOU") {
    if (sFirstFiveDigit != "MTIOU" && sFirstFiveDigit != "MTIMI" && sFirstFiveDigit != "MTICC") {
        var LHRs = dGetValue(objGridRow.parentNode.parentNode.childNodes[6].childNodes[1].value);
        var LRate = dGetValue(objGridRow.parentNode.parentNode.childNodes[7].childNodes[1].value);
        LTotal = LHRs * LRate;
        objGridRow.parentNode.parentNode.childNodes[6].childNodes[0].value = RoundupValue(LHRs);
        objGridRow.parentNode.parentNode.childNodes[7].childNodes[0].value = RoundupValue(LRate);
        objGridRow.parentNode.parentNode.childNodes[8].childNodes[1].value = RoundupValue(LTotal);
    }
    else {        
        var objtxtLSubletAmt = objGridRow.parentNode.parentNode.childNodes[15].childNodes[1];
        var txtTotal = objGridRow.parentNode.parentNode.childNodes[8].childNodes[1];
        var txtLHrs = objGridRow.parentNode.parentNode.childNodes[6].childNodes[1]
        //txtTotal.value = objtxtLSubletAmt.value;
        if (sFirstFiveDigit == "MTIOU") txtTotal.value = objtxtLSubletAmt.value;
        var LRate = dGetValue(objGridRow.parentNode.parentNode.childNodes[7].childNodes[1].value);
        txtLHrs.value = RoundupValue(dGetValue(txtTotal.value) / LRate);
    }

    // Add line level Labour amount To Total Labour Amount   
    AddAmountToTotal();
}

//function AddLabourTotalToClaimAmount(event, ObjAmtControl) {
//    var dOrgLineLabourAmt;
//    var dLineLabourAmt;
//    var dDiffOfLabourAmt;

//    if (ObjAmtControl.value == "") {
//        alert("Please enter the record");
//        //event.returnValue=false     
//        //ObjControl.focus();
//        return false;
//    }
//    // Calculate Line Level Labour amount    
//    //*******************************
//    var objRow = ObjAmtControl.parentNode.parentNode.childNodes;

//    // Get Line Level Old Labour amount  
//    dOrgLineLabourAmt = dGetValue(objRow[6].children[1].value);

//    // Calculate Line Level Labour Amt
//    dLineLabourAmt = dGetValue(objRow[6].children[0].value);
//    dLineLabourAmt = RoundupValue(dLineLabourAmt);

//    if (dLineLabourAmt == 0) return;
//    objRow[6].children[1].value = RoundupValue(dLineLabourAmt);
//    if (dOrgLineLabourAmt == 0) {
//        dDiffOfLabourAmt = dLineLabourAmt;
//    }
//    else if (dOrgLineLabourAmt == dLineLabourAmt) {
//        dDiffOfLabourAmt = 0;
//    }
//    else {

//        dDiffOfLabourAmt = (dLineLabourAmt - dOrgLineLabourAmt);
//    }
//    AddAmountToTotal("Labour", dDiffOfLabourAmt);
//}
////Calculate Total For Labour Details
//function CalculateLineTotalForLabour(event, ObjQtyControl) {
//    var dOrgLineLabourAmt;
//    var dLineLabourAmt;
//    var dDiffOfLabourAmt;
//    var Rate

//    if (CheckTextboxValueForNumeric(event, ObjQtyControl,true ) == false) {
//        return;
//    }
//    // Calculate Line Level Labour amount        
//    var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
//    // Get Rate
//    Rate = dGetValue(objRow[5].childNodes[0].value);

//    // Get Line Level Labour amount    before calculation    
//    dOrgLineLabourAmt = dGetValue(objRow[6].childNodes[0].value);


//    // Calculate Line Level Labour Amt
//    dLineLabourAmt = dGetValue(ObjQtyControl.value) * Rate;
//    dLineLabourAmt = RoundupValue(dLineLabourAmt);
//    objRow[6].childNodes[0].value = dLineLabourAmt;


//    if (dLineLabourAmt == 0) return;
//    if (dOrgLineLabourAmt == 0) {
//        dDiffOfLabourAmt = dLineLabourAmt;
//    }
//    else if (dOrgLineLabourAmt == dLineLabourAmt) {
//        dDiffOfLabourAmt = 0;
//    }
//    else {
//        dDiffOfLabourAmt = (dLineLabourAmt - dOrgLineLabourAmt);
//    }
//    AddAmountToTotal("Labour", dDiffOfLabourAmt);
//}
// When user Click to delete the record then reduce the amount 
function SelectDeleteCheckboxForLabour(ObjChkDelete, ObjtxtWONo) {

    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    var dDiffOfLabourAmt = 0;

    // Get Line Level Labour amount
    if (ObjtxtWONo.value.trim() != "")
    {
        alert("Labor can not delete becuase Work Order Challan already generated.");
        return false;
    }
    var dOrgLineAmt = dGetValue(ObjChkDelete.parentNode.parentNode.parentNode.children[6].childNodes[0].value);

    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            dDiffOfLabourAmt = 0 - dOrgLineAmt
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        dDiffOfLabourAmt = dOrgLineAmt
    }
    //AddAmountToTotal("Labour", dDiffOfLabourAmt);
    AddAmountToTotal();
}

// When user Click on Cancel of Labour Grid then clear the value of row
function ClearRowValueForLabourWarranty(event, objCancelControl) {



    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjControl;
    var TotalAmount;
    //objAddNewControl.style.display="none";


    //Set LabourId;        
    objRow[1].childNodes[0].value = '';

    //SetLabourNo
    objRow[2].children[0].style.display = "";
    objRow[2].children[1].value = '';
    objRow[2].children[1].style.display = "none";

    //SetLabourName
    objRow[3].childNodes[0].value = '';
    objRow[3].childNodes[0].style.display = "";

    //Set Man Hrs    
    objRow[4].childNodes[0].value = '';

    //SetFoBRate        
    objRow[5].childNodes[0].value = '';

    //Total    
    TotalAmount = dGetValue(objRow[6].childNodes[0].value);
    objRow[6].childNodes[0].value = '';
    objRow[6].childNodes[1].value = '';
    TotalAmount = (0 - TotalAmount);
    //AddAmountToTotal("Labour", TotalAmount);
    AddAmountToTotal();

    //Set Jobcode
    ObjControl = objRow[7].childNodes[0];
    if (ObjControl != null) ObjControl.selectedIndex = 0;


    //Set VECV Percentage
    var ObjControl = objRow[8].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }

    ObjControl = objRow[9].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }
    ObjControl = objRow[10].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }

    //SetNewLabel Display        
    ObjControl = objRow[11].children[1];
    if (ObjControl != null) ObjControl.style.display = "";

    // To Calculate Labour Count
    SetLabourRecordCount();
}

//---------------------------------------------

function ClearRowValueForLabour(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjControl;
    var TotalAmount;
    //objAddNewControl.style.display="none";

    //Set LabourId;        
    objRow[1].childNodes[0].value = '';

    //SetLabourNo
    objRow[2].children[0].style.display = "";
    objRow[2].children[1].value = '';
    objRow[2].children[1].style.display = "none";

    //SetLabourName
    objRow[3].childNodes[0].value = '';
    objRow[3].childNodes[0].style.display = "none";

    //Set Man Hrs    
    objRow[4].childNodes[0].value = '1';

    //SetFoBRate        
    objRow[5].childNodes[0].value = '';

    //Total    
    TotalAmount = dGetValue(objRow[6].childNodes[0].value);
    objRow[6].childNodes[0].value = '';
    objRow[6].childNodes[1].value = '';
    TotalAmount = (0 - TotalAmount);
    //AddAmountToTotal("Labour", TotalAmount);
    AddAmountToTotal();

    //Set Jobcode
    ObjControl = objRow[7].childNodes[0];
    if (ObjControl != null) ObjControl.selectedIndex = 0;


    //Set VECV Percentage
    var ObjControl = objRow[8].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }

    ObjControl = objRow[9].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }
    ObjControl = objRow[10].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }

    //SetNewLabel Display        
    ObjControl = objRow[11].children[1];
    if (ObjControl != null) ObjControl.style.display = "none";

    // To Calculate Labour Count
    SetLabourRecordCount();
}
//Check Labor description value is select 
function CheckLbrDescSelected(eve, objcontrol) {
    ////debugger;
    if (CheckForComboValue(eve, objcontrol, false) == true) {
        //if (CheckAddLbrComboValueAlreadySelectInGrid(objcontrol) == false)
        //    return false;
        if (CheckAddLbrComboValueAlreadyUsedInGrid(objcontrol) == false)
            return false;
    }
    else {
        ClearRowValue(event, ObjControl);
    }
}
//Check Labor Description Is already Used in Grid
function CheckLbrDescAlreadyUsedInGrid(event, Objcontrol) {
    //debugger;
    if (CheckTextValueAlreadyUsedInGrid(event, Objcontrol) == true) {
        //var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[0].innerText);
        var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[1].innerText);
        var ObjRecord;
        var objGrid = Objcontrol.parentNode.parentNode.parentNode;
        for (i = 1; i < objGrid.children.length; i++) {
            //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
            ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

            if (dGetValue(i - 1) == iRowOfSelectControl) {
                if (Objcontrol.value.trim() == "") {
                    alert("Please Enter Additional Labor Description")
                    Objcontrol.focus();
                    return false;
                }
            }
        }
    }
}

//When User Change Complaint
function OnLbrDescValueChange(ObjCombo, txtboxId) {
    if (OnComboValueChange(ObjCombo, txtboxId) == false) {

    }
}

//check Combo Value already select in previous / next row in grid
function CheckAddLbrComboValueAlreadyUsedInGrid(ObjCurRecord) {
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
        if (dGetValue(i - 1) != iRowOfSelectControl) {
            if (ObjRecord.selectedIndex != 0) {
                if (sSelecedValue != "NEW") {
                    if (sSelecedValue == ObjRecord.options[ObjRecord.selectedIndex].text) {
                        debugger;
                        alert("Record is already selected at line No." + dGetValue(i - 1));
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
function CheckAddLbrComboValueAlreadySelectInGrid(ObjCurRecord) {
    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
    //debugger;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
    var iColumnIndexOfControl = parseInt(ObjCurRecord.parentNode.cellIndex);
    var ObjRecord;

    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 2 ; i < objGrid.rows.length; i++) {
        ObjRecord = objGrid.rows[i].cells[iColumnIndexOfControl].children[0];

        var objLCode = objGrid.rows[i].cells[2].childNodes[3];
        var sLstTwoDigit = objLCode.value.toString().substr(objLCode.value.toString().length - 2, 2);
        var sFirstFiveDigit = objLCode.value.toString().substr(0, 5);

        if (dGetValue(i - 1) != iRowOfSelectControl) {

            if (dGetValue(i - 1) < iRowOfSelectControl) {
                if (sSelecedValue != "NEW") {
                    //if (ObjRecord.options[ObjRecord.selectedIndex].text == "--Select--" && (sLstTwoDigit == "99" || sFirstFiveDigit == "33333" || sFirstFiveDigit == "44444" || sFirstFiveDigit == "55555")) {
                    if (ObjRecord.options[ObjRecord.selectedIndex].text == "--Select--" && (sFirstFiveDigit == "MTIMI")) {
                        alert("Please select Record at line No." + dGetValue(i - 1));
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