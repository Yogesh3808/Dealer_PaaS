<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmJobcodeDtls.aspx.cs" Inherits="MANART.Forms.Service.frmJobcodeDtls" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery-1.11.1.js"></script>
    <script src="../../Scripts/jsWCFileAttach.js"></script>
    <script src="../../Scripts/jsWCServiceHistory.js"></script>
    <%--<script src="../../Scripts/jsFileAttach.js"></script>--%>
    <script type="text/javascript">
        //function MaxLength1(maxLength) {
        //    debugger;
        //    text = document.getElementById('txtDtlDesc');
        //    if (text.value.length > maxLength) {
        //        alert("only max " + maxLength + " characters are allowed");
        //        //this limits the textbox with only 5 characters as lenght is given as 5.
        //        text.value = text.value.substring(0, maxLength);
        //    }
        //}
        function checkTextAreaMaxLength(textBox, e, length) {
            //debugger;
            text = document.getElementById('txtDtlDesc');
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (textBox.value.length > maxLength - 1) {
                alert("only max " + maxLength + " characters are allowed");
                //this limits the textbox with only 250 characters as lenght is given as 250.
                textBox.value = text.value.substring(0, length-1);
            }
        }
        
    </script>
    <script>
        function ShowAttachDocument(objFileControl) {
            // debugger;
            var objRow = objFileControl.parentNode.parentNode.childNodes;
            var sFileName = '';
            var sUserType = '';
            popht = 3; // popup height
            popwth = 3; // popup width
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
        function CloseMe() {
            window.close();
        }

        function ReturnPCRValue() {
            //debugger;
            var ObjControl = window.document.getElementById("txtID");
            var sValue;
            sValue = ObjControl.value;
            window.returnValue = sValue;
            window.close();

        }

        function ShowPartMaster(objNewPartLabel, sDealerId) {
            debugger;
            var PartDetailsValue;
            //var sSelectedPartID = GetPreviousSelectedPartIDInJobs(objNewPartLabel);
            var sSelectedPartID = "";
            PartDetailsValue = window.showModalDialog("../Common/frmSelectPart.aspx?DealerID=" + sDealerId + "&SelectedPartID=" + sSelectedPartID + "&Superceded=N", "List", "dialogWidth:700px;dialogHeight:380px;status:no;help:no;scrollbars:no;resizable:no;");
            if (PartDetailsValue != null) {
                debugger;
                var ObjtxtPrmPartSupplier = window.document.getElementById("txtPrmPartSupplier");
                var ObjhdnPrmPartSupplier = window.document.getElementById("hdnPrmPartSupplier");

                ObjhdnPrmPartSupplier.value = PartDetailsValue[1];
                //SetPartNo && SetPartName
                ObjtxtPrmPartSupplier.value = "(" + PartDetailsValue[2] + ") " + PartDetailsValue[3];
            }
        }        
    </script>
    <script type="text/javascript">
        function ShowReport_PCR(obj, strReportpath, iUserRoleId) {
            debugger;
            var iDocId = 0;
            var sExportYesNo = "";
            var RptTitle = "";
            var msgAnswer = "";
            //var iUserRoleId = 0;

            var Control = document.getElementById('txtID');

            if (Control == null) return;
            if (Control.value == "") {
                alert("Please Select The Record For Print!");
                return false;
            }
            else {
                iDocId = Control.value;
            }
            // var Url = "/DCS/Forms/Common/frmDocumentView1.aspx?RptName=/DCSReports";
            //var strReportpath;
            var Url = strReportpath;
            // var Url = "/../Common/frmDocumentView1.aspx?RptName=/MANARTREPORT";

            var sReportName = "";

            //if (confirm("Are you sure, you want to Print the Report?") == true) {
            sReportName = "/rptPCR&";  //+ strReportName + "&";
            sExportYesNo = "F";
            //    sFPDAYesNo = "";
            //    sExportYesNo = ""
            //}
            //else {
            //    return false;
            //}

            if (sReportName == "") {
                return false;
            }
            //Url = Url + sReportName + "ID=" + iDocId + "&FPDAYesNo=" + sFPDAYesNo + "&UserRoleId=" + iUserRoleId + "&ExportYesNo=" + sExportYesNo + "";
            Url = Url + sReportName + "ID=" + iDocId + "&UserRoleId=" + iUserRoleId + "&ExportYesNo=" + sExportYesNo + "";

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
    <style type="text/css">
        .auto-style1 {
            font-size: smaller;
        }

        .auto-style2 {
            width: 9%;
            height: 35px;
        }

        .auto-style3 {
            width: 14%;
            height: 35px;
        }
    </style>
    <base target="_self" />
</head>

<body runat="server">
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
                <td style="width: 9%" colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save PCR" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnSave_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit PCR" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnApprove" runat="server" Text="Approve PCR" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnApprove_Click" />
                    <asp:Button ID="btnReject" runat="server" Text="Return PCR" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnReject_Click" />
                    <asp:Button ID="btnRReject" runat="server" Text="Reject PCR" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnRReject_Click"  />
                </td>
                <td class="tdLabel" style="width: 14%;" colspan="2">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-search btn-sm"
                        OnClientClick="ReturnPCRValue();"
                        OnClick="btnBack_Click"></asp:Button>
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnPrint_Click" />
                    <asp:LinkButton ID="lblServiceHistroy" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Service History" Width="20%"
                        ToolTip="Service History Details"> </asp:LinkButton>
                </td>

            </tr>
            <tr id="TblControl">
                <td style="width: 9%; font-weight: 700;">PCR No :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDocNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>

                <td style="width: 9%">PCR Date :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <uc3:CurrentDate ID="txtDocDate" runat="server" Mandatory="true" bCheckforCurrentDate="true" />
                </td>

            </tr>

            <tr>

                <td class="auto-style2">24x7 CS Call Ticket Nr.</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtCRMTicketNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td class="auto-style2">Customer name :</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtCustName" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">DMS Job Card Nr.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtJobNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Customer loc.:</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtCustLoc" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Warranty Claim Nr.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtWarrClaimNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Veh. Regn. Nr. :</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtVehRegNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%" colspan="4"></td>
            </tr>
            <tr>
                <td style="width: 9%">Productline</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtProduct" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Dealer code:</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDlrCode" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%"><asp:Label ID="lblChassisNo" runat="server" Text="Chassis No"  Width="96%"></asp:Label></td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtVIN" runat="server" Width="96%" CssClass="TextBoxForString"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>

                    <asp:TextBox ID="txtAggreagateNo" runat="server"  Width="96%" CssClass="TextBoxForString"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Dealer name</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDlrName" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Vehicle model</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtModel" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Dealer loc.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDlrLoc" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Engine Nr.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtEngNr" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Sales Region</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:DropDownList ID="drpSalesRegID" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" Font-Bold="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Engine type</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtEndType" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Inspected by</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:DropDownList ID="DrpSupervisorName" runat="server" CssClass="ComboBoxFixedSize" Font-Bold="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 9%;">Sale Date :
                </td>
                <td class="tdLabel" style="width: 17%;">
                    <asp:TextBox ID="txtChassisSaleDate" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%"></td>
                <td class="tdLabel" style="width: 14%;"></td>
            </tr>
            <tr>
                <td style="width: 9%" colspan="4"></td>
            </tr>
            <tr>
                <td style="width: 9%">Primary failed part nr.</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtPrmPart" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Failure date</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtFailDate" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">
                    <asp:Label  ID="lblChngPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                    onmouseover="SetCancelStyleonMouseOver(this);" Text="Pr. failed part supplier" ToolTip="Click Here To Change the Part"
                    Width="70%"></asp:Label>
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtPrmPartSupplier" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%">Failure km</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtFailKm" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Pr. failed part batch id</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtPrmPartBatchID" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="20" Font-Bold="False"></asp:TextBox>
                </td>
                <td style="width: 9%">Failure hr</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtFailHr" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td style="width: 9%" colspan="4"></td>
            </tr>
            <tr>

                <td style="width: 9%">Defect Category</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDefectCat" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
                <td style="width: 9%" colspan="2" rowspan="11" valign="Top">
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPInv" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PServices" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEServices" runat="server" TargetControlID="CntServices"
                                    ExpandControlID="TtlServices" CollapseControlID="TtlServices" Collapsed="true"
                                    ImageControlID="ImgTtlServices" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Service history"
                                    ExpandedText="Service history" TextLabelID="lblTtlServices">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlServices" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlServices" runat="server" Text="Service history"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlServices" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntServices" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="ServicesGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtServiceName" runat="server" ReadOnly="true" Text='<%#  Eval("SrvNo") %>' CssClass="TextBoxForString"
                                                        Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtServiceDate" runat="server" ReadOnly="true" Text='<%#  Eval("SrvDate") %>' CssClass="TextBoxForString"
                                                        Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Kms">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtServiceKms" runat="server" ReadOnly="true" Text='<%#  Eval("Kms","{0:#0}") %>' CssClass="TextBoxForString"
                                                        Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hrs">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtServiceHrs" runat="server" ReadOnly="true" Text='<%#  Eval("Hrs","{0:#0}") %>' CssClass="TextBoxForString"
                                                        Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRInv" runat="server" AssociatedUpdatePanelID="UPInv">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">MAN Defect Code</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDefectCode" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">MAN Defect Narrative</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDefectDesc" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%" colspan="2"></td>
            </tr>
            <tr>
                <td style="width: 9%">Vehicle Application</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtVehAppl" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="20" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Nature of load carried</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtNatLoadCar" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="30" Font-Bold="False" Font-Italic="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Payload (in tonnes)</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtPayLoad" runat="server" CssClass="GridTextBoxForAmount" onkeypress="return CheckForTextBoxValue(event,this,'5');" Width="96%"
                        MaxLength="3" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Avg run per day</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtAvgPerDay" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="10" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Road condition</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtRoadCond" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="20" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Fuel consumption</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtFuelCons" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="10" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Eng. oil consumption</td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtEngOilCons" CssClass="TextBoxForString" runat="server" Width="96%"
                        MaxLength="10" Font-Bold="False"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td style="width: 9%" colspan="4">
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPCompl" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="">
                                <cc1:CollapsiblePanelExtender ID="CPEComplaints" runat="server" TargetControlID="CntComplaints"
                                    ExpandControlID="TtlComplaints" CollapseControlID="TtlComplaints" Collapsed="True"
                                    ImageControlID="ImgTtlComplaints" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Customer Complaints/ Incidence of Failure"
                                    ExpandedText="Customer Complaints/ Incidence of Failure" TextLabelID="lblTtlComplaints">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlComplaints" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlComplaints" runat="server" Text="Customer Complaints/ Incidence of Failure" ForeColor="White"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlComplaints" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntComplaints" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="ComplaintsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Complaints">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNewComplaintDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                        Width="90%" ReadOnly="true" Text='<%#  Eval("Complaint_Desc") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRCompl" runat="server" AssociatedUpdatePanelID="UPCompl">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Detailed Description <em><span class="auto-style1">In case of aggregate failures, mention the Aggregate serial number. Attach a photograph of the name/number plate in the &#39;Photos&#39; sheet.</span></em></td>
                <td class="tdLabel" style="width: 14%; vertical-align: top" colspan="3">
                    <asp:TextBox ID="txtDtlDesc" runat="server" Width="96%" MaxLength="500" Rows="10"
                        onblur="checkTextAreaMaxLength(this,event,'500');"
                        TextMode="MultiLine"></asp:TextBox>
                    <%--onchange="MaxLength1(5)  onkeyDown="checkTextAreaMaxLength(this,event,'250');""--%> 
                    
                </td>
            </tr>
            <tr>
                <td style="width: 9%" colspan="4">
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPAction" runat="server" ChildrenAsTriggers="true">
                        <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkNew" EventName="onclick" />
                    </Triggers>--%>
                        <ContentTemplate>
                            <asp:Panel ID="PActions" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEActions" runat="server" TargetControlID="CntActions"
                                    ExpandControlID="TtlActions" CollapseControlID="TtlActions" Collapsed="true"
                                    ImageControlID="ImgTtlActions" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Dealer’s Actions"
                                    ExpandedText="Dealer’s Actions" TextLabelID="lblTtlActions">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlActions" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="panel-title">
                                                <asp:Label ID="lblTtlActions" runat="server" Text="Dealer’s Actions /Probable Cause"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlActions" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntActions" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    Style="display: none;" ScrollBars="None">
                                    <asp:GridView ID="ActionsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNewActionDesc" runat="server" TextMode="MultiLine" CssClass="TextBoxForString"
                                                        Width="90%" ReadOnly="true" Text='<%#  Eval("Dealer_Action") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPAction">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr class="HideControl">
                <td style="width: 9%">Observations by After Sales MSE</td>
                <td class="tdLabel" style="width: 14%;vertical-align:top" colspan="3">
                    <asp:TextBox ID="txtObservMSE"  runat="server" Width="96%"
                         onblur="checkTextAreaMaxLength(this,event,'250');" Rows="3"
                        TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Observations by After Sales ASM <span class="auto-style1">if case is escalated</span></td>
                <td class="tdLabel" style="width: 14%;vertical-align:top" colspan="3">
                    <asp:TextBox ID="txtObservASE"  runat="server" Width="96%"
                        onblur="checkTextAreaMaxLength(this,event,'250');" Rows="3"
                        TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr class="HideControl">
                <td style="width: 9%">Observations by After Sales RSM <span class="auto-style1">if case is escalated</span></td>
                <td class="tdLabel" style="width: 14%;vertical-align:top" colspan="3">
                    <asp:TextBox ID="txtObservRSM"  runat="server" Width="96%"
                        onblur="checkTextAreaMaxLength(this,event,'250');" Rows="3"
                        TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Observations by After Sales Head <span class="auto-style1">if case is escalated</span></td>
                <td class="tdLabel" style="width: 14%;vertical-align:top" colspan="3">
                    <asp:TextBox ID="txtObservHead"  runat="server" Width="96%"
                        onblur="checkTextAreaMaxLength(this,event,'250');" Rows="3"
                        TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
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
                            <table id="Table2" runat="server" class="ContainTable">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                            AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                            GridLines="Horizontal" HeaderStyle-Wrap="true" DataKeyNames="File_Names"
                                            SkinID="NormalGrid" Width="100%">
                                            <%--OnRowCommand="DetailsGrid_RowCommand"--%>
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
                                                        <asp:Label ID="lblFile" runat="server" ForeColor="#49A3D3" onClick="return ShowAttachDocument(this);"
                                                            onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"
                                                            Text='<%# Eval("File_Names") %>' ToolTip="Click Here To Open The File" Width="90%"></asp:Label>
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
                                <tr id="trNewAttachment" runat="server">
                                    <td class="tdLabel" style="width: 50%" align="center">User File Description
                                    </td>
                                    <td class="tdLabel" style="width: 50%" align="center">File Name
                                    </td>
                                </tr>
                                <tr id="trNewAttachment1" runat="server">
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
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPartID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDefectID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerBrID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtJobCodeID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:Label ID="lblComplaintsRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblActionsRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblServicesRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                    <asp:TextBox ID="txtIsSubmit" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtApprove" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtReject" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>

                    <asp:TextBox ID="txtSubmitASM" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtApproveASM" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtRejectASM" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>

                    <asp:TextBox ID="txtSubmitRSM" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtApproveRSM" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtRejectRSM" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>

                    <asp:TextBox ID="txtSubmitHead" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtApproveHead" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtRejectHead" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>

                    <asp:TextBox ID="txtReportChassisID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>

                    <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>

                    <asp:TextBox ID="txtPCRReject" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:HiddenField ID="hdnMSERetCnt" runat="server" />
                    <asp:HiddenField ID="hdnHeadRetCnt" runat="server" />
                    <asp:HiddenField ID="hdnPrmPartSupplier" runat="server" />
                </td>
            </tr>

        </table>
    </form>
</body>
</html>



