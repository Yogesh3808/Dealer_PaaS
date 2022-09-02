function CalculateLineTotal(event, ObjQtyControl, ActFor) {
   // debugger;
    var VECVSharePer;
    var VAmount;
    var DealerSharePer;
    var DAmount;
    var TentAmt;
    var ActAmt;
    var RowCount;
    var VECVapprovedPer
    var ApproveAmt;
    var DealerapprovedPer
    var DealerApproveAmt;

    var ObjID = $("#" + ObjQtyControl.id);
    var objRow = ObjID[0].parentNode.parentNode; //ObjQtyControl.parentNode.parentNode.childNodes;
   
    RowCount = ObjQtyControl.parentNode.parentNode.parentNode.children.length
    //TentAmt = dGetValue(objRow.cells[3].children[1].value); //dGetValue(objRow[2].childNodes[0].value);
    TentAmt = dGetValue(objRow.cells[3].childNodes[1].value);
    if (ActFor == 'Request') {

        VECVSharePer = dGetValue(objRow.cells[4].children[1].value); //dGetValue(objRow[3].childNodes[0].value);
        if (VECVSharePer > 100 || VECVSharePer < 0) {
            alert("VECV Share % Must be Less Than 100% AND Negative Value Not Allowd");
            //objRow[3].childNodes[0].value = "";
            objRow.cells[4].children[1].value=""
            //objRow[3].childNodes[0].focus();
            objRow.cells[4].children[1].focus();
            return true;
        }

        Amount = TentAmt * (VECVSharePer / 100);
        if (ActFor != 'ActProcess')
            //objRow[4].childNodes[0].value = RoundupValue(Amount);
            objRow.cells[5].children[1].value = RoundupValue(Amount);

        DealerSharePer = 100 - VECVSharePer;
        if (ActFor != 'ActProcess')
            //objRow[5].childNodes[0].value = DealerSharePer;
            objRow.cells[6].children[1].value = DealerSharePer;

        DAmount = TentAmt * (DealerSharePer / 100);
        if (ActFor != 'ActProcess')
            objRow[7].childNodes[1].value = RoundupValue(DAmount);


        CalulateTotal(RowCount, 2, ObjQtyControl);
        CalulateTotal(RowCount, 4, ObjQtyControl);
        CalulateTotal(RowCount, 6, ObjQtyControl);

        //objRow[7].childNodes[0].value = VECVSharePer;
        objRow.cells[8].children[1].value = VECVSharePer;

        //objRow[8].childNodes[0].value = RoundupValue(Amount);
        objRow.cells[9].children[1].value = RoundupValue(Amount);

        //objRow[9].childNodes[0].value = DealerSharePer;
        objRow.cells[10].children[1].value = DealerSharePer;

        //objRow[10].childNodes[0].value = RoundupValue(DAmount);
        objRow.cells[11].children[1].value = RoundupValue(DAmount);
        
        CalulateTotal(RowCount, 9, ObjQtyControl);
        CalulateTotal(RowCount, 11, ObjQtyControl);
    }
    else if (ActFor == 'ActRequest') {
        VECVapprovedPer = dGetValue(objRow.cells[8].children[1].value); //dGetValue(objRow[7].childNodes[0].value);
        if (VECVapprovedPer > 100 || VECVapprovedPer < 0) {
            alert("VECV Approve Share % Must be Less Than 100% AND Negative Value Not Allowd");
            //objRow[7].childNodes[0].value = "";
            objRow.cells[8].children[1].value = ""
            //objRow[7].childNodes[0].focus();
            objRow.cells[8].children[1].focus();
            return true;
        }
        //objRow[7].childNodes[0].value = VECVapprovedPer;
        objRow.cells[8].children[1].value = VECVapprovedPer;
        if (VECVapprovedPer > 0)
            //objRow[11].childNodes[0].innerHTML = "Yes"
            objRow.cells[8].children[1].innerHTML = "Yes"
        else
            //objRow[11].childNodes[0].innerHTML = "No"
            objRow.cells[8].children[1].innerHTML = "No"

        ApproveAmt = TentAmt * (VECVapprovedPer / 100);
        //objRow[8].childNodes[0].value = RoundupValue(ApproveAmt);
        objRow.cells[9].children[1].value = RoundupValue(ApproveAmt);
        
        DealerapprovedPer = 100 - VECVapprovedPer;
        DealerApproveAmt = TentAmt * (DealerapprovedPer / 100);
        //objRow[9].childNodes[0].value = 100 - VECVapprovedPer;
        objRow.cells[10].children[1].value = 100 - VECVapprovedPer;

        //objRow[10].childNodes[0].value = RoundupValue(DealerApproveAmt);
        objRow.cells[11].children[1].value = RoundupValue(DealerApproveAmt);
        
        CalulateTotal(RowCount, 9, ObjQtyControl);
        CalulateTotal(RowCount, 11, ObjQtyControl);
    }
    else if (ActFor == 'ActProcess') {
        debugger;
        TentAmt = dGetValue(objRow.cells[2].children[0].value);
        ActAmt = dGetValue(objRow.cells[3].children[0].value); //dGetValue(objRow[3].childNodes[0].value);
        VECVapprovedPer = dGetValue(objRow.cells[8].children[0].value); //dGetValue(objRow[8].childNodes[0].value);
        if (VECVapprovedPer > 100 || VECVapprovedPer < 0) {
            alert("VECV Approve Share % Must be Less Than 100% AND Negative Value Not Allowd");
            //objRow[8].childNodes[0].value = "";
            objRow.cells[8].children[0].value = ""
            //objRow[8].childNodes[0].focus();
            objRow.cells[8].children[0].focus();
            return true;
        }
        //objRow[8].childNodes[0].value = VECVapprovedPer 
        objRow.cells[8].children[0].value = VECVapprovedPer         
        //if (VECVapprovedPer > 0)
        //    //objRow[12].childNodes[0].innerHTML = "Yes"
        //    objRow.cells[12].children[0].innerHTML = "Yes"
        //else
        //    //objRow[12].childNodes[0].innerHTML = "No"
        //    objRow.cells[12].children[0].innerHTML = "No"

        //if (ActAmt > TentAmt && ActAmt != 0)
        //    ApproveAmt = TentAmt * (VECVapprovedPer / 100);
        //else if (ActAmt < TentAmt && ActAmt != 0)
        //    ApproveAmt = ActAmt * (VECVapprovedPer / 100);
        //else
        //    ApproveAmt = TentAmt * (VECVapprovedPer / 100);            
     

        ApproveAmt = ActAmt * (VECVapprovedPer / 100);

        //objRow[9].childNodes[0].value = RoundupValue(ApproveAmt);
        objRow.cells[9].children[0].value = RoundupValue(ApproveAmt);
        
        DealerapprovedPer = 100 - VECVapprovedPer;

        DealerApproveAmt = ActAmt * (DealerapprovedPer / 100);

        //if (ActAmt > TentAmt && ActAmt != 0)
        //    DealerApproveAmt = (TentAmt * (DealerapprovedPer / 100)) + (ActAmt - TentAmt);
        //else if (ActAmt < TentAmt && ActAmt != 0)
        //    DealerApproveAmt = ActAmt * (DealerapprovedPer / 100);
        //else
        //    DealerApproveAmt = TentAmt * (DealerapprovedPer / 100);       
        
        //objRow[10].childNodes[0].value = 100 - VECVapprovedPer;
        objRow.cells[10].children[0].value = 100 - VECVapprovedPer;
        
        //objRow[11].childNodes[0].value = RoundupValue(DealerApproveAmt);
        objRow.cells[11].children[0].value = RoundupValue(DealerApproveAmt);
        
        CalulateTotal(RowCount, 9, ObjQtyControl);
        CalulateTotal(RowCount, 11, ObjQtyControl);
    }
   
}

function CalulateTotal(RCount, CellNo, ObjQtyControl) {
    var TotalTentAmount = 0;
    var RowValue = 0;
    for (var i = 1; i < RCount - 1; i++) {
        RowValue = dGetValue(ObjQtyControl.parentNode.parentNode.parentNode.children[i].cells[CellNo].childNodes[1].value);
        if (RowValue > 0) {
            TotalTentAmount = dGetValue(TotalTentAmount) + RowValue;
        }
    }
    ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[1].value = RoundupValue(TotalTentAmount);

    if (CellNo == 9) {

        var TotalAmount_GST = 0;
        var TotalAmount_GSTWithDeduction = 0;
        var objIGST_SGST_Per = document.getElementById("txtIGST_SGST_Per");
        var objCGST_Per = document.getElementById("txtCGST_Per");
        var objApprVeCVShareTotAmt_GST = document.getElementById("txtApprVeCVShareTotAmt_GST");
        var objDeductionAmount_GST = document.getElementById("txtDeductionAmount_GST");
        var objApprVeCVShareTotAmt_GSTWithDeduction = document.getElementById("txtApprVeCVShareTotAmt_GSTWithDeduction");
        var objIGST_SGST_GST = document.getElementById("txtIGST_SGST_GST");
        var objCGST_GST = document.getElementById("txtCGST_GST");
        var objApprVeCVShareFinalTotAmt_GST = document.getElementById("txtApprVeCVShareFinalTotAmt_GST");

        objApprVeCVShareTotAmt_GST.value = RoundupValue(TotalTentAmount);
        TotalAmount_GSTWithDeduction = RoundupValue(dGetValue(objApprVeCVShareTotAmt_GST.value) - dGetValue(objDeductionAmount_GST.value));
        objApprVeCVShareTotAmt_GSTWithDeduction.value = RoundupValue(TotalAmount_GSTWithDeduction);

        objIGST_SGST_GST.value = RoundupValue(dGetValue(TotalAmount_GSTWithDeduction * objIGST_SGST_Per.value) / 100);
        objCGST_GST.value = RoundupValue(dGetValue(TotalAmount_GSTWithDeduction * objCGST_Per.value) / 100);
        TotalAmount_GST = RoundupValue(dGetValue(TotalAmount_GSTWithDeduction) + dGetValue(TotalAmount_GSTWithDeduction * objIGST_SGST_Per.value) / 100 + dGetValue(TotalAmount_GSTWithDeduction * objCGST_Per.value) / 100);
        objApprVeCVShareFinalTotAmt_GST.value = RoundupValue(TotalAmount_GST);

        //        ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[2].value = RoundupValue(dGetValue(TotalTentAmount * objIGST_SGST_Per.value)/100);
        //        ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[4].value = RoundupValue(dGetValue(TotalTentAmount * objCGST_Per.value) / 100);
        //        TotalAmount_GST = RoundupValue(dGetValue(TotalTentAmount) + dGetValue(TotalTentAmount * objIGST_SGST_Per.value) / 100 + dGetValue(TotalTentAmount * objCGST_Per.value) / 100);

        //        ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[6].value = RoundupValue(TotalAmount_GST);



    }
}






function CalculateClaimLineTotal(event, ObjQtyControl, ActFor) {
    //debugger;
    var VECVSharePer;
    var VAmount;
    var DealerSharePer;
    var DAmount;
    var TentAmt;
    var ActAmt;
    var RowCount;
    var VECVapprovedPer
    var ApproveAmt;
    var DealerapprovedPer
    var DealerApproveAmt;
       
    var ObjID = $("#" + ObjQtyControl.id);
    var objRow = ObjID[0].parentNode.parentNode; //ObjQtyControl.parentNode.parentNode.childNodes;
    
    RowCount = ObjQtyControl.parentNode.parentNode.parentNode.children.length
    TentAmt = dGetValue(objRow.cells[2].children[0].value); //dGetValue(objRow[3].childNodes[0].value);
    ActAmt = dGetValue(objRow.cells[3].children[0].value); //dGetValue(objRow[4].childNodes[0].value);

    VECVSharePer = dGetValue(objRow.cells[4].children[0].value); //dGetValue(objRow[5].childNodes[0].value);
    if (VECVSharePer > 100 || VECVSharePer < 0) {
        alert("VECV Share % Must be Less Than 100% AND Negative Value Not Allowd");
        //objRow[5].childNodes[0].value = "";
        objRow.cells[4].children[0].value="";
        //objRow[5].childNodes[0].focus()
        objRow.cells[4].children[0].focus();
        return true;
    }

    if (ActAmt > TentAmt && ActAmt != 0)
        Amount = TentAmt * (VECVSharePer / 100);
    else if (ActAmt < TentAmt && ActAmt != 0)
        Amount = ActAmt * (VECVSharePer / 100);
    else
        Amount = TentAmt * (VECVSharePer / 100);

    //objRow[6].childNodes[0].value = RoundupValue(Amount);
    objRow.cells[5].children[0].value = RoundupValue(Amount);
    //objRow[9].childNodes[0].value = VECVSharePer;
    objRow.cells[8].children[0].value = VECVSharePer;
    //objRow[10].childNodes[0].value = RoundupValue(Amount);
    objRow.cells[9].children[0].value = RoundupValue(Amount);

    DealerSharePer = 100 - VECVSharePer;

    //objRow[7].childNodes[0].value = DealerSharePer;
    objRow.cells[6].children[0].value = DealerSharePer;

    if (ActAmt > TentAmt && ActAmt != 0)
        DAmount = (TentAmt * (DealerSharePer / 100)) + (ActAmt - TentAmt);
    else if (ActAmt < TentAmt && ActAmt != 0)
        DAmount = ActAmt * (DealerSharePer / 100);
    else
        DAmount = TentAmt * (DealerSharePer / 100);

    //objRow[8].childNodes[0].value = RoundupValue(DAmount);
    objRow.cells[7].children[0].value = RoundupValue(DAmount);
    //objRow[11].childNodes[0].value = DealerSharePer;
    objRow.cells[10].children[0].value = DealerSharePer;
    //objRow[12].childNodes[0].value = RoundupValue(DAmount);
    objRow.cells[11].children[0].value = RoundupValue(DAmount);



    CalulateClaimTotal(RowCount, 2, ObjQtyControl);
    CalulateClaimTotal(RowCount,3, ObjQtyControl);
    CalulateClaimTotal(RowCount, 5, ObjQtyControl);
    CalulateClaimTotal(RowCount, 7, ObjQtyControl);
    CalulateClaimTotal(RowCount, 9, ObjQtyControl);
    CalulateClaimTotal(RowCount, 11, ObjQtyControl);
}

function CalulateClaimTotal(RCount, CellNo, ObjQtyControl) {
    var TotalTentAmount = 0;
    var RowValue = 0;
    for (var i = 1; i < RCount - 1; i++) {
        RowValue = dGetValue(ObjQtyControl.parentNode.parentNode.parentNode.children[i].cells[CellNo].childNodes[1].value);
        if (RowValue > 0) {
            TotalTentAmount = dGetValue(TotalTentAmount) + RowValue;
        }
    }
    ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[1].value = RoundupValue(TotalTentAmount);



    if (CellNo == 9) {

        var TotalAmount_GST = 0;
        var TotalAmount_GSTWithDeduction = 0;
        var objIGST_SGST_Per = document.getElementById("ContentPlaceHolder1_txtIGST_SGST_Per");
        var objCGST_Per = document.getElementById("ContentPlaceHolder1_txtCGST_Per");
        var objApprVeCVShareTotAmt_GST = document.getElementById("ContentPlaceHolder1_txtApprVeCVShareTotAmt_GST");

        var objDeductionAmount_GST = document.getElementById("ContentPlaceHolder1_txtDeductionAmount_GST");
       // var objDeductionAmount_GST = 0;
        var objApprVeCVShareTotAmt_GSTWithDeduction = document.getElementById("ContentPlaceHolder1_txtApprVeCVShareTotAmt_GSTWithDeduction");
        var objIGST_SGST_GST = document.getElementById("ContentPlaceHolder1_txtIGST_SGST_GST");
        var objCGST_GST = document.getElementById("ContentPlaceHolder1_txtCGST_GST");
        var objApprVeCVShareFinalTotAmt_GST = document.getElementById("ContentPlaceHolder1_txtApprVeCVShareFinalTotAmt_GST");

        objApprVeCVShareTotAmt_GST.value = RoundupValue(TotalTentAmount);
        TotalAmount_GSTWithDeduction = RoundupValue(dGetValue(objApprVeCVShareTotAmt_GST.value) - dGetValue(objDeductionAmount_GST.value));
        objApprVeCVShareTotAmt_GSTWithDeduction.value = RoundupValue(TotalAmount_GSTWithDeduction);

        objIGST_SGST_GST.value = RoundupValue(dGetValue(TotalAmount_GSTWithDeduction * objIGST_SGST_Per.value) / 100);
        objCGST_GST.value = RoundupValue(dGetValue(TotalAmount_GSTWithDeduction * objCGST_Per.value) / 100);
        TotalAmount_GST = RoundupValue(dGetValue(TotalAmount_GSTWithDeduction) + dGetValue(TotalAmount_GSTWithDeduction * objIGST_SGST_Per.value) / 100 + dGetValue(TotalAmount_GSTWithDeduction * objCGST_Per.value) / 100);
        objApprVeCVShareFinalTotAmt_GST.value = RoundupValue(TotalAmount_GST);

        //        ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[2].value = RoundupValue(dGetValue(TotalTentAmount * objIGST_SGST_Per.value)/100);
        //        ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[4].value = RoundupValue(dGetValue(TotalTentAmount * objCGST_Per.value) / 100);
        //        TotalAmount_GST = RoundupValue(dGetValue(TotalTentAmount) + dGetValue(TotalTentAmount * objIGST_SGST_Per.value) / 100 + dGetValue(TotalTentAmount * objCGST_Per.value) / 100);

        //        ObjQtyControl.parentNode.parentNode.parentNode.children[RCount - 1].cells[CellNo].childNodes[6].value = RoundupValue(TotalAmount_GST);



    }

}





function CalculateClaimActualTotal(event, ObjQtyControl, ActFor) {

     debugger;
    var VECVSharePer;
    var VAmount;
    var DealerSharePer;
    var DAmount;
    var TentAmt;
    var ActAmt;
    var RowCount;
    var VECVapprovedPer
    var ApproveAmt;
    var DealerapprovedPer
    var DealerApproveAmt;
    var MTIShare;


    var ObjID = $("#" + ObjQtyControl.id);
    var objRow = ObjID[0].parentNode.parentNode; //ObjQtyControl.parentNode.parentNode.childNodes;   
    RowCount = ObjQtyControl.parentNode.parentNode.parentNode.children.length
    TentAmt = dGetValue(objRow.cells[2].children[0].value); //dGetValue(objRow[3].childNodes[0].value);
    ActAmt = dGetValue(objRow.cells[3].children[0].value); //dGetValue(objRow[4].childNodes[0].value);

    MTIShare = document.getElementById("ContentPlaceHolder1_txtMTIShare");
   // VECVSharePer = MTIShare;
   // VECVSharePer = dGetValue(objRow.cells[4].children[0].value); //dGetValue(objRow[5].childNodes[0].value);
    // VECVapprovedPer = dGetValue(objRow.cells[8].children[0].value); //dGetValue(objRow[9].childNodes[0].value);
    objRow.cells[4].children[0].value = MTIShare.value;

    if (ActFor == 'Request') {

       // objRow.cells[4].children[0].value = MTIShare.value;
        objRow.cells[8].children[0].value = MTIShare.value;
    }

     VECVSharePer = dGetValue(objRow.cells[4].children[0].value); //dGetValue(objRow[5].childNodes[0].value);
     VECVapprovedPer = dGetValue(objRow.cells[8].children[0].value); //dGetValue(objRow[9].childNodes[0].valu

     //if (ActAmt == 0) {
     //    //objRow[4].childNodes[0].value = TentAmt
     //    objRow.cells[3].children[0].value = TentAmt
     //    ActAmt = dGetValue(objRow.cells[3].children[0].value);
     //}

    //if (ActAmt > TentAmt && ActAmt != 0)
    //{
    //    Amount = TentAmt * (VECVSharePer / 100);
    //    ApproveAmt = TentAmt * (VECVapprovedPer / 100);
    //}
    //else if (ActAmt < TentAmt && ActAmt != 0) {
    //    Amount = ActAmt * (VECVSharePer / 100);
    //    ApproveAmt = ActAmt * (VECVapprovedPer / 100);
    //}
    //else {
    //    Amount = TentAmt * (VECVSharePer / 100);
    //    ApproveAmt = TentAmt * (VECVapprovedPer / 100);
    //}
     if (ActFor == 'Request')
     {
      

         Amount = TentAmt * (VECVSharePer / 100);
         ApproveAmt = TentAmt * (VECVapprovedPer / 100);
     }
    else
     {
         Amount = ActAmt * (VECVSharePer / 100);
         ApproveAmt = ActAmt * (VECVapprovedPer / 100);
     }

    //objRow[6].childNodes[0].value = RoundupValue(Amount);
    objRow.cells[5].children[0].value = RoundupValue(Amount);

    DealerSharePer = 100 - VECVSharePer;

    //objRow[7].childNodes[0].value = DealerSharePer;
    objRow.cells[6].children[0].value = DealerSharePer;
    
    // DealerapprovedPer = dGetValue(objRow.cells[10].children[0].value); //dGetValue(objRow[11].childNodes[0].value);
    if (ActFor == 'Request') {

        DealerapprovedPer = DealerSharePer;
    }
    else {
        DealerapprovedPer = 100 - VECVapprovedPer;
    }

    objRow.cells[10].children[0].value = DealerapprovedPer;

    //if (ActAmt > TentAmt && ActAmt != 0) {
    //    DAmount = (TentAmt * (DealerSharePer / 100)) + (ActAmt - TentAmt);
    //    DealerApproveAmt = TentAmt * (DealerapprovedPer / 100) + (ActAmt - TentAmt);
    //}
    //else if (ActAmt < TentAmt && ActAmt != 0) {

    if (ActFor == 'Request') {
       
        DAmount = TentAmt * (DealerSharePer / 100);
        DealerApproveAmt = TentAmt * (DealerapprovedPer / 100);
    }
    else {
        DAmount = ActAmt * (DealerSharePer / 100);
        DealerApproveAmt = ActAmt * (DealerapprovedPer / 100);
    }
    //}
    //else {
    //    DAmount = TentAmt * (DealerSharePer / 100);
    //    DealerApproveAmt = TentAmt * (DealerapprovedPer / 100);
    //}  

    //objRow[8].childNodes[0].value = RoundupValue(DAmount);
    objRow.cells[7].children[0].value = RoundupValue(DAmount);
    //objRow[10].childNodes[0].value = RoundupValue(ApproveAmt);
    objRow.cells[9].children[0].value = RoundupValue(ApproveAmt);
    //objRow[12].childNodes[0].value = RoundupValue(DealerApproveAmt);
    objRow.cells[11].children[0].value = RoundupValue(DealerApproveAmt);
    
    CalulateClaimTotal(RowCount, 2, ObjQtyControl);
    CalulateClaimTotal(RowCount,3, ObjQtyControl);
    CalulateClaimTotal(RowCount, 5, ObjQtyControl);
    CalulateClaimTotal(RowCount, 7, ObjQtyControl);
    CalulateClaimTotal(RowCount, 9, ObjQtyControl);
    CalulateClaimTotal(RowCount, 11, ObjQtyControl);

}




function CheckActivityMasterValidation() {
    var txtNameOfActivity = document.getElementById("ContentPlaceHolder1_txtNameOfActivity");
    //var txtObjective=document.getElementById("ContentPlaceHolder1_txtObjective");
    var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
    var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
    var drpDepartmentalActivity = document.getElementById("ContentPlaceHolder1_drpDepartmentalActivity");
    var drpTypeOfActivity = document.getElementById("ContentPlaceHolder1_drpTypeOfActivity");
    var drpDirectClaim = document.getElementById("ContentPlaceHolder1_drpDirectClaim");

    var drpCreditorPositionKey = document.getElementById("ContentPlaceHolder1_drpCreditorPositionKey");
    var drpEtbPostingKey = document.getElementById("ContentPlaceHolder1_drpEtbPostingKey");
    var drpCostCenter = document.getElementById("ContentPlaceHolder1_drpCosrCenter");
    var drpAccount = document.getElementById("ContentPlaceHolder1_drpAccount");


    if (txtNameOfActivity.value == '' && txtNameOfActivity.style.display == '') {
        alert("Please Enter Name Of Acivity !");
        txtNameOfActivity.focus();
        return false;
    }

    //     if(txtObjective.value == '')
    //    {
    //     alert("Please Objective Details !");
    //     txtObjective.focus();
    //        return false;
    //    }

    if (txtFromDate.value == '') {
        alert("Please Enter Activity will Start From Date !");
        txtFromDate.focus();
        return false;
    }
    if (txtToDate.value == '') {
        alert("Please Enter Activity will End To Date !");
        txtToDate.focus();
        return false;
    }
    if (drpDepartmentalActivity.value == "0") {
        alert("Please Select Departmental Activity !");
        drpDepartmentalActivity.focus();
        return false;
    }
    if (drpTypeOfActivity.value == "0") {
        alert("Please Select Activity  Type !");
        drpTypeOfActivity.focus();
        return false;
    }
    if (drpDirectClaim.value == "0") {
        alert("Please Select Direct Claim Allow(Yes/No) Activity !");
        drpDirectClaim.focus();
        return false;
    }
    ///
    if (drpCreditorPositionKey.value == "0") {
        alert("Please Select Creditor Posting Key !");
        drpCreditorPositionKey.focus();
        return false;
    }
    if (drpEtbPostingKey.value == "0") {
        alert("Please Select ETB Posting !");
        drpEtbPostingKey.focus();
        return false;
    }
//    if (drpCostCenter.value == "0") {
//        alert("Please Select Cost Center !");
//        drpCostCenter.focus();
//        return false;
//    }
//    if (drpAccount.value == "0") {
//        alert("Please Select GL Account !");
//        drpAccount.focus();
//        return false;
//    }
    var FromDate = getDateObject(txtFromDate.value, "/");
    var ToDate = getDateObject(txtToDate.value, "/");
    if (FromDate > ToDate) {
        alert("Activity Start Date Should Be Greater Than Acivity End Date");
        txtToDate.focus();
        return false;
    }

    return true;
}

function getDateObject(dateString, dateSeperator) {
    //This function return a date object after accepting 
    //a date string ans dateseparator as arguments
    var curValue = dateString;
    var sepChar = dateSeperator;
    var curPos = 0;
    var cDate, cMonth, cYear;

    //extract day portion
    curPos = dateString.indexOf(sepChar);
    cDate = dateString.substring(0, curPos);

    //extract month portion 
    endPos = dateString.indexOf(sepChar, curPos + 1); cMonth = dateString.substring(curPos + 1, endPos);

    //extract year portion 
    curPos = endPos;
    endPos = curPos + 5;
    cYear = curValue.substring(curPos + 1, endPos);

    //Create Date Object
    dtObject = new Date(cYear, cMonth, cDate);
    return dtObject;
}

function CheckActivityValidation() {
    var drpNameOfActivity = document.getElementById("ContentPlaceHolder1_drpNameOfActivity");
    var txtObjective = document.getElementById("ContentPlaceHolder1_txtObjective");
    var txtActivityReqDate = document.getElementById("ContentPlaceHolder1_txtActivityReqDate_txtDocDate");

    if (drpNameOfActivity != null) {
        if (drpNameOfActivity.value == "0") {
            alert("Please Select Acitvity For Request!");
            drpNameOfActivity.focus();
            return false;
        }

        if (txtObjective.value == '') {
            alert("Please Enter Objective!");
            txtObjective.focus();
            return false;
        }


    }

    if (ActivityGenerationDtlsValidation() == false) {
        return false;
    }

    return true;
}

function CheckActivityClaimValidation() {
    //var drpNameOfActivity = document.getElementById("ContentPlaceHolder1_drpNameOfActivity");
    //var txtObjective = document.getElementById("ContentPlaceHolder1_txtObjective");
    //var txtActivityReqDate = document.getElementById("ContentPlaceHolder1_txtActivityReqDate_txtDocDate");

    //if (drpNameOfActivity != null) {
    //    if (drpNameOfActivity.value == "0") {
    //        alert("Please Select Acitvity For Request!");
    //        drpNameOfActivity.focus();
    //        return false;
    //    }

    //    if (txtObjective.value == '') {
    //        alert("Please Enter Objective!");
    //        txtObjective.focus();
    //        return false;
    //    }


    //}

   if (ActivityGenerationDtlsClaimValidation() == false) {
        return false;
    }

    return true;
}


function ActivityGenerationDtlsValidation() {
 var ObjID = $("#ContentPlaceHolder1_GridActivityClaimDetails");
    var ObjGrid = ObjID[0];//window.document.getElementById("ContentPlaceHolder1_GridActivityClaimDetails");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var bCheckValidation = true;
    var sMessage = "";

    for (var i = 1; i < ObjGrid.rows.length - 1; i++) {

        //      Check Expected Exp. Head   
        //objtxtControl = ObjGrid.rows[i].cells[1].childNodes[0];
        objtxtControl = ObjGrid.rows[i].cells[1].children[0];
        if (objtxtControl.value == "" || objtxtControl.value == null) {
            alert("Please Enter Expected Exp. Head " + i);
            objtxtControl.focus();
            return false;
        }
        //Check Tentative Amount
        //objtxtControl = ObjGrid.rows[i].cells[2].childNodes[0];
        objtxtControl = ObjGrid.rows[i].cells[2].children[0];
        if (objtxtControl.value == "" || objtxtControl.value == null) {
            alert("Please Enter Tentative Amount " + i);
            objtxtControl.focus();
            return false;
        }
        //Check VECV % Share
        //objtxtControl = ObjGrid.rows[i].cells[3].childNodes[0];
         objtxtControl = ObjGrid.rows[i].cells[3].children[0];
        if (objtxtControl.value == "" || objtxtControl.value == null) {
            alert("Please Enter VECV % Share " + i);
            objtxtControl.focus();
            return false;
        }
    }

    return true;
}

function ActivityGenerationDtlsClaimValidation() {
    var ObjID = $("#ContentPlaceHolder1_GridActivityClaimDetails");
    var ObjGrid = ObjID[0];// window.document.getElementById("ContentPlaceHolder1_GridActivityClaimDetails");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var bCheckValidation = true;
    var sMessage = "";

    for (var i = 1; i < ObjGrid.rows.length - 1; i++) {

        //      Check Expected Exp. Head   
        //objtxtControl = ObjGrid.rows[i].cells[1].childNodes[0];
        objtxtControl = ObjGrid.rows[i].cells[1].children[0];
        if (objtxtControl.value == "" || objtxtControl.value == null || objtxtControl.value == "0") {
            alert("Please Enter Actual Exp. Head " + i);
            objtxtControl.focus();
            return false;
        }
        ////      Check Actual Exp. Head   
        ////objtxtControl = ObjGrid.rows[i].cells[2].childNodes[0];
        //objtxtControl = ObjGrid.rows[i].cells[2].children[0];
        //if (objtxtControl.value == "" || objtxtControl.value == null) {
        //    alert("Please Enter Actual Exp. Head " + i);
        //    objtxtControl.focus();
        //    return false;
        //}
        //Check Tentative Amount
        //objtxtControl = ObjGrid.rows[i].cells[3].childNodes[0];
        objtxtControl = ObjGrid.rows[i].cells[2].children[0];
        if (objtxtControl.value == "" || objtxtControl.value == null || objtxtControl.value <=0) {
            alert("Please Enter Tentative Amount " + i);
            objtxtControl.focus();
            return false;
        }
        ////Check Actual Amount
        ////objtxtControl = ObjGrid.rows[i].cells[4].childNodes[0];
        //objtxtControl = ObjGrid.rows[i].cells[4].children[0];
        //if (objtxtControl.value == "" || objtxtControl.value == null) {
        //    alert("Please Enter Actual Amount " + i);
        //    objtxtControl.focus();
        //    return false;
        //}
        ////Check VECV % Share
        ////objtxtControl = ObjGrid.rows[i].cells[5].childNodes[0];
        //objtxtControl = ObjGrid.rows[i].cells[5].children[0];
        //if (objtxtControl.value == "" || objtxtControl.value == null) {
        //    alert("Please Enter VECV % Share " + i);
        //    objtxtControl.focus();
        //    return false;
        //}
    }

    return true;
}

function ActivityClaimDtlsValidation() {
    var drpCostCenter = document.getElementById("drpCostCenter");
    var CosrCenterValue = drpCostCenter.options[drpCostCenter.selectedIndex].value;
    if (CosrCenterValue == "0") {
        alert("Please Select Cost center");
        drpCostCenter.focus();
        return false;
    }
    return true;
}

function BackClose() {
    window.close();
}
function newActivity() {
    var txtNameOfActivity = document.getElementById("ContentPlaceHolder1_txtNameOfActivity");
    var drpActivityName = document.getElementById("ContentPlaceHolder1_drpActivityName");
    var txtID = document.getElementById("ContentPlaceHolder1_txtID");

    if (drpActivityName.value == "9999") {
        txtNameOfActivity.style.display = '';
        txtNameOfActivity.value = "";
        drpActivityName.style.display = 'none';
        txtID.value = "0";
        txtNameOfActivity.focus();
    }
    else {
        for (var i = 0; i < drpActivityName.all.length; i++) {
            if (drpActivityName.all[i].value = drpActivityName.value) {
                txtNameOfActivity.value = drpActivityName.value;
                txtID.value = drpActivityName.value;
            }
        }
    }
    return false;
}


function ChkAprroved(ObjQtyControl) {
    var chkBox;
    var txtAprAmt;
 var ObjID = $("#" + ObjQtyControl.id);
    var objRow = ObjID[0].parentNode.parentNode; //ObjQtyControl.parentNode.parentNode.childNodes;
    chkBox = objRow.cells[6].children[0].checked;//objRow[7].all[1].checked;
    txtAprAmt = dGetValue(objRow.cells[8].children[0]); //objRow[8].childNodes[0];
    if (chkBox) {
        txtAprAmt.value = '';
        txtAprAmt.readOnly = false;
        txtAprAmt.focus();
    }
    else {
        txtAprAmt.value = '0';
        txtAprAmt.readOnly = true;
    }
}

function CheckBeforeApproveRecord() {
    //if (CheckBeforeSaveRecord() == false) return false;
    if (CheckRemarkIsEnter() == false) return false;
    if (confirm("Are you sure, you want to approve the document?") == true)
        return true;
    else
        return false;
}

function CheckActClmProValidation(obj) {
    debugger;
    var txtclaimFlage = document.getElementById("txtclaimFlage");
    var hdnRoleID = document.getElementById("hdnRoleID");
    var Status = obj;
    if (txtclaimFlage.value == 'Approval') {
        if (CheckRemarkIsEnter() == false) return false;
        if (confirm("Are you sure, you want to " + obj + " Activity Request?") == true) {
            // alert("Succeefully Approved Activity");
            return true;
        }
        else {
            //alert("Does not  " + obj + "d Activity");
            return false;
        }
    }
    else {
        if (obj == "Approve")
            obj = "Process"
        else
            obj = "Reject"
            
//        var txtRemark = document.getElementById("txtRemark");
//        if (txtRemark.value == '') {
//            alert("Please Enter Claim Processing Remark!");
//            txtRemark.focus();
//            return false;
        //        }
        if (hdnRoleID.value == "9" && txtclaimFlage.value == 'Processing' && Status=='Approve')       
        if (ActivityClaimDtlsValidation() == false) {
            return false;
        }
        if (CheckRemarkIsEnter() == false) return false;
        if (confirm("Are you sure, you want to " + obj + " Activity Claim?") == true) {
            //alert("Succeefully Claimed Activity ");
            return true;
        }
        else {
            //alert("Does Not Claimed Activity ");
            return false;
        }
    }
}


function CheckDateGreter(obj1, obj2, obj3, obj4, obj5) {

    var txtFromDate = obj4; //document.getElementById("<%=hdnFromDate.ClientID%>")
    var txtToDate = obj5; //document.getElementById("<%=hdnToDate.ClientID%>")

    var splDate = obj1.value.split("/")
    var splDate1 = obj2.value.split("/")
    var dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
    var dt1 = new Date(splDate1[2], splDate1[1] - 1, splDate1[0]);

    if (dt < dt1) {
        alert(obj3)
        d = dt1.getDate() + 1
        dt1.setDate(d);
        obj1.value = dt1.format("dd/MM/yyyy");
        obj1.focus();
        return false
    }

    var objFromDateValue = txtFromDate.value;
    var sFromValue = objFromDateValue;
    var Fday = dGetValue(sFromValue.split("/")[0]);
    var Fmonth = dGetValue(sFromValue.split("/")[1]) - 1;
    var Fyear = dGetValue(sFromValue.split("/")[2]);
    var sFromDate = new Date(Fyear, Fmonth, Fday);

    var objToDateValue = txtToDate.value;
    var sToValue = objToDateValue;
    var Tday = dGetValue(sToValue.split("/")[0]);
    var Tmonth = dGetValue(sToValue.split("/")[1]) - 1;
    var Tyear = dGetValue(sToValue.split("/")[2]);
    var sToDate = new Date(Tyear, Tmonth, Tday);

    if (dt > sToDate) {
        alert('Dealer Activity To Date Should be in betwwen VECV Activity Date')
        obj1.value = "";
        obj1.focus();
        return false;
    }
    if (dt > sToDate) {
        alert('Dealer Activity To Date Should be in betwwen VECV Activity Date')
        obj1.value = "";
        obj1.focus();
        return false;
    }
}
function SetCurrentAndFutureDate(obj, obj1, obj2) {

    var txtFromDate = obj1; //document.getElementById("<%=hdnFromDate.ClientID%>")
    var txtToDate = obj2; //document.getElementById("<%=hdnToDate.ClientID%>")

    var objDateValue = "";
    var ObjDate = obj;
    var x = new Date();
    var y = x.getYear();
    var m = x.getMonth() + 1; // added +1 because javascript counts month from 0
    var d = x.getDate();
    var dtCur = d + '/' + m + '/' + y;
    var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)

    objDateValue = ObjDate.value;
    var sTmpValue = objDateValue;
    var day = dGetValue(sTmpValue.split("/")[0]);
    var month = dGetValue(sTmpValue.split("/")[1]) - 1;
    var year = dGetValue(sTmpValue.split("/")[2]);
    var sTmpDate = new Date(year, month, day);
    var TmpDay = 0;

    if (objDateValue == '') {
        return false;
    }
    if (dtCurDate > sTmpDate) {
        alert('Dealer Activity From Date Should be Greater or equal to Current Date')
        ObjDate.value = "";
        ObjDate.focus();
        return false;
    }

    var objFromDateValue = txtFromDate.value;
    var sFromValue = objFromDateValue;
    var Fday = dGetValue(sFromValue.split("/")[0]);
    var Fmonth = dGetValue(sFromValue.split("/")[1]) - 1;
    var Fyear = dGetValue(sFromValue.split("/")[2]);
    var sFromDate = new Date(Fyear, Fmonth, Fday);

    var objToDateValue = txtToDate.value;
    var sToValue = objToDateValue;
    var Tday = dGetValue(sToValue.split("/")[0]);
    var Tmonth = dGetValue(sToValue.split("/")[1]) - 1;
    var Tyear = dGetValue(sToValue.split("/")[2]);
    var sToDate = new Date(Tyear, Tmonth, Tday);

    if (sTmpDate < sFromDate) {
        alert('Dealer Activity From Date Should be in betwwen VECV Activity Date')
        ObjDate.value = "";
        ObjDate.focus();
        return false;
    }
    if (sTmpDate > sToDate) {
        alert('Dealer Activity From Date Should be in betwwen VECV Activity Date')
        ObjDate.value = "";
        ObjDate.focus();
        return false;
    }

}

function CheckExpHeadAlreadyUsedInGrid(event, ObjCurRecord) {
   // debugger;

    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.value;
    //var sSelecedValue = ObjCurRecord.text;
    
    var ObjRecord;
    var ObjID = $("#" + ObjCurRecord.id);
    var objGrid = ObjID[0].parentNode.parentNode.parentNode; //ObjCurRecord.parentNode.parentNode.parentNode;
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.rowIndex); //parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    for (i = 1; i < objGrid.children.length - 1; i++) {
        ObjRecord = objGrid.rows[i].cells[1].children[0];//objGrid.childNodes[i].childNodes[1].children[0];

        if (i != iRowOfSelectControl) {
            if (ObjRecord !=undefined)
            if (ObjRecord.type == "text") {
                if (sSelecedValue.toUpperCase() == ObjRecord.value.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    ObjCurRecord.selectedIndex = 0;
                    event.returnValue = false;
                    if (bUsed == false) {
                        ObjCurRecord.focus();
                        bUsed = true;
                    }
                    else {
                        bUsed = null;
                    }
                    return false;
                }

            }
            else if (ObjRecord.type == "select-one") {
                //if (sSelecedValue.toUpperCase() == ObjRecord.options[ObjRecord.selectedIndex].text.toUpperCase()) {
                    if (sSelecedValue.toUpperCase() == ObjRecord.options[ObjRecord.selectedIndex].value.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    ObjCurRecord.selectedIndex = 0;
                    if (bUsed == false) {
                        ObjCurRecord.focus();
                        bUsed = true;

                    }
                    else {
                        bUsed = null;
                    }
                    return false;
                }
            }
        }
    }
    return true;
}

//Check Multi Text Length on key press
function ToCheckTextLength(Obj, MaxLength) {
    if (Obj != null) {
        var sTextValue = Obj.value.trim();
        if (Obj.value.trim().length > MaxLength) {
            alert("Remark Length should not be exceed to 500 character");
            Obj.focus();
            Obj.value = sTextValue.trim().slice(0, MaxLength);
            return false;
        }
    }
}

function trim(str) {
    //            if (!str || typeof str != 'string')
    //                return null;

    return str.replace(/^[\s]+/, '').replace(/[\s]+$/, '').replace(/[\s]{2,}/, ' ');
}

// To Check REamrk is Entered or not
function CheckRemarkIsEnter(obj) {
   // debugger;
    var ObjRemark;
    var UserDeptID;
    // Check RPM REmark
    var hdnRoleID = document.getElementById("hdnRoleID");
    UserDeptID = document.getElementById("txtUserDeptID.Text");
    ObjRemark = document.getElementById("txtASMRemark");
    if (ObjRemark != null && hdnRoleID.value == "3") {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }

    ObjRemark = document.getElementById("txtRSMRemark");
    if (ObjRemark != null && hdnRoleID.value == "2") {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check Parts Head REmark
    ObjRemark = document.getElementById("txtHeadRemark");
    if (ObjRemark != null && hdnRoleID.value == "1" && UserDeptID=="6") {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check After Sales Head REmark
    ObjRemark = document.getElementById("txtAfterSalesHeadRemark");
    if (ObjRemark != null && hdnRoleID.value == "1" && UserDeptID == "7") {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
}