<%@ Page Title="Receipt for PDI" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmDirectBillingReceipt.aspx.cs" 
    Inherits="MANART.Forms.VehiclePurchase.frmDirectBillingReceipt" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="~/WebParts/ExportLocation.ascx" TagName="Location" TagPrefix="uc2" %>--%>
    <%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/ExportLocation.ascx" TagPrefix="uc2" TagName="ExportLocation" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>

    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFPFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsSpareReceipt.js"></script>
     <script src="../../Scripts/jsVehicle.js"></script>
    <script src="../../Scripts/jsEGPSpareReceipt.js"></script>
     <script src="../../Scripts/jsFileAttach.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //var txtMReceiptDate = document.getElementById("ContentPlaceHolder1_txtMReceiptDate_txtDocDate");
            var txtDMSInvDate = document.getElementById("ContentPlaceHolder1_txtDMSInvDate_txtDocDate");

            //$('#ContentPlaceHolder1_txtMReceiptDate_txtDocDate').datepick({
            //    dateFormat: 'dd/mm/yyyy', minDate: (txtMReceiptDate.value == '') ? '0d' : txtMReceiptDate.value, maxDate: (txtMReceiptDate.value == '') ? '0d' : txtMReceiptDate.value
            //});
            $('#ContentPlaceHolder1_txtDMSInvDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', maxDate: (txtDMSInvDate.value == '') ? '0d' : txtDMSInvDate.value
            });
        });
    </script>

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
        window.onload
        {
            AtPageLoad();
        }


        //To Show Part Master        
        function ShowMultiPartSearch(objNewPartLabel, sDealerId, IsDistributor, Is_AutoReceipt, sSupplierId) {
            var PartDetailsValue;
            var sSelectedPartID = GetPreviousSelectedPartID(objNewPartLabel);
            //if ((IsDistributor == "Y" || IsDistributor=="N") && Is_WithPO == "N" && Is_AutoReceipt == "N")
            if ((IsDistributor == "Y" || IsDistributor == "N") && Is_AutoReceipt == "N") {
                // Changed By Vikram on Date 20.06.2016
                alert(sSupplierId);
                PartDetailsValue = window.showModalDialog("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + IsDistributor + "&SupplierID=" + sSupplierId + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=MRPartDetails&TransFrom=Common", "List", "dialogHeight: 400px; dialogWidth: 700px;");
                //PartDetailsValue = window.showModalDialog("frmSelectMultiPartSearch.aspx?DealerID=" + sDealerId + "&IsDistributor=" + IsDistributor + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=MRPartDetails&TransFrom=Common", "List", "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
            }
            else {
                PartDetailsValue = window.showModalDialog("frmMReceiptPOSel.aspx?DealerID=" + sDealerId + "&IsDistributor=" + IsDistributor + "&SupplierID=" + sSupplierId + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=MRPartDetails&TransFrom=Common", "List", "dialogHeight: 500px; dialogWidth: 700px;");
                //PartDetailsValue = window.showModalDialog("frmMReceiptPOSel.aspx?DealerID=" + sDealerId + "&IsDistributor=" + IsDistributor + "&SelectedPartID=" + sSelectedPartID + "&SourchFrom=MRPartDetails&TransFrom=Common", "List", "scrollbars=no,resizable=no,dialogWidth=100px,dialogHeight=100px");
            }
        }
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');

            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }
        function refresh() {
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }
        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var filename = objFileUpload.value;
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search('_SparesPO_PartDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }
        }

        function ClearRowValueForMR(event, objCancelControl) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;
            var i = 1;

            //objCancelControl.style.display = "none";
            objCancelControl.innerHTML = "Cancelled"
            objCancelControl.disabled = "true"

            //            //Set PartId;
            //            objRow[1].childNodes[0].value = '';

            //            //SetPartNo

            //            objRow[2].children[0].value = "";
            //            //objRow[2].children[0].style.display = "none";


            //            //SetPartName
            //            objRow[3].childNodes[0].value = '';

            //            //Set MOQ
            //            objRow[4].childNodes[0].value = '0';

            //            //SetQuantity
            //            objRow[5].childNodes[0].value = '0';

            //            //SetFoBRate        
            //            objRow[6].childNodes[0].value = '0';

            //            //Total
            //            objRow[7].childNodes[0].value = '0';
            //Status
            objRow[14].childNodes[0].value = 'C';

            // objRow[2].children[0].style.display = "none";
            // objRow[2].children[1].style.display = "";
            //SetNewLabel Display
            //objRow[].children[1].style.display = "none";
            CalulateReceivePartGranTotal();
        }

        function SelectDeleteRowValueForMR(event, objCancelControl, objDeleteCheck) {
            var objRow = objCancelControl.parentNode.parentNode.childNodes;

            if (objDeleteCheck.checked == true) {
                objRow[14].childNodes[0].value = 'D';
            }
            else {
                objRow[14].childNodes[0].value = 'E';
            }
            CalulateReceivePartGranTotal();
            return false
        }
        function SelectDeletCheckboxMR(event, ObjChkDelete) {
            if (ObjChkDelete.checked) {
                if (confirm("Are you sure you want to delete this record?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }
            }
            else {
                if (confirm("Are you sure you want to revert changes?") == true) {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'white';
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                }
                else {
                    ObjChkDelete.parentNode.parentNode.style.backgroundColor = 'orange';
                    ObjChkDelete.checked = false;
                    SelectDeleteRowValueForMR(event, ObjChkDelete.parentNode, ObjChkDelete);
                    return false;
                }

            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Receipt for PDI"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc1:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                 <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />
                </asp:Panel>

                <asp:Panel ID="POHeader" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntDealerPO"
                            ExpandControlID="TtlDealerPO" CollapseControlID="TtlDealerPO" Collapsed="false"
                            ImageControlID="ImgTtlDealerPODetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Vehicle Receipt" ExpandedText="Vehicle Receipt"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlDealerPO" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="Vehicle Receipt" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDealerPODetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntDealerPO" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                        <table id="txtDocNoDetails" runat="server" class="ContainTable" border="1">
                             
                            
                                <tr>
                                      <td  class="tdLabel">
                                    <asp:Label ID="lblMReceiptNo" runat="server" Text="Receipt No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMReceiptNo" Text="" runat="server" CssClass="TextBoxForString"
                                       ></asp:TextBox>
                                </td>
                                     <td  class="tdLabel">
                                    <asp:Label ID="lblMReceiptDate" runat="server" Text="Receipt Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtMReceiptDate" runat="server" bCheckforCurrentDate="false"
                                        />
                                </td>
                                </tr>
                            <tr>
                                 <td  class="tdLabel">
                                    <asp:Label ID="lblDMSInvNo" runat="server" Text="Invoice No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpInvNo" runat="server"
                                        CssClass="ComboBoxFixedSize "
                                        OnSelectedIndexChanged="drpInvNo_SelectedIndexChanged" AutoPostBack="true" Style="display: none">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtDMSInvNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>

                                  <td  class="tdLabel">
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <uc3:CurrentDate ID="txtDMSInvDate" runat="server" bCheckforCurrentDate="false" Mandatory="false" />
                                </td>
                            </tr>
                                <tr>
                                  
                                     <td  class="tdLabel">
                                    Delivery No:
                                    </td>

                                      <td>
                                    <asp:TextBox ID="txtDeliveryNo" runat="server" CssClass="TextBoxForString" Text="" AutoPostBack="true" OnTextChanged="txtDeliveryNo_TextChanged"></asp:TextBox>
                                    </td>

                                     <td  class="tdLabel">
                                    PO No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                    </td>
                                     <td  class="tdLabel">
                                    PO Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtPODate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>


                                    </tr>
                            <tr>
                                     <td  class="tdLabel">
                                    Parking Location:
                                    </td>

                                      <td>
                                    <asp:TextBox ID="txtParking" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                    </td>


                                    <td  class="tdLabel">
                                    Customer:
                                    </td>

                                      <td>
                                    <asp:TextBox ID="txtCustomer" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                    </td>
                                

                                </tr>
                           
                        </table>
                    </asp:Panel>
                        </asp:Panel>
                
                 <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderStyle="Double">
                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntModel"
                            ExpandControlID="TtlModel" CollapseControlID="TtlModel" Collapsed="false"
                            ImageControlID="ImgTtlModelDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Model" ExpandedText="Model"
                            TextLabelID="lblTtlModel">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlModel" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlModel" runat="server" Text="Model" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntModel" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                         <table id="Table2" runat="server" class="ContainTable" border="1">
                            <%-- <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    Model
                                </td>
                            </tr>--%>
                            <tr>
                                
                                
                                  <td class="tdLabel">
                                Model Category:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label35" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                                  <td  class="tdLabel">
                                    Model Code:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModelCode" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    
                                </td>
                            
                                  <td  class="tdLabel">
                                    Model:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                   
                                </td>
                            </tr>
                            <tr>
                            
                            
                                <td  class="tdLabel">
                                    Model Price:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtModelRate" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td  class="tdLabel">
                                    Quantity:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtQty" runat="server" CssClass="TextBoxForString" AutoPostBack="true" ></asp:TextBox>
                                    &nbsp;
                                    </td>
                                  <td  class="tdLabel">
                                    Total:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="TextBoxForString" AutoPostBack="true" ></asp:TextBox>
                                    &nbsp;
                                    </td>

                             
                            </tr>
                             <tr>
                                   <td  class="tdLabel">
                                    Chassis No:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td  class="tdLabel">
                                    Engine No:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString" AutoPostBack="true" ></asp:TextBox>
                                    &nbsp;
                                    </td>

                                  <td class="auto-style1">
                                 <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false" ></asp:Label>
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false"  CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <%--<b class="Mandatory">*</b>--%>
                                </td>
                             </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

                   

                <asp:Panel ID="PTaxDet" runat="server" BorderColor="Black" BorderStyle="Double">
                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDet"
                            ExpandControlID="TtlTaxDet" CollapseControlID="TtlModel" Collapsed="false"
                            ImageControlID="ImgTtlTaxDetDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Tax Details" ExpandedText="Tax Details"
                            TextLabelID="lblTtlTaxDet">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlTaxDet" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlTaxDet" runat="server" Text="Tax Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlTaxDetDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntTaxDet" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                         <table id="Table1" runat="server" class="ContainTable" border="1">
                          
                           
                            
                            
                             <tr>
                                  <td class="tdLabel">
                                                                        </td>
                                  <td class="tdLabel">
                                                                        </td>

                                  <td  class="tdLabel">
                                    Total Amount
                                    </td>
                               

                                   <td  class="tdLabel">
                                    Discount
                                    </td>
                               
                                   <td  class="tdLabel">
                                    <asp:Label ID="lblCST" runat="server" Text="CST"></asp:Label>
                                </td>
                                 
                                  <td  class="tdLabel">
                                    <asp:Label ID="lblVat" runat="server" Text="VAT"></asp:Label>
                                </td>
                                  
                                
                                    <td  class="tdLabel">
                                    <asp:Label ID="lblTax1" runat="server" Text="Tax1"></asp:Label>
                                </td>

                              <td id="tdlbl" runat="server" >
                                    <td  class="tdLabel">
                                    <asp:Label ID="lblTax2" runat="server" Text="Tax2"></asp:Label>
                                </td>

                                 <td  class="tdLabel">
                                    PF Charges
                                    </td>
                                   <td  class="tdLabel">
                                    Other Charges
                                    </td>
                                  <td  class="tdLabel">
                                    TCS
                                    </td>
                                 </td>
                                    <td  class="tdLabel">
                                    Grand Total
                                    </td>


                             </tr>

                             <tr>
                                   <td class="tdLabel">
                                   %
                                    </td>
                                 <td  class="tdLabel">
                                 
                                    </td>
                                  <td>
                                    <asp:TextBox ID="Totalblank" runat="server"  CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>
                                    &nbsp;
                                    </td>

                                  <td>
                                    <asp:TextBox ID="DiscBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    
                                </td>
                                   <td>
                                    <asp:TextBox ID="txtCSTPer" runat="server" ReadOnly="true" CssClass="TextBoxForString" ></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtVatPer" runat="server" ReadOnly="true"  CssClass="TextBoxForString" ></asp:TextBox>
                                    
                                    </td>
                                

                                   <td>
                                    <asp:TextBox ID="txttax1Per" runat="server" ReadOnly="true" CssClass="TextBoxForString" ></asp:TextBox>
                                    
                                    </td>
                                    <td id="tdper" runat="server">
                                    <td>
                                    <asp:TextBox ID="txtTax2Per" runat="server" ReadOnly="true" CssClass="TextBoxForString" ></asp:TextBox>
                                    
                                    </td>
                              
                                     <td>
                                    <asp:TextBox ID="PfBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                  <td>
                                    <asp:TextBox ID="Otherblank" runat="server"  CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    
                                    </td>
                                 <td>
                                    <asp:TextBox ID="txtTCSPer" runat="server" ReadOnly="true" CssClass="TextBoxForString"  ></asp:TextBox>
                                    
                                    </td>
                                     </td>
                                 <td>
                                    <asp:TextBox ID="GrandBlank" runat="server" CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>



                             </tr>
                              <tr>
                                   <td  class="tdLabel">
                                   Amt
                                    </td>
                                 <td class="tdLabel">
                                 
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtTaxTotalAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"   ></asp:TextBox>
                                    &nbsp;
                                    </td>

                                  <td>
                                    <asp:TextBox ID="txtDisc" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    
                                </td>
                                   <td>
                                    <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtVatAmt" runat="server" CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                 

                                   <td>
                                    <asp:TextBox ID="txttax1Amt" runat="server" CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                  <td id="tdamt" runat="server">
                                    <td>
                                    <asp:TextBox ID="txttax2Amt" runat="server" CssClass="TextBoxForString" ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                   <td>
                                    <asp:TextBox ID="txtPFCharges" runat="server" CssClass="TextBoxForString"  ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtOthercharges" runat="server" CssClass="TextBoxForString"  ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                 <td>
                                    <asp:TextBox ID="txtTCS" runat="server" CssClass="TextBoxForString"  ReadOnly="true" ></asp:TextBox>
                                    
                                    </td>
                                      </td>
                                 <td>
                                    <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    
                                    </td>



                             </tr>
                        </table>
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
                                <td width="1%">
                                    <asp:Image ID="ImgTtlFileAttchDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;">
                        <table id="Table5" runat="server" class="ContainTable">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                        AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                        GridLines="Horizontal" HeaderStyle-Wrap="true" DataKeyNames="File_Names"
                                        SkinID="NormalGrid" Width="100%">
                                        <%--OnRowCommand="DetailsGrid_RowCommand" OnRowDataBound="FileAttchGrid_RowDataBound"--%>
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFileAttchID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Description" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description") %>'
                                                        Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("File_Names") %>' Width="90%"
                                                        onClick="return ShowAttachDocument(this);"> 
                                                    </asp:Label>
                                                    <%-- onClick="return ShowAttachDoc(this);" ToolTip="Click Here To Open The File" ForeColor="#49A3D3"
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"--%>
                                                    <%--<a id="achFileName" runat="server" title="Click here to download file"><%# Eval("File_Names") %></a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);" />
                                                    <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Download" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" ToolTip="Click Here To Open/Download File" OnClick="lnkDownload_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle Wrap="True" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 50%" align="center">File Description
                                </td>
                                <td class="tdLabel" style="width: 50%" align="center">File Name
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdLabel">
                                    <div id="upload1" style="display: inline-block; padding-left: 15px">
                                        <input id="Text1" type="text" name="Text1" class="TextBoxForString" placeholder="File Description" style="width: 50%" />
                                        <input id="AttachFile" type="file" runat="server" style="width: 45%" class="TextBoxForString Cntrl1"
                                            onblur="return addFileUploadBox(this);" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                  <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCashLoan" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtDocCashLoanType" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM1ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM2ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM3ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM4ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM5ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM6ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtAppID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"

                         Text=""></asp:TextBox>
                <asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtCSTID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtVATID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtTax1ID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtTax2ID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                   <asp:HiddenField ID="HiddenGST" runat="server" Value="N" />
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                  <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                        <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                     <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
