//To Show Warranty Claim
function ShowCouponClaim(ObjImg) {
    //debugger;
    var PartDetailsValue;
    var iClaimID = 0;
    var sRequestOrClaim = '';
    var objRow = ObjImg.parentNode.parentNode.childNodes;
    // Get climaid
    iClaimID = dGetValue(objRow[3].children[0].innerText);
    iClaimTypeID = dGetValue(objRow[5].children[0].innerText);
    sRequestOrClaim = document.getElementById('ContentPlaceHolder1_txtRequestOrClaim').value;


    //PartDetailsValue = window.showModalDialog("/DCS/Forms/Warranty/frmWarrantyClaimProcessing.aspx?ClaimID=" + iClaimID, "NO", "scrollbars=yes,resizable=yes,dialogWidth=100%,dialogHeight=1000px");
    PartDetailsValue = window.showModalDialog("../Coupon/frmCouponClaimProcessing.aspx?ClaimID=" + iClaimID + "&RequestOrClaim=" + sRequestOrClaim, "List", "dialogWidth:1350px;dialogHeight:850px;status:no;help:no");
    if (PartDetailsValue != null) {
        //SetPartDetails(objNewPartLabel, PartDetailsValue);
    }
}
// To Close Warranty Prossesing Form
function CloseCouponClaimProsseingWindow() {
    window.close();
}