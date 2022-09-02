
//********************* Part Function******************//

//To Show Part Master
function ShowSpWPFPart(objNewPartLabel, sDealerId) {
    var txtDocDate = document.getElementById('ContentPlaceHolder1_txtFailureDate_txtDocDate');
    if (txtDocDate != null)
        if (txtDocDate.value == "") {
            alert("Please enter Failure date.");
            txtDocDate.focus();
            return false;
        }
    var PartDetailsValue;
    var sClaimtype = "";
    var EstID = "";
    debugger;
    var sSelectedPartID = GetPreviousSelectedPartIDInWarranty();    
    //var hdnSelectedPartID = document.getElementById("ContentPlaceHolder1_hdnSelectedPartID");
    //var sSelectedPartID = hdnSelectedPartID.value;
    var drpClaimType = window.document.getElementById('ContentPlaceHolder1_DropClaimTypes');    
    if (drpClaimType == null) return;
    if (drpClaimType.selectedIndex == 0) {
        alert('Please Select Claim Type !');
        return false;
    }
    else {
        sClaimtype = drpClaimType.options[drpClaimType.selectedIndex].value;
    }    
    
    var hdnCustTaxTag = document.getElementById('ContentPlaceHolder1_hdnCustTaxTag');
    var sCustTaxTag = hdnCustTaxTag.value;

    var hdnISDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
    var sDocGST = hdnISDocGST.value;

    PartDetailsValue = window.showModalDialog("../Common/frmSelectMultipart1.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtDocDate.value + "&EstID=" + EstID + "&CustTaxTag=" + sCustTaxTag + "&sDocGST=" + sDocGST + "&sDocType=EXP", "List", "dialogHeight: 550px; dialogWidth: 700px;");
}

// To Get Part Id which are previously selected by user.
function GetPreviousSelectedPartIDInWarranty() {
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
    var txtRepairOrderDate = document.getElementById('ContentPlaceHolder1_txtRepairOrderDate_txtDocDate');
    var drpClaimType = document.getElementById('ContentPlaceHolder1_DropClaimTypes');

    if (txtRepairOrderDate != null)
        if (txtRepairOrderDate.value == "") {
        alert("Please enter repair order date in Vehicle Details Tab.");
        txtRepairOrderDate.focus();
        return false;
    }
    var PartDetailsValue;
    var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
    PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectMultiPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&RepairOrderDate=" + txtRepairOrderDate.value, "List", "scrollbars:no;resizable:no;dialogWidth:800px;dialogHeight:800px;");
    if (PartDetailsValue != null) {
        SetMultiPartDetails(objNewPartLabel, PartDetailsValue);
    }
}

//SetPartDetails
function SetMultiPartDetails(objAddNewControl, PartValue) {
    var gridView = null;
    gridView = document.getElementById('ContentPlaceHolder1_PartDetailsGrid');
    if (gridView == null) return;
    var iColCnt = 1;
    var rows = gridView.rows;
    var objRow;
    var iCnt = 0;
    var iStartRowCnt = 0;
    iStartRowCnt = objAddNewControl.parentNode.parentNode.rowIndex;
    for (iRowCnt = iStartRowCnt; iRowCnt < rows.length; iRowCnt++) 
    {
        objRow = gridView.children[0].rows[iRowCnt].childNodes;
        iColCnt = 1;

        if (iCnt == PartValue.length) 
        {
            iColCnt = iColCnt + 1;
            objRow[iColCnt].children[0].style.display = "";       // Show New Part button next button
            var objNewPart = objRow[iColCnt].children[1];
            if ( objNewPart != null)
             {
               if ( objNewPart.value != ""  )
               {
                objRow[iColCnt].children[0].style.display = "none";
               }                
             }            
            SetPartsRecordCount();
            return;
        }

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
        

        //  Set Make        

        //Set Replaced PartId;
        objRow[5].childNodes[0].value = PartValue[iCnt][0];      


        //Set Replaced PartNo
        objRow[6].children[0].value = PartValue[iCnt][1];
        objRow[6].children[0].style.display = "";
        objRow[6].children[0].readOnly = true;
        objRow[6].children[1].style.display = "";       // Hide New Part button             
        

        //Set Replaced PartName
        objRow[7].childNodes[0].value = PartValue[iCnt][2];
        objRow[7].childNodes[0].style.display = "";
        objRow[7].childNodes[0].readOnly = true;
        

        //  Set Replaced Make
        

        //SetQuantity        
        objRow[9].childNodes[0].value = '1';
        objRow[9].childNodes[0].focus();
        

        //SetFoBRate            
        var tmpvalue = PartValue[iCnt][3]
        objRow[10].childNodes[0].value = tmpvalue;
        objRow[10].childNodes[0].readOnly = true;
        

        //Total        
        objRow[11].childNodes[0].readOnly = true;
        

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
    debugger;
    var dOrgLinePartAmt;
    var dLinePartAmt;
    var dDiffOfPartAmt;
    var Rate

    if (CheckTextboxValueForNumeric(event, ObjQtyControl,false) == false) {
        ObjQtyControl.value = '1';
        return;
    }
    else 
    {
        ObjQtyControl.value = parseInt(ObjQtyControl.value);
    }
    // Calculate Line Level Part amount        
    var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
    // Get Rate
    Rate = dGetValue(objRow[11].childNodes[1].value);

    // Get Line Level Part amount    before calculation     
    dOrgLinePartAmt = dGetValue(objRow[12].childNodes[1].value);
    dOrgLinePartAmt = RoundupValue(dOrgLinePartAmt);

    // Calculate Line Level Part Amt
    dLinePartAmt = dGetValue(ObjQtyControl.value) * Rate;
    dLinePartAmt = RoundupValue(dLinePartAmt);
    objRow[12].childNodes[1].value = dLinePartAmt;


    //if (dLinePartAmt == 0) return;
    if (dOrgLinePartAmt == 0) {
        dDiffOfPartAmt = dLinePartAmt;
    }
    else if (dOrgLinePartAmt == dLinePartAmt) {
        dDiffOfPartAmt = 0;
    }
    else {
        dDiffOfPartAmt = (dLinePartAmt - dOrgLinePartAmt);
    }
    AddAmountToTotal("Part", dDiffOfPartAmt);
}
// When user Click to delete the record then reduce the amount 
function SelectDeleteCheckboxForPart(ObjChkDelete) {
    debugger;
    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    var dDiffOfPartAmt = 0;
    // Get Line Level Part amount
    var dOrgLinePartAmt = dGetValue(ObjChkDelete.parentNode.parentNode.parentNode.children[11].childNodes[1].value);

    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
            dDiffOfPartAmt = 0 - dOrgLinePartAmt
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
        dDiffOfPartAmt = dOrgLinePartAmt
    }
    AddAmountToTotal("Part", dDiffOfPartAmt);
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
    objRow[9].childNodes[0].value = '';
    
    //SetFoBRate        
    objRow[10].childNodes[0].value = '';

    //Total    
    TotalAmount = dGetValue(objRow[11].childNodes[0].value);
    objRow[11].childNodes[0].value = '';
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
    if (ObjControl.readOnly != true) 
    {
        ObjControl.value = '';
    }
    ObjControl = objRow[15].childNodes[0];
    if (ObjControl.readOnly != true) {
        ObjControl.value = '';
    }
    

    
    //SetNewLabel Display       
    ObjControl = objRow[16].children[1];
    if (ObjControl != null) ObjControl.style.display = "none";

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
    //debugger;
    PartDetailsValue = window.showModalDialog("../Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=100%,dialogHeight=1000px");
    if (PartDetailsValue != null) {
        SetReplacePartDetails(objChngPartLabel, PartDetailsValue);
    }
}

//SetPartDetails
function SetReplacePartDetails(objChngPartLabel, PartValue) {
    var objRow = objChngPartLabel.parentNode.parentNode.childNodes;
    //debugger;
    //Set Replace PartId;
    //objRow[5].childNodes[0].value = PartValue[0];
    objRow[6].childNodes[1].value = PartValue[1];

    //Set Replace PartNo
    //objRow[6].childNodes[0].value = PartValue[1];
    objRow[7].childNodes[1].value = PartValue[2];

    //Set Replace PartName
    //objRow[7].childNodes[0].value = PartValue[2];
    objRow[8].childNodes[1].value = PartValue[3];


    //    //SetFoBRate
    //    var tmpvalue = PartValue[5];
    //    objRow[10].childNodes[0].value = tmpvalue;
    //    objRow[10].childNodes[0].readOnly = true;
    //    objRow[9].childNodes[0].focus();
}


function ShowCausalPartMaster(objChngPartLabel, sDealerId) {
    var PartDetailsValue;
    var sSelectedPartID = "";
    PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=100%,dialogHeight=1000px");
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
    if (ObjJobCode.selectedIndex == 0) {
        var Objculprit = objRow[5].children[0];
        if (Objculprit.selectedIndex != 0) {
            Objculprit.selectedIndex = 0;
        }
    }
    else if (ObjJobCode.selectedIndex != 0) {

        if (objRow[1].childNodes[0].value == "0" || objRow[1].childNodes[0].value == "" || objRow[1].childNodes[0].value == undefined) {
            alert("Please select the part first!.");
            ObjJobCode.selectedIndex = 0;
            return false;
        }
        else {
//            ////##
//            // check Selected Job code is already selected for the Part
//            var PcontainerName = GetContainerName();
//            var ObjGrid = window.document.getElementById(PcontainerName + "PartDetailsGrid");
//            var iCurrRowIndex = ObjJobCode.parentNode.parentNode.rowIndex;
//            var sSelectedJobCode = "";
//            var sCurrJobCode = "";
//            var i = 1;
//            var sPartNo = "";
//            var iMaxJobCodeID;
//            sSelectedJobCode = ObjJobCode.selectedIndex;
//            for (i = 1; i < ObjGrid.rows.length; i++) 
//            {
//                if (i != iCurrRowIndex) 
//                {
//                    sCurrJobCode = ObjGrid.rows[i].cells[12].children[0].selectedIndex;                                        
//                    if (sCurrJobCode != 0)
//                     {
//                        if (sCurrJobCode == sSelectedJobCode) {
//                            sPartNo = ObjGrid.rows[i].cells[2].children[1].value;
//                            alert("Job code is already selected for the part '" + sPartNo + "'");
//                            ObjJobCode.selectedIndex = 0;
//                            return false;
//                        }
//                    }
//                }
//            }
            ///######                
        } //Else
    }
}
