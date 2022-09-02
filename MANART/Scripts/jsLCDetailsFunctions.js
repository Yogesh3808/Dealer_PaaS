  function Show(para)
              {    
    var Flage=document.getElementById('ContentPlaceHolder1_txtFlage').value
    window.showModalDialog("frmSelectDocOnNew.aspx?FormID=" +ActivityID+ "&Flage="+Flage ,"List", "dialogHeight: 700px; dialogWidth: 1200px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");                
             }
    
    function LCTypeOption(OptionFor)
    {
     if(OptionFor="Amendment")
     {
    var OptYes;
    var OptNo;
    var txtAmendment;
    var lblAmendment;
    var lblAmendmentDate;
    var txtAmendmentDate;
      OptYes=document.getElementById("ContentPlaceHolder1_OptAmendment_0");
     OptNo=document.getElementById("ContentPlaceHolder1_OptAmendment_1");
     txtAmendment=document.getElementById("ContentPlaceHolder1_txtAmendment");
     lblAmendment=document.getElementById("ContentPlaceHolder1_lblAmendment");
     lblAmendmentDate=document.getElementById("ContentPlaceHolder1_lblAmendmentDate");
    txtAmendmentDate=document.getElementById("ContentPlaceHolder1_txtAmendmentDate_txtDocDate");
     
           if(OptYes.checked)
             {
                 txtAmendment.style.display='' ;
                 lblAmendment.style.display='' ;
                 lblAmendmentDate.style.display='' ;
                 txtAmendmentDate.style.display='' ;
              }
                  if(OptNo.checked)
                 {
                 txtAmendment.style.display='none' ;
                 lblAmendment.style.display='none' ;
                 lblAmendmentDate.style.display='none' ;
                 txtAmendmentDate.style.display='none' ;
                  }
            }
    }
    
    //Dummy LC Validations
    function CheckDummyLCValidation()
    {
     var txtProformaNo=document.getElementById("ContentPlaceHolder1_txtProformaNo");
    var txtLCNo=document.getElementById("ContentPlaceHolder1_txtLCNo");
    var txtLCDate=document.getElementById("ContentPlaceHolder1_CurrentLCDate_txtDocDate");
    var txtAmount=document.getElementById("ContentPlaceHolder1_txtAmount");
    //Sujata 18072013_Begin
    //var txtExpDate=document.getElementById("ContentPlaceHolder1_ExpiryDate_txtDocDate");
    //Sujata 18072013_End
       var lblDummyLC=document.getElementById("ContentPlaceHolder1_lblIsDummy");
     if(txtProformaNo.value == '')
    {
     alert("Select Proforma Invoice To Create New LC!");
     //txtProformaNo.focus();
        return false;
    }
    
     if(txtLCNo.value == '')
    {
     alert("Please Enter LC NO !");
     txtLCNo.focus();
        return false;
    }
    if(lblDummyLC.innerHTML=="N")
    {
    if(txtLCNo.value=="99999")
    {
    alert("Please Enter LC Original LC No.(Except:99999)!");
    txtLCNo.value="";
    txtLCNo.focus();
        return false;
    }
    
    }
     if(txtLCDate.value == '')
    {
     alert("Please Enter LC Date !");
     txtLCDate.focus();
        return false;
    }
    
    //Sujata 18072013_Begin
//     if(txtExpDate.value == '')
//    {
//     alert("Please Enter Expiry Date !");
//     txtExpDate.focus();
//     return false;
//    }    
//     var LCDate=getDateObject(txtLCDate.value,"/");
//    var LCExprDate=getDateObject(txtExpDate.value,"/");
//    if(LCDate >= LCExprDate)
//    {
//    alert("LC  Expire Date Should Be Greater Than LC Date");
//    txtExpDate.focus();
//     return false;
//    }
    //Sujata 18072013_End
    
    var OptYes;
    var OptNo;
    var txtAmendment;
    var txtAmendmentDate;
     OptYes=document.getElementById("ContentPlaceHolder1_OptAmendment_0")
     OptNo=document.getElementById("ContentPlaceHolder1_OptAmendment_1")
     txtAmendment=document.getElementById("ContentPlaceHolder1_txtAmendment")
     txtAmendmentDate=document.getElementById("ContentPlaceHolder1_txtAmendmentDate_txtDocDate");
     
      if(OptYes.checked)
             {
              if(txtAmendment.value == '')
    {
     alert("Please Enter Amendment  !");
     return false;
     }      
     if(txtAmendmentDate.value == '')
    {
     alert("Please Enter Amendment Date !");
     return false;
     }      
         }
        
        if(CheckLCDetailsValidation() == false)
    {
      return false;
     } 
       return true;
    }
    
    
    //Original LC Validation
    function CheckLCValidation()
    {
    var txtProformaNo=document.getElementById("ContentPlaceHolder1_txtProformaNo");
    var txtLCNo=document.getElementById("ContentPlaceHolder1_txtLCNo");
    var txtLCDate=document.getElementById("ContentPlaceHolder1_CurrentLCDate_txtDocDate");
    var txtAmount=document.getElementById("ContentPlaceHolder1_txtAmount");
    var txtExpDate=document.getElementById("ContentPlaceHolder1_ExpiryDate_txtDocDate");
    var txtExpiryAt=document.getElementById("ContentPlaceHolder1_txtExpiryAt");
    var txtOpeningBank=document.getElementById("ContentPlaceHolder1_txtOpeningBank");
    var txtNegotiatingBank=document.getElementById("ContentPlaceHolder1_txtNegotiatingBank");
    var txtReimbursingBank=document.getElementById("ContentPlaceHolder1_txtReimbursingBank");
    var txtDrawee=document.getElementById("ContentPlaceHolder1_txtDrawee");
    var txtConfirmationStatus=document.getElementById("ContentPlaceHolder1_txtConfirmationStatus");
    var txtMarksNo=document.getElementById("ContentPlaceHolder1_txtMarksNo");
    var txtInvoice=document.getElementById("ContentPlaceHolder1_txtInvoice");
    var txtPackingList=document.getElementById("ContentPlaceHolder1_txtPackingList");
    var txtWeightList=document.getElementById("ContentPlaceHolder1_txtWeightList");
    var txtCOO=document.getElementById("ContentPlaceHolder1_txtCOO");
    var lblDummyLC=document.getElementById("ContentPlaceHolder1_lblIsDummy");
    var drpNPeriod=document.getElementById("ContentPlaceHolder1_drpNegotioationPeriod");    
   
    if(txtProformaNo.value == '')
    {
     alert("Select Proforma Invoice To Create New LC!");
     //txtProformaNo.focus();
        return false;
    }
    
     if(txtLCNo.value == '')
    {
     alert("Please Enter LC NO !");
     txtLCNo.focus();
        return false;
    }
    if(lblDummyLC.innerHTML=="N")
    {
    if(txtLCNo.value=="99999")
    {
    alert("Please Enter LC Original LC No.(Except:99999)!");
    txtLCNo.value="";
    txtLCNo.focus();
        return false;
    }
    
    }
     if(txtLCDate.value == '')
    {
     alert("Please Enter LC Date !");
     txtLCDate.focus();
        return false;
    }
      if(txtAmount.value == '')
    {
     alert("Please Enter LC Amount !");
     txtAmount.focus();
     return false;
    }
     if(txtExpDate.value == '')
    {
     alert("Please Enter Expiry Date !");
     txtExpDate.focus();
     return false;
    }
    if(txtExpiryAt.value == '')
    {
     alert("Please Enter Expiry At !");
     txtExpiryAt.focus();
     return false;
    }    
     if(txtOpeningBank.value == '')
    {
     alert("Please Enter Opening Bank !");
     txtOpeningBank.focus();
     return false;
    }
     if(txtNegotiatingBank.value == '')
    {
     alert("Please Enter Negotiation Bank !");
     txtNegotiatingBank.focus();
     return false;
    }
     if(txtReimbursingBank.value == '')
    {
     alert("Please Enter Reimbursing Bank !");
     txtReimbursingBank.focus();
     return false;
    }
     if(txtDrawee.value == '')
    {
     alert("Please Enter Drawee  !");
     txtDrawee.focus();
     return false;
    }
    if(drpNPeriod.value == "0")
    {
     alert("Please Enter Negotiation Period !");
     drpNPeriod.focus();
     return false;
    }
     if(txtConfirmationStatus.value == '')
    {
     alert("Please Enter Confirmation Status  !");
     txtConfirmationStatus.focus();
     return false;
    }
     if(txtMarksNo.value == '')
    {
     alert("Please Enter Marks No  !");
     txtMarksNo.focus();
     return false;
    }
     if(txtInvoice.value == '')
    {
     alert("Please Enter Invoice No. Of Copeis  !");
     txtInvoice.focus();
     return false;
    }
      if(txtPackingList.value == '')
    {
     alert("Please Enter Packing List No. Of Copeis  !");
     txtPackingList.focus();
     return false;
    }
     if(txtWeightList.value == '')
    {
     alert("Please Enter Weight List No. Of Copeis  !");
     txtWeightList.focus();
     return false;
    }
     if(txtCOO.value == '')
    {
     alert("Please Enter COO No. Of Copeis  !");
     txtCOO.focus();
     return false;
     }
     var LCDate=getDateObject(txtLCDate.value,"/");
    var LCExprDate=getDateObject(txtExpDate.value,"/");
    if(LCDate >= LCExprDate)
    {
    alert("LC  Expire Date Should Be Greater Than LC Date");
    txtExpDate.focus();
     return false;
    }
    
    var OptYes;
    var OptNo;
    var txtAmendment;
     OptYes=document.getElementById("ContentPlaceHolder1_OptAmendment_0")
     OptNo=document.getElementById("ContentPlaceHolder1_OptAmendment_1")
     txtAmendment=document.getElementById("ContentPlaceHolder1_txtAmendment")
     
      if(OptYes.checked)
             {
              if(txtAmendment.value == '')
    {
     alert("Please Enter Amendment  !");
     return false;
     }      
         }
        
        if(CheckLCDetailsValidation() == false)
    {
      return false;
     } 
              
                
                 return true;
    }
    
    function getDateObject(dateString,dateSeperator)
{
//This function return a date object after accepting 
//a date string ans dateseparator as arguments
var curValue=dateString;
var sepChar=dateSeperator;
var curPos=0;
var cDate,cMonth,cYear;

//extract day portion
curPos=dateString.indexOf(sepChar);
cDate=dateString.substring(0,curPos);

//extract month portion 
endPos=dateString.indexOf(sepChar,curPos+1); cMonth=dateString.substring(curPos+1,endPos);

//extract year portion 
curPos=endPos;
endPos=curPos+5; 
cYear=curValue.substring(curPos+1,endPos);

//Create Date Object
dtObject=new Date(cYear,cMonth,cDate); 
return dtObject;
}


function CheckLCDetailsValidation() {
       var ObjGrid = null;
    ObjGrid = window.document.getElementById("ContentPlaceHolder1_GridItemDetails");
    var modelPart=window.document.getElementById("ContentPlaceHolder1_lblModel_Part");           //lblDummyLC.innerHTML
    if (ObjGrid == null) return;    
    var objtxtControl;
    var iCountDel = 0;
    var ObjControl;
    var bCheckValidation = true;
    var sMessage = "";
    if(modelPart.innerHTML=="M")
    {
    for (var i = 1; i < ObjGrid.rows.length; i++) 
    {
        //debugger;
            // Check HS Code          
//            objtxtControl = ObjGrid.rows[i].cells[2].children[0];
//            if (objtxtControl.value == "" || objtxtControl.value == null) 
//            {
//                alert("Please Enter HS Code " + i);
//               
//               return false;
//            }
            //Check Model Description
            objtxtControl = ObjGrid.rows[i].cells[5].children[0];
            if (objtxtControl.value == "" || objtxtControl.value == null) 
            {
                alert("Please Enter description  " + i);
                
               return false;
            }
        }
     }
    return true;
}

function alphaNumOnly(eventRef)
{
 var keyStroke = (eventRef.which) ? eventRef.which : (window.event) ? window.event.keyCode : -1;
 var returnValue = false;

 if ( ((keyStroke >= 65) && (keyStroke <= 90)) ||
      ((keyStroke >= 97) && (keyStroke <= 122)) ||
      ((keyStroke >= 48) && (keyStroke <= 57)) )
      {
         returnValue = true;
         if ( navigator.appName.indexOf('Microsoft') != -1 )
  window.event.returnValue = returnValue;

 return returnValue;
      }
      else
      {
     alert ("Special characters  not allowed.");
     return returnValue;
      }


  }
  function CheckAdvanceDetailsValidation() {
      var ObjGrid = null;
      ObjGrid = window.document.getElementById("ContentPlaceHolder1_GridItemDetails");
      var modelPart = window.document.getElementById("ContentPlaceHolder1_lblModel_Part");           //lblDummyLC.innerHTML
      if (ObjGrid == null) return;
      var objtxtControl;
      var iCountDel = 0;
      var ObjControl;
      var bCheckValidation = true;
      var sMessage = "";       
      if (modelPart.innerHTML == "M") {
          for (var i = 1; i < ObjGrid.rows.length; i++) {                            
              //Check Model Description
              objtxtControl = ObjGrid.rows[i].cells[4].children[0];
              if (objtxtControl.value == "" || objtxtControl.value == null) {
                  alert("Please Enter description  " + i);

                  return false;
              }
          }
      }
      return true;
  }

function CheckAdvPaymentValidation()
    {
    var txtLCNo=document.getElementById("ContentPlaceHolder1_txtLCNo");
    var txtLCDate=document.getElementById("ContentPlaceHolder1_CurrentLCDate_txtDocDate");
    //var txtAmount=document.getElementById("ContentPlaceHolder1_txtAmount");
    var txtMarksNo=document.getElementById("ContentPlaceHolder1_txtMarksNo");
     var lblDummyLC=document.getElementById("ContentPlaceHolder1_lblIsDummy");
     var drpNPeriod = document.getElementById("ContentPlaceHolder1_drpNegotioationPeriod");
     var txtOpeningBank = document.getElementById("ContentPlaceHolder1_txtOpeningBank");
     var txtNegotiatingBank = document.getElementById("ContentPlaceHolder1_txtNegotiatingBank");
     var txtReimbursingBank = document.getElementById("ContentPlaceHolder1_txtReimbursingBank");
     var txtDrawee = document.getElementById("ContentPlaceHolder1_txtDrawee");
     
     if(txtLCNo.value == '')
    {
     alert("Please Enter Advance Payment Document NO !");
     txtLCNo.focus();
        return false;
    }
    if(lblDummyLC.innerHTML=="N")
    {
    if(txtLCNo.value=="99999")
    {
    alert("Please Enter Advance Payment Document NO(Except:99999)!");
    txtLCNo.value="";
    txtLCNo.focus();
        return false;
    }
    }
     if(txtLCDate.value == '')
    {
     alert("Please Enter Advance Payment Document  Date !");
     txtLCDate.focus();
        return false;
    }
//      if(txtAmount.value == '')
//    {
//     alert("Please Enter Advance Payment Document  Amount !");
//     txtAmount.focus();
//     return false;
//    }
    if (txtMarksNo.value == '') {
        alert("Please Enter Marks No !");
        txtMarksNo.focus();
        return false;
    }
    if (drpNPeriod.value == "0") {
        alert("Please Enter Negotiation Period !");
        drpNPeriod.focus();
        return false;
    }
    if (txtNegotiatingBank.value == '') {
        alert("Please Enter Negotiation Bank !");
        txtNegotiatingBank.focus();
        return false;
    }
     if(txtOpeningBank.value == '')
    {
     alert("Please Enter Opening Bank !");
     txtOpeningBank.focus();
     return false;
    }     
     if(txtReimbursingBank.value == '')
    {
     alert("Please Enter Reimbursing Bank !");
     txtReimbursingBank.focus();
     return false;
    }
     if(txtDrawee.value == '')
    {
     alert("Please Enter Drawee  !");
     txtDrawee.focus();
     return false;
    }
    if (CheckAdvanceDetailsValidation() == false)
    {
      return false;
     } 
    return true;
    }
    
function CheckDateGreater(obj1,obj2,obj3)
{
    var splDate=obj1.value.split("/")
    var splDate1=obj2.value.split("/")
    var dt=new Date(splDate[2],splDate[1],splDate[0]);
    var dt1=new Date(splDate1[2],splDate1[1],splDate1[0]);
    
    if(dt<dt1)
    {
        alert (obj3)
        d=parseInt(splDate1[0]) + 1
        obj1.value=d.toString() + "/" + splDate[1] + "/" + splDate[2]      
        obj1.focus();
        return false;
    }
}

function CheckLCDate() 
{ 

   var sShipmentDate="";
    var ObjShipmentDate = window.document.getElementById("ContentPlaceHolder1_CurrentLCDate_txtDocDate");
    var x= new Date();
    var y= x.getYear();
    var m= x.getMonth()+1 ; // added +1 because javascript counts month from 0
    var d= x.getDate();
    var dtCur=d+'/'+m+'/'+y;
    var dtCurDate=new Date(x.getYear(),x.getMonth(),x.getDate(),00,00,00,000)
    sShipmentDate =ObjShipmentDate.value;
    var sTmpValue = sShipmentDate;
    var day = dGetValue(sTmpValue.split("/")[0]);
    var month = dGetValue(sTmpValue.split("/")[1]) - 1;
    var year = dGetValue(sTmpValue.split("/")[2]);
    var sTmpDate =new Date(year, month, day,00,00,00,000);
    var TmpDay = 0;    
    
    if (sShipmentDate == '' ) 
    {
        return false;
    }   
    if(dtCurDate<sTmpDate)
     {
        alert ("LC Date should be Less or equal to Current Date")
        ObjShipmentDate.value="";
        ObjShipmentDate.focus();
        return false;
    }
}
   
