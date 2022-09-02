
<%@ Page Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Title="MTI-Local Part Master" Theme="SkinFile" EnableViewState="true" CodeBehind="frmLocalPartMaster.aspx.cs" Inherits="MANART.Forms.Master.frmLocalPartMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
       <script src="../../Scripts/jsMessageFunction.js"></script>
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
            //debugger;
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }

        function refresh() {
            if (event.keyCode == 116) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }

        document.onkeydown = function () {
            refresh();
        }
    </script>
    <style>
        .tdLabel {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: Black;
            /*font-weight: bold;*/
            padding-left: 25px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" style="width: 100%"  class="table-responsive">
        <tr id="TitleOfPage">
            <td align="center" class="panel-heading">
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title"></asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td>
                <table id="ToolbarContainer" runat="server" border="1"  width="100%"  >
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" wi />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="TblControl">
            <td>
                <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                        ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Part Header Details"
                        ExpandedText="Part Header Details" TextLabelID="lblTtlPartHeaderDetails">
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
                            <tr>
                                <td style="width: 15%" class="tdLabel">Part No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel"> MTI Part Name:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMTIPartName" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Base Unit:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtBaseUnit" runat="server" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                   <%-- <asp:DropDownList ID="drpUnit" runat="server" CssClass="NonEditableFields"></asp:DropDownList>--%>
                                </td>
                               
                            </tr>
                            <tr>
                                
                                <td class="tdLabel" style="width: 15%"> Part Group:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtGroup" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Min Order Qty:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMinOrderQty" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel HideControl">Part Category:
                                </td>
                                <td style="width: 18%" class="HideControl">
                                    <asp:TextBox ID="txtPartCategory" runat="server" CssClass="TextBoxForString"
                                        Text="" MaxLength="1"></asp:TextBox>
                                </td>
                               
                            </tr>
                            <tr> 
                                
                                  
                                <td style="width: 15%" class="tdLabel">Active
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpActive" runat="server" Width="100px" CssClass="TextBoxForString" EnableViewState="true">
                                        <asp:ListItem Value="1" Selected="True">Y</asp:ListItem>
                                        <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                 <td style="width: 15%" class="tdLabel"> 
                                      <asp:Label id="lblLocalPartName"  runat="server"  Text ="Local Part Name:" ></asp:Label> 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLocalPartName" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                     <asp:Label id="lblLocation"  runat="server"  Text ="Location:" ></asp:Label> 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>
                               

                                <td style="width: 15%" class="tdLabel"></td>
                                <td style="width: 18%"></td>
                            </tr>
                        </table>
                        <%-- <div class="row">
                                <div class="form-group">
                                    <div class="col-md-3 col-sm-3">
                                        <asp:Label ID="lblPartNo" runat="server" Text="Part No:"></asp:Label>
                                        <asp:TextBox ID="txtPartNo" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5 col-sm-5">
                                        <asp:Label ID="lblPartName" runat="server" Text="Part Name:"></asp:Label>
                                        <asp:TextBox ID="txtPartName" runat="server" Width="260px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 col-sm-4">
                                        <asp:Label ID="lblBaseUnit" runat="server" Text="Base Unit: "></asp:Label>
                                        <asp:DropDownList ID="drpUnit" runat="server" Width="170px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-md-3 col-sm-3">
                                        <asp:Label ID="Label1" runat="server" Text="Group:"></asp:Label>
                                        <asp:TextBox ID="txtGroup" runat="server" ReadOnly="true" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5 col-sm-5">
                                        <asp:Label ID="Label2" runat="server" Text="Min Order Qty: "></asp:Label>
                                        <asp:TextBox ID="txtMinOrderQty" runat="server" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4 col-sm-4">
                                        <asp:Label ID="Label3" runat="server" Text="Part Category:"></asp:Label>
                                        <asp:TextBox ID="txtPartCategory" runat="server" Text="" MaxLength="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" Text="Active:"></asp:Label>
                                        <asp:DropDownList ID="drpActive" runat="server" Width="170px" EnableViewState="true">
                                            <asp:ListItem Value="1" Selected>Y</asp:ListItem>
                                            <asp:ListItem Value="2">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-8 col-sm-8">
                                </div>
                            </div>--%>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <div class="">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr id="TmpControl">
            <td>
                <div style="display: none">
                    <asp:TextBox ID="txtControlCount" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtLastDateNegotiation" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:DropDownList ID="drpValidityDays" runat="server">
                    </asp:DropDownList>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
