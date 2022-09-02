var bUsed = null;
//************Complaint**************
//When User Change Complaint
function OnComplaintValueChange(eve, ObjCombo, txtboxId) {
    if (OnComboValueChange(ObjCombo, txtboxId) == false)
     {

    }
    if (ObjCombo.options[ObjCombo.selectedIndex].text != "NEW") {
        CheckComplaintSelected(eve, ObjCombo);
    }
    else
        return true;
}
//Check Complaint value is select 
function CheckComplaintSelected(eve, objcontrol)
{
    ////debugger;
     if (CheckForComboValue(eve, objcontrol, false) == true) {
         if (CheckComboValueAlreadySelectInGrid(objcontrol) == false)
             return false;
         if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
             return false;
         SetComplaintRecordCount();
     }
     else {
         ClearRowValueForComplaint(null,objcontrol);
     }
 }
 //Check Complaint Is already Used in Grid
function CheckComplaintAlreadyUsedInGrid(event, Objcontrol) {
    ////debugger;
     if (CheckTextValueAlreadyUsedInGrid(event, Objcontrol) == true) {
         //var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[0].innerText);
         var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[1].innerText);
    var ObjRecord;
    var objGrid = Objcontrol.parentNode.parentNode.parentNode;
    for (i = 1; i < objGrid.children.length; i++) {
        //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
        ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

        if (i == iRowOfSelectControl) {
            if (Objcontrol.value == "") {
                alert("Please Enter Complaint Description")
                Objcontrol.focus();
                return false;
            }
        }
    }
         SetComplaintRecordCount();
     }
 }
 // Claer the Row Value of the selected Row
 function ClearRowValueForComplaint(event, ObjControl) {
     ClearRowValue(event, ObjControl);
     SetComplaintRecordCount();
 }
 // Set Total Complaint Record Count
 function SetComplaintRecordCount() {
     var ObjGrid;
     var iRecordCnt = 0;
     
     ObjGrid = document.getElementById("ContentPlaceHolder1_ComplaintsGrid");
     if (ObjGrid == null) return;
     for (var i = 1; i < ObjGrid.rows.length; i++) {
         if (ObjGrid.rows[i].cells[1].children[0].selectedIndex != 0) 
         {
             if (ObjGrid.rows[i].cells[1].children[0].value == "9999") {
                 if (ObjGrid.rows[i].cells[1].children[1].innerText != null && ObjGrid.rows[i].cells[1].children[1].innerText !="") {
                     iRecordCnt = iRecordCnt + 1;
                 }
             }
             else {
                 iRecordCnt = iRecordCnt + 1;
              }
         }
     }
     // To calculate Complaint Count
     var ObjComplaintCount = document.getElementById("ContentPlaceHolder1_lblComplaintsRecCnt");
     if (ObjComplaintCount != null) {
         ObjComplaintCount.innerText = iRecordCnt;         
     }
 }
 
 //*********************Action Taken***********************
 //When User Change Action
 function OnActionValueChange(eve, ObjCombo, txtboxId) {
     ////debugger;
     if (OnComboValueChange(ObjCombo, txtboxId) == false) {

     }
     if (ObjCombo.options[ObjCombo.selectedIndex].text != "NEW") {
         CheckActionSelected(eve, ObjCombo);
     }
     else
         return true;
     //if (CheckTextValueAlreadyUsedInGrid(event, ObjCombo) == true) {
     //    SetActionRecordCount();
     //}
 }

 // Set Total Investigations Record Count
 function SetActionRecordCount() {
     var ObjGrid;
     var iRecordCnt = 0;

     ObjGrid = document.getElementById("ContentPlaceHolder1_ActionsGrid");
     if (ObjGrid == null) return;
     for (var i = 1; i < ObjGrid.rows.length; i++) {
         if (ObjGrid.rows[i].cells[1].children[0].selectedIndex != 0) {
             if (ObjGrid.rows[i].cells[1].children[0].value == "9999") {
                 if (ObjGrid.rows[i].cells[1].children[1].innerText != null && ObjGrid.rows[i].cells[1].children[1].innerText != "") {
                     iRecordCnt = iRecordCnt + 1;
                 }
             }
             else {
                 iRecordCnt = iRecordCnt + 1;
             }
         }
     }
     // To calculate Investigation Count
     var ObjActionCount = document.getElementById("ContentPlaceHolder1_lblActionsRecCnt");
     if (ObjActionCount != null) {
         ObjActionCount.innerText = iRecordCnt;
     }
 }
 //Check Complaint value is select
 function CheckActionSelected(eve, objcontrol) {
     //debugger;
     if (CheckForComboValue(eve, objcontrol, false) == true) {
         if (CheckComboValueAlreadySelectInGrid(objcontrol) == false)
             return false;
         //if (CheckActionAlreadyUsedInGrid(eve,objcontrol) == false)
         //    return false;
         if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
             return false;
         SetActionRecordCount();
     }

 }

 //Check Action Is already Used in Grid
 function CheckActionAlreadyUsedInGrid(event, Objcontrol) {
     ////debugger;
     if (CheckTextValueAlreadyUsedInGrid(event, Objcontrol) == true) {
         //var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[0].innerText);
         var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[1].innerText);
         var ObjRecord;
         var objGrid = Objcontrol.parentNode.parentNode.parentNode;
         for (i = 1; i < objGrid.children.length; i++) {
             //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
             ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

             if (i == iRowOfSelectControl) {
                 if (Objcontrol.value == "") {
                     alert("Please Enter Action Taken Description")
                     Objcontrol.focus();
                     return false;
                 }
             }
         }         
         SetActionRecordCount()
     }
 }
 // Claer the Row Value of the selected Row
 function ClearRowValueForAction(event, ObjControl) {
     ClearRowValue(event, ObjControl);
     SetActionRecordCount();
 } 
 //*********************Action Taken***********************
 
 //*******************Investigations***********************
 //When User Change Investigation
 function OnInvestigationValueChange(eve, ObjCombo, txtboxId)
 {
     //debugger;
     if (OnComboValueChange(ObjCombo, txtboxId) == false)
      {

      }
     if (ObjCombo.options[ObjCombo.selectedIndex].text != "NEW") {
         CheckInvestigationSelected(eve, ObjCombo);
     }
     else
         return true;
      //if (CheckTextValueAlreadyUsedInGrid(event, ObjCombo) == true) {
      //    SetInvestigationRecordCount();
      //}
  }
  
 //Check Complaint value is select
 function CheckInvestigationSelected(eve, objcontrol) {
     //debugger;
     if (CheckForComboValue(eve, objcontrol, false) == true) {
         if (CheckComboValueAlreadySelectInGrid(objcontrol) == false)
             return false;
         if (CheckComboValueAlreadyUsedInGrid(objcontrol) == false)
             return false;
         SetInvestigationRecordCount();
     }
    
 }

 //Check Investigations Is already Used in Grid
 function CheckInvestigationAlreadyUsedInGrid(event, Objcontrol) {
     //debugger;
     if (CheckTextValueAlreadyUsedInGrid(event, Objcontrol) == true) {
         //var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[0].innerText);
         var iRowOfSelectControl = parseInt(Objcontrol.parentNode.parentNode.childNodes[1].innerText);
         var ObjRecord;
         var objGrid = Objcontrol.parentNode.parentNode.parentNode;
         for (i = 1; i < objGrid.children.length; i++) {
             //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
             ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

             if (i == iRowOfSelectControl) {
                 if (Objcontrol.value == "") {
                     alert("Please Enter Investigation Description")
                     Objcontrol.focus();
                     return false;
                 }
             }
         }
         SetInvestigationRecordCount();
     }
 }
 // Claer the Row Value of the selected Row
 function ClearRowValueForInvestigation(event, ObjControl) {
     ClearRowValue(event, ObjControl);
     SetInvestigationRecordCount();
 }
 // Set Total Investigations Record Count
 function SetInvestigationRecordCount() {
     var ObjGrid;
     var iRecordCnt = 0;

     ObjGrid = document.getElementById("ContentPlaceHolder1_InvestigationsGrid");
     if (ObjGrid == null) return;
     for (var i = 1; i < ObjGrid.rows.length; i++) {
         if (ObjGrid.rows[i].cells[1].children[0].selectedIndex != 0) {
             if (ObjGrid.rows[i].cells[1].children[0].value == "9999") {
                 if (ObjGrid.rows[i].cells[1].children[1].innerText != null && ObjGrid.rows[i].cells[1].children[1].innerText != "") {
                     iRecordCnt = iRecordCnt + 1;
                 }
             }
             else {
                 iRecordCnt = iRecordCnt + 1;
             }
         }
     }
     // To calculate Investigation Count
     var ObjInvestigationCount = document.getElementById("ContentPlaceHolder1_lblInvestigationsRecCnt");
     if (ObjInvestigationCount != null) {
         ObjInvestigationCount.innerText = iRecordCnt;
     }
 }

//***************Common Function To Warranty****************
//To Check Records are entered or selected in a row
function CheckRowValue(event, objAddNewControl) {
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    ////debugger;
    var ObjCell;
    //for (var i = 0; i < objRow.length; i++) {
    for (var i = 1; i < objRow.length; i++) {
        //ObjCell = objRow[i].childNodes[0];
        ObjCell = objRow[i].childNodes[1];
        if (ObjCell.style.display == "none") {
            ObjCell = objRow[i].childNodes[3];
        }        
        if (objRow[i].style.display != "none") {
            //if (ObjCell.type == "text") {
            if (ObjCell.type == "textarea") {
                //if (ObjCell.isDisabled == false) {
                if (ObjCell.value == "") {
                    alert("Please enter the record");
                    ObjCell.focus();
                    return false;
                }
                //}
            }
            else if (ObjCell.type == "select-one") {
                if (ObjCell.style.display != "none") {
                    //if (ObjCell.isDisabled == false) {
                    if (ObjCell.value == "0") {
                        alert("Please select the record");
                        ObjCell.focus();
                        return false;
                    }
                    //}
                }
            }
        }
    }
}





// When user Click on Cancel then clear the value of row
function ClearRowValue(event, objCancelControl) {
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
                ObjCell.style.display = 'none';
            }
            else if (ObjCell.type == "select-one") {
                ObjCell.selectedIndex = 0;
                ObjCell.style.display = '';
            }
        }

    }
}
//check Combo Value already select in previous / next row in grid
function CheckComboValueAlreadyUsedInGrid(ObjCurRecord) {
    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
    var iColumnIndexOfControl = parseInt(ObjCurRecord.parentNode.cellIndex);
    var ObjRecord;
    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1; i < objGrid.rows.length; i++) {
        ObjRecord = objGrid.rows[i].cells[iColumnIndexOfControl].children[0];
        if (i != iRowOfSelectControl) {
            if (ObjRecord.selectedIndex != 0) {
                if (sSelecedValue != "NEW") {
                    if (sSelecedValue == ObjRecord.options[ObjRecord.selectedIndex].text) {
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
    }
    return true;
}

//check Combo Value already select in previous / next row in grid
function CheckComboValueAlreadySelectInGrid(ObjCurRecord) {
    if (bUsed == null) bUsed = false;
    var i;
    var sSelecedValue = ObjCurRecord.options[ObjCurRecord.selectedIndex].text;
    ////debugger;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
    var iColumnIndexOfControl = parseInt(ObjCurRecord.parentNode.cellIndex);
    var ObjRecord;
        
    //if (ObjCurRecord.id.indexOf("ContentPlaceHolder1_LabourDetailsGrid_drpLbrDescription") >= 0) {
    //    i = 2;
    //}
    //else {
    //    i = 1;
    //}

    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1 ; i < objGrid.rows.length; i++) {
        ObjRecord = objGrid.rows[i].cells[iColumnIndexOfControl].children[0];

        if (i != iRowOfSelectControl) {

            if(i<iRowOfSelectControl)
            {
                if (sSelecedValue != "NEW") {
                    if (ObjRecord.options[ObjRecord.selectedIndex].text=="--Select--") {
                        alert("Please select Record at line No." + i);
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
    }
    return true;
}

function OnComboValueChange(ObjCombo, txtboxId) 
{
    var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].text;
    var ParentCtrlID;
    var Sup, Post;
    var objtxtControl;
    if (sSelecedValue == "NEW") {
        ObjCombo.style.display = 'none';

        //ParentCtrlID = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        Post = ObjCombo.id.substring(ObjCombo.id.lastIndexOf("_"),(ObjCombo.id).length);
        Sup = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        ParentCtrlID = Sup.substring(0, Sup.lastIndexOf("_"));

        objtxtControl = document.getElementById(ParentCtrlID + "_" + txtboxId + Post);
        objtxtControl.style.display = '';
        objtxtControl.focus();
    }
    else {
        ObjCombo.style.display = '';

        //ParentCtrlID = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        Post = ObjCombo.id.substring(ObjCombo.id.lastIndexOf("_"), (ObjCombo.id).length);
        Sup = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
        ParentCtrlID = Sup.substring(0, Sup.lastIndexOf("_"));
        objtxtControl = document.getElementById(ParentCtrlID + "_" + txtboxId + Post);

        //objtxtControl = document.getElementById(ParentCtrlID + "_" + txtboxId);
        objtxtControl.style.display = 'none';
        //objtxtControl.value='';
//        if (CheckComboValueAlreadyUsedInGrid(ObjCombo) == false)
        //            return false;

    }
    return true;
}
///*************************


function OnJobTypeChange(sUserDepart) {
    //Check validation for jobcard type selection.
    SetGridColumnsBasedOnChStatusJobType(sUserDepart);
}

//08042015
function SetGridColumnsBasedOnChStatusJobType(sUserDepart) {
    //var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].text;
    //alert(sUserDepart);
    debugger;
    var ObjCombo = window.document.getElementById('ContentPlaceHolder1_drpJobType');
    if (ObjCombo == null) return false;
    
    var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].value

    var ObjGrid;
    var objRow;
    ObjGrid = document.getElementById("ContentPlaceHolder1_PartDetailsGrid");
    if (ObjGrid == null) return;

    ObjtxtAggregate = document.getElementById("ContentPlaceHolder1_txtAggregate");
    if (ObjtxtAggregate == null) return;    
    
    var txtID = window.document.getElementById('ContentPlaceHolder1_txtID');
    var txtPrevAggregate = window.document.getElementById('ContentPlaceHolder1_txtPrevAggregate');
    var txtCAggregate = window.document.getElementById('ContentPlaceHolder1_txtCAggregate');
    var txtCustomer = window.document.getElementById('ContentPlaceHolder1_txtCustomer');
    var TxtModelCode = window.document.getElementById('ContentPlaceHolder1_TxtModelCode');
    var txtModelName = window.document.getElementById('ContentPlaceHolder1_txtModelName');
    var txtVehicleNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleNo');
    var DrpCustomer = window.document.getElementById('ContentPlaceHolder1_DrpCustomer');
    var DrpModelCode = window.document.getElementById('ContentPlaceHolder1_DrpModelCode');
    var DrpModelName = window.document.getElementById('ContentPlaceHolder1_DrpModelName');
    var txtAggreagateNo = window.document.getElementById('ContentPlaceHolder1_txtAggreagateNo');
    var lblVehicleNo = window.document.getElementById('ContentPlaceHolder1_lblVehicleNo');

    var txtChassisNo = window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
    var lblChassisNo = window.document.getElementById('ContentPlaceHolder1_lblChassisNo');

    var txtEngineNo = window.document.getElementById('ContentPlaceHolder1_txtEngineNo');
    var lblEngineNo = window.document.getElementById('ContentPlaceHolder1_lblEngineNo');
    var lblSelectModel = window.document.getElementById('ContentPlaceHolder1_lblSelectModel');
    var lblAggrMndt = window.document.getElementById('ContentPlaceHolder1_lblAggrMndt');
    var lblchassisMandt = window.document.getElementById('ContentPlaceHolder1_lblchassisMandt');
    var lblServiceHistroy = window.document.getElementById('ContentPlaceHolder1_lblServiceHistroy');
    
    var iDocID = 0;

    if (iDocID == 0) iDocID = dGetValue(txtID.value);
    //ObjtxtAggregate.value = (iDocID == 0 && sSelecedValue == "18") ? "G" : txtPrevAggregate.value;
    //txtCAggregate.value = (ObjtxtAggregate.value == "G") ? "Yes" : "No";

    lblSelectModel.style.display = (sSelecedValue == "18") ? "none" : "";
    lblServiceHistroy.style.display = (sSelecedValue == "18") ? "none" : "";
    lblVehicleNo.innerText = (iDocID == 0 && sSelecedValue == "18") ? "Aggregate No:" : "Veh Reg No.:";
    txtChassisNo.style.display = (sSelecedValue == "18") ? "none" : "";
    lblChassisNo.style.display = (sSelecedValue == "18") ? "none" : "";
    lblchassisMandt.style.display = (sSelecedValue == "18") ? "none" : "";

    txtEngineNo.style.display = (sSelecedValue == "18") ? "none" : "";
    lblEngineNo.style.display = (sSelecedValue == "18") ? "none" : "";

    txtCustomer.style.display= (sSelecedValue == "18") ? "none" : "";
    TxtModelCode.style.display=(sSelecedValue == "18") ? "none" : "";
    txtModelName.style.display=(sSelecedValue == "18") ? "none" : "";
    txtVehicleNo.style.display=(sSelecedValue == "18") ? "none" : "";

    DrpCustomer.style.display= (sSelecedValue == "18") ? "" : "none";
    DrpModelCode.style.display = (sSelecedValue == "18") ? "" : "none";
    DrpModelName.style.display = (sSelecedValue == "18") ? "" : "none";
    txtAggreagateNo.style.display = (sSelecedValue == "18") ? "" : "none";
    lblAggrMndt.style.display = (sSelecedValue == "18") ? "" : "none";

    ObjtxtWarrantyTag = document.getElementById("ContentPlaceHolder1_txtWarrantyTag");
    if (ObjtxtWarrantyTag == null) return;

    for (var i = 0; i < ObjGrid.rows.length; i++) {

        ObjGrid.rows[i].cells[06].style.display = "none"; //Req Qty        
        ObjGrid.rows[i].cells[07].style.display = "none"; //Issue Qty
        ObjGrid.rows[i].cells[08].style.display = "none"; //Return Qty
        ObjGrid.rows[i].cells[09].style.display = "none"; //Use Qty

        ObjGrid.rows[i].cells[10].style.display = "none"; //Paid
        ObjGrid.rows[i].cells[13].style.display = ""; //by default FOC
        ObjGrid.rows[i].cells[14].style.display = ""; //by default FOC Reason
        ObjGrid.rows[i].cells[15].style.display = "none"; //FSC Qty No FSC Qty allowed in MAN.
        ObjGrid.rows[i].cells[16].style.display = "none"; //PDI Qty 
        ObjGrid.rows[i].cells[17].style.display = "none"; //AMC Qty 
        ObjGrid.rows[i].cells[18].style.display = "none"; //Campaign Qty 
        ObjGrid.rows[i].cells[19].style.display = "none"; //Transit Qty 
        ObjGrid.rows[i].cells[20].style.display = "none"; //EnRouteTech Qty 
        ObjGrid.rows[i].cells[21].style.display = "none"; //EnrouteNonTech Qty 
        ObjGrid.rows[i].cells[22].style.display = "none"; //SpWar Qty         
        ObjGrid.rows[i].cells[23].style.display = "none"; //GoodWl Qty
        ObjGrid.rows[i].cells[24].style.display = "none"; //Warranty Qty
        ObjGrid.rows[i].cells[25].style.display = "none"; //PRE-PDI Qty
        ObjGrid.rows[i].cells[26].style.display = "none"; //Aggregate Qty
        ObjGrid.rows[i].cells[27].style.display = "none"; //Qty1 Extra Column 
        ObjGrid.rows[i].cells[28].style.display = "none"; //Qty2 Extra Column         


        if (sUserDepart == "6") ObjGrid.rows[i].cells[07].style.display = ""; //Issue Qty
        if (sUserDepart == "6") ObjGrid.rows[i].cells[08].style.display = ""; //Return Qty

        ObjGrid.rows[i].cells[06].style.display = ""; //Req Qty
        ObjGrid.rows[i].cells[09].style.display = ""; //Use Qty

        //1	Paid Service || //2	Accident || //3	Free Service || //10	BreakDown || //11	Fitness Certificate || 
        //14	Unscheduled Repairs ||//15	On Site || //17	Extended Warranty
        if (sUserDepart == "7") {
            if (sSelecedValue == "1" || sSelecedValue == "2" || sSelecedValue == "3" || sSelecedValue == "10" ||
                sSelecedValue == "11" || sSelecedValue == "14" || sSelecedValue == "15" || sSelecedValue == "17") {
                ObjGrid.rows[i].cells[10].style.display = ""; //Paid
                ObjGrid.rows[i].cells[22].style.display = ""; //SpWar Qty         
                ObjGrid.rows[i].cells[23].style.display = ""; //GoodWl Qty
                if (ObjtxtWarrantyTag.value != "N" && ObjtxtAggregate.value == "G") ObjGrid.rows[i].cells[24].style.display = ""; //Warranty Qty  //if chassis Warr flag <>'N'
                //if (ObjtxtAggregate.value == "G") ObjGrid.rows[i].cells[26].style.display = ""; //Aggregate Qty //if chassis aggregate flag ='G'
            }
            else if (sSelecedValue == "5") {//5	Service Aggrement
                ObjGrid.rows[i].cells[10].style.display = ""; //Paid
                ObjGrid.rows[i].cells[22].style.display = ""; //SpWar Qty     
                ObjGrid.rows[i].cells[17].style.display = ""; //AMC Qty 
                if (ObjtxtWarrantyTag.value != "N" && ObjtxtAggregate.value == "G") ObjGrid.rows[i].cells[24].style.display = ""; //Warranty Qty  //if chassis Warr flag <>'N'
            }
            else if (sSelecedValue == "7") {//7	PDI
                ObjGrid.rows[i].cells[10].style.display = ""; //Paid
                ObjGrid.rows[i].cells[16].style.display = ""; //PDI Qty 
            }
            else if (sSelecedValue == "12") {//12	PreSale
                ObjGrid.rows[i].cells[10].style.display = ""; //Paid
                ObjGrid.rows[i].cells[19].style.display = ""; //Transit Qty 
                ObjGrid.rows[i].cells[20].style.display = ""; //EnRouteTech Qty 
                ObjGrid.rows[i].cells[21].style.display = ""; //EnrouteNonTech Qty                   
            }
            else if (sSelecedValue == "13") {//13	Campaign
                ObjGrid.rows[i].cells[10].style.display = ""; //Paid
                ObjGrid.rows[i].cells[22].style.display = ""; //SpWar Qty                        
                ObjGrid.rows[i].cells[18].style.display = ""; //Campaign Qty
                if (ObjtxtWarrantyTag.value != "N" && ObjtxtAggregate.value == "G") ObjGrid.rows[i].cells[24].style.display = ""; //Warranty Qty  //if chassis Warr flag <>'N'
            }
            else if (sSelecedValue == "16") {//16	Pre PDI
                ObjGrid.rows[i].cells[25].style.display = ""; //PRE-PDI Qty
                ObjGrid.rows[i].cells[10].style.display = ""; //Paid
            }
            else if (sSelecedValue == "18") { //18	Aggregate
                if (ObjtxtAggregate.value == "G") ObjGrid.rows[i].cells[26].style.display = ""; //Aggregate Qty //if chassis aggregate flag ='G'
            }
        }
    }
    if (sUserDepart == "7") {
        var ObjLGrid;
        ObjLGrid = document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
        if (ObjLGrid == null) return;

        for (var i = 2; i <= ObjLGrid.rows.length - 1; i++) {
            var list = ObjLGrid.rows[i].cells[9].childNodes[1];
            var ObjDrplabtag = ObjLGrid.rows[i].cells[8].childNodes[1];
            var sDrplabtag = ObjDrplabtag.value.trim();
            //debugger;
            if (list.options.length > 0) {
                for (var k = list.options.length - 1; k >= 0; k--) {
                    list.remove(k);
                }
            }           
            
            //1	Paid Service || //2	Accident || //3	Free Service || //10	BreakDown || //11	Fitness Certificate || 
            //14	Unscheduled Repairs ||//15	On Site || //17	Extended Warranty
            if (sSelecedValue == "1" || sSelecedValue == "2" || sSelecedValue == "3" || sSelecedValue == "10" ||
               sSelecedValue == "11" || sSelecedValue == "14" || sSelecedValue == "15" || sSelecedValue == "17") {                
                if (sDrplabtag == "C") if (ObjtxtWarrantyTag.value != "N" || ObjtxtAggregate.value == "G") {                    
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="W">Warranty</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).SelecedValue = "W";
                }
                if (sDrplabtag == "D") {
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="G">Goodwill</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).SelecedValue = "N";
                }
            }
            else if (sSelecedValue == "5") {//5	Service Aggrement
             
                if (sDrplabtag == "C") {
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="A">AMC</option>');                    
                    if (ObjtxtWarrantyTag.value != "N" || ObjtxtAggregate.value == "G") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="W">Warranty</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).SelecedValue = "A";
                }
                if (sDrplabtag == "D") {
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).SelecedValue = "N";
                }
            }
            else if (sSelecedValue == "7") {//7	PDI              
                if (sDrplabtag == "C") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="P">PDI</option>');
                if (sDrplabtag == "D") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');
            }
            else if (sSelecedValue == "12") {//12	PreSale            
                if (sDrplabtag == "C") {
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="T">Transit</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="E">Enroute Technical</option>');
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="R">Enroute Non Technical</option>');
                }
                if (sDrplabtag == "D") {
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');                    
                }
            }
            else if (sSelecedValue == "13") {//13	Campaign             
                if (sDrplabtag == "C") {
                    $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="C">Campaign</option>');
                    if (ObjtxtWarrantyTag.value != "N" || ObjtxtAggregate.value == "G") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="W">Warranty</option>');
                }                
                if (sDrplabtag == "D") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');
            }
            else if (sSelecedValue == "16") {//16	Pre PDI
                //ObjGrid.rows[i].cells[25].style.display = ""; //PRE-PDI Qty
                if (sDrplabtag == "C") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="I">Pre-PDI</option>');
                if (sDrplabtag == "D") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');
            }
            else if (sSelecedValue == "18") { //18	Aggregate
                //if (ObjtxtAggregate.value == "G") ObjGrid.rows[i].cells[26].style.display = ""; //Aggregate Qty //if chassis aggregate flag ='G'
                if (sDrplabtag == "C" && ObjtxtAggregate.value == "G") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="W">Warranty</option>');
                if (sDrplabtag == "D") $('#ContentPlaceHolder1_LabourDetailsGrid_drpLabWarr_' + (i - 1)).append('<option value="N">Non-Warranty</option>');
            }
        }
    }    
    return true;
}

//To Show Chassis Master
//function ShowChassisMaster(objNewModelLabel, sDealerId, sDocType) {
//    var ChassisNo;
//    ChassisNo = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectChassis.aspx?DealerID=" + sDealerId + "&sDocType=" + sDocType, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
  
//    //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
//    if (ChassisNo == null) {
//        return false;
//    }
//    else {
//        hdnChassis = document.getElementById("ContentPlaceHolder1_hdnChassis");
//        if(hdnChassis!=null)
//        hdnChassis.value=ChassisNo;
//    }
//    return true;
//}

///*************************
//To Show Model Master
//sujata 02022011
//function ShowModelMaster(objNewModelLabel, sDealerId) {
function ShowModelMaster(objNewModelLabel, sDealerId, sDocType) {
//sujata 02022011
    var ModelDetailsValue;
    
    //sujata 02022011
    //ModelDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
    ModelDetailsValue = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId + "&sDocType=" + sDocType, 'PopupPage', 'dialogHeight:205px;dialogWidth:1000px;resizable:0;location=no;');
    //sujata 02022011
    
    //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
    if (ModelDetailsValue == null) 
    {
    return false;
    }
    else
    {
        SetModelDetails(ModelDetailsValue);        
    }
    return true;
}
//SetModelDetails
function SetModelDetails(ModelDetailsValue) {
    var PcontainerName = '';
    var bValue;
    var ObjControl;
    PcontainerName = 'ContentPlaceHolder1_';//GetContainerName();
    //Sujata 20012011
    //bValue = ModelDetailsValue[18];
    bValue = ModelDetailsValue[19];
    //Sujata 20012011
    ObjControl = window.document.getElementById(PcontainerName + "lblModel");
    if (ObjControl != null) {
        ObjControl.style.display = "";
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtModelID");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[0];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtModelDescription");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[1];
        ObjControl.style.display = "";
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtGVW");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[2];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtChassisNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[3];
        ObjControl.readOnly = bValue;
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtEngineNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[4];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtInvoiceNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[5];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtInvoiceDate");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[6];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtInstallationDate");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[7];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtCustomerName");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[8];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtCustomerAddress");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[9];
        ObjControl.readOnly = bValue;
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtModelGroupID");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[10];
        ObjControl.readOnly = bValue;
    }

    ObjControl = window.document.getElementById(PcontainerName + "txtModelCode");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[11];
        ObjControl.readOnly = bValue;
    }
    //Last Repair Order Date
    ObjControl = window.document.getElementById(PcontainerName + "txtLastRepairDate");
    if (ObjControl != null) 
    {
        ObjControl.value = ModelDetailsValue[12];
    }
    ObjControl = window.document.getElementById(PcontainerName + "txtHrsApplicable");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[13];
        if (ModelDetailsValue[13] == "Y") {
            ObjControl = window.document.getElementById(PcontainerName + "txtHrsReading");
            if (ObjControl != null) {
                ObjControl.readOnly = false;
            }

            ObjControl = window.document.getElementById(PcontainerName + "txtOdometer");
            if (ObjControl != null) {
                ObjControl.readOnly = true;
            }
        }
        else {
            ObjControl = window.document.getElementById(PcontainerName + "txtHrsReading");
            if (ObjControl != null) {
                ObjControl.readOnly = true;
            }

            ObjControl = window.document.getElementById(PcontainerName + "txtOdometer");
            if (ObjControl != null) {
                ObjControl.readOnly = false;
            }
        }
    }

    //Odometer
    ObjControl = window.document.getElementById(PcontainerName + "txtOdometer");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[14];
    }
    
    //Vehicle Registration No
    ObjControl = window.document.getElementById(PcontainerName + "txtVehicleNo");
    if (ObjControl != null) 
    {
        if (ModelDetailsValue[15] == "9999") 
        {
            ObjControl.value = '';
            ObjControl.readOnly = false;
        }
        else {
            ObjControl.value = ModelDetailsValue[15];
            ObjControl.readOnly = true;
        }
    } 
    
    //LasrRepairOrderNo
    ObjControl = window.document.getElementById(PcontainerName + "txtLastRepairOrderNo");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[16];
    }
    
    //Root Type ID
    ObjControl = window.document.getElementById(PcontainerName + "txtRootTypeID");
    if (ObjControl != null) {
        if (ModelDetailsValue[15] == "9999") {
            ObjControl.value = '';
            ObjControl.readOnly = false;
        }
        else {
            ObjControl.value = ModelDetailsValue[17];
            ObjControl.readOnly = true;
        }
    }
    //Sujata 20012011
    //Chassis ID
    ObjControl = window.document.getElementById(PcontainerName + "txtchassisID");
    if (ObjControl != null) {
        ObjControl.value = ModelDetailsValue[19];
    }
    //Sujata 20012011
}
///************************* Common***************
// to Check OdometerReading with last meter reading
function CheckOdometerReading(event,ObjControl) 
{
    var ObjLastMeterReading;
    var sLastMeterReading = "";
    if (CheckTextboxValueForNumeric(event, ObjControl,false ) == false) {
        return;
    }
    if (ObjControl.value == "") return;
    ObjLastMeterReading = window.document.getElementById("ContentPlaceHolder1_txtLastMeterReading");
    if (ObjLastMeterReading != null) {
        sLastMeterReading = ObjLastMeterReading.value;
    }
    if (sLastMeterReading == "") return;
    else 
    {
        if (dGetValue(ObjControl.value) < dGetValue(sLastMeterReading)) 
        {
            alert("Odometer Reading should not be less than the Last Odometer Reading  '" + sLastMeterReading + "'!.");
            ObjControl.value = "";
            ObjControl.focus();
        }
    }
}
// to Check Hour meterReading with last meter reading
function CheckHrsReading(event,ObjControl) {
    var ObjLastMeterReading;
    var sLastMeterReading = "";
    if (CheckTextboxValueForNumeric(event, ObjControl,false) == false) {
        return;
    }
    if (ObjControl.value == "") return;
    ObjLastMeterReading = window.document.getElementById("ContentPlaceHolder1_txtLastMeterReading");
    if (ObjLastMeterReading != null) {
        sLastMeterReading = ObjLastMeterReading.value;
    }
    if (sLastMeterReading == "") return;    
    else {
        if (dGetValue(ObjControl.value) < dGetValue(sLastMeterReading))
         {
            alert("Hour Meter Reading should not be less than the Last Odometer Reading  '" + sLastMeterReading + "'!.");
            ObjControl.value = "";
            ObjControl.focus();
        }
    }
    //
}
// check Repair Complete date should be less than or equal to Warranty Claim date
function CheckRepairCompleteDate() {
    var PcontainerName = '';
    var ObjControl;
    var ObjRepairOrderDate;
    var ObjRepairCompleteDate;
    var sRepairOrderDate = '';
    var sClaimDate = '';
    var sRepairCompleteDate = '';
    PcontainerName = GetContainerName();

    // Get Repair Complete Date
    ObjRepairCompleteDate = window.document.getElementById(PcontainerName + "txtRepairCompleteDate_txtDocDate");
    if (ObjRepairCompleteDate != null) {
        sRepairCompleteDate = ObjRepairCompleteDate.value;
    }

    if (sRepairCompleteDate == "") return;

    // Get claim Date
    ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (ObjControl != null) {
        sClaimDate = ObjControl.value;
    }

    //Check With claim Date
    if (bCheckFirstDateIsGreaterThanSecondDate(sRepairCompleteDate, sClaimDate) == true) {
        alert('Repair Complete Date should not be greater than Claim Date !');
        ObjRepairCompleteDate.value = '';
        ObjRepairCompleteDate.focus();
        return false;
    }

    // Get Repair Order Date
    ObjRepairOrderDate = window.document.getElementById(PcontainerName + "txtRepairOrderDate_txtDocDate");
    if (ObjRepairOrderDate != null) {
        sRepairOrderDate = ObjRepairOrderDate.value;
    }
    if (sRepairOrderDate == "") {

        return
    }
    //Check With repair Order Date
    if (bCheckFirstDateIsGreaterThanSecondDate(sRepairOrderDate, sRepairCompleteDate) == true) {
        alert('Repair Complete Date should be greater than the Repair Order Date !');
        ObjRepairCompleteDate.value = '';
        ObjRepairCompleteDate.focus();
        return false;
    }
}

// check Repair order date should be less than or equal to Warranty Claim date
function CheckRepairOrderDateWithClaimDate() {
    var PcontainerName = '';
    var ObjControl;
    var ObjRepairOrderDate;
    var sRepairOrderDate = '';
    var sClaimDate = '';
    var sFailureDate = '';
    var ObjRepairCompleteDate;    
    var sRepairCompleteDate = '';
    PcontainerName = GetContainerName();

    // Get claim Date
    ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (ObjControl != null) {
        sClaimDate = ObjControl.value;
    }
    
    // Get Repair Order Date
    ObjRepairOrderDate = window.document.getElementById(PcontainerName + "txtRepairOrderDate_txtDocDate");

    if (ObjRepairOrderDate != null) {
        sRepairOrderDate = ObjRepairOrderDate.value;
    }
    if (sRepairOrderDate == "") return;

    //Check With Claim Date    
    if (bCheckFirstDateIsGreaterThanSecondDate(sRepairOrderDate, sClaimDate) == true) {
        alert('Repair Order date should not be greater than Claim Date !');
        ObjRepairOrderDate.value = '';
        ObjRepairOrderDate.focus();
        return;
    }    

    // Get Failure Date
    ObjControl = window.document.getElementById(PcontainerName + "txtFailureDate_txtDocDate");
    if (ObjControl != null) {
        sFailureDate = ObjControl.value;
    }
    if (sFailureDate == "") return;

    if (bCheckFirstDateIsGreaterThanSecondDate(sFailureDate, sRepairOrderDate) == true)
     {    
        alert('Repair Order Date should be greater than Failure Date !');
        ObjRepairOrderDate.value = '';
        ObjRepairOrderDate.focus();
        return false;
    }
    // check with Repair Complete Date
    // Get Repair Complete Date
    ObjRepairCompleteDate = window.document.getElementById(PcontainerName + "txtRepairCompleteDate_txtDocDate");
    if (ObjRepairCompleteDate != null) {
        sRepairCompleteDate = ObjRepairCompleteDate.value;
    }
    if (sRepairCompleteDate == "") return;
     if (bCheckFirstDateIsGreaterThanSecondDate(sRepairOrderDate, sRepairCompleteDate) == true)
     {   
        alert('Repair Order Date should be less than Repair Complete Date !');
        ObjRepairOrderDate.value = '';        
        ObjRepairOrderDate.focus();
        return false;
    }
}
// check Repair order date should be less than or equal to Warranty Claim date
function CheckFailureDateWithRepairOrderDate() {
    var PcontainerName = '';
    var ObjControl;
    var sRepairOrderDate = '';
    var sLastRepairDate = '';
    var ObjFailureDate;
    var sFailureDate = '';
    PcontainerName = GetContainerName();

    // Get Failure Date
    ObjFailureDate = window.document.getElementById(PcontainerName + "txtFailureDate_txtDocDate");
    if (ObjFailureDate != null) {
        sFailureDate = ObjFailureDate.value;
    }
    if (sFailureDate == "") return;

    //Check With Claim Date
    ObjControl = window.document.getElementById(PcontainerName + "txtClaimDate_txtDocDate");
    if (bCheckFirstDateIsGreaterThanSecondDate(sFailureDate, ObjControl.value) == true ) 
    {
        //VHP Warranty Start
        alert("Please Select The Failure Date Less Than Claim Date!");        
        //alert("Please Select The Failure Date Greater Than Claim Date!");
        //VHP Warranty End
        ObjFailureDate.value = "";
        return;
    }
    
    //Get Last Repair Complete Date
    ObjControl = window.document.getElementById(PcontainerName + "txtLastRepairDate");
    if (ObjControl != null) {
        sLastRepairDate = ObjControl.value;
    }
    if (sLastRepairDate != "") 
    {
        //Sujata 18012011
        //The following check is not required. Only check should be that Failure can’t be in Future Date or After Job card date
//        if (sLastRepairDate == sFailureDate) 
//        {
//            alert("Failure Date should not be Less than or equal to Last Repair Complete Date '" + sLastRepairDate + "'  !.");
//            ObjFailureDate.value = '';            
//            return false;
//        }
//     if (bCheckFirstDateIsGreaterThanSecondDate(sLastRepairDate, sFailureDate) == true)
//        {          
//            alert("Failure Date should not be Less than Last Repair Complete Date '" + sLastRepairDate +  "'  !.");
//            ObjFailureDate.value = '';            
//            return false;
//        }
        //Sujata 18012011
    }
    // Get Repair Order Date
    ObjControl = window.document.getElementById(PcontainerName + "txtRepairOrderDate_txtDocDate");
    if (ObjControl != null) {
        sRepairOrderDate = ObjControl.value;
    }
    if (sRepairOrderDate == "") return;    
    
    if (bCheckFirstDateIsGreaterThanSecondDate(sFailureDate, sRepairOrderDate) == true)
        {    
        alert('Failure Date should be less than Repair Order Date!.');
        ObjFailureDate.value = '';
        ObjFailureDate.focus();
        return false;
    }
}

// To check Total of all Percent should be 100 for Item Wise
function CheckTotalOfPercentage(event ,objControl) {

    //    if (CheckPercentageValue(event, objControl) == false) {
    //        return false;
    //    }
    var objRow = objControl.parentNode.parentNode.childNodes;
    var iCellId = objControl.parentNode.cellIndex;

    var dCustShare = dGetValue(objRow[iCellId].children[0].value);
    objRow[iCellId].children[0].value = RoundupValue(dCustShare);

    iCellId = iCellId - 1;
    var dDealerShare = dGetValue(objRow[iCellId].children[0].value);
    objRow[iCellId].children[0].value = RoundupValue(dDealerShare);

    iCellId = iCellId - 1;
    var dVECVShare = dGetValue(objRow[iCellId].children[0].value);
    objRow[iCellId].children[0].value = RoundupValue(dVECVShare);

    var dTotal = dVECVShare + dDealerShare + dCustShare;
    if (dTotal != 100) {

        alert("Sum of all the share percentage should be equal to 100.");
        objControl.value = 0;        
        return false;

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

function CheckTextValueAlreadyUsedInGrid(event, ObjCurRecord) {

    if (bUsed == null) bUsed = false;
    var i;
    ////debugger;
    var sSelecedValue = ObjCurRecord.value;
    //var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[0].innerText);
    var iRowOfSelectControl = parseInt(ObjCurRecord.parentNode.parentNode.childNodes[1].innerText);
    var ObjRecord;
    var objGrid = ObjCurRecord.parentNode.parentNode.parentNode;
    for (i = 1; i < objGrid.children.length; i++) {
        //ObjRecord = objGrid.childNodes[i].childNodes[1].children[0];
        ObjRecord = objGrid.childNodes[i].childNodes[2].children[0];

        if (i != iRowOfSelectControl) {
            if (ObjRecord.type == "text" || ObjRecord.type == "textarea") {
                if (sSelecedValue.toUpperCase() == ObjRecord.value.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    if (ObjCurRecord.type == "select-one")
                        ObjCurRecord.selectedIndex = 0;
                    else
                        ObjCurRecord.value = "";
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
                if (sSelecedValue.toUpperCase() == ObjRecord.options[ObjRecord.selectedIndex].text.toUpperCase()) {
                    alert("Record is already selected at line No." + i);
                    if (ObjCurRecord.type == "select-one")
                        ObjCurRecord.selectedIndex = 0;
                    else
                        ObjCurRecord.value = "";
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
//To Check Repair Order no With Last Repair Order no
function CheckRepairOrderNoWithLastOrderNo(ObjCurrentRepirOrderNo) {

    var ObjControl = null;
    if (ObjCurrentRepirOrderNo.value == "") return;
    
    // Get Last Repair Order no
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtLastRepairOrderNo");
    if (ObjControl != null) 
    {
        if (ObjControl.value == ObjCurrentRepirOrderNo.value) 
        {

            alert("Repair Order No. is already used in previous warranty claim, Please enter another Repair Order No. !");
            ObjCurrentRepirOrderNo.value = '';
            return false;    
        }
    }


}
//To Set Total Kms
function SetJobCardTotalKms(ObjCurrentKms) {
    var ObjControl = null;    
    if (ObjCurrentKms.value == "") return;
    var sJobtype = "";
    
    // Get Last Repair Order no
    var ObjhdnAMCStKms = window.document.getElementById("ContentPlaceHolder1_hdnAMCStKms");
    var ObjhdnAMCEndKms = window.document.getElementById("ContentPlaceHolder1_hdnAMCEndKms");

    var ObjhdnWarrChkType = window.document.getElementById("ContentPlaceHolder1_hdnWarrChkType");    

    var ObjhdnAMCStKms = window.document.getElementById("ContentPlaceHolder1_hdnAMCStKms");
    var ObjhdnWarrEndKms = window.document.getElementById("ContentPlaceHolder1_hdnWarrEndKms");

    var ObjhdnExtWarrStartKms = window.document.getElementById("ContentPlaceHolder1_hdnExtWarrStartKms");
    var ObjhdnExtWarrEndKms = window.document.getElementById("ContentPlaceHolder1_hdnExtWarrEndKms");

    var ObjtxtWarrantyTag= window.document.getElementById("ContentPlaceHolder1_txtWarrantyTag");

    var ObjtxtTotKm = window.document.getElementById("ContentPlaceHolder1_txtTotKm"); // It is cumulative Kms

    var ObjtxtBfr_Last_SpdMtrChange_Kms = window.document.getElementById("ContentPlaceHolder1_txtBfr_Last_SpdMtrChange_Kms");
    var ObjtxthdnBfr_Last_SpdMtrChange_Kms = window.document.getElementById("ContentPlaceHolder1_txthdnBfr_Last_SpdMtrChange_Kms");
    //
    
    var ObjtxtSpdMtrChg = window.document.getElementById("ContentPlaceHolder1_txtSpdMtrChg");
    var ObjhdnRepeatJob = window.document.getElementById("ContentPlaceHolder1_hdnRepeatJob");
    
    var ObjtxtLastKms = window.document.getElementById("ContentPlaceHolder1_txtLastKms");
    var ObjtxtLstJbKms = window.document.getElementById("ContentPlaceHolder1_txtLstJbKms");

    var drpJobType = window.document.getElementById('ContentPlaceHolder1_drpJobType');

    if (drpJobType == null) {
        alert("Please Select Jobcard Type. !");
        ObjCurrentKms.value = '0';
        return false;
    }
    else {
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }
    
    if (ObjCurrentKms != null) {
        if (ObjhdnWarrChkType.value.trim() == "H") {
            var CumulativeKms = 0;
            if (dGetValue(ObjtxtLstJbKms.value) > dGetValue(ObjCurrentKms.value) && dGetValue(ObjCurrentKms.value) > 0) {
                CumulativeKms = dGetValue(ObjtxtLstJbKms.value) + dGetValue(ObjtxtBfr_Last_SpdMtrChange_Kms.value) + dGetValue(ObjCurrentKms.value);
            }
            else {
                CumulativeKms = dGetValue(ObjtxtBfr_Last_SpdMtrChange_Kms.value) + dGetValue((dGetValue(ObjCurrentKms.value) == 0) ? dGetValue(ObjtxtLstJbKms.value) : dGetValue(ObjCurrentKms.value));
            }
            if (sJobtype == 5) {
                //if (ObjhdnAMCEndKms.value < ObjCurrentKms.value) {
                if (dGetValue(ObjhdnAMCEndKms.value) < CumulativeKms) {
                    alert("Cumulative Kms " + CumulativeKms + " are greater than RMC End Kms (" + ObjhdnAMCEndKms.value + "). Chassis RMC status will get changed after saving Jobcard.");
                  //alert("Cumulative Kms " + CumulativeKms + " Can not be Greater than Chassis AMC End Kms " + ObjhdnAMCEndKms.value + ". !");                   
                }
            }

            if (sJobtype != 5 && (ObjtxtWarrantyTag.value.trim() == "W" || ObjtxtWarrantyTag.value.trim() == "E")) {
                if (ObjtxtWarrantyTag.value.trim() == "W" && dGetValue(ObjhdnWarrEndKms.value) < CumulativeKms) {
                    alert("Cumulative kms " + CumulativeKms + " are greater than Normal Warranty End Kms (" + ObjhdnWarrEndKms.value + "). Chassis Normal Warranty status will get changed after saving Jobcard.");
                    //alert("Cumulative Kms " + CumulativeKms + " are greater than " + ObjhdnWarrEndKms.value + " so chassis Normal warranty status get changed after saved jobcard. !");
                }
                else if (ObjtxtWarrantyTag.value.trim() == "E" && dGetValue(ObjhdnExtWarrEndKms.value) < CumulativeKms) {
                    alert("Cumulative kms " + CumulativeKms + " are greater than Extended Warranty End Kms (" + ObjhdnExtWarrEndKms.value + "). Chassis Extended Warranty status will get changed after saving Jobcard.");
                    //alert("Cumulative Kms " + CumulativeKms + " are greater than " + ObjhdnExtWarrEndKms.value + " so chassis Extended warranty status get changed after saved jobcard. !");
                }
            }
        }
        //Last Chassis Tot Kms
        var LastSpdKm = 0;
        var LastSpdKm = dGetValue(ObjtxtLastKms.value);
        //check last jobcard kms less than current kms if yes then previous tot kms set to Bfr_Last_SpdMtrChange_Kms
        if (dGetValue(ObjtxtLstJbKms.value) > dGetValue(ObjCurrentKms.value) && dGetValue(ObjCurrentKms.value) > 0) {
            if (confirm("Less Than Previous Kms :Last Km. Is this Odometer Change?") == true) {
                //LastSpdKm = dGetValue(LastSpdKm) + dGetValue(ObjCurrentKms.value);
                ObjtxthdnBfr_Last_SpdMtrChange_Kms.value = ObjtxtLastKms.value;
                //ObjtxtTotKm.value = LastSpdKm;
                ObjtxtTotKm.value = dGetValue(ObjtxtLstJbKms.value) + dGetValue(ObjtxtBfr_Last_SpdMtrChange_Kms.value) + dGetValue(ObjCurrentKms.value);
                
                ObjtxtSpdMtrChg.value = "Yes";
                ObjhdnRepeatJob.value = "N";
                //ObjCurrentKms.readOnly = true;
            }
            else {                
                ObjCurrentKms.value = '0';
                return false;
            }
        }
        else {
            //LastSpdKm = dGetValue(LastSpdKm) + dGetValue(ObjCurrentKms.value);
            //ObjtxtTotKm.value = LastSpdKm;
            //debugger;
            ObjtxtTotKm.value = dGetValue(ObjtxtBfr_Last_SpdMtrChange_Kms.value) + dGetValue((dGetValue(ObjCurrentKms.value) == 0) ? dGetValue(ObjtxtLstJbKms.value) : dGetValue(ObjCurrentKms.value));
            ObjtxtSpdMtrChg.value = "No";
            ObjtxthdnBfr_Last_SpdMtrChange_Kms.value = ObjtxtBfr_Last_SpdMtrChange_Kms.value;
            //ObjlblTotKm.innerText = ObjCurrentKms.value;
            //ObjlblTotKm.outerText = ObjCurrentKms.value;
            //ObjlblTotKm.textContent = ObjCurrentKms.value;
            //ObjlblTotKm.innerHTML = ObjCurrentKms.value;
        }
// Repeat job criteria            
//            if (ObjCurrentKms.value < ObjlblLastKms.value + 40000 && ObjCurrentKms.value > 0) {
//
        //            }
        return;
    }

}

//To Set Total Kms
function SetJobCardTotalHrs(ObjCurrentHrs) {
    var ObjControl = null;
    if (ObjCurrentHrs.value == "") return;
    var sJobtype = "";

    // Get Last Repair Order no
    //ObjhdnAMCStKms = window.document.getElementById("ContentPlaceHolder1_hdnAMCStKms");
    //ObjhdnAMCEndKms = window.document.getElementById("ContentPlaceHolder1_hdnAMCEndKms");

    var ObjhdnAMCStHrs = window.document.getElementById("ContentPlaceHolder1_hdnAMCStHrs");
    var ObjhdnAMCEndHrs = window.document.getElementById("ContentPlaceHolder1_hdnAMCEndHrs");

    var ObjhdnWarrChkType = window.document.getElementById("ContentPlaceHolder1_hdnWarrChkType");
    
    var ObjhdnWarrEndHrs = window.document.getElementById("ContentPlaceHolder1_hdnWarrEndHrs");

    var ObjhdnExtWarrStartHrs = window.document.getElementById("ContentPlaceHolder1_hdnExtWarrStartHrs");
    var ObjhdnExtWarrEndHrs = window.document.getElementById("ContentPlaceHolder1_hdnExtWarrEndHrs");

    var ObjtxtWarrantyTag = window.document.getElementById("ContentPlaceHolder1_txtWarrantyTag");
    
    ObjtxtTotHrs = window.document.getElementById("ContentPlaceHolder1_txtTotHrs"); // It is cumulative Kms

    ObjtxtBfr_Last_HrsMtrChange_Hrs = window.document.getElementById("ContentPlaceHolder1_txtBfr_Last_HrsMtrChange_Hrs");
    ObjtxthdnBfr_Last_HrsMtrChange_Hrs = window.document.getElementById("ContentPlaceHolder1_txthdnBfr_Last_HrsMtrChange_Hrs");
    //
    //debugger;
    ObjtxtHrsMtrChg = window.document.getElementById("ContentPlaceHolder1_txtHrsMtrChg");
    ObjhdnRepeatJob = window.document.getElementById("ContentPlaceHolder1_hdnRepeatJob");

    ObjtxtLastHrs = window.document.getElementById("ContentPlaceHolder1_txtLastHrs");
    ObjtxtLstJbHrs = window.document.getElementById("ContentPlaceHolder1_txtLstJbHrs");

    var drpJobType = window.document.getElementById('ContentPlaceHolder1_drpJobType');

    if (drpJobType == null) {
        alert("Please Select Jobcard Type. !");
        ObjCurrentHrs.value = '0';
        return false;
    }
    else {
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }

    if (ObjCurrentHrs != null) {
        var CumulativeHrs = 0;
        debugger;
        if (ObjhdnWarrChkType.value.trim() == "T") {
            if (dGetValue(ObjtxtLstJbHrs.value) > dGetValue(ObjCurrentHrs.value) && dGetValue(ObjCurrentHrs.value) > 0) {
                CumulativeHrs = dGetValue(ObjtxtLstJbHrs.value) + dGetValue(ObjtxtBfr_Last_HrsMtrChange_Hrs.value) + dGetValue(ObjCurrentHrs.value);
            }
            else {
                CumulativeHrs = dGetValue(ObjtxtBfr_Last_HrsMtrChange_Hrs.value) + dGetValue((dGetValue(ObjCurrentHrs.value) == 0) ? dGetValue(ObjtxtLstJbHrs.value) : dGetValue(ObjCurrentHrs.value));
            }
            if (sJobtype == 5) {
                //if (ObjhdnAMCEndKms.value < ObjCurrentKms.value) {
                if (dGetValue(ObjhdnAMCEndHrs.value) < CumulativeHrs) {
                    alert("Cumulative Hours " + CumulativeHrs + " are greater than RMC End Hours (" + ObjhdnAMCEndHrs.value + "). Chassis RMC status will get changed after saving Jobcard.");
                    //alert("Cumulative Hrs " + CumulativeHrs + " Can not be Greater than AMC End Hrs." + ObjhdnAMCEndHrs.value + " !");                   
                }
            }

            if (sJobtype != 5 && (ObjtxtWarrantyTag.value.trim() == "W" || ObjtxtWarrantyTag.value.trim() == "E")) {
                if (ObjtxtWarrantyTag.value.trim() == "W" && dGetValue(ObjhdnWarrEndHrs.value) < CumulativeHrs) {
                    alert("Cumulative Hours " + CumulativeHrs + " are greater than Normal Warranty End Hours (" + ObjhdnWarrEndHrs.value + "). Chassis Normal Warranty status will get changed after saving Jobcard.");
                    //alert("Cumulative Hrs " + CumulativeHrs + " are greater than " + ObjhdnWarrEndHrs.value + " so chassis Normal warranty status get changed after saved jobcard. !");
                }
                else if (ObjtxtWarrantyTag.value.trim() == "E" && dGetValue(ObjhdnExtWarrEndHrs.value) < CumulativeHrs) {
                    alert("Cumulative Hours " + CumulativeHrs + " are greater than Extended Warranty End Hours (" + ObjhdnExtWarrEndHrs.value + "). Chassis Extended Warranty status will get changed after saving Jobcard.");
                  //alert("Cumulative Hrs " + CumulativeHrs +" are greater than " + ObjhdnExtWarrEndHrs.value + " so chassis Extended warranty status get changed after saved jobcard. !");
                }
            }
        }

        //Last Chassis Tot Hrs
        var LastSpdHrs = 0;
        var LastSpdHrs = dGetValue(ObjtxtLastHrs.value);
        //check last jobcard Hrss less than current Hrs if yes then previous tot Hrs set to Bfr_Last_SpdMtrChange_Hrs
        if (dGetValue(ObjtxtLstJbHrs.value) > dGetValue(ObjCurrentHrs.value) && dGetValue(ObjCurrentHrs.value) > 0) {
            if (confirm("Less Than Previous Hrs :Last Hrs. Is this HourMeter Change?") == true) {
                //LastSpdHrs = dGetValue(LastSpdHrs) + dGetValue(ObjCurrentHrs.value);
                ObjtxthdnBfr_Last_HrsMtrChange_Hrs.value = ObjtxtLastHrs.value;
                //ObjtxtTotHrs.value = LastSpdHrs;
                ObjtxtTotHrs.value = dGetValue(ObjtxtLstJbHrs.value) + dGetValue(ObjtxtBfr_Last_HrsMtrChange_Hrs.value) + dGetValue(ObjCurrentHrs.value);

                ObjtxtHrsMtrChg.value = "Yes";
                ObjhdnRepeatJob.value = "N";
                //ObjCurrentHrs.readOnly = true;
            }
            else {
                ObjCurrentHrs.value = '0';
                return false;
            }
        }
        else {
            //LastSpdHrs = dGetValue(LastSpdHrs) + dGetValue(ObjCurrentHrs.value);
            //ObjtxtTotHrs.value = LastSpdHrs;
            //debugger;
            ObjtxtTotHrs.value = dGetValue(ObjtxtBfr_Last_HrsMtrChange_Hrs.value) + dGetValue((dGetValue(ObjCurrentHrs.value) == 0) ? dGetValue(ObjtxtLstJbHrs.value) : dGetValue(ObjCurrentHrs.value));
            ObjtxtHrsMtrChg.value = "No";
            ObjtxthdnBfr_Last_HrsMtrChange_Hrs.value = ObjtxtBfr_Last_HrsMtrChange_Hrs.value;
            //ObjlblTotHrs.innerText = ObjCurrentHrs.value;
            //ObjlblTotHrs.outerText = ObjCurrentHrs.value;
            //ObjlblTotHrs.textContent = ObjCurrentHrs.value;
            //ObjlblTotHrs.innerHTML = ObjCurrentHrs.value;
        }
        // Repeat job criteria            
        //            if (ObjCurrentHrs.value < ObjlblLastHrss.value + 40000 && ObjCurrentHrs.value > 0) {
        //
        //            }
        return;
    }

}
//To Bay Allocation Details
function SetJobCardBay(ObjBay) {
    if (ObjBay.value == "") return;
    var sJobtype = "";
    var sBayNo = "";
    var drpJobType = window.document.getElementById('ContentPlaceHolder1_drpJobType');

    ////debugger;
    if (drpJobType == null) {
        alert("Please Select Jobcard Type. !");
        return false;
    }
    else {
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }

    if (sJobtype == "10" || sJobtype == "15") {
        return ;
    }
    
    sBayNo = ObjBay.options[ObjBay.selectedIndex].text;
    var BayDetails = sBayNo.split('#');
    if (BayDetails.length == 2) {
        var txtDocNo = window.document.getElementById('ContentPlaceHolder1_txtDocNo');
        if (txtDocNo.value != BayDetails[1]) {
            alert("Bay is Already Used for Job No"+ BayDetails[1] +". Please select Other Bay!");
            ObjBay.value = '0';
            return false;
        }
    }
    if (BayDetails[0] == "Accidental Additional" || BayDetails[0] == "Additional") {
        alter("This Jobcard will not count in Bay Productivity.");
        return;
    }
}

//To Bay Allocation Details
function SetJobCardLabor(ObjGridLabor) {
   
}

//To Set Committed time change reason
function SetJobCardLabor() {
    ObjControl = document.getElementById("ContentPlaceHolder1_dtpJobCommited_txtDocDate");
    if (ObjControl != null) {
        return;
    }
    
    if (ObjControl.value == "") return;
    var sJobtype = "";
    var sBayNo = "";
    var DrpDelayReason = window.document.getElementById('ContentPlaceHolder1_DrpDelayReason');
    if (drpJobType == null) {
        alert("Please Select Jobcard Type. !");
        return false;
    }
    else {
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }

}

function ChkFreeServiceClick(objImgControl) {
    //debugger;

    var objCol = objImgControl.parentNode.parentNode
    var sID = objCol.cells[3].children[0].value;
    var txtID = window.document.getElementById('ContentPlaceHolder1_txtID');
    var txtDocNo = window.document.getElementById('ContentPlaceHolder1_txtDocNo');
    var txtSrvJobID;
    var objGrid = objImgControl.parentNode.parentNode.parentNode;
   
    var i;
   //for (i = 1; i < objGrid.children.length; i++) {
    for (i = 1; i < objGrid.rows.length; i++) {
        txtSrvJobID = objGrid.rows[i].cells[5].children[0];
        txtSrvJobNo = objGrid.rows[i].cells[4].children[0];
        if (txtSrvJobID.value == txtID.value && objGrid.rows[i].cells[3].children[0].value != sID) {
            txtSrvJobID.value = 0;
            txtSrvJobNo.innerText = "";
            objGrid.rows[i].cells[1].children[0].checked = false;            
        }
        else if (objGrid.rows[i].cells[3].children[0].value == sID) {
            txtSrvJobID = objGrid.rows[i].cells[5].children[0];
            txtSrvJobID.value = txtID.value;
            txtSrvJobNo.innerText = txtDocNo.value;
            objGrid.rows[i].cells[1].children[0].checked = true;            
        }
    }
    return;
}

function CalulateJbPartGranTotal() {
    
    var objGrid = document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

    var objLGrid = document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");

    var objGridGroupTax = document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    var objGridDocTax = document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");

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
        objGridGroupTax.rows[k].childNodes[4].children[0].value = 0;
    }

    for (var i = 1; i < CountRow; i++) {
        //total = objGrid.rows[i].childNodes[15].children[1].value;
        total = objGrid.rows[i].childNodes[31].children[0].value;
        sPArtName = objGrid.rows[i].childNodes[4].children[0].value;
        sGroupCode = objGrid.rows[i].childNodes[39].children[2].value;        
        bPartSel = objGrid.rows[i].childNodes[40].children[0].children[0].checked;

        sGridPartTax = objGrid.rows[i].childNodes[39].children[3].value.trim();

        //sGridPartTax = objGrid.rows[i].childNodes[16].children[0].selectedIndex;

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value)).toFixed(2);
            }
        }

        if (sPArtName != "" && bPartSel == false) {
            TotalInv = dGetValue(total) + dGetValue(TotalInv)
        }
    }
    
    CountRow = objLGrid.rows.length;
    for (var i = 1; i < CountRow; i++) {
        //total = objGrid.rows[i].childNodes[15].children[1].value;
        //debugger;
        var objtxtLabTag = objLGrid.rows[i].cells[8].childNodes[1];
        var objtxtLSubletAmt = objLGrid.rows[i].cells[14].childNodes[1];
        var txtTotal = objLGrid.rows[i].cells[7].childNodes[1];
        var objdrpLabWarr = objLGrid.rows[i].cells[9].childNodes[1];
        var objdrpLFOC = objLGrid.rows[i].cells[10].childNodes[1];

        var LFOCTag = objdrpLFOC.value;
        var dSubletAmt = dGetValue(objtxtLSubletAmt.value);
        var dLaborAmt = dGetValue(txtTotal.value);
        var sLabTag = objtxtLabTag.value.trim();
        var sWarrTag = objdrpLabWarr.value;
        
        total = 0;
        sPArtName = objLGrid.rows[i].cells[3].childNodes[1].value;
        if (sLabTag == "D" && dSubletAmt > 0 && LFOCTag == "N" && sWarrTag == "N") total = dSubletAmt;
        if (sLabTag == "D" && dSubletAmt == 0 && LFOCTag == "N" && sWarrTag == "N") total = dLaborAmt;
        
        sGroupCode = objLGrid.rows[i].childNodes[13].children[3].value;
        bPartSel = objLGrid.rows[i].childNodes[22].children[0].children[0].checked;

        sGridPartTax = objLGrid.rows[i].childNodes[13].children[4].value.trim();

        //sGridPartTax = objGrid.rows[i].childNodes[16].children[0].selectedIndex;

        for (var k = 1; k < CountRowGrTax; k++) {
            var sMGroupCode = objGridGroupTax.rows[k].childNodes[2].children[0].value.trim();
            //var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].selectedIndex;
            var sMGrouptax = objGridGroupTax.rows[k].childNodes[7].children[0].value.trim();
            if (sMGrouptax != "" && sPArtName != "" && bPartSel == false
            && sMGrouptax == sGridPartTax && sMGroupCode.trim() == sGroupCode.trim()) {
                objGridGroupTax.rows[k].childNodes[4].children[0].value = parseFloat(dGetValue(total) + dGetValue(objGridGroupTax.rows[k].childNodes[4].children[0].value)).toFixed(2);
            }
        }        
        if (sPArtName != "" && bPartSel == false) {
            TotalInv = dGetValue(total) + dGetValue(TotalInv)
        }
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

    for (var i = 1; i < CountGrpRow; i++) {
        // if (objGridGroupTax.rows[i].className.indexOf('RowStyle') > 0) {
        //group total
        dGrpTotal = dGetValue(objGridGroupTax.rows[i].childNodes[4].children[0].value);
        //group Percentage
        dGrpDiscPer = dGetValue(objGridGroupTax.rows[i].childNodes[5].children[0].value);
        //group Discount Amount
        dGrpDiscAmt = parseFloat(dGetValue(dGetValue(dGrpTotal) * dGetValue(dGrpDiscPer / 100))).toFixed(2);
        //Doc Discount Amount
        dDocDiscAmt = dGetValue(dGrpDiscAmt) + dGetValue(dDocDiscAmt);
        //group Discount Amount display                                   
        objGridGroupTax.rows[i].childNodes[6].children[0].value = parseFloat(dGrpDiscAmt).toFixed(2);
        //Amount whiich is applicable for tax
        dGrpTaxAppAmt = dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt);
        //debugger;
        //Main tax calculation
        dGrpMTaxPer = dGetValue(objGridGroupTax.rows[i].childNodes[9].children[0].value);
        if (isNaN(dGrpMTaxPer) == true) dGrpMTaxPer = 0;
        ////debugger;
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
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt)) * dGetValue(dGrpTax1Per / 100));
        } else if (sGrpTax1ApplOn == "3") {
            dGrpTax1Amt = dGetValue(dGetValue(dGetValue(dGrpTotal) - dGetValue(dGrpDiscAmt) + dGetValue(dGrpMTaxAmt)) * dGetValue(dGrpTax1Per / 100));
        } else {
            dGrpTax1Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax1Per / 100));
        }
        //Sujata 24092014End
        dDocTax1Amt = dGetValue(dDocTax1Amt) + dGetValue(dGrpTax1Amt);
        objGridGroupTax.rows[i].childNodes[14].children[0].value = parseFloat(dGrpTax1Amt).toFixed(2);

        dGrpTax2Per = dGetValue(objGridGroupTax.rows[i].childNodes[17].children[0].value);
        //New
        sGrpTax2ApplOn = objGridGroupTax.rows[i].childNodes[16].children[2].value;
        //sGrpTax2ApplOn = objGridGroupTax.rows[i].childNodes[17].children[2].value;

        if (isNaN(dGrpTax2Per) == true) dGrpTax2Per = 0;
        //Sujata 24092014Begin
        //dGrpTax2Amt = dGetValue(dGetValue(dGrpMTaxAmt) * dGetValue(dGrpTax2Per / 100));
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

        dGrpTotal = dGetValue(dGetValue(dGrpTaxAppAmt) + dGetValue(dGrpMTaxAmt) + dGetValue(dGrpTax1Amt) + dGetValue(dGrpTax2Amt));
        dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal); //This takes for apply PF and Other tax
        objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(2);
        //dDocTotalAmtFrPFOther = dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dGrpTotal).toFixed(0); //This takes for apply PF and Other tax
        //objGridGroupTax.rows[i].childNodes[19].children[0].value = parseFloat(dGrpTotal).toFixed(0);

        //  }
    }

    objGridDocTax.rows[1].childNodes[3].children[0].value = parseFloat(TotalInv).toFixed(2);
    objGridDocTax.rows[1].childNodes[4].children[0].value = parseFloat(dDocDiscAmt).toFixed(2);

    objGridDocTax.rows[1].childNodes[6].children[0].value = parseFloat(dDocLSTAmt).toFixed(2);
    objGridDocTax.rows[1].childNodes[7].children[0].value = parseFloat(dDocCSTAmt).toFixed(2);

    objGridDocTax.rows[1].childNodes[8].children[0].value = parseFloat(dDocTax1Amt).toFixed(2);
    objGridDocTax.rows[1].childNodes[9].children[0].value = parseFloat(dDocTax2Amt).toFixed(2);

    var dDocPFPer = 0;
    var dDocPFAmt = 0;
    var dDocOtherPer = 0;
    var dDocOtherAmt = 0;
    var dDocGrandAmt = 0;

    dDocPFPer = objGridDocTax.rows[1].childNodes[10].children[0].value;
    if (isNaN(dDocPFPer) == true) dDocPFPer = 0;
    dDocPFAmt = dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocPFPer / 100);
    objGridDocTax.rows[1].childNodes[11].children[0].value = parseFloat(dDocPFAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocPFAmt));

    dDocOtherPer = objGridDocTax.rows[1].childNodes[12].children[0].value;
    if (isNaN(dDocOtherPer) == true) dDocOtherPer = 0;
    dDocOtherAmt = dGetValue(dGetValue(dDocTotalAmtFrPFOther) * dGetValue(dDocOtherPer / 100));
    objGridDocTax.rows[1].childNodes[13].children[0].value = parseFloat(dDocOtherAmt).toFixed(2);
    dDocTotalAmtFrPFOther = dGetValue(dGetValue(dDocTotalAmtFrPFOther) + dGetValue(dDocOtherAmt));

    var hdnRounOff = document.getElementById('ContentPlaceHolder1_hdnRounOff');

    if (hdnRounOff.value == "N") {
        objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(2);
    }
    else {
        objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(0);
    }
    //objGridDocTax.rows[1].childNodes[14].children[0].value = parseFloat(dDocTotalAmtFrPFOther).toFixed(0);

}


// Add Amount to Total Amount And Grand Amount
// typeofAmt i.e.Part/Labour/Lubricant/SubLet
function AddAmountToTotal() {
    var PcontainerName = '';
    var txtTotalAmt;
    var txtJbTotAmt;
    var TotalAmtId;


    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";

    var txtPartAmount = document.getElementById(PcontainerName + 'txtPartAmount');
    var txtLubricantAmount = document.getElementById(PcontainerName + 'txtLubricantAmount');

    //Part And Lubricant Calculation START

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    var dPTotal = 0;
    var dOTotal = 0;
    var dLTotal = 0;
    var dSTotal = 0;
    var sPartType = "";
    var sCancle = "";
    debugger;
    for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
        var dBillQty = 0;
        var dPListRate = 0;

        var objPartType = ObjGrid.rows[i].cells[36].childNodes[1];

        var objPCancle = ObjGrid.rows[i].cells[39].childNodes[1].childNodes[0];
        var objtxtPaidQtyControl = ObjGrid.rows[i].cells[10].childNodes[1];
        var objPListRate = ObjGrid.rows[i].cells[29].childNodes[1];
        var objPTotal = ObjGrid.rows[i].cells[30].childNodes[1];

        sPartType = objPartType.value.trim();
        sCancle = (objPCancle.checked == true) ? "Y" : "N";
        dBillQty = dGetValue(objtxtPaidQtyControl.value);
        dPListRate = dGetValue(objPListRate.value);
        if (sCancle == "N") {
            if (sPartType == "P") dPTotal =parseFloat(dGetValue(dPTotal) + (dBillQty * dPListRate)).toFixed(2);
            if (sPartType == "O") dOTotal = parseFloat(dGetValue(dOTotal) + (dBillQty * dPListRate)).toFixed(2);
        }
    }
    txtPartAmount.value = parseFloat(dPTotal).toFixed(2);
    txtLubricantAmount.value =parseFloat(dOTotal).toFixed(2);

    //Part And Lubricant Calculation END
    ////debugger;
    //Labour and Sublet Calculation START
    var txtLabourAmount = document.getElementById('ContentPlaceHolder1_txtLabourAmount');
    var txtSubletAmount = document.getElementById('ContentPlaceHolder1_txtSubletAmount');
    var ObjLGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");

    if (ObjLGrid == null) return;

    for (var i = 2; i <= ObjLGrid.rows.length - 1; i++) {
        var objtxtLabTag = ObjLGrid.rows[i].cells[8].childNodes[1];
        var objtxtLSubletAmt = ObjLGrid.rows[i].cells[14].childNodes[1];
        var txtTotal = ObjLGrid.rows[i].cells[7].childNodes[1];
        var objdrpLabWarr = ObjLGrid.rows[i].cells[9].childNodes[1];
        var objdrpLFOC = ObjLGrid.rows[i].cells[10].childNodes[1];

        var LFOCTag = objdrpLFOC.value;
        var dSubletAmt = dGetValue(objtxtLSubletAmt.value);
        var dLaborAmt = dGetValue(txtTotal.value);
        var sLabTag = objtxtLabTag.value.trim();
        var sWarrTag = objdrpLabWarr.value;

        if (sLabTag == "D" && dSubletAmt > 0 && LFOCTag == "N" && sWarrTag =="N") dSTotal = parseFloat(dGetValue(dSTotal) + dSubletAmt).toFixed(2);
        if (sLabTag == "D" && dSubletAmt == 0 && LFOCTag == "N" && sWarrTag == "N") dLTotal = parseFloat(dGetValue(dLTotal) + dLaborAmt).toFixed(2);
    }
    txtSubletAmount.value = parseFloat(dSTotal).toFixed(2);
    txtLabourAmount.value = parseFloat(dLTotal).toFixed(2);
    //Labour and Sublet Calculation END 

    // Get Grand Amount       
    txtJbTotAmt = document.getElementById('ContentPlaceHolder1_txtJbTotAmt');
    txtJbTotAmt.value = parseFloat(dGetValue(dPTotal) + dGetValue(dOTotal) + dGetValue(dLTotal) + dGetValue(dSTotal)).toFixed(2);

    CalulateJbPartGranTotal();
}


function SetModelCombo(Object1, Object2) {
    //debugger;

    var ParentCtrlID;
    var ObjObject2;    

    ParentCtrlID = Object1.id.substring(0, Object1.id.lastIndexOf("_"));

    ObjObject2 = document.getElementById(ParentCtrlID + "_" + Object2);  

    if (Object1.value == "") return;

    var sObj1Val = "";
    var sObj2Val = "";

    //debugger;
    if (Object1 == null) {
        return false;
    }
    else {
        sObj1Val = Object1.options[Object1.selectedIndex].value.trim();
    }

    if (Object2 == null) {
        return false;
    }
    else {
        sObj2Val = ObjObject2.value.trim();
    }

    if (sObj1Val != sObj2Val)
    {
        //debugger;
        //ObjObject2.selectedIndex = Object1.selectedIndex;
        document.getElementById(ObjObject2.id).value = sObj1Val;
        //ObjObject2.id.value = sObj1Val;
        
        //ObjObject2.options[ObjObject2.selectedIndex].value = sObj1Val;
    }
}