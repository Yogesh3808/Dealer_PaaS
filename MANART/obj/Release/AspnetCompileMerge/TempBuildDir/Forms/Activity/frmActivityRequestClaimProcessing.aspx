<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmActivityRequestClaimProcessing.aspx.cs" 
    Inherits="MANART.Forms.Activity.frmActivityRequestClaimProcessing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
   <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/ScrollableGridPlugin_ASP.NetAJAX_2.0.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
     <script src="../../Scripts/jsActivity.js"></script>
     <script src="../../Scripts/jsActivityRequestClaimFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
     <script src="../../Scripts/jsFileAttach.js"></script>
    
    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsGCRfunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsWarrantyFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsActivityRequestClaimFunction.js" type="text/javascript"></script>

    <script type="text/javascript">
        window.onload
        {
            AtPageLoad();
        }
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            HideControl();

            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }
        function refresh() {
            if (116 == event.keyCode) {
                //event.keyCode = 0; 
                //event.returnValue=false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }


        function BackClose() {
            window.close();
        }
      

    </script>
      <script type="text/javascript">
          function ShowAttachDocumentProcessing1(objFileControl) {


              var sFileName = '';

              popht = 3; // popup height
              popwth = 3; // popup width


              // sFileName = (objRow[5].children[0].innerText);
              //    sFileName = objFileControl.children[0].innerText;
              sFileName = objFileControl.innerText;
              var scrleft = (screen.width / 2) - (popwth / 2) - 80; //centres horizontal
              var scrtop = ((screen.height / 2) - (popht / 2)) - 40; //centres vertical
              //window.open("/DCS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
              window.open("/DCS/Forms/Activity/frmActivityOpendocument.aspx?FileName=" + escape(sFileName), "List", "top=" + scrtop + ",left=" + scrleft + ",width=1px,height=3px");


          }
          // When user Click to delete the record then reduce the amount 
          function SelectDeleteCheckboxCommon(ObjChkDelete) {

              if (ObjChkDelete.checked) {
                  if (confirm("Are you sure you want to delete this record?") == true) {
                      return true;
                  }
                  else {
                      ObjChkDelete.checked = false;
                      return false;
                  }
              }
              else {
                  ObjChkDelete.parentNode.parentNode.parentNode.style.backgroundColor = 'white';
              }
          }
</script>

 <script type="text/javascript">
     
     function CheckActClmProValidation_Exception(obj) {
         // debugger;

         var txtclaimFlage = document.getElementById("txtclaimFlage");
         var hdnRoleID = document.getElementById("hdnRoleID");
         var Status = obj;

         if (obj == "Approve")
             obj = "Process"
         else
             obj = "Reject"


         if (hdnRoleID.value == "9" && txtclaimFlage.value == 'Processing' && Status == 'Approve')

             if (ActivityClaimDtlsValidation_Exception() == false) {
                 return false;
             }
         if (CheckRemarkIsEnter() == false) return false;
         if (confirm("Are you sure, you want to " + obj + " Activity Claim?") == true) {
             //alert("Succeefully Claimed Activity ");
             return true;
         }
         else {
             //alert("Does Not Claimed Activity ");
             return false;
         }
     }


</script>

    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
         
                <table id="PageTbl" class="PageTable" border="1">
                    <tr id="TitleOfPage">
                        <td class="PageTitle" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">
                            <asp:Panel ID="LocationDetails" runat="server">
                                <uc2:Location ID="Location" Visible="false" runat="server" />
                            </asp:Panel>
                           <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Activity Details" ExpandedText="Activity Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Activity Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDocDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                          <table id="txtDocNoDetails" runat="server"  class="ContainTable table table-bordered">
                        
                        <tr>
                             <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="lblActivityName" runat="server" Text="Name of Activity:"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                
                                  

                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="TextBoxForString" Font-Bold="true"
                                    ></asp:TextBox>
                            </td>                           
                             <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="lblRefClaimNo" runat="server" Text="Type Of Activity:"></asp:Label>
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtTypeOfActivity" runat="server" CssClass="TextBoxForString"  Font-Bold="true"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                             <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label2" runat="server" Text="Activity Date From:"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="TextBoxForString" Font-Bold="true" Enabled ="false" ></asp:TextBox>
                          
                            </td>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label4" runat="server" Text=" Activity Date To:"></asp:Label>
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="TextBoxForString" Font-Bold="true" Enabled ="false"></asp:TextBox>
                            </td>
                            
                            
                           
                        </tr>
                        <tr>
                             <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label7" runat="server" Text="Dealer Activity Date From :"></asp:Label>
                               
                            </td>
                            <td style="width: 18%">
                                <%--<asp:TextBox ID="txtDlrActDtFrm" runat="server" CssClass="TextBoxForString" Font-Bold="true"></asp:TextBox>--%>
                                <uc3:CurrentDate ID="txtDlrActDtFrm" runat="server" bCheckforCurrentDate="false" Mandatory="true" />
                                
                            </td>
                             <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label9" runat="server" Text="Dealer Activity Date To :"></asp:Label>
                               
                            </td>
                             <td style="width: 18%">
                                <%--<asp:TextBox ID="txtDlrActDtTo" runat="server" CssClass="TextBoxForString" Font-Bold="true"></asp:TextBox>--%>
                                <uc3:CurrentDate ID="txtDlrActDtTo" runat="server" bCheckforCurrentDate="false" Mandatory="true" />
                             
                            </td>
                            </tr>
                        <tr>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label17" runat="server" Text="Cost Center :"></asp:Label>
                                
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtCostCenter" runat="server" CssClass="TextBoxForString" Font-Bold="true"></asp:TextBox>
                              
                            </td>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label18" runat="server" Text="GL Account :"></asp:Label>
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtGLAccount" runat="server" CssClass="TextBoxForString" Font-Bold="true"></asp:TextBox>
                            </td>
                           
                            
                        </tr>
                            <tr runat ="server" id="trclaim">
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                
                                <asp:Label ID="lblActivityclaimNo" runat="server" Text="Activity Claim No:"></asp:Label>

                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtActivityClaimNo" runat="server" CssClass="TextBoxForString" 
                                    Font-Bold="true"></asp:TextBox>
                            </td>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="lblActivityclaimDate" runat="server" Text="Activity Claim Date:"></asp:Label>
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtActivityClaimDate" runat="server" CssClass="TextBoxForString"
                                    Font-Bold="true" Enabled ="false"></asp:TextBox>
                            </td>
                           
                            
                           
                        </tr>
                        <tr>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                Activity Request No:
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtActivityReqNo" runat="server" CssClass="TextBoxForString" 
                                    Font-Bold="true"></asp:TextBox>
                            </td>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="lblClaimDate" runat="server" Text="Activity Request Date"></asp:Label>
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtActivityReqDate" runat="server" CssClass="TextBoxForString"
                                    Font-Bold="true" Enabled ="false"></asp:TextBox>
                            </td>
                           
                            
                           
                        </tr>
                        <tr>
                       <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                               
                                <asp:Label ID="Label15" runat="server" Text="Objective:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObjective" runat="server" CssClass="MultilineTextbox"  Font-Bold="true" Width="391px" Height="40px"></asp:TextBox>
                                
                            </td>
                           <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label3" runat="server" Text="Comments:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtComments" runat="server" CssClass="MultilineTextbox" 
                                    Font-Bold="true"  Width="391px" Height="40px" ></asp:TextBox>
                            </td>
                        
                        
                        </tr>
                            <tr runat ="server" id="trInvdetails">
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                
                                <asp:Label ID="Label6" runat="server" Text="Invoice No:"></asp:Label>

                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="TextBoxForString" 
                                    Font-Bold="true" Enabled ="false"></asp:TextBox>
                            </td>
                              <td style="width: 10%; padding-left: 10px" class="tdLabel" >
                                <asp:Label ID="Label8" runat="server" Text="Invoice Date:"></asp:Label>
                            </td>
                             <td style="width: 18%">
                                <asp:TextBox ID="txtInvoicedate" runat="server" CssClass="TextBoxForString"
                                    Font-Bold="true" Enabled ="false"></asp:TextBox>
                            </td>
                           
                            
                           
                        </tr>
                      
                    </table>
                  <table id="Table7" runat="server" class="ContainTable table table-bordered">
                       
                                    <tr>
                                            <td style="width: 10%; padding-left: 10px"  class="tdLabel" >Total Budget Available:</td>
                                            <td style="width: 15%" >
                                                <asp:TextBox ID="txtTotalBudgetAvailable" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                            <td style="width: 10%; padding-left: 10px" class="tdLabel">Budget Utilized :</td>
                                            <td style="width: 15%" >
                                                <asp:TextBox ID="txtBudgetUtilized" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                            <td style="width: 10%; padding-left: 10px" class="tdLabel">Pending Budget :</td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtPendingBudget" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>

                                        </tr>
                         
                                </table>
                               <table id="Table8" runat="server" class="ContainTable table table-bordered">
                                 <tr>
                                            <td style="width: 10%; padding-left: 10px">Expected No. Customers/Drivers:</td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtExpectedNoCustomers" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                            <td style="width: 10%; padding-left: 10px">Expected No. of Vehicles :</td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtExpectedNoofVehicles" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                   </tr>
                                 <tr>
                                           <td style="width: 10%; padding-left: 10px">Expected Parts Business [INR]:</td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtExpectedPartsBusiness" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                            <td style="width: 10%; padding-left: 10px">Expected Service Revenue [INR]:</td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtExpectedServiceRevenue" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                        </tr>
                                        <tr>
                                           <td style="width: 10%; padding-left: 10px">Expected Lube/Coolant/ Nox Doser Business [LTRS]:</td>
                                            <td style="width: 15%">
                                                <asp:TextBox ID="txtExpectedLube" runat="server" CssClass="TextBoxForString" 
                                                    Text=""></asp:TextBox> 
                                            </td>
                                            
                                        </tr>
                    </table>


                    </asp:Panel>
                </asp:Panel>


               
                
                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Width ="100%">
                    
                           
                   <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="True"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Activity Expenses Head Details" ExpandedText="Hide Activity Expenses Head Details"
                        TextLabelID="lblTtlPartDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Activity Expenses Head Details"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="GridActivityClaimDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                            Width="100%">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1%>' />
                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Expected Exp Head" ItemStyle-Width="10%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="txtExpectedExpHead" CssClass="GridTextBoxForString" runat="server" Width ="96%"
                                            Text='<%# Eval("Expense_Head") %>' onblur="return CheckExpHeadAlreadyUsedInGrid(event,this);"></asp:TextBox>--%>

                                        <%--<asp:DropDownList ID="drpExpectedExpHead" runat="server" CssClass="GridComboBoxFixedSize"  ReadOnly="true"
                                            
                                                        Width="90%" > 
                                                    </asp:DropDownList>--%>
                                        <%-- onblur="return CheckActivityHeadSelected(event,this);"--%>

                                    <%--</ItemTemplate>                                    
                                </asp:TemplateField>--%>
                                  <asp:TemplateField HeaderText="Actual Exp Head" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                       <%-- <asp:TextBox ID="txtActualExpHead" CssClass="GridTextBoxForString" runat="server" Width ="96%"
                                            Text='<%# Eval("Actual_Head") %>' onblur="return CheckExpHeadAlreadyUsedInGrid(event,this);"></asp:TextBox>--%>

                                         <asp:DropDownList ID="drpActualExpHead" runat="server" CssClass="GridComboBoxFixedSize" ReadOnly="true"
                                                        Width="90%"  >
                                                    </asp:DropDownList>
                                        <%--onblur="return CheckActivityHeadSelected(event,this);"> --%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        Total :
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tentative Amount" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTentativeAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Tentative_Amount", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);" ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtTotalTentativeAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Total_Tentative_Amount", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);"  ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Actual Amount" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtActualAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Actual_Amount", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);" ></asp:TextBox>
                                    </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:TextBox ID="txtTotalActualAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Total_Actual_Amount", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Propose MTI % Share" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtVECVShare" CssClass="GridTextBoxForAmount" runat="server" Text='<%# Eval("VECV_Shr_Per")%>' Width ="96%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Propose MTI Amount" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtVeCVShareAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                            Text='<%# Eval("VECV_Shr_Amt", "{0:#0.00}")%>' ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtVeCVShareTotAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Total_VECV_Shr_Amt", "{0:#0.00}")%>'  ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Propose Dealer % Share" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDealShare" CssClass="GridTextBoxForAmount" runat="server" Text='<%# Eval("Dealer_Shr_Per")%>'
                                             Width ="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);" ></asp:TextBox>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Propose Dealer Amount" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDealShareAmt" ReadOnly="true" CssClass="GridTextBoxForAmount" runat="server" Width ="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                           ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtDealShareTotAmt" ReadOnly="true"  CssClass="GridTextBoxForAmount" runat="server" Width ="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                            ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                
                                   <asp:TemplateField HeaderText="Appr MTI % Share" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtApprVECVShare" CssClass="GridTextBoxForAmount" runat="server" Text='<%# Eval("Apprv_VECV_Per")%>' Width ="96%"
                                          onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);" onblur="return CalculateLineTotal(event,this,'ActProcess');" ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                       Total :
                                         
                                        
                                        
                                    </FooterTemplate>
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Appr MTI Amount" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtApprVeCVShareAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Apprv_VECV_Amt", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtApprVeCVShareTotAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("TotalAppr_VECV_Amt", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                  
                                            
                                            <asp:TextBox ID="txtApprVeCVShareFinalTotAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%" Visible="false"
                                            Text='<%# Eval("TotalAppr_VECV_Amt", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                   
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Appr Dealer % Share" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtApprDealShare" CssClass="GridTextBoxForAmount" runat="server" Text='<%# Eval("Apprv_Dealer_Per")%>'
                                            Width ="96%" onkeydown="return ToSetKeyPressValueFalse(event,this);"  ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        Total :
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Appr Dealer Amount" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtApprDealShareAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("Apprv_Dealer_Amt", "{0:#0.00}")%>'  onkeydown="return ToSetKeyPressValueFalse(event,this);" ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtApprDealShareTotAmt" CssClass="GridTextBoxForAmount" runat="server" Width ="96%"
                                            Text='<%# Eval("TotalAppr_Dealer_Amt", "{0:#0.00}")%>' onkeydown="return ToSetKeyPressValueFalse(event,this);" ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                
                                
                                
                                
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>                    
                </asp:Panel>
 <asp:Panel ID="Panel2" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Width ="100%">
                    
                           
                   <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel1"
                        ExpandControlID="Panel3" CollapseControlID="Panel3" Collapsed="True"
                        ImageControlID="Image1" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show GST Details" ExpandedText="Hide GST Details"
                        TextLabelID="Label1">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel3" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                    <asp:Label ID="Label1" runat="server" Text="GST Details"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                   <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
                          <table id="Table6" runat="server" class="ContainTable" >
                  
                   <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="6"  >
                             Tax Details
                            </td>
                        </tr>
                        <tr>
                            <td >
                              
                               <asp:Label ID="Label5" Text ="Appr MTI Amount" runat ="server" Font-Bold="true" > </asp:Label>
                            </td>
                            <td >
                              
                               <asp:Label ID="Label20" Text ="Deduction Amount" runat ="server" Font-Bold="true"  style="display:none"> </asp:Label>
                            </td>
                            <td >
                              
                               <asp:Label ID="Label21" Text ="Appr MTI Amount - Deduction" runat ="server" Font-Bold="true"  style="display:none"> </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIGST_SGST" Text ="IGST/SGST" runat ="server" Font-Bold="true" > </asp:Label>
                            </td>
                              <td  >
                            <asp:Label ID="lblCGST" Text ="CGST" runat ="server"  Font-Bold="true" > </asp:Label>
                            </td>
                            <td  >
                             <asp:Label ID="Label14" Text ="Total Appr MTI Amount" runat ="server" Font-Bold="true" > </asp:Label>
                            </td>
                             </tr>
                             <tr>
                             <td><asp:TextBox ID="txtApprVeCVShareTotAmt_GST"  runat="server"  Width ="96%"
                                             onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox></td>
                                             <td><asp:TextBox ID="txtDeductionAmount_GST"  runat="server"  Width ="96%"
                                             onkeydown="return ToSetKeyPressValueFalse(event,this);"  style="display:none"></asp:TextBox></td>
                                             <td><asp:TextBox ID="txtApprVeCVShareTotAmt_GSTWithDeduction"  runat="server"  Width ="96%"
                                             onkeydown="return ToSetKeyPressValueFalse(event,this);" style="display:none" ></asp:TextBox></td>
                                             
                                            <td><asp:TextBox ID="txtIGST_SGST_GST"  runat="server"  Width ="96%"
                                             onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox></td>
                                            <td><asp:TextBox ID="txtCGST_GST"  runat="server" Width ="96%"
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox></td>
                                            <td>
                                            <asp:TextBox ID="txtApprVeCVShareFinalTotAmt_GST"  runat="server"  Width ="96%"
                                             onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox></td>
                             </tr>
                  </table>
                 </asp:Panel> 
 </asp:Panel>
                <asp:Panel ID="PMerchantDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Width ="100%">
                    
                           
                   <cc1:CollapsiblePanelExtender ID="CPEMerchantDetails" runat="server" TargetControlID="CntMerchantDetails"
                        ExpandControlID="TtlMerchantDetails" CollapseControlID="TtlMerchantDetails" Collapsed="True"
                        ImageControlID="ImgTtlMerchantDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Merchandize Requirement Details" ExpandedText="Hide Merchandize Requirement Details"
                        TextLabelID="lblTtlMerchantDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlMerchantDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                    <asp:Label ID="lblTtlMerchantDetails" runat="server" Text="Merchandize Requirement Details"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlMerchantDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntMerchantDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="GridActivityMerchantDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                            Width="75%" >
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No." HeaderStyle-Width ="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1%>' />
                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpMerchandizeReq" runat="server" CssClass="GridComboBoxFixedSize" 
                                            
                                                        Width="90%" > 
                                                    </asp:DropDownList>
                                        <%-- onblur="return CheckActivityHeadSelected(event,this);"--%>

                                    </ItemTemplate> 
                                         <ItemStyle Width="30%" />                              
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Qty" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Qty", "{0:#0}")%>' MaxLength="4"
                                                Width="90%"  onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);"  ></asp:TextBox>
                                        </ItemTemplate>
                                      <%--onkeypress=" return CheckForTextBoxValue(event,this,'5');"--%>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                 
                                
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>                    
                </asp:Panel>

            
                
             <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEFileAttchDetails" runat="server" TargetControlID="CntFileAttchDetails"
                            ExpandControlID="TtlFileAttchDetails" CollapseControlID="TtlFileAttchDetails"
                            Collapsed="false" ImageControlID="ImgTtlFileAttchDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Attached Documents"
                            ExpandedText="Attached Documents" TextLabelID="lblTtlFileAttchDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlFileAttchDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="82%">
                                        <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <%--<td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblFileAttachRecCnt" runat="server" Text="0"></asp:Label>
                                </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlFileAttchDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            Style="display: none;">
                            <table id="Table2" runat="server" class="ContainTable">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                            AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                            GridLines="Horizontal" OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                            SkinID="NormalGrid" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1%>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="1%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                    ItemStyle-CssClass="HideControl">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFileAttchID" runat="server" Text='<%# Eval("ID")%>' Width="1"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User File Description" ItemStyle-Width="50%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description")%>'
                                                            Width="96%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name" ItemStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                            onmouseover="SetCancelStyleonMouseOver(this);" onClick="return ShowAttachDocumentProcessing(this);"
                                                            Text='<%# Eval("File_Names")%>' ToolTip="Click Here To Open The File" Width="90%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);"
                                                            Text="Delete" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle Wrap="True" />
                                            <EditRowStyle BorderColor="Black" Wrap="True" />
                                            <AlternatingRowStyle Wrap="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 50%" align="center">User File Description
                                    </td>
                                    <td class="tdLabel" style="width: 50%" align="center">File Name
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="tdLabel">
                                        <div id="upload1">
                                            <input id="Text1" type="text" name="Text1" class="TextBoxForString" style="width: 50%" placeholder="User File Description" />
                                            <input id="AttachFile" type="file" runat="server" style="width: 45%" class="Cntrl1"
                                                onblur="return addFileUploadBox(this);" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>

 <asp:Panel ID="pnlRemark" runat="server" BorderColor="Black" BorderStyle="Double">
                                <table id="tblRemark" runat="server" class="ContainTable">
                                    <%--<tr>
                                        <td class="tdLabel">
                                           
                                            <asp:Label ID="lblCSMRemark" runat="server" Text="CSM Remark"></asp:Label>
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txt1stRemark" runat="server" TextMode="MultiLine" Width="400px"
                                                Height="30px" onkeyup="return ToCheckTextLength(this,150);"></asp:TextBox>
                                        </td>
                                        
                                    </tr>--%>
                                     <tr id="ASMRemark" runat ="server"  >
                                        <td class="tdLabel">
                                            ASM Remark
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtASMRemark" runat="server" TextMode="MultiLine" Width="400px"
                                                Height="30px" onkeyup="return ToCheckTextLength(this,150);"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                     <tr>
                                        <td class="tdLabel">
                                            RPM/RSM Remark
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtRSMRemark" runat="server" TextMode="MultiLine" Width="400px"
                                                Height="30px" onkeyup="return ToCheckTextLength(this,150);"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                     <tr>
                                        <td class="tdLabel">
                                            Parts Head Remark
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtHeadRemark" runat="server" TextMode="MultiLine" Width="400px"
                                                Height="30px" onkeyup="return ToCheckTextLength(this,150);"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                     <tr id="AftersaleRemark" runat ="server"  >
                                        <td class="tdLabel">
                                           After Sales Head Remark
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtAfterSalesHeadRemark" runat="server" TextMode="MultiLine" Width="400px"
                                                Height="30px" onkeyup="return ToCheckTextLength(this,150);"></asp:TextBox>
                                        </td>
                                        
                                    </tr>

                                </table>
                            </asp:Panel>

                        </td>
                    </tr>                    
                    <tr align="center">
                        <td>
                                               
                                
                    
                            <div id="DivBtn" runat="server">
                                &nbsp;&nbsp;
                                <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="CommandButton"
                                    OnClientClick="return CheckActClmProValidation('Approve');" OnClick="btnApprove_Click" />
                                &nbsp;&nbsp;
                                                                    
                            </div>
                              <div id="Div2" runat="server">
                                    <asp:Button ID="btnReturnclaimforModification" runat="server"
                                    Text="Return Request for Modification" CssClass="CommandButton" 
                                    OnClientClick="return  CheckActClmProValidation('Reject');"
                                    onclick="btnReturnclaimforModification_Click"  />
                            </div>
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="CommandButton" OnClientClick="BackClose();" />
                            &nbsp;&nbsp;
                        </td>
                        <tr align="center">
            <td>
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
                    </tr>
                    <tr id="TmpControl" style ="display:none">
                        <td style="width: 14%">
                             <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"  Width="96%"></asp:TextBox>
                            <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" ></asp:TextBox>
                            <asp:TextBox ID="txtControlCount" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtUserID" runat="server" Width="1px" Text=""></asp:TextBox>
                             <asp:TextBox ID="txtDeptID" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtUserDeptID" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtdealerid" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtPreviousDocId" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtclaimFlage" runat="server" Width="1px" Text="" CssClass="dispalynon"></asp:TextBox>
                            <asp:TextBox ID="txtclaimApprovedStatus" runat="server" Width="1px" Text="" CssClass="dispalynon"></asp:TextBox>
                             <asp:TextBox ID="txtpendingatfinance" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:HiddenField ID="hdnRejectcount" runat="server" Value ="0" />
                            <asp:HiddenField ID="hdnRoleID" runat="server" Value ="0" />
                             <asp:TextBox ID="txtIGST_SGST_id" runat="server"  Text="" CssClass="HideControl" ></asp:TextBox>
                <asp:TextBox ID="txtCGST_id" runat="server"  Text="" CssClass="HideControl"></asp:TextBox>
                <asp:TextBox ID="txtIGST_SGST_Per" runat="server"  Text="" CssClass="HideControl" ></asp:TextBox>
                <asp:TextBox ID="txtCGST_Per" runat="server"  Text="" CssClass="HideControl"></asp:TextBox>
                <asp:HiddenField ID="hdnClaimApprovedStatus" runat ="server" Value ="N"/>
                <asp:HiddenField ID="hdnClaimApprovedStatus_Final" runat ="server" Value ="N"/>
                        </td>
                    </tr>
                </table>
            
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
