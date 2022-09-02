
////###########Part Function###############3
//Calculate Part Accepted Amount on Qty Changed         
function CalculatePartAcceptedAmtVecvPercentChanged(event, ObjVECVPercentControl) {
    //debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dRate
    var dClaimedQty = 0;
    var dAcceptedQty = 0;
    var dDeduction = 0;
    var dAmount = 0;
    ////debugger;
    if (CheckPercentageValue(event, ObjVECVPercentControl) == false) {
        return false;
    }
    //var objID = $('#' + ObjVECVPercentControl.id)
    var objID = window.document.getElementById(ObjVECVPercentControl.id);

    // Calculate Line Level Part amount

    //var objID = $('#' + ObjVECVPercentControl.id)
    var objID = window.document.getElementById(ObjVECVPercentControl.id);
    var objRow = objID.parentNode.parentNode;

    // Get Claimed Qty
    dClaimedQty = dGetValue(objRow.cells[10].children[0].innerHTML);

    // Get Rate
    dRate = dGetValue(objRow.cells[11].children[0].innerHTML);

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[12].children[0].innerHTML);

    // Get Accepted Qty
    dAcceptedQty = dGetValue(objRow.cells[13].children[0].value);

    ObjVECVPercentControl.value = RoundupValue(objID.value);
    
    // Get VECV Percentage
    var dVECVPercentage = dGetValue(objID.value);
    var dCustPercentage = dGetValue(objRow.cells[16].children[0].value);    
    if (dVECVPercentage > (100 - dGetValue(dCustPercentage))) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dGetValue(objRow.cells[15].children[0].value) + dCustPercentage));
        return false;
    }

    //Set Dealer Percentage
    objRow.cells[15].children[0].innerHTML = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    objRow.cells[15].children[0].value = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    var dDealerPercentage = dGetValue(objRow.cells[15].children[0].value);

    var dTotalPercentage = dVECVPercentage + dDealerPercentage + dCustPercentage;
    if (dTotalPercentage > 100) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dDealerPercentage + dCustPercentage));
        return false;
    }   
    //#############

    //Get deduction percentage
    dDeduction = 100 - dVECVPercentage;
    objRow.cells[17].children[0].innerHTML = dDeduction;
    objRow.cells[17].children[0].value = dDeduction;

    //Get Accepted Claim Amount
    dOrgPartAmt = dGetValue(objRow.cells[18].children[0].value);

    dAmount = dAcceptedQty * dRate;

    if (dDeduction == 0) {
        dAcceptedAmount = dAmount;
    }
    else {
        dAcceptedAmount = dAmount - dGetValue((dAmount * dDeduction) / 100);
    }
    objRow.cells[18].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[18].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgPartAmt == dAcceptedAmount) {
        dDiffOfPartAmt = 0;
    }
    else {
        dDiffOfPartAmt = (dClaimAmount - dOrgPartAmt) + (dAcceptedAmount - dClaimAmount);
    }
    //debugger;
    dPartTaxPer = dGetValue(objRow.cells[23].children[0].value);
    var dDiffTaxamt = 0;
    dDiffTaxamt = (dDiffOfPartAmt * dPartTaxPer / 100);
    AddAmountToJobTotal("Part", dDiffOfPartAmt, dDiffTaxamt);

}

//Calculate Part Accepted Amount on Qty Changed
function CalculatePartAcceptedAmtQtyChangedNM(event, ObjQtyControl) {
    //debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dRate
    var dClaimedQty = 0;
    var dAcceptedQty = 0;
    var dDeduction = 0;
    var dAmount = 0;


    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false, false) == false) {
        alert(" You are rejecting the part, Please enter the Remarks !");
    }
    // Calculate Line Level Part amount
    //var objID = $('#' + ObjQtyControl.id)
    var objID = window.document.getElementById(ObjQtyControl.id);

    // Calculate Line Level Part amount
    var objRow = objID.parentNode.parentNode;
    // Get Claimed Qty
    dClaimedQty = dGetValue(objRow.cells[10].children[0].innerHTML);

    // Get Rate
    dRate = dGetValue(objRow.cells[11].children[0].innerHTML);

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[12].children[0].innerHTML);

    // Get Accepted Qty
    dAcceptedQty = dGetValue(ObjQtyControl.value);


    if (dAcceptedQty > dClaimedQty) {
        alert(" Accepted quantity should not be greater than claim quantity.");
        ObjQtyControl.value = dClaimedQty;
        ObjQtyControl.focus();
        event.returnValue = false
        return false;
    }

    //Get deduction percentage
    dDeduction = dGetValue(objRow.cells[17].children[0].value);
    dAutoDeduction = dGetValue(objRow.cells[22].children[0].value);
    dReqDeduction = dGetValue(objRow.cells[25].children[0].value);    
    //Get Accepted Claim Amount
    dOrgPartAmt = dGetValue(objRow.cells[18].children[0].value);

    dAmount = dAcceptedQty * dRate;
    //dAmount = dAcceptedQty * ((dRate * 100) / (100 - dReqDeduction));
    if (dDeduction == 0) {
        dAcceptedAmount = dAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);


        if (dAutoDeduction > 0)
            dAcceptedAmount = dGetValue(dAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else
            dAcceptedAmount = dAmount - dGetValue((dAmount * dDeduction) / 100);         
    }
    objRow.cells[18].children[0].innerHTML  = RoundupValue(dAcceptedAmount);
    objRow.cells[18].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgPartAmt == dAcceptedAmount) {
        dDiffOfPartAmt = 0;
    }
    else {
        dDiffOfPartAmt = (dClaimAmount - dOrgPartAmt) + (dAcceptedAmount - dClaimAmount);
    }
    //debugger;
    dPartTaxPer = dGetValue(objRow.cells[23].children[0].value);
    var dDiffTaxamt = 0;
    dDiffTaxamt = (dDiffOfPartAmt * dPartTaxPer / 100);
    AddAmountToJobTotal("Part", dDiffOfPartAmt, dDiffTaxamt);

}



//Calculate Part Accepted Amount on Qty Changed
function CalculatePartAcceptedAmtQtyChanged(event, ObjQtyControl) {
    //debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dRate
    var dClaimedQty = 0;
    var dAcceptedQty = 0;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;

    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;


    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false,false) == false) 
    {
        alert(" You are rejecting the part, Please enter the Remarks !"); 
    }
    // Calculate Line Level Part amount
    //var objID = $('#' + ObjQtyControl.id)
    var objID = window.document.getElementById(ObjQtyControl.id);
    var objRow = objID.parentNode.parentNode;
    // Get Claimed Qty
    dClaimedQty = dGetValue(objRow.cells[10].children[0].innerHTML);    

    // Get Rate
    dRate = dGetValue(objRow.cells[11].children[0].innerHTML);    

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[12].children[0].innerHTML);

    // Get Accepted Qty
    dAcceptedQty = dGetValue(ObjQtyControl.value);
   

    if (dAcceptedQty > dClaimedQty) {
        alert(" Accepted quantity should not be greater than claim quantity.");
        ObjQtyControl.value = dClaimedQty;
        ObjQtyControl.focus();
        event.returnValue = false
        return false;
    }

    //Get deduction percentage
    dDeduction = dGetValue(objRow.cells[17].children[0].innerHTML);
    dAutoDeduction = dGetValue(objRow.cells[22].children[0].innerHTML);
    dReqDeduction = dGetValue(objRow.cells[25].children[0].innerHTML);    
    //Get Accepted Claim Amount
    dOrgPartAmt = dGetValue(objRow.cells[18].children[0].innerHTML);

    dAmount = dAcceptedQty * dRate;
    //dAmount = dAcceptedQty * ((dRate * 100) / (100 - dReqDeduction));
    
    if (dDeduction == 0) {
        dAcceptedAmount = dAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);


        if (dAutoDeduction > 0)
            dAcceptedAmount = dGetValue(dAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else
            dAcceptedAmount = dAmount - dGetValue((dAmount * dDeduction) / 100);         
    }
    objRow.cells[18].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[18].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgPartAmt == dAcceptedAmount) {
        dDiffOfPartAmt = 0;
    }
    else {
        dDiffOfPartAmt = (dClaimAmount - dOrgPartAmt) + (dAcceptedAmount - dClaimAmount);
    }
    //debugger;
    dPartTaxPer = dGetValue(objRow.cells[23].children[0].value);
    var dDiffTaxamt = 0;
    dDiffTaxamt = (dDiffOfPartAmt * dPartTaxPer / 100);
    AddAmountToJobTotal("Part", dDiffOfPartAmt, dDiffTaxamt);

}


//Calculate Part Accepted Amount on Deduction Changed
function CalculatePartAcceptedAmtDeductionChanged(event, ObjDeductionCtrl) {
//function CalculatePartAcceptedAmtDeductionChanged(ObjDeductionCtrl) {
    //debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dRate
    var dClaimedQty = 0;
    var dAcceptedQty = 0;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;
    
    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;
    var dDiffOfPartAmt = 0;
    var dOrgPartAmt = 0;

    // Calculate Line Level Part amount
    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);

    // Calculate Line Level Part amount
    //var objRow = objID[0].parentNode.parentNode;
    var objRow = objID.parentNode.parentNode;

    //Get deduction percentage
    dDeduction = dGetValue(ObjDeductionCtrl.value);
    ObjDeductionCtrl.value = RoundupValue(ObjDeductionCtrl.value);

    if (CheckPercentageValue(event, ObjDeductionCtrl) == false) {
        return false;
    }

    dAutoDeduction = dGetValue(objRow.cells[22].children[0].value);
    dReqDeduction = dGetValue(objRow.cells[25].children[0].value);
    if (dGetValue(dDeduction) < dGetValue(dAutoDeduction)) {
        alert(dGetValue(dAutoDeduction) + "% Auto deduction already applied on this Part.\n Cannot reduce Deduction below Auto deduction percentage!");
        ObjDeductionCtrl.focus();       
        return false;
    }
    else if (dGetValue(dDeduction) < dGetValue(dReqDeduction)) {
    alert(dGetValue(dReqDeduction) + "% Request deduction already applied on this Part.\n Cannot reduce Deduction below Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < (dGetValue(dAutoDeduction) + dGetValue(dReqDeduction))) {
    alert(dGetValue(dAutoDeduction) + dGetValue(dReqDeduction) + "% Auto and Request deduction already applied on this Part.\n Cannot reduce Deduction below Auto and Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }

   

    // Get Claimed Qty
    dClaimedQty = dGetValue(objRow.cells[10].children[0].innerHTML);

    // Get Rate
    dRate = dGetValue(objRow.cells[11].children[0].innerHTML);

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[12].children[0].innerHTML);

    // Get Accepted Qty    
    dAcceptedQty = dGetValue(objRow.cells[13].children[0].value);

    //Get Deduction
    dDeduction = dGetValue(objRow.cells[17].children[0].value);
        
    //Get Accepted Claim Amount
    dOrgPartAmt = dGetValue(objRow.cells[18].children[0].value);
    dAmount = dAcceptedQty * dRate;
    //dAmount = dAcceptedQty * ((dRate * 100) / (100 - dReqDeduction));
    if (dDeduction == 0) {
        dAcceptedAmount = dAmount;
    }
    else {

        dReqDeductionAmt = dGetValue((dAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);


        if (dDeduction > (dReqDeduction + dAutoDeduction))
            dAcceptedAmount = dGetValue(dAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else {
            //dAcceptedAmount = dAmount - dGetValue((dAmount * dDeduction) / 100);
            dAcceptedAmount = dGetValue(dAmount - dReqDeductionAmt - dAutoDeductionAmt);
        }       
    }
    objRow.cells[18].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[18].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgPartAmt == dAcceptedAmount) {
        dDiffOfPartAmt = 0;
    }
    else {
    
        dDiffOfPartAmt = (dClaimAmount - dOrgPartAmt) + (dAcceptedAmount - dClaimAmount);
    }
    //debugger;    
    dPartTaxPer = dGetValue(objRow.cells[23].children[0].value);
    var dDiffTaxamt = 0;
    dDiffTaxamt = (dDiffOfPartAmt * dPartTaxPer / 100);
    AddAmountToJobTotal("Part", dDiffOfPartAmt, dDiffTaxamt);
//    var dDeductionRemark = '';
//    dDeductionRemark = objRow.cells[19].children[0].innerHTML;
//    if (dDeduction > 0) {
//        if (dDeductionRemark.trim() == '') {
//            alert("Please Enter Part Deduction Remark")
//            objRow.cells[19].childNodes[0].focus();
//            return false;
//        }
//    }
//    else if (dDeduction == 0) {
//    objRow.cells[19].children[0].innerHTML = '';
//    }    

}

//Part Deduction Changed Remark
function DeductionChangedRemark(event, ObjDeductionCtrl) {
    
    var dClaimAmount;
    var dAcceptedAmount;
    var dRate
    var dClaimedQty = 0;
    var dAcceptedQty = 0;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;
    
    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;
    var dDiffOfPartAmt = 0;
    var dOrgPartAmt = 0;
    var dDeductionRemark = '';

    //Get deduction percentage Remark
    dDeductionRemark = ObjDeductionCtrl.value;   
   
    // Calculate Line Level Part amount
    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);

    // Calculate Line Level Part amount
    var objRow = objID.parentNode.parentNode;

    // Get Claimed Qty
    dClaimedQty = dGetValue(objRow.cells[10].children[0].innerHTML);

    // Get Rate
    dRate = dGetValue(objRow.cells[11].children[0].innerHTML);

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[12].children[0].innerHTML);

    // Get Accepted Qty    
    dAcceptedQty = dGetValue(objRow.cells[13].children[0].innerHTML);

    //Get Deduction
    dDeduction = dGetValue(objRow.cells[17].children[0].innerHTML);
    dAutoDeduction = dGetValue(objRow.cells[22].children[0].innerHTML);
    dReqDeduction = dGetValue(objRow.cells[25].children[0].innerHTML);

    if (dGetValue(dDeduction) < dGetValue(dAutoDeduction)) {
        alert(dGetValue(dAutoDeduction) + "% Auto deduction already applied on this Part.\n Cannot reduce Deduction below Auto deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < dGetValue(dReqDeduction)) {
        alert(dGetValue(dReqDeduction) + "% Request deduction already applied on this Part.\n Cannot reduce Deduction below Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < (dGetValue(dAutoDeduction) + dGetValue(dReqDeduction))) {
        alert(dGetValue(dAutoDeduction) + dGetValue(dReqDeduction) + "% Auto and Request deduction already applied on this Part.\n Cannot reduce Deduction below Auto and Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }

    //Get Accepted Claim Amount
    dOrgPartAmt = dGetValue(objRow.cells[18].children[0].innerHTML);
    
    //dAmount = dAcceptedQty * dRate;
    dAmount = dAcceptedQty * ((dRate * 100) / (100 - dReqDeduction));
    
    if (dDeduction == 0) {
        dAcceptedAmount = dAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dClaimAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dClaimAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dClaimAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);


        if (dAutoDeduction > 0)
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else
            dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
        
    }
    objRow.cells[18].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[18].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgPartAmt == dAcceptedAmount) {
        dDiffOfPartAmt = 0;
    }
    else {
        dDiffOfPartAmt = (dClaimAmount - dOrgPartAmt) + (dAcceptedAmount - dClaimAmount);
    }

    if (dDeduction > 0) {
        if (dDeductionRemark.trim() == '') {
            alert("Please Enter Part Deduction Remark")
            ObjDeductionCtrl.focus();
            return false;
        }
    }
    else if (dDeduction==0) {
        dDeductionRemark = '';
    }

}


///########Labour############/
//Calculate Labour Accepted Amount on Deduction Changed
function CalculateLabourAcceptedAmtVecvPercentChanged(event, ObjVECVPercentControl) {
    
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAmount = 0;
    var dDiffOfLabourAmt = 0;
    var dOrgAmt = 0;
    var dVECVPercentage = 0;
    //debugger;
    if (CheckPercentageValue(event, ObjVECVPercentControl) == false) {
        return false;
    }
    ObjVECVPercentControl.value = RoundupValue(ObjVECVPercentControl.value);

    // Calculate Line Level Labour amount
    //var objID = $('#' + ObjVECVPercentControl.id)
    var objID = window.document.getElementById(ObjVECVPercentControl.id);
    var objRow = objID.parentNode.parentNode;
    // Get VECV Percentage
    dVECVPercentage = dGetValue(ObjVECVPercentControl.value);


    var dCustPercentage = dGetValue(objRow.cells[8].children[0].value);

    var hdnIsTaxableLbr = window.document.getElementById('hdnIsTaxableLbr');
    var hdnIsAccidental = window.document.getElementById('hdnIsAccidental');
    
    if (dVECVPercentage > (100 - dGetValue(dCustPercentage))) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dGetValue(objRow.cells[7].children[0].value) + dCustPercentage));
        return false;
    }

    //Set Dealer Percentage
    objRow.cells[7].children[0].innerHTML = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    objRow.cells[7].children[0].value = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    var dDealerPercentage = dGetValue(objRow.cells[7].children[0].innerHTML);

    var dTotalPercentage = dVECVPercentage + dDealerPercentage + dCustPercentage;
    if (dTotalPercentage > 100) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dDealerPercentage + dCustPercentage));
        return false;
    }   

    //Get deduction percentage
    dDeduction = 100 - dVECVPercentage;
    objRow.cells[9].children[0].innerHTML = dDeduction;
    objRow.cells[9].children[0].value = dDeduction;


    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Get Org Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].value);
    
    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
    }

    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);


    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLabourAmt = 0;
    }
    else {
        dDiffOfLabourAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }
    ////debugger;
    hdnIsTaxableLbr.value = objRow.cells[17].childNodes[1].innerHTML
    hdnIsAccidental.value = objRow.cells[16].childNodes[1].innerHTML
    AddAmountToJobTotal("Labour", dDiffOfLabourAmt,0);

}
//Calculate Labour Accepted Amount on Deduction Changed
function CalculateLabourAcceptedAmtDeductionChanged(event, ObjDeductionCtrl) {
    ////debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;

    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;
    var dDiffOfLabourAmt = 0;
    var dOrgAmt = 0;

    // Calculate Line Level Labour amount
    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);
    //var objRow = objID[0].parentNode.parentNode;
    var objRow = objID.parentNode.parentNode;
    
    var hdnIsTaxableLbr = window.document.getElementById('hdnIsTaxableLbr');
    var hdnIsAccidental = window.document.getElementById('hdnIsAccidental');
    
    if (CheckPercentageValue(event, ObjDeductionCtrl) == false) {
        return false;
    }

    dAutoDeduction = dGetValue(objRow.cells[13].children[0].value);
    dReqDeduction = dGetValue(objRow.cells[18].children[0].value);
//    if (dGetValue(dAutoDeduction) == 100) {
//        alert(" 100% Auto deduction already applied on the total claim");
//        ObjDeductionCtrl.focus();
//        return false;
//    }   

    //Get deduction percentage
    dDeduction = dGetValue(ObjDeductionCtrl.value);
    ObjDeductionCtrl.value = RoundupValue(ObjDeductionCtrl.value);

    if (dGetValue(dDeduction) < dGetValue(dAutoDeduction)) {
        alert(dGetValue(dAutoDeduction) + "% Auto deduction already applied on this Labor.\n Cannot reduce Deduction below Auto deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < dGetValue(dReqDeduction)) {
    alert(dGetValue(dReqDeduction) + "% Request deduction already applied on this Labor.\n Cannot reduce Deduction below Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < (dGetValue(dAutoDeduction) + dGetValue(dReqDeduction))) {
    alert(dGetValue(dAutoDeduction) + dGetValue(dReqDeduction) + "% Auto and Request deduction already applied on this Labor.\n Cannot reduce Deduction below Auto and Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }

  

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Get Org Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].value);



    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dClaimAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dClaimAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dClaimAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);
 
        
        if (dDeduction > (dReqDeduction + dAutoDeduction))
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt); //dGetValue((dClaimAmount * dAutoDeduction) / 100) - dGetValue(((dClaimAmount - (dClaimAmount * dAutoDeduction) / 100) * (dDeduction - dAutoDeduction)) / 100); //dClaimAmount - dGetValue(((dClaimAmount - (dClaimAmount * dAutoDeduction) / 100) * (dDeduction > dAutoDeduction) ? (dDeduction - dAutoDeduction) : dDeduction) / 100);
        else
            //dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt);
    }
    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);


    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLabourAmt = 0;
    }
    else {
        dDiffOfLabourAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }
    hdnIsTaxableLbr.value = objRow.cells[17].children[0].innerHTML
    hdnIsAccidental.value = objRow.cells[16].children[0].innerHTML
    AddAmountToJobTotal("Labour", dDiffOfLabourAmt,0);

//    var dDeductionRemark = '';
//    dDeductionRemark = objRow.cells[11].children[0].innerHTML;
//    if (dDeduction > 0) {
//        if (dDeductionRemark.trim() == '') {
//            alert("Please Enter Labour Deduction Remark")
//            objRow.cells[11].childNodes[0].focus();
//            return false;
//        }
//    }
//    else if (dDeduction == 0) {
//    objRow.cells[11].children[0].innerHTML = '';
//    }    

}

//Labour Deduction Changed Remark
function LabourDeductionChangedRemark(event, ObjDeductionCtrl) {
    //////debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;

    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;
    var dDiffOfLabourAmt = 0;
    var dOrgAmt = 0;
    var dDeductionRemark = '';
   

    //Get deduction Remark
    dDeductionRemark = ObjDeductionCtrl.value;

    // Calculate Line Level Labour amount
    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);
   var objRow = objID.parentNode.parentNode;

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    //deduction Amount
    dDeduction = dGetValue(objRow.cells[9].children[0].innerHTML);
    dAutoDeduction = dGetValue(objRow.cells[13].children[0].innerHTML);
    dReqDeduction = dGetValue(objRow.cells[18].children[0].innerHTML);
    // Get Org Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].innerHTML);


    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dClaimAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dClaimAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dClaimAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);


        if (dAutoDeduction > 0)
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else
            dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);        
    }

    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);


    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLabourAmt = 0;
    }
    else {    
        dDiffOfLabourAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }

    if (dDeduction > 0) {
        if (dDeductionRemark.trim() == '') {
            alert("Please Enter Labour Deduction Remark")
            ObjDeductionCtrl.focus();
            return false;
        }
    }
    else if (dDeduction == 0) {
        dDeductionRemark = '';
    }

}

///###############Lubricant##############
function CalculateLubricantAcceptedAmtVecvPercentChanged(event, ObjVECVPercentControl) {
    ////debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAmount = 0;
    var dDiffOfLubricantAmt = 0;
    var dOrgAmt = 0;
    var dVECVPercentage = 0;
    if (CheckPercentageValue(event, ObjVECVPercentControl) == false) {
        return false;
    }
    

    //Get deduction percentage
    dVECVPercentage = dGetValue(ObjVECVPercentControl.value);
    ObjVECVPercentControl.value = RoundupValue(ObjVECVPercentControl.value);

    // Calculate Line Level Labour amount
    //var objID = $('#' + ObjVECVPercentControl.id)
    var objID = window.document.getElementById(ObjVECVPercentControl.id);
    var objRow = objID.parentNode.parentNode;

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Get Org Accepted Amount    
    dOrgAmt = dGetValue(objRow.cells[10].children[0].value);
    var dCustPercentage = dGetValue(objRow.cells[8].children[0].value);
    if (dVECVPercentage > (100 - dGetValue(dCustPercentage))) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dGetValue(objRow.cells[7].children[0].value) + dCustPercentage));
        return false;
    }

    //Set Dealer Percentage
    objRow.cells[7].children[0].innerHTML = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    objRow.cells[7].children[0].value = RoundupValue(100 - (dVECVPercentage + dCustPercentage));

    var dDealerPercentage = dGetValue(objRow.cells[7].children[0].value);

    var dTotalPercentage = dVECVPercentage + dDealerPercentage + dCustPercentage;
    if (dTotalPercentage > 100) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dDealerPercentage + dCustPercentage));
        return false;
    }
    //#############
    
    dDeduction = dGetValue(100 - dVECVPercentage);

    objRow.cells[9].children[0].innerHTML = RoundupValue(dDeduction);
    objRow.cells[9].children[0].value = RoundupValue(dDeduction);
    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
    }

    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLubricantAmt = 0;
    }
    else {
        dDiffOfLubricantAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }
    ////debugger;
    var dDiffTaxamt = 0;
    dPartTaxPer = dGetValue(objRow.cells[14].children[0].value);
    dDiffTaxamt = (dDiffOfLubricantAmt * dPartTaxPer / 100);
    AddAmountToJobTotal("Lubricant", dDiffOfLubricantAmt, dDiffTaxamt);
}
//Calculate Lubricant Accepted Amount on Deduction Changed
function CalculateLubricantAcceptedAmtDeductionChanged(event, ObjDeductionCtrl) {
    debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;

    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;
    var dDiffOfLubricantAmt = 0;
    var dOrgAmt = 0;

    // Calculate Line Level lubricant amount
    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);
   var objRow = objID.parentNode.parentNode;
    
    if (CheckPercentageValue(event, ObjDeductionCtrl) == false) {
        return false;
    }

    dAutoDeduction = dGetValue(objRow.cells[13].children[0].value);
    dReqDeduction = dGetValue(objRow.cells[16].children[0].value);
    

    //Get deduction percentage
    dDeduction = dGetValue(ObjDeductionCtrl.value);
    ObjDeductionCtrl.value = RoundupValue(ObjDeductionCtrl.value);    
    
    if (dGetValue(dDeduction) < dGetValue(dAutoDeduction)) {
        alert(dGetValue(dAutoDeduction) + "% Auto deduction already applied on this lubricant.\n Cannot reduce Deduction below Auto deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < dGetValue(dReqDeduction)) {
    alert(dGetValue(dReqDeduction) + "% Request deduction already applied on this lubricant.\n Cannot reduce Deduction below Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < (dGetValue(dAutoDeduction) + dGetValue(dReqDeduction))) {
    alert(dGetValue(dAutoDeduction) + dGetValue(dReqDeduction) + "% Auto and Request deduction already applied on this lubricant.\n Cannot reduce Deduction below Auto and Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    } 
   

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Get Orginal Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].value);
       

    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dClaimAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dClaimAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dClaimAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);

        if (dDeduction > (dReqDeduction + dAutoDeduction))
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else
            //dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
            dAcceptedAmount =dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt);        
    }
//    if (dOrgAmt == dAcceptedAmount) {
//        dDiffOfLubricantAmt = 0;
//    }
//    else {
//        dDiffOfLubricantAmt = dAcceptedAmount - dClaimAmount;
    //    }
    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLubricantAmt = 0;
    }
    else {
        dDiffOfLubricantAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }

    //debugger;
    var dDiffTaxamt = 0;
    dPartTaxPer = dGetValue(objRow.cells[14].children[0].value);
    dDiffTaxamt = (dDiffOfLubricantAmt * dPartTaxPer / 100);
    AddAmountToJobTotal("Lubricant", dDiffOfLubricantAmt, dDiffTaxamt);
    //AddAmountToJobTotal("Lubricant", dDiffOfLubricantAmt,0);

//    var dDeductionRemark = '';
//    dDeductionRemark = objRow.cells[11].children[0].innerHTML;
//    if (dDeduction > 0) {
//        if (dDeductionRemark.trim() == '') {
//            alert("Please Enter Lubricant Deduction Remark")
//            objRow.cells[11].childNodes[0].focus();
//            return false;
//        }
//    }
//    else if (dDeduction == 0) {
//    objRow.cells[11].children[0].innerHTML = '';
//    }    

}

//Lubricant Deduction Changed Remark
function LubricantDeductionChangedRemark(event, ObjDeductionCtrl) {
    ////debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAmount = 0;
    var dDiffOfLubricantAmt = 0;
    var dOrgAmt = 0;
    var dDeductionRemark = '';

    //Get deduction Remark
    dDeductionRemark = ObjDeductionCtrl.value;

    // Calculate Line Level lubricant amount
    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);
   var objRow = objID.parentNode.parentNode;

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Deduction Amount
    dDeduction = dGetValue(objRow.cells[9].children[0].innerHTML);
    

    // Get Orginal Accepted Amount    
    dOrgAmt = dGetValue(objRow.cells[10].children[0].innerHTML);

    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
    }
    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLubricantAmt = 0;
    }
    else {
        dDiffOfLubricantAmt = dAcceptedAmount - dClaimAmount;
    }
    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfLubricantAmt = 0;
    }
    else {
        dDiffOfLubricantAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }


    if (dDeduction > 0) {
        if (dDeductionRemark.trim() == '') {
            alert("Please Enter Lubricant Deduction Remark")
            ObjDeductionCtrl.focus();
            return false;
        }
    }
    else if (dDeduction == 0) {
        dDeductionRemark = '';
    }  

}

///###############Sublet##############
function CalculateSubletAcceptedAmtVecvPercentChanged(event, ObjVECVPercentControl) {
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAmount = 0;
    var dDiffOfLabourAmt = 0;
    var dOrgAmt = 0;
    var dVECVPercentage = 0;
    if (CheckPercentageValue(event, ObjVECVPercentControl) == false) {
        return false;
    }
    var hdnIsTaxableLbr = window.document.getElementById('hdnIsTaxableLbr');
    debugger;
    //Get deduction percentage
    dVECVPercentage = dGetValue(ObjVECVPercentControl.value);
    ObjVECVPercentControl.value = RoundupValue(ObjVECVPercentControl.value);

    // Calculate Line Level sublet amount
    //var objID = $('#' + ObjVECVPercentControl.id)
    var objID = window.document.getElementById(ObjVECVPercentControl.id);
    var objRow = objID.parentNode.parentNode;
    
    var dCustPercentage = dGetValue(objRow.cells[8].children[0].value);
    if (dVECVPercentage >(100 - dGetValue(dCustPercentage))) 
    {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dGetValue(objRow.cells[7].children[0].value) + dCustPercentage));
        return false;
    }
    
    //Set Dealer Percentage
    objRow.cells[7].children[0].innerHTML = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    objRow.cells[7].children[0].value = RoundupValue(100 - (dVECVPercentage + dCustPercentage));
    var dDealerPercentage = dGetValue(objRow.cells[7].children[0].innerHTML);
    
    var dTotalPercentage = dVECVPercentage + dDealerPercentage + dCustPercentage;
    if (dTotalPercentage > 100) {
        alert("Total Of the all Percentage is not equal to 100 ! ");
        ObjVECVPercentControl.value = (100 - (dDealerPercentage + dCustPercentage));
        return false;
    }   
    
    
    //Get deduction percentage
    dDeduction = 100 - dVECVPercentage;
    //set deduction
    objRow.cells[9].children[0].innerHTML = RoundupValue(dDeduction);
    objRow.cells[9].children[0].value = RoundupValue(dDeduction);


    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Get Original Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].value);        
    
    /////////
    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
    }

    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfSubLetAmt = 0;
    }
    else {
        dDiffOfSubLetAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }
    hdnIsTaxableLbr.value = objRow.cells[16].children[0].innerHTML
    AddAmountToJobTotal("SubLet", dDiffOfSubLetAmt,0);
}
//Calculate SubLet Accepted Amount on Deduction Changed
function CalculateSubLetAcceptedAmtDeductionChanged(event, ObjDeductionCtrl) {
    //debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAutoDeduction = 0;
    var dReqDeduction = 0;

    var dDeductionAmt = 0;
    var dAutoDeductionAmt = 0;
    var dReqDeductionAmt = 0;
    
    var dAmount = 0;
    var dDiffOfSubLetAmt = 0;
    var dOrgAmt = 0;

    // Calculate Line Level Sublet amount

    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);
    //var objRow = objID[0].parentNode.parentNode;
    var objRow = objID.parentNode.parentNode;
    
    if (CheckPercentageValue(event, ObjDeductionCtrl) == false) {
        return false;
    }
    var hdnIsTaxableLbr = window.document.getElementById('hdnIsTaxableLbr');
    dAutoDeduction = dGetValue(objRow.cells[13].children[0].value);
    dReqDeduction = dGetValue(objRow.cells[17].children[0].value);
    
    //Get deduction percentage
    dDeduction = dGetValue(ObjDeductionCtrl.value);
    
    if (dGetValue(dDeduction) < dGetValue(dAutoDeduction)) {
        alert(dGetValue(dAutoDeduction) + "% Auto deduction already applied on this sublet.\n Cannot reduce Deduction below Auto deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < dGetValue(dReqDeduction)) {
    alert(dGetValue(dReqDeduction) + "% Request deduction already applied on this sublet.\n Cannot reduce Deduction below Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }
    else if (dGetValue(dDeduction) < (dGetValue(dAutoDeduction) + dGetValue(dReqDeduction))) {
    alert(dGetValue(dAutoDeduction) + dGetValue(dReqDeduction) + "% Auto and Request deduction already applied on this sublet.\n Cannot reduce Deduction below Auto and Request deduction percentage!");
        ObjDeductionCtrl.focus();
        return false;
    }  

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Get Original Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].value);

    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dReqDeductionAmt = dGetValue((dClaimAmount * dReqDeduction) / 100)
        dAutoDeductionAmt = dGetValue(((dClaimAmount - dReqDeductionAmt) * dAutoDeduction) / 100)
        dDeductionAmt = dGetValue(((dClaimAmount - (dAutoDeductionAmt + dReqDeductionAmt)) * ((dDeduction > (dReqDeduction + dAutoDeduction)) ? (dDeduction - dReqDeduction - dAutoDeduction) : dDeduction)) / 100);

        if (dDeduction > (dReqDeduction + dAutoDeduction))
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt - dDeductionAmt);
        else
            //dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
            dAcceptedAmount = dGetValue(dClaimAmount - dReqDeductionAmt - dAutoDeductionAmt); 
    }
    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);
    

    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfSubLetAmt = 0;
    }
    else {
        dDiffOfSubLetAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }
    hdnIsTaxableLbr.value = objRow.cells[16].children[0].innerHTML
    AddAmountToJobTotal("SubLet", dDiffOfSubLetAmt,0);
//    var dDeductionRemark = '';
//    dDeductionRemark = objRow.cells[11].children[0].innerHTML;
//    if (dDeduction > 0) {
//        if (dDeductionRemark.trim() == '') {
//            alert("Please Enter SubLet Deduction Remark")
//            objRow.cells[11].childNodes[0].focus();
//            return false;
//        }
//    }
//    else if (dDeduction == 0) {
//    objRow.cells[11].children[0].innerHTML = '';
//    }    

}

//SubLet Deduction Changed Remark
function SubletDeductionChangedRemark(event, ObjDeductionCtrl) {
    ////debugger;
    var dClaimAmount;
    var dAcceptedAmount;
    var dDeduction = 0;
    var dAmount = 0;
    var dDiffOfSubLetAmt = 0;
    var dOrgAmt = 0;
    var dDeductionRemark = '';
    
    //Get deduction percentage
    dDeductionRemark = ObjDeductionCtrl.value;

    // Calculate Line Level Sublet amount

    //var objID = $('#' + ObjDeductionCtrl.id)
    var objID = window.document.getElementById(ObjDeductionCtrl.id);
   var objRow = objID.parentNode.parentNode;

    // Get Claim amount
    dClaimAmount = dGetValue(objRow.cells[5].children[0].innerHTML);

    // Deduction Remark
    dDeduction = dGetValue(objRow.cells[9].children[0].innerHTML);
    
    // Get Original Accepted Amount
    dOrgAmt = dGetValue(objRow.cells[10].children[0].innerHTML);

    if (dDeduction == 0) {
        dAcceptedAmount = dClaimAmount;
    }
    else {
        dAcceptedAmount = dClaimAmount - dGetValue((dClaimAmount * dDeduction) / 100);
    }

    objRow.cells[10].children[0].innerHTML = RoundupValue(dAcceptedAmount);
    objRow.cells[10].children[0].value = RoundupValue(dAcceptedAmount);

    if (dOrgAmt == dAcceptedAmount) {
        dDiffOfSubLetAmt = 0;
    }
    else {
        dDiffOfSubLetAmt = (dClaimAmount - dOrgAmt) + (dAcceptedAmount - dClaimAmount);
    }

//    if (dDeduction > 0) {
//        if (dDeductionRemark.trim() == '') {
//            alert("Please Enter SubLet Deduction Remark")
//            ObjDeductionCtrl.focus();
//            return false;
//        }
//    }
//    else if (dDeduction == 0) {
//        dDeductionRemark = '';
//    }  

}

// Add Amount to Total Amount And Grand Amount
// typeofAmt i.e.Part/Labour/Lubricant/SubLet
function AddAmountToJobTotal(typeofAmt, dAddAmount, dAddTaxAmt ) {
    ////debugger;
    var PcontainerName = '';
    var iTaxAmt = 0.0;
    
    var iTotAmtId;
    var txtTotalAmt;
    
    var iClaimAmtId = 'txtAccClaimAmt';
    var txtClaimAmt;

    var iGrandTotAmtId;
    var txtGrandTotAmt;

    var iGrandClaimAmtId = 'txtTotalClaimAmt';
    var txtGrandClaimAmt;

    var hdnISDocGST = window.document.getElementById('hdnISDocGST');
    PcontainerName = GetContainerName();
    if (typeofAmt == "Labour") {
        var hdnIsTaxableLbr = window.document.getElementById('hdnIsTaxableLbr');
        var hdnIsAccidental = window.document.getElementById('hdnIsAccidental');
    }
    else if (typeofAmt == "SubLet") {
        var hdnIsTaxableLbr = window.document.getElementById('hdnIsTaxableLbr');        
    }
    
    if (typeofAmt == "Part") {
        // Get Part Amount
        iTotAmtId = 'txtPartAmount';
        iGrandTotAmtId = 'txtTotalPartAmount';   
    }
    else if (typeofAmt == "Labour" && hdnIsTaxableLbr.value == 'Y' && hdnIsAccidental.value == 'N') {
        // Get non accidenatal Labour Amount
        iTotAmtId = 'txtLabourAmount';
        iGrandTotAmtId = 'txtTotalLabourAmount';
    }
    else if (typeofAmt == "Labour" && hdnIsTaxableLbr.value == 'Y' && hdnIsAccidental.value == 'Y') {
        // Get accidental Labour Amount
        iTotAmtId = 'txtLabourAmount';
        iTotAmtId1 = 'txtAccNonTaxLabourAmt';
        iGrandTotAmtId = 'txtTotalLabourAmount';
    }
    else if (typeofAmt == "Labour" && hdnIsTaxableLbr.value == 'N') {
        // Get Labour Amount
        iTotAmtId = 'txtAccNonTaxLabourAmt';
        iGrandTotAmtId = 'txtTotalLabourAmount';
    }
    else if (typeofAmt == "Lubricant") {
        // Get Labour Amount
    iTotAmtId = 'txtLubricantAmount';
    iGrandTotAmtId = 'txtTotalLubricantAmount';
    
    }
    else if (typeofAmt == "SubLet") {
        // Get Labour Amount
    iTotAmtId = 'txtSubletAmount';
    iGrandTotAmtId = 'txtTotalSubletAmount';
    
    }

    //txtTotalAmt = document.getElementById(PcontainerName + iTotAmtId);
    if (typeofAmt == "Labour" && hdnIsTaxableLbr.value == 'Y' && hdnIsAccidental.value == 'Y') {
        // taxable labour amount
        txtTotalAmt = document.getElementById(iTotAmtId);
        var dTotalAmt;
        
        var dActDednAmt;
        dActDednAmt = dGetValue(dAddAmount);
        
        if (txtTotalAmt == null) return;
        dTotalAmt =dGetValue(txtTotalAmt.value);
        dAddAmount = dGetValue(dAddAmount - dGetValue(dAddAmount * 30 / 100));
        txtTotalAmt.value = RoundupValue(dTotalAmt + dAddAmount);
        
        if (hdnIsTaxableLbr.value == 'Y') var iLabourTax = window.document.getElementById('hdnLabourST');
        if (hdnIsTaxableLbr.value == 'N') var iLabourTax = window.document.getElementById('hdnLabourSTNill');
        var iLabourTaxAmount = window.document.getElementById('txtAccLabourST');
        if (hdnIsTaxableLbr.value == 'Y') {
            iTaxAmt = dGetValue(RoundupValue(((dTotalAmt) * dGetValue(iLabourTax.value)) / 100));
            iLabourTaxAmount.value = RoundupValue((((dTotalAmt + dAddAmount) * dGetValue(iLabourTax.value)) / 100));
        }
        //dAddAmount = dGetValue(dAddAmount) + dGetValue(RoundupValue(((dAddAmount * dGetValue(iLabourTax.value)) / 100).toFixed(3)));
        dAddAmount = dGetValue(dAddAmount) + dGetValue(RoundupValue((((dTotalAmt + dAddAmount) * dGetValue(iLabourTax.value)) / 100)));

        // Get Total Amount
        //txtClaimAmt = document.getElementById(PcontainerName + iClaimAmtId);
        txtClaimAmt = document.getElementById(iClaimAmtId);
        var dClaimAmt = 0;
        if (txtClaimAmt == null) return;
        dClaimAmt = dGetValue(txtClaimAmt.value);
        txtClaimAmt.value = RoundupValue((dClaimAmt + dAddAmount - iTaxAmt));

        /////**********************************//////

        //Set Grand amount
        txtGrandTotAmt = document.getElementById(iGrandTotAmtId);
        if (txtGrandTotAmt == null) return;
        txtGrandTotAmt.value = RoundupValue(dGetValue(txtGrandTotAmt.value) + dAddAmount);
        //Get Total Amount
        var dGrandTot = dGetValue(dAddAmount);
        // Get Total Amount
//        //txtClaimAmt = document.getElementById(PcontainerName + iClaimAmtId);
//        txtGrandClaimAmt = document.getElementById(iGrandClaimAmtId);
//        if (txtGrandClaimAmt == null) return;
//        txtGrandClaimAmt.value = RoundupValue(dGetValue(txtGrandClaimAmt.value) + dAddAmount);
        
        // non taxable labour amount
        txtTotalAmt = document.getElementById(iTotAmtId1);
        var dTotalAmt;
        if (txtTotalAmt == null) return;
        dTotalAmt = dGetValue(txtTotalAmt.value);
        dAddAmount = dGetValue(dActDednAmt);
        dAddAmount = dGetValue(dAddAmount - dGetValue(dAddAmount * 70 / 100));
        txtTotalAmt.value = RoundupValue(dTotalAmt + dAddAmount);
        dAddAmount = dGetValue(dAddAmount); 

        // Get Total Amount        
        //txtClaimAmt = document.getElementById(iClaimAmtId);
        //var dClaimAmt = 0;
        //if (txtClaimAmt == null) return;
        dClaimAmt = dGetValue(txtClaimAmt.value);
        txtClaimAmt.value = RoundupValue((dClaimAmt + dAddAmount ));

        /////**********************************//////

        //Set Grand amount
        //txtGrandTotAmt = document.getElementById(iGrandTotAmtId);
        //if (txtGrandTotAmt == null) return;
        txtGrandTotAmt.value = RoundupValue(dGetValue(txtGrandTotAmt.value) + dAddAmount);

        // Get Total Amount
        //txtClaimAmt = document.getElementById(PcontainerName + iClaimAmtId);
        txtGrandClaimAmt = document.getElementById(iGrandClaimAmtId);
        if (txtGrandClaimAmt == null) return;
        //txtGrandClaimAmt.value = RoundupValue(dGetValue(txtGrandClaimAmt.value) + dAddAmount);
        txtGrandClaimAmt.value = RoundupValue(dGetValue(txtGrandClaimAmt.value) + dAddAmount + dGrandTot);
        return;        
    }
    txtTotalAmt = document.getElementById(iTotAmtId);
    var dTotalAmt;
    if (txtTotalAmt == null) return;
    dTotalAmt = dGetValue(txtTotalAmt.value);
    dAddAmount = dGetValue(dAddAmount);
    txtTotalAmt.value = RoundupValue(dTotalAmt + dAddAmount);
    
    if (typeofAmt == "Part") {
        var iPartTax = window.document.getElementById('hdnPartTax');
        var iPartTaxAmount = window.document.getElementById('txtPartTaxAmount');
        iTaxAmt = dGetValue(RoundupValue(((dTotalAmt) * dGetValue(iPartTax.value)) / 100));        
        iPartTaxAmount.value = RoundupValue((((dTotalAmt + dAddAmount) * dGetValue(iPartTax.value)) / 100));
        //dAddAmount = dGetValue(dAddAmount) + dGetValue(((dAddAmount * dGetValue(iPartTax.value)) / 100).toFixed(3));
        dAddAmount = dGetValue(dAddAmount) + dGetValue(RoundupValue((((dTotalAmt + dAddAmount) * dGetValue(iPartTax.value)) / 100)));

    } else if (typeofAmt == "Labour" || typeofAmt == "SubLet") {       
       
        if (hdnIsTaxableLbr.value == 'Y') var iLabourTax = window.document.getElementById('hdnLabourST');
        if (hdnIsTaxableLbr.value == 'N') var iLabourTax = window.document.getElementById('hdnLabourSTNill');
        var iLabourTaxAmount = window.document.getElementById('txtAccLabourST');
        if (hdnIsTaxableLbr.value == 'Y') {
            iTaxAmt = dGetValue(RoundupValue(((dTotalAmt) * dGetValue(iLabourTax.value)) / 100));
            iLabourTaxAmount.value = RoundupValue((((dTotalAmt + dAddAmount) * dGetValue(iLabourTax.value)) / 100));
        }
        //dAddAmount = dGetValue(dAddAmount) + dGetValue(RoundupValue(((dAddAmount * dGetValue(iLabourTax.value)) / 100).toFixed(3)));
        dAddAmount = dGetValue(dAddAmount) + dGetValue(RoundupValue((((dTotalAmt + dAddAmount) * dGetValue(iLabourTax.value)) / 100)));
    }
    if (hdnISDocGST.value == 'Y')
    {
        var iLabourTax = window.document.getElementById('hdnLabourST');
        var iLabourTaxAmount = window.document.getElementById('txtAccLabourST');        
        var txtPartAmount = window.document.getElementById('txtPartAmount');
        var txtLabourAmount = window.document.getElementById('txtLabourAmount');
        var txtSubletAmount = window.document.getElementById('txtSubletAmount');
        var txtLubricantAmount = window.document.getElementById('txtLubricantAmount');
        //debugger;
        var objInvType = window.document.getElementById('hdnInvType');

        if (objInvType.value == "N") {
            var dAllTotal = RoundupValue(dGetValue(txtPartAmount.value) + dGetValue(txtLabourAmount.value) + dGetValue(txtSubletAmount.value) + dGetValue(txtLubricantAmount.value));
            iLabourTaxAmount.value = RoundupValue((((dAllTotal) * dGetValue(iLabourTax.value)) / 100));
        }
        else if (objInvType.value == "L") {
            var dAllTotal = RoundupValue(dGetValue(txtLabourAmount.value) + dGetValue(txtSubletAmount.value));
            iLabourTaxAmount.value = RoundupValue((((dAllTotal) * dGetValue(iLabourTax.value)) / 100));
        }
        else {
            iLabourTaxAmount.value = RoundupValue(dGetValue(iLabourTaxAmount.value) + dGetValue(dAddTaxAmt));
        }
    }
    
    // Get Total Amount
    //txtClaimAmt = document.getElementById(PcontainerName + iClaimAmtId);
    txtClaimAmt = document.getElementById(iClaimAmtId);
    var dClaimAmt=0;
    if (txtClaimAmt == null) return;   
    dClaimAmt =dGetValue(txtClaimAmt.value) ;
    txtClaimAmt.value = RoundupValue((dClaimAmt + dAddAmount + dAddTaxAmt - iTaxAmt));

    /////**********************************//////
    
    //Set Grand amount
    txtGrandTotAmt = document.getElementById(iGrandTotAmtId);
    if (txtGrandTotAmt == null) return;
    txtGrandTotAmt.value = RoundupValue(dGetValue(txtGrandTotAmt.value) + dAddAmount);
    
    // Get Total Amount
    //txtClaimAmt = document.getElementById(PcontainerName + iClaimAmtId);
    txtGrandClaimAmt = document.getElementById(iGrandClaimAmtId);
    if (txtGrandClaimAmt == null) return;
    txtGrandClaimAmt.value = RoundupValue(dGetValue(txtGrandClaimAmt.value) + dAddAmount);    
}

// To Get Job Details Part/Labour/Lubricant/SubLet Amount
function GetJobDetails(Claim_Status) {
    ////debugger;
    if (Claim_Status == "1") {
        if (confirm("Have you saved the changes?") == false) {
            return false;
        }
    }
//    var ReturnValue = new Array();
//    var ObjControl = null;

//    ObjControl = document.getElementById('txtCopyPartAmount');
//    ReturnValue[0] = ObjControl.value;


//    ObjControl = document.getElementById('txtCopyLabourAmount');
//    ReturnValue[1] = ObjControl.value;

//    ObjControl = document.getElementById('txtCopyLubricantAmount');
//    ReturnValue[2] = ObjControl.value;

//    ObjControl = document.getElementById('txtCopySubletAmount');
//    ReturnValue[3] = ObjControl.value;

////    ObjControl = document.getElementById('txtCopyClaimAmt');
////    ReturnValue[4] = ObjControl.value;
//    window.returnValue = ReturnValue;
    window.close();
}
function GetSelectedItem() {
    ////debugger;
    //var RB1 = $('#rdoDeductionType');
    var RB1 = window.document.getElementById('rdoDeductionType');
    if (RB1[0].rows[0].cells[0].children[0].checked)
        return OnOverallDeductionChange(RB1[0].rows[0].cells[0].children[0]);
    else if (RB1[0].rows[0].cells[1].children[0].checked)
        return OnItemwiseDeductionChange(RB1[0].rows[0].cells[1].children[0])
}

// When user select Share percentage
function OnOverallDeductionChange1(ObjOption) {
}
function OnOverallDeductionChange(event, ObjOption) {
    ////debugger;
    var objcontrol = null;
    objControl = window.document.getElementById('txtPartHeaderDeduction');
    if (objControl == null) return false;
    objControl.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
    
    var iOrgPartPercentage = window.document.getElementById('hdnHeaderPrePartDeduction');
    var iOrgLabourPercentage = window.document.getElementById('hdnHeaderPreLabourDeduction');
    var iOrgLubricantPercentage = window.document.getElementById('hdnHeaderPreLubricantDeduction');
    var iOrgSubletPercentage = window.document.getElementById('hdnHeaderPreSubletDeduction');

    var dPartPercentage = 0, dLabourPercentage = 0, dLubricantPercentage = 0, dSubletPercentage = 0;

    if (ObjOption.checked == true)// If Overall Claim
    {
        // Get Part share
        objcontrol = window.document.getElementById('txtPartHeaderDeduction');
        if (objcontrol != null) {            
            dPartPercentage = dGetValue(objcontrol.value) - dGetValue(iOrgPartPercentage.value);
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
        objcontrol.setAttribute("onblur", "return AddDeductionPercentageToParts(event,this)");

        // Get Labour Share
        objcontrol = window.document.getElementById('txtLabourHeaderDeduction');
        if (objcontrol != null) {            
            dLabourPercentage = dGetValue(objcontrol.value) - dGetValue(iOrgLabourPercentage.value);
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
        objcontrol.setAttribute("onblur", "return AddDeductionPercentageToLabour(event,this)");

        // Get Lubricant Share
        objcontrol = window.document.getElementById('txtLubricantHeaderDeduction');
        if (objcontrol != null) {          
            dLubricantPercentage = dGetValue(objcontrol.value) - dGetValue(iOrgLubricantPercentage.value);
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
        objcontrol.setAttribute("onblur", "return AddDeductionPercentageToLubricant(event,this)");

        // Get Sublet Share
        objcontrol = window.document.getElementById('txtSubletHeaderDeduction');
        if (objcontrol != null) {            
            dSubletPercentage = dGetValue(objcontrol.value) - dGetValue(iOrgSubletPercentage.value);
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
        objcontrol.setAttribute("onblur", "return AddDeductionPercentageToSublet(event,this)");

        if (SetDeductionPercentageToPart(event, dPartPercentage, true) == false)
            return false;
        if (SetDeductionPercentageToLabour(event, dLabourPercentage, true) == false)
            return false;
        if (SetDeductionPercentageToLubricant(event, dLubricantPercentage, true) == false)
            return false;
        if (SetDeductionPercentageToSublet(event, dSubletPercentage, true) == false)
             return false;

         return true;
    }
}
function OnItemwiseDeductionChange1(ObjOption) {
}
// When user select Share Item wise
function OnItemwiseDeductionChange(event, ObjOption) {
    ////debugger;
    var objcontrol = null;
    var dPartPercentage = 0, dLabourPercentage = 0, dLubricantPercentage = 0, dSubletPercentage=0;

    var iOrgPartPercentage = window.document.getElementById('hdnHeaderPrePartDeduction');
    var iOrgLabourPercentage = window.document.getElementById('hdnHeaderPreLabourDeduction');
    var iOrgLubricantPercentage = window.document.getElementById('hdnHeaderPreLubricantDeduction');
    var iOrgSubletPercentage = window.document.getElementById('hdnHeaderPreSubletDeduction');

    if (ObjOption.checked == true)// If Itemwise Claim
    {
        // Get Part share
        objcontrol = window.document.getElementById('txtPartHeaderDeduction');
        if (objcontrol != null) {
            dPartPercentage = 0 - dGetValue(objcontrol.value);
            objcontrol.value = 0;
            iOrgPartPercentage.value = "0";
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");        

        // Get Labour Share
        objcontrol = window.document.getElementById('txtLabourHeaderDeduction');
        if (objcontrol != null) {
            dLabourPercentage = 0 - dGetValue(objcontrol.value);
            objcontrol.value = 0;
            iOrgLabourPercentage.value = "0";
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

        // Get Lubricant Share
        objcontrol = window.document.getElementById('txtLubricantHeaderDeduction');
        if (objcontrol != null) {
            dLubricantPercentage = 0 - dGetValue(objcontrol.value);
            objcontrol.value = 0;
            iOrgLubricantPercentage.value = "0";
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

        // Get Sublet Share
        objcontrol = window.document.getElementById('txtSubletHeaderDeduction');
        if (objcontrol != null) {
            dSubletPercentage = 0 - dGetValue(objcontrol.value);
            objcontrol.value = 0;
            iOrgSubletPercentage.value = "0";
        }
        else
            return false;
        objcontrol.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");

        if (SetDeductionPercentageToPart(event, dPartPercentage, false) == false)
            return false;
        if (SetDeductionPercentageToLabour(event, dLabourPercentage, false) == false)
            return false;
        if (SetDeductionPercentageToLubricant(event, dLubricantPercentage, false) == false)
            return false;
        if (SetDeductionPercentageToSublet(event, dSubletPercentage, false) == false)
            return false;
        return true; 
//        SetDeductionPercentageToPart(dPartPercentage, false);
//        SetDeductionPercentageToLabour(dLabourPercentage, false);
//        SetDeductionPercentageToLubricant(dLubricantPercentage, false);
//        SetDeductionPercentageToSublet(dSubletPercentage, false);
    }
}

// To Set Deduction Percentage To Part
function SetDeductionPercentageToPart(event, dVecvShare, bReadonly) {
    ////debugger;
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    //ObjGrid = $('#PartDetailsGrid');
    var ObjGrid = window.document.getElementById('PartDetailsGrid');
    hdnHeaderPrePartDeduction = window.document.getElementById('hdnHeaderPrePartDeduction');
    var txtPartHeaderDeduction = null;
    txtPartHeaderDeduction = window.document.getElementById('txtPartHeaderDeduction');
    if (ObjGrid.length == 0) return;
    //for (i = 1; i < ObjGrid[0].rows.length; i++) {
    for (i = 1; i < ObjGrid.rows.length; i++) {
        ////if (dGetValue(RoundupValue(ObjGrid.rows[i].cells[17].children[0].value)) + dGetValue(RoundupValue(dVecvShare)) > 100) {
        //if (dGetValue(RoundupValue(dVecvShare)) > 100) {
        //    //alert("Total Of all Percentage(Global + LineLevel + AutoDeduction : " + dVecvShare + "+" + RoundupValue(ObjGrid.rows[i].cells[17].children[0].value) + "+" + RoundupValue(ObjGrid.rows[i].cells[22].children[0].innerHTML) + ") is exceed to 100 in Line Level " + i + "! ");
        //    //alert("Deduction percentage is already present at linelevel for part,\n you can enter deduction percentage up to " + (100 - dGetValue(RoundupValue(ObjGrid.rows[i].cells[17].children[0].value))) + ".");
        //    alert("Deduction percentage is already present at linelevel for part,\n you can enter deduction percentage up to " + (100) + ".");
        //    for (j = i - 1; j >= 1; j--) {
        //        //ObjGrid.rows[j].cells[17].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[j].cells[17].children[0].value)) - dGetValue(RoundupValue(dVecvShare));
        //        ObjGrid.rows[j].cells[17].children[0].value = dGetValue(RoundupValue(dVecvShare));
        //        CalculatePartAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[17].children[0])
        //        //CalculatePartAcceptedAmtDeductionChanged(ObjGrid.rows[i].cells[17].children[0])
        //    }
        //    if (dGetValue(RoundupValue(txtPartHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value))<=100)
        //    txtPartHeaderDeduction.value = dGetValue(RoundupValue(txtPartHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value))
        //     txtPartHeaderDeduction.focus();
        //        return false;
        //}

        //ObjGrid.rows[i].cells[17].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[i].cells[17].children[0].value)) + dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[17].children[0].value = dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[17].children[0].readOnly = bReadonly;
        CalculatePartAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[17].children[0])
        //CalculatePartAcceptedAmtDeductionChanged(ObjGrid.rows[i].cells[17].children[0])
        
           
    }

    //if (dGetValue(RoundupValue(txtPartHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value)) > 100)
    //txtPartHeaderDeduction.value = dGetValue(RoundupValue(txtPartHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value));
    if (dGetValue(RoundupValue(txtPartHeaderDeduction.value)) > 100)
        return false;
    //else if (dVecvShare >0)
        txtPartHeaderDeduction.value = dGetValue(RoundupValue(txtPartHeaderDeduction.value));

    if (txtPartHeaderDeduction.value < 0)
        txtPartHeaderDeduction.value = -(txtPartHeaderDeduction.value)
    //if (dVecvShare > 0)
    hdnHeaderPrePartDeduction.value = txtPartHeaderDeduction.value;    
    return true;
}

// To Set Deduction Percentage To Labour
function SetDeductionPercentageToLabour(event, dVecvShare, bReadonly) {
    ////debugger;
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    //ObjGrid = $('#LabourDetailsGrid');
    var ObjGrid = window.document.getElementById('LabourDetailsGrid');
    hdnHeaderPreLabourDeduction = window.document.getElementById('hdnHeaderPreLabourDeduction');
    var txtLabourHeaderDeduction = null;
    txtLabourHeaderDeduction = window.document.getElementById('txtLabourHeaderDeduction');
    if (ObjGrid.length == 0) return;
    for (i = 1; i < ObjGrid.rows.length; i++) {      
        //if (dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value)) + dGetValue(RoundupValue(dVecvShare)) > 100) {
        //    alert("Deduction percentage is already present at linelevel for labor,\n you can enter deduction percentage up to " + (100 - dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value))) + ".");
        //    for (j = i - 1; j >= 1; j--) {
        //        ObjGrid.rows[j].cells[9].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[j].cells[9].children[0].value)) - dGetValue(RoundupValue(dVecvShare));
        //        CalculateLabourAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[9].children[0]);
        //    }
        //    if (dGetValue(RoundupValue(txtLabourHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value)) <= 100)
        //        txtLabourHeaderDeduction.value = dGetValue(RoundupValue(txtLabourHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value))
        //     txtLabourHeaderDeduction.focus();
        //    return false;
        //}

        //ObjGrid.rows[i].cells[9].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value)) + dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[9].children[0].value = dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[9].children[0].readOnly = bReadonly;
        CalculateLabourAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[9].children[0])
    }
    //if (dGetValue(RoundupValue(txtLabourHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value)) > 100)
    //txtLabourHeaderDeduction.value = dGetValue(RoundupValue(txtLabourHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value));
     if (dGetValue(RoundupValue(txtLabourHeaderDeduction.value)) > 100)
        return false;
    //else if (dVecvShare > 0)
        txtLabourHeaderDeduction.value = dGetValue(RoundupValue(txtLabourHeaderDeduction.value));

    if (txtLabourHeaderDeduction.value < 0)
        txtLabourHeaderDeduction.value = -(txtLabourHeaderDeduction.value)
    //if (dVecvShare > 0)
        hdnHeaderPreLabourDeduction.value = txtLabourHeaderDeduction.value;    
    return true;
}

// To Set Deduction Percentage To Lubricant
function SetDeductionPercentageToLubricant(event, dVecvShare, bReadonly) {
    ////debugger;
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    //ObjGrid = $('#LubricantDetailsGrid');
    var ObjGrid = window.document.getElementById('LubricantDetailsGrid');
    var txtLubricantHeaderDeduction = null;
    txtLubricantHeaderDeduction = window.document.getElementById('txtLubricantHeaderDeduction');
    hdnHeaderPreLubricantDeduction = window.document.getElementById('hdnHeaderPreLubricantDeduction');
    if (ObjGrid.length == 0) return;
    for (i = 1; i < ObjGrid.rows.length; i++) {       
        
        //if (dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value)) + dGetValue(RoundupValue(dVecvShare)) > 100) {
        //    alert("Deduction percentage is already present at linelevel for lubricant,\n you can enter deduction percentage up to " + (100 - dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value))) + ".");
        //    for (j = i - 1; j >= 1; j--) {
        //        ObjGrid.rows[j].cells[9].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[j].cells[9].children[0].value)) - dGetValue(RoundupValue(dVecvShare));
        //        CalculateLabourAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[9].children[0]);
        //    }
        //    if (dGetValue(RoundupValue(txtLubricantHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value)) <= 100)
        //        txtLubricantHeaderDeduction.value = dGetValue(RoundupValue(txtLubricantHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value))
        //    txtLubricantHeaderDeduction.focus();
        //    return false;
        //}

        //ObjGrid.rows[i].cells[9].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value)) + dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[9].children[0].value = dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[9].children[0].readOnly = bReadonly;
        CalculateLubricantAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[9].children[0])
    }
    //if (dGetValue(RoundupValue(txtLubricantHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value)) > 100)    
        if (dGetValue(RoundupValue(txtLubricantHeaderDeduction.value)) > 100)
            return false;
    //txtLubricantHeaderDeduction.value = dGetValue(RoundupValue(txtLubricantHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value));
        
    //else if (dVecvShare > 0)
        txtLubricantHeaderDeduction.value = dGetValue(RoundupValue(txtLubricantHeaderDeduction.value));

    if (txtLubricantHeaderDeduction.value < 0)
        txtLubricantHeaderDeduction.value = -(txtLubricantHeaderDeduction.value)
    //if (dVecvShare > 0)
        hdnHeaderPreLubricantDeduction.value = txtLubricantHeaderDeduction.value;    
    return true;
}

// To Set Deduction Percentage To Sublet
function SetDeductionPercentageToSublet(event, dVecvShare, bReadonly) {
    ////debugger;
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    //ObjGrid = $('#SubletDetailsGrid');
    var ObjGrid = window.document.getElementById('SubletDetailsGrid');
    hdnHeaderPreSubletDeduction = window.document.getElementById('hdnHeaderPreSubletDeduction');
    var txtSubletHeaderDeduction = null;
    txtSubletHeaderDeduction = window.document.getElementById('txtSubletHeaderDeduction');
    if (ObjGrid.length == 0) return;
    for (i = 1; i < ObjGrid.rows.length; i++) {
        
        //if (dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value)) + dGetValue(RoundupValue(dVecvShare)) > 100) {
        //    alert("Deduction percentage is already present at linelevel for sublet,\n you can enter deduction percentage up to " + (100 - dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value))) + ".");
        //    for (j = i - 1; j >= 1; j--) {
        //        ObjGrid.rows[j].cells[9].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[j].cells[9].children[0].value)) - dGetValue(RoundupValue(dVecvShare));
        //        CalculateLabourAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[9].children[0]);
        //    }
        //    if (dGetValue(RoundupValue(txtSubletHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value)) <= 100)
        //        txtSubletHeaderDeduction.value = dGetValue(RoundupValue(txtSubletHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value))
        //    txtSubletHeaderDeduction.focus();
        //    return false;
        //}
        //ObjGrid.rows[i].cells[9].children[0].value = dGetValue(RoundupValue(ObjGrid.rows[i].cells[9].children[0].value)) + dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[9].children[0].value = dGetValue(RoundupValue(dVecvShare));
        ObjGrid.rows[i].cells[9].children[0].readOnly = bReadonly;
        CalculateSubLetAcceptedAmtDeductionChanged(event, ObjGrid.rows[i].cells[9].children[0])
    }
    //if (dGetValue(RoundupValue(txtSubletHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value)) > 100)
     if (dGetValue(RoundupValue(txtSubletHeaderDeduction.value))> 100)
        return false;
    //else if (dVecvShare > 0)
        //txtSubletHeaderDeduction.value = dGetValue(RoundupValue(txtSubletHeaderDeduction.value)) + dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value));
        txtSubletHeaderDeduction.value = dGetValue(RoundupValue(txtSubletHeaderDeduction.value));

    if (txtSubletHeaderDeduction.value < 0)
        txtSubletHeaderDeduction.value = -(txtSubletHeaderDeduction.value)
    //if (dVecvShare > 0)
        hdnHeaderPreSubletDeduction.value = txtSubletHeaderDeduction.value; 
    return true;
}


// To Percent should be 100 For Claim wise
function AddDeductionPercentageToParts(event,objControl) {
    ////debugger;
    objControl = window.document.getElementById('txtPartHeaderDeduction');
    hdnHeaderPrePartDeduction = window.document.getElementById('hdnHeaderPrePartDeduction');   
    if (objControl.value > 100) {
        alert("Percentage should be less or equal to 100.");
        objControl.value = '';
        return false;
    }
    //objControl.value = dGetValue(RoundupValue(objControl.value)) - dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value))
    if (SetDeductionPercentageToPart(event, objControl.value, true) == false)
    return false;
//    if (dGetValue(RoundupValue(objControl.value)) + dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value)) > 100)
//        return false;
//    else
//        objControl.value = dGetValue(RoundupValue(objControl.value)) + dGetValue(RoundupValue(hdnHeaderPrePartDeduction.value));
//        
//    if (objControl.value < 0)
//        objControl.value = -(objControl.value)
//    hdnHeaderPrePartDeduction.value = objControl.value;

}
// To Percent should be 100 For Claim wise
function AddDeductionPercentageToLabour(event, objControl) {
    ////debugger;
    objControl = window.document.getElementById('txtLabourHeaderDeduction');
    hdnHeaderPreLabourDeduction = window.document.getElementById('hdnHeaderPreLabourDeduction');
    if (objControl.value > 100) {
        alert("Percentage should be less or equal to 100.");
        objControl.value = '';
        return false;
    }
   // objControl.value = dGetValue(RoundupValue(objControl.value)) - dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value))
    if (SetDeductionPercentageToLabour(event, objControl.value, true) == false)
    return false;
//    if (dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value)) + dGetValue(RoundupValue(objControl.value)) > 0)
//        return false;
//    else
//        objControl.value = dGetValue(RoundupValue(hdnHeaderPreLabourDeduction.value)) + dGetValue(RoundupValue(objControl.value));
//    if (objControl.value < 0)
//        objControl.value = -(objControl.value)
//    hdnHeaderPreLabourDeduction.value = objControl.value;

}
// To Percent should be 100 For Claim wise
function AddDeductionPercentageToLubricant(event, objControl) {
    ////debugger;
    objControl = window.document.getElementById('txtLubricantHeaderDeduction');
    hdnHeaderPreLubricantDeduction = window.document.getElementById('hdnHeaderPreLubricantDeduction');
    if (objControl.value > 100) {
        alert("Percentage should be less or equal to 100.");
        objControl.value = '';
        return false;
    }
    //objControl.value = dGetValue(RoundupValue(objControl.value)) - dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value))
    if (SetDeductionPercentageToLubricant(event, objControl.value, true) == false)
     return false;
//    if (dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value)) + dGetValue(RoundupValue(objControl.value)) > 100)
//        return false;
//    else        
//    objControl.value = dGetValue(RoundupValue(hdnHeaderPreLubricantDeduction.value)) + dGetValue(RoundupValue(objControl.value));
//    if (objControl.value < 0)
//        objControl.value = -(objControl.value)
//    hdnHeaderPreLubricantDeduction.value = objControl.value;

}
// To Percent should be 100 For Claim wise
function AddDeductionPercentageToSublet(event, objControl) {
    ////debugger;
    objControl = window.document.getElementById('txtSubletHeaderDeduction');
    hdnHeaderPreSubletDeduction = window.document.getElementById('hdnHeaderPreSubletDeduction');
    if (objControl.value > 100) {
        alert("Percentage should be less or equal to 100.");
        objControl.value = '';
        return false;
    }
   // objControl.value = dGetValue(RoundupValue(objControl.value)) - dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value))
    if (SetDeductionPercentageToSublet(event, objControl.value, true) == false)
      return false;
//    if (dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value)) + dGetValue(RoundupValue(objControl.value)) > 100)
//        return false;
//    else
//        objControl.value = dGetValue(RoundupValue(hdnHeaderPreSubletDeduction.value)) + dGetValue(RoundupValue(objControl.value));
//    if (objControl.value < 0)
//        objControl.value = -(objControl.value)
//    hdnHeaderPreSubletDeduction.value = objControl.value;

}

function MSGVAlidation() {
    ////debugger;
    var drpCulpritCode = window.document.getElementById('drpCulpritCode');
    var CulpritCode = drpCulpritCode.options[drpCulpritCode.selectedIndex];
    var drpDefectCode = window.document.getElementById('drpDefectCode');
    var DefectCode = drpDefectCode.options[drpDefectCode.selectedIndex];
    var rdoDeductionType = window.document.getElementById('rdoDeductionType');
    var HdnClaimType = window.document.getElementById('HdnClaimType')

    if (CulpritCode.value == "0" && CulpritCode.text == "--Select--" && HdnClaimType.value != "17") {
        alert("Please Select Culprit Code from Job Details Tab");
        return false;
    }

    if (DefectCode.value == "0" && DefectCode.text == "--Select--" && HdnClaimType.value != "17") {
        alert("Please Select Defect Code from Job Details Tab");
        return false;
    }
    
    if (rdoDeductionType.style.display == '') {
        var PcontainerName = '';
        var ObjGrid = null;
        var ObjID = null;
        PcontainerName = GetContainerName();

       

        //Check Deduction Message For Part
        ObjGrid = null;
        //ObjID = $('#PartDetailsGrid');
        ObjID = window.document.getElementById('PartDetailsGrid');        
        ObjGrid = ObjID[0];
        if (ObjGrid != null && ObjGrid.rows.length > 0) {

            for (i = 1; i < ObjGrid.rows.length; i++) {
                dDeductionRemark = ObjGrid.rows[i].cells[19].children[0].value
                if (dGetValue(ObjGrid.rows[i].cells[17].children[0].value) > dGetValue(dGetValue(ObjGrid.rows[i].cells[22].children[0].value) + dGetValue(ObjGrid.rows[i].cells[25].children[0].value))) {
                    if (dDeductionRemark.trim() == '') {
                        alert("Please Enter Part Deduction Remark")
                        ObjGrid.rows[i].cells[19].children[0].focus();
                        return false;
                    }
                }
                else if (dGetValue(ObjGrid.rows[i].cells[17].children[0].value) - dGetValue(dGetValue(ObjGrid.rows[i].cells[22].children[0].value) + dGetValue(ObjGrid.rows[i].cells[25].children[0].value)) == 0) {
                ObjGrid.rows[i].cells[19].children[0].value = '';
                }

            }
        }

        //Check Deduction Message For Labor
        ObjGrid = null;
        //ObjID = $('#LabourDetailsGrid');
        ObjID = window.document.getElementById('LabourDetailsGrid');        
        ObjGrid = ObjID[0];
        if (ObjGrid != null && ObjGrid.rows.length > 0) {
            for (i = 1; i < ObjGrid.rows.length; i++) {
                dDeductionRemark = ObjGrid.rows[i].cells[11].children[0].value
                if (dGetValue(ObjGrid.rows[i].cells[9].children[0].value) > dGetValue(dGetValue(ObjGrid.rows[i].cells[13].children[0].value) + dGetValue(ObjGrid.rows[i].cells[18].children[0].value))) {
                    if (dDeductionRemark.trim() == '') {
                        alert("Please Enter Labour Deduction Remark")
                        ObjGrid.rows[i].cells[11].children[0].focus();
                        return false;
                    }
                }
                else if (dGetValue(ObjGrid.rows[i].cells[9].children[0].value) - dGetValue(dGetValue(ObjGrid.rows[i].cells[13].children[0].value) + dGetValue(ObjGrid.rows[i].cells[18].children[0].value)) == 0) {
                ObjGrid.rows[i].cells[11].children[0].value = '';
                }

            }
        }

        //Check Deduction Message For Lubricant
        ObjGrid = null;
        //ObjID = $('#LubricantDetailsGrid');
        ObjID = window.document.getElementById('LubricantDetailsGrid');
        
        ObjGrid = ObjID[0];
        if (ObjGrid != null && ObjGrid.rows.length > 0) {
            for (i = 1; i < ObjGrid.rows.length; i++) {
                dDeductionRemark = ObjGrid.rows[i].cells[11].children[0].value
                if (dGetValue(ObjGrid.rows[i].cells[9].children[0].value) > dGetValue(dGetValue(ObjGrid.rows[i].cells[13].children[0].value) + dGetValue(ObjGrid.rows[i].cells[16].children[0].value))) {
                    if (dDeductionRemark.trim() == '') {
                        alert("Please Enter Lubricant Deduction Remark")
                        ObjGrid.rows[i].cells[11].children[0].focus();
                        return false;
                    }
                }
                else if (dGetValue(ObjGrid.rows[i].cells[9].children[0].value) - dGetValue(dGetValue(ObjGrid.rows[i].cells[13].children[0].value) + dGetValue(ObjGrid.rows[i].cells[16].children[0].value)) == 0) {
                ObjGrid.rows[i].cells[11].children[0].value = '';
                }

            }
        }
        //Check Deduction Message For Sublet
        ObjGrid = null;
        //ObjID = $('#SubletDetailsGrid');
        ObjID = window.document.getElementById('SubletDetailsGrid');        
        ObjGrid = ObjID[0];
        if (ObjGrid != null && ObjGrid.rows.length > 0) {
            for (i = 1; i < ObjGrid.rows.length; i++) {
                dDeductionRemark = ObjGrid.rows[i].cells[11].children[0].value
                if (dGetValue(ObjGrid.rows[i].cells[9].children[0].value) > dGetValue(dGetValue(ObjGrid.rows[i].cells[13].children[0].value) + dGetValue(ObjGrid.rows[i].cells[17].children[0].value))) {
                    if (dDeductionRemark.trim() == '') {
                        alert("Please Enter SubLet Deduction Remark")
                        ObjGrid.rows[i].cells[11].children[0].focus();
                        return false;
                    }
                }
                else if (dGetValue(ObjGrid.rows[i].cells[9].children[0].value) - dGetValue(dGetValue(ObjGrid.rows[i].cells[13].children[0].value) + dGetValue(ObjGrid.rows[i].cells[17].children[0].value)) == 0) {
                ObjGrid.rows[i].cells[11].children[0].value = '';
                }

            }
        }
    }
    else
        return;
}

function DisplayStatus(obj, SelectText) {
    ////debugger;
    var drpCulpritCode = window.document.getElementById('drpCulpritCode');
    var drpCulpritCodeTemp = window.document.getElementById('drpCulpritCodeTemp');
    var drpDefectCode = window.document.getElementById('drpDefectCode');
    var drpDefectCodeTemp = window.document.getElementById('drpDefectCodeTemp');
    if (SelectText != '' && SelectText != undefined) {
        if (obj.checked == true && SelectText == 'Culprit') {
            drpCulpritCodeTemp.style.display = "";
            drpCulpritCode.style.display = "none";
        }
        else if (obj.checked == false && SelectText == 'Culprit') {
            drpCulpritCodeTemp.style.display = "none";
            drpCulpritCode.style.display = "";
        }
        else if (obj.checked == true && SelectText == 'Defect') {
            drpDefectCodeTemp.style.display = "";
            drpDefectCode.style.display = "none";
        }
        else if (obj.checked == false && SelectText == 'Defect') {
            drpDefectCodeTemp.style.display = "none";
            drpDefectCode.style.display = "";
        }
    }
}
function CheckSelectedValue(SelectState) {
    ////debugger;
    var drpCulpritCode = window.document.getElementById('drpCulpritCode');
    var drpCulpritCodeTemp = window.document.getElementById('drpCulpritCodeTemp');
    var drpDefectCode = window.document.getElementById('drpDefectCode');
    var drpDefectCodeTemp = window.document.getElementById('drpDefectCodeTemp');
    
    
    if (SelectState != '' && SelectState != undefined) {
        if (SelectState == 'CulpritCode') {
            drpCulpritCodeTemp.value = drpCulpritCode.value;
        }
        else if (SelectState == 'CulpritCodeTemp') {
        drpCulpritCode.value = drpCulpritCodeTemp.value;
        }
        else if (SelectState == 'DefectCode') {
        drpDefectCodeTemp.value = drpDefectCode.value;
        }
        else if (SelectState == 'DefectCodeTemp') {
        drpDefectCode.value = drpDefectCodeTemp.value;
        }
    }
}


