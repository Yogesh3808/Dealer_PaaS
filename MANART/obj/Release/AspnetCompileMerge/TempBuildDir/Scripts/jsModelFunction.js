function SetControlsReadOnly(bValue) {
    var objtxtControl;
    objtxtControl = document.getElementById("txtEngineNo");
    objtxtControl.readOnly = bValue;

    objtxtControl = document.getElementById("txtInvoiceNo");
    objtxtControl.readOnly = bValue;

    objtxtControl = document.getElementById("txtInvoiceDate_txtDocDate");
    objtxtControl.readOnly = bValue;
    objtxtControl = document.getElementById("txtInstallationDate_txtDocDate");
    objtxtControl.readOnly = bValue;
    objtxtControl = document.getElementById("txtCustomerName");
    objtxtControl.readOnly = bValue;

    objtxtControl = document.getElementById("txtCustomerAddress");
    objtxtControl.readOnly = bValue;
}
// To Get Selected Labour Details
function ReturnModelChassisDetails() 
{
    var ReturnValue = new Array();
    var ObjControl;
    
    
    
    ObjControl = window.document.getElementById("txtModelID");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[0] = ObjControl.value;

    ObjControl = window.document.getElementById("txtModelDescription");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[1] = ObjControl.value;    

    ObjControl = window.document.getElementById("txtGVW");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[2] = ObjControl.value;

    ObjControl = window.document.getElementById("txtNewChassisNo");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[3] = ObjControl.value;

    ObjControl = window.document.getElementById("txtEngineNo");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[4] = ObjControl.value;

    ObjControl = window.document.getElementById("txtInvoiceNo");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[5] = ObjControl.value;

    ObjControl = window.document.getElementById("txtInvoiceDate_txtDocDate");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[6] = ObjControl.value;


    ObjControl = window.document.getElementById("txtInstallationDate_txtDocDate");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[7] = ObjControl.value;

    ObjControl = window.document.getElementById("txtCustomerName");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[8] = ObjControl.value;

    ObjControl = window.document.getElementById("txtCustomerAddress");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[9] = ObjControl.value;

    ObjControl = window.document.getElementById("txtModelGroupID");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[10] = ObjControl.value;

    ObjControl = window.document.getElementById("txtModelCode");
    if (bCheckForBlank(ObjControl) == false) return false;
    ReturnValue[11] = ObjControl.value;

    ObjControl = window.document.getElementById("txtLastRepairDate");
    ReturnValue[12] = ObjControl.value;

    ObjControl = window.document.getElementById("txtHrsApplicable");
    ReturnValue[13] = ObjControl.value;

    ObjControl = window.document.getElementById("txtLastMeterReading");
    ReturnValue[14] = ObjControl.value;

    ObjControl = window.document.getElementById("txtVehicle_No");
    ReturnValue[15] = ObjControl.value;

     //Get Last Repair Order No 
    ObjControl = window.document.getElementById("txtRepair_Order_No");
    ReturnValue[16] = ObjControl.value;

    ObjControl = window.document.getElementById("txtRootTypeID");
    ReturnValue[17] = ObjControl.value;

    ReturnValue[18] = true;
    
    //Sujata 20012011
    ObjControl = window.document.getElementById("txtchassisID");
    ReturnValue[19] = ObjControl.value;
    //Sujata 20012011
    
    window.returnValue = ReturnValue;
    window.close();
}


function DisplayChassisComboOnCancel() {
    var ObjControl;

    //Get chassis Combo
    ObjControl = document.getElementById("drpChassisNo");
    ObjControl.style.display = '';

    // Get New Chassis Textbox
    ObjControl = document.getElementById("txtNewChassisNo");
    ObjControl.style.display = 'none';

    //Get lblMessage 
    ObjControl = document.getElementById("lblavail");
    ObjControl.style.display = 'none';
}
function OnChassisValueChange(ObjCombo) {
    var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].text;
    var objtxtControl;
    if (sSelecedValue == "NEW") {
        ObjCombo.style.display = 'none';
        objtxtControl = document.getElementById("txtNewChassisNo");
        objtxtControl.style.display = '';
        SetControlsReadOnly('');
    }
    else {
        ObjCombo.style.display = '';
        objtxtControl = document.getElementById("txtNewChassisNo");
        objtxtControl.style.display = 'none';
        objtxtControl.value = sSelecedValue;
        SetControlsReadOnly(true);
    }
}
function DisplayChassisComboOnCancel() {
    var ObjControl;

    //Get chassis Combo
    ObjControl = document.getElementById("drpChassisNo");
    ObjControl.style.display = '';
    ObjControl.selectedIndex = 0;

    // Get New Chassis Textbox
    ObjControl = document.getElementById("txtNewChassisNo");
    ObjControl.style.display = 'none';

    //Get lblMessage 
    ObjControl = document.getElementById("lblavail");
    ObjControl.style.display = 'none';
    ObjControl.outerText = '';


}