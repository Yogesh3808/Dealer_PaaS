////OA Related Calculation start
////function SelectDeletCheckboxAndCalc(ObjChkDelete) {
////    //debugger;
////    if (ObjChkDelete.checked) {
////        if (confirm("Are you sure you want to delete this record?") == true) {
////            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
////            CalulateOAPartGranTotal();
////        }
////        else {
////            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
////            ObjChkDelete.checked = false;
////            CalulateOAPartGranTotal();
////            return false;
////        }
////    }
////    else {
////        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
////        CalulateOAPartGranTotal();
////    }
////}

//function SetMainTax(ObjTax, ObjTaxPer, ObjTaxPerTxt, ObjTaxTag, ObjTaxTagTxt) {
//    var ParentCtrlID;
//    var ObjTaxPerControl;
//    var ObjTaxPerTxtControl;
//    var ObjTaxTagControl;
//    var ObjTaxTagTxtControl;

//    ParentCtrlID = ObjTax.id.substring(0, ObjTax.id.lastIndexOf("_"));

//    ObjTaxPerControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPer);
//    ObjTaxPerTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPerTxt);
//    ObjTaxPerControl.selectedIndex = ObjTax.selectedIndex;
//    ObjTaxPerTxtControl.value = dGetValue(ObjTaxPerControl[ObjTaxPerControl.selectedIndex].innerText);
//    if (isNaN(ObjTaxPerTxtControl.value) == true) ObjTaxPerTxtControl.value = 0;

//    ObjTaxTagControl = document.getElementById(ParentCtrlID + "_" + ObjTaxTag);
//    ObjTaxTagTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxTagTxt);
//    ObjTaxTagControl.selectedIndex = ObjTax.selectedIndex;
//    ObjTaxTagTxtControl.value = ObjTaxTagControl[ObjTaxTagControl.selectedIndex].innerText.trim();

//    if (isNaN(ObjTaxTagTxtControl.value) == true) ObjTaxTagTxtControl.value = 0;

//    CalulateOAPartGranTotal();
//    return true;
//}

//function SetAdditionalTax(ObjTax, ObjTaxPer, ObjTaxPerTxt) {
//    var ParentCtrlID;
//    var ObjTaxPerControl;
//    var ObjTaxPerTxtControl;

//    ParentCtrlID = ObjTax.id.substring(0, ObjTax.id.lastIndexOf("_"));

//    ObjTaxPerControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPer);

//    ObjTaxPerTxtControl = document.getElementById(ParentCtrlID + "_" + ObjTaxPerTxt);

//    ObjTaxPerControl.selectedIndex = ObjTax.selectedIndex;

//    ObjTaxPerTxtControl.value = dGetValue(ObjTaxPerControl[ObjTaxPerControl.selectedIndex].innerText);

//    if (isNaN(ObjTaxPerTxtControl.value) == true) ObjTaxPerTxtControl.value = 0;

//    CalulateOAPartGranTotal();
//    return true;
//}

//function CalculateOAPartTotal(event, ObjQtyControl) {
//    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
//        //ObjControl.focus(); 
//        return;
//    }
//    else {
//        var ObjID = $("#" + ObjQtyControl.id);
//        var objRow = ObjID[0].parentNode.parentNode; // ObjQtyControl.parentNode.parentNode.childNodes;

//        var MOQ = 1;
//        //var Qty = dGetValue(ObjQtyControl.value);
//        var Qty = dGetValue(objRow.cells[5].children[0].value); //dGetValue(objRow[5].childNodes[0].value);

//        //        if (MOQ != 0 && (Qty % MOQ) != 0) {
//        //            if (Qty / MOQ != 0) {
//        //                ObjQtyControl.value = (parseInt(Qty / MOQ) + 1) * MOQ
//        //            }
//        //        }

//        //GetFoBRate
//        var FOBRate = dGetValue(objRow.cells[7].children[0].value); //dGetValue(objRow[7].childNodes[0].value);
//        var DiscPer = dGetValue(objRow.cells[8].children[0].value); //dGetValue(objRow[8].childNodes[0].value);

//        var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
//        if (isNaN(DiscAmt) == true) DiscAmt = 0;
//        //objRow[9].childNodes[0].value = parseFloat(DiscAmt).toFixed(2);
//        objRow.cells[9].children[0].value = parseFloat(DiscAmt).toFixed(2);

//        var PartRate = dGetValue(FOBRate) - dGetValue(DiscAmt);
//        if (isNaN(PartRate) == true) PartRate = 0;

//        //Sujata 01092014_Begin 'Part tax Amt exclude from MRP to set Part rate and group tax calculation
//        var PartTaxRate = dGetValue(objRow.cells[12].children[0].value); //objRow[12].children[4].value;
//        var PartTax1Rate = dGetValue(objRow.cells[12].children[1].value); //objRow[12].children[6].value;
//        var PartTax2Rate = dGetValue(objRow.cells[12].children[2].value); //objRow[12].children[8].value;

//        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
//        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

//        //        var PartRateTaxAmt = dGetValue((PartRate * dGetValue(dGetValue(PartTaxRate) / 100)));
//        //        var PartRateTax1Amt = dGetValue((PartRate * dGetValue(dGetValue(PartTax1Rate) / 100)));
//        //        var PartRateTax2Amt = dGetValue((PartRate * dGetValue(dGetValue(PartTax2Rate) / 100)));

//        //        if (isNaN(PartRateTaxAmt) == true) PartRateTaxAmt = 0;
//        //        if (isNaN(PartRateTax1Amt) == true) PartRateTax1Amt = 0;
//        //        if (isNaN(PartRateTax2Amt) == true) PartRateTax2Amt = 0;

//        //var PartRateExclTax = dGetValue(PartRate - (dGetValue(PartRateTaxAmt) + dGetValue(PartRateTax1Amt) + dGetValue(PartRateTax2Amt)));

//        var PartRateTaxAmt = 1 + (dGetValue(PartTaxRate / 100) + dGetValue(PartTax1Rate / 100) + dGetValue(PartTax2Rate / 100));
//        if (isNaN(PartRateTaxAmt) == true) PartRateTaxAmt = 0;

//        var PartRateExclTax = dGetValue(PartRate / PartRateTaxAmt);

//        if (isNaN(PartRateExclTax) == true) PartRateExclTax = 0;

//        //Sujata 01092014_End
//        //objRow[10].childNodes[0].value = parseFloat(PartRate).toFixed(2);
//        objRow.cells[10].children[0].value = parseFloat(PartRate).toFixed(2);
//        var Total = dGetValue(Qty) * PartRate;
//        if (isNaN(Total) == true) Total = 0;
//        //SetNewLabel Display
//        //objRow[11].childNodes[0].value = RoundupValue(Total);
//        objRow.cells[11].children[0].value = parseFloat(Total).toFixed(2);

//        //objRow[10].childNodes[2].value = parseFloat(PartRateExclTax).toFixed(2);
//        objRow.cells[10].children[1].value = parseFloat(PartRateExclTax).toFixed(2);
//        var PartTotalExclTax = dGetValue(Qty) * PartRateExclTax;
//        if (isNaN(PartTotalExclTax) == true) PartTotalExclTax = 0;
//        //objRow[11].childNodes[2].value = RoundupValue(PartTotalExclTax);
//        objRow.cells[11].children[1].value = parseFloat(PartRateExclTax).toFixed(2);

//        CalulateOAPartGranTotal()

//    }
//}


//function CalulateOAPartGranTotal() {
//    //debugger;
//    var objGridID = $("#ContentPlaceHolder1_PartGrid")//document.getElementById("ContentPlaceHolder1_PartGrid");
//    var objGrid = objGridID[0];
//    var objGridGroupTaxID = $("#ContentPlaceHolder1_GrdPartGroup")//document.getElementById("ContentPlaceHolder1_GrdPartGroup");
//    var objGridGroupTax = objGridGroupTaxID[0];
//    var objGridDocTaxID = $("#ContentPlaceHolder1_GrdDocTaxDet")//document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
//    var objGridDocTax = objGridDocTaxID[0];

//    var total = 0;
//    var TotalSparesRate = 0;
//    var TotalOilRate = 0;
//    var TotalOA = 0;

//    var totalQtypart = 0;
//    var sPArtName = "";
//    var sGroupCode = "";
//    var sGridPartTax = "";

//    var bPartSel;
//    var CountRow = objGrid.rows.length;
//    var CountRowGrTax = objGridGroupTax.rows.length;

//    for (var k = 0; k < CountRowGrTax; k++) {
//        if (objGridGroupTax.rows[k].className.indexOf('RowStyle') > 0) {
//            //objGridGroupTax.rows[k].childNodes[3].children[0].value = 0;
//            objGridGroupTax.rows[k].cells[3].children[0].value = 0;
//        }
//    }

//    for (var i = 0; i < CountRow; i++) {
//        if (objGrid.rows[i].className.indexOf('RowStyle') > 0) {
//            total = objGrid.rows[i].cells[11].children[0].value; //objGrid.rows[i].childNodes[11].children[1].value;
//            sPArtName = objGrid.rows[i].cells[3].children[0].value; //objGrid.rows[i].childNodes[3].children[0].value;
//            sGroupCode = objGrid.rows[i].cells[4].children[0].value; //objGrid.rows[i].childNodes[4].children[0].value;
//            //Sujata 26052014
//            //            bPartSel = objGrid.rows[i].childNodes[12].children[0].children[0].checked;
//            //            if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel == false) {
//            //                TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
//            //            }
//            //            else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
//            //                TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
//            //            }
//            //Sujata 26052014

//            bPartSel = objGrid.rows[i].cells[13].children[0].children[0].checked; //objGrid.rows[i].childNodes[13].children[0].children[0].checked;
//            sGridPartTax = objGrid.rows[i].cells[12].children[0].selectedIndex; //objGrid.rows[i].childNodes[12].children[0].selectedIndex;

//            for (var k = 0; k < CountRowGrTax; k++) {
//                if (objGridGroupTax.rows[k].className.indexOf('RowStyle') > 0) {
//                    var sMGroupCode = objGridGroupTax.rows[k].cells[1].children[0].value.trim(); //objGridGroupTax.rows[k].childNodes[1].children[0].value.trim();
//                    var sMGrouptax = objGridGroupTax.rows[k].cells[6].children[0].value.trim(); //objGridGroupTax.rows[k].childNodes[6].children[0].selectedIndex;
//                    if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
//                    && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
//                        //objGridGroupTax.rows[k].childNodes[3].children[0].value = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[3].children[0].value);
//                        objGridGroupTax.rows[k].cells[3].children[0].value = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].cells[3].children[0].value);
//                    }
//                }
//            }

//            if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel == false) {
//                TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
//            }
//            else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
//                TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
//            }
//            if (sPArtName != "" && bPartSel == false) {
//                TotalOA = dGetValue(total) + dGetValue(TotalOA)
//            }
//        }
//    }

//    //    objGridGroupTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalSparesRate).toFixed(2);
//    //    objGridGroupTax.rows[2].childNodes[3].children[0].value = parseFloat(TotalOilRate).toFixed(2);

//    var CountGrpRow = objGridGroupTax.rows.length;

//    var dGrpTotal = 0;
//    var dGrpDiscPer = 0;
//    var dGrpDiscAmt = 0;
//    var dGrpTaxAppAmt = 0;

//    var dGrpMTaxPer = 0;
//    var dGrpMTaxAmt = 0;

//    var dGrpTax1Per = 0;
//    var dGrpTax1Amt = 0;
//    var sGrpTax1ApplOn = "";

//    var dGrpTax2Per = 0;
//    var dGrpTax2Amt = 0;
//    var sGrpTax2ApplOn = "";

//    var dGrpTotal = 0;
//    var dDocTotalAmtFrPFOther = 0;
//    var dDocDiscAmt = 0;
//    var dDocLSTAmt = 0;
//    var dDocCSTAmt = 0;
//    var dDocTax1Amt = 0;
//    var dDocTax2Amt = 0;
//    var sGrpMTaxTag = "";

//    for (var i = 0; i < CountGrpRow; i++) {
//        if (objGridGroupTax.rows[i].className.indexOf('RowStyle') > 0) {
//            //group total
//            dGrpTotal = dGetValue(objGridGroupTax.rows[i].cells[3].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[3].children[0].value);
//            //group Percentage
//            dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].cells[4].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
//            //group Discount Amount
//            dGrpDiscAmt = dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100));
//            //Doc Discount Amount
//            dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
//            //group Discount Amount display
//            //objGridGroupTax.rows[i].childNodes[5].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
//            objGridGroupTax.rows[i].cells[5].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
//            //Amount whiich is applicable for tax
//            dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);
//            //Main tax calculation
//            dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].cells[8].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[8].children[0].value);
//            if (isNaN(dGrpMTaxPer) == true) dGrpMTaxPer = 0;

//            dGrpMTaxAmt = dGetValue(dGetValue(dGrpTaxAppAmt) * dGetValue(dGrpMTaxPer / 100));
//            sGrpMTaxTag = dGetValue(objGridGroupTax.rows[i].cells[7].children[0].value); //objGridGroupTax.rows[i].childNodes[7].children[2].value;
//            //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
//            if (sGrpMTaxTag == "I") {
//                dDocLSTAmt = dGetValue(dDocLSTAmt) + dGetValue(dGrpMTaxAmt);
//            }
//            else if (sGrpMTaxTag == "O") {
//                dDocCSTAmt = dGetValue(dDocCSTAmt) + dGetValue(dGrpMTaxAmt);
//            }
//            //objGridGroupTax.rows[i].childNodes[9].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);
//            objGridGroupTax.rows[i].cells[9].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);

//            dGrpTax1Per = dGetValue(objGridGroupTax.rows[i].cells[12].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[12].children[0].value);
//            sGrpTax1ApplOn = dGetValue(objGridGroupTax.rows[i].cells[11].children[2].value); //objGridGroupTax.rows[i].childNodes[11].children[2].value;
//            if (isNaN(dGrpTax1Per) == true) dGrpTax1Per = 0;
//            //Sujata 24092014Begin
//            //dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
//            if (sGrpTax1ApplOn == "1") {
//                dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax1Per / 100));
//            } else if (sGrpTax1ApplOn == "3") {
//                dGrpTax1Amt = dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
//            } else {
//                dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
//            }
//            //Sujata 24092014End
//            dDocTax1Amt = dGetValue(dDocTax1Amt) + dGetValue(dGrpTax1Amt);
//            //objGridGroupTax.rows[i].childNodes[13].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);
//            objGridGroupTax.rows[i].cells[13].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);

//            dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].cells[16].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[16].children[0].value);
//            sGrpTax2ApplOn = dGetValue(objGridGroupTax.rows[i].cells[15].children[2].value); //objGridGroupTax.rows[i].childNodes[15].children[2].value;

//            if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;
//            //Sujata 24092014Begin
//            //dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
//            if (sGrpTax2ApplOn == "1") {
//                dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax2Per / 100));
//            } else if (sGrpTax2ApplOn == "3") {
//                dGrpTax2Amt = dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
//            } else {
//                dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
//            }
//            //Sujata 24092014End

//            dDocTax2Amt = dGetValue(dDocTax2Amt) + dGetValue(dGrpTax2Amt);
//            //objGridGroupTax.rows[i].childNodes[17].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);
//            objGridGroupTax.rows[i].cells[17].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);

//            dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt));
//            dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
//            //objGridGroupTax.rows[i].childNodes[18].children[0].value = parseFloat(dGrpTotal).toFixed(2);
//            objGridGroupTax.rows[i].cells[18].children[0].value = parseFloat(dGrpTotal).toFixed(2);

//        }
//    }

//    //objGridDocTax.rows[1].childNodes[2].children[0].value = parseFloat(TotalOA).toFixed(2);
//    objGridDocTax.rows[1].cells[2].children[0].value = parseFloat(TotalOA).toFixed(2);   

//    //objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);
//    objGridDocTax.rows[1].cells[3].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);

//    //objGridDocTax.rows[1].childNodes[5].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);
//    objGridDocTax.rows[1].cells[5].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);

//    //objGridDocTax.rows[1].childNodes[6].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);
//    objGridDocTax.rows[1].cells[6].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);

//    //objGridDocTax.rows[1].childNodes[7].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);
//    objGridDocTax.rows[1].cells[7].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);
//    //objGridDocTax.rows[1].childNodes[8].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);
//    objGridDocTax.rows[1].cells[8].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);

//    var dDocPFPer = 0;
//    var dDocPFAmt = 0;
//    var dDocOtherPer = 0;
//    var dDocOtherAmt = 0;
//    var dDocGrandAmt = 0;

//    dDocPFPer = dGetValue(objGridDocTax.rows[1].cells[9].children[0].value); //objGridDocTax.rows[1].childNodes[9].children[0].value;
//    if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
//    dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
//    //objGridDocTax.rows[1].childNodes[10].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
//    objGridDocTax.rows[1].cells[10].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
//    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

//    dDocOtherPer = objGridDocTax.rows[1].childNodes[11].children[0].value;
//    if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
//    dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
//    //objGridDocTax.rows[1].childNodes[12].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
//    objGridDocTax.rows[1].cells[12].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
//    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));

//    //objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);
//    objGridDocTax.rows[1].cells[13].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);
//}
////OA Related Calculation End

//Invoice start from here


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
    ////debugger;
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {        

        var ObjID = $("#" + ObjQtyControl.id);
        var objRow = ObjID[0].parentNode.parentNode; // ObjQtyControl.parentNode.parentNode.childNodes;


        var MOQ = 1;
        //var Qty = dGetValue(ObjQtyControl.value);
        var Qty = dGetValue(objRow.cells[7].children[0].value); //dGetValue(objRow[7].childNodes[0].value);

        //        if (MOQ != 0 && (Qty % MOQ) != 0) {
        //            if (Qty / MOQ != 0) {
        //                ObjQtyControl.value = (parseInt(Qty / MOQ) + 1) * MOQ
        //            }
        //        }

        //GetFoBRate
        var FOBRate = dGetValue(objRow.cells[9].children[0].value); //dGetValue(objRow[9].childNodes[0].value);
        var DiscPer = dGetValue(objRow.cells[10].children[0].value); //dGetValue(objRow[10].childNodes[0].value);

        var DiscAmt = dGetValue(dGetValue(FOBRate) * dGetValue(DiscPer / 100));
        //objRow[11].childNodes[0].value = parseFloat(DiscAmt).toFixed(2);
        objRow.cells[11].children[0].value = parseFloat(DiscAmt).toFixed(2);

        var PartRate = dGetValue(FOBRate) - dGetValue(DiscAmt);
        //Sujata 01092014_Begin 'Part tax Amt exclude from MRP to set Part rate and group tax calculation
        var PartTaxRate = dGetValue(objRow.cells[14].children[0].value); //objRow[14].children[4].value;

        if (isNaN(PartRate) == true) PartRate = 0;
        if (isNaN(DiscAmt) == true) DiscAmt = 0;

        //Sujata 05092014_End
        //var PartRateExclTax = dGetValue(PartRate - dGetValue((PartRate * dGetValue(dGetValue(PartTaxRate) / 100))));
        var PartTax1Rate = dGetValue(objRow.cells[14].children[1].value); //objRow[14].children[6].value;
        var PartTax2Rate = dGetValue(objRow.cells[14].children[2].value); //objRow[14].children[8].value;

        if (isNaN(PartTaxRate) == true) PartTaxRate = 0;
        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

        /////////////////////////////////////////////////////////////
        var PartRateTaxAmt = 1 + (dGetValue(PartTaxRate / 100) + dGetValue(PartTax1Rate / 100) + dGetValue(PartTax2Rate / 100));
        if (isNaN(PartRateTaxAmt) == true) PartRateTaxAmt = 0;

        var PartRateExclTax = dGetValue(PartRate / PartRateTaxAmt);

        if (isNaN(PartRateExclTax) == true) PartRateExclTax = 0;

        //Sujata 01092014_End
        //objRow[12].childNodes[0].value = parseFloat(PartRate).toFixed(2);
        objRow.cells[12].children[0].value = parseFloat(PartRate).toFixed(2);
        var Total = dGetValue(Qty) * PartRate;
        if (isNaN(Total) == true) Total = 0;
        //objRow[13].childNodes[0].value = RoundupValue(Total);
        objRow.cells[13].children[0].value = parseFloat(Total).toFixed(2);

        //objRow[12].childNodes[2].value = parseFloat(PartRateExclTax).toFixed(2);
        objRow.cells[12].children[1].value = parseFloat(PartRateExclTax).toFixed(2);
        var PartTotalExclTax = dGetValue(Qty) * PartRateExclTax;
        if (isNaN(PartTotalExclTax) == true) PartTotalExclTax = 0;
        //objRow[13].childNodes[2].value = RoundupValue(PartTotalExclTax);
        objRow.cells[13].children[1].value = parseFloat(PartRateExclTax).toFixed(2);
        /////////////////////////////////////////////////////////////        
        CalulateInvPartGranTotal()

    }
}


function CalulateInvPartGranTotal() {

    var objGridID = $("#ContentPlaceHolder1_PartGrid")//document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGrid = objGridID[0];
    var objGridGroupTaxID = $("#ContentPlaceHolder1_GrdPartGroup")//document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridGroupTax = objGridGroupTaxID[0];
    var objGridDocTaxID = $("#ContentPlaceHolder1_GrdDocTaxDet")//document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    var objGridDocTax = objGridDocTaxID[0];

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
        //if (objGridGroupTax.rows[k].className.indexOf('RowStyle') > 0) {
            //objGridGroupTax.rows[k].childNodes[3].children[0].value = 0;
            objGridGroupTax.rows[k].cells[3].children[0].value = 0;
        //}
    }

    for (var i = 1; i < CountRow; i++) {
       // if (objGrid.rows[i].className.indexOf('RowStyle') > 0) {
            total = objGrid.rows[i].cells[13].children[0].value; //objGrid.rows[i].childNodes[13].children[1].value;
            sPArtName = objGrid.rows[i].cells[5].children[0].value; //objGrid.rows[i].childNodes[5].children[0].value;
            sGroupCode = objGrid.rows[i].cells[6].children[0].value; //objGrid.rows[i].childNodes[6].children[0].value;

            //Sujata 26052014
            //            bPartSel = objGrid.rows[i].childNodes[14].children[0].children[0].checked;
            //            if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel ==false) {
            //                TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
            //            }
            //            else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
            //                TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
            //            }
            //Sujata 26052014

            bPartSel = objGrid.rows[i].cells[15].children[0].children[0].checked; //objGrid.rows[i].childNodes[15].children[0].children[0].checked;
            sGridPartTax = objGrid.rows[i].cells[14].children[0].selectedIndex; //objGrid.rows[i].childNodes[14].children[0].selectedIndex;

            for (var k = 1; k < CountRowGrTax; k++) {
                //if (objGridGroupTax.rows[k].className.indexOf('RowStyle') > 0) {
                    var sMGroupCode = objGridGroupTax.rows[k].cells[1].children[0].value.trim(); //objGridGroupTax.rows[k].childNodes[1].children[0].value.trim();
                    var sMGrouptax = objGridGroupTax.rows[k].cells[6].children[0].selectedIndex; //objGridGroupTax.rows[k].childNodes[6].children[0].selectedIndex;
                    if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
                    && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                        //objGridGroupTax.rows[k].childNodes[3].children[0].value = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[3].children[0].value);
                        objGridGroupTax.rows[k].cells[3].children[0].value = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].cells[3].children[0].value);
                    }
                //}
            }

            if (sPArtName != "" && sGroupCode.trim() == "01" && bPartSel == false) {
                TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
            }
            else if (sPArtName != "" && sGroupCode.trim() == "02" && bPartSel == false) {
                TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
            }
            if (sPArtName != "" && bPartSel == false) {
                TotalInv = dGetValue(total) + dGetValue(TotalInv)
            }
       // }
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

    for (var i = 0; i < CountGrpRow; i++) {
      //  if (objGridGroupTax.rows[i].className.indexOf('RowStyle') > 0) {
            //group total
            dGrpTotal = dGetValue(objGridGroupTax.rows[i].cells[3].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[3].children[0].value);
            //group Percentage
            dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].cells[4].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
            //group Discount Amount
            dGrpDiscAmt = dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100));
            //Doc Discount Amount
            dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
            //group Discount Amount display
            //objGridGroupTax.rows[i].childNodes[5].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
            objGridGroupTax.rows[i].cells[5].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
            //Amount whiich is applicable for tax
            dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);

            //Main tax calculation
            dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].childNodes[8].children[0].value);
            if (isNaN(dGrpMTaxPer) == true) dGrpMTaxPer = 0;

            dGrpMTaxAmt =parseFloat(dGetValue(dGetValue(dGrpTaxAppAmt) * dGetValue(dGrpMTaxPer / 100))).toFixed(2);
            sGrpMTaxTag = dGetValue(objGridGroupTax.rows[i].cells[7].children[2].value); //objGridGroupTax.rows[i].childNodes[7].children[2].value;
            //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
            //debugger;
            if (sGrpMTaxTag == "I") {
                dDocLSTAmt = parseFloat(dGetValue(dDocLSTAmt) + dGetValue(dGrpMTaxAmt)).toFixed(2);
            }
            else if (sGrpMTaxTag == "O") {
                dDocCSTAmt =parseFloat(dGetValue(dDocCSTAmt) + dGetValue(dGrpMTaxAmt)).toFixed(2);
            }
            //objGridGroupTax.rows[i].childNodes[9].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);
            objGridGroupTax.rows[i].cells[9].children[0].value = parseFloat(dGrpMTaxAmt).toFixed(2);
            dGrpTax1Per = dGetValue(objGridGroupTax.rows[i].cells[12].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[12].children[0].value);
            sGrpTax1ApplOn = dGetValue(objGridGroupTax.rows[i].cells[11].children[2].value); //objGridGroupTax.rows[i].childNodes[11].children[2].value;

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
            //objGridGroupTax.rows[i].childNodes[13].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);
            objGridGroupTax.rows[i].cells[13].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);

            dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].cells[16].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[16].children[0].value);
            sGrpTax2ApplOn = dGetValue(objGridGroupTax.rows[i].cells[15].children[2].value); //objGridGroupTax.rows[i].childNodes[15].children[2].value;

            if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;
            //Sujata 24092014Begin
            //dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
            if (sGrpTax2ApplOn == "1") {
                dGrpTax2Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax2Per / 100));
            } else if (sGrpTax2ApplOn == "3") {
                dGrpTax2Amt = dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
            } else {
                dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
            }
            //Sujata 24092014End            
            dDocTax2Amt = dGetValue(dDocTax2Amt) + dGetValue(dGrpTax2Amt);
            //objGridGroupTax.rows[i].childNodes[17].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);
            objGridGroupTax.rows[i].cells[17].children[0].value = parseFloat(dGrpTax2Amt).toFixed(2);

            dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt));
            dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
            //objGridGroupTax.rows[i].childNodes[18].children[0].value = parseFloat(dGrpTotal).toFixed(2);
            objGridGroupTax.rows[i].cells[18].children[0].value = parseFloat(dGrpTotal).toFixed(2);
        }
    //}

    //objGridDocTax.rows[1].childNodes[2].children[0].value = parseFloat(TotalInv).toFixed(2);
    objGridDocTax.rows[1].cells[2].children[0].value = parseFloat(TotalInv).toFixed(2);
    
    //objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);
    objGridDocTax.rows[1].cells[3].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);

    //objGridDocTax.rows[1].childNodes[5].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);
    objGridDocTax.rows[1].cells[5].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);

    //objGridDocTax.rows[1].childNodes[6].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);
    objGridDocTax.rows[1].cells[6].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);

    //objGridDocTax.rows[1].childNodes[7].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);
    objGridDocTax.rows[1].cells[7].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);

    //objGridDocTax.rows[1].childNodes[8].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);
    objGridDocTax.rows[1].cells[8].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);

    var dDocPFPer = 0;
    var dDocPFAmt = 0;
    var dDocOtherPer = 0;
    var dDocOtherAmt = 0;
    var dDocGrandAmt = 0;

    dDocPFPer = dGetValue(objGridDocTax.rows[1].cells[9].children[0].value); //objGridDocTax.rows[1].childNodes[9].children[0].value;
    if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
    dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    //objGridDocTax.rows[1].childNodes[10].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
    objGridDocTax.rows[1].cells[10].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    dDocOtherPer = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value); //objGridDocTax.rows[1].childNodes[11].children[0].value;
    if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
    dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
    //objGridDocTax.rows[1].childNodes[12].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    objGridDocTax.rows[1].cells[12].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));

    //objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);
    objGridDocTax.rows[1].cells[13].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);

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