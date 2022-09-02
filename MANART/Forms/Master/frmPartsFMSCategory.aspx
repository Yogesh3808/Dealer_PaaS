<%@ Page Title="Parts Stocking Category" Language="C#" MasterPageFile="~/Header.Master" 
    Theme="SkinFile" EnableViewState="true"
    AutoEventWireup="true" CodeBehind="frmPartsFMSCategory.aspx.cs" Inherits="MANART.Forms.Master.frmPartsFMSCategory" %>

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
    <script type="text/javascript">
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
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search('Parts_StockingNorms_') == -1) {
            //if (filename.search('Parts_FMSCategoryDetails_') == -1) {
                alert('File name is not in given format.');
                return false;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" style="width: 100%" class="table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td align="center" class="panel-title">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Stocking Norms"></asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td>
                <table id="ToolbarContainer" runat="server" border="1" width="100%">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <%-- <uc6:Location ID="Location" runat="server" OndrpCountryIndexChanged="Location_drpCountryIndexChanged" />--%>
                <uc6:Location ID="Location" runat="server" />
                <uc3:Location runat="server" ID="Location1" />
            </td>
        </tr>
        <tr id="TblControl">
            <td>
                <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                   <%-- <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                        ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Parts Stocking Category"
                        ExpandedText="Parts Stocking Category" TextLabelID="lblTtlPartHeaderDetails">
                    </cc1:CollapsiblePanelExtender>--%>
                   <%-- <asp:Panel ID="TtlPartHeaderDetails" runat="server">
                        <table>
                            <tr class="panel-heading">
                                <td align="center" class="panel-title">
                                    <asp:Label ID="lblTtlPartHeaderDetails" runat="server" Text="Parts Stocking Category"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                    <asp:Panel ID="CntPartHeaderDetails" runat="server" BorderColor="Black" BorderStyle="None">

                        <div class="well well-sm table table-bordered" align="center" id="dvNote" runat="server">
                            <span style="color: Red">( Note: Dealer Stocking Norms) </span>
                            <%--<span style="color: Red">( Note: Dealer Categorywise Inventory (ABC & FMS).) </span>--%>
                        </div>
                        <div class="well well-sm table table-bordered" align="center" id="divBtnShow" runat="server">
                            <asp:Button ID="btnShow" Text="Show" runat="server" CssClass="btn btn-search" OnClick="btnShow_Click" />
                        </div>
                        <table id="tblPartsFMSCatUpload" runat="server" class="ContainTable table table-bordered">
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
                                                    OnClick="btnUpload_Click" CssClass="btn btn-search" /><br />
                                                <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px; color: Red" colspan="5">
                                    <p>
                                        1.
                                    <a>Please select the file in the excel format. File name should be in format as 'Parts_StockingNorms_Datestamp'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;Parts_StockingNorms_06042018&#39;. </a>
                                        <br />
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-left: 10px;">
                                    <asp:Label ID="lblListPartNo" runat="server" Text="" Width="100%" ForeColor="Red"
                                        Visible="false"> </asp:Label>
                                    <asp:TextBox ID="txtListPartNo" TextMode="MultiLine" runat="server" Text="" Width="100%" 
                                        ForeColor="Red" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table id="Table2" runat="server" class="ContainTable table table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 18%">No of days
                                </td>
                                <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 18%">No of days
                                </td>
                                <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 18%">No of days
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">Category AF:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_AF" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Category BF:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_BF" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Category CF:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_CF" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td class="tdLabel" style="width: 15%">Category AM:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_AM" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Category BM: 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_BM" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Category CM: 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_CM" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td style="width: 15%" class="tdLabel">Category AS:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_AS" runat="server" CssClass="TextBoxForString" MaxLength="3"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Category BS:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_BS" runat="server" CssClass="TextBoxForString"
                                        MaxLength="3" onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Category CS:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCat_CS" runat="server" MaxLength="3" CssClass="TextBoxForString"
                                        onkeypress="return CheckisNumber(event);"></asp:TextBox>
                                </td>

                            </tr>
                        </table>

                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PChassisHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Style="display: none">
                    <cc1:CollapsiblePanelExtender ID="CPEChassisHeaderDetails" runat="server" TargetControlID="CntChassisHeaderDetails"
                        ExpandControlID="TtlChassisHeaderDetails" CollapseControlID="TtlChassisHeaderDetails"
                        Collapsed="true" ImageControlID="ImgTtlChassisHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show FMS Details"
                        ExpandedText="Hide Dealer Details" TextLabelID="lblTtlChassisHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlChassisHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <%--                                    <asp:Label ID="lblTtlChassisHeaderDetails" runat="server" Text="Dealer Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    --%>                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlChassisHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntChassisHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" class="ContainTable">
                            <tr>
                                <td style="width: 18%">
                                    <asp:CheckBoxList ID="chkDealer" runat="server" Width="619px"></asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

            </td>
        </tr>
        <tr>
            <td style="width: 14%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="None">
                    <div align="center"  style="height: 300px; overflow: auto; display: none;background-color: #D4D4D4; " id="dvbudplan" runat="server">
                        <asp:GridView ID="GrdPartsFMSCat" runat="server" CssClass="table table-condensed table-bordered"
                            AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False" BorderStyle="Double"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                            GridLines="Horizontal" SkinID="NormalGrid"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                            Width="100%">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                                <%--<asp:TemplateField HeaderText="" ItemStyle-Width="2%" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgSelect" runat="server"
                                            ImageUrl="~/Images/arrowRight.png"
                                            Style="height: 16px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Id" ItemStyle-Width="1%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDealerCode" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Dealer_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Region Name" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegionID" runat="server" CssClass="DispalyNon" Text='<%# Eval("RegionID") %>'> </asp:Label>
                                        <asp:Label ID="lblRegionName" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Region_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDealerID" runat="server" CssClass="DispalyNon" Text='<%# Eval("Dealer_ID") %>'> </asp:Label>
                                        <asp:Label ID="lblDealerName" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Dealer_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User ID" ItemStyle-Width="1%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" CssClass="DispalyNon" Text='<%# Eval("User_ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part ID" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="txtPart_ID" runat="server" Text='<%# Eval("Part_ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part No." ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="txtParts_No" runat="server" Text='<%# Eval("Parts_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="txtPart_Name" runat="server" Text='<%# Eval("Part_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Cat ID" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="txtFMSCat_ID" runat="server" Text='<%# Eval("FMS_Cat_ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stocking Norm" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:Label ID="txtCat_Name" runat="server" Text='<%# Eval("Category_Code","{0:#0.00}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="9%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="HoBrID" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoBrID" runat="server" Text='<%# Eval("HoBrID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </div>
                </asp:Panel>

                <%--</asp:Panel>--%>
            </td>
        </tr>
        <tr id="TmpControl">
            <td>
                <div style="display: none">
                    <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
