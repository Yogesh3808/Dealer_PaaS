<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="frmPartsTarget.aspx.cs" Inherits="MANART.Forms.Spares.frmPartsTarget" %>

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
        function ShowMonthTarget(objNewPartLabel, sDealerID, iModelId, iYearId, sDealerName, sModelName, sYear, iMenuId) {
            if (iYearId == "0") {
                alert("Please Select Year");
                return false;
            }
            else {
                window.showModalDialog("/DCS/Forms/Master/frmVehicleTargetMonth.aspx?DealerID=" + sDealerID + "&ModelId=" + iModelId + "&YearId=" + iYearId + "&DealerName=" + sDealerName + "&ModelName=" + sModelName + "&Year=" + sYear + "&MenuId=" + iMenuId, "List", "dialogHeight: 500px; dialogWidth:900px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            }
        }
        function ShowMonthTarget1(objNewPartLabel, sDealerID, iYearId, iAnnualL1Amt, sDealerName, sDealerCode, iYear) {
            //sDealer_ID + "," + lblModel_cat_Id.Text + "," + drpYear.SelectedValue + ",'" + lblTonnage_ID.Text 
            if (iYearId == "0") {
                alert("Please Select Year");
                return false;
            }
            else {
                // window.showModalDialog("/DCS/Forms/Master/frmSparesTargetMonth.aspx?DealerID=" + sDealerID + "&ModelId=" + iModelId + "&YearId=" + iYearId + "&iTonnageID=" + iTonnageID + "&CType=" + CType, "List", "dialogHeight: 500px; dialogWidth:900px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
                window.showModalDialog("/DCS/Forms/Master/frmSparesTargetMonth.aspx?DealerID=" + sDealerID + "&YearId=" + iYearId + "&AnnualL1Amt=" + iAnnualL1Amt + "&DealerName=" + sDealerName + "&DealerCode=" + sDealerCode + "&Year=" + iYear, "List", "dialogHeight: 500px; dialogWidth:500px; dialogTop: 150px; dialogLeft: 500px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            }
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
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search('Parts_AnnualTarget_') == -1) {
                alert('File name is not in given format.');
                return false;
            }

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="Parts Target Plan"> </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
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
            <td style="width: 14%">
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double" ScrollBars="None">
                        <asp:Panel ID="TtlDocDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlDocDetails" runat="server" Text="Target Details"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDocDetails" runat="server" Height="15px"
                                            ImageUrl="~/Images/Minus.png" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable">
                            <tr>
                                <td style="width: 10%; padding-left: 10px" class="tdLabel">Year:</td>
                                <td style="width: 13%">
                                    <asp:DropDownList ID="drpYear" runat="server" CssClass="ComboBoxFixedSize"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%;" class="tdLabel">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-search" OnClick="btnSave_Click" Visible="False" />
                                    <asp:Button ID="btnShow" Text="Show" runat="server" CssClass="btn btn-search" OnClick="BtnShow_Click" />
                                </td>
                                <%--<td style="width: 18%">                                    
                                <asp:Label ID="lblVehSpr" runat="server" CssClass="DispalyNon">
                                        </asp:Label>
                                </td>--%>
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
                                    <a>Please select the file in the excel format. File name should be in format as 'Parts_AnnualTarget_Datestamp'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;Parts_AnnualTarget_14102016&#39;. </a>
                                        <br />
                                        2.
                                   <a>Quarter Targets Values in INR (Lac)</a>
                                      <%--  <br />
                                        3.
                                    <a>Quarter 1 Target Percent is 25%, Quarter 2 Target Percent is 27%, Quarter 3 Target Percent is 23%, Quarter 4 Target Percent is 25% </a>--%>
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
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PChassisHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Style="display: none">
                    <cc1:CollapsiblePanelExtender ID="CPEChassisHeaderDetails" runat="server" TargetControlID="CntChassisHeaderDetails"
                        ExpandControlID="TtlChassisHeaderDetails" CollapseControlID="TtlChassisHeaderDetails"
                        Collapsed="true" ImageControlID="ImgTtlChassisHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Dealer Details"
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
                                    <asp:Image ID="ImgTtlChassisHeaderDetails" runat="server" ImageUrl="~/Images/Minus.png"
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
                <div align="center" class="containtable" style="height: 300px; overflow: auto; display: none" id="dvbudplan" runat="server">
                    <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black"  BorderStyle="Double">
                        <asp:GridView ID="GrdAnnualTarget" runat="server"  CssClass="table table-bordered"
                            AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False" 
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                            GridLines="Horizontal" SkinID="NormalGrid" 
                            Width="100%">
                            <%--OnRowCommand="GrdAnnualTarget_RowCommand"--%>
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
                                <asp:TemplateField HeaderText="Target Id" ItemStyle-Width="1%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTargetID" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Id") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDealerCode" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Dealer_Code") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Region Name" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegionID" runat="server" CssClass="DispalyNon" Text='<%# Eval("RegionID") %>'> </asp:Label>
                                        <asp:Label ID="lblRegionName" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Region_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dealer Name" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDealerID" runat="server" CssClass="DispalyNon" Text='<%# Eval("DealerID") %>'> </asp:Label>
                                        <asp:Label ID="lblDealerName" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Dealer_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />
                                </asp:TemplateField>
                                <%--  <asp:TemplateField HeaderText="Model Category" ItemStyle-Width="3%"  >
                                <ItemTemplate>
                                    <asp:Label ID="lblModel_cat_Id" runat="server" CssClass="DispalyNon" Text='<%# Eval("Model_cat_Id") %>'> </asp:Label>
                                    <asp:Label ID="lblModel_cat_Name" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("Model_cat_Name") %>'> </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>--%>
                                <%--  <asp:TemplateField HeaderText="Tonnage " ItemStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTonnage_ID" runat="server" CssClass="DispalyNon" Text='<%# Eval("Tonnage_ID") %>'> </asp:Label>
                                    <asp:Label ID="lblTonnage_Name" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("Tonnage_Name") %>'> </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Yearly Target" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblAnnualBugPlan" CssClass="GridTextBoxForAmount" runat="server" Enabled="false"
                                            Text='<%# Eval("YTD_Target","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="T1_Q1" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblT1_Q1BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("T1_Q1","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Q1 L1" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ1L1BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q1L1","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q1 L2" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ1L2BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q1L2","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q1 L3" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ1L3BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q1L3","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="T1_Q2" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblT1_Q2BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("T1_Q2","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q2 L1" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ2L1BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q2L1","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q2 L2" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ2L2BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q2L2","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q2 L3" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ2L3BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q2L3","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="T1_Q3" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblT1_Q3BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("T1_Q3","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q3 L1" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ3L1BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q3L1","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q3 L2" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ3L2BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q3L2","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q3 L3" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ3L3BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q3L3","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="T1_Q4" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblT1_Q4BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("T1_Q4","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q4 L1" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ4L1BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q4L1","{0:#0.00}") %>' Width="96%" MaxLength="12"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q4 L2" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ4L2BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q4L2","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q4 L3" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblQ4L3BugPlan" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Q4L3","{0:#0.00}") %>' Width="96%" MaxLength="4"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="" ItemStyle-Width="6%" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelectPart" runat="server">Set Monthly Target</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>--%>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
