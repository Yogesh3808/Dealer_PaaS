
function ShowClaimRequestDtls(ClaimID) {
    window.showModalDialog("../Warranty/frmWarrantyRequestForClaim.aspx?ClaimID=" + ClaimID, "List", "dialogHeight: 500px; dialogWidth: 1170px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    //window.location.reload();
    return false;
}   

//To Show Warranty Claim
function ShowWarrantyClaim(ObjImg) {
    //debugger;
    var PartDetailsValue;
    var iClaimID = 0;
    var sRequestOrClaim = '';
    var objID = $('#' + ObjImg.id);
    var objRow = objID[0].parentNode.parentNode;
    // Get climaid
    iClaimID = dGetValue(objRow.cells[2].children[0].innerHTML);
    iClaimTypeID = dGetValue(objRow.cells[7].children[0].innerHTML);
    sRequestOrClaim = document.getElementById('ContentPlaceHolder1_txtRequestOrClaim').value;    

    //PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Warranty/frmWarrantyClaimProcessing.aspx?ClaimID=" + iClaimID, "NO", "scrollbars=yes,resizable=yes,dialogWidth=100%,dialogHeight=1000px");
    PartDetailsValue = window.showModalDialog("../Warranty/frmWarrantyClaimProcessing.aspx?ClaimID=" + iClaimID + "&RequestOrClaim=" + sRequestOrClaim, "List", "dialogWidth:1350px;dialogHeight:850px;status:no;help:no");
    if (PartDetailsValue != null) {
        //SetPartDetails(objNewPartLabel, PartDetailsValue);
    }
}
//To Show Warranty Claim
function ShowJobDetails(objControl)
 {
     //debugger;
     var JobDetails;
     var iJobId, iClaimID;
     var objID = $('#' + objControl.id);
     var objRow = objID[0].parentNode.parentNode;
     var sRequestOrClaim = '';
     // Get Rate
     iJobId = dGetValue(objRow.cells[0].children[0].innerHTML);
     iClaimID =dGetValue(document.getElementById('txtID').value); 
     sRequestOrClaim = document.getElementById('txtRequestOrClaim').value;
     
    //PartDetailsValue = window.showModalDialog("/AUTODMS/Forms/Warranty/frmWarrantyClaimProcessing.aspx?ClaimID=" + iClaimID, "NO", "scrollbars=yes,resizable=yes,dialogWidth=100%,dialogHeight=1000px");
     JobDetails = window.showModalDialog("../Warranty/frmJobDetails.aspx?RequestOrClaim=" + sRequestOrClaim + "&ClaimID=" + iClaimID + "&JobID=" + iJobId, "List", "dialogWidth:1350px;dialogHeight:850px");
    if (JobDetails != null) {
        //SetJobDetails(objControl,iJobId, JobDetails);
    }
}


function SetJobDetails(ObjControl,iJobId, JobDetails) 
{
    var ObjGrid = ObjControl.parentNode.parentNode.parentNode.parentNode;    
    var dClaimAmount = 0;
    var dDeductedAmount = 0;
    var dDeductedPercent = 0;
    var dAcceptedAmount = 0;
    var iArrayCnt=0;
    var iCurrJobId = 0;
    iJobId = parseInt(iJobId);
    for (i = 1; i < ObjGrid.rows.length; i++) {
        iCurrJobId = parseInt(ObjGrid.rows[i].cells[0].innerText);
        if (iCurrJobId == iJobId) {
            // Get Claim Amount
            dClaimAmount = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
            
            // accepted amount return from Job details
            dAcceptedAmount  =dGetValue(JobDetails[iArrayCnt]);    
            iArrayCnt =iArrayCnt +1;

            dDeductedAmount = RoundupValue(dClaimAmount - dAcceptedAmount)


            // calculate Deducted Percent
            dDeductedPercent = dGetValue((dDeductedAmount * 100)/dClaimAmount);

            //Set Deduction Percent
            ObjGrid.rows[i].cells[4].children[0].value = RoundupValue(dDeductedPercent);            
            
            // set Deducted Amount
             ObjGrid.rows[i].cells[5].children[0].value = RoundupValue(dDeductedAmount);            
            
            // Set Accepted Amount.
             ObjGrid.rows[i].cells[6].children[0].value = RoundupValue(dAcceptedAmount);
        }
    }
}

//Check Before Save
function CheckBeforeSaveRecord() 
{
    if (confirm("Have you saved the document?") == true)
        return true;
    else
        return false;
}


//Check Before Approve
function CheckBeforeApproveRecord() {
    //if (CheckBeforeSaveRecord() == false) return false;
    //debugger;
    if (CheckFileAttachment() == false) return false; 
    if (CheckJobCodeBeforeSubmit() == false) return false;       
    if (CheckRemarkIsEnter() == false) return false;    
    if (confirm("Are you sure, you want to approve the document?") == true)
        return true;
    else
        return false;
}

// To Check REamrk is Entered or not
function CheckRemarkIsEnter() {
    var ObjRemark;
    // Check CSM REmark
    ObjRemark = document.getElementById("txtCSMRemark");
    if (ObjRemark != null) 
    {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check ASM REmark
    ObjRemark = document.getElementById("txtASMRemark");
    if (ObjRemark != null) {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check RSM REmark
    ObjRemark = document.getElementById("txtRSMRemark");
    if (ObjRemark != null) {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim()== "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check Head REmark
    ObjRemark = document.getElementById("txtHeadRemark");
    if (ObjRemark != null) {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check Head Retail Remark
    //ObjRemark = document.getElementById("txtHeadRetailRemark");
    //if (ObjRemark != null) {
    //    if (ObjRemark.readOnly == false) {
    //        if (ObjRemark.value.trim() == "") {
    //            alert("Please Enter The Remarks !.");
    //            //ObjRemark.focus();
    //            return false;
    //        }
    //    }
    //}
    // Check Head Sale Marketing After Marketing Remark
    //ObjRemark = document.getElementById("txtHeadSaleMkgAfterMkgRemark");
    //if (ObjRemark != null) {
    //    if (ObjRemark.readOnly == false) {
    //        if (ObjRemark.value.trim() == "") {
    //            alert("Please Enter The Remarks !.");
    //            //ObjRemark.focus();
    //            return false;
    //        }
    //    }
    //}
    // Check SHQ Resource REmark
    ObjRemark = document.getElementById("txtSHQRRemark");
    if (ObjRemark != null) {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check SHQ REmark
    ObjRemark = document.getElementById("txtSHQRemark");
    if (ObjRemark != null) {
        if (ObjRemark.readOnly == false) {
            if (ObjRemark.value.trim() == "") {
                alert("Please Enter The Remarks !.");
                //ObjRemark.focus();
                return false;
            }
        }
    }
    // Check SA Resource REmark
    //ObjRemark = document.getElementById("txtSAResourceRemark");
    //if (ObjRemark != null) {
    //    if (ObjRemark.readOnly == false) {
    //        if (ObjRemark.value.trim() == "") {
    //            alert("Please Enter The Remarks !.");
    //            //ObjRemark.focus();
    //            return false;
    //        }
    //    }
    //}
}
// check Reason is seleted or not
function CheckReasonIsSelected()
 {
     var ObjReason;
     ObjReason = document.getElementById("drpReason");
     if (ObjReason != null) {
         if (ObjReason.selectedIndex == 0)
          {
                alert("Please Select The Reason For Reject/Return !.");
                ObjReason.focus();
                return false;
            }
        }
        return true;
    }

    function CheckBeforeRecord(objthis) {        
        var IsReject = objthis.parentNode.childNodes[3].innerText;
        if (IsReject=='R') {
            //if (CheckBeforeSaveRecord() == false) return false;
            if (CheckFileAttachment() == false) return false;
            if (CheckJobCodeBeforeSubmit() == false) return false;
            if (CheckRemarkIsEnter() == false) return false;
            if (CheckReasonIsSelected() == false) return false;
            if (confirm("Are you sure, you want to return the document?") == true)
                return true;
            else
                return false;
        }
        else if (IsReject == 'J') {
            if (CheckFileAttachment() == false) return false;
            if (CheckJobCodeBeforeSubmit() == false) return false;
            if (CheckRemarkIsEnter() == false) return false;
            if (CheckReasonIsSelected() == false) return false;
            if (confirm("Are you sure, you want to reject the document?") == true)
                return true;
            else
                return false;
        }
    }

//Check Before Approve
function CheckBeforeReturnRecord() {
    //if (CheckBeforeSaveRecord() == false) return false;
    if (CheckFileAttachment() == false) return false;
    if (CheckRemarkIsEnter() == false) return false;
    if (CheckReasonIsSelected() == false) return false;
    if (confirm("Are you sure, you want to return the document?") == true)
        return true;
    else
        return false;
}

//Check Before Reject
function CheckBeforeRejectRecord() {
    //    if (CheckBeforeSaveRecord() == false) return false;
    if (CheckFileAttachment() == false) return false;
    if (CheckRemarkIsEnter() == false) return false;
    if (CheckReasonIsSelected() == false) return false;
    if (confirm("Are you sure, you want to reject the document?") == true)
        return true;
    else
        return false;
}

function rejectwarrantyCLaim() { 

}

// Check Bofore Submit
function CheckWCBeforeSubmit() {
    if (CheckFileAttachment() == false) return false;
    if (CheckJobCodeBeforeSubmit() == false) return false;
    if (CheckRemarkIsEnter() == false) return false;    
    if (confirm("Are you sure, you want to submit the claim?") == true)
        return true;
    else
        return false;
}
// Check JobCode Bofore Submit
function CheckJobCodeBeforeSubmit() {
    var hdnJobCode = document.getElementById("hdnJobCode");
    if (hdnJobCode.value != "") {
        alert("Culprit Code OR Defect Code not selected in JobCode " + hdnJobCode.value + ".")
        return false;
    }
}

// Check File Validation
function CheckFileAttachment() {

    var iFileExtension = 0;
    var iSameFileCnt = 0;
    var iFileMax = 0;
    var sSameFile = "";

    var Elements = document.getElementsByTagName("input");
    for (var i = 0; i < Elements.length; i++) {
        if (Elements[i].type == 'file') {
            var File = Elements[i]

            var splFileName = File.value.split('.');
            var splFile = File.value.split('/')
            if (splFileName.length > 1)
                if (splFileName[splFileName.length - 1].toUpperCase() == "XLS" || splFileName[splFileName.length - 1].toUpperCase() == "XLSX" || splFileName[splFileName.length - 1].toUpperCase() == "DOC" || splFileName[splFileName.length - 1].toUpperCase() == "DOCX" || splFileName[splFileName.length - 1].toUpperCase() == "JPEG" || splFileName[splFileName.length - 1].toUpperCase() == "JPG") {
                    if (sSameFile != splFile[splFile.length - 1]) {
                        sSameFile = splFile[splFile.length - 1];
                    }
                    else
                        iSameFileCnt = iSameFileCnt + 1;      
                    

            }
            else {
                iFileExtension = iFileExtension + 1;
            }
        }
    }
    if (iFileExtension > 0) {
        alert("Invalid File Type(Only .xls,.xlsx,.doc,.docx,.jpeg,.jpg Allowed).")
        return false;
    }
    if (iSameFileCnt > 0) {
        alert("Same File Not Allowed to Upload.")
        return false;
    }
}
        
            
               



//Check Before Reject OR Return
function CheckRejectReturnApplicable(RejectReturn) {  
    var hdnRejectCount = document.getElementById("hdnRejectCount");
    var hdnReturnCount = document.getElementById("hdnReturnCount");
    if (RejectReturn == 'R') {
        if (hdnRejectCount.value == "1" || hdnReturnCount.value == "3") {
            alert("Cannot Return Claim : \n Frequency for rejection or return for this claim is over")
            return false;
        }
    }
    else if (RejectReturn == 'J') {
        if (hdnRejectCount.value == "1" || hdnReturnCount.value == "3") {
            alert("Cannot Reject Claim : \n Frequency for rejection or return for this claim is over")
            return false;
        }
    }

}

//Check Multi Text Length on key press
function ToCheckTextLength(Obj, MaxLength) {
    if (Obj != null) {
        var sTextValue = Obj.value.trim();
        if (Obj.value.trim().length > MaxLength) {
            alert("Remark Length should not be exceed to 500 character");
            Obj.focus();
            Obj.value = sTextValue.trim().slice(0, MaxLength);
            return false;
        }
    }
}

function trim(str) {
    //            if (!str || typeof str != 'string')
    //                return null;

    return str.replace(/^[\s]+/, '').replace(/[\s]+$/, '').replace(/[\s]{2,}/, ' ');
}