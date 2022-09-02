<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCouponClaimProcessing.aspx.cs" Title="MTI-Coupon Claim Processing"
    Theme="SkinFile" EnableEventValidation="false" EnableViewState="true" Inherits="MANART.Forms.Coupon.frmCouponClaimProcessing" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <%--<script src="../../Scripts/jsCouponFileAttach.js"></script>--%>
    <script src="../../Scripts/jsCouponClaimProcessing.js"></script>
    <script src="../../Scripts/jsFileAttach.js"></script>
    <script>
        function ShowAttachDocument(objFileControl) {
            //debugger;
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
        //function ShowReport_Proforma(obj, strReportpath) {
        function ShowReport_Proforma(Object, strReportpath, sJobcard_HDR_ID) {
            //debugger;
            //alert(sJobcard_HDR_ID);
            //alert(strReportpath);
            var iDocId = 0;
            var sExportYesNo = "";

            var Control = sJobcard_HDR_ID;
            if (Control == null) return;
            if (Control.value == "") {
                alert("Please Select The Record For Print!");
                return false;
            }
            else {
                iDocId = Control;
            }
            var Url = strReportpath;
            var sReportName = "";

            //if (confirm("Are you sure, you want to Print the Report?") == true) {
            sReportName = "/rptJobcardprintingNew&";  //+ strReportName + "&";
            sExportYesNo = "Y";
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
    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
            });
        }


        <%--$("#<%=CouponClaimGrid.ClientID%> input[id*='ChkPart']:checkbox").click(SelectDeleteCheckboxForLabour(this));
        //debugger;
        $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkAllPart']:checkbox").click(function () {
            if ($(this).is(':checked')) {
                $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkPart']:checkbox").attr('checked', true);
                    $("#CouponClaimGrid input[id*='txtRejRemark']:input")[0].disabled = true;
                    $("#CouponClaimGrid input[id*='txtRejRemark']:input")[0].innerText = "";
                }
                else {
                    $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkPart']:checkbox").attr('checked', false);
                    $("#CouponClaimGrid input[id*='txtRejRemark']:input")[0].disabled = false;
                }
            });--%>
    

    <%--function CheckUncheckAllCheckBoxAsNeeded() {
        var totalCheckboxes = $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkPart']:checkbox").size();
        var checkedCheckboxes = $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkPart']:checkbox:checked").size();
        debugger;        
        if (totalCheckboxes == checkedCheckboxes) {
            $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkAllPart']:checkbox").attr('checked', true);
            $("#CouponClaimGrid input[id*='txtRejRemark']:input")[0].innerText = "";
            $("#CouponClaimGrid input[id*='txtRejRemark']:input")[0].disabled = true;            
        }
        else {
            $("#<%=CouponClaimGrid.ClientID%> input[id*='ChkAllPart']:checkbox").attr('checked', false);
            $("#CouponClaimGrid input[id*='txtRejRemark']:input")[0].disabled = false;
        }
    }--%>

        function SelectDeleteCheckboxForLabour(ObjChkDelete) {

            var objRow = ObjChkDelete.parentNode.parentNode.parentNode;

            debugger;
            if (ObjChkDelete.checked) {
                objRow.children[16].childNodes[1].innerText = "";
                objRow.children[16].childNodes[1].disabled = true;
            }
            else {
                objRow.children[16].childNodes[1].disabled = false;
            }

            var ObjGrid;
            var iRecordCnt = 0, iRecordCntSel = 0, iRecordCntNtSel = 0;
            ObjGrid = document.getElementById("CouponClaimGrid");
            if (ObjGrid == null) return;
            for (var i = 1; i < ObjGrid.rows.length; i++) {
                //Set iLabourID
                iRecordCnt = iRecordCnt + 1;
                if (ObjGrid.rows[i].childNodes[16].childNodes[1].childNodes[0].checked) {
                    iRecordCntSel = iRecordCntSel + 1;
                }
                else {
                    iRecordCntNtSel = iRecordCntNtSel + 1;
                }
            }
            if (iRecordCntSel == iRecordCnt)
                ObjGrid.rows[0].childNodes[16].childNodes[1].childNodes[0].childNodes[0].checked = true;
            else
                ObjGrid.rows[0].childNodes[16].childNodes[1].childNodes[0].childNodes[0].checked = false;
        }

        function SetLabourRecordCount(ObjChkAllDelete) {
            var ObjGrid;
            var iRecordCnt = 0;
            ObjGrid = document.getElementById("CouponClaimGrid");
            if (ObjGrid == null) return;
            debugger;
            for (var i = 1; i < ObjGrid.rows.length; i++) {
                //Set iLabourID
                if (ObjChkAllDelete.checked) {
                    ObjGrid.rows[i].childNodes[16].childNodes[1].childNodes[0].checked = true;
                    ObjGrid.rows[i].childNodes[17].childNodes[1].innerText = "";
                    ObjGrid.rows[i].childNodes[17].childNodes[1].disabled = true;
                }
                else {
                    ObjGrid.rows[i].childNodes[16].childNodes[1].childNodes[0].checked = false;
                    ObjGrid.rows[i].childNodes[17].childNodes[1].disabled = false;
                }
            }
        }

    </script>

    <base target="_self" />

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
    <script type="text/javascript" language="javascript">
        function checkCouponSelectStatus() {
            var checkedCheckboxesCount = $("#CouponClaimGrid input[id*='ChkPart']:checkbox:checked").size();
            return true;
            //if (checkedCheckboxesCount == 0) {
            //    alert('Please Select Coupon.');
            //    return false;

            //}

        }

        function CheckBeforeConfirmRecord() {
            // //debugger;
            //if (checkCouponSelectStatus() == false) return false;
            //   if (checkCouponSelectStatus() == true)
            //   {
            if (confirm("Are you sure, you want to Confirm the Record?") == true) {
                return true;
            }
            else {
                return false;
            }
            //}
            //else{
            //  return false;
            //   }
        }
    </script>

    <script type="text/javascript" language="javascript">
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

    </script>


</head>
<body>
    <p>
        `
    </p>
    <% Response.Flush();%>
    <form id="form2" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 14%">
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="#3a3a3a" BorderStyle="Double">
                        <table id="txtDocNoDetails" runat="server" class="ContainTable">
                            <tr>
                                <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="4">Coupon Claim Header Details
                                </td>
                            </tr>
                            <tr>

                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lbldlrcode" runat="server" Text="Dealer Code.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDlrcode" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblDlrName" runat="server" Text="Dealer Name.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDlrName" Text="" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true" Width="271px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblClaimNo" runat="server" Text="Coupon Claim No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCouponClaimNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblCouponClaimDate" runat="server" Text="Coupon Claim Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCouponClaimDate" Font-Bold="true" runat="server" CssClass="TextBoxForString"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblApprovalNo" runat="server" Text="Approval Inv No.:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprNo" Text="" runat="server" CssClass="TextBoxForString" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblApprovalDt" runat="server" Text="Approval Inv Date:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtApprDt" Font-Bold="true" runat="server" CssClass="TextBoxForString"
                                     ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PCouponClaimGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPECouponClaimGrid" runat="server" TargetControlID="CntCouponClaimGrid"
                            ExpandControlID="TtlCouponClaimGrid" CollapseControlID="TtlCouponClaimGrid" Collapsed="false"
                            ImageControlID="ImgTtlCouponClaimGrid" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Coupon Claim Details"
                            ExpandedText="Coupon Claim Details" TextLabelID="lblTtlCouponClaimGrid">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlCouponClaimGrid" runat="server">
                            <table width="100%">
                                <tr>
                                    <td align="center" class="ContaintTableHeader" width="82%">
                                        <asp:Label ID="lblTtlCouponClaimGrid" runat="server" Text="Coupon Claim Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td class="ContaintTableHeader" width="8%">Count:
                                    </td>
                                    <td class="ContaintTableHeader" width="8%">
                                        <asp:Label ID="lblCouponClaimRecCnt" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlCouponClaimGrid" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntCouponClaimGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">

                            <div id="dvCouponClaimGrid" style="height: 280px; overflow: auto">
                                <asp:GridView ID="CouponClaimGrid" GridLines="Horizontal" Width="100%"
                                    AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                    AutoGenerateColumns="False" AllowPaging="false"
                                    runat="server">
                                    <FooterStyle CssClass="GridViewFooterStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Veh Type" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehCat" runat="server" Text='<%# Eval("Model_category") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("Model_Code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model Name" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModlName" runat="server" Text='<%# Eval("Model_name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Chassis No" ItemStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChassisNo" runat="server" Text='<%# Eval("chassis_no") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEngineNo" runat="server" Text='<%# Eval("engine_no") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Kms" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKms" runat="server" Text='<%# Eval("Kms") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hrs" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHrs" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Hrs") %>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coupon No" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcouponno" runat="server" Text='<%# Eval("coupon_no") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="JobCard No" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                  <asp:Label ID="lblJobcard" runat="server" Text='<%# Eval("JobcardNo") %>' CssClass="HideControl" />
                                                <asp:Label ID="lblJobID" runat="server" Text='<%# Eval("Jobcard_HDR_ID") %>' CssClass="HideControl" />
                                                <asp:LinkButton ID="lnkSelectPart" runat="server" Text='<%# Eval("JobcardNo") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Date" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceDate" runat="server" Text='<%# Eval("Service_Dt") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serv Name" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServName" runat="server" Text='<%# Eval("Serv_Name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCouponAmt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total_Amt","{0:#0.00}") %>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount With Tax" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCouponAmtWtTax" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("TotAmtWtTax","{0:#0.00}") %>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="4%" HeaderStyle-Width="4%">
                                            <HeaderTemplate>
                                                <center><asp:CheckBox ID="ChkAllPartPart" runat="server" CssClass="LabelCenterAlign"  onclick="return SetLabourRecordCount(this);"></asp:CheckBox></center>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <center><asp:CheckBox ID="ChkPart" runat="server"></asp:CheckBox></center>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rejection Remark" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRejRemark" runat="server" CssClass="TextBoxForString" Text='<%# Eval("Reason") %>'> </asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEFileAttchDetails" runat="server" TargetControlID="CntFileAttchDetails"
                            ExpandControlID="TtlFileAttchDetails" CollapseControlID="TtlFileAttchDetails"
                            Collapsed="false" ImageControlID="ImgTtlFileAttchDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Attached Documents"
                            ExpandedText="Hide Attached Documents" TextLabelID="lblTtlFileAttchDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlFileAttchDetails" runat="server">
                            <table width="100%">
                                <tr>
                                    <td align="center" class="ContaintTableHeader" width="82%">
                                        <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td class="ContaintTableHeader" width="8%">Count:
                                    </td>
                                    <td class="ContaintTableHeader" width="8%">
                                        <asp:Label ID="lblFileAttachRecCnt" runat="server" Text=""></asp:Label>
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
                                            GridLines="Horizontal" OnRowDataBound="FileAttchGrid_RowDataBound" HeaderStyle-Wrap="true"
                                            SkinID="NormalGrid" Width="100%">
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
                                            <input id="Text1" type="text" name="Text1" class="TextBoxForString" style="width: 50%" placeholder="User File Description" />
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

                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="CommandButton" OnClick="btnSave_Click" />
                        &nbsp;&nbsp;
                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" ToolTip="Confirm"
                        CssClass="CommandButton" OnClientClick="CheckBeforeConfirmRecord()" OnClick="btnConfirm_Click" />
                        &nbsp;&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="CommandButton" OnClick="btnBack_Click" />
                        &nbsp;&nbsp;
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
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>

                    <asp:TextBox ID="txtDomestic_Export" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox>


                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl" Text="" Width="96%"></asp:TextBox>
                    <asp:TextBox ID="txtDealerAID" runat="server" CssClass="HideControl" Text="" Width="96%"></asp:TextBox>
                    <asp:TextBox ID="txtfinalAppUserRole" runat="server" CssClass="HideControl" Text=""
                        Width="96%"></asp:TextBox>
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />

                </td>
            </tr>
        </table>

    </form>
    <% Response.Flush();%>

    <script type="text/javascript" language="javascript">
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
    </script>

</body>
</html>
