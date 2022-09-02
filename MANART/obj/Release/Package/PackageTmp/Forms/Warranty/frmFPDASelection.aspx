<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFPDASelection.aspx.cs" Inherits="MANART.Forms.Warranty.frmFPDASelection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FPDA Claim Details</title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/JSFPDAWarrantyClaim.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
        function CloseMe() {
            window.close();
        }
    </script>

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
                $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkPart']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);
            $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkAllPart']:checkbox").click(function () {
                if ($(this).is(':checked'))
                    $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkPart']:checkbox").attr('checked', true);
                else
                    $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkPart']:checkbox").attr('checked', false);
            });
        });
    }

    function CheckUncheckAllCheckBoxAsNeeded() {
        var totalCheckboxes = $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkPart']:checkbox").size();
        var checkedCheckboxes = $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkPart']:checkbox:checked").size();

        if (totalCheckboxes == checkedCheckboxes) {
            $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkAllPart']:checkbox").attr('checked', true);
        }
        else {
            $("#<%=ClaimsDetailsGrid.ClientID%> input[id*='ChkAllPart']:checkbox").attr('checked', false);
        }
    }

    function ReturnChassisValue(objImgControl) {
        debugger;
        var objRow = objImgControl.parentNode.parentNode.childNodes;
        var sValue;
        var ArrOfChassis = new Array();
        for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
            sValue = objRow[cnt].innerText.trim();
            ArrOfChassis.push(sValue);
        }
        //alert(ArrOfChassis);        
        objhdnFor = document.getElementById('hdnFor');
        if (objhdnFor.value == "CouponClaim") {
            if (dGetValue(ArrOfChassis[8]) == 0) {
                alert("Coupon Amount is 0. So jobcard cannot be selected. Pl. discuss with MTI team.");
                return false;
            }
            else {
                window.returnValue = ArrOfChassis;
                window.close();
            }
        }
        else {
            window.returnValue = ArrOfChassis;
            window.close();
        }

    }
    </script>

    <base target="_self" />

</head>
<body>
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="PageTable table-responsive" border="1" width="100%">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="PageTitle panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text="">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">
                            <div class="ContainTable">
                                <table style="background-color: #efefef;" width="90%">
                                    <tr>
                                        <td class="tdLabel" colspan="4">
                                            <asp:Label ID="lblNote" runat="server" Text="Note:" Style="color: Red; font-size: small" ForeColor="Red" Visible="true">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="tdLabel" style="width: 7%;"></td>
                                        <td class="tdLabel" style="width: 15%;">
                                            <asp:Button ID="btnBackToParaent" runat="server" Text="Back" CssClass="btn btn-search btn-sm"
                                                Height="16px" OnClick="btnBackToParaent_Click"></asp:Button>
                                        </td>
                                        <td class="tdLabel" style="width: 15%;">&nbsp;</td>
                                        <td class="tdLabel" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <asp:GridView ID="ClaimsDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" Width="100%" Height="100%"
                                    AutoGenerateColumns="false" AllowPaging="true"
                                    OnPageIndexChanging="ClaimsDetailsGrid_PageIndexChanging">
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
                                        <asp:TemplateField ItemStyle-Width="1%" HeaderStyle-Width="1%">
                                            <HeaderTemplate>
                                                <center><asp:CheckBox ID="ChkAllPartPart" runat="server" CssClass="LabelCenterAlign" ></asp:CheckBox></center>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <center><asp:CheckBox ID="ChkPart" runat="server"/></center>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:Image ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" onClick="return ReturnChassisValue(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="2%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Claim ID" ItemStyle-Width="2%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClaimID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Claim_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref Doc No." ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWarrantyClaim_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("WarrantyClaim_No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref Doc Date" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWarrantyClaim_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("WarrantyClaim_Date","{0:dd-MM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Claim Credit Date" ItemStyle-Width="69%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClaimCredit_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ClaimCredit_Date","{0:dd-MM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Claim Amount" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClaimAmt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Claim_Amt","{0:0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Claim Accepted Amount" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccClaimAmt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Accepted_Claim_Amt","{0:0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Postshipment Inv No" ItemStyle-Width="15%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPostShipment_Inv_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PostShipment_Inv_No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Wrap="True" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                     <tr id="TmpControl">
                        <td style="width: 14%">             
                            <asp:HiddenField ID="hdnFor" runat="server" />   
                        </td>
        </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
