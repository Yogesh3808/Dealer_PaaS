<%@ Page Title="" Language="C#"
    AutoEventWireup="true" CodeBehind="frmChassisSelection.aspx.cs" Inherits="MANART.Forms.Common.frmChassisSelection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc2" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chassis Details</title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jquery-1.3.2.js"></script>
    <script src="../../Scripts/jquery.min.js"></script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                event.returnValue = false;
                alert(message);
                return false;
            }
        }
        window.onload = function () {
        }

        //Function For return Chassis Details
        function ReturnChassisValue(objImgControl) {
            debugger;
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            var sValue;
            //check ins date is blank or not for jobcard.
            sValue = objRow[39].innerText.trim();
            if (sValue == "Y") {
                alert("INS Date is not available for this chassis so Warranty work cannot be done. If warranty work is involved, don't save the jobcard and contact Helpdesk.");                
            }            
            var ArrOfChassis = new Array();
            for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
                sValue = objRow[cnt].innerText.trim();
                ArrOfChassis.push(sValue);
            }
            //alert(ArrOfChassis);
            window.returnValue = ArrOfChassis;
            window.close();           
        }
    </script>

    <base target="_self" />
</head>

<%--</asp:Content>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
<body runat="server">
    <form id="form1" runat="server">
        <div class="table-responsive">
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </cc1:ToolkitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="" border="1">
                        <tr id="TitleOfPage" class="panel-heading">
                            <td class="panel-title" align="center">
                                <asp:Label ID="lblTitle" runat="server" Text=""> </asp:Label>
                            </td>
                        </tr>
                        <tr id="TblControl">
                            <td style="width: 14%">
                                <div align="center" class="">
                                    <table style="background-color: #efefef;" width="50%">
                                        <tr align="center">
                                            <td class="tdLabel" style="width: 7%;">Search:
                                            </td>
                                            <td class="tdLabel" style="width: 15%;">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                            </td>

                                            <td class="tdLabel" style="width: 15%;">
                                                <asp:DropDownList ID="DdlSelctionCriteria" runat="server"
                                                    CssClass="ComboBoxFixedSize">
                                                    <asp:ListItem Selected="True" Value="Chassis_no">Chassis No</asp:ListItem>
                                                    <asp:ListItem Value="Reg_no">Vehicle Reg No</asp:ListItem>
                                                    <asp:ListItem Value="Customer_name">Customer Name</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search"
                                                    CssClass="btn btn-search" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="PPartDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                                    class="ContaintTableHeader" ScrollBars="Vertical">
                                    <asp:GridView ID="ChassisGrid" runat="server" AlternatingRowStyle-Wrap="true" CssClass="table table-bordered"
                                        EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                                        HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                                        AutoGenerateColumns="false"
                                        OnPageIndexChanging="ChassisGrid_PageIndexChanging">
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" onClick="return ReturnChassisValue(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                                HeaderStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chassis No." ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChassisNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_no") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Vehicle_No") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle In No" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehInNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("VehInNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle In DateTime" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehInDate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("VehInTime") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEngineNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Engine_no") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Editing" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustEdit" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CustEdit") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CustID" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CustID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_code") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Model Name" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aggregate" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAggregate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Aggregate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WarrantyTag" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWarrantyTag" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("WarrantyTag") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMC_Chk" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMC_Chk" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_Chk") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMC Type" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMC_Type" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Model Group ID" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModelGrID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_Gr_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Veh In ID" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehInID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("VehInID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chassis Last Kms" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastKm" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstKm") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="chassis Last Hrs" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastHrs" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstHrs") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMC Start Kms" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCStKm" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_Start_KM") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMC End Kms" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCEndKm" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_End_KM") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Kms Before Spd Mtr Change" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBfr_Last_SpdMtrChange_Kms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Bfr_Last_SpdMtrChange_Kms") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hrs Before Hrs Mtr Change" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBfr_Last_OdoMtrChange_Kms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Bfr_Last_HrsMtrChange_Hrs") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="KAM" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblKAM" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("In_KAM") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Upgrade Campaign" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpgrdCamp" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UpgrdCamp") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AMC End Date" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAMCEndDt" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("AMC_End_Date") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Under Observe" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUndObserv" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UndObserv") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Under Observe Eff From" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUndObservEffFrom" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UndObservEffFrom") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Under Observe Eff To" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUndObservEffTo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("UndObservEffTo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Float Part" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFloatPart" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Float_flag") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Theft Vehicle" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehTheft" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Theft_flag") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Jb Kms" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLstJbKms" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstJbKm") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Jb Hrs" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLstJbHrs" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LstJbHrs") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Jb Hrs" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMod_Bas_Cat" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Mod_Cat_ID_Basic") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CRM HDR ID" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCRMHDRID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CRM_HDR_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Call Ticket No" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTicketNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Ticket_No") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Call Ticket Date" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTicketDate" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Ticket_Date") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="INS Msg" HeaderStyle-CssClass="HideControl" ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblINSMsg" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("INSMsg") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle Wrap="True" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
<%--</asp:Content>--%>
