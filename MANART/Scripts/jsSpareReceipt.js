function CalculateReceiptPartTotal(event, ObjQtyControl) {
    alert('hi');
    //var hdnIsWithPO = document.getElementById("ContentPlaceHolder1_hdnIsWithPO");
    var hdnIsAutoReceipt = document.getElementById("ContentPlaceHolder1_hdnIsAutoReceipt");
    var hdnIsDistributor = document.getElementById("ContentPlaceHolder1_hdnIsDistributor");
   
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var ObjID = $("#" + ObjQtyControl.id);
        var objRow = ObjID[0].parentNode.parentNode; //ObjQtyControl.parentNode.parentNode.childNodes;

        //var Qty = dGetValue(ObjQtyControl.value);
        var BalQty = dGetValue(objRow.cells[4].children[0].value);//dGetValue(objRow[4].childNodes[0].value);
        var BillQty = dGetValue(objRow.cells[5].children[0].value); //dGetValue(objRow[5].childNodes[0].value);
        var RecvQty = dGetValue(objRow.cells[6].children[0].value); //dGetValue(objRow[6].childNodes[0].value);
        var DiscPer = dGetValue(objRow.cells[7].children[0].value); //dGetValue(objRow[8].childNodes[0].value);
        var AcceptRate = 0;
        //if (hdnIsDistributor.value=='Y' && hdnIsWithPO.value == 'Y' && hdnIsAutoReceipt.value == 'Y') {
        if (hdnIsDistributor.value == 'Y' && hdnIsAutoReceipt.value == 'Y') {
            AcceptRate = dGetValue(objRow.cells[9].children[0].value); //dGetValue(objRow[9].childNodes[0].value);
            if (dGetValue(BillQty) - dGetValue(RecvQty) < 0) {
                alert("Receive quanty cannot greater than Bill quantity");
                objRow.cells[6].children[0].focus(); //objRow[6].childNodes[0].focus();
                return false;
            }
            else
                //objRow[4].childNodes[0].value = parseFloat(dGetValue(BillQty) - dGetValue(RecvQty)).toFixed(2);
                objRow[4].childNodes[0].value = parseFloat(dGetValue(BillQty) - dGetValue(RecvQty)).toFixed(2);
        }
        //if ((hdnIsDistributor.value == 'Y' || hdnIsDistributor.value == 'N') && hdnIsWithPO.value == 'N' && hdnIsAutoReceipt.value == 'N') {
        if ((hdnIsDistributor.value == 'N' && hdnIsAutoReceipt.value == 'Y') || hdnIsAutoReceipt.value == 'N') {
            AcceptRate = dGetValue(objRow.cells[7].children[0].value); //dGetValue(objRow[7].childNodes[0].value);
            //objRow[9].childNodes[0].value = dGetValue(AcceptRate);
            objRow.cells[9].children[0].value = dGetValue(AcceptRate);
        }
        var Total = dGetValue(RecvQty) * AcceptRate;

        var PartRate = dGetValue(AcceptRate)//dGetValue(FOBRate) - dGetValue(DiscAmt);
        if (isNaN(PartRate) == true) PartRate = 0;
        
        //Sujata 01092014_Begin 'Part tax Amt exclude from MRP to set Part rate and group tax calculation
        var PartTaxRate = dGetValue(objRow.cells[12].children[0].value); //objRow[12].children[4].value;
        var PartTax1Rate = dGetValue(objRow.cells[12].children[1].value); //objRow[12].children[6].value;
        var PartTax2Rate = dGetValue(objRow.cells[12].children[2].value); //objRow[12].children[8].value;

        if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
        if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

        var PartRateTaxAmt = 1 + (dGetValue(PartTaxRate / 100) + dGetValue(PartTax1Rate / 100) + dGetValue(PartTax2Rate / 100));
        //var PartRateTaxAmt = dGetValue(PartTaxRate) + dGetValue(PartTax1Rate) + dGetValue(PartTax2Rate);
        if (isNaN(PartRateTaxAmt) == true) PartRateTaxAmt = 0;

        var PartRateExclTax =dGetValue(PartRate / PartRateTaxAmt);
        //var PartRateExclTax = parseFloat(PartRate).toFixed(2) - parseFloat(dGetValue(PartRate * PartRateTaxAmt / 100)).toFixed(2);

        if (isNaN(PartRateExclTax) == true) PartRateExclTax = 0;

        //objRow[10].childNodes[2].value = parseFloat(PartRateExclTax).toFixed(2);
        objRow.cells[10].children[1].value = parseFloat(PartRateExclTax).toFixed(2);
        var PartTotalExclTax = dGetValue(RecvQty) * PartRateExclTax;
        if (isNaN(PartTotalExclTax) == true) PartTotalExclTax = 0;
        //objRow[10].childNodes[2].value = RoundupValue(PartTotalExclTax);
        objRow.cells[10].children[1].value = parseFloat(PartTotalExclTax).toFixed(2);     
        //Sujata 01092014_End

        //SetNewLabel Display
        //objRow[10].childNodes[0].value = RoundupValue(Total);
        objRow.cells[10].children[0].value = RoundupValue(Total);
        CalulateReceivePartGranTotal()

    }
}


function CalulateReceivePartGranTotal() {

    var objGridID = $("#ContentPlaceHolder1_PartGrid")//document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGrid = objGridID[0];
    var objGridGroupTaxID = $("#ContentPlaceHolder1_GrdPartGroup")//document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridGroupTax = objGridGroupTaxID[0];
    var objGridDocTaxID = $("#ContentPlaceHolder1_GrdDocTaxDet")//document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    var objGridDocTax = objGridDocTaxID[0];
    
    var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
    var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");

    var total = 0;
    var PartTotal = 0;
    var TotalAmt = 0;
    var TotalSparesRate = 0;
    var TotalOilRate = 0;
    var TotalReceiptAmt = 0;

    var totalQtypart = 0;
    var PartQty = 0;
    var sPArtName = "";
    var sGroupCode = "";
    var bPartSel;
    var sStatus = "";
    var CountRow = objGrid.rows.length;
    var CountRowGrTax = objGridGroupTax.rows.length;

    for (var k = 0; k < CountRowGrTax; k++) {
        if (objGridGroupTax.rows[k].className.indexOf('RowStyle') > 0) {
            //objGridGroupTax.rows[k].childNodes[3].children[0].value = 0;
            objGridGroupTax.rows[k].cells[3].children[0].value = 0;
        }
    }

    for (var i = 0; i < CountRow; i++) {
        if (objGrid.rows[i].className.indexOf('RowStyle') > 0) {
            //Sujata 08102014
            PartTotal = objGrid.rows[i].cells[10].children[0].value; //objGrid.rows[i].childNodes[10].children[0].value;
            total = objGrid.rows[i].cells[10].children[1].value; //objGrid.rows[i].childNodes[10].children[1].value;
            sGroupCode = objGrid.rows[i].cells[15].children[1].value; //objGrid.rows[i].childNodes[15].children[1].value;
            sStatus = objGrid.rows[i].cells[14].children[0].value; //objGrid.rows[i].childNodes[14].children[0].value;
            PartQty = objGrid.rows[i].cells[6].children[0].value; //dGetValue(objGrid.rows[i].childNodes[6].children[0].value);
            sGridPartTax = objGrid.rows[i].cells[12].children[0].selectedIndex; //objGrid.rows[i].childNodes[12].children[0].selectedIndex;
            
            for (var k = 0; k < CountRowGrTax; k++) {
            
                if (objGridGroupTax.rows[k].className.indexOf('RowStyle') > 0) {
                    var sMGroupCode = objGridGroupTax.rows[k].cells[1].children[0].value.trim(); //objGridGroupTax.rows[k].childNodes[1].children[0].value.trim();
                    var sMGrouptax = objGridGroupTax.rows[k].cells[6].children[0].selectedIndex; //objGridGroupTax.rows[k].childNodes[6].children[0].selectedIndex;
                    if (sMGrouptax != "" && sGroupCode != "" && sStatus != "C" && sStatus != "D"
                    && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                        //objGridGroupTax.rows[k].childNodes[3].children[0].value = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[3].children[0].value);
                        objGridGroupTax.rows[k].cells[3].children[0].value = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].cells[3].children[0].value);
                    }
                }
            }
            
            if (sStatus != "C" && sStatus != "D" && (sGroupCode.trim() == "01") || (sGroupCode.trim() == "99")) {
                TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
            }
            else if (sStatus != "C" && sStatus != "D" && sGroupCode.trim() == "02") {
                TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
            }
            if (sGroupCode != "" && sStatus != "C" && sStatus != "D") {
                TotalReceiptAmt = dGetValue(total) + dGetValue(TotalReceiptAmt)
                TotalAmt = dGetValue(PartTotal) + dGetValue(TotalAmt)
                totalQtypart = dGetValue(totalQtypart) + dGetValue(PartQty)
            }
        }
    }
    txtTotalQty.value = totalQtypart;
    txtTotal.value = parseFloat(dGetValue(TotalAmt)).toFixed(2);
    
//    if (objGridGroupTax.rows[1] != undefined)
//    objGridGroupTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalSparesRate).toFixed(2);
//    if (objGridGroupTax.rows[2]!=undefined)
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

    for (var i = 0; i < CountGrpRow; i++) {
        if (objGridGroupTax.rows[i].className.indexOf('RowStyle') > 0) {
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
            dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].cells[8].children[0].value); //dGetValue(objGridGroupTax.rows[i].childNodes[8].children[0].value);
            dGrpMTaxAmt = dGetValue(dGetValue(dGrpTaxAppAmt) * dGetValue(dGrpMTaxPer / 100));
            sGrpMTaxTag = objGridGroupTax.rows[i].cells[7].children[2].value; //objGridGroupTax.rows[i].childNodes[7].children[2].value;
            //depend on tax tag 'L' and 'C' then LST/CST calculation for Doc
            if (sGrpMTaxTag == "I") {
                dDocLSTAmt = dGetValue(dDocLSTAmt) + dGetValue(dGrpMTaxAmt);
            }
            else if (sGrpMTaxTag == "O") {
                dDocCSTAmt = dGetValue(dDocCSTAmt) + dGetValue(dGrpMTaxAmt);
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
            //Sujata 24092014Begin
            //dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
            sGrpTax2ApplOn = dGetValue(objGridGroupTax.rows[i].cells[14].children[2].value); //objGridGroupTax.rows[i].childNodes[14].children[2].value;

            if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;
            
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
    }

    //objGridDocTax.rows[1].childNodes[2].children[0].value = parseFloat(TotalReceiptAmt).toFixed(2);
    objGridDocTax.rows[1].cells[2].children[0].value = parseFloat(TotalReceiptAmt).toFixed(2);

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

    CalulateReceivePartGranTotal();
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

    CalulateReceivePartGranTotal();
    return true;
}
//Receipt Related Calculation End
