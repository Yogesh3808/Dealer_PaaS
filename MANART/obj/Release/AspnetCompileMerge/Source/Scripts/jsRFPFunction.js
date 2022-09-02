//when user select New model then display textbox to enter model description 
var bSetCheckBox;
var bDisplayConfirmAll;
var bDisplayDeselectAll;

//Set Value Of the Fertcode on Model Change
function SetValueOnModelChange(ObjModel, txtboxId, FertCodeID, dModelRtID, dQtyID, dTotalID) {
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
            SetFertCode(ObjModel, FertCodeID, false, dModelRtID, dQtyID, dTotalID);
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
                SetFertCode(ObjModel, FertCodeID, true, dModelRtID, dQtyID, dTotalID);
        }
        else {
            return false;
        }
    }
}
function SetFertCode(ObjModel, FertCodeID, bDisplay, dModelRtID, dQtyID, dTotalID)
{
    var ParentCtrlID;
    var objFertCodeControl;    
    ParentCtrlID=ObjModel.id.substring(0, ObjModel.id.lastIndexOf("_"));    
    objFertCodeControl= document.getElementById(ParentCtrlID+"_"+ FertCodeID);
    objFertCodeControl.selectedIndex = ObjModel.selectedIndex;
    
    var objModelRtControl;
    objModelRtControl = document.getElementById(ParentCtrlID + "_" + dModelRtID);
    objModelRtControl.selectedIndex = ObjModel.selectedIndex;


    var objQty;
    objQty = document.getElementById(ParentCtrlID + "_" + dQtyID);

    var objTotal;
    objTotal = document.getElementById(ParentCtrlID + "_" + dTotalID);
    objTotal.value = RoundupValue(dGetValue(objModelRtControl[objModelRtControl.selectedIndex].innerText) * dGetValue(objQty.value));
    CalulateRFPVehicleGranTotal();
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
function SetValueOnFertCodeChange(ObjFertCode, txtboxId, dModelID, dModelRtID, dQtyID, dTotalID)
{
    var ParentCtrlID;
    var objModelControl;
    ParentCtrlID=ObjFertCode.id.substring(0, ObjFertCode.id.lastIndexOf("_"));
    objModelControl = document.getElementById(ParentCtrlID + "_" + dModelID);
    objModelControl.selectedIndex = ObjFertCode.selectedIndex;
    
    var objModelRtControl;
    objModelRtControl = document.getElementById(ParentCtrlID + "_" + dModelRtID);
    objModelRtControl.selectedIndex = ObjFertCode.selectedIndex;

    var objQty;
    objQty = document.getElementById(ParentCtrlID + "_" + dQtyID);

    var objTotal;
    objTotal = document.getElementById(ParentCtrlID + "_" + dTotalID);
    objTotal.value = RoundupValue(dGetValue(objModelRtControl[objModelRtControl.selectedIndex].innerText) * dGetValue(objQty.value));
    CalulateRFPVehicleGranTotal();
    if (SetValueOnModelChange(objModelControl, txtboxId, null, dModelRtID, dQtyID, dTotalID) == false) {
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

function CalculateRFPVehicleTotal(objQty, dModelRtID, dTotalID) {
    if (CheckQtyValidation(event, objQty, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {

        var ParentCtrlID;        
        var objModelRtControl;
        ParentCtrlID = objQty.id.substring(0, objQty.id.lastIndexOf("_"));
        
        objModelRtControl = document.getElementById(ParentCtrlID + "_" + dModelRtID);
        objTotal = document.getElementById(ParentCtrlID + "_" + dTotalID);

        objTotal.value = RoundupValue(dGetValue(objModelRtControl[objModelRtControl.selectedIndex].innerText) * dGetValue(objQty.value));
        CalulateRFPVehicleGranTotal();
    }
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
        CalulateRFPVehicleGranTotal();
        
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
    //CalulateRFPPartGranTotal();
}


function ClearRowValueForRFPPart(event, objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var i = 1;

    objCancelControl.style.display = "none";
    //Set PartId;
    objRow[1].childNodes[0].value = '';

    //SetPartNo
   
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

   // objRow[2].children[0].style.display = "none";
   // objRow[2].children[1].style.display = "";
    //SetNewLabel Display
    //objRow[].children[1].style.display = "none";
    CalulateRFPPartGranTotal();
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

function CalculateRFPPartTotal(event, ObjQtyControl) {
    if (CheckTextboxValueForNumeric(event, ObjQtyControl, false) == false) {
        //ObjControl.focus(); 
        return;
    }
    else {
        var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

        var MOQ = dGetValue(objRow[4].childNodes[0].value);
        var Qty = dGetValue(ObjQtyControl.value);

        if (MOQ != 0 && (Qty % MOQ) != 0) {
            if (Qty / MOQ != 0) {
                ObjQtyControl.value = (parseInt(Qty / MOQ) + 1) * MOQ
            }
        }
        
        //GetFoBRate                   
        var FOBRate = dGetValue(objRow[6].childNodes[0].value);
        var Total = dGetValue(ObjQtyControl.value) * FOBRate;
        //SetNewLabel Display
        objRow[7].childNodes[0].value = RoundupValue(Total);
        CalulateRFPPartGranTotal()
        
    }
}

function CalulateRFPPartGranTotal() {
    var txtTotalQty = document.getElementById("ContentPlaceHolder1_txtTotalQty");
    var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var qty, Rate;
    var TotalRate = 0;
    var totalQtypart = 0;
    var sPArtName = "";
    var CountRow = objGrid.rows.length;
    //Sujata 19022011
    //for (var i = 1; i < CountRow - 1; i++)
    
    //Shyamal 02062012
    for (var i = 0; i < CountRow; i++)
    //Sujata 19022011
     {
         //Shyamal 02062012
         if (objGrid.rows[i].className.indexOf('RowStyle')>0) {
             qty = objGrid.rows[i].childNodes[5].children[0].value;
             Rate = objGrid.rows[i].childNodes[6].children[0].value;
             sPArtName = objGrid.rows[i].childNodes[3].children[0].value;
             if (sPArtName != "") {
                 TotalRate =dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
                 totalQtypart = dGetValue(totalQtypart) + dGetValue(qty);
             }
         }
    }
    txtTotalQty.value = totalQtypart;
    //Sujata 19022011
    //txtTotal.value = TotalRate;
    //Shyamal 02062012,Added toFixed(2)
    txtTotal.value = parseFloat(TotalRate).toFixed(2);
    //Shyamal 02062012
    //Sujata 19022011
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


}
function SetValueOnBodyTypechange(ObjBodyType, dModelID, dModelCodeID, dQtyID) {
   // if (ObjBodyType.selectedIndex == -1) return;
    var sSelecedValue = ObjBodyType.options[ObjBodyType.selectedIndex].value;
    var objGrid = document.getElementById("ContentPlaceHolder1_DetailsGrid");
    var ParentCtrlID;
    var objtxtControl;
    if (sSelecedValue == 1) {
        var objModelControl;
        ParentCtrlID = ObjBodyType.id.substring(0, ObjBodyType.id.lastIndexOf("_"));
        objModelControl = document.getElementById(ParentCtrlID + "_" + dModelID);
        objModelControl.disabled = false;

        var objModelCodeControl;
        objModelCodeControl = document.getElementById(ParentCtrlID + "_" + dModelCodeID);
        objModelCodeControl.disabled = false;

        var objModelQtyControl;
        objModelQtyControl = document.getElementById(ParentCtrlID + "_" + dQtyID);
        objModelQtyControl.disabled = false;
    }
    else {
        var objModelControl;
        ParentCtrlID = ObjBodyType.id.substring(0, ObjBodyType.id.lastIndexOf("_"));
        objModelControl = document.getElementById(ParentCtrlID + "_" + dModelID);
        objModelControl.disabled = true;

        var objModelCodeControl;
        objModelCodeControl = document.getElementById(ParentCtrlID + "_" + dModelCodeID);
        objModelCodeControl.disabled = true;

        var objModelQtyControl;
        objModelQtyControl = document.getElementById(ParentCtrlID + "_" + dQtyID);
        objModelQtyControl.disabled = true;
    }
     
      return;
    }


    function CalulateRFPVehicleGranTotal() {
        var txtGrandTotal = document.getElementById("ContentPlaceHolder1_txtGrandTotal");
        //var txtTotal = document.getElementById("ContentPlaceHolder1_txtTotal");
        var objGrid = document.getElementById("ContentPlaceHolder1_DetailsGrid");
        var qty, Rate;
        var TotalRate = 0;
        var totalQtypart = 0;
        var sPArtName = "";
        var Total = 0;
        var CountRow = objGrid.rows.length;
        //Sujata 19022011
        //for (var i = 1; i < CountRow - 1; i++)

        //Shyamal 02062012
        for (var i = 0; i < CountRow; i++)
        //Sujata 19022011
        {
            //Shyamal 02062012
            if (objGrid.rows[i].className.indexOf('RowStyle') > 0) {

                qty = objGrid.rows[i].childNodes[4].children[0].value;
                // Rate = objGrid.rows[i].childNodes[5].childNodes[0].value
                Total = objGrid.rows[i].childNodes[6].children[0].value;
                if (qty > 0) {
                    Rate = dGetValue(Total) / dGetValue(qty)
                }
                else {
                    Rate = 0;
                }
               TotalRate = dGetValue(TotalRate) + (dGetValue(qty) * dGetValue(Rate))
               
            }
        }
        txtGrandTotal.value = parseFloat(TotalRate).toFixed(2);

    }
    function ClearRowValueForRFPModel(event, objCancelControl) {
        var objRow = objCancelControl.parentNode.parentNode.childNodes;
        var i = 1;

        //objCancelControl.style.display = "none";
        //Set ModelId;
        objRow[0].childNodes[0].value = '';

        //SetBodyType

        objRow[1].children[0].selectedIndex = 0;
        //objRow[2].children[0].style.display = "none";


        //SetModelName
        objRow[2].children[0].selectedIndex = 0;
        objRow[2].children[0].disabled = true;


        //SetFertCode
        objRow[3].children[0].selectedIndex = 0;
        objRow[3].children[0].disabled = true;


        //SetQuantity
        objRow[4].childNodes[0].value = '1';

        //SetFoBRate
        objRow[5].children[0].selectedIndex = 0;
        

        //Total
        objRow[6].childNodes[0].value = 0;

        // objRow[2].children[0].style.display = "none";
        // objRow[2].children[1].style.display = "";
        //SetNewLabel Display
        //objRow[].children[1].style.display = "none";
        CalulateRFPVehicleGranTotal();
    }


