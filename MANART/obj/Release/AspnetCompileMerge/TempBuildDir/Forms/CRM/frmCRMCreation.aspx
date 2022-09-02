<%@ Page Title="MAN-Service Call Ticket" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="frmCRMCreation.aspx.cs" Inherits="MANART.Forms.CRM.frmCRMCreation"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%--<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>--%>
<%@ Register Src="~/WebParts/CurrentDateTime.ascx" TagName="CurrentDate" TagPrefix="uc3" %>

<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
     <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <style>
        .checkbox .btn,
        .checkbox-inline .btn {
            padding-left: 2em;
            min-width: 8em;
        }

        .checkbox label,
        .checkbox-inline label {
            text-align: left;
            padding-left: 0.5em;
        }
    </style>
    <script type="text/javascript">
        function Acknowledge() {
            var message11 = "Call Acknowledge Successfully.";
            alert(message11);
            return false;
        }
        function CalClose(MessageID) {
            if (MessageID == 1) {
                var message111 = "Call closed Successfully.";
                alert(message111);
                return false;
            }
            else if (MessageID == 2) {
                var message1111 = "MTI FeedBack/Call Center Remark is mandatory.";
                alert(message1111);
                return false;
            }
            else if (MessageID == 3) {
                var message1111 = "Dealer FeedBack is mandatory.";
                alert(message1111);
                return false;
            }
            else if (MessageID == 4) {
                var message11111 = "Call closed Successfully.";
                alert(message11111);
                return false;
            }
            else if (MessageID == 5) {
                var message11111 = "All Feedbacks Data are mandatory.";
                alert(message11111);
                return false;
            }
            else if (MessageID == 6) {
                var message11111 = "FeedBack Data is Saved.";
                alert(message11111);
                return false;
            }
            else if (MessageID == 7) {
                var message11111 = "FeedBack Data is not Saved.";
                alert(message11111);
                return false;
            }
            else if (MessageID == 8) {
                var message11111 = "Call Ticket Opened. Please select same Call Ticket again from list to edit.";
                alert(message11111);
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        //To Show Chassis Master
        function ShowCustomerMaster_CRM(objNewModelLabel) {
            var CustomerDetailsValues;

            //  ChassisDetailsValues = window.showModalDialog("../CRM/frmCRMChassisSelection.aspx?DealerID=" + sDealerId + "&HOBR_ID=" + sHOBR_ID + "&JobTypeID='0'", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
       //     CustomerDetailsValues = window.showModalDialog("../CRM/frmCRMCustomerSelection.aspx", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");

            if (CustomerDetailsValues != null) {

                SetCustomerDetails(CustomerDetailsValues);
            }
            return false;
        }

        //Function For Set Chassis Details
        function SetCustomerDetails(CustomerDetailsValue) {

            var txtCRMCustID = window.document.getElementById('ContentPlaceHolder1_txtCRMCustID');
            var txtGlobalCustID = window.document.getElementById('ContentPlaceHolder1_txtGlobalCustID');
            var txtCustName = window.document.getElementById('ContentPlaceHolder1_txtCustName');
            var txtPhone = window.document.getElementById('ContentPlaceHolder1_txtPhone');
            var txtPinCode = window.document.getElementById('ContentPlaceHolder1_txtPinCode');
            var txtCity = window.document.getElementById('ContentPlaceHolder1_txtCity');
            var txtState = window.document.getElementById('ContentPlaceHolder1_txtState');
            //var txtCustEdit = window.document.getElementById('ContentPlaceHolder1_txtCustEdit');

            //Cleare chassis Data after customer selection 
            var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
            var txtChassisNo = window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
            var txtEngineNo = window.document.getElementById('ContentPlaceHolder1_txtEngineNo');
            var txtVehicleNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleNo');
            var txtModelID = window.document.getElementById('ContentPlaceHolder1_txtModelID');
            var txtModelCode = window.document.getElementById('ContentPlaceHolder1_txtModelCode');
            var txtModelName = window.document.getElementById('ContentPlaceHolder1_txtModelName');

            txtChassisID.value = "0";
            txtChassisNo.value = "";
            txtEngineNo.value = "";

            txtVehicleNo.value = "";
            txtModelID.value = "";
            txtModelCode.value = "";
            txtModelName.value = "";

            txtCRMCustID.value = CustomerDetailsValue[2];
            txtGlobalCustID.value = CustomerDetailsValue[3];
            txtCustName.value = CustomerDetailsValue[4];
            txtPhone.value = CustomerDetailsValue[18];
            txtPinCode.value = CustomerDetailsValue[8];
            txtCity.value = CustomerDetailsValue[15];
            txtState.value = CustomerDetailsValue[10];

        }
    </script>
     <script type="text/javascript">
         function ReturnCRMChassisValue(objImgControl) {
             //debugger;
             var objRow = objImgControl.parentNode.parentNode.childNodes;
             var sValue;
             var ArrOfChassis = new Array();
             for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
                 // sValue = objRow[cnt].innerHTML.trim();
                 sValue = objRow[cnt].innerText.trim();
                 ArrOfChassis.push(sValue);
             }
             if (ArrOfChassis != null) {
                 SetChassisDetails(ArrOfChassis);
             }
             $find("mpe2").hide();
                return true;
         }
      

    </script>
     <script type="text/javascript">
         function ReturnCRMCustomerValue(objImgControl) {

             debugger;
             var objRow = objImgControl.parentNode.parentNode.childNodes;
             var sValue;
             var ArrOfCustomer = new Array();
             for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
                 // sValue = objRow[cnt].innerHTML.trim();
                 sValue = objRow[cnt].innerText.trim();
                 ArrOfCustomer.push(sValue);
                 if (ArrOfCustomer != null) {
                     SetCustomerDetails(ArrOfCustomer);
                 }

             }
             //alert(ArrOfChassis);
             //    window.returnValue = ArrOfCustomer;
             //    window.close();
             $find("mpe1").hide();
             return true;


         }

           </script>

    <script type="text/javascript">
        //To Show Chassis Master
        function ShowChassisMaster_CRM(objNewModelLabel) {
            var ChassisDetailsValues;
            var CustID = 0;
            var txtCustID1 = window.document.getElementById('ContentPlaceHolder1_txtGlobalCustID');
            CustID = txtCustID1.value;
            //  ChassisDetailsValues = window.showModalDialog("../CRM/frmCRMChassisSelection.aspx?DealerID=" + sDealerId + "&HOBR_ID=" + sHOBR_ID + "&JobTypeID='0'", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            ChassisDetailsValues = window.showModalDialog("../CRM/frmCRMChassisSelection.aspx?CustID=" + CustID, "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");

            if (ChassisDetailsValues != null) {

                SetChassisDetails(ChassisDetailsValues);
            }

            return false;
        }

        //Function For Set Chassis Details
        function SetChassisDetails(ChassisDetailsValue) {
           // debugger;
            var txtChassisID = window.document.getElementById('ContentPlaceHolder1_txtChassisID');
            var txtChassisNo = window.document.getElementById('ContentPlaceHolder1_txtChassisNo');
            var txtEngineNo = window.document.getElementById('ContentPlaceHolder1_txtEngineNo');
            var txtVehicleNo = window.document.getElementById('ContentPlaceHolder1_txtVehicleNo');
            var txtModelID = window.document.getElementById('ContentPlaceHolder1_txtModelID');
            var txtModelCode = window.document.getElementById('ContentPlaceHolder1_txtModelCode');
            var txtModelName = window.document.getElementById('ContentPlaceHolder1_txtModelName');
            var txtCRMCustID = window.document.getElementById('ContentPlaceHolder1_txtCRMCustID');
            var txtGlobalCustID = window.document.getElementById('ContentPlaceHolder1_txtGlobalCustID');
            var txtCustName = window.document.getElementById('ContentPlaceHolder1_txtCustName');
            var txtPhone = window.document.getElementById('ContentPlaceHolder1_txtPhone');
            var txtPinCode = window.document.getElementById('ContentPlaceHolder1_txtPinCode');
            var txtCity = window.document.getElementById('ContentPlaceHolder1_txtCity');
            var txtState = window.document.getElementById('ContentPlaceHolder1_txtState');
            //var txtCustEdit = window.document.getElementById('ContentPlaceHolder1_txtCustEdit');



            txtChassisID.value = ChassisDetailsValue[1];
            txtChassisNo.value = ChassisDetailsValue[2];
            txtEngineNo.value = ChassisDetailsValue[3];

            txtVehicleNo.value = ChassisDetailsValue[4];
            txtModelID.value = ChassisDetailsValue[5];
            txtModelCode.value = ChassisDetailsValue[6];
            txtModelName.value = ChassisDetailsValue[7];
            txtCRMCustID.value = ChassisDetailsValue[8];
            txtCustName.value = ChassisDetailsValue[9];
            txtPhone.value = ChassisDetailsValue[10];
            txtPinCode.value = ChassisDetailsValue[11];
            txtCity.value = ChassisDetailsValue[12];
            txtState.value = ChassisDetailsValue[13];
            txtGlobalCustID.value = ChassisDetailsValue[14];
            //txtCustEdit.value = ChassisDetailsValue[8];

            if (txtVehicleNo.value  == "") {
               // txtVehicleNo.disabled = false;
                txtVehicleNo.readOnly = false;
                //txtVehicleNo.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
              //  txtVehicleNo.setAttribute("onkeypress", " return CheckForTextBoxValue(event,this,'6');");
                
            }
            else {
               // txtVehicleNo.disabled = false;
                txtVehicleNo.readOnly = true;
                //txtVehicleNo.disabled = true;
               // txtVehicleNo.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
            }


            //if (txtCustEdit.value == "Y") {
            //    txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueTrue(event,this)");
            //}
            //else {
            //    txtCustName.setAttribute("onkeydown", "return ToSetKeyPressValueFalse(event,this)");
            //}
        }
    </script>


    <script type="text/javascript">
        $(document).ready(function () {

            var txtTicketDate = document.getElementById("ContentPlaceHolder1_txtTicketDate_txtDocDate");
            $('#ContentPlaceHolder1_txtTicketDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtTicketDate.value == '') ? '0d' : txtTicketDate.value, maxDate: (txtTicketDate.value == '') ? '0d' : txtTicketDate.value
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


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Service Call Ticket"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="">
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

                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="TtlDocDetails" CollapseControlID="TtlDocDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Call Ticket Details" ExpandedText="Call Ticket Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDocDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDocDetails" runat="server" Text="Call Ticket Details" Width="96%"
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
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Call Type:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpCallType" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpCallType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--  OnSelectedIndexChanged="drpCallType_SelectedIndexChanged"--%>
                                    <asp:Label ID="lblMPoType" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td class="tdLabel" style="width: 15%; padding-left: 10px;"><%--Call Sub Type:--%>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpCallSubType" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" Style="display: none">
                                    </asp:DropDownList>
                                    <%--  OnSelectedIndexChanged="drpCallSubType_SelectedIndexChanged"--%>
                                    <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true" Style="display: none"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblTicketNo" runat="server" Text="Ticket No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtTicketNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblTicketDate" runat="server" Text="Ticket Date"></asp:Label>
                                </td>
                                <td style="width: 18%">

                                    <uc3:CurrentDate ID="txtTicketDate" runat="server" bCheckforCurrentDate="false" Enabled="false" />
                                </td>
                            </tr>
                            <tr id="Ack1" runat="server">


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Button ID="btnAcknowledge" runat="server"
                                        Text="Acknowledge" CssClass="btn btn-search" OnClick="btnAcknowledge_Click" /></td>

                                <td style="width: 18%"></td>


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                            </tr>
                             <tr id="CallOpen" runat="server">


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Button ID="btnOpen" runat="server"
                                        Text="Call Ticket Open" CssClass="btn btn-search" OnClick="btnOpen_Click" /></td>

                                <td style="width: 18%"></td>


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                            </tr>

                            <tr id="Calclose" runat="server" style="display: none">


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Button ID="btnCalClose" runat="server"
                                        Text="Call Close" CssClass="btn btn-search" OnClick="btnCalClose_Click" /></td>

                                <td style="width: 18%">
                                    <%--<asp:Label ID="lblAck" runat="server" Text="Call Acknowledge Successfully"  Font-Bold="true"  Style="display: none" ></asp:Label>--%>
                                </td>


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                            </tr>
                             <tr id="DealerCallClosure" runat="server" style="display: none">


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Button ID="btnDealerCallClosure" runat="server"
                                        Text="Dealer Call Closure" CssClass="btn btn-search" OnClick="btnDealerCallClosure_Click" /></td>

                                <td style="width: 18%">
                                    <%--<asp:Label ID="lblAck" runat="server" Text="Call Acknowledge Successfully"  Font-Bold="true"  Style="display: none" ></asp:Label>--%>
                                </td>


                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                            </tr>
                             <tr>
                                        <td>
                                            <asp:Label ID="Label25" runat="server" Text="Details/Remark:"></asp:Label></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtDetailsRemark" runat="server" CssClass="MultilineTextbox"
                                                TextMode="MultiLine" Height="50px" Width="96%" MaxLength="500"
                                                Text=" ">

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                            </asp:TextBox></td>

                                    </tr>
                              <tr id="callcenterRemark" runat ="server" >
                                        <td>
                                            <asp:Label ID="Label21" runat="server" Text="Call Center Remark:"></asp:Label></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtcallcenterRemark" runat="server" CssClass="MultilineTextbox"
                                                TextMode="MultiLine" Height="50px" Width="96%" MaxLength="255"
                                                Text=" ">

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                            </asp:TextBox>
                                            <asp:Label ID="Label20" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>

                                    </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <%--<asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">

                    <ContentTemplate>--%>
                <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel3"
                        ExpandControlID="Panel2" CollapseControlID="Panel2" Collapsed="false"
                        ImageControlID="Image1" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Search Vehicle Details" ExpandedText="Search Vehicle Details"
                        TextLabelID="Label2">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel2" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label2" runat="server" Text="Vehicle Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <table id="Table1" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:LinkButton ID="lblSelectChassis" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Select Chassis" Width="80%"
                                        ToolTip="Select Chassis Details" OnClick="lblSelectChassis_Click"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%"></td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>

                            </tr>

                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label3" runat="server" Text="Chassis No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisID" runat="server" CssClass="TextBoxForString"
                                        Text="" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>


                                </td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label4" runat="server" Text="Engine No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label14" runat="server" Text="Vehicle No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="TextBoxForString" MaxLength ="12" ></asp:TextBox>
                                </td>


                            </tr>
                            <tr>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label5" runat="server" Text="Model Code"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtModelID" runat="server" CssClass="TextBoxForString"
                                        Text="" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtModelCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label15" runat="server" Text="Model Name"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtModelName" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>

                            </tr>

                        </table>
                    </asp:Panel>
                </asp:Panel>


                <asp:Panel ID="Panel4" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="Panel6"
                        ExpandControlID="Panel5" CollapseControlID="Panel5" Collapsed="false"
                        ImageControlID="Image2" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Search Customer Details" ExpandedText="Search Customer Details"
                        TextLabelID="Label6">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel5" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label6" runat="server" Text="Customer Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel6" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <table id="Table2" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:LinkButton ID="lblSelectCustomer" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Select Customer" Width="80%"
                                        ToolTip="Select Customer Details" OnClick="lblSelectCustomer_Click"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%"></td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>

                            </tr>

                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label7" runat="server" Text="Customer Name"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCRMCustID" runat="server" CssClass="TextBoxForString"
                                        Text="0" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtGlobalCustID" runat="server" CssClass="TextBoxForString"
                                        Text="0" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label16" runat="server" Text="Contact No."></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label17" runat="server" Text="Pin Code"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPinCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label8" runat="server" Text="City"></asp:Label>
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label9" runat="server" Text="State"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtState" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label19" runat="server" Text="Alternate Contact No."></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtAlternateContactNo" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    <b class="Mandatory">*</b>
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>
                </asp:Panel>
                <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                 <asp:UpdatePanel UpdateMode="Conditional" ID="UPDealer" runat="server" ChildrenAsTriggers="true">

                    <ContentTemplate>
                <asp:Panel ID="Panel7" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Panel9"
                        ExpandControlID="Panel8" CollapseControlID="Panel8" Collapsed="false"
                        ImageControlID="Image3" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Search Dealer Details" ExpandedText="Search Dealer Details"
                        TextLabelID="Label10">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel8" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label10" runat="server" Text="Dealer Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel9" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <table id="Table3" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label13" runat="server" Text="State"></asp:Label>
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpState" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label18" runat="server" Text="District"></asp:Label>
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpDistrict" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpDistrict_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label11" runat="server" Text="Dealer Name"></asp:Label>
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpDealerName" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpDealerName_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">Dealer Code</td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">City</td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDCity" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td class="tdLabel" style="width: 15%; padding-left: 10px;">Region</td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDRegion" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                    <%--  OnSelectedIndexChanged="drpCallType_SelectedIndexChanged"--%>
                                  
                                    
                                </td>

                            </tr>
                            

                        </table>
                    </asp:Panel>
                </asp:Panel>
                </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="PDealerFeedback" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntDealerFeedback"
                        ExpandControlID="TtlDealerFeedback" CollapseControlID="TtlDealerFeedback" Collapsed="false"
                        ImageControlID="ImgTtlDealerFeedback" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Dealer Feedback" ExpandedText="Dealer Feedback"
                        TextLabelID="lblTtlDealerFeedback">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerFeedback" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">

                                    <asp:Label ID="lblTtlDealerFeedback" runat="server" Text="Dealer Feedback" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDealerFeedback" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDealerFeedback" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">



                        <asp:GridView ID="DealerFeedback" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDealerID" runat="server" Text='<%# Eval("ID") %>' Width="3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dealer Post Service" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDealerFeedBack" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("DealerFeedBack") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxForString" Text='<%# Eval("Remark") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                
               
                 <asp:Button ID="btnFeedBackSave" runat="server"
                                        Text="FeedBack Save" CssClass="btn btn-search" OnClick="btnFeedBackSave_Click" />
                
                
                 <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">

                    <ContentTemplate>
                <asp:Panel ID="PMTIFeedback" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="CntMTIFeedback"
                        ExpandControlID="TtlMTIFeedback" CollapseControlID="TtlMTIFeedback" Collapsed="false"
                        ImageControlID="ImgTtlMTIFeedback" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="MTI Call Closure Feedback" ExpandedText="MTI Call Closure Feedback"
                        TextLabelID="lblTtlMTIFeedback">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlMTIFeedback" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">

                                    <asp:Label ID="lblTtlMTIFeedback" runat="server" Text="MTI Call Closure Feedback" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlMTIFeedback" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntMTIFeedback" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">



                        <asp:GridView ID="MTIFeedback" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMTIID" runat="server" Text='<%# Eval("ID") %>' Width="3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FeedBack" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMTIFeedBack" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("MTIFeedBack") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="DAY-1" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                       <asp:DropDownList ID="drpFeedBackStatus1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%"
                                           AutoPostBack="true" 
                                    OnSelectedIndexChanged="drpFeedBackStatus1_SelectedIndexChanged"
                                            >
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtFeedBackStatus1" runat="server" Text='<%# Eval("FeedBackStatus1") %>' Width="3" Visible ="false" ></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="DAY-2" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                       <asp:DropDownList ID="drpFeedBackStatus2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%"  
                                           AutoPostBack="true" 
                                    OnSelectedIndexChanged="drpFeedBackStatus2_SelectedIndexChanged"   >
                                        </asp:DropDownList>
                                       
                                         <asp:TextBox ID="txtFeedBackStatus2" runat="server" Text='<%# Eval("FeedBackStatus2") %>' Width="3" Visible ="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DAY-3" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                       <asp:DropDownList ID="drpFeedBackStatus3" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AutoPostBack="true" 
                                    OnSelectedIndexChanged="drpFeedBackStatus3_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtFeedBackStatus3" runat="server" Text='<%# Eval("FeedBackStatus3") %>' Width="3" Visible ="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="RATING (SCALE)" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                       <asp:DropDownList ID="drpScale" runat="server" CssClass="GridComboBoxFixedSize" Width="99%"  Enabled="false">
                                        </asp:DropDownList>
                                         <asp:TextBox ID="txtScale" runat="server" Text='<%# Eval("Scale") %>' Width="3"  Visible ="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="4%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMTIRemarks" runat="server"  CssClass="TextBoxForString" Text='<%# Eval("MTIRemark") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
</ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                 <asp:TextBox ID="txtMenuid" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                 <asp:TextBox ID="txtUserid" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnPoTypeID" runat="server" />
                <asp:HiddenField ID="hdnPartsIDs" runat="server" />
                <asp:TextBox ID="txtJobcardHDRID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
    <cc1:ModalPopupExtender ID="ModalPopUpExtender1" runat="server" TargetControlID="lblTragetID1" PopupControlID="pnlPopupWindow"
        OkControlID="btnOK1" BackgroundCssClass="modalBackground" BehaviorID="mpe1">
 </cc1:ModalPopupExtender>    
     <asp:Label ID="lblTragetID1" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup" Style="display: none; width:700px; " >
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                     <asp:Label ID="Label27" runat="server" Text="">
                         </asp:Label>
                </td>
            </tr>
          
       
            <tr id="TblControl1">
                <td style="width: 14%">
                    <div align="center" class="ContainTable">
                        <table style="background-color: #efefef;" width="100%">
                           
                        <tr align="center">
                            <td class="tdLabel" style="width: 7%;">
                                Search:
                            </td>
                            <td class="tdLabel" style="width: 15%;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>                                
                            </td>
                            
                            <td class="tdLabel" style="width: 15%;">
                                <asp:DropDownList ID="DdlSelctionCriteria" runat="server" 
                                    CssClass="ComboBoxFixedSize">
                                    <asp:ListItem Selected="True" Value ="Customer_name">Customer Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td  style="width: 15%;">                                
                                <asp:Button ID="btnSearch" runat="server" Text="Search"  
                                     CssClass="btn btn-search btn-sm" onclick="btnSearch_Click" />
                            </td>        
                                 <td  style="width: 15%; float:right;">                                
                                <asp:Button ID="btnOK1" runat="server" Text="OK"  
                                     CssClass="btn btn-search btn-sm"  />
                            </td>                 
                        </tr>
                          
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel10" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                         <div class="rounded_corners">
                        <asp:GridView ID="CustomerGrid" runat="server" AlternatingRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                            AutoGenerateColumns="false"
                            OnPageIndexChanging="CustomerGrid_PageIndexChanging">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                <ItemTemplate>
                                 <%--  <asp:Button ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  OnClientClick="return ReturnCRMChassisValue(this);"/> --%>     
                                <asp:Image ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  onClick="return ReturnCRMCustomerValue(this);"/>  
                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cust_Type" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCust_Type" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cust_Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CRM_Cust_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCRM_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CRM_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Global_Cust_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblGlobal_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Global_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Existing_MTI_Cust" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblExisting_MTI_Cust" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Existing_MTI_Cust") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="add1" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lbladd1" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("add1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="add2" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lbladd2" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("add2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Pincode" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblpincode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("pincode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="state_id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblstate_id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("state_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="State" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("State") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="district_id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lbldistrict_id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("district_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Distict_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistict_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Distict_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Region_id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion_id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Region_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Region_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Region_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="city" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblcity" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("city") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Country_Id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCountry_Id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Country_Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Country_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCountry_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Country_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Phone") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="E_mail" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblE_mail" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("E_mail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Primary_Application_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimary_Application_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Primary_Application_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Secondary_Application_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblSecondary_Application_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Secondary_Application_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prefix" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPrefix" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Prefix") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              </Columns> 
                            <HeaderStyle Wrap="True" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </div> 
                   </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lblTragetID2" PopupControlID="pnlPopupWindow2"
        OkControlID="btnOK2" BackgroundCssClass="modalBackground" BehaviorID="mpe2">
 </cc1:ModalPopupExtender>   
     <asp:Label ID="lblTragetID2" runat="server"></asp:Label>
     <asp:Panel ID="pnlPopupWindow2" runat="server" CssClass="modalPopup" Style="display: none; width:900px;" >
             <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage2" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                     <asp:Label ID="Label12" runat="server" Text="">
                         </asp:Label>
                </td>
            </tr>
          
       
            <tr id="TblControl2">
                <td style="width: 14%">
                    <div align="center" class="ContainTable">
                        <table style="background-color: #efefef;" width="100%">
                           
                        <tr align="center">
                            <td class="tdLabel" style="width: 7%;">
                                Search:
                            </td>
                            <td class="tdLabel" style="width: 15%;">
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="TextBoxForString"></asp:TextBox>                                
                            </td>
                            
                            <td class="tdLabel" style="width: 15%;">
                                <asp:DropDownList ID="DropDownList1" runat="server" 
                                    CssClass="ComboBoxFixedSize">
                                    <asp:ListItem Selected="True" Value="Chassis_no">Chassis No</asp:ListItem>                                                                       
                                    <asp:ListItem Value ="Reg_No">Vehicle Reg No</asp:ListItem>
                                    <asp:ListItem Value ="Customer_name">Customer Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td  style="width: 15%;">                                
                                <asp:Button ID="Button1" runat="server" Text="Search"  
                                     CssClass="btn btn-search btn-sm" OnClick="btnSearch_Click1" />
                            </td>       
                             <td  style="width: 15%; float:right;">                                
                                <asp:Button ID="btnok" runat="server" Text="OK"  
                                     CssClass="btn btn-search btn-sm"  />
                            </td>                          
                        </tr>
                          
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                         <div class="rounded_corners">
                        <asp:GridView ID="ChassisGrid" runat="server" AlternatingRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                            AutoGenerateColumns="false"
                            OnPageIndexChanging="ChassisGrid_PageIndexChanging">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                <ItemTemplate>
                                 <%--  <asp:Button ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  OnClientClick="return ReturnCRMChassisValue(this);"/> --%>     
                                <asp:Image ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  onClick="return ReturnCRMChassisValue(this);"/>  
                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Chassis No." ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblChassisNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_no") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEngineNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Engine_no") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicleNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Reg_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Model_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                              
                            <asp:TemplateField HeaderText="Model Name" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                              <asp:TemplateField HeaderText="CRM_Cust_ID" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCRM_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CRM_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                             
                            <asp:TemplateField HeaderText="Phone" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Phone") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="pincode " ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblpincode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("pincode ") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                                                    
                            <asp:TemplateField HeaderText="city" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblcity" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("city") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("State") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Global_Cust_ID" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblGlobal_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Global_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                                                                                                                              
                        </Columns> 
                            <HeaderStyle Wrap="True" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </div> 
                             </asp:Panel>
                </td>
            </tr>
        </table>
         </asp:Panel>
     <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
</asp:Content>
