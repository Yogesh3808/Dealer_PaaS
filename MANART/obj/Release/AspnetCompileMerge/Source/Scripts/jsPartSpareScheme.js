//when user select New model then display textbox to enter model description 
var bSetCheckBox;
var bDisplayConfirmAll;
var bDisplayDeselectAll;

//Set Value Of the Fertcode on Model Change
function SetValueOnModelChange(ObjModel,txtboxId,FertCodeID) {
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
            SetFertCode(ObjModel, FertCodeID, false);
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
                SetFertCode(ObjModel, FertCodeID, true);
        }
        else {
            return false;
        }
    }
}
function SetFertCode(ObjModel,FertCodeID,bDisplay)
{
    var ParentCtrlID;
    var objFertCodeControl;
    ParentCtrlID=ObjModel.id.substring(0, ObjModel.id.lastIndexOf("_"));    
    objFertCodeControl= document.getElementById(ParentCtrlID+"_"+ FertCodeID);
    objFertCodeControl.selectedIndex=ObjModel.selectedIndex;    
    if (bDisplay == false)
    {
        objFertCodeControl.style.display="none";
    }
    else
    {
        objFertCodeControl.style.display="";
    }
    return true;
}
function SetValueOnFertCodeChange(ObjFertCode, txtboxId, dModelID)
{
    var ParentCtrlID;
    var objModelControl;
    ParentCtrlID=ObjFertCode.id.substring(0, ObjFertCode.id.lastIndexOf("_"));
    objModelControl = document.getElementById(ParentCtrlID + "_" + dModelID);
    objModelControl.selectedIndex = ObjFertCode.selectedIndex;
    if (SetValueOnModelChange(objModelControl, txtboxId, null) == false) {
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

// When user Click on Cancel then clear the value of row
function ClearRowValueforModel(event,objCancelControl)
{
    var objRow=objCancelControl.parentNode.parentNode.childNodes;    
    var ObjCell;
    var icntOfChildren;
    var bUseChildren=true;
    for (var i=0;i< objRow.length;i++)
    {   
        icntOfChildren = objRow[i].children.length;
        bUseChildren = true;
        if (icntOfChildren == 0)
        {
            icntOfChildren =objRow[i].childNodes.length;
            bUseChildren =false; 
        }
        for (var j=0;j< icntOfChildren;j++)
        {   
            if(bUseChildren==true)
            {
                ObjCell =objRow[i].children[j];        
            }
            else
            {
                ObjCell =objRow[i].childNodes[j];        
            }
            
            if (ObjCell.type=="text")
            {
                ObjCell.value='';            
            }        
            else if (ObjCell.type=="textarea")
            {
                ObjCell.value='';            
                ObjCell.style.display='none';
            }
            else if (ObjCell.type=="select-one")
            {
                ObjCell.selectedIndex=0;
                ObjCell.style.display='';
            }
        }
        
    }
}



////when user set model Code for New model 
function OnModelSetToNew(ObjModel,FertCodeID)
{    
   var sSelecedValue= ObjModel.options[ObjModel.selectedIndex].text;       
   if ( sSelecedValue =="NEW")
   {   
    alert('Please select the model.');
   }
   if (CheckModelAlreadyExist(ObjModel)==true)
    {
        SetFertCode(ObjModel,FertCodeID);
    }
    
}
// when user select accept check box
function SelectAcceptCheckbox(ObjChkAccept,objModelID)
{
   var sSelecedValue;
   var ParentCtrlID;
   var ObjModel = null;
   if (objModelID != null) {
       ParentCtrlID = ObjChkAccept.id.substring(0, ObjChkAccept.id.lastIndexOf("_"));
       ObjModel = document.getElementById(ParentCtrlID + "_" + objModelID);
   }
    if (ObjModel != null) 
    {
        if (ObjModel.selectedIndex == 0) {
            alert('Please select the Model Code at line No. ' + ObjChkAccept.parentNode.parentNode.parentNode.rowIndex);
            ObjChkAccept.checked = false;
            bSetCheckBox = false;
            return false;
        }
        sSelecedValue= ObjModel.options[ObjModel.selectedIndex].text;
        if ( sSelecedValue =="NEW")
         {
             alert('Please select FERT Code at line No. ' + ObjChkAccept.parentNode.parentNode.parentNode.rowIndex);
               ObjChkAccept.checked = false;
               bSetCheckBox =false;               
               return false;	
         }
     }
     var iCellIndex = 0;
     iCellIndex = ObjChkAccept.parentNode.parentNode.cellIndex;
     if (iCellIndex == null) {
         iCellIndex = ObjChkAccept.parentNode.cellIndex;
         iCellIndex = iCellIndex + 1;
         var objConfirm = ObjChkAccept.parentNode.parentNode.cells[iCellIndex].children[0];
         //ObjChkAccept.parentNode.parentNode.parentNode.cells[iCellIndex].childNodes[0].children[0].checked
         objConfirm.checked = false;
     }
     else {
         iCellIndex = iCellIndex + 1;
         var objConfirm = ObjChkAccept.parentNode.parentNode.parentNode.cells[iCellIndex].childNodes[0].children[0];
         if (objConfirm == null) objConfirm = ObjChkAccept.parentNode.cells[iCellIndex].childNodes[0].children[0];         
         objConfirm.checked = false;
     }
     // reset Confirm Check box
     
}
// when User Select Chkall checkbox for Accept
function SelectAcceptCheckboxAll(ChkAll,ChkConfirmAllId) {
    var ChkAccAllStatus = ChkAll.checked;
    var ObjGrid = ChkAll.parentNode.parentNode.parentNode.parentNode;
    var ColNo = ChkAll.parentNode.cellIndex;

    if (ChkAll.checked == false) 
    {
        if (confirm("Are you sure you want to deselect all the record?") == true) {

        }
        else {
            return false;
        }
        var ParentCtrlID;
        var objChkConfirmAll;
        ParentCtrlID = ChkAll.id.substring(0, ChkAll.id.lastIndexOf("_"));
        objChkConfirmAll = document.getElementById(ParentCtrlID + "_" + ChkConfirmAllId);
        if (objChkConfirmAll.checked != ChkAccAllStatus) {
            //objChkConfirmAll.click();
            bDisplayDeselectAll = false;
            objChkConfirmAll.checked = ChkAccAllStatus;
            SelectConfirmCheckboxAll(objChkConfirmAll, false);            
        }
    }
    for (i = 1; i < ObjGrid.rows.length; i++)
     {
         var ChkBox = ObjGrid.rows[i].children[ColNo].children[0].childNodes[0];
         if (ChkBox == null) ChkBox = ObjGrid.rows[i].children[ColNo].children[0];
        if (ChkBox != null) {
            if (ChkBox.type == "checkbox" && ChkBox.id != ChkAll.id) {
                bSetCheckBox = true;
                if (ChkBox.checked != ChkAccAllStatus) 
                {                    
                    ChkBox.click();
                }
                if (bSetCheckBox == false) {
                    bDisplayConfirmAll = true;
                    ChkBox.checked = false;
                    return false;
                }
            }
        } 
    }
   
}
// when user select confirm check box
function SelectConfirmCheckbox(ObjChkConfirm,chkAcceptID)
{        
    var txtStatus;
    if(ObjChkConfirm.checked)       
        {            
            var ParentCtrlID;
            var ObjchkAccept;        
            ParentCtrlID=ObjChkConfirm.id.substring(0, ObjChkConfirm.id.lastIndexOf("_"));    
            ObjchkAccept= document.getElementById(ParentCtrlID+"_"+ chkAcceptID);
            if (ObjchkAccept.checked ==false)
            {
                alert('Please first click on accept');
                ObjChkConfirm.checked = false;
                bSetCheckBox =false;                
                return false;			
            }
            if (bDisplayConfirmAll  ==true)
            {                
                if(confirm("Are you sure you want to Confirm the record?")==true)
                    {                        
		                return true;
		            }
	            else
	                {	                    
		                return false;			
		            }		
            }       
            else
            {
                return true;
            }    
        }
    else
        {    
            if (bDisplayDeselectAll ==true)
            {                
                if(confirm("Are you sure you want to deselect the record?")==true)
                    {                        
		                return true;
		            }
	            else
	                {	                 
		                return false;			
		            }		
             }   
             else
                {
                    return true;
                }           
        }        
}
// when User Select Chkall checkbox for Confirm
function SelectConfirmCheckboxAll(ChkAll,bDisplayMsg)
{
    var ChkAllStatus = ChkAll.checked;
    var ObjGrid=ChkAll.parentNode.parentNode.parentNode.parentNode;
    var ColNo = ChkAll.parentNode.cellIndex;
    if (bDisplayMsg == null) bDisplayMsg = true;

    if (ChkAll.checked) 
    {
        if (bDisplayMsg == true) //Check if message have to display
        {
            if (confirm("Are you sure you want to confirm all the record?") == true) {
                bDisplayConfirmAll = false;
            }
            else {
                bDisplayConfirmAll = false;
                ChkAll.checked = false;
                return false;
            }
        }
     }
     else {
         if (bDisplayMsg == true) //Check if message have to display       
         {
             if (confirm("Are you sure you want to deselect all the record?") == true) {
                 bDisplayDeselectAll = false;
             }
             else {
                 bDisplayDeselectAll = false;
                 ChkAll.checked = true;
                 return false;
             }
         }
     }
    for(i=1;i<ObjGrid.rows.length;i++) {

        var ChkBox = ObjGrid.rows[i].children[ColNo].children[0].childNodes[0];
        if (ChkBox == null) ChkBox = ObjGrid.rows[i].children[ColNo].childNodes[0];
        if (ChkBox == null) return;
        if(ChkBox.type=="checkbox" && ChkBox.id!=ChkAll.id)
        {
            bSetCheckBox =true;
            if (ChkBox.checked != ChkAllStatus)
            ChkBox.click();                       
            if (bSetCheckBox== false)
            {      
                bDisplayConfirmAll =true;     
                bDisplayDeselectAll=true;
                ChkBox.checked  =false;              
                return false;         
            }
        }        
     }
     bDisplayConfirmAll =true;     
     bDisplayDeselectAll=true;
}



// To Check RFP Type Is Selected
function CheckRFPTypeIsSelected() {
    var objRFPTypeCombo = document.getElementById('ContentPlaceHolder1_drpRFPSelection');
    if (objRFPTypeCombo == null) return;

    if (objRFPTypeCombo.selectedIndex == 0) {
        alert('Please first select the RFP type !');
        return false;
    }
    else {
        return true;
    }
}


//Check Shipment Terms is FOB Then Do validation
function CheckShipmentTermsValidation(ObjShipTermControl) 
{
    if (ObjShipTermControl.selectedIndex == 0) return;
    var sShipmentTerms = ObjShipTermControl.options[ObjShipTermControl.selectedIndex].text;
    var ObjControl;
    if (sShipmentTerms == "FOB")
     {

        ObjControl = window.document.getElementById("ContentPlaceHolder1_drpShippingLineNominationRequired");
        if (ObjControl != null) ObjControl.disabled = false;
        ObjControl = window.document.getElementById("ContentPlaceHolder1_drpNominatedAgency");
        if (ObjControl != null) ObjControl.disabled = false;        
    }
    else 
    {
        ObjControl = window.document.getElementById("ContentPlaceHolder1_drpShippingLineNominationRequired");
        if (ObjControl != null) {
            ObjControl.disabled = true;
            ObjControl.selectedIndex = 1;
        }
        ObjControl = window.document.getElementById("ContentPlaceHolder1_drpNominatedAgency");
        if (ObjControl != null)
         {
             ObjControl.disabled = true;
            ObjControl.selectedIndex = 0;
        }
    }
}

///////////////@@@@@@Part Function@@@@@@@@@@
// When user Click on Cancel then clear the value of row
function ClearRowValueForPart(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var i = 1;

    //objAddNewControl.style.display="none";
    //Set PartId;
    objRow[1].childNodes[0].value = '';

    //SetPartNo
    //objRow[2].children[0].style.display = "";
    objRow[2].children[0].value = "";    
    //objRow[2].children[0].style.display = "none";
    
    
    //SetPartName
    objRow[3].childNodes[0].value = '';

    //Set MOQ
    objRow[4].childNodes[0].value = '';
    
    //SetQuantity
    objRow[5].childNodes[0].value = '1';
    
    //SetFoBRate        
    objRow[6].childNodes[0].value = '';

    //Total
    objRow[7].childNodes[0].value = '';
    
    //SetNewLabel Display    
    //objRow[].children[1].style.display = "none";
}
//Calculate Total For Part Details
function CalculateTotal(event, ObjQtyControl) {
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
        //GetFoBRate                   
        var FOBRate = dGetValue(objRow[6].childNodes[0].value);
        var Total = dGetValue(ObjQtyControl.value) * FOBRate;
        //SetNewLabel Display
        objRow[7].childNodes[0].value = RoundupValue(Total);
    }
}
//SetPartDetails
function SetPartDetails(objAddNewControl, PartValue) {
    //objLabel.parentNode.childNodes[1].value=Value[1];
    if (PartValue.length == 0) return;
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    objAddNewControl.style.display = "none";
    //Set PartId;
    objRow[1].childNodes[0].value = PartValue[0];
    
    //SetPartNo
    objRow[2].children[1].value = PartValue[1];
    objRow[2].children[1].style.display = "";
        
    
    //SetPartName
    objRow[3].childNodes[0].value = PartValue[2];           

    //Set Qty
    objRow[5].childNodes[0].value = '1';        

    //SetFoBRate
    objRow[6].childNodes[0].value = PartValue[3];


    //Set MOQ
    objRow[4].childNodes[0].value = PartValue[5];
        
    //Total
    objRow[7].childNodes[0].value = PartValue[3];
    //objRow[i].childNodes[0].readOnly = true;    
}
// Function To Check FileName is Selected or Not
function CheckBeforeUpload(objbutton, FileUploadID) {
    var ParentCtrlID;
    var objFileUpload;
    ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
    objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
    var sDealerCode = document.getElementById("ContentPlaceHolder1_Location_txtDealerCode");
    var filename = objFileUpload.value;
    var splfilename1=''
    var objDate=new Date();

    if (filename == "") {
        alert('Please select the file.');
        return false;
    }
    else {
        var splfilename = filename.split('\\');
        if (splfilename.length > 0)
            splfilename1 = splfilename[splfilename.length - 1].split('_');
    }
    if (filename.search('xls') == -1) {
        alert('File is not in excel format.');
        return false;
    }
    if (filename.search('_RFP_PartDetails_') == -1) {
        alert('File name is not in given format.');
        return false;
    }
    if (splfilename1.length > 1)
        if (sDealerCode.value != splfilename1[0]) {
        alert('Invalid File Name.');
        return false;
    }
    else if (filename.search(objDate.getFullYear().toString() + pad((objDate.getMonth() + 1),2) + pad(objDate.getDate(),2))==-1) {
        alert('File Name must contain todays date.');
        return false;
    }

}

function pad(number, length) {
    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str; 
  }

  function HeaderClick(CheckBox, TargetChildControl)
{
   //Get target base & child control.
    var TargetBaseControl = document.getElementById('ContentPlaceHolder1_PartGrid');   

   //Get all the control of the type INPUT in the base control.
   var Inputs = TargetBaseControl.getElementsByTagName("input");

   //Checked/Unchecked all the checkBoxes in side the GridView.
   for(var n = 0; n < Inputs.length; ++n)
      if(Inputs[n].type == 'checkbox' && 
                Inputs[n].id.indexOf(TargetChildControl,0) >= 0)
         Inputs[n].checked = CheckBox.checked;
   
}

function ChildClick(CheckBox, TargetBaseControl)
{
    //get target control.
    var TotalChkBx = 0;
    var Counter = 0;
    var PartGrid = document.getElementById('ContentPlaceHolder1_PartGrid');
    var Inputs = PartGrid.getElementsByTagName("input");
    var HeaderCheckBox = null;
    for (var iHdrCnt = 0; iHdrCnt < Inputs.length; ++iHdrCnt)
        if (Inputs[iHdrCnt].type == 'checkbox' && Inputs[iHdrCnt].id.indexOf(TargetBaseControl, 0) >= 0)
            HeaderCheckBox = document.getElementById(Inputs[iHdrCnt].id);
        if (CheckBox.checked) {
            for (var iDtlsCnt = 0; iDtlsCnt < Inputs.length; ++iDtlsCnt)
                if (Inputs[iDtlsCnt].type == 'checkbox' && Inputs[iDtlsCnt].id.indexOf(TargetBaseControl, 0) == -1 && !Inputs[iDtlsCnt].checked)
                    TotalChkBx = TotalChkBx + 1;
                    
                //Change state of the header CheckBox.
                if(TotalChkBx > 0)
                    HeaderCheckBox.checked = false;
                else 
                    HeaderCheckBox.checked = true;
        }
        else {
            //Change state of the header CheckBox.           
                HeaderCheckBox.checked = false;
            
        }
}

