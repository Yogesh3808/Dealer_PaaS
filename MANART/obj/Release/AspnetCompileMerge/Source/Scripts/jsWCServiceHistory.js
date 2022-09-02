
function ChkWCServiceHistory()
{
var txtServiceNo=document.getElementById("txtServiceNo");
 if(txtServiceNo.value == '')
    {
     alert("Please Enter Service No!");
     txtServiceNo.focus();
        return false;
    }
    
    var drpServiceType=document.getElementById("drpServiceType");
if(drpServiceType.value == "0")
    {
     alert("Please Select Service Type !");
     drpServiceType.focus();
     return false;
    }   
    
    var txtRepairOrderNO=document.getElementById("txtRepairOrderNO");
 if(txtRepairOrderNO.value == '')
    {
     alert("Please Enter Repair Order No!");
     txtRepairOrderNO.focus();
        return false;
    }

    var txtRepairOrderDate = document.getElementById("txtRepairOrderDate_txtDocDate");
 if(txtRepairOrderDate.value == '')
    {
     alert("Please Enter Repair Order Date!");
     txtRepairOrderDate.focus();
        return false;
    }
    
         var txtKMS=document.getElementById("txtKMS");
 if(txtKMS.value == '')
    {
     alert("Please Enter KMS!");
     txtKMS.focus();
        return false;
    }

    var txtFailuareDate = document.getElementById("txtFailuareDate_txtDocDate");
 if(txtFailuareDate.value == '')
    {
     alert("Please Enter Failuare Date!");
     txtFailuareDate.focus();
        return false;
    }
    
    
         var txtRepairDetails=document.getElementById("txtRepairDetails");
 if(txtRepairDetails.value == '')
    {
     alert("Please Enter Repair Details!");
     txtRepairDetails.focus();
        return false;
    }
    
    
    return true;
}


function ShowWServiceHistory(HdrID,sFor,ClaimOrRequest)
{

    window.showModalDialog("/AUTODMS/Forms/Warranty/frmServiceHistory.aspx?HdrID=" + HdrID + "&sFor=" + sFor + "&ClaimOrRequest=" + ClaimOrRequest, "List", "dialogHeight: 500px; dialogWidth: 1170px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");                
    //window.location.reload();
    return false;
     
}       

function CheckServiceNo(ServiceNo)
{
var sServiceNo=ServiceNo.value;
var gServiceNo;

var gridObj = document.getElementById("GridServiceDtls");

if (gridObj == null) {
    return true;
}
for(var n = 1; n <gridObj.rows.length; ++n)
    {
       gServiceNo=gridObj.rows[n].cells[1].innerText;
       if(removeSpaces(sServiceNo)==removeSpaces(gServiceNo))
       {
       alert('Please Enter Another Service No! Its Already Exists!!')
       ServiceNo.value="";
       ServiceNo.focus();
       return false
       }
  }
  return true;

}


function CheckRepairNo(RepairNo) {
    var sRepairNo = RepairNo.value;
    var gServiceNo;

    var gridObj = document.getElementById("GridServiceDtls");

    if (gridObj == null) {
        return true;
    }
    for (var n = 1; n < gridObj.rows.length; ++n) {
        gRepairNo = gridObj.rows[n].cells[3].innerText;
        if (removeSpaces(sRepairNo) == removeSpaces(gRepairNo)) {
            alert('Please Enter Another Repair No!! Its Already Exists!!')
            RepairNo.value = "";
            RepairNo.focus();
            return false
        }
    }
    return true;

}

function removeSpaces(string) {
 return string.split(' ').join('');
}

function ShowChassisWDtls(iDocId,Url)
{
    //debugger;
    //var iDocId = document.getElementById("txtchassisID").value;
    //var iDocId = document.getElementById("ContentPlaceHolder1_txtchassisID").value;
    var sReportName ="";
    sReportName ="/RptWarrantyClaimHistory&";   
    var Url = "";
    var sExportYesNo = "Y";
    Url = "../Common/frmDocumentView.aspx?RptName=/MANARTREPORT";
    Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=" + sExportYesNo;

    //Vrushali12012011_Begin
    //Changes done to show Claim History report in full screen  
    //window.showModalDialog(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    var windowFeatures;
    window.opener = self;
    //window.close()  
    windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
    newWindow = window.open(Url, "", windowFeatures)
    window.moveTo(0, 0);
    window.resizeTo(screen.width, screen.height - 100);
    newWindow.focus();
    //Sujata 13012011
    //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    //Sujata 13012011
    //Vrushali12012011_End
    return false;



}

function ShowClaimHistoryDtls(iDocId, Url) {
    //var iDocId = document.getElementById("txtchassisID").value;   
    //debugger;
    var sReportName = "";
    sReportName = "/rptServiceHistory&";
    //var Url ="";
    //Url ="/AUTODMS/Forms/Common/frmDocumentView.aspx?";
    Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=Y";

    //Vrushali12012011_Begin
    //Changes done to show Claim History report in full screen  
    //window.showModalDialog(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    var windowFeatures;
    window.opener = self;
    //window.close()  
    windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
    newWindow = window.open(Url, "", windowFeatures)
    window.moveTo(0, 0);
    window.resizeTo(screen.width, screen.height - 100);
    newWindow.focus();
    //Sujata 13012011
    //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    //Sujata 13012011
    //Vrushali12012011_End
    return false;



}

function ShowRequisitionDtls(iDocId, Url) {
    //debugger;
    var DrpRequisitionLst = window.document.getElementById("ContentPlaceHolder1_DrpRequisitionLst");
    var sReqNo = "";
    if (DrpRequisitionLst != null) {
        if (DrpRequisitionLst.selectedIndex == 0) {            
            alert('Please Select Requisition No For Print!')
            return false;
        }
    sReqNo = DrpRequisitionLst.options[DrpRequisitionLst.selectedIndex].innerText;
    }
    
    var sReportName = "";
    sReportName = "/RptServiceRequisition&";
    
    Url = Url + sReportName + "ID=" + iDocId + "&ReqNo=" + sReqNo + "&ExportYesNo=J";
   
    var windowFeatures;

    window.opener = self;
   
    windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
    newWindow = window.open(Url, "", windowFeatures)
    window.moveTo(0, 0);
    window.resizeTo(screen.width, screen.height - 100);
    newWindow.focus();
   
    return false;



}
//function ShowClaimHistoryDtls(iDocId, Url) {
//    //var iDocId = document.getElementById("txtchassisID").value;    
//    var sReportName = "";
//    sReportName = "/rptServiceHistory&";
//    //var Url ="";
//    //Url ="/AUTODMS/Forms/Common/frmDocumentView.aspx?";             
//    Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=N";
//    //window.showModalDialog(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
//    window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");                    
//    return false;

//}      

// To Close Warranty Prossesing Form
function CloseWarrantyClaimProsseingWindow() {
    window.close();
}