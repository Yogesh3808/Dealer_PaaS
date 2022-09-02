// To Get Warranty Claim Details
//function ReturnWarrantyClaimDetails() {        
//    var gridView = null;
//    gridView = document.getElementById('ClaimsDetailsGrid');    
//    if (gridView == null) return;
//    var ArrOfParts = new Array();
//    var i;
//    var rows = gridView.rows;
//    var sCellValue = "";
//    var bPartIsSelected = false;
//    var gridCell = null;
//    var iCntOfPart = 0; 
//    var strWarrantyClaims = "";
//    for (i = 1; i < rows.length; i++) 
//    {
//        gridCell = gridView.children[0].rows[i].cells[0];
//        bPartIsSelected = false;
//        if (gridCell.childNodes[0].checked == true) bPartIsSelected = true;

//        if (bPartIsSelected == true)// If user select the part
//        {
//            gridCell = gridView.children[0].rows[i].cells[3];             
//            sCellValue = gridCell.children[0].innerText;
//            strWarrantyClaims = strWarrantyClaims + sCellValue  + ",";
//            iCntOfPart = iCntOfPart + 1;
//        }        
//    }
//    window.returnValue = strWarrantyClaims;    
//    window.close();
//     return false;
//}

   
////check before Save
//function CheckForSaveFPDA(ID) 
//{
//     if (ID == 27) //Check VehicleIN Function
//    {
//        if (ValidateVehicleIN()== false) {
//            return false;
//        }
//    }
//    
// }   
// 
// 
// // To Check Vehicle RFP Vlaidation 
//function ValidateFPDA() {
//    var ObjGrid = null;
//    ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
//    if (ObjGrid == null) return;    
//    var objtxtControl;
//    var iCountDel = 0;
//    var ObjControl;    
//    var sMessage = "";
//    for (var i = 1; i < ObjGrid.rows.length; i++) 
//    {
//    }
//   }

//

//Check Part Accepted Qty on Accepted Qty Changed
function CheckFPDAPartAcceptedQty(event, ObjQtyControl) {
    ////debugger;  
    var dClaimedQty = 0;
    var dAcceptedQty = 0;
    
    // Calculate Line Level Part amount
    var objRow = ObjQtyControl.parentNode.parentNode.childNodes;

    // Get Claimed Qty
    dClaimedQty = dGetValue(objRow[5].childNodes[1].innerText);   

    // Get Accepted Qty
    dAcceptedQty = dGetValue(ObjQtyControl.value);


    if (dAcceptedQty > dClaimedQty) {
        ObjQtyControl.focus();
        alert(" Accepted quantity should not be greater than Part quantity.");
        ObjQtyControl.value = dClaimedQty;                
        return false;
    }   

}
    
