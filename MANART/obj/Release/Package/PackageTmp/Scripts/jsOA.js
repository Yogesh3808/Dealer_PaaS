//OA Related Calculation start
//function SelectDeletCheckboxAndCalc(ObjChkDelete) {
//    debugger;
//    if (ObjChkDelete.checked) {
//        if (confirm("Are you sure you want to delete this record?") == true) {
//            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
//            CalulateOAPartGranTotal();
//        }
//        else {
//            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
//            ObjChkDelete.checked = false;
//            CalulateOAPartGranTotal();
//            return false;
//        }
//    }
//    else {
//        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
//        CalulateOAPartGranTotal();
//    }
//}

function SetMainTax(ObjTax, ObjTaxPer, ObjTaxPerTxt, ObjTaxTag, ObjTaxTagTxt) {
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

    CalulateOAPartGranTotal();
    return true;
}

function SetAdditionalTax(ObjTax, ObjTaxPer, ObjTaxPerTxt) {
    var ParentCtrlID;
    var ObjTaxPerControl;
    var ObjTaxPerTxtControl;

    ParentCtrlID = ObjTax.id.substring(0, ObjTax.id.lastIndexOf("_"));

    ObjTaxPerControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPer);

    ObjTaxPerTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPerTxt);

    ObjTaxPerControl.selectedIndex = ObjTax.selectedIndex;

    ObjTaxPerTxtControl.value = dGetValue(ObjTaxPerControl[ObjTaxPerControl.selectedIndex].innerText);

    if (isNaN(ObjTaxPerTxtControl.value) == true) ObjTaxPerTxtControl.value = 0;

    CalulateOAPartGranTotal();
    return true;
}

function CalculateOAPartTotal(event, ObjQtyControl) {
    //debugger;
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var ObjID = $("#" + ObjQtyControl.id);
        var objRow = ObjID[0].parentNode.parentNode;

        //var MOQ = 1;
        //var Qty = dGetValue(ObjQtyControl.value);
        var GroupCode = objRow.cells[4].children[0].value;
        var Qty = dGetValue(objRow.cells[5].children[0].value);

        //GetFoBRate
        var FOBRate = dGetValue(objRow.cells[9].children[0].value);
        var DiscPer = dGetValue(objRow.cells[10].children[0].value);

        var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
        if (isNaN(DiscAmt) == true) DiscAmt = 0;
        //Set DiscAmt
        objRow.cells[11].children[0].value = parseFloat(DiscAmt).toFixed(2);

        var PartRate = dGetValue(FOBRate) - dGetValue(DiscAmt);
        if (isNaN(PartRate) == true) PartRate = 0;

        //Sujata 01092014_Begin 'Part tax Amt exclude from MRP to set Part rate and group tax calculation
        // Get Tax Percentages
        var PartTaxRate = dGetValue(objRow.cells[14].children[4].value);
        var PartTax1Rate = dGetValue(objRow.cells[14].children[6].value);
        var PartTax2Rate = dGetValue(objRow.cells[14].children[8].value);

        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

        //Set Discounted rate
        objRow.cells[12].children[0].value = parseFloat(PartRate).toFixed(4);
        var Total = dGetValue(Qty) * PartRate;
        //var Total = dGetValue(Qty) * dGetValue(objRow.cells[12].children[0].value);
        if (isNaN(Total) == true) Total = 0;
        //Set Total
        objRow.cells[13].children[0].value = parseFloat(Total).toFixed(2);
        //Vikram Begin_24052017
        //if (GroupCode == '99') {
        //    objRow.cells[13].children[1].value = parseFloat(Total).toFixed(2);
        //}
        //else {
        //    // Vikram Begin 29042017 new COde for reverse rate
        //    var Price = dGetValue(objRow.cells[8].children[0].value);
        //    var PartMainTaxRevRate = 0;
        //    PartMainTaxRevRate = Price / (1 + (PartTaxRate / 100))
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var RevDiscPer = dGetValue(objRow.cells[10].children[0].value);
        //    var RevDiscAmt = dGetValue(dGetValue(PartMainTaxRevRate) * dGetValue(DiscPer / 100));
        //    if (isNaN(RevDiscAmt) == true) RevDiscAmt = 0;
        //    var PartMainTaxRevRate = dGetValue(PartMainTaxRevRate) - dGetValue(RevDiscAmt);
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var PartTotalMainTaxAmt = dGetValue(Qty) * PartMainTaxRevRate;

        //    objRow.cells[13].children[1].value = parseFloat(PartTotalMainTaxAmt).toFixed(3);
        //}
        //END 
        objRow.cells[13].children[1].value = parseFloat(Total).toFixed(2);

        CalulateOAPartGranTotal()

    }
}


function CalulateOAPartGranTotal() {
    //debugger;
    var objGridID = $("#ContentPlaceHolder1_PartGrid")//document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGrid = objGridID[0];
    var objGridGroupTaxID = $("#ContentPlaceHolder1_GrdPartGroup")//document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridGroupTax = objGridGroupTaxID[0];
    var objGridDocTaxID = $("#ContentPlaceHolder1_GrdDocTaxDet")//document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    var objGridDocTax = objGridDocTaxID[0];
    var rbtLstDiscount = $("[id*=rbtLstDiscount] input:checked");
    var IS_PerAmt = rbtLstDiscount.val();
    var hdnCustTaxTag = document.getElementById("ContentPlaceHolder1_hdnCustTaxTag");
    var hdnIsRoundOFF = document.getElementById("ContentPlaceHolder1_hdnIsRoundOFF");

    var dDocPFTaxPer = 0;
    var dDocPFTaxPer1 = 0;
    var total = 0;
    var totalRev = 0;
    var TotalSparesRate = 0;
    var TotalOilRate = 0;
    var TotalOA = 0;
    var TotalRev = 0;

    var totalQtypart = 0;
    var sPArtName = "";
    var sGroupCode = "";
    var sGridPartTax = "";

    var bPartSel;
    var CountRow = objGrid.rows.length;
    var CountRowGrTax = objGridGroupTax.rows.length;

    for (var k = 1; k < CountRowGrTax; k++) {
        //objGridGroupTax.rows[k].childNodes[3].children[0].value = 0;
        //Net Taxable OA Amt
        objGridGroupTax.rows[k].cells[3].children[0].value = 0.00;
        //objGridGroupTax.rows[k].cells[3].children[1].value = 0.00;
    }

    for (var i = 1; i < CountRow; i++) {
        totalRev = objGrid.rows[i].cells[13].children[0].value;
        total = objGrid.rows[i].cells[13].children[1].value;
        sPArtName = objGrid.rows[i].cells[3].children[0].value;
        sGroupCode = objGrid.rows[i].cells[4].children[0].value;
        bPartSel = objGrid.rows[i].cells[15].children[0].children[0].checked;
        sGridPartTax = objGrid.rows[i].cells[14].children[0].value.trim();
        // Heigest tax per for Pf Charges and Amt
        var PartTaxRate = objGrid.rows[i].cells[14].children[4].value;
        var PartTax1Rate = objGrid.rows[i].cells[14].children[6].value;
        if (isNaN(PartTaxRate) == true) PartTaxRate = 0;
        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].cells[1].children[0].value.trim();
            var sMGrouptax = objGridGroupTax.rows[k].cells[6].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].cells[3].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].cells[3].children[0].value)).toFixed(2);
                //objGridGroupTax.rows[k].cells[3].children[1].value = dGetValue(totalRev) + dGetValue(objGridGroupTax.rows[k].cells[3].children[1].value);
                if (dGetValue(PartTaxRate) > dGetValue(dDocPFTaxPer)) {
                    dDocPFTaxPer = dGetValue(PartTaxRate);
                } // END IF
                if (dGetValue(PartTax1Rate) > dGetValue(dDocPFTaxPer1)) {
                    dDocPFTaxPer1 = dGetValue(PartTax1Rate);
                } //END IF
            }
        }

        if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel == false) {
            TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
        }
        else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
            TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
        }
        if (sPArtName != "" && bPartSel == false) {
            TotalOA = dGetValue(total) + dGetValue(TotalOA)
            TotalRev = dGetValue(totalRev) + dGetValue(TotalRev)
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
    var sGrpTax2ApplOn = "";

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
        dGrpTotal = parseFloat(dGetValue(objGridGroupTax.rows[i].cells[3].children[0].value)).toFixed(2); //dGetValue(objGridGroupTax.rows[i].childNodes[3].children[0].value);
        if (IS_PerAmt == "Per") {
            //group Percentage
            dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].cells[4].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
            //group Discount Amount
            dGrpDiscAmt = dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100));
        }
        else {
            //group Percentage
            dGrpDiscPer = 0.00;
            //group Discount Amount
            dGrpDiscAmt = dGetValue(objGridGroupTax.rows[i].cells[5].children[0].value);
        }
        //Set Discount Per again
        objGridGroupTax.rows[i].cells[4].children[0].value = dGetValue(dGrpDiscPer);
        //Doc Discount Amount
        dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
        //group Discount Amount display
        //objGridGroupTax.rows[i].childNodes[5].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
        objGridGroupTax.rows[i].cells[5].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
        //Amount whiich is applicable for tax
        dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);
        //Main tax calculation
        dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].cells[8].children[0].value);
        if (isNaN(dGrpMTaxPer) == true) dGrpMTaxPer = 0;

        dGrpMTaxAmt = dGetValue(dGetValue(dGrpTaxAppAmt) * dGetValue(dGrpMTaxPer / 100));
        sGrpMTaxTag = objGridGroupTax.rows[i].cells[7].children[2].value;
        //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
        if (sGrpMTaxTag == "I") {
            dDocLSTAmt = dGetValue(dDocLSTAmt) + dGetValue(dGrpMTaxAmt);
        }
        else if (sGrpMTaxTag == "O") {
            dDocCSTAmt = dGetValue(dDocCSTAmt) + dGetValue(dGrpMTaxAmt);
        }
        //objGridGroupTax.rows[i].childNodes[9].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);
        objGridGroupTax.rows[i].cells[9].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);

        dGrpTax1Per = dGetValue(objGridGroupTax.rows[i].cells[12].children[0].value);
        sGrpTax1ApplOn = dGetValue(objGridGroupTax.rows[i].cells[11].children[2].value);
        if (isNaN(dGrpTax1Per) == true) dGrpTax1Per = 0;
        
        if (sGrpTax1ApplOn == "1") {
            //Vikram Begin_20042017
            //dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) / (1 + dGetValue(dGrpTax1Per / 100))) * dGetValue(dGrpTax1Per / 100));
            //Vikram END_20042017
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax1Per / 100));
        } else if (sGrpTax1ApplOn == "3") {
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax1Per / 100));
        } else {
            dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        }
       
        dDocTax1Amt = dGetValue(dDocTax1Amt) + dGetValue(dGrpTax1Amt);
        //objGridGroupTax.rows[i].childNodes[13].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);
        objGridGroupTax.rows[i].cells[13].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);
        dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].cells[16].children[0].value);
        sGrpTax2ApplOn = dGetValue(objGridGroupTax.rows[i].cells[15].children[2].value);

        if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;
        if (sGrpTax2ApplOn == "1") {
            dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax2Per / 100));
        } else if (sGrpTax2ApplOn == "3") {
            dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax2Per / 100));
        } else {
            dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
        }

        dDocTax2Amt = dGetValue(dDocTax2Amt) + dGetValue(dGrpTax2Amt);
        //objGridGroupTax.rows[i].childNodes[17].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);
        objGridGroupTax.rows[i].cells[17].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);
        //Vikram Begin_02052017
        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt))
        //dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax2Amt))
        //END
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].cells[18].children[0].value = parseFloat(dGrpTotal).toFixed(3);

    }

    objGridDocTax.rows[1].cells[2].children[0].value = parseFloat(TotalOA).toFixed(2);
    //Vikram_Begin 02052017
    //objGridDocTax.rows[1].cells[2].children[1].value = parseFloat(TotalRev).toFixed(2);
    //END
    objGridDocTax.rows[1].cells[3].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);
    objGridDocTax.rows[1].cells[5].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);
    objGridDocTax.rows[1].cells[6].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);
    objGridDocTax.rows[1].cells[7].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);
    objGridDocTax.rows[1].cells[8].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);

    var dDocPFPer = 0;
    var dDocPFAmt = 0;
    var dDocOtherPer = 0;
    var dDocOtherAmt = 0;
    var dDocGrandAmt = 0;

    if (IS_PerAmt == "Per") {
        dDocPFPer = dGetValue(objGridDocTax.rows[1].cells[9].children[0].value); //objGridDocTax.rows[1].childNodes[9].children[0].value;
        if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
        dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    }
    else {
        dDocPFPer = 0.00;
        dDocPFAmt = dGetValue(objGridDocTax.rows[1].cells[10].children[0].value);
    }
    //Set pf Percent
    objGridDocTax.rows[1].cells[9].children[0].value = parseFloat(dDocPFPer).toFixed(2);//Set pf Percent
    objGridDocTax.rows[1].cells[10].children[0].value = parseFloat(dDocPFAmt).toFixed(2);// Set pf Amount

    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    if (IS_PerAmt == "Per") {
        // Other per Amount
        dDocOtherPer = objGridDocTax.rows[1].cells[11].children[0].value;
        if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
        dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
    } else {
        dDocOtherPer = 0.00;
        dDocOtherAmt = dGetValue(objGridDocTax.rows[1].cells[12].children[0].value);
    }
    // Set Other per
    objGridDocTax.rows[1].cells[11].children[0].value = parseFloat(dDocOtherPer).toFixed(2);
    //objGridDocTax.rows[1].childNodes[12].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    objGridDocTax.rows[1].cells[12].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);

    //Set Heighest tax per to PF tax per and tax per 1
    objGridDocTax.rows[1].cells[13].children[0].value = parseFloat(dDocPFTaxPer).toFixed(2);
    objGridDocTax.rows[1].cells[15].children[0].value = parseFloat(dDocPFTaxPer1).toFixed(2);

    //PF Tax Per and PF Tax Amount // Get Heigest Tax per for PF 
    debugger;
    dDocPFTaxPer = dGetValue(objGridDocTax.rows[1].childNodes[14].children[0].value);
    if (hdnCustTaxTag.value == "I") {
        dDocPFTaxAmt1 = dGetValue(dDocPFAmt) * dGetValue(dDocPFTaxPer / 100);
        objGridDocTax.rows[1].cells[14].children[0].value = parseFloat(dDocPFTaxAmt1).toFixed(2);
        dDocPFTaxAmt2 = dGetValue(dDocPFAmt) * dGetValue(dDocPFTaxPer / 100);
        objGridDocTax.rows[1].cells[16].children[0].value = parseFloat(dDocPFTaxAmt2).toFixed(2);
    }
    else {
        dDocPFTaxAmt1 = dGetValue(dDocPFAmt) * dGetValue(dDocPFTaxPer / 100);
        objGridDocTax.rows[1].cells[14].children[0].value = parseFloat(dDocPFTaxAmt1).toFixed(2);
    }
    //END

    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt) + dGetValue(dDocPFTaxAmt1) + dGetValue(dDocPFTaxAmt2));
    //dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));

    //objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);
    objGridDocTax.rows[1].cells[17].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(hdnIsRoundOFF.value == "Y" ? 0 : 2);
}
//OA Related Calculation End