<%@ page title="" language="C#" theme="SkinFile" autoeventwireup="true" codebehind="frmSelectMultiLabour.aspx.cs" inherits="MANART.Forms.Common.frmSelectMultiLabour" %>

<%@ register assembly="ASPnetPagerV2_8" namespace="ASPnetControls" tagprefix="cc2" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Details</title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />

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
        function ChkLabourClick(objImgControl) {
            debugger;
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            //var txtparst = document.getElementById('txtPartIds');
            //var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds');
            //var objLbrSelection = document.getElementById('ContentPlaceHolder1_DrpLabourSelect');
            //var txtparst = document.getElementById('ContentPlaceHolder1_txtPartIds_Labour');
            //var objLbrSelection = document.getElementById('ContentPlaceHolder1_DrpLabourSelect');
            var txtparst = document.getElementById('txtPartIds');
            var objLbrSelection = document.getElementById('DrpLabourSelect');

            var ArrOfLabDtls;
            var removePartID;
            //var sLabourID = objRow[1].innerText;
            //var sLabourCode = objRow[2].innerText;
            //var sLabourName = objRow[3].innerText;
            //var sLabManHrs = objRow[4].innerText;
            //var sLabRate = objRow[5].innerText;
            //var sLabTotal = objRow[6].innerText;
            objLbrSelection.disabled = true;

            var sLabourID = objRow[2].childNodes[1].innerText;
            var sLabourCode = objRow[3].childNodes[1].innerText;
            var sLabourName = objRow[4].childNodes[1].innerText;
            var sLabManHrs = objRow[5].childNodes[1].innerText;
            var sLabRate = objRow[6].childNodes[1].innerText;
            var sLabTotal = objRow[7].childNodes[1].innerText;
            var sLabTag = objRow[8].childNodes[1].innerText;
            var sGroupCode = objRow[9].childNodes[1].innerText;
            var sTax = objRow[10].childNodes[1].innerText;
            var sTax1 = objRow[11].childNodes[1].innerText;
            var sTax2 = objRow[12].childNodes[1].innerText;
            var estDtlID = objRow[13].childNodes[1].innerText;
            //debugger;
            var sMiscDesc = objRow[13].childNodes[3].innerText;
            var sOutSubDesc = objRow[13].childNodes[5].innerText;

            ArrOfLabDtls = sLabourID + '<--' + sLabourCode + '<--' + sLabourName + '<--' + sLabManHrs + '<--' + sLabRate + '<--' + sLabTotal + '<--' + sLabTag + '<--' + sGroupCode
            + '<--' + sTax + '<--' + sTax1 + '<--' + sTax2 + '<--' + estDtlID + '<--' + sMiscDesc + '<--' + sOutSubDesc;

            //ArrOfLabDtls = sLabourID + '#' + sLabourCode + '#' + sLabourName + '#' + sLabManHrs + '#' + sLabRate + '#' + sLabTotal + '#' + sLabTag + '#' + sGroupCode
            //+ '#' + sTax + '#' + sTax1 + '#' + sTax2 + '#' + estDtlID + '#' + sMiscDesc + '#' + sOutSubDesc;

            if (objImgControl.checked == true) {
                if (txtparst.value == "") {
                    txtparst.value = ArrOfLabDtls;
                }
                else {
                    txtparst.value = txtparst.value + '#' + ArrOfLabDtls;
                }

            } else {
                removePartID = txtparst.value;

                var afterRemove = "";
                var arr = removePartID.split("#");
                txtparst.value = "";
                var arrlen = arr.length;
                for (var i = 0; i < arrlen; i++) {
                    if (arr[i] == ArrOfLabDtls) {
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
    <style type="text/css">
        .CommandButton {
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:scriptmanager id="ToolkitScriptManager1" runat="server">
        </asp:scriptmanager>

        <asp:updatepanel id="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="PageTable table-responsive" border="1" width="100%">
                    <tr id="TitleOfPage" class="panel-heading">
                        <td class="panel-title" align="center" style="width: 14%">
                            <asp:Label ID="lblTitle" runat="server" Text="" ForeColor="White">
                            </asp:Label>
                            <asp:Label ID="lblMessage" runat="server" Text="" Visible="false">
                            </asp:Label>
                        </td>
                    </tr>
                    <%--style="background-color: #efefef;"--%>
                    <tr id="TblControl">
                        <td style="width: 14%">
                            <div align="center" class="ContainTable">
                                <table style="background-color: #efefef;" width="100%">
                                    <tr align="center">
                                        <td style="padding-left:10px;">
                                            <asp:DropDownList ID="DrpLabGrp" runat="server"  >
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="padding-left:10px;">Search:
                                        </td>
                                        <td class="tdLabel" style="padding-left:10px;" >
                                            <asp:TextBox ID="txtSearch" runat="server" ></asp:TextBox>
                                        </td>
                                        <td class="tdLabel" style="padding-left:10px;" >
                                            <asp:DropDownList ID="DdlSelctionCriteria" runat="server" CssClass="">
                                                <%--Sujata 27012011 Set Value For Labour Code(L) and Name(N)--%>
                                                <asp:ListItem Selected="True" Value="L">Labour Code</asp:ListItem>
                                                <asp:ListItem Value="N">Labour Name</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 25%;padding-left:10px;">
                                            <asp:DropDownList ID="DrpLabourSelect" runat="server" CssClass="">
                                                <%--Sujata 27012011 Set Value For Labour Code(L) and Name(N)--%>
                                               <%-- <asp:ListItem Selected="True" Value="D">Paid Labour</asp:ListItem>
                                                <asp:ListItem Value="C">Warranty Labour</asp:ListItem>
                                                <asp:ListItem Value="E">Estimated Labour</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 15%; padding-left:10px;">
                                            <asp:DropDownList ID="DrpSelFrom" runat="server" CssClass="ComboBoxFixedSize">
                                                <asp:ListItem Selected="True" Value="M">From Master</asp:ListItem>
                                                <asp:ListItem Value="E">From Estimate</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdLabel" style="width: 10%; padding-left:10px;">
                                            <asp:Button ID="btnSave" runat="server" Text="Search" CssClass="btn btn-search btn-sm"
                                                 OnClick="btnSave_Click" />
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="tdLabel" style="width: 10%;padding-left:10px;">
                                            <%-- Sujata 25012011 Add ReturnMultiWLabourDetails instead of ReturnMultiLabourDetails--%>
                                            <%--<asp:Label ID="lblBack" runat="server" Text="Back" onClick="return ReturnMultiWLabourDetails();"
                                    CssClass="CommandButton"></asp:Label>--%>
                                            <asp:Button ID="btnBack" runat="server" Text="OK" CssClass="btn btn-search btn-sm"
                                                 OnClick="btnBack_Click"></asp:Button>
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 110px;">
                            <asp:Panel ID="PLabourDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader" ScrollBars="Vertical" Height="500px">
                                <%-- Sujata 25012011 Add AllowPaging="True" PageSize="20" for Grid --%>
                                <asp:GridView ID="LabourDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                                    EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                    HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%" AutoGenerateColumns="false"
                                    AllowPaging="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <%-- Sujata 25012011 Add OnClick( code) --%>
                                                <asp:CheckBox ID="ChkLabour" OnClick="return ChkLabourClick(this);" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Code." ItemStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabourCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LabourCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Name" ItemStyle-Width="70%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabourName" runat="server" CssClass="LabelCenterAlign"  Text='<%# Eval("Labour_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Man Hrs." ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManHrs" runat="server" Text='<%# Eval("Man Hrs","{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <%-- Sujata 02022011
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Labour_Rate") %>'></asp:Label>--%>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Labour_Rate","{0:#0.00}") %>'></asp:Label>
                                                <%--Sujata 02022011--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total" ,"{0:#0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Labour Tag" ItemStyle-Width="5%" ItemStyle-CssClass="LabelRightAlign">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabTag" runat="server" Text='<%# Eval("Lab_Tag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group code" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartGroup" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("group_code")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax1" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax1" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax1")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax2" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax2" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("Tax2")%>'></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="EstID" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstDtlID" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("EstDtlID")%>'></asp:Label>
                                            <asp:Label ID="lblMiscDesc" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("AddLbrDescriptionID")%>'></asp:Label>
                                            <asp:Label ID="lblOutSubDesc" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("out_lab_desc")%>'></asp:Label>                                            
                                        </ItemTemplate>
                                         <HeaderStyle CssClass="HideControl" />
                                         <ItemStyle CssClass="HideControl" Width="1%" />
                                    </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Wrap="True" />
                                    <EditRowStyle BorderColor="Black" Wrap="True" />
                                    <AlternatingRowStyle Wrap="True" />
                                </asp:GridView>
                                <%--Sujata 25012011 --%>
                                <cc2:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" Width="1000px"
                                    GenerateGoToSection="true" />
                                <%--Sujata 25012011--%>
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
        </asp:updatepanel>
    </form>
</body>
</html>

