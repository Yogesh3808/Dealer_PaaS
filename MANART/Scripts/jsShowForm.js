//To Show Part Master
function ShowPartMaster(objNewPartLabel, sDealerId) {
    var PartDetailsValue;
    var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
    PartDetailsValue = window.showModalDialog("frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID, "List", "scrollbars=no,resizable=no,dialogWidth=400px,dialogHeight=300px");
    if (PartDetailsValue != null) {
        SetPartDetails(objNewPartLabel, PartDetailsValue);
    }
}
// Function is used to return value which is selected by user
function ReturnValue(objImgControl) {
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    var ReturnID = 0;
    ReturnID = objRow[2].innerText;
    window.returnValue = ReturnID;
    window.close();
}

// To Get Part Details from Part Master
//function ReturnPartMDetails(objImgControl) {
//    //debugger
//    var objRow = objImgControl.parentNode.parentNode.childNodes;
//    var ArrOfParts = new Array();
//    var sCellValue = "";


//    // Add Part ID        
//    sCellValue = objRow[2].children[0].innerText;
//    ArrOfParts.push(sCellValue);

//    // Add PartNo
//    sCellValue = objRow[3].children[0].innerText;
//    ArrOfParts.push(sCellValue);

//    // Add PartName
//    sCellValue = objRow[4].children[0].innerText;
//    ArrOfParts.push(sCellValue);

//    window.returnValue = ArrOfParts;
//    window.close();
//}
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
    
    // Add FOB Rate
    sCellValue = objRow[4].children[0].innerText;
    ArrOfParts.push(sCellValue);

    // Add Warratable
    sCellValue = objRow[5].children[0].innerText;
    ArrOfParts.push(sCellValue);

    // Add MOQ
    sCellValue = objRow[6].children[0].innerText;
    ArrOfParts.push(sCellValue);
    
  window.returnValue = ArrOfParts; 
  window.close(); 
}

// To Get Selected Labour Details
function ReturnLabourDetails()
{   
  
  var LabourID= window.document.getElementById("txtID").value;  
  var LabourCode = window.document.getElementById("1").value;  
  var LabourDescription =  window.document.getElementById("2").value;    
  var ManHrs =  window.document.getElementById("3").value; 
  var Rate =  window.document.getElementById("4").value;  
  var Total = parseFloat( Rate) * parseFloat( ManHrs);
  var ret=new Array( '',LabourID,LabourCode,LabourDescription,ManHrs,Rate,Total) ; 
  window.returnValue=ret; 
  window.close(); 
}

// To Get Part Details
function ReturnMultiPartDetails() {
    var gridView = null;
    gridView = document.getElementById('PartDetailsGrid');
    if (gridView == null) return;
    var ArrOfParts = new Array();
    var i;
    var rows = gridView.rows;
    var sCellValue = "";
    var bPartIsSelected = false;
    var gridCell = null;
    var iCntOfPart = 0;    
    for (i = 1; i < rows.length; i++) 
    {
        gridCell = gridView.children[0].rows[i].cells[0];
        bPartIsSelected = false;
        if (gridCell.childNodes[0].checked == true) bPartIsSelected = true;

        if (bPartIsSelected == true)// If user select the part
        {
            // Add Part ID
            gridCell = gridView.children[0].rows[i].cells[1];
            ArrOfParts[iCntOfPart] = new Array();
            sCellValue = gridCell.children[0].innerText;
            ArrOfParts[iCntOfPart].push(sCellValue);
            
            // Add PartNo
            gridCell = gridView.children[0].rows[i].cells[2];
            sCellValue = gridCell.children[0].innerText;
            ArrOfParts[iCntOfPart].push(sCellValue);

            // Add PartName
            gridCell = gridView.children[0].rows[i].cells[3];
            sCellValue = gridCell.children[0].innerText;
            ArrOfParts[iCntOfPart].push(sCellValue);
            
            // Add FOB Rate
            gridCell = gridView.children[0].rows[i].cells[4];
            sCellValue = gridCell.children[0].innerText;
            ArrOfParts[iCntOfPart].push(sCellValue);
            
            // Add Warratable
            gridCell = gridView.children[0].rows[i].cells[5];
            sCellValue = gridCell.children[0].innerText;
            ArrOfParts[iCntOfPart].push(sCellValue);


            iCntOfPart = iCntOfPart + 1;
        }
        
    }

    window.returnValue = ArrOfParts;
    window.close();
    return false;
}
// To Get Labour Details
function ReturnMultiLabourDetails() {


    var gridView = null;
    gridView = document.getElementById('LabourDetailsGrid');
    if (gridView == null) return;
    var ArrOfLabours = new Array();
    var i;
    var rows = gridView.rows;
    var sCellValue = "";
    var bLabourIsSelected = false;
    var gridCell = null;
    var iCntOfLabour = 0;
    for (i = 1; i < rows.length; i++) {
        gridCell = gridView.children[0].rows[i].cells[0];
        bLabourIsSelected = false;
        if (gridCell.childNodes[0].checked == true) bLabourIsSelected = true;

        if (bLabourIsSelected == true)// If user select the Labour
        {
            // Add Labour ID
            gridCell = gridView.children[0].rows[i].cells[1];
            ArrOfLabours[iCntOfLabour] = new Array();
            sCellValue = gridCell.children[0].innerText;
            ArrOfLabours[iCntOfLabour].push(sCellValue);

            // Add LabourNo
            gridCell = gridView.children[0].rows[i].cells[2];
            sCellValue = gridCell.children[0].innerText;
            ArrOfLabours[iCntOfLabour].push(sCellValue);

            // Add LabourName
            gridCell = gridView.children[0].rows[i].cells[3];
            sCellValue = gridCell.children[0].innerText;
            ArrOfLabours[iCntOfLabour].push(sCellValue);

            // Add ManHrs
            gridCell = gridView.children[0].rows[i].cells[4];
            sCellValue = gridCell.children[0].innerText;
            ArrOfLabours[iCntOfLabour].push(sCellValue);

            // Add Rate
            gridCell = gridView.children[0].rows[i].cells[5];
            sCellValue = gridCell.children[0].innerText;
            ArrOfLabours[iCntOfLabour].push(sCellValue);

            //Add Total
            gridCell = gridView.children[0].rows[i].cells[6];
            sCellValue = gridCell.children[0].innerText;
            ArrOfLabours[iCntOfLabour].push(sCellValue);


            iCntOfLabour = iCntOfLabour + 1;
        }

    }

    window.returnValue = ArrOfLabours;
    window.close();
    return false;
}
// To Get Return Claim Details
function ReturnClaimDetails(objImgControl)
 {

    var ID = 0;
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    
    // Add Claim ID
    ID = objRow[1].children[0].innerText;    

    window.returnValue = ID;
    window.close();
}

function ReturnFromForm() {
    window.close();
}

function ChkPartClick(objImgControl) {
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    //var txtparst = document.getElementById('txtPartIds');
    var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds').value;
    alert(objRow, txtparst);
    var ArrOfPartDtls;
    var removePartID;
    var sPartID = objRow[1].innerText;
    var sParNo = objRow[2].innerText;
    var sParName = objRow[3].innerText;
    var sFobRate = objRow[4].innerText;
    var Warratable = objRow[5].innerText;

    ArrOfPartDtls = sPartID + '<--' + sParNo + '<--' + sParName + '<--' + sFobRate + '<--' + Warratable;

    if (objImgControl.checked == true) {
        if (txtparst.value == "") {
            txtparst.value = ArrOfPartDtls;
        }
        else {
            txtparst.value = txtparst.value + '#' + ArrOfPartDtls;
        }

    } else {
        removePartID = txtparst.value;

        var afterRemove = "";
        var arr = removePartID.split("#");
        txtparst.value = "";
        var arrlen = arr.length;
        for (var i = 0; i < arrlen; i++) {
            if (arr[i] == ArrOfPartDtls) {
                // arr.splice(i, 1);

            }
            else {

                if (txtparst.value == "") {
                    txtparst.value = arr[i];
                }
                else {
                    txtparst.value = txtparst.value + '#' + arr[i];
                }
            }
        }
        // txtparst.value = arr;
    }
    return txtparst.value;
}

function ReturnMultiWPartDetails() {
    var txtparst = document.getElementById('txtPartIds').value;
    alert(txtparst);
    if (txtparst == "") {
        window.close();
        return;
    }
    
    if (txtparst != "") {
        var iCntOfPart = 0;
        var ArrOfParts = new Array();
        var arr = txtparst.split("#");
        for (var i = 0; i < arr.length; i++) {
            
                var ArrParts = new Array();
                ArrParts = arr[i].split("<--");

                // Add Part ID

                ArrOfParts[iCntOfPart] = new Array();
                ArrOfParts[iCntOfPart].push(ArrParts[0]);

                // Add PartNo

                ArrOfParts[iCntOfPart].push(ArrParts[1]);

                // Add PartName

                ArrOfParts[iCntOfPart].push(ArrParts[2]);

                // Add FOB Rate
                ArrOfParts[iCntOfPart].push(ArrParts[3]);

                // Add Warratable
                ArrOfParts[iCntOfPart].push(ArrParts[4]);
                iCntOfPart = iCntOfPart + 1;            
        }


        window.returnValue = ArrOfParts;
        //alert(ArrOfParts);
        window.close();
        return false;
    }
}

function ChkSpNDPPartClick(objImgControl) {
    debugger;
    var objID = $('#' + objImgControl.id);
    var objCol = objID[0].parentNode.parentNode;
   // var txtparst = document.getElementById("ContentPlaceHolder1_txtPartIds");
    //Changed by Vikram Date 17.06.2016
    //objImgControl.parentNode.parentNode.childNodes;
    //var objCol = objImgControl.parentNode.parentNode
    var txtparst = document.getElementById('txtPartIds');
    //var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds');
   
    var ArrOfPartDtls='';
    var removePartID; 
//    var sPartID = objRow[1].innerText;
//    var sParFOBRt = objRow[2].innerText;
//    var sParMOQ = objRow[3].innerText;
//    var sParNo = objRow[4].innerText;
//    var sParName = objRow[5].innerText;
    //    var sNDPRate = objRow[6].innerText;
    
    //Changes done for jobcard part selection solution for part type tag not get selected here
    //for (i = 1; i < objCol.cells.length - 1; i++) {
    for (i = 1; i < objCol.cells.length; i++) {
        if (i == objCol.cells.length - 1)
            ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML;
        else
            ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML +'<--';
    }

    //ArrOfPartDtls = sPartID + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;
    //ArrOfPartDtls = sPartID + '<--' + sParFOBRt + '<--' + sParMOQ + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;

    if (objImgControl.checked == true) {
        if (txtparst.value == "") {
            txtparst.value = ArrOfPartDtls;
        }
        else {
            txtparst.value = txtparst.value + '#' + ArrOfPartDtls;
        }

    } else {
        removePartID = txtparst.value;

        var afterRemove = "";
        var arr = removePartID.split("#");
        txtparst.value = "";
        var arrlen = arr.length;
        for (var i = 0; i < arrlen; i++) {
            if (arr[i] == ArrOfPartDtls) {
                // arr.splice(i, 1);

            }
            else {

                if (txtparst.value == "") {
                    txtparst.value = arr[i];
                }
                else {
                    txtparst.value = txtparst.value + '#' + arr[i];
                }
            }
        }
        // txtparst.value = arr;
    }
    return txtparst.value;

}

function ChkSpNDPPartClickOnJobcard(objImgControl) {
    debugger;
    //var objID = $('#' + objImgControl.id);
    //var objCol = objID[0].parentNode.parentNode;
    //var txtparst = document.getElementById("txtPartIds");
    //Changed by Vikram
    objImgControl.parentNode.parentNode.childNodes;
    var objCol = objImgControl.parentNode.parentNode
    //var txtparst = document.getElementById('txtPartIds');
    var txtparst = document.getElementById('txtPartIds');

    var ArrOfPartDtls = '';
    var removePartID;
    //    var sPartID = objRow[1].innerText;
    //    var sParFOBRt = objRow[2].innerText;
    //    var sParMOQ = objRow[3].innerText;
    //    var sParNo = objRow[4].innerText;
    //    var sParName = objRow[5].innerText;
    //    var sNDPRate = objRow[6].innerText;

    //Changes done for jobcard part selection solution for part type tag not get selected here
    //for (i = 1; i < objCol.cells.length - 1; i++) {
    for (i = 1; i < objCol.cells.length; i++) {
        if (i == objCol.cells.length - 1)
            ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML;
        else
            ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML + '<--';
    }

    //ArrOfPartDtls = sPartID + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;
    //ArrOfPartDtls = sPartID + '<--' + sParFOBRt + '<--' + sParMOQ + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;

    if (objImgControl.checked == true) {
        if (txtparst.value == "") {
            txtparst.value = ArrOfPartDtls;
        }
        else {
            txtparst.value = txtparst.value + '#' + ArrOfPartDtls;
        }

    } else {
        removePartID = txtparst.value;

        var afterRemove = "";
        var arr = removePartID.split("#");
        txtparst.value = "";
        var arrlen = arr.length;
        for (var i = 0; i < arrlen; i++) {
            if (arr[i] == ArrOfPartDtls) {
                // arr.splice(i, 1);

            }
            else {

                if (txtparst.value == "") {
                    txtparst.value = arr[i];
                }
                else {
                    txtparst.value = txtparst.value + '#' + arr[i];
                }
            }
        }
        // txtparst.value = arr;
    }
    return txtparst.value;
}

function ChkLabourClick(objImgControl) {
    debugger;
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    //var txtparst = document.getElementById('txtPartIds');
    //var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds');
    //var objLbrSelection = document.getElementById('ContentPlaceHolder1_DrpLabourSelect');
    var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds_Labour');
    var objLbrSelection = document.getElementById('ContentPlaceHolder1_DrpLabourSelect');
   // var txtparst = document.getElementById('txtPartIds');
    //var objLbrSelection = document.getElementById('DrpLabourSelect');
    
    var ArrOfLabDtls;
    var removePartID;
    //var sLabourID = objRow[1].innerText;
    //var sLabourCode = objRow[2].innerText;
    //var sLabourName = objRow[3].innerText;
    //var sLabManHrs = objRow[4].innerText;
    //var sLabRate = objRow[5].innerText;
    //var sLabTotal = objRow[6].innerText;
    objLbrSelection.disabled = true;

    var sLabourID = objRow[2].childNodes[1].innerText;
    var sLabourCode = objRow[3].childNodes[1].innerText;
    var sLabourName = objRow[4].childNodes[1].innerText;
    var sLabManHrs = objRow[5].childNodes[1].innerText;
    var sLabRate = objRow[6].childNodes[1].innerText;
    var sLabTotal = objRow[7].childNodes[1].innerText;
    var sLabTag = objRow[8].childNodes[1].innerText;
    var sGroupCode = objRow[9].childNodes[1].innerText;
    var sTax = objRow[10].childNodes[1].innerText;
    var sTax1 = objRow[11].childNodes[1].innerText;
    var sTax2 = objRow[12].childNodes[1].innerText;
    var estDtlID = objRow[13].childNodes[1].innerText;
    //debugger;
    var sMiscDesc = objRow[13].childNodes[3].innerText;
    var sOutSubDesc = objRow[13].childNodes[5].innerText;

    ArrOfLabDtls = sLabourID + '<--' + sLabourCode + '<--' + sLabourName + '<--' + sLabManHrs + '<--' + sLabRate + '<--' + sLabTotal + '<--' + sLabTag + '<--' + sGroupCode
    + '<--' + sTax + '<--' + sTax1 + '<--' + sTax2 + '<--' + estDtlID + '<--' + sMiscDesc + '<--' + sOutSubDesc;

    if (objImgControl.checked == true) {
        if (txtparst.value == "") {
            txtparst.value = ArrOfLabDtls;
        }
        else {
            txtparst.value = txtparst.value + '#' + ArrOfLabDtls;
        }

    } else {
        removePartID = txtparst.value;

        var afterRemove = "";
        var arr = removePartID.split("#");
        txtparst.value = "";
        var arrlen = arr.length;
        for (var i = 0; i < arrlen; i++) {
            if (arr[i] == ArrOfLabDtls) {
                // arr.splice(i, 1);

            }
            else {

                if (txtparst.value == "") {
                    txtparst.value = arr[i];
                }
                else {
                    txtparst.value = txtparst.value + '#' + arr[i];
                }
            }
        }
        // txtparst.value = arr;
    }
    return txtparst.value;
}

function ReturnMultiWLabourDetails() {
    var txtparst = document.getElementById('txtPartIds').value;
    if (txtparst == "") {
        window.close();
        return;
    }

    if (txtparst != "") {
        var iCntOfPart = 0;
        var ArrOfLabours = new Array();
        var arr = txtparst.split("#");
        for (var i = 0; i < arr.length; i++) {

            var ArrLabours = new Array();
            ArrLabours = arr[i].split("<--");

            // Add Labour ID

            ArrOfLabours[iCntOfPart] = new Array();
            ArrOfLabours[iCntOfPart].push(ArrLabours[0]);

            // Add Labour Code

            ArrOfLabours[iCntOfPart].push(ArrLabours[1]);

            // Add LabourName

            ArrOfLabours[iCntOfPart].push(ArrLabours[2]);

            // Add Labour Man Hrs
            ArrOfLabours[iCntOfPart].push(ArrLabours[3]);

            // Add Labour Rate
            ArrOfLabours[iCntOfPart].push(ArrLabours[4]);
            
            // Add Labour Total
            ArrOfLabours[iCntOfPart].push(ArrLabours[5]);
            
            iCntOfPart = iCntOfPart + 1;
        }


        window.returnValue = ArrOfLabours;
        //alert(ArrOfParts);
        window.close();
        return false;
    }
}
//Sujata 27012011
