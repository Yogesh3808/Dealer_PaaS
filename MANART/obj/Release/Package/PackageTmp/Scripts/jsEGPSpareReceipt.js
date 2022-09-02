// Material Receipt
function CalculateReceiptPartTotal(event, ObjQtyControl) {
    ////debugger;
    //var hdnIsWithPO = document.getElementById("ContentPlaceHolder1_hdnIsWithPO");
    var hdnIsAutoReceipt = document.getElementById("ContentPlaceHolder1_hdnIsAutoReceipt");
    var hdnIsDistributor = document.getElementById("ContentPlaceHolder1_hdnIsDistributor");
    var hdnSupplierType = window.document.getElementById("ContentPlaceHolder1_hdnSupplierType");
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objID = $("#" + ObjQtyControl.id);
        var objRow = objID[0].parentNode.parentNode;

        //var Qty = dGetValue(ObjQtyControl.value);
        var def_BalQty = dGetValue(objRow.cells[4].children[0].defaultValue);
        var def_BillQty = dGetValue(objRow.cells[5].children[0].defaultValue);
        var def_BalQty = dGetValue(def_BalQty + def_BillQty);
        var BalQty = dGetValue(objRow.cells[4].children[0].value);
        var BillQty = dGetValue(objRow.cells[5].children[0].value);
        var RecvQty = dGetValue(objRow.cells[6].children[0].value);
        var DiscPer = dGetValue(objRow.cells[10].children[0].value);
        var PO_No = objRow.cells[15].children[0].value;//11
        var GroupName = objRow.cells[18].children[1].value;//11
        var AcceptRate = 0;

        // For Local MR- Local and Dealer Type
        if (hdnIsDistributor.value == 'N' && hdnIsAutoReceipt.value == 'N') {
            if (dGetValue(BillQty) > dGetValue(def_BalQty) && (PO_No != "")) {
                alert("Bill Quantity cannot greater than Bal PO Quantity");
                objRow.cells[5].children[0].value = "";
                objRow.cells[5].children[0].value = 0;
                objRow.cells[4].children[0].value = parseFloat(dGetValue(def_BalQty)).toFixed(2);
                objRow.cells[5].children[0].focus();
                return false;
            }
        }
        // For Auto Receipt
        if (hdnIsAutoReceipt.value == 'Y') {
            var BalPoQty = 0;
            if (dGetValue(RecvQty) < dGetValue(BillQty) && (PO_No != "")) {
                BalPoQty = BillQty - RecvQty;
                objRow.cells[4].children[0].value = parseFloat(dGetValue(BalPoQty)).toFixed(2);
            }
        }
        // Calculation for only hdnIsAutoReceipt is N
        if (hdnIsAutoReceipt.value == 'N') {
            // Set Bill Qty as Recv Qty
            objRow.cells[6].children[0].value = parseFloat(dGetValue(BillQty)).toFixed(2);
            var Rate = dGetValue(objRow.cells[9].children[0].value);
            // Disc Amt
            var DiscAmt = dGetValue(Rate * dGetValue(DiscPer / 100));
            if (isNaN(DiscAmt) == true) DiscAmt = 0;

            var AcceptRate = dGetValue(Rate) - dGetValue(DiscAmt);
            if (isNaN(AcceptRate) == true) AcceptRate = 0;
            //Set Discounted Rate
            objRow.cells[11].children[0].value = parseFloat(AcceptRate).toFixed(4);

            var Total = 0;
            Total = dGetValue(BillQty) * AcceptRate;

            var PartRate = dGetValue(AcceptRate)
            if (isNaN(PartRate) == true) PartRate = 0;

            //Part tax Amt exclude from MRP to set Part rate and group tax calculation
            var PartTaxRate = objRow.cells[16].children[4].value;//12
            var PartTax1Rate = objRow.cells[16].children[6].value;
            var PartTax2Rate = objRow.cells[16].children[8].value;
            var PartTaxPer = parseFloat(dGetValue(objRow.cells[31].children[0].value), 2);

            if (isNaN(PartTaxPer) == true) PartTaxPer = 0;
            if (isNaN(PartTax1Rate) == true) PartTax1Rate = 0;
            if (isNaN(PartTax2Rate) == true) PartTax2Rate = 0;

            //set Tax Per
            objRow.cells[31].children[0].value = parseFloat(PartTaxPer).toFixed(2);
            //Set Tax1 Per
            objRow.cells[34].children[0].value = parseFloat(PartTax1Rate).toFixed(2);
            //Set Total Amount
            objRow.cells[12].children[0].value = parseFloat(Total).toFixed(2);

            var PartTaxAmt = 0;
            var PartTax1Amt = 0;
            PartTaxAmt = dGetValue(dGetValue(Total) * dGetValue(PartTaxPer / 100));
            PartTax1Amt = dGetValue(dGetValue(Total) * dGetValue(PartTax1Rate / 100));
            if (isNaN(PartTaxAmt) == true) PartTaxAmt = 0.00;
            if (isNaN(PartTax1Amt) == true) PartTax1Amt = 0.00;
            //Set Tax Amount
            objRow.cells[32].children[0].value = parseFloat(PartTaxAmt).toFixed(2);
            //Set Tax1 Amt
            objRow.cells[35].children[0].value = parseFloat(PartTax1Amt).toFixed(2);


            //if (GroupName == '99') {
            //    //Set Total to Rev calculation
            //    objRow.cells[12].children[1].value = parseFloat(Total).toFixed(3);
            //}
            //else {
            //    // Vikram Begin 05052017 new COde for reverse rate
            //    var Price = dGetValue(objRow.cells[8].children[0].value);
            //    var PartMainTaxRevRate = 0;
            //    PartMainTaxRevRate = Price / (1 + (PartTaxRate / 100))
            //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

            //    var RevDiscPer = dGetValue(objRow.cells[10].children[0].value);
            //    var RevDiscAmt = dGetValue(dGetValue(PartMainTaxRevRate) * dGetValue(DiscPer / 100));
            //    if (isNaN(RevDiscAmt) == true) RevDiscAmt = 0;
            //    var PartMainTaxRevRate = dGetValue(PartMainTaxRevRate) - dGetValue(RevDiscAmt);
            //    if (isNaN(PartMainTaxRevRate) == true) PartMainTaxRevRate = 0;

            //    var PartTotalMainTaxAmt = dGetValue(BillQty) * PartMainTaxRevRate;

            //    objRow.cells[12].children[1].value = parseFloat(PartTotalMainTaxAmt).toFixed(3);
            //}
            objRow.cells[12].children[1].value = parseFloat(Total).toFixed(2);


        }

        if (hdnIsDistributor.value == 'N' && hdnIsAutoReceipt.value == 'Y' && hdnSupplierType.value == 'M') {
            var ShortageQty = 0;
            //dGetValue(objRow.cells[16].children[0].value);
            var ExcessQty = 0;
            //dGetValue(objRow.cells[17].children[0].value);
            var DamageQty = dGetValue(objRow.cells[22].children[0].value);//18
            var manfDefectQty = dGetValue(objRow.cells[23].children[0].value);//19
            var WrgSupplyQty = dGetValue(objRow.cells[24].children[0].value);//20
            var Retain_YV = objRow.cells[25].children[0].value;//21
            var SubTotal = 0;

            // Shortage case 1. If RecvQty is less than BillQty            
            if (dGetValue(BillQty) > dGetValue(RecvQty)) {
                ShortageQty = dGetValue(BillQty) - dGetValue(RecvQty);
                objRow.cells[20].children[0].value = parseFloat(dGetValue(ShortageQty)).toFixed(2);//16
                objRow.cells[19].children[0].value = 'Y';//15
                objRow.cells[21].children[0].value = 0.00;//17
                if ((dGetValue(DamageQty) < dGetValue(RecvQty)) || (dGetValue(manfDefectQty) < dGetValue(RecvQty)) || (dGetValue(WrgSupplyQty) < dGetValue(RecvQty))) {
                    SubTotal = dGetValue(DamageQty) + dGetValue(manfDefectQty) + dGetValue(WrgSupplyQty);
                    if (dGetValue(SubTotal) > dGetValue(RecvQty)) {
                        alert("Descripancy quanty cannot greater than Receive quantity");
                        return false;
                    }
                }
                else {
                    if (dGetValue(ShortageQty) != dGetValue(BillQty)) {
                        alert("Descripancy quanty cannot greater than Receive quantity");
                        return false;
                    }
                }
            }

            //Excess case 2. If RecvQty is Greter than BillQty
            if (dGetValue(BillQty) < dGetValue(RecvQty)) {
                ExcessQty = dGetValue(RecvQty) - dGetValue(BillQty);
                objRow.cells[21].children[0].value = parseFloat(dGetValue(ExcessQty)).toFixed(2);//17
                objRow.cells[19].children[0].value = 'Y';//15
                objRow.cells[20].children[0].value = 0.00;//16
                if (dGetValue(ExcessQty) > 0)
                    objRow.cells[25].children[0].disabled = false;//21
                SubTotal = dGetValue(DamageQty) + dGetValue(manfDefectQty) + dGetValue(WrgSupplyQty);
                if (dGetValue(SubTotal) > dGetValue(RecvQty)) {
                    alert("Descripancy quanty cannot greater than Receive quantity");
                    return false;
                }
            }

            // Equal Qty then set Zero Values
            if (dGetValue(BillQty) == dGetValue(RecvQty)) {
                //dGetValue(objRow.cells[16].children[0].value) = 0.00;
                objRow.cells[20].children[0].value = parseFloat(dGetValue(0)).toFixed(2);//16
                objRow.cells[21].children[0].value = parseFloat(dGetValue(0)).toFixed(2);//17
                objRow.cells[19].children[0].value = 'N';//15
                //objRow.cells[21].children[0].value = 'N';
                if (dGetValue(DamageQty) > 0)
                    objRow.cells[19].children[0].value = 'Y';//15
                if (dGetValue(manfDefectQty) > 0)
                    objRow.cells[19].children[0].value = 'Y';//15
                if (dGetValue(WrgSupplyQty) > 0)
                    objRow.cells[19].children[0].value = 'Y';//15
                //dGetValue(objRow.cells[17].children[0].value) = 0.00;
                SubTotal = dGetValue(DamageQty) + dGetValue(manfDefectQty) + dGetValue(WrgSupplyQty);
                if (dGetValue(SubTotal) > dGetValue(RecvQty)) {
                    alert("Descripancy quanty cannot greater than Receive quantity");
                    return false;
                }
            }

            //1. If ExcessQty >0
            if (dGetValue(objRow.cells[21].children[0].value) > 0)//17
                objRow.cells[25].children[0].disabled = false;//21
            else
                objRow.cells[25].children[0].disabled = true;//21
            //
            //debugger;
            var lnkSelectPart1_ID = objRow.cells[27].children[1].id;//23
            var txtWrgParName_No = "";
            var txtWrgParName = "";
            var txtWrgParName_ID = objRow.cells[28].children[0].id;//24
            if (dGetValue(WrgSupplyQty) > 0) {
                objRow.cells[25].children[0].disabled = false;//21
                if (objRow.cells[27].children[0].value != "" || objRow.cells[27].children[0].value != 0) {//23
                    document.getElementById('' + lnkSelectPart1_ID + '').style.display = "";
                    document.getElementById('' + txtWrgParName_ID + '').style.display = "";
                }
                else {
                    document.getElementById('' + lnkSelectPart1_ID + '').style.display = "";
                    document.getElementById('' + txtWrgParName_ID + '').style.display = "";
                    document.getElementById('' + txtWrgParName_ID + '').style.display = "";
                }
            }
            else {
                if (dGetValue(ExcessQty) > 0)
                    objRow.cells[25].children[0].disabled = false;//21
                else
                    objRow.cells[25].children[0].disabled = true;//21
                objRow.cells[27].children[0].value = "";//23
                objRow.cells[28].children[0].value = "";//24
                document.getElementById('' + objRow.cells[27].children[0].id + '').style.display = "none";//23
                document.getElementById('' + lnkSelectPart1_ID + '').style.display = "none";
                document.getElementById('' + txtWrgParName_ID + '').style.display = "none";
            }

        }
        if (hdnIsAutoReceipt.value == "N")
            CalulateReceivePartGranTotal()
    }
}


function CalulateReceivePartGranTotal() {
    debugger;
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var objGridGroupTax = document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridDocTax = document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
    var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");
    var hdnIsAutoReceipt = document.getElementById("ContentPlaceHolder1_hdnIsAutoReceipt");
    var rbtLstDiscount = $("[id*=rbtLstDiscount] input:checked");
    var IS_PerAmt = rbtLstDiscount.val();
    var hdnIsRoundOFF = document.getElementById("ContentPlaceHolder1_hdnIsRoundOFF");

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

    for (var k = 1; k < CountRowGrTax; k++) {
        objGridGroupTax.rows[k].childNodes[4].children[0].value = 0.00;
        //objGridGroupTax.rows[k].childNodes[4].children[1].value = 0.00;
    }

    for (var i = 1; i < CountRow; i++) {
        PartTotal = objGrid.rows[i].childNodes[13].children[0].value;
        total = objGrid.rows[i].childNodes[13].children[1].value;
        if (total == "") total = 0.00;
        sStatus = objGrid.rows[i].childNodes[18].children[0].value;//14
        sGroupCode = objGrid.rows[i].childNodes[19].children[1].value;//15
        if (sGroupCode == "") sGroupCode = "00";

        if (hdnIsAutoReceipt.value == 'Y')
            PartQty = dGetValue(objGrid.rows[i].childNodes[7].children[0].value);
        else
            PartQty = dGetValue(objGrid.rows[i].childNodes[6].children[0].value);

        sGridPartTax = objGrid.rows[i].childNodes[17].children[0].value.trim(); //objGrid.rows[i].childNodes[13].children[0].selectedIndex;//13

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();//objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            if (sMGrouptax != "" && sGroupCode != "" && sStatus != "C" && sStatus != "D"
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                var NetRecAmt = 0;
                NetRecAmt = dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value);
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(NetRecAmt)).toFixed(2);
                //objGridGroupTax.rows[k].childNodes[4].children[1].value = parseFloat(dGetValue(PartTotal) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[1].value),2);
            }
        }

        if (sStatus != "C" && sStatus != "D" && (sGroupCode == "01") || (sGroupCode == "99")) {
            TotalSparesRate = dGetValue(total) + dGetValue(TotalSparesRate)
        }
        else if (sStatus != "C" && sStatus != "D" && sGroupCode == "02") {
            TotalOilRate = dGetValue(total) + dGetValue(TotalOilRate)
        }
        if (sGroupCode != "" && sGroupCode != "00" && sStatus != "C" && sStatus != "D") {
            TotalReceiptAmt = dGetValue(total) + dGetValue(TotalReceiptAmt)
            TotalAmt = dGetValue(PartTotal) + dGetValue(TotalAmt)
            totalQtypart = dGetValue(totalQtypart) + dGetValue(PartQty)
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

    for (var i = 1; i < CountGrpRow; i++) {
        ////debugger;
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
            //Vikram Begin_05052017
            //dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) / (1 + dGetValue(dGrpTax1Per / 100))) * dGetValue(dGrpTax1Per / 100));
            //Vikram END_05052017
            dGrpTax1Amt = parseFloat(dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax1Per / 100))).toFixed(2);
        } else if (sGrpTax1ApplOn == "3") {
            dGrpTax1Amt = parseFloat(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100))).toFixed(2);
        } else {
            dGrpTax1Amt = parseFloat(dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100))).toFixed(2);
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
        //Vikram Begin_05052017
        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt))
        //dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax2Amt))
        //END
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(3);

    }

    objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalReceiptAmt).toFixed(2);
    //Vikram_Begin 02052017
    //objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalAmt).toFixed(2);
    //END
    objGridDocTax.rows[1].childNodes[4].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);

    objGridDocTax.rows[1].childNodes[8].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);//6
    objGridDocTax.rows[1].childNodes[9].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);//7

    objGridDocTax.rows[1].childNodes[10].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);//8
    objGridDocTax.rows[1].childNodes[11].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);//9

    var dDocPFPer = 0;
    var dDocPFAmt = 0;
    var dDocOtherPer = 0;
    var dDocOtherAmt = 0;
    var dDocGrandAmt = 0;

    if (IS_PerAmt == "Per") {
        dDocPFPer = objGridDocTax.rows[1].childNodes[12].children[0].value;//10
        if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
        dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    }
    else {
        dDocPFPer = 0.00;
        dDocPFAmt = dGetValue(objGridDocTax.rows[1].childNodes[13].children[0].value);
    }
    //Set pf Percent
    objGridDocTax.rows[1].childNodes[12].children[0].value = parseFloat(dDocPFPer).toFixed(2);

    objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocPFAmt).toFixed(2);//11
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    if (IS_PerAmt == "Per") {
        dDocOtherPer = objGridDocTax.rows[1].childNodes[14].children[0].value;//12
        if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
        dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
    }
    else {
        dDocOtherPer = 0.00;
        dDocOtherAmt = dGetValue(objGridDocTax.rows[1].childNodes[15].children[0].value);
    }
    // Set Other per
    objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocOtherPer).toFixed(2);

    objGridDocTax.rows[1].childNodes[15].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);//13
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));

    objGridDocTax.rows[1].childNodes[16].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed((hdnIsRoundOFF.value == "Y" && hdnIsAutoReceipt.value == "N") ? 0 : 2);//14
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

// Select Part  from Part Master
function ShowPartMaster(objNewPartLabel, sDealerId, sSuperceded) {
    //debugger;
    var sSelectedPartID = "";
    var objRow = objNewPartLabel.parentNode.parentNode.childNodes; //Selected Row
    sSelectedPartID = objRow[2].childNodes[1].value; //Part ID
    var PartDetailsValue;
    //var sSelectedPartID = GetPreviousSelectedPartIDInJobs(objNewPartLabel);

    PartDetailsValue = window.showModalDialog("../Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&Superceded=" + sSuperceded, "List", "dialogWidth=700px,dialogHeight=550px");
    if (PartDetailsValue != null) {
        SetPartDetails(objNewPartLabel, PartDetailsValue);
    }
}
//SetPartDetails
function SetPartDetails(objAddNewControl, PartValue) {
    ////debugger;
    //objLabel.parentNode.childNodes[1].value=Value[1];
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    objAddNewControl.style.display = "none";

    //Set PartId;
    //objRow[3].children[0].value = PartValue[0];
    objRow[27].children[0].value = PartValue[1];//23


    //SetPartNo
    objRow[28].children[0].value = PartValue[2];//24
    objRow[28].children[0].style.display = "";
    objRow[28].children[0].readOnly = true;

    //SetPartName
    objRow[29].childNodes[1].value = PartValue[3];//27
    objRow[29].childNodes[1].style.display = "";
    objRow[29].childNodes[1].readOnly = true;
}
