// When user Click on Cancel then clear the value of row
function ClearRowValueforNewModel(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjCell;
    var icntOfChildren;
    var bUseChildren = true;
    for (var i = 0; i < objRow.length; i++) {
        icntOfChildren = objRow[i].children.length;
        bUseChildren = true;
        if (icntOfChildren == 0) {
            icntOfChildren = objRow[i].childNodes.length;
            bUseChildren = false;
        }
        for (var j = 0; j < icntOfChildren; j++) {
            if (bUseChildren == true) {
                ObjCell = objRow[i].children[j];
            }
            else {
                ObjCell = objRow[i].childNodes[j];
            }

            if (ObjCell.type == "text") {
                ObjCell.value = '';
            }
            else if (ObjCell.type == "textarea") {
                ObjCell.value = '';
                ObjCell.style.display = '';
            }
            else if (ObjCell.type == "select-one") {
                ObjCell.selectedIndex = 0;
                ObjCell.style.display = '';
            }
             else if (ObjCell.type == "hidden") {
                ObjCell.value = '';
            }
            
        }

    }
}
// To Check RFQ Type Is Selected
function CheckRFQTypeIsSelected() {
    var objRFQTypeCombo = document.getElementById('ContentPlaceHolder1_drpRFQSelection');
    if (objRFQTypeCombo == null) return;

    if (objRFQTypeCombo.selectedIndex == 0) {
        alert('Please first select the RFQ type !');
        return false;
    }
    else {
        return true;
    }
}

// To Check RFQ Type Is Selected
function CheckRFQRemarkIsEntered() 
{
    var objRFQRemark = document.getElementById('ContentPlaceHolder1_txtRemarks');
    if (objRFQRemark == null) return;

    if (objRFQRemark.value == "") {
        alert('Please Enter the Remarks !');
        return false;
    }
    else {
        return true;
    }
}


function SetValueOnFertCodeChange(ObjFertCode, txtboxId, dModelID)
{
    var ParentCtrlID;
    var objModelControl;
    ParentCtrlID=ObjFertCode.id.substring(0, ObjFertCode.id.lastIndexOf("_"));
    objModelControl = document.getElementById(ParentCtrlID + "_" + dModelID);
    objModelControl.selectedIndex = ObjFertCode.selectedIndex;
    if (SetValueOnModelChange(objModelControl, txtboxId, null,null) == false) {
        ObjFertCode.selectedIndex = 0;
        return false;
    }
    if (CheckModelAlreadyExist(objModelControl)==false)
    {
        ObjFertCode.selectedIndex=0;
        return false;
    }    
    return true;
}

//Set Value Of the Fertcode on Model Change
function SetValueOnModelChange(ObjModel,txtboxId,FertCodeID,NewDesc) {
    if (ObjModel.selectedIndex == -1) return;
   var sSelecedValue= ObjModel.options[ObjModel.selectedIndex].text;
   var ParentCtrlID;
   var objtxtControl;
   if ( sSelecedValue =="NEW")
   {    
        ObjModel.style.display='none';
        ParentCtrlID=ObjModel.id.substring(0, ObjModel.id.lastIndexOf("_"));    
        //objtxtControl= document.getElementById(ParentCtrlID+"_"+ txtboxId);
        //objtxtControl.style.display='' ;
        //objtxtControl.focus();    
        if(FertCodeID != null)
            SetFertCode(ObjModel, FertCodeID,NewDesc, false);
//        //Reset Checkbox
//        // reset Confirm Check box
//        var ObjAccept = ObjModel.parentNode.parentNode.childNodes[5].childNodes[0].children[0].checked;
//        var objConfirm = ObjModel.parentNode.parentNode.childNodes[5].childNodes[0].children[0].checked;
//        objConfirm.checked = false;
   }
   else
   {    ObjModel.style.display='';
        ParentCtrlID=ObjModel.id.substring(0, ObjModel.id.lastIndexOf("_"));    
        //objtxtControl= document.getElementById(ParentCtrlID+"_"+ txtboxId);
        //objtxtControl.style.display='none' ;           
        //objtxtControl.value='';
        if (CheckModelAlreadyExist(ObjModel) == true) {
            if (FertCodeID != null)
                SetFertCode(ObjModel, FertCodeID,NewDesc, true);
        }
        else {
            return false;
        }
    }
}

//check model already select in previous / next row in grid
function CheckModelAlreadyExist(ObjCurModel) {

    if (ObjCurModel.selectedIndex == 0) return;
    var i;
    var sSelecedValue= ObjCurModel.options[ObjCurModel.selectedIndex].text;
    var iRowOfSelectControl = parseInt(ObjCurModel.parentNode.parentNode.rowIndex);
    var ObjModel ;
    var objGrid=ObjCurModel.parentNode.parentNode.parentNode;
    for (i=1;i< objGrid.children.length;i++)
    {
        ObjModel =objGrid.childNodes[i].childNodes[2].children[0];
        
        if(i!=iRowOfSelectControl ) {

            if (sSelecedValue != "NEW") {
                if (sSelecedValue == ObjModel.options[ObjModel.selectedIndex].text) {
                    alert("Record is already selected at line No." + i);
                    ObjCurModel.selectedIndex = 0;
                    objGrid.childNodes[iRowOfSelectControl].children[3].children[0].selectedIndex = 0;
                    //ObjCurModel.focus();
                    return false;
                }
            }
        }
    }
    return true;
}

function SetFertCode(ObjModel,FertCodeID,NewDesc,bDisplay)
{
    var ParentCtrlID;
    var objFertCodeControl;
    var objNewDesc;
    ParentCtrlID=ObjModel.id.substring(0, ObjModel.id.lastIndexOf("_"));    
    objFertCodeControl= document.getElementById(ParentCtrlID+"_"+ FertCodeID);
    objNewDesc= document.getElementById(ParentCtrlID+"_"+ NewDesc);
    objFertCodeControl.selectedIndex=ObjModel.selectedIndex;    
    if (bDisplay == false)
    {
        objFertCodeControl.style.display="none";
        objNewDesc.style.display="";
        objNewDesc.disabled=false;
    }
    else
    {
        objFertCodeControl.style.display="";
        objNewDesc.style.display="";
        objNewDesc.disabled=true;
    }
    
    return true;
}

//check model already select in previous / next row in grid
function CheckModelAlreadyExist(ObjCurModel) {

    if (ObjCurModel.selectedIndex == 0) return;
    var i;
    var sSelecedValue= ObjCurModel.options[ObjCurModel.selectedIndex].text;
    var iRowOfSelectControl = parseInt(ObjCurModel.parentNode.parentNode.rowIndex);
    var ObjModel ;
    var objGrid=ObjCurModel.parentNode.parentNode.parentNode;
    for (i=1;i< objGrid.children.length;i++)
    {
        ObjModel =objGrid.childNodes[i].childNodes[2].children[0];
        
        if(i!=iRowOfSelectControl ) {

            if (sSelecedValue != "NEW") {
                if (sSelecedValue == ObjModel.options[ObjModel.selectedIndex].text) {
                    alert("Record is already selected at line No." + i);
                    ObjCurModel.selectedIndex = 0;
                    objGrid.childNodes[iRowOfSelectControl].children[3].children[0].selectedIndex = 0;
                    //ObjCurModel.focus();
                    return false;
                }
            }
        }
    }
    return true;
}

//To Show Fert Master
function ShowFertMaster(objNewPartLabel, sDealerId) {
    var PartDetailsValue;
    var sSelectedPartID = GetPreviousSelectedFertID(objNewPartLabel);
    PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectFert.aspx?DealerID=" + sDealerId + "&SelectedFertID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
    if (PartDetailsValue != null) {
        SetFertDetails(objNewPartLabel, PartDetailsValue);
    }
}

// To Get Part Details
function ReturnFertDetails(objImgControl)
{
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    var ArrOfParts = new Array();
    var sCellValue = "";   
    
    
    // Add Part ID        
    sCellValue = objRow[1].children[0].innerText;
    ArrOfParts.push(sCellValue);

    // Add PartNo
    sCellValue = objRow[2].children[0].innerText;
    ArrOfParts.push(sCellValue);

    // Add PartName
    sCellValue = objRow[3].children[0].innerText;
    ArrOfParts.push(sCellValue);   
  window.returnValue = ArrOfParts; 
  window.close(); 
}

//To Show Part Master
function ShowPartMaster(objNewPartLabel, sDealerId) {
    var PartDetailsValue;
    var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
    PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
    if (PartDetailsValue != null) {
        SetFertDetails(objNewPartLabel, PartDetailsValue);
    }
}



// To Get Part Details
function ReturnPartDetails(objImgControl)
{
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    var ArrOfParts = new Array();
    var sCellValue = "";   
    
    
    // Add Part ID        
    sCellValue = objRow[1].children[0].innerText;
    ArrOfParts.push(sCellValue);

    // Add PartNo
    sCellValue = objRow[2].children[0].innerText;
    ArrOfParts.push(sCellValue);

    // Add PartName
    sCellValue = objRow[3].children[0].innerText;
    ArrOfParts.push(sCellValue);   
  window.returnValue = ArrOfParts; 
  window.close(); 
}

//SetPartDetails
function SetFertDetails(objAddNewControl, PartValue) {
    //objLabel.parentNode.childNodes[1].value=Value[1];
    if (PartValue.length == 0) return;
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    objAddNewControl.style.display = "none";
    //Set PartId;
    objRow[4].childNodes[2].style.display = "";
    objRow[2].childNodes[0].value=PartValue[2];
    objRow[2].childNodes[2].value=PartValue[2];
    objRow[4].childNodes[0].value=PartValue[1];
    objRow[4].childNodes[4].value=PartValue[1];  
    objRow[5].childNodes[0].value=PartValue[0];    
}

function SetValueOnRequestTypeChange(objdrpReqType, txtNewModelDesc,txtFertCode,lblSelectPart,txtDetails,txtModelID,hdnNewModelDescription) 
{
    var ParentCtrlID,NewModelDesc,FertCode,SelectPart,Details;
        ParentCtrlID=objdrpReqType.id.substring(0, objdrpReqType.id.lastIndexOf("_"));
        NewModelDesc = document.getElementById(ParentCtrlID+"_"+ txtNewModelDesc);
        FertCode=document.getElementById(ParentCtrlID+"_"+ txtFertCode);
        SelectPart=document.getElementById(ParentCtrlID+"_"+ lblSelectPart);
        Details=document.getElementById(ParentCtrlID+"_"+ txtDetails);
        ModelID=document.getElementById(ParentCtrlID+"_"+ txtModelID);
        NewModelDescription=document.getElementById(ParentCtrlID+"_"+ hdnNewModelDescription);
    if(objdrpReqType.options[objdrpReqType.selectedIndex].value=="0")
    {
        NewModelDesc.disabled=true;
        FertCode.disabled=true;
        SelectPart.disabled=true;
        Details.disabled=true;
        FertCode.value="";
        Details.value="";
        NewModelDesc.value="";
        ModelID.value="";
        NewModelDescription.value="";
    }
    else if(objdrpReqType.options[objdrpReqType.selectedIndex].value=="1")
    {
        NewModelDesc.disabled=true;
        FertCode.disabled=true;
        SelectPart.disabled=false;
        Details.disabled=false;
        FertCode.value="";
        Details.value="";
        NewModelDesc.value="";
        ModelID.value="";
        NewModelDescription.value="";
    }
    else if(objdrpReqType.options[objdrpReqType.selectedIndex].value=="2")
    {
        NewModelDesc.disabled=false;
        FertCode.disabled=true;
        SelectPart.disabled=true;
        Details.disabled=false;
        FertCode.value="";
        Details.value="";
        NewModelDesc.value="";
        ModelID.value="";
        NewModelDescription.value="";
    }
}

//###Common
// When user Click to delete the record then reduce the amount 
function SelectDeleteCheckboxCommon(ObjChkDelete) {

    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            return true;
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.parentNode.style.backgroundColor = 'white';        
    }   
}
// To Get Part Id which are previously selected by user.
function GetPreviousSelectedFertID(objNewPartLabel) {
    var objRow;
    var PartIds = "";
    var PartId = "";
    var lblPartId;
    // get grid object
    var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
    if (objGrid == null) return PartIds;

    for (var i = 1; i < objGrid.rows.length; i++) {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Lable of Part ID 
        lblPartId = objGrid.rows[i].children[5].childNodes[0];
        //Get PartId;
        PartId = lblPartId.value
        if (PartId != "0" && PartId != "" && PartId != null) {
            PartIds = PartIds + PartId + ",";
        }
    }
    PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

    return PartIds;
}