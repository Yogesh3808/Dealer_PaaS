function PartClaimSaveT() {
    hdn3PLStatus = document.getElementById("hdn3PLStatus");
    hdnUserRoleId = document.getElementById("hdnUserRoleId");
    hdn3PLStatus.value = '';
    if (hdnUserRoleId.value == 11)//For PartClaim Manager
    {
        if (confirm("Do you want to go to 3PL?") == true) {
            hdn3PLStatus.value = '3PL';
        }
        else
            if (confirm("Do you want to save deatils ?") == false)
            return false;
    }
    else {
        if (hdnUserRoleId.value == 12) {//For 3PL
            var txtASMRemark = document.getElementById("txtASMRemark");
            var hdnUserRoleId = document.getElementById("hdnUserRoleId");
            if (txtASMRemark.value == '' && hdnUserRoleId.value == '12') {
                alert("Please Enter Remarks !");
                return false;
            }
        }
    }
}

function PartClaim(sStatus) {
    //debugger;
    var bReturn;
    TargetBaseControl = document.getElementById("GridPartClaimDetail");
    var txtClaimType = document.getElementById("txtClaimType");
    var Inputs;
    //Sujata 01022011
    var iROW;

    iROW = 0;
    if (txtClaimType.value == "Wrong Supply") {
        for (var n = 1; n < TargetBaseControl.rows.length; ++n) {
            MRNApprovedQty = dGetValue(TargetBaseControl.rows[n].cells[14].childNodes[0].value);
            if (MRNApprovedQty > 0) {
                iROW = iROW + 1;
            }
        }
    }
    else if (txtClaimType.value == "Shortage") {
        for (var n = 1; n < TargetBaseControl.rows.length; ++n) {
            MRNApprovedQty = dGetValue(TargetBaseControl.rows[n].cells[11].childNodes[0].value);
            if (MRNApprovedQty > 0) {
                iROW = iROW + 1;
            }
        }
    }
    else {
        for (var n = 1; n < TargetBaseControl.rows.length; ++n) {
            MRNApprovedQty = dGetValue(TargetBaseControl.rows[n].cells[12].childNodes[0].value);
            if (MRNApprovedQty > 0) {
                iROW = iROW + 1;
            }
        }
    }
    if (sStatus == "Approved" && iROW == 0) {
        alert('Please Approve Atleast one Part...')
        return false;
    }
    if (sStatus == "Reject" && iROW != 0) {
        alert('Please Reject All Part Details...')
        return false;
    }

    if (txtClaimType.value == "Shortage") {
        for (var n = 1; n < TargetBaseControl.rows.length; ++n) {

            var TxtCredit = TargetBaseControl.rows[n].cells[15].childNodes[0];
            if (TxtCredit.value == "0") {
                alert('Please Enter The Credit Type =' + n)
                return false;
            }
            
            var TxtDebit = TargetBaseControl.rows[n].cells[16].childNodes[0];
            if (TxtDebit.value == "0") {
                alert('Please Enter The Debit Type =' + n)
                return false;
            }
        }
    }    
    

    var txtCSMRemark = document.getElementById("txtCSMRemark");
    var txtASMRemark = document.getElementById("txtASMRemark");
    //    var txtRSMRemark = document.getElementById("txtRSMRemark");
    //    var txtHeadRemark = document.getElementById("txtHeadRemark");
    var hdnUserRoleId = document.getElementById("hdnUserRoleId");
    if ((txtCSMRemark.value == '' && hdnUserRoleId.value == '11') || (txtASMRemark.value == '' && hdnUserRoleId.value == '12')) {
        alert("Please Enter Remark !");
        return false;
    }
    if (sStatus == 'Approved') {
        if (confirm("Are you sure, you want to Approve Part Claim?") == true) {
            //Sujata 27012011
            //alert("Succeefully Approved Part Claim ");
            //Sujata 27012011
            return true
        }
        else {
            return false
        }
    }
    else if (sStatus == 'Reject') {
        //Sujata 27012011
        //        var txtRejectionReason=document.getElementById("txtRejectionReason");
        //        if(txtRejectionReason.value == '')
        //          {
        //             alert("Please Enter Rejection Reason !");
        //             txtRejectionReason.style.display = '';
        //             txtRejectionReason.style.enabled = true;
        //             txtRejectionReason.focus();
        //             return false
        //          }
        //          else if (confirm("Are you sure, you want to Reject Part Claim?") == true)
        if (confirm("Are you sure, you want to Reject Part Claim?") == true)
        //Sujata 27012011
        {
            //alert("Reject Part Claim ");
            return true
        }
        else {
            return false;
        }
    }
}   
   
   
   function MRNValidation()
   {
   var drpMRNStatus=document.getElementById("ContentPlaceHolder1_drpMRNStatus");
    if(drpMRNStatus.value == "0")
    {
     alert("Please Select MRN Status !");
     drpMRNStatus.focus();
     return false;
    }  
    return true;
   
   }
   
     function MRNBack()
         {
         var ReturnID=0;
         ReturnID = document.getElementById("txtPreviousDocId").value;
        window.returnValue = ReturnID;
       window.close(); 
            }
        
        
        
        function PartClaimSelVal()
   {
   var drpClaimType=document.getElementById("ContentPlaceHolder1_drpClaimType");
    if(drpClaimType.value == "0")
    {
     alert("Please Select Part Claim  Type !");
     drpClaimType.focus();
     return false;
    }  
    
     var drpPartClaimStatus=document.getElementById("ContentPlaceHolder1_drpPartClaimStatus");
    if(drpPartClaimStatus.value == "0")
    {
     alert("Please Select Part Claim  Status !");
     drpPartClaimStatus.focus();
     return false;
    }  
    
    return true;
   
   }
   
   
  function CheckMRN()
  {
  var TargetBaseControl;
   TargetBaseControl =document.getElementById("GridMRNProcess");
  var Inputs;
  
     for(var n = 1; n <TargetBaseControl.rows.length; ++n)
    {
          Inputs=TargetBaseControl.rows[n].cells[CheckBoxCelIndex].childNodes[0];
          Inputs.all[0].checked =bchk;
          TargetBaseControl.rows[n].cells[10].childNodes[0].style.display='none'; 
     }
                   
   }
   
    function CheckMRNRemark()
  {
  var TargetBaseControl;
   TargetBaseControl =document.getElementById("GridMRNProcess");
  var Inputs;
  var TxtRemark;
  var TxtApprove;
  
     for(var n = 1; n <TargetBaseControl.rows.length-1; ++n)
    {
        TxtApprove = TargetBaseControl.rows[n].cells[7].childNodes[0];
        Inputs = TargetBaseControl.rows[n].cells[9].childNodes[0];
        if (TxtApprove.value == '') {
            alert('Please Enter Approve Quantity= ' + n)
            TxtApprove.focus();
              return false;
          }
          
          if (!Inputs.childNodes[0].checked)
          {
            TxtRemark=TargetBaseControl.rows[n].cells[10].childNodes[0];
            if (TxtRemark.value == '' && TxtRemark.disabled == false)
              {
              alert('Please Enter Rejection Resaon= '+n)
              return false;
              }
          }
     }
     return true;

 }

 function ShowInformationMessage(message) {
     alert(message)
 }


 //Sujata 29012011
 function MRNProcessing(sStatus) {
     var bReturn;
     var TargetBaseControl;
     TargetBaseControl = document.getElementById("GridMRNProcess");
     var iROW;
    
     iROW = 0;
     for (var n = 1; n < TargetBaseControl.rows.length; ++n) {                                 
         MRNApprovedQty = dGetValue(TargetBaseControl.rows[n].cells[7].childNodes[0].value);
         if (MRNApprovedQty > 0) {
             iROW = iROW + 1;         
         }
     }
     if (sStatus == "Approved" && iROW == 0) {
         alert('Please Approve At lease one Part...')
         return false;
     }
     if (sStatus == "Reject" && iROW != 0) {
         alert('Please Reject All Part Details...')
         return false;
     }
     
     for (var n = 1; n < TargetBaseControl.rows.length; ++n) {
         MRNQty = dGetValue(TargetBaseControl.rows[n].cells[6].childNodes[0].innerHTML);
         MRNApprovedQty = dGetValue(TargetBaseControl.rows[n].cells[7].childNodes[0].value);
         var txtReason = TargetBaseControl.rows[n].cells[10].childNodes[0];
         if ((MRNApprovedQty < MRNQty) && (txtReason.value == "0") && txtReason.disabled == false) {
             alert('Please Enter The Rejection Reason =' + n)
             return false;
         }
     }

     var txtCSMRemark = document.getElementById("txtCSMRemark");
     var txtASMRemark = document.getElementById("txtASMRemark");
//     var txtRSMRemark = document.getElementById("txtRSMRemark");
     //     var txtHeadRemark = document.getElementById("txtHeadRemark");
     var hdnUserRoleId = document.getElementById("hdnUserRoleId");
     if ((txtCSMRemark.value == '' && hdnUserRoleId.value=='11') ||(txtASMRemark.value == '' && hdnUserRoleId.value=='12')) {
         alert("Please Enter Remark !");
         return false;
     }
     if (sStatus == 'Approved') {
         if (confirm("Are you sure, you want to Approve MRN Processing?") == true) {             
             return true
         }
         else {
             return false
         }
     }
     else if (sStatus == 'Reject') {         
         if (confirm("Are you sure, you want to Reject MRN Processing?") == true)         
         {             
             return true
         }
         else {
             return false;
         }
     }
 }
 //Sujata 29012011