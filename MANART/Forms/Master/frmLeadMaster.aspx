<%@ Page Title="Lead Master" Theme="SkinFile" EnableViewState="true" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmLeadMaster.aspx.cs" Inherits="MANART.Forms.Master.frmLeadMaster" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/cssDatePicker.css" rel="stylesheet" type="text/css">
    <link href="../../CSS/GridStyle.css" rel="stylesheet" type="text/css" />

    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsProformaFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>
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
            FirstTimeGridDisplay('ctl00_ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }


        //        function refresh() {
        //            if (event.keyCode == 116 || event.keyCode == 8) {
        //                event.keyCode = 0;
        //                event.returnValue = false
        //                return false;
        //            }
        //        }

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
                // objcontrol.focus();
                objcontrol.value = "0";
                return false;
            }

            else {
                return true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 15%">
                <asp:Label ID="lblTitle" runat="server" Text="">
                </asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td style="width: 15%; height: 92px;">
                <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                        ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                        Collapsed="False" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show EGP Customer Header Details"
                        ExpandedText="Hide EGP Customer Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="EGP Customer Header Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" class="ContainTable">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Type:
                                </td>
                                <td style="width: 15%">
                                    <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                      onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                         AutoPostBack="true"  >
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="drpCustType" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpCustType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                                <td style="width: 15%" class="tdLabel">Organisational Subcategory:
                                </td>
                                <td style="width: 15%">
                                    <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                      onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                         AutoPostBack="true"  >
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="drporgType" runat="server"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label8" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                                <td style="width: 15%" class="tdLabel">Customer Subcategory:
                                </td>
                                <td style="width: 15%">
                                    <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                      onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                         AutoPostBack="true"  >
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="drpcustSubType" runat="server"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label9" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">Name:
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                                <td style="width: 15%" class="tdLabel">Address 1 :
                                </td>
                                <td style="width: 15%">

                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td style="width: 15%" class="tdLabel">Address 2 :
                                </td>
                                <td style="width: 15%">

                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td style="width: 15%" class="tdLabel">Address 3 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress3" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>


                                </td>

                                <td style="width: 15%" class="tdLabel">City:
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label5" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>
                                <td style="width: 15%" class="tdLabel">Pincode:
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtpincode" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>

                            </tr>
                            <tr>

                                <td style="width: 15%" class="tdLabel">State:
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpState" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label4" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>

                                <td style="width: 15%" class="tdLabel">District:
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpDistrict" runat="server" AutoPostBack="True"
                                        CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label10" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>

                                <td style="width: 15%" class="tdLabel">Region:
                                </td>
                                <td style="width: 18%">

                                    <asp:DropDownList ID="drpRegion" runat="server"
                                        CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label3" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                </td>

                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 15%">Country:
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString"
                                        Text="India" ReadOnly="true"></asp:TextBox>

                                </td>
                                <td style="width: 15%" class="tdLabel">Mobile:
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>

                                </td>

                                <td style="width: 15%" class="tdLabel">Phone:
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    <asp:Label ID="Label7" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>

                            </tr>
                            <tr>

                                <td class="tdLabel" style="width: 15%">Email:
                                </td>
                                <td style="width: 18%">

                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>

                                </td>
                                <td style="width: 15%" class="tdLabel">Contact Person :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>

                                <td class="tdLabel" style="width: 15%">Active:
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="drpActive" runat="server" Width="100px" CssClass="ComboBoxFixedSize" EnableViewState="true"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1" Selected>Y</asp:ListItem>
                                        <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label6" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
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
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                    Text=""></asp:TextBox>
                <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                </asp:DropDownList>
                <input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
                <input id="hsiv" type="hidden" name="hsiv" runat="server" />
                <input id="__ET1" type="hidden" name="__ET1" runat="server" />
                <input id="__EA1" type="hidden" name="__EA1" runat="server" />
                <input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />

            </td>
        </tr>
    </table>
</asp:Content>
