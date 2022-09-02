// To Check Dealer Enter Qty Greater Than Total Qty
function bCheckDealerQtyValidation(event,ObjQtyControl) 
{
    if (CheckForTextBoxLostFocus(event, ObjQtyControl, '5') == false) 
    {
        ObjQtyControl.value = '';
        return;
    }
    else 
    {
        ObjQtyControl.value = parseInt(ObjQtyControl.value);
    }
    
    // Ge tObject of the Row
    var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

    var iTotalQty = 0;
    var iDealerQty=0
    var iColIndex = 0;
    iDealerQty = parseInt(ObjQtyControl.value);
    iColIndex = ObjQtyControl.parentNode.cellIndex;
    iTotalQty = parseInt(objRow[iColIndex - 1].childNodes[0].innerHTML);
//    Sujata 10022011
//    if (iDealerQty == 0) {
//        if (iTotalQty != 0) {
//            alert("Please Enter The Quanity!");
//            ObjQtyControl.value = iTotalQty;
//        }
//    }
//    Sujata 10022011
    CalculateDealerQtyTotal(ObjQtyControl);
}

// To Show Indent
function ShowIndent(ObjImg) 
{    
    var iIndID = 0;    
    var objRow = ObjImg.parentNode.parentNode.childNodes;
    
    // Get climaid
    iIndID = dGetValue(objRow[2].children[0].innerText);
    var randomNum = Math.random() * 5;
    var sUrl = "/AUTODMS/Forms/Indent/frmIndentProcessing.aspx?IndID=" + iIndID + "&Random=" + randomNum;
    window.showModalDialog(sUrl, "List", "dialogWidth:1500px;dialogHeight:800px;status:yes;help:no;");
    CollectGarbage();    
    //window.location.reload();
    return true;
}


// To Check VECV User Entered Qty Greater Than Dealer Entered Qty
function bCheckVECVQtyValidation(event, ObjQtyControl) {
    if (CheckForTextBoxLostFocus(event, ObjQtyControl, '5') == false) {
        ObjQtyControl.value = '';
        return;
    }
    else {
        ObjQtyControl.value = parseInt(ObjQtyControl.value);
    }

    // Calculate Line Level Part amount
    var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

    var iVECVQty = 0;
    var iDealerQty = 0
    var iColIndex = 0;
    iVECVQty = parseInt(ObjQtyControl.value);
    iColIndex = ObjQtyControl.parentNode.cellIndex;
    iDealerQty = parseInt(objRow[iColIndex - 1].childNodes[0].innerHTML);
    if (iVECVQty == 0) 
    {
        if (iDealerQty != 0) 
        {
            alert("Please Enter The Quantity!");
            ObjQtyControl.value = iDealerQty;
        }
    }
    CalculateVECVQtyTotal(ObjQtyControl);
    //Sujata 10022011
    //CalculateM1QtyTotal(ObjQtyControl);
    //Sujata 10022011
}

function CheckForConfirm(ID) {
    if (CheckDepoIndBeforeSave() == true) {
        if (confirm("Are you sure, you want to submit the claim?") == true)
            return true;
        else
            return false;
    }
    else
        return false;
}
// Check Bofore Submit
function CheckIndBeforeSubmit() 
{
    if (CheckDepoIndBeforeSave() == true) {
        if (confirm("Are you sure, you want to submit the claim?") == true)
            return true;
        else
            return false;
    }
    else
        return false;
}

// Check Bofore Save Depo Indent
function CheckDepoIndBeforeSave() {
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_ModelDetailsGrid");
    if (ObjGrid == null)
        ObjGrid = window.document.getElementById("ModelDetailsGrid");
    if (ObjGrid == null) return false;
    for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {       
        objtxtControl = ObjGrid.rows[i].cells[2].childNodes[0];
        if (objtxtControl.value == "0") {
            alert("Please Select Model Code at line " + (i - 1));
            return false;
            break;
        }

    }
    return true;   
}

// To Exist
function CloseIndentForm() {
    window.returnvalue = 1;
    window.close();    
    return true;
}

// To Calculate Dealer total Qty of the Month
function CalculateDealerQtyTotal(ObjQtyControl)
 {
     // Ge tObject of the Row
     var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

     var ParentCntrl= ObjQtyControl.id.substring(0, ObjQtyControl.id.lastIndexOf("_"))
          
     var iDealerQty = 0
     //var iColIndex = 11;
     var iTotalQty = 0;
     
     //Get Total Of The 1 Week
     iDealerQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW1DealerIndQty").value);
     iTotalQty = iTotalQty + iDealerQty;
     //iColIndex = iColIndex + 4;


     //Get Total Of The 2 Week
     iDealerQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW2DealerIndQty").value);
     iTotalQty = iTotalQty + iDealerQty;
     //iColIndex = iColIndex + 4;

     //Get Total Of The 3 Week
     iDealerQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW3DealerIndQty").value);
     iTotalQty = iTotalQty + iDealerQty;
     //iColIndex = iColIndex + 4;

     //Get Total Of The 4 Week
     if (window.document.getElementById(ParentCntrl + "_txtW4DealerIndQty") != null) {
         iDealerQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW4DealerIndQty").value);
         iTotalQty = iTotalQty + iDealerQty;
     }
     //iColIndex = iColIndex + 4;   


     //Get Total Of The 5 Week
     if( window.document.getElementById(ParentCntrl + "_txtW5DealerIndQty")!=null)
     {
         iDealerQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW5DealerIndQty").value); ;
     iTotalQty = iTotalQty + iDealerQty;
     }
     else
     {
        //iColIndex = iColIndex - 4;
     }

     //Set Total Of The all Week
     //iColIndex = iColIndex + 2;
     window.document.getElementById(ParentCntrl + "_lblM0DealerQty").innerHTML = iTotalQty;
     window.document.getElementById(ParentCntrl + "_hdnM0DealerQty").value = iTotalQty;
 }

 // To Calculate VECV total Qty of the Month
 function CalculateVECVQtyTotal(ObjQtyControl) {
     // Ge tObject of the Row
     var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
     var ParentCntrl = ObjQtyControl.id.substring(0, ObjQtyControl.id.lastIndexOf("_"))
     var iVECVQty = 0
     //var iColIndex = 12;
     var iTotalQty = 0;

     //Get Total Of The 1 Week
     //iVECVQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iVECVQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW1VECVQty").value);
     iTotalQty = iTotalQty + iVECVQty;
     //iColIndex = iColIndex + 5;


     //Get Total Of The 2 Week
     //iVECVQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iVECVQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW2VECVQty").value);
     iTotalQty = iTotalQty + iVECVQty;
     //iColIndex = iColIndex + 5;

     //Get Total Of The 3 Week
     //iVECVQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iVECVQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW3VECVQty").value);
     iTotalQty = iTotalQty + iVECVQty;
     //iColIndex = iColIndex + 5;

     //Get Total Of The 4 Week
     //iVECVQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iVECVQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW4VECVQty").value);
     iTotalQty = iTotalQty + iVECVQty;
     //iColIndex = iColIndex + 5;


//     //Get Total Of The 5 Week
//     if (objRow[iColIndex].childNodes[0].id.lastIndexOf("txtW5VECVQty")!=-1) {
//         iDealerQty = parseInt(objRow[iColIndex].childNodes[0].value);
//         iTotalQty = iTotalQty + iDealerQty;
//     }
//     else
     //         iColIndex = iColIndex - 5;

     //iVECVQty = parseInt(objRow[iColIndex].childNodes[0].value);
     if (window.document.getElementById(ParentCntrl + "_txtW5VECVQty") != null) {
         iVECVQty = parseInt(window.document.getElementById(ParentCntrl + "_txtW5VECVQty").value);
         iTotalQty = iTotalQty + iVECVQty;
     }
     //iColIndex = iColIndex + 5;

     //Set Total Of The all Week
     //iColIndex = iColIndex + 3;
     //objRow[iColIndex].childNodes[0].innerHTML = iTotalQty;
     window.document.getElementById(ParentCntrl + "_lblM0VECVQty").innerHTML = iTotalQty;
     //objRow[iColIndex].childNodes[2].value = iTotalQty;
     window.document.getElementById(ParentCntrl + "_hdnM0VECVQty").value = iTotalQty;
     
 }

 function CalculateM1QtyTotal(ObjQtyControl) {
     // Ge tObject of the Row
     var objRow = ObjQtyControl.parentNode.parentNode.childNodes;
     var ParentCntrl = ObjQtyControl.id.substring(0, ObjQtyControl.id.lastIndexOf("_"))
     var iDealerQty = 0
     var iColIndex = 8;
     var iTotalQty = 0;

     //Get Total Of The 1 Week
     iDealerQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iTotalQty = iTotalQty + iDealerQty;
     iColIndex = iColIndex + 5;


     //Get Total Of The 2 Week
     iDealerQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iTotalQty = iTotalQty + iDealerQty;
     iColIndex = iColIndex + 5;

     //Get Total Of The 3 Week
     iDealerQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iTotalQty = iTotalQty + iDealerQty;
     iColIndex = iColIndex + 5;

     //Get Total Of The 4 Week
     iDealerQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iTotalQty = iTotalQty + iDealerQty;
     iColIndex = iColIndex + 5;


     //Get Total Of The 5 Week
     iDealerQty = parseInt(objRow[iColIndex].childNodes[0].value);
     iTotalQty = iTotalQty + iDealerQty;

     //Set Total Of The all Week
     iColIndex = iColIndex + 3;
     objRow[iColIndex].childNodes[0].value = iTotalQty;
 }