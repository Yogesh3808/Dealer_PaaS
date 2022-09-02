function SelectDeletCheckboxAndCalcSRN(event, ObjChkDelete) {
    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            SelectDeleteRowValueForSRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
        }
        else {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
            ObjChkDelete.checked = false;
            SelectDeleteRowValueForSRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
            return false;
        }
    }
    else {
        if (confirm("Are you sure you want to revert changes?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
            SelectDeleteRowValueForSRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
        }
        else {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            ObjChkDelete.checked = false;
            SelectDeleteRowValueForSRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
            return false;
        }
        //ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        //SelectDeleteRowValueForSRNPart(event, ObjChkDelete.parentNode, ObjChkDelete);
    }
}

function SelectDeleteRowValueForSRNPart(event, objCancelControl, objDeleteCheck) {
    debugger;
    var objRow = objCancelControl.parentNode.parentNode.childNodes;

    if (objDeleteCheck.checked == true) {
        objRow[19].childNodes[1].value = 'D';
    }
    else {
        objRow[19].childNodes[1].value = 'E';
    }
    CalulateSRNPartGranTotal();
    return false
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

    CalulateSRNPartGranTotal();
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

    CalulateSRNPartGranTotal();
    return true;
}

// To Get Part Id which are previously selected by user.
function GetPrevSelPartIDInvNoPONo(objNewPartLabel) {
    //debugger;
    var objRow;
    var PartIds = "";
    var PartId = "";
    var InvNo = "";
    var lblPartId;

    // get grid object
    var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
    if (objGrid == null) return PartIds;

    for (var i = 2; i <= objGrid.rows.length - 1; i++) {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Lable of Part ID
        lblPartId = objGrid.rows[i].children[1].childNodes[1];

        //Get Lable of InvNo 
        lblInvNo = objGrid.rows[i].children[4].childNodes[1];
        //Get PartId;

        if (lblPartId.value != "0" && lblPartId.value != "" && lblPartId.value != null) {
            PartId = lblPartId.value
        }

        if (lblInvNo.value != "0" && lblInvNo.value != "" && lblInvNo.value != null) {
            InvNo = lblInvNo.value
        }
        else {
            InvNo = "0";
        }

        PartId = InvNo + "-" + (lblPartId.value);
        if (PartId != "0" && PartId != "" && PartId != null) {
            PartIds = PartIds + PartId + ",";
        }
    }
    PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

    return PartIds;
}

// Calculate 
function CalculateSRNPartTotal(event, ObjQtyControl) {
    //debugger;
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

        var MOQ = 1;
        //var Qty = dGetValue(ObjQtyControl.value);
        var GroupCode = dGetValue(objRow[21].childNodes[1].value);
        var Qty = dGetValue(objRow[8].childNodes[1].value);
        var Inv_Qty = dGetValue(objRow[6].childNodes[1].value);
        var Inv_No = objRow[5].childNodes[1].value;
        if (dGetValue(Qty) > dGetValue(Inv_Qty) && (Inv_No != "")) {
            alert("Return quanty cannot greater than Invoice quantity");
            objRow[8].childNodes[1].focus(); //objRow[6].childNodes[0].focus();
            objRow[8].childNodes[1].value = 0;
            return false;
        }

        //GetRate
        var FOBRate = dGetValue(objRow[12].childNodes[1].value);
        var DiscPer = dGetValue(objRow[13].childNodes[1].value);

        var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
        objRow[14].childNodes[1].value = parseFloat(DiscAmt).toFixed(2);

        var PartRate = dGetValue(FOBRate) - dGetValue(DiscAmt);
        if (isNaN(PartRate) == true) PartRate = 0;

        var PartTaxRate = objRow[17].children[4].value;
        var PartTax1Rate = objRow[17].children[6].value;
        var PartTax2Rate = objRow[17].children[8].value;

        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

        //var PartRateTaxAmt = 1 + (dGetValue(PartTaxRate / 100) + dGetValue(PartTax1Rate / 100) + dGetValue(PartTax2Rate / 100));
        //if (isNaN(PartRateTaxAmt) == true) PartRateTaxAmt = 0;

        //var PartRateExclTax = dGetValue(PartRate / PartRateTaxAmt);

        //if (isNaN(PartRateExclTax) == true) PartRateExclTax = 0;
        // Set Discounted Rate
        objRow[15].childNodes[1].value = parseFloat(PartRate).toFixed(4);
        var Total = dGetValue(Qty) * PartRate;
        if (isNaN(Total) == true) Total = 0;
        //SetNewLabel Display
        objRow[16].childNodes[1].value = parseFloat(Total).toFixed(2);
        //if (GroupCode == '99') {
        //    objRow[16].childNodes[3].value = parseFloat(Total).toFixed(3);
        //}
        //else {
        //    // Vikram Begin 29042017 new COde for reverse rate
        //    var Price = dGetValue(objRow[11].childNodes[1].value);
        //    var PartMainTaxRevRate = 0;
        //    PartMainTaxRevRate = Price / (1 + (PartTaxRate / 100))
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var RevDiscPer = dGetValue(objRow[13].childNodes[1].value);
        //    var RevDiscAmt = dGetValue(dGetValue(PartMainTaxRevRate) * dGetValue(DiscPer / 100));
        //    if (isNaN(RevDiscAmt) == true) RevDiscAmt = 0;
        //    var PartMainTaxRevRate = dGetValue(PartMainTaxRevRate) - dGetValue(RevDiscAmt);
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var PartTotalMainTaxAmt = dGetValue(Qty) * PartMainTaxRevRate;

        //    objRow[16].childNodes[3].value = parseFloat(PartTotalMainTaxAmt).toFixed(3);
        //}

        //objRow[15].childNodes[3].value = parseFloat(PartRateExclTax).toFixed(2);
        //var PartTotalExclTax = dGetValue(Qty) * PartRateExclTax;
        //if (isNaN(PartTotalExclTax) == true) PartTotalExclTax = 0;
        //objRow[16].childNodes[3].value = RoundupValue(PartTotalExclTax);

        //var Total = dGetValue(Qty) * PartRate;

        ////SetNewLabel Display
        //objRow[16].childNodes[0].value = RoundupValue(Total);
        ////////
        objRow[16].childNodes[3].value = parseFloat(Total).toFixed(2);
        CalulateSRNPartGranTotal()

    }
}

function CalulateSRNPartGranTotal() {
    //debugger;
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGridGroupTax = document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridDocTax = document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    var rbtLstDiscount = $("[id*=rbtLstDiscount] input:checked");
    var IS_PerAmt = rbtLstDiscount.val();
    var hdnIsRoundOFF = document.getElementById("ContentPlaceHolder1_hdnIsRoundOFF");
    if (objGridGroupTax == null)
        return false;
    var total = 0;
    var TotalSparesRate = 0;
    var TotalOilRate = 0;
    var TotalOA = 0;
    var TotalRev = 0;

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
        //objGridGroupTax.rows[k].childNodes[4].children[1].value = 0;
    }

    for (var i = 1; i < CountRow; i++) {
        totalRev = objGrid.rows[i].childNodes[16].children[0].value;
        total = objGrid.rows[i].childNodes[16].children[1].value;
        sPArtName = objGrid.rows[i].childNodes[4].children[0].value;
        sGroupCode = objGrid.rows[i].childNodes[21].children[0].value;
        bPartSel = objGrid.rows[i].childNodes[18].children[0].children[0].checked;
        //sGridPartTax = objGrid.rows[i].childNodes[17].children[0].selectedIndex;
        sGridPartTax = objGrid.rows[i].childNodes[17].children[0].value.trim();
        sStatus = objGrid.rows[i].childNodes[19].children[0].value;

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && sStatus != "C" && sStatus != "D"
    && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value)).toFixed(2);
                //objGridGroupTax.rows[k].childNodes[4].children[1].value = dGetValue(totalRev) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[1].value);
            }
        }

        if (sPArtName != "" && sGroupCode.trim() == "01" && sStatus != "C" && sStatus != "D" && bPartSel == false) {
            TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
        }
        else if (sPArtName != "" && sGroupCode.trim() == "02" && sStatus != "C" && sStatus != "D" && bPartSel == false) {
            TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
        }
        if (sPArtName != "" && sStatus != "C" && sStatus != "D" && bPartSel == false) {
            TotalOA = dGetValue(total) + dGetValue(TotalOA)
            TotalRev = dGetValue(totalRev) + dGetValue(TotalRev)
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
        // if (objGridGroupTax.rows[i].className.indexOf('RowStyle') > 0) {
        //group total
        dGrpTotal = dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
        if (IS_PerAmt == "Per") {
            //group Percentage
            dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].childNodes[5].children[0].value);
            //group Discount Amount
            dGrpDiscAmt = parseFloat(dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100))).toFixed(2);
        }
        else {
            //group Percentage
            dGrpDiscPer = 0.00;
            //group Discount Amount
            dGrpDiscAmt = dGetValue(objGridGroupTax.rows[i].childNodes[6].children[0].value);
        }
        //Set Discount Per again
        objGridGroupTax.rows[i].childNodes[5].children[0].value = parseFloat(dGrpDiscPer).toFixed(2)
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
            //Vikram Begin_09052017
            //dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) / (1 + dGetValue(dGrpTax1Per / 100))) * dGetValue(dGrpTax1Per / 100));
            //Vikram END_09052017
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
        //Vikram Begin_09052017
        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt))
        //dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax2Amt))
        //END
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(2);
    }

    objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalOA).toFixed(2);
    //Vikram_Begin 09052017
    //objGridDocTax.rows[1].childNodes[3].children[1].value = parseFloat(TotalRev).toFixed(2);
    //END
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

    if (IS_PerAmt == "Per") {
        dDocPFPer = objGridDocTax.rows[1].childNodes[10].children[0].value;
        if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
        dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    }
    else {
        dDocPFPer = 0.00;
        dDocPFAmt = dGetValue(objGridDocTax.rows[1].childNodes[11].children[0].value);
    }
    //Set pf Percent
    objGridDocTax.rows[1].childNodes[10].children[0].value = parseFloat(dDocPFPer).toFixed(2);
    // Set pf Amount
    objGridDocTax.rows[1].childNodes[11].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    if (IS_PerAmt == "Per") {
        dDocOtherPer = objGridDocTax.rows[1].childNodes[12].children[0].value;
        if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
        dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
    }
    else {
        dDocOtherPer = 0.00;
        dDocOtherAmt = dGetValue(objGridDocTax.rows[1].childNodes[13].children[0].value);
    }
    // Set Other per
    objGridDocTax.rows[1].childNodes[12].children[0].value = parseFloat(dDocOtherPer).toFixed(2);
    objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));


    objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(hdnIsRoundOFF.value == "Y" ? 0 : 2);
}