//********************* Part Function******************//

//To Show Part Master
function ShowSpWPFPart(objNewPartLabel, sDealerId) {
    var txtDocDate = document.getElementById('ContentPlaceHolder1_txtDocDate_txtDocDate');
    var PartDetailsValue;
    var sJobtype = "";
    var EstID = "";
    var sSelectedPartID = GetPreviousSelectedPartIDInJobCard();    
    //var hdnSelectedPartID = document.getElementById("ContentPlaceHolder1_hdnSelectedPartID");
    //var sSelectedPartID = hdnSelectedPartID.value;
    var drpJobType = window.document.getElementById('ContentPlaceHolder1_drpJobType');
    if (drpJobType == null) return;
    if (drpJobType.selectedIndex == 0) {
        alert('Please Select Jobcard Type !');
        return false;
    }
    else {
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }
    //if (sJobtype.trim() == "2")
    //{
        var DrpEstimate = window.document.getElementById('ContentPlaceHolder1_DrpEstimate');
        if (DrpEstimate == null) return;
        if (DrpEstimate.selectedIndex == 0 && sJobtype.trim() == "2") {
            alert('Please Select Estimate !');
            return false;
        }
        else {
            EstID = DrpEstimate.options[DrpEstimate.selectedIndex].value;
        }
    //}
    debugger;
    var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
    var sCustTaxTag = hdnCustTaxTag.value;

    var hdnISDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
    var sDocGST = hdnISDocGST.value;

    PartDetailsValue = window.showModalDialog("../Common/frmSelectMultipart1.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtDocDate.value + "&EstID=" + EstID + "&CustTaxTag=" + sCustTaxTag + "&sDocGST=" + sDocGST, "List", "dialogHeight: 550px; dialogWidth: 700px;");
}

// To Get Part Id which are previously selected by user.
function GetPreviousSelectedPartIDInJobCard() {
    var objRow;
    var PartIds = "";
    var PartId = "";
    var txtPartId;
    // get grid object    
    var objGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");
    if (objGrid == null) return PartIds;    
    for (var i = 1; i < objGrid.rows.length; i++) {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Textbox of the Part ID
        txtPartId = objGrid.rows[i].children[1].childNodes[1];
       var txtDtlId = objGrid.rows[i].children[38].childNodes[1];
       var txtReqqty = objGrid.rows[i].children[6].childNodes[1];
       var txtIssueQty = objGrid.rows[i].children[7].childNodes[1];

        //Get PartId;
       PartId = dGetValue(txtPartId.value);
       if (PartId != "0" && (txtDtlId.value == "0" || dGetValue(txtReqqty.value) > dGetValue(txtIssueQty.value))) {
           PartIds = PartIds + PartId + ",";
       }
    }
    PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

    return PartIds;
}


//To Show Multi Part Selection
function ShowMultiPartMaster(objNewPartLabel, sDealerId) {
    var txtDocDate = document.getElementById('ContentPlaceHolder1_txtDocDate_txtDocDate');
    var drpClaimType = document.getElementById('ContentPlaceHolder1_DropClaimTypes');
    //debugger;
    if (txtDocDate != null)
        if (txtDocDate.value == "") {
        alert("Please enter Jobcard date.");
        txtDocDate.focus();
        return false;
    }
    var PartDetailsValue;
    var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
    //PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectMultiPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtDocDate.value, "List", "scrollbars:no;resizable:no;dialogWidth:800px;dialogHeight:800px;");
    PartDetailsValue = window.showModalDialog("../Common/frmSelectMultiPart1.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtDocDate.value, "List", "scrollbars:no;resizable:no;dialogWidth:800px;dialogHeight:800px;");
    var hdnNewAddPart = document.getElementById('ContentPlaceHolder1_hdnNewAddPart');
    
//    if (PartDetailsValue != null) {
//        SetMultiPartDetails(objNewPartLabel, PartDetailsValue);
//    }
}

//SetPartDetails
function SetMultiPartDetails(objAddNewControl, PartValue) {
    var gridView = null;
    //debugger;
    gridView = document.getElementById('ContentPlaceHolder1_PartDetailsGrid');
    if (gridView == null) return;
    var iColCnt = 1;
    var rows = gridView.rows;
    var objRow;
    var iCnt = 0;
    var iStartRowCnt = 0;
    var lblPartRecCnt = document.getElementById('ContentPlaceHolder1_lblPartRecCnt');
    

//    if (PartValue.length == 1) {
//        var tbod = gridView.rows[0].parentNode;
//        var newRow = gridView.rows[gridView.rows.length - 1].cloneNode(true);
//        tbod.appendChild(newRow);
//    }
//    else {
//        var iRow = (rows.length - 1) - (dGetValue(lblPartRecCnt.innerText) + PartValue.length);
//        if (iRow < 0) iRow = iRow * -1;

//        if (iRow > 0) {
//            for (iRowCnt = 0; iRowCnt < iRow + 1; iRowCnt++) {
//                var tbod = gridView.rows[0].parentNode;
//                var newRow = gridView.rows[gridView.rows.length - 1].cloneNode(true);
//                tbod.appendChild(newRow);
//            }
//        }
//    }



    for (iRowCnt = 0; iRowCnt < PartValue.length; iRowCnt++) {
        var tbod = gridView.rows[0].parentNode;
        var newRow = gridView.rows[gridView.rows.length - 1].cloneNode(true);
        tbod.appendChild(newRow);
    }
    
    iStartRowCnt = objAddNewControl.parentNode.parentNode.rowIndex;
    for (iRowCnt = iStartRowCnt; iRowCnt < rows.length; iRowCnt++) 
    {
        objRow = gridView.children[0].rows[iRowCnt].childNodes;
        iColCnt = 1;
        objRow[0].childNodes[0].innerText = iRowCnt;
        
        if (iCnt == PartValue.length) {
            iColCnt = iColCnt + 1;
            objRow[iColCnt].children[0].style.display = "";       // Show New Part button next button
            var objNewPart = objRow[iColCnt].children[1];
            if (objNewPart != null) {
                if (objNewPart.value != "") {
                    objRow[iColCnt].children[0].style.display = "none";
                }
            }
            SetPartsRecordCount();
            return;
        }
        else {                       
            objRow[5].children[2].style.display = "none";            
        }
        objRow[5].children[2].style.display = "none";
        //Set PartId;
        objRow[1].childNodes[0].value = PartValue[iCnt][0];

        //SetPartNo
        objRow[2].children[1].value = PartValue[iCnt][1];
        objRow[2].children[1].style.display = "";
        objRow[2].children[1].readOnly = true;
        objRow[2].children[0].style.display = "none";       // Hide New Part button         
        

        //SetPartName
        objRow[3].childNodes[0].value = PartValue[iCnt][2];
        objRow[3].childNodes[0].style.display = "";
        objRow[3].childNodes[0].readOnly = true;

        //SetQuantity        
        objRow[4].childNodes[0].value = '1';
        objRow[4].childNodes[0].focus();
                        
        //  Set Make        

//        //Set Replaced PartId;
//        objRow[5].childNodes[0].value = PartValue[iCnt][0];      


//        //Set Replaced PartNo
//        objRow[6].children[0].value = PartValue[iCnt][1];
//        objRow[6].children[0].style.display = "";
//        objRow[6].children[0].readOnly = true;
//        objRow[6].children[1].style.display = "";       // Hide New Part button             
//        

//        //Set Replaced PartName
//        objRow[7].childNodes[0].value = PartValue[iCnt][2];
//        objRow[7].childNodes[0].style.display = "";
//        objRow[7].childNodes[0].readOnly = true;
//        

//        //  Set Replaced Make
//        

//        //SetQuantity        
//        objRow[9].childNodes[0].value = '1';
//        objRow[9].childNodes[0].focus();
//        

//        //SetFoBRate            
//        var tmpvalue = PartValue[iCnt][3]
//        objRow[10].childNodes[0].value = tmpvalue;
//        objRow[10].childNodes[0].readOnly = true;
//        

//        //Total        
//        objRow[11].childNodes[0].readOnly = true;
        

        iCnt = iCnt + 1;
    }
    
}

// Set Total Parts Record Count
function SetPartsRecordCount() {
    var ObjGrid;
    var iRecordCnt = 0;
    ObjGrid = document.getElementById("ContentPlaceHolder1_PartDetailsGrid");
    if (ObjGrid == null) return;
    var iPartID = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {        
        
        //Set PartId;
        iPartID = dGetValue(ObjGrid.rows[i].cells[1].children[0].value );
        if (iPartID != 0) 
        {
            iRecordCnt = iRecordCnt + 1;            
        }
    }
    // To calculate Total Part  Count
    var ObjPartsCount = document.getElementById("ContentPlaceHolder1_lblPartRecCnt");
    if (ObjPartsCount != null) {
        ObjPartsCount.innerText = iRecordCnt;
    }
}

//Calculate Total For Part Details
function CalculateLineTotalForPart(event, ObjQtyControl) {
    
    var dOrgLinePartAmt;
    var dLinePartAmt;
    var dDiffOfPartAmt;
    var Rate
    debugger;

    //if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
    //    ObjQtyControl.value = '';
    //    return;
    //}
        // Calculate Line Level Part amount
        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

        //var ReqQty = dGetValue(objRow[6].childNodes[0].value);
        //var IssueQty = dGetValue(objRow[7].childNodes[0].value);
        //var ReturnQty = dGetValue(objRow[8].childNodes[0].value);
        //var BillQty = dGetValue(objRow[9].childNodes[0].value);
        var PartType = objRow[37].childNodes[1].value.trim();
        var LubLocID = dGetValue(objRow[33].childNodes[1].value);
        if (PartType == "O" && LubLocID == 0) {
            alert("Select Pouring Location.");
            ObjQtyControl.value = '';
            return;
        }
        var lubCapasityQty = 0;

        if (PartType == "O" && LubLocID != 0) {
            ////debugger;
            var objLubLocCapasityID = objRow[34].childNodes[1];
            lubCapasityQty = dGetValue(objLubLocCapasityID[objLubLocCapasityID.selectedIndex].text);
            lubCapasityQty = RoundupValue(lubCapasityQty + dGetValue(lubCapasityQty * 0.05));
        }
        var ReqQty = dGetValue(objRow[7].childNodes[1].value);
        var IssueQty = dGetValue(objRow[8].childNodes[1].value);
        var ReturnQty = dGetValue(objRow[9].childNodes[1].value);
        
        var StockQty = dGetValue(objRow[38].childNodes[1].value);
        var PrvIssueQty = dGetValue(objRow[38].childNodes[3].value);
        var PrvRetQty = dGetValue(objRow[38].childNodes[5].value);

        if (ReqQty == 0 || ReqQty == "") {
            alert("Required Qty should not be blank.");
            ObjQtyControl.value = '';
            return;
        }

        if (PartType == "O" && ReqQty > lubCapasityQty) {
            alert("Required Qty should not be greater than pouring location capasity.");
            ObjQtyControl.value = '';
            return;
        }
        if (ReqQty < IssueQty) {
            ObjQtyControl.value = '';
            alert("Issue Qty is greater than Required Qty for Part.");
            if (PrvIssueQty > 0) ObjQtyControl.value = PrvIssueQty;
            return;
        }
        if (PrvIssueQty > IssueQty && PrvIssueQty >0) {
            ObjQtyControl.value = '';
            alert("Previous Issue Qty is greater than editable Issue Qty for Part.");
            ObjQtyControl.value = PrvIssueQty;
            return;
        }
        if (PrvRetQty > ReturnQty && PrvRetQty > 0) {
            ObjQtyControl.value = '';
            alert("Return Qty cannot be reduced.");
            ObjQtyControl.value = PrvRetQty;
            return;
        }

        if (IssueQty > dGetValue(StockQty) + dGetValue(PrvIssueQty)) {
            ObjQtyControl.value = '';
            alert("Issue Qty is greater than Stock Qty for Part.");
            if (PrvIssueQty > 0) ObjQtyControl.value = PrvIssueQty;
            return;
        }
        if (IssueQty < ReturnQty) {
            ObjQtyControl.value = '';
            alert("Return Qty is greater than Issue Qty for Part.");
            return;
        }
        var UseQty = dGetValue(IssueQty - ReturnQty);
        objRow[10].childNodes[1].value = UseQty;

        var FOCQty = dGetValue(objRow[14].childNodes[1].value);
        var FSCQty = dGetValue(objRow[16].childNodes[1].value);
        var PDIQty = dGetValue(objRow[17].childNodes[1].value);
        var AMCQty = dGetValue(objRow[18].childNodes[1].value);
        var CampaignQty = dGetValue(objRow[19].childNodes[1].value);
        var TransitQty = dGetValue(objRow[20].childNodes[1].value);
        var EnrouteTQty = dGetValue(objRow[21].childNodes[1].value);
        var EnrouteNTQty = dGetValue(objRow[22].childNodes[1].value);
        var SpWarrQty = dGetValue(objRow[23].childNodes[1].value);
        var GoodWillQty = dGetValue(objRow[24].childNodes[1].value);
        var WarrQty = dGetValue(objRow[25].childNodes[1].value);
        var PrePDIQty = dGetValue(objRow[26].childNodes[1].value);
        var aggtQty = dGetValue(objRow[27].childNodes[1].value);

        //var BillQty = dGetValue(objRow[10].childNodes[1].value);
        var BillQty = UseQty - (FOCQty + FSCQty + PDIQty + AMCQty + CampaignQty + TransitQty + EnrouteTQty + EnrouteNTQty + SpWarrQty + GoodWillQty + WarrQty + PrePDIQty + aggtQty);
        objRow[11].childNodes[1].value = BillQty;

        //if ((ReturnQty - (BillQty + FOCQty + FSCQty + PDIQty + AMCQty + CampaignQty + TransitQty + EnrouteTQty + EnrouteNTQty + SpWarrQty + GoodWillQty + WarrQty + PrePDIQty + aggtQty)) < 0) {
        //    BillQty = BillQty - ObjQtyControl.value;
        //    objRow[11].childNodes[1].value = BillQty;
        //    ObjQtyControl.value = '';
        //    alert("Qty sum mismatch with used qty for Part.");
        //    return;
        //}
        if (BillQty < 0) {
            alert("Enter Correct Part Quantity for Part");
            return;
        }
        ObjQtyControl.value = ObjQtyControl.value;

        objRow[11].childNodes[1].value = BillQty;
        // Get Rate
        //Rate = dGetValue(objRow[24].childNodes[0].value);
        Rate = dGetValue(objRow[30].childNodes[1].value);

        // Calculate Line Level Part Amt
        //dLinePartAmt = dGetValue(ObjQtyControl.value) * Rate;
        dLinePartAmt = dGetValue(BillQty) * Rate;
        dLinePartAmt = RoundupValue(dLinePartAmt);
        //objRow[25].childNodes[0].value = dLinePartAmt;
        objRow[31].childNodes[1].value = dLinePartAmt;
        //    AddAmountToTotal("Part", dDiffOfPartAmt);
        AddAmountToTotal();
}

// When user Click to delete the record then reduce the amount
function SelectDeleteCheckboxForPart(ObjChkDelete) {

    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    var sReqNo = "";

    // Get Line Level Part amount
    //debugger;
    var sReqNo = ObjChkDelete.parentNode.parentNode.parentNode.children[4].childNodes[1].value;
    if (ObjChkDelete.checked && sReqNo.trim() != "") {
        alert("Part can not delete because Requisition allready done.");
        ObjChkDelete.checked = false;
        return false;
    }
    else if (ObjChkDelete.checked && sReqNo == "") {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    AddAmountToTotal();
}

// When user Click on Cancel of part then clear the value of row
function ClearRowValueForPart(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;    
    var ObjControl;
    var TotalAmount;
    //objAddNewControl.style.display="none";
    //Set PartId;        
    objRow[1].childNodes[0].value = '';
    
    //SetPartNo
    objRow[2].children[0].style.display = "";
    objRow[2].children[1].value = '';
    objRow[2].children[1].style.display = "none";
    
    
    //SetPartName
    objRow[3].childNodes[0].value = '';

    //SetQuantity
    objRow[4].childNodes[0].value = '';

    //SetNewLabel Display       
    ObjControl = objRow[5].children[1];
    if (ObjControl != null) ObjControl.style.display = "none";    
    
//    // Set Failed Make
//    objRow[4].childNodes[0].value = '';

//    // set Replaced Partid
//    objRow[5].childNodes[0].value = '';

//    // set Replaced PartNo
//    objRow[6].children[1].style.display = "none";
//    objRow[6].children[0].value = '';
//    

//    //Set Replaced PartName
//    objRow[7].childNodes[0].value = '';    

//    // Set Replaced Make
//    objRow[8].childNodes[0].value = '';
//    
//    //SetQuantity
//    objRow[9].childNodes[0].value = '';
//    
//    //SetFoBRate        
//    objRow[10].childNodes[0].value = '';

//    //Total    
//    TotalAmount = dGetValue(objRow[11].childNodes[0].value);
//    objRow[11].childNodes[0].value = '';
//    TotalAmount = (0 - TotalAmount);
//    AddAmountToTotal("Part", TotalAmount);

//    // set Job Code
//    objRow[12].childNodes[0].selectedIndex = 0;

//    //Set VECV Percentage
//    var ObjControl = objRow[13].childNodes[0];
//    if (ObjControl.readOnly != true) {
//        ObjControl.value = '';
//    }

//    ObjControl = objRow[14].childNodes[0];
//    if (ObjControl.readOnly != true) 
//    {
//        ObjControl.value = '';
//    }
//    ObjControl = objRow[15].childNodes[0];
//    if (ObjControl.readOnly != true) {
//        ObjControl.value = '';
//    }
//    

//    
//    //SetNewLabel Display       
//    ObjControl = objRow[16].children[1];
//    if (ObjControl != null) ObjControl.style.display = "none";

    SetPartsRecordCount();
}

function ClearRowValueForPartWarranty(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjControl;
    var TotalAmount;
    //objAddNewControl.style.display="none";
    //Set PartId;        
    objRow[1].childNodes[0].value = '';

    //SetPartNo
    objRow[2].children[0].style.display = "";
    objRow[2].children[1].value = '';
    objRow[2].children[1].style.display = "none";


    //SetPartName
    objRow[3].childNodes[0].value = '';

    // Set Failed Make
    objRow[4].childNodes[0].value = '';

    // set Replaced Partid
    objRow[5].childNodes[0].value = '';

    // set Replaced PartNo
    objRow[6].children[1].style.display = "none";
    objRow[6].children[0].value = '';


    //Set Replaced PartName
    objRow[7].childNodes[0].value = '';

    // Set Replaced Make
    objRow[8].childNodes[0].value = '';

    //SetQuantity
    objRow[9].childNodes[0].value = '1';

    //SetFoBRate        
    objRow[10].childNodes[0].value = '0';

    //Total    
    TotalAmount = dGetValue(objRow[11].childNodes[0].value);
    objRow[11].childNodes[0].value = '0';
    TotalAmount = (0 - TotalAmount);
    AddAmountToTotal("Part", TotalAmount);

    // set Job Code
    objRow[12].childNodes[0].selectedIndex = 0;

    //Set VECV Percentage
    var ObjControl = objRow[13].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }

    ObjControl = objRow[14].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }
    ObjControl = objRow[15].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }



    //SetNewLabel Display       
    ObjControl = objRow[16].children[1];
    if (ObjControl != null) ObjControl.style.display = "";

    SetPartsRecordCount();
}

//To Show Part Master
function ShowPartMasterForReplaced(objChngPartLabel, sDealerId) {
    var PartDetailsValue;
    var sSelectedPartID = ""; 
    PartDetailsValue = window.showModalDialog("/DCS/Forms/Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=100%,dialogHeight=1000px");
    if (PartDetailsValue != null) {
        SetReplacePartDetails(objChngPartLabel, PartDetailsValue);
    }
}

//SetPartDetails
function SetReplacePartDetails(objChngPartLabel, PartValue) {
    var objRow = objChngPartLabel.parentNode.parentNode.childNodes;

    //Set Replace PartId;
    objRow[5].childNodes[0].value = PartValue[0];

    //Set Replace PartNo
    objRow[6].childNodes[0].value = PartValue[1];

    //Set Replace PartName
    objRow[7].childNodes[0].value = PartValue[2];


    //    //SetFoBRate
    //    var tmpvalue = PartValue[5];
    //    objRow[10].childNodes[0].value = tmpvalue;
    //    objRow[10].childNodes[0].readOnly = true;
    //    objRow[9].childNodes[0].focus();
}

function ShowCausalPartMaster(objChngPartLabel, sDealerId) {
    var PartDetailsValue;
    var sSelectedPartID = "";
    PartDetailsValue = window.showModalDialog("/DCS/Forms/Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=100%,dialogHeight=1000px");
    if (PartDetailsValue != null) {
        SetCausalPart(objChngPartLabel, PartDetailsValue);
    }
}

function SetCausalPart(objChngPartLabel, PartValue) {
    var objRow = objChngPartLabel.parentNode.parentNode.childNodes;

    //Set Replace PartId;
    objChngPartLabel.parentNode.childNodes[0].value = PartValue[0];

      
    //Set Replace PartNo
    objChngPartLabel.parentNode.childNodes[1].parentNode.childNodes[2].value = PartValue[1];
    //Tech
    if (PartValue[4] == "Y") {
        var objTechCode = document.getElementById('drpTechnicalCode');
        objTechCode.disabled = true ;
    } else {
        var objTechCode = document.getElementById('drpTechnicalCode');
        objTechCode.disabled = false;    
    } 
    //Set Replace PartName
    //objRow[7].childNodes[0].value = PartValue[2];


    //    //SetFoBRate
    //    var tmpvalue = PartValue[5];
    //    objRow[10].childNodes[0].value = tmpvalue;
    //    objRow[10].childNodes[0].readOnly = true;
    //    objRow[9].childNodes[0].focus();
}

// function Check JobCode Validation For Part
function CheckJobCodeValidationForPart(ObjJobCode) {
    var objRow = ObjJobCode.parentNode.parentNode.childNodes;
    var iCurrRowIndex = ObjJobCode.parentNode.parentNode.rowIndex;
    var PcontainerName = GetContainerName();
    var ObjGrid = window.document.getElementById(PcontainerName + "PartDetailsGrid");
    var objLGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
    
    var iJob = 0;
    var iCurrJobCode = 0;
    var sExist="N";
    var i = 1;
    if (ObjJobCode.selectedIndex != 0) {        
        iJob = dGetValue(ObjJobCode.selectedIndex);
    }
    var m=0,n=0;
    for (m = iJob - 1; m > 0; m--) {
        for (i = 1; i < ObjGrid.rows.length; i++) {
            if (i != iCurrRowIndex) {
                iCurrJobCode = dGetValue(ObjGrid.rows[i].cells[35].children[0].selectedIndex);
                if (iCurrJobCode == m) {
                    sExist = "Y";
                }
            }
        }
        if (objLGrid != null)
        {
            for (i = 1; i < objLGrid.rows.length; i++) {
                iCurrJobCode = dGetValue(ObjGrid.rows[i].cells[35].children[0].selectedIndex);
                if (iCurrJobCode == m) {
                    sExist = "Y";
                }
            }
        }
        if (sExist == "N") {
            alert("Please select jobcode J" + m + " first!.");
            ObjJobCode.selectedIndex = 0;
            return false;
        }
    }
    


//    if (ObjJobCode.selectedIndex == 0) {
//        var Objculprit = objRow[5].children[0];
//        if (Objculprit.selectedIndex != 0) {
//            Objculprit.selectedIndex = 0;
//        }
//    }
//    else if (ObjJobCode.selectedIndex != 0) {

//        if (objRow[1].childNodes[0].value == "0" || objRow[1].childNodes[0].value == "" || objRow[1].childNodes[0].value == undefined) {
//            alert("Please select the part first!.");
//            ObjJobCode.selectedIndex = 0;
//            return false;
//        }
//        else {
////            ////##
////            // check Selected Job code is already selected for the Part
////            var PcontainerName = GetContainerName();
////            var ObjGrid = window.document.getElementById(PcontainerName + "PartDetailsGrid");
////            var iCurrRowIndex = ObjJobCode.parentNode.parentNode.rowIndex;
////            var sSelectedJobCode = "";
////            var sCurrJobCode = "";
////            var i = 1;
////            var sPartNo = "";
////            var iMaxJobCodeID;
////            sSelectedJobCode = ObjJobCode.selectedIndex;
////            for (i = 1; i < ObjGrid.rows.length; i++) 
////            {
////                if (i != iCurrRowIndex) 
////                {
////                    sCurrJobCode = ObjGrid.rows[i].cells[12].children[0].selectedIndex;                                        
////                    if (sCurrJobCode != 0)
////                     {
////                        if (sCurrJobCode == sSelectedJobCode) {
////                            sPartNo = ObjGrid.rows[i].cells[2].children[1].value;
////                            alert("Job code is already selected for the part '" + sPartNo + "'");
////                            ObjJobCode.selectedIndex = 0;
////                            return false;
////                        }
////                    }
////                }
////            }
//            ///######                
//        } //Else
//    }
}

function SetLubCapasityOnLubLocationChange(ObjLubLoc, dLubCap) {
    var ParentCtrlID;
    var objLubCapControl;
    var Sup, Post;
    //debugger;

    //ParentCtrlID = ObjLubLoc.id.substring(0, ObjLubLoc.id.lastIndexOf("_"));
    Post = ObjLubLoc.id.substring(ObjLubLoc.id.lastIndexOf("_"), (ObjLubLoc.id).length);
    Sup = ObjLubLoc.id.substring(0, ObjLubLoc.id.lastIndexOf("_"));
    ParentCtrlID = Sup.substring(0, Sup.lastIndexOf("_"));

    objLubCapControl = document.getElementById(ParentCtrlID + "_" + dLubCap + Post);
    objLubCapControl.selectedIndex = ObjLubLoc.selectedIndex;
    
    return true;
}

