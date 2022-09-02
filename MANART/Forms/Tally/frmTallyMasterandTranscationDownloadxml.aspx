<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmTallyMasterandTranscationDownloadxml.aspx.cs"
    Inherits="MANART.Forms.Tally.frmTallyMasterandTranscationDownloadxml" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/ScrollableGridPlugin_ASP.NetAJAX_2.0.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Tally Xmls Download"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="">
                    <tr>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 14%">
                <%--<asp:Label  ID="lblDownloadPath" Text="Download Folder Path" runat="server" />
                 <asp:TextBox ID="txtDownloadPath" runat="server" Text='<%# Eval("FileName") %>' />--%>
                <asp:Button ID="btnGetFiles" Text="Get Tally Files" visible="false"  runat="server" OnClick="btnGetFiles_Click" />
                 <asp:Label ID="lblNoMsg" runat="server" Visible="false"> </asp:Label>
                <%--<asp:GridView ID="gvDetails" CellPadding="5" runat="server" AutoGenerateColumns="false"  OnRowCommand="gvDetails_RowCommand" >--%>
                <asp:GridView ID="gvDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid" ShowFooter="true"
                    CssClass="table table-condensed table-bordered" 
                    AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                    EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                    OnRowCommand="gvDetails_RowCommand" Width="25%">
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                            <ItemTemplate>
                                <asp:Label ID="lblNo" runat="server" Text='<%# Container.DataItemIndex + 1   %>' />
                                <%--Text='<%# Container.DataItemIndex +1  %>'--%>
                            </ItemTemplate>
                            <ItemStyle Width="1%" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField>
<ItemTemplate>
<asp:Label  ID="lblFilename" runat="server" Text='<%# Eval("FileName") %>' />

</ItemTemplate>
</asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="File Name" ItemStyle-Width="50%">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblFile" OnClick="btnDownload_Click" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
       onmouseover="SetCancelStyleonMouseOver(this);" 
       Text='<%# Eval("FileName") %>' ToolTip="Click Here To Open The File" Width="90%"></asp:Label>--%>

                                <%--<asp:LinkButton  ID="lnkbtn" runat="server" ForeColor="#49A3D3" 
                                                            Text='<%# Eval("FileName") %>' ToolTip="Click Here To Open The File" Width="90%"></asp:LinkButton>--%>

                                <asp:Label ID="lblFileName" runat="server"
                                    Text='<%# Eval("FileName") %>' Width="90%"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Path" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                    ItemStyle-CssClass="HideControl">
                            <ItemTemplate>
                                <asp:Label ID="lblFilePath" runat="server" Text='<%# Eval("FilePath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="Text" HeaderText="FileName" />
<asp:BoundField DataField="Text" HeaderText="FilePath" />--%>
                    </Columns>
                </asp:GridView>


            </td>
          
        </tr>
        <tr>
         <td style="width: 14%">
               

            <asp:Button ID="btnDownload" Text="Download Files" runat="server" OnClick="btnDownload_Click" />
            <asp:Label ID="lblMessage" runat="server" Visible="false"  Text="Files are downloaded successfully" > </asp:Label></td>

             <%-- Enter Drive :--%>
             <asp:TextBox ID="txtdrive" runat="server" Text="C:\" width="20%" Visible ="false" ></asp:TextBox>
             <asp:Button ID="btnPath" Text="Select Path" runat="server" Visible="false"  OnClick="btnpath_Click" />

            <asp:FileUpload ID="FileUpload1"  Visible="false"   runat="server" />

        </tr>
        <tr>
            <td style="width: 14%"></td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnPoTypeID" runat="server" Value="" />
                <asp:HiddenField ID="hdnPartsIDs" runat="server" Value="" />
                <asp:HiddenField ID="hdnSelectedPartID" runat="server" Value="" />
              
            </td>
        </tr>
    </table>
</asp:Content>
