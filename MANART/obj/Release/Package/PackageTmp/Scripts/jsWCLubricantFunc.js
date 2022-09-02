//**************Lubricant function***********************
//To Show Part Master
function ShowSpWPFLubricant(objNewPartLabel, sDealerId) {
    var txtDocDate = document.getElementById('ContentPlaceHolder1_txtFailureDate_txtDocDate');
    if (txtDocDate != null)
        if (txtDocDate.value == "") {
            alert("Please enter Failure date.");
            txtDocDate.focus();
            return false;
        }
    var PartDetailsValue;
    var sClaimtype = "";
    var EstID = "";
    debugger;
    var sSelectedPartID = GetPreviousSelectedLubIDInWarranty();
    //var hdnSelectedPartID = document.getElementById("ContentPlaceHolder1_hdnSelectedPartID");
    //var sSelectedPartID = hdnSelectedPartID.value;
    var drpClaimType = window.document.getElementById('ContentPlaceHolder1_DropClaimTypes');
    if (drpClaimType == null) return;
    if (drpClaimType.selectedIndex == 0) {
        alert('Please Select Claim Type !');
        return false;
    }
    else {
        sClaimtype = drpClaimType.options[drpClaimType.selectedIndex].value;
    }

    var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
    var sCustTaxTag = hdnCustTaxTag.value;

    var hdnISDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
    var sDocGST = hdnISDocGST.value;

    PartDetailsValue = window.showModalDialog("../Common/frmSelectMultipart1.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtDocDate.value + "&EstID=" + EstID + "&CustTaxTag=" + sCustTaxTag + "&sDocGST=" + sDocGST + "&sDocType=EXO", "List", "dialogHeight: 550px; dialogWidth: 700px;");
}

// To Get Part Id which are previously selected by user.
function GetPreviousSelectedLubIDInWarranty() {
    var objRow;
    var PartIds = "";
    var PartId = "";
    var txtPartId;
    // get grid object    
    debugger;
    var objGrid = window.document.getElementById("ContentPlaceHolder1_LubricantDetailsGrid");
    if (objGrid == null) return PartIds;
    for (var i = 1; i < objGrid.rows.length; i++) {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Textbox of the Part ID
        txtPartId = objGrid.rows[i].children[1].childNodes[3];

        //Get PartId;
        PartId = dGetValue(txtPartId.value);
        if (PartId != "0") {
            PartIds = PartIds + PartId + ",";
        }
    }
    PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

    return PartIds;
}
//Calculate Total For Lubricant Details
function CalculateLineTotalForLubricant(event, ObjControl) {
    var dOrgLineLubricantAmt;
    var dLineLubricantAmt;
    var dDiffOfLubricantAmt;
    
    if (CheckTextboxValueForNumeric(event, ObjControl, false, false) == false) return false;;   

    var objRow = ObjControl.parentNode.parentNode.childNodes;

    var dQty = 0;
    var dRate = 0;
    dQty = dGetValue(objRow[3].children[0].value);
    dRate = dGetValue(objRow[5].children[0].value);

    var LubLocID = dGetValue(objRow[19].childNodes[1].value);
    if (LubLocID == 0) {
        alert("Select Pouring Location.");
        ObjControl.value = '';
        return;
    }
    var lubCapasityQty = 0;

    if (LubLocID != 0) {
        ////debugger;
        var objLubLocCapasityID = objRow[20].childNodes[1];
        lubCapasityQty = dGetValue(objLubLocCapasityID[objLubLocCapasityID.selectedIndex].text);
        lubCapasityQty = RoundupValue(lubCapasityQty + dGetValue(lubCapasityQty * 0.05));
    }
    if (dQty > lubCapasityQty) {
        alert("Warranty Qty should not be greater than pouring location capasity.");
        ObjControl.value = '';
        return;
    }

    ObjControl.value = RoundupValue(ObjControl.value);

    // Get Line Level Old Lubricant amount
    dOrgLineLubricantAmt = dGetValue(objRow[6].children[1].value);
       
    // Calculate Line Level Lubricant Amt
    dLineLubricantAmt = dQty * dRate;

    //if (dLineLubricantAmt == 0) return;
    objRow[6].children[0].value = RoundupValue(dLineLubricantAmt);
    objRow[6].children[1].value = RoundupValue(dLineLubricantAmt);

    if (dOrgLineLubricantAmt == 0) {
        dDiffOfLubricantAmt = dLineLubricantAmt;
    }
    else if (dOrgLineLubricantAmt == dLineLubricantAmt) {
        dDiffOfLubricantAmt = 0;
    }
    else {
        dDiffOfLubricantAmt = (dLineLubricantAmt - dOrgLineLubricantAmt);
    }
    AddAmountToTotal("Lubricant", dDiffOfLubricantAmt);
    SetLubricantRecordCount();
    return true;
}

//Check Lubricant Description Is Entered
function CheckLubricantDescUsed(eve, objcontrol) {
    if (CheckTextValueAlreadyUsedInGrid(eve, objcontrol) == false) {
        ClearRowValueForLubricant(eve, objcontrol)
        return false;
    }
}


//Check Lubricant value is select
function CheckLubricantSelected(eve, objcontrol) {
    if (CheckForComboValue(eve, objcontrol) == true) {
        if (CheckComboValueAlreadySelectInGrid(objcontrol) == false)
            return false;
        if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
            return false;
        //SetLubricantRecordCount();
        SetLubricantLineDetails(objcontrol);
    }
    else {
        ClearRowValueForLubricant(null, objcontrol);
    }
    
    
}

//Set function  To Set UOM and Max Qty
function SetLubricantLineDetails(ObjLubType) {
    
    var objRow = ObjLubType.parentNode.parentNode.childNodes;
    if (ObjLubType.options[ObjLubType.selectedIndex].text == "NEW") // If Others
    {
        ObjLubType.style.display = "none";
        objRow[1].children[1].style.display = "";
        objRow[2].children[0].value = "1.00";
        objRow[3].children[0].value = "L";
        objRow[4].children[0].value = "1.00";
        objRow[5].children[0].value = "1.00";
    }
    else 
    {
        var objLubData = objRow[1].children[2];
        objLubData.selectedIndex = ObjLubType.selectedIndex

        var sLubData = objLubData.options[objLubData.selectedIndex].text;
        
        //changes related to Display Lubricant Rate also in Warranty claim creation  
        
        //Set Max Qty
        objRow[2].children[1].value = sLubData.substring(0, sLubData.indexOf('#'));
         
        //Set UOM
        //objRow[3].children[0].value = sLubData.substring(sLubData.indexOf('#') + 1)
        objRow[3].children[0].value = sLubData.substring(sLubData.indexOf('#') + 1, sLubData.indexOf('%'))
        
        //set Rate
        objRow[4].children[0].value = sLubData.substring(sLubData.indexOf('%')+1)
        
        if(ObjLubType.options[ObjLubType.selectedIndex].text != "NEW")
            objRow[3].children[0].readOnly = true;
            objRow[4].children[0].readOnly = true;

        if (objRow[2].children[0].value != "") 
        {
            CheckLubQtyWithMaxQty(null, objRow[2].children[0]);
        }
    }

}

// Check Lubricant Qty
function CheckLubQtyWithMaxQty(event, ObjLubQty) {

    if (CheckTextboxValueForNumeric(event, ObjLubQty,false  ) == false) return false;

    var objRow = ObjLubQty.parentNode.parentNode.childNodes;

    var ObjLubType = objRow[1].childNodes[0]

    var objLubData = objRow[1].children[2];
    objLubData.options[objLubData.selectedIndex].value = ObjLubType.options[ObjLubType.selectedIndex].value;
    var sLubData = objLubData.options[objLubData.selectedIndex].text;
    
    //Set Max Qty
    objRow[2].children[1].value = sLubData.substring(0, sLubData.indexOf('#'));
    
    var dMaxQty = dGetValue(objRow[2].children[1].value);
//    if (dMaxQty == 0) {
//        SetLubricantLineDetails(objRow[1].children[2]);
//    }
    var dLubty = dGetValue(ObjLubQty.value);

    if (sLubData == "NEW")
        dMaxQty = dLubty;
        
    ObjLubQty.value = RoundupValue(dLubty);
    if (dLubty > dMaxQty && sLubData != "--Select--" && ObjLubType.options[ObjLubType.selectedIndex].text != "NEW") {
        alert("Please Enter the Quantity Less Than Or Equal To Max Allowed Qty '" + dMaxQty + "'!.");
        ObjLubQty.value = '';
        ObjLubQty.focus();
        return false;
    }
    //
    var dRate = dGetValue(objRow[4].children[0].value);
    if (dRate != 0) {
        // Calculate Amount
        CalculateLineTotalForLubricant(event, ObjLubQty)
    }
}
// Set Total Lubricant Record Count
function SetLubricantRecordCount() {
    var ObjGrid;
    var iRecordCnt = 0;

    ObjGrid = document.getElementById("ContentPlaceHolder1_LubricantDetailsGrid");
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
    // To calculate Lubricant Count
    var ObjLubricantCount = document.getElementById("ContentPlaceHolder1_lblLubricantRecCnt");
    if (ObjLubricantCount != null) {
        ObjLubricantCount.innerText = iRecordCnt;
    }
}
// When user Click to delete the record then reduce the amount 
function SelectDeleteCheckboxForLubricant(ObjChkDelete) {

    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    var dDiffOfLubricantAmt = 0;
    debugger;
    // Get Line Level Lubricant amount
    var dOrgLineAmt = dGetValue(ObjChkDelete.parentNode.parentNode.parentNode.children[5].childNodes[1].value);

    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            dDiffOfLubricantAmt = 0 - dOrgLineAmt
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        dDiffOfLubricantAmt = dOrgLineAmt
    }
    AddAmountToTotal("Lubricant", dDiffOfLubricantAmt);

}

// When user Click on Cancel of Lubricant Grid then clear the value of row
function ClearRowValueForLubricant(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjControl;
    var TotalAmount;

    //Set Lubricant Type
    objRow[1].children[0].selectedIndex = 0;
    objRow[1].children[0].style.display = "";
    objRow[1].children[1].value = "";
    objRow[1].children[1].style.display = "none";
    objRow[1].children[2].selectedIndex = 0;

    //SetQuantity
    objRow[2].childNodes[0].value = '';

    //Set UOM
    objRow[3].childNodes[0].value = '';

    //Set Rate
    objRow[4].childNodes[0].value = '';

    //Total
    TotalAmount = dGetValue(objRow[5].children[0].value);
    objRow[5].children[0].value = '0.00';
    objRow[5].children[1].value = '';
    TotalAmount = (0 - TotalAmount);
    AddAmountToTotal("Lubricant", TotalAmount);


    // Job code    
    objRow[6].childNodes[0].selectedIndex = 0;



    // Vecv Percentage
    objRow[7].childNodes[0].value = '';

    // Dealer Percentage
    objRow[8].childNodes[0].value = '';

    // Cust Percentage
    objRow[9].childNodes[0].value = '';

    //SetNewLabel Display        
    ObjControl = objRow[10].children[1];
    if (ObjControl != null) ObjControl.style.display = "none";

    // To Calculate Lubricant Count
    var ObjLubricantCount = document.getElementById("ContentPlaceHolder1_lblLubricantRecCnt");
    if (ObjLubricantCount != null) {
        if (ObjLubricantCount.innerText != null) {
            ObjLubricantCount.innerText = dGetValue(ObjLubricantCount.innerText) - 1;
        }
    }
    SetLubricantRecordCount();
}
// Set Focus to Lubricant
function SetLubricantFocus(ObjControl) {
    var objRow = ObjControl.parentNode.parentNode.childNodes;
    if (objRow[1].children[0].selectedIndex == 0) {
        objRow[1].children[0].focus();
    }
    else if (objRow[1].children[0].selectedIndex == 1) {
    if (objRow[1].children[1].style.display=="")
        if (objRow[1].children[1].value == "") {
            objRow[1].children[1].focus();
        }
    }
}
//---------------------------------------------

function SetLubCapasityOnLubLocationChange(ObjLubLoc, dLubCap) {
    var ParentCtrlID;
    var objLubCapControl;
    var Sup, Post;
    //debugger;

    //ParentCtrlID = ObjLubLoc.id.substring(0, ObjLubLoc.id.lastIndexOf("_"));
    Post = ObjLubLoc.id.substring(ObjLubLoc.id.lastIndexOf("_"), (ObjLubLoc.id).length);
    Sup = ObjLubLoc.id.substring(0, ObjLubLoc.id.lastIndexOf("_"));
    ParentCtrlID = Sup.substring(0, Sup.lastIndexOf("_"));

    objLubCapControl = document.getElementById(ParentCtrlID + "_" + dLubCap + Post);
    objLubCapControl.selectedIndex = ObjLubLoc.selectedIndex;

    return true;
}
