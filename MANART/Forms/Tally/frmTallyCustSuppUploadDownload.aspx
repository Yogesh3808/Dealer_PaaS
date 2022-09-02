<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmTallyCustSuppUploadDownload.aspx.cs" Inherits="MANART.Forms.Tally.frmTallyCustSuppUploadDownload" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>
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

        function CheckBeforeUploadClick(objbutton, FileUploadID) {
            var ParentCtrlID;
            var objFileUpload;
            debugger;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
            objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
            var objtxtDealerCode;
            var objtxtType;
            objtxtDealerCode = document.getElementById("ContentPlaceHolder1_txtDealerCode");
            objtxtType = document.getElementById("ContentPlaceHolder1_txtType");
            var filename = objFileUpload.value;
            var sFileExtension = filename.split('.')[filename.split('.').length - 1].toLowerCase();
            var iFileSize = objFileUpload.size;
            var iConvert = (objFileUpload.size / 10485760).toFixed(2);
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search(objtxtDealerCode.value + '_' + objtxtType.value) == -1) {
                alert('File name is not in given format.');
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle" align="center" >
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title" ForeColor="White">
                </asp:Label>
            </td>
        </tr>        
        <tr id="TblControl">
            <td style="width: 15%; height: 92px;">
                <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                        ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Master Header Details"
                        ExpandedText="Master Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center"  width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Dealer Header Details" CssClass="panel-title" 
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
                        <table id="Table1" runat="server" width="100%">
                            <tr>
                                <td style="width: 15%; padding-left: 10px;" >
                                   <asp:Label ID="lblMasterType" runat="server" CssClass="LabelCenterAlign" Text="Master Type"> </asp:Label>           
                                </td>
                                <td style="width: 10%; padding-left: 10px;">
                                     <asp:DropDownList ID="DrpMasterType" Width="96%" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="DrpMasterType_SelectedIndexChanged" >
                                    <asp:ListItem Selected="True" Value="Customer" Text="Customer"></asp:ListItem>
                                    <asp:ListItem Value="Supplier" Text="Supplier"></asp:ListItem>                                    
                                    </asp:DropDownList>
                                </td>   
                                <td style="width: 30%; padding-left: 10px;">

                                </td>
                                <td style="width: 25%; padding-left: 10px;">
                                      <asp:Button ID="BtnRefresh" runat="server" CssClass="CommandButton" Text="Refresh Customer List" OnClick="BtnRefresh_Click" />
                                </td>   
                                <td style="width: 10%" >
                                       <asp:Button ID="btnDownload" runat="server" CssClass="CommandButton" Text="Download Customer List" OnClick="btnDownload_Click" />
                                </td>
                                <td style="width: 10%">
                                     
                                </td>                                                   
                            </tr>  
                             <tr>
                                <td style="width: 15%" >
                                   <br />
                                </td>
                                 <td style="width: 10%; padding-left: 10px;">
                                     </td>
                                <td style="width: 30%">
                                   <br />
                                </td>     
                                 <td style="width: 25%">
                                      <br />
                                </td>  
                                <td style="width: 10%" >
                                   <br />
                                </td>
                                <td style="width: 10%">
                                     <br />
                                </td>                                                   
                            </tr>         
                              <tr>
                            <td style="width: 15%; padding-left: 10px;">Select File For Upload
                            </td>
                            <td style="width: 55%; padding-left: 10px;" colspan="3" >
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="75%" CssClass="Cntrl1" />
                            </td>
                                 
                            <td style="width: 10%;">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-search btn-sm" Text="Upload"
                                    OnClientClick="return CheckBeforeUploadClick(this,'FileUpload1');" OnClick="btnUpload_Click" />
                                <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <img src="../../Images/Search_Progress.gif" alt="Searchig" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr id="trCust" runat="server" >
                            <td style="padding-left: 20px; color: Red" colspan="6">
                                <p>
                                    1.
                                    <a>Please select the file in the excel format. File name should be in format as 'DealerCode_Customer'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;DealerCode_Customer&#39;. </a>
                                    <br />
                                    2.
                                   <a>Not updated Customer Show in Red Color.</a>
                                </p>
                            </td>
                        </tr>
                        <tr id="trSupp" runat="server" >
                            <td style="padding-left: 20px; color: Red" colspan="6">
                                <p>
                                    1.
                                    <a>Please select the file in the excel format. File name should be in format as 'DealerCode_Supplier'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;DealerCode_Supplier&#39;. </a>
                                    <br />
                                    2.
                                   <a>Not updated Supplier Show in Red Color.</a>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="padding-left: 10px;">
                                <asp:Label ID="lblListPartNo" runat="server" Text="" Width="100%" ForeColor="Red"
                                    Visible="false"> </asp:Label>
                                <asp:TextBox ID="txtListPartNo" TextMode="MultiLine" runat="server" Text="" Width="100%"
                                    ForeColor="Red" Visible="false"></asp:TextBox>                               
                            </td>
                        </tr>      
                        </table>
                    </asp:Panel>                    
                </asp:Panel>
                
                <asp:Panel ID="PModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEModelDetails" runat="server" TargetControlID="CntModelDetails"
                        ExpandControlID="TtlModelDetails" CollapseControlID="TtlModelDetails" Collapsed="false"
                        ImageControlID="ImgTtlModel Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Customer List Details" ExpandedText="Customer List Details"
                        TextLabelID="lblTtlModelDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlModelDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlModelDetails" runat="server" Text="HO Branch Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<div align="center" class="containtable" style="height: 200px; overflow: auto">--%>
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                            AutoGenerateColumns="False" 
                           >
                            <%--OnRowDataBound="DetailsGrid_RowDataBound"--%>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>
                               
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Cust ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                            HeaderStyle-CssClass="HideControl">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCustID" runat="server" Text='<%# Eval("ID") %>' Width="1%"></asp:TextBox>
                                            </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Name" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DCAN Code" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("DCANCode") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tally Ledger Name" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCLedgerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("LedgerName") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                    <%-- </div>--%>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 15%">
            </td>
        </tr>
        <tr id="TmpControl" style="display:none">
            <td style="width: 15%">
                <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                    Text=""></asp:TextBox>
                <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>                
                <asp:TextBox ID="txtDealerCode" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>                
                <asp:TextBox ID="txtType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>                
            </td>
        </tr>
    </table>
    </div>
</asp:Content>