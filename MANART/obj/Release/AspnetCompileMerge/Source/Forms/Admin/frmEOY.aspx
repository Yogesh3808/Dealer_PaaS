<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmEOY.aspx.cs" Inherits="MANART.Forms.Admin.frmEOY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .WordWrap {
            width: 100%;
            word-break: break-all;
        }

        .WordBreak {
            width: 100px;
            OVERFLOW: hidden;
            TEXT-OVERFLOW: ellipsis;
        }
    </style>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <%--<script src="../../Scripts/jquery.datepick.js"></script>--%>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <style type="text/css">
        .scrolling-table-container {
            overflow-y: scroll;
            overflow-x: scroll;
        }
    </style>

    <script type="text/javascript">
        function confirm() {
            var strConfirm = confirm("Are you sure, you want to Done the EOY ?");
            if (strConfirm == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
        }

            .modalPopup .header {
                background-color: #2FBDF1;
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .body {
                min-height: 50px;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .footer {
                padding: 3px;
            }

            .modalPopup .yes, .modalPopup .no {
                height: 23px;
                color: White;
                line-height: 23px;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
            }

            .modalPopup .yes {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
            }

            .modalPopup .no {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" style="width: 100%" class="" border="1">
            <tr id="TitleOfPage">
                <td class="panel-heading" align="center">
                    <asp:Label ID="lblTitle" CssClass="panel-title" runat="server" Text=""> </asp:Label>
                    <div style="display: none">
                        <asp:Label ID="lblHighValueMsg" runat="server" Text="(  High Value Request Amount:  "> </asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="TblControl1">
                <td colspan="2">
                    <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="well well-sm table table-bordered" align="center" id="dvNote" runat="server">
                        <%--<span style="color: Red">( Note: Dealer Pending Documents list Displayed here,After Confirm all pending document EOY Button Enabled for Perticular Dealer.) </span>--%>
                        <%--<br />--%>
                        <%--<span style="color: Red">* An Asterisk indicates required information </span>--%>
                        <asp:Button ID="btnEOY" runat="server" CssClass="btn btn-search btn-sm" Text="  Begin EOY   " OnClientClick="return confirm();" OnClick="btnEOY_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDownload" runat="server" CssClass="btn btn-search btn-sm" Text="Download" OnClick="btnDownload_Click" />
                    </div>
                </td>
            </tr>
            <tr id="TblControl2">
                <%--<td style="height:10%; width: 15%; padding-left: 25%" class="">class="well well-sm table table-bordered"
                    <asp:Button ID="btnEOY"  runat="server" CssClass="btn btn-search btn-sm" Text="EOY" OnClick="btnEOY_Click" />
                </td>
                <td style="height:10%; width: 18%; align-items:center">
                    <asp:Button ID="btnDownload"   runat="server" CssClass="btn btn-search btn-sm" Text="Download" OnClick="btnDownload_Click"  /></td>--%>
                <td colspan="2">
                    <div class="well well-sm table table-bordered" align="center" id="Div1" runat="server">
                        <span style="color: Red">( Note: Below list shows documents that should be closed before EOY. EOY will not proceed till the documents are closed.) </span>
                        <%--<asp:Button ID="btnEOY" runat="server" CssClass="btn btn-search btn-sm" Text="   EOY   " OnClick="btnEOY_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDownload" runat="server" CssClass="btn btn-search btn-sm" Text="Download" OnClick="btnDownload_Click" />--%>
                    </div>
                </td>
            </tr>
            <tr id="TblControl3">
                <td colspan="2">
                    <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                            ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="false"
                            ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Pending Documents List" ExpandedText="Pending Documents List"
                            TextLabelID="lblTtlPartDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlPartDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlPartDetails" runat="server" Text="Pending Documents Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            Style="display: none;">
                            <div class="scrolling-table-container WordWrap">
                                <asp:GridView ID="PartDetailsGrid" runat="server" AllowPaging="false" Width="100%" ShowHeaderWhenEmpty="true" EmptyDataText="No Pending Documents. Dealer can proceed for EOY (END OF YEAR)"
                                    AutoGenerateColumns="False" EditRowStyle-BorderColor="Black"
                                    GridLines="Horizontal" SkinID="NormalGrid" OnDataBound="PartDetailsGrid_DataBound" OnRowDataBound="PartDetailsGrid_RowDataBound"
                                    AlternatingRowStyle-Wrap="true"
                                    EditRowStyle-Wrap="true"
                                    HeaderStyle-Wrap="true">
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                    <RowStyle CssClass="GridViewRowStyle" />
                                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Status" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="90px" DataField="Status" HeaderStyle-Width="25%" />
                                         <%--<asp:TemplateField HeaderText="Status" ItemStyle-Width="50%">
                                            <ItemTemplate> 
                                                <asp:Label ID="txtStatus" runat="server" CssClass="LabelLeftAlign"  Text='<%# Eval("Status") %>' Width="96%"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="true" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDcID" runat="server" Text='<%# Eval("ID") %>' Width="1%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Doc No." ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtDCNo" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("DocNo") %>' Width="96%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Doc Date" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtDCDate" runat="server" CssClass="LabelLeftAlign" Text='<%# Eval("DocDate") %>' Width="96%"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="true" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 14%" colspan="2">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerCode" runat="server" Width="1%" Text="" CssClass="HideControl"></asp:TextBox>
                </td>
            </tr>
        </table>
        <%-- PopUpScreen --%>
        <cc1:ModalPopupExtender ID="ModalPopUpExtender" runat="server" TargetControlID="lblTragetID" PopupControlID="pnlPopupWindow"
            OkControlID="btnOK" BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
        <asp:Label ID="lblTragetID" runat="server"></asp:Label>
        <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup" Style="display: none;">
            <div class="header">
                Information
            </div>
            <div class="body">
               EOY completed successfully.
               <br />
                Click on Logout to close dCAN and Login again.
            </div>
            <div class="footer" align="right">
                <asp:Button ID="btnLogOut" runat="server" Text=" LogOut " CssClass="btn btn-search" OnClick="btnLogOut_Click" />
            </div>
        </asp:Panel>
        <%-- END --%>
    </div>
</asp:Content>
