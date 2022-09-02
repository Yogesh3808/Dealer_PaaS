
      //function to check three types of characters in password
    function ValidateThreeCharTypesPwd(Pwd)
         {
            var MainCount=0;
            var SpecialCharCount=0;
            var RegCheckSmall= /[a-z]/;
            var RegCheckCaps= /[A-Z]/;
            var RegCheckDigits= /[0-9]/;
            var pos = Pwd.indexOf("@"); // See if a question mark is present in string (! - @ - $) 
            var pos1 = Pwd.indexOf("!");
            var pos2 = Pwd.indexOf("$");
            var pos3 = Pwd.indexOf("#");
            var pos4 = Pwd.indexOf("%");
            var pos5 = Pwd.indexOf("^");
            var pos6 = Pwd.indexOf("&");
            var pos7 = Pwd.indexOf("*");
            var pos8 = Pwd.indexOf("_");
            var pos9 = Pwd.indexOf("-");
           
           if (Pwd.search(RegCheckSmall)==-1) //if match failed
               { // alert('Do not have small letter'); 
               }
           else
               {    MainCount = MainCount + 1;}    
                
           if (Pwd.search(RegCheckCaps)==-1) //if match failed
               {// alert('Do not have Capital letter');
               }
            else
               {    MainCount = MainCount + 1;}     
               
           if (Pwd.search(RegCheckDigits)==-1) //if match failed
               {//  alert('Do not have Digits');
               }
            else
               {  MainCount = MainCount + 1; }         
          
            if (pos == -1)
                { }
            else
               {  SpecialCharCount = SpecialCharCount + 1;}    
            if (pos1 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;}    
             if (pos2 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;} 
             
             if (pos3 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;}   
                if (pos4 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;}  
               
               if (pos5 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;} 
               
                if (pos6 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;} 
               
               if (pos7 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;} 
               if (pos8 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;} 
               
               if (pos9 == -1)
                {}
            else
               {  SpecialCharCount = SpecialCharCount + 1;} 
               
               
            if (SpecialCharCount > 0)
              {  MainCount = MainCount + 1;} 
               else
               {    //alert('do not have special char');
               }
            
            if (MainCount > 3)   
                 {      return 1;   }
            else
                 {      return 0;  }                                         
                       
  }//end of function
  
  //function to check first four characters of username in password
    function ValidateUsernameInPwd(name,pass)
        {                          
            var uname = name.toLowerCase().substring(0,4);  
            var rslt= pass.toLowerCase().indexOf(uname);
                        
           if(rslt != -1 )
           {
                //alert("Inncorrect");
                return 0;
           }           
           else
           {
               //alert("Correct" );
               return 1;
           }
        }
        ///////////////////Password Policy ends here


    function validate() {
        //debugger;
            var drpLevels = document.getElementById("ContentPlaceHolder1_drpLevels");
            var drpUserType = document.getElementById("ContentPlaceHolder1_drpUserType");


            var jtxtName = document.getElementById('ContentPlaceHolder1_txtLoginName');
            var txtFirstName = document.getElementById('ContentPlaceHolder1_txtFirstName');
            var txtLastName = document.getElementById('ContentPlaceHolder1_txtLastName');

            var TextBox_number = document.getElementById('ContentPlaceHolder1_TextBox_number');           

            var txtEmpCode = document.getElementById('ContentPlaceHolder1_txtEmpCode');

            var iChars = "'\"";
            var iCharsAddress = "!@$%^&*()+=[]\\\';{}|\":<>?~";


            if (drpUserType.value == "0") {
                alert("Please Select User Type !");
                drpUserType.focus();
                return false;
            }

            if (drpLevels.value == "0") {
                alert("Please Select User Role !");
                drpLevels.focus();
                return false;
            }
            if ((drpUserType.value == "1" || drpUserType.value == "2") && drpUserType.value != "6") {
                var drpDept = document.getElementById("ContentPlaceHolder1_drpDept");
                if (drpDept.value == "0") {
                    alert("Please Select Department !");
                    drpLevels.focus();
                    return false;
                }
                else if (drpDept.value == "5" || drpDept.value == "7" || drpDept.value == "1" || drpDept.value == "3") {
                   
                        var chkDept = document.getElementById("ContentPlaceHolder1_chkModelCategory");
                        if (chkDept != null) {
                            var chkBoxCount = chkDept.getElementsByTagName("input");
                            var iChkCnt = 0;
                            for (var i = 0; i < chkBoxCount.length; i++) {
                                if (chkBoxCount[i].checked == true && chkBoxCount[i].disabled == false) {
                                    iChkCnt = iChkCnt + 1;
                                }
                            }
                            if (iChkCnt == 0) {
                                alert("Please Select atleast one Model Catagory !");
                                return false;
                            }
                        }
                }
            }
            else if(drpUserType.value != "6") {
                var chkDept = document.getElementById("ContentPlaceHolder1_chkDept");
                if (chkDept != null) {
                    var chkBoxCount = chkDept.getElementsByTagName("input");
                    var iChkCnt = 0;
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        if (chkBoxCount[i].checked == true && chkBoxCount[i].disabled == false) {
                            iChkCnt = iChkCnt + 1;
                        }

                    }
                    if (iChkCnt == 0 && drpUserType.value != "3" && drpUserType.value != "4" && drpUserType.value != "9") {
                        alert("Please Select atleast one Department !");
                        return false;
                    }
                }
            }

            if (jtxtName.value=="") {
                alert("Login Name: Login name should not blank.");                
                return false;
            }
            if (txtFirstName.value == "") {
                alert("First Name: First name should not blank.");
                return false;
            }
            if (drpUserType.value != "3" && drpUserType.value != "4" && drpUserType.value != "6" && txtLastName.value == "" && drpUserType.value != "9") {
            //if (txtLastName.value == "") {
                alert("Last Name: Last name should not blank.");
                return false;
            }
            if (jtxtName.value.length < 3) {
                alert("Login Name: Should be atleast 3 characters long.")// Change from 4 to 3 as discussed with Deepti Ma'am
                document.getElementById('ContentPlaceHolder1_txtLoginName').focus();
                return false;
            }

            if (TextBox_number.value == "") {
                alert("Last Name: Please enter number shown above.");
                TextBox_number.focus();
                return false;
            }

            //First name
            if (document.getElementById("ContentPlaceHolder1_txtFirstName").value != '') {
                for (var i = 0; i < document.getElementById("ContentPlaceHolder1_txtFirstName").value.length; i++) {
                    if (document.getElementById("ContentPlaceHolder1_txtFirstName").value.charAt(0) == '') {
                        alert("First Name: Please do not enter space(s).");
                        document.getElementById("ContentPlaceHolder1_txtFirstName").focus();
                        return false;
                    }

                }
            }
            else {
                alert("First Name: Please enter First Name.");
                document.getElementById('ContentPlaceHolder1_txtFirstName').focus();
                return false;
            }


        //Last Name
            if (drpUserType.value != "3" && drpUserType.value != "4" && drpUserType.value != "9") {
                if (document.getElementById("ContentPlaceHolder1_txtLastName").value != '') {
                    for (var i = 0; i < document.getElementById("ContentPlaceHolder1_txtLastName").value.length; i++) {
                        if (document.getElementById("ContentPlaceHolder1_txtLastName").value.charAt(0) == '') {
                            alert("Last Name: Please do not enter space(s).");
                            document.getElementById("ContentPlaceHolder1_txtLastName").focus();
                            return false;
                        }
                    }
                }
                else {
                    alert("Last Name: Please enter Last Name.");
                    document.getElementById('ContentPlaceHolder1_txtLastName').focus();
                    return false;
                }
            }



            //txtEmail
            if (document.getElementById("ContentPlaceHolder1_txtEmail").value == '') {
                alert("Email: Please enter a valid e-mail address, such as abc@def.ghi")
                document.getElementById("ContentPlaceHolder1_txtEmail").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtEmail").disabled == false) {

                var result = validEmail(document.getElementById("ContentPlaceHolder1_txtEmail").value)
                if (result != "") {
                    alert("Email: Please enter a valid e-mail address, such as abc@def.ghi\n\n")
                    document.getElementById("ContentPlaceHolder1_txtEmail").focus();
                    return false;
                }
            }            

        }

function clearform()
{
    document.getElementById('ContentPlaceHolder1_txtLoginName').value = "";
    document.getElementById('ContentPlaceHolder1_txtPassword').value = "";
    document.getElementById('ContentPlaceHolder1_txtRPassword').value = "";
    //document.getElementById('combDept').value = "0"; 
    document.getElementById('ContentPlaceHolder1_txtFirstName').value = "";
    document.getElementById('ContentPlaceHolder1_txtLastName').value = "";
    //document.getElementById('ContentPlaceHolder1_txtAddress').value = "";
    document.getElementById('ContentPlaceHolder1_txtEmail').value = "";
    document.getElementById('ContentPlaceHolder1_txtLoginName').focus();	
}	

function checkName(nameStr) 
{
	var result="";
	nameStr = nameStr.charAt(0);
	regRes = new RegExp("^[a-zA-Z]"); 	
	result=nameStr.match(regRes)
	if(result == null)	
	{
		result = 1
	}
	else
	{
		result = 0
	}		
	return result;	
}	

function hotkeyHandler()
{ 
if (event.keyCode == 13)
  {
     validate();      
  }
}	
function PhoneValid(strstring) 
{
	var result = ""
	var regRes = new RegExp("[0-9]{5}-[0-9]{4}")
	
	
	result = strstring.match(regRes)
	if( result == null)
	{
		result = false;
	}
	else
	{
		result = true;
	}
	return result;
}
	
function PhoneValid1(strstring) 
{
	var result = ""
	var regRes = new RegExp("[0-9]{3}-[0-9]{3}-[0-9]{4}")
	result = strstring.match(regRes)
	if( result == null)
	{
		result = false;
	}
	else
	{
		result = true;
	}
	return result;
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


function ChangePasswordvalidate()
{

    var jtxtName = document.getElementById('ContentPlaceHolder1_lblUserName').innerHTML;
    var jtxtPassword = document.getElementById('ContentPlaceHolder1_txtPassword').value;
    var jtxtRPassword = document.getElementById('ContentPlaceHolder1_txtRPassword').value;
    var jOldPassword = document.getElementById('ContentPlaceHolder1_txtOldPassword');
    var jhdnPassword = document.getElementById('ContentPlaceHolder1_hdnPassword');
		var iChars = "'\"";
	var iCharsAddress = "!@$%^&*()+=[]\\\';{}|\":<>?~";



	//Old Password
	if (jOldPassword.value == '') {
	    alert("Old Password: Please enter the Old Password.");
	    document.getElementById('ContentPlaceHolder1_txtOldPassword').focus();
	    return false;
	}
	else if (jOldPassword.value != jhdnPassword.value) {
	alert("Password: Your Old Password is Incorrect.");
	jOldPassword.value.value = "";
	document.getElementById('ContentPlaceHolder1_txtOldPassword').focus();
	return false;
	}	
 	
 	
 	
 	//Password
	if (jtxtPassword =='')
	{
	    alert("Password: Please enter the password.");
		document.getElementById('ContentPlaceHolder1_txtPassword').focus();
		return false;
	}
	else
	{  
		var pwd = jtxtPassword ;
		var user = jtxtName; 
		if (pwd.search(" ")<= -1)
		{
		    if (pwd.length < 8 && pwd.length > 15)
			{
	
				alert("Invalid Password.\nPassword must contain 8 to 15 characters.");
				document.getElementById('ContentPlaceHolder1_txtPassword').focus();
				return false;
			}
			
						
			//call to a function to check part of username in pwd
			var rslt=ValidateUsernameInPwd(user,pwd);
			if(rslt==0 )
			{
			    alert("Password should not contain part of Login Name.");
			    document.getElementById('ContentPlaceHolder1_txtPassword').focus();
				return false;
			}		
			
			// call to a function to validate Password Policy
			var validrslt = ValidateThreeCharTypesPwd(pwd);
			
			if(validrslt == 0 )
			{
                alert("Invalid Password.\nPassword must contain atleast one character of types(lower case, UPPER CASE, Numeric (Number 1-2-3), and symbol (! - @ - $)\nSample = Pa55word!");
                document.getElementById('ContentPlaceHolder1_txtPassword').focus();
				return false;			
			}
			//Password Policy ends here	 
			
			
		}		
		else				   
		{
			alert("Password: Please do not enter space(s).")
			document.getElementById('ContentPlaceHolder1_txtPassword').focus();
			return false;  
		}
		
	}	
	
	
	
		    
	if (jtxtRPassword =='')
	{
	    alert("Retype your Password: Please Retype the new Password.")
		document.getElementById('ContentPlaceHolder1_txtRPassword').focus();
		return false;
	}
	else
	{  
		var pwd = jtxtRPassword; 
		if (pwd.search(" ")<= -1)
		{
			if (pwd.length < 8)
			{
				alert("Invalid Retype Password must match Password.")
				document.getElementById('ContentPlaceHolder1_txtRPassword').focus();
				return false;
			}
		}		
		else				   
		{
			alert("Retype your Password: Please do not enter space(s).")
			document.getElementById('ContentPlaceHolder1_txtRPassword').focus();
			return false;  
		}
	}
	var upperpwd1 = jtxtPassword;
	var upperpwd2 = jtxtRPassword;
	var Oupperpwd = jOldPassword.value

	if (upperpwd1 == Oupperpwd) {
	    alert("Old Password and New Password should not be same.")
	    document.getElementById('ContentPlaceHolder1_txtPassword').value = '';
	    document.getElementById('ContentPlaceHolder1_txtPassword').focus();
	    return false;
	} 
   
	if (upperpwd1 != upperpwd2)
	{
		alert("Password and Retype Password are not same.")
		document.getElementById('ContentPlaceHolder1_txtRPassword').value = '';
		document.getElementById('ContentPlaceHolder1_txtPassword').value = '';
		document.getElementById('ContentPlaceHolder1_txtPassword').focus();
		return false;
	}
	  
 	
 	
	}
	