<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSelectMultiPartSearch.aspx.cs"
    Title="MTI-Select Multipart" Theme="SkinFile" Inherits="MANART.Forms.Spares.frmSelectMultiPartSearch" %>

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
    <script src="../../Scripts/jquery-1.3.2.js"></script>
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

        function ChkSpNDPPartClick(objImgControl) {
            //debugger;
            var objID = $('#' + objImgControl.id);
            var objCol = objID[0].parentNode.parentNode;
            //var txtparst = document.getElementById("ContentPlaceHolder1_txtPartIds");
            //Changed by Vikram Date 17.06.2016
            //objImgControl.parentNode.parentNode.childNodes;
            //var objCol = objImgControl.parentNode.parentNode
            var txtparst = document.getElementById('txtPartIds');
            //var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds');

            var ArrOfPartDtls = '';
            var removePartID;
            //    var sPartID = objRow[1].innerText;
            //    var sParFOBRt = objRow[2].innerText;
            //    var sParMOQ = objRow[3].innerText;
            //    var sParNo = objRow[4].innerText;
            //    var sParName = objRow[5].innerText;
            //    var sNDPRate = objRow[6].innerText;

            //Changes done for jobcard part selection solution for part type tag not get selected here
            //for (i = 1; i < objCol.cells.length - 1; i++) {
            for (i = 1; i < objCol.cells.length; i++) {
                if (i == objCol.cells.length - 1)
                    ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML;
                else
                    ArrOfPartDtls = ArrOfPartDtls + objCol.cells[i].children[0].innerHTML + '<--';
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
    <form id="form1" runat="server" submitdisabledcontrols="False" visible="True">
        <%--<p>Dialog argments: <span id="arg"></span></p>--%>
        <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="PageTable table-responsive" border="1">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="PageTitle panel-title" align="center">
                            <asp:Label ID="lblTitle" runat="server">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr id="TblControl">
                        <td style="width: 14%">
                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef; width: 70%">
                                    <tr align="center">
                                        <td class="tdLabel">Search:
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="tdLabel">
                                            <asp:DropDownList ID="DdlSelctionCriteria" runat="server">
                                                <asp:ListItem Selected="True" Value="P">Part No</asp:ListItem>
                                                <asp:ListItem Value="N">Part Name</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel">
                                            <%--<asp:Label ID="lblSearch" runat="server" Text="Search" onClick="return SearchTextInGrid('PartDetailsGrid');" CssClass=CommandButton></asp:Label> --%>
                                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                                OnClick="btnSave_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="tdLabel">
                                            <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search btn-sm"
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
                                <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkPart" runat="server" OnClick="return ChkSpNDPPartClick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part No." ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="50%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group code" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Group_code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qty" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFOBRate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Qty","{0:#0.00}")%>'></asp:Label>
                                            </ItemTemplate>
                                             <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MOQ" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMOQ" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("MOQ") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("MRPRate","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                             <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Total","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                             <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BFRGST" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBFRGST" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("BFRGST") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BFRGST_Stock" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBFRGST_Stock" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("BFRGST_Stock","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="HideControl" />
                                            <ItemStyle CssClass="HideControl" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Wrap="True" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                                <cc2:pagerv2_8 id="PagerV2_1" runat="server" oncommand="pager_Command" width="1000px" pagesize="15"
                                    generategotosection="true" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="TmpControl">
                        <td style="width: 15%">
                            <asp:TextBox ID="txtPartIds" CssClass="HideControl txtPartIds" runat="server" Width="1px"></asp:TextBox>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script>
        //document.getElementById('arg').innerHTML = window.dialogArguments;
        //window.returnValue = 
        //document.getElementById('btnBack').click();
    </script>
</body>
</html>
