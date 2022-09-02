<%@ Page Title="M0 (General Discussion)" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" 
    EnableEventValidation="false" CodeBehind="frmAllocation.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmAllocation" %>

<%--<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>--%>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/MANPendingDoc.ascx" TagName="PendingDocument" TagPrefix="UCPDoc" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <%--<script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>--%>
    <%--<link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>--%>
     
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsLCDetailsFunctions.js"></script>
    <script src="../../Scripts/jsVehicleINAndInsttaltionCer.js"></script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
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
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }
        document.onkeydown = function () {
            refresh();
        }

        function CheckcustType(eve, objcontrol) {
            if (objcontrol.selectedIndex == 3) {
                alert("Only One customer of Cash Sale Type Allowed");
                objcontrol.value = "0";
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var DetailsGrid = document.getElementById("<%=DetailsGrid.ClientID%>");
            if (DetailsGrid != null) {
                var Elements = DetailsGrid.getElementsByTagName("input");
            }
            objDate = new Date();

            if (Elements != null) {
                var splDate = '';
                var dt = '';
                var Today = new Date(objDate.getFullYear(), objDate.getMonth + 1, objDate.getDay)
                for (var i = 0; i < Elements.length; i++) {


                    if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtNextObjDate") != -1) {
                        splDate = Elements[i].value.split("/")
                        dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
                        $('#' + Elements[i].id).datepick({
                            //                    dateFormat: 'dd/mm/yyyy', minDate: (Today > dt) ? '0d' : Elements[i].value
                            dateFormat: 'dd/mm/yyyy', minDate: '0d'
                        });


                    }
                }
            }

        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            var DetailsGrid = document.getElementById("<%=DetailsGrid.ClientID%>");
            if (DetailsGrid != null) {
                var Elements = DetailsGrid.getElementsByTagName("input");
            }
            objDate = new Date();

            if (Elements != null) {
                var splDate = '';
                var dt = '';
                var Today = new Date(objDate.getFullYear(), objDate.getMonth + 1, objDate.getDay)
                for (var i = 0; i < Elements.length; i++) {

                    if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtObjDate") != -1) {
                        splDate = Elements[i].value.split("/")
                        dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
                        $('#' + Elements[i].id).datepick({
                            //                    dateFormat: 'dd/mm/yyyy', minDate: (Today > dt) ? '0d' : Elements[i].value
                            dateFormat: 'dd/mm/yyyy', minDate: '0d'
                        });


                    }
                }
            }

        });
    </script>
    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {
                var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");

                $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick({
                    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                });

                //var txtNextDate = document.getElementById("ContentPlaceHolder1_txtNextDate_txtDocDate");

                //$('#ContentPlaceHolder1_txtNextDate_txtDocDate').datepick({
                //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                //});




                function customRange(dates) {
                    if (this.id == 'ContentPlaceHolder1_txtDocDate_txtDocDate') {
                        $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                    }
                    else {
                        $('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                    }
                }
            });
        }

        function OnCustNameValueChange(ObjCombo, txtboxId) {
            if (OnComboCustValueChange(ObjCombo, txtboxId) == false) {

            }
        }

        function OnComboCustValueChange(ObjCombo, txtboxId) {
            var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].text;
            var ParentCtrlID;
            var Sup, Post;
            var objtxtControl;
            if (sSelecedValue == "NEW") {
                ObjCombo.style.display = 'none';
                objtxtControl = document.getElementById("ContentPlaceHolder1_txtNewCust");
                objtxtControl.style.display = '';
                objtxtControl.focus();
            }
            else {
                ObjCombo.style.display = '';

                objtxtControl = document.getElementById("ContentPlaceHolder1_txtNewCust");
                objtxtControl.style.display = 'none';
            }
            return true;
        }
    </script>
    <style type="text/css">
        .auto-style3 {
            width: 15%;
            height: 35px;
        }
        .auto-style4 {
            width: 18%;
            height: 35px;
        }
        .auto-style5 {
            width: 15%;
            height: 33px;
        }
        .auto-style6 {
            width: 18%;
            height: 33px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td align="center" style="height: 15px" class="panel-title">
                    <asp:Label ID="lblTitle" runat="server" Text="M0 (General Discussion)" > </asp:Label>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 15%">
                    <table id="ToolbarContainer" runat="server" width="100%" border="1">
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
                    <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>
                </td>
                      
                 <asp:Panel ID="PnlPendingDocument" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                   <UCPDoc:PendingDocument ID="PDoc" runat="server" />
                   <%--<uc4:SearchGridView ID="PDoc" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />--%>
                </asp:Panel>

            </tr>
            <tr id="TblControl">
                <td style="width: 15%; height: 92px;">
                      <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </asp:Panel>
                    <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <%-- <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
                             <ContentTemplate>--%>
                        <table id="Table1" runat="server" style="width:100%"   class ="ContainTable" border="1">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">M0 (General Discussion) Details
                                </td>
                            </tr>
                            <tr>

                                  <td>
                                    <asp:Label ID="lblLeadNo" runat="server" Text="M0 No:" CssClass=""></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLeadNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblLeadDate" runat="server" Text="M0 Date:" CssClass=""></asp:Label>
                                </td>
                                <td>
                                    <uc3:CurrentDate ID="txtDocDate" runat="server" bCheckforCurrentDate="false" />
                      <%--              <asp:Label ID="Label2" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                                </td>

                                 <td style="width: 15%" class="">Type:
                                </td>
                                <td style="width: 15%">
                                    <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                      onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         AutoPostBack="true"  >
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="drpCustType" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpCustType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label8" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                               
                                <td style="width: 15%" class="">Title:
                                </td>
                                <td style="width: 15%">

                                    <asp:DropDownList ID="drpTitle" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label6" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                
                                 <td style="width: 15%" class="">Name:
                                </td>
                                <td style="width: 15%">

                                   <%-- <asp:DropDownList ID="drpCustName" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>--%>
                                    
                                    <asp:DropDownList ID="drpCustName" runat="server" CssClass="GridComboBoxFixedSize" AutoPostBack="true"
                                     Width="90%" OnSelectedIndexChanged="drpCustName_SelectedIndexChanged" EnableViewState="true" >
                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewCust"  runat="server"   CssClass="TextBoxForString"
                                     Width="90%" ></asp:TextBox>
                                    <asp:Label ID="Label11" Text="*" runat="server" CssClass="Mandatory"></asp:Label>

                                    </td>
                                
                                  <td>
                                <asp:Label ID="Label7" runat="server" Text="Existing MTI Customer:" CssClass="tdLabel"></asp:Label>
                            </td>

                                <td>
                                <asp:DropDownList ID="drpIsMTICust" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                               <asp:Label ID="Label12" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                                <%--<td style="width: 15%" class="">Name:
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                </td>--%>
                            </tr>
                            <tr>
                                <%--<td style="width: 15%" class="">First Name:
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="TextBoxForString" OnTextChanged="txtFirstName_TextChanged"
                                        AutoPostBack="true"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">Last Name:
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="TextBoxForString" OnTextChanged="txtLastName_TextChanged"
                                        AutoPostBack="true"
                                        Text=""></asp:TextBox>
                                </td>--%>

                               

                            </tr>
                            <tr>
                                <td style="width: 15%" class="">Address 1 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label1" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">Address 2 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="">Pincode:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtpincode" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label13" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">State:
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpState" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label4" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">District:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpDistrict" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                  <asp:Label ID="Label10" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td style="width: 15%" class="">Region:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpRegion" runat="server"
                                        CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label3" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="">City:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label5" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td class="" style="width: 15%">Country:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString"
                                        Text="India" ReadOnly="true"></asp:TextBox>
                                </td>
                                 <td style="width: 15%" class="">Mobile:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    <asp:Label ID="Label14" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                               
                                <td class="" style="width: 15%">Email:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                               <td class="auto-style5">Primary Application:
                                </td>                              

                                <td class="auto-style6">
                                <asp:DropDownList ID="drpPrimaryApplication" runat="server"
                                CssClass="ComboBoxFixedSize" AutoPostBack="True" OnSelectedIndexChanged="drpPrimaryApplication_SelectedIndexChanged">
                                </asp:DropDownList>
                                    <asp:Label ID="Label15" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                            
                                <td class="auto-style5">Secondary Application:
                                </td>
                                <td class="auto-style6">
                                <asp:DropDownList ID="drpSeconadryApplication" runat="server"
                                CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>
                                    <asp:Label ID="Label16" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                

                                <td class="tdLabel">
                                Financier of the Customer:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpM0Financier" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                               
                            </td>

                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Body Builder Details:" CssClass=""></asp:Label>
                                </td>

                                <td style="width: 15%">
                                    <asp:TextBox ID="txtBodyBuilder" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                                <td class="" style="width: 15%"></td>
                                    <td>
                                        <asp:Button ID="bConfirm" runat="server" CssClass="ComboBoxFixedSize" OnClick="bConvertToM1" Text="Convert to M1" Visible="false" />
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="CRM Ticket No:" CssClass=""></asp:Label>
                                </td>

                                <td style="width: 15%">
                                    <asp:TextBox ID="txtTcktNo" ReadOnly="true" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>

                                   <td>
                                    <asp:Label ID="Label17" runat="server" Text="CRM Ticket Date:" CssClass=""></asp:Label>
                                </td>

                                <td style="width: 15%">
                                    <asp:TextBox ID="txtTcktDate" ReadOnly="true" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>

                            </tr>
                            <%--<tr>
                               
                                <%--<td class="" style="width: 15%">Visit Objective:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpVisitObj" runat="server"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                </td>

                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Next Visit Date:" CssClass=""></asp:Label>
                                </td>
                                <td>
                                    <uc3:CurrentDate ID="txtNextDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                </td>--%>

                                 
                          
                        </table>
                        
                              <%--   </ContentTemplate>                             
                             </asp:UpdatePanel>  --%>
                    </asp:Panel>

                     <asp:UpdatePanel UpdateMode="Conditional" ID="UpFleet" runat="server" ChildrenAsTriggers="true">
                         <ContentTemplate>
                    <asp:Panel ID="PFleetDet" runat="server" BorderColor="Black" BorderStyle="Double"
                    class="">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntFleetDetails"
                        ExpandControlID="TtlFleetDetails" CollapseControlID="TtlFleetDetails" Collapsed="false"
                        ImageControlID="ImgTtlFleetDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Fleet Details" ExpandedText="Fleet Details"
                        TextLabelID="lblTtlFleetDetails">
                    </cc1:CollapsiblePanelExtender>
                    

                         <asp:Panel ID="TtlFleetDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlFleetDetails" runat="server" Text="Fleet Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlFleetDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="123%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    <asp:Panel ID="CntFleetDetails" runat="server" BorderColor="Black" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="FleetDtls" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" > 
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFleetID" runat="server" Text='<%# Eval("ID") %>' Width="3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Competitor No" ItemStyle-Width="15%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompNo" runat="server"                                             
                                           
                                            CssClass="LabelCenterAlign" Text='<%# Eval("CompID")  %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>      
                                    
                                                                 
                                    <asp:TemplateField HeaderText="Manufacturer" ItemStyle-Width="17%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Competitor_Name") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="17%" />
                                    </asp:TemplateField>      
                                    
                                           
                                 <%--<asp:TemplateField HeaderText="Bus" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHDTQty" runat="server" EnableViewState="false" CssClass="TextForAmount" 
                                                
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');"> </asp:TextBox>
                                            <%--<asp:Label ID="lblHDTQty" runat="server" CssClass="LabelCenterAlign" EnableViewState ="false" > </asp:Label>--%>
                                            <%--Text='<%# Eval("HDTQty","{0:#0}") %>'--%>
                                       <%-- </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpFleetModel1" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty1" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>     
                                     <asp:TemplateField HeaderText="Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpFleetModel2" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>  

                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty2" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>   

                                 <asp:TemplateField HeaderText="Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpFleetModel3" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>  

                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty3" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>   
                                
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                             </ContentTemplate>
                         </asp:UpdatePanel>
                    
                     <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpFleet">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <%--<asp:UpdatePanel UpdateMode="Conditional" ID="UPObj" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>--%>
                    <asp:Panel ID="PObjectiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEObjectiveDetails" runat="server" TargetControlID="CntObjectiveDetails"
                        ExpandControlID="TtlObjectiveDetails" CollapseControlID="TtlObjectiveDetails"
                        Collapsed="false" ImageControlID="ImgTtlObjectiveDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Visit Details"
                        ExpandedText="Visit Details" TextLabelID="lblTtlObjectiveDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlObjectiveDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title">
                                    <asp:Label ID="lblTtlObjectiveDetails" runat="server" Text="Visit Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlObjectiveDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntObjectiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                            <Columns>
                                <%--<asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtObjID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Visit Details" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpVisitObj" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <uc3:CurrentDate ID="txtObjDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discussion/Result" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscussion" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Time Spent" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTimeSpent" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Visit Details" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpNextObj" runat="server" CssClass="GridComboBoxFixedSize">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <uc3:CurrentDate ID="txtNextObjDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Commitment To Customer" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCommitment" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New/Cancel" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" />
                                        <asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValueLead(event,this);"
                                            runat="server">New</asp:LinkButton>
                                        <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" 
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
<%--</ContentTemplate>
         </asp:UpdatePanel>                                
             <asp:UpdateProgress ID="UPRObject" runat="server" AssociatedUpdatePanelID="UPObj">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>  --%>   
                </td>
            </tr>
            <tr>
                <td style="width: 15%"></td>
            </tr>
            <tr id="TmpControl" style="display: none">
                <td style="width: 15%">
                    <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtID"  CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtCustID" CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtCrmCustID" CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtTcktID" CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" CssClass="DispalyNon" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
                    <input id="hsiv" type="hidden" name="hsiv" runat="server" />
                    <input id="__ET1" type="hidden" name="__ET1" runat="server" />
                    <input id="__EA1" type="hidden" name="__EA1" runat="server" />
                    <input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
