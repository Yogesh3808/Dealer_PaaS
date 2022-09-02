<%@ Page Title="MAN-Sales Call Ticket" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    CodeBehind="frmCRMCreation_Sales.aspx.cs" Inherits="MANART.Forms.CRM.frmCRMCreation_Sales"
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
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
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
        //To Show Chassis Master
        function ShowCustomerMaster_CRM(objNewModelLabel) {
            var CustomerDetailsValues;

            //  ChassisDetailsValues = window.showModalDialog("../CRM/frmCRMChassisSelection.aspx?DealerID=" + sDealerId + "&HOBR_ID=" + sHOBR_ID + "&JobTypeID='0'", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            //   CustomerDetailsValues = window.showModalDialog("../CRM/frmCRMCustomerSelection.aspx", "List", "dialogHeight: 300px; dialogWidth: 800px;  edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            // debugger;
            if (CustomerDetailsValues != null) {
                // debugger;
                SetCustomerDetails(CustomerDetailsValues);

            }

            return true;


        }

        function SetCustomerDetails(CustomerDetailsValue) {
            debugger;
            var drpCustType = window.document.getElementById('ContentPlaceHolder1_drpCustType');
            var txtCRMCustID = window.document.getElementById('ContentPlaceHolder1_txtCRMCustID');
            var txtGlobalCustID = window.document.getElementById('ContentPlaceHolder1_txtGlobalCustID');
            var txtCustName = window.document.getElementById('ContentPlaceHolder1_txtCustName');
            var drpIsMTICust = window.document.getElementById('ContentPlaceHolder1_drpIsMTICust');
            var txtAddress1 = window.document.getElementById('ContentPlaceHolder1_txtAddress1');
            var txtAddress2 = window.document.getElementById('ContentPlaceHolder1_txtAddress2');
            var txtPinCode = window.document.getElementById('ContentPlaceHolder1_txtPinCode');
            var drpState = window.document.getElementById('ContentPlaceHolder1_drpState');
            var drpDistrict = window.document.getElementById('ContentPlaceHolder1_drpDistrict');
            var drpRegion = window.document.getElementById('ContentPlaceHolder1_drpRegion');
            var txtCity = window.document.getElementById('ContentPlaceHolder1_txtCity');
            var drpCountry = window.document.getElementById('ContentPlaceHolder1_drpCountry');
            var txtMobile = window.document.getElementById('ContentPlaceHolder1_txtMobile');
            var txtEmail = window.document.getElementById('ContentPlaceHolder1_txtEmail');
            var drpPrimaryApplication = window.document.getElementById('ContentPlaceHolder1_drpPrimaryApplication');
            var drpSeconadryApplication = window.document.getElementById('ContentPlaceHolder1_drpSeconadryApplication');
            var drpTitle = window.document.getElementById('ContentPlaceHolder1_drpTitle');

            drpCustType.value = CustomerDetailsValue[1];
            txtCRMCustID.value = CustomerDetailsValue[2];
            txtGlobalCustID.value = CustomerDetailsValue[3];
            txtCustName.value = CustomerDetailsValue[4];
            drpIsMTICust.value = CustomerDetailsValue[5];
            txtAddress1.value = CustomerDetailsValue[6];
            txtAddress2.value = CustomerDetailsValue[7];
            txtPinCode.value = CustomerDetailsValue[8];
            drpState.value = CustomerDetailsValue[9];
            drpDistrict.value = CustomerDetailsValue[11];
            drpRegion.value = CustomerDetailsValue[13];
            txtCity.value = CustomerDetailsValue[15];
            drpCountry.value = CustomerDetailsValue[16];
            txtMobile.value = CustomerDetailsValue[18];
            txtEmail.value = CustomerDetailsValue[19];

            drpPrimaryApplication.value = CustomerDetailsValue[20];
            drpSeconadryApplication.value = CustomerDetailsValue[21];
            drpTitle.value = CustomerDetailsValue[22];
        }
    </script>

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
                var message11111 = "All Feedbacks are mandatory.";
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
                <asp:Label ID="lblTitle" runat="server" Text="Sales Call Ticket"> </asp:Label>
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
                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">

                    <ContentTemplate>
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
                                            <asp:Label ID="lblTtlDocDetails" runat="server" Text="Document Details" Width="96%"
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
                                        <td class="tdLabel" style="width: 15%; padding-left: 10px;">Call Sub Type:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:DropDownList ID="drpCallSubType" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <%--  OnSelectedIndexChanged="drpCallSubType_SelectedIndexChanged"--%>
                                            <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                            <asp:Label ID="lblTicketNo" runat="server" Text="Ticket No"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtTicketNo" runat="server" CssClass="NonEditableFields" Enabled="false"></asp:TextBox>
                                        </td>


                                        <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                            <asp:Label ID="lblTicketDate" runat="server" Text="Ticket Date"></asp:Label>
                                        </td>
                                        <td style="width: 18%">

                                            <uc3:CurrentDate ID="txtTicketDate" runat="server" bCheckforCurrentDate="false" Enabled="false" />
                                            <%-- <uc3:CurrentDate ID="dtpAllocateTime" runat="server" bTimeVisible="true" Mandatory="true" bCheckforCurrentDate="true" />--%>
                                        </td>
                                    </tr>
                                    <tr id="Ack1" runat="server">


                                        <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                            <asp:Button ID="btnAcknowledge" runat="server"
                                                Text="Acknowledge" CssClass="btn btn-search" OnClick="btnAcknowledge_Click" /></td>

                                        <td style="width: 18%">
                                            <%--<asp:Label ID="lblAck" runat="server" Text="Call Acknowledge Successfully"  Font-Bold="true"  Style="display: none" ></asp:Label>--%>
                                     
                                        </td>


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
                                                TextMode="MultiLine" Height="50px" Width="96%" MaxLength="255"
                                                Text=" ">

                                            </asp:TextBox></td>

                                    </tr>
                                     <tr id="callcenterRemark" runat ="server" >
                                        <td>
                                            <asp:Label ID="Label28" runat="server" Text="Call Center Remark:"></asp:Label></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtcallcenterRemark" runat="server" CssClass="MultilineTextbox"
                                                TextMode="MultiLine" Height="50px" Width="96%" MaxLength="255"
                                                Text=" ">

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                            </asp:TextBox>
                                             <asp:Label ID="Label29" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>

                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

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
                                <td style="width: 15%" class="">Customer Type:
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="drpCustType" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpCustType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label2" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">Title:
                                </td>
                                <td style="width: 15%">

                                    <asp:DropDownList ID="drpTitle" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label24" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label7" runat="server" Text="Customer Name"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCRMCustID" runat="server" CssClass="TextBoxForString"
                                        Text="0" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtGlobalCustID" runat="server" CssClass="TextBoxForString"
                                        Text="0" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                    <asp:Label ID="Label20" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>


                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Existing MTI Customer:" CssClass="tdLabel"></asp:Label>
                                </td>

                                <td>
                                    <asp:DropDownList ID="drpIsMTICust" runat="server" Width="100px" CssClass="ComboBoxFixedSize">
                                        <%--EnableViewState="true">--%>
                                        <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Y</asp:ListItem>
                                        <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label4" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">Address 1 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label5" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">Address 2 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <%--<asp:Label ID="Label21" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label17" runat="server" Text="Pin Code"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPinCode" runat="server" CssClass="TextBoxForString"
                                        MaxLength="6"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    <asp:Label ID="Label22" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>

                                <td style="width: 15%" class="">State:
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpState" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label8" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">District:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpDistrict" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label9" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%" class="">Region:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpRegion" runat="server"
                                        CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label14" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">City:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label15" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td class="" style="width: 15%">Country:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpCountry" runat="server"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label23" Text="*" runat="server" CssClass="Mandatory"></asp:Label>

                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%" class="">Mobile:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    <asp:Label ID="Label16" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td class="" style="width: 15%">Email:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <%--<asp:Label ID="Label26" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                                </td>
                                <td class="auto-style5">Primary Application:
                                </td>

                                <td class="auto-style6">
                                    <asp:DropDownList ID="drpPrimaryApplication" runat="server"
                                        CssClass="ComboBoxFixedSize" AutoPostBack="True" OnSelectedIndexChanged="drpPrimaryApplication_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label18" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>


                            </tr>
                            <tr>
                                <td class="auto-style5">Secondary Application:
                                </td>
                                <td class="auto-style6">
                                    <asp:DropDownList ID="drpSeconadryApplication" runat="server"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label19" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                               <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label21" runat="server" Text="Alternate Contact No."></asp:Label>
                                </td>
                                <td style="width: 18%">
                                  
                                    <asp:TextBox ID="txtAlternateContactNo" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    <asp:Label ID="Label26" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td class="auto-style5"></td>
                                <td class="auto-style6"></td>
                            </tr>


                        </table>
                    </asp:Panel>
                </asp:Panel>
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

                                    <asp:DropDownList ID="drpStateDealer" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpStateDealer_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label11" runat="server" Text="District"></asp:Label>
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpDistrictDealer" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpDistrictDealer_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label12" runat="server" Text="Dealer Name"></asp:Label>
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
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntDealerFeedback"
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
                                <asp:TemplateField HeaderText="Dealer Feedback" ItemStyle-Width="10%">
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
                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">

                    <ContentTemplate>
                <asp:Panel ID="PMTIFeedback" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntMTIFeedback"
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
                                        <asp:TextBox ID="txtMTIRemarks" runat="server" CssClass="TextBoxForString" Text='<%# Eval("MTIRemark") %>'> </asp:TextBox>
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
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup" Style="display: none; width: 700px;">
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
                                <td class="tdLabel" style="width: 7%;">Search:
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString" Width="120px"></asp:TextBox>
                                </td>

                                <td class="tdLabel">
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" Width="120px"
                                        CssClass="ComboBoxFixedSize">
                                        <asp:ListItem Selected="True" Value="Customer_name">Customer Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%;">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                                        CssClass="btn btn-search btn-sm" OnClick="btnSearch_Click" />
                                </td>
                                <td style="float: right; margin-right: 5px;">
                                    <asp:Button ID="btnok" runat="server" Text="OK" CssClass="btn btn-search btn-sm" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
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
                                            <asp:Image ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" onClick="return ReturnCRMCustomerValue(this);" />

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
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="25%">
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
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblE_mail" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("E_mail") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Primary_Application_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrimary_Application_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Primary_Application_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Secondary_Application_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSecondary_Application_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Secondary_Application_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prefix" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
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
     <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
</asp:Content>

