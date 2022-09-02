var ArrAllJobes;


// function is used Clear Controls Of the Form
//when User Click On New button
function ClearFormControlsValue() {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";

    var i
    var ObjControl;
    var ObjControlonlyDate;
    var controlid;
    var ControlContainer = document.getElementById(PcontainerName + 'ControlContainer');
    var colcount = 0;
    //var colcount=ControlContainer.children[0].children.length;
    ObjControl = document.getElementById(PcontainerName + 'txtControlCount');
    if (ObjControl != null) {
        colcount = ObjControl.value;
    }
    document.getElementById(PcontainerName + 'txtID').value = '';


    for (i = 1; i <= colcount; i++) {
        controlid = PcontainerName + i;
        ObjControl = document.getElementById(controlid)
        //Megha20042011
        ObjControlonlyDate = document.getElementById(controlid + '_txtDocDate')
        //Megha20042011
        if (ObjControl != null) {
            if (ObjControl.type == "text") {
                ObjControl.value = '';
                //ObjControl.readOnly=false;
                ObjControl.disabled = false;
            }
            else if (ObjControl.type == "select-one") {
                ObjControl.value = '';
                ObjControl.disabled = false;
                ObjControl.children[0].innerText = '--Select--';
            }
        }
            //Megha20042011
        else if (ObjControlonlyDate != null) {
            ObjControl = document.getElementById(controlid + '_txtDocDate')
            if (ObjControl.value == "01/08/2009" || ObjControl.value == "01/04/2005") {

                var currdate = new Date();
                currdate = currdate.format("dd/MM/yyyy")
                ObjControl.value = currdate;
                ObjControl.disabled = false;

            }
        }
            //Megha20042011 
        else {
            controlid = PcontainerName + i + '_ctl01_ChkAll';
            ObjControl = document.getElementById(controlid)
            if (ObjControl != null) {
                ObjControl.checked = false;
                SelectAllCheckboxes(ObjControl, 'N');
            }

        }
    }
}

// function is used to Check basic Validation before save the record.
function CheckValidDataBeforeSave(ImgId) {
    var PcontainerName = '';
    var ObjControl
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";;
    var i;
    ObjControl = document.getElementById(PcontainerName + 'txtID');
    if (ObjControl != null) {
        if (ObjControl.value != "") {
            ShowToolBarMessage(ImgId)
            return false;
        }
    }
    else {
        return;
    }
    var ControlContainer = document.getElementById(PcontainerName + 'ControlContainer');
    //var colcount=ControlContainer.children[0].children.length;      
    ObjControl = document.getElementById(PcontainerName + 'txtControlCount');
    if (ObjControl == null) return;
    var colcount = ObjControl.value;
    for (i = 1; i <= colcount; i++) {
        var controlid = PcontainerName + i;
        ObjControl = document.getElementById(controlid)
        if (ObjControl != null) {
            if (ObjControl.type == "text") {
                if (ObjControl.value == "") {
                    alert("Please Enter record");
                    ObjControl.focus();
                    return false;
                }
            }
            else if (ObjControl.type == "select-one") {
                if (ObjControl.value == "0") {
                    alert("Please Select record");
                    ObjControl.focus();
                    return false;
                }
            }
        }
    }
    return true;
}
// function used to show Toolbar button action message from textbox
function ShowToolBarMessage(ObjImg) {
    var Control = document.getElementById("ContentPlaceHolder1_Toolbar_txtShowToolBarName");
    if (Control != null) {
        Control.value = ObjImg.title;
    }
    if (ObjImg.title == "New") {
        SetCancelStyleonMouseOver(ObjImg);
    }
}
// function used to remove Toolbar button action message from textbox
function RemoveToolBarName(ObjImg) {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    var Control = document.getElementById(PcontainerName + 'Toolbar_txtShowToolBarName');
    if (Control != null) {
        Control.value = '';
    }
    if (ObjImg.title == "New") {
        SetCancelStyleOnMouseOut(ObjImg);
    }
}
//check before Save
function CheckForSave(ID) {
    if (ID == 0) {
        return true;
    }
    else if (ID == 1)//CheckFor Vehicle RFP
    {
        if (CheckVRFPValidation() == false) {
            return false;
        }
    }
    else if (ID == 2)//CheckFor Parts PO
    {//CheckSRFPValidation
        if (CheckPOSaveValidation() == false) {
            return false;
        }
    }
        //Megha 23092011
    else if (ID == 9999)// CheckFor Dynamic Master
    {
        if (CheckforSaveDynamicFormControls() == false) {
            return false;
        }
    }
        //Megha 23092011
    else if (ID == 3)//CheckFor Vehicle Proforma 
    {
        if (CheckVehicleProformaValidation() == false) {
            return false;
        }
    }
    else if (ID == 4)//CheckFor Spaes Proforma 
    {
        if (CheckSparesProformaValidation() == false) {
            return false;
        }
    }
    else if (ID == 5)//CheckFor Vehicle ORF
    {
        if (CheckVehicleORFValidation() == false) {
            return false;
        }
    }
    else if (ID == 6)//CheckFor Spaes ORF 
    {
        if (CheckVehicleORFValidation() == false) {
            return false;
        }
    }
    else if (ID == 7)// For Stock Transfer Receipt
    {
        if (CheckStkTrnsReceiptSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 8)//CheckFor Packing Slip
    {
        if (CheckSparesPackingListValidation() == false)
            return false;
    }
    else if (ID == 9)//CheckFor Parts RFP
    {
        if (CheckPartsRFPSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 10)//CheckFor Parts Proforma
    {
        if (CheckPartsProformaSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 21) //Check LC function
    {
        if (CheckLCValidation() == false) {
            return false;
        }
    }
    else if (ID == 22) //Check Advance Payment function
    {
        if (CheckAdvPaymentValidation() == false) {
            return false;
        }
    }
    else if (ID == 23) //Check Acivity Master function
    {
        if (CheckActivityMasterValidation() == false) {
            return false;
        }
    }
    else if (ID == 24) //Check Acivity function
    {
        if (CheckActivityValidation() == false) {
            return false;
        }
    }
    else if (ID == 240) //Check Acivity Claim function
    {
        if (CheckActivityClaimValidation() == false) {
            return false;
        }
    }
    else if (ID == 25) //Check RFQ Function
    {
        if (CheckRFQValidation() == false) {
            return false;
        }
    }
        //else if (ID == 26) //Check Installation Function
        //{
        //    if (ValidateInstallationCer() == false) {
        //        return false;
        //    }
        //}
    else if (ID == 27) //Check VehicleIN Function
    {
        if (ValidateVehicleIN() == false) {
            return false;
        }
    }
    else if (ID == 28) //Check MRN
    {
        if (CheckMRN() == false) {
            return false;
        }
    }
    else if (ID == 29) //Check Dummy LC Function
    {
        if (CheckDummyLCValidation() == false) {
            return false;
        }
    }

    else if (ID == 30) //Check Warranty Service History
    {
        if (ChkWCServiceHistory() == false) {
            return false;
        }
    }

    else if (ID == 31) //Check Warranty Service History
    {
        if (ValidateINSProcess() == false) {
            return false;
        }
    }

    else if (ID == 32) {
        if (CheckMRNRemark() == false) {
            return false;
        }
    }

    else if (ID == 40) //Check Wrranty function
    {
        if (CheckWarrantyValidation() == false) {
            return false;
        }
    }

    else if (ID == 60) //Check Wrranty function
    {
        if (CheckExportWarrantyPolicyValidation() == false) {
            return false;
        }
    }

    else if (ID == 50) {
        var Control = document.getElementById(PcontainerName + 'txtID');
        if (Control == null) return;
        if (Control.value == "") {
            var sMsg = "System will create the Indent from curent status of the Inquiry.\n Are you sure, you want to continue?";
            if (confirm(sMsg) == false) {

                return false
            }
        }
    }
    else if (ID == 51) //Check FPDA Function
    {
        //debugger;
        if (ValidateFPDA() == false) {
            return false;
        }
    }
    else if (ID == 53) //Check Part Scheme Value Function
    {
        if (ValidatePartSchemeValue() == false) {
            return false;
        }
    }
        //Sujata 12122012
    else if (ID == 41) //Check Request Saving
    {
        //debugger;
        var ObjControl = document.getElementById("txtID");

        var iRecordCnt = 0;
        var ObjComplaintCount = document.getElementById("lblComplaintsRecCnt");
        if (ObjComplaintCount != null) {
            iRecordCnt = dGetValue(ObjComplaintCount.innerText);
        }
        if (iRecordCnt == 0) {
            alert("Please save Complaints details on Jobcard before creating Request....!");
            return false;
        }
        iRecordCnt = 0;
        var ObjInvestigationCount = document.getElementById("lblInvestigationsRecCnt");
        if (ObjInvestigationCount != null) {
            iRecordCnt = dGetValue(ObjInvestigationCount.innerText);
        }
        if (iRecordCnt == 0) {
            alert("Please save investigation details on Jobcard before creating Request....!");
            return false;
        }

        var ObjhdnJobCode = document.getElementById("hdnJobCode");
        if (ObjhdnJobCode != null) {
            if (ObjhdnJobCode.value == "N" && ObjControl.value == "0") {
                alert("Please save jobcode details on Jobcard before creating Request....!");
                return false;
            }
        }

        if (CheckWarrantyValidation() == false) {
            return false;
        }
    }
    else if (ID == 42) //Check Spares Plant Details Function
    {
        if (ValidateSPLNT("Y") == false) {
            return false;
        }
    }
    else if (ID == 43) //Check Spares Plant Details Function
    {
        if (CheckExportRequestValidation("N") == false) {
            return false;
        }
    }
    else if (ID == 44) //Export Warranty Details Function
    {
        if (CheckExportWarrantyValidation("N") == false) {
            return false;
        }
    }
        //    //Sujata 12122012
        //else if (ID == 61) //EGP Dealer Creation
        //{
        //    if (CheckEGPDealerCreationValidation() == false) {
        //        return false;
        //    }
        //}
    else if (ID == 61) {
        if (CheckSRNValidation() == false) {
            return false;
        }
    }

    else if (ID == 62) //Customer Creation
    {
        if (CheckCustomerCreationValidation() == false) {
            return false;
        }
    }
    else if (ID == 63) //Parts Sales OA
    {
        if (CheckOAValidation() == false) {
            return false;
        }
    }
    else if (ID == 64) //Parts Sales Invoice
    {
        if (CheckOAInvValidation() == false) {
            return false;
        }
    }


    else if (ID == 65) //Lead Transaction
    {
        if (CheckLeadTransaction() == false) {
            return false;
        }
    }

    else if (ID == 66) //Material Receipt Transaction
    {
        //if (CheckLeadMaster() == false) {
        //    return false;
        //}
    }
    else if (ID == 67) // Jobcard Transaction
    {
        ////debugger;
        if (CheckJobcard("N") == false) {
            return false;
        }
    }
    else if (ID == 68) // Jobcard Transaction
    {
        ////debugger;
        if (CheckEstimate("N") == false) {
            return false;
        }
    }
    else if (ID == 69) // Jobcard Transaction
    {
        ////debugger;
        if (CheckDiscountApp() == false) {
            return false;
        }
    }
    else if (ID == 70) // Jobcard Transaction
    {
        ////debugger;
        if (CheckLossApp() == false) {
            return false;
        }
    }
    else if (ID == 71) // Jobcard Transaction
    {
        ////debugger;
        if (CheckDCSPOApp() == false) {
            return false;
        }
    }
    else if (ID == 72)// Material Receipt
    {
        if (CheckMaterialReceiptSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 73)// Stocking Norms Saving
    {
        //debugger;
        var numericExpression = /^[0-9]+$/;
        var objControl = document.getElementsByClassName('TextBoxForString');
        for (var i = 1; i <= objControl.length; i++) {
            if (objControl[i].value == "") {
                alert("Please fill All Category");
                return false;
            }
            //else if (objControl[i].value != "" && !(numericExpression.test(objControl[i].value)))
            //{
            //    txt += "Please make sure your file is in pdf or zip format and less than 3 MB.\n\n";
            //    alert("Please Enter only Numeric");
            //    return false;
            //}
        }
    }
        //else if (ID == 74)// For CRM Service
        //{
        //    debugger;
        //    var objMenuid;
        //    var ObjControl;
        //    var ObjUserID;
        //    objMenuid = window.document.getElementById("ContentPlaceHolder1_txtMenuid");
        //    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpDealerName");
        //    ObjUserID = window.document.getElementById("ContentPlaceHolder1_txtUserid");
        //    //if (objMenuid.value == "670" && ObjUserID.value == "24") //Amrit id
        //    //{
        //    //    if (ObjControl != null) {
        //    //        if (ObjControl.selectedIndex == 0) {
        //    //            alert("Please Select The Dealer Name!.");
        //    //            return false;
        //    //        }
        //    //    }
        //    //}
        //}
    else if (ID == 75)// PRN 
    {
        if (CheckPRNSaveValidation() == false) {
            return false;
        }
    }
}

// Check Validation On Save PRN
function CheckPRNSaveValidation() {
    debugger;
    var ObjControl;
    ObjControl = window.document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName");
    var ObjType = window.document.getElementById("ContentPlaceHolder1_hdnIsAutoPRN");

    var ObjDrpInvice = window.document.getElementById("ContentPlaceHolder1_ddlInvoice");
    var objtxtInvoiceNo = window.document.getElementById("ContentPlaceHolder1_txtInvNo");
    //var ObjDelNo = window.document.getElementById("ContentPlaceHolder1_txtDeliveryNo");
    var objInvoiceDate = window.document.getElementById("ContentPlaceHolder1_txtInvDate_txtDocDate");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select The Supplier Name!.");
            return false;
        }
    }
    if (ObjType != null) {
        if (ObjType.value == "Y") {
            //if ((ObjDrpInvice.selectedIndex == 0 ) ) {
            //    alert("Please Select The Invoice No!.");
            //    return false;
            //}
        }
        else if (ObjType.value == "N") {
            if (objtxtInvoiceNo != null) {
                if (objtxtInvoiceNo.value == "") {
                    alert("Please Enter Invoice No!.");
                    return false;
                }
            }
            if (objInvoiceDate != null) {
                if (objInvoiceDate.value == "") {
                    alert("Please Select Invoice Date!.");
                    return false;
                }
            }
        }
    }
    debugger;
    // Part Grid
    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iBillQty = 0;
    var iRecvQty = 0;
    var iDamgeQty = 0;
    var iManfQty = 0;
    var iWrgQty = 0;
    var iTotal = 0;
    var iWrgPartId = 0;
    var iShortageQty = 0;
    var iSrNo = 0;
    var iPartAdd = 0;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[17].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            iPartAdd = iPartAdd + 1;
        }
    }

    if (ObjGrid.rows.length == 2 || iPartAdd == 0) {
        alert("Please Select The Part!.");
        return false;
    }

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check Part ID
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        if (iPartID != 0) {
            // Check Quantity is enter
            if (ObjType != null) {
                if (ObjType.value == "Y" || ObjType.value == "N") {
                    iSrNo = ObjGrid.rows[i].cells[0].innerText;
                    objtxtControl = ObjGrid.rows[i].cells[6].children[0];
                }
            }
            iQty = dGetValue(objtxtControl.value);
            if (iQty == 0) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + iSrNo;
            }
            //if (ObjType.value == "N") {
            objtxtControl = ObjGrid.rows[i].cells[11].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed at line " + iSrNo;
            }
            //sGridPartTax = ObjGrid.rows[i].cells[16].children[0].selectedIndex;//ObjGrid.rows[i].childNodes[12].children[0].selectedIndex;
            var sGridPartTax = ObjGrid.rows[i].cells[16].children[0].value.trim();
            //alert(sGridPartTax);
            if (isNaN(sGridPartTax) == true) sGridPartTax = 0;
            if (sGridPartTax == 0) {
                sMessage = sMessage + " \n Please Enter the Part Tax at line " + iSrNo;
            }
            //}
        }

    }
    //New Part Stock Quantity Validation Dated 06042018

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        iSrNo = ObjGrid.rows[i].cells[0].innerText;
        //sBFRGST = ObjGrid.rows[i].cells[27].children[0];// BFRGST Rate Flag
        ObjControl = ObjGrid.rows[i].cells[1].children[0]; // PartID
        var bPartSel = ObjGrid.rows[i].cells[17].children[0].children[0].checked;
        dCurrSTK = dGetValue(ObjGrid.rows[i].cells[15].children[0].value);
        //dBFRGSTSTK = dGetValue(ObjGrid.rows[i].cells[28].children[0].value);
        iPartID = dGetValue(ObjControl.value);
        var iCount = 0;

        if (iPartID != 0 && bPartSel == false) {
            var totPrevQty = 0;
            var totInvQty = 0;
            for (var j = 1; j < ObjGrid.rows.length; j++) {
                var ObjControl1 = ObjGrid.rows[j].cells[1].children[0];
                var bPartSel1 = ObjGrid.rows[j].cells[17].children[0].children[0].checked;
                jPartID = dGetValue(ObjControl1.value);
                if (jPartID != 0 && jPartID == iPartID && bPartSel1 == false) {
                    var jInvQty = dGetValue(ObjGrid.rows[j].cells[6].children[0].value);
                    var jPrevQty = dGetValue(ObjGrid.rows[j].cells[6].children[1].value);
                    totPrevQty = totPrevQty + jPrevQty;
                    totInvQty = totInvQty + jInvQty;
                    iCount = iCount + 1;
                }  //END IF
            } // END For
        } // END IF

        if (iPartID != 0 && totInvQty > (dCurrSTK + totPrevQty) && bPartSel == false) {
            sMessage = sMessage + " \n Please Enter less  return Quantity from Part Stock at Row No " + iSrNo;
        }

    } // END For

    //Vikram 26102016 Begin Discount not allowed more than equal to 100    
    ObjGrid = null;
    var GrdPartGroupID = $("#ContentPlaceHolder1_GrdPartGroup");
    ObjGrid = GrdPartGroupID[0];
    if (ObjGrid == null) return;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        var dAmt = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
        sGroupCode = ObjControl.value;
        if (sGroupCode.trim() != "" && dAmt > 0) {
            // Check Discount is enter
            objtxtControl = ObjGrid.rows[i].cells[4].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed in group tax details at line " + i;
            }
            // Check Discount Amount is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= dAmt) {
                sMessage = sMessage + " \n Enter Discount Amount Less than Taxable Inv Amt at Line " + i;
            }
        }
    }

    var Total = 0;
    var objGridDocTax = null;
    //debugger;
    var GrdDocTaxDetID = $("#ContentPlaceHolder1_GrdDocTaxDet");
    objGridDocTax = GrdDocTaxDetID[0];
    Total = dGetValue(objGridDocTax.rows[1].cells[13].children[0].value);
    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n PF Charges Percentage should be less than 100....!";
    }
    // Other chagres Per
    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n Other Charges Percentage should be less than 100....!";
    }

    //end vikram
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;
}


// Check Validation On Save Material Receipt
function CheckMaterialReceiptSaveValidation() {
    //debugger;
    var ObjControl;
    ObjControl = window.document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName");
    var ObjType = window.document.getElementById("ContentPlaceHolder1_hdnIsAutoReceipt");
    var hdnSupplierType = window.document.getElementById("ContentPlaceHolder1_hdnSupplierType");

    var ObjDrpInvice = window.document.getElementById("ContentPlaceHolder1_ddlInvoice");
    var objtxtInvoiceNo = window.document.getElementById("ContentPlaceHolder1_txtDMSInvNo");
    var ObjDelNo = window.document.getElementById("ContentPlaceHolder1_txtDeliveryNo");
    var objInvoiceDate = window.document.getElementById("ContentPlaceHolder1_txtDMSInvDate_txtDocDate");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select The Supplier Name!.");
            return false;
        }
    }
    if (ObjType != null) {
        if (ObjType.value == "Y") {
            if (ObjDrpInvice.selectedIndex == 0 && objtxtInvoiceNo.value == "") {
                alert("Please Select The Invoice No!.");
                return false;
            }
            if (ObjDelNo != null) {
                if (ObjDelNo.value == "" && hdnSupplierType.value == "M") {
                    alert("Please Enter Delivery No!.");
                    return false;
                }
            }
        }
        else if (ObjType.value == "N") {
            if (objtxtInvoiceNo != null) {
                if (objtxtInvoiceNo.value == "") {
                    alert("Please Enter Invoice No!.");
                    return false;
                }
            }
            if (objInvoiceDate != null) {
                if (objInvoiceDate.value == "") {
                    alert("Please Select Invoice Date!.");
                    return false;
                }
            }
        }
    }
    //debugger;
    // Part Grid
    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iBillQty = 0;
    var iRecvQty = 0;
    var iDamgeQty = 0;
    var iManfQty = 0;
    var iWrgQty = 0;
    var iTotal = 0;
    var iWrgPartId = 0;
    var iShortageQty = 0;
    var iSrNo = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check Part ID
        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            iPartID = dGetValue(ObjControl.value);
            if (iPartID != 0) {
                // Check Quantity is enter
                if (ObjType != null) {
                    if (ObjType.value == "Y") {
                        iSrNo = ObjGrid.rows[i].cells[0].innerText;
                        objtxtControl = ObjGrid.rows[i].cells[6].children[0];
                        iBillQty = ObjGrid.rows[i].cells[5].children[0];
                        iDamgeQty = ObjGrid.rows[i].cells[22].children[0];
                        iManfQty = ObjGrid.rows[i].cells[23].children[0];
                        iWrgQty = ObjGrid.rows[i].cells[24].children[0];
                        iTotal = dGetValue(iDamgeQty.value) + dGetValue(iManfQty.value) + dGetValue(iWrgQty.value);
                        iWrgPartId = ObjGrid.rows[i].cells[26].children[0];
                        iShortageQty = ObjGrid.rows[i].cells[20].children[0];
                    }
                    else {
                        iSrNo = ObjGrid.rows[i].cells[0].innerText;
                        objtxtControl = ObjGrid.rows[i].cells[5].children[0];
                    }
                }
                iQty = dGetValue(objtxtControl.value);
                //if (iQty == 0) {
                //    sMessage = sMessage + " \n Please Enter the Quantity at line " + iSrNo;
                //}

                if (iQty < iTotal && ObjType.value == "Y" && dGetValue(iShortageQty) != dGetValue(iBillQty)) { // add last condition and comment 0 condition
                    sMessage = sMessage + " \n Descripancy Quantity cannot greater than Receive Quantity at line " + iSrNo;
                }
                if (iWrgQty > 0 && iWrgPartId == 0 && ObjType.value == "Y") {
                    sMessage = sMessage + " \n Please Select Wrong Part at line " + iSrNo;
                }
                if (ObjType.value == "N") {
                    objtxtControl = ObjGrid.rows[i].cells[10].children[0];
                    iQty = dGetValue(objtxtControl.value);
                    if (iQty >= 100) {
                        sMessage = sMessage + " \n 100% and above discount not allowed at line " + iSrNo;
                    }
                    sGridPartTax = ObjGrid.rows[i].cells[14].children[0].selectedIndex;//ObjGrid.rows[i].childNodes[12].children[0].selectedIndex;
                    if (sGridPartTax == 0) {
                        sMessage = sMessage + " \n Please Enter the Part Tax at line " + iSrNo;
                    }
                }
            }
        }
    }
    //Vikram 26102016 Begin Discount not allowed more than equal to 100    
    ObjGrid = null;
    var GrdPartGroupID = $("#ContentPlaceHolder1_GrdPartGroup");
    ObjGrid = GrdPartGroupID[0];
    if (ObjGrid == null) return;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        var dAmt = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
        sGroupCode = ObjControl.value;
        if (sGroupCode.trim() != "" && dAmt > 0) {
            // Check Discount is enter
            objtxtControl = ObjGrid.rows[i].cells[4].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed in group tax details at line " + i;
            }
            // Check Discount Amount is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= dAmt) {
                sMessage = sMessage + " \n Enter Discount Amount Less than Taxable Inv Amt at Line " + i;
            }
        }
    }

    var Total = 0;
    var objGridDocTax = null;
    //debugger;
    var GrdDocTaxDetID = $("#ContentPlaceHolder1_GrdDocTaxDet");
    objGridDocTax = GrdDocTaxDetID[0];
    Total = dGetValue(objGridDocTax.rows[1].cells[13].children[0].value);
    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n PF Charges Percentage should be less than 100....!";
    }
    // Other chagres Per
    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n Other Charges Percentage should be less than 100....!";
    }

    //end vikram
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;
}

// TO Check Stock Transfer Receipt Validation
function CheckStkTrnsReceiptSaveValidation() {
    //debugger;
    var ObjControl;
    ObjControl = window.document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName");
    var ObjCntrl;
    ObjCntrl = window.document.getElementById("ContentPlaceHolder1_drpChallanNo");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select The Supplier Name!.");
            return false;
        }
    }
    if (ObjCntrl != null) {
        if (ObjCntrl.selectedIndex == 0) {
            alert("Please Select The Chanlla No!.");
            return false;
        }
    }

    // Part Details
    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check Part ID
        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            iPartID = dGetValue(ObjControl.value);
            if (iPartID != 0) {
                // Check Quantity is enter
                objtxtControl = ObjGrid.rows[i].cells[5].children[0];
                iQty = dGetValue(objtxtControl.value);
                if (iQty == 0) {
                    sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
                }
            }
        }
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;
}

// To Check Parts Proforma Validation Dated 16042018
function CheckPartsProformaSaveValidation() {
    //debugger;
    var ObjControl;
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpSupplier");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Supplier!.");
            return false;
        }
    }
    var skillsSelect = document.getElementById("ContentPlaceHolder1_DrpProfomaInvoice");
    var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;

    if (selectedText == "--Select--") {
        alert("Please Select Proforma Invoice No!.");
        return false;
    }

    //ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpProfomaInvoice");

    //if (ObjControl != null) {
    //    if (ObjControl.selectedIndex == 0) {
    //        alert("Please Select Proforma Invoice No!.");
    //        return false;
    //    }
    //}

    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iPartAdd = 0;
    var bPartSel = false;
    var iSrNo = 0;

    for (var i = 0; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[9].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            iPartAdd = iPartAdd + 1;
        }
    }

    if (ObjGrid.rows.length == 0 || iPartAdd == 0) {
        alert("Please Select The Part!.");
        return false;
    }

    for (var i = 0; i < ObjGrid.rows.length; i++) {
        iSrNo = ObjGrid.rows[i].cells[0].innerText;
        ObjControl = ObjGrid.rows[i].cells[1].children[0];// Check Part ID
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[9].children[0].children[0].checked;
        if (iPartID != 0) {
            // Check Quantity is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty == 0 && bPartSel == false) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + iSrNo;
            }
        }
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;
}

// To Check Parts RFP Validation Dated 16042018
function CheckPartsRFPSaveValidation() {
    //debugger;
    var ObjControl;
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpSupplier");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Supplier!.");
            return false;
        }
    }

    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iPartAdd = 0;
    var bPartSel = false;
    var iSrNo = 0;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[8].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            iPartAdd = iPartAdd + 1;
        }
    }

    if (ObjGrid.rows.length == 1 || iPartAdd == 0) {
        alert("Please Select The Part!.");
        return false;
    }

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        iSrNo = ObjGrid.rows[i].cells[0].innerText;
        ObjControl = ObjGrid.rows[i].cells[1].children[0];// Check Part ID
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[8].children[0].children[0].checked;
        if (iPartID != 0) {
            // Check Quantity is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty == 0 && bPartSel == false) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + iSrNo;
            }
        }
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;
}
// To Check Parts PO Validation
function CheckPOSaveValidation() {
    debugger;
    var ObjSupplier = window.document.getElementById("ContentPlaceHolder1_ExportLocation_drpDealerName");
    if (ObjSupplier != null) {
        if (ObjSupplier.selectedIndex == 0 && ObjSupplier.value == "0") {
            alert("Please Select Supplier!.");
            return false;
        }
    }
    var ObjControl;
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPoType");
    var ObjPoType = window.document.getElementById("ContentPlaceHolder1_hdnPoTypeID");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0 && ObjPoType.value == "") {
            alert("Please Select The PO Type!.");
            return false;
        }
    }

    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iPartAdd = 0;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[8].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            iPartAdd = iPartAdd + 1;
        }
    }

    //if (ObjGrid.rows.length == 1 || iPartAdd == 0) {
    //    alert("Please Select The Part!.");
    //    return false;
    //}

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check Part ID
        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            iPartID = dGetValue(ObjControl.value);
            if (iPartID != 0) {
                // Check Quantity is enter
                objtxtControl = ObjGrid.rows[i].cells[5].children[0];
                iQty = dGetValue(objtxtControl.value);
                if (iQty == 0) {
                    sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
                }
                // Commented by Shyamal on 02062012,Already cover in code behind
                //            if (ObjGrid.rows[i].cells[8].children[0].children[0].checked == true) {
                //                iCountDel = iCountDel + 1;
                //            }            
            }
        }
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    // Commented by Shyamal on 02062012,Already cover in code behind
    //    iCountDel = iCountDel + 1;
    //    if (iCountDel == ObjGrid.rows.length) {
    //        alert("Please keep at least one record at details!");
    //        return false;
    //    }
    return true;
}

// To  Check vehicle Vehicle ORF Validation
function CheckJobcard(Confirm) {

    var sMessage = "";
    var ObjControl;
    var sJobtype = "";
    debugger;
    //Job Type
    drpJobType = window.document.getElementById("ContentPlaceHolder1_drpJobType");
    if (drpJobType != null) {
        if (drpJobType.selectedIndex == 0) sMessage = sMessage + "\n Please Select Job Type.";
        sJobtype = drpJobType.options[drpJobType.selectedIndex].value;
    }
    //chassis 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtChassisID");
    if (ObjControl != null) {
        if (sJobtype != "18" && (ObjControl.value == "" || dGetValue(ObjControl.value) == 0)) sMessage = sMessage + "\n Please Select Chassis.";
    }
    //Aggregate 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtAggreagateNo");
    if (ObjControl != null) {
        if (sJobtype == "18" && (ObjControl.value == "" || dGetValue(ObjControl.value) == 0)) sMessage = sMessage + "\n Please Enter Aggregate No.";
    }
    //Customer for Aggregate 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpCustomer");
    if (ObjControl != null) {
        if (sJobtype == "18" && ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select Customer.";
    }
    //Model Code for Aggregate 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpModelCode");
    if (ObjControl != null) {
        if (sJobtype == "18" && ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select Model.";
    }
    //Kms 
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtKms");
    if (ObjControl != null) {
        if (ObjControl.value == "" || dGetValue(ObjControl.value) == 0) sMessage = sMessage + "\n Please Enter Kms.";
    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_hdnWarrChkType");
    if (ObjControl != null) {
        if (ObjControl.value.trim() == "T") {
            ObjControl = window.document.getElementById("ContentPlaceHolder1_txtHrs");
            if (ObjControl != null) {
                if (ObjControl.value == "" || dGetValue(ObjControl.value) == 0) sMessage = sMessage + "\n Please Enter Hrs.";
            }
        }
    }
    //DrpBay
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpBay");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0 && (sJobtype != "10" && sJobtype != "15" && sJobtype != "13" && sJobtype != "18")) sMessage = sMessage + "\n Please Select Bay.";
    }
    //txtApPartAmt
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtApPartAmt");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter Approximate Part Amount.";
    }
    //txtApLabAmt
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtApLabAmt");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter Approximate Labor Amount.";
    }
    //txtApLubAmt
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtApLubAmt");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter Approximate Lubricant Amount.";
    }
    //txtApMiscAmt
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtApMiscAmt");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter Approximate Misc. Amount.";
    }
    //DrpBay
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpInsurnceComp");
    if (ObjControl != null) {
        if (Confirm == "Y" && ObjControl.selectedIndex == 0 && (sJobtype == "2")) sMessage = sMessage + "\n Please Select Insurance Company Customer.";
    }
    //DrpSupervisorName
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpSupervisorName");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select Supervisor Name.";
    }

    ObjControl = document.getElementById("ContentPlaceHolder1_dtpAllocateTime_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Select Bay Allocation Time.";
    }

    //ObjControl = document.getElementById("ContentPlaceHolder1_dtpVehSaleDt_txtDocDate");
    //if (ObjControl != null) {
    //    if (ObjControl.value == "") sMessage = sMessage + "\n Please Select Vehicle Sales Date.";
    //}

    ObjControl = document.getElementById("ContentPlaceHolder1_dtpJobCommited_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Select Commited Date.";
    }

    ObjControl = document.getElementById("ContentPlaceHolder1_txtFailureDt_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Select Complaint Date.";
    }

    ObjControl = document.getElementById("ContentPlaceHolder1_DtpVehicleOut_txtDocDate");
    if (Confirm == "Y" && ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Select Vehicle Out Date.";
    }
    ObjControl = document.getElementById("ContentPlaceHolder1_hdnPendingVORPORec");
    if (Confirm == "Y" && ObjControl != null) {
        if (ObjControl.value == "Y") sMessage = sMessage + "\n VOR PO receipt Not Done.";
    }

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    //debugger;
    //var ObjControl = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
    //if (sJobtype != "7" && Confirm == "Y" && ObjControl != null) {
    //    if (ObjControl.rows.length == 2) {
    //        alert("At least one labour should be present in Jobcard.");
    //        return false;
    //    }
    //}

    //Part Validations START
    //debugger;
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    var ArrAllJobs = new Array();
    var ArrTransitJobs = new Array();
    var ArrEnrouteTechJobs = new Array();
    var ArrEnrouteNonTechJobs = new Array();
    var ArrGoodwillJobs = new Array();

    var ArrWarrJobs = new Array();
    var ArrSprWarrJobs = new Array();
    var ArrPDIJobs = new Array();
    var ArrPrePDIJobs = new Array();
    var ArrCampaignJobs = new Array();
    var ArrAMCJobs = new Array();
    var ArrFOCJobs = new Array();
    var ArrFSCJobs = new Array();

    //debugger;
    for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
        var objPartChk = ObjGrid.rows[i].cells[39].childNodes[1].childNodes[0].checked;
        //var objLubLoc = ObjGrid.rows[i].cells[31].childNodes[1];
        if (objPartChk == false) {
            //var objtxtReqControl = ObjGrid.rows[i].cells[6].childNodes[0];
            //var objtxtIssueControl = ObjGrid.rows[i].cells[7].childNodes[0];
            //var objtxtReturnControl = ObjGrid.rows[i].cells[8].childNodes[0];
            //var objtxtPaidQtyControl = ObjGrid.rows[i].cells[9].childNodes[0];

            var objtxtReqControl = ObjGrid.rows[i].cells[6].childNodes[1];
            var objtxtIssueControl = ObjGrid.rows[i].cells[7].childNodes[1];
            var objtxtReturnControl = ObjGrid.rows[i].cells[8].childNodes[1];
            var objtxtPaidQtyControl = ObjGrid.rows[i].cells[10].childNodes[1];
            var objMechName = ObjGrid.rows[i].cells[5].childNodes[1];
            var objPartType = ObjGrid.rows[i].cells[36].childNodes[1];
            var objLubLoc = ObjGrid.rows[i].cells[32].childNodes[1];
            var objLubCap = ObjGrid.rows[i].cells[33].childNodes[1];

            var objtxtFOCQty = ObjGrid.rows[i].cells[13].childNodes[1];
            var objdrpReasonID = ObjGrid.rows[i].cells[14].childNodes[1];
            ////debugger;
            var ReqQty = dGetValue(objtxtReqControl.value);
            var IssueQty = dGetValue(objtxtIssueControl.value);
            var ReturnQty = dGetValue(objtxtReturnControl.value);
            var BillQty = dGetValue(objtxtPaidQtyControl.value);
            var FOCQty = dGetValue(objtxtFOCQty.value);
            var FOCReasonId = objdrpReasonID.value;
            var sPartType = objPartType.value.trim();
            var sLubLocID = objLubLoc.value;
            //var sLubCapacity = dGetValue(objLubCap[objLubCap.selectedIndex].text.trim());

            var PDIQty = dGetValue(ObjGrid.rows[i].cells[16].childNodes[1].value);
            var AMCQty = dGetValue(ObjGrid.rows[i].cells[17].childNodes[1].value);
            var CampaignQty = dGetValue(ObjGrid.rows[i].cells[18].childNodes[1].value);
            var TransitQty = dGetValue(ObjGrid.rows[i].cells[19].childNodes[1].value);
            var EnrouteTQty = dGetValue(ObjGrid.rows[i].cells[20].childNodes[1].value);
            var EnrouteNTQty = dGetValue(ObjGrid.rows[i].cells[21].childNodes[1].value);
            var SpWarrQty = dGetValue(ObjGrid.rows[i].cells[22].childNodes[1].value);
            var GoodWillQty = dGetValue(ObjGrid.rows[i].cells[23].childNodes[1].value);
            var WarrQty = dGetValue(ObjGrid.rows[i].cells[24].childNodes[1].value);
            var PrePDIQty = dGetValue(ObjGrid.rows[i].cells[25].childNodes[1].value);
            var aggtQty = dGetValue(ObjGrid.rows[i].cells[26].childNodes[1].value);
            var objMake = ObjGrid.rows[i].cells[31].childNodes[1];
            var objJobcode = ObjGrid.rows[i].cells[34].childNodes[1];

            if (ReqQty == 0 || ReqQty == "") {
                alert("Required Qty should not be blank For Part/Lub." + dGetValue(i - 1));
                return false;
            }
            if (ReqQty < IssueQty) {
                alert("Issue Qty is greater than Required Qty for Part line " + dGetValue(i - 1));
                return false;
            }
            if (IssueQty < ReturnQty) {
                alert("Return Qty is greater than Issue Qty for Part line " + dGetValue(i - 1));
                return false;
            }
            if (IssueQty < BillQty) {
                alert("Bill Qty is greater than Issue Qty for Part line " + dGetValue(i - 1));
                return false;
            }
            if (BillQty < 0) {
                alert("Enter Correct Part Quantity for Part line  " + dGetValue(i - 1));
                return false;
            }
            if (Confirm == "Y" && objMechName.value == "0" && IssueQty > 0) {
                alert("Fill Mechanic Details for Part line " + dGetValue(i - 1));
                objMechName.focus();
                return false;
            }
            if (FOCQty > 0 && FOCReasonId == 0) {
                alert("Select FOC Reason for Part line " + dGetValue(i - 1));
                return false;
            }
            if (sPartType == "O" && sLubLocID == "0") {
                alert("Select Lubricant Location for Part line " + dGetValue(i - 1));
                return false;
            }

            if ((sJobtype == 12 && (TransitQty > 0 && (EnrouteTQty > 0 || EnrouteNTQty > 0)) ||
                    (EnrouteTQty > 0 && (TransitQty > 0 || EnrouteNTQty > 0)) ||
                    (EnrouteNTQty > 0 && (EnrouteTQty > 0 || TransitQty > 0)))

                || ((sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
                    sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17) &&
                (WarrQty > 0 && (GoodWillQty > 0 || SpWarrQty > 0)) ||
                (GoodWillQty > 0 && (WarrQty > 0 || SpWarrQty > 0)) ||
                (SpWarrQty > 0 && (WarrQty > 0 || GoodWillQty > 0)))

                || (sJobtype == 5 && (WarrQty > 0 && (SpWarrQty > 0 || AMCQty > 0)) ||
                                      (SpWarrQty > 0 && (WarrQty > 0 || AMCQty > 0)) ||
                                      (AMCQty > 0 && (SpWarrQty > 0 || WarrQty > 0)))

                || (sJobtype == 13 && (CampaignQty > 0 && (WarrQty > 0 || SpWarrQty > 0)) ||
                (WarrQty > 0 && (CampaignQty > 0 || SpWarrQty > 0)) ||
                (SpWarrQty > 0 && (CampaignQty > 0 || WarrQty > 0)))
                ) {
                alert("Multiple Warranty Qty can not define under same requisition for Part line " + dGetValue(i - 1));
                return false;
            }

            if (PDIQty + AMCQty + CampaignQty + TransitQty + EnrouteTQty + EnrouteNTQty + SpWarrQty + GoodWillQty + WarrQty + PrePDIQty + aggtQty > 0 && objMake.value == "0") {
                alert("Select Make for Part line " + dGetValue(i - 1));
                return false;
            }
            if (Confirm == "Y" && objJobcode.value == "0" && IssueQty > 0) {
                alert("Select Jobcode for Part line " + dGetValue(i - 1));
                return false;
            }
            //Check Job Code Exist in Array of the Part Job
            if (objJobcode.value != "0") {//Confirm == "Y" &&            
                if (bCheclValueExistInArray(ArrAllJobs, objJobcode.value) == false) ArrAllJobs.push(objJobcode.value);

                if (sJobtype == 12 && TransitQty > 0 && bCheclValueExistInArray(ArrTransitJobs, objJobcode.value) == false) ArrTransitJobs.push(objJobcode.value);
                if (sJobtype == 12 && EnrouteTQty > 0 && bCheclValueExistInArray(ArrEnrouteTechJobs, objJobcode.value) == false) ArrEnrouteTechJobs.push(objJobcode.value);
                if (sJobtype == 12 && EnrouteNTQty > 0 && bCheclValueExistInArray(ArrEnrouteNonTechJobs, objJobcode.value) == false) ArrEnrouteNonTechJobs.push(objJobcode.value);

                if ((sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
                    sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17) && GoodWillQty > 0 && bCheclValueExistInArray(ArrGoodwillJobs, objJobcode.value) == false)
                    ArrGoodwillJobs.push(objJobcode.value);

                if ((sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
                    sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17 ||
                    sJobtype == 13 || sJobtype == 5) && WarrQty > 0 && bCheclValueExistInArray(ArrWarrJobs, objJobcode.value) == false)
                    ArrWarrJobs.push(objJobcode.value);

                if ((sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
                   sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17 ||
                    sJobtype == 5 || sJobtype == 13) && SpWarrQty > 0 && bCheclValueExistInArray(ArrSprWarrJobs, objJobcode.value) == false)
                    ArrSprWarrJobs.push(objJobcode.value);

                if (sJobtype == 5 && AMCQty > 0 && bCheclValueExistInArray(ArrAMCJobs, objJobcode.value) == false) ArrAMCJobs.push(objJobcode.value);
                if (sJobtype == 13 && CampaignQty > 0 && bCheclValueExistInArray(ArrCampaignJobs, objJobcode.value) == false) ArrCampaignJobs.push(objJobcode.value);
            }
        }
    }
    //Part Validations END

    //Labour Validation START
    var ObjLGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
    if (ObjLGrid == null) return;
    //var ObjLGrid = null;
    var cnt = 0;
    //if (ObjLGrid.rows.length == 2)
    //{
    //    alert("Please Select Labour.");
    //    return false;
    //}
    for (var i = 2; i <= ObjLGrid.rows.length - 1; i++) {
        debugger;
        var objLbrChk = ObjLGrid.rows[i].cells[21].childNodes[1].childNodes[0];
        var lbrchk = objLbrChk.checked;
        if (lbrchk == false) {
            var objLCode = ObjLGrid.rows[i].cells[2].childNodes[3];
            var objDrpLAddDesc = ObjLGrid.rows[i].cells[4].childNodes[1];
            var objLMechName = ObjLGrid.rows[i].cells[13].childNodes[1];

            var objdrpLFOC = ObjLGrid.rows[i].cells[10].childNodes[1];
            var objdrpLReasonID = ObjLGrid.rows[i].cells[11].childNodes[1];

            var LFOCTag = objdrpLFOC.value;
            var LFOCReasonId = objdrpLReasonID.value;

            var objdrpLWarrTag = ObjLGrid.rows[i].cells[9].childNodes[1];
            var objtxtLabTag = ObjLGrid.rows[i].cells[8].childNodes[1];


            var sLstTwoDigit = objLCode.value.toString().substr(objLCode.value.toString().length - 2, 2);
            var sFirstFiveDigit = objLCode.value.toString().substr(0, 5);



            //if (objDrpLAddDesc.value == "0" && (sLstTwoDigit == "99" || sFirstFiveDigit == "33333" || sFirstFiveDigit == "44444" || sFirstFiveDigit == "55555")) {
            if (objDrpLAddDesc.value == "0" && (sFirstFiveDigit == "MTIMI")) {
                alert("Fill Additional Labor for Labor line " + dGetValue(i - 1));
                return false;
            }

            if (Confirm == "Y" && objLMechName.value == "0") {
                alert("Fill Mechanic Details for Labor line " + dGetValue(i - 1));
                objLMechName.focus();
                return false;
            }
            if (LFOCTag == "Y" && LFOCReasonId == 0) {
                alert("Select FOC Reason for Labor line " + dGetValue(i - 1));
                return false;
            }
            if (LFOCTag == "Y" && objdrpLWarrTag.value == "G") {
                alert("Please select either FOC or Goodwill for Labor line " + dGetValue(i - 1));
                return false;
            }

            var objtxtLSubletAmt = ObjLGrid.rows[i].cells[14].childNodes[1];
            var objdrpLSubletSupplier = ObjLGrid.rows[i].cells[15].childNodes[1];
            var objdrpLOutMechName = ObjLGrid.rows[i].cells[16].childNodes[1];
            var objtxtSubletDesc = ObjLGrid.rows[i].cells[17].childNodes[1];
            var objtxtWONo = ObjLGrid.rows[i].cells[19].childNodes[1];
            var objLJobcode = ObjLGrid.rows[i].cells[20].childNodes[1];

            var SubletAmt = dGetValue(objtxtLSubletAmt.value);
            var LSubletSupp = objdrpLSubletSupplier.value;
            var LSubletOutMech = objdrpLOutMechName.value;
            var LSubletDesc = objtxtSubletDesc.value.trim();
            var sWORef = objtxtWONo.value.trim();
            debugger;

            if (SubletAmt == 0 && sFirstFiveDigit == "MTIOU") {
                alert("Fill Sublet Amount for Labor line " + dGetValue(i - 1));
                return false;
            }

            if (SubletAmt > 0 && LSubletSupp == 0) {
                alert("Select Sublet Supplier for line " + dGetValue(i - 1));
                return false;
            }

            //if (SubletAmt > 0 && LSubletOutMech == 0) {
            //    alert("Select Sublet Mechanic for line " + i);
            //    return false;
            //}
            if (SubletAmt > 0 && LSubletDesc == "") {
                alert("Select Sublet Description for line " + dGetValue(i - 1));
                return false;
            }

            if (Confirm == "Y" && sWORef == "" && sFirstFiveDigit == "MTIOU") {
                alert("Work Order Pendig for Sublet Labor line " + dGetValue(i - 1));
                return false;
            }
            //debugger;
            var objtxtMRecp = ObjLGrid.rows[i].cells[19].childNodes[3];
            var sMatRecp = objtxtMRecp.value.trim();
            if (Confirm == "Y" && sMatRecp == "" && sFirstFiveDigit == "MTIOU") {
                alert("Material Receipt Pendig for Sublet Labor line " + dGetValue(i - 1));
                return false;
            }

            if (Confirm == "Y" && objLJobcode.value == "0") {
                alert("Select Jobcode for Labor line " + dGetValue(i - 1));
                return false;
            }
            //Check Job Code Exist in Array of the Part Job
            if (objLJobcode.value != "0") {//Confirm == "Y" &&            
                if (bCheclValueExistInArray(ArrAllJobs, objLJobcode.value) == false) ArrAllJobs.push(objLJobcode.value);

                if (sJobtype == 12 && objdrpLWarrTag.value == "T" && bCheclValueExistInArray(ArrTransitJobs, objLJobcode.value) == false) ArrTransitJobs.push(objLJobcode.value);

                if (sJobtype == 12 && objdrpLWarrTag.value == "E" && bCheclValueExistInArray(ArrEnrouteTechJobs, objLJobcode.value) == false) ArrEnrouteTechJobs.push(objLJobcode.value);

                if (sJobtype == 12 && objdrpLWarrTag.value == "R" && bCheclValueExistInArray(ArrEnrouteNonTechJobs, objLJobcode.value) == false) ArrEnrouteNonTechJobs.push(objLJobcode.value);

                if ((sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
                    sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17) && objdrpLWarrTag.value == "G" && bCheclValueExistInArray(ArrGoodwillJobs, objLJobcode.value) == false) ArrGoodwillJobs.push(objLJobcode.value);

                if ((sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
                    sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17 ||
                    sJobtype == 13 || sJobtype == 5) && objdrpLWarrTag.value == "W" && bCheclValueExistInArray(ArrWarrJobs, objLJobcode.value) == false)
                    ArrWarrJobs.push(objLJobcode.value);

                if (sJobtype == 5 && objdrpLWarrTag.value == "A" && bCheclValueExistInArray(ArrAMCJobs, objLJobcode.value) == false) ArrAMCJobs.push(objLJobcode.value);
                if (sJobtype == 13 && objdrpLWarrTag.value == "C" && bCheclValueExistInArray(ArrCampaignJobs, objLJobcode.value) == false) ArrCampaignJobs.push(objLJobcode.value);

            }
        }
    }//Labour VAlidation END

    //Jobcode Validations START        
    var arrLen = 0;
    arrLen = ArrAllJobs.length;

    for (Arrj = 1; Arrj <= arrLen; Arrj++) {
        if (iGetIndexOfValueFromArrayJob(ArrAllJobs, (Arrj)) == false) {
            alert("Please select Jobcode " + ('J' + Arrj) + " before select next jobcode.");
            return false;
        }
    }
    var bTJob = false;
    var bETJob = false;
    var bENTJob = false;
    var bGdWllJob = false;
    var bWarrJob = false;
    var bAMCJob = false;
    var bCampaignJob = false;
    var bSprWarrJob = false;

    if (sJobtype == 12) {
        for (Arrj = 1; Arrj <= arrLen; Arrj++) {
            bTJob = iGetIndexOfValueFromArrayJob(ArrTransitJobs, (Arrj));
            bETJob = iGetIndexOfValueFromArrayJob(ArrEnrouteTechJobs, (Arrj));
            bENTJob = iGetIndexOfValueFromArrayJob(ArrEnrouteNonTechJobs, (Arrj));

            if ((bTJob == true && (bETJob == true || bENTJob == true)) ||
                (bETJob == true && (bTJob == true || bENTJob == true)) ||
                (bENTJob == true && (bETJob == true || bTJob == true))) {
                //debugger;
                alert("Please Jobcode " + ('J' + Arrj) + " should not present under multiple warranty.");
                return false;
            }
        }
    }
    else if (sJobtype == 1 || sJobtype == 2 || sJobtype == 3 || sJobtype == 10 ||
           sJobtype == 11 || sJobtype == 14 || sJobtype == 15 || sJobtype == 17) {

        for (Arrj = 1; Arrj <= arrLen; Arrj++) {
            bWarrJob = iGetIndexOfValueFromArrayJob(ArrWarrJobs, (Arrj));
            bGdWllJob = iGetIndexOfValueFromArrayJob(ArrGoodwillJobs, (Arrj));
            bSprWarrJob = iGetIndexOfValueFromArrayJob(ArrSprWarrJobs, (Arrj));

            if ((bWarrJob == true && (bGdWllJob == true || bSprWarrJob == true)) ||
                    (bGdWllJob == true && (bWarrJob == true || bSprWarrJob == true)) ||
                    (bSprWarrJob == true && (bGdWllJob == true || bWarrJob == true))) {
                //debugger;
                alert("Please Jobcode " + ('J' + Arrj) + " should not present under multiple warranty.");
                return false;
            }
        }
    }
    else if (sJobtype == 5) {
        for (Arrj = 1; Arrj <= arrLen; Arrj++) {
            bAMCJob = iGetIndexOfValueFromArrayJob(ArrAMCJobs, (Arrj));
            bSprWarrJob = iGetIndexOfValueFromArrayJob(ArrSprWarrJobs, (Arrj));

            if (bAMCJob == true && bSprWarrJob == true) {
                //debugger;
                alert("Please Jobcode " + ('J' + Arrj) + " should not present under multiple warranty.");
                return false;
            }
        }
    }
    else if (sJobtype == 13) {
        for (Arrj = 1; Arrj <= arrLen; Arrj++) {
            bCampaignJob = iGetIndexOfValueFromArrayJob(ArrCampaignJobs, (Arrj));
            bSprWarrJob = iGetIndexOfValueFromArrayJob(ArrSprWarrJobs, (Arrj));
            bWarrJob = iGetIndexOfValueFromArrayJob(ArrWarrJobs, (Arrj));

            if ((bCampaignJob == true && (bSprWarrJob == true || bWarrJob == true)) ||
                        (bSprWarrJob == true && (bCampaignJob == true || bWarrJob == true)) ||
                        (bWarrJob == true && (bSprWarrJob == true || bCampaignJob == true))) {
                //debugger;
                alert("Please Jobcode " + ('J' + Arrj) + " should not present under multiple warranty.");
                return false;
            }
        }
    }
    //Jobcode validations END        
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;

}

function CheckEstimate(Confirm) {
    var sMessage = "";
    var ObjControl;
    var sJobtype = "";
    //chassis 
    //alert("Hi");
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtChassisID");
    if (ObjControl != null) {
        if (ObjControl.value == "" || dGetValue(ObjControl.value) == 0) sMessage = sMessage + "Please Select Chassis.";
    }
    //DrpSupervisorName
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpSupervisorName");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select Supervisor Name.";
    }
    //debugger;

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    //Part Validations START    
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    var Partcnt = 0;
    var LabourCnt = 0;
    ////debugger;
    for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
        var objtxtReqControl = ObjGrid.rows[i].cells[4].childNodes[1];
        var ObjChkForDelete = ObjGrid.rows[i].cells[9].childNodes[1].childNodes[0];
        if (ObjChkForDelete.checked == false) {
            var ReqQty = dGetValue(objtxtReqControl.value);

            if (ReqQty == 0 || ReqQty == "") {
                alert("Estimated Qty should not be blank For Part/Lub." + dGetValue(i - 1));
                return false;
            }
            cnt = cnt + 1;
        }
    }
    Partcnt = cnt;
    //Part Validations END
    //Labour Validation START
    var ObjLGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");

    if (ObjLGrid == null) return;
    //var ObjLGrid = null;
    ////debugger;
    cnt = 0;
    for (var i = 2; i <= ObjLGrid.rows.length - 1; i++) {
        var ObjLChkForDelete = ObjLGrid.rows[i].cells[12].childNodes[1].childNodes[0];
        if (ObjLChkForDelete.checked == false) {

            var objLCode = ObjLGrid.rows[i].cells[2].childNodes[3];
            var objDrpLAddDesc = ObjLGrid.rows[i].cells[4].childNodes[1];

            var sLstTwoDigit = objLCode.value.toString().substr(objLCode.value.toString().length - 2, 2);
            var sFirstFiveDigit = objLCode.value.toString().substr(0, 5);

            cnt = cnt + 1;

            //if (objDrpLAddDesc.value == "0" && (sLstTwoDigit == "99" || sFirstFiveDigit == "33333" || sFirstFiveDigit == "44444" || sFirstFiveDigit == "55555")) {
            if (objDrpLAddDesc.value == "0" && (sFirstFiveDigit == "MTIMI")) {
                alert("Fill Additional Labor Description for Labor line " + dGetValue(i - 1));
                return false;
            }

            var objtxtLSubletAmt = ObjLGrid.rows[i].cells[10].childNodes[1];
            var objtxtSubletDesc = ObjLGrid.rows[i].cells[11].childNodes[1];
            var SubletAmt = dGetValue(objtxtLSubletAmt.value);
            var LSubletDesc = objtxtSubletDesc.value.trim();

            if (SubletAmt == 0 && sFirstFiveDigit == "MTIOU") {
                alert("Fill Sublet Amount for Labor line " + dGetValue(i - 1));
                return false;
            }
            if (SubletAmt > 0 && LSubletDesc == "") {
                alert("Select Sublet Description for line " + dGetValue(i - 1));
                return false;
            }
        }
    }//Labour VAlidation END
    LabourCnt = cnt;

    if (LabourCnt == 0 && Partcnt == 0 && Confirm == "Y") {
        alert("Select Details for Estimate");
        return false;
    }


    return true;
}

function ValidateVPLNT(Confirm) {

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    for (var i = 1; i <= ObjGrid.rows.length - 1; i++) {
        var objtxtBalControl = ObjGrid.rows[i].cells[5].childNodes[0];
        var objtxtPlantControl = ObjGrid.rows[i].cells[6].childNodes[0];

        var BalQty = dGetValue(objtxtBalControl.innerText)
        var PlantQty = dGetValue(objtxtPlantControl.value)

        if (BalQty < PlantQty) {
            alert("Please Enter the Plant Qty less than Balance Qty at line " + i);
            return false;
        }
        if (PlantQty == 0) {
            cnt = cnt + 1;
        }
    }

    if (Confirm == "Y" && cnt == ObjGrid.rows.length - 1) {
        alert("Please Enter the Plant Qty.");
        return false;
    }
}

function ValidateSPLNT(Confirm) {

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    for (var i = 1; i <= ObjGrid.rows.length - 1; i++) {
        var objtxtBalControl = ObjGrid.rows[i].cells[5].childNodes[0];
        var objtxtPlantControl = ObjGrid.rows[i].cells[6].childNodes[0];

        var BalQty = dGetValue(objtxtBalControl.innerText)
        var PlantQty = dGetValue(objtxtPlantControl.value)

        if (BalQty < PlantQty) {
            alert("Please Enter the Plant Qty less than Balance Qty at line " + i);
            return false;
        }
        if (PlantQty == 0) {
            cnt = cnt + 1;
        }
    }

    if (Confirm == "Y" && cnt == ObjGrid.rows.length - 1) {
        alert("Please Enter the Plant Qty.");
        return false;
    }
}

function ValidateVEXPORT(Confirm) {

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;

    var cnt = 0;
    for (var i = 1; i <= ObjGrid.rows.length - 1; i++) {
        var objtxtBalControl = ObjGrid.rows[i].cells[5].childNodes[0];
        var objtxtBillControl = ObjGrid.rows[i].cells[6].childNodes[0];

        var BalQty = dGetValue(objtxtBalControl.innerText)
        var BillQty = dGetValue(objtxtBillControl.value)

        if (BalQty < BillQty) {
            alert("Please Enter the Bill Qty less than Balance Qty at line " + i);
            return false;
        }
        if (BillQty == 0) {
            cnt = cnt + 1;
        }
    }

    if (Confirm == "Y" && cnt == ObjGrid.rows.length - 1) {
        alert("Please Enter the Bill Qty.");
        return false;
    }
}

function ValidateSEXPORT(Confirm) {

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");

    if (ObjGrid == null) return;
    var ObjControl = null;
    var cnt = 0;
    for (var i = 1; i <= ObjGrid.rows.length - 1; i++) {
        var objtxtBalControl = ObjGrid.rows[i].cells[5].childNodes[0];
        var objtxtBillControl = ObjGrid.rows[i].cells[6].childNodes[0];

        var BalQty = dGetValue(objtxtBalControl.innerText)
        var BillQty = dGetValue(objtxtBillControl.value)

        if (BalQty < BillQty) {
            alert("Please Enter the Bill Qty less than Balance Qty at line " + i);
            return false;
        }
        if (BillQty == 0) {
            cnt = cnt + 1;
        }
    }

    if (Confirm == "Y" && cnt == ObjGrid.rows.length - 1) {
        alert("Please Enter the Bill Qty.");
        return false;
    }
}

//Megha 23092011
//To check Dynamic Master Save Validation
function CheckforSaveDynamicFormControls() {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";

    var i;
    var ObjControl;
    var ObjControlonlyDate;
    var controlid;
    var ControlContainer = document.getElementById(PcontainerName + 'ControlContainer');
    var colcount = 0;
    //var colcount=ControlContainer.children[0].children.length;
    ObjControl = document.getElementById(PcontainerName + 'txtControlCount');
    if (ObjControl == null) {
        return;
    }

    if (ObjControl != null) {
        colcount = ObjControl.value;
    }

    for (i = 1; i <= colcount; i++) {
        controlid = PcontainerName + i;
        ObjControl = document.getElementById(controlid)

        if (ObjControl != null) {
            if (ObjControl.type == "text") {
                if (ObjControl.value == "") {
                    alert("Please Enter record");
                    ObjControl.focus();
                    return false;
                }
            }
            else if (ObjControl.type == "select-one") {
                if (ObjControl.value == "0") {
                    alert("Please Select record");
                    ObjControl.focus();
                    return false;
                }
            }
        }

    }
}
//Megha 23092011    

// To  Check RFQ Validation
function CheckRFQValidation() {
    //Check Details Working
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    if (ObjGrid == null) return;
    var ObjControl = null;
    var iCntOfSelect = 0, iCntOfDelete = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        if (ObjControl != null) {
            if (ObjControl.value != "") {
                iCntOfSelect = iCntOfSelect + 1;
            }
        }
        ObjControl = ObjGrid.rows[i].cells[3].children[0].children[0];
        if (ObjControl != null) {
            if (ObjControl.checked == true) {
                iCntOfDelete = iCntOfDelete + 1;
            }
        }
    }
    if (iCntOfDelete == iCntOfSelect) {

        alert("Please keep at least one record at details!");
        return false;
    }
    return true;
}
//
// To  Check vehicle Vehicle ORF Validation
function CheckVehicleORFValidation() {
    var sMessage = "";
    var ObjControl;

    //Expected Date Of LC
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDummyLC");
    if (ObjControl != null) {
        if (ObjControl.value == "Y") {
            ObjControl = window.document.getElementById("ContentPlaceHolder1_txtLCExptDate_txtDocDate");
            if (ObjControl != null) {
                if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter the Expected Date Of LC Availablity !.";
            }
        }
    }

    //ExportIncentive
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpExportsIncentive");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Exports Incentive.";
    }

    // Port Of Loading
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPortofLoading");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Port Of Loading.";
    }

    //drpContainerisationLocation
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpContainerisationLocation");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Containerisation Location.";
    }

    // drpClearingHandlingAgent
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpClearingHandlingAgent");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Clearing Handling Agent.";
    }

    //    // drpPricingApprovalRequired
    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPricingApprovalRequired");
    //    if (ObjControl != null) {
    //        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Pricing Approval Required.";
    //    }

    // drpPricingApprovalAttached
    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPricingApprovalAttached");
    //    if (ObjControl != null) {
    //        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Pricing Approval Attached.";
    //    }

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;
}

// To  Check vehicle Proforma Validation
function CheckVehicleProformaValidation() {
    if (CheckRFPHdrValidation() == false) return false;
    var sMessage = "";
    var ObjControl;

    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPaymentTerms");
    //    if (ObjControl != null) 
    //    {
    //        if (ObjControl.selectedIndex == 1) sMessage = sMessage + "\n Please Select The Payment Terms Other Than 'Others'.";
    //    }

    //Check MultiModalShipment
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpMultiModalShipment");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            sMessage = sMessage + "\n Please Select The MultiModal Shipment.";
        }
        else if (ObjControl.selectedIndex == 1) {
            ObjControl = window.document.getElementById("ContentPlaceHolder1_drpMultiModalDestination");
            if (ObjControl != null)
                if (ObjControl.selectedIndex == 0) {
                    sMessage = sMessage + "\n Please Select The MultiModal Destination.";
                }
        }
    }

    // Check for Partial shipment Allowed
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPartialshipmentAllowed");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            sMessage = sMessage + "\n Please Select The Partial Shipment Allowed.";
        }
    }

    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpValidityDays");
    //    if 
    //(ObjControl != null) {
    //        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Validity Days.";
    //    }



    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtLastDateNegotiation");
    //    if (ObjControl != null)
    //        if (ObjControl.value == 0) {
    //            sMessage = sMessage + "\n Please Select The Last Date Negotiation.";
    //    }

    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    if (ObjGrid == null) return;
    var bCheckValidation = true;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[16].children[0];

        if (ObjControl.children[0].checked == true) {
            // Check Quantity is enter           
            objtxtControl = ObjGrid.rows[i].cells[4].children[0];
            if (dGetValue(objtxtControl.value) == 0) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
                bCheckValidation = false;
            }
            // Check For FOBRate
            ObjControl = ObjGrid.rows[i].cells[5].children[0];
            if (dGetValue(ObjControl.value) == 0) {
                sMessage = sMessage + " \n Please Enter the FOB Rate at line " + i;
                bCheckValidation = false;
            }

            // Check For Freight
            if (ObjGrid.rows[i].cells[6].style.display == "") {
                ObjControl = ObjGrid.rows[i].cells[6].children[0];
                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
                {
                    sMessage = sMessage + " \n Please Enter the Freight at line " + i;
                    bCheckValidation = false;
                }
            }

            // Check For Insurance
            if (ObjGrid.rows[i].cells[7].style.display == "") {
                ObjControl = ObjGrid.rows[i].cells[7].children[0];

                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
                {
                    sMessage = sMessage + " \n Please Enter the Insurance at line " + i;
                    bCheckValidation = false;
                }
            }
            // Check For Carriage
            if (ObjGrid.rows[i].cells[8].style.display == "") {
                ObjControl = ObjGrid.rows[i].cells[8].children[0];
                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
                {
                    sMessage = sMessage + " \n Please Enter the Carriage at line " + i;
                    bCheckValidation = false;
                }
            }

            // Check For AgentCommission
            if (ObjGrid.rows[i].cells[9].style.display == "") {
                ObjControl = ObjGrid.rows[i].cells[9].children[0];
                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
                {
                    sMessage = sMessage + "  \n Please Enter the Commission at line " + i;
                    bCheckValidation = false;
                }
            }
            // Check For OtherCharges
            if (ObjGrid.rows[i].cells[10].style.display == "") {
                ObjControl = ObjGrid.rows[i].cells[10].children[0];

                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
                {
                    sMessage = sMessage + " \n Please Enter the OtherCharges at line " + i;
                    bCheckValidation = false;
                }
            }

            // Check For Taxes
            if (ObjGrid.rows[i].cells[11].style.display == "") {
                ObjControl = ObjGrid.rows[i].cells[11].children[0];
                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
                {
                    sMessage = sMessage + " \n Please Enter the Taxes at line " + i;
                    bCheckValidation = false;
                }
            }
            //            // Check For Discount
            //            if (ObjGrid.rows[i].cells[12].style.display == "") {
            //                ObjControl = ObjGrid.rows[i].cells[12].children[0];
            //                if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
            //                {
            //                    sMessage = sMessage + " \n Please Enter the Taxes at line " + i;
            //                    bCheckValidation = false;
            //                }
            //                else {

            //                    if (dGetValue(ObjControl.value) == 0) {
            //                        sMessage = sMessage + " \n Please Enter the Discount Amount greater than 0 at line " + i;
            //                        bCheckValidation = false;
            //                    }
            //                }
            //            }

            // Check ShipMent Days
            ObjControl = ObjGrid.rows[i].cells[15].children[0];
            if (ObjControl.selectedIndex == 0) {
                bCheckValidation = false;
                sMessage = sMessage + "\n Please select the Shipment Days At Line " + i;
            }

        }
    }
    //    var ObjHiddenValue = window.document.getElementById("ContentPlaceHolder1_HiddenPrintYesNo");
    //    if (confirm('Signature Should include in Printout??') == true)
    //        ObjHiddenValue.value = true;
    //    else
    //        ObjHiddenValue.value = false;

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    return true;
}
// To  Check Spares Proforma Validation
function CheckSparesProformaValidation() {
    if (CheckRFPHdrValidation() == false) return false;
    var sMessage = "";
    var ObjControl;
    //Check Multimodal Shipmet
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpMultiModalShipment");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            sMessage = sMessage + "\n Please Select The MultiModal Shipment.";
        }
        else if (ObjControl.selectedIndex == 1) {
            ObjControl = window.document.getElementById("ContentPlaceHolder1_drpMultiModalDestination");
            if (ObjControl != null)
                if (ObjControl.selectedIndex == 0) {
                    sMessage = sMessage + "\n Please Select The MultiModal Destination.";
                }
        }
    }

    // Shipment Days
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpShipmentDays");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Shipment Days .";
    }

    // Check for Partial shipment Allowed
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPartialshipmentAllowed");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            sMessage = sMessage + "\n Please Select The Partial Shipment Allowed.";
        }
    }

    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpValidityDays");
    //    if (ObjControl != null) {
    //        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Validity Days.";
    //    }



    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtLastDateNegotiation");
    //    if (ObjControl != null)
    //        if (ObjControl.value == 0) {
    //        sMessage = sMessage + "\n Please Select The Last Date Negotiation.";
    //    }

    //Check Details Working
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_IncoDetails");
    if (ObjGrid == null) return;
    var bCheckValidation = true;
    for (var i = 2; i < 9; i++) {
        // Check For Inco Column Details
        if (ObjGrid.rows[1].cells[i].style.display == "") {
            ObjControl = ObjGrid.rows[1].cells[i].children[0];
            if (ObjControl.value == "") //if (dGetValue(ObjControl.value) == 0)
            {
                sMessage = sMessage + " \n Please Enter the " + ObjGrid.rows[0].cells[i].innerHTML;
                bCheckValidation = false;
            }
            else {
                if (dGetValue(ObjControl.value) == 0 && ObjGrid.rows[0].cells[i].innerHTML == "Discount Amount") {
                    sMessage = sMessage + " \n Please Enter the Discount Amount greater than 0";
                    bCheckValidation = false;
                }
            }
        }

    }
    if (sMessage != "") {

        alert(sMessage);
        return false;
    }
    return true;
}
// To Check Spares Packing List Validation
function CheckSparesPackingListValidation() {
    var ObjControl, sMessage = "";

    //No Of the Boxes
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtNoOfBoxes");
    if (ObjControl != null) {
        if (dGetValue(ObjControl.value) == 0) sMessage = sMessage + "\n Please Enter The No. Of Boxes.";
    }
    //Net Weight
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtNoOfBoxes");
    if (ObjControl != null) {
        if (dGetValue(ObjControl.value) == 0) sMessage = sMessage + "\n Please Enter The No. Of Boxes.";
    }
    //Gross Weight
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtNetWeight");
    if (ObjControl != null) {
        if (dGetValue(ObjControl.value) == 0) sMessage = sMessage + "\n Please Enter The Net Weight.";
    }

    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[4].children[0];
        if (ObjControl != null) {
            if (ObjControl.value == "") {
                alert("Please Enter The Box No at Line " + i);
                return false;
            }
        }

    }
}
// To Check Preshipment Validation
function CheckPreshipmentValidation() {
    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    if (ObjGrid == null) return;
    var ObjControl;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[14].children[0];
        if (ObjControl != null) {
            if (ObjControl.value == "") {
                alert("Please Select The Shipment Date At Line " + i + " !");
                return false;
            }
        }

    }
}
// To Check RFP Validation
function CheckRFPHdrValidation() {

    var sMessage = "";
    var ObjControl;

    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPaymentTerms");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Payment Terms.";
    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpThirdPartyInspectionAgency");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0)
            sMessage = sMessage + "\n Please Select The Third Party Inspection Agency Name.";
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpIncoTerms");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            sMessage = sMessage + "\n Please Select The Shipment Terms.";
        }
        else {
            var sShipmentTerms = ObjControl.options[ObjControl.selectedIndex].text;
            if (sShipmentTerms == "FOB") {
                ObjControl = window.document.getElementById("ContentPlaceHolder1_drpNominatedAgency");
                if (ObjControl != null) {
                    if (ObjControl.selectedIndex == 0) {
                        sMessage = sMessage + "\n Please Select The Nominated Agency.";
                    }
                }
                ObjControl = window.document.getElementById("ContentPlaceHolder1_drpShippingLineNominationRequired");
                //                 if (ObjControl != null) 
                //                 {
                //                     if (ObjControl.selectedIndex == 0) 
                //                     {
                //                         sMessage = sMessage + "\n Please Select Shipping Line Nomination Required.";
                //                     }
                //                 }

            }
            else {
                ObjControl = window.document.getElementById("ContentPlaceHolder1_drpNominatedAgency");
                if (ObjControl != null) ObjControl.selectedIndex = 0;

                ObjControl = window.document.getElementById("ContentPlaceHolder1_drpShippingLineNominationRequired");
                if (ObjControl != null) ObjControl.selectedIndex = 1;
            }
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpModeofShipment");
    if (ObjControl != null)
        if (ObjControl.selectedIndex == 0) {
            sMessage = sMessage + "\n Please Select The Mode Of Shipment.";
        }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPortofDischarge");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Port Of Discharge.";
    }



    if (sMessage != "") {

        alert(sMessage);
        return false;
    }
    return true;
}
// To Check Vehicle RFP Vlaidation 
function CheckVRFPValidation() {
    if (CheckRFPHdrValidation() == false) return false;
    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";

    //Sujata 14012011
    //Check How many rows get added into details
    var total = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        if (ObjControl.selectedIndex != 0) {
            total = total + 1;
        }
    }
    // If No rows then tell to User to add atleast one reocrd in details section
    if (total == 0) {
        alert("Please select atleast One Record in Detail");
        return false;
    }
    // If there are no of rows then check wheather 1st row should not blank
    if (total > 0) {
        var sRFPType = ObjGrid.rows[1].cells[1].children[0].selectedIndex;
        if (sRFPType == 0) {
            alert("Please select Record at line No. 1");
            return false;
        }
    }
    //Sujata 14012011

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check CBU/SKD/CKD
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        if (ObjControl.selectedIndex != 0) {
            // Sujata 14012011
            // Compare with 1st row type
            if (sRFPType != ObjControl.selectedIndex) {
                sMessage = sMessage + "\n Please select Same Type as line No. 1 To the Line No." + i;
            }
            // Sujata 14012011

            // check Model Name 
            ObjControl = ObjGrid.rows[i].cells[2].children[0];
            if (ObjControl.selectedIndex == 0) {
                sMessage = sMessage + "\n Please select the model details at line " + i;
            }

            // Check Quantity is enter           
            objtxtControl = ObjGrid.rows[i].cells[4].children[0];
            if (objtxtControl.value == "0" || objtxtControl.value == "" || objtxtControl.value == null) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
            }

            if (ObjGrid.rows[i].cells[5].children[0].children[0].checked == true) {
                iCountDel = iCountDel + 1;
            }
        }
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    iCountDel = iCountDel + 1;
    if (iCountDel == ObjGrid.rows.length) {
        alert("Please keep at least one record at details!");
        return false;
    }
    return true;
}
// To Check Spares RFP Vlaidation
function CheckSRFPValidation() {
    if (CheckRFPHdrValidation() == false) return false;

    var ObjControl;

    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPoType");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select The PO Type!.");
            return false;
        }
    }

    var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartGrid");
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check Part ID
        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            iPartID = dGetValue(ObjControl.value);
            if (iPartID != 0) {
                // Check Quantity is enter
                objtxtControl = ObjGrid.rows[i].cells[5].children[0];
                iQty = dGetValue(objtxtControl.value);
                if (iQty == 0) {
                    sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
                }
                // Commented by Shyamal on 02062012,Already cover in code behind
                //            if (ObjGrid.rows[i].cells[8].children[0].children[0].checked == true) {
                //                iCountDel = iCountDel + 1;
                //            }            
            }
        }
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    // Commented by Shyamal on 02062012,Already cover in code behind
    //    iCountDel = iCountDel + 1;
    //    if (iCountDel == ObjGrid.rows.length) {
    //        alert("Please keep at least one record at details!");
    //        return false;
    //    }
    return true;
}
//check before Confirm
function CheckForConfirm(ID) {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    var Control = "";
    if (parseInt(ID) == 32) {
        Control = document.getElementById('txtID');
    }
    else {
        if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
        Control = document.getElementById(PcontainerName + 'txtID');
    }
    if (Control == null) return;
    if (Control.value == "") {
        alert("Please Select The Record To Confirm!");
        return false;
    }
    if (ID == 0) {
        return true;
    }

    else if (ID == 76) {
        var ObjControl;
        var iPartID = null;
        var iDealer = null;
        var iMTI = null;
        ObjGrid = window.document.getElementById("ContentPlaceHolder1_QuotationDtls");

        ObjControl = ObjGrid.rows[3].cells[3].children[0];
        iPartID = dGetValue(ObjControl.value);
        if (iPartID == "0") {
            var sMsg = "Discount Offered Amount is 0.\nAre you sure, you want to continue?";
            if (confirm(sMsg) == true) {
                return true;
            }
            else {
                return false;
            }
        }

        var ObjControlDealer;
        ObjControlDealer = ObjGrid.rows[4].cells[3].children[0];
        iDealer = dGetValue(ObjControlDealer.value);
        var ObjControlMTI;
        ObjControlMTI = ObjGrid.rows[5].cells[3].children[0];
        iMTI = dGetValue(ObjControlMTI.value);

        if (iPartID != "0") {
            if (iDealer == "0" && iMTI == "0") {
                alert("Discount Offered Amount is greater than 0,Dealer Share and MTI Share both can not be 0");
                return false;
            }
            else {
                return true;
            }
        }





    }
        //Sujata  18022011
    else if (ID == 1)//CheckFor Vehicle RFP
    {
        if (CheckVRFPValidation() == false) {
            return false;
        }
        else {
            var ObjControl = "";
            ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPaymentTerms");
            if (ObjControl.options[ObjControl.selectedIndex].value == "1") {
                ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRemarks");
                if (ObjControl != null) {
                    if (ObjControl.value == "") {
                        alert("Please Enter the Remarks For 'Others' Payment Term!");
                        return false;
                    }
                }
            }
        }
    }
    else if (ID == 2)//CheckFor Vehicle RFP
    {
        if (CheckPOSaveValidation() == false) {
            return false;
        }
    }
        //Sujata  18022011
    else if (ID == 3)//CheckFor Vehicle Proforma
    {
        var ObjHiddenValue = window.document.getElementById("ContentPlaceHolder1_HiddenPrintYesNo");
        if (ObjHiddenValue != null) {
            //Sujata 03082013_Begin        
            //            if (confirm('Signature Should include in Printout??') == true)
            //                ObjHiddenValue.value = true;
            //            else
            //                ObjHiddenValue.value = false;
            //Sujata 03082013_End       
            ObjHiddenValue.value = true;
            if (CheckVehicleProformaValidation() == false) {
                return false;
            }

        }

    }
    else if (ID == 5)//CheckFor Preshipment
    {
        if (CheckPreshipmentValidation() == false) {
            return false;
        }
    }
    else if (ID == 7) { // Stock Transfer Receipt
        if (CheckStkTrnsReceiptSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 8)//CheckFor Packing Slip
    {
        if (CheckSparesPackingListValidation() == false)
            return false;
    }
    else if (ID == 9)//CheckFor Parts RFP
    {
        if (CheckPartsRFPSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 10)//CheckFor Parts Proforma
    {
        if (CheckPartsProformaSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 21) //Check LC function
    {
        if (CheckLCValidation() == false) {
            return false;
        }
    }
    else if (ID == 22) //Check Advance Payment function
    {
        if (CheckAdvPaymentValidation() == false) {
            return false;
        }
    }
    else if (ID == 23) //Check Acivity Master function
    {
        if (CheckActivityMasterValidation() == false) {
            return false;
        }
    }
    else if (ID == 24) //Check Acivity Claim function
    {
        if (CheckActivityValidation() == false) {
            return false;
        }
    }
    else if (ID == 240) //Check Acivity Claim function
    {
        if (CheckActivityClaimValidation() == false) {
            return false;
        }
    }
        //else if (ID == 26) //Check Installation Function
        //{
        //    if (ValidateInstallationCer() == false) {
        //        return false;
        //    }
        //}
    else if (ID == 27) //Check VehicleIN Function
    {
        if (ValidateVehicleIN() == false) {
            return false;
        }
    }
    else if (ID == 28) //Check Wrranty function
    {
        if (CheckMRN() == false) {
            return false;
        }
    }

    else if (ID == 29) //Check Dummy LC Function
    {
        if (CheckDummyLCValidation() == false) {
            return false;
        }
    }

    else if (ID == 31) //Check Warranty Service History
    {
        if (ValidateINSProcess() == false) {
            return false;
        }
    }

    else if (parseInt(ID) == 32) {
        if (CheckMRNRemark() == false) {
            return false;
        }
    }

    else if (ID == 40) //Check Wrranty function
    {   //Sujata 19012011
        //if (CheckWarrantyValidation() == false) {
        if (CheckWarrantyValidationForConfirm() == false) {
            //Sujata 19012011
            return false;
        }
    }
    else if (ID == 51) //Check FPDA Function
    {
        //debugger;
        if (CheckFPDA() == false) {
            return false;
        }
    }
    else if (ID == 54) //Check FPDA Function
    {
        if (CheckStockingNorms() == false) {
            return false;
        }
    }
        //Sujata 12122012
    else if (ID == 41) //Check Vehicle Plant Details  Function
    {
        if (ValidateVPLNT("Y") == false) {
            return false;
        }
    }         //Sujata 12122012
    else if (ID == 42) //Check Spares Plant Details Function
    {
        if (ValidateSPLNT("Y") == false) {
            return false;
        }
    }
    else if (ID == 43) //Check Spares Plant Details Function
    {
        if (CheckExportRequestValidation("Y") == false) {
            return false;
        }
    }
    else if (ID == 44) //Check Spares Plant Details Function
    {
        if (CheckExportWarrantyValidation("Y") == false) {
            return false;
        }
    }
    else if (ID == 61) {
        if (CheckSRNValidation() == false) {
            return false;
        }
    }

    else if (ID == 62) //Customer Creation
    {
        if (CheckCustomerCreationValidation() == false) {
            return false;
        }
    }
    else if (ID == 63) //Parts Sales OA
    {
        if (CheckOAValidation() == false) {
            return false;
        }
    }
    else if (ID == 64) //Parts Sales Invoice
    {
        if (CheckOAInvValidation() == false) {
            return false;
        }
    }
    else if (ID == 67) // Jobcard Transaction
    {
        ////debugger;
        if (CheckJobcard("Y") == false) {
            return false;
        }
    }

    else if (ID == 68) // Jobcard Transaction
    {
        ////debugger;
        if (CheckEstimate("Y") == false) {
            return false;
        }
    }
    else if (ID == 72) // Material Receipt
    {
        if (CheckMaterialReceiptSaveValidation() == false) {
            return false;
        }
    }
    else if (ID == 74)// For CRM Service
    {
        debugger;
        var objMenuid;
        var ObjControl;
        var ObjUserID;
        objMenuid = window.document.getElementById("ContentPlaceHolder1_txtMenuid");
        ObjControl = window.document.getElementById("ContentPlaceHolder1_drpDealerName");
        ObjUserID = window.document.getElementById("ContentPlaceHolder1_txtUserid");
        if (objMenuid.value == "670" && (ObjUserID.value == "24" || ObjUserID.value == "308" || ObjUserID.value == "318" || ObjUserID.value == "319" || ObjUserID.value == "320" || ObjUserID.value == "321" || ObjUserID.value == "728")) //Amrit id
        {
            if (ObjControl != null) {
                if (ObjControl.selectedIndex == 0) {
                    alert("Please Select The Dealer Name!.");
                    return false;
                }
            }
        }
    }
    else if (ID == 75)// PRN 
    {
        if (CheckPRNSaveValidation() == false) {
            return false;
        }
    }
    if (confirm("Are you sure, you want to confirm the document?") == true) {

    }
    else return false;
}

//Sujata 19012011

function CheckExportWarrantyValidation(Confirm) {
    if (CheckClaimValidation() == false) {
        return false;
    }
    //Hrs Applicable
    var ObjControl = null;
    debugger;
    //txtHrsReading
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtHrsReading");
    if (ObjControl != null) {
        if (ObjControl.disabled == false) {
            if (ObjControl.value == "" || ObjControl.value == "0") {
                alert("Please Enter The Hrs Reading in Vehicle Details Tab. ");
                return false;
            }
        }
    }
    //txtOdometer
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtOdometer");
    if (ObjControl != null) {
        if (ObjControl.disabled == false) {
            if (ObjControl.value == "" || ObjControl.value == "0") {
                alert("Please Enter The Odometer Reading in Vehicle Details Tab. ");
                return false;
            }
        }
    }
    
    var iPartcnt = 0;
    var iLubcnt = 0;
    var iLabourcnt = 0;
    var iSubletcnt = 0;

    var ArrAllJobs = new Array();

    var ObjDrpInvType = window.document.getElementById("ContentPlaceHolder1_DrpInvType");
    if (ObjDrpInvType[ObjDrpInvType.selectedIndex].value == "P") {
        var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

        if (ObjGrid == null) return;        
        //debugger;
        for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
            var objPartChk = ObjGrid.rows[i].cells[16].childNodes[1].childNodes[0].checked;
            //var objLubLoc = ObjGrid.rows[i].cells[31].childNodes[1];
            if (objPartChk == false) {
                iPartcnt = iPartcnt + 1;
                var objMechName = ObjGrid.rows[i].cells[5].childNodes[1];
                //var objLubLoc = ObjGrid.rows[i].cells[32].childNodes[1];
                //var objLubCap = ObjGrid.rows[i].cells[33].childNodes[1];
                var WarrQty = dGetValue(ObjGrid.rows[i].cells[9].childNodes[1].value);
                //var sLubLocID = objLubLoc.value;
                //var sLubCapacity = dGetValue(objLubCap[objLubCap.selectedIndex].text.trim());            

                var objMake = ObjGrid.rows[i].cells[4].childNodes[1];
                var objJobcode = ObjGrid.rows[i].cells[12].childNodes[1];

                if (WarrQty == 0 || WarrQty == "") {
                    alert("Warranty Qty should not be blank For Part " + dGetValue(i - 1));
                    return false;
                }
                if (objMechName.value == "0") {
                    //Confirm == "Y" &&
                    alert("Fill Mechanic Details for Part line " + dGetValue(i - 1));
                    objMechName.focus();
                    return false;
                }
                //if (sPartType == "O" && sLubLocID == "0") {
                //    alert("Select Lubricant Location for Part line " + dGetValue(i - 1));
                //    return false;
                //}           

                if (objMake.value == "0") {
                    alert("Select Failed Part Make for Part line " + dGetValue(i - 1));
                    return false;
                }
                if (Confirm == "Y" && objJobcode.value == "0") {
                    alert("Select Jobcode for Part line " + dGetValue(i - 1));
                    return false;
                }
                if (objJobcode.value != "0") {
                    if (bCheclValueExistInArray(ArrAllJobs, objJobcode.value) == false) ArrAllJobs.push(objJobcode.value);
                }
            }
        }
    }
    debugger;
    if (ObjDrpInvType[ObjDrpInvType.selectedIndex].value == "L") {
        ObjGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
        if (ObjGrid == null) return;


        for (var i = 2; i < ObjGrid.rows.length; i++) {
            var objLabourChk = ObjGrid.rows[i].children[11].childNodes[1].childNodes[0].checked;
            if (objLabourChk == false) {
                //Get Lable of Labour ID         
                iLabourcnt = iLabourcnt + 1;
                var lblLabjobcode = ObjGrid.rows[i].children[7].childNodes[1];
                var lblLabTotal = ObjGrid.rows[i].children[6].childNodes[1].value;
                var lblLabCd = ObjGrid.rows[i].children[2].children[1];
                var objDrpLAddDesc = ObjGrid.rows[i].children[3].childNodes[3];

                if (objDrpLAddDesc.value == "0" && (lblLabCd.value.substr(0, 5) == "MTIMI")) {
                    alert("Fill Additional Labor for Labour line " + dGetValue(i - 1));
                    return false;
                }

                if ((lblLabTotal == "" || dGetValue(lblLabTotal) == 0) && (lblLabCd.value.substr(0, 5) == "MTIMI" || lblLabCd.value.substr(0, 5) == "MTICC")) {
                    alert("Enter Total Amount for Labour line " + dGetValue(i - 1));
                    return false;
                }


                //if (sPartType == "O" && sLubLocID == "0") {
                //    alert("Select Lubricant Location for Part line " + dGetValue(i - 1));
                //    return false;
                //}        

                if (Confirm == "Y" && lblLabjobcode.value == "0") {
                    alert("Select Jobcode for Labour line " + dGetValue(i - 1));
                    return false;
                }
                if (lblLabjobcode.value != "0") {//Confirm == "Y" &&            
                    if (bCheclValueExistInArray(ArrAllJobs, lblLabjobcode.value) == false) ArrAllJobs.push(lblLabjobcode.value);
                }
            }
        }
    }
    //debugger;
    if (ObjDrpInvType[ObjDrpInvType.selectedIndex].value == "P") {
        ObjGrid = window.document.getElementById("ContentPlaceHolder1_LubricantDetailsGrid");
        if (ObjGrid == null) return;


        for (var i = 2; i < ObjGrid.rows.length; i++) {
            var objLubChk = ObjGrid.rows[i].children[10].childNodes[1].childNodes[0].checked;
            if (objLubChk == false) {
                iLubcnt = iLubcnt + 1;
                var lblLubjobcode = ObjGrid.rows[i].children[6].childNodes[1];

                var WarrQty = dGetValue(ObjGrid.rows[i].children[2].childNodes[1].value);
                var sLubLocID = dGetValue(ObjGrid.rows[i].children[19].childNodes[1].value);
                if (sLubLocID == "0") {
                    alert("Select Lubricant Location for Lubricant line " + dGetValue(i - 1));
                    return false;
                }
                if (WarrQty == "0" || WarrQty == "") {
                    alert("Warranty Qty should not be blank For Lubricant line " + dGetValue(i - 1));
                    return false;
                }
                if (Confirm == "Y" && lblLubjobcode.value == "0") {
                    alert("Select Jobcode for Lubricant line " + dGetValue(i - 1));
                    return false;
                }
                if (lblLubjobcode.value != "0") {//Confirm == "Y" &&            
                    if (bCheclValueExistInArray(ArrAllJobs, lblLubjobcode.value) == false) ArrAllJobs.push(lblLubjobcode.value);
                }
            }
        }
    }
    debugger;
    objtxtID = window.document.getElementById("ContentPlaceHolder1_txtID");
    if (objtxtID.value != "" && objtxtID.value != "0" && ObjDrpInvType[ObjDrpInvType.selectedIndex].value == "L") {

        ObjGrid = window.document.getElementById("ContentPlaceHolder1_SubletDetailsGrid");
        if (ObjGrid == null) return;

        for (var i = 1; i < ObjGrid.rows.length; i++) {
            var objsubChk = ObjGrid.rows[i].children[10].childNodes[1].childNodes[0].checked;
            var lblsubsAmt = ObjGrid.rows[i].children[5].childNodes[1];
            if (objsubChk == false && (lblsubsAmt.value != "0" && lblsubsAmt.value != "")) {
                iSubletcnt = iSubletcnt + 1;

                var lblsubsjobcode = ObjGrid.rows[i].children[6].childNodes[1];

                var lblsubDesc = ObjGrid.rows[i].children[2].childNodes[1];

                if (lblsubDesc.value.trim() == "") {
                    alert("Enter Description for Sublet line " + dGetValue(i));
                    return false;
                }
                if (lblsubsAmt.value == "0" || lblsubsAmt.value == "") {
                    alert("Enter Amount for Sublet line " + dGetValue(i));
                    return false;
                }
                if (Confirm == "Y" && lblsubsjobcode.value == "0") {
                    alert("Select Jobcode for Sublet line " + dGetValue(i));
                    return false;
                }
                if (lblsubsjobcode.value != "0") {//Confirm == "Y" &&            
                    if (bCheclValueExistInArray(ArrAllJobs, lblsubsjobcode.value) == false) ArrAllJobs.push(lblsubsjobcode.value);
                }
            }
        }
    }
   

    if (Confirm == "Y" && ObjDrpInvType[ObjDrpInvType.selectedIndex].value == "P" && dGetValue(iPartcnt) == 0 && dGetValue(iLubcnt) == 0) {
        alert("Please Enter Part or Lubricant Details before confirmation.");
        return false;
    }

    if (Confirm == "Y" && ObjDrpInvType[ObjDrpInvType.selectedIndex].value == "L" && dGetValue(iSubletcnt) == 0 && dGetValue(iLabourcnt) == 0) {
        alert("Please Enter Labour or Sublet Details before confirmation.");
        return false;
    }

    var arrLen = 0;
    arrLen = ArrAllJobs.length;

    for (Arrj = 1; Arrj <= arrLen; Arrj++) {
        if (iGetIndexOfValueFromArrayJob(ArrAllJobs, (Arrj)) == false) {
            alert("Please select Jobcode " + ('J' + Arrj) + " before select next jobcode.");
            return false;
        }
    }

    if (Confirm == "Y") {
        if (CheckComplaintValidation() == false) {
            return false;
        }
        if (CheckInvestigationValidation() == false) {
            return false;
        }
    }
}

//Check Claim Validation
function CheckRequestValidation() {
    var sMessage = "";
    var ObjControl;
    debugger;
    //Claim Types
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Claim Type in Vehicle Details Tab.";
    }

    //ModelCode
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtModelCode");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Model Code in Vehicle Details Tab.";
    }
    //Customer Name
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustomerName");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Customer Name in Vehicle Details Tab.";
    }
    //Customer Address
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustomerAddress");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Customer Address in Vehicle Details Tab.";
    }
    //Customer EmailAddress
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustEmail");
    if (ObjControl != null) {
        if (ObjControl.value != "" && ObjControl.readOnly == false) {
            var result = validEmail(document.getElementById("ContentPlaceHolder1_txtCustEmail").value)
            if (result != "") {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi\n\n  in Vehicle Details Tab.")
                document.getElementById("ContentPlaceHolder1_txtCustEmail").focus();
                return false;
            }
        }
    }

    
    //txtHrsReading
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtHrsReading");
    if (ObjControl != null) {
        if (ObjControl.disabled == false) {
            if (ObjControl.value == "" || ObjControl.value == "0") sMessage = sMessage + "\n Please Enter The Hrs Reading in Vehicle Details Tab.";
        }
    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtOdometer");
    if (ObjControl != null) {
        if (ObjControl.disabled == false) {
            if (ObjControl.value == "" || ObjControl.value == "0") sMessage = sMessage + "\n Please Enter The Odometer Reading in Vehicle Details Tab.";
        }
    }

    ////    //drpRouteType
    ////    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpRouteType");
    ////    if (ObjControl != null) {
    ////        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Route Type.";
    ////    }


    //txtVehicleNo
    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    //    if (ObjControl != null) {
    //        if (ObjControl.value == "15" ) {
    //            ObjControl = window.document.getElementById("ContentPlaceHolder1_txtVehicleNo");
    //            if (ObjControl != null) {
    //                if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Vehicle No in Vehicle Details Tab.";
    //            }        
    //        }         
    //    }


    //txtRepairOrderNo
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRepairOrderNo");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Repair Order No in Vehicle Details Tab.";
    }



    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    //    if (ObjControl != null) {
    //        if (ObjControl.value != "18") {
    //txtFailureDate_txtDocDate
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtFailureDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Failure Date in Vehicle Details Tab.";
    }

    //txtRepairOrderDate_txtDocDate
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRepairOrderDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Repair Order Date in Vehicle Details Tab.";
    }

    //txtRepairCompleteDate_txtDocDate
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Repair Complete Date in Vehicle Details Tab.";
    }

    //}
    //}
    //Remark
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDealerRemark");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Remark in Vehicle Details Tab.";
    }






    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDealerRemark");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Payment Terms in Vehicle Details Tab.";
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    //Sujata 19012011
    //if (CheckComplaintValidation() == false) {
    //   return false;
    //}
    //if (CheckInvestigationValidation() == false) {
    //    return false;
    //}
    //Sujata 19012011
    return true;
}

function CheckExportRequestValidation(Confirm) {
    if (CheckRequestValidation() == false) {
        return false;
    }
    var iPartcnt = 0;
    var iLubcnt = 0;
    var iLabourcnt = 0;
    var iSubletcnt = 0;

    var ObjCombo = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    if (ObjCombo.options[ObjCombo.selectedIndex].value == "16") {
        var ObjOption = window.document.getElementById("ContentPlaceHolder1_OptShareType_0");
        var ObjOption1 = window.document.getElementById("ContentPlaceHolder1_OptShareType_1");
        if (ObjOption.checked == true) {
            var objtxtVECVShare = window.document.getElementById("ContentPlaceHolder1_txtVECVShare");
            var objtxtDealerShare = window.document.getElementById("ContentPlaceHolder1_txtDealerShare");
            var objtxtCustomerShare = window.document.getElementById("ContentPlaceHolder1_txtCustomerShare");

            var dVECVShare = dGetValue(objtxtVECVShare.value);
            var dDealerShare = dGetValue(objtxtDealerShare.value);
            var dCustShare = dGetValue(objtxtCustomerShare.value);

            if (parseFloat(dVECVShare + dDealerShare + dCustShare) != parseFloat(100)) {
                alert("Share Percent value should be equal to 100 !");
                return false;
            }
        }
    }

    var ArrAllJobs = new Array();
        var ObjGrid = window.document.getElementById("ContentPlaceHolder1_PartDetailsGrid");

        if (ObjGrid == null) return;
        var ObjControl = null;
        //debugger;
        for (var i = 2; i <= ObjGrid.rows.length - 1; i++) {
            var objPartChk = ObjGrid.rows[i].cells[16].childNodes[1].childNodes[0].checked;
            //var objLubLoc = ObjGrid.rows[i].cells[31].childNodes[1];
            if (objPartChk == false) {
                iPartcnt = iPartcnt + 1;
                var objMechName = ObjGrid.rows[i].cells[5].childNodes[1];
                //var objLubLoc = ObjGrid.rows[i].cells[32].childNodes[1];
                //var objLubCap = ObjGrid.rows[i].cells[33].childNodes[1];
                var WarrQty = dGetValue(ObjGrid.rows[i].cells[9].childNodes[1].value);
                //var sLubLocID = objLubLoc.value;
                //var sLubCapacity = dGetValue(objLubCap[objLubCap.selectedIndex].text.trim());            

                var objMake = ObjGrid.rows[i].cells[4].childNodes[1];
                var objJobcode = ObjGrid.rows[i].cells[12].childNodes[1];

                if (WarrQty == 0 || WarrQty == "") {
                    alert("Warranty Qty should not be blank For Part " + dGetValue(i - 1));
                    return false;
                }
                if (objMechName.value == "0") {
                    //Confirm == "Y" &&
                    alert("Fill Mechanic Details for Part line " + dGetValue(i - 1));
                    objMechName.focus();
                    return false;
                }
                //if (sPartType == "O" && sLubLocID == "0") {
                //    alert("Select Lubricant Location for Part line " + dGetValue(i - 1));
                //    return false;
                //}           

                if (objMake.value == "0") {
                    alert("Select Failed Part Make for Part line " + dGetValue(i - 1));
                    return false;
                }
                if (Confirm == "Y" && objJobcode.value == "0") {
                    alert("Select Jobcode for Part line " + dGetValue(i - 1));
                    return false;
                }
                if (objJobcode.value != "0") {
                    if (bCheclValueExistInArray(ArrAllJobs, objJobcode.value) == false) ArrAllJobs.push(objJobcode.value);
                }
                if (ObjCombo.options[ObjCombo.selectedIndex].value == "16" && ObjOption1.checked == true) {
                    dVECVShare = dGetValue(ObjGrid.rows[i].cells[13].childNodes[1].value);
                    dDealerShare = dGetValue(ObjGrid.rows[i].cells[14].childNodes[1].value);
                    dCustShare = dGetValue(ObjGrid.rows[i].cells[15].childNodes[1].value);
                    if (parseFloat(dVECVShare + dDealerShare + dCustShare) != parseFloat(100)) {
                        alert("Share Percent value should be equal to 100 in Part Line " + dGetValue(i - 1) + " !");
                        return false;
                    }
                }
            }
        }
    
    debugger;    
        ObjGrid = window.document.getElementById("ContentPlaceHolder1_LabourDetailsGrid");
        if (ObjGrid == null) return;


        for (var i = 2; i < ObjGrid.rows.length; i++) {
            var objLabourChk = ObjGrid.rows[i].children[11].childNodes[1].childNodes[0].checked;
            if (objLabourChk == false) {
                //Get Lable of Labour ID         
                iLabourcnt = iLabourcnt + 1;
                var lblLabjobcode = ObjGrid.rows[i].children[7].childNodes[1];
                var lblLabTotal = ObjGrid.rows[i].children[6].childNodes[1].value;
                var lblLabCd = ObjGrid.rows[i].children[2].children[1];
                var objDrpLAddDesc = ObjGrid.rows[i].children[3].childNodes[3];

                if (objDrpLAddDesc.value == "0" && (lblLabCd.value.substr(0, 5) == "MTIMI")) {
                    alert("Fill Additional Labor for Labour line " + dGetValue(i - 1));
                    return false;
                }

                if ((lblLabTotal == "" || dGetValue(lblLabTotal) == 0) && (lblLabCd.value.substr(0, 5) == "MTIMI" || lblLabCd.value.substr(0, 5) == "MTICC")) {
                    alert("Enter Total Amount for Labour line " + dGetValue(i - 1));
                    return false;
                }


                //if (sPartType == "O" && sLubLocID == "0") {
                //    alert("Select Lubricant Location for Part line " + dGetValue(i - 1));
                //    return false;
                //}        

                if (Confirm == "Y" && lblLabjobcode.value == "0") {
                    alert("Select Jobcode for Labour line " + dGetValue(i - 1));
                    return false;
                }
                if (lblLabjobcode.value != "0") {//Confirm == "Y" &&            
                    if (bCheclValueExistInArray(ArrAllJobs, lblLabjobcode.value) == false) ArrAllJobs.push(lblLabjobcode.value);
                }
                if (ObjCombo.options[ObjCombo.selectedIndex].value == "16" && ObjOption1.checked == true) {
                    dVECVShare = dGetValue(ObjGrid.rows[i].cells[8].childNodes[1].value);
                    dDealerShare = dGetValue(ObjGrid.rows[i].cells[9].childNodes[1].value);
                    dCustShare = dGetValue(ObjGrid.rows[i].cells[10].childNodes[1].value);
                    if (parseFloat(dVECVShare + dDealerShare + dCustShare) != parseFloat(100)) {
                        alert("Percent value should be equal to 100 in Labour line " + dGetValue(i - 1) + " !");
                        return false;
                    }
                }
            }
        }
    
    //debugger;    
        ObjGrid = window.document.getElementById("ContentPlaceHolder1_LubricantDetailsGrid");
        if (ObjGrid == null) return;


        for (var i = 2; i < ObjGrid.rows.length; i++) {
            var objLubChk = ObjGrid.rows[i].children[10].childNodes[1].childNodes[0].checked;
            if (objLubChk == false) {
                iLubcnt = iLubcnt + 1;
                var lblLubjobcode = ObjGrid.rows[i].children[6].childNodes[1];

                var WarrQty = dGetValue(ObjGrid.rows[i].children[2].childNodes[1].value);
                var sLubLocID = dGetValue(ObjGrid.rows[i].children[13].childNodes[1].value);
                if (sLubLocID == "0") {
                    alert("Select Lubricant Location for Lubricant line " + dGetValue(i - 1));
                    return false;
                }
                if (WarrQty == "0" || WarrQty == "") {
                    alert("Warranty Qty should not be blank For Lubricant line " + dGetValue(i - 1));
                    return false;
                }
                if (Confirm == "Y" && lblLubjobcode.value == "0") {
                    alert("Select Jobcode for Lubricant line " + dGetValue(i - 1));
                    return false;
                }
                if (lblLubjobcode.value != "0") {//Confirm == "Y" &&            
                    if (bCheclValueExistInArray(ArrAllJobs, lblLubjobcode.value) == false) ArrAllJobs.push(lblLubjobcode.value);
                }
                if (ObjCombo.options[ObjCombo.selectedIndex].value == "16" && ObjOption1.checked == true) {
                    dVECVShare = dGetValue(ObjGrid.rows[i].cells[7].childNodes[1].value);
                    dDealerShare = dGetValue(ObjGrid.rows[i].cells[8].childNodes[1].value);
                    dCustShare = dGetValue(ObjGrid.rows[i].cells[9].childNodes[1].value);
                    if (parseFloat(dVECVShare + dDealerShare + dCustShare) != parseFloat(100)) {
                        alert("Share Percent value should be equal to 100 in lubricant line " + dGetValue(i - 1) + " !");
                        return false;
                    }
                }
            }
        }
    
    debugger;
    objtxtID = window.document.getElementById("ContentPlaceHolder1_txtID");
    if (objtxtID.value != "" && objtxtID.value != "0") {

        ObjGrid = window.document.getElementById("ContentPlaceHolder1_SubletDetailsGrid");
        if (ObjGrid == null) return;

        for (var i = 1; i < ObjGrid.rows.length; i++) {
            var objsubChk = ObjGrid.rows[i].children[10].childNodes[1].childNodes[0].checked;
            var lblsubsAmt = ObjGrid.rows[i].children[5].childNodes[1];
            if (objsubChk == false && (lblsubsAmt.value != "0" && lblsubsAmt.value != "")) {
                iSubletcnt = iSubletcnt + 1;

                var lblsubsjobcode = ObjGrid.rows[i].children[6].childNodes[1];

                var lblsubDesc = ObjGrid.rows[i].children[2].childNodes[1];

                if (lblsubDesc.value.trim() == "") {
                    alert("Enter Description for Sublet line " + dGetValue(i));
                    return false;
                }
                if (lblsubsAmt.value == "0" || lblsubsAmt.value == "") {
                    alert("Enter Amount for Sublet line " + dGetValue(i));
                    return false;
                }
                if (Confirm == "Y" && lblsubsjobcode.value == "0") {
                    alert("Select Jobcode for Sublet line " + dGetValue(i));
                    return false;
                }
                if (lblsubsjobcode.value != "0") {//Confirm == "Y" &&            
                    if (bCheclValueExistInArray(ArrAllJobs, lblsubsjobcode.value) == false) ArrAllJobs.push(lblsubsjobcode.value);
                }
                if (ObjCombo.options[ObjCombo.selectedIndex].value == "16" && ObjOption1.checked == true) {
                    dVECVShare = dGetValue(ObjGrid.rows[i].cells[7].childNodes[1].value);
                    dDealerShare = dGetValue(ObjGrid.rows[i].cells[8].childNodes[1].value);
                    dCustShare = dGetValue(ObjGrid.rows[i].cells[9].childNodes[1].value);
                    if (parseFloat(dVECVShare + dDealerShare + dCustShare) != parseFloat(100)) {
                        alert("Share Percent value should be equal to 100 in sublet detail " + i + " !");
                        return false;
                    }
                }
            }
        }
    }


    if (Confirm == "Y" && dGetValue(iPartcnt) == 0 && dGetValue(iLubcnt) == 0 && dGetValue(iSubletcnt) == 0 && dGetValue(iLabourcnt) == 0) {
        alert("Please Enter Details before Request confirmation.");
        return false;
    }    

    var arrLen = 0;
    arrLen = ArrAllJobs.length;

    for (Arrj = 1; Arrj <= arrLen; Arrj++) {
        if (iGetIndexOfValueFromArrayJob(ArrAllJobs, (Arrj)) == false) {
            alert("Please select Jobcode " + ('J' + Arrj) + " before select next jobcode.");
            return false;
        }
    }

    if (Confirm == "Y") {
        if (CheckComplaintValidation() == false) {
            return false;
        }
        if (CheckInvestigationValidation() == false) {
            return false;
        }
    }
}
// Check Warranty Validation For Confirm
function CheckWarrantyValidationForConfirm() {
    ArrAllJobes = new Array();
    if (CheckClaimValidation() == false) {
        return false;
    }
    if (CheckComplaintValidation() == false) {
        return false;
    }
    if (CheckInvestigationValidation() == false) {
        return false;
    }
    if (CheckJobCodeForPartLubSubOil() == false) {
        return false;
    }
    if (CheckJobCodeForPart() == false) {
        return false;
    }
    if (CheckJobCodeForLabour() == false) {
        return false;
    }
    if (CheckJobCodeForLubricant() == false) {
        return false;
    }
    if (CheckJobCodeForSubLet() == false) {
        return false;
    }
    if (CheckeDetailsForJob() == false) {
        return false;
    }

    return true;
}
//Sujata 19012011

// Check Warranty Validation
function CheckWarrantyValidation() {
    ArrAllJobes = new Array();
    if (CheckClaimValidation() == false) {
        return false;
    }
    //Sujata 19012011
    //    if (CheckJobCodeForPart() == false) {
    //        return false;
    //    }
    //    if (CheckJobCodeForLabour() == false) {
    //        return false;
    //    }
    //    if (CheckJobCodeForLubricant() == false) {
    //        return false;
    //    }
    //    if (CheckJobCodeForSubLet() == false) {
    //        return false;
    //    }
    //    if (CheckeDetailsForJob() == false) {
    //        return false;
    //    }
    //Sujata 19012011
    return true;
}
function validEmail(email) {
    // returns "" if valid else the error string	
    // you can add your own custom checks.

    // check for invalid character
    if (email.match(/^[\w_\-\@\.]+$/) == null)
        return ("\tEmail contains character other than alphanumeric and _ - @ .}");
    // check if the .dot is in the begining of the email string
    if (email.match(/^[\.]/) != null)
        return ("\tEmail cannot start with a dot");
    // check if the .dot is in the begining of the email string
    if (email.match(/[\.]$/) != null)
        return ("\tEmail cannot end with a dot");
    // check for initial pattern
    //if (email.match(/^[\w_\-\.]+@[\w_\.\-]+\.[a-z]{2,3}$/i) == null)		// 'i' for case insensitive
    if (email.match(/^[\w_\-\.]+@[\w_\.\-]+\.[a-z]{2,4}$/i) == null)		// 'i' for case insensitive
        return ("\tEmail is not of the correct form");
    // check if the dots are adjacent	
    if (email.match(/[\.]{2,}/) != null)
        return ("\tEmail cannot have adjacent dots");
    // check if the dot is adjacent to the @ character
    if (email.match(/[\.]+@|@[\.]+/) != null)
        return ("\tEmail cannot have dot adjacent to the @ character");

    // return blank string for valid email	
    return ("");
}


//Check Claim Validation
function CheckClaimValidation() {
    var sMessage = "";
    var ObjControl;
    debugger;
    //Claim Types
    ObjControl = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Claim Type in Vehicle Details Tab.";
    }

    //ModelCode
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtModelCode");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Model Code in Vehicle Details Tab.";
    }
    //Customer Name
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustomerName");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Customer Name in Vehicle Details Tab.";
    }
    //Customer Address
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustomerAddress");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Customer Address in Vehicle Details Tab.";
    }
    //Customer EmailAddress
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustEmail");
    if (ObjControl != null) {
        if (ObjControl.value != "" && ObjControl.readOnly == false) {
            var result = validEmail(document.getElementById("ContentPlaceHolder1_txtCustEmail").value)
            if (result != "") {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi\n\n  in Vehicle Details Tab.")
                document.getElementById("ContentPlaceHolder1_txtCustEmail").focus();
                return false;
            }
        }
    }

    //Hrs Applicable
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtHrsApplicable");
    if (ObjControl != null) {
        if (ObjControl.value == "Y") //Hrs applicable Yes
        {
            //txtHrsReading
            ObjControl = window.document.getElementById("ContentPlaceHolder1_txtHrsReading");
            if (ObjControl != null) {
                if (ObjControl.disabled == false) {
                    if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Hrs Reading in Vehicle Details Tab.";
                }

            }
        }
        else //Hrs applicable No
        {
            //txtOdometer
            ObjControl = window.document.getElementById("ContentPlaceHolder1_txtOdometer");
            if (ObjControl != null) {
                if (ObjControl.disabled == false) {
                    if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Odometer Reading in Vehicle Details Tab.";
                }
            }

        }
    }



    ////    //drpRouteType
    ////    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpRouteType");
    ////    if (ObjControl != null) {
    ////        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Route Type.";
    ////    }


    //txtVehicleNo
    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    //    if (ObjControl != null) {
    //        if (ObjControl.value == "15" ) {
    //            ObjControl = window.document.getElementById("ContentPlaceHolder1_txtVehicleNo");
    //            if (ObjControl != null) {
    //                if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Vehicle No in Vehicle Details Tab.";
    //            }        
    //        }         
    //    }


    //txtRepairOrderNo
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRepairOrderNo");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Repair Order No in Vehicle Details Tab.";
    }



    //    ObjControl = window.document.getElementById("ContentPlaceHolder1_DropClaimTypes");
    //    if (ObjControl != null) {
    //        if (ObjControl.value != "18") {
    //txtFailureDate_txtDocDate
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtFailureDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Failure Date in Vehicle Details Tab.";
    }

    //txtRepairOrderDate_txtDocDate
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRepairOrderDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Repair Order Date in Vehicle Details Tab.";
    }

    //txtRepairCompleteDate_txtDocDate
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtRepairCompleteDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Repair Complete Date in Vehicle Details Tab.";
    }

    //}
    //}
    //Remark
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDealerRemark");
    if (ObjControl != null) {
        if (ObjControl.value == "") sMessage = sMessage + "\n Please Enter The Remark in Vehicle Details Tab.";
    }






    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDealerRemark");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Payment Terms in Vehicle Details Tab.";
    }
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    //Sujata 19012011
    //if (CheckComplaintValidation() == false) {
    //   return false;
    //}
    //if (CheckInvestigationValidation() == false) {
    //    return false;
    //}
    //Sujata 19012011
    return true;
}
// Set Total Complaint Record Count
function CheckComplaintValidation() {
    var ObjGrid;
    var iRecordCnt = 0;
    var iDeleteRecordCnt = 0;
    var sTextValue = "";
    // To calculate Count
    var ObjComplaintCount = document.getElementById("ContentPlaceHolder1_lblComplaintsRecCnt");
    if (ObjComplaintCount != null) {
        iRecordCnt = dGetValue(ObjComplaintCount.innerText);
    }
    if (iRecordCnt == 0) {
        alert("Please Enter the Complaints Details");
        return false;
    }
    ObjGrid = document.getElementById("ContentPlaceHolder1_ComplaintsGrid");
    if (ObjGrid == null) return true;
    iRecordCnt = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        if (ObjGrid.rows[i].cells[1].children[0].selectedIndex != 0) {
            if (ObjGrid.rows[i].cells[1].children[0].value == "9999") {
                sTextValue = ObjGrid.rows[i].cells[1].children[1].innerText
                if (sTextValue != null && sTextValue != "") {
                    iRecordCnt = iRecordCnt + 1;
                }
            }
            else {
                iRecordCnt = iRecordCnt + 1;
            }
        }
        if (ObjGrid.rows[i].cells[2].children[0].childNodes[0].checked == true) {
            iDeleteRecordCnt = iDeleteRecordCnt + 1;
        }
    }
    if (iRecordCnt == 0) {
        alert("Please Enter the Complaints Details !");
        return false;
    }
    else if (iRecordCnt == iDeleteRecordCnt) {
        alert("Please Select/Enter atleast one Complaints");
        return false;
    }
    return true;
}

// Set Total Investigations Record Count
function CheckInvestigationValidation() {
    var ObjGrid;
    var iRecordCnt = 0;
    var iDeleteRecordCnt = 0;
    var sTextValue = "";
    // To calculate Count
    var ObjInvestigationCount = document.getElementById("ContentPlaceHolder1_lblInvestigationsRecCnt");
    if (ObjInvestigationCount != null) {
        iRecordCnt = dGetValue(ObjInvestigationCount.innerText);
    }
    if (iRecordCnt == 0) {
        alert("Please Enter the Investigations Details");
        return false;
    }
    ObjGrid = document.getElementById("ContentPlaceHolder1_InvestigationsGrid");
    if (ObjGrid == null) return true;
    iRecordCnt = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        if (ObjGrid.rows[i].cells[1].children[0].selectedIndex != 0) {
            if (ObjGrid.rows[i].cells[1].children[0].value == "9999") {
                sTextValue = ObjGrid.rows[i].cells[1].children[1].innerText
                if (sTextValue != null && sTextValue != "") {
                    iRecordCnt = iRecordCnt + 1;
                }
            }
            else {
                iRecordCnt = iRecordCnt + 1;
            }
        }
        if (ObjGrid.rows[i].cells[2].children[0].childNodes[0].checked == true) {
            iDeleteRecordCnt = iDeleteRecordCnt + 1;
        }
    }
    if (iRecordCnt == 0) {
        alert("Please Enter the Investigations Details !");
        return false;
    }
    else if (iRecordCnt == iDeleteRecordCnt) {
        alert("Please Select/Enter atleast one Investigations");
        return false;
    }
    return true;
}


// Check for Job Details Are Entered.
function CheckeDetailsForJob() {
    //alert("Hi");
    //var PcontainerName = '';
    //var ObjGrid = null;
    //PcontainerName = GetContainerName();
    //if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //ObjGrid = window.document.getElementById(PcontainerName + "JobDetailsGrid");
    //if (ObjGrid == null) return;
    //var sJobCode = "";
    //var iPartID = "";
    //var PartNo = "";
    //var objJobCodeControl = null;
    //var iEnterRecordCount = 0;
    //var sJobCode = "";
    //var ObjChkForDelete = null;
    //var sWarrantablePart = "";
    //for (i = 1; i < ObjGrid.rows.length; i++)
    // {
    //     //Check Job Code in Common Array
    //     if (i <= ArrAllJobes.length)
    //      {
    //         if (bCheclValueExistInArray(ArrAllJobes, i) == false) {
    //             alert("Job Code J" + i + " is not selected in any One of the Part,Labour,Lubricant,Sublets details!");
    //             return false;
    //         }
    //     }
    //     iPartID = dGetValue(ObjGrid.rows[i].cells[2].childNodes[0].value);
    //     sWarrantablePart = ObjGrid.rows[i].cells[2].childNodes[1].value;
    //    if (iPartID != 0) 
    //    {            
    //        iEnterRecordCount = iEnterRecordCount + 1;
    //        sJobCode = ObjGrid.rows[i].cells[1].children[0].innerText;

    //        ObjChkForDelete = ObjGrid.rows[i].cells[8].children[0].childNodes[0];
    //        if (ObjChkForDelete.checked == false)
    //        {
    //            //Check Culprit Code
    //            if (ObjGrid.rows[i].cells[5].children[0].selectedIndex == 0) 
    //            {
    //                alert("Please Select the Culprit code For Job Code J"+ i);                    
    //                return false;
    //            }

    //            //Check Defect Code
    //            if (ObjGrid.rows[i].cells[6].children[0].selectedIndex == 0) 
    //            {
    //                alert("Please Select the Defect code For Job Code J" + i);                                        
    //                return false;
    //            }
    //            if (sWarrantablePart == "N") 
    //            {
    //                //Check Technical Code
    //                if (ObjGrid.rows[i].cells[7].children[0].selectedIndex == 0) {
    //                    alert("Please Select the Technical code For Job Code J" + i);
    //                    return false;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {            
    //        //Check Job Code in Common Array
    //        if (bCheclValueExistInArray(ArrAllJobes, i) == true) 
    //        {   
    //            alert("Causal Part Not selected For The Job Code J" + i);
    //            return false;
    //        }
    //    }
    //}

    //if (iEnterRecordCount != ArrAllJobes.length) 
    //{
    //    var sMessage = "Job Codes are not match!";        
    //    sMessage = sMessage + "\n Total Job Codes Selected in Part,Labour,Lubricant,Sublets is =" + ArrAllJobes.length;
    //    sMessage = sMessage + "\n Total Job Codes Selected in Job Details is =" + iEnterRecordCount;        
    //    alert(sMessage );
    //    return false;  
    //}
}
// Check for Each SubLet Job Code is Selected.
function CheckJobCodeForSubLet() {
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    ObjGrid = window.document.getElementById(PcontainerName + "SubletDetailsGrid");
    if (ObjGrid == null) return;
    var sJobCode = "";
    var SubLetID = "";
    var objJobCodeControl = null;
    var ObjChkForDelete;
    var iCurrJobCode = 0;
    var ObjTotalControl;

    for (i = 1; i < ObjGrid.rows.length; i++) {
        //Check SubLet Details
        SubLetID = dGetValue(ObjGrid.rows[i].cells[1].childNodes[0].selectedIndex);
        if (SubLetID != 0) {
            ObjChkForDelete = ObjGrid.rows[i].cells[10].children[0].childNodes[0];
            if (ObjChkForDelete.checked == false) {

                ObjTotalControl = ObjGrid.rows[i].cells[2].children[0];
                SubLetDescID = dGetValue(ObjGrid.rows[i].cells[2].childNodes[0].selectedIndex);
                if (ObjTotalControl != null && SubLetDescID == 0) {
                    // if (dGetValue(ObjTotalControl.value) == 0) {
                    alert("Sublet Description Required at line " + i);
                    return false;
                    // }
                }

                ObjTotalControl = ObjGrid.rows[i].cells[5].children[0];
                if (ObjTotalControl != null) {
                    if (dGetValue(ObjTotalControl.value) == 0) {
                        alert("Please Enter The Sublet Amount at line " + i);
                        return false;
                    }
                }
                objJobCodeControl = ObjGrid.rows[i].cells[6].children[0];
                iCurrJobCode = objJobCodeControl.selectedIndex;
                if (iCurrJobCode == 0) {
                    alert("Please select the Job Code for the SubLet at line " + i);
                    return false;
                }
                else {
                    //Add Job Code in Common Array
                    if (bCheclValueExistInArray(ArrAllJobes, iCurrJobCode) == false) {
                        ArrAllJobes.push(iCurrJobCode);
                    }
                    // Check   Total Of The Percentage is equal to 100
                    //if (ObjGrid.rows[i].cells[13] != null) {
                    //    if (ObjGrid.rows[i].cells[13].style.display == "") {
                    //        var dVECVShare = dGetValue(ObjGrid.rows[i].cells[8].children[0].value);
                    //        var dDealerShare = dGetValue(ObjGrid.rows[i].cells[9].children[0].value);
                    //        var dCustShare = dGetValue(ObjGrid.rows[i].cells[10].children[0].value);
                    //        var dTotal = dVECVShare + dDealerShare + dCustShare;
                    //        if (dTotal != 100) {
                    //            alert("Sum of the all the share percentage  is not equal to 100 at Line '" + i + "', For The Sublet " + ObjGrid.rows[i].cells[1].children[1].value);
                    //            event.returnValue = false
                    //            return false;
                    //        }

                    //}
                    //}//End of Percentage Check
                }
            }//End of the check for Delete
        } //End of the Sublet ID
    } //End of the For Loop

    return true;
}
// Check for Each Lubricant Job Code is Selected.
function CheckJobCodeForLubricant() {
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    ObjGrid = window.document.getElementById(PcontainerName + "LubricantDetailsGrid");
    if (ObjGrid == null) return;
    var LubricantID = "";
    var dQty = 0, dRate = 0;
    var objJobCodeControl = null;
    var ObjChkForDelete;
    var iCurrJobCode = 0;
    var sDescription = "";

    for (i = 1; i < ObjGrid.rows.length; i++) {
        LubricantID = ObjGrid.rows[i].cells[1].childNodes[0].selectedIndex;
        if (LubricantID != "0" && LubricantID != undefined) {
            ObjChkForDelete = ObjGrid.rows[i].cells[10].children[0].childNodes[0];
            if (ObjChkForDelete.checked == false) {

                objJobCodeControl = ObjGrid.rows[i].cells[6].children[0];
                iCurrJobCode = objJobCodeControl.selectedIndex;
                if (iCurrJobCode == 0) {
                    alert("Please select the Job Code for the Lubricant at line " + i);
                    return false;
                }
                else {
                    //Add Job Code in Common Array
                    if (bCheclValueExistInArray(ArrAllJobes, iCurrJobCode) == false) {
                        ArrAllJobes.push(iCurrJobCode);
                    }
                    //Check Description
                    var LubType = ObjGrid.rows[i].cells[1].childNodes[0];
                    if (LubType.options[LubType.selectedIndex].text == "NEW") {
                        sDescription = ObjGrid.rows[i].cells[1].children[1];
                        if (sDescription.value == "") {
                            alert("Please Enter the Description for Lubricant at line " + i);
                            sDescription.focus();
                            return false;
                        }
                    }

                    //Check Qty
                    dQty = dGetValue(ObjGrid.rows[i].cells[2].children[0].value);
                    if (dQty == 0) {
                        alert("Please Enter the Qty for Lubricant at line " + i);
                        return false;
                    }

                    //Check Rate
                    dRate = dGetValue(ObjGrid.rows[i].cells[4].children[0].value);
                    if (dRate == 0) {
                        alert("Please Enter the Rate for Lubricant at line " + i);
                        return false;
                    }
                    // Check   Total Of The Percentage is equal to 100
                    //if (ObjGrid.rows[i].cells[13] != null) {
                    //    if (ObjGrid.rows[i].cells[13].style.display == "") {
                    //        var dVECVShare = dGetValue(ObjGrid.rows[i].cells[7].children[0].value);
                    //        var dDealerShare = dGetValue(ObjGrid.rows[i].cells[8].children[0].value);
                    //        var dCustShare = dGetValue(ObjGrid.rows[i].cells[9].children[0].value);
                    //        var dTotal = dVECVShare + dDealerShare + dCustShare;
                    //        if (dTotal != 100) {
                    //            alert("Sum of the all the share percentage  is not equal to 100 at Line '" + i + "', For The Lubricant '" + ObjGrid.rows[i].cells[2].children[1].value + "'");
                    //            event.returnValue = false
                    //            return false;

                    //        }

                    //}
                    //}//End of the Percentage Check
                } //End of the check of the job code
            } //End of the check for delete
        } //End of the lubricant id
    } //End of the For    

    return true;
}

// Check for Each Labour Job Code is Selected.
function CheckJobCodeForLabour() {
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    ObjGrid = window.document.getElementById(PcontainerName + "LabourDetailsGrid");
    if (ObjGrid == null) return;
    var LabourID = "";
    var objJobCodeControl = null;
    var ObjChkForDelete;
    var iCurrJobCode = 0;
    for (i = 1; i < ObjGrid.rows.length; i++) {
        LabourID = dGetValue(ObjGrid.rows[i].cells[1].childNodes[0].value);
        if (LabourID != 0) {
            ObjChkForDelete = ObjGrid.rows[i].cells[11].children[0].childNodes[0];
            if (ObjChkForDelete.checked == false) {
                objJobCodeControl = ObjGrid.rows[i].cells[7].children[0];
                iCurrJobCode = objJobCodeControl.selectedIndex;
                if (iCurrJobCode == 0) {
                    alert("Please select the job code for a Labour '" + ObjGrid.rows[i].cells[2].children[1].value + "' at line " + i);
                    return false;
                }
                else {
                    //Add Job Code in Common Array
                    if (bCheclValueExistInArray(ArrAllJobes, iCurrJobCode) == false) {
                        ArrAllJobes.push(iCurrJobCode);
                    }
                    // Check   Total Of The Percentage is equal to 100
                    //if (ObjGrid.rows[i].cells[13] != null) {
                    //    if (ObjGrid.rows[i].cells[13].style.display == "") {
                    //        var dVECVShare = dGetValue(ObjGrid.rows[i].cells[8].children[0].value);
                    //        var dDealerShare = dGetValue(ObjGrid.rows[i].cells[9].children[0].value);
                    //        var dCustShare = dGetValue(ObjGrid.rows[i].cells[10].children[0].value);
                    //        var dTotal = dVECVShare + dDealerShare + dCustShare;
                    //        if (dTotal != 100) {
                    //            alert("Sum of the all the share percentage  is not equal to 100 at Line '" + i + "' For The Labour '" + ObjGrid.rows[i].cells[2].children[1].value + "'");
                    //            event.returnValue = false
                    //            return false;
                    //        }
                    //}       
                    //} //End of percentage check
                } //End of the check of Chefordelete
            }
        } //End of the Labour id check
    } //End of the For    
    return true;
}
//Check For Each Job 'Causal ', 'Culprit' 'Defect' is mandatory in Parts level
function CheckJobCodeForPart() {
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    ObjGrid = window.document.getElementById(PcontainerName + "PartDetailsGrid");
    if (ObjGrid == null) return;
    var iRecordCnt = 0;
    var ObjPartCount = document.getElementById("ContentPlaceHolder1_lblPartRecCnt");
    if (ObjPartCount != null) {
        iRecordCnt = dGetValue(ObjPartCount.innerText);
    }
    //Sujata 19012011
    //    if (iRecordCnt == 0) {
    //        alert("Please Enter the Part Details");
    //        return false;
    //    }
    //Sujata 19012011
    var sTmpValue = "";
    var PartID = "";
    var ObjDefectCode = null;
    var objJobCodeControl = null;
    var ObjChkForDelete;
    //ArrOfCodes = new Array(ObjGrid.rows.length);

    var iMaxJocCode = 0;
    var iCurrJobCode = 0;
    var iCntofJobCode = 0;
    var ArrOfPartsJob = new Array();

    for (i = 1; i < ObjGrid.rows.length; i++) {
        PartID = dGetValue(ObjGrid.rows[i].cells[1].childNodes[0].value);
        if (PartID != 0) {
            ObjChkForDelete = ObjGrid.rows[i].cells[16].children[0].childNodes[0];
            if (ObjChkForDelete.checked == false) {
                objJobCodeControl = ObjGrid.rows[i].cells[12].children[0];
                iCurrJobCode = objJobCodeControl.selectedIndex;
                if (iCurrJobCode == 0) {
                    alert("Please select the job code for a part '" + ObjGrid.rows[i].cells[2].children[1].value + "' at line " + i);
                    return false;
                }
                else {
                    if (iCurrJobCode > iMaxJocCode) {
                        iMaxJocCode = iCurrJobCode;
                    }
                    //Check Job Code Exist in Array of the Part Job
                    if (bCheclValueExistInArray(ArrOfPartsJob, iCurrJobCode) == false) {
                        ArrOfPartsJob.push(iCurrJobCode);
                        iCntofJobCode = iCntofJobCode + 1;
                    }

                    //Add Job Code in Common Array
                    if (bCheclValueExistInArray(ArrAllJobes, iCurrJobCode) == false) {
                        ArrAllJobes.push(iCurrJobCode);
                    }
                    // Check   Total Of The Percentage is equal to 100
                    //if (ObjGrid.rows[i].cells[13] != null) {
                    //    if (ObjGrid.rows[i].cells[13].style.display == "") {
                    //        var dVECVShare = dGetValue(ObjGrid.rows[i].cells[13].children[0].value);
                    //        var dDealerShare = dGetValue(ObjGrid.rows[i].cells[14].children[0].value);
                    //        var dCustShare = dGetValue(ObjGrid.rows[i].cells[15].children[0].value);
                    //        var dTotal = dVECVShare + dDealerShare + dCustShare;
                    //        if (dTotal != 100) {

                    //            alert("Sum of the all the share percentage  is not equal to 100 at Line '" + i + "' For The Part '" + ObjGrid.rows[i].cells[2].children[1].value + "'");
                    //            //objControl.value = 0;
                    //            event.returnValue = false
                    //            return false;
                    //        }
                    //}
                    //}
                }
            }
        }
    }
    // Check sequentially Job Codes are entered
    //    if (iCntofJobCode != iMaxJocCode) {
    //        //alert("Part details are not entered for the Job codes wich are come before the Job code J" + iMaxJocCode)
    //        CheckJobDetailsAreEntered(ArrOfPartsJob, "Part");
    //        return false;
    //    }
    return true;
}

//check Job Details Are Enter For Part,Labour,Lubricant,Sublet
function CheckJobDetailsAreEntered(ArrWithJobCodes, sMaterialName) {
    var sFinalMessage = "";
    for (var iArrIndex = 0; iArrIndex < ArrWithJobCodes.length; iArrIndex++) {
        if (bCheclValueExistInArray(ArrWithJobCodes, (iArrIndex + 1)) == false) {
            sFinalMessage = sFinalMessage + "J" + (iArrIndex + 1) + ",";
        }
    }
    if (sFinalMessage != "") {
        sFinalMessage = sFinalMessage.substring(0, sFinalMessage.lastIndexOf(','));
        alert(sMaterialName + " Details Are not entered for the Job Codes  " + sFinalMessage + "!");
        return false;
    }
    return true;
}


//check before Cancel
function CheckForCancel() {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    var Control = document.getElementById(PcontainerName + 'txtID');
    if (Control == null) return;
    if (Control.value == "") {
        alert("Please Select The Record For Cancel!");
        return false;
    }
    if (confirm("Are you sure, you want to cancel the document?") == true)
        return true;
    else
        return false;
}

// Function is used to Display record when user click on new button
function OpenFormOnNewClick(FormId) {
    if (FormId == 9999) //For Dynamic (Template Master) Page
    {
        ClearFormControlsValue();
        return false;
    }
    if (FormId == 0) {
        return;
    }
    //Megha24082011
    if (FormId == 51 || FormId == 55 || FormId == 56 || FormId == 222 || FormId == 104 || FormId == 106) //FPDA & Indent consolidation & Depovise Indent Details Report
    {
        return;

    }

    var w = 2000;
    var h = 300;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    HideControl();
    var DocID;
    var PcontainerName = '';
    var ObjControl;
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    feature = "dialogWidth:1000px;dialogHeight:400px;status:no;help:no;scrollbars:no;resizable:no;";
    var Parameters = null;
    var iDealerId = 0;
    var ObjDealer = document.getElementById("ContentPlaceHolder1_Location_txtAllDealerID");
    if (ObjDealer == null) return false;
    iDealerId = ObjDealer.value;
    //1 Open Form to Select RFP(Model) To Create Proforma,same way for vehicle model 3-ORF,5-Preshipment,7-Packing List,9-Bill of ladding,11-Challan,13-Indent,15-Postshipment,17-Packing Slip From Postshipment,19--Challan From Postsipment
    //2 Open Form to Select RFP(Part) To Create Proforma ,same way for parts 4-ORF,6-Preshipment,8-Packing List,10-Bill of ladding,12-Challan,14-Indent,16-Postshipment,18-Packing Slip From Postshipment,20--Challan From Postsipment
    //13 Indent
    //41 Vehicle Plant Details
    debugger;
    if (FormId == "1" || FormId == "3" || FormId == "5" || FormId == "7" || FormId == "9" || FormId == "13" || FormId == "15" || FormId == "17" || FormId == "19" || FormId == "21" || FormId == "23" || FormId == "27" || FormId == "41" ) {//|| FormId == "11"
        Parameters = "FormID=" + FormId + "&DealerId=" + iDealerId;
        DocID = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectDocOnNew.aspx?" + Parameters, null, feature);
    }
    else if (FormId == "4" || FormId == "6" || FormId == "8" || FormId == "10" || FormId == "12" || FormId == "14" || FormId == "16" || FormId == "18" || FormId == "20" || FormId == "22" || FormId == "24" || FormId == "42") {
        Parameters = "Model_Part=P&FormID=" + FormId + "&DealerId=" + iDealerId;
        DocID = window.showModalDialog("/AUTODMS/Forms/Common/frmSelectDocOnNew.aspx?" + Parameters, null, feature);
    }
    else {
        DocID = 0;
    }
    if (DocID == undefined) {
        return false;
    }
    ObjControl = document.getElementById(PcontainerName + 'txtPreviousDocId');
    ObjControl.value = DocID;
}
// Function is used to return value which is selected by user
function ReturnValue(objImgControl) {
    var objRow = objImgControl.parentNode.parentNode.childNodes;
    var ReturnID = 0;
    ReturnID = objRow[2].innerText;
    window.returnValue = ReturnID;
    window.close();
}

function ShowReport(FormId, ReportPath) {

    //window.showModalDialog("/AUTODMS/Forms/Common/frmSelectDocOnNew.aspx?" + Parameters, null, feature);
    var iDocId = 0;
    var iUserRoleId = 0;
    var sExportYesNo = "";
    var sFPDAYesNo = "";
    var RptTitle = "";
    var msgAnswer = "";
    var iregionID = 0;
    var idepoID = 0;
    var iCurrDate = "";
    var imenuid = 0;



    //var Control = document.getElementById(PcontainerName + 'txtID');
    var Control = document.getElementById('ContentPlaceHolder1_txtID');

    if (Control == null) {
        var Control = document.getElementById('txtID');

    }

    if (FormId == "51") {
        var Control = document.getElementById('ContentPlaceHolder1_txtID');
        //Megha24012012
        var UserRole = document.getElementById('ContentPlaceHolder1_txtUserRoleID');
        iUserRoleId = UserRole.value;
        //Megha24012012
    }
    if (FormId == "55") {
        var Control = document.getElementById('ContentPlaceHolder1_txtID');
        var CurrDate = document.getElementById('ContentPlaceHolder1_txtcurrdate');
        iCurrDate = CurrDate.value;
        var regionID = document.getElementById('ContentPlaceHolder1_txtRegionId');
        iregionID = regionID.value
        var depoID = document.getElementById('ContentPlaceHolder1_txtDepoIDs');
        idepoID = depoID.value
        var ProdCat = document.getElementById('ContentPlaceHolder1_txtProdCat');
        iProdCat = ProdCat.value
        var menuid = document.getElementById('ContentPlaceHolder1_txtModelId');
        imenuid = menuid.value

    }
    if (FormId == "56") {
        var Control = document.getElementById('ContentPlaceHolder1_txtID');
        var ProdCat = document.getElementById('ContentPlaceHolder1_txtProdCat');
        iProdCat = ProdCat.value
    }
    if (Control == null) return;
    if (Control.value == "") {
        alert("Please Select The Record For Print!");
        return false;
    }
    else {
        iDocId = Control.value;
    }
    //var Url = "/AUTODMS/Forms/Common/frmDocumentView.aspx?";
    var Url = ReportPath;

    var sReportName = "";
    sExportYesNo = "Y";
    if (FormId == "65") //Vehicle order form 
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hidengst');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptOrderbillingprinting&";  //+ strReportName + "&";
        else
            sReportName = "/rptOrderbillingGSTprinting&";  //+ strReportName + "&";
    }
    else if (FormId == "67") //Jobcard  
    {
        sReportName = "/rptJobcardprintingNew&";  //+ strReportName + "&";
    }
    else if (FormId == "11") //Estimat  
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptEstimateprt&";  //+ strReportName + "&";
        else
            sReportName = "/RptEstimatePrtGST&";
    }
    else if (FormId == "40") //Warranty claim
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptWarrClaimprinting&";  //+ strReportName + "&";
        else
            sReportName = "/RptWarrantyInvoiceGST&";
    }
    else if (FormId == "68") //Warranty claim
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdntaxtype');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/RptM8Doc&";  //+ strReportName + "&";
        else
            sReportName = "/RptM8DocGST&";
    }
    else if (FormId == "25") //Couponclaim
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptCouponclaim&";  //+ strReportName + "&";
        else
            sReportName = "/rptCouponInvoiceGST&";
    }
    else if (FormId == "26") //Vehicle purchas
    {
        sReportName = "/rptVehiclePOPrinting&";  //+ strReportName + "&";
    }
    else if (FormId == "27") //quotation
    {

        sReportName = "/rptQuotationprinting&";  //+ strReportName + "&";
    }
    else if (FormId == "222") // SparesPO
    {
        sReportName = "/rptSparespoPrinting&";  //+ strReportName + "&";
    } //Sujata 24012013
    else if (FormId == "103") //SparesOA
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptSparesOA&";  //+ strReportName + "&";
        else
            sReportName = "/rptSparesOAGST&";
    }
    else if (FormId == "104") //Preshipment Spares
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptSparesmaterialreeipt&";  //+ strReportName + "&";
        else
            sReportName = "/rptSparesMaterialReceiptGST&";
    }
    else if (FormId == "202") //Preshipment pklist Vehicle
    {
        //sReportName = "/RptDealerInvoce&";  //+ strReportName + "&";
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hidegst');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/RptDealerInvoce&"; //+ strReportName + "&";
        else
            sReportName = "/RptDealerInvoiceGST&";
    }
    else if (FormId == "301") //Preshipment pklist Spares
    {
        sReportName = "/rptSpStockChallanPrint&";  //+ strReportName + "&";
    } //Sujata 24012013
    else if (FormId == "13") //Vehicle ORF Indent 
    {
        sReportName = "/rptORFIndent&";  //+ strReportName + "&";
    }
    else if (FormId == "14") //Spares ORF Indent 
    {
        sReportName = "/rptORFIndentSpares&";  //+ strReportName + "&";
    } //Sujata 24012013
    else if (FormId == "15") //Postshipment Vehicle
    {
        sReportName = "/rptPostShipmentVInvoice&";  //+ strReportName + "&";
    }
    else if (FormId == "16") //Postshipment Spares
    {
        sReportName = "/rptPostShipmentSInvoice&";  //+ strReportName + "&";
    }
    else if (FormId == "17") //Postshipment Pklist Vehicle
    {
        sReportName = "/rptPostShipmentVPackingList&";  //+ strReportName + "&";
    }
    else if (FormId == "18") //Postshipment Pklist Spares
    {
        sReportName = "/rptPostShipmentSPackingList&";  //+ strReportName + "&";
    }
        //else if (FormId == "11" || FormId == "19") //Delivery Challan
        //{
        //    sReportName = "/rptDeliveryChallan&";  //+ strReportName + "&";
        //}

    else if (FormId == "12") //Spares Preshipment Delivery Challan
    {
        sReportName = "/rptSPPreDelvChallan&";  //+ strReportName + "&";
    }
    else if (FormId == "19") //Vehicle Postshipment Delivery Challan
    {
        sReportName = "/rptVehPostShipmentDelvChallan&";  //+ strReportName + "&";
    }
    else if (FormId == "20") //Spares PostShipment Delivery Challan
    {
        sReportName = "/rptSPPostShipmentDelvChallan&";  //+ strReportName + "&";
    }
        //else if (FormId == "40") //Warranty Claim
        //{
        //    var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnISDocGST');
        //    var igstFlag = objhdnDocGST.value;
        //    if (igstFlag == 'N')
        //        sReportName = "/rptWarrClaimReport&";  //+ strReportName + "&";
        //    else
        //        sReportName = "/RptWarrantyInvoiceGST&";
        //}
    else if (FormId == "52") //Bank Statement
    {
        sReportName = "/rptBankStatement&";  //+ strReportName + "&";
    }
    else if (FormId == "99") //Service History on Chassis Master
    {

        sReportName = "/rptServiceHistory&";  //+ strReportName + "&";
        //sExportYesNo = "";
        sExportYesNo = "N"
    }

    else if (FormId == "102") //EGP Spare Invoice
    {

        var objhdnJobHDRID = document.getElementById('ContentPlaceHolder1_hdnJobHDRID');
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
        var ijobID = objhdnJobHDRID.value;
        var igstFlag = objhdnDocGST.value;
        if (ijobID == "0" || ijobID == "") {
            if (igstFlag == 'N') {
                sReportName = "/RptSparesInvoice&";  //+ strReportName + "&";  
            }
            else {
                sReportName = "/RptSparesInvoiceGST&";
            }
        }
        else {
            if (igstFlag == 'N') {
                sReportName = "/RptJbSparesInvoice&";  //+ strReportName + "&";        
                //sExportYesNo = "";
            }
            else {
                sReportName = "/RptJbSparesInvoiceGST&";  //+ strReportName + "&";  
            }
        }

        sExportYesNo = "Y"
    }
        //else if (FormId == "103") //EGP Spare Invoice
        //{
        //    sReportName = "/RptEGPSpareOA&";  //+ strReportName + "&";
        //    //sExportYesNo = "";
        //    sExportYesNo = "N"
        ////}
        // else if (FormId == "104") //EGP Spare Invoice
        //{
        //    sReportName = "/RptEGPSpareMaterialReceipt&";  //+ strReportName + "&";
        //    //sExportYesNo = "";
        //    sExportYesNo = "N"
        //}
    else if (FormId == "105") //EGP Sales Return
    {
        //sReportName = "/RptEGPsalesreturn&";  //+ strReportName + "&";
        ////sExportYesNo = "";
        //sExportYesNo = "N"
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'N')
            sReportName = "/rptSRN&"; //+ strReportName + "&";
        else
            sReportName = "/rptSRNGST&";
    }
    else if (FormId == "106") // Purchase Return
    {
        var objhdnDocGST = document.getElementById('ContentPlaceHolder1_hdnIsDocGST');
        var igstFlag = objhdnDocGST.value;
        if (igstFlag == 'Y')
            sReportName = "/rptPRNGST&";  //+ strReportName + "&";
        //sExportYesNo = "";
        //sExportYesNo = "N"
    }
    else if (FormId == "55") //Indent Cosolidation & Monthly Supply Plan Report
    {
        //if (confirm("Are you sure, you want to Print the Indent Consolidation Report?") == true) {
        if (confirm("Are you sure, you want to Print the Report?") == true) {
            sReportName = "/RptIndentConsolidation&";  //+ strReportName + "&";
            //  sFPDAYesNo = null;
            sExportYesNo = "I"

        }
        else {
            return false;
        }

        if (sReportName == "") {
            return false;
        }
    }
    else if (FormId == "56") //Indent Monthly Supply Plan
    {
        if (confirm("Are you sure, you want to Print the Depot Indent Report?") == true) {

            sReportName = "/RptDepotIndent&";  //+ strReportName + "&";
            //sExportYesNo = "";
            // sFPDAYesNo = null;
            sExportYesNo = "D"

        }
        else {
            return false;
        }

        if (sReportName == "") {
            return false;
        }
    }
        //Megha24082011
    else if (FormId == "51") //FPDA Details
    {
        //Megha24012012
        // if (confirm("Please Click 'OK' to FPDA Report Print,'Cancel' to Scrap Report Print") == true) {

        //var objhdnJobHDRID = document.getElementById('ContentPlaceHolder1_txtID');
        sReportName = "/RptFPDADetails&";  //+ strReportName + "&";
        sExportYesNo = "Y"
        //sFPDAYesNo = "Y";
        //sExportYesNo = "F"



    }        //Megha24082011
    
    else if (FormId == "240") //Activity Claim Details
    {
        debugger;
        sReportName = "/RptActivityWarrantyInvoiceGST&";  //+ strReportName + "&";
        sExportYesNo = "Y";
    }


    if (sReportName == "") {
        return false;
    }
    if (FormId == "51") //FPDA Details
    {
        //Megha24012012
        Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=" + sExportYesNo + "";
        //Megha24012012
    }

    else if (FormId == "55") //Indent Cosolidation
    {

        Url = Url + sReportName + "ID=" + iDocId + "&RegionId=" + iregionID + "&DepoId=" + idepoID + "&CurrDate=" + iCurrDate + "&ProdCat=" + iProdCat + "&MenuID=" + imenuid + "&ExportYesNo=" + sExportYesNo + "";

    }
    else if (FormId == "56") //Indent supply Plan
    {

        Url = Url + sReportName + "ID=" + iDocId + "&ProdCat=" + iProdCat + "&ExportYesNo=" + sExportYesNo + "";

    }
    else {
        Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=" + sExportYesNo + "";
    }

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

// To Check Vehicle RFP Vlaidation
function ValidateFPDA() {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";

    var Control = document.getElementById(PcontainerName + 'txtID');
    var ObjGrid = null;
    var ObjScrapPart = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    ObjScrapPart = window.document.getElementById("ContentPlaceHolder1_gvScrapPart");
    if (ObjGrid == null && ObjScrapPart == null) {
        alert('Please Select Warranty Claim for FPDA Details');
        return false;
    }
    else if (ObjGrid == null && ObjScrapPart != null && Control.value == "0") {
        alert('No Despatchable Parts in the selection, FPDA can not be created.');
        return false;
        //    var sMsg = "No Despatchable Parts in the selection, FPDA can not be created. \n Only Scrap Register will be generated.\n Are you sure, you want to continue?";
        //    if (confirm(sMsg) == false) {
        //        return false
        //    }
    }
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    if (Control.value == "0") {
        //var sMsg = "Can't change 'Credit Date from' and 'Credit Date To' Once you Save record.\n Are you sure, you want to continue?";
        var sMsg = "Can't change 'Claim Date from' and 'Claim Date To' Once you Save record.\n Are you sure, you want to continue?";
        if (confirm(sMsg) == false) {
            return false
        }
    }

}
function SelectATleastOnePart() {
    var checkExist;
    var frm = document.forms[0];
    checkExist = false;
    for (i = 0; i < frm.elements.length; i++) {

        if ((frm.elements[i].type == "checkbox") && (frm.elements[i].id != "ContentPlaceHolder1_DetailsGrid_ctl01_ChkAcceptAll")) {
            if (frm.elements[i].checked == true)
                checkExist = true;
            //= document.getElementById(id).checked;
        }

    }
    if (checkExist == false) {
        alert("Plz Select Atleast One Part")
        return false;
    }

}
//Check For Confirmation FPDA
function CheckFPDA() {
    var txtTranporter = null;
    txtTranporter = window.document.getElementById("ContentPlaceHolder1_txtTransporter");
    if (txtTranporter.value == "") {
        alert("Please Enter the Transporter Details!");
        return false;
    }
    var txtLRNo = null;
    txtLRNo = window.document.getElementById("ContentPlaceHolder1_txtLRNo");
    if (txtLRNo.value == "") {
        alert("Please Enter the LR No Details!");
        return false;
    }
    //     var txtNoOfCases = null;
    //     txtNoOfCases = window.document.getElementById("ContentPlaceHolder1_txtNoOfCases");
    //     if (txtNoOfCases.value == "" || dGetValue(txtNoOfCases.value) == 0)      
    //     {    
    //        alert("Please Enter the No Of Boxes!");
    //        return false ;
    //     }
    var txtxtDocDate = null;
    txtxtDocDate = window.document.getElementById("ContentPlaceHolder1_txtLRDate_txtDocDate");
    if (txtxtDocDate.value == "") {
        alert("Please Enter the LR Date Details!");
        return false;
    }
    //Sujata 04122012_Begin    
    var ObjGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    if (ObjGrid == null) return true;

    var txtNoOfCases = null;
    txtNoOfCases = window.document.getElementById("ContentPlaceHolder1_txtNoOfCases");
    if (txtNoOfCases.value == "" || dGetValue(txtNoOfCases.value) == 0) {
        alert("Please Enter the No Of Boxes!");
        return false;
    }
    //debugger;
    for (var i = 1; i <= ObjGrid.rows.length - 1; i++) {
        var objtxtQtyControl = ObjGrid.rows[i].cells[6].childNodes[1];
        var Qty = dGetValue(objtxtQtyControl.value)
        if ((Qty == 0 || objtxtQtyControl == null) && (ObjGrid.rows[i].cells[8].childNodes[1].checked == true)) {
            alert("Please Enter the Qty For All Failed Part Detail!");
            return false;
        }
        //      Check Expected Exp. Head       
        objtxtControl = ObjGrid.rows[i].cells[7].childNodes[1];
        TotDetailsBox = dGetValue(objtxtControl.value)
        if ((TotDetailsBox == 0 || objtxtControl == null) && (ObjGrid.rows[i].cells[8].childNodes[1].checked == true)) {
            alert("Please Enter the Box No For All Failed Part Detail!");
            return false;
        }

        objtxtControlRmk = ObjGrid.rows[i].cells[9].childNodes[1];
        txtRmkTxt = objtxtControlRmk.value
        if ((txtRmkTxt == "" || objtxtControlRmk == null) && (ObjGrid.rows[i].cells[8].childNodes[1].checked == false)) {
            alert("Please Enter the Reject Remark For All Rejected Failed Part Detail!");
            return false;
        }
    }
    //Sujata 04122012_End

    return true;
}
//alert("Please Enter the Doc Date Details!") ;

function CheckStockingNorms() {
    var PcontainerName = '';
    PcontainerName = GetContainerName();
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";

    var Control = document.getElementById(PcontainerName + 'txtID');
    var ObjGrid = null;
    var ObjScrapPart = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_gvErrorPart");
    if (ObjGrid != null) {

        alert("Can't Confirm : Please remove error part's by upload data again!");
        return false;
    }
}


function ValidatePartSchemeValue() {
    txtFromLevel1 = window.document.getElementById("ContentPlaceHolder1_txtFromLevel1");
    txtToLevel1 = window.document.getElementById("ContentPlaceHolder1_txtToLevel1");
    txtFromLevel2 = window.document.getElementById("ContentPlaceHolder1_txtFromLevel2");
    txtToLevel2 = window.document.getElementById("ContentPlaceHolder1_txtToLevel2");
    txtDiscountLevel1 = window.document.getElementById("ContentPlaceHolder1_txtDiscountLevel1");
    txtDiscountLevel2 = window.document.getElementById("ContentPlaceHolder1_txtDiscountLevel2");
    txtDiscountLevel3 = window.document.getElementById("ContentPlaceHolder1_txtDiscountLevel3");
    txtFromLevel3 = window.document.getElementById("ContentPlaceHolder1_txtFromLevel3");
    txtToLevel3 = window.document.getElementById("ContentPlaceHolder1_txtToLevel3");


    if (eval(txtFromLevel1.value) >= eval(txtToLevel1.value)) {
        txtFromLevel1.focus();
        alert('From Level1 should be Less than To Level1');
        return false;
    }
    else if (eval(txtToLevel1.value) >= eval(txtFromLevel2.value)) {
        txtToLevel1.focus();
        alert('To Level1 should be Less than From Level2');
        return false;
    }
    else if (eval(txtFromLevel2.value) >= eval(txtToLevel2.value)) {
        txtFromLevel2.focus();
        alert('From Level2 should be Less than To Level2');
        return false;
    }
    else if (eval(txtToLevel2.value) >= eval(txtFromLevel3.value)) {
        txtToLevel2.focus();
        alert('To Level2 should be Less than From Level3');
        return false;
    }
    else if (eval(txtFromLevel3.value) >= eval(txtToLevel3.value)) {
        txtFromLevel3.focus();
        alert('From Level3 should be Less than To Level3 ');
        return false;
    }

    else if (eval(txtDiscountLevel1.value) >= eval(txtDiscountLevel2.value)) {
        txtDiscountLevel1.focus();
        alert('Discount Level1 should be Less than Discount Level2');
        return false;
    }
    else if (eval(txtDiscountLevel2.value) >= eval(txtDiscountLevel3.value)) {
        txtDiscountLevel2.focus();
        alert('Discount Level2 should be Less than Discount Level3');
        return false;
    }

}

//Sujata 19012011
function CheckJobCodeForPartLubSubOil() {
    var PcontainerName = '';
    var ObjGrid = null;
    PcontainerName = GetContainerName();

    var iRecordCnt = 0;
    var ObjPartCount = document.getElementById("ContentPlaceHolder1_lblPartRecCnt");
    if (ObjPartCount != null) {
        iRecordCnt = dGetValue(ObjPartCount.innerText);
    }
    if (iRecordCnt == 0) {
        // check for Labour lblLabourRecCnt
        ObjPartCount = document.getElementById("ContentPlaceHolder1_lblLabourRecCnt");
        if (ObjPartCount != null) {
            iRecordCnt = dGetValue(ObjPartCount.innerText);
            if (iRecordCnt == 0) {
                //Check For Oil lblLubricantRecCnt            
                ObjPartCount = document.getElementById("ContentPlaceHolder1_lblLubricantRecCnt");
                if (ObjPartCount != null) {
                    iRecordCnt = dGetValue(ObjPartCount.innerText);
                    if (iRecordCnt == 0) {
                        //Check For Sublet lblSubletRecCnt                 
                        ObjPartCount = document.getElementById("ContentPlaceHolder1_lblSubletRecCnt");
                        if (ObjPartCount != null) {
                            iRecordCnt = dGetValue(ObjPartCount.innerText);
                            if (iRecordCnt == 0) {
                                alert("Please select the part/ Labour/ Lubricant/ Sublet ");
                                return false;
                            }
                        }
                    }
                }
            }
        }
    }
    return true;
}
//Sujata 19012011
//Megha 27092013
function CheckExportWarrantyPolicyValidation() {

    var sMessage = "";
    var ObjControl;

    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpCountry");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Country.";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpFertCode");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Model Code.";
    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtExportdays");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter The Export Warranty Policy!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtExportdays");
    if (ObjControl != null) {
        if (ObjControl.value == 0 && ObjControl.value.trim().length != 0) sMessage = sMessage + "\n Please Enter The Export Warranty Policy Days greater than 0!";
    }

    txtSAPdays = window.document.getElementById("ContentPlaceHolder1_txtSAPWardays");
    txtExportdays = window.document.getElementById("ContentPlaceHolder1_txtExportdays");
    if (eval(txtExportdays.value) > eval(txtSAPdays.value)) {
        sMessage = sMessage + "\n Export Warranty Policy Days should be Less than or Equal to SAP Warranty Policy Days!";
    }

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}

function CheckEGPDealerCreationValidation() {

    var sMessage = "";
    var ObjControl;

    //     ObjControl = window.document.getElementById("ContentPlaceHolder1_drpRegion");
    //     if (ObjControl != null) {
    //         if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Region.";
    //         
    //     }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpState");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The State.";

    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtEGPDealerName");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter EGP Dealer Name!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtEGPDealerShortName");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter EGP Dealer Short Name!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtAddress1");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Address 1!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtAddress2");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Address 2!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtDealerMobile");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Dealer Mobile!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtLandLinePhone");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter LandLine Phone!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCity");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter City!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtEmail");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Email!";

    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtEmail");
    if (ObjControl != null) {
        if (ObjControl.value != "") {
            var result = validEmail(document.getElementById("ContentPlaceHolder1_txtEmail").value)
            if (result != "") {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi")
                document.getElementById("ContentPlaceHolder1_txtEmail").focus();
                return false;
            }
        }
    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtMDEmail");
    if (ObjControl != null) {
        if (ObjControl.value != "") {
            var result = validEmail(document.getElementById("ContentPlaceHolder1_txtMDEmail").value)
            if (result != "") {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi")
                document.getElementById("ContentPlaceHolder1_txtMDEmail").focus();
                return false;
            }
        }
    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpStartYear");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Year.";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpActive");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Active.";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtPANNo");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter PANNo !";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtTINNo");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter TINNo !";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtcst");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter C.S.T !";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtST");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter S.T./VAT !";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtpincode");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Pincode !";

    }

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}

function CheckCustomerCreationValidation() {
    var sMessage = "";
    var iRowCount = 0;
    var iRowCountGlobal = 0;
    var iID = 0;
    var ObjControl;
    var ObjControl;
    var ObjControl_Flag;
    ObjControl1 = window.document.getElementById("ContentPlaceHolder1_drpCustType");
    if (ObjControl1 != null) {
        if (ObjControl1.selectedIndex == 0) sMessage = sMessage + "\n Please Select Customer Type.";

    }

    //     ObjControl = window.document.getElementById("ContentPlaceHolder1_drpRegion");
    //     if (ObjControl != null) {
    //         if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Region.";
    //         
    //     }
    if (ObjControl1.selectedIndex == 6 || ObjControl1.selectedIndex == 4) {
        ObjControl = window.document.getElementById("ContentPlaceHolder1_drpDealerName");
        if (ObjControl != null) {
            if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select Customer.";

        }

    }
    else {
        ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCustomerName");
        if (ObjControl != null) {
            if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Customer Name!";

        }
    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpState");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The State.";

    }




    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtAddress1");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Address 1!";

    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtCity");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter City!";

    }
    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtMobile");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) sMessage = sMessage + "\n Please Enter Mobile!";

    }



    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtEmail");
    if (ObjControl != null) {
        if (ObjControl.value != "") {
            var result = validEmail(document.getElementById("ContentPlaceHolder1_txtEmail").value)
            if (result != "") {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi")
                document.getElementById("ContentPlaceHolder1_txtEmail").focus();
                return false;
            }
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpActive");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) sMessage = sMessage + "\n Please Select The Active.";

    }
    //ObjControl_DetailsGrid = window.document.getElementById("ContentPlaceHolder1_DetailsGrid");
    //ObjControl_hdnRowCount = window.document.getElementById("ContentPlaceHolder1_hdnRowCount");
    //ObjControl_ID = window.document.getElementById("ContentPlaceHolder1_txtID");
    //ObjControl_Flag = window.document.getElementById("ContentPlaceHolder1_hdnFlag")
    //ObjControl_hdnRowCountGlobal = window.document.getElementById("ContentPlaceHolder1_hdnRowCountGlobal");

    // if (ObjControl_DetailsGrid != null)
    // {
    //     iRowCount = dGetValue(ObjControl_hdnRowCount.value);
    //     iRowCountGlobal = dGetValue(ObjControl_hdnRowCountGlobal.value);
    //    iID = dGetValue(ObjControl_ID.value);
    //    if (ObjControl_DetailsGrid.rows.length > 1 && iID == 0 && iRowCountGlobal>0)
    //     {

    //         if (confirm("Same customer already Present in Global Customer Master.you want to link that Customer to your Customer?") == true) 
    //        {

    //            ObjControl_Flag.value = "N";

    //        }
    //        else 
    //        {
    //            ObjControl_Flag.value = "N";
    //        }
    //    }
    //   else if (ObjControl_DetailsGrid.rows.length > 1 && iID == 0 && iRowCount > 0) {

    //       alert("Same customer already Present");
    //       return false;
    //    }
    //    else if (iID != 0)
    //    {
    //        ObjControl_Flag.value = "N";
    //    }
    //    else
    //    {
    //       ObjControl_Flag.value = "Y";
    //    }
    // }
    // else 
    //{
    //    ObjControl_Flag.value = "Y";
    //}



    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}


// To Check Parts OA Vlaidation
function CheckOAValidation() {
    //debugger;
    var ObjControl;

    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpCustomer");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select The Customer!.");
            return false;
        }
    }

    var ObjGrid = null;
    var objGridID = $("#ContentPlaceHolder1_PartGrid")//window.document.getElementById("ContentPlaceHolder1_PartGrid");
    var ObjGrid = objGridID[0];

    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;

    var iPartAdd = 0;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[15].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            iPartAdd = iPartAdd + 1;
        }
    }

    if (ObjGrid.rows.length == 2 || iPartAdd == 0) {
        alert("Please Select The Part!.");
        return false;
    }
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        // Check Part ID
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[15].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            // Check Quantity is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty == 0) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
            }
            objtxtControl = ObjGrid.rows[i].cells[10].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed at line " + i;
            }
            var sGridPartTax = ObjGrid.rows[i].cells[14].children[0].value.trim();
            //alert(sGridPartTax);
            if (isNaN(sGridPartTax) == true) sGridPartTax = 0;
            //sGridPartTax = ObjGrid.rows[i].cells[14].children[0].selectedIndex;//ObjGrid.rows[i].childNodes[12].children[0].selectedIndex;
            if (sGridPartTax == 0) {
                sMessage = sMessage + " \n Please Enter the Part Tax at line " + i;
            }
        }
    }

    //Discount not allowed more than equal to 100
    ObjGrid = null;
    var objGridID = $("#ContentPlaceHolder1_GrdPartGroup")//window.document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    ObjGrid = objGridID[0];
    //debugger;
    if (ObjGrid == null) {
        if (sMessage != "") {
            alert(sMessage);
            return false;
        }
        return;
    }

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        var dAmt = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
        sGroupCode = ObjControl.value;
        if (sGroupCode.trim() != "" && dAmt > 0) {
            // Check Discount is enter
            objtxtControl = ObjGrid.rows[i].cells[4].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed in group tax details at line " + i;
            }
            // Check Discount Amount is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= dAmt) {
                sMessage = sMessage + " \n Please Enter Discount Amount Less than Taxable Inv Amt at Line " + i;
            }
        }
    }
    var objGridDocTax = null;
    var objGridDocTaxID = $("#ContentPlaceHolder1_GrdDocTaxDet"); //window.document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    objGridDocTax = objGridDocTaxID[0];
    iQty = dGetValue(objGridDocTax.rows[1].cells[9].children[0].value); // dGetValue(objGridDocTax.rows[1].childNodes[9].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n PF Charges Percentage should be less than 100....!";
    }

    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value); // dGetValue(objGridDocTax.rows[1].childNodes[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n Other Charges Percentage should be less than 100....!";
    }
    //Sujata 18062014 Begin Discount not allowed more than equal to 100
    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    // Commented by Shyamal on 02062012,Already cover in code behind
    //    iCountDel = iCountDel + 1;
    //    if (iCountDel == ObjGrid.rows.length) {
    //        alert("Please keep at least one record at details!");
    //        return false;
    //    }
    return true;
}

// To Check EGP Spares Invoice Vlaidation
function CheckDiscountApp() {
    var ObjControl;
    var sMessage = "";


    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtAppDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Enter Approval Date!.");

    }


    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}

function CheckLossApp() {
    var ObjControl;
    var sMessage = "";


    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtAppDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Enter Approval Date!.");

    }


    //alert("Hi");



    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}


function CheckDCSPOApp() {
    var ObjControl;
    var sMessage = "";


    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtPOApppDate_txtDocDate");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Enter Approval Date!.");

    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtChangeQty");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Approve Qty!.");
    }


    //alert("Hi");



    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}

//vrushali25032015_Start
function CheckLeadTransaction() {
    var ObjControl;
    var sMessage = "";


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpInqSource");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Lead Source!.");
            return false;
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpLeadName");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Lead Name!.");
            return false;
        }
    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtSourceName");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Enter Source Name!.");
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtSourceAdd");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Enter Source Address!.");

    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_txtSourceMob");
    if (ObjControl != null) {
        if (ObjControl.value.trim().length == 0) alert("Please Enter Source Phone!.");

    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpArea");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Area!.");
            return false;
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpAttendedby");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Attended By!.");
            return false;
        }
    }



    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpAlloatedTo");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Allocated To!.");
            return false;
        }
    }


    //ObjControl = window.document.getElementById("ContentPlaceHolder1_drpModelCat");
    //if (ObjControl != null) {
    //    if (ObjControl.selectedIndex == 0) {
    //        alert("Please Select Interested Purchase Category!.");
    //        return false;
    //    }
    //}


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpLoadType");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Load Type!.");
            return false;
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpPrimaryApplication");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Primary Application!.");
            return false;
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpSeconadryApplication");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Secondary Type!.");
            return false;
        }
    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpRoadType");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Secondary Type!.");
            return false;
        }
    }





    if (sMessage != "") {
        alert(sMessage);
        return false;
    }

    return true;

}


function CheckLeadMaster() {
    var ObjControl;
    var ObjControlTmp;




    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpCustType");

    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Lead Type!.");
            return false;
        }


    }


    ObjControl = window.document.getElementById("ContentPlaceHolder1_drporgType");
    ObjControlTmp = window.document.getElementById("ContentPlaceHolder1_drpCustType");

    if (ObjControlTmp != null) {
        if (ObjControlTmp.selectedIndex == 2) {

            if (ObjControl != null) {
                if (ObjControl.selectedIndex == 0) {

                    alert("Please Select Organisational SubCategory!.");
                    return false;
                }
            }
        }
    }



    ObjControl = window.document.getElementById("ContentPlaceHolder1_drpcustSubType");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select Lead Type!.");
            return false;
        }
    }



    //     ObjControl = window.document.getElementById("ContentPlaceHolder1_txtSourceMob");
    //     if (ObjControl != null) {
    //         if (ObjControl.value.trim().length == 0) alert("Please Enter Source Phone!.");

    //     }






    //     if (sMessage != "") {
    //         alert(sMessage);
    //         return false;
    //     }

    return true;

}

//vrushali25032015_End

function CheckOAInvValidation() {
    //debugger;
    var ObjControl;
    var ObjInvType;
    var InvTypeID = 0;
    // Inv Type
    ObjInvType = window.document.getElementById("ContentPlaceHolder1_DrpInvType");
    if (ObjInvType != null) {
        if (ObjInvType.selectedIndex == 0) {
            alert("Please Select Invoice Type");
            return false;
        }
        InvTypeID = ObjInvType.options[ObjInvType.selectedIndex].value;
    }

    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpCustomer");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0 && (InvTypeID < 3 || InvTypeID == 9)) {
            alert("Please Select The Customer!.");
            return false;
        }
        else if (ObjControl.selectedIndex == 0 && (InvTypeID > 2 || InvTypeID < 9)) {
            alert("Please Select The JobCard!.");
            return false;
        }
    }


    var ObjGrid = null;
    var objGridID = $("#ContentPlaceHolder1_PartGrid")//document.getElementById("ContentPlaceHolder1_PartGrid");
    var ObjGrid = objGridID[0];
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iPartAdd = 0;
    var jPartID = 0;
    var dTotalInvQty = 0;
    var sBFRGST = "";
    var dCurrSTK = 0;
    var dBFRGSTSTK = 0;
    var dTotalCurrStk = 0;
    var dTotalBFRGSTStk = 0;
    var iSrNo = 0;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            iPartID = dGetValue(ObjControl.value);
            bPartSel = ObjGrid.rows[i].cells[19].children[0].children[0].checked; //ObjGrid.rows[i].childNodes[15].children[0].children[0].checked;
            if (iPartID != 0 && bPartSel == false) {
                iPartAdd = iPartAdd + 1;
            }
        }
    }
    //if ((InvTypeID > 2 && ObjGrid.rows.length == 1) || (InvTypeID < 3 && (ObjGrid.rows.length == 2 || iPartAdd == 0))) {
    if (((InvTypeID > 2 || InvTypeID < 9) && (ObjGrid.rows.length == 1 || iPartAdd == 0)) || ((InvTypeID < 3 || InvTypeID == 9) && (ObjGrid.rows.length == 2 || iPartAdd == 0))) {
        alert("Please Select The Part!.");
        return false;
    }
    // Validation For Part Details
    for (var i = 1; i < ObjGrid.rows.length; i++) {

        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            sBFRGST = ObjGrid.rows[i].cells[27].children[0];
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            var bPartSel = ObjGrid.rows[i].cells[19].children[0].children[0].checked; //ObjGrid.rows[i].childNodes[15].children[0].children[0].checked;
            // Check Part ID
            iPartID = dGetValue(ObjControl.value);
            if (iPartID != 0 && bPartSel == false) {
                //Check Inv Quantity
                iSrNo = ObjGrid.rows[i].cells[0].innerText;
                objtxtControl = ObjGrid.rows[i].cells[8].children[0];
                iQty = dGetValue(objtxtControl.value);
                // Check Qty When Parts Invoice Type only 1 & 2 
                if (iQty == 0 && (InvTypeID == 1 || InvTypeID == 2 || InvTypeID == 9)) {
                    sMessage = sMessage + " \n Please Enter the Quantity at line " + iSrNo;
                }
                //Check 100 % discount should not allowed for Counter sale and only 100 to Jobcard Invoice and not more than 100.
                objtxtControl = ObjGrid.rows[i].cells[13].children[0];
                iQty = dGetValue(objtxtControl.value);
                if (iQty >= 100 && (InvTypeID < 3 || InvTypeID == 9)) {
                    sMessage = sMessage + " \n 100% and above discount not allowed at line " + iSrNo;
                }
                else if (iQty > 100 && (InvTypeID > 2 || InvTypeID < 9)) {
                    sMessage = sMessage + " \n 100% and above discount not allowed at line " + iSrNo;
                }
                // Check Tax is Applied
                var sGridPartTax = ObjGrid.rows[i].cells[17].children[0].value.trim();
                //alert(sGridPartTax);
                if (isNaN(sGridPartTax) == true) sGridPartTax = 0;
                //var sGridPartTax = ObjGrid.rows[i].cells[16].children[0].selectedIndex; //ObjGrid.rows[i].childNodes[14].children[0].selectedIndex;
                if (sGridPartTax == 0) {
                    sMessage = sMessage + " \n Please Enter the Part Tax at line " + iSrNo;
                }
            }
        }
    }
    //New Part Stock Quantity Validation

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        iSrNo = ObjGrid.rows[i].cells[0].innerText;
        sBFRGST = ObjGrid.rows[i].cells[27].children[0];// BFRGST Rate Flag
        ObjControl = ObjGrid.rows[i].cells[1].children[0]; // PartID
        var bPartSel = ObjGrid.rows[i].cells[19].children[0].children[0].checked;
        dCurrSTK = dGetValue(ObjGrid.rows[i].cells[9].children[0].value);
        dBFRGSTSTK = dGetValue(ObjGrid.rows[i].cells[28].children[0].value);
        iPartID = dGetValue(ObjControl.value);
        var iCount = 0;

        if (iPartID != 0 && bPartSel == false) {
            var totPrevQty = 0;
            var totInvQty = 0;
            for (var j = 1; j < ObjGrid.rows.length; j++) {
                var ObjControl1 = ObjGrid.rows[j].cells[1].children[0];
                var bPartSel1 = ObjGrid.rows[j].cells[19].children[0].children[0].checked;
                jPartID = dGetValue(ObjControl1.value);
                if (jPartID != 0 && jPartID == iPartID && bPartSel1 == false) {
                    var jInvQty = dGetValue(ObjGrid.rows[j].cells[8].children[0].value);
                    var jPrevQty = dGetValue(ObjGrid.rows[j].cells[8].children[1].value);
                    totPrevQty = totPrevQty + jPrevQty;
                    totInvQty = totInvQty + jInvQty;
                    iCount = iCount + 1;
                }  //END IF
            } // END For
        } // END IF

        if (iPartID != 0 && sBFRGST.value == "N" && totInvQty > (dCurrSTK + totPrevQty) && (InvTypeID < 3 || InvTypeID == 9) && bPartSel == false) {
            sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Part Stock at Row No " + iSrNo;
        }
        if (iPartID != 0 && sBFRGST.value == "Y" && totInvQty > (dBFRGSTSTK + totPrevQty) && (InvTypeID < 3 || InvTypeID == 9) && bPartSel == false) {
            sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Pre GST Part Stock at Row No " + iSrNo;
        }

        //Check Stock==PrevQty && TotalInv>Stock (Curr Stock)
        //if (sBFRGST.value == "N" && totPrevQty == dCurrSTK && totInvQty > dCurrSTK && (InvTypeID < 3 || InvTypeID == 9) && bPartSel == false) {
        //    sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Part Stock at Row No " + iSrNo;
        //}
        //// OLD Stock
        //if (sBFRGST.value == "Y" && totPrevQty == dBFRGSTSTK && totInvQty > dBFRGSTSTK && (InvTypeID < 3 || InvTypeID == 9) && bPartSel == false) {
        //    sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Pre GST Part Stock at Row No " + iSrNo;
        //}
        //// New Stock 
        //if (sBFRGST.value == "N" && totPrevQty != dCurrSTK && (totInvQty) > (dCurrSTK + totPrevQty) && (InvTypeID < 3 || InvTypeID == 9) && bPartSel == false) {
        //    sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Part Stock at Row No " + iSrNo;
        //}
        ////OLD Stock
        //if (sBFRGST.value == "Y" && totPrevQty != dBFRGSTSTK && (totInvQty) > (dBFRGSTSTK + totPrevQty) && (InvTypeID < 3 || InvTypeID == 9) && bPartSel == false) {
        //    sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Pre GST Part Stock at Row No " + iSrNo;
        //}
        //New Code Dated on 1/11/2017
        //New Stock
        //if (sBFRGST.value == "N"  && totPrevQty != dCurrSTK && (totInvQty + totPrevQty) > (dCurrSTK + totPrevQty) && InvTypeID < 3 && bPartSel == false && iCount > 1) {
        //if (sBFRGST.value == "N" && totPrevQty != totInvQty && totPrevQty != dCurrSTK && (totInvQty + totPrevQty) > (dCurrSTK + totPrevQty) && InvTypeID < 3 && bPartSel == false && iCount > 1) {


        //if (sBFRGST.value == "N" && dTotInvAndPrevQty > (dCurrSTK + totPrevQty) && InvTypeID < 3 && bPartSel == false && iCount > 1) {
        //if (sBFRGST.value == "N" && totInvQty > (dCurrSTK + totPrevQty) && InvTypeID < 3 && bPartSel == false && iCount > 1) {
        //    sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Part Stock at Row No " + iSrNo;
        //}
        ////OLD Stock
        ////if (sBFRGST.value == "Y" && totPrevQty != dBFRGSTSTK && (totInvQty + totPrevQty) > (dBFRGSTSTK + totPrevQty) && InvTypeID < 3 && bPartSel == false && iCount > 1) {
        //if (sBFRGST.value == "Y" && totInvQty > (dBFRGSTSTK + totPrevQty) && InvTypeID < 3 && bPartSel == false && iCount > 1) {
        //    sMessage = sMessage + " \n Please Enter less  Invoice Quantity from Pre GST Part Stock at Row No " + iSrNo;
        //}


    } // END For

    //Begin Discount not allowed more than equal to 100    
    ObjGrid = null;
    var GrdPartGroupID = $("#ContentPlaceHolder1_GrdPartGroup")////window.document.getElementById("ContentPlaceHolder1_GrdPartGroup");
    ObjGrid = GrdPartGroupID[0];
    if (ObjGrid == null) return;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
            ObjControl = ObjGrid.rows[i].cells[1].children[0];
            var dAmt = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
            sGroupCode = ObjControl.value;
            if (sGroupCode.trim() != "" && dAmt > 0) {
                // Check Discount is enter
                objtxtControl = ObjGrid.rows[i].cells[4].children[0];
                iQty = dGetValue(objtxtControl.value);
                if (iQty >= 100) {
                    sMessage = sMessage + " \n 100% and above discount not allowed in group tax details at line " + i;
                }
                // Check Discount Amount is enter
                objtxtControl = ObjGrid.rows[i].cells[5].children[0];
                iQty = dGetValue(objtxtControl.value);
                if ((iQty >= dAmt && (InvTypeID < 3 || InvTypeID == 9)) || (iQty > dAmt && (InvTypeID > 2 || InvTypeID < 9))) {
                    sMessage = sMessage + " \n Enter Discount Amount Less than Taxable Inv Amt at Line " + i;
                }
            }
        }
    }

    var Total = 0;
    var objGridDocTax = null;

    var GrdDocTaxDetID = $("#ContentPlaceHolder1_GrdDocTaxDet")////window.document.getElementById("ContentPlaceHolder1_GrdDocTaxDet");
    objGridDocTax = GrdDocTaxDetID[0];

    Total = dGetValue(objGridDocTax.rows[1].cells[13].children[0].value); //objGridDocTax.rows[1].childNodes[13].children[0].value;

    iQty = dGetValue(objGridDocTax.rows[1].cells[9].children[0].value); //dGetValue(objGridDocTax.rows[1].childNodes[9].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n PF Charges Percentage should be less than 100....!";
    }

    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value); //dGetValue(objGridDocTax.rows[1].childNodes[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n Other Charges Percentage should be less than 100....!";
    }
    //Sujata 18062014 Begin Discount not allowed more than equal to 100
    //ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpCustomerType");
    //if (ObjControl != null) {
    //    if (ObjControl[ObjControl.selectedIndex].innerText.trim() == "Counter sale Cash" && Total > 2000) {
    //        alert("Cash Sale invoice amount should be less than 2000....!");
    //        return false;
    //    }
    //}

    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    // Commented by Shyamal on 02062012,Already cover in code behind
    //    iCountDel = iCountDel + 1;
    //    if (iCountDel == ObjGrid.rows.length) {
    //        alert("Please keep at least one record at details!");
    //        return false;
    //    }
    return true;
}

function CheckSRNValidation() {
    //debugger;
    var ObjControl;

    ObjControl = window.document.getElementById("ContentPlaceHolder1_DrpCustomer");
    if (ObjControl != null) {
        if (ObjControl.selectedIndex == 0) {
            alert("Please Select The Customer!.");
            return false;
        }
    }


    var ObjGrid = null;
    var objGridID = $("#ContentPlaceHolder1_PartGrid")
    var ObjGrid = objGridID[0];
    if (ObjGrid == null) return;
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var sMessage = "";
    var iPartID = 0;
    var iQty = 0;
    var iPartAdd = 0;

    for (var i = 1; i < ObjGrid.rows.length; i++) {
        //if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
        //Part ID
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        iPartID = dGetValue(ObjControl.value);
        bPartSel = ObjGrid.rows[i].cells[17].children[0].children[0].checked;
        if (iPartID != 0 && bPartSel == false) {
            iPartAdd = iPartAdd + 1;
            //  }
        }
    }
    if (ObjGrid.rows.length == 2 || iPartAdd == 0) {
        alert("Please Select The Part!.");
        return false;
    }
    // Validation For Part Details
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        //if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        var bPartSel = ObjGrid.rows[i].cells[17].children[0].children[0].checked; //ObjGrid.rows[i].childNodes[15].children[0].children[0].checked;
        // Check Part ID
        iPartID = dGetValue(ObjControl.value);
        if (iPartID != 0 && bPartSel == false) {
            //Check Inv Quantity
            objtxtControl = ObjGrid.rows[i].cells[7].children[0];
            iQty = dGetValue(objtxtControl.value);
            // Check Qty When Parts Invoice Type only 1 & 2 
            if (iQty == 0) {
                sMessage = sMessage + " \n Please Enter the Quantity at line " + i;
            }
            //Check 100 % discount should not allowed.
            objtxtControl = ObjGrid.rows[i].cells[12].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed at line " + i;
            }
            // Check Tax is Applied
            var sGridPartTax = ObjGrid.rows[i].cells[16].children[0].selectedIndex; //ObjGrid.rows[i].childNodes[14].children[0].selectedIndex;
            if (sGridPartTax == 0) {
                sMessage = sMessage + " \n Please Enter the Part Tax at line " + i;
            }

        }
        // }
    }
    //Begin Discount not allowed more than equal to 100    
    ObjGrid = null;
    var GrdPartGroupID = $("#ContentPlaceHolder1_GrdPartGroup")
    ObjGrid = GrdPartGroupID[0];
    if (ObjGrid == null) return;
    for (var i = 1; i < ObjGrid.rows.length; i++) {
        //if (ObjGrid.rows[i].className == 'GridViewRowStyle' || ObjGrid.rows[i].className == 'GridViewAlternatingRowStyle') {
        ObjControl = ObjGrid.rows[i].cells[1].children[0];
        var dAmt = dGetValue(ObjGrid.rows[i].cells[3].children[0].value);
        sGroupCode = ObjControl.value;
        if (sGroupCode.trim() != "" && dAmt > 0) {
            // Check Discount is enter
            objtxtControl = ObjGrid.rows[i].cells[4].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= 100) {
                sMessage = sMessage + " \n 100% and above discount not allowed in group tax details at line " + i;
            }
            // Check Discount Amount is enter
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            iQty = dGetValue(objtxtControl.value);
            if (iQty >= dAmt) {
                sMessage = sMessage + " \n Enter Discount Amount Less than Taxable Inv Amt at Line " + i;
            }
        }
        // }
    }

    var Total = 0;
    var objGridDocTax = null;

    var GrdDocTaxDetID = $("#ContentPlaceHolder1_GrdDocTaxDet")
    objGridDocTax = GrdDocTaxDetID[0];

    Total = dGetValue(objGridDocTax.rows[1].cells[13].children[0].value);

    iQty = dGetValue(objGridDocTax.rows[1].cells[9].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n PF Charges Percentage should be less than 100....!";
    }

    iQty = dGetValue(objGridDocTax.rows[1].cells[11].children[0].value);
    if (iQty > 100) {
        sMessage = sMessage + "\n Other Charges Percentage should be less than 100....!";
    }


    if (sMessage != "") {
        alert(sMessage);
        return false;
    }
    // Commented by Shyamal on 02062012,Already cover in code behind
    //    iCountDel = iCountDel + 1;
    //    if (iCountDel == ObjGrid.rows.length) {
    //        alert("Please keep at least one record at details!");
    //        return false;
    //    }
    return true;
}