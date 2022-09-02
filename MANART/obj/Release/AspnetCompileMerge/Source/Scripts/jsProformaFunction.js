

//To Set Enable disable working of the moddl destination control
function EnableMultiModlaDestination(ObjMultiModalCtrl, MultiModlaDestinationID) {
    var sSelecedValue = ObjMultiModalCtrl.options[ObjMultiModalCtrl.selectedIndex].text;
    var ParentCtrlID;
    var objMultiModlaDestination;
    ParentCtrlID = ObjMultiModalCtrl.id.substring(0, ObjMultiModalCtrl.id.lastIndexOf("_"));
    objMultiModlaDestination = document.getElementById(ParentCtrlID + "_" + MultiModlaDestinationID);
    if (objMultiModlaDestination == null) return false;    
    if (sSelecedValue == "Y") {
        objMultiModlaDestination.disabled = false;
    }
    else if (sSelecedValue == "N") {
        objMultiModlaDestination.selectedIndex = 0;
        objMultiModlaDestination.disabled = true;
    }
}
//Calculate Total on Qtychange
function CalculateTotalOnQtyChange(event,ObjControl,txtUnitAmtID,txtLineTotalAmtID,txtTotalAmtID,txtGrandTotalAmtID) {
    if (CheckTextboxValueForNumeric(event, ObjControl,false) == false)
    {
        //ObjControl.focus(); 
        return ;
    }
    else
    {
       var ParentCtrlID;
       var objtxtUnitAmount;
       var objtxtLineTotalAmt;
       var objtxtTotalAmt;
       var objtxtGrandTotalAmt;
       var ContainerName;

       if (dGetValue(ObjControl.defaultValue) < dGetValue(ObjControl.value)) {
           alert("Qty should not be greater then original Qty(" + ObjControl.defaultValue + ")");
           ObjControl.value = ObjControl.defaultValue;
           return false;
       }

       //AddValueToAmountPerUnitForFobRate(event, ObjControl, txtUnitAmtID, txtTotalAmtID, txtGrandTotalAmtID, 1);
       return AddValueToAmountPerUnit(event, ObjControl, txtUnitAmtID, txtTotalAmtID, txtGrandTotalAmtID, 1);
       
//       ParentCtrlID=ObjControl.id.substring(0, ObjControl.id.lastIndexOf("_"));    
//       
//       //Get Object Of Unit Amount
//       objtxtUnitAmount= document.getElementById(ParentCtrlID+"_"+ txtUnitAmtID);
//       
//       //Get Object Of Line Total        
//       objtxtLineTotalAmt= document.getElementById(ParentCtrlID+"_"+ txtLineTotalAmtID);       
//       
//       // To get Object of Heade Total 
//       ContainerName =GetContainerName();
//       objtxtTotalAmt =document.getElementById("ContentPlaceHolder1_txtTotalAmt");    
//       objtxtTotalAmt.readOnly=true;
//       
//       // To get Object of Grand Total 
//       ContainerName =GetContainerName();
//       objtxtGrandTotalAmt = document.getElementById("ContentPlaceHolder1_txtGrandTotalAmt");       
//       objtxtGrandTotalAmt.readOnly=true;
//       
//       if (objtxtGrandTotalAmt.value !="")
//       {
//            objtxtTotalAmt.value=RoundupValue(dGetValue( objtxtTotalAmt.value) - dGetValue(objtxtLineTotalAmt.value));
//            objtxtGrandTotalAmt.value=RoundupValue(dGetValue( objtxtGrandTotalAmt.value) - dGetValue(objtxtLineTotalAmt.value));
//       }
//       else
//       {
//            objtxtTotalAmt.value=0;
//            objtxtGrandTotalAmt.value=0;
//       }
//       // calculate Line wise Total
//       if (objtxtUnitAmount == null)
//       {
//        return ;
//       } 
//       else
//       {
//        objtxtLineTotalAmt.value= RoundupValue(dGetValue(objtxtUnitAmount.value) * dGetValue(ObjControl.value));
//       }
//       objtxtLineTotalAmt.readOnly=true;
//       // Calculate Grand Total
//       if (objtxtGrandTotalAmt.value !="")
//       {
//            objtxtTotalAmt.value= RoundupValue(dGetValue(objtxtTotalAmt.value) + dGetValue(objtxtLineTotalAmt.value));
//            objtxtGrandTotalAmt.value = RoundupValue(dGetValue(objtxtGrandTotalAmt.value) + dGetValue(objtxtLineTotalAmt.value));        
//       }
//       else
//       {
//            objtxtTotalAmt.value= RoundupValue(objtxtLineTotalAmt.value);
//            objtxtGrandTotalAmt.value= RoundupValue(objtxtLineTotalAmt.value);
//       }
    }
}

//To calculate the amount in Vehicle Proforma
function AddValueToAmountPerUnitForFobRate(event, ObjControl, txtUnitAmtID, txtTotalAmtID, txtGrandTotalAmtID) {
//    if (CheckTextboxValueForNumeric(event, ObjControl,true) == false) {        
//        return false;
//    }
//    else 
//    {
        return AddValueToAmountPerUnit(event, ObjControl, txtUnitAmtID, txtTotalAmtID, txtGrandTotalAmtID,2);
//    }
}
////Calculate Total Model
//function AddValueToAmountPerUnit(event,ObjControl,txtUnitAmtID,txtTotalAmtID, txtGrandTotalAmtID) {

////    if (CheckTextboxValueForNumeric(event, ObjControl,false) == false)
////    {
////        ObjControl.value = '';
////        return false;
////    }
////    else
//    {
//       var ParentCtrlID;
//       var objtxtUnitAmount;
//       ParentCtrlID=ObjControl.id.substring(0, ObjControl.id.lastIndexOf("_"));    
//       objtxtUnitAmount= document.getElementById(ParentCtrlID+"_"+ txtUnitAmtID);       
//       if (objtxtUnitAmount == null)
//       {
//        return ;
//       } 
//       else
//       {
//           objtxtUnitAmount.value  =0;       
//           objtxtUnitAmount.readOnly=true;   
//           var objRow=ObjControl.parentNode.parentNode.childNodes;   
//           var dAmount;
//           var Qty=0;
//           var Total=0;
//           var ColIndex= objtxtUnitAmount.parentNode.cellIndex;// To Get Column index of Amount Per unit           
//           var ContainerName;
//           var objtxtLineTotalAmt;
//           var objtxtTotalAmt;
//           var objtxtGrandTotalAmt;
//           var dDiffofAmount = 0;
//           var dOrgLineAmount = 0;
//           var dDiscount = 0;         
//           var dUnitAmount=0;
//           var dLineTotal=0;
//           ObjControl =objRow[4].childNodes[0];
//           Qty =  ObjControl.value // To Get Qty

//           ObjControl = objRow[16].childNodes[0];
//           if (ObjControl.checked == false) return;
//           
//           for(i=5;i< 12 ;i++)// started from 5 i.e from Fobrate
//           {           
//                ObjControl =objRow[i].childNodes[0];
//                dAmount = 0;                
//                if (ObjControl.value =="")
//                {
//                    dAmount = 0;
//                }
//                else
//                 {
//                    ObjControl.value = RoundupValue(ObjControl.value);
//                    dAmount =  dGetValue(ObjControl.value);
//                }
//                
//                objtxtUnitAmount.value = RoundupValue(dGetValue(objtxtUnitAmount.value) + (dAmount));

//            }
//            
//            //Get Discount
//            dDiscount = dGetValue(objRow[12].childNodes[0].value);
//            objRow[12].childNodes[0].value = RoundupValue(dDiscount);
//           //To Get Object of Line Total
//           objtxtLineTotalAmt =objRow[14].childNodes[0];
//           dUnitAmount = dGetValue(objtxtUnitAmount.value);
//           if (dDiscount >= dUnitAmount) 
//           {
//               alert("Discount is greater than or equal to the Unit Amount,Please reentered the discount !");
//               objRow[12].childNodes[0].value = 0.00;
//               dDiscount = 0;
//           }
//           dUnitAmount = dUnitAmount - dDiscount;
//           objtxtUnitAmount.value = RoundupValue(dUnitAmount);
//           
//           dOrgLineAmount = dGetValue(objtxtLineTotalAmt.value);

//           //To calculate Line Total           
//           dLineTotal =(dGetValue(Qty) * dUnitAmount);
//           

//           
//           dLineTotal = dLineTotal;
//           objtxtLineTotalAmt.value = RoundupValue(dLineTotal);
//           
//            // To Calculate Diff            
//           dDiffofAmount = (dLineTotal - dOrgLineAmount);

//           // To get Object of Heade Total 
//           ContainerName = GetContainerName();
//           //objtxtTotalAmt = (document.getElementById(ContainerName + txtTotalAmtID));
//           objtxtTotalAmt = document.getElementById('ContentPlaceHolder1_txtTotalAmt'); 

//           // To get Object of Grand Total
//           objtxtGrandTotalAmt = document.getElementById('ContentPlaceHolder1_txtGrandTotalAmt');
//           
//           // To Calculate Grand Total
//           if (dGetValue(objtxtGrandTotalAmt.value) != "0")
//            {
//               objtxtTotalAmt.value = RoundupValue(dGetValue(objtxtTotalAmt.value) + dDiffofAmount);
//               objtxtGrandTotalAmt.value = RoundupValue(dGetValue(objtxtGrandTotalAmt.value) + dDiffofAmount);        
//           }
//           else
//           {
//                objtxtTotalAmt.value= RoundupValue(objtxtLineTotalAmt.value);
//                objtxtGrandTotalAmt.value= RoundupValue(objtxtTotalAmt.value);
//           }
//       }
//    }
//}

//Calculate Total Model
function AddValueToAmountPerUnit(event, ObjControl, txtUnitAmtID, txtTotalAmtID, txtGrandTotalAmtID, TextboxType) {

    //    if (CheckTextboxValueForNumeric(event, ObjControl,false) == false)
    //    {
    //        ObjControl.value = '';
    //        return false;
    //    }
    //    else
    {
        var ParentCtrlID;
        var objtxtUnitAmount;
        
        ParentCtrlID = ObjControl.id.substring(0, ObjControl.id.lastIndexOf("_"));
        objtxtUnitAmount = document.getElementById(ParentCtrlID + "_" + txtUnitAmtID);
        if (objtxtUnitAmount == null) {
            return;
        }
        else {
            objtxtUserRoleID = document.getElementById('ContentPlaceHolder1_txtUserRoleID');
            objhdbEditDisc = document.getElementById('ContentPlaceHolder1_hdbEditDisc');
            var objlblHeadUserIDDisc = document.getElementById(ParentCtrlID + "_txtHeadUserIDDisc");
            
//            objtxtUnitAmount.value = 0;
//            objtxtUnitAmount.readOnly = true;
            var objRow = ObjControl.parentNode.parentNode.childNodes;
            var dAmount;
            var Qty = 0;
            var Total = 0;
            var ColIndex = objtxtUnitAmount.parentNode.cellIndex; // To Get Column index of Amount Per unit           
            var ContainerName;
            var objtxtLineTotalAmt;
            var objtxtTotalAmt;
            var objtxtGrandTotalAmt;
            var dDiffofAmount = 0;
            var dOrgLineAmount = 0;
            var dDiscount = 0;
            var dUnitAmount = 0;
            var dOrgUnitAmount = 0;
            var dLineTotal = 0;
            ObjControl = objRow[4].childNodes[0];
            Qty = ObjControl.value // To Get Qty
            
            ObjControl = objRow[16].childNodes[0];            
            if (ObjControl.checked == false) return;
            
            var objtxtFOBRate = document.getElementById(ParentCtrlID + "_txtFOBRate"); //dGetValue(objRow[5].childNodes[0].value);
            var dMinRate = dGetValue(objtxtFOBRate.value);
                        
            var objtxtFreight = document.getElementById(ParentCtrlID + "_txtFreight");
            var dOrgUnitAmount = dGetValue(objtxtFreight.value);
            
           // To get Rate From database for this proforma
            var objtxtAmtDt = document.getElementById(ParentCtrlID + "_txtAmtDt");
            var dOrgDtUnitAmount = dGetValue(objtxtAmtDt.value);
            
            //To get Per Unit Amount
            dUnitAmount = objtxtUnitAmount.value;

            if (TextboxType == 2) {
                if (objtxtUserRoleID.value != "1" && dGetValue(dMinRate) > dGetValue(dUnitAmount)) {
                    //alert("Please Take A Approval From Head..!");
                    var objtxtbSentMail = document.getElementById(ParentCtrlID + "_txtbSentMail");
                    var response1 = confirm('Do You Want Sent Mail To Head?');
                    if (response1) {
                        objtxtbSentMail.value = "Y";
                    }
                    else {
                        objtxtbSentMail.value = "N";
                    }     
                    //objtxtUnitAmount.value = RoundupValue(dOrgUnitAmount);
                    objtxtUnitAmount.value = RoundupValue(dOrgDtUnitAmount);
                    dUnitAmount = objtxtUnitAmount.value;
                    objlblHeadUserIDDisc.value = "0";
                }
                else if (objtxtUserRoleID.value == "1" && dOrgDtUnitAmount != dUnitAmount) {
                    objlblHeadUserIDDisc.value = "1";
                }
                else {
                    objlblHeadUserIDDisc.value = "0";
                }
                if (dGetValue(dUnitAmount) == 0) {
                    alert("Please Enter the Rate..!");
                    //objtxtUnitAmount.value = RoundupValue(dOrgUnitAmount);
                    objtxtUnitAmount.value = RoundupValue(dOrgDtUnitAmount);
                    dUnitAmount = objtxtUnitAmount.value;
                    objlblHeadUserIDDisc.value = "0";
                }
            }
            //To Get Object of Line Total
            objtxtLineTotalAmt = objRow[14].childNodes[0];
            
            //Original Total
            dOrgLineAmount = dGetValue(objtxtLineTotalAmt.value);

            // Amount based on Original Rate
            //var dOrgLineAmtBsOnOrgRate = (dGetValue(Qty) * dGetValue(dOrgUnitAmount));
            
            // Amount based on Original Min Rate
            var dOrgLineAmtBsOnOrgRate = (dGetValue(Qty) * dGetValue(dMinRate));
            
            //Changed Rate Total
            dLineTotal = (dGetValue(Qty) * dGetValue(dUnitAmount));           
            
            // To Calculate Diff
            dDiffofAmount = (dLineTotal - dOrgLineAmount);
            
            //To get Discount
            if (dOrgLineAmtBsOnOrgRate == dLineTotal) {
                dDiscount = 0;
            }
            else {
                //if (dGetValue(dMinRate - dUnitAmount) <= 0) {
                if (dGetValue(dOrgLineAmtBsOnOrgRate - dLineTotal) <= 0) {
                    dDiscount = 0;
                }
                else {
                    //dDiscount = (dMinRate - dUnitAmount);
                    dDiscount = (dOrgLineAmtBsOnOrgRate - dLineTotal);
                } 
            }            
            // Set Discount and change Rate Total
            //objRow[12].childNodes[0].value = RoundupValue(dDiscount);            
            if (dDiscount > 0 && dLineTotal != dGetValue(objtxtLineTotalAmt.value)) {
                objhdbEditDisc.value = "Y"
            } else {
                objhdbEditDisc.value = "N"
            }
            
            objRow[13].childNodes[0].value = RoundupValue(dDiscount);
            objRow[14].childNodes[0].value = RoundupValue(dLineTotal);
            
            dUnitAmount = dGetValue(objtxtUnitAmount.value);
//            if (dDiscount >= dUnitAmount) {
//                alert("Discount is greater than or equal to the Unit Amount,Please reentered the discount !");
//                objRow[12].childNodes[0].value = 0.00;
//                dDiscount = 0;
//            }
//            objtxtUnitAmount.value = RoundupValue(dUnitAmount);


            // To get Object of Heade Total 
            ContainerName = GetContainerName();
            //objtxtTotalAmt = (document.getElementById(ContainerName + txtTotalAmtID));            
            objtxtTotalAmt = document.getElementById('ContentPlaceHolder1_txtTotalAmt');

            // To get Object of Grand Total
            objtxtGrandTotalAmt = document.getElementById('ContentPlaceHolder1_txtGrandTotalAmt');

            // To Calculate Grand Total
            if (dGetValue(objtxtGrandTotalAmt.value) != "0") {
                objtxtTotalAmt.value = RoundupValue(dGetValue(objtxtTotalAmt.value) + dDiffofAmount);
                objtxtGrandTotalAmt.value = RoundupValue(dGetValue(objtxtGrandTotalAmt.value) + dDiffofAmount);
            }
            else {
                objtxtTotalAmt.value = RoundupValue(objtxtLineTotalAmt.value);
                objtxtGrandTotalAmt.value = RoundupValue(objtxtTotalAmt.value);
            }
        }
    }
}


function ReduceAmountFromTotal(ObjControl) {
    //var objRow = ObjControl.parentNode.parentNode.childNodes;
    var ObjLineAmount = ObjControl.parentNode.parentNode.parentNode.childNodes[14].children[0];//Get Total
    var ObjTotalAmount = document.getElementById('ContentPlaceHolder1_txtTotalAmt'); ;
    var ObjGrandAmount = document.getElementById('ContentPlaceHolder1_txtGrandTotalAmt'); ;
    var dLineAmount = 0;
    dLineAmount = dGetValue(ObjLineAmount.value);

    if (ObjControl.checked == false) {
        // To Calculate Grand Total
        ObjTotalAmount.value = RoundupValue(dGetValue(ObjTotalAmount.value) - dLineAmount);
        ObjGrandAmount.value = RoundupValue(dGetValue(ObjGrandAmount.value) - dLineAmount);
    }
    else {
        // To Calculate Grand Total
        ObjTotalAmount.value = RoundupValue(dGetValue(ObjTotalAmount.value) + dLineAmount);
        ObjGrandAmount.value = RoundupValue(dGetValue(ObjGrandAmount.value) + dLineAmount);
    }
    
}
// to Set visible value on Proforma Accept /reject  change
function SetValueOnProformaSelectchange()
{
    var DropdownList=document.getElementById('ContentPlaceHolder1_drpSelect');
    var SelectedIndex=DropdownList.selectedIndex;
    var SelectedValue=DropdownList.value;
    var SelectedText=DropdownList.options[DropdownList.selectedIndex].text;

    if(SelectedText == "Accept")
    {   
        document.getElementById('ContentPlaceHolder1_btnAccept').style.display='';
        document.getElementById('ContentPlaceHolder1_btnReject').style.display='none';
        document.getElementById('ContentPlaceHolder1_lblRejectioresion').style.display='none';
        document.getElementById('ContentPlaceHolder1_drpReasonSelection').style.display='none';
    }     
    else if(SelectedText == "Reject")
    {
        document.getElementById('ContentPlaceHolder1_btnReject').style.display='';
        document.getElementById('ContentPlaceHolder1_btnAccept').style.display='none';
        document.getElementById('ContentPlaceHolder1_lblRejectioresion').style.display='';
        document.getElementById('ContentPlaceHolder1_drpReasonSelection').style.display='';
    }
}

// To Check Proforma Type Is Selected
function CheckProformaTypeIsSelected() {
    var objProformaTypeCombo = document.getElementById('ContentPlaceHolder1_drpProformaSelection');
    if (objProformaTypeCombo == null) return;

    if (objProformaTypeCombo.selectedIndex == 0) {
        alert('Please first select the Proforma type !');
        return false;
    }
    else {
        return true;
    }
}

// To Check Proforma Type Is Selected
function CheckProformaReasonIsSelected() {
    var objProformaTypeCombo = document.getElementById('ContentPlaceHolder1_drpReasonSelection');
    if (objProformaTypeCombo == null) return;

    if (objProformaTypeCombo.selectedIndex == 0) {
        alert('Please first select the rejection reason !');
        return false;
    }
    else {
        return true;
    }
}

//#############################Spares(Part) Proforma function
// To Add Value To  Total Amount Of Parts
function AddValueToPartTotal(event, ObjControl) 
{
    if (CheckTextboxValueForNumeric(event, ObjControl, false) == false) {
        return false;
    }
    else {
        CalculatePartTotal(event, ObjControl)
        return true;
    }
}
// To Calculate Part Total Amount
function CalculatePartTotal(event, ObjControl) 
{
    var objRow = ObjControl.parentNode.parentNode.childNodes;
    var dBaseAmount = 0;
    var dTotal = 0;
    var objtxtGrandTotalAmt;
    var dValue = 0;
    var dDiscount = 0;

    dBaseAmount = dGetValue(objRow[1].childNodes[0].value);  // To get base Amount 
    dTotal = dBaseAmount;
    for (i = 2; i <= 7; i++)// started from 2 i.e from Freight
    {
        ObjControl = objRow[i].childNodes[0];
        dValue = dGetValue(ObjControl.value);

        dTotal = dTotal + dGetValue(dValue);
    }
    // To Get Discount
    var dDiscountPer = dGetValue(objRow[8].childNodes[0].value);
    dDiscount = dGetValue((dBaseAmount * dDiscountPer) / 100);
    if (dDiscountPer > 100) {
        alert("Discount Percentage is greater than 100%, Please reentered the discount !");
        objRow[8].childNodes[0].value = 0.00;
        dDiscount = 0;
    }    
    else if (dDiscount >= dTotal) 
    {
        alert("Discount is greater than or equal to the Total,Please reentered the discount !");
        objRow[8].childNodes[0].value = 0.00;
        dDiscount = 0;
    }
    dTotal = dTotal - dDiscount;

    // To Set Total
    objRow[9].childNodes[0].value = RoundupValue(dTotal);

    // To get Object of Grand Total         
    objtxtGrandTotalAmt = document.getElementById("ContentPlaceHolder1_txtGrandTotalAmt");
    objtxtGrandTotalAmt.value = RoundupValue(dTotal);


}

function ToShowPriceBreakup(ObjControl,obj) {

    var sSelecedValue = ObjControl.options[ObjControl.selectedIndex].text;    
    
    var ObjGrid = null;
    var ObjDicount = null;

    ObjGrid = window.document.getElementById("ContentPlaceHolder1_" + obj);
    ObjDicount = window.document.getElementById("ContentPlaceHolder1_" + obj);
    if (ObjGrid == null) return;
    var ObjCell;
    for (i = 0; i < ObjGrid.rows.length; i++) //Rows
    {
        for (j = 6; j < 12; j++) //Column
        {
            ObjCell = ObjGrid.rows[i].cells[j];
            ObjDicount = ObjGrid.rows[0].cells[j];
            if (sSelecedValue == "Y") {
                if (ObjCell != null) 
                {
                    if (ObjCell.title != "9999" && ObjDicount.innerHTML != "Discount Amount")    
                    ObjCell.style.display = '';
                }
            }
            else if (sSelecedValue == "N") {
            if (ObjCell != null) {
                if (ObjCell.style.display == "none" && ObjDicount.innerHTML != "Discount Amount")
                    ObjCell.title = "9999";
                else {
                    if (ObjDicount.innerHTML != "Discount Amount")
                        ObjCell.style.display = 'none';
                }
            }
            }
        } //End Col
    }//end rows
}
function ToShowDiscount(ObjControl, obj) {

    var sSelecedValue = ObjControl.options[ObjControl.selectedIndex].text;

    var ObjGrid = null;

    ObjGrid = window.document.getElementById("ContentPlaceHolder1_" + obj);
    if (ObjGrid == null) return;
    var ObjCell;
    for (i = 0; i < ObjGrid.rows.length; i++) //Rows
    {
        for (j = 6; j < 13; j++) //Column
        {
            ObjCell = ObjGrid.rows[0].cells[j];
            if (sSelecedValue == "Y") {
                if (ObjCell != null) {

                    if (ObjCell.innerHTML == "Discount Amount") {
                        ObjCell = ObjGrid.rows[i].cells[j];
                        ObjCell.style.display = '';
                    }
                }
            }
            else if (sSelecedValue == "N") {
                if (ObjCell != null) {

                    if (ObjCell.innerHTML == "Discount Amount") {
                        ObjCell = ObjGrid.rows[i].cells[j];
                        ObjCell.style.display = 'none';
                    }
                }
            }
        } //End Col
       } //end rows

   

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

function CheckShipmentDateAndSetOtherDate() 
{ 

   var sShipmentDate="";
   var ObjShipmentDate = window.document.getElementById("ContentPlaceHolder1_txtShipmentDate_txtDocDate");
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
    var sTmpDate =new Date(year, month, day);
    var TmpDay = 0;    
    
    if (sShipmentDate == '' ) 
    {
        return false;
    }   
    if(dtCurDate>sTmpDate)
     {
        alert ("Shipment Date should be greter or equal to Current Date")
        ObjShipmentDate.value="";
        ObjShipmentDate.focus();
        return false;
    }   
    
    var ObjLastDateNegotiation=window.document.getElementById("ContentPlaceHolder1_txtLastDateNegotiation_txtDocDate");
    var ObjValidityDate=window.document.getElementById("ContentPlaceHolder1_txtValidityDate_txtDocDate");

   //sTmpDate.setFullYear(year, month, day);
    
    var dtt=new Date()
    dtt.setDate(10);
    var a=dtt;
    
    // Last Date Of Negotiation =Shipment Date + 20
    var sTmpDate2 = new Date(year, month, day)    
    sTmpDate2.setDate(sTmpDate2.getDate() + 20)    
    ObjLastDateNegotiation.value=sTmpDate2.format("dd/MM/yyyy");
  
    // Validity Date =Shipment Date + 30
    sTmpDate2= new Date(year, month, day)
    sTmpDate2.setDate(sTmpDate2.getDate() + 30)
    ObjValidityDate.value = sTmpDate2.format("dd/MM/yyyy")
}
//sujata 25122010
function ShowProforma(iDocId, Url) {
    
    var sReportName = "";
    sReportName = "/rptProformaInvoice&";    
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


//sujata 25122010
function ShowSpProforma(iDocId, Url) {

    var sReportName = "";
    sReportName = "/rptProformaSprInvoice&";
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