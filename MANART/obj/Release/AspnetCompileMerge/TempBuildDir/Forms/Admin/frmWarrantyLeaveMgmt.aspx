<%@ Page Title="MTI-Leave Management" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmWarrantyLeaveMgmt.aspx.cs" Inherits="MANART.Forms.Admin.frmWarrantyLeaveMgmt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>

    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    
    
<script type ="text/javascript" >


    $(document).ready(function () {
        var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
        var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
        $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
            onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value, minDate: (txtFromDate.value == '') ? '0d' : txtFromDate.value
        });

        $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
            onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtFromDate.value
        });

        function customRange(dates) {
            if (this.id == 'ContentPlaceHolder1_txtFromDate_txtDocDate') {
                $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
            }
            else {
                $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
            }
        }
    });

     </script>


    <script type="text/javascript">

        function Validation() {
            var CurrentUser = document.getElementById("<%=drpCurrentUser.ClientID %>")
            var CurrentUserVal = CurrentUser.options[CurrentUser.selectedIndex].value;
            var NewUser = document.getElementById("<%=drpAssignUser.ClientID %>")
            var NewUserVal = NewUser.options[NewUser.selectedIndex].value;
            var FromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate")
            var ToDate = document.getElementById("ContentPlaceHolder1_ToDate_txtDocDate")
            var Remark = document.getElementById("<%=txtRemark.ClientID %>")

            if (CurrentUserVal == 0) {
                alert("Please select Current User");
                CurrentUser.focus();
                return false;
            }
            else if (NewUserVal == 0) {
                alert("Please select Assign to User");
                NewUser.focus();
                return false;
            }
            else if (FromDate.value == "") {
                alert("Please select From Date");
                FromDate.focus();
                return false;
            }
            else if (ToDate.value == "") {
                alert("Please select To Date");
                ToDate.focus();
                return false;
            }
            else if (Remark.value == "") {
                alert("Please enter Remark");
                Remark.focus();
                return false;
            }

        }

        function CheckDateLess(obj1, obj2, obj3) {
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
        function SetFutureDate(obj, Msg) {

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
            if (dtCurDate >= sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="Leave Management"> </asp:Label>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 15%">
                <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:Location ID="Location" runat="server" />
                </asp:Panel>
                <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="txtDocNoDetails" runat="server" class="ContainTable">
                        <tr>
                            <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="6">
                                Header Leave Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%">
                                <asp:Label ID="lblCurrentUser" runat="server" Text="Current User" Font-Bold="true"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:DropDownList ID="drpCurrentUser" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                            </td>
                            <td class="tdLabel" style="width: 15%">
                                <asp:Label ID="lblAssignUser" runat="server" Text="Assign to User" Font-Bold="true"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <asp:DropDownList ID="drpAssignUser" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <asp:Label ID="lblMandatory1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                            </td>
                            <td class="tdLabel" style="width: 15%">
                                &nbsp;
                            </td>
                            <td style="width: 18%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%">
                                <asp:Label ID="lblFromDate" Font-Bold="true" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <uc3:CurrentDate ID="txtFromDate" runat="server" bCheckDateGreaterThanOrEqualToCurrentDate="false"
                                    bCheckforCurrentDate="false" />
                            </td>
                            <td class="tdLabel" style="width: 15%">
                                <asp:Label ID="lblToDate" Font-Bold="true" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                <uc3:CurrentDate ID="txtToDate" runat="server" bCheckDateGreaterThanOrEqualToCurrentDate="false"
                                    bCheckforCurrentDate="false" />
                            </td>
                            <td class="tdLabel" style="width: 15%">
                                &nbsp;
                            </td>
                            <td style="width: 18%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="width: 15%;">
                                Remark:
                            </td>
                            <td style="width: 18%" colspan="2">
                                <asp:TextBox ID="txtRemark" Text="" runat="server" CssClass="MultilineTextbox" ReadOnly="true"
                                    Width="90%"></asp:TextBox>
                                     <asp:Label ID="lblMandatory2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                            </td>
                            <td style="width: 18%">
                                &nbsp;
                            </td>
                            <td class="tdLabel" style="width: 15%">
                                &nbsp;
                            </td>
                            <td style="width: 18%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="tdLabel" style="width: 15%" colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="CommandButton" 
                                    OnClientClick="return Validation()" onclick="btnSave_Click" />
                                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="CommandButton"
                                    OnClientClick="return Validation()" onclick="btnConfirm_Click" />
                                <asp:Button ID="btnNew" runat="server" Text="New" CssClass="CommandButton" 
                                    onclick="btnNew_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                
                
                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEPartDetails" runat="server" TargetControlID="CntPartDetails"
                        ExpandControlID="TtlPartDetails" CollapseControlID="TtlPartDetails" Collapsed="true"
                        ImageControlID="ImgTtlPartDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Leave Details" ExpandedText="Hide Leave Details"
                        TextLabelID="lblTtlPartDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartDetails" runat="server">
                        <table width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlPartDetails" runat="server" Text="Leave Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                  <%--  <asp:Panel ID="CntPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="gvLeaveGrid" runat="server" AllowPaging="True" AlternatingRowStyle-Wrap="true"
                            AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                            GridLines="Horizontal" SkinID="NormalGrid" Width="96%" 
                            onselectedindexchanged="gvLeaveGrid_SelectedIndexChanged">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="3%" HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="25%" HeaderText="Current User">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrentUser" runat="server" Text='<%# Eval("CurrentUser") %>' CssClass="LabelLeftAlign" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="25%" HeaderText="Assign to User">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewUser" runat="server" Text='<%# Eval("NewUser") %>' CssClass="LabelLeftAlign" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="center" ItemStyle-CssClass="LabelCenterAlign "
                                    HeaderStyle-Width="10%" HeaderText="From Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("FromDate","{0:dd/MM/yyyy}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="To Date" ItemStyle-CssClass="LabelRightAlign ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("ToDate","{0:dd/MM/yyyy}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="27%" HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemark" runat="server" CssClass="GridTextBoxForAmount" Text='<%# Eval("Remark") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>--%>
                     <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" />
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
                <asp:TextBox ID="txtDummyLC" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>
                <asp:HiddenField ID="hdnLCApplicable" runat="server" Value="N"                    />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
