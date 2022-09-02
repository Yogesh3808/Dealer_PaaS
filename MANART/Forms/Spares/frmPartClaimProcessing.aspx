<%@ Page Title="MTI-PartClaim Processing" Language="C#" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="frmPartClaimProcessing.aspx.cs" Inherits="MANART.Forms.Spares.frmPartClaimProcessing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />

    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script src="../../Scripts/jsFileAttach.js"></script>
    <%--Sujata 24012011 Only for File attachment related--%>
    <%--<script src="../../Scripts/jsWCFileAttach.js"></script>--%>
    <%--Sujata 28012011 Only for File attachment related--%>
    <%--<script src="../../Scripts/jsWarrantyFunction.js"></script>--%>
    <%--Sujata 28012011--%>
    <script src="../../Scripts/jsPartMRNClaim.js"></script>
    <script src="../../Scripts/jsPartClaimProcessing.js"></script>

    <%--<script type="text/vbscript">
function PartClaimSave() 
    Dim hdn3PLStatus
    Dim hdnUserRoleId
    Dim intSelection
     Dim txtASMRemark     
    hdn3PLStatus = document.getElementById("hdn3PLStatus").value
    hdnUserRoleId = document.getElementById("hdnUserRoleId").value 
        
    if hdnUserRoleId = 11 Then'For PartClaim Manager  
    intSelection = MsgBox("Do you want to go to 3PL?",VbYesNoCancel, "Confirmation Dialog")  
        if intSelection = 6 Then
            document.getElementById("hdn3PLStatus").value = "3PL" 
        elseif intSelection = 7 Then
            document.getElementById("hdn3PLStatus").value=""      
        elseif intSelection = 2 Then
            PartClaimSave=false
        End If   
    Else
        if hdnUserRoleId = 12  Then'For 3PL        
             txtASMRemark = document.getElementById("txtASMRemark").value            
            if txtASMRemark = "" and hdnUserRoleId = "12" Then
                MsgBox("Please Enter Remarks !") 
                PartClaimSave=false               
            End If
        End If
   End If

end function
    </script>--%>

    <script type="text/javascript">
        function ShowInformationMessage(sID, sAddionalMsg) {
            var sDocName = "Part Claim Processing";
            if (sID == 1) {
                alert("" + sDocName + " " + sAddionalMsg + " Saved.");
            }
            else if (sID == 2) {
                alert("" + sDocName + " " + sAddionalMsg + " Approved.");
            }
            else if (sID == 3) {
                alert("" + sDocName + " " + sAddionalMsg + " Reject.");
            }

        }
        function PartClaim(sStatus) {
            //debugger;
            var bReturn;
            TargetBaseControl = document.getElementById("GridPartClaimDetail");
            var txtClaimType = document.getElementById("txtClaimType");
            var Inputs;
            var iROW;
            var ClaimQty = "";
            var ObjControl;
            var iPartID = 0;
            var sMessage = "";
            var iReason = 0;
            iROW = 0;

            for (var n = 1; n < TargetBaseControl.rows.length; ++n) {
                ObjControl = TargetBaseControl.rows[n].cells[0].childNodes[3];
                iPartID = ObjControl.innerHTML;
                if (iPartID != 0) {
                    MRNApprovedQty = dGetValue(TargetBaseControl.rows[n].cells[14].childNodes[1].value);
                    ClaimQty = TargetBaseControl.rows[n].cells[10].childNodes[1].innerHTML;
                    Reason = TargetBaseControl.rows[n].cells[18].childNodes[1].value;
                    if (MRNApprovedQty > 0) {
                        iROW = iROW + 1;
                    }
                    if (MRNApprovedQty < ClaimQty && Reason == "") {
                        sMessage = sMessage + " \n Please Enter the Reason at line " + n;
                        iReason = iReason + 1;
                    }
                }

            }

            if (sStatus == "Approved" && iROW == 0) {
                alert('Please Approve Atleast one Part...')
                return false;
            }
            if (sStatus == "Approved" && iReason != 0) {
                alert(sMessage)
                return false;
            }
            if (sStatus == "Reject" && iReason != 0) {
                //alert('Please Reject All Part Details...')
                alert(sMessage)
                return false;
            }

            var txtCSMRemark = document.getElementById("txtCSMRemark");
            var txtASMRemark = document.getElementById("txtASMRemark");
            var txtRSMRemark = document.getElementById("txtRSMRemark");
            //    var txtHeadRemark = document.getElementById("txtHeadRemark");
            var hdnUserRoleId = document.getElementById("hdnUserRoleId");
            if (txtRSMRemark.value == '' && hdnUserRoleId.value == '2'  && (sStatus == 'Approved' || sStatus == 'Reject')) {
                //|| (txtASMRemark.value == '' && hdnUserRoleId.value == '12')) 
                alert("Please Enter Remark !");
                return false;
            }

            if (sStatus == 'Approved') {
                if (confirm("Are you sure, you want to Approve Part Claim?") == true) {
                    return true
                }
                else {
                    return false
                }
            }
            else if (sStatus == 'Reject') {
                if (confirm("Are you sure, you want to Reject Part Claim?") == true) {
                    return true
                }
                else {
                    return false;
                }
            }
        }

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
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }

        function ClaimBack() {
            window.close();
        }



        function CheckTextBoxNull(event, ObjControl) {
            if (ObjControl.readOnly == true) {
                return;
            }
            if (ObjControl.value == "") {
                alert("Please enter the Approved Quantity");
                event.returnValue = false
                ObjControl.focus();
                return false;
            }
            else if (ObjControl.value < "0") {
                event.keyCode = 0;
                alert("Please enter the Qty greater than zero.");
                ObjControl.focus();
                event.returnValue = false
                return false;
            }

        }
        function ValidateQuantity(event, ObjQtyControl, objSelectType) {
            //debugger;
            var ClaimQty;
            var ClaimApprQty;
            var objRow;
            if (objSelectType == 'Rejected') {
                objRow = ObjQtyControl.parentNode.parentNode.parentNode.childNodes;
                ClaimApprQty = dGetValue(objRow[15].childNodes[1].value);
                objRow[15].childNodes[1].value = "0";
            }
            else if (objSelectType == 'Approved') {
                objRow = ObjQtyControl.parentNode.parentNode.parentNode.childNodes;
                ClaimQty = objRow[11].childNodes[1].innerHTML;
                ClaimApprQty = dGetValue(objRow[15].childNodes[1].value);
                if (ClaimApprQty == 0)
                    objRow[13].childNodes[1].value = ClaimQty;
            }
            else {
                if (CheckTextBoxNull(event, ObjQtyControl) == false) {
                    return;
                }

                objRow = ObjQtyControl.parentNode.parentNode.childNodes;
                ClaimQty = dGetValue(objRow[11].childNodes[1].innerHTML);
                ClaimApprQty = dGetValue(objRow[15].childNodes[1].value);
                //if (ClaimApprQty > 0)
                //    objRow[15].childNodes[1].childNodes[0].checked = true;
                //else
                //    objRow[16].childNodes[1].childNodes[0].checked = true;

                if (ClaimApprQty > ClaimQty) {
                    alert("Part Claim Approved Quanity Must Be Less Than Or Equal Claim Quantity");
                    objRow[15].childNodes[1].value = "";
                    objRow[15].childNodes[1].focus();
                    return false;
                }
            }
        }

        function ValidateQuantityForShortage(event, ObjQtyControl, objSelectType) {
            ////debugger;
            var ClaimQty;
            var ClaimApprQty;
            var objRow;
            if (objSelectType == 'Rejected') {
                objRow = ObjQtyControl.parentNode.parentNode.parentNode.childNodes;
                ClaimApprQty = dGetValue(objRow[11].childNodes[0].value);
                objRow[11].childNodes[0].value = "0";
            }
            else if (objSelectType == 'Approved') {
                objRow = ObjQtyControl.parentNode.parentNode.parentNode.childNodes;
                ClaimQty = objRow[7].childNodes[0].innerHTML;
                ClaimApprQty = dGetValue(objRow[11].childNodes[0].value);
                if (ClaimApprQty == 0)
                    objRow[11].childNodes[0].value = ClaimQty;
            }
            else {
                if (CheckTextBoxNull(event, ObjQtyControl) == false) {
                    return;
                }

                objRow = ObjQtyControl.parentNode.parentNode.childNodes;
                ClaimQty = dGetValue(objRow[7].childNodes[0].innerHTML);
                ClaimApprQty = dGetValue(objRow[11].childNodes[0].value);
                if (ClaimApprQty > 0)
                    objRow[13].childNodes[0].childNodes[0].checked = true;
                else
                    objRow[14].childNodes[0].childNodes[0].checked = true;
                if (ClaimApprQty > ClaimQty) {
                    alert("Part Claim Approved Quanity Must Be Less Than Or Equal Claim Quantity");
                    objRow[11].childNodes[0].value = "";
                    objRow[11].childNodes[0].focus();
                    return true;
                }
            }
        }


        function ValidateQuantityForWrongSupply(event, ObjQtyControl, objSelectType) {
            ////debugger;
            var ClaimQty;
            var MRNApprovedQty;
            var objRow;

            if (objSelectType == 'Rejected') {
                objRow = ObjQtyControl.parentNode.parentNode.parentNode.childNodes;
                ClaimApprQty = dGetValue(objRow[12].childNodes[0].value);
                objRow[12].childNodes[0].value = "0";
            }
            else if (objSelectType == 'Approved') {
                objRow = ObjQtyControl.parentNode.parentNode.parentNode.childNodes;
                ClaimQty = objRow[10].childNodes[0].innerHTML;
                ClaimApprQty = dGetValue(objRow[12].childNodes[0].value);
                if (ClaimApprQty == 0)
                    objRow[12].childNodes[0].value = ClaimQty;
            }
            else {

                if (CheckTextBoxNull(event, ObjQtyControl) == false) {
                    return;
                }

                objRow = ObjQtyControl.parentNode.parentNode.childNodes;

                ClaimQty = dGetValue(objRow[10].childNodes[0].innerHTML);
                ClaimApprQty = dGetValue(objRow[14].childNodes[0].value);
                if (ClaimApprQty > 0)
                    objRow[16].childNodes[0].childNodes[0].checked = true;
                else
                    objRow[17].childNodes[0].childNodes[0].checked = true;
                if (ClaimApprQty > ClaimQty) {
                    alert("Part Claim Approved Quanity Must Be Less Than Or Equal Claim Quantity");
                    objRow[14].childNodes[0].value = "";
                    objRow[14].childNodes[0].focus();
                    return true;
                }
            }
        }
        function ChkAprroved(chkbox) {
            var objRow = chkbox.parentNode.parentNode.childNodes;
            var MRNQty;
            var MRNApprovedQty;
            //    Sujata 22012011
            //    MRNQty=dGetValue(objRow[7].childNodes[0].innerHTML);
            //     MRNApprovedQty=dGetValue(objRow[11].childNodes[0].value);
            MRNQty = dGetValue(objRow[5].childNodes[0].innerHTML);
            MRNApprovedQty = dGetValue(objRow[9].childNodes[0].value);
            //    Sujata 22012011
            if (chkbox.checked) {
                //    Sujata 22012011
                //objRow[13].childNodes[0].style.display='none';
                objRow[11].childNodes[0].style.display = 'none';
                //    Sujata 22012011
                MRNApprovedQty.value = MRNQty.value;
            }
            else {
                //    Sujata 22012011
                //objRow[13].childNodes[0].style.display = '';
                objRow[11].childNodes[0].style.display = '';
                //    Sujata 22012011        
                MRNApprovedQty.value = MRNApprovedQty.value;
            }
        }
    </script>

    <style type="text/css">
        .style1 {
            height: 13px;
        }
    </style>
    <base target="_self" />
</head>
<%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
<body runat="server">
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table id="PageTbl" class="table-responsive" border="1">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="PageTitle panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server"> </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">
                            <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                                <table id="txtDocNoDetails" runat="server" class="ContainTable">
                                    <tr class="panel-heading">
                                        <td align="center" class="ContaintTableHeader panel-title" style="height: 15px" colspan="6">Document Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="width: 15%">
                                            <asp:Label ID="lblRequestDate" runat="server" Text="Claim Request No:"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtPartClaimNo" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Claim Request Date: </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtClaimDate" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Claim Amount:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtClaimAmount" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">LR No:</td>
                                        <td>
                                            <asp:TextBox ID="txtLrNo" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">LR Date: </td>
                                        <td>
                                            <asp:TextBox ID="txtLRDate" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%">Part Claim Type:  </td>
                                        <td>
                                            <asp:TextBox ID="txtApprovedBy" runat="server" CssClass="DispalyNon" Enabled="False"></asp:TextBox>
                                            <asp:TextBox ID="txtClaimType" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">Narration:                        
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxForString" TextMode="MultiLine" MaxLength="100"
                                                onblur="checkTextAreaMaxLength(this,event,'100');" Rows="2"></asp:TextBox>

                                        </td>
                                        <td style="width: 15%" class="tdLabel">Accepted Claim Amount:                         
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtAccClaimAmt" runat="server" Text='<%# Eval("Acc_Claim_Amt","{0:#0.00}") %>' Enabled="False" CssClass="TextBoxForString"></asp:TextBox>

                                        </td>
                                        <td style="width: 15%" class="tdLabel HideControl">Box Condition:
                                        </td>
                                        <td style="width: 18%" class="HideControl">
                                            <asp:TextBox ID="txtBoxCondition" runat="server" CssClass="MultilineTextbox"
                                                TextMode="MultiLine" Height="43px" Width="420px" Enabled="false"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <table id="tblIns" runat="server" class="ContainTable">
                                                <tr>
                                                    <td class="tdLabel" style="width: 15%">Insurance Cover Note No:
                                                    </td>
                                                    <td style="width: 18%">
                                                        <asp:TextBox ID="txtSurveyNo" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                                    </td>
                                                    <td class="tdLabel" style="width: 15%">Validity Date:
                                                    </td>
                                                    <td style="width: 18%">
                                                        <asp:TextBox ID="txtSurveyDate" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                                    </td>
                                                    <td class="tdLabel" style="width: 15%">Survey By:
                                                    </td>
                                                    <td style="width: 18%">
                                                        <asp:TextBox ID="txtSurveyBy" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel" style="width: 15%">Insurance Company Name:
                                                    </td>
                                                    <td style="width: 18%">
                                                        <asp:TextBox ID="txtIncComp" runat="server" CssClass="TextBoxForString" Enabled="False"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trDamageType" runat="server">
                                        <td class="tdLabel" style="width: 15%">
                                            <asp:Label ID="lblDamageType" runat="server" Text="Damage Type"></asp:Label>
                                        </td>
                                        <td class="tdLabel">&nbsp;&nbsp;<asp:DropDownList ID="drpDamageType" runat="server" CssClass="GridComboBoxFixedSize" Enabled="false">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="I">Insurance</asp:ListItem>
                                            <asp:ListItem Value="R">Regular</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">
                                            <asp:Label ID="Label1" runat="server" Text="Claim Invoice No:"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtApprovedNo" runat="server" CssClass="TextBoxForString"
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">
                                            <asp:Label ID="Label14" runat="server" Text="Claim Invoice Date:"></asp:Label>
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="TextBoxForString"
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel HideControl">&nbsp; Rejection Reason
                                        </td>
                                        <td style="width: 18%" class="HideControl">
                                            <asp:TextBox ID="txtRejectionReason" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="HideControl">
                                        <td style="width: 15%" class="tdLabel">Error Description
                                        </td>
                                        <td style="width: 60%" colspan="5">
                                            <div style="overflow: auto; height: 60px; width: 855px;">
                                                <asp:CheckBoxList ID="chkError" runat="server" Width="826px" CssClass="ComboBoxFixedSize">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr align="center" style="display: none">
                                        <td colspan="6" class="style1">
                                            <div id="DivBtn" runat="server">
                                                &nbsp;&nbsp;
                                                
                                                &nbsp;&nbsp;                                                
                                            </div>
                                        </td>
                                    </tr>
                                    <tr align="center" style="display: none">
                                        <td colspan="6">
                                            <div id="Div1" runat="server">
                                                &nbsp;&nbsp;
                                                
                                                &nbsp;&nbsp;                                                
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                                    ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="false"
                                    ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Details" ExpandedText="Details"
                                    TextLabelID="lblTtlPartDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlPartDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="style3 panel-title" width="82%">
                                                <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Claim Details" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%" class="style4">
                                                <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                    <asp:GridView ID="GridPartClaimDetail" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                        Width="100%" OnRowDataBound="GridPartClaimDetail_RowDataBound">
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." ItemStyle-Width="0.5%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                    <asp:Label ID="lblID" runat="server" CssClass="HideControl" Text='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inv Part No" ItemStyle-Width="3%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("parts_no") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inv Part Name" ItemStyle-Width="6%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartName" runat="server" Text='<%# Eval("part_name") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Recv Part No" ItemStyle-Width="3%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRPartNo" runat="server" Text='<%# Eval("RecvPartNo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Recv Part Name" ItemStyle-Width="6%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRPartName" runat="server" Text='<%# Eval("RecvPartName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Retain" ItemStyle-Width="2%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRetain" runat="server" Text='<%# Eval("retain") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inv No" ItemStyle-Width="2%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("inv_no") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inv Date" ItemStyle-Width="2%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("inv_date","{0:d}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inv Qty" ItemStyle-Width="1%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvQty" runat="server" Text='<%# Eval("Inv Qty","{0:0}") %>' Width="60%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rcvd Qty" ItemStyle-Width="1%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRcvdQty" runat="server" Text='<%# Eval("RcvdQty","{0:0}") %>' Width="60%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Claim Qty" ItemStyle-Width="1%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClaimQty" runat="server" Text='<%# Eval("Claim Qty","{0:0}") %>' Width="60%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GRN No" ItemStyle-Width="3%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGrrNo" runat="server" Text='<%# Eval("grr_tr_no") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GRN Date" ItemStyle-Width="2%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGrrDate" runat="server" Text='<%# Eval("grr_date","{0:d}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Box Condition" ItemStyle-Width="1%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn">
                                                <ItemTemplate>
                                                    <%--Sujata 24012011
                                                    <asp:Label ID="lblBoxCondition" runat="server" Text='<%# Eval("Box Condition") %>' />--%>
                                                    <asp:Label ID="lblBoxCondition" runat="server" Text="" />
                                                    <%--Sujata 24012011--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Rate" ItemStyle-CssClass="LabelRightAlign">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-CssClass="LabelRightAlign">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("total","{0:#0.00}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Appr Qty" ItemStyle-Width="2%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <%--Sujata 24012011 28012011 remove onblur="return ValidateQuantity(event,this);" onblur="return ValidateQuantity(event,this);"
                                            <asp:TextBox ID="txtApprovedQty" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Approved_qty","{0:0}") %>'
                                                onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);" onblur="return ValidateQuantity(event,this);"></asp:TextBox>--%>
                                                    <asp:TextBox ID="txtApprovedQty" runat="server" CssClass="GridTextBoxForString" MaxLength="5" Text='<%# Eval("Approved_qty","{0:0.00}") %>'
                                                        Width="96%" onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return CalculateAccPartClaimTotal(event,this);"></asp:TextBox>
                                                    <%--Sujata 24012011--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approved" ItemStyle-Width="0%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl " ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <%-- <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="ChkApproved" Text="Approved" onclick="HeaderClick(this)" />
                                        </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <%--Sujata 29012011 remove onclick="ChkAprroved(this)"--%>
                                                    <asp:CheckBox ID="ChkStatusApproved" runat="server" Checked='<%# Eval("Approved_Status") %>' Width="50%" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approved" ItemStyle-Width="0%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                                <%-- <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="ChkApproved" Text="Approved" onclick="HeaderClick(this)" />
                                        </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <%--Sujata 29012011 remove onclick="ChkAprroved(this)"--%>
                                                    <asp:RadioButton ID="rdoApproved" runat="server" Width="50%" GroupName="A" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rejected" ItemStyle-Width="0%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                                <%-- <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="ChkApproved" Text="Approved" onclick="HeaderClick(this)" />
                                        </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <%--Sujata 29012011 remove onclick="ChkAprroved(this)"--%>
                                                    <asp:RadioButton ID="rdoRejected" runat="server" Width="50%" GroupName="A" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reject Reason" ItemStyle-Width="6%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <%-- <asp:DropDownList ID="drpDescription" runat="server" CssClass="GridComboBoxFixedSize"
                                                Width="96%">
                                            </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Rejection_Res") %>'
                                                        Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Credit Type" ItemStyle-Width="0%" ItemStyle-Font-Size="9px" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpShortageClaimTypeCredit" runat="server" CssClass="GridComboBoxFixedSize" Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Debit Type" ItemStyle-Width="0%" ItemStyle-Font-Size="9px" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpClaimType" runat="server" CssClass="GridComboBoxFixedSize" Width="96%">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HO Appr Qty" ItemStyle-Width="2%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHOApprovedQty" runat="server" CssClass="GridTextBoxForString" MaxLength="5" Text='<%# Eval("HOApproved_qty","{0:0}") %>'
                                                        Width="96%" onkeypress="return CheckForTextBoxValue(event,this,'5');" onblur="return ValidateQuantity(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HO Rej Reason" ItemStyle-Width="6%" ItemStyle-Font-Size="9px" HeaderStyle-Font-Size="10px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHODescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("HORejection_Res") %>'
                                                        Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Rate","{0:#0.00}")%>'
                                                        onkeypress="return CheckForTextBoxValue(event,this);" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("Total","{0:#0.00}")%>'
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acc Total" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAccTotal" runat="server" CssClass="GridTextBoxForAmount" Width="96%" Text='<%# Eval("AccTotal","{0:#0.00}")%>'
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                                        onblur="return CalculateLineTotalForPart(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Part Tax" ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpPartTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%">
                                                        <%--OnSelectedIndexChanged="drpPartTax_SelectedIndexChanged" AutoPostBack="True"--%>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="DrpPartTax1" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="DrpPartTax2" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="drpPartTaxPer" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtPartTaxPer" runat="server" CssClass="HideControl" Width="80%"></asp:TextBox>
                                                    <asp:DropDownList ID="DrpPartTax1Per" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtPartTax1Per" runat="server" CssClass="HideControl" Width="80%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>

                                                    <asp:DropDownList ID="DrpPartTax2Per" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtPartTax2Per" runat="server" CssClass="HideControl" Width="80%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Part Group" ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrNo" runat="server" Width="80%" Text='<%# Eval("group_code") %>'
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntPartGroupDetails"
                                    ExpandControlID="TtlPartGroupDetails" CollapseControlID="TtlPartGroupDetails" Collapsed="true"
                                    ImageControlID="ImgTtlPartGroupDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Group Tax Details" ExpandedText="Group Tax Details"
                                    TextLabelID="lblTtlPartGroupDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlPartGroupDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="lblTtlPartGroupDetails" runat="server" Text="Group Tax Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlPartGroupDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    ScrollBars="None">
                                    <asp:GridView ID="GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                                        CssClass="table table-condensed table-bordered">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Code" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGRPID" runat="server" Width="96%" Text='<%# Eval("group_code") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMGrName" runat="server" Text='<%# Eval("Gr_Name") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable Inv Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                        MaxLength="6"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="drpTaxTag" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTaxTag" runat="server" Text='<%# Eval("Tax_Tag") %>' CssClass="GridTextBoxForString" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxPer" runat="server" Text='<%# Eval("TAX_Percentage","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTaxAmt" runat="server" Text='<%# Eval("tax_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <%--Sujata 22092014_Begin--%>
                                                    <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--Sujata 22092014_End--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax1Per" runat="server" Text='<%# Eval("Tax1_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTax1Amt" runat="server" Text='<%# Eval("tax1_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <%--Sujata 22092014_Begin--%>
                                                    <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--Sujata 22092014_End--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax2Per" runat="server" Text='<%# Eval("Tax2_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTax2Amt" runat="server" Text='<%# Eval("tax2_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxTot" runat="server" Width="90%" Text='<%# Eval("Total","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntTaxDetails"
                                    ExpandControlID="TtlTaxDetails" CollapseControlID="TtlTaxDetails" Collapsed="true"
                                    ImageControlID="ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Receipt Tax Details" ExpandedText="Receipt Tax Details"
                                    TextLabelID="lblTtlTaxDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlTaxDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="lblTtlTaxDetails" runat="server" Text="Receipt Tax Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="ImgTtlTaxDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    ScrollBars="None">
                                    <asp:GridView ID="GrdDocTaxDet" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDocID" runat="server" Width="5%" Text='<%# Eval("ID") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable Total" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDocTotal" runat="server" Text='<%# Eval("net_tr_amt","{0:#0.00}") %>' Width="90%"
                                                        CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--<asp:TextBox ID="txtDocRevTotal" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForAmount HideControl" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDocDisc" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Before Tax Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBeforeTax" runat="server" Text='<%# Eval("before_tax_amt") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LST Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("mst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CST Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("cst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax 1" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax1" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("surcharge_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax 2" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax2" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("tot_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PF Charges%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                                        Width="90%" MaxLength="5"></asp:TextBox>
                                                    <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PF Charges Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                                        Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                                    <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                        Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                        Text='<%# Eval("Claim_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>

                            <%-- Accepted Quantity Calculations Start Here --%>
                            <asp:Panel ID="Acc_PPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Acc_CntPartGroupDetails"
                                    ExpandControlID="Acc_TtlPartGroupDetails" CollapseControlID="Acc_TtlPartGroupDetails" Collapsed="true"
                                    ImageControlID="Acc_ImgTtlPartGroupDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText=" Acc Group Tax Details" ExpandedText=" Acc Group Tax Details"
                                    TextLabelID="Acc_lblTtlPartGroupDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="Acc_TtlPartGroupDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="Acc_lblTtlPartGroupDetails" runat="server" Text="Group Tax Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="Acc_ImgTtlPartGroupDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Acc_CntPartGroupDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    ScrollBars="None">
                                    <asp:GridView ID="Acc_GrdPartGroup" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5"
                                        CssClass="table table-condensed table-bordered">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Code" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGRPID" runat="server" Width="96%" Text='<%# Eval("group_code") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMGrName" runat="server" Text='<%# Eval("Gr_Name") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable Inv Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrnetinvamt" runat="server" Text='<%# Eval("Acc_net_inv_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrDiscountPer" runat="server" Text='<%# Eval("discount_per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"
                                                        MaxLength="6"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrDiscountAmt" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" Width="90%"></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="drpTaxTag" runat="server" CssClass="GridComboBoxFixedSize" Width="96%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTaxTag" runat="server" Text='<%# Eval("Tax_Tag") %>' CssClass="GridTextBoxForString" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxPer" runat="server" Text='<%# Eval("TAX_Percentage","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTaxAmt" runat="server" Text='<%# Eval("Acc_tax_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer1" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <%--Sujata 22092014_Begin--%>
                                                    <asp:DropDownList ID="DrpTax1ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTax1ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--Sujata 22092014_End--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax1Per" runat="server" Text='<%# Eval("Tax1_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax1 Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTax1Amt" runat="server" Text='<%# Eval("Acc_tax1_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTax2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 %" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpTaxPer2" runat="server" CssClass="GridComboBoxFixedSize" Width="99%" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <%--Sujata 22092014_Begin--%>
                                                    <asp:DropDownList ID="DrpTax2ApplOn" runat="server" CssClass="HideControl" Width="99%"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTax2ApplOn" runat="server" CssClass="HideControl" Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--Sujata 22092014_End--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 %" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax2Per" runat="server" Text='<%# Eval("Tax2_Per","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax2 Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrTax2Amt" runat="server" Text='<%# Eval("Acc_tax2_amt","{0:#0.00}") %>' CssClass="GridTextBoxForAmount"
                                                        Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxTot" runat="server" Width="90%" Text='<%# Eval("Acc_Total","{0:#0.00}") %>' CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>

                            <asp:Panel ID="Acc_PCntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Acc_CntTaxDetails"
                                    ExpandControlID="Acc_TtlTaxDetails" CollapseControlID="Acc_TtlTaxDetails" Collapsed="true"
                                    ImageControlID="Acc_ImgTtlTaxDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Acc Receipt Tax Details" ExpandedText="Acc Receipt Tax Details"
                                    TextLabelID="Acc_lblTtlTaxDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="Acc_TtlTaxDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="Acc_lblTtlTaxDetails" runat="server" Text="Acc Receipt Tax Details" Width="96%"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%">
                                                <asp:Image ID="Acc_ImgTtlTaxDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="16px"
                                                    Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Acc_CntTaxDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    ScrollBars="None">
                                    <asp:GridView ID="Acc_GrdDocTaxDet" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                        Width="99%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                        EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" PageSize="5" CssClass="table table-condensed table-bordered">
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDocID" runat="server" Width="5%" Text='<%# Eval("ID") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Taxable Total" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDocTotal" runat="server" Text='<%# Eval("Acc_net_tr_amt","{0:#0.00}") %>' Width="90%"
                                                        CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                    <%--<asp:TextBox ID="txtDocRevTotal" runat="server" Text='<%# Eval("net_rev_amt","{0:#0.00}") %>' Width="90%"
                                            CssClass="GridTextBoxForAmount HideControl" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" ItemStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDocDisc" runat="server" Text='<%# Eval("discount_amt","{0:#0.00}") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Before Tax Amt" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBeforeTax" runat="server" Text='<%# Eval("before_tax_amt") %>' Width="90%"
                                                        CssClass="GridTextBoxForString" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LST Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("Acc_mst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CST Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCSTAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("Acc_cst_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax 1" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax1" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("Acc_surcharge_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax 2" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTax2" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                        Text='<%# Eval("tot_amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this)"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PF Charges%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPFPer" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_per","{0:#0.00}") %>'
                                                        Width="90%" MaxLength="5"></asp:TextBox>
                                                    <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PF Charges Amt" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPFAmt" runat="server" Width="90%" CssClass="GridTextBoxForAmount" Text='<%# Eval("pf_amt","{0:#0.00}") %>'></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this)"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Charges %" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOtherPer" runat="server" CssClass="GridTextBoxForAmount" Width="80%" MaxLength="5"
                                                        Text='<%# Eval("other_per","{0:#0.00}") %>'></asp:TextBox>
                                                    <%--onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur="return CalulateInvPartGranTotal();"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Charges" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOtherAmt" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                        Text='<%# Eval("other_money","{0:#0.00}") %>'></asp:TextBox>
                                                    <%--onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grand Total" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrandTot" runat="server" CssClass="GridTextBoxForAmount" Width="80%"
                                                        Text='<%# Eval("Acc_Claim_Amt","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>

                            <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEFileAttchDetails" runat="server" TargetControlID="CntFileAttchDetails"
                                    ExpandControlID="TtlFileAttchDetails" CollapseControlID="TtlFileAttchDetails"
                                    Collapsed="true" ImageControlID="ImgTtlFileAttchDetails" ExpandedImage="~/Images/Minus.png"
                                    CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Attached Documents"
                                    ExpandedText="Attached Documents" TextLabelID="lblTtlFileAttchDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlFileAttchDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="style3 panel-title" width="82%">
                                                <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents"
                                                    onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <%--<td class="ContaintTableHeader" width="8%">Count:
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
                                            <td>
                                                <asp:GridView ID="FileAttchGrid"
                                                    runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                                    AutoGenerateColumns="False"
                                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                                    GridLines="Horizontal"
                                                    OnRowCommand="DetailsGrid_RowCommand" HeaderStyle-Wrap="true"
                                                    SkinID="NormalGrid" Width="100%">
                                                    <FooterStyle CssClass="GridViewFooterStyle" />
                                                    <RowStyle CssClass="GridViewRowStyle" />
                                                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                                                    <PagerStyle CssClass="GridViewPagerStyle" />
                                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign"
                                                                    Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="1%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                            ItemStyle-CssClass="HideControl">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFileAttchID" runat="server" Text='<%# Eval("ID") %>'
                                                                    Width="1"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Description" ItemStyle-Width="50%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDescption" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description") %>'
                                                                    Width="96%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="File Name" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFile" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                                                    onmouseover="SetCancelStyleonMouseOver(this);" onClick="return ShowAttachDocumentProcessing(this);"
                                                                    Text='<%# Eval("File_Names") %>' ToolTip="Click Here To Open The File"
                                                                    Width="90%"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--Sujata 24012011--%>
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
                                    </table>
                                </asp:Panel>
                            </asp:Panel>

                            <asp:Panel ID="PRemarkDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPERemarkDetails" runat="server" TargetControlID="CntRemarkDetails"
                                    ExpandControlID="TtlRemarkDetails" CollapseControlID="TtlRemarkDetails" Collapsed="True"
                                    ImageControlID="ImgTtlRemarkDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                                    SuppressPostBack="true" CollapsedText="Remark Details" ExpandedText="Remark Details"
                                    TextLabelID="lblTtlRemarkDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlRemarkDetails" runat="server">
                                    <table width="100%">
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="99%">
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
                                            <td style="padding-left: 10%;">
                                                <table id="tblRemark" runat="server" width="80%" class="NormalTable">
                                                    <tr>
                                                        <%--RSM Remark:--%>
                                                        <td class="tdLabel" style="width: 15%">Parts Support Desk Remark : 
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox ID="txtRSMRemark" Text="" runat="server" CssClass="MultilineTextbox" MaxLength="200"
                                                                TextMode="MultiLine" onblur="checkTextAreaMaxLength(this,event,'200');" Rows="5"
                                                                 Height="50%" Width="96%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="HideControl">
                                                        <td class="tdLabel" style="width: 15%">Head Remark:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox ID="txtCSMRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                                TextMode="MultiLine" Height="50%" Width="96%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="HideControl">
                                                        <td class="tdLabel" style="width: 15%">HO Remark:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox ID="txtASMRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                                TextMode="MultiLine" Height="50%" Width="96%"></asp:TextBox>
                                                            &nbsp;
                                                        
                                                        </td>
                                                    </tr>
                                                    <tr id="trShow3PLRemark" runat="server" class="HideControl">
                                                        <td class="tdLabel" style="width: 15%">&nbsp;
                                                        </td>
                                                        <td style="width: 80%">&nbsp;<asp:CheckBox ID="ChkShow3PLRemark" Text="Show 3PL Remark" runat="server" />
                                                        </td>

                                                    </tr>

                                                    <tr class="HideControl">
                                                        <td class="tdLabel" style="width: 15%">Head Remark:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox ID="txtHeadRemark" Text="" runat="server" CssClass="MultilineTextbox"
                                                                TextMode="MultiLine" Height="50%" Width="96%"></asp:TextBox>
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
                        <td>&nbsp;&nbsp;
                             <%--Sujata Change position from divButtons for the buttons Approve and Reject
                             remove OnClientClick="return PartClaim('Reject');" this from btnReject--%>
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-search" OnClientClick="ClaimBack();" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-search"
                                OnClick="btnSave_Click" />
                            <%--OnClientClick="return PartClaimSave();"--%>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-search"
                                OnClick="btnApprove_Click" OnClientClick="return PartClaim('Approved');" />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-search"
                                OnClick="btnReject_Click" OnClientClick="return PartClaim('Reject');" />

                        </td>
                    </tr>
                    <tr id="TmpControl">
                        <td style="width: 14%">
                            <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtID" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtDealerCode" CssClass="DispalyNon" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="txtPreviousDocId" CssClass="DispalyNon" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:HiddenField ID="hdnUserRoleId" runat="server" />
                            <asp:HiddenField ID="hdn3PLStatus" runat="server" />
                            <asp:HiddenField ID="hdnPartClaimMgr" runat="server" />
                            <asp:HiddenField ID="hdn3PL" runat="server" />
                             <asp:HiddenField ID="hdnIsRoundOFF" runat="server" Value="N" />
                            <asp:HiddenField ID="hdnProcessContinue" runat="server" />
                            <asp:HiddenField ID="hdnIsDocGST" runat="server" Value="" />
                            <asp:HiddenField ID="hdnDealerID" runat="server" Value="" />
                            <asp:HiddenField ID="hdnCustTaxTag" runat="server" Value="N" />

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
<%--</asp:Content>--%>
