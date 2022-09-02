<%@ Page Title="MTI-Dealer Utility" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Theme="SkinFile" EnableViewState="true" EnableEventValidation="false" CodeBehind="frmSchedulers.aspx.cs" Inherits="MANART.Forms.Admin.frmSchedulers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/DateStyle.css" rel="stylesheet" />

    <script src="../../Scripts/JSFPDAWarrantyClaim.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jquery-jtemplates.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtCrDate = document.getElementById("ContentPlaceHolder1_txtCrDate_txtDocDate");
            if (txtCrDate != null)
                $('#ContentPlaceHolder1_txtCrDate_txtDocDate').datepick({
                    dateFormat: 'dd/mm/yyyy'
                });
        });
    </script>
     <script>
        function SetCurrentAndPastDate(obj, Msg) {

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
            if (dtCurDate < sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="menu">
<asp:Menu ID="Menu1" Visible="false" CssClass="NavigationMenu" Orientation="Horizontal" runat="server"
            Height="36px" StaticEnableDefaultPopOutImage="False" StaticTopSeparatorImageUrl="~/Images/menu_sprtr.gif"
            OnMenuItemClick="Menu1_MenuItemClick1" BackColor="#919191">
            <StaticMenuItemStyle CssClass="staticMenuItemStyle" ForeColor="Blue" />
            <StaticHoverStyle CssClass="staticHoverStyle" ForeColor="White" />
            <StaticSelectedStyle CssClass="staticMenuItemSelectedStyle" ForeColor="White" />
            <DynamicMenuItemStyle CssClass="dynamicMenuItemStyle" ForeColor="White" />
            <DynamicHoverStyle CssClass="menuItemMouseOver" ForeColor="White" />
            <DynamicMenuStyle CssClass="menuItem" ForeColor="White" />
            <DynamicSelectedStyle CssClass="menuItemSelected" ForeColor="White" />
            <Items>
                <%--<asp:MenuItem ImageUrl="" Text="Patch Management" Value="0" Selected="True"></asp:MenuItem>
                <asp:MenuItem ImageUrl="" Text="Dealer Live Entry" Value="1"></asp:MenuItem>--%>
                <asp:MenuItem ImageUrl="" Text="SQL Jobs Status" Value="0">
                </asp:MenuItem>
                <asp:MenuItem></asp:MenuItem>
            </Items>
        </asp:Menu>

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">


            <asp:View ID="Tab3" runat="server">
                <table class="PageTable" border="1" runat="server" >
                 
                    
                    <tr>
                    <td>
                     <table class="PageTable" border="1" id="tblJobstatus" >
                         <tr>
                             <td>
                                 <asp:Panel ID="Panel3" runat="server">
                                     <table width="100%">
                                         <tr>
                                             <td align="center" class="ContaintTableHeader" width="96%">
                                                 <asp:Label ID="Label3" runat="server" Height="16px" 
                                                     onmouseout="SetCancelStyleOnMouseOut(this);" 
                                                     onmouseover="SetCancelStyleonMouseOver(this);" 
                                                     Text="SQL Job Status-Set Enable/Disable" Font-Bold="true" Width="96%"></asp:Label>
                                             </td>
                                             <td width="1%">
                                                 <asp:Image ID="Image2" runat="server" Height="15px" 
                                                     ImageUrl="~/Images/Plus.png" Width="100%" />
                                             </td>
                                         </tr>
                                     </table>
                                 </asp:Panel>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:Button ID="btnEnableAll" runat="server" Text="Enable All" CssClass="btn btn-search btn-sm" 
                   Width="100px" OnClick="btnEnableAll_Click" />
                                 <asp:Button ID="btnDisableAll" runat="server" Text="Disable All" CssClass="btn btn-search btn-sm" 
                   Width="100px" OnClick="btnDisableAll_Click" />
                             </td>
                           
                         </tr>
                         <tr>
                        <asp:GridView ID="GridView2"  runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover">
                       
                            <Columns>
                                <asp:TemplateField HeaderText="Job Name" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblname" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("job_name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Enabled Status" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEnabled" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("job_status") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Run Status" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastrunstatus" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("last_run_status") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Run Date(MM/dd/YYYY:hh:mm:ss)" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastrundate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("last_run_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Sch Run Date(MM/dd/YYYY:hh:mm:ss)" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnextscheduledrundate" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("next_scheduled_run_date") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Step Description" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstep_description" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("step_description") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Enable" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSQLJobenable" runat="server" CssClass="CommandButton" Text="Enable"
                                            OnClick="btnbtnSQLJOBEnable" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Disable" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSQLJobdisable" runat="server" CssClass="CommandButton" Text="Disable"
                                            OnClick="btnbtnSQLJOBDisable" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </tr>
                     </table>&nbsp;
                     </td>
                    </tr>
                    
                </table>               
            </asp:View>


 </asp:MultiView>



    </div>
    
</asp:Content>