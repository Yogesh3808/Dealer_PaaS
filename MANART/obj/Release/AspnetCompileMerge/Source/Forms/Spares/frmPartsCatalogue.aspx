<%@ Page Title="Parts Catalogue" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmPartsCatalogue.aspx.cs" Inherits="MANART.Forms.Spares.frmPartsCatalogue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="~/WebParts/Location.ascx" TagPrefix="uc3" TagName="Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <script>
        function ShowAttachDocument(objFileControl) {
            var objRow = objFileControl.parentNode.parentNode.childNodes;
            sFileName = (objRow[4].children[0].innerText);
            window.open("../Spares/frmOpenFile.aspx?FileName=" + sFileName, "List", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=200, left=500, width=800, height=600");
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

        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var filename = objFileUpload.value;
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('pdf') == -1) {
                alert('File is not in pdf format.');
                return false;
            }
        }

    </script>

    <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1" align="center">
        <tr class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Catalogue"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel" class="HideControl">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="HideControl">
            <td>
                <%-- <uc6:Location ID="Location" runat="server" OndrpCountryIndexChanged="Location_drpCountryIndexChanged" />--%>
                <uc6:Location ID="Location" runat="server" Visible="false" />
                <uc3:Location runat="server" ID="Location1" Visible="false" />
            </td>
        </tr>
        <tr id="TblControl" runat="server">
            <td style="width: 14%">
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <table class="containtable table-responsive">
                        <tr>
                            <td style="width: 12%; padding-left: 10px;" class="tdLabel" runat="server" id="tdSelectFile">
                                <asp:Label ID="lblSelectFile" runat="server" Text="Select File For Upload: "></asp:Label>
                            </td>
                            <td style="width: 55%;" runat="server" id="tdUploadFile">
                                <table>
                                    <tr>
                                        <td style="width: 75%; padding-left: 10px;">
                                            <asp:FileUpload ID="txtFilePath" runat="server" Width="75%" CssClass="Cntrl1" />
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');"
                                                CssClass="btn btn-search" OnClick="btnUpload_Click" /><br />

                                            <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; color: Red" colspan="4">
                                            <p>
                                                1.
                                    <a>Please select the file in the pdf format only and Parts Catalogue List files only Uploaded</a>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr class="">
            <td style="width: 14%">
                <div align="center" class="containtable" style="height: 400px; overflow: auto;" id="dvbudplan" runat="server">
                    <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <asp:GridView ID="GrdAnnualTarget" runat="server" CssClass="table table-bordered"
                            AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                            GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <%--<HeaderStyle CssClass="FixedHeader" />--%>
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgSelect" runat="server" OnClick="ImgSelect_Click" ImageUrl="~/Images/arrowRight.png"
                                            Style="height: 16px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Id" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModel_Cat_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model Name" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModel_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Model Description" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModel_Desc" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_Desc") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCleintIPAddress" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <cc1:ModalPopupExtender ID="mpeSelectModel" runat="server" TargetControlID="lblTragetID" PopupControlID="pnlPopupWindow"
        OkControlID="btnOK" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup_ModelCatelogue" Style="display: none">
        <table class="PageTable">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="PageTitle panel-title" align="center" width="90%">
                    <asp:Label ID="lblModelName" runat="server">
                    </asp:Label>

                </td>
                <td align="right" width="10%">
                    <asp:Button ID="btnClose" runat="server" Text="X" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <asp:GridView ID="PartDetailsGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-condensed table-bordered"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            HeaderStyle-Wrap="true" SkinID="NormalGrid" Width="100%" EmptyDataText="No records found."
                            AutoGenerateColumns="false">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Container.DataItemIndex + 1  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model Cat ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl" HeaderStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAggregateID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("ID") %>'></asp:Label>
                                        <asp:Label ID="lblModel_Cat_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_Cat_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" Width="1%" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Aggregate Name" ItemStyle-Width="12%">--%>
                                     <asp:TemplateField HeaderText="Function Group" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAggregateName" runat="server" Text='<%# Eval("Agg_Name") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File  Name" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFile" runat="server" Text='<%# Eval("File_Name") %>' Width="90%"
                                            onClick="return ShowAttachDocument(this);" ToolTip="Click Here To Open The File" ForeColor="#49A3D3"
                                            onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Path" ItemStyle-Width="25%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocument_Path" runat="server" Text='<%# Eval("Path") %>' />
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
            <tr id="TmpControl1">
                <td style="width: 15%">
                    <asp:TextBox ID="txtPartIds" CssClass="HideControl txtPartIds" runat="server" Width="1px"></asp:TextBox>

                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
