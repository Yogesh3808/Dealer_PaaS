        //Function For Popoup Form FOr Select Chassis Details
         function ShowChassis()
              {  
              var ChassisDetailsValues;
              var DelaerID = document.getElementById('ContentPlaceHolder1_txtDealerId').value             
                 ChassisDetailsValues=window.showModalDialog("frmSelectChassisDtlsForInstCer.aspx?DealerId=" +DelaerID,"List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");                
                 if (ChassisDetailsValues != null) {
                    SetChassisDetails(ChassisDetailsValues);
                }
                
                return true;
            }
            //Function For Set Chassis Details
            function SetChassisDetails(ChassisDetailsValue)
            {
                var txtChassisNo=window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
                var txtEngineNo=window.document.getElementById('ContentPlaceHolder1_txtEngineNO');
                var txtModel=window.document.getElementById('ContentPlaceHolder1_txtModel');
                var txtRefNo=window.document.getElementById('ContentPlaceHolder1_txtRefNo');
                var txtRefDate=window.document.getElementById('ContentPlaceHolder1_txtRefDate');
                var txtVelInDtlID = window.document.getElementById('ContentPlaceHolder1_txtVelInDtlID');
                var hdnRepairCompleteDate = window.document.getElementById('ContentPlaceHolder1_hdnRepairCompleteDate');
                txtVelInDtlID.value=ChassisDetailsValue[0];
                txtChassisNo.value=ChassisDetailsValue[1];
                txtEngineNo.value=ChassisDetailsValue[2];
                txtModel.value=ChassisDetailsValue[3];
                txtRefNo.value=ChassisDetailsValue[4];
                txtRefDate.value = ChassisDetailsValue[5];
                hdnRepairCompleteDate.value = ChassisDetailsValue[6];
            }
            
            //Function For return Chassis Details
              function ReturnChassisValue(objImgControl)
               {
                    var objRow = objImgControl.parentNode.parentNode.childNodes;
                    var sValue;
                    var ArrOfChassis = new Array();
                    for(var cnt=2;cnt<objRow.length;cnt++)
                    { 
                    sValue=objRow[cnt].innerText;
                    ArrOfChassis.push(sValue);
                    }
                    window.returnValue = ArrOfChassis;
                    window.close();
              }
    
    //Function For Validate Instalation Certificate
    
    function ValidateInstallationCer()
    {
     var txtINSNo=document.getElementById("ContentPlaceHolder1_txtINSNo");
     var txtINSDate=document.getElementById("ContentPlaceHolder1_txtINSDate_txtDocDate");
     var txtCustName=document.getElementById("ContentPlaceHolder1_txtCustName");
     var txtChassisNo=window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
     var txtINSDate=document.getElementById("ContentPlaceHolder1_txtINSDate_txtDocDate");
     var txtVehicleNo=document.getElementById("ContentPlaceHolder1_txtVehicleNo");
     var drpCustType=document.getElementById("ContentPlaceHolder1_drpCustType");
     var drpIndustryType=document.getElementById("ContentPlaceHolder1_drpIndustryType");
     var drpDriveType=document.getElementById("ContentPlaceHolder1_drpDriveType");
     var drpLoadType=document.getElementById("ContentPlaceHolder1_drpLoadType");
     var drpPrimaryApplication=document.getElementById("ContentPlaceHolder1_drpPrimaryApplication");
     var drpSeconadryApplication=document.getElementById("ContentPlaceHolder1_drpSeconadryApplication");
     var drpRoadType=document.getElementById("ContentPlaceHolder1_drpRoadType");
     var drpRouteType=document.getElementById("ContentPlaceHolder1_drpRouteType");
     var drpFinancierType = document.getElementById("ContentPlaceHolder1_drpFinancierType");
     //Sujata 05022011
     var txtPhNo = document.getElementById("ContentPlaceHolder1_txtPhNo");
     var txtEmail = document.getElementById("ContentPlaceHolder1_txtEmail");
     //Sujata 05022011
     var txtCity = document.getElementById("ContentPlaceHolder1_txtCity");
     var txtAddress = document.getElementById("ContentPlaceHolder1_txtAddress");  
        
      if(txtINSNo.value == '')
    {
     alert("Please Enter Installation No.!");
     txtINSNo.focus();
        return false;
    }
      if(txtINSDate.value == '')
    {
     alert("Please Enter Installation Date!");
     txtINSDate.focus();
        return false;
    }
    
     if(txtCustName.value == '')
    {
     alert("Please Enter Customer Name!");
     txtCustName.focus();
        return false;
    }
    if (txtAddress.value == '') {
        alert("Please Enter Customer Address!");
        txtAddress.focus();
        return false;
    }
    if (txtCity.value == '') {
        alert("Please Enter Customer City!");
        txtCity.focus();
        return false;
    }    
    //Sujata 05022011
    if (txtPhNo.value == '') {
        alert("Please Enter Customer Phone!");
        txtPhNo.focus();
        return false;
    }
    // mantory Email from  Export INS
    if (txtEmail.value == '') {
        alert("Please Enter Customer Email Id!");
        txtEmail.focus();
        return false;
    }
    //Sujata 05022011
    if(txtINSDate.value == '')
    {
     alert("Please Enter Installation Date !");
     txtINSDate.focus();
        return false;
    }
    if(txtVehicleNo.value == '')
    {
     alert("Please Enter Vehicle No !");
     txtVehicleNo.focus();
        return false;
    }
    if(txtChassisNo.value == '')
    {
     alert("Please Select Vehicle Chassis Details (Click ON 'New Chassis/Engine')!");
     txtChassisNo.focus();
        return false;
    }
    
    if(drpCustType.value == "0")
    {
     alert("Please Select Customer Type!");
     drpCustType.focus();
        return false;
    }
    if(drpIndustryType.value == "0")
    {
     alert("Please Select Industry Type!");
     drpIndustryType.focus();
        return false;
    }
    if(drpDriveType.value == "0")
    {
     alert("Please Select Drive Type!");
     drpDriveType.focus();
        return false;
    }
    if(drpLoadType.value == "0")
    {
     alert("Please Select Load Type!");
     drpLoadType.focus();
        return false;
    }
    if(drpPrimaryApplication.value == "0")
    {
     alert("Please Select Primary Application Type!");
     drpPrimaryApplication.focus();
        return false;
    }
    if(drpSeconadryApplication.value == "0")
    {
     alert("Please Select Secondary Application Type!");
     drpSeconadryApplication.focus();
        return false;
    }
    if(drpRoadType.value == "0")
    {
     alert("Please Select Road Type!");
     drpRoadType.focus();
        return false;
    }
    if(drpRouteType.value == "0")
    {
     alert("Please Select Route Type!");
     drpRouteType.focus();
        return false;
    }
    if(drpFinancierType.value == "0")
    {
     alert("Please Select Financier Type!");
     drpFinancierType.focus();
        return false;
    }

    if (txtEmail.value != '') {

        var result = validEmail(document.getElementById("ContentPlaceHolder1_txtEmail").value)
        if (result != "") {
            alert("Email: Please enter a valid e-mail address, such as abc@def.ghi\n\n")
            document.getElementById("ContentPlaceHolder1_txtEmail").focus();
            return false;
        }
    }
        return true;
    }
    
    function validEmail(email)
{
	// returns "" if valid else the error string	
	// you can add your own custom checks.
	
	// check for invalid character
	if (email.match(/^[\w_\-\@\.]+$/) == null)
		return("\tEmail contains character other than alphanumeric and _ - @ .}");
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
    
    //Close Installation Certificate  Child Form
    function INSClose()
    {
    window.close();
    }
    
    
      function ValidateVehicleIN()
    {
     var txtReceiptNo=document.getElementById("ContentPlaceHolder1_txtReceiptNo");
     var VehicleInRDate=document.getElementById("ContentPlaceHolder1_VehicleInRDate_txtDocDate");
     var drpInvoiceNo=document.getElementById("ContentPlaceHolder1_drpInvoiceNo");
      
      if(txtReceiptNo.value == '')
    {
     alert("Please Enter VehicleIN Reciept No.!");
     txtReceiptNo.focus();
        return false;
    }
     if(VehicleInRDate.value == '')
    {
     alert("Please Enter VehicleIN  Certificate !");
     VehicleInRDate.focus();
        return false;
    }
      if(drpInvoiceNo.value == "0")
    {
     alert("Please Select ETB Invoice No!");
     drpInvoiceNo.focus();
        return false;
    }
    return true;
   }
   
   
   
   function ValidateINSProcess()
    {
   
     var drpCustType=document.getElementById("ContentPlaceHolder1_drpCustType");
     var drpIndustryType=document.getElementById("ContentPlaceHolder1_drpIndustryType");
     var drpDriveType=document.getElementById("ContentPlaceHolder1_drpDriveType");
     var drpLoadType=document.getElementById("ContentPlaceHolder1_drpLoadType");
     var drpPrimaryApplication=document.getElementById("ContentPlaceHolder1_drpPrimaryApplication");
     var drpSeconadryApplication=document.getElementById("ContentPlaceHolder1_drpSeconadryApplication");
     var drpRoadType=document.getElementById("ContentPlaceHolder1_drpRoadType");
     var drpRouteType=document.getElementById("ContentPlaceHolder1_drpRouteType");
     var drpFinancierType=document.getElementById("ContentPlaceHolder1_drpFinancierType");
     //Sujata 28022011
     var drpDepo = document.getElementById("ContentPlaceHolder1_drpDepo");
     var txtCustCode = document.getElementById("ContentPlaceHolder1_txtCustCode");
     var drpBillType = document.getElementById("ContentPlaceHolder1_drpBillType");
     var drpPlant = document.getElementById("ContentPlaceHolder1_drpPlant");
     var txtDANO = document.getElementById("ContentPlaceHolder1_txtDANO");
     //Sujata 14052011
     var txtEnquiryNo = document.getElementById("ContentPlaceHolder1_txtEnquiryNo");
     var strpos =txtDANO.value
     var pos = strpos.indexOf("I");
     if (pos != -1 && txtEnquiryNo.value !="") {
         //Sujata 14052011
         if (txtCustCode != null) {
             if (txtCustCode.value == "") {
                 alert("Please Select Customer Code!");
                 txtCustCode.focus();
                 return false;
             }
         }
         //Sujata 14052011
         }
         //Sujata 14052011
         
         if (drpBillType.value == "0") {
             alert("Please Select Bill Type!");
             drpBillType.focus();
             return false;
         }
         if (drpBillType.value == "D" && drpDepo.value == 0) {
             alert("Please Select Depot!");
             drpDepo.focus();
             return false;
         }
         if (drpBillType.value == "P" && drpPlant.value == "0") {
             alert("Please Select Model Plant!");
             drpPlant.focus();
             return false;
         }
         //Sujata 28022011
         //Sujata 14052011
         if (pos != -1 && txtEnquiryNo.value != "") {
         //Sujata 14052011
         if (drpCustType.value == "0") {
             alert("Please Select Customer Type!");
             drpCustType.focus();
             return false;
         }
         if (drpIndustryType.value == "0") {
             alert("Please Select Industry Type!");
             drpIndustryType.focus();
             return false;
         }
         if (drpDriveType.value == "0") {
             alert("Please Select Drive Type!");
             drpDriveType.focus();
             return false;
         }
         if (drpLoadType.value == "0") {
             alert("Please Select Load Type!");
             drpLoadType.focus();
             return false;
         }
         if (drpPrimaryApplication.value == "0") {
             alert("Please Select Primary Application Type!");
             drpPrimaryApplication.focus();
             return false;
         }
         if (drpSeconadryApplication.value == "0") {
             alert("Please Select Secondary Application Type!");
             drpSeconadryApplication.focus();
             return false;
         }
         if (drpRoadType.value == "0") {
             alert("Please Select Road Type!");
             drpRoadType.focus();
             return false;
         }
         if (drpRouteType.value == "0") {
             alert("Please Select Route Type!");
             drpRouteType.focus();
             return false;
         }
         if (drpFinancierType.value == "0") {
             alert("Please Select Financier Type!");
             drpFinancierType.focus();
             return false;
         }
     //Sujata 14052011
     }
     //Sujata 14052011
        return true;
    }

    //Sujata 11012011
    // check ETB Inv date should be less than Receipt date
    function CheckETBINVDateWithReceiptDate() {        
        var ObjControl;
        var objETBINVDate;
        var sETBINVDate = '';
        var sReceiptDate = '';
        
        // Get ETB Invoice Date
        objETBINVDate = document.getElementById("ContentPlaceHolder1_txtInvDate");

        if (objETBINVDate != null) {
            sETBINVDate = objETBINVDate.value;
        }
        if (sETBINVDate == "") return;        

        // Get Vehicle In Date
        ObjControl = document.getElementById("ContentPlaceHolder1_VehicleInRDate_txtDocDate");
                     
        if (ObjControl != null) {
            sReceiptDate = ObjControl.value;
        }
        if (sReceiptDate == "") return;

        if (bCheckFirstDateIsGreaterThanSecondDate(sReceiptDate, sETBINVDate) == false) {
            alert('Receipt Date Should be greater than ETB Inv Date !');
            ObjControl.value = '';
            ObjControl.focus();
            return false;
        }
    }

    
    // check Receipt date should be less than or equal to INS date
    function CheckReceiptDateWithINSDate() {
        var ObjControl;
        var objINSDate;
        var sINSDate = '';
        var sReceiptDate = '';      

        // Get Vehicle In Date
        ObjControl = document.getElementById("ContentPlaceHolder1_txtRefDate");

        if (ObjControl != null) {
            sReceiptDate = ObjControl.value;
        }
        if (sReceiptDate == "") return;

        // Get ETB Invoice Date
        objINSDate = document.getElementById("ContentPlaceHolder1_txtINSDate_txtDocDate");

        if (objINSDate != null) {
            sINSDate = objINSDate.value;
        }
        if (sINSDate == "") return;

        

        if (bCheckFirstDateIsGreaterThanSecondDate(sReceiptDate, sINSDate) == true) {
            
            if (sReceiptDate == sINSDate) return;    
            
            alert('INS Date Should be greater than or Equal to Receipt Date !');
            objINSDate.value = '';
            objINSDate.focus();
            return false;
        }
        }

        //Sujata 11012011

        //Sujata 28022011
        function BillTypeChange() {                     
            var drpDepo = document.getElementById("ContentPlaceHolder1_drpDepo");            
            var drpBillType = document.getElementById("ContentPlaceHolder1_drpBillType");
            var drpPlant = document.getElementById("ContentPlaceHolder1_drpPlant");
            var lblDepoPlantMan = document.getElementById("ContentPlaceHolder1_lblDepoPlantMan");
            var lblDealerDepoID = document.getElementById("ContentPlaceHolder1_lblDealerDepoID");
            var txtDepotPlantID = document.getElementById("ContentPlaceHolder1_txtDepotPlantID");

            if (drpBillType.value == "0") {
                txtDepotPlantID.innerText = "";                
//                drpDepo.Visible = false;
//                drpPlant.Visible = false;
//                lblDepoPlantMan.Visible = false;
                drpPlant.Attributes.Add("DispalyNon", "none");
                drpDepo.Attributes.Add("DispalyNon", "none");
                lblDepoPlantMan.Attributes.Add("DispalyNon", "none");
            }
            else if (drpBillType.value == "D") {
                txtDepotPlantID.innerText = "Depot Name";
                drpDepo.selectedIndex = lblDealerDepoID.innerText;
//                drpDepo.Visible = true;
//                drpPlant.Visible = false;
//                lblDepoPlantMan.Visible = true;
                drpPlant.Attributes.Add("DispalyNon", "none");
                drpDepo.Attributes.Add("DispalyNon", "");
                lblDepoPlantMan.Attributes.Add("DispalyNon", "");
            }
            else if (drpBillType.value == "P") {
                txtDepotPlantID.innerText = "Plant Name";
//                drpPlant.Visible = true;
//                drpDepo.Visible = false;
//                lblDepoPlantMan.Visible = true;
                drpPlant.Attributes.Add("DispalyNon", "");
                drpDepo.Attributes.Add("DispalyNon", "none");
                lblDepoPlantMan.Attributes.Add("DispalyNon", "");
            }
            return true;
        }
        //Sujata 28022011