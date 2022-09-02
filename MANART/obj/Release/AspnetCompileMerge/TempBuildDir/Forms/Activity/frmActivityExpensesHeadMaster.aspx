
<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmActivityExpensesHeadMaster.aspx.cs" Inherits="MANART.Forms.Activity.frmActivityExpensesHeadMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Src="../../WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>


<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>

    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>


    <script type="text/javascript">
        window.onload
        {
            AtPageLoad();

        }
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            HideControl();
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
        function SetCurrAndFutureDate(obj, Msg) {

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
        function CheckAcDateGreter(obj1, obj2, obj3) {

            var splDate = obj1.value.split("/")
            var splDate1 = obj2.value.split("/")
            var dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
            var dt1 = new Date(splDate1[2], splDate1[1] - 1, splDate1[0]);

            if (dt < dt1) {
                alert(obj3)
                d = dt1.getDate() + 1
                dt1.setDate(d);
                obj1.value = dt1.format("dd/MM/yyyy");
                obj1.focus();
                return false
            }
        }

    </script>

    <script language="JavaScript">
        var message = "Right-mouse click is not allowed";
        function click(e) {
            if (document.all) {
                if (event.button == 2 || event.button == 3) {
                    alert(message);
                    return false;
                }
            }
            else {
                if (e.button == 2 || e.button == 3) {
                    e.preventDefault();
                    e.stopPropagation();
                    alert(message);
                    return false;
                }
            }
        }
        if (document.all) // for IE
        {
            document.onmousedown = click;
        }
        else // for other browsers
        {
            document.onclick = click;
        }
    </script>

    <script type="text/javascript">

        if (typeof window.event != 'undefined')
            document.onkeydown = function () {
                if (event.srcElement.tagName.toUpperCase() != 'INPUT')
                    return (event.keyCode != 8);
            }
        else
            document.onkeypress = function (e) {
                if (e.target.nodeName.toUpperCase() != 'INPUT')
                    return (e.keyCode != 8);
            }

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td align="center" class="panel-title">
                <asp:Label ID="lblTitle" runat="server" Text="Activity Expenses Head Master"></asp:Label>
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
        <tr id="TblControl1">
            <td>
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" width="100%" runat="server">
                        <tr>
                            <td>
                                <uc2:Location ID="Location" runat="server" OnDDLSelectedIndexChanged="Location_DDLSelectedIndexChanged" Visible="false"  />
                            </td>
                        </tr>

                    </table>
                    <tr id="TblControl">
                        <td style="width: 15%; height: 92px;">
                            <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                class="ContaintTableHeader">
                                <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                                    ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                                    Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                                    CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Activity Expenses Head Header Details"
                                    ExpandedText="Activity Expenses Head Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                                </cc1:CollapsiblePanelExtender>
                                <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                                    <table width="100%">
                                        <%-- Green Color Code- #91b900,  --%>
                                        <tr class="panel-heading">
                                            <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                                <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Activity Expenses Head Header Details"
                                                    Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                            </td>
                                            <td width="1%" class="panel-title">
                                                <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                                    Height="15px" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                                    <table id="Table1" runat="server" class="table table-bordered">
                                        <tr>
                                           
                                            <td style="width: 15%; padding-left: 10px;" class="tdLabel">Expenses Head:
                                            </td>
                                            <td style="width: 15%">
                                              
                                                <asp:TextBox ID="txtExpensesHead" runat="server" CssClass="TextBoxForString" MaxLength ="50"
                                                    Text=""></asp:TextBox> 
                                                <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td style="width: 15%" class="tdLabel">Active:
                                            </td>
                                            <td style="width: 15%">
                                                <asp:DropDownList ID="drpActive" runat="server" CssClass="ComboBoxFixedSize"
                                                    EnableViewState="true" Width="100px">
                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                                    <asp:ListItem Value="2">N</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            
                                        </tr>
                                       
                                      
                                   
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                                <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                                    bIsCallForServer="true" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%"></td>
                    </tr>
                    <tr id="TmpControl">
                        <td style="width: 15%">
                               <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtDealerCode" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                                Text=""></asp:TextBox>
                            <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtRecordUsed" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                            <input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
                            <input id="hsiv" type="hidden" name="hsiv" runat="server" />
                            <input id="__ET1" type="hidden" name="__ET1" runat="server" />
                            <input id="__EA1" type="hidden" name="__EA1" runat="server" />
                            <input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />
                        </td>
                    </tr>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
