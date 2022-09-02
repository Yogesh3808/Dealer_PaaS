<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmDynamicMaster.aspx.cs" Inherits="MANART.Forms.Master.frmDynamicMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <link href="../../Content/style.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script type="text/javascript">
        window.onload = function () {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            HideControl();
            setTimeout("disableBackButton()", 0);
            disableBackButton();
        }
        javascript: window.history.forward(1);

        setTimeout("disableBackButton()", 0);

        function disableBackButton() {
            window.history.forward();
        }
        function refresh() {
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }
        document.onkeydown = function () { refresh(); }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var sDate = '<%=sEffDate %>'
            if (sDate != '')
                $('#ContentPlaceHolder1_<%=sEffDate %>_txtDocDate').datepick({
                    dateFormat: 'dd/mm/yyyy', selectDefaultDate: true
                });
        });

        $(document).ready(function () {
            var EffFromDate = '<%=sEffFromDate %>'
            var EffToDate = '<%=sEffToDate %>'
            if (EffFromDate != '' && EffToDate != '') {

                var sEffFromDate = document.getElementById("ContentPlaceHolder1_<%=sEffFromDate %>_txtDocDate");
                var sEffToDate = document.getElementById("ContentPlaceHolder1_<%=sEffToDate %>_txtDocDate");
                $('#ContentPlaceHolder1_<%=sEffFromDate %>_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: sEffToDate.value
                });

                $('#ContentPlaceHolder1_<%=sEffToDate %>_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: sEffFromDate.value
                });

                function customRange(dates) {
                    if (this.id == 'ContentPlaceHolder1_<%=sEffFromDate %>_txtDocDate') {
                       $('#ContentPlaceHolder1_<%=sEffToDate %>_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                   }
                   else {
                       $('#ContentPlaceHolder1_<%=sEffFromDate %>_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                   }
               }
           }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
        <tr class="panel-heading">
            <td class="PageTitle panel-title" align="center">
                <asp:Label ID="lblTitle" runat="server" Text="">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc1:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>       
        <tr id="TblControl">
            <td>
             <asp:Panel ID="ControlDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                <table id="ControlContainer" runat="server" class="ContainTable">
                    <tr>
                        <td>
                            <cc1:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="drpTarget"
                                Category="M_State" ServiceMethod="GetDataForCombo" ServicePath="~/WebService/GetComboDataService.asmx"
                                PromptText="--Select--">
                            </cc1:CascadingDropDown>
                            <cc1:CascadingDropDown ID="CascadingDropDown2" runat="server" TargetControlID="drpTarget"
                                Category="M_State" ServiceMethod="GetDataForCombo" ServicePath="~/WebService/GetComboDataService.asmx"
                                PromptText="--Select--">
                            </cc1:CascadingDropDown>
                            <cc1:CascadingDropDown ID="CascadingDropDown3" runat="server" TargetControlID="drpTarget"
                                Category="M_State" ServiceMethod="GetDataForCombo" ServicePath="~/WebService/GetComboDataService.asmx"
                                PromptText="--Select--">
                            </cc1:CascadingDropDown>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
        </tr>      
        <tr id="TblSelectionGrid">
            <td>
                <uc2:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                    bIsCallForServer="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <%--Megha02052011--%>
                 <asp:TextBox ID="txtFormTypeID" runat="server"  CssClass="HideControl" Width="1px" Text=""></asp:TextBox>
               <%-- Megha02052011--%>
                 <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:DropDownList ID="drpParent" runat="server" Width="1px" CssClass="HideControl" >
                </asp:DropDownList>
                <asp:DropDownList ID="drpTarget" runat="server" Width="1px" OnSelectedIndexChanged="drpTarget_SelectedIndexChanged" CssClass="HideControl" >
                </asp:DropDownList>                
            </td>
        </tr>
    </table>
        <%--<table id="PageTbl" style="width: 100%" border="1">
          
            <tr>
                <td align="center" class="panel-heading" style="height: 15px">
                    <asp:Label ID="lblTitle" runat="server" CssClass="panel-title"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="ToolbarContainer" runat="server" border="1" class="">
                        <tr>
                            <td>
                                <uc1:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TblControl">
                <td>
                    <asp:Panel ID="ControlDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="ControlContainer" runat="server" class="table table-condensed">
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="TblSelectionGrid">
                <td>
                        <uc2:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: none">
                        <asp:TextBox ID="txtControlCount" runat="server" Width="1px"
                            Text=""></asp:TextBox>
                        <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtFormTypeID" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                        <asp:DropDownList ID="drpParent" runat="server" Width="1px">
                        </asp:DropDownList>
                        <asp:DropDownList ID="drpTarget" runat="server" Width="1px" OnSelectedIndexChanged="drpTarget_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
        </table>--%>
    </div>
</asp:Content>
