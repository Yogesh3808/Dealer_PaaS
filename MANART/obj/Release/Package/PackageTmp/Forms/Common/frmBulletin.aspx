<%@ Page Title="MTI-Bulletin" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmBulletin.aspx.cs" Inherits="MANART.Forms.Common.frmBulletin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />

    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                // event.returnValue = false;
                //alert(message);
                // return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }
        function AtPageLoad() {
            //FirstTimeGridDisplay('ContentPlaceHolder1_');

            //setTimeout("disableBackButton()", 0);
            //disableBackButton();
            return true;
        }

        function refresh() {
            if (event.keyCode == 116 || event.keyCode == 8) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }

        function Validation() {
            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_drpDocType").value == "0") {
                errMessage += "*Please Select Document Type.\n";
            }

            if (errMessage != "") {
                alert(errMessage);
                return false;
            }
            else {
                return true;
            }
        }
        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
//            //debugger;
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);

            var filename = objFileUpload.value;
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            var sFileExtension = filename.split('.')[filename.split('.').length - 1].toLowerCase();
            var iFileSize = objFileUpload.files[0].size;
            var iConvert = (iFileSize / 1048576).toFixed(2);

            if (!(sFileExtension === "pdf" || sFileExtension === "zip")) {
                txt = "File type : " + sFileExtension + "\n\n";
                // txt += "Size: " + iConvert + " MB \n\n";
                txt += "Please make sure your file is in pdf or zip format and less than 3 MB.\n\n";
                alert(txt);
                return false;
            }
            //else if ((sFileExtension === "pdf" || sFileExtension === "zip") || iFileSize > 3145728) {
            //    txt = "Size: " + iConvert + " MB \n\n";
            //    txt += "Please make sure your file is in pdf or zip format and less than 3 MB.\n\n";
            //    alert(txt);
            //    return false;
            //}

            var errMessage = "";
            if (document.getElementById("ContentPlaceHolder1_drpDocType").value == "0") {
                errMessage += "*Please Select Document Type.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtDocName").value == "") {
                errMessage += "*Please Enter Document Name Invoice No.\n";
            }
            if (document.getElementById("ContentPlaceHolder1_txtDocHeading").value == "") {
                errMessage += "*Please Enter Document Heading.\n";
            }

            if (errMessage != "") {
                alert(errMessage);
                return false;
            }

            

        }
    </script>
    <script type="text/javascript">
        function disableBackButton() {
            window.history.forward(1);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" >
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Bulletin"></asp:Label>
            </td>
        </tr>
        <tr id="Tr1">
            <td style="width: 14%">
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDocDetails" runat="server" TargetControlID="CntDocDetails"
                        ExpandControlID="CntModelDetails" CollapseControlID="CntModelDetails" Collapsed="false"
                        ImageControlID="ImgTtlDocDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Bulletin Details" ExpandedText="Bulletin Details"
                        TextLabelID="lblTtlDocDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double" ScrollBars="None">
                        <asp:Panel ID="TtlDocDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlDocDetails" runat="server" Text="Bulletin Details"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDocDetails" runat="server" Height="15px"
                                            ImageUrl="~/Images/Plus.png" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="None"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td class="tdLabel" style="width: 10%; padding-left: 10px;">Document Type:
                                </td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="drpDocType" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMDocType" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td style="width: 10%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lblDocName" runat="server" Text="Document Name:"></asp:Label>
                                </td>
                                <td style="width: 25%">
                                    <asp:TextBox ID="txtDocName" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="lblMDocName" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 10%; padding-left: 10px;" class="tdLabel" id="tdlblDocHeading" runat="server">
                                    <asp:Label ID="lblDocHeading" runat="server" Text="Document Heading:"></asp:Label>
                                </td>
                                <td style="width: 25%" id="tdtxtDocHeading" runat="server">
                                    <asp:TextBox ID="txtDocHeading" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="lblMDocHeading" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 18%" colspan="6">
                                    <asp:Button ID="btnShow" Text="Show" runat="server" CssClass="btn btn-search" OnClientClick=" return Validation();" OnClick="btnShow_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <%--Style="display: none"--%>
                <asp:Panel ID="UploafFile" runat="server" BorderColor="DarkGray" BorderStyle="Double" Style="display: none">
                    <table id="Table1" runat="server" class="table table-bordered">
                        <tr>
                            <td style="width: 15%; padding-left: 10px;">Select File For Upload
                            </td>
                            <td style="width: 65%; padding-left: 10px;">
                                <asp:FileUpload ID="txtFilePath" runat="server" Width="75%" CssClass="Cntrl1" />
                            </td>
                            <td style="width: 10%;">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-search btn-sm" Text="Upload"
                                    OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');" OnClick="btnUpload_Click" />
                                <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <img src="../../Images/Search_Progress.gif" alt="Searchig" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px; color: Red" colspan="4">
                                <p>
                                    1.
                                    <a>Please select the file in the pdf or zip format. </a>
                                </p>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 15%">
                <asp:Panel ID="PChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEChassisFSDetails" runat="server" TargetControlID="CntChassisFSDetails"
                        ExpandControlID="TtlChassisFSDetails" CollapseControlID="TtlChassisFSDetails" Collapsed="false"
                        ImageControlID="ImgTtlChassisFSDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Bulletin List" ExpandedText="Bulletin List"
                        TextLabelID="lblTtlChassisFSDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlChassisFSDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlChassisFSDetails" runat="server" Text="Bulletin List" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);"
                                        onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlChassisFSDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="Horizontal">
                        <asp:GridView ID="gvBulletin" runat="server" GridLines="Horizontal" CssClass="table table-bordered table-condensed"
                            Width="100%" AllowPaging="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                            AutoGenerateColumns="False" HeaderStyle-Wrap="true"
                            PageSize="10" OnPageIndexChanging="gvBulletin_PageIndexChanging"
                            OnRowDataBound="gvBulletin_RowDataBound" OnRowCommand="gvBulletin_RowCommand">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="File  Name" ItemStyle-Width="18%">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>' />--%>
                                        <a id="achFileName" runat="server" title="Click here to download file"><%# Eval("File_Name") %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Name" ItemStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("Doc_Name") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Heading" ItemStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocument_Heading" runat="server" Text='<%# Eval("Doc_Heading") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Path" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoc_Type" runat="server" Text='<%# Eval("Doc_Type") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Path" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocument_Path" runat="server" Text='<%# Eval("Path") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" Width="60" Text="View" CommandName="EditButton" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
