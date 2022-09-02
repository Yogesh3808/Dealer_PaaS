//********************* Job Function*******************//

// To Get Part Id which are previously selected by user.
function GetPreviousSelectedPartIDInJobs(objNewPartLabel) {
    var objRow;
    var PartIds = "";
    var PartId = "";
    var txtPartId;
    // get grid object
    var objGrid = objNewPartLabel.parentNode.parentNode.parentNode.parentNode;
    if (objGrid == null) return PartIds;

    for (var i = 1; i < objGrid.rows.length; i++) 
    {
        //Get Row
        objRow = objGrid.rows[i];

        //Get Textbox of the Part ID
        txtPartId = objGrid.rows[i].children[2].childNodes[0];
        
        //Get PartId;
        PartId = dGetValue(txtPartId.value);
        if (PartId != 0)
         {
            PartIds = PartIds + PartId + ",";
        }
    }
    PartIds = PartIds.substring(0, (PartIds.lastIndexOf(",")));

    return PartIds;
}

//To Show Part Master
function ShowPartMaster(objNewPartLabel, sDealerId, sSuperceded) {
    //debugger;
    var PartDetailsValue;
    //var sSelectedPartID = GetPreviousSelectedPartIDInJobs(objNewPartLabel);
    var sSelectedPartID = "";    
    //PartDetailsValue = window.showModalDialog("../Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&Superceded=N", "List", "scrollbars=no,resizable=no,dialogWidth=100%,dialogHeight=1000px");
    PartDetailsValue = window.showModalDialog("../Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&Superceded=N", "List", "dialogWidth:700px;dialogHeight:380px;status:no;help:no;scrollbars:no;resizable:no;");
    if (PartDetailsValue != null) {
        SetPartDetails(objNewPartLabel, PartDetailsValue);
    }
}
//SetPartDetails
function SetPartDetails(objAddNewControl, PartValue) {
    //objLabel.parentNode.childNodes[1].value=Value[1];
    var objRow = objAddNewControl.parentNode.parentNode.childNodes;
    //debugger;
    objAddNewControl.style.display = "none";

    //Set PartId;
    //objRow[3].children[0].value = PartValue[0];
    objRow[4].children[0].value = PartValue[1];
   

    //SetPartNo
    objRow[5].children[0].value = PartValue[2];
    objRow[5].children[0].style.display = "";
    objRow[5].children[0].readOnly = true;

    //SetPartName
    objRow[6].childNodes[1].value = PartValue[3];
    objRow[6].childNodes[1].style.display = "";
    objRow[6].childNodes[1].readOnly = true;


    // If  Non Warrantable Part
    objRow[4].children[1].value = PartValue[5];
    if (PartValue[5] == "N") 
    {
        objRow[9].children[0].selectedIndex = 0;
        objRow[9].children[0].disabled = false;
    }
    else 
    {
        objRow[9].children[0].selectedIndex = 0;
        objRow[9].children[0].disabled = true;
    }
    objRow[10].children[1].style.display = "";
    SetJobCodeRecordCount();
}

//Check Job Code Validation
function CheckJobCodeValidation(ObjJobCode) 
{
    var iSelectedJobCode = 0, iCurrJobCode = 0;
    iSelectedJobCode = ObjJobCode.selectedIndex;
    var irowIndex = ObjJobCode.parentNode.parentNode.rowIndex;
    if (ObjJobCode.selectedIndex == 0) {
        var objRow = ObjJobCode.parentNode.parentNode.childNodes;
        objRow[5].children[0].selectedIndex = 0; // Reset Culprit
        objRow[6].children[0].selectedIndex = 0; // Reset Defect
        objRow[7].children[0].selectedIndex = 0; // Reset Technical
    }
    //    if (iSelectedJobCode != irowIndex) {

    //        alert("Please Select The Job Code J" + irowIndex);
    //        ObjJobCode.selectedIndex = 0;
    //        return false;
    //    }
    var bJobCodeSelected = true;
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_JobDetailsGrid");
    for (i = 1; i < irowIndex; i++) {
        iCurrJobCode = ObjGrid.rows[i].cells[1].children[0].selectedIndex;
        if (iCurrJobCode == 0) {
            bJobCodeSelected = false;
            alert("Please First Select The Job Code J" + i);
            ObjJobCode.selectedIndex = 0;
            return false;
        }
    }
    return true;
}

// check CulpritCode Validation
function CheckCulpritCodeValidation(ObjCulprit) {
    var PcontainerName = '';
    var ObjGrid = null;
    var objRow = ObjCulprit.parentNode.parentNode.childNodes;
    var sCurrJobCode = "";
    var sSelectedJobCode = "";    
    var iCurrRowIndex = ObjCulprit.parentNode.parentNode.rowIndex;
    var iPartID = 0;
    var iSelectedCulpritCode = 0;
    var iCurrCulpritCode = 0;
    debugger;
    if (ObjCulprit.selectedIndex == 0) 
    {
        var ObjDefect = objRow[6].children[0];
        ObjDefect.selectedIndex = 0;
        return;
    }

    sSelectedJobCode = objRow[3].children[0].innerText;

    //Check Part Details;
    iPartID = dGetValue(objRow[4].children[0].value);
    if (iPartID == 0) 
    {
        alert("Please select the Causal Part for the Job " + sSelectedJobCode);
        ObjCulprit.selectedIndex = 0;
        return false;    
    }   

    //Get Culprit Code ID
    iSelectedCulpritCode = objRow[7].children[0].selectedIndex
    // check Selected Culprit code is already selected for the job code
    PcontainerName = GetContainerName();
    //Megha 09102012
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //Megha 09102012
    ObjGrid = window.document.getElementById(PcontainerName + "JobDetailsGrid");
    for (i = 1; i < ObjGrid.rows.length; i++)
     {
         if (i != iCurrRowIndex) 
        {
            //sCurrJobCode = ObjGrid.rows[i].cells[1].children[0].selectedIndex;
            sCurrJobCode = ObjGrid.rows[i].cells[2].children[0].innerText;
            iPartID = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
            if (iPartID != 0) {

                iCurrCulpritCode =ObjGrid.rows[i].cells[6].children[0].selectedIndex;
                if (iCurrCulpritCode != 0) // check  culprit Code
                {
                    if (iCurrCulpritCode == iSelectedCulpritCode) 
                    {
                        alert("Culprit code is already selected for the Job code  '" + sCurrJobCode + " '");
                        ObjCulprit.selectedIndex = 0;
                        return false;
                    }
                }
            }

        }
    }
}


// check Defect Code Validation
function CheckDefectCodeValidation(ObjDefect) {
    var PcontainerName = '';
    var ObjGrid = null;
    var objRow = ObjDefect.parentNode.parentNode.childNodes;
    var sCurrJobCode = "";
    var sSelectedJobCode = "";        
    var iCurrRowIndex = ObjDefect.parentNode.parentNode.rowIndex;
    var iSelectedDefectCode = 0;
    var iCurDefectCode = 0;
    //debugger;
    if (ObjDefect.selectedIndex == 0)
     {
        return;
    }
    sSelectedJobCode = objRow[2].children[0].innerText;

    //Check Part Details;
    var iPartID = dGetValue(objRow[4].children[0].value);
    if (iPartID == 0) 
    {
        alert("Please select the Causal Part for the Job " + sSelectedJobCode);
        ObjDefect.selectedIndex = 0;
        return false;
    }
    //Check Culprit Code
    if (objRow[7].children[0].selectedIndex == 0) {
        alert("Please first select the Culprit code !");
        ObjDefect.selectedIndex = 0;
        return false;
    }

    //Get Defect Code ID
    iSelectedDefectCode = objRow[8].children[0].selectedIndex;
        
    // check Selected defect code is already selected for the job code
    PcontainerName = GetContainerName();
    //Megha 09102012
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //Megha 09102012

    // Commented by Shyamal as on 26022013 as per DMS Logic
//    ObjGrid = window.document.getElementById(PcontainerName + "JobDetailsGrid");
//    for (i = 1; i < ObjGrid.rows.length; i++)
//     {
//         if (i != iCurrRowIndex) 
//         {        
//            sCurrJobCode = ObjGrid.rows[i].cells[1].children[0].innerText;
//            iPartID = dGetValue(ObjGrid.rows[i].cells[2].children[0].value);
//            if (iPartID != 0) {

//                iCurDefectCode = ObjGrid.rows[i].cells[6].children[0].selectedIndex;
//                if (iCurDefectCode  != 0) // Check culprit Code
//                {
//                    if (iSelectedDefectCode == iCurDefectCode) 
//                    {
//                        alert("Defect code is already selected for the for the Job code  '" + sCurrJobCode + " '");
//                        ObjDefect.selectedIndex = 0;
//                        return false;
//                    }
//                }

//            }
//        }
//    }
}

// check Technical Code Validation
function CheckTechnicalCodeValidation(ObjTechnical) {
    var PcontainerName = '';
    var ObjGrid = null;
    var objRow = ObjTechnical.parentNode.parentNode.childNodes;
    var sCurrJobCode = "";
    var sSelectedJobCode = "";
    var iCurrRowIndex = ObjTechnical.parentNode.parentNode.rowIndex;    
    var iSelectedTechCode = 0;
    var iCurTechCode = 0;

    if (ObjTechnical.selectedIndex == 0) 
    {
        return;
    }
    sSelectedJobCode = objRow[1].children[0].innerText;

    //Check Part Details;
   var  iPartID = dGetValue(objRow[2].children[0].value);
    if (iPartID == 0) {
        alert("Please select the Causal Part for the Job " + sSelectedJobCode);
        ObjTechnical.selectedIndex = 0;
        return false;
    }
    
    //Check Culprit Code
    if (objRow[5].children[0].selectedIndex == 0) {
        alert("Please first select the Culprit code !");
        ObjTechnical.selectedIndex = 0;
        return false;
    }
    
    //Check Defect Code
    if (objRow[6].children[0].selectedIndex == 0) 
    {
        alert("Please first select the Defect code !");
        ObjTechnical.selectedIndex = 0;
        return false;
    }
    
    //Check Technical Code
    iSelectedTechCode = objRow[7].children[0].selectedIndex
    
    // check Selected Technical code is already selected for the job code
    PcontainerName = GetContainerName();
    //Megha 09102012
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //Megha 09102012
    // Commented by Shyamal as on 26022013 as per DMS Logic
//    ObjGrid = window.document.getElementById(PcontainerName + "JobDetailsGrid");
//    for (i = 1; i < ObjGrid.rows.length; i++) 
//    {
//        if (i != iCurrRowIndex)
//         {
//            sCurrJobCode = ObjGrid.rows[i].cells[1].children[0].innerText;
//            iPartID = dGetValue(ObjGrid.rows[i].cells[2].children[0].value);
//            if (iPartID != 0)
//             {
//                 iCurTechCode = ObjGrid.rows[i].cells[7].children[0].selectedIndex;
//                if (iCurTechCode != 0) // Check culprit Code
//                {
//                    if (iSelectedTechCode == iCurTechCode) {
//                        alert("Technical code is already selected for the job code '" + sCurrJobCode + " '");
//                        ObjTechnical.selectedIndex = 0;
//                        return false;
//                    }
//                }

//            }
//        }
//    }
}

// When user Click to delete the record
function SelectDeleteCheckboxForJob(ObjChkDelete) {
    var objRow = ObjChkDelete.parentNode.parentNode.childNodes;
    if (ObjChkDelete.checked) {
        if (confirm("Are you sure you want to delete this record?") == true) {
            ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
        }
        else {
            ObjChkDelete.checked = false;
            return false;
        }
    }
    else {
        ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';

    }
}

// When user Click on Cancel of Job Grid then clear the value of row
function ClearRowValueForJob(objCancelControl) {
    var objRow = objCancelControl.parentNode.parentNode.childNodes;
    var ObjControl;   
    
    //Set JobCodeID;
    objRow[1].childNodes[0].selectedIndex = 0;

    //SetPartID
    objRow[2].children[0].value = "";
    objRow[2].children[1].value = "";
    
    //SetPartNo
    objRow[3].children[1].style.display = "";
    objRow[3].children[0].value = '';
    objRow[3].children[0].style.display = "none";

    //SetPartName
    objRow[4].childNodes[0].value = '';
    //objRow[3].childNodes[0].style.display = "none";

    //Set Culprit Code
    objRow[5].childNodes[0].selectedIndex = 0;

    //Set Defect Code
    objRow[6].childNodes[0].selectedIndex = 0;

    //Set Technical Code
    objRow[7].childNodes[0].selectedIndex = 0;


    //SetNewLabel Display        
//    ObjControl = objRow[8].children[1];
//    if (ObjControl != null) ObjControl.style.display = "none";

    // To Calculate Jobcode Record Count
   SetJobCodeRecordCount();
}

// Set Total Job Code Record Count
function SetJobCodeRecordCount() 
{
    var ObjGrid;
    var iRecordCnt = 0;
    ObjGrid = document.getElementById("ContentPlaceHolder1_JobDetailsGrid");
    if (ObjGrid == null) return;
    var iPartID = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        //Set PartId;
        iPartID = dGetValue(ObjGrid.rows[i].cells[2].children[0].value);
        if (iPartID != 0) {
            iRecordCnt = iRecordCnt + 1;
        }
    }
    // To calculate Total Part  Count
    var ObjJobRecCount = document.getElementById("ContentPlaceHolder1_lblJobRecCnt");
    if (ObjJobRecCount != null) 
    {
        ObjJobRecCount.innerText = iRecordCnt;
    }
}