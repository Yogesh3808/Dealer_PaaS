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