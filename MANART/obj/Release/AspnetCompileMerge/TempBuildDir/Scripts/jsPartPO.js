function POValidation()
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
   // To Show Indent
function ShowPO(ObjImg) 
{    
    var iPOID = 0;    
    var objRow = ObjImg.parentNode.parentNode.childNodes;
    
    // Get climaid
    iPOID = dGetValue(objRow[2].children[0].innerText);   
    var sUrl = "/AUTODMS/Forms/PO/frmPOProcessing.aspx?POID=" + iPOID;
    window.showModalDialog(sUrl, "List", "dialogWidth:1100px;dialogHeight:800px;status:yes;help:no;");
    CollectGarbage();    
    //window.location.reload();
    return true;
}

// To Exist
function ClosePOForm() {
    window.returnvalue = 1;
    window.close();    
    return true;
}