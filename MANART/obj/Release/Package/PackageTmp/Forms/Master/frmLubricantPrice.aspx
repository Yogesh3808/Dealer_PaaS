<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmLubricantPrice.aspx.cs" Inherits="MANART.Forms.Master.frmLubricantPrice" %>

<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/cssDatePicker.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/GridStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsProformaFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>

        <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <link href="../../Content/PopUpModal.css" rel="stylesheet" />
    <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
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
    <script type="text/javascript">
        $(document).ready(function () {
            var txtUpdateDate = document.getElementById("ctl00_ContentPlaceHolder1_txtUpdateDate_txtDocDate");
            $('#ctl00_ContentPlaceHolder1_txtUpdateDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', maxDate: '0d'
            });
        });
    </script>
    <script type="text/javascript">
        // Function To Check FileName is Selected or Not
        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var filename = objFileUpload.value;
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('LubricantPartRate') == -1) {
                alert('File name is not in given format.');
                return false;
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table id="PageTbl" class="PageTable" border="1">

        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 15%; display: none">
                <asp:Label ID="Label1" runat="server" Text="Lubricant Part Rate"> </asp:Label>
            </td>
        </tr>

        <%--<tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
        <tr id="TblControl">
            <td style="width: 15%">
               
                <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" Visible="false" >
                    <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                        ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Upload File"
                        ExpandedText="Hide Upload File" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server" >
                        <table width="100%">
                            <tr>
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Upload File" Width="100%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double" >
                        <table id="Table3" runat="server" class="table table-responsive table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Select File:
                                </td>
                                <%--  OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');" --%>
                                <td style="width: 18%">
                                    <asp:FileUpload ID="txtFilePath" runat="server" />
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-search"
                                        OnClick="btnUpload_Click" /><br />
                                    <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double" >
                    <table id="Table2" runat="server" class="table table-bordered" border="1">
                        <tr>



                            <td>

                                <asp:DropDownList ID="drpSearch" runat="server" CssClass="ComboBoxFixedSize" Width="150px">
                                    <%--onchange="OnStateChange()--%>
                                    <asp:ListItem Selected="True" Value="1">Lubricant Code</asp:ListItem>
                                    <%--<asp:ListItem Selected="True" Value="1">State</asp:ListItem>
                                    <asp:ListItem Value="2">Lubricant Code</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Search: "></asp:Label>
                            </td>
                            <td style="width: 1%; height: 23px;">


                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString" Width="150px"
                                    Text=""></asp:TextBox>
                            </td>

                            <td class="tdLabel" style="width: 10%; height: 23px;">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="CommandButton" OnClick="btnSearch_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search" CssClass="CommandButton"
                                    Visible="false" OnClick="btnClearSearch_Click" />
                                <asp:Label ID="lblMessage1" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>

                            </td>
                            <td></td>

                        </tr>
                    </table>
                </asp:Panel>
                   

                <asp:Panel ID="PChassisAMCDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEChassisAMCDetails" runat="server" TargetControlID="CntChassisAMCDetails"
                        ExpandControlID="TtlChassisAMCDetails" CollapseControlID="TtlChassisAMCDetails" Collapsed="false"
                        ImageControlID="ImgTtlChassisAMCDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Show Lubricant Part Rate Details" ExpandedText="Hide Lubricant Part Rate Details">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlChassisAMCDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader" width="96%">
                                    <asp:Label ID="lblTtlChassisAMCDetails" runat="server" Text="LUBRICANT PART RATE DETAILS" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);" ForeColor="White" Font-Bold="true"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlChassisAMCDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntChassisAMCDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="Horizontal">
                        <%--<asp:GridView ID="gvAMCDetails" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                Width="100%" AllowPaging="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                                AutoGenerateColumns="False" HeaderStyle-Wrap="true" >--%>
                        <%--Megha23062011--%>
                        <asp:GridView ID="gvLubPartRateDetails" runat="server" AllowPaging="True" Width="100%" CssClass="datatable table table-bordered" DataKeyNames="ID"
                            AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="False" CellPadding="0"
                            CellSpacing="0" GridLines="Horizontal"
                            HeaderStyle-CssClass="GridViewHeaderStyle"
                            OnPageIndexChanging="gvLubPartRateDetails_PageIndexChanging"
                            RowStyle-CssClass="even" Style="border-collapse: separate;">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <%--Megha23062011--%>
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnedit" runat="server" ImageUrl="~/Images/arrowRight.png" OnClick="btnedit_Click"  />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State" ItemStyle-Width="1%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDealerCode" runat="server" Text='<%# Eval("State") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lubricant Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLubricantCode" runat="server" Text='<%# Eval("LubricantCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lubricant Name" ItemStyle-Width="35%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLubricantName" runat="server" Text='<%# Eval("Lubricant Name") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblunit" runat="server" Text='<%# Eval("Unit") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eff From Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffFromDate" runat="server"
                                            Text='<%# Eval("Eff_From_Date", "{0:d}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eff To Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffToDate" runat="server"
                                            Text='<%# Eval("Eff_To_Date", "{0:d}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="HSN Code" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHSNcode" runat="server"
                                            Text='<%# Eval("HSN_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="GST %" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGst" runat="server"
                                            Text='<%# Eval("GSTTaxPer","{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="DLP" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                    <%--    <asp:Label ID="lblLISTPrice" runat="server"
                                            Text='<%# Eval("LIST_Price", "{0:#0.00}") %>'></asp:Label>--%>
                                          <asp:Label ID="lblNDP" runat="server"
                                            Text='<%# Eval("NDP", "{0:#0.00}") %>'></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="List Price" ItemStyle-Width="10%" >
                                    <ItemTemplate>
                                      <%--  <asp:Label ID="lblNDP" runat="server"
                                            Text='<%# Eval("NDP", "{0:#0.00}") %>'></asp:Label>--%>

                                            <asp:Label ID="lblLISTPrice" runat="server"
                                            Text='<%# Eval("LIST_Price", "{0:#0.00}") %>'></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="MRP" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMRP" runat="server"
                                            Text='<%# Eval("MRP", "{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Pack Size" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPackSize" runat="server"
                                            Text='<%# Eval("PackSize", "{0:#0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Update Date" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCSUpdateDate" runat="server"
                                            Text='<%# Eval("DCS_Update_Date", "{0:d}") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="HideControl" />
                                    <ItemStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lubricant Price History">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Btnshowhistory" runat="server" ImageUrl="~/Images/History.png" OnClick="Btnshowhistory_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                   
                            </Columns>
                        </asp:GridView>


                    </asp:Panel>

                    <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                            ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                            Collapsed="true" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Lubricant Part Header Details" ExpandedText="Lubricant Part Header Details" TextLabelID="lblTtlPartHeaderDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlPartHeaderDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlPartHeaderDetails" runat="server" Text="Part Header Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlPartHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntPartHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="Table1" runat="server" width="100%">
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Lubricant Code:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtLubricantCode" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Lubricant Name:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtLubName" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Unit:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtUnit" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>

                                     <td style="width: 15%" class="tdLabel">Effective From:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Effective To:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtEffectiveTo" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true" Text="" Width="180px"></asp:TextBox>
                                    </td>
                                      <td style="width: 15%" class="tdLabel">HSN Code:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtHsncode" runat="server" CssClass="TextBoxForString"
                                            ReadOnly="true" Text="" Width="180px"></asp:TextBox>
                                    </td>

                                      
                                    
                                    <%-- <td style="width: 15%" class="tdLabel">NDP:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtNDP" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>--%>
                                    <%--<td style="width: 15%" class="tdLabel">Update Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtDCSUpdateDate" runat="server" CssClass="TextBoxForString" ReadOnly="true" Width="180px"
                                            Text=""></asp:TextBox>
                                    </td>--%>
                                </tr>

                                <tr>
                                      <td style="width: 15%" class="tdLabel">GST%:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPer" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                    <td style="width: 15%" class="tdLabel">
                                          <asp:Label ID="lblDLP" runat="server" Text="DLP:"></asp:Label>
                                        
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtNDP" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">
                                        <asp:Label ID="lblListPrice" runat="server" Text="List Price:"></asp:Label>
                                    </td>
                                   <td style="width: 18%">
                                        <asp:TextBox ID="txtLISTPRICE" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                         </td>
                                   

                                     
                                </tr>
                               <tr>
                                    
                                    <td style="width: 15%" class="tdLabel">MRP:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtMRP" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Pack Size:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtPackSize" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                            Text="" Width="180px"></asp:TextBox>
                                    </td>

                                   
                                    <td style="width: 15%" class="tdLabel">Update Date:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtDCSUpdateDate" runat="server" CssClass="TextBoxForString" ReadOnly="true" Width="180px"
                                            Text=""></asp:TextBox>
                                    </td>
                               </tr>
                              <%--  <tr>
                                       <td style="width: 15%" class="tdLabel">GST Percentage:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPer" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="" Width="180px"></asp:TextBox>
                                </td>
                                </tr>--%>
                            </table>
                        </asp:Panel>
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
                <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                    Text=""></asp:TextBox>
                <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                </asp:DropDownList>
            </td>
        </tr>
    </table>

       <cc1:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="lblTragetID3" PopupControlID="pnlPopupWindow3"
        OkControlID="btnOK2" BackgroundCssClass="modalBackground" BehaviorID="mpe3">
    </cc1:ModalPopupExtender>
    <asp:Label ID="lblTragetID3" runat="server"></asp:Label>
    <asp:Panel ID="pnlPopupWindow3" runat="server" CssClass="modalPopup" Style="display: none;  width: 900px; height: 400px;">
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage1" class="panel-heading">
                <td class="PageTitle panel-title" align="center">
                    <asp:Label ID="Label3" runat="server" Text="Show Lubricant Price History">
                    </asp:Label>
                </td>
            </tr>
            <tr id="TblControl2">
                <td style="width: 14%">
                    <div align="center" class="ContainTable">
                        <table style="background-color: #efefef;" width="100%">

                            <tr align="center">



                                <td style="width: 15%; float: right;">
                                    <asp:Button ID="btnok" runat="server" Text="OK" CssClass="btn btn-search btn-sm" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel11" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                        <div class="rounded_corners">
                            <asp:GridView ID="PartPriceGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                                CssClass=""
                                Width="100%" AllowPaging="true" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False"
                            
                                HeaderStyle-BackColor="#00FFFF" RowStyle-BackColor="" AlternatingRowStyle-BackColor="White"
                                RowStyle-ForeColor="#3A3A3A">

                                <RowStyle CssClass="GridViewRowStyle" />
                                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                <Columns>

                                    <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">                                        
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PartID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                        HeaderStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartID" runat="server" Text='<%# Eval("Part_ID") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPart_No" runat="server" Text='<%# Eval("Part_No") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part Name" ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartl_Name" runat="server" Text='<%# Eval("Part_name") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective From Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveFromDate" runat="server" Text='<%# Eval("EffectiveFromDate") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective To Date" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveToDate" runat="server" Text='<%# Eval("EffectiveToDate") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LIST" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("LIST","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MRP" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("MRP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NDP/DLP" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNDP" runat="server" Text='<%# Eval("NDP","{0:#0.00}") %>' CssClass="LabelLeftAlign" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
   
</asp:Content>
