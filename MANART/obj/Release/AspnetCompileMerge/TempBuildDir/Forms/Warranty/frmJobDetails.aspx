<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmJobDetails.aspx.cs" Inherits="MANART.Forms.Warranty.frmJobDetails" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsJobDetailsFunction.js"></script>
    <script src="../../Scripts/jsWCPartFunc.js"></script>
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

        function GetJobcodeDtls(objLink, sDealerId, sJobCodeID, sPCRID, sJobSavedORNot) {
            var iJob_HDR_ID = 0;
           
            var ObjControl = window.document.getElementById("txtJobHDRID");
            //Get ORF ID
            if (ObjControl != null) {
                iJob_HDR_ID = dGetValue(ObjControl.value);
            }

            var ObjDomestic = window.document.getElementById("txtDomestic_Export");
            var ObjtxtID = window.document.getElementById("txtID");
            if (ObjDomestic.value == "E") {
                iJob_HDR_ID = dGetValue(ObjtxtID.value);
            }
                       
            debugger;         
            var Parameters = "JobID=" + iJob_HDR_ID + "&DealerID=" + sDealerId + "&JobCodeID=" + sJobCodeID + "&PCRID=" + sPCRID + "&Display=Y" + ((ObjDomestic.value == "E") ? "&sFOR=CLM" : "&sFOR=JbC");
            var feature = "dialogWidth:1000px;dialogHeight:900px;status:no;help:no;scrollbars:no;resizable:no;";
            var PCRId;
            PCRId = window.showModalDialog("../Service/frmJobcodeDtls.aspx?" + Parameters, null, feature);
            if (PCRId != null) {
                objRow[11].childNodes[1].value = PCRId;
            }
            return;
        }
    </script>
    <base target="_self" />
    <style type="text/css">
        .auto-style1 {
            font-size: smaller;
        }
    </style>
</head>
<body runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 9%">
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double"
                        CssClass="DispalyNon">
                        <table id="txtDocNoDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="6">Document Details
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%">Claim Type:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtClaimType" Text="Goodwill Request" Font-Bold="true" runat="server"
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
                                    <asp:TextBox ID="txtClaimDate" Text="" runat="server" CssClass="TextForDate" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%" colspan="2"></td>
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
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PVehicleDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <%--CssClass="DispalyNon"--%>
                        <cc1:CollapsiblePanelExtender ID="CPEVehicleDetails" runat="server" TargetControlID="CntVehicleDetails"
                            ExpandControlID="TtlVehicleDetails" CollapseControlID="TtlVehicleDetails" Collapsed="false"
                            ImageControlID="ImgTtlVehicleDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Header Deduction Details" ExpandedText="Header Deduction Details"
                            TextLabelID="lblTtlVehicleDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlVehicleDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title">
                                        <asp:Label ID="lblTtlVehicleDetails" runat="server" Text="Header Deduction Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            ></asp:Label>
                                    </td>
                                    <%--<td class="ContaintTableHeader" ></td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlVehicleDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntVehicleDetails" runat="server">
                            <%--Style="display: none;"--%>
                            <table id="tblVehicleDetails" runat="server" class="ContainTable table table-bordered" style="display: none;">
                                <tr>
                                    <td class="tdLabel" style="width: 15%">
                                        <asp:Label ID="lblModelCode" runat="server" Text="Fert Code:"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtModelCode" Text="" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">
                                        <asp:Label ID="lblModelName" runat="server" Text="Model Name:"></asp:Label>
                                    </td>
                                    <td style="width: 18%" colspan="2">
                                        <asp:TextBox ID="txtModelDescription" Text="" runat="server" CssClass="TextBoxForString"
                                            Width="80%" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 18%;"></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%;">Vehicle Reg. No.:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtVehicleNo" Text="" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">Chassis No.:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtChassisNo" Text="" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 15%;">Engine No.:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtEngineNo" Text="" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table id="tblHeaderDeduction" runat="server" class="ContainTable">
                                <tr>
                                    <td class="tdLabel" style="width: 10%">
                                        <asp:Label ID="lblDeductionType" runat="server" Text="Deduction Type:"></asp:Label>
                                    </td>
                                    <td style="width: 17%" colspan="7">
                                        <asp:RadioButtonList ID="rdoDeductionType" runat="server"
                                            RepeatDirection="Horizontal" CssClass="TextBoxForString" AutoPostBack="true" 
                                            OnSelectedIndexChanged="rdoDeductionType_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="G" onClick=" OnOverallDeductionChange(event,this);">IsGlobal</asp:ListItem>
                                            <asp:ListItem Value="L" onClick=" OnItemwiseDeductionChange(event, this);">IsLineLevel</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <%-- <td colspan ="7" style="width: 73%">
                                        <asp:Label ID="lblAutoDeductPer" runat ="server" Text ="" Font-Bold ="true" Font-Size="Small" ForeColor ="Red" ></asp:Label>
                                        <asp:Label ID="lblFixed" runat ="server" Text =" Auto Deduction Applied on " Font-Bold ="true" Font-Size="Small" ForeColor ="Red" ></asp:Label>
                                        <asp:Label ID="lblAutoDeductOn" runat ="server" Text ="" Font-Bold ="true" Font-Size="Small" ForeColor ="Red" ></asp:Label>
                                        </td>--%>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 10%">
                                        <asp:Label ID="lblPart" runat="server" Text="Part:"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtPartHeaderDeduction" Text="" runat="server" CssClass="TextBoxForString"
                                            MaxLength="3"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 10%;">
                                        <asp:Label ID="lblLabour" runat="server" Text="Labor:"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtLabourHeaderDeduction" Text="" runat="server" CssClass="TextBoxForString"
                                            Width="80%" MaxLength="3"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 10%;">
                                        <asp:Label ID="lblLubricant" runat="server" Text="Lubricant:"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtLubricantHeaderDeduction" Text="" runat="server" CssClass="TextBoxForString"
                                            Width="80%" MaxLength="3"></asp:TextBox>
                                    </td>
                                    <td class="tdLabel" style="width: 10%;">
                                        <asp:Label ID="lblSublet" runat="server" Text="Sublet:"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtSubletHeaderDeduction" Text="" runat="server" CssClass="TextBoxForString"
                                            Width="80%" MaxLength="3"></asp:TextBox>
                                    </td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PJobDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <cc1:CollapsiblePanelExtender ID="CPEJobDetails" runat="server" TargetControlID="CntJobDetails"
                            ExpandControlID="TtlJobDetails" CollapseControlID="TtlJobDetails" Collapsed="true"
                            ImageControlID="ImgTtlJobDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Job Details" ExpandedText="Job Details"
                            TextLabelID="lblTtlJobDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlJobDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                        <asp:Label ID="lblTtlJobDetails" runat="server" Text="Job Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);" ></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlJobDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntJobDetails" runat="server" Style="display: none;">
                            <table id="Table2" runat="server" class="ContainTable">
                                <tr>
                                    <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="4">Job
                                            <asp:Label ID="lblJobCode" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        Details
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" class="tdLabel">Primary failed part nr.:
                                    </td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="lblCausalPartNo" runat="server" CssClass="DispalyNon"></asp:TextBox>
                                        <asp:TextBox ID="txtPartNo" runat="server" CssClass="TextBoxForString" Text=""
                                            onkeydown="return ToSetKeyPressValueFalse(event,this);" Width="96%"></asp:TextBox>
                                        <asp:Label ID="lblAddPart" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            onmouseover="SetCancelStyleonMouseOver(this);" Text="SelectPart" ToolTip="Click Here To Select Part" Visible="false"
                                            Width="35%"></asp:Label>
                                        <asp:Label ID="lblDefectCulprit" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                        <%--  <asp:DropDownList ID="drpParts" runat="server" CssClass="ComboBoxFixedSize" Width="90%">
                                            </asp:DropDownList>--%>
                                    </td>
                                    <td style="width: 10%" class="tdLabel">Culprit Code:
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="drpCulpritCode" runat="server" CssClass="ComboBoxFixedSize"
                                            Width="74%" onblur="return CheckSelectedValue('CulpritCode')">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="drpCulpritCodeTemp" runat="server" CssClass="ComboBoxFixedSize"
                                            Width="74%" onblur="return CheckSelectedValue('CulpritCodeTemp')">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkCulpritInfo" runat="server" Text="SearchByName" onclick="DisplayStatus(this,'Culprit');" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" class="tdLabel">Defect Code:
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="drpDefectCode" runat="server" CssClass="ComboBoxFixedSize"
                                            Width="75%" onblur="return CheckSelectedValue('DefectCode')">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="drpDefectCodeTemp" runat="server" CssClass="ComboBoxFixedSize"
                                            Width="75%" onblur="return CheckSelectedValue('DefectCodeTemp')">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkDefectInfo" runat="server" Text="SearchByName" onclick="DisplayStatus(this,'Defect');" />
                                    </td>
                                    <td style="width: 10%" class="tdLabel">Technical Code:
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="drpTechnicalCode" runat="server" CssClass="ComboBoxFixedSize"
                                            Width="90%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" class="tdLabel">
                                        <asp:LinkButton ID="lnkSelectJobDtl" runat="server" >Job PCR</asp:LinkButton> 
                                    </td>
                                    <td style="width: 35%">
                                        
                                    </td>
                                    <td style="width: 10%" class="tdLabel">
                                    </td>
                                    <td style="width: 35%">
                                        
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                            ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="true"
                            ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Part Details" ExpandedText="Part Details"
                            TextLabelID="lblTtlPartDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlPartDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="99%">
                                        <asp:Label ID="lblTtlPartDetails" runat="server" Text="Part Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            ></asp:Label>
                                    </td>
                                    <%--<td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblPartRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntPartDetails" runat="server">
                            <asp:GridView ID="PartDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                GridLines="Horizontal" SkinID="NormalGrid">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed PartNo." ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartNo" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("part_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed PartName" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartName" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed Make" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMake" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Failed_Make") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Replaced Part No." ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRPartNo" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Rpart_no") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Replaced PartName" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRPartName" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("RPart_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Replaced Make" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRMake" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Replaced_Make") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aggegrate" ItemStyle-Width="5%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAggegrate" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Aggegrate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inv No." ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvNo" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inv Date" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvDate" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Invoice_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerCode" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Dealer_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Qty" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty","{0:#,#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Rate" ItemStyle-Width="4%"
                                        ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Amount" ItemStyle-Width="6%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClaimedAmount" runat="server" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Qty" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccQty" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Accepted_Qty","{0:#,#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VECV_Share","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Dealer_Share","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Cust_Share","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="" Width="90%"
                                                Text='<%# Eval("Deduction_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Amount" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Accepted_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction Remarks" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="GridTextBoxForString" Width="90%"
                                                TextMode="MultiLine" Text='<%# Eval("Deduction_Remark") %>' Height="100%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Change Details" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForAccept" runat="server" ToolTip="Allow Changed To Dealer" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aggregate Name" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAggregateName" runat="server" ToolTip="Aggregate Name" CssClass="LabelLeftAlign" Text='<%# Eval("Aggegrate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Auto Deduction(%)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPartAutoDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("AutoDeduct_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="VAT(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATPercentage" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(Amount)" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ReqDeductPercentage(Amount)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReqDeductPer" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("ReqDeduction_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="D/M Flag" ItemStyle-Width="2%" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDealerOrMTI_Flag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PLabourDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPELabourDetails" runat="server" TargetControlID="CntLabourDetails"
                            ExpandControlID="TtlLabourDetails" CollapseControlID="TtlLabourDetails" Collapsed="true"
                            ImageControlID="ImgTtlLabourDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Labor Details" ExpandedText="Labor Details"
                            TextLabelID="lblTtlLabourDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlLabourDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="99%">
                                        <asp:Label ID="lblTtlLabourDetails" runat="server" Text="Labor Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            ></asp:Label>
                                    </td>
                                    <%--  <td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblLabourRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlLabourDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntLabourDetails" runat="server">
                            <asp:GridView ID="LabourDetailsGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                GridLines="Horizontal" SkinID="NormalGrid" Width="100%">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-CssClass="LabelCenterAlign" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Labor Code" ItemStyle-CssClass="LabelLeftAlign" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabourCode" runat="server" Text='<%# Eval("labour_code") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Labor Description" ItemStyle-CssClass="LabelLeftAlign"
                                        ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabourDesc" runat="server" Text='<%# Eval("Labour_Desc") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Man Hrs" ItemStyle-CssClass="LabelRightAlign" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblManHrs" runat="server" Text='<%# Eval("ManHrs","{0:#0.00}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-CssClass="LabelRightAlign" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Amount" ItemStyle-CssClass="LabelRightAlign"
                                        ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClaimedAmount" runat="server" Text='<%# Eval("Total","{0:#0.00}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("VECV_Share","{0:#0.00}") %>'
                                                Width="90%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                                Text='<%# Eval("Dealer_Share","{0:#0.00}") %>' Width="90%"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" onkeydown="return ToSetKeyPressValueFalse(event,this);"
                                                Text='<%# Eval("Cust_Share","{0:#0.00}") %>' Width="96%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Deduction_Percentage","{0:#0.00}") %>'
                                                Width="90%" onkeypress="return CheckPercentageAmount(event,this);" onblur="return CalculateLabourAcceptedAmtDeductionChanged(event,this);">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Amount" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Accepted_Amount","{0:#0.00}") %>'
                                                Width="90%" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction Remarks" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Deduction_Remark") %>'
                                                TextMode="MultiLine" Width="90%"></asp:TextBox>
                                            <%--onblur="return LabourDeductionChangedRemark(event,this);" --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Change Details" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForAccept" runat="server" ToolTip="Allow Changed To Dealer" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Auto Deduction(%)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLaborAutoDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("AutoDeduct_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATPercentage" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(Amount)" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsAccidental" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccidental" runat="server" Text='<%# Eval("IsAccidental") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsTaxable" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaxable" runat="server" Text='<%# Eval("IsTaxable") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ReqDeductPercentage(Amount)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReqDeductPer" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("ReqDeduction_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PLubricantDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPELubricantDetails" runat="server" TargetControlID="CntLubricantDetails"
                            ExpandControlID="TtlLubricantDetails" CollapseControlID="TtlLubricantDetails"
                            Collapsed="false" ImageControlID="ImgTtlLubricantDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Lubricant Details"
                            ExpandedText="Lubricant Details" TextLabelID="lblTtlLubricantDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlLubricantDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                        <asp:Label ID="lblTtlLubricantDetails" runat="server" Text="Lubricant Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            ></asp:Label>
                                    </td>
                                    <%--<td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblLubricantRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlLubricantDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntLubricantDetails" runat="server" Style="display: none;">
                            <asp:GridView ID="LubricantDetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AutoGenerateColumns="False">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lubricant Description" ItemStyle-Width="35%" ItemStyle-CssClass="LabelLeftAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLubricant_Desc" runat="server" Text='<%# Eval("Lubricant_Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Qty" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty","{0:#,#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Std. Qty" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStdQty" runat="server" Text='<%# Eval("StndQty","{0:#,#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="4%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Amount" ItemStyle-Width="6%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClaimedAmount" runat="server" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VECV_Share","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Dealer_Share","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Cust_Share","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Deduction_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Amount" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Accepted_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction Remarks" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="GridTextBoxForString" Width="90%"
                                                TextMode="MultiLine" Text='<%# Eval("Deduction_Remark") %>'></asp:TextBox>
                                            <%--onblur="return LubricantDeductionChangedRemark(event,this);"--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Change Details" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForAccept" runat="server" ToolTip="Allow Changed To Dealer" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Auto Deduction(%)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLubAutoDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("AutoDeduct_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATPercentage" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(Amount)" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ReqDeductPercentage(Amount)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReqDeductPer" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("ReqDeduction_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="D/M Flag" ItemStyle-Width="2%" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLubDealerOrMTI_Flag" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PSublettDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPESublettDetails" runat="server" TargetControlID="CntSublettDetails"
                            ExpandControlID="TtlSublettDetails" CollapseControlID="TtlSublettDetails" Collapsed="true"
                            ImageControlID="ImgTtlSublettDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Sublet Details" ExpandedText="Sublet Details"
                            TextLabelID="lblTtlSublettDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlSublettDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="82%">
                                        <asp:Label ID="lblTtlSublettDetails" runat="server" Text="Sublet Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"
                                            ></asp:Label>
                                    </td>
                                    <%--  <td class="ContaintTableHeader" width="8%">
                                            Count:
                                        </td>
                                        <td class="ContaintTableHeader" width="8%">
                                            <asp:Label ID="lblSubletRecCnt" runat="server" Text="0"></asp:Label>
                                        </td>--%>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlSublettDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntSublettDetails" runat="server" Style="display: none;">
                            <asp:GridView ID="SubletDetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1  %>' CssClass="LabelCenterAlign"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sublet Code" ItemStyle-CssClass="LabelLeftAlign" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabourCode" runat="server" Text='<%# Eval("labour_code") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sublet Description" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubletDesc" runat="server" Text='<%# Eval("Sublet_Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Man Hrs" ItemStyle-CssClass="LabelRightAlign" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblManHrs" runat="server" Text='<%# Eval("ManHrs","{0:#0.00}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-CssClass="LabelRightAlign" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claimed Amount" ItemStyle-Width="6%" ItemStyle-CssClass="LabelRightAlign">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClaimedAmount" runat="server" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MTI(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVECVShare" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VECV_Share","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDealerShare" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Dealer_Share","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cust(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCustShare" runat="server" CssClass="GridTextBoxForAmount" Width="96%"
                                                Text='<%# Eval("Cust_Share","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Deduction_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acc Amount" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccAmount" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("Accepted_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduction Remarks" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="GridTextBoxForString" Width="90%"
                                                TextMode="MultiLine" Text='<%# Eval("Deduction_Remark") %>'></asp:TextBox>
                                            <%--onblur="return SubletDeductionChangedRemark(event,this);"--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Change Details" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForAccept" runat="server" ToolTip="Allow Changed To Dealer" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Auto Deduction(%)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSubletAutoDeduction" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("AutoDeduct_Percentage","{0:#0.00}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(%)" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATPercentage" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT(Amount)" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVATAmt" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("VAT_Amount","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsTaxable" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaxable" runat="server" Text='<%# Eval("IsTaxable") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ReqDeductPercentage(Amount)" ItemStyle-Width="5%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl ">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReqDeductPer" runat="server" CssClass="GridTextBoxForAmount" Width="90%"
                                                Text='<%# Eval("ReqDeduction_Percentage","{0:#0.00}") %>' onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 9%; font-weight: 700;">
                    <table id="TblTotal" runat="server" border="1" width="50%" class="NormalTable">
                        <tr>
                            <td style="text-align: center" colspan="2">
                                <b>
                                    <asp:Label ID="lblClaimAmt" runat="server" Text="Claimed Amount" Font-Bold="true"></asp:Label>
                                </b></td>
                            <td style="text-align: center" colspan="2">
                                <b>
                                    <asp:Label ID="lblAcceptedAmt" runat="server" Text="Accepted Amount" Font-Bold="true"></asp:Label></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <b>Parts: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgPartAmount" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    Text="0"> </asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                <b>Parts: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtPartAmount" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    Text="0"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr id="PTax1" runat="server">
                            <td style="text-align: right">
                                <b>Tax: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgPartTaxAmount" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    Text="0"> </asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                <b>Tax: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtPartTaxAmount" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    Text="0"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <%--  <b>Labor Taxable: </b>--%>
                                <asp:Label ID="lblLaborTaxable" runat="server" Text="<b>Labor Taxable:</b>"></asp:Label></td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgLabourAmount" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                            <td style="text-align: right">
                                <%--<b>Labor Taxable: </b>--%>
                                <asp:Label ID="lblLaborTaxable1" runat="server" Text="<b>Labor Taxable:</b>"></asp:Label></td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtLabourAmount" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <b>Sublet: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgSubletAmount" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                            <td style="text-align: right">
                                <b>Sublet: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtSubletAmount" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                        </tr>
                        <tr id="LTax1" runat="server">
                            <td style="text-align: right">
                               <%-- <b>Tax(Labor + Sublet): </b>--%>
                                <asp:Label ID="lblOrgLabourST" runat="server" Text="<b>Tax(Labor + Sublet): </b>"></asp:Label>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgLabourST" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    Text="0"> </asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                <%--<b>Tax(Labor + Sublet): </b>--%>
                                <asp:Label ID="lblAccLabourST" runat="server" Text="<b>Tax(Labor + Sublet): </b>"></asp:Label>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtAccLabourST" runat="server" CssClass="TextForAmount" Font-Bold="true"
                                    Text="0"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr id="LTTax1" runat="server">
                            <td style="text-align: right">
                                <b>Labor NonTaxable: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgNonTaxLabourAmt" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                            <td style="text-align: right">
                                <b>Labor NonTaxable: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtAccNonTaxLabourAmt" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <b>Lubricant: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgLubricantAmount" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                            <td style="text-align: right">
                                <b>Lubricant: </b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtLubricantAmount" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <b>For Job:</b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtOrgAccClaimAmt" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                            <td style="text-align: right">
                                <b>For Job:</b>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="txtAccClaimAmt" runat="server" CssClass="TextForAmount" Text="0"
                                    Font-Bold="true"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="width: 14%;">
                    <asp:Button ID="btnSave" runat="server" Text="Save Job Details" CssClass="CommandButton"
                        OnClick="btnSave_Click" OnClientClick="return MSGVAlidation()" />
                    &nbsp;&nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="CommandButton" OnClick="btnBack_Click" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtTotalPartAmount" runat="server" Width="1px"> </asp:TextBox>
                    <asp:TextBox ID="txtTotalLabourAmount" runat="server"> </asp:TextBox>
                    <asp:TextBox ID="txtTotalLubricantAmount" runat="server"> </asp:TextBox>
                    <asp:TextBox ID="txtTotalSubletAmount" runat="server"> </asp:TextBox>
                    <asp:TextBox ID="txtDomestic_Export" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:HiddenField ID="hdnHeaderPartDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnHeaderLabourDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnHeaderLubricantDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnHeaderSubletDeduction" runat="server" Value="0" />

                    <asp:HiddenField ID="hdnHeaderPrePartDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnHeaderPreLabourDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnHeaderPreLubricantDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnHeaderPreSubletDeduction" runat="server" Value="0" />
                    <asp:HiddenField ID="HdnClaimType" runat="server" Value="" />
                    <asp:HiddenField ID="hdnPartTax" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnLabourST" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIsAccidental" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnIsTaxableLbr" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnLabourSTNill" runat="server" Value="0.00" />
                    <asp:HiddenField ID="hdnOrgAccdentalAmtTotal" runat="server" Value="0.00" />
                    <asp:HiddenField ID="hdnAccAccdentalAmtTotal" runat="server" Value="0.00" />
                    <asp:HiddenField ID="hdnCurrency" runat="server" Value="" />
                    <asp:Label ID="lblPartRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblLabourRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblLubricantRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblSubletRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>    
                    <asp:TextBox ID="txtPCRHDRID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>         
                    <asp:TextBox ID="txtJobHDRID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>                             
                    <asp:HiddenField ID="hdnISDocGST" runat="server" />   
                    <asp:HiddenField ID="hdnInvType" runat="server" />   
                    <asp:TextBox ID="txtDealerCode" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>