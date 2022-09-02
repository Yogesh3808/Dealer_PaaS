<%@ Page Title="MTI-Select Multipart" Theme="SkinFile" Language="C#" AutoEventWireup="true" CodeBehind="frmMReceiptPOSel.aspx.cs" Inherits="MANART.Forms.Spares.frmMReceiptPOSel" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Details</title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
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


        function ChkMRPartClick(objImgControl) {
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            var txtparst = document.getElementById('txtPartIds');
            var ArrOfPartDtls = '';
            var removePartID;
            //    var sPartID = objRow[1].innerText;
            //    var sParFOBRt = objRow[2].innerText;
            //    var sParMOQ = objRow[3].innerText;
            //    var sParNo = objRow[4].innerText;
            //    var sParName = objRow[5].innerText;
            //    var sNDPRate = objRow[6].innerText;

            for (i = 1; i < objRow.length; i++) {
                if (i == objRow.length - 1)
                    ArrOfPartDtls = ArrOfPartDtls + objRow[i].innerText;
                else
                    ArrOfPartDtls = ArrOfPartDtls + objRow[i].innerText + '<--';
            }
            //ArrOfPartDtls = sPartID + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;
            //ArrOfPartDtls = sPartID + '<--' + sParFOBRt + '<--' + sParMOQ + '<--' + sParNo + '<--' + sParName + '<--' + sNDPRate;

            if (objImgControl.checked == true) {
                if (txtparst.value == "") {
                    txtparst.value = ArrOfPartDtls;
                }
                else {
                    txtparst.value = txtparst.value + '#' + ArrOfPartDtls;
                }

            } else {
                removePartID = txtparst.value;

                var afterRemove = "";
                var arr = removePartID.split("#");
                txtparst.value = "";
                var arrlen = arr.length;
                for (var i = 0; i < arrlen; i++) {
                    if (arr[i] == ArrOfPartDtls) {
                        // arr.splice(i, 1);

                    }
                    else {

                        if (txtparst.value == "") {
                            txtparst.value = arr[i];
                        }
                        else {
                            txtparst.value = txtparst.value + '#' + arr[i];
                        }
                    }
                }
                // txtparst.value = arr;
            }
            return txtparst.value;
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
                <table class="PageTable table-responsive" border="1"  >
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text="">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">

                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef;">
                                    <tr align="center">
                                        <td class="tdLabel" >Search:
                                        </td>
                                        <td class="tdLabel" >
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass=""></asp:TextBox>
                                        </td>

                                        <td class="tdLabel" >
                                            <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="">
                                                <asp:ListItem Selected="True" Value="PONO">PO No</asp:ListItem>
                                                <asp:ListItem Value="PARTNO">Part No</asp:ListItem>
                                                <asp:ListItem Value="PARTNAME">Part Name</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel">
                                            <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                                OnClick="btnSave_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="tdLabel" >
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-search btn-sm "
                                                OnClick="btnBack_Click"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="tdLabel" style="width: 7%;"></td>
                                        <td class="tdLabel" style="width: 15%;" align="left" colspan="2">
                                            <asp:Label ID="lblNMsg" runat="server" Font-Size="8" CssClass="Mandatory" Text='Search Not Found...!'></asp:Label>
                                        </td>
                                        <td class="tdLabel" style="width: 15%;"></td>
                                        <td class="tdLabel" style="width: 10%;"></td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%" CssClass="table table-condensed table-bordered"
                                    AutoGenerateColumns="false" AllowPaging="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkMRPartClick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PartID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PartID") %>'></asp:Label>

                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="POHDRID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOHdrID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_Hdr_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO No." ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("PO_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="80%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="LabelRightAlign" Width="5%" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <HeaderStyle Wrap="True" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                                <cc2:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" Width="1000px"
                                    GenerateGoToSection="true" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="TmpControl">
                        <td style="width: 15%">
                            <asp:TextBox ID="txtPartIds" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
