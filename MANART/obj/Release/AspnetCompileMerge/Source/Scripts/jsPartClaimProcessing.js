// Calculate 
function CalculateAccPartClaimTotal(event, ObjQtyControl) {
    //debugger;
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

        var DespQty = objRow[11].childNodes[1].innerHTML;
        var ApprvQty = dGetValue(objRow[15].childNodes[1].value);
        var PartRate = dGetValue(objRow[24].childNodes[1].value);
        if (ApprvQty > DespQty) {
            alert("Part Claim Approved Quanity Must Be Less Than Or Equal Claim Quantity");
            objRow[15].childNodes[1].value = 0;
            objRow[15].childNodes[1].focus();
            return false;
        }
        if (isNaN(PartRate) == true) PartRate = 0;
        var Total = dGetValue(ApprvQty) * PartRate;
        if (isNaN(Total) == true) Total = 0;
        objRow[26].childNodes[1].value = parseFloat(Total).toFixed(2);//Set Total

        CalulateAccPartClaimGranTotal()

    }
}

function CalulateAccPartClaimGranTotal() {
    //debugger;
    var objGrid = document.getElementById("GridPartClaimDetail");
    var objGridGroupTax = document.getElementById("Acc_GrdPartGroup");
    var objGridDocTax = document.getElementById("Acc_GrdDocTaxDet");
    var hdnIsRoundOFF = document.getElementById("hdnIsRoundOFF");
    if (objGridGroupTax == null)
        return false;
    var Acctotal = 0;
    var TotalSparesRate = 0;
    var TotalOilRate = 0;
    var TotalOA = 0;

    var totalQtypart = 0;
    var sPArtName = "";
    var sGroupCode = "";
    var sGridPartTax = "";

    var bPartSel;
    var sStatus = "";
    var CountRow = objGrid.rows.length;
    var CountRowGrTax = objGridGroupTax.rows.length;

    for (var k = 1; k < CountRowGrTax; k++) {
        objGridGroupTax.rows[k].childNodes[4].children[0].value = 0.00;
    }

    for (var i = 1; i < CountRow; i++) {
        Acctotal = objGrid.rows[i].childNodes[26].children[0].value;
        sPArtName = objGrid.rows[i].childNodes[2].children[0].innerHTML;
        sGridPartTax = objGrid.rows[i].childNodes[27].children[0].value.trim();
        sStatus = 'N';
        sGroupCode = '01';

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && sStatus != "C" && sStatus != "D"
    && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = dGetValue(Acctotal) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value);
            }
        }

        if (sPArtName != "" && sGroupCode.trim() == "01" && sStatus != "C" && sStatus != "D") {
            TotalSparesRate = dGetValue(Acctotal) + dGetValue(TotalSparesRate)
        }
        else if (sPArtName != "" && sGroupCode.trim() == "02" && sStatus != "C" && sStatus != "D") {
            TotalOilRate = dGetValue(Acctotal) + dGetValue(TotalOilRate)
        }
        if (sPArtName != "" && sStatus != "C" && sStatus != "D") {
            TotalOA = dGetValue(Acctotal) + dGetValue(TotalOA)
        }
    }

    var CountGrpRow = objGridGroupTax.rows.length;

    var dGrpTotal = 0;
    var dGrpDiscPer = 0;
    var dGrpDiscAmt = 0;
    var dGrpTaxAppAmt = 0;

    var dGrpMTaxPer = 0;
    var dGrpMTaxAmt = 0;

    var dGrpTax1Per = 0;
    var dGrpTax1Amt = 0;

    var dGrpTax2Per = 0;
    var dGrpTax2Amt = 0;

    var dGrpTotal = 0;
    var dDocTotalAmtFrPFOther = 0;
    var dDocDiscAmt = 0;
    var dDocLSTAmt = 0;
    var dDocCSTAmt = 0;
    var dDocTax1Amt = 0;
    var dDocTax2Amt = 0;
    var sGrpMTaxTag = "";

    for (var i = 1; i < CountGrpRow; i++) {
        //group total
        dGrpTotal = dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
        //group Percentage
        dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].childNodes[5].children[0].value);
        //group Discount Amount
        dGrpDiscAmt = parseFloat(dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100))).toFixed(2);
        //Set Discount Per again
        objGridGroupTax.rows[i].childNodes[5].children[0].value = parseFloat(dGrpDiscPer).toFixed(2)
        //Doc Discount Amount
        dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
        //group Discount Amount display                                   
        objGridGroupTax.rows[i].childNodes[6].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
        //Amount whiich is applicable for tax
        dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);
        //Main tax calculation
        dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].childNodes[9].children[0].value);
        if (isNaN(dGrpMTaxPer) == true) dGrpMTaxPer = 0;
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

        if (sGrpTax1ApplOn == "1") {
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax1Per / 100));
        } else if (sGrpTax1ApplOn == "3") {
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax1Per / 100));
        } else {
            dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        }

        dDocTax1Amt = dGetValue(dDocTax1Amt) + dGetValue(dGrpTax1Amt);
        objGridGroupTax.rows[i].childNodes[14].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);

        dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].childNodes[17].children[0].value);
        //New
        sGrpTax2ApplOn = objGridGroupTax.rows[i].childNodes[16].children[2].value;

        if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;

        if (sGrpTax2ApplOn == "1") {
            dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax2Per / 100));
        } else if (sGrpTax2ApplOn == "3") {
            dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax2Per / 100));
        } else {
            dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
        }

        dDocTax2Amt = dGetValue(dDocTax2Amt) + dGetValue(dGrpTax2Amt);
        objGridGroupTax.rows[i].childNodes[18].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);
        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt))
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(2);
    }

    objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalOA).toFixed(2);
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

    //Set pf Percent
    objGridDocTax.rows[1].childNodes[10].children[0].value = parseFloat(dDocPFPer).toFixed(2);
    // Set pf Amount
    objGridDocTax.rows[1].childNodes[11].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    dDocOtherPer = objGridDocTax.rows[1].childNodes[12].children[0].value;
    if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
    dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));

    // Set Other per
    objGridDocTax.rows[1].childNodes[12].children[0].value = parseFloat(dDocOtherPer).toFixed(2);
    objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));

    objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(hdnIsRoundOFF.value == "Y" ? 0 : 2);

    var txtAccClaimAmt = document.getElementById("txtAccClaimAmt");
    txtAccClaimAmt.value = parseFloat(dDocTotalAmtFrPFOther).toFixed(hdnIsRoundOFF.value == "Y" ? 0 : 2);
}
function SetMainInvTax(ObjTax, ObjTaxPer, ObjTaxPerTxt, ObjTaxTag, ObjTaxTagTxt) {
    var ParentCtrlID;
    var ObjTaxPerControl;
    var ObjTaxPerTxtControl;
    var ObjTaxTagControl;
    var ObjTaxTagTxtControl;

    ParentCtrlID = ObjTax.id.substring(0, ObjTax.id.lastIndexOf("_"));

    ObjTaxPerControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPer);
    ObjTaxPerTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPerTxt);
    ObjTaxPerControl.selectedIndex = ObjTax.selectedIndex;
    ObjTaxPerTxtControl.value = dGetValue(ObjTaxPerControl[ObjTaxPerControl.selectedIndex].innerText);

    if (isNaN(ObjTaxPerTxtControl.value) == true) ObjTaxPerTxtControl.value = 0;

    ObjTaxTagControl = document.getElementById(ParentCtrlID + "_" + ObjTaxTag);
    ObjTaxTagTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxTagTxt);
    ObjTaxTagControl.selectedIndex = ObjTax.selectedIndex;
    ObjTaxTagTxtControl.value = ObjTaxTagControl[ObjTaxTagControl.selectedIndex].innerText.trim();

    if (isNaN(ObjTaxTagTxtControl.value) == true) ObjTaxTagTxtControl.value = 0;

    CalulateAccPartClaimGranTotal();
    return true;
}

function SetInvAdditionalTax(ObjTax, ObjTaxPer, ObjTaxPerTxt) {
    var ParentCtrlID;
    var ObjTaxPerControl;
    var ObjTaxPerTxtControl;

    ParentCtrlID = ObjTax.id.substring(0, ObjTax.id.lastIndexOf("_"));

    ObjTaxPerControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPer);

    ObjTaxPerTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPerTxt);

    ObjTaxPerControl.selectedIndex = ObjTax.selectedIndex;

    ObjTaxPerTxtControl.value = dGetValue(ObjTaxPerControl[ObjTaxPerControl.selectedIndex].innerText);

    if (isNaN(ObjTaxPerTxtControl.value) == true) ObjTaxPerTxtControl.value = 0;

    CalulateAccPartClaimGranTotal();
    return true;
}