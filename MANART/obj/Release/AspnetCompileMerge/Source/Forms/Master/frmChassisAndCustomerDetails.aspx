<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmChassisAndCustomerDetails.aspx.cs" Inherits="MANART.Forms.Master.frmChassisAndCustomerDetails" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
        <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />


    <script type="text/javascript">
        $(document).ready(function () {

            var txtCustPODate = document.getElementById("ContentPlaceHolder1_txtinsdate_txtDocDate");
            $('#ContentPlaceHolder1_txtinsdate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: txtCustPODate.value, maxDate: 0

            });
        })
    </script>
    <%-- New Webmethod--%>
    <script type="text/javascript">
        function onTextChange(data) {
            PageMethods.GetInvoice(document.getElementById("<%=txtChassisNo.ClientID%>").value, OnSuccess);
        }
        function OnSuccess(response, userContext, methodName) {

            if (response != 0) {
                alert('This Chassis already Available!');


            }
        }
    </script>

    <script type="text/javascript">
        function avoidSplChars(e) {
            var s = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?";
            str = document.getElementById('<%=txtcouponno.ClientID%>').value;
            for (var i = 0; i < str.length; i++) {
                if (s.indexOf(str.charAt(i)) != -1) {
                    alert("The box has special characters. \nThese are not allowed.\n");
                    document.getElementById('<%=txtcouponno.ClientID%>').value = "";
                    return false;
                }

            }
        }
    </script>
     <script type="text/javascript">
         function CheckDealer(data) {
         
             PageMethods.GetDealer(document.getElementById("<%=txtDealercode.ClientID%>").value, OnSuccess1);
        }
        function OnSuccess1(response, userContext, methodName) {
             if (response == 0) {
                alert('This Dealer Not Available!');
                document.getElementById("<%=txtDealercode.ClientID%>").value = '';
                
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table id="PageTbl" style="width: 100%" class="table-responsive">
        <tr id="TitleOfPage">
            <td align="center" class="panel-heading">
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title"></asp:Label>
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

        <tr id="TblControl">
            <td>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                        bIsCallForServer="true" Visible="false" />
                </asp:Panel>

                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel3"
                        ExpandControlID="Panel2" CollapseControlID="Panel2" Collapsed="false"
                        ImageControlID="Image1" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Chassis Details" ExpandedText="Chassis Details"
                        TextLabelID="Label2">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel2" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label2" runat="server" Text="Vehicle Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" ScrollBars="None">
                        <table id="Table1" runat="server" class="ContainTable table table-bordered">

                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="lbl" runat="server" Text="PDI Save Or NOT"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:Label ID="lblmsg" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label3" runat="server" Text="Chassis No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtChassisNo" runat="server" CssClass="TextBoxForString" onchange="onTextChange(this)" OnTextChanged="txtChassisNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:Label ID="Label11" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label4" runat="server" Text="Engine No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                      <asp:Label ID="Label16" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>


                            </tr>
                            <tr>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label5" runat="server" Text="Model Code"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpmodelcode" runat="server" CssClass="TextBoxForString"></asp:DropDownList>
                                     <asp:Label ID="Label13" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>



                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label14" runat="server" Text="Ins Date"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <%--                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="TextBoxForString"></asp:TextBox>--%>
                                    <uc3:CurrentDate ID="txtinsdate" runat="server" Mandatory="false" bCheckforCurrentDate="false" Enabled="false" />
                                    <%--  <asp:Label ID="Label15" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>--%>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label12" runat="server" Text="Vechicle No"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtvechicleno" runat="server" CssClass="TextBoxForString" MaxLength="12"></asp:TextBox>

                                </td>
                                <td></td>
                                <td style="display:none">
                                    <asp:Button ID="btnupdate" runat="server" Text="Chassis Updation" OnClick="btnupdate_Click" />
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                        ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Customer Header Details"
                        ExpandedText="Customer Header Details" TextLabelID="lblTtlPartHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartHeaderDetails" runat="server">
                        <table>
                            <tr class="panel-heading">
                                <td align="center" class="panel-title">
                                    <asp:Label ID="lblTtlPartHeaderDetails" runat="server" Text="Part Header Details"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table2" runat="server" class="table table-bordered">
                            <tr style="display:none">
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:LinkButton ID="lblSelectCustomer" runat="server" ForeColor="#49A3D3" onmouseout="SetCancelStyleOnMouseOut(this);"
                                        onmouseover="SetCancelStyleonMouseOver(this);" Text="Select Customer" Width="80%"
                                        ToolTip="Select Customer Details" OnClick="lblSelectCustomer_Click"> </asp:LinkButton>
                                </td>
                                <td style="width: 18%"></td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel"></td>
                                <td style="width: 18%"></td>

                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">Dealer Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtDealercode" runat="server" CssClass="TextBoxForString" onchange="CheckDealer(this)"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label19" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true" ></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">Customer Name:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtcustname" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="Label6" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">Phone:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtphone" runat="server" CssClass="TextBoxForString" onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label7" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true" ></asp:Label>
                                    <%-- <asp:DropDownList ID="drpUnit" runat="server" CssClass="NonEditableFields"></asp:DropDownList>--%>
                                </td>

                            </tr>
                            <tr>

                                <td class="tdLabel" style="width: 15%">Address 1:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtaddress1" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label8" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">Address 2:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtaddress2" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">State:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlstate" runat="server" CssClass="TextBoxForString" AutoPostBack="true" OnSelectedIndexChanged="drpState_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:Label ID="Label9" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">Region:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="ddlRegion" runat="server" Width="100px" CssClass="TextBoxForString" EnableViewState="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label10" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                                <td style="width: 15%" class="tdLabel">Mobile No: 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtmobileno" runat="server" CssClass="TextBoxForString" onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                        Text=""></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">Pin Code:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtpincode" runat="server" CssClass="TextBoxForString" onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                        Text=""></asp:TextBox>
                                        <asp:Label ID="Label20" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true" ></asp:Label>
                                </td>



                                <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" BorderColor="Black" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="Panel6"
                        ExpandControlID="Panel5" CollapseControlID="Panel5" Collapsed="false"
                        ImageControlID="Image2" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Coupon  Details" ExpandedText="Coupon  Details"
                        TextLabelID="Label1">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel5" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label1" runat="server" Text="Vehicle Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel6" runat="server" ScrollBars="None">
                        <table id="Table3" runat="server" class="ContainTable table table-bordered">

                            <tr>
                                <td style="width: 15%; padding-left: 10px;" class="tdLabel">
                                    <asp:Label ID="Label15" runat="server" Text="Coupon Number"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtcouponno" runat="server" CssClass="TextBoxForString" MaxLength="10" onkeydown="return avoidSplChars(event,this);" />

                                </td>

                                <td style="width: 15%; padding-left: 10px;" class="tdLabel" id="txtcourate" runat="server" visible="false">
                                    <asp:Label ID="Label17" runat="server" Text="Coupon Rate"></asp:Label>
                                </td>
                                <td style="width: 18%" id="txtcoupon" runat="server" visible="false">
                                    <asp:TextBox ID="txtcouponrate" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="Label18" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>


                                </td>


                            </tr>



                        </table>
                        <div>
                            <asp:GridView ID="srvno" runat="server" AutoGenerateColumns="false" GridLines="Horizontal" SkinID="NormalGrid" DataKeyNames="ID"
                                CssClass="table table-condensed table-bordered" ShowHeader="true"
                                AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black">
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <HeaderStyle CssClass="GridViewHeaderStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>

                                    <asp:TemplateField HeaderText="ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="Service Name" DataField="Serv_Name" />
                                    <asp:TemplateField HeaderText="Select Service">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkselctsrv" runat="server" AutoPostBack="true" OnCheckedChanged="chkselctsrv_CheckedChanged" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status Details">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="checkboxcoupon" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Coupon Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtcouponrate" runat="server" Enabled="false" Text="0" onkeypress=" return CheckForTextBoxValue(event,this,'6');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:Button ID="Button2" runat="server" Text="Coupon Updation" Font-Bold="true" Style="margin-left: 1000px;" Visible="false" />
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>

    </table>
    <cc1:ModalPopupExtender ID="ModalPopUpExtender1" runat="server" TargetControlID="lblTragetID1" PopupControlID="pnlPopupWindow"
        OkControlID="btnOK1" BackgroundCssClass="modalBackground" BehaviorID="mpe1">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID1" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow" runat="server" CssClass="modalPopup" Style="display: none; width: 900px; height: 400px;">
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                    <asp:Label ID="Label27" runat="server" Text="">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl1">
                <td style="width: 14%">
                    <div align="center" class="ContainTable">
                        <table style="background-color: #efefef;" width="100%">

                            <tr align="center">
                                <td class="tdLabel" style="width: 7%;">Search:
                                </td>
                                <td class="tdLabel" style="width: 15%;">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString" Width="120px"></asp:TextBox>
                                </td>

                                <td class="tdLabel">
                                    <asp:DropDownList ID="DdlSelctionCriteria" runat="server" Width="120px"
                                        CssClass="ComboBoxFixedSize">
                                        <asp:ListItem Selected="True" Value="Customer_name">Customer Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 15%;">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                                        CssClass="btn btn-search btn-sm" OnClick="btnSearch_Click" />
                                </td>
                                <td style="float: right; margin-right: 5px;">
                                    <asp:Button ID="btnok" runat="server" Text="OK" CssClass="btn btn-search btn-sm" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel7" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                        <div class="rounded_corners">
                            <asp:GridView ID="CustomerGrid" runat="server" AlternatingRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal" DataKeyNames="ID"
                                HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                                AutoGenerateColumns="false"
                                OnPageIndexChanging="CustomerGrid_PageIndexChanging">
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <RowStyle CssClass="GridViewRowStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <HeaderStyle CssClass="GridViewHeaderStyle" />

                                <Columns>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <%--  <asp:Button ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  OnClientClick="return ReturnCRMChassisValue(this);"/> --%>
                                            <asp:ImageButton ID="ImgSelect" runat="server" ImageUrl="~/Images/arrowRight.png" OnClick="btnSelectCustomer_Click1" />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                      
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                               
                                    <asp:TemplateField HeaderText="Pincode" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpincode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("pincode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Phone" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhone" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Phone") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                              
                                </Columns>
                                <HeaderStyle Wrap="True" />
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>

                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
