<%@ Page Title="MTI-Site Maintainance" Language="C#" MasterPageFile="~/Header.Master"
    Theme="SkinFile" EnableViewState="true" AutoEventWireup="true" CodeBehind="frmSiteMaintainance.aspx.cs" Inherits="MANART.Forms.Master.frmSiteMaintainance" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="../../Content/bootstrap.css" rel="stylesheet" />--%>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>



    <script type="text/javascript">
        $(document).ready(function () {
            //debugger;
            //var txtSelDate = document.getElementById("ContentPlaceHolder1_txtSelDate_txtDocDate");
            //$('#ContentPlaceHolder1_txtSelDate_txtDocDate').datepick({
            //    dateFormat: 'dd/mm/yyyy', minDate: (txtSelDate.value == '') ? '0d' : txtSelDate.value
            //});
            // New COde Vikram K Dated 12022018
            var txtSelDate = document.getElementById("ContentPlaceHolder1_txtSelDate_txtDocDate");
            var txtDMsgTime = document.getElementById("ContentPlaceHolder1_txtDMsgTime_txtDocDate");
            $('#ContentPlaceHolder1_txtSelDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtSelDate.value == '') ? '0d' : txtSelDate.value, autoSize: true
                //dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value
            });

            $('#ContentPlaceHolder1_txtDMsgTime_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtSelDate.value
            });

            function customRange(dates) {
                //debugger;
                if (this.id == 'ContentPlaceHolder1_txtSelDate_txtDocDate') {
                    $('#ContentPlaceHolder1_txtDMsgTime_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                }
                else {
                    $('#ContentPlaceHolder1_txtSelDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                }
            }

        });
    </script>

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
            FirstTimeGridDisplay('ContentPlaceHolder1_');

            setTimeout("disableBackButton()", 0);
            disableBackButton();
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

        function SetCurrentFutureDate(obj, Msg) {

            var objDateValue = "";
            var ObjDate = obj;
            var x = new Date();
            var y = x.getYear();
            var m = x.getMonth() + 1; // added +1 because javascript counts month from 0
            var d = x.getDate();
            var dtCur = d + '/' + m + '/' + y;
            var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)

            objDateValue = ObjDate.value;
            var sTmpValue = objDateValue;
            var day = dGetValue(sTmpValue.split("/")[0]);
            var month = dGetValue(sTmpValue.split("/")[1]) - 1;
            var year = dGetValue(sTmpValue.split("/")[2]);
            var sTmpDate = new Date(year, month, day);
            var TmpDay = 0;

            if (objDateValue == '') {
                return false;
            }
            if (dtCurDate > sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        function disableBackButton() {
            window.history.forward(1);
        }
    </script>

    <script type="text/javascript">
        function Validation() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <%-- <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="">
                </asp:Label>
            </td>
        </tr>--%>
        <tr id="TblControl">
            <td style="width: 15%">
                <asp:Panel ID="PChassisHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="">
                    <cc1:collapsiblepanelextender id="CPEChassisHeaderDetails" runat="server" targetcontrolid="CntChassisHeaderDetails"
                        expandcontrolid="TtlChassisHeaderDetails" collapsecontrolid="TtlChassisHeaderDetails"
                        collapsed="false" imagecontrolid="ImgTtlChassisHeaderDetails" expandedimage="~/Images/Minus.png"
                        collapsedimage="~/Images/Plus.png" suppresspostback="true" collapsedtext="Maintenance Details"
                        expandedtext="Maintenance Details" textlabelid="lblTtlChassisHeaderDetails">
                    </cc1:collapsiblepanelextender>
                    <asp:Panel ID="TtlChassisHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlChassisHeaderDetails" runat="server" Text="Maintenance Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlChassisHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntChassisHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" class="ContainTable table  table-condensed table-bordered" width="100%">
                            <tr>
                                <td style="width: 10%" class="tdLabel">Select Date
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <uc3:currentdate id="txtSelDate" runat="server" visible="true" mandatory="true"
                                        bcheckforcurrentdate="false" />
                                </td>
                                <td style="width: 10%;"></td>
                                <td style="width: 15%;"></td>
                                <td style="width: 18%;"></td>
                                <td style="width: 7%;"></td>
                                <td style="width: 35%;"></td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="tdLabel">Start Time:
                                </td>
                                <td style="width: 15%">
                                    <%--<asp:TextBox ID="txtStartTime" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>--%>
                                    <asp:DropDownList ID="drpStartHours" runat="server" CssClass="ComboBoxFixedSize "
                                        Width="50px">
                                        <asp:ListItem Selected="True" Value="-1">Hours</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; :<asp:DropDownList ID="drpStartMinutes" runat="server" CssClass="ComboBoxFixedSize "
                                        Width="60px">
                                        <asp:ListItem Selected="True" Value="-1">Minutes</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:DropDownList ID ="drpStart" runat ="server" 
                                        CssClass ="ComboBoxFixedSize " Width="45px" >
                                        <asp:ListItem Selected ="True" Value ="AM">AM</asp:ListItem>
                                         <asp:ListItem Value ="PM">PM</asp:ListItem>
                                        </asp:DropDownList>--%>
                                    <%--(Format: 24 Hours)--%>
                                </td>
                                <td style="width: 10%" class="tdLabel">End Time:
                                </td>
                                <td style="width: 15%">
                                    <%-- <asp:TextBox ID="txtEndTime" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>--%>
                                    <asp:DropDownList ID="drpEndHours" runat="server" CssClass="ComboBoxFixedSize " Width="51px">
                                        <asp:ListItem Selected="True" Value="-1">Hours</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; :<asp:DropDownList ID="drpEndMinutes" runat="server" CssClass="ComboBoxFixedSize "
                                        Width="62px">
                                        <asp:ListItem Selected="True" Value="-1">Minutes</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--(Format: 24 Hours)--%>
                                    <%-- <asp:DropDownList ID ="drpEnd" runat ="server" CssClass ="ComboBoxFixedSize " Width="45px">
                                        <asp:ListItem Selected ="True" Value ="AM">AM</asp:ListItem>
                                         <asp:ListItem Value ="PM">PM</asp:ListItem>
                                        </asp:DropDownList>--%>
                                </td>
                                <td style="width: 18%" class="tdLabel">Display Message Start Time:
                                </td>
                                <td style="width: 7%">
                                    <%--<asp:TextBox ID="txtDMsgTime" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>--%>
                                    <uc3:currentdate id="txtDMsgTime" runat="server" visible="true" mandatory="false"
                                        bcheckforcurrentdate="false" />
                                    <%--(Format: 24 Hours)--%>
                                    <%--<asp:DropDownList ID ="drpDMsg" runat ="server" 
                                        CssClass ="ComboBoxFixedSize " Width="45px" >
                                        <asp:ListItem Selected ="True" Value ="AM">AM</asp:ListItem>
                                         <asp:ListItem Value ="PM">PM</asp:ListItem>
                                        </asp:DropDownList>--%>
                                </td>
                                <td  style="width: 35%">
                                    <asp:DropDownList ID="drpDMsgHours" runat="server" CssClass="ComboBoxFixedSize "
                                        Width="50px">
                                        <asp:ListItem Selected="True" Value="-1">Hours</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; :<asp:DropDownList ID="drpDMsgMinutes" runat="server" CssClass="ComboBoxFixedSize "
                                        Width="60px">
                                        <asp:ListItem Selected="True" Value="-1">Minutes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 10%">Reason:
                                </td>
                                <td colspan="3" style="width: 40%;">
                                    <asp:TextBox ID="txtRegion" runat="server" CssClass="TextBoxForString" Text="" Width="603px"></asp:TextBox>
                                </td>
                                <td style="width: 18%;"></td>
                                <td style="width: 7%;"></td>
                                <td style="width: 35%;"></td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="tdLabel"></td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Button ID="bSave" runat="server" Text="Save" CssClass="btn btn-search" OnClick="bSave_Click"
                                        OnClientClick="return Validation()" />
                                    <asp:Button ID="btnNEW" runat="server" Text="New" CssClass="btn btn-search"
                                        OnClick="btnNEW_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="btn btn-search" OnClick="btnCancel_Click" />
                                </td>
                                <td style="width: 10%;"></td>
                                <td style="width: 15%;"></td>
                                <td style="width: 18%;"></td>
                                <td style="width: 7%;"></td>
                                <td style="width: 35%;"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="">
                    <cc1:collapsiblepanelextender id="CPEChassisFSDetails" runat="server" targetcontrolid="CntChassisFSDetails"
                        expandcontrolid="TtlChassisFSDetails" collapsecontrolid="TtlChassisFSDetails"
                        collapsed="true" imagecontrolid="ImgTtlChassisFSDetails" expandedimage="~/Images/Minus.png"
                        collapsedimage="~/Images/Plus.png" suppresspostback="true" collapsedtext="Service Details"
                        expandedtext="Service Details" textlabelid="lblTtlChassisFSDetails">
                    </cc1:collapsiblepanelextender>
                    <asp:Panel ID="TtlChassisFSDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlChassisFSDetails" runat="server" Text="Service Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlChassisFSDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntChassisFSDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <div class="scrolling-table-container" style="height: 200px; background-color: #D4D4D4;">
                            <asp:GridView ID="gvFSDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                CssClass="table table-condensed table-bordered"
                                Width="100%" AllowPaging="false" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true" OnRowCommand="gvFSDetails_RowCommand">
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png"
                                                CommandName="ImgSelect" CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Code" ItemStyle-Width="10%" HeaderStyle-CssClass="DispalyNon "
                                        ItemStyle-CssClass="DispalyNon ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Time" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartTime" runat="server" Text='<%# Eval("StartTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Time" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndTime" runat="server" Text='<%# Eval("EndTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Display Massage Time" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDisplayMsgTime" runat="server" Text='<%# Eval("DisplayMsgTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason" ControlStyle-Width="50%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegion" runat="server" Text='<%# Eval("Region") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
        
        <tr id="TmpControl">
            <td style="width: 15%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                    Text=""></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
