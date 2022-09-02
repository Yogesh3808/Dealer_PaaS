<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmServiceVANCharges.aspx.cs" Inherits="MANART.Forms.Service.frmServiceVANCharges" %>
<%@ Register Src="~/WebParts/CurrentDateTime.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagPrefix="uc1" TagName="CurrentDate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">                 
        <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>

    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery-1.11.1.js"></script>  
    <script src="../../Scripts/jsWCFileAttach.js"></script>

    

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                event.returnValue = false;
                alert(message);
                return false;
            }
        }
        window.onload = function () {
            FirstTimeGridDisplay('');
        }

        $(document).ready(function () {
            //debugger;
            var txtDocDate = document.getElementById("txtDocDate_txtDocDate");
            var txtFailDate = document.getElementById("txtFailDate_txtDocDate");
            var txtVANOutDate = document.getElementById("txtVANOutDate_txtDocDate");
            
            $('#txtFailDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
            });

            $('#txtVANOutDate_txtDocDate').datepick({
               dateFormat: 'dd/mm/yyyy', minDate: (txtFailDate.value == '') ? '0d' : txtFailDate.value
            });

            //function customRange(dates) {
            //    if (this.id == '#ContentPlaceHolder1_txtFromDate_txtDocDate') {
            //        alert("inside");
            //        alert(this.id);
            //        $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
            //    }
            //    else {
            //        $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
            //    }
            //}


        });


        function CloseMe() {
            window.close();
        }

        function ReturnSrvVANIDValue() {
            //debugger;
            var ObjControl = window.document.getElementById("txtID");
            var sValue;
            sValue = ObjControl.value;
            window.returnValue = sValue;
            window.close();

        }
        function SetSrvVANTotal()
        {
            var ObjtxtTripRate;
            var ObjtxtDistKm;
            var ObjtxtTotal;
            var ObjtxtNoTrip;
            //debugger;
            ObjtxtTripRate = window.document.getElementById("txtTripRate"); // It is cumulative Kms
            ObjtxtDistKm = window.document.getElementById("txtDistKm");
            ObjtxtTotal = window.document.getElementById("txtTotal");
            ObjtxtNoTrip = window.document.getElementById("txtNoTrip");

            var dTotal = 0;
            var iNoOfTrip = 0;
            var dDistKm = 0;
            var dTripRate = 0;

            iNoOfTrip = dGetValue(ObjtxtNoTrip.value);
            dDistKm = dGetValue(ObjtxtDistKm.value);
            dTripRate = dGetValue(ObjtxtTripRate.value);

            dTotal = (dDistKm * 2) * iNoOfTrip * dTripRate;

            ObjtxtTotal.value = dTotal;
        }
        function CheckStartEndKm(event, Objcontrol) {
            var ObjtxtStKms;
            var ObjtxtEndKms;            
            //debugger;
            ObjtxtStKms = window.document.getElementById("txtStKms"); // It is cumulative Kms
            ObjtxtEndKms = window.document.getElementById("txtEndKms");           
           
            var dStKms = 0;
            var dEndKms = 0;

            dStKms = dGetValue(ObjtxtStKms.value);
            dEndKms = dGetValue(ObjtxtEndKms.value);

            if (dStKms >= dEndKms)
            {
                alert("End Kms should be greater than Start Kms.")
                Objcontrol.focus();
            }
        }
    </script>
         <base target="_self" />
                </head>
<body>
   <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">            
        </asp:ScriptManager>
    <table class="PageTable" border="1" width="100%">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 14%" colspan="4">
                <asp:Label ID="lblTitle" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 9%" >
                  <asp:Button ID="btnSave" runat="server" Text="Save Service VAN Details" CssClass="btn btn-search btn-sm" 
                   Width="120px" onclick="btnSave_Click"/>               
            </td>
             <td style="width: 9%; font-weight: 700;">                 
            </td>  
            <td class="tdLabel" style="width: 14%;" colspan="2">
                   <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="btn btn-search btn-sm"
                   OnClientClick="ReturnSrvVANIDValue();"
                   onclick="btnBack_Click"></asp:Button> 
             </td>                     
        </tr>
        <tr id="TblControl">
            <td style="width: 9%; font-weight: 700;">
                <asp:Label ID="lblFromPage" runat="server" Text="Jobcard No :"></asp:Label>
            </td>  
            <td class="tdLabel" style="width: 14%;">                     
                <asp:TextBox ID="txtJobNo" CssClass="TextBoxForString" runat="server" Width="96%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
            </td>
            <td style="width: 9%">
                Trip Rate :
            </td>  
            <td class="tdLabel" style="width: 14%;">
                 <asp:TextBox ID="txtTripRate" CssClass="TextBoxForString" runat="server" Width="96%"
                  Font-Bold="False"></asp:TextBox>   <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
             </td>                     
        </tr>
        <tr>                    
             <td style="width: 9%">Labour Type :</td>
            <td class="tdLabel" style="width: 14%;">
                 <asp:DropDownList ID="DrpLabType" runat="server" CssClass="GridComboBoxFixedSize" Width="96%">
                 </asp:DropDownList>
             </td>             
             <td style="width: 9%">To Location:</td>
            <td class="tdLabel" style="width: 14%;">
                <asp:TextBox ID="txtToLocation" CssClass="TextBoxForString" runat="server" Width="96%" MaxLength="50"
                 Font-Bold="False"></asp:TextBox>  </td>  
        </tr>         
          <tr>
            <td style="width: 9%">From Location:</td>
            <td class="tdLabel" style="width: 14%;">
                 <asp:TextBox ID="txtFrmLocation" CssClass="TextBoxForString" runat="server" Width="96%" MaxLength="50"
                 Font-Bold="False"></asp:TextBox>
             </td>           
             <td style="width: 9%">Service VAN:</td>
            <td class="tdLabel" style="width: 14%;">
                 <asp:DropDownList ID="DrpSrvVAN" runat="server" AutoPostBack="true" CssClass="GridComboBoxFixedSize" Width="96%" OnSelectedIndexChanged="DrpSrvVAN_SelectedIndexChanged">
                 </asp:DropDownList>       
             </td>             
        </tr> 
        <tr>
            <td style="width: 9%">Start Kms:</td>
            <td class="tdLabel" style="width: 14%;">
                 <asp:TextBox ID="txtStKms" CssClass="TextBoxForString" runat="server" Width="96%" MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');"
                 onBlur="return CheckStartEndKm();" Font-Bold="False"></asp:TextBox>
             </td>           
             <td style="width: 9%">End Kms:</td>
            <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtEndKms" CssClass="TextBoxForString" runat="server" Width="96%" MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');"
                    onBlur="return CheckStartEndKm();" Font-Bold="False"></asp:TextBox>    
             </td>             
        </tr>    
        <tr>
            <td style="width: 9%">Distance Travelled One Way(In Km):</td>
            <td class="tdLabel" style="width: 14%;">
                   <asp:TextBox ID="txtDistKm" CssClass="TextBoxForString" runat="server" Width="96%" MaxLength="5" onkeypress=" return CheckForTextBoxValue(event,this,'6');"
                    onBlur="return SetSrvVANTotal();"  Font-Bold="False"></asp:TextBox>
             </td>           
             <td style="width: 9%">Total.</td>
            <td class="tdLabel" style="width: 14%;">
                  <asp:TextBox ID="txtTotal" CssClass="TextBoxForString" runat="server" Width="96%"
                  onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
             </td>             
        </tr>    
        <tr>
            <td style="width: 9%">No. of Trips:</td>
            <td class="tdLabel" style="width: 14%;">
                   <asp:TextBox ID="txtNoTrip" CssClass="TextBoxForString" runat="server" Width="96%" MaxLength="2" onkeypress=" return CheckForTextBoxValue(event,this,'5');"
                      onBlur="return SetSrvVANTotal();"  Font-Bold="False"></asp:TextBox>
             </td>           
             <td style="width: 9%">VAN Out Time:</td>
            <td class="tdLabel" style="width: 14%;">
              <%--  <asp:TextBox ID="txtVANOutDate" CssClass="TextBoxForString" runat="server" Width="96%"
                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>--%>
                <uc3:CurrentDate ID="txtVANOutDate" runat="server" Mandatory="true" bTimeVisible="true"  bCheckforCurrentDate="true" />
             </td>             
        </tr>    
        <tr>
             <td style="width: 9%">Complaint Time:</td>
            <td  class="tdLabel" style="width: 14%;">
                <%--<asp:TextBox ID="txtFailDate" CssClass="TextBoxForString" runat="server" Width="96%"
                    onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>--%>
                <uc3:CurrentDate ID="txtFailDate" runat="server" Mandatory="true" bTimeVisible="true"  bCheckforCurrentDate="true" />
            </td>
            <td style="width: 9%">Mechanic:</td>
            <td class="tdLabel" style="width: 14%;">
                 <asp:DropDownList ID="drpPMechanic" runat="server" CssClass="GridComboBoxFixedSize" Width="96%">
                 </asp:DropDownList>
            </td>
        </tr>            
                
        <tr>
            <td colspan="4">
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>                
                <asp:TextBox ID="txtDealerID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                <asp:TextBox ID="txtDealerBrID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                <asp:TextBox ID="txtDlrCode" CssClass="HideControl" runat="server" Width="96%" Text=""></asp:TextBox>
                <asp:TextBox ID="txtJobConfirm" CssClass="HideControl" runat="server" Width="96%" Text=""></asp:TextBox>
                <asp:TextBox ID="txtJobTypeID" CssClass="HideControl" runat="server" Width="96%" Text=""></asp:TextBox>
                <asp:TextBox ID="txtWarrantyTag" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                  <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtAggregate" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                <uc3:CurrentDate ID="txtDocDate" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="true" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>
