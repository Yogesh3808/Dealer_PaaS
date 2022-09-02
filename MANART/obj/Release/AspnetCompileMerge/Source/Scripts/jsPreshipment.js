// when user select accept check box
function DeSelectCheckbox(ObjChkAccept,drpReasonID)
{
   var ParentCtrlID;
   var ObjReason;   
   ParentCtrlID=ObjChkAccept.id.substring(0, ObjChkAccept.id.lastIndexOf("_"));    
   ObjReason= document.getElementById(ParentCtrlID+"_"+ drpReasonID);
   if (ObjReason == null) return ;        
   if(ObjChkAccept.checked)       
   {        
       ObjReason.style.display='none' ;
   }
   else
   {
        ObjReason.style.display='' ;
        ObjReason.focus();                    
   }
   return true;
}
// when user select the Reason
function ChekcReasonSelectedInPreshipment(objReason)
{    
   var sSelecedValue= objReason.options[objReason.selectedIndex].text;   
   if ( objReason.selectedIndex==0)
   {
        alert('Please select the reason for deselect the record');    
        return true ;    
   }

}

function ChekQtySelectedInPreshipment(ObjControl) {    
 // var ObjCheckForSelect = ObjControl.parentNode.parentNode.parentNode.childNodes[16].childNodes[0].childNodes[0];    
    var ObjtxtDeSelectQty = ObjControl;
    var ObjQty = ObjControl.parentNode.parentNode.childNodes[3].childNodes[0];
    
    var txtTotal = window.document.getElementById("ContentPlaceHolder1_txtTotalAmt");
    var txtIncoAmount = window.document.getElementById("ContentPlaceHolder1_txtIncoAmount");
    var txtGTotal = window.document.getElementById("ContentPlaceHolder1_txtGrandAmt");

    if (dGetValue(ObjtxtDeSelectQty.value) == 0) {
        ObjtxtDeSelectQty.value = ObjQty.innerText;
        alert('Please Select checkbox for no rejection');
    }
    else if (dGetValue(ObjtxtDeSelectQty.value) > dGetValue(ObjQty.innerText)) {
        ObjtxtDeSelectQty.value = ObjQty.innerText;
        alert('Value should not less than qty');
    }
    
    var txtGrdTotal = 0;
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var CountRow = objGrid.rows.length;
    for (var i = 1; i < CountRow; i++) {
        var ObjGrdtxtDeSelectQty = objGrid.rows[i].childNodes[6].childNodes[2];
        var ObjGrdQty = objGrid.rows[i].childNodes[3].childNodes[0];
        var ObjGrdRate = objGrid.rows[i].childNodes[4].childNodes[0];
        txtGrdTotal = txtGrdTotal + (dGetValue(ObjGrdQty.innerText) - dGetValue(ObjGrdtxtDeSelectQty.value)) * dGetValue(ObjGrdRate.innerText)
    }
    txtTotal.value = parseFloat(txtGrdTotal).toFixed(2);
    txtGTotal.value = parseFloat(dGetValue(txtTotal.value) + dGetValue(txtIncoAmount.value)).toFixed(2);    
}

function ChangeIncoTermPreshipment(ObjControl) {
    // var ObjCheckForSelect = ObjControl.parentNode.parentNode.parentNode.childNodes[16].childNodes[0].childNodes[0];
    var txtIncoAmount = ObjControl;    
    var txtTotal = window.document.getElementById("ContentPlaceHolder1_txtTotalAmt");    
    var txtGTotal = window.document.getElementById("ContentPlaceHolder1_txtGrandAmt");
    txtGTotal.value = parseFloat(dGetValue(txtTotal.value) + dGetValue(txtIncoAmount.value)).toFixed(2);
    
}

//To Show Part Master
function GetPackingBoxDetails(objNewPartLabel) 
{
    var iBoxCnt = 0;
    var iPacking_List_ID = 0;
    var iPacking_List_No = "123";
    var iPreshipment_Hdr_ID = 0;
    var iIndent_No = "123";
    var iMenuID = 0;
    var sConfirm = "N";
    var sPreInvInv = "123";
    var sDealerCode = "123";
    var ObjControl = window.document.getElementById("ContentPlaceHolder1_txtNoOfBoxes");
    //Get Box Count
    if (ObjControl != null) 
    {
        iBoxCnt = dGetValue(ObjControl.value);
    }
    if (iBoxCnt == 0) 
    { 
        alert ("Please Enter The No. Of Boxes !");
        return false;
    }
    //Get Packing List ID
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtID");
    if (ObjControl != null) {
        iPacking_List_ID = dGetValue(ObjControl.value);
    }
    //Get Packing List No
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDocNo");
    if (ObjControl != null) {
        iPacking_List_No= (ObjControl.value);
    }
    //Get Preshipment Hdr ID    
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtPreshipmentID");
    if (ObjControl != null) {
        iPreshipment_Hdr_ID = (ObjControl.value);
    }
    //Get Indent No
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtIndentNo");
    if (ObjControl != null) {
        iIndent_No = (ObjControl.value);
    }
    //Get Menuid
    ObjControl = window.document.getElementById("ContentPlaceHolder1_hdnMenuID");
    if (ObjControl != null) {
        iMenuID = (ObjControl.value);
    }
    //Get Confirm
    ObjControl = window.document.getElementById("ContentPlaceHolder1_hdnDocConfirm");
    if (ObjControl != null) {
        sConfirm = (ObjControl.value);
    }
    //Get Preshipment Invoice No
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtPreviousDocNo");
    if (ObjControl != null) {
        sPreInvInv = (ObjControl.value);
    }
    //Get dealer Spares Code
    ObjControl = window.document.getElementById("ContentPlaceHolder1_hdnDealerCode");
    if (ObjControl != null) {
        sDealerCode = (ObjControl.value);
    }
    //var Parameters = "BoxCnt=" + iBoxCnt + "&ID=" + iPacking_List_ID + "&No=" + iPacking_List_No;
    var Parameters = "BoxCnt=" + iBoxCnt + "&ID=" + iPacking_List_ID + "&No=" + iPacking_List_No + "&PreshipmentHDRID=" + iPreshipment_Hdr_ID + "&IndentNo=" + iIndent_No + "&MenuID=" + iMenuID + "&DocConfirm=" + sConfirm + "&PreInvInv=" + sPreInvInv + "&DealerCode=" + sDealerCode;
    var feature = "dialogWidth:1000px;dialogHeight:900px;status:no;help:no;scrollbars:no;resizable:no;";
    var ArrOfWeight;
    ArrOfWeight = window.showModalDialog("/AUTODMS/Forms/Common/frmEnterBoxDetails.aspx?" + Parameters, null, feature);

    if (ArrOfWeight == null) return;
    //Sujata 31082013_Begin
    //if (ArrOfWeight.length < 2) return;
    if (ArrOfWeight.length < 3) return;
    //Sujata 31082013_End
    //Set Net Weight
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtNetWeight");
    if (ObjControl != null) {
        ObjControl.value =RoundupValue( ArrOfWeight[0]);
    }
    //Set Gross Weight
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtGrossWeight");
    if (ObjControl != null) {
        ObjControl.value = RoundupValue(ArrOfWeight[1]);
    }
    //Sujata 31082013_Begin
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtNoOfBoxes");
    if (ObjControl != null) {
        ObjControl.value = parseInt(ArrOfWeight[2]);
    }    
    //Sujata 31082013_End
}

//when User Deselect the Record then Display Reason Combo and reduce total amount.
function HideUnhideResaon(ObjControl) {    
    var ObjCheckForSelect = ObjControl.parentNode.parentNode.parentNode.childNodes[17].childNodes[0].childNodes[0];
    if (ObjCheckForSelect != null) 
    {
        var ObjdrpReason = ObjControl.parentNode.parentNode.parentNode.childNodes[17].childNodes[2];
        var txtTotal = window.document.getElementById("ContentPlaceHolder1_txtTotalAmt");
        var txtGTotal = window.document.getElementById("ContentPlaceHolder1_txtGrandAmt");

        var Qty = ObjControl.parentNode.parentNode.parentNode.childNodes[4].childNodes[0];
        var Rate = ObjControl.parentNode.parentNode.parentNode.childNodes[5].childNodes[0];
        
        if (ObjdrpReason != null) {
            if (ObjCheckForSelect.checked == false) 
            {
                ObjdrpReason.style.display = '';
                txtTotal.value = dGetValue(txtTotal.value) - (dGetValue(Qty.innerHTML) * dGetValue(Rate.innerHTML));
                txtGTotal.value = txtTotal.value;
            }
            else {
           
                txtTotal.value = dGetValue(txtTotal.value) + (dGetValue(Qty.innerHTML) * dGetValue(Rate.innerHTML));
                txtGTotal.value = txtTotal.value;
                ObjdrpReason.selectedIndex = 0;
                ObjdrpReason.style.display = 'none';
            }
        }
        
        
    }
}

//when User Deselect the Record then Display Reason Combo and reduce total amount.
function HideUnhideSpPreshipResaon(ObjControl) {    
    var ObjCheckForSelect = ObjControl.parentNode.parentNode.parentNode.childNodes[6].childNodes[0].childNodes[0];    
    
    if (ObjCheckForSelect != null) {
        var ObjdrpReason = ObjControl.parentNode.parentNode.parentNode.childNodes[6].childNodes[4];
        var ObjtxtDeSelectQty = ObjControl.parentNode.parentNode.parentNode.childNodes[6].childNodes[2];       
        var txtTotal = window.document.getElementById("ContentPlaceHolder1_txtTotalAmt");
        var txtIncoAmount = window.document.getElementById("ContentPlaceHolder1_txtIncoAmount");
        var txtGTotal = window.document.getElementById("ContentPlaceHolder1_txtGrandAmt");

        var Qty = ObjControl.parentNode.parentNode.parentNode.childNodes[3].childNodes[0];
       // ObjtxtDeSelectQty.value = dGetValue(Qty.innerHTML);        
        var Rate = ObjControl.parentNode.parentNode.parentNode.childNodes[4].childNodes[0];
        if (ObjdrpReason != null) {
            if (ObjCheckForSelect.checked == false) {
                ObjdrpReason.style.display = '';
                ObjtxtDeSelectQty.style.display = '';
                ObjtxtDeSelectQty.value = dGetValue(Qty.innerHTML);
            }
            else {
                ObjtxtDeSelectQty.value = dGetValue(0);
                ObjtxtDeSelectQty.style.display = 'none';
                ObjdrpReason.selectedIndex = 0;
                ObjdrpReason.style.display = 'none';
            }
        }
    }
    var txtGrdTotal = 0;
    var objGrid = document.getElementById("ContentPlaceHolder1_PartGrid");
    var CountRow = objGrid.rows.length;      
    for (var i = 1; i < CountRow; i++)    
    {        
        var ObjGrdtxtDeSelectQty = objGrid.rows[i].childNodes[6].childNodes[2];
        var ObjGrdQty = objGrid.rows[i].childNodes[3].childNodes[0];
        var ObjGrdRate = objGrid.rows[i].childNodes[4].childNodes[0];
        txtGrdTotal = txtGrdTotal + (dGetValue(ObjGrdQty.innerHTML) - dGetValue(ObjGrdtxtDeSelectQty.value)) * dGetValue(ObjGrdRate.innerHTML)
    }
    txtTotal.value = parseFloat(txtGrdTotal).toFixed(2);
    txtGTotal.value = parseFloat(dGetValue(txtTotal.value) + dGetValue(txtIncoAmount.value)).toFixed(2);    
}

function ChekShipmentDateValidation(ObjShipmentDate)
 {
    var sPreshipmentValue = "";
    // Get Preshipment Date
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
    if (ObjControl != null) {
        sPreshipmentValue = ObjControl.value;
    }
    var ObjGrid;
    var sShipmentDate = ObjShipmentDate.value;
    if (sShipmentDate == "") return;

    //Check With Preshipment Date
    if (sPreshipmentValue == sShipmentDate) {

    }
    else if (bCheckFirstDateIsGreaterThanSecondDate(sShipmentDate, sPreshipmentValue) == false) {
        alert('Shipment Date should not be less than Preshipment Date !');
        ObjRepairOrderDate.value = '';
        ObjRepairOrderDate.focus();
        return;
    }
}
// To Check Validation before sending The mail
function CheckVehiclePreshipmentMailValidation() {
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    if (ObjGrid == null) return;
    var bCheckValidation = true;
    var ObjCheckForSelect;
    var ObjdrpReason;
    var sMessage = "";
    for (i = 1; i < ObjGrid.rows.length; i++) 
    {
        if (ObjGrid.rows[i].cells[15].children[0].selectedIndex == 0) 
        {
            alert("Please Select The PDI Done (Y/N) At Line" + i);
            return false;  
        }
        ObjCheckForSelect = ObjGrid.rows[i].cells[16].children[0].childNodes[0];
        if (ObjCheckForSelect.checked == false) {
            ObjdrpReason = ObjGrid.rows[i].cells[16].children[1];
            if (ObjdrpReason != null) {
                if (ObjdrpReason.selectedIndex == 0) {
                    sMessage = sMessage + "\n" + " Chassis No. '" + ObjGrid.rows[i].cells[12].innerText + "' At Line " + i;
                }
            }
        }

    }
    if (sMessage != "") {
        sMessage = "Please Select The Reason For The Following Chassis :-" + sMessage;
        alert(sMessage);
        return false;
    }
    //System will Send a Mail to SAP, To Get Commercial Invoice For the Selected Chassis !.\n
    var sMsg = "Are you sure, you want to continue?";
    if (confirm(sMsg) == false) {
        return false;
    }
}
// To Check Sapre Preshipment Validation Before Mail Send
function CheckSparePreshipmentMailValidation() {
    var sMessage = "";
    var sShipmentDate = "";
    var sPreshipmentDate = "";
    var ObjControl;

    // Get Preshipment Date
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
    if (ObjControl != null) {
        sPreshipmentDate = ObjControl.value;
    }
    // Get Shipment Date 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtShipmentDate_txtDocDate");
    if (ObjControl != null) {
        sShipmentDate = ObjControl.value;
    }


    if (bCheckFirstDateIsGreaterThanSecondDate(sShipmentDate, sPreshipmentDate) == true) {
        alert("Please Select The Shipment Date Greater Than The Preshipment Invoice Date !.");
        return false;
    }

    var sMsg = "Are you sure, you want to continue?";
    if (confirm(sMsg) == false) {
        return false;
    }
}

//To Check Shipment Date Should be greater than Preshipment Invoice Date
function ChekSparesShipmentDateValidation(ObjShipmentDate) 
{
    var sShipmentDate = "";
    var sPreshipmentDate = "";
    var ObjControl;

    // Get Preshipment Date
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
    if (ObjControl != null) {
        sPreshipmentDate = ObjControl.value;
    }

    // Get Shipment Date 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtShipmentDate_txtDocDate");
    if (ObjControl != null) {
        sShipmentDate = ObjControl.value;
    }

    if (bCheckFirstDateIsGreaterThanSecondDate(sShipmentDate, sPreshipmentDate) == true) {
        alert("Please Select The Shipment Date Greater Than The Preshipment Invoice Date !.");
        return false;
    }
}

function ShowDocumentReport()
 {
    //window.showModalDialog("/AUTODMS/Forms/Common/frmSelectDocOnNew.aspx?" + Parameters, null, feature);
    var iDocId = 0;
    var ObjControl = document.getElementById('ContentPlaceHolder1_drpPreshipmentInv');
    if (ObjControl == null) return;
    
    if (ObjControl.selectedIndex == 0) 
    {    
        alert("Please Select The Record!");
        return false;
    }
    var Url = "/AUTODMS/Forms/Common/frmDocumentView.aspx?";
    var sReportName = "";
    //Megha 01062011 
    var sMenuId = document.getElementById('ContentPlaceHolder1_txtMenuId');
   
    iDocId = ObjControl.value;
   
    if (sMenuId.value == '375') {
        ObjControl = document.getElementById('ContentPlaceHolder1_OptReportType_0');
        if (ObjControl.checked == true) //Bill of Exchange
        {
            sReportName = "RptName=/DCSReports/rptBillOfExchange&";  //+ strReportName + "&";
        }
        else {
            ObjControl = document.getElementById('ContentPlaceHolder1_OptReportType_1');
            if (ObjControl.checked == true)//Certificate of Origin
            {
                sReportName = "RptName=/DCSReports/rptPreCertOrigin&";  //+ strReportName + "&";
            }
            else {
                ObjControl = document.getElementById('ContentPlaceHolder1_OptReportType_2');
                if (ObjControl.checked == true)//Shipment Advice
                {
                    sReportName = "RptName=/DCSReports/rptPreShipmentadvice&";  //+ strReportName + "&";
                }
            }
        }
    }
    else if (sMenuId.value == '380') {
        ObjControl = document.getElementById('ContentPlaceHolder1_OptReportType_0');
        if (ObjControl.checked == true) //Bill of Exchange
        {
            sReportName = "RptName=/DCSReports/rptBillOfExchange&";  //+ strReportName + "&";
        }
        else {
            ObjControl = document.getElementById('ContentPlaceHolder1_OptReportType_1');
            if (ObjControl.checked == true)//Certificate of Origin
            {
                sReportName = "RptName=/DCSReports/rptPreCertOriginSpr&";  //+ strReportName + "&";
            }
            else {
                ObjControl = document.getElementById('ContentPlaceHolder1_OptReportType_2');
                if (ObjControl.checked == true)//Shipment Advice
                {
                    sReportName = "RptName=/DCSReports/rptSpareShipmentadvice&";  //+ strReportName + "&";
                }
            }
        }
    }
    //Megha 01062011
    if (sReportName == "") 
    {
        return false;
        
    }
    //Sujata 24122010
    //Url = Url + sReportName + "ID=" + iDocId;
    Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=N"
    //Sujata 24122010
    
    //window.open(Url, "MyReport");

    var windowFeatures;
    window.opener = self;
    //window.close()  
    windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
    newWindow = window.open(Url, "", windowFeatures)
    window.moveTo(0, 0);
    window.resizeTo(screen.width, screen.height - 100);
    newWindow.focus();
     
    //window.showModalDialog(Url, "MyReport", null, null);

    return false;
}
//Sujata 16012011
// check Proforma date should be less than or equal to Preshipment date
function CheckPreshipmentDateWithProformaDate() {
    var ObjControl;
    var objINSDate;
    var sINSDate = '';
    var sReceiptDate = '';
    
   
    // Get Vehicle In Date
    ObjControl = document.getElementById("ContentPlaceHolder1_txtProformaDate");

    if (ObjControl != null) {
        sReceiptDate = ObjControl.value;
    }
    if (sReceiptDate == "") return;

    // Get ETB Invoice Date
    objINSDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");

    if (objINSDate != null) {
        sINSDate = objINSDate.value;
    }
    if (sINSDate == "") return;    

    if (bCheckFirstDateIsGreaterThanSecondDate(sReceiptDate, sINSDate) == true) {

        if (sReceiptDate == sINSDate) return;

        alert('Preshipment Date Should be greater than or Equal to Proforma Date !');
        objINSDate.value = '';
        objINSDate.focus();
        return false;
    }
    //Sujata 17012011
    //Code For Current Date
    
    var x = new Date();
    var y = x.getYear();
    var m = x.getMonth() + 1;
    var d = x.getDate();
    var dtCur = d + '/' + m + '/' + y;
    var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)
    
    
    var sTmpValue = sINSDate;
    var day = dGetValue(sTmpValue.split("/")[0]);
    var month = dGetValue(sTmpValue.split("/")[1]) - 1;
    var year = dGetValue(sTmpValue.split("/")[2]);
    var sTmpDate = new Date(year, month, day);
    var TmpDay = 0;

    if (dtCurDate < sTmpDate) {
        alert('Preshipment Date Should not be greater than Current Date !')
        objINSDate.value = '';
        objINSDate.focus();
        return false;
    }
    //Sujata 17012011
}

// check Preshipment date should be less than or equal to Postshipment date
function CheckPostshipmentDateWithPreshipmentDate() {
    var ObjControl;
    var objINSDate;
    var sINSDate = '';
    var sReceiptDate = '';

    // Get Vehicle In Date
    ObjControl = document.getElementById("ContentPlaceHolder1_txtProformaDate");

    if (ObjControl != null) {
        sReceiptDate = ObjControl.value;
    }
    if (sReceiptDate == "") return;

    // Get ETB Invoice Date
    objINSDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");

    if (objINSDate != null) {
        sINSDate = objINSDate.value;
    }
    if (sINSDate == "") return;



    if (bCheckFirstDateIsGreaterThanSecondDate(sReceiptDate, sINSDate) == true) {

        if (sReceiptDate == sINSDate) return;

        alert('Postshipment Date Should be greater than or Equal to Preshipment Date !');
        objINSDate.value = '';
        objINSDate.focus();
        return false;
    }
    //sujata 17012011
    // Code For Current date  
    var x = new Date();
    var y = x.getYear();
    var m = x.getMonth() + 1;
    var d = x.getDate();
    var dtCur = d + '/' + m + '/' + y;
    var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)
    
    var sTmpValue = sINSDate;
    var day = dGetValue(sTmpValue.split("/")[0]);
    var month = dGetValue(sTmpValue.split("/")[1]) - 1;
    var year = dGetValue(sTmpValue.split("/")[2]);
    var sTmpDate = new Date(year, month, day);
    var TmpDay = 0;

    if (dtCurDate < sTmpDate) {
        alert('Postshipment Date Should not be greater than Current Date !')
        objINSDate.value = '';
        objINSDate.focus();
        return false;
    }
    //Sujata 17012011
}
//Sujata 16012011

//sujata 25122010
function ShowVehPreshipment(iDocId, Url) {

    var sReportName = "";
    sReportName = "/rptPreshipmentInvoiceNew&";
    Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=Y";
    //window.showModalDialog(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
    var windowFeatures;
    window.opener = self;
    //window.close()  
    windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
    newWindow = window.open(Url, "", windowFeatures)
    window.moveTo(0, 0);
    window.resizeTo(screen.width, screen.height - 100);
    newWindow.focus();

    return false;
}
//sujata 25122010