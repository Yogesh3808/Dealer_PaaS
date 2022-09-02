//********************* Part Function******************//

//To Show Part Master
function ShowSpWPFPart(objNewPartLabel, sDealerId) {      
    var txtDocDate = document.getElementById('ContentPlaceHolder1_txtDocDate_txtDocDate');
    var PartDetailsValue;

    var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
    var sCustTaxTag = hdnCustTaxTag.value;

    var sSelectedPartID = GetPreviousSelectedPartIDInJobCard();    
   // PartDetailsValue = window.showModalDialog("../Common/frmSelectMultipart1.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtDocDate.value + "&EstID=0&CustTaxTag=" + sCustTaxTag, "List", "scrollbars=no,resizable=no,dialogWidth=400px,dialogHeight=300px");
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

        //Get PartId;
        PartId = dGetValue(txtPartId.value);
        if (PartId != "0") {
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


    for (iRowCnt = 0; iRowCnt < PartValue.length; iRowCnt++) {
        var tbod = gridView.rows[0].parentNode;
        var newRow = gridView.rows[gridView.rows.length - 1].cloneNode(true);
        tbod.appendChild(newRow);
    }

    iStartRowCnt = objAddNewControl.parentNode.parentNode.rowIndex;
    for (iRowCnt = iStartRowCnt; iRowCnt < rows.length; iRowCnt++) {
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
        iPartID = dGetValue(ObjGrid.rows[i].cells[1].children[0].value);
        if (iPartID != 0) {
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
    ////debugger;

    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        ObjQtyControl.value = '';
        return;
    }
    // Calculate Line Level Part amount
    var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
    var PartType = objRow[8].childNodes[1].value.trim();
   
    var ReqQty = dGetValue(objRow[5].childNodes[1].value);
  
    if (ReqQty == 0 || ReqQty == "") {
        alert("Estimated Qty should not be blank.");
        ObjQtyControl.value = '';
        return;
    }
  
    var UseQty =ReqQty;
   
    var BillQty = UseQty ;
  
    if (BillQty < 0) {
        alert("Enter Correct Part Quantity for Part");
        return;
    }
    ObjQtyControl.value = ObjQtyControl.value;

    //objRow[11].childNodes[1].value = BillQty;
    // Get Rate
    //Rate = dGetValue(objRow[24].childNodes[0].value);
    Rate = dGetValue(objRow[6].childNodes[1].value);

    // Calculate Line Level Part Amt
    //dLinePartAmt = dGetValue(ObjQtyControl.value) * Rate;
    dLinePartAmt = dGetValue(BillQty) * Rate;
    dLinePartAmt = RoundupValue(dLinePartAmt);
    //objRow[25].childNodes[0].value = dLinePartAmt;
    objRow[7].childNodes[1].value = dLinePartAmt;
    //    AddAmountToTotal("Part", dDiffOfPartAmt);
    AddAmountToTotal();
}

// When user Click to delete the record then reduce the amount
function SelectDeleteCheckboxForPart(ObjChkDelete) {

    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    var sReqNo = "";
   if (ObjChkDelete.checked) {
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



