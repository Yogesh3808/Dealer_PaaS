<%@ Page Title="" Language="C#" 
    EnableEventValidation="false" EnableViewState="true" AutoEventWireup="true" CodeBehind="frmWarrantyClaimProcessing.aspx.cs" Inherits="MANART.Forms.Warranty.frmWarrantyClaimProcessing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">--%>
    <title ></title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsGCRfunction.js"></script>
    <script src="../../Scripts/jsWarrantyFunction.js"></script>
    <script src="../../Scripts/jsWarrantyProcessing.js"></script>
    <script src="../../Scripts/jsWCServiceHistory.js"></script>
    <%--<script src="../../Scripts/jsWCFileAttach.js"></script>--%>
    <script src="../../Scripts/jsFileAttach.js"></script>
    <base target="_self" />

    <script>
        function ShowAttachDocument(objFileControl) {
            //debugger;
            var objRow = objFileControl.parentNode.parentNode.childNodes;
            var sFileName = '';
            var sUserType = '';
            popht = 3; // popup height
            popwth = 3; // popup width
            //var sDealerCode = document.getElementById('ContentPlaceHolder1_txtDealerCode').value;
            //var txtUserType = document.getElementById('ContentPlaceHolder1_txtUserType');
            var sDealerCode = document.getElementById('txtDealerCode').value;
            var txtUserType = document.getElementById('txtUserType');
            if (txtUserType != null) {
                sUserType = txtUserType.value;
            }
            sFileName = (objRow[4].children[0].innerText);
            var scrleft = (screen.width / 2) - (popwth / 2) - 80; //centres horizontal
            var scrtop = ((screen.height / 2) - (popht / 2)) - 40; //centres vertical
            window.open("../Spares/frmOpenAttachDocument.aspx?FileName=" + sFileName + "&DealerCode=" + sDealerCode + "&UserType=" + sUserType, "List", "top=" + scrtop + ",left=" + scrleft + ",width=1px,height=3px");
        }
    </script>
    <script type="text/javascript">
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
    <script type="text/javascript">
        function ShowReport_OtherClaim(obj, strReportpath, iUserRoleId) {
            debugger;
            var iDocId = 0;
            var sExportYesNo = "";
            var RptTitle = "";
            var msgAnswer = "";
            //var iUserRoleId = 0;

            var Control = document.getElementById('hdnOtherClaimId');

            if (Control == null) return;
            if (Control.value == "") {
                alert("Please Select The Record For Print!");
                return false;
            }
            else {
                iDocId = Control.value;
            }
           
            var Url = strReportpath;
           
            var sReportName = "";
            
            sReportName = "/RptWarrantyInvoiceGST&";
            sExportYesNo = "Y";
           
            if (sReportName == "") {
                return false;
            }
            //Url = Url + sReportName + "ID=" + iDocId + "&FPDAYesNo=" + sFPDAYesNo + "&UserRoleId=" + iUserRoleId + "&ExportYesNo=" + sExportYesNo + "";
            Url = Url + sReportName + "ID=" + iDocId + "&ExportYesNo=" + sExportYesNo + "";

            //window.open(Url, "MyReport", "dialogHeight: 700px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No;    scroll: Yes; status: Yes;");
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


    </script>

    <%--<script type="text/javascript" language="javascript">
        if (document.getElementById) {
            // IE 5 and up, FF  
            var upLevel = true;
        } else if (document.layers) {
            // Netscape 4   
            var ns4 = true;
        } else if (document.all) {
            //IE 4   
            var ie4 = true;
        }
        function showObject(obj) {
            if (ns4) { obj.visibility = "show"; }
            else if (ie4 || upLevel) {
                drawMessageBox();
                //obj.style.visibility = "visible";
            }
        }
        showObject('splashScreen');
        function hideObject(obj) {
            if (ns4) {
                obj.visibility = "hide";
            }
            if (ie4 || upLevel)
            { obj.style.visibility = "hidden"; }
        }
        function drawMessageBox() {
            var box = '<div id="splashScreen" style="position: absolute; z-index: 5; top: 30%; left: 35%; background-color: #FFFFFF; font-family: Verdana">'
           + '<table cellpadding="0" cellspacing="0" style="width:300px; height:200px;">'
              + '<tr><td style="width:100%; height:100%;font-family:Tahoma;" align="center" valign="middle">'
                     + '<br/><br />' + '<asp:Image ID="LoadImg" ImageUrl="~/images/Wait.gif" runat="server" />'
                                                         + '</td><td></td></tr>'
                                     + '</table>'
                                       + '</div>';
            document.write(box);
        }
        function ShowInformationMessage(obj) {
            alert(obj)
            return false;
        }

    </script>--%>
<%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
    <%-- <% Response.Flush();%>--%>
    <%--<form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>--%>
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
    </head>
<body runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
    <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <asp:Panel ID="DocNoDetails" runat="server" BorderColor="#3a3a3a" BorderStyle="Double">
                    <table id="txtDocNoDetails" runat="server" class="ContainTable">
                        <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="6">Document Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%"><asp:Label ID="lblClaimTy" runat="server" Text="Claim Type:"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtClaimName" Text="Goodwill Request" Font-Bold="true" runat="server"
                                    CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 15%" class="tdLabel">
                                <asp:Label ID="lblClaimNo" runat="server" Text="Claim No.:"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtClaimNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 15%" class="tdLabel">
                                <asp:Label ID="lblClaimDate" runat="server" Text="Claim Date:"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtClaimDate" Font-Bold="true" runat="server" CssClass="TextBoxForString"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trWarranty">                           
                            <td style="width: 15%" class="tdLabel">Jobcard Confirm Date:
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtJCDate" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 15%" class="tdLabel">WarrantyClaim Confirm Date:
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtWCCDate" Font-Bold="true" runat="server" CssClass="TextBoxForString"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                             <td class="tdLabel" style="width: 15%"><%--DCS Posted Date:--%>
                            </td>
                            <td style="width: 18%">                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%" colspan="2">
                                 <asp:LinkButton ID="lnkOtherClaim" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                onmouseover="SetCancelStyleonMouseOver(this);" Text="Other Claim Print" Width="96%"
                                ToolTip="Other Claim Print"> </asp:LinkButton>
                            </td>
                            <td style="width: 15%" class="tdLabel">
                                <asp:Label ID="lblRequestNo" runat="server" Text="Request No.:"></asp:Label>
                                <asp:Label ID="lblRefClaimNo" runat="server" Text="Ref. Claim No."></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtRequestNo" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                <asp:TextBox ID="txtRefClaimNo" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                    Text=""></asp:TextBox>
                            </td>
                            <td style="width: 15%" class="tdLabel">
                                <asp:Label ID="lblRequestDate" runat="server" Text="Request Date:"></asp:Label>
                                <asp:Label ID="lblRefClaimDate" runat="server" Text="Ref. Claim Date:"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtRequestDate" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                                <asp:TextBox ID="txtRefClaimDate" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%" colspan="2">&nbsp;
                            </td>
                            <td style="width: 15%" class="tdLabel">
                                <asp:Label ID="lblApprovalNo" runat="server" Text="Approval No.:"></asp:Label>                               
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtApprovalNo" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>                              
                            </td>
                            <td style="width: 15%" class="tdLabel">
                                <asp:Label ID="lblApprovalDt" runat="server" Text="Approval Date:"></asp:Label>                              
                            </td>
                            <td style="width: 18%">
                                <asp:TextBox ID="txtApprovalDt" runat="server" CssClass="TextBoxForString" Text=""></asp:TextBox>                             
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PVehicleDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEVehicleDetails" runat="server" TargetControlID="CntVehicleDetails"
                        ExpandControlID="TtlVehicleDetails" CollapseControlID="TtlVehicleDetails" Collapsed="True"
                        ImageControlID="ImgTtlVehicleDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Vehicle Details" ExpandedText="Vehicle Details"
                        TextLabelID="lblTtlVehicleDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlVehicleDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="99%">
                                    <asp:Label ID="lblTtlVehicleDetails" runat="server" Text="Vehicle Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td>
                                    <asp:Image ID="ImgTtlVehicleDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntVehicleDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <table id="tblVehicleDetails" runat="server" class="ContainTable">
                            <tr>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:LinkButton ID="lblServiceHistroy" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Service History" Width="80%" Visible="false"
                                        ToolTip="Service History Details"> </asp:LinkButton>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:LinkButton ID="lblWarrantyHistroy" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Warranty History" Width="80%"  Visible="false"
                                        ToolTip="Warranty History Details" OnClick="lblWarrantyHistroy_Click1"> </asp:LinkButton>
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:LinkButton ID="lnkSelectRequest" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Request Details" Width="80%"  Visible="false"
                                        ToolTip="Request Details"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%" colspan="2">&nbsp;
                                </td>
                                <td class="tdLabel" style="width: 18%;">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:Label ID="lblModelCode" runat="server" Text="Model Code:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtModelCode" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:Label ID="lblModelName" runat="server" Text="Model Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtModelDescription" Text="" runat="server" CssClass="TextBoxForString"
                                        Width="70%"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:Label ID="lblChangeFertCode" CssClass="HideControl" runat="server" Text="Change Model Code:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChangeFertCode" Text="" runat="server" CssClass="HideControl"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 18%;">
                                    <asp:TextBox ID="txtClaimTypeID" runat="server" Text="" Width="5%" CssClass="HideControl"></asp:TextBox>
                                    <asp:TextBox ID="txtRequestID" runat="server" Text="" Width="5%"></asp:TextBox>
                                    <asp:TextBox ID="txtRefClaimID" runat="server" Text="" Width="5%"></asp:TextBox>
                                    <asp:TextBox ID="txtClaimRevNo" runat="server" Text="" Width="5%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:Label ID="lblChassisNo" runat="server" Text="Chassis No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:TextBox ID="txtAggreagateNo" Text="" runat="server" CssClass="TextBoxForString" Visible="false"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%"><asp:Label ID="lblEngineNo" runat="server" Text="Engine No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEngineNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:Label ID="lblChangeFertName" CssClass="HideControl"  runat="server" Text="Change Model Name:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChangeFertName" Text="" runat="server" CssClass="HideControl"
                                        Width="70%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%;">Customer Name:
                                </td>
                                <td style="width: 18%;">
                                    <asp:TextBox ID="txtCustomerName" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">Customer Address:
                                </td>
                                <td style="width: 18%;" colspan="3">
                                    <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="TextBoxForString" Width="94%"
                                        Height="50%" Text="" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%;">Odometer Reading :
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtOdometer" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">Hour Meter Reading:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtHrsReading" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    <%--Route Type:--%>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpRouteType" runat="server" CssClass="ComboBoxFixedSize" Enabled="false" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%;">Vehicle Reg. No.:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtVehicleNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:Label ID="lblVehicleHistory" runat="server" ForeColor="#49A3D3" Text="Vehicle History"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td style="width: 18%">&nbsp;
                                </td>
                                <td class="tdLabel" style="width: 15%">INS Date:</td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtInstallationDate" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblRepairOrderNo" runat="server" Text="Repair Order No."></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRepairOrderNo" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Transporter Name:
                                </td>
                                <td style="width: 18%" colspan="3">
                                    <asp:TextBox ID="txtTransporterName" Width="90%" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblFailureDate" runat="server" Text="Failure Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtFailureDate" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblRepairOrderDate" runat="server" Text="Repair Order Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRepairOrderDate" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    <asp:Label ID="lblRepairComplete" runat="server" Text="Repair Complete Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtRepairCompleteDate" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="ServVan1" runat="server">
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="Label1" runat="server" Text="Service Van From Location"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtFrmLocation" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="Label2" runat="server" Text="Service Van To Location"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtToLocation" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                    
                                </td>
                                <td style="width: 18%">
                                 
                                </td>
                            </tr>
                            <tr id="ServVan2"  runat="server">
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="Label4" runat="server" Text="Service Van Start Kms"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtStKms" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="Label5" runat="server" Text="Service Van End Kms"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEndKms" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">
                                 
                                </td>
                                <td style="width: 18%">
                                   
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PRequestDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="tblRequestDetails" runat="server" class="ContainTable">
                        <tr>
                            <td style="width: 15%" class="tdLabel">Goodwill Type:
                            </td>
                            <td style="width: 50%">
                                <asp:RadioButtonList ID="OptGoodwillType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">Technical Goodwill</asp:ListItem>
                                    <asp:ListItem>Commercial Goodwill </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEComplaints" runat="server" TargetControlID="CntComplaints"
                        ExpandControlID="TtlComplaints" CollapseControlID="TtlComplaints" Collapsed="True"
                        ImageControlID="ImgTtlComplaints" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Customer Complaints/ Incidence of Failure"
                        ExpandedText="Customer Complaints/ Incidence of Failure" TextLabelID="lblTtlComplaints">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlComplaints" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="82%">
                                    <asp:Label ID="lblTtlComplaints" runat="server" Text="Customer Complaints/ Incidence of Failure"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <%--<td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblComplaintsRecCnt" runat="server" Text="0"></asp:Label>
                                </td>--%>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlComplaints" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;" ScrollBars="None">
                        <asp:GridView ID="ComplaintsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                            AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                            GridLines="Horizontal" SkinID="NormalGrid" Width="100%">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Complaints" ItemStyle-Width="95%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtComplaintDesc" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text='<%# Eval("Complaint_Desc") %>' TextMode="MultiLine" Width="90%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PInvestigations" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEInvestigations" runat="server" TargetControlID="CntInvestigations"
                        ExpandControlID="TtlInvestigations" CollapseControlID="TtlInvestigations" Collapsed="True"
                        ImageControlID="ImgTtlInvestigations" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Dealer’s Investigations /Probable Cause"
                        ExpandedText="Dealer’s Investigations /Probable Cause" TextLabelID="lblTtlInvestigations">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlInvestigations" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="82%">
                                    <asp:Label ID="lblTtlInvestigations" runat="server" Text="Dealer’s Investigations /Probable Cause"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <%--<td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblInvestigationsRecCnt" runat="server" Text="0"></asp:Label>
                                </td>--%>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlInvestigations" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntInvestigations" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;" ScrollBars="None">
                        <asp:GridView ID="InvestigationsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Investigations" ItemStyle-Width="95%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtInvestigationDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                            Text='<%# Eval("Investigation_Desc") %>' ReadOnly="true" Width="90%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PParameter" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEParameter" runat="server" TargetControlID="CntParameter"
                        ExpandControlID="TtlParameter" CollapseControlID="TtlParameter" Collapsed="True"
                        ImageControlID="ImgTtlParameter" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Parts/Parameters checked" ExpandedText="Parts/Parameters checked"
                        TextLabelID="lblTtlParameter">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlParameter" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="82%">
                                    <asp:Label ID="lblTtlParameter" runat="server" Text="Parts/Parameters checked" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <%--  <td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblParametrRecCnt" runat="server" Text="0"></asp:Label>
                                </td>--%>
                                <td width="1%">
                                    <asp:Image ID="ImgParametr" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntParameter" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;" ScrollBars="None">
                        <asp:GridView ID="ParameterGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="95%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParameterDesc" ReadOnly="true" runat="server" TextMode="MultiLine"
                                            CssClass="TextBoxForString" Text='<%# Eval("Description") %>' Width="90%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PAction" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEAction" runat="server" TargetControlID="CntAction"
                        ExpandControlID="TtlAction" CollapseControlID="TtlAction" Collapsed="True" ImageControlID="ImgTtlAction"
                        ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png" SuppressPostBack="true"
                        CollapsedText="Interference/Action Taken" ExpandedText="Interference/Action Taken"
                        TextLabelID="lblTtlAction">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlAction" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="82%">
                                    <asp:Label ID="lblTtlAction" runat="server" Text="Interference/Action Taken" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <%-- <td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblActionRecCnt" runat="server" Text="0"></asp:Label>
                                </td>--%>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlAction" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntAction" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;" ScrollBars="None">
                        <asp:GridView ID="ActionGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="95%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtActionDesc" runat="server" TextMode="MultiLine" ReadOnly="true"
                                            CssClass="TextBoxForString" Text='<%# Eval("Description") %>' Width="90%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PJobSummary" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="cpeMyPanelExtender" runat="server" TargetControlID="cntPanel"
                        ExpandControlID="ttlPanel" CollapseControlID="ttlPanel" Collapsed="false" ImageControlID="imgTitlePanel"
                        ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png" SuppressPostBack="true"
                        CollapsedText="Job Summary" ExpandedText="JobSummary" TextLabelID="lblTitleJob">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="ttlPanel" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="99%">
                                    <asp:Label ID="lblTitleJob" runat="server" Text="Job Summary" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                        onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td>
                                    <asp:Image ID="imgTitlePanel" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="cntPanel" runat="server" Style="overflow: hidden;">
                        <div align="center">
                            <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="67%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" ShowFooter="false" OnRowCommand="DetailsGrid_RowCommand">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Job Code ID" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobCodeID" runat="server" Text='<%# Eval("job_code_ID") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job Code" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkJobCode" runat="server" Text='<%# Eval("Job_Code") %>' Font-Bold="true"
                                                OnClientClick="return ShowJobDetails(this);"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>' Font-Bold="true" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="Total:" Font-Bold="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Amount" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                        ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtClaimAmount" runat="server" CssClass="GridTextBoxForAmount" Width="98%"
                                                ReadOnly="true" Text='<%# Eval("ClaimAmount","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalClaimAmount" runat="server" Font-Bold="true" />
                                        </FooterTemplate>
                                        <FooterStyle Width="10%" />
                                        <ItemStyle CssClass="LabelRightAlign" Width="10%" />
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Deduction (%)" ItemStyle-Width="12%" FooterStyle-Width="10%"
                                                ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl" FooterStyle-CssClass="HideControl"  >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDeduction_Percentage" runat="server" CssClass="GridTextBoxForAmount"
                                                        ReadOnly="true" Width="98%" Text='<%# Eval("Deduction_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalDeduction_Percentage" runat="server" Font-Bold="true" />
                                                </FooterTemplate>
                                                <FooterStyle Width="10%" />
                                                <ItemStyle CssClass="LabelRightAlign" Width="12%" />
                                            </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Deducted Amount" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                        ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeducted_Amount" runat="server" CssClass="GridTextBoxForAmount"
                                                ReadOnly="true" Width="98%" Text='<%# Eval("Deducted_Amount","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalDeducted_Amount" runat="server" Font-Bold="true" />
                                        </FooterTemplate>
                                        <FooterStyle Width="10%" />
                                        <ItemStyle CssClass="LabelRightAlign" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accepted Amount" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                        ItemStyle-CssClass="LabelRightAlign" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccepted_Amount" runat="server" CssClass="GridTextBoxForAmount"
                                                ReadOnly="true" Width="98%" Text='<%# Eval("Accepted_Amount","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalAccepted_Amount" runat="server" Font-Bold="true" />
                                        </FooterTemplate>
                                        <FooterStyle CssClass="HideControl" Width="10%" />
                                        <ItemStyle CssClass="HideControl" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Auto Deduction Amount" ItemStyle-Width="12%" FooterStyle-Width="10%"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAutoDeductAmt" runat="server" CssClass="GridTextBoxForAmount"
                                                ReadOnly="true" Width="98%" Text='<%# Eval("AutoDeduct_Amount","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalAutoDeductAmt" runat="server" Font-Bold="true" />
                                        </FooterTemplate>
                                        <FooterStyle Width="10%" />
                                        <ItemStyle CssClass="LabelRightAlign" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Accepted Amount" ItemStyle-Width="12%" FooterStyle-Width="8%"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFinalAcceptedAmt" runat="server" CssClass="GridTextBoxForAmount"
                                                ReadOnly="true" Width="98%" Text='<%# Eval("FinalAccepted_Amount","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblFinalAcceptedAmt" runat="server" Font-Bold="true" />
                                        </FooterTemplate>
                                        <FooterStyle Width="8%" />
                                        <ItemStyle CssClass="LabelRightAlign" Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                            <table id="tblTotalMain" runat="server" width="67%" class="datatable" cellspacing="2"
                                border="0">
                                <tr>
                                    <%--<td style="width: 1%;">
                                            &nbsp;
                                            </td>
                                            <td style="width: 3%;">
                                            &nbsp;
                                            </td>--%>
                                    <td style="width: 12%">
                                        <asp:Label ID="lblTotal" runat="server" Text="Total:" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 9px">
                                        <asp:TextBox ID="txtTotalClaimAmount" Text="" Font-Bold="true" runat="server" CssClass="GridTextBoxForAmount"
                                            ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                    <%--<td class="LabelRightAlign" style="width: 12%">
                                                <asp:TextBox ID="txtTotalDeduction_Percentage" Text="" Font-Bold="true" runat="server"
                                                    CssClass="GridTextBoxForAmount" ReadOnly="true" Width="96%"></asp:TextBox>
                                            </td>--%>
                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 8px">
                                        <asp:TextBox ID="txtTotalDeducted_Amount" Text="" Font-Bold="true" runat="server"
                                            CssClass="GridTextBoxForAmount" ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 8%; padding-right: 4px; display: none" id="tdautoDeduction" runat="server">
                                        <asp:TextBox ID="txtTotalAccepted_Amount" Text="" Font-Bold="true" runat="server"
                                            CssClass="GridTextBoxForAmount" ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 8px">
                                        <asp:TextBox ID="txtTotalAutoDeduction" Text="" Font-Bold="true" runat="server" CssClass="GridTextBoxForAmount"
                                            ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 5px">
                                        <asp:TextBox ID="txtTotalFinalAcceptedAmt" Text="" Font-Bold="true" runat="server"
                                            CssClass="GridTextBoxForAmount" ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                            <table id="tblTotalMainExport" runat="server" width="67%" class="datatable" cellspacing="2"
                                border="0">
                                <tr>
                                    <%--<td style="width: 1%;">
                                            &nbsp;
                                            </td>
                                            <td style="width: 3%;">
                                            &nbsp;
                                            </td>--%>
                                    <td style="width: 12%">
                                        <asp:Label ID="lblTotalExp" runat="server" Text="Total:" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 9px">
                                        <asp:TextBox ID="txtTotalClaimAmountExp" Text="" Font-Bold="true" runat="server" CssClass="GridTextBoxForAmount"
                                            ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 8px">
                                        <asp:TextBox ID="txtTotalDeducted_AmountExp" Text="" Font-Bold="true" runat="server"
                                            CssClass="GridTextBoxForAmount" ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                    <td class="LabelRightAlign" style="width: 8%; padding-right: 4px; display: none" id="td1" runat="server">
                                        <asp:TextBox ID="txtTotalAccepted_AmountExp" Text="" Font-Bold="true" runat="server"
                                            CssClass="GridTextBoxForAmount" ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>

                                    <td class="LabelRightAlign" style="width: 9%; padding-right: 5px">
                                        <asp:TextBox ID="txtTotalFinalAcceptedAmtExp" Text="" Font-Bold="true" runat="server"
                                            CssClass="GridTextBoxForAmount" ReadOnly="true" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <%-- Sujata 12012011 <asp:Panel ID="PFileAttchDetails"  CssClass="DispalyNon"  runat="server" BorderColor="DarkGray" BorderStyle="Double" Sujata 12012011 --%>
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
                                <%--  <td class="ContaintTableHeader" width="8%">
                                    Count:
                                </td>
                                <td class="ContaintTableHeader" width="8%">
                                    <asp:Label ID="lblFileAttachRecCnt" runat="server" Text=""></asp:Label>
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
                                        GridLines="Horizontal" OnRowDataBound="FileAttchGrid_RowDataBound" HeaderStyle-Wrap="true"
                                        SkinID="NormalGrid" Width="100%">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
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
                                            <asp:TemplateField HeaderText="User File Description" ItemStyle-Width="50%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description") %>'
                                                        Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFile" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                        onmouseover="SetCancelStyleonMouseOver(this);" onClick="return ShowAttachDocument(this);"
                                                        Text='<%# Eval("File_Names") %>' ToolTip="Click Here To Open The File" Width="90%"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="CreatedUserRole" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                    ItemStyle-CssClass="HideControl">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCrUserRole" runat="server" Text='<%# Eval("CreatedUserRole") %>' Width="1"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            <%-- Sujata 24012011--%>
                                            <%--<asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);"
                                                                Text="Delete" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" />
                                                    </asp:TemplateField>--%>
                                            <%--Sujata 24012011--%>
                                        </Columns>
                                        <HeaderStyle Wrap="True" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <%--sujata 02062011 Remove commented code to upload a file from RSM User for Domestic Goodwill Claim Request--%>
                            <%--sujata 12012011 --%>
                            <tr id="trNewAttachment" runat="server">
                                <td class="tdLabel" style="width: 50%" align="center">User File Description
                                </td>
                                <td class="tdLabel" style="width: 50%" align="center">File Name
                                </td>
                            </tr>
                            <tr id="trNewAttachment1" runat="server">
                                <td colspan="2" class="tdLabel">
                                    <div id="upload1">
                                        <input id="Text1" type="text" name="Text1" class="TextBoxForString" style="width: 50%" placeholder="User File Description"/>
                                        <input id="AttachFile" type="file" runat="server" style="width: 45%" class="Cntrl1"
                                            onblur="return addFileUploadBox(this);" />
                                    </div>
                                </td>
                            </tr>
                            <%--sujata 12012011--%>
                            <%--sujata 02062011 Remove commented code to upload a file from RSM User for Domestic Goodwill Claim Request--%>
                        </table>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PRemarkDetails" runat="server">
                    <cc1:CollapsiblePanelExtender ID="CPERemarkDetails" runat="server" TargetControlID="CntRemarkDetails"
                        ExpandControlID="TtlRemarkDetails" CollapseControlID="TtlRemarkDetails" Collapsed="True"
                        ImageControlID="ImgTtlRemarkDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Remark Details" ExpandedText="Remark Details"
                        TextLabelID="lblTtlRemarkDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlRemarkDetails" runat="server">
                        <table width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader" width="99%">
                                    <asp:Label ID="lblTtlRemarkDetails" runat="server" Text="Remark Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td>
                                    <asp:Image ID="ImgTtlRemarkDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="CntRemarkDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <table width="100%">
                            <tr class="ContainTable">
                                <td style="padding-left: 10px">
                                    <table id="tblRemark" runat="server" width="80%" class="NormalTable">
                                        <tr id="trCSMRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">MSE Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtCSMRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trASMRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">ASM Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtASMRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trRSMRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">RSM Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtRSMRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trHeadRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">Head Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtHeadRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trSHQRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">SHQ Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtSHQRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trSHQRRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">SHQ Resource Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtSHQRRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trHeadRetailRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">Head Retail Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtHeadRetailRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trHeadSaleMkgAfterMkgRemark" runat="server">
                                            <td class="tdLabel" style="width: 15%">Head-Sales Marketing & After Marketing Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtHeadSaleMkgAfterMkgRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trSAResourceRemark" runat="server" style="display: none">
                                            <td class="tdLabel" style="width: 15%">SA Resource Remark:
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="txtSAResourceRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                    TextMode="MultiLine" Height="50%" Width="96%" onkeyup="return ToCheckTextLength(this,500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="CommandButton" OnClick="btnSave_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" ToolTip="Submit" OnClientClick=" return CheckWCBeforeSubmit();"
                        CssClass="CommandButton" OnClick="btnSubmit_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="CommandButton"
                        OnClientClick="return CheckBeforeApproveRecord();" OnClick="btnApprove_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="CommandButton"
                        OnClick="btnReject_Click" /><%--OnClientClick="return CheckRejectReturnApplicable('J');"--%>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="CommandButton"
                        OnClick="btnReturn_Click" /><%--OnClientClick="return CheckRejectReturnApplicable('R');"--%>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="CommandButton" OnClick="btnBack_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="CommandButton" OnClick="btnPrint_Click" />

                </asp:Panel>
                <asp:Panel ID="panReason" runat="server" Style="display: none">
                    <asp:DropDownList ID="drpReason" runat="server" CssClass="ComboBoxFixedSize">
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Label ID="lblRejectOrConfirm" runat="server" Text="N" CssClass="DispalyNon"></asp:Label>
                    <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="CommandButton" OnClientClick="return CheckBeforeRecord(this);"
                        OnClick="btnOK_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="CommandButton"
                        OnClick="btnCancel_Click" />
                </asp:Panel>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" ></asp:TextBox>
                <asp:TextBox ID="txtPartAmount" runat="server" Width="1px"> </asp:TextBox>
                <asp:TextBox ID="txtLabourAmount" runat="server"> </asp:TextBox>
                <asp:TextBox ID="txtLubricantAmount" runat="server"> </asp:TextBox>
                <asp:TextBox ID="txtSubletAmount" runat="server"> </asp:TextBox>
                <asp:TextBox ID="txtClaimAmt" runat="server"> </asp:TextBox>
                <asp:TextBox ID="txtRequestOrClaim" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtDomestic_Export" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtchassisID" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <%--Sujata 12012011--%>
                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"  Width="96%"></asp:TextBox>
                <asp:TextBox ID="txtDealerAID" runat="server" CssClass="HideControl"  Width="96%"></asp:TextBox>
                <asp:TextBox ID="txtfinalAppUserRole" runat="server" CssClass="HideControl" Text=""
                    Width="96%"></asp:TextBox>
                <asp:HiddenField ID="hdnIsSHQResource" runat="server" Value="" />
                <asp:HiddenField ID="hdnIsSHQ" runat="server" Value="" />
                <asp:HiddenField ID="hdnJobCode" runat="server" Value="" />
                <asp:HiddenField ID="hdnDealerID" runat="server" Value="" />
                <asp:HiddenField ID="hdnRejectCount" runat="server" Value="" />
                <asp:HiddenField ID="hdnReturnCount" runat="server" Value="" />
                <asp:HiddenField ID="hdnCurrency" runat="server" Value="" />
                <asp:HiddenField ID="hdnIsAggregate" runat="server" Value="N" />
                <asp:Label ID="lblFileAttachRecCnt" CssClass="HideControl" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblActionRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                <asp:Label ID="lblParametrRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                <asp:Label ID="lblInvestigationsRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                <asp:Label ID="lblComplaintsRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                <asp:TextBox ID="txtInvoiceNo" Text="" runat="server" CssClass="TextBoxForString HideControl"></asp:TextBox>
                <asp:TextBox ID="txtInvoiceDate" Text="" runat="server" CssClass="TextBoxForString HideControl"></asp:TextBox>
                <asp:TextBox ID="txtDCSPostedDate" Text="" Font-Bold="true" runat="server" CssClass="TextBoxForString HideControl" ReadOnly="true"></asp:TextBox>
                <asp:HiddenField ID="hdnISDocGST" runat="server" /> 
                <asp:HiddenField ID="hdnInvType" runat="server" /> 
                <asp:HiddenField ID="hdnServVanID" runat="server" />                 
                <asp:HiddenField ID="hdnOtherClaimId" runat="server" />                 
                <%--Sujata 12012011--%>
            </td>
        </tr>
    </table>
 </form>
</body>
    </html>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <%-- </form>--%>
    <%-- <% Response.Flush();%>+--%>

    <%--<script type="text/javascript" language="javascript">
        if (upLevel) {
            var splash = document.getElementById("splashScreen");
        }
        else if 
                                (ns4) {
            var splash = document.splashScreen;
        }
        else if (ie4) {
            var splash = document.all.splashScreen;
        }
        hideObject(splash);
    </script>--%>
<%--</asp:Content>--%>
