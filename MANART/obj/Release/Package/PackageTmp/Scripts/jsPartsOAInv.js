//Invoice start from here
function SelectDeleteRowValueForInv(event, objCancelControl, objDeleteCheck) {
    //debugger;
    var objRow = objCancelControl.parentNode.parentNode.childNodes;

    if (objDeleteCheck.checked == true) {
        objRow[21].childNodes[1].value = 'D';
    }
    else {
        objRow[21].childNodes[1].value = 'E';
    }
    CalulateInvPartGranTotal();
    return false
}
function SelectDeletCheckboxAndCalcInv(event, ObjChkDelete) {
    //debugger;
    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            SelectDeleteRowValueForInv(event, ObjChkDelete.parentNode, ObjChkDelete);
        }
        else {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
            ObjChkDelete.checked = false;
            SelectDeleteRowValueForInv(event, ObjChkDelete.parentNode, ObjChkDelete);
            return false;
        }
    }
    else {
        if (confirm("Are you sure you want to revert changes?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
            SelectDeleteRowValueForInv(event, ObjChkDelete.parentNode, ObjChkDelete);
        }
        else {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            ObjChkDelete.checked = false;
            SelectDeleteRowValueForInv(event, ObjChkDelete.parentNode, ObjChkDelete);
            return false;
        }

    }
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

    CalulateInvPartGranTotal();
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

    CalulateInvPartGranTotal();
    return true;
}

function CalculateInvPartTotal(event, ObjQtyControl) {
    //debugger;
    var hdnIsDocGST = document.getElementById("ContentPlaceHolder1_hdnIsDocGST");
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objInvType = document.getElementById("ContentPlaceHolder1_DrpInvType");
        var sInvType = "";

        if (objInvType.selectedIndex == 0 && objInvType.value == "") {
            alert("Please Select The Invoice Type!.");
            return false;
        }
        else {
            sInvType = objInvType.options[objInvType.selectedIndex].value;
        }


        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
        //var Qty = dGetValue(ObjQtyControl.value);

        var MOQ = 1;
        var BalOAQty_def = 0;
        var StkBalQty_def = 0;
        var BFRGST_Stk_def = 0;
        var Total_bal = 0;
        var Total_BFRGST = 0;
        var sPartNo = "";
        var sLstTwoDigit = "";
        var sFirstFiveDigit = "";
        var dLabourTotal = 0;

        var OA_No = objRow[4].childNodes[1].value;
        var GroupCode = objRow[7].childNodes[1].value;
        var BalOAQty = dGetValue(objRow[8].childNodes[1].value);
        BalOAQty_def = dGetValue(objRow[8].childNodes[1].defaultValue);
        var defValue = dGetValue(objRow[9].childNodes[1].defaultValue);
        var Total_def = BalOAQty_def + defValue;

        var Qty = dGetValue(objRow[9].childNodes[1].value);
        var PrevQty = dGetValue(objRow[9].childNodes[3].value);
        var StkBalQty = dGetValue(objRow[10].childNodes[1].value);
        var BFRGST = objRow[28].childNodes[1].value;
        var BFRGST_Stk = dGetValue(objRow[29].childNodes[1].value);
        //CalCulate Total Part Stock = StkBalQty + defValue 
        Total_bal = StkBalQty + defValue;
        //Calculate Total BFR Stock =  BFRGST_Stk + defValue
        Total_BFRGST = BFRGST_Stk + defValue;

        if (dGetValue(Qty) > dGetValue(Total_def) && (OA_No != "") && (sInvType.trim() == "1" || sInvType.trim() == "2")) {
            alert("Invoice quanty cannot greater than Bal OA quantity");
            objRow[9].childNodes[1].value = "";
            objRow[9].childNodes[1].value = 0;
            objRow[9].childNodes[1].focus(); //objRow[6].childNodes[0].focus();
            objRow[8].childNodes[1].value = dGetValue(Total_def);
            return false;
        }
        // if (dGetValue(Qty) > dGetValue(StkBalQty) && (sInvType.trim() == "1" || sInvType.trim() == "2")) {
        if (dGetValue(Qty) > dGetValue(Total_bal) && (sInvType.trim() == "1" || sInvType.trim() == "2" || sInvType.trim() == "9")) {
            alert("Invoice quanty cannot greater than Part Stock quantity");
            objRow[9].childNodes[1].value = "";
            objRow[9].childNodes[1].value = 0;
            objRow[9].childNodes[1].focus();
            //objRow[10].childNodes[1].value = dGetValue(Total_bal);
            //objRow[29].childNodes[1].value = dGetValue(Total_BFRGST);
            return false;
        }

        //if (dGetValue(Qty) > dGetValue(BFRGST_Stk) && BFRGST.trim() == "Y" && hdnIsDocGST.value == "Y") {
        if (dGetValue(Qty) > dGetValue(Total_BFRGST) && BFRGST.trim() == "Y" && hdnIsDocGST.value == "Y" && (sInvType.trim() == "1" || sInvType.trim() == "2" || sInvType.trim() == "9")) {
            alert("Invoice quanty cannot greater than Pre GST Part Stock quantity");
            objRow[9].childNodes[1].value = "";
            objRow[9].childNodes[1].value = 0;
            objRow[9].childNodes[1].focus();
            //objRow[10].childNodes[1].value = dGetValue(Total_bal);
            //objRow[29].childNodes[1].value = dGetValue(Total_BFRGST);
        }

        sPartNo = objRow[5].childNodes[1].value;
        if (sPartNo != '' || sPartNo != '0') {
            sLstTwoDigit = sPartNo.substring(parseInt(sPartNo.Length) - 2, 2);
            sFirstFiveDigit = sPartNo.substring(0, 5);
            dLabourTotal = dGetValue(objRow[17].childNodes[5].value);
        }

        var FOBRate = dGetValue(objRow[13].childNodes[1].value);
        var DiscPer = dGetValue(objRow[14].childNodes[1].value);
        if (DiscPer >= 100 && (sInvType.trim() == "1" || sInvType.trim() == "2" || sInvType.trim() == "9")) {
            alert("100% and above discount not allowed");
            objRow[14].childNodes[1].value = "";
            objRow[14].childNodes[1].value = 0.00;
            DiscPer = 0.00;
        }
        else if (DiscPer > 100 && (sInvType.trim() == "3" || sInvType.trim() == "4" || sInvType.trim() == "5" || sInvType.trim() == "6" || sInvType.trim() == "7" || sInvType.trim() == "8")) {
            alert("100% and above discount not allowed");
            objRow[14].childNodes[1].value = "";
            objRow[14].childNodes[1].value = 0.00;
            DiscPer = 0.00;
        }

        var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
        if (isNaN(DiscAmt) == true) DiscAmt = 0;
        objRow[15].childNodes[1].value = parseFloat(DiscAmt).toFixed(2);

        var PartRate = dGetValue(FOBRate) - dGetValue(DiscAmt);
        if (isNaN(PartRate) == true) PartRate = 0;

        //Sujata 01092014_Begin 'Part tax Amt exclude from MRP to set Part rate and group tax calculation
        var PartTaxRate = objRow[18].children[4].value;
        var PartTax1Rate = objRow[18].children[6].value;
        var PartTax2Rate = objRow[18].children[8].value;

        var FOCTag = objRow[23].childNodes[1].value;
        var WarrTag = objRow[24].childNodes[1].value;

        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

        //Set Discounted Rate
        objRow[16].childNodes[1].value = parseFloat(PartRate).toFixed(4);
        var Total = (FOCTag.trim() == "N" && WarrTag.trim() == "N") ? (((sFirstFiveDigit == "MTIMI" || sFirstFiveDigit == "MTIOU" || sFirstFiveDigit == "MTICC") && DiscPer == 0) ? dLabourTotal : dGetValue(Qty) * PartRate) : 0;
        if (isNaN(Total) == true) Total = 0;
        //Set Calculated Total
        objRow[17].childNodes[1].value = parseFloat(Total).toFixed(2);

        // objRow[16].childNodes[3].value = parseFloat(PartRateExclTax).toFixed(2);
        //var PartTotalExclTax = dGetValue(Qty) * PartRateExclTax;
        //if (isNaN(PartTotalExclTax) == true) PartTotalExclTax = 0;
        //Comment on 20042017
        //objRow[17].childNodes[3].value = RoundupValue(PartTotalExclTax);
        /////////////////////////////////////////////////////////////   
        //Vikram Begin_05052017
        //if (GroupCode == '99') {
        //    objRow[17].childNodes[3].value = parseFloat(Total).toFixed(3);
        //}
        //else {
        //    //debugger;
        //    var Price = dGetValue(objRow[12].childNodes[1].value);
        //    var PartMainTaxRevRate = 0;
        //    PartMainTaxRevRate = Price / (1 + (PartTaxRate / 100))
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

        //    var RevDiscPer = dGetValue(objRow[14].childNodes[1].value);
        //    var RevDiscAmt = dGetValue(dGetValue(PartMainTaxRevRate) * dGetValue(DiscPer / 100));
        //    if (isNaN(RevDiscAmt) == true) RevDiscAmt = 0;
        //    var PartMainTaxRevRate = dGetValue(PartMainTaxRevRate) - dGetValue(RevDiscAmt);
        //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;
        //    //Set Rev Disc Rate
        //    objRow[16].childNodes[3].value = parseFloat(PartMainTaxRevRate).toFixed(2);
        //    var PartTotalMainTaxAmt = dGetValue(Qty) * PartMainTaxRevRate;

        //    objRow[17].childNodes[3].value = parseFloat(PartTotalMainTaxAmt).toFixed(3);
        //}
        //END
        objRow[17].childNodes[3].value = parseFloat(Total).toFixed(2);
        ///////////////////////////////////////
        CalulateInvPartGranTotal()

    }
}


function CalulateInvPartGranTotal() {
    debugger;
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGridGroupTax = document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridDocTax = document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
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
    var TotalInv = 0;
    var TotalInvRev = 0;

    var totalQtypart = 0;
    var sPArtName = "";
    var sGroupCode = "";

    var bPartSel = "";
    var CountRow = objGrid.rows.length;
    var CountRowGrTax = objGridGroupTax.rows.length;

    for (var k = 1; k < CountRowGrTax; k++) {
        objGridGroupTax.rows[k].childNodes[4].children[0].value = 0.00;
        //objGridGroupTax.rows[k].childNodes[4].children[1].value = 0.00;
    }

    for (var i = 1; i < CountRow; i++) {
        //New Code 20042017
        total = objGrid.rows[i].childNodes[17].children[1].value;
        totalRev = objGrid.rows[i].childNodes[17].children[0].value;
        sPArtName = objGrid.rows[i].childNodes[6].children[0].value;
        sGroupCode = objGrid.rows[i].childNodes[7].children[0].value;

        //Sujata 26052014
        //            bPartSel = objGrid.rows[i].childNodes[14].children[0].children[0].checked;
        //            if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel ==false) {
        //                TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
        //            }
        //            else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
        //                TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
        //            }
        //Sujata 26052014

        //bPartSel = objGrid.rows[i].childNodes[17].children[0].children[0].checked;
        bPartSel = objGrid.rows[i].childNodes[20].children[0].children[0].checked;
        sGridPartTax = objGrid.rows[i].childNodes[18].children[0].value.trim();
        // Heigest tax per for Pf Charges and Amt
        var PartTaxRate = objGrid.rows[i].childNodes[18].children[4].value;
        var PartTax1Rate = objGrid.rows[i].childNodes[18].children[6].value;
        if (isNaN(PartTaxRate) == true) PartTaxRate = 0;
        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;

        //sGridPartTax = objGrid.rows[i].childNodes[16].children[0].selectedIndex;
        // Find heighest tax per 

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value)).toFixed(2);

                //objGridGroupTax.rows[k].childNodes[4].children[1].value = dGetValue(totalRev) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[1].value);
                if (dGetValue(PartTaxRate) > dGetValue(dDocPFTaxPer)) {
                    dDocPFTaxPer = dGetValue(PartTaxRate);
                }
                if (dGetValue(PartTax1Rate) > dGetValue(dDocPFTaxPer1)) {
                    dDocPFTaxPer1 = dGetValue(PartTax1Rate);
                }
            }
        }

        if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel == false) {
            TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
        }
        else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
            TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
        }
        if (sPArtName != "" && bPartSel == false) {
            TotalInv = dGetValue(total) + dGetValue(TotalInv)
            TotalInvRev = dGetValue(totalRev) + dGetValue(TotalInvRev)
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
        //dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
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

        dDocTax2Amt = dGetValue(dDocTax2Amt) + dGetValue(dGrpTax2Amt);
        objGridGroupTax.rows[i].childNodes[18].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);
        //Vikram Begin_20042017
        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt))
        //dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt)  + dGetValue(dGrpTax2Amt))
        //Vikram End_20042017
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(2);
    }

    objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalInv).toFixed(2);
    //objGridDocTax.rows[1].childNodes[3].children[1].value = parseFloat(TotalInvRev).toFixed(2);
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

    var dDocPFTaxAmt1 = 0;
    var dDocPFTaxAmt2 = 0;

    if (IS_PerAmt == "Per") {
        dDocPFPer = objGridDocTax.rows[1].childNodes[10].children[0].value;
        if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
        dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    }
    else {
        dDocPFPer = 0.00;
        dDocPFAmt = dGetValue(objGridDocTax.rows[1].childNodes[11].children[0].value);
    }
    objGridDocTax.rows[1].childNodes[10].children[0].value = parseFloat(dDocPFPer).toFixed(2);//Set pf Percent
    objGridDocTax.rows[1].childNodes[11].children[0].value = parseFloat(dDocPFAmt).toFixed(2);// Set pf Amount

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

    //Set Heighest tax per to PF tax per and tax per 1
    objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocPFTaxPer).toFixed(2);
    objGridDocTax.rows[1].childNodes[16].children[0].value = parseFloat(dDocPFTaxPer1).toFixed(2);

    // Get Heigest Tax per for PF 
    dDocPFTaxPer = dGetValue(objGridDocTax.rows[1].childNodes[14].children[0].value);
    if (hdnCustTaxTag.value == "I") {
        dDocPFTaxAmt1 = dGetValue(dDocPFAmt) * dGetValue(dDocPFTaxPer / 100);
        objGridDocTax.rows[1].childNodes[15].children[0].value = parseFloat(dDocPFTaxAmt1).toFixed(2);
        dDocPFTaxAmt2 = dGetValue(dDocPFAmt) * dGetValue(dDocPFTaxPer / 100);
        objGridDocTax.rows[1].childNodes[17].children[0].value = parseFloat(dDocPFTaxAmt2).toFixed(2);
    }
    else {
        dDocPFTaxAmt1 = dGetValue(dDocPFAmt) * dGetValue(dDocPFTaxPer / 100);
        objGridDocTax.rows[1].childNodes[15].children[0].value = parseFloat(dDocPFTaxAmt1).toFixed(2);
    }

    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt) + dGetValue(dDocPFTaxAmt1) + dGetValue(dDocPFTaxAmt2));

    objGridDocTax.rows[1].childNodes[18].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(hdnIsRoundOFF.value == "Y" ? 0 : 2);

}
function SetCustType(ObjCust, ObjCustType) {
    var ParentCtrlID;
    var ObjCustTypeControl;

    ParentCtrlID = ObjCust.id.substring(0, ObjCust.id.lastIndexOf("_"));

    ObjCustTypeControl = document.getElementById(ParentCtrlID + "_" + ObjCustType);
    ObjCustTypeControl.selectedIndex = ObjCust.selectedIndex;
    return true;
}
//Invoice end