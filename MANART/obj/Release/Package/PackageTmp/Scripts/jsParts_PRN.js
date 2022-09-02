function CalculatePRNPartTotal(event, ObjQtyControl) {
    debugger;
    var hdnIsAutoPRN = document.getElementById("ContentPlaceHolder1_hdnIsAutoPRN");
    var hdnSupplierType = document.getElementById("ContentPlaceHolder1_hdnSupplierType");
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

        var MOQ = 1;
        //var Qty = dGetValue(ObjQtyControl.value);
        var GroupCode = dGetValue(objRow[20].childNodes[3].value);
        var Qty = dGetValue(objRow[7].childNodes[1].value);
        var Inv_Qty = dGetValue(objRow[6].childNodes[1].value);
        var Inv_No = objRow[5].childNodes[1].value;
        var Prev_RetQty = dGetValue(objRow[7].childNodes[3].value);
        var dCurrStkQty = dGetValue(objRow[16].childNodes[1].value);

        if (dGetValue(Qty) > dGetValue(Inv_Qty) && (Inv_No != "")) {
            alert("Return quanty cannot greater than Invoice quantity");
            objRow[7].childNodes[1].focus(); //objRow[6].childNodes[0].focus();
            objRow[7].childNodes[1].value = 0;
            return false;
        }
        if (dGetValue(Qty) > (dGetValue(dCurrStkQty) + dGetValue(Prev_RetQty))) {
            alert("Return quanty cannot greater than Stock quantity");
            objRow[7].childNodes[1].focus(); //objRow[6].childNodes[0].focus();
            objRow[7].childNodes[1].value = 0;
            return false;
        }

        var FOBRate = dGetValue(objRow[11].childNodes[1].value);
        // Calculation Start Here

        //if ((hdnIsAutoPRN.value.trim() == "N" && hdnSupplierType.value.trim() == "M") || hdnSupplierType.value.trim() != "M") {
        var DiscPer = dGetValue(objRow[12].childNodes[1].value);
        var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
        var PartRate = dGetValue(FOBRate) - dGetValue(parseFloat(DiscAmt).toFixed(2));
        if (isNaN(PartRate) == true) PartRate = 0;
        var PartTaxRate = objRow[17].children[4].value;
        var PartTax1Rate = objRow[17].children[6].value;
        var PartTax2Rate = objRow[17].children[8].value;

        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

        objRow[13].childNodes[1].value = parseFloat(PartRate).toFixed(4);

        var Total = dGetValue(Qty) * PartRate;
        if (isNaN(Total) == true) Total = 0;
        objRow[14].childNodes[1].value = parseFloat(Total).toFixed(2);
        objRow[14].childNodes[3].value = parseFloat(Total).toFixed(2);
        //if (GroupCode == '99') {
        //    objRow[14].childNodes[3].value = parseFloat(Total).toFixed(3);
        //}
        //else {
        //    // Vikram Begin 29042017 new COde for reverse rate
        //    var Price = dGetValue(objRow[10].childNodes[1].value);
        //    var PartMainTaxRevRate = 0;
        //    PartMainTaxRevRate = Price / (1 + (PartTaxRate / 100))
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var RevDiscPer = dGetValue(objRow[12].childNodes[1].value);
        //    var RevDiscAmt = dGetValue(dGetValue(PartMainTaxRevRate) * dGetValue(DiscPer / 100));
        //    if (isNaN(RevDiscAmt) == true) RevDiscAmt = 0;
        //    var PartMainTaxRevRate = dGetValue(PartMainTaxRevRate) - dGetValue(RevDiscAmt);
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var PartTotalMainTaxAmt = dGetValue(Qty) * PartMainTaxRevRate;

        //    objRow[14].childNodes[3].value = parseFloat(PartTotalMainTaxAmt).toFixed(3);
        //}
        CalulatePRNPartGranTotal();
        //}//END Here
    }
}

function CalulatePRNPartGranTotal() {
    //debugger;
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGridGroupTax = document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridDocTax = document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    var hdnIsAutoPRN = document.getElementById("ContentPlaceHolder1_hdnIsAutoPRN");
    var hdnSupplierType = document.getElementById("ContentPlaceHolder1_hdnSupplierType");
    var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
    var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");
    var rbtLstDiscount = $("[id*=rbtLstDiscount] input:checked");
    var hdnIsRoundOFF = document.getElementById("ContentPlaceHolder1_hdnIsRoundOFF");
    var IS_PerAmt = rbtLstDiscount.val();
    if (objGridGroupTax == null)
        return false;
    var total = 0;
    var TotalSparesRate = 0;
    var TotalOilRate = 0;
    var TotalOA = 0;
    var TotalAmt = 0;
    var PartQty = 0;
    var totalQtypart = 0;
    var sPArtName = "";
    var sGroupCode = "";
    var sGridPartTax = "";

    var bPartSel;
    var sStatus = "";
    var CountRow = objGrid.rows.length;
    var CountRowGrTax = objGridGroupTax.rows.length;

    for (var k = 1; k < CountRowGrTax; k++) {
        objGridGroupTax.rows[k].childNodes[4].children[0].value = 0;
    }

    for (var i = 1; i < CountRow; i++) {
        total = objGrid.rows[i].childNodes[14].children[0].value;
        sPArtName = objGrid.rows[i].childNodes[4].children[0].value;
        sGroupCode = objGrid.rows[i].childNodes[20].children[1].value;
        bPartSel = objGrid.rows[i].childNodes[18].children[0].children[0].checked;
        //sGridPartTax = objGrid.rows[i].childNodes[14].children[0].selectedIndex;
        sGridPartTax = objGrid.rows[i].childNodes[17].children[0].value.trim();
        sStatus = objGrid.rows[i].childNodes[19].children[0].value;
        PartQty = dGetValue(objGrid.rows[i].childNodes[7].children[0].value);

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[6].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != ""  && bPartSel == false
    && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value), 2);
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
            TotalAmt = dGetValue(total) + dGetValue(TotalAmt)
            totalQtypart = dGetValue(totalQtypart) + dGetValue(PartQty)
        }
    }
    txtTotalQty.value = totalQtypart;
    txtTotal.value = parseFloat(dGetValue(TotalAmt)).toFixed(2);


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
        //debugger;
        //group total
        dGrpTotal = dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
        if (IS_PerAmt == "Per") {
            //group Percentage
            dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].childNodes[5].children[0].value);
            //group Discount Amount
            dGrpDiscAmt = parseFloat(dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100))).toFixed(2);
        } else {
            //group Percentage
            dGrpDiscPer = 0.00;
            //group Discount Amount
            dGrpDiscAmt = dGetValue(objGridGroupTax.rows[i].childNodes[6].children[0].value);
        }
        //Set Discount Per again
        objGridGroupTax.rows[i].childNodes[5].children[0].value = dGetValue(dGrpDiscPer);
        //Doc Discount Amount
        dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
        //group Discount Amount display                                   
        objGridGroupTax.rows[i].childNodes[6].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
        //Amount whiich is applicable for tax
        dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);

        //Main tax calculation
        dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].childNodes[9].children[0].value);
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
            dGrpTax1Amt = dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        } else {
            dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        }
        //Sujata 24092014End
        dDocTax1Amt = dGetValue(dDocTax1Amt) + dGetValue(dGrpTax1Amt);
        objGridGroupTax.rows[i].childNodes[14].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);

        dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].childNodes[17].children[0].value);
        //Sujata 24092014Begin
        //dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
        sGrpTax2ApplOn = objGridGroupTax.rows[i].childNodes[15].children[2].value;

        if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;

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