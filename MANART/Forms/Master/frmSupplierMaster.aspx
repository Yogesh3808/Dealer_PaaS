<%@ Page Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Title="MTI-Supplier Master" Theme="SkinFile" EnableViewState="true" CodeBehind="frmSupplierMaster.aspx.cs" Inherits="MANART.Forms.Master.frmSupplierMaster" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

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
                        Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Supplier Header Details"
                        ExpandedText="Supplier Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                        <table width="100%">
                            <%-- Green Color Code- #91b900,  --%>
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Supplier Header Details"
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
                                <td style="width:15%; padding-left:10px">Type :</td>
                                <td>
                                    <asp:DropDownList ID="drpSupType" runat="server"
                                        CssClass="ComboBoxFixedSize" AutoPostBack="True" OnSelectedIndexChanged="drpSupType_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                    <asp:Label ID="Label2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">Name:
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtSupplierName" runat="server" CssClass="TextBoxForString" MaxLength="50"
                                        Text=""></asp:TextBox>
                                     <asp:DropDownList ID="drpSupplierName" runat="server"  AutoPostBack="true" 
                                                CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpSupplierName_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 15%" class="tdLabel">Address 1 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"  MaxLength="200"
                                        Text=""></asp:TextBox>
                                    <asp:Label ID="Label3" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                <%--<td style="width: 15%" class="tdLabel">Address 2 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>--%>
                            </tr>
                            <tr>
                                <td style="width: 15%;padding-left:10px;" class="tdLabel">Address 2 :
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString" Width="96%" MaxLength="200"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">City:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">State:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpState" runat="server"
                                        CssClass="ComboBoxFixedSize" AutoPostBack="True" OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label4" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">Region:
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpRegion" runat="server"
                                        CssClass="ComboBoxFixedSize"
                                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdLabel" style="width: 15%;padding-left:10px">Country:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text="India"></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Mobile:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength="11" Text=""></asp:TextBox>
                                    <asp:Label ID="Label5" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                </td>
                              
                            </tr>
                            <tr>
                                  <td class="tdLabel" style="width: 15%">Phone:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="TextBoxForString" Width="96%" Text=""></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%;padding-left:10px">Email:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text=""></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">PAN No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPANNo" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text=""></asp:TextBox>
                                </td>
                               
                            </tr>
                            <tr>
                                 <td style="width: 15%" class="tdLabel">TIN No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtTINNo" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text=""></asp:TextBox>
                                </td>
                             
                                <td style="width: 15%;padding-left:10px; display:none;" class="tdLabel">C.S.T.:
                                </td>
                                <td style="width: 18%; display:none;">
                                    <asp:TextBox ID="txtcst" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%; display:none;" class="tdLabel">S.T./VAT:
                                </td>
                                <td style="width: 18%; display:none;">
                                    <asp:TextBox ID="txtST" runat="server" CssClass="TextBoxForString" Width="96%"
                                        Text=""></asp:TextBox>
                                </td>
                               </tr>
                            <tr>
                            <td style="width: 15%" class="tdLabel">GSTIN:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtGSTIn" runat="server" CssClass="TextBoxForString" Width="96%" MaxLength="20"
                                        Text=""></asp:TextBox>
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
                                <td style="width: 15%" class="tdLabel"><%--Ledger Name:--%>
                                </td>
                                <td style="width: 18%">                                            
                                        <asp:TextBox ID="txtLedgerName" runat="server" CssClass="TextBoxForString"   MaxLength="100" Visible="false"
                                        Text=""></asp:TextBox>
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
    </table>
</asp:Content>
